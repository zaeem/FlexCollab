using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using WebMeeting;
using WebMeeting.Client;
using WebMeeting.Common;


namespace WebMeeting.Client
{			

	/// <summary>
	/// Summary description for QuestionAnswerPresenter.
	/// </summary>
	public class QuestionAnswerAttendee : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel baseLeftPanel;
		private System.Windows.Forms.Panel baseRightPanel;
		private System.Windows.Forms.Panel baseBottomPanel;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ListView listQuestions;
		private System.Windows.Forms.ListView listAnswers;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel bodyTopPanel;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox textQuestion;
		private System.Windows.Forms.Panel panelBody;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Panel panel9;
		private System.Windows.Forms.Panel panel10;
		private System.Windows.Forms.Panel panel11;
		private System.Windows.Forms.TextBox textAnswer;
		private System.Windows.Forms.TextBox textPreviousQuestion;
		private NSPAControls.NSButton button1;
	
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public bool ShowClose()
		{
			return true;
		}

		public QuestionAnswerAttendee()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(QuestionAnswerAttendee));
			this.baseLeftPanel = new System.Windows.Forms.Panel();
			this.baseRightPanel = new System.Windows.Forms.Panel();
			this.baseBottomPanel = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.panelBody = new System.Windows.Forms.Panel();
			this.panel8 = new System.Windows.Forms.Panel();
			this.panel11 = new System.Windows.Forms.Panel();
			this.listAnswers = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.panel10 = new System.Windows.Forms.Panel();
			this.textPreviousQuestion = new System.Windows.Forms.TextBox();
			this.panel9 = new System.Windows.Forms.Panel();
			this.textAnswer = new System.Windows.Forms.TextBox();
			this.panel5 = new System.Windows.Forms.Panel();
			this.panel6 = new System.Windows.Forms.Panel();
			this.listQuestions = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.panel7 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.bodyTopPanel = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.textQuestion = new System.Windows.Forms.TextBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.button1 = new NSPAControls.NSButton();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel4.SuspendLayout();
			this.panelBody.SuspendLayout();
			this.panel8.SuspendLayout();
			this.panel11.SuspendLayout();
			this.panel10.SuspendLayout();
			this.panel9.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel7.SuspendLayout();
			this.bodyTopPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseLeftPanel
			// 
			this.baseLeftPanel.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(179)), ((System.Byte)(239)));
			this.baseLeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.baseLeftPanel.Location = new System.Drawing.Point(0, 0);
			this.baseLeftPanel.Name = "baseLeftPanel";
			this.baseLeftPanel.Size = new System.Drawing.Size(16, 464);
			this.baseLeftPanel.TabIndex = 0;
			// 
			// baseRightPanel
			// 
			this.baseRightPanel.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.baseRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.baseRightPanel.Location = new System.Drawing.Point(640, 0);
			this.baseRightPanel.Name = "baseRightPanel";
			this.baseRightPanel.Size = new System.Drawing.Size(16, 464);
			this.baseRightPanel.TabIndex = 1;
			// 
			// baseBottomPanel
			// 
			this.baseBottomPanel.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.baseBottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.baseBottomPanel.Location = new System.Drawing.Point(16, 456);
			this.baseBottomPanel.Name = "baseBottomPanel";
			this.baseBottomPanel.Size = new System.Drawing.Size(624, 8);
			this.baseBottomPanel.TabIndex = 2;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.panelBody);
			this.panel4.Controls.Add(this.bodyTopPanel);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(16, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(624, 456);
			this.panel4.TabIndex = 3;
			// 
			// panelBody
			// 
			this.panelBody.Controls.Add(this.panel8);
			this.panelBody.Controls.Add(this.panel5);
			this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelBody.Location = new System.Drawing.Point(0, 104);
			this.panelBody.Name = "panelBody";
			this.panelBody.Size = new System.Drawing.Size(624, 352);
			this.panelBody.TabIndex = 7;
			// 
			// panel8
			// 
			this.panel8.Controls.Add(this.panel11);
			this.panel8.Controls.Add(this.panel10);
			this.panel8.Controls.Add(this.panel9);
			this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel8.Location = new System.Drawing.Point(0, 120);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(624, 232);
			this.panel8.TabIndex = 8;
			// 
			// panel11
			// 
			this.panel11.Controls.Add(this.listAnswers);
			this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel11.Location = new System.Drawing.Point(0, 48);
			this.panel11.Name = "panel11";
			this.panel11.Size = new System.Drawing.Size(624, 128);
			this.panel11.TabIndex = 6;
			// 
			// listAnswers
			// 
			this.listAnswers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.columnHeader4,
																						  this.columnHeader5});
			this.listAnswers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listAnswers.FullRowSelect = true;
			this.listAnswers.HideSelection = false;
			this.listAnswers.Location = new System.Drawing.Point(0, 0);
			this.listAnswers.MultiSelect = false;
			this.listAnswers.Name = "listAnswers";
			this.listAnswers.Size = new System.Drawing.Size(624, 128);
			this.listAnswers.TabIndex = 2;
			this.listAnswers.View = System.Windows.Forms.View.Details;
			this.listAnswers.SelectedIndexChanged += new System.EventHandler(this.listAnswers_SelectedIndexChanged);
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Answer By";
			this.columnHeader4.Width = 91;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Answer";
			this.columnHeader5.Width = 508;
			// 
			// panel10
			// 
			this.panel10.Controls.Add(this.textPreviousQuestion);
			this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel10.Location = new System.Drawing.Point(0, 0);
			this.panel10.Name = "panel10";
			this.panel10.Size = new System.Drawing.Size(624, 48);
			this.panel10.TabIndex = 5;
			// 
			// textPreviousQuestion
			// 
			this.textPreviousQuestion.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.textPreviousQuestion.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textPreviousQuestion.Location = new System.Drawing.Point(0, 0);
			this.textPreviousQuestion.Multiline = true;
			this.textPreviousQuestion.Name = "textPreviousQuestion";
			this.textPreviousQuestion.ReadOnly = true;
			this.textPreviousQuestion.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textPreviousQuestion.Size = new System.Drawing.Size(624, 48);
			this.textPreviousQuestion.TabIndex = 3;
			this.textPreviousQuestion.Text = "";
			// 
			// panel9
			// 
			this.panel9.Controls.Add(this.textAnswer);
			this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel9.Location = new System.Drawing.Point(0, 176);
			this.panel9.Name = "panel9";
			this.panel9.Size = new System.Drawing.Size(624, 56);
			this.panel9.TabIndex = 4;
			// 
			// textAnswer
			// 
			this.textAnswer.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.textAnswer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textAnswer.Location = new System.Drawing.Point(0, 0);
			this.textAnswer.Multiline = true;
			this.textAnswer.Name = "textAnswer";
			this.textAnswer.ReadOnly = true;
			this.textAnswer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textAnswer.Size = new System.Drawing.Size(624, 56);
			this.textAnswer.TabIndex = 2;
			this.textAnswer.Text = "";
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.panel6);
			this.panel5.Controls.Add(this.panel7);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel5.Location = new System.Drawing.Point(0, 0);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(624, 120);
			this.panel5.TabIndex = 7;
			// 
			// panel6
			// 
			this.panel6.Controls.Add(this.listQuestions);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel6.Location = new System.Drawing.Point(0, 24);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(624, 96);
			this.panel6.TabIndex = 9;
			// 
			// listQuestions
			// 
			this.listQuestions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							this.columnHeader1,
																							this.columnHeader2});
			this.listQuestions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listQuestions.FullRowSelect = true;
			this.listQuestions.HideSelection = false;
			this.listQuestions.Location = new System.Drawing.Point(0, 0);
			this.listQuestions.MultiSelect = false;
			this.listQuestions.Name = "listQuestions";
			this.listQuestions.Size = new System.Drawing.Size(624, 96);
			this.listQuestions.TabIndex = 1;
			this.listQuestions.View = System.Windows.Forms.View.Details;
			this.listQuestions.Click += new System.EventHandler(this.listQuestions_Click);
			this.listQuestions.SelectedIndexChanged += new System.EventHandler(this.listQuestions_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "ID";
			this.columnHeader1.Width = 34;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Question";
			this.columnHeader2.Width = 561;
			// 
			// panel7
			// 
			this.panel7.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.panel7.Controls.Add(this.label2);
			this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel7.Location = new System.Drawing.Point(0, 0);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(624, 24);
			this.panel7.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.TabIndex = 0;
			this.label2.Text = "Questions";
			// 
			// bodyTopPanel
			// 
			this.bodyTopPanel.Controls.Add(this.panel1);
			this.bodyTopPanel.Controls.Add(this.panel3);
			this.bodyTopPanel.Controls.Add(this.panel2);
			this.bodyTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.bodyTopPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bodyTopPanel.Location = new System.Drawing.Point(0, 0);
			this.bodyTopPanel.Name = "bodyTopPanel";
			this.bodyTopPanel.Size = new System.Drawing.Size(624, 104);
			this.bodyTopPanel.TabIndex = 0;
			this.bodyTopPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.bodyTopPanel_Paint);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.textQuestion);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(624, 40);
			this.panel1.TabIndex = 10;
			// 
			// textQuestion
			// 
			this.textQuestion.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textQuestion.Location = new System.Drawing.Point(0, 0);
			this.textQuestion.Multiline = true;
			this.textQuestion.Name = "textQuestion";
			this.textQuestion.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textQuestion.Size = new System.Drawing.Size(624, 40);
			this.textQuestion.TabIndex = 0;
			this.textQuestion.Text = "";
			this.textQuestion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textQuestion_KeyPress);
			this.textQuestion.TextChanged += new System.EventHandler(this.textQuestion_TextChanged);
			this.textQuestion.Enter += new System.EventHandler(this.textQuestion_Enter);
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.panel3.Controls.Add(this.button1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 64);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(624, 40);
			this.panel3.TabIndex = 9;
			// 
			// button1
			// 
			this.button1.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.button1.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.button1.HottrackImage = ((System.Drawing.Image)(resources.GetObject("button1.HottrackImage")));
			this.button1.Location = new System.Drawing.Point(552, 8);
			this.button1.Name = "button1";
			this.button1.NormalImage = ((System.Drawing.Image)(resources.GetObject("button1.NormalImage")));
			this.button1.OnlyShowBitmap = true;
			this.button1.PressedImage = ((System.Drawing.Image)(resources.GetObject("button1.PressedImage")));
			this.button1.Size = new System.Drawing.Size(62, 27);
			this.button1.Text = "nsButton1";
			this.button1.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.button1.ToolTip = null;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.panel2.Controls.Add(this.label1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(624, 24);
			this.panel2.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Ask Question";
			// 
			// QuestionAnswerAttendee
			// 
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.baseBottomPanel);
			this.Controls.Add(this.baseRightPanel);
			this.Controls.Add(this.baseLeftPanel);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "QuestionAnswerAttendee";
			this.Size = new System.Drawing.Size(656, 464);
			this.panel4.ResumeLayout(false);
			this.panelBody.ResumeLayout(false);
			this.panel8.ResumeLayout(false);
			this.panel11.ResumeLayout(false);
			this.panel10.ResumeLayout(false);
			this.panel9.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			this.bodyTopPanel.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}
		public static int QuestionID = 1;
		public ListViewItem lv;
		private void button1_Click(object sender, System.EventArgs e)
		{
			try
			{
				QAQuestion qa = new QAQuestion(NetworkManager.getInstance().profile);
				textQuestion.Text=textQuestion.Text.Replace("\r"," ");
				textQuestion.Text=textQuestion.Text.Replace("\n"," ");
				if(textQuestion.Text.Trim()=="")
					return;
				qa.Question = textQuestion.Text;
				qa.QID = QuestionID;
				NetworkManager.getInstance().SendLoadPacket(qa);
				QuestionID++;
				AnswerObject answer = new AnswerObject(qa.QID);
				answer.Question = qa.Question;
				lv = listQuestions.Items.Add(qa.QID.ToString());

				qa.Question = qa.Question.Replace("\r"," ");
				qa.Question = qa.Question.Replace("\n"," ");
				qa.Question=qa.Question.Trim();
				if(qa.Question.Length > 150)
					qa.Question = qa.Question.Substring(0,150) + "...";
				lv.SubItems.Add(qa.Question);		
				lv.Tag = answer;
				textQuestion.Text="";
				textAnswer.Text="";
				//ClientUI.getInstance().tabBody.SelectedTabEx = ClientUI.getInstance().tabBody.TabPages[ClientUI.getInstance().nLastSelectedIndex];
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("QuestionAnswerAttendee.cs line==> 473",exp,null,false);			
			}
			
		}

		public void HandleAnswer(QAAnswer msg)
		{
			int selected_index=0;
			try
			{
				for(int i = 0 ; i < listQuestions.Items.Count ; i++)
				{
					if(((AnswerObject)listQuestions.Items[i].Tag).QID == msg.QID)
					{
						listQuestions.Items[i].Selected=true;
						selected_index=i;
						AnswerObject answer = (AnswerObject)listQuestions.Items[i].Tag;
						answer.answersArray.Add(msg.Answer);
						answer.answersByArray.Add(msg.senderProfile.Name);										
						break;
					}
				}			
				showQuestionsAnswers(selected_index);
				//listQuestions.Items[selected_index].Selected=true;
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("QuestionAnswerAttendee.cs line==> 496",exp,null,false);			
			}
		}

		private void showQuestionsAnswers(int index)
		{
			try
			{
				if(listQuestions.SelectedItems.Count < 1)
					return;
				
//				if(index!=0)
//					index=index+1;
//				
				
				// 
				AnswerObject answer;
				
				if(index!=0)
				{
					answer = (AnswerObject)listQuestions.Items[index].Tag;
				}
				else
				{
					answer = (AnswerObject)listQuestions.SelectedItems[0].Tag;
				
				}





				textPreviousQuestion.Text = answer.Question;			
				listAnswers.Items.Clear();
				for(int i = 0 ; i < answer.answersArray.Count; i++)
				{
					string str = (string)answer.answersArray[i];
					str=str.Replace("\r"," ");
					str=str.Replace("\n"," ");
					if(str.Length > 150)
						str = str.Substring(0,150);
					ListViewItem lv = listAnswers.Items.Add(answer.answersByArray[i].ToString());
					lv.SubItems.Add(str);
					lv.Tag = answer.answersArray[i];
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("QuestionAnswerAttendee.cs line==> 523",exp,null,false);			
			}

		}
		private void listQuestions_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			showQuestionsAnswers(0);
			textAnswer.Text="";
			
		}

		private void listAnswers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				if(listAnswers.SelectedItems.Count < 1)
					return;

				textAnswer.Text = listAnswers.Items[listAnswers.SelectedItems[0].Index].Tag.ToString();
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("QuestionAnswerAttendee.cs line==> 550",exp,null,false);			
			}

			
			
		}

		private void bodyTopPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void textQuestion_TextChanged(object sender, System.EventArgs e)
		{
			if(textQuestion.Text=="\r\n")			
				textQuestion.Text="";			
			button1.Enabled = (textQuestion.Text.Length > 0);
		}

		private void listQuestions_Click(object sender, System.EventArgs e)
		{
			
			showQuestionsAnswers(0);
		}

		private void textQuestion_Enter(object sender, System.EventArgs e)
		{
			
		}

		private void textQuestion_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar==13)
			{
				if(textQuestion.Text.Trim()!="")
				{				
					button1_Click(null,null);					
					textQuestion.Clear();
				}
			}
		}
	}

	public class AnswerObject
	{
		public int QID;
		public string Question;
		public ArrayList answersArray = new ArrayList();
		public ArrayList answersByArray = new ArrayList();

		public AnswerObject(int QuestionID)
		{
			QID = QuestionID;
		}
	}
	}
