
//#define WhiteBoardTrace

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using WebMeeting.Common;
using Utility.NiceMenu;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using WebMeeting.Client.Alerts;
using System.Diagnostics;
using WebMeeting.Client;




namespace WebMeetingDrawingBoard
{
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

	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class WhiteboardControl : System.Windows.Forms.UserControl
	{


		#region userdefined variable decleration

		int image_width;
		public System.Windows.Forms.Button btn_whtLogg;/*store width of Displaying Background image*/
		int image_height;/*store height of Displaying background image*/
		
		public delegate void ScrollingDelegate(int nH,int nV);
		public event ScrollingDelegate OnScrolling;
		
		

		
		private struct ARROWSTRUCT 
		{
			public int nWidth;		// width (in pixels) of the full base of the arrowhead
			public float fTheta;	// angle (in radians) at the arrow tip between the two
			//  sides of the arrowhead
			public bool bFill;		// flag indicating whether or not the arrowhead should be
		} ;

		/*#############################################################
		 * App sharing coding
		 * */
		Win32.USER32.ScrollInfoStruct si = new Win32.USER32.ScrollInfoStruct();

		private ARROWSTRUCT pA;
        
		public WhiteboardPictureBox pictureBox1;		

		public WhiteboardToolCode tool;		

		public Color color;
						
		public Bitmap mainImage;		
		private Image CursorImage;
		public Image lastFrameImage;
						
		private System.Windows.Forms.TextBox textBox1;		
		
		private bool m_bIsActive;		
		public bool IsAppSharingWindow = false;
		private bool refreshLock=false;
		bool ZoomView;

		Graphics g;

		private Rectangle eraser;
		Rectangle m_bDummyRect;

		private Point mouseDownPos;
		private Point mouseCurrentPos;
		private Point pencilLastPoint;
		private Point lastPoints;

		Pen _pen = new Pen(Color.Transparent,2);

		WhiteboardMessage message;
		WhiteboardMessage msg;
		private WhiteboardMessage lTxtMsg;
		
		
		private string filename;

		private int slideNo=0;
		private int lineThickness;
		private int brushThickness;

		public int nSessionId =0;

		
		int preSendCount = 0;
		public int nCount=0;
		private int waitingcount=0;
		
		public  ArrayList eventStore;
		private ArrayList undoArray;
		private ArrayList redoArray;
		public ArrayList sendingArrayList = new ArrayList();
		ArrayList m_ArrayMessages = new ArrayList();
		
        public  Font localCopyOfFont;
		private FontStyle SelfFont;
		private float zoomLevel;

		public Thread sendingThread;
		public Thread thConsumeThread;



		
		//      By Zaeem For Place Holder		
		public FontStyle constFont;
		public Font constmyFont;

				
		#endregion		


		
		#region constructor Distructor of WhiteboardControl 
		public void RunSendingThread()
		{
			try
			{
				if(this.sendingThread!=null)
				{
					if(this.sendingThread.IsAlive==false)
					{
						sendingThread = new Thread(new ThreadStart(SendingThreadFunction));
						sendingThread.Name = "Whiteboard Control Constructor Thread: SendingThreadFunction()";
						sendingThread.Start();		
					}
				}
				else/*bind method with thread*/
				{
				}
			}
			catch(System.Threading.ThreadAbortException ex)
			{
				ex=ex;
			}
					
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WhiteBoard public void RunSendingThread() ",ex,"",false);			
			}
		}
		public WhiteboardControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			try
			{
			
			   
				SelfFont=new FontStyle();
				SelfFont=SelfFont|FontStyle.Bold;
				localCopyOfFont = new Font(this.Font.FontFamily,this.Font.Size,SelfFont);

				
				constFont=new FontStyle();
				constFont=constFont|FontStyle.Bold;
				constmyFont = new Font(this.Font.FontFamily,this.Font.Size,constFont);


				InitializeComponent();
				this.BackColor=Color.White;
				tool=WhiteboardToolCode.None;//.Brush;//.Ellipse;//Rectangle;//.Line;
				color=Color.Black;
				mouseDownPos=new Point(0,0);
				mouseCurrentPos=new Point(0,0);
				brushThickness=10;
				eventStore=new ArrayList();			
				pencilLastPoint=new Point(-1,-1);
				eraser=new Rectangle(0,0,10,10);
				msg=new WhiteboardMessage();
			
				m_bDummyRect=new Rectangle(0,0,0,0);
				zoomLevel=1;
				ZoomView=false;
				undoArray=new ArrayList();
				redoArray=new ArrayList();
				filename="";
				lineThickness=4;

				pA.nWidth = 10;
				pA.fTheta = 0.3f;
				pA.bFill = true;

				m_bIsActive = true;
				sendingThread = new Thread(new ThreadStart(SendingThreadFunction));
				sendingThread.Name = "Whiteboard Control Constructor Thread: SendingThreadFunction()";
				sendingThread.Start();

				CursorImage = WebMeeting.Client.ClientUI.getInstance().LoadImageFromResource("WebMeeting.Client.cur.gif");
			}
			catch(Exception ex)
			{	
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WhiteBoard public WhiteboardControl() ",ex,"",false);
				//Trace.WriteLine("public WhiteboardControl()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}
		}
		
		~WhiteboardControl()
		{
			try
			{
				sendingThread.Abort();
				sendingThread.Join();
				thConsumeThread.Abort();
				thConsumeThread.Join();
			}
			catch(Exception ex)
			{	
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WhiteBoard public WhiteboardControl() ",ex,"",false);
				//Trace.WriteLine("~WhiteboardControl()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}
		}
        		
		#endregion


		#region	Thread Methods
		
		public void SendingThreadFunction()
		{
			try
			{
				WebMeeting.Client.NetworkManager network = WebMeeting.Client.NetworkManager.getInstance();
				StateUpdateMessage msg ;
				while(m_bIsActive)
				{
					if(sendingArrayList.Count <1)
					{
						Thread.Sleep(10);
						continue;
					}
					msg = new StateUpdateMessage();
					if(this.IsAppSharingWindow)
						msg.updateType = UpdateTypeCode.UpdateAppsharingannotations;
					else
						msg.updateType = UpdateTypeCode.UpdateWhiteboard;
					msg.dataArray = (ArrayList)sendingArrayList.Clone();
					msg.SenderID = network.profile.ClientId;
					preSendCount = sendingArrayList.Count;
					network.SendLoadPacket(msg);
				
					if(preSendCount < sendingArrayList.Count)
					{ // remove only seneded messages
						sendingArrayList.RemoveRange(0, preSendCount );
					}
					else
					{
						sendingArrayList.Clear();
					}
					msg  = null;
				}
			}	
			catch(System.Threading.ThreadAbortException ex)
			{
				ex=ex;
			}
			catch(Exception ex)
			{	
				ClientUI.getInstance().ShowExceptionMessage("Module ::: WhiteBoard public void SendingThreadFunction()",ex,"",false);
				//Trace.WriteLine("public void SendingThreadFunction()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}
		}

		private void ConsumeThread()
		{
			try
			{
				int counter=0;				
				while(m_bIsActive==true)
				{					
					while(eventStore.Count>0)
					{			
						try
						{
							//							if(((++counter)%10)==0)
							//							{
							//								counter=0;					
							//							}
							message=(WhiteboardMessage)eventStore[0];
							if((WhiteboardToolCode)message.tool==WhiteboardToolCode.Pencil)
							{						
								lock(this)
								{
									g.DrawLine((new Pen(message.c,message.thickness)),message.pencilLastPoint,message.end);
								}
								if(message.SenderID != NetworkManager.getInstance().profile.ClientId)
								{
									this.undoArray.Add(message);
								}
								while(this.pictureBox1.isPictureAssign==true)
								{
								}
								this.pictureBox1.Invalidate();
								if(eventStore.Count >0)
								eventStore.RemoveAt(0);
//								while(this.pictureBox1.isPictureAssign==true)
//								{
//									System.Threading.Thread.Sleep(10);
//								}
//								this.pictureBox1.Invalidate();
							}
							else
							{						
								WhiteboardMessage m=new WhiteboardMessage();						
								m.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
								if((((WhiteboardToolCode)message.tool) != WhiteboardToolCode.Undo) && ( ((WhiteboardToolCode)message.tool)  != WhiteboardToolCode.Redo))
								{
									m.c=message.c;
									m.font=message.font;
									m.text=message.text;
									m.txtSize = message.txtSize;
									m.FontFamily = message.FontFamily;
									m.txtBold = message.txtBold;
									m.txtItalics = message.txtItalics;
									m.txtUnderline = message.txtUnderline;
							
									m.tool=message.tool;
									m.thickness = message.thickness;							
									m.start=message.start;
									m.pencilLastPoint=message.pencilLastPoint;
									m.end=message.end;
									m.thickness=message.thickness;
									m.SenderID = message.SenderID;
									undoArray.Add(m);
								}	
								if(eventStore.Count >0)
								eventStore.RemoveAt(0);				
								//this.pictureBox1.Refresh();
								//this.pictureBox1.Focus();
								CreateMouseMove(message);//message.start,message.end,(WebMeetingDrawingBoard.WhiteboardToolCode) message.tool,message.c);
								while(this.pictureBox1.isPictureAssign==true)
								{
									System.Threading.Thread.Sleep(10);
								}
								this.pictureBox1.Invalidate();
								//this.pictureBox1.Update();
								CreateMouseMove(message);//message.start,message.end,(WebMeetingDrawingBoard.WhiteboardToolCode) message.tool,message.c);
							}
						}
						catch(System.Threading.ThreadAbortException ex)
						{
							ex=ex;
						}
					
						catch(Exception ex)
						{
							ClientUI.getInstance().ShowExceptionMessage("Module ::: private void ConsumeThread() ",ex,"",false);
							//Trace.WriteLine("private void ConsumeThread()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
						}					
					}					
					Thread.Sleep(10); // only for loop
				}				
			}
			catch(System.Threading.ThreadAbortException ex)
			{
				ex=ex;
			}
					
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: private void ConsumeThread() ",ex,"",false);
			}		
		}	

				
		#endregion 	Thread Methods
	
		
		#region Method


		//		public void ExceptionLogWhiteBoard(Exception methodException, string strMethodName)
		//		{								
		//			#if WhiteBoardTrace
		//			try
		//			{
		//				Trace.WriteLine(" Module:::WhiteBoard >>> " + " Method Name:::"+strMethodName+" >>>");
		//			}
		//			catch(Exception ex)
		//			{
		//				Trace.WriteLine("public void ExceptionLogWhiteBoard(Exception ex, string strMethodName)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
		//			}
		//			#endif
		//		}


		public void InitializeToSize(int width,int height)
		{
			try
			{			
				this.BackColor=Color.White;
				mainImage=new Bitmap(width,height);											
				this.pictureBox1.Image=mainImage;			
				g=Graphics.FromImage(mainImage);			
				lock(this)
				{
					g.FillRectangle(new Pen(Color.Transparent).Brush,0,0,width,height);
				}
				thConsumeThread=new Thread(new ThreadStart(ConsumeThread));
				thConsumeThread.Name = "Initialize to Size Whiteboard Control Thread: ConsumeThread()";
				thConsumeThread.Start();
				m_bIsActive=true;		
				if(IsAppSharingWindow)
				{				
					this.AutoScroll = true;
				}
				else
				{	
					this.AutoScroll = false;
				}			
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage(" Module ::: Whiteboard " + "public void InitializeToSize(int width,int height)" ,ex,"",false);

				//ShowExceptionMessage()
				//Trace.WriteLine("public void InitializeToSize(int width,int height)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);							
			}	
		}
		public void SelectFont()
		{
			try
			{
				FontDialog fd=new FontDialog();
				fd.Font=this.Font;
				fd.ShowDialog();
				this.localCopyOfFont=fd.Font;
				
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void SelectFont() ",ex,"",false); 
				//Trace.WriteLine("public void SelectFont()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);							
			}	
			
		}
		public void SelectColor()
		{
			try
			{
				ColorDialog cc= new ColorDialog();
				cc.Color=this.color;
				cc.ShowDialog();
				this.color=cc.Color;
		
			}				
			catch(Exception ex)
			{
				//Trace.WriteLine("public void SelectColor()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);							
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void SelectColor()",ex,"",false);
			}	
		}
		public void ZoomIn()
		{
			try
			{
				zoomLevel=(float)Math.Min((float)(zoomLevel*1.5),5.0625);
				ZoomView=true;
				this.tool=WhiteboardToolCode.None;
				this.AutoScrollMinSize = new Size ((int)(this.mainImage.Width * this.zoomLevel), (int)(this.mainImage.Height * this.zoomLevel));
				Invalidate();		
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void ZoomIn()",ex,"",false);
				//Trace.WriteLine("public void ZoomIn()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);							
			}
		}
		public void ZoomOut()
		{
			try
			{
				zoomLevel=(float)Math.Max((float)(zoomLevel/1.5) , .19753);
				ZoomView=true;
				this.tool=WhiteboardToolCode.None;
				this.AutoScrollMinSize = new Size ((int)(this.mainImage.Width * this.zoomLevel), (int)(this.mainImage.Height * this.zoomLevel));
				Invalidate();			
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void ZoomOut() ",ex,"",false);
				//Trace.WriteLine("public void ZoomOut()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);							
			}
		}
		public void Zoom500()
		{
			try
			{
				zoomLevel=5;
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void Zoom500()",ex,"",false);
				//Trace.WriteLine("public void Zoom500()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}
		}
		public void Zoom200()
		{
			try
			{
				zoomLevel=200;
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void Zoom200()",ex,"",false);
				//Trace.WriteLine("public void Zoom200()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);							
			}		
		}
		public void Zoom100()
		{
			try
			{
				zoomLevel=100;
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void Zoom100()",ex,"",false);
				//Trace.WriteLine("public void Zoom100()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);							
			}		
		}
		public void Zoom50()
		{
			try
			{
				zoomLevel=50;
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void Zoom50()",ex,"",false);
				//Trace.WriteLine("public void Zoom50()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);							
			}		
		}
		public void zoom25()
		{
			try
			{
				zoomLevel=25;
			}
			catch(Exception ex)
			{
				//Trace.WriteLine("public void zoom25()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void zoom25()",ex,"",false);
			}		
		}	
		public void display_Annotation()
		{
			g.Clear(Color.White);				
			for(int i=0;i<undoArray.Count ;i++)
			{			
				CreateMouseMove((WhiteboardMessage)undoArray[i]);
			}
			this.mouseCurrentPos=new Point(0,0);
			this.mouseDownPos=new Point(0,0);
			while(this.pictureBox1.isPictureAssign==true)
			{
			}
			this.pictureBox1.Invalidate();		
		}
		public void resize(int width, int height)
		{
			try
			{
				mainImage  = new Bitmap(width,height); //GC1
				//Graphics newGraphics = Graphics.FromImage(bmp); //GC1
				//newGraphics.DrawImage(mainImage,0,0);
				//mainImage = bmp; //GC1
				this.pictureBox1.Image = mainImage;
				g =Graphics.FromImage(mainImage);// newGraphics;				
				display_Annotation();
				//Undo();
				//Redo();
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void resize(int width, int height)",ex,"",false);
				//Trace.WriteLine("public void resize(int width, int height)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}		
		}
		
		public void abortThreads()
		{
			try
			{
				sendingThread.Abort();
				sendingThread.Join();			
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void abortThreads()",ex,"",false);
				//Trace.WriteLine("public void abortThreads()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}
			try
			{
				thConsumeThread.Abort();
				thConsumeThread.Join();
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void abortThreads()",ex,"",false);
			}
		}
		public void Terminate()
		{
			try
			{
				m_bIsActive=false;
				
			}			
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void Terminate()",ex,"",false);
				//Trace.WriteLine("public void Terminate()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}
			try
			{
				if(this.sendingThread!=null)
				{
					if(this.sendingThread.IsAlive)
					{
						this.sendingThread.Abort();						
					}

				}
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void Terminate()",ex,"",false);
			}	
			try
			{
				
				if(this.thConsumeThread!=null)
				{
					if(this.thConsumeThread.IsAlive)
					{
						this.thConsumeThread.Abort();
					}

				}
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void Terminate()",ex,"",false);
			}	

		}
	
		/*clear all graphics before displaying new image of appshare.*/
		public void OptimizeAppShareClear()
		{
			DrawingClearMessage msg = new DrawingClearMessage(0);
			msg.m_ControlType = ControlType.DrawingBoard;
			msg.SenderID = -1;
			msg.senderProfile = NetworkManager.getInstance().profile;					
			//network.SendLoadPacket(msg);
			lock(this.sendingArrayList)
			{
				this.sendingArrayList.Add(msg);
			}

			eventStore.Clear();
			undoArray.Clear();
//			if(g!=null)
//				g.Clear(Color.White);								

				
		}
		public void ClearDrawingAppShare()
		{
			try
			{
				this.mouseDownPos=this.mouseCurrentPos;
				eventStore.Clear();
				if(g!=null)
					g.Clear(Color.White);								
					if(this.IsAppSharingWindow)
					{
						if(this.lastFrameImage!=null)
						{
							mainImage = new Bitmap(lastFrameImage.Width,lastFrameImage.Height);//GC1
						}
						else
						{							
							mainImage = new Bitmap(400,400);//GC1
						}
					}
					//Graphics newGraphics = Graphics.FromImage(bmp); //GC1								 
					// mainImage = bmp; //GC1
					//Trace.WriteLine("width  " + mainImage.Width.ToString() + "height "+mainImage.Height.ToString());
					this.pictureBox1.Image = mainImage;
					g =Graphics.FromImage(mainImage);				
				
					//for(int i=undoArray.Count-1;i>=0;i--)
					this.undoArray.Clear();
					
					//check presenter is host
				if(NetworkManager.getInstance().profile.clientType==ClientType.ClientHost)
				{
					if(!ClientUI.getInstance().ifHostisPresenter())
					{
						g.DrawImage(this.CursorImage, lastPoints.X,lastPoints.Y);
					}	
				}
				else if(NetworkManager.getInstance().profile.clientType==ClientType.ClientAttendee)
				{
					lock(this)
					{
						g.DrawImage(this.CursorImage, lastPoints.X,lastPoints.Y);
					}
				}						
				Refresh();				
			}
			catch(Exception ex)
			{
				//Trace.WriteLine("public void Clear(bool bAll, int sessionToRemove)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message+" Source ::: " + ex.Source+ " Target Site ::: "+ ex.TargetSite.ToString());				
				ClientUI.getInstance().ShowExceptionMessage(" Module ::: Whiteboard " + "public void Clear(bool bAll, int sessionToRemove)" ,ex,"",false);
			}
		}

		public void Clear(bool bAll, int sessionToRemove)
		{
			try
			{
				this.mouseDownPos=this.mouseCurrentPos;
				eventStore.Clear();
				if(g!=null)
					g.Clear(Color.White);								
				if(bAll)
				{
					undoArray.Clear();
					redoArray.Clear();
					Thread.Sleep(1000); //GC4
					if(this.IsAppSharingWindow)
					{
						if(this.lastFrameImage!=null)
						{
							mainImage = new Bitmap(lastFrameImage.Width,lastFrameImage.Height);//GC1
						}
						else
						{
							mainImage = new Bitmap(400,400);//GC1	
						}
						
					}
					
					//Graphics newGraphics = Graphics.FromImage(bmp);//GC1								
					//mainImage = bmp; //GC1
					this.pictureBox1.Image = mainImage;
					g =Graphics.FromImage(mainImage);				
					pictureBox1.Invalidate(m_bDummyRect);
					return;
				}
				else
				{
					if(this.IsAppSharingWindow)
					{
						if(this.lastFrameImage!=null)
						{
							mainImage = new Bitmap(lastFrameImage.Width,lastFrameImage.Height);//GC1
							
						}
						else
						{							
							mainImage = new Bitmap(400,400);//GC1
						}
					}
					//Graphics newGraphics = Graphics.FromImage(bmp); //GC1								 
					// mainImage = bmp; //GC1
					//Trace.WriteLine("width  " + mainImage.Width.ToString() + "height "+mainImage.Height.ToString());
					this.pictureBox1.Image = mainImage;
					g =Graphics.FromImage(mainImage);				
				
					//for(int i=undoArray.Count-1;i>=0;i--)
					for(int i=0;i<undoArray.Count;)
					{		
						//Trace.WriteLine("Tools Sender IDs :::  "+((WhiteboardMessage)undoArray[i]).SenderID.ToString());
						if(sessionToRemove==-1)
						{
							//redoArray.Remove(undoArray[i]);
							if(undoArray.Count>0)
							{
								undoArray.RemoveAt(i);
							}
							continue;	
						}
						
						if(((WhiteboardMessage)undoArray[i]).SenderID == sessionToRemove)
						{
							//redoArray.Remove(undoArray[i]);
							if(undoArray.Count>0)
							{
								undoArray.RemoveAt(i);
							}
							continue;	
						}
						CreateMouseMove((WhiteboardMessage) undoArray[i]);														
						i++;
					}				
					Refresh();
				}
				Refresh();
			}
			catch(Exception ex)
			{
				//Trace.WriteLine("public void Clear(bool bAll, int sessionToRemove)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message+" Source ::: " + ex.Source+ " Target Site ::: "+ ex.TargetSite.ToString());				
				ClientUI.getInstance().ShowExceptionMessage(" Module ::: Whiteboard " + "public void Clear(bool bAll, int sessionToRemove)" ,ex,"",false);
			}
		}

		public void Undo()
		{			
			try
			{
				g.Clear(Color.White);
				
				for(int i=0;i<undoArray.Count -1;i++)
				{			
					CreateMouseMove((WhiteboardMessage) undoArray[i]);
				}
				if(undoArray.Count>=2)
				{
					if((redoArray.Count -2)>=0 || (undoArray.Count -2)>=0 )//Nonnegative
					redoArray.Add(undoArray[undoArray.Count -2]);
					if((undoArray.Count -2)>=0 )//Nonnegative
					undoArray.RemoveAt(undoArray.Count -2);
				}
				
				this.mouseCurrentPos=new Point(0,0);
				this.mouseDownPos=new Point(0,0);
				this.pictureBox1.Invalidate();		
			}
			catch(Exception ex)
			{
				//Trace.WriteLine("public void Undo()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message + " Source ::: "+ex.Source);
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void Undo()",ex,"",false);
			}
		}

		public void ClearAll()
		{
			try
			{
			
				if(this.tool==WhiteboardToolCode.Text)
				{
					if(this.textBox1.Text!="")
					{
						textBox1_Leave(this,new System.EventArgs());
					}
				}
				g.Clear(Color.White);
				while(this.pictureBox1.isPictureAssign==true)
				{
				}
				this.pictureBox1.Invalidate();
			}		
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void ClearAll()",ex,"",false);
				//Trace.WriteLine("public void ClearAll()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}

		}
		public void Redo()
		{
			try
			{
				if(redoArray.Count>0)
				{

					CreateMouseMove((WhiteboardMessage) redoArray[redoArray.Count-1]);
					undoArray.Add(redoArray[redoArray.Count-1]);
					redoArray.RemoveAt(redoArray.Count -1);
				}
				this.mouseCurrentPos=new Point(0,0);
				this.mouseDownPos=new Point(0,0);
				this.pictureBox1.Invalidate();				
			}
			catch(Exception ex)
			{
				//Trace.WriteLine("public void Redo()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void Redo()",ex,"",false);
			}
			
		}

		public void Save()
		{
			try
			{
			
				if(this.filename=="")
				{
					SaveAs();
					return;
				}
				try
				{
					this.mainImage.Save(filename,System.Drawing.Imaging.ImageFormat.Jpeg);
				}
				catch(Exception ex)
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(WebMeeting.Client.Alerts.MeetingAlerts.AlertType.NonFatal,"Couldnt Save to " + filename,true,false);
				}         
			}				
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void Save()",ex,"",false);
				//Trace.WriteLine("public void Save()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}
		}	
		public void SaveAs()
		{
			try
			{

				SaveFileDialog fd=new SaveFileDialog();
				fd.Filter="Image Files(*.jpg)|*.jpg";
				DialogResult res=fd.ShowDialog();
				if(res==DialogResult.Cancel)
					return;				
				filename=fd.FileName;
				Save();
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void SaveAs()",ex,"",false);
				//Trace.WriteLine("public void SaveAs()"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}

		}

		public void ResizeWhiteboard(int width, int height)
		{
			try
			{
				this.pictureBox1.Width=width;
				this.pictureBox1.Height=height;
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void ResizeWhiteboard(int width, int height)",ex,"",false);
				//Trace.WriteLine("public void ResizeWhiteboard(int width, int height)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}

		}

		private void CreateMouseMove(WhiteboardMessage msg)//Point mouseDownPos,Point currentPos,WhiteboardToolCode tool,Color c)
		{
			try
			{
				switch((WhiteboardToolCode)msg.tool)
				{
					case WhiteboardToolCode.Brush:
						DrawBrush(msg.pencilLastPoint,msg.end,msg.c,msg.thickness);
						break;
					case WhiteboardToolCode.Ellipse:
						DrawEllipse(msg.start,msg.end,msg.c, msg.thickness);
						break;
					case WhiteboardToolCode.Eraser:
						DrawEraser(msg.end);
						break;
					case WhiteboardToolCode.Line:
						DrawLine(msg.start,msg.end,msg.c, msg.thickness);
						break;
					case WhiteboardToolCode.Pencil:
						DrawPencil(msg.pencilLastPoint,msg.end,msg.c, msg.thickness);
						break;
					case WhiteboardToolCode.Rectangle:
						DrawRectangle(msg.start,msg.end,msg.c, msg.thickness);
						break;
					case WhiteboardToolCode.Text:
						lTxtMsg=msg;
						DrawText(msg.start,msg.text,msg.FontFamily,msg.txtSize,msg.txtBold,msg.txtItalics,msg.txtUnderline,msg.c);
						break;
					case WhiteboardToolCode.UniArrow:
						DrawUniArrow(msg.start,msg.end,msg.c,msg.thickness);
						break;
					case WhiteboardToolCode.Undo:
						Undo();
						break;
					case WhiteboardToolCode.Redo:
						Redo();break;
					case WhiteboardToolCode.PlaceHolderArrow:
						PlaceHolderArrow(msg.start,msg.c,msg.text);
						break;
					default:
						break;
				}
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void CreateMouseMove(WhiteboardMessage msg)",ex,"",false);
				//Trace.WriteLine("private void CreateMouseMove(WhiteboardMessage msg)//Point mouseDownPos,Point currentPos,WhiteboardToolCode tool,Color c)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}

		}

		private void PlaceHolderArrow(Point p,Color c,String text)
		{			
	
			try
			{
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
				lock(this)
				{
					g.FillRegion(new Pen(c).Brush,r);
				}
			
				FontStyle ff=new FontStyle();
				ff=ff|FontStyle.Bold;
						
				Font myFont = new Font(this.Font.FontFamily,this.Font.Size,ff);
				lock(this)
				{
					g.DrawString(text,constmyFont,new Pen(Color.White).Brush,p.X + 3 ,p.Y + 12);
				}
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void PlaceHolderArrow(Point p,Color c,String text)",ex,"",false);
				//Trace.WriteLine("private void PlaceHolderArrow(Point p,Color c,String text)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}

		}
		private void DrawText(Point pos,string text,Font f,Color c)
		{
			try
			{
				g.DrawString(text,f,(new Pen(c)).Brush,pos);			
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawText(Point pos,string text,Font f,Color c)",ex,"",false);
				//Trace.WriteLine("private void DrawText(Point pos,string text,Font f,Color c)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}					
		}
		private void DrawBrush(Point start, Point end, Color c,int thickness)
		{		
			try
			{
				Point[] points=new Point[4];
				int thick=thickness/2;
				points[0]=new Point(start.X - thick, start.Y-thick);//start.X - brushThickness/2,start.Y - brushThickness/2);
				points[1]=new Point(start.X+thick,start.Y+thick);//,maxY);//start.X - brushThickness/2,start.Y + brushThickness/2);			
				points[3]=new Point(end.X-thick,end.Y-thick);//maxX,minY);//end.X + brushThickness/2,end.Y + brushThickness/2);
				points[2]=new Point(end.X+thick,end.Y+thick);//,maxY);//end.X + brushThickness/2,end.Y - brushThickness/2);

				GraphicsPath path = new GraphicsPath();
				path.AddPolygon(points);
				Region r=new Region(path);			
				lock(this)
				{
				
					g.FillRegion(new Pen(c).Brush,r);
				}
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawBrush(Point start, Point end, Color c,int thickness)",ex,"",false);
				//Trace.WriteLine("private void DrawBrush(Point start, Point end, Color c,int thickness)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}					

		}
		private void DrawPencil(Point start,Point end,Color c,int thickness)
		{			
			try
			{
				g.DrawLine((new Pen(c,thickness)),start,end);
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawPencil(Point start,Point end,Color c,int thickness)",ex,"",false);
				//Trace.WriteLine("private void DrawPencil(Point start,Point end,Color c,int thickness)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
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
				lock(this)
				{
					g.FillPolygon((new Pen(c,this.lineThickness)).Brush,new Point[]{p6,p7,p3,p4,p5});
					g.DrawLine((new Pen(c)),start,end);
				}
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawUniArrow(Point start,Point end,Color c,int thickness)",ex,"",false);
				//Trace.WriteLine("private void DrawUniArrow(Point start,Point end,Color c,int thickness)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}					
		}
		private void DrawEraser(Point p)
		{
			try
			{

				lock(this)
				{
				
					g.FillRectangle((new Pen(Color.White)).Brush,p.X,p.Y,eraser.Width,eraser.Height);
				}
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawEraser(Point p)",ex,"",false);
				//Trace.WriteLine("private void DrawEraser(Point p)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
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
						this.Font = new Font(fontFamily,size,ff);
						DrawText(pos,text,this.Font,c);
					}
				}
				catch(Exception ex)
				{
					ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void DrawText(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c)",ex,"",false);
					//Trace.WriteLine("public void DrawText(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
				}
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void DrawText(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c)",ex,"",false);
			}
		}

		private void DrawLine(Point start, Point end, Color c, int thickness)
		{
			try
			{				
				g.DrawLine((new Pen(c, thickness)),start,end);
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawLine(Point start, Point end, Color c, int thickness)",ex,"",false);
				//Trace.WriteLine("private void DrawLine(Point start, Point end, Color c, int thickness)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
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
				g.DrawEllipse((new Pen(c, thickness)),left,top,width,height);

			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawEllipse(Point start,Point end,Color c, int thickness)",ex,"",false);
				//Trace.WriteLine("private void DrawEllipse(Point start,Point end,Color c, int thickness)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
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
				lock(this)
				{
					g.DrawRectangle((new Pen(c, thickness)),left,top,width,height);
				}
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawRectangle(Point start,Point end,Color c, int thickness)",ex,"",false);
				//Trace.WriteLine("private void DrawRectangle(Point start,Point end,Color c, int thickness)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}
		}
		public void DrawArrow(Point pt )
		{
			try
			{
				/*
				 * image_width & image_height are the current background imaage properties
				 * */
				this.pictureBox1.isPictureAssign=true;
				//Trace.WriteLine("Picture Paint Status");
				if(this.pictureBox1.isPaint==false)
					Trace.WriteLine("Picture Paint Status false");

				while(this.pictureBox1.isPaint==true)
				{
				}
				

				mainImage = new Bitmap(image_width,image_height); //GC1
				g =Graphics.FromImage(mainImage);				
				
				this.pictureBox1.Image=mainImage;
				

				//this.pictureBox1.Image = mainImage;
				//for(int i=undoArray.Count-1;i>=0;i--)
				for(int i=0;i<undoArray.Count;i++)
				{					
					CreateMouseMove((WhiteboardMessage) undoArray[i]);														
				}
				lastPoints= pt;		
				lock(this)
				{
					g.DrawImage(this.CursorImage, lastPoints.X,lastPoints.Y);
				}
				this.pictureBox1.isPictureAssign=false;				
				Refresh();
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void DrawArrow(Point pt )",ex,"",false);
			}
		}

		private void saveMenuClick(object sender, EventArgs e)
		{
			try
			{
				Save();
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void saveMenuClick(object sender, EventArgs e)",ex,"",false);
				//Trace.WriteLine("private void saveMenuClick(object sender, EventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}			
		}

		#endregion Methods


		#region PictureBox handlers

		private void pictureBox1_SizeChanged(object sender, System.EventArgs e)
		{
			try
			{
				int i=0;
				i=i;
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void pictureBox1_SizeChanged(object sender, System.EventArgs e)",ex,"",false);
				//Trace.WriteLine("private void pictureBox1_SizeChanged(object sender, System.EventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}			
		}

		private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{			
			try
			{

			//	if(this.IsAppSharingWindow)
			//		if(NetworkManager.getInstance().profile.clientType == ClientType.ClientAttendee)					
			//			return;
				//MessageBox.Show("==>"+this.tool.ToString());

				color = NetworkManager.getInstance().profile.m_AssignedColor;
				mouseDownPos=new Point(e.X,e.Y);
				if(this.textBox1.Visible==true)//!="")
				{
					textBox1_Leave(this,new System.EventArgs());
					this.textBox1.Visible=false;
				}
				pencilLastPoint=new Point(e.X,e.Y);
				if(this.tool!=WhiteboardToolCode.None)
				{

					if(ZoomView==true)
					{					
						ZoomView=false;
						this.pictureBox1.Invalidate();
					}				
					else if(this.tool==WhiteboardToolCode.Text)
					{
						if(this.textBox1.Text!="")
						{
							textBox1_Leave(this,new System.EventArgs());
						}
						textBox1.Font=this.localCopyOfFont;					
					
						this.textBox1.Top=e.Y;
						this.textBox1.Left=e.X;
						this.textBox1.Visible=true;
						this.textBox1.Width=100;
						this.textBox1.BorderStyle=BorderStyle.None;
						this.textBox1.Focus();
					}		
		
				}
			}					
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)",ex,"",false);
				//Trace.WriteLine("private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}			

		}

		private void pictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{	
			try
			{
			
				if(e.Button==MouseButtons.Left)
				{				
					if(((ushort)tool & (ushort)(WhiteboardToolCode.Brush | WhiteboardToolCode.Pencil |  WhiteboardToolCode.Eraser )) == (ushort)tool)
					{
						WhiteboardMessage msg=new WhiteboardMessage();
						msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
						msg.start=mouseDownPos;
						msg.end=new Point(e.X,e.Y);
						msg.pencilLastPoint=this.pencilLastPoint;//new Point(this.pencilLastPoint.X,this.pencilLastPoint.Y);
						msg.tool=(ushort)tool;
						msg.c=this.color;
						if(tool == WhiteboardToolCode.Brush)
							msg.thickness=this.brushThickness;
						else
							msg.thickness=this.lineThickness;
						msg.nSessionId = nSessionId;			
					
						if(IsAppSharingWindow)
							msg.MessageType = (ushort) ((int)MessageObjectType.MsgAppSharingAnnotation);
      										
						if((ushort)WhiteboardToolCode.Pencil==(ushort)tool)
						{	
							pencilLastPoint=msg.end;//new Point(e.X,e.Y);					
							lock(this)
							{
								g.DrawLine((new Pen(msg.c,msg.thickness)),msg.pencilLastPoint,msg.end);
							}

							while(this.pictureBox1.isPictureAssign==true)
							{
							}
							this.pictureBox1.Invalidate();
							msg.SenderID = NetworkManager.getInstance().profile.ClientId;
							sendingArrayList.Add(msg);
							undoArray.Add(msg);
						}
						else if((ushort)WhiteboardToolCode.Brush==(ushort)tool)
						{
							pencilLastPoint=msg.end;//new Point(e.X,e.Y);
							DrawBrush(msg.pencilLastPoint, msg.end,msg.c,msg.thickness);
							while(this.pictureBox1.isPictureAssign==true)
							{
							}
							this.pictureBox1.Invalidate();
							msg.SenderID = NetworkManager.getInstance().profile.ClientId;
							sendingArrayList.Add(msg);
							undoArray.Add(msg);
						}
						else
						{
							this.eventStore.Add(msg);					
							pencilLastPoint=msg.end;//new Point(e.X,e.Y);
							//debug2.Add(new Point(pencilLastPoint.X,pencilLastPoint.Y));
							msg.SenderID = NetworkManager.getInstance().profile.ClientId;
							sendingArrayList.Add(msg);
						}
					}
				}			
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void pictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)",ex,"",false);
				//Trace.WriteLine("private void pictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}			
		}

		private void pictureBox1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				mouseCurrentPos.X=e.X;
				mouseCurrentPos.Y=e.Y;
				if(e.Button==MouseButtons.Left)
				{
					if(msg ==null)
						msg=new WhiteboardMessage();
					
					if((ushort)WhiteboardToolCode.Pencil==(ushort)tool)
						return;
					if(((ushort)tool & (ushort)(WhiteboardToolCode.Brush | WhiteboardToolCode.Pencil |  WhiteboardToolCode.Eraser |WhiteboardToolCode.PlaceHolderArrow)) == (ushort)tool)
					{
						//WhiteboardMessage msg=new WhiteboardMessage();
						msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
						msg.start=new Point(mouseDownPos.X , mouseDownPos.Y); // new for checking why points are not new 
						msg.end=new Point(e.X,e.Y);
						msg.tool=(ushort)tool;
						msg.c=this.color;
						msg.thickness=this.lineThickness;
						msg.pencilLastPoint=new Point(this.pencilLastPoint.X,this.pencilLastPoint.Y);
						msg.SenderID = NetworkManager.getInstance().profile.ClientId;
						msg.nSessionId = nSessionId;
						msg.text=NetworkManager.getInstance().profile.Name;
						if(IsAppSharingWindow)
							msg.MessageType = (ushort) ((int)MessageObjectType.MsgAppSharingAnnotation);
                    						
						this.eventStore.Add(msg);	
						sendingArrayList.Add(msg);	
						//System.Windows.Forms.PaintEventArgs e1=new System.Windows.Forms.PaintEventArgs(null,);
						//this.pictureBox1_Paint(sender,e1);
						this.pictureBox1.Refresh();
						this.pictureBox1.Update();
						
					}
					else
					{					
						if((ushort)tool!=(ushort)WhiteboardToolCode.Text)
						{
							msg.start=new Point(mouseDownPos.X , mouseDownPos.Y); // check here for new points
							msg.end=new Point(e.X,e.Y);
							msg.tool=(ushort)tool;
							msg.c=this.color;
							msg.thickness=this.lineThickness;
							msg.nSessionId = nSessionId;
							if(IsAppSharingWindow)
								msg.MessageType = (ushort) ((int)MessageObjectType.MsgAppSharingAnnotation);
      
							this.eventStore.Add(msg);
							msg.SenderID = NetworkManager.getInstance().profile.ClientId;
							NetworkManager network = NetworkManager.getInstance();
							sendingArrayList.Add(msg);
						}
					}
					msg = null;
				}			
			}			
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void pictureBox1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)",ex,"",false);
				//Trace.WriteLine("private void pictureBox1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}			
		}

	
		private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			try
			{
				if(this.ZoomView==true)
				{
					e.Graphics.Clear(Color.White);
					e.Graphics.DrawImage(this.mainImage, new Rectangle(0,0, (int)(this.mainImage.Width * this.zoomLevel)	, (int)(this.mainImage.Height  * this.zoomLevel)));										
					this.pictureBox1.Invalidate(m_bDummyRect);						
					return;
				}
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)",ex,"",false);
				//Trace.WriteLine("private void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}
		}

		#endregion PictureBox handlers


		#region Textbox Handlers
		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.textBox1.Width = Math.Min((int)( this.textBox1.Text.Length * this.localCopyOfFont.Size /1.5) + 10,this.Width - this.textBox1.Left);									
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void textBox1_TextChanged(object sender, System.EventArgs e)",ex,"",false);
				//Trace.WriteLine("private void textBox1_TextChanged(object sender, System.EventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}
		}

		private void textBox1_Leave(object sender, System.EventArgs e)
		{
			try
			{
				WhiteboardMessage msg=new WhiteboardMessage();
				msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;

				msg.start=this.textBox1.Location;//mouseDownPos;
				msg.end=this.textBox1.Location;
				msg.tool=(ushort)WhiteboardToolCode.Text;//tool;
				msg.c=this.color;
				msg.font=this.localCopyOfFont;
				msg.thickness=this.lineThickness;
			
				msg.text=this.textBox1.Text;
				this.eventStore.Add(msg);			
				msg.txtSize = localCopyOfFont.Size;

				msg.txtItalics = localCopyOfFont.Italic;
				msg.txtBold = localCopyOfFont.Bold;
				msg.txtUnderline = localCopyOfFont.Underline;
				msg.FontFamily = localCopyOfFont.Name;

				NetworkManager network = NetworkManager.getInstance();
				msg.SenderID = NetworkManager.getInstance().profile.ClientId;;
				msg.nSessionId = nSessionId;
				if(IsAppSharingWindow)
					msg.MessageType = (ushort) ((int)MessageObjectType.MsgAppSharingAnnotation);
     
				sendingArrayList.Add(msg);
				network.SendLoadPacket(msg);						
				this.textBox1.Text="";
				this.tool=WhiteboardToolCode.None;
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void textBox1_Leave(object sender, System.EventArgs e)",ex,"",false);
				//Trace.WriteLine("private void textBox1_Leave(object sender, System.EventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}

		}
		

		private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
		        this.textBox1.Width = Math.Min((int)( this.textBox1.Text.Length * this.localCopyOfFont.Size /1.5) + 10,this.Width - this.textBox1.Left);
				if(e.KeyCode == System.Windows.Forms.Keys.Enter)
				{
					textBox1_Leave(this,new System.EventArgs());				
					this.BringToFront();
					this.Focus();
					textBox1.Visible = false;				
				}
				if(e.KeyCode==System.Windows.Forms.Keys.Escape)
				{
					textBox1.Text="";
					textBox1.Visible = false;
				}
			}
			catch(Exception ex)
			{
				//Trace.WriteLine("private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)",ex,"",false);
			}
		}

		#endregion Textbox Handlers

		#region Method that Appshare use
		
		public void RecieveAppSharingImage( System.Drawing.Image image)
		{
			try
			{									
				try
				{
					if(image == null)
						return;
					int nX = image.Width;
				}
				catch(Exception ex)
				{
					ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void RecieveAppSharingImage( System.Drawing.Image image)",ex,"",false);
					//Trace.WriteLine("public void RecieveAppSharingImage( System.Drawing.Image image) :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
					return;
				}						
				waitingcount++;
				while(refreshLock==true)
				{
					//return;//
					Thread.Sleep(100);
					if(waitingcount>2)
					{
						waitingcount--;
						return;
					}
				}
				refreshLock=true;			
				try
				{
					Trace.WriteLine("from appshare::::App Sharing Image attributes " + image.Width.ToString()+"  " +image.Height.ToString());					
					image_width=image.Width;
					image_height=image.Height;
					this.pictureBox1.isPictureAssign=true;
					Trace.WriteLine("Picture Paint Status");
					if(this.pictureBox1.isPaint==false)
						Trace.WriteLine("Picture Paint Status false");
					while(this.pictureBox1.isPaint==true)
					{
						Trace.WriteLine("waiting picture paint to free..");
					}


					if(lastFrameImage!=null)
					{						
						if(this.pictureBox1.BackgroundImage!=null)
						{														
							//this.pictureBox1.BackgroundImage.Dispose();
							//this.pictureBox1.BackgroundImage=null;							
						}					
						lastFrameImage =(Image)image.Clone();
					}
					else
					{
						lastFrameImage =(Image)image.Clone();
					}
				
					//lastFrameImage.Save(@"c:\temp\abc"+ nCount.ToString() +".jpg");
					//nCount++;
					//this.pictureBox1.BackgroundImage=null;
					
					this.pictureBox1.isPictureAssign=false;
					this.ClearDrawingAppShare();
					//this.Clear(false,-1);
					//this.OptimizeAppShareClear();
					this.pictureBox1.BackgroundImage = lastFrameImage ;
					
					/*----------------------- Exception no ::: 9 'Module ::: whiteboard public void RecieveAppSharingImage( System.Drawing.Image image)'  exception stringSystem.InvalidOperationException: The object is currently in use elsewhere.
					   at System.Drawing.Image.get_Width()
					   at System.Drawing.Image.get_Size()
					   at System.Windows.Forms.PictureBox.GetPreferredSize()
					   at System.Windows.Forms.PictureBox.SetBoundsCore(Int32 x, Int32 y, Int32 width, Int32 height, BoundsSpecified specified)
					   at System.Windows.Forms.Control.SetBounds(Int32 x, Int32 y, Int32 width, Int32 height, BoundsSpecified specified)
					   at System.Windows.Forms.Control.set_Width(Int32 value)
					   at WebMeetingDrawingBoard.WhiteboardControl.RecieveAppSharingImage(Image image)  :::::Message :::: The object is currently in use elsewhere. ::::Stack Trace::::   at System.Drawing.Image.get_Width()
					   at System.Drawing.Image.get_Size()
					   at System.Windows.Forms.PictureBox.GetPreferredSize()
					   at System.Windows.Forms.PictureBox.SetBoundsCore(Int32 x, Int32 y, Int32 width, Int32 height, BoundsSpecified specified)
					   at System.Windows.Forms.Control.SetBounds(Int32 x, Int32 y, Int32 width, Int32 height, BoundsSpecified specified)
					   at System.Windows.Forms.Control.set_Width(Int32 value)
					   at WebMeetingDrawingBoard.WhiteboardControl.RecieveAppSharingImage(Image image):::: Source :::::System.Drawing:::: TargetSite ::::Int32 get_Width()
					 -------------------------------  end of Exception-------------------------------
					 */
					this.pictureBox1.Width = image_width;
					this.pictureBox1.Height = image_height;				
				
					if(waitingcount==1)
					{	
						//image.Save("c:\\app\\abc" + nCount.ToString() +".jpg");
						nCount++;
						if((image_width != mainImage.Width) || (image_height != mainImage.Height))
						{
							//							if(mainImage != null)
							//								mainImage.Dispose();
							//GC.Collect();//GC1

							/*if(mainImage != null) //GC1
							{
								mainImage = null;
								GC.Collect(); //GC1
							}*/

							mainImage = new Bitmap(image_width ,image_height);
							//Graphics newGraphics = Graphics.FromImage(bmp); //GC1								
							//mainImage = bmp; //GC1
							Trace.WriteLine("from appshare::::Main Image attributes " + mainImage.Width.ToString()+"  " +mainImage.Height.ToString());							
//							if(this.pictureBox1.Image!=null)
//							{
//								this.pictureBox1.Image.Dispose();
//								this.pictureBox1.Image=null;
//							}
							/*cause exception 
							 * ----------------------- Exception no ::: 3 'Module ::: whiteboard public void RecieveAppSharingImage( System.Drawing.Image image)'  exception stringSystem.InvalidOperationException: The object is currently in use elsewhere.
							   at System.Drawing.Graphics.FromImage(Image image)
							   at WebMeetingDrawingBoard.WhiteboardControl.RecieveAppSharingImage(Image image)  :::::Message :::: The object is currently in use elsewhere. ::::Stack Trace::::   at System.Drawing.Graphics.FromImage(Image image)
							   at WebMeetingDrawingBoard.WhiteboardControl.RecieveAppSharingImage(Image image):::: Source :::::System.Drawing:::: TargetSite ::::System.Drawing.Graphics FromImage(System.Drawing.Image)
							   -------------------------------  end of Exception-------------------------------
							 */
							g =Graphics.FromImage(mainImage);
							lock(pictureBox1)
							{								
								this.pictureBox1.Image = mainImage;
							}

							//Console.WriteLine("Image Reallocation");							
							//OptimizeAppShareClear();
//							for(int i=0;i<undoArray.Count;i++)
//							{			
//								CreateMouseMove((WhiteboardMessage) undoArray[i]);
//							}                    
						}  
						//this.pictureBox1.isPictureAssign=false;
						
						Refresh();
						//pictureBox1.Invalidate();
					}						
				}
				catch(Exception ex)
				{
					this.pictureBox1.isPictureAssign=false;
					ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void RecieveAppSharingImage( System.Drawing.Image image)",ex,"",false);
				}
				waitingcount--;
				refreshLock=false;
			}
			catch(Exception ex)
			{
				//Trace.WriteLine("public void RecieveAppSharingImage( System.Drawing.Image image) :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void RecieveAppSharingImage( System.Drawing.Image image)",ex,"",false);
				this.pictureBox1.isPictureAssign=false;
			}
		}

		#endregion Method that Appshare use



		#region Scroll methods
		
		private void FireScrolling(int nX,int nY)
		{
			try
			{
				if(OnScrolling!= null)
					OnScrolling(nX,nY);
			}
			catch(Exception ex)
			{				
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void FireScrolling(int nX,int nY) ",ex,"",false);
				//Trace.WriteLine("private void FireScrolling(int nX,int nY) :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}

		}
		

		public void SetHPosition(int nPos)
		{
			try
			{
				si.fMask = Win32.USER32.SIF_POS;
				si.nPos = nPos;
				si.cbSize = Marshal.SizeOf(si);
				Win32.USER32.SetScrollInfo(this.Handle, Win32.USER32.SB_HORZ, ref si,true);
			}
			catch(Exception ex)
			{
				ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void SetHPosition(int nPos) ",ex,"",false);
				//				Trace.WriteLine("public void SetHPosition(int nPos) :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}

		}
		public void SetVPosition(int nPos)
		{
			try
			{
				si.fMask = Win32.USER32.SIF_POS;
				si.nPos = nPos;
				si.cbSize = Marshal.SizeOf(si);
				Win32.USER32.SetScrollInfo(this.Handle, Win32.USER32.SB_VERT, ref si,true);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void SetVPosition(int nPos)  ",ex,"",false);
				//Trace.WriteLine("public void SetVPosition(int nPos) :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}
		}
		public int ScrollPositionH()
		{					
			try
			{
				si.fMask = Win32.USER32.SIF_POS;
				si.cbSize = Marshal.SizeOf(si);
				Win32.USER32.GetScrollInfo(this.Handle, Win32.USER32.SB_HORZ, ref si);		
				return si.nPos;
			}			
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public int ScrollPositionH() ",ex,"",false);
				//Trace.WriteLine("public int ScrollPositionH() :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
				return 0;
			}

		}
		

		public int ScrollPositionV()
		{		
			try
			{
				int nX = Win32.USER32.GetScrollPos(this.Handle, Win32.USER32.SB_VERT);

				si.fMask = Win32.USER32.SIF_POS;
				si.cbSize = Marshal.SizeOf(si);
				if(Win32.USER32.GetScrollInfo(this.Handle, Win32.USER32.SB_VERT, ref si) != 0)
					return si.nPos;
				else
					return 0;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public int ScrollPositionV()",ex,"",false);
				//Trace.WriteLine("public int ScrollPositionV() :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
				return 0;
			}
		}

		#endregion Scroll methods

		#region winprog Method
		
		protected override void WndProc(ref Message m)
		{
			try
			{			
				switch(m.Msg)
				{
					case Win32.USER32.WM_HSCROLL:
						FireScrolling(ScrollPositionH(),ScrollPositionV());
						break;
					case Win32.USER32.WM_VSCROLL:
						FireScrolling(ScrollPositionH(),ScrollPositionV());
						break;
				}
				base.WndProc(ref m);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard protected override void WndProc(ref Message m)",ex,"",false);
				//Trace.WriteLine("protected override void WndProc(ref Message m) :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}			
		}
		#endregion winprog Method

		/// <summary> 
		/// Required designer variable.
		/// </summary>		
		private System.ComponentModel.Container components = null;
		public Utility.NiceMenu.NiceMenu myMenu;

		#region Properties of the Whiteboard		
		public int SlideNo
		{
			
			get
			{
				return slideNo;
			}
			set
			{
				this.slideNo= value;
			}
		}
		public int LineThickness
		{
			set
			{
				this.lineThickness=value;
			}
			get
			{
				return lineThickness;
			}
		}

		public Color DrawingColor
		{
			set
			{
				color=value;
			}
			get
			{
				return color;
			}
		}
		public WhiteboardToolCode DrawingTool
		{			
			set
			{				
				tool=value;
			}
			get
			{
				return tool;
			}			
		}
		public float ZoomLevel
		{
			set
			{
				zoomLevel=value;
			}
			get
			{
				return zoomLevel;
			}
		}

		#endregion			
								
		
		
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pictureBox1 = new WebMeetingDrawingBoard.WhiteboardPictureBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.btn_whtLogg = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(200, 200);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);
			this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
			this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(80, 136);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(16, 21);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "";
			this.textBox1.Visible = false;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
			this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
			// 
			// btn_whtLogg
			// 
			this.btn_whtLogg.Location = new System.Drawing.Point(470, 10);
			this.btn_whtLogg.Name = "btn_whtLogg";
			this.btn_whtLogg.TabIndex = 2;
			this.btn_whtLogg.Text = "Log";
			this.btn_whtLogg.Visible = false;
			this.btn_whtLogg.Click += new System.EventHandler(this.btn_whtLogg_Click);
			// 
			// WhiteboardControl
			// 
			this.AutoScroll = true;
			this.Controls.Add(this.btn_whtLogg);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.pictureBox1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.Name = "WhiteboardControl";
			this.Size = new System.Drawing.Size(336, 213);
			this.ResumeLayout(false);

		}
		#endregion
	
		private void gridDocument_PrintPage(object sender, PrintPageEventArgs e)
		{
		}

		private void btn_whtLogg_Click(object sender, System.EventArgs e)
		{
		ClientUI.getInstance().MM_Controller.Send_MinutsofMeetingMsg_Whiteboard();		
		}		
	}
	public class WhiteboardPictureBox : System.Windows.Forms.PictureBox
	{

		public bool isPaint=false;
		public bool isPictureAssign=false;		
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			try
			{
				//Trace.WriteLine("protected override void OnPaintBackground(PaintEventArgs pevent)");
				if(isPictureAssign==false)
				{
					this.isPaint=true;
					base.OnPaintBackground(pevent);
					this.isPaint=false;					
				}				
			}
			catch(Exception ex)
			{
				isPaint=false;
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard protected override void OnPaintBackground(PaintEventArgs pevent)",ex,"",false);
			}
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				//Trace.WriteLine("protected override void OnPaint(PaintEventArgs e)");
				if(isPictureAssign==false)
				{
					this.isPaint=true;
					base.OnPaint(e);
					isPaint=false;
				}
				//Trace.WriteLine("protected override void OnPaint(PaintEventArgs e)");
			}
			catch(Exception ex)
			{
				isPaint=false;
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard protected override void OnPaintBackground(PaintEventArgs pevent)",ex,"",false);
			}
		}
	}
}
