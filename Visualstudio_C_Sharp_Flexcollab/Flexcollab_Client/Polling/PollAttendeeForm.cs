using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WebMeeting.Common;
using WebMeeting.Client.Alerts;
namespace WebMeeting.Polling
{
	//resize this window in design mode to see the checkboxes window

	/// <summary>
	/// Summary description for PollAttendeeForm.
	/// </summary>
	public class PollAttendeeForm : System.Windows.Forms.Form
	{
		private NSPAControls.NSButton btnAnswer;
		private System.Windows.Forms.Button btnClose;
		//private bool m_bAnswered=false;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtQuestion;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton4;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton5;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.GroupBox groupRadio;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.CheckBox checkBox5;
		public System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.GroupBox groupCheckboxes;
		private System.Windows.Forms.GroupBox groupTextBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.Label label1;
		private bool anonymous;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		 
		public string getQuestionText()
		{
			return txtQuestion.Text;
		}

		private System.ComponentModel.Container components = null;

		public PollAttendeeForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.textBox1.Focus();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		public void SetAnonymousPoll(bool val)
		{
			anonymous=val;
		}
		public bool GetAnonymousPoll()
		{
			return anonymous;
		}
		public int GetAnswerIndex()
		{
			if(this.radioButton1.Checked==true)
				return 0;
			else if(this.radioButton2.Checked==true)
				return 1;
			else if(this.radioButton3.Checked==true)
				return 2;
			else if(this.radioButton4.Checked==true)
				return 3;
			else if(this.radioButton5.Checked==true)
				return 4;			
			return 0;
		}

		public void changeInterfaceToEvaluation()
		{
			//this.Text.Replace("Poll By","Evaluation By");
			//this.Text = "Evaluation"			
			this.Text = "Evaluation";// + thisWindowPollingObject.ClientName;

			groupBox1.Text = "Evaluation";			
		}
		public string GetAnswerTextRadio()
		{
			if(this.radioButton1.Checked==true)
				return this.radioButton1.Text;
			else if(this.radioButton2.Checked==true)
				return this.radioButton2.Text;
			else if(this.radioButton3.Checked==true)
				return this.radioButton3.Text;
			else if(this.radioButton4.Checked==true)
				return this.radioButton4.Text;
			else if(this.radioButton5.Checked==true)
				return this.radioButton5.Text;			
			return "";
		}
		public string GetAnswerTextMultipleChoice()
		{
			string str = "";
			if(this.checkBox1.Checked==true)
				str = this.checkBox1.Text + "^"	;
			if(this.checkBox2.Checked==true)
				str += this.checkBox2.Text + "^";			
			if(this.checkBox3.Checked==true)
				str += this.checkBox3.Text + "^";				
			if(this.checkBox4.Checked==true)
				str += this.checkBox4.Text + "^";				
			if(this.checkBox5.Checked==true)
				str += this.checkBox5.Text + "^";				

			str = str.Substring(0,str.Length-1);
			return str;
		}
		public string GetAnswerText(PollType pollType)
		{	
			int a=0;
			string str = "";
			switch(pollType)
			{
				case PollType.SingleSelect: case PollType.TrueFalse: case PollType.YesNo:
					str = GetAnswerTextRadio();
					break;
				case PollType.MultipleSelect:
					str = GetAnswerTextMultipleChoice();
					break;
				case PollType.FreeResponse:
					str = this.textBox1.Text.ToString();

					str=str.Replace("\r"," ");
					str=str.Replace("\n"," ");

					//a=str.Length;
					/*
					if(a>1)
					{
					str=str.Substring(0,a-1);
					}
					*/
					break;
			
			}

			return str+" ";

            
		}

		public void SetQuestion(string quest)
		{
			this.txtQuestion.Text=quest;
			
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PollAttendeeForm));
			this.btnAnswer = new NSPAControls.NSButton();
			this.btnClose = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtQuestion = new System.Windows.Forms.TextBox();
			this.groupRadio = new System.Windows.Forms.GroupBox();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton4 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton5 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.groupCheckboxes = new System.Windows.Forms.GroupBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.groupTextBox = new System.Windows.Forms.GroupBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblInfo = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupRadio.SuspendLayout();
			this.groupCheckboxes.SuspendLayout();
			this.groupTextBox.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnAnswer
			// 
			this.btnAnswer.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnAnswer.Enabled = false;
			this.btnAnswer.HighlightColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.btnAnswer.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnAnswer.HottrackImage")));
			this.btnAnswer.Location = new System.Drawing.Point(144, 328);
			this.btnAnswer.Name = "btnAnswer";
			this.btnAnswer.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnAnswer.NormalImage")));
			this.btnAnswer.OnlyShowBitmap = true;
			this.btnAnswer.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnAnswer.PressedImage")));
			this.btnAnswer.Size = new System.Drawing.Size(105, 23);
			this.btnAnswer.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnAnswer.ToolTip = null;
			this.btnAnswer.Click += new System.EventHandler(this.btnAnswer_Click);
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.White;
			this.btnClose.Location = new System.Drawing.Point(48, 328);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(64, 24);
			this.btnClose.TabIndex = 7;
			this.btnClose.Visible = false;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtQuestion);
			this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(16, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(344, 100);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Poll";
			// 
			// txtQuestion
			// 
			this.txtQuestion.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.txtQuestion.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtQuestion.ForeColor = System.Drawing.SystemColors.ControlText;
			this.txtQuestion.Location = new System.Drawing.Point(8, 24);
			this.txtQuestion.Multiline = true;
			this.txtQuestion.Name = "txtQuestion";
			this.txtQuestion.ReadOnly = true;
			this.txtQuestion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtQuestion.Size = new System.Drawing.Size(328, 64);
			this.txtQuestion.TabIndex = 10;
			this.txtQuestion.Text = "textBox1";
			this.txtQuestion.TextChanged += new System.EventHandler(this.txtQuestion_TextChanged);
			// 
			// groupRadio
			// 
			this.groupRadio.Controls.Add(this.radioButton2);
			this.groupRadio.Controls.Add(this.radioButton4);
			this.groupRadio.Controls.Add(this.radioButton1);
			this.groupRadio.Controls.Add(this.radioButton5);
			this.groupRadio.Controls.Add(this.radioButton3);
			this.groupRadio.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupRadio.Location = new System.Drawing.Point(0, 0);
			this.groupRadio.Name = "groupRadio";
			this.groupRadio.Size = new System.Drawing.Size(344, 192);
			this.groupRadio.TabIndex = 10;
			this.groupRadio.TabStop = false;
			this.groupRadio.Text = "Select an option";
			// 
			// radioButton2
			// 
			this.radioButton2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.radioButton2.Location = new System.Drawing.Point(16, 56);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(312, 32);
			this.radioButton2.TabIndex = 2;
			this.radioButton2.Text = "radioButton2";
			this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged_1);
			// 
			// radioButton4
			// 
			this.radioButton4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.radioButton4.Location = new System.Drawing.Point(16, 120);
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.Size = new System.Drawing.Size(312, 32);
			this.radioButton4.TabIndex = 4;
			this.radioButton4.Text = "radioButton4";
			this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
			// 
			// radioButton1
			// 
			this.radioButton1.Checked = true;
			this.radioButton1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.radioButton1.Location = new System.Drawing.Point(16, 24);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(312, 32);
			this.radioButton1.TabIndex = 1;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "radioButton1";
			this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
			// 
			// radioButton5
			// 
			this.radioButton5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.radioButton5.Location = new System.Drawing.Point(16, 152);
			this.radioButton5.Name = "radioButton5";
			this.radioButton5.Size = new System.Drawing.Size(312, 32);
			this.radioButton5.TabIndex = 5;
			this.radioButton5.Text = "radioButton5";
			this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
			// 
			// radioButton3
			// 
			this.radioButton3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.radioButton3.Location = new System.Drawing.Point(16, 88);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(312, 32);
			this.radioButton3.TabIndex = 3;
			this.radioButton3.Text = "radioButton3";
			this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
			// 
			// groupCheckboxes
			// 
			this.groupCheckboxes.Controls.Add(this.checkBox5);
			this.groupCheckboxes.Controls.Add(this.checkBox4);
			this.groupCheckboxes.Controls.Add(this.checkBox3);
			this.groupCheckboxes.Controls.Add(this.checkBox2);
			this.groupCheckboxes.Controls.Add(this.checkBox1);
			this.groupCheckboxes.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupCheckboxes.Location = new System.Drawing.Point(400, 16);
			this.groupCheckboxes.Name = "groupCheckboxes";
			this.groupCheckboxes.Size = new System.Drawing.Size(344, 192);
			this.groupCheckboxes.TabIndex = 11;
			this.groupCheckboxes.TabStop = false;
			this.groupCheckboxes.Text = "Select an option";
			// 
			// checkBox5
			// 
			this.checkBox5.Location = new System.Drawing.Point(24, 152);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(272, 24);
			this.checkBox5.TabIndex = 4;
			this.checkBox5.Text = "checkBox5";
			this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
			// 
			// checkBox4
			// 
			this.checkBox4.Location = new System.Drawing.Point(24, 120);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(272, 24);
			this.checkBox4.TabIndex = 3;
			this.checkBox4.Text = "checkBox4";
			this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
			// 
			// checkBox3
			// 
			this.checkBox3.Location = new System.Drawing.Point(24, 88);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(272, 24);
			this.checkBox3.TabIndex = 2;
			this.checkBox3.Text = "checkBox3";
			this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
			// 
			// checkBox2
			// 
			this.checkBox2.Location = new System.Drawing.Point(24, 56);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(272, 24);
			this.checkBox2.TabIndex = 1;
			this.checkBox2.Text = "checkBox2";
			this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(24, 24);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(272, 24);
			this.checkBox1.TabIndex = 0;
			this.checkBox1.Text = "checkBox1";
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// groupTextBox
			// 
			this.groupTextBox.Controls.Add(this.textBox1);
			this.groupTextBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupTextBox.Location = new System.Drawing.Point(400, 224);
			this.groupTextBox.Name = "groupTextBox";
			this.groupTextBox.Size = new System.Drawing.Size(344, 192);
			this.groupTextBox.TabIndex = 12;
			this.groupTextBox.TabStop = false;
			this.groupTextBox.Text = "Please Write the answer";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(8, 24);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(328, 160);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "";
			this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.groupRadio);
			this.panel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.panel1.Location = new System.Drawing.Point(16, 128);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(344, 192);
			this.panel1.TabIndex = 12;
			// 
			// lblInfo
			// 
			this.lblInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblInfo.ForeColor = System.Drawing.Color.Red;
			this.lblInfo.Location = new System.Drawing.Point(24, 360);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(320, 24);
			this.lblInfo.TabIndex = 13;
			this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(8, 384);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(360, 23);
			this.label1.TabIndex = 14;
			this.label1.Text = "Note: In case of new Poll question, this window will disappear";
			// 
			// PollAttendeeForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.ClientSize = new System.Drawing.Size(370, 416);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.groupCheckboxes);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnAnswer);
			this.Controls.Add(this.groupTextBox);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "PollAttendeeForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Poll for Attendee";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.PollAttendeeForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupRadio.ResumeLayout(false);
			this.groupCheckboxes.ResumeLayout(false);
			this.groupTextBox.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public Polling thisWindowPollingObject;

		
		
		private void btnAnswer_Click(object sender, System.EventArgs e)
		{
			if(testPollType == PollType.FreeResponse)
			{
				if(textBox1.Text.Length < 1)
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Client.Alerts.MeetingAlerts.AlertType.Info,"Please enter your answer",true,false);
					//MessageBox.Show("Please enter your answer","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
					return;
				}
			}
			//this.m_bAnswered=true;
			btnAnswer.Enabled=false;
			DialogResult=DialogResult.OK;									
			Close();
			
		}

		public void SetRadioOptions(ArrayList options)
		{
			this.radioButton1.Visible=false;
			this.radioButton2.Visible=false;
			this.radioButton3.Visible=false;
			this.radioButton4.Visible=false;
			this.radioButton5.Visible=false;

			groupRadio.Visible=true; 
			groupCheckboxes.Enabled=false;
			groupTextBox.Enabled=false;
			for(int i=0;i<options.Count;i++)
			{
				string opt=(string)options[i];
				switch(i)
				{
					case 0:
					{
						this.radioButton1.Visible=true;
						this.radioButton1.Text=opt;

						break;
					}
					case 1:
					{
						this.radioButton2.Visible=true;
						this.radioButton2.Text=opt;
						
						break;
					}
					case 2:
					{
						this.radioButton3.Visible=true;
						this.radioButton3.Text=opt;
				
						break;
					}
					case 3:
					{
						this.radioButton4.Visible=true;
						this.radioButton4.Text=opt;
						
						break;
					}
					case 4:
					{
						this.radioButton5.Visible=true;
						this.radioButton5.Text=opt;
					
						break;
					}
				}
			}
		
            			
			
		}
		public void SetCheckOptions(ArrayList options)
		{
			this.checkBox1.Visible=false;
			this.checkBox2.Visible=false;
			this.checkBox3.Visible=false;
			this.checkBox4.Visible=false;
			this.checkBox5.Visible=false;
			groupRadio.Visible=false; 
			groupCheckboxes.Enabled=true;
			groupTextBox.Enabled=false;
			groupCheckboxes.Text="You may select more than one answer choice.";
			for(int i=0;i<options.Count;i++)
			{
				string opt=(string)options[i];
				switch(i)
				{
					case 0:
					{
						this.checkBox1.Visible=true;
						this.checkBox1.Text=opt;
						
						break;
					}
					case 1:
					{
						this.checkBox2.Visible=true;
						this.checkBox2.Text=opt;						
					
						break;
					}
					case 2:
					{
						this.checkBox3.Visible=true;
						this.checkBox3.Text=opt;
						
						break;
					}
					case 3:
					{
						this.checkBox4.Visible=true;
						this.checkBox4.Text=opt;
						
						break;
					}
					case 4:
					{
						this.checkBox5.Visible=true;
						this.checkBox5.Text=opt;
						
						break;
					}
				}
			}
						
		}
		public void SetTrueFalse(ArrayList options)
		{
			
		}
		public void SetOptions(PollType type,ArrayList options)
		{
			ChangeWindowState(type);
			switch(type)
			{
				case PollType.FreeResponse:
					groupCheckboxes.Visible=false;
					groupRadio.Visible=false;
					groupTextBox.Visible=true;	
					this.textBox1.Focus();
					break;
				case PollType.SingleSelect:
					SetRadioOptions(options);
					break;
				case PollType.MultipleSelect:
					SetCheckOptions(options);
					break;
				case PollType.TrueFalse: case PollType.YesNo:
					SetRadioOptions(options);					
					break;								
					//case PollType.FreeResponse:
			}
			if(anonymous)
				lblInfo.Text="This answer will be submitted anonymously";
			
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			DialogResult=DialogResult.Cancel;
			this.Close();
		}

		public PollType testPollType;

		public void ChangeWindowState(WebMeeting.Common.PollType pollType)
		{
			testPollType = pollType;
			switch(pollType)
			{
				case PollType.FreeResponse:
					panel1.Controls.Clear();
					panel1.Controls.Add(groupTextBox);
					panel1.Controls.Remove(groupRadio);					
					groupTextBox.Dock = DockStyle.Fill;
					break;
				case PollType.SingleSelect:
					panel1.Controls.Clear();
					panel1.Controls.Add(groupRadio);
					panel1.Controls.Remove(groupCheckboxes);
					groupRadio.Dock = DockStyle.Fill;
					break;
				case PollType.MultipleSelect:
					panel1.Controls.Clear();
					panel1.Controls.Add(groupCheckboxes);
					panel1.Controls.Remove(groupRadio);
					groupCheckboxes.Dock = DockStyle.Fill;
					break;
				case PollType.TrueFalse:
					panel1.Controls.Clear();
					panel1.Controls.Add(groupRadio);
					panel1.Controls.Remove(groupCheckboxes);			
					groupRadio.Dock = DockStyle.Fill;
					break;                 			
				case PollType.YesNo:
					panel1.Controls.Clear();
					panel1.Controls.Add(groupRadio);
					panel1.Controls.Remove(groupCheckboxes);
					groupRadio.Dock = DockStyle.Fill;				
					break;                 			
			}
		}
        
		public void changeButtonState()
		{
			if(	(radioButton1.Checked == true) ||
				(radioButton2.Checked == true) ||
				(radioButton3.Checked == true) ||
				(radioButton4.Checked == true) ||
				(radioButton5.Checked == true))
			{             
				btnAnswer.Enabled = true;
			}
			else
				btnAnswer.Enabled = false;

		}

		public void changeButtonStateCheckBox()
		{
			if(	(checkBox1.Checked == true) ||
				(checkBox2.Checked == true) ||
				(checkBox3.Checked == true) ||
				(checkBox4.Checked == true) ||
				(checkBox5.Checked == true))
			{             
				btnAnswer.Enabled = true;
			}
			else
				btnAnswer.Enabled = false;

		}

		public void DisableForm()
		{

			radioButton1.Enabled = false;
			radioButton2.Enabled = false;
			radioButton3.Enabled = false;
			radioButton4.Enabled = false;
			radioButton5.Enabled = false;
			btnAnswer.Enabled = false;
			btnClose.Enabled = true;
		}
		private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
		{
			changeButtonState();
		}

		

		private void radioButton2_CheckedChanged_1(object sender, System.EventArgs e)
		{
			changeButtonState();

		}

		private void radioButton3_CheckedChanged(object sender, System.EventArgs e)
		{
			changeButtonState();

		}

		private void radioButton4_CheckedChanged(object sender, System.EventArgs e)
		{
			changeButtonState();

		}

		private void radioButton5_CheckedChanged(object sender, System.EventArgs e)
		{
			changeButtonState();

		}

		private void txtQuestion_TextChanged(object sender, System.EventArgs e)
		{
			btnAnswer.Enabled=  (txtQuestion.Text.Length != 0);

		}

		private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
		{
			changeButtonStateCheckBox();
		}

		private void checkBox2_CheckedChanged(object sender, System.EventArgs e)
		{
			changeButtonStateCheckBox();
		}

		private void checkBox3_CheckedChanged(object sender, System.EventArgs e)
		{
			changeButtonStateCheckBox();
		}

		private void checkBox4_CheckedChanged(object sender, System.EventArgs e)
		{
			changeButtonStateCheckBox();
		}

		private void checkBox5_CheckedChanged(object sender, System.EventArgs e)
		{
			changeButtonStateCheckBox();
		}

		private void PollAttendeeForm_Load(object sender, System.EventArgs e)
		{
		
		}

		private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if( e.KeyChar == '\r' )
			{
				if(textBox1.Text.Trim()!="")
				{
					if(testPollType == PollType.FreeResponse)
					{
						if(textBox1.Text.Length < 1)
						{
							MeetingAlerts alert=new MeetingAlerts();
							alert.ShowMessage(Client.Alerts.MeetingAlerts.AlertType.Info,"Please enter your answer",true,false);
							//MessageBox.Show("Please enter your answer","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
							return;
						}
					}
					//this.m_bAnswered=true;
					btnAnswer.Enabled=false;
					DialogResult=DialogResult.OK;									
					Close();

				}
			}
		}

		
	}
}
