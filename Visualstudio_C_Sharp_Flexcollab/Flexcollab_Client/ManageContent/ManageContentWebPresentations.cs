using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using WebMeeting;
using WebMeeting.Common;
using WebMeeting.Client;
using System.Text.RegularExpressions;

using System.Diagnostics;

using System.Data;
using System.Windows.Forms;
using WebMeeting.Client.Alerts;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for ManageContent.
	/// </summary>
	public class ManageContentWebPresentations : System.Windows.Forms.UserControl
	{
		IEMenuHook menuHook = new IEMenuHook();
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		public System.Windows.Forms.Label lblInfo;
		public System.Windows.Forms.ListView listWebPresentations;
		//private AxSHDocVw.AxWebBrowser axWebBrowser1;
		private System.Windows.Forms.Button btnUploadContent;
		public AxSHDocVw.AxWebBrowser axWebBrowser1;
		private System.Windows.Forms.Button btnRefresh;
		private mshtml.HTMLDocument doc;
		public string strOpenedPage=null;
		public bool locktoexecute=false;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ManageContentWebPresentations()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ManageContentWebPresentations));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.listWebPresentations = new System.Windows.Forms.ListView();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.lblInfo = new System.Windows.Forms.Label();
			this.btnUploadContent = new System.Windows.Forms.Button();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.listWebPresentations);
			this.groupBox1.Location = new System.Drawing.Point(10, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(634, 431);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Manage WebPresentations";
			// 
			// listWebPresentations
			// 
			this.listWebPresentations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								   this.columnHeader2});
			this.listWebPresentations.FullRowSelect = true;
			this.listWebPresentations.Location = new System.Drawing.Point(3, 17);
			this.listWebPresentations.MultiSelect = false;
			this.listWebPresentations.Name = "listWebPresentations";
			this.listWebPresentations.Size = new System.Drawing.Size(0, 0);
			this.listWebPresentations.TabIndex = 0;
			this.listWebPresentations.View = System.Windows.Forms.View.Details;
			this.listWebPresentations.DoubleClick += new System.EventHandler(this.listWebPresentations_DoubleClick);
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Item";
			this.columnHeader2.Width = 258;
			// 
			// lblInfo
			// 
			this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblInfo.ForeColor = System.Drawing.Color.Red;
			this.lblInfo.Location = new System.Drawing.Point(183, 452);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(0, 0);
			this.lblInfo.TabIndex = 6;
			this.lblInfo.Text = "Retrieving Content. Please Wait..";
			this.lblInfo.Visible = false;
			// 
			// btnUploadContent
			// 
			this.btnUploadContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUploadContent.Location = new System.Drawing.Point(560, 448);
			this.btnUploadContent.Name = "btnUploadContent";
			this.btnUploadContent.Size = new System.Drawing.Size(0, 0);
			this.btnUploadContent.TabIndex = 9;
			this.btnUploadContent.Text = "Upload";
			this.btnUploadContent.Visible = false;
			this.btnUploadContent.Click += new System.EventHandler(this.btnUploadContent_Click);
			// 
			// btnRefresh
			// 
			this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(212)), ((System.Byte)(202)), ((System.Byte)(227)));
			this.btnRefresh.Location = new System.Drawing.Point(288, 448);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.TabIndex = 10;
			this.btnRefresh.Text = "&Refresh";
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// axWebBrowser1
			// 
			this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser1.Enabled = true;
			this.axWebBrowser1.Location = new System.Drawing.Point(0, 0);
			this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
			this.axWebBrowser1.Size = new System.Drawing.Size(656, 480);
			this.axWebBrowser1.TitleChange +=new AxSHDocVw.DWebBrowserEvents2_TitleChangeEventHandler(axWebBrowser1_TitleChange);
			//this.axWebBrowser1.BeforeNavigate2 +=new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.axLunchWebsite); 
				//.DWebBrowserEvents2_TitleChangeEventHandler(axWebBrowser1_TitleChange);
			this.axWebBrowser1.TabIndex = 1;

			this.axWebBrowser1.DocumentComplete  += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.axWebBrowser1_DownloadComplete);
			// 
			// ManageContentWebPresentations
			// 
			this.Controls.Add(this.axWebBrowser1);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnUploadContent);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "ManageContentWebPresentations";
			this.Size = new System.Drawing.Size(656, 480);
			this.Load+=new EventHandler(this.ManageContentWebPresentations_Enter);
			//this.Enter += new System.EventHandler();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// old code not used Now 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listWebPresentations_DoubleClick(object sender, System.EventArgs e)
		{
			if(listWebPresentations.SelectedItems.Count < 1)
				return;			
			if((MessageBox.Show("Do you want to Execute the WebPresentation","WebMeeting",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes))
			{
				ClientUI.getInstance().ExecuteWebPresentation(listWebPresentations.SelectedItems[0].Index);		
			}
		}

			
		public void RetrieveContent()
		{
			try
			{
				menuHook.StartSubclass(menuHook.IEFromhWnd(axWebBrowser1.Handle),axWebBrowser1);

				listWebPresentations.Items.Clear();
				lblInfo.Text = "Retrieving Content. Please Wait..";
				ChangeInfoStatus(true);

				WebContentRetrieveMessage msg = new WebContentRetrieveMessage(NetworkManager.getInstance().profile.ClientId,0);
				NetworkManager.getInstance().SendLoadPacket(msg);
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebPolls.cs line==> 205",exp,null,false);			
			}
		}
		
		public void ChangeInfoStatus(bool bState)
		{
			lblInfo.Visible = bState;
					
		}

	
		private void btnUploadContent_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(btnUploadContent.Text == "Upload")
				{
					string str = Info.getInstance().WebsiteName + "/application/contentlibrary.php?page=1&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId;
					object oUrl = str;
					object o = new object();
			
					axWebBrowser1.Visible = true;
					axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
			
					btnUploadContent.Text = "Manage";
					listWebPresentations.Visible = false;
				}
				else
				{
					RetrieveContent();
					axWebBrowser1.Visible = false;
					listWebPresentations.Visible = true;
					btnUploadContent.Text = "Upload";
				}
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebPresentations.cs line==> 242",exp,null,false);			
			}
		}

		private void axWebBrowser1_DownloadComplete(object sender,  AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			
			try
			{
				//MessageBox.Show(axWebBrowser1.Handle.ToString());
				menuHook.StartSubclass(menuHook.IEFromhWnd(axWebBrowser1.Handle),axWebBrowser1);
			
				string str=axWebBrowser1.LocationURL; 
				SHDocVw.IWebBrowser2 doc=e.pDisp as SHDocVw.IWebBrowser2;
			 	
				if (doc==(sender as AxSHDocVw.AxWebBrowser).GetOcx())
				{
					if(str.IndexOf("slideshow.php") > 0)  // these are dependent on file(.php) names of manage Content.
					{
						strOpenedPage="slideshow";
//						if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebPresentations.Title) )
//							ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebPresentations;
					}
					else if(str.IndexOf("premquestions.php") > 0)
					{
						strOpenedPage="pre-question";	
//						if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebContentPolls.Title)) 
//							ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebContentPolls;
					}					
					//else if(str.IndexOf("webshare.php") > 0)					
					else if(str.IndexOf("webshare.php") > 0)
					{
						strOpenedPage="webshare";
//						if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebContentBookmarks.Title) )
//							ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebContentBookmarks;
					}
					else if(str.IndexOf("dmquestions.php") > 0)
					{
						strOpenedPage="dmquestion";
//						if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebContentPolls.Title)) 
//							ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebContentPolls;
					}
					else
						if(str.IndexOf("uploads.php") > 0)
					{
						strOpenedPage="uploads";
//						if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebFiles.Title) )
//							ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebFiles;
					}
					else
						if(str.IndexOf("postmquestions.php") > 0)
					{
						strOpenedPage="postquestion";
//						if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebEvaluation.Title) )
//							ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebEvaluation;
					}
				}
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebPresentations.cs line==> 298",exp,null,false);			
			} 
//			else
//			{
//				ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebInfo;
//			}
		}
		public void ShowContentPage()
		{
			//string str=@"C:/webshare.html";
			// commented by Zaeem as it shd pass the Host id for the meeting 
			//string str = Info.getInstance().WebsiteName + "/application/contentlibrary.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId;
			// the line below is the modified line of the above , it will send the Host id 
			string str = Info.getInstance().WebsiteName + "/application/contentlibrary.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&mid="+ClientUI.getInstance().GetHostId_Registration();
			object oUrl = str;
			object o = new object();			
			axWebBrowser1.Visible = true;
			axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
		}
		private void ManageContentWebPresentations_Enter(object sender, System.EventArgs e)
		{
//			string str = Info.getInstance().WebsiteName + "/application/contentlibrary.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId;
//			object oUrl = str;
//			object o = new object();			
//			axWebBrowser1.Visible = true;
//			axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
//			string str = Info.getInstance().WebsiteName + "/application/slideshow.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId;
//			object oUrl = str;
//			object o = new object();
//			
//			axWebBrowser1.Visible = true;
//			axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
		}

		private void btnRefresh_Click(object sender, System.EventArgs e)
		{
			axWebBrowser1.Refresh();
		}

		/*private void axWebBrowser1_DownloadComplete_1(object sender, System.EventArgs e)
		{
			doc = (mshtml.HTMLDocument)axWebBrowser1.Document;
			doc.title=
			mshtml.HTMLDocumentEvents2_Event iEvent;
			iEvent = (mshtml.HTMLDocumentEvents2_Event) doc;
			iEvent.oncontextmenu+=new mshtml.HTMLDocumentEvents2_oncontextmenuEventHandler(ContextMenuEventHandler);
		}
		private bool ContextMenuEventHandler(mshtml.IHTMLEventObj e)
		{
			//e.cancelBubble=true;
			return false;
		}*/

		private bool flagToUnique = false;
		string prevFileName = "";
		ArrayList arrstr=new ArrayList();
 
		string[] arrstrOpenFiles;
		string strRepeating ="";
		bool StopNavigation=false;
		
		int nCountOpenFiles=1;
		//string uploadingPath="";
		object o = new object();
		
		private void axLunchWebsite(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
		{
			try
			{
				string str=e.uRL.ToString();
			
				if(str=="javascript:check();")
					return;
				if (Regex.IsMatch(str,"www.compassnav.com"))
				{
					if(str.IndexOf(".php") < 0)	
					{
						object oUrl;
						object ojunk= new object();
						axWebBrowser1.Stop();
						oUrl="http://www.compassnav.com/application/webshare.php";
						axWebBrowser1.Navigate2(ref oUrl,ref ojunk,ref ojunk,ref ojunk,ref ojunk); 
						//System.Windows.Forms.MessageBox.Show(e.uRL.ToString());
						StopNavigation=true;
						Client.ClientUI.getInstance().shareBrowserForWebFiles(str);
						//Client.ClientUI. shareBrowserForWebFiles(str);

					}
					else
						StopNavigation=false;

				}
				else
				{
					object oUrl;
					object ojunk= new object();
					axWebBrowser1.Stop();
					oUrl="http://www.compassnav.com/application/webshare.php";
					axWebBrowser1.Navigate2(ref oUrl,ref ojunk,ref ojunk,ref ojunk,ref ojunk); 
					//System.Windows.Forms.MessageBox.Show(e.uRL.ToString());
					StopNavigation=true;
					Client.ClientUI.getInstance().shareBrowserForWebFiles(str);
					//Client.ClientUI. shareBrowserForWebFiles(str);
				}
			
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebPresentation.cs line==> 388",exp,null,false);			
			}
			//
		}

		private void startSlideShow(object sender,WebMeeting.Client.ManageContent.DartbordEventArgs e)
		{
			if(e.CurrentStatus =="completed")
			{
				try
				{
					//MessageBox.Show("==> manage content presentatiuon 400");
			
					ClientUI.getInstance().ShareMyDocumentByContentManagement(e.UploadPath);
				//	nCountOpenFiles=1;
				//	this.flagToUnique=false;
					//this.StopNavigation=false;
				}
				catch(Exception ex)
				{
					MessageBox.Show("error downloading " +  ex.ToString());
				}
			}
//			else
//			{
//				//nCountOpenFiles=1;
//				//this.flagToUnique=false;
//				//this.StopNavigation=false;
//			}
		}
		public void SetEmptytitleofPage()
		{
			try
			{
				String code2 =	"document.title='';";

				mshtml.IHTMLDocument2 doc = (mshtml.IHTMLDocument2) this.axWebBrowser1.Document;
				mshtml.IHTMLWindow2 parentWindow = doc.parentWindow;
				parentWindow.execScript(code2, "javascript");
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebPresentation.cs line==> 428",exp,null,false);			
			}
			
		}
		private void changeTitleofPage()
		{
			try
			{
				//String code2 =	"document.title='http://www.compassnav.com/application/webshare.php';";
				String code2 =	"document.title='';";
				mshtml.IHTMLDocument2 doc = (mshtml.IHTMLDocument2) this.axWebBrowser1.Document;
				mshtml.IHTMLWindow2 parentWindow = doc.parentWindow;
				parentWindow.execScript(code2, "javascript");
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebPresentation.cs line==> 428",exp,null,false);			
			}
			
		}
		private void axWebBrowser1_TitleChange(object sender, AxSHDocVw.DWebBrowserEvents2_TitleChangeEvent e)
		{
			System.Windows.Forms.SaveFileDialog SvgFileDia = new SaveFileDialog();
			string str = (string ) e.text;			
			string filePath = e.text ;							
			
			if(strOpenedPage=="")
				return;
			if(locktoexecute==true)
				return;
			//MessageBox.Show(str);
			if( (filePath.IndexOf("&") < 0  && filePath.IndexOf(".php") < 0))//&&  nCountOpenFiles==2)
			{
				//MessageBox.Show(strOpenedPage,"CompassNav");
				if(strOpenedPage=="dmquestion")
				{
					changeTitleofPage();				
					Client.ClientUI.getInstance().CreateNewPollingWindowFrmMangeContent(null,null,false,str);
					return;
				}
				else if(strOpenedPage=="webshare")
				{
					changeTitleofPage();
					Client.ClientUI.getInstance().shareBrowserForWebFiles(str);
					return;
				}
				else if(strOpenedPage=="uploads")
				{
					changeTitleofPage();
					SvgFileDia.FileName=System.IO.Path.GetFileName(str);  		
					if( SvgFileDia.ShowDialog()==DialogResult.OK)
					{					
						if(SvgFileDia.CheckPathExists)
						{
							WebMeeting.Client.ManageContent.frmDownloader frm=new WebMeeting.Client.ManageContent.frmDownloader(filePath,SvgFileDia.FileName);
							frm.Show();
						}
					}		
					return;
				}
				
				
				string fileName = filePath.Substring( filePath.LastIndexOf("/")+1);
				nCountOpenFiles=0;
				changeTitleofPage();
				//string uploadPath = WebMeeting.Client.DocumentSharingEx.createUploadFullPath_MangCont(fileName);
				try
				{						
//					WebMeeting.Client.ManageContent.frmDownloader frm=new WebMeeting.Client.ManageContent.frmDownloader(filePath ,uploadPath,true);
//					frm.DownComp +=new WebMeeting.Client.ManageContent.DownloadingComplete(this.startSlideShow);
//					frm.TopMost=true;
//					frm.Owner=ClientUI.getInstance();
//					frm.ShowDialog();						
					prevFileName = filePath;
					//MessageBox.Show(filePath);
					ClientUI.getInstance().ShareMyDocumentByContentManagement(filePath);
					filePath = filePath.Substring( filePath.LastIndexOf("/")+1 );					
				}
				catch(Exception exp)
				{
					//frmDownload.Close();
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebPresentation.cs line==> 458",exp,"Error downloading " +  exp.Message.ToString(),true);			
					
				}

				//					filePath = filePath.Substring( filePath.LastIndexOf("/")+1 );
				//					
				//					//MessageBox.Show("sharing: " + Application.StartupPath + "\\Downloaded\\" + fileName);
				//					//ClientUI.getInstance().ShareMyDocumentByContentManagement(filePath);
				//					ClientUI.getInstance().ShareMyDocumentByContentManagement(uploadPath);
			}
			else
			{
				//		if(nCountOpenFiles==2)nCountOpenFiles=0;
				//		nCountOpenFiles+=1;
			}		
			flagToUnique = !flagToUnique ;
			//}
				
			//flagToUnique = !flagToUnique ;
		}
			
			
			

//		}
	}
}
