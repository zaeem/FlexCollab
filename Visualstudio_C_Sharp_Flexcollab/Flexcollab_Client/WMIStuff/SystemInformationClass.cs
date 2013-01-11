/* Class used for getting System Information using WMI(Window Machine Instrumentation
 *
 * processor
 * System PhysicalRam
 * System Drive
 * Total Virtual Memory
 * Operating System
 * Operating System Packs
 * 
 * 
 * References::
 * Win32_OperatingSystem
 * http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wmisdk/wmi/win32_operatingsystem.asp
 * Win32_ComputerSystem
 * http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wmisdk/wmi/win32_computersystem.asp
 *
 * http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wmisdk/wmi/wmi_reference.asp?frame=true
 * http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wmisdk/wmi/wmi_classes.asp
 * http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wmisdk/wmi/win32_classes.asp
 * http://msdn.microsoft.com/library/default.asp?url=/library/en-us/wmisdk/wmi/computer_system_hardware_classes.asp
 * http://www.csharphelp.com/archives2/archive334.html  
 
 */

using System;
using System.Management;

namespace WebMeeting.Client.WMIStuff
{
	/// <summary>
	/// Summary description for SystemInformationClass.
	/// </summary>
	public class SystemInformationClass
	{
		ConnectionOptions oConn;
		System.Management.ManagementScope oMs;
		/*
		* processor
		* System PhysicalRam
		* System Drive		
		* Total Virtual Memory
		* Operating System
		* Operating System Packs
		*/
		
		/// <summary>
		/// User Defined structed system info
		/// </summary>
		public struct UDFStructSysInfo
		{
			/// <summary>
			/// System Processor Name
			/// </summary>
			public string strProcessorName;
			/// <summary>
			/// System Drive
			/// </summary>
			public string strSystemDrive;			
			/// <summary>
			/// Application Directory Path
			/// </summary>
			public string strDirectoryPath;			
			/// <summary>
			/// Operating System Name
			/// </summary>
			public string strOperatingSystem;
			/// <summary>
			/// Operating System Service Pack
			/// </summary>
			public string strOperatingSystemPack;			
			/// <summary>
			/// Total Physical Ram
			/// Using 
			/// </summary>
			public int nPhysicalRam;
			/// <summary>
			/// Total Virtual Memory
			/// </summary>
			public System.Int64 nTotalVirtualMemory;			
		}
		
		public UDFStructSysInfo sysinfo;


		public SystemInformationClass()
		{			
			//
			// TODO: Add constructor logic here
			//							

			//Connection credentials to the remote computer - not needed if the logged in account has access
			ConnectionOptions oConn = new ConnectionOptions();
			//oConn.Username = "username";
			//oConn.Password = "password";
			System.Management.ManagementScope oMs = new System.Management.ManagementScope("\\\\localhost", oConn); 
		}

		/// <summary>
		/// Extract System Information in the UDFStructSysInfo
		/// </summary>
		public void ExtractSysInformation()
		{									
			
			sysinfo=new UDFStructSysInfo();
			/*
			 * get information from Win32_OperatingSystem
			 * 
			 */
			
			// Get process info including a method to return the user who is running it
			System.Management.ObjectQuery oQuery = new System.Management.ObjectQuery("select Name,ServicePackMajorVersion,ServicePackMinorVersion,TotalVirtualMemorySize  from Win32_OperatingSystem");
			ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs,oQuery);
			ManagementObjectCollection oReturnCollection = oSearcher.Get();  
			/*
			Name
			ServicePackMajorVersion
			ServicePackMinorVersion
			TotalVirtualMemorySize
			*/
			foreach( ManagementObject oReturn in oReturnCollection ) 
			{
				if(oReturn["Name"]!=null)	
				{
					sysinfo.strOperatingSystem=oReturn["Name"].ToString();;				
				}

				if(oReturn["ServicePackMajorVersion"]!=null)
				{
					sysinfo.strOperatingSystemPack=oReturn["ServicePackMajorVersion"].ToString();
				}

				if(oReturn["ServicePackMinorVersion"]!=null)
				{
					sysinfo.strOperatingSystemPack=sysinfo.strOperatingSystemPack+"." + oReturn["ServicePackMinorVersion"].ToString();
				}
				if(oReturn["TotalVirtualMemorySize"]!=null)
				{
					/*
					 * Maximum number, in kilobytes, of memory that can be allocated to a process. For operating systems with no virtual memory, typically this value is equal to the total amount of physical memory minus the memory used by the BIOS and OS. For some operating systems, this value may be infinity, in which case 0 (zero) should be entered. In other cases, this value could be a constant, for example, 2G or 4G.
					 */
					sysinfo.nTotalVirtualMemory=(Convert.ToInt64(oReturn["TotalVirtualMemorySize"].ToString()))/1024;
				}
			}	
					
			oQuery = new System.Management.ObjectQuery("select *  from Win32_Processor");
			oSearcher = new ManagementObjectSearcher(oMs,oQuery);
			oReturnCollection = oSearcher.Get();  
			/*
			 * Descritption
			 */
			foreach( ManagementObject oReturn in oReturnCollection ) 
			{			
				if(oReturn["CurrentClockSpeed"]!=null)	
				{
					sysinfo.strProcessorName=oReturn["CurrentClockSpeed"].ToString();
				}
			}	

			oQuery = new System.Management.ObjectQuery("select Manufacturer,TotalPhysicalMemory,Model  from Win32_ComputerSystem");
			oSearcher = new ManagementObjectSearcher(oMs,oQuery);
			oReturnCollection = oSearcher.Get();  
			/*
			 * Descritption
			 */
			foreach( ManagementObject oReturn in oReturnCollection ) 
			{			
				if(oReturn["Manufacturer"]!=null)	
				{
					sysinfo.strProcessorName=sysinfo.strProcessorName + "-" + oReturn["Manufacturer"].ToString();				
				}
				if(oReturn["TotalPhysicalMemory"]!=null)	
				{
					/* Qualifiers: Units(Bytes)
					 * Total size of physical memory. Be aware that, under some circumstances, this property may not return an accurate value for the physical memory. For example, it is not accurate if the BIOS is using some of the physical memory.
					 * */
					System.Int64  TotalRam=Convert.ToInt64(oReturn["TotalPhysicalMemory"].ToString());
					sysinfo.nPhysicalRam=(int)((TotalRam/1024)/1024);					
				}
				if(oReturn["Model"]!=null)	
				{
					sysinfo.strProcessorName=sysinfo.strProcessorName + "-" + oReturn["Model"].ToString();				
				}
			}	

			

//			System.Environment.o
//			foreach( ManagementObject oReturn in oReturnCollection ) 
//			{
//				oReturn[Name]
//			}
		}

	}
}
