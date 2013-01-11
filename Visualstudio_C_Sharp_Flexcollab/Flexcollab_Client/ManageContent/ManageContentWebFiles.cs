using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using WebMeeting;
using WebMeeting.Common;
using WebMeeting.Client;
using System.Threading;
using System.Data;
using System.Windows.Forms;
using System.Net;


namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for ManageContent.
	/// </summary>
	public class ManageContentWebFiles : System.Windows.Forms.UserControl
	{
		IEMenuHook menuHook = new IEMenuHook();
		private System.Windows.Forms.GroupBox groupBox3;
		public System.Windows.Forms.ListView listWebFiles;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		public System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.Button btnUploadContent;
		private AxSHDocVw.AxWebBrowser axWebBrowser1;
		private System.Windows.Forms.Button btnRefresh;
		private mshtml.HTMLDocument doc;
		private bool RefreshPage=false;



		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ManageContentWebFiles()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ManageContentWebFiles));
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.listWebFiles = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.lblInfo = new System.Windows.Forms.Label();
			this.btnUploadContent = new System.Windows.Forms.Button();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.listWebFiles);
			this.groupBox3.Location = new System.Drawing.Point(10, 13);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(634, 431);
			this.groupBox3.TabIndex = 3;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Manage WebFiles";
			// 
			// listWebFiles
			// 
			this.listWebFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						   this.columnHeader1});
			this.listWebFiles.Dock = System.Windows.Forms.DockStyle.Left;
			this.listWebFiles.Enabled = false;
			this.listWebFiles.FullRowSelect = true;
			this.listWebFiles.Location = new System.Drawing.Point(3, 17);
			this.listWebFiles.MultiSelect = false;
			this.listWebFiles.Name = "listWebFiles";
			this.listWebFiles.Size = new System.Drawing.Size(0, 411);
			this.listWebFiles.TabIndex = 0;
			this.listWebFiles.TabStop = false;
			this.listWebFiles.View = System.Windows.Forms.View.Details;
			this.listWebFiles.Visible = false;
			this.listWebFiles.DoubleClick += new System.EventHandler(this.listWebFiles_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Item";
			this.columnHeader1.Width = 561;
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
			this.btnUploadContent.TabIndex = 8;
			this.btnUploadContent.Text = "Upload";
			this.btnUploadContent.Visible = false;
			this.btnUploadContent.Click += new System.EventHandler(this.btnUploadContent_Click);
			// 
			// btnRefresh
			// 
			this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(212)), ((System.Byte)(202)), ((System.Byte)(227)));
			this.btnRefresh.Location = new System.Drawing.Point(288, 448);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.TabIndex = 9;
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
			this.axWebBrowser1.TabIndex = 1;
			this.axWebBrowser1.Enter += new System.EventHandler(this.axWebBrowser1_Enter);
			this.axWebBrowser1.TitleChange += new AxSHDocVw.DWebBrowserEvents2_TitleChangeEventHandler(this.axWebBrowser1_TitleChange);
			this.axWebBrowser1.DocumentComplete  +=new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.axWebBrowser1_DocumentComplete);
			//this.axWebBrowser1.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.axWebBrowser1_DocumentComplete);
			// 
			// ManageContentWebFiles
			// 
			this.Controls.Add(this.axWebBrowser1);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.btnUploadContent);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.lblInfo);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "ManageContentWebFiles";
			this.Size = new System.Drawing.Size(656, 480);
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void listWebFiles_DoubleClick(object sender, System.EventArgs e)
		{
			if(listWebFiles.SelectedItems.Count < 1)
				return;
			ClientUI.getInstance().ExecuteWebFile(listWebFiles.SelectedItems[0].Index);		
		}

		bool bDownloading;
		public Thread downloadthread;
		public void RetrieveContent()
		{
			try
			{
				if(!bDownloading)
				{
					downloadthread = new Thread(new ThreadStart(RetrieveFunction));
					downloadthread.Name="ManageContent WebFile Thread: RetrieveFunction()";
					downloadthread.Start();
					bDownloading = true;
				}
			}	
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebFiles.cs line==> 204",exp,null,false);			
			}
		}
		public void RetrieveFunction()
		{
			try
			{
				menuHook.StartSubclass(menuHook.IEFromhWnd(axWebBrowser1.Handle),axWebBrowser1);

				listWebFiles.Items.Clear();
				lblInfo.Text = "Retrieving Content. Please Wait..";
				ChangeInfoStatus(true);

				WebContentRetrieveMessage msg = new WebContentRetrieveMessage(NetworkManager.getInstance().profile.ClientId,0);
				NetworkManager.getInstance().SendLoadPacket(msg);
				bDownloading= false;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebFiles.cs line==> 220",exp,null,false);			
			}
		}
		
		public void ChangeInfoStatus(bool bState)
		{
			lblInfo.Visible = bState;
					
		}

		private void listWebPresentations_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}

		private void btnUploadContent_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(btnUploadContent.Text == "Upload")
				{
			
					string str = Info.getInstance().WebsiteName + "/application/contentlibrary.php?page=6&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId;
					object oUrl = str;
					object o = new object();
			
					axWebBrowser1.Visible = true;
					axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
			
					btnUploadContent.Text = "Manage";
					listWebFiles.Visible = false;
				}
				else
				{
					RetrieveContent();
					axWebBrowser1.Visible = false;
					listWebFiles.Visible = true;
					btnUploadContent.Text = "Upload";
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebFiles.cs line==> 265",exp,null,false);			
			
			}
		}

		private void axWebBrowser1_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			try
			{
				menuHook.StartSubclass(menuHook.IEFromhWnd(axWebBrowser1.Handle),axWebBrowser1);
			
				string str=axWebBrowser1.LocationURL; 
				SHDocVw.IWebBrowser2 doc=e.pDisp as SHDocVw.IWebBrowser2;
			 	
				if (doc==(sender as AxSHDocVw.AxWebBrowser).GetOcx())
				{
					if(str.IndexOf("slideshow.php") > 0)  // these are dependent on file(.php) names of manage Content.
					{
//						if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebPresentations.Title) )
//							ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebPresentations;
					}
					else
						if(str.IndexOf("premquestions.php") > 0)
					{
//						if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebContentPolls.Title)) 
//							ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebContentPolls;
					}
					else
						if(str.IndexOf("webshare.php") > 0)
					{
//						if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebContentBookmarks.Title) )
//							ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebContentBookmarks;
					}
					else if(str.IndexOf("dmquestions.php") > 0)
					{
//						if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebContentPolls.Title)) 
//							ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebContentPolls;
					}

					else
						if(str.IndexOf("uploads.php") > 0)
					{
				
//						if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebFiles.Title) )
//							ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebFiles;
					}
					else
						if(str.IndexOf("postmquestions.php") > 0)
					{
//						if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebEvaluation.Title) )
//							ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebEvaluation;
					}
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebFiles.cs line==> 321",exp,null,false);			
			
			}
		}


		private void btnRefresh_Click(object sender, System.EventArgs e)
		{
			axWebBrowser1.Refresh();
		}
		
		private void axWebBrowser1_Enter(object sender, EventArgs e)
		{
			
			try
			{
				string str ;
				if(NetworkManager.getInstance().profile.IsGuest)
					str = Info.getInstance().WebsiteName + "/application/uploads.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&gid=" + NetworkManager.getInstance().profile.ClientRegistrationId;
				else
					str = Info.getInstance().WebsiteName + "/application/uploads.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId ;			
				object oUrl = str;
				object o = new object();
		
				axWebBrowser1.Visible = true;
				axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebFiles.cs line==> 350",exp,null,false);			
			}
			
		}
		
		private bool flagToUnique = false;
		string prevFileName = "";
		int ncountFiles=1;
		
		//object o = new object();
		
		private void pickfile(object sender,System.ComponentModel.CancelEventArgs  e)
		{
			//System.Windows.Forms.MessageBox.Show(e.Cancel.ToString());
					
				
					
				
			//System.Windows.Forms.MessageBox.Show(System.IO.Path.GetDirectoryName(str));
			//Client.DownloadFile();
		}
			
		private void changeTitleofPage()
		{
			String code2 =	"document.title='"+Info.getInstance().WebsiteName + "/application/uploads.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&gid=" + NetworkManager.getInstance().profile.ClientRegistrationId + "';";

			mshtml.IHTMLDocument2 doc = (mshtml.IHTMLDocument2) this.axWebBrowser1.Document;
			mshtml.IHTMLWindow2 parentWindow = doc.parentWindow;
			parentWindow.execScript(code2, "javascript");
			
		}
	private void axWebBrowser1_TitleChange(object sender, AxSHDocVw.DWebBrowserEvents2_TitleChangeEvent e)
	{
		//System.Net.WebClient Client = new System.Net.WebClient ();
		System.Windows.Forms.SaveFileDialog SvgFileDia = new SaveFileDialog();
		string filePath="";
		filePath = (string )e.text;
//		if(RefreshPage==true)
//		{
//			RefreshPage=false;
//			this.axWebBrowser1_Enter(null,null); 
//		}
//		else
		if(filePath.IndexOf(".php") < 0)
		{
					
	//if(ncountFiles==2)
	//	{
	//		ncountFiles=1;
			//Trace.WriteLine("Evnet Occurs" + filePath.ToString());
			changeTitleofPage();
			SvgFileDia.FileName=System.IO.Path.GetFileName(filePath);  
					
			if( SvgFileDia.ShowDialog()==DialogResult.OK)
			{
					
				if(SvgFileDia.CheckPathExists)
				{
						
					WebMeeting.Client.ManageContent.frmDownloader frm=new WebMeeting.Client.ManageContent.frmDownloader(filePath,SvgFileDia.FileName);
					frm.Show();
		//			RefreshPage=true;
					
					//this.axWebBrowser1.
//					if(NetworkManager.getInstance().profile.IsGuest)
//						axWebBrowser1.LocationName=  Info.getInstance().WebsiteName + "/application/uploads.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&gid=" + NetworkManager.getInstance().profile.ClientRegistrationId;
//					else
//						axWebBrowser1.LocationName= Info.getInstance().WebsiteName + "/application/uploads.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId ;			
//					
					//axWebBrowser1.GoBack(); 
					//this.axWebBrowser1.
					
					//frmDownloadWaiting frmDownload = new frmDownloadWaiting();
//					frmDownload.Show();
//					Client.DownloadFile(filePath,SvgFileDia.FileName);
//					frmDownload.Close();
//					SvgFileDia.Dispose();
				}
		//System.Windows.Forms.MessageBox.Show("file location is selected"); 
			}
			else RefreshPage=true;

		}
	//SvgFileDia.FileOk += new CancelEventHandler(this.pickfile); 
	//
					
	//	}
	//	else
	//		ncountFiles=ncountFiles+1;
	//	Trace.WriteLine("Evnet Occurs" + ncountFiles.ToString());
	}

	//			if(str.IndexOf("slideshow.php") > 0)  // these are dependent on file(.php) names of manage Content.
	//			{
	//				//if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebPresentations.Title) )
	//					ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebPresentations;
	//			}
	//			else
	//				if(str.IndexOf("premquestions.php") > 0)
	//			{
	//				//if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebContentPolls.Title)) 
	//					ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebContentPolls;
	//			}
	//			else
	//				if(str.IndexOf("webshare.php") > 0)
	//			{
	//				//if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebContentBookmarks.Title) )
	//					ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebContentBookmarks;
	//			}
	//			else
	//				if(str.IndexOf("uploads.php") > 0)
	//			{
	//				
	//				//if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebFiles.Title) )
	//					ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebFiles;
	//			}
	//			else
	//				if(str.IndexOf("postmquestions.php") > 0)
	//			{
	//				//if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebEvaluation.Title) )
	//					ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebEvaluation;
	//			}
	//			else
	//			{
	//				ClientUI.getInstance().tabControlWebContent.SelectedTab = ClientUI.getInstance().tabPageWebInfo;
	//			}

	/*
				if(flagToUnique )
			{
				string filePath = e.text ;
				if( (filePath.IndexOf("&") < 0  && filePath.IndexOf(".php") < 0)&& prevFileName != filePath )
				{
					string fileName = filePath.Substring( filePath.LastIndexOf("/")+1 );

					System.Net.WebClient Client = new System.Net.WebClient ();
					
					frmDownloadWaiting frmDownload = new frmDownloadWaiting();
					frmDownload.Show();
					
					//CreateLocalDirectoryStructure(
					////////////////////////////// target file path
					string uploadPath = WebMeeting.Client.DocumentSharingEx.createUploadFullPath_MangCont(fileName);
					//createUploadFullPath_MangCont
					////////////////////////////// target 
					
					try 
					{
						Client.DownloadFile(filePath ,uploadPath);
						frmDownload.Close();
						frmDownload = null;			
						prevFileName = filePath;

						filePath = filePath.Substring( filePath.LastIndexOf("/")+1 );
					
						ClientUI.getInstance().ShareMyDocumentByContentManagement(uploadPath);
					}
					catch(Exception ee)
					{
						frmDownload.Close();
						MessageBox.Show("error downloading " +  ee.ToString());
					}
				}
				
				flagToUnique = !flagToUnique ;
			}
			flagToUnique = !flagToUnique ;
			*/


	/*private void axWebBrowser1_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			doc = (mshtml.HTMLDocument)axWebBrowser1.Document;
			mshtml.HTMLDocumentEvents2_Event iEvent;
			iEvent = (mshtml.HTMLDocumentEvents2_Event) doc;
			iEvent.oncontextmenu+=new mshtml.HTMLDocumentEvents2_oncontextmenuEventHandler(ContextMenuEventHandler);
		}
		private bool ContextMenuEventHandler(mshtml.IHTMLEventObj e)
		{
			//e.cancelBubble=true;
			return false;
		}*/
}
}
