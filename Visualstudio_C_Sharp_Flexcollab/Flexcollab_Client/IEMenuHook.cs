using System;
using System.Runtime.InteropServices;
using mshtml;
using System.Text;
namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for IEMenuHook.
	/// </summary>
	public class IEMenuHook
	{
		#region PINVOKE Code
        
		public IntPtr oldWndProc = IntPtr.Zero;
		private Win32.USER32.Win32WndProc newWndProc = null;
		private IntPtr GhWnd;
		#endregion
	
		#region Hooking/Subclassing Functions
		public void StopSubclass(IntPtr hWnd)
		{
			Win32.USER32.SetWindowLong(hWnd, Win32.USER32.GWL_WNDPROC, oldWndProc);
		}		
	
		
		public IntPtr IEFromhWnd(IntPtr hWnd)
		{
			if(!hWnd.Equals(0))
			{
				if(!IsIEServerWindow(hWnd))
				{
					// Get 1st child IE server window
					Win32.USER32.EnumChildProc myEnumChildProc = new Win32.USER32.EnumChildProc(EnumChild);
					Win32.USER32.EnumChildWindows(hWnd.ToInt32(), myEnumChildProc, hWnd.ToInt32());
				}
			}

			return GhWnd;

		}
		
		private Int32 EnumChild(IntPtr hWnd, IntPtr lParam)
		{
			if(IsIEServerWindow(hWnd))
			{
				GhWnd = hWnd;
				return 0;
			}
			else
				return 1;

		}

		private bool IsIEServerWindow(IntPtr hWnd)
		{
			Int32 Res;
			StringBuilder ClassName = new StringBuilder(256);

			// Get the window class name
			Res = Win32.USER32.GetClassName(hWnd.ToInt32(), ClassName, ClassName.MaxCapacity);
			if(ClassName.ToString() == "Internet Explorer_Server")
				return true;
			else
				return false;
		}

		public AxSHDocVw.AxWebBrowser browserControl;
	
		public void StartSubclass(IntPtr hWnd,AxSHDocVw.AxWebBrowser browser)
		{
			// delegate for the new wndproc
			newWndProc = new Win32.USER32.Win32WndProc(MyWndProc);
				
			if(oldWndProc.ToInt32() != 0)
				Win32.USER32.SetWindowLong(hWnd, Win32.USER32.GWL_WNDPROC, newWndProc);
			else
				oldWndProc = Win32.USER32.SetWindowLong(hWnd, Win32.USER32.GWL_WNDPROC, newWndProc);
				
			if(browser!= null)
			{
				browserControl = browser;		
			}
	
			
		}
		private int MyWndProc(IntPtr hWnd, int Msg, int wParam, int lParam)
		{
			try
			{
				try
				{
					switch(Msg)
					{					
						case Win32.USER32.WM_RBUTTONDOWN:
							return 0;
						case Win32.USER32.WM_RBUTTONDBLCLK:
							return 0;
						default:							
							break;
					}
				}
				catch(Exception ee)
				{
					ee = ee;
					
				}				
			}
			catch(Exception)// ee)
			{
				
				
			}
			return Win32.USER32.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
		}

		#endregion

		public IEMenuHook()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
