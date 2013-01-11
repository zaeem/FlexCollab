#define OFFICEXP 

using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Threading;
using WebMeeting.FTP;
using System.IO;

using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using WebMeeting.Common;


namespace DocumentSharingControl
{
	


	/// <summary>
	/// Summary description for DocumentSharing.
	/// </summary>
	/// 
	/*
	public class DocumentSharing
	{
		private static object Unknown =Type.Missing; 
		
		private const string LOGGING_MSG = "Logging on to FTP";
		private const string LOGGED_ON_MSG ="Logged on";
		private const string UPLOADING_MSG ="Uploading Files";

		Word.Application wordApp;
		PowerPoint.Application	ppApp;
		//Excel.Application excelApp;
		private string filename;
		private string destFile;
		bool asJpeg;
		int processingTime;
		Thread processingThread;
		FtpClient ftp;
		string meetingID;
		string serverIP;
		string userName;
		string password;
		string sessionID;
		string errorText;
		public string progressString;

		public string ErrorText
		{
			set
			{
				errorText=value;

			}
			get
			{
				return errorText;
			}
		}
		public string ServerIP
		{
			set
			{
				serverIP=value;
			}
			get
			{
				return serverIP;
			}
		}
		public string UserName
		{
			set
			{
				userName=value;
			}
			get
			{
				return userName;
			}
		}
		public string Password
		{
			set
			{
				password=value;
			
			}
			get
			{
				return password;
			}
		}
		public int PercentageComplete
		{
			set
			{
				processingTime=value;
			}
			get
			{
				return processingTime;
			}
		}
		public string MeetingID
		{
			set
			{
				meetingID=value;
			}
			get
			{
				return meetingID;
			}
		}
		public DocumentSharing(string Session)
		{
			sessionID = Session;
			meetingID= WebMeeting.Client.Info.getInstance().ConferenceID;
			processingTime=0;
			userName="ahmed";
			password="one";
			serverIP="127.0.0.1";		
			errorText="";
		}

		private void powerpoint()
		{					
			if(asJpeg==false)
			{
				progressString = "Converting Powerpoint Presentation";
				ppApp= new PowerPoint.Application();
				processingTime=10;
				try
				{
					Directory.CreateDirectory(destFile);
				}
				catch(Exception ee)
				{
					processingTime = -1;
					errorText = "Access is denied in creating directory in WebMeeting installation folder." + ee.Message;    
					return;
				}
				try
				{
					ppApp.Presentations.Open(filename,Microsoft.Office.Core.MsoTriState.msoCTrue,Microsoft.Office.Core.MsoTriState.msoCTrue,Microsoft.Office.Core.MsoTriState.msoFalse);
				}
				catch(Exception ee)
				{
					processingTime = -1;
					errorText = "Powerpoint is unable to open specified file. Please make sure file is not opened in any external Application" + ee.Message;                    
					return;
				}
				processingTime=20;
				try
				{
					PowerPoint.PpSaveAsFileType format= PowerPoint.PpSaveAsFileType.ppSaveAsHTML;//.ppSaveAsJPG;
					ppApp.Presentations[1].SaveAs(destFile + "\\" + Path.GetFileName(destFile),format,Microsoft.Office.Core.MsoTriState.msoCTrue);							
					
				}
				catch(Exception ee)
				{
					processingTime = -1;
					errorText = "Powerpoint is unable to save file in WebMeeting folder. Make sure current user has enough Access rights." + ee.Message;                    
					return;
				}
				
				processingTime=90;
				ppApp.Quit();
			}
			else
			{
				PowerPoint.PpSaveAsFileType format;
				format= PowerPoint.PpSaveAsFileType.ppSaveAsBMP;//.ppSaveAsJPG;
				try
				{
					try
					{
						ppApp= new PowerPoint.Application();
						processingTime=10;				
						ppApp.Presentations.Open(filename,Microsoft.Office.Core.MsoTriState.msoCTrue,Microsoft.Office.Core.MsoTriState.msoCTrue,Microsoft.Office.Core.MsoTriState.msoFalse);
					}
					catch(Exception ee)
					{
						processingTime = -1;
						errorText = "Powerpoint is unable to open specified Presentation. Access is denied. " + ee.Message;                    
						return;

					}
					processingTime=20;
					try
					{
						ppApp.Presentations[1].SaveAs(destFile,format,Microsoft.Office.Core.MsoTriState.msoCTrue);	
						ppApp.Presentations[1].Close();
						ConvertBMPToJPG(destFile);
					}
					catch(Exception ee)
					{
						processingTime = -1;
						errorText = "Powerpoint is unable to save in WebMeeting installation Folder. Access is denied. "+ ee.Message;                    
						return;
					}
				}
				catch(Exception ee)
				{
					processingTime=-1;
					errorText = "Unable to Convert PowerPoint Document. " + ee.Message;

				}
				
				// dummy to prevent error on resending
				try
				{					
					//ppApp.Presentations[1].SaveAs(destFile + "\\temp"  ,format,Microsoft.Office.Core.MsoTriState.msoCTrue);							
					//ppApp.Presentations[1].Close();
				}
				catch(Exception)
				{
					processingTime = -1;
					errorText = "Powerpoint is unable to save in WebMeeting installation Folder. Access is denied";                    
				
				}

				processingTime=90;				
				ppApp.Quit();
				//ppApp.Activate();
			}

			progressString = LOGGING_MSG;
			ftp=new FtpClient();
			ftp.OnDirectoryStatusFunction = new WebMeeting.FTP.FtpClient.OnStatusUpdate(OnDirectoryStatus);
			ftp.OnStatusUpdateFunction = new WebMeeting.FTP.FtpClient.OnStatusUpdate(OnFileStatus);
			ftp.Server=ServerIP;//"127.0.0.1";
			ftp.Username=UserName;
			ftp.Password=Password;
			try
			{
				ftp.Login();
			}
			catch(Exception ee)
			{
				System.Windows.Forms.MessageBox.Show("Error in connecting to FTP server. Contact support","WebMeeting",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
				errorText=ee.Message;
				processingTime=0;
				return;
			}
			try
			{
				
				ftp.MakeDir(meetingID);//.GetFileList();
			}
			catch(Exception)
			{
			
				// it already exists
			}			
			progressString = LOGGED_ON_MSG;
			ftp.ChangeDir(meetingID);
			

			try
			{
				
				ftp.MakeDir(sessionID);//.GetFileList();
			}
			catch(Exception)
			{				
				// it already exists
			}
			progressString = UPLOADING_MSG;
			ftp.ChangeDir(sessionID);			
			try
			{
				nTotalFiless = GetTotalFiles(destFile);
                ftp.UploadDirectory(destFile,true);			

			}
			catch(Exception ee)
			{				
				errorText="Unable to upload files to WebServer. " + ee.Message;
				processingTime=-1;
				return;
			}
			processingTime=100;	

		}

		public void ConvertBMPToJPG(string folder)
		{
			string[] files = Directory.GetFiles(folder,"*.bmp");
			System.Drawing.Imaging.ImageCodecInfo		imageCodecInfo;
			Encoder				encoder;
			EncoderParameter	encoderParameter;
			EncoderParameters	encoderParameters;

			imageCodecInfo = GetEncoderInfo("image/jpeg");

			encoder = Encoder.Quality;	
			encoderParameters = new EncoderParameters(1);

			// Save the bitmap as a JPEG file with quality level 70.
			encoderParameter = new EncoderParameter(encoder, 70L);
			encoderParameters.Param[0] = encoderParameter;

			for(int i=0;i<files.Length;i++)
			{
				try
				{					
					Image bitmap2=Image.FromFile(files[i]);
				
					// Save the image using the JPEG encoder
					// with the specified Quality level.
					bitmap2.Save(Path.ChangeExtension(files[i],".jpg"), imageCodecInfo, encoderParameters);
					bitmap2.Dispose();
      
					//ima.Save(Path.GetFileName(files[i]),System.Drawing.Imaging.ImageFormat.Jpeg);
					File.Delete(files[i]);
				}
				catch(Exception ee)
				{
					ee=ee;
				}					
			}
		}

		private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			int j;
			System.Drawing.Imaging.ImageCodecInfo[] encoders;
			encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
			for(j = 0; j < encoders.Length; ++j)
			{
				if(encoders[j].MimeType == mimeType)
					return encoders[j];
			}
			return null;
		}

		public void SharePowerPoint(string file,string dest, bool jpg)
		{
			filename=file;
			destFile=dest;
			asJpeg=jpg;
			processingTime=0;
			processingThread=new Thread(new ThreadStart(powerpoint));			
			processingThread.Start();
		}

		public bool ExcelDocument()
		{
			try
			{				
				
				if(Directory.Exists(destFile))
					Directory.Delete(destFile,true);

				string destination= "";
				try
				{
					Directory.CreateDirectory(destFile);
					destination = destFile + "\\" + Path.GetFileName(destFile);
				}
				catch(Exception ee)
				{
					processingTime = -1;
					errorText = "Excel is create folder. Access is denied. " + ee.Message;                    
					return false;
				}
								
				Excel.Application excelApp = new Excel.ApplicationClass();
				processingTime = 20;
				excelApp.Visible = false;
				try
				{
					Excel.Workbook newWorkbook = 
						excelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);			

				}
				catch(Exception ee)
				{
					processingTime = -1;
					errorText = "Unable to open specified WorkBook. Access is denied. "+ ee.Message ;                    
					return false;
				}
				processingTime = 30;
				try
				{
					Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(filename,
						0, true, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
						true, false, 0, true, false, false);

					processingTime = 40;							
					try
					{
						excelWorkbook.SaveAs(destination,Excel.XlFileFormat.xlHtml,"","",false,false,XlSaveAsAccessMode.xlNoChange,false,false,false,false,true);
					}
					catch(Exception ee)
					{
						processingTime = -1;
						errorText = "Unable to save specified WorkBook. Access is denied. " + ee.Message;                    
						return false;
					}

				}
				catch(Exception ee)
				{
					processingTime = -1;
					errorText = "Unable to open specified WorkBook. Access is denied. "+ ee.Message;                    
					return false; 

				}
				
				
				
				excelApp.Quit();
				return true;
			}
			catch(Exception ee)
			{
				processingTime = -1;
				errorText = "Unable to convert Excel Document. "+ ee.Message;
			}
			return false;
		}
		public void document()
		{
			try
			{
				
				progressString = "Converting Document";

				if(asJpeg==false)
				{
					object Source=filename;
					Directory.CreateDirectory(destFile);
					object Destination=destFile + "\\" + Path.GetFileName(destFile);
					object rdOnly=true;//Boolean.TrueString;
					object fileSave=false;
					wordApp=new Word.Application();
					processingTime=10;
#if OFFICEXP
					try
					{
						wordApp.Documents.Open(ref Source,ref Unknown, 
							ref rdOnly,ref Unknown,ref Unknown, 
							ref Unknown,ref Unknown,ref Unknown, 
							ref Unknown,ref Unknown,ref Unknown, 
							ref Unknown,ref Unknown,ref Unknown,ref Unknown);//,ref Unknown);
					}
					catch(Exception ee)
					{
						processingTime = -1;
						errorText = "Unable to open specified Document. Access is denied. "+ ee.Message;                    
						return ;
					}
#else
					wordApp.Documents.Open(ref Source,ref Unknown, 
						ref rdOnly,ref Unknown,ref Unknown, 
						ref Unknown,ref Unknown,ref Unknown, 
						ref Unknown,ref Unknown,ref Unknown, 
						ref Unknown,ref Unknown,ref Unknown,ref Unknown,ref Unknown);
#endif

					processingTime=20;

					object format = Word.WdSaveFormat.wdFormatHTML;// kein XML, nutzen?
					try
					{
						wordApp.ActiveDocument.SaveAs(ref Destination,ref format, 
							ref Unknown,ref Unknown,ref Unknown, 
							ref Unknown,ref Unknown,ref Unknown, 
							ref Unknown,ref Unknown,ref Unknown, 
							ref Unknown,ref Unknown,ref Unknown, 
							ref Unknown,ref Unknown);
						processingTime=90;
					
					}
					catch(Exception ee)
					{
						processingTime = -1;
						errorText = "Unable to save specified Document. Access is denied. "+ ee.Message;                    
						return ;
					}
					wordApp.Quit(ref fileSave, ref Unknown, ref Unknown);
				}
				else
				{

					try
					{

						object Source=filename;
						Directory.CreateDirectory(destFile);
						object Destination=destFile + "\\" + Path.GetFileName(destFile);
						object rdOnly=true;//Boolean.TrueString;
						object fileSave=false;
						wordApp=new Word.ApplicationClass();
						processingTime=10;
#if OFFICEXP
						try
						{

							wordApp.Documents.Open(ref Source,ref Unknown, 
								ref Unknown,ref Unknown,ref Unknown, 
								ref Unknown,ref Unknown,ref Unknown, 
								ref Unknown,ref Unknown,ref Unknown, 
								ref Unknown,ref Unknown,ref Unknown,ref Unknown);//, ref Unknown);
							processingTime=20;
						}
						catch(Exception ee)
						{
							processingTime = -1;
							errorText = "Unable to open specified Document. Access is denied. "+ ee.Message;                    
							return ;
						}
#else
						wordApp.Documents.Open(ref Source,ref Unknown, 
							ref Unknown,ref Unknown,ref Unknown, 
							ref Unknown,ref Unknown,ref Unknown, 
							ref Unknown,ref Unknown,ref Unknown, 
							ref Unknown,ref Unknown,ref Unknown,ref Unknown,ref Unknown);
						processingTime=20;
#endif
				
						try
						{

							object format = Word.WdSaveFormat.wdFormatHTML;//.wdFormatRTF;// kein XML, nutzen?
							wordApp.ActiveDocument.SaveAs(ref Destination,ref format, 
								ref Unknown,ref Unknown,ref Unknown, 
								ref Unknown,ref Unknown,ref Unknown, 
								ref Unknown,ref Unknown,ref Unknown, 
								ref Unknown,ref Unknown,ref Unknown, 
								ref Unknown, ref Unknown);
							processingTime=90;
						}
						catch(Exception ee)
						{
							processingTime = -1;
							errorText = "Unable to save specified Document. Access is denied. "+ ee.Message;                    
							return ;
					
						}
						wordApp.Quit(ref fileSave, ref Unknown, ref Unknown);
					}
					catch(Exception ee)
					{				
						processingTime=-1;					
						errorText = ee.Message ;
					}
				}
			}
			catch(Exception ee)
			{
			
				ee=ee;
			}
			progressString = LOGGING_MSG;
			ftp=new FtpClient();
			ftp.OnStatusUpdateFunction = new WebMeeting.FTP.FtpClient.OnStatusUpdate(OnFileStatus);
			ftp.OnDirectoryStatusFunction = new WebMeeting.FTP.FtpClient.OnStatusUpdate(OnDirectoryStatus);
			ftp.Server=ServerIP;//"127.0.0.1";
			ftp.Username=UserName;
			ftp.Password=Password;
			try
			{
				ftp.Login();

			}
			catch(Exception ee)
			{
				System.Windows.Forms.MessageBox.Show("Coundnt connect to the ftp server. Contact Support","WebMeeting",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
				errorText=ee.Message;
				processingTime=100;
				return;
			}
			try
			{
				ftp.MakeDir(meetingID);//.GetFileList();
			}
			catch(Exception)
			{
				// it already exists
			}
			progressString = LOGGED_ON_MSG;
			ftp.ChangeDir(meetingID);

			try
			{				
				ftp.MakeDir(sessionID);//.GetFileList();
			}
			catch(Exception)
			{				
				// it already exists
			}
			progressString = UPLOADING_MSG;
			ftp.ChangeDir(sessionID);

			try
			{
				Thread.Sleep(200);
				nTotalFiless = GetTotalFiles(destFile);
				ftp.UploadDirectory(destFile,true);

			}
			catch(Exception)// ee)
			{
				processingTime = -1;
				errorText = "Unable to upload files to webserver. ";                    
				return ;
					
			}

			processingTime=100;
			//wordApp.Quit(ref Unknown, ref Unknown, ref Unknown);

		}

		public void ShareWordDocument(string file,string dest, bool jpg)
		{
			filename=file;
			destFile=dest;
			asJpeg=jpg;
			processingTime=0;
			processingThread=new Thread(new ThreadStart(document));			
			processingThread.Start();
		}

		public void ShareXlsFile(string file,string dest, bool jpg)
		{
			filename=file;
			destFile=dest;
			asJpeg=jpg;
			processingTime=0;
			processingThread=new Thread(new ThreadStart(excelFile));			
			processingThread.Start();
		}

		
		public void SharePDFFile(string file,string dest, bool jpg)
		{
			filename=file;
			destFile=dest;
			asJpeg=jpg;
			processingTime=0;
			processingThread=new Thread(new ThreadStart(pdfFile));			
			processingThread.Start();
		
		}

		public int GetTotalFiles(string folder)
		{
			try
			{		
				int nTotal = 0;
				string[] directories = Directory.GetDirectories(folder);
				foreach(string directory in directories)
					nTotal += GetTotalFiles(directory);
				string[] files = Directory.GetFiles(folder);
				if(files != null)
					nTotal+= files.Length;
                return nTotal;		
						
			}
			catch(Exception)// ee)
			{
			}
			return 0;
		}
	
		public int nTotalFiless;
		public int nCurrentFile;
		
		public void OnFileStatus(int nPercent)
		{
            progressString = "Uploading " + nCurrentFile.ToString() + " of " + nTotalFiless.ToString() + ". Current File " + nPercent.ToString() + "%";
        }
		public void OnDirectoryStatus(int nFileNo)
		{
			nCurrentFile = nFileNo;
		}

		public void pdfFile()
		{
			try
			{
				progressString = "Converting Document";
				string folderName = System.Windows.Forms.Application.StartupPath + "\\PDF\\" + Path.GetFileNameWithoutExtension(destFile);
				if(Directory.Exists(folderName))
				{
					Directory.Delete(folderName,true);
				}
				Directory.CreateDirectory(folderName);
				string parameters = "\"" + filename + "\" \"" + folderName + @"\" +  Path.GetFileNameWithoutExtension(filename) + "\"";

				ProcessStartInfo pInfo = new ProcessStartInfo(System.Windows.Forms.Application.StartupPath + "\\pdftohtml.exe",parameters);
				pInfo.WindowStyle = ProcessWindowStyle.Hidden;
				Process p = Process.Start(pInfo);
				p.WaitForExit();
				progressString = LOGGING_MSG;
               
				ftp=new FtpClient();
				ftp.OnStatusUpdateFunction = new WebMeeting.FTP.FtpClient.OnStatusUpdate(OnFileStatus);
				ftp.OnDirectoryStatusFunction = new WebMeeting.FTP.FtpClient.OnStatusUpdate(OnDirectoryStatus);

				ftp.Server=ServerIP;//"127.0.0.1";
				ftp.Username=UserName;
				ftp.Password=Password;
				processingTime=50;
				try
				{
					ftp.Login();
				}
				catch(Exception ee)
				{
					System.Windows.Forms.MessageBox.Show("Error in connecting to FTP server. Contact support","WebMeeting",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
					errorText=ee.Message;
					processingTime=100;
					return;
				}
				try
				{
					processingTime=60;
					ftp.MakeDir(meetingID);//.GetFileList();
				}
				catch(Exception)
				{
				
					// it already exists
				}			
				progressString = LOGGED_ON_MSG;
				ftp.ChangeDir(meetingID);
				
				try
				{				
					ftp.MakeDir(sessionID);//.GetFileList();
				}
				catch(Exception)
				{				
					// it already exists
				}
				ftp.ChangeDir(sessionID);
				try
				{				
					ftp.MakeDir("PDF");//.GetFileList();
				}
				catch(Exception)
				{			
					
				}

				ftp.ChangeDir("PDF");
				processingTime=65;
				
				// upload the associated directory
				
				progressString = UPLOADING_MSG;
				try
				{
				
					this.nTotalFiless = GetTotalFiles(folderName);
                    ftp.UploadDirectory(folderName,true);
					processingTime=88;															
				}
				catch(Exception ee)
				{
					System.Windows.Forms.MessageBox.Show(ee.Message + " Upload Directory");

				}
				processingTime=100;	
                				
			}
			catch(Exception)// ee)
			{				
				errorText="Unable to convert PDF to HTML";
				processingTime=100;
				return;
			}

		}
		public byte[] GetBytes(string str)
		{
			byte[] br = new byte[str.Length];
			for(int i = 0 ;i < str.Length   ; i++)
			{
				br[i] = (byte)str[i];
			}
			return br;
		}

		public void WriteFile(string buffer,string path)
		{
			try
			{
				FileStream fr = File.OpenWrite(path);

				fr.Write(GetBytes(buffer),0,buffer.Length);

				fr.Close();
			}
			catch(Exception ee)
			{
				ee = ee;
			}
			

		}

		public byte[] ReadFile(string path)
		{
			FileStream fr = File.OpenRead(path);
			byte[] buffer = new byte[fr.Length];

			fr.Read(buffer,0,buffer.Length);

			fr.Close();

			return buffer;		

			
		}
		public void excelFile()
		{
			// convert the cls document to HTML files here
			// from filename to the destFile (and its folder)
			progressString = "Converting Excel Document...";			
			if(ExcelDocument())
			{
				Thread.Sleep(1000);
				
				string temp2 = destFile+ "\\" + Path.GetFileNameWithoutExtension(this.filename) + "_Files";
				try					
				{
					if(Directory.Exists(temp2))
					{
						string[] strfiles = Directory.GetFiles(temp2,"sheet*.htm");
						if(strfiles != null)
						{
							foreach (string strfile in strfiles)
							{
								byte[] buffer = ReadFile(strfile);
								string str ;//= buffer.ToString();

								System.Text.UTF7Encoding a = new System.Text.UTF7Encoding();
								str =a.GetString(buffer,0,buffer.Length);

								/*
								int nStart = 	str.IndexOf("<script");
								int nEnd = str.IndexOf("</script>");
								if((nStart != -1) && (nEnd  != -1))
								{
									string str2 = str.Substring(nStart,str.Length  - nEnd);
									str = str.Replace(str2," ");									
									WriteFile(str,strfile);					
								}
								*//*
								string toFind = "window.name!=\"frSheet\"";
								int nStart = str.IndexOf(toFind);
								str  = str.Replace(toFind,"1 != 1");
								WriteFile(str,strfile);

								Thread.Sleep(300);								
							}
						}
					}
				}
				catch(Exception ee)
				{

					errorText = ee.Message;
				}

				this.progressString = LOGGING_MSG;
				ftp=new FtpClient();
				ftp.OnStatusUpdateFunction = new WebMeeting.FTP.FtpClient.OnStatusUpdate(OnFileStatus);
				ftp.OnDirectoryStatusFunction = new WebMeeting.FTP.FtpClient.OnStatusUpdate(OnDirectoryStatus);

				ftp.Server=ServerIP;//"127.0.0.1";
				ftp.Username=UserName;
				ftp.Password=Password;
				processingTime=50;
				try
				{
					ftp.Login();
				}
				catch(Exception ee)
				{
					System.Windows.Forms.MessageBox.Show("Error in connecting to FTP server. Contact support","WebMeeting",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
					errorText=ee.Message;
					processingTime=100;
					return;
				}
				try
				{
					processingTime=60;
					ftp.MakeDir(meetingID);//.GetFileList();
				}
				catch(Exception)
				{
				
					// it already exists
				}			
				progressString = LOGGED_ON_MSG;
				ftp.ChangeDir(meetingID);
				
				try
				{				
					ftp.MakeDir(sessionID);//.GetFileList();
				}
				catch(Exception)
				{				
					// it already exists
				}
				ftp.ChangeDir(sessionID);
				progressString = UPLOADING_MSG;
				string title=Path.GetFileNameWithoutExtension(destFile);							
				try
				{
					ftp.MakeDir(title);
				}
				catch(Exception)
				{

				}
				ftp.ChangeDir(title);
				processingTime=65;
								
				// upload the associated directory
				
				string temp = destFile+ "\\" + Path.GetFileNameWithoutExtension(this.filename) + "_Files";
				try					
				{
					nTotalFiless = GetTotalFiles(temp) + 1;
					if(Directory.Exists(temp))
						ftp.UploadDirectory(temp,true);
					processingTime=88;
					// upload the file					
					nCurrentFile++;
					ftp.Upload(destFile + "\\" + Path.GetFileName(destFile) + ".htm");


				}
				catch(Exception ee)
				{
					System.Windows.Forms.MessageBox.Show(ee.Message + " ftp.Upload");

				}
				processingTime=100;	
			}

		}
	}*/
}
