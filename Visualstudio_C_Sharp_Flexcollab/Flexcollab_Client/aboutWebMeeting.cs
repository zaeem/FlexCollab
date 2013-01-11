using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for aboutWebMeeting.
	/// </summary>
	public class aboutWebMeeting : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblWebMeetingVersion;
		private System.Windows.Forms.Label lblProductName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public aboutWebMeeting()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(aboutWebMeeting));
			this.label1 = new System.Windows.Forms.Label();
			this.lblWebMeetingVersion = new System.Windows.Forms.Label();
			this.lblProductName = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(24, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(148, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Web Meeting Client Version:";
			// 
			// lblWebMeetingVersion
			// 
			this.lblWebMeetingVersion.Location = new System.Drawing.Point(176, 40);
			this.lblWebMeetingVersion.Name = "lblWebMeetingVersion";
			this.lblWebMeetingVersion.TabIndex = 1;
			this.lblWebMeetingVersion.Text = "0.0.0.0";
			// 
			// lblProductName
			// 
			this.lblProductName.Location = new System.Drawing.Point(176, 16);
			this.lblProductName.Name = "lblProductName";
			this.lblProductName.TabIndex = 3;
			this.lblProductName.Text = "Web Meeting ";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(24, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(83, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Product Name :";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(104, 72);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(88, 24);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// aboutWebMeeting
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(304, 102);
			this.ControlBox = false;
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblProductName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblWebMeetingVersion);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(312, 136);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(312, 136);
			this.Name = "aboutWebMeeting";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About Web Meeting";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.aboutWebMeeting_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void aboutWebMeeting_Load(object sender, System.EventArgs e)
		{
			lblProductName.Text = Application.ProductName;
			lblWebMeetingVersion.Text = Application.ProductVersion;
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			Close();
		}
	}
}
