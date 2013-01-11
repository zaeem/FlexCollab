using System;
using System.Drawing;
using System.Windows.Forms;
using WMEncoderLib;
using WMPREVIEWLib;
using WMDEVICECONTROLLib;
using Microsoft.DirectX;
using Microsoft.DirectX.AudioVideoPlayback;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace ScreenCapture
{
	/// <summary>
	/// Summary description for ScreenCapture.
	/// </summary>
	
	public class ScreenCapture
	{
		public static string Flag_configpro="Screen Video/Audio Medium (CBR)";
		
		// WMEncoder object
		public WMEncoder					m_encoder;
		// Source group collection 
		private IWMEncSourceGroupCollection m_sourceGroupCollection;
		// source group
		private IWMEncSourceGroup			m_srcGrp;
		// encoder source
		private IWMEncSource				m_audSource;
		// screen source
		private IWMEncVideoSource2			m_screenSource;
		// output file
		private IWMEncFile					m_encFile;
		// profile collection
		private IWMEncProfileCollection		m_profileCollection;
		// profile
		private IWMEncProfile				m_profile;
		// source enumerator dialog!!! 
		private SourceEnum					m_sourceEnumDlg;
		// audio source
		private string						m_szAudioSource;
		// timer
		private System.Windows.Forms.Timer	m_timer;
		// whether timer is enabled or not
		private bool						m_bTimerEnabled = false;
		// just a flag for encoder state
		private bool						m_bFirstTime	= true;


		private Video ourVideo = null;
		private Audio ourAudio = null;
		public WebMeeting.Client.Screen_Capture.frm_Rec frm_Rec;
		public Config config;
 
		#region initialize encoder	
		public void InitializeEncoder()
		{
			try 
			{
                frm_Rec=new WebMeeting.Client.Screen_Capture.frm_Rec();
				config=new Config();
				config.cfgFile = "WebMeeting.exe.config"; 

				// create encoder object
				m_encoder = new WMEncoder();
				m_encoder.DisplayInfo.Author="Zaeem";
				m_encoder.DisplayInfo.Copyright="Uraan Software Solutions";
				m_encoder.DisplayInfo.Description="Uraan Generated Media File for Screen Recording";
				m_encoder.DisplayInfo.Title="Recorded Screen";
				m_encoder.DisplayInfo.Title="Recorded Screen";
				m_encoder.DisplayInfo.Rating="1";
				m_encoder.DisplayInfo.Description="This file is Generated through CampusNav developed by Uraan Software Solution";
				


				// retrive source group collection
				m_sourceGroupCollection = m_encoder.SourceGroupCollection;

				// add a source group to collection
				m_srcGrp = m_sourceGroupCollection.Add("SG_1");

				m_sourceEnumDlg= new SourceEnum();

				// add a screen source
				if(EnumerateAudioSources())
				{
					m_audSource = m_srcGrp.AddSource(WMENC_SOURCE_TYPE.WMENC_AUDIO);
					m_audSource.SetInput(m_szAudioSource, "Device", "");
				}

				// set screen as source
				m_screenSource = (IWMEncVideoSource2)m_srcGrp.AddSource(WMENC_SOURCE_TYPE.WMENC_VIDEO);
				m_screenSource.SetInput("ScreenCapture1", "ScreenCap", "");
				
			}
			catch(Exception  exp)
			{
				
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>Screen Capture.cs line==> 99",exp,"Error Loading Encoder: " + exp.Message.ToString(),false);			
				if( MessageBox.Show("Either You do not have required components of Windows Media Encoder or they are corrupted on your Machine: "+ /*+exp.Message.ToString() + */". Do you want to download now?","WebMeeting",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.Yes)
				{
					Win32.Shell32.ShellExecute(0,"Open",WebMeeting.Client.Info.getInstance().WebsiteName + "/wmencoder.exe","","",1);				
				}
				
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Stoping encoder: " + exp.Message.ToString());			
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
			
			}
		}

		#endregion

	
		#region Enumerate audio sources
		public bool EnumerateAudioSources()
		{
			try 
			{
				WMEncoder Encoder = new WMEncoder();

				// Retrieve source and device plug-in info manager objects from WMEncoder.
				IWMEncSourcePluginInfoManager SrcPlugMgr = Encoder.SourcePluginInfoManager;
				IWMEncDeviceControlPluginInfoManager DCPlugMgr = Encoder.DeviceControlPluginInfoManager;

				// Loop through all the audio and video devices on the system.
				IWMEncPluginInfo PlugInfo;
				for (int i = 0; i < SrcPlugMgr.Count; i++)
				{
					// Set the IWMEncPluginInfo object to the current plug-in.
					PlugInfo = SrcPlugMgr.Item(i);

					// Find the device plug-ins that support resources.
					if (PlugInfo.SchemeType == "DEVICE" && PlugInfo.Resources == true)
					{
						// Loop through the resources in the current plug-in.
						for (int j = 0; j<PlugInfo.Count; j++)
						{
						
							//if(frm_Rec.chkAudio.Checked==true)

							{
							// Add audio resources to the cboAudioSource combo box.
							if (PlugInfo.MediaType == WMENC_SOURCE_TYPE.WMENC_AUDIO)
								m_sourceEnumDlg.audioSources.Items.Add(PlugInfo.Item(j));
															
							}
						}	
					}	
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>Screen Capture.cs line==> 147",exp,"Error Enumeration Audio Resources: " + exp.Message.ToString(),true);			
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error Stoping encoder: " + exp.Message.ToString());			
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
			}

			//DialogResult ret = m_sourceEnumDlg.ShowDialog();
			//if( ret == DialogResult.Cancel)
			//	return false;

			//m_szAudioSource = m_sourceEnumDlg.selectedItem;
		
			m_szAudioSource = config.Read("Driver");
		
			return true;

		}

		#endregion

		#region set file as output
		public void SetFileAsOutput(string szFileName)
		{
			try 
			{
				m_encFile = m_encoder.File;
				m_encFile.LocalFileName =szFileName;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>Screen Capture.cs line==> 175",exp,"Error Setting Audio File: " + exp.Message.ToString(),true);			
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error setting file" + exp.ToString());
			}
		}

		#endregion 

		#region set profile
		public void SetProfile()
		{
			m_profileCollection = m_encoder.ProfileCollection;
			for(int i = 0; i < m_profileCollection.Count; i++)	
			{
				m_profile = m_profileCollection.Item(i);

				if( m_profile.Name == Flag_configpro)	
				{
					m_srcGrp.set_Profile(m_profile);
					break;
				}
		
			}
			
		}

		#endregion
	
		#region  start encoder
		public void StartEncoder()
		{
			try 
			{
				
				m_encoder.Start();	
			
				if( m_bFirstTime == true )
				{
					if( m_bTimerEnabled == true )
					{
						m_timer.Enabled = true;
						m_encoder.OnStateChange +=
							new _IWMEncoderEvents_OnStateChangeEventHandler(OnStateChanged);

					}
				}
				else
				{
					m_timer.Enabled = true;
				}
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>Screen Capture.cs line==> 229",exp,"Error Starting Encoder: " + exp.Message.ToString(),true);			
				
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error starting encoder: " + exp.Message.ToString());

			}
		}

		#endregion
	
		#region  pause encoder
		public void PauseEncoder()
		{
			m_encoder.Pause();
			m_timer.Enabled = false;
		}

		#endregion
	
		#region  stop encoder
		public void StopEncoder()
		{
			m_encoder.Stop();
			m_timer.Enabled = false;
		}

		#endregion
	
		#region  input file name and timer for retrieving stats
		public ScreenCapture(string fileName,ref System.Windows.Forms.Timer timer)
		{
			m_timer = timer;
			m_timer.Enabled = false;
			
			// enable the flag that we are using a timer. If we do not use this 
			// no stats will be provided
			m_bTimerEnabled = true;

			InitializeEncoder();
			SetProfile();
			SetFileAsOutput(fileName);
		}

		#endregion

		#region input file name
		public ScreenCapture(string fileName)
		{
			InitializeEncoder();
			SetProfile();
			SetFileAsOutput(fileName);
			
		}

		#endregion

        #region default constructor
		public ScreenCapture()
		{
			// do nothing. 			
		}

		#endregion

		#region Play back (Not in use)
		public void Playback(string fileName, System.Windows.Forms.PictureBox panel)
		{
			ourVideo = new Video(fileName);

			ourVideo.Ending += new System.EventHandler(this.ClipEnded);

			// get bounds values
			int x = panel.Bounds.X;
			int y = panel.Bounds.Y;
			int height = panel.Bounds.Height;
			int width = panel.Bounds.Width;			

			ourVideo.Owner = panel;

			// we have to set bounds again coz when playing the video, it increases the 
			// size of the control for "some reason"..!!
			panel.SetBounds(x, y, width, height);
			
			// Start playing now
			ourVideo.Play();
		}

		#endregion
		
		#region Stop and Playback (Not In Use)
		public void StopPlayback()
		{
			if (ourVideo != null)
				ourVideo.Stop();
			else
			{
				if (ourAudio != null)
					ourAudio.Stop();
			}
		}

		#endregion

		#region Pause and Play Back
		public void PausePlayback()
		{
			if( ourVideo != null)
			{
				if( ourVideo.Paused == true)
				{
					ourVideo.Play();
				}
				else
				{
					ourVideo.Pause();
				}
			}
			else
			{
				if( ourAudio!=null)
				{
					if( ourAudio.Paused == true)
					{
						ourAudio.Play();
					}
					else 
					{
						ourAudio.Pause();
					}
				}
			}
			/*
			if (ourVideo != null)
				ourVideo.Pause();
			else
			{
				if (ourAudio != null)
					ourAudio.Pause();
			}
			*/
		}

		#endregion

		#region Full Screen Mode
		public void FullScreen()
		{
			if (ourVideo != null)
				ourVideo.Fullscreen = !ourVideo.Fullscreen;
		}

		#endregion

		#region Clip Ended (Not in Use)
		private void ClipEnded(object sender, System.EventArgs e)
		{
			// The clip has ended, stop and restart it
			if (ourVideo != null)
			{
				ourVideo.Stop();
				ourVideo.Play();
			}
			else
			{
				if (ourAudio != null)
				{
					ourAudio.Stop();
					ourAudio.Play();
				}
			}
		}

		#endregion

		#region OnStateChanged Event is controlled
		protected void OnStateChanged(WMENC_ENCODER_STATE enumState)
		{
			try 
			{
				if( WMENC_ENCODER_STATE.WMENC_ENCODER_STOPPED == enumState)
				{
					// Stop the event handler when the process has finished.
					m_encoder.OnStateChange -= new _IWMEncoderEvents_OnStateChangeEventHandler(OnStateChanged);
				}
			}
			catch( Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Video Recordong Module ===>Screen Capture.cs line==> 416",exp,"Error Cahnging Encoder State: " + exp.Message.ToString(),true);			
				//Trace.WriteLine(exp.ToString()+"---"+exp.Message+"---"+exp.Source+exp.StackTrace+"---"+exp.TargetSite+"---"+exp.InnerException);		
				//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Error starting encoder: " + exp.Message.ToString());
			}
		}
		#endregion
	}
}
