using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for NewWelcome.
	/// </summary>
	public class NewWelcome : System.Windows.Forms.UserControl
	{
		private AxSHDocVw.AxWebBrowser nWebBrowser;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NewWelcome()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			Object param1,param2,param3,param4;
			param1=new object();
			param2=new object();
			param3=new object();
			param4=new object();
            this.nWebBrowser.Navigate("www.hotmail.com",ref param1,ref param2,ref param3,ref param4);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NewWelcome));
			this.nWebBrowser = new AxSHDocVw.AxWebBrowser();
			((System.ComponentModel.ISupportInitialize)(this.nWebBrowser)).BeginInit();
			this.SuspendLayout();
			// 
			// nWebBrowser
			// 
			this.nWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nWebBrowser.Enabled = true;
			this.nWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.nWebBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("nWebBrowser.OcxState")));
			this.nWebBrowser.Size = new System.Drawing.Size(920, 576);
			this.nWebBrowser.TabIndex = 0;
			// 
			// NewWelcome
			// 
			this.Controls.Add(this.nWebBrowser);
			this.Name = "NewWelcome";
			this.Size = new System.Drawing.Size(920, 576);
			((System.ComponentModel.ISupportInitialize)(this.nWebBrowser)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
