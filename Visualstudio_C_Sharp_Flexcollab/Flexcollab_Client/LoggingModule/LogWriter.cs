using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace BinaryReaderWriter
{
	/// <summary>
	/// Summary description for LogWriter.
	/// </summary>
	public class LogWriter
	{
		public LogWriter()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		BinaryWriter binWriter;
		public string strLoggingThreadName="";		
		public void SetUpLogging()
		{						
			
			binWriter=new BinaryWriter(File.Open(RetFileName(), FileMode.Create));
		}		
		public void CloseLogging()
		{
			binWriter.Close();
		}
		private string RetFileName()
		{
			string strLogFileName="";
			string strLogFileNamePreFix=Application.StartupPath +@"\log\"+DateTime.Today.ToShortDateString();
			strLogFileNamePreFix=strLogFileNamePreFix.Replace(@"/","-");//remove / from date
			string strLogFileNamePostFix=".cnavlog";
			strLogFileName=strLogFileNamePreFix+strLogFileNamePostFix;
			int filecounter=1;
			while(System.IO.File.Exists(strLogFileName))
			{
				strLogFileName=	strLogFileNamePreFix+"("+filecounter.ToString()+")"+strLogFileNamePostFix;
				filecounter+=1;
			} 
			return strLogFileName;
		}
		private object objSync=new object();
		public void WriteLog(string str)
		{
			strLoggingThreadName=System.Threading.Thread.CurrentThread.Name;

			lock(objSync)
			{
				if (str == null)
					this.binWriter.Write(string.Empty);
				else
					this.binWriter.Write(str);
				this.binWriter.Flush();
			}
		}
	}
}
