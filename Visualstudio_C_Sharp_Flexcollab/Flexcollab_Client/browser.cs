using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;


using System.Data;
using System.Windows.Forms;
using WebMeeting;
using WebMeeting.Common;
using System.Threading;
using WebMeeting.Client;
using WebBrowserSubClassing;
using WebMeeting.Client.Alerts;
using mshtml;
using WebMeeting.Client.WhiteBoard;


// this section added by Zaeem for the web page to Jpg conversion 
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using SHDocVw;


// For Image Compression 
using ImageQuantization;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;

using Win32;



namespace WebMeeting
{
	/// <summary>
	/// Summary description for browser.
	/// </summary>
	public class browser : System.Windows.Forms.UserControl
	{



		#region Thread's variable declaration Used in Web To image
		public Thread consumeThread;
		public Thread sendingThread;
		#endregion 

		#region Other varable declarion 
		public int Global_width=0, Global_height=0;
		private bool annotationsEnabled = false;
		public ArrayList msgPool;
		public int sessionID;
		private bool m_bActive;
		private System.ComponentModel.IContainer components;


		public int BrowserDoc_count=0;
		public int ClientID = -1;
		public bool IsAttendee = false;
		public bool IsClosed = false;
		private System.Windows.Forms.Panel panelButtons;
		private System.Windows.Forms.TextBox txtUrl;
		private NSPAControls.NSButton btnBack;
		private NSPAControls.NSButton btnForward;
		private NSPAControls.NSButton btnHome;
		private NSPAControls.NSButton btnGo;
		private NSPAControls.NSButton btnShare;
		private NSPAControls.NSButton btnLogg;
		private  NJFLib.Controls.CollapsibleSplitter splitter1;
		public AxSHDocVw.AxWebBrowser axWebBrowser1;
		public MessageObject intialMsg;
		private System.Windows.Forms.CheckBox chk_ShowAnnotation;
		public System.Windows.Forms.CheckBox chk_AllowAnnotation;
		public bool isSharedBrowser=false;
		public System.Windows.Forms.Panel panel1;
		public  System.Windows.Forms.CheckBox chk_Autoshare;
		private NSPAControls.NSButton btnLog;
		public System.Windows.Forms.CheckBox chk_Imagemode; // this is a member message object that is intialized with the 
		//last message of the array recieved from server when this client
		//connects the server.
			
		
		public WebBrowserSubClassing.IEHwd annotationBrowser = new IEHwd();
		private System.Windows.Forms.ToolTip toolTip1;
			
		//public WebMeetingDrawingBoard.WhiteboardControl whiteBoard;
		public   Whiteboard whiteBoard;
			
		#endregion 

		#region Browse Control's constructor
		public browser()
		{
			
			try
			{
				// This call is required by the Windows.Forms Form Designer.
				InitializeComponent();
				//InitializeWhiteboard();
				whiteBoard = new Whiteboard();
			
				whiteBoard.Visible=false;
				this.panel1.Controls.Add(whiteBoard);
				this.InitializeWhiteboard();

				consumeThread=new Thread(new ThreadStart(consumeFunction));


				m_bActive=true;
				sessionID=-1;
				msgPool=new ArrayList();
			
			
				//Get the unique session id here;
				//			//NetworkManager
				// TODO: Add any initialization after the InitializeComponent call
				this.splitter1.SplitterThumbBackColoor = System.Drawing.Color.Empty;
				splitter1.SplitterThumbBackColoor = Color.DarkBlue;
				annotationBrowser.AddAnnotations = false;

			

				/*
				if(WebMeeting.Client.NetworkManager.getInstance().profile.clientType==ClientType.ClientAttendee)
					chk_AllowAnnotation.Checked=true;
				if(WebMeeting.Client.NetworkManager.getInstance().profile.clientType==ClientType.ClientPresenter)
					chk_AllowAnnotation.Checked=true;
				chk_ShowAnnotation.Checked = false;
				if(WebMeeting.Client.NetworkManager.getInstance().profile.clientType==ClientType.ClientAttendee)

				{
					this.splitter1.ToggleState();
					this.splitter1.Visible=false;
				}		
				*/
				chk_ShowAnnotation.Checked = false;
				
				if (ClientUI.getInstance()!=null)
				{
					if (ClientUI.getInstance().IfIamthePresenter())
					{
						if(chk_AllowAnnotation.Checked)
						chk_AllowAnnotation.Checked = false;								   
					}		
					else
					{
						if(!chk_AllowAnnotation.Checked)
						chk_AllowAnnotation.Checked = true;
						this.splitter1.ToggleState();
						this.splitter1.Visible=false;		   
					
					}
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public browser()",ex,"",false);
			}

		}

		# endregion 

		#region ShowSplitter
		public void ShowSplitter(bool IsVisible)
		{
			try
			{
				if(!IsVisible)
				{
					if(!this.splitter1.IsCollapsed)
					{
						this.splitter1.ToggleState();
						this.splitter1.Visible=false;
					}
					else
					{
						this.splitter1.Visible=false;
					}
				}

				if(IsVisible)
				{
					if(this.splitter1.IsCollapsed)
					{
						this.splitter1.ToggleState();
						this.splitter1.Visible=true;
					}
					else
					{
						this.splitter1.Visible=true;
					}
				}				
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void ShowSplitter(bool IsVisible)",ex,"",false);
			}
		}

		# endregion 
	
		# region GetSessionID()
		public void GetSessionID()
		{
			try
			{	
				NetworkManager.getInstance().GetUniqueSessionID(ref sessionID);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void GetSessionID()",ex,"",false);
			}
		}		
		# endregion 

		#region navigateTo(string strUrl)
		public void navigateTo(string strUrl)
		{
			try
			{
				object oUrl = (object)strUrl;
				object o = new object();	
				this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void navigateTo(string strUrl)",ex,"",false);
			}
		}
		#endregion 

		#region Consume Thread's Function 
		private void consumeFunction()
		{			
			try
			{
				while(m_bActive==true)
				{
					if(msgPool.Count>0)
					{
					
						MsgWebBrowseMessage msg=(MsgWebBrowseMessage) msgPool[0];
						msgPool.RemoveAt(0);
						if(this.sessionID==msg.webBrowseID)
						{
							if(msg.active)
							{
								object oUrl = (object)msg.navigateToUrl;
								object o = new object();	
								this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);

							}
							else
							{
								m_bActive=false;
							}
						}
					}
					
					Thread.Sleep(100);
				}
			}

			catch(System.Threading.ThreadAbortException exp)
			{
				exp=exp;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void consumeFunction()",ex,"",false);
			}
		}

		#endregion 

		#region  Terminate()
		public void Terminate()
		{
			try
			{

				
				if(ClientUI.getInstance().IfIamthePresenter())
				{
				
					//this.DisposeBrowser();
					new Thread(new ThreadStart(this.DisposeBrowser)).Start();
					this.panel1.Controls.Clear();
					this.panelButtons.Controls.Clear();
					this.panel1.Dispose();
					this.panelButtons.Dispose();
					this.CleanandClear();
				}
				else
				{
					this.CleanandClear();
					return;
				}

				
				//this.Dispose(true);
				try
				{
					annotationBrowser.DisposeResources();
					annotationBrowser.abortThreads();
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void Terminate()    299",ex,"",false);
				}
			
				if(ClientUI.getInstance().IfIamthePresenter())
				{
				
					try
					{
						m_bActive=false;

						if(axWebBrowser1!=null)
						{
							System.IntPtr pointer = annotationBrowser.IEFromhWnd(axWebBrowser1.Handle);
							annotationBrowser.StopSubclass(pointer);
						}
				
						consumeThread.Abort();
						consumeThread.Join(10000);
					}
					catch(Exception ex)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void Terminate() 317",ex,"",false);
					}
				}

				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void Terminate() 325",ex,"",false);
			}
			
		}

		#endregion 






		#region  Terminate_temp() 
		public void Terminate_temp()
		{
			try
			{

				
				

					//this.DisposeBrowser();
					this.panel1.Controls.Clear();
					this.panelButtons.Controls.Clear();
					this.panel1.Dispose();
					this.panelButtons.Dispose();
					this.CleanandClear();
				
				
				//this.Dispose(true);
				try
				{
					annotationBrowser.DisposeResources();
					annotationBrowser.abortThreads();
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void Terminate()    299",ex,"",false);
				}
			
				try
				{
					m_bActive=false;

					if(axWebBrowser1!=null)
					{
						System.IntPtr pointer = annotationBrowser.IEFromhWnd(axWebBrowser1.Handle);
						annotationBrowser.StopSubclass(pointer);
					}
				
					consumeThread.Abort();
					consumeThread.Join(10000);
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void Terminate() 317",ex,"",false);
				}
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void Terminate() 325",ex,"",false);
			}
			
		}

		#endregion 



		#region  SetSize(int width,int height)
		/// <summary>
		/// This function set the size of the browser control equal to the given size
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void SetSize(int width,int height)
		{
			try
			{
				this.Width=width;
				this.Height=height;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void SetSize(int width,int height)",ex,"",false);
			}
		}
		#endregion 
		
		#region PresenterInterface()
		/// <summary>
		/// This function makes the presenter's interface for the browser control
		/// </summary>
		public void PresenterInterface()
		{
			try
			{
				GetSessionID();
				this.btnGo.Visible=true;
				this.txtUrl.Visible=true;
				this.btnShare.Visible=true;			
				this.btnLogg.Visible=true;			
				this.btnBack.Visible=true;
				this.btnForward.Visible=true;
				this.btnHome.Visible=true;
				chk_AllowAnnotation.Enabled = false;
				chk_ShowAnnotation.Checked = false;
				//	this.txtUrl.Left=10;
				//	this.txtUrl.Width=this.axWebBrowser1.Width - 140;
				//	this.btnGo.Left=this.txtUrl.Width + this.txtUrl.Left + 10;
				//	this.btnGo.Width=50;
				//this.btnGo.Height=this.txtUrl.Height;

				//	this.btnShare.Left=this.btnGo.Left + this.btnGo.Width + 10;
				IsAttendee = false;

				//	this.btnBack.Left=10;
				//	this.btnForward.Left=this.btnBack.Width + this.btnBack.Left + 20;
				//	this.btnHome.Left=this.btnForward.Width + this.btnForward.Left + 20;
				//	*/
				//object oUrl = (object)Info.getInstance().WebsiteName; 
				object oUrl = (object)"www.uraan.net"; 
				object o = new object();	
				this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
				/*
				object oUrl = (object)"";
				object o = new object();	
				this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
				*/
				annotationBrowser.thisSessionID = sessionID;			
				annotationBrowser.annotationType = AnnotationMessageType.WEB;

				if(consumeThread.ThreadState==System.Threading.ThreadState.Unstarted)
					consumeThread.Start();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void PresenterInterface()",ex,"",false);
			}            
		}

		# endregion 

		#region AttendeeInterface
		/// <summary>
		/// This function makes the presenter's interface for the browser control
		/// </summary>

		public void AttendeeInterface()
		{
	
			try
			{
				IsAttendee = true;
				this.chk_Autoshare.Visible = false;
				this.btnGo.Visible = false;
				this.btnGo.Enabled=false;
				this.txtUrl.Visible=false;
				this.btnShare.Visible=false;
				this.btnLogg.Visible=false;
				this.btnBack.Visible=true;
				this.btnForward.Visible=true;
				this.btnHome.Visible=false;
				chk_AllowAnnotation.Enabled = true;
		
				/*	this.axWebBrowser1.Left=10;
					this.axWebBrowser1.Top=10;
					this.axWebBrowser1.Width= this.Width -  axWebBrowser1.Left - 10;
					this.axWebBrowser1.Height= this.Height - 130;
				*/	
				splitter1.SplitPosition += 10;

				if(consumeThread.ThreadState==System.Threading.ThreadState.Unstarted)
					consumeThread.Start();
          	
				chk_ShowAnnotation.Visible= !IsClosed;
				chk_AllowAnnotation.Visible = true;
				chk_ShowAnnotation.Checked = false;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void AttendeeInterface()",ex,"",false);
			}
		}
		# endregion 

		#region Dispose
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
		
			try
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
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing==> browser.cs  Dispose(",ex,"",false);
			}

		}
		#endregion 

		#region InitializeWhiteboard()
		/// <summary>
		/// Made by Zaeem 
		/// initialise and start the whiteboard Threads. 
		/// </summary>
		public void InitializeWhiteboard()
		{
			
			consumeThread =new Thread(new ThreadStart(whiteBoard.ConsumeThread));
			sendingThread=new Thread(new ThreadStart(whiteBoard.SendingThread));
			
			
			consumeThread.Start();
			sendingThread.Start();
			


			
			}
		#endregion 

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(browser));
			this.panelButtons = new System.Windows.Forms.Panel();
			this.chk_Imagemode = new System.Windows.Forms.CheckBox();
			this.btnLog = new NSPAControls.NSButton();
			this.chk_AllowAnnotation = new System.Windows.Forms.CheckBox();
			this.chk_ShowAnnotation = new System.Windows.Forms.CheckBox();
			this.btnShare = new NSPAControls.NSButton();
			this.btnLogg = new NSPAControls.NSButton();
			this.btnGo = new NSPAControls.NSButton();
			this.btnHome = new NSPAControls.NSButton();
			this.btnForward = new NSPAControls.NSButton();
			this.btnBack = new NSPAControls.NSButton();
			this.txtUrl = new System.Windows.Forms.TextBox();
			this.chk_Autoshare = new System.Windows.Forms.CheckBox();
			this.splitter1 = new NJFLib.Controls.CollapsibleSplitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.panelButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelButtons
			// 
			this.panelButtons.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.panelButtons.Controls.Add(this.chk_Imagemode);
			this.panelButtons.Controls.Add(this.btnLog);
			this.panelButtons.Controls.Add(this.chk_AllowAnnotation);
			this.panelButtons.Controls.Add(this.chk_ShowAnnotation);
			this.panelButtons.Controls.Add(this.btnShare);
			this.panelButtons.Controls.Add(this.btnLogg);
			this.panelButtons.Controls.Add(this.btnGo);
			this.panelButtons.Controls.Add(this.btnHome);
			this.panelButtons.Controls.Add(this.btnForward);
			this.panelButtons.Controls.Add(this.btnBack);
			this.panelButtons.Controls.Add(this.txtUrl);
			this.panelButtons.Controls.Add(this.chk_Autoshare);
			this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelButtons.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.panelButtons.Location = new System.Drawing.Point(0, 296);
			this.panelButtons.Name = "panelButtons";
			this.panelButtons.Size = new System.Drawing.Size(488, 104);
			this.panelButtons.TabIndex = 8;
			// 
			// chk_Imagemode
			// 
			this.chk_Imagemode.Enabled = false;
			this.chk_Imagemode.Location = new System.Drawing.Point(258, 75);
			this.chk_Imagemode.Name = "chk_Imagemode";
			this.chk_Imagemode.Size = new System.Drawing.Size(99, 24);
			this.chk_Imagemode.TabIndex = 23;
			this.chk_Imagemode.Text = "Image Mode";
			this.toolTip1.SetToolTip(this.chk_Imagemode, "Caution : Only select the image mode when you come across the flashy sites or get" +
				"ting problems in annotation.");
			this.chk_Imagemode.CheckedChanged += new System.EventHandler(this.chk_Imagemode_CheckedChanged);
			// 
			// btnLog
			// 
			this.btnLog.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnLog.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnLog.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnLog.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnLog.HottrackImage")));
			this.btnLog.Location = new System.Drawing.Point(406, 14);
			this.btnLog.Name = "btnLog";
			this.btnLog.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnLog.NormalImage")));
			this.btnLog.OnlyShowBitmap = true;
			this.btnLog.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnLog.PressedImage")));
			this.btnLog.Size = new System.Drawing.Size(63, 28);
			this.btnLog.Text = "nsButton1";
			this.btnLog.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnLog.ToolTip = null;
			this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
			// 
			// chk_AllowAnnotation
			// 
			this.chk_AllowAnnotation.Enabled = false;
			this.chk_AllowAnnotation.Location = new System.Drawing.Point(134, 73);
			this.chk_AllowAnnotation.Name = "chk_AllowAnnotation";
			this.chk_AllowAnnotation.Size = new System.Drawing.Size(115, 24);
			this.chk_AllowAnnotation.TabIndex = 16;
			this.chk_AllowAnnotation.Text = "Add Annotations";
			this.chk_AllowAnnotation.CheckedChanged += new System.EventHandler(this.chk_AllowAnnotation_CheckedChanged);
			// 
			// chk_ShowAnnotation
			// 
			this.chk_ShowAnnotation.Enabled = false;
			this.chk_ShowAnnotation.Location = new System.Drawing.Point(134, 52);
			this.chk_ShowAnnotation.Name = "chk_ShowAnnotation";
			this.chk_ShowAnnotation.Size = new System.Drawing.Size(115, 24);
			this.chk_ShowAnnotation.TabIndex = 15;
			this.chk_ShowAnnotation.Text = "Show Annotations";
			this.chk_ShowAnnotation.CheckedChanged += new System.EventHandler(this.chk_ShowAnnotation_CheckedChanged);
			// 
			// btnShare
			// 
			this.btnShare.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnShare.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnShare.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnShare.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnShare.HottrackImage")));
			this.btnShare.Location = new System.Drawing.Point(343, 14);
			this.btnShare.Name = "btnShare";
			this.btnShare.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnShare.NormalImage")));
			this.btnShare.OnlyShowBitmap = true;
			this.btnShare.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnShare.PressedImage")));
			this.btnShare.Size = new System.Drawing.Size(63, 28);
			this.btnShare.Text = "nsButton1";
			this.btnShare.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnShare.ToolTip = null;
			this.btnShare.Click += new System.EventHandler(this.btnShare_Click);
			// 
			// btnLogg
			// 
			this.btnLogg.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnLogg.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnLogg.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnLogg.HottrackImage = null;
			this.btnLogg.Location = new System.Drawing.Point(346, 14);
			this.btnLogg.Name = "btnLogg";
			this.btnLogg.NormalImage = null;
			this.btnLogg.OnlyShowBitmap = true;
			this.btnLogg.PressedImage = null;
			this.btnLogg.Size = new System.Drawing.Size(63, 28);
			this.btnLogg.Text = "nsButton1";
			this.btnLogg.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnLogg.ToolTip = null;
			// 
			// btnGo
			// 
			this.btnGo.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnGo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnGo.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnGo.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnGo.HottrackImage")));
			this.btnGo.Location = new System.Drawing.Point(312, 14);
			this.btnGo.Name = "btnGo";
			this.btnGo.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnGo.NormalImage")));
			this.btnGo.OnlyShowBitmap = true;
			this.btnGo.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnGo.PressedImage")));
			this.btnGo.Size = new System.Drawing.Size(25, 26);
			this.btnGo.Text = "nsButton1";
			this.btnGo.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnGo.ToolTip = null;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// btnHome
			// 
			this.btnHome.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnHome.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnHome.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnHome.HottrackImage")));
			this.btnHome.Location = new System.Drawing.Point(87, 50);
			this.btnHome.Name = "btnHome";
			this.btnHome.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnHome.NormalImage")));
			this.btnHome.OnlyShowBitmap = true;
			this.btnHome.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnHome.PressedImage")));
			this.btnHome.Size = new System.Drawing.Size(33, 27);
			this.btnHome.Text = "nsButton1";
			this.btnHome.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnHome.ToolTip = null;
			this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
			// 
			// btnForward
			// 
			this.btnForward.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnForward.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnForward.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnForward.HottrackImage")));
			this.btnForward.Location = new System.Drawing.Point(55, 50);
			this.btnForward.Name = "btnForward";
			this.btnForward.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnForward.NormalImage")));
			this.btnForward.OnlyShowBitmap = true;
			this.btnForward.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnForward.PressedImage")));
			this.btnForward.Size = new System.Drawing.Size(28, 28);
			this.btnForward.Text = "nsButton1";
			this.btnForward.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnForward.ToolTip = null;
			this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
			// 
			// btnBack
			// 
			this.btnBack.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnBack.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnBack.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnBack.HottrackImage")));
			this.btnBack.Location = new System.Drawing.Point(23, 50);
			this.btnBack.Name = "btnBack";
			this.btnBack.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnBack.NormalImage")));
			this.btnBack.OnlyShowBitmap = true;
			this.btnBack.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnBack.PressedImage")));
			this.btnBack.Size = new System.Drawing.Size(28, 28);
			this.btnBack.Text = "nsButton1";
			this.btnBack.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnBack.ToolTip = null;
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			// 
			// txtUrl
			// 
			this.txtUrl.Location = new System.Drawing.Point(23, 18);
			this.txtUrl.Name = "txtUrl";
			this.txtUrl.Size = new System.Drawing.Size(280, 21);
			this.txtUrl.TabIndex = 8;
			this.txtUrl.Text = "http://www.uraan.net";
			this.txtUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUrl_KeyDown);
			this.txtUrl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUrl_KeyPress);
			// 
			// chk_Autoshare
			// 
			this.chk_Autoshare.Enabled = false;
			this.chk_Autoshare.Location = new System.Drawing.Point(258, 48);
			this.chk_Autoshare.Name = "chk_Autoshare";
			this.chk_Autoshare.Size = new System.Drawing.Size(96, 32);
			this.chk_Autoshare.TabIndex = 15;
			this.chk_Autoshare.Text = "Auto Share";
			this.chk_Autoshare.CheckedChanged += new System.EventHandler(this.chk_Autoshare_CheckedChanged);
			// 
			// splitter1
			// 
			this.splitter1.AnimationDelay = 20;
			this.splitter1.AnimationStep = 20;
			this.splitter1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(250)), ((System.Byte)(162)), ((System.Byte)(37)));
			this.splitter1.BorderStyle3D = System.Windows.Forms.Border3DStyle.Flat;
			this.splitter1.ControlToHide = this.panelButtons;
			this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.ExpandParentForm = false;
			this.splitter1.Location = new System.Drawing.Point(0, 288);
			this.splitter1.Name = "splitter1";
			this.splitter1.SplitterThumbBackColoor = System.Drawing.Color.Transparent;
			this.splitter1.TabIndex = 7;
			this.splitter1.TabStop = false;
			this.splitter1.UseAnimations = false;
			this.splitter1.VisualStyle = NJFLib.Controls.VisualStyles.XP;
			this.splitter1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter1_SplitterMoved);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(488, 288);
			this.panel1.TabIndex = 9;
			// 
			// browser
			// 
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panelButtons);
			this.Name = "browser";
			this.Size = new System.Drawing.Size(488, 400);
			this.Load += new System.EventHandler(this.browser_Load);
			this.panelButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region btnGo_Click(object sender, System.EventArgs e)
		private void btnGo_Click(object sender, System.EventArgs e)
		{
			chk_Autoshare.Enabled=false;
			chk_ShowAnnotation.Enabled=false;
			chk_AllowAnnotation.Enabled=false;
			chk_Imagemode.Enabled=false;

			try
			{
				object oUrl = (object)this.txtUrl.Text;
				object o = new object();	
				this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void btnGo_Click(object sender, System.EventArgs e)",ex,"",false);
			}
		}
		# endregion 

		#region SendCloseMessage()
		public void SendCloseMessage()
		{
			try
			{
				MsgWebBrowseMessage msg=new MsgWebBrowseMessage(NetworkManager.getInstance().profile.ClientId);
				msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
				msg.webBrowseID=sessionID;			
				msg.active = false;
				msg.navigateToUrl="";
				NetworkManager.thisInstance.SendLoadPacket(msg);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void SendCloseMessage()",ex,"",false);
			}

			//NetworkManager.getInstance().SendLoadPacket(msg);
		}

		#endregion 

		#region ShareNewAttendee()
		public void ShareNewAttendee()
		{			
			try
			{
			
				this.btnShare_Click(this,null);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public bool ShowClose()",ex,"",false);
			}
			
		}

		#endregion 

		#region btnShare_Click(object sender, System.EventArgs e)
		private void btnShare_Click(object sender, System.EventArgs e)
		{
			try
			{
				MsgWebBrowseMessage msg=new MsgWebBrowseMessage(NetworkManager.getInstance().profile.ClientId);
				msg.ConferenceID =  NetworkManager.thisInstance.profile.ConferenceID ;
				msg.webBrowseID=sessionID;			
				msg.navigateToUrl=this.axWebBrowser1.LocationURL;
				
				this.txtUrl.Text=this.axWebBrowser1.LocationURL.ToString();
				this.isSharedBrowser=true;
				//this.isShared=true;
				NetworkManager.getInstance().SendLoadPacket(msg);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void btnShare_Click(object sender, System.EventArgs e)",ex,"",false);
			}
			// send this over the network;
		}

		#endregion 

		#region browser_Load(object sender, System.EventArgs e)
		private void browser_Load(object sender, System.EventArgs e)
		{
			try
			{
				System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(browser));
				this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
				((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
				this.axWebBrowser1.ContainingControl = this;
				this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
				this.axWebBrowser1.Enabled = true;
				this.axWebBrowser1.Location = new System.Drawing.Point(0, 0);
				this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
				this.axWebBrowser1.Size = new System.Drawing.Size(488, 288);
				this.axWebBrowser1.TabIndex = 0;
				this.axWebBrowser1.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(this.axWebBrowser1_NavigateComplete2);
				this.axWebBrowser1.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.axWebBrowser1_DocComplete);
				
				this.axWebBrowser1.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.axWebBrowser1_BeforeNavigate2);
				this.panel1.Controls.Add(this.axWebBrowser1);
				//this.Controls.Add(this.axWebBrowser1);
				((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();	
				//if(WebMeeting.Client.NetworkManager.getInstance().profile.clientType==ClientType.ClientAttendee)
					EnableAnnotation();
				//if(WebMeeting.Client.NetworkManager.getInstance().profile.clientType==ClientType.ClientPresenter)
				//	EnableAnnotation();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void browser_Load(object sender, System.EventArgs e)",ex,"",false);
			}
		}

		#endregion 

		#region btnBack_Click(object sender, System.EventArgs e)
		private void btnBack_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.axWebBrowser1.GoBack();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void btnBack_Click(object sender, System.EventArgs e)",ex,"",false);
			}
		}

		#endregion 

		#region btnForward_Click(object sender, System.EventArgs e)
		private void btnForward_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.axWebBrowser1.GoForward();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void btnForward_Click(object sender, System.EventArgs e)",ex,"",false);
				//				ee=ee;
			}
		}
		#endregion
		
		#region  btnHome_Click(object sender, System.EventArgs e)
		private void btnHome_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.axWebBrowser1.GoHome();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void btnHome_Click(object sender, System.EventArgs e)",ex,"",false);
			}
		}
		#endregion 

		#region WndProc(ref Message m)
		protected override void WndProc(ref Message m)
		{
		
			try
			{
				
				base.WndProc(ref m);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void btnHome_Click(object sender, System.EventArgs e)",ex,"",true);
				//				MeetingAlerts alert=new MeetingAlerts();
				//				alert.ShowMessage(Client.Alerts.MeetingAlerts.AlertType.NonFatal,ee.Message,true,false);
				//MessageBox.Show(ee.Message + " " + ee.StackTrace);
			}
		}	

		#endregion 

		#region splitter1_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		private void splitter1_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
		
		
			//	this.btnGo.Top=this.splitter1.Top  + 20;
			/*	this.txtUrl.Top=this.splitter1.Top;
				this.btnShare.Top=this.btnGo.Top;
				int X = 0;
				*/
			//splitter1.is
			try
			{
				if(NetworkManager.getInstance().profile.clientType== ClientType.ClientAttendee) // for restricting attendees)
				{
					this.btnForward.Top=txtUrl.Top;
					this.btnBack.Top=txtUrl.Top;
					this.btnHome.Top=txtUrl.Top;
					this.chk_ShowAnnotation.Top =txtUrl.Top;
					this.chk_AllowAnnotation.Top = btnHome.Top;
				}			
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void btnHome_Click(object sender, System.EventArgs e)",ex,"",false);
			}
		}

		#endregion 
			
		#region ClearIt()
		public void ClearIt()
		{
			try
			{
				DrawingClearMessage msg = new DrawingClearMessage(sessionID);
				msg.m_ControlType = ControlType.Browser;
				msg.SenderID = NetworkManager.getInstance().profile.ClientId;
				msg.senderProfile = NetworkManager.getInstance().profile;					
				NetworkManager.getInstance().SendLoadPacket(msg);
				if(NetworkManager.getInstance().profile.clientType == ClientType.ClientHost)
					annotationBrowser.ClearItems(true,0);
				else
					annotationBrowser.ClearItems(false,NetworkManager.getInstance().profile.ClientId);			        

				Refresh();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void ClearIt()",ex,"",false);
			}

		}
		
		#endregion
		
		
		#region  ShareXLSDocument()
		public void ShareXLSDocument()
		{
			
		}

		#endregion

		

		private void chk_ShowAnnotation_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if((chk_ShowAnnotation.Checked))
				{
					if(!annotationsEnabled)
					{
						System.IntPtr pointer = annotationBrowser.IEFromhWnd(axWebBrowser1.Handle);
						if(pointer == System.IntPtr.Zero)
						{
							Client.ClientUI.getInstance().ShowExceptionMessage("Unable to initialize annotations. Annotations may not work properly");
						
						}
						else
						{
							annotationBrowser.StartSubclass(pointer,axWebBrowser1);
						}

						annotationBrowser.DrawNow = true;
						annotationBrowser.addAnnotations = true;
						annotationBrowser.annotationType = AnnotationMessageType.WEB;
						annotationBrowser.thisSessionID = sessionID;
					
						annotationsEnabled = true;

					}				
				}

				annotationBrowser.ShowAnnotations= chk_ShowAnnotation.Checked;

				if(!chk_ShowAnnotation.Checked)
					chk_AllowAnnotation.Checked = false;

				annotationBrowser.AddAnnotations = chk_AllowAnnotation.Checked;
				annotationBrowser.SendEraseMessage();			
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void chk_ShowAnnotation_CheckedChanged(object sender, System.EventArgs e)",ex,"",false);
			}
		}
		
		public void DisposeBrowser()
		{
			try
			{
					this.axWebBrowser1.Stop();
					this.axWebBrowser1.Dispose();
					this.axWebBrowser1=null;
				
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void DisposeBrowser()",ex,"",true);			
			}			
		}

		public void RecallCheckBOx()
		{
			try
			{
				
				chk_AllowAnnotation_CheckedChanged(chk_AllowAnnotation,new EventArgs());
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  public void RecallCheckBOx()",ex,"",false);
			}



		}





		private void chk_AllowAnnotation_CheckedChanged(object sender, System.EventArgs e)
		{
			
			try
			{
				annotationBrowser.bBlockMouseMovment = chk_AllowAnnotation.Checked;
				System.IntPtr pointer=new IntPtr(1); 
				if((chk_AllowAnnotation.Checked))
				{
					if(!annotationsEnabled)
					{
						
							pointer = annotationBrowser.IEFromhWnd(axWebBrowser1.Handle);
							if(pointer == System.IntPtr.Zero)
							{	
								//	Client.ClientUI.getInstance().ShowExceptionMessage("Unable to initialize annotations. Annotations may not work properly");						
								Thread _timer = new Thread(new ThreadStart(RecallCheckBOx));
								_timer.Name = "browser RecallCheckBOx Thread: RecallCheckBOx()";
								_timer.Start();
								return;
							}
							else
							{
								annotationBrowser.StartSubclass(pointer,axWebBrowser1);
							}
						
						
						annotationBrowser.DrawNow = true;
						annotationBrowser.addAnnotations = true;
						annotationBrowser.annotationType = AnnotationMessageType.WEB;
						annotationBrowser.thisSessionID = sessionID;
						//MessageBox.Show("Annotations Enabled","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
						annotationsEnabled = true;
					}				
				}
				annotationBrowser.AddAnnotations = chk_AllowAnnotation.Checked;

				if(chk_AllowAnnotation.Checked)
				{	
					chk_ShowAnnotation.Checked = true;
					annotationBrowser.SendEraseMessage();				
				}
				if(axWebBrowser1!=null)
				{
					if(this.axWebBrowser1.Document!= null)
					{
						mshtml.IHTMLDocument2 HTMLDocument;
						HTMLDocument = (IHTMLDocument2) this.axWebBrowser1.Document;
						//HTMLDocument.parentWindow.
						//HTMLParaElement parentEle=HTMLDocument.parentWindow;
				
				
						if(HTMLDocument.frames.length>0)
						{					
							IHTMLFramesCollection2 frames = (IHTMLFramesCollection2)HTMLDocument.frames;
							for (int index = 0; index < frames.length; index++)
							{
								object i = index;
								frames.item(ref i);

								IHTMLWindow2 window = (IHTMLWindow2)frames.item(ref i);
								//window.onscroll+=
								//((HTMLWindowEvents2_Event)window).onscroll +=new HTMLWindowEvents2_onscrollEventHandler(this.showscroll);
								//IHTMLDocument2 frameDocument = (IHTMLDocument2)window.document;
						

								//Console.WriteLine(frameDocument.body.innerHTML);
							} 
							//IHTMLFramesCollection2 cframe=HTMLDocument.frames.item(0);
					
							//MessageBox.Show("length of the frames :"+ HTMLDocument.frames.length.ToString());
						}



						if(chk_AllowAnnotation.Checked==true)
						{
						
						
							mshtml.IHTMLElementCollection cc=HTMLDocument.all;
							foreach (mshtml.IHTMLElement anObj in cc) 
							{ 
								anObj.style.cursor="default";					
							} 
							//cc.	
					

						}
						else
						{
							mshtml.IHTMLElementCollection cc=HTMLDocument.all;
							foreach (mshtml.IHTMLElement anObj in cc) 
							{ 
								//if(anObj.tagName=="A")						
								anObj.style.cursor="";
								//if(anObj.tagName=="BODY")						
								//	anObj.style.cursor="";
								//						if (anObj.innerText != null) 
								//						{ 
								//							if (anObj.innerText != "") 
								//							{ 
								//								if ((emailReg.IsMatch(anObj.innerText))&
								//									(anObj.innerText!="")) aQueue.Enqueue(
								//															   anObj.innerText); 
								//							} 
								//						} 
							} 
						}
					}
				}
			}
			catch(Exception ex)
			{
				//Definate Exception 
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void chk_AllowAnnotation_CheckedChanged(object sender, System.EventArgs e)",ex,"",false);
				
			}
		
		}





		#region Browser to image



		/// <summary>
		/// This function convert the web page to image and retuen an image object 
		/// depending upon the strategy choosed.
		/// There are two strtegies which are currently implemented 
		/// 1 for the pages having single pages and the other one for the pages having multiple docs like CNN .com
		/// </summary>
		/// <param name="bool_Multidoc"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public Image TakeBrowseJPG_Image(bool bool_Multidoc,string path)
		{


			#region Buffer 	initailsation 
			MemoryStream buffer = new MemoryStream();
			#endregion 
			// Reduce Resolution Size
			double myResolution = Convert.ToDouble(100) * 0.01;
			long myQuality = 60;//Convert.ToInt64(cmbQuality.Text);
			//Write Image.
			EncoderParameters eps = new EncoderParameters(1);
			ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
			

			try
			{
				// If the page having multple docs.
				if(bool_Multidoc)
				{

					#region Jpg conversion for Multidoc pages
					
					#region variable initialiseation 
					

					mshtml.IHTMLDocument2 myDoc = (mshtml.IHTMLDocument2)this.axWebBrowser1.Document;
					
					
					
					int heightsize = (int)myDoc.body.getAttribute("scrollHeight", 0);
					int widthsize = (int)myDoc.body.getAttribute("scrollWidth", 0);
			
					int screenHeight = this.axWebBrowser1.Height;
					int screenWidth = this.axWebBrowser1.Width;

					int myPageWidth = 0;
					int count =0;


					//Create a target bitmap to draw into.
					Bitmap bm2 = new Bitmap(widthsize /*+ URLExtraLeft*/, heightsize+300 /*+ URLExtraHeight - trimHeight */, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
					Graphics g2 = Graphics.FromImage(bm2);
					g2.Clear(Color.White);
			
					Bitmap bm; 

			
					Graphics g = null;
					IntPtr hdc;
					Image screenfrag = null;
					int brwTop = 0;
					int brwLeft = 0;
					int myPage = 0;
					IntPtr myIntptr = (IntPtr)this.axWebBrowser1.Handle;
					//Get inner browser window.
					int hwndInt = myIntptr.ToInt32();
					IntPtr hwnd = myIntptr;
					hwnd = (IntPtr)Win32.USER32.GetWindow(hwnd, Win32.USER32.GW_CHILD); 
					StringBuilder sbc = new StringBuilder(256);
					
					# endregion 
					
					#region Get Browser "Document" Handle
					while (hwndInt != 0) 
					{ 
						hwndInt = hwnd.ToInt32();
						Win32.USER32.GetClassName(hwndInt, sbc, 256);

						if(sbc.ToString().IndexOf("Shell DocObject View", 0) > -1)
						{
							hwnd = Win32.USER32.FindWindowEx(hwnd, IntPtr.Zero, "Internet Explorer_Server", IntPtr.Zero);
							break;
						}                
						hwnd = (System.IntPtr)Win32.USER32.GetWindow(hwnd, Win32.USER32.GW_HWNDNEXT);

					}
					# endregion 
            
					#region		Get Screen Height (for bottom up screen drawing)
					while ((myPage * screenHeight) < heightsize)
					{
						//myDoc.body.setAttribute("scrollTop", (screenHeight - 5) * myPage, 0);
						++myPage;
					}
					//Rollback the page count by one
					--myPage;
					#endregion 

					#region Some screen Height calculations 

					if( screenWidth < widthsize)
					{
						screenHeight-=22;
				
					}
					//Get bitmap to hold screen fragment.
					bm = new Bitmap(screenWidth, screenHeight, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
					#endregion 

					#region SetCursor's Position to click on the browser to make it focused 
					Point p = Cursor.Position;
					p.X-=250;
					p.Y-=100;
			
					# endregion 
	
					myPageWidth=0; 

					#region Main While loop for full page Nevigation 
					int limit=8;
					int whilecounter=0;

					// In Case of H Scroll it will run Multiple Times 
					while ((myPageWidth * screenWidth) < widthsize)
					{
						++whilecounter;
						
						#region For Setting the initialsed condition of 
						
						if(whilecounter<=1)
						{
							for (int j =0 ; j <=myPage; j++)
							{
						
								if(j==0)
								{
									for(int k=0;k<16;k++)
									{
										this.SendClick_LEFTDOWN(p);
										this.GenerateKeyboardEvent("Left",p);
										Application.DoEvents();
									}
								}
								for(int l=0;l<8;l++)
								{
									this.SendClick_LEFTDOWN(p);
									this.GenerateKeyboardEvent("Up",p);
									Application.DoEvents();
					
								}
							}
						}
						
						#endregion 



						// For Vertical Scroll Bar
						for (int i =0 ; i <=myPage; i++)
						{

				
							g = Graphics.FromImage(bm);
							hdc = g.GetHdc();
							Win32.USER32.PrintWindow(hwnd, hdc, 0);
							g.ReleaseHdc(hdc);
							g.Flush();



				
							if(i-1==myPage)
							{
								screenfrag=(Image)this.Window(this.axWebBrowser1.Handle,0,0,bm.Width,(heightsize%screenHeight));
								//bm.SetResolution(bm.Width,bm.Height-widthsize%screenWidth);
							}
							else
							{
								screenfrag = Image.FromHbitmap(bm.GetHbitmap());
							}

				
							// g2.DrawImage(screenfrag, brwLeft /*+ URLExtraLeft*/, brwTop /*+ URLExtraHeight*/);
							if(count>0)
							{
								
								g2.DrawImage(screenfrag,40,screenHeight*i/*(myPage-i)*/);
				
					
								for(int l=0;l<limit;l++)
								{

									this.SendClick_LEFTDOWN(p);
									this.GenerateKeyboardEvent("Down",p);
									Application.DoEvents();
					
								}
						
					
							}
							else
							{

								//
								g2.DrawImage(screenfrag,0,screenHeight*i);
								
								if(i==0)
								{
									for(int m=0;m<=limit;m++)
									{
										this.SendClick_LEFTDOWN(p);
										this.GenerateKeyboardEvent("Down",p);
										Application.DoEvents();
						
									}
								}
								else
								{
									for(int m=0;m<limit;m++)
									{
										this.SendClick_LEFTDOWN(p);
										this.GenerateKeyboardEvent("Down",p);
										Application.DoEvents();
						
									}
								}
								
					
							}
			
							
						}//For 
						for(int k=0;k<16;k++)
						{
							this.SendClick_LEFTDOWN(p);
							this.GenerateKeyboardEvent("Right",p);
							Application.DoEvents();
						}


						for (int i=0;i<myPage;i++)
							for(int l=0;l<8;l++)
							{
								this.SendClick_LEFTDOWN(p);
								this.GenerateKeyboardEvent("Up",p);
								Application.DoEvents();
					
							}
			
			

						count ++;
						myPageWidth ++;
			

					}//while
	
					
					
					#endregion
					

					#region restore Mosuse previous position 
					# region Press the right Down mouse button 				
					
					for(int k=0;k<16;k++)
					{
						this.SendClick_LEFTDOWN(p);
						this.GenerateKeyboardEvent("Left",p);
						Application.DoEvents();
					}

					#endregion

					p.X+=250;
					p.Y+=100;
					this.SendClick_LEFTUP(p);
					#endregion 							

					
					# region save Image in a buffer after calculations
					int finalWidth = (int)((widthsize /*+ URLExtraLeft*/) * myResolution);
					int finalHeight = (int)((heightsize/* + URLExtraHeight*/) * myResolution);
					Bitmap finalImage = new Bitmap(finalWidth, finalHeight, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
			
					Graphics gFinal = Graphics.FromImage((Image)finalImage);
					gFinal.DrawImage( bm2, 0, 0, finalWidth, finalHeight);
  
					
					
					
					eps.Param[0] = new EncoderParameter( System.Drawing.Imaging.Encoder.Quality, myQuality);
					finalImage.Save(buffer, ici, eps);
					#endregion 



					#region Clean Up and Dispose stuff .
					
					myDoc = null;
					g.Dispose();
					g2.Dispose();
					gFinal.Dispose();
					bm.Dispose();
					bm2.Dispose();
					finalImage.Dispose();
					
					Cursor.Current = Cursors.Default;

					#endregion 
				
					Application.DoEvents();
					return Image.FromStream(buffer);

					#endregion 
	
				}

				else
				{
					// zaeem region to convert the web file in the Jpg
					#region Jpg conversion 
                
					mshtml.IHTMLDocument2 myDoc = (mshtml.IHTMLDocument2)this.axWebBrowser1.Document;
					myDoc.body.setAttribute("scroll", "yes", 0);
 


					#region Width and height settings 

					//Get Browser Window Height
					int heightsize = (int)myDoc.body.getAttribute("scrollHeight", 0);
					int widthsize = (int)myDoc.body.getAttribute("scrollWidth", 0);
	 
					//Get Screen Height
					int screenHeight = (int)myDoc.body.getAttribute("clientHeight", 0);
					int screenWidth = (int)myDoc.body.getAttribute("clientWidth", 0);

					#endregion 

				//Get bitmap to hold screen fragment.
				Bitmap bm = new Bitmap(screenWidth, screenHeight, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);

				//Create a target bitmap to draw into.
				Bitmap bm2 = new Bitmap(widthsize, heightsize, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
				Graphics g2 = Graphics.FromImage(bm2);

				Graphics g = null;
				IntPtr hdc;
				Image screenfrag = null;
				int brwTop = 0;
				int brwLeft = 0;
				int myPage = 0;
				//IntPtr myIntptr = (IntPtr)m_browser.HWND;
				IntPtr myIntptr = (IntPtr)this.axWebBrowser1.Handle;
				//Get inner browser window.
				int hwndInt = myIntptr.ToInt32();
				IntPtr hwnd = myIntptr;
				hwnd = (System.IntPtr)Win32.USER32.GetWindow(hwnd, Win32.USER32.GW_CHILD); 
				StringBuilder sbc = new StringBuilder(256);
				//Get Browser "Document" Handle
				while (hwndInt != 0) 
				{ 
					hwndInt = hwnd.ToInt32();
					Win32.USER32.GetClassName(hwndInt, sbc, 256);

					if(sbc.ToString().IndexOf("Shell DocObject View", 0) > -1)
					{
						hwnd = Win32.USER32.FindWindowEx(hwnd, IntPtr.Zero, "Internet Explorer_Server", IntPtr.Zero);
						break;
					}                
					hwnd =(System.IntPtr) Win32.USER32.GetWindow(hwnd, Win32.USER32.GW_HWNDNEXT);

				} 
            
				//Get Screen Height (for bottom up screen drawing)
				while ((myPage * screenHeight) < heightsize)
				{
					myDoc.body.setAttribute("scrollTop", (screenHeight - 5) * myPage, 0);
					++myPage;
				}
				//Rollback the page count by one
				--myPage;
            
				int myPageWidth = 0;

				while ((myPageWidth * screenWidth) < widthsize)
				{
					myDoc.body.setAttribute("scrollLeft", (screenWidth - 5) * myPageWidth, 0);
					brwLeft = (int)myDoc.body.getAttribute("scrollLeft", 0);
					for (int i = myPage; i >= 0; --i)
					{
						//Shoot visible window
						g = Graphics.FromImage(bm);
						hdc = g.GetHdc();
						myDoc.body.setAttribute("scrollTop", (screenHeight - 5) * i, 0);
						brwTop = (int)myDoc.body.getAttribute("scrollTop", 0);
						Win32.USER32.PrintWindow(hwnd, hdc, 0);
						g.ReleaseHdc(hdc);
						g.Flush();
						screenfrag = Image.FromHbitmap(bm.GetHbitmap());
						g2.DrawImage(screenfrag, brwLeft /*+ URLExtraLeft*/, brwTop /*+ URLExtraHeight*/);
					}
					++myPageWidth;
				}




				annotationBrowser.Add_Annotation_JPG(ref g2);



				int finalWidth = (int)((widthsize /*+ URLExtraLeft*/) * myResolution);
				int finalHeight = (int)((heightsize /*+ URLExtraHeight*/) * myResolution);
				Bitmap finalImage = new Bitmap(finalWidth, finalHeight, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
				Graphics gFinal = Graphics.FromImage((Image)finalImage);
				gFinal.DrawImage( bm2, 0, 0, finalWidth, finalHeight);
  
				
				eps.Param[0] = new EncoderParameter( System.Drawing.Imaging.Encoder.Quality, myQuality);
				
				finalImage.Save(buffer, ici, eps);
				//finalImage.Save("c:\\abc.jpg", ici, eps);
				//return Image.FromStream(buffer);
				

				//Clean Up.
				myDoc = null;
				g.Dispose();
				g2.Dispose();
				gFinal.Dispose();
				bm.Dispose();
				bm2.Dispose();
				finalImage.Dispose();

				Cursor.Current = Cursors.Default;
				return Image.FromStream(buffer);



				# endregion 
				}
			}
			catch(Exception exp)
			{
				MessageBox.Show(exp.Message.ToString());
				return null;
			}

		}



		# region Explicit Mouse Events

		public  void SendClick_RIGHTDOWN(Point location)
		{
			Cursor.Position = location;
			Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, new System.IntPtr());
			//mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, new System.IntPtr());
			
		}

		public  void SendClick_RIGHTUP(Point location)
		{
			Cursor.Position = location;
			Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_RIGHTUP, 0, 0, 0, new System.IntPtr());
			
		}

		public  void SendClick_LEFTDOWN(Point location)
		{
			Cursor.Position = location;
			Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
			//mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, new System.IntPtr());
			
		}

		public  void SendClick_LEFTUP(Point location)
		{
			Cursor.Position = location;
			Win32.USER32.mouse_event(Win32.USER32.MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
			//mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, new System.IntPtr());
			
		}
		#endregion 


		# region Explicit Keyboard Events
		public void GenerateKeyboardEvent(string Key, Point p)
		{
			Cursor.Position=p;
		
			uint intReturn = 0;
			NativeWIN32.INPUT structInput;
			structInput = new NativeWIN32.INPUT();
			structInput.type = (uint)1;
			structInput.ki.wScan = 0;
			structInput.ki.time = 0;
			structInput.ki.dwFlags = 0;
			structInput.ki.dwExtraInfo = 0;

			if(Key.Equals("Down"))
			{
				structInput.ki.wVk = (ushort)NativeWIN32.VK.DOWN;
				intReturn = NativeWIN32.SendInput((uint)1, ref structInput, Marshal.SizeOf(structInput));
			}
			else if(Key.Equals("Up"))
			{
			
				structInput.ki.wVk = (ushort)NativeWIN32.VK.UP;
				intReturn = NativeWIN32.SendInput((uint)1, ref structInput, Marshal.SizeOf(structInput));
			}
			else if(Key.Equals("Right"))
			{
			
				structInput.ki.wVk = (ushort)NativeWIN32.VK.RIGHT;
				intReturn = NativeWIN32.SendInput((uint)1, ref structInput, Marshal.SizeOf(structInput));
			}
			
			else if(Key.Equals("Left"))
			{
			
				structInput.ki.wVk = (ushort)NativeWIN32.VK.LEFT;
				intReturn = NativeWIN32.SendInput((uint)1, ref structInput, Marshal.SizeOf(structInput));
			}
			Application.DoEvents();
			//SendMessage(this.axWebBrowser1.Handle, WM_KEYDOWN, 0, 10);
	
		}

		#endregion 



		/// <summary>
		/// Captures the window or part thereof to a bitmap image.
		/// </summary>
		public Bitmap Window(IntPtr wndHWND, int x, int y, int width, int height)
		{
			IntPtr  wndHDC = Win32.USER32.GetDC(wndHWND);
			// get context for window 

			//  create compatibile capture context and bitmap
			IntPtr  capHDC = Win32.USER32.CreateCompatibleDC(wndHDC);
			IntPtr  capBMP = Win32.USER32.CreateCompatibleBitmap(wndHDC, width, height);

			//  make sure bitmap non-zero
			if (capBMP == IntPtr.Zero)// if no compatible bitmap
			{
				Win32.USER32.ReleaseDC(wndHWND,wndHDC); //   release window context
				Win32.USER32.DeleteDC(capHDC); //   delete capture context
				//	return null; //   return null bitmap
			}

			//  select compatible bitmap in compatible context
			//  copy window context to compatible context
			//  select previous bitmap back into compatible context
			IntPtr  prvHDC = (IntPtr)Win32.USER32.SelectObject(capHDC,capBMP); 
			Win32.USER32.BitBlt(capHDC,0,0,width,height,wndHDC,x,y,Win32.USER32.SRCCOPY); 
			Win32.USER32.SelectObject(capHDC,prvHDC);

			//  create GDI+ bitmap for window
			Bitmap  bmp = System.Drawing.Image.FromHbitmap(capBMP); 

			//  release window and capture resources
			Win32.USER32.ReleaseDC(wndHWND,wndHDC); // release window context
			Win32.USER32.DeleteDC(capHDC); // delete capture context
			Win32.USER32.DeleteObject(capBMP); // delete capture bitmap

			
			//  return bitmap image to user
			return bmp;  // return bitmap

			
		}              // end method Window



		public void scrollpageDown(int i,Point p)
		{

			if(i!=0)
			{
				
				this.click(p);
			
				
			}

		}



		private void click(Point p)
		{
			this.GenerateKeyboardEvent("Down",p);
			
		}


		private static ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			int j;
			ImageCodecInfo[] encoders;
			encoders = ImageCodecInfo.GetImageEncoders();
			for(j = 0; j < encoders.Length; ++j)
			{
				if(encoders[j].MimeType == mimeType)
					return encoders[j];
			}
			return null;
		}







		#region TakeBrowseJPG(string path) 
		/// <summary>
		/// This function  takes the snapshot of the browser control and 
		/// save it in a given directory
		/// It covers both modes of webshare 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool TakeBrowseJPG(string path)
		{
			//this check box shows whether its the image mode or the othee one
			if(ClientUI.flag_ImageMode)
			{
				// if true, it means that its the image mode 
				# region JPG Save from picture box
				try
				{
					if(ClientUI.browserControl.whiteBoard._picBox.Image!=null)
					{
						ClientUI.browserControl.whiteBoard._picBox.Image.Save(path);
						return true;
					}
								
					else 
						return false;

				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing if(this.chk_Imagemode) is true in TakeBrowseJPG(string path)",exp,exp.Message+" Due to :"+exp.StackTrace.ToString(),true);				
					return false;
				}
				#endregion 
			}
			// zaeem region to convert the web file in the Jpg
			// else means that its not the image mode and webbrowser is converted into the image on run time
			else
			{
				#region Jpg conversion 
                
				try
				{
					mshtml.IHTMLDocument2 myDoc = (mshtml.IHTMLDocument2)this.axWebBrowser1.Document;
					//Set scrolling on.
					myDoc.body.setAttribute("scroll", "yes", 0);
 
					//Get Browser Window Height
					int heightsize = (int)myDoc.body.getAttribute("scrollHeight", 0);
					int widthsize = (int)myDoc.body.getAttribute("scrollWidth", 0);
 
					//Get Screen Height
					int screenHeight = (int)myDoc.body.getAttribute("clientHeight", 0);
					int screenWidth = (int)myDoc.body.getAttribute("clientWidth", 0);



					//Get bitmap to hold screen fragment.
					Bitmap bm = new Bitmap(screenWidth, screenHeight, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);

					//Create a target bitmap to draw into.
					Bitmap bm2 = new Bitmap(widthsize /*+ URLExtraLeft*/, heightsize /*+ URLExtraHeight - trimHeight */, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
					Graphics g2 = Graphics.FromImage(bm2);

					Graphics g = null;
					IntPtr hdc;
					Image screenfrag = null;
					int brwTop = 0;
					int brwLeft = 0;
					int myPage = 0;
					//IntPtr myIntptr = (IntPtr)m_browser.HWND;
					IntPtr myIntptr = (IntPtr)this.axWebBrowser1.Handle;
					//Get inner browser window.
					int hwndInt = myIntptr.ToInt32();
					IntPtr hwnd = myIntptr;
					hwnd =(IntPtr) Win32.USER32.GetWindow(hwnd, Win32.USER32.GW_CHILD); 
					StringBuilder sbc = new StringBuilder(256);
					//Get Browser "Document" Handle
					while (hwndInt != 0) 
					{ 
						hwndInt = hwnd.ToInt32();
						Win32.USER32.GetClassName(hwndInt, sbc, 256);

						if(sbc.ToString().IndexOf("Shell DocObject View", 0) > -1)
						{
							hwnd = Win32.USER32.FindWindowEx(hwnd, IntPtr.Zero, "Internet Explorer_Server", IntPtr.Zero);
							break;
						}                
						hwnd = (IntPtr)Win32.USER32.GetWindow(hwnd, Win32.USER32.GW_HWNDNEXT);

					} 
            
					//Get Screen Height (for bottom up screen drawing)
					while ((myPage * screenHeight) < heightsize)
					{
						myDoc.body.setAttribute("scrollTop", (screenHeight - 5) * myPage, 0);
						++myPage;
					}
					//Rollback the page count by one
					--myPage;
            
					int myPageWidth = 0;

					while ((myPageWidth * screenWidth) < widthsize)
					{
						myDoc.body.setAttribute("scrollLeft", (screenWidth - 5) * myPageWidth, 0);
						brwLeft = (int)myDoc.body.getAttribute("scrollLeft", 0);
						for (int i = myPage; i >= 0; --i)
						{
							//Shoot visible window
							g = Graphics.FromImage(bm);
							hdc = g.GetHdc();
							myDoc.body.setAttribute("scrollTop", (screenHeight - 5) * i, 0);
							brwTop = (int)myDoc.body.getAttribute("scrollTop", 0);
							Win32.USER32.PrintWindow(hwnd, hdc, 0);
							g.ReleaseHdc(hdc);
							g.Flush();
							screenfrag = Image.FromHbitmap(bm.GetHbitmap());
							g2.DrawImage(screenfrag, brwLeft /*+ URLExtraLeft*/, brwTop /*+ URLExtraHeight*/);
						}
						++myPageWidth;
					}



				{   //Backfill URL paint location
					SolidBrush whiteBrush = new SolidBrush(Color.White);
					Rectangle fillRect = new Rectangle(0, 0, widthsize,/* URLExtraHeight+*/2);
					Region fillRegion = new Region(fillRect);
					g2.FillRegion(whiteBrush, fillRegion);

					SolidBrush drawBrushURL = new SolidBrush(Color.Black);
					Font drawFont = new Font("Arial", 12);
					StringFormat drawFormat = new StringFormat();
					drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;

					g2.DrawString(/*myLocalLink*/ txtUrl.Text.ToString(), drawFont, drawBrushURL, 0, 0, drawFormat);
				}

					annotationBrowser.Add_Annotation_JPG(ref g2);



					//Reduce Resolution Size
					double myResolution = Convert.ToDouble(90) * 0.01;
					int finalWidth = (int)((widthsize /*+ URLExtraLeft*/) * myResolution);
					int finalHeight = (int)((heightsize /*+ URLExtraHeight*/) * myResolution);
					Bitmap finalImage = new Bitmap(finalWidth, finalHeight, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
					Graphics gFinal = Graphics.FromImage((Image)finalImage);
					gFinal.DrawImage( bm2, 0, 0, finalWidth, finalHeight);
  
					//Get Time Stamp
					DateTime myTime = DateTime.Now;
					String format = "MM.dd.hh.mm.ss";

			
					//Write Image.
					EncoderParameters eps = new EncoderParameters(1);
					long myQuality = Convert.ToInt64(70/*cmbQuality.Text*/);
					eps.Param[0] = new EncoderParameter( System.Drawing.Imaging.Encoder.Quality, myQuality);
					ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
					//finalImage.Save(@"c:\\IECapture\Captured_" + myTime.ToString(format) + ".jpg", ici, eps);
					finalImage.Save(path, ici, eps);



					//Clean Up.
					myDoc = null;
					g.Dispose();
					g2.Dispose();
					gFinal.Dispose();
					bm.Dispose();
					bm2.Dispose();
					finalImage.Dispose();

					Cursor.Current = Cursors.Default;
					return true;
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing if(this.chk_Imagemode) is false in TakeBrowseJPG(string path)",exp,exp.Message+" Due to :"+exp.StackTrace.ToString(),true);				
					return false;
				
				}

				# endregion 
			}

		}



		#endregion 


		#endregion 









		private void axWebBrowser1_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
		{
			BrowserDoc_count=0;
			
		}


		private void axWebBrowser1_DocComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			

			
			try
			
			{
				
				SHDocVw.IWebBrowser2 doc=e.pDisp as SHDocVw.IWebBrowser2;





			if (doc==(sender as AxSHDocVw.AxWebBrowser).GetOcx())
				{
					try
					{
						//MessageBox.Show("Document Completed");
						mshtml.IHTMLDocument2 HTMLDocument;
						HTMLDocument = (IHTMLDocument2) this.axWebBrowser1.Document;
						if(chk_AllowAnnotation.Checked==true)
						{
												
							mshtml.IHTMLElementCollection cc=HTMLDocument.all;
							foreach (mshtml.IHTMLElement anObj in cc) 
							{ 
								//							if(anObj.tagName=="A")						
								//								anObj.style.cursor="default";
								//							if(anObj.tagName=="BODY")						
								//								anObj.style.cursor="default";


								anObj.style.cursor="default";
								//						if (anObj.innerText != null) 
								//						{ 
								//							if (anObj.innerText != "") 
								//							{ 
								//								if ((emailReg.IsMatch(anObj.innerText))&
								//									(anObj.innerText!="")) aQueue.Enqueue(
								//															   anObj.innerText); 
								//							} 
								//						} 
							} 
							//cc.	
					

						}
						else
						{
							mshtml.IHTMLElementCollection cc=HTMLDocument.all;
							foreach (mshtml.IHTMLElement anObj in cc) 
							{ 
								//if(anObj.tagName=="A")						
								anObj.style.cursor="";
								//if(anObj.tagName=="BODY")						
								//	anObj.style.cursor="";
								//						if (anObj.innerText != null) 
								//						{ 
								//							if (anObj.innerText != "") 
								//							{ 
								//								if ((emailReg.IsMatch(anObj.innerText))&
								//									(anObj.innerText!="")) aQueue.Enqueue(
								//															   anObj.innerText); 
								//							} 
								//						} 
							} 
						}
						///MessageBox.Show(cc.length.ToString());
					}
					catch(Exception ex)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  axWebBrowser1_DocComplete() on 2098",ex,ex.Message,false);
						//axWebBrowser1_DocComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
					}
				}
				// Attach the event handler with the events interface.
				//			((HTMLButtonElementEvents2_Event)button).onclick += 
				//				new HTMLButtonElementEvents2_onclickEventHandler(this.Button_onclick);
				//			if(NetworkManager.getInstance().clientType == ClientType.ClientAttendee)
				//			{
				//				chk_AllowAnnotation.Enabled = false;
				//			}
				//			ClearIt();
				//			/*if(!annotationsEnabled)
				//			{
				//				this.chk_ShowAnnotation.Checked = true;
				//
				//			}*/
				//			if(chk_Autoshare.Checked)
				//			{
				//				btnShare_Click(btnShare,null);
				//			}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  axWebBrowser1_DocComplete() 2122",ex,"",false);
			}



			++BrowserDoc_count;
			chk_Autoshare.Enabled=true;
			chk_ShowAnnotation.Enabled=true;
			chk_AllowAnnotation.Enabled=true;
			chk_Imagemode.Enabled=true;




		}	
	







	




		private bool Button_onclick(IHTMLEventObj e)
		{
			try
			{

				MessageBox.Show("Alert from the app: Received theButton.onclick!");
				return false;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private bool Button_onclick(IHTMLEventObj e)",ex,"",false);
				return true;
			}
		}
		private void axWebBrowser1_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
		{
			try
			{
			

				if(NetworkManager.getInstance().clientType == ClientType.ClientAttendee)
				{
					chk_AllowAnnotation.Enabled = false;
				}
				ClearIt();
				/*if(!annotationsEnabled)
				{
					this.chk_ShowAnnotation.Checked = true;

				}*/
				if(chk_Autoshare.Checked)
				{
					btnShare_Click(btnShare,null);
				}
				else
				{
					this.txtUrl.Text=this.axWebBrowser1.LocationURL.ToString();
				}
				
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void axWebBrowser1_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)",ex,"",false);
			}
		}

		private void txtUrl_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			
		}

		private void txtUrl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if(e.KeyCode==Keys.Enter)
				{
					btnGo_Click(btnGo,new EventArgs());
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void txtUrl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)",ex,"",false);
			}
		}
		private void EnableAnnotation()
		{
			try
			{


				if(!annotationsEnabled)
				{
					System.IntPtr pointer = annotationBrowser.IEFromhWnd(axWebBrowser1.Handle);
					if(pointer == System.IntPtr.Zero)
					{
						//	Client.ClientUI.getInstance().ShowExceptionMessage("Unable to initialize annotations. Annotations may not work properly");						
						Thread _timer = new Thread(new ThreadStart(RecallCheckBOx));
						_timer.Name = "EnableAnnotation Thread: RecallCheckBOx()";
						_timer.Start();
						return;

					}
					else
					{
						//					if(NetworkManager.getInstance().clientType == ClientType.ClientAttendee)
						//						annotationBrowser.tool=WhiteboardToolCode.None;
						annotationBrowser.StartSubclass(pointer,axWebBrowser1);
					}

					annotationBrowser.DrawNow = true;
					annotationBrowser.addAnnotations = true;
					annotationBrowser.annotationType = AnnotationMessageType.WEB;
					annotationBrowser.thisSessionID = sessionID;

					//MessageBox.Show("Annotations Enabled","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
					annotationsEnabled = true;
					chk_AllowAnnotation.Checked=true;
				}				
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  private void EnableAnnotation()",ex,"",false);
			}

		}

		private void btnLog_Click(object sender, System.EventArgs e)
		{
			ClientUI.getInstance().MM_Controller.Send_MinutsofMeetingMsg_WebBrowser();
		}







		static ImageConverter conv = new ImageConverter();
		public void CompressandSendImage(Image inc_img)
		{
			
			
			Common.MsgIncImage_Generic msg=new MsgIncImage_Generic();
				
			
			
			
			
			try
			{
				if(inc_img==null)
				{
					msg.compressedbuffer=null;
				}
				else
				{
					OctreeQuantizer quantizer =new OctreeQuantizer (255,8);
					Image _quantized;
					_quantized = quantizer.Quantize(inc_img);
					byte[] b;
				
		                                
					// convert quantized array to byte array
					b=null;
					b=(byte[]) conv.ConvertTo(_quantized, typeof(byte[]));



					//First of all, we'll start with compression. Since we're using MemoryStreams, let's create a new one:
					MemoryStream msCompressed = new MemoryStream();

					//Next, we create a BZip2 output stream, passing in our MemoryStream.
					BZip2OutputStream zosCompressed = new BZip2OutputStream(msCompressed);

					//From here, we start writing data to the BZip2OutputStream:
					byte[] bytesBuffer = b;
					zosCompressed.Write(bytesBuffer, 0, bytesBuffer.Length);
					//zosCompressed.Finalize();
					zosCompressed.Close();


					//So we encode our output as a byte array, then write it to the compression stream, 
					//which in turn compresses the data and writes it to the inner stream, which is our MemoryStream.


					bytesBuffer = msCompressed.ToArray();

					msg.compressedbuffer=bytesBuffer;
					msg.SenderID=NetworkManager.thisInstance.profile.ClientId;
					msg.moduleType=Common.enum_Module.Webshare;



					
				
				}
				NetworkManager.thisInstance.SendLoadPacket(msg);

				
			}
			catch(Exception ex)
			
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  compressandsendImge Method in broser.cs",ex,"",true);
			}		
		
		}



		public Image UncompresstoImage(byte[] bytesBuffer)
		{
			try
			{
				//Uncompression
				//Now of course, to test our code above, we need some uncompression code. I will put all the code together, since it's pretty much the same, just using a BZip2InputStream instead of a BZip2OutputStream, and Read instead of Write:
				MemoryStream msUncompressed =   new MemoryStream(bytesBuffer);
				
				BZip2InputStream zisUncompressed = new BZip2InputStream(msUncompressed);
				bytesBuffer = new byte[zisUncompressed.Length];
				zisUncompressed.Read(bytesBuffer, 0, bytesBuffer.Length);
				zisUncompressed.Close();
				msUncompressed.Close();
				
				MemoryStream ms = new MemoryStream(bytesBuffer,0,bytesBuffer.Length);
				Image img=Image.FromStream(ms);
				return img;
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  UncompresstoImage Method in broser.cs",exp,"",true);
			return null; 
			
			}
			
		}


		private void chk_Imagemode_CheckedChanged(object sender, System.EventArgs e)
		{
			

			try

			{

				if(ClientUI.getInstance().IfIamthePresenter())
				{
					if((chk_Imagemode.Checked))
					{
						ClientUI.flag_ImageMode=true;
						// So that it doesn't annotate in the image made
						ClientUI.getInstance().whiteBoard.tool=WebMeetingDrawingBoard.WhiteboardToolCode.None;
						
						// So that scrolling can be done
						if(ClientUI.browserControl.chk_AllowAnnotation.Checked)
						{
						ClientUI.browserControl.chk_AllowAnnotation.Checked=false;
						}

						if(!this.isSharedBrowser==true)
						{
							this.btnShare_Click(null,null);
						}

					
						//Set the image 
						// This count check whether the current page has multiple docs 
						// If yes then it will follow the Key board scroll stretegy
						if(BrowserDoc_count>1)
						{
							if(!this.splitter1.IsCollapsed)
								this.splitter1.ToggleState();
							
							if(!ClientUI.getInstance().splitter1.IsCollapsed)
							ClientUI.getInstance().splitter1.ToggleState();
						    whiteBoard._picBox.Image=(Image)this.TakeBrowseJPG_Image(true,null);
						}
						else
						{
							whiteBoard._picBox.Image=(Image)this.TakeBrowseJPG_Image(false,null);
						}

						whiteBoard.Dock = DockStyle.Fill;
						whiteBoard.Visible=true;
						whiteBoard._picBox.BackColor=Color.White;
						whiteBoard._picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
				
						//				Bitmap mainImage=new Bitmap(this.Global_width,this.Global_height);											
						//				whiteBoard._picBox.Image=(Image)mainImage;

						// After setting the Image , You have to set the Graphics object for that image
						whiteBoard.setGraphicsobject();



						whiteBoard.BringToFront();
						whiteBoard.setModule(3);
						//this.axWebBrowser1.Visible=false;


						// To send the image across the network;
						CompressandSendImage(whiteBoard._picBox.Image);

						if(whiteBoard.MsgPool_Annotation.Count >0 || whiteBoard.MsgPool_SendAnnotation.Count >0)
						{
							//to clear previous arrays 
							ClearPreviousMessages();
						}
					
					
					}
					else
					{
						// To send the image across the network;
						ClientUI.flag_ImageMode=false;
						CompressandSendImage(null);
						whiteBoard.Visible = false;
						this.BringToFront();
						

				
					}
				}

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  on 1845 line chk_Imagemode_CheckedChanged() in broser.cs",exp,"",true);
			
			
			}


		}


		public void ClearPreviousMessages()
		{
			
			ClientUI.Flag_MsgPool_Annotation=true;
			ClientUI.Flag_MsgPool_SendAnnotation=true;

		}

		public void CleanandClear_Threadcall()
		{
			
			try
			{
				//A Thread object should be terminated when the application exits. 
				//To do this, you use the Abort method. When this method is invoked,
				//the system throws a System.Threading.ThreadAbortException in the thread to abort it.
				//Calling the Abort method does not guarantee that the thread aborts immediately or at all.
				//If a thread does an unbounded amount of computation in the final block called as part of the abort procedure,
				//for example, the thread will be delayed from being aborted indefinitely. 
				//To make sure a thread has aborted, call the Join method after calling Abort.
		
				//Clear Previous Messages
				//ClearPreviousMessages();
				// Give it the time to clear previous messages
				//Thread.Sleep(2000);
				//ClientUI.flag_MouseMove_whiteboard=false;
				//Thread.Sleep(2000);
				//this.whiteBoard.m_bIsActive=false;
				try
				{
				
					if(this.whiteBoard._picBox!=null)
					{
						this.whiteBoard.Enabled=false;
						this.whiteBoard._picBox.Dispose();
					}
				}
				catch(Exception exp)
				{
					exp=exp;
				}

				if (consumeThread.ThreadState != System.Threading.ThreadState.Unstarted)
				{
					consumeThread.Abort();
					consumeThread.Join(5000);
				}

				
				if (sendingThread.ThreadState != System.Threading.ThreadState.Unstarted)
				{
					sendingThread.Abort();
					sendingThread.Join(5000);
				}


				ClientUI.flag_ImageMode=false;
				NetworkManager.thisInstance.flag_IsWebImageAlreadyActive=0;
				
				try
				{
					this.whiteBoard.Dispose();
					this.Dispose();
				}
				catch(Exception ex)
				{
					ex=ex;
				}


			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing  on 1885 line CleanandClear() in broser.cs",exp,"",true);
						
			}
			finally
			{
				//ClientUI.flag_MouseMove_whiteboard=true;
			}

		}

		/// <summary>
		/// Made By Zaeem to clean the threads, Initialse few vaiables and the other resources taken by the Image based Webbrowser conrtol.
		/// 
		/// </summary>
		public void CleanandClear()
		{
		
		new Thread(new ThreadStart (CleanandClear_Threadcall)).Start();

		}

		private void chk_Autoshare_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chk_Autoshare.Checked)
			{
			this.btnShare_Click(null, null);			
			}
		}




	
	}
}


