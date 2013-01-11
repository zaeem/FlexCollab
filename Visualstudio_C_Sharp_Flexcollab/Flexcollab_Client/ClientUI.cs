//#define __DEBUGn

//#undef __DEBUG
#define dbgTesting

using System.Text;
using System.Net;
using System.IO; 

using System;
using System.Drawing; 
using System.Collections; 
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;  
using WebMeeting.Common;
using System.Threading;     
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking; 
using WebMeetingDrawingBoard;
using System.Runtime.InteropServices; 
using Utility.NiceMenu;
using System.Data.OleDb;
using WebMeeting; 
using WebMeeting.Client.StatusBar;
using WebMeeting.OAKControls;
using mshtml;
using System.Diagnostics; 
using System.Xml;
using rpaulo.toolbar;
using SpecialServices;
using WebMeeting.Client.Alerts;
using WebMeeting.Client.Xml_Handling;
using IMWindow;
using Microsoft.Win32;
using Crownwood.Magic.Menus;
using System.Reflection;
using WebMeeting.FTP;
using System.Configuration;
using Win32;
//using  DMSoft;


namespace WebMeeting.Client
{
	delegate void delegatebtn_Whiteboard_Click(object sender, System.EventArgs e);
	public delegate void closeAppShare(int i);
	public delegate void closeGeneric(int i);
	public delegate void closeAll();

	

	public class ClientUI : System.Windows.Forms.Form
	{
		#region Variable Declerations and Instentiations
		
		public WebMeeting.Client.Minuts_Meeting.Minuts_Controller MM_Controller;
		public WebMeeting.Client.ClientDataBase.DatabaseAccess db;
		public	FtpClient fcu;
		public	Minuts_Meeting.Progressbar pb;
		public static MeetingAlerts Flexalert;
		public static browser browserControl;
		public static bool flag_ImageMode=false;
		public static bool flag_MouseMove_whiteboard=true;
		public static object Lock_MsgPool_SendAnnotation;
		public static object Lock_MsgPool_Annotation;


		public static bool Flag_MsgPool_SendAnnotation=false;
		public static bool Flag_MsgPool_Annotation=false;




		public static browser browserMM=null;
		public closeAppShare del_closeAppShare;
		public closeGeneric del_closeGeneric;
		public closeAll del_closeAll;
		public bool flag_checkbox;
		public int saveID;
		public string str_cdate;
		public string str_ctime;

		public int var_no_appshare=1;
		public int var_no_docshare=1;
		public int var_temp_appshare=0;
		public int var_temp_docshare=0;
		public int var_temp_webshare=0;


		string[] commandArgs;
		public PollResultsMessage tempPollMessage;
		public bool bAskPresenter = true;

		public ArrayList ApplicationSharingTabPagesArray = new ArrayList();
		public bool myAppShareStarted = false;
		public ApplicationSharing _appShareControl;
		public ApplicationSharing DesktopSharingserver = new ApplicationSharing(); 
		public bool IsGuest = false;
		public bool IsHostVideoEnabled = false;
		public Image tempImage;		
		
		
		//		public MouseEventCancelHook TabControlsSwitchingHook = new MouseEventCancelHook();
		
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private NSPAControls.NSButton btn_Invite;
		private System.Windows.Forms.Button btnPlaceHolderArrow;
		private BalloonCS.HoverBalloon m_listBalloon = new BalloonCS.HoverBalloon();
		private docsSharing dsStores;//use to manipulate the docsharing database
		
		//public static ArrayList DocumentShareFolderName = new ArrayList();
		public Screen_Capture.frm_Rec fm;	
		private System.Windows.Forms.MenuItem mnuAboutCompassnavMeeting;
		private NSPAControls.NSButton btn_Chat;
		private System.Windows.Forms.Panel panalAdvertiseBrowser;		
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.Splitter splitter3;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button btnRestoreUp;
		private mshtml.HTMLDocument doc;
		private mshtml.HTMLDocument doc2;
		
		private static bool bArgument;	

		public int nclientType=-1;
		public int nmeeting_id=-1;


		private string strCmdArg;
		private NSPAControls.NSButton btn_Record;
		private System.Windows.Forms.ImageList imageList2;
		private System.Windows.Forms.Panel panelSplitter;
		private NSPAControls.NSButton btnNext;
		private NSPAControls.NSButton btnPrev;
		private System.Windows.Forms.MenuItem mnuSynchronize;
		private System.Windows.Forms.MenuItem mnuSynchronizeD;
		private System.Windows.Forms.MenuItem mnuApplication;
		private System.Windows.Forms.MenuItem mnuWebBrowser;
		private System.Windows.Forms.MenuItem mnupDocument;
		private System.Windows.Forms.MenuItem mnuWhiteboard;
		private System.Windows.Forms.MenuItem mnuVideo;
		private System.Windows.Forms.MenuItem mnuAudio;
		private System.Windows.Forms.MenuItem mnuInvite;
		private System.Windows.Forms.MenuItem mnumdetails;
		private NSPAControls.NSButton btn_Webshare;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuQA;
		private System.Windows.Forms.MenuItem mnuPoll;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem mnu_Minuts;
		private System.Windows.Forms.MenuItem mnu_MM_Wht;
		private System.Windows.Forms.MenuItem mnu_MM_App;
		private System.Windows.Forms.MenuItem mnu_MM_Web;
		private NSPAControls.NSButton btn_Poll;
		private NSPAControls.NSButton btn_QA;
		

		public enum AlertType
		{
			Info=1,Warning=2,Action=3,NonFatal=4,Fatal=5
		}
		
		

		public ClientProfile tempProfile = null;
		public ComboBox sendTo;
		private bool BoolrecordingWarningSent =false;
		private bool CheckBoxStateChangeByNetwork=false;
		public bool PasswordError = false;
		public string ImageURL;

		//true when value of ExceptionBehaviour is set show
		//false when value of ExceptionBehaviour is set log
		public bool bIsExceptionShow=false;
		public int nExceptionCounter=0;
		Login loginFrm = new Login();	
		public NSpring.Logging.Loggers.FileLogger logger;//= new FileLogger(Application.StartupPath + "LogFile.txt");
		private System.Windows.Forms.ContextMenu cmenu_drawingTools = new ContextMenu();
		public delegate void FnSynchWindow(TabPagesSyncrhonizationMsessage msg);
		public FnSynchWindow SynchWindowProc;
		rightcarrotWindow window = new rightcarrotWindow();

		private NetworkManager network;		
		public Info info;

		#region designer variable decleration		
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemSave;
		private System.Windows.Forms.MenuItem menuItemSaveAs;
		private System.Windows.Forms.MenuItem menuItemSaveAll;
		private System.Windows.Forms.MenuItem menuItemPrint;
		private System.Windows.Forms.MenuItem menuItemSaveDocument;
		private System.Windows.Forms.MenuItem menuItemSavePollQuestionnaire;
		private System.Windows.Forms.MenuItem menuItemSavePollResult;
		private System.Windows.Forms.MenuItem menuItemSaveChat;
		private System.Windows.Forms.MenuItem menuItemSaveNotes;
		private System.Windows.Forms.MenuItem menuItemSaveAsDocument;
		private System.Windows.Forms.MenuItem menuItemSaveAsPollQuestionnaire;
		private System.Windows.Forms.MenuItem menuItemSaveAsPollResult;
		private System.Windows.Forms.MenuItem menuItemSaveAsChat;
		private System.Windows.Forms.MenuItem menuItemSaveAsNotes;
		private System.Windows.Forms.MenuItem menuItemPrintDocument;
		private System.Windows.Forms.MenuItem menuItemPrintPollQuestionnaire;
		private System.Windows.Forms.MenuItem menuItemPrintPollResult;
		private System.Windows.Forms.MenuItem menuItemPrintChat;
		private System.Windows.Forms.MenuItem menuItemPrintNotes;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItemUndo;
		private System.Windows.Forms.MenuItem menuItemRedo;
		private System.Windows.Forms.MenuItem menuItemAddPage;
		private System.Windows.Forms.MenuItem menuItemPastePage;
		private System.Windows.Forms.MenuItem menuItemClearAllAnnotations;
		private System.Windows.Forms.MenuItem menuItemClearMyPointer;
		private System.Windows.Forms.MenuItem menuItemClearAllPointers;
		private System.Windows.Forms.MenuItem menuItem3;
		//private System.Windows.Forms.MenuItem menuItemSharePresentation;
		private System.Windows.Forms.MenuItem menuItemShareWhiteboard;
		private System.Windows.Forms.MenuItem menuItemShareWebContent;
		private System.Windows.Forms.MenuItem menuItemShareApplication;
		private System.Windows.Forms.MenuItem menuItemShareWebBrowser;
		private System.Windows.Forms.MenuItem menuItemShareDesktop;
		private System.Windows.Forms.MenuItem menuItemShareRemoteComputer;
		private System.Windows.Forms.MenuItem menuItemClear;
		private System.Windows.Forms.MenuItem menuItemFont;
		private System.Windows.Forms.MenuItem menuItemView;
		private System.Windows.Forms.MenuItem menuItemViewFullScreen;
		private System.Windows.Forms.MenuItem menuItemViewThumbNails;
		private System.Windows.Forms.MenuItem menuItemViewZoomIn;
		private System.Windows.Forms.MenuItem menuItemZoomOut;
		private System.Windows.Forms.MenuItem menuItemViewZoomBy;
		private System.Windows.Forms.MenuItem menuItemViewFitInViewer;
		private System.Windows.Forms.MenuItem menuItemFirInWidth;
		private System.Windows.Forms.MenuItem menuItemViewAutomaticallyAdvancePages;
		private System.Windows.Forms.MenuItem menuItemViewSynchronizeAll;
		private System.Windows.Forms.MenuItem menuItemAudio;
		private System.Windows.Forms.MenuItem menuItemTools;
		private System.Windows.Forms.MenuItem menuItemHelp;
		private System.Windows.Forms.MenuItem menuItemToolsEnableContinuation;
		private System.Windows.Forms.MenuItem menuItemToolsInternetPhone;
		private System.Windows.Forms.MenuItem menuItemToolsOpenRecorder;
		private System.Windows.Forms.MenuItem menuItemToolsRecorderSettings;
		private System.Windows.Forms.MenuItem menuItemToolsRecordAudio;
		private System.Windows.Forms.MenuItem menuItemInviteOthers;
		private System.Windows.Forms.MenuItem menuItemHelpLiveChat;
		private System.Windows.Forms.MenuItem menuItemHelpMeetingManager;
		private System.Windows.Forms.MenuItem menuItemHelpAboutMeetingManager;
		private System.Windows.Forms.MenuItem menuItemVerifyRichMedia;
		private System.Windows.Forms.MenuItem menuItemHelpMeetingDetails;
		private System.Windows.Forms.MenuItem menuItemHelpTeleconferenceDetails;
		#endregion

		private System.ComponentModel.IContainer components;
		Crownwood.Magic.Controls.TabPage tabPageFind = new Crownwood.Magic.Controls.TabPage("Find Participants");
		WebMeeting.Client.findparticipents objFindParticipetsControl = new findparticipents();

		#region Form Variables
		
		protected Crownwood.Magic.Docking.
			DockingManager _dockingManager = null;
		private System.Windows.Forms.MainMenu mainMenu1;
		public System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		public System.Windows.Forms.MenuItem menuItem32;
		public System.Windows.Forms.MenuItem menuItem51;
		public System.Windows.Forms.MenuItem menuItem59;
		private System.Windows.Forms.Panel panelQuickLaunch;
		public ArrayList arrayParticipents  = new ArrayList();
		public WebMeeting.chatControl chatControl1;
		
		public WebMeetingDrawingBoard.WhiteboardControl whiteBoard;		
		
		public Crownwood.Magic.Controls.TabPage tabPageChat =
			new Crownwood.Magic.Controls.TabPage();
		public Crownwood.Magic.Controls.TabPage tabPageNotes =
			new Crownwood.Magic.Controls.TabPage();

		public Crownwood.Magic.Controls.TabPage tabPageAdvertise=
			new Crownwood.Magic.Controls.TabPage();


		// tab control for video, attendee host list etc.
		public Crownwood.Magic.Controls.TabControl tabControl1;
		private System.Windows.Forms.Panel rightPanel;
		private System.Windows.Forms.ImageList whiteBoardtoolbarImageList;
		public Crownwood.Magic.Controls.TabControl chatTabControl;
		public Crownwood.Magic.Controls.TabControl advertiseTabControl;
		public static ClientUI thisInstance;


		//desktop Sharing variables
				
		public WebMeeting.Common.ClientProfile desktopSharedProfile;
		public bool bIsDesktopServer = false;

		public bool bAlreadyPainting = false; // already painting the picture box.
		public Crownwood.Magic.Controls.TabPage tabPageDesktopSharing = new Crownwood.Magic.Controls.TabPage("Remote Control");
		
		private Point prev=new Point(0,0);

		private System.Windows.Forms.MenuItem menu_Close;
		private System.Windows.Forms.ContextMenu tabPagesMenu;
	
		public Crownwood.Magic.Controls.TabPage tabPageInfo =	new Crownwood.Magic.Controls.TabPage();

		//video variables		
		public  bool videoEnabled = false;
		public ArrayList VideoTabPagesArray = new ArrayList();
		public Crownwood.Magic.Controls.TabPage  MyVideoTabPage;//s = new Crownwood.Magic.Controls.TabPage("My Video");
		public Crownwood.Magic.Controls.TabPage  MyAudioTabPage;//s = new Crownwood.Magic.Controls.TabPage("My Video");


		private System.Windows.Forms.MenuItem menuItemChat;
	
		//audio variables
		public bool audioEnabled = false;
		private WaveLib.WaveInRecorder m_Recorder;
		private WaveLib.WaveOutPlayer m_Player;

		private byte[] m_RecBuffer;
		byte [] compressBuff = new byte[100];
		public ArrayList subscribedAudioList = new ArrayList();        
		public ArrayList audioMessageList = new ArrayList();
		//private Thread audioThread;
		private AxSHDocVw.AxWebBrowser axWebBrowser1;
		private AxSHDocVw.AxWebBrowser axWebBrowser2;
		//question and Answers
		public QuestionAnswerAttendee qaAttendee = new QuestionAnswerAttendee();
		public QuestionAnswerPresenter qaPresenter = new QuestionAnswerPresenter();
		//public Crownwood.Magic.Controls.TabPage TabPageQAAttendee = new Crownwood.Magic.Controls.TabPage("Questions & Answers");
		//public Crownwood.Magic.Controls.TabPage TabPageQAPresenter = new Crownwood.Magic.Controls.TabPage("Questions & Answers");

		
		WebMeeting.Client.QAStuff.frmQAAttendee frmQAA=new WebMeeting.Client.QAStuff.frmQAAttendee();
		WebMeeting.Client.QAStuff.frmQAPresenter frmQAP=new WebMeeting.Client.QAStuff.frmQAPresenter();



		public bool QAEnabled = false;

        

		//document sharing
		public ArrayList arraydocumentSharing = new ArrayList();
		
		//webcontrolSharing
		public ArrayList arrayWebControl = new ArrayList();

		//screenCaptuing
		public Crownwood.Magic.Controls.TabPage tabPageScreenCapture =	new Crownwood.Magic.Controls.TabPage();
		bool screenCaptureEnabled = false;
		public ScreenCapture.ScreenControl screencaptureControl = new ScreenCapture.ScreenControl();
		public ToolBarManager _toolBarManager ;//= new ToolBarManager(this);


		public GreetingsMessage globalGreetingsMessage;

		//Content 
		public ArrayList webPollsArray;
		public Crownwood.Magic.Controls.TabControl tabControlWebContent = new Crownwood.Magic.Controls.TabControl();

		public Crownwood.Magic.Controls.TabPage tabPageWebContent = new Crownwood.Magic.Controls.TabPage("Manage Content");

		//public Crownwood.Magic.Controls.TabPage tabPageWebContentPolls =new Crownwood.Magic.Controls.TabPage("Polls");
		//public Crownwood.Magic.Controls.TabPage tabPageWebContentBookmarks = new Crownwood.Magic.Controls.TabPage("Websites");
		//public Crownwood.Magic.Controls.TabPage tabPageWebPresentations = new Crownwood.Magic.Controls.TabPage("Slideshow");
		//public Crownwood.Magic.Controls.TabPage tabPageWebFiles = new Crownwood.Magic.Controls.TabPage("Files");
		//public Crownwood.Magic.Controls.TabPage tabPageWebEvaluation = new Crownwood.Magic.Controls.TabPage("Evaluations");
		//public Crownwood.Magic.Controls.TabPage tabPageWebInfo = new Crownwood.Magic.Controls.TabPage("Information");
		//ManageContentInfo
		//public WebMeeting.Client.ManageContentBookmarks manageContentBookmarks = new ManageContentBookmarks();
		//public WebMeeting.Client.ManageContentWebEvaluations manageContentEvaluations = new ManageContentWebEvaluations();
		//public WebMeeting.Client.ManageContentWebFiles manageContentWebFiles = new ManageContentWebFiles();
		//public WebMeeting.Client.ManageContentWebPolls manageContentWebPolls = new ManageContentWebPolls();
		public WebMeeting.Client.ManageContentWebPresentations manageContentPresentations = new ManageContentWebPresentations();		
		//public WebMeeting.Client.ManageContentInfo manageContentInformtaion = new ManageContentInfo();		
               	
		public StartPage startPageControl = new StartPage();
		public Crownwood.Magic.Controls.TabPage startPage = new Crownwood.Magic.Controls.TabPage("Welcome");
		public Crownwood.Magic.Controls.TabPage nWelcomePage = new Crownwood.Magic.Controls.TabPage("Welcome");


		
		//WebPResentations
		public ArrayList WebPresentationsArray;
		//WebFiles
		public ArrayList webFiles;
		//WebEvaluations
		public ArrayList WebEvaluationsArray;
		public ArrayList WebBookMarksArray;

		//statusbar
		public string statusBarString;

		public ArrayList evaluationWindowsOfAttendee = new ArrayList();

		//Polling
		public ArrayList arrayPolling = new ArrayList();

		public Crownwood.Magic.Controls.TabPage tabPollingGlobal =	new Crownwood.Magic.Controls.TabPage("Polls");
		public Crownwood.Magic.Controls.TabControl pollingTabControl = new Crownwood.Magic.Controls.TabControl();

		

		//public ArrayList avaiLablePollMessages = new ArrayList();

		#endregion

		#region Userdeclared variabled
		
        

		public NiceMenu myNiceMenu;

		public static bool RunOnceListAnswer=true;
		private System.Windows.Forms.ImageList mainMenuImageList;
        
		WebMeeting.CustomUIControls.TaskbarNotifier notifier = new WebMeeting.CustomUIControls.TaskbarNotifier();
	
		public ArrayList chatWindowsArray = new ArrayList();
	
		
		
		public ArrayList chatWindowsToCreate  = new ArrayList();

		//bool bAudioCreated = false;
		
		private NotesControl notesControl;
		//private ArrayList documentSharingControls=new ArrayList();
		//bool bAudioListen = false;	
		ArrayList threadList=new ArrayList();
		private System.Windows.Forms.MenuItem menuItemSaveWhiteboard;
		private System.Windows.Forms.MenuItem menuItem24;
		private NSPAControls.NSButton btn_Audio;
		private NSPAControls.NSButton btn_Whiteboard;
		private NSPAControls.NSButton btn_Appshare;
		private NSPAControls.NSButton btn_Exit;
		private System.Windows.Forms.MenuItem menuOpenRecorder;
		private System.Windows.Forms.ImageList listViewImageList;
		private NSPAControls.NSButton btn_Docshare;
		private System.Windows.Forms.Panel panelMain;
		public NJFLib.Controls.CollapsibleSplitter splitter1;
		private System.Windows.Forms.Splitter splitter2;
		public TabControlEx tabBody;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.MenuItem menuFileSaveAsNotes;
		private System.Windows.Forms.ImageList contextMenuImageList;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel toolbarButtonsPanel;
		private System.Windows.Forms.Panel panel2;
		private NSPAControls.NSButton btn_Managecontent;
		private System.Windows.Forms.ImageList imageList1;
		private NSPAControls.NSButton btn_Video;
		private System.Windows.Forms.MenuItem menuSavePoll;
		public System.Windows.Forms.Panel panelRecordingToolbar;
		private System.Windows.Forms.Button recordingStopButton;
		private System.Windows.Forms.Button recordingRecordButton;
		private System.Windows.Forms.Button recordingPauseButton;
		public NSPAControls.NSButton toolbarBtnDesktopSharingDisable;
		public WebMeeting.OAKControls.OAKListView listParticipents;
		private System.Windows.Forms.ColumnHeader Video;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.ColumnHeader conn;
		//private System.Windows.Forms.ColumnHeader Name;
		private System.Windows.Forms.ColumnHeader Mood;
		private System.Windows.Forms.ColumnHeader color;
		public NSPAControls.NSButton toolbarBtnDesktopSharing;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private NSPAControls.NSButton btn_Snapshot;
		public System.Windows.Forms.Panel toolbarPanel;
		private System.Windows.Forms.Button btnClearWhiteBoard;
		private System.Windows.Forms.Button btnArrow;
		private System.Windows.Forms.Button btnColor;
		private System.Windows.Forms.Button btnFont;
		private System.Windows.Forms.Button btnNone;
		private System.Windows.Forms.Button btnPencil;
		private System.Windows.Forms.Button btnEraser;
		private System.Windows.Forms.Button btnBrush;
		private System.Windows.Forms.Button btnRectangle;
		private System.Windows.Forms.Button btnText;
		private System.Windows.Forms.Button btnLine;
		private System.Windows.Forms.Button btnCircle;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Panel panelBody;
		public DotNetStatusBar _statusBar;
		#endregion
    		
		FlashMenuHook objHook = new FlashMenuHook();
		public int nLastSelectedIndex = 0;
		private ToolTip m_wndToolTip = new ToolTip();
		public ArrayList recentlyUsedPresentationsArray = new ArrayList();
		Crownwood.Magic.Controls.TabPage tabPage1 =	new Crownwood.Magic.Controls.TabPage();
		
		
		
		# endregion
		
		# region public class ClientProfileWrapper
		public class ClientProfileWrapper
		{
			public int nUniqueID; 
			public Crownwood.Magic.Controls.TabPage tabPage;
			public ClientProfileWrapper()
			{			
				nUniqueID = -1;
				tabPage = null;
			}			

		};
		# endregion 

		# region public static ClientUI getInstance()
		public static ClientUI getInstance()
		{
			
			return thisInstance;
		}
		# endregion 
				
		# region Distructor
		~ClientUI(){}
		public ClientUI(int a)
		{
			
			try
			{
				//notesControl=new NotesControl();
				this.BackColor=Info.getInstance().backColor;
				tabBody.BackColor=Info.getInstance().backColor;
				panelBody.BackColor=Info.getInstance().backColor;
				tabControl1.BackColor=Info.getInstance().ParticipantBackColor;
				tabControlWebContent.BackColor = Info.getInstance().backColor;
				pollingTabControl.BackColor = Info.getInstance().backColor;
				rightPanel.BackColor=Info.getInstance().backColor;
				
				panelQuickLaunch.BackColor = System.Drawing.Color.FromArgb(209,194,197);
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 563",exp,null,false);
				//ShowExceptionMessage("Constructor of Application GUI has encountered an exception. " + ee.Message);
			}			
		}

		# endregion

		# region ReadConfigFiles() Method
		public void ReadConfigFiles()
		{
			try
			{
				if(System.Configuration.ConfigurationSettings.AppSettings["ExceptionBehaviour"]=="show")
				{
					bIsExceptionShow=true;
				}
				else if(System.Configuration.ConfigurationSettings.AppSettings["ExceptionBehaviour"]=="log")
				{
					bIsExceptionShow=false;
				}
			
			}
			catch(Exception exp)
			{
			
			}
				
		}
		#endregion

		#region Default Constructor ClientUI()
		public ClientUI()
		{
			InitializeComponent();
			
			MM_Controller=new WebMeeting.Client.Minuts_Meeting.Minuts_Controller();
			db=new WebMeeting.Client.ClientDataBase.DatabaseAccess("WebMeeting_Client.dll");
			fcu=new FtpClient(ConfigurationSettings.AppSettings["FtpServer"].ToString(),ConfigurationSettings.AppSettings["FtpServer_Username"].ToString(),ConfigurationSettings.AppSettings["FtpServer_Password"].ToString(),2000,21);
			pb=new WebMeeting.Client.Minuts_Meeting.Progressbar();



			
			try
			{
				//ff = new SaveFileDialog ();
				//this.Text = ;
				notesControl=new NotesControl();
				#region set backcolor of tab controls
				this.BackColor=Info.getInstance().backColor;
				tabBody.BackColor=Info.getInstance().backColor;
				panelBody.BackColor=Info.getInstance().backColor;
				tabControl1.BackColor=Info.getInstance().ParticipantBackColor;
				tabControlWebContent.BackColor = Info.getInstance().backColor;
				pollingTabControl.BackColor = Info.getInstance().backColor;
				rightPanel.BackColor=Info.getInstance().backColor;
				chatTabControl.BackColor=Info.getInstance().backColor;
				panelQuickLaunch.BackColor = System.Drawing.Color.FromArgb(209,194,197);
				#endregion
				#region set height
				tabPageChat.Height=200;
				chatTabControl.Height=308;
				tabPageNotes.ImageIndex = 1;
				tabPageNotes.Height=200;
				tabBody.Height = splitter1.Height  ;
				tabPageChat.ImageIndex = 0;
				#endregion
				#region set status bar 
				_statusBar = new DotNetStatusBar();
				_statusBar.BringToFront();
				_statusBar.Parent = this;
				this._statusBar.Name = "statusBar1";
				#endregion
				#region Set Menu.
				myNiceMenu = new NiceMenu();
				//myNiceMenu.AddMenuImage = false;
				myNiceMenu.MenuImages = mainMenuImageList;
				myNiceMenu.UpdateMenu(this.mainMenu1, new NiceMenuClickEvent(mnuGestioneEventoClick));
				

				#endregion
				#region get info and network instances
				info = Info.getInstance();
				info.ConferenceID="1";
				thisInstance = this;
				network= NetworkManager.getInstance();
				network.ChildWindowCreationEvent +=new MakeChildWindow(network_ChildWindowCreationEvent);
				#endregion
				#region add Participants tabs
				tabPage1.Title = "Participants";
				tabPage1.Control = listParticipents;			
				tabControl1.TabPages.Add(tabPage1);
			
				tabControl1.TabPages.Add(tabPageFind);				
				tabPageFind.Control=objFindParticipetsControl;
				#endregion
				#region set instances of netword and this(clientUI)
				network.pClient = this;				
				#endregion
				#region set chat controls tabs
				tabPageChat.Title = "Chat";
				tabPageNotes.Title="Notes";
				chatControl1 = chatControl.getInstance();
				
				tabPageChat.Control = chatControl1;
				chatControl1.thisWindowTabPage = tabPageChat;				
				
				tabPageNotes.Control=this.notesControl;
				chatTabControl.TabPages.Add(tabPageChat);
				chatTabControl.TabPages.Add(tabPageNotes);
				chatTabControl.ClosePressed+=new System.EventHandler(this.Close_ChatPanel);
				#endregion				
				#region white board	settind
				tabPageInfo.Title = "WhiteBoard";
				tabPageInfo.ImageList=this.imageList1;
				tabPageInfo.ImageIndex=4;
				whiteBoard = new WhiteboardControl(); //per2
				tabPageInfo.Control = whiteBoard; //per2
				tabPageInfo.Visible=false; //per2
				this.whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.None;	//per2			
				#endregion
				#region set visibility
				tabPageDesktopSharing.Visible = false;				
				listParticipents.Visible = true;
				#endregion
				#region screen capturing
				tabPageScreenCapture.Control = screencaptureControl;
				screencaptureControl.ChangeState(true);
				#endregion
				#region listParticipents settings
				listParticipents.SmallImageList = listViewImageList;
				listParticipents.GridLines = false;
				#endregion


				#region set controls to tabpages
//
				//TabPageQAAttendee.Control = qaAttendee;
				//TabPageQAPresenter.Control = qaPresenter;
				
				frmQAA.Controls.Add(qaAttendee);
				frmQAP.Controls.Add(qaPresenter);
				
				//tabPageWebContentPolls.Control = manageContentWebPolls;
				//tabPageWebContentBookmarks.Control = manageContentBookmarks;
				//tabPageWebFiles.Control = manageContentWebFiles;
				//tabPageWebEvaluation.Control = manageContentEvaluations;
				//tabPageWebPresentations.Control = manageContentPresentations;
				//tabPageWebInfo.Control = manageContentInformtaion;
				
				#endregion
				
				
				#region add tab pages in tabControlWebContent (Manage Content) and setting
				//tabControlWebContent.TabPages.Add(tabPageWebInfo);
				//tabControlWebContent.TabPages.Add(tabPageWebPresentations);
				//tabControlWebContent.TabPages.Add(tabPageWebContentPolls);
				//tabControlWebContent.TabPages.Add(tabPageWebContentBookmarks);
				//tabControlWebContent.TabPages.Add(tabPageWebFiles);
				//tabControlWebContent.TabPages.Add(tabPageWebEvaluation);
				
				tabPageWebContent.Title = "Manage Content";
				tabBody.OnSelectionChangingEx +=new WebMeeting.Client.TabControlEx.DelegateSelectionChanged(tabBody_OnSelectionChangingEx);
				splitter1.SplitterThumbBackColoor = Color.DarkBlue;
				tabPageWebContent.Control = manageContentPresentations;
				whiteBoard.pictureBox1.SizeMode = PictureBoxSizeMode.Normal; //per2
				SynchWindowProc = new FnSynchWindow(SynchWindow);
				#endregion
				#region set tooltips for drawing tools
				ToolTip drawingToolTip = new ToolTip();

				// Set up the delays for the ToolTip.
				drawingToolTip.AutoPopDelay = 1000;
				drawingToolTip.InitialDelay = 1000;
				drawingToolTip.ReshowDelay = 500;
				// Force the ToolTip text to be displayed whether or not the form is active.
				drawingToolTip.ShowAlways = true;

				drawingToolTip.SetToolTip(this.btnCircle, "Elipse");
				drawingToolTip.SetToolTip(this.btnArrow, "UnSelect");
				drawingToolTip.SetToolTip(this.btnBrush, "Brush");
				drawingToolTip.SetToolTip(this.btnClearWhiteBoard, "Clear All");
				drawingToolTip.SetToolTip(this.btnEraser, "Eraser");
				drawingToolTip.SetToolTip(this.btnFont, "Font");
				drawingToolTip.SetToolTip(this.btnLine, "Line");
				drawingToolTip.SetToolTip(this.btnNone, "Pointing Tool");
				drawingToolTip.SetToolTip(this.btnPencil, "Pencil");
				drawingToolTip.SetToolTip(this.btnPlaceHolderArrow, "Place Holder Arrow");
				drawingToolTip.SetToolTip(this.btnRectangle, "Rectangle");
				drawingToolTip.SetToolTip(this.btnText, "Text");
				drawingToolTip.SetToolTip(this.btnColor,"Color Chooser");
				drawingToolTip.SetToolTip(this.button4,"Line Thickness");
				drawingToolTip.SetToolTip(this.splitter1,"Hide/Show Right Panels");
				#endregion
				#region set listParticipents(Participents) coloumns width
				this.listParticipents.Columns[1].Width=42; //Color
				this.listParticipents.Columns[2].Width=17; //Speed
				this.listParticipents.Columns[3].Width=0; //Mood
				this.listParticipents.Columns[4].Width=45;
				this.listParticipents.Columns[5].Width=20;
				#endregion

				/*
				 * get the configuration setting relevant to exception handling
				 * 
				 * */
				ReadConfigFiles();

				ChangeResolution(true);
				







			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 750",exp,null,false);			
				//ShowExceptionMessage("Constructor of Application GUI has encountered an exception. " + ee.Message);
			}			
		}
		#endregion


		#region ChnageResolution(bool startup,bool true)
	
		/// <summary>
		/// changes the resolution and the icons stuff on startup 
		/// </summary>
		/// <param name=" startup">called on the start up, if its false then it is called from the end </param>
		/// <returns></returns>
		public bool ChangeResolution(bool startup)
		{
			return false;
			try
			{
				if(startup)
				{
					# region resoltion stuff By Zaeem on Startup
					WebMeeting.Client.Resolution.ResolutionController res=WebMeeting.Client.Resolution.ResolutionController.getInstance();
					//if the resolution is already this then no need to do this Stuff
					if( res.tempHeight==768 && res.tempWidth ==1024)
					{
					MessageBox.Show("temp resolution is already 1024*768");
					
					}
					else
					{
						// If there is any record in this data base 
						//it means the application was not succeesfully exited through proper channel and those settings have to be retored with overwriting extra settings
						//As whenever application exits through proper channel then it will delete the Entries in this table 
						//to show that Application explicitly chaged the resolution settings 
						//
						if(db.GetNumberofRecordsInTable("Resolution")>0)
						{
							MessageBox.Show("records in Db are > 0 ");
						
						}
							// No previous settings to shift to , So new seeting s have to be stored in the database. 
						else
						{
							// Before this block it will enter the current resolution in the database 
							db.EnterResolutiontoDB(1, res.tempWidth, res.tempHeight);
							res.CSaveIcons();
							res.ChangeResolution();
						}
					}
					return true;
					#endregion 
				}//if startup

				// On application exit
				else
					{

					# region resoltion stuff By Zaeem
					WebMeeting.Client.Resolution.ResolutionController res=WebMeeting.Client.Resolution.ResolutionController.getInstance();
					
					if(db.GetNumberofRecordsInTable("Resolution")>0)
					{
					//if there is any record in db then act accrding to that 

						res.RetainResolution(db.GetwidthfromDB(),db.GetheightfromDB());
					}
					else
					{
						res.RetainResolution();
					}
					
					
					res.CRestoreIcons();
					// After restoring the db settings delete records fromthe database;
					db.DeleteRecordsfromTable("Resolution");
					#endregion 
					return true;
					}




			}
			catch(Exception exp)
			
			{
				MeetingAlerts alert=new MeetingAlerts();
				alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Optimised resolution scheme could not be applied :" + exp.Message,true,false);
				alert.Dispose();
						
				return false;
			
			}

		}
	#endregion 

		# region void ShowExceptionMessage 2 Methods
		public void ShowExceptionMessage(string msg)
		{
			try
			{
				MeetingAlerts alert=new MeetingAlerts();
				alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"" + msg,true,false);			
			}
			catch(Exception exp)
			{
				exp=exp;
			}
			//MessageBox.Show("An error has occured while performing some action. Please contact support.\nError Details: " + msg,"WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
		}

	
		/*
		 * strMessagePrefixedModuleName---message that is prefixed with module name( for our use caz it has some technical info)
		 * bShowDialog---exceptions that are final will always send true for displaying proper message
		 * exMethodException---exception object that calling method will send
		 * strClientMessage---message that will show to client
		 * */
		public void ShowExceptionMessage(string strMessagePrefixedModuleName, Exception exMethodException,string strClientMessage ,bool isFinalException)
		{			
				
			try
			{
				if(isFinalException==true)/*show confirmed exceptions for end-user information like low space,duplicate file & connection closed etc */
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"An error has occured while performing some action. Please contact support.\nError Details:" +exMethodException.Message +exMethodException.StackTrace.ToString() + strClientMessage,true,false);
					nExceptionCounter++;
					ExceptionLog("----------------------- Exception no ::: " +nExceptionCounter.ToString()+ " '" +strMessagePrefixedModuleName + "'  exception string" +exMethodException.ToString()+"  :::::Message :::: " +exMethodException.Message + " ::::Stack Trace::::"+exMethodException.StackTrace+":::: Source :::::"+exMethodException.Source+":::: TargetSite ::::"+exMethodException.TargetSite);
					return;
				}
				if(this.bIsExceptionShow==true)/*show exception messages globaly*/
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"An error has occured while performing some action. Please contact support.\nError Details: " + strMessagePrefixedModuleName + " " + exMethodException.ToString()+exMethodException.Message +exMethodException.StackTrace.ToString(),true,false);
				}
				else/*false part ---------log*/  
				{
					nExceptionCounter++;
					if(exMethodException!=null)
					{
						ExceptionLog("----------------------- Exception no ::: " +nExceptionCounter.ToString()+ " '" +strMessagePrefixedModuleName + "'  exception string" +exMethodException.ToString()+"  :::::Message :::: " +exMethodException.Message + " ::::Stack Trace::::"+exMethodException.StackTrace+":::: Source :::::"+exMethodException.Source+":::: TargetSite ::::"+exMethodException.TargetSite);
					}
					else
					{
						ExceptionLog("----------------------- Exception no ::: " +nExceptionCounter.ToString()+ " '" +strMessagePrefixedModuleName + "'");
					}
				}
				//MessageBox.Show("An error has occured while performing some action. Please contact support.\nError Details: " + msg,"WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			catch(Exception ex)
			{
				ex=ex;
			}
		}


		#endregion 

		# region ExceptionLog( Method
		
		public void ExceptionLog(string strValue)
		{
			//File exceptionWrite;	
//			try
//			{		
//			
				ExceptionLogWrite.WriteLog(strValue);
//				string strFileName=DateTime.Today.Date.ToShortDateString();
//				strFileName=strFileName.Replace(@"/","-");//remove / from date
//				strFileName=Application.StartupPath+@"/log/" + strFileName+".log";
//				
//				/*
//				 * Initializes a new instance of the StreamWriter class for the specified file on the specified path, using the default encoding and buffer size. 
//				 * If the file exists, 
//				 * it can be either overwritten or appended to.
//				 * If the file does not exist, this constructor creates a new file.
//				 * */
//
//				//if(File.Exists(strFileName))
//				//	{
//				//	exceptionWrite=new StreamWriter(strFileName,true);//append already file
//				//	}
//				//else
//				//	{
//				//	exceptionWrite=new StreamWriter(strFileName,false);//create new file
//				//}
//				//exceptionWrite.WriteLine(strValue);
//				//exceptionWrite.WriteLine("-------------------------------  end of Exception-------------------------------");
//				//exceptionWrite.Flush();
//				//exceptionWrite.Close();
//				
//				//This code example opens the file in Output mode;
//				//any process can read or write to file.
//				//exceptionWrite=new 
//			
//
//
//
//				/**********************************************************************/
//				//								Zaeem Block.
//				/**********************************************************************/
//
//				FileStream fs;
//				StreamWriter exceptionWrite;
//				//strFileName ==> full path of the file.
//				FileInfo fis=new FileInfo(strFileName);
//
//
//				//1 )FileMode.Append
//				//It opens the file if it exists and seeks to the end of the file,
//				//or creates a new file.
//				//FileMode.Append can only be used in conjunction with FileAccess.Write.
//				
//				//2 )FileAccess.ReadWrite
//				//Represents the access to the file
//				
//				//3 )FileShare.ReadWrite
//				//It allows subsequent opening of the file for reading or writing. 
//				//If this flag is not specified, any request to open the file for reading or writing (by this process or another process) will fail until the file is closed.  However, additional permissions will still be needed to access the file, even if this flag is specified. 
//
//				fs=fis.Open(FileMode.Append,FileAccess.Write,FileShare.Write);
//				exceptionWrite=new StreamWriter(fs);
//				//exceptionWrite=new StreamWriter(strFileName,true);//append already file
//				//strValue contains the exception message.
//				exceptionWrite.WriteLine(strValue);
//				exceptionWrite.WriteLine("-------------------------------  End Exception-------------------------------");
//				exceptionWrite.WriteLine();
//				exceptionWrite.Flush();
//				exceptionWrite.Close();
//				
//				//Reference Articles
//				//http://aspalliance.com/920#Page5
//				//Msdn Help.
//			
//
//				/**********************************************************************/
//				//								End Zaeem Block.
//				/**********************************************************************/


//			}
//			catch(Exception exp)
//			{
//				exp=exp;
//				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 866",exp,"Couuldn't set the Log file properly        "+exp.Message.ToString(),false);			
//				//MessageBox.Show("Log file could not be set up properly");
//			}
		}
		#endregion 

		#region Clean up any resources being used.
		
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion 
        
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ClientUI));
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemSave = new System.Windows.Forms.MenuItem();
			this.menuItemSaveDocument = new System.Windows.Forms.MenuItem();
			this.menuItemSavePollQuestionnaire = new System.Windows.Forms.MenuItem();
			this.menuItemSavePollResult = new System.Windows.Forms.MenuItem();
			this.menuItemSaveChat = new System.Windows.Forms.MenuItem();
			this.menuItemSaveNotes = new System.Windows.Forms.MenuItem();
			this.menuItemSaveAs = new System.Windows.Forms.MenuItem();
			this.menuItemSaveAsDocument = new System.Windows.Forms.MenuItem();
			this.menuItemSaveAsPollQuestionnaire = new System.Windows.Forms.MenuItem();
			this.menuItemSaveAsPollResult = new System.Windows.Forms.MenuItem();
			this.menuItemSaveAsChat = new System.Windows.Forms.MenuItem();
			this.menuItemSaveAsNotes = new System.Windows.Forms.MenuItem();
			this.menuItemSaveAll = new System.Windows.Forms.MenuItem();
			this.menuItemPrint = new System.Windows.Forms.MenuItem();
			this.menuItemPrintDocument = new System.Windows.Forms.MenuItem();
			this.menuItemPrintPollQuestionnaire = new System.Windows.Forms.MenuItem();
			this.menuItemPrintPollResult = new System.Windows.Forms.MenuItem();
			this.menuItemPrintChat = new System.Windows.Forms.MenuItem();
			this.menuItemPrintNotes = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemUndo = new System.Windows.Forms.MenuItem();
			this.menuItemRedo = new System.Windows.Forms.MenuItem();
			this.menuItemAddPage = new System.Windows.Forms.MenuItem();
			this.menuItemPastePage = new System.Windows.Forms.MenuItem();
			this.menuItemClear = new System.Windows.Forms.MenuItem();
			this.menuItemClearAllAnnotations = new System.Windows.Forms.MenuItem();
			this.menuItemClearMyPointer = new System.Windows.Forms.MenuItem();
			this.menuItemClearAllPointers = new System.Windows.Forms.MenuItem();
			this.menuItemFont = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItemShareWhiteboard = new System.Windows.Forms.MenuItem();
			this.menuItemShareWebContent = new System.Windows.Forms.MenuItem();
			this.menuItemShareApplication = new System.Windows.Forms.MenuItem();
			this.menuItemShareWebBrowser = new System.Windows.Forms.MenuItem();
			this.menuItemShareDesktop = new System.Windows.Forms.MenuItem();
			this.menuItemShareRemoteComputer = new System.Windows.Forms.MenuItem();
			this.menuItemView = new System.Windows.Forms.MenuItem();
			this.menuItemViewFullScreen = new System.Windows.Forms.MenuItem();
			this.menuItemViewThumbNails = new System.Windows.Forms.MenuItem();
			this.menuItemViewZoomIn = new System.Windows.Forms.MenuItem();
			this.menuItemZoomOut = new System.Windows.Forms.MenuItem();
			this.menuItemViewZoomBy = new System.Windows.Forms.MenuItem();
			this.menuItemViewFitInViewer = new System.Windows.Forms.MenuItem();
			this.menuItemFirInWidth = new System.Windows.Forms.MenuItem();
			this.menuItemViewAutomaticallyAdvancePages = new System.Windows.Forms.MenuItem();
			this.menuItemViewSynchronizeAll = new System.Windows.Forms.MenuItem();
			this.menuItemAudio = new System.Windows.Forms.MenuItem();
			this.menuItemTools = new System.Windows.Forms.MenuItem();
			this.menuItemToolsEnableContinuation = new System.Windows.Forms.MenuItem();
			this.menuItemToolsInternetPhone = new System.Windows.Forms.MenuItem();
			this.menuItemToolsOpenRecorder = new System.Windows.Forms.MenuItem();
			this.menuItemToolsRecorderSettings = new System.Windows.Forms.MenuItem();
			this.menuItemToolsRecordAudio = new System.Windows.Forms.MenuItem();
			this.menuItemInviteOthers = new System.Windows.Forms.MenuItem();
			this.menuItemHelp = new System.Windows.Forms.MenuItem();
			this.menuItemHelpLiveChat = new System.Windows.Forms.MenuItem();
			this.menuItemHelpMeetingManager = new System.Windows.Forms.MenuItem();
			this.menuItemHelpAboutMeetingManager = new System.Windows.Forms.MenuItem();
			this.menuItemVerifyRichMedia = new System.Windows.Forms.MenuItem();
			this.menuItemHelpMeetingDetails = new System.Windows.Forms.MenuItem();
			this.menuItemHelpTeleconferenceDetails = new System.Windows.Forms.MenuItem();
			this.whiteBoardtoolbarImageList = new System.Windows.Forms.ImageList(this.components);
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuSavePoll = new System.Windows.Forms.MenuItem();
			this.menuItemChat = new System.Windows.Forms.MenuItem();
			this.menuItem24 = new System.Windows.Forms.MenuItem();
			this.menuFileSaveAsNotes = new System.Windows.Forms.MenuItem();
			this.menuItemSaveWhiteboard = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem32 = new System.Windows.Forms.MenuItem();
			this.mnuVideo = new System.Windows.Forms.MenuItem();
			this.mnuAudio = new System.Windows.Forms.MenuItem();
			this.mnuWhiteboard = new System.Windows.Forms.MenuItem();
			this.mnuApplication = new System.Windows.Forms.MenuItem();
			this.mnuWebBrowser = new System.Windows.Forms.MenuItem();
			this.mnupDocument = new System.Windows.Forms.MenuItem();
			this.menuItem51 = new System.Windows.Forms.MenuItem();
			this.menuOpenRecorder = new System.Windows.Forms.MenuItem();
			this.mnuInvite = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.mnuSynchronize = new System.Windows.Forms.MenuItem();
			this.mnuSynchronizeD = new System.Windows.Forms.MenuItem();
			this.mnuQA = new System.Windows.Forms.MenuItem();
			this.mnuPoll = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.mnu_Minuts = new System.Windows.Forms.MenuItem();
			this.mnu_MM_Wht = new System.Windows.Forms.MenuItem();
			this.mnu_MM_App = new System.Windows.Forms.MenuItem();
			this.mnu_MM_Web = new System.Windows.Forms.MenuItem();
			this.menuItem59 = new System.Windows.Forms.MenuItem();
			this.mnuAboutCompassnavMeeting = new System.Windows.Forms.MenuItem();
			this.mnumdetails = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.listParticipents = new WebMeeting.OAKControls.OAKListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.conn = new System.Windows.Forms.ColumnHeader();
			this.Mood = new System.Windows.Forms.ColumnHeader();
			this.color = new System.Windows.Forms.ColumnHeader();
			this.Video = new System.Windows.Forms.ColumnHeader();
			this.panelQuickLaunch = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.toolbarBtnDesktopSharingDisable = new NSPAControls.NSButton();
			this.toolbarBtnDesktopSharing = new NSPAControls.NSButton();
			this.toolbarButtonsPanel = new System.Windows.Forms.Panel();
			this.btn_QA = new NSPAControls.NSButton();
			this.btn_Poll = new NSPAControls.NSButton();
			this.btn_Record = new NSPAControls.NSButton();
			this.btn_Whiteboard = new NSPAControls.NSButton();
			this.btn_Chat = new NSPAControls.NSButton();
			this.btn_Managecontent = new NSPAControls.NSButton();
			this.btn_Video = new NSPAControls.NSButton();
			this.btn_Docshare = new NSPAControls.NSButton();
			this.btn_Audio = new NSPAControls.NSButton();
			this.btn_Appshare = new NSPAControls.NSButton();
			this.btn_Webshare = new NSPAControls.NSButton();
			this.btn_Invite = new NSPAControls.NSButton();
			this.btn_Snapshot = new NSPAControls.NSButton();
			this.btn_Exit = new NSPAControls.NSButton();
			this.rightPanel = new System.Windows.Forms.Panel();
			this.splitter3 = new System.Windows.Forms.Splitter();
			this.panalAdvertiseBrowser = new System.Windows.Forms.Panel();
			this.axWebBrowser2 = new AxSHDocVw.AxWebBrowser();
			this.panel1 = new System.Windows.Forms.Panel();
			this.chatTabControl = new Crownwood.Magic.Controls.TabControl();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.panel3 = new System.Windows.Forms.Panel();
			this.btnRestoreUp = new System.Windows.Forms.Button();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.tabControl1 = new Crownwood.Magic.Controls.TabControl();
			this.tabPagesMenu = new System.Windows.Forms.ContextMenu();
			this.menu_Close = new System.Windows.Forms.MenuItem();
			this.mainMenuImageList = new System.Windows.Forms.ImageList(this.components);
			this.listViewImageList = new System.Windows.Forms.ImageList(this.components);
			this.panelMain = new System.Windows.Forms.Panel();
			this.splitter1 = new NJFLib.Controls.CollapsibleSplitter();
			this.panelBody = new System.Windows.Forms.Panel();
			this.tabBody = new WebMeeting.Client.TabControlEx(this.components);
			this.panelRecordingToolbar = new System.Windows.Forms.Panel();
			this.recordingStopButton = new System.Windows.Forms.Button();
			this.recordingRecordButton = new System.Windows.Forms.Button();
			this.recordingPauseButton = new System.Windows.Forms.Button();
			this.panelSplitter = new System.Windows.Forms.Panel();
			this.toolbarPanel = new System.Windows.Forms.Panel();
			this.btnPlaceHolderArrow = new System.Windows.Forms.Button();
			this.btnPrev = new NSPAControls.NSButton();
			this.btnNext = new NSPAControls.NSButton();
			this.btnClearWhiteBoard = new System.Windows.Forms.Button();
			this.btnArrow = new System.Windows.Forms.Button();
			this.btnColor = new System.Windows.Forms.Button();
			this.btnFont = new System.Windows.Forms.Button();
			this.btnNone = new System.Windows.Forms.Button();
			this.btnPencil = new System.Windows.Forms.Button();
			this.btnEraser = new System.Windows.Forms.Button();
			this.btnBrush = new System.Windows.Forms.Button();
			this.btnRectangle = new System.Windows.Forms.Button();
			this.btnText = new System.Windows.Forms.Button();
			this.btnLine = new System.Windows.Forms.Button();
			this.btnCircle = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.contextMenuImageList = new System.Windows.Forms.ImageList(this.components);
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			this.imageList2 = new System.Windows.Forms.ImageList(this.components);
			this.panelQuickLaunch.SuspendLayout();
			this.panel2.SuspendLayout();
			this.toolbarButtonsPanel.SuspendLayout();
			this.rightPanel.SuspendLayout();
			this.panalAdvertiseBrowser.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser2)).BeginInit();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.panelMain.SuspendLayout();
			this.panelBody.SuspendLayout();
			this.tabBody.SuspendLayout();
			this.panelRecordingToolbar.SuspendLayout();
			this.toolbarPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.SuspendLayout();
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = -1;
			this.menuItemFile.Text = "";
			// 
			// menuItemSave
			// 
			this.menuItemSave.Index = -1;
			this.menuItemSave.Text = "";
			// 
			// menuItemSaveDocument
			// 
			this.menuItemSaveDocument.Index = -1;
			this.menuItemSaveDocument.Text = "";
			// 
			// menuItemSavePollQuestionnaire
			// 
			this.menuItemSavePollQuestionnaire.Index = -1;
			this.menuItemSavePollQuestionnaire.Text = "";
			// 
			// menuItemSavePollResult
			// 
			this.menuItemSavePollResult.Index = -1;
			this.menuItemSavePollResult.Text = "";
			// 
			// menuItemSaveChat
			// 
			this.menuItemSaveChat.Index = -1;
			this.menuItemSaveChat.Text = "";
			// 
			// menuItemSaveNotes
			// 
			this.menuItemSaveNotes.Index = -1;
			this.menuItemSaveNotes.Text = "";
			// 
			// menuItemSaveAs
			// 
			this.menuItemSaveAs.Index = -1;
			this.menuItemSaveAs.Text = "";
			// 
			// menuItemSaveAsDocument
			// 
			this.menuItemSaveAsDocument.Index = -1;
			this.menuItemSaveAsDocument.Text = "";
			// 
			// menuItemSaveAsPollQuestionnaire
			// 
			this.menuItemSaveAsPollQuestionnaire.Index = -1;
			this.menuItemSaveAsPollQuestionnaire.Text = "";
			// 
			// menuItemSaveAsPollResult
			// 
			this.menuItemSaveAsPollResult.Index = -1;
			this.menuItemSaveAsPollResult.Text = "";
			// 
			// menuItemSaveAsChat
			// 
			this.menuItemSaveAsChat.Index = -1;
			this.menuItemSaveAsChat.Text = "";
			// 
			// menuItemSaveAsNotes
			// 
			this.menuItemSaveAsNotes.Index = -1;
			this.menuItemSaveAsNotes.Text = "";
			// 
			// menuItemSaveAll
			// 
			this.menuItemSaveAll.Index = -1;
			this.menuItemSaveAll.Text = "";
			// 
			// menuItemPrint
			// 
			this.menuItemPrint.Index = -1;
			this.menuItemPrint.Text = "";
			// 
			// menuItemPrintDocument
			// 
			this.menuItemPrintDocument.Index = -1;
			this.menuItemPrintDocument.Text = "";
			// 
			// menuItemPrintPollQuestionnaire
			// 
			this.menuItemPrintPollQuestionnaire.Index = -1;
			this.menuItemPrintPollQuestionnaire.Text = "";
			// 
			// menuItemPrintPollResult
			// 
			this.menuItemPrintPollResult.Index = -1;
			this.menuItemPrintPollResult.Text = "";
			// 
			// menuItemPrintChat
			// 
			this.menuItemPrintChat.Index = -1;
			this.menuItemPrintChat.Text = "";
			// 
			// menuItemPrintNotes
			// 
			this.menuItemPrintNotes.Index = -1;
			this.menuItemPrintNotes.Text = "";
			// 
			// menuItemEdit
			// 
			this.menuItemEdit.Index = -1;
			this.menuItemEdit.Text = "";
			// 
			// menuItemUndo
			// 
			this.menuItemUndo.Index = -1;
			this.menuItemUndo.Text = "";
			// 
			// menuItemRedo
			// 
			this.menuItemRedo.Index = -1;
			this.menuItemRedo.Text = "";
			// 
			// menuItemAddPage
			// 
			this.menuItemAddPage.Index = -1;
			this.menuItemAddPage.Text = "";
			// 
			// menuItemPastePage
			// 
			this.menuItemPastePage.Index = -1;
			this.menuItemPastePage.Text = "";
			// 
			// menuItemClear
			// 
			this.menuItemClear.Index = -1;
			this.menuItemClear.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.menuItemClearAllAnnotations,
																						  this.menuItemClearMyPointer,
																						  this.menuItemClearAllPointers});
			this.menuItemClear.Text = "Clear";
			// 
			// menuItemClearAllAnnotations
			// 
			this.menuItemClearAllAnnotations.Index = 0;
			this.menuItemClearAllAnnotations.Text = "All Annotations";
			// 
			// menuItemClearMyPointer
			// 
			this.menuItemClearMyPointer.Index = 1;
			this.menuItemClearMyPointer.Text = "My Pointer";
			// 
			// menuItemClearAllPointers
			// 
			this.menuItemClearAllPointers.Index = 2;
			this.menuItemClearAllPointers.Text = "All Pointers";
			// 
			// menuItemFont
			// 
			this.menuItemFont.Index = -1;
			this.menuItemFont.Text = "Font";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = -1;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemShareWhiteboard,
																					  this.menuItemShareWebContent,
																					  this.menuItemShareApplication,
																					  this.menuItemShareWebBrowser,
																					  this.menuItemShareDesktop,
																					  this.menuItemShareRemoteComputer});
			this.menuItem3.Text = "Share";
			// 
			// menuItemShareWhiteboard
			// 
			this.menuItemShareWhiteboard.Index = 0;
			this.menuItemShareWhiteboard.Text = "Whiteboard";
			// 
			// menuItemShareWebContent
			// 
			this.menuItemShareWebContent.Index = 1;
			this.menuItemShareWebContent.Text = "WebContent";
			// 
			// menuItemShareApplication
			// 
			this.menuItemShareApplication.Index = 2;
			this.menuItemShareApplication.Text = "Application";
			// 
			// menuItemShareWebBrowser
			// 
			this.menuItemShareWebBrowser.Index = 3;
			this.menuItemShareWebBrowser.Text = "Web Browser";
			// 
			// menuItemShareDesktop
			// 
			this.menuItemShareDesktop.Index = 4;
			this.menuItemShareDesktop.Text = "Desktop";
			// 
			// menuItemShareRemoteComputer
			// 
			this.menuItemShareRemoteComputer.Index = 5;
			this.menuItemShareRemoteComputer.Text = "Remote Computer";
			// 
			// menuItemView
			// 
			this.menuItemView.Index = -1;
			this.menuItemView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemViewFullScreen,
																						 this.menuItemViewThumbNails,
																						 this.menuItemViewZoomIn,
																						 this.menuItemZoomOut,
																						 this.menuItemViewZoomBy,
																						 this.menuItemViewFitInViewer,
																						 this.menuItemFirInWidth,
																						 this.menuItemViewAutomaticallyAdvancePages,
																						 this.menuItemViewSynchronizeAll});
			this.menuItemView.Text = "View";
			// 
			// menuItemViewFullScreen
			// 
			this.menuItemViewFullScreen.Index = 0;
			this.menuItemViewFullScreen.Text = "Full Screen";
			// 
			// menuItemViewThumbNails
			// 
			this.menuItemViewThumbNails.Index = 1;
			this.menuItemViewThumbNails.Text = "Thumb Nails";
			// 
			// menuItemViewZoomIn
			// 
			this.menuItemViewZoomIn.Index = 2;
			this.menuItemViewZoomIn.Text = "Zoom In";
			// 
			// menuItemZoomOut
			// 
			this.menuItemZoomOut.Index = 3;
			this.menuItemZoomOut.Text = "Zoom Out";
			// 
			// menuItemViewZoomBy
			// 
			this.menuItemViewZoomBy.Index = 4;
			this.menuItemViewZoomBy.Text = "Zoom By";
			// 
			// menuItemViewFitInViewer
			// 
			this.menuItemViewFitInViewer.Index = 5;
			this.menuItemViewFitInViewer.Text = "Fit In Viewer";
			// 
			// menuItemFirInWidth
			// 
			this.menuItemFirInWidth.Index = 6;
			this.menuItemFirInWidth.Text = "Fit In Width";
			// 
			// menuItemViewAutomaticallyAdvancePages
			// 
			this.menuItemViewAutomaticallyAdvancePages.Index = 7;
			this.menuItemViewAutomaticallyAdvancePages.Text = "Automatically Advance Pages";
			// 
			// menuItemViewSynchronizeAll
			// 
			this.menuItemViewSynchronizeAll.Index = 8;
			this.menuItemViewSynchronizeAll.Text = "Synchronize For All";
			// 
			// menuItemAudio
			// 
			this.menuItemAudio.Index = -1;
			this.menuItemAudio.Text = "Audio";
			// 
			// menuItemTools
			// 
			this.menuItemTools.Index = -1;
			this.menuItemTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.menuItemToolsEnableContinuation,
																						  this.menuItemToolsInternetPhone,
																						  this.menuItemToolsOpenRecorder,
																						  this.menuItemToolsRecorderSettings,
																						  this.menuItemToolsRecordAudio,
																						  this.menuItemInviteOthers});
			this.menuItemTools.Text = "Tools";
			// 
			// menuItemToolsEnableContinuation
			// 
			this.menuItemToolsEnableContinuation.Index = 0;
			this.menuItemToolsEnableContinuation.Text = "Enable Continuation";
			// 
			// menuItemToolsInternetPhone
			// 
			this.menuItemToolsInternetPhone.Index = 1;
			this.menuItemToolsInternetPhone.Text = "Internet Phone";
			// 
			// menuItemToolsOpenRecorder
			// 
			this.menuItemToolsOpenRecorder.Index = 2;
			this.menuItemToolsOpenRecorder.Text = "Open Recorder";
			// 
			// menuItemToolsRecorderSettings
			// 
			this.menuItemToolsRecorderSettings.Index = 3;
			this.menuItemToolsRecorderSettings.Text = "Recorder Settings";
			// 
			// menuItemToolsRecordAudio
			// 
			this.menuItemToolsRecordAudio.Index = 4;
			this.menuItemToolsRecordAudio.Text = "Record Audio";
			// 
			// menuItemInviteOthers
			// 
			this.menuItemInviteOthers.Index = 5;
			this.menuItemInviteOthers.Text = "Invite Others";
			// 
			// menuItemHelp
			// 
			this.menuItemHelp.Index = -1;
			this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemHelpLiveChat,
																						 this.menuItemHelpMeetingManager,
																						 this.menuItemHelpAboutMeetingManager,
																						 this.menuItemVerifyRichMedia,
																						 this.menuItemHelpMeetingDetails,
																						 this.menuItemHelpTeleconferenceDetails});
			this.menuItemHelp.Text = "Help";
			// 
			// menuItemHelpLiveChat
			// 
			this.menuItemHelpLiveChat.Index = 0;
			this.menuItemHelpLiveChat.Text = "Live IM Chat";
			// 
			// menuItemHelpMeetingManager
			// 
			this.menuItemHelpMeetingManager.Index = 1;
			this.menuItemHelpMeetingManager.Text = "Meeting Manager Help";
			// 
			// menuItemHelpAboutMeetingManager
			// 
			this.menuItemHelpAboutMeetingManager.Index = 2;
			this.menuItemHelpAboutMeetingManager.Text = "About Meeting Manager";
			// 
			// menuItemVerifyRichMedia
			// 
			this.menuItemVerifyRichMedia.Index = 3;
			this.menuItemVerifyRichMedia.Text = "Verify Rich Media";
			// 
			// menuItemHelpMeetingDetails
			// 
			this.menuItemHelpMeetingDetails.Index = 4;
			this.menuItemHelpMeetingDetails.Text = "Meeting Details";
			// 
			// menuItemHelpTeleconferenceDetails
			// 
			this.menuItemHelpTeleconferenceDetails.Index = 5;
			this.menuItemHelpTeleconferenceDetails.Text = "Teleconference Details";
			// 
			// whiteBoardtoolbarImageList
			// 
			this.whiteBoardtoolbarImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.whiteBoardtoolbarImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("whiteBoardtoolbarImageList.ImageStream")));
			this.whiteBoardtoolbarImageList.TransparentColor = System.Drawing.Color.White;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem32,
																					  this.menuItem51,
																					  this.mnu_Minuts,
																					  this.menuItem59});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2,
																					  this.menuItem11});
			this.menuItem1.OwnerDraw = true;
			this.menuItem1.Text = "File";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuSavePoll,
																					  this.menuItemChat,
																					  this.menuFileSaveAsNotes,
																					  this.menuItemSaveWhiteboard});
			this.menuItem2.Text = "09Save";
			// 
			// menuSavePoll
			// 
			this.menuSavePoll.Index = 0;
			this.menuSavePoll.Text = "Poll";
			// 
			// menuItemChat
			// 
			this.menuItemChat.Index = 1;
			this.menuItemChat.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem24});
			this.menuItemChat.Text = "02Chat";
			// 
			// menuItem24
			// 
			this.menuItem24.Index = 0;
			this.menuItem24.Text = "Public Chat";
			// 
			// menuFileSaveAsNotes
			// 
			this.menuFileSaveAsNotes.Index = 2;
			this.menuFileSaveAsNotes.Text = "Notes";
			// 
			// menuItemSaveWhiteboard
			// 
			this.menuItemSaveWhiteboard.DefaultItem = true;
			this.menuItemSaveWhiteboard.Index = 3;
			this.menuItemSaveWhiteboard.Text = "Whiteboard";
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 1;
			this.menuItem11.Text = "Exit ";
			this.menuItem11.Click += new System.EventHandler(this.ContextMenuClickEvent);
			// 
			// menuItem32
			// 
			this.menuItem32.Index = 1;
			this.menuItem32.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuVideo,
																					   this.mnuAudio,
																					   this.mnuWhiteboard,
																					   this.mnuApplication,
																					   this.mnuWebBrowser,
																					   this.mnupDocument});
			this.menuItem32.OwnerDraw = true;
			this.menuItem32.Text = "Share";
			// 
			// mnuVideo
			// 
			this.mnuVideo.Index = 0;
			this.mnuVideo.Text = "Video";
			// 
			// mnuAudio
			// 
			this.mnuAudio.Index = 1;
			this.mnuAudio.Text = "Audio";
			// 
			// mnuWhiteboard
			// 
			this.mnuWhiteboard.Index = 2;
			this.mnuWhiteboard.Text = "Whiteboard ";
			// 
			// mnuApplication
			// 
			this.mnuApplication.Index = 3;
			this.mnuApplication.Text = "Application ";
			// 
			// mnuWebBrowser
			// 
			this.mnuWebBrowser.Index = 4;
			this.mnuWebBrowser.Text = "Web Browser";
			// 
			// mnupDocument
			// 
			this.mnupDocument.Index = 5;
			this.mnupDocument.Text = "Presentation or Document";
			// 
			// menuItem51
			// 
			this.menuItem51.Index = 2;
			this.menuItem51.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuOpenRecorder,
																					   this.mnuInvite,
																					   this.menuItem8,
																					   this.mnuSynchronize,
																					   this.mnuSynchronizeD,
																					   this.mnuQA,
																					   this.mnuPoll,
																					   this.menuItem5});
			this.menuItem51.OwnerDraw = true;
			this.menuItem51.Text = "Tools";
			// 
			// menuOpenRecorder
			// 
			this.menuOpenRecorder.Index = 0;
			this.menuOpenRecorder.Text = "Record";
			this.menuOpenRecorder.Click += new System.EventHandler(this.btn_Record_Click);
			// 
			// mnuInvite
			// 
			this.mnuInvite.Index = 1;
			this.mnuInvite.Text = "Invite";
			this.mnuInvite.Click += new System.EventHandler(this.btn_Invite_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 2;
			this.menuItem8.Text = "Snapshot";
			// 
			// mnuSynchronize
			// 
			this.mnuSynchronize.Enabled = false;
			this.mnuSynchronize.Index = 3;
			this.mnuSynchronize.Text = "Synchronize";
			// 
			// mnuSynchronizeD
			// 
			this.mnuSynchronizeD.Enabled = false;
			this.mnuSynchronizeD.Index = 4;
			this.mnuSynchronizeD.Text = "Synchronize tools";
			// 
			// mnuQA
			// 
			this.mnuQA.Index = 5;
			this.mnuQA.Text = "Q and A";
			// 
			// mnuPoll
			// 
			this.mnuPoll.Index = 6;
			this.mnuPoll.Text = "Poll ";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 7;
			this.menuItem5.Text = "";
			// 
			// mnu_Minuts
			// 
			this.mnu_Minuts.Index = 3;
			this.mnu_Minuts.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnu_MM_Wht,
																					   this.mnu_MM_App,
																					   this.mnu_MM_Web});
			this.mnu_Minuts.OwnerDraw = true;
			this.mnu_Minuts.Text = "&Save";
			// 
			// mnu_MM_Wht
			// 
			this.mnu_MM_Wht.Index = 0;
			this.mnu_MM_Wht.OwnerDraw = true;
			this.mnu_MM_Wht.Text = "&Whiteboard  ";
			// 
			// mnu_MM_App
			// 
			this.mnu_MM_App.Index = 1;
			this.mnu_MM_App.Text = "&Application  ";
			// 
			// mnu_MM_Web
			// 
			this.mnu_MM_Web.Index = 2;
			this.mnu_MM_Web.Text = "Web &Browser  ";
			// 
			// menuItem59
			// 
			this.menuItem59.Index = 4;
			this.menuItem59.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuAboutCompassnavMeeting,
																					   this.mnumdetails,
																					   this.menuItem4});
			this.menuItem59.OwnerDraw = true;
			this.menuItem59.Text = "Help";
			// 
			// mnuAboutCompassnavMeeting
			// 
			this.mnuAboutCompassnavMeeting.Index = 0;
			this.mnuAboutCompassnavMeeting.Text = "About Compassnav meeting";
			// 
			// mnumdetails
			// 
			this.mnumdetails.Index = 1;
			this.mnumdetails.Text = "Meeting details";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "Test network";
			// 
			// listParticipents
			// 
			this.listParticipents.BackColor = System.Drawing.SystemColors.Window;
			this.listParticipents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							   this.columnHeader1,
																							   this.columnHeader6,
																							   this.columnHeader2,
																							   this.columnHeader3,
																							   this.columnHeader4,
																							   this.columnHeader5});
			this.listParticipents.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.listParticipents.FullRowSelect = true;
			this.listParticipents.Location = new System.Drawing.Point(8, 24);
			this.listParticipents.MultiSelect = false;
			this.listParticipents.Name = "listParticipents";
			this.listParticipents.Size = new System.Drawing.Size(256, 128);
			this.listParticipents.TabIndex = 18;
			this.listParticipents.View = System.Windows.Forms.View.Details;
			this.listParticipents.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listParticipents_MouseDown);
			this.listParticipents.SelectedIndexChanged += new System.EventHandler(this.listParticipents_SelectedIndexChanged);
			this.listParticipents.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listParticipents_MouseMove);
			this.listParticipents.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listParticipents_ItemCheck);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Rights";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "S";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "M";
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Color";
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "P";
			// 
			// conn
			// 
			this.conn.Text = "Speed";
			this.conn.Width = 18;
			// 
			// Mood
			// 
			this.Mood.Text = "M";
			this.Mood.Width = 18;
			// 
			// color
			// 
			this.color.Text = "Color";
			this.color.Width = 45;
			// 
			// Video
			// 
			this.Video.Text = "";
			this.Video.Width = 45;
			// 
			// panelQuickLaunch
			// 
			this.panelQuickLaunch.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
			this.panelQuickLaunch.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.panelQuickLaunch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelQuickLaunch.BackgroundImage")));
			this.panelQuickLaunch.Controls.Add(this.panel2);
			this.panelQuickLaunch.Controls.Add(this.toolbarButtonsPanel);
			this.panelQuickLaunch.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelQuickLaunch.Location = new System.Drawing.Point(0, 0);
			this.panelQuickLaunch.Name = "panelQuickLaunch";
			this.panelQuickLaunch.Size = new System.Drawing.Size(1016, 50);
			this.panelQuickLaunch.TabIndex = 6;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.panel2.Controls.Add(this.pictureBox1);
			this.panel2.Controls.Add(this.toolbarBtnDesktopSharingDisable);
			this.panel2.Controls.Add(this.toolbarBtnDesktopSharing);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(1025, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(0, 50);
			this.panel2.TabIndex = 23;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(0, 50);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 11;
			this.pictureBox1.TabStop = false;
			// 
			// toolbarBtnDesktopSharingDisable
			// 
			this.toolbarBtnDesktopSharingDisable.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.toolbarBtnDesktopSharingDisable.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.toolbarBtnDesktopSharingDisable.HottrackImage = ((System.Drawing.Image)(resources.GetObject("toolbarBtnDesktopSharingDisable.HottrackImage")));
			this.toolbarBtnDesktopSharingDisable.Location = new System.Drawing.Point(76, -2);
			this.toolbarBtnDesktopSharingDisable.Name = "toolbarBtnDesktopSharingDisable";
			this.toolbarBtnDesktopSharingDisable.NormalImage = ((System.Drawing.Image)(resources.GetObject("toolbarBtnDesktopSharingDisable.NormalImage")));
			this.toolbarBtnDesktopSharingDisable.OnlyShowBitmap = true;
			this.toolbarBtnDesktopSharingDisable.PressedImage = ((System.Drawing.Image)(resources.GetObject("toolbarBtnDesktopSharingDisable.PressedImage")));
			this.toolbarBtnDesktopSharingDisable.Size = new System.Drawing.Size(27, 21);
			this.toolbarBtnDesktopSharingDisable.Text = "nsButton1";
			this.toolbarBtnDesktopSharingDisable.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.toolbarBtnDesktopSharingDisable.ToolTip = null;
			this.toolbarBtnDesktopSharingDisable.Click += new System.EventHandler(this.toolbarBtnDesktopSharingDisable_Click);
			// 
			// toolbarBtnDesktopSharing
			// 
			this.toolbarBtnDesktopSharing.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.toolbarBtnDesktopSharing.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.toolbarBtnDesktopSharing.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.toolbarBtnDesktopSharing.HottrackImage = ((System.Drawing.Image)(resources.GetObject("toolbarBtnDesktopSharing.HottrackImage")));
			this.toolbarBtnDesktopSharing.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.toolbarBtnDesktopSharing.Location = new System.Drawing.Point(74, -5);
			this.toolbarBtnDesktopSharing.Name = "toolbarBtnDesktopSharing";
			this.toolbarBtnDesktopSharing.NormalImage = ((System.Drawing.Image)(resources.GetObject("toolbarBtnDesktopSharing.NormalImage")));
			this.toolbarBtnDesktopSharing.OnlyShowBitmap = true;
			this.toolbarBtnDesktopSharing.PressedImage = ((System.Drawing.Image)(resources.GetObject("toolbarBtnDesktopSharing.PressedImage")));
			this.toolbarBtnDesktopSharing.Size = new System.Drawing.Size(31, 34);
			this.toolbarBtnDesktopSharing.Text = "toolbarBtnDesktopSharing";
			this.toolbarBtnDesktopSharing.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.toolbarBtnDesktopSharing.ToolTip = null;
			this.toolbarBtnDesktopSharing.Click += new System.EventHandler(this.nsButton8_Click);
			// 
			// toolbarButtonsPanel
			// 
			this.toolbarButtonsPanel.AutoScroll = true;
			this.toolbarButtonsPanel.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.toolbarButtonsPanel.Controls.Add(this.btn_QA);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Poll);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Record);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Whiteboard);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Chat);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Managecontent);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Video);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Docshare);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Audio);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Appshare);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Webshare);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Invite);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Snapshot);
			this.toolbarButtonsPanel.Controls.Add(this.btn_Exit);
			this.toolbarButtonsPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.toolbarButtonsPanel.Location = new System.Drawing.Point(0, 0);
			this.toolbarButtonsPanel.Name = "toolbarButtonsPanel";
			this.toolbarButtonsPanel.Size = new System.Drawing.Size(1025, 50);
			this.toolbarButtonsPanel.TabIndex = 22;
			this.toolbarButtonsPanel.Text = "toolbarButtonsPanel";
			// 
			// btn_QA
			// 
			this.btn_QA.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_QA.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btn_QA.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_QA.HottrackImage")));
			this.btn_QA.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_QA.Location = new System.Drawing.Point(867, 0);
			this.btn_QA.Name = "btn_QA";
			this.btn_QA.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_QA.NormalImage")));
			this.btn_QA.OnlyShowBitmap = true;
			this.btn_QA.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_QA.PressedImage")));
			this.btn_QA.Size = new System.Drawing.Size(70, 49);
			this.btn_QA.Text = "z";
			this.btn_QA.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_QA.ToolTip = null;
			// 
			// btn_Poll
			// 
			this.btn_Poll.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Poll.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btn_Poll.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Poll.HottrackImage")));
			this.btn_Poll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_Poll.Location = new System.Drawing.Point(793, 0);
			this.btn_Poll.Name = "btn_Poll";
			this.btn_Poll.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Poll.NormalImage")));
			this.btn_Poll.OnlyShowBitmap = true;
			this.btn_Poll.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Poll.PressedImage")));
			this.btn_Poll.Size = new System.Drawing.Size(70, 49);
			this.btn_Poll.Text = "z";
			this.btn_Poll.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Poll.ToolTip = null;
			// 
			// btn_Record
			// 
			this.btn_Record.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Record.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btn_Record.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Record.HottrackImage")));
			this.btn_Record.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_Record.Location = new System.Drawing.Point(288, 0);
			this.btn_Record.Name = "btn_Record";
			this.btn_Record.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Record.NormalImage")));
			this.btn_Record.OnlyShowBitmap = true;
			this.btn_Record.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Record.PressedImage")));
			this.btn_Record.Size = new System.Drawing.Size(70, 49);
			this.btn_Record.Text = "btn_Record";
			this.btn_Record.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Record.ToolTip = null;
			this.btn_Record.Click += new System.EventHandler(this.btn_Record_Click);
			// 
			// btn_Whiteboard
			// 
			this.btn_Whiteboard.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Whiteboard.HighlightColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btn_Whiteboard.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Whiteboard.HottrackImage")));
			this.btn_Whiteboard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_Whiteboard.Location = new System.Drawing.Point(1, 0);
			this.btn_Whiteboard.Name = "btn_Whiteboard";
			this.btn_Whiteboard.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Whiteboard.NormalImage")));
			this.btn_Whiteboard.OnlyShowBitmap = true;
			this.btn_Whiteboard.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Whiteboard.PressedImage")));
			this.btn_Whiteboard.Size = new System.Drawing.Size(70, 49);
			this.btn_Whiteboard.Text = "btn_Whiteboard";
			this.btn_Whiteboard.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Whiteboard.ToolTip = "Share Whiteboard";
			this.btn_Whiteboard.Click += new System.EventHandler(this.btn_Whiteboard_Click);
			// 
			// btn_Chat
			// 
			this.btn_Chat.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Chat.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btn_Chat.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Chat.HottrackImage")));
			this.btn_Chat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_Chat.Location = new System.Drawing.Point(647, 0);
			this.btn_Chat.Name = "btn_Chat";
			this.btn_Chat.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Chat.NormalImage")));
			this.btn_Chat.OnlyShowBitmap = true;
			this.btn_Chat.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Chat.PressedImage")));
			this.btn_Chat.Size = new System.Drawing.Size(70, 49);
			this.btn_Chat.Text = "btn_Chat";
			this.btn_Chat.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Chat.ToolTip = null;
			this.btn_Chat.Click += new System.EventHandler(this.btn_Chat_Click);
			// 
			// btn_Managecontent
			// 
			this.btn_Managecontent.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Managecontent.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btn_Managecontent.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Managecontent.HottrackImage")));
			this.btn_Managecontent.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_Managecontent.Location = new System.Drawing.Point(144, 0);
			this.btn_Managecontent.Name = "btn_Managecontent";
			this.btn_Managecontent.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Managecontent.NormalImage")));
			this.btn_Managecontent.OnlyShowBitmap = true;
			this.btn_Managecontent.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Managecontent.PressedImage")));
			this.btn_Managecontent.Size = new System.Drawing.Size(70, 49);
			this.btn_Managecontent.Text = "btn_Managecontent";
			this.btn_Managecontent.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Managecontent.ToolTip = null;
			this.btn_Managecontent.Click += new System.EventHandler(this.btn_Managecontent_Click);
			// 
			// btn_Video
			// 
			this.btn_Video.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Video.HighlightColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btn_Video.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Video.HottrackImage")));
			this.btn_Video.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_Video.Location = new System.Drawing.Point(503, 0);
			this.btn_Video.Name = "btn_Video";
			this.btn_Video.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Video.NormalImage")));
			this.btn_Video.OnlyShowBitmap = true;
			this.btn_Video.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Video.PressedImage")));
			this.btn_Video.Size = new System.Drawing.Size(70, 49);
			this.btn_Video.Text = "btn_Video";
			this.btn_Video.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Video.ToolTip = null;
			this.btn_Video.Click += new System.EventHandler(this.btn_Video_Click);
			// 
			// btn_Docshare
			// 
			this.btn_Docshare.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Docshare.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btn_Docshare.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Docshare.HottrackImage")));
			this.btn_Docshare.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_Docshare.Location = new System.Drawing.Point(73, 0);
			this.btn_Docshare.Name = "btn_Docshare";
			this.btn_Docshare.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Docshare.NormalImage")));
			this.btn_Docshare.OnlyShowBitmap = true;
			this.btn_Docshare.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Docshare.PressedImage")));
			this.btn_Docshare.Size = new System.Drawing.Size(70, 49);
			this.btn_Docshare.Text = "btn_Docshare";
			this.btn_Docshare.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Docshare.ToolTip = null;
			this.btn_Docshare.Click += new System.EventHandler(this.btn_Docshare_Click);
			// 
			// btn_Audio
			// 
			this.btn_Audio.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Audio.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btn_Audio.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Audio.HottrackImage")));
			this.btn_Audio.ImeMode = System.Windows.Forms.ImeMode.Off;
			this.btn_Audio.Location = new System.Drawing.Point(432, 0);
			this.btn_Audio.Name = "btn_Audio";
			this.btn_Audio.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Audio.NormalImage")));
			this.btn_Audio.OnlyShowBitmap = true;
			this.btn_Audio.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Audio.PressedImage")));
			this.btn_Audio.Size = new System.Drawing.Size(70, 49);
			this.btn_Audio.Text = "btn_Audio";
			this.btn_Audio.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Audio.ToolTip = null;
			this.btn_Audio.Click += new System.EventHandler(this.btn_Audio_Click);
			// 
			// btn_Appshare
			// 
			this.btn_Appshare.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Appshare.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btn_Appshare.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Appshare.HottrackImage")));
			this.btn_Appshare.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_Appshare.Location = new System.Drawing.Point(216, 0);
			this.btn_Appshare.Name = "btn_Appshare";
			this.btn_Appshare.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Appshare.NormalImage")));
			this.btn_Appshare.OnlyShowBitmap = true;
			this.btn_Appshare.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Appshare.PressedImage")));
			this.btn_Appshare.Size = new System.Drawing.Size(70, 49);
			this.btn_Appshare.Text = "btn_Appshare";
			this.btn_Appshare.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Appshare.ToolTip = null;
			this.btn_Appshare.Click += new System.EventHandler(this.nsButton6_Click);
			// 
			// btn_Webshare
			// 
			this.btn_Webshare.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Webshare.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btn_Webshare.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Webshare.HottrackImage")));
			this.btn_Webshare.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_Webshare.Location = new System.Drawing.Point(575, 0);
			this.btn_Webshare.Name = "btn_Webshare";
			this.btn_Webshare.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Webshare.NormalImage")));
			this.btn_Webshare.OnlyShowBitmap = true;
			this.btn_Webshare.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Webshare.PressedImage")));
			this.btn_Webshare.Size = new System.Drawing.Size(70, 49);
			this.btn_Webshare.Text = "btn_Webshare";
			this.btn_Webshare.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Webshare.ToolTip = null;
			this.btn_Webshare.Click += new System.EventHandler(this.btnQA_Click);
			// 
			// btn_Invite
			// 
			this.btn_Invite.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Invite.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btn_Invite.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Invite.HottrackImage")));
			this.btn_Invite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_Invite.Location = new System.Drawing.Point(360, 0);
			this.btn_Invite.Name = "btn_Invite";
			this.btn_Invite.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Invite.NormalImage")));
			this.btn_Invite.OnlyShowBitmap = true;
			this.btn_Invite.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Invite.PressedImage")));
			this.btn_Invite.Size = new System.Drawing.Size(70, 49);
			this.btn_Invite.Text = "btn_Invite";
			this.btn_Invite.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Invite.ToolTip = null;
			this.btn_Invite.Click += new System.EventHandler(this.btn_Invite_Click);
			// 
			// btn_Snapshot
			// 
			this.btn_Snapshot.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Snapshot.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btn_Snapshot.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Snapshot.HottrackImage")));
			this.btn_Snapshot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_Snapshot.Location = new System.Drawing.Point(719, 0);
			this.btn_Snapshot.Name = "btn_Snapshot";
			this.btn_Snapshot.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Snapshot.NormalImage")));
			this.btn_Snapshot.OnlyShowBitmap = true;
			this.btn_Snapshot.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Snapshot.PressedImage")));
			this.btn_Snapshot.Size = new System.Drawing.Size(70, 49);
			this.btn_Snapshot.Text = "btn_Snapshot";
			this.btn_Snapshot.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Snapshot.ToolTip = null;
			this.btn_Snapshot.Click += new System.EventHandler(this.btn_Snapshot_Click);
			// 
			// btn_Exit
			// 
			this.btn_Exit.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btn_Exit.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btn_Exit.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btn_Exit.HottrackImage")));
			this.btn_Exit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btn_Exit.Location = new System.Drawing.Point(940, 0);
			this.btn_Exit.Name = "btn_Exit";
			this.btn_Exit.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_Exit.NormalImage")));
			this.btn_Exit.OnlyShowBitmap = true;
			this.btn_Exit.PressedImage = ((System.Drawing.Image)(resources.GetObject("btn_Exit.PressedImage")));
			this.btn_Exit.Size = new System.Drawing.Size(70, 49);
			this.btn_Exit.Text = "z";
			this.btn_Exit.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btn_Exit.ToolTip = null;
			this.btn_Exit.Click += new System.EventHandler(this.toolbarBtnExit_Click);
			// 
			// rightPanel
			// 
			this.rightPanel.Controls.Add(this.splitter3);
			this.rightPanel.Controls.Add(this.panalAdvertiseBrowser);
			this.rightPanel.Controls.Add(this.panel1);
			this.rightPanel.Controls.Add(this.splitter2);
			this.rightPanel.Controls.Add(this.tabControl1);
			this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightPanel.Location = new System.Drawing.Point(766, 0);
			this.rightPanel.Name = "rightPanel";
			this.rightPanel.Size = new System.Drawing.Size(250, 642);
			this.rightPanel.TabIndex = 12;
			// 
			// splitter3
			// 
			this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter3.Location = new System.Drawing.Point(0, 497);
			this.splitter3.Name = "splitter3";
			this.splitter3.Size = new System.Drawing.Size(250, 4);
			this.splitter3.TabIndex = 12;
			this.splitter3.TabStop = false;
			// 
			// panalAdvertiseBrowser
			// 
			this.panalAdvertiseBrowser.Controls.Add(this.axWebBrowser2);
			this.panalAdvertiseBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panalAdvertiseBrowser.Location = new System.Drawing.Point(0, 497);
			this.panalAdvertiseBrowser.Name = "panalAdvertiseBrowser";
			this.panalAdvertiseBrowser.Size = new System.Drawing.Size(250, 145);
			this.panalAdvertiseBrowser.TabIndex = 11;
			// 
			// axWebBrowser2
			// 
			this.axWebBrowser2.ContainingControl = this;
			this.axWebBrowser2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser2.Enabled = true;
			this.axWebBrowser2.Location = new System.Drawing.Point(0, 0);
			this.axWebBrowser2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser2.OcxState")));
			this.axWebBrowser2.Size = new System.Drawing.Size(250, 145);
			this.axWebBrowser2.TabIndex = 0;
			this.axWebBrowser2.DownloadComplete += new System.EventHandler(this.axWebBrowser2_DownloadComplete);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Silver;
			this.panel1.Controls.Add(this.chatTabControl);
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 207);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(250, 290);
			this.panel1.TabIndex = 10;
			// 
			// chatTabControl
			// 
			this.chatTabControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.chatTabControl.ButtonActiveColor = System.Drawing.Color.FromArgb(((System.Byte)(250)), ((System.Byte)(162)), ((System.Byte)(37)));
			this.chatTabControl.ButtonInactiveColor = System.Drawing.Color.FromArgb(((System.Byte)(250)), ((System.Byte)(162)), ((System.Byte)(37)));
			this.chatTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chatTabControl.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways;
			this.chatTabControl.IDEPixelBorder = true;
			this.chatTabControl.ImageList = this.imageList1;
			this.chatTabControl.Location = new System.Drawing.Point(0, 0);
			this.chatTabControl.Name = "chatTabControl";
			this.chatTabControl.PositionTop = true;
			this.chatTabControl.ShowArrows = true;
			this.chatTabControl.ShowClose = true;
			this.chatTabControl.ShrinkPagesToFit = false;
			this.chatTabControl.Size = new System.Drawing.Size(250, 273);
			this.chatTabControl.TabIndex = 6;
			this.chatTabControl.TextInactiveColor = System.Drawing.Color.FromArgb(((System.Byte)(250)), ((System.Byte)(162)), ((System.Byte)(37)));
			this.chatTabControl.SelectionChanged += new System.EventHandler(this.chatTabControl_SelectionChanged);
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.panel3.Controls.Add(this.btnRestoreUp);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 273);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(250, 17);
			this.panel3.TabIndex = 7;
			// 
			// btnRestoreUp
			// 
			this.btnRestoreUp.ForeColor = System.Drawing.Color.Transparent;
			this.btnRestoreUp.ImageIndex = 3;
			this.btnRestoreUp.ImageList = this.imageList1;
			this.btnRestoreUp.Location = new System.Drawing.Point(4, 0);
			this.btnRestoreUp.Name = "btnRestoreUp";
			this.btnRestoreUp.Size = new System.Drawing.Size(17, 16);
			this.btnRestoreUp.TabIndex = 0;
			this.btnRestoreUp.Text = "btnRestoreUp";
			this.btnRestoreUp.Click += new System.EventHandler(this.btnRestoreUp_Click);
			// 
			// splitter2
			// 
			this.splitter2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(212)), ((System.Byte)(208)), ((System.Byte)(200)));
			this.splitter2.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point(0, 203);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(250, 4);
			this.splitter2.TabIndex = 7;
			this.splitter2.TabStop = false;
			// 
			// tabControl1
			// 
			this.tabControl1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.tabControl1.Controls.Add(this.listParticipents);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabControl1.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.PositionTop = true;
			this.tabControl1.Size = new System.Drawing.Size(250, 203);
			this.tabControl1.TabIndex = 3;
			this.tabControl1.SizeChanged += new System.EventHandler(this.tabControl1_SizeChanged);
			this.tabControl1.SelectionChanged += new System.EventHandler(this.tabControl1_SelectionChanged);
			this.tabControl1.ClosePressed += new System.EventHandler(this.Close_UserPanel);
			// 
			// tabPagesMenu
			// 
			this.tabPagesMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menu_Close});
			// 
			// menu_Close
			// 
			this.menu_Close.Index = 0;
			this.menu_Close.Text = "Close";
			this.menu_Close.Click += new System.EventHandler(this.Close_Click);
			// 
			// mainMenuImageList
			// 
			this.mainMenuImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.mainMenuImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mainMenuImageList.ImageStream")));
			this.mainMenuImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// listViewImageList
			// 
			this.listViewImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.listViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("listViewImageList.ImageStream")));
			this.listViewImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panelMain
			// 
			this.panelMain.Controls.Add(this.splitter1);
			this.panelMain.Controls.Add(this.panelBody);
			this.panelMain.Controls.Add(this.rightPanel);
			this.panelMain.Controls.Add(this.toolbarPanel);
			this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMain.Location = new System.Drawing.Point(0, 50);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(1016, 642);
			this.panelMain.TabIndex = 23;
			// 
			// splitter1
			// 
			this.splitter1.AnimationDelay = 20;
			this.splitter1.AnimationStep = 20;
			this.splitter1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(250)), ((System.Byte)(162)), ((System.Byte)(37)));
			this.splitter1.BorderStyle3D = System.Windows.Forms.Border3DStyle.Raised;
			this.splitter1.ControlToHide = this.rightPanel;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.ExpandParentForm = false;
			this.splitter1.Location = new System.Drawing.Point(758, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.SplitterThumbBackColoor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.splitter1.TabIndex = 13;
			this.splitter1.TabStop = false;
			this.splitter1.UseAnimations = false;
			this.splitter1.VisualStyle = NJFLib.Controls.VisualStyles.XP;
			// 
			// panelBody
			// 
			this.panelBody.BackColor = System.Drawing.Color.White;
			this.panelBody.Controls.Add(this.tabBody);
			this.panelBody.Controls.Add(this.panelSplitter);
			this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelBody.Location = new System.Drawing.Point(28, 0);
			this.panelBody.Name = "panelBody";
			this.panelBody.Size = new System.Drawing.Size(738, 642);
			this.panelBody.TabIndex = 14;
			// 
			// tabBody
			// 
			this.tabBody.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.tabBody.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabBody.BackgroundImage")));
			this.tabBody.ButtonActiveColor = System.Drawing.Color.FromArgb(((System.Byte)(250)), ((System.Byte)(162)), ((System.Byte)(37)));
			this.tabBody.ButtonInactiveColor = System.Drawing.Color.FromArgb(((System.Byte)(250)), ((System.Byte)(162)), ((System.Byte)(37)));
			this.tabBody.Controls.Add(this.panelRecordingToolbar);
			this.tabBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabBody.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways;
			this.tabBody.HotTextColor = System.Drawing.Color.Red;
			this.tabBody.Location = new System.Drawing.Point(0, 0);
			this.tabBody.Name = "tabBody";
			this.tabBody.PositionTop = true;
			this.tabBody.SelectedTabEx = null;
			this.tabBody.ShowArrows = true;
			this.tabBody.Size = new System.Drawing.Size(730, 642);
			this.tabBody.TabIndex = 9;
			this.tabBody.TextInactiveColor = System.Drawing.Color.FromArgb(((System.Byte)(250)), ((System.Byte)(162)), ((System.Byte)(37)));
			this.tabBody.Resize += new System.EventHandler(this.tabBody_Resize);
			this.tabBody.SelectionChanged += new System.EventHandler(this.tabBody_SelectionChanged);
			this.tabBody.ClosePressed += new System.EventHandler(this.Close_Click);
			// 
			// panelRecordingToolbar
			// 
			this.panelRecordingToolbar.Controls.Add(this.recordingStopButton);
			this.panelRecordingToolbar.Controls.Add(this.recordingRecordButton);
			this.panelRecordingToolbar.Controls.Add(this.recordingPauseButton);
			this.panelRecordingToolbar.Location = new System.Drawing.Point(650, 30);
			this.panelRecordingToolbar.Name = "panelRecordingToolbar";
			this.panelRecordingToolbar.Size = new System.Drawing.Size(66, 23);
			this.panelRecordingToolbar.TabIndex = 4;
			this.panelRecordingToolbar.Visible = false;
			// 
			// recordingStopButton
			// 
			this.recordingStopButton.BackColor = System.Drawing.SystemColors.Control;
			this.recordingStopButton.Enabled = false;
			this.recordingStopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.recordingStopButton.ImageIndex = 7;
			this.recordingStopButton.ImageList = this.listViewImageList;
			this.recordingStopButton.Location = new System.Drawing.Point(44, 1);
			this.recordingStopButton.Name = "recordingStopButton";
			this.recordingStopButton.Size = new System.Drawing.Size(21, 21);
			this.recordingStopButton.TabIndex = 9;
			this.recordingStopButton.Click += new System.EventHandler(this.button8_Click);
			// 
			// recordingRecordButton
			// 
			this.recordingRecordButton.BackColor = System.Drawing.SystemColors.Control;
			this.recordingRecordButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.recordingRecordButton.ImageIndex = 8;
			this.recordingRecordButton.ImageList = this.listViewImageList;
			this.recordingRecordButton.Location = new System.Drawing.Point(1, 1);
			this.recordingRecordButton.Name = "recordingRecordButton";
			this.recordingRecordButton.Size = new System.Drawing.Size(21, 21);
			this.recordingRecordButton.TabIndex = 8;
			this.recordingRecordButton.Click += new System.EventHandler(this.recordingRecordButton_Click);
			// 
			// recordingPauseButton
			// 
			this.recordingPauseButton.BackColor = System.Drawing.SystemColors.Control;
			this.recordingPauseButton.Enabled = false;
			this.recordingPauseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.recordingPauseButton.ImageIndex = 6;
			this.recordingPauseButton.ImageList = this.listViewImageList;
			this.recordingPauseButton.Location = new System.Drawing.Point(22, 1);
			this.recordingPauseButton.Name = "recordingPauseButton";
			this.recordingPauseButton.Size = new System.Drawing.Size(21, 21);
			this.recordingPauseButton.TabIndex = 7;
			this.recordingPauseButton.Text = "cv";
			this.recordingPauseButton.Click += new System.EventHandler(this.recordingPauseButton_Click);
			// 
			// panelSplitter
			// 
			this.panelSplitter.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelSplitter.Location = new System.Drawing.Point(730, 0);
			this.panelSplitter.Name = "panelSplitter";
			this.panelSplitter.Size = new System.Drawing.Size(8, 642);
			this.panelSplitter.TabIndex = 10;
			// 
			// toolbarPanel
			// 
			this.toolbarPanel.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.toolbarPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.toolbarPanel.Controls.Add(this.btnPlaceHolderArrow);
			this.toolbarPanel.Controls.Add(this.btnPrev);
			this.toolbarPanel.Controls.Add(this.btnNext);
			this.toolbarPanel.Controls.Add(this.btnClearWhiteBoard);
			this.toolbarPanel.Controls.Add(this.btnArrow);
			this.toolbarPanel.Controls.Add(this.btnColor);
			this.toolbarPanel.Controls.Add(this.btnFont);
			this.toolbarPanel.Controls.Add(this.btnNone);
			this.toolbarPanel.Controls.Add(this.btnPencil);
			this.toolbarPanel.Controls.Add(this.btnEraser);
			this.toolbarPanel.Controls.Add(this.btnBrush);
			this.toolbarPanel.Controls.Add(this.btnRectangle);
			this.toolbarPanel.Controls.Add(this.btnText);
			this.toolbarPanel.Controls.Add(this.btnLine);
			this.toolbarPanel.Controls.Add(this.btnCircle);
			this.toolbarPanel.Controls.Add(this.button4);
			this.toolbarPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.toolbarPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.toolbarPanel.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(250)), ((System.Byte)(162)), ((System.Byte)(37)));
			this.toolbarPanel.Location = new System.Drawing.Point(0, 0);
			this.toolbarPanel.Name = "toolbarPanel";
			this.toolbarPanel.Size = new System.Drawing.Size(28, 642);
			this.toolbarPanel.TabIndex = 13;
			// 
			// btnPlaceHolderArrow
			// 
			this.btnPlaceHolderArrow.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnPlaceHolderArrow.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnPlaceHolderArrow.ForeColor = System.Drawing.Color.Transparent;
			this.btnPlaceHolderArrow.Image = ((System.Drawing.Image)(resources.GetObject("btnPlaceHolderArrow.Image")));
			this.btnPlaceHolderArrow.Location = new System.Drawing.Point(2, 25);
			this.btnPlaceHolderArrow.Name = "btnPlaceHolderArrow";
			this.btnPlaceHolderArrow.Size = new System.Drawing.Size(21, 21);
			this.btnPlaceHolderArrow.TabIndex = 16;
			this.btnPlaceHolderArrow.Text = "btnPlaceHolderArrow";
			this.btnPlaceHolderArrow.Click += new System.EventHandler(this.btnPlaceHolderArrow_Click);
			// 
			// btnPrev
			// 
			this.btnPrev.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnPrev.Enabled = false;
			this.btnPrev.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnPrev.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnPrev.HottrackImage")));
			this.btnPrev.Location = new System.Drawing.Point(0, 390);
			this.btnPrev.Name = "btnPrev";
			this.btnPrev.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnPrev.NormalImage")));
			this.btnPrev.OnlyShowBitmap = true;
			this.btnPrev.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnPrev.PressedImage")));
			this.btnPrev.Size = new System.Drawing.Size(23, 27);
			this.btnPrev.Text = "nsButton3";
			this.btnPrev.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnPrev.ToolTip = "Press to go Previous Slide";
			this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
			// 
			// btnNext
			// 
			this.btnNext.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnNext.Enabled = false;
			this.btnNext.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnNext.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnNext.HottrackImage")));
			this.btnNext.Location = new System.Drawing.Point(1, 367);
			this.btnNext.Name = "btnNext";
			this.btnNext.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnNext.NormalImage")));
			this.btnNext.OnlyShowBitmap = true;
			this.btnNext.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnNext.PressedImage")));
			this.btnNext.Size = new System.Drawing.Size(21, 25);
			this.btnNext.Text = "nsButton3";
			this.btnNext.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnNext.ToolTip = "Press to go Next Slide";
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnClearWhiteBoard
			// 
			this.btnClearWhiteBoard.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnClearWhiteBoard.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnClearWhiteBoard.ForeColor = System.Drawing.Color.Transparent;
			this.btnClearWhiteBoard.Image = ((System.Drawing.Image)(resources.GetObject("btnClearWhiteBoard.Image")));
			this.btnClearWhiteBoard.Location = new System.Drawing.Point(2, 304);
			this.btnClearWhiteBoard.Name = "btnClearWhiteBoard";
			this.btnClearWhiteBoard.Size = new System.Drawing.Size(21, 21);
			this.btnClearWhiteBoard.TabIndex = 15;
			this.btnClearWhiteBoard.Text = "btnClearWhiteBoard";
			this.btnClearWhiteBoard.Click += new System.EventHandler(this.btnClearWhiteBoard_Click);
			// 
			// btnArrow
			// 
			this.btnArrow.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnArrow.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnArrow.ForeColor = System.Drawing.Color.Transparent;
			this.btnArrow.Image = ((System.Drawing.Image)(resources.GetObject("btnArrow.Image")));
			this.btnArrow.Location = new System.Drawing.Point(2, 48);
			this.btnArrow.Name = "btnArrow";
			this.btnArrow.Size = new System.Drawing.Size(21, 21);
			this.btnArrow.TabIndex = 14;
			this.btnArrow.Text = "btnArrow";
			this.btnArrow.Click += new System.EventHandler(this.button5_Click);
			// 
			// btnColor
			// 
			this.btnColor.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnColor.ForeColor = System.Drawing.Color.Transparent;
			this.btnColor.Image = ((System.Drawing.Image)(resources.GetObject("btnColor.Image")));
			this.btnColor.Location = new System.Drawing.Point(2, 238);
			this.btnColor.Name = "btnColor";
			this.btnColor.Size = new System.Drawing.Size(21, 21);
			this.btnColor.TabIndex = 9;
			this.btnColor.Text = "btnColor";
			this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
			// 
			// btnFont
			// 
			this.btnFont.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnFont.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnFont.ForeColor = System.Drawing.Color.Transparent;
			this.btnFont.Image = ((System.Drawing.Image)(resources.GetObject("btnFont.Image")));
			this.btnFont.Location = new System.Drawing.Point(2, 282);
			this.btnFont.Name = "btnFont";
			this.btnFont.Size = new System.Drawing.Size(21, 21);
			this.btnFont.TabIndex = 8;
			this.btnFont.Text = "btnFont";
			this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
			// 
			// btnNone
			// 
			this.btnNone.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnNone.ForeColor = System.Drawing.Color.Transparent;
			this.btnNone.Image = ((System.Drawing.Image)(resources.GetObject("btnNone.Image")));
			this.btnNone.Location = new System.Drawing.Point(2, 67);
			this.btnNone.Name = "btnNone";
			this.btnNone.Size = new System.Drawing.Size(21, 21);
			this.btnNone.TabIndex = 7;
			this.btnNone.Text = "btnNone";
			this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
			// 
			// btnPencil
			// 
			this.btnPencil.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnPencil.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnPencil.ForeColor = System.Drawing.Color.Transparent;
			this.btnPencil.Image = ((System.Drawing.Image)(resources.GetObject("btnPencil.Image")));
			this.btnPencil.Location = new System.Drawing.Point(2, 172);
			this.btnPencil.Name = "btnPencil";
			this.btnPencil.Size = new System.Drawing.Size(21, 21);
			this.btnPencil.TabIndex = 6;
			this.btnPencil.Text = "btnPencil";
			this.btnPencil.Click += new System.EventHandler(this.btnPencil_Click);
			// 
			// btnEraser
			// 
			this.btnEraser.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnEraser.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnEraser.ForeColor = System.Drawing.Color.Transparent;
			this.btnEraser.Image = ((System.Drawing.Image)(resources.GetObject("btnEraser.Image")));
			this.btnEraser.Location = new System.Drawing.Point(2, 326);
			this.btnEraser.Name = "btnEraser";
			this.btnEraser.Size = new System.Drawing.Size(21, 21);
			this.btnEraser.TabIndex = 5;
			this.btnEraser.Text = "btnEraser";
			this.btnEraser.Click += new System.EventHandler(this.btnEraser_Click);
			// 
			// btnBrush
			// 
			this.btnBrush.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnBrush.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnBrush.ForeColor = System.Drawing.Color.Transparent;
			this.btnBrush.Image = ((System.Drawing.Image)(resources.GetObject("btnBrush.Image")));
			this.btnBrush.Location = new System.Drawing.Point(2, 151);
			this.btnBrush.Name = "btnBrush";
			this.btnBrush.Size = new System.Drawing.Size(21, 21);
			this.btnBrush.TabIndex = 4;
			this.btnBrush.Text = "btnBrush";
			this.btnBrush.Click += new System.EventHandler(this.btnBrush_Click);
			// 
			// btnRectangle
			// 
			this.btnRectangle.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnRectangle.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnRectangle.ForeColor = System.Drawing.Color.Transparent;
			this.btnRectangle.Image = ((System.Drawing.Image)(resources.GetObject("btnRectangle.Image")));
			this.btnRectangle.Location = new System.Drawing.Point(2, 88);
			this.btnRectangle.Name = "btnRectangle";
			this.btnRectangle.Size = new System.Drawing.Size(21, 21);
			this.btnRectangle.TabIndex = 3;
			this.btnRectangle.Text = "btnRectangle";
			this.btnRectangle.Click += new System.EventHandler(this.btnRectangle_Click);
			// 
			// btnText
			// 
			this.btnText.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnText.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnText.ForeColor = System.Drawing.Color.Transparent;
			this.btnText.Image = ((System.Drawing.Image)(resources.GetObject("btnText.Image")));
			this.btnText.Location = new System.Drawing.Point(2, 193);
			this.btnText.Name = "btnText";
			this.btnText.Size = new System.Drawing.Size(21, 21);
			this.btnText.TabIndex = 2;
			this.btnText.Text = "btnText";
			this.btnText.Click += new System.EventHandler(this.btnText_Click);
			// 
			// btnLine
			// 
			this.btnLine.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnLine.ForeColor = System.Drawing.Color.Transparent;
			this.btnLine.Image = ((System.Drawing.Image)(resources.GetObject("btnLine.Image")));
			this.btnLine.Location = new System.Drawing.Point(2, 130);
			this.btnLine.Name = "btnLine";
			this.btnLine.Size = new System.Drawing.Size(21, 21);
			this.btnLine.TabIndex = 1;
			this.btnLine.Text = "btnLine";
			this.btnLine.Click += new System.EventHandler(this.btnLine_Click);
			// 
			// btnCircle
			// 
			this.btnCircle.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.btnCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnCircle.ForeColor = System.Drawing.Color.Transparent;
			this.btnCircle.Image = ((System.Drawing.Image)(resources.GetObject("btnCircle.Image")));
			this.btnCircle.Location = new System.Drawing.Point(2, 109);
			this.btnCircle.Name = "btnCircle";
			this.btnCircle.Size = new System.Drawing.Size(21, 21);
			this.btnCircle.TabIndex = 0;
			this.btnCircle.Text = "btnCircle";
			this.btnCircle.Click += new System.EventHandler(this.btnCircle_Click);
			// 
			// button4
			// 
			this.button4.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 1.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.button4.ForeColor = System.Drawing.Color.Transparent;
			this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
			this.button4.Location = new System.Drawing.Point(2, 260);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(21, 21);
			this.button4.TabIndex = 10;
			this.button4.Text = "fontWidth_button4";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// contextMenuImageList
			// 
			this.contextMenuImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.contextMenuImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("contextMenuImageList.ImageStream")));
			this.contextMenuImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// axWebBrowser1
			// 
			this.axWebBrowser1.ContainingControl = this;
			this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser1.Enabled = true;
			this.axWebBrowser1.Location = new System.Drawing.Point(0, 0);
			this.axWebBrowser1.Size = new System.Drawing.Size(656, 411);
			this.axWebBrowser1.TabIndex = 0;
			this.axWebBrowser1.DownloadComplete += new System.EventHandler(this.axWebBrowser1_DownloadComplete);
			// 
			// imageList2
			// 
			this.imageList2.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
			this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// ClientUI
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.Red;
			this.ClientSize = new System.Drawing.Size(1016, 692);
			this.Controls.Add(this.panelMain);
			this.Controls.Add(this.panelQuickLaunch);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(1024, 768);
			this.Menu = this.mainMenu1;
			this.Name = "ClientUI";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Compassnav Meeting";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.ClientUI_Closing);
			this.Load += new System.EventHandler(this.ClientUI_Load);
			this.panelQuickLaunch.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.toolbarButtonsPanel.ResumeLayout(false);
			this.rightPanel.ResumeLayout(false);
			this.panalAdvertiseBrowser.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser2)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.panelMain.ResumeLayout(false);
			this.panelBody.ResumeLayout(false);
			this.tabBody.ResumeLayout(false);
			this.panelRecordingToolbar.ResumeLayout(false);
			this.toolbarPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/*for logging*/
		BinaryReaderWriter.LogWriter ExceptionLogWrite=new BinaryReaderWriter.LogWriter();
		public WebMeeting.Client.Polling.frm_PollPresenter frm_PollingGlobalPresenter;
		public WebMeeting.Client.Polling.frm_PollAttendee frm_PollingGlobalAttendee;

		//public WebMeeting.Client.Polling.frm_PollPresenter frm_PollingGlobalPresenter=new WebMeeting.Client.Polling.frm_PollPresenter();
		//public WebMeeting.Client.Polling.frm_PollAttendee frm_PollingGlobalAttendee=new WebMeeting.Client.Polling.frm_PollAttendee();
		
		
		#region  The main entry point for the application.i.e Main Method
		
		public WMIStuff.WebServiceLogging webser=new WebMeeting.Client.WMIStuff.WebServiceLogging();
		
		
		
		[STAThread] // for memory leakage
		static void Main(string[] Args) 
		{

			




//			try
//			{
//
//			
//				DMSoft.SkinCrafter.Init();
//	
//				/* ------------ Begin of CODE SECTION ------------- */
//				DMSoft.SkinCrafter SkinOb = new DMSoft.SkinCrafter();
//				SkinOb.InitLicenKeys("SKINCRAFTER","SKINCRAFTER.COM","support@skincrafter.com","DEMOSKINCRAFTERLICENCE");
//				SkinOb.InitDecoration(true);
//				/* ------------ End of CODE SECTION ------------- */
//	
//				SkinOb.LoadSkinFromFile("C:\\Documents and Settings\\uraan\\Desktop\\Skincrafter\\Zondar_ST.skf");
//				SkinOb.ApplySkin();
//			}
//			catch(Exception exp){}

			try
			{
				
bArgument=true;

#if dbgTesting
				
				DebugConsole.Instance.Init(true,true); // addition by junaid //per1
				
				ClientUI cl = new ClientUI();
				cl.ExceptionLogWrite.SetUpLogging();/*setup logging*/
				//MessageBox.Show(Args.ToString());
				//cl.ExceptionLogWrite.WriteLog(Args.ToString());
				
				if (Args.Length < 5) 		
				{
					/*MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Unable to load application. Error: Invalid execution detected",true,false);
					return;*/
					SessionWnd wnd=new SessionWnd();
					//Args.Initialize().
					if(wnd.ShowDialog()==DialogResult.Cancel)
					{	
						System.Environment.Exit(0);					
						return;
					}
					bArgument=false;
				}
#endif		


				if((WebMeeting.Client.ClientUI.getInstance().strCmdArg=="")||(WebMeeting.Client.ClientUI.getInstance().strCmdArg=="\t\t"))
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(WebMeeting.Client.Alerts.MeetingAlerts.AlertType.Fatal,"InValid Session ID provided,Aborting..",true,false);
					System.Environment.Exit(1);				
				}



				/*using(SingleProgramInstance spi = new SingleProgramInstance("xkj0972xz"))
				{
					//if (!spi.IsSingleInstance)
				{
					//		MessageBox.Show("An instance of WebMeeting is already running","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
					//spi.RaiseOtherProcess();
                    
				}
					//else
				{*/   //per1
				try
				{

					//Application.ThreadException += 	new ThreadExceptionEventHandler(Application_ThreadException);
					CustomExceptionunhandler eh    = new CustomExceptionunhandler();
					cl.ExceptionLogWrite.WriteLog(Args[0]+" " + Args[1]+ " " + Args[2]+" " + Args[3]+" " + Args[4]);
					eh.parameters=Args[0]+" " + Args[1]+ " " + Args[2]+" " + Args[3];
					Application.ThreadException += new System.Threading.
						ThreadExceptionEventHandler(eh.
						OnThreadException);
					
					if(File.Exists(Application.StartupPath + "\\flag.txt"))
					{
						File.Delete(Application.StartupPath + "\\flag.txt");
					}					
					if(bArgument)
					{
						cl.commandArgs = new string[5];	
						cl.commandArgs = Args;
					}
					
					string []firstParam = Args[0].Split('+');
					cl.nclientType=Convert.ToInt32(firstParam[0]);					
					cl.nmeeting_id=Convert.ToInt32(Args[1]);
					cl.webser.RecordID=Convert.ToInt32(Args[4]);
					Application.Run(cl);
				}
				catch(Exception exp)
				{	
					
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 226",exp,null,false);
					
				}
				/*}
				}*/ //per1
		
				//get the verification url
			
			}
			catch(Exception exp)
			{
				
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 2982",exp,null,false);
			}
		}
		# endregion 


		#region public static void Application_ThreadException( Method
		public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			try
			{
				char a;
				/*// Exit the program if the user clicks Cancel.
				DialogResult result = ShowThreadExceptionDialog(
					e.Exception);

				if (result == DialogResult.Cancel) 
					Application.Exit();
					*/
			}
			catch(Exception exp)
			{
				
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3001",exp,null,false);

				// Fatal error, terminate program
				/*try
				{
					MessageBox.Show("Fatal Error", 
						"Fatal Error",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Stop);
				}
				finally
				{
					Application.Exit();
				}*/
			}
		}
		#endregion 

		#region Getting command line args
		public void setCommandLine(string cmd)
		{
			strCmdArg=cmd;
		}
		#endregion 

		#region ChangeParticipentsListControl(
		public void ChangeParticipentsListControl(bool bToAttendee)		
		{
			try
			{
				if(bToAttendee)
				{
					//listParticipents.Columns.RemoveAt(0);
				
					listParticipents.Columns.RemoveAt(3);
					listParticipents.Columns.RemoveAt(2);				
					listParticipents.Columns.RemoveAt(1);
				
				
				}
				else
				{
					/**/
					listParticipents.Columns.Insert(1,"",18,HorizontalAlignment.Left);
					listParticipents.Columns.Insert(2,"Speed",18,HorizontalAlignment.Center); 
					listParticipents.Columns.Insert(3,"",0,HorizontalAlignment.Center); 

					int x = listParticipents.Columns.Count;
					x = x;

				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3055",exp,null,false);
			}
		}
		#endregion 

		# region Crownwood.Magic.Controls.TabPage AddTabPageToMainBodyFunction(
		private Crownwood.Magic.Controls.TabPage AddTabPageToMainBodyFunction(Crownwood.Magic.Controls.TabPage tabPage)
		{
			try
			{
				Crownwood.Magic.Controls.TabPage tempPage =  tabBody.TabPages.Add(tabPage);
				//if(this.IfIamthePresenter())
				//{
					tabBody.SelectedTab = tempPage;
				//}
				return tempPage;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3072",exp,null,false);
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,e.StackTrace,true,false);
				return null;
			}
			
		}
		#endregion 

		#region RemoveTabPageFromMainBodyFunction(
		private bool RemoveTabPageFromMainBodyFunction(Crownwood.Magic.Controls.TabPage tabPage)
		{
			try
			{
				tabBody.TabPages.Remove(tabPage);				
				return true;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3090",exp,null,false);
				//	MeetingAlerts alert=new MeetingAlerts();
				//	alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,e.StackTrace,true,false);
				return false;
			}
		}
		#endregion 

		#region cleanMyRes(	
		void cleanMyRes()
		{
			try
			{
				while(true)
				{
					GC.Collect(0);
					GC.WaitForPendingFinalizers();
					GC.Collect(1);
					GC.WaitForPendingFinalizers();
					GC.Collect(2);
					GC.WaitForPendingFinalizers();
					GC.Collect();
					GC.WaitForPendingFinalizers();
					
					Thread.Sleep(5000);
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3090",exp,null,false);
			
			}
		}
		#endregion 

		#region SetProcessWorkingSetSize(
		[DllImport("kernel32.dll")]
		public static extern bool SetProcessWorkingSetSize( IntPtr proc, int min, int max );
		#endregion 

		#region FlushMemory() 
		public void FlushMemory() 
		{
			try
			{
				GC.Collect() ;
				GC.WaitForPendingFinalizers() ;
				if(Environment.OSVersion.Platform == PlatformID.Win32NT) 
				{
					SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1) ;
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3146",exp,null,false);
			}
		}
		#endregion 

		#region ClientUI_Load(
		private void ClientUI_Load(object sender, System.EventArgs e)
		{
			
			
			System.Threading.Thread.CurrentThread.Priority=ThreadPriority.Highest;
			System.Threading.Thread.CurrentThread.Name="ClientUI_Thread";
//			Removing Enable/Disable annoatation tools
//			enableWhiteBoardTools(false);
			
			str_cdate=System.DateTime.Now.Date.ToLongDateString();
			
			str_ctime=System.DateTime.Now.ToString("HH:mm");
			//System.DateTime.Now.TimeOfDay.ToString(
			

			try
			{
				#region instantiation initialization
				//_AddTabPageToTabControl= new AddTabPageToTabControlDelegate(AddTabPageToTabControlFn);

				toolbarBtnDesktopSharingDisable.SendToBack();
				CreateContextMenuItems();
				
				m_wndToolTip.SetToolTip(btnCircle,"Ellipse");
				m_wndToolTip.Active = true;
				
				objHook.InstallMouseHook();
				
				CreateDocumentSharingWindowEx = new DelegateCreateDocumentSharingWindow(CreateDocumentSharingWindow);
				DeleteDocumentSharingWindow = new DelegateRemoveDocumentSharingWindow(tabBody.TabPages.Remove);
				DeleteSharingWindow = new DelegateRemoveSharingWindow(tabBody.TabPages.Remove);
				RemoveAudioVideoTabPage = new DelegateRemoveAudioVideoTabPage(RemoveAudioTabPage);
				AddTabPageToMainBody = new AddTabPageToMainBodyDelegate(AddTabPageToMainBodyFunction);
				RemoveTabPageFromMainBody=new RemoveTabPageFromMainBodyDelegate(RemoveTabPageFromMainBodyFunction);
				FnShowChatWindow = new FnShowChatWindowDelegate(ShowChatWindow);				
				this.tabControl1.ShowArrows=false;
				#endregion
				#region set command line arguments
				string[] Args;
				if(bArgument)
				{
					Args = commandArgs ;
				}
				else
				{
					Args=strCmdArg.Split(' ');
				}
				sendTo = chatControl1.testCombo;//.testCombo
				//this.ExceptionLog("sendTo object start pointing chatControl1.testCombo and following values are added <Main Chat>,<All Presenters> & <All Attendees> ::: method :: clientui_load");
				sendTo.Visible = true;
				sendTo.Items.Add("<Main Chat>");
				sendTo.Items.Add("<All Presenters>");
				sendTo.Items.Add("<All Attendees>");
				sendTo.SelectedIndex = 0;
				
				tabPollingGlobal.Control = pollingTabControl;
				
				
				//#if !__DEBUG
				string []firstParam = Args[0].Split('+');
			
				if((firstParam == null) || (firstParam.Length) < 2)				
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Invalid execution of software detected. Aborting...",true,false);
					//MessageBox.Show("Invalid execution of software detected. Aborting...","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
					System.Environment.Exit(1);
					return;
				}
			
				string clientType = firstParam[0];
				string MeetingHasPassword = firstParam[1];
				switch (clientType) //Client Type
				{		
		
					
					case "1":
						
					{
						NetworkManager.getInstance().profile.clientType = ClientType.ClientHost;
						ChangeMenuItemState(mnuSynchronize,true);
						ChangeMenuItemState(mnuSynchronizeD,true);
						NetworkManager.getInstance().profile.clientAccess.annotationRights=true;
						loginFrm.radioHost.Checked = true;
						//this.enableWhiteBoardTools(true);
						this.btnEraserVisible(true);
						this.enableMenus(true);
						flag_checkbox=true;
					}
						break;
					case "2":
					{
						NetworkManager.getInstance().profile.clientType = ClientType.ClientPresenter;
						loginFrm.radioPresenter.Checked = true;
						this.enableMenus(true);
					}
						break;
					case "3":
					{
						loginFrm.radioAttendee.Checked = true;
						NetworkManager.getInstance().profile.clientType = ClientType.ClientAttendee;
						this.enableMenus(false);
					}
						break;
					default:					
					{
						loginFrm.radioAttendee.Checked = true;
						NetworkManager.getInstance().profile.clientType = ClientType.ClientAttendee;				
						IsGuest = true;
						this.enableMenus(false);
					}
						break;
				}	
					
				Info.getInstance().ConferenceID = Args[1]; //Conference ID
				NetworkManager.getInstance().profile.ClientRegistrationId= Convert.ToInt32(Args[2]); // ClientId
				string VerificationURL = Args[3]; //verification URL											
				Invalidate();
				#endregion
				#region url verification 
				int nIndex = VerificationURL.IndexOf(":");
				string serverIP = VerificationURL.Substring(0,nIndex);
				string serverPort = VerificationURL.Substring(nIndex+1,VerificationURL.Length - nIndex-1);

				if(network.profile.clientType == ClientType.ClientAttendee)
				{
					ChangeParticipentsListControl(true);
					//					TabControlsSwitchingHook.bAllowLeftClick = false;	
				}
				listParticipents.Columns[0].Width = 120;
				#endregion 
				#region emoticons navagation
				object oUrl = Application.StartupPath + "\\emoticons\\startpage\\index.htm";
				object o = new object();				
				//startPageControl.axWebBrowser1.Navigate2(ref oUrl,ref o ,ref o,ref o ,ref o);
				#endregion
				#region  open login screen
				if(MeetingHasPassword == "1")
				{					
					/*
					 * loginFrm.ShowDialog();
					if(loginFrm.DialogResult != DialogResult.OK)
					{
						bAskPresenter = false;
						Close();
						return;
					}	
					*/
				}
				this.Password = loginFrm.txtPass.Text;			
				#endregion 
				#region set listBallon title
				m_listBalloon.Title = "WebMeeting";
				m_listBalloon.TitleIcon = BalloonCS.TooltipIcon.Info;
				m_listBalloon.SetToolTip(listParticipents, "Participants in Meeting");
				#endregion 
				#region Are we able to communicate to server or not
				try
				{
					info.ServerIP = serverIP;
					info.ConnectionPort = Convert.ToInt32(serverPort);

					#region navigate to advertise image
					object oUrla = Info.getInstance().WebsiteName +"/default.jpg"; // advertise ur pics at right bottom control
					object oa = new object();				
					this.axWebBrowser2.Navigate2(ref oUrla,ref oa ,ref oa,ref oa ,ref oa);
					#endregion
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3288",exp,null,false);
					//MeetingAlerts alert=new MeetingAlerts();
					//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Unable to communicate with server. Authentication Failed. Disconnecting!!!",true,false);
					//MessageBox.Show("Unable to communicate with server. Authentication Failed. Disconnecting!!!","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
					network.profile.clientType = ClientType.ClientAttendee;
					Close();
					return;				
				}
				//#endif
				#endregion 
			
				
				#region start thread for network connection 
				Thread th=new Thread(new ThreadStart(networkConnectThread));					
				th.Name = "ClientUI_Load Thread: networkConnectThread()";
				th.Start();			
				#endregion				
				#region debug checking				
				/*
				 * #if __DEBUG
								if(loginFrm.radioAttendee.Checked == true)
									network.clientType= ClientType.ClientAttendee;
								else if(loginFrm.radioPresenter.Checked == true)
									network.clientType= ClientType.ClientPresenter;
								else if(loginFrm.radioHost.Checked == true)
									network.clientType= ClientType.ClientHost;

								if(network.clientType == ClientType.ClientAttendee)
									{
										btn_Managecontent.Enabled = false;
										btn_Whiteboard.Enabled = false;
									}

#endif
				*/
				#endregion
				
				#region whiteboard initialization
				try
				{
					whiteBoard.InitializeToSize(tabBody.Width,tabBody.Height); //per2
					whiteBoard.resize(tabBody.Width,tabBody.Height);			//per2		
					
					splitter1.SplitPosition = splitter1.SplitPosition -1;
					WaveLib.WaveFormat fmt = new WaveLib.WaveFormat(4000, 16,1);
					m_Player = new WaveLib.WaveOutPlayer(-1, fmt, 16000, 1);
				}
				catch(Exception)// ee)
				{
				}
				#endregion
				#region Polling tab page layout
				try
				{
					
					this.tabControlWebContent.ShowArrows = true;			
					this.pollingTabControl.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways;										
					//this.pollingTabControl.PositionTop = true;
					this.pollingTabControl.ShowArrows = true;

					this.pollingTabControl.ShowClose = true;										
					this.pollingTabControl.Click += new EventHandler(pollingTabControl_Click);
					this.pollingTabControl.SelectionChanged += new EventHandler(pollingTabControl_SelectionChanged);
					this.pollingTabControl.ClosePressed += new EventHandler(pollingTabControl_ClosePressed);
				}
				catch(Exception ee)
				{
					ee =ee;
				}
				#endregion
				#region menue click events
				IMWindow.EmoticonMenuItem _menuItem;
				//int _count = 0;
				try
				{
					_menuItem = new EmoticonMenuItem(Image.FromFile(Application.StartupPath + "\\emoticons\\1.bmp"));
					_menuItem.Click +=new EventHandler(ToolbarLines_OneWidth);
					_menuItem.Select+=new EventHandler(ToolbarLines_OneWidth);
					cmenu_drawingTools.MenuItems.Add(_menuItem);							
					_menuItem = new EmoticonMenuItem(Image.FromFile(Application.StartupPath + "\\emoticons\\2.bmp"));
					_menuItem.Click +=new EventHandler(ToolbarLines_TwoWidth);
					_menuItem.Select+=new EventHandler(ToolbarLines_TwoWidth);
					cmenu_drawingTools.MenuItems.Add(_menuItem);							
					_menuItem = new EmoticonMenuItem(Image.FromFile(Application.StartupPath + "\\emoticons\\3.bmp"));
					_menuItem.Click +=new EventHandler(ToolbarLines_ThreeWidth);
					_menuItem.Select+=new EventHandler(ToolbarLines_ThreeWidth);
					cmenu_drawingTools.MenuItems.Add(_menuItem);			
			
					_menuItem = new EmoticonMenuItem(Image.FromFile(Application.StartupPath + "\\emoticons\\4.bmp"));
					_menuItem.Click +=new EventHandler(ToolbarLines_FourWidth);
					_menuItem.Select+=new EventHandler(ToolbarLines_FourWidth);
					cmenu_drawingTools.MenuItems.Add(_menuItem);		
				}
				catch(Exception ee)
				{
					ee=ee;
				}				
				window.AddButton("Share My Desktop");
				#endregion				
				#region get WebMeetingVideo.swf stream and put into picture box.
				System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
				try
				{
					Stream myStream = myAssembly.GetManifestResourceStream("WebMeetingClient.WebMeetingVideo.swf");
					Bitmap image = new Bitmap(myStream);
					this.ClientSize = new Size(image.Width, image.Height);
					PictureBox pb = new PictureBox();
					pb.Image = image;
					pb.Dock = DockStyle.Fill;
					this.Controls.Add(pb);	
				}
				catch(Exception)
				{
				}
				#endregion
				
				
				
				
				#region start thread for QATesting 
				/*
				if(Args.Length > 4)
				{
					Thread thQATesting=new Thread(new ThreadStart(funcQATesting));					
					thQATesting.Name = "thQATesting Thread: thQATesting()";
					thQATesting.Start();			
				}
				*/
				#endregion		
				
				
				this.del_closeAppShare=new closeAppShare(call);
				this.del_closeGeneric=new closeGeneric(CallGeneric);
				this.del_closeAll=new closeAll(Call_CloseAllTabs);
				Flexalert=new MeetingAlerts();
				browserControl=new browser();
				Lock_MsgPool_SendAnnotation=new object();
				Lock_MsgPool_Annotation=new object();

				
			}
			catch(Exception ee)
			{
				ShowExceptionMessage(" Loading of GUI has encountered an exception. " + ee.Message);
			}
			#region new well come page
			nWelcomePage.Title="Welcome";
			nWelcomePage.Control=new NewBrowserWnd();
			try
			{
				tabBody.TabPages.Add(nWelcomePage);					
			}
			catch(Exception ee )//ee)
			{
				ee = ee;
			}
			#endregion 
			/*
			#region download emoticons
						try
						{
							thDLEmoticons = new Thread(new ThreadStart(downLoadEmoticons));
							thDLEmoticons.Name = "downLoad Emoticons from web server thread";
							thDLEmoticons.Start();
						}
						catch(Exception ex)
						{
							ex=ex;
						}
			#endregion
						*/




				








		}		
		
		#endregion 


		# region funcQATesting() 
		void funcQATesting()
		{
			try
			{
				Thread.Sleep(60000); // should be dynamic ...

				XmlTextReader tr = new XmlTextReader("wmscript.xml");			
			
				while(tr.Read())
				{
					try
					{

						if(tr.Name == "command" && tr.NodeType == XmlNodeType.Element && tr.AttributeCount > 0)
						{
							if(tr.GetAttribute("name") == "open" )
							{
								openControl(tr.GetAttribute("tool"));
								Thread.Sleep( int.Parse(tr.GetAttribute("timedelay")));
							}
						}
						else
						{
						
						}
					}
					catch(Exception exp)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>3430",exp,null,false);
						//MessageBox.Show(ex.Message, "funcQATesting");
					}
				}	
				
				
				//SendMessage(this.Handle,WM_OPEN_WHITEBOARD,new IntPtr(),new IntPtr());

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>3500",exp,null,false);
				//MessageBox.Show(ex.Message, "funcQATesting");
			}
		
		}
		#endregion 
		
		#region QAtesting code
		void openControl(string tool)
		{
			try
			{				
				switch(tool)
				{
					case "whiteboard":
					{
						GenerateClick("btn_Whiteboard");
						/*SendMessage(this.Handle, WM_PARENTNOTIFY, WM_LBUTTONDOWN, new IntPtr(0x0200085));    
						SendMessage(this.Handle, WM_MOUSEACTIVATE, 0x020836, new IntPtr(0x02010001));    
						*/
						
						/*SendMessage(btn_Whiteboard.Handle, WM_LBUTTONDOWN, 0,10 ); 
						
						SendMessage(btn_Whiteboard.Handle, WM_LBUTTONUP, 0, 10); 
						SendMessage(hwnd , BM_SETSTATE, 1, IntPtr.Zero);
*/
						//SendMessage(this.Handle , WM_LBUTTONDOWN, 0,IntPtr.Zero ); 
						//SendMessage(hwnd ,WM_OPEN_WHITEBOARD,new IntPtr(),new IntPtr()); 
						//SendMessage(findHandler("whiteboard"),WM_OPEN_WHITEBOARD,new IntPtr(),new IntPtr()); // call goes to handler winproc
					}
						break;
					case "document_sharing":
					{
						//SendMessage(this.Handle,WM_OPEN_WHITEBOARD,new IntPtr(),new IntPtr()); 
					}
						break;
				}
			}
			catch(Exception ex)
			{

				MessageBox.Show(ex.Message, "funcQATesting");
			}
		}
		
		
		public void GenerateClick(string button) // send button text
		{
			int handle = Win32.USER32.FindWindow(null, info.ConferenceName);

			IntPtr hWnd = new IntPtr(handle);
			int Child = Win32.USER32.GetWindow(hWnd, Win32.USER32.GW_CHILD) ;
			IntPtr ptrChild = new IntPtr(Child);
			StringBuilder builder = new StringBuilder();
			while(!ptrChild.Equals(IntPtr.Zero))
			{
				int target = Win32.USER32.FindWindowEx(hWnd, IntPtr.Zero, "", button);
				Child = Win32.USER32.GetWindow(ptrChild, Win32.USER32.GW_HWNDNEXT) ;
				ptrChild = new IntPtr(Child);
							
				Win32.USER32.GetWindowText(ptrChild, builder, 56);

				string name = builder.ToString();			

				if( name == button)
				{
					Win32.USER32.SendMessage(btn_Whiteboard.Handle, Win32.USER32.WM_LBUTTONDOWN, 0, IntPtr.Zero); 
					Win32.USER32.SendMessage(btn_Whiteboard.Handle, Win32.USER32.WM_LBUTTONUP, 0, IntPtr.Zero); 
					Win32.USER32.SendMessage(btn_Whiteboard.Handle, Win32.USER32.BM_SETSTATE,1 , IntPtr.Zero);
				}			
				
			}
		}


		IntPtr findHandler(string tool)
		{
			IntPtr toolHandler = new IntPtr(3) ;
			try
			{
				switch(tool)
				{
					case "whiteboard":
					{
						toolHandler = whiteBoard.Handle;  // check whiteBoard.Handle exists or not
					}
						break;
					case "document_sharing":
					{
					}
						break;
				}

			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "funcQATesting");
			}
			return toolHandler ;
		}
		#endregion

		#region downLoadEmoticons()
		private void downLoadEmoticons()
		{
			try
			{
				DirectoryInfo di = new DirectoryInfo(Application.StartupPath + "\\Emoticons");
				if(! di.Exists )
				{
					while(Info.getInstance().WebsiteName == null)
					{
						Thread.Sleep(1000);
					}

					System.Net.WebClient client = new WebClient();
					
					client.DownloadFile(Info.getInstance().WebsiteName + "/application/Emoticons.zip", Application.StartupPath + "\\Emoticons.zip");
					ICSharpZip.UnzipZip(  Application.StartupPath + "\\Emoticons.zip",  Application.StartupPath + "\\Emoticons" );
					
					/*chatControl ct = (chatControl)tabPageChat.Control;
					ct.LoadEmoticonFile();
					*/
					chatControl1 = chatControl.getInstance();
					chatControl1.LoadEmoticonFile();

					client.Dispose();
					//thDLEmoticons.Abort();
				}
				di = null;
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>3650",exp,null,false);
				
			}
		}

		#endregion 

		#region RemoveAudioTabPage(int x)
		public void RemoveAudioTabPage(int x)
		{
			try
			{
				tabControl1.TabPages.RemoveAt(2+x);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3660",exp,null,false);
			
			}
		}
		#endregion 

		# region AddRecentlyUsedPresentations()
		public void AddRecentlyUsedPresentations()
		{

			try
			{
				Utility.NiceMenu.NiceMenu parentMenu = Client.ClientUI.getInstance().myNiceMenu.SearchNiceMenuItem("Recent Presentations");
				if(parentMenu != null)
				{
					RegistryKey SUBKEY; 
					RegistryKey TAWKAY = Registry.CurrentUser;
					for(int z = 15; z > 8 ; z --)
					{
						string subKey = @"Software\Microsoft\Office\" + z.ToString() + @".0\PowerPoint\Recent File List";
						SUBKEY = TAWKAY.OpenSubKey(subKey); 
						if(SUBKEY != null)
						{
							for(int i = 0 ; i < SUBKEY.ValueCount ; i ++)				    
							{
								Utility.NiceMenu.NiceMenu myMenu = new Utility.NiceMenu.NiceMenu();				
								myMenu.Click += new EventHandler(presentationsMenu_Click);
								object obj = SUBKEY.GetValue("File" +(i+1).ToString());
								if(obj!=null)
								{
                        					
									myMenu.Text = obj.ToString();
									parentMenu.MenuItems.Add(myMenu);
									recentlyUsedPresentationsArray.Add(myMenu.Text);
								}
					
							}
							break;
						}
					}
				
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>3700",exp,null,false);
			}
		}

		#endregion 

		#region ClientUI_Closing(
		public void ClientUI_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{



			PresenterExitForm frm = new PresenterExitForm();
			try

			{
				
						
				#region post Questionn 
				if(network.profile.clientType != ClientType.ClientHost)
				{
					if(sender!=null && e!=null)
					{
					
					if(network.IConferenceRoom.IfPostMeetingquestions(Convert.ToInt32(network.profile.ConferenceID),0,0))
						{
						  Process.Start("IExplore.exe", Info.getInstance().WebsiteName + "/application/postmquestions_end.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId + "&usertype=" + NetworkManager.getInstance().profile.clientType.ToString()+"\n Thats the end....."  );
						//Process.Start("IExplore.exe", "http://soerwa.hopto.org/Compassnav + "/application/postmquestions_end.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId + "&usertype=" + NetworkManager.getInstance().profile.clientType.ToString()+"\n Thats the end....."  );
						}
					}
					
					
				}
				#endregion

				#region show presenter exit form to exit
				bool bCloseConference = false;
				if(bAskPresenter)
				{
					if(network.profile.clientType == ClientType.ClientHost)
					{
						if(!isAlreadyhClosed)
						{
							if(sender==null && e==null)
							{
								bCloseConference=false;
							}
							else
							{							
							
								if(DialogResult.Cancel == frm.ShowDialog())
								{
									e.Cancel = true;
									return;
								}					
								
								bCloseConference=frm.close;
															
							}
							//closeConference_closeAllParticipants(); // close other all participants
						}

					}
				
					this.ChangeResolution(false);
					
				}

				#endregion

				#region terminate to whiteboard, chat control
				
				try
				{
					whiteBoard.Terminate();
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3753",ex,null,false);
				}
				try
				{
					chatControl1.Terminate();
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3761",ex,null,false);	
				}

				try
				{
					for(int i =0 ; i < 	chatTabControl.TabPages.Count ; i++)
					{				
						Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)chatTabControl.TabPages[i];
						System.Type a=tabPage.Control.GetType();
						if(a==chatControl.getInstance().GetType())
						{
							chatControl pUserChat = (chatControl)tabPage.Control;
							pUserChat.Terminate();
						}						
					}
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3779  client closing method",ex,null,false);
				}

				#endregion

				#region video closing
				try
				{
					if(videoEnabled)
					{								
					}
				}
				catch(Exception )//ee)
				{

				}

				try
				{
					//	VideoCompressor.ClearGraph(0);	
					//	VideoCompressor.DestroyCodec();
				}			
				catch(Exception ee)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage(ee.Message);
					//MessageBox.Show(ee.Message);
			
				}
				#endregion

				#region document sharing termication
				try
				{
					for(int i = 0 ; i < arraydocumentSharing.Count ; i++)
					{	
					
						Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
						documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
									
						if(!control.IsAttendee)
						{
							DocumentSharing msg = new DocumentSharing();
							msg.senderProfile = this.network.profile;
							msg.SenderID = this.network.profile.ClientId;
							msg.DocumentSharingSessionId = control.sessionID;
							msg.sessionID = control.sessionID;
							msg.bEnabled = false;
							msg.ConferenceID = network.profile.ConferenceID ;
							network.SendLoadPacket(msg);
						}
						/////////////////////////////////////////////
						control.Terminate();					
					}
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3839",ex,null,false);
				}

				#endregion

				#region application sharing termination
				////////////////////////////////////////////////
								
				//ApplicationSharing app = new ApplicationSharing();  // for checking purpose
				//ApplicationSharing app = (ApplicationSharing)tabBody.SelectedTab.Control;				
				try
				{				
					for(int i=0; i < tabBody.TabPages.Count;)
					{
						if(tabBody.TabPages[i].Control.GetType().Name == "ApplicationSharing")
						{					
							ApplicationSharing app = (ApplicationSharing)tabBody.TabPages[i].Control;
							if(app.controlOwnerId == network.profile.ClientId)
							{											
								
								//Added by Zaeem
								app.btnShare_Click(null,null);
								app.QuickAppShareExit();/*for releasing shared window handle and making its parent '0'*/
								myAppShareStarted= false;
								tabBody.TabPages.Remove(tabBody.TabPages[i]);
								ApplicationSharingTabPagesArray.Remove(i);
								app.TerminateEx();								
							}
							else
							{
								myAppShareStarted= false;
								tabBody.TabPages.Remove(tabBody.TabPages[i]);
								ApplicationSharingTabPagesArray.Remove(i);
								app.TerminateEx();			
							}						
						}
						else if( tabBody.TabPages[i].Title == "WhiteBoard" )
						{/*for closing whiteboard on // side.
						  */
							try
							{
								if(network.profile.clientType==ClientType.ClientHost)
								{																
									whiteBoard.btn_whtLogg.Visible=false;
									WhiteboardMessage msg=new WhiteboardMessage();
									msg.MessageType=(ushort)MessageObjectType.MsgHideWhiteBoard;
									msg.ConferenceID = network.profile.ConferenceID;
									msg.SenderID = network.profile.ClientId;
									network.SendLoadPacket(msg);									
								}
								i++;
							}
							catch(Exception exp)
							{

								WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3800",exp,null,false);
								
							}
					
						}
						else
							i++;
					}
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3907",ex,null,false);
				}							
				///////////////////////////////////////////////
				#endregion

				#region browser control termination
				try
				{
					for(int i = arrayWebControl.Count-1 ; i >= 0 ; i--)
					{
						Crownwood.Magic.Controls.TabPage tabBrowser = (Crownwood.Magic.Controls.TabPage)arrayWebControl[i];
						browser browserControl = (browser)tabBrowser.Control;
						//if(!browserControl.IsAttendee)
						if(this.IfIamthePresenter())
						{
							browserControl.SendCloseMessage();
						
						}
						browserControl.Terminate();
						//browserControl.Dispose();
						// Added By zaeem for the image based websharing cleaning purpose
						//browserControl.CleanandClear();

						tabBrowser.Dispose();
						GC.Collect();
						GC.WaitForPendingFinalizers();
					}
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3925",ex,null,false);
				}
				#endregion
				#region audio closing
				try				
				{				
					DisableMyAudio(null);

				}
				catch(Exception exp)// ee)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3910",exp,null,false);
				}
				#endregion		
		
				#region polling termication			
				this.Hide();
				#endregion
				#region evaluation termination
				try
				{
					for(int i = 0 ; i < evaluationWindowsOfAttendee.Count; i++)
					{
						WebMeeting.Polling.Polling evaluation = (WebMeeting.Polling.Polling)evaluationWindowsOfAttendee[i];
						if(evaluation != null)
						{
							evaluation.attendeeForm.changeInterfaceToEvaluation();
							evaluation.ShowAttendeeForm(evaluation.thisPollType);
						}
					}
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3900",ex,null,false);
				}
				#endregion
				#region download thread abort and disable to tab controls
				try
				{
					if(downloadThread!=null)
					{
						if(downloadThread.IsAlive)	
						{
							downloadThread.Abort();
						}						
					}
				}
				catch(Exception exp)// ee)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3940",exp,null,false);

				}
				panelQuickLaunch.Enabled = false;
				whiteBoard.Enabled = false;
				chatControl1.Enabled = false;
				tabBody.Enabled = false;
				tabControl1.Enabled = false;
				
				if(bAskPresenter)
				{
					if(bCloseConference)
					{
						ControlMessage msg = new ControlMessage(ControlCode.PresenterClosedConference);
						msg.Parameter = network.profile;
						msg.SenderID = network.profile.ClientId;
						network.SendLoadPacket(msg);
						Thread.Sleep(1000);
						((IConferenceRoom)network._dispatcher.TransparentProxy).CloseConference(msg);						
						closeConference_closeAllParticipants(); // close other all participants
					}

				}

				#endregion
				this.webser.EndMeetingLog();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>3980",exp," Closing of GUI has encountered an exception. " + exp.Message.ToString(),true);
				//ShowExceptionMessage(" Closing of GUI has encountered an exception. " + ee.Message);
			}
			finally
			{
				if(e.Cancel==false)
				{
				
					#region delete temporary files and exit from whole application
					System.Diagnostics.Process newProcessToDeleteSharedDocuments = new System.Diagnostics.Process();
					newProcessToDeleteSharedDocuments.StartInfo = new System.Diagnostics.ProcessStartInfo(System.Windows.Forms.Application.StartupPath + "\\DeleteSharedDocuments.exe");
					network.Close();
					this.Dispose(true);
					//System.IO.File.Create(Application.StartupPath + "\\flag.txt");
					newProcessToDeleteSharedDocuments.Start();				
				}
				#endregion
			}
			try
			{
				ExceptionLogWrite.CloseLogging();
				this.Visible=false;

				# region Minuts of meeting section
				
				if(network.profile.clientType==ClientType.ClientHost)
				{
					if(frm.chk_MM.Checked==true)
					{
						pb.Show();
						pb.Updateprogress();
					}
				}
				#endregion 

				System.Environment.Exit(0);
			}
			catch(Exception ex)
			{
				//MessageBox.Show(ex.Message.ToString());
				ex=ex;
			}
		}
		#endregion		

		bool isAlreadyhClosed = false;

		#region ConferenceClosed()
		public void ConferenceClosed()
		{
			isAlreadyhClosed = true;

			for(int i = arrayWebControl.Count-1 ; i >= 0 ; i--)
			{
				Crownwood.Magic.Controls.TabPage tabBrowser = (Crownwood.Magic.Controls.TabPage)arrayWebControl[i];
				browser browserControl = (browser)tabBrowser.Control;
				browserControl.AttendeeInterface();
			}

			//if(audioEnabled)
			//	audioThread.Abort();

			evaluationWindowsOfAttendee.Clear();			
			
			whiteBoard.Enabled = false;
			chatControl1.Enabled = false;
			tabBody.Enabled = false;
			tabControl1.Enabled = false;		
			try
			{
				ClientType type = network.profile.clientType;
				network.profile.clientType = ClientType.ClientAttendee;
				UpdateMainMenuAccess();    	
				network.profile.clientType = type;
				btn_Snapshot.Enabled = false;
				btn_Webshare.Enabled = false;
				btn_Whiteboard.Enabled = false;


			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>4020",exp,null,false);

			}			
			
			panel2.Enabled = false;
			myNiceMenu.Enabled = false;			
			this._statusBar.LeftMessage="Conference has been Closed";
			network.Close();
			
			
		}
		#endregion 

		
		#region Network Layer Connect Thread
		public string Password;
		public void SetConnectionProperties(string hostpass,string meet_id,string userid,string ipport)
		{
			try
			{
				string[] temp=ipport.Split(':');
				info.ServerIP=temp[0] ;
				info.ConnectionPort=Convert.ToInt32(temp[1]);
				string[] temp2=hostpass.Split('+');
				switch (temp2[0]) //Client Type
				{				
					case "1":
						NetworkManager.getInstance().profile.clientType = ClientType.ClientHost;
						loginFrm.radioHost.Checked = true;
						break;
					case "2":
						NetworkManager.getInstance().profile.clientType = ClientType.ClientPresenter;
						loginFrm.radioPresenter.Checked = true;
						break;
					case "3":
						loginFrm.radioAttendee.Checked = true;
						NetworkManager.getInstance().profile.clientType = ClientType.ClientAttendee;
						break;
					default:					
						loginFrm.radioAttendee.Checked = true;
						NetworkManager.getInstance().profile.clientType = ClientType.ClientAttendee;				
						IsGuest = true;
						break;
				}						
				Info.getInstance().ConferenceID = meet_id; //Conference ID
				NetworkManager.getInstance().profile.ClientRegistrationId= Convert.ToInt32(userid); // ClientId
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4100",exp,null,false);
			}
		}


		private void networkConnectThread()
		{
			try
			{
				do
				{
					whiteBoard.Enabled = false;		
					
					//	this._statusBar.LeftMessage="Connecting to server";
					//statusBar1.Text = "Connecting to server";
					panelQuickLaunch.Enabled = false;
					
					if(!network.Connect(info.ServerIP,info.ConnectionPort,info.UserName, this.Password,info.ConferenceID,info.UserEmail,this.IsGuest))
					{
						if(this.PasswordError)
						{
							loginFrm.lblInvalid.Visible = true;							
							loginFrm.txtPass.Text = "";
							loginFrm.ShowDialog();
							if(loginFrm.DialogResult != DialogResult.OK)
							{
								bAskPresenter = false;
								Close();
								return;
							}		
							this.Password = loginFrm.txtPass.Text;			
							continue;
						}

						bAskPresenter = false;
						this.ChangeResolution(false);
						Close();
						return;
					}
					break;
				}while(true);
				
				panelQuickLaunch.Enabled = true;
				UpdateMainMenuAccess();
				//statusBar1.Text = "Connected to server";

				FlushMemory(); // per1 Kamran
				Thread thFlushMemory = new Thread(new ThreadStart(overallFlushMemory)); //per2
				thFlushMemory.Name = "overallFlushMemory Thread";//per2
				thFlushMemory.Start();//per2

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4150",exp,null,false);
				//ShowExceptionMessage(" Network Connection thread has encountered an exception. " + ee.Message);
			}
		}

		#endregion Flush Memory , free resoures

		#region overallFlushMemory()
		public void overallFlushMemory()//per2
		{
			try
			{
				while(true)
				{
					if(Environment.WorkingSet > 25000000)// maximum 25 mb
					{
						FlushMemory();
					}
					Thread.Sleep(500);
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4155",exp,null,false);			
			}
		}
		#endregion 

		#region imageDownloadThread()
		public void imageDownloadThread()
		{
			
			try
			{
				string ext = Path.GetFileName(ImageURL);			
				string local = Application.StartupPath + "\\" + ext ;
				if(File.Exists(local))
					File.Delete(local);
				ImageURL = ImageURL;
				//MessageBox.Show(ImageURL);
				if(DownloadFile(ImageURL,local) > 1)
				{
					//pictureBoxCoBranding.Image = Image.FromFile(local);
				}
				ImageURL = ImageURL;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4200",exp,null,false);			
			}
		}
		#endregion 

		Thread downloadThread = null;

		#region ConnectionDisabled()
		public void ConnectionDisabled()
		{
			isAlreadyhClosed = true;
		
			//if(audioEnabled)
			//	audioThread.Abort();
			evaluationWindowsOfAttendee.Clear();
			//MessageBox.Show("==>  Connection Disable line no 4335 ");
			whiteBoard.Enabled = false;
			chatControl1.Enabled = false;
			tabBody.Enabled = false;
			tabControl1.Enabled = false;		

			try
			{
				ClientType type = network.profile.clientType;
				
				network.profile.clientType = ClientType.ClientAttendee;
				UpdateMainMenuAccess();    	
				
				if (type==ClientType.ClientHost)
				{
					network.profile.clientType = type;
				}

				/*
				if (type==ClientType.ClientPresenter)
				{
					for(int i = 0; i < ClientUI.getInstance().arrayParticipents.Count; i++)
					{								
						ClientProfile client = (ClientProfile)ClientUI.getInstance().arrayParticipents[i];
											
						if(network.profile.ClientId== client.ClientId)
						{	
							//ClientUI.getInstance().arrayParticipents.Remove[i];
							client.clientType=ClientType.ClientAttendee;


						}
					}
				}
				*/


				btn_Snapshot.Enabled = false;
				btn_Chat.Enabled=false;
				btn_Webshare.Enabled = false;
				btn_Whiteboard.Enabled = false;


 
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4105",exp,null,false);

			}						
			panel2.Enabled = false;
			myNiceMenu.Enabled = false;			
			this._statusBar.LeftMessage="Reconnecting....";
			//MeetingAlerts alert=new MeetingAlerts();
			//alert.ShowMessage(1,"Connection lost to server. Reconnecting Please wait...",true,false);
			//MessageBox.Show("Connection lost to server. Reconnecting Please wait...","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
			
		}
		#endregion 

		#region ConnectionEnabled()
		public void ConnectionEnabled()
		{
			
			
			try
			{
				//whiteBoard.Enabled = true; //per1
				//UpdateMainMenuAccess();     //per1 it is repeating 
				//thisInstance.Text = "Compassnav Meeting - " + info.ConferenceName;			
				//				downloadThread = new Thread(new ThreadStart(imageDownloadThread)); // for checking purposes
				//				downloadThread.Start();// for checking purposes
				_statusBar.comboMood.SelectedIndex = 2;
				_statusBar.comboMood.Visible=false;
				//if(network.profile.clientType != ClientType.ClientAttendee)
				//	AddRecentlyUsedPresentations();				
				this._statusBar.synchButton.Visible = (network.profile.clientType == ClientType.ClientHost);
				//this._statusBar.synchButton.Visible =false;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4135",exp,null,false);
				//ShowExceptionMessage(ee.Message + " Connection Enabled");
			}

			try
			{
				isAlreadyhClosed = false;
			
				//if(audioEnabled)
				//	audioThread.Abort();

				evaluationWindowsOfAttendee.Clear();			
			
				whiteBoard.Enabled = true;
				chatControl1.Enabled = true;
				tabBody.Enabled = true;
				tabControl1.Enabled = true;		
				try
				{
				
					UpdateMainMenuAccess();    						
					btn_Snapshot.Enabled = true;
					btn_Chat.Enabled=true;
					//btn_Webshare.Enabled = true;
					
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4161",exp,null,false);

				}						
				panel2.Enabled = true;
				myNiceMenu.Enabled = true;			
				this._statusBar.LeftMessage="Connected  ";
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4171",exp,null,false);
			}

		}
		#endregion

		#region StartEvaluation()
		public void StartEvaluation()
		{
			CreateNewPollingWindow(null,null,true);
		}
		#endregion 

		#region OpenRecorder()
		public void OpenRecorder()
		{
			try
			{
				if(!screenCaptureEnabled)
				{					
					panelRecordingToolbar.Visible = true;
					panelRecordingToolbar.BringToFront();

					//toolbarRecording.Visible = true;
					//	toolbarRecoding.Visible =true;
					//_toolBarManager.ShowControl(recordingToolbar,true);
					screencaptureControl.Left = 10;
					screencaptureControl.Top = 10;
					tabPageScreenCapture.Title = "Screen Capture";
					//tabBody.TabPages.Add(tabPageScreenCapture);
					//tabBody.SelectedTab = tabPageScreenCapture;					//
					screenCaptureEnabled= true;
				}
				else
					///tabBody.SelectedTabEx = tabPageScreenCapture;					
					/////panelRecordingToolbar
					//tabBody.TabPages.Remove(tabPageScreenCapture);
				{
					panelRecordingToolbar.Visible=false;
					screenCaptureEnabled=false;
				}
			}
		
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4211",exp," Open Recorder has encountered an exception. " + exp.ToString(),true);			
				
			}
		}
		#endregion 

		#region Main Menu and Context Menu Handlers
		public void mnuGestioneEventoClick(object sender, System.EventArgs e)
		{
			try
			{
				//MessageBox.Show("=========>  4008");
				MeetingAlerts alert=new MeetingAlerts();
				NiceMenu item = (NiceMenu)sender;
				if(item.Parent!=null)
				{
					NiceMenu itemParent=(NiceMenu)item.Parent;
						
					switch(itemParent.Text)
					{
							
						case "Print":
						{
							switch(item.Text)
							{
								case "Notes":
									this.notesControl.PrintIt();
									break;
							}
						}break;
						case "Chat":					
						{
							NiceMenu itemGrandParent=(NiceMenu)itemParent.Parent;
							switch(itemGrandParent.Text)
							{
								case "Save":							
								{
									switch(item.Text)
									{
										case "Public Chat":
											this.chatControl1.Save();
											break;										
										default:
											for(int i = 0 ; i < chatWindowsArray.Count; i ++)
											{
												ClientProfileWrapper CP = (ClientProfileWrapper ) chatWindowsArray[i];
												if(CP.tabPage.Title == item.Text)
												{
													chatControl ChatControl = (chatControl)CP.tabPage.Control;
													ChatControl.Save();
												}
											}
											break;
									}
									break;
								}
								case "Save As":
								{
									switch(item.Text)
									{
										case "Public Chat":
											this.chatControl1.SaveAs();
											break;
										case "Notes":
											this.notesControl.SaveAs();
											break;
									}
									break;
								}									
								case "Print":
								{
									switch(item.Text)
									{
										case "Public Chat":
											this.chatControl1.PrintIt();
											break;				
										case "Notes":
											this.notesControl.PrintIt();
											break;
									}
								}
									break;
							}
							break;
						}
						case "Save As":
						{
							switch(item.Text)
							{							
								case "Whiteboard":
									this.whiteBoard.SaveAs();
									break;
								case "Notes":
									
									break;
								case "":
									break;								
							}
							break;
						}
						
						case "View":
						{
							switch(item.Text)
							{
								case "Zoom In":
									whiteBoard.ZoomIn();
									break;
								case "Zoom Out":
									whiteBoard.ZoomOut();
									break;		
								case "Zoom By":
									break;
								case "Fit in Viewer":
									break;
								case "Fit in Width":
									break;
							}
							break;
						}
						case "Tools":
						# region Tools menu items
						{
					
							switch(item.Text)
							{
								
								
								case "Record":
								{
									//OpenRecorder();
									try
									{
										fm=new Screen_Capture.frm_Rec();
										fm.Show();
									}
									catch(Exception exp)
									{
										WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7646",exp,null,false);			
									}



								}	
									break;

								case "Poll ":
									{

									this.CloseMsg();
									CreateNewPollingWindow(null,null,false);
									}
									break;

								case "Invite":
									{
									InviteForm form = new InviteForm();
									form.Show();
									}
									break;


								case "Q and A":
									{


										try
										{
											if(!(this.btn_Chat.Enabled && this.btn_Snapshot.Enabled))
											{
											alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"The operation can't be performed as either Network is disconnected or Application is still loading....",true,false);
											return;
											}
				
											//if((network.profile.clientType == ClientType.ClientPresenter) || (network.profile.clientType == ClientType.ClientHost))
											
											if(this.IfIamthePresenter())
											{
												try
												{
														frmQAP.Controls.Add(qaPresenter);
														frmQAP.Show();
									
												}
												catch(Exception exp)// ee)
												{
													WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9725",exp,null,false);
												}
											}
											else
											{
												try
												{
									
													frmQAA.Controls.Add(qaAttendee);
													frmQAA.Show();
												}
												catch(Exception exp)// ee)
												{
													WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9735",exp,null,false);
												}
											}
					
										}
										catch(Exception exp)
										{
											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs Question Answer line==>9742",exp,null,false);
										}
										
				
									}
							break;


								case "Synchronize":
								{
									HostCloseAllTabs();
								}
									break;

								case "Synchronize tools":
								{
									SyncronizeDrawingTools();
								}
									break;

								case "Message or Greetings":
									if(listParticipents.SelectedItems.Count < 1) 
										return;
									
									SendGreetings(listParticipents.SelectedItems[0].Index);
									break;
								case "Record Audio":
									break;
								case "Recorder Settings":
									//screencaptureControl.SetFile();
									
									break;
								case "Playback Recording":
									WebMeeting.Client.Screen_Capture.Playback obj = new WebMeeting.Client.Screen_Capture.Playback();
									obj.ShowDialog();
									break;
								case "Snapshot":
									TakeSnapShot();
									break;
							}
					
							break;
						}
							#endregion

						
						case "&Save":
							#region
						{
							
							switch(item.Text)
							{
									
								case "&Whiteboard  ":
									MM_Controller.Send_MinutsofMeetingMsg_Whiteboard();

									break;
								case "&Application  ":
									MM_Controller.Send_MinutsofMeetingMsg_Application();

									//MM_Controller.Appshare_Snapshot();
									break;
								case "Web &Browser  ":
									MM_Controller.Send_MinutsofMeetingMsg_WebBrowser();
									break;
								case "Notes":
									this.notesControl.Save();									
									break;
							}		
							break;						
						}
					
						#endregion
						case "Share":
						{
							
							switch(item.Text)
							{
								case "Whiteboard ":
									
									this.ShareWhiteboard();
									break;

								case "Presentation or Document":
									ShareMyDocument("");
									break;
								
								case "Desktop":
									if(listParticipents.SelectedItems.Count <1)
									{
										alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Please select a user ",true,false);
										//MessageBox.Show("Please select a user ","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
										return;
									}
									
									StartDesktopSharing(listParticipents.SelectedItems[0].Index);	
									break;
								case "Entire Desktop":
									((NiceMenu)sender).Text= "Close Entire Desktop";
									(myNiceMenu.SearchNiceMenuItem("Application")).Enabled = false;
									StartEntireDesktopSharing();	
									break;
								case "Close Entire Desktop":
									((NiceMenu)sender).Text = "Entire Desktop";
									CloseEntireDeskHostToClient(false);
									break;
								
								case "Application ":
									//(myNiceMenu.SearchNiceMenuItem("Entire Desktop")).Enabled = false;
									StartApplicationSharingEx();
									break;
								case "Web Browser":
									////true);
									ShareMyBrowser();
									break;

								case "Audio":
									ShareMyAudio();
									break;
								
								case "Video":
									ShareMyCamera();
									break;

								case "Evaluation":
									StartEvaluation();
									break;
							}
							break;
						}
						case "Edit":
						{
							WhiteboardMessage msg = new WhiteboardMessage();							
							
							switch(item.Text)
							{
								case "Undo":
									whiteBoard.Undo();
									msg.tool = (ushort)WhiteboardToolCode.Undo;
									msg.ConferenceID = network.profile.ConferenceID;
									network.SendLoadPacket(msg);
									
									break;
								case "Redo":
									whiteBoard.Redo();
									msg.tool = (ushort)WhiteboardToolCode.Redo;
									msg.ConferenceID = network.profile.ConferenceID;
									network.SendLoadPacket(msg);
									break;
							}
							break;
						}
						case "Help":
						{
							switch(item.Text)
							{
								case "About Compassnav meeting":
								{
									aboutCompassnav();	
									break;
								}
								case "Updates history":
								{
									Process.Start("notepad.exe",Application.StartupPath + @"\Updates.txt");
									break;
								}
								case "Meeting details":
									MessageBox.Show("          Meeting Number :          "+Info.getInstance().ConferenceID.ToString()+"\n"+"          Host                   :          "+HostName()+"\n"+"          Start date           :          "+str_cdate+"\n"+"          Start time            :          "+str_ctime);
									
									break;

								case "Test network":
									WebMeeting.Client.NetworkDiagonstic.TestNetwork nd=new NetworkDiagonstic.TestNetwork();
									nd.Show();
									break;
							}
							
							break;
						}
					}
				}
			
	
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4542",exp,null,false);			
				//ShowExceptionMessage(" Menu handler has encountered an exception. " + ee.Message);
			}
			
		}		

		
		#endregion		

		#region "Hostname"
		public string HostName()
		{

			# region for Loop
			ClientProfile profile1;
			for(int i=0;i<Client.ClientUI.getInstance().arrayParticipents.Count;i++)
			{
			
				profile1=(ClientProfile)arrayParticipents[i];
				
				if(profile1.clientType.ToString().Equals("ClientHost"))
				{
					return profile1.Name.ToString();			
				}
					 
 				
			}// end For Loof
			# endregion 
		return "Currently there is no host in the meeting";			
		
		}
		#endregion		
		
		#region "Presentername"
		public string PresenterName()
		{

			# region for Loop
			ClientProfile profile1;
			for(int i=0;i<Client.ClientUI.getInstance().arrayParticipents.Count;i++)
			{
			
				profile1=(ClientProfile)arrayParticipents[i];
				
				if(profile1.clientType.ToString().Equals("ClientHost"))
				{
					if(ifHostisPresenter())
						return profile1.Name.ToString();			
				}
				else if (profile1.clientType.ToString().Equals("ClientPresenter"))
				{
				
				
				}
					 
 				
			}// end For Loof
			# endregion 
			return "Currently there is no host in the meeting";			
		
		}
		#endregion		

		#region OpenChatWindow(bool btrue)
		public void OpenChatWindow(bool btrue)
		{
			try
			{
				
				chatControl userChatnew;                                
				//chatControl 	
				if(listParticipents.SelectedItems.Count < 1)
					return;
				ClientProfile userProfile = (ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index];
				if(userProfile.ClientId == network.profile.ClientId)
					return;

				for(int i = 0 ; i < chatWindowsArray.Count ; i++)
				{
					ClientProfileWrapper CP2 = (ClientProfileWrapper)chatWindowsArray[i];
					if(CP2.nUniqueID ==  userProfile.ClientId)
					{
						chatTabControl.SelectedTab= CP2.tabPage;	
						userChatnew=(chatControl)CP2.tabPage.Control;

						//chatControl userChatWindow = new chatControl();                                
						//change made by junaid
						//userChatWindow.txtBoxPreviewChat.Clear();
							
						string str = Client.ClientUI.getInstance().chatControl1.txtBoxPreviewChat.Rtf;					
						
						//chatTabControl.SelectedTab = chatWindow;
						
						IMChatMessage msg=new IMChatMessage();
						msg.senderID = network.profile.ClientId;					
						msg.RecipientId = userProfile.ClientId;
						msg.MessageType = (ushort)MessageObjectType.MsgIMChat;           				
						msg.IMMessage=chatControl1.txtBoxPreviewChat.Rtf;					
						msg.sender=network.profile.Name;
						network.SendLoadPacket(msg);
						
						userChatnew.messagePool.Add(msg);
						
						
						
						return;
					}
				}			

				if(btrue)
				{
					if(chatControl1.txtBoxPreviewChat.Text.Length <1 )
					{
						return;
					}
				}
				
	            
				ClientProfileWrapper CP = new WebMeeting.Client.ClientUI.ClientProfileWrapper();
				chatControl userChatWindow = new chatControl();                                
				
				userChatWindow.nRecipientID = userProfile.ClientId;
				CP.nUniqueID = userProfile.ClientId;
				Crownwood.Magic.Controls.TabPage chatWindow = new Crownwood.Magic.Controls.TabPage();
				chatWindow.Title = userProfile.Name;
				chatWindow.Control= userChatWindow;
				CP.tabPage = chatWindow;						
				chatWindowsArray.Add(CP);

				chatWindow.Width = chatControl1.Width;
				chatWindow.Height= chatControl1.Height;				
				chatTabControl.TabPages.Add(chatWindow);
				chatWindow.ImageIndex = 0;

				userChatWindow.thisWindowTabPage = chatWindow;


				chatTabControl.SelectedTab = chatWindow;
				Utility.NiceMenu.NiceMenu mSaveMenu = myNiceMenu.SearchNiceMenuItem("Chat");				
//				if(mSaveMenu != null)
//				{
//					NiceMenu mNewMenu = new NiceMenu();
//					mNewMenu.Click += new EventHandler(mChatWindowSaveMenu_Click);							 
//					mNewMenu.Text = userProfile.Name;														
//					mSaveMenu.MenuItems.Add(mNewMenu);					
//					
//				}
//				Utility.NiceMenu.NiceMenu mSaveAs = myNiceMenu.SearchNiceMenuItem("Save");
//				if(mSaveAs != null)
//				{
//					//Utility.NiceMenu.NiceMenu mChatMenu = mSaveAs.SearchNiceMenuItem("Chat",2);
//					Utility.NiceMenu.NiceMenu mChatMenu = mSaveAs.SearchNiceMenuItem("Chat");
//					if(mChatMenu != null)
//					{
//						NiceMenu mNewMenu = new NiceMenu();
//						mNewMenu.Click += new EventHandler(mChatWindowSaveAsMenu_Click);							 
//						mNewMenu.Text = userProfile.Name;
//						mChatMenu.MenuItems.Add(mNewMenu);					
//					}					
//				}
				if(btrue)
				{
					userChatWindow.txtBoxPreviewChat.Clear();
					string str = Client.ClientUI.getInstance().chatControl1.txtBoxPreviewChat.Rtf;					
					chatTabControl.SelectedTab = chatWindow;

					IMChatMessage msg=new IMChatMessage();
					msg.senderID = network.profile.ClientId;					
					msg.RecipientId = userProfile.ClientId;
					msg.MessageType = (ushort)MessageObjectType.MsgIMChat;           				
					msg.IMMessage=chatControl1.txtBoxPreviewChat.Rtf;					
					msg.sender=network.profile.Name;
					network.SendLoadPacket(msg);

					userChatWindow.messagePool.Add(msg);
					
				}

				mSaveMenu = myNiceMenu.SearchNiceMenuItem("Chat");				
				if(mSaveMenu != null)
				{
					NiceMenu mNewMenu = new NiceMenu();
					mNewMenu.Click += new EventHandler(chatMenuPrint_Click);
					mNewMenu.Text = userProfile.Name;														
					mSaveMenu.MenuItems.Add(mNewMenu);															
				}

				userChatWindow.txtBoxPreviewChat.Focus();

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4673",exp,null,false);			
				//ShowExceptionMessage(" Open Chat Window has encountered an exception. " + ee.Message);
			}			
		}
		#endregion 
 
		#region Browsershare Module
		#region shareBrowserForWebFiles(string url)
		public void shareBrowserForWebFiles(string url)
		{
			


			int var_temp_webshare=0;
			
			for(int i=0;i<tabBody.TabPages.Count;i++)
			{
				//MessageBox.Show(tabBody.TabPages[i].Title.ToString());
				if(tabBody.TabPages[i].Title.ToString().Equals("Web Sharing"))
				{
					++var_temp_webshare;
				}

			}

			//if(var_temp_appshare<=this.var_no_appshare)
			if(var_temp_webshare<1)
			{
	


				try
				{
					Crownwood.Magic.Controls.TabPage tabBrowser = new Crownwood.Magic.Controls.TabPage("Web Sharing");			

					try
					{
						if(browserControl!=null)
						{
							browserControl.CleanandClear();
							//browserControl.Dispose();
							browserControl=null;
						
						}
					}
					catch(Exception exp)
					{
					MessageBox.Show(exp.StackTrace.ToString()+exp.Message.ToString());
					}
					
					
					
					
					browserControl = new browser();
					browserControl.chk_Autoshare.Checked=true;
					browserControl.Dock = DockStyle.Fill;
					tabBrowser.Control = browserControl;
					try
					{
					
						tabBody.TabPages.Add(tabBrowser);
					}
					catch(Exception exp)// ee)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4820",exp,null,false);

					}
					tabBody.SelectedTabEx = tabBrowser;
					arrayWebControl.Add(tabBrowser);
				
					browserControl.Left = 0;
					browserControl.Top = 0;
					
					browserControl.PresenterInterface();
					browserControl.navigateTo(url);
					
			
				
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4850",exp,null,false);			
					//ShowExceptionMessage(" Share Browser has encountered an exception. " + ee.Message);

				}
			}
			else
			{
				this.ShowExceptionMessage("Sorry! Another web site has already been shared by some user in this meeting");

			}
		}
		#endregion 

		#region ShareMyBrowser()
		void ShareMyBrowser()
		{
			var_temp_webshare=0;
			
			for(int i=0;i<tabBody.TabPages.Count;i++)
			{
				//MessageBox.Show(tabBody.TabPages[i].Title.ToString());
				if(tabBody.TabPages[i].Title.ToString().Equals("Web Sharing"))
				{
					++var_temp_webshare;
				}

			}

			//if(var_temp_appshare<=this.var_no_appshare)
			if(var_temp_webshare<1)
			{
	
				try
				{
					Crownwood.Magic.Controls.TabPage tabBrowser = new Crownwood.Magic.Controls.TabPage("Web Sharing");			

					try
					{
						if(browserControl!=null)
						{
							browserControl.CleanandClear();
							//browserControl.Dispose();
							browserControl=null;
						
						}
					}
					catch(Exception exp)
					{
						MessageBox.Show(exp.StackTrace.ToString()+exp.Message.ToString());
					}
					
					
					browserControl = new browser();
					

					browserControl.Global_height=tabBody.Height;
					browserControl.Global_width=tabBody.Width;
					// Used in minuts of meeting 
					browserMM=browserControl;
					browserControl.Dock = DockStyle.Fill;
					tabBrowser.Control = browserControl;
				

					try
					{
					
						tabBody.TabPages.Add(tabBrowser);
					}
					catch(Exception exp)// ee)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4732",exp,null,false);			
					}
					tabBody.SelectedTabEx = tabBrowser;
					arrayWebControl.Add(tabBrowser);
				
					browserControl.Left = 0;
					browserControl.Top = 0;
					browserControl.PresenterInterface();
					browserControl.InitializeWhiteboard();
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4744",exp," Share Browser has encountered an exception. "+exp.ToString(),true);			
					//ShowExceptionMessage(" Share Browser has encountered an exception. " + ee.Message);

				}
			}
			else

			{
				this.ShowExceptionMessage("Sorry! Another web site has already been shared by some user in this meeting");
			
			}


		}

		#endregion 

		public bool RemoveParticipent(WebMeeting.Common.ClientProfile cP)
		{
			try
			{
				bool isAttendeeRemovedFromMainParticpantList=false;
				for(int i = 0 ; i < arrayParticipents.Count ; i++)
				{
					ClientProfile clientProfile =(ClientProfile) arrayParticipents[i];
					Trace.WriteLine("No of Participents in the list" + clientProfile.ClientId.ToString());
					if(clientProfile.ClientId== cP.ClientId)
					{
						isAttendeeRemovedFromMainParticpantList=true;
						lock(arrayParticipents)
						{
							try
							{
								if(arrayParticipents.Count>i)
								{
									arrayParticipents.RemoveAt(i);
								}
							}
							catch(Exception exp)
							{
								WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> if(arrayParticipents.Count>i)",exp,null,false);
							}

						}
						lock(listParticipents)
						{
							try
							{
							
								if(listParticipents.Items.Count>i)
								{
									listParticipents.Items.RemoveAt(i);												
								}
							}
							catch(Exception exp)
							{
								WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> if(listParticipents.Items.Count>i)",exp,null,false);
							}
						}
						
						break;				
					}
				}
				string strParticipantName;
				if(isAttendeeRemovedFromMainParticpantList)/*if participant is removed from main list*/
				{
					for(int ni=0;ni<=this.sendTo.Items.Count;ni++)
					{
						strParticipantName=(string)this.sendTo.Items[ni];
						if(strParticipantName==cP.Name)
						{
							try
							{
								this.ExceptionLog("Removing index from the sendTo " + ni.ToString() + " Method :: Remove Participant ");
								this.sendTo.Items.RemoveAt(ni);				
							}
							catch(Exception exp)
							{
								WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> this.sendTo.Items.RemoveAt(ni);",exp,null,false);
							}						
							break;
						}
					}
				}
				return true;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4859",exp,null,false);			
				
				//ShowExceptionMessage(" Remove participents has encountered an exception. " + ee.Message);
			}
			
			return false;
		}
			
		
		public int GetClientProfileIndex(int profileId)
		{
			try
			{
				for(int i = 0 ; i < arrayParticipents.Count ; i++)
				{
					ClientProfile prof = (ClientProfile) arrayParticipents[i];
					if(prof.ClientId == profileId)
						return i;							
				}
				return -1;
			}
			catch(Exception exp)
			{
			
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4883",exp,null,false);			
				return -1;
			}
		}
		public void OnJoinConversationUpdateDocumentSharing()
		{
			try
			{
				for(int	i =	0 ;	i <	arraydocumentSharing.Count ;	i++)
				{
					Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
					documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
					if(!control.IsClosed)
					{
						if(GetClientProfileIndex(control.ClientId) == -1)
						{						
							tabDocumentSharing.Title = tabDocumentSharing.Title  + " - Disabled ";		
							control.IsClosed = true;
						}
					}		
				}	
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4907",exp,null,false);			
				
			}
		}

		public void OnJoinConversationUpdateWebBrowsers()
		{
			try
			{
				for(int	i =	0 ;	i <	arrayWebControl.Count;	i++)
				{								
					Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)arrayWebControl[i];
					browser browserControl = (browser)tabPage.Control;								
					if(!browserControl.IsClosed)
					{
						if(GetClientProfileIndex(browserControl.ClientID) == -1)					
						{
							tabPage.Title = tabPage.Title  + " - Disabled ";					
							browserControl.IsClosed = true;
						}
					}
				}	
			}
			catch(Exception exp)
			{			
				
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4907",exp," On Join Conversation Update WebBrowsers has encountered an exception. " + exp.Message.ToString(),true);			
				//ShowExceptionMessage(" On Join Conversation Update WebBrowsers has encountered an exception. " + ee.Message);
			}
		}
		//this function is called by network manager when an attendee is droped. the ClientUI updates control
		//accordingly. If there is some active session with recently droped attendee close those windowss
		public void AttendeeDroped(WebMeeting.Common.ClientProfile profile)
		{
			//DesktopSharing cleanup. If attendee was using desktopsharing remove its reference
			try
			{	
				//MessageBox.Show("Our ID is :::: " + network.profile.ClientId.ToString());
				#region desktopSharing Cleanup
				if(desktopSharedProfile != null)
				{
					if(profile.ClientId == desktopSharedProfile.ClientId)
					{
						try
						{
							TerminateDesktopSharing();						
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4955",exp,null,false);			

						}
						desktopSharedProfile = null;
					}
				}

				#endregion
				//_statusBar.LeftMessage = "Connected to server";
				#region Appsharing cleanup	
				try
				{
					for(int i = 0 ; i < ApplicationSharingTabPagesArray.Count ;)
					{
						Crownwood.Magic.Controls.TabPage tab = (Crownwood.Magic.Controls.TabPage)ApplicationSharingTabPagesArray[i];
						ApplicationSharing appControl = (ApplicationSharing)tab.Control;
						//	int controlOwnerId=appControl.controlOwnerId;
						///	MessageBox.Show(controlOwnerId.ToString());										
						//	MessageBox.Show("Profile Client ID ::: "+ profile.ClientId.ToString());
						if(appControl.controlOwnerId == profile.ClientId)
						{
							//MessageBox.Show("Profile Client ID ::: "+ profile.ClientId.ToString());
							if(this.tabBody.Contains(tab))
							{
								try
								{
									Invoke(DeleteDocumentSharingWindow,new object[]{tab});					
								}
								catch(Exception exp)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs AttendeeDroped method for application sharingline==> 5390",exp,null,false);			
								}
							}
							ApplicationSharingTabPagesArray.RemoveAt(i);						
						}
						else
							i++;
					}
				}		
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs AttendeeDroped method for application sharingline==> 5402",exp,null,false);			
				}		
				#endregion				

				#region WebBrowser Cleanup
				try
				{
					for(int	i =	0 ;	i <	arrayWebControl.Count;)
					{								
						Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)arrayWebControl[i];
						browser browserControl = (browser)tabPage.Control;													
						if((browserControl.ClientID  ==	profile.ClientId) && (!browserControl.IsClosed))
						{
							if(this.tabBody.Contains(tabPage))
							{
								try
								{
									Invoke(RemoveTabPageFromMainBody,new object[]{tabPage});
								}
								catch(Exception exp)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs AttendeeDroped method for Web sharingline==> 5420",exp,null,false);			
								}
							}
							this.arrayWebControl.RemoveAt(i);
							tabPage.Title = tabPage.Title  + " - Disabled ";
							browserControl.IsClosed = true;						
						}
						else
							i++;												
					}				
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs AttendeeDroped method for Web sharingline==> 5440",exp,null,false);			
				}
				
				#endregion				

				#region Document/PPt sharing
				try
				{
					for(int	i =	0 ;	i <	arraydocumentSharing.Count;)
					{
						Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
						documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
						if((control.ClientId == profile.ClientId) && (!control.IsClosed))
						{										
							if(this.tabBody.Contains(tabDocumentSharing))
							{						
								try
								{
							
									Invoke(RemoveTabPageFromMainBody,new object[]{tabDocumentSharing});
								}
								catch(Exception exp)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs AttendeeDroped method for Document/PPt sharing==> 5460",exp,null,false);			
								}
							}
							arraydocumentSharing.RemoveAt(i);
							if(tabDocumentSharing.Title.IndexOf("- Disabled") == -1)
								tabDocumentSharing.Title = tabDocumentSharing.Title  + " - Disabled";
							control.IsClosed= true;						
						}
						else
							i++;
					}
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs AttendeeDroped method for Document/PPt sharing==> 5473",exp,null,false);			
				}
				
				#endregion				

				#region AudioMessage
				/*for(int i = 0; i < subscribedAudioList.Count ; i++)
				{
					CaudioTabPage audioTabPage = (CaudioTabPage)subscribedAudioList[i];
					if(	audioTabPage.sessionID == profile.audioID)
					{
						audioTabPage.Title += " - Disabled ";						
					}
				}*/

				#endregion				
	
				/*
				 * the same working is already implemented in the "RemoveParticipent" method so dont need to remove this here
				 */
				//sendTo.Items.Remove(profile.Name);

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 4907",exp,"Attendee Droped " +  exp.Message.ToString(),false);
				//WebMeeting.Error.Show("Attendee Droped " +  ee.Message);
			}			
		}

		#endregion

		#region Windows_Creation Module


		public void CreateChildControl(MessageObject msg)
		{
			try
			{
				chatWindowsToCreate.Add(msg);
				//message identifies 10999 custom message
				Win32.USER32.SendMessage(this.Handle,10999,new IntPtr(),new IntPtr());
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5180",exp,null,false);
			}
		}
		
		public void CreateWebControlWindow(MessageObject msg)
		{
			try
			{
				chatWindowsToCreate.Add(msg);
				Win32.USER32.SendMessage(this.Handle,Win32.USER32.WM_CREATE_WEBBROWSER,new IntPtr(),new IntPtr());
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5200",exp,null,false);
			}

		}

		private void network_ChildWindowCreationEvent(MessageObject msg) 
		{		
			CreateChildControl(msg);
		}
		
	
		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			try
			{
				switch(m.Msg)
				{
					case Win32.USER32.WM_PAINT:	
						if(this.myAppShareStarted)
						{
							if(this._appShareControl!=null)
							{
								if(this._appShareControl.InvokeRequired)
								{
									//Trace.WriteLine("Invoke is required");
								}
								else
								{
									this._appShareControl.Minimize_SmallWindow();
								}
							}
						}
						base.WndProc(ref m);						
						break;
						#region chatWindowCreation
					case 10999: //custom message passed
						try
						{

							for(int i = chatWindowsToCreate.Count-1 ; i >= 0 ; i--)
							{
								MessageObject msg = (MessageObject)chatWindowsToCreate[i];
								if(!msg.GetType().Equals(typeof(IMChatMessage)))
									continue;
								this.btn_Chat_Click(null,null);/*for maximizing chat*/
								IMChatMessage chatMsg = (IMChatMessage)msg;
								ClientProfileWrapper CP = new WebMeeting.Client.ClientUI.ClientProfileWrapper();
								chatControl userChatWindow = new chatControl();                                
								if(chatMsg.senderID == network.profile.ClientId)
								{
									userChatWindow.nRecipientID = chatMsg.RecipientId;

									CP.nUniqueID = chatMsg.RecipientId;
								}						
								else
								{
									userChatWindow.nRecipientID = chatMsg.senderID;
									CP.nUniqueID = chatMsg.senderID;
								}

								Crownwood.Magic.Controls.TabPage chatWindow = new Crownwood.Magic.Controls.TabPage();
								if(chatMsg.RecipientId == network.profile.ClientId)
									chatWindow.Title = chatMsg.sender;
								else
									chatWindow.Title = sendTo.Text;
						
						
								//attach chat control to tabpage
								chatWindow.Control= userChatWindow;
								//chatWindow.Left = 0;
								//chatWindow.Top = 0;					
								CP.tabPage = chatWindow;						
								chatWindowsArray.Add(CP);
						
								Utility.NiceMenu.NiceMenu mSaveMenu = myNiceMenu.SearchNiceMenuItem("Chat");
								if(mSaveMenu != null)
								{
									NiceMenu mNewMenu = new NiceMenu();
									mNewMenu.Click += new EventHandler(mChatWindowSaveMenu_Click);							 
									mNewMenu.Text = chatMsg.sender;														
									mSaveMenu.MenuItems.Add(mNewMenu);					
					
								}
								//							Utility.NiceMenu.NiceMenu mSaveAs = myNiceMenu.SearchNiceMenuItem("Save As");
								//							if(mSaveAs != null)
								//							{
								//								Utility.NiceMenu.NiceMenu mChatMenu = mSaveAs.SearchNiceMenuItem("Chat",2);
								//								if(mChatMenu != null)
								//								{
								//									NiceMenu mNewMenu = new NiceMenu();
								//									mNewMenu.Click += new EventHandler(mChatWindowSaveAsMenu_Click);							 
								//									mNewMenu.Text = chatMsg.sender;
								//									mChatMenu.MenuItems.Add(mNewMenu);					
								//								}					
								//							}

								chatWindow.Width = chatControl1.Width;
								chatWindow.Height= chatControl1.Height;

						
								userChatWindow.thisWindowTabPage = chatWindow;
								chatWindow.ImageIndex = 0;
								chatTabControl.TabPages.Add(chatWindow);
						
						


								chatTabControl.SelectedTab = chatWindow;
								//now add the chat message to chat window
								userChatWindow.messagePool.Add(msg);
								chatWindowsToCreate.Remove(msg);
						

							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("wndproce of client ui.cs",exp,null,false);
						}
						
						break;
						#endregion
					case 10997:
						try
						{
							tabBody.TabPages.Remove(tabPageDesktopSharing);
						}
						catch(Exception exp)// ee)

						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5550",exp,null,false);

						}
					

						break;
					case 10998:
						try
						{	
							tabBody.TabPages.Add(tabPageDesktopSharing);
						
							tabBody.SelectedTab = tabPageDesktopSharing;
							if(network.clientType == ClientType.ClientHost)
							{
								tabBody.ShowClose = true;
							}
							else
								tabBody.ShowClose = false;
						
							//tabBody.SelectedTab = tabPageDesktopSharing;
						}
						catch(Exception exp)// ee)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5400",exp,null,false);

						}	

						break;						
			
					case Win32.USER32.WM_REMOVE_MANAGE_CONTENT_TAB:
						try
						{
							tabBody.TabPages.Remove(tabPageWebContent);
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5328",exp,null,false);
						}
						break;
					case Win32.USER32.WM_SHOW_POLL_RESULTS:
						CreateNewPollingWindow(tempPollMessage,null,tempPollMessage.IsEvaluation);					
						break;
					case Win32.USER32.WM_SUBSCRIBE_USER_CAMERa:
						SubscribetoUserCamera(tempProfile);
						break;
					case Win32.USER32.WM_CREATE_PRESENTER_QA_WIDOW:
						try
						{
													
						//	tabBody.TabPages.Add(TabPageQAPresenter);	
				
							/*
							
							if(frmQAP==null)
							{
								//frmQAP.Disposed;
								MessageBox.Show("frmQAP==null is Null");
								frmQAP=new WebMeeting.Client.QAStuff.frmQAPresenter();
							}
							*/

							//frmQAP=new WebMeeting.Client.QAStuff.frmQAPresenter();
							frmQAP.Controls.Add(qaPresenter);
							frmQAP.Show();
						}
						catch(Exception exp)// ee)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5350",exp,null,false);

						}
						break;
					case Win32.USER32.WM_CREATE_ATTENDEE_QA_WIDOW:
						try
						{
						
							//tabBody.TabPages.Add(TabPageQAAttendee);
							//	tabBody.SelectedTabEx = TabPageQAAttendee;
							/*
							if(frmQAA==null)
							{
								MessageBox.Show("frmQAA==null is Null");
								frmQAA=new WebMeeting.Client.QAStuff.frmQAAttendee();
							}
							*/
							//frmQAA=new WebMeeting.Client.QAStuff.frmQAAttendee();
							frmQAA.Controls.Add(qaAttendee);
							frmQAA.Show();
						
						}
						catch(Exception exp)//ee)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5360",exp,null,false);

						}
						break;
					case Win32.USER32.WM_SUBSCRIBE_USER_AUDIO:
						SubscribetoUserAudio(tempProfile);
						break;                    
					case Win32.USER32.WM_SHOW_GREETINGS:
					{
						FontStyle ff=new FontStyle();
						ff=ff|FontStyle.Bold;			
						notifier= new WebMeeting.CustomUIControls.TaskbarNotifier();
						notifier.ContentClickable = false;	
						Image image = Image.FromFile(Application.StartupPath + "\\window.bmp");
						notifier.SetBackgroundBitmap(image,Color.FromArgb(255,0,255));
						Image imageClose = Image.FromFile(Application.StartupPath + "\\close.bmp");
						notifier.SetCloseBitmap(imageClose,Color.FromArgb(255,0,255),new Point(218,2));
						notifier.TitleRectangle=new Rectangle(08,15,230,25); 
						notifier.NormalTitleFont = new Font(this.Font.FontFamily,8,ff);
						notifier.NormalTitleColor = Color.FromArgb(0,0,0);
						notifier.ContentRectangle=new Rectangle(08,25,230,100);								
						notifier.CloseClick += new EventHandler(notifier_CloseClick);
						if(!globalGreetingsMessage.IsInformation)
							notifier.Show("Greetings From " + globalGreetingsMessage.senderProfile.Name,globalGreetingsMessage.MessageString,500,3000,500);
						else
							notifier.Show("Information Sent By:" + globalGreetingsMessage.senderProfile.Name,globalGreetingsMessage.MessageString,500,3000,500);
					}
						break;
					case Win32.USER32.WM_CREATE_DOC_SHARING_WINDOW:
					{						
						//MessageBox.Show(" Wnd Proc WM_CREATE_DOC_SHARING_WINDOW");

						break;
					}				
					case Win32.USER32.WM_ADD_NOTES_TAB:
						chatTabControl.TabPages.Add(tabPageNotes);
						break;
					case Win32.USER32.WM_REMOVE_NOTES_TAB:
						try
						{
							chatTabControl.TabPages.Remove(tabPageNotes);
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5405",exp,null,false);

						}

						break;
					case Win32.USER32.WM_CREATE_WEBBROWSER:
					{
						try
						{
							for(int i = chatWindowsToCreate.Count-1 ; i >= 0 ; i--)
							{
								MessageObject msg = (MessageObject)chatWindowsToCreate[i];
								if(msg.GetType().Equals(typeof(MsgWebBrowseMessage)))		
								{
									MsgWebBrowseMessage WebMsg = (MsgWebBrowseMessage)msg;							
									Crownwood.Magic.Controls.TabPage tabBrowser = new Crownwood.Magic.Controls.TabPage("Web Sharing");			

									try
									{
										if(browserControl!=null)
										{
											browserControl.CleanandClear();
											//browserControl.Dispose();
											browserControl=null;
						
										}
									}
									catch(Exception exp)
									{
										MessageBox.Show(exp.StackTrace.ToString()+exp.Message.ToString());
									}
					
					
									browserControl = new browser();
									tabBrowser.Control = browserControl;
									tabBody.TabPages.Add(tabBrowser);
									tabBody.SelectedTabEx = tabBrowser;
									browserControl.sessionID = WebMsg.webBrowseID;
									browserControl.msgPool.Add(WebMsg);
									browserControl.Left = 0;
									browserControl.Top = 0;
									bool Found = false;
									for(int z = 0 ; z < arrayParticipents.Count	 ; z++)
									{
										ClientProfile CP = (ClientProfile)arrayParticipents[z];
										if(CP.ClientId == WebMsg.ClientId)
										{
											Found = true;
											break;
										}

									}
									WebMsg.active = Found;
							
									browserControl.IsClosed = !WebMsg.active;
									browserControl.AttendeeInterface();
									browserControl.ClientID = WebMsg.ClientId;
									if(!WebMsg.active) 
										tabBrowser.Title = "WebSharing - Disabled";
							
									arrayWebControl.Add(tabBrowser);
									chatWindowsToCreate.Remove(WebMsg);
								}
							}
						}
						catch(Exception exp)// ee)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage(" chat control ClientUI.cs line==> 5550 ",exp,null,false);
						}

					}
						break;
					case Win32.USER32.WM_OPEN_WHITEBOARD:
					{
						try
						{
							btn_Whiteboard_Click(null,null);
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5550",exp,null,false);
					
						}
					}
						break;
					default:
						base.WndProc(ref m);                
						break;

				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5550",exp,null,false);
			}

		}		
		#endregion		
		
		#region SynchWindow(TabPagesSyncrhonizationMsessage msg)
		public void SynchWindow(TabPagesSyncrhonizationMsessage msg)
		{
			//return;
			//msg.MessageType=TabPageType
			try
			{
				if(msg.sessionId == -1)
				{
					switch(msg.type)
					{
						case TabPageType.DesktopSharing:							
							tabBody.SelectedTabEx = tabPageDesktopSharing;
							//Crownwood.Magic.Controls.TabPage p;
							break;
						case TabPageType.ManageContent:
							
							tabBody.SelectedTabEx = tabPageWebContent;
							break;
						case TabPageType.WelcomeScreen:
							tabBody.SelectedTabEx = nWelcomePage;
							break;
						case TabPageType.Whiteboard:
							
							tabBody.SelectedTabEx = tabPageInfo;
							break;						
					}
					nLastSelectedIndex = tabBody.SelectedIndex;


				}
				else
				{		
					bool bFound = false;
					for(int i = 0 ; i < this.arraydocumentSharing.Count ; i++)
					{		
						Crownwood.Magic.Controls.TabPage page = (Crownwood.Magic.Controls.TabPage)arraydocumentSharing[i];					
						if(((documentSharingControl)page.Control).sessionID == msg.sessionId)
						{
							
							tabBody.SelectedTabEx = page;
							bFound = true;
							break;
						}

					}
					nLastSelectedIndex = tabBody.SelectedIndex;
					if(bFound)
						return;
					for(int i = 0 ; i < this.pollingTabControl.TabPages.Count ; i++)
					{
						if(this.pollingTabControl.TabPages[i].GetType().Equals(typeof(PollingTabPage)))
						{
							Crownwood.Magic.Controls.TabPage page = (Crownwood.Magic.Controls.TabPage)this.pollingTabControl.TabPages[i];					
							if(((WebMeeting.Polling.PollResult)page.Control).polling.sessionID == msg.sessionId)
							{
							
								tabBody.SelectedTabEx = tabPollingGlobal;
								pollingTabControl.SelectedTab = page;
								bFound = true;
								break;
							}
						}

					}
					nLastSelectedIndex = tabBody.SelectedIndex;
					if(bFound)
						return;
					for(int i = 0 ; i < this.arrayWebControl.Count ; i++)
					{
						Crownwood.Magic.Controls.TabPage page = (Crownwood.Magic.Controls.TabPage)arrayWebControl[i];					
						if(((browser)page.Control).sessionID== msg.sessionId)
						{
							
							tabBody.SelectedTabEx = page;
							bFound = true;
							break;
						}
					}
					nLastSelectedIndex = tabBody.SelectedIndex;
					if(bFound)
						return;
					for(int i = 0 ; i < this.ApplicationSharingTabPagesArray.Count ; i++)
					{
						Crownwood.Magic.Controls.TabPage page = (Crownwood.Magic.Controls.TabPage )ApplicationSharingTabPagesArray[i];
						ApplicationSharing control = (ApplicationSharing) page.Control;
						if(control.nSessionID == msg.sessionId)
						{
							tabBody.SelectedTabEx = page;
							bFound = true;
							break;
						}
					}

					nLastSelectedIndex = tabBody.SelectedIndex;
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5422",exp,"Attendee Droped " +  exp.Message.ToString(),false);
			}
		}
		#endregion 

		#region TabBody (Center View) Close_Click tabBody_SelectionChanged Exception Done

		public bool bSendIt = true;
		private Crownwood.Magic.Controls.TabPage lastSelectedTab;

		private  void Close_ChatPanel(object sender, System.EventArgs e)
		{
			try
			{
				///////////////////////////////////////////////////
				/*ClientProfile userProfile = (ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index];
				if(userProfile.ClientId == network.profile.ClientId)
					return;

				for(int i = 0 ; i < chatWindowsArray.Count ; i++)
				{
					ClientProfileWrapper CP2 = (ClientProfileWrapper)chatWindowsArray[i];
					if(CP2.nUniqueID ==  userProfile.ClientId)
					{
						chatTabControl.SelectedTab= CP2.tabPage;	
						return;
					}
				}*/			
				//////////////////////////////////////////////////////
				///

				//System.Windows.Forms.Control page = chatTabControl.SelectedTab.Control;			
				Crownwood.Magic.Controls.TabPage page=chatTabControl.SelectedTab;
				int mem=-1,i=-1;		
				int indexMenuItems;
				string strParticipantName="";
				for(i=0;i<chatWindowsArray.Count;i++)
				{
					ClientProfileWrapper CP2 = (ClientProfileWrapper)chatWindowsArray[i];
					if(CP2.tabPage==page)
					{
						strParticipantName=page.Title;

						mem=i;
						break;
					}
				}
				if(mem!=-1)
					chatWindowsArray.RemoveAt(mem);
				Utility.NiceMenu.NiceMenu mSaveMenu = myNiceMenu.SearchNiceMenuItem("Chat");
				if(mSaveMenu != null)
				{
					for(indexMenuItems=0;indexMenuItems<mSaveMenu.MenuItems.Count;indexMenuItems++)
					{
						if(mSaveMenu.MenuItems[indexMenuItems].Text==strParticipantName)
						{
							mSaveMenu.MenuItems.RemoveAt(indexMenuItems);	
						}
					}
//					NiceMenu mNewMenu = new NiceMenu();
//					mNewMenu.Click += new EventHandler(mChatWindowSaveMenu_Click);							 
//					mNewMenu.Text = chatMsg.sender;														
//					mSaveMenu.MenuItems.Add(mNewMenu);										
				}				
				chatTabControl.TabPages.Remove(chatTabControl.SelectedTab);
				if(chatTabControl.TabPages.Count<=2)
					this.openRightWindow();
				page.Dispose();
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5471",exp,null,false);
		
			}
		}
		private void Close_UserPanel(object sender, System.EventArgs e)
		{
			try
			{
				string name = tabControl1.SelectedTab.Control.Name;
				if(name == "videovoiceControl")
				{
					if(tabControl1.SelectedTab.Title == "My Audio")
					{
						DisableMyAudio();
					}
					else if(tabControl1.SelectedTab.Title == "My Video")
					{
						System.Windows.Forms.Control page = tabControl1.SelectedTab.Control;
						videoEnabled = false;
						IsHostVideoEnabled = false;
						tabControl1.TabPages.Remove(tabControl1.SelectedTab);
						page.Dispose();

						network.profile.videoID = -1;//network.profile.ClientId;				
						sendProfileMessage();
						
						
					}
				}
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5471",exp,"Error removing tab: "+exp.ToString(),true);
				
			}
		}


		//By Zaeem this method is suppose to check that if the profile coming is 
		//the presenter's profile if Yes then it returns true or else returns false;
		
		/// <summary>
		/// This method checks whether i am the presenter or not
		/// </summary>
		/// <returns></returns>
		public bool IfIamthePresenter()
		{
			if(network.profile.clientType==ClientType.ClientHost)
			{
				if(this.ifHostisPresenter())
					return true;
				else
					return false;

			}
			else if(network.profile.clientType==ClientType.ClientPresenter)
			{
				return true;
			
			}
			else
			{
				return false;
			
			}
			

			
		
		return false;
		
		}








		/// <summary>
		/// For closing the opened tabs
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void Close_Click(object sender, System.EventArgs e) // when small close button on client is clicked
		{
			try

				{	
				
				
			
				if( tabBody.SelectedTab == nWelcomePage)
				{		
					return;
				}

					
				else if( tabBody.SelectedTab.Title == "WhiteBoard" )
				{
					try
					{
						//if(network.profile.clientType==ClientType.ClientHost)
						if(this.IfIamthePresenter())
						{
							tabBody.TabPages.Remove(tabBody.SelectedTab);
							
							WhiteboardMessage msg=new WhiteboardMessage();
							msg.MessageType=(ushort)MessageObjectType.MsgHideWhiteBoard;
							msg.ConferenceID = network.profile.ConferenceID;
							msg.SenderID = network.profile.ClientId;
							network.SendLoadPacket(msg);
						}
					}
					catch(Exception exp)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5537",exp,null,false);
					}
					
				}
				else
					if((tabBody.SelectedTab != tabPageInfo) && (tabBody.SelectedTab != startPage))
				{		
					System.Type tType=tabBody.SelectedTab.GetType();					
					if(tabBody.SelectedTab.Control.GetType().Name == "ApplicationSharing")
					{					
						ApplicationSharing app = (ApplicationSharing)tabBody.SelectedTab.Control;
						if(app.controlOwnerId == network.profile.ClientId)
						{											
							//Added by Zaeem
							app.btnShare_Click(null,null);
							//////////////////////////
							//app.StopRefreshListTimer();						
							//app.TerminateEx();
							app.QuickAppShareExit();/*for releasing shared window handle and making its parent '0'*/
							

							myAppShareStarted= false;
							tabBody.TabPages.Remove(tabBody.SelectedTab);
							ApplicationSharingTabPagesArray.Remove(tabBody.SelectedTab);
							
						}	
						return;
					}
					if(tabBody.SelectedTab == tabPageDesktopSharing)
					{
						try
						{
							DesktopSharingControlMessage message = new DesktopSharingControlMessage(false,
								network.profile.ClientId,desktopSharedProfile.ClientId);							
							message.Start=false;		
							message.Status = -1;	
							message.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
							network.SendLoadPacket(message);		
							

						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5573",exp,null,false);
						}

						try
						{
							TerminateDesktopSharing();
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5582",exp,null,false);
							
						}					
						this.desktopSharedProfile=null;

						System.GC.Collect();
						System.GC.WaitForPendingFinalizers();
								
					}								
					else
					{
						try
						{
							System.Type a=tabBody.SelectedTab.Control.GetType();
						
							if(a.Equals(typeof(browser))) //dont' close tab if its attendee and its a browser control
							{
								
								this.whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.None;	//per2			
								Thread.Sleep(2000);
													
								//this.whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.None;	//per2			
								browser browserControl = (browser)tabBody.SelectedTab.Control;
								
								//if(!browserControl.IsAttendee)
								if(this.IfIamthePresenter())
								{
									browserControl.SendCloseMessage();
									
								}
									
								browserControl.Terminate();								
								
								
								browserControl.Dispose();								
								browserControl=null;
								
								
								Crownwood.Magic.Controls.TabPage tabcontrol=tabBody.SelectedTab;
								//Replaced by Zaeem
								Invoke(DeleteSharingWindow,new object[]{tabcontrol});
								//tabBody.TabPages.Remove(tabcontrol);	
								//tabcontrol.Dispose();
								//tabcontrol=null;
								


								// Zaeem's addition 
								// This was a troublesome block so i remove that Remove method and replaced it with 
								//"RemoveAt" method 
								//arrayWebControl.Remove(browserControl);								
								for(int i=0;i<arrayWebControl.Count;i++)
								{
									if(browserControl.sessionID==((browser)	((Crownwood.Magic.Controls.TabPage)	arrayWebControl[i] ).Control).sessionID)
										arrayWebControl.RemoveAt(i);
																
								}
								

													
								GC.Collect();
								GC.WaitForPendingFinalizers();
							}
							else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
							{
								documentSharingControl doc = (documentSharingControl) tabBody.SelectedTab.Control;
								//if(doc.IsAttendee)
								if(!this.IfIamthePresenter())
								{
									return;
								}
								else									
								{
									
									//////////////////////////////////////// Document sharing close button 
									
									DocumentSharing msg = new DocumentSharing();
									msg.senderProfile = this.network.profile;
									msg.SenderID = this.network.profile.ClientId;
									msg.DocumentSharingSessionId = doc.sessionID;
									msg.sessionID = doc.sessionID;
									msg.bEnabled = false;
									msg.ConferenceID = network.profile.ConferenceID ;
									network.SendLoadPacket(msg);
									
									////////////////////////////////////
									
									for(int i = 0 ; i < arraydocumentSharing.Count; i++)
									{
										WebMeeting.Client.documentSharingControl _control = (documentSharingControl)((Crownwood.Magic.Controls.TabPage)arraydocumentSharing[i]).Control;
										if(_control == 	tabBody.SelectedTab.Control)
										{
											arraydocumentSharing.RemoveAt(i);
											_control.ClearThumbnailsFromPanel();/*clear all thumnails from panel*/
											_control.threadFunctionStop();
										}
									}
									////////////////////////////////////////
									tabBody.TabPages.Remove(tabBody.SelectedTab);
									
								
								}
							}
							else if(tabBody.SelectedTab == tabPageDesktopSharing)
							{
								if(network.profile.clientType == ClientType.ClientHost)
								{
									DesktopSharingControlMessage msg = new DesktopSharingControlMessage(false,network.profile.ClientId,desktopSharedProfile.ClientId);
									msg.Status = -1;
									msg.Start = false;
									msg.RecipientId = desktopSharedProfile.ClientId;
									msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID;
									network.SendLoadPacket(msg);
									try
									{
										TerminateDesktopSharing();
									}
									catch(Exception exp)
									{                                   
								
										WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5673",exp,null,false);
									}
								}
							}							
							else if(tabBody.SelectedTab == tabPageScreenCapture)
							{
								screencaptureControl.stopRecording();
								tabBody.TabPages.Remove(tabPageScreenCapture);
								screenCaptureEnabled = false;
							}
							else if(tabBody.SelectedTab == tabPollingGlobal)
							{					
								if(pollingTabControl.TabPages.Count >= 1)
								{		
									try
									{
										int totalSubPages = pollingTabControl.TabPages.Count ;
										for(int i=0; i< totalSubPages ;i++) // by kamran
										{
											//SharedPoolResult="";
											//pollingTabControl.TabPages[0].Control
											Crownwood.Magic.Controls.TabPage tab  = (Crownwood.Magic.Controls.TabPage)pollingTabControl.TabPages[i];
											PollingTabPage pWnd = (PollingTabPage)tab;
											//PollingTabPage pWnd = (PollingTabPage)pollingTabControl.TabPages[i].Control;
											if(pWnd.pollResult.SharedPoolResult==true)
											{
												pWnd.pollResult.SendCloseMessage();												
											}
										}
										for(int i=0; i< totalSubPages ;i++) // by kamran
										{
											//SharedPoolResult="";
											//pollingTabControl.TabPages[0].Control
											//Crownwood.Magic.Controls.TabPage tab  = (Crownwood.Magic.Controls.TabPage)pollingTabControl.TabPages[i];
											//PollingTabPage pWnd = (PollingTabPage)tab;
											Invoke(DeleteDocumentSharingWindow,new object[]{tabPollingGlobal});
											pollingTabControl.TabPages.RemoveAt(i);
										}
									}
									catch(Exception exp)
									{
										WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5715",exp,null,false);
									
									}
									finally
									{
										pollingTabControl.TabPages.Clear();
									}
									try
									{
										Invoke(DeleteDocumentSharingWindow,new object[]{tabPollingGlobal});										///
									}
									catch(Exception exp)
									{  //Always whenever u close the tab
										//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5715",exp,null,false);
									}
									//tabBody.TabPages.Remove(tabPollingGlobal);
								}
							}
							else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
							{
								ApplicationSharing app = (ApplicationSharing)tabBody.SelectedTab.Control;
								if(app.controlOwnerId == network.profile.ClientId)
								{
									myAppShareStarted= false;
									Crownwood.Magic.Controls.TabPage tabpage=tabBody.SelectedTab;
									tabBody.TabPages.Remove(tabpage);
									ApplicationSharingTabPagesArray.Remove(tabpage);
									//app.DisposeResources();
									app.TerminateEx();
									//app.Dispose();
									app=null;
									tabpage.Control.Dispose();									
									tabpage.Control=null;
									tabpage.Dispose();
									GC.Collect();
									GC.WaitForPendingFinalizers();									
								}
							}
							else
								
							{
								tabBody.TabPages.RemoveAt(tabBody.SelectedIndex);
								
								
								//tabBody.TabPages.Remove(this);
							}

						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5745",exp,null,true);
							
						}
					}
				}
				//MessageBox.Show("==> Before FlushMemory");
				FlushMemory(); //per2
				///MessageBox.Show("==> After FlushMemory");
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5560",exp," Main Body close click event has encountered an exception. "+exp.ToString(),true);
				//ShowExceptionMessage(" Main Body close click event has encountered an exception. " + ee.Message);
			}			
		}

		public void CloseEntireDeskHostToClient(bool bParam) // bParam check the where funcion is calling if callin from niceMenu's click then no need to change the text // by kamran hasnain
		{
			try
			{
				if( bParam ) 
					(myNiceMenu.SearchNiceMenuItem("Close Entire Desktop")).Text = "Entire Desktop";
				//Trace.WriteLine("in CloseEntireDeskHostToClient(): application enabled is true to close entire desktop sharing ");
				(myNiceMenu.SearchNiceMenuItem("Application")).Enabled = true;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5773",exp,null,false);
				//Trace.WriteLine("Exception : CloseEntireDeskHostToClient() 1 to application sharing called.");
			}
			try
			{
				//if(EntireDesktopSharingserver.nSessionID == network.profile.ClientId) // comment to close syncronize
			{
				myAppShareStarted= false;
				//Trace.WriteLine("in CloseEntireDeskHostToClient(): TerminateEx() to application sharing called.");
				EntireDesktopSharingserver.TerminateEx();
			}
				myAppShareStarted= false;
				deskShare.closeDesktopSharingWnd();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5790",exp,null,false);
				//Trace.WriteLine("Exception : CloseEntireDeskHostToClient() 2 to application sharing called.");
			}

		}
		
		public void SynchronizeTabPages()
		{
			tabBody_SelectionChanged(this,null);
		}
		public  void DisableSlideNav(bool status)
		{
			this.btnNext.Enabled=status;
			this.btnPrev.Enabled=status;
		}


		public void SynTabpositioning()
		{
			int sessionId = -1;
			TabPageType tempType = TabPageType.Whiteboard;
			this.tabBody.ShowArrows=true;

					
			try
			{

				if(this.IfIamthePresenter())
				{
				
					if( tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl))||
						tabBody.SelectedTab.Control.GetType().Equals(typeof(browser))
						||(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing))))
					{
						if( tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
						{
							this.tabBody.ShowClose=((documentSharingControl)tabBody.SelectedTab.Control).ShowClose();
							sessionId= ((documentSharingControl)tabBody.SelectedTab.Control).sessionID;
							
							
							
							try
							{
								if(((documentSharingControl)tabBody.SelectedTab.Control).m_DocumentMessage!= null)
								{
									if(network.profile.clientType!=ClientType.ClientAttendee)
									{

										if(((documentSharingControl)tabBody.SelectedTab.Control).m_DocumentMessage.DocumentType==DocumentMessageType.typePresentation)
											this.DisableSlideNav(true);//enable slide show nav bar
										else 
											this.DisableSlideNav(false);//enable slide show nav bar
									}
									else
										this.DisableSlideNav(false);//enable slide show nav bar
								}
							}
							catch(Exception exp)
							{
								WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6025",exp,null,false);
							}
						
						}
						else if( tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
						{						
							this.tabBody.ShowClose=true;						
							sessionId = ((browser)tabBody.SelectedTab.Control).sessionID;					
							this.DisableSlideNav(false);//disable the slide nav buttons
						}
					
					}				
					else
					{
						this.DisableSlideNav(false);//disable the slide nav buttons
						
					}
				
					this._statusBar.Message=tabBody.SelectedTab.Title;
					if((tabBody.SelectedTab.Title == "WhiteBoard") 
						|| (tabBody.SelectedTab.Title == "WhiteBoard - Disabled"))
					{
						this.tabBody.ShowClose = true;
						this.DisableSlideNav(false);//disable the slide nav buttons
						this._statusBar.Message = 	statusBarString;
						tempType = TabPageType.Whiteboard;
					
					}
					else if(tabBody.SelectedTab == nWelcomePage)
					{
						this.tabBody.ShowClose = false;
						this.DisableSlideNav(false);//disable the slide nav buttons
						tempType = TabPageType.WelcomeScreen;
					}
					else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
					{
						this.DisableSlideNav(false);//disable the slide nav buttons
						if(network.profile.clientType == ClientType.ClientAttendee)
						{						
							tabBody.ShowClose = false;						
						}
						else
						{
							ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
							if(!control.isDesktopSharing)
							{
								if(control !=null)
								{
									if(control.controlOwnerId == network.profile.ClientId)
										tabBody.ShowClose = true;		
									sessionId = control.nSessionID;
								}
							}
							else
								tabBody.ShowClose = true;		

						}
					}
					else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(Crownwood.Magic.Controls.TabControl)))
					{
						Crownwood.Magic.Controls.TabControl control = (Crownwood.Magic.Controls.TabControl)tabBody.SelectedTab.Control;
						/////////////////////////
						if(!this.splitter1.IsCollapsed)
							this.splitter1.ToggleState();
						/////////////////////////
						if((control.TabPages.Count != 0) && (control.SelectedTab != null))
						{
							if(control.SelectedTab.GetType().Equals(typeof(Client.PollingTabPage)))
							{
								this.DisableSlideNav(false);//disable the slide nav buttons
								Crownwood.Magic.Controls.TabPage tempPage = (Crownwood.Magic.Controls.TabPage)control.SelectedTab;						
								sessionId  =((WebMeeting.Polling.PollResult)tempPage.Control).polling.sessionID;
								this.tabBody.ShowClose = true;
							}
						}
						else
							this.tabBody.ShowClose = true;
					
					}
					else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(QuestionAnswerAttendee)))
					{
						this.DisableSlideNav(false);//disable the slide nav buttons
						this.tabBody.ShowClose=((QuestionAnswerAttendee)this.tabBody.SelectedTab.Control).ShowClose();
						tempType = TabPageType.QAAttendee;					
					}
					else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(QuestionAnswerPresenter)))
					{
						this.tabBody.ShowClose=((QuestionAnswerPresenter)this.tabBody.SelectedTab.Control).ShowClose();
						this.DisableSlideNav(false);//disable the slide nav buttons		
						tempType = TabPageType.QAAttendee;
					}
					else if(tabBody.SelectedTab.Title.IndexOf("- Application Sharing")!=-1)
					{
						this.tabBody.ShowClose=true;
						this.DisableSlideNav(false);//disable the slide nav buttons
						tempType = TabPageType.DesktopSharing;
					}
					else if(tabBody.SelectedTab.Title == "Manage Content")
					{
						this.tabBody.ShowClose=true;
						this.DisableSlideNav(false);//disable the slide nav buttons
						tempType = TabPageType.ManageContent;
					}
					else if(tabBody.SelectedTab == tabPollingGlobal)
					{					
						PollingTabPage tempPage = (PollingTabPage )pollingTabControl.SelectedTab;					
						sessionId =tempPage.PollingControl.sessionID;
						this.DisableSlideNav(false);//disable the slide nav buttons
						
					}
					
				
					if(this.IfIamthePresenter())
					{
						if(bSendIt)
						{
							if((tabBody.SelectedTab.Title!="Manage Content") && (tabBody.SelectedTab.Title!="Questions & Answers"))
							{
								TabPagesSyncrhonizationMsessage Msg = new TabPagesSyncrhonizationMsessage(network.profile,sessionId);
								Msg.type = tempType;
								NetworkManager.getInstance().SendLoadPacket(Msg);						
							}
						
						}
					}
 
					lastSelectedTab = tabBody.SelectedTab;
					bSendIt = true;
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6216",exp," Main Body selection change event has encountered an exception. " + exp.Message.ToString(),true);
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6216",exp," Main Body selection change event has encountered an exception. " + exp.Message.ToString(),false);
				//ShowExceptionMessage(" Main Body selection change event has encountered an exception. " + ee.Message);
			}
		
		}


		private void tabBody_SelectionChanged(object sender, System.EventArgs e)
		{		
		
			if(this.IfIamthePresenter())
			{
				try
				{
			
					/* check the sender caz sometime Webmeeting.Clinet.ClientUI is sender and exception occur so
					   we check sender type if its  Webmeeing.Client.TabControlEx then we proceed otherwise we return
					*/
					if(!sender.GetType().Equals(typeof(WebMeeting.Client.TabControlEx)))
					{
						return;		
						//MessageBox.Show("==>if ");

					}
					WebMeeting.Client.TabControlEx tab=(WebMeeting.Client.TabControlEx)sender;
					//Crownwood.Magic.Controls.TabControl tab=(Crownwood.Magic.Controls.TabControl)sender;
					if(tab==null)
						return;

				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5820",exp,null,false);
				
				}
				finally
				{

					//MessageBox.Show("Else==>in finally");
					//ToolbarClicked(btnC);
					int sessionId = -1;
					TabPageType tempType = TabPageType.Whiteboard;

					this.tabBody.ShowArrows=true;
					try
					{

						if(this.IfIamthePresenter())
						{
				
							if( tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl))||
								tabBody.SelectedTab.Control.GetType().Equals(typeof(browser))
								||(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing))))
							{
								if( tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
								{
									this.tabBody.ShowClose=((documentSharingControl)tabBody.SelectedTab.Control).ShowClose();
									sessionId= ((documentSharingControl)tabBody.SelectedTab.Control).sessionID;
							
							
							
									try
									{
										if(((documentSharingControl)tabBody.SelectedTab.Control).m_DocumentMessage!= null)
										{
											if(network.profile.clientType!=ClientType.ClientAttendee)
											{

												if(((documentSharingControl)tabBody.SelectedTab.Control).m_DocumentMessage.DocumentType==DocumentMessageType.typePresentation)
													this.DisableSlideNav(true);//enable slide show nav bar
												else 
													this.DisableSlideNav(false);//enable slide show nav bar
											}
											else
												this.DisableSlideNav(false);//enable slide show nav bar
										}
									}
									catch(Exception exp)
									{
										WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6025",exp,null,false);
									}
						
								}
								else if( tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
								{						
									this.tabBody.ShowClose=true;						
									sessionId = ((browser)tabBody.SelectedTab.Control).sessionID;					
									this.DisableSlideNav(false);//disable the slide nav buttons
								}
					
							}				
							else
							{
								this.DisableSlideNav(false);//disable the slide nav buttons
						
							}
				
							this._statusBar.Message=tabBody.SelectedTab.Title;
							if((tabBody.SelectedTab.Title == "WhiteBoard") 
								|| (tabBody.SelectedTab.Title == "WhiteBoard - Disabled"))
							{
								this.tabBody.ShowClose = true;
								this.DisableSlideNav(false);//disable the slide nav buttons
								this._statusBar.Message = 	statusBarString;
								tempType = TabPageType.Whiteboard;
					
							}
							else if(tabBody.SelectedTab == nWelcomePage)
							{
								this.tabBody.ShowClose = false;
								this.DisableSlideNav(false);//disable the slide nav buttons
								tempType = TabPageType.WelcomeScreen;
							}
							else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
							{
								this.DisableSlideNav(false);//disable the slide nav buttons
								if(network.profile.clientType == ClientType.ClientAttendee)
								{						
									tabBody.ShowClose = false;						
								}
								else
								{
									ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
									if(!control.isDesktopSharing)
									{
										if(control !=null)
										{
											if(control.controlOwnerId == network.profile.ClientId)
												tabBody.ShowClose = true;		
											sessionId = control.nSessionID;
										}
									}
									else
										tabBody.ShowClose = true;		

								}
							}
							else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(Crownwood.Magic.Controls.TabControl)))
							{
								Crownwood.Magic.Controls.TabControl control = (Crownwood.Magic.Controls.TabControl)tabBody.SelectedTab.Control;
								/////////////////////////
								if(!this.splitter1.IsCollapsed)
									this.splitter1.ToggleState();
								/////////////////////////
								if((control.TabPages.Count != 0) && (control.SelectedTab != null))
								{
									if(control.SelectedTab.GetType().Equals(typeof(Client.PollingTabPage)))
									{
										this.DisableSlideNav(false);//disable the slide nav buttons
										Crownwood.Magic.Controls.TabPage tempPage = (Crownwood.Magic.Controls.TabPage)control.SelectedTab;						
										sessionId  =((WebMeeting.Polling.PollResult)tempPage.Control).polling.sessionID;
										this.tabBody.ShowClose = true;
									}
								}
								else
									this.tabBody.ShowClose = true;
					
							}
							else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(QuestionAnswerAttendee)))
							{
								this.DisableSlideNav(false);//disable the slide nav buttons
								this.tabBody.ShowClose=((QuestionAnswerAttendee)this.tabBody.SelectedTab.Control).ShowClose();
								tempType = TabPageType.QAAttendee;					
							}
							else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(QuestionAnswerPresenter)))
							{
								this.tabBody.ShowClose=((QuestionAnswerPresenter)this.tabBody.SelectedTab.Control).ShowClose();
								this.DisableSlideNav(false);//disable the slide nav buttons		
								tempType = TabPageType.QAAttendee;
							}
							else if(tabBody.SelectedTab.Title.IndexOf("- Application Sharing")!=-1)
							{
								this.tabBody.ShowClose=true;
								this.DisableSlideNav(false);//disable the slide nav buttons
								tempType = TabPageType.DesktopSharing;
							}
							else if(tabBody.SelectedTab.Title == "Manage Content")
							{
								this.tabBody.ShowClose=true;
								this.DisableSlideNav(false);//disable the slide nav buttons
								tempType = TabPageType.ManageContent;
							}
							else if(tabBody.SelectedTab == tabPollingGlobal)
							{					
								PollingTabPage tempPage = (PollingTabPage )pollingTabControl.SelectedTab;					
								sessionId =tempPage.PollingControl.sessionID;
								this.DisableSlideNav(false);//disable the slide nav buttons
						
							}
					
				
							if(this.IfIamthePresenter())
							{
								if(bSendIt)
								{
									if((tabBody.SelectedTab.Title!="Manage Content") && (tabBody.SelectedTab.Title!="Questions & Answers"))
									{
										TabPagesSyncrhonizationMsessage Msg = new TabPagesSyncrhonizationMsessage(network.profile,sessionId);
										Msg.type = tempType;
										NetworkManager.getInstance().SendLoadPacket(Msg);						
									}
						
								}
							}
 
							lastSelectedTab = tabBody.SelectedTab;
							bSendIt = true;
						}
					}
					catch(Exception exp)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6216",exp," Main Body selection change event has encountered an exception. " + exp.Message.ToString(),true);
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6216",exp," Main Body selection change event has encountered an exception. " + exp.Message.ToString(),false);
						//ShowExceptionMessage(" Main Body selection change event has encountered an exception. " + ee.Message);
					}
				}
			}
		}		
		
		private bool tabBody_OnSelectionChangingEx(int nNewTabPageIndex)
		{
			/*
			if(network.profile.clientType !=ClientType.ClientAttendee)
				return true;

			
			if(nNewTabPageIndex >= tabBody.TabPages.Count )
				return true;

			if((tabBody.TabPages[nNewTabPageIndex] == TabPageQAAttendee)
				|| (nNewTabPageIndex == nLastSelectedIndex))
				return true;
           
			return false;
			*/
			if (this.IfIamthePresenter())
			{return true;}
			else
			return false;
		}
		#endregion
		
		#region sendProfileMessage()
		public void sendProfileMessage()
		{
			try
			{
				ControlMessage msg = new ControlMessage(ControlCode.AttendeeUpdate);
				msg.SenderID = network.profile.ClientId;
				msg.Parameter = network.profile;
				network.SendLoadPacket(msg);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6250",exp,null,false);
			}
		}
		#endregion 

		#region AudioSetMute(bool setMute) ==> Commented
		/*public void AudioSetMute(bool setMute)
		{
			try
			{
				if(setMute)
					ChangeSpeakGroup(0);
				else
					ChangeSpeakGroup(info.VoiceConferenceID);

			}
			catch(Exception ee)
			{
			}
		}
		*/
		#endregion 

		#region DisableMyAudio(Crownwood.Magic.Controls.TabPage tabPage)
		public void DisableMyAudio(Crownwood.Magic.Controls.TabPage tabPage)
		{
			try
			{
				if(!audioEnabled)
					return;
				MyAudioTabPage.Control.Dispose();
				tabControl1.TabPages.Remove(MyAudioTabPage);
				audioEnabled = false;
				
						
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6112",exp,null,false);
				//	ShowExceptionMessage("Unable to close audio session");
			}
			
		}
		#endregion 

		#region ChatControl Coding
		private void chatTabControl_SelectionChanged(object sender, System.EventArgs e)
		{
			try
			{
				if(this.chatTabControl.TabPages.Count<3)
					this.chatTabControl.ShowArrows=false;
				else
					this.chatTabControl.ShowArrows=true;

				if(this.chatTabControl.SelectedTab.Title=="Chat" ||this.chatTabControl.SelectedTab.Title=="Notes"  )
				{
					this.chatTabControl.ShowClose=false;
				}
				else
					this.chatTabControl.ShowClose=true;
			
				if(chatTabControl.SelectedTab.Control.GetType().Equals(typeof(chatControl)))
				{
					((chatControl)chatTabControl.SelectedTab.Control).timer1.Stop();
				}
				//sendTo.Visible = (chatTabControl.SelectedIndex == 0)	;		
				/*if(chatTabControl.SelectedIndex ==0)
					sendTo.SelectedIndex = -1;
					*/
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6325",exp,null,false);
			}


		}
	
		#endregion

		#region Video  Function Exception done by Zaeem

		#region Share My Camera Functions Exception done by zaeem.

		public void SubscribetoUserCameraEx(WebMeeting.Common.ClientProfile clientProfile)
		{
			try
			{
				if(clientProfile.ClientId != network.profile.ClientId)
				{

					tempProfile = clientProfile;
					Win32.USER32.SendMessage(this.Handle,Win32.USER32.WM_SUBSCRIBE_USER_CAMERa,new IntPtr(),new IntPtr());
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6350",exp,null,false);
			}
		}

		public void SubscribetoUserCamera(WebMeeting.Common.ClientProfile clientProfile)
		{
			try
			{
				if(clientProfile.ClientId == network.profile.ClientId)
					return;
				if(clientProfile.videoID < 1) 
					return;
				for(int z = 0 ; z< ClientUI.getInstance().VideoTabPagesArray.Count ; z++)
				{
					Crownwood.Magic.Controls.TabPage tabPage= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().VideoTabPagesArray[z];
					videovoiceControl control = (videovoiceControl) tabPage.Control;
					if(control.associatedProfile != null)
					{
						if((control.associatedProfile.videoID == clientProfile.videoID) && (control.isVideo))
						{
							tabControl1.SelectedTab = tabPage;
							return;	
						}
					}
				}
				               					
				//we create a new object everytime to dispose SWF object
				Crownwood.Magic.Controls.TabPage MyVideoTabPage1 = new Crownwood.Magic.Controls.TabPage("Video Chat");

				videovoiceControl objSwf = new videovoiceControl();
				objSwf.isVideo = true;
				IsHostVideoEnabled = true;
				objSwf.associatedProfile = clientProfile;
				objSwf.LoadMovie();
				objSwf.SubscribeCamera(info.ServerIP,info.ConferenceID,clientProfile.videoID.ToString());				
				MyVideoTabPage1.Control = objSwf;
				
				tabControl1.TabPages.Add(MyVideoTabPage1);	
				MyVideoTabPage1.Name = "Video";
				tabControl1.SelectedTab = MyVideoTabPage1	;		
				VideoTabPagesArray.Add(MyVideoTabPage1);
				if(splitter1.SplitPosition != Win32.USER32.SPLIT_SIZE)
				{
					splitter1.SplitPosition = Win32.USER32.SPLIT_SIZE;
					splitter2.SplitPosition = Win32.USER32.SPLIT_VERT_SIZE;
				}

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6230",exp," Subscribe to User Camera has encountered an exception. "+exp.ToString() ,true);
			}
		}
		public void DisableMyCamera()
		{
			try
			{
				if(!videoEnabled)
					return;
				MyVideoTabPage.Control.Dispose();
				tabControl1.TabPages.Remove(MyVideoTabPage);
				videoEnabled = false;
				IsHostVideoEnabled = false;
						
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6230",exp,"Disable Camera has encountered an exception. "+exp.ToString(),true);
			}
		}
				
		public void ShareMyCameraEx()
		{
			try
			{
				if(videoEnabled)
				{		
					tabControl1.SelectedTab = MyVideoTabPage;
					return;
				}
				if(IsHostVideoEnabled)
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"Current version of Software is not support to more than one instance of Video.",true,false);
					//	MessageBox.Show("Current version of Software is not support to more than one instance of Video.");
					return;
				}

				videoEnabled = true;
				IsHostVideoEnabled = true;
				//we create a new object everytime to dispose SWF object
				MyVideoTabPage = new Crownwood.Magic.Controls.TabPage("My Video");				
				videovoiceControl objSwf = new videovoiceControl();
				objSwf.LoadMovie();
				//MessageBox.Show(info.ServerIP);
				objSwf.ShareMyCamera(info.ServerIP,info.ConferenceID,network.profile.ClientId.ToString());				
				objSwf.nUniqueID = network.profile.ClientId;
				MyVideoTabPage.Control = objSwf;
				tabControl1.TabPages.Add(MyVideoTabPage);				
				tabControl1.SelectedTab = MyVideoTabPage;
				//network.profile.videoID = network.profile.ClientId;
				//sendProfileMessage();
				if(splitter1.SplitPosition != Win32.USER32.SPLIT_SIZE)
				{
					splitter1.SplitPosition = Win32.USER32.SPLIT_SIZE;
					splitter2.SplitPosition = Win32.USER32.SPLIT_VERT_SIZE;
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6243",exp,null,false);			
			}
		}
		public void ShareMyCamera()
		{
			try
			{
				ShareMyCameraEx(); //flashed based implementations
				return;				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6243",exp,null,false);			
			}

		
		}


		#endregion 

		public bool CheckAlreadyVideoSessionAvailable(WebMeeting.Common.ClientProfile profile)
		{
			return false;
                 
		}
				

	
		#endregion			

		#region desktop sharing threads Exception handling Done
		
		public void TerminateDesktopSharing()
		{
			try
			{
				toolbarBtnDesktopSharing.BringToFront();				
				toolbarBtnDesktopSharingDisable.SendToBack();								
				DesktopSharingserver.Terminate();			
				//MessageBox.Show("Desktop Sharing has been stopped","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);

				
				if(!DesktopSharingserver.bIsServer)
				{
					Win32.USER32.SendMessage(this.Handle, 10997,IntPtr.Zero,IntPtr.Zero);
					//Client.ClientUI.getInstance().Invoke(Client.ClientUI.getInstance().DeleteDocumentSharingWindow,new object[] {ClientUI.getInstance().tabPageDesktopSharing});	
				}
				//				tabBody.TabPages.Remove(tabPageDesktopSharing);

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6318",exp,null,false);
				//Console.WriteLine(ee.ToString());
			}
		}
		
		//WebMeeting.ApplicationSharing appShareControl ; //per1
		DesktopCloseWnd deskShare ;
		ApplicationSharing EntireDesktopSharingserver ;
		public void StartEntireDesktopSharing()
		{
			try
			{	
				EntireDesktopSharingserver = new ApplicationSharing();	
				EntireDesktopSharingserver.nSessionID = network.profile.ClientId;
				
				EntireDesktopSharingserver.InitializeServer2(network);	
			
				toolbarBtnDesktopSharingDisable.BringToFront();				
				toolbarBtnDesktopSharing.SendToBack();
				
				deskShare = new DesktopCloseWnd();
				deskShare.Location = new Point(10,10 );
				deskShare.Show();
			}

			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6345",exp,null,false);
			}
		}
		
		public void StartDesktopSharing(int nParticipentIndex)
		{
			try
			{
				if(desktopSharedProfile != null)
				{
					bool bFound = false;
					for(int i = 0 ; i < tabBody.TabPages.Count ; i++)
					{
						if(tabBody.TabPages[i] == tabPageDesktopSharing)
						{
							bFound = true;
							break;
						}
					}
					if(bFound)
					{
						MeetingAlerts alert=new MeetingAlerts();
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"You already have a remote control session.",true,false);
						//MessageBox.Show("You already have a remote control session.","Webmeeting",MessageBoxButtons.OK,MessageBoxIcon.Warning);
						return;
					}
					desktopSharedProfile = null;
				}
				int senderID = network.profile.ClientId;
				if(nParticipentIndex != -1) 
				{
					int RecipientId = ((ClientProfile)arrayParticipents[nParticipentIndex]).ClientId;
					if(RecipientId != network.profile.ClientId)
					{
						DesktopSharingControlMessage message = new DesktopSharingControlMessage(true,senderID,RecipientId);
						message.Status = 0;
						message.Start = true;
						message.RecipientId = RecipientId;
						message.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID;
						network.SendLoadPacket(message);						
					}
					else
					{
						MeetingAlerts alert=new MeetingAlerts();
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"You cannot start desktop sharing with yourself",true,false);
						//MessageBox.Show("You cannot start desktop sharing with yourself","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
					}
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6397",exp," Start Remote Control has encountered an exception. " + exp.Message.ToString(),true);
				//ShowExceptionMessage(" Start Remote Control has encountered an exception. " + ee.Message);
			}
		}
		

		public void InitializeServer(MessageObject DSCMsg,ClientProfile profile)
		{			
			try
			{
				if(desktopSharedProfile != null)
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"You already have a remote control session.",true,false);
					//MessageBox.Show("You already have a remote control session.","Webmeeting",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				bIsDesktopServer = true;
				desktopSharedProfile = profile;								
				DesktopSharingserver.nSessionID =((DesktopSharingControlMessage)DSCMsg).ServerToClientSessionID; //per1
				DesktopSharingserver = new ApplicationSharing();	
				DesktopSharingserver.InitializeServer(network,true);	
			
				toolbarBtnDesktopSharingDisable.BringToFront();				
				toolbarBtnDesktopSharing.SendToBack();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6423",exp,null,false);
			}
		}

		public void InitializeClient(MessageObject DSCMsg,ClientProfile profile)
		{
			try
			{
				if(desktopSharedProfile != null)
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"You already have a remote control session.",true,false);
					//MessageBox.Show("You already have a remote control session.","Webmeeting",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				bIsDesktopServer= false;					
				desktopSharedProfile = profile;								
				tabPageDesktopSharing.Title = Client.ClientUI.getInstance().desktopSharedProfile.Name + " - Remote Control ";
				tabPageDesktopSharing.Visible=true;		
				DesktopSharingserver = new ApplicationSharing();	
				tabPageDesktopSharing.Control = DesktopSharingserver;
				DesktopSharingserver.InitializeClient(network,true);	
				Win32.USER32.SendMessage(Client.ClientUI.getInstance().Handle,10998,new IntPtr(),new IntPtr()); 
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6449",exp,null,false);
			}
			/*
			toolbarBtnDesktopSharingDisable.BringToFront();				
			toolbarBtnDesktopSharing.SendToBack();
			*/
		}
		#endregion

		#region ApplicationSharing
		
		public void openRightWindow()
		{
			if(this.splitter1.IsCollapsed)
				this.splitter1.ToggleState();
		}
		


		public delegate Crownwood.Magic.Controls.TabPage AddTabPageToMainBodyDelegate(Crownwood.Magic.Controls.TabPage pTab);
		public delegate bool RemoveTabPageFromMainBodyDelegate(Crownwood.Magic.Controls.TabPage pTab);
		public AddTabPageToMainBodyDelegate AddTabPageToMainBody;
		public RemoveTabPageFromMainBodyDelegate RemoveTabPageFromMainBody;
		private void CloseApplicaticationSharing(int nSession)
		{
			try
			{
				for(int i = 0 ; i < ApplicationSharingTabPagesArray.Count ;i++)
				{
					Crownwood.Magic.Controls.TabPage tab = (Crownwood.Magic.Controls.TabPage)ApplicationSharingTabPagesArray[i];
					ApplicationSharing appControl = (ApplicationSharing)tab.Control;
					if(appControl.nSessionID == nSession)
					{
						Invoke(DeleteDocumentSharingWindow,new object[]{tab});					
						ApplicationSharingTabPagesArray.Remove(tab);
						return;
					}
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6490",exp,null,false);
			}
		}
		/*
		 return true in the case of presenter & host(when he is presenter)
		 return false in the case of attendee & host(when he is not presenter)
		 */
		public bool IsShowThumbnails()
		{
			if(this.network.profile.clientType==ClientType.ClientAttendee)
			{
				return false;
			}
			else if(this.network.profile.clientType==ClientType.ClientPresenter)	
			{
				return true;
			}
			else if(this.network.profile.clientType==ClientType.ClientHost)	
			{
				# region Normal version
				if(btn_Whiteboard.Enabled==false && btn_Docshare.Enabled==false)
				{
					return false;
				}	
				#	endregion

				# region Stripped down Version
//				if(btn_Whiteboard.Enabled==false )//&& btn_Docshare.Enabled==false)
//				{
//					return false;
//				}	
				#	endregion

				else
				{					
					return true;
				}
				
			}
			
			return false;/*it will only run in the case of guest*/
			
		}
		public void RecieveAppSharingMessage(ref WebMeeting.Common.AppSharingMessageEx appMsgEx)
		{
			
			
			
			
			try
			{
				if(!appMsgEx.bEnabled)
				{
					//MessageBox.Show("Closing session id: " + appMsgEx.sessionId);
					CloseApplicaticationSharing(appMsgEx.sessionId);
				
					return;
				}
				for(int i = 0 ; i < ApplicationSharingTabPagesArray.Count ;i++)
				{
					ApplicationSharing appControl = (ApplicationSharing)((Crownwood.Magic.Controls.TabPage)ApplicationSharingTabPagesArray[i]).Control;
					//ApplicationSharing appControl = (ApplicationSharing)tab.Control;
					if(appControl.nSessionID == appMsgEx.sessionId)
					{
						//IfNativemsg=true;
						appControl.RecieveMessageFromNetwork(ref appMsgEx);
						
						return;
					}
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6517",exp,null,false);
			}
			//create a new window

			try 
			{
			
				if((DataType)appMsgEx.nDataType ==DataType.SizeInformation)
				{
					Crownwood.Magic.Controls.TabPage tab = new Crownwood.Magic.Controls.TabPage("Application Sharing");
				
					ApplicationSharing appControl = new ApplicationSharing();
				

					tab.Control = appControl;
					appControl.nSessionID = appMsgEx.sessionId;
					appControl.InitializeClient(network,false);
				IfNativemsg=true;
					appControl.RecieveMessageFromNetwork(ref appMsgEx);
					Invoke(AddTabPageToMainBody,new object[]{tab});
					ApplicationSharingTabPagesArray.Add (tab);				
					appControl.controlOwnerId = appMsgEx.SenderID;
					tabBody.SelectedTabEx = tab;
					if(!this.splitter1.IsCollapsed)
						this.splitter1.ToggleState();

					whiteBoard.BringToFront();//F1

					//whiteBoard.Clear(true,0);//jj
					//appControl.whiteBoard.Clear(true, 0); //jj
					//appControl.mPreviousImage  = null;


				}
				else
				{
					//make a dummy application message with size information 
					AppSharingMessageEx aTemp = new	AppSharingMessageEx();
					aTemp.sessionId = appMsgEx.sessionId;
					aTemp.SenderID = appMsgEx.SenderID;
					aTemp.nDataType = (int)DataType.SizeInformation;
					aTemp.X = appMsgEx.windowWidth;
					aTemp.Y = appMsgEx.windowHeight;
					aTemp.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID;

					Crownwood.Magic.Controls.TabPage tab = new Crownwood.Magic.Controls.TabPage("Application Sharing");				
					ApplicationSharing appControl = new ApplicationSharing();
					tab.Control = appControl;
					appControl.nSessionID = appMsgEx.sessionId;
					appControl.InitializeClient(network,false);
					appControl.RecieveMessageFromNetwork(ref aTemp);
					appControl.RecieveMessageFromNetwork(ref appMsgEx);
					ApplicationSharingTabPagesArray.Add(tab);
					Invoke(AddTabPageToMainBody,new object[]{tab});			
					appControl.controlOwnerId = appMsgEx.SenderID;
					tabBody.SelectedTabEx = tab;

					if(!this.splitter1.IsCollapsed)
						this.splitter1.ToggleState();

					whiteBoard.BringToFront();//F1

				
					//whiteBoard.Clear(true,0);	//jj 
					//appControl.whiteBoard.Clear(true, 0); //jj
					//appControl.mPreviousImage  = null;
				}
			}
			catch(Exception exp)
			{

				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6490",exp,exp.Message.ToString(),true);
				//MessageBox.Show(e.ToString());
			}
		}
		public void StartApplicationSharingEx()
		{
			var_temp_appshare=0;
			/*
				public int var_no_appshare=1;
				public int var_no_docshare=1;
				public int var_temp_appshare=0;
				public int var_temp_docshare=0;
			*/

			for(int i=0;i<tabBody.TabPages.Count;i++)
			{
				//MessageBox.Show(tabBody.TabPages[i].Title.ToString());
				if(tabBody.TabPages[i].Title.ToString().Equals("Application Sharing"))
				{
					++var_temp_appshare;
				}

			}

			//if(var_temp_appshare<=this.var_no_appshare)
			if(var_temp_appshare<1)
			{
				try
				{					
					if(myAppShareStarted)
					{
						MeetingAlerts alert=new MeetingAlerts();
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"You already have an application sharing session",true,false);
						//MessageBox.Show("You already have an application sharing session","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
						return;
					}
		
				
					////true);
					myAppShareStarted = true;
					Crownwood.Magic.Controls.TabPage appTabPage = new Crownwood.Magic.Controls.TabPage("Application Sharing");
					WebMeeting.ApplicationSharing appShareControl = new ApplicationSharing();
					appTabPage.Control = appShareControl;
					_appShareControl = appShareControl;
					appShareControl.controlOwnerId = network.profile.ClientId;
					int nSession = 0;
					network.GetUniqueSessionID(ref nSession);
					appShareControl.nSessionID = nSession;
					appShareControl.InitializeServer(network, false);			
					tabBody.TabPages.Add(appTabPage);
					ApplicationSharingTabPagesArray.Add(appTabPage);
			
					tabBody.SelectedTab = appTabPage;
					if(!this.splitter1.IsCollapsed)
						this.splitter1.ToggleState();
				}
				catch(Exception exp)
				{
					ShowExceptionMessage("Module ::: AppShare 	public void StartApplicationSharingEx()",exp,null,false);
				}
			}
			else

			{
				this.ShowExceptionMessage("Sorry! Another application has already been shared by some user in this meeting");
			
				//MessageBox.Show("Sorry! Another user has shared the application already");
			
			}

		}
		#endregion

		#region Image LoadImageFromResource(string ImageName)
		public Image LoadImageFromResource(string ImageName)
		{
			try
			{
				Stream imgStream = null;
				//Bitmap bmp = null;            

				// get a reference to the current assembly
				Assembly a = Assembly.GetExecutingAssembly();		
			
				string [] resNames = a.GetManifestResourceNames();
				// attach to stream to the resource in the manifest
				imgStream = a.GetManifestResourceStream(ImageName);
				if( !(null==imgStream) )
				{                    
					// create a new bitmap from this stream and 
					// add it to the arraylist
					Image tempImage = Image.FromStream(imgStream);				
					imgStream.Close();
					imgStream = null;
					return tempImage;
				}
				return null;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6655",exp,null,false);
				return null;
			}
		}

		#endregion 

		#region Implementation of acesss rights to enable disable menu items
		public void ChangeMenuItemState(MenuItem menuItem,bool state)
		{
			Utility.NiceMenu.NiceMenu mItem = ClientUI.getInstance().myNiceMenu.SearchNiceMenuItem(menuItem.Text);
			if(mItem != null)
				mItem.Enabled = state;
		}
		
				
		private void ChangeMenuItemCheckedState(MenuItem menuItem,bool state)
		{
			Utility.NiceMenu.NiceMenu mItem = myNiceMenu.SearchNiceMenuItem(menuItem.Text);
			if(mItem != null)
				mItem.Checked= state;
		}
		//this function updates the main menu of the applicationj

		/// <summary>
		/// On profile changes the Menus are Updated accordingly.
		/// A lot of extra work is done here 
		/// 1 if this is fo rMenu then Why buttons are changes here ?
		/// 2 Why do we get the controls of web or document or stuffs like that 
		/// 3 Why the drawing colors are assigned here
		/// 4 Why the chat tab is controled here.
		/// 5 This method is suppose to be called only when we get the AttendeUpdate msgs
		/// then why all the stuff is done here or whenever menus and rights needed to be changed.
		/// </summary>
		/// 
		
		
		// Currently this method is called from the following locations 



		//   1 public void ConferenceClosed()
		//   ClientUI.cs(3792):				UpdateMainMenuAccess(); 
		
		
		//   private void networkConnectThread()
		//   ClientUI.cs(3901):				UpdateMainMenuAccess();

		//	public void ConnectionDisabled()
		//  ClientUI.cs(3986):				UpdateMainMenuAccess();    

		//	public void Connection Enabled()
		//  ClientUI.cs(4076):				UpdateMainMenuAccess();    

		// UpdateClientProfile(int nSenderID,int i,ref ClientProfile client,ref ClientProfile clientProf)
		// NetworkManager.cs(570):				Client.ClientUI.getInstance().UpdateMainMenuAccess();

		// In receive thread AttendeeUpdate message
		
		public void UpdateMainMenuAccess()
		{
			
			
			ClientProfile profile=null;
			// Why this try catch block is here.
			// There shd be a good internal checking mechanisam rather then Try catch.
			try
			{
				profile = network.profile;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7105  Profile problem",ex,null,false);
			}

			if(profile.clientType == ClientType.ClientAttendee)
			{


				# region Attendee Block 

				ChangeMenuItemState(menuOpenRecorder,false);							
				this.enableMenus(false);
				
				btn_Whiteboard.Enabled=false;
				btn_Appshare.Enabled = false;
				btn_Record.Enabled = false;
				btn_Docshare.Enabled = false;
				btn_Webshare.Enabled = false;
				btn_Video.Enabled  = false;
				btn_Managecontent.Enabled = false;
				btn_Audio.Enabled = false;
				toolbarBtnDesktopSharing.Enabled = false;
				btn_Invite.Enabled= false;
				







				// Zaeem View 
				// Why do we get these controls in UpdatemainMenus ?
				// BAD THING 
				
				for(int i = 0 ; i < arraydocumentSharing.Count; i++)
				{
					WebMeeting.Client.documentSharingControl _control = (documentSharingControl)((Crownwood.Magic.Controls.TabPage)arraydocumentSharing[i]).Control;
					//_control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.None;				
				}
			
				
				for(int i = 0 ; i < this.arrayWebControl.Count;i++)
				{
					
					//browser _control = (browser)arrayWebControl[i];
					Crownwood.Magic.Controls.TabPage tabBrowser = (Crownwood.Magic.Controls.TabPage)arrayWebControl[i];
					browser _control = (browser)tabBrowser.Control;
					//_control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.None;				
				}
				for(int i = 0 ; i < ApplicationSharingTabPagesArray.Count ;i++)
				{
					Crownwood.Magic.Controls.TabPage tab = (Crownwood.Magic.Controls.TabPage)ApplicationSharingTabPagesArray[i];
					ApplicationSharing appControl = (ApplicationSharing)tab.Control;
				}				
				
				
				# endregion 
			}



			else if(profile.clientType == ClientType.ClientPresenter)
			{		
			
				// Buttons shd be enabled or disabled in one function.
				# region Presenter Block 

				this.enableMenus(true);
				
				#region Normal version
				
				btn_Invite.Enabled = true;
				btn_Video.Enabled  = true;
				btn_Record.Enabled = true;
				btn_Audio.Enabled =true;
				btn_Managecontent.Enabled = true;
				toolbarBtnDesktopSharing.Enabled = (profile.clientAccess.accessDesktopSharing);
				btn_Appshare.Enabled = profile.clientAccess.accessApplicationSharing;
				btn_Whiteboard.Enabled=profile.clientAccess.accessDrawWhiteBoard;
				btn_Docshare.Enabled = profile.clientAccess.accessSharePolling;
				btn_Webshare.Enabled  = profile.clientAccess.accessShareWebbrowser;
				
				# endregion 



				#region Stripped Down Version
//				
//				btn_Invite.Enabled = false;
//				btn_Video.Enabled  = false;
//				btn_Record.Enabled = false;
//				btn_Audio.Enabled =false;
//				btn_Managecontent.Enabled = false;
//				toolbarBtnDesktopSharing.Enabled = false;
//				btn_Appshare.Enabled = false;
//				btn_Docshare.Enabled = false;
//
//				btn_Whiteboard.Enabled=true;
//				btn_Webshare.Enabled  = true;

				# endregion 



				// this new tab is related to chat but i am not sure that what it does and 
				//why its here ?
				if(chatTabControl.TabPages.IndexOf(tabPageNotes) == -1)
					Win32.USER32.SendMessage(this.Handle,Win32.USER32.WM_ADD_NOTES_TAB,new IntPtr(),new IntPtr());
				
				
				//				//Enable disable whiteboard according to access rights
				//
				//				if(profile.clientAccess.accessSharePresentation)
				//					ChangeMenuItemState(menuPresentation,true);
				//				else
				//					ChangeMenuItemState(menuPresentation,false);

				// 
				whiteBoard.DrawingColor = profile.m_AssignedColor;
				//MessageBox.Show("==>");
				//By Zaeem 
				//whiteBoard.Enabled =profile.clientAccess.accessDrawWhiteBoard;
				whiteBoard.Enabled =true;
			
				

				//
				//Zaeem View 
				// Thats a looks a rediculus piece of Bock to me 
				// BAD

				if(!whiteBoard.Enabled)
					tabPageInfo.Title = "WhiteBoard - Disabled";
				else
					tabPageInfo.Title = "WhiteBoard";


				


				// Zaeem View 
				// This If condition does nothing then why its here
				if(profile.clientAccess.accessApplicationSharing != true) 
				{
					//ChangeMenuItemState(menuApplication,false);
					//ChangeMenuItemCheckedState(menuApplication,false);					
				}
				else
				{
					bool bVal = false;
					if(!myAppShareStarted)
						bVal = true;						
					//ChangeMenuItemState(menuApplication,bVal);
					//ChangeMenuItemCheckedState(menuApplication,!bVal);					
					
				}
								
				
				// Zaeem View 
				// According to current Business Logic 
				// This block of code means nothing as presenter will ALWAYS have the annotation rights 
				//
				if(!profile.clientAccess.accessDrawWhiteBoard)					
				{

					// Zaeem View 
					// Again why do we get these extra controls 
					// 
					for(int i = 0 ; i < arraydocumentSharing.Count; i++)
					{
						WebMeeting.Client.documentSharingControl _control = (documentSharingControl)((Crownwood.Magic.Controls.TabPage)arraydocumentSharing[i]).Control;
						//_control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.None;				
					}
					for(int i = 0 ; i < this.arrayWebControl.Count;i++)
					{
				
						Crownwood.Magic.Controls.TabPage tabBrowser = (Crownwood.Magic.Controls.TabPage)arrayWebControl[i];
						browser _control = (browser)tabBrowser.Control;
						//_control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.None;
					}
					
					for(int i = 0 ; i < ApplicationSharingTabPagesArray.Count ;i++)
					{
						Crownwood.Magic.Controls.TabPage tab = (Crownwood.Magic.Controls.TabPage)ApplicationSharingTabPagesArray[i];
						ApplicationSharing appControl = (ApplicationSharing)tab.Control;
						//appControl.whiteBoard.tool= WhiteboardToolCode.None;					
					}				
				}
				# endregion 

			
			}
			else if(profile.clientType == ClientType.ClientHost) // if its a presenter
			{
			
				# region Host

				# region Stripped down version 
				/*
				btn_Invite.Enabled = false;
				btn_Record.Enabled = false;
				btn_Audio.Enabled = false;
				btn_Managecontent.Enabled = false;
				btn_Video.Enabled  = false;
				btn_Appshare.Enabled = false;
				btn_Docshare.Enabled = false;
				btn_Whiteboard.Enabled = true;
				btn_Webshare.Enabled = true;
				*/
				# endregion 


				# region Normal version 
				
				btn_Invite.Enabled = true;
				btn_Record.Enabled = true;
				btn_Audio.Enabled = true;
				btn_Managecontent.Enabled = true;
				btn_Video.Enabled  = true;
				btn_Appshare.Enabled = true;
				btn_Whiteboard.Enabled = true;
				btn_Docshare.Enabled = true;
				btn_Webshare.Enabled = true;
				
				# endregion 

			
				
				if(chatTabControl.TabPages.IndexOf(tabPageNotes) == -1)
					Win32.USER32.SendMessage(this.Handle,Win32.USER32.WM_ADD_NOTES_TAB,new IntPtr(),new IntPtr());
				
				
				profile.clientAccess.accessSharePresentation = true;

				


				bool bVal = false;
				if(desktopSharedProfile== null)
					bVal = true;	
				//by zaeem	to hide dektop from menu;
				//ChangeMenuItemState(menuDesktop,bVal);
				//ChangeMenuItemCheckedState(menuDesktop,!bVal);	

				if(!myAppShareStarted)
					bVal = true;						

				# endregion 
							
			}
			
		}





		/// <summary>
		/// 
		/// </summary>

		public void ChangeAccessCodesEx()
		{
			try
			{
			
				WebMeeting.Client.AccessCodesForm frm = new AccessCodesForm();
				frm.Text = "All Attendee - Access Codes";					
				frm.AttendeeRadio.Checked = true;						
				frm.ShowDialog();
				if(frm.DialogResult != DialogResult.OK)
					return;

				for(int i = 0 ; i < arrayParticipents.Count ; i++)
				{
					ClientProfile profile = (ClientProfile)arrayParticipents[i];
					if((profile.ClientId != network.profile.ClientId) &&(profile.clientType == ClientType.ClientAttendee))
					{
						
						if(profile.IsGuest)
							continue;
						profile.m_AssignedColor = frm.color;

						if(frm.AttendeeRadio.Checked == true)
							profile.clientType = ClientType.ClientAttendee;
						else
							profile.clientType = ClientType.ClientPresenter;
	            	    	
						profile.clientAccess.accessApplicationSharing = frm.chkApplication.Checked ;
						profile.clientAccess.accessShareAudio = frm.chkAudio.Checked ;
						profile.clientAccess.accessDesktopSharing = frm.chkDesktop.Checked ;
						profile.clientAccess.accessDrawWhiteBoard = frm.chkDrawing.Checked ;
						profile.clientAccess.accessSharePresentation = frm.chkPresentation.Checked ;
						profile.clientAccess.accessShareScreen = frm.chkScreen.Checked ;
						profile.clientAccess.accessShareVideo = frm.chkVideo.Checked ;
						profile.clientAccess.accessSharePolling = frm.chkPolling.Checked  ;
						profile.clientAccess.accessPrivateChat = frm.chkPvtChat.Checked;
						profile.clientAccess.accessAllowRightsAssignments = frm.chkAssignRights.Checked ;
						profile.m_AssignedColor = frm.color;
						//Trace.WriteLine("line 6054 message of attendee update");
						ControlMessage msg = new ControlMessage(ControlCode.AttendeeUpdate);			
						msg.Parameter = profile;		

						network.SendLoadPacket(msg);
						network.myRecvMsgList.Add(msg);
					}
				}
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7200",exp," Change Access Codes has encountered an exception. " + exp.Message.ToString(),true);
				//ShowExceptionMessage(" Change Access Codes has encountered an exception. " + ee.Message);
			}

		}

		//This function updates the context menu of participents list control
		public void ChangeAccessCodes(int nIndex)
		{
			try
			{
				
				ClientProfile profile = (ClientProfile)arrayParticipents[nIndex];
				
				if(profile.ClientId == network.profile.ClientId)
					return;
				if(profile.clientType == ClientType.ClientHost)
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"You cannot change rights of a Host",true,false);
					//MessageBox.Show("You cannot change rights of a Host","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return;
				}
				if(profile.IsGuest)
				{
					//MessageBox.Show("You cannot upgrade access rights of a Guest","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
					//return;
				}

				WebMeeting.Client.AccessCodesForm frm = new AccessCodesForm();
				frm.Text = profile.Name + " - Access Codes";
				frm.chkApplication.Checked = profile.clientAccess.accessApplicationSharing;
				frm.chkAudio.Checked = profile.clientAccess.accessShareAudio;
				frm.chkDesktop.Checked = profile.clientAccess.accessDesktopSharing;
				frm.chkDrawing.Checked = profile.clientAccess.accessDrawWhiteBoard;
				frm.chkPresentation.Checked = profile.clientAccess.accessSharePresentation;
				frm.chkScreen.Checked = profile.clientAccess.accessShareScreen;
				frm.chkVideo.Checked = profile.clientAccess.accessShareVideo;
				frm.chkPolling.Checked = profile.clientAccess.accessSharePolling;
				frm.chkPvtChat.Checked = profile.clientAccess.accessPrivateChat ;
				frm.chkAssignRights.Checked = profile.clientAccess.accessAllowRightsAssignments ;
				if(profile.clientType == ClientType.ClientAttendee)
					frm.AttendeeRadio.Checked = true;
				else
					frm.PresenterRadio.Checked =  true;

				frm.color = profile.m_AssignedColor;
				frm.ShowDialog();
				if(frm.DialogResult != DialogResult.OK)
					return;

				profile.m_AssignedColor = frm.color;

				if(frm.AttendeeRadio.Checked == true)
					profile.clientType = ClientType.ClientAttendee;
				else
					profile.clientType = ClientType.ClientPresenter;
	            	    
				profile.m_AssignedColor = frm.color;
				profile.clientAccess.accessApplicationSharing = frm.chkApplication.Checked ;
				profile.clientAccess.accessShareAudio = frm.chkAudio.Checked ;
				profile.clientAccess.accessDesktopSharing = frm.chkDesktop.Checked ;
				profile.clientAccess.accessDrawWhiteBoard = frm.chkDrawing.Checked ;
				profile.clientAccess.accessSharePresentation = frm.chkPresentation.Checked ;
				profile.clientAccess.accessShareScreen = frm.chkScreen.Checked ;
				profile.clientAccess.accessShareVideo = frm.chkVideo.Checked ;
				profile.clientAccess.accessSharePolling = frm.chkPolling.Checked  ;
				profile.clientAccess.accessPrivateChat = frm.chkPvtChat.Checked ;
				profile.clientAccess.accessAllowRightsAssignments = frm.chkAssignRights.Checked ;
				//Trace.WriteLine("line 6139 message of attendee update");
				ControlMessage msg = new ControlMessage(ControlCode.AttendeeUpdate);			
				msg.Parameter = profile;
				network.SendLoadPacket(msg);
				network.myRecvMsgList.Add(msg);
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7250",exp," Change Access Codes has encountered an exception. " + exp.Message.ToString(),true);
				//ShowExceptionMessage(" Change Access Codes has encountered an exception. " + ee.Message);
			}

		}


		#endregion

		#region Sending and Recieving greetings Messages Exception handling Done
		//Notifier window when clicked
		private void notifier_ContentClick(object sender, EventArgs e)
		{
			notifier.Dispose();
		}
		//Notifier window when closed
		private void notifier_CloseClick(object sender, EventArgs e)
		{
			notifier.Dispose();

		}

		
		public void ShowRecievedGreetingsMessage(GreetingsMessage m)
		{
			try
			{
				globalGreetingsMessage = m;
				//MessageBox.Show("Greetings From " + m.senderProfile.Name + ": " + m.MessageString,"WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);				
				Win32.USER32.SendMessage(this.Handle,Win32.USER32.WM_SHOW_GREETINGS,new IntPtr(),new IntPtr());
							
			}
			
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6490",exp,"Error Displaying Greetings Message " + exp.Message.ToString(),true);
			}            
			
		}

		public void SendGreetings(int nIndex)
		{
			try
			{
				ClientProfile cp = (ClientProfile)arrayParticipents[nIndex];
				if(cp.ClientId == network.profile.ClientId)
					return;

				WebMeeting.Client.Greetings	GreetingsDialog = new Greetings();
				GreetingsDialog.ShowDialog();
			

				if(GreetingsDialog.DialogResult != DialogResult.OK)
					return;

				GreetingsMessage msg = new GreetingsMessage(cp.ClientId,network.profile,GreetingsDialog.txtBoxMsg.Text);
				network.SendLoadPacket(msg);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 6490",exp,null,false);
			}
            
		}
		#endregion

		#region audio Module

		private void audioSendingThread()
		{
			try
			{
				int nCount;
				while(1==1)
				{
					if(audioMessageList.Count < 1)
					{
						Thread.Sleep(100);
						continue;
					}
					nCount = audioMessageList.Count;
					if(nCount > 25)
					{
						audioMessageList.RemoveRange(0,25);					
					}
					nCount = audioMessageList.Count;					
					for(int i = 0 ; i < nCount ; i++)
						network.SendAudioPacket((AudioMessage)audioMessageList[i]);

					audioMessageList.RemoveRange(0,nCount);
				
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7150",exp,null,false);}
		}
		private void DataCallback(IntPtr data, int size)
		{
			if (m_RecBuffer == null || m_RecBuffer.Length < size)
				m_RecBuffer = new byte[size];
			System.Runtime.InteropServices.Marshal.Copy(data, m_RecBuffer, 0, size);
			AudDllFuncs.GetEncodedBuffer(compressBuff,m_RecBuffer);
			AudioMessage msg = new AudioMessage();
			//msg.audioID = audioTabPage.sessionID;
			msg.compressedData = compressBuff;
			audioMessageList.Add(msg);
            		
			
		}
		private void Stop()
		{

			/*	if (m_Player != null)
					try
					{
						m_Player.Dispose();
					}
					finally
					{
						m_Player = null;
						
					}*/
			try
			{
				if (m_Recorder != null)
					try
					{
						m_Recorder.Dispose();
					}
					finally
					{
						m_Recorder = null;
					}
			}
			catch(Exception exp)// ee)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7370",exp,null,false);
			}

			
		}

		public void DisableMyAudio()
		{
			try
			{
				if(!audioEnabled)
					return;
				videovoiceControl objSwf = (videovoiceControl)MyAudioTabPage.Control;
				objSwf.DisableMyAudio();							
				MyAudioTabPage.Control.Dispose();
				tabControl1.TabPages.Remove(MyAudioTabPage);
				audioEnabled = false;		
		
				network.profile.audioID= -1;//network.profile.ClientId;				
				sendProfileMessage();
				objSwf = null;


			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>frm_Main.cs line==>ShareMyAudioEx() 7250",exp,"Unable to disable Audio",true);			

			}
		}
		public void ShareMyAudioEx()
		{
			try
			{				
				if(audioEnabled)
				{
					//tabControl1.TabPages.Add(MyAudioTabPage);
					tabControl1.SelectedTab = MyAudioTabPage;
					return;
				}		
				MyAudioTabPage = new Crownwood.Magic.Controls.TabPage("My Audio");				
				videovoiceControl objSwf = new videovoiceControl();
				objSwf.LoadMovie();
				objSwf.ShareMyAudio(info.ServerIP,info.ConferenceID,network.profile.ClientId.ToString());				
				MyAudioTabPage.Control = objSwf;
				tabControl1.TabPages.Add(MyAudioTabPage);
				tabControl1.SelectedTab = MyAudioTabPage;
				network.profile.audioID= network.profile.ClientId;				
				sendProfileMessage();
				audioEnabled = true;
				if(splitter1.SplitPosition != Win32.USER32.SPLIT_SIZE)
				{
					//Trace.WriteLine("splitter1 is changeed" +"Splitter is changed");
					splitter1.SplitPosition = Win32.USER32.SPLIT_SIZE;
					splitter2.SplitPosition = Win32.USER32.SPLIT_VERT_SIZE;
				}

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>frm_Main.cs line==>ShareMyAudioEx() 7250",exp,"Unable to start audio message ",true);			
				//ShowExceptionMessage("Unable to start audio message");				
			}         

		}
		public delegate void DelegateRemoveAudioVideoTabPage(int nIndex);
		public DelegateRemoveAudioVideoTabPage RemoveAudioVideoTabPage;


		public void ShareMyAudio()
		{		
          
			ShareMyAudioEx();
			return;
		
		}
	
		public void SubscribetoUserAudioEx(WebMeeting.Common.ClientProfile clientProf)
		{
			if(clientProf.ClientId != network.profile.ClientId)
			{
				tempProfile = clientProf;
				Win32.USER32.SendMessage(this.Handle,Win32.USER32.WM_SUBSCRIBE_USER_AUDIO,new IntPtr(),new IntPtr());
			}
		}
		public void SubscribetoUserAudio(WebMeeting.Common.ClientProfile clientProf)
		{
			try
			{
				ClientProfile hostProfile  = clientProf;
				if(hostProfile == null)
				{
					if(listParticipents.SelectedItems.Count < 1)
						return;

				

					hostProfile = (ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index];
				}
						
				for(int z = 0 ; z< ClientUI.getInstance().VideoTabPagesArray.Count ; z++)
				{
					Crownwood.Magic.Controls.TabPage tabPage= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().VideoTabPagesArray[z];
					videovoiceControl control = (videovoiceControl) tabPage.Control;
					if(control.associatedProfile != null)
					{
						if((control.associatedProfile.audioID == clientProf.audioID) && (!control.isVideo))
						{
							tabControl1.SelectedTab = tabPage;
							return;	
						}
					}
				}

			
				//we create a new object everytime to dispose SWF object
				Crownwood.Magic.Controls.TabPage MyVideoTabPage = new Crownwood.Magic.Controls.TabPage("Audio Chat");
				videovoiceControl objSwf = new videovoiceControl();
				objSwf.associatedProfile = clientProf;
				objSwf.LoadMovie();
				objSwf.SubscribeAudio(info.ServerIP,info.ConferenceID,clientProf.audioID.ToString(),clientProf.Name);				
				MyVideoTabPage.Control = objSwf;
				tabControl1.TabPages.Add(MyVideoTabPage);									
				VideoTabPagesArray.Add(MyVideoTabPage);
				if(splitter1.SplitPosition != Win32.USER32.SPLIT_SIZE)
				{
					splitter1.SplitPosition = Win32.USER32.SPLIT_SIZE;
					splitter2.SplitPosition = Win32.USER32.SPLIT_VERT_SIZE;
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7500",exp,null,false);
			}
		}
				
		bool bCreated = false;
	
		public void ReceiveAudioMessagFromNetwork(WebMeeting.Common.AudioMessage message)
		{

			
			try
			{
				if(!bCreated)
				{

					Stop();
					/*
					System.IO.FileStream fs = new System.IO.FileStream(@"C:\networkRecieved.wav",System.IO.FileMode.CreateNew);					
					br = new System.IO.BinaryWriter(fs);
					*/
					bCreated = true;
					AudDllFuncs.InitDecoder();
				}				
				/*	
				for(int i = 0 ; i < subscribedAudioList.Count ; i ++)
				{
					CaudioTabPage tabPage = (CaudioTabPage)	subscribedAudioList[i];
					if(tabPage.sessionID == message.audioID)
					{
						tabPage.ConsumeMessage(message);
						break;
					}					
				}*/
				
							

				//br.Write(uncompressedBuff);
				
				//AudioCodec.PlaySoundData(uncompBuffer,1600);
			}
			catch(Exception ee)
			{
				WebMeeting.Error.Show(ee.Message);
			}


		}

		
		#endregion

		# region TakeSnapShot() Excepion handling Done by Zaeem
		private void TakeSnapShot()
		{
			new ClientUI(2);		
			
			try
			{
				tempImage=GetScreenShot(tabBody.SelectedTab.Control.Handle);
				SaveFileDialog ff = new SaveFileDialog ();
				ff.Filter = "Jpeg Files(*.jpg)|*.jpg";		
				
				btn_Snapshot.Enabled=false;
				if( ff.ShowDialog() == DialogResult.OK)
				{
					if(tempImage != null)
					{
						tempImage.Save(ff.FileName,System.Drawing.Imaging.ImageFormat.Jpeg);
					}
					Thread.Sleep(3000);
					btn_Snapshot.Enabled=true;
				
				}
				else 
					
				{
					btn_Snapshot.Enabled=false;
					Thread.Sleep(1000);
					btn_Snapshot.Enabled=true;				
				}						
				ff.Dispose();
				tempImage.Dispose();

			}
			
				
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7278",exp,"Unable to take Snapshot" ,true);
				btn_Snapshot.Enabled=true;
				//ShowExceptionMessage("Unable to take snapshot");
				//ShowExceptionMessage(exp.StackTrace.ToString());
			}

			
		}

		# endregion 


		# region TakeSnapShot_MM(string path) 
		/// <summary>
		/// This function is used in Minuts of meeting to take the snap shot of the particular module.
		/// </summary>
		public void TakeSnapShot_MM(string path)
		{
			path+=".jpg";
			try
			{
				tempImage=GetScreenShot(tabBody.SelectedTab.Control.Handle);
				
					if(tempImage != null)
					{
						tempImage.Save(path,System.Drawing.Imaging.ImageFormat.Jpeg);
					}
					tempImage.Dispose();
			}
			
				
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7278",exp,"Unable to take Snapshot" ,true);
				btn_Snapshot.Enabled=true;
				//ShowExceptionMessage("Unable to take snapshot");
				//ShowExceptionMessage(exp.StackTrace.ToString());
			}

			
		}

		# endregion 

		

		# region TakeSnapShot_MM_WebBrowser(string path) 
		/// <summary>
		/// This function is used in Minuts of meeting to take the snap shot of the particular module.
		/// </summary>
		public bool TakeSnapShot_MM_WebBrowser(string path)
		{
			path+=".jpg";
			
			try
			{
				
				
				if(browserMM != null)
				{
					browserMM.TakeBrowseJPG(path);
					return true;
				}
				else
				{
					Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arrayWebControl[0];
					browserMM = (browser)tabPage.Control;								
					if(browserMM.TakeBrowseJPG(path))
						return true;
					else 
						return false;
					
				
				}
				
			}
			
				
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7278",exp,"Unable to take Snapshot" ,true);
				btn_Snapshot.Enabled=true;
				return false;
				//ShowExceptionMessage("Unable to take snapshot");
				//ShowExceptionMessage(exp.StackTrace.ToString());
			}

			
		}

		# endregion 

		
		
		
		




//                                    DATA BASE ON CLIENT SIDE
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//**********************************************************************************************************//
//**********************************************************************************************************//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	

		public int Noof_records()
		{
			return db.GetNumberofRecordsInTable("Client_MM");
		
		}

		public bool Save_FileInfo_TobeUpload(int meetingID,string Host,string  Presenter,string  Module,string  path,string Current_DateTime,string Mis,string relativePath)
		{
		
			if(db.Save_FileInfo_TobeUpload(meetingID,Host,Presenter,Module,path+".jpg",Current_DateTime,Mis,relativePath))
			{
				//fromdb_to_Upload();
				return true;
				

			}
			else
				return false;


		}

		/// <summary>
		/// This function copies all the contents in a file first and then Upload them 
		/// by taking the Ftp Imformation from the configuration file.
		/// if this method returns true then it should send the message to the Server to add those records 
		/// in the database.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool fromdb_to_Upload()
		{
		
			//decide, for which module the uploading is gonna done.
			int count =db.GetNumberofRecordsInTable("Client_MM");
			int ratio=(int)100/count;
			try
			{
				if(count>0)
				{
			
					Mes_MinutsofMeeting msg_Minuts_Server= new Mes_MinutsofMeeting();
			

					# region "FTP login Module"
					try
					{
						fcu.Login();
					}
					catch(Exception exp)
					{
						fcu=new FtpClient(ConfigurationSettings.AppSettings["FtpServer"].ToString(),ConfigurationSettings.AppSettings["FtpServer_Username"].ToString(),ConfigurationSettings.AppSettings["FtpServer_Password"].ToString(),2000,21);
						fcu.Login();
					}
					#endregion

		
					ArrayList arr_files_toUPload=db.Getfilepath_fromDb();
					ArrayList arr=new ArrayList();
				

					//These are the indexes which are gona return
					/*
						* Datetime     (0)
						* Host		   (1)
						* Meetingid    (2)
						* Mis		   (3)
						* module	   (4) 
						* path		   (5)
						* presenter    (6)
						* relativeurl  (7)
						*/

					// if the file got uploaded successfully 
					// then it will make the message 
					// send the msg object to the Server 
					// and server will save it in the db
					// After that the record will be deleted from the database.
		    
					for (int i=0;i<arr_files_toUPload.Count;i++)
					{

						try
						{

							if (i==(arr_files_toUPload.Count-1))
							{
								pb.pb_MM.Value=100;
							}
								// So to avoid exception 
							else if(pb.pb_MM.Value+ratio<100)
							{
								pb.pb_MM.Value+=ratio;
							}
						}
						catch(Exception ){}
						
						arr=(ArrayList) arr_files_toUPload[i];
							
							
						# region "Uploading module"
						// First check the module and meeting ID to make the directory

							
									
						if(((string)arr[4]).Equals("Whiteboard"))
						{


							fcu.MakeDir("\\Minuts\\"+(string)arr[2]+"\\Whiteboard\\");
							fcu.ChangeDir("\\Minuts\\"+(string)arr[2]+"\\Whiteboard\\");
							msg_Minuts_Server.Module="Whiteboard";
									
						}

						else if(((string)arr[4]).Equals("Appshare"))
						{


							fcu.MakeDir("\\Minuts\\"+(string)arr[2]+"\\Appshare\\");
							fcu.ChangeDir("\\Minuts\\"+(string)arr[2]+"\\Appshare\\");
							msg_Minuts_Server.Module="Appshare";	
									
						}

						else if(((string)arr[4]).Equals("Webshare"))
						{


							fcu.MakeDir("\\Minuts\\"+(string)arr[2]+"\\Webshare\\");
							fcu.ChangeDir("\\Minuts\\"+(string)arr[2]+"\\Webshare\\");
							msg_Minuts_Server.Module="Webshare";	
									
						}


						fcu.Upload((string)arr[5]);
						
						#endregion

						# region "Sending message to Server"
						msg_Minuts_Server.Available=1;
						msg_Minuts_Server.Current_DateTime=(string)arr[0];
						msg_Minuts_Server.Host=(string)arr[1];
						msg_Minuts_Server.meetingID=int.Parse((string)arr[2]);
						msg_Minuts_Server.Presenter=(string)arr[6];
						msg_Minuts_Server.ImagePath=(string)arr[7];
					
						if(network.IConferenceRoom.Add_MM_toServerDB(msg_Minuts_Server))
						{}
					
						#endregion 
				

						# region "Deletion from Db module"
						this.Delete_FileInfo_Upload(int.Parse((string)arr[2]),(string)arr[0],(string)arr[7]);
						#endregion 
				

					}

				}
			
				return true;










			}
			catch(Exception exp)
			{
				MessageBox.Show(exp.Message.ToString()+exp.StackTrace.ToString());
				return false;
			}
			finally
			{
			
				fcu.Close();
			}
			
		
		
		}
		public bool Delete_FileInfo_Upload(int meetingID,string DateTime, string relativepath)
		{
		;
		db.Delete("Delete from Client_MM where meetingid="+meetingID+" and datetime='"+DateTime+"'  and relativeurl='"+relativepath+"'" );
		return true;
		
		}



////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//**********************************************************************************************************//
//**********************************************************************************************************//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////




		/// <summary>
		/// Zaeem View 
		/// This method is useless and should not be here
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		#region listParticipents_SelectedIndexChanged( ==>Method
		private void listParticipents_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				if(listParticipents.SelectedItems.Count < 1)
					return;
				if(arrayParticipents.Count < 1)
					return;
                
				ClientProfile userProfile = (ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index];			
				//toolbarBtnChat.Enabled = !(userProfile.ClientId == network.profile.ClientId);
				
				
				if(userProfile.ClientId == network.profile.ClientId)  //own window
				{
				    
	
					if(network.profile.clientType == ClientType.ClientPresenter)
					{					
						btn_Audio.Enabled = true;
						btn_Record.Enabled =  true;
						
					}
					
				}
				else
				{			

					
	                if(network.profile.clientType == ClientType.ClientPresenter)
					{
						btn_Appshare.Enabled = network.profile.clientAccess.accessApplicationSharing;
						//	ChangeMenuItemState(menuApplication,network.profile.clientAccess.accessApplicationSharing);					
						
	
						btn_Audio.Enabled = network.profile.clientAccess.accessShareAudio;					
						btn_Record.Enabled = network.profile.clientAccess.accessShareScreen;
						
						
					}
					
					
				}			
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7127",exp," Participents selection change has encountered an exception. " + exp.Message.ToString(),true);
				//ShowExceptionMessage(" Participents selection change has encountered an exception. " + ee.Message);
			}
			
		}

		#endregion 

		#region WhiteBoard Coding
		private void ToolbarClicked(System.Windows.Forms.Button Button)
		{
			try
			{
				
				if(Button == btnLine)
				{
					whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.Line;
					statusBarString = tabPageInfo.Title + " - (Line)";
				}
				else if(Button == btnCircle)
				{
					whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.Ellipse	;
					statusBarString = tabPageInfo.Title + " - (Ellipse)";
				}	
				else if(Button == btnRectangle)
				{
					whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.Rectangle	;
					statusBarString = tabPageInfo.Title + " - (Rectangle)";
				}


				else if(Button == btnText)
				{	
					whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.Text;
					
					if(tabBody.SelectedTab.Title == "WhiteBoard")
					{
						//whiteBoard.ingoing=true;
						//whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.Text;
						statusBarString = tabPageInfo.Title + " - (Text)";
						
					}
					else
					{
						//whiteBoard.ingoing=true;
						if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
						{
							documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
							control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.Text;
						}
						else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
						{
							browser control = (browser)tabBody.SelectedTab.Control;
							control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.Text;
										
						}
					}

				}



				else if(Button == btnBrush)
				{
					whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.Brush;
					statusBarString = tabPageInfo.Title + " - (Brush)";
				}
				else if(Button == btnEraser)
				{
					whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.Eraser;
					statusBarString = tabPageInfo.Title + " - (Eraser)";
				}
				else if(Button == btnPencil)
				{
					whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.Pencil;
					statusBarString = tabPageInfo.Title + " - (Pencil)";
				}
				else if(Button == btnNone)
				{					
					whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.UniArrow;
					if(tabBody.SelectedTab.Title == "WhiteBoard")
					{
						
						statusBarString = tabPageInfo.Title ;						
					}
					else
					{
						if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
						{
							documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
							control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.UniArrow;
						}
						else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
						{
							browser control = (browser)tabBody.SelectedTab.Control;
							control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.UniArrow;										
						}
					}
					if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
					{
						ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
						if(!control.isDesktopSharing)			
							control.whiteBoard.tool = WhiteboardToolCode.UniArrow;
					}

					//control.whiteBoard.tool = WhiteboardToolCode.UniArrow;


				}
				else if(Button == btnArrow)
				{
					whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.None;
				
					if(tabBody.SelectedTab.Title == "WhiteBoard")
					{
						whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.None;
						whiteBoard.BringToFront();
						whiteBoard.Focus();						
						statusBarString = tabPageInfo.Title ;				
						
					}
					else
					{
						if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
						{
							documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
								control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.None;
							//	control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.UniArrow;
						}
						else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
						{
							browser control = (browser)tabBody.SelectedTab.Control;
							control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.None;
							//control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.UniArrow;
										
						}
					}
					if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
					{
						ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
						if(!control.isDesktopSharing)			
							control.whiteBoard.tool = WhiteboardToolCode.None;
					}

				}	
		

				else if(Button==btnFont)
				{
					if(tabBody.SelectedTab.Title == "WhiteBoard")
					{
						whiteBoard.SelectFont();
					}
					else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(WhiteboardControl)))
					{
						if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
						{
							ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
							if(!control.isDesktopSharing)			
								control.whiteBoard.SelectFont();
						}

						
					}
					else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
					{
						WebMeeting.ApplicationSharing appShareControl = (WebMeeting.ApplicationSharing )tabBody.SelectedTab.Control;
						//if(appShareControl.bIsServer)
					{
						appShareControl.whiteBoard.SelectFont();
					}

					}					
					else
					{
						if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
						{
							documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
							control.annotationBrowser.SelectFont();
						}
						else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
						{
							browser control = (browser)tabBody.SelectedTab.Control;
							control.annotationBrowser.SelectFont();
										
						}
					}				
					

					


				}
				else if(Button==btnColor)
				{				
					whiteBoard.SelectColor();
					
					if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
					{
						documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
						control.annotationBrowser.color = whiteBoard.color;
					}
					else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
					{
						browser control = (browser)tabBody.SelectedTab.Control;
						control.annotationBrowser.color = whiteBoard.color;
										
					}
					
					ControlMessage msg = new ControlMessage(ControlCode.AttendeeUpdate);			
					//Trace.WriteLine("line 6734 message of attendee update");
					
					network.profile.m_AssignedColor = whiteBoard.color;	

					if(NetworkManager.getInstance().profile.clientType == ClientType.ClientHost)
					{
						ClientUI.getInstance().listParticipents.Items[0].SubItems[4].BackColor=whiteBoard.color;	
						NetworkManager.getInstance().profile.m_AssignedColor=whiteBoard.color;
						
					}
			
					
					msg.Parameter = network.profile;		
					network.SendLoadPacket(msg);					
					network.myRecvMsgList.Add(msg);
				}
				else if(Button==btnPlaceHolderArrow)
				{					
					whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.PlaceHolderArrow;
					statusBarString = tabPageInfo.Title + " - (PlaceHolder Arrow)";

					if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
					{
						documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
						control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.PlaceHolderArrow;
					}
					else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
					{
						browser control = (browser)tabBody.SelectedTab.Control;
						control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.PlaceHolderArrow;
										
					}
					else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
					{
						ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
						if(!control.isDesktopSharing)			
							control.whiteBoard.tool = WhiteboardToolCode.PlaceHolderArrow;
					}

				}


				this._statusBar.Message = 	statusBarString;

			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7725",exp," Toolbar event has encountered an exception. " + exp.Message.ToString(),true);
				//Trace.WriteLine("Toolbar event has encountered an exception.");
				//ShowExceptionMessage(" Toolbar event has encountered an exception. " + ee.Message);
			}

		}
		
		#endregion

		#region nsButton6_Click(
		private void nsButton6_Click(object sender, System.EventArgs e)
		{			
			try
			{
				StartApplicationSharingEx();
			}
			catch(Exception exp)
			{
				ShowExceptionMessage("Module ::: AppShare private void nsButton6_Click(",exp,null,false);
			}


		}
		#endregion 

		# region btn_Record_Click Exception handled by Zaeem .
		private void btn_Record_Click(object sender, System.EventArgs e)
		{				
			/*bool vis=panelRecordingToolbar.Visible;
			if(vis)
				btn_Record.*/
			try
			{
				fm=new Screen_Capture.frm_Rec();
				fm.Show();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7646",exp,null,false);			
			}
			//fm.Visible=true;


			//OpenRecorder();	


		}
		# endregion 

		#region toolbarBtnExit_Click(
		private void toolbarBtnExit_Click(object sender, System.EventArgs e)
		{
			try
			{
				Close();
			}
			catch(Exception exp)
			{exp=exp;}
		}
		#endregion 


		#region drawing toolbar buttons 
	
		private void btnCircle_Click(object sender, System.EventArgs e)
		{
			try
			{
	
			{
			
				ToolbarClicked(btnCircle);
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
				{
					documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
					control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.Ellipse;
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
				{
					browser control = (browser)tabBody.SelectedTab.Control;
					control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.Ellipse;	
				
				}
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
				{
					ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
					if(!control.isDesktopSharing)			
						control.whiteBoard.tool = WhiteboardToolCode.Ellipse;
				}

			}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7828",exp,null,false);
				
			}
		}

		
		private void btnLine_Click(object sender, System.EventArgs e)
		{
			
			
			
			try
			{
			{	

				ToolbarClicked(btnLine);
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
				{
					documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
					control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.Line;
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
				{
					browser control = (browser)tabBody.SelectedTab.Control;
					control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.Line;	
				
				}
			
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
				{
					ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
					if(!control.isDesktopSharing)			
						control.whiteBoard.tool = WhiteboardToolCode.Line;
				}
			}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7878",exp,null,false);
				
			}
		}


		private void btnText_Click(object sender, System.EventArgs e)
		{
			
			try
			{
				ToolbarClicked(btnText);
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
				{
					ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
					if(!control.isDesktopSharing)			
						control.whiteBoard.tool = WhiteboardToolCode.Text;
				}
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7912",exp,null,false);
			}
		}


		private void btnRectangle_Click(object sender, System.EventArgs e)
		{
		
			try
			{
			{
			{
			
				ToolbarClicked(btnRectangle);
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
				{
					documentSharingControl control =(documentSharingControl) tabBody.SelectedTab.Control;
					control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.Rectangle;
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
				{
					browser control = (browser)tabBody.SelectedTab.Control;
					control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.Rectangle;	
				
				}
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
				{
					ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
					if(!control.isDesktopSharing)			
						control.whiteBoard.tool = WhiteboardToolCode.Rectangle;
				}

			}
			}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 7970",exp,null,false);
			
			}
		}


		private void btnBrush_Click(object sender, System.EventArgs e)
		{
			try
			{
			{
				ToolbarClicked(btnBrush);
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
				{
					documentSharingControl control =(documentSharingControl) tabBody.SelectedTab.Control;
					control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.Brush;
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
				{
					browser control = (browser)tabBody.SelectedTab.Control;
					control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.Brush;	
				
				}
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
				{
					ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
					if(!control.isDesktopSharing)			
						control.whiteBoard.tool = WhiteboardToolCode.Brush;
				}
			}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 8010",exp,null,false);
			}
		}




		private void btnPencil_Click(object sender, System.EventArgs e)
		{
			try
			{
				
			{
				ToolbarClicked(btnPencil);
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
				{
					documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
					control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.Pencil;
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
				{
					browser control = (browser)tabBody.SelectedTab.Control;
					control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.Pencil;	
				
				}
			
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
				{
					ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
					if(!control.isDesktopSharing)			
						control.whiteBoard.tool = WhiteboardToolCode.Pencil;
				}
			}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 8100",exp,null,false);
			}
		}


		private void btnNone_Click(object sender, System.EventArgs e)
		{
		
			try
			{
				ToolbarClicked(btnNone);
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
				{
					ApplicationSharing control = (ApplicationSharing)tabBody.SelectedTab.Control;
					if(!control.isDesktopSharing)			
						control.whiteBoard.tool = WhiteboardToolCode.UniArrow;
				}
				
			}
			catch(Exception exp)
			{
			
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 8133",exp,null,false);
			}

		}

		private void btnFont_Click(object sender, System.EventArgs e)
		{
			try
			{
				
				ToolbarClicked(btnFont);
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 8160",exp,null,false);
			}

		}

	
		private void btnPlaceHolderArrow_Click(object sender, System.EventArgs e)
		{
			try
			{
				ToolbarClicked(btnPlaceHolderArrow);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8200",exp,null,false);
			}
		}

		private void btnColor_Click(object sender, System.EventArgs e)
		{
			try
			{
				
				ToolbarClicked(btnColor);
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8225",exp,null,false);
			}
		
		}
		#endregion

		#region Polling Exceptions done
		
		Panel pollingPanel = new Panel();
		//public delegate void AddTabPageToTabControlDelegate(Crownwood.Magic.Controls.TabControl _control,Crownwood.Magic.Controls.TabPage _page);
		//AddTabPageToTabControlDelegate _AddTabPageToTabControl;
		
		/*
		public void AddTabPageToTabControlFn(Crownwood.Magic.Controls.TabControl _control,Crownwood.Magic.Controls.TabPage _page)
		{
			_control.TabPages.Add(_page);
		}
		*/	
	




		public void CreateNewPollingWindowFrmMangeContent(PollResultsMessage pm,WebPoll webPollObject,bool evaluation,string prmquestion)
		{
			try
			{
				if((pm == null) && (webPollObject == null))
				{
					WebMeeting.Polling.PollingQuestionDetails question = new WebMeeting.Polling.PollingQuestionDetails(true,prmquestion);
					if(evaluation)
						question.changeInterfacetoEvaluation();
					if(question.isAutomated==true) 
					{
						question.LunchPollMangesContent();
						//PollingTabPage pollingWindow = new PollingTabPage ();            

						if(this.frm_PollingGlobalPresenter!=null)
						{
							this.frm_PollingGlobalPresenter.Close();
							
							
							try
							{
								arrayPolling.RemoveAt(0);
							}
							catch(Exception exp)
							{
								exp=exp; 
							}

						}

						this.frm_PollingGlobalPresenter=new WebMeeting.Client.Polling.frm_PollPresenter();
						this.frm_PollingGlobalPresenter.PollingControl.pqd = question;
						this.frm_PollingGlobalPresenter.askQuestionWindow = question;
						//this.frm_PollingGlobalPresenter.askQuestionWindow.parentTabPage = pollingWindow;			
						this.frm_PollingGlobalPresenter.Controls.Add(this.frm_PollingGlobalPresenter.pollResult);
						this.frm_PollingGlobalPresenter.PollingControl.thisPollType = question.GetTypeEx();					
						if((this.frm_PollingGlobalPresenter.PollingControl.thisPollType  == PollType.MultipleSelect) || 
							(this.frm_PollingGlobalPresenter.PollingControl.thisPollType == PollType.FreeResponse))
						{
							this.frm_PollingGlobalPresenter.PollingControl.pr.DisableCharting();

						}
						
						this.frm_PollingGlobalPresenter.PollingControl.ShowResults();										

						this.frm_PollingGlobalPresenter.PollingControl.GetQuestion();
						this.frm_PollingGlobalPresenter.pollResult.lblQuestion.Text = this.frm_PollingGlobalPresenter.PollingControl.question;
						this.frm_PollingGlobalPresenter.PollingControl.AddClients(arrayParticipents);
						
						arrayPolling.Add(this.frm_PollingGlobalPresenter);
						
						if(!evaluation)
						{
							this.frm_PollingGlobalPresenter.PollingControl.AddSaveMenu();

						}


					}

					this.frm_PollingGlobalPresenter.Show();
				}
				else if(webPollObject != null)
				{
					PollingTabPage pollingWindow = new PollingTabPage ();            
					pollingWindow.Tag = webPollObject;
					if(!evaluation)
						pollingWindow.Title = "Polling";					
					else
						pollingWindow.Title = "Evaluation";

					WebMeeting.Polling.PollingQuestionDetails question = new WebMeeting.Polling.PollingQuestionDetails();
					if(evaluation)
						question.changeInterfacetoEvaluation();

					if(!question.ShowWebPollQuestion(webPollObject))
						return;

					pollingWindow.PollingControl.pqd = question;
					question.IsEvaluation = evaluation;


					pollingWindow.Control = pollingWindow.pollResult; 					
					pollingWindow.PollingControl.choices = webPollObject.choices;					
					pollingWindow.PollingControl.question = webPollObject.question; 
					pollingWindow.PollingControl.AddClients(arrayParticipents);
					pollingWindow.PollingControl.thisPollType = webPollObject.pollType;
					if((webPollObject.pollType == PollType.MultipleSelect) || (webPollObject.pollType == PollType.FreeResponse))
						pollingWindow.PollingControl.pr.DisableCharting();
                   
					pollingWindow.PollingControl.ShowResults();									
					pollingWindow.PollingControl.SendWebPollMessage(webPollObject.pollType,evaluation);
					pollingWindow.pollResult.lblQuestion.Text = webPollObject.question;

					//Invoke(_AddTabPageToTabControl,new object[]{pollingTabControl,pollingWindow});
					//pollingTabControl.TabPages.Add(pollingWindow);
					//pollingTabControl.SelectedTab = pollingWindow;

					//Invoke(_AddTabPageToTabControl,new object[]{tabBody,tabPollingGlobal});
					//tabBody.TabPages.Add(tabPollingGlobal);
					//tabBody.SelectedTabEx = tabPollingGlobal;
					arrayPolling.Add(pollingWindow);
				}
				
				else
				{
					bool bFound =false; // find if we already have a sharing window 
					for(int i = 0 ; i < pollingTabControl.TabPages.Count ; i++)
					{
						Crownwood.Magic.Controls.TabPage tab  = (Crownwood.Magic.Controls.TabPage)pollingTabControl.TabPages[i];
						PollingTabPage pWnd = (PollingTabPage)tab;
						if(pWnd.PollingControl.sessionID == pm.sessionID)
						{
							pWnd.PollingControl.AnswersList = pm.answersList;
							pWnd.PollingControl.choices = pm.choices;
							pWnd.PollingControl.question = pm.strQuestion;
							pWnd.PollingControl.thisPollType = pm.type;
							if((pm.type == PollType.MultipleSelect ) || (pm.type == PollType.FreeResponse))
								pWnd.PollingControl.pr.DisableCharting();

							pWnd.PollingControl.ShowResults();	
							pWnd.PollingControl.anonymous = pm.anonymous;
						
							pWnd.PollingControl.pr.EnableShareMode();								
							bFound = true;
						}
						
					}
					if(!bFound)
					{
						Thread.Sleep(500);
						PollingTabPage pollingWindow = new PollingTabPage ();            
						pollingWindow.Title = "Shared Results By " + pm.senderProfile.Name;															
						pollingWindow.Control = pollingWindow.pollResult; 
						pollingWindow.PollingControl.AnswersList = pm.answersList;
						pollingWindow.PollingControl.choices = pm.choices;
						pollingWindow.PollingControl.sessionID = pm.sessionID;
						pollingWindow.PollingControl.anonymous = pm.anonymous;
						pollingWindow.pollResult.lblQuestion.Text = pm.strQuestion;
						pollingWindow.PollingControl.question = pm.strQuestion;
						pollingWindow.PollingControl.pr.EnableShareMode();
						pollingWindow.PollingControl.thisPollType = pm.type;
						if((pm.type == PollType.MultipleSelect ) || (pm.type == PollType.FreeResponse))
							pollingWindow.PollingControl.pr.DisableCharting();

						pollingWindow.PollingControl.ShowResults();		
						
						
						//Invoke(_AddTabPageToTabControl,new object[]{pollingTabControl,pollingWindow});
						//pollingTabControl.TabPages.Add(pollingWindow);
						pollingTabControl.SelectedTab = pollingWindow;

						//Invoke(_AddTabPageToTabControl,new object[]{tabBody,tabPollingGlobal});
						//tabBody.TabPages.Add(tabPollingGlobal);
						tabBody.SelectedTabEx = tabPollingGlobal;

						/*
						tabBody.TabPages.Add(pollingWindow);
						tabBody.SelectedTab = pollingWindow;
						*/
					}
					

				}

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8225",exp," Create New Polling Window has encountered an exception. " + exp.Message+ "  " + exp.StackTrace.ToString(),true);
				//ShowExceptionMessage(" Create New Polling Window has encountered an exception. " + ee.Message+ "  " + ee.StackTrace.ToString());
			}		
		}

		public void CreateNewPollingWindow(PollResultsMessage pm,WebPoll webPollObject,bool evaluation)
		{
			
			try
			{

				
				if((pm == null) && (webPollObject == null))
				{
					WebMeeting.Polling.PollingQuestionDetails question = new WebMeeting.Polling.PollingQuestionDetails();
					
					
					//                     Commented  by Zaeem
//					if(evaluation)
//						question.changeInterfacetoEvaluation();
					
					
					if(question.ShowDialog() == DialogResult.OK)
					{
//						PollingTabPage pollingWindow = new PollingTabPage();
//
//                     Commented  by Zaeem
//						
//						pollingWindow.PollingControl.pqd = question;
//						pollingWindow.askQuestionWindow = question;
//						pollingWindow.askQuestionWindow.parentTabPage = pollingWindow;			
//						pollingWindow.Control = pollingWindow.pollResult;

						// Changed to form by Zaeem
						if(this.frm_PollingGlobalPresenter!=null)
						{
						this.frm_PollingGlobalPresenter.Close();
							
							
							try
							{
								arrayPolling.RemoveAt(0);
							}
							catch(Exception exp)
							 {
								exp=exp; 
							 }

						}

						this.frm_PollingGlobalPresenter=new WebMeeting.Client.Polling.frm_PollPresenter();
						this.frm_PollingGlobalPresenter.PollingControl.pqd=question;
						this.frm_PollingGlobalPresenter.askQuestionWindow = question;
						this.frm_PollingGlobalPresenter.Controls.Add(this.frm_PollingGlobalPresenter.pollResult);


//                     Commented  by Zaeem
//						pollingWindow.PollingControl.thisPollType = question.GetTypeEx();					

						// Changed to form by Zaeem
						this.frm_PollingGlobalPresenter.PollingControl.thisPollType = question.GetTypeEx();					
						


						//                     Commented  by Zaeem
//						if((pollingWindow.PollingControl.thisPollType  == PollType.MultipleSelect) || 
//							(pollingWindow.PollingControl.thisPollType == PollType.FreeResponse))
//						{
//							pollingWindow.PollingControl.pr.DisableCharting();
//
//						}

//                      Changed to form by Zaeem
						if((this.frm_PollingGlobalPresenter.PollingControl.thisPollType  == PollType.MultipleSelect) || 
							(this.frm_PollingGlobalPresenter.PollingControl.thisPollType == PollType.FreeResponse))
							{
							this.frm_PollingGlobalPresenter.PollingControl.pr.DisableCharting();

							}
						
//					                    Commented  by Zaeem
//						pollingWindow.PollingControl.ShowResults();										
						
						
//                      Changed to form by Zaeem
						this.frm_PollingGlobalPresenter.PollingControl.ShowResults();										
						
//					                    Commented  by Zaeem
						//tabBody.TabPages.Add(pollingWindow);
						//Invoke(_AddTabPageToTabControl,new object[]{pollingTabControl,pollingWindow});
						
//						pollingTabControl.SelectedTab = pollingWindow;
//
//						pollingWindow.PollingControl.GetQuestion();
//						pollingWindow.pollResult.lblQuestion.Text = pollingWindow.PollingControl.question;
//						pollingWindow.PollingControl.AddClients(arrayParticipents);
//						arrayPolling.Add(pollingWindow);

						
						
//                      Changed to form by Zaeem
						this.frm_PollingGlobalPresenter.PollingControl.GetQuestion();
						this.frm_PollingGlobalPresenter.pollResult.lblQuestion.Text = this.frm_PollingGlobalPresenter.PollingControl.question;
						this.frm_PollingGlobalPresenter.PollingControl.AddClients(arrayParticipents);
						arrayPolling.Add(this.frm_PollingGlobalPresenter);

						//WebMeeting.Polling.PollResult p=new WebMeeting.Polling.PollResult();
						//this.frm_PollingGlobalPresenter.Controls.Add(p);
						//this.frm_PollingGlobalPresenter.Show();

						if(!evaluation)
						{

//					                    Commented  by Zaeem
//							pollingWindow.Title = "Polling " + arrayPolling.Count;
//							pollingWindow.PollingControl.AddSaveMenu();

//							this.frm_PollingGlobalPresenter.Title = "Polling " + arrayPolling.Count;
							this.frm_PollingGlobalPresenter.PollingControl.AddSaveMenu();


						}
						else
						{
							//pollingWindow.Title = "Evaluation";
							//pollingWindow.askQuestionWindow.changeInterfacetoEvaluation();
							//pollingWindow.pollResult.changeInterfacetoEvaluation();

						}

//					                    Commented  by Zaeem
//
//						Invoke(_AddTabPageToTabControl,new object[]{tabBody,tabPollingGlobal});
//						
//						tabBody.SelectedTabEx = tabPollingGlobal;

						
						this.frm_PollingGlobalPresenter.Show();


					}
				}
				else if(webPollObject != null)
				{
					PollingTabPage pollingWindow = new PollingTabPage ();            
					pollingWindow.Tag = webPollObject;
					if(!evaluation)
						pollingWindow.Title = "Polling";					
					else
						pollingWindow.Title = "Evaluation";

					WebMeeting.Polling.PollingQuestionDetails question = new WebMeeting.Polling.PollingQuestionDetails();
					if(evaluation)
						question.changeInterfacetoEvaluation();

					if(!question.ShowWebPollQuestion(webPollObject))
						return;

					pollingWindow.PollingControl.pqd = question;
					question.IsEvaluation = evaluation;

					
					pollingWindow.Control = pollingWindow.pollResult; 					
					pollingWindow.PollingControl.choices = webPollObject.choices;					
					pollingWindow.PollingControl.question = webPollObject.question; 
					pollingWindow.PollingControl.AddClients(arrayParticipents);
					pollingWindow.PollingControl.thisPollType = webPollObject.pollType;
					if((webPollObject.pollType == PollType.MultipleSelect) || (webPollObject.pollType == PollType.FreeResponse))
						pollingWindow.PollingControl.pr.DisableCharting();
                   
					pollingWindow.PollingControl.ShowResults();									
					pollingWindow.PollingControl.SendWebPollMessage(webPollObject.pollType,evaluation);
					pollingWindow.pollResult.lblQuestion.Text = webPollObject.question;

					//Invoke(_AddTabPageToTabControl,new object[]{pollingTabControl,pollingWindow});
					//pollingTabControl.TabPages.Add(pollingWindow);
					pollingTabControl.SelectedTab = pollingWindow;

					//Invoke(_AddTabPageToTabControl,new object[]{tabBody,tabPollingGlobal});
					//tabBody.TabPages.Add(tabPollingGlobal);
					tabBody.SelectedTabEx = tabPollingGlobal;
					arrayPolling.Add(pollingWindow);
				}
				// Zaeem comment 
				// This else is used when you share the poll from Presenter side
				// and the poll is opened at the client side after receiving the 
				// PollResult Message .
				
				else
				{

					
					bool bFound =false; // find if we already have a sharing window 
					for(int i = 0 ; i < pollingTabControl.TabPages.Count ; i++)
					{
						Crownwood.Magic.Controls.TabPage tab  = (Crownwood.Magic.Controls.TabPage)pollingTabControl.TabPages[i];
						//frm_PollingGlobalAttendee=new WebMeeting.Client.Polling.frm_PollAttendee();

						PollingTabPage pWnd = (PollingTabPage)tab;
						if(pWnd.PollingControl.sessionID == pm.sessionID)
						{
							pWnd.PollingControl.AnswersList = pm.answersList;
							pWnd.PollingControl.choices = pm.choices;
							pWnd.PollingControl.question = pm.strQuestion;
							pWnd.PollingControl.thisPollType = pm.type;
							if((pm.type == PollType.MultipleSelect ) || (pm.type == PollType.FreeResponse))
								pWnd.PollingControl.pr.DisableCharting();

							pWnd.PollingControl.ShowResults();	
							pWnd.PollingControl.anonymous = pm.anonymous;
						
							pWnd.PollingControl.pr.EnableShareMode();								
							bFound = true;
						}
						
					}
					if(!bFound)
					{
						Thread.Sleep(500);
						//PollingTabPage pollingWindow = new PollingTabPage (); 
           

						if(this.frm_PollingGlobalAttendee!=null)
						{
						this.frm_PollingGlobalAttendee.Close();
						}

						this.frm_PollingGlobalAttendee=new WebMeeting.Client.Polling.frm_PollAttendee();


						// Commented By zaeem 


						/*
						pollingWindow.Title = "Shared Results By " + pm.senderProfile.Name;															
						pollingWindow.Control = pollingWindow.pollResult; 
						pollingWindow.PollingControl.AnswersList = pm.answersList;
						pollingWindow.PollingControl.choices = pm.choices;
						pollingWindow.PollingControl.sessionID = pm.sessionID;
						pollingWindow.PollingControl.anonymous = pm.anonymous;
						pollingWindow.pollResult.lblQuestion.Text = pm.strQuestion;
						pollingWindow.PollingControl.question = pm.strQuestion;
						pollingWindow.PollingControl.pr.EnableShareMode();
						pollingWindow.PollingControl.thisPollType = pm.type;

						*/


						this.frm_PollingGlobalAttendee.Controls.Add(this.frm_PollingGlobalAttendee.pollResult); 
						this.frm_PollingGlobalAttendee.PollingControl.AnswersList = pm.answersList;
						this.frm_PollingGlobalAttendee.PollingControl.choices = pm.choices;
						this.frm_PollingGlobalAttendee.PollingControl.sessionID = pm.sessionID;
						this.frm_PollingGlobalAttendee.PollingControl.anonymous = pm.anonymous;
						this.frm_PollingGlobalAttendee.pollResult.lblQuestion.Text = pm.strQuestion;
						this.frm_PollingGlobalAttendee.PollingControl.question = pm.strQuestion;
						this.frm_PollingGlobalAttendee.PollingControl.pr.EnableShareMode();
						this.frm_PollingGlobalAttendee.PollingControl.thisPollType = pm.type;




						if((pm.type == PollType.MultipleSelect ) || (pm.type == PollType.FreeResponse))
							this.frm_PollingGlobalAttendee.PollingControl.pr.DisableCharting();

						this.frm_PollingGlobalAttendee.PollingControl.ShowResults();		
						
						this.frm_PollingGlobalAttendee.Show();
						
//						Invoke(_AddTabPageToTabControl,new object[]{pollingTabControl,pollingWindow});
//						pollingTabControl.SelectedTab = pollingWindow;
//						Invoke(_AddTabPageToTabControl,new object[]{tabBody,tabPollingGlobal});
						//tabBody.TabPages.Add(tabPollingGlobal);
//						tabBody.SelectedTabEx = tabPollingGlobal;

						/*
						tabBody.TabPages.Add(pollingWindow);
						tabBody.SelectedTab = pollingWindow;
						*/
					}
					

				}

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8573",exp," Create New Polling Window has encountered an exception. " + exp.Message.ToString() ,true);
				//ShowExceptionMessage(" Create New Polling Window has encountered an exception. " + ee.Message);
			}
		
		}
		//public void CreateNewPollingWindow
		public void ClosePoolResultWindow(PollResultsMessage pm)
		{
			try
			{//main try
				if(pollingTabControl!=null)
					if(pollingTabControl.TabPages.Count>=1)
					{									
						for(int i = 0 ; i < pollingTabControl.TabPages.Count ; i++)
						{
							Crownwood.Magic.Controls.TabPage tab  = (Crownwood.Magic.Controls.TabPage)pollingTabControl.TabPages[i];										
							PollingTabPage pWnd = (PollingTabPage)tab;//pollingTabControl.TabPages[i].Control;
							//PollingTabPage pollingWindow=												
							if(pWnd.PollingControl.sessionID == pm.sessionID)
							{						
								//WM_DELETE_POLL_RESULT         send message to control
								Win32.USER32.SendMessage(pWnd.pollResult.Handle,Win32.USER32.WM_DELETE_POLL_RESULT,new IntPtr(),new IntPtr());							
							}
						}
					}		
			}//end try
			catch(Exception exp)
			{//start catch
				
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8600",exp,null,false);
			}//end catch

		
		}

		public void DisplayPoolFromWebResultWindow(PollingTabPage pollingWindow)
		{
			try
			{
				PollingTabPage tabPollWindow;
				for(int i = 0 ; i < arrayPolling.Count ; i ++)
				{
					tabPollWindow = (PollingTabPage)arrayPolling[i];				
					if(tabPollWindow == pollingWindow)
					{	
						Panel myPanel = (Panel)pollingWindow.Control;
						pollingWindow.PollingControl.GetQuestion();
						//pollingWindow.PollingControl.AddClients(new ArrayList());
						myPanel.Controls[0].Visible = false;
						pollingWindow.PollingControl.ShowResults();                    
						tabPollWindow.Title="Polling " + i.ToString();
						myPanel.Controls[1].Visible = true;					
						break;
					}				
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8650",exp,null,false);
			}
		}
		public void DisplayResultWindow(PollingTabPage pollingWindow)
		{
			
			try
			{
			
				PollingTabPage tabPollWindow;
				for(int i = 0 ; i < arrayPolling.Count ; i ++)
				{
					tabPollWindow = (PollingTabPage)arrayPolling[i];				
					if(tabPollWindow == pollingWindow)
					{	
						Panel myPanel = (Panel)pollingWindow.Control;

						//this.frm_PollingGlobalPresenter.Controls.Add(pollingWindow.Control);
						//this.frm_PollingGlobalPresenter.Show();

						pollingWindow.PollingControl.GetQuestion();
						//pollingWindow.PollingControl.AddClients(new ArrayList());
						myPanel.Controls[0].Visible = false;
						pollingWindow.PollingControl.ShowResults();                    
						tabPollWindow.Title="Polling " + i.ToString();
						myPanel.Controls[1].Visible = true;					
						break;
					}				
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8659",exp,null,false);
			}

		}
		#endregion

		#region XML Dataset Handling Exception handling done
		private bool CreateandDataSet()
		{
			try
			{//main try			
				this.dsStores=new docsSharing();			
				if (File.Exists(Application.StartupPath	+ "\\DocSharedList.xml"))
				{
					this.dsStores.ReadXml(Application.StartupPath	+ "\\DocSharedList.xml");				
					return true;
				}
			}//End try

			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8680",exp,null,false);
				return false;
			}//End Catch
			return false;
			
		}
		private bool LoadDataset() // load Dataset
		{						
			try
			{//main try			
				this.dsStores.Clear();
				if (File.Exists(Application.StartupPath	+ "\\DocSharedList.xml"))
				{
					this.dsStores.ReadXml(Application.StartupPath	+ "\\DocSharedList.xml");				
					return true;
				}
			}//End try
			catch(Exception exp)
			{
				
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8700",exp,null,false);

				return false;
			}//End Catch
			return false;
		}


		public  bool InsertNewRecord(string strFileName,string strRemoteUrl,string strLocalPath,int totalFiles, string strDocType,int session,string LastAccessDate)
		{
			try
			{//main try			
				DataTable dt = this.dsStores.Tables["DocsShared"];
				//				DataView dv = new DataView(dt);
				//													
				// insert it
				DataRow dr=dt.NewRow();
				dr["ID"]=dt.Rows.Count;
				dr["RemoteURL"]=strRemoteUrl;
				dr["LocalPath"]=strLocalPath;
				dr["TotalPages"]=totalFiles;
				dr["DocType"]=strDocType;					
				dr["FileName"]=strFileName;
				dr["Session"]=session;
				dr["LastAccessDate"]=LastAccessDate;
				dt.Rows.Add(dr);
					
				this.dsStores.WriteXml(Application.StartupPath	+ "\\DocSharedList.xml");
				return true;

			}//End try
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8750",exp,null,false);
				return false;
			}//End Catch
			//return false;
		}

		public DataRow getRecord(int id)
		{
			try
			{
				object primarykey =id ;			
				DataTable dt = this.dsStores.Tables["DocsShared"];
				return  dt.Rows.Find(new object[]{primarykey});			
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8750",exp,exp.Message.ToString(),true);
				return null;
			}
		}
		public  string findRecord(string strname,string dateoffile)
		{
			try
			{
			
				DataTable dt = this.dsStores.Tables["DocsShared"];
				DataView dv = new DataView(dt);
				dv.RowFilter = "FileName ='" + strname +"' AND LastAccessDate='" + dateoffile + "'";
				if (dv.Count > 0)
				{
					DataRowView drv=dv[0];
					return drv["ID"].ToString();

					//				object primarykey = drv["ID"];			
					//				// update the dataset
					//				DataRow drFound = dt.Rows.Find(new object[]{primarykey});
					//				dr["RemoteURL"];
					//				dr["LocalPath"];
					//				dr["TotalPages"];
					//				dr["DocType"];					
					//				dr["FileName"];
					//
					//				drFound["ImageName"] = theFile;
					//				drFound["Caption"] = labels[CurrentPosition].Text;
				}		
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8780",exp,exp.Message.ToString(),true);
				//		MessageBox.Show(ex.ToString());
			}
			return "-1";
		}
		#endregion

		#region DocumentSharing NEw Exception done
		public void ShareMyDocument(string strfileName)
		{
			
			var_temp_docshare=0;
	
			for(int i=0;i<tabBody.TabPages.Count;i++)
			{
				//MessageBox.Show(tabBody.TabPages[i].Title.ToString());
				if(tabBody.TabPages[i].Title.ToString().Equals("Document Sharing"))
				{
					++var_temp_docshare;
				}

			}

			//if(var_temp_appshare<=this.var_no_appshare)
			if(var_temp_docshare<1)
			{
	
			
				try
				{
					strfileName = strfileName.Replace("\\r","");
					strfileName = strfileName.Replace("\\n","");
					OpenFileDialog opfn=new OpenFileDialog();
					opfn.Filter="PowerPoint Files|*.ppt;*.pptx|Document Files|*.doc;*.docx|Excel Files|*.xls;*.xlsx|PDF Files|*.pdf";
					string selectedFile = strfileName;
					if(strfileName == "")
					{
						DialogResult res=opfn.ShowDialog();
						if(res==DialogResult.Cancel)
							return;
						selectedFile = opfn.FileName;
					//	selectedFile=selectedFile.Replace(selectedFile,"asasas");
						if(this.dsStores== null)
						{
							if(!this.CreateandDataSet())
								MessageBox.Show("Dataset not created properly");			
						}
					}
					Crownwood.Magic.Controls.TabPage tabDocumentSharing = new Crownwood.Magic.Controls.TabPage("Document Sharing");			
					WebMeeting.Client.documentSharingControl documentSharingControl = new documentSharingControl(false);
					//////////////////////////////////////////////////////////////////////
					tabDocumentSharing.Control = documentSharingControl;
			
					try
					{
						tabBody.TabPages.Add(tabDocumentSharing);		
					}
					catch(Exception exp)// ee)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8826",exp,null,false);
					}
						
					tabBody.SelectedTabEx = tabDocumentSharing;
			
					arraydocumentSharing.Add(tabDocumentSharing);
					documentSharingControl.Top=10;
					documentSharingControl.Left =10;// System.Drawing.Point(16, 24);			
					//documentSharingControl.Size = new System.Drawing.Size(632, 528);
					documentSharingControl.parentTabPage =  tabDocumentSharing;
					string extension = Path.GetExtension(selectedFile);
					
					if(!splitter1.IsCollapsed )                
						splitter1.ToggleState();

					extension = extension.ToLower();
					if(( extension == ".xls")|| ( extension == ".pdf"))
					{
						documentSharingControl.axWebBrowser1.Visible = true;
					}		

					documentSharingControl.StartDocumentSharingInitiater(selectedFile);
				}
			
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8820",exp,null,false);
				}
			}
			else
			{
				this.ShowExceptionMessage("Sorry! Another document has already been shared by some user in this meeting");
			}

			//			arraydocumentSharing.Add(tabDocumentSharing);
		}

		public void ShareMyDocumentByContentManagement(string strfileName)
		{
	
			var_temp_docshare=0;
			/*
				public int var_no_appshare=1;
				public int var_no_docshare=1;
				public int var_temp_appshare=0;
				public int var_temp_docshare=0;
			*/

			for(int i=0;i<tabBody.TabPages.Count;i++)
			{
				//MessageBox.Show(tabBody.TabPages[i].Title.ToString());
				if(tabBody.TabPages[i].Title.ToString().Equals("Document Sharing"))
				{
					++var_temp_docshare;
				}

			}

			//if(var_temp_appshare<=this.var_no_appshare)
			if(var_temp_docshare<1)
			{
	
			
			
				try
				{
					/*Assumed Inputs*/
					string[] strSplit=strfileName.Split(',');

					int no_of_files=Convert.ToInt32(strSplit[1]);
					//string RemoteUrl="http://www.compassnav.com/members/slideshows/230New Microsoft Excel Worksheet/230New Microsoft Excel Worksheet_files/sheet";
					//string RemoteUrl="http://www.compassnav.com/members/slideshows/222Copy of Webmeeting Knowledge Base/222Copy of Webmeeting Knowledge Base.htm";
					string RemoteUrl=strSplit[0];//"http://www.compassnav.com/members/slideshows/231Hello4/slide";
					string extenionRemoteFile=Path.GetExtension(strSplit[2]);
			
			
					if(strfileName == "")
					{
						return;
					}
					strfileName = strfileName.Replace("\\r","");
					strfileName = strfileName.Replace("\\n","");
					string selectedFile = strfileName;

					/*OpenFileDialog opfn=new OpenFileDialog();
						opfn.Filter="PowerPoint Files|*.ppt|Document Files|*.doc|Excel Files|*.xls|PDF Files|*.pdf";
						string selectedFile = strfileName;
						if(strfileName == "")
						{
							DialogResult res=opfn.ShowDialog();
							if(res==DialogResult.Cancel)
								return;
							selectedFile = opfn.FileName;
						}*/

					Crownwood.Magic.Controls.TabPage tabDocumentSharing = new Crownwood.Magic.Controls.TabPage("Document Sharing");			
					WebMeeting.Client.documentSharingControl documentSharingControl = new documentSharingControl(false);
					//////////////////////////////////////////////////////////////////////
					tabDocumentSharing.Control = documentSharingControl;
			
					try
					{
						tabBody.TabPages.Add(tabDocumentSharing);		
					}
					catch(Exception exp)// ee)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8900",exp,null,false);
					}
						
					tabBody.SelectedTabEx = tabDocumentSharing;
					documentSharingControl.IsLaunchFromMangeContent=true;
					documentSharingControl.no_of_pages=no_of_files;
					documentSharingControl.strRemoteUrl=RemoteUrl;
					arraydocumentSharing.Add(tabDocumentSharing);
					documentSharingControl.Top=10;
					documentSharingControl.Left =10;// System.Drawing.Point(16, 24);			
					//documentSharingControl.Size = new System.Drawing.Size(632, 528);
					documentSharingControl.parentTabPage =  tabDocumentSharing;
					documentSharingControl.strExtension=extenionRemoteFile;
					string extension =extenionRemoteFile; //Path.GetExtension(selectedFile);
					
					if(!splitter1.IsCollapsed )                
						splitter1.ToggleState();

					extension = extension.ToLower();
					if(( extension == ".xls")|| ( extension == ".pdf"))
					{
						documentSharingControl.axWebBrowser1.Visible = true;
					}
					else if(extension == ".ppt")
					{
				
						this.DisableSlideNav(true);
					}

					//documentSharingControl.StartDocumentSharingInitiater(selectedFile);
					documentSharingControl.StartDocumentSharingInitiaterManageContent("jjj","fsfds");
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8931",exp,null,false);
			
				}
			}
			else
			{
				this.ShowExceptionMessage("Sorry! Another document has already been shared by some user in this meeting");
			}
			//			arraydocumentSharing.Add(tabDocumentSharing);
		}

		#endregion

		#region tabbody and tabcontrol1 selection and resize events Exceptions done

		private void tabControl1_SelectionChanged(object sender, System.EventArgs e)
		{
			if(tabControl1.TabPages.Count > 2)
				this.tabControl1.ShowArrows=true;			
			else 
				this.tabControl1.ShowArrows=false;	

			if(tabControl1.SelectedTab ==  tabPage1 || tabControl1.SelectedTab == tabPageFind )
				this.tabControl1.ShowClose=false;			
			else 
				this.tabControl1.ShowClose=true;	
		}
		/// <summary>
		/// used to resize to tabBody
		/// </summary>
		public void tabBodyResize()
		{
			tabBody_Resize(null, null);
		}

		public bool bIsSeeNextTimeMessageAlert = true;
		public bool isChatAlreadyShown=false;		
		System.Threading.Thread thrShowChatBoxAlert;
		public void thrShowChatBoxAlert_method()
		{
			isChatAlreadyShown=true;
			chatMessageAlert frmChatMessageAlert = new chatMessageAlert();
			frmChatMessageAlert.Location = new Point( tabBody.Width - 224 , tabBody.Height );
			frmChatMessageAlert.ShowDialog(); 
		}
		private object lock_onAlert=new object();
		public void  showMessageAlert()
		{
			if( bIsSeeNextTimeMessageAlert )
			{				
				lock(lock_onAlert)
				{
					if(!isChatAlreadyShown)
					{
						Trace.WriteLine("showMessageAlert() thread name "+System.Threading.Thread.CurrentThread.Name );						
						isChatAlreadyShown=true;
						thrShowChatBoxAlert=new Thread(new ThreadStart(thrShowChatBoxAlert_method));
						thrShowChatBoxAlert.Name="MessagBoxAlert";
						thrShowChatBoxAlert.Start();					
					}
				}				
			}
		}


		public bool splitter1Closed = false;
		
		
		private void tabBody_Resize(object sender, System.EventArgs e)
		{
			try
			{
				if(this.WindowState == FormWindowState.Minimized)
					return;
				if(whiteBoard != null)
				{
					if(whiteBoard.mainImage != null)
						if((whiteBoard.mainImage.Width != tabBody.Width) || (whiteBoard.mainImage.Height != tabBody.Height))
						{
							if( this.splitter1.IsCollapsed )
								splitter1Closed = true;
							else
								splitter1Closed = false;
							whiteBoard.resize(tabBody.Width,tabBody.Height);						
							whiteBoard.ResizeWhiteboard(tabBody.Width,tabBody.Height);
							this.whiteBoard.Refresh();						
						}
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9000",exp,null,false);
			}
		}

	
		private void tabControl1_SizeChanged(object sender, System.EventArgs e)
		{
				
		}

		#endregion		

		#region ListView SubItem HitTest
	
		private bool HitTest(Point hitPoint, out int row, out int column)
		{
			// clear the output values
			row = column = -1;

			
			// set up the return value
			bool hitLocationFound = false;

			// initialise a hittest information structure
			Win32.USER32.LVHITTESTINFO lvHitTestInfo = new Win32.USER32.LVHITTESTINFO();
			lvHitTestInfo.pt.x = hitPoint.X;
			lvHitTestInfo.pt.y = hitPoint.Y;

			// send the hittest message to find out where the click was
			if(Win32.USER32.SendMessage(listParticipents.Handle, Win32.USER32.LVM_SUBITEMHITTEST, 0, ref lvHitTestInfo) != -1)
			{
				bool nowhere = ((lvHitTestInfo.flags & Win32.USER32.LVHT_NOWHERE) != 0);
				bool onItem = ((lvHitTestInfo.flags & Win32.USER32.LVHT_ONITEM) != 0);

				if(onItem && !nowhere)
				{
					row = lvHitTestInfo.iItem;
					column = lvHitTestInfo.iSubItem;
					hitLocationFound = true;
				}
			}

			return hitLocationFound;
		} 

		private bool HitTestForMouseHover(Point hitPoint, out int row, out int column)
		{
			bool retval = false;
			Win32.USER32.RECT subItemRect;
			row = column = -1;
			ListViewItem item = listParticipents.GetItemAt(hitPoint.X, hitPoint.Y);

			if(item != null && listParticipents.Columns.Count > 1)
			{
				if(listParticipents.AllowColumnReorder)
				{
					int[] columnOrder = new int[listParticipents.Columns.Count];
					// Get the order of columns in case they've changed from the user.
					if(Win32.USER32.SendMessage(listParticipents.Handle, Win32.USER32.LVM_COLUMNORDERARRAY, listParticipents.Columns.Count, columnOrder) != 0)
					{
						int i;
						// Get the subitem rectangles (except column 0), but get them in the proper order.
						Win32.USER32.RECT[] subItemRects = new Win32.USER32.RECT[listParticipents.Columns.Count];
						for(i = 1; i < listParticipents.Columns.Count; i++)
						{
							subItemRects[columnOrder[i]].top = i;
							subItemRects[columnOrder[i]].left = Win32.USER32.LVIR_BOUNDS;
							Win32.USER32.SendMessage(listParticipents.Handle, Win32.USER32.LVM_GETSUBITEMRECT, item.Index, ref subItemRects[columnOrder[i]]);							
						}
						
						// Find where column 0 is.
						for(i = 0; i < columnOrder.Length; i++)
							if(columnOrder[i] == 0)
								break;
						
						// Fix column 0 since we can't get the rectangle bounds of it using above.
						if(i > 0)
						{
							// If column 0 not at index 0, set using the previous.
							subItemRects[i].left = subItemRects[i-1].right;
							subItemRects[i].right = subItemRects[i].left + listParticipents.Columns[0].Width;
						}
						else
						{
							// Else, column 0 is at index 0, so use the next.
							subItemRects[0].left = subItemRects[1].left - listParticipents.Columns[0].Width;
							subItemRects[0].right = subItemRects[1].left;
						}

						// Go through the subitem rectangle bounds and see where our point is.
						for(int index = 0; index < subItemRects.Length; index++)
						{
							if (hitPoint.X >= subItemRects[index].left & hitPoint.X <= subItemRects[index].right)
							{
								row = item.Index;
								column = columnOrder[index];
								retval = true;
								break;
							}
						}
					}
				}
				else
				{
					for(int index = 1; index <= listParticipents.Columns.Count-1; index++)
					{
						subItemRect = new Win32.USER32.RECT();
						subItemRect.top = index;
						subItemRect.left = Win32.USER32.LVIR_BOUNDS;
						if(Win32.USER32.SendMessage(listParticipents.Handle, Win32.USER32.LVM_GETSUBITEMRECT, item.Index, ref subItemRect) != 0)
						{
							if (hitPoint.X < subItemRect.left)
							{
								row = item.Index;
								column = 0;
								retval = true;
								break;
							}
							if (hitPoint.X >= subItemRect.left & hitPoint.X <= subItemRect.right)
							{
								row = item.Index;
								column = index;
								retval = true;
								break;
							}
						}
					}
				}
			}
			return retval;
		}

		#endregion

		
		#region ShowPollMessages(PollResultsMessage pm)
		public void ShowPollMessages(PollResultsMessage pm)
		{
			tempPollMessage = pm;
			Win32.USER32.SendMessage(this.Handle,Win32.USER32.WM_SHOW_POLL_RESULTS,new IntPtr(),new IntPtr());
			
		}
		#endregion 

	
		/// <summary>
		/// Used in partial rights managment
		/// But currently not used
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		#region listParticipents_DoubleClick
		private void listParticipents_DoubleClick(object sender, System.EventArgs e)
		{			
			try
			{
				if(network.profile.clientType == ClientType.ClientHost)
				{
					if(listParticipents.SelectedItems.Count <1)
						return;
					if(((ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index]).ClientId == -1)
						return;
					ChangeAccessCodes(listParticipents.SelectedItems[0].Index);		
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9193",exp,null,false);
			}

		}
		
		#endregion 

		#region Manage Content 
		public void ExecuteWebBookMark(int nIndex)
		{
			try
			{
				WebBookMark mark = (WebBookMark)WebBookMarksArray[nIndex];
				bool bFound = false;
				for(int i = 0 ; i < arrayWebControl.Count ; i++)
				{					
					Crownwood.Magic.Controls.TabPage tabBrowser = (Crownwood.Magic.Controls.TabPage)arrayWebControl[i];					
					if(tabBrowser != null)
					{
						browser browserControl = (browser)tabBrowser.Control;
						if((int)browserControl.Tag == (nIndex+1))
						{
							tabBody.SelectedTabEx = tabBrowser;
							bFound =true;
						}
					}
				}

				if(!bFound)
				{					
					Crownwood.Magic.Controls.TabPage tabBrowser = new Crownwood.Magic.Controls.TabPage("Web Sharing");
					try
					{
						if(browserControl!=null)
						{
							browserControl.CleanandClear();
							//browserControl.Dispose();
							browserControl=null;
						
						}
					}
					catch(Exception exp)
					{
						MessageBox.Show(exp.StackTrace.ToString()+exp.Message.ToString());
					}
					
					
					browserControl = new browser();
					tabBrowser.Control = browserControl;
					tabBody.TabPages.Add(tabBrowser);
					tabBody.SelectedTabEx = tabBrowser;
					int sessionId = 0;
					network.GetUniqueSessionID(ref sessionId);// = ;
					browserControl.sessionID = sessionId;					
					browserControl.Left = 0;
					browserControl.Top = 0;			
					browserControl.Tag = nIndex+1;			
					browserControl.IsClosed = false;
					browserControl.PresenterInterface();
					browserControl.navigateTo(mark.Url);
					//browserControl.navigateTo("www.uraan.net");
					arrayWebControl.Add(tabBrowser);					

				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9243",exp,null,false);
			}
		}

		public void ExecuteWebPoll(int nIndex)
		{
			try
			{
				WebPoll poll =(WebPoll) webPollsArray[nIndex];
				bool bFound = false;
				for(int i = 0 ; i < arrayPolling.Count ; i++)
				{					
					object obj = ((PollingTabPage)arrayPolling[i]).Tag;
					if(obj != null)
					{
						if(obj.GetType().Equals(typeof(WebPoll)))
						{
							WebPoll pollFound = (WebPoll)obj;
							if(poll == pollFound)
							{
								tabBody.SelectedTab = ((PollingTabPage)arrayPolling[i]);								
								return;
							}

						}
					}
				}
				if(!bFound)
					CreateNewPollingWindow(null,poll,false);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9275",exp,"Error in executing web poll. " + exp.Message.ToString(),true);
				//	ShowExceptionMessage("Error in executing web poll. " + ee.Message);

			}
		}



		public void ExecuteEvaluation(int nIndex)
		{
			try
			{
				WebPoll poll =(WebPoll) webPollsArray[nIndex];
				bool bFound = false;
				for(int i = 0 ; i < arrayPolling.Count ; i++)
				{					
					object obj = ((PollingTabPage)arrayPolling[i]).Tag;
					if(obj != null)
					{
						if(obj.GetType().Equals(typeof(WebPoll)))
						{
							WebPoll pollFound = (WebPoll)obj;
							if(poll == pollFound)
							{
								tabBody.SelectedTab = ((PollingTabPage)arrayPolling[i]);								
								return;
							}

						}
					}
				}
				if(!bFound)
					CreateNewPollingWindow(null,poll,true);				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9310",exp,"Error in executing web poll. " + exp.Message.ToString(),true);
				//ShowExceptionMessage("Error in executing web poll. " + ee.Message);

			}



		}
		
		public void ExecuteWebPresentation(int nIndex)
		{
			try
			{
				
				string strURL =(string)((WebPresentationEntry) WebPresentationsArray[nIndex]).path;				
				if(strURL.Length>0)
				{
					string localpath = Application.StartupPath + "\\Presentations\\abc.ppt";
					int nIndex2 = strURL.LastIndexOf("/");
					if(nIndex2 != -1)
						localpath = strURL.Substring(nIndex2+1,strURL.Length - nIndex2-1);						
					localpath = Application.StartupPath + @"\Presentations\" + localpath;					

					WebFIleInformation frm = new WebFIleInformation();
					strURL = strURL.ToLower();
					strURL = strURL.Replace("/uploads/","/SlideShows/");
					frm.fileInformation = new WebUploadedFiles(strURL,strURL);					
					frm.label1.Text = "Execute Document";
					frm.label2.Visible = false;
					frm.IsWebPresentation = true;
					frm._downloadPath = localpath;
					frm.ShowDialog();
					if(frm.IsDownloadedSuccessfully)
					{
						ShareMyDocument(frm.localFilePath);						 
					}					

				}
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9352",exp,exp.Message.ToString() + " ExecuteWebPresentation",true);
				//ClientUI.getInstance().ShowExceptionMessage(ee.Message + " ExecuteWebPresentation");
				
			}		
			
		}
		

		public void ExecuteWebFile(int nIndex)
		{
			try
			{
				WebUploadedFiles file = (WebUploadedFiles) webFiles[nIndex];
				//string strURL =(string) WebPresentationsArray[nIndex];
				if(file.filePath.Length > 0)
				{
					string strURL = file.filePath;
					string localpath = "webfile";
					int nIndex2 = strURL.LastIndexOf("/");
					if(nIndex2 != -1)
						localpath = strURL.Substring(nIndex2+1,strURL.Length - nIndex2-1);						
					localpath = Application.StartupPath + @"\" + localpath;					
					
					WebMeeting.Client.WebFIleInformation fileDownloadDlg = new WebMeeting.Client.WebFIleInformation();
					fileDownloadDlg.fileInformation = new WebUploadedFiles(file.name,file.filePath);
					fileDownloadDlg.label1.Text = "";//Download and Launch " + file.name + " in Meeting? ";
					fileDownloadDlg.label2.Text = file.filePath;
					fileDownloadDlg.IsExecute = true;
					fileDownloadDlg.ShowDialog();
					
					fileDownloadDlg.Invalidate();
					if(fileDownloadDlg.IsDownloadedSuccessfully)
					{						
						WebFileMessage msg = new WebFileMessage();
						msg.SenderID = network.profile.ClientId;
						msg.senderprofile = network.profile;
						msg.FileInformation = new WebUploadedFiles(file.name,file.filePath);
						network.SendLoadPacket(msg);
						//network.myRecvMsgList.Add(msg);
					}		

				}

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9410",exp,exp.Message.ToString(),true);
			}
		}


		#endregion

		#region Different Control Click Event handlers
		private void btn_Docshare_Click(object sender, System.EventArgs e)
		{
			//CreateNewPollingWindow(null,null,false);
			ShareMyDocument("");
		}	

		private void toolbarBtnChat_Click(object sender, System.EventArgs e)
		{
			OpenChatWindow(false);		
		}

		private void btn_Whiteboard_Click(object sender, System.EventArgs e)
		{
			ShareWhiteboard();
			
		}

		public void ShareWhiteboard()
		{
		
			///////////////////////////////////////////////////////////////////
			///////////////////////////by junaid//////////////////////////////
			/////////////////////////////////////////////////////////////////			
			
			/*whiteBoard.InitializeToSize(tabBody.Width,tabBody.Height); //per2
			whiteBoard.resize(tabBody.Width,tabBody.Height);		//per2		
			*/		
		

			if(tabBody.TabPages.Contains(tabPageInfo))
			{				
				tabBody.SelectedTabEx = tabPageInfo;
			}
			else
			{
				tabBody.TabPages.Add(tabPageInfo);
				tabBody.SelectedTabEx = tabPageInfo;
			}
			whiteBoard.btn_whtLogg.Visible=true;
			WhiteboardMessage msg = new WhiteboardMessage();
			msg.MessageType=(ushort)MessageObjectType.MsgShowWhiteBoard;
			msg.ConferenceID = network.profile.ConferenceID;
			msg.SenderID = network.profile.ClientId;
			network.SendLoadPacket(msg);			
		

		}
		public void btnEraserVisible(bool flag)
		{
			try
			{
		
				this.btnEraser.Visible=flag;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>Problem in btnEraseVisiblefunction 10115",exp,null,false);
			}
		
		}

		/// <summary>
		/// This function should only be called by the Host
		/// </summary>
		/// <returns></returns>
 
		public bool ifHostisPresenter()
		{
			# region Normal Version 
			if(btn_Whiteboard.Enabled==false && btn_Docshare.Enabled==false)
				return false;
			# endregion 
				
			# region	Stripped down Version
//			if(btn_Whiteboard.Enabled==false )//&& btn_Docshare.Enabled==false)
//				return false;
			# endregion 
			else
				return true;
		}


		/*
				
		public void enableWhiteBoardTools(bool blFlag)
		{
			blFlag=true;
			try
			{
				this.toolbarPanel.Enabled=blFlag;
				this.btnPlaceHolderArrow.Enabled=blFlag;			
				this.btnArrow.Enabled=blFlag;
				this.btnColor.Enabled=blFlag;
				this.btnFont.Enabled=blFlag;
				this.btnNone.Enabled=blFlag;
				this.btnPencil.Enabled=blFlag;			
				this.btnBrush.Enabled=blFlag;
				this.btnRectangle.Enabled=blFlag;
				this.btnText.Enabled=blFlag;
				this.btnLine.Enabled=blFlag;
				this.btnCircle.Enabled=blFlag;
				this.button4.Enabled=blFlag;
				this.btnClearWhiteBoard.Enabled=blFlag;
				this.btnEraser.Enabled=blFlag;

				if(!blFlag)
				{
				
					
					whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.None;
					

					for(int i = 0 ; i < arraydocumentSharing.Count; i++)
					{
						WebMeeting.Client.documentSharingControl _control = (documentSharingControl)((Crownwood.Magic.Controls.TabPage)arraydocumentSharing[i]).Control;
						_control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.None;				
					}
					for(int i = 0 ; i < this.arrayWebControl.Count;i++)
					{
					
						//browser _control = (browser)arrayWebControl[i];
						Crownwood.Magic.Controls.TabPage tabBrowser = (Crownwood.Magic.Controls.TabPage)arrayWebControl[i];
						browser _control = (browser)tabBrowser.Control;
						_control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.None;				
					}
					for(int i = 0 ; i < ApplicationSharingTabPagesArray.Count ;i++)
					{
						Crownwood.Magic.Controls.TabPage tab = (Crownwood.Magic.Controls.TabPage)ApplicationSharingTabPagesArray[i];
						ApplicationSharing appControl = (ApplicationSharing)tab.Control;
						appControl.whiteBoard.tool=WhiteboardToolCode.None;
					}		

//						documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
//						control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.None;
//						
//						//control = (browser)tabBody.SelectedTab.Control;
//						control.annotationBrowser.tool = WebBrowserSubClassing.WhiteboardToolCode.None;										
//				
//						//control = (ApplicationSharing)tabBody.SelectedTab.Control;
//						//control.whiteBoard.tool = WhiteboardToolCode.None;
//					

		
				}




			
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>Problem in enabling and disabling of tools 10084",exp,null,false);
			}
			
			
		}

		*/


		public void enableMenus(bool blFlag)
		{
			
			# region Normal Version
			
			ChangeMenuItemState(menuOpenRecorder,blFlag);
			ChangeMenuItemState(mnuInvite,blFlag);
			ChangeMenuItemState(mnuApplication,blFlag);
			ChangeMenuItemState(mnuWebBrowser,blFlag);
			ChangeMenuItemState(mnupDocument,blFlag);
			ChangeMenuItemState(mnuWhiteboard,blFlag);
			ChangeMenuItemState(mnuPoll,blFlag);


			//Minuts of Meeting section 
			ChangeMenuItemState(mnu_MM_Wht,blFlag);
			ChangeMenuItemState(mnu_MM_App,blFlag);
			ChangeMenuItemState(mnu_MM_Web,blFlag);


			//ChangeMenuItemState(mnu_Minuts,blFlag);
			ChangeMenuItemState(mnuVideo,blFlag);
			ChangeMenuItemState(mnuAudio,blFlag);
			
			# endregion


			# region Stripped Down Version
			/*
			ChangeMenuItemState(menuOpenRecorder,false);
			ChangeMenuItemState(mnuInvite,false);
			ChangeMenuItemState(mnuApplication,false);
			ChangeMenuItemState(mnuVideo,false);
			ChangeMenuItemState(mnuAudio,false);
			ChangeMenuItemState(mnupDocument,false);
			
			ChangeMenuItemState(mnuWhiteboard,blFlag);
			ChangeMenuItemState(mnuWebBrowser,blFlag);
			ChangeMenuItemState(mnuPoll,blFlag);
			*/

			# endregion
		
		} 

		private void btn_Audio_Click(object sender, System.EventArgs e)
		{
			ShareMyAudio();
			//MessageBox.Show("This feature is not available","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Warning);

		}


		private void mChatWindowSaveMenu_Click(object sender, EventArgs e)
		{		
			try
			{
				NiceMenu menu = (NiceMenu)sender;
				for(int i = 0 ; i < chatWindowsArray.Count ; i++)
				{
					ClientProfileWrapper CP = (ClientProfileWrapper)chatWindowsArray[i];
					if(i+1 == menu.Index)
					{
						if(CP.tabPage.Title == menu.Text)
						{
							((chatControl)CP.tabPage.Control).Save();
						}
					

					}
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9535",exp,null,false);
	
			}
		}


		private void mChatWindowSaveAsMenu_Click(object sender, EventArgs e)
		{
			try
			{
				NiceMenu menu = (NiceMenu)sender;
				for(int i = 0 ; i < chatWindowsArray.Count ; i++)
				{
					ClientProfileWrapper CP = (ClientProfileWrapper)chatWindowsArray[i];
					if(i+1 == menu.Index)
					{
						if(CP.tabPage.Title == menu.Text)
						{
						
							((chatControl)CP.tabPage.Control).SaveAs();
						

						}
					

					}
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9565",exp,null,false);
			}
		}
		

		private void button5_Click(object sender, System.EventArgs e)
		{
			
			ToolbarClicked(btnArrow);

		}


		private void ToolbarLines_OneWidth(object sender, EventArgs e)
		{
			try
			{
				whiteBoard.LineThickness = 2;
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
				{
					documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
					control.annotationBrowser.lineThickness = 2;
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
				{
					browser control = (browser)tabBody.SelectedTab.Control;
					control.annotationBrowser.lineThickness = 2;										
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
				{				
					Crownwood.Magic.Controls.TabPage tab = tabBody.SelectedTab;
					ApplicationSharing appControl = (ApplicationSharing)tab.Control;
					appControl.whiteBoard.LineThickness = 2;
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9615",exp,null,false);
			}
		}


		private void ToolbarLines_TwoWidth(object sender, System.EventArgs e)
		{
			try
			{
				whiteBoard.LineThickness = 4;
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
				{
					documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
					control.annotationBrowser.lineThickness = 4;
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
				{
					browser control = (browser)tabBody.SelectedTab.Control;
				
					control.annotationBrowser.lineThickness = 4;										
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
				{				
					Crownwood.Magic.Controls.TabPage tab = tabBody.SelectedTab;
					ApplicationSharing appControl = (ApplicationSharing)tab.Control;
					appControl.whiteBoard.LineThickness = 4;
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9655",exp,null,false);
			}
		}
	
		
		private void ToolbarLines_ThreeWidth(object sender, System.EventArgs e)
		{
		
			try
			{
				whiteBoard.LineThickness = 6;
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
				{
					documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
					control.annotationBrowser.lineThickness = 6;
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
				{
					browser control = (browser)tabBody.SelectedTab.Control;
				
					control.annotationBrowser.lineThickness = 6;										
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
				{				
					Crownwood.Magic.Controls.TabPage tab = tabBody.SelectedTab;
					ApplicationSharing appControl = (ApplicationSharing)tab.Control;
					appControl.whiteBoard.LineThickness = 6;
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9675",exp,null,false);
			}
		
		
		}


		private void ToolbarLines_FourWidth(object sender, System.EventArgs e)
		{
			try
			{
				whiteBoard.LineThickness = 8;
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
				{
					documentSharingControl control = (documentSharingControl)tabBody.SelectedTab.Control;
					control.annotationBrowser.lineThickness = 8;
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(browser)))
				{
					browser control = (browser)tabBody.SelectedTab.Control;				
					control.annotationBrowser.lineThickness = 8;										
				}
				else if(tabBody.SelectedTab.Control.GetType().Equals(typeof(ApplicationSharing)))
				{				
					Crownwood.Magic.Controls.TabPage tab = tabBody.SelectedTab;
					ApplicationSharing appControl = (ApplicationSharing)tab.Control;
					appControl.whiteBoard.LineThickness = 8;
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9705",exp,null,false);
			}
		}

		
		private void btnQA_Click(object sender, System.EventArgs e)
		{
			
			ShareMyBrowser();
			//By zaeem as that was the old	QA block.
			/*
			try
			{
				if((network.profile.clientType == ClientType.ClientPresenter) || (network.profile.clientType == ClientType.ClientHost))
				{
					try
					{
						tabBody.TabPages.Add(TabPageQAPresenter);
						tabBody.SelectedTabEx = TabPageQAPresenter;
					}
					catch(Exception exp)// ee)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9725",exp,null,false);
					}
				}
				else
				{
					try
					{
						tabBody.TabPages.Add(TabPageQAAttendee);
						tabBody.SelectedTabEx = TabPageQAAttendee;
					}
					catch(Exception exp)// ee)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9735",exp,null,false);
					}
				}
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs Question Answer line==>9742",exp,null,false);
			}
			*/
			
		}



		private void RetrieveManageContent()
		{
			try
			{
				tabPageWebContent.Title = "Manage Content";
				
				//((ManageContentWebPresentations)(tabPageWebContent.Control)).axWebBrowser1.Stop();
				((ManageContentWebPresentations)(tabPageWebContent.Control)).strOpenedPage="";//stop opening doc/question from launching
				((ManageContentWebPresentations)(tabPageWebContent.Control)).ShowContentPage();//open the home page
				tabBody.TabPages.Add(tabPageWebContent);					
				tabBody.SelectedTabEx = tabPageWebContent;
				
				
				this.tabBody.ShowClose = true; // by kamran 
			
				//manageContentBookmarks.RetrieveContent();
//				manageContentBookmarks.listWebPolls.Items.Clear();
//				manageContentPresentations.listWebPresentations.Items.Clear();
//				//manageContentWebFiles.listWebFiles.Items.Clear();
//				manageContentWebPolls.listWebPolls.Items.Clear();
//				manageContentEvaluations.RetrieveContent();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9770",exp,null,false);
			}
		}


		private void btn_Managecontent_Click(object sender, System.EventArgs e)
		{
			try
			{
				
				Thread th = new Thread(new ThreadStart(RetrieveManageContent));
				th.Name = "WebContent Click Thread: RetrieveManageContent()";
				th.Start();
				
			
				/*
				manageContentPresentations.RetrieveContent();
				manageContentWebFiles.RetrieveContent();
				manageContentWebPolls.RetrieveContent();	
				*/		
			}
			catch(Exception exp)// ee)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9800",exp,null,false);

			}

		}
		



		/// <summary>
		/// This method returns Host registration Id as an integer 
		/// </summary>
		/// <returns></returns>
		public int GetHostId_Registration()
		{

			# region for Loop
			int id=0;
			ClientProfile profile1=new ClientProfile();

			for(int i=0;i<Client.ClientUI.getInstance().arrayParticipents.Count;i++)
			{
			
				profile1=(ClientProfile)arrayParticipents[i];
				if(profile1.clientType.ToString().Equals("ClientHost"))
				{
					id=profile1.ClientRegistrationId;
				}
								
			}

			return id;
			# endregion 
		
		}









		/// <summary>
		/// // this Methods only called from here 
		/// When the presenter leaves the meeting 
		/// All tabs are closed and drawing tools are Syncronyzed
		/// flag_checkbox=false;
		/// Assigning false to the above flag means 
		/// that now there is no presenter in the meeting
		/// and you are free to give any one the rights
		/// </summary>
		/// <param name="comin_clientProf"></param>

		public bool NoofLogicalPresenters(ClientProfile comin_clientProf)
		{
			try
			{
				ClientProfile profile1=new ClientProfile();
				//******************************************************//
				int noof_logicalPresenter=0;
				//******************************************************//
				//  This loop count the no of Presenter Except Host and add in the above defined variables
				//******************************************************//
				# region for Loop
				for(int i=0;i<Client.ClientUI.getInstance().arrayParticipents.Count;i++)
				{
			
					profile1=(ClientProfile)arrayParticipents[i];
					if(profile1.clientType.ToString().Equals("ClientPresenter"))
					{
						++noof_logicalPresenter;
					}
				
				}
				# endregion 
				//******************************************************//
				

				// if there is any presenter in the Meeting (Except Host) 
				// and its ID is the same as that of the saved presenter, by host
				// It means that the presenter is leaving the meeting 
				
				
				if(noof_logicalPresenter>0 && saveID==comin_clientProf.ClientId)
				{
					//As now there is no Presenter in the meeting 
					//So you dont have to take the rights away from any person.
					// thats why  flag_checkbox=false; 

					flag_checkbox=false;
			
					bool bTrue = false;//(e.NewValue == CheckState.Checked);
					profile1.clientAccess.accessApplicationSharing =bTrue ;
					profile1.clientAccess.accessShareAudio =bTrue ;
					profile1.clientAccess.accessDesktopSharing = bTrue ;
					profile1.clientAccess.accessDrawWhiteBoard =bTrue;
					profile1.clientAccess.accessSharePresentation =bTrue ;
					profile1.clientAccess.accessShareScreen = bTrue ;
					profile1.clientAccess.accessShareVideo = bTrue ;
					profile1.clientAccess.accessSharePolling = bTrue;
					profile1.clientAccess.accessPrivateChat = bTrue;
					profile1.clientAccess.accessAllowRightsAssignments= bTrue;
					
					ControlMessage msg2 = new ControlMessage(ControlCode.AttendeeUpdate);			
					msg2.Parameter = profile1;	
					network.SendLoadPacket(msg2);
					network.myRecvMsgList.Add(msg2);
					// when some one Presenter leaves the meeting 
					// All of its tabs are closed 				
					HostCloseAllTabs();	
					// Drawing tools are syncronyzed
					this.SyncronizeDrawingTools();
					return true;

				}
				return false;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10493 NoofLogicalPresenters(",exp,null,false);
				return false;
			
			}
		}

		


		/// <summary>
		/// This method should run when the Host is dropped from the meeting
		/// The purpose of this method is that the meeting shd come to the initial
		/// state i.e Presenter becomnes Attendee , Drawing tools are Syncronyzed 
		/// and all opened instances are closed.
		/// It should run only for Host
		/// 
		/// </summary>
		/// <param name="comin_clientProf"></param>
		public bool WhoisDropped(ClientProfile comin_clientProf)
		{
			// Just initialise a variable name hostID to 0;
			int hostID=0;

			try
			{

				ClientProfile profile2=new ClientProfile();
				// This variable "noof_Hosts" will contain the no of Host 
				// present at a perticular time in the meeting.

				int noof_Hosts=0;
				//Pass through the all the participant in the meeting
				for(int i=0;i<Client.ClientUI.getInstance().arrayParticipents.Count;i++)
				{
					profile2=(ClientProfile)arrayParticipents[i];

					//if this perticular participant is the host
					// Improvment Areas
					// Dont make comparison on the basis of string 
					// 1 Its inefficient then object comparison 
					// 2 Chance of Mistake in spellings


					// if any host is found 
					// I am assuming that there will be one host in the meeting at 
					// a perticular time.

					if(profile2.clientType.ToString()=="ClientHost")
					{
						
						++noof_Hosts;
						// We save the hostID as we want to check as if the current host is dropped from the meeting
						hostID=profile2.ClientId;
					
					}
					
				}

				// If the host is dropped from the meeting
				// hostID						==> host in the meeting
				// comin_clientProf.ClientId	==> Drop message for the Host
				// If they matches it means that Host is dropped from the meeting.



				if(hostID==comin_clientProf.ClientId)
				{
					//MessageBox.Show("No of Hosts after host drop called=>"+noof_Hosts.ToString() );

					ClientProfile profile1=new ClientProfile();
					//******************************************************//
					int noof_logicalPresenter=0;
					//******************************************************//
					//  This loop count the no of Presenter and add in the above defined variables
					//It takes the rights from All the participants Except Host
					// as if Host drops from the meeting then Meeting shd come to the
					// Initialse stage
					//******************************************************//
					# region for Loop
					for(int i=0;i<Client.ClientUI.getInstance().arrayParticipents.Count;i++)
					{
			
						profile1=(ClientProfile)arrayParticipents[i];


						if(profile1.clientType.ToString()!="ClientHost")
						{
							//++noof_logicalPresenter;
							// All the rights are taken back 
							bool bTrue = false;//(e.NewValue == CheckState.Checked);
							profile1.clientAccess.accessApplicationSharing =bTrue ;
							profile1.clientAccess.accessShareAudio =bTrue ;
							profile1.clientAccess.accessDesktopSharing = bTrue ;
							profile1.clientAccess.accessDrawWhiteBoard =bTrue;
							profile1.clientAccess.accessSharePresentation =bTrue ;
							profile1.clientAccess.accessShareScreen = bTrue ;
							profile1.clientAccess.accessShareVideo = bTrue ;
							profile1.clientAccess.accessSharePolling = bTrue;
							profile1.clientAccess.accessPrivateChat = bTrue;
							profile1.clientAccess.accessAllowRightsAssignments= bTrue;
					
							ControlMessage msg2 = new ControlMessage(ControlCode.AttendeeUpdate);			
							profile1.clientType = ClientType.ClientAttendee;
							msg2.Parameter = profile1;	
							
							// Message is send by taking all the rights 
							network.SendLoadPacket(msg2);
							network.myRecvMsgList.Add(msg2);
							HostCloseAllTabs();
							
				
						}
						
					}
					# endregion 

					// Drawing tools should be Syncronyzed when host is dropped.
					SyncronizeDrawingTools();
					return true;	
				}
				return false;	
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10569 Whoisdroped()",exp,null,false);
				return false;
			}


		}

		/// <summary>
		/// Changes the state of the check boxes.
		/// zaeem view:
		/// This function is called whenever the state of the check boxes are changed.
		/// But the conditions in this functions are not right.
		///  
		/// </summary>
		/// <param name="nIndex"></param>
		/// <param name="clientType"></param>
		/// 

		// This function sis called from the following places in the programme.
		
		
		//NetworkManager.cs(394):	ClientUI.getInstance().ListParticipentCheckBoxChange(X,clientProf.clientType);
		//				ReInsertClientProfile(WebMeeting.Common.ClientProfile clientProf, int nIndex)



		//NetworkManager.cs(412):			ClientUI.getInstance().ListParticipentCheckBoxChange(X,clientProf.clientType);
		// public void UpdateClientProfile(int nSenderID,int i,ref ClientProfile client,ref ClientProfile clientProf)
	
		//
		//NetworkManager.cs(826):									ClientUI.getInstance().ListParticipentCheckBoxChange(X,clientProfile.clientType);
		//	From receive thread in ServerInfnoMessage	if(msg.GetType().Equals(typeof(ServerInfoMessage)))
			
		//NetworkManager.cs(1308):									ClientUI.getInstance().ListParticipentCheckBoxChange(X,clientProf.clientType);
		//On Attendee Join Method 	else if(controlMessage.Code == ControlCode.AttendeeJoin)
						

		public void ListParticipentCheckBoxChange(int nIndex,WebMeeting.Common.ClientType clientType)
		{
			// Zaeem View
			// this bool is refered in listparticipents_itemChecks()
			// But not clear to me that why its refered there
			CheckBoxStateChangeByNetwork = true;
			
			//Trace.WriteLine("List Participant Check Box Change");
			// Zaeen View these check are not right now 
			// according to current business rules 
			if(clientType == ClientType.ClientAttendee) 
			{
				listParticipents.Items[nIndex].Checked = false;
			}
			else
				listParticipents.Items[nIndex].Checked = true;
			CheckBoxStateChangeByNetwork = false;

		}


			


		# region listParticipents_ItemCheck() ==>All by Zaeem 
		/// <summary>
		/// Sends the main message of rights Managment
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listParticipents_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			
			
			System.Windows.Forms.DialogResult ret;
			MeetingAlerts alert=new MeetingAlerts();
			ClientProfile profile1;
			
			
			
			// these 3 decision object contains those Attendee
			//which is currently the presenter 
			//So that when u want to give some else the rights
			//Firstly rights can be taken away from the current presenter()
			
			// Contains the profile of the current presenter
			ClientProfile decision_profile=null;
			// if Host is the presenter the decision_flag will be true.
			bool decision_flag=false;
			// Index of the current presenter in the list.
			int decision_index=0;

			//  These 3 Variables store the values of no of different users.
			//******************************************************//
			int noof_ClientPresenter=0;
			int noof_ClientHost=0;
			int noof_ClientAttendee=0;
			//******************************************************//
			

			//  This loop count the no of Users and add in the above defined variables
			//******************************************************//
			# region for Loop
			for(int i=0;i<Client.ClientUI.getInstance().arrayParticipents.Count;i++)
			{
			
				profile1=(ClientProfile)arrayParticipents[i];
				
				if(profile1.clientType.ToString().Equals("ClientHost"))
				{
					++noof_ClientHost;
					

					//if host has the rights currently
					# region Normal Version
					if(btn_Whiteboard.Enabled==true && btn_Docshare.Enabled==true)
					
					# endregion 
					# region Stripped Down Version
					//if(btn_Whiteboard.Enabled==true )//&& btn_Docshare.Enabled==true)
					
					# endregion 
					{
						decision_profile=profile1;
                        decision_flag=true;
						decision_index=i;
					}
					
					
				}
				
				else if(profile1.clientType.ToString().Equals("ClientPresenter"))
				{
					++noof_ClientPresenter;
					
					if(!decision_flag)
					{
						decision_profile=profile1;
						decision_index=i;
						
					}	
					

				}
				else if(profile1.clientType.ToString().Equals("ClientAttendee"))
				{
					++noof_ClientAttendee;
				}
					 
 				
			}// end For Loof
			# endregion 
			//******************************************************//
			

			try
			{

				// Zaeem View
				//i dont understand this check 
				if(CheckBoxStateChangeByNetwork)
					return;
				
				
				//Profile contains that participent which is going to be changed.
				// This is the profile clicked by you on the Host side.
				ClientProfile profile = (ClientProfile)arrayParticipents[e.Index];

				// Now added by zaeem.
				// This will run only if i am the presenter
				// It means no one else can change the rights except the Host
				if(NetworkManager.getInstance().profile.clientType == ClientType.ClientHost)
				{


					// if they are not enabled it means that 
					// either Network is disconnected 
					// or Application is still loading.
					if(!(this.btn_Chat.Enabled && this.btn_Snapshot.Enabled))
					{
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"You can't change the rights as either Network is disconnected or Application is still loading",true,false);
						return;
					}
					
					
			
					
					// This falg is a pretty complex one 
					// As it has been changed from a no of places in the application 
					// Zaeem View 
					// That was in old case when u have to take the rights of the current presenter
					// In order to give some one else the rights
					// Flag is false ==> going to be checked


					//		Flag is changed from these locations
					
					
					//                   1
					// This flag is set True when host starts and i am the host.
					//  ClientUI.cs(2913):	==>	flag_checkbox=true;			

					//                   2
					///	ClientUI.cs(10483):					flag_checkbox=false;
					/// NoofLogicalPresenters(ClientProfile comin_clientProf)
					/// this Methods only called on AttendeeDrop. 
					/// When the presenter leaves the meeting 
					/// All tabs are closed and drawing tools are Syncronyzed
					/// flag_checkbox=false;
					/// Assigning false to the above flag means 
					/// that now there is no presenter in the meeting
					/// and you are free to give any one the rights
					
					
					///                 3
					///ClientUI.cs(10808):		flag_checkbox=true;
					/// After Host has be given rights
					

					///                 4
					///ClientUI.cs(10844):								flag_checkbox=true;
					///When Status of some one is set as a presenter
					/// or Some one is made the presenter
					


					///               5
					/// ClientUI.cs(10872):						flag_checkbox=false;
					/// ClientUI.cs(10945):						flag_checkbox=false;
					/// When rights are taken back from some one
					/// 
					///			IN           Forciblytakerights()
					///       ClientUI.cs(11031):				flag_checkbox=false;
					///       ClientUI.cs(11052):				flag_checkbox=false;
					///       


					// Flag is false ==> going to be checked
					// It means u are going to give the rights to some one 
					// as no one has the rights
					//
					if(!flag_checkbox)
					{
						# region Giving rights block
						//Whenever u are giving rights,
						//We assumed in old case that there is no presenter in the meeting
						// So now that assumption is wrong 
						// for this region of code.

						if(profile.clientType == ClientType.ClientHost)
						{
							if(noof_ClientPresenter<1)
							{		
								// This method gives Host the illusion that 
								// Host is the presenter, By enabling its tools
								// Only server knows that he is the Presenter or not 
								// On the code side as well as on the profile side
								// We pass in this method the the profile of sever 
								
								
								// if the 2nd parameter is set true
								// It means that server illusion is true 
								//else
								// Server illusion is false.
								Serverillusion(true,profile);

								// Image is set to Red which means true 
								// Just for the Host,so that he gets the idea abt its status

								//22==>Set image
								listParticipents.SetImage(e.Index,1,22);
								
								
								// Whenever u give some one rights, this check Box "flag_checkbox=true"; 
								// Whenever you take rights from some one, this check Box "flag_checkbox=false"; 
								// So that Host knows that 
								// if some one in the meeting has the rights or not
								// It means that Server Knows that only as this falg is for Server only.
								flag_checkbox=true;
								//e.NewValue= CheckState.Checked;	
								// Making host, a presenter does not require any other message  
								// except that 
								return;
							}
							else
							{
								alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"There is already a presenter in the Meeting",true,false);																
							}
							
						}
						else
						{
						
							# region Normal Version
							if(btn_Whiteboard.Enabled==false && btn_Docshare.Enabled==false)
							# endregion 		
							
								
							# region Stripped Down Version
							//if(btn_Whiteboard.Enabled==false )//&& btn_Docshare.Enabled==false)
							# endregion 		
							
							{

								/*                 "saveID"
								 *  Saves the id of the current presenter Except Host
								 So that this information is usefull when some one leaves the meeting 
								 it matches the ClientID and checkes
								 whether the current presenter has left the meeting
								 As if the current presenter leaves the meeting then
								 according to the business Rules, All the instances
								 shared by him are close and host send the message
								 to all participents to close all the opened tabs
								 and all the drawing tools are Syncronyzed.
								 thats why host should have the record of those who is the 
								 current presenter in the meeting.
								 *								 
								 */


								saveID=profile.ClientId;
								profile.clientType = ClientType.ClientPresenter;
								flag_checkbox=true;
							}
							else
							{
								alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Sorry! Following operation is not allowed as Host is the current presenter of the meeting. In order to make some one else the presenter take the rights away from host.",true,false);
								//MessageBox.Show("");
							}
					
						}
						#endregion 
					}
					
						//Flag is true ==> going to be Unchecked	
					else
					
					{
						# region Taking rights BACK	 block
						
						// This block will run when u take rights back from some one 
						// it means that now there is no presenter in the meeting 
						// So SaveId should be zero as currently there is no presenter in the meeting
						saveID=0;
			
						// If host rights are going to be taken back 

						# region for HOST
						if(profile.clientType == ClientType.ClientHost)
						{
							// first we check that if there is no presenter in the meeting
							// As if there is any presenter in the meeting ,its rights should be taken back first.
							// and we assume that host do not have the rights then , so we return from this in else case
							// if there is no presenter in the meeting then host has the rights deffinitely 
							// as we have checked from the "flag_checkbox", if it true then it means that 
							// some one having the rights 
							// and if that some one is not presenter then deffinitely it will be the host.
							if(noof_ClientPresenter<1)
							{
								ret = MessageBox.Show(this, " Taking the rights will forcibly close all the instances shared by: "+decision_profile.Name.ToString()+"\n"+"                          Do you want to continue ?", "Taking the right back", System.Windows.Forms.MessageBoxButtons.OKCancel);
								if( ret == System.Windows.Forms.DialogResult.OK)
								{				
									// if you say yes then it means that you are willing 
									// to take rights back from Host
									// So all operations are performed again 
									// like closing shared instances of all applications by HOST
									// and Syncronyzing drawing tools.
									this.SyncronizeDrawingTools();
									Serverillusion(false,profile);
									//23==>Unset image
									//Image is set to white to show to host that 
									// he hasn't got the rights
									//
									listParticipents.SetImage(e.Index,1,23);
									flag_checkbox=false;
									// Again all shared are closed 
									HostCloseAllTabs();
									// As in this case its only upto the Host to so need to send messages to 
									// every one 
									return;
								}
								// if Host cancels that message it shold return immediately
								else return;
							}


							// if you want to take rights back from Host and there is already a presenter 
							// It means that presenter is the guys, Who shouldn't have the rights except Host
							// Zaeem View 
							// But i think that this check should not be here as tou come in this block
							//when you click the Host to take Rights back from Him Not from the presenter
							// But i dont remember why that check was made here

							else
							{
								// Zaeem View 
								// Actually i think he should not come in this block 
								// If he comes in this block then 
								// then it means that first the current presenter having the
								// annotation rights should not have them as the call is to take the annotation rights 
								// from the person.
								//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"There can only be one presenter in the Meeting.",true,false);
								ret = MessageBox.Show(this, " Taking the rights will forcibly close all the instances shared by: "+decision_profile.Name.ToString()+"\n"+"                         Do you want to continue ?", "Taking the right back", System.Windows.Forms.MessageBoxButtons.OKCancel);
								if( ret == System.Windows.Forms.DialogResult.OK)
								{				
				
									// This bloack will take rights back from the current presenter.
									// "decision_profile" has the current presenter profile (including HOST)
									// So now the profile of the current presenter including Host having
									// rights is in the decision profile.
									// and e is the object(event)
									// we use its property of index to get the index of that person 
									// whose rights are suppose to be taken back.

									Forciblytakerights(decision_profile,e);
									// Recursilvely call the function again as control has to be back what 
									// it suppose to do in the forst circle.
									// As in the next cirlcle it will run for giving rights, not for taking them.
									listParticipents_ItemCheck(sender,e);
								
									// check box state and image shd be consistent.
									if(decision_profile.clientType.ToString().Equals("ClientHost"))
									{
										//uncheck
										listParticipents.SetImage(decision_index,1,23);
									}
									else if(decision_profile.clientType.ToString().Equals("ClientPresenter"))
									{
										//uncheck
										listParticipents.SetImage(decision_index,1,23);
									}
								}
							
								return;
							}

						}
							# endregion 

						# region for Attendee
						else if(profile.clientType == ClientType.ClientAttendee)
						{
							
							
							ret = MessageBox.Show(this, " Taking the rights will forcibly close all the instances shared by: "+decision_profile.Name.ToString()+"\n"+"                         Do you want to continue ?", "Taking the right back", System.Windows.Forms.MessageBoxButtons.OKCancel);
							if( ret == System.Windows.Forms.DialogResult.OK)
							{
								Forciblytakerights(decision_profile,e);
								listParticipents_ItemCheck(sender,e);
								//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"There can only be one presenter in the Meeting.",true,false);
								if(decision_profile.clientType.ToString().Equals("ClientHost"))
								{
									//uncheck
									listParticipents.SetImage(decision_index,1,23);
								}
								else if(decision_profile.clientType.ToString().Equals("ClientPresenter"))
								{
									//uncheck
									listParticipents.SetImage(decision_index,1,23);
								}
							}
							else return;
						}
						# endregion

						# region for Presenter
						else if(profile.clientType == ClientType.ClientPresenter)
						{


							ret = MessageBox.Show(this, " Taking the rights will forcibly close all the instances shared by: "+decision_profile.Name.ToString()+"\n"+"                         Do you want to continue ?", "Taking the right back", System.Windows.Forms.MessageBoxButtons.OKCancel);
							if( ret == System.Windows.Forms.DialogResult.OK)
							{				
		
								profile.clientType = ClientType.ClientAttendee;
								//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Warning,"Taking the rights will forcibly close all the instances shared by: "+profile.Name.ToString(),true,false);
								HostCloseAllTabs();
								// when u are taking rights if u close all tabs , u shd also syncronyze drawing tools.
								this.SyncronizeDrawingTools();
						
								flag_checkbox=false;
								//listParticipents.SetImage(e.Index,1,23);
							}
							else return;
							
						}
						# endregion 

					# endregion 
					}
						
					
					# region setting image of text Box
					//23 means Unchecked
					if(network.profile.clientType != ClientType.ClientAttendee)
					{
						if(profile.clientType == ClientType.ClientAttendee)
								
						{
							listParticipents.SetImage(e.Index,1,23);
							//e.NewValue= CheckState.Unchecked;
						}
					//22 means Checked
						else
						{
							listParticipents.SetImage(e.Index,1,22);
							//e.NewValue= CheckState.Checked;
						}
					}
					# endregion

					//CheckBoxStateChangeByNetwork=true;//by junaid...for solving problem of presenter becomer attendee and back forth

					//Zaeem View 
					// I have a confusion about this piece of code
					// i think 
					//if state is checked it returns true
					// else False
					bool bTrue = (e.NewValue == CheckState.Checked);
					
					#region Assigning bool bTrue to rights.
					profile.clientAccess.accessApplicationSharing =bTrue ;
					profile.clientAccess.accessShareAudio =bTrue ;
					profile.clientAccess.accessDesktopSharing = bTrue ;
					profile.clientAccess.accessDrawWhiteBoard =bTrue;
					profile.clientAccess.accessSharePresentation =bTrue ;
					profile.clientAccess.accessShareScreen = bTrue ;
					profile.clientAccess.accessShareVideo = bTrue ;
					profile.clientAccess.accessSharePolling = bTrue;
					profile.clientAccess.accessPrivateChat = bTrue;
					profile.clientAccess.accessAllowRightsAssignments= bTrue;
					# endregion 
				
					# region Defining the msg, seeting profile as a parameter and sending receiving msg
				
					// A new message is defined with a control code of AttendeeUpdate
					ControlMessage msg = new ControlMessage(ControlCode.AttendeeUpdate);			
					msg.Parameter = profile;	
					// this method is supposed to send the msg to all for this profile.
					network.SendLoadPacket(msg);
					// Also adds this to the list of my receive messages.
					// Zaeem View 
					// EXTREME CAUTION 
					// Bad mistake in DesktopSharing code in receive thread 
					// myRecvMsgList is removed again in desktop sharing code 
					// although that piece of code will never run but still Why it is there?
					// That might skip a messag ein between 


					network.myRecvMsgList.Add(msg);

					//Trace.WriteLine("line 7968 message of attendee update");
					
					#endregion
				}
					/*
						else if(profile.ClientId == network.profile.ClientId)
						{
							e.NewValue = e.CurrentValue;
							return;				
						}
					*/
				else
				{
				
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Only host can change the rights",true,false);
				
				}

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9900",exp,null,false);
			}

		}

		#endregion 


		/// <summary>
		/// This method suppose to be taken rights back from the profile passed in it 
		/// </summary>
		/// <param name="profile"></param>
		/// <param name="e"></param>
		#region Forciblytakerights(ClientProfile profile,System.Windows.Forms.ItemCheckEventArgs e)

		public void Forciblytakerights(ClientProfile profile,System.Windows.Forms.ItemCheckEventArgs e)
		{
			MeetingAlerts alert=new MeetingAlerts();
			// saveID=0, as we are taking rights back and now there will bw no presenter in the meeting
			saveID=0;


			// If the profile whoes rights are to be taken back is that of Host 
			if(profile.clientType == ClientType.ClientHost)
			{

				//the perticular operations are performed
				this.SyncronizeDrawingTools();
				Serverillusion(false,profile);
				//23==>Unset image
				listParticipents.SetImage(e.Index,1,23);
				flag_checkbox=false;
				HostCloseAllTabs();
				return;
				
			}
			// Zaeem View 
			// It would never be Attendee i think but i am not sure as i
			// might be missing some senario.

			else if(profile.clientType == ClientType.ClientAttendee)
			{

				alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"There can only be one presenter in the Meeting.",true,false);
				return;			
			}
		
			// If the profile whoes rights are to be taken back is that of presenter 
			else if(profile.clientType == ClientType.ClientPresenter)
			{
				
				profile.clientType = ClientType.ClientAttendee;
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Warning,"Taking the rights will forcibly close all the instances shared by: "+profile.Name.ToString(),true,false);
				HostCloseAllTabs();
				// when u are taking rights if u close all tabs , u shd also syncronyze drawing tools.
				// Zaeem view 
				// This is not right as yet due to one condition in that as mentioned in it definition

				this.SyncronizeDrawingTools();
				flag_checkbox=false;
				//listParticipents.SetImage(e.Index,1,23);
							
			}


			// there should be some condition for Host as well 
			// As currently Host has the selected red Box on it 
			if(network.profile.clientType != ClientType.ClientAttendee)
			{

				// Zaeem view 
				// this check will never occur 

				if(profile.clientType == ClientType.ClientAttendee)
								
				{
					listParticipents.SetImage(e.Index,1,23);
					//e.NewValue= CheckState.Unchecked;
				}


				// This else should not be here as this code is suppose to be taking rights back not 
				// for giving rights 
				else
				{
					listParticipents.SetImage(e.Index,1,22);
					//e.NewValue= CheckState.Checked;
				}
			}




			// Also this code is confusing as this piece of code suppose to be taking rights back 
			//CheckBoxStateChangeByNetwork=true;//by junaid...for solving problem of presenter becomer attendee and back forth
			// So bool bTrue should be false.

			bool bTrue = (e.NewValue == CheckState.Checked);
			
			#region Assigning bool bTrue to rights.
			
			profile.clientAccess.accessApplicationSharing =bTrue ;
			profile.clientAccess.accessShareAudio =bTrue ;
			profile.clientAccess.accessDesktopSharing = bTrue ;
			profile.clientAccess.accessDrawWhiteBoard =bTrue;
			profile.clientAccess.accessSharePresentation =bTrue ;
			profile.clientAccess.accessShareScreen = bTrue ;
			profile.clientAccess.accessShareVideo = bTrue ;
			profile.clientAccess.accessSharePolling = bTrue;
			profile.clientAccess.accessPrivateChat = bTrue;
			profile.clientAccess.accessAllowRightsAssignments= bTrue;
			# endregion 		
			
			# region Defining the msg, seeting profile as a parameter and sending receiving msg
			ControlMessage msg = new ControlMessage(ControlCode.AttendeeUpdate);			
			msg.Parameter = profile;	
			network.SendLoadPacket(msg);
			network.myRecvMsgList.Add(msg);
			# endregion 		
					
		
		}

		#endregion 




		public void Serverillusion(bool flag,ClientProfile profile)
		{


			//The buttons are set according to flag

			# region buttons set according to flag


			# region Normal Version
			
			btn_Whiteboard.Enabled=flag;
			btn_Docshare.Enabled=flag;
			btn_Webshare.Enabled =flag;
			btn_Managecontent.Enabled=flag;
			btn_Appshare.Enabled=flag;
			btn_Record.Enabled=flag;
			btn_Invite.Enabled=flag;
			btn_Audio.Enabled=flag;
			btn_Video.Enabled=flag;
			
			# endregion
			



			# region Stripped down Version
			/*
			btn_Docshare.Enabled=false;
			btn_Managecontent.Enabled=false;
			btn_Appshare.Enabled=false;
			btn_Record.Enabled=false;
			btn_Invite.Enabled=false;
			btn_Audio.Enabled=false;
			btn_Video.Enabled=false;

			btn_Whiteboard.Enabled=flag;
			btn_Webshare.Enabled =flag;
			*/
			# endregion
			



			# endregion
			// Menues set according to flag
			this.enableMenus(flag);
			ChangeMenuItemState(menuOpenRecorder,flag);							

			//Annotation rights set according to flag
			profile.clientAccess.annotationRights=flag;
		
			// Current server.
			// If i am the server 
			//Removing Enable/Disable annoatation tools
//			if(profile.ClientId==network.profile.ClientId)
//			{
//				this.enableWhiteBoardTools(flag);
//			}
			
			// Send the Update Message to All
			// I think the purpose of this was that all the other participents 
			// gets the update of the the annotation tool rights status of the Host.
			SendClientProfileUpdateMessage(profile);

			/*ControlMessage msg = new ControlMessage(ControlCode.AttendeeUpdate);			
			msg.Parameter = profile;	
			network.SendLoadPacket(msg);
			network.myRecvMsgList.Add(msg);
			*/	
				
		
		}


		private void btn_Video_Click(object sender, System.EventArgs e)
		{
			if(network.profile.clientType == ClientType.ClientHost)
				ShareMyCamera();
			else if(network.profile.clientAccess.accessShareVideo)
				ShareMyCamera();

		}


		private void btn_Snapshot_Click(object sender, System.EventArgs e)
		{
			try
			{
				TakeSnapShot();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 9546",exp,"Unable to take Snapshot" ,true);
			}
			finally
			{
				if(btn_Snapshot.Enabled==false)
					btn_Snapshot.Enabled=true;
			}
		}


		private void nsButton8_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(desktopSharedProfile !=null)
				{
					try
					{

					}
					catch(Exception exp)// ee)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9950",exp,null,false);
						return;
					}
				}
				if(listParticipents.SelectedItems.Count <1)
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Please select a user ",true,false);
					//MessageBox.Show("Please select a user ","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
					return;
				}
				if(listParticipents.SelectedItems.Count < 1)
					return;
			
				StartDesktopSharing(listParticipents.SelectedItems[0].Index);	
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9958",exp,null,false);
			}

		}


		Thread thStopRecording;



		private void button8_Click(object sender, System.EventArgs e)
		{
			try
			{
				thStopRecording=new Thread(new ThreadStart(stopRecordingThread));
				thStopRecording.Name = "Screen Capture Stop Thread: stopRecordingThread()";	
				thStopRecording.Start();
			}

			catch(Exception exp)// ee)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>9980",exp,null,false);
			}

		}
		
		
		void stopRecordingThread()
		{
			try
			{
				recordingStopButton.Enabled =false;
				recordingPauseButton.Enabled = false;
				recordingRecordButton.Enabled = true;
				screencaptureControl.stop();
				screenCaptureEnabled = false;				
				panelRecordingToolbar.Visible = false;
				if(thStopRecording != null) //rec1
				{
					thStopRecording.Abort();
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10001",exp,null,false);
			}
			finally
			{
				thStopRecording = null;
			}

		}


		[DllImport("shell32.dll")]


		public static extern int ShellExecute(int hWnd,
			string lpszOp, string lpszFile,
			string lpszParams, string lpszDir,int FsShowCmd);

		private void recordingRecordButton_Click(object sender, System.EventArgs e)
		{
			try			
			{	
				screencaptureControl.start();			
				recordingStopButton.Enabled =true;
				recordingPauseButton.Enabled = true;
				recordingRecordButton.Enabled = false;			

				if(!BoolrecordingWarningSent)
				{
					GreetingsMessage msg = new GreetingsMessage(network.profile,"This conference is been recorded");
					NetworkManager.getInstance().SendLoadPacket(msg);
					BoolrecordingWarningSent = true;
				}
				/*Thread th=new Thread(new ThreadStart(recordingStartThread));
				th.Name = "Screen Capture Stop Thread: recordingStartThread()";	
				th.Start();*/
			}
			catch(Exception ee)
			{
				if( MessageBox.Show("You do not have required components of Windows Media Encoder. Encoder caused error: " + ee.ToString() + ". Do you want to download now?","WebMeeting",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.Yes)
				{
					ShellExecute(0,"Open",Info.getInstance().WebsiteName + "/wmencoder.exe","","",1);				
				}
                			

				
				// insert the download the installer here for media sdk

				ee = ee;
			}

		}

		void recordingStartThread()
		{
			try			
			{	
				screencaptureControl.start();			
				recordingStopButton.Enabled =true;
				recordingPauseButton.Enabled = true;
				recordingRecordButton.Enabled = false;			

				if(!BoolrecordingWarningSent)
				{
					GreetingsMessage msg = new GreetingsMessage(network.profile,"This conference is been recorded");
					NetworkManager.getInstance().SendLoadPacket(msg);
					BoolrecordingWarningSent = true;
				}

			}
			catch(Exception ee)
			{
				if( MessageBox.Show("You do not have required components of Windows Media Encoder. Encoder caused error: " + ee.ToString() + ". Do you want to download now?","WebMeeting",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.Yes)
				{
					ShellExecute(0,"Open",Info.getInstance().WebsiteName + "/wmencoder.exe","","",1);				
				}
                			

				
				// insert the download the installer here for media sdk

				ee = ee;
			}

		}


		private void recordingPauseButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				
				recordingStopButton.Enabled =true;
				recordingPauseButton.Enabled = false;
				recordingRecordButton.Enabled = true;
				screencaptureControl.pause();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10100",exp,null,false);
			}
		}

		private void btnWhiteBoard_Click(object sender, System.EventArgs e)
		{
			if(this.IfIamthePresenter())
			{
				tabBody.SelectedTabEx = tabPageInfo;
			}
			
		}

		private void chatMenuPrint_Click(object sender, EventArgs e)
		{
			
		}		
		private void button5_Click_1(object sender, System.EventArgs e)
		{
			if(desktopSharedProfile!=null)
			{
				
			}
		}

		private void toolbarBtnDesktopSharingDisable_Click(object sender, System.EventArgs e)
		{			

			try
			{
				DesktopSharingControlMessage message = new DesktopSharingControlMessage(false,
					network.profile.ClientId,desktopSharedProfile.ClientId);							
				message.Start=false;		
				message.Status = -1;	
				message.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID;
				network.SendLoadPacket(message);		
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10134",exp,null,false);
			}
			try
			{
				TerminateDesktopSharing();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10145",exp,null,false);
			}
		}
		#endregion

		#region Screen shot Exception and checking done by Zaeem
	
		public virtual Image GetScreenShot(System.IntPtr handle)
		{
			Graphics graphics =null;;
			Graphics graphics_new=null;
			Image screenShot_ = null;
			try
			{
				System.Windows.Forms.Control tempCtrl = FromHandle(handle);
				if(tempCtrl != null)
				{
					graphics = tempCtrl.CreateGraphics();
					screenShot_ = new Bitmap(tempCtrl.ClientRectangle.Width, tempCtrl.ClientRectangle.Height, graphics);
					graphics_new= Graphics.FromImage(screenShot_);
					IntPtr handle_1 = graphics.GetHdc();
					IntPtr handle_2 =graphics_new.GetHdc();
					Win32.USER32.BitBlt(handle_2, 0, 0, tempCtrl.ClientRectangle.Width, tempCtrl.ClientRectangle.Height, handle_1, 0, 0, 13369376);
					graphics.ReleaseHdc(handle_1);
					graphics_new.ReleaseHdc(handle_2);
					return screenShot_;
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 9799",exp,"Unable to take Snapshot" ,true);
			}
			finally
			{
				if(null!=graphics) graphics.Dispose();
				if(null!=graphics_new) graphics_new.Dispose();
			}
			return screenShot_;

		}

		
		
		#endregion

		#region public static int DownloadFile(String remoteFilename,
		public static int DownloadFile(String remoteFilename,
			String localFilename)
		{
			// Function will return the number of bytes processed
			// to the caller. Initialize to 0 here.
			int bytesProcessed = 0;

			// Assign values to these objects here so that they can
			// be referenced in the finally block
			Stream remoteStream  = null;
			Stream localStream   = null;
			WebResponse response = null;

			// Use a try/catch/finally block as both the WebRequest and Stream
			// classes throw exceptions upon error
			try
			{
				// Create a request for the specified remote file name
				WebRequest request = WebRequest.Create(remoteFilename);
				if (request != null)
				{
					// Send the request to the server and retrieve the
					// WebResponse object 
					response = request.GetResponse();
					if (response != null)
					{
						// Once the WebResponse object has been retrieved,
						// get the stream object associated with the response's data
						remoteStream = response.GetResponseStream();

						// Create the local file
						localStream = File.Create(localFilename);

						// Allocate a 1k buffer
						byte[] buffer = new byte[1024];
						int bytesRead;

						// Simple do/while loop to read from stream until
						// no bytes are returned
						do
						{
							// Read data (up to 1k) from the stream
							bytesRead = remoteStream.Read (buffer, 0, buffer.Length);

							// Write the data to the local file
							localStream.Write (buffer, 0, bytesRead);

							// Increment total bytes processed
							bytesProcessed += bytesRead;
						} while (bytesRead > 0);
					}
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10260",exp,null,false);
				//Console.WriteLine(e.Message);
			}
			finally
			{
				// Close the response and streams objects here 
				// to make sure they're closed even if an exception
				// is thrown at some point
				if (response     != null) response.Close();
				if (remoteStream != null) remoteStream.Close();
				if (localStream  != null) localStream.Close();
			}

			// Return total bytes processed to caller.
			return bytesProcessed;
		}
	

		#endregion 

		#region documentSharing
		public delegate void DelegateCreateDocumentSharingWindow(MessageObject msg);
		public delegate void DelegateRemoveDocumentSharingWindow(Crownwood.Magic.Controls.TabPage page);
		public delegate void DelegateRemoveSharingWindow(Crownwood.Magic.Controls.TabPage page);
		
		public DelegateCreateDocumentSharingWindow CreateDocumentSharingWindowEx;			
		public DelegateRemoveDocumentSharingWindow DeleteDocumentSharingWindow;
		public DelegateRemoveSharingWindow DeleteSharingWindow;

		private void CreateDocumentSharingWindow(MessageObject msg)
		{		
			try
			{
				if(!((DocumentSharing)msg).bEnabled)
					return;
				ClientProfile prof = NetworkManager.getInstance().GetClientProfileFromList(((DocumentSharing)msg).senderProfile.ClientId);
				if(prof == null)
					return;

				Crownwood.Magic.Controls.TabPage tabDocumentSharing = new Crownwood.Magic.Controls.TabPage("Document Sharing");			
			
				WebMeeting.Client.documentSharingControl documentSharingControl = new documentSharingControl(false);
			
				tabDocumentSharing.Control = documentSharingControl;
				try
				{
					tabBody.TabPages.Add(tabDocumentSharing);		
					//tabBody.TabPages
				}
				catch(Exception exp)// ee)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10306",exp,null,false);
				}
				tabBody.SelectedTabEx = tabDocumentSharing;
			
				arraydocumentSharing.Add(tabDocumentSharing);
			

				//////
				///
				DocumentSharing msg1=(DocumentSharing)msg;
				string[] strLen=msg1.DownloadURL.Split(',');
				//MessageBox.Show(strLen.Length.ToString());
				if(strLen.Length>1)
				{
					documentSharingControl.IsLaunchFromMangeContent=true;									
				}

			
				//documentSharingControl.no_of_pages=
				//////
				documentSharingControl.Top=10;
				documentSharingControl.Left =10;// System.Drawing.Point(16, 24);			
				//documentSharingControl.Size = new System.Drawing.Size(632, 528);
				documentSharingControl.parentTabPage=tabDocumentSharing;
				documentSharingControl.axWebBrowser1.Visible = true;
				documentSharingControl.StartDocumentSharingReciever(msg);
				if(!this.splitter1.IsCollapsed)
					this.splitter1.ToggleState();
				documentSharingControl.IsClosed = false;
				if(prof == null)
				{
					if(tabDocumentSharing.Title.IndexOf("- Disabled") == -1)
						tabDocumentSharing.Title += " - Disabled";
					documentSharingControl.IsClosed = true;
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10346",exp,null,false);
			}
		}

		#endregion

		#region PollingTabControl events
		private void pollingTabControl_Click(object sender, EventArgs e)
		{
			
		}

		private void pollingTabControl_SelectionChanged(object sender, EventArgs e)
		{
			try
			{	
			
				if(pollingTabControl.TabPages.Count > 0)
				{

					PollingTabPage tempPage = (PollingTabPage )pollingTabControl.SelectedTab;					
					int sessionId =tempPage.PollingControl.sessionID;
					if(network.profile.clientType == ClientType.ClientHost)
					{
						TabPagesSyncrhonizationMsessage Msg = new TabPagesSyncrhonizationMsessage(network.profile,sessionId);
						Msg.type = TabPageType.Polling;
						NetworkManager.getInstance().SendLoadPacket(Msg);
					}
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10380",exp,null,false);
			}
		}


		private void pollingTabControl_ClosePressed(object sender, EventArgs e)
		{
			try
			{
				if(pollingTabControl.SelectedTab==null)
					return;
				System.Type tType= pollingTabControl.SelectedTab.GetType();
				if(tType.Equals(typeof(PollingTabPage))) //dont' close tab if its attendee and its a browser control
				{
					PollingTabPage tabPage = (PollingTabPage )pollingTabControl.SelectedTab;				
					if(tabPage.PollingControl.pqd != null) //if this is null it surely is a shared result window so we don't need to send the message across network
					{
						NewPollMessage msg = new NewPollMessage(tabPage.PollingControl.pqd.IsEvaluation);
						msg.sessionID = tabPage.PollingControl.sessionID;
						msg.hostID =  NetworkManager.getInstance().profile.ClientId ;
						msg.active = false;
						msg.ConferenceID = NetworkManager.getInstance().profile.ConferenceID ;
						network.SendLoadPacket(msg);
						pollingTabControl.TabPages.Remove(tabPage);
						arrayPolling.Remove(tabPage);
					}					
								
				}
			
				tabBody.Refresh();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10412",exp,null,false);
			}
		}
		#endregion

		#region button4_Click(
		private void button4_Click(object sender, System.EventArgs e)
		{
			try
			{
				System.Drawing.Point pt = new Point(button4.Top,button4.Right);			
				pt.X = 0;
				cmenu_drawingTools.Show(button4,pt);
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10450",exp,null,false);
			}
		}
		#endregion 

		#region Erease All section 

		private void btnEraser_Click(object sender, System.EventArgs e)
		{
			try
			{
			
				if(tabBody.SelectedTab == tabPageInfo)
				{
				
					DrawingClearMessage msg = new DrawingClearMessage(0);
					msg.m_ControlType = ControlType.DrawingBoard;
					msg.SenderID = -1;
					msg.senderProfile = network.profile;					
					//network.SendLoadPacket(msg);
					lock(this.whiteBoard.sendingArrayList)
					{
						this.whiteBoard.sendingArrayList.Add(msg);
					}
					//ClientUI.getInstance().whiteBosard.ClearAll();
					if(network.profile.clientType == ClientType.ClientHost)
						ClientUI.getInstance().whiteBoard.Clear(false,-1);
					else
						ClientUI.getInstance().whiteBoard.Clear(false,-1);

					ClientUI.getInstance().whiteBoard.Refresh();
				}
				else
				{		
					System.Type a=tabBody.SelectedTab.Control.GetType();
						
					//					if(a.Equals(typeof(browser))) //dont' close tab if its attendee and its a browser control
					//					{
					//						browser browserControl = (browser)tabBody.SelectedTab.Control;
					//					
					//						DrawingClearMessage msg = new DrawingClearMessage(browserControl.sessionID);
					//						msg.m_ControlType = ControlType.Browser;
					//						msg.SenderID = -1;
					//						msg.senderProfile = network.profile;					
					//						network.SendLoadPacket(msg);
					//						if(network.profile.clientType == ClientType.ClientHost)
					//							browserControl.annotationBrowser.ClearItems(false,-1);
					//						else
					//							browserControl.annotationBrowser.ClearItems(false,-1);
					//
					//						browserControl.Refresh();
					//					}

					//Old browser 
					/*
					if(a.Equals(typeof(browser))) //dont' close tab if its attendee and its a browser control
					{
						browser browserControl = (browser)tabBody.SelectedTab.Control;
					
						DrawingClearMessage msg = new DrawingClearMessage(browserControl.sessionID);
						msg.m_ControlType = ControlType.Browser;
						msg.SenderID = network.profile.ClientId;
						msg.senderProfile = network.profile;					
						network.SendLoadPacket(msg);
						browserControl.whiteBoard.ClearAll();
						//						if(network.profile.clientType == ClientType.ClientHost)
						//							browserControl.annotationBrowser.ClearItems(false,NetworkManager.getInstance().profile.ClientId);
						//						else
						//							browserControl.annotationBrowser.ClearItems(false,NetworkManager.getInstance().profile.ClientId);

						browserControl.Refresh();
					}

					*/	







					if(a.Equals(typeof(browser))) //dont' close tab if its attendee and its a browser control
					{
						browser browserControl = (browser)tabBody.SelectedTab.Control;
					
						DrawingClearMessage msg = new DrawingClearMessage(browserControl.sessionID);
						
						if(ClientUI.flag_ImageMode)
						{
							msg.m_ControlType = ControlType.BrowserImage;
							browserControl.whiteBoard.Clear(-1);

						}
						else 
						{
							msg.m_ControlType = ControlType.Browser;
							browserControl.annotationBrowser.ClearItems(true,NetworkManager.getInstance().profile.ClientId);
						
						}

						// To clear All Annotation 
						msg.SenderID = -1;
						msg.senderProfile = network.profile;		
						//MessageBox.Show(msg.SenderID+" button click eveent ");
						network.SendLoadPacket(msg);

						browserControl.Refresh();
					}


					else if(a.Equals(typeof(documentSharingControl)))
					{
						documentSharingControl docControl = (documentSharingControl)tabBody.SelectedTab.Control;
						DrawingClearMessage msg = new DrawingClearMessage(docControl.sessionID);
						msg.m_ControlType = ControlType.DocumentSharing;
						msg.SenderID = -1;
						msg.senderProfile = network.profile;					
						network.SendLoadPacket(msg);

						if(network.profile.clientType == ClientType.ClientHost)
							docControl.annotationBrowser.ClearItems(false,-1);
						else
							docControl.annotationBrowser.ClearItems(false,-1);
							
							
						docControl.Refresh();
					
					}
					else if(tabBody.SelectedTab.Title.IndexOf("Application Sharing") != -1)
					{
						DrawingClearMessage msg = new DrawingClearMessage(0);
						msg.m_ControlType = ControlType.ApplicationSharing;
						msg.SenderID = -1;
						msg.senderProfile = network.profile;						
						msg.m_ControlType = ControlType.ApplicationSharing;
									
						WebMeeting.ApplicationSharing appShareControl = (WebMeeting.ApplicationSharing)tabBody.SelectedTab.Control;
						appShareControl.whiteBoard.Clear(false,-1);
						//ClientUI.getInstance().whiteBoard.Clear(false,NetworkManager.getInstance().profile.ClientId);
						msg.sessionID = -1;
						//network.SendLoadPacket(msg);
						lock(appShareControl.whiteBoard.sendingArrayList)
						{
							appShareControl.whiteBoard.sendingArrayList.Add(msg);
						}


						//ClientUI.getInstance().whiteBoard.ClearAll();
					
					}
				}
		
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>8755",exp,null,false);
			}
		

		}


		#endregion 

		#region btnClearWhiteBoard_Click(
		private void btnClearWhiteBoard_Click(object sender, System.EventArgs e)
		{
			try
			{
			{
			
				if(tabBody.SelectedTab == tabPageInfo)
				{
				
					DrawingClearMessage msg = new DrawingClearMessage(0);
					msg.m_ControlType = ControlType.DrawingBoard;
					msg.SenderID = network.profile.ClientId;
					msg.senderProfile = network.profile;					
					//network.SendLoadPacket(msg);
					lock(this.whiteBoard.sendingArrayList)
					{
						this.whiteBoard.sendingArrayList.Add(msg);
					}
					//ClientUI.getInstance().whiteBosard.ClearAll();
					if(network.profile.clientType == ClientType.ClientHost)
						ClientUI.getInstance().whiteBoard.Clear(false,NetworkManager.getInstance().profile.ClientId);
					else
						ClientUI.getInstance().whiteBoard.Clear(false,NetworkManager.getInstance().profile.ClientId);

					ClientUI.getInstance().whiteBoard.Refresh();
				}

				

				else
				{		
					System.Type a=tabBody.SelectedTab.Control.GetType();
					//Old Browser 
//						
//					if(a.Equals(typeof(browser))) //dont' close tab if its attendee and its a browser control
//					{
//						browser browserControl = (browser)tabBody.SelectedTab.Control;
//					
//						DrawingClearMessage msg = new DrawingClearMessage(browserControl.sessionID);
//						msg.m_ControlType = ControlType.Browser;
//						msg.SenderID = network.profile.ClientId;
//						msg.senderProfile = network.profile;					
//						network.SendLoadPacket(msg);
//						if(network.profile.clientType == ClientType.ClientHost)
//							browserControl.annotationBrowser.ClearItems(false,NetworkManager.getInstance().profile.ClientId);
//						else
//							browserControl.annotationBrowser.ClearItems(false,NetworkManager.getInstance().profile.ClientId);
//
//						browserControl.Refresh();
//					}

					if(a.Equals(typeof(browser))) //dont' close tab if its attendee and its a browser control
					{
						browser browserControl = (browser)tabBody.SelectedTab.Control;
					
						DrawingClearMessage msg = new DrawingClearMessage(browserControl.sessionID);
						
						if(flag_ImageMode)
						{
							msg.m_ControlType = ControlType.BrowserImage;
							browserControl.whiteBoard.Clear(NetworkManager.getInstance().profile.ClientId);

						}
						else 
						{
							msg.m_ControlType = ControlType.Browser;
							browserControl.annotationBrowser.ClearItems(false,NetworkManager.getInstance().profile.ClientId);
						
						}

						
						msg.SenderID = network.profile.ClientId;
						msg.senderProfile = network.profile;		
						//MessageBox.Show(msg.SenderID+" button click eveent ");
						network.SendLoadPacket(msg);

						browserControl.Refresh();
					}
					else if(a.Equals(typeof(documentSharingControl)))
					{
						documentSharingControl docControl = (documentSharingControl)tabBody.SelectedTab.Control;
						DrawingClearMessage msg = new DrawingClearMessage(docControl.sessionID);
						msg.m_ControlType = ControlType.DocumentSharing;
						msg.SenderID = network.profile.ClientId;
						msg.senderProfile = network.profile;					
						network.SendLoadPacket(msg);

						if(network.profile.clientType == ClientType.ClientHost)
							docControl.annotationBrowser.ClearItems(false,NetworkManager.getInstance().profile.ClientId);
						else
							docControl.annotationBrowser.ClearItems(false,NetworkManager.getInstance().profile.ClientId);
							
							
						docControl.Refresh();
					
					}
					else if(tabBody.SelectedTab.Title.IndexOf("Application Sharing") != -1)
					{
						DrawingClearMessage msg = new DrawingClearMessage(0);
						
						msg.SenderID = network.profile.ClientId;
						msg.senderProfile = network.profile;						
						msg.m_ControlType = ControlType.ApplicationSharing;
									
						WebMeeting.ApplicationSharing appShareControl = (WebMeeting.ApplicationSharing)tabBody.SelectedTab.Control;
						appShareControl.whiteBoard.Clear(false,NetworkManager.getInstance().profile.ClientId);
						//ClientUI.getInstance().whiteBoard.Clear(false,NetworkManager.getInstance().profile.ClientId);
						msg.sessionID = appShareControl.nSessionID;
						//network.SendLoadPacket(msg);
						lock(appShareControl.whiteBoard.sendingArrayList)
						{
							appShareControl.whiteBoard.sendingArrayList.Add(msg);
						}


						//ClientUI.getInstance().whiteBoard.ClearAll();
					
					}
				
				}
			}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10540",exp,null,false);
			}
		}
		
		#endregion 

		#region menuMoodYellow_Click(
		private void menuMoodYellow_Click(object sender, System.EventArgs e)
		{
			//	MessageBox.Show("Yellow");
		}		
		#endregion 

		#region Chatpopup Window 
		public delegate void FnShowChatWindowDelegate(string heading,string message);
		public FnShowChatWindowDelegate FnShowChatWindow; 
		
		public void ShowChatWindow(string heading,string message)
		{
			try
			{
				FontStyle ff=new FontStyle();
				ff=ff|FontStyle.Bold;			
				notifier= new WebMeeting.CustomUIControls.TaskbarNotifier();
				notifier.ContentClickable = false;	
				Image image = Image.FromFile(Application.StartupPath + "\\window.bmp");
				notifier.SetBackgroundBitmap(image,Color.FromArgb(255,0,255));
				Image imageClose = Image.FromFile(Application.StartupPath + "\\close.bmp");
				notifier.SetCloseBitmap(imageClose,Color.FromArgb(255,0,255),new Point(218,2));
				notifier.TitleRectangle=new Rectangle(08,20,230,25); 
				notifier.NormalTitleFont = new Font(this.Font.FontFamily,8,ff);
				notifier.NormalTitleColor = Color.FromArgb(0,0,0);
				notifier.ContentRectangle=new Rectangle(08,25,230,100);								
				notifier.CloseClick += new EventHandler(notifier_CloseClick);
				notifier.Height= 40;
				notifier.Show(heading,message,500,3000,500);
				
				/*
				if(!globalGreetingsMessage.IsInformation)
					notifier.Show("Greetings From " + globalGreetingsMessage.senderProfile.Name,globalGreetingsMessage.MessageString,500,3000,500);
				else
					notifier.Show("Information Sent By:" + globalGreetingsMessage.senderProfile.Name,globalGreetingsMessage.MessageString,500,3000,500);
					*/
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10590",exp,null,false);
			}				

		}

		#endregion

		#region presentationsMenu_Click(
		private void presentationsMenu_Click(object sender, EventArgs e)
		{
			try
			{
				Utility.NiceMenu.NiceMenu myMenu = (Utility.NiceMenu.NiceMenu)sender;
				if((network.profile.clientType == ClientType.ClientHost) || (
					network.profile.clientAccess.accessSharePresentation))
				{
					
					ShareMyDocument(recentlyUsedPresentationsArray[myMenu.Index].ToString());
				}

			}
			catch(Exception exp )
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10610",exp,null,false);
			}
         
		}
		#endregion 


		#region listParticipents_MouseDown(
		private void listParticipents_MouseDown(object sender, MouseEventArgs e)
		{
			
			
			try
			{
			
				if(e.Button == MouseButtons.Right)
					return;
			
				ListViewItem lvItem =  listParticipents.GetItemAt(e.X,e.Y);
				if(lvItem!=null)
				{
					int row,column;
					if(HitTest(new Point(e.X,e.Y),out row,out column))
					{
						if((column == 5) || ((network.profile.clientType == ClientType.ClientAttendee) && (column==2)))
						{
							lvItem.Selected = true;
							ShowPopup(new Point(e.X,e.Y));
							//	contextMenuParticipents.Show(listParticipents,new Point(e.X,e.Y+2));
						}
						else if(column == 1) 
						{
							CheckState state;
							CheckState newState;
							if(((ClientProfile)arrayParticipents[lvItem.Index]).clientType != ClientType.ClientAttendee)
							{
								state = CheckState.Checked;
								newState = CheckState.Unchecked;
							}
							else
							{
								state = CheckState.Unchecked;
								newState = CheckState.Checked;
							}

							ItemCheckEventArgs nEvent = new ItemCheckEventArgs(lvItem.Index,newState,state);
							listParticipents_ItemCheck(listParticipents,nEvent);

						}
                    
					}
				
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>10666",exp,null,false);
			}
		}

		#endregion 



		#region Context Menu Coding

		#region Variable Declration
		MenuCommand contextMenuAttendeePreviliges;
		MenuCommand contextMenuPriviligesAllAttendees;
		
		
		MenuCommand contextMenuPresenter_Doc;
		
		MenuCommand contextMenuBoot ;//= new MenuCommand("Boot from meeting",contextMenuImageList, 0);			
		//MenuCommand contextMenuMood ;//= new MenuCommand("Mood",contextMenuImageList, 0);
		
		
		MenuCommand contextMenuMoodYellow;
		MenuCommand contextMenuMoodBlue;
		MenuCommand contextMenuMoodGreen;
		MenuCommand contextMenuMoodRed;
		MenuCommand contextMenuMoodOrange;
		PopupMenu popup = new PopupMenu();
		#endregion
		private void CreateContextMenuItems()
		{
			contextMenuAttendeePreviliges = new MenuCommand("Attendee Privileges");
			contextMenuAttendeePreviliges.Enabled=false;
			contextMenuPriviligesAllAttendees = new MenuCommand("All Attendees");
			contextMenuPriviligesAllAttendees.Click += new EventHandler(contextMenuPriviligesAllAttendees_Click);
			
			//contextMenuPresenter_Doc = new MenuCommand("Annotation Rights");
			//contextMenuPresenter_Doc.Click +=new EventHandler(ContextMenuClickEvent);
			
			contextMenuBoot = new MenuCommand("Boot from meeting");			
			contextMenuBoot.Click +=new EventHandler(ContextMenuClickEvent);
			//contextMenuMood = new MenuCommand("Mood");
			//contextMenuMood.Click +=new EventHandler(ContextMenuClickEvent);
			contextMenuMoodYellow = new MenuCommand("Yellow",listViewImageList,10);
			contextMenuMoodYellow.Click   +=new EventHandler(ContextMenuClickEvent);
			contextMenuMoodBlue = new MenuCommand("Blue",listViewImageList,11);
			contextMenuMoodBlue.Click  +=new EventHandler(ContextMenuClickEvent);
			contextMenuMoodGreen =new MenuCommand("Green",listViewImageList,12);
			contextMenuMoodGreen.Click  +=new EventHandler(ContextMenuClickEvent);
			contextMenuMoodRed =new MenuCommand("Red",listViewImageList,13);
			contextMenuMoodRed.Click  +=new EventHandler(ContextMenuClickEvent);
			contextMenuMoodOrange = new MenuCommand("Orange",listViewImageList,14);
			contextMenuMoodOrange.Click  +=new EventHandler(ContextMenuClickEvent);

			//contextMenuMood.MenuCommands.AddRange(new MenuCommand[]{contextMenuMoodYellow,contextMenuMoodBlue,
						//											   contextMenuMoodGreen,contextMenuMoodRed,contextMenuMoodOrange});
			contextMenuAttendeePreviliges.MenuCommands.Add(contextMenuPriviligesAllAttendees);

			popup.MenuCommands.AddRange(new MenuCommand[] {contextMenuBoot/*,contextMenuMood*//*,contextMenuPresenter_Doc*/});

            			
		}
		private void contextMenuPriviligesAllAttendees_Click(object sender, EventArgs e)
		{
			if(network.profile.clientType == ClientType.ClientHost)
				ChangeAccessCodesEx();
		}



		private void UpdateMenuAccordingly(WebMeeting.Common.ClientProfile prof)
		{
	
			if(prof.clientType.ToString()=="ClientPresenter")
			{
				contextMenuPresenter_Doc.Checked=true;
			
			}

			else if(prof.clientType.ToString()!="ClientPresenter")
			{
				if(prof.clientAccess.annotationRights)
				{
					contextMenuPresenter_Doc.Checked=true;
				}
				else
				{
					contextMenuPresenter_Doc.Checked=false;
				}
			}
			

		}
		


		private void AttachHostMenu(bool bState,WebMeeting.Common.ClientProfile prof)
		{
			//UpdateMenuAccordingly(prof);
				if(network.profile.clientType == ClientType.ClientHost)
				{
					/*
					if(this.ifHostisPresenter())
						{
							contextMenuPresenter_Doc.Visible = true;
							contextMenuPresenter_Doc.Enabled = true;
					
						}
					else
						{
							contextMenuPresenter_Doc.Visible = false;
							contextMenuPresenter_Doc.Enabled = false;
						}
					*/

					contextMenuBoot.Visible = true;
					contextMenuBoot.Enabled= true;

				}	
				else if(network.profile.clientType == ClientType.ClientPresenter)
				{
					/*
					contextMenuPresenter_Doc.Enabled = true;
					contextMenuPresenter_Doc.Visible=true;
					*/
					contextMenuBoot.Enabled=false;
					contextMenuBoot.Visible=false;
				
				}
			/*
			if(prof.clientType==ClientType.ClientPresenter)			
			{
				contextMenuPresenter_Doc.Checked=true;
			
			}
			*/
			
		}
		
		private void AttachParticipentMenu(bool bState,WebMeeting.Common.ClientProfile prof)
		{
			/*
			if(prof.ClientId!=network.profile.ClientId)
			{
				UpdateMenuAccordingly(prof);
			}
			*/
			
		}
	
		public void ConnectNetwork(bool val)
		{		
		}
		private void ShowPopup(Point pt)
		{
			try
			{
				if(network.profile.clientType.ToString()!="ClientAttendee")
				{
					
					if(listParticipents.SelectedItems.Count > 0)
					{
						ClientProfile prof = (ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index];
						if(prof.ClientId!=network.profile.ClientId)
						{
							
							AttachHostMenu(false,prof);
							//contextMenuMood.Visible= true;
					
						}
						else
						{
						
							if((network.profile.clientType == ClientType.ClientHost) || ((network.profile.clientType == ClientType.ClientPresenter) && (network.profile.clientAccess.accessAllowRightsAssignments)))
							{
								AttachHostMenu(true,prof);					
							}
							else
							{
								AttachHostMenu(false,prof);
							
							}

							//AttachParticipentMenu(true,prof);
							//contextMenuMood.Visible= false;
						}								
						popup.TrackPopup(listParticipents.PointToScreen(pt));
					}
				}
			}
			catch(Exception ee)
			{
				ee= ee;

			}
		}		

		private void closeConference_closeAllParticipants()
		{
			try
			{ 
				if(listParticipents.Items.Count < 2)
					return;					
				for(int i=0; i<listParticipents.Items.Count ;i++)
				{
					if(this.network.profile.ClientId == ((ClientProfile)arrayParticipents[i]).ClientId  )
						continue;
					ControlMessage cMessage = new ControlMessage(ControlCode.PresenterKickedPerson);
					cMessage.Parameter = (ClientProfile)arrayParticipents[i];
					/*if(((ClientProfile)cMessage.Parameter).clientType == ClientType.ClientHost)
					{
						MeetingAlerts alert=new MeetingAlerts();
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"You cannot kick host",true,false);
						return;
					}*/
					cMessage.SenderID = network.profile.ClientId;
					network.SendLoadPacket(cMessage);
				}
			}
			catch(Exception )
			{
			
			}
		}

		/// <summary>
		/// Used to send the Attendee Update message to the server.
		/// </summary>
		/// <param name="profile"></param>
		public void SendClientProfileUpdateMessage(ClientProfile profile)
		{
			ControlMessage msg = new ControlMessage(ControlCode.AttendeeUpdate);			
			msg.Parameter = profile;	
			network.SendLoadPacket(msg);
			network.myRecvMsgList.Add(msg);
		}
		private void popup_Selected(MenuCommand item)
		{
			try
			{				
				if(item.Text == "Share Video")
				{
					ShareMyCamera();
				}		
			
				else if(item.Text == "Make Presenter")
				{
					CheckBoxStateChangeByNetwork = false;
					listParticipents.SelectedItems[0].Checked = true;
				}			



				// this is used for annotation rights 
				//
				else if(item.Text == "Annotation Rights")
				{
					// there shd atleast be a single presenter in the meeting.
					if(listParticipents.SelectedItems.Count > 0)
					{
						ClientProfile prof = (ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index];
						
						if(prof != null)
						{
							// If i am the Host Or its not me as i dont want to change my rights 
							// presenter is the only person who is allowed to change the annotation rights 
							// and he shd not be allowed to take his own rights 
							if(prof.ClientId!=network.profile.ClientId || prof.clientType==ClientType.ClientHost)
							{
								//own check 
								//MessageBox.Show("==>"+network.profile.clientType.ToString());
								if(network.profile.clientType.ToString()!="ClientAttendee" || (btn_Whiteboard.Enabled==true && btn_Docshare.Enabled==true))
								{
									if(contextMenuPresenter_Doc.Checked==false)
									{
										contextMenuPresenter_Doc.Checked=true;
										prof.clientAccess.annotationRights=true;
										SendClientProfileUpdateMessage(prof);
									}
									else
									{
										contextMenuPresenter_Doc.Checked=false;
										prof.clientAccess.annotationRights=false;
										SendClientProfileUpdateMessage(prof);
									}
								}//own check
								else
								{
									MeetingAlerts alert=new MeetingAlerts();
									alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"You don't have the privilleges to change the rights.",true,false);
						
								}

							}
							else
							{
								MeetingAlerts alert=new MeetingAlerts();
								alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"You dont have the privilleges to change your own rights.",true,false);
						
							}

							//Trace.WriteLine("line 8826 message of attendee update");
						}
					}


					//MessageBox.Show("==>Document");
					//CheckBoxStateChangeByNetwork = false;
					//listParticipents.SelectedItems[0].Checked = true;
				}			
			
					/*
									else if(item.Text == "Make Presenter")
									{
										CheckBoxStateChangeByNetwork = false;
										listParticipents.SelectedItems[0].Checked = true;
									}			
			
									else if(item.Text == "Make Presenter")
									{
										CheckBoxStateChangeByNetwork = false;
										listParticipents.SelectedItems[0].Checked = true;
									}			
			
									else if(item.Text == "Make Presenter")
									{
										CheckBoxStateChangeByNetwork = false;
										listParticipents.SelectedItems[0].Checked = true;
									}			
			
					*/
				else if(item.Text == "Allow Private Chat")
				{
					if(listParticipents.SelectedItems.Count > 0)
					{
						ClientProfile prof = (ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index];
						if(prof != null)
						{
							prof.clientType = ClientType.ClientPresenter;
							prof.clientAccess.accessPrivateChat = !item.Checked;
							SendClientProfileUpdateMessage(prof);							
							//Trace.WriteLine("line 8826 message of attendee update");
						}
					}
				}
				else if(item == contextMenuBoot)
				{
					if(network.profile.clientType == ClientType.ClientAttendee)
						return;

					if(listParticipents.SelectedItems.Count < 1)
						return;					
					ControlMessage cMessage = new ControlMessage(ControlCode.PresenterKickedPerson);
					cMessage.Parameter = (ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index];
					if(((ClientProfile)cMessage.Parameter).clientType == ClientType.ClientHost)
					{
						MeetingAlerts alert=new MeetingAlerts();
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"You cannot kick host",true,false);
						//MessageBox.Show("You cannot kick host","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Warning);
						return;
					}
					cMessage.SenderID = network.profile.ClientId;

					if(network.profile.clientType == ClientType.ClientPresenter)
					{
						MeetingAlerts alert=new MeetingAlerts();
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Only host can kick a person out.",true,false);						
					}

					else if(MessageBox.Show("Are you sure you want to kick " + ((ClientProfile)cMessage.Parameter).Name,"WebMeeting" ,MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
					{
						network.SendLoadPacket(cMessage);
					}

					
				}
				else if(item.Text == "Allow Audio")
				{
					if(listParticipents.SelectedItems.Count > 0)
					{
						ClientProfile prof = (ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index];
						if(prof != null)
						{
							prof.clientType = ClientType.ClientPresenter;
							prof.clientAccess.accessShareAudio = !item.Checked;
							SendClientProfileUpdateMessage(prof);							
						}
					}
				}
				else if(item.Text == "Allow Video")
				{
					if(listParticipents.SelectedItems.Count > 0)
					{
						ClientProfile prof = (ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index];
						if(prof != null)
						{
							prof.clientType = ClientType.ClientPresenter;
							prof.clientAccess.accessShareVideo = !item.Checked;
							SendClientProfileUpdateMessage(prof);							
						}
					}
				}
				else if(item.Text == "Subscribe Video")
				{
					if(listParticipents.SelectedItems.Count < 1)
						return;
					ClientProfile prof = (ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index];
					SubscribetoUserCamera(prof);
				}
				else if(item.Text == "Subscribe Audio")
				{
					if(listParticipents.SelectedItems.Count < 1)
						return;
					ClientProfile prof = (ClientProfile)arrayParticipents[listParticipents.SelectedItems[0].Index];
					
					SubscribetoUserAudio(prof);

				}
				else if(item.Text == "Open Chat Window")
				{
					OpenChatWindow(false);
				}
				else if(item.Text == "Share Audio")
				{
					ShareMyAudio();
				}
				else if(item.Text == "Disable Audio")
				{
					DisableMyAudio();
					//DisableMyAudio(audioTabPage);
				}
				else if(item.Text == "Disable Video")
				{
					DisableMyCamera();
				}			
					/*	else if(item.Text == "Remote Control")	
						{
							if(listParticipents.SelectedItems.Count <1)
							{
								MeetingAlerts alert=new MeetingAlerts();
								alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Please select a user ",true,false);
								//MessageBox.Show("Please select a user ","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
								return;
							}
					
							//MeetingAlerts alert1=new MeetingAlerts();
							// alert1.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Limited Version  ",true,false);

					
							StartDesktopSharing(listParticipents.SelectedItems[0].Index);	// limited version
						}
						*/
				else if(item.Text == "Send Greetings")
				{

					if(listParticipents.SelectedItems.Count <1)
						return;
					SendGreetings(listParticipents.SelectedItems[0].Index);		
				}
				else if(item.Text == "Options")
				{
					if(listParticipents.SelectedItems.Count <1)
						return;
					ChangeAccessCodes(listParticipents.SelectedItems[0].Index);
				}
				int nIndex = 12;					
				bool bSend = false;
				if(item.Text == contextMenuMoodYellow.Text)
				{							
					_statusBar.comboMood.SelectedIndex = 0;
					nIndex = 10 + 0;
					bSend = true;
				}
				else if(item.Text ==  contextMenuMoodBlue.Text)
				{
					nIndex = 10 + 1;
					_statusBar.comboMood.SelectedIndex = 1;
					bSend = true;
				}
				else if(item.Text ==  contextMenuMoodGreen.Text)
				{
					nIndex = 10 + 2;
					_statusBar.comboMood.SelectedIndex = 2;
					bSend = true;
				}
				else if(item.Text ==  contextMenuMoodRed.Text)
				{
					nIndex = 10 + 3;
					_statusBar.comboMood.SelectedIndex = 3;
					bSend = true;
				}
				else if(item.Text ==  contextMenuMoodOrange.Text)
				{
					nIndex = 10 + 4;
					_statusBar.comboMood.SelectedIndex = 4;
					bSend = true;
				}
				if(bSend)
				{
					NetworkManager.getInstance().profile.m_MoodIndicatorColor =(MoodIndicator) nIndex;
					//Trace.WriteLine("line 8972 message of attendee update");
					ControlMessage m = new ControlMessage(ControlCode.AttendeeUpdate);
					m.SenderID = -1;//network.profile.ClientId;
					m.Parameter = network.profile;
					NetworkManager.getInstance().SendLoadPacket(m);						
					NetworkManager.getInstance().myRecvMsgList.Add(m);
				}			
				
			}
			catch(Exception ee)
			{
				ShowExceptionMessage(" Menu handler has encountered an exception. " + ee.Message);
			}

		}
		#endregion

		#region ContextMenuClickEvent(
		private void ContextMenuClickEvent(object sender, EventArgs e)
		{
			popup_Selected((MenuCommand)sender);

		}
		#endregion 

		#region btn_Invite_Click(
		private void btn_Invite_Click(object sender, EventArgs e)
		{
			InviteForm form = new InviteForm();
			form.Show();
		}
		#endregion 

		#region listParticipents_MouseMove(
		private void listParticipents_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				//Trace.WriteLine("Mouse Move event occur in the listparticipant");
				ListViewItem lvItem =  listParticipents.GetItemAt(e.X,e.Y);
				if(lvItem!=null)
				{				
					int row,column;		
					if(this.HitTestForMouseHover(new Point(e.X,e.Y),out row,out column))
					{									
						if(network.profile.clientType == ClientType.ClientHost)
						{
							switch(column)
							{
								case 2:									                                
								{								
									if(NetworkManager.thisInstance.profile.m_ConnectionIndicator==ConnectionIndicator.Red)
									{//red=slow
										m_listBalloon.SetToolTip(listParticipents,"Connection Indicator. \r\nSpeed is Slow");
									}
									else if(NetworkManager.thisInstance.profile.m_ConnectionIndicator==ConnectionIndicator.Yellow)
									{//yellow=moderate
										m_listBalloon.SetToolTip(listParticipents,"Connection Indicator. \r\nSpeed is Moderate");
									}
									else if(NetworkManager.thisInstance.profile.m_ConnectionIndicator==ConnectionIndicator.Green)
									{//green=fast
										m_listBalloon.SetToolTip(listParticipents,"Connection Indicator. \r\nSpeed is Fast");
									}									
									//Trace.WriteLine(column.ToString());
									//m_listBalloon.SetToolTip(listParticipents,"Connection Indicator. \r\nGreen = Fast\r\nYellow = Moderate\r\nRed = Slow");
								}
									break;							
								case 3:
								{								
									m_listBalloon.SetToolTip(listParticipents,"Mood Indicator.");
									//Trace.WriteLine(column.ToString());
								}
									break;					
							}				
						}
						return;
					}								
				}	
				m_listBalloon.SetToolTip(listParticipents, "Participants in Meeting");
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>11220",exp,null,false);
			}
		}
		#endregion 

		#region aboutCompassnav()
		private void aboutCompassnav()
		{
			try
			{
				aboutWebMeeting frmAboutWebMeeting = new aboutWebMeeting();
				frmAboutWebMeeting.ShowDialog();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>11240",exp,exp.Message.ToString(),true);
				//				MessageBox.Show(this, ex.Message);
			}	
		}
		#endregion 

		#region btn_Chat_Click(
		private void btn_Chat_Click(object sender, System.EventArgs e)
		{
			if(this.splitter1.IsCollapsed)
				this.splitter1.ToggleState();
			
			chatTabControl.SelectedTab = tabPageChat ;

			OpenChatWindow(false);
		}
		#endregion 


		#region menuItem11_Click(
		private void menuItem11_Click(object sender, System.EventArgs e)
		{
			Close();
		}
		#endregion 

		
		#region ShowWhiteBoard(bool val)
		public bool IfNativemsg=false;
		public void ShowWhiteBoard(bool val)
		{
			if(val)//show white board
			{
				//true);
				IfNativemsg=true;
				//tabBody.count=0;
				if(tabBody.TabPages.Contains(tabPageInfo))
				{				
					tabBody.SelectedTabEx = tabPageInfo;

				}
				else
				{					
					Invoke(AddTabPageToMainBody,new object[]{tabPageInfo});	
					tabBody.SelectedTabEx = tabPageInfo;
					
				}
			}
			else//hide whiteboard
			{
				if(tabBody.TabPages.Contains(tabPageInfo))
				{					
					//tabBody.TabPages.Remove(tabPageInfo);
					Invoke(RemoveTabPageFromMainBody,new object[]{tabPageInfo});
					//		WhiteBoardStatus(false);
				}
			}

		}

		#endregion 

		#region JustOpenWhiteBoard()
		public void JustOpenWhiteBoard()
		{
			//			if(val)//show white board
			//			{
			Crownwood.Magic.Controls.TabPage tbpage;
			tbpage=tabBody.SelectedTabEx; 
			//true);
			if(!tabBody.TabPages.Contains(tabPageInfo))
			{
				Invoke(AddTabPageToMainBody,new object[]{tabPageInfo});	
				//tabBody.SelectedTabEx=tbpage; 
			}
		}
		#endregion 

		bool RestoreUp = true;
		int panalHeight = 0;


		#region btnRestoreUp_Click(object sender, System.EventArgs e)
		private void btnRestoreUp_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(RestoreUp )
				{	
					//this.panel1.Height = 35 ;
					this.panel1.Height = this.panel1.Height + this.panalAdvertiseBrowser.Height ;
					RestoreUp = false;
					btnRestoreUp.ImageIndex = 2;
				}
				else
				{
					//this.panel1.Height = this.panel1.Height + this.panalAdvertiseBrowser.Height;
					this.panel1.Height = this.panel1.Height + this.panalAdvertiseBrowser.Height-110 ;
					RestoreUp = true;			
					btnRestoreUp.ImageIndex = 3;
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>11350",exp,exp.Message.ToString(),true);
			}
		}

		#endregion 

		#region axWebBrowser1_DownloadComplete(object sender, System.EventArgs e)
		private void axWebBrowser1_DownloadComplete(object sender, System.EventArgs e)
		{
			try
			{
				doc = (mshtml.HTMLDocument)axWebBrowser1.Document;
				mshtml.HTMLDocumentEvents2_Event iEvent;
				iEvent = (mshtml.HTMLDocumentEvents2_Event) doc;
				iEvent.oncontextmenu+=new mshtml.HTMLDocumentEvents2_oncontextmenuEventHandler(ContextMenuEventHandler);
			}
			catch(Exception exp)
			{
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>11336",exp,exp.Message.ToString(),true);
			}
		}

		#endregion 


		#region axWebBrowser2_DownloadComplete(object sender, System.EventArgs e)
		private void axWebBrowser2_DownloadComplete(object sender, System.EventArgs e)
		{
			try
			{
				doc2 = (mshtml.HTMLDocument)axWebBrowser2.Document;
				mshtml.HTMLDocumentEvents2_Event iEvent;
				iEvent = (mshtml.HTMLDocumentEvents2_Event) doc2;
				iEvent.oncontextmenu+=new mshtml.HTMLDocumentEvents2_oncontextmenuEventHandler(ContextMenuEventHandler);
			}			
			catch(Exception exp)
			{
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>11336",exp,exp.Message.ToString(),true);
			}

		}
		#endregion 

		#region ContextMenuEventHandler(mshtml.IHTMLEventObj e)
		private bool ContextMenuEventHandler(mshtml.IHTMLEventObj e)
		{
			//e.cancelBubble=true;
			return false;
		}
		#endregion 


		#region isWhiteBoardSelected()
		public bool isWhiteBoardSelected()
		{
			try
			{
				//Trace.WriteLine("    white brush     " + tabPageInfo.Title); 
				if(tabBody.TabPages.Contains(tabPageInfo))
					if (tabBody.TabPages[tabPageInfo.Title].Selected == true)
						return true;
					else
						return false;
				return false;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>11375",exp,exp.Message.ToString(),true);
				
			}
			return false;
		}
		#endregion 

		#region isWhiteBoardEnabled()
		public bool isWhiteBoardEnabled()
		{
			//Trace.WriteLine("    white brush     " + tabPageInfo.Title); 
			//			if (tabBody.TabPages[tabPageInfo.Title].Selected == true)
			//				return true;
			//			else
			//				return false;
			if(tabBody.TabPages.Contains(tabPageInfo))
				return true;     
			else
				return false;
		}
		#endregion 

		/*public bool isDocumentSharingEnabled()
		{
			if(tabBody.TabPages["Document Sharing"]!=null)
				return true;     
			else
				return false;
			
		}*/
	

		#region CloseAllExtraPages()
		public void CloseAllExtraPages()
		{
			try
			{	
				//int totalPages = tabBody.TabPages.Count ;
				for(int i=0; i < tabBody.TabPages.Count;)
				{
					if(tabBody.TabPages[i].Control.GetType().Name == "ApplicationSharing")
					{					
						ApplicationSharing app = (ApplicationSharing)tabBody.TabPages[i].Control;
						if(app.controlOwnerId == network.profile.ClientId)
						{											
							app.QuickAppShareExit();/*for releasing shared window handle and making its parent '0'*/
							myAppShareStarted= false;
							tabBody.TabPages.Remove(tabBody.SelectedTab);
							ApplicationSharingTabPagesArray.Remove(tabBody.SelectedTab);
							app.TerminateEx();										
						}
						else
						{
							myAppShareStarted= false;
							tabBody.TabPages.Remove(tabBody.SelectedTab);
							ApplicationSharingTabPagesArray.Remove(tabBody.SelectedTab);
							app.TerminateAttendeeSide();							
						}						
					}
					else
						i++;
				}
				ApplicationSharingTabPagesArray.Clear();
				/*used for the termination of the document sharing*/
				try
				{
					for(int i = 0 ; i < arraydocumentSharing.Count ; i++)
					{	
					
						Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
						documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
						/////////////////////////////////////////////
						/*if(control.IsAttendee)
						{
							return;
						}
						else*/
									
						if(!control.IsAttendee)
						{
							DocumentSharing msg = new DocumentSharing();
							msg.senderProfile = this.network.profile;
							msg.SenderID = this.network.profile.ClientId;
							msg.DocumentSharingSessionId = control.sessionID;
							msg.sessionID = control.sessionID;
							msg.bEnabled = false;
							msg.ConferenceID = network.profile.ConferenceID ;
							network.SendLoadPacket(msg);
						}
						/////////////////////////////////////////////
						control.ClearThumbnailsFromPanel();
						control.threadFunctionStop();
						control.Terminate();					
					}
					lock(arraydocumentSharing)
					{
						arraydocumentSharing.Clear();
					}

				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3839",ex,null,false);
				}
				try
				{
					for(int i = arrayWebControl.Count-1 ; i >= 0 ; i--)
					{
						Crownwood.Magic.Controls.TabPage tabBrowser = (Crownwood.Magic.Controls.TabPage)arrayWebControl[i];
						browser browserControl = (browser)tabBrowser.Control;
						//if(!browserControl.IsAttendee)
							if(this.IfIamthePresenter())
							browserControl.SendCloseMessage();
							browserControl.Terminate();                
						//browserControl.CleanandClear();
						
					}
					lock(arrayWebControl)
					{
						arrayWebControl.Clear();
					}
				}
				catch(Exception ex)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 3925",ex,null,false);
				}
				for(int i=0; i < tabBody.TabPages.Count ;i++)
				{
					if(tabBody.TabPages[i].Title != "Welcome")			
					{						
						tabBody.TabPages.Remove(tabBody.TabPages[i]);
						--i;
					}
				}
			}
			catch(Exception exp)
			{				
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>11417",exp,exp.Message.ToString(),true);
				//MessageBox.Show(ex.Message);
			}
		}
		
		#endregion 




		
		private void btnNext_Click(object sender, System.EventArgs e)
		{//Work idependent on all Document Sharing Modules(PPT Slide Show)		
			try
			{
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
				{
					documentSharingControl doc = (documentSharingControl) tabBody.SelectedTab.Control;
					if(doc.m_DocumentMessage.DocumentType==DocumentMessageType.typePresentation)
					{
						doc.btnNext_Click(null,null);
					}
				}	
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>11440",exp,exp.Message.ToString(),true);
				
			}
		}

		private void btnPrev_Click(object sender, System.EventArgs e)
		{//Work idependent on all Document Sharing Modules(PPT Slide Show)
			try
			{		
				if(tabBody.SelectedTab.Control.GetType().Equals(typeof(documentSharingControl)))
				{
					documentSharingControl doc = (documentSharingControl) tabBody.SelectedTab.Control;
					if(doc.m_DocumentMessage.DocumentType==DocumentMessageType.typePresentation)
					{
						doc.btnPrev_Click(null,null);
					}
				}	
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>11460",exp,exp.Message.ToString(),true);
			}
			
		}

		/// <summary>
		/// Added by Zaeem
		// It first closes all the tabs on its own side
		// Then send the message across the network so that all other can close their own tabs.
	 
		/// </summary>
		



		public void HostCloseAllTabs()
		{

			try
			{
				// closes all its own tabs 			
				ClientCloseAllTabs();
				// Close the Poll Windows
							
				this.CloseMsg();	
				//define a new Msg to ClearAllTabs, defined in MessageObjectType of KnownObjects/Messages.cs
				ClearAllTabs msg=new WebMeeting.Common.ClearAllTabs(0);
				// MsgClearAllTabs is defined in Knownobject.
				msg.SenderID = NetworkManager.thisInstance.profile.ClientId;
				msg.senderProfile = NetworkManager.thisInstance.profile;					
				NetworkManager.thisInstance.SendLoadPacket(msg);
			}
			catch(Exception exp)
			{
				// This block should be elimated as its useless.
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage(" HostCloseAllTabs() Method in Client UI",exp,null,false);
			}

		}

		
		public void HostPollWndClose()
		{
			try
			{
				if(this.frm_PollingGlobalPresenter!=null)
				{
					this.frm_PollingGlobalPresenter.Close();
					if(arrayPolling.Count>0)
					arrayPolling.RemoveAt(0);

				}
			}
			catch(Exception exp)
			{}
		
		}




		public void HostPollHide()
		{
			try
			{
				if(this.frm_PollingGlobalPresenter!=null)
				{
					this.frm_PollingGlobalPresenter.Hide();
					if(arrayPolling.Count>0)
						arrayPolling.RemoveAt(0);

				}
			}
			catch(Exception exp)
			{}
		
		}


		/// <summary>
		/// Added by Zaeem
		//  CloseMsg defined by Zaeem.
		// This message is to be send fronm the presenter to the Attendees
		// It shd only be for Attendee windows of summary
		// There can be 2 Attendee windows at a time.
		// They will be closed when this message will be send.
	 	/// </Poll>
		

		public void CloseMsg()
		{
			try
			{
				WebMeeting.Common.CloseMsg msg=new WebMeeting.Common.CloseMsg(0,"PollClose");
				msg.SenderID = NetworkManager.thisInstance.profile.ClientId;
				msg.senderProfile = NetworkManager.thisInstance.profile;					
				msg.type="PollClose";
				NetworkManager.thisInstance.SendLoadPacket(msg);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("CloseMsg() Method in Client UI",exp,null,false);
			}
			
		}
		


		
		public void ClientCloseAllTabs()
		{
			this.HostPollWndClose();
			// For tabs closing
			BeginInvoke(this.del_closeAll);	
		}
		
		/// <summary>
		/// This Method is made by Zaeem 
		/// It Hides the QA forms on different event like connection reestablish
		/// 
		/// </summary>
		public void HideQA_forms()
		{
		
			try
			{

				frmQAA.Hide();
				frmQAP.Hide();

			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs =>call_CloseAlltabs Exception in form hide for QA==>line==> 13316",exp,null,false);				
			}
				
		}

		public void Call_CloseAllTabs()
		{
			try
			{
				this.HideQA_forms();
				
				
				//while(tabBody.TabPages.Count>1)
				//Make 5 tries to close all tabs
				for(int no_tries=0 ;no_tries<=2; no_tries++)
				{
					if(tabBody.TabPages.Count==1)
					{
						break;
					}
					else if(tabBody.TabPages.Count>1)
					{
						for(int i=0;i<tabBody.TabPages.Count;i++)
						{
							//MessageBox.Show(tabBody.TabPages[i].Title.ToString());

					
							if(!tabBody.TabPages[i].Title.Equals("Welcome"))
							{

								# region Whiteboard Close
								/////////////                *****************  ///////////////
								if( tabBody.TabPages[i].Title == "WhiteBoard" || tabBody.TabPages[i].Title == "WhiteBoard - Disabled")
								{
									try
									{
										whiteBoard.btn_whtLogg.Visible=false;
										tabBody.TabPages.Remove(tabBody.TabPages[i]);
										//if(tabBody.TabPages.Count>1)
										//	//true);
										//else
										//	//false);
										if(no_tries<1)
										{
											if(this.IfIamthePresenter())
											{
												WhiteboardMessage msg=new WhiteboardMessage();
												msg.MessageType=(ushort)MessageObjectType.MsgHideWhiteBoard;
												msg.ConferenceID = network.profile.ConferenceID;
												msg.SenderID = network.profile.ClientId;
												network.SendLoadPacket(msg);
											}
										}
									}
									catch(Exception exp)
						
									{
										//	MessageBox.Show("==>whiteboard TAB EXCEPTION ");
										WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 5537",exp,null,false);
									}
						
					
								}
									/////////////                *****************  ///////////////
					
									#endregion 

								# region Start pasting

								else if((tabBody.TabPages[i] != tabPageInfo) && (tabBody.TabPages[i] != startPage))
								{		
									System.Type tType=tabBody.TabPages[i].GetType();					
									if(tabBody.TabPages[i].Control.GetType().Name == "ApplicationSharing")
									{			
										
										
										this.call(i);
										//Invoke(this.del_closeAppShare,new object[]{i});	
								
									}
						
						
									else if(tabBody.SelectedTab == tabPageDesktopSharing)
									{
										try
										{
											DesktopSharingControlMessage message = new DesktopSharingControlMessage(false,
												network.profile.ClientId,desktopSharedProfile.ClientId);							
											message.Start=false;		
											message.Status = -1;	
											message.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
											network.SendLoadPacket(message);		
							

										}
										catch(Exception exp)
										{
											
											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs Call_CloseAltabs 14482",exp,null,false);
										}

										try
										{
											TerminateDesktopSharing();
										}
										catch(Exception exp)
										{
											
											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 12256",exp,null,false);
							
										}					
										this.desktopSharedProfile=null;

										System.GC.Collect();
										System.GC.WaitForPendingFinalizers();
								
									}								
									else
									{
										try
										{
											System.Type a=tabBody.TabPages[i].Control.GetType();
						
											if(a.Equals(typeof(browser))) //dont' close tab if its attendee and its a browser control
											{
												try
												{
													this.whiteBoard.DrawingTool = WebMeetingDrawingBoard.WhiteboardToolCode.None;	//per2			
													Thread.Sleep(2000);
													
													browser browserControl = (browser)tabBody.TabPages[i].Control;
													//if(!browserControl.IsAttendee)
													//if(this.IfIamthePresenter())

													//Zaeem View 
													// The Commneted line of code was a stupid code 
													Invoke(DeleteSharingWindow,new object[]{tabBody.TabPages[i]});
													//tabBody.TabPages.Remove(tabBody.TabPages[i]);
													//tabBody.TabPages.RemoveAt(i);
												
													if(no_tries<1)
													{
														browserControl.Terminate();
													
														//browserControl.CleanandClear();
													
													
														// Zaeem's addition 
														// This was a troublesome block so i remove that Remove method and replaced it with 
														//"RemoveAt" method 
														//arrayWebControl.Remove(browserControl);								
														for(int j=0;j<arrayWebControl.Count;j++)
														{
															if(browserControl.sessionID==((browser)	((Crownwood.Magic.Controls.TabPage)	arrayWebControl[j] ).Control).sessionID)
															{
																//arrayWebControl.Remove(browserControl);								
																arrayWebControl.RemoveAt(j);
															}
															
																
														}
													}
//													arrayWebControl.Clear();
												}
												catch(Exception exp)
												{
												WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs Call_CloseAllTabsline==> 14536",exp,null,false);
												
												}
											}
											else if(tabBody.TabPages[i].Control.GetType().Equals(typeof(documentSharingControl)))
											{
												documentSharingControl doc = (documentSharingControl) tabBody.TabPages[i].Control;
												//////////////////////////////////////// Document sharing close button 
												if(no_tries<1)
												{
													DocumentSharing msg = new DocumentSharing();
													msg.senderProfile = this.network.profile;
													msg.SenderID = this.network.profile.ClientId;
													msg.DocumentSharingSessionId = doc.sessionID;
													msg.sessionID = doc.sessionID;
													msg.bEnabled = false;
													msg.ConferenceID = network.profile.ConferenceID ;
													network.SendLoadPacket(msg);
												}
									
												////////////////////////////////////
									
												for(int ii = 0 ; ii < arraydocumentSharing.Count; ii++)
												{
													WebMeeting.Client.documentSharingControl _control = (documentSharingControl)((Crownwood.Magic.Controls.TabPage)arraydocumentSharing[ii]).Control;
													if(_control == 	tabBody.TabPages[i].Control)
													{
														arraydocumentSharing.RemoveAt(ii);
														_control.ClearThumbnailsFromPanel();
														_control.threadFunctionStop();
													}
												}

												////////////////////////////////////////
												tabBody.TabPages.Remove(tabBody.TabPages[i]);
											
											}







											else if(tabBody.TabPages[i].Control.GetType().Equals(typeof(ApplicationSharing)))
											{
										
												ApplicationSharing app = (ApplicationSharing)tabBody.TabPages[i].Control;
										
												myAppShareStarted= false;
												tabBody.TabPages.Remove(tabBody.TabPages[i]);
												ApplicationSharingTabPagesArray.Remove(tabBody.TabPages[i]);
												app.TerminateEx();
										
											}
											else
								
											{
										
												BeginInvoke(this.del_closeGeneric,new object[]{i});	
												//tabBody.TabPages.Remove(tabBody.TabPages[i]);
											}

										}
										catch(Exception exp)
										{
											//	MessageBox.Show("===> 12430"+exp.Message.ToString());
											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> 12441",exp,null,false);
							
										}
									}
								}
								//MessageBox.Show("==> Before FlushMemory");
								FlushMemory(); //per2
								///MessageBox.Show("==> After FlushMemory");

								#endregion 


							}
						}

					}//return true;
		
				}
			}
			catch(Exception exp)
			{
				//MessageBox.Show("==>Kno by zaeem");
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs =>call_CloseAlltabs ==>line==> 1253",exp,null,false);
			}
			
		}


		
		
		public void call(int i)
		{
	

			try
			{
				ApplicationSharing app = (ApplicationSharing)tabBody.TabPages[i].Control;

				//Added by Zaeem
				app.btnShare_Click(null,null);
				//////////////////////////
				//app.StopRefreshListTimer();						
				//app.TerminateEx();
				app.QuickAppShareExit();/*for releasing shared window handle and making its parent '0'*/
							
				myAppShareStarted= false;
				tabBody.TabPages.Remove(tabBody.TabPages[i]);
				ApplicationSharingTabPagesArray.Remove(i);
				
			}
			catch(Exception exp)
			{
				//MessageBox.Show("==>"+exp.Message.ToString());
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs =>call ==>line==> 12554",exp,null,false);

			}
			
		}


		
		public void CallGeneric(int i)
		{
			try
			{
				tabBody.TabPages.Remove(tabBody.TabPages[i]);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs==>calgeneric() line==> 12569",exp,null,false);

			}
		
		}

		

		/// <summary>
		/// CAUTION 
		/// This method Syncronyze the drawing tools and has to be called by the host only
		/// //OR ELSE THE STATUS OF ANNOTATION TOOOL TFOR THE HOST WILL NOT BE UPGRADED 
		/// 
		/// 
		/// </summary>
		public void SyncronizeDrawingTools()
		{
			
			try
			{
				ClientProfile profile1;
			
				//  This loop count the no of Users and add in the above defined variables
				//******************************************************//
				# region for Loop
				// Zaeem View 
				// we are already in the clientUi so why this ClientUI.getInstance() is there ?

				for(int i=0;i<Client.ClientUI.getInstance().arrayParticipents.Count;i++)
				{
			
					profile1=(ClientProfile)arrayParticipents[i];
					if(profile1.clientType.ToString().Equals("ClientAttendee"))
					{
						if(profile1.clientAccess.annotationRights)
						{
							profile1.clientAccess.annotationRights=false;
							
						}
											
					}
							
					// If Sthis method is called by presenter then 
					// "if(profile1.ClientId==network.profile.ClientId)"
					// the quoted check shdn't be here as if its called by presenter 
                    // the above condition wil never met and anootation rights are not send to 
					//the host ==> it means his status will not be updated.

					// There shd be another check to improve this thing in host block 
					// which shd run if its called by the presenter
                    // SO that the status of the host will be upgraded properly.

					else if((profile1.clientType.ToString().Equals("ClientHost")))
					{
						// My own Id ==> i am the host
						if(profile1.ClientId==network.profile.ClientId)
						{
							//if server's buttons are disabled ==> means have no rights
						

							# region Normal Version
							//if(btn_Whiteboard.Enabled==false && btn_Docshare.Enabled==false)
							# endregion 		
							
								
							# region Stripped Down Version
							//if(btn_Whiteboard.Enabled==false )//&& btn_Docshare.Enabled==false)
							# endregion 		
							
							//Removing Enable/Disable annoatation tools
//							{
//								profile1.clientAccess.annotationRights=false;
//								this.enableWhiteBoardTools(false);
//					
//							}
					
						}	
				
					}

					 
					SendClientProfileUpdateMessage(profile1);	
				}// end For Loof
				# endregion 
				//******************************************************//
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs==>SyncronizeDrawingTools() line==> 13023",exp,null,false);
			}



		}

		
		
		
			
	}
}

#region Audio Polling TabPages derived classes
namespace WebMeeting.Client
{
	
	public class PollingTabPage: Crownwood.Magic.Controls.TabPage
	{
		public WebMeeting.Polling.Polling PollingControl;// = new WebMeeting.Polling.Polling();
		public WebMeeting.Polling.PollResult pollResult ;//= new WebMeeting.Polling.PollResult();

		public WebMeeting.Polling.PollingQuestionDetails askQuestionWindow;

		public ClientProfile clientProfile;		
		public PollingTabPage()
		{			
			try
			{
				PollingControl= new WebMeeting.Polling.Polling();
				pollResult = new WebMeeting.Polling.PollResult();

				PollingControl.pqd = askQuestionWindow;
				PollingControl.pr = pollResult;		
				PollingControl.pr.polling = PollingControl;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>11501",exp,exp.Message.ToString(),true);
			}
		}
	}

	
}

#endregion


