using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for AccessCodes.
	/// </summary>
	public class AccessCodesForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.Button btnUpdate;
		public System.Windows.Forms.Button button1;
		public System.Windows.Forms.CheckBox chkVideo;
		public System.Windows.Forms.CheckBox chkAudio;
		public System.Windows.Forms.CheckBox chkScreen;
		public System.Windows.Forms.CheckBox chkPresentation;
		public System.Windows.Forms.CheckBox chkDrawing;
		public System.Windows.Forms.CheckBox chkApplication;
		public System.Windows.Forms.CheckBox chkDesktop;
		private System.Windows.Forms.GroupBox groupBox2;		
		public System.Windows.Forms.RadioButton AttendeeRadio;
		public System.Windows.Forms.RadioButton PresenterRadio;
		private System.Windows.Forms.Button button2;
			
		public System.Drawing.Color	color;
		public System.Windows.Forms.CheckBox chkPolling;
		public System.Windows.Forms.CheckBox chkPvtChat;
		public System.Windows.Forms.CheckBox chkAssignRights;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AccessCodesForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AccessCodesForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkAssignRights = new System.Windows.Forms.CheckBox();
			this.chkPvtChat = new System.Windows.Forms.CheckBox();
			this.chkPolling = new System.Windows.Forms.CheckBox();
			this.button2 = new System.Windows.Forms.Button();
			this.chkScreen = new System.Windows.Forms.CheckBox();
			this.chkPresentation = new System.Windows.Forms.CheckBox();
			this.chkDrawing = new System.Windows.Forms.CheckBox();
			this.chkApplication = new System.Windows.Forms.CheckBox();
			this.chkDesktop = new System.Windows.Forms.CheckBox();
			this.chkAudio = new System.Windows.Forms.CheckBox();
			this.chkVideo = new System.Windows.Forms.CheckBox();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.PresenterRadio = new System.Windows.Forms.RadioButton();
			this.AttendeeRadio = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkAssignRights);
			this.groupBox1.Controls.Add(this.chkPvtChat);
			this.groupBox1.Controls.Add(this.chkPolling);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.chkScreen);
			this.groupBox1.Controls.Add(this.chkPresentation);
			this.groupBox1.Controls.Add(this.chkDrawing);
			this.groupBox1.Controls.Add(this.chkApplication);
			this.groupBox1.Controls.Add(this.chkDesktop);
			this.groupBox1.Controls.Add(this.chkAudio);
			this.groupBox1.Controls.Add(this.chkVideo);
			this.groupBox1.Location = new System.Drawing.Point(7, 80);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(293, 358);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Select access codes";
			this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
			// 
			// chkAssignRights
			// 
			this.chkAssignRights.Enabled = false;
			this.chkAssignRights.Location = new System.Drawing.Point(30, 323);
			this.chkAssignRights.Name = "chkAssignRights";
			this.chkAssignRights.Size = new System.Drawing.Size(242, 24);
			this.chkAssignRights.TabIndex = 13;
			this.chkAssignRights.Text = "Allow Him To Assign Rights for Others";
			// 
			// chkPvtChat
			// 
			this.chkPvtChat.Enabled = false;
			this.chkPvtChat.Location = new System.Drawing.Point(30, 293);
			this.chkPvtChat.Name = "chkPvtChat";
			this.chkPvtChat.Size = new System.Drawing.Size(167, 24);
			this.chkPvtChat.TabIndex = 12;
			this.chkPvtChat.Text = "Allow Private Chat";
			// 
			// chkPolling
			// 
			this.chkPolling.Enabled = false;
			this.chkPolling.Location = new System.Drawing.Point(30, 262);
			this.chkPolling.Name = "chkPolling";
			this.chkPolling.Size = new System.Drawing.Size(167, 24);
			this.chkPolling.TabIndex = 11;
			this.chkPolling.Text = "Allow Poll Posting";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(220, 161);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(36, 26);
			this.button2.TabIndex = 10;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// chkScreen
			// 
			this.chkScreen.Enabled = false;
			this.chkScreen.Location = new System.Drawing.Point(30, 194);
			this.chkScreen.Name = "chkScreen";
			this.chkScreen.Size = new System.Drawing.Size(138, 24);
			this.chkScreen.TabIndex = 9;
			this.chkScreen.Text = "Allow Screen Sharing";
			// 
			// chkPresentation
			// 
			this.chkPresentation.Enabled = false;
			this.chkPresentation.Location = new System.Drawing.Point(30, 228);
			this.chkPresentation.Name = "chkPresentation";
			this.chkPresentation.Size = new System.Drawing.Size(167, 24);
			this.chkPresentation.TabIndex = 8;
			this.chkPresentation.Text = "Allow Presentation Sharing";
			// 
			// chkDrawing
			// 
			this.chkDrawing.Enabled = false;
			this.chkDrawing.Location = new System.Drawing.Point(30, 160);
			this.chkDrawing.Name = "chkDrawing";
			this.chkDrawing.Size = new System.Drawing.Size(96, 24);
			this.chkDrawing.TabIndex = 7;
			this.chkDrawing.Text = "Allow Drawing";
			this.chkDrawing.Click += new System.EventHandler(this.button2_Click);
			// 
			// chkApplication
			// 
			this.chkApplication.Enabled = false;
			this.chkApplication.Location = new System.Drawing.Point(30, 126);
			this.chkApplication.Name = "chkApplication";
			this.chkApplication.Size = new System.Drawing.Size(166, 24);
			this.chkApplication.TabIndex = 6;
			this.chkApplication.Text = "Allow Application Sharing";
			// 
			// chkDesktop
			// 
			this.chkDesktop.Enabled = false;
			this.chkDesktop.Location = new System.Drawing.Point(30, 92);
			this.chkDesktop.Name = "chkDesktop";
			this.chkDesktop.Size = new System.Drawing.Size(138, 24);
			this.chkDesktop.TabIndex = 5;
			this.chkDesktop.Text = "Allow Remote Control";
			// 
			// chkAudio
			// 
			this.chkAudio.Enabled = false;
			this.chkAudio.Location = new System.Drawing.Point(30, 58);
			this.chkAudio.Name = "chkAudio";
			this.chkAudio.Size = new System.Drawing.Size(138, 24);
			this.chkAudio.TabIndex = 4;
			this.chkAudio.Text = "Allow Audio Sharing";
			// 
			// chkVideo
			// 
			this.chkVideo.Enabled = false;
			this.chkVideo.Location = new System.Drawing.Point(30, 24);
			this.chkVideo.Name = "chkVideo";
			this.chkVideo.Size = new System.Drawing.Size(138, 24);
			this.chkVideo.TabIndex = 3;
			this.chkVideo.Text = "Allow Video Sharing";
			this.chkVideo.Click += new System.EventHandler(this.PresenterRadio_CheckedChanged);
			// 
			// btnUpdate
			// 
			this.btnUpdate.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnUpdate.Location = new System.Drawing.Point(132, 443);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.TabIndex = 1;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Location = new System.Drawing.Point(220, 442);
			this.button1.Name = "button1";
			this.button1.TabIndex = 2;
			this.button1.Text = "Cancel";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.PresenterRadio);
			this.groupBox2.Controls.Add(this.AttendeeRadio);
			this.groupBox2.Location = new System.Drawing.Point(7, 9);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(294, 65);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Select user role";
			// 
			// PresenterRadio
			// 
			this.PresenterRadio.Location = new System.Drawing.Point(153, 26);
			this.PresenterRadio.Name = "PresenterRadio";
			this.PresenterRadio.TabIndex = 12;
			this.PresenterRadio.Text = "Presenter";
			this.PresenterRadio.CheckedChanged += new System.EventHandler(this.PresenterRadio_CheckedChanged);
			// 
			// AttendeeRadio
			// 
			this.AttendeeRadio.Location = new System.Drawing.Point(33, 26);
			this.AttendeeRadio.Name = "AttendeeRadio";
			this.AttendeeRadio.TabIndex = 11;
			this.AttendeeRadio.Text = "Attendee";
			this.AttendeeRadio.CheckedChanged += new System.EventHandler(this.AttendeeRadio_CheckedChanged);
			// 
			// AccessCodesForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(306, 475);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "AccessCodesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Access Codes";
			this.Load += new System.EventHandler(this.AccessCodes_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void AccessCodes_Load(object sender, System.EventArgs e)
		{		
			button2.BackColor = color;
		}

		private void groupBox1_Enter(object sender, System.EventArgs e)
		{
		
		}
		
		public void EnableDisableControls(bool val)
		{			
			
			chkVideo.Enabled = val;
			chkAudio.Enabled = val;
			chkScreen.Enabled = val;
			chkPresentation.Enabled = val;
			chkDrawing.Enabled = val;
			chkApplication.Enabled = val;
			chkDesktop.Enabled = val;
			chkPolling.Enabled = val;
			chkPvtChat.Enabled = val;
			chkAssignRights.Enabled = val;

		}


		private void AttendeeRadio_CheckedChanged(object sender, System.EventArgs e)
		{
			
			EnableDisableControls(false);		
		}

		private void PresenterRadio_CheckedChanged(object sender, System.EventArgs e)
		{
			EnableDisableControls(true);
		}

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			
			if(AttendeeRadio.Checked == true) // if he selected attende
				EnableDisableControls(false); //reset all the values

		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			ColorDialog cc = new ColorDialog();
			cc.Color = color;
			if(cc.ShowDialog() == DialogResult.OK)
			{
				color = cc.Color;
				button2.BackColor = color;

			}
			
		}
	}
}
