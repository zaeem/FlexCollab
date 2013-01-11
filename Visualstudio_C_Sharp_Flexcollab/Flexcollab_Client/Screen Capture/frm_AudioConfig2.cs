using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
namespace WebMeeting.Client.Screen_Capture
{
	/// <summary>
	/// Summary description for frm_AudioConfig2.
	/// </summary>
	public class frm_AudioConfig2 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox Tip;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btn_Cancel;
		private System.Windows.Forms.Button btn_Finish;
		private System.Windows.Forms.Button btn_Next;
		private System.Windows.Forms.Button btn_Previous;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rd_Default;
		private System.Windows.Forms.RadioButton rd_High;
		private System.Windows.Forms.RadioButton rd_Medium;
		private System.Windows.Forms.RadioButton rd_Low;
		public ScreenCapture.Config config2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Audio Configuration Constructor
		public frm_AudioConfig2()
		{
			//
			// Required for Windows Form Designer support
			//
			config2=new ScreenCapture.Config();
			config2.cfgFile = "WebMeeting.exe.config"; 
		    
			InitializeComponent();
			btn_Next.Enabled=false;
			//correct
			//System.Configuration.ConfigurationSettings.AppSettings.Get("Quality");
			
			
			
			string str_quality=config2.Read("Quality");
			
			if(str_quality.Equals("1"))
				rd_Low.Checked=true;
			else if(str_quality.Equals("2"))
			rd_Medium.Checked=true;
			else if(str_quality.Equals("3"))
				rd_High.Checked=true;
			else 
				rd_Default.Checked=true;
			
			//else if(ConfigurationSettings.AppSettings.Get("Quality")=="3")
			//rd_High.Checked=true;
			
			//else 
			//rd_Default.Checked=true;
			

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		#endregion
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frm_AudioConfig2));
			this.Tip = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.btn_Finish = new System.Windows.Forms.Button();
			this.btn_Next = new System.Windows.Forms.Button();
			this.btn_Previous = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rd_Low = new System.Windows.Forms.RadioButton();
			this.rd_Medium = new System.Windows.Forms.RadioButton();
			this.rd_High = new System.Windows.Forms.RadioButton();
			this.rd_Default = new System.Windows.Forms.RadioButton();
			this.Tip.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// Tip
			// 
			this.Tip.Controls.Add(this.label3);
			this.Tip.Controls.Add(this.pictureBox1);
			this.Tip.Location = new System.Drawing.Point(8, 136);
			this.Tip.Name = "Tip";
			this.Tip.Size = new System.Drawing.Size(352, 80);
			this.Tip.TabIndex = 4;
			this.Tip.TabStop = false;
			this.Tip.Text = "Tip";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(56, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(272, 40);
			this.label3.TabIndex = 3;
			this.label3.Text = "The default Stream quality is selected automatically. Use other cherck boxes to c" +
				"hange the streaming quality.";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 24);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.Location = new System.Drawing.Point(280, 224);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.TabIndex = 12;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
			// 
			// btn_Finish
			// 
			this.btn_Finish.Location = new System.Drawing.Point(192, 224);
			this.btn_Finish.Name = "btn_Finish";
			this.btn_Finish.TabIndex = 11;
			this.btn_Finish.Text = "Finish";
			this.btn_Finish.Click += new System.EventHandler(this.btn_Finish_Click);
			// 
			// btn_Next
			// 
			this.btn_Next.Location = new System.Drawing.Point(88, 224);
			this.btn_Next.Name = "btn_Next";
			this.btn_Next.TabIndex = 10;
			this.btn_Next.Text = "Next >";
			// 
			// btn_Previous
			// 
			this.btn_Previous.Location = new System.Drawing.Point(8, 224);
			this.btn_Previous.Name = "btn_Previous";
			this.btn_Previous.TabIndex = 9;
			this.btn_Previous.Text = "< Previous";
			this.btn_Previous.Click += new System.EventHandler(this.btn_Previous_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rd_Low);
			this.groupBox1.Controls.Add(this.rd_Medium);
			this.groupBox1.Controls.Add(this.rd_High);
			this.groupBox1.Controls.Add(this.rd_Default);
			this.groupBox1.Location = new System.Drawing.Point(24, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(120, 120);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Quality Control";
			// 
			// rd_Low
			// 
			this.rd_Low.Location = new System.Drawing.Point(8, 88);
			this.rd_Low.Name = "rd_Low";
			this.rd_Low.Size = new System.Drawing.Size(56, 24);
			this.rd_Low.TabIndex = 3;
			this.rd_Low.Text = "Low";
			// 
			// rd_Medium
			// 
			this.rd_Medium.Location = new System.Drawing.Point(8, 64);
			this.rd_Medium.Name = "rd_Medium";
			this.rd_Medium.Size = new System.Drawing.Size(64, 24);
			this.rd_Medium.TabIndex = 2;
			this.rd_Medium.Text = "Medium";
			// 
			// rd_High
			// 
			this.rd_High.Location = new System.Drawing.Point(8, 40);
			this.rd_High.Name = "rd_High";
			this.rd_High.Size = new System.Drawing.Size(64, 24);
			this.rd_High.TabIndex = 1;
			this.rd_High.Text = "High";
			// 
			// rd_Default
			// 
			this.rd_Default.Location = new System.Drawing.Point(8, 16);
			this.rd_Default.Name = "rd_Default";
			this.rd_Default.Size = new System.Drawing.Size(64, 24);
			this.rd_Default.TabIndex = 0;
			this.rd_Default.Text = "Default";
			// 
			// frm_AudioConfig2
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(376, 262);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.btn_Finish);
			this.Controls.Add(this.btn_Next);
			this.Controls.Add(this.btn_Previous);
			this.Controls.Add(this.Tip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frm_AudioConfig2";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Audio Configuration";
			this.Load += new System.EventHandler(this.frm_AudioConfig2_Load);
			this.Tip.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Cancel Button
		private void btn_Cancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
		#endregion

		#region Previous Button
		private void btn_Previous_Click(object sender, System.EventArgs e)
		{
			frm_AudioConfig fa=new frm_AudioConfig();
			this.Close();
			fa.Show();
		}
		#endregion	

		#region Finish Button
		private void btn_Finish_Click(object sender, System.EventArgs e)
		{
			
			if(rd_Low.Checked)
			{
			ScreenCapture.ScreenCapture.Flag_configpro="Screen Video/Audio Medium (CBR)100";
			config2.SetValue("//appSettings//add[@key='Quality']", "1");
			
			}
			else if(rd_Medium.Checked)
			{
				ScreenCapture.ScreenCapture.Flag_configpro="Screen Video/Audio Medium (CBR)400";
				config2.SetValue("//appSettings//add[@key='Quality']", "2");
			
			}
			else if(rd_High.Checked)
			{
				ScreenCapture.ScreenCapture.Flag_configpro="Screen Video/Audio Medium (CBR)600";
				config2.SetValue("//appSettings//add[@key='Quality']", "3");
			
			}
			else 
				
			{
				ScreenCapture.ScreenCapture.Flag_configpro="Screen Video/Audio Medium (CBR)";
				config2.SetValue("//appSettings//add[@key='Quality']", "0");
			}

			this.Close();
		}
         #endregion

		private void frm_AudioConfig2_Load(object sender, System.EventArgs e)
		{
		
		}

		

		

		

		
	}
}
