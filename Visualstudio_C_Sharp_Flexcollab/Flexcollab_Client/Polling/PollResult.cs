using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data;
using System.IO;
using WebMeeting.Common;
using System.Drawing.PieChart;
using System.Threading;

namespace WebMeeting.Polling
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class PollResult :System.Windows.Forms.UserControl
	{
		public System.Drawing.PieChart.PieChartControl m_panelDrawing;

		#region Controls

		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.NumericUpDown numericUpDownValue1;
		private System.Windows.Forms.NumericUpDown numericUpDownValue2;
		private System.Windows.Forms.NumericUpDown numericUpDownValue3;
		private System.Windows.Forms.NumericUpDown numericUpDownValue4;
		private System.Windows.Forms.NumericUpDown numericUpDownValue5;
		private System.Windows.Forms.NumericUpDown numericUpDownDisplacement1;
		private System.Windows.Forms.NumericUpDown numericUpDownDisplacement2;
		private System.Windows.Forms.NumericUpDown numericUpDownDisplacement3;
		private System.Windows.Forms.NumericUpDown numericUpDownDisplacement4;
		private System.Windows.Forms.NumericUpDown numericUpDownDisplacement5;
		private System.Windows.Forms.NumericUpDown numericUpDownValue6;
		private System.Windows.Forms.NumericUpDown numericUpDownDisplacement6;
		private System.Windows.Forms.Button buttonColor1;
		private System.Windows.Forms.Button buttonColor2;
		private System.Windows.Forms.Button buttonColor3;
		private System.Windows.Forms.Button buttonColor4;
		private System.Windows.Forms.Button buttonColor5;
		private System.Windows.Forms.Button buttonColor6;
		private System.Windows.Forms.ColorDialog m_colorDialog;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox textBoxToolTip1;
		private System.Windows.Forms.TextBox textBoxToolTip2;
		private System.Windows.Forms.TextBox textBoxToolTip3;
		private System.Windows.Forms.TextBox textBoxToolTip4;
		private System.Windows.Forms.TextBox textBoxToolTip5;
		private System.Windows.Forms.TextBox textBoxToolTip6;

		#endregion

		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ColumnHeader ColumnOption;
		private System.Windows.Forms.ColumnHeader columnColor;
		private System.Windows.Forms.ColumnHeader columnCount;
		private System.Windows.Forms.ColumnHeader ColumnPercentage;
		private System.Windows.Forms.ColumnHeader columnName;
		private System.Windows.Forms.ColumnHeader columnOptionSelected;
		public System.Windows.Forms.ListView listViewAttendeeAnswers;
		ArrayList BarChartEntries = new ArrayList();

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel panel2;
		private NSPAControls.NSButton button1;
		private NSPAControls.NSButton button2;
		public Polling polling;		
		private System.Windows.Forms.PictureBox barGraphBox;
		private System.Windows.Forms.Button btnBarGraph;
		private System.Windows.Forms.Panel panelTop;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel5;
		private NSPAControls.NSButton btnBarGraphEx;
		private System.Windows.Forms.Button btnOption;
		private NSPAControls.NSButton btnPieChart;
		public System.Windows.Forms.Label lblQuestion;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Panel panel4;
		public bool SharedPoolResult=false;
		
		public const int WM_DELETE_POLL_RESULT = 88194;
		private System.Windows.Forms.Label label1;

		WebMeeting.Polling.PollingOptions objPollingOptions = new PollingOptions();

		//	#region Userdeclared variabled
			
		# region WndProc(ref System.Windows.Forms.Message m)
		//public	const int WM_REMOVE_MANAGE_CONTENT_TAB = 99195;
		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			
			try
			{
			switch(m.Msg)
			{
				case WM_DELETE_POLL_RESULT:
					try
					{
						this.closeit();
					}
					catch(Exception)// ee)
					{

					}					
					break;				
				default:
					base.WndProc(ref m);                
					break;
			}
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollResult.cs @WndProc(ref System.Windows.Forms.Message m) line==> 127",exp,null,false);
			}
			
		}		
		# endregion

		# region PollResult() 
		public PollResult() 
		{
			try
			{
				//Trace.WriteLine("constructor of Pool Result is called");
				InitializeComponent();
				FillEdgeColorTypeListBox();
				InitializeChart();
				barGraphBox.BackColor = Color.White;
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollResult.cs @PollResult()  line==> 148",exp,null,false);
			}

		}
		# endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		# region Dispose Method
		protected override void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		# endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PollResult));
			this.m_panelDrawing = new System.Drawing.PieChart.PieChartControl();
			this.barGraphBox = new System.Windows.Forms.PictureBox();
			this.numericUpDownValue6 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownValue5 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownValue4 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownValue3 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownValue2 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownValue1 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownDisplacement1 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownDisplacement6 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownDisplacement5 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownDisplacement4 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownDisplacement3 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownDisplacement2 = new System.Windows.Forms.NumericUpDown();
			this.buttonColor6 = new System.Windows.Forms.Button();
			this.buttonColor5 = new System.Windows.Forms.Button();
			this.buttonColor4 = new System.Windows.Forms.Button();
			this.buttonColor3 = new System.Windows.Forms.Button();
			this.buttonColor2 = new System.Windows.Forms.Button();
			this.buttonColor1 = new System.Windows.Forms.Button();
			this.m_colorDialog = new System.Windows.Forms.ColorDialog();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.textBoxToolTip6 = new System.Windows.Forms.TextBox();
			this.textBoxToolTip5 = new System.Windows.Forms.TextBox();
			this.textBoxToolTip4 = new System.Windows.Forms.TextBox();
			this.textBoxToolTip3 = new System.Windows.Forms.TextBox();
			this.textBoxToolTip2 = new System.Windows.Forms.TextBox();
			this.textBoxToolTip1 = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.ColumnOption = new System.Windows.Forms.ColumnHeader();
			this.columnColor = new System.Windows.Forms.ColumnHeader();
			this.columnCount = new System.Windows.Forms.ColumnHeader();
			this.ColumnPercentage = new System.Windows.Forms.ColumnHeader();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.panel4 = new System.Windows.Forms.Panel();
			this.listViewAttendeeAnswers = new System.Windows.Forms.ListView();
			this.columnName = new System.Windows.Forms.ColumnHeader();
			this.columnOptionSelected = new System.Windows.Forms.ColumnHeader();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.btnBarGraphEx = new NSPAControls.NSButton();
			this.btnPieChart = new NSPAControls.NSButton();
			this.btnBarGraph = new System.Windows.Forms.Button();
			this.button1 = new NSPAControls.NSButton();
			this.button2 = new NSPAControls.NSButton();
			this.btnOption = new System.Windows.Forms.Button();
			this.panelTop = new System.Windows.Forms.Panel();
			this.lblQuestion = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel5 = new System.Windows.Forms.Panel();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplacement1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplacement6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplacement5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplacement4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplacement3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplacement2)).BeginInit();
			this.groupBox6.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panelTop.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel5.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_panelDrawing
			// 
			this.m_panelDrawing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_panelDrawing.BackColor = System.Drawing.SystemColors.Window;
			this.m_panelDrawing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.m_panelDrawing.Location = new System.Drawing.Point(344, 48);
			this.m_panelDrawing.Name = "m_panelDrawing";
			this.m_panelDrawing.Size = new System.Drawing.Size(304, 208);
			this.m_panelDrawing.TabIndex = 0;
			this.m_panelDrawing.ToolTips = null;
			this.m_panelDrawing.MouseHover += new System.EventHandler(this.m_panelDrawing_MouseHover);
			// 
			// barGraphBox
			// 
			this.barGraphBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.barGraphBox.Location = new System.Drawing.Point(8, 48);
			this.barGraphBox.Name = "barGraphBox";
			this.barGraphBox.Size = new System.Drawing.Size(328, 208);
			this.barGraphBox.TabIndex = 0;
			this.barGraphBox.TabStop = false;
			this.barGraphBox.Paint += new System.Windows.Forms.PaintEventHandler(this.barGraphBox_Paint);
			// 
			// numericUpDownValue6
			// 
			this.numericUpDownValue6.Location = new System.Drawing.Point(32, 215);
			this.numericUpDownValue6.Name = "numericUpDownValue6";
			this.numericUpDownValue6.Size = new System.Drawing.Size(56, 21);
			this.numericUpDownValue6.TabIndex = 30;
			this.numericUpDownValue6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownValue6.Value = new System.Decimal(new int[] {
																			  25,
																			  0,
																			  0,
																			  0});
			this.numericUpDownValue6.ValueChanged += new System.EventHandler(this.numericUpDownValue6_ValueChanged);
			// 
			// numericUpDownValue5
			// 
			this.numericUpDownValue5.Location = new System.Drawing.Point(32, 180);
			this.numericUpDownValue5.Name = "numericUpDownValue5";
			this.numericUpDownValue5.Size = new System.Drawing.Size(56, 21);
			this.numericUpDownValue5.TabIndex = 25;
			this.numericUpDownValue5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownValue5.Value = new System.Decimal(new int[] {
																			  25,
																			  0,
																			  0,
																			  0});
			this.numericUpDownValue5.ValueChanged += new System.EventHandler(this.numericUpDownValue5_ValueChanged);
			// 
			// numericUpDownValue4
			// 
			this.numericUpDownValue4.Location = new System.Drawing.Point(32, 145);
			this.numericUpDownValue4.Name = "numericUpDownValue4";
			this.numericUpDownValue4.Size = new System.Drawing.Size(56, 21);
			this.numericUpDownValue4.TabIndex = 20;
			this.numericUpDownValue4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownValue4.Value = new System.Decimal(new int[] {
																			  60,
																			  0,
																			  0,
																			  0});
			this.numericUpDownValue4.ValueChanged += new System.EventHandler(this.numericUpDownValue4_ValueChanged);
			// 
			// numericUpDownValue3
			// 
			this.numericUpDownValue3.Location = new System.Drawing.Point(32, 110);
			this.numericUpDownValue3.Name = "numericUpDownValue3";
			this.numericUpDownValue3.Size = new System.Drawing.Size(56, 21);
			this.numericUpDownValue3.TabIndex = 15;
			this.numericUpDownValue3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownValue3.Value = new System.Decimal(new int[] {
																			  20,
																			  0,
																			  0,
																			  0});
			this.numericUpDownValue3.ValueChanged += new System.EventHandler(this.numericUpDownValue3_ValueChanged);
			// 
			// numericUpDownValue2
			// 
			this.numericUpDownValue2.Location = new System.Drawing.Point(32, 75);
			this.numericUpDownValue2.Name = "numericUpDownValue2";
			this.numericUpDownValue2.Size = new System.Drawing.Size(56, 21);
			this.numericUpDownValue2.TabIndex = 10;
			this.numericUpDownValue2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownValue2.Value = new System.Decimal(new int[] {
																			  15,
																			  0,
																			  0,
																			  0});
			this.numericUpDownValue2.ValueChanged += new System.EventHandler(this.numericUpDownValue2_ValueChanged);
			// 
			// numericUpDownValue1
			// 
			this.numericUpDownValue1.Location = new System.Drawing.Point(32, 40);
			this.numericUpDownValue1.Name = "numericUpDownValue1";
			this.numericUpDownValue1.Size = new System.Drawing.Size(56, 21);
			this.numericUpDownValue1.TabIndex = 5;
			this.numericUpDownValue1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownValue1.Value = new System.Decimal(new int[] {
																			  10,
																			  0,
																			  0,
																			  0});
			this.numericUpDownValue1.ValueChanged += new System.EventHandler(this.numericUpDownValue1_ValueChanged);
			// 
			// numericUpDownDisplacement1
			// 
			this.numericUpDownDisplacement1.DecimalPlaces = 2;
			this.numericUpDownDisplacement1.Increment = new System.Decimal(new int[] {
																						 5,
																						 0,
																						 0,
																						 131072});
			this.numericUpDownDisplacement1.Location = new System.Drawing.Point(96, 40);
			this.numericUpDownDisplacement1.Maximum = new System.Decimal(new int[] {
																					   1,
																					   0,
																					   0,
																					   0});
			this.numericUpDownDisplacement1.Name = "numericUpDownDisplacement1";
			this.numericUpDownDisplacement1.Size = new System.Drawing.Size(64, 21);
			this.numericUpDownDisplacement1.TabIndex = 6;
			this.numericUpDownDisplacement1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownDisplacement1.Value = new System.Decimal(new int[] {
																					 2,
																					 0,
																					 0,
																					 65536});
			this.numericUpDownDisplacement1.ValueChanged += new System.EventHandler(this.numericUpDownDisplacement1_ValueChanged);
			// 
			// numericUpDownDisplacement6
			// 
			this.numericUpDownDisplacement6.DecimalPlaces = 2;
			this.numericUpDownDisplacement6.Increment = new System.Decimal(new int[] {
																						 5,
																						 0,
																						 0,
																						 131072});
			this.numericUpDownDisplacement6.Location = new System.Drawing.Point(96, 215);
			this.numericUpDownDisplacement6.Maximum = new System.Decimal(new int[] {
																					   1,
																					   0,
																					   0,
																					   0});
			this.numericUpDownDisplacement6.Name = "numericUpDownDisplacement6";
			this.numericUpDownDisplacement6.Size = new System.Drawing.Size(64, 21);
			this.numericUpDownDisplacement6.TabIndex = 31;
			this.numericUpDownDisplacement6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownDisplacement6.Value = new System.Decimal(new int[] {
																					 5,
																					 0,
																					 0,
																					 131072});
			this.numericUpDownDisplacement6.ValueChanged += new System.EventHandler(this.numericUpDownDisplacement6_ValueChanged);
			// 
			// numericUpDownDisplacement5
			// 
			this.numericUpDownDisplacement5.DecimalPlaces = 2;
			this.numericUpDownDisplacement5.Increment = new System.Decimal(new int[] {
																						 5,
																						 0,
																						 0,
																						 131072});
			this.numericUpDownDisplacement5.Location = new System.Drawing.Point(96, 180);
			this.numericUpDownDisplacement5.Maximum = new System.Decimal(new int[] {
																					   1,
																					   0,
																					   0,
																					   0});
			this.numericUpDownDisplacement5.Name = "numericUpDownDisplacement5";
			this.numericUpDownDisplacement5.Size = new System.Drawing.Size(64, 21);
			this.numericUpDownDisplacement5.TabIndex = 26;
			this.numericUpDownDisplacement5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownDisplacement5.Value = new System.Decimal(new int[] {
																					 5,
																					 0,
																					 0,
																					 131072});
			this.numericUpDownDisplacement5.ValueChanged += new System.EventHandler(this.numericUpDownDisplacement5_ValueChanged);
			// 
			// numericUpDownDisplacement4
			// 
			this.numericUpDownDisplacement4.DecimalPlaces = 2;
			this.numericUpDownDisplacement4.Increment = new System.Decimal(new int[] {
																						 5,
																						 0,
																						 0,
																						 131072});
			this.numericUpDownDisplacement4.Location = new System.Drawing.Point(96, 145);
			this.numericUpDownDisplacement4.Maximum = new System.Decimal(new int[] {
																					   1,
																					   0,
																					   0,
																					   0});
			this.numericUpDownDisplacement4.Name = "numericUpDownDisplacement4";
			this.numericUpDownDisplacement4.Size = new System.Drawing.Size(64, 21);
			this.numericUpDownDisplacement4.TabIndex = 21;
			this.numericUpDownDisplacement4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownDisplacement4.Value = new System.Decimal(new int[] {
																					 5,
																					 0,
																					 0,
																					 131072});
			this.numericUpDownDisplacement4.ValueChanged += new System.EventHandler(this.numericUpDownDisplacement4_ValueChanged);
			// 
			// numericUpDownDisplacement3
			// 
			this.numericUpDownDisplacement3.DecimalPlaces = 2;
			this.numericUpDownDisplacement3.Increment = new System.Decimal(new int[] {
																						 5,
																						 0,
																						 0,
																						 131072});
			this.numericUpDownDisplacement3.Location = new System.Drawing.Point(96, 110);
			this.numericUpDownDisplacement3.Maximum = new System.Decimal(new int[] {
																					   1,
																					   0,
																					   0,
																					   0});
			this.numericUpDownDisplacement3.Name = "numericUpDownDisplacement3";
			this.numericUpDownDisplacement3.Size = new System.Drawing.Size(64, 21);
			this.numericUpDownDisplacement3.TabIndex = 16;
			this.numericUpDownDisplacement3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownDisplacement3.Value = new System.Decimal(new int[] {
																					 5,
																					 0,
																					 0,
																					 131072});
			this.numericUpDownDisplacement3.ValueChanged += new System.EventHandler(this.numericUpDownDisplacement3_ValueChanged);
			// 
			// numericUpDownDisplacement2
			// 
			this.numericUpDownDisplacement2.DecimalPlaces = 2;
			this.numericUpDownDisplacement2.Increment = new System.Decimal(new int[] {
																						 5,
																						 0,
																						 0,
																						 131072});
			this.numericUpDownDisplacement2.Location = new System.Drawing.Point(96, 75);
			this.numericUpDownDisplacement2.Maximum = new System.Decimal(new int[] {
																					   1,
																					   0,
																					   0,
																					   0});
			this.numericUpDownDisplacement2.Name = "numericUpDownDisplacement2";
			this.numericUpDownDisplacement2.Size = new System.Drawing.Size(64, 21);
			this.numericUpDownDisplacement2.TabIndex = 11;
			this.numericUpDownDisplacement2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownDisplacement2.Value = new System.Decimal(new int[] {
																					 5,
																					 0,
																					 0,
																					 131072});
			this.numericUpDownDisplacement2.ValueChanged += new System.EventHandler(this.numericUpDownDisplacement2_ValueChanged);
			// 
			// buttonColor6
			// 
			this.buttonColor6.BackColor = System.Drawing.Color.DeepSkyBlue;
			this.buttonColor6.Location = new System.Drawing.Point(180, 215);
			this.buttonColor6.Name = "buttonColor6";
			this.buttonColor6.Size = new System.Drawing.Size(20, 20);
			this.buttonColor6.TabIndex = 32;
			this.buttonColor6.Click += new System.EventHandler(this.buttonColor6_Click);
			// 
			// buttonColor5
			// 
			this.buttonColor5.BackColor = System.Drawing.Color.Firebrick;
			this.buttonColor5.Location = new System.Drawing.Point(180, 180);
			this.buttonColor5.Name = "buttonColor5";
			this.buttonColor5.Size = new System.Drawing.Size(20, 20);
			this.buttonColor5.TabIndex = 27;
			this.buttonColor5.Click += new System.EventHandler(this.buttonColor5_Click);
			// 
			// buttonColor4
			// 
			this.buttonColor4.BackColor = System.Drawing.Color.Yellow;
			this.buttonColor4.Location = new System.Drawing.Point(180, 145);
			this.buttonColor4.Name = "buttonColor4";
			this.buttonColor4.Size = new System.Drawing.Size(20, 20);
			this.buttonColor4.TabIndex = 22;
			this.buttonColor4.Click += new System.EventHandler(this.buttonColor4_Click);
			// 
			// buttonColor3
			// 
			this.buttonColor3.BackColor = System.Drawing.Color.Blue;
			this.buttonColor3.Location = new System.Drawing.Point(180, 110);
			this.buttonColor3.Name = "buttonColor3";
			this.buttonColor3.Size = new System.Drawing.Size(20, 20);
			this.buttonColor3.TabIndex = 17;
			this.buttonColor3.Click += new System.EventHandler(this.buttonColor3_Click);
			// 
			// buttonColor2
			// 
			this.buttonColor2.BackColor = System.Drawing.Color.LimeGreen;
			this.buttonColor2.Location = new System.Drawing.Point(180, 75);
			this.buttonColor2.Name = "buttonColor2";
			this.buttonColor2.Size = new System.Drawing.Size(20, 20);
			this.buttonColor2.TabIndex = 12;
			this.buttonColor2.Click += new System.EventHandler(this.buttonColor2_Click);
			// 
			// buttonColor1
			// 
			this.buttonColor1.BackColor = System.Drawing.Color.Red;
			this.buttonColor1.Location = new System.Drawing.Point(180, 40);
			this.buttonColor1.Name = "buttonColor1";
			this.buttonColor1.Size = new System.Drawing.Size(20, 20);
			this.buttonColor1.TabIndex = 7;
			this.buttonColor1.Click += new System.EventHandler(this.buttonColor1_Click);
			// 
			// groupBox6
			// 
			this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox6.Controls.Add(this.textBoxToolTip6);
			this.groupBox6.Controls.Add(this.textBoxToolTip5);
			this.groupBox6.Controls.Add(this.textBoxToolTip4);
			this.groupBox6.Controls.Add(this.textBoxToolTip3);
			this.groupBox6.Controls.Add(this.textBoxToolTip2);
			this.groupBox6.Controls.Add(this.textBoxToolTip1);
			this.groupBox6.Controls.Add(this.label12);
			this.groupBox6.Controls.Add(this.checkBox6);
			this.groupBox6.Controls.Add(this.checkBox5);
			this.groupBox6.Controls.Add(this.checkBox4);
			this.groupBox6.Controls.Add(this.checkBox3);
			this.groupBox6.Controls.Add(this.checkBox2);
			this.groupBox6.Controls.Add(this.checkBox1);
			this.groupBox6.Controls.Add(this.label10);
			this.groupBox6.Controls.Add(this.label9);
			this.groupBox6.Controls.Add(this.label8);
			this.groupBox6.Controls.Add(this.numericUpDownValue2);
			this.groupBox6.Controls.Add(this.numericUpDownValue3);
			this.groupBox6.Controls.Add(this.numericUpDownValue6);
			this.groupBox6.Controls.Add(this.numericUpDownValue1);
			this.groupBox6.Controls.Add(this.numericUpDownValue5);
			this.groupBox6.Controls.Add(this.numericUpDownValue4);
			this.groupBox6.Controls.Add(this.numericUpDownDisplacement1);
			this.groupBox6.Controls.Add(this.numericUpDownDisplacement6);
			this.groupBox6.Controls.Add(this.numericUpDownDisplacement3);
			this.groupBox6.Controls.Add(this.numericUpDownDisplacement4);
			this.groupBox6.Controls.Add(this.numericUpDownDisplacement5);
			this.groupBox6.Controls.Add(this.numericUpDownDisplacement2);
			this.groupBox6.Controls.Add(this.buttonColor3);
			this.groupBox6.Controls.Add(this.buttonColor6);
			this.groupBox6.Controls.Add(this.buttonColor5);
			this.groupBox6.Controls.Add(this.buttonColor2);
			this.groupBox6.Controls.Add(this.buttonColor4);
			this.groupBox6.Controls.Add(this.buttonColor1);
			this.groupBox6.Location = new System.Drawing.Point(64, 528);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(304, 248);
			this.groupBox6.TabIndex = 1;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "&Pie slices";
			this.groupBox6.Visible = false;
			// 
			// textBoxToolTip6
			// 
			this.textBoxToolTip6.Location = new System.Drawing.Point(216, 215);
			this.textBoxToolTip6.Name = "textBoxToolTip6";
			this.textBoxToolTip6.Size = new System.Drawing.Size(80, 21);
			this.textBoxToolTip6.TabIndex = 33;
			this.textBoxToolTip6.Text = "";
			this.textBoxToolTip6.TextChanged += new System.EventHandler(this.textBoxToolTip_TextChanged);
			// 
			// textBoxToolTip5
			// 
			this.textBoxToolTip5.Location = new System.Drawing.Point(216, 180);
			this.textBoxToolTip5.Name = "textBoxToolTip5";
			this.textBoxToolTip5.Size = new System.Drawing.Size(80, 21);
			this.textBoxToolTip5.TabIndex = 28;
			this.textBoxToolTip5.Text = "";
			this.textBoxToolTip5.TextChanged += new System.EventHandler(this.textBoxToolTip_TextChanged);
			// 
			// textBoxToolTip4
			// 
			this.textBoxToolTip4.Location = new System.Drawing.Point(216, 145);
			this.textBoxToolTip4.Name = "textBoxToolTip4";
			this.textBoxToolTip4.Size = new System.Drawing.Size(80, 21);
			this.textBoxToolTip4.TabIndex = 23;
			this.textBoxToolTip4.Text = "";
			this.textBoxToolTip4.TextChanged += new System.EventHandler(this.textBoxToolTip_TextChanged);
			// 
			// textBoxToolTip3
			// 
			this.textBoxToolTip3.Location = new System.Drawing.Point(216, 110);
			this.textBoxToolTip3.Name = "textBoxToolTip3";
			this.textBoxToolTip3.Size = new System.Drawing.Size(80, 21);
			this.textBoxToolTip3.TabIndex = 18;
			this.textBoxToolTip3.Text = "";
			this.textBoxToolTip3.TextChanged += new System.EventHandler(this.textBoxToolTip_TextChanged);
			// 
			// textBoxToolTip2
			// 
			this.textBoxToolTip2.Location = new System.Drawing.Point(216, 75);
			this.textBoxToolTip2.Name = "textBoxToolTip2";
			this.textBoxToolTip2.Size = new System.Drawing.Size(80, 21);
			this.textBoxToolTip2.TabIndex = 13;
			this.textBoxToolTip2.Text = "";
			this.textBoxToolTip2.TextChanged += new System.EventHandler(this.textBoxToolTip_TextChanged);
			// 
			// textBoxToolTip1
			// 
			this.textBoxToolTip1.Location = new System.Drawing.Point(216, 40);
			this.textBoxToolTip1.Name = "textBoxToolTip1";
			this.textBoxToolTip1.Size = new System.Drawing.Size(80, 21);
			this.textBoxToolTip1.TabIndex = 8;
			this.textBoxToolTip1.Text = "";
			this.textBoxToolTip1.TextChanged += new System.EventHandler(this.textBoxToolTip_TextChanged);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(216, 16);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(64, 16);
			this.label12.TabIndex = 3;
			this.label12.Text = "ToolTip";
			// 
			// checkBox6
			// 
			this.checkBox6.Checked = true;
			this.checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox6.Location = new System.Drawing.Point(8, 213);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(16, 24);
			this.checkBox6.TabIndex = 29;
			this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
			// 
			// checkBox5
			// 
			this.checkBox5.Checked = true;
			this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox5.Location = new System.Drawing.Point(8, 178);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(16, 24);
			this.checkBox5.TabIndex = 24;
			this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
			// 
			// checkBox4
			// 
			this.checkBox4.Checked = true;
			this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox4.Location = new System.Drawing.Point(8, 143);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(16, 24);
			this.checkBox4.TabIndex = 19;
			this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
			// 
			// checkBox3
			// 
			this.checkBox3.Checked = true;
			this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox3.Location = new System.Drawing.Point(8, 108);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(16, 24);
			this.checkBox3.TabIndex = 14;
			this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
			// 
			// checkBox2
			// 
			this.checkBox2.Checked = true;
			this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox2.Location = new System.Drawing.Point(8, 73);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(16, 24);
			this.checkBox2.TabIndex = 9;
			this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			// 
			// checkBox1
			// 
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(8, 38);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(16, 24);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(176, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(38, 16);
			this.label10.TabIndex = 2;
			this.label10.Text = "Color";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(96, 16);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(80, 16);
			this.label9.TabIndex = 1;
			this.label9.Text = "Displacement";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(32, 16);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(48, 16);
			this.label8.TabIndex = 0;
			this.label8.Text = "Value";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ColumnOption
			// 
			this.ColumnOption.Text = "Option";
			this.ColumnOption.Width = 153;
			// 
			// columnColor
			// 
			this.columnColor.Text = "Color";
			this.columnColor.Width = 41;
			// 
			// columnCount
			// 
			this.columnCount.Text = "Count";
			this.columnCount.Width = 47;
			// 
			// ColumnPercentage
			// 
			this.ColumnPercentage.Text = "%";
			this.ColumnPercentage.Width = 26;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.panel4);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox3.ForeColor = System.Drawing.Color.White;
			this.groupBox3.Location = new System.Drawing.Point(0, 0);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(656, 219);
			this.groupBox3.TabIndex = 15;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Answer Details";
			this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.listViewAttendeeAnswers);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(3, 17);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(650, 199);
			this.panel4.TabIndex = 1;
			// 
			// listViewAttendeeAnswers
			// 
			this.listViewAttendeeAnswers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																									  this.columnName,
																									  this.columnOptionSelected});
			this.listViewAttendeeAnswers.FullRowSelect = true;
			this.listViewAttendeeAnswers.Location = new System.Drawing.Point(3, 0);
			this.listViewAttendeeAnswers.Name = "listViewAttendeeAnswers";
			this.listViewAttendeeAnswers.Scrollable = false;
			this.listViewAttendeeAnswers.Size = new System.Drawing.Size(985, 232);
			this.listViewAttendeeAnswers.Sorting = System.Windows.Forms.SortOrder.Descending;
			this.listViewAttendeeAnswers.TabIndex = 0;
			this.listViewAttendeeAnswers.View = System.Windows.Forms.View.Details;
			this.listViewAttendeeAnswers.DoubleClick += new System.EventHandler(this.listViewAttendeeAnswers_DoubleClick);
			this.listViewAttendeeAnswers.SelectedIndexChanged += new System.EventHandler(this.listViewAttendeeAnswers_SelectedIndexChanged);
			// 
			// columnName
			// 
			this.columnName.Text = "Respondent";
			this.columnName.Width = 195;
			// 
			// columnOptionSelected
			// 
			this.columnOptionSelected.Text = "Answer";
			this.columnOptionSelected.Width = 462;
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.btnBarGraphEx);
			this.panel2.Controls.Add(this.btnPieChart);
			this.panel2.Controls.Add(this.btnBarGraph);
			this.panel2.Controls.Add(this.button1);
			this.panel2.Location = new System.Drawing.Point(96, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(552, 40);
			this.panel2.TabIndex = 19;
			this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(90, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(460, 19);
			this.label1.TabIndex = 6;
			this.label1.Text = "Note: In case of new Poll question, the current resulting window will disappear";
			// 
			// btnBarGraphEx
			// 
			this.btnBarGraphEx.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnBarGraphEx.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnBarGraphEx.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnBarGraphEx.HottrackImage")));
			this.btnBarGraphEx.Location = new System.Drawing.Point(214, 8);
			this.btnBarGraphEx.Name = "btnBarGraphEx";
			this.btnBarGraphEx.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnBarGraphEx.NormalImage")));
			this.btnBarGraphEx.OnlyShowBitmap = true;
			this.btnBarGraphEx.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnBarGraphEx.PressedImage")));
			this.btnBarGraphEx.Size = new System.Drawing.Size(64, 32);
			this.btnBarGraphEx.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnBarGraphEx.ToolTip = null;
			this.btnBarGraphEx.Visible = false;
			this.btnBarGraphEx.Click += new System.EventHandler(this.btnBarGraph_Click);
			// 
			// btnPieChart
			// 
			this.btnPieChart.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnPieChart.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnPieChart.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnPieChart.HottrackImage")));
			this.btnPieChart.Location = new System.Drawing.Point(140, 8);
			this.btnPieChart.Name = "btnPieChart";
			this.btnPieChart.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnPieChart.NormalImage")));
			this.btnPieChart.OnlyShowBitmap = true;
			this.btnPieChart.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnPieChart.PressedImage")));
			this.btnPieChart.Size = new System.Drawing.Size(64, 32);
			this.btnPieChart.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnPieChart.ToolTip = null;
			this.btnPieChart.Visible = false;
			this.btnPieChart.Click += new System.EventHandler(this.nsButton1_Click);
			// 
			// btnBarGraph
			// 
			this.btnBarGraph.Location = new System.Drawing.Point(284, 10);
			this.btnBarGraph.Name = "btnBarGraph";
			this.btnBarGraph.Size = new System.Drawing.Size(97, 23);
			this.btnBarGraph.TabIndex = 5;
			this.btnBarGraph.Text = "Show Bar Graph";
			this.btnBarGraph.Visible = false;
			this.btnBarGraph.Click += new System.EventHandler(this.btnBarGraph_Click);
			// 
			// button1
			// 
			this.button1.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.button1.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.button1.HottrackImage = ((System.Drawing.Image)(resources.GetObject("button1.HottrackImage")));
			this.button1.Location = new System.Drawing.Point(9, 6);
			this.button1.Name = "button1";
			this.button1.NormalImage = ((System.Drawing.Image)(resources.GetObject("button1.NormalImage")));
			this.button1.OnlyShowBitmap = true;
			this.button1.PressedImage = ((System.Drawing.Image)(resources.GetObject("button1.PressedImage")));
			this.button1.Size = new System.Drawing.Size(63, 28);
			this.button1.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.button1.ToolTip = null;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.button2.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.button2.HottrackImage = ((System.Drawing.Image)(resources.GetObject("button2.HottrackImage")));
			this.button2.Location = new System.Drawing.Point(88, 8);
			this.button2.Name = "button2";
			this.button2.NormalImage = ((System.Drawing.Image)(resources.GetObject("button2.NormalImage")));
			this.button2.OnlyShowBitmap = true;
			this.button2.PressedImage = ((System.Drawing.Image)(resources.GetObject("button2.PressedImage")));
			this.button2.Size = new System.Drawing.Size(63, 28);
			this.button2.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.button2.ToolTip = null;
			this.button2.Visible = false;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// btnOption
			// 
			this.btnOption.Location = new System.Drawing.Point(616, 10);
			this.btnOption.Name = "btnOption";
			this.btnOption.Size = new System.Drawing.Size(16, 23);
			this.btnOption.TabIndex = 2;
			this.btnOption.Text = "Options";
			this.btnOption.Visible = false;
			this.btnOption.Click += new System.EventHandler(this.btnOption_Click);
			// 
			// panelTop
			// 
			this.panelTop.Controls.Add(this.lblQuestion);
			this.panelTop.Controls.Add(this.m_panelDrawing);
			this.panelTop.Controls.Add(this.barGraphBox);
			this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTop.Location = new System.Drawing.Point(0, 0);
			this.panelTop.Name = "panelTop";
			this.panelTop.Size = new System.Drawing.Size(656, 264);
			this.panelTop.TabIndex = 20;
			this.panelTop.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTop_Paint);
			// 
			// lblQuestion
			// 
			this.lblQuestion.BackColor = System.Drawing.SystemColors.HotTrack;
			this.lblQuestion.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblQuestion.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblQuestion.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.lblQuestion.Location = new System.Drawing.Point(0, 0);
			this.lblQuestion.Name = "lblQuestion";
			this.lblQuestion.Size = new System.Drawing.Size(656, 40);
			this.lblQuestion.TabIndex = 1;
			this.lblQuestion.Text = "Q.";
			this.lblQuestion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.btnOption);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 483);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(656, 40);
			this.panel1.TabIndex = 21;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.panel5);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 264);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(656, 219);
			this.panel3.TabIndex = 22;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.groupBox3);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel5.Location = new System.Drawing.Point(0, 0);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(656, 219);
			this.panel5.TabIndex = 17;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Description";
			this.columnHeader2.Width = 251;
			// 
			// PollResult
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.Controls.Add(this.panel3);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panelTop);
			this.Controls.Add(this.groupBox6);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "PollResult";
			this.Size = new System.Drawing.Size(656, 523);
			this.Resize += new System.EventHandler(this.PollResult_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.PollResult_Paint);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownValue1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplacement1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplacement6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplacement5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplacement4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplacement3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplacement2)).EndInit();
			this.groupBox6.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panelTop.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel5.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		# region changeInterfacetoEvaluation()
		public void changeInterfacetoEvaluation()
		{
			button1.Visible = false;
			button2.Visible = false;

		}
		# endregion
		
		# region FillEdgeColorTypeListBox() 
		private void FillEdgeColorTypeListBox() 
		{
			string[] types = Enum.GetNames(typeof(EdgeColorType));
			
		}
		# endregion

		# region InitializeChart() 
		private void InitializeChart() 
		{
			SetValues();
			SetPieDisplacements();
			SetColors();
			SetToolTips();

			m_panelDrawing.LeftMargin        = (float)10.0;
			m_panelDrawing.RightMargin       = (float)10.0;
			m_panelDrawing.TopMargin         = (float)10.0;
			m_panelDrawing.BottomMargin      = (float)10.0;             
			m_panelDrawing.FitChart          = false;
			m_panelDrawing.SliceRelativeHeight = (float)0.20;
			m_panelDrawing.InitialAngle      = (float)0;
			m_panelDrawing.EdgeLineWidth     = (float)0.0;
			
		}

		# endregion

		# region SetColors() 
		private void SetColors() 
		{
			ArrayList colors = new ArrayList();
			if (buttonColor1.Enabled)
				colors.Add(buttonColor1.BackColor);
			if (buttonColor2.Enabled)
				colors.Add(buttonColor2.BackColor);
			if (buttonColor3.Enabled)
				colors.Add(buttonColor3.BackColor);
			if (buttonColor4.Enabled)
				colors.Add(buttonColor4.BackColor);
			if (buttonColor5.Enabled)
				colors.Add(buttonColor5.BackColor);
			if (buttonColor6.Enabled)
				colors.Add(buttonColor6.BackColor);
			m_panelDrawing.Colors = (Color[])colors.ToArray(typeof(Color));
		}
		# endregion

		# region  SetValues(ArrayList values)
		public void SetValues(ArrayList values)
		{
			
			m_panelDrawing.Values = (decimal[])values.ToArray(typeof(decimal));
			
			

		}
		# endregion 
		
		# region SetValues() 
		private void SetValues() 
		{
			ArrayList values = new ArrayList();
			if (numericUpDownValue1.Enabled)
				values.Add(numericUpDownValue1.Value);
			if (numericUpDownValue2.Enabled)
				values.Add(numericUpDownValue2.Value);
			if (numericUpDownValue3.Enabled)
				values.Add(numericUpDownValue3.Value);
			if (numericUpDownValue4.Enabled)
				values.Add(numericUpDownValue4.Value);
			if (numericUpDownValue5.Enabled)
				values.Add(numericUpDownValue5.Value);
			if (numericUpDownValue6.Enabled)
				values.Add(numericUpDownValue6.Value);
			m_panelDrawing.Values = (decimal[])values.ToArray(typeof(decimal));
		}

		# endregion 

		# region SetDisplacements(ArrayList displacements)

		public void SetDisplacements(ArrayList displacements)
		{
			m_panelDrawing.SliceRelativeDisplacements = (float[])displacements.ToArray(typeof(float));
		}
		# endregion 


		# region SetPieDisplacements() 
		private void SetPieDisplacements() 
		{
			
			ArrayList displacements = new ArrayList();
			if (numericUpDownDisplacement1.Enabled)
				displacements.Add((float)numericUpDownDisplacement1.Value);
			if (numericUpDownDisplacement2.Enabled)
				displacements.Add((float)numericUpDownDisplacement2.Value);
			if (numericUpDownDisplacement3.Enabled)
				displacements.Add((float)numericUpDownDisplacement3.Value);
			if (numericUpDownDisplacement4.Enabled)
				displacements.Add((float)numericUpDownDisplacement4.Value);
			if (numericUpDownDisplacement5.Enabled)
				displacements.Add((float)numericUpDownDisplacement5.Value);
			if (numericUpDownDisplacement6.Enabled)
				displacements.Add((float)numericUpDownDisplacement6.Value);
			m_panelDrawing.SliceRelativeDisplacements = (float[])displacements.ToArray(typeof(float));
		}

		# endregion

		# region buttonColor1_Click(object sender, System.EventArgs e) 
		private void buttonColor1_Click(object sender, System.EventArgs e) 
		{
			if (m_colorDialog.ShowDialog() == DialogResult.OK) 
			{
				buttonColor1.BackColor = m_colorDialog.Color;
				SetColors();
			}
		}

		# endregion

		# region buttonColor2_Click(object sender, System.EventArgs e) 
		private void buttonColor2_Click(object sender, System.EventArgs e) 
		{
			if (m_colorDialog.ShowDialog() == DialogResult.OK) 
			{
				buttonColor2.BackColor = m_colorDialog.Color;
				SetColors();
			}
		}

		# endregion

		# region EnableShareMode()
		public void EnableShareMode()
		{
			button1.Visible = false;
			button2.Visible = false;
		


		}
		# endregion 

		# region buttonColor3_Click(object sender, System.EventArgs e) 
		private void buttonColor3_Click(object sender, System.EventArgs e) 
		{
			if (m_colorDialog.ShowDialog() == DialogResult.OK) 
			{
				buttonColor3.BackColor = m_colorDialog.Color;
				SetColors();
			}
		}

		# endregion 


		# region buttonColor4_Click(object sender, System.EventArgs e) 
		private void buttonColor4_Click(object sender, System.EventArgs e) 
		{
			if (m_colorDialog.ShowDialog() == DialogResult.OK) 
			{
				buttonColor4.BackColor = m_colorDialog.Color;
				SetColors();
			}
		}

		# endregion 

		# region buttonColor5_Click(object sender, System.EventArgs e) 
		private void buttonColor5_Click(object sender, System.EventArgs e) 
		{
			if (m_colorDialog.ShowDialog() == DialogResult.OK) 
			{
				buttonColor5.BackColor = m_colorDialog.Color;
				SetColors();
			}
		}

		# endregion


		# region buttonColor6_Click(object sender, System.EventArgs e) 
		private void buttonColor6_Click(object sender, System.EventArgs e) 
		{
			if (m_colorDialog.ShowDialog() == DialogResult.OK) 
			{
				buttonColor6.BackColor = m_colorDialog.Color;
				SetColors();
			}
		}
		# endregion

		# region numericUpDownValue1_ValueChanged(object sender, System.EventArgs e) 
		private void numericUpDownValue1_ValueChanged(object sender, System.EventArgs e) 
		{
			SetValues();
		}
		# endregion

		# region numericUpDownValue2_ValueChanged(object sender, System.EventArgs e) 
		private void numericUpDownValue2_ValueChanged(object sender, System.EventArgs e) 
		{
			SetValues();
		}
		# endregion


		# region numericUpDownValue3_ValueChanged(object sender, System.EventArgs e) 
		private void numericUpDownValue3_ValueChanged(object sender, System.EventArgs e) 
		{
			SetValues();
		}
		# endregion

		private void numericUpDownValue4_ValueChanged(object sender, System.EventArgs e) 
		{
			SetValues();
		}

		private void numericUpDownValue5_ValueChanged(object sender, System.EventArgs e) 
		{
			SetValues();
		}

		private void numericUpDownValue6_ValueChanged(object sender, System.EventArgs e) 
		{
			SetValues();
		}

		private void numericUpDownDisplacement1_ValueChanged(object sender, System.EventArgs e) 
		{
			SetPieDisplacements();
		}

		private void numericUpDownDisplacement2_ValueChanged(object sender, System.EventArgs e) 
		{
			SetPieDisplacements();
		}

		private void numericUpDownDisplacement3_ValueChanged(object sender, System.EventArgs e) 
		{
			SetPieDisplacements();
		}

		private void numericUpDownDisplacement4_ValueChanged(object sender, System.EventArgs e) 
		{
			SetPieDisplacements();
		}

		private void numericUpDownDisplacement5_ValueChanged(object sender, System.EventArgs e) 
		{
			SetPieDisplacements();
		}

		private void numericUpDownDisplacement6_ValueChanged(object sender, System.EventArgs e) 
		{
			SetPieDisplacements();
		}

			
		private void checkBox1_CheckedChanged(object sender, System.EventArgs e) 
		{
			numericUpDownValue1.Enabled = checkBox1.Checked;
			numericUpDownDisplacement1.Enabled = checkBox1.Checked;
			buttonColor1.Enabled = checkBox1.Checked;
			textBoxToolTip1.Enabled = checkBox1.Checked;
			UpdateChart();
		}

		private void checkBox2_CheckedChanged(object sender, System.EventArgs e) 
		{
			numericUpDownValue2.Enabled = checkBox2.Checked;
			numericUpDownDisplacement2.Enabled = checkBox2.Checked;
			buttonColor2.Enabled = checkBox2.Checked;
			textBoxToolTip2.Enabled = checkBox2.Checked;
			UpdateChart();
		}

		private void checkBox3_CheckedChanged(object sender, System.EventArgs e) 
		{
			numericUpDownValue3.Enabled = checkBox3.Checked;
			numericUpDownDisplacement3.Enabled = checkBox3.Checked;
			buttonColor3.Enabled = checkBox3.Checked;
			textBoxToolTip3.Enabled = checkBox3.Checked;
			UpdateChart();
		}

		private void checkBox4_CheckedChanged(object sender, System.EventArgs e) 
		{
			numericUpDownValue4.Enabled = checkBox4.Checked;
			numericUpDownDisplacement4.Enabled = checkBox4.Checked;
			buttonColor4.Enabled = checkBox4.Checked;
			textBoxToolTip4.Enabled = checkBox4.Checked;
			UpdateChart();
		}

		private void checkBox5_CheckedChanged(object sender, System.EventArgs e) 
		{
			numericUpDownValue5.Enabled = checkBox5.Checked;
			numericUpDownDisplacement5.Enabled = checkBox5.Checked;
			buttonColor5.Enabled = checkBox5.Checked;
			textBoxToolTip5.Enabled = checkBox5.Checked;
			UpdateChart();
		}

		private void checkBox6_CheckedChanged(object sender, System.EventArgs e) 
		{
			numericUpDownValue6.Enabled = checkBox6.Checked;
			numericUpDownDisplacement6.Enabled = checkBox6.Checked;
			buttonColor6.Enabled = checkBox6.Checked;
			textBoxToolTip6.Enabled = checkBox6.Checked;
			UpdateChart();
		}

		private void UpdateChart() 
		{
			SetValues();
			SetPieDisplacements();
			SetColors();
			SetToolTips();
		}

		
		private void textBoxToolTip_TextChanged(object sender, System.EventArgs e) 
		{
			SetToolTips();
		}

		public void SetToolTips(ArrayList toolTips)
		{
			m_panelDrawing.ToolTips = (string[])toolTips.ToArray(typeof(string));
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			//m_panelDrawing
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			
			
		}

		private void SetToolTips() 
		{
			ArrayList toolTips = new ArrayList();
			if (textBoxToolTip1.Enabled)
				toolTips.Add(textBoxToolTip1.Text);
			if (textBoxToolTip2.Enabled)
				toolTips.Add(textBoxToolTip2.Text);
			if (textBoxToolTip3.Enabled)
				toolTips.Add(textBoxToolTip3.Text);
			if (textBoxToolTip4.Enabled)
				toolTips.Add(textBoxToolTip4.Text);
			if (textBoxToolTip5.Enabled)
				toolTips.Add(textBoxToolTip5.Text);
			if (textBoxToolTip6.Enabled)
				toolTips.Add(textBoxToolTip6.Text);
			m_panelDrawing.ToolTips = (string[])toolTips.ToArray(typeof(string));
		}


		# region SetAttendeeAnswers(ArrayList answers,PollType type)
		public void SetAttendeeAnswers(ArrayList answers,PollType type)
		{
			try
			{
				//Thread.Sleep(10);
				lblQuestion.Text = polling.question;

				listViewAttendeeAnswers.Clear();
				ArrayList colors = new ArrayList();
				if (buttonColor1.Enabled)
					colors.Add(buttonColor1.BackColor);
				if (buttonColor2.Enabled)
					colors.Add(buttonColor2.BackColor);
				if (buttonColor3.Enabled)
					colors.Add(buttonColor3.BackColor);
				if (buttonColor4.Enabled)
					colors.Add(buttonColor4.BackColor);
				if (buttonColor5.Enabled)
					colors.Add(buttonColor5.BackColor);
				if (buttonColor6.Enabled)
					colors.Add(buttonColor6.BackColor);
				//this.listViewAttendeeAnswers.Dock = System.Windows.Forms.DockStyle.Left;
				this.listViewAttendeeAnswers.Columns.Add(this.columnName);
				this.listViewAttendeeAnswers.Columns.Add(this.columnOptionSelected);

			
				this.listViewAttendeeAnswers.Name = "listViewAttendeeAnswers";
			
				//this.listViewAttendeeAnswers.Width=this.panel4.Width-2;
				//this.listViewAttendeeAnswers.Size = new System.Drawing.Size(272, 96);
				this.listViewAttendeeAnswers.Sorting = System.Windows.Forms.SortOrder.Descending;
				this.listViewAttendeeAnswers.TabIndex = 0;
				this.listViewAttendeeAnswers.View = System.Windows.Forms.View.Details;
				//this.listViewAttendeeAnswers.Location = new System.Drawing.Point(3, 0);
				if(type != PollType.FreeResponse)
				{
					//	Trace.WriteLine(barGraphBox.Top.ToString());
					this.listViewAttendeeAnswers.Location = new System.Drawing.Point(3, 0);
					//listViewAttendeeAnswers.Top = barGraphBox.Top;
				
				}
				else
				{
					this.listViewAttendeeAnswers.Location = new System.Drawing.Point(0,0);
					//this.listViewAttendeeAnswers.Top=0;
				}

			
				if(Client.ClientUI.RunOnceListAnswer)
				{
				
					if(type != PollType.FreeResponse)
					{
						//Trace.WriteLine("TYPe is not poll of Free Response...");
						this.listViewAttendeeAnswers.Size = new System.Drawing.Size(985, 221);
					}
					else
						this.listViewAttendeeAnswers.Size = new System.Drawing.Size(985,2000);
					Client.ClientUI.RunOnceListAnswer=false;
				}
				else
					this.listViewAttendeeAnswers.Dock=System.Windows.Forms.DockStyle.Fill;
			
				this.columnName.Text = "Respondent";
				this.columnName.Width = 195;
			 
				this.columnOptionSelected.Text = "Answer";
				this.columnOptionSelected.Width = 462;
				this.listViewAttendeeAnswers.Scrollable=false;
			
				if(type == PollType.FreeResponse)
				{
					this.barGraphBox.Visible = false;
					this.panelTop.Size= new System.Drawing.Size(656, 35);
					//this.m_panelDrawing.Visible = false;				
					//panelTop.Visible = false;
					//btnBarGraph.Visible = false;
					//btnOption.Visible = false;
					if(Client.ClientUI.RunOnceListAnswer==false)
						listViewAttendeeAnswers.Top=0;		
						//listViewAttendeeAnswers.Top = barGraphBox.Top;
					else
						listViewAttendeeAnswers.Top = barGraphBox.Top;
					//listViewAttendeeAnswers.Top=0;		
				}
				if(!polling.anonymous)
				{
					for(int i = 0 ; i < answers.Count ; i ++)
					{
						string strChoice = ((PollingAnswer)answers[i]).choice;
						if(strChoice == null)
							continue;
						if((type != PollType.MultipleSelect )  && (type != PollType.FreeResponse))
						{
							ListViewItem lv = listViewAttendeeAnswers.Items.Add(((PollingAnswer)answers[i]).clientName);						
							lv.SubItems.Add(strChoice);		
						}
						else if (type == PollType.MultipleSelect)
						{
							if(strChoice.Length > 0)
							{
								string []strChoicies = strChoice.Split('^');
								ListViewItem lv2 = listViewAttendeeAnswers.Items.Add(((PollingAnswer)answers[i]).clientName);
								string _answer="";
							
								foreach (string choice  in strChoicies)
								{
									_answer += choice + ",";
								}
								_answer = _answer.Substring(0,_answer.Length-2);
								lv2.SubItems.Add(_answer);
							}
						}
						else 
						{
							try
							{
								if(strChoice.Length > 0)
								{
									ListViewItem lv2 = listViewAttendeeAnswers.Items.Add(((PollingAnswer)answers[i]).clientName);							
									lv2.Tag = strChoice;
									strChoice.Replace("\r"," ");
									strChoice.Replace("\n"," ");
									lv2.SubItems.Add(strChoice);

								}
							}
							catch (Exception exp)
							{
								WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollResult.cs @SetAttendeeAnswers(ArrayList answers,PollType type) line==> 1527",exp,null,false);
							}
		
						}


					
					}									
				}
				else if(type == PollType.FreeResponse)
				{

					for(int i = 0 ; i < answers.Count ; i ++)
					{
						string strChoice = ((PollingAnswer)answers[i]).choice;
						if(strChoice == null)
							continue;
						ListViewItem lv2 = listViewAttendeeAnswers.Items.Add("Anonymous");							
						lv2.Tag = strChoice;
						lv2.SubItems.Add(strChoice);				
					
					}
				}
				else
				{
				
				
					for(int i = 0 ; i < answers.Count ; i ++)
					{
						string strChoice = ((PollingAnswer)answers[i]).choice;
						if(strChoice == null)
							continue;
						ListViewItem lv2 = listViewAttendeeAnswers.Items.Add("Anonymous");							
						lv2.Tag = strChoice;
						strChoice=strChoice.Replace("^",",");
						lv2.SubItems.Add(strChoice);				
					
					}
				
				}
					//listViewAttendeeAnswers.Enabled = false;

		}
		catch (Exception exp)
		{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollResult.cs @SetAttendeeAnswers(ArrayList answers,PollType type) line==> 1556",exp,null,false);
		}
		
		}
		
		# endregion 

		public void DisableCharting()
		{
			try
			{
				//m_panelDrawing.Enabled = false;			
				this.m_panelDrawing.Visible=false;
				//this.barGraphBox.Visible = true;
				this.barGraphBox.BackColor = Color.White;
				//btnBarGraph.Enabled = false;
				btnOption.Enabled = false;					
				panel2.Dock=DockStyle.Bottom;
				this.groupBox3.Dock=System.Windows.Forms.DockStyle.Fill;
				this.groupBox3.Controls[0].Dock=DockStyle.Fill;
				//panelTop.Visible = false;
				btnOption.Visible = false;
				btnBarGraph.Visible = false;
				barGraphBox.Visible = true;
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollResult.cs @DisableCharting() line==> 1583",exp,null,false);
			}


		}

		# region DrawChart(ref Graphics objGraphics)
		void DrawChart(ref Graphics objGraphics)
		{
		
			try
			{
				SolidBrush objBrush;
			
				int top=20;
				System.Drawing.Font f = new Font("Arial",8);						
				for(int i=0; i<BarChartEntries.Count;i++)
				{				
					BarChartEntry bEntry = (BarChartEntry)BarChartEntries[i];
					objBrush=new SolidBrush(bEntry.DrawColor);
					objGraphics.FillRectangle(objBrush,15,top,20,20);
					objGraphics.DrawString(bEntry.StrText,
						f,System.Drawing.Brushes.Red,50,top+5);
					//					f,System.Drawing.Brushes.Red,iBar,points[0].Y - 25);
					//				objGraphics.DrawString(bEntry.StrText + " " + bEntry.Percentage.ToString() +"%",
					//					f,System.Drawing.Brushes.Red,iBar,points[0].Y - 25);
					top=top+20+15;
				}
				int iMax = 0;
						
				//			Random rand = new Random();
					
				// Basic rhombus image
				Point point1 = new Point(100, 250);
				Point point2 = new Point(120, 250);
				Point point3 = new Point(125, 260);
				Point point4 = new Point(105, 260);
				Point[] points = {point1, point2, point3, point4};
			
				objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
			
				int width = 40;
				// iBar represents pixels along the y axis, draws 10 bars 
				int nStart = barGraphBox.Width/2;
				nStart = nStart - ((BarChartEntries.Count/2) * width);
				if(nStart < 0)
					nStart = 5;
				
				nStart=120;
				int Y_percentage=0;
				
				for(int i = 0,iBar = nStart; i < BarChartEntries.Count; i++,iBar += width)
				{
				
					// Set up for new bar
					
					points[0].X = iBar;
					points[1].X = iBar + 20;
					points[2].X = iBar + 25;
					points[3].X = iBar + 5;
				
					
					
					//Y_percentage=points[0].Y ;
					Y_percentage=140;

					points[0].Y = barGraphBox.Height - 40;
					points[1].Y = barGraphBox.Height - 40;
					points[2].Y = barGraphBox.Height - 30;
					points[3].Y = barGraphBox.Height - 30;
					
					
					
								
					BarChartEntry bEntry = (BarChartEntry)BarChartEntries[i];
					objBrush = new SolidBrush(bEntry.DrawColor);
					iMax = (int)bEntry.Percentage + 5;	// add 5 so the 0 percentage can be drawn
					for(int iCount = 0; iCount < iMax; iCount++)
					{				
						if(iCount == iMax - 1)
							objGraphics.FillPolygon(objBrush, points);
						else
							objGraphics.FillPolygon(new HatchBrush(HatchStyle.Percent50, objBrush.Color), points);
				
									
						// Set up for next rhomnus shape
						for(int iLoop = 0; iLoop < points.Length; iLoop++)
							points[iLoop].Y -= 1;
					}
				 
					
						objGraphics.DrawString(bEntry.Percentage.ToString() +"%",
							f,System.Drawing.Brushes.Red,iBar,Y_percentage+45);
					
					
				}
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollResult.cs @DrawChart() line==> 1669",exp,null,false);
			}
		
			
		}

		# endregion

		
		# region SetSummary(ArrayList values, ArrayList textList)
		public void SetSummary(ArrayList values, ArrayList textList)
		{
			try
			{
			ArrayList colors = new ArrayList();
			if (buttonColor1.Enabled)
				colors.Add(buttonColor1.BackColor);
			if (buttonColor2.Enabled)
				colors.Add(buttonColor2.BackColor);
			if (buttonColor3.Enabled)
				colors.Add(buttonColor3.BackColor);
			if (buttonColor4.Enabled)
				colors.Add(buttonColor4.BackColor);
			if (buttonColor5.Enabled)
				colors.Add(buttonColor5.BackColor);
			if (buttonColor6.Enabled)
				colors.Add(buttonColor6.BackColor);			
	
		
			int totalcount=0;
			for(int i=0;i<values.Count;i++)
			{
				totalcount+=(int)(decimal)values[i];
			}
			
			BarChartEntries.Clear();// = new ArrayList();            	
			for(int i=0;i<values.Count;i++)
			{
				BarChartEntry entry = new BarChartEntry();                
				if(totalcount!=0)
				{
					entry.StrText = (string)textList[i];
					entry.DrawColor = (Color)colors[i];
					//if(i <= (values.Count-1))
                        entry.Percentage = Decimal.Round(((decimal) values[i] * 100 / totalcount),2);                    
					//else
					//	entry.Percentage = 0;

				}
				else
				{
					entry.StrText = (string)textList[i];
					entry.DrawColor = (Color)colors[i];
					entry.Percentage = 0;                   		
     
				}
											
				BarChartEntries.Add(entry);
			}
			
			this.Refresh();
			
			/*
			//temporary coding
			for(int i=0;i<5;i++)
			{
				BarChartEntry entry = new BarChartEntry();                
				//if(totalcount!=0)
				{
					entry.StrText = (string)"Asda";
					entry.DrawColor = System.Drawing.Color.Red;
					entry.Percentage = ((decimal) 20);                    
				}
			/*	else
				{
					entry.StrText = (string)textList[i];
					entry.DrawColor = (Color)colors[i];
					entry.Percentage = 0;                    		     
				}*/
				/*BarChartEntries.Add(entry);
			}
			*/
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollResult.cs @SetSummary( line==> 1754",exp,null,false);
			}
		
				
		}
		# endregion

		public void SetAnswerList(ArrayList answerList)
		{
		}

			

		private void m_panelDrawing_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			/*
			
			if(mouseDown)
			{
				if(MouseX > e.X)
				{
					Decimal val = numericUpDownAngle.Value - 10;
					if((val > numericUpDownAngle.Minimum) && ( val < numericUpDownAngle.Maximum ))
					{
						numericUpDownAngle.Value = val;
					}
				}
				MouseX = e.X;				
			}		
			*/
		}
		

		# region SendCloseMessage()
		public void SendCloseMessage()
		{

			try
			{
				WebMeeting.Common.PollResultsMessage pMsg = new WebMeeting.Common.PollResultsMessage();		
				pMsg.answersList = 	polling.AnswersList;
				pMsg.choices= polling.choices;
				pMsg.strQuestion = polling.question;
				pMsg.sessionID = polling.sessionID;
				pMsg.anonymous = polling.anonymous;
				pMsg.type= polling.thisPollType;
				pMsg.isShow=false;
			

				pMsg.senderProfile = WebMeeting.Client.NetworkManager.getInstance().profile;
				pMsg.SenderID = WebMeeting.Client.NetworkManager.thisInstance.profile.ClientId;
				WebMeeting.Client.NetworkManager.getInstance().SendLoadPacket(pMsg);
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollResult.cs @SendCloseMessage() line==> 1809",exp,null,false);
			}
		}
		# endregion 

		# region button1_Click(object sender, System.EventArgs e)
		private void button1_Click(object sender, System.EventArgs e)
		{

			SharedPoolResult=true;
			
			WebMeeting.Common.PollResultsMessage pMsg = new WebMeeting.Common.PollResultsMessage();		
			pMsg.answersList = 	polling.AnswersList;
			pMsg.choices= polling.choices;
			pMsg.strQuestion = polling.question;
			pMsg.sessionID = polling.sessionID;
			pMsg.anonymous = polling.anonymous;
			pMsg.type= polling.thisPollType;
			pMsg.isShow=true;
			

			pMsg.senderProfile = WebMeeting.Client.NetworkManager.getInstance().profile;
			pMsg.SenderID = WebMeeting.Client.NetworkManager.thisInstance.profile.ClientId;
			WebMeeting.Client.NetworkManager.getInstance().SendLoadPacket(pMsg);


	
		
		}

		# endregion

		# region closeit()
		public void closeit()
		{
		
			PollStatusUpdateMessage msg = new PollStatusUpdateMessage();
			msg.enabled = false;
			msg.locked = true;
			msg.sessionID = polling.sessionID;
			WebMeeting.Client.NetworkManager.getInstance().SendLoadPacket(msg);
			//Client.PollingTabPage tabPage = 
			if(Client.ClientUI.getInstance().pollingTabControl.TabPages.Count>1)
			{
				Client.ClientUI.getInstance().pollingTabControl.TabPages.Remove((Client.PollingTabPage)Client.ClientUI.getInstance().pollingTabControl.SelectedTab); 
				//Client.ClientUI.getInstance().tabBody.TabPages.Remove(Client.ClientUI.getInstance().tabBody.SelectedTab);
				//Client.ClientUI.getInstance().tabPollingGlobal.Control.Controls 
				
				//Trace.WriteLine("if");
			}
			else
			{
				//Trace.WriteLine("else");
				
				//Client.ClientUI.getInstance().pollingTabControl.TabPages.RemoveAt(1);
				
				Client.ClientUI.getInstance().pollingTabControl.SelectedTab.Visible=false;
				Client.ClientUI.getInstance().pollingTabControl.TabPages.Remove((Client.PollingTabPage)Client.ClientUI.getInstance().pollingTabControl.SelectedTab); 

				
				Crownwood.Magic.Controls.TabPage ptab=Client.ClientUI.getInstance().tabBody.SelectedTab;
				//Trace.WriteLine("no of control in poolingtabwindow" + Client.ClientUI.getInstance().pollingTabControl.TabPages.Count.ToString());
				//Trace.WriteLine("selected tab page control" + ptab.CanSelect.ToString());
				Client.ClientUI.getInstance().tabBody.SelectedTab.Visible=false;
				//Client.ClientUI.getInstance().tabPollingGlobal.Visible=false;	
				
				Client.ClientUI.getInstance().tabBody.TabPages.Remove(Client.ClientUI.getInstance().tabBody.SelectedTab);
				Invoke(Client.ClientUI.getInstance().DeleteDocumentSharingWindow,new object[]{Client.ClientUI.getInstance().tabPollingGlobal});										///Invoke(DeleteDocumentSharingWindow,new object[]{tabPollingGlobal});	
				//Trace.WriteLine("TabPool Window " + Client.ClientUI.getInstance().pollingTabControl.CanSelect.ToString());
				
				//				Trace.WriteLine("Global selecte " + Client.ClientUI.getInstance().tabPollingGlobal.CanSelect.ToString());


			}

		}

		# endregion

		# region button2_Click(object sender, System.EventArgs e)
		private void button2_Click(object sender, System.EventArgs e)
		{
			
			try
			{

				//Client.PollingTabPage 
				//Crownwood.Magic.Controls.TabPage tpage=Client.ClientUI.getInstance().tabBody.SelectedTab;
			
				//Trace.WriteLine("Tab Page is selected : " + tabPage.Title);
				//Crownwood.Magic.Controls.TabPage tpage=Client.ClientUI.getInstance().tabBody.SelectedTab;
				//Trace.WriteLine("Closing Page Title" + Client.ClientUI.getInstance().tabBody.SelectedTab.Title);
				PollStatusUpdateMessage msg = new PollStatusUpdateMessage();
				msg.enabled = false;
				msg.locked = true;
				msg.sessionID = polling.sessionID;
				WebMeeting.Client.NetworkManager.getInstance().SendLoadPacket(msg);
				//Client.PollingTabPage tabPage = 
				if(Client.ClientUI.getInstance().pollingTabControl.TabPages.Count>1)
				{
					Client.PollingTabPage tabPage =(Client.PollingTabPage)Client.ClientUI.getInstance().pollingTabControl.SelectedTab;
					if(tabPage.pollResult.SharedPoolResult==true)
						tabPage.pollResult.SendCloseMessage();
					Client.ClientUI.getInstance().pollingTabControl.TabPages.Remove((Client.PollingTabPage)Client.ClientUI.getInstance().pollingTabControl.SelectedTab); 
					//Client.ClientUI.getInstance().tabBody.TabPages.Remove(Client.ClientUI.getInstance().tabBody.SelectedTab);
					//Client.ClientUI.getInstance().tabPollingGlobal.Control.Controls 
				
					//Trace.WriteLine("if");
				}
				else
				{
					//Trace.WriteLine("else");
				
					//Client.ClientUI.getInstance().pollingTabControl.TabPages.RemoveAt(1);
					Client.PollingTabPage tabPage =(Client.PollingTabPage)Client.ClientUI.getInstance().pollingTabControl.SelectedTab;
					if(tabPage.pollResult.SharedPoolResult==true)
						tabPage.pollResult.SendCloseMessage();

					Client.ClientUI.getInstance().pollingTabControl.SelectedTab.Visible=false;
					Client.ClientUI.getInstance().pollingTabControl.TabPages.Remove((Client.PollingTabPage)Client.ClientUI.getInstance().pollingTabControl.SelectedTab); 

				
					Crownwood.Magic.Controls.TabPage ptab=Client.ClientUI.getInstance().tabBody.SelectedTab;
					//Trace.WriteLine("no of control in poolingtabwindow" + Client.ClientUI.getInstance().pollingTabControl.TabPages.Count.ToString());
					//Trace.WriteLine("selected tab page control" + ptab.CanSelect.ToString());
					Client.ClientUI.getInstance().tabBody.SelectedTab.Visible=false;
					//Client.ClientUI.getInstance().tabPollingGlobal.Visible=false;	
				
					Client.ClientUI.getInstance().tabBody.TabPages.Remove(Client.ClientUI.getInstance().tabBody.SelectedTab);
					Invoke(Client.ClientUI.getInstance().DeleteDocumentSharingWindow,new object[]{Client.ClientUI.getInstance().tabPollingGlobal});										///Invoke(DeleteDocumentSharingWindow,new object[]{tabPollingGlobal});	
					//Trace.WriteLine("TabPool Window " + Client.ClientUI.getInstance().pollingTabControl.CanSelect.ToString());
				
					//				Trace.WriteLine("Global selecte " + Client.ClientUI.getInstance().tabPollingGlobal.CanSelect.ToString());


				}

			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling\\Pollresults.cs line==> 1949",exp,null,false);			
			}


		}

		# endregion

//		private void DockChanges(object sender, System.EventArgs e)
//		{			
//			//this.listViewAttendeeAnswers.Width=this.panel4.Width-100;
//			Trace.WriteLine("Pan4size :" + this.listViewAttendeeAnswers.Size.ToString() + " listansview : " +this.listViewAttendeeAnswers.Width);
//			Trace.WriteLine("listansview Size:" + this.listViewAttendeeAnswers.Size.ToString() + " listansview : " +this.listViewAttendeeAnswers.Width);
//			Trace.WriteLine("panel 4 size:" + this.panel4.Size.ToString() + " panel4 size" + this.panel4.Location.ToString());
//		}


		private void listViewAttendeeAnswers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
		}

	
		
		
		
		# region listViewAttendeeAnswers_DoubleClick(object sender, System.EventArgs e)
		private void listViewAttendeeAnswers_DoubleClick(object sender, System.EventArgs e)
		{
			if(listViewAttendeeAnswers.SelectedItems.Count < 1)
				return;
            
			try
			{
				string strName = listViewAttendeeAnswers.SelectedItems[0].Text;
				string strText = (string)listViewAttendeeAnswers.SelectedItems[0].Tag;
				if(strText.Length!=0)
				{
					WebMeeting.Client.PollingFreeFormAnswerForm form = new WebMeeting.Client.PollingFreeFormAnswerForm();
					form.textBox1.Text = strText;
					form.label1.Text = strName +  " Replied";
					form.ShowDialog();
				}
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollResult.cs @listViewAttendeeAnswers_DoubleClick( line==> 1989",exp,null,false);
			}
		}

		# endregion


		private void btnOption_Click(object sender, System.EventArgs e)
		{
			
			objPollingOptions.m_panelDrawing = m_panelDrawing;
			objPollingOptions.ShowDialog();
		
		}

		private void PollResult_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			barGraphBox.Refresh();
		}

	

		# region barGraphBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		private void barGraphBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			try
			{
				Graphics objGraphics = e.Graphics;						
				DrawChart(ref objGraphics);
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>PollResult.cs @barGraphBox_Paint( line==> 2022",exp,null,false);
			}
			
		}
		# endregion


		private void btnBarGraph_Click(object sender, System.EventArgs e)
		{
			barGraphBox.BackColor = Color.White;
			barGraphBox.Visible = true;
			m_panelDrawing.Visible = false;
			btnPieChart.Visible = true;
			btnBarGraph.Visible = false;
		}

		private void panel2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void nsButton1_Click(object sender, System.EventArgs e)
		{				
			barGraphBox.BackColor = Color.White;
			m_panelDrawing.Visible = true;			
			barGraphBox.Visible = false;
			btnPieChart.Visible = true;
		}

	
		private void PollResult_Resize(object sender, System.EventArgs e)
		{
			barGraphBox.Width = this.Width/2 ;
			m_panelDrawing.Left = barGraphBox.Right + 10;	
			m_panelDrawing.Width = this.Width/2-20;
			barGraphBox.Refresh();
		}

		private void m_panelDrawing_MouseHover(object sender, System.EventArgs e)
		{
		   
		}

		private void groupBox3_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void panelTop_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

//		private void button3_Click(object sender, System.EventArgs e)
//		{
//			
//			Trace.WriteLine("GroupBox3 Location " + this.groupBox3.Location.ToString());
//			Trace.WriteLine("GroupBox3 top : " + this.groupBox3.Top.ToString());
//			Trace.WriteLine("GroupBox3 Size : " + this.groupBox3.Size.ToString());
//			Trace.WriteLine("listViewAttendeeAnswers location : " + this.listViewAttendeeAnswers.Top.ToString());
//			Trace.WriteLine("listViewAttendeeAnswers top : " + this.listViewAttendeeAnswers.Top.ToString());
//			Trace.WriteLine("listViewAttendeeAnswers size : " + this.listViewAttendeeAnswers.Top.ToString());
//		}

//		private void button3_Click(object sender, System.EventArgs e)
//		{
//			System.Windows.Forms.MessageBox.Show("GroupsBox : " + this.groupBox3.Size.ToString());
//			System.Windows.Forms.MessageBox.Show("Panel4 : " + this.panel4.Size.ToString());
//			System.Windows.Forms.MessageBox.Show("Paneltop : " + this.panelTop.Size.ToString());
//			System.Windows.Forms.MessageBox.Show("listview : " + this.listViewAttendeeAnswers.Size.ToString());
//		}

		
		#region BarChart Structure
		public struct BarChartEntry
		{
			public System.Drawing.Color DrawColor;
            public string StrText;
			public decimal Percentage;			
		};
		#endregion
	}
}