using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using MySQL;
using Belikov.GenuineChannels;
using Belikov.GenuineChannels.BroadcastEngine;
using Belikov.GenuineChannels.DotNetRemotingLayer;
using Belikov.GenuineChannels.Logbook;
using WebMeeting.Common;
using System.Runtime.InteropServices;

//using WebMeeting.WebIntegeration;
using System.Xml;
using Microsoft.Win32;
using System.Drawing;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

using System.Threading;


namespace WebMeeting.Server
{
	/// <summary>
	/// Chat server implements server that configures Genuine Server TCP Channel and implements
	/// chat server behavior.
	/// </summary>
	class ConferenceServer : MarshalByRefObject, IConferenceServer
	{
		
		
		[DllImport("kernel32.dll")]
		public static extern bool SetProcessWorkingSetSize( IntPtr proc, int min, int max );
		
		/// <summary>
		/// This class is used in addFlowMsgs method of Class conferenceRoom
		/// for logging messages of tpye DocumentSharing,WhiteboardMessage,MsgWebBrowseMessage
		/// ,IMChatMessage,StateUpdateMessage,AppSharingMessageEx,QAQuestion and QAAnswer
		/// </summary>
		public class ST_flowMsg
		{
			public string meetID;
			public string curDate;
			public string curMsg;
			public string MsgCategory;
		}
		
		/// <summary>
		/// This class is used in addFlowMsgs2 method of Class conferenceRoom
		/// for logging messages of tpye  PollAnswerMessage only
		/// </summary>
		public class ST_LogPolling
		{
			public string meetID;
			public string curDate;
			public string question;
			public string answer;
			public string userid; 
			public string username;
			public string qtype; //question type
			public string mchoice;
		}


		public enum ConferenceVoiceEnum
		{
			Unused,
			Used
		}	
		
	
		#region "Configuration relevant to config.xml"		
		/// <summary>
		/// Document Sharing & Uploading Path & conversion process
		/// </summary>
		public string WEBSERVER_DOCUMENTSHARING_UPLOAD_PATH = "c:\\inetput\\wwwroot";
		/// <summary>
		/// Video Audio settings
		/// </summary>
		public string voiceServerIP;
		public string voiceServerPort;
		/// <summary>
		/// Database Server settings
		/// </summary>
		private string databaseServer;
		private string databaseUsername;
		private string databasePassword;
		/// <summary>
		/// Ftp Settings
		/// FTP_IP---ipaddress of ftp server
		/// FTP_Username---Username of the ftp server
		/// FTP_PORT---port of ftp server
		/// FTP_Password---password of the ftp server
		/// </summary>
		private string FTP_IP;
		private string FTP_Username;
		private string FTP_PORT;

		private string FTP_Password;
		
		/// <summary>
		/// Following setting are not using anywhere in the program
		/// or commented in the review of certain module.For more information
		/// check  2-Configuration.doc
		/// All variable & method dependencies are provided there
		/// </summary>
		private string IMAGE_URL;
		public string WEBSERVER_PRESENTATION_PATH = "http://localhost/ctool/public/presentations/";
		public string WEBSERVER_WEBFILES_PATH = "http://localhost/ctool/public/uploads/";		
		public string WEBSERVER_DOCUMENTSHARING_DOWNLOAD_PATH = @"C:\Inetpub\wwwroot\documentsharing";
		public string WEBSERVER_DOCUMENTSHARING_DOWNLOAD_URL = "http://localhost/documentsharing/";

		#endregion
	
		ConferenceList RunningConferenceList=new ConferenceList();
		public MySqlWrapper dbCon;

		
		private Thread insertFlowThread;	

		#region Commented Variables
		//public Thread billingThread;
		//public ConferenceVoiceEnum[] mconferenceVoices = new ConferenceVoiceEnum[5000];
		//public Hashtable tempClientList ;
		#endregion


		

		//public ArrayList flowMsgs = new ArrayList();
		
		
		
		static public ConferenceServer staticServerInstance;
				
		public static ConferenceRoom GlobalRoom = new ConferenceRoom();
	

		
		#region IConferenceServer Implementation
		/// <summary>
		/// This method is the implementation of IConferenceServer.
		/// This methos is called from client side to connect to a particular meeting.
		/// This methods do the following things:
		/// -Search Conference from running conferences on this server--if found return conference room object
		/// otherwise search db  & make new object of conferenceroom and return that
		/// -Create new AuthenticationMessage with profile.ClientId that is passed to JoinConference method.
		/// -Check conf==null then throw exception that specified conference id not found.
		/// -check conference capacity by executing query to mysql and throw exception in the case when capacity is full
		/// -sleep thread 1 seconds.
		/// -validate password of the meeting....its just empty function return true in all casees
		/// -Always else condition of if(!ValidateAuthentication(Password,"" + conferenceID)) statement run
		///  --check ctool_guest if IsGuest=true, if IsGuest=false then check ctool_company_member table
		///  --Assign Msg object different values like ftp_ip,username & password etc
		///  --check query result that is run again IsGuest true/false, assign different values to profile & msg object
		/// -check duplicate profile name..if found throw exception of duplicate user
		/// -this condition will not run because on previous statement we throw exception on same name
		///  if(cp.ClientRegistrationId == profile.ClientRegistrationId) // client exist in this meeting 
		/// -conf.AttachClient(profile.Name,profile,Msg); will add the profile uri to dispatcher list so that 
		///  user can get the message when some message is broadcast.
		///  -Add message join messages
		///  -insert profile.cleintid in the ctool_billing with billing_mtype=1 with quest and billing_mtype=0 for non-guest.
		///  -Select billing id against insertion and asssign it into the profile.billingid which further used in the billing consumer function named 
		///  public void UpdateBillingInformation()
		///  -return conferenceroom object.
		/// </summary>
		/// <param name="conferenceID"></param>
		/// <param name="profile"></param>
		/// <param name="Msg"></param>
		/// <param name="Password"></param>
		/// <param name="IsGuest"></param>
		/// <returns></returns>
		public IConferenceRoom JoinConference(int conferenceID,ref WebMeeting.Common.ClientProfile profile,out AuthenticationMessage Msg, string Password,bool IsGuest)
		{


				Console.WriteLine(" On Join Called \n Going inside Search Conference");
				ConferenceRoom      conf;
				
				//
				// Find the conference
				//			
				conf = SearchConference(conferenceID.ToString());
				Msg = new AuthenticationMessage(profile.ClientId,true);
				//try
				//{
			
				//  
				// If conf not found, raise an error
				//
				if(conf == null)
				{
					throw (new System.ArgumentException("No conference with specified Id found hosted on this server", conferenceID.ToString()));
				}
				string sql = "select meeting_capacity from ctool_meeting where meeting_id = " + conf.ConferenceId;			
				ArrayList Recordset = dbCon.Execute(sql);
				//Console.WriteLine(sql + "\n Execute. Recordset Count = " + Recordset.Count.ToString());
				if(Recordset.Count < 1)
				{
					throw (new System.ArgumentException("No conference with specified Id found hosted on this server", conferenceID.ToString() ));				
				}										
				ArrayList Record = (ArrayList)Recordset[0];
				int nTotal = Convert.ToInt32(Record[0]);
				if((conf.ClientList.Count + 1) > (nTotal+1))
				{
					throw (new System.ArgumentException("No More users can join this conference. Conference Capacity reached", "Max" + conferenceID.ToString()));
				}				
				//this sleep code should be removed as it slow the connection speed of the application.
				System.Threading.Thread.Sleep(1000);
				//ValidateAuthentication(Password,"" + conferenceID)--this method does nothing,its always send
				//true
				/*
				if(!ValidateAuthentication(Password,"" + conferenceID))
				{
					Console.WriteLine("\nUser not validated ");
					throw (new System.ArgumentException("Invalid Meeting Password", Password));
				}
				else
				*/
				{
					string sql2= "Select company_member_fname,company_member_email,company_acc_name From ctool_company_member,ctool_company_acc where company_acc_id = company_member_cid AND company_member_id = " + profile.ClientRegistrationId;				
					if(IsGuest)
					{
						sql2 = "Select * from ctool_guest where guest_id = " +  profile.ClientRegistrationId;				
					}
					profile.IsGuest = IsGuest;
					ArrayList recordSet = dbCon.Execute(sql2);
					if(recordSet.Count < 1)
					{
						throw (new System.ArgumentException("No user exists with specified username","ClientRegistrationId"));
						
					}
					else
					{

						Msg.ConferenceName = conf.conferenceName;				
						Msg.FTPPassoword = this.FTP_Password;
						Msg.FTPUsername = this.FTP_Username;
						Msg.FTPPort = this.FTP_PORT;
						Msg.FTP_IP = this.FTP_IP;
						Msg.ImageURL = GetImage("" + conferenceID);		
						Msg.SenderID=-1;
						Msg.VoiceIP = voiceServerIP;
						Msg.VoicePort = voiceServerPort;	
						Msg.voiceConferenceId = conf.conferenceVoiceSessionID;
						if(recordSet.Count > 0)
						{
							if(IsGuest)
							{
								ArrayList record = (ArrayList)recordSet[0];
								string strFName =record[3].ToString();
								profile.Name = strFName;
								profile.EMailAddress = record[4].ToString().ToLower();											
								Msg.companyName = "Guest Account";
								Msg.ClientName = strFName;
								Msg.ClientEmail = profile.EMailAddress;//record[1].ToString().ToLower();										
								Msg.SenderID=-1;
							}
							else
							{
								ArrayList record = (ArrayList)recordSet[0];
								string strFName =record[0].ToString();
								profile.Name = strFName;
								profile.EMailAddress = record[1].ToString().ToLower();
								profile.Name = strFName;						
								Msg.companyName =record[2].ToString();
								Msg.ClientName = strFName;
								Msg.ClientEmail = record[1].ToString().ToLower();										
								Msg.SenderID=-1;
							}
						}
					}
				}	
				Console.WriteLine("profile name :::"+profile.Name);		



				foreach(ClientProfile cp in conf.ClientList.Values)
				{
					Console.WriteLine("client List name :::"+cp.Name.Trim());
					// To avoid the duplicate name 
					if(cp.Name.Trim()==profile.Name.Trim())
					{
						Console.WriteLine("Exception fired of login duplicate");
						throw (new System.ArgumentException("A user is already login with name.", profile.Name.Trim()));
					}
					//
					if(cp.ClientRegistrationId == profile.ClientRegistrationId) // client exist in this meeting 
					{

						// zaeem Vierw thios condition doesnt seem to be working ok
						// Wil see that later
						conf.checkConnectionUrgent(cp.ClientId );
						//break;
					}
				}			
				Console.WriteLine("Meeting Name " + Msg.ConferenceName);

				Console.WriteLine("\n Calling AttachClient");
				conf.AttachClient(profile.Name,profile,Msg);
				Console.WriteLine("\n Attach Client Called");

				GenuineUtility.CurrentSession["Profile"] = profile;
			
				//******************** Logging
				ST_flowMsg FlowMsgObj = new  ST_flowMsg ();
				FlowMsgObj.meetID = conf.ConferenceId ;
				FlowMsgObj.curDate= DateTime.Now.Year + "-" + DateTime.Now.Month +"-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
				FlowMsgObj.curMsg = profile.Name + " has joined the "+ conf.conferenceName +" conference ." ;
				FlowMsgObj.MsgCategory = "Attendee Joins and Leaves";
				//Zaeem Removed logging
				//flowMsgs.Add(FlowMsgObj);
			
				FlowMsgObj = null;			
				return conf;
			/*
			}

			catch(Exception exp)
			{
			
				
				Console.WriteLine("******** Exception in Join conference **********");
				Console.WriteLine(exp.Message.ToString());
				Console.WriteLine(exp.StackTrace.ToString());
				Console.WriteLine("********End Exception in Join conference **********");
				return conf;
			
			}
			*/
		}

		#endregion

		
		#region "Thread Functions"
		/*
		private void insertFlowMessages()
		{
			try
			{
				string sql;
				while(true)
				{
					if(flowMsgs.Count > 0)
					{
						try
						{
							sql="";
							if(flowMsgs[0].GetType().Equals(typeof(ST_flowMsg)) )
							{								
								sql="Insert into ctool_log (log_meeting_id,log_stamp,log_log,log_category )";
								sql = sql + "values (" + ((ST_flowMsg)flowMsgs[0]).meetID +",'"+ ((ST_flowMsg)flowMsgs[0]).curDate  +"','" + ((ST_flowMsg)flowMsgs[0]).curMsg +"','" + ((ST_flowMsg)flowMsgs[0]).MsgCategory +"')"; 
								//sql = "Insert into ctool_log (log_meeting_id,log_stamp,log_log)values (6,'2005-12-28 03:03:03','it is new heere')";
							}
							else if(flowMsgs[0].GetType().Equals(typeof(ST_LogPolling)) )
							{
								sql="Insert into ctool_logpolling (meetingid,stamp,question, answer, userid, username, qtype, mchoice )";
								sql = sql + "values (" + ((ST_LogPolling)flowMsgs[0]).meetID +",'"+ ((ST_LogPolling)flowMsgs[0]).curDate  +"','" +((ST_LogPolling)flowMsgs[0]).question+"','" + ((ST_LogPolling)flowMsgs[0]).answer+"'," +((ST_LogPolling)flowMsgs[0]).userid +",'" + ((ST_LogPolling)flowMsgs[0]).username+"','" +((ST_LogPolling)flowMsgs[0]).qtype+"','" + ((ST_LogPolling)flowMsgs[0]).mchoice +"')"; 
							}
							dbCon.Execute(sql);
							flowMsgs.RemoveAt(0); 
						}
						catch(Exception ex)
						{
							ex=ex;
						}
						continue;
					}
					Thread.Sleep(500);
				}

			}
			catch(Exception ex)
			{
				ex=ex;
			}
		
		}
		*/
		
		#endregion

		/// <summary>
		/// This function is commented because there is no  functionailty of this in our current application
		/// </summary>
		/// <returns></returns>
//		public int getConferenceVoiceID()
//		{
//			for(int i= 1 ; i < 5000 ; i++)
//			{
//				if(mconferenceVoices[i] == ConferenceVoiceEnum.Unused)
//				{
//					mconferenceVoices[i]= ConferenceVoiceEnum.Used;
//					return i;
//				}
//			}
//			return -1;
//		}		
		

		
		#region public ConferenceServer Commented Code

		//		billingThread = new Thread(new ThreadStart(UpdateBillingInformation));
		//		billingThread.Name = "UpdateBillingInformation Thread : ConferenceServer()";
		//		billingThread.Start();
			
		#endregion

		/// <summary>
		/// this Constructor do the following things
		/// 1-Set Configurations
		/// 2-Connect to db
		/// 3-Start Logging Thread
		/// 4-Start thFlushMemory Thread
		/// </summary>
		public ConferenceServer()
		{
			/*
			 * loading configuration files
			 * */
			getConfig();
			dbCon=new MySqlWrapper();
			dbCon.Connect(databaseServer,databaseUsername,databasePassword);
			dbCon.SelectDB("ctool");
			staticServerInstance = this;
			#if TESTING
			Console.Write(databaseServer + " " + databaseUsername + " "+ databasePassword);
			#endif

//			insertFlowThread = new Thread(new ThreadStart(insertFlowMessages));
//			insertFlowThread.Name = "insertFlowThread Thread : ConferenceServer()";
//			insertFlowThread.Start();
//			
			// thread free all extra resorces after specific time   Kamran
			Thread thFlushMemory = new Thread(new ThreadStart(overallFlushMemory)); //per2
			thFlushMemory.Name = "overallFlushMemory Thread : ConferenceServer";//per2
			thFlushMemory.Start();//per2

				
		}
      
		/// <summary>
		/// This method is used to load configuration settings
		/// in difffent variables of this class relevant to ftp
		/// ,audio/video,database server & document sharing.This method
		/// used config.xml to load configuration settings
		/// </summary>
		public void getConfig()
		{
			try
			{
				XmlTextReader reader =  new XmlTextReader(Application.StartupPath +"/config.xml");
			
				while(reader.Read())
				{
					if(reader.NodeType == XmlNodeType.Element)
					{
					
						if(reader.Name == "Presentatin_Path")
						{
							WEBSERVER_PRESENTATION_PATH = reader.ReadString();
						}//end if reader.Name == "ServerIP"
						else if(reader.Name == "voiceserverAddress")
						{
							voiceServerIP = reader.ReadString();
						}
						else if(reader.Name == "voiceserverPort")
						{
							voiceServerPort = reader.ReadString();
						}
						else if(reader.Name == "WEBSERVER_DOCUMENTSHARING_UPLOAD_PATH")
						{
							WEBSERVER_DOCUMENTSHARING_UPLOAD_PATH = reader.ReadString();
						}
						else if(reader.Name == "WEBSERVER_DOCUMENTSHARING_DOWNLOAD_PATH")
						{
							WEBSERVER_DOCUMENTSHARING_DOWNLOAD_PATH = reader.ReadString();
						}
						else if(reader.Name == "WEBSERVER_DOCUMENTSHARING_DOWNLOAD_URL")
						{
							WEBSERVER_DOCUMENTSHARING_DOWNLOAD_URL = reader.ReadString();
						}
						else if(reader.Name == "WebFiles_Path")
						{
							WEBSERVER_WEBFILES_PATH = reader.ReadString();
						}//end else if reader.Name == "ServerPort"
					
						else if(reader.Name == "databaseserver")
						{
							databaseServer = reader.ReadString();
						}//end else if reader.Name == "FTPIP"
						else if(reader.Name == "databaseusername")
						{
							databaseUsername = reader.ReadString();
						}
						else if(reader.Name == "databasepassword")
						{
							databasePassword = reader.ReadString();
						}																									
						else if(reader.Name == "FTP_IP")
						{
							this.FTP_IP= reader.ReadString();								
						}//end else if reader.Name == "ServerPort"
						else if(reader.Name == "FTP_Username")
						{
							this.FTP_Username = reader.ReadString();								
						}
						else if(reader.Name == "FTP_Password")
						{
							this.FTP_Password = reader.ReadString();
						}
						else if(reader.Name == "Image_URL")
						{
							this.IMAGE_URL = reader.ReadString();								
						}
						else if(reader.Name == "FTP_Port")
						{
							this.FTP_PORT = reader.ReadString();
						}	
					}//end if reader.Name == "ServerIP"
				}//end while
			
				reader.Close();
				reader = null; //GC
			}
			catch(Exception ee)
			{
				System.Console.WriteLine(ee.Message);
				//MessageBox.Show(ee.Message);
				//System.Console.WriteLine(e.Message);
				//WebConference.Util.Logger.Error(e.Message,e);
			}
		}//end of the getConfig

		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// 

		[MTAThread]
		static void Main(string[] args)
		{
			try
			{
				// setup .NET remoting
				System.Configuration.ConfigurationSettings.GetConfig("DNS");
				/*
				 * Belikov.GenuineChannels.DotNetRemotingLayer.GenuineGlobalEventProvider provides the global
				 * GenuineChannelsGlobalEvent event fired for each Genuine Channels event. You can attach a
				 * handler to it if you want to process events generated by all channels and services.
				 */
				GenuineGlobalEventProvider.GenuineChannelsGlobalEvent += new GenuineChannelsGlobalEventHandler(GenuineChannelsEventHandler);
				//GlobalLoggerContainer.Logger = new BinaryLog(@"c:\tmp\server.log", false);
				//Belikov.GenuineChannels.Logbook.GenuineLoggingServices.SetUpLoggingToFile("D:\\GenuineChannelsLog\\server.genchlog",GenuineLoggingServices.DefaultLoggingOptions);			
				RemotingConfiguration.Configure("Server.exe.config");
				// bind the server
				ConferenceServer server = new ConferenceServer();
				RemotingServices.Marshal(server, "ChatServer.rem");
				
				#if TESTING
				Console.WriteLine("Server has been started. Press enter to exit.");
				#endif

				for(;;)
                    Console.ReadLine();

				//server.billingThread.Abort();

				
			}
			catch(Exception ex)
			{
				Console.WriteLine("Exception: {0}. Stack trace: {1}.", ex.Message, ex.StackTrace);
			}
		}


		
		public void FlushMemory() 
		{
			GC.Collect() ;
			GC.WaitForPendingFinalizers() ;
			if(Environment.OSVersion.Platform == PlatformID.Win32NT) 
			{
				SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, 40000000, 80000000) ;
			}
		}

		public void overallFlushMemory()//per2
		{
			try
			{
				while(true)
				{
					if(Environment.WorkingSet > 80000000)// maximum 15 mb
					{
						FlushMemory();
					}
					Thread.Sleep(10000);
				}
			}
			catch(Exception )
			{
			
			}
		}

		#region public static void GenuineChannelsEventHandler(object sender, GenuineEventArgs e) Already Commented
		// call the message of this conferenceid
		//SearchConference(conferenceID).DetachUser(e.AdditionalInfo
		
		/*FlowMsgObj.question = "";
					FlowMsgObj.answer = "";
					FlowMsgObj.userid= "";
					FlowMsgObj.usertype= "";
					FlowMsgObj.qtype = "";
					*/
		#endregion

		/// <summary>
		/// Catches Genuine Channels events and removes client session when
		/// user disconnects.
		/// This Method do the following things
		/// 1-show source exception if its not null else its show the profile information of user
		/// 2-Handling GeneralConnectionReestablishing,GeneralConnectionClosed  event-types 
		///  do following things on these events:
		///  -get profile using GenuineEventArgs e parameter....profile=e.HostInformation["Profile"]
		///  -by seaching conference staticServerInstance.SearchConference(profile.ConferenceID) use the
		///   DetachUser() method of conference room...
		///   DetachUser do two things
		///   -Remove URI from dispatcher list to assure that this paticipant no longer able to receive message
		///   -Send all participant a message of "ControlMessage" type with ControlCode.AttendeeDropped  to update their Participant lists
		/// 3-Add message of leaving user from meeting
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void GenuineChannelsEventHandler(object sender, GenuineEventArgs e)
		{
			try
			{
				if (e.SourceException == null)
				#if TESTING
					Console.WriteLine("\r\n\r\n---Global event: {0}\r\nRemote host: {1}", 
						e.EventType,
						e.HostInformation == null ? "<unknown>" : e.HostInformation.ToString());
				#endif
				//else
				{
					#if TESTING
					Console.WriteLine("\r\n\r\n---Global event: {0}\r\nRemote host: {1}\r\nException: {2}", 
						e.EventType, 
						e.HostInformation == null ? "<unknown>" : e.HostInformation.ToString(), 
						e.SourceException);
					#endif
					ClientProfile profile=e.HostInformation["Profile"] as ClientProfile;
					if(profile != null)
					{
						#if TESTING
						Console.WriteLine("An Exception has occured where profile Name = " + profile.Name);
						#endif
					}
					else
					{
						#if TESTING
							Console.WriteLine("An Exception has occured where profile was not found");
						#endif
					}
				}

				/*  Effect of handling event of type e.EventType == GenuineEventType.GeneralConnectionReestablishing 
					"if a user disconnected from meeting then its clear all his participant list when its connected again it dont add user list that comes from server; 
					 to correct this problem we will write code in the client app to add users in the list."		
				 */			
				/* Some event types description 
				 * 1--GTcpConnectionAccepted--The GTCP server channel has accepted a connection. You can analyze the IP address of the remote host and decline
				 *		the connection. See the explanation below.			 
				 * 2--GeneralConnectionEstablished--The GTCP client channel has connected to the server.The GTCP server channel has accepted a connection opened
				 *     by the client.
				 * 3--GeneralConnectionReestablishing--The GTCP client channel recongnizes that the connection is broken but will attempt to reconnect to the server
				 *	   automatically.The GTCP server channel recognizes that the connection is broken and the client is expected to reestablish the
				 *     connection within the specified time span.
				 * 4--GeneralConnectionClosed--The GTCP server channel has released all resources
				 *  associated with an appropriate client connection and will not be able to accept a reconnection from it.
				 *  The GTCP client channel has closed the connection to the remote peer.
				 */
				
				/* some info about this event handler.				 
				 * Belikov.GenuineChannels.DotNetRemotingLayer.GenuineGlobalEventProvider provides the global
				 * GenuineChannelsGlobalEvent event fired for each Genuine Channels event. You can attach a
				 * handler to it if you want to process events generated by all channels and services.
				 */
				if (e.EventType == GenuineEventType.GeneralConnectionClosed ||e.EventType == GenuineEventType.GeneralConnectionReestablishing )
				{										
					// the client disconnected
					ClientProfile profile=e.HostInformation["Profile"] as ClientProfile;//string ConnectionID= e.HostInformation["ConnectionID"] as string;											
					staticServerInstance.SearchConference(profile.ConferenceID).DetachUser(profile);
					
					ST_flowMsg FlowMsgObj = new  ST_flowMsg ();
					FlowMsgObj.meetID = profile.ConferenceID ;
					FlowMsgObj.curDate= DateTime.Now.Year + "-" + DateTime.Now.Month +"-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
					FlowMsgObj.curMsg = profile.Name +" has left from "+ profile.ConferenceID+ " conference." ;
					FlowMsgObj.MsgCategory = "Attendee Joins and Leaves";
					//staticServerInstance.flowMsgs.Add(FlowMsgObj);
					FlowMsgObj = null;					
					#if TESTING
					Console.WriteLine("Client \"{0}\" has been disconnected.", profile.ConferenceID);			
					#endif
				}
			}
			catch(Exception)
			{
			}
		}
		

		#region public bool ValidateAuthentication(string Password,string confId) Already Commented Code
		/*
				string sql = "select meeting_password from ctool_meeting where meeting_password = '" + Password + "' AND meeting_id = " + confId;
				ArrayList recordSet = dbCon.Execute(sql);
				if(recordSet.Count > 0)
				{
					return true;
			
				}
				*/
		#endregion
		public bool ValidateAuthentication(string Password,string confId)
		{			
			return true;
		}
		
		
		#region	 "Logging"	
		public void addFlowMsgs(string datetime, string curMessage, string msgCategory)
		{
			try
			{
				ST_flowMsg FlowMsgObj = new  ST_flowMsg ();
				//FlowMsgObj.meetID = Convert.ToInt32(profile.ConferenceID) ;
				FlowMsgObj.curDate= datetime;
				FlowMsgObj.curMsg = curMessage;
				FlowMsgObj.MsgCategory = msgCategory;
				/*FlowMsgObj.question = "";
				FlowMsgObj.answer = "";
				FlowMsgObj.userid = "";
				FlowMsgObj.usertype = "";
				FlowMsgObj.qtype = "";
				*/
				//flowMsgs.Add(FlowMsgObj);
				FlowMsgObj = null;
			}
			catch(Exception ex)
			{
				ex=ex;
			}
		}
			
		#endregion
		
		#region private ConferenceRoom SearchConference(String confId ) Already Commented Code
		//string curDateTime=DateTime.Now.Year + "-" + DateTime.Now.Month +"-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
		/*ST_flowMsg FlowMsgObj = new  ST_flowMsg ();
			FlowMsgObj.meetID = Convert.ToInt32(conf.ConferenceId) ;
			FlowMsgObj.curDate= DateTime.Now.Year + "-" + DateTime.Now.Month +"-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
			FlowMsgObj.curMsg = conf.conferenceName + " conference started." ;
				
			flowMsgs.Add(FlowMsgObj);
			FlowMsgObj = null;
			*/
		#endregion

		
		/// <summary>
		/// This helper routine searches for the conference with specified meeting
		/// id in the running conference list as well as the scheduled conference
		/// list. If Conference is not found, it throws an ArgumentException.
		/// This fucntion do the following things
		/// 1-Take ConferenceRoom object with assign null
		/// 2-Find conference on the base of this method parameter "confId" in the Hash "RunningConferenceList.ConfList"
		/// and assign it to conf object of type ConferenceRoom.
		/// Note: we use "RunningConferenceList.ConfList" for maintaining all the conferences objects in the this class.
		/// 3-if conference not found in the RunningConferenceList.ConfList then it search from db
		/// and make new object of type ConferenceRoom and assign different setting
		/// </summary>
		/// <param name="confId"> Globally unique Id of the conference to search for </param>
		/// <returns> The Conference corresponding to the Id specified by the caller </returns>
		private ConferenceRoom SearchConference(String confId )
		{
			ConferenceRoom conf = null;
			string exceptionText = "No Conference with specified Id was found on the server";
			string conferenceName;
			DateTime d = DateTime.Now;
			//
			// Find this conference on the server
			//
			do
			{
				//
				// Verify whether a meeting with this Id already exists on the server.
				//
				conf = RunningConferenceList.FindConference(confId);
				
				if(conf != null)
				{
					break;
				}                
				//Search the conference
				
				Console.WriteLine(" Find Conference failed. Creating New Conference \n");
				try
				{
					string sql = "select meeting_date,meeting_time,meeting_entry,meeting_title from ctool_meeting where meeting_completed = 0 and meeting_id = " + confId;
					//MessageBox.Show(sql);
					ArrayList Recordset = dbCon.Execute(sql);
					Console.WriteLine(sql + "\n Execute. Recordset Count = " + Recordset.Count.ToString());
					if(Recordset.Count < 1)
					{
						conf = null; 
						break;
					}							
					//MessageBox.Show("Recordset is not null");
					ArrayList Record = (ArrayList)Recordset[0];
					conferenceName= Record[3].ToString();
					string strdate = Record[0].ToString();//
					string[] date = strdate.Split('-');
					int Year = Convert.ToInt32(date[0].ToString());
					int Month = Convert.ToInt32(date[1].ToString());
					int Day = Convert.ToInt32(date[2].ToLower());

					string strTime = Record[1].ToString();
					string[] time = strTime.Split(':');
					int hour = Convert.ToInt32(time[0]);
					int min = Convert.ToInt32(time[1]);

					d = new DateTime(Year,Month,Day,hour,min,0);

					if(Record[2].ToString() == "1")
					{
					
						TimeSpan s = d.Subtract(DateTime.Now);

						DateTime c = DateTime.Now;
						string currentDateTimeString = c.Year.ToString() + "-" + c.Month.ToString() + "-" + c.Day.ToString() + " " + c.Hour + ":" + c.Minute + ":" + c.Second;

						if(s.TotalDays >1 )
						{
							exceptionText= "Meeting is scheduled at " + strdate + " " +strTime + ". Current Time is " + currentDateTimeString ;

							//MessageBox.Show("Total Days " + s.TotalDays.ToString());
							break;
						}
						if(s.TotalDays< -1)
						{
							exceptionText= "Meeting was scheduled for  " + strdate + " " +strTime + ". Current Time is " + currentDateTimeString ;
							//MessageBox.Show("Total Days " + s.TotalDays.ToString());
							break;
						}
						if((s.TotalMinutes > 15))
						{
							exceptionText= "Meeting is scheduled at " + strdate + " " +strTime + ". Current Time is " + currentDateTimeString ;
							//MessageBox.Show("Total Minutes " + s.TotalMinutes.ToString());
							break;
						}
					}
					Console.WriteLine("Returning from Search Conference");
				}
				catch(Exception ee)
				{
					break;
				}			
				conf = new ConferenceRoom(confId);
				/*
				 * conf.conferenceVoiceSessionID = getConferenceVoiceID();
				 * is commented because its not using in the code
				 */
				//conf.conferenceVoiceSessionID = getConferenceVoiceID();
				conf.conferenceStartTime = d;
				RunningConferenceList.Add(confId, conf);
				conf.conferenceName = conferenceName;
			} while(false);
			
			if(conf == null)
			{
				throw new ArgumentException(exceptionText, "Conference");
			}
			return conf;
		}	
			
		/// <summary>
		/// This method has no functionality at all in application but we are using it.
		/// </summary>
		/// <param name="confId">Conference Id</param>
		/// <returns></returns>
		private string GetImage(string confId)
		{
			try
			{
				string sql = "select Concat(company_acc_id,company_acc_image) as Image from ctool_company_acc,ctool_meeting where meeting_id = " +  confId;
				ArrayList Recordset = dbCon.Execute(sql);
				if(Recordset.Count > 0)
				{
					ArrayList Record = (ArrayList)Recordset[0];
					return Record[0].ToString();			
				}
			}
			catch(Exception ee)
			{
			}
			return "";
			
		}

		/// <summary>
		/// This is to insure that when created as a Singleton, the first instance never dies,
		/// regardless of the expired time.
		/// </summary>
		/// <returns></returns>
		public override object InitializeLifetimeService()
		{
			return null;
		}
		
		/// <summary>
		/// Used to Marked a meeting closed.
		/// This method only called from the public void CloseConference(MessageObject msg)--ConferenceRoom
		/// with parameter ConferenceServer.staticServerInstance.CloseConference(this,true);
		/// so code in if(!ForceClose) never run, should be commented.
		/// This function do the following things
		/// -update ctool_meeting tables with meeting_completed = 1 
		/// -send attendeDropped message to all participant to passed conf object.
		/// -Add log that shows this conference is closed.
		/// -Remove passed conference object from the RunningConferenceList.ConfList hash table
		/// -Assign Conf set to null for garbage collector to collect.
		/// </summary>
		/// <param name="conf"></param>
		/// <param name="ForceClose"></param>
		public void CloseConference(ConferenceRoom conf,bool ForceClose)
		{
			try
			{				
				string sql = "select meeting_date,meeting_time,meeting_dur from ctool_meeting where meeting_completed = 0 and meeting_id = " + conf.ConferenceId;
				if(!ForceClose)
				{									
					ArrayList Recordset = dbCon.Execute(sql);
					if(Recordset.Count < 1)
					{
						return;
					}
					ArrayList Record = (ArrayList)Recordset[0];
					string strdate = Record[0].ToString();//
					string[] date = strdate.Split('-');
					int Year = Convert.ToInt32(date[0].ToString());
					int Month = Convert.ToInt32(date[1].ToString());
					int Day = Convert.ToInt32(date[2].ToLower());

					string strTime = Record[1].ToString();
					string[] time = strTime.Split(':');
					int hour = Convert.ToInt32(time[0]);
					int min = Convert.ToInt32(time[1]);

					DateTime d = new DateTime(Year,Month,Day,hour,min,0);
					d.AddHours(Convert.ToInt32(Record[2].ToString()));                
					if(d > DateTime.Now)
						return;					
				}                			                    			    
				sql = "update ctool_meeting set meeting_completed = 1 WHERE meeting_id = " + conf.ConferenceId;
				dbCon.Execute(sql);
				try
				{
					foreach(ClientProfile client in conf.ClientList)
					{				
						conf.RemoveClientFromConference(client.clientURI,false);									
					}
				}
				catch(Exception ee)
				{					
				}				
				ST_flowMsg FlowMsgObj = new  ST_flowMsg ();
				FlowMsgObj.meetID = conf.ConferenceId ;
				FlowMsgObj.curDate= DateTime.Now.Year + "-" + DateTime.Now.Month +"-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
				FlowMsgObj.curMsg = conf.conferenceName + " conference has closed." ;				
				//flowMsgs.Add(FlowMsgObj);
				FlowMsgObj = null;
				//this.mconferenceVoices[conf.conferenceVoiceSessionID] = ConferenceVoiceEnum.Unused;
				RunningConferenceList.ConfList.Remove(conf.ConferenceId);
				conf.ShutDownConference();				
				conf = null;
			}
			catch(Exception ee)
			{
				ee =ee;
			}
		}        /// <summary>
	
		#region public void CloseConference(ConferenceRoom conf,bool ForceClose) Already Commented Code
		//conf.RemoveClientFromConference(client.Profile.ClientId, false);										
		/*FlowMsgObj.question = "";
				FlowMsgObj.answer = "";
				FlowMsgObj.userid = "";
				FlowMsgObj.usertype = "";
				FlowMsgObj.qtype = "";
				*/
		#endregion

		#region Billing Thread Method Commented
		/// <summary>
		/// This function updates the billing information in the database this method is called after each 30 seconds and it updates the information
		/// </summary>
//		public void UpdateBillingInformation()
//		{
//			// update all clients in all conferences here
//			string date,sql = "";
//			DateTime t;// = null;
//			try
//			{
//			
//				while(true)
//				{
//					
//					t = DateTime.Now;							
//					//date = t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + " " + t.ToLongTimeString().ToString();
//					date = t.ToString("yy-MM-dd HH:mm:ss");
//					
//					foreach(Object obj in RunningConferenceList.ConfList)
//					{
//						try
//						{
//							ConferenceRoom conf=(WebMeeting.Server.ConferenceRoom)((System.Collections.DictionaryEntry)obj).Value;
//							foreach(ClientProfile client in conf.ClientList.Values)
//							{
//								sql="Update ctool_billing set billing_duration_logouttime = '" + date + "' where billing_id=" + client.billingID;
//						
//								//System.Windows.Forms.MessageBox.Show(sql);
//								dbCon.Execute(sql);
//								//..
//							}
//							Console.WriteLine("Billing information updated for Conf id " + conf.ConferenceId);
//						}
//						catch(Exception ee)
//						{
//							//Console.WriteLine(ee.ToString());
//							ee =ee;
//						}
//					}
//					Thread.Sleep(30000);
//				}
//			}
//			catch(Exception ex)
//			{
//				ex=ex;
//			}
//			
//		}
		
		#endregion

		#region "public IConferenceRoom JoinConference(int conferenceID,ref WebMeeting.Common.ClientProfile profile,out AuthenticationMessage Msg, string Password,bool IsGuest) Commented Code"
		//**************** billing
		//		string curDateTime=DateTime.Now.Year + "-" + DateTime.Now.Month +"-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
		//		string curTime=DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
		//				
		//		sql="Insert into ctool_billing (billing_member_id,billing_mtype,billing_meeting_id,billing_duration_logintime,billing_duration_logouttime,billing_stamp,billing_conid)";
		//		if(!IsGuest)
		//		sql = sql + "values (" + profile.ClientRegistrationId +",0," + conferenceID+ ",'" + curDateTime +"','";
		//		else
		//		sql = sql + "values (" + profile.ClientRegistrationId +",1," + conferenceID+ ",'" + curDateTime +"','";
		//
		//		sql+=curDateTime + "','" + curDateTime + "'," + profile.ClientId + ")";
		//		dbCon.Execute(sql);
		//		Console.WriteLine(sql + " Executed ");
		//		//MySQL.MySql.Query((System.IntPtr)dbCon,sql);			
		//		sql="Select billing_id from ctool_billing where billing_conid=" + profile.ClientId + " and billing_stamp='" + curDateTime + "'";
		//		ArrayList results =  dbCon.Execute(sql);
		//		Console.WriteLine(sql + " Executed ");
		//		ArrayList Fields = (ArrayList)results[0];			
		//		int billingID = Convert.ToInt32((string)Fields[0]);
		//		Console.WriteLine(" Billing ID Retrieved ");
		//		profile.billingID = billingID;
		//		Console.WriteLine("\n Billing Id retrieved");
		//conf.SendUserJoinMessage(profile);
		#endregion

		#region "public IConferenceRoom JoinConference(int conferenceID,ref WebMeeting.Common.ClientProfile profile,out AuthenticationMessage Msg, string Password,bool IsGuest) Already Commented Code"
		/////////////////////// if user is exist then remove it and it is automatically add as a new user			
		//Hashtable tempClientList = new Hashtable(conf.ClientList);
			
			
		//			Console.WriteLine("profile name :::"+profile.Name);
		//			
		//
		//			foreach(ClientProfile cp in conf.ClientList.Values)
		//			{
		//				Console.WriteLine("client List name :::"+cp.Name.Trim());
		//				if(cp.Name.Trim()==profile.Name.Trim())
		//				{
		//					Console.WriteLine("Exception fired of login duplicate");
		//					throw (new System.ArgumentException("A user is already login with name.", profile.Name.Trim()));
		//				}
		//				if(cp.ClientRegistrationId == profile.ClientRegistrationId) // client exist in this meeting 
		//				{
		//					//throw (new System.ArgumentException("You already have an alive session with WebMeeting.", "ClientId"));
		//					//tempClientList.Remove( profile.ClientId );
		//					//conf.ClientList.Remove(cp.ClientId );
		//					conf.checkConnectionUrgent(cp.ClientId );
		//					//break;
		//				}
		//			}
		
		//conf.ClientList = null;
		//conf.ClientList = new Hashtable(tempClientList);

		/*
				AuthenticationMessage msg = new AuthenticationMessage(profile.ClientId,false);
				client.SendMessage(msg);
				conf = null;					
				ServerUI.serverUI.AddUserCount();
				*/

		/*
				Msg.FTPPassoword = this.FTP_Password;
				Msg.FTPUsername = this.FTP_Username;
				Msg.FTPPort = this.FTP_PORT;
				Msg.FTP_IP = this.FTP_IP;
				Msg.ImageURL = GetImage("" + conferenceID);				
				Msg.ClientEmail = "Test";
				Msg.ClientName = "alice";
				Msg.SenderID=-1;
					*/


		//throw (new System.ArgumentException("You already have an alive session with WebMeeting.", "ClientId"));
		//tempClientList.Remove( profile.ClientId );
		//conf.ClientList.Remove(cp.ClientId );

		/*FlowMsgObj.question = "";
			FlowMsgObj.answer = "";
			FlowMsgObj.userid = "";
			FlowMsgObj.usertype = "";
			FlowMsgObj.qtype = "";
			*/	
		/////////////////////// /////////////////////// /////////////////////// /////////////////////// 

		//			string sql2="Select company_member_fname,company_member_email From ctool_company_member where company_member_id = " + profile.ClientRegistrationId ;//+ " AND login_pass " ;								
		//Console.WriteLine("\n " + sql2 + " \n Executed. Recordset count is " + recordSet.Count.ToString());
		//MessageBox.Show(sql2);

		#endregion
	}
}
