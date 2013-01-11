using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for NewBrowserWnd.
	/// </summary>
	public class NewBrowserWnd : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		public AxSHDocVw.AxWebBrowser axWebBrowser1;
		private mshtml.HTMLDocument doc;

		private System.ComponentModel.Container components = null;

		public NewBrowserWnd()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			object param1=new object();			
			Info objInfo = new Info();
			int temp;
//			if((NetworkManager.thisInstance.profile.clientType==WebMeeting.Common.ClientType.ClientPresenter)||(NetworkManager.thisInstance.profile.clientType==WebMeeting.Common.ClientType.ClientHost))
//                temp=1;
//			else 
//				temp=0;

			if(WebMeeting.Client.ClientUI.getInstance().nclientType==1)
			{
				temp=1;
			}
			else
			{
				temp=0;
			}
			
			//string str = objInfo.WebsiteName + "/welcome/?mid=" + NetworkManager.getInstance().profile.ConferenceID +"&check="+temp ;									 
			
			string str = objInfo.WebsiteName + @"/welcome/?mid=" + WebMeeting.Client.ClientUI.getInstance().nmeeting_id+"&check="+temp ;									 
			//MessageBox.Show(str,"Main windows");
			//IDocHostUIHandler						
			//////////////////////////////////
			///mshtml.HTMLDocument doc;			
			////////////////////////////////////
			this.axWebBrowser1.Navigate(str,ref param1,ref param1,ref param1,ref param1);			

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NewBrowserWnd));
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.SuspendLayout();
			// 
			// axWebBrowser1
			// 
			this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser1.Enabled = true;
			this.axWebBrowser1.Location = new System.Drawing.Point(0, 0);
			this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
			this.axWebBrowser1.Size = new System.Drawing.Size(808, 512);
			this.axWebBrowser1.TabIndex = 0;
			this.axWebBrowser1.TitleChange += new AxSHDocVw.DWebBrowserEvents2_TitleChangeEventHandler(this.axWebBrowser1_TitleChange);
			this.axWebBrowser1.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.axWebBrowser1_DocumentComplete);
			this.axWebBrowser1.OnMenuBar += new AxSHDocVw.DWebBrowserEvents2_OnMenuBarEventHandler(this.axWebBrowser1_OnMenuBar);
			// 
			// NewBrowserWnd
			// 
			this.Controls.Add(this.axWebBrowser1);
			this.Name = "NewBrowserWnd";
			this.Size = new System.Drawing.Size(808, 512);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		
		private void axWebBrowser1_TitleChange(object sender, AxSHDocVw.DWebBrowserEvents2_TitleChangeEvent e)
		{
			string text=e.text.ToLower();
			if(text=="present a document")
			{
				WebMeeting.Client.ClientUI.getInstance().ShareMyDocument("");
			}
			else if(text=="share an application")
			{
				WebMeeting.Client.ClientUI.getInstance().StartApplicationSharingEx();
			}
			else if(text=="share your desktop")
			{
                //WebMeeting.Client.ClientUI.getInstance().StartEntireDesktopSharing(WebMeeting.Client.ClientUI.getInstance().listParticipents.SelectedItems[0].Index);
				WebMeeting.Client.ClientUI.getInstance().StartEntireDesktopSharing();
				
			}
		}

		private void axWebBrowser1_OnMenuBar(object sender, AxSHDocVw.DWebBrowserEvents2_OnMenuBarEvent e)
		{	
		}
		
		private bool ContextMenuEventHandler(mshtml.IHTMLEventObj e)
		{
			//e.cancelBubble=true;
			return false;
		}

		private void axWebBrowser1_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			doc = (mshtml.HTMLDocument)axWebBrowser1.Document;
			mshtml.HTMLDocumentEvents2_Event iEvent;
			iEvent = (mshtml.HTMLDocumentEvents2_Event) doc;
			iEvent.oncontextmenu+=new mshtml.HTMLDocumentEvents2_oncontextmenuEventHandler(ContextMenuEventHandler);
		}
	}
}
