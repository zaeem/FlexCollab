using System;
using System.Data;
using SQLite.NET;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using Finisar.SQLite;
using System.Diagnostics;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
						
//								This class was created by Zaeem                                              //

///////////////////////////////////////////////////////////////////////////////////////////////////////////////




namespace WebMeeting.Client.ClientDataBase
{
	
	/// <summary>
	/// Summary description for DatabaseAccess.
	/// </summary>
	public class DatabaseAccess
	{
		
		private SQLiteClient m_sqlClient;
		private string m_szDatabase;
		//private Finisar.SQLite.SQLiteConnection AdapterSqlClient;
		

		//		public DatabaseAccess(string szDatabaseName)
		//		{
		//			//
		//			// TODO: Add constructor logic here
		//			//
		//
		//			m_szDatabase = szDatabaseName;
		//			ConnectToDatabase();
		//		}
		
		#region Constructor
		public DatabaseAccess(string szDatabaseName)
		{
			//
			// TODO: Add constructor logic here
			//

			m_szDatabase = szDatabaseName;
			ConnectToDatabase();
		}
		#endregion

		
		# region Close DB
		public void Close()
		{
			this.m_sqlClient.Close();
		}

		#endregion


		# region DB Connection
		private bool ConnectToDatabase()
		{
			try 
			{
				this.m_sqlClient = new SQLiteClient(Application.StartupPath+"/WebMeeting_Client.dll");
				
			}
			catch(Exception e)
			{
				// display error
				//MessageBox.Show(e.Message.ToString());
				return false;
			}
			return true;
		}		
		#endregion
		
	
		#region Execute query
		private bool ExecuteQuery(string query)
		{
			try 
			{
				this.m_sqlClient.Execute(query);
			}
			catch(Exception e)
			{
				//MessageBox.Show(e.Message.ToString());
				try
				{
					this.ConnectToDatabase();
					this.m_sqlClient.Execute(query);
				}
				catch(Exception exp)
				{
					return false;
				}
				return true;
			}
			return true;
		}
		#endregion

		#region Getrecords from the DB
		public ArrayList Getfilepath_fromDb()

		{
			string query = "select * from Client_MM";
			ArrayList array = this.m_sqlClient.GetAll(query);
			return array;// innerArray1=(ArrayList)innerArray;
		
		}
		#endregion

		#region Save file info into Db
		public bool Save_FileInfo_TobeUpload(int meetingID,string Host,string  Presenter,string  Module,string  Path,string Current_DateTime,string Mis,string relativePath)
		{
			string query1="insert into Client_MM(meetingid,Host,presenter,module,path,datetime,Mis,relativeurl) values("+meetingID+",'"+Host+"','"+Presenter+"','"+Module+"','"+Path+"','"+Current_DateTime+"','"+Mis+"','"+relativePath+"')";
			//query="insert into Files_todownload  (id,pages,path) values(1,2,3)";
			if(this.ExecuteQuery(query1))
				return true;
			else
				return false;
		
		
		}

		# endregion

		
		#region Delete data
		public bool Delete(string query)
		{
			if(this.ExecuteQuery(query))
				return true;
			else
				return false;
		
		}

		#endregion 

		#region Get the no of records from the database 
		public int GetNumberofRecordsInTable(string tableName)	
		{			
			
			string query="";
			string strResult="";

			switch(tableName)
			{
				case "Client_MM":

					query = "SELECT count( * ) from " + tableName;
					break;

				case "Resolution":
					query = "SELECT count( * ) from " + tableName;
					break;
				
			};
			try 
			{
				strResult=this.m_sqlClient.GetOne(query);
			}
			catch(Exception e)
			{								
				return 0;				
			}
			//strResult=this.m_sqlClient.GetOne(query);
			return int.Parse(strResult);

		}
		
	
		#endregion 


		#region Get all records from the database 
		public bool DeleteRecordsfromTable(string tableName)	
		{			
			
			string query="";
			string strResult="";

			switch(tableName)
			{

				case "Resolution":
					query = "Delete  from " + tableName;
					break;
				
			};
			try 
			{
				this.m_sqlClient.Execute(query);
				return true;
			}
			catch(Exception e)
			{								
				return false;				
			}
			//strResult=this.m_sqlClient.GetOne(query);
			

		}
		
	
		#endregion 





///////////////////////////////////////////////////////////////////////////////////////////
///------------------------------------------------------------------------------------ ///
///.................................................................................... ///
///								RESOLUTION SECTION									    ///
///.................................................................................... ///
///------------------------------------------------------------------------------------ ///
///////////////////////////////////////////////////////////////////////////////////////////

		
		/// <summary>
		/// Enters the given resolutionm in the database
		/// </summary>
		/// <param name="inc_resolution"></param>
		/// <returns></returns>
		#region EnterResolutiontoDB
		public bool EnterResolutiontoDB(int inc_tobechanged,int inc_width,int inc_height)
		{

			string query="insert into Resolution(tobechanged,width,height) values("+inc_tobechanged+","+inc_width+","+inc_height+")";
			//query="insert into Files_todownload  (id,pages,path) values(1,2,3)";
			if(this.ExecuteQuery(query))
				return true;
			else
				return false;
		
		
		}
		#endregion 


		#region Get the width and height from Db 

		public int GetwidthfromDB()
		{
			string query="select width from Resolution";
			//query="insert into Files_todownload  (id,pages,path) values(1,2,3)";
			string strResult=this.m_sqlClient.GetOne(query);
			return int.Parse(strResult);
		
		}
		


		public int GetheightfromDB()
		{
			string query="select height from Resolution";
			//query="insert into Files_todownload  (id,pages,path) values(1,2,3)";
			string strResult=this.m_sqlClient.GetOne(query);
			return int.Parse(strResult);
		
		
		}
		

		#endregion 


	}
}
