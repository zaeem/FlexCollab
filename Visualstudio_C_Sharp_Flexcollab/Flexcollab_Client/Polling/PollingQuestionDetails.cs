using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WebMeeting.Common;
using WebMeeting.Client.Alerts;
namespace WebMeeting.Polling
{
	/// <summary>
	/// Summary description for Form2.
	/// Code improvement and Exception Handling done by Zaeem
	/// </summary>
	public class PollingQuestionDetails : System.Windows.Forms.Form
	{
			
			/// <summary> 
			/// Required designer variable.
			/// </summary>
		
			
		private System.Windows.Forms.TextBox textQuestion;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxNumberOfAnswers;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private NSPAControls.NSButton  btnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public System.Windows.Forms.CheckBox checkAnonymous;
		private System.Windows.Forms.Label label8;
		public System.Windows.Forms.ComboBox comboType;
		public System.Windows.Forms.GroupBox heading;

		public Client.PollingTabPage parentTabPage;
		private NSPAControls.NSButton  btnOpen;
		private bool makeReadOnly = false;
		public bool isAutomated=false;
		private string questiondetails="";
		private System.Windows.Forms.Label label9;
		public bool IsEvaluation =false;

		#region PollingQuestionDetails() ie contructor
		public PollingQuestionDetails()
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();
				this.comboBoxNumberOfAnswers.DataSource=new string[]{"2","3","4","5"};
				comboType.SelectedIndex = 0;
			
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollingQueationDetails.cs @PollingQueationDetails() line==> 66",exp,null,false);
			}
			
			
		}
		#endregion 


		#region PollingQuestionDetails(bool prmisAutomated,string prmquestiondetails)
		public PollingQuestionDetails(bool prmisAutomated,string prmquestiondetails)
		{
		try
			{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.comboBoxNumberOfAnswers.DataSource=new string[]{"2","3","4","5"};
			comboType.SelectedIndex = 1;
			this.isAutomated=prmisAutomated;
			this.questiondetails=prmquestiondetails;
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollingQueationDetails.cs @PollingQueationDetails() line==> 96",exp,null,false);
			}
		}
		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		# region Dispose to clean resources 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PollingQuestionDetails));
			this.textQuestion = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxNumberOfAnswers = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.btnCancel = new NSPAControls.NSButton();
			this.heading = new System.Windows.Forms.GroupBox();
			this.comboType = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.checkAnonymous = new System.Windows.Forms.CheckBox();
			this.btnOpen = new NSPAControls.NSButton();
			this.label9 = new System.Windows.Forms.Label();
			this.heading.SuspendLayout();
			this.SuspendLayout();
			// 
			// textQuestion
			// 
			this.textQuestion.Location = new System.Drawing.Point(120, 48);
			this.textQuestion.MaxLength = 150;
			this.textQuestion.Multiline = true;
			this.textQuestion.Name = "textQuestion";
			this.textQuestion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textQuestion.Size = new System.Drawing.Size(352, 80);
			this.textQuestion.TabIndex = 0;
			this.textQuestion.Text = "";
			this.textQuestion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textQuestion_KeyPress);
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(24, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 24);
			this.label1.TabIndex = 55;
			this.label1.Text = "Question to Ask";
			// 
			// comboBoxNumberOfAnswers
			// 
			this.comboBoxNumberOfAnswers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxNumberOfAnswers.Location = new System.Drawing.Point(120, 136);
			this.comboBoxNumberOfAnswers.Name = "comboBoxNumberOfAnswers";
			this.comboBoxNumberOfAnswers.Size = new System.Drawing.Size(56, 21);
			this.comboBoxNumberOfAnswers.TabIndex = 1;
			this.comboBoxNumberOfAnswers.SelectedIndexChanged += new System.EventHandler(this.comboBoxNumberOfAnswers_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(24, 136);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 24);
			this.label2.TabIndex = 56;
			this.label2.Text = "Number of Options";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(120, 168);
			this.textBox1.MaxLength = 30;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(352, 21);
			this.textBox1.TabIndex = 4;
			this.textBox1.Text = "";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(120, 200);
			this.textBox2.MaxLength = 30;
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(352, 21);
			this.textBox2.TabIndex = 5;
			this.textBox2.Text = "";
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(120, 232);
			this.textBox3.MaxLength = 30;
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(352, 21);
			this.textBox3.TabIndex = 6;
			this.textBox3.Text = "";
			this.textBox3.Visible = false;
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(120, 264);
			this.textBox4.MaxLength = 30;
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(352, 21);
			this.textBox4.TabIndex = 7;
			this.textBox4.Text = "";
			this.textBox4.Visible = false;
			// 
			// textBox5
			// 
			this.textBox5.Location = new System.Drawing.Point(120, 296);
			this.textBox5.MaxLength = 30;
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(352, 21);
			this.textBox5.TabIndex = 8;
			this.textBox5.Text = "";
			this.textBox5.Visible = false;
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(24, 168);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 24);
			this.label3.TabIndex = 57;
			this.label3.Text = "Option # 1";
			// 
			// label4
			// 
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(24, 200);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 24);
			this.label4.TabIndex = 58;
			this.label4.Text = "Option # 2";
			// 
			// label5
			// 
			this.label5.ForeColor = System.Drawing.Color.White;
			this.label5.Location = new System.Drawing.Point(24, 232);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 24);
			this.label5.TabIndex = 59;
			this.label5.Text = "Option # 3";
			this.label5.Visible = false;
			// 
			// label6
			// 
			this.label6.ForeColor = System.Drawing.Color.White;
			this.label6.Location = new System.Drawing.Point(24, 264);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 24);
			this.label6.TabIndex = 60;
			this.label6.Text = "Option # 4";
			this.label6.Visible = false;
			// 
			// label7
			// 
			this.label7.ForeColor = System.Drawing.Color.White;
			this.label7.Location = new System.Drawing.Point(24, 296);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(80, 24);
			this.label7.TabIndex = 61;
			this.label7.Text = "Option # 5";
			this.label7.Visible = false;
			// 
			// btnCancel
			// 
			this.btnCancel.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnCancel.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnCancel.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.HottrackImage")));
			this.btnCancel.Location = new System.Drawing.Point(432, 336);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.NormalImage")));
			this.btnCancel.OnlyShowBitmap = true;
			this.btnCancel.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.PressedImage")));
			this.btnCancel.Size = new System.Drawing.Size(64, 28);
			this.btnCancel.Text = "Cancel";
			this.btnCancel.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnCancel.ToolTip = null;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// heading
			// 
			this.heading.Controls.Add(this.comboType);
			this.heading.Controls.Add(this.label8);
			this.heading.Controls.Add(this.checkAnonymous);
			this.heading.ForeColor = System.Drawing.Color.White;
			this.heading.Location = new System.Drawing.Point(16, 24);
			this.heading.Name = "heading";
			this.heading.Size = new System.Drawing.Size(472, 312);
			this.heading.TabIndex = 16;
			this.heading.TabStop = false;
			this.heading.Text = " Poll Details ";
			// 
			// comboType
			// 
			this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboType.Items.AddRange(new object[] {
														   "Free Question",
														   "Single Select",
														   "Multiple Select",
														   "True/False",
														   "Yes/No"});
			this.comboType.Location = new System.Drawing.Point(200, 112);
			this.comboType.Name = "comboType";
			this.comboType.Size = new System.Drawing.Size(152, 21);
			this.comboType.TabIndex = 2;
			this.comboType.SelectedIndexChanged += new System.EventHandler(this.comboType_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.ForeColor = System.Drawing.Color.White;
			this.label8.Location = new System.Drawing.Point(168, 112);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(32, 24);
			this.label8.TabIndex = 57;
			this.label8.Text = "Type";
			// 
			// checkAnonymous
			// 
			this.checkAnonymous.ForeColor = System.Drawing.Color.White;
			this.checkAnonymous.Location = new System.Drawing.Point(360, 112);
			this.checkAnonymous.Name = "checkAnonymous";
			this.checkAnonymous.Size = new System.Drawing.Size(110, 24);
			this.checkAnonymous.TabIndex = 3;
			this.checkAnonymous.Text = "&Anonymous Poll";
			// 
			// btnOpen
			// 
			this.btnOpen.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnOpen.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnOpen.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnOpen.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnOpen.HottrackImage")));
			this.btnOpen.Location = new System.Drawing.Point(360, 336);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnOpen.NormalImage")));
			this.btnOpen.OnlyShowBitmap = true;
			this.btnOpen.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnOpen.PressedImage")));
			this.btnOpen.Size = new System.Drawing.Size(74, 28);
			this.btnOpen.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnOpen.ToolTip = null;
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label9.ForeColor = System.Drawing.Color.Red;
			this.label9.Location = new System.Drawing.Point(8, 346);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(352, 16);
			this.label9.TabIndex = 63;
			this.label9.Text = "Note: On Asking a new question, previous Poll will disappear.";
			// 
			// PollingQuestionDetails
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.ClientSize = new System.Drawing.Size(498, 370);
			this.ControlBox = false;
			this.Controls.Add(this.label9);
			this.Controls.Add(this.comboBoxNumberOfAnswers);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOpen);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBox5);
			this.Controls.Add(this.textBox4);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.textQuestion);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.heading);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "PollingQuestionDetails";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.heading.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	
		# region btnCancel_Click(object sender, System.EventArgs e)
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult=DialogResult.Cancel;
			this.Close();
		}
		# endregion 


		#region comboBoxNumberOfAnswers_SelectedIndexChanged(object sender, System.EventArgs e)
		private void comboBoxNumberOfAnswers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch((string)comboBoxNumberOfAnswers.SelectedItem)
			{
				case "2":
					this.label3.Visible=true;
					this.label4.Visible=true;
					this.label5.Visible=false;
					this.label6.Visible=false;
					this.label7.Visible=false;					
					this.textBox1.Visible=true;
					this.textBox2.Visible=true;
					this.textBox3.Visible=false;
					this.textBox4.Visible=false;
					this.textBox5.Visible=false;
					btnOpen.Top = textBox2.Bottom + 16;
					btnCancel.Top = textBox2.Bottom + 16;
					label9.Top=textBox2.Bottom + 18;
					break;
				case "3":
					this.label3.Visible=true;
					this.label4.Visible=true;
					this.label5.Visible=true;
					this.label6.Visible=false;
					this.label7.Visible=false;					
					this.textBox1.Visible=true;
					this.textBox2.Visible=true;
					this.textBox3.Visible=true;
					this.textBox4.Visible=false;
					this.textBox5.Visible=false;
					btnOpen.Top = textBox3.Bottom + 16;
					btnCancel.Top = textBox3.Bottom + 16;
					label9.Top=textBox3.Bottom + 18;
					break;
				case "4":
					this.label3.Visible=true;
					this.label4.Visible=true;
					this.label5.Visible=true;
					this.label6.Visible=true;
					this.label7.Visible=false;					
					this.textBox1.Visible=true;
					this.textBox2.Visible=true;
					this.textBox3.Visible=true;
					this.textBox4.Visible=true;
					this.textBox5.Visible=false;
					btnOpen.Top = textBox4.Bottom + 16;
					btnCancel.Top = textBox4.Bottom + 16;
					this.label9.Top=textBox4.Bottom + 18;
					break;
				case "5":
					this.label3.Visible=true;
					this.label4.Visible=true;
					this.label5.Visible=true;
					this.label6.Visible=true;
					this.label7.Visible=true;					
					this.textBox1.Visible=true;
					this.textBox2.Visible=true;
					this.textBox3.Visible=true;
					this.textBox4.Visible=true;
					this.textBox5.Visible=true;
					btnOpen.Top = textBox5.Bottom + 16;
					btnCancel.Top = textBox5.Bottom + 16;
					this.label9.Top=textBox5.Bottom + 18;
					break;
			}
			heading.Height = (btnOpen.Top - 6 ) - heading.Top ;
			this.Height = btnCancel.Bottom + 40;
		}

		# endregion

		#region ShowExceptionMessage(string msg)
		public void ShowExceptionMessage(string msg)
		{
			MeetingAlerts alert=new MeetingAlerts();
			alert.ShowMessage(Client.Alerts.MeetingAlerts.AlertType.NonFatal,msg,true,false);
			//MessageBox.Show(msg,"WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}
		# endregion
		
		# region LunchPollMangesContent()
		public void LunchPollMangesContent()
		{
			try
			{
				if(this.isAutomated)
				{
					//this.questiondetails="whats ur mobile phone:1:nokia 100:nokia 3310:nokia 6600";
					string [] dateComponents;
					int indexlength=0;
					//char sep=@":";
					dateComponents=this.questiondetails.Split(':');
					// 1 index of this array always have a question 
					this.textQuestion.Text= dateComponents[0];
					//Trace.WriteLine(dateComponents.Length.ToString());
					this.comboType.SelectedIndex=int.Parse(dateComponents[1])-1; 
					indexlength=dateComponents.Length-2;
					this.comboBoxNumberOfAnswers.SelectedItem =indexlength.ToString();;
					//				if(indexlength=0)
					//					
					//				if(indexlength=1)
					//				if(indexlength=2)
					//				if(indexlength=3)
					//				if(indexlength=4)
					//				if(indexlength=5)

					//	this.comboBoxNumberOfAnswers.SelectedItem=indexlength.ToString();
					//Trace.WriteLine(indexlength.ToString());
					//Trace.WriteLine(comboBoxNumberOfAnswers.SelectedItem.ToString());
					//Trace.WriteLine(dateComponents.Length.ToString());
					for(int i=2;i<dateComponents.Length;i++)
					{
						switch(i-1)
						{
							case 1:
								this.textBox1.Text=dateComponents[i];									
								break;
							case 2:
								this.textBox2.Text=dateComponents[i];									
							
								break;
							case 3:
								this.textBox3.Text=dateComponents[i];									
								break;
							case 4:
								this.textBox4.Text=dateComponents[i];									
								break;
							case 5:
								this.textBox5.Text=dateComponents[i];
								break;
						}
					}
				}
				if(this.textQuestion.Text=="")
				{
					ShowExceptionMessage("Invalid Question");
					return;
				}
				if((comboType.SelectedIndex == 1)||(comboType.SelectedIndex == 2))
				{
					for(int i=1;i<=Int16.Parse((string)comboBoxNumberOfAnswers.SelectedItem);i++)
					{
						switch(i)
						{
							case 1:
								if(this.textBox1.Text=="")
								{
									ShowError(i);
									return;
								}
								break;
							case 2:
								if(this.textBox2.Text=="")
								{
									ShowError(i);
									return;
								}
								break;
							case 3:
								if(this.textBox3.Text=="")
								{
									ShowError(i);
									return;
								}
								break;
							case 4:
								if(this.textBox4.Text=="")
								{
									ShowError(i);
									return;
								}
								break;
							case 5:
								if(this.textBox5.Text=="")
								{
									ShowError(i);
									return;
								}
								break;
						}
					}
				}


				Client.ClientUI.getInstance().DisplayResultWindow(parentTabPage);
				if(!this.isAutomated)
				{
					DialogResult = DialogResult.OK;
					Close();
				}
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollingQueationDetails.cs @PollingQueationDetails() line==> 96",exp,null,false);
			}
		
		}
		
		# endregion 
		
		
		# region btnOpen_Click(object sender, System.EventArgs e)
		private void btnOpen_Click(object sender, System.EventArgs e)
		{
			if(this.textQuestion.Text.Trim()=="")
			{
				ShowExceptionMessage("Question field is empty. Please enter your question !");
				textQuestion.Focus();
				return;
			}
			textQuestion.Text=textQuestion.Text.Replace("\r"," ");
			textQuestion.Text=textQuestion.Text.Replace("\n"," ");


			if((comboType.SelectedIndex == 1)||(comboType.SelectedIndex == 2))
			{
				for(int i=1;i<=Int16.Parse((string)comboBoxNumberOfAnswers.SelectedItem);i++)
				{
					switch(i)
					{
						case 1:
							if(this.textBox1.Text=="")
							{
								ShowError(i);
								return;
							}
							break;
						case 2:
							if(this.textBox2.Text=="")
							{
								ShowError(i);
								return;
							}
							break;
						case 3:
							if(this.textBox3.Text=="")
							{
								ShowError(i);
								return;
							}
							break;
						case 4:
							if(this.textBox4.Text=="")
							{
								ShowError(i);
								return;
							}
							break;
						case 5:
							if(this.textBox5.Text=="")
							{
								ShowError(i);
								return;
							}
							break;
					}
				}
			}


			Client.ClientUI.getInstance().DisplayResultWindow(parentTabPage);
			DialogResult = DialogResult.OK;
			Close();														
		
		}

		# endregion 

		# region ArrayList GetChoices(PollType pollType)
		public ArrayList GetChoices(PollType pollType)
		{
			ArrayList list = new ArrayList();
			switch(pollType)
			{
				case PollType.SingleSelect:
					list =  GetRadioChoicies();
					break;
				case PollType.MultipleSelect:
					list = GetRadioChoicies();
					break;
				case PollType.FreeResponse:					
					break;
				case PollType.TrueFalse:
					list = GetTrueFalseChoicies();
					break;
				case PollType.YesNo:
					list = GetYesNoChoices();
					break;				
			}
			return list;
		}
		# endregion
 
		# region ArrayList GetChoices(PollType pollType)
		public ArrayList GetTrueFalseChoicies()
		{
			ArrayList a1 = new ArrayList();			
			a1.Add("True");
			a1.Add("False");
			return a1;
		}
		# endregion

		# region ArrayList GetYesNoChoices()
		public ArrayList GetYesNoChoices()
		{
			ArrayList a1 = new ArrayList();			
			a1.Add("Yes");
			a1.Add("No");
			return a1;
		}
		# endregion
		
		# region changeInterfacetoEvaluation()
		public void changeInterfacetoEvaluation()
		{
			this.Text = "Evaluation";

			heading.Text = "Evaluation Details";
			checkAnonymous.Visible = false;
			IsEvaluation = true;


		}
		# endregion

		
		# region ArrayList GetRadioChoicies()
		public ArrayList GetRadioChoicies()
		{

			ArrayList al=new ArrayList();
			for(int i=1;i<=Int16.Parse((string)comboBoxNumberOfAnswers.SelectedItem);i++)
			{
				switch(i)
				{
					case 1:
						al.Add(this.textBox1.Text);						
						break;
					case 2:
						al.Add(this.textBox2.Text);						
						break;						
					case 3:
						al.Add(this.textBox3.Text);						
						break;
					case 4:
						al.Add(this.textBox4.Text);						
						break;
					case 5:
						al.Add(this.textBox5.Text);						
						break;
				}
			}
			return al;			
		}
		# endregion

		# region GetQuestion()
		public string GetQuestion()
		{
			return this.textQuestion.Text;
		}
		# endregion

		# region ShowError(int index)
		public void ShowError(int index)
		{
			ShowExceptionMessage("Option " + index + " is not valid");
        		}
		# endregion

		# region PollType GetTypeEx()
		public PollType GetTypeEx()
		{
			PollType type = new PollType();
			switch(comboType.SelectedIndex)
			{
				case 0:
					type = PollType.FreeResponse;
					break;
				case 1:
					type = PollType.SingleSelect;
					break;
				case 2:
					type = PollType.MultipleSelect;					
					break;
				case 3:
					type = PollType.TrueFalse;
					break;                 			
				case 4:
					type = PollType.YesNo;					
					break;                 			
			}
			return type;
		}
		# endregion

		# region HideWindows(bool val)
		public void HideWindows(bool val)
		{			
			label3.Visible = val;			
			label4.Visible = val;
			textBox1.Visible = val;
			textBox2.Visible = val;	
			checkAnonymous.Enabled = true;
			if(!val)
			{
				checkAnonymous.Checked = false;
				checkAnonymous.Enabled = false;
				
				label5.Visible = val;
				label6.Visible = val;
				label7.Visible = val;
				textBox3.Visible = val;
				textBox4.Visible = val;
				textBox5.Visible = val;
			}
			comboBoxNumberOfAnswers.Enabled = val;            
		}
		# endregion

		# region ShowWebPollQuestion(WebPoll poll)
		public bool ShowWebPollQuestion(WebPoll poll)
		{
			try
			{
				this.textQuestion.Text = poll.question;
				if((poll.pollType == PollType.FreeResponse)||(poll.pollType == PollType.TrueFalse) || (poll.pollType == PollType.YesNo))
				{
					HideWindows(false);
					switch(poll.pollType)
					{
						case PollType.FreeResponse:
							
							textBox1.Visible = false;
							textBox2.Visible = false;							
							comboType.SelectedIndex = 0;
							//comboBoxNumberOfAnswers.SelectedIndex = 0;
							break;
						case PollType.TrueFalse:
							//comboBoxNumberOfAnswers.SelectedIndex = 0;
							comboType.SelectedIndex = 3;
							break;
						case PollType.YesNo:
							//comboBoxNumberOfAnswers.SelectedIndex = 0;
							comboType.SelectedIndex = 4;
							break;											  
					}					
					
				}
				else
				{
					HideWindows(true);
					if(poll.pollType == PollType.MultipleSelect)
						comboType.SelectedIndex = 2;
					else
						comboType.SelectedIndex = 1;
                  
					int nCount=0;
					foreach(string str in poll.choices)
					{
						switch(nCount)
						{
							case 0:
								textBox1.Text = str;
								comboBoxNumberOfAnswers.SelectedIndex = 0;
								break;
							case 1:
								textBox2.Text = str;
								comboBoxNumberOfAnswers.SelectedIndex = 0;
								break;
							case 2:
								textBox3.Text = str;
								comboBoxNumberOfAnswers.SelectedIndex = 1;
								break;
							case 3:
								comboBoxNumberOfAnswers.SelectedIndex = 2;
								textBox4.Text = str;
								break;
							case 4:
								comboBoxNumberOfAnswers.SelectedIndex = 3;
								textBox5.Text = str;
								break;
						}
						nCount++;
					}

				}
				textBox1.ReadOnly = true;
				textBox2.ReadOnly =true;
				textBox3.ReadOnly = true;
				textBox4.ReadOnly = true;
				textBox5.ReadOnly = true;
				comboBoxNumberOfAnswers.Enabled = false;
				comboType.Enabled = false;
				textQuestion.ReadOnly = true;
				checkAnonymous.Enabled = false;
				comboBoxNumberOfAnswers.Enabled = false;
				makeReadOnly = true;
				if(ShowDialog() == DialogResult.OK)
                    return true;
				else
					return false;


			}
			catch(Exception )
			{
			}
			return false;
		}
	
		# endregion

		# region comboType_SelectedIndexChanged(object sender, System.EventArgs e)
		private void comboType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(makeReadOnly)
			{
				comboBoxNumberOfAnswers.Enabled = false;
				checkAnonymous.Enabled = false;
				return;
			}

			switch(comboType.SelectedIndex)
			{
				case 0:
					comboBoxNumberOfAnswers.SelectedIndex = 0;
					HideWindows(false);
					checkAnonymous.Enabled =true;
					break;
				case 1:
					HideWindows(true);
					comboBoxNumberOfAnswers.SelectedIndex = 0;
					break;
				case 2:
					HideWindows(true);
					comboBoxNumberOfAnswers.SelectedIndex = 0;
					checkAnonymous.Enabled =true;
					break;
				case 3:
					HideWindows(false);
					comboBoxNumberOfAnswers.SelectedIndex = 0;
					comboBoxNumberOfAnswers.Enabled = false;  
					checkAnonymous.Enabled =true;
					break;
				case 4:
					HideWindows(false);
					comboBoxNumberOfAnswers.SelectedIndex = 0;
					comboBoxNumberOfAnswers.Enabled = false;  
					checkAnonymous.Enabled =true;
					break;
			
			}
		

		}
		# endregion

		private void textQuestion_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar==13)
			{
			this.btnOpen_Click(sender,e);
			}
		}
	}
}
