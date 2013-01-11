using System;

using System.Collections;
using MySQL;
using System.Windows.Forms;
using WebMeeting.Common;

namespace WebMeeting.WebIntegeration
{
	/// <summary>
	/// Summary description for WebIntegeration.
	/// </summary>
	/// 
	
	public class WebFiles
	{
		
		public static ArrayList GetWebFiles(string serverPath,string clientId,MySqlWrapper dbConn)
		{
			ArrayList WebFiles = new ArrayList();
			try
			{
				string sql = "select Concat(uploads_id,uploads_url) as FilePath,uploads_title from ctool_uploads where uploads_ownerid= " + clientId;				
				ArrayList recordset = dbConn.Execute(sql);
				foreach(ArrayList record in recordset)
				{
					WebUploadedFiles file = new WebUploadedFiles((string)record[1],serverPath + record[0]);
					WebFiles.Add(file);
				}

				sql = "select Concat(uploads_id,uploads_url) as FilePath,uploads_title from ctool_uploads,ctool_uploadstouser where uploads_id = uploadstouser_uploads_id AND uploadstouser_mid = " + clientId;
				recordset = dbConn.Execute(sql);
				foreach(ArrayList record in recordset)
				{
					WebUploadedFiles file = new WebUploadedFiles((string)record[1],serverPath + record[0]);
					WebFiles.Add(file);
				}               
				
			}			
			catch(Exception ee)
			{
//				MessageBox.Show("We Integration " + ee.Message);
			}
			return WebFiles;
		}
	}
	public class WebBookMarks
	{
		public static ArrayList GetWebBookMarks(string serverPath,string clientID, MySqlWrapper dbConn)
		{
			ArrayList webBookmarks = new ArrayList();
			try
			{
				string sql = "select bookmark_title,bookmark_url from ctool_bookmark where bookmark_ownerid = " + clientID;
				ArrayList recordset = dbConn.Execute(sql);
				foreach(ArrayList record in recordset)
				{
					WebBookMark entry = new WebBookMark((string)record[0],(string)record[1]);					
					webBookmarks.Add(entry);
				}

				sql = "select bookmark_title,bookmark_url from ctool_bookmark,ctool_bmtouser where bookmark_id = bmtouser_bookmark_id AND bmtouser_mid = " + clientID;
				recordset = dbConn.Execute(sql);
				foreach(ArrayList record in recordset)
				{
					WebBookMark entry = new WebBookMark((string)record[0],(string)record[1]);					
					webBookmarks.Add(entry);
				}
				
			}
			catch(Exception ee)
			{
			//	MessageBox.Show(ee.Message);
			}
			return webBookmarks;
		}
		
	}
	public class WebPresentations
	{
		public WebPresentations()
		{
		}

		
		public static ArrayList GetWebPresentations(string serverPath,string clientID, MySqlWrapper dbConn)
		{
			ArrayList WebPresentations = new ArrayList();
			try
			{
				string sql = "select presentation_title,Concat(presentation_id,presentation_url) from ctool_presentation where presentation_ownerid = " + clientID;
				ArrayList recordset = dbConn.Execute(sql);
				foreach(ArrayList record in recordset)
				{
					WebPresentationEntry entry = new WebPresentationEntry((string)record[0],serverPath + record[1]);					
                    WebPresentations.Add(entry);
				}
				
				sql = "select presentation_title,Concat(presentation_id,presentation_url),pretouser_presentation_id from ctool_presentation,ctool_pretouser WHERE presentation_id = pretouser_presentation_id  AND pretouser_mid = " + clientID;

				recordset = dbConn.Execute(sql);
				foreach(ArrayList record in recordset)
				{
					WebPresentationEntry entry = new WebPresentationEntry((string)record[0],serverPath + record[1]);					
					WebPresentations.Add(entry);
				}
			
			}
			catch(Exception ee)
			{
		//		MessageBox.Show(ee.Message);
			}
			return WebPresentations;
		}
	}

	public class WebPolls
	{	
		
		public ArrayList WebPollArray = new ArrayList();
		private MySqlWrapper dbCon;
		public WebPolls(MySqlWrapper dbConn)
		{
			dbCon = dbConn;
			
		}
		
		private ArrayList GetAnswersOfPoll(string nPollID)
		{
			ArrayList answerList = new ArrayList();			
			string []answerInformation = new string[2];
			try
			{
				string sql = "Select A.q_answer_mid,A.q_answer_ans,C.company_member_fname,C.company_member_lname from ctool_q_answer A, ctool_company_member C where A.q_answer_mid = C.company_member_id AND A.q_answer_qid =" + nPollID;
				ArrayList recordset = dbCon.Execute(sql);
				if(recordset.Count > 0)
				{
					foreach(ArrayList record in recordset)
					{
						WebPollAnswer answer = new WebPollAnswer((string)record[1],(string)record[2] + ' ' + ((string)record[3]));
						answerList.Add(answer);
					}
					
				}
			
			}
			catch(Exception ee)
			{
			//	MessageBox.Show(ee.Message);
			}

			return answerList;
		}
		
		public bool GetAllPolls(string ClientID)
		{			
			try
			{				
				bool nError = false;
				string sql;
				sql = "Select * from ctool_question where question_ownerid= " + ClientID;				
				ArrayList results =  dbCon.Execute(sql);
				for(int i = 0 ; i < results.Count ; i++)
				{
					ArrayList Fields = (ArrayList)results[i];	
					WebPoll poll = new WebPoll();
					poll.question = (string)Fields[2].ToString();
					poll.questionOwner = Convert.ToInt32(Fields[1]);
					int Type = Convert.ToInt32(Fields[3]);
					switch(Type)
					{
						case 1:
							poll.pollType = PollType.FreeResponse;
							break;						
						case 2:
							poll.pollType = PollType.SingleSelect;
							break;
						case 3:
							poll.pollType = PollType.MultipleSelect;
							break;
						case 4:
							poll.pollType = PollType.TrueFalse;
							break;
						case 5:
							poll.pollType = PollType.YesNo;
							break;
						default:
							poll.pollType = PollType.SingleSelect;
							break;

					}

					string answers = (string)Fields[4];
					string[] choices = answers.Split(':');					
					poll.choices= new ArrayList();
					foreach (string choice in choices)
						poll.choices.Add(choice);

					poll.WebPollAnswersList = GetAnswersOfPoll((string)Fields[0]);					

					WebPollArray.Add(poll);			
				}

				return true;
	

			}
			catch(Exception ee)
			{
			//	MessageBox.Show(ee.Message);

			}

			return false;

		}

	}

	public class WebEvaluations
	{	
		
		public ArrayList WebPollArray = new ArrayList();
		private MySqlWrapper dbCon;
		public WebEvaluations(MySqlWrapper dbConn)
		{
			dbCon = dbConn;
			
		}
		
		private ArrayList GetAnswersOfPoll(string nPollID)
		{
			ArrayList answerList = new ArrayList();			
			string []answerInformation = new string[2];
			try
			{
				string sql = "Select A.q_answer_mid,A.q_answer_ans,C.company_member_fname,C.company_member_lname from ctool_q_answer A, ctool_company_member C where A.q_answer_mid = C.company_member_id AND A.q_answer_qid =" + nPollID;
				ArrayList recordset = dbCon.Execute(sql);
				if(recordset.Count > 0)
				{
					foreach(ArrayList record in recordset)
					{
						WebPollAnswer answer = new WebPollAnswer((string)record[1],(string)record[2] + ' ' + ((string)record[3]));
						answerList.Add(answer);
					}
					
				}
			
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
			}

			return answerList;
		}
		
		public bool GetAllEvaluations(string ClientID)
		{			
			try
			{				
		
				string sql;
				sql = "Select * from ctool_evaluation where question_ownerid= " + ClientID;				
				ArrayList results =  dbCon.Execute(sql);
				for(int i = 0 ; i < results.Count ; i++)
				{
					ArrayList Fields = (ArrayList)results[i];	
					WebPoll poll = new WebPoll();
					poll.question = (string)Fields[2].ToString();
					poll.questionOwner = Convert.ToInt32(Fields[1]);
					int Type = Convert.ToInt32(Fields[3]);
					switch(Type)
					{
						case 1:
							poll.pollType = PollType.FreeResponse;
							break;						
						case 2:
							poll.pollType = PollType.SingleSelect;
							break;
						case 3:
							poll.pollType = PollType.MultipleSelect;
							break;
						case 4:
							poll.pollType = PollType.TrueFalse;
							break;
						case 5:
							poll.pollType = PollType.YesNo;
							break;
						default:
							poll.pollType = PollType.SingleSelect;
							break;

					}

					string answers = (string)Fields[4];
					string[] choices = answers.Split(':');					
					poll.choices= new ArrayList();
					foreach (string choice in choices)
						poll.choices.Add(choice);

					//poll.WebPollAnswersList ;// GetAnswersOfPoll((string)Fields[0]);					

					WebPollArray.Add(poll);			
				}

				return true;
	

			}
			catch(Exception ee)
			{
		//		MessageBox.Show(ee.Message);

			}

			return false;

		}

	}
	
}
