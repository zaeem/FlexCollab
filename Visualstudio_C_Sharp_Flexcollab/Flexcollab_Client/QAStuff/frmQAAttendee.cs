using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WebMeeting.Client.QAStuff
{
	/// <summary>
	/// Summary description for frmQAAttendee.
	/// </summary>
	public class frmQAAttendee : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmQAAttendee()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmQAAttendee));
			// 
			// frmQAAttendee
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(655, 463);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmQAAttendee";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Attendee";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmQAAttendee_Closing);
			this.Closed += new System.EventHandler(this.frmQAAttendee_Closed);

		}
		#endregion

		private void frmQAAttendee_Closed(object sender, System.EventArgs e)
		{
			
		}

		private void frmQAAttendee_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		this.Hide();
		e.Cancel=true;
		}
	}
}
