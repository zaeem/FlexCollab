using System;
using System.Net;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using WebMeeting.Common;
using System.Diagnostics;
using WebMeeting.FTP;
using WebMeeting.Client;
using System.IO;
using System.Threading;
using Utility.NiceMenu;
using System.Runtime.InteropServices;
using WebBrowserSubClassing;
using mshtml;
using SHDocVw;
using WebMeeting.Client.Alerts;
//using System.Net;



using System.Xml;


namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for documentSharingControl.
	/// </summary>
	public class documentSharingControl : System.Windows.Forms.UserControl
	{
		#region Variables 
		#region Local Used Variables
		string saveFileName;
		string DocumentFilePath;
		string DocumentFilePath_temp;
		
		bool annotationsEnabled;
		bool m_DocumentDownloaded =false;
		string strLocalPath;
		DocumentMessageType documentType;
		public DocumentSharing m_DocumentMessage;
		
	//	bool IsRecieveThreadCompleted;
		int pictureCount ;
		int currentImage =-1;			
		public ArrayList MessagePool;
		public bool m_bTerminated;
		public Crownwood.Magic.Controls.TabPage parentTabPage;
		public int sessionID;
		private System.Windows.Forms.Panel panel1;
		public string documentTitle;
		public bool IsAttendee = false;
		public bool IsClosed = false;
		public int ClientId = -1;
		public Hashtable ParticipentsArrayList = Hashtable.Synchronized(new Hashtable());
		string filenamed;        

		public Thread RecieveThread;
		public Thread ScrollingThread;
		public Thread uploadThread ;
		private ArrayList scrollMessagesArray = new ArrayList();

		public MessageObject intialMsg; //this member is used to update UI according to the intial message recieved from server.

		private System.Windows.Forms.Label lblStatus;
		
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public ArrayList PresentationNailArray=new ArrayList();
		

		#endregion
	
		#region form variables
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private NSPAControls.NSButton nsButton2;
		private NSPAControls.NSButton buttonDown;
		private NSPAControls.NSButton nsButton1;

		private ArrayList presentationAnnotationList;
		public System.Windows.Forms.CheckBox chkThumbNail;
		private System.Windows.Forms.Panel panel4;
		public Utility.NiceMenu.NiceMenu myMenu;
		public System.Windows.Forms.CheckBox chkShowThumbNails;
		private NSPAControls.NSButton btnPrev;
		private NSPAControls.NSButton btnNext;
		private NSPAControls.NSButton btnLast;
		private NSPAControls.NSButton btnFirst;
		public AxSHDocVw.AxWebBrowser axWebBrowser1;
		private NSPAControls.NSButton btnOptions;
		public WebBrowserSubClassing.IEHwd	annotationBrowser = new IEHwd();
		#endregion
		#endregion
		public bool m_bIsAllowedToAddAnnotations =false;
		public System.Windows.Forms.CheckBox chkSynch;
		public bool m_bOwnerofthisControl = false;
		public bool m_bVisited= false;
		public Point lastScrolPos = new Point(0,0);
		public bool  IsLaunchFromMangeContent=false;
		public int no_of_pages=1;
		public string strRemoteUrl="";
		public string strExtension="";
		public System.Windows.Forms.Panel panelAnnotations;
		private System.Windows.Forms.PictureBox annotateBox;
		public bool chkThumbnail;
		public FileInfo theFile;
		private bool IntializationCompleted = false;
        Thread createThumbnailThread ;
		Thread sendingThread ;
		private ArrayList scrollMessages = new ArrayList();
		private delegate void WindowCreationDelegate(System.IntPtr Handle);
		private WindowCreationDelegate CreatePictureBox;
		PictureBox annotation = null;
		int _mouseLastX,_mouseLastY;

		public bool ShowClose()
		{
			return true;
		}


		# region Terminate()
		public void Terminate()
		{			
			this.m_bTerminated=true;

			try
			{
				this.annotationBrowser.abortThreads();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs line==> 130",exp,null,false);			
			
			}
			
			try
			{
				System.IntPtr pointer = annotationBrowser.IEFromhWnd(axWebBrowser1.Handle);
				annotationBrowser.StopSubclass(pointer);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs line==> 141",exp,null,false);			
			
			}
			

		}
		# endregion 

		# region documentSharingControl(bool bIsAttendee)
		public documentSharingControl(bool bIsAttendee)
		{
			// This call is required by the Windows.Forms Form Designer.
			try
			{

				InitializeComponent();
				MessagePool=new ArrayList();
				pictureCount=0;
				currentImage=0;

				sessionID=-1;
				m_bTerminated=false;
				//			// get the sessionID here;
				IsAttendee = bIsAttendee;
				if(!bIsAttendee)	
					NetworkManager.getInstance().GetUniqueSessionID(ref sessionID);

				presentationAnnotationList=new ArrayList();		
				CreatePictureBox = new WindowCreationDelegate(AddPictureToThumbNail);
				annotationBrowser.OnScroll+=new WebBrowserSubClassing.IEHwd.OnVerticalScrollDelegate(annotationBrowser_OnScroll);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>documentSharingControl( line==> 181",exp,null,false);			
					
			}
			

			//	panelAnnotations.Visible = false;
		}
		# endregion 

		#region form gui 
		public void HostFormPresentation()
		{
			this.btnNext.Visible=true;
			this.btnNext.Enabled=true;

			this.btnPrev.Visible=true;
			this.btnPrev.Enabled=true;
			

			this.btnFirst.Enabled=true;
			this.btnLast.Enabled=true;

			this.btnFirst.Visible=true;
			this.btnLast.Visible=true;

			this.chkThumbNail.Visible=true;
			this.chkShowThumbNails.Visible=true;

		}
		public void AttendeeFormPresentation()
		{
			this.btnNext.Visible=true;
			this.btnNext.Enabled=false;

			this.btnPrev.Visible=true;
			this.btnPrev.Enabled=false;		

			this.btnFirst.Enabled=false;
			this.btnLast.Enabled=false;
			
			this.btnFirst.Visible=true;
			this.btnLast.Visible=true;

			this.chkThumbNail.Visible=false;
			this.chkShowThumbNails.Visible=true;

		}
		public void HostFormDocument()
		{
			
			this.btnNext.Visible=false;
			this.btnPrev.Visible=false;
						
			this.chkThumbNail.Visible=false;

			this.btnFirst.Visible=false;
			this.btnLast.Visible=false;

			this.chkShowThumbNails.Visible=false;
			
			//this.textBox.Top=5;
			//this.textBox.Left=5;
			//this.textBox.Width=this.Width - 30;
			//this.textBox.Height=this.Height - 85;
		}
		
		public void AttendeeFormDocument()
		{	
			this.btnNext.Visible=false;
			this.btnPrev.Visible=false;
			
			this.chkThumbNail.Visible=false;			

			this.btnFirst.Visible=false;
			this.btnLast.Visible=false;
			this.chkShowThumbNails.Visible=false;

		}

		#endregion


		public int currentPageNo()
		{
			return currentImage;
		}


		# region GetImage(string urlFile)
		public Image GetImage(string urlFile)
		{
			try
			{
				WebRequest wreq = WebRequest.Create( urlFile );
				HttpWebResponse httpResponse = (HttpWebResponse) wreq.GetResponse();
				Stream stream = httpResponse.GetResponseStream();
				return Image.FromStream(stream);
			}
		
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>GetImage( line==> 278",exp,null,false);			
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : GetImage() function ," + e.Source + " Source ," + e.Message,true,false);
				//alert.ShowMessage(4,this.Name + " Class : GetImage() function ," + e.Source + " Source ," + e.Message  );
				return (new Bitmap(1,1));
			}
			
		}
		# endregion 


		private void mNewMenuDocuments_Click(object sender, EventArgs e)
		{
			this.Save();			
		}
		private void mNewMenuDocumentsSaveAs_Click(object sender, EventArgs e)
		{
			this.SaveAs();			
		}
	
		
		# region Save()
		public void Save()
		{
			if((this.saveFileName=="") || (saveFileName == null))
			{
				SaveAs();
				return;
			}
			try
			{			
				if(documentType == DocumentMessageType.typePresentation)
				{
					
				//	string str =  Directory.GetParent(this.DocumentFilePath ).FullName;
				//	str = str + "\\slide" + this.currentImage.ToString() + ".jpg";			
					
				//	File.Copy(str,this.saveFileName,true);
				}
				
				{
					IWebBrowser2 wb2 = (IWebBrowser2) axWebBrowser1.GetOcx();
					De.Fun.IESaveAsHack.SaveAsWebbrowser aSave = new De.Fun.IESaveAsHack.SaveAsWebbrowser(wb2,saveFileName,De.Fun.IESaveAsHack.SaveType.SAVETYPE_HTMLPAGE);
					aSave.SaveAs();
					//                 	SavePage();
				}
			
				return ;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>Save( line==> 332",exp,null,false);			
				//MeetingAlerts alert=new MeetingAlerts();
				///alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : Save() function ," + ee.Source + " Source ," + ee.Message + "\nCouldnt save to " + saveFileName,true,false);
				//alert.ShowMessage(4,this.Name + " Class : Save() function ," + ee.Source + " Source ," + ee.Message  );
				//MessageBox.Show("Couldnt save to " + saveFileName,"WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}

		# endregion 

		# region SaveAs
		public void SaveAs()
		{
			try
			{
				SaveFileDialog fd=new SaveFileDialog();
				//if(this.m_DocumentMessage.DocumentType != DocumentMessageType.typePresentation)
					fd.Filter = "Web Page(*.htm)|*.html";			
				//else
				//	fd.Filter = "Image Files(*.jpg)|*.jpg";			



				DialogResult res=fd.ShowDialog();
				if(res==DialogResult.Cancel)
					return;
				
				saveFileName=fd.FileName;
				Save();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>SaveAs( line==> 362",exp,null,false);			
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : SaveAs() function ," + ex.Source + " Source ," + ex.Message,true,false);
				//MessageBox.Show(this.Name + " Class : SaveAs() function ," + ex.Source + " Source ," + ex.Message  );
			}
		
		}
		# endregion 

		# region 
		public void hideAnnotations()
		{
			try
			{
				if(documentType != DocumentMessageType.typePresentation)
				{
					buttonDown.Visible = false;
					nsButton1.Visible = false;
					panelAnnotations.Visible = false;
				}

				if(Client.NetworkManager.getInstance().clientType == ClientType.ClientAttendee)
					chkThumbNail.Enabled = false;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>SaveAs( line==> 362",exp,null,false);			
			
			   //MeetingAlerts alert=new MeetingAlerts();
			  // alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : hideAnnotations() function ," + ex.Source + " Source ," + ex.Message,true,false);
			   //MessageBox.Show(this.Name + " Class : hideAnnotations() function ," + ex.Source + " Source ," + ex.Message  );
			}
		}
		# endregion 


		# region AddSaveMenus()
		public void AddSaveMenus()
		{
			try
			{
				Utility.NiceMenu.NiceMenu parentMenu = Client.ClientUI.getInstance().myNiceMenu.SearchNiceMenuItem("Document");
				if(parentMenu != null)
				{
					myMenu = new Utility.NiceMenu.NiceMenu();				
					myMenu.Click += new EventHandler(mNewMenuDocuments_Click);							 
					myMenu.Text = documentTitle;				
					parentMenu.MenuItems.Add(myMenu);
				}
			
				//add menu to save as
				parentMenu = Client.ClientUI.getInstance().myNiceMenu.SearchNiceMenuItem("Document",2);
				if(parentMenu != null)
				{
					myMenu = new Utility.NiceMenu.NiceMenu();				
					myMenu.Click += new EventHandler(mNewMenuDocumentsSaveAs_Click);							 
					myMenu.Text = documentTitle ;				
			
					parentMenu.MenuItems.Add(myMenu);
				}

				try
				{
					//add menu to Print
					parentMenu = Client.ClientUI.getInstance().myNiceMenu.SearchNiceMenuItem("Document",3);
					if(parentMenu != null)
					{
						myMenu = new Utility.NiceMenu.NiceMenu();				
						myMenu.Click += new EventHandler(this.printMenu_Click);
						myMenu.Text = documentTitle ;				
			
						parentMenu.MenuItems.Add(myMenu);
					}
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>AddSaveMenus() line==> 437",exp,null,false);			
					//MeetingAlerts alert=new MeetingAlerts();
					//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : AddSaveMenus() function ," + ee.Source + " Source ," + ee.Message ,true,false);
					//MessageBox.Show(this.Name + " Class : AddSaveMenus() function ," + ee.Source + " Source ," + ee.Message  );
				}

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>AddSaveMenus() line==> 446",exp,null,false);			
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : AddSaveMenus() function ," + ee.Source + " Source ," + ee.Message ,true,false);
				////MessageBox.Show(this.Name + " Class : AddSaveMenus() function ," + ee.Source + " Source ," + ee.Message  );
			}
		}
		# endregion 

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(documentSharingControl));
			this.lblStatus = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnOptions = new NSPAControls.NSButton();
			this.btnFirst = new NSPAControls.NSButton();
			this.btnLast = new NSPAControls.NSButton();
			this.btnPrev = new NSPAControls.NSButton();
			this.chkShowThumbNails = new System.Windows.Forms.CheckBox();
			this.chkThumbNail = new System.Windows.Forms.CheckBox();
			this.btnNext = new NSPAControls.NSButton();
			this.chkSynch = new System.Windows.Forms.CheckBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.nsButton1 = new NSPAControls.NSButton();
			this.buttonDown = new NSPAControls.NSButton();
			this.nsButton2 = new NSPAControls.NSButton();
			this.panel4 = new System.Windows.Forms.Panel();
			this.panelAnnotations = new System.Windows.Forms.Panel();
			this.annotateBox = new System.Windows.Forms.PictureBox();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.panel2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panelAnnotations.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblStatus
			// 
			this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblStatus.Location = new System.Drawing.Point(0, 476);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(708, 24);
			this.lblStatus.TabIndex = 3;
			this.lblStatus.Text = "1 of 1";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.axWebBrowser1);
			this.panel1.Controls.Add(this.nsButton1);
			this.panel1.Controls.Add(this.buttonDown);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(708, 476);
			this.panel1.TabIndex = 5;
			// 
			// axWebBrowser1
			// 
			this.axWebBrowser1.ContainingControl = this;
			this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser1.Enabled = true;
			this.axWebBrowser1.Location = new System.Drawing.Point(0, 0);
			this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
			this.axWebBrowser1.Size = new System.Drawing.Size(708, 476);
			this.axWebBrowser1.TabIndex = 8;
			this.axWebBrowser1.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(this.axWebBrowser1_NavigateComplete2);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnOptions);
			this.panel2.Controls.Add(this.btnFirst);
			this.panel2.Controls.Add(this.btnLast);
			this.panel2.Controls.Add(this.btnPrev);
			this.panel2.Controls.Add(this.chkShowThumbNails);
			this.panel2.Controls.Add(this.chkThumbNail);
			this.panel2.Controls.Add(this.btnNext);
			this.panel2.Controls.Add(this.chkSynch);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 500);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(752, 20);
			this.panel2.TabIndex = 6;
			this.panel2.Click += new System.EventHandler(this.panel2_Click);
			// 
			// btnOptions
			// 
			this.btnOptions.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnOptions.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnOptions.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnOptions.HottrackImage")));
			this.btnOptions.Location = new System.Drawing.Point(340, -9);
			this.btnOptions.Name = "btnOptions";
			this.btnOptions.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnOptions.NormalImage")));
			this.btnOptions.OnlyShowBitmap = true;
			this.btnOptions.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnOptions.PressedImage")));
			this.btnOptions.Size = new System.Drawing.Size(60, 25);
			this.btnOptions.Text = "Options";
			this.btnOptions.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnOptions.ToolTip = null;
			this.btnOptions.Visible = false;
			this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
			// 
			// btnFirst
			// 
			this.btnFirst.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnFirst.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnFirst.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnFirst.HottrackImage")));
			this.btnFirst.Location = new System.Drawing.Point(61, 30);
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnFirst.NormalImage")));
			this.btnFirst.OnlyShowBitmap = true;
			this.btnFirst.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnFirst.PressedImage")));
			this.btnFirst.Size = new System.Drawing.Size(23, 1);
			this.btnFirst.Text = "nsButton3";
			this.btnFirst.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnFirst.ToolTip = null;
			this.btnFirst.Visible = false;
			this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
			// 
			// btnLast
			// 
			this.btnLast.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnLast.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnLast.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnLast.HottrackImage")));
			this.btnLast.Location = new System.Drawing.Point(106, 31);
			this.btnLast.Name = "btnLast";
			this.btnLast.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnLast.NormalImage")));
			this.btnLast.OnlyShowBitmap = true;
			this.btnLast.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnLast.PressedImage")));
			this.btnLast.Size = new System.Drawing.Size(23, 2);
			this.btnLast.Text = "nsButton3";
			this.btnLast.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnLast.ToolTip = null;
			this.btnLast.Visible = false;
			this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
			// 
			// btnPrev
			// 
			this.btnPrev.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnPrev.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnPrev.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnPrev.HottrackImage")));
			this.btnPrev.Location = new System.Drawing.Point(27, 32);
			this.btnPrev.Name = "btnPrev";
			this.btnPrev.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnPrev.NormalImage")));
			this.btnPrev.OnlyShowBitmap = true;
			this.btnPrev.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnPrev.PressedImage")));
			this.btnPrev.Size = new System.Drawing.Size(23, 1);
			this.btnPrev.Text = "nsButton3";
			this.btnPrev.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnPrev.ToolTip = null;
			this.btnPrev.Visible = false;
			this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
			// 
			// chkShowThumbNails
			// 
			this.chkShowThumbNails.Location = new System.Drawing.Point(129, 38);
			this.chkShowThumbNails.Name = "chkShowThumbNails";
			this.chkShowThumbNails.Size = new System.Drawing.Size(112, 16);
			this.chkShowThumbNails.TabIndex = 8;
			this.chkShowThumbNails.Text = "Show Annotations";
			this.chkShowThumbNails.Visible = false;
			this.chkShowThumbNails.CheckedChanged += new System.EventHandler(this.chkShowThumbNails_CheckedChanged);
			// 
			// chkThumbNail
			// 
			this.chkThumbNail.Enabled = false;
			this.chkThumbNail.Location = new System.Drawing.Point(13, 38);
			this.chkThumbNail.Name = "chkThumbNail";
			this.chkThumbNail.Size = new System.Drawing.Size(104, 16);
			this.chkThumbNail.TabIndex = 7;
			this.chkThumbNail.Text = "Add Annotation";
			this.chkThumbNail.Visible = false;
			this.chkThumbNail.CheckedChanged += new System.EventHandler(this.chkThumbNail_CheckedChanged);
			// 
			// btnNext
			// 
			this.btnNext.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnNext.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnNext.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnNext.HottrackImage")));
			this.btnNext.Location = new System.Drawing.Point(255, 31);
			this.btnNext.Name = "btnNext";
			this.btnNext.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnNext.NormalImage")));
			this.btnNext.OnlyShowBitmap = true;
			this.btnNext.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnNext.PressedImage")));
			this.btnNext.Size = new System.Drawing.Size(23, 1);
			this.btnNext.Text = "nsButton3";
			this.btnNext.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnNext.ToolTip = null;
			this.btnNext.Visible = false;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// chkSynch
			// 
			this.chkSynch.Location = new System.Drawing.Point(259, 38);
			this.chkSynch.Name = "chkSynch";
			this.chkSynch.Size = new System.Drawing.Size(86, 16);
			this.chkSynch.TabIndex = 8;
			this.chkSynch.Text = "Synchornize";
			this.chkSynch.Visible = false;
			// 
			// panel3
			// 
			this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel3.Location = new System.Drawing.Point(732, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(20, 500);
			this.panel3.TabIndex = 7;
			this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
			// 
			// nsButton1
			// 
			this.nsButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.nsButton1.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.nsButton1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.nsButton1.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.nsButton1.HottrackImage = null;
			this.nsButton1.Location = new System.Drawing.Point(600, 16);
			this.nsButton1.Name = "nsButton1";
			this.nsButton1.NormalImage = ((System.Drawing.Image)(resources.GetObject("nsButton1.NormalImage")));
			this.nsButton1.OnlyShowBitmap = true;
			this.nsButton1.PressedImage = null;
			this.nsButton1.Size = new System.Drawing.Size(18, 21);
			this.nsButton1.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.nsButton1.ToolTip = null;
			this.nsButton1.Visible = false;
			this.nsButton1.Click += new System.EventHandler(this.buttonDown_Click);
			// 
			// buttonDown
			// 
			this.buttonDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonDown.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.buttonDown.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonDown.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.buttonDown.HottrackImage = null;
			this.buttonDown.Location = new System.Drawing.Point(640, 56);
			this.buttonDown.Name = "buttonDown";
			this.buttonDown.NormalImage = ((System.Drawing.Image)(resources.GetObject("buttonDown.NormalImage")));
			this.buttonDown.OnlyShowBitmap = true;
			this.buttonDown.PressedImage = null;
			this.buttonDown.Size = new System.Drawing.Size(18, 21);
			this.buttonDown.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.buttonDown.ToolTip = null;
			this.buttonDown.Visible = false;
			this.buttonDown.Click += new System.EventHandler(this.nsButton2_Click);
			// 
			// nsButton2
			// 
			this.nsButton2.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.nsButton2.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.nsButton2.HottrackImage = null;
			this.nsButton2.Location = new System.Drawing.Point(0, 0);
			this.nsButton2.Name = "nsButton2";
			this.nsButton2.NormalImage = null;
			this.nsButton2.OnlyShowBitmap = false;
			this.nsButton2.PressedImage = null;
			this.nsButton2.Size = new System.Drawing.Size(23, 23);
			this.nsButton2.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.nsButton2.ToolTip = null;
			this.nsButton2.Visible = false;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.panel1);
			this.panel4.Controls.Add(this.lblStatus);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(24, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(708, 500);
			this.panel4.TabIndex = 9;
			// 
			// panelAnnotations
			// 
			this.panelAnnotations.AutoScroll = true;
			this.panelAnnotations.Controls.Add(this.annotateBox);
			this.panelAnnotations.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelAnnotations.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.panelAnnotations.Location = new System.Drawing.Point(0, 0);
			this.panelAnnotations.Name = "panelAnnotations";
			this.panelAnnotations.Size = new System.Drawing.Size(24, 500);
			this.panelAnnotations.TabIndex = 8;
			// 
			// annotateBox
			// 
			this.annotateBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.annotateBox.Location = new System.Drawing.Point(10, 16);
			this.annotateBox.Name = "annotateBox";
			this.annotateBox.Size = new System.Drawing.Size(60, 50);
			this.annotateBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.annotateBox.TabIndex = 8;
			this.annotateBox.TabStop = false;
			this.annotateBox.Click += new System.EventHandler(this.annotateBox_Click);
			// 
			// documentSharingControl
			// 
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.panelAnnotations);
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel2);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "documentSharingControl";
			this.Size = new System.Drawing.Size(752, 520);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panelAnnotations.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		#region button handlers
		public void btnPrev_Click(object sender, System.EventArgs e)
		{
			if(this.m_DocumentMessage.CurrentPage < 2)
				return;
			ForwardToPage(this.m_DocumentMessage.CurrentPage- 1);
			//currentImage-=1;
		}		

		private void synchornizePPTAnnotations()
		{
			
			int nIndex = currentImage ;
			nIndex--;
			if(nIndex < 0)
				nIndex = 0;

			if(nIndex < annotationBrowser.sheetsEventStores.Count)
				annotationBrowser.eventStore =  (ArrayList)annotationBrowser.sheetsEventStores[nIndex];
			annotationBrowser.SendEraseMessage();
									
		}
		private void ForwardToPage(int ncurrentImage)
		{
			annotationBrowser.currentSlideNo=ncurrentImage ; // by kamran
			m_DocumentMessage.CurrentPage = ncurrentImage;  // also check current annotation number and bind to current page
				
			this.MessagePool.Add(m_DocumentMessage);
			NetworkManager.getInstance().SendLoadPacket(m_DocumentMessage);
		}
		
		public void btnNext_Click(object sender, System.EventArgs e)
		{	
			
			if(m_DocumentMessage.CurrentPage+1> m_DocumentMessage.TotalPages)
				return;
			
			ForwardToPage(m_DocumentMessage.CurrentPage+1);
			//currentImage = currentImage + 1;
			//synchornizePPTAnnotations();

		}
		private void btnFirst_Click(object sender, System.EventArgs e)
		{	
			if(currentImage == 1)
				return;		
			ForwardToPage(1);
		}

		private void btnLast_Click(object sender, System.EventArgs e)
		{				
			if(currentImage == this.m_DocumentMessage.TotalPages)
				return;	
			ForwardToPage(this.m_DocumentMessage.TotalPages);
		}			

		private void btnClose_Click(object sender, System.EventArgs e)
		{
		}


		#endregion
		#region Form control events
		private void annotateBox_Click(object sender, System.EventArgs e)
		{
			try
			{
				//MessageBox.Show("Annotate Box Click Thread Name " + System.Threading.Thread.CurrentThread.Name);

				if(currentImage == (int)((PictureBox)sender).Tag) // by kamran 
				{
					return;
				}		
				if(currentImage<=pictureCount)
				{
					ForwardToPage((int)((PictureBox)sender).Tag);
				}		
			}
			catch(Exception)// ee)
			{          

			}			
		}

		private void nsButton2_Click(object sender, System.EventArgs e)
		{
			panelAnnotations.Height = 65;
			//nsButton1.Visible = true;
			//buttonDown.Visible = false;
		}

		private void buttonDown_Click(object sender, System.EventArgs e)
		{
			panelAnnotations.Height = 0;
			//nsButton1.Visible = false;
			//buttonDown.Visible = true;

		}

		private void panel3_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void panel2_Click(object sender, System.EventArgs e)
		{
			
		}

		private void viewBox_Click(object sender, System.EventArgs e)
		{
			Point mousePos=new Point(MousePosition.X,MousePosition.Y);

			//MessageBox.Show(mousePos.X + " " + mousePos.Y);
		}

	
		public void AddPresentationNail(PresentationThumbnail nail)
		{
			this.PresentationNailArray.Add(nail);
			//	this.viewBox.Invalidate();
		}
		private void viewBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if(chkShowThumbNails.Checked==true)
			{
				for(int i=0;i<PresentationNailArray.Count;i++)
				{
					PresentationThumbnail p=(PresentationThumbnail)PresentationNailArray[i];
					if(p.slideNumber == currentImage)
						e.Graphics.DrawString(p.text,new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif,10),new Pen(p.foreColor).Brush,p.x,p.y);				
				}
			}
		}

		private void chkShowThumbNails_CheckedChanged(object sender, System.EventArgs e)
		{
			//is.viewBox.Invalidate();
			if((chkShowThumbNails.Checked))
			{
				if(!annotationsEnabled)
				{
					System.IntPtr pointer = annotationBrowser.IEFromhWnd(axWebBrowser1.Handle);
					if(pointer == System.IntPtr.Zero)
					{
						//ClientUI.getInstance().ShowExceptionMessage("Unable to initialize Annotations. Annotation may not work properly");					
					}
					else
					{
						annotationBrowser.StartSubclass(pointer,axWebBrowser1);
					}

					annotationBrowser.DrawNow = true;
					annotationBrowser.addAnnotations = true;					
					annotationBrowser.thisSessionID = sessionID;
					annotationsEnabled = true;

				}

				
			}

			annotationBrowser.ShowAnnotations= chkShowThumbNails.Checked;

			if(!chkShowThumbNails.Checked)
				chkThumbNail.Checked = false;

			annotationBrowser.AddAnnotations = chkThumbNail.Checked;
			annotationBrowser.SendEraseMessage();


		}

		private void chkThumbNail_CheckedChanged(object sender, System.EventArgs e)
		{
			if(NetworkManager.getInstance().profile.clientType == ClientType.ClientAttendee)
			{
				ClientUI.getInstance().toolbarPanel.Enabled = true;
			}
			if((chkThumbNail.Checked))
			{
				if(!annotationsEnabled)
				{
					System.IntPtr pointer = annotationBrowser.IEFromhWnd(axWebBrowser1.Handle);
					if(pointer == System.IntPtr.Zero)
					{
						//ClientUI.getInstance().ShowExceptionMessage("Unable to initialize Annotations. Annotation may not work properly");					
						
						return;
					}
					else
					{
						annotationBrowser.StartSubclass(pointer,axWebBrowser1);
					}

					annotationBrowser.DrawNow = true;
					annotationBrowser.addAnnotations = true;
					
					annotationBrowser.thisSessionID = sessionID;

					//MessageBox.Show("Annotations Enabled","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
					annotationsEnabled = true;

				}

				
			}
			annotationBrowser.AddAnnotations = chkThumbNail.Checked;

			if(chkThumbNail.Checked)
			{
				chkShowThumbNails.Checked = true;
				annotationBrowser.SendEraseMessage();
			}

		}

		private void textBox_TextChanged(object sender, System.EventArgs e)
		{
		
		}		

	
		private bool Button_onclick(IHTMLEventObj e)
		{
			//MeetingAlerts alert=new MeetingAlerts();
			//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Alert from the app: Received theButton.onclick!",true,false);
			//MessageBox.Show("Alert from the app: Received theButton.onclick!");
			return true;
		}

		private void textBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			/*TextBox txtBox = new TextBox();			
			txtBox.Width = 100;
			txtBox.Height = 25;
			txtBox.Top = e.X;			
			txtBox.Height = e.Y;
			textBox.Controls.Add(txtBox);
			//txtBox.Contro
			*/
		}

		
		#endregion


		private void axWebBrowser1_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
		{
			if(!annotationsEnabled)
			{
				chkShowThumbNails.Checked = true;				
				chkThumbNail.Checked =this.m_bIsAllowedToAddAnnotations;				
			}
		
		}
		[StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode)] 												
			public struct OLECMDTEXT
		{	
			public uint cmdtextf;
			public uint cwActual;
			public uint cwBuf;
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=100)]public char rgwz;
		}

		[StructLayout(LayoutKind.Sequential)] 
		public struct OLECMD
		{
			public uint cmdID;
			public uint cmdf;
		}


		# region PrintPage
		private void PrintPage()
		{
			try
			{	
				object o = "";
				// Constants useful when printing
				SHDocVw.OLECMDID Print = SHDocVw.OLECMDID.OLECMDID_PRINT;
			
				// Use this value to print without prompting
				SHDocVw.OLECMDEXECOPT PromptUser =  SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER;
				
			 
				axWebBrowser1.ExecWB(Print, PromptUser, ref o, ref o);
			}
		   catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>PrintPage() line==> 1071",exp,null,false);			
			}
		}
		# endregion 


		# region SavePage()
		public  void SavePage()
		{
			try
			{
				object o = "";
				// Constants useful when printing
				SHDocVw.OLECMDID Print = SHDocVw.OLECMDID.OLECMDID_SAVEAS;				
				// Use this value to print without prompting
				SHDocVw.OLECMDEXECOPT PromptUser =  SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_PROMPTUSER;								
				axWebBrowser1.ExecWB(Print, PromptUser, ref o, ref o);
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>SavePage( line==> 1091",exp,null,false);			
				//ClientUI.getInstance().ShowExceptionMessage("Unable to save. " + ee.Message);
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,"Unable to save. " + ee.Message ,true,false);
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : SavePage() function ," + ee.Source + " Source ," + ee.Message ,true,false);
				//MessageBox.Show(this.Name + " Class : SavePage() function ," + ee.Source + " Source ," + ee.Message  );
			}
		}	
		# endregion 


		# region printMenu_Click()
		private void printMenu_Click(object sender, EventArgs e)
		{
			try
			{								
				PrintPage();
			}
			
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>printMenu_Click( line==> 1112",exp,null,false);			
		
				//ClientUI.getInstance().ShowExceptionMessage("Unable to communicate with printing device. " + ee.Message);
				//MeetingAlerts alert=new MeetingAlerts();
				// alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,"Unable to communicate with printing device. " ,true,false);
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : printMenu_Click() function ," + ee.Source + " Source ," + ee.Message ,true,false);
				//MessageBox.Show(this.Name + " Class : printMenu_Click() function ," + ee.Source + " Source ," + ee.Message  );

			}
		}

		# endregion



		/************** new Coding ***************
		 * /
		 * */
		# region RecieveThreadFunction()
	


		public void RecieveThreadFunction()
		{
			try
			{
				while(!m_bTerminated)
				{
					if(this.MessagePool.Count < 1)
					{
						Thread.Sleep(500);
						continue;
					}

					MessageObject objMsg = (MessageObject) MessagePool[0];
										
					if((this.IntializationCompleted) && (objMsg.GetType() == typeof(DocumentSharing)))
					{
						
						DocumentSharing DocSharing = (DocumentSharing) this.MessagePool[0];									
						MessagePool.RemoveAt(0);
						//MessageBox.Show(DocSharing.DownloadURL);	
						string[] strLen=DocSharing.DownloadURL.Split(',');						
						//MessageBox.Show(strLen.Length.ToString());
						
						if(strLen.Length>1)
						{
							//DocSharing.DownloadURL=strLen[1];
							this.ProcessMessageManageContent(DocSharing);							
						}
						else			
						{
							if(this.IsLaunchFromMangeContent)
							{
								DocSharing.DownloadURL="Already Downloaded,"+DocSharing.DownloadURL;
								this.ProcessMessageManageContent(DocSharing);							
							}
							else
								ProcessMessage(DocSharing);

						}
							

						// this Method doesn't work 
						annotationBrowser.ScrollView(DocSharing.nScrollX,DocSharing.nScrollY);
					}
					// this Method doesn't work 
					else if (objMsg.GetType() == typeof(DocumentSharingMouseMove))
					{
						DocumentSharingMouseMove oMsg = (DocumentSharingMouseMove)objMsg;
						DrawMouse(oMsg.mousepoint.X,oMsg.mousepoint.Y);
						MessagePool.RemoveAt(0);	
					}
					// this Method doesn't work 
					else if (objMsg.GetType() == typeof(DocumentSharingStatusUpdate))
					{						
						DocumentSharingStatusUpdate DocUpdate = (DocumentSharingStatusUpdate)this.MessagePool[0];						
						MessagePool.RemoveAt(0);
						OnRemoteStatusUpdate(DocUpdate);
					}
                    
				}
			//IsRecieveThreadCompleted = true;
			}
			catch(Exception exp)
			{
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>RecieveThreadFunction() line==> 1192",exp,null,false);			
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,ee.Message,true,false);
				
			}
		}
		# endregion 

		private void DrawMouse(int nX, int nY)
		{
			annotationBrowser._mouseX = nX;
			annotationBrowser._mousey = nY;			
			annotationBrowser.Refresh();

		}


		private void OnRemoteStatusUpdate(DocumentSharingStatusUpdate msg)
		{
			lblStatus.Text = msg.strMessage;
		}

		public void OnStatusUpdate(string strUpdateMessage)
		{
			lblStatus.Text = strUpdateMessage;
		}
	
		private void SetControls(bool bState)
		{
			//chkThumbNail.Visible = true;
			//chkShowThumbNails.Visible = true;			
			chkThumbNail.Enabled = bState;
			chkShowThumbNails.Enabled = bState;			
			
		}


		private void SetDocumentAndPDFInterface()
		{
			axWebBrowser1.Height += btnNext.Height;
			chkThumbNail.Top = lblStatus.Top;
			chkShowThumbNails.Top = lblStatus.Top;
			chkSynch.Left = chkThumbNail.Left;
			chkSynch.Top = chkShowThumbNails.Bottom + 5;
		
			panelAnnotations.Visible = false;
			nsButton1.Visible = false;
			buttonDown.Visible = false;

			btnNext.Visible = false;
			btnPrev.Visible = false;
			btnLast.Visible = false;
			btnFirst.Visible = false;
		}


		#region SetPresentationAndExcelInterface()
		private void SetPresentationAndExcelInterface()

		{
			try
			{
				axWebBrowser1.Height += btnNext.Height;
				panelAnnotations.Height = 65;
				chkSynch.Top = chkSynch.Top;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>SetPresentationAndExcelInterface() line==> 1260",exp,null,false);			
				////MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : SetPresentationAndExcelInterface() function ," + ex.Source + " Source ," + ex.Message,true,false);
			}

		}
		# endregion 

		
		# region AddPictureToThumbNail(System.IntPtr Handle)
		private void AddPictureToThumbNail(System.IntPtr Handle)
		{
			try
			{
				//Trace.WriteLine("Panel Annotation Thread ::::: " + System.Threading.Thread.CurrentThread.Name);
				annotation = new PictureBox();
				this.panelAnnotations.Controls.Add(annotation);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>AddPictureToThumbNail(System.IntPtr Handle) line==> 1282",exp,null,false);			
			}
		}

		#endregion 
		
		# region  CreateThumbnails()
		private void CreateThumbnails()
		{
			try
			{
				PictureBox previous = annotateBox;
				//previous.Top =0;
				int nCount = 1;
				int GAP = 15;
				
				//Rectangle rectpicture=new Rectangle(0,0,0,0);
				int top=0;//annotateBox.Top;
				int left=annotateBox.Left;
				int width=annotateBox.Width;
				int height=annotateBox.Height;


				annotation = previous;
				bool bFirst =true;
				string strImagePath;
//				if(this.IsLaunchFromMangeContent)//check if its ManageContent
//					nCount=1;
//				else
//					nCount=1;
				this.panelAnnotations.Width=80;
				//this.panel1.left=100;
				//this.axWebBrowser1.Left=100;
			
				while(nCount <=  pictureCount)
				{	
					if(!bFirst)	
					{
						Invoke(CreatePictureBox, new object[] {System.IntPtr.Zero } );
						//annotation.Height=previous.Height;						
						annotation.Left=left-10;
						annotation.Width=width;					
						annotation.Height=height;												
						annotation.Top=top;
						//annotation.Left =rectpicture.Left;
						//						annotation.Height=previous.Height;						
						//						annotation.Left=previous.Left;
						//						annotation.Width= previous.Width;					
						//						//annotation.Height = previous.Height;												
						//						annotation.Top =previous.Top+previous.Height+GAP;
					}
					else
					{
						annotation.Left=left-10;
						annotation.Width=width;					
						annotation.Height=height;												
						annotation.Top=top;
					}

					
					annotation.SizeMode = PictureBoxSizeMode.StretchImage;
					if(this.IsLaunchFromMangeContent==true || this.IsLaunchFromMangeContent==false)//check if its ManageContent
					{//Start ManageContent						
						HttpWebRequest req;
						HttpWebResponse	resp;
						try
						{
							if(this.strLocalPath!="")
							{
								if(File.Exists((this.strLocalPath+"\\"+nCount+".jpg")))
								{
									annotation.Image = Image.FromFile(this.strLocalPath+"\\"+nCount+".jpg");
								}
								else
								{
									if(IsLaunchFromMangeContent==true)
										req = (HttpWebRequest) HttpWebRequest.Create(this.strRemoteUrl + nCount + ".jpg");
									else
										req = (HttpWebRequest) HttpWebRequest.Create(this.m_DocumentMessage.DownloadURL + nCount + ".jpg");
							
									req.ProtocolVersion = HttpVersion.Version11;
									req.UserAgent="Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 1.1.4322)";
									//req.CookieContainer=CookieCont;
									//	req.AllowAutoRedirect = CbxAutoRedirect.Checked;
									req.Accept="image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/msword, application/vnd.ms-powerpoint, application/vnd.ms-excel";
									//HttpWebResponse resp;
									try
									{			
										resp = (HttpWebResponse) req.GetResponse();
										if(resp.ContentType=="image/jpeg")
										{
											//Stream rcvStream = resp.GetResponseStream();
											//									if(NewImage!=null)
											//									{
											//										NewImage.Dispose();
											//										NewImage=null;
											//									}
											Image NewImage=Image.FromStream(resp.GetResponseStream());
											if(! Directory.Exists(Application.StartupPath + "\\"+this.sessionID.ToString() ))
											{
												Directory.CreateDirectory(Application.StartupPath + "\\"+this.sessionID.ToString());
											}
											strImagePath=Application.StartupPath+"\\"+this.sessionID.ToString()+ "\\"+nCount+".jpg";
											NewImage.Save(strImagePath,System.Drawing.Imaging.ImageFormat.Jpeg);
											annotation.Image = NewImage;									
//											if(! Directory.Exists(Application.StartupPath + "\\"+this.sessionID.ToString() ))
//											{
//												Directory.CreateDirectory(Application.StartupPath + "\\"+this.sessionID.ToString());
//											}
											
//											lock(annotation)
//											{
//												strImagePath=Application.StartupPath+"\\"+this.sessionID.ToString()+ "\\"+nCount+".jpg";
//												annotation.Image.Save(strImagePath,System.Drawing.Imaging.ImageFormat.Jpeg);
//											}
											//annotation.Image.Save(Application.StartupPath+"\\"+this.sessionID.ToString()+ "\\"+nCount+".jpg", System.Drawing.Imaging.ImageFormat.Jpeg); 
											
											
//											Trace.WriteLine("File Name ::: " + strImagePath);
//											Image.FromStream(resp.GetResponseStream()).Save(strImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

											// save to image //

											resp.Close();
											req=null; 
											resp=null;
											GC.Collect();
											GC.WaitForPendingFinalizers();
											//								this.pictureBox1.Image=NewImage;
											//								this.pictureBox1.Width=NewImage.Width;
											//								this.pictureBox1.Height=NewImage.Height;
										}
									}
									catch (WebException err)
									{
										//MessageBox.Show(err.Status + " - " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
										resp = (HttpWebResponse) err.Response;
										if (resp == null)
										{
										}
									}
								}


							}
							else
							{
								if(IsLaunchFromMangeContent==true)
									req = (HttpWebRequest) HttpWebRequest.Create(this.strRemoteUrl + nCount + ".jpg");
								else
									req = (HttpWebRequest) HttpWebRequest.Create(this.m_DocumentMessage.DownloadURL + nCount + ".jpg");
							
								req.ProtocolVersion = HttpVersion.Version11;
								req.UserAgent="Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 1.1.4322)";
								//req.CookieContainer=CookieCont;
								//	req.AllowAutoRedirect = CbxAutoRedirect.Checked;
								req.Accept="image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/msword, application/vnd.ms-powerpoint, application/vnd.ms-excel";
								//HttpWebResponse resp;
								try
								{			
									resp = (HttpWebResponse) req.GetResponse();
									if(resp.ContentType=="image/jpeg")
									{
										//Stream rcvStream = resp.GetResponseStream();
										//									if(NewImage!=null)
										//									{
										//										NewImage.Dispose();
										//										NewImage=null;
										//									}
										//NewImage=
										Image NewImage=Image.FromStream(resp.GetResponseStream());
										if(! Directory.Exists(Application.StartupPath + "\\"+this.sessionID.ToString() ))
										{
											Directory.CreateDirectory(Application.StartupPath + "\\"+this.sessionID.ToString());
										}
										NewImage.Save(Application.StartupPath+"\\"+this.sessionID.ToString()+ "\\"+nCount+".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
										annotation.Image = NewImage;																												

										//annotation.Image.Save(Application.StartupPath+"\\"+this.sessionID.ToString()+ "\\"+nCount+".jpg", System.Drawing.Imaging.ImageFormat.Jpeg); 

										// save to image //

										resp.Close();
										req=null; 
										resp=null;
										GC.Collect();
										GC.WaitForPendingFinalizers();
										//								this.pictureBox1.Image=NewImage;
										//								this.pictureBox1.Width=NewImage.Width;
										//								this.pictureBox1.Height=NewImage.Height;
									}
								}
								catch (WebException err)
								{
					
									
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==> CreateThumbnails() line==> 1468",err,null,false);				
									//MessageBox.Show(err.Status + " - " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
									resp = (HttpWebResponse) err.Response;
									if (resp == null)
									{
									}
								}
							}

							
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==> CreateThumbnails() line==> 1478",exp,null,false);			
	
						}						
					}//End ManageContent

					
					
					
					
					
					//					//if(this.IsLaunchFromMangeContent)
                        annotation.Tag = nCount;                        					
					//annotation.Image = Image.FromFile(@"C:\Documents and Settings\Danish\My Documents\Visual Studio Projects\Uraan Projects\WebMeetingClient\bin\Debug\Presentation\Copy of presentation2\Slide3.JPG");
					annotation.Visible = true;
					annotation.Invalidate();
					annotation.Click += new EventHandler(annotateBox_Click);
					nCount+=1;
					//previous = annotation;					
					bFirst = false;
					top=top+height+GAP;
					

				}					
	
				annotationBrowser.annotationType = AnnotationMessageType.PPT;
			}
			catch(Exception ee)
			{	
				ee = ee;
			}
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		# endregion 

		
		# region DocumentSharingThread()
		private void DocumentSharingThread()
		{
			//string strExt=".ppt";
			int no_of_RemoteFiles=8;
			string strPostRemoteUrl="";
			bool isRemoteUrlExist=false;
			string recordid="";
			MessageObject objMsg ;
			FileInfo theinfo;
             
			try
			{
				if(!this.IsLaunchFromMangeContent)
				{//in case false-----check manage content slideshow
					//run when document is shared using menus.
					//string Extension =strExt;//Path.GetExtension(DocumentFilePath);
					string filename=Path.GetFileName(DocumentFilePath);
					filenamed=Path.GetFileNameWithoutExtension(DocumentFilePath);
					DocumentFilePath_temp=DocumentFilePath;
					string Extension =Path.GetExtension(DocumentFilePath_temp);
										
					theFile=new FileInfo(filename);
						
					// 10 Mb Check 
					if((theFile.Length/1048576)>10)
					{
						MeetingAlerts alert=new MeetingAlerts();
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"The size of this file is greater then 10Mb which may take time to Upload ",true,false);
							
					}
					//filenamed+=theFile.LastAccessTime.ToFileTime().ToString();
					
					filenamed+=theFile.LastWriteTime.ToFileTime().ToString();
					filenamed=filenamed.Replace('.','z');
					filenamed=filenamed+Extension;
					theFile.CopyTo(filenamed,true);
					DocumentFilePath=Path.GetDirectoryName(DocumentFilePath)+@"\"+filenamed;
					//DocumentFilePath=Path.GetFullPath(DocumentFilePath)+@"\"+filenamed;
					Extension =Path.GetExtension(DocumentFilePath_temp);
					//DocumentFilePath+=Extension
					Extension = Extension.ToLower();
			
					if(Extension == ".doc" ||Extension == ".docx")
					{
						documentType = DocumentMessageType.typeDocument;
						strPostRemoteUrl="/"+Path.GetFileNameWithoutExtension(DocumentFilePath)+".htm";
					}
					else if(Extension == ".xls" ||Extension == ".xlsx")
					{
						documentType = DocumentMessageType.typeExcel;
						if(no_of_RemoteFiles==1)
							strPostRemoteUrl="/"+Path.GetFileNameWithoutExtension(DocumentFilePath)+".htm";
						else
							strPostRemoteUrl="/"+Path.GetFileNameWithoutExtension(DocumentFilePath)+"_files/sheet";
					}
					else if(Extension == ".ppt" || Extension == ".pptx")
					{
						documentType = DocumentMessageType.typePresentation;
						// German PPT
						//strPostRemoteUrl="/Folie";
						strPostRemoteUrl="/slide";
					}
					else if(Extension == ".pdf")
					{
						documentType = DocumentMessageType.typePDF;
						strPostRemoteUrl="/index.htm";
					}
				
					theinfo=new FileInfo(this.DocumentFilePath);											
					recordid=Client.ClientUI.getInstance().findRecord(Path.GetFileName(this.DocumentFilePath),theinfo.LastWriteTime.ToShortDateString().ToString());							
					if(recordid!="-1")
						isRemoteUrlExist=true;
					else
					{
						strLocalPath="";
						isRemoteUrlExist=false;
					}


					//         Zaeem
					if(isRemoteUrlExist==false)
					{
						DocumentSharingEx docEx = new WebMeeting.Client.DocumentSharingEx(
							documentType,this.sessionID,Info.getInstance().FtpIP,Info.getInstance().UserName,
							Info.getInstance().Password,Info.getInstance().ConferenceID);

						docEx.OnStatusUpdateFunction = new WebMeeting.Client.DocumentSharingEx.OnStatusUpdate(OnStatusUpdate);
						//MessageObject objMsg ;
				
						//			docEx.DocumentShareStart(DocumentFilePath,out objMsg);

						
						if(!docEx.DocumentShareStart(DocumentFilePath,out objMsg))
						{
							//Handle the failure;
							MeetingAlerts alert=new MeetingAlerts();
							alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Unable to upload document to webserver",true,false);
							return;
						}
						//						if(WebMeeting.Client.ClientUI.getInstance().InsertNewRecord(filename,((DocumentSharing)objMsg).DownloadURL,this.DocumentFilePath,((DocumentSharing)objMsg).TotalPages,((DocumentSharing)objMsg).DocumentType.ToString(),this.sessionID))
						//							MessageBox.Show("Record Inserted");
						//						else
						//							MessageBox.Show("Record Not Inserted");
						//
						if(Extension == ".doc" ||Extension == ".docx")
						{
							documentType = DocumentMessageType.typeDocument;
							strPostRemoteUrl="/"+Path.GetFileNameWithoutExtension(DocumentFilePath)+".htm";
						}
						else if(Extension == ".xls" || Extension == ".xlsx")
						{
							documentType = DocumentMessageType.typeExcel;
						
							if(((DocumentSharing)objMsg).TotalPages==1)
								//if()
								strPostRemoteUrl="/"+Path.GetFileNameWithoutExtension(DocumentFilePath)+".htm";
							else
								strPostRemoteUrl="/"+Path.GetFileNameWithoutExtension(DocumentFilePath)+"_files/sheet";
						}
						else if(Extension == ".ppt" || Extension == ".pptx")
						{
							documentType = DocumentMessageType.typePresentation;
							// German PPT
							//strPostRemoteUrl="/Folie";
							strPostRemoteUrl="/slide";
						}
						else if(Extension == ".pdf")
						{
							documentType = DocumentMessageType.typePDF;
							strPostRemoteUrl="/index.htm";
						}

						IntializationCompleted = true;
						//((DocumentSharing)objMsg).TotalPages=no_of_RemoteFiles;
						
						((DocumentSharing)objMsg).DownloadURL+=strPostRemoteUrl;
						//MessageBox.Show("==>((DocumentSharing)objMsg).DownloadURL"+((DocumentSharing)objMsg).DownloadURL);
						this.pictureCount = ((DocumentSharing)objMsg).TotalPages;
						//theinfo=new FileInfo(this.DocumentFilePath);						
						Client.ClientUI.getInstance().InsertNewRecord(Path.GetFileName(DocumentFilePath),((DocumentSharing)objMsg).DownloadURL, Application.StartupPath +"\\" + this.sessionID.ToString()  ,((DocumentSharing)objMsg).TotalPages,((DocumentSharing)objMsg).DocumentType.ToString(),this.sessionID,theinfo.LastWriteTime.ToShortDateString().ToString());
						theinfo=null;
					}
						////////////               BY Zaeem                     ///////
					else
					{
						DocumentSharing msg=new DocumentSharing();
						objMsg=msg;
						DataRow drShareMessage;
						drShareMessage=Client.ClientUI.getInstance().getRecord(Convert.ToInt32(recordid));
						string Extsion =Path.GetExtension(drShareMessage["FileName"].ToString());
						this.strLocalPath=drShareMessage["LocalPath"].ToString();
						if(Extsion == ".doc" ||Extsion == ".docx")
						{
							((DocumentSharing)objMsg).DocumentType= DocumentMessageType.typeDocument;
							//strPostRemoteUrl="/"+Path.GetFileNameWithoutExtension(DocumentFilePath)+".htm";
						}
						else if(Extsion == ".xls" || Extsion == ".xlsx")
						{
							((DocumentSharing)objMsg).DocumentType= DocumentMessageType.typeExcel;
							//							if(no_of_RemoteFiles==1)
							//								strPostRemoteUrl="/"+Path.GetFileNameWithoutExtension(DocumentFilePath)+".htm";
							//							else
							//								strPostRemoteUrl="/"+Path.GetFileNameWithoutExtension(DocumentFilePath)+"_files/sheet";
						}
						else if(Extsion == ".ppt" || Extsion == ".pptx")
						{
							((DocumentSharing)objMsg).DocumentType= DocumentMessageType.typePresentation;
							//	strPostRemoteUrl="/slide";
						}
						else if(Extsion == ".pdf")
						{
							((DocumentSharing)objMsg).DocumentType= DocumentMessageType.typePDF;
							//	strPostRemoteUrl="/index.htm";
						}
						this.documentType=((DocumentSharing)objMsg).DocumentType;
						//drShareMessage["LocalPath"]			
						((DocumentSharing)objMsg).SenderID= NetworkManager.getInstance().profile.ClientId;
						((DocumentSharing)objMsg).ConferenceID=NetworkManager.getInstance().profile.ConferenceID;
						((DocumentSharing)objMsg).senderProfile=NetworkManager.getInstance().profile;
						//((DocumentSharing)objMsg).ConferenceID=Info.getInstance().ConferenceID;
						((DocumentSharing)objMsg).sessionID=Convert.ToInt32(drShareMessage["Session"].ToString());
						((DocumentSharing)objMsg).DocumentSharingSessionId=Convert.ToInt32(drShareMessage["Session"].ToString());
						((DocumentSharing)objMsg).CurrentPage=1;
						//						((DocumentSharing)objMsg).MessageType=
						((DocumentSharing)objMsg).nScrollX=0;
						((DocumentSharing)objMsg).nScrollY=0;
						this.sessionID=	Convert.ToInt32(drShareMessage["Session"].ToString());
						IntializationCompleted = true;
						//drShareMessage["Session"]
						((DocumentSharing)objMsg).TotalPages=Convert.ToInt32(drShareMessage["TotalPages"]);
						((DocumentSharing)objMsg).DownloadURL=drShareMessage["RemoteURL"].ToString();
										
						this.pictureCount = ((DocumentSharing)objMsg).TotalPages;
						drShareMessage=null;
					}
			
					
					
	
					ProcessMessage(objMsg); //TODO:: optimized way is to add this mssg to poll and let thread do it
					//but this causses file sharing error while same machine testing				
					
					//MessagePool.Add(objMsg);
					NetworkManager.getInstance().SendMessage(objMsg);
				}//end if
				else
				{//start else
					//runs when document sharing starts from manage content slideshow page
	
					string Extension = this.strExtension;//Path.GetExtension(DocumentFilePath);
					Extension = Extension.ToLower();
			
					if(Extension == ".doc" || Extension == ".docx")
						documentType = DocumentMessageType.typeDocument;
					else if(Extension == ".xls" || Extension == ".xlsx")
						documentType = DocumentMessageType.typeExcel;
					else if(Extension == ".ppt" || Extension == ".pptx")
						documentType = DocumentMessageType.typePresentation;
					else if(Extension == ".pdf")
						documentType = DocumentMessageType.typePDF;
			
					//					DocumentSharingEx docEx = new WebMeeting.Client.DocumentSharingEx(
					//						documentType,this.sessionID,Info.getInstance().FtpIP,Info.getInstance().UserName,
					//						Info.getInstance().Password,Info.getInstance().ConferenceID);

					//					docEx.OnStatusUpdateFunction = new WebMeeting.Client.DocumentSharingEx.OnStatusUpdate(OnStatusUpdate);
					//					MessageObject objMsg ;
				
					//			docEx.DocumentShareStart(DocumentFilePath,out objMsg);

					NetworkManager networkMngr = NetworkManager.getInstance();
					WebMeeting.Common.DocumentSharing message = new WebMeeting.Common.DocumentSharing();
					message.SenderID = networkMngr.profile.ClientId;
					message.senderProfile = networkMngr.profile;
					message.sessionID = this.sessionID;
					message.DocumentSharingSessionId = this.sessionID;
					message.DownloadURL = "Already Downloaded," + this.strRemoteUrl;
					message.DocumentType = documentType;
					message.CurrentPage=1;
					message.TotalPages=this.no_of_pages;

					/*if(!docEx.DocumentShareStart(DocumentFilePath,out objMsg))
					{
						//Handle the failure;
						MeetingAlerts alert=new MeetingAlerts();
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Unable to upload document to webserver",true,false);
						return;
					}*/
					
					IntializationCompleted = true;
					//this.pictureCount = ((DocumentSharing)objMsg).TotalPages; //  by junaid for now
					this.m_DocumentDownloaded=true;
					this.ProcessMessageManageContent(message); //TODO:: optimized way is to add this mssg to poll and let thread do it
					//but this causses file sharing error while same machine testing				
					this.m_DocumentMessage=	message;
					message.DownloadURL="Already Downloaded," + message.DownloadURL;
					//MessagePool.Add(objMsg);
					NetworkManager.getInstance().SendMessage(message);
				}//end else
			
			}
			catch (WebException err)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==> DocumentSharingThread() line==> 1773",err,null,false);				
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : DocumentSharingThread() function ," + ex.Source + " Source ," + ex.Message ,true,false);
			}

			finally
			{
				try
				{
					theFile=new FileInfo(filenamed);
					theFile.Delete();
				}
				catch(Exception exp){}
			}
		}

		# endregion 
		
		# region scrollThreadFunction()
		public void scrollThreadFunction()
		{
			try
			{
				while(!this.m_bTerminated)
				{
					if(	scrollMessagesArray.Count < 1)
					{
						Thread.Sleep(200); // 200 sleep
						continue;
					}
					for(int i = 0 ; i <scrollMessagesArray.Count ; i++)
					{
						NetworkManager.getInstance().SendLoadPacket((MessageObject)scrollMessagesArray[i]);
						scrollMessagesArray.RemoveAt(i);
					}
				}
			}
			catch (WebException err)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==> scrollThreadFunction() line==> 1773",err,null,false);				
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : scrollThreadFunction() function ," + ex.Source + " Source ," + ex.Message ,true,false);
			}
		}
		#endregion 

		/// <summary>
		/// This function is called for presenter 
		/// </summary>
		/// <param name="strFileName"></param>
		/// 
		#region StartDocumentSharingInitiater(string strFileName)
		public void  StartDocumentSharingInitiater(string strFileName)
		{
			try
			{
				
				/*			
				// This Thread does nothing (Junaid's View)
				ScrollingThread = new Thread(new ThreadStart(scrollThreadFunction));
				ScrollingThread.Name = "StartDocumentSharingInitiater Thread: scrollThreadFunction()";
				ScrollingThread.Start();


				
			
				// This Thread does nothing (Junaid's View)
				sendingThread = new Thread(new ThreadStart(threadFunction));
				sendingThread.Name="StartDocumentSharingInitiater Thread: sendingThread() ";
				sendingThread.Start();
			
				*/


				annotationBrowser.IsAttendee = false;				
				// This Thread does nothing (Junaid's View)
				annotationBrowser.OnMouseMoveEx +=new OnMouseMoveDelegate(annotationBrowser_OnMouseMoveEx);

				string Extension = Path.GetExtension(strFileName);
				Extension = Extension.ToLower();
				m_bIsAllowedToAddAnnotations = true;
				m_bOwnerofthisControl = true;
				if(Extension == ".doc" || Extension == ".docx")
					documentType = DocumentMessageType.typeDocument;
				else if(Extension == ".xls" || Extension == ".xlsx")
					documentType = DocumentMessageType.typeExcel;
				else if(Extension == ".ppt" || Extension == ".pptx")
				{
					this.panelAnnotations.Width=80;					
					documentType = DocumentMessageType.typePresentation;
				}
				else if(Extension == ".pdf")
					documentType = DocumentMessageType.typePDF;

				switch(this.documentType)
				{
					case DocumentMessageType.typeDocument: 
					{
						SetDocumentAndPDFInterface();
				
					}
						break;
					case DocumentMessageType.typePDF:	
						SetDocumentAndPDFInterface();
						break;
					case DocumentMessageType.typeExcel:
					{
						SetPresentationAndExcelInterface();
						panelAnnotations.Visible = false;
						nsButton1.Visible = false;
						buttonDown.Visible = false;
					}
						break;
					case DocumentMessageType.typePresentation:
					{
						SetPresentationAndExcelInterface();
						this.panelAnnotations.Width=80;
						Client.ClientUI.getInstance().DisableSlideNav(true);
					}
						break;
				}
				SetControls(false);
				m_bTerminated = false;
				IntializationCompleted = false;

				DocumentFilePath = strFileName;		 // make file path here for manage COntant
	
				RecieveThread = new Thread(new ThreadStart(RecieveThreadFunction));
				RecieveThread.Name = "StartDocumentSharingInitiater Thread: RecieveThreadFunction()";
				RecieveThread.Start();


				//Thread uploadThread = new Thread(new ThreadStart(DocumentSharingThread));
				uploadThread = new Thread(new ThreadStart(DocumentSharingThread));
				uploadThread.Name = "StartDocumentSharingInitiater Thread: DocumentSharingThread()";
				uploadThread.Start();
				
			}
			catch (WebException err)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==> StartDocumentSharingInitiater( line==> 1886",err,null,false);				
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : StartDocumentSharingInitiater() function ," + ex.Source + " Source ," + ex.Message ,true,false);
			}

		}
		#endregion 

		# region StartDocumentSharingInitiaterManageContent(string strFileName,string strextension)
		public void  StartDocumentSharingInitiaterManageContent(string strFileName,string strextension)
		{
			try
			{				
				ScrollingThread = new Thread(new ThreadStart(scrollThreadFunction));
				ScrollingThread.Name = "StartDocumentSharingInitiater Thread: scrollThreadFunction()";
				ScrollingThread.Start();
				sendingThread = new Thread(new ThreadStart(threadFunction));
				sendingThread.Name="StartDocumentSharingInitiater Thread: sendingThread() ";
				sendingThread.Start();
			
				annotationBrowser.IsAttendee = false;
				annotationBrowser.OnMouseMoveEx +=new OnMouseMoveDelegate(annotationBrowser_OnMouseMoveEx);

				string Extension =this.strExtension;// Path.GetExtension(strFileName);
				Extension = Extension.ToLower();
				m_bIsAllowedToAddAnnotations = true;
				m_bOwnerofthisControl = true;
				if(Extension == ".doc" || Extension == ".docx")
					documentType = DocumentMessageType.typeDocument;
				else if(Extension == ".xls" || Extension == ".xlsx")
					documentType = DocumentMessageType.typeExcel;
				else if(Extension == ".ppt" ||Extension == ".pptx")
				{
					documentType = DocumentMessageType.typePresentation;
					this.panelAnnotations.Width=80;
				}
				else if(Extension == ".pdf")
					documentType = DocumentMessageType.typePDF;

				switch(this.documentType)
				{
					case DocumentMessageType.typeDocument: 
					{
						SetDocumentAndPDFInterface();
				
					}
						break;
					case DocumentMessageType.typePDF:	
						SetDocumentAndPDFInterface();
						break;
					case DocumentMessageType.typeExcel:
					{
						SetPresentationAndExcelInterface();
						panelAnnotations.Visible = false;
						nsButton1.Visible = false;
						buttonDown.Visible = false;
					}
						break;
					case DocumentMessageType.typePresentation:
						SetPresentationAndExcelInterface();
						this.panelAnnotations.Width=80;
						break;
				}
				SetControls(false);
				m_bTerminated = false;
				IntializationCompleted = false;

				//DocumentFilePath = strFileName;		 // make file path here for manage COntant
	
				RecieveThread = new Thread(new ThreadStart(RecieveThreadFunction));
				RecieveThread.Name = "StartDocumentSharingInitiater Thread: RecieveThreadFunction()";
				RecieveThread.Start();
				//Thread uploadThread = new Thread(new ThreadStart(DocumentSharingThread));
				uploadThread = new Thread(new ThreadStart(DocumentSharingThread));
				uploadThread.Name = "StartDocumentSharingInitiater Thread: DocumentSharingThread()";
				uploadThread.Start();
			}		
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==> StartDocumentSharingInitiaterManageContent( line==> 1961",exp,null,false);				
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : StartDocumentSharingInitiater() function ," + ex.Source + " Source ," + ex.Message ,true,false);
			}

		}
		# endregion 
		
		#region StartDocumentSharingReciever(MessageObject m)
		public void StartDocumentSharingReciever(MessageObject m)
		{
			try
			{
				if( NetworkManager.getInstance().profile.clientType==ClientType.ClientPresenter ||  NetworkManager.getInstance().profile.clientType==ClientType.ClientHost)
				{
					m_bIsAllowedToAddAnnotations=true;
					chkThumbNail.Checked = true;
					this.btnNext.Visible=true;
					this.btnPrev.Visible=true;
			
					this.chkThumbNail.Visible=true;			

					this.btnFirst.Visible=true;
					this.btnLast.Visible=true;
					this.chkShowThumbNails.Visible=true;
				}
				else if( NetworkManager.getInstance().profile.clientType==ClientType.ClientAttendee)
				{
					
					
					m_bIsAllowedToAddAnnotations=true;
					
					chkThumbNail.Checked = false;
					annotationBrowser.IsAttendee = true;
					chkSynch.Visible = false;
					IsAttendee = true;
					AttendeeFormDocument();
				}
				
				WebMeeting.Common.DocumentSharing DocSharing = (WebMeeting.Common.DocumentSharing)m;
				m_DocumentMessage = DocSharing;
				this.sessionID = DocSharing.sessionID;
				switch(DocSharing.DocumentType)
				{
					case DocumentMessageType.typeDocument: 
						SetDocumentAndPDFInterface();
						break;
					case DocumentMessageType.typePDF:
						SetDocumentAndPDFInterface();
						break;
					case DocumentMessageType.typeExcel:
						SetPresentationAndExcelInterface();
						SetDocumentAndPDFInterface();
					{
						SetPresentationAndExcelInterface();
						panelAnnotations.Visible = false;
						nsButton1.Visible = false;
						buttonDown.Visible = false;
					}
						break;	
					case DocumentMessageType.typePresentation:
						SetPresentationAndExcelInterface();
						SetDocumentAndPDFInterface();

						break;
				};

				IntializationCompleted = true;
				SetControls(false);

			
				MessagePool.Add(m);	
				m_bTerminated = false;			
				RecieveThread = new Thread(new ThreadStart(RecieveThreadFunction));
				RecieveThread.Name = "StartDocumentSharingReciever Thread: RecieveThreadFunction()";
				RecieveThread.Start();
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==> StartDocumentSharingInitiaterManageContent( line==> 1961",exp,null,false);				
				//	MeetingAlerts alert=new MeetingAlerts();
				//	alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : StartDocumentSharingReciever() function ," + ex.Source + " Source ," + ex.Message  ,true,false);
			}

		}
		#endregion 
		
		# region ProcessMessage(MessageObject obj)
		/// <summary>
		/// Processes a message in the queue
		/// </summary>
		/// <param name="obj"></param>
		private void ProcessMessage(MessageObject obj)
		{
			try
			{
				
				if(obj != null)
				{
					
					WebMeeting.Common.DocumentSharing DocSharing = (WebMeeting.Common.DocumentSharing)obj;
					
					DocumentSharingEx DocEx = new WebMeeting.Client.DocumentSharingEx(DocSharing.DocumentType,DocSharing.sessionID);
					DocEx.OnStatusUpdateFunction = new WebMeeting.Client.DocumentSharingEx.OnStatusUpdate(OnStatusUpdate);

					string localFilePath = this.DocumentFilePath;
					SetControls(false);						

/////////////     ===============================================================//////////
///                                      By Zaeem uncomented
/////////////     ===============================================================//////////

		/*			
					// used for downloading documents.					
     					
					if(!m_DocumentDownloaded) // these checks are for ppt and xls sharing cause there can be Next and Previous movement messages
					{ // keep track if the presentation has already been downloaded don't download it again. just move the the current page of presentation
						if(!DocEx.RecieveDocumentSharing(obj,out localFilePath)) //Failed
						{
							this.MessagePool.Remove(obj);
					
							//						alert.ShowMessage(4,"Unable to download document to webserver","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
							//Process failure
						
							return;
						}
						else
							this.m_DocumentDownloaded = true;

						this.DocumentFilePath = localFilePath;
						
						

						annotationBrowser.sheetsEventStores = new ArrayList();
						annotationBrowser.sheetsEventStores.Add(annotationBrowser.eventStore);				
						// only for checking purpose
						try
						{
							for(int i = 1 ;i < DocSharing.TotalPages ; i++) //Create Transparent Annotation windows over the
								//browser control for drawing on each presentation page.
							{
								annotationBrowser.eventStore = new ArrayList();
								annotationBrowser.sheetsEventStores.Add(annotationBrowser.eventStore );						
							}
						}
						catch(Exception ee)
						{
							annotationBrowser.eventStore = new ArrayList();
							annotationBrowser.sheetsEventStores.Add(annotationBrowser.eventStore );
							ee = ee;
						}
						

					
						this.documentTitle = Path.GetFileNameWithoutExtension(DocumentFilePath);
						AddSaveMenus();
					
						
					}
*/
					
/////////////     ===============================================================//////////
///                                      End Block
/////////////     ===============================================================//////////

					
					/////////////
					
					this.DocumentFilePath = localFilePath;
					this.pictureCount = DocSharing.TotalPages;
					//synchornizePPTAnnotations();
					m_DocumentMessage = DocSharing;
				
					switch(m_DocumentMessage.DocumentType)
					{
						case DocumentMessageType.typeDocument:
						{//Load the Word Document
							object oUrl = m_DocumentMessage.DownloadURL;
							object o = new object();	
							if(!m_bVisited)
							{
								this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);						
								lblStatus.Text ="Document " + Path.GetFileName(this.DocumentFilePath);
								m_bVisited = true;
							}						
						
						}
							break;
						case DocumentMessageType.typePresentation:
						{
//							this.DocumentFilePath = localFilePath;
//							localFilePath = Directory.GetParent(localFilePath).FullName;
							//object oUrl = localFilePath + "\\slide" + 
							object oUrl = m_DocumentMessage.DownloadURL+DocSharing.CurrentPage.ToString() + ".jpg";
							object o = new object();
							
							if(currentImage != m_DocumentMessage.CurrentPage)
							{
								if(File.Exists(strLocalPath+"\\"+DocSharing.CurrentPage.ToString() + ".jpg"))
								{
									oUrl=strLocalPath+"\\"+DocSharing.CurrentPage.ToString() + ".jpg";
									this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);						
								}								
								else
									this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);						
							}

							currentImage = m_DocumentMessage.CurrentPage;
							pictureCount = m_DocumentMessage.TotalPages;
							/////////////////////////////////////////
							
							annotationBrowser.currentSlideNo=currentImage ;
							/////////////////////////////////////////
							lblStatus.Text = "Slide " + currentImage.ToString() + " of " + pictureCount.ToString();

						
						}break;
						case DocumentMessageType.typePDF:
						{
							//object oUrl = Path.GetDirectoryName(localFilePath) + "\\"+  Path.GetFileNameWithoutExtension(localFilePath) + "\\index.htm";
							object oUrl=this.m_DocumentMessage.DownloadURL;
							object o = new object();	
							if(!m_bVisited)
							{
								this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
								m_bVisited = true;
							}
						}
							break;
						case DocumentMessageType.typeExcel:
						{
							object oUrl = m_DocumentMessage.DownloadURL;
							object o = new object();							
							if((this.pictureCount == 1) || (m_DocumentMessage.TotalPages == 1))
							{
								lblStatus.Text = "Book 1 of 1";
								this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);						
							}
							else
							{
								
								string basePath = m_DocumentMessage.DownloadURL;	
								//string basePath = Path.GetDirectoryName(localFilePath);
								//basePath +=  "\\" + Path.GetFileNameWithoutExtension(localFilePath) + "_files\\sheet";
								//currentImage+=1;
								if(this.m_DocumentMessage.CurrentPage > 10)
									basePath += this.m_DocumentMessage.CurrentPage.ToString() + ".htm";
								else
									basePath += "00" +  this.m_DocumentMessage.CurrentPage.ToString() + ".htm";
								//MessageBox.Show(basePath);
							
								oUrl = basePath;
								if(!m_bVisited)
								{
									this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
									lblStatus.Text = "Book " + currentImage.ToString() + " of " + pictureCount.ToString();
									if(this.m_DocumentMessage.TotalPages==1)
										m_bVisited =true;
									
								}
							
							}						

		
						}
							break;
					}		
					SetControls(true);	
					lblStatus.Visible = false;
				
					if(m_bOwnerofthisControl)
					{
						btnOptions.Visible = true;
						btnOptions.Enabled = true;
						btnOptions.Top = btnLast.Top+4;
						//chkSynch.Visible = true;
					}
					else
					{
						btnOptions.Visible = false;
							
					}
					chkThumbNail.Enabled =  m_bIsAllowedToAddAnnotations;			
					

					if(this.documentType == DocumentMessageType.typePresentation && chkThumbnail==false)
					{
						try
						{
							if(ClientUI.getInstance().IsShowThumbnails())/*check either we need to display thumnails or not*/
							{
								createThumbnailThread = new Thread(new ThreadStart(CreateThumbnails));
								createThumbnailThread.Name = "createThumbnailThread() : documentSharingControl";
								createThumbnailThread.Start();
							}
						}
						catch(Exception ex)
						{ 
							ex = ex;
						}
						//CreateThumbnails();

						chkThumbnail=true;
						this.panelAnnotations.Height=65;
					}

	
				}
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>ProcessMessage( line==> 2265",exp,null,false);				
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : ProcessMessage() function ," + ex.Source + " Source ," + ex.Message  ,true,false);
			}
		}

		#endregion 
		/// <summary>
		/// Processes a MangeContent SlideShow message in the queue
		/// </summary>
		/// <param name="obj"></param>
		#region ProcessMessageManageContent
		private void ProcessMessageManageContent(WebMeeting.Common.DocumentSharing DocSharing)
		{
			try
			{
//				if(DocSharing!=null)
//				{
//					string[] strSplit=DocSharing.DownloadURL.Split(@",");
//					
//
//				}

							
				if(DocSharing != null)
				{
					
//					MessageBox.Show(DocSharing.DownloadURL);
					string[] strLen=DocSharing.DownloadURL.Split(',');						
//					MessageBox.Show(strLen.Length.ToString());
					if(strLen.Length>0)
					{
						DocSharing.DownloadURL=strLen[1];
						
					}
					//WebMeeting.Common.DocumentSharing DocSharing = (WebMeeting.Common.DocumentSharing)obj;
					
					//DocumentSharingEx DocEx = new WebMeeting.Client.DocumentSharingEx(DocSharing.DocumentType,DocSharing.sessionID);
					//DocEx.OnStatusUpdateFunction = new WebMeeting.Client.DocumentSharingEx.OnStatusUpdate(OnStatusUpdate);

					string localFilePath = this.strRemoteUrl;//this.DocumentFilePath;
					SetControls(false);						
					
					
					
//					if(!m_DocumentDownloaded) // these checks are for ppt and xls sharing cause there can be Next and Previous movement messages
//					{ // keep track if the presentation has already been downloaded don't download it again. just move the the current page of presentation
//						if(!DocEx.RecieveDocumentSharing(obj,out localFilePath)) //Failed
//						{
//							this.MessagePool.Remove(obj);
//					
//							//						alert.ShowMessage(4,"Unable to download document to webserver","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
//							//Process failure
//						
//							return;
//						}
//						else
//							this.m_DocumentDownloaded = true;
//
//						this.DocumentFilePath = localFilePath;
//
//						annotationBrowser.sheetsEventStores = new ArrayList();
//						annotationBrowser.sheetsEventStores.Add(annotationBrowser.eventStore);				
//						/*  // only for checking purpose
//						try
//						{
//							for(int i = 1 ;i < DocSharing.TotalPages ; i++) //Create Transparent Annotation windows over the
//								//browser control for drawing on each presentation page.
//							{
//								annotationBrowser.eventStore = new ArrayList();
//								annotationBrowser.sheetsEventStores.Add(annotationBrowser.eventStore );						
//							}
//						}
//						catch(Exception ee)
//						{
//							annotationBrowser.eventStore = new ArrayList();
//							annotationBrowser.sheetsEventStores.Add(annotationBrowser.eventStore );
//							ee = ee;
//						}
//						*/
//
//					
//						this.documentTitle = Path.GetFileNameWithoutExtension(DocumentFilePath);
//						AddSaveMenus();
//					
//						
//					}
					
					
					/////////////
					
					//this.DocumentFilePath = localFilePath;
					//this.pictureCount = DocSharing.TotalPages;
					//synchornizePPTAnnotations();
//					m_DocumentMessage = DocSharing;
				
					switch(DocSharing.DocumentType)
					{
						case DocumentMessageType.typeDocument:
						{//Load the Word Document
							object oUrl = DocSharing.DownloadURL;
							object o = new object();	
							if(!m_bVisited)
							{
								this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);						
								lblStatus.Text ="Document " + Path.GetFileName(localFilePath);
								m_bVisited = true;
							}												
						}
							break;
						case DocumentMessageType.typePresentation:
						{
							//this.DocumentFilePath = localFilePath;
							//localFilePath = Directory.GetParent(localFilePath).FullName;
							//object oUrl = localFilePath + "\\slide" + DocSharing.CurrentPage.ToString() + ".jpg";
							object oUrl = DocSharing.DownloadURL+ DocSharing.CurrentPage.ToString() + ".jpg";
							object o = new object();	
							if(currentImage != DocSharing.CurrentPage)
							{
								this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);						
							}

							currentImage = DocSharing.CurrentPage;
							pictureCount = DocSharing.TotalPages;
							/////////////////////////////////////////
							
							annotationBrowser.currentSlideNo=currentImage ;
							/////////////////////////////////////////
							lblStatus.Text = "Slide " + currentImage.ToString() + " of " + pictureCount.ToString();

						
						}break;
						case DocumentMessageType.typePDF:
						{
							//object oUrl = Path.GetDirectoryName(localFilePath) + "\\"+  Path.GetFileNameWithoutExtension(localFilePath) + "\\index.htm";
							object oUrl=DocSharing.DownloadURL;
							object o = new object();	
							if(!m_bVisited)
							{
								this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
								m_bVisited = true;
							}
						}
							break;
						case DocumentMessageType.typeExcel:
						{
							object oUrl = DocSharing.DownloadURL;
							object o = new object();							
							if((this.pictureCount == 1) || (DocSharing.TotalPages == 1))
							{
								lblStatus.Text = "Book 1 of 1";
								this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);						
							}
							else
							{
								string basePath = DocSharing.DownloadURL;
								//basePath +=  "\\" + Path.GetFileNameWithoutExtension(localFilePath) + "_files\\sheet";
								//currentImage+=1;
								if(currentImage > 10)
									basePath += DocSharing.CurrentPage.ToString() + ".htm";
								else
									basePath += "00" +  DocSharing.CurrentPage.ToString() + ".htm";
								//MessageBox.Show(basePath);
							
								oUrl = basePath;
								if(!m_bVisited)
								{
									this.axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
									lblStatus.Text = "Book " + currentImage.ToString() + " of " + pictureCount.ToString();
									//m_bVisited =true;
								}
							
							}						

		
						}
							break;
					}		
					SetControls(true);	
					lblStatus.Visible = false;
									
					if(m_bOwnerofthisControl)
					{
						btnOptions.Visible = true;
						btnOptions.Enabled = true;
						btnOptions.Top = btnLast.Top+4;									
						//chkSynch.Visible = true;
					}
					else
					{
						btnOptions.Visible = false;
							
					}
					chkThumbNail.Enabled =  m_bIsAllowedToAddAnnotations;			
					if(this.documentType == DocumentMessageType.typePresentation && chkThumbnail==false)
					{
						if(ClientUI.getInstance().IsShowThumbnails())/*check either we need to display thumnails or not*/
						{
							createThumbnailThread = new Thread(new ThreadStart(CreateThumbnails));
							createThumbnailThread.Name = "createThumbnailThread() : documentSharingControl";
							createThumbnailThread.Start();
							chkThumbnail=true;
							this.panelAnnotations.Height=65;
						}
					}
				}
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>ProcessMessageManageContent( line==> 2470",exp,null,false);				
//				MeetingAlerts alert=new MeetingAlerts();
//				alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : ProcessMessageManageContent() function ," + ex.Source + " Source ," + ex.Message  ,true,false);
			}
		}

		#endregion 

		# region btnOptions_Click(object sender, System.EventArgs e)
		private void btnOptions_Click(object sender, System.EventArgs e)
		{
			try
			{
				ArrayList arrayParticipents = ClientUI.getInstance().arrayParticipents;
				Hashtable tempHashTable = Hashtable.Synchronized(new Hashtable());
				DocumentSharingOptions objOptions = new DocumentSharingOptions();
				objOptions.listAllowed.Items.Clear();
				for(int i = 0 ; i < arrayParticipents.Count ;i++)
				{
					ClientProfile profile = (ClientProfile)arrayParticipents[i];
					ParticipentInfo p = new ParticipentInfo();
					if(profile.clientType == ClientType.ClientPresenter)
						p.m_ClientType = "Presenter";
					else if(profile.clientType == ClientType.ClientAttendee)
						p.m_ClientType = "Presenter";
					else if(profile.clientType == ClientType.ClientHost)
						p.m_ClientType = "Host";
					p.m_ParticipentName = profile.Name;
				
					if(this.ParticipentsArrayList.ContainsKey(profile.ClientId))
						p.m_bIsAllowed = ((ParticipentInfo)this.ParticipentsArrayList[profile.ClientId]).m_bIsAllowed;				
                				
					tempHashTable.Add(profile.ClientId,p);		
				
					ListViewItem lv = objOptions.listAllowed.Items.Add(p.m_ClientType);
					lv.SubItems.Add(p.m_ParticipentName);
					lv.Checked = p.m_bIsAllowed;  
					lv.Tag = profile.ClientId;

				}
				ParticipentsArrayList.Clear();
				ParticipentsArrayList = tempHashTable;
				if(objOptions.ShowDialog() == DialogResult.OK) 
				{	
				
				
					for(int i = 0 ; i < objOptions.listAllowed.Items.Count ; i++)
					{
						ListViewItem lvTemp =  objOptions.listAllowed.Items[i];//.Checked;
						ParticipentInfo pTemp = ((ParticipentInfo)ParticipentsArrayList[lvTemp.Tag]);
						pTemp.m_bIsAllowed = lvTemp.Checked;
						AnnotationControlMessage msg = new AnnotationControlMessage(this.sessionID,NetworkManager.getInstance().profile);
						msg.ControlMessageType = ControlType.DocumentSharing;
						msg.IsAllowed = lvTemp.Checked;
						NetworkManager.getInstance().SendLoadPacket(msg);
					}
				}
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>btnOptions_Click( line==> 2531",exp,null,false);				
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : btnOptions_Click() function ," + ex.Source + " Source ," + ex.Message  ,true,false);
			}
		}	
		#endregion 

		#region custom Structure
		public struct ParticipentInfo
		{
			public string m_ParticipentName;
			public string m_ClientType;
			public bool m_bIsAllowed;
		}
		#endregion
		
	
		/// <summary>
		/// ClearThumbnailsFromPanel() used to clear thumnails from the panelAnnotations control and dispose it.
		/// </summary>
		public void ClearThumbnailsFromPanel()
		{
			try
			{
				/*clear all the controls*/
				this.panelAnnotations.Controls.Clear();
				/*dispose panelannotation control*/
				this.panelAnnotations.Dispose();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>threadFunctionStop() line==> 2594",ex,null,false);
			}
			
		}
		

		/// <summary>
		/// threadFunctionStop () used to stop threads at stop document sharing , \nwhich are started at initialization of document sharing
		/// </summary>
		#region  threadFunctionStop()
		public void threadFunctionStop()
		{
			try
			{
				this.annotationBrowser.abortThreads();
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>threadFunctionStop() line==> 2561",exp,null,false);				
			}
			

			try
			{
				if(ScrollingThread != null)
				{
					ScrollingThread.Abort();
					ScrollingThread.Join(10000);
				}
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>threadFunctionStop() line==> 2575",exp,null,false);				
			}
	
			try
			{
				if(RecieveThread != null)
				{
					RecieveThread.Abort();
					RecieveThread.Join(10000);
				}
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>threadFunctionStop() line==> 2588",exp,null,false);				
			}
	
			try
			{
				if(uploadThread != null)
				{
					uploadThread.Abort();
					uploadThread.Join(10000);
				}
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>threadFunctionStop() line==> 2601",exp,null,false);				
			}
	
			try
			{
				if(sendingThread != null)
				{
					sendingThread.Abort();
					sendingThread.Join(10000);
				}
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>threadFunctionStop() line==> 2614",exp,null,false);				
			}
			try
			{
				if(createThumbnailThread  != null)
				{
					if(createThumbnailThread.IsAlive)
					{
						createThumbnailThread.Abort();
						createThumbnailThread.Join(10000);
					}
				}
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>threadFunctionStop() line==> 2614",exp,null,false);				
			}
	
		}

		# endregion 
		
		# region  threadFunction()
		private void threadFunction()
		{
			try
			{
				while(!this.m_bTerminated)
				{
					if(scrollMessages.Count < 1)
					{
						Thread.Sleep(20); // 200
						continue;
					}

					for(int i = 0 ; i <scrollMessages.Count ; i++)
					{
						NetworkManager.getInstance().SendLoadPacket((MessageObject)scrollMessages[i]);					
						//System.Diagnostics.Debug.WriteLine("x = " + ((DocumentSharingMouseMove)scrollMessages[i]).mousepoint.X  + " , y = " +((DocumentSharingMouseMove)scrollMessages[i]).mousepoint.Y);
						scrollMessages.RemoveAt(i);
						i--;
					}
					scrollMessages.Clear();
				}
			}
			catch (WebException exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>threadFunction() line==> 2647",exp,null,false);				
			//MeetingAlerts alert=new MeetingAlerts();
			//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : btnOptions_Click() function ," + ex.Source + " Source ," + ex.Message ,true,false);
			}
		}
	# endregion

		# region annotationBrowser_OnScroll(int nVScroll, int nHScroll)
		private void annotationBrowser_OnScroll(int nVScroll, int nHScroll)
		{			
			try
			{
				lastScrolPos.X = nHScroll;
				lastScrolPos.Y = nVScroll;
				if(!IsAttendee && chkSynch.Checked)
				{		
					DocumentSharing _msg = new DocumentSharing();
					_msg.bEnabled = m_DocumentMessage.bEnabled;
					_msg.CurrentPage = m_DocumentMessage.CurrentPage;
					_msg.DocumentSharingSessionId = m_DocumentMessage.DocumentSharingSessionId;
					_msg.DocumentType = m_DocumentMessage.DocumentType;
					_msg.DownloadURL = m_DocumentMessage.DownloadURL;
					_msg.MessageType = m_DocumentMessage.MessageType;
					_msg.nScrollX = nHScroll;
					_msg.nScrollY = nVScroll;
					_msg.SenderID = m_DocumentMessage.SenderID;
					_msg.senderProfile= m_DocumentMessage.senderProfile;
					_msg.sessionID= m_DocumentMessage.sessionID;
					_msg.TotalPages= m_DocumentMessage.TotalPages;
					_msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
					scrollMessages.Add(_msg);
					//scrollMessagesArray.Add(_msg);
				}
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>annotationBrowser_OnScroll( line==> 2684",exp,null,false);				
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : annotationBrowser_OnScroll() function ," + ex.Source + " Source ," + ex.Message  ,true,false);
			}
		}

		# endregion 
		
		# region button1_Click(object sender, System.EventArgs e)
		private void button1_Click(object sender, System.EventArgs e)
		{
			try
			{
				//annotationBrowser.FireScrollEvent();			
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>button1_Click( line==> 2704",exp,null,false);				
			}
		}
		#endregion 

		# region annotationBrowser_OnMouseMoveEx(int X, int Y)
		private void annotationBrowser_OnMouseMoveEx(int X, int Y)
		{
			try
			{
				if(this.m_bOwnerofthisControl)
				{
					if(annotationBrowser.tool !=WhiteboardToolCode.None)
					{
																	
						if((_mouseLastX != X) || (_mouseLastY != Y))
						{
							
							if(scrollMessages.Count > 50) // 25 
							{
								scrollMessages.Clear(); // for checking purpose
							}

							DocumentSharingMouseMove msg = new DocumentSharingMouseMove();
							msg.DocumentType = this.documentType;
							msg.sessionID = this.sessionID;
							msg.mousepoint.X = X + lastScrolPos.X ;  // add scroll bar lenghts.
							msg.mousepoint.Y = Y + lastScrolPos.Y ;
							msg.SenderID = NetworkManager.thisInstance.profile.ClientId;
				
							scrollMessages.Add(msg);
						}
						_mouseLastX = X;
						_mouseLastY = Y;
					}
				}
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>annotationBrowser_OnMouseMoveEx( line==> 2748",exp,null,false);				
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : annotationBrowser_OnMouseMoveEx() function ," + ex.Source + " Source ," + ex.Message,true,false);
			}
		}
		#endregion 

		#region ~documentSharingControl()

		~documentSharingControl()
		{
			try
			{	
				//System.IO.File.Create(Application.StartupPath + "\\flag.txt");
				//MessageBox.Show("Webmeeting document Sharing Control dispose called");
				this.Dispose(true);
			}
			catch (WebException exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("documentSharingControl.cs ==>~documentSharingControl() line==> 2767",exp,null,false);				
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,this.Name + " Class : ~documentSharingControl() function ," + ex.Source + " Source ," + ex.Message  ,true,false);				
			}
		}
				
		#endregion 	
	}

}
