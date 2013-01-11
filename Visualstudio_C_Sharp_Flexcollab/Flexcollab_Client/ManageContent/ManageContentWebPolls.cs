using System;
//using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using WebMeeting;
using WebMeeting.Common;
using WebMeeting.Client;

using System.Data;
using System.Windows.Forms;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for ManageContent.
	/// </summary>
	public class ManageContentWebPolls : System.Windows.Forms.UserControl
	{
		IEMenuHook menuHook = new IEMenuHook();
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.ListView listWebPolls;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		public System.Windows.Forms.Label lblInfo;
		//private AxSHDocVw.AxWebBrowser axWebBrowser1;
		private System.Windows.Forms.Button btnUploadContent;
		//private AxSHDocVw.AxWebBrowser axWebBrowser1;
		private System.Windows.Forms.Button btnRefresh;
		private AxSHDocVw.AxWebBrowser axWebBrowser1;
		private mshtml.HTMLDocument doc;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ManageContentWebPolls()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ManageContentWebPolls));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.listWebPolls = new System.Windows.Forms.ListView();
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
			this.groupBox1.Controls.Add(this.listWebPolls);
			this.groupBox1.Location = new System.Drawing.Point(10, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(634, 431);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Manage WebPolls";
			// 
			// listWebPolls
			// 
			this.listWebPolls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						   this.columnHeader2});
			this.listWebPolls.FullRowSelect = true;
			this.listWebPolls.Location = new System.Drawing.Point(3, 17);
			this.listWebPolls.MultiSelect = false;
			this.listWebPolls.Name = "listWebPolls";
			this.listWebPolls.Size = new System.Drawing.Size(0, 0);
			this.listWebPolls.TabIndex = 0;
			this.listWebPolls.View = System.Windows.Forms.View.Details;
			this.listWebPolls.Visible = false;
			this.listWebPolls.DoubleClick += new System.EventHandler(this.listWebPolls_DoubleClick);
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
			this.btnUploadContent.TabIndex = 10;
			this.btnUploadContent.Text = "Upload";
			this.btnUploadContent.Visible = false;
			this.btnUploadContent.Click += new System.EventHandler(this.btnUploadContent_Click);
			// 
			// btnRefresh
			// 
			this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(212)), ((System.Byte)(202)), ((System.Byte)(227)));
			this.btnRefresh.Location = new System.Drawing.Point(288, 448);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.TabIndex = 11;
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
			this.axWebBrowser1.TitleChange  += new AxSHDocVw.DWebBrowserEvents2_TitleChangeEventHandler(this.axWebBrowser1_TitleChange);
			this.axWebBrowser1.DocumentComplete  +=new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.axWebBrowser1_DocumentComplete);
			// 
			// ManageContentWebPolls
			// 
			this.Controls.Add(this.axWebBrowser1);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.btnUploadContent);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "ManageContentWebPolls";
			this.Size = new System.Drawing.Size(656, 480);
			this.Enter += new System.EventHandler(this.ManageContentWebPolls_Enter);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void listWebPolls_DoubleClick(object sender, System.EventArgs e)
		{
			if(listWebPolls.SelectedItems.Count < 1)
				return;
			ClientUI.getInstance().ExecuteWebPoll(listWebPolls.SelectedItems[0].Index);
		}
			
		public void RetrieveContent()		
		{
			try
			{
				menuHook.StartSubclass(menuHook.IEFromhWnd(axWebBrowser1.Handle),axWebBrowser1);
			
				listWebPolls.Items.Clear();
				lblInfo.Text = "Retrieving Content. Please Wait..";
				ChangeInfoStatus(true);

				WebContentRetrieveMessage msg = new WebContentRetrieveMessage(NetworkManager.getInstance().profile.ClientId,0);
				NetworkManager.getInstance().SendLoadPacket(msg);
			}catch(Exception exp)
			 {
			 WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebPolls.cs line==> 194",exp,null,false);			
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
			if(btnUploadContent.Text == "Upload")
			{
				string str = Info.getInstance().WebsiteName + "/application/contentlibrary.php?page=3&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId;
				object oUrl = str;
				object o = new object();
			
				axWebBrowser1.Visible = true;
				axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
			
				btnUploadContent.Text = "Manage";
				listWebPolls.Visible = false;
			}
			else
			{
				RetrieveContent();
				axWebBrowser1.Visible = false;
				listWebPolls.Visible = true;
				btnUploadContent.Text = "Upload";
			}
		}
		private void changeTitleofPage()
		{
			try
			{
				String code2 =	"document.title='http://www.compassnav.com/application/dmquestions.php';";

				mshtml.IHTMLDocument2 doc = (mshtml.IHTMLDocument2) this.axWebBrowser1.Document;
				mshtml.IHTMLWindow2 parentWindow = doc.parentWindow;
				parentWindow.execScript(code2, "javascript");
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebPolls.cs line==> 243",exp,null,false);			
			}
		}
		private void axWebBrowser1_TitleChange(object sender,  AxSHDocVw.DWebBrowserEvents2_TitleChangeEvent e)
		{
			string str = (string ) e.text;
			//Trace.WriteLine("Selected Question" + str);
			if(str.IndexOf(".php") < 0)
			{
				//Trace.WriteLine("Title Change");
				changeTitleofPage();
				
				Client.ClientUI.getInstance().CreateNewPollingWindowFrmMangeContent(null,null,false,str);
				//Client.ClientUI.getInstance().pollingTabControl.Enabled=true;
			//	Trace.WriteLine("Selected Question" + str);
				
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

			//menuHook.StartSubclass(menuHook.IEFromhWnd(axWebBrowser1.Handle),axWebBrowser1);
		}

//		private void axWebBrowser1_DownloadComplete(object sender, System.EventArgs e)
//		{
//			menuHook.StartSubclass(menuHook.IEFromhWnd(axWebBrowser1.Handle),axWebBrowser1);
//		}

		private void btnRefresh_Click(object sender, System.EventArgs e)
		{
			axWebBrowser1.Refresh();
		}

		private void ManageContentWebPolls_Enter(object sender, System.EventArgs e)
		{
			try
			{
			
				string str = Info.getInstance().WebsiteName + "/application/dmquestions.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId;
				object oUrl = str;
				object o = new object();
			
				axWebBrowser1.Visible = true;
				axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebPolls.cs line==> 322",exp,null,false);			
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
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebPolls.cs line==> 322",exp,null,false);			
			}
			
//			doc = (mshtml.HTMLDocument)axWebBrowser1.Document;
//			mshtml.HTMLDocumentEvents2_Event iEvent;
//			iEvent = (mshtml.HTMLDocumentEvents2_Event) doc;
//			iEvent.oncontextmenu+=new mshtml.HTMLDocumentEvents2_oncontextmenuEventHandler(ContextMenuEventHandler);
		}
/*		private bool ContextMenuEventHandler(mshtml.IHTMLEventObj e)
		{
			//e.cancelBubble=true;
			return false;
		}*/

	}
}
