using System;
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
	public class ManageContentWebEvaluations : System.Windows.Forms.UserControl
	{
		IEMenuHook menuHook = new IEMenuHook();
		private System.Windows.Forms.GroupBox groupBox4;
		public System.Windows.Forms.ListView listEvaluation;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		public System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.Button btnUploadContent;
		//private AxSHDocVw.AxWebBrowser axWebBrowser1;
		private System.Windows.Forms.Button btnRefresh;
		private AxSHDocVw.AxWebBrowser axWebBrowser1;
		private mshtml.HTMLDocument doc;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ManageContentWebEvaluations()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ManageContentWebEvaluations));
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.listEvaluation = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.lblInfo = new System.Windows.Forms.Label();
			this.btnUploadContent = new System.Windows.Forms.Button();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.listEvaluation);
			this.groupBox4.Location = new System.Drawing.Point(10, 13);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(634, 431);
			this.groupBox4.TabIndex = 4;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Manage Evaluation";
			// 
			// listEvaluation
			// 
			this.listEvaluation.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							 this.columnHeader4});
			this.listEvaluation.FullRowSelect = true;
			this.listEvaluation.Location = new System.Drawing.Point(3, 17);
			this.listEvaluation.MultiSelect = false;
			this.listEvaluation.Name = "listEvaluation";
			this.listEvaluation.Size = new System.Drawing.Size(0, 0);
			this.listEvaluation.TabIndex = 0;
			this.listEvaluation.View = System.Windows.Forms.View.Details;
			this.listEvaluation.DoubleClick += new System.EventHandler(this.listEvaluation_DoubleClick);
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Item";
			this.columnHeader4.Width = 538;
			// 
			// lblInfo
			// 
			this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblInfo.ForeColor = System.Drawing.Color.Red;
			this.lblInfo.Location = new System.Drawing.Point(200, 452);
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
			this.btnUploadContent.TabIndex = 7;
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
			this.axWebBrowser1.TabIndex = 1;
			this.axWebBrowser1.TitleChange  += new AxSHDocVw.DWebBrowserEvents2_TitleChangeEventHandler(this.axWebBrowser1_TitleChange);//.DWebBrowserEvents2_DocumentCompleteEventHandler(this.axWebBrowser1_DocumentComplete);
			this.axWebBrowser1.DocumentComplete  +=new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.axWebBrowser1_DocumentComplete);
			// 
			// ManageContentWebEvaluations
			// 
			this.Controls.Add(this.axWebBrowser1);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.btnUploadContent);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.lblInfo);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "ManageContentWebEvaluations";
			this.Size = new System.Drawing.Size(656, 480);
			this.VisibleChanged += new System.EventHandler(this.ManageContentWebEvaluations_VisibleChanged);
			this.Load += new System.EventHandler(this.ManageContentWebEvaluations_Load);
			this.Enter += new System.EventHandler(this.ManageContentWebEvaluations_Enter);
			this.groupBox4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.ResumeLayout(false);
 
		}
		#endregion

		private void listEvaluation_DoubleClick(object sender, System.EventArgs e)
		{
		
			if(listEvaluation.SelectedItems.Count < 1)
				return;
			ClientUI.getInstance().ExecuteEvaluation(listEvaluation.SelectedItems[0].Index);
		}


		public void RetrieveContent()
		{
			try
			{
				menuHook.StartSubclass(menuHook.IEFromhWnd(axWebBrowser1.Handle),axWebBrowser1);
				listEvaluation.Items.Clear();		
				lblInfo.Text = "Retrieving Content. Please Wait..";
				ChangeInfoStatus(true);
			}			
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebEvaluationo.cs line==> 192",exp,null,false);			
			}


//			WebContentRetrieveMessage msg = new WebContentRetrieveMessage(NetworkManager.getInstance().profile.ClientId,0);
//			NetworkManager.getInstance().SendLoadPacket(msg);
		}
		
		public void ChangeInfoStatus(bool bState)
		{
			lblInfo.Visible = bState;
				
		}

		private void listWebPresentations_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			System.Windows.Forms.MessageBox.Show("selection changed"); 
		}

		private void btnUploadContent_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(btnUploadContent.Text == "Upload")
				{
			
					string str = Info.getInstance().WebsiteName + "/application/contentlibrary.php?page=4&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId;
					object oUrl = str;
					object o = new object();
			
					axWebBrowser1.Visible = true;
					axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
			
					btnUploadContent.Text = "Manage";
					listEvaluation.Visible = false;
				}
				else
				{
					RetrieveContent();
					axWebBrowser1.Visible = false;
					listEvaluation.Visible = true;
					btnUploadContent.Text = "Upload";

				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebEvaluation.cs line==> 239",exp,null,false);			
			}

		}

		private void axWebBrowser1_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			try
			{
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
					else if(str.IndexOf("dmquestions.php") > 0)
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
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebEvaluation.cs line==> 292",exp,null,false);			
			}

			//			doc = (mshtml.HTMLDocument)axWebBrowser1.Document;
			//			mshtml.HTMLDocumentEvents2_Event iEvent;
			//			iEvent = (mshtml.HTMLDocumentEvents2_Event) doc;
			//			iEvent.oncontextmenu+=new mshtml.HTMLDocumentEvents2_oncontextmenuEventHandler(ContextMenuEventHandler);
		}


		private void ManageContentWebEvaluations_Load(object sender, System.EventArgs e)
		{

		}
		private void ManageContentWebEvaluations_VisibleChanged(object sender, System.EventArgs e)
		{
			if(NetworkManager.getInstance().profile.clientType != ClientType.ClientAttendee)
				btnUploadContent.Visible = true;
			else
				btnUploadContent.Visible = false;

		}

		private void axWebBrowser1_TitleChange(object sender,  AxSHDocVw.DWebBrowserEvents2_TitleChangeEvent e)
		{
			string str = (string ) e.text;
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
//			//	if(!(ClientUI.getInstance().tabControlWebContent.SelectedTab.Title == ClientUI.getInstance().tabPageWebFiles.Title) )
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

		private void btnRefresh_Click(object sender, System.EventArgs e)
		{
			axWebBrowser1.Refresh();
		}

		private void ManageContentWebEvaluations_Enter(object sender, System.EventArgs e)
		{
			try
			{
				string str = Info.getInstance().WebsiteName + "/application/postmquestions.php?meetingid=" + NetworkManager.getInstance().profile.ConferenceID + "&mid=" + NetworkManager.getInstance().profile.ClientRegistrationId;
				object oUrl = str;
				object o = new object();
			
				axWebBrowser1.Visible = true;
				axWebBrowser1.Navigate2(ref oUrl,ref o,ref o,ref o,ref o);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>ManageContentWebEvaluation.cs line==> 374",exp,null,false);			
			}

			
		}

	/*	private void axWebBrowser1_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
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
