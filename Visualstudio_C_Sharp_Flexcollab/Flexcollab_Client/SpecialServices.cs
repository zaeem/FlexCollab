using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using WebMeeting.Client;
using WebMeeting.Client.Alerts;

namespace SpecialServices
{
	//SingleProgamInstance uses a mutex synchronization object
	// to ensure that only one copy of process is running at
	// a particular time.  It also allows for UI identification
	// of the intial process by bring that window to the foreground.
	public class SingleProgramInstance : IDisposable
	{

		bool boolShareDocThread;

		//private members 
		private Mutex _processSync;
		private bool _owned = false;
		
	
		public SingleProgramInstance()
		{	
			//Initialize a named mutex and attempt to
			// get ownership immediatly 
			_processSync = new Mutex(
				true, // desire intial ownership
				Assembly.GetExecutingAssembly().GetName().Name,
				out _owned);
		}

		public SingleProgramInstance(string identifier)
		{	
			//Initialize a named mutex and attempt to
			// get ownership immediately.
			//Use an addtional identifier to lower
			// our chances of another process creating
			// a mutex with the same name.
			_processSync = new Mutex(
				true, // desire intial ownership
				Assembly.GetExecutingAssembly().GetName().Name + identifier,
				out _owned);
		}

		/*private void deleteSharedDocuments()
		{
			try
			{				
				System.Diagnostics.Process newProcessToDeleteSharedDocuments = new System.Diagnostics.Process();
				newProcessToDeleteSharedDocuments.StartInfo = new System.Diagnostics.ProcessStartInfo(System.Windows.Forms.Application.StartupPath + "\\DeleteSharedDocuments.exe");
				newProcessToDeleteSharedDocuments.Start();
				boolShareDocThread = false;
			}
			catch(Exception ex)
			{
				System.Windows.Forms.MessageBox.Show("ClientUI.cs Class: deleteSharedDocuments() function  ,"+ ex.Message );
			}
		}*/

		~SingleProgramInstance()
		{
			//Release mutex (if necessary) 
			//This should have been accomplished using Dispose() 
			try
			{
				Release();
				System.IO.File.Create(System.Windows.Forms.Application.StartupPath + "\\flag.txt");
			}
			catch(Exception ex)
			{
				MeetingAlerts alert=new MeetingAlerts();
				alert.ShowMessage(WebMeeting.Client.Alerts.MeetingAlerts.AlertType.NonFatal,"Special Services Class : ~SingleProgramInstance() functioh" + ex.Message,true,false);
				//System.Windows.Forms.MessageBox.Show("Special Services Class : ~SingleProgramInstance() functioh" + ex.Message );
			}
		}

		public bool IsSingleInstance
		{
			//If we don't own the mutex than
			// we are not the first instance.
			get {return	_owned;}
		}

		public void RaiseOtherProcess()
		{
			Process proc = Process.GetCurrentProcess();
			// Using Process.ProcessName does not function properly when
			// the name exceeds 15 characters. Using the assembly name
			// takes care of this problem and is more accruate than other
			// work arounds.
			string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
			foreach (Process otherProc in Process.GetProcessesByName(assemblyName))
			{
				//ignore this process
				if (proc.Id != otherProc.Id)
				{
					// Found a "same named process".
					// Assume it is the one we want brought to the foreground.
					// Use the Win32 API to bring it to the foreground.
					IntPtr hWnd = otherProc.MainWindowHandle;
					if (Win32.USER32.IsIconic(hWnd))
					{
						Win32.USER32.ShowWindowAsync(hWnd,Win32.USER32.SW_RESTORE);
					}
					Win32.USER32.SetForegroundWindow(hWnd);
					return;
				}
			}
		}

		private void Release()
		{
			if (_owned)
			{
				//If we owne the mutex than release it so that
				// other "same" processes can now start.
				_owned = false;
				//_processSync.ReleaseMutex(); // commented by kamran bc we have to delete some files after execution
			}
		}

	#region Implementation of IDisposable
		public void Dispose()
		{
			//release mutex (if necessary) and notify 
			// the garbage collector to ignore the destructor
			Release();
			GC.SuppressFinalize(this);
		}
	#endregion
	}
}

	