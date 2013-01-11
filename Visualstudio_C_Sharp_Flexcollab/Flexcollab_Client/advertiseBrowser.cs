using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for advertiseBrowser.
	/// </summary>
	public class advertiseBrowser : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public AxSHDocVw.AxWebBrowser axWebBrowser1;

		public advertiseBrowser()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NewBrowserWnd));
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.SuspendLayout();
			// // axWebBrowser1
			// 
			this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser1.Enabled = true;
			this.axWebBrowser1.Location = new System.Drawing.Point(0, 0);
			this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
			this.axWebBrowser1.Size = new System.Drawing.Size(808, 512);
			this.axWebBrowser1.TabIndex = 0;
			
			// 
			// advertiseBrowser
			// 
			this.Controls.Add(this.axWebBrowser1);
			this.Name = "advertiseBrowser";
			this.Size = new System.Drawing.Size(240, 160);
			this.Load += new System.EventHandler(this.advertiseBrowser_Load);
			
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.ResumeLayout(false);
		}
		#endregion

		private void advertiseBrowser_Load(object sender, System.EventArgs e)
		{
			object param1=new object();			
			Info objInfo = new Info();
			string str = objInfo.WebsiteName + "/welcome/?mid=" + NetworkManager.getInstance().profile.ConferenceID ;
			this.axWebBrowser1.Navigate(str,ref param1,ref param1,ref param1,ref param1);
		}
	}
}
