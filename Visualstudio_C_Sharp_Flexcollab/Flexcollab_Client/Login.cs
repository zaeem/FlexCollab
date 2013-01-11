using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WebMeeting.Client.Alerts;
namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for Login.
	/// </summary>
	public class Login : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.TextBox txtBoxName;
		public System.Windows.Forms.TextBox txtBoxEmail;
		public System.Windows.Forms.Button loginBtn;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		public System.Windows.Forms.RadioButton radioHost;
		public System.Windows.Forms.RadioButton radioPresenter;
		public System.Windows.Forms.RadioButton radioAttendee;
		public System.Windows.Forms.TextBox txtFTPIP;
		private System.Windows.Forms.Label label5;
		public System.Windows.Forms.TextBox txtConferenceID;
		private System.Windows.Forms.Label label6;
		public System.Windows.Forms.TextBox	 txtPassword;
		private System.Windows.Forms.Label label7;
		public System.Windows.Forms.TextBox txtPort;
		public System.Windows.Forms.TextBox txtServer;
		public System.Windows.Forms.TextBox txtPass;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label9;
		public System.Windows.Forms.Label lblInvalid;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Login()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.txtServer.Text=Info.getInstance().ServerIP;
			this.txtPort.Text="" + Info.getInstance().ConnectionPort;
			this.txtFTPIP.Text="" + Info.getInstance().FtpIP;

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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtBoxName = new System.Windows.Forms.TextBox();
			this.txtBoxEmail = new System.Windows.Forms.TextBox();
			this.loginBtn = new System.Windows.Forms.Button();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.txtServer = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.radioHost = new System.Windows.Forms.RadioButton();
			this.radioPresenter = new System.Windows.Forms.RadioButton();
			this.radioAttendee = new System.Windows.Forms.RadioButton();
			this.txtFTPIP = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtConferenceID = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtPass = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.lblInvalid = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(99, 255);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Username";
			this.label1.Visible = false;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(99, 287);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Email";
			this.label2.Visible = false;
			// 
			// txtBoxName
			// 
			this.txtBoxName.Location = new System.Drawing.Point(185, 252);
			this.txtBoxName.Name = "txtBoxName";
			this.txtBoxName.Size = new System.Drawing.Size(70, 21);
			this.txtBoxName.TabIndex = 3;
			this.txtBoxName.Text = "";
			this.txtBoxName.Visible = false;
			// 
			// txtBoxEmail
			// 
			this.txtBoxEmail.Location = new System.Drawing.Point(185, 284);
			this.txtBoxEmail.Name = "txtBoxEmail";
			this.txtBoxEmail.Size = new System.Drawing.Size(70, 21);
			this.txtBoxEmail.TabIndex = 4;
			this.txtBoxEmail.Text = "1";
			this.txtBoxEmail.Visible = false;
			// 
			// loginBtn
			// 
			this.loginBtn.Location = new System.Drawing.Point(248, 135);
			this.loginBtn.Name = "loginBtn";
			this.loginBtn.Size = new System.Drawing.Size(84, 23);
			this.loginBtn.TabIndex = 11;
			this.loginBtn.Text = "Connect";
			this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(185, 220);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(70, 21);
			this.txtPort.TabIndex = 2;
			this.txtPort.Text = "8737";
			this.txtPort.Visible = false;
			this.txtPort.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// txtServer
			// 
			this.txtServer.Location = new System.Drawing.Point(187, 165);
			this.txtServer.Name = "txtServer";
			this.txtServer.Size = new System.Drawing.Size(70, 21);
			this.txtServer.TabIndex = 1;
			this.txtServer.Text = "127.0.0.1";
			this.txtServer.Visible = false;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(26, 43);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 16);
			this.label3.TabIndex = 7;
			this.label3.Text = "Password";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(99, 224);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 16);
			this.label4.TabIndex = 8;
			this.label4.Text = "Server Port";
			this.label4.Visible = false;
			// 
			// radioHost
			// 
			this.radioHost.Checked = true;
			this.radioHost.Enabled = false;
			this.radioHost.Location = new System.Drawing.Point(90, 69);
			this.radioHost.Name = "radioHost";
			this.radioHost.Size = new System.Drawing.Size(48, 24);
			this.radioHost.TabIndex = 8;
			this.radioHost.TabStop = true;
			this.radioHost.Text = "host";
			// 
			// radioPresenter
			// 
			this.radioPresenter.Enabled = false;
			this.radioPresenter.Location = new System.Drawing.Point(138, 69);
			this.radioPresenter.Name = "radioPresenter";
			this.radioPresenter.Size = new System.Drawing.Size(72, 24);
			this.radioPresenter.TabIndex = 9;
			this.radioPresenter.Text = "Presenter";
			// 
			// radioAttendee
			// 
			this.radioAttendee.Enabled = false;
			this.radioAttendee.Location = new System.Drawing.Point(210, 69);
			this.radioAttendee.Name = "radioAttendee";
			this.radioAttendee.Size = new System.Drawing.Size(72, 24);
			this.radioAttendee.TabIndex = 10;
			this.radioAttendee.Text = "Attendee";
			// 
			// txtFTPIP
			// 
			this.txtFTPIP.Location = new System.Drawing.Point(184, 314);
			this.txtFTPIP.Name = "txtFTPIP";
			this.txtFTPIP.Size = new System.Drawing.Size(70, 21);
			this.txtFTPIP.TabIndex = 5;
			this.txtFTPIP.Text = "127.0.0.1";
			this.txtFTPIP.Visible = false;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(99, 317);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(48, 16);
			this.label5.TabIndex = 13;
			this.label5.Text = "FTP IP";
			this.label5.Visible = false;
			// 
			// txtConferenceID
			// 
			this.txtConferenceID.Location = new System.Drawing.Point(183, 374);
			this.txtConferenceID.Name = "txtConferenceID";
			this.txtConferenceID.Size = new System.Drawing.Size(38, 21);
			this.txtConferenceID.TabIndex = 7;
			this.txtConferenceID.Text = "1";
			this.txtConferenceID.Visible = false;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(99, 379);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(78, 16);
			this.label6.TabIndex = 15;
			this.label6.Text = "Conference ID";
			this.label6.Visible = false;
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(183, 344);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(70, 21);
			this.txtPassword.TabIndex = 6;
			this.txtPassword.Text = "one";
			this.txtPassword.Visible = false;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(99, 348);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(78, 16);
			this.label7.TabIndex = 17;
			this.label7.Text = "FTP Password";
			this.label7.Visible = false;
			// 
			// txtPass
			// 
			this.txtPass.Location = new System.Drawing.Point(90, 36);
			this.txtPass.Name = "txtPass";
			this.txtPass.PasswordChar = '*';
			this.txtPass.Size = new System.Drawing.Size(133, 21);
			this.txtPass.TabIndex = 18;
			this.txtPass.Text = "";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(99, 196);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(64, 16);
			this.label8.TabIndex = 19;
			this.label8.Text = "Password";
			this.label8.Visible = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblInvalid);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.radioHost);
			this.groupBox1.Controls.Add(this.radioPresenter);
			this.groupBox1.Controls.Add(this.radioAttendee);
			this.groupBox1.Controls.Add(this.txtPass);
			this.groupBox1.Location = new System.Drawing.Point(18, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(317, 120);
			this.groupBox1.TabIndex = 20;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Authentication";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(18, 70);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(63, 16);
			this.label9.TabIndex = 19;
			this.label9.Text = "Client Type";
			// 
			// lblInvalid
			// 
			this.lblInvalid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblInvalid.ForeColor = System.Drawing.Color.Red;
			this.lblInvalid.Location = new System.Drawing.Point(89, 17);
			this.lblInvalid.Name = "lblInvalid";
			this.lblInvalid.Size = new System.Drawing.Size(174, 16);
			this.lblInvalid.TabIndex = 20;
			this.lblInvalid.Text = "Invalid Meeting Password";
			this.lblInvalid.Visible = false;
			// 
			// Login
			// 
			this.AcceptButton = this.loginBtn;
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(346, 165);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.txtConferenceID);
			this.Controls.Add(this.txtFTPIP);
			this.Controls.Add(this.txtServer);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.txtBoxEmail);
			this.Controls.Add(this.txtBoxName);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.loginBtn);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "Login";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WebMeeting Login";
			this.Load += new System.EventHandler(this.Login_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void loginBtn_Click(object sender, System.EventArgs e)
		{
			/*if(txtBoxName.Text.Length < 1)
			{
				MessageBox.Show("Please enter a name","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return;
			}
			
			if(txtBoxEmail.Text.Length < 1)
			{
				MessageBox.Show("Please enter an email","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return;
			}
			if(txtFTPIP.Text.Length < 1)
			{
				MessageBox.Show("Please provide ftp address","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return;
			}
			if(txtConferenceID.Text.Length <  1)
			{
				MessageBox.Show("Please provide conference ID","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return;
			}*/
			if(txtPassword.Text.Length < 1)
			{
				MeetingAlerts alert=new MeetingAlerts();
				alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Please provide passsword ",true,false);
				//MessageBox.Show("Please provide passsword ","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return;
			}
			Info.getInstance().Password=this.txtPassword.Text;
			//Info.getInstance().FtpIP=this.txtFTPIP.Text;

			
			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void Login_Load(object sender, System.EventArgs e)
		{
			
			txtPassword.Focus();
		}

		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
		
		}
	}
}
