using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using WebMeeting.Client;
using WebMeeting.Common;
using System.Reflection;
using System.IO;
using WebMeeting.Client.Alerts;
 
namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for videovoiceControl.
	/// </summary>
	public class videovoiceControl : System.Windows.Forms.UserControl
	{
		public int nUniqueID;
		public bool isVideo=false;
		public ClientProfile associatedProfile;
		private AxShockwaveFlashObjects.AxShockwaveFlash axShockwaveFlash2;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public videovoiceControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}
		~videovoiceControl()
		{
			try
			{
				axShockwaveFlash2.LoadMovie(0,"");
			}
			catch(Exception )
			{

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(videovoiceControl));
			this.axShockwaveFlash2 = new AxShockwaveFlashObjects.AxShockwaveFlash();
			((System.ComponentModel.ISupportInitialize)(this.axShockwaveFlash2)).BeginInit();
			this.SuspendLayout();
			// 
			// axShockwaveFlash2
			// 
			this.axShockwaveFlash2.Enabled = true;
			this.axShockwaveFlash2.Location = new System.Drawing.Point(0, 0);
			this.axShockwaveFlash2.Name = "axShockwaveFlash2";
			this.axShockwaveFlash2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axShockwaveFlash2.OcxState")));
			this.axShockwaveFlash2.Size = new System.Drawing.Size(230, 200);
			this.axShockwaveFlash2.TabIndex = 2;
			this.axShockwaveFlash2.FSCommand += new AxShockwaveFlashObjects._IShockwaveFlashEvents_FSCommandEventHandler(this.axShockwaveFlash2_FSCommand);
			// 
			// videovoiceControl
			// 
			this.Controls.Add(this.axShockwaveFlash2);
			this.Name = "videovoiceControl";
			this.Size = new System.Drawing.Size(231, 202);
			this.Load += new System.EventHandler(this.videovoiceControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.axShockwaveFlash2)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		# region LoadMovie
		public void LoadMovie()
		{	
		//	axShockwaveFlash2.LoadMovie(0,"c:\\WebMeetingVideo.swf");	
			try
			{
				axShockwaveFlash2.LoadMovie(0,Application.StartupPath + "\\" + "WebMeetingVideo.swf");					
			}
			catch(Exception exp)
			{
				
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("videovoiceControl.cs==> LoadMovie() ==> 108",exp,null,false);			
				
			}
		}
		#endregion 

		private void videovoiceControl_Load(object sender, System.EventArgs e)
		{
			
		}

		#region Public Methods 
		#region Audio Functions
		public void ShareMyAudio(string serverIp,string conferenceName,string nUniqueID)
		{
			ServerIP = serverIp;
			Conference = conferenceName;
			AudioToJoin =nUniqueID;
			axShockwaveFlash2.GotoFrame(4);
			//axShockwaveFlash2.Play();
		}
		public void ShareMyAudio()
		{			
			axShockwaveFlash2.GotoFrame(4);
			//axShockwaveFlash2.Play();
		}
		public void SubscribeAudio(string serverIp,string conferenceName,string nUniqueID,string strNameOfPerson)
		{
			axShockwaveFlash2.SetVariable("ListenName",strNameOfPerson);
			ServerIP = serverIp;
			Conference = conferenceName;
			AudioToJoin =nUniqueID;
			axShockwaveFlash2.GotoFrame(3);
		//	axShockwaveFlash2.Play();
		}
		public void SubscribeAudio()
		{		
			axShockwaveFlash2.GotoFrame(3);
	//		axShockwaveFlash2.Play();
		}
		public void DisableSharedAudio()
		{

			axShockwaveFlash2.SetVariable("muteItem","true");
			axShockwaveFlash2.GotoFrame(4);		
			try
			{
				axShockwaveFlash2.LoadMovie(0,"");
			}
			catch(Exception )
			{

			}
			
		}
		public void DisableMyAudio()
		{
			axShockwaveFlash2.SetVariable("disableAudio","true");
			axShockwaveFlash2.GotoFrame(4);	
			try
			{
				axShockwaveFlash2.LoadMovie(0,"");
			}
			catch(Exception )
			{

			}
		}
		#endregion
		#region video
		public void ShareMyCamera()
		{
			axShockwaveFlash2.GotoFrame(2);
			//axShockwaveFlash2.Play();
		}
		public bool CameraFound()
		{
			string strText = axShockwaveFlash2.GetVariable("cameraCount");
			int nCount = Convert.ToInt32(strText);
			if(nCount <0)
			{
				MeetingAlerts alert=new MeetingAlerts();
				alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"No Camera found",true,false);
				//MessageBox.Show("No Camera found","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);				
			}

			return (nCount >=0);

		}
		public void ShareMyCamera(string serverIp,string conferenceName,string nUniqueID)
	   {

			ServerIP = serverIp;
			Conference = conferenceName;
			VideoToJoin =nUniqueID;
			axShockwaveFlash2.GotoFrame(2);
			//axShockwaveFlash2.Play();
		}


		public void SubscribeCamera()
		{
			axShockwaveFlash2.GotoFrame(1);
			//axShockwaveFlash2.Play();
		}

		private void axShockwaveFlash2_FSCommand(object sender, AxShockwaveFlashObjects._IShockwaveFlashEvents_FSCommandEvent e)
		{
			switch(e.command)
			{
				case "BtnOk":
				{
//					ShareMyCamera(Info.getInstance().ServerIP,Info.getInstance().ConferenceID,nUniqueID.ToString());
					NetworkManager.getInstance().profile.videoID = nUniqueID;
					ClientUI.getInstance().sendProfileMessage();
					
					break;
				}
			}
			
				
		}
	
		public void SubscribeCamera(string serverIp,string conferenceName,string nUniqueID)
		{
			ServerIP = serverIp;
			Conference = conferenceName;
			//AudioToJoin =nUniqueID;
			VideoToJoin = nUniqueID;
			axShockwaveFlash2.GotoFrame(1);
			//axShockwaveFlash2.Play();
		}
		#endregion

		#endregion
		#region Properties
		public string AudioToJoin
		{
			set
			{
				axShockwaveFlash2.SetVariable("audioStreamName",value);
			}
			get
			{
				return axShockwaveFlash2.GetVariable("audioStreamName");
			}
	
		}
		public string Conference
		{
			set
			{
				axShockwaveFlash2.SetVariable("conferenceName",value);
			}
			get
			{
				return axShockwaveFlash2.GetVariable("conferenceName");
			}
		}
		public string ServerIP
		{
			set
			{
				//axShockwaveFlash2.SetVariable("serverIP","192.168.0.100");
				//axShockwaveFlash2.SetVariable("serverIP","192.168.0.135");
				axShockwaveFlash2.SetVariable("serverIP","192.168.1.33");
			}
			get
			{
				return axShockwaveFlash2.GetVariable("serverIP");
			}
		}
		public string VideoToJoin
		{
			set
			{
				axShockwaveFlash2.SetVariable("videoStreamName",value);
			}
			get
			{
				return axShockwaveFlash2.GetVariable("videoStreamName");

			}
		}
		#endregion
	}
}
