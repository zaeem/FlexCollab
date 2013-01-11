using System;
using System.Threading;
using WebMeeting.Common;
using WebMeeting.Client;

namespace WebMeeting
{
	/// <summary>
	/// Summary description for ThreadPoolUnit.
	/// </summary>
	public class ThreadPoolUnit
	{
		public StateUpdateMessage msg;
		public bool isDesktopSharing = false;
		public bool bTaken = false;
		public int nSessionID;
		

		public ThreadPoolUnit()
		{
		}

		public void Prepare(int senderID, object hash, bool isDesktopSharing, int nSID)
		{
			bTaken = true;
			msg = new StateUpdateMessage();
			msg.updateType = UpdateTypeCode.UpdateApplicationSharingCompund;
			msg.SenderID = senderID;
			msg.dataArray.Add(hash);
			nSessionID = nSID;			
		}

		public void SendingFunction()
		{
			if(isDesktopSharing)
				NetworkManager.thisInstance.SendAppShareLoadPacket(ref msg,nSessionID);
			else
				NetworkManager.thisInstance.SendAppShareLoadPacket(ref msg,-1);

			bTaken = false;
		}
	}
}
