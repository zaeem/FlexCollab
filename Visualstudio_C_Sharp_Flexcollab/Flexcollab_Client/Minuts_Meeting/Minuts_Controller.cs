	using System;
	using System.IO;
	using System.Windows.Forms;
	using WebMeeting.Common;
	using WebMeeting.Client;

	namespace WebMeeting.Client.Minuts_Meeting
	{
		/// <summary>
		/// Summary description for Minuts_Controller.
		/// This class was created by Zaeem in order to add the minuts of minute (MM)
		///  
		/// </summary>
		/// 
		
		public class Minuts_Controller
		{

			
			#region variable delclareation
			private string path_Appshare;
			private string path_Whiteboard ;
			private string path_WebShare ;
			


			private string path_Whiteboardfile;
			private string path_Appsharefile;
			private string path_Websharefile;


			
			public WebMeeting.Common.Mes_MinutsofMeeting msg_Minuts_Whiteboard;
			public WebMeeting.Common.Mes_MinutsofMeeting msg_Minuts_Appshare;
			public WebMeeting.Common.Mes_MinutsofMeeting msg_Minuts_Webshare;

			public FileInfo file_Whiteboard;
			public FileInfo file_Appshare;
			public FileInfo file_Webshare;
			# endregion


			# region constructor
			public Minuts_Controller()
			{
				
//				path_Appshare=Application.StartupPath +"\\Minuts\\"+NetworkManager.getInstance().profile.ConferenceID+"\\Appshare\\" ;
//				path_Whiteboard=Application.StartupPath +"\\Minuts\\"+NetworkManager.getInstance().profile.ConferenceID+"\\Whiteboard\\" ;
//				path_WebShare=Application.StartupPath +"\\Minuts\\"+NetworkManager.getInstance().profile.ConferenceID+"\\WebShare\\" ;

			
				msg_Minuts_Whiteboard=new Mes_MinutsofMeeting();
				msg_Minuts_Appshare=new Mes_MinutsofMeeting();
				msg_Minuts_Webshare=new Mes_MinutsofMeeting();
				//
				// TODO: Add constructor logic here
				//
			}

			#endregion


			# region Host called block from the network Manager 
			/// <summary>
			/// This Method will only be called by the Host
			/// </summary>
			/// <param name="incoming_MMmsg">  this message comes from the Network and of type Minuts of meeting</param>
			public void _Snapshot(MessageObject incoming_MMmsg)
			{
				try
				{
					Common.Mes_MinutsofMeeting mm_msg;

					// Will be filled on the Host side 
					//msg_Minuts.Available=false;
					//msg_Minuts.Host=HostName();
					//msg_Minuts.Mis="";
					//msg_Minuts.Available
					// This filed is used for Text only
					//msg_Minuts.Text
		
					mm_msg =(Common.Mes_MinutsofMeeting)incoming_MMmsg;
					mm_msg.Available=0;
					mm_msg.Host=ClientUI.getInstance().HostName();
				
					if(	mm_msg.Module=="Whiteboard")
					{
						Whiteboard_Snapshot(mm_msg);
					}
					else if(	mm_msg.Module=="Appshare")
					{
						Appshare_Snapshot(mm_msg);
					}
					else if(	mm_msg.Module=="Webshare")
					{
						WebBrowser_Snapshot(mm_msg);
					}

				}
				catch(Exception exp)
				{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Minuts_controller.cs line==> 102",exp,null,false);			
				}
			}




			//                                Methods , which are to be called by Host Only 
			//////////////////////////////////////////////////////////////////////////////////////////////////////////////
			///*******************************************************************************************************////
			/////////////////////////////////////////////////////////////////////////////////////////////////////////////
			
			

			///
			/// <summary>
			/// This Method will only be called by the Host
			/// </summary>
			/// <param name="incoming_MMmsg">  this message comes from the Network and of type Minuts of meeting</param>
			# region Whiteboard_Snapshot
			public void Whiteboard_Snapshot(Mes_MinutsofMeeting incoming_MMmsg)
			{
				
				Mes_MinutsofMeeting mm_msg=(Common.Mes_MinutsofMeeting)incoming_MMmsg;
				path_Whiteboardfile=Application.StartupPath +"\\Minuts\\"+mm_msg.ImagePath;		
				file_Whiteboard=new FileInfo(path_Whiteboardfile);
                file_Whiteboard.Directory.Create();
				


				int var_temp_appshare=0;
			
				for(int i=0;i<ClientUI.getInstance().tabBody.TabPages.Count;i++)
				{
					
					//MessageBox.Show(tabBody.TabPages[i].Title.ToString());
					if(ClientUI.getInstance().tabBody.TabPages[i].Title.ToString().Equals("WhiteBoard"))
					{
						++var_temp_appshare;
						ClientUI.getInstance().tabBody.TabPages[i].Selected=true;
						break;
					}
				
				}

				//if(var_temp_appshare<=this.var_no_appshare)
				if(var_temp_appshare>0)
				{
					
					if(ClientUI.getInstance().tabBody.SelectedTab.Title.ToString().Equals("WhiteBoard"))
					{
						ClientUI.getInstance().whiteBoard.mainImage.Save(path_Whiteboardfile+".jpg",System.Drawing.Imaging.ImageFormat.Jpeg);
						ClientUI.getInstance().Save_FileInfo_TobeUpload(mm_msg.meetingID,mm_msg.Host,mm_msg.Presenter,mm_msg.Module,path_Whiteboardfile,mm_msg.Current_DateTime,mm_msg.Mis,mm_msg.ImagePath+".jpg");
						

					}
					else
						ClientUI.getInstance().ShowExceptionMessage("Sorry! The ordered tab is not Selected on the Host side");
				
				
			
				}
				else
				{
					ClientUI.getInstance().ShowExceptionMessage("Sorry! No instance of appsharing is currently running.");
				}

				
			}

			#endregion 

			
			/// <summary>
			/// This Method will only be called by the Host
			/// </summary>
			/// <param name="incoming_MMmsg">  this message comes from the Network and of type Minuts of meeting</param>
			#region   Appshare_Snapshot
			public void Appshare_Snapshot(Mes_MinutsofMeeting incoming_MMmsg)
			{
				
				
				Mes_MinutsofMeeting mm_msg=(Common.Mes_MinutsofMeeting)incoming_MMmsg;
				path_Appsharefile=Application.StartupPath +"\\Minuts\\"+mm_msg.ImagePath;		
				file_Appshare=new FileInfo(path_Appsharefile);
				file_Appshare.Directory.Create();
			
				int var_temp_appshare=0;
			
				for(int i=0;i<ClientUI.getInstance().tabBody.TabPages.Count;i++)
				{
					
					//MessageBox.Show(tabBody.TabPages[i].Title.ToString());
					if(ClientUI.getInstance().tabBody.TabPages[i].Title.ToString().Equals("Application Sharing"))
					{
						++var_temp_appshare;
						ClientUI.getInstance().tabBody.TabPages[i].Selected=true;
						break;
					}
				
				}

				//if(var_temp_appshare<=this.var_no_appshare)
				if(var_temp_appshare>0)
				{
				
					if(ClientUI.getInstance().tabBody.SelectedTab.Title.ToString().Equals("Application Sharing"))
					{
						ClientUI.getInstance().TakeSnapShot_MM(path_Appsharefile);
						ClientUI.getInstance().Save_FileInfo_TobeUpload(mm_msg.meetingID,mm_msg.Host,mm_msg.Presenter,mm_msg.Module,path_Appsharefile,mm_msg.Current_DateTime,mm_msg.Mis,mm_msg.ImagePath+".jpg");
						

					}
					else
						ClientUI.getInstance().ShowExceptionMessage("Sorry! The ordered tab is not Selected on the Host side");
				
			
				}
				else
				{
					ClientUI.getInstance().ShowExceptionMessage("Sorry! No instance of appsharing is currently running.");
				}

				
			}

			# endregion
			/// <summary>
			/// This Method will only be called by the Host for WebBrowser
			/// </summary>
			/// <param name="incoming_MMmsg">  this message comes from the Network and of type Minuts of meeting</param>
			# region WebBrowser_Snapshot

			public void WebBrowser_Snapshot(Mes_MinutsofMeeting  incoming_MMmsg)
			{
				
				Mes_MinutsofMeeting mm_msg=(Common.Mes_MinutsofMeeting)incoming_MMmsg;
				path_Websharefile=Application.StartupPath +"\\Minuts\\"+mm_msg.ImagePath;		
				file_Webshare=new FileInfo(path_Websharefile);
				file_Webshare.Directory.Create();
			
				int var_temp_appshare=0;
			
				for(int i=0;i<ClientUI.getInstance().tabBody.TabPages.Count;i++)
				{
					
					//MessageBox.Show(tabBody.TabPages[i].Title.ToString());
					if(ClientUI.getInstance().tabBody.TabPages[i].Title.ToString().Equals("Web Sharing"))
					{
						++var_temp_appshare;
						ClientUI.getInstance().tabBody.TabPages[i].Selected=true;
						break;
					}
				
				}

				//if(var_temp_appshare<=this.var_no_appshare)
				if(var_temp_appshare>0)
				{
					if(ClientUI.getInstance().tabBody.SelectedTab.Title.ToString().Equals("Web Sharing"))
					{
					
					/// If the image wa ssuccessfully taken then thats information can be saved other wise, Itr can't be
					if(ClientUI.getInstance().TakeSnapShot_MM_WebBrowser(path_Websharefile))
					ClientUI.getInstance().Save_FileInfo_TobeUpload(mm_msg.meetingID,mm_msg.Host,mm_msg.Presenter,mm_msg.Module,path_Websharefile,mm_msg.Current_DateTime,mm_msg.Mis,mm_msg.ImagePath+".jpg");
					else 
					ClientUI.Flexalert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,"Presenter requested a Sanpshot , which can not be taken.",true,false);

					}
				
				}
				else
				{
					ClientUI.getInstance().ShowExceptionMessage("Sorry! No instance of appsharing is currently running.");
				}

				
			}

		
			#endregion


			#endregion 


			# region Presenter called block 

			#region  Send_MinutsofMeetingMsd_Application()
			/// <summary>
			/// This Method should only be called by the presenter
			/// This method make and send the Minuts of minute message to the Host 
			/// </summary>
			
			public void Send_MinutsofMeetingMsg_Application()
			{
				string path_Appshare=NetworkManager.getInstance().profile.ConferenceID+"\\Appshare";
				
				// Will be filled on the Host side 
				//msg_Minuts.Available=false;
				//msg_Minuts.Host=HostName();
				//msg_Minuts.Mis="";
				//msg_Minuts.Available
				// This filed is used for Text only
				//msg_Minuts.Text
						
				msg_Minuts_Appshare.Current_DateTime=DateTime.Now.ToString();
				msg_Minuts_Appshare.ImagePath=path_Appshare+"\\"+NetworkManager.getInstance().profile.Name+"_"+DateTime.Now.ToFileTime().ToString();
				msg_Minuts_Appshare.meetingID=Convert.ToInt32(NetworkManager.getInstance().profile.ConferenceID);
				msg_Minuts_Appshare.MessageType=(ushort) ((int)MessageObjectType.Mes_MinutsofMeeting);
				msg_Minuts_Appshare.Module="Appshare";
				// It Should have been assigned a value on the Host side as currently there is no way for OTHERS to get 
				// if the Host is the presenter 
				// BUT* we assumed that this method will only be called by the presenter 
				// So the current profile is the presenter.
				msg_Minuts_Appshare.Presenter=NetworkManager.getInstance().profile.Name;
				msg_Minuts_Appshare.SenderID=NetworkManager.getInstance().profile.ClientId;
				msg_Minuts_Appshare.senderProfile=NetworkManager.getInstance().profile;







				int var_temp_appshare=0;
			
				for(int i=0;i<ClientUI.getInstance().tabBody.TabPages.Count;i++)
				{
					
					//MessageBox.Show(tabBody.TabPages[i].Title.ToString());
					if(ClientUI.getInstance().tabBody.TabPages[i].Title.ToString().Equals("Application Sharing"))
					{
						++var_temp_appshare;
						ClientUI.getInstance().tabBody.TabPages[i].Selected=true;
						break;
					}
				
				}

				//if(var_temp_appshare<=this.var_no_appshare)
				if(var_temp_appshare>0)
				{
				
					if(ClientUI.getInstance().tabBody.SelectedTab.Title.ToString().Equals("Application Sharing"))
					{
					
						// Host checks if he is the Presenter 
						if(NetworkManager.getInstance().profile.clientType==ClientType.ClientHost)
						{
							if(ClientUI.getInstance().IfIamthePresenter())
							{
								Appshare_Snapshot(msg_Minuts_Appshare);
							}
						}
						else
						{
							NetworkManager.getInstance().SendLoadPacket(msg_Minuts_Appshare);
						}
						
					}
					else
						ClientUI.getInstance().ShowExceptionMessage("Sorry! The ordered tab is not Selected on the Presenter's side");
				
			
				}
				else
				{
					ClientUI.getInstance().ShowExceptionMessage("Sorry! No instance of Appsharing is currently running.");
				}
			}
			
			#endregion

			#region  Send_MinutsofMeetingMsd_Whiteboard()
			/// <summary>
			/// This Method should only be called by the presenter
			/// This method make and send the Minuts of minute message to the Host 
			/// </summary>
			
			public void Send_MinutsofMeetingMsg_Whiteboard()
			{
				string path_Whiteboard=NetworkManager.getInstance().profile.ConferenceID+"\\Whiteboard";
				

				// Will be filled on the Host side 
				//msg_Minuts.Available=false;
				//msg_Minuts.Host=HostName();
				//msg_Minuts.Mis="";
				//msg_Minuts.Available
				// This filed is used for Text only
				//msg_Minuts.Text
				
				msg_Minuts_Whiteboard.Current_DateTime=DateTime.Now.ToFileTime().ToString();
				msg_Minuts_Whiteboard.ImagePath=path_Whiteboard+"\\"+NetworkManager.getInstance().profile.Name+"_"+DateTime.Now.ToFileTime().ToString();
				msg_Minuts_Whiteboard.meetingID=Convert.ToInt32(NetworkManager.getInstance().profile.ConferenceID);
				msg_Minuts_Whiteboard.MessageType=(ushort) ((int)MessageObjectType.Mes_MinutsofMeeting);
				msg_Minuts_Whiteboard.Module="Whiteboard";
				// It Should have been assigned a value on the Host side as currently there is no way for OTHERS to get 
				// if the Host is the presenter 
				// BUT* we assumed that this method will only be called by the presenter 
				// So the current profile is the presenter.
				msg_Minuts_Whiteboard.Presenter=NetworkManager.getInstance().profile.Name;
				msg_Minuts_Whiteboard.SenderID=NetworkManager.getInstance().profile.ClientId;
				msg_Minuts_Whiteboard.senderProfile=NetworkManager.getInstance().profile;

				
				int var_temp_appshare=0;
			
				for(int i=0;i<ClientUI.getInstance().tabBody.TabPages.Count;i++)
				{
					
					//MessageBox.Show(tabBody.TabPages[i].Title.ToString());
					if(ClientUI.getInstance().tabBody.TabPages[i].Title.ToString().Equals("WhiteBoard"))
					{
						++var_temp_appshare;
						ClientUI.getInstance().tabBody.TabPages[i].Selected=true;
						break;
					}
				
				}

				//if(var_temp_appshare<=this.var_no_appshare)
				if(var_temp_appshare>0)
				{
					
					if(ClientUI.getInstance().tabBody.SelectedTab.Title.ToString().Equals("WhiteBoard"))
					{	
				
						// Host checks if he is the Presenter 
						if(NetworkManager.getInstance().profile.clientType==ClientType.ClientHost)
						{
							if(ClientUI.getInstance().IfIamthePresenter())
							{
								this.Whiteboard_Snapshot(msg_Minuts_Whiteboard);
							}
						}
						else
						{
							NetworkManager.getInstance().SendLoadPacket(msg_Minuts_Whiteboard);
						}
			

					}
					else
						ClientUI.getInstance().ShowExceptionMessage("Sorry! The ordered tab is not Selected on the Presenter's side");
				
				
			
				}
				else
				{
					ClientUI.getInstance().ShowExceptionMessage("Sorry! No instance of Whiteboard is currently running.");
				}

				
				
				
				
				
				
				
				
				
			}
			#endregion

			#region  Send_MinutsofMeetingMsd_WebBrowser()
			/// <summary>
			/// This Method should only be called by the presenter
			/// This method make and send the Minuts of minute message to the Host 
			/// </summary>
			
			public void Send_MinutsofMeetingMsg_WebBrowser()
			{
				path_WebShare=NetworkManager.getInstance().profile.ConferenceID+"\\WebBrowser";
				
				// Will be filled on the Host side 
				//msg_Minuts.Available=false;
				//msg_Minuts.Host=HostName();
				//msg_Minuts.Mis="";
				//msg_Minuts.Available
				// This filed is used for Text only
				//msg_Minuts.Text
						
				msg_Minuts_Webshare.Current_DateTime=DateTime.Now.ToString();
				msg_Minuts_Webshare.ImagePath=path_WebShare+"\\"+NetworkManager.getInstance().profile.Name+"_"+DateTime.Now.ToFileTime().ToString();
				msg_Minuts_Webshare.meetingID=Convert.ToInt32(NetworkManager.getInstance().profile.ConferenceID);
				msg_Minuts_Webshare.MessageType=(ushort) ((int)MessageObjectType.Mes_MinutsofMeeting);
				msg_Minuts_Webshare.Module="Webshare";
				// It Should have been assigned a value on the Host side as currently there is no way for OTHERS to get 
				// if the Host is the presenter 
				// BUT* we assumed that this method will only be called by the presenter 
				// So the current profile is the presenter.
				msg_Minuts_Webshare.Presenter=NetworkManager.getInstance().profile.Name;
				msg_Minuts_Webshare.SenderID=NetworkManager.getInstance().profile.ClientId;
				msg_Minuts_Webshare.senderProfile=NetworkManager.getInstance().profile;

				
				
				
				
				int var_temp_appshare=0;
			
				for(int i=0;i<ClientUI.getInstance().tabBody.TabPages.Count;i++)
				{
					
					//MessageBox.Show(tabBody.TabPages[i].Title.ToString());
					if(ClientUI.getInstance().tabBody.TabPages[i].Title.ToString().Equals("Web Sharing"))
					{
						++var_temp_appshare;
						ClientUI.getInstance().tabBody.TabPages[i].Selected=true;
						break;
					}
				
				}

				//if(var_temp_appshare<=this.var_no_appshare)
				if(var_temp_appshare>0)
				{
					
					if(ClientUI.getInstance().tabBody.SelectedTab.Title.ToString().Equals("Web Sharing"))
					{	
				
				
						// Host checks if he is the Presenter 
						if(NetworkManager.getInstance().profile.clientType==ClientType.ClientHost)
						{
							if(ClientUI.getInstance().IfIamthePresenter())
							{
								this.WebBrowser_Snapshot(msg_Minuts_Webshare);
							}
						}
						else
						{
							NetworkManager.getInstance().SendLoadPacket(msg_Minuts_Webshare);
						}

					}
					else
						ClientUI.getInstance().ShowExceptionMessage("Sorry! The ordered tab is not Selected on the Presenter's side");
				
				
			
				}
				else
				{
					ClientUI.getInstance().ShowExceptionMessage("Sorry! No instance of WebBrowser is currently running.");
				}



				
			}
			#endregion


			#endregion 


		}
	}
