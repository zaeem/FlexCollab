using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using mshtml;
namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for StartPage.
	/// </summary>
	public class StartPage : System.Windows.Forms.UserControl
	{
		public AxSHDocVw.AxWebBrowser axWebBrowser1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public StartPage()
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
			// 
			// StartPage
			// 
			this.Name = "StartPage";
			this.Size = new System.Drawing.Size(632, 480);
			this.Load += new System.EventHandler(this.StartPage_Load);

		}
		#endregion

		public void ChangeString(string str)
		{
			HTMLDocument doc = (HTMLDocument) axWebBrowser1.Document;
			IHTMLElement e =  doc.getElementById("oay");
			if(e!=null)
			{
				e.innerText = str;//"Thank you for joining {insert company name}’s web meeting. Please wait. The meeting will begin shortly ";
				/*
				mshtml.HTMLBody body = (IHTMLElement2)doc.body;		
			
				IHTMLElementCollection col =  (IHTMLElementCollection)body.getElementsByTagName("frame");							
				*/
			}
		}
		private void axWebBrowser1_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			
			if(NetworkManager.getInstance().profile.clientType == WebMeeting.Common.ClientType.ClientHost)
				ChangeString("Thank you for joining cnav's web meeting. Please wait. The meeting will begin shortly." );
		}

		private void StartPage_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
