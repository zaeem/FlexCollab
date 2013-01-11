using System;
using System.Xml;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing;


namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for Info.
	/// </summary>
	public class Info
	{ 
		public static Info getInstance()
		{
			if(mainInstance==null)
			{
				mainInstance=new Info();
				return mainInstance;
			}
			else
				return mainInstance;
		}
		static Info mainInstance;
		#region private variables
		private int width;
		private int height;
		private int connectionStage;
		private bool connected;
		private string websiteName;
		private string serverIP;
		private int connectionPort;
		private string applicationPath;
		private string accountNumber;
		private string userName;
		private string userEmail;
		private string strCompanyName;
		private string conferenceName;
		private string conferenceID;
		private string conferenceAgenda;
		private string conferenceTopic;
		private string ftpIP;
		private string password;
		private Color backcolor;
		private Color chatBackColor;
		private Color participantBackColor;
		private Color screenControlBackColor;
		private string mVoiceIP;
		private int mVoicePort;
		private int mVoiceConferenceID;
		#endregion
		#region getters and setters
		public int VoiceConferenceID
		{
			set
			{
				mVoiceConferenceID = value;
			}
			get
			{
				return mVoiceConferenceID;
			}
		}
		public int VoicePort
		{
			set
			{
				mVoicePort = value;
			}
			get
			{
				return mVoicePort;
			}
		}
		public string VoiceIP
		{
			set
			{
				mVoiceIP = value;
			}	
			get
			{
				return mVoiceIP;
			}
			
		}


		public Color ScreenControlBackColor
		{
			set
			{
				screenControlBackColor=value;
			}
			get
			{
				return screenControlBackColor;
			}
		}
	

		public Color ChatBackColor
		{
			set
			{
				chatBackColor=value;
			}
			get
			{
				return chatBackColor;
			}
		}



		public Color backColor
		{
			set
			{
				backcolor=value;
			}

			get
			{
				return backcolor;
			}
		}
		public int Width
		{
			set
			{
				width = value;
			}
			get
			{
				return width;
			}
		}//end of Width propety
		public int Height
		{
			set
			{
				height = value;
			}
			get
			{
				return height;
			}
		}//end of Height propety
		public int ConnectionStage
		{
			set
			{
				connectionStage = value;
			}
			get
			{
				return connectionStage;
			}
		}//end of connectionStage propety

		public bool Connected
		{
			set
			{
				connected = value;
			}
			get
			{
				return connected;
			}
		}//end of connected propety

		public string WebsiteName
		{
			set
			{
				websiteName = value;
			}
			get
			{
				return websiteName;
			}
		}
		
		public string ServerIP
		{
			set
			{
				serverIP = value;
			}
			get
			{
				return serverIP;
			}
		}//end of serverIP propety
		public int ConnectionPort
		{
			set
			{
				connectionPort = value;
			}
			get
			{
				return connectionPort;
			}
		}//end of connectionPort propety
		public string ApplicationPath
		{
			set
			{
				applicationPath = value;
			}
			get
			{
				return applicationPath;
			}
		}//end of connected propety
		public string AccountNumber
		{
			set
			{
				accountNumber = value;
			}
			get
			{
				return accountNumber;
			}
		}//end of accountNumber propety
		public string UserName
		{
			set
			{
				userName = value;
			}
			get
			{
				return userName;
			}
		}//end of userName propety
		public string UserEmail
		{
			set
			{
				userEmail = value;
			}
			get
			{
				return userEmail;
			}
		}//end of userEmail propety
		public string ConferenceName
		{
			set
			{
				conferenceName = value;
			}
			get
			{
				return conferenceName;
			}
		}//end of connected propety

		public string ConferenceAgenda
		{
			set
			{
				conferenceAgenda=value;
			}
			get
			{
				return conferenceAgenda;
			}

		}
		public string ConferenceTopic
		{
			set
			{
				conferenceTopic=value;
			}
			get
			{
				return conferenceTopic;
			}
		}

		public string ConferenceID
		{
			set
			{
				conferenceID=value;
			}
			get
			{
				return conferenceID;
			}
		}

		public string FtpIP
		{
			set
			{
				ftpIP=value;
			}
			get
			{
				return ftpIP;
			}
		}
		public string Password
		{
			set
			{
				//TODO: change the password to value
				//password="three";
				password="zaeem";
			}
			get
			{
				return password;
			}
		}

		
		#endregion
		public Color ParticipantBackColor
		{
			get
			{
				return participantBackColor;
			}
			set
			{
				participantBackColor=value;
			}
		}
		public string CompanyName
		{
			get
			{
				return strCompanyName;
			}
			set
			{
				strCompanyName = value;
			}

		}

		public Info()
		{
			//
			// TODO: Add constructor logic here
			//
			width=0;
			height=0;
			connectionStage=0;
			connected=false;
			serverIP="127.0.0.1";
			connectionPort=8737;
			applicationPath="";
			accountNumber="1";
			userName="";
			userEmail="abc@hotmail.com";
			conferenceName="TestConference";
			conferenceID="1";
			conferenceTopic="Testing";
			conferenceAgenda="";
			ftpIP="danish";
			password="one";

//			backcolor=System.Drawing.Color.FromArgb(212,202,227);
//			chatBackColor=System.Drawing.Color.FromArgb(212,202,227);
//			participantBackColor=System.Drawing.Color.FromArgb(212,202,227);
//			screenControlBackColor=System.Drawing.Color.FromArgb(212,202,227);


			backcolor=System.Drawing.Color.FromArgb(212,208,200);  // by kamran to change gui gray color
			chatBackColor=System.Drawing.Color.FromArgb(212,208,200);
			participantBackColor=System.Drawing.Color.FromArgb(212,208,200);
			screenControlBackColor=System.Drawing.Color.FromArgb(212,208,200);
			
			readRegistry();
			getConfig();

		}
		private void readRegistry()
		{

			// change it to the installed application path;
			applicationPath=Application.ExecutablePath;
			width=100;
			height=100;
		}
		public void getConfig()
		{
			try
			{
				string configFilePath = Application.StartupPath ;
				configFilePath  += "\\config.xml";
				XmlTextReader reader =  new XmlTextReader(configFilePath  );
				
				while(reader.Read())
				{
					if(reader.NodeType == XmlNodeType.Element)
					{					
							
						if(reader.Name == "ServerIP")
						{
							//info.ServerIP = reader.ReadString();
						}
						else if(reader.Name == "ServerPort")
						{
							//info.ConnectionPort = Convert.ToInt32(reader.ReadString());
						}
						else if(reader.Name == "WebsiteName")
						{
							WebsiteName = reader.ReadString();
						}
/*						else if(reader.Name == "Attendee_Name")
						{
							network.profile.Name = reader.ReadString();								
						}//end if reader.Name == "ServerIP"					
						else if(reader.Name == "FTP_IP")
						{
							info.FtpIP = reader.ReadString();								
						}//end else if reader.Name == "ServerPort"
						else if(reader.Name == "FTP_Username")
						{
							info.UserName = reader.ReadString();								
						}
						else if(reader.Name == "FTP_Password")
						{
							info.Password = reader.ReadString();
						}
						else if(reader.Name == "Image_URL")
						{
							string ImageURL = reader.ReadString();								
						}
*/	
					}//end elementType
				}//end while
				
				reader.Close();
			}
			catch(Exception e)
			{
				//System.Console.WriteLine(e.Message);
				//WebConference.Util.Logger.Error(e.Message,e);
			}
		}//end of the getConfig
	}
}
