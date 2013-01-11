using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for InviteForm.
	/// </summary>
	public class InviteForm : System.Windows.Forms.Form
	{
		private NSPAControls.NSButton btnSend;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.TextBox txtEmamil;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.GroupBox groupBox1;
		private NSPAControls.NSButton btnClose;
		private System.Windows.Forms.TextBox txtname;
		private System.Windows.Forms.Label lblname;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InviteForm()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(InviteForm));
			this.btnSend = new NSPAControls.NSButton();
			this.lblInfo = new System.Windows.Forms.Label();
			this.txtEmamil = new System.Windows.Forms.TextBox();
			this.lblStatus = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtname = new System.Windows.Forms.TextBox();
			this.lblname = new System.Windows.Forms.Label();
			this.btnClose = new NSPAControls.NSButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnSend
			// 
			this.btnSend.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnSend.Enabled = false;
			this.btnSend.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnSend.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnSend.HottrackImage")));
			this.btnSend.Location = new System.Drawing.Point(171, 72);
			this.btnSend.Name = "btnSend";
			this.btnSend.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnSend.NormalImage")));
			this.btnSend.OnlyShowBitmap = true;
			this.btnSend.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnSend.PressedImage")));
			this.btnSend.Size = new System.Drawing.Size(56, 25);
			this.btnSend.Text = "Invite";
			this.btnSend.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnSend.ToolTip = null;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// lblInfo
			// 
			this.lblInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblInfo.ForeColor = System.Drawing.Color.White;
			this.lblInfo.Location = new System.Drawing.Point(16, 48);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(120, 23);
			this.lblInfo.TabIndex = 1;
			this.lblInfo.Text = "Email Address :";
			// 
			// txtEmamil
			// 
			this.txtEmamil.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtEmamil.Location = new System.Drawing.Point(120, 48);
			this.txtEmamil.Name = "txtEmamil";
			this.txtEmamil.Size = new System.Drawing.Size(168, 21);
			this.txtEmamil.TabIndex = 1;
			this.txtEmamil.Text = "";
			this.txtEmamil.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEmamil_KeyPress);
			this.txtEmamil.TextChanged += new System.EventHandler(this.txtEmamil_TextChanged);
			// 
			// lblStatus
			// 
			this.lblStatus.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.lblStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(237)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.lblStatus.Location = new System.Drawing.Point(29, 90);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(264, 16);
			this.lblStatus.TabIndex = 3;
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtname);
			this.groupBox1.Controls.Add(this.lblname);
			this.groupBox1.Controls.Add(this.btnClose);
			this.groupBox1.Controls.Add(this.btnSend);
			this.groupBox1.Controls.Add(this.txtEmamil);
			this.groupBox1.Controls.Add(this.lblInfo);
			this.groupBox1.ForeColor = System.Drawing.Color.White;
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(304, 112);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = " Invite  ";
			// 
			// txtname
			// 
			this.txtname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtname.Location = new System.Drawing.Point(120, 24);
			this.txtname.Name = "txtname";
			this.txtname.Size = new System.Drawing.Size(168, 21);
			this.txtname.TabIndex = 0;
			this.txtname.Text = "";
			// 
			// lblname
			// 
			this.lblname.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblname.ForeColor = System.Drawing.Color.White;
			this.lblname.Location = new System.Drawing.Point(16, 24);
			this.lblname.Name = "lblname";
			this.lblname.Size = new System.Drawing.Size(80, 23);
			this.lblname.TabIndex = 3;
			this.lblname.Text = "Name :";
			// 
			// btnClose
			// 
			this.btnClose.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnClose.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnClose.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnClose.HottrackImage")));
			this.btnClose.Location = new System.Drawing.Point(235, 72);
			this.btnClose.Name = "btnClose";
			this.btnClose.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnClose.NormalImage")));
			this.btnClose.OnlyShowBitmap = true;
			this.btnClose.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnClose.PressedImage")));
			this.btnClose.Size = new System.Drawing.Size(56, 25);
			this.btnClose.Text = "Invite";
			this.btnClose.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnClose.ToolTip = null;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// InviteForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.ClientSize = new System.Drawing.Size(322, 130);
			this.ControlBox = false;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lblStatus);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "InviteForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void txtEmamil_TextChanged(object sender, System.EventArgs e)
		{
			btnSend.Enabled = txtEmamil.Text.Length > 0;
		}
		public static string ByteArrayToStr(byte[] dBytes)
		{		
						
			string str;
			System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
			str = enc.GetString(dBytes);
			return str;
		}
		public static int OpenWebsite(String remoteFilename)
		{
			// Function will return the number of bytes processed
			// to the caller. Initialize to 0 here.
			int bytesProcessed = 0;

			// Assign values to these objects here so that they can
			// be referenced in the finally block
			Stream remoteStream  = null;
			Stream localStream   = null;
			WebResponse response = null;

			// Use a try/catch/finally block as both the WebRequest and Stream
			// classes throw exceptions upon error
			try
			{
				// Create a request for the specified remote file name
				WebRequest request = WebRequest.Create(remoteFilename);
				if (request != null)
				{
					// Send the request to the server and retrieve the
					// WebResponse object 
					response = request.GetResponse();
					if (response != null)
					{
						
						// Once the WebResponse object has been retrieved,
						// get the stream object associated with the response's data
						remoteStream = response.GetResponseStream();

						// Create the local file
						localStream = new MemoryStream();

						// Allocate a 1k buffer
						byte[] buffer = new byte[1024];
						int bytesRead;

						// Simple do/while loop to read from stream until
						// no bytes are returned
						bool bError =false;
						do
						{
							// Read data (up to 1k) from the stream
							bytesRead = remoteStream.Read (buffer, 0, buffer.Length);

							// Write the data to the local file
							localStream.Write (buffer, 0, bytesRead);
							string result = ByteArrayToStr(buffer);
							if(result.IndexOf("No input") != -1)
							{
								bytesProcessed = 0;
								bError= true;
								break;
							}
							// Increment total bytes processed
							bytesProcessed += bytesRead;
						} while (bytesRead > 0);
							
						if(!bError)
							bytesProcessed++;


					
					}
					
				}
			}
			catch(Exception e)
			{
				//Console.WriteLine(e.Message);
			}
			finally
			{
				// Close the response and streams objects here 
				// to make sure they're closed even if an exception
				// is thrown at some point
				if (response     != null) response.Close();
				if (remoteStream != null) remoteStream.Close();
				if (localStream  != null) localStream.Close();
			}

			// Return total bytes processed to caller.
			return bytesProcessed;
		}
	
		public void SendEmail()
		{
			isSending = true;
			lblStatus.Text = "Sending Invitation. Please wait";			// NetworkManager.getInstance().profile.Name
			if(OpenWebsite(Info.getInstance().WebsiteName + "/application/invite.php?name="+ this.txtname.Text + "&mid=" + Info.getInstance().ConferenceID + "&email=" + txtEmamil.Text + "&invite=1") > 0)
			{
				lblStatus.Text = "Invitation has been sent";
			}
			else
			{
				lblStatus.Text = "Unable to send Invitation";
			}
			btnSend.Enabled = true;
			isSending= false;
		}
		private bool isSending = false;
		public void btnSend_Click(object sender, System.EventArgs e)
		{
			Thread thread = new Thread(new ThreadStart(SendEmail));
            thread.Name = "InviteForm Send Email Thread: SendEmail()";
			btnSend.Enabled = false;
			thread.Start();
			this.Close();
		}

		/*private void InviteForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//e.Cancel = isSending;
			e.Cancel=true;
		}*/

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

	

		private void txtEmamil_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if( e.KeyChar == '\r' )
			{
		
				if(txtEmamil.Text.Trim()!="")
				{
					Thread thread = new Thread(new ThreadStart(SendEmail));
					thread.Name = "InviteForm Send Email Thread: SendEmail()";
					btnSend.Enabled = false;
					thread.Start();
					this.Close();
			
				}
			}
		}

		
		
	}
}
