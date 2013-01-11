/*************************************************************************
OAKListView 1.0
10/02/2004
Developer: Carlos Carvalho
carvalho@flag.com.br
+55 31 99440862 / +55 31 32616977
**************************************************************************/

using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WebMeeting;
namespace WebMeeting.OAKControls
{
	/// <summary>
	/// Summary description for OAKListVsiew.
	/// </summary>
	public class OAKListView : ListView
	{
		[StructLayoutAttribute(LayoutKind.Sequential)]
			public struct LV_ITEM
		{
			public UInt32 mask;
			public Int32 iItem;
			public Int32 iSubItem;
			public UInt32 state;
			public UInt32 stateMask;
			public String pszText;
			public Int32 cchTextMax;
			public Int32 iImage;
			public IntPtr lParam;
		}
		

		
		/// <summary>
		/// Changing the style of listview to accept image on subitems
		/// </summary>7
		/// 
		public void SetImage(int row,int column,int imageIndex)
		{
			Win32.USER32.LV_ITEM lvi = new Win32.USER32.LV_ITEM();
			// Row
			lvi.iItem = row; 
			// Column
			lvi.iSubItem = column;
			//lvi.pszText = "";//"OAKListView-" + I.ToString();
			lvi.mask = Win32.USER32.LVIF_IMAGE ;//| OAKListView.LVIF_TEXT; 
			// Image index on imagelist
			lvi.iImage = imageIndex;
			Win32.USER32.SendMessage(this.Handle, Win32.USER32.LVM_SETITEM, 0, ref lvi);

		}
		public OAKListView()
		{
			// Change the style of listview to accept image on subitems
			System.Windows.Forms.Message m = new Message();
			m.HWnd = this.Handle;
			m.Msg = Win32.USER32.LVM_GETEXTENDEDLISTVIEWSTYLE;
			m.LParam = (IntPtr)(Win32.USER32.LVS_EX_GRIDLINES | 
				Win32.USER32.LVS_EX_FULLROWSELECT | 
				Win32.USER32.LVS_EX_SUBITEMIMAGES | 
				Win32.USER32.LVS_EX_CHECKBOXES | 
				Win32.USER32.LVS_EX_TRACKSELECT);
			m.WParam = IntPtr.Zero;
			this.WndProc(ref m);
		}

	
		private void InitializeComponent()
		{
		
		} 

	}
}
