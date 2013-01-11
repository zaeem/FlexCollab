using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for MouseEventCancel.
	
	public class MouseEventCancelHook
	{

		public IntPtr oldWndProc = IntPtr.Zero;
		private Win32.USER32.Win32WndProc newWndProc = null;
		
		public bool bAllowLeftClick = true;

		#region Hooking/Subclassing Functions
		public void StopSubclass(IntPtr hWnd)
		{
			Win32.USER32.SetWindowLong(hWnd, Win32.USER32.GWL_WNDPROC, oldWndProc);
		}		
	
	
		public void StartSubclass(IntPtr hWnd)
		{
			// delegate for the new wndproc
			newWndProc = new Win32.USER32.Win32WndProc(MyWndProc);
				
			if(oldWndProc.ToInt32() != 0)
				Win32.USER32.SetWindowLong(hWnd, Win32.USER32.GWL_WNDPROC, newWndProc);
			else
				oldWndProc = Win32.USER32.SetWindowLong(hWnd, Win32.USER32.GWL_WNDPROC, newWndProc);							
	
			
		}
		private int MyWndProc(IntPtr hWnd, int Msg, int wParam, int lParam)
		{
			try
			{
				try
				{
					switch(Msg)
					{					
						case Win32.USER32.WM_LBUTTONDOWN:
							if(!bAllowLeftClick)
								return 0;
							bAllowLeftClick =false;
							break;
						case Win32.USER32.WM_LBUTTONDBLCLK:
							if(!bAllowLeftClick)
								return 0;
							bAllowLeftClick =false;
							break;
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

		public MouseEventCancelHook()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
	
}
