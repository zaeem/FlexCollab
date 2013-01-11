using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WebMeeting.Client.Screen_Capture
{
	/// <summary>
	/// Summary description for Playback.
	/// </summary>
	public class Playback : System.Windows.Forms.Form
	{
		private ScreenCapture.ScreenControl screenControl1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Playback()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			screenControl1.ChangeState(false);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Playback));
			// 
			// Playback
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(592, 390);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Playback";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Playback";
			this.Closed += new System.EventHandler(this.Playback_Closed);

		}
		#endregion

		private void screenControl1_Load(object sender, System.EventArgs e)
		{
		
		}

		private void Playback_Closed(object sender, System.EventArgs e)
		{
			try
			{
				screenControl1.screenCapture.StopPlayback();
			}

			catch(Exception exp)
			{
			exp.Message.ToString();
			}
		}

		
	}
}
