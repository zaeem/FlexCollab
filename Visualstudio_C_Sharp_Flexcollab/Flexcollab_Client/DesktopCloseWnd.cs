using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for DesktopCloseWnd.
	/// </summary>
	public class DesktopCloseWnd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.LinkLabel btnCloseDeskShare;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DesktopCloseWnd()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			// TODO: Add any constructor code after InitializeComponent call
			this.BringToFront();
			//
		}
		~DesktopCloseWnd()
		{
		
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnCloseDeskShare = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// btnCloseDeskShare
			// 
			this.btnCloseDeskShare.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
			this.btnCloseDeskShare.LinkColor = System.Drawing.Color.White;
			this.btnCloseDeskShare.Location = new System.Drawing.Point(9, 12);
			this.btnCloseDeskShare.Name = "btnCloseDeskShare";
			this.btnCloseDeskShare.Size = new System.Drawing.Size(150, 17);
			this.btnCloseDeskShare.TabIndex = 0;
			this.btnCloseDeskShare.TabStop = true;
			this.btnCloseDeskShare.Text = "Close Desktop Sharing";
			this.btnCloseDeskShare.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnCloseDeskShare_LinkClicked);
			// 
			// DesktopCloseWnd
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.ClientSize = new System.Drawing.Size(168, 40);
			this.ControlBox = false;
			this.Controls.Add(this.btnCloseDeskShare);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(168, 40);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(168, 40);
			this.Name = "DesktopCloseWnd";
			this.Opacity = 0.65;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		#endregion

		private void btnCloseDeskShare_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			ClientUI.getInstance().CloseEntireDeskHostToClient(true);
			this.Close();
		}
		public void closeDesktopSharingWnd()
		{
			this.Close();
		}
		
	}
}
