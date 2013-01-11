
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using WebMeeting.Common;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using IMWindow;
using Utility.NiceMenu;
using BinaryReaderWriter;
using Win32;



namespace WebMeeting
{
	
	
	public class chatControl : System.Windows.Forms.UserControl
	{
		
		public WebMeeting.Client.PrintableRichTextBox txtBoxViewChat;
		public Khendys.Controls.ExRichTextBox txtBoxPreviewChat;
		private System.Windows.Forms.Button btnBold;
		private System.Windows.Forms.Button btnItalics;
		private System.Windows.Forms.ContextMenu cmenu_Emoticons;
		private string localMessageHeader;
		
		private IMChatMessage message;
		public ArrayList messagePool;
		private Thread processingThread;
		private bool m_bIsActive;
		private bool m_bBold;
		private bool m_bItalics;
		private bool m_bUnderLine;
		private System.Windows.Forms.Button btnUnderLine;
		private ArrayList emoticonArray;
		string[] length;
		private ArrayList previousChar;
		private ToolTip tooltip;
		public int nRecipientID;
		private Font titleFont;
		public int count=0;

		private string filename;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Splitter splitter1;
		private NSPAControls.NSButton btnEmoticon;
		private NSPAControls.NSButton btnFont;
		private NSPAControls.NSButton btnSend; // by kamran
		public int test;
		public System.Windows.Forms.ComboBox testCombo;
		public Crownwood.Magic.Controls.TabPage thisWindowTabPage;
		public System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ImageList imageList1;
		private System.ComponentModel.IContainer components;

		public string UserName
		{
			set
			{
				localMessageHeader= value;
			}
			get
			{
				return localMessageHeader;
			}
		}

		
		
		# region LoadEmoticonFile() 
		public void LoadEmoticonFile() 
		{
			try
			{
				DirectoryInfo di = new DirectoryInfo(Application.StartupPath + "\\Emoticons");
				if(! di.Exists)
				{
					//ICSharpZip.ZipDirectory( Application.StartupPath + "\\Emoticons",  Application.StartupPath + "\\Emoticons.zip" );
					ICSharpZip.UnzipZip( Application.StartupPath + "\\Emoticons.zip",  Application.StartupPath + "\\Emoticons" );
				}
				di = null;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs line==>92",exp,null,false);
			}
			try
			{
				emoticonArray.Clear();
				FileStream fs=null;
				string temp;
				temp=Application.StartupPath;
				temp+="\\Emoticons\\emoticons.txt";
				fs = new FileStream(temp, FileMode.Open, FileAccess.Read);
				StreamReader sr = new StreamReader(fs);			
				string nextLine = sr.ReadLine();
				while(nextLine!=null)
				{
					try
					{
						string[] tokens=nextLine.Split(new char[]{'\t'});
						EmoticonData ed=new EmoticonData();
						ed.shortcut=tokens[0].ToUpper();
						ed.img=new Bitmap(Application.StartupPath + @"\Emoticons\" + tokens[1]);
						emoticonArray.Add(ed);
					}
					catch(Exception exp)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==>public void LoadEmoticonFile() ==> 117",exp,null,false);			
					}
					nextLine=sr.ReadLine();
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==>line==> 124",exp,null,false);
			}			
		}

		# endregion 


		# region chatControl() ==>constructor
		public DelegateshowChatAlert objDelShowAlert;

		public chatControl()
		{
			
			try
			{
				//Setting up the Input/Output RTF Text Box Controls
				// This call is required by the Windows.Forms Form Designer.
				InitializeComponent();			
				filename="";
				messagePool= new ArrayList();
				previousChar = new ArrayList();
				emoticonArray = new ArrayList();
				processingThread=new Thread(new ThreadStart(consumeFunction));
				processingThread.Name = "chatControl Thread: consumeFunction()";
				//processingThread.Priority=ThreadPriority.Highest;
				m_bIsActive=true;
				processingThread.Start();
				objDelShowAlert=new DelegateshowChatAlert(Client.ClientUI.getInstance().showMessageAlert);


				// Self made exception by Zaeem
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs Self made Exception 145 Thread was started ",new Exception(),null,false);				

				
				m_bBold=false;
				m_bItalics=false;
				m_bUnderLine=false;
				length=new string[10];
				//LoadEmoticonFile();
				tooltip=new ToolTip();
				titleFont= new Font("Franklin Gothic Medium",10,System.Drawing.FontStyle.Bold);
				this.BackColor=WebMeeting.Client.Info.getInstance().ChatBackColor;
				this.cmenu_Emoticons = new System.Windows.Forms.ContextMenu();

				// Create Emoticon DropDownMenu
//				EmoticonMenuItem _menuItem;
//				int _count = 0;
//				foreach(EmoticonData _emoticon in this.emoticonArray) 
//				{
//					_menuItem = new EmoticonMenuItem(_emoticon.img);
//					_menuItem.Click += new EventHandler(cmenu_Emoticons_Click);
//					_menuItem.Select+=new EventHandler(_menuItem_Select);
//				
//					if (_count % 3 == 0)
//						_menuItem.BarBreak = true;
//
//					cmenu_Emoticons.MenuItems.Add(_menuItem);
//					++_count;
//					if(_count==42)
//						break;
//				}

				nRecipientID = -1;
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==>chatControl() ==> 178",exp,null,false);				
			}
		}

		# endregion 

		# region cmenu_Emoticons_Click
		private void cmenu_Emoticons_Click(object _sender, EventArgs _args) 
		{
			EmoticonMenuItem _item = (EmoticonMenuItem) _sender;
			try 
			{
				this.txtBoxPreviewChat.InsertImage(_item.Image);
				this.txtBoxPreviewChat.Focus();
			}
			catch (Exception exp) 
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==>cmenu_Emoticons_Click( ==> 197",exp,"Rtf Image Insert Error\n\n"+exp.Message.ToString(),true);				
			}
		}
		# endregion 


		public void Terminate()
		{
			m_bIsActive=false;
		}


		public void consumeFunction()
		{
			try
			{
				while(m_bIsActive)
				{
					while(messagePool.Count>0)
					{
						try
						{
							try
							{
								if(Client.ClientUI.getInstance().chatTabControl.SelectedTab != thisWindowTabPage)
								{						
									Win32.USER32.SendMessage(this.Handle,100009,new IntPtr(),new IntPtr());
								}
							}
							catch(Exception ex)
							{
								WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Exception occur at message processing loop ",ex,null,false);
							}
							message=(IMChatMessage)messagePool[0];
							ProcessMessage(message);
							messagePool.RemoveAt(0); 
						}
						catch(Exception ex)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Exception occur at message processing loop ",ex,null,false);
						}
					}
					Thread.Sleep(100);
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==>chatControl() ==> 232",exp,null,false);				
			
			}
		}
		public bool isNewMessage = false;
		public bool isCheckMessage=false;
		public System.Drawing.Font myfont= new System.Drawing.Font("Verdana",10,System.Drawing.FontStyle.Bold);

		# region ProcessMessage(IMChatMessage receivedMessage)
		private void ProcessMessage(IMChatMessage receivedMessage)
		{
			try
			{
				//TODO: Change the default font of chat control.
				//this.txtBoxViewChat.TextColor=Khendys.Controls.RtfColor.Green;//Gray;
				
				//this.txtBoxViewChat.Font=titleFont;   made by junaid
				//this.txtBoxViewChat.Font
				//////////
				//if(Client.ClientUI.getInstance().chatTabControl.SelectedTab.Title != "Chat" )
				if(txtBoxViewChat==null)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("private void ProcessMessage(IMChatMessage receivedMessage)txtBoxViewChat is null",new Exception(),null,false);										
				}
				if( Client.ClientUI.getInstance().splitter1Closed )
				{
					if(receivedMessage.sender == null)
					{
						
						//Trace.WriteLine("check is 2");
						//System.Windows.Forms.MessageBox.Show(receivedMessage.IMMessage);
						receivedMessage.IMMessage=receivedMessage.IMMessage.Substring(0,receivedMessage.IMMessage.Length-3);

						//						if(receivedMessage.IMMessage.Substring(receivedMessage.IMMessage.Length-2,1)=="\n")
						//						{
						//							System.Windows.Forms.MessageBox.Show ("check 1 \n");
						//						}
						//						if(receivedMessage.IMMessage.Substring(receivedMessage.IMMessage.Length-2,1)=="\r")
						//						{
						//							System.Windows.Forms.MessageBox.Show("check 2 \r");
						//						}
						//txtBoxViewChat.AppendText(receivedMessage.sender + " :\n");
						txtBoxViewChat.AppendRtf(receivedMessage.IMMessage); /// original sat:29.10
						
					}
					else
					{

						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Self made (Selfchat)exception before 1 else",new Exception(),null,false);										
						
						//Trace.WriteLine("check is 1");
						txtBoxViewChat.AppendTextAsRtf(receivedMessage.sender + " :\n",myfont);	
						//txtBoxViewChat.AppendText(receivedMessage.sender + " :\n");
						txtBoxViewChat.AppendRtf(receivedMessage.IMMessage); /// original sat:29.10
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Self made (Selfchat)exception after 1 else ",new Exception(),null,false);				
					}
									
					//					if(receivedMessage.sender != null)
					//						txtBoxViewChat.AppendText(receivedMessage.sender + " :\n");
					//					txtBoxViewChat.AppendRtf(receivedMessage.IMMessage); /// original sat:29.10
					if(!Client.ClientUI.getInstance().isChatAlreadyShown)
					{						
						//fire delegate to show message alert
						//in this way we are able to stop thread blockage
						//if we show this form directly from this thread then it 
						//blocks the current running thread
						Invoke(this.objDelShowAlert);
						//Client.ClientUI.getInstance().showMessageAlert();
					}
					
				}
				else
				{
					////////////////////	
					//	if(receivedMessage.sender.Length > 0)
					if(receivedMessage.sender == null)
					{
						receivedMessage.IMMessage=receivedMessage.IMMessage.Substring(0,receivedMessage.IMMessage.Length-8);
						///System.Windows.Forms.MessageBox.Show(receivedMessage.IMMessage);
						//						if(receivedMessage.IMMessage.Substring(receivedMessage.IMMessage.Length-3,1)=="\n")
						//						{
						//							
						//						}
						//						if(receivedMessage.IMMessage.Substring(receivedMessage.IMMessage.Length-2,1)=="\r")
						//						{
						//							
						//						}
						//txtBoxViewChat.AppendText(receivedMessage.sender + " :\n");
						//Trace.WriteLine("check is 3");
						if(txtBoxViewChat.Text.Trim().ToString()=="")
							txtBoxViewChat.AppendRtf(receivedMessage.IMMessage); /// original sat:29.10
					}
					else
					{
						
				
						//System.Windows.Forms.MessageBox.Show(myfont.Bold.ToString());	
 
						///		Trace.WriteLine("check is 4");
						//txtBoxViewChat.AppendTextAsRtf(receivedMessage.sender  +" :");    
						//Trace.WriteLine("rtf text"  + txtBoxViewChat.Rtf.ToString() );
						if(txtBoxViewChat.Text=="")
							isCheckMessage=false;
						else
							isCheckMessage=true;
						//txtBoxViewChat.AppendText(receivedMessage.sender + " :\n");
						//		Trace.WriteLine("default value : " +txtBoxViewChat.Font.Bold.ToString() );
						//		Trace.WriteLine("header information"  +txtBoxViewChat.Rtf.ToString());
						//System.Windows.Forms.MessageBox.Show(txtBoxViewChat.Rtf.ToString());							
						//		Trace.WriteLine("1 rtf text"  + receivedMessage.IMMessage.ToString());
						//System.Windows.Forms.MessageBox.Show(txtBoxViewChat.Rtf.ToString());
						
						txtBoxViewChat.AppendTextAsRtf(receivedMessage.sender + " :\n",myfont);	
						//str = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Courier New;}}\viewkind4\uc1\pard\f0\fs20\\b0\par}";
						
						
						///		System.Windows.Forms.MessageBox.Show(txtBoxViewChat.Rtf.ToString());
						//		System.Windows.Forms.MessageBox.Show(receivedMessage.IMMessage.ToString());			
						txtBoxViewChat.AppendRtf(receivedMessage.IMMessage); /// original sat:29.10
						//txtBoxViewChat.AppendRtf(@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Courier New;}}\viewkind4\uc1\pard\f0\fs20\b0}"); 
						///		Trace.WriteLine("rtf text"  + txtBoxViewChat.Rtf.ToString());
						//		System.Windows.Forms.MessageBox.Show(txtBoxViewChat.Rtf.ToString());						
						//txtBoxViewChat.Ap
						
					}
				}
				//else
				//	txtBoxViewChat.AppendText(receivedMessage.IMMessage); // appendRTF
	
				// Scroll to bottom so newly added text is seen.
				txtBoxViewChat.Select(txtBoxViewChat.TextLength,0) ;
				txtBoxViewChat.ScrollToCaret();

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==>cmenu_Emoticons_Click( ==> 346",exp,"Chat box error : "+exp.Message.ToString(),true);				
			
			}
		}

		#endregion 

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// 
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(chatControl));
			this.txtBoxViewChat = new WebMeeting.Client.PrintableRichTextBox();
			this.txtBoxPreviewChat = new Khendys.Controls.ExRichTextBox();
			this.btnBold = new System.Windows.Forms.Button();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.btnItalics = new System.Windows.Forms.Button();
			this.btnUnderLine = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnFont = new NSPAControls.NSButton();
			this.btnEmoticon = new NSPAControls.NSButton();
			this.panel4 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.testCombo = new System.Windows.Forms.ComboBox();
			this.btnSend = new NSPAControls.NSButton();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtBoxViewChat
			// 
			this.txtBoxViewChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtBoxViewChat.Font = new System.Drawing.Font("Courier New", 9.75F);
			this.txtBoxViewChat.HideSelection = false;
			this.txtBoxViewChat.HiglightColor = Khendys.Controls.RtfColor.White;
			this.txtBoxViewChat.Location = new System.Drawing.Point(0, 0);
			this.txtBoxViewChat.Name = "txtBoxViewChat";
			this.txtBoxViewChat.ReadOnly = true;
			this.txtBoxViewChat.Size = new System.Drawing.Size(256, 184);
			this.txtBoxViewChat.TabIndex = 3;
			this.txtBoxViewChat.Text = "";
			this.txtBoxViewChat.TextColor = Khendys.Controls.RtfColor.Black;
			this.txtBoxViewChat.TextChanged += new System.EventHandler(this.txtBoxViewChat_TextChanged);
			this.txtBoxViewChat.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBoxViewChat_KeyUp);
			// 
			// txtBoxPreviewChat
			// 
			this.txtBoxPreviewChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtBoxPreviewChat.Font = new System.Drawing.Font("Courier New", 9.75F);
			this.txtBoxPreviewChat.HiglightColor = Khendys.Controls.RtfColor.White;
			this.txtBoxPreviewChat.Location = new System.Drawing.Point(0, 35);
			this.txtBoxPreviewChat.Name = "txtBoxPreviewChat";
			this.txtBoxPreviewChat.Size = new System.Drawing.Size(256, 45);
			this.txtBoxPreviewChat.TabIndex = 4;
			this.txtBoxPreviewChat.Text = "";
			this.txtBoxPreviewChat.TextColor = Khendys.Controls.RtfColor.Black;
			this.txtBoxPreviewChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBoxPreviewChat_KeyDown);
			this.txtBoxPreviewChat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxPreviewChat_KeyPress);
			this.txtBoxPreviewChat.TextChanged += new System.EventHandler(this.txtBoxPreviewChat_TextChanged);
			// 
			// btnBold
			// 
			this.btnBold.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBold.BackgroundImage")));
			this.btnBold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnBold.ImageList = this.imageList1;
			this.btnBold.Location = new System.Drawing.Point(0, -1);
			this.btnBold.Name = "btnBold";
			this.btnBold.Size = new System.Drawing.Size(22, 23);
			this.btnBold.TabIndex = 0;
			this.btnBold.Click += new System.EventHandler(this.btnBold_Click);
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(23, 23);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// btnItalics
			// 
			this.btnItalics.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnItalics.BackgroundImage")));
			this.btnItalics.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnItalics.ImageList = this.imageList1;
			this.btnItalics.Location = new System.Drawing.Point(32, -1);
			this.btnItalics.Name = "btnItalics";
			this.btnItalics.Size = new System.Drawing.Size(22, 23);
			this.btnItalics.TabIndex = 1;
			this.btnItalics.Click += new System.EventHandler(this.btnItalics_Click);
			// 
			// btnUnderLine
			// 
			this.btnUnderLine.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUnderLine.BackgroundImage")));
			this.btnUnderLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnUnderLine.ImageList = this.imageList1;
			this.btnUnderLine.Location = new System.Drawing.Point(64, -1);
			this.btnUnderLine.Name = "btnUnderLine";
			this.btnUnderLine.Size = new System.Drawing.Size(22, 23);
			this.btnUnderLine.TabIndex = 2;
			this.btnUnderLine.Click += new System.EventHandler(this.btnUnderline_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.splitter1);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.txtBoxViewChat);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(256, 296);
			this.panel1.TabIndex = 11;
			// 
			// splitter1
			// 
			this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Enabled = false;
			this.splitter1.Location = new System.Drawing.Point(0, 179);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(256, 5);
			this.splitter1.TabIndex = 12;
			this.splitter1.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.panel2.Controls.Add(this.btnFont);
			this.panel2.Controls.Add(this.btnEmoticon);
			this.panel2.Controls.Add(this.panel4);
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Controls.Add(this.txtBoxPreviewChat);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 184);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(256, 112);
			this.panel2.TabIndex = 11;
			this.panel2.Resize += new System.EventHandler(this.panel2_Resize);
			// 
			// btnFont
			// 
			this.btnFont.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnFont.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnFont.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnFont.HottrackImage")));
			this.btnFont.Location = new System.Drawing.Point(8, 2);
			this.btnFont.Name = "btnFont";
			this.btnFont.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnFont.NormalImage")));
			this.btnFont.OnlyShowBitmap = true;
			this.btnFont.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnFont.PressedImage")));
			this.btnFont.Size = new System.Drawing.Size(58, 28);
			this.btnFont.Text = "nsButton1";
			this.btnFont.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnFont.ToolTip = null;
			this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
			// 
			// btnEmoticon
			// 
			this.btnEmoticon.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnEmoticon.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnEmoticon.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnEmoticon.HottrackImage")));
			this.btnEmoticon.Location = new System.Drawing.Point(8, 0);
			this.btnEmoticon.Name = "btnEmoticon";
			this.btnEmoticon.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnEmoticon.NormalImage")));
			this.btnEmoticon.OnlyShowBitmap = true;
			this.btnEmoticon.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnEmoticon.PressedImage")));
			this.btnEmoticon.Size = new System.Drawing.Size(57, 29);
			this.btnEmoticon.Text = "nsButton1";
			this.btnEmoticon.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnEmoticon.ToolTip = null;
			this.btnEmoticon.Visible = false;
			this.btnEmoticon.Click += new System.EventHandler(this.btnEmoticon_Click);
			// 
			// panel4
			// 
			this.panel4.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.panel4.Controls.Add(this.btnBold);
			this.panel4.Controls.Add(this.btnItalics);
			this.panel4.Controls.Add(this.btnUnderLine);
			this.panel4.Location = new System.Drawing.Point(152, 5);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(96, 27);
			this.panel4.TabIndex = 12;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.testCombo);
			this.panel3.Controls.Add(this.btnSend);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 82);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(256, 30);
			this.panel3.TabIndex = 11;
			this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
			// 
			// testCombo
			// 
			this.testCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.testCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.testCombo.Location = new System.Drawing.Point(0, 2);
			this.testCombo.Name = "testCombo";
			this.testCombo.Size = new System.Drawing.Size(148, 21);
			this.testCombo.TabIndex = 1;
			this.testCombo.Visible = false;
			// 
			// btnSend
			// 
			this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSend.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnSend.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnSend.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnSend.HottrackImage")));
			this.btnSend.Location = new System.Drawing.Point(172, 2);
			this.btnSend.Name = "btnSend";
			this.btnSend.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnSend.NormalImage")));
			this.btnSend.OnlyShowBitmap = true;
			this.btnSend.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnSend.PressedImage")));
			this.btnSend.Size = new System.Drawing.Size(59, 25);
			this.btnSend.Text = "Send";
			this.btnSend.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnSend.ToolTip = null;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			this.btnSend.Enter += new System.EventHandler(this.btnSend_Enter);
			// 
			// timer1
			// 
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// chatControl
			// 
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(212)), ((System.Byte)(208)), ((System.Byte)(200)));
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "chatControl";
			this.Size = new System.Drawing.Size(256, 296);
			this.Load += new System.EventHandler(this.chatControl_Load);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			
			
			switch(m.Msg)
			{
				case 100009:
					timer1.Start();
					
					break;
				default:
					base.WndProc(ref m);                
					break;

			}


		}		
			
		
		private static chatControl controller;

		public static chatControl getInstance()
		{
			if(controller == null)
			{
				controller = new chatControl();
			}//end if 

			return controller;
		}//end of the getInstance method

		# region PrintIt()
		public void PrintIt()
		{
			try
			{
				System.Drawing.Printing.PrintDocument p = new System.Drawing.Printing.PrintDocument();
				p.DocumentName = "Chat";
				p.PrintPage+=new System.Drawing.Printing.PrintPageEventHandler(p_PrintPage);
				p.BeginPrint += new PrintEventHandler(p_BeginPrint);
				p.Print();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==>PrintIt() ==> 661",exp,null,false);				
			}
		}

		# endregion 

		/*	private void btnSend_Click(object sender, System.EventArgs e)
			{			
				if(WebMeeting.Client.NetworkManager.thisInstance.bIsConnectedToServer)
					SendText("");		
			}*/

		# region SendText
		public void SendText(string str)
		{
			try
			{
				if((testCombo.SelectedIndex == 0)|| (testCombo.SelectedIndex == -1)||
					(testCombo.SelectedIndex == 1) ||(testCombo.SelectedIndex == 2))
				{
					WebMeeting.Client.NetworkManager network = WebMeeting.Client.NetworkManager.getInstance();

					IMChatMessage msg=new IMChatMessage();
					msg.senderID = network.profile.ClientId;
	            	
					if(testCombo.SelectedIndex == 1)
						msg.m_ForClientType = ClientType.ClientPresenter;
					else if(testCombo.SelectedIndex == 2)
						msg.m_ForClientType = ClientType.ClientAttendee;						
					if(nRecipientID == -1)			
					{
						//MessageBox.Show("if(nRecipientID == -1)  --------TRUE");
						//Trace.WriteLine("Recipient ID is -1");
						if(testCombo.Visible == false)
						{
							//	Trace.WriteLine("if(testCombo.Visible == false) is true");
							//	Trace.WriteLine("Client.ClientUI.getInstance().sendTo.SelectedIndex : " + Client.ClientUI.getInstance().sendTo.SelectedIndex.ToString() ) ;
							if(Client.ClientUI.getInstance().sendTo.SelectedIndex != -1)
							{
								msg.RecipientId = ((ClientProfile)Client.ClientUI.getInstance().arrayParticipents[Client.ClientUI.getInstance().sendTo.SelectedIndex]).ClientId;
								msg.MessageType = (ushort)MessageObjectType.MsgIMChat;           				
					
							}
						}
					}
					else
					{
						//MessageBox.Show("if(nRecipientID == -1)  --------FALSE");
						//Trace.WriteLine("Else condition is run");	
						msg.RecipientId = nRecipientID;
						msg.MessageType = (ushort)MessageObjectType.MsgIMChat;           				
						//Trace.WriteLine(msg.MessageType.ToString()) ;
					}
				
					
					if(msg.RecipientId  == network.profile.ClientId)
						return;

					msg.sender= network.profile.Name;
					msg.ConferenceID = network.profile.ConferenceID; //conf used in transcript
					
					if(str == "")
					{
						msg.IMMessage=txtBoxPreviewChat.Rtf;
						if(txtBoxPreviewChat.Text.Length==0)
							return;

					}
					else
						msg.IMMessage=str;
						

					if((Client.ClientUI.getInstance().sendTo.SelectedIndex != -1) && (testCombo.Visible == false))
					{				

						IMChatMessage chatMsg = (IMChatMessage)msg;
						
						bool found = false;
						for(int i = 0 ; i < Client.ClientUI.getInstance().chatWindowsArray.Count; i++)
						{
							Client.ClientUI.ClientProfileWrapper CP =(Client.ClientUI.ClientProfileWrapper ) Client.ClientUI.getInstance().chatWindowsArray[i];
							if(CP.nUniqueID== chatMsg.RecipientId) //if found
							{
								//place the message in chat control message pump
								chatControl UserChatCtrl = (chatControl)CP.tabPage.Control;
								UserChatCtrl.messagePool.Add(msg);
								found = true;
								break;
							}																			
						}							

						if(!found)
						{
							// if no chat window found. create a new tab page .. add it to tab control
							Client.ClientUI.getInstance().CreateChildControl(msg);							    									                                    	                                                                   
						}
					}
					else
					{
						messagePool.Add(msg);							
					}							
					this.txtBoxPreviewChat.Font=this.txtBoxPreviewChat.SelectionFont;					
					txtBoxPreviewChat.Clear();
					txtBoxPreviewChat.Focus();					
					//Trace.WriteLine("main chat " + msg.MessageType.ToString());
					network.SendLoadPacket(msg);						
				}
				else
				{										
					int priMsgRecIndex=-1;
//					if(((WebMeeting.Client.NetworkManager.getInstance().profile.clientType== ClientType.ClientPresenter) && (
//						WebMeeting.Client.NetworkManager.getInstance().profile.clientAccess.accessPrivateChat)) ||
//						(WebMeeting.Client.NetworkManager.getInstance().profile.clientType== ClientType.ClientHost))					
					if(((WebMeeting.Client.NetworkManager.getInstance().profile.clientType== ClientType.ClientPresenter) || (WebMeeting.Client.NetworkManager.getInstance().profile.clientType== ClientType.ClientAttendee)) ||
						(WebMeeting.Client.NetworkManager.getInstance().profile.clientType== ClientType.ClientHost))
					{
						//Trace.WriteLine("some user is selected");						
						priMsgRecIndex=getIndexofSelectedChatter(testCombo.SelectedItem.ToString());
						Client.ClientUI.getInstance().listParticipents.Items[priMsgRecIndex].Selected = true;			
						//Trace.WriteLine("User Name " +  Client.ClientUI.getInstance().listParticipents.Items[(testCombo.SelectedIndex - 3)].Text.ToString());
						//Client.ClientUI.getInstance().listParticipents.						
						//Trace.WriteLine("testcombo selected index" +Client.ClientUI.getInstance().listParticipents.Items[0].ToString() + " " + priMsgRecIndex.ToString());											
						//Client.ClientUI.getInstance().listParticipents.Items.i    
						this.txtBoxPreviewChat.Font=this.txtBoxPreviewChat.SelectionFont;					
						Client.ClientUI.getInstance().OpenChatWindow(true);				
						txtBoxPreviewChat.Clear();
						txtBoxPreviewChat.Focus();				
					}

				}
			}

			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==>SendText( ==> 810",exp,exp.ToString()+" SendText",false);				
			}


		}
		# endregion 


		#region getIndexofSelectedChatter(string reciptname)
		private int getIndexofSelectedChatter(string reciptname)
		{
			
			try
			{
				int i=0;
				string stritem="";
				int nLenItem;
				for(i=0;i<Client.ClientUI.getInstance().listParticipents.Items.Count;i++)
				{
					stritem=Client.ClientUI.getInstance().listParticipents.Items[i].ToString();
					//ListViewItem: {Saif}
					nLenItem=stritem.Length;

					stritem=stritem.Substring(15,nLenItem-16);
					if(stritem==reciptname)
						return i;
				}
				return -1;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==> getIndexofSelectedChatter( ==> 840",exp,null,false);				
				return -1;
			}

		}
		# endregion 
		
		string strText  = "";
			
		#region Sys_SendText(int ClientId)
		public void Sys_SendText(int ClientId)
		{
			try
			{
				strText = txtBoxViewChat.Rtf;
				//Trace.WriteLine("strText" + strText.ToString());
				///	Trace.WriteLine("index of string : " + strText.IndexOf(@"\viewkind4\uc1\pard\b\f0\fs20 Saif :\par").ToString());	
				
				if(txtBoxViewChat.Text.Trim().ToString()=="")
					return;
				//Trace.WriteLine("strText first message" + strText.Trim().ToString() + " txtBoxViewChat text property " +txtBoxViewChat.Text.ToString() );
				//	if(strText=="")return;
				WebMeeting.Client.NetworkManager network = WebMeeting.Client.NetworkManager.getInstance();		         	
				
				
				IMChatMessage msg=new IMChatMessage();
				//Trace.WriteLine("before" + msg.MessageType.ToString());

				//msg.MessageType = (ushort)MessageObjectType.MsgIMChat;
				//Trace.WriteLine("after" + msg.MessageType.ToString());
				msg.senderID = network.profile.ClientId;
	            
				
				if(testCombo.SelectedIndex == 1)
					msg.m_ForClientType = ClientType.ClientPresenter;
				else if(testCombo.SelectedIndex == 2)
					msg.m_ForClientType = ClientType.ClientAttendee;

				msg.RecipientId = ClientId;
				//msg.MessageType = (ushort)MessageObjectType.MsgIMChat;           				
				
				if(msg.RecipientId  == network.profile.ClientId)
					return;
				
				//msg.sender= network.profile.Name;
				//network.profile.clientType=ClientType.ClientHost 
				if(strText == "")
				{
					return;
				}
				else
					msg.IMMessage=strText;
					
				/*
								IMChatMessage chatMsg = (IMChatMessage)msg;
				
								for(int i = 0 ; i < Client.ClientUI.getInstance().chatWindowsArray.Count; i++)
								{
									Client.ClientUI.ClientProfileWrapper CP =(Client.ClientUI.ClientProfileWrapper ) Client.ClientUI.getInstance().chatWindowsArray[i];
									if(CP.nUniqueID== chatMsg.RecipientId) //if found
									{
										//place the message in chat control message pump
										chatControl UserChatCtrl = (chatControl)CP.tabPage.Control;
										UserChatCtrl.messagePool.Add(msg);
										break;
									}																			
								}							
			
								this.txtBoxPreviewChat.Font=this.txtBoxPreviewChat.SelectionFont;
								txtBoxPreviewChat.Clear();
								txtBoxPreviewChat.Focus();				
				*/
				//Trace.WriteLine("network.profile.clientType" +network.profile.clientType.ToString()+ " ClientType.ClientHost" +ClientType.ClientHost.ToString());						
				if(network.profile.clientType==ClientType.ClientHost)
					network.SendLoadPacket(msg);						

			}

			
			
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==>Sys_SendText( ==> 922",exp,exp.ToString()+" SendText",true);				
				
			}
		}

		# endregion 

		private void btnBold_Click(object sender, System.EventArgs e)
		{
			m_bBold=!m_bBold;
			if(m_bBold)
			{	
				this.btnBold.FlatStyle=FlatStyle.Standard;												
				this.btnBold.ImageIndex = 1;
			}
			else
			{
				this.btnBold.FlatStyle=FlatStyle.Popup;//.Flat;								
				this.btnBold.ImageIndex = 0;
			}
			FontStyle style=FontStyle.Regular;
			if(m_bItalics)
				style=style|FontStyle.Italic;
			if(m_bUnderLine)
				style=style|FontStyle.Underline;				
			if(m_bBold)
				style=style|FontStyle.Bold;
		
			this.txtBoxPreviewChat.SelectionFont=new Font(this.txtBoxPreviewChat.Font.FontFamily,this.txtBoxPreviewChat.Font.Size,style);

			txtBoxPreviewChat.Focus();
		}


		private void btnItalics_Click(object sender, System.EventArgs e)
		{
			m_bItalics=!m_bItalics;
			if(m_bItalics)
			{	
				this.btnItalics.FlatStyle=FlatStyle.Standard;				
				this.btnItalics.ImageIndex = 3;
			}
			else
			{
				this.btnItalics.FlatStyle=FlatStyle.Popup;//.Flat;
				this.btnItalics.ImageIndex = 2;
			}
			FontStyle style=FontStyle.Regular;
			if(m_bItalics)
				style=style|FontStyle.Italic;
			if(m_bUnderLine)
				style=style|FontStyle.Underline;				
			if(m_bBold)
				style=style|FontStyle.Bold;
		
			this.txtBoxPreviewChat.SelectionFont=new Font(this.txtBoxPreviewChat.Font.FontFamily,this.txtBoxPreviewChat.Font.Size,style);

			txtBoxPreviewChat.Focus();
		}


		private void btnUnderline_Click(object sender, System.EventArgs e)
		{
			m_bUnderLine=!m_bUnderLine;
			if(m_bUnderLine)
			{	
				this.btnUnderLine.FlatStyle=FlatStyle.Standard;
				this.btnUnderLine.ImageIndex = 5;
			}
			else
			{
				this.btnUnderLine.FlatStyle=FlatStyle.Popup;//.Flat;
				this.btnUnderLine.ImageIndex = 4;
			}	
			FontStyle style=FontStyle.Regular;
			if(m_bItalics)
				style=style|FontStyle.Italic;
			if(m_bUnderLine)
				style=style|FontStyle.Underline;				
			if(m_bBold)
				style=style|FontStyle.Bold;
		
			this.txtBoxPreviewChat.SelectionFont=new Font(this.txtBoxPreviewChat.Font.FontFamily,this.txtBoxPreviewChat.Font.Size,style);

			txtBoxPreviewChat.Focus();
		
		}

		
		private void txtBoxPreviewChat_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode==Keys.Enter && e.Shift==false)
			{				
				btnSend_Click(this,new System.EventArgs());				
				e.Handled=true;
				if(previousChar!=null)
				{
					previousChar.Clear();
				}
				return;
			}			
		}
		
	
		# region CheckSmiley(ref string txt)

		private int CheckSmiley(ref string txt)
		{
			
			
			try
			{
				int i=0;
				for(i=0;i<10;i++)
				{
					if(txt.Length>=i)
						length[i]=txt.Substring(txt.Length - i, i).ToUpper();					
					else length[i]=".";
				}
				for(i=0;i<this.emoticonArray.Count;i++)
				{
					if(((EmoticonData)emoticonArray[i]).shortcut==length[((EmoticonData)emoticonArray[i]).shortcut.Length])
						return i;							
				}
				return -1;	
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==>CheckSmiley( ==> 1046",exp,null,false);				
				return -1;		
			}

					
		}

		# endregion 

		# region txtBoxPreviewChat_KeyPress(

		private void txtBoxPreviewChat_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{			
			try
			{
				string s=this.txtBoxPreviewChat.Text.Substring(0,this.txtBoxPreviewChat.SelectionStart) + e.KeyChar;
				int i=CheckSmiley(ref s);
				if(i!=-1)
				{
					int startSelect=this.txtBoxPreviewChat.SelectionStart;
					this.txtBoxPreviewChat.SelectionStart=0;
					this.txtBoxPreviewChat.SelectionLength=this.txtBoxPreviewChat.Text.Length - ((EmoticonData)emoticonArray[i]).shortcut.Length +1;
					s=this.txtBoxPreviewChat.SelectedRtf;
					this.txtBoxPreviewChat.Rtf=s;
					this.txtBoxPreviewChat.SelectionStart=startSelect - ((EmoticonData)emoticonArray[i]).shortcut.Length +1;				
					this.txtBoxPreviewChat.InsertImage(((EmoticonData)emoticonArray[i]).img);	
					e.Handled=true;
				}				
				previousChar.Add(e.KeyChar);
	
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==>txtBoxPreviewChat_KeyPress( ==> 1078",exp,null,false);				
			}
			
		}

		# endregion 


		private void btnFont_Click(object sender, System.EventArgs e)
		{
			FontDialog fd=new FontDialog();
			fd.Font=this.txtBoxPreviewChat.SelectionFont;			
			fd.ShowDialog();
			this.txtBoxPreviewChat.SelectionFont=fd.Font;


			

			this.m_bUnderLine=fd.Font.Underline;
			this.m_bItalics=fd.Font.Italic;
			this.m_bBold=fd.Font.Bold;

			
			if(fd.Font.Bold)
				this.btnBold.FlatStyle=FlatStyle.Standard;	
			else
				this.btnBold.FlatStyle=FlatStyle.Popup;

			if(fd.Font.Underline)
				this.btnUnderLine.FlatStyle=FlatStyle.Standard;
			else
				this.btnUnderLine.FlatStyle=FlatStyle.Popup;
			
			if(fd.Font.Italic)
				this.btnItalics.FlatStyle=FlatStyle.Standard;	
			else
				this.btnItalics.FlatStyle=FlatStyle.Popup;				
		}

		private void btnEmoticon_Click(object sender, System.EventArgs e)
		{
			try
			{ 
				/* // to remove emoticons
				System.Drawing.Point pt = new Point(btnEmoticon.Location.X,btnEmoticon.Location.Y);
				pt.Y += btnEmoticon.Height - 5;
				pt.X += 100;
				cmenu_Emoticons.Show(btnEmoticon,pt);
				*/
			}
			catch(Exception ex)
			{
				ex=ex;
			}
		}

		private void _menuItem_Select(object _sender, EventArgs e)
		{
		}

		/*private void btnSend_Enter(object sender, System.EventArgs e)
		{
			this.txtBoxPreviewChat.Focus();
		}*/

		private void chatControl_Load(object sender, System.EventArgs e)
		{
			/*thUpdateLog = new Thread(new ThreadStart(updateLog));
			thUpdateLog.Name = "Thread : updateLog(text messages) to database";
			thUpdateLog.Start();
			*/

		}

		# region Save()
		public void Save()
		{
			if(this.filename=="")
			{
				SaveAs();
				return;
			}
			try
			{
				StreamWriter sw = new StreamWriter(filename);							

				string[] lines=this.txtBoxViewChat.Text.Split(new char[]{'\n'});
				for(int i=0;i<lines.Length;i++)
				{
					sw.WriteLine(lines[i]);
				}
				
				sw.Close();
				
			}
			catch(Exception exp)
			{				
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==> Save( ==> 1178",exp,"Couldn't save to " + filename,true);				
			}
		}
		#endregion 


		public void SaveAs()
		{

			try
			{
				SaveFileDialog fd=new SaveFileDialog();
				fd.Filter="Text files(*.txt)|*.txt";
				DialogResult res=fd.ShowDialog();
				if(res==DialogResult.Cancel)
					return;
				
				filename=fd.FileName;
				Save();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==> Save( ==> 1199",exp,"Couldn't save to " + filename,true);				
			}
		}

		private void txtBoxViewChat_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			if(thisWindowTabPage.ImageIndex == 0)
				thisWindowTabPage.ImageIndex = 1;
			else
				thisWindowTabPage.ImageIndex = 0;
            

		}

		private void p_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			checkPrint = txtBoxViewChat.Print(checkPrint, txtBoxViewChat.TextLength, e);

			// Check for more pages
			if (checkPrint < txtBoxViewChat.TextLength)
				e.HasMorePages = true;
			else
				e.HasMorePages = false;
		}
		private int checkPrint;
		

		private void p_BeginPrint(object sender, PrintEventArgs e)
		{
			checkPrint = 0;
		}


		private void txtBoxViewChat_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			this.txtBoxPreviewChat.Focus();		
		}

		private void panel2_Resize(object sender, System.EventArgs e)
		{
			if(this.panel2.Width<220)
				this.panel4.Hide();
			else
				this.panel4.Show();
		}

		private void panel3_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void txtBoxPreviewChat_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		System.Text.ASCIIEncoding encoding;
		
		Thread thUpdateLog;
		ArrayList sendUpdateMessages=new ArrayList();
		
		# region 
		private void updateLog()
		{
			
			try
			{
				while(true)
				{
					if(sendUpdateMessages.Count > 0)
					{
						string postData = "meetingid="+ WebMeeting.Client.NetworkManager.getInstance().profile.ClientRegistrationId +"&meetinglog="+ sendUpdateMessages[0]    ;
						sendUpdateMessages.RemoveAt(0);
					
						encoding=new System.Text.ASCIIEncoding();
						byte[]  byte1=encoding.GetBytes(postData);	
					
						System.Net.HttpWebRequest request = (System.Net.HttpWebRequest) System.Net.WebRequest.Create(WebMeeting.Client.Info.getInstance().WebsiteName + "/application/addlog.php");
						request.Method = "POST";
						request.ContentType = "application/x-www-form-urlencoded" ;
						//request.ContentLength = sendData.Length ;

						try
						{
							Stream sendStream = request.GetRequestStream();	
							sendStream.Write(byte1,0,byte1.Length );
							sendStream.Flush();
							sendStream.Close();
							//byte1 = "";
							//thUpdateLog.Abort(); 
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==> updateLog() ==> 1199",exp,null,false);				
						}
					
						encoding = null;
					
					}
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==> updateLog() ==> 1306",exp,null,false);				
			}
			
		}

		#endregion 


		# region btnSend_Click(object sender, System.EventArgs e)
		private void btnSend_Click(object sender, System.EventArgs e)
		{
			try
			{
				//this piece of code is useless according to Junaid.
				sendUpdateMessages.Add(txtBoxPreviewChat.Text);
				
				/*thUpdateLog = new Thread(new ThreadStart(updateLog));

				thUpdateLog.Name = "Thread : updateLog(text messages) to database";
				thUpdateLog.Start();*/
				if(processingThread==null)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Self made exception==>chatControl processingThread is null ",new Exception(),null,false);				
				}
				
				
				else if(!(processingThread.IsAlive))
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Self made exception==>chatControl processingThread is not alive  ",new Exception(),null,false);				
					
					/*count++;
					MessageBox.Show("else if(!(processingThread.IsAlive))"+count.ToString());
					MessageBox.Show("ThreadState :"+processingThread.ThreadState.ToString());
					//new chatControl(2);
					//m_bIsActive=true;
					//processingThread.Resume();
					*/
				}
				

				/*
				if(!(processingThread.IsAlive) && count>1)
				{
					this.AliveprocessingThread();
					count=0;
				}
				*/
				
				// By Zaeem 
				if(!(processingThread.IsAlive))
				{
					AliveprocessingThread();
				}
				
				//WebMeeting.Client.ClientUI.getInstance().ExceptionLog("Nework Connectivity Issues " + WebMeeting.Client.NetworkManager.thisInstance.bIsConnectedToServer.ToString() + " true mean ok and false mean problem of network connectivity");
				//bIsConnectedToServer
				// When u get connected its true 

				if(WebMeeting.Client.NetworkManager.thisInstance.bIsConnectedToServer)
					SendText("");	
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("chatControl.cs ==> btnSend_Click( ==> 1333",exp,null,false);				
			}
		
		}
		# endregion 


		//  This method make the process thread alive if it is stopped
		public void AliveprocessingThread()
		{
			
			if(!(processingThread.IsAlive))
			{
				processingThread=new Thread(new ThreadStart(consumeFunction));
				m_bIsActive=true;
				processingThread.Start();			
			
			}
		
		}





		private void btnSend_Enter(object sender, System.EventArgs e)
		{
			this.txtBoxPreviewChat.Focus();
		}

		private void btnRestore_Click(object sender, System.EventArgs e)
		{
			
		}
	}
	[Serializable]
	// smileys code
	public class EmoticonData
	{
		public string shortcut;
		public Image img;
	}

	public class Error
	{
		public static void Show(string strMessage)
		{

			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage(strMessage);
		}
	}

	public delegate void DelegateshowChatAlert();	
}
