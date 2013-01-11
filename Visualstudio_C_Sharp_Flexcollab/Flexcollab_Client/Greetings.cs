using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for Greetings.
	/// </summary>
	public class Greetings : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.TextBox txtBoxMsg;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.Button tbnCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Greetings()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Greetings));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnSend = new System.Windows.Forms.Button();
			this.txtBoxMsg = new System.Windows.Forms.TextBox();
			this.tbnCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnSend);
			this.groupBox1.Controls.Add(this.txtBoxMsg);
			this.groupBox1.Controls.Add(this.tbnCancel);
			this.groupBox1.Location = new System.Drawing.Point(8, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(288, 128);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Please Enter Your Message";
			// 
			// btnSend
			// 
			this.btnSend.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSend.Enabled = false;
			this.btnSend.Location = new System.Drawing.Point(112, 96);
			this.btnSend.Name = "btnSend";
			this.btnSend.TabIndex = 2;
			this.btnSend.Text = "Send";
			// 
			// txtBoxMsg
			// 
			this.txtBoxMsg.Location = new System.Drawing.Point(8, 24);
			this.txtBoxMsg.MaxLength = 200;
			this.txtBoxMsg.Multiline = true;
			this.txtBoxMsg.Name = "txtBoxMsg";
			this.txtBoxMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtBoxMsg.Size = new System.Drawing.Size(272, 64);
			this.txtBoxMsg.TabIndex = 1;
			this.txtBoxMsg.Text = "";
			this.txtBoxMsg.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// tbnCancel
			// 
			this.tbnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.tbnCancel.Location = new System.Drawing.Point(200, 96);
			this.tbnCancel.Name = "tbnCancel";
			this.tbnCancel.TabIndex = 2;
			this.tbnCancel.Text = "&Cancel";
			// 
			// Greetings
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(306, 136);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Greetings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Greetings";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			btnSend.Enabled = (txtBoxMsg.Text.Length > 1);
		}
	}
}
