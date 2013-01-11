//	11/17/2003
//	Alexander Kent
//	MSHTML-Automation
//	Version 1.1

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using mshtml;
using System.Diagnostics;

namespace WebMeeting.Client.Screen_Capture
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private AxSHDocVw.AxWebBrowser axWebBrowser1;
		private int Task = 1; // global

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.SuspendLayout();
			// 
			// axWebBrowser1
			// 
			this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.axWebBrowser1.Enabled = true;
			this.axWebBrowser1.Location = new System.Drawing.Point(0, 0);
			this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
			this.axWebBrowser1.Size = new System.Drawing.Size(456, 550);
			this.axWebBrowser1.TabIndex = 0;
			this.axWebBrowser1.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.axWebBrowser1_DocumentComplete);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(456, 550);
			this.Controls.Add(this.axWebBrowser1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Microsoft WebBrowser Automation";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.FrmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		
		private void FrmMain_Load(object sender, System.EventArgs e)
		{
			
			try
			{
				object loc = "http://www.compassnav.com/WebMeeting/WebMeeting/Uploads/Pre.gif";
				object null_obj_str = "";
				System.Object null_obj = 0;
				this.axWebBrowser1.Navigate2(ref loc , ref null_obj, ref null_obj, ref null_obj_str, ref null_obj_str);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>frmPicView.cs line==> 109",exp,"Error Opening Page:  " + exp.Message.ToString(),true);			
			
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Opening Page: " + exp.Message.ToString());			
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
			
			}
		}


		private void axWebBrowser1_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			switch(Task)
			{
				case 1:

					HTMLDocument myDoc = new HTMLDocumentClass();
					myDoc = (HTMLDocument) axWebBrowser1.Document;

					// a quick look at the google html source reveals: 
					// <INPUT maxLength="256" size="55" name="q">
					//
					HTMLInputElement otxtSearchBox = (HTMLInputElement) myDoc.all.item("q", 0);

					otxtSearchBox.value = "intel corp";

					// google html source for the I'm Feeling Lucky Button:
					// <INPUT type=submit value="I'm Feeling Lucky" name=btnI>
					//
					HTMLInputElement btnSearch = (HTMLInputElement) myDoc.all.item("btnI", 0);
					btnSearch.click();

					Task++;
					break;

				case 2:

					// continuation of automated tasks...
					break;
			}
		}
	}
}
