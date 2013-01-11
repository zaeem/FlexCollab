using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WebMeeting
{
	/// <summary>
	/// Summary description for DebugForm.
	/// </summary>
	public class DebugForm : System.Windows.Forms.Form
	{
		public System.Windows.Forms.ListBox listDebug;
		public System.Windows.Forms.PictureBox pictureBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DebugForm()
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
			this.listDebug = new System.Windows.Forms.ListBox();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// listDebug
			// 
			this.listDebug.Location = new System.Drawing.Point(8, 8);
			this.listDebug.Name = "listDebug";
			this.listDebug.ScrollAlwaysVisible = true;
			this.listDebug.Size = new System.Drawing.Size(664, 95);
			this.listDebug.TabIndex = 0;
			// 
			// pictureBox
			// 
			this.pictureBox.Location = new System.Drawing.Point(48, 128);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(296, 200);
			this.pictureBox.TabIndex = 1;
			this.pictureBox.TabStop = false;
			this.pictureBox.Visible = false;
			// 
			// DebugForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(680, 110);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.listDebug);
			this.Name = "DebugForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DebugForm";
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		#endregion
	}
}
