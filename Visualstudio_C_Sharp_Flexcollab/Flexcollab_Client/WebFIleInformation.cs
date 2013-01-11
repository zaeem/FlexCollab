using System.Text;
using System.Net;
using System.IO;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using WebMeeting.Common;
using System.Threading;
using System.Runtime.InteropServices;
using WebMeeting;
using System.Xml;
using WebMeeting.Client.Alerts;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for WebFIleInformation.
	/// </summary>
	public class WebFIleInformation : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button button2;
		public System.Windows.Forms.Label label2;
		public bool IsDownloadedSuccessfully = false;
		public string localFilePath;
		public bool IsExecute = false;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WebFIleInformation()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WebFIleInformation));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.progressBar1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(16, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(328, 112);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Download Information";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(288, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "label2";
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(8, 72);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(296, 23);
			this.progressBar1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(312, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(184, 128);
			this.button1.Name = "button1";
			this.button1.TabIndex = 1;
			this.button1.Text = "Download";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Enabled = false;
			this.button2.Location = new System.Drawing.Point(264, 128);
			this.button2.Name = "button2";
			this.button2.TabIndex = 1;
			this.button2.Text = "OK";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// WebFIleInformation
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(354, 168);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.button2);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "WebFIleInformation";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Download Web File";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.WebFIleInformation_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	
		private int DownloadFile(String remoteFilename,
			String localFilename)
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
						localStream = File.Create(localFilename);
						progressBar1.Step = (int)response.ContentLength/100;

						// Allocate a 1k buffer
						byte[] buffer = new byte[1024];
						int bytesRead;

						// Simple do/while loop to read from stream until
						// no bytes are returned
						do
						{
							// Read data (up to 1k) from the stream
							bytesRead = remoteStream.Read (buffer, 0, buffer.Length);
							progressBar1.PerformStep();


							// Write the data to the local file
							localStream.Write (buffer, 0, bytesRead);

							// Increment total bytes processed
							bytesProcessed += bytesRead;
						} while (bytesRead > 0);
						progressBar1.Step = 100;
						progressBar1.PerformStep();

						
					}
				}
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
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

			button2.Enabled = true;
			// Return total bytes processed to caller.
			return bytesProcessed;
		}
		
		public WebUploadedFiles fileInformation;
		private Thread downloadThread;
		string _selectedFile;
		public void downloadThreadFunction()
		{
			string strURL = fileInformation.filePath;
			string localpath = "webfile";
			int nIndex2 = strURL.LastIndexOf("/");
			if(nIndex2 != -1)
				localpath = strURL.Substring(nIndex2+1,strURL.Length - nIndex2-1);		
			
			string localPath = _selectedFile;

			IsDownloadedSuccessfully = (DownloadFile(fileInformation.filePath,localpath) > 0);
			localFilePath = localpath;			
			if(!IsDownloadedSuccessfully)
			{
				MeetingAlerts alert=new MeetingAlerts();
				alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Unable to download file from server. Please contact Support",true,false);
				//MessageBox.Show("Unable to download file from server. Please contact Support","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}			
			else if(IsWebPresentation)
			{
				DialogResult = DialogResult.OK;
				Close();
			}


		}	
		private void button1_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			if(dlg.ShowDialog()== DialogResult.OK)
			{
				_selectedFile = dlg.FileName;
				
			}
		    DownloadFile();
		}
		private void DownloadFile()
		{
			downloadThread = new Thread(new ThreadStart(downloadThreadFunction));
			downloadThread.Name = "Download Web File Thread: downloadThreadFunction()";
			downloadThread.Start();
			button1.Enabled = false;
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			if((IsDownloadedSuccessfully) && (IsExecute))
			{
				System.Diagnostics.Process.Start("explorer.exe",Path.GetDirectoryName(localFilePath));
			}
			Close();
		}

		public string _downloadPath;
		public bool IsWebPresentation = false;
		private void WebFIleInformation_Load(object sender, System.EventArgs e)
		{
			_selectedFile = _downloadPath;
			if(IsWebPresentation)
			{
				button1.Enabled = false;
				DownloadFile();
			}
			
		}
	}
}
