using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using WebMeeting;
using WebMeeting.Common;
using WebMeeting.Client;


namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for QuestionAnswerPresenter.
	/// </summary>
	public class QuestionAnswerPresenter : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel baseLeftPanel;
		private System.Windows.Forms.Panel baseRightPanel;
		private System.Windows.Forms.Panel baseBottomPanel;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel bodyTopPanel;
		private System.Windows.Forms.Panel bodyTopLeft;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panelTopLeftTop;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel bodyBottomPanel;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Panel panel9;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel10;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textQuestion;
		private System.Windows.Forms.Panel panel11;
		private System.Windows.Forms.TextBox textAnswer;
		private System.Windows.Forms.Panel panel12;
		private System.Windows.Forms.ListView listPrevious;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ListView listNotAnswered;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ListView listAnswered;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private NSPAControls.NSButton button1;
		private System.Windows.Forms.Panel panel13;
		private System.Windows.Forms.Panel panel14;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.Splitter splitter3;
		private NSPAControls.NSButton nsbtn_ignore;
	
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public bool ShowClose()
		{
			return true;
		}

		public QuestionAnswerPresenter()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(QuestionAnswerPresenter));
			this.baseLeftPanel = new System.Windows.Forms.Panel();
			this.baseRightPanel = new System.Windows.Forms.Panel();
			this.baseBottomPanel = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.splitter3 = new System.Windows.Forms.Splitter();
			this.panel14 = new System.Windows.Forms.Panel();
			this.panel13 = new System.Windows.Forms.Panel();
			this.textQuestion = new System.Windows.Forms.TextBox();
			this.panel10 = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.bodyBottomPanel = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel8 = new System.Windows.Forms.Panel();
			this.listPrevious = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.panel12 = new System.Windows.Forms.Panel();
			this.panel9 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.panel5 = new System.Windows.Forms.Panel();
			this.textAnswer = new System.Windows.Forms.TextBox();
			this.panel6 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.panel11 = new System.Windows.Forms.Panel();
			this.nsbtn_ignore = new NSPAControls.NSButton();
			this.button1 = new NSPAControls.NSButton();
			this.bodyTopPanel = new System.Windows.Forms.Panel();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.listAnswered = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.bodyTopLeft = new System.Windows.Forms.Panel();
			this.listNotAnswered = new System.Windows.Forms.ListView();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.panelTopLeftTop = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel4.SuspendLayout();
			this.panel14.SuspendLayout();
			this.panel13.SuspendLayout();
			this.panel10.SuspendLayout();
			this.bodyBottomPanel.SuspendLayout();
			this.panel8.SuspendLayout();
			this.panel9.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel11.SuspendLayout();
			this.bodyTopPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.bodyTopLeft.SuspendLayout();
			this.panelTopLeftTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// baseLeftPanel
			// 
			this.baseLeftPanel.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(179)), ((System.Byte)(239)));
			this.baseLeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.baseLeftPanel.Location = new System.Drawing.Point(0, 0);
			this.baseLeftPanel.Name = "baseLeftPanel";
			this.baseLeftPanel.Size = new System.Drawing.Size(8, 464);
			this.baseLeftPanel.TabIndex = 0;
			// 
			// baseRightPanel
			// 
			this.baseRightPanel.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(179)), ((System.Byte)(239)));
			this.baseRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.baseRightPanel.Location = new System.Drawing.Point(560, 0);
			this.baseRightPanel.Name = "baseRightPanel";
			this.baseRightPanel.Size = new System.Drawing.Size(8, 464);
			this.baseRightPanel.TabIndex = 1;
			// 
			// baseBottomPanel
			// 
			this.baseBottomPanel.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(179)), ((System.Byte)(239)));
			this.baseBottomPanel.Location = new System.Drawing.Point(8, 456);
			this.baseBottomPanel.Name = "baseBottomPanel";
			this.baseBottomPanel.Size = new System.Drawing.Size(552, 8);
			this.baseBottomPanel.TabIndex = 2;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.splitter3);
			this.panel4.Controls.Add(this.panel14);
			this.panel4.Controls.Add(this.bodyBottomPanel);
			this.panel4.Controls.Add(this.bodyTopPanel);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(8, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(552, 464);
			this.panel4.TabIndex = 3;
			// 
			// splitter3
			// 
			this.splitter3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter3.Location = new System.Drawing.Point(0, 360);
			this.splitter3.Name = "splitter3";
			this.splitter3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitter3.Size = new System.Drawing.Size(552, 3);
			this.splitter3.TabIndex = 7;
			this.splitter3.TabStop = false;
			// 
			// panel14
			// 
			this.panel14.Controls.Add(this.panel13);
			this.panel14.Controls.Add(this.panel10);
			this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel14.Location = new System.Drawing.Point(0, 360);
			this.panel14.Name = "panel14";
			this.panel14.Size = new System.Drawing.Size(552, 104);
			this.panel14.TabIndex = 6;
			// 
			// panel13
			// 
			this.panel13.Controls.Add(this.textQuestion);
			this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel13.Location = new System.Drawing.Point(0, 32);
			this.panel13.Name = "panel13";
			this.panel13.Size = new System.Drawing.Size(552, 72);
			this.panel13.TabIndex = 5;
			// 
			// textQuestion
			// 
			this.textQuestion.BackColor = System.Drawing.Color.White;
			this.textQuestion.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textQuestion.Location = new System.Drawing.Point(0, 0);
			this.textQuestion.Multiline = true;
			this.textQuestion.Name = "textQuestion";
			this.textQuestion.ReadOnly = true;
			this.textQuestion.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textQuestion.Size = new System.Drawing.Size(552, 72);
			this.textQuestion.TabIndex = 4;
			this.textQuestion.Text = "";
			// 
			// panel10
			// 
			this.panel10.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(179)), ((System.Byte)(239)));
			this.panel10.Controls.Add(this.label5);
			this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel10.Location = new System.Drawing.Point(0, 0);
			this.panel10.Name = "panel10";
			this.panel10.Size = new System.Drawing.Size(552, 32);
			this.panel10.TabIndex = 3;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.label5.Location = new System.Drawing.Point(0, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(136, 23);
			this.label5.TabIndex = 0;
			this.label5.Text = "Question Text";
			// 
			// bodyBottomPanel
			// 
			this.bodyBottomPanel.Controls.Add(this.splitter1);
			this.bodyBottomPanel.Controls.Add(this.panel8);
			this.bodyBottomPanel.Controls.Add(this.panel5);
			this.bodyBottomPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.bodyBottomPanel.Location = new System.Drawing.Point(0, 136);
			this.bodyBottomPanel.Name = "bodyBottomPanel";
			this.bodyBottomPanel.Size = new System.Drawing.Size(552, 224);
			this.bodyBottomPanel.TabIndex = 1;
			// 
			// splitter1
			// 
			this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter1.Location = new System.Drawing.Point(288, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 224);
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// panel8
			// 
			this.panel8.Controls.Add(this.listPrevious);
			this.panel8.Controls.Add(this.panel12);
			this.panel8.Controls.Add(this.panel9);
			this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel8.Location = new System.Drawing.Point(288, 0);
			this.panel8.Name = "panel8";
			this.panel8.Size = new System.Drawing.Size(264, 224);
			this.panel8.TabIndex = 2;
			// 
			// listPrevious
			// 
			this.listPrevious.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						   this.columnHeader1});
			this.listPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listPrevious.FullRowSelect = true;
			this.listPrevious.Location = new System.Drawing.Point(0, 32);
			this.listPrevious.MultiSelect = false;
			this.listPrevious.Name = "listPrevious";
			this.listPrevious.Size = new System.Drawing.Size(264, 152);
			this.listPrevious.TabIndex = 5;
			this.listPrevious.View = System.Windows.Forms.View.Details;
			this.listPrevious.Click += new System.EventHandler(this.listPrevious_Click);
			this.listPrevious.SelectedIndexChanged += new System.EventHandler(this.listPrevious_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Answer";
			this.columnHeader1.Width = 303;
			// 
			// panel12
			// 
			this.panel12.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(179)), ((System.Byte)(239)));
			this.panel12.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel12.Location = new System.Drawing.Point(0, 184);
			this.panel12.Name = "panel12";
			this.panel12.Size = new System.Drawing.Size(264, 40);
			this.panel12.TabIndex = 7;
			// 
			// panel9
			// 
			this.panel9.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(179)), ((System.Byte)(239)));
			this.panel9.Controls.Add(this.label4);
			this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel9.Location = new System.Drawing.Point(0, 0);
			this.panel9.Name = "panel9";
			this.panel9.Size = new System.Drawing.Size(264, 32);
			this.panel9.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.label4.Location = new System.Drawing.Point(8, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(136, 23);
			this.label4.TabIndex = 0;
			this.label4.Text = "Previous Answers";
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.textAnswer);
			this.panel5.Controls.Add(this.panel6);
			this.panel5.Controls.Add(this.panel11);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel5.Location = new System.Drawing.Point(0, 0);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(288, 224);
			this.panel5.TabIndex = 1;
			// 
			// textAnswer
			// 
			this.textAnswer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textAnswer.Location = new System.Drawing.Point(0, 32);
			this.textAnswer.Multiline = true;
			this.textAnswer.Name = "textAnswer";
			this.textAnswer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textAnswer.Size = new System.Drawing.Size(288, 152);
			this.textAnswer.TabIndex = 3;
			this.textAnswer.Text = "";
			this.textAnswer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textAnswer_KeyPress);
			this.textAnswer.TextChanged += new System.EventHandler(this.textAnswer_TextChanged);
			this.textAnswer.Enter += new System.EventHandler(this.textAnswer_Enter);
			// 
			// panel6
			// 
			this.panel6.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(179)), ((System.Byte)(239)));
			this.panel6.Controls.Add(this.label3);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel6.Location = new System.Drawing.Point(0, 0);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(288, 32);
			this.panel6.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.label3.Location = new System.Drawing.Point(0, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "Answer";
			// 
			// panel11
			// 
			this.panel11.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(179)), ((System.Byte)(239)));
			this.panel11.Controls.Add(this.nsbtn_ignore);
			this.panel11.Controls.Add(this.button1);
			this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel11.Location = new System.Drawing.Point(0, 184);
			this.panel11.Name = "panel11";
			this.panel11.Size = new System.Drawing.Size(288, 40);
			this.panel11.TabIndex = 6;
			// 
			// nsbtn_ignore
			// 
			this.nsbtn_ignore.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.nsbtn_ignore.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.nsbtn_ignore.HottrackImage = ((System.Drawing.Image)(resources.GetObject("nsbtn_ignore.HottrackImage")));
			this.nsbtn_ignore.Location = new System.Drawing.Point(72, 8);
			this.nsbtn_ignore.Name = "nsbtn_ignore";
			this.nsbtn_ignore.NormalImage = ((System.Drawing.Image)(resources.GetObject("nsbtn_ignore.NormalImage")));
			this.nsbtn_ignore.OnlyShowBitmap = true;
			this.nsbtn_ignore.PressedImage = ((System.Drawing.Image)(resources.GetObject("nsbtn_ignore.PressedImage")));
			this.nsbtn_ignore.Size = new System.Drawing.Size(62, 27);
			this.nsbtn_ignore.Text = "Ignore";
			this.nsbtn_ignore.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.nsbtn_ignore.ToolTip = null;
			this.nsbtn_ignore.Click += new System.EventHandler(this.nsbtn_ignore_Click);
			// 
			// button1
			// 
			this.button1.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.button1.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.button1.HottrackImage = ((System.Drawing.Image)(resources.GetObject("button1.HottrackImage")));
			this.button1.Location = new System.Drawing.Point(0, 8);
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
			// bodyTopPanel
			// 
			this.bodyTopPanel.Controls.Add(this.splitter2);
			this.bodyTopPanel.Controls.Add(this.panel1);
			this.bodyTopPanel.Controls.Add(this.bodyTopLeft);
			this.bodyTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.bodyTopPanel.Location = new System.Drawing.Point(0, 0);
			this.bodyTopPanel.Name = "bodyTopPanel";
			this.bodyTopPanel.Size = new System.Drawing.Size(552, 136);
			this.bodyTopPanel.TabIndex = 0;
			// 
			// splitter2
			// 
			this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitter2.Location = new System.Drawing.Point(288, 0);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(3, 136);
			this.splitter2.TabIndex = 2;
			this.splitter2.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.listAnswered);
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(288, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(264, 136);
			this.panel1.TabIndex = 1;
			// 
			// listAnswered
			// 
			this.listAnswered.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						   this.columnHeader4,
																						   this.columnHeader7,
																						   this.columnHeader5});
			this.listAnswered.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listAnswered.FullRowSelect = true;
			this.listAnswered.HideSelection = false;
			this.listAnswered.Location = new System.Drawing.Point(0, 32);
			this.listAnswered.MultiSelect = false;
			this.listAnswered.Name = "listAnswered";
			this.listAnswered.Size = new System.Drawing.Size(264, 104);
			this.listAnswered.TabIndex = 4;
			this.listAnswered.View = System.Windows.Forms.View.Details;
			this.listAnswered.Click += new System.EventHandler(this.listAnswered_Click);
			this.listAnswered.SelectedIndexChanged += new System.EventHandler(this.listAnswered_SelectedIndexChanged);
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "#";
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Sender";
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Question";
			this.columnHeader5.Width = 436;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(179)), ((System.Byte)(239)));
			this.panel3.Controls.Add(this.label2);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.ForeColor = System.Drawing.SystemColors.InactiveCaption;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(264, 32);
			this.panel3.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 23);
			this.label2.TabIndex = 0;
			this.label2.Text = "Questions Answered";
			// 
			// bodyTopLeft
			// 
			this.bodyTopLeft.Controls.Add(this.listNotAnswered);
			this.bodyTopLeft.Controls.Add(this.panelTopLeftTop);
			this.bodyTopLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.bodyTopLeft.Location = new System.Drawing.Point(0, 0);
			this.bodyTopLeft.Name = "bodyTopLeft";
			this.bodyTopLeft.Size = new System.Drawing.Size(288, 136);
			this.bodyTopLeft.TabIndex = 0;
			// 
			// listNotAnswered
			// 
			this.listNotAnswered.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							  this.columnHeader2,
																							  this.columnHeader6,
																							  this.columnHeader3});
			this.listNotAnswered.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listNotAnswered.FullRowSelect = true;
			this.listNotAnswered.HideSelection = false;
			this.listNotAnswered.Location = new System.Drawing.Point(0, 32);
			this.listNotAnswered.MultiSelect = false;
			this.listNotAnswered.Name = "listNotAnswered";
			this.listNotAnswered.Size = new System.Drawing.Size(288, 104);
			this.listNotAnswered.TabIndex = 2;
			this.listNotAnswered.View = System.Windows.Forms.View.Details;
			this.listNotAnswered.Click += new System.EventHandler(this.listNotAnswered_Click);
			this.listNotAnswered.DoubleClick += new System.EventHandler(this.listNotAnswered_DoubleClick);
			this.listNotAnswered.SelectedIndexChanged += new System.EventHandler(this.listNotAnswered_SelectedIndexChanged);
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "#";
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Sender";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Question";
			this.columnHeader3.Width = 215;
			// 
			// panelTopLeftTop
			// 
			this.panelTopLeftTop.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(179)), ((System.Byte)(239)));
			this.panelTopLeftTop.Controls.Add(this.label1);
			this.panelTopLeftTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTopLeftTop.Location = new System.Drawing.Point(0, 0);
			this.panelTopLeftTop.Name = "panelTopLeftTop";
			this.panelTopLeftTop.Size = new System.Drawing.Size(288, 32);
			this.panelTopLeftTop.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.label1.Location = new System.Drawing.Point(0, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(160, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Questiosn Not Answered";
			// 
			// QuestionAnswerPresenter
			// 
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.baseBottomPanel);
			this.Controls.Add(this.baseRightPanel);
			this.Controls.Add(this.baseLeftPanel);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "QuestionAnswerPresenter";
			this.Size = new System.Drawing.Size(568, 464);
			this.panel4.ResumeLayout(false);
			this.panel14.ResumeLayout(false);
			this.panel13.ResumeLayout(false);
			this.panel10.ResumeLayout(false);
			this.bodyBottomPanel.ResumeLayout(false);
			this.panel8.ResumeLayout(false);
			this.panel9.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.panel6.ResumeLayout(false);
			this.panel11.ResumeLayout(false);
			this.bodyTopPanel.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.bodyTopLeft.ResumeLayout(false);
			this.panelTopLeftTop.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
			
		public void HandleQuestion(QAQuestion msg)
		{
			try
			{
				ListViewItem lv =  listNotAnswered.Items.Insert(0,msg.QID.ToString());
				lv.SubItems.Add(msg.senderProfile.Name);
				lv.Tag = msg.Question;
				msg.Question = msg.Question.Replace("\r"," ");
				msg.Question = msg.Question.Replace("\n"," ");
				if(msg.Question.Length > 25)
					msg.Question = msg.Question.Substring(0,25);
				lv.SubItems.Add(msg.Question);            					
			
				QuestionObject qa = new QuestionObject();
				qa.QID = msg.QID;
				qa.Question = msg.Question;
				qa.RecipientID = msg.senderProfile.ClientId;
				qa.senderName = msg.senderProfile.Name;

				lv.Tag = qa;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("QuestionAnswerPresenter.cs line==> 600",exp,null,false);			
		
			}
		}
		bool bAnsweringAlreadyAnswered = false;

		private void listNotAnswered_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(listNotAnswered.SelectedItems.Count < 1)
				return;
		
			textQuestion.Text = ((QuestionObject)listNotAnswered.Items[listNotAnswered.SelectedItems[0].Index].Tag).Question;
			bAnsweringAlreadyAnswered = false;
		}
		
		void ShowAnsweredQuestions(int answered_index)
		{
			string str="";
			try
			{


				if(answered_index==0)
				{
					listAnswered.Items[0].Selected=true;
					//nIndex = 0;
				}


				if(listAnswered.SelectedItems.Count < 1)
					return;

				int nIndex=0;

				if(answered_index==0)
				{
					nIndex = listAnswered.Items[0].Index;
					//nIndex = 0;
				}
				else
				{
					nIndex = listAnswered.SelectedItems[0].Index;
				}


				QuestionObject obj = (QuestionObject)listAnswered.Items[nIndex].Tag;
				listPrevious.Items.Clear();
				for(int i = 0 ; i < obj.Answers.Count ; i++)
				{

					//ListViewItem lv = listPrevious.Items.Add(obj.Answers[i].ToString());
					str=obj.Answers[i].ToString();
					str=str.Replace("\r"," ");
					str=str.Replace("\n"," ");

					ListViewItem lv = listPrevious.Items.Add(str);
					lv.Tag = obj.Answers[i].ToString();				
				}
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("QuestionAnswerPresenter.cs line==> 632",exp,null,false);			
			}
		}
		private void listAnswered_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			bAnsweringAlreadyAnswered = true;
			ShowAnsweredQuestions(-1);

		}

		public void button1_Click(object sender, System.EventArgs e)
		{
			//first locate if we are answering a virgin PC
			try
			{


				/*	
				private void listNotAnswered_SelectedIndexChanged(object sender, System.EventArgs e)
				{
					if(listNotAnswered.SelectedItems.Count < 1)
						return;
		
					textQuestion.Text = ((QuestionObject)listNotAnswered.Items[listNotAnswered.SelectedItems[0].Index].Tag).Question;
					bAnsweringAlreadyAnswered = false;
				}
				*/
			// If The questions to be answered are clicked 
			// this bool is set to false

			// If The questions not to be answered are clicked 
			// this bool is set to  true.


				// True goes into if==>If The questions to be answered are clicked 
				if(!bAnsweringAlreadyAnswered)
				{
					if(listNotAnswered.SelectedItems.Count < 1)
						return;
					QuestionObject obj = (QuestionObject)listNotAnswered.SelectedItems[0].Tag;
					QAAnswer answerMessage = new QAAnswer(obj.QID,NetworkManager.getInstance().profile,obj.RecipientID);
					answerMessage.Answer = textAnswer.Text;
					answerMessage.Question = textQuestion.Text;
					NetworkManager.getInstance().SendLoadPacket(answerMessage);
                
					//now move the item to answered items
					ListViewItem lv = listAnswered.Items.Insert(0,answerMessage.QID.ToString());
					lv.SubItems.Add(obj.senderName);
					lv.SubItems.Add(obj.Question);
					lv.Tag = obj.Clone();              	
					listNotAnswered.Items.RemoveAt(listNotAnswered.SelectedItems[0].Index);
					obj = (QuestionObject)lv.Tag;
					obj.Answers.Add(textAnswer.Text);

					textAnswer.Text = "";

				}
				else
				{
					if(listAnswered.SelectedItems.Count < 1)
						return;

					QuestionObject obj = (QuestionObject)listAnswered.SelectedItems[0].Tag;
					QAAnswer answerMessage = new QAAnswer(obj.QID,NetworkManager.getInstance().profile,obj.RecipientID);
					answerMessage.Answer = textAnswer.Text;
					answerMessage.Question = textQuestion.Text;
					NetworkManager.getInstance().SendLoadPacket(answerMessage);

					obj.Answers.Add(textAnswer.Text);
					textAnswer.Text = "";

				}
				// It means 0 to be selected by default 
				ShowAnsweredQuestions(0);

				textQuestion.Text="";
				
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("QuestionAnswerPresenter.cs line==> 688",exp,null,false);			
			}
	
		}

		private void listPrevious_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(listPrevious.SelectedItems.Count < 1)
				return;
			int nIndex = listPrevious.SelectedItems[0].Index;
			textAnswer.Text = listPrevious.Items[nIndex].Tag.ToString();
			
		}

		private void textAnswer_TextChanged(object sender, System.EventArgs e)
		{
			if(textAnswer.Text.Trim().Length < 1)
				button1.Enabled = false;
			else
			{				
				if((listNotAnswered.SelectedItems.Count > 0) || (listAnswered.SelectedItems.Count > 0))
					button1.Enabled = true;

			}

		}

		private void listNotAnswered_Click(object sender, System.EventArgs e)
		{
			
			
			textAnswer.ReadOnly = false;
			

		}

		private void listAnswered_Click(object sender, System.EventArgs e)
		{
				
			textAnswer.ReadOnly = false;

		}

		private void listPrevious_Click(object sender, System.EventArgs e)
		{			
			try
			{
				if(listPrevious.SelectedItems.Count < 1)
					return;
				int nIndex = listPrevious.SelectedItems[0].Index;
				textAnswer.Text = listPrevious.Items[nIndex].Tag.ToString();
				textAnswer.ReadOnly = true;
			}
			catch(Exception exp)
			{
			
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("QuestionAnswerPresenter.cs line==> 744",exp,null,false);			
			}
		}

		private void listNotAnswered_DoubleClick(object sender, System.EventArgs e)
		{
			//textAnswer.Focus();
			if(this.button1.Enabled)
				this.button1_Click(sender,e);
		}

		private void splitter1_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
		
		}

		private void textAnswer_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			
			if( e.KeyChar == '\r' )
			{
			//if(listPrevious.SelectedItems.Count < 1)
			//return;

			if(this.button1.Enabled)
			this.button1_Click(sender,e);

			}
		}

		private void nsbtn_ignore_Click(object sender, System.EventArgs e)
		{
		
				if(listNotAnswered.SelectedItems.Count < 1)
					return;
				listNotAnswered.Items.RemoveAt(listNotAnswered.SelectedItems[0].Index);
				textQuestion.Text="";
		
		}

		private void textAnswer_Enter(object sender, System.EventArgs e)
		{
			if(textAnswer.Text.Trim().Length < 1)
				button1.Enabled = false;
			else
			{				
				if((listNotAnswered.SelectedItems.Count > 0) || (listAnswered.SelectedItems.Count > 0))
					button1.Enabled = true;

			}
		}


	}
	public class QuestionObject
	{
		public int QID;
		public string Question;
		public int RecipientID;
		public string senderName;
		public ArrayList Answers= new ArrayList();
		public QuestionObject()
		{				


		}
		public QuestionObject Clone()
		{	
			QuestionObject qa = new QuestionObject();
			qa.Question = Question;
			qa.QID = QID;
			qa.RecipientID	= RecipientID;
			qa.senderName = senderName;
			qa.Answers = Answers;

			return qa;
		}
	}

}
