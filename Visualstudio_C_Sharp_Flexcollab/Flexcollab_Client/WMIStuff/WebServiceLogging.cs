/*Class used for logging System,Joining & Disconnection informations using online
 * servic at http://www.compassnav.com/LoggingService/LogService.asmx
 * This service acting as a gateway to run queries at online server.
 * It has one method with named "LogRecord".We only pass 
 * 
 * 
 * This class method used on certain events.Following are the details of those events
 * Event 1-In start of application when connection is established to the server.
 * Event 2-On Connection Re-establishing
 * Event 3-On Closing of application.
 * 
 * Information each events will log
 * Event 1 - First Connection Time,CLR Version,System Aspects(RAM & OS & Service Packs),Application version & Application Name
 * Event 2 - Log how many time a client is disconnected.
 * Event 3 - Log total time of the meeting means Difference of Start & End Time including re-establishing. 
 * 
 * 
 * Event 1 can be used in other applications too becuase its application independent.
 * 
 * 
 * 
 * */
using System;
using System.Windows.Forms;

namespace WebMeeting.Client.WMIStuff
{
	/// <summary>
	/// Summary description for WebServiceLogging.
	/// </summary>
	public class WebServiceLogging
	{
		LogServiceOnline.LogService WSlogService;
		public int RecordID;
		int nDisconnection=0;
		int startTickCount;
		int endTickCount;
		public WebServiceLogging()
		{
			WSlogService=new LogServiceOnline.LogService();
			
		}

		public bool UpdateSystemAspectsandMeetingStartTime(int firstConnectionTime)
		{
			try
			{
			

				WMIStuff.SystemInformationClass sysinfo=new WMIStuff.SystemInformationClass();
				/*load System Information*/
				sysinfo.ExtractSysInformation();			
				string strQuery="Update ctool_applicationlogging set ";
				
				startTickCount=System.Environment.TickCount;

				string strQueryPostFix=" where id=" + RecordID.ToString()+";";
				/*
				 * `applog_processor` text COMMENT 'this filed contain processor name of client sytem',
					`applog_ram` int(1) default NULL COMMENT 'this field contain the ram of client machine',
					`applog_ipaddress` text COMMENT 'this field contain the ip address of the client',
					`applog_systemdrive` text COMMENT 'this field contain the System drive of the client',
					`applog_appinstallationpath` text COMMENT 'this field contains the installation path',
					`applog_disconnection` int(4) default '0' COMMENT 'this field tell about client disconnection in a meeting',
					`applog_virtualmemory` text COMMENT 'this field contain the total virtual memory of the system',
					`applog_clr` text COMMENT 'this field contain the .Net framwork version',
					`applog_os` text COMMENT 'this field contain the operating system of client',
					`applog_servicepack` text COMMENT 'this field contains the service pack of client machine.',
					`applog_meetstarttime` int(4) default NULL COMMENT 'this filed contains the meeting start time',
					`applog_meetendtime` int(4) default NULL COMMENT 'this field contains the meeting end time',
					`applog_applicationversion` text COMMENT 'this filed contains the application version',
					`applog_companyid` int(4) default NULL COMMENT 'this field contains the company id',
					`applog_browseruseragent` text COMMENT 'this field contains the browser agent of client',
					`applog_firstconnectiontime` int(4) default NULL COMMENT 'this field contain the first connection time in milliseconds',
					`applog_otherdata` text COMMENT 'this field contains the other data like Antivirus, firewall',
					`applog_applicationName` text COMMENT 'this field is contain app name like webmeeting..for future',
					`meetingid` int(4) default NULL,
					`joiningDate`
				 * */
				
				strQuery+=" applog_processor='" + sysinfo.sysinfo.strProcessorName+"',";
				strQuery+="applog_ram=" + sysinfo.sysinfo.nPhysicalRam.ToString()+ ",";
				strQuery+="applog_appinstallationpath='" + Application.StartupPath + "',";
				strQuery+="applog_virtualmemory=" + sysinfo.sysinfo.nTotalVirtualMemory+ ",";
				strQuery+="applog_clr='"+System.Environment.Version.ToString()+"',";
				strQuery+="applog_os='"+sysinfo.sysinfo.strOperatingSystem+"',";
				strQuery+="applog_servicepack='"+sysinfo.sysinfo.strOperatingSystemPack+"',";
				strQuery+="applog_applicationversion='"+Application.ProductVersion+"',";
				strQuery+="applog_firstconnectiontime="+firstConnectionTime.ToString()+",";
				strQuery+="applog_applicationName='Webmeeting',";				
				strQuery+="applog_meetstarttime="+Convert.ToString((this.startTickCount/1000)/60);
				strQuery=strQuery+strQueryPostFix;				
				if(WSlogService.LogRecord(strQuery)=="1")
				{				
					return true;
				}
			}
			catch(Exception ex)
			{
				Client.ClientUI.getInstance().ExceptionLog(" Stack Trace :: " + ex.StackTrace + " Source :: "+ex.Source + " Message::: "+ex.Message);
			}
			return false;
		}

		public bool  UpdateDisconnectionStuff()
		{
			try
			{
				nDisconnection+=1;
				string strQuery="Update ctool_applicationlogging set ";
				string strQueryPostFix=" where id=" + RecordID.ToString()+";";
				strQuery+="applog_disconnection="+nDisconnection.ToString();
				strQuery=strQuery+strQueryPostFix;
				if(WSlogService.LogRecord(strQuery)=="1")
					return true;
			}
			catch(Exception ex)
			{
				Client.ClientUI.getInstance().ExceptionLog(" Stack Trace :: " + ex.StackTrace + " Source :: "+ex.Source + " Message::: "+ex.Message);				
			}
			return false;			
		}	
		
		public bool EndMeetingLog()
		{
			try
			{
				this.endTickCount=System.Environment.TickCount;
				string strQuery="Update ctool_applicationlogging set ";
				string strQueryPostFix=" where id=" + RecordID.ToString()+";";
				strQuery+="applog_meetendtime="+Convert.ToString((this.endTickCount/1000)/60);
				strQuery=strQuery+strQueryPostFix;
				if(WSlogService.LogRecord(strQuery)=="1")
					return true;
			}
			catch(Exception ex)
			{
				Client.ClientUI.getInstance().ExceptionLog(" Stack Trace :: " + ex.StackTrace + " Source :: "+ex.Source + " Message::: "+ex.Message);
			}
			return false;
		}


	}
}
