using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Threading;
using WebMeeting.FTP;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using WebMeeting.Common;
using WebMeeting.Client.Alerts;
namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for DesktopSharingEx.
	/// </summary>
	public class DocumentSharingEx
	{	
		#region variable Declarations
		public delegate void OnStatusUpdate(string strCurrentStatus);
        public FileInfo theFile;
		public OnStatusUpdate OnStatusUpdateFunction;		
		private const string LOGGING_MSG = "Logging on to FTP";
		private const string LOGGED_ON_MSG ="Logged on";
		private const string UPLOADING_MSG ="Uploading Files    ";
		private const string COMPRESSING_MSG="Compressing File   ";
		private const string DOCSHARING_REQUEST_MSG = "Initializing document sharing in Conference....";
		private string m_sharedFilePath;
		private string strRemoteUrl;
		private string m_serverIP;
		private string m_username;
		private string m_password;			
		private string m_meetingID;		
		private string savedFileNameAndPathCookie;		// remove file after sharing
		public DocumentMessageType m_DocumentType;
		public int sessionID;		
		
		#endregion

		public string getSavedFileNameAndPath ()
		{
			return savedFileNameAndPathCookie;
		}


		public void setSavedFileNameAndPath (string FilePath)
		{
			savedFileNameAndPathCookie = FilePath;
		}


		public DocumentSharingEx(WebMeeting.Common.DocumentMessageType type,int session,string ServerIP,string ServerUsername,string ServerPassword,string meetingID)

		{
			m_DocumentType = type;
			sessionID = session;
			m_serverIP = ServerIP;
			m_username = ServerUsername;
			m_password = ServerPassword;
			m_meetingID = meetingID;
		}


		public DocumentSharingEx(WebMeeting.Common.DocumentMessageType type,int session)
		{
			m_DocumentType = type;
			sessionID = session;
		}
		/// <summary>
		/// This function create a local directory structure for both uploaded and downloaded files
		/// </summary>
		/// <param name="strPath"></param>
		/// <param name="bUpload"></param>
		/// 

		#region CreateLocalDirectoryStructure(bool bUpload)

		void CreateLocalDirectoryStructure(bool bUpload)
		{
			try
			{
				string strLocalPath = Application.StartupPath + "\\DocumentSharing";
				try
				{
					if(!Directory.Exists(strLocalPath))
						Directory.CreateDirectory(strLocalPath);
				}
				catch(Exception exp)
				{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("DocumentSharingEx.cs ==>CreateLocalDirectoryStructure( line==>90",exp,null,false);			
				}
				if(bUpload)
					strLocalPath += "\\Uploaded";				
				else
					strLocalPath += "\\Downloaded";

				try
				{
					if(!Directory.Exists(strLocalPath))
						Directory.CreateDirectory(strLocalPath);
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("DocumentSharingEx.cs ==>CreateLocalDirectoryStructure( line==>104",exp,null,false);			
				}
				
				string temp = strLocalPath;
				temp += "\\Presentations";
				try
				{
					if(!Directory.Exists(temp))
						Directory.CreateDirectory(temp);
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("DocumentSharingEx.cs ==>CreateLocalDirectoryStructure( line==>116",exp,null,false);			
				}

				temp = strLocalPath;
				temp += "\\Documents";
				try
				{
					if(!Directory.Exists(temp))
						Directory.CreateDirectory(temp);
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("DocumentSharingEx.cs ==>CreateLocalDirectoryStructure( line==>129",exp,null,false);			
				}

				temp = strLocalPath;
				temp += "\\WorkBooks";
				try
				{
					if(!Directory.Exists(temp))
						Directory.CreateDirectory(temp);
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("DocumentSharingEx.cs ==>CreateLocalDirectoryStructure( line==>140",exp,null,false);			
				}


			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("DocumentSharingEx.cs ==>CreateLocalDirectoryStructure( line==>147",exp,null,false);			
			}

		}

		# endregion 



		#region ConvertContentLength(long length)
		private string ConvertContentLength(long length)
		{
			try
			{
				length = length / 1024;
				return length.ToString() + " KB";			
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("DocumentSharingEx.cs ==>ConvertContentLength( line==>167",exp,null,false);			
				return "";
			}
			return "";
		}
		#endregion 


		# region DownloadFile(String remoteFilename,String localFilename)
		public int DownloadFile(String remoteFilename,String localFilename)
		{
			// Function will return the number of bytes processed
			// to the caller. Initialize to 0 here.
			int bytesProcessed = 0;

			// Assign values to these objects here so that they can
			// be referenced in the finally block
			Stream remoteStream  = null;
			Stream localStream   = null;
			WebResponse response = null;

			// Use a try/catch/finally block as both the WebRequest and Stream
			// classes throw exceptions upon error
			try
			{
				// Create a request for the specified remote file name
				if(OnStatusUpdateFunction !=null)
					OnStatusUpdateFunction("Connecting to webserver");//"Unable to upload files to WebServer. " + ee.Message);                    				
				WebRequest request = WebRequest.Create(remoteFilename);
				if (request != null)
				{
					// Send the request to the server and retrieve the
					// WebResponse object 
					response = request.GetResponse();
					if (response != null)
					{
						if(OnStatusUpdateFunction !=null)
							OnStatusUpdateFunction("Connected to webserver");//"Unable to upload files to WebServer. " + ee.Message);                    				

						// Once the WebResponse object has been retrieved,
						// get the stream object associated with the response's data
						remoteStream = response.GetResponseStream();

						// Create the local file
						localStream = File.Create(localFilename);

						long total = response.ContentLength;
						// Allocate a 1k buffer
						byte[] buffer = new byte[1024];
						int bytesRead;

						// Simple do/while loop to read from stream until
						// no bytes are returned
						string strTotal= ConvertContentLength(total);
						do
						{
							// Read data (up to 1k) from the stream
							bytesRead = remoteStream.Read (buffer, 0, buffer.Length);

							// Write the data to the local file
							localStream.Write (buffer, 0, bytesRead);

							// Increment total bytes processed
							bytesProcessed += bytesRead;
							if(OnStatusUpdateFunction !=null)
								OnStatusUpdateFunction("Downloading File " + Path.GetFileName(remoteFilename) + " " + ConvertContentLength(bytesProcessed) + " Of " + strTotal );//"Unable to upload files to WebServer. " + ee.Message);                    				

						} while (bytesRead > 0);
					}
				}
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("DocumentSharingEx.cs ==>DownloadFile( line==>241",exp,null,false);			


				//Console.WriteLine(e.Message);
				if(OnStatusUpdateFunction !=null)
					OnStatusUpdateFunction("Unable to download file from server. Please make sure file exists on webserver.");//"Unable to upload files to WebServer. " + ee.Message);                    					
			}
			finally
			{
				// Close the response and streams objects here 
				// to make sure they're closed even if an exception
				// is thrown at some point
				if (response     != null) response.Close();
				if (remoteStream != null) remoteStream.Close();
				if (localStream  != null) localStream.Close();
			}

			// Return total bytes processed to caller.
			return bytesProcessed;
		}
		#endregion 

		#region RecieveDocumentSharing(MessageObject msg,out string localFile)
		public bool RecieveDocumentSharing(MessageObject msg,out string localFile)
		{
			localFile = "";
			try
			{

				string documentFolderName = "";					
				if(this.m_DocumentType == DocumentMessageType.typeDocument)
					documentFolderName = "Documents";
				else if(this.m_DocumentType == DocumentMessageType.typeExcel)
					documentFolderName = "WorkBooks";
				else if(this.m_DocumentType == DocumentMessageType.typePDF)
					documentFolderName = "PDF";
				else if(this.m_DocumentType == DocumentMessageType.typePresentation)
					documentFolderName = "Presentations";                

				WebMeeting.Common.DocumentSharing docMessage = (DocumentSharing)msg;	
				localFile = Application.StartupPath + "\\DocumentSharing\\Downloaded\\" + documentFolderName + "\\" + docMessage.sessionID;
				CreateLocalDirectoryStructure(false);					
				if(!Directory.Exists(localFile))
					Directory.CreateDirectory(localFile);
				string strDirectory = localFile;
				localFile = localFile + "\\"  + Path.GetFileName(docMessage.DownloadURL);

				if(File.Exists(localFile))
					File.Delete(localFile);
				
				if(DownloadFile(docMessage.DownloadURL,localFile) <= 1)
					return false;

				///////////////////////////////////////
				this.setSavedFileNameAndPath(localFile);
						
				ICSharpZip.UnzipZip(localFile,strDirectory + "\\");
				//ICSharpZip.UnzipFile.UnZippedFrom(localFile,strDirectory + "\\" );//+ Path.GetFileNameWithoutExtension(docMessage.DownloadURL));
                
				localFile = strDirectory + "\\" + Path.GetFileNameWithoutExtension(docMessage.DownloadURL);
				return true;
				
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>RecieveDocumentSharing( line==>304",exp,"Unable to communicate with WebServer. " + exp.Message.ToString(),true);			
		
				if(OnStatusUpdateFunction !=null)
					OnStatusUpdateFunction("Unable to communicate with WebServer. " + exp.Message);                    			
				return false;
			}

			//return false;

		}
		# endregion 

		/// <summary>
		/// Generic Upload function for all type of documents
		/// 
		/// </summary>
		# region UploadDocument()
		private bool UploadDocument()
		{
			try
			{
				// login to ftp server\
				
				//strRemoteUrl="http://www.compassnav.com/";
				//strRemoteUrl=Info.getInstance().WebsiteName +@"/"; 

				//zaeem: Hard coded to run on Vista System
				//strRemoteUrl="http://216.117.130.240:5216"+@"/";
				//strRemoteUrl="http://soerwa.hopto.org/Compassnav"+@"/";
				strRemoteUrl="http://vista/Compassnav"+@"/";

				
				//FtpClient ftp = new FtpClient(m_serverIP,m_username,m_password);
				//FtpClient ftp = new FtpClient("soerwa.hopto.org","zaeem","zaeem");
				FtpClient ftp = new FtpClient("vista","Ahmed","three");

				ftp.OnStatusUpdateFunction = new WebMeeting.FTP.FtpClient.OnStatusUpdate(FtpUploadStatus);
				try
				{
					if(OnStatusUpdateFunction !=null)
						OnStatusUpdateFunction(LOGGING_MSG);//"Unable to upload files to WebServer. " + ee.Message);                    			
	
					ftp.Login();
				}
				catch(Exception exp)
				{
					
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>UploadDocument() line==>341",exp,"Unable to Logon to server.",true);			
					if(OnStatusUpdateFunction !=null)
						OnStatusUpdateFunction("Unable to Logon to server.");                    
					return false;
				}

				
				try
				{

					ftp.ChangeDir("WebMeeting");
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>UploadDocument() line==>355",exp,null,false);			
					ftp.MakeDir("WebMeeting");
					ftp.ChangeDir("WebMeeting");
				}
				strRemoteUrl+="WebMeeting/";
			
				try
				{
					ftp.ChangeDir("Uploads");
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>UploadDocument() line==>367",exp,null,false);			
					ftp.MakeDir("Uploads");
					ftp.ChangeDir("Uploads");
				}
				strRemoteUrl+="Uploads/";
				try
				{						
					ftp.MakeDir(m_meetingID);//.GetFileList();
					strRemoteUrl+=(m_meetingID.ToString()+"/");
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>UploadDocument() line==>379",exp,null,false);			
					// it already exists
				}			
				
				if(OnStatusUpdateFunction !=null)
					OnStatusUpdateFunction(LOGGED_ON_MSG);//"Unable to upload files to WebServer. " + ee.Message);                    				

				ftp.ChangeDir(m_meetingID);
		
				try
				{				
					ftp.MakeDir(sessionID.ToString());//.GetFileList();
					strRemoteUrl+=sessionID.ToString()+"/";
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>UploadDocument() line==>395",exp,null,false);			
			
					// it already exists
				}
				OnStatusUpdateFunction(UPLOADING_MSG);
				ftp.ChangeDir(sessionID.ToString());			
				string documentFolderName = "";
				try
				{
					
					if(this.m_DocumentType == DocumentMessageType.typeDocument)
						documentFolderName = "Documents";
					else if(this.m_DocumentType == DocumentMessageType.typeExcel)
						documentFolderName = "WorkBooks";
					else if(this.m_DocumentType == DocumentMessageType.typePDF)
						documentFolderName = "PDF";
					else if(this.m_DocumentType == DocumentMessageType.typePresentation)
						documentFolderName = "Presentations";
                    
					ftp.MakeDir(documentFolderName);
					strRemoteUrl+=documentFolderName;
					
					
				}
				catch(Exception exp )
				{						
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>UploadDocument() line==>419",exp,null,false);			
			
				}
				
				ftp.ChangeDir(documentFolderName);
				try
				{
					if(OnStatusUpdateFunction !=null)
						OnStatusUpdateFunction(COMPRESSING_MSG + Path.GetFileName(m_sharedFilePath));//"Unable to upload files to WebServer. " + ee.Message);                    				

					string localFile = "";
					string fileWithoutExt = "";
					if(m_sharedFilePath.IndexOf("Downloaded", 0, m_sharedFilePath.Length)!=-1)
					{
						localFile = m_sharedFilePath;
					}
					else
					{
						fileWithoutExt = Path.GetFileNameWithoutExtension(m_sharedFilePath);
						/*
							if(fileWithoutExt.IndexOf(".") > 0)
						{
							MessageBox.Show("==>file with Dot");
							fileWithoutExt = Path.GetFileNameWithoutExtension(fileWithoutExt );
							
							localFile = Application.StartupPath + "\\DocumentSharing\\Uploaded\\" + documentFolderName + "\\"  + fileWithoutExt ;
							CreateLocalDirectoryStructure(true);					
							if(Directory.Exists(localFile))
								Directory.Delete(localFile,true);
							Directory.CreateDirectory(localFile);
							
							localFile += "\\" + Path.GetFileName(m_sharedFilePath) ;
						}
						else
						*/
						//{
						//	MessageBox.Show("==>file with NOOOOO Dot");
							
							localFile = Application.StartupPath + "\\DocumentSharing\\Uploaded\\" + documentFolderName + "\\"  + fileWithoutExt ;
							CreateLocalDirectoryStructure(true);					
							if(Directory.Exists(localFile))
								Directory.Delete(localFile,true);
							Directory.CreateDirectory(localFile);
							
							localFile += "\\" + Path.GetFileName(m_sharedFilePath) + ".zip";
						//}

					}

					
					try
					{
						
						ICSharpZip.ZipFile(m_sharedFilePath, localFile);
						if(OnStatusUpdateFunction !=null)
							OnStatusUpdateFunction(UPLOADING_MSG + Path.GetFileName(m_sharedFilePath));//"Unable to upload files to WebServer. " + ee.Message);                    				
					}
					catch(Exception exp)
					{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>UploadDocument() line==>474",exp,exp.Message.ToString(),true);			
			
					}
					
					ftp.Upload(localFile);
					//ftp.Upload(this.m_sharedFilePath);	
					//ftp.UploadDirectory(destFile,true);
					ftp.Close();
				//	theFile=new FileInfo(localFile);
				//	theFile.Delete();
					
					return true;

				}
				catch(Exception exp)
				{	
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>UploadDocument() line==>490",exp,"Unable to upload files to WebServer. " + exp.Message.ToString(),true);			
					if(OnStatusUpdateFunction !=null)
						OnStatusUpdateFunction("Unable to upload files to WebServer. " + exp.Message);                    			

					//ftp.Close();
					//MessageBox.Show("Couldnt upload " + ee.ToString());
					//Zaeem//return false;
					return true;
				}				
							
				
			}							
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>UploadDocument() line==>490",exp,"Unable to upload files to WebServer. " + exp.Message.ToString(),true);			
				if(OnStatusUpdateFunction !=null)
					OnStatusUpdateFunction("Unable to upload files to WebServer. " + exp.Message);  
                
  			 // MessageBox.Show("Couldnt upload " + ee.ToString());
			}
			//return false;
			return true;;
		}
		
		#endregion 

		# region createUploadFullPath_MangCont(string fileName)
		public static string createUploadFullPath_MangCont(string fileName)
		{
			string localFile = "";
			try
			{
				string documentFolderName = "";

				string Extension = Path.GetExtension(fileName);
				Extension = Extension.ToLower();
			
				if(Extension == ".doc" || Extension == ".docx")
					documentFolderName = "Documents";
				else if(Extension == ".xls" || Extension == ".xlsx")
					documentFolderName = "WorkBooks";
				else if(Extension == ".ppt" || Extension == ".pptx")
					documentFolderName = "Presentations";
				else if(Extension == ".pdf")
					documentFolderName = "PDF";
 
				
				localFile = Application.StartupPath + "\\DocumentSharing\\uupload\\" + documentFolderName + "\\"  + Path.GetFileNameWithoutExtension(fileName) ;
				CreateLocalDirStruct_ManCont();					
				if(Directory.Exists(localFile))
					Directory.Delete(localFile,true);
				Directory.CreateDirectory(localFile);
				localFile = localFile + "\\" + fileName;
			}
				
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>UploadDocument() line==>549",exp,null,false);
				
			}

			return localFile ;
		
		}
		# endregion 

		# region CreateLocalDirStruct_ManCont()
		public static void CreateLocalDirStruct_ManCont()
		{
			try
			{
				string strLocalPath = Application.StartupPath + "\\DocumentSharing";
				try
				{
					if(!Directory.Exists(strLocalPath))
						Directory.CreateDirectory(strLocalPath);
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>CreateLocalDirStruct_ManCont()line==>570",exp,null,false);
				}
				
				strLocalPath += "\\Uploaded";				
				
				try
				{
					if(!Directory.Exists(strLocalPath))
						Directory.CreateDirectory(strLocalPath);
				}
				catch(Exception exp)
				{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>CreateLocalDirStruct_ManCont()line==>582",exp,null,false);
				
				}
				
				string temp = strLocalPath;
				temp += "\\Presentations";
				try
				{
					if(!Directory.Exists(temp))
						Directory.CreateDirectory(temp);
				}
				catch(Exception exp)
				{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>CreateLocalDirStruct_ManCont()line==>595",exp,null,false);
				}

				temp = strLocalPath;
				temp += "\\Documents";
				try
				{
					if(!Directory.Exists(temp))
						Directory.CreateDirectory(temp);
				}
				catch(Exception exp)
				{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>CreateLocalDirStruct_ManCont()line==>607",exp,null,false);
				}

				temp = strLocalPath;
				temp += "\\WorkBooks";
				try
				{
					if(!Directory.Exists(temp))
						Directory.CreateDirectory(temp);
				}
				catch(Exception exp)
				{
						WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>CreateLocalDirStruct_ManCont()line==>618",exp,null,false);
				}


			}
			catch(Exception exp)
			{
					WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>CreateLocalDirStruct_ManCont()line==>626",exp,null,false);
			}

		}

		#endregion 

		#region StatusUpdateFunctions
		private void FtpUploadStatus(int nProgress)
		{
			try
			{
				if(OnStatusUpdateFunction !=null)
					OnStatusUpdateFunction("Processing File: " + Path.GetFileName(m_sharedFilePath) + " " +  nProgress.ToString() + "%");                    						
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>FtpUploadStatus(line==>643",exp,null,false);
	
			}
		}
		#endregion


		#region DocumentShareStart(string strPath,out MessageObject msg)
		public bool DocumentShareStart(string strPath,out MessageObject msg)
		{
			msg = null;
			try
			{
				m_sharedFilePath = strPath;
				if(UploadDocument())
				{
					if(OnStatusUpdateFunction !=null)
						OnStatusUpdateFunction(DOCSHARING_REQUEST_MSG);
					NetworkManager networkMngr = NetworkManager.getInstance();
					WebMeeting.Common.DocumentSharing message = new WebMeeting.Common.DocumentSharing();
					message.SenderID = networkMngr.profile.ClientId;
					message.ConferenceID=networkMngr.profile.ConferenceID;
					message.CurrentPage=1;				
					message.senderProfile = networkMngr.profile;
					message.sessionID = this.sessionID;
					message.DocumentSharingSessionId = this.sessionID;
					message.DownloadURL = Path.GetFileName(strPath);//this.strRemoteUrl;
					message.DocumentType = this.m_DocumentType;
					MessageObject Message2 = ((MessageObject)message);
					if(networkMngr.StartDocumentSharing(ref Message2) == false)
					{
						if(OnStatusUpdateFunction !=null)
							OnStatusUpdateFunction("Unable to Initialize Document Sharing. Please try again");     					
						return false;
					}
					if(message.DocumentType==DocumentMessageType.typePDF)
						this.strRemoteUrl+=("/"+Path.GetFileNameWithoutExtension(strPath));//this.strRemoteUrl;
					
					message.DownloadURL=this.strRemoteUrl;/// Remote URL of the files
					
					msg = message;
					((DocumentSharing)msg).TotalPages=((DocumentSharing)Message2).TotalPages;
					
					//msg = Message2;
					if(OnStatusUpdateFunction !=null)
						OnStatusUpdateFunction("Starting Document Sharing.");
				
					return true;
				}				
			}
			catch(Exception exp)
			{
			WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage( "DocumentSharingEx.cs ==>DocumentShareStart(   line==>650",exp,null,false);
			}
			return false;
		}
		#endregion
	}
	
}
