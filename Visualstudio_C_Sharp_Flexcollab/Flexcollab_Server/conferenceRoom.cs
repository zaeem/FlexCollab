using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Threading;
using Belikov.GenuineChannels;
using Belikov.GenuineChannels.BroadcastEngine;
using WebMeeting.Common;
using WebMeeting.WebIntegeration;
using System.Drawing;
using System.Drawing.Imaging;
using Word;
using Excel;
using PowerPoint;
using Net.Sgoliver.NRtfTree;
using MySQL;


namespace WebMeeting.Server
{
	/// <summary>
	/// Represents a chat room.
	/// </summary>
	public class ConferenceRoom : MarshalByRefObject, IConferenceRoom
	{
		

		#region Commented Variables
		/// <summary>
		/// Not Using Anywhere
		/// </summary>
		//private bool restartTimer;
		//private System.Windows.Forms.RichTextBox richTextBox1;
		#endregion


		#region "Variable Section"
		
		public bool isFirstSlide = true ;
		private bool bAppShared = false;
		public bool bHostJoined = false;
		
		private bool m_bLocked=false;
		private bool m_bIsActive = true;

		public int uniqueSessionID;
		public int conferenceVoiceSessionID;
		/// <summary>
		/// The identifier, unique within this conference, that will be assigned
		/// to the next client to enter this meeting
		/// </summary>
		public int NextClientId = (int) MessageObjectDestId.FirstClientId;
		private int mcount=0;


		/// <summary>
		/// A unique identifier for the conference in progress
		/// </summary>
		public String           ConferenceId;
		public string conferenceName;
		/// <summary>
		/// Chat members.
		/// </summary>	
		private string strMeetingId;




		public Hashtable PollsList;
		public Hashtable VideoList;
		public Hashtable AudioList;
		public Hashtable DesktopList;
		public Hashtable DocumentSharingList;
		public Hashtable WebBrowserList;
		Hashtable hashCurSlide = new Hashtable();
		//	public Hashtable DocumentList;
		public Hashtable FileList;
		/// <summary>
		/// Holds instance of all the client attending the current meeting
		/// </summary>
		public Hashtable        ClientList;



		/* 
		 * WhiteboardMessage list contains all the whiteboard events for the new attendee to attain the current state
		 * 
		 */
		public ArrayList WhiteBoardMessagesList;
		public ArrayList CompoundMessageList;
		
		/*
		 *	Will contain all the chat messages so that the new attendee can view the messages previously sent	 
		 * 
		 */
		public ArrayList ChatMessages;			
		public ArrayList QAQuestionMessages;
		public ArrayList WebBrowserMessages;
		public ArrayList LoggedMessages;
		/* 
		 * Will contain the list of files shared / viewable in the current conference 
		 */
		public ArrayList SharedFiles;
		/* 
		 * Will contain the list of currently available videos in this conference
		 */
		public ArrayList SharedVideos;
		/*
		 * Will contain the currently available pollings and their current state
		 */ 
		public ArrayList SharedPolls;


		public Color[] colorsArray;// = new Color[15];


		
		private static object Unknown =Type.Missing; 



		public DateTime conferenceStartTime;

		
		//public Thread conferenceCloseThread ;
		private Thread connectionCheckThread;



		
		
		private TabPagesSyncrhonizationMsessage msgSynchronization;
		
		#endregion				

		#region "IConferenceRoom Implementation"
		/*void SendLoggedMessage(MessageObject message);
		void SendNonLoggedMessage(MessageObject message);
		void SendNonLoggedMessage2(MessageObject message);
		void SendPrivateChatMessage(MessageObject obj,int receiverConnectionID);		
		void RemoveClientFromConference(string clientURI, bool gracefulExit);	
		void GetUniqueSessionID(ref int i);
		void GetManageContent(MessageObject msg);

		void SendAudioMessage(MessageObject m);
		void SendVideoMessage(MessageObject m);
		void SubscribeToAudio(MessageObject m);
		void SubscribeToVideo(MessageObject m);
		void RegisterVideo(MessageObject msg);
		void RegisterAudio(MessageObject msg);			
		void CloseConference(MessageObject msg);
		int StartDocumentSharing(ref MessageObject msg);	
		void RequestDesktopShare(int clientID, int senderID);
		int HandleDesktopShareRespnse(int clientID,int senderId,bool response);
		void RecieveConnectionMsgResponse(MessageObject m);
		void SendDesktopMessage(MessageObject m);		
		string UserDefinedPing(string msgtext);				
		*/		
		//Method for network test.
		public void SendDesktopMessage(MessageObject m)
		{
			AppSharingMessageEx msg=(AppSharingMessageEx ) m;
			Desktop  a=(Desktop)DesktopList[msg.sessionId];
			if(a!=null)
				a.SendMessage(ref msg);
		}
		public string UserDefinedPing(string msgtext)
		{	
			return (msgtext + msgtext);
		}
		
	    public void RecieveConnectionMsgResponse(MessageObject m)
		{
			ConnectionChechMessage msg = (ConnectionChechMessage)m;
			foreach(ClientProfile p in ClientList.Values)
			{				
				if(p.ClientId == msg.profile.ClientId)
				{					
					p.connectionCheckRecievedDateTime = DateTime.Now.ToFileTime();
					break;
				}
			}		

		}

		public int HandleDesktopShareRespnse(int clientID,int senderID,bool response)
		{
			ClientProfile cp = (ClientProfile)this.ClientList[senderID];
			if(cp != null)
			{
				
				IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), cp.clientURI);
				// make message here and send it
				//iMessageReceiver.ReceiveMessage(obj);
				DesktopSharingControlMessage msg = new DesktopSharingControlMessage(response,senderID,clientID);
				msg.senderID = senderID;
				msg.Status = 1;
				

				if(response)
				{						
					Desktop serverToClient=new Desktop(); // this is for server to client
					GetUniqueSessionID(ref serverToClient.sessionID);
					serverToClient.AddClient(iMessageReceiver);		




					Desktop clientToServer=new Desktop(); // this is for client to server
					GetUniqueSessionID(ref clientToServer.sessionID);
					// get the server id here and add it to the clienttoserver desktop channel
					//DesktopList.Add( clientToServer,clientToServer.sessionID);
					//DesktopList.Add( serverToClient,serverToClient.sessionID);
					DesktopList.Add( clientToServer.sessionID,clientToServer);
					DesktopList.Add( serverToClient.sessionID,serverToClient);
					
					ClientProfile cpX = (ClientProfile)ClientList[senderID];
					if(cpX != null)					
					{	
						IMessageReceiver iMessageReceiverServer =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), cpX.clientURI);
						clientToServer.AddClient(iMessageReceiverServer);

						// also send this server a message to initialize its server and set its session id
						DesktopSharingControlMessage msg2 = new DesktopSharingControlMessage(response,senderID,clientID);
						msg2.senderID = clientID;
						msg2.Status = 2;
						msg2.ServerToClientSessionID=serverToClient.sessionID;
						iMessageReceiverServer.ReceiveMessage(msg2);	
				
						msg2 = null ; //GC
						GC.Collect(); //GC
												
					}						

					// sending the session id to the client. So that he may send the messages to the specific channel
					iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), cp.clientURI);
					msg.ClientToServerSessionID=clientToServer.sessionID;
					iMessageReceiver.ReceiveMessage(msg); //this is the client who requested to start the desktop sharing
					//we've created a desktop object and have attached it with him
					//now the person going to share his desktop needs to have connection with the requester

				
					serverToClient = null; //GC
					clientToServer = null; //GC
					GC.Collect(); //GC
				}
				else
				{
					iMessageReceiver.ReceiveMessage(msg);
				}
				
				msg = null; //GC
				GC.Collect(); //GC
							
			}

			return 0;
		}

		public void RequestDesktopShare(int clientId, int senderID)
		{
			ClientProfile cp = (ClientProfile)this.ClientList[senderID];
			if(cp != null)
			{
				
				IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), cp.clientURI);
				// make message here and send it
				//DesktopSharingControlMessage msg = new DesktopSharingControlMessage(true,senderID,clientId,
				//iMessageReceiver.ReceiveMessage(obj);
				DesktopSharingControlMessage msg = new DesktopSharingControlMessage(true,senderID,clientId);
				msg.Status = 0;
				msg.SenderID=senderID;
				iMessageReceiver.ReceiveMessage(msg);

				msg = null; //GC
				GC.Collect(); //GC
				
			}			

		}



		# region StartDocumentSharing(ref MessageObject msg)
		public int StartDocumentSharing(ref MessageObject msg)
		{
			try
			{
				DocumentSharing Message = (DocumentSharing)msg;
				string strUploadedPath = ConferenceServer.staticServerInstance.WEBSERVER_DOCUMENTSHARING_UPLOAD_PATH;
				string documentFolderName = "";
				if(Message.DocumentType == DocumentMessageType.typeDocument)
					documentFolderName = "Documents";
				else if(Message.DocumentType == DocumentMessageType.typeExcel)
					documentFolderName = "WorkBooks";
				else if(Message.DocumentType == DocumentMessageType.typePDF)
					documentFolderName = "PDF";
				else if(Message.DocumentType == DocumentMessageType.typePresentation)
					documentFolderName = "Presentations";
			
				strUploadedPath += "\\" + this.ConferenceId + "\\" + Message.sessionID + "\\" + documentFolderName + "\\" + Message.DownloadURL;
				strUploadedPath = strUploadedPath.Replace("\\\\","\\");
                        
				string strLocalPath ="";// ConferenceServer.staticServerInstance.WEBSERVER_DOCUMENTSHARING_DOWNLOAD_PATH ;
				
				/*  ccode for making temporary directory
				strLocalPath += "\\" + this.ConferenceId ;//+ "\\" + Message.SenderID
				CreateDirectory(strLocalPath);
				strLocalPath += "\\" + Message.sessionID;
				CreateDirectory(strLocalPath);						
				strLocalPath += "\\" + Message.DownloadURL;
				strLocalPath  = strLocalPath .Replace("\\\\","\\");
				*/
				WebMeeting.Common.ICSharpZip.UnzipZip(strUploadedPath +".zip",Path.GetDirectoryName(strUploadedPath) + @"\");
				
				////////////////START  Code for create local copy of the document for conversion and send update message
				strLocalPath = Path.GetDirectoryName(strUploadedPath) + "\\" + Path.GetFileNameWithoutExtension(Message.DownloadURL) ;//+ ".htm";
				//				if(!Directory.Exists(strLocalPath))
				//					Directory.CreateDirectory(strLocalPath);
				//				strLocalPath += "\\Converted";
				//				if(!Directory.Exists(strLocalPath))
				//					Directory.CreateDirectory(strLocalPath);
				//				strLocalPath += "\\" + Path.GetFileNameWithoutExtension(Message.DownloadURL) + ".htm";	
				DocumentSharingStatusUpdate updateMsg = new DocumentSharingStatusUpdate();
				updateMsg.sessionID = Message.sessionID;
				updateMsg.SenderID = -1;
				updateMsg.strMessage = "Converting Document...";
				((IMessageReceiver) this._dispatcher.TransparentProxy).DocumentSharingStatusUpdate(updateMsg);
				//				
				////////////////END  Code for create local copy of the document for conversion				
				
				// code for converting documents.
				switch(Message.DocumentType)
				{
					case DocumentMessageType.typeDocument:
						if(!ConvertWordDocument(strUploadedPath,strLocalPath))
							return 0;
						break;
					case DocumentMessageType.typePresentation:
						
						if(!ConvertPresentationDocument(strUploadedPath,Directory.GetParent(strLocalPath).FullName,ref Message))
							return 0;
						break;
					case DocumentMessageType.typeExcel:
						if(File.Exists(strLocalPath))
							File.Delete(strLocalPath);
						if(!ConvertExcelDocument(strUploadedPath,strLocalPath ,ref Message))
							return 0;
						break;
					case DocumentMessageType.typePDF:
					{
						if(File.Exists(strLocalPath))
							File.Delete(strLocalPath);
						if(!ConvertPDFDocument(strUploadedPath,strLocalPath))
							return 0;
						break;
					}
				}			
				System.Threading.Thread.Sleep(500);	
				/*
				ArrayList Files = new ArrayList();
				GetFilesofDirectories(Path.GetDirectoryName(strLocalPath),Files);
				for(int i = 0 ; i < Files.Count ; i++)						
				{
					zip.AddFile(Files[i].ToString());				
				}
				*/
					
				//string temp = Directory.GetParent(strLocalPath).FullName;
				//strLocalPath = Directory.GetParent(strLocalPath).Parent.FullName + "\\" +  Path.GetFileName(strLocalPath) + ".zip";
            	
				/*  for sending message to clients
				 updateMsg.strMessage = "Compressing Document...";	
				((IMessageReceiver) this._dispatcher.TransparentProxy).DocumentSharingStatusUpdate(updateMsg);*/
				//updateMsg = null;
				//ICSharpZip.ZipDirectory(temp,strLocalPath);					
				//Message.DownloadURL = ConferenceServer.staticServerInstance.WEBSERVER_DOCUMENTSHARING_DOWNLOAD_URL + "/" + this.ConferenceId + "/" + Message.sessionID + "/" + Path.GetFileNameWithoutExtension(Message.DownloadURL) + "/" + Path.GetFileName(strLocalPath);
				
				//Directory.Delete(temp,true);
				
				msg = Message;
            
				return 1;				
			}
			catch(Exception ee)
			{
				Console.Write("\n\nERRRRRRRRRRRORRRR " + ee.Message);
			}
			return 0;
		}
	# endregion




		public void CloseConference(MessageObject msg)
		{
			try
			{
				this.BroadcastControlMessage(ControlCode.PresenterClosedConference,((ControlMessage)msg).Parameter);
				ConferenceServer.staticServerInstance.CloseConference(this,true);
				m_bIsActive = false;
			}
			catch(Exception ee)
			{
				Console.WriteLine(ee.Message);
			}
		}




		public void RegisterVideo(MessageObject msg)
		{
			RegisterVideoMessage m=(RegisterVideoMessage) msg;

			int i=0;
			ClientProfile profile=(ClientProfile)ClientList[msg.SenderID];
			if(profile != null)
			{

				if(m.subscribe==true)
				{

					profile.videoID=m.videoID;					
					Video v=new Video();
					VideoList.Add( m.videoID,v);
				}
				else
				{
					profile.videoID=-1;
				}
				BroadcastControlMessage(ControlCode.AttendeeUpdate, profile);
			}
		}





		public void RegisterAudio(MessageObject msg)
		{
			
			RegisterAudioMessage m=(RegisterAudioMessage) msg;

			int i=0;
			ClientProfile profile=(ClientProfile)ClientList[m.SenderID];						
			if(profile != null)
			{
				if(m.subscribe==true)
				{
					profile.audioID=m.audioID;					
					Audio a=new Audio();
					AudioList.Add( m.audioID,a);

				}
				else
					profile.audioID=0;
				BroadcastControlMessage(ControlCode.AttendeeUpdate, profile);
			}           	
			
		}




		public void SubscribeToAudio(MessageObject msg)
		{
			SubscribeToAudioMessage m=(SubscribeToAudioMessage)msg;
			Audio a=(Audio)AudioList[m.audioID];
			if(a==null) 
			{
				// tell that client that there is no such a video available						
				return;
			}
			int i=0;
		
			ClientProfile cp =(ClientProfile) ClientList[m.SenderID];
			if(cp!=null)
			{			
				if(m.subscribe==true)
					a.AddClient((IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver),cp.clientURI ));					
				else
					a.RemoveClient((IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver),cp.clientURI ));
			}
			
		}


		public void SubscribeToVideo(MessageObject msg)
		{			
			SubscribeToVideoMessage m=(SubscribeToVideoMessage)msg;
						
			Video v=(Video)VideoList[m.videoID];

			if(v==null)
			{
				// tell that client that there is no such a video available						
				return;
			}
			int i=0;
			/*
			string uri="";
			for(i=0;i<this.ClientList.Count;i++)
			{
				if(((ClientProfile)ClientList[i]).ClientId==m.SenderID)
				{
					uri=((ClientProfile)ClientList[i]).clientURI;
					break;
				}
			}
			*/
			ClientProfile cp = (ClientProfile) ClientList[m.SenderID];
			if(cp!=null)
			{			
				if(m.subscribe==true)
					v.AddClient((IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver),cp.clientURI ));				
				else
					v.RemoveClient((IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver),cp.clientURI ));
			}

		
		}


		public void SendVideoMessage(MessageObject msg)
		{
			VideoMessage m=(VideoMessage) msg;
			Video v=(Video)VideoList[m.videoID];
			if(v!=null)
				v.SendMessage(ref m);					
		}



		public void SendAudioMessage(MessageObject msg)
		{
			AudioMessage m =(AudioMessage) msg;
			Audio a=(Audio)AudioList[m.audioID];
			if(a!=null)
				a.SendMessage(ref m);
		}
		/// <summary>
		/// Sends message to all clients.
		/// </summary>
		/// <param name="message">Message to send.</param>
		/// <returns>Number of clients having received this message.</returns>
		public void SendLoggedMessage(MessageObject message)
		{
			// fetch the nickname
			//string nickname = GenuineUtility.CurrentSession["Nickname"] as string;
			//Console.WriteLine("Message \"{0}\" will be sent to all clients from {1}.", message, nickname);

			//IMessageReceiver iMessageReceiver = (IMessageReceiver) this._dispatcher.TransparentProxy;

			// Zaeem view 
			/// This code should be commented 
			/// 
			if(message.GetType() == typeof(DocumentSharing))
			{					
				int x = ((DocumentSharing)message).sessionID;				
				Object obj =  DocumentSharingList[x];
				if(obj == null)
					DocumentSharingList.Add(((DocumentSharing)message).sessionID,message);
				else
					DocumentSharingList[x] = message;				
			}
			else if(message.GetType() == typeof(DrawingClearMessage))
			{
				try
				{
					DrawingClearMessage dMsg = (DrawingClearMessage)message;
					if(dMsg.m_ControlType == ControlType.DrawingBoard)
					{
						for(int i = LoggedMessages.Count -1 ; i >= 0 ; i --)
						{
							MessageObject objMsg = (MessageObject)LoggedMessages[i];
							if(objMsg.GetType().Equals(typeof(StateUpdateMessage)))
							{
								if(((StateUpdateMessage)objMsg).updateType == UpdateTypeCode.UpdateWhiteboard)
								{
									LoggedMessages.Remove(objMsg);
								}
							}
						}
					}
				}
				catch(Exception ee)
				{
				}
			}
			else if(message.GetType() == typeof(MsgWebBrowseMessage))
			{
				MsgWebBrowseMessage msg = (MsgWebBrowseMessage)message;
				Object obj = WebBrowserList[msg.webBrowseID];
				if(obj == null)
					WebBrowserList.Add(msg.webBrowseID,message);
				else					
					WebBrowserList[msg.webBrowseID] = message;
				
			}
			else if(message.GetType().Equals(typeof(StateUpdateMessage)))
			{
				StateUpdateMessage msg = (StateUpdateMessage )message;
				if(msg.updateType != UpdateTypeCode.UpdateAppsharingannotations)
					LoggedMessages.Add(message);
			}
			else if(message.GetType().Equals(typeof(TabPagesSyncrhonizationMsessage)))
			{
				msgSynchronization	= (TabPagesSyncrhonizationMsessage)message;				
			}
			else
				LoggedMessages.Add(message);











			if(message.GetType() == typeof(ControlMessage))
			{
				ControlMessage tempMessage = (ControlMessage) message;
				if(tempMessage.Code == ControlCode.AttendeeUpdate)
				{
					ClientProfile prof = (ClientProfile)tempMessage.Parameter;// = profile;
					foreach(ClientProfile cp in ClientList.Values)
					{
						if(prof.ClientId == cp.ClientId)
						{
							ClientList.Remove(prof.ClientId);
							ClientList.Add(prof.ClientId,prof);
							
							//GenuineUtility.CurrentSession["Profile"] = prof;
							break;
						}
					}
					
				}
				else if(tempMessage.Code == ControlCode.PresenterClosedConference)
				{
					CloseConference(tempMessage);
				}
				else if(tempMessage.Code == ControlCode.PresenterKickedPerson)
				{
					ClientProfile prof = (ClientProfile)tempMessage.Parameter;// = profile;
					IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), prof.clientURI);
					if(iMessageReceiver != null)
					{
						iMessageReceiver.ReceiveMessage(tempMessage);
						LoggedMessages.Remove(message);
						return;
					}					
				}
			}
			((IMessageReceiver) this._dispatcher.TransparentProxy).ReceiveMessage(message);

		}

		/// <summary>
		/// This Method is used for sending messages to all clien in a meeting
		/// Its do two things
		/// First Send message to all clients
		/// Second Log different meeting activies using different messages like
		/// DocumentSharing,WhiteboardMessage,MsgWebBrowseMessage
		/// ,IMChatMessage,StateUpdateMessage,AppSharingMessageEx,QAQuestion and QAAnswer PollAnswerMessage
		/// </summary>
		/// <param name="message">Message to send.</param>
		/// <returns>Number of clients having received this message.</returns>
		#region SendNonLoggedMessage--Commented Code
		/*(control.sessionID ==	Message.sessionID) && */

		#endregion

		public bool IfPostMeetingquestions(int meetingID, int a, int b)
		{
			
			//string sql="select * from ctool_evaluation,ctool_etomeeting where evaluation_ownerid=$memid and etomeeting_meetingid=$meetingid and evaluation_id=etomeeting_qid";
			string sql="select * from ctool_evaluation,ctool_etomeeting where etomeeting_meetingid="+meetingID.ToString();
			ArrayList results =  ConferenceServer.staticServerInstance.dbCon.GetRow(sql);
			if(results.Count>0)
			return true;
			else
			return false;

		}
		/// <summary>
		/// This method add the Datastructure for the minuts of meeting message to the data base on the 
		/// server side.
		/// </summary>
		/// <param name="msg_MM">  Minuts of meeting message Object</param>
		/// <returns></returns>
		public bool Add_MM_toServerDB(Mes_MinutsofMeeting msg_MM)
		{
			
			try
			{
				string query ="insert into ctool_minutsofmeeting (meetingid,host,presenter,datetime,module,imagepath,Text,Available,Mis) values("+msg_MM.meetingID+",'"+msg_MM.Host+"','"+msg_MM.Presenter+"','"+msg_MM.Current_DateTime+"','"+msg_MM.Module+"','"+msg_MM.ImagePath+"','"+msg_MM.Text+"',"+msg_MM.Available+",'"+msg_MM.Mis+"')";
				
				ConferenceServer.staticServerInstance.dbCon.Execute(query);			
				return true;
			}
			catch(Exception exp)
			{
			return false;
			}
	

		}


		public void SendNonLoggedMessage( MessageObject message)
		{  
			try
			{
				((IMessageReceiver) this._dispatcher.TransparentProxy).ReceiveMessage(message);
				///////////////////////////////////////TRANSCRIPT///////////////////////
				string curDate= DateTime.Now.Year + "-" + DateTime.Now.Month +"-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;

				if(message.GetType().Equals(typeof(DocumentSharing)))
				{
					//DocumentSharing Message = (DocumentSharing)message;
					if((((DocumentSharing)message).bEnabled== false) && hashCurSlide.Contains(((DocumentSharing)message).sessionID) == true )
					{
						hashCurSlide.Remove( ((DocumentSharing)message).sessionID );
						//isFirstSlide = true;
						//document sharing close
						addFlowMsgs(((DocumentSharing)message).ConferenceID ,curDate
							, ((DocumentSharing)message).senderProfile.Name +"  : has closed the document sharing where document id is "+ ((DocumentSharing)message).sessionID, "Launch Events" );
					}
					else if(  (((DocumentSharing)message).bEnabled== true)&& hashCurSlide.Contains(((DocumentSharing)message).sessionID) == false) /*if(control.sessionID ==	Message.sessionID)*/
					{
						object obj=new object();
						hashCurSlide.Add( ((DocumentSharing)message).sessionID, obj  );
						//hashCurSlide[((DocumentSharing)message).sessionID]
						//isFirstSlide = false;
						//document sharing start
						addFlowMsgs(((DocumentSharing)message).ConferenceID ,curDate
							, ((DocumentSharing)message).senderProfile.Name +" : has started the document sharing where document id is "+ ((DocumentSharing)message).sessionID, "Launch Events" );
					}	
					
					//iPrevSlide =  ((DocumentSharing)message).CurrentPage;

					addFlowMsgs(((DocumentSharing)message).ConferenceID ,curDate
						, ((DocumentSharing)message).senderProfile.Name  +" : has changed Slide, Current Slide is " + ((DocumentSharing)message).CurrentPage + " where document id is "+ ((DocumentSharing)message).sessionID  ,"Slide Changes" );

				}
				else if(message.GetType().Equals(typeof(WhiteboardMessage)))
				{	
					//WhiteboardMessage whiteBoardMsg = (WhiteboardMessage)message;
					

					if((message.MessageType & (ushort)MessageObjectType.MsgHideWhiteBoard) == (ushort)MessageObjectType.MsgHideWhiteBoard)	
					{	
						// whiteboard closed
						addFlowMsgs(((WhiteboardMessage)message).ConferenceID, curDate
							, ((WhiteboardMessage)message).SenderID  +" client id :has closed the Whiteboard ", "Launch Events" );
					}
					else if((message.MessageType & (ushort)MessageObjectType.MsgShowWhiteBoard) == (ushort)MessageObjectType.MsgShowWhiteBoard)	
					{	
						// whiteboard opened
						addFlowMsgs(((WhiteboardMessage)message).ConferenceID, curDate
							, ((WhiteboardMessage)message).SenderID  +" client id :has opened the Whiteboard ", "Launch Events" );
					}						
					
					//					else if((message.MessageType & (ushort)MessageObjectType.MsgAppSharingAnnotation) == (ushort)MessageObjectType.MsgAppSharingAnnotation)	
					//					{
					//					}
					//					else if((message.MessageType & (ushort)MessageObjectType.MsgWhiteboard) == (ushort)MessageObjectType.MsgWhiteboard)	
					//					{	
					//						//ClientUI.getInstance().JustOpenWhiteBoard();							
					//					}
					//					else
					//						ClientUI.getInstance().whiteBoard.eventStore.Add(message);
					//					
					

				}

					/*else if(message.GetType().Equals(typeof(DrawingClearMessage)))
					{
						DrawingClearMessage CMessage = (DrawingClearMessage)message;

						if(CMessage.m_ControlType == ControlType.ApplicationSharing)
						{
							//clear all drawing from application sharing 
							addFlowMsgs(message.ConferenceID ,curDate
								, CMessage.sessionID+" client id : has erased all drawing of application sharing ");
						}
						else if(CMessage.m_ControlType == ControlType.DrawingBoard)
						{	//clear all drawing from application DrawingBoard 
							addFlowMsgs(message.ConferenceID ,curDate
								, CMessage.sessionID +" client id : has erased all drawing of white Board ");
						}
						else if(CMessage.m_ControlType == ControlType.Browser)
						{	//clear all drawing from application Browser
							addFlowMsgs(message.ConferenceID ,curDate
								, CMessage.sessionID+" client id : has erased all drawing of Browser ");
						}
						else if(CMessage.m_ControlType == ControlType.DocumentSharing)
						{	//clear all drawing from application DocumentSharing
							addFlowMsgs(message.ConferenceID ,curDate
								, CMessage.sessionID +" client id : has erased all drawing of Document Sharing ");
						}
				
					}*/
				else if(message.GetType().Equals(typeof(MsgWebBrowseMessage)))
				{
					//MsgWebBrowseMessage webBrowse = (MsgWebBrowseMessage)message
					if( ((MsgWebBrowseMessage)message).active )
					{
						// new web browser has shared.
						addFlowMsgs(((MsgWebBrowseMessage)message).ConferenceID, curDate
							, ((MsgWebBrowseMessage)message).SenderID +" client id : has shared new web browser : id = " + ((MsgWebBrowseMessage)message).webBrowseID , "Launch Events" );
					}
					else
					{
						// close web browser has shared.
						addFlowMsgs(((MsgWebBrowseMessage)message).ConferenceID, curDate
							, ((MsgWebBrowseMessage)message).SenderID +" client id : has closed web browser : id = " + ((MsgWebBrowseMessage)message).webBrowseID , "Launch Events" );
					}
				}
				
				else if(message.GetType().Equals(typeof(IMChatMessage)))
				{
					IMChatMessage chatMsg = (IMChatMessage)message;
					
					Net.Sgoliver.NRtfTree.RtfTree  objRTF = new Net.Sgoliver.NRtfTree.RtfTree(); 
					objRTF.LoadRtfText(chatMsg.IMMessage); 		
					
					RtfTreeNode root = objRTF.RootNode;	
					RtfTreeNode node = new RtfTreeNode();

					string strText="";
					for(int i = 0; i < root.FirstChild.ChildNodes.Count; i++)
					{
						node = root.FirstChild.ChildNodes[i];
						if(node.NodeType == RTF_NODE_TYPE.TEXT)
						{
							strText += node.NodeKey.ToString();
						}
					}

					if ((message.MessageType & MessageObject.BroadcastMessageFlag) == 0)
					{
						//this is a private message
						addFlowMsgs(chatMsg.ConferenceID,curDate
							, chatMsg.sender + " says : " + strText +  " (private message for)" + chatMsg.RecipientId.ToString(), "Chat"  );
					}
					else
					{
						//this is a global message
						addFlowMsgs(chatMsg.ConferenceID ,curDate
							, chatMsg.sender +" says : "+ strText  , "Chat" );
					}
				}
				else if(message.GetType().Equals(typeof(StateUpdateMessage)))
				{
					for(int i=0; i<((WebMeeting.Common.StateUpdateMessage)(message)).dataArray.Count  ; i++)
					{
						if ( ((MessageObject ) ((WebMeeting.Common.StateUpdateMessage)(message)).dataArray[i]).GetType().Equals( typeof(AppSharingMessageEx)))
						{
							AppSharingMessageEx appMsg = (AppSharingMessageEx) ((MessageObject ) ((WebMeeting.Common.StateUpdateMessage)(message)).dataArray[i]);
							if(appMsg.isDesktopshareing == false )
							{
								if(appMsg.bEnabled == true )
								{
									if(bAppShared == false )
									{
										bAppShared = true;
										// new app sharing has started.
										addFlowMsgs(appMsg.ConferenceID , curDate
											, appMsg.SenderID+" client id : has started new Application sharing " , "Launch Events" );
									}
								}
								else
								{
									if(bAppShared == true )
									{
										bAppShared = false;
										// new desktop sharing has closed.
										addFlowMsgs(appMsg.ConferenceID , curDate
											, appMsg.SenderID+" client id : has Closed Application sharing " , "Launch Events" );
									}
								}
							}
						
						}
					}
				}
				else if(message.GetType().Equals(typeof(AppSharingMessageEx)))
				{
					AppSharingMessageEx appMsg = (AppSharingMessageEx)message ;
					if(appMsg.isDesktopshareing == false )
					{
						if(appMsg.bEnabled == true )
						{
							if(bAppShared == false )
							{
								bAppShared = true;
								// new app sharing has started.
								addFlowMsgs(appMsg.ConferenceID , curDate
									, appMsg.SenderID+" client id : has started new Application sharing " , "Launch Events" );
							}
						}
						else
						{
							if(bAppShared == true )
							{
								bAppShared = false;
								// new app sharing has closed.
								addFlowMsgs(appMsg.ConferenceID , curDate
									, appMsg.SenderID+" client id : has Closed Application sharing " , "Launch Events" );
							}
						}
					}
				}
				else if(message.GetType().Equals(typeof(QAQuestion)))
				{
					addFlowMsgs( ((QAQuestion)message).senderProfile.ConferenceID  , curDate
						, ((QAQuestion)message).senderProfile.Name +" has asked Question: " + ((QAQuestion)message).Question + " ? "  , "Attendee Question" );
				}
				else if(message.GetType().Equals(typeof(QAAnswer)))
				{
					addFlowMsgs( ((QAAnswer)message).senderProfile.ConferenceID , curDate
						, ((QAAnswer)message).senderProfile.Name +" has Answered : " + ((QAAnswer)message).Answer + " , where Question is : "+ ((QAAnswer)message).Question + " ? "  , "Attendee Question" );
				}
				else if(message.GetType().Equals(typeof(NewPollMessage)))
				{
					/*addFlowMsgs( ((NewPollMessage)message).ConferenceID , curDate
						, ((NewPollMessage)message).hostID +" client id : has polled : " + ((NewPollMessage)message).Question , "Polling Questions" );
						*/
				}
				else if(message.GetType().Equals(typeof(PollAnswerMessage)))
				{
					addFlowMsgs2( ((PollAnswerMessage)message).ConferenceID, curDate, 
						((PollAnswerMessage)message).questiontext,((PollAnswerMessage)message).choiceText,((PollAnswerMessage)message).clientID.ToString() , ((PollAnswerMessage)message).clientName , ((PollAnswerMessage)message).pollType.ToString(), ((PollAnswerMessage)message).choices);
				}

			}
			catch(Exception ex)
			{
				ex=ex;
			}
		}
		
		/// <summary>
		/// Sends message(desktop only) to all clients.
		/// </summary>
		/// <param name="message">Message to send.</param>
		/// <returns>Number of clients having received this message.</returns>
		public void SendNonLoggedMessage2( MessageObject message)
		{  
			try
			{
				((IMessageReceiver) this._dispatcher.TransparentProxy).ReceiveMessage(message);
			}
			catch
			{

			}
		}
		/// <summary>
		/// This method is used for Desktop-Sharing for particular receiver id.
		/// Currently, our application not doing this functionality as we have omitted the
		/// functionality of desktop-share.
		/// This function do two things
		/// First Send message to specified client
		/// Second Log either Desktop Sharing is Stopped/Started.
		/// </summary>
		/// <param name="obj">This is of type MessageObject</param>
		/// <param name="receiverConnectionID"></param>
		public void SendPrivateChatMessage(MessageObject obj,int receiverConnectionID)
		{
			ClientProfile cp = (ClientProfile)ClientList[receiverConnectionID];
			if(cp != null)
			{

				IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), cp.clientURI);
				iMessageReceiver.ReceiveMessage(obj);
				//	break;
			}

			/////////////Transcript 
			if(obj.GetType().Equals(typeof(DesktopSharingControlMessage )) )
			{
				DesktopSharingControlMessage DesktopMsg = (DesktopSharingControlMessage)obj;
			
				string curDate= DateTime.Now.Year + "-" + DateTime.Now.Month +"-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;

				if(DesktopMsg.Start == true && DesktopMsg.Status == 2)
				{
					// new desktop sharing has started.
					addFlowMsgs(DesktopMsg.ConferenceID, curDate
						, DesktopMsg.senderID+" client id : has started new Desktop sharing " , "Launch Events" );
				}
				else if(DesktopMsg.Start == false && DesktopMsg.Status == -1)
				{
					// desktop sharing has closed.
					addFlowMsgs(DesktopMsg.ConferenceID, curDate
						, DesktopMsg.SenderID+" client id : has Closed Desktop sharing ", "Launch Events" );
				}
			}
		}

		public void RemoveClientFromConference(string clientURI, bool gracefulExit)
		{
			IMessageReceiver iMessageReceiver=(IMessageReceiver)this._dispatcher.FindObjectByUri(clientURI);			
			
			foreach(ClientProfile cp in ClientList.Values)
			{				
				if(cp.clientURI==clientURI)
				{
					SendUserDisconnectedMessage(cp);
					ClientList.Remove(cp.ClientId);
					break;
				}

				Console.WriteLine(cp.Name + " has been removed fromc Client List");
			}


			//SendUserListUpdateMessage();
			//SendNonLoggedMessage(clientURI);//iMessageReceiver);			
		}

		public void GetUniqueSessionID(ref int id)
		{
			id=uniqueSessionID++;
		}

		public void GetManageContent(MessageObject msg)
		{
			ClientProfile prof =(ClientProfile) ClientList[msg.SenderID];
			if(prof != null)
			{
				WebContentRetrieveMessage Msg = (WebContentRetrieveMessage)msg;
				SendManageContents(prof);
			}

			
		}


		#endregion

		#region "Thread Functions"
		public void ConnectionThreadFunction()
		{
			Console.WriteLine("Thread is running.");
			while(m_bIsActive)
			{							
				Thread.Sleep(30000); //sleep for half minute
				ConnectionChechMessage msg = new ConnectionChechMessage(-1,DateTime.Now.ToFileTime());
				//for(int i=0;i<this.ClientList.Count;i++)
				try
				{
					foreach (ClientProfile p in ClientList.Values)
					{						
						//ClientProfile p = (ClientProfile)ClientList[i];
						IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), p.clientURI);					
						if(p.connectionCheckDateTime != 0)
						{
					
							DateTime dTemp = DateTime.FromFileTime(p.connectionCheckDateTime);									
							TimeSpan aSpan ;
							if(p.connectionCheckRecievedDateTime != 0)							
								aSpan = DateTime.FromFileTime(p.connectionCheckRecievedDateTime).Subtract(dTemp);					
							else
								aSpan = DateTime.Now.Subtract(dTemp);

							if(aSpan.TotalMilliseconds > 300 ) //criteria
							{							//slow
								p.m_ConnectionIndicator = ConnectionIndicator.Red;
							}
							else if((aSpan.TotalMilliseconds > 100 ) && (aSpan.TotalMilliseconds < 300))
							{
								p.m_ConnectionIndicator = ConnectionIndicator.Yellow;
							}
							else 
							{
								p.m_ConnectionIndicator = ConnectionIndicator.Green;
							}
							SendUserUpdateMessage(p);
						}
						p.connectionCheckDateTime = msg.sentTickCount;				
						iMessageReceiver.ReceiveMessage(msg);					
					}					
				}	
				catch(Exception ex)
				{
					ex=ex;
					//ClientList.Remove(p.ClientId);	
				}
				SendNonLoggedMessage(msg); // send the message to all clients
				msg = null;

			}
		}

		#endregion
	
		#region "Genuine Channels Portion"


		private Dispatcher _dispatcher = new Dispatcher(typeof(IMessageReceiver));
		private ArrayList FailedReciverURIList=new ArrayList();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dispatcher"></param>
		/// <param name="message"></param>
		/// <param name="resultCollector"></param>
		public void BroadcastCallFinishedHandler(Dispatcher dispatcher, IMessage
			message, ResultCollector resultCollector)
		{
			// analyze broadcast results
		}



		public void showFailReciverList(string strCalledMethod)
		{
			try
			{
				Console.WriteLine("Called by method :::: " + strCalledMethod);
				lock(FailedReciverURIList)
				{
					Console.WriteLine("***** Meeting ID ::: *******  "+strMeetingId);
					Console.WriteLine("*********** Start Failed Reciver List****************");
					Console.WriteLine("***********************************************");
					for(int i=0;i<FailedReciverURIList.Count;i++)
					{
						Console.WriteLine(FailedReciverURIList[i].ToString());
					}
					Console.WriteLine("***********************************************");
					Console.WriteLine("*********** End Failed Reciver List****************");
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine("Exception in showFailReciverList " + ex.ToString());
			}
		}

		/// <summary>
		/// Send a control message to all the attendees in the conference
		/// </summary>
		/// <param name="ctrlCode"> Control code identifying this message </param>
		/// <param name="param"> Any extra info needed by the clients </param>
		public void BroadcastControlMessage(ControlCode ctrlCode, Object param)
		{
			try
			{
				ControlMessage ctrlMsg;
				//
				// Construct a control message
				//
				ctrlMsg = new ControlMessage(ctrlCode);
				ctrlMsg.Parameter = param;
				ctrlMsg.SenderID=-1;

				//
				// Send the message to all the clients in the conference
				//
				((IMessageReceiver) this._dispatcher.TransparentProxy).ReceiveMessage(ctrlMsg);		
				ctrlMsg = null; //GC
				GC.Collect(); //GC
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		private void _dispatcher_BroadcastReceiverHasBeenExcluded(MarshalByRefObject marshalByRefObject, ReceiverInfo receiverInfo)
		{
			if(receiverInfo!=null)
			{
				lock(FailedReciverURIList)
				{					
					FailedReciverURIList.Add(receiverInfo.MbrUri+"  receiver fails "+receiverInfo.NumberOfFails.ToString());
				}
				showFailReciverList("private void _dispatcher_BroadcastReceiverHasBeenExcluded(MarshalByRefObject marshalByRefObject, ReceiverInfo receiverInfo)");
			}
		}	
		
		
		#endregion

		#region "Document Conversion relevant things"
		public void ConvertBMPToJPG(string folder)
		{
			string[] files = Directory.GetFiles(folder,"*.bmp");
			System.Drawing.Imaging.ImageCodecInfo		imageCodecInfo;
			Encoder				encoder;
			EncoderParameter	encoderParameter;
			EncoderParameters	encoderParameters;

			imageCodecInfo = GetEncoderInfo("image/jpeg");

			encoder = Encoder.Quality;	
			encoderParameters = new EncoderParameters(1);

			// Save the bitmap as a JPEG file with quality level 70.
			encoderParameter = new EncoderParameter(encoder, 70L);
			encoderParameters.Param[0] = encoderParameter;

			for(int i=0;i<files.Length;i++)
			{
				try
				{					
					Image bitmap2=Image.FromFile(files[i]);
				
					// Save the image using the JPEG encoder
					// with the specified Quality level.
					bitmap2.Save(Path.ChangeExtension(files[i],".jpg"), imageCodecInfo, encoderParameters);
					bitmap2.Dispose();
      
					//ima.Save(Path.GetFileName(files[i]),System.Drawing.Imaging.ImageFormat.Jpeg);
					File.Delete(files[i]);
				}
				catch(Exception ee)
				{
					ee=ee;
				}	
			}
			encoderParameter = null;//GC
			encoderParameters = null;//GC
			GC.Collect(); //GC

		}
		/// <summary>
		/// Convert Pdf Document.
		/// </summary>
		/// <param name="strPath"></param>
		/// <param name="savePath"></param>
		/// <returns></returns>
		private bool ConvertPDFDocument(string strPath,string savePath)
		{
			try
			{	
				string parameters = "-q -m -r -c -i \"" + strPath + "\" -o \"" + savePath + "\"";
				ProcessStartInfo pInfo = new ProcessStartInfo(System.Windows.Forms.Application.StartupPath + "\\pdf2html.exe",parameters);
				pInfo.WindowStyle = ProcessWindowStyle.Hidden;
				//pInfo.CreateNoWindow = true;				
				Process p = Process.Start(pInfo);
				p.WaitForExit();
				return true;
			}
			catch(Exception ee)
			{								
				return false;
			}			
		}
		/// <summary>
		/// Convert Presentation Document.
		/// </summary>
		/// <param name="strPath"></param>
		/// <param name="savePath"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		private bool ConvertPresentationDocument(string strPath, string savePath,ref DocumentSharing msg)
		{
			try
			{
				PowerPoint.Application	ppApp;
				PowerPoint.PpSaveAsFileType format;
				format= PowerPoint.PpSaveAsFileType.ppSaveAsJPG;//.ppSaveAsJPG;		
				try
				{
					
					ppApp= new PowerPoint.Application();									
					ppApp.Presentations.Open(strPath,Microsoft.Office.Core.MsoTriState.msoCTrue,Microsoft.Office.Core.MsoTriState.msoCTrue,Microsoft.Office.Core.MsoTriState.msoFalse);					
				}
				catch(Exception ee)
				{
					Console.Write("Unable to convert PowerPoint document " + ee.Message);
					return false;
				}
				
				try
				{
					ppApp.Presentations.Item(1).SaveAs(savePath,format,Microsoft.Office.Core.MsoTriState.msoCTrue);	
					ppApp.Presentations.Item(1).Close();
					//ConvertBMPToJPG(savePath);
					ppApp.Quit();
					System.Threading.Thread.Sleep(500);
					string[] files = Directory.GetFiles(savePath,"*.jpg");
					if(files != null)
					{
						msg.TotalPages = files.Length;
						//Console.WriteLine("No of Files in presentation ::::"+files.Length.ToString());
					}
					else
					{
						msg.CurrentPage = 1;
						msg.TotalPages =1;
					}
					//Thread.Sleep(500);
					return true;

				}
				catch(Exception ee)
				{
					string errorText = "Unable to save specified Presentation. Access is denied. "+ ee.Message;                    
					Console.Write("Unable to convert Powerpoint Document " + errorText);
					return false;
				}
			}
			catch(Exception ee)
			{				
				Console.Write("Unable to Convert PowerPoint Document. " + ee.Message);
				return false;
			}           											
		}
		/// <summary>
		/// Convert Word Document
		/// </summary>
		/// <param name="strPath"></param>
		/// <param name="savePath"></param>
		/// <returns></returns>
		private bool ConvertWordDocument(string strPath,string savePath)
		{
			try
			{
				Word.Application wordApp = new Word.Application();			
				object rdOnly=true;//Boolean.TrueString;
				object Source=strPath;					
				object Destination= savePath;

				try
				{
					
					wordApp.Documents.Open(ref Source,ref Unknown, 
						ref rdOnly,ref Unknown,ref Unknown, 
						ref Unknown,ref Unknown,ref Unknown, 
						ref Unknown,ref Unknown,ref Unknown, 
						ref Unknown,ref Unknown,ref Unknown,ref Unknown);//,ref Unknown);
				}
				catch(Exception ee)
				{
					Console.Write("Unable to convert Word Document " + ee.Message);
					return false;
				}

				try
				{

					object format = Word.WdSaveFormat.wdFormatHTML;//.wdFormatRTF;// kein XML, nutzen?
					wordApp.ActiveDocument.SaveAs(ref Destination,ref format, 
						ref Unknown,ref Unknown,ref Unknown, 
						ref Unknown,ref Unknown,ref Unknown, 
						ref Unknown,ref Unknown,ref Unknown, 
						ref Unknown,ref Unknown,ref Unknown, 
						ref Unknown, ref Unknown);
					
				}
				catch(Exception ee)
				{					
					string errorText = "Unable to save specified Document. Access is denied. "+ ee.Message;                    
					Console.Write("Unable to convert Word Document " + errorText);
					return false;
					
				}
				object fileSave=false;
				
				wordApp.Quit(ref fileSave, ref Unknown, ref Unknown);


				wordApp = null; //GC
				GC.Collect(); //GC
				return true;
			}
			catch(Exception ee)
			{

			}
			return false;
		}
		/// <summary>
		/// Convert Excel Document
		/// </summary>
		/// <param name="strPath"></param>
		/// <param name="savePath"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		private bool ConvertExcelDocument(string strPath,string savePath,ref DocumentSharing msg)
		{
			try
			{
				if(ExcelDocument(strPath,savePath))
				{
					System.Threading.Thread.Sleep(1000);

					string temp2= Directory.GetParent(savePath).FullName + "\\" + Path.GetFileNameWithoutExtension(strPath) + "_Files";
					Console.WriteLine("Remote Directory of Excel..... "+temp2);					
					try					
					{
						msg.TotalPages = 1;
						msg.CurrentPage = 1;
						if(Directory.Exists(temp2))
						{
							string[] strfiles = Directory.GetFiles(temp2,"sheet*.htm");
							if(strfiles != null)
							{
								foreach (string strfile in strfiles)
								{
									byte[] buffer = ReadFile(strfile);
									string str ;//= buffer.ToString();

									System.Text.UTF8Encoding a = new System.Text.UTF8Encoding();
									str =a.GetString(buffer,0,buffer.Length);
									string toFind = "window.name!=\"frSheet\"";
									int nStart = str.IndexOf(toFind);
									str  = str.Replace(toFind,"1 != 1");
									WriteFile(str,strfile);									
								}
								//Console.WriteLine("No of Sheet Files::::" + strfiles.Length.ToString());
								msg.TotalPages = strfiles.Length;
								msg.CurrentPage = 1;
							}
							else
							{
								msg.TotalPages = 1;
								msg.CurrentPage = 1;
							}
						}
						return true;
					}
					catch(Exception ee)
					{
						Console.WriteLine("Exception in the procedure of Excel Documentation"+ ee.ToString());							
					}
				}

			}
			catch(Exception ee)
			{
			}
			return false;

		}
		private bool ExcelDocument(string strPath,string savePath)
		{
			try
			{
				Excel.Application excelApp = new Excel.ApplicationClass();				
				excelApp.Visible = false;
				try
				{
					Excel.Workbook newWorkbook = excelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);			
				}
				catch(Exception ee)
				{		
					string errorText = "Unable to open specified WorkBook. Access is denied. "+ ee.Message ;                    				
					Console.Write(errorText +"\n");
					return false;					
				}				
				try
				{
					Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(strPath,
						0, true, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
						true, false, 0, true, false, false);				
					try
					{
						excelWorkbook.SaveAs(savePath,Excel.XlFileFormat.xlHtml,"","",false,false,XlSaveAsAccessMode.xlNoChange,false,false,false,false,true);
					}
					catch(Exception ee)
					{
						string errorText = "Unable to open specified WorkBook. Access is denied. "+ ee.Message ;                    				
						Console.Write(errorText +"\n");
						return false;	
					}
					excelApp.Quit();

					excelApp = null; //GC
					GC.Collect(); //GC
					return true;
				}
				catch(Exception ee)
				{
					string errorText = "Unable to open specified WorkBook. Access is denied. "+ ee.Message ;                    				
					Console.Write(errorText +"\n");
					return false;			
				}				
				return false;
			}
			catch(Exception ee)
			{
			}
			return false;
		}
		private void GetFilesofDirectories(string strPath,ArrayList files)
		{		
			try
			{				
				string[] Directories = Directory.GetDirectories(strPath);
				if(Directories != null)
				{
					foreach(string dir in Directories)
					{							
						GetFilesofDirectories(dir,files);						
					}
				}		
				string[] DirFiles = Directory.GetFiles(strPath);
				if(DirFiles != null)
				{				
					foreach(string file in DirFiles)
					{	
						files.Add(file);
					}					
				}
			}               								
			catch(Exception ee)
			{
			}
		}
				
		private void CreateDirectory(string strPath)
		{
			try
			{
				if(!Directory.Exists(strPath))
					Directory.CreateDirectory(strPath);
			}
			catch(Exception ee)
			{
			}
		}

		private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			int j;
			System.Drawing.Imaging.ImageCodecInfo[] encoders;
			encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
			for(j = 0; j < encoders.Length; ++j)
			{
				if(encoders[j].MimeType == mimeType)
					return encoders[j];
			}
			return null;
		}
		
		public byte[] GetBytes(string str)
		{
			byte[] br = new byte[str.Length];
			for(int i = 0 ;i < str.Length   ; i++)
			{
				br[i] = (byte)str[i];
			}
			return br;
		}
		public void WriteFile(string buffer,string path)
		{
			try
			{
				FileStream fr = File.OpenWrite(path);

				fr.Write(GetBytes(buffer),0,buffer.Length);

				fr.Close();
			}
			catch(Exception ee)
			{
				ee = ee;
			}			
		}
		public byte[] ReadFile(string path)
		{
			FileStream fr = File.OpenRead(path);
			byte[] buffer = new byte[fr.Length];

			fr.Read(buffer,0,buffer.Length);

			fr.Close();

			return buffer;		

			
		}
		#endregion
		
		#region "Logging Stuff"
		/// <summary>
		/// Method used for logging following tpyes of messages
		/// DocumentSharing,WhiteboardMessage,MsgWebBrowseMessage
		/// ,IMChatMessage,StateUpdateMessage,AppSharingMessageEx,QAQuestion and QAAnswer		 
		/// This method do two things
		/// First its Instantiate ST_flowMsg object with passed parameters to this method
		/// Second it add ST_flowMsg  type object to "flowMsgs" array-list of ConferenceServer for consume by
		/// insertFlowThread.
		/// </summary>
		/// <param name="confID"></param>
		/// <param name="datetime"></param>
		/// <param name="curMessage"></param>
		/// <param name="msgCategory"></param>
		public void addFlowMsgs(string confID ,string datetime, string curMessage, string msgCategory)
		{
//			
//			try
//			{
//				ConferenceServer.ST_flowMsg FlowMsgObj = new  ConferenceServer.ST_flowMsg ();
//				FlowMsgObj.meetID = confID;
//				FlowMsgObj.curDate= datetime;
//				FlowMsgObj.curMsg = curMessage;
//				FlowMsgObj.MsgCategory = msgCategory ;
//				/*FlowMsgObj.question = question;
//				FlowMsgObj.answer = answer;
//				FlowMsgObj.userid = userid;
//				FlowMsgObj.usertype = usertype;
//				FlowMsgObj.qtype = qtype;*/
//				
//				ConferenceServer.staticServerInstance.flowMsgs.Add(FlowMsgObj);
//				FlowMsgObj = null;
//			}
//			catch(Exception ex)
//			{
//				ex=ex;
//			}
		}
		/// <summary>
		/// Method used for logging following tpyes of messages
		/// PollAnswerMessage
		/// This method do two things
		/// First its Instantiate ST_LogPolling object with passed parameters to this method
		/// Second it add ST_LogPolling type object to "flowMsgs" array-list of ConferenceServer for consume by
		/// insertFlowThread.
		/// </summary>
		/// <param name="confID"></param>
		/// <param name="datetime"></param>
		/// <param name="curMessage"></param>
		/// <param name="msgCategory"></param>		
		public void addFlowMsgs2(string confID ,string datetime,string question , string answer, string userid, string username, string qtype,string mchoice)
		{
//
//			try
//			{
//				ConferenceServer.ST_LogPolling LogPolling = new  ConferenceServer.ST_LogPolling ();
//				LogPolling.meetID = confID;
//				LogPolling.curDate = datetime;
//				LogPolling.question = question;
//				LogPolling.answer = answer;
//				LogPolling.userid = userid;
//				LogPolling.username= username;
//				LogPolling.qtype = qtype;
//				LogPolling.mchoice = mchoice;
//				
//				ConferenceServer.staticServerInstance.flowMsgs.Add(LogPolling);
//				LogPolling = null;
//			}
//			catch(Exception ex)
//			{
//				ex=ex;
//			}
		}

		#endregion		
		
		#region "uncategorized Method"
		/// <summary>
		///This function do the following things
		///-increment the NextClientId		
		///-Assign Profile.ClientId=i
		/// Attaches the client.
		/// 
		///
		/// Fetches the URI of the remote host that sent a request. 
		/// </summary>
		/// <param name="nickname">Nickname.</param>
		public int AttachClient(string nickname, ClientProfile profile, MessageObject msg)
		{
			try
			{

				int id=NextClientId++;
				profile.ClientId=id;
				/*
				 * this.restartTimer = true;
				 *is commented because its not using anywhere in the code.				 
				 */
				//this.restartTimer = true;
				showFailReciverList("public int AttachClient(string nickname, ClientProfile profile, MessageObject msg)");
				
				// change relevant to close meeting.							
				//				if(conferenceCloseThread.ThreadState == System.Threading.ThreadState.Running)
				//					this.conferenceCloseThread.Suspend();


				bHostJoined =(profile.clientType == ClientType.ClientHost);
				
				strMeetingId=profile.ConferenceID.ToString();				

				string receiverUri = GenuineUtility.FetchCurrentRemoteUri() + "/MessageReceiver.rem";
				IMessageReceiver iMessageReceiver = (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), receiverUri);
			
				profile.clientURI=receiverUri;
				GenuineUtility.CurrentSession["ConnectionID"] = id;
				//	Console.WriteLine("Client with nickname \"{0}\" has been registered{1}.", nickname,NextClientId);
				while(m_bLocked==true)
				{
					Console.WriteLine("Meeting jammed in while loop in attachclient()line 1710 & meetingID->" + strMeetingId +  " Participant Name ->"+profile.Name );
					System.Threading.Thread.Sleep(1000);
				}
				m_bLocked=true;

			
				this._dispatcher.Add((MarshalByRefObject) iMessageReceiver);
				this.ClientList.Add(profile.ClientId,profile);
				m_bLocked=false;
				iMessageReceiver.ReceiveMessage(msg);// sending the authentication message							
				/*
				 * When host is joined, its color should be Red.
				 * colorsArray[0] is red color				 
				 */				
				if(profile.clientType==ClientType.ClientHost)
				{
					profile.m_AssignedColor = colorsArray[0];	
				}
				else
				{
					if(mcount==0)//checking index for assigning attendee
					{			//if 0 index then increment it
						profile.m_AssignedColor = colorsArray[mcount+1];
					}
					else //Simple Assign index because assinging will not 
					{	//be red color					
						profile.m_AssignedColor = colorsArray[mcount];	
					}
				}
				SendUserJoinMessage(profile); 
				mcount++;
				if(mcount >= 15)
					mcount = 0;
				
				//Send all profile Lists to joing participant.
				ServerInfoMessage infoMsg = new ServerInfoMessage();
				infoMsg.SenderID=-1;			
				infoMsg.AttendeeProfileList = ClientList;
				iMessageReceiver.ReceiveMessage(infoMsg); // senfing the info message		

				
				// It should be removed 
				SendConferenceParameters(iMessageReceiver);									        
				
				return id;
			}
			catch(Exception ex)
			{
				m_bLocked=false;
				Console.WriteLine("Exception Message Occured at AttachClient-> "+ex.Message.ToString()+" Exception  source :::"+ex.StackTrace+" Exception StackTrace ::: "+ex.StackTrace+" Exception Tostring ::: "+ex.ToString());
				m_bLocked=false;
				return 0;

			}

		}

		public int DetachUser(WebMeeting.Common.ClientProfile profile)
		{				
			this.ClientList.Remove(profile.ClientId);			   			
			string receiverUri =profile.clientURI;
			IMessageReceiver iMessageReceiver = (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), receiverUri);
			this._dispatcher.Remove((MarshalByRefObject) iMessageReceiver);
			SendUserDisconnectedMessage(profile);			
			return 0;
		}
		/// <summary>
		/// Send User joining message  to all participant  in the meeting
		/// This method is called from AttachClient.
		/// </summary>
		/// <param name="newlyJoined">profile of new comming participant</param>
		public void SendUserJoinMessage(WebMeeting.Common.ClientProfile newlyJoined)
		{

			ControlMessage m = new ControlMessage(ControlCode.AttendeeJoin);
			m.SenderID =-1; 
			m.Parameter= newlyJoined;
			((IMessageReceiver) this._dispatcher.TransparentProxy).ReceiveMessage(m);

			m = null; //GC
			GC.Collect(); //GC

		}

		private void SendUserUpdateMessage(WebMeeting.Common.ClientProfile clientUpdated)
		{
			ControlMessage m = new ControlMessage(ControlCode.AttendeeUpdate);
			m.Parameter = clientUpdated;
			((IMessageReceiver) this._dispatcher.TransparentProxy).ReceiveMessage(m);			
			m = null; //GC
			GC.Collect(); //GC
		}

		/// <summary>
		/// Send attendee dropped message against a profile passed it as parameter to all clients in a meeting
		/// </summary>
		/// <param name="clientDropped">clientDropped</param>
		private void SendUserDisconnectedMessage(WebMeeting.Common.ClientProfile clientDropped)
		{
			ControlMessage m = new ControlMessage(ControlCode.AttendeeDropped);
			m.Parameter = clientDropped;
			((IMessageReceiver) this._dispatcher.TransparentProxy).ReceiveMessage(m);
			m = null; //GC
			GC.Collect(); //GC
		}	
		
		/// <summary>
		/// Functionality:This method do following things
		///               1-SenderID=-1 represent its send by server
		///               2-Send all participant list of running conference by assinging infoMsg.AttendeeProfileList=ClientList
		///                 infoMsg.AttendeeProfileList is HashTable
		///               3-Receive message to a particular id
		/// Status : This function is not using anywhere in the code.
		///                     
		/// </summary>
		/// <param name="receiver"></param>
		private void SendUserListUpdateMessage(IMessageReceiver receiver)
		{
			ServerInfoMessage infoMsg = new ServerInfoMessage();
			infoMsg.SenderID=-1;
			infoMsg.AttendeeProfileList = ClientList;		
			//	((IMessageReceiver) this._dispatcher.TransparentProxy).ReceiveMessage(infoMsg);
			receiver.ReceiveMessage(infoMsg);
			infoMsg = null; // GC
		}

		
		
		/// <summary>
		/// This method is used long before for sending logging messages
		/// of Document Sharing,Web Sharing etc.
		/// It should be removed from the code as per discussion. 
		/// </summary>
		/// <param name="receiver"></param>
		private void SendConferenceParameters(IMessageReceiver receiver)
		{
			StateUpdateMessage stateMessage2=new StateUpdateMessage();
			stateMessage2.updateType=UpdateTypeCode.UpdateMixedMessages;
			stateMessage2.SenderID=-1;
			Console.WriteLine("Logged Messages Count = " + LoggedMessages.Count);			
			stateMessage2.dataArray=(ArrayList)this.LoggedMessages.Clone();
			receiver.ReceiveMessage(stateMessage2);	
	
			stateMessage2 = null; //GC
			GC.Collect(); //GC

			if(this.DocumentSharingList.Count > 0) //Send the document sharing messages
			{// Todo::Ahmed u can look at it. I've stored documentsharing messages in a hashtable
				// so that there is only one message of a particular prestation session. this reduces 
				// the the flickering at the client side.
				StateUpdateMessage stateMessage3=new StateUpdateMessage();
				stateMessage3.updateType=UpdateTypeCode.UpdateMixedMessages;
				stateMessage3.SenderID=-1;
				System.Collections.ICollection collection =  this.DocumentSharingList.Values;
				stateMessage3.dataArray = new ArrayList(collection);			
				receiver.ReceiveMessage(stateMessage3);		

				stateMessage3 = null; //GC
			}
			else if(this.WebBrowserList.Count > 0)
			{
				StateUpdateMessage stateMessage3=new StateUpdateMessage();
				stateMessage3.updateType=UpdateTypeCode.UpdateMixedMessages;
				stateMessage3.SenderID=-1;
				System.Collections.ICollection collection =  this.WebBrowserList.Values;
				stateMessage3.dataArray = new ArrayList(collection);			
				receiver.ReceiveMessage(stateMessage3);		

				stateMessage3 = null; //GC
			}		
			GC.Collect(); //GC
			if(msgSynchronization != null)
				receiver.ReceiveMessage(msgSynchronization);
			
		}
		
		
		/// <summary>
		/// Used to terminate the "connectionCheckThread" Thread--its thread method  "public void ConnectionThreadFunction()"		
		/// </summary>
		public void ShutDownConference()
		{
				m_bIsActive = false;			
		}
		
	
		
		public void checkConnectionUrgent(int clientID)
		{
			try
			{
				ConnectionChechMessage msg = new ConnectionChechMessage(-1,DateTime.Now.ToFileTime());				
					
				ClientProfile p = (ClientProfile)ClientList[clientID];

				IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), p.clientURI);					
				SendUserUpdateMessage(p);
				//iMessageReceiver.ReceiveMessage(msg);

				SendNonLoggedMessage(msg); // send the message to all clients
			}
			catch(Exception ex)
			{
				ex=ex;
			}
		}
			
		#endregion


		#region ConferenceRoom Constructors
		/// <summary>
		/// Create a conference with the specified Conference Id
		/// </summary>
		/// <param name="confId"></param>
		public ConferenceRoom(String confId)
		{
			//
			// TODO: We should use a default capacity. Based on config info
			//
			mcount = 0;
			colorsArray = new Color[] { Color.Red,Color.Blue,Color.Yellow,Color.Green,Color.Tomato,
										  Color.AliceBlue,Color.Purple,Color.YellowGreen,Color.PaleTurquoise,
										  Color.Teal,Color.Orange,Color.Black,Color.DarkSlateGray,Color.PeachPuff,
										  Color.Violet}										  ;			
			ConferenceId = confId;
			ClientList = Hashtable.Synchronized(new Hashtable());
			WhiteBoardMessagesList=new ArrayList();
			CompoundMessageList = new ArrayList();
			ChatMessages = new ArrayList();
			LoggedMessages = new ArrayList();
		
			SharedFiles= new ArrayList();											
			SharedPolls=new ArrayList();
			WebBrowserMessages=new ArrayList();
			QAQuestionMessages = new ArrayList();

			uniqueSessionID=1;
			
			//	PresntationList=Hashtable.Synchronized(new Hashtable());
			VideoList=Hashtable.Synchronized(new Hashtable());
			AudioList=Hashtable.Synchronized(new Hashtable());
			DesktopList=Hashtable.Synchronized(new Hashtable());
			DocumentSharingList = Hashtable.Synchronized(new Hashtable());
			WebBrowserList= Hashtable.Synchronized(new Hashtable());
			/*
			 * Represents a value indicating the maximum number of failures allowing to specific remote 
			 * receiver before switching it to simulation mode. That is as soon as a true broadcast sender 
			 * will not be able to deliver a message consecutively for this number of tries to the specific 
			 * receiver, it will automatically start sending messages to this specific receiver via the usual 
			 * channel. Set it to zero to prevent switching to simulation mode. I would recommend to set a 
			 * large value (for example, 100) in this case. Remember, each time the receiver sends successful 
			 * response to the message sent via true multicast channel, this counter is resetted to zero and 
			 * receiver is switched back to normal mode when messages are delivered via "true" multicast
			 *  channel.
			 * */
			//this._dispatcher.MaximumNumberOfConsecutiveFailsToEnableSimulationMode=1000;
			/*
			 * Gets or sets a value indicating the maximum allowed number of consecutive failures that can be made by a receiver. As soon as the receiver consecutively fails for this number of time, it is automatically removed from the receiver list. You can set this value to zero in order to prevent automatic excluding. I would recommend to set a large value (for example, 100) in this case. Remember, each time the receiver sends successful response, counter for this receiver is resetted to zero.
			 */
			this._dispatcher.MaximumNumberOfConsecutiveFailsToExcludeReceiverAutomatically=1000;
			this._dispatcher.BroadcastCallFinishedHandler += new BroadcastCallFinishedHandler(this.BroadcastCallFinishedHandler);
			this._dispatcher.BroadcastReceiverHasBeenExcluded+=new BroadcastReceiverHasBeenExcludedEventHandler(_dispatcher_BroadcastReceiverHasBeenExcluded);
			this._dispatcher.CallIsAsync = true;
			/*
			 * The Assumptions & reasons on which we removed this thread
			 */
			/*
			 *1-- when new client is connecting to the server its get  all the participants list that are in particular conference.
			 *2-- if Client Attach successfully to server then server sends the new connected user to all participants to add in their participant list.
			 *3-- when host changes the status of the any paricipant from attendee to Presenter or give some rights then host send all messages to all participant of that particular meeting.
			 *-------------------------------
			 *------------------------------- NOTE
			 *---------- After discussing above three points there is no need to run this thread except that
			 *---------  "if some packet drops then connection thread function update the participants list of that client, connection thread function runs after every 30 seconds"
			 */

			/*code is recomment caz there are comming problem in rights updation on different clients 
			 * eg. Suppose, there are five attendee in a meeting, host make one attendee to presenter back and forth two time then sometime attendee status not updated at some participant sides.  
			 * */
			connectionCheckThread = new Thread(new ThreadStart(ConnectionThreadFunction));
			connectionCheckThread.Name = "ConnectionThreadFunction Thread : ConferenceRoom(string)";
			connectionCheckThread.Start();
			
			ConferenceServer.staticServerInstance.getConfig();
			// change relevant to close meeting.							
			//			conferenceCloseThread = new Thread(new ThreadStart(CloseConferenceTimerHandler));
		}

		public ConferenceRoom()
		{
			mcount = 0;
			// bind server's methods
			this._dispatcher.BroadcastCallFinishedHandler += new BroadcastCallFinishedHandler(this.BroadcastCallFinishedHandler);
			this._dispatcher.CallIsAsync = true;
		
				
			/*this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.richTextBox1.Name = "richTextBox1";
			is commented because its not using anywhere
			*/
			//this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			//this.richTextBox1.Name = "richTextBox1";
			
		}									

		#endregion
		
		#region CommentedCode in Review of joiningProcess
		//		public void CloseConferenceTimerHandler()
		//		{
		//			try
		//			{
		//				while(m_bIsActive)
		//				{
		//					//Thread.Sleep(60 *1000 * 15); //15 minutes
		//					Thread.Sleep(3600000); //15 minutes
		//					if(restartTimer)
		//					{                    
		//						restartTimer = false;
		//						continue;
		//					}				
		//					//Server.ConferenceServer.staticServerInstance.CloseConference(this,true);
		//				}
		//			}
		//			catch(Exception ex)
		//			{
		//				ex=ex;
		//			}
		//		}		
		#endregion

		#region Commented Code in Review of MYSQL Module
		/// <summary>
		/// this method is commented in the review of MYSQL Module.
		/// </summary>
		/// <param name="profile"></param>
		public void SendManageContents(ClientProfile profile)
		{
			//			try
			//			{
			//				WebIntegeration.WebPolls webPolls = new WebPolls(ConferenceServer.staticServerInstance.dbCon);
			//				if(webPolls.GetAllPolls(profile.ClientRegistrationId.ToString()))
			//				{
			//					StateUpdateMessage stateMessage=new StateUpdateMessage();
			//					stateMessage.updateType=UpdateTypeCode.UpdateWebPolls;			
			//					stateMessage.dataArray = webPolls.WebPollArray;
			//					IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), profile.clientURI);
			//					iMessageReceiver.ReceiveMessage(stateMessage);					
			//					stateMessage = null; //GC
			//				}
			//				else
			//				{
			//					StateUpdateMessage stateMessage=new StateUpdateMessage();
			//					stateMessage.updateType=UpdateTypeCode.UpdateWebPolls;			
			//					stateMessage.dataArray = new ArrayList();
			//					IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), profile.clientURI);
			//					iMessageReceiver.ReceiveMessage(stateMessage);					
			//		
			//					stateMessage = null; //GC
			//				}
			//				GC.Collect(); //GC
			//				
			//				ArrayList webBookmarks = WebIntegeration.WebBookMarks.GetWebBookMarks(ConferenceServer.staticServerInstance.WEBSERVER_PRESENTATION_PATH,profile.ClientRegistrationId.ToString(),ConferenceServer.staticServerInstance.dbCon);
			//				if(webBookmarks.Count > 0)
			//				{
			//					StateUpdateMessage stateMessage=new StateUpdateMessage();
			//					stateMessage.updateType=UpdateTypeCode.UpdateWebBookmarks;			
			//					stateMessage.dataArray = webBookmarks;
			//					IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), profile.clientURI);
			//					iMessageReceiver.ReceiveMessage(stateMessage);					
			//					stateMessage = null; //GC
			//				}
			//				else
			//				{
			//				
			//					StateUpdateMessage stateMessage=new StateUpdateMessage();
			//					stateMessage.updateType=UpdateTypeCode.UpdateWebBookmarks;			
			//					stateMessage.dataArray = new ArrayList();
			//					IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), profile.clientURI);
			//					iMessageReceiver.ReceiveMessage(stateMessage);					
			//
			//					stateMessage = null; //GC
			//				}
			//				GC.Collect(); //GC
			//				ArrayList webPresentations = WebIntegeration.WebPresentations.GetWebPresentations(ConferenceServer.staticServerInstance.WEBSERVER_PRESENTATION_PATH,profile.ClientRegistrationId.ToString(),ConferenceServer.staticServerInstance.dbCon);
			//				if(webPresentations.Count > 0)
			//				{
			//					StateUpdateMessage stateMessage=new StateUpdateMessage();
			//					stateMessage.updateType=UpdateTypeCode.UpdateWebPresentations;			
			//					stateMessage.dataArray = webPresentations;
			//					IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), profile.clientURI);
			//					iMessageReceiver.ReceiveMessage(stateMessage);					
			//					stateMessage = null; //GC
			//				}
			//				else
			//				{
			//				
			//					StateUpdateMessage stateMessage=new StateUpdateMessage();
			//					stateMessage.updateType=UpdateTypeCode.UpdateWebPresentations;			
			//					stateMessage.dataArray = new ArrayList();
			//					IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), profile.clientURI);
			//					iMessageReceiver.ReceiveMessage(stateMessage);					
			//					stateMessage = null; //GC
			//
			//				}
			//				GC.Collect(); //GC
			//				ArrayList webFiles = WebIntegeration.WebFiles.GetWebFiles(ConferenceServer.staticServerInstance.WEBSERVER_WEBFILES_PATH,profile.ClientRegistrationId.ToString(),ConferenceServer.staticServerInstance.dbCon);
			//				if(webFiles.Count > 0)
			//				{
			//					StateUpdateMessage stateMessage=new StateUpdateMessage();
			//					stateMessage.updateType=UpdateTypeCode.UpdateWebFiles;			
			//					stateMessage.dataArray = webFiles;
			//					IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), profile.clientURI);
			//					iMessageReceiver.ReceiveMessage(stateMessage);					
			//					stateMessage = null; //GC
			//				}
			//				else
			//				{
			//					StateUpdateMessage stateMessage=new StateUpdateMessage();
			//					stateMessage.updateType=UpdateTypeCode.UpdateWebFiles;			
			//					stateMessage.dataArray = new ArrayList();
			//					IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), profile.clientURI);
			//					iMessageReceiver.ReceiveMessage(stateMessage);					
			//
			//					stateMessage = null; //GC
			//				}
			//				GC.Collect(); //GC
			//				WebIntegeration.WebEvaluations eval = new WebEvaluations(ConferenceServer.staticServerInstance.dbCon);
			//				if(eval.GetAllEvaluations(profile.ClientRegistrationId.ToString()))			
			//				{
			//					StateUpdateMessage stateMessage=new StateUpdateMessage();
			//					stateMessage.updateType=UpdateTypeCode.UpdateWebEvaluations;			
			//					stateMessage.dataArray = eval.WebPollArray;
			//					IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), profile.clientURI);
			//					iMessageReceiver.ReceiveMessage(stateMessage);					
			//				
			//					stateMessage = null; //GC
			//				}
			//				else
			//				{
			//					StateUpdateMessage stateMessage=new StateUpdateMessage();
			//					stateMessage.updateType=UpdateTypeCode.UpdateWebEvaluations;			
			//					stateMessage.dataArray = new ArrayList();
			//					IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), profile.clientURI);
			//					iMessageReceiver.ReceiveMessage(stateMessage);					
			//					stateMessage = null; //GC
			//				}
			//				GC.Collect(); //GC
			//			}
			//			catch(Exception ee)
			//			{
			//			}
		}
		#endregion

		#region Piece of Commented Code from DetachUse
		//			SendUserListUpdateMessage();

		/***************
			 *  change relevant to close thread 
			 ****************/

		//			if(this.ClientList.Count < 1) 
		//			{
		//				TimeSpan t = conferenceStartTime.Subtract(DateTime.Now);
		//				if(t.TotalMinutes < 0)
		//				{
		//					//Meeting time has started
		//					//Close the conference is there is no attendee and host has joined
		//					if(bHostJoined) //Close the conference
		//						Server.ConferenceServer.staticServerInstance.CloseConference(this,true);
		//
		//					try
		//					{
		//						if(conferenceCloseThread.ThreadState ==System.Threading.ThreadState.Running)
		//							conferenceCloseThread.Abort();
		//					}
		//					catch(Exception ee)
		//					{
		//					}
		//				}
		//				else
		//				{
		//					//Before the meeting time.
		//					if(bHostJoined)
		//					{
		//						//Start the 15 minutes timer;
		//						if(conferenceCloseThread.ThreadState == System.Threading.ThreadState.Unstarted)
		//						{
		//							restartTimer = false;
		//							conferenceCloseThread.Start();
		//						}
		//						else if (conferenceCloseThread.ThreadState == System.Threading.ThreadState.Stopped)
		//						{
		//							restartTimer = false;
		//							conferenceCloseThread.Resume();
		//						}
		//					}
		//				}
		//			}

		#endregion

		#region SendUserJoinMessage Commented Code
		//(ArrayList)ClientList.Clone();			
		//	for(int i=0;i<this.ClientList.Count;i++)
		//	{
		//		//if(((ClientProfile)ClientList[i]).ClientId==receiverConnectionID)
		//		{
		//			IMessageReceiver iMessageReceiver =   (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), ((ClientProfile)ClientList[i]).clientURI);
		//			iMessageReceiver.ReceiveMessage(m);
		//			break;
		//		}				
		//
		//			}
		#endregion

	}
}
