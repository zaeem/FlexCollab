using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using WMEncoderLib;
using WMPREVIEWLib;
using WMDEVICECONTROLLib;
using System.Threading;
using WebMeeting.Client.Alerts;
using System.Diagnostics;

namespace ScreenCapture
{
	/// <summary>
	/// Summary description for ScreenControl.
	/// </summary>
	public class ScreenControl : System.Windows.Forms.UserControl
	{
		public ScreenCapture screenCapture;
		
		private System.Windows.Forms.SaveFileDialog saveFileDialog;

		//		Thread Safety
//		Any public static (Shared in Visual Basic) members of this type are thread safe. Any instance members are not guaranteed to be thread safe

		private bool m_bFileSet;
		private System.Windows.Forms.Timer encoderTimer;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBoxRecording;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.Label m_currentFileSize;
		private System.Windows.Forms.Label m_currentFileDuration;
		private System.Windows.Forms.Label m_currentBitRate;
		private System.Windows.Forms.Label labelFileName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label_bitrate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox previewBox;
		public System.Windows.Forms.GroupBox recordingOptionGroup;
		private System.Windows.Forms.Button buttonPause;
		private System.Windows.Forms.Button recordingStopButton;
		private System.Windows.Forms.Button recordingStartButton;
		public System.Windows.Forms.GroupBox playbackGroup;
		private System.Windows.Forms.Button playbackStartButton;
		private System.Windows.Forms.Button playbackStopButton;
		private System.Windows.Forms.Button playbackPauseButton;
		private System.Windows.Forms.CheckBox fullScreenCheckBox;
		private System.Windows.Forms.Panel panelBottom;
		private System.ComponentModel.IContainer components;


	
		//***********************************************************************//
		
				
		public ScreenControl(bool openFileDialog)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			// open save file dialog
			saveFileDialog.ShowDialog(this);
			labelFileName.Text = saveFileDialog.FileName;
			screenCapture = new ScreenCapture(labelFileName.Text, ref encoderTimer);
		}

		
		public void ChangeState(bool bRecording)
		{
					
			groupBoxRecording.Visible = bRecording;
			previewBox.Visible = !bRecording;
			recordingOptionGroup.Visible =bRecording;
			playbackGroup.Visible  = !bRecording;
			fullScreenCheckBox.Visible =!bRecording;

		}


		#region Component Designer generated code
		public bool SetFile()
		{
        
			try
			{
				saveFileDialog.Filter = "Windows Media Video (*.wmv) | *.wmv";
				if(saveFileDialog.ShowDialog(this) == DialogResult.OK)
				{	
					labelFileName.Text = saveFileDialog.FileName;
					screenCapture = new ScreenCapture(labelFileName.Text,  ref encoderTimer);
					m_bFileSet = true;
					return true;
				}
				else
				{
					WebMeeting.Client.ClientUI.getInstance().panelRecordingToolbar.Visible = false;
					return false;
				}
			}
			catch ( Exception exp)
			{
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>Screen Control.cs line==> 416",exp,"Error Setting File: " + exp.Message.ToString(),true);			
				
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Setting File: " + exp.Message.ToString());			
				return false;
			}

		}

		#endregion 

		#region Contructor of Screen Control By Zaeem. 
		
		public ScreenControl()
		{
			InitializeComponent();
			this.m_bFileSet = false;
			this.recordingStopButton.Enabled=false;
			this.recordingStartButton.Enabled=true;
			this.buttonPause.Enabled=false;

		}

		#endregion

		# region Dispose Method Clean up any resources being used.
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
		# endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.encoderTimer = new System.Windows.Forms.Timer(this.components);
			this.label4 = new System.Windows.Forms.Label();
			this.groupBoxRecording = new System.Windows.Forms.GroupBox();
			this.label_bitrate = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelFileName = new System.Windows.Forms.Label();
			this.m_currentBitRate = new System.Windows.Forms.Label();
			this.m_currentFileDuration = new System.Windows.Forms.Label();
			this.labelStatus = new System.Windows.Forms.Label();
			this.m_currentFileSize = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.previewBox = new System.Windows.Forms.PictureBox();
			this.recordingOptionGroup = new System.Windows.Forms.GroupBox();
			this.buttonPause = new System.Windows.Forms.Button();
			this.recordingStopButton = new System.Windows.Forms.Button();
			this.recordingStartButton = new System.Windows.Forms.Button();
			this.playbackGroup = new System.Windows.Forms.GroupBox();
			this.playbackStartButton = new System.Windows.Forms.Button();
			this.playbackStopButton = new System.Windows.Forms.Button();
			this.playbackPauseButton = new System.Windows.Forms.Button();
			this.fullScreenCheckBox = new System.Windows.Forms.CheckBox();
			this.panelBottom = new System.Windows.Forms.Panel();
			this.groupBoxRecording.SuspendLayout();
			this.panel1.SuspendLayout();
			this.recordingOptionGroup.SuspendLayout();
			this.playbackGroup.SuspendLayout();
			this.panelBottom.SuspendLayout();
			this.SuspendLayout();
			// 
			// encoderTimer
			// 
			this.encoderTimer.Tick += new System.EventHandler(this.encoderTimer_Tick);
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(40, 32);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 16);
			this.label4.TabIndex = 12;
			this.label4.Text = "Status";
			this.label4.Click += new System.EventHandler(this.label4_Click);
			// 
			// groupBoxRecording
			// 
			this.groupBoxRecording.Controls.Add(this.label_bitrate);
			this.groupBoxRecording.Controls.Add(this.label3);
			this.groupBoxRecording.Controls.Add(this.label2);
			this.groupBoxRecording.Controls.Add(this.labelFileName);
			this.groupBoxRecording.Controls.Add(this.m_currentBitRate);
			this.groupBoxRecording.Controls.Add(this.m_currentFileDuration);
			this.groupBoxRecording.Controls.Add(this.label4);
			this.groupBoxRecording.Controls.Add(this.labelStatus);
			this.groupBoxRecording.Controls.Add(this.m_currentFileSize);
			this.groupBoxRecording.Controls.Add(this.label6);
			this.groupBoxRecording.Location = new System.Drawing.Point(32, 16);
			this.groupBoxRecording.Name = "groupBoxRecording";
			this.groupBoxRecording.Size = new System.Drawing.Size(448, 192);
			this.groupBoxRecording.TabIndex = 13;
			this.groupBoxRecording.TabStop = false;
			this.groupBoxRecording.Text = "Recording Details";
			// 
			// label_bitrate
			// 
			this.label_bitrate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label_bitrate.Location = new System.Drawing.Point(40, 128);
			this.label_bitrate.Name = "label_bitrate";
			this.label_bitrate.Size = new System.Drawing.Size(92, 16);
			this.label_bitrate.TabIndex = 18;
			this.label_bitrate.Text = "Bitrate";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(40, 96);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(92, 16);
			this.label3.TabIndex = 17;
			this.label3.Text = "File Duration";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(40, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 16);
			this.label2.TabIndex = 16;
			this.label2.Text = "File Size";
			// 
			// labelFileName
			// 
			this.labelFileName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelFileName.Location = new System.Drawing.Point(144, 160);
			this.labelFileName.Name = "labelFileName";
			this.labelFileName.Size = new System.Drawing.Size(280, 24);
			this.labelFileName.TabIndex = 15;
			// 
			// m_currentBitRate
			// 
			this.m_currentBitRate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_currentBitRate.Location = new System.Drawing.Point(144, 128);
			this.m_currentBitRate.Name = "m_currentBitRate";
			this.m_currentBitRate.Size = new System.Drawing.Size(112, 16);
			this.m_currentBitRate.TabIndex = 14;
			// 
			// m_currentFileDuration
			// 
			this.m_currentFileDuration.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_currentFileDuration.Location = new System.Drawing.Point(144, 96);
			this.m_currentFileDuration.Name = "m_currentFileDuration";
			this.m_currentFileDuration.Size = new System.Drawing.Size(112, 16);
			this.m_currentFileDuration.TabIndex = 13;
			// 
			// labelStatus
			// 
			this.labelStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelStatus.Location = new System.Drawing.Point(144, 32);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(152, 16);
			this.labelStatus.TabIndex = 12;
			this.labelStatus.Text = "Stopped";
			// 
			// m_currentFileSize
			// 
			this.m_currentFileSize.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_currentFileSize.Location = new System.Drawing.Point(144, 64);
			this.m_currentFileSize.Name = "m_currentFileSize";
			this.m_currentFileSize.Size = new System.Drawing.Size(112, 16);
			this.m_currentFileSize.TabIndex = 12;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(40, 160);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(92, 16);
			this.label6.TabIndex = 18;
			this.label6.Text = "File Name";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.groupBoxRecording);
			this.panel1.Controls.Add(this.previewBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(520, 376);
			this.panel1.TabIndex = 14;
			// 
			// previewBox
			// 
			this.previewBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.previewBox.BackColor = System.Drawing.SystemColors.ControlText;
			this.previewBox.Location = new System.Drawing.Point(0, 0);
			this.previewBox.Name = "previewBox";
			this.previewBox.Size = new System.Drawing.Size(648, 304);
			this.previewBox.TabIndex = 6;
			this.previewBox.TabStop = false;
			// 
			// recordingOptionGroup
			// 
			this.recordingOptionGroup.Controls.Add(this.buttonPause);
			this.recordingOptionGroup.Controls.Add(this.recordingStopButton);
			this.recordingOptionGroup.Controls.Add(this.recordingStartButton);
			this.recordingOptionGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.recordingOptionGroup.Location = new System.Drawing.Point(264, 8);
			this.recordingOptionGroup.Name = "recordingOptionGroup";
			this.recordingOptionGroup.Size = new System.Drawing.Size(208, 56);
			this.recordingOptionGroup.TabIndex = 2;
			this.recordingOptionGroup.TabStop = false;
			this.recordingOptionGroup.Text = "Recording options";
			// 
			// buttonPause
			// 
			this.buttonPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonPause.Location = new System.Drawing.Point(80, 24);
			this.buttonPause.Name = "buttonPause";
			this.buttonPause.Size = new System.Drawing.Size(48, 23);
			this.buttonPause.TabIndex = 3;
			this.buttonPause.Text = "Pause";
			this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
			// 
			// recordingStopButton
			// 
			this.recordingStopButton.Enabled = false;
			this.recordingStopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.recordingStopButton.Location = new System.Drawing.Point(144, 24);
			this.recordingStopButton.Name = "recordingStopButton";
			this.recordingStopButton.Size = new System.Drawing.Size(48, 23);
			this.recordingStopButton.TabIndex = 2;
			this.recordingStopButton.Text = "Stop";
			this.recordingStopButton.Click += new System.EventHandler(this.recordingStopButton_Click);
			// 
			// recordingStartButton
			// 
			this.recordingStartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.recordingStartButton.Location = new System.Drawing.Point(16, 24);
			this.recordingStartButton.Name = "recordingStartButton";
			this.recordingStartButton.Size = new System.Drawing.Size(48, 23);
			this.recordingStartButton.TabIndex = 0;
			this.recordingStartButton.Text = "Start";
			this.recordingStartButton.Click += new System.EventHandler(this.recordingStartButton_Click);
			// 
			// playbackGroup
			// 
			this.playbackGroup.Controls.Add(this.playbackStartButton);
			this.playbackGroup.Controls.Add(this.playbackStopButton);
			this.playbackGroup.Controls.Add(this.playbackPauseButton);
			this.playbackGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.playbackGroup.Location = new System.Drawing.Point(64, 8);
			this.playbackGroup.Name = "playbackGroup";
			this.playbackGroup.Size = new System.Drawing.Size(208, 56);
			this.playbackGroup.TabIndex = 3;
			this.playbackGroup.TabStop = false;
			this.playbackGroup.Text = "Playback options";
			// 
			// playbackStartButton
			// 
			this.playbackStartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.playbackStartButton.Location = new System.Drawing.Point(16, 24);
			this.playbackStartButton.Name = "playbackStartButton";
			this.playbackStartButton.Size = new System.Drawing.Size(48, 23);
			this.playbackStartButton.TabIndex = 3;
			this.playbackStartButton.Text = "Start";
			this.playbackStartButton.Click += new System.EventHandler(this.playbackStartButton_Click);
			// 
			// playbackStopButton
			// 
			this.playbackStopButton.Enabled = false;
			this.playbackStopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.playbackStopButton.Location = new System.Drawing.Point(144, 24);
			this.playbackStopButton.Name = "playbackStopButton";
			this.playbackStopButton.Size = new System.Drawing.Size(48, 23);
			this.playbackStopButton.TabIndex = 4;
			this.playbackStopButton.Text = "Stop";
			this.playbackStopButton.Click += new System.EventHandler(this.playbackStopButton_Click);
			// 
			// playbackPauseButton
			// 
			this.playbackPauseButton.Enabled = false;
			this.playbackPauseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.playbackPauseButton.Location = new System.Drawing.Point(80, 24);
			this.playbackPauseButton.Name = "playbackPauseButton";
			this.playbackPauseButton.Size = new System.Drawing.Size(48, 23);
			this.playbackPauseButton.TabIndex = 3;
			this.playbackPauseButton.Text = "Pause";
			this.playbackPauseButton.Click += new System.EventHandler(this.playbackPauseButton_Click);
			// 
			// fullScreenCheckBox
			// 
			this.fullScreenCheckBox.BackColor = System.Drawing.SystemColors.Control;
			this.fullScreenCheckBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.fullScreenCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
			this.fullScreenCheckBox.Location = new System.Drawing.Point(368, 24);
			this.fullScreenCheckBox.Name = "fullScreenCheckBox";
			this.fullScreenCheckBox.Size = new System.Drawing.Size(80, 16);
			this.fullScreenCheckBox.TabIndex = 1;
			this.fullScreenCheckBox.Text = "Full Screen";
			this.fullScreenCheckBox.CheckedChanged += new System.EventHandler(this.fullScreenCheckBox_CheckedChanged);
			// 
			// panelBottom
			// 
			this.panelBottom.Controls.Add(this.fullScreenCheckBox);
			this.panelBottom.Controls.Add(this.playbackGroup);
			this.panelBottom.Controls.Add(this.recordingOptionGroup);
			this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelBottom.Location = new System.Drawing.Point(0, 304);
			this.panelBottom.Name = "panelBottom";
			this.panelBottom.Size = new System.Drawing.Size(520, 72);
			this.panelBottom.TabIndex = 15;
			this.panelBottom.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBottom_Paint);
			// 
			// ScreenControl
			// 
			this.Controls.Add(this.panelBottom);
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "ScreenControl";
			this.Size = new System.Drawing.Size(520, 376);
			this.groupBoxRecording.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.recordingOptionGroup.ResumeLayout(false);
			this.playbackGroup.ResumeLayout(false);
			this.panelBottom.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void fullScreenCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			if(fullScreenCheckBox.Checked==true)
			{
				try 
				{
					this.screenCapture.FullScreen();
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>Screen Control.cs line==> 455",exp,"Full Screen Exception :" + exp.Message.ToString(),true);			
				
					//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
					//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Full Screen Exception: " + exp.Message.ToString());			
				}
				fullScreenCheckBox.Checked=false;
			}
		}
		bool bThreadCaptureStarted = false;
        
		
		# region RecordingThreadFunction 
		public void recordingThreadFunction()
		{
			try 
			{
				bThreadCaptureStarted= true;
				this.screenCapture.StartEncoder();
				
				//labelStatus.Text = "Recording...";
				//recordingStartButton.Enabled=false;
				//this.recordingStopButton.Enabled=true;
				//this.buttonPause.Enabled=true;

				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>Screen Control.cs line==> 483",exp,"Error Starting encoder : " + exp.Message.ToString(),true);			
				
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Starting encoder : " + exp.Message.ToString());			
				
				
			}
		}

		 #endregion


		Thread thRecording;
		public bool start()
		{
			try
			{
				if(!SetFile())
					return false;
				else
				{
					//recordingThreadFunction();
					thRecording=new Thread(new ThreadStart(recordingThreadFunction));
					thRecording.Name = "Screen Capture Control Thread: recordingThreadFunction()";	
					thRecording.Start();
					return true;
				}
			}
			catch(Exception exp)
			{
				
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>Screen Control.cs line==> 513",exp,"Recording couldn't be stated du to : " + exp.Message.ToString(),true);			
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Recording couldn't be stated " + exp.StackTrace.ToString());
				thRecording.Abort();
				return false;
			}
		}

		public void stop()
		{

			try 
			{	
				if(bThreadCaptureStarted)
				{
					this.screenCapture.StopEncoder();

					screenCapture = null; //rec1
				
					labelStatus.Text = "recording stopped";
					this.recordingStopButton.Enabled=false;
					this.recordingStartButton.Enabled=true;
					if(thRecording != null) //rec1
					{
						thRecording.Abort();
					}

					

				}
				bThreadCaptureStarted = false;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>Screen Control.cs line==> 548",exp,"Error Stoping Encoder : " + exp.Message.ToString(),true);			
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error stopping encoder: " + exp.ToString());
			}
			finally
			{
			 thRecording = null;
			}
		}
		public void pause()
		{
			if(this.labelStatus.Text=="paused")
			{
				try 
				{
					this.screenCapture.StartEncoder();//.PauseEncoder();
					labelStatus.Text = "recording";				
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>Screen Control.cs line==> 568",exp,"Error Pausing Encoder : " + exp.Message.ToString(),true);			
				
					//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Pausing: " + exp.Message.ToString());			
					//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
				}					
			}
			else
			{
				try 
				{
					this.screenCapture.PauseEncoder();
					labelStatus.Text = "paused";				
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>Screen Control.cs line==> 583",exp,"Error Pausing Encoder : " + exp.Message.ToString(),true);			
				
					//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Pausing: " + exp.Message.ToString());			
					//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
					
				}
			}

		}
		private void recordingStartButton_Click(object sender, System.EventArgs e)
		{
			//ShowLabels();
			// check whether the file has been set or not
			
			start();

			
		}

		private void recordingStopButton_Click(object sender, System.EventArgs e)
		{
			// stop screen capture.
			stop();
		}

		private void playbackStartButton_Click(object sender, System.EventArgs e)
		{
			// hide labels before playback
			HideLabels();

			try 
			{
				System.Windows.Forms.OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Windows Media Video (*.wmv) | *.wmv";
				if(openFileDialog.ShowDialog() == DialogResult.OK)
				{				
					screenCapture = new ScreenCapture();
					this.screenCapture.Playback(openFileDialog.FileName, previewBox);
					labelStatus.Text = "playback...";
					this.playbackPauseButton.Enabled=true;
					this.playbackStartButton.Enabled=false;
					this.playbackStopButton.Enabled=true;
				}
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Screen Control.cs line==> 629",exp,"Error in Playback : " + exp.Message.ToString(),true);			
				
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Play back: " + exp.Message.ToString());			
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
			}
		}

		private void playbackPauseButton_Click(object sender, System.EventArgs e)
		{
		

			try 
			{
				this.screenCapture.PausePlayback();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Screen Control.cs line==> 646",exp,"Error in  Pause & Playback : " + exp.Message.ToString(),true);			
				
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Pausing: " + exp.Message.ToString());			
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
			}
		
		
		}
		public void stopRecording()
		{
			try
			{
				screenCapture.StopEncoder();

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recording ===>Screen Control.cs line==> 663",exp,"Error Stoping Encoder : " + exp.Message.ToString(),true);			
				
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Stoping: " + exp.Message.ToString());			
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
		
			}
		}
		private void playbackStopButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.screenCapture.StopPlayback();
				labelStatus.Text = "playback stopped";
				this.playbackStartButton.Enabled=true;
				this.playbackStopButton.Enabled=false;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recording ===>Screen Control.cs line==> 681",exp,"Error in Play back Stop Encoder : " + exp.Message.ToString(),true);			
				
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Play back: " + exp.Message.ToString());			
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
				
			}	
		}

		private void buttonPause_Click(object sender, System.EventArgs e)
		{
			pause();
		}

		public void HideLabels()
		{
			label2.Visible				= false;
			label3.Visible				= false;
			label_bitrate.Visible		= false;

			m_currentFileSize.Visible		= false;
			m_currentFileDuration.Visible	= false;
			m_currentBitRate.Visible		= false;
		}

		public void ShowLabels()
		{
			label2.Visible			= true;
			label3.Visible			= true;
			label_bitrate.Visible	= true;

			m_currentFileSize.Visible		= true;
			m_currentFileDuration.Visible	= true;
			m_currentBitRate.Visible		= true;
		}

		private void encoderTimer_Tick(object sender, System.EventArgs e)
		{
			try 
			{
				// retrive statistics object
				IWMEncStatistics stats= this.screenCapture.m_encoder.Statistics;

				// retrive an IWMEncFileArchiveStats objects
				IWMEncFileArchiveStats fileStats = 
					(IWMEncFileArchiveStats) stats.FileArchiveStats;

				// display file size and duration
				decimal fileSize = fileStats.FileSize*10;
				decimal fileDuration = fileStats.FileDuration*10;

				// display this on certain labels.
				m_currentFileSize.Text = fileSize.ToString() + " Kb";
				m_currentFileDuration.Text = fileDuration.ToString() + " seconds";

				// retrive IWMEncOutputStats object and display current bitrate
				IWMEncOutputStats outputStats = (IWMEncOutputStats) stats.WMFOutputStats;
				decimal bitRate = outputStats.CurrentBitrate/1000;

				// display current bitrate
				m_currentBitRate.Text = bitRate.ToString() + " Kbps";

			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recording ===>Screen Control.cs line==> 745",exp,"Error in Timer Status : " + exp.Message.ToString(),true);			
				
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("rror in stats timer: " + exp.Message.ToString());			
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
				
			}
		}

		private void label4_Click(object sender, System.EventArgs e)
		{
		
		}

		private void panelBottom_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}
	}
}
