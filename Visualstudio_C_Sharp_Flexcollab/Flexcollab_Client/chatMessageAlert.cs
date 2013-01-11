using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for chatMessageAlert.
	/// </summary>
	public class chatMessageAlert : System.Windows.Forms.Form
	{
		private System.Windows.Forms.LinkLabel linkCheckMessageAlert;
		private System.Windows.Forms.CheckBox chkIsSeeNextTimeMessageAlert;
		private System.Windows.Forms.Button btnEsc;
		private System.Windows.Forms.Timer timerSplitterChecker;
		private System.ComponentModel.IContainer components;

		# region chatMessageAlert()  //contructor
		public chatMessageAlert()
		{
			//
			// Required for Windows Form Designer support
			//
			//this.Location = new Point(500,600);
			

			InitializeComponent();
			timerSplitterChecker.Start();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		#endregion 



//		public chatMessageAlert(int x, int y)
//		{
//			//chatMessageAlert.Location.X = x;
//			//chatMessageAlert.Location.Y = y;
//		}

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
			this.components = new System.ComponentModel.Container();
			this.chkIsSeeNextTimeMessageAlert = new System.Windows.Forms.CheckBox();
			this.linkCheckMessageAlert = new System.Windows.Forms.LinkLabel();
			this.btnEsc = new System.Windows.Forms.Button();
			this.timerSplitterChecker = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// chkIsSeeNextTimeMessageAlert
			// 
			this.chkIsSeeNextTimeMessageAlert.ForeColor = System.Drawing.Color.White;
			this.chkIsSeeNextTimeMessageAlert.Location = new System.Drawing.Point(16, 40);
			this.chkIsSeeNextTimeMessageAlert.Name = "chkIsSeeNextTimeMessageAlert";
			this.chkIsSeeNextTimeMessageAlert.Size = new System.Drawing.Size(208, 24);
			this.chkIsSeeNextTimeMessageAlert.TabIndex = 1;
			this.chkIsSeeNextTimeMessageAlert.Text = "I do not want to see Chat Message alert window next time";
			this.chkIsSeeNextTimeMessageAlert.CheckedChanged += new System.EventHandler(this.chkIsSeeNextTimeMessageAlert_CheckedChanged);
			// 
			// linkCheckMessageAlert
			// 
			this.linkCheckMessageAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.linkCheckMessageAlert.LinkColor = System.Drawing.Color.Blue;
			this.linkCheckMessageAlert.Location = new System.Drawing.Point(16, 16);
			this.linkCheckMessageAlert.Name = "linkCheckMessageAlert";
			this.linkCheckMessageAlert.Size = new System.Drawing.Size(216, 23);
			this.linkCheckMessageAlert.TabIndex = 3;
			this.linkCheckMessageAlert.TabStop = true;
			this.linkCheckMessageAlert.Text = "See New Message (s) in chat box";
			this.linkCheckMessageAlert.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCheckMessageAlert_LinkClicked);
			// 
			// btnEsc
			// 
			this.btnEsc.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnEsc.Location = new System.Drawing.Point(256, 8);
			this.btnEsc.Name = "btnEsc";
			this.btnEsc.Size = new System.Drawing.Size(0, 0);
			this.btnEsc.TabIndex = 4;
			this.btnEsc.Click += new System.EventHandler(this.btnEsc_Click);
			// 
			// timerSplitterChecker
			// 
			this.timerSplitterChecker.Interval = 400;
			this.timerSplitterChecker.Tick += new System.EventHandler(this.timerSplitterChecker_Tick);
			// 
			// chatMessageAlert
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(250)), ((System.Byte)(162)), ((System.Byte)(37)));
			this.CancelButton = this.btnEsc;
			this.ClientSize = new System.Drawing.Size(238, 88);
			this.ControlBox = false;
			this.Controls.Add(this.btnEsc);
			this.Controls.Add(this.linkCheckMessageAlert);
			this.Controls.Add(this.chkIsSeeNextTimeMessageAlert);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Location = new System.Drawing.Point(400, 400);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(224, 90);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(224, 90);
			this.Name = "chatMessageAlert";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.chatMessageAlert_Closing);
			this.ResumeLayout(false);

		}
		#endregion
		
		public static  chatMessageAlert  thisInstance;
		public static chatMessageAlert getInstance()
		{
			if(thisInstance == null)
			{				
				thisInstance = new chatMessageAlert();
				//thisInstance.profile = new ClientProfile();	
			}
			return thisInstance;
		}

		private void linkCheckMessageAlert_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			Client.ClientUI.getInstance().openRightWindow();
//			for(double i=100.0 ; i > 0.0 ;)
//			{
//				this.Opacity = i;
//				i = i - 0.05;
//			}
			this.Close();
		}

		private void chkIsSeeNextTimeMessageAlert_CheckedChanged(object sender, System.EventArgs e)
		{
			if( chkIsSeeNextTimeMessageAlert.Checked )
				Client.ClientUI.getInstance().bIsSeeNextTimeMessageAlert = false;
			else
				Client.ClientUI.getInstance().bIsSeeNextTimeMessageAlert = true;

		}

		private void btnEsc_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void chatMessageAlert_Closing(object sender, CancelEventArgs e)
		{
			//true this dialog to run dialog for next time
			Client.ClientUI.getInstance().isChatAlreadyShown=false;
		}

		private void timerSplitterChecker_Tick(object sender, EventArgs e)
		{
			if(!Client.ClientUI.getInstance().splitter1Closed)
			{
				this.timerSplitterChecker.Stop();
				this.Close();
			}
		}
	}
}
