using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WebMeeting.Client.Resolution
{
	/// <summary>
	/// Summary description for ResolutionController.
	/// </summary>
	/// 
		
	public class ResolutionController
	{
		[DllImport("IconOutliner.dll")]
		public static extern void RestoreIcons();

		[DllImport("IconOutliner.dll")]
		public static extern void SaveIcons();
	



		private int FixHeight=768,FixWidth=1024;
		public int tempHeight=0,tempWidth=0;

		public static ResolutionController thisInstance=new ResolutionController();     
		# region public static ResolutionController getInstance()

		public static ResolutionController getInstance()
		{
			
			return thisInstance;
		}
		# endregion 
	

		public ResolutionController()
		{

			Screen Srn=Screen.PrimaryScreen;
			tempHeight=Srn.Bounds.Height;
			tempWidth=Srn.Bounds.Width;
			
			//
			// TODO: Add constructor logic here
			//
		}


		public void ChangeResolution()
		{
			Resolution.CResolution ChangeRes=new Resolution.CResolution(FixWidth,FixHeight);
		}

		public void RetainResolution()
		{
			//Resolution.CResolution ChangeRes=new Resolution.CResolution(tempHeight,tempWidth);
			Resolution.CResolution ChangeRes=new Resolution.CResolution(tempWidth,tempHeight);
		}


		public void RetainResolution(int width,int height)
		{
			//Resolution.CResolution ChangeRes=new Resolution.CResolution(tempHeight,tempWidth);
			Resolution.CResolution ChangeRes=new Resolution.CResolution(width,height);
		}


		public void CRestoreIcons()
		{
		RestoreIcons();
		}

	
		public void CSaveIcons()
		{
			SaveIcons();
		}









	}
}
