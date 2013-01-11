using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WebMeeting.Client.Polling
{
	/// <summary>
	/// Summary description for frm_PollPresenter.
	/// </summary>
	public class frm_PollPresenter : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		
		public WebMeeting.Polling.Polling PollingControl;// = new WebMeeting.Polling.Polling();
		public WebMeeting.Polling.PollResult pollResult ;//= new WebMeeting.Polling.PollResult();
		public WebMeeting.Polling.PollingQuestionDetails askQuestionWindow;
		//public WebMeeting.Common.ClientProfile clientProfile;		




		public frm_PollPresenter()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			try
			{
				PollingControl= new WebMeeting.Polling.Polling();
				pollResult = new WebMeeting.Polling.PollResult();

				PollingControl.pqd = askQuestionWindow;
				PollingControl.pr = pollResult;		
				PollingControl.pr.polling = PollingControl;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==>11501",exp,exp.Message.ToString(),true);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frm_PollPresenter));
			// 
			// frm_PollPresenter
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(658, 520);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frm_PollPresenter";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Presenter Poll";
			this.Closed += new System.EventHandler(this.frm_PollPresenter_Closed);

		}
		#endregion

		private void frm_PollPresenter_Closed(object sender, System.EventArgs e)
		{

			if(ClientUI.getInstance()!=null)
			ClientUI.getInstance().CloseMsg();

		}
	}
}
