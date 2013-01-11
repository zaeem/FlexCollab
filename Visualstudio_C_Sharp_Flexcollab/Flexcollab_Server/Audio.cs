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
	/// Summary description for Audio.
	/// </summary>
	

	public class Audio
	{
		public ArrayList clientList;
		public WebMeeting.Common.AudioMessage currentFrame;
		private Dispatcher _dispatcher = new Dispatcher(typeof(IMessageReceiver));


		public Audio()
		{
			//
			// TODO: Add constructor logic here
			//
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
			//clientList.Add(iMessageReceiver);//m.Profile.ClientId,m);
		}
		public void RemoveClient(IMessageReceiver iMessageReceiver)
		{
			this._dispatcher.Remove((MarshalByRefObject) iMessageReceiver);
			//clientList.Remove(iMessageReceiver);//.Profile.ClientId);
		}
		public void SendMessage(ref WebMeeting.Common.AudioMessage m)
		{
			currentFrame=m;
			
			((IMessageReceiver) this._dispatcher.TransparentProxy).ReceiveAudioMessage(m);

			//foreach( IMessageReceiver iMessageReceiver in clientList)
		//	{
		//		iMessageReceiver.ReceiveAudioMessage(m);
		//	}
		}		
	}	
}

