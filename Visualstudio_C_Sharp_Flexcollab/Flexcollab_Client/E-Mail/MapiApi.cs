/******************************************************
                   Simple MAPI.NET
		      netmaster@swissonline.ch
*******************************************************/

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


namespace Win32Mapi
{


	public class Mapi
	{


		#region SESSION
		// ----------------------------------------------------------- SESSION ---------

		public bool Logon( IntPtr hwnd )
		{
			winhandle = hwnd;
			error = Win32.MAPI32.MAPILogon( hwnd, null, null, 0, 0, ref session );
			if( error != 0 )
				error = Win32.MAPI32.MAPILogon( hwnd, null, null, Win32.MAPI32.MapiLogonUI, 0, ref session );
			return error == 0;
		}

		public void Reset()
		{
			findseed = null;
			origin = new Win32.MapiRecipDesc();
			recpts.Clear();
			attachs.Clear();
			lastMsg = null;
		}

		public void Logoff()
		{
			if( session != IntPtr.Zero )
			{
				error = Win32.MAPI32.MAPILogoff( session, winhandle, 0, 0 );
				session = IntPtr.Zero;
			}
		}


		

		private IntPtr session		= IntPtr.Zero;
		private IntPtr winhandle	= IntPtr.Zero;
		#endregion






		private StringBuilder	lastMsgID = new StringBuilder( 600 );
		private string			findseed  = null;


		#region SENDING
		// ----------------------------------------------------------- SENDING ---------

		public bool Send( string sub, string txt )
		{
			lastMsg				= new Win32.MapiMessage();
			lastMsg.subject		= sub;
			lastMsg.noteText	= txt;

			// set pointers
			lastMsg.originator	= AllocOrigin();
			lastMsg.recips		= AllocRecips(  out lastMsg.recipCount );
			lastMsg.files		= AllocAttachs( out lastMsg.fileCount  );

			error = Win32.MAPI32.MAPISendMail( session, winhandle, lastMsg, 0, 0 );
			Dealloc();
			Reset();
			return error == 0;
		}

		public void AddRecip( string name, string addr, bool cc )
		{
			Win32.MapiRecipDesc dest = new Win32.MapiRecipDesc();
			if( cc )
				dest.recipClass = Win32.MAPI32.MapiCC;
			else
				dest.recipClass = Win32.MAPI32.MapiTO;
			dest.name = name;
			dest.address = addr;
			recpts.Add( dest );
		}

		public void SetSender( string sname, string saddr )
		{
			origin.name		= sname;
			origin.address	= saddr;
		}

		public void Attach( string filepath )
		{
			attachs.Add( filepath );
		}

		private IntPtr AllocOrigin()
		{
			origin.recipClass = Win32.MAPI32.MapiORIG;
			Type rtype = typeof(Win32.MapiRecipDesc);
			int rsize = Marshal.SizeOf( rtype );
			IntPtr ptro = Marshal.AllocHGlobal( rsize );
			Marshal.StructureToPtr( origin, ptro, false );
			return ptro;
		}

		private IntPtr AllocRecips( out int recipCount )
		{
			recipCount = 0;
			if( recpts.Count == 0 )
				return IntPtr.Zero;

			Type rtype = typeof(Win32.MapiRecipDesc);
			int rsize = Marshal.SizeOf( rtype );
			IntPtr ptrr = Marshal.AllocHGlobal( recpts.Count * rsize );

			int runptr = (int) ptrr;
			for( int i = 0; i < recpts.Count; i++ )
			{
				Marshal.StructureToPtr( recpts[i] as Win32.MapiRecipDesc, (IntPtr) runptr, false );
				runptr += rsize;
			}

			recipCount = recpts.Count;
			return ptrr;
		}
		
		private IntPtr AllocAttachs( out int fileCount )
		{
			fileCount = 0;
			if( attachs == null )
				return IntPtr.Zero;
			if( (attachs.Count <= 0) || (attachs.Count > 100) )
				return IntPtr.Zero;

			Type atype = typeof(Win32.MapiFileDesc);
			int asize = Marshal.SizeOf( atype );
			IntPtr ptra = Marshal.AllocHGlobal( attachs.Count * asize );

			Win32.MapiFileDesc mfd = new Win32.MapiFileDesc();
			mfd.position = -1;
			int runptr = (int) ptra;
			for( int i = 0; i < attachs.Count; i++ )
			{
				string path = attachs[i] as string;
				mfd.name = Path.GetFileName( path );
				mfd.path = path;
				Marshal.StructureToPtr( mfd, (IntPtr) runptr, false );
				runptr += asize;
			}

			fileCount = attachs.Count;
			return ptra;
		}

		private void Dealloc()
		{
			Type rtype = typeof(Win32.MapiRecipDesc);
			int rsize = Marshal.SizeOf( rtype );

			if( lastMsg.originator != IntPtr.Zero )
			{
				Marshal.DestroyStructure( lastMsg.originator, rtype );
				Marshal.FreeHGlobal( lastMsg.originator );
			}

			if( lastMsg.recips != IntPtr.Zero )
			{
				int runptr = (int) lastMsg.recips;
				for( int i = 0; i < lastMsg.recipCount; i++ )
				{
					Marshal.DestroyStructure( (IntPtr) runptr, rtype );
					runptr += rsize;
				}
				Marshal.FreeHGlobal( lastMsg.recips );
			}

			if( lastMsg.files != IntPtr.Zero )
			{
				Type ftype = typeof(Win32.MapiFileDesc);
				int fsize = Marshal.SizeOf( ftype );

				int runptr = (int) lastMsg.files;
				for( int i = 0; i < lastMsg.fileCount; i++ )
				{
					Marshal.DestroyStructure( (IntPtr) runptr, ftype );
					runptr += fsize;
				}
				Marshal.FreeHGlobal( lastMsg.files );
			}
		}

		

		private Win32.MapiRecipDesc	origin	= new Win32.MapiRecipDesc();
		private ArrayList		recpts	= new ArrayList();
		private ArrayList		attachs = new ArrayList();
		#endregion



		#region FINDING
		// ----------------------------------------------------------- FINDING ---------

		public bool Next( ref MailEnvelop env )
		{
			error = Win32.MAPI32.MAPIFindNext(	session, winhandle, null, findseed,
				Win32.MAPI32.MapiLongMsgID, 0, lastMsgID );
			if( error != 0 )
				return false;
			findseed = lastMsgID.ToString();

			IntPtr ptrmsg = IntPtr.Zero;
			error = Win32.MAPI32.MAPIReadMail( session, winhandle, findseed,
				Win32.MAPI32.MapiEnvOnly | Win32.MAPI32.MapiPeek | Win32.MAPI32.MapiSuprAttach, 0, ref ptrmsg );
			if( (error != 0) || (ptrmsg == IntPtr.Zero) )
				return false;

			lastMsg = new Win32.MapiMessage();
			Marshal.PtrToStructure( ptrmsg, lastMsg );
			Win32.MapiRecipDesc orig = new Win32.MapiRecipDesc();
			if( lastMsg.originator != IntPtr.Zero )
				Marshal.PtrToStructure( lastMsg.originator, orig );

			env.id		= findseed;
			env.date	= DateTime.ParseExact( lastMsg.dateReceived, "yyyy/MM/dd HH:mm", DateTimeFormatInfo.InvariantInfo );
			env.subject	= lastMsg.subject;
			env.from	= orig.name;
			env.unread	= (lastMsg.flags & Win32.MAPI32.MapiUnread) != 0;
			env.atts	= lastMsg.fileCount;

			error = Win32.MAPI32.MAPIFreeBuffer( ptrmsg );
			return error == 0;
		}


		
		#endregion




		#region READING
		// ----------------------------------------------------------- READING ---------

		public string Read( string id, out MailAttach[] aat )
		{
			aat = null;
			IntPtr ptrmsg = IntPtr.Zero;
			error = Win32.MAPI32.MAPIReadMail( session, winhandle, id,
				Win32.MAPI32.MapiPeek | Win32.MAPI32.MapiSuprAttach, 0, ref ptrmsg );
			if( (error != 0) || (ptrmsg == IntPtr.Zero) )
				return null;

			lastMsg = new Win32.MapiMessage();
			Marshal.PtrToStructure( ptrmsg, lastMsg );

			if( (lastMsg.fileCount > 0) && (lastMsg.fileCount < 100) && (lastMsg.files != IntPtr.Zero) )
				GetAttachNames( out aat );

			Win32.MAPI32.MAPIFreeBuffer( ptrmsg );
			return lastMsg.noteText;
		}

		public bool Delete( string id )
		{
			error = Win32.MAPI32.MAPIDeleteMail( session, winhandle, id, 0, 0 );
			return error == 0;
		}

		public bool SaveAttachm( string id, string name, string savepath )
		{
			IntPtr ptrmsg = IntPtr.Zero;
			error = Win32.MAPI32.MAPIReadMail( session, winhandle, id,
				Win32.MAPI32.MapiPeek, 0, ref ptrmsg );
			if( (error != 0) || (ptrmsg == IntPtr.Zero) )
				return false;

			lastMsg = new Win32.MapiMessage();
			Marshal.PtrToStructure( ptrmsg, lastMsg );
			bool f = false;
			if( (lastMsg.fileCount > 0) && (lastMsg.fileCount < 100) && (lastMsg.files != IntPtr.Zero) )
				f = SaveAttachByName( name, savepath );
			Win32.MAPI32.MAPIFreeBuffer( ptrmsg );
			return f;
		}


		private void GetAttachNames( out MailAttach[] aat )
		{
			aat = new MailAttach[ lastMsg.fileCount ];
			Type fdtype = typeof(Win32.MapiFileDesc);
			int fdsize = Marshal.SizeOf( fdtype );
			Win32.MapiFileDesc fdtmp = new Win32.MapiFileDesc();
			int runptr = (int) lastMsg.files;
			for( int i = 0; i < lastMsg.fileCount; i++ )
			{
				Marshal.PtrToStructure( (IntPtr) runptr, fdtmp );
				runptr += fdsize;
				aat[i] = new MailAttach();
				if( fdtmp.flags == 0 )
				{
					aat[i].position = fdtmp.position;
					aat[i].name		= fdtmp.name;
					aat[i].path		= fdtmp.path;
				}
			}
		}

		
		private bool SaveAttachByName( string name, string savepath )
		{
			bool f = true;
			Type fdtype = typeof(Win32.MapiFileDesc);
			int fdsize = Marshal.SizeOf( fdtype );
			Win32.MapiFileDesc fdtmp = new Win32.MapiFileDesc();
			int runptr = (int) lastMsg.files;
			for( int i = 0; i < lastMsg.fileCount; i++ )
			{
				Marshal.PtrToStructure( (IntPtr) runptr, fdtmp );
				runptr += fdsize;
				if( fdtmp.flags != 0 )
					continue;
				if( fdtmp.name == null )
					continue;

				try 
				{
					if( name == fdtmp.name )
					{
						if( File.Exists( savepath ) )
							File.Delete( savepath );
						File.Move( fdtmp.path, savepath );
					}
				}
				catch( Exception )
				{ f = false; error = 13; }

				try 
				{
					File.Delete( fdtmp.path );
				}
				catch( Exception )
				{}
			}
			return f;
		}


		
		

		private Win32.MapiMessage lastMsg = null;
		#endregion




		#region ADDRESS

		public bool SingleAddress( string label, out string name, out string addr )
		{
			name = null;
			addr = null;
			int newrec = 0;
			IntPtr ptrnew = IntPtr.Zero;
			error = MAPIAddress(	session, winhandle, null, 1, label, 0, IntPtr.Zero,
				0, 0, ref newrec, ref ptrnew );
			if( (error != 0) || (newrec < 1) || (ptrnew == IntPtr.Zero) )
				return false;

			Win32.MapiRecipDesc recip = new Win32.MapiRecipDesc();
			Marshal.PtrToStructure( ptrnew, recip );
			name = recip.name;
			addr = recip.address;

			Win32.MAPI32.MAPIFreeBuffer( ptrnew );
			return true;
		}


		[DllImport( "MAPI32.DLL", CharSet=CharSet.Ansi)]
		private static extern int MAPIAddress( IntPtr sess, IntPtr hwnd, string caption,
			int editfld, string labels, int recipcount, IntPtr ptrrecips,
			int flg, int rsv, ref int newrec, ref IntPtr ptrnew );
		#endregion




		#region ERRORS
		// ----------------------------------------------------------- ERRORS ---------

		public string Error()
		{
			if( error <= 26 )
				return errors[ error ];
			return "?unknown? [" + error.ToString() + "]";
		}

		private int error = 0;

		private readonly string[] errors	= new string[] {
															   "OK [0]", "User abort [1]", "General MAPI failure [2]", "MAPI login failure [3]",
															   "Disk full [4]", "Insufficient memory [5]", "Access denied [6]", "-unknown- [7]",
															   "Too many sessions [8]", "Too many files were specified [9]", "Too many recipients were specified [10]", "A specified attachment was not found [11]",
															   "Attachment open failure [12]", "Attachment write failure [13]", "Unknown recipient [14]", "Bad recipient type [15]",
															   "No messages [16]", "Invalid message [17]", "Text too large [18]", "Invalid session [19]",
															   "Type not supported [20]", "A recipient was specified ambiguously [21]", "Message in use [22]", "Network failure [23]",
															   "Invalid edit fields [24]", "Invalid recipients [25]", "Not supported [26]" 
														   };
		#endregion

	}









	public class MailEnvelop
	{
		public string	id;
		public DateTime	date;
		public string	from;
		public string	subject;
		public bool		unread;
		public int		atts;
	}

	public class MailAttach
	{
		public int		position;
		public string	path;
		public string	name;
	}

}
