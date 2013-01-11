#define dbgNetworkLayer




using System;
using System.Configuration;
using System.Runtime.Remoting;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;
using System.Management;

using Belikov.GenuineChannels;
using Belikov.GenuineChannels.BroadcastEngine;
using WebMeeting.Common;

using Belikov.GenuineChannels.DotNetRemotingLayer;
using Belikov.GenuineChannels.Logbook;

using System.Collections;



using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;

using Belikov.GenuineChannels.GenuineTcp;
//using Belikov.GenuineChannels.GenuineXHttp;
using System.Windows.Forms;

using Belikov.GenuineChannels.Parameters;
using System.Drawing.Imaging;
using WebMeeting.Client.Alerts;
 
namespace WebMeeting.Client
{
	public delegate void del_CloseAllTabs();
	public delegate void MakeChildWindow(MessageObject msg);
	public delegate void DelegateThreadFinished();
	
	
	
	/// <summary>
	/// Summary description for NetworkManager.
	/// </summary>
	public class NetworkManager: MarshalByRefObject, IMessageReceiver
	{
	
		
		public del_CloseAllTabs ddel_CloseAllTabs;
		/// <summary>
		/// This flag is used to check if the the webshare image based module has already been 
		/// shared or not. and is initilaised with a 0 
		/// When ever the Webshare is closed it should be assigned 0 again as on the start up when there was 
		/// no instance of web was shared.   
		/// When ever it has a value >0 it means that new generic Whiteboard control doesn't need to be initialised 
		/// </summary>
		public int flag_IsWebImageAlreadyActive=0;



		public int count=0;
		public bool Websharestarted=false;
		private Thread ReceiveThread;
		public WebMeeting.Polling.Polling pControl; 
		public delegate void _del_InitializeWhiteboard_Attendee();
		private WebMeeting.Client.Minuts_Meeting.Minuts_Controller MM_Controller=new WebMeeting.Client.Minuts_Meeting.Minuts_Controller();

		#region variables
		#region public  
		public WebMeeting.Common.ClientProfile  profile = new ClientProfile();		
		public  string Nickname;
		public  IConferenceRoom IConferenceRoom;
		public  IConferenceServer IConferenceServer;	
		public  object IConferenceServerLock = new object();
		public ArrayList myRecvMsgList;
		public bool m_bIsActive;
		public Queue appShareMsgQueue = new Queue();
		public ClientUI pClient;
		public WebMeeting.Common.ClientType clientType;
		public bool bAlreadyPainting = false;
		public bool bRetry = true;
		#endregion
		#region private
		private int retryCount=0;		
		//private bool m_bIsSending;
		private bool videoMessageSent=true;
		private bool desktopMessageSent=true;
		private bool bCreated = false;
		private bool reconnectionnestablish=false;
		private bool reConnEstablish_IsCleared=false;
		bool bInitialMessage =false;
		public bool isNewAttendee = false;
		#endregion		
		#region dispatcher
		public Dispatcher _dispatcher = new Dispatcher(typeof(IConferenceRoom));
		private Dispatcher _dispatcherAudio = new Dispatcher(typeof(IConferenceRoom));
		private Dispatcher _dispatchervideo = new Dispatcher(typeof(IConferenceRoom));
		private Dispatcher _dispatcherDesktopSharing = new Dispatcher(typeof(IConferenceRoom));
		#endregion
		#endregion
		#region messages
		private ArrayList videoMessages=new ArrayList();
		private ArrayList desktopMessages=new ArrayList();
		#endregion
		//private string confID;
		#region initialize network manager
		public static NetworkManager thisInstance;
		public static NetworkManager getInstance()
		{
			if(thisInstance == null)
			{				
				thisInstance = new NetworkManager();
				thisInstance.profile = new ClientProfile();	
			}
			return thisInstance;
		}


		public static NetworkManager getInstance_()
		{
			if(thisInstance == null)
			{				
				thisInstance = new NetworkManager();
			}
			return thisInstance;
		}
		#endregion		
		public DelegateThreadFinished m_DelegateThreadFinished;
		public event MakeChildWindow ChildWindowCreationEvent;		

		public int oldNetworkId;
		public int NewNetworkId;
		//private Thread videoMessageSendThread;
		//private Thread desktopMessageSendThread;

		public void Close()
		{
			this.m_bIsActive=false;
			bRetry = false;
		}
		
		/*
		public void SendingThreadFunction()
		{
			while(m_bIsActive)
			{
				if(MessagesArray.Count < 1)
				{
					Thread.Sleep(200);
					continue;
				}
				
			}
		}*/

		public NetworkManager()
		{
			
			try
			{
				//
				// TODO: Add constructor logic here
				//
				
				DebugConsole.Instance.Init(true,true); 
				myRecvMsgList=new ArrayList();
				m_bIsActive=true;
				
			
				this._dispatcher.BroadcastCallFinishedHandler += new BroadcastCallFinishedHandler(this.BroadcastCallFinishedHandler);
				this._dispatcher.CallIsAsync = true;
			
				//Thread AppShareThread = new Thread(new ThreadStart(AppShareDataHandleThread));
				//AppShareThread.Start();
				ReceiveThread=new Thread(new ThreadStart(receiveThread));
				ReceiveThread.Name="Network Manager Constructor Thread: receiveThread()";
				ReceiveThread.Start();				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 148 networkmanager==>",exp,null,false);				
			
			}
		}
		
		public void ReceiveMessage(string m,string n)
		{
			//Console.WriteLine(m + n);
		}

		/// <summary>
		///  Member	Description
		///  Aborted	The thread is in the Stopped state.
		///  AbortRequested	The ThreadAbort method has been invoked on the thread,
		///  but the thread has not yet received the pending System.Threading.
		///  ThreadAbortException that will attempt to terminate it.
		///  Background	The thread is being executed as a background thread,
		///  as opposed to a foreground thread. 
		///  This state is controlled by setting the IsBackground property of the Thread class.
		///  Running	The thread has been started, it is not blocked, and there is no pending ThreadAbortException.
		///  Stopped	The thread has stopped.
		///  StopRequested	The thread is being requested to stop. This is for internal use only.
		///  Suspended	The thread has been suspended.
		///  SuspendRequested	The thread is being requested to suspend.
		///  Unstarted	The Thread.Start method has not been invoked on the thread.
		///  WaitSleepJoin	The thread is blocked as a result of a call to Wait, Sleep or Join methods.
		///  
		/// </summary>
		/// <param name="msg"></param>
		public void ReceiveMessage(MessageObject msg)
		{
			
			

			try
			{
				//if the thead is Not alive due to some reason it will make that alive
			



				if(ReceiveThread==null)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Self made Exception of receive thread, Thread was null ", null ,null,false);
				}

				else if(!(ReceiveThread.IsAlive))
				{
					
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Self made Exception of receive thread, Thread was not Alive,Its state was :==>==> "+this.ReceiveThread.ThreadState.ToString(),null,null,false);
					//					ReceiveThread=new Thread(new ThreadStart(receiveThread));
					//					ReceiveThread.Name="Network Manager Constructor Thread: receiveThread()";
					//					ReceiveThread.Start();				
					//					if((ReceiveThread.IsAlive))
					//					{
					//						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Now receive thread is alive.......and its state is :"+this.ReceiveThread.ThreadState.ToString(),new Exception(),null,false);
					//					
					//					}
					//				
				}
				

				
				if(msg.MessageType == (ushort)MessageObjectType.MsgConnectionCheck)
				{
					myRecvMsgList.Add(msg);
					if(count >50)
					{
						count=0;

					}
					else
						count++;

				}
				else if(msg.SenderID!=this.profile.ClientId)
				{
					myRecvMsgList.Add(msg);
					if(count >50)
					{
						count=0;
					}
					else
						count++;
				}
			}
			catch(Exception exp)// e)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 166 receivethread ",exp,null,false);	
				//WebMeeting.Error.Show(e.Message + " On Receive, Exception");
			}
		}

		public ClientProfile GetClientProfileFromList(int clientID)
		{
			ClientProfile cp = null;
			try
			{
				for(int i = 0 ; i <ClientUI.getInstance().arrayParticipents.Count ; i++)
					if(((ClientProfile)ClientUI.getInstance().arrayParticipents[i]).ClientId == clientID)
						return (ClientProfile)ClientUI.getInstance().arrayParticipents[i];

			}
			catch(Exception exp)// ee)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 182 receivethread ",exp,null,false);	
				
			}
			return cp;
		}

		
		//private int DesktopSessionID;
		#region set properties
		public void SetClientTypeImage(int nItem,WebMeeting.Common.ClientProfile profile)
		{
			int nColumnIndex = 0;
			
			//if(this.profile.clientType == ClientType.ClientHost) // by kamran 
			//	nColumnIndex = 1;
	
			int nIndex = 19;
			switch(profile.clientType)
			{
				case ClientType.ClientAttendee:
					nIndex = 24;
					break;
				case ClientType.ClientPresenter:
					nIndex = 24;
					break;
				case ClientType.ClientHost:
					nIndex = 25;
					break;
			}
			ClientUI.getInstance().listParticipents.SetImage(nItem,nColumnIndex,nIndex);
		}
		public void SetArrow(int X)
		{
			int nColumnIndex = 5;
			if(profile.clientType != ClientType.ClientAttendee)
				nColumnIndex = 5;
			else
				nColumnIndex = 2;
			ClientUI.getInstance().listParticipents.SetImage(X,nColumnIndex,18);


		}
		public void SetMoodSubItemImage(int nItem,WebMeeting.Common.ClientProfile profile)
		{
			if(this.profile.clientType == ClientType.ClientAttendee)
				return ;

			int nIndex = 12;
			switch(profile.m_MoodIndicatorColor)
			{
				case MoodIndicator.Yellow:					
					nIndex =10;
					break;				
				case MoodIndicator.Blue:			
					nIndex =11;
					break;	
				case MoodIndicator.Green:					
					nIndex =12;
					break;
				case MoodIndicator.Red:					
					nIndex =13;
					break;
				case MoodIndicator.Question:					
					nIndex =14;
					break;
			}
			ClientUI.getInstance().listParticipents.SetImage(nItem,3,nIndex);
		}

		public void SetCheckBox(int nItem,WebMeeting.Common.ClientProfile profile)
		{
			if(this.profile.clientType == ClientType.ClientAttendee)
				return ;

			if(profile.clientType == ClientType.ClientAttendee)
				ClientUI.getInstance().listParticipents.SetImage(nItem,1,23);
			else
				ClientUI.getInstance().listParticipents.SetImage(nItem,1,22);
				
		}



		public void SetConnItemImage(int nItem,WebMeeting.Common.ClientProfile profile)
		{
			SetCheckBox(nItem,profile);
			if(this.profile.clientType == ClientType.ClientAttendee)
				return ;
			int nIndex = 15;
			switch(profile.m_ConnectionIndicator)
			{
				case ConnectionIndicator.Green:
					nIndex = 15;
					break;
				case ConnectionIndicator.Yellow:
					nIndex = 16;
					break;
				case ConnectionIndicator.Red:
					nIndex = 17;
					break;
			}
			ClientUI.getInstance().listParticipents.SetImage(nItem,2,nIndex);
		}
		#endregion

		#region profile handling
		public void ReInsertClientProfile(WebMeeting.Common.ClientProfile clientProf, int nIndex)
		{
			try
			{
				ListViewItem v;
				int X = 0;				
				if(clientProf.clientType ==  ClientType.ClientHost)
				{				
					v = pClient.listParticipents.Items.Insert(0, clientProf.Name);				
					if(profile.clientType!=ClientType.ClientAttendee)
					{
						v.SubItems.Add("");
						v.SubItems.Add("");				
						v.SubItems.Add("");
					}
				}
				else
				{								
					v = pClient.listParticipents.Items.Insert(pClient.listParticipents.Items.Count, clientProf.Name);
					if(profile.clientType!=ClientType.ClientAttendee)
					{
						v.SubItems.Add("");
						v.SubItems.Add("");					
						v.SubItems.Add("");
					}
				}
				if(clientProf.clientType!=ClientType.ClientAttendee)
					v.SubItems[0].Font=new System.Drawing.Font("Arial",8,System.Drawing.FontStyle.Bold);
				v.UseItemStyleForSubItems = false;
				v.SubItems.Add("").BackColor = clientProf.m_AssignedColor;
				v.SubItems.Add("");															
				X = v.Index;											
				ClientUI.getInstance().listParticipents.SetImage(X,0,0); // by kamran
				SetArrow(X);
				SetClientTypeImage(X,clientProf);
				SetConnItemImage(X,clientProf);
				SetMoodSubItemImage(X,clientProf);		          
	        
				ClientUI.getInstance().ListParticipentCheckBoxChange(X,clientProf.clientType);													
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 325 receivethread ReinsertClientProfile;",exp,null,false);	
			}
		}

		/// <summary>
		/// This Method will run for those for which attendee Update is called 
		/// and performs all the list related operations fot them
		/// As yet there is no try catch block on it 
		/// </summary>
		/// <param name="nSenderID"></param>
		/// <param name="i"></param>
		/// <param name="client"></param>
		/// <param name="clientProf"></param>
		// Zaeem View Update client Profile changes the state of the check boxes.
		public void UpdateClientProfile(int nSenderID,int i,ref ClientProfile client,ref ClientProfile clientProf)
		{


			// This Method will run for those for which attendee Update is called 
			// and performs all the list related operations fot them
			// controlMessage.SenderID==>Sender ID from the Message.
			// i					  ==>index in the participant list
			//clientProf              ==>profile coming with the message
			//client                  ==>In loop  
											
			

			#region copyProfile attributes
			//Zaeem View 
			// I dont understand this video audio block why its here and what it does?
			client.videoID = clientProf.videoID;
			client.audioID = clientProf.audioID;									

			//string YesNo;
			int X = i;
			// Set the grey small circular image for the Host and Non to the presenter.
			SetClientTypeImage(X,clientProf);
			
			// Zaeem view 
			// This call maintains the state of the check box 
			// Wrong Conditions are set in this right now 
			ClientUI.getInstance().ListParticipentCheckBoxChange(X,clientProf.clientType);
									

			//Zaeem view 
			// Not sure what they do.
			SetConnItemImage(i,clientProf);
			SetMoodSubItemImage(i,clientProf);
			// They should not be here now.
		
	
			// Set the Font 
			//i.e Bold for other except Attendee.
			//this code might throw and exception.
			//due to ==>ClientUI.getInstance()
			// Array index out of bound 
			if(clientProf.clientType==ClientType.ClientAttendee)
			{
				ClientUI.getInstance().listParticipents.Items[i].SubItems[0].Font=new System.Drawing.Font("Arial",8,System.Drawing.FontStyle.Regular);
				////Trace.WriteLine("Condition True"); 		
			}
			else
			{
				////Trace.WriteLine("Condition False"); 		
				ClientUI.getInstance().listParticipents.Items[i].SubItems[0].Font=new System.Drawing.Font("Arial",8,System.Drawing.FontStyle.Bold);
			}				

			

			//Gets and assign color from the profile

			if(this.profile.clientType == ClientType.ClientAttendee)			
				ClientUI.getInstance().listParticipents.Items[i].SubItems[1].BackColor =clientProf.m_AssignedColor;
			
			else			
				ClientUI.getInstance().listParticipents.Items[i].SubItems[4].BackColor =clientProf.m_AssignedColor;				
			
			
			//Zaeem View 
			// Not sure what this function does.
			SetArrow(i);
			
			
#endregion
			//									ClientUI.getInstance().listParticipents.Items[i].SubItems[1].BackColor = clientProf.m_MoodIndicatorColor;
			//									ClientUI.getInstance().listParticipents.Items[i].SubItems[2].BackColor = clientProf.m_AssignedColor;										;


			// Zaeem View 

			/// this block obviously sets the Audio and video properties 
			/// Didn't go into the details of this block 
			/// But have a confusion why this block is here ?
			/// 
			#region setaudio/vedio properties  
				
			if(clientProf.audioID >= 1) 
			{
				//						ClientUI.getInstance().listParticipents.SetImage(X,3,1);									
				//YesNo = "Y";
				if(clientProf.clientType == ClientType.ClientHost)
				{
					if(nSenderID!= -1)
						ClientUI.getInstance().SubscribetoUserAudioEx(clientProf);
				}
			}
			else
			{
				//YesNo = "N";
				for(int z = 0 ; z< ClientUI.getInstance().VideoTabPagesArray.Count ; z++)
				{	
					Crownwood.Magic.Controls.TabPage tabPage= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().VideoTabPagesArray[z];
					videovoiceControl control = (videovoiceControl) tabPage.Control;
					if(control.associatedProfile != null)
					{
						if(control.associatedProfile.ClientId == clientProf.ClientId)
						{
							ClientUI.getInstance().VideoTabPagesArray.RemoveAt(z);
							ClientUI.getInstance().Invoke(ClientUI.getInstance().RemoveAudioVideoTabPage, new object[]{z});
							//	ClientUI.getInstance().tabControl1.TabPages.RemoveAt(z);
							/*
								if(tabPage.Title.IndexOf("Disconnected") == -1)
									tabPage.Title += " - Disconnected";
		
								*/
							break;
						}
					}
				}
			}
			//	pClient.listParticipents.Items[i].SubItems[3].Text = YesNo;								
								
			if(clientProf.videoID >= 1) 
			{
								
				//	ClientUI.getInstance().listParticipents.SetImage(X,4,3);									
				//YesNo = "Y";

				if(clientProf.clientType == ClientType.ClientHost)
					ClientUI.getInstance().SubscribetoUserCameraEx(clientProf);
                                            
			}
			else
			{
				ClientUI.getInstance().IsHostVideoEnabled = false;// IsHostVideoEnabled
				for(int z = 0 ; z< ClientUI.getInstance().VideoTabPagesArray.Count ; z++)
				{
					Crownwood.Magic.Controls.TabPage tabPage= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().VideoTabPagesArray[z];
					videovoiceControl control = (videovoiceControl) tabPage.Control;
					if((control.associatedProfile != null) &&(control.isVideo))
					{
						
						
						if(control.associatedProfile.ClientId == clientProf.ClientId)
						{
							ClientUI.getInstance().VideoTabPagesArray.RemoveAt(z);
							ClientUI.getInstance().Invoke(ClientUI.getInstance().RemoveAudioVideoTabPage, new object[]{z});
							break;
						}
					}
				}
				//			ClientUI.getInstance().listParticipents.SetImage(X,4,4);																		
				//YesNo = "N";
			}
			//		pClient.listParticipents.Items[i].SubItems[4].Text = YesNo;								
			#endregion
							
            
			// On the updation of profile menus are changed accordingly

			if(clientProf.ClientId == NetworkManager.getInstance().profile.ClientId)
			{//if current client profile has been updated
				Client.ClientUI.getInstance().UpdateMainMenuAccess();
			}


			// Why does this block suppose to do.
			if(ClientUI.getInstance().audioEnabled)
			{
				//Client.ClientUI.getInstance().AudioSetMute(clientProf.clientAccess.accessShareAudio);
			}



		}






		
		public void AppShareDataHandleThread()
		{

			while(true)
			{
				if( appShareMsgQueue.Count == 0)
				{
					Thread.Sleep(50);
					continue;
				}

				MessageObject msg = (MessageObject)appShareMsgQueue.Dequeue();
				AppSharingMessageEx appMsg = (AppSharingMessageEx)msg;							
				appMsg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;

				if(appMsg.isDesktopshareing)
				{
					ClientUI.getInstance().DesktopSharingserver.RecieveMessageFromNetwork(ref appMsg);
					//DataType d= (DataType)appMsg.nDataType;												
				}
				else
				{
					ClientUI.getInstance().RecieveAppSharingMessage(ref appMsg);
				}
				
				msg = null; //KH
				appMsg = null; //KH

				Thread.Sleep(30);
			}

		}
		#endregion

		
		private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
		{
			try
			{

				Client.ClientUI client = ClientUI.getInstance();
				for(int xx = 0 ; xx < client.ApplicationSharingTabPagesArray.Count ; xx++)
				{
					Crownwood.Magic.Controls.TabPage tabPage =  (Crownwood.Magic.Controls.TabPage ) client.ApplicationSharingTabPagesArray[xx];
					ApplicationSharing control = (ApplicationSharing)tabPage.Control;
					control.SendLastMSG_AppShare();		
				}	

			
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>492 receivethread ;",exp,null,false);	
			}


			
		}


		bool bClientFlag =false;
		int myticker=0;
        /// <summary>
        /// for receiving messages of different types 
        /// </summary>
		public void receiveThread()
		{
			
					
			MessageObject msg=null;			
			while(m_bIsActive)
			{

				try
				{		
			
					if(myRecvMsgList.Count==0)
					{
						Thread.Sleep(10);
						//Application.DoEvents(); // for check purpuse
						continue;
					}
					
					//try
					//{
					msg=(MessageObject)myRecvMsgList[0];
					//}
					//catch(Exception exp)
					//{
					//	WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 599 msg=(MessageObject)myRecvMsgList[0]; ",exp,null,false);
					//}						
						
					//try
					//{
					myRecvMsgList.RemoveAt(0);
					//}
					//catch(Exception exp)
					//{
					//	WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 526 receivethread myRecvMsgList.RemoveAt(0);",exp,null,false);	
					//}

					//for Syncronyzing tab positioning 

					if(myticker>10)
					{
						
						if(ClientUI.getInstance()!=null)
						ClientUI.getInstance().SynTabpositioning();
						myticker=0;
					}
					myticker++;




						#region Minuts of Meeting message
					// MessageClose.
					if(msg.GetType().Equals(typeof(Mes_MinutsofMeeting))) // per1
					{
						
						try
						{	
							// Only Host will take and UPload the SnapShot.
							if(profile.clientType==  ClientType.ClientHost)
							{
								
								MM_Controller._Snapshot(msg);
							}
						
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 759 receivethread CloseMsg",exp,null,false);	
						}
						
					}
					#endregion

					    #region ClearAlltabs
					// This message close the tabs.
					if(msg.GetType().Equals(typeof(ClearAllTabs))) // per1
					{
						
						try
						{
							//c.BeginInvoke(this.ddel_CloseAllTabs);	
							//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before ClearAllTabs",null,null,false);	
							ClientUI.getInstance().ClientCloseAllTabs();
							
							//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After ClearAllTabs",null,null,false);	

						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 532 receivethread ClearAllTabs",exp,null,false);	
						}
						
					}
					#endregion

						#region CloseMsg
					// MessageClose.
					if(msg.GetType().Equals(typeof(CloseMsg))) // per1
					{
						
						try
						{	
							if(pControl!=null)
							{
								pControl.attendeeForm.Close();
							}
							if(ClientUI.getInstance()!=null)
							{
								if(ClientUI.getInstance().frm_PollingGlobalAttendee!=null)
								{
									ClientUI.getInstance().frm_PollingGlobalAttendee.Close();
								}
														
							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 741 receivethread CloseMsg",exp,null,false);	
						}
						
					}
					#endregion
			
						#region server info message
					//
					// This should be the first message from the server of type ServerInfoMessage.
					// Discard if this is not the case.
					//
					
					if(msg.GetType().Equals(typeof(ServerInfoMessage)))
					{
						
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before ServerInfoMessage",null,null,false);
						try
						{
						
							ServerInfoMessage infoMsgEx = (ServerInfoMessage) msg;														
							if(!bInitialMessage)
							{
								//pClient.listParticipents.Items.Clear();
								//pClient.arrayParticipents.Clear();
								try
								{
									Client.ClientUI.getInstance().ExceptionLog("sendTo object start pointing chatControl1.testCombo and following values are added <Main Chat>,<All Presenters> & <All Attendees> ::: method :: recived thread");
									pClient.sendTo.Items.Clear();
									int n = pClient.sendTo.Items.Add("<Main Chat>");
									pClient.sendTo.Items.Add("<All Presenters>");
									pClient.sendTo.Items.Add("<All Attendees>");
									pClient.sendTo.SelectedIndex = n;
								}
								catch(Exception exp)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 577 receivethread Serverinfo Messages;",exp,null,false);	
								}



							}	
						
							Thread.Sleep(3000);
						
							//pClient.listParticipents.Clear(); //.Items.re.Insert(0,name);								

							//////////////////// RECONNECT //////////////////////
							if(reconnectionnestablish)
							{
								try
								{
									while(! reConnEstablish_IsCleared)
									{
										Thread.Sleep(10); 
										Application.DoEvents();
									}
								}
								catch(Exception exp)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>601 if(reconnectionnestablish);",exp,null,false);	
								}

							}
							///This code is added by zaeem on the 1st of December to solve the prob
							///of ==>on connection reestablishing menues gets disabled.
							///

							reConnEstablish_IsCleared = false;
							//////////////////////////////////////////

							int countFlag_LeaveFirstTime = 0;
							foreach (ClientProfile Cprofile in infoMsgEx.AttendeeProfileList.Values)
							{	
								try
								{
									string email=Cprofile.EMailAddress;
									string name= Cprofile.Name;
									string company= Cprofile.Company;
									ListViewItem v ;
									int local_id,remote_id;							
									remote_id=Cprofile.ClientId;
									local_id=profile.ClientId;
						    
									if(profile.ClientRegistrationId == Cprofile.ClientRegistrationId)
									{
										countFlag_LeaveFirstTime++;
									} 

									if(countFlag_LeaveFirstTime > 1) //Kamran
									{
										MeetingAlerts alert=new MeetingAlerts();
										alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"This User already entered in this Conference, You should use another User Registration ID.",true,false);
										alert.Dispose();
										Environment.Exit(0); // check for exit by sending closing message properly (call client_closing)
									}
							
									if(local_id==remote_id)
										continue;
									if(remote_id==0)
										continue;
							
									if(this.profile.Name==Cprofile.Name)
										continue;
									////////////////////////////
									bClientFlag = false;
									try
									{
										for(int i=0;i<pClient.arrayParticipents.Count  ;i++)
										{
											ClientProfile clientProf = (ClientProfile)pClient.arrayParticipents[i];
											if( clientProf.EMailAddress ==  Cprofile.EMailAddress )
											{
												bClientFlag = true;
												break;
											}
										}
									}
									catch(Exception ex)
									{
										WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>656 Networkmanager receivethread;",ex,null,false);	
									}
									if(bClientFlag )
										continue;

									////////////////////////////

									if( Cprofile.clientType ==  ClientType.ClientHost )
									{
										v = pClient.listParticipents.Items.Insert(0,name);								
										pClient.listParticipents.Items[0].Font=new System.Drawing.Font("Arial",8,System.Drawing.FontStyle.Bold);
										if(profile.clientType!=ClientType.ClientAttendee)
										{
											v.SubItems.Add("");
											v.SubItems.Add("");									
											v.SubItems.Add("");
										}
									}
									else if( Cprofile.clientType ==  ClientType.ClientPresenter)
									{
										v = pClient.listParticipents.Items.Insert(pClient.listParticipents.Items.Count, name);
										v.SubItems[0].Font=new System.Drawing.Font("Arial",8,System.Drawing.FontStyle.Bold);
										if(profile.clientType!=ClientType.ClientAttendee)
										{
											v.SubItems.Add("");
											v.SubItems.Add("");	
											v.SubItems.Add("");
										}
									}
									else
									{								
										v = pClient.listParticipents.Items.Insert(pClient.listParticipents.Items.Count, name);
										if(profile.clientType!=ClientType.ClientAttendee)
										{
											v.SubItems.Add("");
											v.SubItems.Add("");									
											v.SubItems.Add("");
										}
									}							
									v.UseItemStyleForSubItems = false;
							
									v.SubItems.Add("").BackColor = Cprofile.m_AssignedColor;
									v.SubItems.Add("");

									SetMoodSubItemImage(v.Index,Cprofile);
									SetConnItemImage(v.Index,Cprofile);
									SetArrow(v.Index);

									int X = v.Index;							
									ClientProfile clientProfile = (ClientProfile)Cprofile;
									SetClientTypeImage(X,clientProfile);
									/////////////////////////////////////////
							
									pClient.arrayParticipents.Insert(v.Index,clientProfile);

							
									//////////////////////////////////////////
									ClientUI.getInstance().ExceptionLog("Adding Sendto Items " + clientProfile.Name.ToString());
									pClient.sendTo.Items.Add(clientProfile.Name);
									ClientUI.getInstance().ListParticipentCheckBoxChange(X,clientProfile.clientType);							
									/*
									if(ClientUI.getInstance().IfIamthePresenter())
									{
									ClientUI.getInstance().ena
									}
									*/

								}
								catch(Exception exp)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>720 foreach Loop;",exp,null,false);	
								}

							}

							Client.ClientUI.getInstance().listParticipents.Items[0].Selected = true;
						}
						
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 700 receivethread Server Info msg",exp,null,false);	
						}
							
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After ServerInfoMessage",null,null,false);
					}	

					
					else if(msg.GetType().Equals(typeof(AuthenticationMessage)))
					{
#if dbgNetworkLayer
						//Trace.WriteLine("Message Name :AuthenticationMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before AuthenticationMessage",null,null,false);	
						try
						{
							AuthenticationMessage Msg = (AuthenticationMessage)msg;
							if(!Msg.bAuthenticated)
							{
								ClientUI.getInstance().bAskPresenter = false;
								ClientUI.getInstance().Close();
								return;							
							}
							else
							{
								Info.getInstance().FtpIP = Msg.FTP_IP;
								Info.getInstance().Password = Msg.FTPPassoword;
								Info.getInstance().UserName = Msg.FTPUsername;
								Info.getInstance().VoiceConferenceID = Msg.voiceConferenceId;
								Info.getInstance().ConferenceName = Msg.ConferenceName;
							
								Info.getInstance().VoiceIP = Msg.VoiceIP;
								Info.getInstance().VoicePort = Convert.ToInt32(Msg.VoicePort);


								ClientUI.getInstance().ImageURL = Info.getInstance().WebsiteName + "/members/logos/" + Msg.ImageURL;
								profile.Name = Msg.ClientName;
								profile.EMailAddress = Msg.ClientEmail;

								ClientUI.getInstance().ConnectionEnabled();
								Info.getInstance().CompanyName = Msg.companyName ;			
								/////////////// meeting title to application title
								//ClientUI.getInstance().Text = Info.getInstance().CompanyName;
								ClientUI.getInstance().Text = Info.getInstance().ConferenceName;

							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 739 receivethread Server Info msg",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After AuthenticationMessage",null,null,false);	
					}
				
						#endregion
					
						#region Application Sharing 

					else if(msg.GetType().Equals(typeof(AppSharingMessageEx)))
					{	
						try
						{

						
							bool bContinue = false;
							appShareMsgQueue.Enqueue(msg);
							AppSharingMessageEx appMsg = (AppSharingMessageEx)msg;							
							if(appMsg.isDesktopshareing)
							{
 
								ClientUI.getInstance().DesktopSharingserver.RecieveMessageFromNetwork( ref appMsg);
							
								DataType d= (DataType)appMsg.nDataType;
								// delete the message only if it's mouse message.
								if( d == DataType.KeyBoardActivity)
									bContinue = true;
								myRecvMsgList.RemoveAt(0);
							}
							else
							{
								
								ClientUI.getInstance().RecieveAppSharingMessage(ref appMsg);
								if(!appMsg.bEnabled)
								{
									
									appShareMsgQueue.Clear();
								}
								appMsg = null; //KH
								GC.Collect(); //KH
								GC.WaitForPendingFinalizers(); //KH
							}
							if(bContinue)
								continue;
							
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 825 receivethread==new application msg",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After AppSharingMessageEx",null,null,false);	
					}
						#endregion
					
						#region PingTime
					else if(msg.GetType().Equals(typeof(ConnectionChechMessage)))
					{
#if dbgNetworkLayer
						//Trace.WriteLine("Message Name :ConnectionChechMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif
						try
						{
							//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before ConnectionChechMessage",null,null,false);	
							ConnectionChechMessage connMsg = (ConnectionChechMessage)msg;
							connMsg.profile = profile;
							connMsg.profile.connectionCheckRecievedDateTime = DateTime.Now.ToFileTime();

							IConferenceRoom.RecieveConnectionMsgResponse(connMsg);
								
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>811 receivethread==>ping time",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After ConnectionChechMessage",null,null,false);
				
					}
						#endregion
					
						#region conrol message
					else if(msg.GetType().Equals(typeof(ControlMessage)))
					{

						
						try
						{
							//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before ControlMessage",null,null,false);	
							ControlMessage controlMessage = (ControlMessage)msg;
							if(controlMessage.Code == ControlCode.AttendeeDropped)
							{
										
								try
								{
									ClientProfile clientProf = (ClientProfile)controlMessage.Parameter;								
									// this Methods only called from here 
									// When the presenter leaves the meeting 
									// All tabs are closed and drawing tools are Syncronyzed
									// flag_checkbox=false;
									// Assigning false to the above flag means 
									// that now there is no presenter in the meeting
									// and you are free to give any one the rights

									// CAUTION
									//	The line below may cause an Exceprion 
									// So first check if ClientUI.getInstance()!=null
									bool flg_presenterdrop=false;
									flg_presenterdrop=ClientUI.getInstance().NoofLogicalPresenters(clientProf);
									if(flg_presenterdrop)
									{
										// Poll windows should also be closed on this action
										try
										{	
											if(pControl!=null)
											{
												pControl.attendeeForm.Close();
											}
											if(ClientUI.getInstance()!=null)
											{
												ClientUI.getInstance().frm_PollingGlobalAttendee.Close();
														
											}
										}
										catch(Exception exp)
										{
											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 1149 AfterWho is Dropped",exp,null,false);	
										}

									}


									// If the Attendee Drop message is for host
									if(clientProf.clientType == ClientType.ClientHost)
									{
										//This method should run when the Host is dropped from the meeting
										/// The purpose of this method is that the meeting shd come to the initial
										/// state i.e Presenter becomnes Attendee , Drawing tools are Syncronyzed 
										/// and all opened instances are closed.
										/// It should run only for Host
										/// 
										///					Zaeem View 
										// Improvment Area
										// 1 Message shd be send only to those who are Not Attendee currently
										// as i think its useless to send message to those who 
										// are already Attendee.
														
										bool flag=false;
										if(ClientUI.getInstance()!=null)
										{
											flag=ClientUI.getInstance().WhoisDropped(clientProf);
										}
										if(flag)
										{
																		
											// Poll windows should also be closed on this action
											try
											{	
												if(pControl!=null)
												{
													pControl.attendeeForm.Close();
												}
												if(ClientUI.getInstance()!=null)
												{
													ClientUI.getInstance().frm_PollingGlobalAttendee.Close();
														
												}
											}
											catch(Exception exp)
											{
												WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 1149 AfterWho is Dropped",exp,null,false);	
											}

										
										}
											
									}										
										
										
									ClientUI.getInstance().AttendeeDroped(clientProf);
										
									pClient.RemoveParticipent(clientProf);	
										
								}
								catch(Exception exp)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 852 receivethread==ControlCode.AttendeeDropped",exp,null,false);	
								}
							}
								/*
																	 else if (controlMessage.Code == ControlCode.AttendeeDropped)
																	 {
																		 ClientProfile clientProf = (ClientProfile)controlMessage.Parameter;
																		 Trace.WriteLine("ControlCode.AttendeeDropped"+ " ::: " + controlMessage.SenderID.ToString() + "  :::: " +  clientProf.ClientId.ToString());
							
																		 pClient.RemoveParticipent(clientProf);
																		 Client.ClientUI.getInstance()._statusBar.LeftMessage = "Connected    ";
																		 ClientUI.getInstance().AttendeeDroped(clientProf);
																	 }
											 */					

							else if(controlMessage.Code == ControlCode.PresenterKickedPerson)
							{
										
								try
								{
									//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before PresenterKickedPerson",null,null,false);
									ClientProfile clientProf = (ClientProfile)controlMessage.Parameter;
									if(clientProf.ClientId==this.profile.ClientId)
									{
										ClientUI.getInstance().ConferenceClosed();
										//MessageBox.Show("You have been kicked out of the conference by " + clientProf.Name ,"WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);								
										Environment.Exit(1);
										break;
									}
								} 
								catch(Exception exp)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 883 ControlCode.PresenterKickedPerson in receive thread msg",exp,null,false);	
								}
								//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After PresenterKickedPerson",null,null,false);
							}							
							else if (controlMessage.Code == ControlCode.PresenterClosedConference)
							{
									
								try
								{
									//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before PresenterClosedConference",null,null,false);
									ClientProfile clientProf = (ClientProfile)controlMessage.Parameter;
									if(clientProf.ClientId == profile.ClientId)
									{
										ClientUI.getInstance().ConferenceClosed();
										MeetingAlerts alert=new MeetingAlerts();
										alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Conference has been closed by " + clientProf.Name,true,false);
										alert.Dispose();
										Environment.Exit(0);
										//MessageBox.Show("Conference has been closed by " + clientProf.Name,"WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);								
										break;
									}
								} 
								catch(Exception exp)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 905 ControlCode.PresenterClosedConference in receive thread msg",exp,null,false);	
								}
								//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After PresenterClosedConference",null,null,false);
								//Close();																
								//break;
							}
								#region AtttendeJoin
							else if(controlMessage.Code == ControlCode.AttendeeJoin)
							{			
								//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before AttendeeJoin",null,null,false);
								isNewAttendee = true;	// for new app sharing msgs
								ListViewItem v;
								int X = 0;
								ClientProfile clientProf = (ClientProfile)controlMessage.Parameter;
								bool bFound =false;
						
									
								# region RemoveODD---commented

								////								try
								////								{
								////									for(int i = 0 ; i < pClient.arrayParticipents.Count ; i++)
								////									{
								////										//	MessageBox.Show("listParticipents==>"+pClient.listParticipents.Items[i].Text.ToString());
								////
								////										ClientProfile CP = (ClientProfile)pClient.arrayParticipents[i];
								////										
								////										
								////										if(CP.Name == pClient.listParticipents.Items[i].Text.ToString())
								////										{
								////										
								////											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Self made Line==> 1065 it was in list items",null,null,false);	
								////										
								////										
								////										}
								////										
								////										
								////										if(CP.Name == clientProf.Name)
								////										{	
								////											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Self made  Line==> 1073 it was in arrayparticipents",null,null,false);	
								////									
								////											//	WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 905 ControlCode.PresenterClosedConference in receive thread msg",exp,null,false);	
								////											
								////											/*
								////												lock(pClient.arrayParticipents)
								////												{
								////													try
								////													{
								////														if(pClient.arrayParticipents.Count>i)
								////														{
								////															pClient.arrayParticipents.RemoveAt(i);
								////														}
								////													}
								////													catch(Exception exp)
								////													{
								////														WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> if(arrayParticipents.Count>i)",exp,null,false);
								////													}
								////
								////												}
								////											
								////												lock(pClient.listParticipents)
								////												{
								////													try
								////													{
								////							
								////														if(pClient.listParticipents.Items.Count>i)
								////														{
								////															pClient.listParticipents.Items.RemoveAt(i);												
								////														}
								////													}
								////													catch(Exception exp)
								////													{
								////														WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("ClientUI.cs line==> if(listParticipents.Items.Count>i)",exp,null,false);
								////													}
								////												}
								////												*/
								////						
								////										}
								////									}
								////									
								////								}
								////								catch(Exception exp)
								////								{
								////									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 1115 zaeem method of attendee join",exp,null,false);	
								////									
								//								}
								#endregion
									
									
									
								/*for(int i = 0 ; i < pClient.arrayParticipents.Count ; i++)
												 {
													 ClientProfile CP = (ClientProfile)pClient.arrayParticipents[i];
													 if(CP.ClientRegistrationId == clientProf.ClientRegistrationId)
													 {										
														 CP = clientProf;
														 bFound = true;
														 pClient.listParticipents.SetImage(i,0,0);										
														 v = pClient.listParticipents.Items[i];
														 v.SubItems[0].Text = CP.Name;
														 X = v.Index;
														 break;
													 }
												 }*/
							
								//							int local_id,remote_id;							
								//							remote_id=clientProf.ClientId;
								//							local_id=profile.ClientId;
								//							if(! reconnectionnestablish)
								bool isProfileExist =  false;
							
								if(reconnectionnestablish)
								{
									foreach(ClientProfile p in ClientUI.getInstance().arrayParticipents)
									{	
										if(clientProf.Name==p.Name)
										{
											isProfileExist = true;
											break;
										}
									}
								}
								
								
								  
						
								if(!isProfileExist )
								
									//							if( (local_id!=remote_id) && (local_id!=0) && (remote_id!=0) )

								{
									if(!bFound)
									{				
										//
										/*Thread.Sleep(3000); // for new app sharing msgs
													 isNewAttendee = true;	// for new app sharing msgs
													 */

										if(profile.clientType!=ClientType.ClientAttendee)								
										{									
											if((clientProf.clientType==ClientType.ClientAttendee)||(clientProf.clientType==ClientType.ClientPresenter))
												v =pClient.listParticipents.Items.Insert(pClient.listParticipents.Items.Count,clientProf.Name);
											else
												v =pClient.listParticipents.Items.Insert(0,clientProf.Name); 
									
											v.SubItems.Add("");
											v.SubItems.Add("");								
											v.SubItems.Add("");
										}
										else
										{			
											if(clientProf.clientType==ClientType.ClientAttendee)
												v =pClient.listParticipents.Items.Insert(pClient.listParticipents.Items.Count,clientProf.Name);
											else
												v =pClient.listParticipents.Items.Insert(0,clientProf.Name); 									
										}
										if(clientProf.clientType!=ClientType.ClientAttendee)									
											v.SubItems[0].Font= new System.Drawing.Font("Arial",8,System.Drawing.FontStyle.Bold);
										v.UseItemStyleForSubItems = false;								
										v.SubItems.Add("").BackColor = clientProf.m_AssignedColor;
										v.SubItems.Add("");															
										X = v.Index;									
										pClient.arrayParticipents.Insert(v.Index,clientProf);
										ClientUI.getInstance().ExceptionLog("Adding Sendto Items " + clientProf.Name.ToString() + " Message in ReceiveThread ::: ControlCode.AttendeeJoin");
										pClient.sendTo.Items.Add(clientProf.Name);								
										ClientUI.getInstance().listParticipents.SetImage(X,0,0);

										if(ClientUI.getInstance().myAppShareStarted)								
											ClientUI.getInstance()._appShareControl.AttendeeJoined();								

										////////////////////Sys_ChatMsgs//////////
										/*
										 * send all public message if the client type is host.
										 * */
										ClientUI.getInstance().chatControl1.Sys_SendText(clientProf.ClientId );

										//DocumentSharing msg = documentSharingControl.sys_DocSharing();
									
										for(int ii=0;ii<ClientUI.getInstance().arraydocumentSharing.Count ;ii++)
										{
											Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[ii];
											documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
											if(!control.IsClosed)
											{
												if(control.IsLaunchFromMangeContent)
												{
													string[] strLen=control.m_DocumentMessage.DownloadURL.Split(',');						
													//MessageBox.Show(strLen.Length.ToString());
						
													if(strLen.Length>1)
														NetworkManager.getInstance().SendLoadPacket(control.m_DocumentMessage);																									
													else			
													{
														//string previousUrl=control.m_DocumentMessage.DownloadURL;
														control.m_DocumentMessage.DownloadURL="Already Downloaded,"+control.m_DocumentMessage.DownloadURL;
													
														NetworkManager.getInstance().SendLoadPacket(control.m_DocumentMessage);
														//													control.m_DocumentMessage.DownloadURL=previousUrl;
													}
						
												
													//control.m_DocumentMessage.DownloadURL="Already Downloaded,"+control.m_DocumentMessage.DownloadURL;
												
												}
												else												
													NetworkManager.getInstance().SendLoadPacket(control.m_DocumentMessage);
											}				
										}
	

										///////////////////////////////////////////////
									}
									SetArrow(X);
									SetClientTypeImage(X,clientProf);
									SetConnItemImage(X,clientProf);
									SetMoodSubItemImage(X,clientProf);                     
									ClientUI.getInstance().ListParticipentCheckBoxChange(X,clientProf.clientType);
									Thread.Sleep(3000);
									
									//By zaeem to enable the menus on reconnection 
									if(ClientUI.getInstance()!=null)
									{
										if(ClientUI.getInstance().IfIamthePresenter())
										{
											
											ClientUI.getInstance().enableMenus(true);
										}
									}
										
										
									//ClientUI.getInstance().ShowWhiteBoard(false);
									if(this.profile.clientType == ClientType.ClientHost || this.profile.clientType == ClientType.ClientPresenter)
									{
										if(WebMeeting.Client.ClientUI.getInstance().isWhiteBoardSelected())
										{
										
											WhiteboardMessage t_msg = new WhiteboardMessage();
											t_msg.MessageType=(ushort)MessageObjectType.MsgShowWhiteBoard;
											t_msg.ConferenceID = this.profile.ConferenceID ;	
											SendLoadPacket(t_msg);			
											//Trace.WriteLine("new user come and packet of show WhiteBoard is selected"); 
										}
										else if(WebMeeting.Client.ClientUI.getInstance().isWhiteBoardEnabled())
										{
											//for problem of changing tab to WhiteBoard.
											WhiteboardMessage t_msg = new WhiteboardMessage();
											t_msg.ConferenceID = this.profile.ConferenceID ;	
											t_msg.MessageType=(ushort)MessageObjectType.MsgWhiteboard ;

											SendLoadPacket(t_msg);			
											//Trace.WriteLine("new user come and packet of WhiteBoard is Send"); 

										}
										try
										{
											//Trace.WriteLine("tabpages : " +ClientUI.getInstance().tabBody.TabPages.Count.ToString());
											for(int ii=0;ii<ClientUI.getInstance().tabBody.TabPages.Count;ii++)
											{
												Crownwood.Magic.Controls.TabPage tabpages = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().tabBody.TabPages[ii];
												if(tabpages.Control.GetType().Equals(typeof(browser)))
												{
													//browser tabBrowser=new browser();
													//Trace.WriteLine("found browser window");
													browser tabBrowser=(browser) tabpages.Control;
													if(tabBrowser.isSharedBrowser== true)
														tabBrowser.ShareNewAttendee();
													//Zaeem's code 
													// whenever a new Attendee joins the meeting joins, its necessary to provide him with a new state 
													// this check checks whether the browser is in the image state 
													// if yes then the new guy shd provided with the latest image
													if(ClientUI.flag_ImageMode)
													{
														tabBrowser.CompressandSendImage(tabBrowser.whiteBoard._picBox.Image);
													}

												}
															
											}
										}
										catch(Exception exp)
										{
											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 1580 Browser Update on Attendee Joining in receive thread msg",exp,null,false);	
										}

										ClientUI.getInstance().SynchronizeTabPages();//for the purpose of Synchronization of tabs when new attendee joins
									}
								

								
									// ApplicationSharing.maxApplication();

									////////////////////////
									//System.Timers.Timer aTimer = new System.Timers.Timer();
									//aTimer.Elapsed+=new System.Timers.ElapsedEventHandler(OnTimedEvent);
									// Set the Interval to 60 seconds.
									//aTimer.Interval=60000;
									//aTimer.Enabled=true;
									/////////////////////////////////
								
								

									/*Thread thLastMess = new Thread( new ThreadStart(ApplicationSharing.SendLastMSG_AppShare));
												 thLastMess.Name = ""; 
												 thLastMess.Start();
														 */
								}
								//else
								//	reconnectionnestablish = false;
								//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After AttendeeJoin",null,null,false);
							}
								#endregion
							else if(controlMessage.Code == ControlCode.AttendeeLeave)
							{
								//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before AttendeeLeave",null,null,false);
								try
								{
									//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before AttendeeLeave",null,null,false);
									Trace.WriteLine("ControlCode.AttendeeLeave "+ " ::: " + controlMessage.SenderID.ToString());
									ClientProfile clientProf = (ClientProfile)controlMessage.Parameter;
									pClient.RemoveParticipent(clientProf);
									Client.ClientUI.getInstance()._statusBar.LeftMessage = "Connected    ";
									ClientUI.getInstance().AttendeeDroped(clientProf);
									//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After AttendeeLeave",null,null,false);
								} 
								catch(Exception exp)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 1132 ControlCode.AttendeeLeave in receive thread msg",exp,null,false);	
								}
								//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After AttendeeLeave",null,null,false);
							}
						
							else if(controlMessage.Code == ControlCode.AttendeeUpdate)
							{ 

								//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before AttendeeUpdate",null,null,false);
								ClientProfile clientProf = (ClientProfile)controlMessage.Parameter;								
								bool OurProfileIsUpdate=false;
								//								try
								//								{
								//									//Trace.WriteLine(clientProf.Name.ToString()+"In receive thread has the==>"+clientProf.clientAccess.annotationRights.ToString());
								//								}
								//								catch(Exception exp){}
								// For making the the erease button enable and disable	

								// Written All by Zaeem
								// clientProf ---Coming with Message
								// profile ------profile of the this user
								// this try catch block deals with the erease All buuton 
								// Erease all can only be done by the Presenter 
								// So this button should only be visible to presenter 
								# region Erease All functionality
								//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

								try
								{
									
									if(ClientUI.getInstance()!=null)
									{
										if(clientProf.ClientId ==profile.ClientId && clientProf.clientType==ClientType.ClientPresenter)
										{
											ClientUI.getInstance().btnEraserVisible(true);
								
										}
										else if(clientProf.ClientId ==profile.ClientId && clientProf.clientType==ClientType.ClientHost)
										{
										
											if(ClientUI.getInstance().ifHostisPresenter())
											{
												ClientUI.getInstance().btnEraserVisible(true);
											}
											else
											{
												ClientUI.getInstance().btnEraserVisible(false);
											}
									
										}
										else if(clientProf.ClientId ==profile.ClientId && clientProf.clientType==ClientType.ClientAttendee)
										
										{
											ClientUI.getInstance().btnEraserVisible(false);
									
										}
									}
								}
								catch(Exception exp )
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 1310",exp,null,false);	
								}
								//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
								# endregion

								//If the comming message is for me and i am not the current presenter
								#region updation of drawing Tools on the basis of ur profile.annotation rights
								//Removing Enable/Disable annoatation tools
								/*
								if(clientProf.ClientId ==profile.ClientId && clientProf.clientType!=ClientType.ClientPresenter)
								{
									try

									{
										if(ClientUI.getInstance()!=null)
										{
											if(clientProf.clientAccess.annotationRights)
											{
												ClientUI.getInstance().enableWhiteBoardTools(true);
											}
											else if(ClientUI.getInstance().IfIamthePresenter())
											{
												ClientUI.getInstance().enableWhiteBoardTools(true);
											}
											else
											{
												ClientUI.getInstance().enableWhiteBoardTools(false);
											}
										}
									}
									catch(Exception exp)
									{
										WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 1335",exp,null,false);	
														
									}
								
								
								}
								*/					
								# endregion

								//Zaeem View
								//Problem cause ==> this check is the real problematic one in the Attendee Update;
								// If the coming message is for Host and i am the current Host. 
								if(clientProf.clientType==ClientType.ClientHost && profile.clientType==ClientType.ClientHost)
								{	
									//MessageBox.Show("Host is deopped.So all the shared things are closing...");
									//ClientUI.getInstance().WhoisDropped(clientProf);
								}
								else
								{
									bool bFound = false;									
									for(int i = 0; i < ClientUI.getInstance().arrayParticipents.Count; i++)
									{								
										ClientProfile client = (ClientProfile)ClientUI.getInstance().arrayParticipents[i];
										if(clientProf.ClientId == client.ClientId)
										{									
											//Zaeem View
											// I think this pieve of code is not right.
											# region short fixing 
											// Only 1 time 
											//if its not our profile
											// Who so ever having the annotation rights
											// clientTemp is updated accordingly.
											if(clientProf.ClientId != profile.ClientId) 
											{
												for(int z = 0 ; z < ClientUI.getInstance().arrayParticipents.Count ; z++)
												{
													ClientProfile clientTemp = (ClientProfile)ClientUI.getInstance().arrayParticipents[z];
													if(clientTemp.ClientId==clientProf.ClientId)
													{															
														if(clientProf.clientAccess.annotationRights)
														{
															clientTemp.clientAccess.annotationRights=true;			
														}
														else
														{
															clientTemp.clientAccess.annotationRights=false;			
														}
														//ReInsertClientProfile(clientTemp,z);
														break;
													}
													////Trace.WriteLine("ReInsertClientProfile(clientTemp,z) is going to call");															
													
												}
											}
											# endregion

											
											/*this check will run when message is about our/current participant profile*/											
											if(clientProf.ClientId == profile.ClientId) // if this is the same user who shared the audio 
											{	
												# region D/V
												
												
												bool bChangeInterface = false;
												
												// clientProf---new comming profile
												if(profile.clientType == clientProf.clientType)
													bChangeInterface = false;
												else
													bChangeInterface = true;
												//////Trace.WriteLine("bChangeInterface : " + bChangeInterface.ToString()); 
												
												//Zaeem View
												// If our prifile is changing .......
												if(bChangeInterface)
												{
													/*to change splitter control visibility */
													//Zaeem View
													//Why this piece of code is here ?
													# region the current browser control's tool is assigned None
													for(int	ctrlbrowser =	0 ;	ctrlbrowser <	ClientUI.getInstance().arrayWebControl.Count;	ctrlbrowser++)
													{								
														Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arrayWebControl[ctrlbrowser];
														browser browserControl = (browser)tabPage.Control;																
														if(clientProf.clientType==ClientType.ClientAttendee)
														{
															browserControl.ShowSplitter(false);
															browserControl.annotationBrowser.selectToolNone();
														}
														else
														{
															browserControl.ShowSplitter(true);
															browserControl.annotationBrowser.SelectToolLine();
														}
														//												if(browserControl.sessionID ==	Msg.webBrowseID)
														//												{
														//													bFound = true;
														//													browserControl.msgPool.Add(Msg);																	
														//													break;
														//												}																						
													}
													# endregion 			


													# region Attendee Block								
													if(clientProf.clientType == ClientType.ClientAttendee)
													{
														try
														{

															//Zaeem View
															//if i am the attendee and has the annotation rights true 
															// then my tools shd be enabled but 
															// CAUTION
															// THIS WILL ONLY RUN WHEN MY PROFILE IS CHANGING.
															//profile1.clientAccess.accessSharePolling =
															# region 
															//Removing Enable/Disable annoatation tools
															/*
															if(profile.ClientId==clientProf.ClientId)
															{
																if(clientProf.clientAccess.annotationRights)
																{
																	ClientUI.getInstance().enableWhiteBoardTools(true);
																}
																else
																{
														
																	ClientUI.getInstance().enableWhiteBoardTools(false);
																}
															}
															*/	
															#endregion 
															////Trace.WriteLine("Condition true ClientUI.getInstance().ChangeParticipentsListControl(true )"); 
															///



															//Zaeem View
															// i dont know why it is here and what it does ?
															// this line may produce Exception as well.
															ClientUI.getInstance().ChangeParticipentsListControl(true);													
													
															
												
															//for(int index=0; index<=)
															//Zaeem View
															// Why this document sharing block is here ?
															# region document sharing block
															for(int	ListShareDocs =	0 ;	ListShareDocs <	ClientUI.getInstance().arraydocumentSharing.Count ;	ListShareDocs++)
															{															
									
																Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[ListShareDocs];
																documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
																control.m_bIsAllowedToAddAnnotations=false;
																control.chkThumbNail.Checked=false;
																control.chkThumbNail.Visible=true;
																control.chkShowThumbNails.Visible=true;
													
																control.chkThumbNail.Enabled=false;
																control.annotationBrowser.IsAttendee=true;
													
																//control.chkShowThumbNails.Enabled =false;
																//control.AttendeeFormDocument();
																														
															}
															# endregion

														}
														catch(Exception exp)
														{
															WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 1344",exp,null,false);	
														}
													}
														# endregion

														# region Presenter Block
														
													else if(clientProf.clientType == ClientType.ClientPresenter )
													{
														try
														{
															//Removing Enable/Disable annoatation tools
															// By Zaeem 
															// I am supposing that who ever is the presenter 
															// he should have the drawing rights;
															// that should be written once in the code 
															// ClientUI.getInstance() should check for not= Null;
															//ClientUI.getInstance().enableWhiteBoardTools(true);


															//Zaeem View 
															// these blocks shd not be in the Attendee Update Msg		
															// DOCSHARING BLOCK
															# region Why this doc sharing block is here ?
															for(int	ListShareDocs =	0 ;	ListShareDocs <	ClientUI.getInstance().arraydocumentSharing.Count ;	ListShareDocs++)
															{															
									
																Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[ListShareDocs];
																documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
																control.m_bIsAllowedToAddAnnotations=true;
																control.chkThumbNail.Checked=true;

																control.chkThumbNail.Visible=true;
																control.chkThumbNail.Enabled=true;
																control.chkShowThumbNails.Visible=true;
																//control.chkShowThumbNails.Enabled =true;
										
																control.annotationBrowser.IsAttendee=false;																					
																//control.HostFormPresentation();
															}
															# endregion
															////Trace.WriteLine("Condition true ClientUI.getInstance().ChangeParticipentsListControl(false)"); 
															///

															//Zaeem View 
															// Again what it does and why its here ?
															ClientUI.getInstance().ChangeParticipentsListControl(false);
														}
														catch(Exception exp)
														{
															WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 1379 receivethread ==>attendee update if (presenter);",exp,null,false);	
														}
														
													}
													# endregion


												}//end if(bChangeInterface)						
												
												/*
												 * Assign Current profile of network manager with new profile
												 * profile is network manager variable &  clientProf is new profile that came in message
												 * */
												profile = clientProf;
												////Trace.WriteLine("Update Main Menu Acccess Client.ClientUI.getInstance().UpdateMainMenuAccess();");
												///This wil Update the main Menu (Enable , disable according to rights)
												Client.ClientUI.getInstance().UpdateMainMenuAccess();
										
												//Zaeem View 
												// Very BAd piece of code 
												//this check already dealt above.
												if(bChangeInterface)
												{
													# region D/V 
													//checking our profile is update, in case of true OurProfileIsUpdate is true
													// Zaeem View ==> I think "IF" we are going to be the presenter in this check
													if(clientProf.clientType==ClientType.ClientPresenter && clientProf.ClientId==profile.ClientId)
													{
														OurProfileIsUpdate=true;				
													}
													try
													{
														bChangeInterface=false;

														
														// Zaeem View 
														// Why list is cleared here.
														// May be its updated again
														ClientUI.getInstance().listParticipents.Items.Clear();



														// Zaeem View 
														// Again , this check is does'nt look good to me.
														for(int z = 0 ; z < ClientUI.getInstance().arrayParticipents.Count ; z++)
														{
													
															// Previous clientProfile is Updates , so i think no use of the above check.
															ClientProfile clientTemp = (ClientProfile)ClientUI.getInstance().arrayParticipents[z];																											
															if(clientTemp.clientType==ClientType.ClientHost)
															{
																if(OurProfileIsUpdate==true)
																{
																	// Zaeem View
																	// Why they are made false here ?
																	// thats totally wrong i think
																	clientTemp.clientAccess.annotationRights=false;
																}
			
															}
															////Trace.WriteLine("ReInsertClientProfile(clientTemp,z) is going to call");															
															///
															// Now after clearing the list participent, They are all reinserted again
															// But this methdology needs to be checked
															// I think there shd be an optimised way for that
															// this method needs a detailed review
															ReInsertClientProfile(clientTemp,z);
														}
														Trace.WriteLine("No of participents in listparticipents & arrayparticipents :::: "  +ClientUI.getInstance().listParticipents.Items.Count.ToString() +  "  "+  ClientUI.getInstance().arrayParticipents.Count.ToString());
														bFound = true;
													}

													catch(Exception exp)
													{
														WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 1408 receivethread ==>attendee update if (presenter)==> if(bChangeInterface);",exp,null,false);	
													}
													# endregion 
												
												}//if(bChangeInterface)
													
												#endregion
											}//end of ==>  if(clientProf.ClientId == profile.ClientId) // if this is the same user who shared the audio 
											

											// This Method will run for those for which attendee Update is called 
											// and performs all the list related operations fot them
											// controlMessage.SenderID==>Sender ID from the Message.
											// i					  ==>index in the participant list
											//clientProf              ==>profile coming with the message
											//client                  ==>In loop  
											UpdateClientProfile(controlMessage.SenderID,i,ref client,ref clientProf);
											
											break;
										}//end if(clientProf.ClientId == client.ClientId)									
										
										
										if(bFound)
											break;
									}
								}
								//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After AttendeeUpdate",null,null,false);
							}//end if(controlMessage.Code == ControlCode.AttendeeUpdate)							
								

						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>1342 receivethread==>control msg",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After ControlMessage",null,null,false);	
					}					
						#endregion
					
						#region annotation message
					else if(msg.GetType().Equals(typeof(AnnotMsg)))				
					{
						try
						{
							//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before AnnotMsg",null,null,false);
							AnnotMsg amsg=  (AnnotMsg)msg;
							switch(amsg.annotationType)
							{
								case AnnotationMessageType.WEB:
									HandleWebBrowserAnnotation(amsg);
									break;					
								default:								
									HandlePDFAnnotation(amsg);
									break;

							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>1372 receivethread==>annotation msg",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After AnnotMsg",null,null,false);
						
					}
						#endregion
		
						#region annotation control messcage
					else if(msg.GetType().Equals(typeof(AnnotationControlMessage)))
					{
#if dbgNetworkLayer
						//Trace.WriteLine("Message Name :AnnotationControlMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif			
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before AnnotationControlMessage",null,null,false);
						try
						{
												
							AnnotationControlMessage aMsg = (AnnotationControlMessage)msg;
							if(aMsg.ControlMessageType == ControlType.DocumentSharing)
							{
								for(int	i =	0 ;	i <	ClientUI.getInstance().arraydocumentSharing.Count ;	i++)
								{															
									
									Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
									documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
									if(control.sessionID ==	aMsg.sessionID)
									{																		
										control.chkThumbNail.Checked = false;
										control.m_bIsAllowedToAddAnnotations = aMsg.IsAllowed;
										control.chkThumbNail.Enabled = aMsg.IsAllowed;					
										break;
									}								
								}		

							}
							else if(aMsg.ControlMessageType == ControlType.Browser)
							{
								for(int	i =	0 ;	i <	ClientUI.getInstance().arrayWebControl.Count;	i++)
								{								

									browser browserControl = ClientUI.browserControl;								
									if(browserControl.sessionID ==	aMsg.sessionID)
									{
										browserControl.chk_AllowAnnotation.Checked = false;
										browserControl.chk_AllowAnnotation.Checked = aMsg.IsAllowed;

																									
										break;
									}															
							
								}

							}
						}

						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>1490 receivethread==>annotation control msg",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After AnnotationControlMessage",null,null,false);
					
					}
						#endregion
			
						#region UpdateMessages					
					else if(msg.GetType().Equals(typeof(StateUpdateMessage)))
					{						
#if dbgNetworkLayer
						//Trace.WriteLine("Message Name :StateUpdateMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif						
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before StateUpdateMessage",null,null,false);
						try
						{
																		
							StateUpdateMessage updateMsg = (StateUpdateMessage )msg;						
							if(updateMsg.updateType == UpdateTypeCode.UpdateChat) // chat updation messages
							{							
								foreach(IMChatMessage chatMsg in updateMsg.dataArray)
								{		
									ClientUI.getInstance().chatControl1.messagePool.Add(chatMsg);
								}
								//ClientUI.getInstance().chatControl1.consumeFunction();
							}
							else if(updateMsg.updateType == UpdateTypeCode.UpdateApplicationSharingCompund)
							{
								//Trace.WriteLine("AppShare message come");
								foreach(MessageObject obj in updateMsg.dataArray)
									this.myRecvMsgList.Add(obj);
							}
							else if(updateMsg.updateType == UpdateTypeCode.UpdateQA)
							{ 
								
								foreach(QAQuestion qaMsg in updateMsg.dataArray)
								{
									ClientType type = NetworkManager.getInstance().profile.clientType;
									if((type == ClientType.ClientPresenter) || (type == ClientType.ClientHost))
									{
										ClientUI.getInstance().qaPresenter.HandleQuestion(qaMsg);
										Win32.USER32.SendMessage(ClientUI.getInstance().Handle,Win32.USER32.WM_CREATE_PRESENTER_QA_WIDOW,new IntPtr(),new IntPtr());
									}
								}								
							}
							else if(updateMsg.updateType == UpdateTypeCode.UpdateWhiteboard)
							{
							
								for(int i=0; i<updateMsg.dataArray.Count;i++)
								{
									if(updateMsg.dataArray[i] is WhiteboardMessage)
									{
										ClientUI.getInstance().whiteBoard.eventStore.Add((WhiteboardMessage)updateMsg.dataArray[i]);
										//Trace.WriteLine("Whiteboard Message");
										continue;
									}
									if(updateMsg.dataArray[i] is DrawingClearMessage)
									{									
										ClientUI.getInstance().whiteBoard.Clear(false,((DrawingClearMessage)updateMsg.dataArray[i]).SenderID);
										//ClientUI.getInstance().whiteBoard.Refresh();
										//Trace.WriteLine("Drawing Clear Message");	
									}
								}
								//							foreach(WhiteboardMessage wMsg in updateMsg.dataArray)
								//							{
								//								ClientUI.getInstance().whiteBoard.eventStore.Add(wMsg);
								//							}
							}
							else if(updateMsg.updateType == UpdateTypeCode.UpdateWebDocument)
							{
							
								//MessageBox.Show("else if(updateMsg.updateType == UpdateTypeCode.UpdateWebBrowser)");
							
								//Trace.WriteLine("UpdateTypeCode.UpdateWebBrowser--------");
								//Trace.WriteLine("UpdateTypeCode.UpdateWebBrowser--------");
							
								foreach(AnnotMsg amsg in updateMsg.dataArray)
								{	
									HandlePDFAnnotation(amsg);
								}
							}
							else if(updateMsg.updateType == UpdateTypeCode.UpdateWebBrowser)
							{
							
								//MessageBox.Show("else if(updateMsg.updateType == UpdateTypeCode.UpdateWebBrowser)");
							
								//Trace.WriteLine("UpdateTypeCode.UpdateWebBrowser--------");
								//Trace.WriteLine("UpdateTypeCode.UpdateWebBrowser--------");
							
								foreach(AnnotMsg amsg in updateMsg.dataArray)
								{	
									//AnnotMsg amsg=  (AnnotMsg)msg;																	
									HandleWebBrowserAnnotation(amsg);								
								
								}
								//							foreach(MsgWebBrowseMessage uMsg in updateMsg.dataArray)
								//							{
								//								if(!uMsg.active)
								//									continue;
								//                                
								//								ClientProfile prof = GetClientProfileFromList(uMsg.ClientId);
								//								if(prof != null)
								//									continue;
								//									
								//								bool bFound = false;
								//								for(int	i =	0 ;	i <	ClientUI.getInstance().arrayWebControl.Count;	i++)
								//								{								
								//									Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arrayWebControl[i];
								//									browser browserControl = (browser)tabPage.Control;								
								//									if(browserControl.sessionID ==	uMsg.webBrowseID)
								//									{
								//										bFound = true;
								//										browserControl.intialMsg = uMsg;																	
								//										break;
								//									}															
								//								
								//								}
								//								if(!bFound)
								//								{							
								//
								//									ClientUI.getInstance().CreateWebControlWindow(uMsg);
								//								}
								//							}
								//
								//							for(int	i =	0 ;	i <	ClientUI.getInstance().arrayWebControl.Count;	i++)
								//							{								
								//								Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arrayWebControl[i];
								//								browser browserControl = (browser)tabPage.Control;								
								//								if(browserControl.intialMsg != null)
								//								{
								//									browserControl.msgPool.Add(browserControl.intialMsg);																					
								//									MsgWebBrowseMessage uMsg = (MsgWebBrowseMessage) browserControl.intialMsg;
								//									/*if(!uMsg.active)
								//												tabPage.Title += " - Disabled";
								//												*/
								//									browserControl.IsAttendee = true;
								//									//browserControl.IsClosed = !uMsg.active;
								//								}								
								//							}
								//							ClientUI.getInstance().OnJoinConversationUpdateWebBrowsers();
								
							}
						
							else if(updateMsg.updateType == UpdateTypeCode.UpdateWebPresentations)
							{
								int nIndex = 0;		
								ClientUI.getInstance().manageContentPresentations.listWebPresentations.Items.Clear();
								foreach (WebPresentationEntry entry in updateMsg.dataArray)
								{									
									string str = entry.path;
									ListViewItem lv =  ClientUI.getInstance().manageContentPresentations.listWebPresentations.Items.Add(entry.title);									
									lv.SubItems.Add(str);								
									lv.Tag =nIndex;
									nIndex++;

								}
								ClientUI.getInstance().manageContentPresentations.ChangeInfoStatus(false);
								ClientUI.getInstance().WebPresentationsArray =(ArrayList) updateMsg.dataArray.Clone();
								
							}							
							else if(updateMsg.updateType == UpdateTypeCode.UpdateWebFiles)							
							{
								
								//									int nIndex=0;
								//									ClientUI.getInstance().manageContentWebFiles.listWebFiles.Items.Clear();
								//									foreach (WebUploadedFiles file in updateMsg.dataArray)
								//									{									
								//										ListViewItem lv =  ClientUI.getInstance().manageContentWebFiles.listWebFiles.Items.Add(file.name);
								//										//lv.SubItems.Add(file.name);									
								//										lv.Tag =nIndex;
								//										nIndex++;
								//									}
								//									ClientUI.getInstance().manageContentWebFiles.ChangeInfoStatus(false);
								//								
								//									ClientUI.getInstance().webFiles =(ArrayList) updateMsg.dataArray.Clone();
                                
							}
							else if(updateMsg.updateType == UpdateTypeCode.UpdateWebPresentations)
							{
								int nIndex = 0;		
								ClientUI.getInstance().manageContentPresentations.listWebPresentations.Items.Clear();
								foreach (WebPresentationEntry entry in updateMsg.dataArray)
								{									
									string str = entry.path;
									ListViewItem lv =  ClientUI.getInstance().manageContentPresentations.listWebPresentations.Items.Add(entry.title);									
									lv.SubItems.Add(str);								
									lv.Tag =nIndex;
									nIndex++;

								}
								ClientUI.getInstance().manageContentPresentations.ChangeInfoStatus(false);
								
								ClientUI.getInstance().WebPresentationsArray =(ArrayList) updateMsg.dataArray.Clone();
							}							
							else if(updateMsg.updateType == UpdateTypeCode.UpdateWebFiles)							
							{
								
								//									int nIndex=0;
								//									ClientUI.getInstance().manageContentWebFiles.listWebFiles.Items.Clear();
								//									foreach (WebUploadedFiles file in updateMsg.dataArray)
								//									{									
								//										ListViewItem lv =  ClientUI.getInstance().manageContentWebFiles.listWebFiles.Items.Add(file.name);
								//										//lv.SubItems.Add(file.name);									
								//										lv.Tag =nIndex;
								//										nIndex++;
								//									}
								//									ClientUI.getInstance().manageContentWebFiles.ChangeInfoStatus(false);
								//								
								//									ClientUI.getInstance().webFiles =(ArrayList) updateMsg.dataArray.Clone();

							}
							else if(updateMsg.updateType == UpdateTypeCode.UpdateWebBookmarks)
							{
								//									int nIndex=0;
								//									ClientUI.getInstance().manageContentBookmarks.listWebPolls.Items.Clear();
								//									foreach (WebBookMark poll in updateMsg.dataArray)
								//									{								
								//										ListViewItem lv =  ClientUI.getInstance().manageContentBookmarks.listWebPolls.Items.Add(poll.title);
								//										lv.SubItems.Add(poll.Url);									
								//										lv.Tag =nIndex;
								//										nIndex++;
								//									}
								//									ClientUI.getInstance().manageContentBookmarks.ChangeInfoStatus(false);								
								//									ClientUI.getInstance().WebBookMarksArray =(ArrayList) updateMsg.dataArray.Clone();
							}
							else if(updateMsg.updateType == UpdateTypeCode.UpdateWebPolls)
							{
								//									int nIndex=0;
								//									ClientUI.getInstance().manageContentWebPolls.listWebPolls.Items.Clear();
								//									foreach (WebPoll poll in updateMsg.dataArray)
								//									{
								//										string pollstring = poll.question;
								//										ListViewItem lv =  ClientUI.getInstance().manageContentWebPolls.listWebPolls.Items.Add(poll.question);
								//										//lv.SubItems.Add(poll.question);
								//									
								//										lv.Tag =nIndex;
								//										nIndex++;
								//
								//									}
								//									ClientUI.getInstance().manageContentWebPolls.ChangeInfoStatus(false);
								//								
								//									ClientUI.getInstance().webPollsArray =(ArrayList) updateMsg.dataArray.Clone();

							}	
							else if(updateMsg.updateType == UpdateTypeCode.UpdateWebEvaluations)
							{
								//									int nIndex=0;
								//									foreach (WebPoll poll in updateMsg.dataArray)
								//									{
								//										ClientUI.getInstance().manageContentEvaluations.listEvaluation.Items.Clear();
								//										string pollstring = poll.question;
								//										ListViewItem lv =  ClientUI.getInstance().manageContentEvaluations.listEvaluation.Items.Add(poll.question);
								//										//lv.SubItems.Add(poll.question);
								//									
								//										lv.Tag =nIndex;
								//										nIndex++;
								//
								//									}
								//									ClientUI.getInstance().manageContentEvaluations .ChangeInfoStatus(false);
								//								
								//									ClientUI.getInstance().WebEvaluationsArray =(ArrayList) updateMsg.dataArray.Clone();

							}	
							else if(updateMsg.updateType == UpdateTypeCode.UpdatePolling)
							{
								foreach(object obj in updateMsg.dataArray)
								{
									DocumentSharing doc = (DocumentSharing)obj;
									if(!doc.bEnabled)
										continue;
									ClientProfile cp = GetClientProfileFromList(doc.senderProfile.ClientId);
									if(cp == null)
										continue;

									bool bFound = false;
									for(int	i =	0 ;	i <	ClientUI.getInstance().arraydocumentSharing.Count ;	i++)
									{	
							
										DocumentSharing Message = (DocumentSharing)doc;
										Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
										documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
										if(control.sessionID ==	Message.sessionID)
										{
											control.MessagePool.Add(Message);
											bFound = true;
											break;
										}								
									}		
									if(!bFound)
									{
										ClientUI.getInstance().Invoke(ClientUI.getInstance().CreateDocumentSharingWindowEx,new object[]{msg});
							
									}

								}
							}
							else if(updateMsg.updateType == UpdateTypeCode.UpdatePolling)
							{								
								foreach(object obj in updateMsg.dataArray)
								{	
									if(!obj.GetType().Equals(typeof(NewPollMessage)))
										continue;
                                    
									NewPollMessage pollMessage = (NewPollMessage)obj;                                    
									if(profile.ClientId == pollMessage.hostID)
										continue;
									if(pollMessage.active)
									{
										for(int i = 0 ; i < ClientUI.getInstance().arrayParticipents.Count ; i++)
										{
											ClientProfile profileEx = (ClientProfile)ClientUI.getInstance().arrayParticipents[i];
											if(profileEx.ClientId == pollMessage.hostID)
											{	
          
												WebMeeting.Polling.Polling pControl = new WebMeeting.Polling.Polling ();
												pControl.enabled = true;
										
												//NewPollMessage pollMessage = (NewPollMessage)avaiLablePollMessages[listPolls.SelectedItems[0].Index];
				
												pControl.sessionID = pollMessage.sessionID;
												pControl.question = pollMessage.Question;
												pControl.locked = pollMessage.active;
												pControl.choices = pollMessage.choices;
												if(pControl.enabled == false)
													pControl.attendeeForm.DisableForm();
												pControl.ClientID = NetworkManager.getInstance().profile.ClientId;
												pControl.ClientName = NetworkManager.getInstance().profile.Name;
												pControl.attendeeForm.changeInterfaceToEvaluation();
												pControl.thisPollType = pollMessage.pollType;

												if(pollMessage.IsEvaluation)
													ClientUI.getInstance().evaluationWindowsOfAttendee.Add(pControl);
												else
													pControl.ShowAttendeeForm(pollMessage.pollType);
											

			
												/*
															ListViewItem lv = ClientUI.getInstance().listPolls.Items.Add(profile.Name);
															if(pollMessage.Question.Length > 25)
																lv.SubItems.Add(pollMessage.Question.Substring(0,25));
															else
																lv.SubItems.Add(pollMessage.Question);

															ClientUI.getInstance().listPolls.SetImage(lv.Index,0,0);
															ClientUI.getInstance().avaiLablePollMessages.Add(pollMessage);									
															*/
											}
										}										
									}
								}
							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>1429 receivethread==>Update msg",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After StateUpdateMessage",null,null,false);
						
					}
						#endregion
					
						#region Chat
					else if(msg.GetType().Equals(typeof(IMChatMessage))) // per1
					{
#if dbgNetworkLayer
						//Trace.WriteLine("Message Name :IMChatMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif							
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before IMChatMessage",null,null,false);
						try
						{

							try
							{
								chatControl.getInstance().AliveprocessingThread();
							}
							catch(Exception exp)
							{
								WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>1960 == before call forchatControl.getInstance().AliveprocessingThread(); in  receivethread==>Chat",exp,null,false);	
								
							}
	
							IMChatMessage chatMsg = (IMChatMessage)msg;
							/*
					
									if(Client.ClientUI.getInstance().WindowState == FormWindowState.Minimized)
										if(profile.clientType == ClientType.ClientAttendee)
											Client.ClientUI.getInstance().Invoke(Client.ClientUI.getInstance().FnShowChatWindow,
												new object[] { chatMsg.sender + " has sent you a chat message",chatMsg.IMMessage});
										else 
											Client.ClientUI.FlashWindow(Client.ClientUI.getInstance().Handle,true);
									*/
							//	Trace.WriteLine("msg.MessageType : " + msg.MessageType.ToString() + " MessageObject.BroadcastMessageFlag" +MessageObject.BroadcastMessageFlag.ToString());
						
							
							
							//this condition will work when message is not broadcast
							if ((msg.MessageType & MessageObject.BroadcastMessageFlag) == 0)
							{
								
								//this is a private message
								bool found = false;

								if(chatMsg.RecipientId != profile.ClientId)
									continue;


								//locate if there is already opened window for this user
								for(int i = 0 ; i < ClientUI.getInstance().chatWindowsArray.Count; i++)
								{
									ClientUI.ClientProfileWrapper CP =(ClientUI.ClientProfileWrapper ) ClientUI.getInstance().chatWindowsArray[i];
									if(CP.nUniqueID== chatMsg.senderID) //if found
									{
										//place the message in chat control message pump
										chatControl UserChatCtrl = (chatControl)CP.tabPage.Control;
										UserChatCtrl.messagePool.Add(chatMsg);
										found = true;
										break;
									}																			
								}							

								if(!found)
								{
									// if no chat window found. create a new tab page .. add it to tab control
									//Trace.WriteLine("flag is true");
									ChildWindowCreationEvent(msg);							
							
                                    	                                                                   
								}
								
							}
							else
							{	//this else is equal to this condition		
								//if ((msg.MessageType & MessageObject.BroadcastMessageFlag) ==MessageObject.BroadcastMessageFlag )
								//this message is broadcast
								if (profile.clientType == ClientType.ClientHost)
									ClientUI.getInstance().chatControl1.messagePool.Add(chatMsg);

								else if((chatMsg.m_ForClientType == ClientType.ClientNone) || (chatMsg.m_ForClientType == profile.clientType))
								{
									//Trace.WriteLine("ComboBox");
									ClientUI.getInstance().chatControl1.messagePool.Add(chatMsg);						
								}
							}							
							
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>1429 receivethread==>Chat",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After IMChatMessage",null,null,false);
					}
						#endregion
					
						#region Q/A
					else  if(msg.GetType().Equals(typeof(QAQuestion)))
					{							
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before QAQuestion",null,null,false);
						try
						{
#if dbgNetworkLayer
							//Trace.WriteLine("Message Name :QAQuestion , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif																													
							QAQuestion qa = (QAQuestion)msg;
							if(qa.senderProfile.ClientId != profile.ClientId)
							{
								ClientType type = NetworkManager.getInstance().profile.clientType;								
								//if((type== ClientType.ClientHost)||(type== ClientType.ClientHost)) 
								if(ClientUI.getInstance()!=null)
								{
									if(ClientUI.getInstance().IfIamthePresenter())
									{
										ClientUI.getInstance().qaPresenter.HandleQuestion(qa);
										Win32.USER32.SendMessage(ClientUI.getInstance().Handle,Win32.USER32.WM_CREATE_PRESENTER_QA_WIDOW,new IntPtr(),new IntPtr());
									}
								}

							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 1916 receivethread ",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After QAQuestion",null,null,false);	
                        
					}
					else if(msg.GetType().Equals(typeof(QAAnswer)))
					{							
						try
						{																														
						
							QAAnswer qa = (QAAnswer)msg;
							if(qa.senderProfile.ClientId != profile.ClientId)
							{
								//Zaeem hide that code
								//if(NetworkManager.getInstance().profile.clientType==ClientType.ClientAttendee)
								//{
								//zaeem code 
								// 
								//MessageBox.Show(profile.ToString()+"==>");
								if(qa.RecipientID==profile.ClientId)
								{
									ClientUI.getInstance().bSendIt = false;
									ClientUI.getInstance().qaAttendee.HandleAnswer(qa);
									Win32.USER32.SendMessage(ClientUI.getInstance().Handle,Win32.USER32.WM_CREATE_ATTENDEE_QA_WIDOW,new IntPtr(),new IntPtr());
									ClientUI.getInstance().bSendIt = true;
								}
								//}
							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 1944 receivethread  QA Answer",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After QAAnswer",null,null,false);
					}
						#endregion
					
						#region webfiles
					else if(msg.GetType().Equals(typeof(WebFileMessage)))
					{						
#if dbgNetworkLayer
						//Trace.WriteLine("Message Name :WebFileMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif						
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before WebFileMessage",null,null,false);
						try
						{
																																										
							if(msg.SenderID == profile.ClientId)
								continue;
							WebFileMessage Webmsg = (WebFileMessage)msg;
							if(Webmsg.senderprofile.ClientId != profile.ClientId)
							{
								WebFIleInformation file = new WebFIleInformation();
								file.fileInformation = Webmsg.FileInformation;
								file.label1.Text = Webmsg.senderprofile.Name + " wants you to download the following file ";
								file.label2.Text = file.fileInformation.filePath;
								file.ShowDialog();
								file.Invalidate();
							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>1429 receivethread==>webfiles",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After WebFileMessage",null,null,false);
					}
						#endregion
						//		
						#region whiteboard
					else if(msg.GetType().Equals(typeof(WhiteboardMessage)))
					{	
		                				
						try
						{

							WhiteboardMessage whiteBoardMsg = (WhiteboardMessage)msg;
							if((ushort)msg.MessageType  == (ushort)MessageObjectType.MsgAppSharingAnnotation)	
							{

								Client.ClientUI client = ClientUI.getInstance();
								//Trace.WriteLine("Annotation Message Come");
								for(int xx = 0 ; xx < client.ApplicationSharingTabPagesArray.Count ; xx++)
								{
									Crownwood.Magic.Controls.TabPage tabPage =  (Crownwood.Magic.Controls.TabPage ) client.ApplicationSharingTabPagesArray[xx];
									ApplicationSharing control = (ApplicationSharing)tabPage.Control;
									if(control.nSessionID == whiteBoardMsg.nSessionId)
									{				
										//Trace.WriteLine("Annotation message is stored in eventstore for further use of consumethread ");
										control.whiteBoard.eventStore.Add(msg);
										break;								
									}
									
								}			
								continue;							
							}
							else if((msg.MessageType & (ushort)MessageObjectType.MsgHideWhiteBoard) == (ushort)MessageObjectType.MsgHideWhiteBoard)	
							{	
								ClientUI.getInstance().ShowWhiteBoard(false);							
							}
							else if((msg.MessageType & (ushort)MessageObjectType.MsgWhiteboard) == (ushort)MessageObjectType.MsgWhiteboard)	
							{	
								ClientUI.getInstance().JustOpenWhiteBoard();							
							}


							else if((msg.MessageType & (ushort)MessageObjectType.MsgShowWhiteBoard) == (ushort)MessageObjectType.MsgShowWhiteBoard)	
							{	
								ClientUI.getInstance().ShowWhiteBoard(true);
							}						
							else
								ClientUI.getInstance().whiteBoard.eventStore.Add(msg);
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>1429 receivethread==>Whiteboard",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After WhiteboardMessage",null,null,false);

					}
						#endregion
		
						#region Desktop Sharing Control Messages
					else if(msg.GetType().Equals(typeof(DesktopSharingControlMessage)))		
					{
#if dbgNetworkLayer
						//Trace.WriteLine("Message Name :DesktopSharingControlMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif						
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before DesktopSharingControlMessage",null,null,false);
						try
						{

							DesktopSharingControlMessage DSCMsg = (DesktopSharingControlMessage)msg;
							if(DSCMsg.Status == 0) // Initializiation Message
							{
								if((DSCMsg.Start==true) && (ClientUI.getInstance().desktopSharedProfile == null))
								{
									ClientProfile p = GetClientProfileFromList(DSCMsg.senderID);
									if(p != null)
									{					
										if(p.ClientId == DSCMsg.senderID)
										{
											int nTemp = DSCMsg.senderID;
											DSCMsg.senderID = DSCMsg.RecipientId;
											DSCMsg.SenderID = DSCMsg.RecipientId;
											DSCMsg.RecipientId = nTemp;
											DSCMsg.Status = 1;
											if(MessageBox.Show("\"" + p.Name + "\" wants to share your desktop. Do you want to start Remote Control ","WebMeeting",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.Yes)
											{
												DSCMsg.Start = true;
												SendLoadPacket(msg);
											}
											else
											{	
												DSCMsg.Start = false;
												SendLoadPacket(msg);
											}
										}							
									}
								}
								else
								{
									int nTemp = DSCMsg.senderID;
									DSCMsg.senderID = DSCMsg.RecipientId;
									DSCMsg.SenderID = DSCMsg.RecipientId;
									DSCMsg.RecipientId = nTemp;
									DSCMsg.Status =99;
									DSCMsg.Start = false;
									SendLoadPacket(msg);
								}
							
							
							}
							else if(DSCMsg.Status == 99)
							{
								ClientProfile p = GetClientProfileFromList(DSCMsg.senderID);							
								if(p != null)
								{
									MeetingAlerts alert=new MeetingAlerts();
									alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,p.Name + " already has a desktop sharing session. Please wait for previous session to end",true,false);
									alert.Dispose();
									//MessageBox.Show(p.Name + " already has a desktop sharing session. Please wait for previous session to end","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
									ClientUI.getInstance().desktopSharedProfile = null;
								}
							}
							else if(DSCMsg.Status == 1)
							{														
								ClientProfile p = GetClientProfileFromList(DSCMsg.senderID);							
								if(DSCMsg.Start == false)
								{
									if(p != null)
									{
										MeetingAlerts alert=new MeetingAlerts();
										alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"\"" + p.Name + "\" has declined your request for Remote Control",true,false);
										alert.Dispose();
										//MessageBox.Show("\"" + p.Name + "\" has declined your request for Remote Control","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Information);
										ClientUI.getInstance().desktopSharedProfile = null;
									}
								}
								else
								{
									int nSession =0;
									int nTemp = DSCMsg.senderID;
									GetUniqueSessionID(ref nSession);
									DSCMsg.ServerToClientSessionID =nSession;		
									DSCMsg.senderID = DSCMsg.RecipientId;
									DSCMsg.SenderID = DSCMsg.RecipientId;
									DSCMsg.RecipientId = nTemp;
									DSCMsg.Status = 2;
								
									//initialize the client											
									ClientUI.getInstance().InitializeClient(DSCMsg,p);
									SendLoadPacket(msg);
								}

							}
							else if(DSCMsg.Status == 2)
							{	
								ClientProfile p = GetClientProfileFromList(DSCMsg.senderID);
								ClientUI.getInstance().InitializeServer(DSCMsg,p);
							}
							else if(DSCMsg.Status == -1)
							{
								if(DSCMsg.Start==false)
								{						
								
									Client.ClientUI.getInstance().desktopSharedProfile=null;
									Client.ClientUI.getInstance().TerminateDesktopSharing();

								
								
									System.GC.Collect();
									System.GC.WaitForPendingFinalizers();
								


								}
							}
						}	
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>1429 receivethread==>Desktop sharing",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After DesktopSharingControlMessage",null,null,false);	
			
					}
						#endregion
				
						#region poll
					else if(msg.GetType().Equals(typeof(PollAnswerMessage)))
					{
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before PollAnswerMessage",null,null,false);
						try
						{
#if dbgNetworkLayer
							//Trace.WriteLine("Message Name :PollAnswerMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif					
							PollAnswerMessage pollAnswer = (PollAnswerMessage) msg;
							for(int i = 0 ; i < ClientUI.getInstance().arrayPolling.Count ; i++)
							{
								//Client.PollingTabPage pollTabPage = (Client.PollingTabPage)ClientUI.getInstance().arrayPolling[i];
								//Client.PollingTabPage pollTabPage = (Client.PollingTabPage)ClientUI.getInstance().arrayPolling[i];
								WebMeeting.Client.Polling.frm_PollPresenter frm_PollPresenter_Answer=(WebMeeting.Client.Polling.frm_PollPresenter)ClientUI.getInstance().arrayPolling[i];

								//if(pollTabPage.PollingControl.sessionID == pollAnswer.sessionID)
								if(frm_PollPresenter_Answer.PollingControl.sessionID == pollAnswer.sessionID)
									
								{
									//pollTabPage.PollingControl.HandlePollingAnswer(pollAnswer);
									frm_PollPresenter_Answer.PollingControl.HandlePollingAnswer(pollAnswer);
									break;
								}
							}
						}
						catch (Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2190 receivethread==>Poll",exp,null,false);	
						
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After PollAnswerMessage",null,null,false);
					}
					else if(msg.GetType().Equals(typeof(PollResultsMessage)))
					{
#if dbgNetworkLayer
						//Trace.WriteLine("Message Name :PollResultsMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif									
						/*								if(pollingTabControl.TabPages.Count >= 1)
															{		
																try
																{
																	int totalSubPages = pollingTabControl.TabPages.Count ;
																	for(int i=0; i< totalSubPages ;i++) // by kamran
																	{
																		SharedPoolResult="";
																		//pollingTabControl.TabPages[0].Control
																		Invoke(DeleteDocumentSharingWindow,new object[]{tabPollingGlobal});	
																		pollingTabControl.TabPages.RemoveAt(0);
																	}
																}
																catch(Exception ex)
																{
																	ex = ex;
																}
																finally
																{
																	pollingTabControl.TabPages.Clear();
																}*/
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before PollResultsMessage",null,null,false);
						try
						{
							PollResultsMessage pm = (PollResultsMessage) msg;
							if(pm.isShow)
							{						
								//Trace.WriteLine("Method is not called");
								ClientUI.getInstance().bSendIt = false;
								Client.ClientUI.getInstance().ShowPollMessages(pm);
								ClientUI.getInstance().bSendIt=true;
							}
							else
							{
								//Trace.WriteLine("Method is called");
								ClientUI.getInstance().ClosePoolResultWindow(pm);
							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2240 receivethread==>PollResultsMessage",exp,null,false);	
						
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After PollResultsMessage",null,null,false);
					}
					else if(msg.GetType().Equals(typeof(TabPagesSyncrhonizationMsessage)))
					{					

						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before TabPagesSyncrhonizationMsessage",null,null,false);	
						try
						{
#if dbgNetworkLayer
							//Trace.WriteLine("Message Name :TabPagesSyncrhonizationMsessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif
							if(msg.SenderID != profile.ClientId)
							{
								TabPagesSyncrhonizationMsessage Msg = (TabPagesSyncrhonizationMsessage)msg;
								ClientUI.getInstance().bSendIt = false;
								ClientUI.getInstance().Invoke(ClientUI.getInstance().SynchWindowProc,new object[] { Msg });							
								ClientUI.getInstance().bSendIt =true;
							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2261 receivethread==>PollResultsMessage",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After TabPagesSyncrhonizationMsessage",null,null,false);	
					}
					else if(msg.GetType().Equals(typeof(NewPollMessage)))
					{

						//MessageBox.Show("==>");
							
#if dbgNetworkLayer
						//Trace.WriteLine("Message Name :NewPollMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before NewPollMessage",null,null,false);	
						NewPollMessage pollMessage  = (NewPollMessage)msg;
						if(pollMessage.active == true)
						{
							if(pollMessage.hostID != profile.ClientId)
							{
								
								bool identity_flag=false;
								
								for(int i = 0 ; i < ClientUI.getInstance().arrayParticipents.Count ; i++)
								{
									ClientProfile profileEx = (ClientProfile)ClientUI.getInstance().arrayParticipents[i];
									if(	profileEx.ClientId == pollMessage.hostID)
									{		
										identity_flag=true;
						
										if(pControl!=null)
										{
											pControl.attendeeForm.Close();
										}


										pControl = new WebMeeting.Polling.Polling ();
										pControl.enabled = true;
									
										//NewPollMessage pollMessage = (NewPollMessage)avaiLablePollMessages[listPolls.SelectedItems[0].Index];
			
										pControl.sessionID = pollMessage.sessionID;
										pControl.question = pollMessage.Question;
										pControl.locked = pollMessage.active;
										pControl.choices = pollMessage.choices;
										pControl.anonymous=pollMessage.anonymousPoll;
										if(pControl.enabled == false)
											pControl.attendeeForm.DisableForm();
										pControl.ClientID = NetworkManager.getInstance().profile.ClientId;										
										pControl.ClientName = profileEx.Name;
										pControl.thisPollType = pollMessage.pollType;

										if(pollMessage.IsEvaluation)
											ClientUI.getInstance().evaluationWindowsOfAttendee.Add(pControl);
										else
										{
											pControl.ShowAttendeeForm(pollMessage.pollType);

										}									
								
									}
								}
								#region Short fix
								if(!identity_flag)
								{
							
							
									if(pControl!=null)
									{
										pControl.attendeeForm.Close();
									}


									pControl = new WebMeeting.Polling.Polling ();
									pControl.enabled = true;
									
									//NewPollMessage pollMessage = (NewPollMessage)avaiLablePollMessages[listPolls.SelectedItems[0].Index];
			
									pControl.sessionID = pollMessage.sessionID;
									pControl.question = pollMessage.Question;
									pControl.locked = pollMessage.active;
									pControl.choices = pollMessage.choices;
									pControl.anonymous=pollMessage.anonymousPoll;
									if(pControl.enabled == false)
										pControl.attendeeForm.DisableForm();
									pControl.ClientID = NetworkManager.getInstance().profile.ClientId;										
									pControl.ClientName ="Presenter";
									pControl.thisPollType = pollMessage.pollType;

									if(pollMessage.IsEvaluation)
										ClientUI.getInstance().evaluationWindowsOfAttendee.Add(pControl);
									else
									{
										pControl.ShowAttendeeForm(pollMessage.pollType);

									}									
								
							
								}
								#endregion 

							}

							
						}
						else if(pollMessage.active == false)
						{
							for(int i = 0 ; i < ClientUI.getInstance().pollingTabControl.TabPages.Count ; i++)
							{
								PollingTabPage pPage = (PollingTabPage)ClientUI.getInstance().pollingTabControl.TabPages[i];
								if(pPage.PollingControl.sessionID == pollMessage.sessionID)
								{
									ClientUI.getInstance().pollingTabControl.TabPages.RemoveAt(i);//(pPage);
									break;
								}
							}
												
						}
						/*
							else
							{
								if(pollMessage.IsEvaluation)
								{
									for(int i= 0 ;i < ClientUI.getInstance().evaluationWindowsOfAttendee.Count; i++)
									{
										WebMeeting.Polling.Polling pControl = (WebMeeting.Polling.Polling)ClientUI.getInstance().evaluationWindowsOfAttendee[0];
										if(pControl.sessionID == pollMessage.sessionID)
										{
											ClientUI.getInstance().evaluationWindowsOfAttendee.RemoveAt(i);
											break;
										}
										
									}
								}
							}*/
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After NewPollMessage",null,null,false);	
					}
				
						#endregion

						#region presentation thumbnail
					else if(msg.GetType().Equals(typeof(PresentationThumbnail)))
					{
#if dbgNetworkLayer
						//Trace.WriteLine("Message Name :PresentationThumbnail , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif						
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before PresentationThumbnail",null,null,false);	
						PresentationThumbnail PTmsg = (PresentationThumbnail)msg;														
						
						try
						{
							for(int	i =	0 ;	i <	ClientUI.getInstance().arraydocumentSharing.Count ;	i++)
							{
								Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
								documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
								if(control.sessionID ==	PTmsg.sessionID)
								{
									control.AddPresentationNail(PTmsg);
								
									break;
								}								
							}
						}
							
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>1429 receivethread==>Presentation thumbnail",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After PresentationThumbnail",null,null,false);	

					}
						#endregion
		
						#region Greetings Message
					else if(msg.GetType().Equals(typeof(GreetingsMessage)))
					{
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before GreetingsMessage",null,null,false);
						try
						{
#if dbgNetworkLayer
							//Trace.WriteLine("Message Name :GreetingsMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif						
							GreetingsMessage vMessage = (GreetingsMessage)msg;							
							Client.ClientUI.getInstance().ShowRecievedGreetingsMessage(vMessage);
						}									
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2423 Networkmanager ==>receivethread	#region Greetings Message",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After GreetingsMessage",null,null,false);
                        
					}
						#endregion

					
						#region Document/PPT sharing
						//TODO: Add Messeage handler when a new user joins the conference and there are already
						//some presentation
					else if(msg.GetType().Equals(typeof(DocumentSharingMouseMove)))
					{
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before DocumentSharingMouseMove",null,null,false);	
						try
						{
#if dbgNetworkLayer
							//Trace.WriteLine("Message Name :DocumentSharingMouseMove , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif
							for(int	i =	0 ;	i <	ClientUI.getInstance().arraydocumentSharing.Count ;	i++)
							{	
							
								DocumentSharingMouseMove Message = (DocumentSharingMouseMove)msg;
								Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
								documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
								if((control.sessionID == Message.sessionID))
								{
									control.MessagePool.Add(Message);								
									break;
								}								
							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2412 receivethread==>DocumentSharingMouseMove",exp,null,false);	
						
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After DocumentSharingMouseMove",null,null,false);	

					}
					else if(msg.GetType().Equals(typeof(DocumentSharing)))
					{
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before DocumentSharing",null,null,false);	
						try
						{
#if dbgNetworkLayer
							//Trace.WriteLine("Message Name :DocumentSharing, SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif						
							bool bFound = false;
							for(int	i =	0 ;	i <	ClientUI.getInstance().arraydocumentSharing.Count ;	i++)
							{	
							
								DocumentSharing Message = (DocumentSharing)msg;
								Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
								documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
								
								//ifMessage.bEnabled == false
								// Document sharing is closed 

								if((control.sessionID ==	Message.sessionID) && (Message.bEnabled == false))
								{
									ClientUI.getInstance().Invoke(ClientUI.getInstance().DeleteDocumentSharingWindow,new object[]{tabDocumentSharing});																							
									ClientUI.getInstance().arraydocumentSharing.RemoveAt(i);
									continue;

								}
								else if(control.sessionID ==	Message.sessionID)
								{
									control.MessagePool.Add(Message);
									bFound = true;
									break;
								}								
							}		
							if(!bFound)
							{
								ClientProfile cp = GetClientProfileFromList(((DocumentSharing)msg).senderProfile.ClientId);
								if(cp != null)
								{
									if( ((DocumentSharing)msg).bEnabled)
										ClientUI.getInstance().Invoke(ClientUI.getInstance().CreateDocumentSharingWindowEx,new object[]{msg});							
								}
							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2457 receivethread==>DocumentSharing ",exp,null,false);	
						
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After DocumentSharing",null,null,false);							
					}
							
						#endregion


						#region drawing clear msg
					else if(msg.GetType().Equals(typeof(DrawingClearMessage)))
					{
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before DrawingClearMessage",null,null,false);
						try
						{
#if dbgNetworkLayer
							//Trace.WriteLine("Message Name :DrawingClearMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif						
							DrawingClearMessage CMessage = (DrawingClearMessage)msg;

							if(CMessage.m_ControlType == ControlType.ApplicationSharing)
							{
								int nId = 0;///CMessage.SenderID;							
								nId = CMessage.SenderID;
							
								Client.ClientUI client = ClientUI.getInstance();
								for(int xx = 0 ; xx < client.ApplicationSharingTabPagesArray.Count ; xx++)
								{
									Crownwood.Magic.Controls.TabPage tabPage =  (Crownwood.Magic.Controls.TabPage ) client.ApplicationSharingTabPagesArray[xx];
									ApplicationSharing control = (ApplicationSharing)tabPage.Control;
									if(control.nSessionID == CMessage.sessionID || nId==-1)
									{
										if((CMessage.senderProfile.clientType == ClientType.ClientHost))									
										{
											control.whiteBoard.Clear(false,nId);
											control.Refresh();
										}
										else
										{
											control.whiteBoard.Clear(false,nId);
											control.Refresh();
										}
									
									}
								}							
							}
							else if(CMessage.m_ControlType == ControlType.DrawingBoard)
							{
							
								int nId = 0;///CMessage.SenderID;							

								if((CMessage.senderProfile.clientType == ClientType.ClientHost))
								{
									nId = CMessage.SenderID;
									//Trace.WriteLine("Drawing Board Clear :: Sender ID:::"+nId.ToString());
									ClientUI.getInstance().whiteBoard.Clear(false,nId);
									ClientUI.getInstance().whiteBoard.Refresh();
		
								}
								else
								{
									nId = CMessage.SenderID;
									//Trace.WriteLine("Drawing Board Clear :: Sender ID:::"+nId.ToString());
									ClientUI.getInstance().whiteBoard.Clear(false,nId);
									ClientUI.getInstance().whiteBoard.Refresh();

								}
							}
							else if(CMessage.m_ControlType == ControlType.Browser)
							{
								for(int	i =	0 ;	i <	ClientUI.getInstance().arrayWebControl.Count;	i++)
								{								
									Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arrayWebControl[i];
									browser browserControl = (browser)tabPage.Control;		
									if(browserControl.sessionID ==	CMessage.sessionID)
									{
										int nId = 0;
										if(CMessage.sessionID==-1)
										{
											browserControl.annotationBrowser.ClearItems(true,-1);
											browserControl.Refresh();
										}
										else if((CMessage.senderProfile.clientType == ClientType.ClientHost))
										{
											browserControl.annotationBrowser.ClearItems(false,CMessage.SenderID);
											browserControl.Refresh();
										}
										else
										{
											nId = CMessage.SenderID;
											browserControl.annotationBrowser.ClearItems(false,nId);
											browserControl.Refresh();
										}									
										break;
									}															
							
								}

							}



							else if(CMessage.m_ControlType == ControlType.BrowserImage)
							{
								for(int	i =	0 ;	i <	ClientUI.getInstance().arrayWebControl.Count;	i++)
								{								
									Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arrayWebControl[i];
									browser browserControl = (browser)tabPage.Control;		
									if(browserControl.sessionID ==	CMessage.sessionID)
									{
										int nId = 0;
										/*
										if((CMessage.senderProfile.clientType == ClientType.ClientHost))
										{
											browserControl.annotationBrowser.ClearItems(false,CMessage.SenderID);
											browserControl.Refresh();
										}
										else
										*/
										{
										
											nId = CMessage.SenderID;
											
											browserControl.whiteBoard.Clear(nId);
											browserControl.Refresh();
										}									
										break;
									}															
							
								}

							}







							else if(CMessage.m_ControlType == ControlType.DocumentSharing)
							{
#if dbgNetworkLayer
								//Trace.WriteLine("Message Name :DocumentSharing , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif																				
								for(int	i =	0 ;	i <	ClientUI.getInstance().arraydocumentSharing.Count ;	i++)
								{							
								
									Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
									documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
									if(control.sessionID ==	CMessage.sessionID)
									{
										int nId = 0;
										if((CMessage.senderProfile.clientType == ClientType.ClientHost))
										{
										
											control.annotationBrowser.ClearItems(false,CMessage.SenderID);
											control.Refresh();
										}
										else
										{
											nId = CMessage.SenderID;
											control.annotationBrowser.ClearItems(false,nId);										
											control.Refresh();
										}									
										break;
									}								
								}		

							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 2634 receivethread #region drawing clear msg;",exp,null,false);	
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After DrawingClearMessage",null,null,false);
							
					}
						#endregion
		

						#region Generic Annotation Message
					else if(msg.GetType().Equals(typeof(Common.MsgAnnotation_Generic)))
					{
						
						
						try
						{
							

							Common.MsgAnnotation_Generic _msgannotation_Generic = (Common.MsgAnnotation_Generic)msg;														
							this.HandleAnnotation_Imagebased(_msgannotation_Generic);
							
							
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2644 receivethread==>WebControl sharing",exp,null,false);	
						
						}
						
					
					}
						#endregion 


						#region Generic incoming Image Message
					else if(msg.GetType().Equals(typeof(Common.MsgIncImage_Generic)))
					{
						
						
						
						
						try
						{
							

							Common.MsgIncImage_Generic _msg = (Common.MsgIncImage_Generic)msg;														
							// Empty array comes when the box is unchecked and it meant the control 
							//is cleaned and not initialised 
							// So we dont add in varialbe as it shows that control has been initialised

							if(_msg.compressedbuffer!=null)
							{
								++flag_IsWebImageAlreadyActive;
							}
							this.Handle_Image(_msg,flag_IsWebImageAlreadyActive );
							
							
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2644 receivethread==>WebControl sharing",exp,null,false);	
						
						}
						
					
					}
						#endregion 


						#region WebControl sharing
					else if(msg.GetType().Equals(typeof(MsgWebBrowseMessage)))
					{
						try
						{

							
							MsgWebBrowseMessage Msg = (MsgWebBrowseMessage)msg;														
							if(Msg.active) // new browser is been shared by one of presenter
							{
								ClientProfile cp = GetClientProfileFromList(Msg.ClientId);
								if(cp == null)
									continue;
								bool bFound = false;
								for(int	i =	0 ;	i <	ClientUI.getInstance().arrayWebControl.Count;	i++)
								{								
									Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arrayWebControl[i];
									browser browserControl = (browser)tabPage.Control;																
								
									if(browserControl.sessionID ==	Msg.webBrowseID)
									{
										bFound = true;
										browserControl.msgPool.Add(Msg);																	
										break;
									}															
							
								}
								if(!bFound)
								{								
									ClientUI.getInstance().bSendIt = false;
									ClientUI.getInstance().CreateWebControlWindow(Msg);
									ClientUI.getInstance().bSendIt = true;
								}
							}
							else
							{
								for(int	i =	0 ;	i <	ClientUI.getInstance().arrayWebControl.Count;	i++)
								{								
									Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arrayWebControl[i];
									browser browserControl = (browser)tabPage.Control;								
									if(browserControl.sessionID ==	Msg.webBrowseID)
									{
										ClientUI.getInstance().bSendIt = false;
										try
										{
											ClientUI.getInstance().Invoke(ClientUI.getInstance().DeleteDocumentSharingWindow,new object[]{tabPage});
											browser b= (browser)tabPage.Control;
											b.CleanandClear();
										}
										catch(Exception exp)
										{
											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2644 receivethread==>WebControl sharing inner exception on deleting tab page",exp,null,false);	
										}
										try
										{
											ClientUI.getInstance().arrayWebControl.RemoveAt(i);
										}
										catch(Exception exp)
										{
											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2644 receivethread==>WebControl sharing inner exception on remove page from tab array",exp,null,false);	
										}
											
										ClientUI.getInstance().bSendIt = true;
										break;
									}																						
								}
							}
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2644 receivethread==>WebControl sharing",exp,null,false);	
						
						}
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After MsgWebBrowseMessage",null,null,false);
					}
					#endregion

						#region StatusUpdatePacket
					try
					{
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread Before StatusUpdatePacket",null,null,false);
						int u=(ushort)MessageObjectType.MsgStateUpdatePacket+(ushort)MessageObject.BroadcastMessageFlag;
						
						if(msg.MessageType==u)
						{
							StateUpdateMessage pkt=(StateUpdateMessage)msg;
							for(int j=0;j<pkt.dataArray.Count;j++)
							{
								myRecvMsgList.Add((MessageObject)pkt.dataArray[j]);
							}
						}		
						//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("receivethread After StatusUpdatePacket",null,null,false);
					}
					catch(Exception exp)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>1429 receivethread==>Status Update",exp,null,false);	
					}
		
					#endregion
				
				
					Thread.Sleep(20); // 50    06-05-2006
				
				}
				catch(System.Threading.ThreadAbortException ex)
				{	
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("networkmanager.cs line==>508 Receive Thread is terminated ",ex,null,false);
				}

				catch(Exception exp)
				{	
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("network manager.cs line==> 2379 receivethread "+exp.StackTrace,exp,null,false);
				}

			}
			//WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("networkmanager.cs line==>2687 ",new Exception("Receive thread is closed"),null,false);
		}
	
	
	

		public bool isJoinedAlready(ClientProfile prof)
		{
			int i;			
			for(i=0;i<Client.ClientUI.getInstance().arrayParticipents.Count;i++)
			{
				if((ClientProfile)Client.ClientUI.getInstance().arrayParticipents[i]==prof)
					return true;
			}
			return false;
		}
			
		public void HandleWebBrowserAnnotation(WebMeeting.Common.AnnotMsg Msg)
		{
			//bool bFound = false;
			try
			{
				for(int	i =	0 ;	i <	ClientUI.getInstance().arrayWebControl.Count;	i++)
				{								
					Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arrayWebControl[i];
					browser browserControl = (browser)tabPage.Control;								
					if(browserControl.sessionID ==	Msg.sessionID)
					{					
						browserControl.annotationBrowser.messageStore.Add(Msg);
						break;
					}															
								
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 2795 networkmanager==> HandleWebBrowserAnnotation(WebMeeting.Common.AnnotMsg Msg)",exp,null,false);	

			}
		}

		static browser browserControl;


		public void HandleAnnotation_Imagebased(Common.MsgAnnotation_Generic _msgannotation_Generic)
		{
			
			try
			{

				//Monitor.Enter(ClientUI.getInstance().arrayWebControl);
				if(ClientUI.getInstance().arrayWebControl.Count>0)
				{
					//Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arrayWebControl[0];
					//browser browserControl = (browser)tabPage.Control;
					browser browserControl = ClientUI.browserControl;
					//browser browserControl = (browser)ClientUI.getInstance().tabBody.TabPages[temp].Control;
					// double check 
					//if(tabPage.Title.ToString().Equals("Web Sharing"))
					//{
						browserControl.whiteBoard.MsgPool_Annotation.Add(_msgannotation_Generic);																	
					/*
					}
					else
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 3652 networkmanager==> HandleAnnotation_Imagebased()   ",null, "Cleaning of resources in Web share could not be done due to wrong control assignment in Web share Clean and clear Module",true);	
					}
					*/
				}
				//Monitor.Exit(ClientUI.getInstance().arrayWebControl);

					//browserControl.whiteBoard.AddMessagetoPool(_msgannotation_Generic);

			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 3652 networkmanager==> HandleAnnotation_Imagebased()   ",exp,exp.Message.ToString(),false);	

			}
		}











			public void Handle_Image(WebMeeting.Common.MsgIncImage_Generic msg_Image, int  alreadyActive)
			{
				try
				{

					//Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arrayWebControl[0];
					//browserControl = (browser)tabPage.Control;								
								
					browser browserControl=ClientUI.browserControl;

					Common.MsgIncImage_Generic _msg =(Common.MsgIncImage_Generic)msg_Image;
					// It means that the check box is unchecked from the Presenter's side 
					if(_msg.compressedbuffer==null)
					{
						ClientUI.flag_ImageMode=false;
						browserControl.axWebBrowser1.Visible=true;
						browserControl.axWebBrowser1.BringToFront();
						
					}
					// It means that the check box is checked from the Presenter's side 
					else 
					{

						try
						{
							// It means no need to initialise the control again as it has already been initialised. 
							if(alreadyActive>1)
							{
								System.Drawing.Bitmap mainImage=(System.Drawing.Bitmap)browserControl.UncompresstoImage(_msg.compressedbuffer);
								browserControl.whiteBoard._picBox.Image=(System.Drawing.Image)mainImage;
								browserControl.whiteBoard._picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
								// After setting the Image , You have to set the Graphics object for that image
								browserControl.whiteBoard.setGraphicsobject();
								browserControl.whiteBoard.BringToFront();

								ClientUI.flag_ImageMode=true;
							
							}
								//				//First time 
							else
							{
								
								if(ClientUI.getInstance().arrayWebControl.Count>0)
								{
								
									
									browserControl.InitializeWhiteboard();
									browserControl.whiteBoard.BringToFront();
									browserControl.whiteBoard.Dock = DockStyle.Fill;
									browserControl.whiteBoard.Visible=true;
								
								
									System.Drawing.Bitmap mainImage=(System.Drawing.Bitmap)browserControl.UncompresstoImage(_msg.compressedbuffer);
									browserControl.whiteBoard._picBox.Image=(System.Drawing.Image)mainImage;
									browserControl.whiteBoard._picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
									// After setting the Image , You have to set the Graphics object for that image
									browserControl.whiteBoard.setGraphicsobject();
									browserControl.whiteBoard.BringToFront();
									browserControl.whiteBoard.setModule(3);
									browserControl.axWebBrowser1.Visible=false;
								
									ClientUI.flag_ImageMode=true;
								}		
							}

						}
						catch(Exception exp)
						{
							MessageBox.Show("==>"+exp.Message.ToString());
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 2795 networkmanager==> HandleWebBrowserAnnotation(WebMeeting.Common.AnnotMsg Msg)",exp,null,true);	

						}
					}
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 3709 networkmanager==> HandleWebBrowserAnnotation(WebMeeting.Common.AnnotMsg Msg)",exp,null,true);	

				}
				
		}












		public void HandlePDFAnnotation(WebMeeting.Common.AnnotMsg msg)
		{
			try
			{
				//bool bFound = false;
				for(int	i =	0 ;	i <	ClientUI.getInstance().arraydocumentSharing.Count;	i++)
				{							
					Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
					documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
					control.hideAnnotations();
					if(control.sessionID ==	msg.sessionID)
					{
						if(msg.annotationType == AnnotationMessageType.XLS)
						{
							control.annotationBrowser.messageStore.Add(msg);
						}
						else
							control.annotationBrowser.messageStore.Add(msg);
						break;
					
					}								
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2826 networkmanager==> HandlePDFAnnotation(WebMeeting.Common.AnnotMsg msg)",exp,null,false);	

			}											
			        
		}

		public void SendMessage(MessageObject msg)
		{
			//Trace.WriteLine("sender id : " + msg.SenderID + " Description  : " + msg.ToString() );
			try
			{
				msg.SenderID=this.profile.ClientId;
				if(msg.GetType().Equals(typeof(DocumentSharing)))
				{
					((DocumentSharing)msg).ConferenceID = this.profile.ConferenceID ;
				}

				((IConferenceRoom)this._dispatcher.TransparentProxy).SendNonLoggedMessage(msg);
				//			((IConferenceRoom)this._dispatcher.TransparentProxy).SendLoggedMessage(msg); // new check logged to nonlogged
            
				//IConferenceRoom.SendLoggedMessage(msg);	
				Thread.Sleep(5);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2850 networkmanager==> HandlePDFAnnotation(WebMeeting.Common.AnnotMsg msg)",exp,null,false);				
			
			}

		}
		public void DocumentSharingStatusUpdate(MessageObject m)
		{
			try
			{
				for(int	i =	0 ;	i <	ClientUI.getInstance().arraydocumentSharing.Count ;	i++)
				{
					DocumentSharingStatusUpdate UpdateMessage = (DocumentSharingStatusUpdate )m;
					Crownwood.Magic.Controls.TabPage tabDocumentSharing	= (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
					documentSharingControl control = (documentSharingControl) tabDocumentSharing.Control;
					if(control.sessionID ==	UpdateMessage.sessionID)
					{
						control.MessagePool.Add(UpdateMessage);
								
						break;
					}								
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==>2875 networkmanager==>DocumentSharingStatusUpdate(MessageObject m) ",exp,null,false);				
			
			}
		}
		/*
		public void OnStatusUpdate(int nValue,string strValue,MessageObject m)
		{
			for(int i = 0 ; i < ClientUI.getInstance().arraydocumentSharing.Count; i++)
			{
				Crownwood.Magic.Controls.TabPage tabDocumentSharing = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arraydocumentSharing[i];
				WebMeeting.Client.documentSharingControl documentSharingControl = (WebMeeting.Client.documentSharingControl)tabDocumentSharing.Control;
				if(documentSharingControl.sessionID == ((WebMeeting.Common.DocumentSharing)m).sessionID)
				{
					documentSharingControl.OnStatusUpdate(strValue);
				}

			}
		}*/
		public bool StartDocumentSharing(ref MessageObject msg)
		{
			msg.SenderID=this.profile.ClientId;
			if(IConferenceRoom.StartDocumentSharing(ref msg) == 0)
			{
				return false;
			}
			//((IConferenceRoom)this._dispatcher.TransparentProxy).StartDocumentSharing(ref msg);
            
			//IConferenceRoom.SendLoggedMessage(msg);	
			Thread.Sleep(5);
			return true;
		}
		public bool bIsConnectedToServer = false;
		string m_sPassword="";
		bool m_bIsGuest;
		#region connect network
		public bool Connect(string ip, int port, string userName, string Password,string conferenceID,string email,bool IsGuest)
		{
			
			int starttickcount=System.Environment.TickCount;
			int endtickcount;
			m_sPassword=Password;
			m_bIsGuest=IsGuest;
			try
			{
				#region setup .NET Remoting
				if(!bCreated)
				{
					try
					{					
						IDictionary props = new Hashtable();



						//						The invocation timeout. An exception will be dispatched to
						//						the caller if the response to the message is not received
						//						within this time period specified by this value.
						props["InvocationTimeout"]=600000.ToString();

						
						//						Description of ConnectTimeout --->"An exception is dispatched to the
						//						caller if no connection to the remote host is established within
						//						this time span."

						props["ConnectTimeout"]=240000.ToString();



					
						//						An empty message (6 byte) is sent
						//						to the remote host if there are no
						//						messages sent to the remote host
						//						within this time span.

						props["PersistentConnectionSendPingAfterInactivity"]=1000.ToString();

						props["MaxQueuedItems"]= 10000.ToString(); // new change to string by documentation 
						props["MaxTotalSize"]= 200000000000.ToString();
						props["MaxTimeSpanToReconnect"]=300000.ToString();/*its 5 minutes ((300000/1000)seconds/60)minutes*/									
						props["ReconnectionTries"]=180.ToString();/*no of tries	*/		
						props["SleepBetweenReconnections"]=2000.ToString();/*in milliseconds....sleep btw connection..set by 1 second*/
						//props["MaxContentSize"]=20000000.ToString();/*in milliseconds....sleep btw connection..set by 1 second*/
						GenuineTcpChannel channel =	new GenuineTcpChannel(props, null, null);
						//GenuineXHttpChannel channel =	new GenuineXHttpChannel(props, null, null);
						

						System.Configuration.ConfigurationSettings.GetConfig("DNS");
						GenuineGlobalEventProvider.GenuineChannelsGlobalEvent += new GenuineChannelsGlobalEventHandler(GenuineChannelsEventHandler);
						ChannelServices.RegisterChannel(channel);
						WellKnownClientTypeEntry remotetype = new
							WellKnownClientTypeEntry(typeof(IMessageReceiver),
							@"gtcp://" + ip + ":" + port + "/FetchCurrentBusinessObject.rem");
						RemotingConfiguration.RegisterWellKnownClientType(remotetype);
					}
					catch(Exception ex)
					{
						//Trace.WriteLine("Connect Exception :: Message : " + ex.Message + ", Source :" + ex.Source  + ", StackTrace :" + ex.StackTrace);
					}				 
					//RemotingConfiguration.Configure("Client.exe.config");
					
					Nickname = userName;
				
					// bind client's receiver
					RemotingServices.Marshal(this, "MessageReceiver.rem");				
					bCreated = true;
					IConferenceServer = (IConferenceServer) Activator.GetObject(typeof(IConferenceRoom),
						"gtcp://" + ip + ":" + port + "/ChatServer.rem");
					//string receiverUri = "gtcp://" + ip + ":" + port + "/ChatServer.rem";

					profile.Name=userName;
					profile.EMailAddress=email;
					profile.ConferenceID=conferenceID;					
					// subscribe to the chat event
				}
				#endregion 
				lock(IConferenceServerLock)
				{
					
					AuthenticationMessage msg = null;
					#region  join Conference
					starttickcount=System.Environment.TickCount;
					IConferenceRoom =IConferenceServer.JoinConference(Int32.Parse(conferenceID),ref profile,out msg,Password,IsGuest);
					endtickcount=System.Environment.TickCount;
					Client.ClientUI.getInstance().webser.UpdateSystemAspectsandMeetingStartTime(endtickcount-starttickcount);
					if(IConferenceRoom != null)
					{
					
						bIsConnectedToServer= true;
						//string receiverUri = GenuineUtility.FetchCurrentRemoteUri() + "/MessageReceiver.rem";
						//IMessageReceiver iMessageReceiver = (IMessageReceiver) Activator.GetObject(typeof(IMessageReceiver), receiverUri);
			
						//IConferenceRoom ii= (IConferenceRoom) Activator.GetObject(typeof(IConferenceRoom), receiverUri);
						//this._dispatcher.Add((MarshalByRefObject) ii);
						#region dispatcher
							
						this._dispatcher.Add((MarshalByRefObject) IConferenceRoom);
						this._dispatcher.BroadcastReceiverHasBeenExcluded+=new BroadcastReceiverHasBeenExcludedEventHandler(_dispatcher_BroadcastReceiverHasBeenExcluded);
						this._dispatcherAudio.Add((MarshalByRefObject) IConferenceRoom);
						this._dispatchervideo.Add((MarshalByRefObject) IConferenceRoom);
						this._dispatcherDesktopSharing.Add((MarshalByRefObject) IConferenceRoom);
						#endregion
						AuthenticationMessage Msg = (AuthenticationMessage)msg;
						if(!Msg.bAuthenticated)
						{
							MeetingAlerts alert=new MeetingAlerts();
							alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Unable to authenticate",true,false);
							alert.Dispose();
							//MessageBox.Show("Unable to authenticate","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
						}
						else
						{
							Info.getInstance().FtpIP = Msg.FTP_IP;
							Info.getInstance().Password = Msg.FTPPassoword;
							Info.getInstance().UserName = Msg.FTPUsername;
							Info.getInstance().ConferenceName = Msg.ConferenceName;							
							ClientUI.getInstance().ImageURL = Info.getInstance().WebsiteName + "/members/logos/" + Msg.ImageURL;
							profile.Name = Msg.ClientName;
							profile.EMailAddress = Msg.ClientEmail;
						

							//ClientUI.getInstance().ConnectionEnabled();
						}
						retryCount=0;
						
					}
				}

				return true;
				#endregion
			}
			
			catch(Belikov.GenuineChannels.OperationException exp)
			{
				MeetingAlerts alert=new MeetingAlerts();
				SimpleMAPIdotNET.SendForm mail=new SimpleMAPIdotNET.SendForm();
				
				if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.TransportProblem"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : The remote peer closed the connection before sending the entire message (chunked, according to the GTCP settings).Please try to reestablish the connection",true,false);
				
				}
				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.Send.ServerDidNotReply"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : The remote host has not replied to the sent message for Belikov.Timout error .Please try again ...",true,false);
				
				}

				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.Send.Timeout"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : The message has not been sent within the specified timeout.This might be due to slow network connection or Server might be busy. Plz try again",true,false);
				
				}
			
				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.Send.NoSender"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : Server tried to send a message to the client that has already closed the connection and has not reconnected. In addition,This error may also be due to the client uses a server MBR object (not SAO or CAO), or the destination URL is invalid .Please try later..",true,false);
				
				}
		
				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.Send.DestinationIsUnreachable"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : There is no way to send the message to the remote host. Please ask for the technical help.",true,false);
				
				}

					
				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.Send.NoNamedConnectionFound"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : The requested named connection has not been found. Please resehedule the meeting and try again",true,false);
				
				}

				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.CanNotConnectToRemoteHost"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : The Socket.Connect method has thrown an exception. Please try to correct your DNS settings or disable any firewall or Antivirus on your System.",true,false);
				
				}
				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.Connect.CanNotResolveHostName"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : Dns.Resolve cannot resolve the specified host name. The host name is included into the error message i.e :"+exp.Message.ToString()+"Server might not be available at this time due to  maintainance purposes or too busy to reply to the specific request.",true,false);
				
				}

				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.NoPortToListen"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : The server channel cannot identify port to be listened to. That is, the port parameter has no correct string value.Either some firewall or some security settings are blocking your requests. Please disable the firewall or particular security settings.",true,false);
				
				}

				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.Connect.RemoteHostDoesNotRespond"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : The Remote host did not reply to the ping request.Server might not be available at this time due to maintainance purposes or too busy to reply to the specific request",true,false);
				
				}
				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.Connect.ConnectionReestablished"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : The remote host has opened another connection and enforced connection reestablishing. The currently established connection is shutting down. Please try after some time as server might be restarting ..",true,false);
				
				}
					
				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.Receive.ConnectionClosed"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : The connection has been forcibly closed. please try to reconnect ... ",true,false);
				
				}

					
				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.Receive.ServerHasBeenRestarted"))
				{
				 alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : The client channel has detected that the GUID of the remote host had been changed due to restart. Please reschedule the meeting and try again.",true,false);
				}
			
				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.Processing.ThreadPoolLimitExceeded"))
				{
				alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : The thread limit has been exceeded, Please try later..after some time , Sorry for your inconvenienc ",true,false);
				}
			
				else if (exp.OperationErrorMessage.ErrorIdentifier.Equals("GenuineChannels.Exception.Server.IncorrectUrl"))
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to : The invalid port number to listen to due incorrect URL. Please check the security setting of your browser or and disable any firewall on your system and then try again",true,false);
				}
								
				else
				{
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection could not be established due to :"+exp.Message.ToString()+"::"+exp.OperationErrorMessage.ErrorIdentifier.ToString()+"Please try later after some time.",true,false);
				}
				//mail.Send_text("Zaeem.Asif@Uraan.net","Webmeeting disconnection Bug from client",exp.OperationErrorMessage.ErrorIdentifier.ToString());
				string processes =Getallprocesses();
				string programs =Getallprograms();
				mail.Send_text("qa@Uraan.net","Webmeeting disconnection Bug from client",exp.OperationErrorMessage.ErrorIdentifier.ToString()+"\nHere is the processes list===>\n\n"+processes+"\n\n"+programs);
				
				return false;
				
			}

			catch(Exception e)
			{
				string processes =Getallprocesses();
				string programs =Getallprograms();
				SimpleMAPIdotNET.SendForm mail=new SimpleMAPIdotNET.SendForm();
				mail.Send_text("qa@Uraan.net","Webmeeting disconnection Bug from client",e.Message.ToString()+"\n\n"+e.StackTrace.ToString()+"\n\n"+"\nHere is the processes list===>\n\n"+processes+"\n\n"+programs);
				
				#region error joining conference
				//Console.WriteLine(e.ToString());
				if(e.GetType().Equals(typeof(ArgumentException)))
				{
					ArgumentException a = (ArgumentException)e;
					if(a.ParamName == Password)
					{
						Client.ClientUI.getInstance().PasswordError = true;
						//						ChannelServices.UnregisterChannel(channel);
						
						return false;
					}
					else if(a.ParamName == "Max" + conferenceID)
					{
						MeetingAlerts alert=new MeetingAlerts();
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Sorry, Unable to join conference. Conference is full.",true,false);
						alert.Dispose();
						//MessageBox.Show("Sorry, Unable to join conference. Conference is full.","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
						
					}
					else if(a.ParamName == "Conference")
					{
						MeetingAlerts alert=new MeetingAlerts();
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"No Such conference exists.",true,false);
						alert.Dispose();
						//MessageBox.Show("No Such conference exists.","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
					else
					{
						MeetingAlerts alert=new MeetingAlerts();
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"The connection could not be established. Please try later. Error Details:  " + a.Message,true,false);
						alert.Dispose();
						//MessageBox.Show("The connection could not be established. Please try later. Error Details:  " + a.Message,"WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}
				}
				else
				{
					if(retryCount<1)
					{
						MeetingAlerts alert=new MeetingAlerts();
						alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Fatal,"Connection Error: The connection could not be established. Please try later" ,true,false);
						alert.Dispose();
					
					}
					//MessageBox.Show("Connection Error: " + e.ToString() + ". Consult system administrator");
					bCreated = false;							
					this.retryCount++;
					//if(retryCount<2)//13Jan06
					//	thisInstance.Connect(ip,port,userName,Password,conferenceID,email,IsGuest); //13Jan06
					/*else
					{
						MeetingAlerts alert2=new MeetingAlerts();
						//alert2.ShowMessage(5,"The connection could not be established. Please try later",true,false);
						//MessageBox.Show("The connection could not be established. Please try later","WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
					}*/

				}			
				
				return false;
				#endregion
			}

			
		}

		#endregion


		public string Getallprocesses()
		{
		string str="";
		Process[] Processes;
		Processes=Process.GetProcesses();
			for(int a=0;a<Processes.Length;a++)
			{
				str+=Processes[a].ProcessName.ToString()+"\n";
			}
		return str;
		
		
		}


		public string Getallprograms()
		{
			string str="";
			
			
			ManagementClass wmiRegistry = new ManagementClass("root/default",
				"StdRegProv", null);

			//Enumerate subkeys

			string keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

			object[] methodArgs = new object[] {Win32.USER32.HKEY_LOCAL_MACHINE, keyPath, null};

			uint returnValue = (uint)wmiRegistry.InvokeMethod("EnumKey", methodArgs);

			Console.WriteLine("Executing EnumKey() returns: {0}", returnValue);

			if (null != methodArgs[2])

			{

				string[] subKeys = methodArgs[2] as String[];

				if (subKeys == null) return str;

				ManagementBaseObject inParam =
					wmiRegistry.GetMethodParameters("GetStringValue");

				inParam["hDefKey"] =Win32.USER32.HKEY_LOCAL_MACHINE;

				string keyName = "";

				foreach(string subKey in subKeys)

				{

					//Display application name

					keyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" +
						subKey;

					keyName = "DisplayName";

					inParam["sSubKeyName"] = keyPath;

					inParam["sValueName"] = keyName;

					ManagementBaseObject outParam =
						wmiRegistry.InvokeMethod("GetStringValue", inParam, null);

					if ((uint)outParam["ReturnValue"] == 0)
						//Console.WriteLine(outParam["sValue"]);
						str+=outParam["sValue"].ToString()+"\n";


				}

			} 


			return str;
		
		
		}

		public void SendAppShareMessage(byte[] data, int header)
		{
			try
			{
				if(header!=-1)
				{					
					if(data.Length>900000) // else too large a data to be sent
					{
						return;
					}
					AppSharingMessage message = new AppSharingMessage(header,data);
					message.Parameter = profile;
					myRecvMsgList.Add(message);
					SendLoadPacket(message);

					//ConferenceMessageChannel.SendMessage(message);
				}
			}
			catch(Exception e)
			{				
				WebMeeting.Error.Show(e.Message);
			}
		}

		public void SendDesktopShareMessage(MessageObject msg)
		{
			try
			{						
				/*
				if(desktopMessages.Count==16)
				{
					desktopMessages.RemoveRange(0,8);
				}			
				*/
				this.desktopMessages.Add(msg);					
				
			}
			catch(Exception e)
			{			
				MeetingAlerts alert=new MeetingAlerts();
				alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,"There is some Error occured. Please Contact support. Error Details " + e.Message,true,false);
				alert.Dispose();
				//MessageBox.Show("There is some Error occured. Please Contact support. Error Details " + e.Message,"WebMeeting",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
			}
		}
				
		#region send packet functions
		public void SendLoadPacket(MessageObject msg)
		{
			
			try
			{
				//msg.ConferenceID = Convert.ToInt32(this.profile.ConferenceID ); //conf used in transcript
				//			bool ChkControlMsg=false;
				//			 
				//			if(msg.GetType().Equals(typeof(ControlMessage)))
				//			{
				//				ControlMessage controlMessage = (ControlMessage)msg;
				//				if(controlMessage.Code == ControlCode.AttendeeUpdate)	
				//				{
				//					//ControlMessage controlMessage = (ControlMessage)msg;
				//					ChkControlMsg=true;
				//				}		
				//			}
					#region register audio
				if(msg.MessageType==(ushort)MessageObjectType.MsgRegisterAudio)
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :MsgRegisterAudio , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif				
					msg.SenderID=this.profile.ClientId;
					((IConferenceRoom)this._dispatcher.TransparentProxy).RegisterAudio(msg);
					//IConferenceRoom.RegisterAudio(msg);				
				}
					#endregion
					#region mouse events
				else if (msg.MessageType == (ushort)MessageObjectType.MsgAppSharMouseMove)
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :MsgAppSharMouseMove , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif				
					IConferenceRoom.SendNonLoggedMessage(msg);
				}
					#endregion



					//Zaeem View 
					//This close conference comment is totally wrong , i think as the message 
					// of rights update not only comes on the close conference.it comes whenever the 
					// the rights are updated.

					#region close confernce
				else if(msg.MessageType == (ushort)MessageObjectType.MsgClientControl)
				{

					msg.SenderID = this.profile.ClientId;
					ControlMessage Msg = (ControlMessage)msg;
					if(	Msg.Code == ControlCode.PresenterClosedConference)
					{

						((IConferenceRoom)this._dispatcher.TransparentProxy).CloseConference(msg);

					}
					else
					{
						if(Msg.Code==ControlCode.AttendeeUpdate)
						{
							//Trace.WriteLine("Attendee Update Message");
							((IConferenceRoom)this._dispatcher.TransparentProxy).SendLoggedMessage(msg); 
							//IConferenceRoom.SendLoggedMessage(msg);					
						}
						else
							SendMessage(msg);
					}
				}
					#endregion
					#region audiovideo msgs
				else if(msg.MessageType==(ushort)MessageObjectType.MsgSubscribeToAudio)
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :MsgSubscribeToAudio , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif				

					msg.SenderID=this.profile.ClientId;
					((IConferenceRoom)this._dispatcher.TransparentProxy).SubscribeToAudio(msg);
					//IConferenceRoom.SubscribeToAudio(msg);
				}
				else if(msg.MessageType==(ushort)MessageObjectType.MsgRegisterVideo)
				{

#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :MsgRegisterVideo , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif				

					msg.SenderID=this.profile.ClientId;
					((IConferenceRoom)this._dispatcher.TransparentProxy).RegisterVideo(msg);
					//IConferenceRoom.RegisterVideo(msg);
				}
				else if(msg.MessageType==(ushort)MessageObjectType.MsgSubscribeToVideo)
				{

#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :MsgSubscribeToVideo , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif				

					msg.SenderID=this.profile.ClientId;
					((IConferenceRoom)this._dispatcher.TransparentProxy).SubscribeToVideo(msg);
					//IConferenceRoom.SubscribeToVideo(msg);
				}
					#endregion
					#region desktop region
				else if(msg.MessageType ==(ushort)MessageObjectType.MsgDesktopControl)
				{			

#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :MsgDesktopControl , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif								
					msg.SenderID = this.profile.ClientId;
					DesktopSharingControlMessage m = (DesktopSharingControlMessage)msg;
					((IConferenceRoom)this._dispatcher.TransparentProxy).SendPrivateChatMessage(m,m.RecipientId);
				
				}
				else if(msg.MessageType == (ushort)MessageObjectType.MsgDesktopSharing)
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :MsgDesktopSharing , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif								

					msg.SenderID = this.profile.ClientId;
					((IConferenceRoom)this._dispatcherDesktopSharing.TransparentProxy).SendDesktopMessage(msg);
				}
					#endregion
					#region webcontent
				else if(msg.MessageType==(ushort)MessageObjectType.MsgRetrieveWebContent)
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :MsgRetrieveWebContent , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif								

					msg.SenderID=this.profile.ClientId;
					IConferenceRoom.GetManageContent(msg);
				}			
					#endregion
					#region app sharing
				else if(msg.MessageType == (ushort)MessageObjectType.MsgAppSharing)
				{				
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :MsgAppSharing , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif								

					AppSharingMessageEx m = (AppSharingMessageEx)msg;
					if(m.isDesktopshareing)				
					{
						if(!ClientUI.getInstance().DesktopSharingserver.bIsServer)
							((IConferenceRoom)this._dispatcher.TransparentProxy).SendPrivateChatMessage(m,m.nReciepientID);
						else
							IConferenceRoom.SendPrivateChatMessage(m,m.nReciepientID);
					}
						//((IConferenceRoom)this._dispatcher.TransparentProxy).SendPrivateChatMessage(m,m.nReciepientID);
					else
						IConferenceRoom.SendNonLoggedMessage(msg);
				}
				else if(msg.GetType().Equals(typeof(AppSharingControlMessage)))
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :AppSharingControlMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif								

					SendMessage((MessageObject)msg);	  
				}
				else if((msg.MessageType & (ushort)(MessageObjectType.MsgAppSharing)) == (ushort)MessageObjectType.MsgAppSharing)	
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :MsgAppSharing , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif	
					IConferenceRoom.SendNonLoggedMessage(msg);
				}
				else if(msg.MessageType==(ushort)MessageObjectType.MsgClose)
				{
					IConferenceRoom.SendNonLoggedMessage(msg);
				}
				else if((msg.MessageType & (ushort)(MessageObjectType.MsgAppSharingAnnotation)) == (ushort)MessageObjectType.MsgAppSharingAnnotation)	
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :MsgAppSharingAnnotation , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif
					IConferenceRoom.SendNonLoggedMessage(msg);
				}

//				else if(msg.MessageType==(ushort)MessageObjectType.MsgClose)
//				{
//					IConferenceRoom.SendNonLoggedMessage(msg);
//				}

					#endregion
					#region status update
				else if(msg.GetType().Equals(typeof(StateUpdateMessage)))
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :StateUpdateMessage , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif	
					//IConferenceRoom.SendLoggedMessage(msg); // new check logged to nonLogged
					IConferenceRoom.SendNonLoggedMessage(msg);

				}
					#endregion
					#region connection check
				else if(msg.MessageType == (ushort)MessageObjectType.MsgConnectionCheck)
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :MsgConnectionCheck , SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif	
					IConferenceRoom.SendNonLoggedMessage(msg);
				
				}
					#endregion
					#region Drawing msgs			
				else if(msg.GetType().Equals(typeof(DrawingClearMessage)))
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :DrawingClearMessage, SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif	

					SendMessage((MessageObject)msg);	
				}
					#endregion
					#region document sharing
				else if(msg.GetType().Equals(typeof(DocumentSharing)))
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :DocumentSharing, SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif	

					SendMessage((MessageObject)msg);
				}
					#endregion
					#region Pooling msgs
				else if((msg.GetType().Equals(typeof(NewPollMessage))) || (msg.GetType().Equals(typeof(PollResultsMessage)))
					|| (msg.GetType().Equals(typeof(PollAnswerMessage)))) 
				{
#if dbgNetworkLayer
					//Trace.WriteLine("Message Name :PollAnswerMessage & NewPollMessage & PollResultsMessage, SenderId "+msg.SenderID + "Message Desc: " + msg.ToString()); 
#endif	

					IConferenceRoom.SendNonLoggedMessage(msg);
				}
					#endregion 
			
					#region others
				else 
				{
					//	m_bIsSending=true;
					try
					{
						//SendLoadPacket(new byte[1000]);
						SendMessage((MessageObject)msg);									
					}
					catch(Exception exp)// ee )
					{

						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: ==>Line 3368 Network Manager public void SendLoadPacket(MessageObject msg)",exp,"",false);						
						//MessageBox.Show("Send Load Packet" + ee.Message);				
					}
				}
				#endregion
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Network Manager public void SendLoadPacket(MessageObject msg)",ex,"",false);
			}
			//	m_bIsSending=false;
			
		}

		public void SendAppShareLoadPacket(ref StateUpdateMessage _msg, int nReceiverId)
		{			
			try
			{
				if(nReceiverId != -1)				
				{
					IConferenceRoom.SendPrivateChatMessage(_msg,nReceiverId);
				}			
				else
				{
					IConferenceRoom.SendNonLoggedMessage(_msg);
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Network Manager public void SendAppShareLoadPacket(ref StateUpdateMessage _msg, int nReceiverId)",ex,"",false);
			}
		}
			
		public void SendDesktopShareLoadPacket(ref StateUpdateMessage _msg, int nReceiverId)
		{			
			try
			{
				if(nReceiverId != -1)				
				{
					IConferenceRoom.SendPrivateChatMessage(_msg,nReceiverId);
				}			
				else
				{
					//IConferenceRoom.SendNonLoggedMessage2(_msg); // add this function to server
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Network Manager public void SendDesktopShareLoadPacket(ref StateUpdateMessage _msg, int nReceiverId)",ex,"",false);
			}
		}

		public void SendViewPacket(VideoMessage msg)
		{
			try
			{			
				if(videoMessages.Count==5)
				{
					videoMessages.RemoveRange(0,3);
				}
				videoMessages.Add(msg);
				/*		
								// analyze broadcast results			
								if(currentVideoMessage==null)
									((IConferenceRoom)this._dispatchervideo.TransparentProxy).SendVideoMessage(msg);
								else
									currentVideoMessage=msg;
				*/
			
				//	m_bIsSending=true;
				//SendLoadPacket(new byte[1000]);
				//SendMessage(msg);									
			}
			catch(Exception ee )
			{
				WebMeeting.Error.Show("Send Load Packet" + ee.Message);				
			}
			//	m_bIsSending=false;
			
		}
		public void SendAudioPacket(WebMeeting.Common.AudioMessage msg)
		{
			try
			{
				((IConferenceRoom)this._dispatcherAudio.TransparentProxy).SendAudioMessage(msg);
			}
			catch(Exception ee)
			{
				WebMeeting.Error.Show("Send Load Packet " + ee.Message);
			}
		}
		
		#endregion		
		
		bool isConnectedAgain = false;
		//System.Timers.Timer tmrClock;
		public void GenuineChannelsEventHandler(object sender, GenuineEventArgs e)
		{
			try
			{
					#region show event info
				//Trace.WriteLine("GenuineChannelsEventHandler :: EventType : " + e.EventType.ToString());
				
				Mutex objMutex = new Mutex();
				objMutex.WaitOne();

				if (e.SourceException == null)
				{
					/*Console.WriteLine("\r\n\r\n---Global event: {0}\r\nRemote host: {1}", 
						e.EventType,
						e.HostInformation == null ? "<unknown>" : e.HostInformation.ToString());
					*/
					Trace.WriteLine("Global event: " + e.EventType + " , Remote host: " + e.HostInformation == null ? "<unknown>" : e.HostInformation.ToString());
				}
				else
				{
					/*Console.WriteLine("\r\n\r\n---Global event: {0}\r\nRemote host: {1}\r\nException: {2}", 
						e.EventType, 
						e.HostInformation == null ? "<unknown>" : e.HostInformation.ToString(), 
						e.SourceException);
					*/
					Trace.WriteLine("Global event: " + e.EventType + " , Remote host: " + e.HostInformation == null ? "<unknown>" : e.HostInformation.ToString()  +"  , Exception: " + e.SourceException);
				}
				#endregion
					
					#region server disconnected or re establishing
				if(e.EventType == GenuineEventType.GeneralConnectionReestablishing)
				{
					
					
					
					# region QA closing 
					// For closing the QA window on the Pc
					//on which disconnection occured.
					
					if(ClientUI.getInstance()!=null)
					{
						ClientUI.getInstance().HideQA_forms();
					}
					# endregion 


					# region Poll closing on disconnection
					// For closing the Host Window of the Poll on the Pc
					//on which disconnection occured.
				
						
					try
					{
					if(ClientUI.getInstance()!=null)
					{
						//ClientUI.getInstance().HostPollWndClose();
						ClientUI.getInstance().HostPollHide();

					}

					}
					catch(Exception exp)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 4182 NetwokManager",exp,null,false);	
					}

					
					
					// For closing the Attendee Windows of the Poll on the Pc
					//on which disconnection occured.
					

					try
					{	
						if(pControl!=null)
						{
							pControl.attendeeForm.Close();
						}
						if(ClientUI.getInstance()!=null)
						{
							ClientUI.getInstance().frm_PollingGlobalAttendee.Close();
														
						}
					}
					catch(Exception exp)
					{
WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 4205 NetwokManager",exp,null,false);						}

					# endregion 


					#region Websharing cleaning stuff and boolean initialisation 
					try
					{
						if(ClientUI.getInstance().arrayWebControl.Count>0)
						{
							//Crownwood.Magic.Controls.TabPage tabPage = (Crownwood.Magic.Controls.TabPage)ClientUI.getInstance().arrayWebControl[0];
							//browser browserControl = (browser)tabPage.Control;

							browser browserControl = ClientUI.browserControl;
							//browser browserControl = (browser)ClientUI.getInstance().tabBody.TabPages[temp].Control;
							// double check 
							//if(tabPage.Title.ToString().Equals("Web Sharing"))
							//{
								browserControl.CleanandClear();
							//}
							//else
							//{
							//	WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 4769 GC Event handler connection reestablised   ",null, "Cleaning of resources in Web share could not be done due to wrong control assignment in Web share Clean and clear Module",true);	
							//}
						}
					}
					catch(Exception exp)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Line==> 4775 GC Event handler connection reestablised  ",exp,exp.Message+ "due to wrong control assignment",true);	
					
					}
					#endregion 

					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage(" Module ::: NetworkManager " + "GenuineChannelsEventHandler :: GeneralConnectionReestablishing" ,new Exception("GenuineChannelsEventHandler :: GeneralConnectionReestablishing"),"",false);
					reconnectionnestablish=true;
					oldNetworkId=this.profile.ClientId;
					///////////////////////////////////
					isConnectedAgain = false;
		
					
					
					
					
					
					
					// waiting for one minute
					/*for(int i=0;i<4000;i++) 
					{
						Thread.Sleep(10);
						if(isConnectedAgain)
							break;
					}
					*/

					/////////////////////////////////////////////
					if(!isConnectedAgain)
					{
						ClientType ParticipantType=profile.clientType;
						
						
						Client.ClientUI.getInstance().TerminateDesktopSharing();
						
						bRetry = false;
						Info info = Info.getInstance();
						ClientUI.getInstance().ConnectionDisabled();
						if(ParticipantType==ClientType.ClientPresenter)
						{
							ClientUI.getInstance().ChangeParticipentsListControl(true);
						}
						//						if(ParticipantType==ClientType.ClientPresenter)
						//						{
						//							
						ClientUI.getInstance().listParticipents.Items.Clear();
						for(int z = 0 ; z < ClientUI.getInstance().arrayParticipents.Count ; )
						{
								
							ClientProfile clientTemp = (ClientProfile)ClientUI.getInstance().arrayParticipents[z];												
							if(this.oldNetworkId==clientTemp.ClientId)
							{
								//MessageBox.Show("==>user is deleted");
								/*remove our profile from participant list*/
								ClientUI.getInstance().arrayParticipents.RemoveAt(z);
							}
							else
							{
								ReInsertClientProfile(clientTemp,z);
								z++;
							}
							////Trace.WriteLine("ReInsertClientProfile(clientTemp,z) is going to call");
								
							//					}
						}
						ClientUI.getInstance()._statusBar.LeftMessage="Disconnected...";					
						reconnectionnestablish=true;
						//Connect(info.ServerIP,info.ConnectionPort,info.UserName, ClientUI.getInstance().Password,info.ConferenceID,info.UserEmail,ClientUI.getInstance().IsGuest);					
					}
				}				
					#endregion

					#region connection closed 
				else if(e.EventType == GenuineEventType.GeneralConnectionClosed )
				{

					try
					{
						if(ClientUI.getInstance()!=null)
						{
						ClientUI.getInstance().HideQA_forms();
						}


					}
					catch (Exception exp)
					{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage(" Module Network manager 4130 frmQA.Hide Exception",null,"",false);					
					}


					reconnectionnestablish=false;
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage(" Module ::: NetworkManager " + "GenuineChannelsEventHandler :: GeneralConnectionClosed" ,new Exception("GenuineChannelsEventHandler :: GeneralConnectionClosed"),"",false);
					Trace.WriteLine("GenuineChannelsEventHandler :: GeneralConnectionClosed" );
					if(bRetry)
					{
						//Console.WriteLine("Reestablishing connection, Connection Closed");

						bRetry = false;
						Info info = Info.getInstance();
						ClientUI.getInstance().ConnectionDisabled();
						reconnectionnestablish=false;
						//Connect(info.ServerIP,info.ConnectionPort,info.UserName, ClientUI.getInstance().Password,info.ConferenceID,info.UserEmail,ClientUI.getInstance().IsGuest);					
					}
				}
					#endregion

					#region server restart event
				else if (e.EventType == GenuineEventType.GeneralServerRestartDetected )
				{
					// server has been restarted so it does not know that we have been subscribed to
					// messages and ours nickname
					reconnectionnestablish=false;
					Trace.WriteLine("GenuineChannelsEventHandler :: GeneralServerRestartDetected" );
					lock(IConferenceServerLock)
					{

						try
						{
							ClientUI.getInstance().ConnectionEnabled();
							//reconnectionnestablish=false;
							ClientUI.getInstance().listParticipents.Items.Clear();
							ClientUI.getInstance().arrayParticipents.Clear();
							Info info = Info.getInstance();
							Connect(info.ServerIP,info.ConnectionPort,info.UserName, ClientUI.getInstance().Password,info.ConferenceID,info.UserEmail,ClientUI.getInstance().IsGuest);					
							
							

						}
						catch(Exception ex)
						{
							ex= ex;
						}

						/*
						#region join confernce
						try
						{
							IConferenceServer = (IConferenceServer) Activator.GetObject(typeof(IConferenceRoom),
								ConfigurationSettings.AppSettings["RemoteHostUri"] + "/ChatServer.rem");
							AuthenticationMessage msg = null;
							IConferenceRoom =IConferenceServer.JoinConference(Int32.Parse(Console.ReadLine()),ref profile,out msg,"hello",false);
						#region dispatcher
							this._dispatcher.Clear();
							this._dispatcherAudio.Clear();
							this._dispatcherDesktopSharing.Clear();
							this._dispatchervideo.Clear();

							this._dispatcher.Add((MarshalByRefObject) IConferenceRoom);
							this._dispatcherAudio.Add((MarshalByRefObject) IConferenceRoom);
							this._dispatchervideo.Add((MarshalByRefObject) IConferenceRoom);
							this._dispatcherDesktopSharing.Add((MarshalByRefObject) IConferenceRoom);
						#endregion
						}
						#endregion
						catch(Exception)
						{
							Info info = Info.getInstance();
							ClientUI.getInstance().ConnectionDisabled();
							reconnectionnestablish=true;
							
							//WebMeeting.Client.ClientUI.getInstance().RemoveParticipent(this.profile);

							//WebMeeting.Client.ClientUI.getInstance().listParticipents.Clear();
							//WebMeeting.Client.ClientUI.getInstance().arrayParticipents.RemoveAt();

							//WebMeeting.Client.ClientUI.getInstance().listParticipents.Clear();
							//WebMeeting.Client.ClientUI.getInstance().arrayParticipents.Clear();
							Connect(info.ServerIP,info.ConnectionPort,info.UserName, ClientUI.getInstance().Password,info.ConferenceID,info.UserEmail,ClientUI.getInstance().IsGuest);					
						}
						*/
					}
				}			
					#endregion

					#region connection established
				else if(e.EventType==GenuineEventType.GeneralConnectionEstablished)
				{					
					//Console.WriteLine("Connection Established");
					//MessageBox.Show("GenuineChannelsEventHandler :: GeneralConnectionEstablished");
					//Trace.WriteLine("GenuineChannelsEventHandler :: GeneralConnectionEstablished");
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage(" Module ::: NetworkManager " + "GenuineChannelsEventHandler :: GeneralConnectionEstablished" ,new Exception("GenuineChannelsEventHandler :: GeneralConnectionEstablished"),"",false);
					isConnectedAgain = true;
					#region if(reconnectionnestablish==true)
					if(reconnectionnestablish==true)
					{
						try
						{
							ClientUI.getInstance().ConnectionEnabled();
						
							m_DelegateThreadFinished = new DelegateThreadFinished(ClientUI.getInstance().CloseAllExtraPages);
						
							ClientUI.getInstance().tabBody.BeginInvoke(m_DelegateThreadFinished); 
							m_DelegateThreadFinished =null;
						
							reconnectionnestablish=true;
						
							AuthenticationMessage msg = null;
							IConferenceRoom =IConferenceServer.JoinConference(Int32.Parse(profile.ConferenceID),ref profile,out msg,m_sPassword,m_bIsGuest);
							Client.ClientUI.getInstance().webser.UpdateDisconnectionStuff();
							this.NewNetworkId=this.profile.ClientId;
							bool isExist=false;
							for(int z = 0 ; z < ClientUI.getInstance().arrayParticipents.Count ;z++ )
							{
								
								ClientProfile clientTemp = (ClientProfile)ClientUI.getInstance().arrayParticipents[z];												
								if(this.profile.ClientId==clientTemp.ClientId)
								{
									isExist=true;
									break;
								}																
							}
							if(isExist==false)
							{
								this.ReInsertClientProfile(this.profile,ClientUI.getInstance().listParticipents.Items.Count);
							}						
							//MessageBox.Show("Old Network Id :::" + this.oldNetworkId.ToString() + " New Network Id :::" + this.NewNetworkId);	

							//						ClientUI.getInstance().listParticipents.Items.Clear();
							//						ClientUI.getInstance().arrayParticipents.Clear();
						
							reConnEstablish_IsCleared = true;
							/*
							// connect with that specific conference again
							AuthenticationMessage msg = null;
							IConferenceRoom =IConferenceServer.JoinConference(Int32.Parse(profile.ConferenceID),ref profile,out msg,m_sPassword,m_bIsGuest);
							if(msg==null)
							{
								int a=0;
								a++;
							}
							else
							{
								int a=0;
								a++;
							}
							*/
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("network manager.cs==> GenuinechannelsEventhandler line==> 3590 receivethread ==>if(reconnectionnestablish==true)",exp,null,false);
						}
					}
					#endregion
					
				}
					#endregion

					#region Listener shutdown
				else if(e.EventType==GenuineEventType.GeneralListenerShutDown)
				{
					//Console.WriteLine("Reestablishing connection, Connection Closed");
					//Trace.WriteLine("GenuineChannelsEventHandler :: GeneralListenerShutDown" );
					bRetry = false;
					Info info = Info.getInstance();
					ClientUI.getInstance().ConnectionDisabled();
					reconnectionnestablish=true;
					//Connect(info.ServerIP,info.ConnectionPort,info.UserName, ClientUI.getInstance().Password,info.ConferenceID,info.UserEmail,ClientUI.getInstance().IsGuest);
				}
				objMutex.ReleaseMutex();
				#endregion
			}
			catch(Exception exp	)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("network manager.cs==> GenuinechannelsEventhandler line==> 3613 ",exp,null,false);				
			}
		}

		public void GetUniqueSessionID(ref int sessionID)
		{
			IConferenceRoom.GetUniqueSessionID(ref sessionID);
		}

		#region receieve msgs functions
		public void ReceiveAudioMessage(MessageObject msg)
		{
			try
			{
				// link with the existing code
				AudioMessage vMessage = (AudioMessage)msg;
				Client.ClientUI.getInstance().ReceiveAudioMessagFromNetwork(vMessage);
			}
			catch(Exception exp	)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("network manager.cs==>line==> 3749==> ReceiveAudioMessage( ",exp,null,false);				
			}
			

		}
		public void ReceiveVideoMessage(MessageObject msg)
		{
			// link with the existing code
			VideoMessage vMessage = (VideoMessage)msg;
			/*				
			for(int i = 0 ; i < ClientUI.getInstance().tabControl1.TabPages.Count; i++)
			{								
				Crownwood.Magic.Controls.TabPage tabPage = ClientUI.getInstance().tabControl1.TabPages[i];
				if(tabPage.GetType().Equals(typeof(videoTabPage)))
				{
					videoTabPage vTab = (videoTabPage) tabPage;
					if(vTab.sessionID == vMessage.videoID)
					{
										
						ClientUI.getInstance().showFunction(vMessage,vTab);
					}
				}
			}
			*/
		}
		
		public void ReceiveDesktopMessage(MessageObject m)
		{/*
			if(m.SenderID!=this.profile.ClientId)
			{
				ClientUI.getInstance().DesktopSharingserver.RecieveMessageFromNetwork((AppSharingMessageEx)m);
			}*/
		}		
		#endregion
		#region delegates
		public void BroadcastCallFinishedHandler(Dispatcher dispatcher, IMessage
			message, ResultCollector resultCollector)
		{
			// analyze broadcast results
		}
		public void BroadcastCallFinishedHandler1(Dispatcher dispatcher, IMessage
			message, ResultCollector resultCollector)
		{
			// analyze broadcast results
		}
		public void BroadcastCallFinishedHandlerVideo(Dispatcher dispatcher, IMessage
			message, ResultCollector resultCollector)
		{			
			videoMessageSent=true;
		}
		
		public void BroadcastCallFinishedHandlerDesktop(Dispatcher dispatcher, IMessage
			message, ResultCollector resultCollector)
		{
			// analyze broadcast results
			desktopMessageSent=true;
		}

		#endregion
		#region send thread function
		private void SendVideoThreadFunction()
		{
			
			while(this.m_bIsActive)
			{
				Thread.Sleep(100);
				try
				{
					if(videoMessageSent==true && this.videoMessages.Count>0)
					{
						VideoMessage m=(VideoMessage)this.videoMessages[0];
						this.videoMessages.RemoveAt(0);
						videoMessageSent=false;
						((IConferenceRoom)this._dispatchervideo.TransparentProxy).SendVideoMessage(m);
					
					}
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("network manager.cs==>line==> 3829==> SendVideoThreadFunction() ",exp,null,false);								 
				}
			}
		}
		private void SendDesktopThreadFunction()
		{
			while(this.m_bIsActive)
			{
				Thread.Sleep(50);
				if(desktopMessageSent==true && this.desktopMessages.Count>0)
				{
					MessageObject m = (MessageObject)this.desktopMessages[0];
					this.desktopMessages.RemoveAt(0);
					desktopMessageSent=false;
					((IConferenceRoom)this._dispatcherDesktopSharing.TransparentProxy).SendDesktopMessage(m);					
				}
			}
		}

		#endregion

		private void _dispatcher_BroadcastReceiverHasBeenExcluded(MarshalByRefObject marshalByRefObject, ReceiverInfo receiverInfo)
		{
			
		}
	}
}
