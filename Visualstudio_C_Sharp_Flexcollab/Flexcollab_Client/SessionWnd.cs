using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text;


namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for SessionWnd.
	/// </summary>
	public class SessionWnd : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtSessionID;
		public string t_response;
		private NSPAControls.NSButton btnClose;
		private NSPAControls.NSButton btnJoin;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SessionWnd()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SessionWnd));
			this.label1 = new System.Windows.Forms.Label();
			this.txtSessionID = new System.Windows.Forms.TextBox();
			this.btnClose = new NSPAControls.NSButton();
			this.btnJoin = new NSPAControls.NSButton();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(136, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Please Enter Session Id";
			// 
			// txtSessionID
			// 
			this.txtSessionID.Location = new System.Drawing.Point(144, 13);
			this.txtSessionID.Name = "txtSessionID";
			this.txtSessionID.Size = new System.Drawing.Size(96, 20);
			this.txtSessionID.TabIndex = 1;
			this.txtSessionID.Text = "";
			this.txtSessionID.TextChanged += new System.EventHandler(this.txtSessionID_TextChanged);
			// 
			// btnClose
			// 
			this.btnClose.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnClose.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnClose.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnClose.HottrackImage")));
			this.btnClose.Location = new System.Drawing.Point(165, 40);
			this.btnClose.Name = "btnClose";
			this.btnClose.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnClose.NormalImage")));
			this.btnClose.OnlyShowBitmap = true;
			this.btnClose.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnClose.PressedImage")));
			this.btnClose.Size = new System.Drawing.Size(80, 32);
			this.btnClose.Text = "Invite";
			this.btnClose.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnClose.ToolTip = null;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnJoin
			// 
			this.btnJoin.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnJoin.Enabled = false;
			this.btnJoin.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnJoin.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnJoin.HottrackImage")));
			this.btnJoin.Location = new System.Drawing.Point(104, 45);
			this.btnJoin.Name = "btnJoin";
			this.btnJoin.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnJoin.NormalImage")));
			this.btnJoin.OnlyShowBitmap = true;
			this.btnJoin.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnJoin.PressedImage")));
			this.btnJoin.Size = new System.Drawing.Size(56, 24);
			this.btnJoin.TextAlign = NSPAControls.eTextAlign.Right;
			this.btnJoin.ToolTip = null;
			this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
			// 
			// SessionWnd
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.ClientSize = new System.Drawing.Size(250, 74);
			this.ControlBox = false;
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnJoin);
			this.Controls.Add(this.txtSessionID);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "SessionWnd";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.ResumeLayout(false);

		}
		#endregion

		private void btnJoin_Click(object sender, System.EventArgs e)
		{
			string urlEnd;
			try
			{				
				// used to build entire input
				StringBuilder sb  = new StringBuilder();

				// used on each read operation
				byte[]        buf = new byte[8192];

				// prepare the web page we will be asking for
				HttpWebRequest  request  = (HttpWebRequest)
					WebRequest.Create("http://www.compassnav.com/members/getinfo1.php?session_id="+ txtSessionID.Text.Trim());

				// execute the request
				HttpWebResponse response = (HttpWebResponse)
					request.GetResponse();

				// we will read data via the response stream
				Stream resStream = response.GetResponseStream();

				string tempString = null;
				int    count      = 0;

				do
				{
					// fill the buffer with data
					count = resStream.Read(buf, 0, buf.Length);

					// make sure we read some data
					if (count != 0)
					{
						// translate from bytes to ASCII text
						tempString = Encoding.ASCII.GetString(buf, 0, count);

						// continue building the string
						sb.Append(tempString);
					}
				}
				while (count > 0); // any more data to read?
				string res=sb.ToString();
				//MessageBox.Show("retrive information:::"+res.ToString());
                WebMeeting.Client.ClientUI.getInstance().setCommandLine(res);
				//string[] data=res.Split(' ');
				//1+0 6 5 "192.168.0.111:5600     host+password meeting_id userid ip:port
				//WebMeeting.Client.ClientUI.getInstance().SetConnectionProperties(data[0],data[1],data[2],data[3]);
                //WebMeeting.Client.ClientUI.getInstance().ConnectNetwork(true);
				this.DialogResult=DialogResult.OK;
				this.Close();
			}
			catch(Exception ee)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage(ee.Message);
			}

		}

		private void txtSessionID_TextChanged(object sender, System.EventArgs e)
		{
		  if(txtSessionID.Text.Length>0)
			  btnJoin.Enabled=true;
		  else
              btnJoin.Enabled=false;
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.DialogResult=DialogResult.Cancel;			
			//System.Environment.Exit(1);
		}
	}
}
