
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ImageQuantization;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Text;
using WebMeeting.Common;
using Win32;

namespace WebMeeting
{
	/// <summary>
	/// Summary description for AppSharing.
	/// </summary>
	/// 
	[Serializable]
	public enum DataType
	{
		SizeInformation,
		PictureBlock,
		MouseActivity,		
		DrawMouse,
		Scrolling,		
		KeyBoardActivity,
		PictureBlockRawData
	}	
	public delegate void OnDataAvilableDelegate(ref byte []buffer,DataType bDataType,ref int nX,ref int nY);
	public delegate void OnDataAvilableDelegateEx(ref byte []buffer,DataType bDataType,ref int nX,ref int nY, ref int width, ref int height,ref int length);
	public delegate void OnWindowClosedDelegate();
	public delegate void OnScrollPositionChanged(int nPosition,DataType dataType);
	
		

	public class AppSharing
	{
		
	
		

		#region For Handling Mouse Movements in APPSHARE
		Point lastMousePoint;
		Win32.USER32.POINT tempPosition;
		Win32.USER32.WINDOWPLACEMENT wp;/*for handling Mouse Movements*/
		#endregion

		#region For Modularising Capture Thread

		StringBuilder bt;
		StringBuilder bt2;
		//ImageConverter conv;
		int nWidthBlocks=1;
		int nHeightBlocks=1;
		int hBitmap=-1;
		int hdcDest_CaptureThread; 	
		byte[] buffer_CaptureThread;
		int nNumberOfBitsPerColor;
		int bufferSize_CaptureThead;			
		int blockWidth;/*contains the width of the shared window*/
		int blockHeight;/*contains the heigth of the shared window*/
	

		#endregion

		
		private int iPrev_HorXScroll = 0;
		private int iPrev_VerXScroll = 0;

		private int counter=0;

		private int height_Bitmap;
		private int width_Bitmap;

		public bool ShowPicOnBoard=false;
		
		



		
		const int widthBlockCount=4;
		const int heightBlockCount=12;
		bool m_bActive=true;
		Thread captureThread;
		//ArrayList hashArray=new ArrayList();
		//byte[] prevHash;
		System.IntPtr sharedWnd = IntPtr.Zero;
		int hdcSrc =0;
		bool bIsServer = false;
		public event OnWindowClosedDelegate OnWindowClosed;
		string oldString;
		public bool bIsDesktopSharing = false;
		static ImageConverter conv = new ImageConverter();
		public WebMeetingDrawingBoard.WhiteboardControl _whiteboard;

		//public DebugForm debugForm = new DebugForm();
		public event OnScrollPositionChanged OnScrolling;

		public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		private void FireOnScrolling(int nPos,DataType bData)
		{
			try
			{
				if(OnScrolling !=null)
				{
					OnScrolling(nPos,bData);
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private void FireOnScrolling(int nPos,DataType bData)",ex,"",false);
			}			
		}
		private void FireOnWindowClosed()
		{
			try
			{
				if(OnWindowClosed != null)
					OnWindowClosed();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private void FireOnWindowClosed()",ex,"",false);
			}	
		}
		public static Image ByteArrayToImage(byte[] buffer)
		{
			try
			{
				if(buffer!=null)
				{
					return  (Image) conv.ConvertFrom(buffer);	
				}
				else
					return null;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public static Image ByteArrayToImage(byte[] buffer)",ex,"",false);
			}	
		
			return  null ;//(Image) conv.ConvertFrom(buffer);	
		}

		/// 
		/// Converts a image to a byte array
		/// 
		/// the image which you want to convert
		/// 
		public static byte[] ImageToByteArray(Image image)
		{
			ImageConverter conv = new ImageConverter();
			try
			{
				return (byte[]) conv.ConvertTo(image, typeof(byte[]));
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public static byte[] ImageToByteArray(Image image)",ex,"",false);
			}	
			return null;
		}
		
		~AppSharing()
		{
			try
			{
				captureThread.Abort();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare ~AppSharing()",ex,"",false);
			}
		}
		public BITMAPINFO bitmap=new BITMAPINFO();
		public void AddDebugString(string szDebug)
		{
			//debugForm.listDebug.Items.Add(szDebug);
			//debugForm.listDebug.SelectedIndex = debugForm.listDebug.Items.Count-1;
			//Application.DoEvents();
		}

		public AppSharing(bool bIsServer)
		{			
			try
			{
				bitmap.bmiHeader.biSize = Marshal.SizeOf(bitmap);
				bitmap.bmiHeader.biPlanes = 1;
				bitmap.bmiHeader.biBitCount = 16;
				bitmap.bmiHeader.biCompression = Win32.USER32.BI_RGB;

				bIsServer = bIsServer;
				isWaitingPic = true;
				if(bIsServer)
				{
					//debugForm.Show();
				}
				/*
				string strFile = "C:\\app\\packetslog.txt";
				if(System.IO.File.Exists(strFile))
					System.IO.File.Delete(strFile);
				writer = new StreamWriter(strFile);*/
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public AppSharing(bool bIsServer)",ex,"",false);
			}

            
		}
		public void Terminate()
		{
			m_bActive = false;
			try
			{
				if(captureThread!= null)
				{
					captureThread.Abort();
					captureThread.Join();

				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void Terminate()",ex,"",false);
			}
			try
			{
				Win32.USER32.RECT rect = new Win32.USER32.RECT();
				if(sharedWnd != IntPtr.Zero)
				{
					Win32.USER32.GetWindowRect(sharedWnd, ref rect);
					//SetWindowPos((int)sharedWnd,(int)WindowPlacement.HWD_NOTOPMOST,rect.Left,rect.Top,rect.Right-rect.Left,rect.Bottom-rect.Top,0);            
					
					//					StringBuilder bt2 = new StringBuilder( oldString);
					//            		SetWindowText(sharedWnd,bt2);
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void Terminate()",ex,"",false);
			}
		}
		public event OnDataAvilableDelegate OnDataAvilable;
		public event OnDataAvilableDelegateEx OnDataAvilableEx;
		public void OnDataAvailable(ref byte[] buffer,DataType bDataType, ref int nX,ref int nY)
		{
			try
			{
				if(OnDataAvilable != null)
					OnDataAvilable(ref buffer,bDataType,ref nX,ref nY);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void OnDataAvailable(ref byte[] buffer,DataType bDataType, ref int nX,ref int nY)",ex,"",false);
			}	
		}
		public void OnDataAvailable(ref byte[] buffer,DataType bDataType, ref int nX,ref int nY, ref int width, ref int height,ref int length)
		{
			try
			{
				if(OnDataAvilableEx != null)
					OnDataAvilableEx(ref buffer,bDataType,ref nX,ref nY,ref width,ref height,ref length);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void OnDataAvailable(ref byte[] buffer,DataType bDataType, ref int nX,ref int nY, ref int width, ref int height,ref int length)",ex,"",false);
			}	
			

		}
		public void SetCaptureWindowAndStartCapturing(System.IntPtr hWnd)
		{
			try
			{
				sharedWnd = hWnd;
				if(bIsDesktopSharing)
					captureThread =new Thread(new ThreadStart(captureThreadFuncEx2));
				else
					captureThread =new Thread(new ThreadStart(captureThreadFuncEx));


				captureThread.Name = "Application and Desktop Sharing window capture thread";
				captureThread.Start();
				m_bActive = true;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void SetCaptureWindowAndStartCapturing(System.IntPtr hWnd)",ex,"",false);
			}	
			
		}
		
		public byte[] UncompressBuffer(ref byte[] buffer)
		{		
			MemoryStream Compressed = new MemoryStream(buffer);
			MemoryStream UnCompressed = new MemoryStream();
			
			try
			{	
				BZip2InputStream zisUncompressed = new BZip2InputStream(Compressed);
				int size;
				byte[] data = new byte[4096];
				do
				{
					size = zisUncompressed.Read(data, 0, data.Length);
					UnCompressed.Write(data, 0, size);
				}while (size > 0);			

				zisUncompressed.Close();
				Compressed.Close();
				byte []bUncompressedData = UnCompressed.GetBuffer();
				UnCompressed.Close();
				return bUncompressedData;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public byte[] UncompressBuffer(ref byte[] buffer)",ex,"",false);
			}	
			return null;
		}

		public byte[] UncompressBufferEx(ref byte[] buffer, int length)
		{		
			byte[] data = new byte[length];
			try
			{
				MemoryStream Compressed = new MemoryStream(buffer);
				GZipInputStream zisUncompressed = new GZipInputStream(Compressed);
				MemoryStream UnCompressed = new MemoryStream();
				int size;
				
				do
				{
					size = zisUncompressed.Read(data, 0, data.Length);
					UnCompressed.Write(data, 0, size);
				}while (size > 0 && UnCompressed.Length<length);			
				zisUncompressed.Close();
				Compressed.Close();
				data = UnCompressed.ToArray();//.GetBuffer();
				UnCompressed.Close();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public byte[] UncompressBufferEx(ref byte[] buffer, int length)",ex,"",false);
			}	

			
			return data;

		}

		//MemoryStream msCompressed = new MemoryStream();	
		//BZip2OutputStream zosCompressed = null;//new BZip2OutputStream(msCompressed,4096);

		
		public void SendMouseMove(DataType bDataType,ref int nX,ref int nY)
		{
		}
		public void CompressAndSendBuffer(ref byte []buffer,DataType bDataType,ref int nX ,ref int nY)
		{
			if(buffer != null)
			{
									
				try
				{
					MemoryStream msCompressed = new MemoryStream();				
					BZip2OutputStream zosCompressed = new BZip2OutputStream(msCompressed);
					zosCompressed.Write(buffer, 0, buffer.Length);
					zosCompressed.Close();					
					buffer =msCompressed.ToArray(); 										
				}
				catch(Exception ex)
				{					
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void CompressAndSendBuffer(ref byte []buffer,DataType bDataType,ref int nX ,ref int nY)",ex,"",false);
				}
			}
			try
			{
				OnDataAvailable(ref buffer,bDataType,ref nX,ref nY);		
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void CompressAndSendBuffer(ref byte []buffer,DataType bDataType,ref int nX ,ref int nY)",ex,"",false);
			}
		}


					
		
		public void CompressAndSendBuffer(ref byte []buff,DataType bDataType,ref int nX ,ref int nY, ref int width, ref int height,ref int length)
		{					
			byte[] buffer=null;
			if(buff != null)
			{
				try
				{
					//	Console.WriteLine("Uncompressed = " + buffer.Length.ToString());
					
					//MemoryStream msCompressed = new MemoryStream();		
					//ICSharpCode.SharpZipLib.Zip zosCOmp=new ICSharpCode.SharpZipLib.Zip(
								
					
					MemoryStream msComp = new MemoryStream();							
					GZipOutputStream zosComp=new GZipOutputStream(msComp);//
					zosComp.Write(buff, 0, buff.Length);
				
					zosComp.Finish();
					buffer =msComp.ToArray(); 							
					zosComp.Close();

				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void CompressAndSendBuffer(ref byte []buff,DataType bDataType,ref int nX ,ref int nY, ref int width, ref int height,ref int length)",ex,"",false);
				}

				//byte[] bb=UncompressBufferEx(ref buffer,length);
				//bb=bb;
			}
			
			
			/*
			if(buffer!=null)
			{
				writer.WriteLine(buffer.Length.ToString());
				writer.Flush();
			}
			
			*/

			try
			{
				OnDataAvailable(ref buffer,bDataType,ref nX,ref nY,ref width,ref height,ref length);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void CompressAndSendBuffer(ref byte []buff,DataType bDataType,ref int nX ,ref int nY, ref int width, ref int height,ref int length)",ex,"",false);
			}	
		}
		static ArrayList processList1 = new ArrayList();

		enum GetWindow_Cmd 
		{
			GW_HWNDFIRST = 0,
			GW_HWNDLAST = 1,
			GW_HWNDNEXT = 2,
			GW_HWNDPREV = 3,
			GW_OWNER = 4,
			GW_CHILD = 5,
			GW_ENABLEDPOPUP = 6
		}
		private static bool CaptureEnumWindowsProc(int handle, int param)
		{
			try
			{
				StringBuilder formDetails = new StringBuilder(256);
				int txtValue;
				string editText="";
				int dlgHandle = Win32.USER32.GetWindow( new IntPtr(handle),(int)GetWindow_Cmd.GW_CHILD);
				if(dlgHandle != 0)
				{
					txtValue =Win32.USER32.GetWindowText((IntPtr)handle ,formDetails,256);
					editText = formDetails.ToString().Trim();
					if(editText!="")
					{
						if(editText!="Start Menu" || editText!="Power Meter" || editText!="Program Manager")
						{
							processList1.Add(handle);
						}
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private static bool CaptureEnumWindowsProc(int handle, int param)",ex,"",false);
			}
			return true;
		}
		
		public static ArrayList GetAllTaskbarApps()
		{
			try
			{
				int hWnd;
				processList1.Clear();
				hWnd = Win32.USER32.FindWindow("Shell_TrayWnd", "");
				Win32.USER32.CallBack cewp = new Win32.USER32.CallBack(CaptureEnumWindowsProc);
				if(hWnd != 0)
					Win32.USER32.EnumWindows(cewp, hWnd);
				
				return processList1 ;
				
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public static ArrayList GetAllTaskbarApps()",ex,"",false);
			}	
			return null;
		}

		public static ArrayList GetAllProcesses()
		{
			try
			{
				Process []pAllProcesses = Process.GetProcesses();
				if(pAllProcesses != null)
				{
					ArrayList processList = new ArrayList();
					for(int i = 0; i < pAllProcesses.Length; i++)
					{
						if(pAllProcesses[i].MainWindowHandle != IntPtr.Zero)
						{
							//if(IsIconic(pAllProcesses[i].MainWindowHandle )) //kamran
						{
							processList.Add(pAllProcesses[i].MainWindowHandle.ToInt32());
							//processList.Add(pAllProcesses[i]);
						}
						}
					}
					return processList;
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public static ArrayList GetAllProcesses()",ex,"",false);
			}	


			return null;

		}


		public void SendSizeInformation (int nWidth, int nHeight)
		{	
			try
			{
				byte[] bTemp = null;
				CompressAndSendBuffer(ref bTemp,DataType.SizeInformation,ref nWidth,ref nHeight);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void SendSizeInformation (int nWidth, int nHeight)",ex,"",false);
			}	
			
		}
		
		public Win32.USER32.RECT rect = new Win32.USER32.RECT();
		public Win32.USER32.RECT rect2 = new Win32.USER32.RECT();
		public Win32.USER32.RECT temprect=new Win32.USER32.RECT();
		public Image lastFrameImage;
		int nCounter = 0;
		
		
		//		public Image retunLastFram()
		//		{
		//			
		//		}


		public Image GetServerFullBitmap()
		{
			Image image =null;
			try
			{
				if(!Win32.USER32.IsWindow(sharedWnd))
				{				
					return lastFrameImage; // by kamran
					//return image ;
				}			
				if(!isDesktopWindowInApp)
				{
					/*
					if( GetForegroundWindow()!= sharedWnd)
					{
						//lastFrameImage.Save(@"c:\SendNewImage.jpg",System.Drawing.Imaging.ImageFormat.Jpeg);
						if(lastFrameImage != null)
							return lastFrameImage; // by kamran
						//return image ;
					}
					*/
					
				}				
				//GetWindowPlacement(sharedWnd, ref wp);

				/*create compatible device context(dc) of source/shared window */
				int hdcDest =Win32.USER32.CreateCompatibleDC(hdcSrc);
				
				/*get window rectangle of shared window*/
				Win32.USER32.GetWindowRect(sharedWnd,ref rect2);
				int left_rect=rect2.left;
				int top_rect=rect2.top;
				int bitbltx=0;/* by default copy from x=0 coordinate*/
				int bitblty=0;/* by default copy from y=0 coordinate*/

				int width;
				int height;
				
				/*
				 * when we get rectangular coordinate of shared window using GetWindowRect
				 * left goes in negative if we move shared window until it goes outside screen at left side
				 * top goes in negative if we moe shared window until it goes outside  screen at top side
				 * right always goes in positive either we bring share window outside screen at right side
				 * bottom always goes in positive either we bring share window outside the screen at right side
				 */
				if(left_rect<0)/* left is minus if window is offscreen at left side*/
				{										
					width=rect2.right;
				}
				else
				{
					width=(rect2.right - rect2.left);
				}
				if(top_rect<0)/*top  is minus if window is offscreen at top side*/
				{
					height=rect2.bottom ;	
				}
				else
				{
					height=(rect2.bottom - rect2.top);
				}
								 
				int hBitmap = Win32.USER32.CreateCompatibleBitmap(hdcSrc,width,height);			
				//SendSizeInformation(width,height);
				this.width_Bitmap=width;
				this.height_Bitmap=height;

				//										(rect.Bottom - rect.Top));
				image =null;
				try
				{	
					//when you perform any operation on hdcDest then hBitmap will automatically changed
					Win32.USER32.SelectObject(hdcDest,hBitmap);				

					Graphics tempGraphics = Graphics.FromHdc((IntPtr)hdcDest);
					if(tempGraphics != null)
						tempGraphics.FillRectangle(Brushes.White,0,0,width,height);
					
					/**/
					if(left_rect<0)
						bitbltx=(-1*left_rect);
					if(top_rect<0)
						bitblty=(-1*top_rect);
					// Copies the Source hdcSrc device object in destination (hdcDest)
					Win32.USER32.BitBlt(hdcDest,0,0,width,height,
						hdcSrc,bitbltx,bitblty,
						0x00CC0020);
				
					image = Image.FromHbitmap((IntPtr)hBitmap);				
					/////////////////////////////////GC5				
					if(image.Width > 170 && image.Height > 40 )
					{
						lastFrameImage = image;		//GC5					
					}
					///////////////////////////
					//				lastFrameImage = image;		//GC5
					
					
					// This counter is useless 
					//
					nCounter++;
				}
				catch(Exception ex)
				{
					//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public Image GetServerFullBitmap()",ex,"",false);
				}
				finally
				{
					Win32.USER32.DeleteObject(hBitmap);
					Win32.USER32.DeleteDC(hdcDest);
				}						
			}
			catch(Exception ex)
			{
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public Image GetServerFullBitmap()",ex,"",false);
			}
			
			return image ;
		}
		
		
		public bool isDesktopWindowInApp =false;
	


		
		Win32.USER32.FLASHINFO f = new Win32.USER32.FLASHINFO();
		private void FlashWindow()
		{
			try
			{
				Win32.USER32.FlashWindow(sharedWnd,true ); // correct is true 2nd param
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private void FlashWindow()",ex,"",false);
			}	
		}
		
		
		public void SendAll()
		{       
			try
			{
				//send the size 	information
				
				

				SendSizeInformation(this.width_Bitmap,this.height_Bitmap);
				
				

				/////////////////////////////////////////////////////

				////////////
				//sendAppShare_Jpeg 
				

				////.////////////////////////////////////////////////

				///////
				
				//Send a complete 

				Image _img = null;
				
				while(_img == null)
				{
					_img = 	GetServerFullBitmap();				
					Thread.Sleep(200);
				}	
				byte []buffer = null;
				buffer=(byte[]) conv.ConvertTo(_img , typeof(byte[]));
				//	Bitmap _bmp = (Bitmap)_img;	
				
				int length=buffer.Length;
				int nStartX = 0;
				int nStartY = 0;
				//				int nWidth = _bmp.Width;
				//				int nHeight =_bmp.Height;
				//				buffer = new byte[_bmp.Width * _bmp.Height * 4];			
				//				int length=GetBitmapBits((IntPtr)_bmp.GetHbitmap() , buffer.Length, buffer);
				

				CompressAndSendBuffer(ref buffer,DataType.PictureBlock,ref nStartX,ref 	nStartY,ref  this.width_Bitmap,ref this.height_Bitmap,ref length);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void SendAll()",ex,"",false);					
			}	
										
			
		}

		public byte[] sendFirstImage()
		{
			byte []image = new byte[]{} ;
			try
			{
				FileStream stream = new FileStream(@"c:\test.jpg",FileMode.Open,FileAccess.Read);

				// Determine the size of the image
				int size = Convert.ToInt32(stream.Length);

				// Allocate the byte array
				image = new  byte[size];

				// Read the data into the byte array
				stream.Read(image, 0, size);

				// Close the stream
				stream.Close();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public byte[] sendFirstImage()",ex,"",false);	
			}
			return image ;
		}

	     
		public Image staticImage;
		public string sPrevWindowText = "";
		
		
		/* Modularize code of Capture Thread*/

		private void IntializeMouseCoordinates()
		{
			try
			{

				// get mouse coordinates
				lastMousePoint=new System.Drawing.Point(0,0);	
				tempPosition = new Win32.USER32.POINT();
				wp = new Win32.USER32.WINDOWPLACEMENT();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private void IntializeMouseCoordinates()",ex,"",false);	
			}

		}
		

		/*
		 * this function sends the dummy mouse movement message		 
		 */
		private void SendDummyMouseMovements()
		{

			//byte []bArray1 = null;
			try
			{
				// get cursor position
				//GetCursorPos(ref tempPosition);							
				tempPosition.x=lastMousePoint.X;
				tempPosition.y=lastMousePoint.Y;
				//tempPosition=(Point)new POINT(lastMousePoint.X,lastMousePoint.Y);
				/*
					 * GetWindowPlacement used to get the location of shared window on the screen
					 * its helps us to decide either the mouse x,y coordinate are in the range of
					 * window location or not.
					 * --to get more understanding of GetWindowPlacement check API's Guide.
					 */
	
				/*
						 * ScreenToClientEx is used to display cursor image at attendee side 
						 * with new mouse position relevant to shared window.										 
						 */
				ScreenToClientEx(ref tempPosition);
				byte []buffer1=null;
				CompressAndSendBuffer(ref buffer1,DataType.DrawMouse,ref tempPosition.x,ref tempPosition.y);
			}
			catch(Exception ex)	
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private void SendMouseMovements()",ex,"",false);	
			}

			//			try
			//			{
			//
			//				POINT  pt=new POINT();
			//				pt.x=0;
			//				pt.y=0;
			//			
			//				byte []buffer1=null;
			//				CompressAndSendBuffer(ref buffer1,DataType.DrawMouse,ref pt.x,ref pt.y);				
			//			}
			//			catch(Exception ex)
			//			{
			//				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private void SendDummyMouseMovements()",ex,"",false);	
			//			}
		}
		


		
		/*
		 * this function check last and current coordinate then check 
		 *	the window coordinate
		 */
		private void SendMouseMovements()
		{
			//byte []bArray1 = null;
			try
			{
				// get cursor position
				Win32.USER32.GetCursorPos(ref tempPosition);							
				// check if there is a different in mouse position
				if(tempPosition.x!=lastMousePoint.X 
					|| tempPosition.y!=lastMousePoint.Y	)
				{
					lastMousePoint.X=tempPosition.x;
					lastMousePoint.Y=tempPosition.y;										
					/*
					 * GetWindowPlacement used to get the location of shared window on the screen
					 * its helps us to decide either the mouse x,y coordinate are in the range of
					 * window location or not.
					 * --to get more understanding of GetWindowPlacement check Msdn
					 */
				
					Win32.USER32.GetWindowRect(sharedWnd,ref temprect);
					Win32.USER32.GetWindowPlacement(sharedWnd, ref wp);
					//				Trace.WriteLine("Left ::: " + wp.rcNormalPosition.Left.ToString());
					//				Trace.WriteLine("Top ::: " + wp.rcNormalPosition.Top.ToString());	
					//				Trace.WriteLine("Right ::: "+wp.rcNormalPosition.Right.ToString());
					//				Trace.WriteLine("Bottom ::: "+wp.rcNormalPosition.Bottom.ToString());
					//				
					//				Trace.WriteLine("Window Rect Left ::: "+temprect.Left.ToString());
					//				Trace.WriteLine("Window Rect Top ::: "+temprect.Top.ToString());
					//				Trace.WriteLine("Window Rect Right ::: "+temprect.Right.ToString());
					//				Trace.WriteLine("Window Rect Bottom ::: "+temprect.Bottom.ToString());


					/*
					 * only send mouse movements when mouse is in the the area of shared
					 * window. 
					 */
					if((tempPosition.x>=wp.rcNormalPosition.Left && tempPosition.x<=wp.rcNormalPosition.Right) && (tempPosition.y>=wp.rcNormalPosition.Top && tempPosition.y<=wp.rcNormalPosition.Bottom))
					{																													
						/*
						 * ScreenToClientEx is used to display cursor image at attendee side 
						 * with new mouse position relevant to shared window.										 
						 */
						//	Trace.WriteLine(tempPosition.x.ToString() + " ::: " + tempPosition.y.ToString());
						ScreenToClientEx(ref tempPosition);
						//Trace.WriteLine(tempPosition.x.ToString() + " ::: " + tempPosition.y.ToString());
						//this.SendMouseMove(DataType.DrawMouse,ref tempPosition.x,ref tempPosition.y);
						byte []buffer1=null;
						CompressAndSendBuffer(ref buffer1,DataType.DrawMouse,ref tempPosition.x,ref tempPosition.y);
					
						//OnDataAvailable(ref buffer1,DataType.DrawMouse,ref tempPosition.x,ref tempPosition.y);

						//Trace.WriteLine("Mouse Movements is sent");
						//CompressAndSendBuffer(ref bArray1,DataType.DrawMouse,ref tempPosition.x,ref tempPosition.y);
					}																	
					//if(tempPosition.y>=wp.rcNormalPosition.Top && tempPosition.y<=wp.rcNormalPosition.Bottom)									
					// send mouse coods																		
				}
			}
			catch(Exception ex)	
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private void SendMouseMovements()",ex,"",false);	
			}

				
		}
		

		/*	Tasks, this method will do
		 *  1...Get shared windows rectangle ..The GetWindowRect function retrieves the dimensions of the bounding rectangle of the specified window
		 *	2...set window position ...The SetWindowPos function changes the size, position, and Z order of a child, pop-up, or top-level window. Child, pop-up, and top-level windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
		 *	3...get window title text....The GetWindowText function copies the text of the specified window’s title bar (if it has one) into a buffer. If the specified window is a control, the text of the control is copied.
		 *	4...assign it as old string
		 *	5...create new text for displaying on the title bar
		 *	6...set new created text to window title...The SetWindowText function changes the text of the specified window’s title bar (if it has one). If the specified window is a control, the text of the control is changed.	
		 *  7...make shared window as active...The SetActiveWindow function activates a window. The window must be attached to the calling thread's message queue.
		 *  8...focus share window for mouse activation...The SetForegroundWindow function puts the thread that created the specified window into the foreground and activates the window. Keyboard input is directed to the window, and various visual cues are changed for the user.
		 *
		 */
		public void SetWindowStuff()
		{
			try
			{
				Win32.USER32.GetWindowRect(sharedWnd, ref rect);
				Win32.USER32.SetWindowPos((int)sharedWnd,
					(int)Win32.USER32.WindowPlacement.HWD_NOTOPMOST,
					rect.left,rect.top,rect.right-rect.left,
					rect.bottom-rect.top,0);            
				//	bt = new StringBuilder(1024);
				//GetWindowText(sharedWnd,bt,1024);			
				//oldString = bt.ToString();
				//bt2 = new StringBuilder(  "Shared Window - Press F4 to Application Share & Press F3 to return Annotation Mode " + bt.ToString());            
				//SetWindowText(sharedWnd,bt2);
				Win32.USER32.SetActiveWindow (sharedWnd);		
				new Thread(new ThreadStart(Enum_Child)).Start();
				Win32.USER32.SetForegroundWindow(sharedWnd);

			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void SetWindowStuff()",ex,"",false);	
			}
		}







		//********************************************************************************************************88//

		/// <summary>
		/// ///////////////////////////////////
		/// 
		/// </summary>
		
		public void Enum_Child()
		{
			while (true)
			{
				try
				{
					Thread.Sleep(10000);
					if(sharedWnd!=Win32.USER32.GetLastActivePopup(sharedWnd))
					{
						
						Win32.USER32.GetWindowText(Win32.USER32.GetLastActivePopup(sharedWnd),b,1024);
						
					}
	
					//this.IEFromhWnd(GetDesktopWindow());
				}
				catch(Exception Exp)
				{}
			
			}
		}



		/// <summary>
		/// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// </summary>
		/// ////////////////////////////////////////////////////////////////////////////////////////////////////// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// /// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// /// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// /// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// 

	
		private Win32.USER32.Win32WndProc newWndProc = null;		
		private delegate int Win32WndProc(IntPtr hWnd, int Msg, int wParam, int lParam);
		private IntPtr GhWnd;

		/*
		GWL_WNDPROC         (-4)
		#define GWL_HINSTANCE       (-6)
		#define GWL_HWNDPARENT      (-8)
		#define GWL_STYLE           (-16)
		#define GWL_EXSTYLE         (-20)
		#define GWL_USERDATA        (-21)
		#define GWL_ID              (-12)
		*/


		public IntPtr oldWndProc = IntPtr.Zero;	
		/// <summary>
		/// App shre fixing ............
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="browser"></param>
		public void StartSubclass(IntPtr hWnd)
		{
			
			// delegate for the new wndproc
			newWndProc = new Win32.USER32.Win32WndProc(MyWndProc);
				
			if(oldWndProc.ToInt32() != 0)
				Win32.USER32.SetWindowLong(hWnd,Win32.USER32.GWL_WNDPROC, (Win32.USER32.Win32WndProc)newWndProc);
			else
				oldWndProc = Win32.USER32.SetWindowLong(hWnd, Win32.USER32.GWL_WNDPROC, newWndProc);
			//oldWndProc = SetWindowLong(hWnd,-12, newWndProc);
				
			//MessageBox.Show(SetWindowLong(hWnd,-12, newWndProc).ToString());
			
		}

		/// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// /// ///////////////////////////////////////////////////////////////////////////////////////////////////
		
		StringBuilder b=new StringBuilder(1024);
		
		int temp_count=0;
		
		
		
		private Int32 EnumChild(IntPtr hWnd, IntPtr lParam)
		{
			try
			{
				
				//if(sharedWnd==GetAncestor(hWnd,GA_ROOTOWNER)&& sharedWnd==GetParent(hWnd))
				
			{
				Win32.USER32.GetWindowText(hWnd,b,1024);
				temp_count++;

			}


				if(IsIEServerWindow(hWnd))
				{
					GhWnd = hWnd;
					return 0;
				}
				else
					return 1;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private Int32 EnumChild(IntPtr hWnd, IntPtr lParam)",ex,"",false);
				return 1;
			}

		}
		/// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// /// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// 

		private bool IsIEServerWindow(IntPtr hWnd)
		{
			Int32 Res;
			StringBuilder ClassName = new StringBuilder(256);

			// Get the window class name
			Res = Win32.USER32.GetClassName(hWnd.ToInt32(), ClassName, ClassName.MaxCapacity);
			if(ClassName.ToString() == "DDE Server Window")
				return true;
			else
				return false;
		}

		



		public IntPtr IEFromhWnd(System.IntPtr hWnd)
		{

			
			try
			{
				if(!hWnd.Equals(0))
				{
					if(!IsIEServerWindow(hWnd))
					{
						// Get 1st child IE server window
						// Zaeem 
						temp_count=0;
						
						
						Win32.USER32.EnumChildProc myEnumChildProc = new Win32.USER32.EnumChildProc(EnumChild);
						Win32.USER32.EnumChildWindows(hWnd.ToInt32(), myEnumChildProc, hWnd.ToInt32());
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public IntPtr IEFromhWnd(IntPtr hWnd)",ex,"",false);
			}
			return GhWnd;

		}





		/// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// /// ///////////////////////////////////////////////////////////////////////////////////////////////////
		
		/// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// /// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// 
		private int MyWndProc(IntPtr hWnd, int Msg, int wParam, int lParam)
		{
			try
			{
				switch(Msg)
				{					
					case Win32.USER32.WM_PARENTNOTIFY:
							
						if(Win32.USER32.WM_CREATE==wParam)
						{
								
						}
						break;
					default:							
						break;
				}
			}
			catch(Exception ee)
			{
				ee = ee;

					
			}				
			
			return Win32.USER32.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
		}


		/// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// /// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// /// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// /// ///////////////////////////////////////////////////////////////////////////////////////////////////
		/// /// ///////////////////////////////////////////////////////////////////////////////////////////////////


		//********************************************************************************************************88//





		/*	Tasks, this method will do
		 *  1...work on shared windows rectangle and get its width,height
		 *	2...get window dc...The GetWindowDC function retrieves the device context (DC) for the entire window, including title bar, menus, and scroll bars. A window device context permits painting anywhere in a window, because the origin of the device context is the upper-left corner of the window instead of the client area.
		 *	3...create memory dc for destination....The CreateCompatibleDC function creates a memory device context (DC) compatible with the specified device.
		 *	4...create bitmap on width & height using device context of sharedwindow
		 *	5...select bitmap object into destination dc for use furture...The SelectObject function selects an object into the specified device context. The new object replaces the previous object of the same type.				
		 */
		public void CreateBitmapStructureForSharedWindow()
		{
			try
			{

				blockWidth = (rect.right - rect.left);
				blockHeight = (rect.bottom - rect.top);

				hdcSrc = (int)Win32.USER32.GetWindowDC((int)sharedWnd);
					
				hdcDest_CaptureThread =Win32.USER32.CreateCompatibleDC(hdcSrc);

				// create window bitmap
				hBitmap = Win32.USER32.CreateCompatibleBitmap(hdcSrc,
					blockWidth,blockHeight);			
					
				Win32.USER32.SelectObject(hdcDest_CaptureThread,hBitmap);					
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void CreateBitmapStructureForSharedWindow()",ex,"",false);	
			}
		}


		/*	Tasks, this method will do
		 *  1...get no-of-bits takes a color...The GetDeviceCaps function retrieves device-specific information about a specified device.
		 *	2...calculate buffer size based on the no_of_bits a color will take
		 *	3...allocate buffer for image
		 */
		public void Initialize_buffer()
		{
			try
			{
				nNumberOfBitsPerColor= Win32.USER32.GetDeviceCaps(hdcSrc,Win32.USER32.BITSPIXEL);
				/*
					 * calculate buffer size
					 * calculate buffer size in bytes caz we dividing whole expression with 8
					 */
				bufferSize_CaptureThead=(blockWidth*blockHeight*nNumberOfBitsPerColor)/8;								
				// initialize buffer
				buffer_CaptureThread = new byte[bufferSize_CaptureThead];		
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void Initialize_buffer()",ex,"",false);	
			}
		}
		/// <summary>
		/// /* Tasks, this method will do
		/// *  1...check window exist...The IsWindow function determines whether the specified window handle identifies an existing window.
		///*	2...if window not exist then call method that will stop application sharing process and refresh process list
		/// *	3...return true if sharewnd found
		///*  4...return false if sharewnd not found
		/// */
		/// </summary>
		/// <returns></returns>
		/// 

		public bool bCloseAppshare()
		{
			try
			{
				
				if(!Win32.USER32.IsWindow(sharedWnd))
				{
					FireOnWindowClosed();						
					return false;
				}
				else
					return true;			
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public bool bCloseAppshare()",ex,"",false);
				return true;
			}
		}

		/* Tasks, this method will do
		 *  1...its not dektop sharing
		 *	2...check working window not equral sharewindow.....
		 * The GetForegroundWindow function returns the handle of the foreground window (the window with which the user is currently working).
		 *  The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
		 *	3...if not equal means its minimized then flashes shared window
		 *  4...return true if we flush window
		 *  5...return false working window and shared window is same.
		 */
		public bool bFlashWindow()
		{
			try
			{
				
				// on returning true image is not send 
				// Image is not send when the shared window is not at front
				// but it might be possible that its child is on front 
				// If the child is on front then the image of that area shd be send 
				

				if( Win32.USER32.GetForegroundWindow()!= sharedWnd) // if foreground window is not shared window
				{
						
						
					int for_ID=0, shd_ID=0;	
					Win32.USER32.GetWindowThreadProcessId(Win32.USER32.GetForegroundWindow(),out for_ID);
					Win32.USER32.GetWindowThreadProcessId(sharedWnd,out shd_ID);
					if (for_ID==shd_ID)
					{
						return false;
												
					}
					else
					{
						return true;
					}
						
				}
					
			
			}
			catch(Exception ex)
			{	
				
				//MessageBox.Show(ex.StackTrace.ToString());
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public bool bFlashWindow()",ex,"",false);
			}
			return false;
		}

		/* Tasks, this method will do
		 *  1...get window text...The GetWindowText function copies the text of the specified window’s title bar (if it has one) into a buffer. If the specified window is a control, the text of the control is copied.
		 *	2...set it to temp 		 
		 *	3...if its changeed then keep its in old string for setting again
		 */
		public void CheckShareWindowTitle()
		{
			try
			{
				// get shared window text
				Win32.USER32.GetWindowText(sharedWnd,bt,1024);
				string _tempString = bt.ToString();
				// see if it has shared window
				if(_tempString.IndexOf("Shared Window - Press F4 to Application Share & Press F3 to return Annotation Mode ") != 0)
				{
					oldString =_tempString ;
					bt2 = new StringBuilder(  "Shared Window - Press F4 to Application Share & Press F3 to return Annotation Mode " + bt.ToString());            
					// set window text again:S
					Win32.USER32.SetWindowText(sharedWnd,bt2);
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void CheckShareWindowTitle()",ex,"",false);
			}
		}

		/*run in the case of the App sharing*/
		public void captureThreadFuncEx()
		{
			try
			{
				int nBlocks=nHeightBlocks *nWidthBlocks;				
				/*
				 * Intialize Mouse Relevant Stuff 
				 */
				IntializeMouseCoordinates();
				/*s
				 * set windows relevant stuff,like get its rectangle,its title,activate it etc..
				 * for more detail go to method header
				 */
				SetWindowStuff();								
				/*
				 * initialize imageconverter object
				 * this will be used to convert image into byte array
				 */
				conv=new ImageConverter();
				/*
				 * create window bitmap sturcture on the shared window architecture
				 */
				CreateBitmapStructureForSharedWindow();
				/*
				 * initialize the buffer
				 */	
				Initialize_buffer();
				/*
				 * send picture size
				 */
				SendSizeInformation(blockWidth,blockHeight);				

				// variables to contain cursor position
				Win32.USER32.POINT cursorPosition = new Win32.USER32.POINT();

				Win32.USER32.RECT temprect = new Win32.USER32.RECT();
			
				// screen width and height,
				// these are used later to track whether the window has gone out of the screen or not
				// blocks that are out of screen are not painted or sent
				int _nScreenWidth = Screen.PrimaryScreen.Bounds.Width;
				int _nScreenHeight = Screen.PrimaryScreen.Bounds.Height;

				//int nTemp =0;
			
				// start coods
				int nStartX = 0,nStartY = 0;

				// current block width and height
				//int nCurrentBlockWidth = 0,nCurrentBlockHeight=0;

				// hash of the current block
				//byte[] hash = null;

				//int nQuantizeCount=10; // set the quanization count as 10 initially

				bt = new StringBuilder(1024);	

				byte[] b;
			
			
				//PitchBlack = 0xffffff

				// create brush
				int _hBrush = Win32.USER32.CreateSolidBrush(0xffffff);


				Win32.USER32.RECT _tempRect = new Win32.USER32.RECT();

				//				int nCopyX,  nCopyY;
		
				// this is not used anywhere so we will comment this coz this isnt used anywhere
				//int hBitmap_dummy = CreateCompatibleBitmap(hdcSrc,blockWidth,blockHeight);			
				
				//			int nBlockNo = 0;

				OctreeQuantizer quantizer = new OctreeQuantizer(255,8);

				// When u close App share it becomes  false 
				// else it remain true uptill the App share is true 

				

				while(m_bActive) // send data only if the window is active
				{
					try
					{												
						/*
						 * check window exist if not found then close app share process
						 */
						if(!bCloseAppshare())
						{
							break;
						}
						
						/*
						 * check working window is not shared  then flush and continue
						 */


						//Zaeem Addition 

						// In previous case thte Sleep was not added here , so that 
						//when it reaches the continue statement, it will skip all the next statement including the 
						// for loop , so that no Sleep in the way , which makes the processing very high 
						// Yet an other Stupid piece of code 			
						// Another Check which zaeem is Adding is if the current tab is Appsharing then the image shd be send and same for receiving

						Thread.Sleep(2000);
						if(bFlashWindow())
						{
							continue;
						}
				
						quantizer =new OctreeQuantizer (255,8);

						


						// get server full bitmap
						if(staticImage!=null)
						{
							staticImage.Dispose();
							staticImage=null;
						}
						staticImage= GetServerFullBitmap();
						/*
						 * this function check last and current coordinate then check 
						 * the window coordinate
						 */

						SendMouseMovements();
						
						// If window is inactive then we flush it 
						//						
						//						if(bFlashWindow())
						//						{
						//							continue;
						//							/*
						//									again return to while loop skip all next instruction in a while
						//									block.
						//								*/								
						//						}								

						int length2;//=buffer_CaptureThread.Length;
						Image _quantized;
						if(this.staticImage!=null)
						{
							_quantized = quantizer.Quantize(staticImage);
						}
						else
							continue;

                                    
						// convert quantized array to byte array
						b=null;
						b=(byte[]) conv.ConvertTo(_quantized, typeof(byte[]));
						
						length2 = b.Length;

						// send image
						// note that image was added in the hash array list and 
						// this data is sent, 
						CompressAndSendBuffer(ref b,DataType.PictureBlock,
							ref nStartX,ref nStartY,ref this.width_Bitmap,
							ref this.height_Bitmap,ref length2);																					
						SendDummyMouseMovements();
						_quantized.Dispose();
				
						Thread.Sleep(2000);					
					}
					catch(Exception ex)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void captureThreadFuncEx()",ex,"",false);
					}
				}

			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void captureThreadFuncEx()",ex,"",false);
			}
		}


		public void captureThreadFuncEx2() // it is used only in desktop sharing (remote control)
		{
			try
			{
				//int nWidthBlocks=1;
				//int nHeightBlocks = 1;
				//int nBlocks=nHeightBlocks *nWidthBlocks;

				#region  Set window stuff
				// set window stuff
				Win32.USER32.GetWindowRect(sharedWnd, ref rect);

				//SetWindowPos((int)sharedWnd, (int)WindowPlacement.HWD_TOPMOST,rect.Left,rect.Top,
				//					rect.Right-rect.Left, rect.Bottom-rect.Top,0);            
				
				//ShowWindow((IntPtr)sharedWnd, SW_SHOWMAXIMIZED);// new 5 dec
				

				//SetActiveWindow(sharedWnd);

				//SetForegroundWindow(sharedWnd);
				#endregion

				ImageConverter conv = new ImageConverter();

				#region Create screen bitmap

				// get window dimensions
				blockWidth = (rect.right - rect.left);//nWidthBlocks;
				blockHeight = (rect.bottom - rect.top);//nHeightBlocks;

				hdcSrc = (int)Win32.USER32.GetWindowDC((int)sharedWnd);
				int hdcDest =Win32.USER32.CreateCompatibleDC(hdcSrc);

				// create window bitmap
				int hBitmap = Win32.USER32.CreateCompatibleBitmap(hdcSrc,
					blockWidth,blockHeight);			
				Win32.USER32.SelectObject(hdcDest,hBitmap);


			
				#endregion

				#region Different initializations
				// get number of adjacent color bits for each pixel
				// in this we will get 4
				int nNumber = Win32.USER32.GetDeviceCaps(hdcSrc,Win32.USER32.BITSPIXEL);

				// calculate buffer size
				int bufferSize= blockWidth*blockHeight*nNumber / 8;

				// initialize buffer
				byte[] buffer = new byte[bufferSize];

				// initialiaze current block number
				//int nBlockNo = 0;

				// clear the hash array.
				//hashArray.Clear();

				// previous hash: this will contain, hash of previously captured blocks
				byte[] prevHash = new byte[blockWidth*blockHeight];

				// b containts a block data
				byte[] blockData=new byte[blockWidth*blockHeight]; 
				#endregion

				#region STEP1 Send Screen SIZE
				// send window size information
				SendSizeInformation((rect.right - rect.left),
					(rect.bottom - rect.top));
				#endregion

				// variables to contain cursor position
				Win32.USER32.POINT cursorPosition = new Win32.USER32.POINT();

				Win32.USER32.POINT tempPosition = new Win32.USER32.POINT();
				Win32.USER32.RECT temprect = new Win32.USER32.RECT();
			
				// screen width and height,
				// these are used later to track whether the window has gone out of the screen or not
				// blocks that are out of screen are not painted or sent
				int _nScreenWidth = Screen.PrimaryScreen.Bounds.Width;
				int _nScreenHeight = Screen.PrimaryScreen.Bounds.Height;

				//int nTemp =0;
			
				// start coods
				int nStartX = 0,nStartY = 0;

				// current block width and height
				int nCurrentBlockWidth = blockWidth ,nCurrentBlockHeight=blockHeight;

				// hash of the current block
				//	byte[] hash = null;

				int nQuantizeCount=10; // set the quanization count as 10 initially

				//				bt = new StringBuilder(1024);	

				//byte[] b;
			
				// get mouse coordinates
				Point lastMousePoint=new System.Drawing.Point(0,0);	
			
				// create brush
				int _hBrush = Win32.USER32.CreateSolidBrush(0xffffff);

				Win32.USER32.RECT _tempRect = new Win32.USER32.RECT();

				//int nCopyX,  nCopyY;
		
				// this is not used anywhere so we will comment this coz this isnt used anywhere
				//int hBitmap_dummy = CreateCompatibleBitmap(hdcSrc,blockWidth,blockHeight);			

				//bool bDifferenceFound = false;

				OctreeQuantizer quantizer = new OctreeQuantizer(255,8);

				while(m_bActive) // send data only if the window is active
				{
					try
					{						
						// whats quantizer count
						if(nQuantizeCount > 5) // this will be true initially as the quantization count's been set to 10
						{
							nQuantizeCount=0; // reinitialize quantizer count
							quantizer = null;
							quantizer =new OctreeQuantizer (255,8);
						}

						nQuantizeCount++; // increment quantize count

						/*
						// get window rect again
						GetWindowRect(sharedWnd, ref temprect);
						int tempX= GetScrollPos(sharedWnd, SB_HORZ);
						int tempY= GetScrollPos(sharedWnd, SB_VERT);
					*/
						#region check if the window has been resized.
						/*if(((temprect.Right - temprect.Left) != (rect.Right -rect.Left)) ||
							((temprect.Bottom - temprect.Top) != (rect.Bottom - rect.Top)) || 
							(sPrevWindowText != oldString ) || (iPrev_HorXScroll != tempX) || 
							(iPrev_VerXScroll != tempY))
						{
						#region WINDOW_RESIZED
							//If the window has been resized
							bool bRepeatIT = true;					

							while((bRepeatIT) && (m_bActive))
							{
								Trace.WriteLine("catptureThread: quantizer while((bRepeatIT) && (m_bActive)) ");
								try
								{
									rect = temprect;
									sPrevWindowText = oldString;
									iPrev_HorXScroll = tempX;
									iPrev_VerXScroll = tempY;


									blockWidth = (rect.Right - rect.Left)/nWidthBlocks;
									blockHeight = (rect.Bottom - rect.Top)/nHeightBlocks;

									hBitmap = CreateCompatibleBitmap(hdcSrc,blockWidth,blockHeight);			
									SelectObject(hdcDest,hBitmap);	

									// not used, not required
									//hBitmap_dummy = CreateCompatibleBitmap(hdcSrc,blockWidth,blockHeight);	

									bufferSize=blockWidth*blockHeight*nNumber/8;//4;
									buffer = new byte[bufferSize];
								
									nBlockNo = 0;
									hashArray.Clear();		
									b = null;
								
									b=new byte[blockWidth*blockHeight]; // reinitialize block data

									// send size information
									SendSizeInformation((rect.Right - rect.Left),
										(rect.Bottom - rect.Top));
									bRepeatIT = false;
							
								}
								catch(Exception )
								{

									bRepeatIT = true;

								}
							}
						#endregion
						}
						else if((temprect.Left != rect.Left) || (temprect.Top != rect.Top)  )
						{
						#region Window_Moved
							//if the window has been moved
							rect = temprect;

						#endregion
					
						}*/
						#endregion 

						//nBlockNo = 0;

						//for(int nRow = 0 ; nRow < nHeightBlocks ; nRow++)
					{
						// for each block
						//for(int nColumn=0; nColumn< nWidthBlocks; nColumn++,nBlockNo++)
					{									
						try
						{	
							#region divided into blocks
							/*
									// get start coods
									nStartX = nColumn * blockWidth;
									nStartY = nRow * blockHeight;
						
									// get block width
									nCurrentBlockWidth = blockWidth;
									nCurrentBlockHeight = blockHeight;
								
									//Tricking missing pisxels.
									// check if its the last column
									if(nColumn == (nWidthBlocks -1))
									{
										// if its the last column, append the remaining
										// bits with the rest of the stuff
										nTemp = nStartX + blockWidth;
										if(nTemp < (rect.Right - rect.Left))
											nCurrentBlockWidth += (rect.Right - rect.Left) - nTemp;								

									}

									// if it's the last row
									if(nRow == (nHeightBlocks-1))
									{
										// increate the height
										nTemp = nStartY + blockHeight;
									
									}
									*/
							#endregion
							#region BUILD_BITMAP
							// build the bitmap

							Win32.USER32.SelectObject(hdcDest,hBitmap);										
							/*
									 * nCopyX = nCurrentBlockWidth;
									nCopyY = nCurrentBlockHeight;		
					            
									_tempRect.Left = 0;
									_tempRect.Top = 0;
									_tempRect.Right = nCurrentBlockWidth;
									_tempRect.Bottom = nCurrentBlockHeight;
									//								int startX=0;
									//								int startY=0;
									int endX=nCurrentBlockWidth;
									int endY=nCurrentBlockHeight;

									if(nCurrentBlockWidth + nStartX + temprect.Left > _nScreenWidth) 
									{
										FillRect((IntPtr)hdcDest,ref _tempRect,(IntPtr)_hBrush);									
										endX=(_nScreenWidth - nStartX- temprect.Left) % nCurrentBlockWidth;
										if(endX==0)
											endX=nCurrentBlockWidth;
									}
									else if(nStartX + temprect.Left < 0)
									{
										FillRect((IntPtr)hdcDest,ref _tempRect,(IntPtr)_hBrush);															
										//startX=tempRect.Left;
									}	
									else if(nCurrentBlockHeight + nStartY + temprect.Top > _nScreenHeight)
									{
										FillRect((IntPtr)hdcDest,ref _tempRect,(IntPtr)_hBrush);
										endY=(_nScreenHeight - nStartY- temprect.Top) % nCurrentBlockHeight;
										if(endY==0)
											endY=nCurrentBlockWidth;
									}
									else if(nStartY + temprect.Top < 0)
									{
										FillRect((IntPtr)hdcDest,ref _tempRect,(IntPtr)_hBrush);									
									}
									*/									
							/*BitBlt(hdcDest,0,0,endX,endY,
										hdcSrc,	nStartX,nStartY,
										0x00CC0020);
										*/
							Win32.USER32.BitBlt(hdcDest,0,0,blockWidth,blockHeight,
								hdcSrc,	0,0,
								0x00CC0020);

							//GetBitmapBits((IntPtr)hBitmap,buffer.Length,buffer);
									
							Image _image = Image.FromHbitmap((IntPtr)hBitmap);
							using( Image _quantized = quantizer.Quantize(_image))
							{
								buffer =(byte[]) conv.ConvertTo(_quantized, typeof(byte[]));
								//buffer = (byte[]) conv.ConvertTo(_image , typeof(byte[]));
							}
									
							_image.Dispose();


							#endregion									
							#region hashing method
							//Compute a hash for each image
							SHA256Managed shaM = new SHA256Managed();
							byte[] hash1 = shaM.ComputeHash(buffer);
							byte[] hash2 = shaM.ComputeHash(prevHash );
									
							//Compare the hash values
							for (int i = 0; i < hash1.Length && i < hash2.Length  ; i++)
							{
								if (hash1[i] != hash2[i])  
								{								// image has changed
									Console.WriteLine(" new image has sent using hash  :  " + DateTime.Now.ToString()+ " " +DateTime.Now.Millisecond.ToString());
									int length2=buffer.Length;
											
									/*Image _image = Image.FromHbitmap((IntPtr)hBitmap);
											Image _quantized = quantizer.Quantize(_image);
											_image.Dispose();
                                    
											// convert quantized array to byte array
											b=(byte[]) conv.ConvertTo(_quantized, typeof(byte[]));																		
											length2 = b.Length;
											*/
									// send image
									// note that image was added in the hash array list and 
									// this data is sent, 
									CompressAndSendBuffer(ref buffer,DataType.PictureBlock,
										ref nStartX,ref nStartY,ref nCurrentBlockWidth,
										ref nCurrentBlockHeight, ref length2);									
									//_quantized.Dispose();	
									prevHash = (byte[])buffer.Clone();
									break;
								}
							}
									

							#endregion
									
							/*int length2=buffer.Length;
									Image _image = Image.FromHbitmap((IntPtr)hBitmap);
									Image _quantized = quantizer.Quantize(_image);
									_image.Dispose();
                                    
									// convert quantized array to byte array
									b=(byte[]) conv.ConvertTo(_quantized, typeof(byte[]));																		
									length2 = b.Length;

									// send image
									// note that image was added in the hash array list and 
									// this data is sent, 
									CompressAndSendBuffer(ref b,DataType.PictureBlock,
										ref nStartX,ref nStartY,ref nCurrentBlockWidth,
										ref nCurrentBlockHeight,ref length2);									

									_quantized.Dispose();*/
									
						}
						catch(Exception ex)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void captureThreadFuncEx2()",ex,"",false);
						}
					}
						Thread.Sleep(500);
					}

					
					}
					catch(Exception ex)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void captureThreadFuncEx2()",ex,"",false);
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void captureThreadFuncEx2()",ex,"",false);
			}
		}



		System.Drawing.Bitmap bClientBitmap = null;
		System.Drawing.Graphics bitmapGraphics;
		public System.Drawing.Image UncompressAndGetImage(byte []buffer)
		{
			try
			{
				return AppSharing.ByteArrayToImage(buffer);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public System.Drawing.Image UncompressAndGetImage(byte []buffer)",ex,"",false);
			}	
			return null;
			
		}

		public void TempRecieve(byte[] buffer,ref BITMAPINFO _info, DataType bDataType, int X, int Y,int width,int height,int size)		
		{
		


		}
		//returns true if image data
		public Image RecieveData(byte[] buffer, DataType bDataType, int X, int Y)
		{	
			try
			{
				if(buffer != null)
				{
					buffer = UncompressBuffer(ref buffer);
				}
			
				if(!bIsServer)
				{
					//for client side this will reutrn the image
					if(bDataType == DataType.SizeInformation)
					{
						bClientBitmap = new System.Drawing.Bitmap(X,Y);					
						bitmapGraphics = Graphics.FromImage(bClientBitmap);
						bitmapGraphics.Clear(System.Drawing.Color.White);
				
					}
					else
					{
						if(bitmapGraphics != null)
						{					
							if(buffer != null)
							{
								Image image = AppSharing.ByteArrayToImage(buffer);//UncompressAndGetImage(buffer);
								bitmapGraphics.DrawImage(image,X,Y);
								Image tempImage;
								tempImage = bClientBitmap;
								return tempImage;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public Image RecieveData(byte[] buffer, DataType bDataType, int X, int Y)",ex,"",false);
			}	

			
			return null;

		}
		//int ocunter=0;
		//returns true if image data
		
		public bool isWaitingPic = true;
	
		public Image RecieveDataEx(byte[] buffer, ref BITMAPINFO _info, DataType bDataType, int X, int Y,int width,int height,int size)
		{	
			try
			{
			
				if(!bIsServer)
				{
					if(buffer != null)
					{
						buffer = UncompressBufferEx(ref buffer,size);
					}

					//for client side this will reutrn the image
					if(bDataType == DataType.SizeInformation)
					{
						//						if(bClientBitmap != null)
						//							bClientBitmap.Dispose();
						//						if(X<=0 || Y<=0)
						//							return null;
						//						bClientBitmap = new System.Drawing.Bitmap(X,Y);					
						//						bitmapGraphics=Graphics.FromImage(bClientBitmap);
						//						bitmapGraphics.Clear(System.Drawing.Color.White);
				
					}
					else
					{
						//						if(bitmapGraphics != null)
						//						{
						Image image = AppSharing.ByteArrayToImage(buffer);//UncompressAndGetImage(buffer);	
						try
						{														
							if(image==null)
							{
								//Trace.WriteLine("Image is null...Exception will occur");
								if(isWaitingPic)
								{
									//Console.WriteLine("isWaitingPic = true");
									isWaitingPic = false;
									//Trace.WriteLine("isWaitingPic = false");
									image=Image.FromFile(Application.StartupPath + @"\AppDefault.JPG");
									//bitmapGraphics.DrawImage(img,X,Y);
									//img.Dispose();									
								}
								else
									return null;
							}
							else
								return image;
							//bitmapGraphics.DrawImage(image,X,Y);
							//image.Dispose();
						}
						catch(Exception ex)
						{	
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public Image RecieveDataEx(byte[] buffer, ref BITMAPINFO _info, DataType bDataType, int X, int Y,int width,int height,int size)",ex,"",false);
							//								if(isWaitingPic)
							//								{
							//									//Console.WriteLine("isWaitingPic = true");
							//									isWaitingPic = false;
							//									//Trace.WriteLine("isWaitingPic = false");
							//									Image img = Image.FromFile(Application.StartupPath + @"\AppDefault.JPG");
							//									bitmapGraphics.DrawImage(img,X,Y);
							//									img.Dispose();									
							//								}
							//								else
							//									return null; //GC5
						}						
						GC.Collect(); //KH
						GC.WaitForPendingFinalizers(); //KH
						//							counter++;
						//							bClientBitmap.Save("c:\\temp\\"+counter.ToString()+".bmp");
						return image;
							
						//						}
					}
				}
				
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public Image RecieveDataEx(byte[] buffer, ref BITMAPINFO _info, DataType bDataType, int X, int Y,int width,int height,int size)",ex,"",false);
			}							
			return null;

		}
		private static int HiWord(int number)
		{
			try
			{
				if ((number & 0x80000000) == 0x80000000)
					return (number >> 16);
				else
					return (number >> 16) & 0xffff ;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private static int HiWord(int number)",ex,"",false);
			}	
			return 0;
		}

		private static int LoWord(int number)
		{
			try
			{
				return number & 0xffff;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private static int LoWord(int number)",ex,"",false);
			}	
			return 0;
		}

		private static int MakeLong(int LoWord, int HiWord)
		{
			try
			{
				return (HiWord << 16) | (LoWord & 0xffff);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private static int MakeLong(int LoWord, int HiWord)",ex,"",false);
			}	
			return 0;
			
		}

		private static IntPtr MakeLParam(int LoWord, int HiWord)
		{
			try
			{
				return (IntPtr) ((HiWord << 16) | (LoWord & 0xffff));
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private static IntPtr MakeLParam(int LoWord, int HiWord)",ex,"",false);
			}	
			return new IntPtr(0);
		}
		

		public void ClientToScreenEx(ref Win32.USER32.POINT pt)
		{	
			try
			{
				pt.x += rect.left;
				pt.y += rect.top;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void ClientToScreenEx(ref POINT pt)",ex,"",false);
			}	
		}


		/// <summary>
		///  Get the relervant Mouse Position accordig to Shared Window X,Y
		/// </summary>
		/// <param name="pt"></param>
		public void ScreenToClientEx(ref Win32.USER32.POINT pt)
		{
			try
			{				
				Win32.USER32.GetWindowRect(sharedWnd, ref rect);
				pt.x -= rect.left;
				pt.y -= rect.top;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void ScreenToClientEx(ref POINT pt)",ex,"",false);
			}	
		}


		bool ignore_msg = false;// hack to disable two messages coming together.

		public void RecieveServerData(byte[] buffer,DataType bDataType,int X, int Y)
		{
			try
			{
				
				if(buffer != null)
				{
					buffer = UncompressBuffer(ref buffer);
				}
				if(bDataType == DataType.MouseActivity)
				{
					string strData = ConvertByteArrayToString(buffer);
					int nMessage = Convert.ToInt32(strData);	
					Win32.USER32.POINT pt;
					pt.x = X;
					pt.y = Y;
					ClientToScreenEx(ref pt);//(sharedWnd,ref pt);	
			    
					//Console.WriteLine("" + nMessage);

					switch(nMessage)
					{
						case Win32.USER32.WM_MOUSEMOVE:
							Win32.USER32.SetCursorPos(pt.x,pt.y);
							break;
						case Win32.USER32.WM_LBUTTONDOWN:							 
						
							Win32.USER32.SetCursorPos(pt.x,pt.y);
							//Application.DoEvents();
							//System.Threading.Thread.Sleep(100);				
							//Application.DoEvents();
							//mouse_event(MOUSEEVENTF_ABSOLUTE|MOUSEEVENTF_LEFTUP,pt.x,pt.y,0,IntPtr.Zero);
							AddDebugString("Rcv: WM_LBUTTONDOWN");
							Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_LEFTDOWN,0,0,0,IntPtr.Zero);
						
							break;
						case Win32.USER32.WM_LBUTTONUP:
							Win32.USER32.SetCursorPos(pt.x,pt.y);
						
							AddDebugString("Rcv: WM_LBUTTONUP");
							//	mouse_event(MOUSEEVENTF_ABSOLUTE|MOUSEEVENTF_LEFTUP,pt.x,pt.y,0,IntPtr.Zero);
							//mouse_event(MOUSEEVENTF_LEFTDOWN,0,0,0,IntPtr.Zero);
							Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_LEFTUP,0,0,0,IntPtr.Zero);	
							break;
						case Win32.USER32.WM_LBUTTONDBLCLK:
							AddDebugString("Rcv: WM_LBUTTONDBLCLK");
						
							Win32.USER32.SetCursorPos(pt.x,pt.y);						
							Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_LEFTDOWN,0,0,0,IntPtr.Zero);
							Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_LEFTUP,0,0,0,IntPtr.Zero);						
							Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_LEFTDOWN,0,0,0,IntPtr.Zero);
							Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_LEFTUP,0,0,0,IntPtr.Zero);
							break;
						case Win32.USER32.WM_RBUTTONDOWN:
							Win32.USER32.SetCursorPos(pt.x,pt.y);			
							AddDebugString("Rcv: WM_RBUTTONDOWN");
							//mouse_event(MOUSEEVENTF_RIGHTUP,pt.x,pt.y,0,IntPtr.Zero);
							Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_RIGHTDOWN,pt.x,pt.y,0,IntPtr.Zero);
							break;	
						case Win32.USER32.WM_RBUTTONUP:
							Win32.USER32.SetCursorPos(pt.x,pt.y);			
							AddDebugString("Rcv: WM_RBUTTONUP");
							Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_RIGHTUP,pt.x,pt.y,0,IntPtr.Zero);
							//mouse_event(MOUSEEVENTF_RIGHTUP,pt.x,pt.y,0,IntPtr.Zero);
							break;	
						case Win32.USER32.WM_CLICK:
							//Console.WriteLine("WM_CLICK");
							AddDebugString("Rcv: WM_CLICK");
							Win32.USER32.SetCursorPos(pt.x,pt.y);
						
							Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_LEFTDOWN,0,0,0,IntPtr.Zero);
							Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_LEFTUP,0,0,0,IntPtr.Zero);	
							//mouse_event(MOUSEEVENTF_ABSOLUTE |MOUSEEVENTF_LEFTUP,pt.x,pt.y,0,IntPtr.Zero);	
							break;


					}
					//				SendMessage(sharedWnd,WM_SETCURSOR,IntPtr.Zero,MakeLParam(pt.x,pt.y));
					//SendMessage(sharedWnd,WM_LBUTTONUP,IntPtr.Zero,MakeLParam(584,235));
				
					//SendMessage(sharedWnd,WM_LBUTTONDOWN
				}
				else if(bDataType == DataType.KeyBoardActivity)
				{
					string strData = ConvertByteArrayToString(buffer);
					string []strArray = strData.Split('|');
					if(strArray != null)
					{				
						int nMessage = Convert.ToInt32(strArray[0]);			
						int nCode = Convert.ToInt32(strArray[1]);					
						switch(nMessage)
						{
							case Win32.USER32.WM_KEYDOWN:
								//keybd_event(Convert.ToChar(nCode),0x45,(int)(KEYEVENTF_EXTENDEDKEY|0),IntPtr.Zero);
								Win32.USER32.keybd_event(Convert.ToChar(nCode),0x45,(int)(0),IntPtr.Zero);
								break;
							case Win32.USER32.WM_KEYUP:
								//keybd_event(Convert.ToChar(nCode),0x45,(int)KEYEVENTF_EXTENDEDKEY,IntPtr.Zero);
								//keybd_event(Convert.ToChar(nCode),0x45,(int)KEYEVENTF_EXTENDEDKEY|KEYEVENTF_KEYUP,IntPtr.Zero);
								Win32.USER32.keybd_event(Convert.ToChar(nCode),0x45,(int)Win32.USER32.KEYEVENTF_KEYUP,IntPtr.Zero);
								break;
							case Win32.USER32.WM_CHAR:
								//keybd_event(Convert.ToChar(nCode),0x45,(int)KEYEVENTF_EXTENDEDKEY,IntPtr.Zero);
								Win32.USER32.keybd_event(Convert.ToChar(nCode),0x45,(int)Win32.USER32.KEYEVENTF_EXTENDEDKEY,IntPtr.Zero);
								Win32.USER32.keybd_event(Convert.ToChar(nCode),0x45,(int)Win32.USER32.KEYEVENTF_EXTENDEDKEY|Win32.USER32.KEYEVENTF_KEYUP,IntPtr.Zero);
								//keybd_event(Convert.ToChar(nCode),0x45,(int)KEYEVENTF_EXTENDEDKEY|KEYEVENTF_KEYUP,IntPtr.Zero);
								break;
							
						}
					}

				}
				//for server side it'll just manipulate the key strokes and mouse			
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void RecieveServerData(byte[] buffer,DataType bDataType,int X, int Y)",ex,"",false);
			}	
		}
		private byte[] ConverStringToByteArray(string str)
		{
			byte[] bArray = new byte[str.Length];
			try
			{
				for(int i = 0 ; i < str.Length ; i++)
					bArray[i] = Convert.ToByte(str[i]);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private byte[] ConverStringToByteArray(string str)",ex,"",false);
				//MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
			}	
			return bArray;
		}
		private string ConvertByteArrayToString(byte[] bArray)
		{
			System.Text.StringBuilder a ;
			try
			{
				char[] bCharacters = new char[bArray.Length];
				for(int i = 0 ; i < bArray.Length ; i ++)
					bCharacters[i] = Convert.ToChar(bArray[i]);
				a = new System.Text.StringBuilder(bArray.Length);
				a.Append(bCharacters);
				return a.ToString(0,a.Length);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private string ConvertByteArrayToString(byte[] bArray)",ex,"",false);
			}	
		
			return "";
			//return a.ToString(0,a.Length);
		}
		System.Text.ASCIIEncoding  encoding=new System.Text.ASCIIEncoding();
		public bool SendMouseActivity(uint nWindowMessageCode, Point point)
		{
			try
			{
				//string strData =Convert.ToString(nWindowMessageCode);
				byte []bArray = encoding.GetBytes(Convert.ToString(nWindowMessageCode));
				int nX = point.X;
				int nY = point.Y;	
				CompressAndSendBuffer(ref bArray,DataType.MouseActivity,ref nX,ref nY);
				return true;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public bool SendMouseActivity(uint nWindowMessageCode, Point point)",ex,"",false);
			}
			return false;			
		} 

		public void SendKeys(int iKey)
		{
			try
			{
				SendKeyStroke(Win32.USER32.WM_KEYDOWN, iKey);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void SendKeys(int iKey)",ex,"",false);
			}
		}
		public bool SendKeyStroke(uint nWindowMessageCode,int strKey)
		{
			try
			{
				int bData = Convert.ToInt32(nWindowMessageCode);				
				string strData = bData.ToString() + "|" + Convert.ToString(strKey);
				byte []bArray = ConverStringToByteArray(strData);
				int nX = 0;
				int nY = 0;
				CompressAndSendBuffer(ref bArray,DataType.KeyBoardActivity,ref nX,ref nY);
				return true;
			}
			catch(Exception ex )
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public bool SendKeyStroke(uint nWindowMessageCode,int strKey)",ex,"",false);
			}
			
			return false;	
		}
	}
}


