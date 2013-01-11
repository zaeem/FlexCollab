using System;
using System.Drawing;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using Batte.CodeProject.Download;   
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace WebMeeting.Client.ManageContent
{
	/// <summary>
	/// Summary description for frmDownloader.
	/// </summary>
	public delegate void DownloadingComplete(object sender,DartbordEventArgs e);	
	public class frmDownloader : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label statusLabel;
		private System.Windows.Forms.Label currentProgressLabel;
		private System.Windows.Forms.Label fileNameLabel;
		public event DownloadingComplete DownComp;
		private Batte.Utilities.Controls.SmoothProgressBar currentProgressBar;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private WaitHandle cancelEvent;
		int completeMistake=0;//for removing two times complete event.
		[DllImport("kernel32.dll")]
		static extern uint GetShortPathName(string lpszLongPath,
			[Out] StringBuilder lpszShortPath, uint cchBuffer);


		#region variables of frmDownloader Class
			string strurldownload="";
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lbldestination;
		private System.Windows.Forms.CheckBox chkDwndCompl;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblfilesize;
			string strurlStore="";
		#endregion

		/// <summary>
		/// The ToShortPathName function retrieves the short path from of a specified long input path
		/// </summary>
		/// <param name="longName">The long name path</param>
		/// <returns>A short name path string</returns>
		/// <remarks>The file must exist for this to work.</remarks>
		public static string ToShortPathName(string longName)
		{
			try
			{
				StringBuilder shortNameBuffer = new StringBuilder(longName.Length );
				//Dim result As New System.Text.StringBuilder(longPath.Length);
				uint bufferSize = (uint)shortNameBuffer.Capacity;

				uint result = GetShortPathName(longName, shortNameBuffer,bufferSize);
				if (result == 0)
				{
					// Failed to convert to a short name. Does the file exist?
				}

				return shortNameBuffer.ToString();
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ManageContents ===>frmDownloader.cs line==> 120",exp,null,false);			
				return null;
			}
		}

		public frmDownloader(string urldownload,string urlstore)
		{
			
			//
			// Required for Windows Form Designer support
			//
			try
			{
				InitializeComponent();

				//
				// TODO: Add any constructor code after InitializeComponent call
				//
				//this.fileNameChanger = new StringDelegate(this.ChangeFileName);
				this.statusChanger = new StringDelegate(this.ChangeStatus);
				this.singlePercentChanger = new IntDelegate(this.ChangeSinglePercent);
				//this.totalPercentChanger = new IntDelegate(this.ChangeTotalPercent);
				//this.activeStateChanger = new BoolsDelegate(this.ChangeActiveState);
			
				//initialize the urls settings.
				this.strurldownload=urldownload; 
				this.strurlStore=urlstore;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Managecontents==>frmDownloader.cs 226",exp,null,false);			

			}
			
			
		}

		public frmDownloader(string urldownload,string urlstore,bool top)
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();

				//
				// TODO: Add any constructor code after InitializeComponent call
				//
				//this.fileNameChanger = new StringDelegate(this.ChangeFileName);
				this.statusChanger = new StringDelegate(this.ChangeStatus);
				this.singlePercentChanger = new IntDelegate(this.ChangeSinglePercent);
				//this.totalPercentChanger = new IntDelegate(this.ChangeTotalPercent);
				//this.activeStateChanger = new BoolsDelegate(this.ChangeActiveState);
			
				//initialize the urls settings.
				this.strurldownload=urldownload; 
				this.strurlStore=urlstore;
				this.TopMost=true;
				this.ShowInTaskbar=false;
				this.chkDwndCompl.Checked=true;
				this.chkDwndCompl.Enabled=false;
				this.btnCancel.Enabled =false; 
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Managecontents==>frmDownloader.cs 178",exp,null,false);			

			}
		
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmDownloader));
			this.statusLabel = new System.Windows.Forms.Label();
			this.currentProgressLabel = new System.Windows.Forms.Label();
			this.fileNameLabel = new System.Windows.Forms.Label();
			this.currentProgressBar = new Batte.Utilities.Controls.SmoothProgressBar();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lbldestination = new System.Windows.Forms.Label();
			this.chkDwndCompl = new System.Windows.Forms.CheckBox();
			this.lblfilesize = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// statusLabel
			// 
			this.statusLabel.Location = new System.Drawing.Point(16, 80);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(104, 16);
			this.statusLabel.TabIndex = 10;
			this.statusLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// currentProgressLabel
			// 
			this.currentProgressLabel.Location = new System.Drawing.Point(280, 80);
			this.currentProgressLabel.Name = "currentProgressLabel";
			this.currentProgressLabel.Size = new System.Drawing.Size(64, 16);
			this.currentProgressLabel.TabIndex = 9;
			this.currentProgressLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// fileNameLabel
			// 
			this.fileNameLabel.Location = new System.Drawing.Point(112, 16);
			this.fileNameLabel.Name = "fileNameLabel";
			this.fileNameLabel.Size = new System.Drawing.Size(256, 16);
			this.fileNameLabel.TabIndex = 8;
			this.fileNameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// currentProgressBar
			// 
			this.currentProgressBar.BackColor = System.Drawing.SystemColors.Control;
			this.currentProgressBar.BarColor = System.Drawing.Color.RoyalBlue;
			this.currentProgressBar.Border = System.Windows.Forms.BorderStyle.FixedSingle;
			this.currentProgressBar.Location = new System.Drawing.Point(16, 96);
			this.currentProgressBar.Maximum = 100;
			this.currentProgressBar.Minimum = 0;
			this.currentProgressBar.Name = "currentProgressBar";
			this.currentProgressBar.Size = new System.Drawing.Size(328, 16);
			this.currentProgressBar.Step = 1;
			this.currentProgressBar.TabIndex = 7;
			this.currentProgressBar.Value = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 11;
			this.label1.Text = "File Name :";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.TabIndex = 12;
			this.label2.Text = "Downloaded to :";
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(152, 144);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(72, 24);
			this.btnCancel.TabIndex = 14;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lbldestination
			// 
			this.lbldestination.AccessibleRole = System.Windows.Forms.AccessibleRole.Caret;
			this.lbldestination.AllowDrop = true;
			this.lbldestination.Location = new System.Drawing.Point(112, 40);
			this.lbldestination.Name = "lbldestination";
			this.lbldestination.Size = new System.Drawing.Size(256, 32);
			this.lbldestination.TabIndex = 16;
			// 
			// chkDwndCompl
			// 
			this.chkDwndCompl.Location = new System.Drawing.Point(16, 120);
			this.chkDwndCompl.Name = "chkDwndCompl";
			this.chkDwndCompl.Size = new System.Drawing.Size(328, 16);
			this.chkDwndCompl.TabIndex = 17;
			this.chkDwndCompl.Text = "Close this dialog box when download completes";
			this.chkDwndCompl.CheckedChanged += new System.EventHandler(this.chkDwndCompl_CheckedChanged);
			// 
			// lblfilesize
			// 
			this.lblfilesize.Location = new System.Drawing.Point(136, 80);
			this.lblfilesize.Name = "lblfilesize";
			this.lblfilesize.Size = new System.Drawing.Size(120, 16);
			this.lblfilesize.TabIndex = 18;
			// 
			// frmDownloader
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.ClientSize = new System.Drawing.Size(376, 191);
			this.Controls.Add(this.lblfilesize);
			this.Controls.Add(this.chkDwndCompl);
			this.Controls.Add(this.lbldestination);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.statusLabel);
			this.Controls.Add(this.currentProgressLabel);
			this.Controls.Add(this.fileNameLabel);
			this.Controls.Add(this.currentProgressBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmDownloader";
			this.Opacity = 0.85;
			this.Text = "Compassnav Downloader";
			this.Load += new System.EventHandler(this.frmDownloader_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void SingleDownload(object data)
		{
			//this.Invoke(this.activeStateChanger, new object[]{true, true});
			
			
			try
			{
				DownloadInstructions instructions = (DownloadInstructions) data;
				//GetFileSize(url, out progressKnown);
				//this.Invoke(this.fileNameChanger, new object[]{Path.GetFileName(instructions.Destination)});
				
					
				using(FileDownloader dL = new FileDownloader())
				{
				
					dL.ProgressChanged += new DownloadProgressHandler(this.SingleProgressChanged);
					dL.StateChanged += new DownloadProgressHandler(this.StateChanged);
					dL.Download(instructions.URLs, instructions.Destination, this.cancelEvent);
												
					
				}
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Managecontents==>frmDownloader.cs 354",exp,null,false);			

			}
		
			//this.Invoke(this.activeStateChanger, new object[]{false, true});
		}

		#region Thread-Safe User Interface Updaters
		
		private delegate void StringDelegate(string value);
		private delegate void IntDelegate(int value);
		private delegate void BoolsDelegate(bool value1, bool value2);

		//private StringDelegate fileNameChanger;
		private StringDelegate statusChanger;
		private IntDelegate singlePercentChanger;
		//private IntDelegate totalPercentChanger;
		//private BoolsDelegate activeStateChanger;

		private void ChangeFileName(string fileName)
		{
			this.fileNameLabel.Text = fileName;
		}

		private void ChangeStatus(string status)
		{
			this.statusLabel.Text = string.Format("Status: {0}", status);
		}

		private void ChangeSinglePercent(int percent)
		{
			try
			{
				this.currentProgressLabel.Text = string.Format("{0}%", percent);
				this.currentProgressBar.Value = percent;
				if(percent==100)
				{
					completeMistake+=1;
					//Trace.WriteLine("Download is completed");	
					if (completeMistake==2)
					{

						this.btnCancel.Enabled=false;
						if(this.chkDwndCompl.Checked ==true)
						{
							this.Close();
							
							DartbordEventArgs dea = new DartbordEventArgs("completed",this.strurlStore);
							DownComp1(dea);		
						}

						//					MessageBox.Show("Completed");
					}
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Managecontents==>frmDownloader.cs 410",exp,null,false);			

			}
	
		}

		protected virtual void DownComp1( DartbordEventArgs e )
		{
	//			if( TripleThrown != null )
	//			{
			DownComp( this, e );
	//			}
		}
		private void ChangeTotalPercent(int percent)
		{
			//this.totalProgressLabel.Text = string.Format("{0}% Complete", percent);
			//this.totalProgressBar.Value = percent;
		}

		private void ChangeActiveState(bool active, bool singleFile)
		{
			//this.singleFileDownloadButton.Enabled =
			//			this.batchDownloadButton.Enabled =
			//			this.batchModeAddURLButton.Enabled =
			//			this.batchModeEditURLButton.Enabled =
			//			this.batchModeRemoveURLButton.Enabled =
			//			this.singleFileDestBrowseButton.Enabled =
			//			this.importURLsButton.Enabled =
			//			this.saveURLsButton.Enabled =
			//			this.batchModeDestBrowseButton.Enabled =
			//			this.ControlBox = !active;

			//this.singleFileCancelButton.Enabled = (singleFile && active);
			//this.batchCancelButton.Enabled = (!singleFile && active);
		}

		private static void ShowErrorMessage(Exception ex)
		{
//			MessageBox.Show(
//				string.Format(
//				"An exception of type: {0} has occurred.\n" +
//				"Message: {1}",
//				ex.GetType(),
//				ex.Message),
//				"Error");
		}
		#endregion


		#region Downloader events
					
		

		private void SingleProgressChanged(object sender, DownloadEventArgs e)
		{
			this.Invoke(this.singlePercentChanger, new object[]{e.PercentDone});
		}
		private void StateChanged(object sender, DownloadEventArgs e)
		{
			this.Invoke(this.statusChanger, new object[]{e.DownloadState});
		}
			#endregion

		private void frmDownloader_Load(object sender, System.EventArgs e)
		{
			try
			{
				//bool progressBar;
				//long size;
				this.cancelEvent = new AutoResetEvent(false);
				//fileNameLabel.text=System.IO.Path.Get
				//			if(progressBar)
				//			{
				//				this.lblfilesize.Text =size.ToString();			
				//			}
				this.fileNameLabel.Text = System.IO.Path.GetFileName(this.strurldownload); 
				this.lbldestination.Text=this.strurlStore; 

				//			System.Windows.Forms.MessageBox.Show(ToShortPathName(this.strurlStore));
				DownloadInstructions instructions = new DownloadInstructions
					(this.strurldownload,this.strurlStore);
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.SingleDownload), instructions);
			}
				
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Managecontents==>frmDownloader.cs 500",exp,null,false);			
			}
		
	
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			((AutoResetEvent)this.cancelEvent).Set();
			this.Close();
		}

		private void chkDwndCompl_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

		#region ThreadPool Message Objects
		private class DownloadInstructions
		{
			private string urls;
			private string destination;

			public DownloadInstructions(string urls, string destination)
			{
				this.urls = urls;
				this.destination = destination;
			}

			/// <summary>
			/// shallow copy
			/// </summary>
			public string URLs
			{
				get
				{
					return  this.urls;
				}
			}

			public string Destination
			{
				get
				{
					return this.destination;
				}
			}
		}
		#endregion
	}
	public class  DartbordEventArgs  : EventArgs
	{
		private readonly string  strStatus;
		private readonly string strUploadingPath;

		
		//Constructor
		public DartbordEventArgs( string  strStatus1,string strUploadingPath1)
		{
			this.strStatus=strStatus1;
			this.strUploadingPath=strUploadingPath1;
		}

		#region Properties
		public string  CurrentStatus
		{
			get
			{
				return strStatus;
			}
		}
		public string  UploadPath
		{
			get
			{
				return this.strUploadingPath;
			}
		}

		//		public string  ScoreText 
		//		{
		//			get
		//			{
		//				return mstrScore;
		//			}
		//		}

		//		public int  Throw 
		//		{
		//			get
		//			{
		//				return mintThrow;
		//			}
		//		}
		#endregion

	}
}
