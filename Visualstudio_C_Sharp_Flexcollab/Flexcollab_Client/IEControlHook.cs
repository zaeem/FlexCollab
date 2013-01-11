/*
 *     PROBLEMS THAT ARE CURRENTLY IN OUR SYSTEM
 * 
1--- DrawErase dont work properly when we intercept the WM_PAINT message in the winprog because
	when we draw eraser from the message of WM_PAINT there is not default listener so message of draw eraser comes after drawing
	and overlap the drawing area....like we draw a arrow head then we erase one time and then again draw arrowhead on first arrowhead
	...now problem comes at the time of repaint object while we intercept WM_PAINT method.now drawing occur in this sequence.
	1.....Arrow head
	2.....Arrow head
	3.....Erase.

2---we make Erase tool using InvalidateRect API which send a rectangle erasemessage on setting its erase parameter to true.
    But in futur if we plan to do double buffering then this technique will fail because InvalidateRect API only usable in
	window environment not function on in-memory bitmap..so there will be some good solution of this tool.
 



*/



using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using mshtml;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Threading;
using WebMeeting.Common;
using WebMeeting.Client;
using System.Diagnostics; 
using System.IO;
using System.Xml;
using SHDocVw;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using WebMeeting.Client.Alerts;
using Microsoft.Win32;
using Win32;




//using System.Diagnostics;

namespace WebBrowserSubClassing
{

	[
	Guid("3050f669-98b5-11cf-bb82-00aa00bdce0b"), 
	InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown), 
	ComVisible(true), 
	ComImport 
	] 
	interface IHTMLElementRender 
	{ 
		void DrawToDC([In] IntPtr hDC); 
		void SetDocumentPrinter([In, MarshalAs(UnmanagedType.BStr)] string bstrPrinterName, [In] IntPtr hDC); 
	}; 
	[Serializable]
	public enum WhiteboardToolCode
	{
		WhiteboardToolMin,
		Pencil,
		Eraser,
		Brush,
		Line,
		Ellipse,
		Rectangle,		
		None,
		Text,
		UniArrow,
		Undo,
		Redo,
		PlaceHolderArrow,
		WhiteboardToolMax
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}


	public delegate void OnMouseMoveDelegate(int X,int Y);
	public delegate void htmlViewerEventHandler(object sender, mshtml.IHTMLEventObj e);
	public delegate void htmlViewerEventHandler2(object sender, mshtml.IHTMLEventObj e); 
	
	public class IEHwd
	{
		public event OnMouseMoveDelegate OnMouseMoveEx;
		private void FireMouseMove(int nX, int nY)

		{
			try
			{
				if(OnMouseMoveEx != null)
					OnMouseMoveEx(nX,nY);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void FireMouseMove(int nX, int nY)",ex,"",false);
			}

		}
	
		#region PINVOKE Code
		//Some Delegates to extend our functionality
		private delegate Int32 EnumChildProc(IntPtr hWnd, IntPtr lParam);
		private delegate int Win32WndProc(IntPtr hWnd, int Msg, int wParam, int lParam);

		
        
		public IntPtr oldWndProc = IntPtr.Zero;
		private Win32.USER32.Win32WndProc newWndProc = null;
		private IntPtr GhWnd;
		//private ContextMenu ieContextMenu;		
		public bool DrawNow = false;



		private bool navigationCompleted = false;
		#endregion
		
		#region Scrolling Events and Functions
		public delegate void OnVerticalScrollDelegate(int nVScroll,int nHScroll) ;
		public event OnVerticalScrollDelegate OnScroll;
		public void FireScrollEvent()		
		{
			try
			{
				if(OnScroll != null)
				{
					System.Drawing.Point pt = 	GetCurrentScrollPositionBrowser(browserControl);
					OnScroll(pt.Y,pt.X);
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void FireScrollEvent()	",ex,"",false);
			}
		}

		public void ScrollView(int nX, int nY)
		{
			try
			{
				IHTMLDocument2 doc = (IHTMLDocument2)  browserControl.Document;
				if(doc == null)
					return;
				IHTMLElement2 body = doc.body as IHTMLElement2;
				//Console.WriteLine("nX = " + nX.ToString() + " nY = " + nY.ToString());
				if(body != null)
				{
					body.scrollTop = nY;
					body.scrollLeft = nX;
				}				
				this.lastScrollPosition.X=nX;
				this.lastScrollPosition.Y = nY;

			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void ScrollView(int nX, int nY)",ex,"",false);
			}

		}
		#endregion

	
		#region custom variables
		IntPtr browserHandle = new IntPtr();
		private bool ButtonPressed =false;
		Point previousScrollPos = new Point(0,0);
		public bool addAnnotations = true;
		private bool showAnnotations  = true;
		#endregion
		
		WhiteboardMessage msgPencilMessage ;
		WhiteboardMessage  msgGlobalPencilMessage ;
		//int pointsDrawByPencil = 0;
		Point pointOldScrollTop = new Point(0,0); 
		public int thisSessionID = 0;
		public int currentSlideNo = 0;
		public WebMeeting.Common.AnnotationMessageType annotationType;
		//Excel variables
		public mshtml.IHTMLDocument2 HTMLDoc1;
		public mshtml.IHTMLWindow2 HTMLWin1;
		public mshtml.IHTMLElement2 htmlele;


		public event htmlViewerEventHandler htmlViewerEvent;
		public event htmlViewerEventHandler2 htmlViewerEvent2;
		public mshtml.IHTMLDocument2 HTMLDocument;
		public mshtml.HTMLDocument myDocument;
		public mshtml.IHTMLWindow2 window2;

		public bool IsPaintingStarted=true;
		public bool IsScrollProcessed=true;
		public bool IsWMSizeProcessed=true;
		public bool IsExcel = false;
		public bool IsChangeAllowed = true;
		public bool IsAttendee = false;
		public bool haveFrames = false;
		public bool InvalidMousedown = false;
		public string currentOpenedExcelFile;
		public ArrayList sheetsEventStores;
		public int currentSheet;
		public bool sinkExcelCalled = false;
		public bool  bScrollDraw=false;		//new change
		
		public bool IsPPT = false;
			
		//      By Zaeem For Place Holder		
		//public FontStyle constFont=new FontStyle();
		//public Font constmyFont = new Font(this.Font.FontFamily,this.Font.Size,constFont);
		
		private Font localCopyOfFont=new Font("Microsoft Sans Serif",8);
		
		public FontStyle constFont;
		public Font constmyFont ;
		
			
		public void AttendeeHookStart()
		{
			IsAttendee = true;
			try

			{
				string strPath =  currentOpenedExcelFile;
				strPath += "_files";
				if(Directory.Exists(strPath))
				{
					string[] files = Directory.GetFiles(strPath ,"sheet*.htm");
					if(files.Length > 0)
					{
							
						if(!sinkExcelCalled)
						{					
							sheetsEventStores = new ArrayList();
							sheetsEventStores.Add(eventStore);
							for(int i = 0 ; i < files.Length-1 ; i++)
							{
								eventStore = new ArrayList();
								sheetsEventStores.Add(eventStore);
							}

							eventStore = (ArrayList)sheetsEventStores[0];
							sinkExcelCalled = true;
						}
						
						try
						{
							IHTMLDocument2 doc = (IHTMLDocument2)  browserControl.Document;
							IHTMLElement2 body = (IHTMLElement2)doc.body;
							IHTMLElementCollection col =  (IHTMLElementCollection)body.getElementsByTagName("frame");							
							if(col.length > 0)
							{
								object o1 = "frSheet";                        
								object index = 0;	
								IHTMLElement frm = (IHTMLElement)col.item(o1,index);
								object o2 = frm.getAttribute("src",0);
								o2 = o2;								

							}
					        
						}
						catch(Exception ex)// ee)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void AttendeeHookStart()",ex,"",false);
						}
					}	
				}

	
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void AttendeeHookStart()",ex,"",false);
			}
		}
		
		public ArrayList sendingArray = new ArrayList();
		public void sendFunction()
		{
			try
			{
				StateUpdateMessage msg;
				int preSendCount;
				WebMeeting.Client.NetworkManager network = WebMeeting.Client.NetworkManager.getInstance();
				while(m_bIsActive)
				{
					try
					{						
						if(sendingArray.Count <1)
						{
							Thread.Sleep(10);
							continue;
						}
						msg=new StateUpdateMessage();
						if(WebMeeting.Common.AnnotationMessageType.WEB== annotationType)
						{
							msg.updateType=	UpdateTypeCode.UpdateWebBrowser;
						}
						else
						{
							msg.updateType=	UpdateTypeCode.UpdateWebDocument;
						}
						msg.dataArray = (ArrayList)sendingArray.Clone();
						msg.SenderID = network.profile.ClientId;
						preSendCount=sendingArray.Count;
						network.SendLoadPacket(msg);
				
						if(preSendCount < sendingArray.Count)
						{ // remove only seneded messages
							sendingArray.RemoveRange(0,preSendCount);
						}
						else
						{
							sendingArray.Clear();
						}
						msg  = null;
//						for(int i = 0 ; i < sendingArray.Count ; i++)
//						{							
//							NetworkManager.getInstance().SendLoadPacket((MessageObject) sendingArray[i]);
//						}
//						sendingArray.Clear();
					}
				
					catch(System.Threading.ThreadAbortException exp)
					{
						exp=exp;
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void sendFunction()",ex,"",false);
					}

					catch(Exception ex)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void sendFunction()",ex,"",false);
					}

				}
			}
			catch(System.Threading.ThreadAbortException exp)
			{
				exp=exp;
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void sendFunction()",ex,"",false);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void sendFunction()",ex,"",false);
			}
			
		}
		public void SelectToolLine()
		{
			try
			{
				this.tool=WhiteboardToolCode.Line;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void SelectToolLine()",ex,"",false);
			}

				
		}
		
		public void selectToolNone()
		{
			try
			{
				this.tool=WhiteboardToolCode.None;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void selectToolNone()",ex,"",false);
			}
		}
		#region Constructor
		public IEHwd()
		{
			try
			{
				msgGlobalPencilMessage = new WhiteboardMessage();

				/*if(NetworkManager.getInstance().profile.clientType== ClientType.ClientAttendee) // for restricting attendees
				{
					Trace.WriteLine("attendee");
					tool=WhiteboardToolCode.None;
				}
				else
				*/
				{
					//Trace.WriteLine("presenter");
					tool=WhiteboardToolCode.Line;//.Brush;//.Ellipse;//Rectangle;//.Line;
				}
				CursorImage = WebMeeting.Client.ClientUI.getInstance().LoadImageFromResource("WebMeeting.Client.cur.gif");			
				//tool=WhiteboardToolCode.Line;//.Brush;//.Ellipse;//Rectangle;//.Line;
				color=Color.Red;
				mouseDownPos=new Point(0,0);
				mouseCurrentPos=new Point(0,0);
				brushThickness=10;
				eventStore=new ArrayList();			
				pencilLastPoint=new Point(-1,-1);
				eraser=new Rectangle(0,0,10,10);
				msg=new WhiteboardMessage();
			
				m_bDummyRect=new Rectangle(0,0,0,0);
			
			
				undoArray=new ArrayList();
				redoArray=new ArrayList();
				//filename="";
				lineThickness=4;

				pA.nWidth = 10;
				pA.fTheta = 0.3f;
				pA.bFill = true;
				recieveThread = new Thread(new ThreadStart(ConsumeThread));
				sendingThread = new Thread(new ThreadStart(sendFunction));
				sendingThread.Name="IE ControlHook Constructor: sendFunction()";
				sendingThread.Start();
				m_bIsActive =true;
				recieveThread.Name="IE ControlHook Constructor: ConsumeThread()";
				recieveThread.Start();

				textBox1 = new TextBox();
				textBox1.Visible = false;
				//this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
				this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
				this.textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public IEHwd()",ex,"",false);
			}
			
            
		}
		~IEHwd()
		{
			
		}
		#endregion
		
		public void DisposeResources()
		{
			try
			{
				msgGlobalPencilMessage = null;	
				if(CursorImage!=null)
					CursorImage.Dispose();
				//tool=WhiteboardToolCode.Line;//.Brush;//.Ellipse;//Rectangle;//.Line;
				eventStore=null;								
				msg=null;
				this.browserControl=null;
				if(undoArray!=null)
				{
					this.undoArray.Clear();
					this.undoArray=null;
				}
			
				if(redoArray!=null)
				{
					redoArray.Clear();
					redoArray=null;
				}
			
			
				//filename="";
		
				if(textBox1!=null)
				{
					textBox1.Dispose();
					textBox1=null;
				}
			}
			catch(Exception exp)
			{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void Dispose Resources 488 line",exp,"",false);
			}
			
		}
		public void abortThreads()
		{
			try
			{
				recieveThread.Abort();
				recieveThread.Join(10000);
			
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void abortThreads()",ex,"",false);
			}
			try
			{
				sendingThread.Abort();
				sendingThread.Join(10000);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void abortThreads()",ex,"",false);
			}
		}

		#region Hooking/Subclassing Functions
		public void StopSubclass(IntPtr hWnd)
		{
			try
			{
				Win32.USER32.SetWindowLong(hWnd, Win32.USER32.GWL_WNDPROC, oldWndProc);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void StopSubclass(IntPtr hWnd)",ex,"",false);
			}
		}		
	
		
		public IntPtr IEFromhWnd(IntPtr hWnd)
		{
			try
			{
				if(!hWnd.Equals(0))
				{
					if(!IsIEServerWindow(hWnd))
					{
						// Get 1st child IE server window
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

		private Int32 EnumChild(IntPtr hWnd, IntPtr lParam)
		{
			try
			{
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

		private bool IsIEServerWindow(IntPtr hWnd)
		{
			try
			{

				Int32 Res;
				StringBuilder ClassName = new StringBuilder(256);

				// Get the window class name
				Res = Win32.USER32.GetClassName(hWnd.ToInt32(), ClassName, ClassName.MaxCapacity);
				if(ClassName.ToString() == "Internet Explorer_Server")
					return true;
				else
					return false;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private bool IsIEServerWindow(IntPtr hWnd)",ex,"",false);
				return false;
			}
		}

		public AxSHDocVw.AxWebBrowser browserControl;

		public void SinkExcel()
		{
			try
			{

				if(haveFrames)
				{
					
					string strPath =  currentOpenedExcelFile;
					strPath += "_files";
					if(Directory.Exists(strPath))
					{
						string[] files = Directory.GetFiles(strPath ,"sheet*.htm");
						if(files.Length > 0)
						{
							
							if(!sinkExcelCalled)
							{					
								sheetsEventStores = new ArrayList();
								sheetsEventStores.Add(eventStore);
								for(int i = 0 ; i < files.Length-1 ; i++)
								{
									eventStore = new ArrayList();
									sheetsEventStores.Add(eventStore);
								}

								eventStore = (ArrayList)sheetsEventStores[0];
								sinkExcelCalled = true;
							}
							SynchronizePage();
						}	
					}
					
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void SinkExcel()",ex,"",false);
			}
		}
		public void SinkNavigationEvents(AxSHDocVw.AxWebBrowser browser)
		{
			try
			{
				browser.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(browser_NavigateComplete2);
				browser.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(browser_BeforeNavigate2);
				browser.DocumentComplete += new  AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(browser_DocumentComplete);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void SinkNavigationEvents(AxSHDocVw.AxWebBrowser browser)",ex,"",false);
			}
	
		}
		
		IntPtr hWndTemp;
		public void StartSubclass(IntPtr hWnd,AxSHDocVw.AxWebBrowser browser)
		{
			// delegate for the new wndproc
			//System.Diagnostics.Debug.Write("calling mywndproc from startsybclass");
			try
			{
				newWndProc = new Win32.USER32.Win32WndProc(MyWndProc);
				hWndTemp = hWnd;
				if(oldWndProc.ToInt32() != 0)
					Win32.USER32.SetWindowLong(hWnd, Win32.USER32.GWL_WNDPROC, newWndProc);
				else
					oldWndProc = Win32.USER32.SetWindowLong(hWnd, Win32.USER32.GWL_WNDPROC, newWndProc);
				

				if(browser!= null)
				{
				
					if((browser.LocationURL != null) || (browser.LocationURL != ""))
					{
						navigationCompleted = true;
					}

					browserControl = browser;
					SinkNavigationEvents(browser);
				
	
					try
					{
						if(browser.Document != null)				
						{
							IHTMLDocument2 html = (IHTMLDocument2)browser.Document;
							haveFrames=  (html.frames.length > 0);
						}
					}
					catch(Exception ex)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void StartSubclass(IntPtr hWnd,AxSHDocVw.AxWebBrowser browser)",ex,"",false);
					}
				}
				SinkExcel();			
				int nRegion = Win32.GDI32.CreateRectRgn(10,10,50,100);
		
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void StartSubclass(IntPtr hWnd,AxSHDocVw.AxWebBrowser browser)",ex,"",false);
			}
	

			//browser.Region = new Region(new Rectangle(10,10,20,30));

		}
		#endregion

		

		public Thread recieveThread;
		public Thread sendingThread;
		 

    
		public bool ShowAnnotations
		{
			get
			{
				return showAnnotations;
			}
			set
			{
				showAnnotations = value;
				if(!showAnnotations)
					AddAnnotations = false;
			}
		}

		public bool AddAnnotations
		{
			get
			{
				return addAnnotations;
			}
			set
			{
				addAnnotations = value;
				if(addAnnotations)
					showAnnotations = true;
				
			}
		}

		public int _mouseX, _mousey;
		private Image CursorImage;
		public void Refresh()
		{
			try
			{
				//System.Diagnostics.Debug.Write("in refresh");
				if(browserHandle != IntPtr.Zero)
					Win32.USER32.PostMessage(browserHandle,Win32.USER32.WM_PAINT,new IntPtr(),new IntPtr());						
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void Refresh()",ex,"",false);
			}
		}
		private void DrawMouse(ref Graphics g)
		{
			try
			{
				if(this.IsAttendee)
				{
					//	g.DrawImage(this.CursorImage , _mouseX - lastScrollPosition.X ,_mousey - lastScrollPosition.Y );
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawMouse(ref Graphics g)",ex,"",false);
			}
		}

		public bool bBlockMouseMovment = false;
		private Point lastScrollPosition = new Point(0,0);
		bool bOnlyDrawParent = false;
		public bool bOnlyOneRef=false; 
		public System.Timers.Timer  myTimer;
		
		//= new StreamWriter("c:\\abc.dat")
		private  void DisplayTimeEvent( object source,System.Timers.ElapsedEventArgs eArgs )
		{			
			RepaintObjects(-1);
			this.bScrollDraw=false; 
			this.myTimer.Dispose();
		}
		public bool chkWm_Size=true;
		public bool checkmsg=false;
		public int  bodyClientHeight=0;
		public bool checkDrawPaint=true;
		int nSize_width=0,nSize_heigth=0;
		private int MyWndProc(IntPtr hWnd, int Msg, int wParam, int lParam)
		{			
			haveFrames = false;
			
			try
			{
				
				try
				{
					//Trace.WriteLine("\n Tracing Message of Repain Window When something change"+Msg.ToString("X"));
					if(this.chkWm_Size==true)
					{					
						this.chkWm_Size=false;
					}
					
					
					switch(Msg)
					{										
//						case WM_NCPAINT:    // it paint clientarea when we minimize,maximize but the only prob
											//is that when we collapse right pane and botton pane then there 
											//is problem of redraw, by junaid.
//							this.bScrollDraw=true;
							//RepaintObjects(-1);
							//break;						
						case Win32.USER32.WM_LBUTTONDBLCLK:
							return 0;
							#region case WM_SIZE :
						/// This evernt occurs on the size chagew when ever you change the size of the bowser 		
						case Win32.USER32.WM_SIZE :
							 //If m.Msg = WM_SIZE Then
							 System.Drawing.Size wnd_size;
							 wnd_size = new  System.Drawing.Size(new System.Drawing.Point(lParam));
							// this.nSize_heigth=wnd_size.Height;
							// this.nSize_width=wnd_size.Width;
 
							if(wnd_size.Width!=0 && wnd_size.Height!=0 )
							{
								if(this.nSize_width < wnd_size.Width  || this.nSize_heigth < wnd_size.Height)
								{
									this.nSize_width=wnd_size.Width;
									this.nSize_heigth=wnd_size.Height; 
									this.chkWm_Size=true;
									this.checkDrawPaint=true;
								}
								else
								{
									this.nSize_width=wnd_size.Width;
									this.nSize_heigth=wnd_size.Height; 
									this.chkWm_Size=false;
									this.checkDrawPaint=false; 
								}
							}
							else
							{
								this.nSize_heigth=0;
								this.nSize_width=0;
								this.checkDrawPaint=false; 
								this.chkWm_Size=false;
							}
							
							//Trace.WriteLine("height : " + wnd_size.Height.ToString() + "Width : " + wnd_size.Width.ToString()); 
							return 0;
							//wnd_size=null;
							//End If
							#endregion

						// This function of scrolling doesn't work 
						case Win32.USER32.WM_VSCROLL :
							//Trace.WriteLine("WM_VSCROLL is fired");
							FireScrollEvent();
							break;
							#region case WM_PAINT:							
						case Win32.USER32.WM_PAINT:								
							try
							{						
							
								if(bOnlyDrawParent)
								{
									bOnlyDrawParent = false;
									return Win32.USER32.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
								}
								if((haveFrames) && (!IsExcel))
									return Win32.USER32.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
								
								Point p = GetCurrentScrollPositionBrowser(browserControl);
								if(p != lastScrollPosition)
								{
									checkmsg=true;
									
									System.Drawing.Graphics glocal;
									try
									{
										if(this.bScrollDraw==false)
										{
											myTimer = new System.Timers.Timer(); 
											myTimer.Interval = 200;
											myTimer.Elapsed += new System.Timers.ElapsedEventHandler(DisplayTimeEvent);
											myTimer.Start();
											this.bScrollDraw=true; 
										}
										glocal = Graphics.FromHwnd(hWnd);
										if(g!=null)
										{		
											lock(g)
											{
												g = glocal;
											}
										}
										else
										{
											g=glocal;
										}
										browserHandle = hWnd;
				
										Size s = browserControl.ClientSize;
										this.bodyClientHeight=getBodyClientHeight().Y;
										

										int pixels = Win32.USER32.GetSystemMetrics(Win32.USER32.SM_CXVSCROLL)+2 ;				
										int pixels2 = Win32.USER32.GetSystemMetrics(3);
				
										s.Width -= pixels ;
										//s.Height -= pixels ; // by kamran

										//store the previous screen point, help in at the While of WM_SIZE
										s.Width -= pixels ;
										s.Height -= pixels ; // by kamran
										nSize_width=s.Width;
										nSize_heigth=this.bodyClientHeight; //;this.browserControl.Height;
										/* 
										    Tracing exception of the getBodyClientHeight
											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: private int MyWndProc(IntPtr hWnd, int Msg, int wParam, int lParam) wrong parameter WebSharing Region Parameters :::: X=0,Y=0; "+ "Width ::: " + s.Width.ToString()+ " Height ::: " + this.bodyClientHeight.ToString(),new Exception(" annotation problem in document sharing"),"",false);
											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: private int MyWndProc(IntPtr hWnd, int Msg, int wParam, int lParam) correct parameter WebSharing Region Parameters :::: X=0,Y=0; "+ "Width ::: " + s.Width.ToString()+ " Height ::: " + (s.Height-2).ToString(),new Exception(" annotation problem in document sharing"),"",false);
										*/																				
										
										/*
										 * this check is due to the exception of the getBodyClientHeight
										 */
										if(this.bodyClientHeight==0)
										{			
											lock(g)
											{
												g.Clip = new Region(new Rectangle(0,0,s.Width,s.Height-2));
											}
										}
										else
										{
											lock(g)
											{
												g.Clip = new Region(new Rectangle(0,0,s.Width,this.bodyClientHeight));
											}
										}
										
										//g.Clip = new Region(new Rectangle(0,0,s.Width,this.bodyClientHeight));
										IsWMSizeProcessed=true;
										//g.Clip = new Region(new Rectangle(0,0,s.Width,s.Height));										
									}
									catch(Exception ex)// ee)
									{
										WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private int MyWndProc(IntPtr hWnd, int Msg, int wParam, int lParam)",ex,"",false);
									}

									DrawNow=true;
									FireScrollEvent();
									//this.bScrollDraw=true; 
								}
								else
								{	
									this.checkmsg=false;									
								}
								//Trace.WriteLine("lastScrollPosition" + lastScrollPosition.ToString() + "p" + p.ToString());
								lastScrollPosition = p;
					

								//repaint non-client area when WM_SIZE messages intercept and WM_SIZE width,heigth not equal 
                                // to zero and current width screen size is greater than the new WM_SIZE width,height

								if(this.checkDrawPaint==true )
								{
									//Trace.WriteLine("Now graphics will appear");
									p = GetCurrentScrollPositionBrowser(browserControl);
									System.Drawing.Graphics glocal;	
									glocal = Graphics.FromHwnd(hWnd);
									if(g==null)
									{
										g = glocal;
									}
									else
									{
										lock(g)
										{
											g = glocal;
										}
									}
									browserHandle = hWnd;
				
									Size s = browserControl.ClientSize;
									this.bodyClientHeight=getBodyClientHeight().Y;
									int pixels = Win32.USER32.GetSystemMetrics(Win32.USER32.SM_CXVSCROLL)+2 ;				
									//SM_CXHSCROLL 21 for getting scroll
									int pixels2 = Win32.USER32.GetSystemMetrics(3);
				
									s.Width -= pixels ;
									s.Height -= pixels ; // by kamran
									//this.browserControl.ClientToWindow;
									
									//store the previous screen point, help in at the While of WM_SIZE
									nSize_width=s.Width;
									nSize_heigth=this.bodyClientHeight;
									if(this.bodyClientHeight==0)
									{
										lock(g)
										{
											g.Clip = new Region(new Rectangle(0,0,s.Width,s.Height-2));
										}
									}
									else
									{
										lock(g)
										{
											g.Clip = new Region(new Rectangle(0,0,s.Width,this.bodyClientHeight));
										}
									}
									//Trace.WriteLine(s.Width.ToString()+"  " +this.bodyClientHeight.ToString()+" " +s.Height.ToString());
									//g.Clip = new Region(new Rectangle(0,0,s.Width,this.bodyClientHeight));
									//Trace.WriteLine("Now graphics will appear2");
									//g.Clip = new Region(new Rectangle(0,0,s.Width,s.Height));
									this.DrawNow=true; 
									this.chkWm_Size=false;
									this.checkDrawPaint=false;
									//Trace.WriteLine("In Size Handle");
									IsWMSizeProcessed=true;
									RepaintObjects(-1);									
								}
								else 
								{
									if((this.IsWMSizeProcessed==false))
									{
										if(this.bScrollDraw==false)
										{
//											if(this.IsScrollProcessed==true)
//											{
												Win32.USER32.RECT update = new Win32.USER32.RECT();
											if(Win32.USER32.GetUpdateRect(browserHandle,ref update, 
												false)==0)
											{
												//Trace.WriteLine("there is no Update Region Is available");
											}	
											else
											{

												if(this.IsPaintingStarted==true)
												{
													IsPaintingStarted=false;
													this.DrawNow=true;
													RepaintObjects(-1);
													//Trace.WriteLine("Update Region is available for painting");
													this.IsPaintingStarted=true;
												}
											}
//											}
										}
									}
									else
									{
									//	if(this.IsScrollProcessed==true)this.IsScrollProcessed=false;
										if(this.IsWMSizeProcessed==true)this.IsWMSizeProcessed=false;
									}
								}
//								if((DrawNow) && (this.bScrollDraw==true))
//								{		
//								//	this.bScrollDraw=true;
//									if(showAnnotations)
//									{
//										//System.Diagnostics.Debug.Write("calling from mywnd proc\n");																				
//										//RepaintObjects(-1);
//									}
//								}
							
							}
							catch(Exception ex)
							{
								WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private int MyWndProc(IntPtr hWnd, int Msg, int wParam, int lParam)",ex,"",false);
							}
							Win32.USER32.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
							return 0;
							#endregion
						case Win32.USER32.WM_RBUTTONDOWN:
							return 0;
						case Win32.USER32.WM_RBUTTONDBLCLK:
							return 0;
							#region case WM_LBUTTONDOWN:
							
						case Win32.USER32.WM_LBUTTONDOWN:		
							try
							{
								haveFrames = false;
								if((haveFrames) && (!IsExcel))
									return Win32.USER32.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
								if((addAnnotations) && ( navigationCompleted))
								{
									

									Point p = new Point();					
									Win32.USER32.GetCursorPos(ref p);
									Win32.USER32.ScreenToClient(browserControl.Handle.ToInt32(), ref p);										

									Size s = browserControl.ClientSize;
									int pixels = Win32.USER32.GetSystemMetrics(Win32.USER32.SM_CXVSCROLL)+2 ;				
									int pixels2 = Win32.USER32.GetSystemMetrics(3);
				
									s.Width -= pixels ;
									s.Height -= pixels ; // by kamran

									if(IsExcel)
									{
										if(haveFrames)
										{
											int nHeight = GetExcelLowerFrameHeight(browserControl);                                        											
											s.Height -= nHeight;
										}

									}

									if(p.X > s.Width)//, 384
									{			
										InvalidMousedown = true;
										return Win32.USER32.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
									}
									if(p.Y > s.Height)
									{
										InvalidMousedown = true;
										return Win32.USER32.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);

									}
									InvalidMousedown = false; 																						
									OnMouseDown(p);
									ButtonPressed= true;					
									return 0;			
								}
								else
								{
									break;
								}
							}
							catch(Exception ex)// ee)
							{
								WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private int MyWndProc(IntPtr hWnd, int Msg, int wParam, int lParam)",ex,"",false);
							}
							break;					
							#endregion
							#region case WM_MOUSEMOVE:
						case Win32.USER32.WM_MOUSEMOVE:
							
							if((haveFrames) && (!IsExcel))
								return Win32.USER32.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);

							Point p2 = new Point();
							Win32.USER32.GetCursorPos(ref p2);
							Win32.USER32.ScreenToClient((int)browserHandle,ref p2);
							FireMouseMove(p2.X,p2.Y);

							if((addAnnotations)&& ( navigationCompleted))
							{
								bool once=false;
								if(ButtonPressed)
								{                                    
									Point  p1 = new Point();
									Win32.USER32.GetCursorPos(ref p1);								
									Win32.USER32.ScreenToClient(browserControl.Handle.ToInt32(), ref p1);		
									//bOnlyDrawParent = true;
									if(this.tool==WhiteboardToolCode.Eraser)
									{
										
										OnMouseMove(p1);
										return 0;			
									}
	

									if(this.tool != WhiteboardToolCode.Pencil)
									{
										//browserControl.Refresh();
										//this.Refresh();		
										OnMouseMove(p1);	
										//this.Refresh();		
									}
									else
										OnMouseMove(p1);
									if(this.tool == WhiteboardToolCode.Text)																			
										ButtonPressed=false;									
									
									return 0;
//									Point  p1 = new Point();
//									GetCursorPo s(ref p1);								
//									ScreenToClient(browserControl.Handle.ToInt32(), ref p1);		
//									//bOnlyDrawParent = true;
//									if(WebMeeting.Client.ClientUI.getInstance().tabBody.SelectedTab.Title=="Web Sharing")
//									{										
//										browserControl.Refresh();												
//										//this.Refresh();
//										OnMouseMove(p1);	
//									}
//									else
//									{										
//										browserControl.Refresh();												
//										OnMouseMove(p1);
//									}
//									if(this.tool == WhiteboardToolCode.Text)																			
//										ButtonPressed=false;									
//									
//									return 0;
								}
								if(bBlockMouseMovment)
									return 0;
							}
							break;
							#endregion
							#region case WM_LBUTTONUP:
							
						case Win32.USER32.WM_LBUTTONUP:
						
							if((haveFrames) && (!IsExcel))
								return Win32.USER32.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
							if(InvalidMousedown)
								return Win32.USER32.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);

							if((addAnnotations)&& ( navigationCompleted))
							{
								Point p1 = new Point();
								Win32.USER32.GetCursorPos(ref p1);
								Win32.USER32.ScreenToClient(browserControl.Handle.ToInt32(),ref p1);  

								//Excel Check
								if(IsExcel)
								{
									if(haveFrames)
									{
										int nHeight = GetExcelLowerFrameHeight(browserControl);                                        
										Size s = browserControl.ClientSize;
										int pixels = Win32.USER32.GetSystemMetrics(Win32.USER32.SM_CXVSCROLL)+2 ;				
										int pixels2 = Win32.USER32.GetSystemMetrics(3);
				
										s.Width -= pixels ;
										s.Height -= pixels ; // by kamran
																				
										s.Height -= nHeight;	 

										if(p1.Y > s.Height)
											return 0;
									}
								}
								//OnMouseMove(p1);
								if(this.tool == WhiteboardToolCode.Eraser)
									this.sendEraserPacket();
								else if(this.tool == WhiteboardToolCode.Brush)
								{
									this.sendBrushPacket(); 
																
								}
								else if(this.tool != WhiteboardToolCode.Pencil)
								{
									if(this.tool!= WhiteboardToolCode.Text)
										NewOnMouseUp(p1);
								}
								else
								{
									sendPencilPacket();
									//Trace.WriteLine("PENCIL MESSAGE COUNT" + this.msgPencilMessage.endPoints.Count.ToString()  );
								}
								//OnMouseUp(p1);
								
								ButtonPressed= false;	
								//System.Diagnostics.Debug.Write("calling wm_paint from mywndproc");
								Win32.USER32.PostMessage(hWnd,Win32.USER32.WM_PAINT,new IntPtr(),new IntPtr());	// by kamran
								
								return 0;	
							}
							else
							{
								break;
							}					
							#endregion
						default:							
							break;
					}
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private int MyWndProc(IntPtr hWnd, int Msg, int wParam, int lParam)",ex,"",false);
					
				}				
			}
			catch(Exception ex)// ee)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private int MyWndProc(IntPtr hWnd, int Msg, int wParam, int lParam)",ex,"",false);				
			}
			return Win32.USER32.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
		}

		
		
		public void SendEraseMessage()
		{
		
			try
			{
				Win32.USER32.RedrawWindow(browserHandle,IntPtr.Zero,IntPtr.Zero,Win32.USER32.RDW_INVALIDATE | Win32.USER32.RDW_UPDATENOW | Win32.USER32.RDW_ALLCHILDREN);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void SendEraseMessage()",ex,"",false);
			}
		}
		
		public void ClearItems(bool bClearAll,int clientId)
		{
			/////////////////////////
			try
			{
				WhiteboardMessage eraseMsg = new WhiteboardMessage();
				msg.ConferenceID = WebMeeting.Client.NetworkManager.thisInstance.profile.ConferenceID;
				int noofItem=eventStore.Count;
				for(int i =(noofItem-1) ; i >=0; i--)
				{
					eraseMsg = (WhiteboardMessage)eventStore[i];	
					
					if(clientId==-1)
					{
						if(eraseMsg.SlideNo == currentSlideNo)
						{
							if(i>-1)
							{
								eventStore.RemoveAt(i);  // only remove current messages
								//eventStore.Clear();
							}
						}
						//eventStore.RemoveAt(i);  // only remove current messages
					
					}
					
					if(eraseMsg.SenderID==clientId)
					{
						if(eraseMsg.SlideNo == currentSlideNo)
						{
							if(i>-1)
							{
								eventStore.RemoveAt(i);  // only remove current messages
								//eventStore.Clear();
							}
						}
						//eventStore.RemoveAt(i);  // only remove current messages
					}
					//////////////////////////
					
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void ClearItems(bool bClearAll,int clientId)",ex,"",false);
			}
			//return; // check it
			//RepaintObjects(clientId);//done by junaid
			
			return;  
		}
		
		WhiteboardMessage messagePencil;
		WhiteboardMessage mPencil;
		Thread thDrawPencil ;
		ArrayList objPencilMessages = new ArrayList();
		Point previousStartPencilPoint = new Point(0,0);







		private void test_PlaceHolderArrow(Point p,Color c,String text)
		{			
			try
			{
				constFont=new FontStyle();
				constFont=constFont|FontStyle.Bold;
				//Font myFont = new Font(textBox1.Font.FontFamily,textBox1.Font.Size,ff);
				constmyFont = new Font("Microsoft Sans Serif",8,constFont);
				



				
					int width=text.Length * 5 + 20;
					p.X=p.X - width - 19;
					p.Y=p.Y- 19;
			
					Point[] points=new Point[7];
					points[0]=new Point(p.X ,p.Y+10);
					points[1]=new Point(p.X +width,p.Y+10);
					points[2]=new Point(p.X +width,p.Y+1);
					points[3]=new Point(p.X +width+19,p.Y+19);
					points[4]=new Point(p.X +width,p.Y+37);
					points[5]=new Point(p.X +width,p.Y+28);
					points[6]=new Point(p.X ,p.Y+28);
			
					GraphicsPath path = new GraphicsPath();
					path.AddPolygon(points);
					Region r=new Region(path);
			
					//g.FillEllipse(new Pen(c).Brush,end.X,end.Y,brushThickness,brushThickness);			
					if(g!=null)
					{
						lock(g)
						{
							g.FillRegion(new Pen(Color.AliceBlue).Brush,r);
						}
					}
					FontStyle ff=new FontStyle();
					ff=ff|FontStyle.Bold;
			
					//Font myFont = new Font(textBox1.Font.FontFamily,textBox1.Font.Size,ff);
					if(g!=null)
					{
						lock(g)
						{
							g.DrawString(text,constmyFont,new Pen(Color.White).Brush,p.X + 3 ,p.Y + 12);						
						}
					}
				
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void PlaceHolderArrow(Point p,Color c,String text)",ex,"",false);
			}
		}
	


		//                                "ZAEEM JPG SECTION"
		//******************************************************************************************//
		//******************************************************************************************//
		//******************************************************************************************//
		//******************************************************************************************//
		//******************************************************************************************//
		# region JPG Annotation Section made by Zaeem 
			
		public void Add_Annotation_JPG(ref Graphics g2)
		{

			WhiteboardMessage m=new WhiteboardMessage();
			if(eventStore.Count >0)  
			{						
				for(int i = 0 ; i <eventStore.Count ; i ++)
				{
					Point p = GetCurrentScrollPositionBrowser(browserControl);
					message=(WhiteboardMessage )eventStore[i];	
				
							
					m.c=message.c;
					m.font=message.font;						
					m.tool=message.tool;
					m.text= message.text;
					m.txtSize = message.txtSize;
					m.txtItalics = message.txtItalics;
					m.txtBold = message.txtBold;
					m.txtUnderline =message.txtUnderline;
					m.FontFamily = textBox1.Font.Name;
					m.start=message.start;				
					m.pencilLastPoint=message.pencilLastPoint;
					m.end=message.end;	
					m.SlideNo = message.SlideNo;
					m.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
					m.pencilLastPoints = message.pencilLastPoints;   // by kamran
					m.endPoints= message.endPoints;
					m.startPoints= message.startPoints;


					if((ushort)message.tool==(ushort)WhiteboardToolCode.Brush)
					{
						this.CreateMouseMoveEx_JPG(m,ref g2);
					}
					else
					{
						if(this.IsAttendee)
						{
							m.thickness=message.thickness;
							CreateMouseMoveEx_JPG(m,ref g2);
						}
						else
						{
							m.start.Y -= p.Y;  
							m.end.Y -= p.Y; 
							m.start.X -= p.X;
							m.end.X -= p.X;
							m.thickness=message.thickness;
							CreateMouseMoveEx_JPG(m,ref g2);
						}
					}
				}//for loop 
			}// If Statement 
							

		
		}



		private void CreateMouseMoveEx_JPG(WhiteboardMessage msg, ref Graphics g2)//Point mouseDownPos,Point currentPos,WhiteboardToolCode tool,Color c)
		{
			try
			{
				p = GetCurrentScrollPositionBrowser(browserControl);							
				pe = new Point(p.X , p.Y);
				scrollPoints = new Point(p.X , p.Y);
				//System.Diagnostics.Debug.Write("in create mouse move  EX");
			 
				try
				{
					//Trace.WriteLine("current Slide No:" +this.currentSlideNo.ToString  "Next Slide No :");
					//if(currentSlideNo == msg.SlideNo)
				{
					switch((WhiteboardToolCode)msg.tool)
					{	
							
						case WhiteboardToolCode.Ellipse:
							if(IsAttendee)
							{
								p.X = msg.start.X - p.X;
								p.Y = msg.start.Y - p.Y;					
								pe.X = msg.end.X - pe.X;
								pe.Y = msg.end.Y - pe.Y ;
								//DrawEllipse(msg.start,msg.end,msg.c, msg.thickness);
								DrawEllipse_JPG(p,pe,msg.c, msg.thickness,ref g2);
							}
							else
								DrawEllipse_JPG(msg.start,msg.end,msg.c, msg.thickness,ref g2);
							break;
						case WhiteboardToolCode.Eraser:
							break;
						case WhiteboardToolCode.Line:
							DrawLine_JPG(msg.start,msg.end,msg.c, msg.thickness,ref g2);
							break;
						
							case WhiteboardToolCode.Brush:
							
							//if(IsAttendee)  // isAttendee is true
							for(int i=0; i<msg.endPoints.Count ; i++ )
							{
								p.X = ((Point)msg.pencilLastPoints[i]).X - scrollPoints.X;
								p.Y = ((Point)msg.pencilLastPoints[i]).Y - scrollPoints.Y;					
							
								pe.X = ((Point)msg.endPoints[i]).X - scrollPoints.X;
								pe.Y = ((Point)msg.endPoints[i]).Y - scrollPoints.Y;

								this.DrawBrush1_JPG(p,pe,msg.c, this.brushThickness,ref g2);
								//msg.pencilLastPoint,msg.end,msg.c,msg.thickness
								//this.DrawBrush1(msg.pencilLastPoint,msg.end,msg.c, msg.thickness);
								

								this._mouseX=pe.X ;  // by kamran check mouse move syncronyzation
								this._mousey=pe.Y ;
								DrawMouse(ref g2);
							}
						
								
							break;
							
						case WhiteboardToolCode.Pencil:
								
						{
							if(msg.endPoints.Count>0)
							{
								for(int i=0; i<msg.endPoints.Count ; i++ )
								{
									p.X = ((Point)msg.pencilLastPoints[i]).X - scrollPoints.X;
									p.Y = ((Point)msg.pencilLastPoints[i]).Y - scrollPoints.Y;					
							
									pe.X = ((Point)msg.endPoints[i]).X - scrollPoints.X;
									pe.Y = ((Point)msg.endPoints[i]).Y - scrollPoints.Y;
									DrawPencil_JPG(p,pe,msg.c, msg.thickness,ref g2);

									this._mouseX=pe.X ;  // by kamran check mouse move syncronyzation
									this._mousey=pe.Y ;
									DrawMouse(ref g2);
								}
							}
							else
							{
								p.X = msg.pencilLastPoint.X - p.X;
								p.Y = msg.pencilLastPoint.Y - p.Y;					
								DrawPencil_JPG(p,msg.end ,msg.c, msg.thickness,ref g2);
							}
					
								
						}
							break;
							
						case WhiteboardToolCode.Rectangle:
							DrawRectangle_JPG(msg.start,msg.end,msg.c, msg.thickness,ref g2);
							break;
							
						case WhiteboardToolCode.Text:
							if((msg.text!=null)&&(msg.txtSize!=0.0)&&(msg.text!=""))
								DrawText_JPG(msg.start,msg.text,msg.FontFamily,msg.txtSize,msg.txtBold,msg.txtItalics,msg.txtUnderline,msg.c,ref g2);
							break;
							
						case WhiteboardToolCode.UniArrow:
							if(IsAttendee)
							{
								p.X = msg.start.X - p.X;
								p.Y = msg.start.Y - p.Y;					
								pe.X = msg.end.X - pe.X;
								pe.Y = msg.end.Y - pe.Y ;
								DrawUniArrow_JPG(p,pe,msg.c,msg.thickness,ref g2);
								//DrawUniArrow(msg.start,msg.end,msg.c,msg.thickness);
							}
							else
								DrawUniArrow_JPG(msg.start,msg.end,msg.c,msg.thickness,ref g2);
							break;
							
						case WhiteboardToolCode.PlaceHolderArrow:
							if(IsAttendee)
							{
								p.X = msg.start.X - p.X;
								p.Y = msg.start.Y - p.Y;					
								PlaceHolderArrow_JPG(p,msg.c,msg.text,ref g2);
							
							}
							else
								PlaceHolderArrow_JPG(msg.start,msg.c,msg.text,ref g2);

							break;
			
						default:
							break;
					}
				}
				}//end of try
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void CreateMouseMoveEx(WhiteboardMessage msg)",ex,"",false);
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void CreateMouseMoveEx(WhiteboardMessage msg)",ex,"",false);
			}
		}
	


		/// <summary>
		/// This section is added by zaeem for to show the annotation on the Jpg image. 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="c"></param>
		/// <param name="text"></param>
		/// <param name="g2"></param>
 
		#region PlaceHolderArrow_JPG
		private void PlaceHolderArrow_JPG(Point p,Color c,String text, ref Graphics g2)
		{			
			try
			{
				constFont=new FontStyle();
				constFont=constFont|FontStyle.Bold;
				//Font myFont = new Font(textBox1.Font.FontFamily,textBox1.Font.Size,ff);
				constmyFont = new Font("Microsoft Sans Serif",8,constFont);
				
			
				int width=text.Length * 5 + 20;
				p.X=p.X - width - 19;
				p.Y=p.Y- 19;
			
				Point[] points=new Point[7];
				points[0]=new Point(p.X ,p.Y+10);
				points[1]=new Point(p.X +width,p.Y+10);
				points[2]=new Point(p.X +width,p.Y+1);
				points[3]=new Point(p.X +width+19,p.Y+19);
				points[4]=new Point(p.X +width,p.Y+37);
				points[5]=new Point(p.X +width,p.Y+28);
				points[6]=new Point(p.X ,p.Y+28);
			
				GraphicsPath path = new GraphicsPath();
				path.AddPolygon(points);
				Region r=new Region(path);
			
				//g.FillEllipse(new Pen(c).Brush,end.X,end.Y,brushThickness,brushThickness);			
				if(g2!=null)
				{
					lock(g2)
					{
						g2.FillRegion(new Pen(c).Brush,r);
					}
				}
				FontStyle ff=new FontStyle();
				ff=ff|FontStyle.Bold;
			
				//Font myFont = new Font(textBox1.Font.FontFamily,textBox1.Font.Size,ff);
				if(g2!=null)
				{
					lock(g2)
					{
						g2.DrawString(text,constmyFont,new Pen(Color.White).Brush,p.X + 3 ,p.Y + 12);						
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void PlaceHolderArrow(Point p,Color c,String text)",ex,"",false);
			}
		}


		# endregion 


		private void DrawText_JPG(Point pos,string text,Font f,Color c, ref Graphics g2)
		{
			try
			{				
				//System.Diagnostics.Debug.Write("Draw string calling");
				if(g2!=null)
				{
					lock(g2)
					{
						g2.DrawString(text,f,(new Pen(c)).Brush,pos);
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawText(Point pos,string text,Font f,Color c)",ex,"",false);
			}
			//this.Refresh();
		}
		private void DrawBrush_JPG(Point p,Point start, Color c, ref Graphics g2)
		{		
			try
			{
				if(g2!=null)
				{
					lock(g2)
					{
						g2.FillEllipse(new Pen(c).Brush,p.X,p.Y,brushThickness,brushThickness);			
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawBrush(Point p,Point start, Color c)",ex,"",false);
			}
		}
		private void DrawPencil_JPG(Point start,Point end,Color c,int thickness, ref Graphics g2)
		{
			try
			{
				if(g2!=null)
				{
					lock(g2)
					{
						g2.DrawLine((new Pen(c,thickness)),start,end);
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawPencil(Point start,Point end,Color c,int thickness)",ex,"",false);
			}
		}
		private void DrawUniArrow_JPG(Point start,Point end,Color c,int thickness, ref Graphics g2)
		{			
			try
			{
				Point pFrom = start;
				Point pBase = new Point(0,0);
				Point[] aptPoly=new Point[3];
				float[] vecLine = new float[2];
				float[] vecLeft=  new float[2];;
				float fLength;
				float th;
				float ta;

				// get from point
		

				// set to point
				aptPoly[0].X = end.X;
				aptPoly[0].Y = end.Y;

				// build the line vector
				vecLine[0] = (float) aptPoly[0].X - pFrom.X;
				vecLine[1] = (float) aptPoly[0].Y - pFrom.Y;

				// build the arrow base vector - normal to the line
				vecLeft[0] = -vecLine[1];
				vecLeft[1] = vecLine[0];

				// setup length parameters
				fLength = (float) Math.Sqrt(vecLine[0] * vecLine[0] + vecLine[1] * vecLine[1]);
				th = pA.nWidth / (2.0f * fLength);
				ta = pA.nWidth / (2.0f * ((float)Math.Tan(pA.fTheta) / 2.0f) * fLength);

				// find the base of the arrow
				pBase.X = (int) (aptPoly[0].X + -ta * vecLine[0]);
				pBase.Y = (int) (aptPoly[0].Y + -ta * vecLine[1]);

				// build the points on the sides of the arrow
				aptPoly[1].X = (int) (pBase.X + th * vecLeft[0]);
				aptPoly[1].Y = (int) (pBase.Y + th * vecLeft[1]);
				aptPoly[2].X = (int) (pBase.X + -th * vecLeft[0]);
				aptPoly[2].Y = (int) (pBase.Y + -th * vecLeft[1]);


				Point  p6 = new Point(pBase.X, pBase.Y);
				Point p7= new Point(aptPoly[1].X, aptPoly[1].Y);
				Point p3= new Point (aptPoly[0].X, aptPoly[0].Y);
				Point p4= new Point(aptPoly[2].X, aptPoly[2].Y);
				Point p5= new Point(pBase.X, pBase.Y);
				if(g2!=null)
				{
					lock(this)
					{
						g2.FillPolygon((new Pen(c,this.lineThickness)).Brush,new Point[]{p6,p7,p3,p4,p5});
						g2.DrawLine((new Pen(c)),start,end);
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawUniArrow(Point start,Point end,Color c,int thickness)",ex,"",false);
			}
	

		}
		private void DrawEraser_JPG(Point p)
		{
			try
			{
				Rectangle rd=new Rectangle(p.X,p.Y,p.X+eraser.Width,p.Y+eraser.Height);
				Win32.USER32.InvalidateRect(browserHandle,ref rd,true);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawEraser(Point p)",ex,"",false);
			}

		
		}
		public void DrawText_JPG(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c, ref Graphics g2)
		{
			try
			{
				FontStyle ff=new FontStyle();
				if(bold)
					ff=ff|FontStyle.Bold;
				if(italics)
					ff=ff|FontStyle.Italic;
				if(underline)
					ff=ff|FontStyle.Underline;           
				try
				{
					if(fontFamily!=null)
					{
						Font f = new Font(fontFamily,size,ff);
						DrawText_JPG(pos,text,f,c, ref g2);
					}
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void DrawText(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c)",ex,"",false);
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void DrawText(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c)",ex,"",false);
			}
		}

		private void DrawLine_JPG(Point start, Point end, Color c, int thickness, ref Graphics g2)
		{
			try
			{				
				if(g2!=null)
				{
					lock(g2)
					{
						g2.DrawLine((new Pen(c, thickness)),start,end);
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawLine(Point start, Point end, Color c, int thickness)",ex,"",false);
			}


		}
		private void DrawEllipse_JPG(Point start,Point end,Color c, int thickness, ref Graphics g2)
		{
			try
			{
				int left=Math.Min(start.X,end.X);
				int top=Math.Min(start.Y,end.Y);
				int width=Math.Max(start.X,end.X) - left;
				int height=Math.Max(start.Y,end.Y) - top;
				if(g2!=null)
				{
					lock(g2)
					{
						g2.DrawEllipse((new Pen(c, thickness)),left,top,width,height);
					}
				}

			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawEllipse(Point start,Point end,Color c, int thickness)",ex,"",false);
			}

		}
		private void DrawRectangle_JPG(Point start,Point end,Color c, int thickness, ref Graphics g2)
		{
			try
			{
				int left=Math.Min(start.X,end.X);
				int top=Math.Min(start.Y,end.Y);
				int width=Math.Max(start.X,end.X) - left;
				int height=Math.Max(start.Y,end.Y) - top;
				if(g2!=null)
				{
					lock(g2)
					{
						g2.DrawRectangle((new Pen(c, thickness)),left,top,width,height);
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawRectangle(Point start,Point end,Color c, int thickness)",ex,"",false);
			}
		}

		private void DrawBrush1_JPG(Point start, Point end, Color c,int thickness,ref Graphics g2)
		{		
			//Rectangle r=new Rectangle(Math.Min(start.X,p.X),Math.Min(start.Y,p.Y),Math.Max(start.X,p.X)-Math.Min(start.X,p.X),Math.Max(start.Y,p.Y)-Math.Min(start.Y,p.Y));
			//g.FillEllipse((new Pen(c)).Brush,r);
			try
			{
				Point[] points=new Point[4];
				int thick=thickness/2;
				points[0]=new Point(start.X - thick, start.Y-thick);//start.X - brushThickness/2,start.Y - brushThickness/2);
				points[1]=new Point(start.X+thick,start.Y+thick);//,maxY);//start.X - brushThickness/2,start.Y + brushThickness/2);			
				points[3]=new Point(end.X-thick,end.Y-thick);//maxX,minY);//end.X + brushThickness/2,end.Y + brushThickness/2);
				points[2]=new Point(end.X+thick,end.Y+thick);//,maxY);//end.X + brushThickness/2,end.Y - brushThickness/2);

				//GraphicsPath path = new GraphicsPath();
				//g.DrawPolygon(points); 
				if(g2!=null)
				{
					lock(g2)
					{				
						g2.DrawPolygon(new Pen(c),points); 				
						g2.FillPolygon(new Pen(c).Brush,points,System.Drawing.Drawing2D.FillMode.Winding); 
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawBrush1(Point start, Point end, Color c,int thickness)",ex,"",false);
			}


			//g.FillEllipse(new Pen(c).Brush,start.X,start.Y,brushThickness,brushThickness);			
			
			
			//FillRegion(new Pen(c).Brush,r);

			/*	points[0]=new Point(start.X + thick, start.Y-thick);//start.X - brushThickness/2,start.Y - brushThickness/2);
				points[2]=new Point(start.X-thick,start.Y-thick);//,maxY);//start.X - brushThickness/2,start.Y + brushThickness/2);			
				points[3]=new Point(end.X-thick,end.Y-thick);//maxX,minY);//end.X + brushThickness/2,end.Y + brushThickness/2);
				points[1]=new Point(end.X+thick,end.Y-thick);//,maxY);//end.X + brushThickness/2,end.Y - brushThickness/2);
				path=new GraphicsPath();
			
				path.AddPolygon(points);
				r=new Region(path);			
				//g.FillEllipse(new Pen(c).Brush,end.X,end.Y,brushThickness,brushThickness);			
				g.FillRegion(new Pen(c).Brush,r);
	*/


			//pencilLastPoint=p;


		}	
	
	


		# endregion 

		//******************************************************************************************//
		//******************************************************************************************//
		//******************************************************************************************//
		//******************************************************************************************//

		void RepaintObjects(int clientIdToSkip )
		{			
			try
			{
				Point bpt=new Point(0);
				bpt = GetCurrentScrollPositionBrowser(browserControl);							
				//Trace.WriteLine(DrawNow.ToString());	
				if(NetworkManager.getInstance().profile.clientType != ClientType.ClientAttendee)
					this.IsAttendee=false;
				else
					this.IsAttendee =true;
				try
				{
				
					if(DrawNow)
					{
						//System.Diagnostics.Debug.Write("in repaint object");
						DrawNow = false;
						Win32.USER32.RedrawWindow(browserHandle,IntPtr.Zero,IntPtr.Zero,Win32.USER32.RDW_INVALIDATE | Win32.USER32.RDW_UPDATENOW | Win32.USER32.RDW_ALLCHILDREN);						
					
						
						DrawMouse(ref g);
						WhiteboardMessage m=new WhiteboardMessage();
						previousStartPencilPoint=new Point(0);
						//Trace.AutoFlush=true;  
						//Trace.WriteLine("no of values in eventstore : " + this.eventStore.Count.ToString() );
						if(eventStore.Count >0)  
						{						
							for(int i = 0 ; i <eventStore.Count ; i ++)
							{
								Point p = GetCurrentScrollPositionBrowser(browserControl);
								message=(WhiteboardMessage )eventStore[i];	
								if(message.SenderID == clientIdToSkip)
								{
									eventStore.RemoveAt(i);
									i--;
									continue;
								}
								//Trace.WriteLine("Event Store = "	+ eventStore.Count);

							
							
								m.c=message.c;
								m.font=message.font;						
								m.tool=message.tool;
								m.text= message.text;
								m.txtSize = message.txtSize;
								m.txtItalics = message.txtItalics;
								m.txtBold = message.txtBold;
								m.txtUnderline =message.txtUnderline;
								m.FontFamily = textBox1.Font.Name;
								m.start=message.start;				
								m.pencilLastPoint=message.pencilLastPoint;
								m.end=message.end;	
								m.SlideNo = message.SlideNo;
								m.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;

								m.pencilLastPoints = message.pencilLastPoints;   // by kamran
								m.endPoints= message.endPoints;
								m.startPoints= message.startPoints;

								if((ushort)message.tool==(ushort)WhiteboardToolCode.Eraser)
								{
									//Trace.WriteLine("End Points : " + msg.endPoints.ToString());
									// Trace.WriteLine("End  " + msg.end.ToString());
									//Trace.WriteLine("Pencil Last Points" + msg.pencilLastPoint.ToString());
									//Trace.WriteLine("Start " + msg.start.ToString());
									//Trace.WriteLine("Start Poits" + msg.startPoints.ToString());

									this.DrawEraser(msg.end);
									if(currentSlideNo == message.SlideNo)
									{
										//Trace.WriteLine("prev point : " + previousStartPencilPoint.ToString()  + " ," + message.ToString());
										//Trace.WriteLine("starting point of pencil" + message.startPoints.ToString() );
										//Trace.WriteLine("how much loop runs " + message.endPoints.Count.ToString());
										if( previousStartPencilPoint !=  message.start )
										{
											//p = GetCurrentScrollPositionBrowser(browserControl);
											for(int k=0; k<message.endPoints.Count ; k++ ) 
											{
												p=bpt;
												//m.pencilLastPoint = ((Point)message.pencilLastPoints[k ]);
												m.end = ((Point)message.endPoints[k ]);
												//m.start.Y -= p.Y;  
												m.end.Y -= p.Y; 
												//m.start.X -= p.X;
												m.end.X -= p.X;
												//m.thickness=message.thickness;
												this.DrawEraser(m.end); 

												//p.X = m.pencilLastPoint.X - p.X;
												//p.Y = m.pencilLastPoint.Y - p.Y;					
										
												//DrawPencil(p,m.end ,m.c, m.thickness);
												//CreateMouseMoveEx(m);
												//Trace.WriteLine("inner for loop" ); 
											}
											p=bpt;
										}
										previousStartPencilPoint = message.start;
									}
								}
								else if((ushort)message.tool==(ushort)WhiteboardToolCode.Pencil)
								{
									if(currentSlideNo == message.SlideNo)
									{
										//Trace.WriteLine("prev point : " + previousStartPencilPoint.ToString()  + " ," + message.ToString());
										//Trace.WriteLine("starting point of pencil" + message.startPoints.ToString() );
										//Trace.WriteLine("how much loop runs " + message.endPoints.Count.ToString());
										if( previousStartPencilPoint !=  message.start )
										{
											//p = GetCurrentScrollPositionBrowser(browserControl);
											for(int k=0; k<message.endPoints.Count ; k++ ) 
											{
												p=bpt;
												m.pencilLastPoint = ((Point)message.pencilLastPoints[k ]);
												m.end = ((Point)message.endPoints[k ]);
												m.start.Y -= p.Y;  
												m.end.Y -= p.Y; 
												m.start.X -= p.X;
												m.end.X -= p.X;
												m.thickness=message.thickness;
											
												p.X = m.pencilLastPoint.X - p.X;
												p.Y = m.pencilLastPoint.Y - p.Y;					
					
												DrawPencil(p,m.end ,m.c, m.thickness);
												//CreateMouseMoveEx(m);
												//Trace.WriteLine("inner for loop" ); 
											}
											p=bpt;
										}
										previousStartPencilPoint = message.start;
									}
								}
								else if((ushort)message.tool==(ushort)WhiteboardToolCode.Brush)
								{
									this.CreateMouseMoveEx(m);
								}
								else
								{
									if(this.IsAttendee)
									{
										m.thickness=message.thickness;
										CreateMouseMoveEx(m);
									}
									else
									{
									
										m.start.Y -= p.Y;  
										m.end.Y -= p.Y; 
										m.start.X -= p.X;
										m.end.X -= p.X;
										m.thickness=message.thickness;
										CreateMouseMoveEx(m);
									}
								}
							
								///if(IsAttendee)
								//{
								//	m.thickness=message.thickness;
								//	CreateMouseMoveEx(m);
								//}
							}
						}
				
						DrawNow = false;
					}
					else
					{				
						//	SendMessage(browserControl.Handle,WM_ERASEBKGND, new IntPtr(),new IntPtr());
						//	SendMessage(browserControl.Handle,WM_PAINT, new IntPtr(),new IntPtr());
					}
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  void RepaintObjects(int clientIdToSkip )",ex,"",false);
					//MeetingAlerts alert=new MeetingAlerts();
					//alert.ShowMessage(WebMeeting.Client.Alerts.MeetingAlerts.AlertType.NonFatal,ee.Message,true,false);
					//MessageBox.Show(ee.Message);
					DrawNow = false;
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  void RepaintObjects(int clientIdToSkip )",ex,"",false);
			}

			
		}
		

		#region Utitlity functions for Scrolling
		private mshtml.IHTMLDocument2 getFrameDoc(mshtml.IHTMLDocument2 doc, string frameName)
		{
			try
			{
				object oI;
				int len = (int)doc.frames.length;
				for (int i = 0; i < len; i++) //stupid doc.frames doesn't implement IEnumerable
				{
					oI = i; //stupid doc.frames.item requires 'ref object' instead of int.
					mshtml.IHTMLWindow2 frame = (mshtml.IHTMLWindow2)doc.frames.item(ref oI);
					if (frame.name == frameName)
						return frame.document;
				}
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private mshtml.IHTMLDocument2 getFrameDoc(mshtml.IHTMLDocument2 doc, string frameName)",(new ArgumentException("No frame found")),"No frame found",true);
				//throw new ArgumentException("No frame found");
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private mshtml.IHTMLDocument2 getFrameDoc(mshtml.IHTMLDocument2 doc, string frameName)",ex,"",false);
				
			}
			return null;

		}
		private mshtml.IHTMLWindow2 getFrame(mshtml.IHTMLDocument2 doc, string frameName)
		{
			try
			{
				object oI;
				int len = (int)doc.frames.length;
				for (int i = 0; i < len; i++) //stupid doc.frames doesn't implement IEnumerable
				{
					oI = i; //stupid doc.frames.item requires 'ref object' instead of int.
					mshtml.IHTMLWindow2 frame = (mshtml.IHTMLWindow2)doc.frames.item(ref oI);
					if (frame.name == frameName)
						return frame;
				}
				throw new ArgumentException("No frame found");
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private mshtml.IHTMLWindow2 getFrame(mshtml.IHTMLDocument2 doc, string frameName)",ex,"",false);
				return null;
			}
		}
		public int  GetExcelLowerFrameHeight(AxSHDocVw.AxWebBrowser browser)
		{
			return 18;
			/*
						int Height = 0;

						try
						{
			
							IHTMLDocument2 pDoc;
								
				
							pDoc = (IHTMLDocument2) browser.Document;
							if(IsExcel)
							{
								try
								{
									IHTMLWindow2 wnd = getFrame(pDoc,"frTabs");
									if(wnd != null)
									{
										return wnd.screen.height;
									}


						
								}
								catch(Exception ee)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage(ee.Message);
						
								}
                    
							}												
			

						}
						catch(Exception ee)
						{
							ee = ee;
							MessageBox.Show(ee.Message);
						}

						return Height;
						*/
		}

		
		public Point GetTotalScrollHeightBrowser(AxSHDocVw.AxWebBrowser browser) 
		{
			
			try
			{
				IHTMLDocument2 pDoc;
				IHTMLElement2 pBody;
				pDoc = (IHTMLDocument2) browser.Document;
				pBody = (IHTMLElement2) pDoc.body;	

				int currentY =  pBody.scrollTop;
				int real_scroll_height = 0;
				pBody.scrollTop =  20000000 ; // ask to scroll really far down...
				real_scroll_height = pBody.scrollTop;
				real_scroll_height += browser.Height; // will return the scroll height

				int currentX = pBody.scrollLeft;
				int real_scroll_width = 0;
				pBody.scrollLeft =  20000000 ; // ask to scroll really far down...
				real_scroll_width = pBody.scrollLeft;
				real_scroll_width += browser.Width;
            
				Point pt = new Point();
				pt.X = real_scroll_width;
				pt.Y = real_scroll_height;
   
				pBody.scrollTop = currentY;
				pBody.scrollLeft = currentX;
            

				return pt;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public Point GetTotalScrollHeightBrowser(AxSHDocVw.AxWebBrowser browser)",ex,"",false);
				return new Point(0,0);
			}
		}
		
		
		public Point getBodyClientHeight()
		{
			Point pt = new Point();
			try
			{	
			
				
				IHTMLDocument2 pDoc;
				IHTMLElement2 pBody;

				if(this.browserControl == null)
					return pt;
				
				if(this.browserControl.Name == null)
					return pt;							
				
				pDoc = (IHTMLDocument2) this.browserControl.Document;
				//pDoc.body.style. 
				if(IsExcel)
				{
					try
					{
						pDoc = getFrameDoc(pDoc,"frSheet");
					}
					catch(Exception ee)
					{
						ee = ee;
						
					}
                    
				}	
			
				pBody = (IHTMLElement2) pDoc.body;						
				
				//((IHTMLDocument2) browserControl.Document).execCommand("alert(document.body.scrollTop)",true,new object);
				//t X = GetScrollPos((int)browserControl.HWND,SB_VERT );
				
					
				pt.Y=pBody.clientHeight; 
				//pt.Y	= 0; // by kamran for checking 
				pt.X=pBody.clientWidth;
				
//				Trace.WriteLine("this.browserControl.Width  : " +this.browserControl.Width.ToString());
//				Trace.WriteLine("this.browserControl.Height  : " +this.browserControl.Height.ToString());
//
//				Trace.WriteLine("pBody.clientTop  : " + pBody.clientTop.ToString());
//				Trace.WriteLine("pBody.clientHeight  : " + pBody.clientHeight.ToString());
//				Trace.WriteLine("pBody.clientLeft   : " + pBody.clientLeft.ToString());
//				Trace.WriteLine("pBody.clientWidth  : " + pBody.clientWidth.ToString());
//				Trace.WriteLine("pBody.Top : " + pBody.scrollTop.ToString());
//				Trace.WriteLine("pBody.scrollLeft : " + pBody.scrollLeft.ToString());
//				Trace.WriteLine("pBody.scrollWidth : "+ pBody.scrollWidth.ToString());
//				Trace.WriteLine("pBody.scrollHeight : "+ pBody.scrollHeight.ToString());

		
				//	MessageBox.Show("scroll top : "+ pBody.scrollTop.ToString());
				//	MessageBox.Show("scroll Height : "+ pBody.scrollHeight.ToString());
				//	MessageBox.Show("scroll Left : "+ pBody.scrollLeft.ToString());
				//	MessageBox.Show("scroll Width : "+ pBody.scrollWidth.ToString());

			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public Point getBodyClientHeight()",ex,"",false);
			}			
			return pt;
		}
		public Point GetCurrentScrollPositionBrowser(AxSHDocVw.AxWebBrowser browser)
		{
			
			
			Point pt = new Point();
			try
			{	
			
				
				IHTMLDocument2 pDoc;
				IHTMLElement2 pBody;

				if(browser == null)
					return pt;
				
				if(browser.Name == null)
					return pt;							
				
				/*
				 * chcek the browser document
				 */
				if(browser.Document==null)
					return pt;
				pDoc = (IHTMLDocument2) browser.Document;
				//pDoc.body.style. 
				if(IsExcel)
				{
					try
					{
						pDoc = getFrameDoc(pDoc,"frSheet");
					}
					catch(Exception ex)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public Point GetCurrentScrollPositionBrowser(AxSHDocVw.AxWebBrowser browser)",ex,"",false);
					}
                    
				}	
			
				pBody = (IHTMLElement2) pDoc.body;						
				
				//((IHTMLDocument2) browserControl.Document).execCommand("alert(document.body.scrollTop)",true,new object);
				//t X = GetScrollPos((int)browserControl.HWND,SB_VERT );
				
					
				pt.Y = pBody.scrollTop ; 
				//pt.Y	= 0; // by kamran for checking 
				pt.X	= pBody.scrollLeft;
				
//				Trace.WriteLine("this.browserControl.Width  : " +this.browserControl.Width.ToString());
//				Trace.WriteLine("this.browserControl.Height  : " +this.browserControl.Height.ToString());
//
//				Trace.WriteLine("pBody.clientTop  : " + pBody.clientTop.ToString());
//				Trace.WriteLine("pBody.clientHeight  : " + pBody.clientHeight.ToString());
//				Trace.WriteLine("pBody.clientLeft   : " + pBody.clientLeft.ToString());
//				Trace.WriteLine("pBody.clientWidth  : " + pBody.clientWidth.ToString());
//				Trace.WriteLine("pBody.Top : " + pBody.scrollTop.ToString());
//				Trace.WriteLine("pBody.scrollLeft : " + pBody.scrollLeft.ToString());
//				Trace.WriteLine("pBody.scrollWidth : "+ pBody.scrollWidth.ToString());
//				Trace.WriteLine("pBody.scrollHeight : "+ pBody.scrollHeight.ToString());

		
				//	MessageBox.Show("scroll top : "+ pBody.scrollTop.ToString());
				//	MessageBox.Show("scroll Height : "+ pBody.scrollHeight.ToString());
				//	MessageBox.Show("scroll Left : "+ pBody.scrollLeft.ToString());
				//	MessageBox.Show("scroll Width : "+ pBody.scrollWidth.ToString());

			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public Point GetCurrentScrollPositionBrowser(AxSHDocVw.AxWebBrowser browser)",ex,"",false);
			}			
			return pt;

		}
		private void Execute(string code)
		{
			try
			{
				IHTMLDocument2 doc = (IHTMLDocument2) browserControl.Document;

				if (doc != null)
				{
					IHTMLWindow2 parentWindow = doc.parentWindow;
				
					if (parentWindow != null)
						parentWindow.execScript(code, "javascript");
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void Execute(string code)",ex,"",false);
			}
		}

		#endregion
		public void SelectFont()
		{
			try
			{
				FontDialog fd=new FontDialog();
				fd.Font=textBox1.Font;
				fd.ShowDialog();
				textBox1.Font = fd.Font;
				this.localCopyOfFont=fd.Font;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void SelectFont()",ex,"",false);
			}
			

			//this.Font=fd.Font;
		}
		

		void OnPostPaint(System.Drawing.Graphics g)
		{
			try
			{
				switch(this.tool)
				{
					case WhiteboardToolCode.Line:
					{
						if(g!=null)
						{
							lock(g)
							{
								g.DrawLine((new Pen(color,this.lineThickness)),this.mouseDownPos,this.mouseCurrentPos);
							}
						}
						break;
					}
					case WhiteboardToolCode.Rectangle:
					{
						int left=Math.Min(this.mouseDownPos.X,this.mouseCurrentPos.X);
						int top=Math.Min(this.mouseDownPos.Y,this.mouseCurrentPos.Y);
						int width=Math.Max(this.mouseDownPos.X,this.mouseCurrentPos.X) - left;
						int height=Math.Max(this.mouseDownPos.Y,this.mouseCurrentPos.Y) - top;						
						if(g!=null)
						{
							lock(g)
							{
								g.DrawRectangle((new Pen(color, this.lineThickness)),left,top,width,height);
							}
						}
						break;
					}
					case WhiteboardToolCode.Ellipse:
					{
						int left=Math.Min(this.mouseDownPos.X,this.mouseCurrentPos.X);
						int top=Math.Min(this.mouseDownPos.Y,this.mouseCurrentPos.Y);
						int width=Math.Max(this.mouseDownPos.X,this.mouseCurrentPos.X) - left;
						int height=Math.Max(this.mouseDownPos.Y,this.mouseCurrentPos.Y) - top;						
						if(g!=null)
						{						
							lock(g)
							{
								g.DrawEllipse((new Pen(color,this.lineThickness)),left,top,width,height);
							}
						}
						break;
					}
					case WhiteboardToolCode.UniArrow:
					{
						Point pFrom = this.mouseDownPos;
						Point pBase = new Point(0,0);
						Point[] aptPoly=new Point[3];
						float[] vecLine = new float[2];
						float[] vecLeft=  new float[2];;
						float fLength;
						float th;
						float ta;

						// get from point
		

						// set to point
						aptPoly[0].X = this.mouseCurrentPos.X;
						aptPoly[0].Y = this.mouseCurrentPos.Y;

						// build the line vector
						vecLine[0] = (float) aptPoly[0].X - pFrom.X;
						vecLine[1] = (float) aptPoly[0].Y - pFrom.Y;

						// build the arrow base vector - normal to the line
						vecLeft[0] = -vecLine[1];
						vecLeft[1] = vecLine[0];

						// setup length parameters
						fLength = (float) Math.Sqrt(vecLine[0] * vecLine[0] + vecLine[1] * vecLine[1]);
						th = pA.nWidth / (2.0f * fLength);
						ta = pA.nWidth / (2.0f * ((float)Math.Tan(pA.fTheta) / 2.0f) * fLength);

						// find the base of the arrow
						pBase.X = (int) (aptPoly[0].X + -ta * vecLine[0]);
						pBase.Y = (int) (aptPoly[0].Y + -ta * vecLine[1]);

						// build the points on the sides of the arrow
						aptPoly[1].X = (int) (pBase.X + th * vecLeft[0]);
						aptPoly[1].Y = (int) (pBase.Y + th * vecLeft[1]);
						aptPoly[2].X = (int) (pBase.X + -th * vecLeft[0]);
						aptPoly[2].Y = (int) (pBase.Y + -th * vecLeft[1]);


						Point  p6 = new Point(pBase.X, pBase.Y);
						Point p7= new Point(aptPoly[1].X, aptPoly[1].Y);
						Point p3= new Point (aptPoly[0].X, aptPoly[0].Y);
						Point p4= new Point(aptPoly[2].X, aptPoly[2].Y);
						Point p5= new Point(pBase.X, pBase.Y);
						if(g!=null)
						{						
							lock(g)
							{
								g.FillPolygon((new Pen(this.color,this.lineThickness)).Brush,new Point[]{p6,p7,p3,p4,p5});
								g.DrawLine((new Pen(this.color)),this.mouseDownPos,this.mouseCurrentPos);
							}
						}
						break;
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  void OnPostPaint(System.Drawing.Graphics g)",ex,"",false);
			}
				
		}
		
		//WhiteboardMessage msgPencilMessage_onlyMove;// = new WhiteboardMessage();
		void sendPencilPacket()
		{
			try
			{
				this.eventStore.Add(msgPencilMessage);
				//Trace.WriteLine("Event Store of current drawing" + this.eventStore.Count.ToString());
	                		
				AnnotMsg amsg = new AnnotMsg(annotationType,thisSessionID ,NetworkManager.getInstance().profile);								
				amsg.drawingMsg = msgPencilMessage;				
				if(haveFrames)
					amsg.sheetNo = currentSheet;
				else
					amsg.sheetNo = -1;
		
				amsg.SenderID = NetworkManager.getInstance().profile.ClientId;
		
				sendingArray.Add(amsg); // by kamran 
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  void sendPencilPacket()",ex,"",false);
			}

		}


		void sendEraserPacket()
		{
			try
			{
				this.eventStore.Add(msgPencilMessage);
				//Trace.WriteLine("pencil message count" + this.msgPencilMessage.endPoints.Count.ToString());
				//Trace.WriteLine("Eraser" + this.eventStore.Count.ToString());
	                		
				AnnotMsg amsg = new AnnotMsg(annotationType,thisSessionID ,NetworkManager.getInstance().profile);								
				amsg.drawingMsg = msgPencilMessage;				
				if(haveFrames)
					amsg.sheetNo = currentSheet;
				else
					amsg.sheetNo = -1;
		
				amsg.SenderID = NetworkManager.getInstance().profile.ClientId;
		
				sendingArray.Add(amsg); // by kamran 
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  void sendEraserPacket()",ex,"",false);
			}

		}


		void sendBrushPacket()
		{
			try
			{
				this.eventStore.Add(msgPencilMessage);
				//Trace.WriteLine("Event Store of current drawing" + this.eventStore.Count.ToString());
	                		
				AnnotMsg amsg = new AnnotMsg(annotationType,thisSessionID ,NetworkManager.getInstance().profile);								
				amsg.drawingMsg = msgPencilMessage;				
				if(haveFrames)
					amsg.sheetNo = currentSheet;
				else
					amsg.sheetNo = -1;
		
				amsg.SenderID = NetworkManager.getInstance().profile.ClientId;
		
				sendingArray.Add(amsg); // by kamran 
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  void sendBrushPacket()",ex,"",false);
			}
		}

		public void DefaultMethod()
		{
			
			try
			{
				HTMLDocument = (IHTMLDocument2) this.browserControl.Document;
				HTMLWindow2 win = (HTMLWindow2)this.HTMLDocument.parentWindow; 
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void DefaultMethod()",ex,"",false);
			}
			//Trace.WriteLine("Object: " + win.@event.srcElement + ", Type: " + win.@event.type);
		
		}
	
		
		void NewOnMouseUp(Point e)
		{
			try
			{
				color = NetworkManager.getInstance().profile.m_AssignedColor;
				//pointsDrawByPencil = 0;
				//this.bScrollDraw=true; 
				mouseCurrentPos.X=e.X;
				mouseCurrentPos.Y=e.Y;								
			
				//if(((ushort)tool & (ushort)(WhiteboardToolCode.PlaceHolderArrow|WhiteboardToolCode.Brush | WhiteboardToolCode.Pencil |  WhiteboardToolCode.Eraser | WhiteboardToolCode.Brush)) == (ushort)tool)
				if(((ushort)tool & (ushort)(WhiteboardToolCode.PlaceHolderArrow|WhiteboardToolCode.Brush | WhiteboardToolCode.Pencil |  WhiteboardToolCode.Eraser )) == (ushort)tool)
				{
					//WhiteboardMessage msg=new WhiteboardMessage(); 
				
					msgPencilMessage.start=mouseDownPos;
					Point p = GetCurrentScrollPositionBrowser(browserControl);
					msgPencilMessage.start.X =  mouseDownPos.X-p.X;
					msgPencilMessage.start.Y =  mouseDownPos.Y-p.Y;
					msgPencilMessage.end.X=e.X;
					msgPencilMessage.end.Y=e.Y;  
					msgPencilMessage.tool=(ushort)tool;	
			
					msgPencilMessage.tool=(ushort)tool;	

					msgPencilMessage.thickness=this.lineThickness;						
					msgPencilMessage.c=this.color;				

					//				if(tool.ToString().ToLower()=="pencil")
					//					msgPencilMessage.thickness=this.lineThickness;						
					//				else if(tool.ToString().ToLower()=="brush")
					//					msgPencilMessage.thickness=this.brushThickness;
					//				msgPencilMessage.c=this.color;				 
					msgPencilMessage.pencilLastPoint=new Point(this.pencilLastPoint.X,this.pencilLastPoint.Y);
					msgPencilMessage.SenderID = NetworkManager.getInstance().profile.ClientId;
					msgPencilMessage.text=WebMeeting.Client.NetworkManager.getInstance().profile.Name;
					//////////////////////////////////
					//++pointsDrawByPencil;
					msgPencilMessage.pencilLastPoints.Add(this.pencilLastPoint) ;
					msgPencilMessage.endPoints.Add(msgPencilMessage.end) ;
					msgPencilMessage.SlideNo = currentSlideNo;
					//MessageBox.Show("paks");
					/////////////////////////////////////////////
					this.CreateMouseMoveEx(msgPencilMessage); 
					//				if(this.tool!=WhiteboardToolCode.Eraser)
					//				{
					int yx,yy;
					if(e.X > (mouseDownPos.X))				
						yx=e.X-(mouseDownPos.X);	
					else
						yx=e.X-(mouseDownPos.X);
				
					if(e.Y > (mouseDownPos.Y))				
						yy=e.Y-(mouseDownPos.Y);	
					else
						yy=e.Y-(mouseDownPos.Y);

				
					//yy=mouseDownPos.Y-e.Y;
					msgPencilMessage.start=mouseDownPos;
				
					msgPencilMessage.end.X=mouseDownPos.X+yx+p.X;  
					msgPencilMessage.end.Y=mouseDownPos.Y+yy+p.Y;  
					//				msgPencilMessage.end.Y=e.X-p.Y;
					//				msgPencilMessage.end.X=e.Y-p.X;

					//Point p = GetCurrentScrollPositionBrowser(browserControl);
					//					msgPencilMessage.end=new Point(e.X + p.X ,e.Y + p.Y );  
					//				}


					//			if(e.X > mouseDownPos.X)
					//					msgPencilMessage.end.X=e.X-mouseDownPos.X;
					//				else
					//					msgPencilMessage.end.X=mouseDownPos.X-e.X;
					//
					//				if(e.Y > mouseDownPos.Y)
					//					msgPencilMessage.end.Y=e.Y-mouseDownPos.Y;
					//				else
					//					msgPencilMessage.end.Y=mouseDownPos.Y-e.Y;
				
					//				msgPencilMessage.start.X =  mouseDownPos.X;
					//				msgPencilMessage.start.Y =  mouseDownPos.Y;
					//				msgPencilMessage.end=new Point(e.X +p.X,e.Y+p.Y);  
					this.eventStore.Add(msgPencilMessage);
					//Trace.WriteLine(this.eventStore.Count.ToString());

                		
					AnnotMsg amsg = new AnnotMsg(annotationType,thisSessionID,NetworkManager.getInstance().profile);								
					amsg.drawingMsg = msgPencilMessage;
					if(haveFrames)
						amsg.sheetNo = currentSheet;
					else
						amsg.sheetNo = -1;
				
					amsg.SenderID = NetworkManager.getInstance().profile.ClientId;
					sendingArray.Add(amsg);
				}
				else
				{					
					if((ushort)tool!=(ushort)WhiteboardToolCode.Text)
					{
						//msg=new WhiteboardMessage();
						Point p = GetCurrentScrollPositionBrowser(browserControl);

						msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
						msg = new WhiteboardMessage();
						//msg.start=mouseDownPos;
						msg.start.X=mouseDownPos.X-p.X;
						msg.start.Y=mouseDownPos.Y-p.Y;

						msg.end=new Point(e.X,e.Y) ;		
						msg.tool=(ushort)tool;
						msg.c=this.color;	
						msg.thickness=this.lineThickness;
						msg.SenderID = NetworkManager.getInstance().profile.ClientId;
						msg.SlideNo = currentSlideNo;
					

						this.CreateMouseMoveEx(msg); 
						MessageBox.Show(e.X.ToString()+ e.Y.ToString()+mouseDownPos.X.ToString()+mouseDownPos.Y.ToString());
						msg.start=mouseDownPos;
						if(e.X > mouseDownPos.X)
							msg.end.X=e.X-mouseDownPos.X;
						else
							msg.end.X=mouseDownPos.X-e.X;

						if(e.Y > mouseDownPos.Y)
							msg.end.Y=e.Y-mouseDownPos.Y;
						else
							msg.end.Y=mouseDownPos.Y-e.Y;

						MessageBox.Show(msg.end.X.ToString()+ msg.end.Y.ToString()+msg.start.X.ToString()+msg.start.Y.ToString());

						//					msg.end.Y=e.Y;
						//					msg.end.X=e.X;
					
						this.eventStore.Add(msg);

						AnnotMsg amsg = new AnnotMsg(annotationType,thisSessionID,NetworkManager.getInstance().profile);								
						amsg.drawingMsg = msg;
						if(haveFrames)
							amsg.sheetNo = currentSheet;
						else
							amsg.sheetNo = -1;

						amsg.SenderID = NetworkManager.getInstance().profile.ClientId;
						sendingArray.Add(amsg);//NetworkManager.getInstance().SendLoadPacket(amsg);
				
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  void NewOnMouseUp(Point e)",ex,"",false);
			}
		}

		void OnMouseUp(Point e)
		{
			try
			{

				color = NetworkManager.getInstance().profile.m_AssignedColor;
				//pointsDrawByPencil = 0;
				//this.bScrollDraw=true; 
				mouseCurrentPos.X=e.X;
				mouseCurrentPos.Y=e.Y;								
			
				//if(((ushort)tool & (ushort)(WhiteboardToolCode.PlaceHolderArrow|WhiteboardToolCode.Brush | WhiteboardToolCode.Pencil |  WhiteboardToolCode.Eraser | WhiteboardToolCode.Brush)) == (ushort)tool)
				if(((ushort)tool & (ushort)(WhiteboardToolCode.PlaceHolderArrow|WhiteboardToolCode.Brush | WhiteboardToolCode.Pencil |  WhiteboardToolCode.Eraser )) == (ushort)tool)	
				{
					//WhiteboardMessage msg=new WhiteboardMessage(); 
					msgPencilMessage.start=mouseDownPos;
				
					Point p = GetCurrentScrollPositionBrowser(browserControl);
					msgPencilMessage.end=new Point(e.X + p.X ,e.Y + p.Y );  
					msgPencilMessage.tool=(ushort)tool;	
			
					msgPencilMessage.thickness=this.lineThickness;						
					msgPencilMessage.c=this.color;				

					//				if(tool.ToString().ToLower()=="pencil")
					//					msgPencilMessage.thickness=this.lineThickness;						
					//				else if(tool.ToString().ToLower()=="brush")
					//					msgPencilMessage.thickness=this.brushThickness;
					//				msgPencilMessage.c=this.color;				 
					msgPencilMessage.pencilLastPoint=new Point(this.pencilLastPoint.X,this.pencilLastPoint.Y);
					msgPencilMessage.SenderID = NetworkManager.getInstance().profile.ClientId;
					msgPencilMessage.text=WebMeeting.Client.NetworkManager.getInstance().profile.Name;
					//////////////////////////////////
					//++pointsDrawByPencil;
					msgPencilMessage.pencilLastPoints.Add(this.pencilLastPoint) ;
					msgPencilMessage.endPoints.Add(msgPencilMessage.end) ;
					msgPencilMessage.SlideNo = currentSlideNo;
					msgPencilMessage.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID  ;
					/////////////////////////////////////////////
				
					this.eventStore.Add(msgPencilMessage);
                		
					AnnotMsg amsg = new AnnotMsg(annotationType,thisSessionID,NetworkManager.getInstance().profile);								
					amsg.drawingMsg = msgPencilMessage;
					if(haveFrames)
						amsg.sheetNo = currentSheet;
					else
						amsg.sheetNo = -1;
				
					amsg.SenderID = NetworkManager.getInstance().profile.ClientId;
					sendingArray.Add(amsg);
				}
				else
				{					
					if((ushort)tool!=(ushort)WhiteboardToolCode.Text)
					{
						//msg=new WhiteboardMessage();
						Point p = GetCurrentScrollPositionBrowser(browserControl);
						msg = new WhiteboardMessage();
					
						msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
						msg.start=mouseDownPos;
						msg.end=new Point(e.X+p.X,e.Y + p.Y) ;		
						msg.tool=(ushort)tool;
						msg.c=this.color;	
						msg.thickness=this.lineThickness;
						msg.SenderID = NetworkManager.getInstance().profile.ClientId;
						msg.SlideNo = currentSlideNo;
						this.eventStore.Add(msg);
					
					
						AnnotMsg amsg = new AnnotMsg(annotationType,thisSessionID,NetworkManager.getInstance().profile);								
						amsg.drawingMsg = msg;
						if(haveFrames)
							amsg.sheetNo = currentSheet;
						else
							amsg.sheetNo = -1;

						amsg.SenderID = NetworkManager.getInstance().profile.ClientId;
						sendingArray.Add(amsg);//NetworkManager.getInstance().SendLoadPacket(amsg);
				
					}
				}
			}
			catch(Exception ex)	
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  void OnMouseUp(Point e)",ex,"",false);
			}



		}

		void OnMouseDown(Point e)
		{
			try
			{
				msgPencilMessage = new WhiteboardMessage();
				color = NetworkManager.getInstance().profile.m_AssignedColor;
				Point p = GetCurrentScrollPositionBrowser(browserControl);
			
				mouseDownPos=new Point(e.X+p.X,e.Y+p.Y ) ;			
			
				//pencilLastPoint=new Point(e.X+p.X,e.Y+p.Y );
				pencilLastPoint=new Point(e.X+p.X,e.Y + p.Y ); // by kamran fine
			
				if(this.textBox1.Visible==true)
				{
					this.textBox1.Visible=false;
					if (this.textBox1.Text!= "" )
						textBox1_Leave(this,new System.EventArgs());
				
				}
	
				if(this.tool==WhiteboardToolCode.Text)
				{
					//				if(this.textBox1.Text!="")
					//				{
					//					textBox1_Leave(this,new System.EventArgs());
					//				}
				
					this.textBox1.Top=e.Y;
					this.textBox1.Left=e.X;
					this.textBox1.Visible=true;
					this.textBox1.Width=100;
					this.textBox1.BorderStyle=BorderStyle.None;
					browserControl.Controls.Add(textBox1);
					textBox1.BringToFront();
					this.textBox1.Focus();
				

				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  void OnMouseDown(Point e)",ex,"",false);
			}

		}

		void OnMouseMove(Point e)   // call if mouse button is pressed and move mouse
		{			
			try
			{
				 
				color = NetworkManager.getInstance().profile.m_AssignedColor;
				if(((ushort)tool & (ushort)(WhiteboardToolCode.Brush | WhiteboardToolCode.Pencil |  WhiteboardToolCode.Eraser )) == (ushort)tool)
				{
					// draw things by free hand section
					//msg=new WhiteboardMessage();
					//msgPencilMessage = new WhiteboardMessage();

					
					msgPencilMessage.start=mouseDownPos;
					//Trace.WriteLine("Mouse Down Position " + mouseDownPos.ToString());
					Point p = GetCurrentScrollPositionBrowser(browserControl);
				
					if((ushort)tool==(ushort)WhiteboardToolCode.Eraser)							
					{
						msgPencilMessage.end=new Point(e.X+p.X,e.Y+p.Y);
					}
					else
						msgPencilMessage.end=new Point(e.X+p.X,e.Y+p.Y);



					msgPencilMessage.tool=(ushort)tool;    // there are no need of these things check it in mouseUp
					msgPencilMessage.c=this.color;
					/*if(((ushort)tool & (ushort)(WhiteboardToolCode.Brush)) == (ushort)tool)
						msgPencilMessage.thickness=this.brushThickness;
					else*/
				
					if((ushort)tool==(ushort)WhiteboardToolCode.Pencil)
						msgPencilMessage.thickness=this.lineThickness;				
				
					/*if(tool.ToString().ToLower()=="pencil")
						msgPencilMessage.thickness=this.lineThickness;			
					else if(tool.ToString().ToLower()=="brush")
						msgPencilMessage.thickness=this.brushThickness;*/
					msgPencilMessage.pencilLastPoint=new Point(this.pencilLastPoint.X,this.pencilLastPoint.Y);
					msgPencilMessage.SenderID = NetworkManager.getInstance().profile.ClientId;
					msgPencilMessage.text=WebMeeting.Client.NetworkManager.getInstance().profile.Name;
				
					if((ushort)tool==(ushort)WhiteboardToolCode.Pencil)
					{
						if(g!=null)
						{						
							lock(g)
							{
								g.DrawLine((new Pen(msgPencilMessage.c,msgPencilMessage.thickness)),pencilLastPoint.X -p.X,pencilLastPoint.Y - p.Y, msgPencilMessage.end.X -p.X, msgPencilMessage.end.Y - p.Y);
							}
						}
					}
					else if((ushort)tool==(ushort)WhiteboardToolCode.Brush)							
					{
						this.msgPencilMessage.thickness=this.brushThickness; 
						Point ptlast=msgPencilMessage.pencilLastPoint;
						Point ptend=msgPencilMessage.end;
						ptend.X=ptend.X-p.X;
						ptend.Y=ptend.Y-p.Y;

						ptlast.X=ptlast.X-p.X;
						ptlast.Y=ptlast.Y-p.Y;

						DrawBrush1(ptlast,ptend,msgPencilMessage.c,this.brushThickness);

						//	g.DrawLine((new Pen(msgPencilMessage.c,msgPencilMessage.thickness)),pencilLastPoint, msgPencilMessage.end);				
					}
					else if((ushort)tool==(ushort)WhiteboardToolCode.Eraser)							
					{
						//this.msgPencilMessage.end.X= this.msgPencilMessage.end.X-p.X;
						//this.msgPencilMessage.end.Y=this.msgPencilMessage.end.Y-p.Y;
						Point ptend=msgPencilMessage.end;
						ptend.X=ptend.X-p.X;
						ptend.Y=ptend.Y-p.Y;
						this.DrawEraser(ptend);
					}
					////g.DrawLine((new Pen(msgPencilMessage.c,msgPencilMessage.thickness)),pencilLastPoint, msgPencilMessage.end);				    
					//DrawPencil(msgPencilMessage.pencilLastPoint,msgPencilMessage.end,msgPencilMessage.c, msgPencilMessage.thickness);
					
					//DrawBrush(e,msgPencilMessage.pencilLastPoint,msgPencilMessage.c);

					
					
				
					//////////////////////////////////
					//++pointsDrawByPencil;
					msgPencilMessage.pencilLastPoints.Add(this.pencilLastPoint);
					msgPencilMessage.endPoints.Add(msgPencilMessage.end) ;
					msgPencilMessage.SlideNo = currentSlideNo;
					/////////////////////////////////////////////
				
				
				
					//				this.eventStore.Add(msgPencilMessage);
					//				Trace.WriteLine("Event Store of current drawing" + this.eventStore.Count.ToString());
					//                		
					//				AnnotMsg amsg = new AnnotMsg(annotationType,thisSessionID ,NetworkManager.getInstance().profile);								
					//				amsg.drawingMsg = msgPencilMessage;				
					//				if(haveFrames)
					//					amsg.sheetNo = currentSheet;
					//				else
					//					amsg.sheetNo = -1;
					//			
					//				amsg.SenderID = NetworkManager.getInstance().profile.ClientId;
					//
					//
					//				sendingArray.Add(amsg); // by kamran 
				
				
				
					pencilLastPoint.X = msgPencilMessage.end.X; // by kamran
					pencilLastPoint.Y = msgPencilMessage.end.Y ; // by kamran
				}
				else
				{
					// draw lines other than pencil
					Point p = GetCurrentScrollPositionBrowser(browserControl);
					WhiteboardMessage msg=new WhiteboardMessage();							
					msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
					msg.start=mouseDownPos;				
					msg.end=new Point(e.X + p.X ,e.Y + p.Y);				
					//msg.end=new Point(e.X + p.X ,e.Y );				
					msg.tool=(ushort)tool;
					msg.c=this.color;	
					msg.thickness=this.lineThickness;
					msg.SenderID = NetworkManager.getInstance().profile.ClientId;

					WhiteboardMessage m=new WhiteboardMessage();
					m.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
					m.c=msg.c;
					m.font=msg.font;						
					m.tool=msg.tool;
					m.text= msg.text;
					m.txtSize = msg.txtSize;
					m.txtItalics = msg.txtItalics;
					m.txtBold = msg.txtBold;
					m.thickness = this.lineThickness;
					m.txtUnderline =msg.txtUnderline;
					m.FontFamily = textBox1.Font.Name;
					m.start=msg.start;				
					m.pencilLastPoint=msg.pencilLastPoint;
					m.end=msg.end;
					m.SlideNo = currentSlideNo;
						
					m.start.Y -= p.Y;
					m.end.Y -= p.Y;

					m.start.X -= p.X;
					m.end.X -= p.X;

					//	CreateMouseMove(m);
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  void OnMouseMove(Point e)",ex,"",false);
			}
		}
	


		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				//in Old case 
				//this.textBox1.Width =Math.Min((int)( this.textBox1.Text.Length * this.textBox1.Font.Size /1.5) + 10,browserControl.Width - this.textBox1.Left);						
				//Zaeem Change
				this.textBox1.Width =Math.Min((int)( this.textBox1.Text.Length * this.localCopyOfFont.Size /1.5) + 10,browserControl.Width - this.textBox1.Left);						
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void textBox1_TextChanged(object sender, System.EventArgs e)",ex,"",false);
			}
		}

		private void textBox1_Leave(object sender, System.EventArgs e)
		{			
			try
			{
				string textboxvalue=this.textBox1.Text;
				WhiteboardMessage msg=new WhiteboardMessage();

				Point p = GetCurrentScrollPositionBrowser(browserControl);
			

				msg.start=this.textBox1.Location;//mouseDownPos;
				msg.start.X += p.X;
				msg.start.Y += p.Y;
				msg.end=this.textBox1.Location;
				msg.end.X += p.X;
				msg.end.Y += p.Y;

				msg.tool=(ushort)WhiteboardToolCode.Text;//tool;
				msg.c=this.color;
				//msg.font=textBox1.Font;
				msg.font=this.localCopyOfFont;

				msg.thickness=this.lineThickness;
				msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
			
				msg.text=textboxvalue;
				
//				msg.txtSize = textBox1.Font.Size;
//				msg.txtItalics = textBox1.Font.Italic;
//				msg.txtBold = textBox1.Font.Bold;
//				msg.txtUnderline = textBox1.Font.Underline;
//				msg.FontFamily = textBox1.Font.Name;
//				

				msg.txtSize = this.localCopyOfFont.Size;
				msg.txtItalics = this.localCopyOfFont.Italic;
				msg.txtBold = this.localCopyOfFont.Bold;
				msg.txtUnderline = this.localCopyOfFont.Underline;
				msg.FontFamily = this.localCopyOfFont.Name;
				

				msg.SlideNo=currentSlideNo;
				//Trace.WriteLine("no of Eventstore before msg writtign text" + this.eventStore.Count);
				msg.SenderID=NetworkManager.getInstance().profile.ClientId;
				this.eventStore.Add(msg);
				//Trace.WriteLine("no of Eventstore after msg  writtign text" + this.eventStore.Count);
				//textboxvalue=this.textBox1.Text;
				this.textBox1.Visible=false;
				this.textBox1.Text="";
			
				//msg.start.X -= p.X;
				//msg.start.Y -= p.Y;
				//msg.end=this.textBox1.Location;
				//msg.end.X -= p.X;
				//msg.end.Y -= p.Y;
				//this.CreateMouseMoveEx(msg);

				msg = new WhiteboardMessage();
				msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
				msg.start=this.textBox1.Location;//mouseDownPos;
				msg.start.X += p.X;
				msg.start.Y += p.Y;
				msg.end=this.textBox1.Location;
				msg.end.X += p.X;
				msg.end.Y += p.Y;

				msg.tool=(ushort)WhiteboardToolCode.Text;//tool;
				msg.c=this.color;
				msg.font=textBox1.Font;
				msg.thickness=this.lineThickness;
			
				msg.text=textboxvalue;
				msg.txtSize = textBox1.Font.Size;
				msg.txtItalics = textBox1.Font.Italic;
				msg.txtBold = textBox1.Font.Bold;
				msg.txtUnderline = textBox1.Font.Underline;
				msg.FontFamily = textBox1.Font.Name;
				msg.SlideNo=this.currentSlideNo;

				msg.SenderID = NetworkManager.getInstance().profile.ClientId;

				WebMeeting.Client.NetworkManager network = WebMeeting.Client.NetworkManager.getInstance();

				AnnotMsg aMsg = new AnnotMsg(this.annotationType,this.thisSessionID,network.profile);
				aMsg.drawingMsg = msg;	
				if(haveFrames)
					aMsg.sheetNo = currentSheet;
				else
					aMsg.sheetNo = -1;

				aMsg.SenderID = NetworkManager.getInstance().profile.ClientId;

				//network.SendLoadPacket(aMsg);						
				sendingArray.Add(aMsg);
			
				//System.Diagnostics.Debug.Write("calling from textbox1_leave\n");
				this.DrawNow=true;
				this.RepaintObjects(-1);			
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void textBox1_Leave(object sender, System.EventArgs e)",ex,"",false);
			}
		}

		private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyCode == System.Windows.Forms.Keys.Enter)
				{
					textBox1_Leave(this,new System.EventArgs());
					this.tool=WhiteboardToolCode.Text;
					//this.Refresh();
					textBox1.Visible = false;
				}
				if(e.KeyCode==System.Windows.Forms.Keys.Escape)
				{			
					textBox1.Text="";
					this.tool=WhiteboardToolCode.None;
					textBox1.Visible = false;
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)",ex,"",false);
			}
		}

		public void SynchronizePage()
		{
			try
			{
				
				IHTMLDocument2 doc =  getFrameDoc((IHTMLDocument2)browserControl.Document,"frSheet");																					
				string URL = doc.url;
				string sFile = Path.GetFileNameWithoutExtension(URL);
				sFile = sFile.Replace("sheet","");
				int nPageNo = Convert.ToInt32(sFile);
						
				nPageNo--;
				if(nPageNo < sheetsEventStores.Count)
				{
					currentSheet = nPageNo;
					eventStore = (ArrayList)sheetsEventStores[nPageNo];
					SendEraseMessage();
					
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void SynchronizePage()",ex,"",false);
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(WebMeeting.Client.Alerts.MeetingAlerts.AlertType.NonFatal,"Error Occured: " + ee.Message,true,false);
			}
		}

				
		private void browser_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
		{
			try
			{
				navigationCompleted = true;		
				haveFrames = (((IHTMLDocument2) browserControl.Document).frames.length >0);

				object obj = 0;
				object frameObject = (((IHTMLDocument2) browserControl.Document).frames.item(ref obj));
			

				if(IsExcel)
				{
					if(!IsChangeAllowed)
					{
						if(haveFrames)
						{
							try
							{
								SynchronizePage();
							}
							catch(Exception ee)
							{
								ee =ee;
							}

						}
					}
				}
				else if(annotationType ==  AnnotationMessageType.WEB)
				{
					eventStore.Clear();

				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void browser_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)",ex,"",false);
			}
		}
		public void handleEvent(mshtml.IHTMLEventObj eventobj)
		{
			try
			{
				switch(eventobj.type)
				{
					case "scroll":
					{
						//Trace.WriteLine("event type : " + eventobj.type  + " " + eventobj.ToString());
					}break;
					case "click":
						break;
					case "mousedown":
					{

					}break;
					case "mousemove":
					{

					}break;
					case "mouseup":
					{

					}break;
					case "selectstart":
					{
					}break;
					case "dragstart":
					{
					}break;
					case "mouseout":
					{
					}break;
				};
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void handleEvent(mshtml.IHTMLEventObj eventobj)",ex,"",false);
			}
		}

		public void mouseEvent(object sender, mshtml.IHTMLEventObj e)
		{
			try
			{
				handleEvent(e);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void mouseEvent(object sender, mshtml.IHTMLEventObj e)",ex,"",false);
			}
		}

		public void mouseEvent2(object sender, mshtml.IHTMLEventObj e)
		{
			try
			{
				if(e.type == "contextmenu")
				{
					e.returnValue = false;
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void mouseEvent2(object sender, mshtml.IHTMLEventObj e)",ex,"",false);
			}
		} 
		private void browser_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			try
			{
				SHDocVw.IWebBrowser2 doc=e.pDisp as SHDocVw.IWebBrowser2;
			 	
				if (doc==(sender as AxSHDocVw.AxWebBrowser).GetOcx())
				{
				
					try
					{
					
									
						HTMLDocument = (IHTMLDocument2) this.browserControl.Document;
						this.myDocument = (mshtml.HTMLDocument)HTMLDocument;
						//this.window2 = (mshtml.IHTMLWindow2)HTMLDocument.parentWindow;
					
						//this.window2.onscroll=new mshtml.HTMLWindowEvents2_onscrollEventHandler 
						//mshtml.HTMLWindowEvents2_onscrollEventHandler(this.HTMLWindow_onscroll);

					
						//					//this.htmlViewerEvent2 -= new htmlViewerEventHandler2(mouseEvent2);
						//					this.htmlViewerEvent2 += new htmlViewerEventHandler2(mouseEvent2);
						//					//this.htmlViewerEvent -= new htmlViewerEventHandler(mouseEvent);//kill the extra event handlers
						//					this.htmlViewerEvent += new htmlViewerEventHandler(mouseEvent);
						//
						//					this.HTMLDocument.body.onmouseup = this;
						//					this.HTMLDocument.body.onclick = this;
						//					this.HTMLDocument.body.onselectstart = this;
						//					this.HTMLDocument.body.onmousedown = this;
						//					this.HTMLDocument.body.onmousemove = this;
						//					this.HTMLDocument.body.ondragstart = this;
						//					this.HTMLDocument.body.onmouseout = this;
						//					this.myDocument.oncontextmenu = this;
						//					this.window2.onscroll = this;//
						////					IHTMLDocument2 HTMLDoc1 =	(IHTMLDocument2)this.browserControl.Document;
						////					IHTMLWindow2 HTMLWin1 = (mshtml.IHTMLWindow2)HTMLDocument.parentWindow;
						////					this.HTMLWindow.onscroll = new mshtml.HTMLWindowEvents2_onscrollEventHandler(this.HTMLWindow_onscroll); 	
						//						//mshtml.HTMLWindowEvents2_onscrollEventHandler(this.HTMLWindow_onscroll); 
						//					//HTMLWindow.o
						//Trace.WriteLine("Page is completed");						
					}
					catch(Exception ex)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void browser_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)",ex,"",false);
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void browser_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)",ex,"",false);
			}
			
		}
		private void HTMLWindow_onscroll(mshtml.IHTMLEventObj eventobj)
		{
			//Trace.WriteLine("scroll is completed" + eventobj.type.ToString() );
		}
		private void browser_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
		{
			try
			{
				navigationCompleted = false;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void browser_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)",ex,"",false);
			}
			
		}
		

		///**************************************************************************************************************************
		#region Native WhiteBoard Drawing Controls

		#region WhiteBoard variables inherited from WhiteBoardControl.cs
		public WhiteboardToolCode tool;		
		public Color color;

		private Point mouseDownPos;
		private Point mouseCurrentPos;
		private int brushThickness;
		private Rectangle eraser;
		public  ArrayList eventStore;
		public  ArrayList messageStore = new ArrayList();
		private Point pencilLastPoint;
		public Bitmap mainImage;	
		Graphics g;
		private bool m_bIsActive;		
		WhiteboardMessage message;
		WhiteboardMessage msg;
		Rectangle m_bDummyRect;
		
		private System.Windows.Forms.TextBox textBox1;
		
		private ArrayList undoArray;
		private ArrayList redoArray;
		//private string filename="";
		public int lineThickness;

	
		#endregion

		private struct ARROWSTRUCT 
		{
			public int nWidth;		// width (in pixels) of the full base of the arrowhead
			public float fTheta;	// angle (in radians) at the arrow tip between the two
			//  sides of the arrowhead
			public bool bFill;		// flag indicating whether or not the arrowhead should be
		} ;
		private ARROWSTRUCT pA;
		
		private void ConsumeThread()
		{
			try
			{
				int counter=0;
				bool modified=false;
				//Trace.WriteLine("Consume thread start now ");
				

				while(m_bIsActive==true)
				{
					//Trace.WriteLine("Consume thread start now ");
					while(messageStore.Count>0)
					{			
						try
						{
							modified=true;	
							if(((++counter)%10)==0)
							{
								modified=true;
								counter=0;					
							}
							AnnotMsg aMsg =(AnnotMsg)messageStore[0];
							message = aMsg.drawingMsg;						
							WhiteboardMessage m=new WhiteboardMessage();
							m.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
							if((((WhiteboardToolCode)message.tool) != WhiteboardToolCode.Undo) && ( ((WhiteboardToolCode)message.tool)  != WhiteboardToolCode.Redo))
							{
								m.c=message.c;
								m.font=message.font;
								m.text=message.text;
								m.tool=message.tool;
								m.start=message.start;
								m.pencilLastPoint=message.pencilLastPoint;
								m.end=message.end;
								m.thickness=message.thickness;						
								m.font=message.font;
								m.thickness=message.thickness;
								m.txtSize = message.txtSize;
								m.txtItalics = message.txtItalics;
								m.txtBold = message.txtBold;
								m.txtUnderline = message.txtUnderline;
								m.FontFamily = message.FontFamily;
								m.SenderID = aMsg.SenderID;
								m.pencilLastPoints = message.pencilLastPoints;   // by kamran
								m.endPoints= message.endPoints;
								m.startPoints= message.startPoints;
								m.SlideNo= message.SlideNo;

								undoArray.Add(m);
						
								if((haveFrames) && (IsExcel))
								{
									((ArrayList)sheetsEventStores[aMsg.sheetNo]).Add(m);								
								}
								else
									this.eventStore.Add(m);
								CreateMouseMoveEx(m);
							}	

							messageStore.RemoveAt(0);
						}
						catch(System.Threading.ThreadAbortException exp)
						{
							exp=exp;
						}
					
						catch(Exception ex)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void ConsumeThread()",ex,"",true);	
						}
					}
					
					Thread.Sleep(10); // new old 1000
				}
			}
			catch(System.Threading.ThreadAbortException ex)
			{
				ex=ex;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void ConsumeThread()",ex,"",false);
				//Thread th=new Thread(new ThreadStart(ConsumeThread));
				//th.Start();
			}
		}

		private void CreateMouseMove(WhiteboardMessage msg)
		{
			//System.Diagnostics.Debug.Write("in create mouse move");
			try
			{
				CreateMouseMoveEx(msg);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void CreateMouseMove(WhiteboardMessage msg)",ex,"",false);
			}
		}
		
		Point p ;
		Point pe;
		Point scrollPoints ;
	
	
		












		private void CreateMouseMoveEx(WhiteboardMessage msg)//Point mouseDownPos,Point currentPos,WhiteboardToolCode tool,Color c)
		{
			try
			{
				p = GetCurrentScrollPositionBrowser(browserControl);							
				pe = new Point(p.X , p.Y);
				scrollPoints = new Point(p.X , p.Y);
				//System.Diagnostics.Debug.Write("in create mouse move  EX");
			 
				if(NetworkManager.getInstance().profile.clientType != ClientType.ClientAttendee)
					this.IsAttendee=false;
				else
					this.IsAttendee =true;
				try
				{
					//Trace.WriteLine("current Slide No:" +this.currentSlideNo.ToString  "Next Slide No :");
					if(currentSlideNo == msg.SlideNo)
					{
						switch((WhiteboardToolCode)msg.tool)
						{						
							case WhiteboardToolCode.Ellipse:
								if(IsAttendee)
								{
									p.X = msg.start.X - p.X;
									p.Y = msg.start.Y - p.Y;					
									pe.X = msg.end.X - pe.X;
									pe.Y = msg.end.Y - pe.Y ;
									//DrawEllipse(msg.start,msg.end,msg.c, msg.thickness);
									DrawEllipse(p,pe,msg.c, msg.thickness);
								}
								else
									DrawEllipse(msg.start,msg.end,msg.c, msg.thickness);
								break;
							case WhiteboardToolCode.Eraser:
								/*
								if(IsAttendee)  // isAttendee is true
								{
									//Trace.WriteLine("Attendee Side pencil Drawing");
									for(int i=0; i<msg.endPoints.Count ; i++ )
									{
										//p.X = ((Point)msg.pencilLastPoints[i]).X - scrollPoints.X;
										//p.Y = ((Point)msg.pencilLastPoints[i]).Y - scrollPoints.Y;					
							
										pe.X = ((Point)msg.endPoints[i]).X - scrollPoints.X;
										pe.Y = ((Point)msg.endPoints[i]).Y - scrollPoints.Y;
										this.DrawEraser(pe);
										//DrawPencil(p,pe,msg.c, msg.thickness);

										this._mouseX=pe.X ;  // by kamran check mouse move syncronyzation
										this._mousey=pe.Y ;
										DrawMouse(ref g);
									}
								}
								else
									*/
								{
									if(msg.endPoints.Count>0)
									{
										for(int i=0; i<msg.endPoints.Count ; i++ )
										{
											//	p.X = ((Point)msg.pencilLastPoints[i]).X - scrollPoints.X;
											//	p.Y = ((Point)msg.pencilLastPoints[i]).Y - scrollPoints.Y;					
							
											pe.X = ((Point)msg.endPoints[i]).X - scrollPoints.X;
											pe.Y = ((Point)msg.endPoints[i]).Y - scrollPoints.Y;
											this.DrawEraser(pe);
											//DrawPencil(p,pe,msg.c, msg.thickness);

											this._mouseX=pe.X ;  // by kamran check mouse move syncronyzation
											this._mousey=pe.Y ;
											DrawMouse(ref g);
										}
									}
									else
									{
										p.X = msg.pencilLastPoint.X - p.X;
										p.Y = msg.pencilLastPoint.Y - p.Y;					
										DrawPencil(p,msg.end ,msg.c, msg.thickness);
									}
					
								
								}
								//DrawEraser(msg.end);
								break;
							case WhiteboardToolCode.Line:
								
									DrawLine(msg.start,msg.end,msg.c, msg.thickness);
								break;
							case WhiteboardToolCode.Brush:
							
								//if(IsAttendee)  // isAttendee is true
								for(int i=0; i<msg.endPoints.Count ; i++ )
								{
									p.X = ((Point)msg.pencilLastPoints[i]).X - scrollPoints.X;
									p.Y = ((Point)msg.pencilLastPoints[i]).Y - scrollPoints.Y;					
							
									pe.X = ((Point)msg.endPoints[i]).X - scrollPoints.X;
									pe.Y = ((Point)msg.endPoints[i]).Y - scrollPoints.Y;

									this.DrawBrush1(p,pe,msg.c, this.brushThickness);
									//msg.pencilLastPoint,msg.end,msg.c,msg.thickness
									//this.DrawBrush1(msg.pencilLastPoint,msg.end,msg.c, msg.thickness);
								

									this._mouseX=pe.X ;  // by kamran check mouse move syncronyzation
									this._mousey=pe.Y ;
									DrawMouse(ref g);
								}
								break;
							case WhiteboardToolCode.Pencil:
									if(msg.endPoints.Count>0)
									{
										for(int i=0; i<msg.endPoints.Count ; i++ )
										{
											p.X = ((Point)msg.pencilLastPoints[i]).X - scrollPoints.X;
											p.Y = ((Point)msg.pencilLastPoints[i]).Y - scrollPoints.Y;					
							
											pe.X = ((Point)msg.endPoints[i]).X - scrollPoints.X;
											pe.Y = ((Point)msg.endPoints[i]).Y - scrollPoints.Y;
											DrawPencil(p,pe,msg.c, msg.thickness);

											this._mouseX=pe.X ;  // by kamran check mouse move syncronyzation
											this._mousey=pe.Y ;
											DrawMouse(ref g);
										}
									}
									else
									{
										p.X = msg.pencilLastPoint.X - p.X;
										p.Y = msg.pencilLastPoint.Y - p.Y;					
										DrawPencil(p,msg.end ,msg.c, msg.thickness);
									}
					
								break;
							case WhiteboardToolCode.Rectangle:
								/*if(IsAttendee)
								{
									p.X = msg.start.X - p.X;
									p.Y = msg.start.Y - p.Y;					
									pe.X = msg.end.X - pe.X;
									pe.Y = msg.end.Y - pe.Y;
									//DrawRectangle(msg.start,msg.end,msg.c, msg.thickness);
									DrawRectangle(p,pe,msg.c, msg.thickness);
								}
								else
								*/
									DrawRectangle(msg.start,msg.end,msg.c, msg.thickness);
								break;
							case WhiteboardToolCode.Text:
							/*
								if(IsAttendee)
								{
									p.X = msg.start.X - p.X;
									p.Y = msg.start.Y - p.Y;					
									DrawText(p,msg.text,msg.FontFamily,msg.txtSize,msg.txtBold,msg.txtItalics,msg.txtUnderline,msg.c);
									//Trace.WriteLine("text written");
									//								if((msg.text!=null)&&(msg.txtSize!=0.0)&&(msg.text!=""))
									//									DrawText(msg.start,msg.text,msg.FontFamily,msg.txtSize,msg.txtBold,msg.txtItalics,msg.txtUnderline,msg.c);
								}
								else
								*/
								{
									//System.Diagnostics.Debug.Write("calling draw text");
									//Trace.WriteLine("text written");
									if((msg.text!=null)&&(msg.txtSize!=0.0)&&(msg.text!=""))
										DrawText(msg.start,msg.text,msg.FontFamily,msg.txtSize,msg.txtBold,msg.txtItalics,msg.txtUnderline,msg.c);
								}
								//					DrawText(msg.start,msg.text,msg.font,msg.c);
								break;
							case WhiteboardToolCode.UniArrow:
								if(IsAttendee)
								{
									p.X = msg.start.X - p.X;
									p.Y = msg.start.Y - p.Y;					
									pe.X = msg.end.X - pe.X;
									pe.Y = msg.end.Y - pe.Y ;
									DrawUniArrow(p,pe,msg.c,msg.thickness);
									//DrawUniArrow(msg.start,msg.end,msg.c,msg.thickness);
								}
								else
									DrawUniArrow(msg.start,msg.end,msg.c,msg.thickness);
								break;
							case WhiteboardToolCode.PlaceHolderArrow:
								if(IsAttendee)
								{
									p.X = msg.start.X - p.X;
									p.Y = msg.start.Y - p.Y;					
									PlaceHolderArrow(p,msg.c,msg.text);
									//PlaceHolderArrow(msg.start,msg.c,msg.text);
								}
								else
									PlaceHolderArrow(msg.start,msg.c,msg.text);

								break;

			
							default:
								break;
						}
					}
				}//end of try
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void CreateMouseMoveEx(WhiteboardMessage msg)",ex,"",false);
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void CreateMouseMoveEx(WhiteboardMessage msg)",ex,"",false);
			}
		}
		private void DrawBrush1(Point start, Point end, Color c,int thickness)
		{		
			//Rectangle r=new Rectangle(Math.Min(start.X,p.X),Math.Min(start.Y,p.Y),Math.Max(start.X,p.X)-Math.Min(start.X,p.X),Math.Max(start.Y,p.Y)-Math.Min(start.Y,p.Y));
			//g.FillEllipse((new Pen(c)).Brush,r);
			try
			{
				Point[] points=new Point[4];
				int thick=thickness/2;
				points[0]=new Point(start.X - thick, start.Y-thick);//start.X - brushThickness/2,start.Y - brushThickness/2);
				points[1]=new Point(start.X+thick,start.Y+thick);//,maxY);//start.X - brushThickness/2,start.Y + brushThickness/2);			
				points[3]=new Point(end.X-thick,end.Y-thick);//maxX,minY);//end.X + brushThickness/2,end.Y + brushThickness/2);
				points[2]=new Point(end.X+thick,end.Y+thick);//,maxY);//end.X + brushThickness/2,end.Y - brushThickness/2);

				//GraphicsPath path = new GraphicsPath();
				//g.DrawPolygon(points); 
				if(g!=null)
				{
					lock(g)
					{				
						g.DrawPolygon(new Pen(c),points); 				
						g.FillPolygon(new Pen(c).Brush,points,System.Drawing.Drawing2D.FillMode.Winding); 
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawBrush1(Point start, Point end, Color c,int thickness)",ex,"",false);
			}


			//g.FillEllipse(new Pen(c).Brush,start.X,start.Y,brushThickness,brushThickness);			
			
			
				//FillRegion(new Pen(c).Brush,r);

			/*	points[0]=new Point(start.X + thick, start.Y-thick);//start.X - brushThickness/2,start.Y - brushThickness/2);
				points[2]=new Point(start.X-thick,start.Y-thick);//,maxY);//start.X - brushThickness/2,start.Y + brushThickness/2);			
				points[3]=new Point(end.X-thick,end.Y-thick);//maxX,minY);//end.X + brushThickness/2,end.Y + brushThickness/2);
				points[1]=new Point(end.X+thick,end.Y-thick);//,maxY);//end.X + brushThickness/2,end.Y - brushThickness/2);
				path=new GraphicsPath();
			
				path.AddPolygon(points);
				r=new Region(path);			
				//g.FillEllipse(new Pen(c).Brush,end.X,end.Y,brushThickness,brushThickness);			
				g.FillRegion(new Pen(c).Brush,r);
	*/


			//pencilLastPoint=p;


		}	
	

	


		private void PlaceHolderArrow(Point p,Color c,String text)
		{			
			try
			{
					constFont=new FontStyle();
					constFont=constFont|FontStyle.Bold;
					//Font myFont = new Font(textBox1.Font.FontFamily,textBox1.Font.Size,ff);
					constmyFont = new Font("Microsoft Sans Serif",8,constFont);
				
			
				int width=text.Length * 5 + 20;
				p.X=p.X - width - 19;
				p.Y=p.Y- 19;
			
				Point[] points=new Point[7];
				points[0]=new Point(p.X ,p.Y+10);
				points[1]=new Point(p.X +width,p.Y+10);
				points[2]=new Point(p.X +width,p.Y+1);
				points[3]=new Point(p.X +width+19,p.Y+19);
				points[4]=new Point(p.X +width,p.Y+37);
				points[5]=new Point(p.X +width,p.Y+28);
				points[6]=new Point(p.X ,p.Y+28);
			
				GraphicsPath path = new GraphicsPath();
				path.AddPolygon(points);
				Region r=new Region(path);
			
				//g.FillEllipse(new Pen(c).Brush,end.X,end.Y,brushThickness,brushThickness);			
				if(g!=null)
				{
					lock(g)
					{
						g.FillRegion(new Pen(c).Brush,r);
					}
				}
				FontStyle ff=new FontStyle();
				ff=ff|FontStyle.Bold;
			
				//Font myFont = new Font(textBox1.Font.FontFamily,textBox1.Font.Size,ff);
				if(g!=null)
				{
					lock(g)
					{
						g.DrawString(text,constmyFont,new Pen(Color.White).Brush,p.X + 3 ,p.Y + 12);						
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void PlaceHolderArrow(Point p,Color c,String text)",ex,"",false);
			}
		}
		private void DrawText(Point pos,string text,Font f,Color c)
		{
			try
			{				
				//System.Diagnostics.Debug.Write("Draw string calling");
				if(g!=null)
				{
					lock(g)
					{
						g.DrawString(text,f,(new Pen(c)).Brush,pos);
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawText(Point pos,string text,Font f,Color c)",ex,"",false);
			}
			//this.Refresh();
		}
		
		private void DrawPencil(Point start,Point end,Color c,int thickness)
		{
			try
			{
				if(g!=null)
				{
					lock(g)
					{
						g.DrawLine((new Pen(c,thickness)),start,end);
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawPencil(Point start,Point end,Color c,int thickness)",ex,"",false);
			}
		}
		private void DrawUniArrow(Point start,Point end,Color c,int thickness)
		{			
			try
			{
				Point pFrom = start;
				Point pBase = new Point(0,0);
				Point[] aptPoly=new Point[3];
				float[] vecLine = new float[2];
				float[] vecLeft=  new float[2];;
				float fLength;
				float th;
				float ta;

				// get from point
		

				// set to point
				aptPoly[0].X = end.X;
				aptPoly[0].Y = end.Y;

				// build the line vector
				vecLine[0] = (float) aptPoly[0].X - pFrom.X;
				vecLine[1] = (float) aptPoly[0].Y - pFrom.Y;

				// build the arrow base vector - normal to the line
				vecLeft[0] = -vecLine[1];
				vecLeft[1] = vecLine[0];

				// setup length parameters
				fLength = (float) Math.Sqrt(vecLine[0] * vecLine[0] + vecLine[1] * vecLine[1]);
				th = pA.nWidth / (2.0f * fLength);
				ta = pA.nWidth / (2.0f * ((float)Math.Tan(pA.fTheta) / 2.0f) * fLength);

				// find the base of the arrow
				pBase.X = (int) (aptPoly[0].X + -ta * vecLine[0]);
				pBase.Y = (int) (aptPoly[0].Y + -ta * vecLine[1]);

				// build the points on the sides of the arrow
				aptPoly[1].X = (int) (pBase.X + th * vecLeft[0]);
				aptPoly[1].Y = (int) (pBase.Y + th * vecLeft[1]);
				aptPoly[2].X = (int) (pBase.X + -th * vecLeft[0]);
				aptPoly[2].Y = (int) (pBase.Y + -th * vecLeft[1]);


				Point  p6 = new Point(pBase.X, pBase.Y);
				Point p7= new Point(aptPoly[1].X, aptPoly[1].Y);
				Point p3= new Point (aptPoly[0].X, aptPoly[0].Y);
				Point p4= new Point(aptPoly[2].X, aptPoly[2].Y);
				Point p5= new Point(pBase.X, pBase.Y);
				if(g!=null)
				{
					lock(this)
					{
						g.FillPolygon((new Pen(c,this.lineThickness)).Brush,new Point[]{p6,p7,p3,p4,p5});
						g.DrawLine((new Pen(c)),start,end);
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawUniArrow(Point start,Point end,Color c,int thickness)",ex,"",false);
			}
	

		}
		private void DrawEraser(Point p)
		{
			//g.Clip 
				
			//RedrawWindow(browserHandle, IntPtr.Zero,IntPtr.Zero,RDW_INVALIDATE | RDW_UPDATENOW | RDW_ALLCHILDREN);						
			//g.FillRectangle((new Pen(Color.White)).Brush,p.X,p.Y,eraser.Width,eraser.Height);
			
//			Rectangle rd=new Rectangle(p.X,p.Y,eraser.Width,eraser.Height);
			//System.Drawing.Rectangle rect=new System.Drawing.Rectangle(); 
			//Rectangle rd=new Rectangle();
			//rd.Top=p.X;
			//rd.Left =p.Y;
			//rd.Right=eraser.Width;
			//rd.Bottom=eraser.Height;
			//g.Clip.Exclude(new Region(new Rectangle(p.X,p.Y,eraser.Width,eraser.Height)));
			//System.Drawing.Rectangle(); 
			try
			{
				Rectangle rd=new Rectangle(p.X,p.Y,p.X+eraser.Width,p.Y+eraser.Height);
				//Trace.WriteLine("X : " + p.X.ToString() + " Y : " + p.Y.ToString() + " Erase Width : " + eraser.Width.ToString() + " Eraser Height : " + eraser.Height.ToString());
				//Rectangle rd=new  System.Drawing.Rectangle();
				//Trace.WriteLine(p.X + p.Y + eraser.Width.ts
				//	);
				//this.browserControl.Invalidate(rd); 
				//System.Drawing.Graphics.  
				Win32.USER32.InvalidateRect(browserHandle,ref rd,true);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawEraser(Point p)",ex,"",false);
			}

		
		}
		public void DrawText(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c)
		{
			try
			{
				FontStyle ff=new FontStyle();
				if(bold)
					ff=ff|FontStyle.Bold;
				if(italics)
					ff=ff|FontStyle.Italic;
				if(underline)
					ff=ff|FontStyle.Underline;           
				try
				{
					if(fontFamily!=null)
					{
						Font f = new Font(fontFamily,size,ff);
						DrawText(pos,text,f,c);
					}
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void DrawText(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c)",ex,"",false);
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  public void DrawText(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c)",ex,"",false);
			}
		}

		private void DrawLine(Point start, Point end, Color c, int thickness)
		{
			try
			{				
				if(g!=null)
				{
					lock(g)
					{
						g.DrawLine((new Pen(c, thickness)),start,end);
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawLine(Point start, Point end, Color c, int thickness)",ex,"",false);
			}


		}
		private void DrawEllipse(Point start,Point end,Color c, int thickness)
		{
			try
			{
				int left=Math.Min(start.X,end.X);
				int top=Math.Min(start.Y,end.Y);
				int width=Math.Max(start.X,end.X) - left;
				int height=Math.Max(start.Y,end.Y) - top;
				if(g!=null)
				{
					lock(g)
					{
						g.DrawEllipse((new Pen(c, thickness)),left,top,width,height);
					}
				}

			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawEllipse(Point start,Point end,Color c, int thickness)",ex,"",false);
			}

		}
		private void DrawRectangle(Point start,Point end,Color c, int thickness)
		{
			try
			{
				int left=Math.Min(start.X,end.X);
				int top=Math.Min(start.Y,end.Y);
				int width=Math.Max(start.X,end.X) - left;
				int height=Math.Max(start.Y,end.Y) - top;
				if(g!=null)
				{
					lock(g)
					{
						g.DrawRectangle((new Pen(c, thickness)),left,top,width,height);
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WebSharing  private void DrawRectangle(Point start,Point end,Color c, int thickness)",ex,"",false);
			}
		}

		#endregion
	}
}