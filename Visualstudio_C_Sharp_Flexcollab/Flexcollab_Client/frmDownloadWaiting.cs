using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for frmDownloadWaiting.
	/// </summary>
	public class frmDownloadWaiting : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar progressBar1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		Thread progressThread ;
		public frmDownloadWaiting()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			progressThread = new Thread( new ThreadStart(showProgressThread));
			progressThread.Name = "Show progress to download file: showProgressThread()";
			progressThread.Start();

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
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.progressBar1.Location = new System.Drawing.Point(48, 32);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(280, 23);
			this.progressBar1.Step = 1;
			this.progressBar1.TabIndex = 0;
			// 
			// frmDownloadWaiting
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.ClientSize = new System.Drawing.Size(376, 94);
			this.ControlBox = false;
			this.Controls.Add(this.progressBar1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(384, 128);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(384, 128);
			this.Name = "frmDownloadWaiting";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.TopMost = true;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmDownloadWaiting_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		public void showProgressThread()
		{
			try 
			{
				while(true)
				{					
					this.progressBar1.Value += 1;
					
					if (this.progressBar1.Value > 99)
					{
						this.progressBar1.Value = 0;
					}
					Thread.Sleep(20);
				}
			}
			catch(Exception ex)
			{
				ex=ex;
			}
		}

		private void frmDownloadWaiting_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				progressThread.Abort();
			}
			catch(Exception ex)
			{
				ex=ex;				   
			}
		}
		public void closeDownloadForm()
		{
			frmDownloadWaiting_Closing(null,null);
		}
	}
}
