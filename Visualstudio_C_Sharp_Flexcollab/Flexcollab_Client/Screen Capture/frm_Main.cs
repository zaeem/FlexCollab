using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices; 


namespace WebMeeting.Client.Screen_Capture
{
	/// <summary>
	/// Summary description for frm_Main.
	/// </summary>

	//*************************************************************************//
	//                        By Zaeem	
	//*************************************************************************//
	
	public class frm_Rec : System.Windows.Forms.Form
	{
		# region Generic Declerations
		public System.Windows.Forms.Button btn_Rec;
		public System.Windows.Forms.Button btn_Pause;
		public System.Windows.Forms.Button btn_Stop;
		public System.Windows.Forms.Button btn_Config;
		private System.Windows.Forms.Button btn_Set;
		public ScreenCapture.ScreenControl sc;
		private System.Windows.Forms.Button btn_Performance;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;
		private WebMeeting.Client.advertiseBrowser advertiseBrowser1;
		# endregion 

		#region Contuructor of Recording Form By Zaeem 
		public frm_Rec()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			btn_Pause.Enabled=false;
			btn_Stop.Enabled=false;
			btn_Rec.Enabled=true;
			sc=new ScreenCapture.ScreenControl();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		#endregion
		
		#region Dispose Method for cleaning resources
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


		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frm_Rec));
			this.btn_Rec = new System.Windows.Forms.Button();
			this.btn_Pause = new System.Windows.Forms.Button();
			this.btn_Stop = new System.Windows.Forms.Button();
			this.btn_Config = new System.Windows.Forms.Button();
			this.btn_Set = new System.Windows.Forms.Button();
			this.btn_Performance = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.advertiseBrowser1 = new WebMeeting.Client.advertiseBrowser();
			this.SuspendLayout();
			// 
			// btn_Rec
			// 
			this.btn_Rec.Image = ((System.Drawing.Image)(resources.GetObject("btn_Rec.Image")));
			this.btn_Rec.Location = new System.Drawing.Point(8, 8);
			this.btn_Rec.Name = "btn_Rec";
			this.btn_Rec.Size = new System.Drawing.Size(32, 23);
			this.btn_Rec.TabIndex = 0;
			this.toolTip1.SetToolTip(this.btn_Rec, "Record");
			this.btn_Rec.Click += new System.EventHandler(this.btn_Rec_Click);
			// 
			// btn_Pause
			// 
			this.btn_Pause.Image = ((System.Drawing.Image)(resources.GetObject("btn_Pause.Image")));
			this.btn_Pause.Location = new System.Drawing.Point(40, 8);
			this.btn_Pause.Name = "btn_Pause";
			this.btn_Pause.Size = new System.Drawing.Size(32, 23);
			this.btn_Pause.TabIndex = 1;
			this.toolTip1.SetToolTip(this.btn_Pause, "Pause");
			this.btn_Pause.Click += new System.EventHandler(this.btn_Pause_Click);
			// 
			// btn_Stop
			// 
			this.btn_Stop.Image = ((System.Drawing.Image)(resources.GetObject("btn_Stop.Image")));
			this.btn_Stop.Location = new System.Drawing.Point(72, 8);
			this.btn_Stop.Name = "btn_Stop";
			this.btn_Stop.Size = new System.Drawing.Size(32, 23);
			this.btn_Stop.TabIndex = 2;
			this.toolTip1.SetToolTip(this.btn_Stop, "Stop");
			this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
			// 
			// btn_Config
			// 
			this.btn_Config.Image = ((System.Drawing.Image)(resources.GetObject("btn_Config.Image")));
			this.btn_Config.Location = new System.Drawing.Point(128, 8);
			this.btn_Config.Name = "btn_Config";
			this.btn_Config.Size = new System.Drawing.Size(32, 23);
			this.btn_Config.TabIndex = 3;
			this.toolTip1.SetToolTip(this.btn_Config, "Set and Save your Streaming Configurations");
			this.btn_Config.Click += new System.EventHandler(this.btn_Config_Click);
			// 
			// btn_Set
			// 
			this.btn_Set.Image = ((System.Drawing.Image)(resources.GetObject("btn_Set.Image")));
			this.btn_Set.Location = new System.Drawing.Point(160, 8);
			this.btn_Set.Name = "btn_Set";
			this.btn_Set.Size = new System.Drawing.Size(32, 23);
			this.btn_Set.TabIndex = 4;
			this.toolTip1.SetToolTip(this.btn_Set, "Set Audio Configuration");
			this.btn_Set.Click += new System.EventHandler(this.btn_Set_Click);
			// 
			// btn_Performance
			// 
			this.btn_Performance.Image = ((System.Drawing.Image)(resources.GetObject("btn_Performance.Image")));
			this.btn_Performance.Location = new System.Drawing.Point(192, 8);
			this.btn_Performance.Name = "btn_Performance";
			this.btn_Performance.Size = new System.Drawing.Size(32, 23);
			this.btn_Performance.TabIndex = 5;
			this.toolTip1.SetToolTip(this.btn_Performance, "Performance Tips");
			this.btn_Performance.Click += new System.EventHandler(this.btn_Performance_Click);
			// 
			// advertiseBrowser1
			// 
			this.advertiseBrowser1.Location = new System.Drawing.Point(56, 72);
			this.advertiseBrowser1.Name = "advertiseBrowser1";
			this.advertiseBrowser1.Size = new System.Drawing.Size(240, 160);
			this.advertiseBrowser1.TabIndex = 6;
			// 
			// frm_Rec
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(232, 38);
			this.Controls.Add(this.advertiseBrowser1);
			this.Controls.Add(this.btn_Performance);
			this.Controls.Add(this.btn_Set);
			this.Controls.Add(this.btn_Config);
			this.Controls.Add(this.btn_Stop);
			this.Controls.Add(this.btn_Pause);
			this.Controls.Add(this.btn_Rec);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frm_Rec";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Recorder Panel";
			this.toolTip1.SetToolTip(this, "Recording Control");
			this.TransparencyKey = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.Closed += new System.EventHandler(this.btn_Stop_Click);
			this.ResumeLayout(false);

		}
		#endregion

		#region For controlling the click event of recording button
		private void btn_Rec_Click(object sender, System.EventArgs e)
		{
		
			try
			{
				if(sc.start())
				{
					btn_Rec.Enabled=false;
					btn_Pause.Enabled=true;
					btn_Stop.Enabled=true;		
				}
				else
				{
					btn_Rec.Enabled=true;
					btn_Pause.Enabled=false;
					btn_Stop.Enabled=false;		
				}
			}
			
			catch(Exception exp)
			{
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);

				if( MessageBox.Show("You do not have required components of Windows Media Encoder. Encoder caused error: "+ /*+exp.Message.ToString() + */". Do you want to download now?","WebMeeting",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.Yes)
				{
					Win32.Shell32.ShellExecute(0,"Open",Info.getInstance().WebsiteName + "/wmencoder.exe","","",1);				
				}
				
				btn_Rec.Enabled=true;
				btn_Pause.Enabled=false;
				btn_Stop.Enabled=false;		
				this.Close();
			}
			
		}
		#endregion

		#region For controlling the click event of Pause button
		private void btn_Pause_Click(object sender, System.EventArgs e)
		{
			try
			{
				sc.pause();
				btn_Rec.Enabled=false;
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>frm_Main.cs line==> 226",exp,"Error Pausing Encoder: " + exp.Message.ToString(),true);			
			//	Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);
			//	WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Pausing Encoder: " + exp.Message.ToString());			
			}
		}
		#endregion
		
		#region For controlling the click event of Pause button
		private void btn_Stop_Click(object sender, System.EventArgs e)
			{
			try
			{
				sc.stop();
				btn_Rec.Enabled=true;
				btn_Pause.Enabled=false;
				btn_Stop.Enabled=false;		
		    }
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>frm_Main.cs line==> 245",exp,"Error Stoping Encoder: " + exp.Message.ToString(),true);			
			//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
			//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Stoping encoder: " + exp.Message.ToString());			
			}
		}
		#endregion 
		
		#region For runnig Audio configuration 
		private void btn_Set_Click(object sender, System.EventArgs e)
		{
			try
			{
				//string str =Application.StartupPath+"\\"+"MixerTest.exe"; 
				//Process.Start("E:\\MainProgramme\\bin\\Release\\MixerTest.exe");			
				//Process.Start(str);
				Process.Start("sndvol32.exe");
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>frm_Main.cs line==> 264",exp,"Error excuting process of setting Audio properties: " + exp.Message.ToString(),true);			
			
			//	Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);
			//	WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error excuting process of setting Audio properties: " + exp.Message.ToString());			
			}
		}
		#endregion

		# region For Setting and Configuring the Audio Properties

		private void btn_Config_Click(object sender, System.EventArgs e)
		{
			frm_AudioConfig frmcon=new frm_AudioConfig();
			frmcon.Visible=true;
		}
		
				private void btn_Performance_Click(object sender, System.EventArgs e)
		{
			frmPerformance frmper=new frmPerformance();
			frmper.Show();
		}

		# endregion


		
	}
}
