using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace WebMeeting.Client.Screen_Capture
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmPerformance : System.Windows.Forms.Form
	{
		private System.Windows.Forms.RichTextBox txtInstruction;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnEg;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmPerformance()
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
				if (components != null) 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmPerformance));
			this.txtInstruction = new System.Windows.Forms.RichTextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnEg = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtInstruction
			// 
			this.txtInstruction.Location = new System.Drawing.Point(8, 72);
			this.txtInstruction.Name = "txtInstruction";
			this.txtInstruction.Size = new System.Drawing.Size(272, 64);
			this.txtInstruction.TabIndex = 0;
			this.txtInstruction.Text = @"1 Right Click Your Desktop                                              2 Go to Properties->Settings->Advanced->Troubleshoot   3 Reduce Hardware accelartion From Full to None                                                                 e.g             None  < ---<---<-----<-----<------  Full";
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(144, 144);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(88, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnEg
			// 
			this.btnEg.Location = new System.Drawing.Point(56, 144);
			this.btnEg.Name = "btnEg";
			this.btnEg.Size = new System.Drawing.Size(88, 23);
			this.btnEg.TabIndex = 3;
			this.btnEg.Text = "View Example";
			this.btnEg.Click += new System.EventHandler(this.btnEg_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 16);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(56, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Did You Know....";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(48, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(192, 23);
			this.label2.TabIndex = 7;
			this.label2.Text = "Improve Performance Dramatically";
			// 
			// frmPerformance
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(288, 182);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.btnEg);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.txtInstruction);
			this.Name = "frmPerformance";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Performance Tip";
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		#endregion

		
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnEg_Click(object sender, System.EventArgs e)
		{
			MainForm frmpre=new MainForm();
			frmpre.Show();
		}

		
	}
}
