using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;





namespace WebMeeting.Client.Resolution
{
	/// <summary>
	/// Summary description for Resolution.
	/// </summary>
	class CResolution
	{
		public CResolution(int a,int b)
		{
			Screen screen = Screen.PrimaryScreen;
			
			
			int iWidth =a;
			int iHeight =b;
			

			Win32.USER32.DEVMODE1 dm = new Win32.USER32.DEVMODE1();
			dm.dmDeviceName = new String (new char[32]);
			dm.dmFormName = new String (new char[32]);
			dm.dmSize = (short)Marshal.SizeOf (dm);

			if (0 != Win32.USER32.EnumDisplaySettings (null, Win32.USER32.ENUM_CURRENT_SETTINGS, ref dm))
			{
				
				dm.dmPelsWidth = iWidth;
				dm.dmPelsHeight = iHeight;

				int iRet = Win32.USER32.ChangeDisplaySettings (ref dm,Win32.USER32.CDS_TEST);

				if (iRet == Win32.USER32.DISP_CHANGE_FAILED)
				{
					MessageBox.Show("Unable to process your request");
					MessageBox.Show("Description: Unable To Process Your Request. Sorry For This Inconvenience.","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
				}
				else
				{
					iRet = Win32.USER32.ChangeDisplaySettings (ref dm, Win32.USER32.CDS_UPDATEREGISTRY);
					//iRet = User_32.ChangeDisplaySettings (ref dm, User_32.CDS_FULLSCREEN);
					//CDS_RESET = 0x40000000
					//CDS_NORESET = 0x10000000; 
					//CDS_GLOBAL = 0x00000008;
					//iRet = User_32.ChangeDisplaySettings (ref dm,User_32.CDS_FULLSCREEN);

					switch (iRet) 
					{
						case Win32.USER32.DISP_CHANGE_SUCCESSFUL:
						{
							break;

							//successfull change
						}
						case Win32.USER32.DISP_CHANGE_RESTART:
						{
							
							MessageBox.Show("Description: You Need To Reboot For The Change To Happen.\n If You Feel Any Problem After Rebooting Your Machine\nThen Try To Change Resolution In Safe Mode.","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
							break;
							//windows 9x series you have to restart
						}
						default:
						{
							
							MessageBox.Show("Description: Failed To Change The Resolution.","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
							break;
							//failed to change
						}
					}
				}
				
			}
		}
	}
}
