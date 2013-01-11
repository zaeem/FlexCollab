using System;
using System.Runtime.InteropServices;
using TaskbarSorter;

namespace CommonOfProcessList
{

//-----------------------------------------------------------------------------
// Structures

	[StructLayout(LayoutKind.Sequential)]
	internal struct SYSTEM_INFO 
	{
		public _PROCESSOR_INFO_UNION uProcessorInfo;
		public uint dwPageSize;
		public uint lpMinimumApplicationAddress;
		public uint lpMaximumApplicationAddress;
		public uint dwActiveProcessorMask;
		public uint dwNumberOfProcessors;
		public uint dwProcessorType;
		public uint dwAllocationGranularity;
		public uint dwProcessorLevel;
		public uint dwProcessorRevision;
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct _PROCESSOR_INFO_UNION
	{
		[FieldOffset(0)]
		public uint dwOemId;
		[FieldOffset(0)]
		public ushort wProcessorArchitecture;
		[FieldOffset(2)]
		public ushort wReserved;
	}

	[ StructLayout( LayoutKind.Sequential )]
	internal struct BY_HANDLE_FILE_INFORMATION
	{
		public UInt32 dwFileAttributes;
		public FILETIME ftCreationTime;
		public FILETIME ftLastAccessTime;
		public FILETIME ftLastWriteTime;
		public UInt32 dwVolumeSerialNumber;
		public UInt32 nFileSizeHigh;
		public UInt32 nFileSizeLow;
		public UInt32 nNumberOfLinks;
		public UInt32 nFileIndexHigh;
		public UInt32 nFileIndexLow;
	}

	[ StructLayout( LayoutKind.Sequential )]
	internal class MEMORYSTATUSEX
	{
		public Int32 Length;
		public Int32 MemoryLoad;
		public UInt64 TotalPhysical;
		public UInt64 AvailablePhysical;
		public UInt64 TotalPageFile;
		public UInt64 AvailablePageFile;
		public UInt64 TotalVirtual;
		public UInt64 AvailableVirtual;
		public UInt64 AvailableExtendedVirtual;

		public MEMORYSTATUSEX() { Length = Marshal.SizeOf( this ); }

		private void StopTheCompilerComplaining()
		{
			Length = 0;
			MemoryLoad = 0;
			TotalPhysical = 0;
			AvailablePhysical = 0;
			TotalPageFile = 0;
			AvailablePageFile = 0;
			TotalVirtual = 0;
			AvailableVirtual = 0;
			AvailableExtendedVirtual = 0;
		}
	}

}
