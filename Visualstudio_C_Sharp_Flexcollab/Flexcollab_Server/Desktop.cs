using System;
using System.Collections;
using WebMeeting.Common;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels;
using Belikov.GenuineChannels;
using Belikov.GenuineChannels.BroadcastEngine;


namespace WebMeeting.Server
{
	/// <summary>
	/// Summary description for Video.
	/// </summary>
	public class Desktop
	{
		//public Hashtable clientList;
		public ArrayList clientList;
		private Dispatcher _dispatcher = new Dispatcher(typeof(IMessageReceiver));

		public int sessionID;


		
		public Desktop()
		{
			//
			// TODO: Add constructor logic here
			//
			//clientList=new ArrayList();
			clientList =new ArrayList();//Hashtable.Synchronized(new Hashtable());
			this._dispatcher.BroadcastCallFinishedHandler += new BroadcastCallFinishedHandler(this.BroadcastCallFinishedHandler);
			this._dispatcher.CallIsAsync = true;
			this._dispatcher.ReceiveResultsTimeOut=new TimeSpan (0,0,0,2,0);
			this._dispatcher.MaximumNumberOfConsecutiveFailsToEnableSimulationMode=0;
			
			
		}
		
		public void BroadcastCallFinishedHandler(Dispatcher dispatcher, IMessage
			message, ResultCollector resultCollector)
		{
			// analyze broadcast results
		}


		public void AddClient(IMessageReceiver iMessageReceiver)
		{
			this._dispatcher.Add((MarshalByRefObject) iMessageReceiver);
			
		}
		public void RemoveClient(IMessageReceiver iMessageReceiver)
		{
			this._dispatcher.Remove((MarshalByRefObject) iMessageReceiver);
			
		}

		public void SendMessage(ref WebMeeting.Common.AppSharingMessageEx m)
		{			
			((IMessageReceiver) this._dispatcher.TransparentProxy).ReceiveDesktopMessage(m);
		}		
	}
}
