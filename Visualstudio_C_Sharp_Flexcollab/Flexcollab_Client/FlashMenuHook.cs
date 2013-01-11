using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using WebMeeting.Client;
using System.Runtime.InteropServices;
using System.Text;
using WebMeeting.Client.Alerts;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for FlashMenuHook.
	/// </summary>
	public class FlashMenuHook
	{
		~FlashMenuHook()
		{
			bool ret = Win32.USER32.UnhookWindowsHookEx(hHook);
			//If UnhookWindowsHookEx fails.
			if(ret == false )
			{
				//MeetingAlerts alert=new MeetingAlerts();
				//alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Warning,"UnhookWindowsHookEx Failed",true,false);				
				return;
			}
			hHook = 0;			

		}
		public static int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			//Marshall the data from callback.
			Win32.USER32.MouseHookStruct MyMouseHookStruct = (Win32.USER32.MouseHookStruct) Marshal.PtrToStructure(lParam, typeof(Win32.USER32.MouseHookStruct));

			if (nCode < 0)
			{
				return Win32.USER32.CallNextHookEx(hHook, nCode, wParam, lParam);
			}
			else
			{
				if(nCode == Win32.USER32.HC_ACTION)
				{
					if(wParam.ToInt32() == Win32.USER32.WM_RBUTTONDOWN)
					{

						Int32 Res;
						Control ctrl = Control.FromChildHandle((System.IntPtr)MyMouseHookStruct.hwnd);
						StringBuilder ClassName = new StringBuilder(256);

						// Get the window class name
						Res = Win32.USER32.GetClassName(MyMouseHookStruct.hwnd , ClassName, ClassName.MaxCapacity);
						if(ClassName.ToString() == "MacromediaFlashPlayerActiveX")
						{
							//MessageBox.Show("Oay");
						
							return 1;
						}					
						return 0;

					}


				}
				/*
					//Create a string variable with shows current mouse. coordinates
					String strCaption = "x = " +
						MyMouseHookStruct.pt.x.ToString("d") +
						"  y = " +
						MyMouseHookStruct.pt.y.ToString("d");
					//Need to get the active form because it is a static function.
					Form tempForm = Form.ActiveForm;

					//Set the caption of the form.
					tempForm.Text = strCaption;
					*/
				return Win32.USER32.CallNextHookEx(hHook, nCode, wParam, lParam);
			}
		}
				
		public static System.IO.StreamWriter fs ;
		public static bool bCreated = false;
	
		public static int WindowHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if(!bCreated)
			{
				fs = new System.IO.StreamWriter("C:\\testStream.dat");
				bCreated = true;
			}
			//Marshall the data from callback.
			Win32.USER32.CWPSTRUCT MyCWPSTRUCT= (Win32.USER32.CWPSTRUCT) Marshal.PtrToStructure(lParam, typeof(Win32.USER32.CWPSTRUCT));

			if (nCode < 0)
			{
				return Win32.USER32.CallNextHookEx(hHook, nCode, wParam, lParam);
			}
			else
			{
				if(nCode == Win32.USER32.HC_ACTION)
				{						

					System.Int32 Res;
					Control ctrl = Control.FromChildHandle((System.IntPtr)MyCWPSTRUCT.hwnd);
					StringBuilder ClassName = new StringBuilder(256);
					Res = Win32.USER32.GetClassName(MyCWPSTRUCT.hwnd , ClassName, ClassName.MaxCapacity);
					if(ClassName.ToString() == "MacromediaFlashPlayerActiveX")
					{

						if((MyCWPSTRUCT.message != Win32.USER32.WM_PAINT))
							fs.WriteLine(MyCWPSTRUCT.message. ToString());

						if(MyCWPSTRUCT.message == Win32.USER32.WM_CREATE)
						{
					
							//MessageBox.Show("Window Creation");
						
							return 1;
						}
					}
					else
						return 1;


				}
				return Win32.USER32.CallNextHookEx(hHook, nCode, wParam, lParam);
			}
		}
				
		public void InstallMouseHook()
		{
			if(hHook == 0)
			{
				// Create an instance of HookProc.
				MouseHookProcedure = new Win32.USER32.HookProc(MouseHookProc);

				hHook = Win32.USER32.SetWindowsHookEx(Win32.USER32.WH_MOUSE,
					MouseHookProcedure,
					(IntPtr)0,
					AppDomain.GetCurrentThreadId());
				//If SetWindowsHookEx fails.
				if(hHook == 0 )
				{			
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Warning,"SetWindowsHookEx Failed",true,false);					
					//MessageBox.Show("SetWindowsHookEx Failed");
					return;
				}
				
			}
			else
			{
				bool ret = Win32.USER32.UnhookWindowsHookEx(hHook);
				//If UnhookWindowsHookEx fails.
				if(ret == false )
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Warning,"UnhookWindowsHookEx Failed",true,false);					
					//MessageBox.Show("UnhookWindowsHookEx Failed");
					return;
				}
				hHook = 0;
			}
		}


		public void InstallWindowsHook()
		{
			if(hHook == 0)
			{
				// Create an instance of HookProc.
				MouseHookProcedure = new Win32.USER32.HookProc(WindowHookProc);

				hHook = Win32.USER32.SetWindowsHookEx(Win32.USER32.WH_CALLWNDPROC,
					MouseHookProcedure,
					(IntPtr)0,
					AppDomain.GetCurrentThreadId());
				//If SetWindowsHookEx fails.
				if(hHook == 0 )
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Warning,"SetWindowsHookEx Failed",true,false);			
//					MessageBox.Show("SetWindowsHookEx Failed");
					return;
				}
			
			}
			else
			{
				bool ret = Win32.USER32.UnhookWindowsHookEx(hHook);
				//If UnhookWindowsHookEx fails.
				if(ret == false )
				{
					MeetingAlerts alert=new MeetingAlerts();
					alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Warning,"UnhookWindowsHookEx Failed",true,false);		
					//MessageBox.Show("UnhookWindowsHookEx Failed");
					return;
				}
				hHook = 0;
				
			}
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			InstallMouseHook();
			
		}

		public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

		//Declare hook handle as int.
		static int hHook = 0;

		//Declare mouse hook constant.
		//For other hook types, you can obtain these values from Winuser.h in Microsoft SDK.
		
		
		//Declare MouseHookProcedure as HookProc type.
		Win32.USER32.HookProc MouseHookProcedure;

		

		public FlashMenuHook()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
