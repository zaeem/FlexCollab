using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using SHDocVw;

namespace De.Fun.IESaveAsHack
{
	/// <summary>
	/// IE has this file options in the "SaveAs" dialog
	/// </summary>
	public enum SaveType
	{
		SAVETYPE_HTMLPAGE = 0,
		SAVETYPE_ARCHIVE,
		SAVETYPE_HTMLONLY,
		SAVETYPE_TXTONLY
	};

	/// <summary>
	/// Because security reasons the IE always shows a "Save As" dialog box if you want to save a html page complete.
	/// This helper class automatically fills out the fields and presses ok.
	/// </summary>
	public class SaveAsWebbrowser  
	{
		/// <summary>
		/// Construction
		/// </summary>
		/// <param name="webbrowser">Webbrowser interface</param>
		/// <param name="pathFile"><IMG src="/script/images/smiley_tongue.gif" align=absMiddle>ath and filename</param>
		/// <param name="saveType">Type of file</param>
		public SaveAsWebbrowser(IWebBrowser2 webbrowser, string pathFile, SaveType saveType)
		{
			this.WebBrowser = webbrowser;
			this.PathFile = pathFile;
			this.SaveType = saveType;
		}

		IWebBrowser2 webBrowser;
		public IWebBrowser2 WebBrowser
		{
			get { return webBrowser; }
			set { webBrowser = value; }
		}

		string pathFile;
		public string PathFile
		{
			get { return pathFile; }
			set { pathFile = value; }
		}

		SaveType saveType;
		public SaveType SaveType
		{
			get { return saveType; }
			set { saveType = value; }
		}

		/// <summary>
		/// Call this function to save current page - after page is loaded complete
		/// </summary>
		/// <returns>true if successful</returns>
		public bool SaveAs()
		{
			if (0==pathFile.Length)
				pathFile = "untitled";
			// TODO check path file. If file exists, IE prompts again...

			if (null==webBrowser)
				return false;
      
			if (0!=hook)
				return false;

			HookProcedure = new Win32.USER32.HookProc(SaveAsHookProc);

			// prepare SaveAs dialog hook
			hook = Win32.USER32.SetWindowsHookEx(5 /*WH_CBT*/, HookProcedure, (IntPtr) 0, AppDomain.GetCurrentThreadId());
			if (0==hook)
				return false;

			// this will show the dialog
			saveaswebbrowser = this;
			object o = null;
            string str="";
			o=(object)str;
			// remove hook






			//webBrowser.Navigate2("http://www.compassnav.com",)
			try
			{
				webBrowser.ExecWB(SHDocVw.OLECMDID.OLECMDID_SAVEAS,
					SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, ref o, ref o);
			}
			catch(Exception exp){exp.StackTrace.ToString();}



			Win32.USER32.UnhookWindowsHookEx(hook);
			
			saveaswebbrowser = null;
			hook = 0;
			return true;
		}
		static int hook = 0;
		Win32.USER32.HookProc HookProcedure;        

		static IntPtr hwndDlg = (IntPtr) 0;
		static SaveAsWebbrowser saveaswebbrowser = null;

		[StructLayout(LayoutKind.Sequential)]
			public struct CBT_CREATEWND 
		{ 
			public IntPtr   lpcs; 
			int             hwndInsertAfter; 
		}; 

		[StructLayout(LayoutKind.Sequential)]
			public struct CREATESTRUCT 
		{
			int             lpCreateParams;
			int             hInstance;
			int             hMenu;
			int             hwndParent;
			int             cy;
			public int      cx;
			int             y;
			public int      x;
			int             style;
			int             lpszName;
			public int      lpszClass;
			int             dwExStyle;
		} 

		/// <summary>
		/// The hook procedure for dialogs. Only called by windows.
		/// </summary>
		/// <param name="nCode">Action code</param>
		/// <param name="wParam">Depends on action code</param>
		/// <param name="lParam">Depends on action code</param>
		/// <returns></returns>
		public static int SaveAsHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			switch(nCode)
			{
				case 3:  // HCBT_CREATEWND
					CBT_CREATEWND cw = (CBT_CREATEWND) Marshal.PtrToStructure(lParam, typeof(CBT_CREATEWND));
					CREATESTRUCT cs = (CREATESTRUCT) Marshal.PtrToStructure(cw.lpcs, typeof(CREATESTRUCT));
					if (cs.lpszClass == 0x00008002 && (IntPtr)0 == hwndDlg)
					{
						hwndDlg = (IntPtr) wParam;  // Get hwnd of SaveAs dialog
						cs.x = -2 * cs.cx;          // Move dialog off screen
					}
					break;
				case 5: // HCBT_ACTIVATE
					ThreadPressOk tpok = new ThreadPressOk(hwndDlg, saveaswebbrowser.PathFile, saveaswebbrowser.SaveType);
					hwndDlg = (IntPtr) 0;
					// Create a thread to execute the task, and then
					// start the thread.
					new Thread((new ThreadStart(tpok.ThreadProc))).Start();
					break;
			}
			return Win32.USER32.CallNextHookEx(hook, nCode, wParam, lParam); 
		}

		/// <summary>
		/// Helper thread class.
		/// </summary>
		class ThreadPressOk 
		{
			public ThreadPressOk(IntPtr hwndDlg, string pathFile, SaveType saveType)
			{
				this.hwndDlg = hwndDlg;
				this.pathFile = pathFile;
				this.saveType = saveType;
			}
      
			IntPtr    hwndDlg;
			string    pathFile;
			SaveType  saveType;

		
			// The thread procedure performs the message loop and place the data
			public void ThreadProc() 
			{        
				// Create the dialog
				Application.DoEvents();
    
				//  Begin saving the webpage
				//  Note: these settings only work on win2k IE 6 SP1
				//  You can make them work the same way on other versions

				// editbox : filepath (control id = 0x047c)
				// dropdown combo : filetypes (options=complete page|archive|html only|txt) (control id = 0x0470)
				// save button : control id = 0x0001
				// cancel button : control id = 0x0002

				IntPtr typeB = Win32.USER32.GetDlgItem(hwndDlg, 0x0470);
				IntPtr nameB = Win32.USER32.GetDlgItem(hwndDlg, 0x047c);
				IntPtr saveBtn = Win32.USER32.GetDlgItem(hwndDlg, 0x0001);
				if (((IntPtr)0!=typeB) && ((IntPtr)0!=nameB) && ((IntPtr)0!=saveBtn))
				{
					//select save type
					Win32.USER32.SendMessage(typeB, 0x014E /*CB_SETCURSEL*/, (int)saveType, 0);
					Win32.USER32.SendMessage(hwndDlg, 0x0111 /*WM_COMMAND*/, 0x80470/*MAKEWPARAM(0x0470, CBN_CLOSEUP)*/, (int)typeB);
					// set output pathFile
					Win32.USER32.SetWindowText(nameB, pathFile);
					// Invoke Save button
					Win32.USER32.SendMessage(saveBtn, 0x00F5 /*BM_CLICK*/, 0, 0);
				}
				// Clean up GUI - we have clicked ok
				Application.DoEvents();

				// thread finish, garbage collection should do the clean up...
			}
		}
	}
}
