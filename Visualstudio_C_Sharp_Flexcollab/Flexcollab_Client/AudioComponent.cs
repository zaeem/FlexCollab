using System;
using System.Runtime.InteropServices;

namespace WebMeeting
{
	/// <summary>
	/// Summary description for AudioComponent.
	/// </summary>
	/// 
	
	public class AudDllFuncs
	{
		[DllImport("AudDLL.dll")]
		public static extern void InitEncoder();

		[DllImport("AudDLL.dll")]
		public static extern void InitDecoder();

		[DllImport("AudDLL.dll")]
		public static extern int GetEncodedBuffer(byte [] compBuff, byte [] unComBuff);

		[DllImport("AudDLL.dll")]
		public static extern int GetDecodedBuffer(byte [] compBuff, byte [] unComBuff);


	}
}
