using System;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for CustomExceptionunhandler.
	/// </summary>
	public class CustomExceptionunhandler
	{

		public string parameters;
		public CustomExceptionunhandler(  )
		{
		}
		public CustomExceptionunhandler( string ss )
		{

		}

		public CustomExceptionunhandler( Exception ExceptionDesc )
		{
			// Log the exception to a file
			LogException(ExceptionDesc);
			MessageBox.Show("Consturctor :::: CustomExceptionunhandler");
			// Tell the user that the app will restart			 			
			if(MessageBox.Show("Invalid operation !! click OK to restart the application",	"CompassNav",MessageBoxButtons.OK, MessageBoxIcon.Stop)==DialogResult.OK)
			{
				// Shut down the current app instance
				//Application.Exit();
				
				// Restart the app
				//System.Diagnostics.Process.Start(Application.ExecutablePath,parameters);
			}

		}
		//
			// TODO: Add constructor logic here
			//
			// Event handler that will be called when an unhandled
			// exception is caught
			public void NonThreadException( Exception ExceptionDesc )
			{
				// Log the exception to a file
				
				LogException(ExceptionDesc);

				// Tell the user that the app will restart
				//MessageBox.Show("A Fatal Error was detected and logged.	Click the OK button to restart the  application" + ExceptionDesc.Message+ExceptionDesc.Source+ExceptionDesc.StackTrace+ExceptionDesc.ToString(),
				//	"Fatal Application Error",
				//	MessageBoxButtons.OK, MessageBoxIcon.Stop);

				if(MessageBox.Show("Invalid operation !! click OK to restart the application",	"CompassNav",MessageBoxButtons.OK, MessageBoxIcon.Stop)==DialogResult.OK)
				{
					// Shut down the current app instance
					Application.Exit();
				
					// Restart the app
					System.Diagnostics.Process.Start(Application.ExecutablePath,parameters);
				}

				// Shut down the current app instance
				//Application.Exit();

				// Restart the app
				//System.Diagnostics.Process.Start(Application.ExecutablePath);
			}
			// Event handler that will be called when an unhandled
			// exception is caught
			public void OnThreadException(object sender,
				ThreadExceptionEventArgs t)
			{
				
				try
				{
					Trace.WriteLine("  Message :: " + t.Exception.Message+" Source ::: "+ t.Exception.Source + " Stack Trace ::: "+ t.Exception.StackTrace +" Exception Tostring ::: " + t.Exception.ToString());
					if(t.Exception.InnerException!=null)
					{
						Trace.WriteLine("  Message :: " + t.Exception.InnerException.Message+" Source ::: "+ t.Exception.InnerException.Source + " Stack Trace ::: "+ t.Exception.InnerException.StackTrace +" Exception Tostring ::: " + t.Exception.InnerException.ToString());
					}

					// Log the exception to a file
					LogException(t.Exception);

					//				// Tell the user that the app will restart
					//				MessageBox.Show("A Fatal Error was detected and logged.	Click the OK button to restart the  application" + t.Exception.Message + t.Exception.Source + t.Exception.StackTrace ,
					//																	  "Fatal Application Error",
					//																								   MessageBoxButtons.OK, MessageBoxIcon.Stop);
					if(MessageBox.Show("Invalid operation !! click OK to restart the application",	"CompassNav",MessageBoxButtons.OK, MessageBoxIcon.Stop)==DialogResult.OK)
					{
						// Shut down the current app instance
						//					System.Diagnostics.Process.Start(Application.ExecutablePath,parameters);
						//					ClientUI.getInstance().ClientUI_Closing(null,null);
						//					Application.Exit();
						//System.Environment.Exit(0);					
						//System.Threading.Thread.Sleep(1000);
				
				
						// Restart the app
					
					}
					// Shut down the current app instance
					//Application.Exit();

					// Restart the app
					// System.Diagnostics.Process.Start(Application.ExecutablePath);
				}
				catch(Exception ex)
				{
					Trace.WriteLine(ex.ToString());
				}

			}         

		// *Very* simple logging function to write exception details
		// to disk
		protected void LogException(string ss)
		{
			DateTime now    = System.DateTime.Now;
			string error    = ss;
			string filename = String.Format("Log-{0}{1}{2}-{3}{4}{5}.txt",  now.Year.ToString(), now.Month.ToString(),  now.Day.ToString(),now.Hour, now.Minute,now.Second);

			StreamWriter stream = null;
			try
			{
				stream = new StreamWriter(filename, false);
				stream.Write(error);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				if (null != stream)
					stream.Close();
			}
		}
		protected void LogException(Exception e)
		{
			DateTime now    = System.DateTime.Now;
			string error    = e.Message + "\n\nStack Trace:\n"
				+ e.StackTrace;
			string filename = String.Format("Log-{0}{1}{2}-{3}{4}{5}.txt",  now.Year.ToString(), now.Month.ToString(),  now.Day.ToString(),now.Hour, now.Minute,now.Second);

			StreamWriter stream = null;
			try
			{
				stream = new StreamWriter(@Application.StartupPath + "/log/"+filename, false);
				stream.Write(error);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				if (null != stream)
					stream.Close();
			}
		}
		//}
	}
}
