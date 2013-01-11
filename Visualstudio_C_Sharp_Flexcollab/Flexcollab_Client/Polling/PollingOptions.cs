using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.PieChart;
namespace WebMeeting.Polling
{
	/// <summary>
	/// Summary description for PollingOptions.
	/// </summary>
	public class PollingOptions : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.NumericUpDown numericUpDownBottomMargin;
		private System.Windows.Forms.NumericUpDown numericUpDownTopMargin;
		private System.Windows.Forms.NumericUpDown numericUpDownRightMargin;
		private System.Windows.Forms.NumericUpDown numericUpDownLeftMargin;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioButtonShadowStyleGradual;
		private System.Windows.Forms.RadioButton radioButtonShadowStyleUniform;
		private System.Windows.Forms.RadioButton radioButtonShadowStyleNone;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.CheckBox checkBoxFitChart;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBoxEdgeType;
		private System.Windows.Forms.NumericUpDown numericUpDownAngle;
		private System.Windows.Forms.NumericUpDown numericUpDownEdgeLineWidth;
		private System.Windows.Forms.NumericUpDown numericUpDownPieHeight;
		private System.Windows.Forms.GroupBox groupBox2;

		public System.Drawing.PieChart.PieChartControl m_panelDrawing = null;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PollingOptions()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PollingOptions));
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.numericUpDownBottomMargin = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownTopMargin = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownRightMargin = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownLeftMargin = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioButtonShadowStyleGradual = new System.Windows.Forms.RadioButton();
			this.radioButtonShadowStyleUniform = new System.Windows.Forms.RadioButton();
			this.radioButtonShadowStyleNone = new System.Windows.Forms.RadioButton();
			this.label11 = new System.Windows.Forms.Label();
			this.checkBoxFitChart = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBoxEdgeType = new System.Windows.Forms.ComboBox();
			this.numericUpDownAngle = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownEdgeLineWidth = new System.Windows.Forms.NumericUpDown();
			this.numericUpDownPieHeight = new System.Windows.Forms.NumericUpDown();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownBottomMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTopMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRightMargin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeftMargin)).BeginInit();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngle)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownEdgeLineWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPieHeight)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.numericUpDownBottomMargin);
			this.groupBox4.Controls.Add(this.numericUpDownTopMargin);
			this.groupBox4.Controls.Add(this.numericUpDownRightMargin);
			this.groupBox4.Controls.Add(this.numericUpDownLeftMargin);
			this.groupBox4.Controls.Add(this.label7);
			this.groupBox4.Controls.Add(this.label6);
			this.groupBox4.Controls.Add(this.label5);
			this.groupBox4.Controls.Add(this.label4);
			this.groupBox4.Location = new System.Drawing.Point(8, 0);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(400, 88);
			this.groupBox4.TabIndex = 19;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "&Margins";
			// 
			// numericUpDownBottomMargin
			// 
			this.numericUpDownBottomMargin.Location = new System.Drawing.Point(208, 48);
			this.numericUpDownBottomMargin.Maximum = new System.Decimal(new int[] {
																					  20,
																					  0,
																					  0,
																					  0});
			this.numericUpDownBottomMargin.Name = "numericUpDownBottomMargin";
			this.numericUpDownBottomMargin.Size = new System.Drawing.Size(56, 20);
			this.numericUpDownBottomMargin.TabIndex = 7;
			this.numericUpDownBottomMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownBottomMargin.Value = new System.Decimal(new int[] {
																					10,
																					0,
																					0,
																					0});
			this.numericUpDownBottomMargin.ValueChanged += new System.EventHandler(this.numericUpDownBottomMargin_ValueChanged);
			// 
			// numericUpDownTopMargin
			// 
			this.numericUpDownTopMargin.Location = new System.Drawing.Point(208, 24);
			this.numericUpDownTopMargin.Maximum = new System.Decimal(new int[] {
																				   20,
																				   0,
																				   0,
																				   0});
			this.numericUpDownTopMargin.Name = "numericUpDownTopMargin";
			this.numericUpDownTopMargin.Size = new System.Drawing.Size(56, 20);
			this.numericUpDownTopMargin.TabIndex = 5;
			this.numericUpDownTopMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownTopMargin.Value = new System.Decimal(new int[] {
																				 10,
																				 0,
																				 0,
																				 0});
			this.numericUpDownTopMargin.ValueChanged += new System.EventHandler(this.numericUpDownTopMargin_ValueChanged);
			// 
			// numericUpDownRightMargin
			// 
			this.numericUpDownRightMargin.Location = new System.Drawing.Point(56, 48);
			this.numericUpDownRightMargin.Maximum = new System.Decimal(new int[] {
																					 20,
																					 0,
																					 0,
																					 0});
			this.numericUpDownRightMargin.Name = "numericUpDownRightMargin";
			this.numericUpDownRightMargin.Size = new System.Drawing.Size(56, 20);
			this.numericUpDownRightMargin.TabIndex = 3;
			this.numericUpDownRightMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownRightMargin.Value = new System.Decimal(new int[] {
																				   10,
																				   0,
																				   0,
																				   0});
			this.numericUpDownRightMargin.ValueChanged += new System.EventHandler(this.numericUpDownRightMargin_ValueChanged);
			// 
			// numericUpDownLeftMargin
			// 
			this.numericUpDownLeftMargin.Location = new System.Drawing.Point(56, 24);
			this.numericUpDownLeftMargin.Maximum = new System.Decimal(new int[] {
																					20,
																					0,
																					0,
																					0});
			this.numericUpDownLeftMargin.Name = "numericUpDownLeftMargin";
			this.numericUpDownLeftMargin.Size = new System.Drawing.Size(56, 20);
			this.numericUpDownLeftMargin.TabIndex = 1;
			this.numericUpDownLeftMargin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownLeftMargin.Value = new System.Decimal(new int[] {
																				  10,
																				  0,
																				  0,
																				  0});
			this.numericUpDownLeftMargin.ValueChanged += new System.EventHandler(this.numericUpDownLeftMargin_ValueChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(160, 48);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(40, 16);
			this.label7.TabIndex = 6;
			this.label7.Text = "&Bottom:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(160, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(40, 16);
			this.label6.TabIndex = 4;
			this.label6.Text = "&Top:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 48);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 16);
			this.label5.TabIndex = 2;
			this.label5.Text = "&Right:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 16);
			this.label4.TabIndex = 0;
			this.label4.Text = "&Left:";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Controls.Add(this.label11);
			this.panel1.Controls.Add(this.checkBoxFitChart);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.comboBoxEdgeType);
			this.panel1.Controls.Add(this.numericUpDownAngle);
			this.panel1.Controls.Add(this.numericUpDownEdgeLineWidth);
			this.panel1.Controls.Add(this.numericUpDownPieHeight);
			this.panel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.panel1.Location = new System.Drawing.Point(16, 16);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(368, 120);
			this.panel1.TabIndex = 20;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(144, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 16);
			this.label3.TabIndex = 9;
			this.label3.Text = "&Edge color:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(144, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Initial angle:";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioButtonShadowStyleGradual);
			this.groupBox1.Controls.Add(this.radioButtonShadowStyleUniform);
			this.groupBox1.Controls.Add(this.radioButtonShadowStyleNone);
			this.groupBox1.Location = new System.Drawing.Point(8, 32);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(112, 80);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Shadow style";
			// 
			// radioButtonShadowStyleGradual
			// 
			this.radioButtonShadowStyleGradual.Checked = true;
			this.radioButtonShadowStyleGradual.Location = new System.Drawing.Point(16, 56);
			this.radioButtonShadowStyleGradual.Name = "radioButtonShadowStyleGradual";
			this.radioButtonShadowStyleGradual.Size = new System.Drawing.Size(80, 16);
			this.radioButtonShadowStyleGradual.TabIndex = 2;
			this.radioButtonShadowStyleGradual.TabStop = true;
			this.radioButtonShadowStyleGradual.Tag = "";
			this.radioButtonShadowStyleGradual.Text = "&Gradual";
			this.radioButtonShadowStyleGradual.CheckedChanged += new System.EventHandler(this.radioButtonShadowStyleNone_CheckedChanged);
			// 
			// radioButtonShadowStyleUniform
			// 
			this.radioButtonShadowStyleUniform.Location = new System.Drawing.Point(16, 36);
			this.radioButtonShadowStyleUniform.Name = "radioButtonShadowStyleUniform";
			this.radioButtonShadowStyleUniform.Size = new System.Drawing.Size(80, 16);
			this.radioButtonShadowStyleUniform.TabIndex = 1;
			this.radioButtonShadowStyleUniform.Tag = "";
			this.radioButtonShadowStyleUniform.Text = "&Uniform";
			this.radioButtonShadowStyleUniform.CheckedChanged += new System.EventHandler(this.radioButtonShadowStyleNone_CheckedChanged);
			// 
			// radioButtonShadowStyleNone
			// 
			this.radioButtonShadowStyleNone.Location = new System.Drawing.Point(16, 16);
			this.radioButtonShadowStyleNone.Name = "radioButtonShadowStyleNone";
			this.radioButtonShadowStyleNone.Size = new System.Drawing.Size(80, 16);
			this.radioButtonShadowStyleNone.TabIndex = 0;
			this.radioButtonShadowStyleNone.Tag = "";
			this.radioButtonShadowStyleNone.Text = "&None";
			this.radioButtonShadowStyleNone.CheckedChanged += new System.EventHandler(this.radioButtonShadowStyleNone_CheckedChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(144, 88);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(64, 16);
			this.label11.TabIndex = 11;
			this.label11.Text = "Edge &width:";
			// 
			// checkBoxFitChart
			// 
			this.checkBoxFitChart.Location = new System.Drawing.Point(8, 8);
			this.checkBoxFitChart.Name = "checkBoxFitChart";
			this.checkBoxFitChart.Size = new System.Drawing.Size(112, 16);
			this.checkBoxFitChart.TabIndex = 3;
			this.checkBoxFitChart.Text = "&Fit chart to panel";
			this.checkBoxFitChart.CheckedChanged += new System.EventHandler(this.checkBoxFitChart_CheckedChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(144, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Pie &height:";
			// 
			// comboBoxEdgeType
			// 
			this.comboBoxEdgeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxEdgeType.Location = new System.Drawing.Point(216, 8);
			this.comboBoxEdgeType.Name = "comboBoxEdgeType";
			this.comboBoxEdgeType.Size = new System.Drawing.Size(120, 21);
			this.comboBoxEdgeType.TabIndex = 10;
			this.comboBoxEdgeType.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdgeType_SelectedIndexChanged);
			// 
			// numericUpDownAngle
			// 
			this.numericUpDownAngle.Increment = new System.Decimal(new int[] {
																				 10,
																				 0,
																				 0,
																				 0});
			this.numericUpDownAngle.Location = new System.Drawing.Point(264, 64);
			this.numericUpDownAngle.Maximum = new System.Decimal(new int[] {
																			   400,
																			   0,
																			   0,
																			   0});
			this.numericUpDownAngle.Minimum = new System.Decimal(new int[] {
																			   360,
																			   0,
																			   0,
																			   -2147483648});
			this.numericUpDownAngle.Name = "numericUpDownAngle";
			this.numericUpDownAngle.Size = new System.Drawing.Size(64, 21);
			this.numericUpDownAngle.TabIndex = 8;
			this.numericUpDownAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownAngle.ValueChanged += new System.EventHandler(this.numericUpDownAngle_ValueChanged);
			// 
			// numericUpDownEdgeLineWidth
			// 
			this.numericUpDownEdgeLineWidth.DecimalPlaces = 1;
			this.numericUpDownEdgeLineWidth.Location = new System.Drawing.Point(264, 88);
			this.numericUpDownEdgeLineWidth.Maximum = new System.Decimal(new int[] {
																					   5,
																					   0,
																					   0,
																					   0});
			this.numericUpDownEdgeLineWidth.Name = "numericUpDownEdgeLineWidth";
			this.numericUpDownEdgeLineWidth.Size = new System.Drawing.Size(64, 21);
			this.numericUpDownEdgeLineWidth.TabIndex = 12;
			this.numericUpDownEdgeLineWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownEdgeLineWidth.ValueChanged += new System.EventHandler(this.numericUpDownEdgeLineWidth_ValueChanged);
			// 
			// numericUpDownPieHeight
			// 
			this.numericUpDownPieHeight.DecimalPlaces = 2;
			this.numericUpDownPieHeight.Increment = new System.Decimal(new int[] {
																					 5,
																					 0,
																					 0,
																					 131072});
			this.numericUpDownPieHeight.Location = new System.Drawing.Point(264, 40);
			this.numericUpDownPieHeight.Maximum = new System.Decimal(new int[] {
																				   5,
																				   0,
																				   0,
																				   65536});
			this.numericUpDownPieHeight.Name = "numericUpDownPieHeight";
			this.numericUpDownPieHeight.Size = new System.Drawing.Size(64, 21);
			this.numericUpDownPieHeight.TabIndex = 6;
			this.numericUpDownPieHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numericUpDownPieHeight.Value = new System.Decimal(new int[] {
																				 20,
																				 0,
																				 0,
																				 131072});
			this.numericUpDownPieHeight.ValueChanged += new System.EventHandler(this.numericUpDownPieHeight_ValueChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.panel1);
			this.groupBox2.Location = new System.Drawing.Point(8, 104);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(400, 144);
			this.groupBox2.TabIndex = 21;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Misc";
			// 
			// PollingOptions
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 254);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox4);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "PollingOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Polling Options";
			this.Load += new System.EventHandler(this.PollingOptions_Load);
			this.groupBox4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownBottomMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownTopMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownRightMargin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeftMargin)).EndInit();
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngle)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownEdgeLineWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDownPieHeight)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void radioButtonShadowStyleNone_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radioButtonShadowStyleNone.Checked)
				m_panelDrawing.ShadowStyle = ShadowStyle.NoShadow;
			else if (radioButtonShadowStyleUniform.Checked)
				m_panelDrawing.ShadowStyle = ShadowStyle.UniformShadow;
			else
				m_panelDrawing.ShadowStyle = ShadowStyle.GradualShadow;
		}

		private void checkBoxFitChart_CheckedChanged(object sender, System.EventArgs e)
		{	
			m_panelDrawing.FitChart = checkBoxFitChart.Checked;
		
		}

		private void numericUpDownLeftMargin_ValueChanged(object sender, System.EventArgs e)
		{
			m_panelDrawing.LeftMargin = (float)numericUpDownLeftMargin.Value;
		}

		private void numericUpDownRightMargin_ValueChanged(object sender, System.EventArgs e)
		{
			m_panelDrawing.RightMargin = (float)numericUpDownRightMargin.Value;
		}

		private void numericUpDownTopMargin_ValueChanged(object sender, System.EventArgs e)
		{
			m_panelDrawing.TopMargin = (float)numericUpDownTopMargin.Value;
		}

		private void numericUpDownBottomMargin_ValueChanged(object sender, System.EventArgs e)
		{
			m_panelDrawing.BottomMargin = (float)numericUpDownBottomMargin.Value;
		}

		private void comboBoxEdgeType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_panelDrawing.EdgeColorType = (EdgeColorType)comboBoxEdgeType.SelectedIndex;
		}

		private void numericUpDownPieHeight_ValueChanged(object sender, System.EventArgs e)
		{
			m_panelDrawing.SliceRelativeHeight = (float)numericUpDownPieHeight.Value;
		}

		private void numericUpDownAngle_ValueChanged(object sender, System.EventArgs e)
		{
			m_panelDrawing.InitialAngle = (float)numericUpDownAngle.Value;
		}

		private void numericUpDownEdgeLineWidth_ValueChanged(object sender, System.EventArgs e)
		{
			m_panelDrawing.EdgeLineWidth = (float)numericUpDownEdgeLineWidth.Value;
		}

		private void PollingOptions_Load(object sender, System.EventArgs e)
		{
			string[] types = Enum.GetNames(typeof(EdgeColorType));
			comboBoxEdgeType.Items.AddRange(types);
			comboBoxEdgeType.SelectedIndex = (int)EdgeColorType.DarkerThanSurface;

			/*		numericUpDownLeftMargin.Value = (decimal) m_panelDrawing.LeftMargin;
					numericUpDownRightMargin.Value =(decimal) m_panelDrawing.RightMargin;
					numericUpDownTopMargin.Value = (decimal)m_panelDrawing.TopMargin;			         
					numericUpDownBottomMargin.Value = (decimal)m_panelDrawing.BottomMargin;//      = (float);
					checkBoxFitChart.Checked = m_panelDrawing.FitChart           ;
					numericUpDownPieHeight.Value =(decimal) m_panelDrawing.SliceRelativeHeight ;
					numericUpDownAngle.Value =(decimal) m_panelDrawing.InitialAngle;
					numericUpDownEdgeLineWidth.Value = (decimal)m_panelDrawing.EdgeLineWidth;
					*/

		}

	
	}
}
