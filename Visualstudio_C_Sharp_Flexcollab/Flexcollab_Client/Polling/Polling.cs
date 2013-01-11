using System;
using System.Collections;
using System.Windows.Forms;
using WebMeeting.Common;
using WebMeeting.Client;
using System.Threading;
using System.Xml;
using System.Diagnostics;


namespace WebMeeting.Polling
{

	
					/// <summary>
					/// Summary description for Polling.
					/// Code improvment done by Zaeem.
					/// </summary>
					/// 
					
	public delegate void Del_Attendeeform();

	public class Polling
	{		
		public PollAttendeeForm attendeeForm;
		public ArrayList AnswersList;
		public ArrayList choices;
		public string question;
		public int sessionID;
		public bool enabled;
		public bool locked;
		public bool anonymous;
		public PollResult pr;
		public PollingQuestionDetails pqd;
		public int ClientID;
		public string ClientName;
		public Utility.NiceMenu.NiceMenu myMenu;
		public PollType thisPollType;
		public string saveFileName;
		public PollType testtype;
		public ArrayList ArrayAttendeeForm=new ArrayList();
		
		

		#region Polling default Constructor
		public Polling()
		{

			try
			{
				//
				// TODO: Add constructor logic here
				//			
				
				this.choices=new ArrayList();
				this.AnswersList=new ArrayList();
				AddClients(new ArrayList());
			
				sessionID = -1;
				NetworkManager.getInstance().GetUniqueSessionID(ref sessionID);
				//thr=new Thread(new ThreadStart(test));	
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @contructor line==> 58",exp,null,false);			
			}
			//			get unique session id here
		}

		#endregion

		#region AddSaveMenu()
		public void AddSaveMenu()
		{
			try
			{
				Utility.NiceMenu.NiceMenu parentMenu = Client.ClientUI.getInstance().myNiceMenu.SearchNiceMenuItem("Poll");
				if(parentMenu != null)
				{
					myMenu = new Utility.NiceMenu.NiceMenu();				
					myMenu.Click += new EventHandler(saveMenu_Click);
					if(question.Length >15)
						myMenu.Text = question.Substring(0,15) + "...";
					else
						myMenu.Text = question;
					parentMenu.MenuItems.Add(myMenu);
				}
			
				//add menu to save as
				parentMenu = Client.ClientUI.getInstance().myNiceMenu.SearchNiceMenuItem("Poll",2);
				if(parentMenu != null)
				{
					myMenu = new Utility.NiceMenu.NiceMenu();				
					myMenu.Click += new EventHandler(saveAsMenu_Click);
					//myMenu.Click += new EventHandler(mNewMenuDocumentsSaveAs_Click);							 
					if(question.Length >15)
						myMenu.Text = question.Substring(0,15) + "...";
					else
						myMenu.Text = question;
			
					parentMenu.MenuItems.Add(myMenu);
				}
								
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @AddSaveMenu() line==> 100",exp,null,false);
			}
		}
		#endregion 

		#region AddClients(ArrayList clientlist)
		public void AddClients(ArrayList clientlist)
		{
			try
			{
				//if((enabled) && (locked == false))
			{
				for(int i=0;i<clientlist.Count;i++)
				{				
					ClientProfile profile = (ClientProfile) clientlist[i];
					if(profile.ClientId == NetworkManager.getInstance().profile.ClientId)
						continue;
					PollingAnswer pa=new PollingAnswer();
					//				// set the client parameters here
					pa.clientId = profile.ClientId;
					pa.clientName = profile.Name;				
					
					this.AnswersList.Add(pa);
				}
			}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @AddClients(ArrayList clientlist) line==> 128",exp,null,false);
			}
		}
	
		#endregion

		#region LunchFromMangeGetQuestion()
		
		public void LunchFromMangeGetQuestion()
		{					
			try
			{
				this.question=pqd.GetQuestion();	
				NewPollMessage msg=new NewPollMessage(pqd.IsEvaluation);
				msg.sessionID=this.sessionID;
				msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
			
				anonymous = pqd.checkAnonymous.Checked;
				msg.Question=this.question;
				msg.anonymousPoll =  pqd.checkAnonymous.Checked;
				msg.hostID = NetworkManager.getInstance().profile.ClientId;
				switch(pqd.comboType.SelectedIndex)
				{
					case 0:
						msg.pollType = PollType.FreeResponse;
						break;
					case 1:
						msg.pollType = PollType.SingleSelect;
						break;
					case 2:
						msg.pollType = PollType.MultipleSelect;
						break;
					case 3:
						msg.pollType = PollType.TrueFalse;
						break;
					case 4:
						msg.pollType = PollType.YesNo;
						break;
				
				}
				thisPollType= msg.pollType;			
				choices=pqd.GetChoices(msg.pollType);
				msg.choices=this.choices;

				NetworkManager.getInstance().SendLoadPacket(msg);	
		
				//			msg.hostID=this.host;
				//network mannager . send msg;
			}
			catch (Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @LunchFromMangeGetQuestion() line==> 179",exp,null,false);
			}
		}


		#endregion

		#region GetQuestion()
		public void GetQuestion()
		{					
			try
			{
				this.question=pqd.GetQuestion();	
				NewPollMessage msg=new NewPollMessage(pqd.IsEvaluation);
				msg.sessionID=this.sessionID;
				msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
						
				anonymous = pqd.checkAnonymous.Checked;
				msg.Question=this.question;
				msg.anonymousPoll =  pqd.checkAnonymous.Checked;
				msg.hostID = NetworkManager.getInstance().profile.ClientId;
				switch(pqd.comboType.SelectedIndex)
				{
					case 0:
						msg.pollType = PollType.FreeResponse;
						break;
					case 1:
						msg.pollType = PollType.SingleSelect;
						break;
					case 2:
						msg.pollType = PollType.MultipleSelect;
						break;
					case 3:
						msg.pollType = PollType.TrueFalse;
						break;
					case 4:
						msg.pollType = PollType.YesNo;
						break;
				
				}
				thisPollType= msg.pollType;			
				choices=pqd.GetChoices(msg.pollType);
				msg.choices=this.choices;

				NetworkManager.getInstance().SendLoadPacket(msg);	
		
				//			msg.hostID=this.host;
				//network mannager . send msg;
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @GetQuestion() line==> 230",exp,null,false);
			}
		}
		# endregion 

		# region SendWebPollMessage(PollType type,bool IsEvaluation)
		public void SendWebPollMessage(PollType type,bool IsEvaluation)
		{
			try
			{
				thisPollType= type;
				NewPollMessage msg = new NewPollMessage(false);
				msg.sessionID = this.sessionID;
				msg.choices = this.choices;
				msg.Question = this.question;
				msg.anonymousPoll = false;			
				msg.hostID = NetworkManager.getInstance().profile.ClientId;
				msg.pollType = type;
				msg.IsEvaluation = IsEvaluation;
			
				msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID;
				NetworkManager.getInstance().SendLoadPacket(msg);
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @SendWebPollMessage(PollType type,bool IsEvaluation) line==> 255",exp,null,false);
			
			}

		}
		# endregion 

		# region IsChoiceExistsInMulitpleChoicies(string multipleChoiceString,string choiceToFind)
		public bool IsChoiceExistsInMulitpleChoicies(string multipleChoiceString,string choiceToFind)
		{
			try
			{
				string []answers = 	multipleChoiceString.Split('^');
				foreach(string answer in answers)
				{
					if(answer == choiceToFind)
						return true;
				}
				return false;
			}
			catch (Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @IsChoiceExistsInMulitpleChoicies(   line==> 277",exp,null,false);
			return false;
			}

		}

		# endregion 

		#region SetResultData(PollType type)
		private void SetResultData(PollType type)
		{

			try
			{

				ArrayList values = new ArrayList();
				ArrayList tooltips=new ArrayList();
				bool notNull=false;
				int count=0;
				// get the "waiting for reply " count
				for(int j=0;j<this.AnswersList.Count;j++)
				{
					if(((PollingAnswer)this.AnswersList[j]).choiceIndex==-1)
					{
						count++;
						notNull=true;
					}
				}

				
				//			if(count != 0)
				//			{
				if(this.AnswersList.Count<1)
				{
					values.Add((decimal)count);						
					tooltips.Add("waiting for reply");
				}
				//			}

				// get the count for each option
				if(( type != PollType.MultipleSelect) && (type != PollType.FreeResponse))
				{
					//Trace.WriteLine("how many options there " + this.choices.Count.ToString());
					for(int i=0;i<this.choices.Count;i++)
					{
						// get their values and insert them
						count=0;
						for(int j=0;j<this.AnswersList.Count;j++)
						{
					
							if(((PollingAnswer)AnswersList[j]).choiceIndex==i)
							{
								count++;
								notNull=true;
							}
						}
						//					if(count != 0)
						//					{
						values.Add((decimal)count);
						tooltips.Add((string)choices[i]);

						//					}
					
					}
				}
				else if(type == PollType.MultipleSelect)
				{
					//Trace.WriteLine("how many options in multi select : " + this.choices.Count.ToString());
					for(int i=0;i<this.choices.Count;i++)
					{
						// get their values and insert them
						count=0;
						for(int j=0;j<this.AnswersList.Count;j++)
						{
							if((((PollingAnswer)AnswersList[j]).choice != "") && (((PollingAnswer)AnswersList[j]).choice !=null))
							{												
								if(IsChoiceExistsInMulitpleChoicies(((PollingAnswer)AnswersList[j]).choice,(string)choices[i]))
								{
									count++;
									notNull=true;
								}
							}
						}
						//					if(count != 0)
						//					{
						values.Add((decimal)count);
						tooltips.Add((string)choices[i]);
						//					}
					}			
				}
			
		
				pr.SetToolTips(tooltips);
				/*          Zaeem  Asif           */           
				//There are some checks introduced in order to avoid  
				// Array Index out of bound @ -ve Index occurance in code 
				/*	                              */

				try
				{
					if(notNull==false)
					{
							decimal i=1;
						if(values.Count!=0)
						{
							values.RemoveAt(0);
							values.Insert(0,i);//=i;//=(decimal)1;
						}
						
					}
					if((type != PollType.MultipleSelect) && (type != PollType.FreeResponse))
					{
						pr.SetValues(values);				
					}
				}
				catch(Exception exp)
				{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @SetResultData(PollType type)   line==> 398",exp,null,false);
				}


				pr.SetSummary(values,tooltips);
				pr.SetAttendeeAnswers(this.AnswersList,type);
				pr.Text=this.question;

			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @SetResultData(PollType type)   line==> 408",exp,null,false);
			
			}

		
		}

		#endregion

		#region  ShowResults()
		public void ShowResults()
		{
			try
			{
				SetResultData(thisPollType);
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @ShowResults()   line==> 423",exp,null,false);
			
			}

		//	pr.ShowDialog();			
		}
		#endregion
		
		# region HandlePollingAnswer(PollAnswerMessage msg)
		public void HandlePollingAnswer(PollAnswerMessage msg)
		{
			try
			{
				//if((thisPollType != PollType.FreeResponse) && (thisPollType != PollType.MultipleSelect))
			{
				for(int i=0;i<this.AnswersList.Count;i++)
				{
					if(((PollingAnswer)AnswersList[i]).clientId==msg.clientID)
					{
						((PollingAnswer)AnswersList[i]).choiceIndex=msg.choice;
						((PollingAnswer)AnswersList[i]).choice=msg.choiceText;
						((PollingAnswer)AnswersList[i]).clientName=msg.clientName;
				
						SetResultData(thisPollType);
						return;
					}				
				}

			}
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @HandlePollingAnswer(PollAnswerMessage msg)  line==> 456",exp,null,false);
			
			}
				/*
			else //if(thisPollType == PollType.MultipleSelect)
			{
				for(int i=0;i<this.AnswersList.Count;i++)
				{
					if(((PollingAnswer)AnswersList[i]).clientId==msg.clientID)
					{						
						((PollingAnswer)AnswersList[i]).choice=msg.choiceText;
						((PollingAnswer)AnswersList[i]).clientName=msg.clientName;
						((PollingAnswer)AnswersList[i]).choiceIndex=msg.choice;
						SetResultData(thisPollType);
						return;
					}				
				}
			}			*/
		}

		# endregion 

		# region LockPoll()
		public void LockPoll()
		{
			try
			{
				PollStatusUpdateMessage msg=new PollStatusUpdateMessage();
				msg.enabled=true;
				msg.locked=true;
				NetworkManager.getInstance().SendLoadPacket(msg);
				// send the message
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @LockPoll() line==> 491",exp,null,false);
			}
		}
		# endregion 

		# region UnlockPoll()
		public void UnlockPoll()
		{
			try
			{
				PollStatusUpdateMessage msg=new PollStatusUpdateMessage();
				msg.enabled=true;
				msg.locked=false;
				NetworkManager.getInstance().SendLoadPacket(msg);
			
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @UnlockPoll() line==> 507",exp,null,false);
			}
		
			
			// send the message
		}
		# endregion 

		# region ClosePoll
		public void ClosePoll()
		{
			try
			{
				PollStatusUpdateMessage msg=new PollStatusUpdateMessage();
				msg.enabled=false;
				NetworkManager.getInstance().SendLoadPacket(msg);
				// send the message
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @ClosePoll() line==> 529",exp,null,false);
			}

		}

		# endregion

		#region // attendee functions comented
		// attendee functions
		/*
		private Thread DialogThread;
		public void threadFunction()
		{
				
			DialogResult res=attendeeForm.ShowDialog();
			attendeeForm.thisWindowPollingObject = this;
			if(res==DialogResult.Cancel)
				return;
			DialogThread.Abort();			
		}*/
		#endregion

		# region ShowAttendeeForm(PollType type)
		
		
		

		public void ShowAttendeeForm(PollType type)
		{
			this.testtype=type;
//			Del_Attendeeform del_Attendee=new Del_Attendeeform(test1);
//			ClientUI.getInstance().BeginInvoke(del_Attendee);
//			
			new Thread(new ThreadStart(AttendeeFormBlock)).Start();

			/*
			try
			{
				if((type == PollType.MultipleSelect ) && (type == PollType.FreeResponse))
					pr.DisableCharting();

				attendeeForm.SetAnonymousPoll(this.anonymous);
				attendeeForm.SetQuestion(this.question);
				attendeeForm.SetOptions(type,this.choices);			
	
				if(attendeeForm.Text.IndexOf("Evaluation") == -1)
					attendeeForm.Text = "Poll By: " + ClientName;
				else
					attendeeForm.Text = "Evaluation By: " + ClientName;


				attendeeForm.thisWindowPollingObject = this;
			
				if(DialogResult.OK == attendeeForm.ShowDialog())
				{				
					PollAnswerMessage msg=new PollAnswerMessage();
					if((type != PollType.MultipleSelect ) && (type != PollType.FreeResponse))
						msg.choice=attendeeForm.GetAnswerIndex();
					else
						msg.choice= -2;
					msg.choiceText=attendeeForm.GetAnswerText(type);
					msg.sessionID=this.sessionID;
					msg.clientName=NetworkManager.getInstance().profile.Name;				
					msg.clientID=NetworkManager.thisInstance.profile.ClientId;
					msg.questiontext = attendeeForm.getQuestionText();
					msg.pollType = type;
				
					msg.choices = getChoicesString(this.choices);
				
					msg.ConferenceID = NetworkManager.getInstance().profile.ConferenceID ;				
					NetworkManager.getInstance().SendLoadPacket(msg);
				}
			

				return false;
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @ShowAttendeeForm(PollType type) line==> 596",exp,null,false);
				return false;
			}

			*/

		}
	
	 	# endregion  

/*
		public void test1()
		{
			
			thr.Name="test thread";
			thr.Start();
			

		}

*/
		public void AttendeeFormBlock()
		{



			this.attendeeForm=new PollAttendeeForm();
			this.ArrayAttendeeForm.Add(this.attendeeForm);


			PollType type=this.testtype;
			try
			{
				if((type == PollType.MultipleSelect ) && (type == PollType.FreeResponse))
					pr.DisableCharting();

				attendeeForm.SetAnonymousPoll(this.anonymous);
				attendeeForm.SetQuestion(this.question);
				attendeeForm.SetOptions(type,this.choices);			
	
				if(attendeeForm.Text.IndexOf("Evaluation") == -1)
					attendeeForm.Text = "Poll By: " + ClientName;
				else
					attendeeForm.Text = "Evaluation By: " + ClientName;


				attendeeForm.thisWindowPollingObject = this;
			
				

				if(DialogResult.OK == attendeeForm.ShowDialog())
				{				
					
					PollAnswerMessage msg=new PollAnswerMessage();
					if((type != PollType.MultipleSelect ) && (type != PollType.FreeResponse))
						msg.choice=attendeeForm.GetAnswerIndex();
					else
						msg.choice= -2;
					msg.choiceText=attendeeForm.GetAnswerText(type);
					msg.sessionID=this.sessionID;
					msg.clientName=NetworkManager.getInstance().profile.Name;				
					msg.clientID=NetworkManager.thisInstance.profile.ClientId;
					msg.questiontext = attendeeForm.getQuestionText();
					msg.pollType = type;
					msg.choices = getChoicesString(this.choices);
					msg.ConferenceID = NetworkManager.getInstance().profile.ConferenceID ;				
					NetworkManager.getInstance().SendLoadPacket(msg);


				}
			

				
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @ShowAttendeeForm(PollType type) line==> 596",exp,null,false);
				
			}

			

		
		
		}

		# region string getChoicesString(ArrayList aryChoices)
		public string getChoicesString(ArrayList aryChoices)
		{
			System.Text.StringBuilder st = new System.Text.StringBuilder("");
			try
			{
				
				
				for(int i=0;i<aryChoices.Count ; i++)
				{
					if(i != 0)
						st.Append(":");
					st.Append( aryChoices[i].ToString() );
				}
				return st.ToString();
			}
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @getChoicesString(ArrayList aryChoices) line==> 623",exp,null,false);
				return null;
			}

					
		}
		# endregion

		# region SetParameters(string clientname,int clientid )
		public void SetParameters(string clientname,int clientid )
		{
			this.ClientName=clientname;			
			this.ClientID=clientid;
		}
		# endregion 

		# region HandlePollStatusUpdateMessage(PollStatusUpdateMessage msg)
		public void HandlePollStatusUpdateMessage(PollStatusUpdateMessage msg)
		{
			if(msg.enabled==false)
			{
			}
			if(msg.locked==true)
			{
			}
		}
		#endregion 

		# region HandleOpenPollMessage(NewPollMessage msg)
		public void HandleOpenPollMessage(NewPollMessage msg)
		{
			try
			{
				this.choices=new ArrayList();
				for(int i=0;i<msg.choices.Count;i++)
				{
					choices.Add(msg.choices[i]);
				}
				this.question=msg.Question;		
			}	
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @getChoicesString(ArrayList aryChoices) line==> 623",exp,null,false);
			}
			}

		# endregion 
		
		# region string GetQuestionTypeString()
		public string GetQuestionTypeString()
		{
			string str = "";
			switch(this.thisPollType)
			{
				case PollType.FreeResponse:
					return "Free Response";
				case PollType.MultipleSelect:
					return "Multiple Select";
				case PollType.SingleSelect:
					return "Single Select";
				case PollType.TrueFalse:
					return "True/False";
				case PollType.YesNo:
					return "Yes/No";				
			}
			return str;
		}
		# endregion 

		# region Save()
		public void Save()
		{
			if((this.saveFileName=="") || (saveFileName == null))
			{
				SaveAs();
				return;
			}
			try
			{		
				XmlTextWriter writer = new XmlTextWriter(saveFileName,System.Text.Encoding.UTF7);
				writer.WriteStartDocument();
				writer.WriteComment("Poll results saved by WebMeeting");				
				writer.WriteStartElement("WebMeeting","PollResult");
				
				writer.WriteStartElement("Question");
				writer.WriteAttributeString("Type",GetQuestionTypeString());
				writer.WriteString(this.question);
				writer.WriteEndElement();		
				/*
				if(this.thisPollType != PollType.FreeResponse)
				{
					writer.WriteStartElement("Statistics");                
					for(int z= 0 ; z < 	this.pr.listViewSummary.Items.Count ; z++)
					{
						writer.WriteStartElement("Choice");						
						writer.WriteAttributeString("Text",this.pr.listViewSummary.Items[z].SubItems[0].Text);
						writer.WriteAttributeString("Count",this.pr.listViewSummary.Items[z].SubItems[2].Text);
						writer.WriteString(this.pr.listViewSummary.Items[z].SubItems[3].Text + "%");
						writer.WriteEndElement();					
					}
					writer.WriteEndElement();
				}
				*/
				writer.WriteStartElement("Answers");				
				for(int i = 0; i < this.pr.listViewAttendeeAnswers.Items.Count ; i++)
				{					
					writer.WriteStartElement("Answer");
					writer.WriteAttributeString("By",this.pr.listViewAttendeeAnswers.Items[i].SubItems[0].Text);
					writer.WriteString(this.pr.listViewAttendeeAnswers.Items[i].SubItems[1].Text);
					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndElement();                                				
				writer.Close();			

			
				return ;
			}
			
			
			catch (Exception exp)
			{
				MessageBox.Show("Couldnt save to " + saveFileName,"WebMeeting",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}
		# endregion 

		# region SaveAs
		public void SaveAs()
		{
		
			try
			{
				SaveFileDialog fd=new SaveFileDialog();
				fd.Filter = "XML Files (*.xml)|*.xml";
				DialogResult res=fd.ShowDialog();
				if(res==DialogResult.Cancel)
					return;
				
				saveFileName=fd.FileName;
				Save();
			}			
			catch (Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Polling ===>Polling.cs @SaveAs() line==> 768",exp,null,false);
			}
		
		}
		# endregion 

		# region saveMenu_Click(object sender, EventArgs e)
		private void saveMenu_Click(object sender, EventArgs e)
		{
			Save();	

		}
		# endregion

		# region
		private void saveAsMenu_Click(object sender, EventArgs e)
		{
			SaveAs();

		}
		# endregion
	}
}
