using System;
using System.Runtime.InteropServices;
using MySQL;
using System.Collections;

namespace MySQL
{


	/// <summary>
	/* 
	 * 1  MySqlWrapper is a class which takes Server User Password and the database name on which u r working
	 * 2 Connect is the method which u want to know that is connection is ok or not
	 * 3 GetLastError is the method to get the last error occured for which ur query was not successfull
	 * 4 procResults is not mine i got it from Test class
	 * 5 rowVal same case
	 * 6 GetNumRows how many rows were affected
	 * 7 GetNumFeilds how many no of feilds were used in the query
	 * 8 NameOfFeilds is for taking the names used in the query if u r not botherin it to remember like select * 
	 * 9 GetRowFirst get the first row of the query whatever it is
	 * 10 GetRows is the core method of this class it collect all the result whatever the query was
	 * 11 GetColumnByName is the method which collect the column by name whatever the query was
	 * 12 GetColumnByNo is the method which collect the column by no it had in the table whatever the query was
	 * 13 listProcesses not mine i got it from test.cs
	 * 14 listTables  is for listing tables in the given object which is initialized for a database
	 * */
	/// </summary>
	public class MySqlWrapper
	{

		IntPtr db;
		IntPtr conn;
		int sdb ;
		string LastError;

		public MySqlWrapper(/*string Server,string User, string Password,string Database*/)
		{			
			db = MySql.Init(IntPtr.Zero);
			if (db == IntPtr.Zero)
				throw new ApplicationException("?MySQL Init failed");			
			 
			/*conn = MySql.Connect(db,Server, User, Password, Database, MySql.Port,
				null, (uint)0);
			// sdb = MySql.SelectDb(db, Database);*/
		}

		public bool Connect(string server, string user, string password)
		{	
			string dbname = "";

			conn = MySql.Connect(db, server, user, password,dbname, MySql.Port,null, (uint)0);

			if (conn == IntPtr.Zero)			
			{
				LastError = MySql.Error(db);
				return false;
			}
			else
			{
				return true;
			}

		}

		public void Close()
		{
			MySql.Close(db);
		}
		public string GetLastError()
		{
			return LastError;

		}

		public bool SelectDB(string dbName)
		{	  
			sdb = MySql.SelectDb(db,dbName);
		
			if (sdb != 0)
			{
				LastError = MySql.Error(db);
				return false;
			}
			else
			{
				return true;
			}

		}
		static void procResults(IntPtr db)
		{
			IntPtr res = MySql.StoreResult(db);
			int numRows = MySql.NumRows(res);
			Console.WriteLine("Number of records found: " + numRows);
			int numFields = MySql.NumFields(res);
			string[] fields = new string[numFields];
			for (int i = 0; i < numFields; i++)
			{
				Field fd = (Field) Marshal.PtrToStructure(MySql.FetchField(res), typeof(Field));
				fields[i] = fd.Name;
			}
			IntPtr row;
			int recCnt = 1;
			while ((row = MySql.FetchRow(res)) != IntPtr.Zero)
			{
				Console.WriteLine("Record #" + recCnt + ":");
			
				for (int i = 0, j = 1; i < numFields; i++, j++)
				{
					Console.WriteLine("  Fld #"+j+" ("+fields[i]+"): "+rowVal(row, i));
				}
				Console.WriteLine("==============================");
			}
			MySql.FreeResult(res);
		}

		static string rowVal(IntPtr res, int index)
		{
			IntPtr str = Marshal.ReadIntPtr(res, index*IntPtr.Size);
			if (str == IntPtr.Zero)
				return "NULL";
			string s = Marshal.PtrToStringAnsi(str);
			return s;
		}

		public int  GetNumRows(string query)
		{			
			int rcq = MySql.Query(db, query);
			if (rcq != 0)
			{
				LastError = MySql.Error(db);
				return 0;
			}
			
			IntPtr res = MySql.StoreResult(db);			
			int numRows = MySql.NumRows(res);
			return numRows;
		}
		
		public int GetNumFeilds(string query)
		{			
			int rcq = MySql.Query(db, query);
			if (rcq != 0)
			{
				LastError = MySql.Error(db);
				return 0;
			}
			
			IntPtr res = MySql.StoreResult(db);
			int numFields = MySql.NumFields(res);
			return numFields;
		}

		// return First Row.
		public ArrayList GetRow(string query)
		{
			ArrayList List = new ArrayList();

			int rcq = MySql.Query(db, query);
			if (rcq != 0)
			{
				LastError = MySql.Error(db);	
				return List;
			}
			
			IntPtr res = MySql.StoreResult(db);
		
			int numRows = MySql.NumRows(res);
			
			int numFields = MySql.NumFields(res);
	
			IntPtr row;	
		
			row = MySql.FetchRow(res);
		
			if(row != IntPtr.Zero)
			{
				for (int i = 0; i < numFields; i++)
				{				
					List.Add(rowVal(row, i));
				}
			}
			
			MySql.FreeResult(res);
			return List;
		}

		public ArrayList GetRowByNo(string query, int row)
		{
			ArrayList List = Execute(query);

			if(row<List.Count)
				return (ArrayList)List[row];
			else
				return new ArrayList();
		}

		public string GetOne(string query)
		{
			string result = "";

			int rcq = MySql.Query(db, query);

			if (rcq != 0)
			{
				LastError = MySql.Error(db);
				return result;
			}
			
			IntPtr res = MySql.StoreResult(db);
			
			IntPtr row;			
		
			row = MySql.FetchRow(res);
			
			if(row != IntPtr.Zero)
				result = rowVal(row, 0);			
			
			
			MySql.FreeResult(res);

			return result;
		}


		public ArrayList GetAll(string query)
		{
			ArrayList List = new ArrayList();	
		
			int rcq = MySql.Query(db, query);
		
			if (rcq != 0)
			{
				LastError = MySql.Error(db);
				return List;
			}
			
			IntPtr res = MySql.StoreResult(db);
			
			if(res == IntPtr.Zero)
			{
				return List;
			}
			int numRows =0;
			
			numRows	= MySql.NumRows(res);
			
			if(numRows == 0)
			{
				return new ArrayList();
			}

			int numFields = MySql.NumFields(res);
		
			IntPtr row;
			
			while ((row = MySql.FetchRow(res)) != IntPtr.Zero)
			{				
				ArrayList temp = new ArrayList();
				for (int i = 0, j = 1; i < numFields; i++, j++)
				{
					temp.Add(rowVal(row, i));					
				}
				List.Add(temp);
			}
			MySql.FreeResult(res);

			return List;
		}


		public ArrayList Execute(string query)
		{
			GC.Collect(); //GC
			ArrayList List = new ArrayList();	
		
			int rcq = MySql.Query(db, query);
		
			if (rcq != 0)
			{
				LastError = MySql.Error(db);
				return List;
			}
			
			IntPtr res = MySql.StoreResult(db);
			
			if(res == IntPtr.Zero)
			{
				return List;
			}
			int numRows =0;
			
			numRows	= MySql.NumRows(res);
			
			if(numRows == 0)
			{
				return new ArrayList();
			}

			int numFields = MySql.NumFields(res);
		
			IntPtr row;
			
			while ((row = MySql.FetchRow(res)) != IntPtr.Zero)
			{				
				ArrayList temp = new ArrayList();

				for (int i = 0, j = 1; i < numFields; i++, j++)
				{
					temp.Add(rowVal(row, i));					
				}
				List.Add(temp);
				temp = null;
			}
			MySql.FreeResult(res);
			
			GC.Collect(); //GC
			//GC.WaitForPendingFinalizers(); //GC

			return List;
		}

		public ArrayList GetColumnByName(string query, string name)
		{
			ArrayList List = new ArrayList();	
			
			int no = -1;

			int rcq = MySql.Query(db, query);
	
			if (rcq != 0)
			{
				LastError = MySql.Error(db);
				return List;
			}
			
			IntPtr res = MySql.StoreResult(db);
	
			int numFields = MySql.NumFields(res);
			
			for (int i = 0; i < numFields; i++)
			{
				Field fd = (Field) Marshal.PtrToStructure(MySql.FetchField(res), typeof(Field));
			
				if(fd.Name.Equals(name))
					no = i;

			}

			if(no == -1)
				return List;

			IntPtr row;			
			
			while ((row = MySql.FetchRow(res)) != IntPtr.Zero)
			{
				List.Add(rowVal(row, no));
			}

			MySql.FreeResult(res);

			return List;

		}

		public ArrayList GetColumnByNo(string query , int no)
		{
			ArrayList List = new ArrayList();	
			int rcq = MySql.Query(db, query);
			if (rcq != 0)
			{
				
				LastError = MySql.Error(db);
				return List;
			}
			
			IntPtr res = MySql.StoreResult(db);
			
			int numFields = MySql.NumFields(res);
			
			IntPtr row;
			
			while ((row = MySql.FetchRow(res)) != IntPtr.Zero)
			{
				List.Add(rowVal(row, no));
			}

			MySql.FreeResult(res);

			return List;
		}

		public ArrayList GetColumn(string query)
		{
			ArrayList List = new ArrayList();	
			int rcq = MySql.Query(db, query);
			if (rcq != 0)
			{
				
				LastError = MySql.Error(db);
				return List;
			}
			
			IntPtr res = MySql.StoreResult(db);
			
			int numFields = MySql.NumFields(res);
			
			IntPtr row;
			
			while ((row = MySql.FetchRow(res)) != IntPtr.Zero)
			{
				List.Add(rowVal(row, 0));
			}

			MySql.FreeResult(res);

			return List;
		}

		public ArrayList NameOfFeilds(string query)
		{
			ArrayList List = new ArrayList();	
		
			int rcq = MySql.Query(db, query);
			if (rcq != 0)
			{
				LastError = MySql.Error(db);
				return List;
			}
			
			IntPtr res = MySql.StoreResult(db);
			
			int numRows = MySql.NumRows(res);			
			int numFields = MySql.NumFields(res);
			
			for (int i = 0; i < numFields; i++)
			{
				Field fd = (Field) Marshal.PtrToStructure(MySql.FetchField(res), typeof(Field));
				List.Add(fd.Name);
			}

			return List;
		}
	
		public void ListProcesses(IntPtr db)
		{
			
			IntPtr res = MySql.ListProcesses(db);
			if (res == IntPtr.Zero)
			{
				Console.WriteLine("Got error "+MySql.Error(db)+" when retreiving processlist");
				return;
			}
			int numRows = MySql.NumRows(res);
			//    Console.WriteLine("Number of records found: "+i);
			int numFields = MySql.NumFields(res);
			string[] fields = new string[numFields];
			for (int i = 0; i < numFields; i++)
			{
				Field fd = (Field) Marshal.PtrToStructure(MySql.FetchField(res), typeof(Field));
				fields[i] = fd.Name;
			}
			IntPtr row;
			int recCnt = 1;
			while ((row = MySql.FetchRow(res)) != IntPtr.Zero)
			{
				Console.WriteLine("Process #" + recCnt + ":");
				for (int i = 0, j = 1; i < numFields; i++, j++)
				{
					Console.WriteLine("  Fld #"+j+" ("+fields[i]+"): "+rowVal(row, i));
				}
				Console.WriteLine("==============================");
			}
			MySql.FreeResult(res);
			fields = null; //GC
			GC.Collect(); //GC
			
		}

		public ArrayList ListTables()
		{
			ArrayList List = new ArrayList();	
			
			IntPtr res = MySql.ListTables(db, "%");
			if (res == IntPtr.Zero)
			{
				LastError = MySql.Error(db);
				return List;
			}
			int numFields = MySql.NumFields(res);
			
			IntPtr row;
			
			while ((row = MySql.FetchRow(res)) != IntPtr.Zero)
			{
				for (int i = 0; i < numFields; i++)
				{
					List.Add(rowVal(row, i));				
				}				
			}

			MySql.FreeResult(res);

			return List;
		}

		public string LastInsertID()
		{
			return GetOne("SELECT LAST_INSERT_ID()");
			
		}
	}
}
