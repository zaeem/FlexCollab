using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using WebMeeting.Client.Alerts;

using System.Drawing.Printing;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for NotesControl.
	/// </summary>
	public class NotesControl : System.Windows.Forms.UserControl
	{
		private PrintableRichTextBox textBox;
		private bool modified;
		private string fileName;
		private bool ignoreFirst;
		private System.Windows.Forms.Panel bottomPanel;
		private System.Windows.Forms.Panel topPanel;
		private NSPAControls.NSButton btnOpen;
		private NSPAControls.NSButton btnSave;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NotesControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			modified=false;
			fileName="";
			ignoreFirst=false;

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NotesControl));
			this.textBox = new WebMeeting.Client.PrintableRichTextBox();
			this.bottomPanel = new System.Windows.Forms.Panel();
			this.btnSave = new NSPAControls.NSButton();
			this.btnOpen = new NSPAControls.NSButton();
			this.topPanel = new System.Windows.Forms.Panel();
			this.bottomPanel.SuspendLayout();
			this.topPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox.HiglightColor = Khendys.Controls.RtfColor.White;
			this.textBox.Location = new System.Drawing.Point(0, 0);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(256, 240);
			this.textBox.TabIndex = 0;
			this.textBox.Text = "";
			this.textBox.TextColor = Khendys.Controls.RtfColor.Black;
			this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
			// 
			// bottomPanel
			// 
			this.bottomPanel.Controls.Add(this.btnSave);
			this.bottomPanel.Controls.Add(this.btnOpen);
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Location = new System.Drawing.Point(0, 240);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size(256, 40);
			this.bottomPanel.TabIndex = 3;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnSave.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnSave.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnSave.HottrackImage")));
			this.btnSave.Location = new System.Drawing.Point(176, 8);
			this.btnSave.Name = "btnSave";
			this.btnSave.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnSave.NormalImage")));
			this.btnSave.OnlyShowBitmap = true;
			this.btnSave.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnSave.PressedImage")));
			this.btnSave.Size = new System.Drawing.Size(72, 32);
			this.btnSave.Text = "nsButton1";
			this.btnSave.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnSave.ToolTip = null;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnOpen
			// 
			this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpen.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnOpen.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnOpen.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnOpen.HottrackImage")));
			this.btnOpen.Location = new System.Drawing.Point(104, 8);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnOpen.NormalImage")));
			this.btnOpen.OnlyShowBitmap = true;
			this.btnOpen.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnOpen.PressedImage")));
			this.btnOpen.Size = new System.Drawing.Size(80, 32);
			this.btnOpen.Text = "nsButton1";
			this.btnOpen.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnOpen.ToolTip = null;
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// topPanel
			// 
			this.topPanel.Controls.Add(this.textBox);
			this.topPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.topPanel.Location = new System.Drawing.Point(0, 0);
			this.topPanel.Name = "topPanel";
			this.topPanel.Size = new System.Drawing.Size(256, 240);
			this.topPanel.TabIndex = 4;
			// 
			// NotesControl
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(212)), ((System.Byte)(208)), ((System.Byte)(200)));
			this.Controls.Add(this.topPanel);
			this.Controls.Add(this.bottomPanel);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "NotesControl";
			this.Size = new System.Drawing.Size(256, 280);
			this.bottomPanel.ResumeLayout(false);
			this.topPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOpen_Click(object sender, System.EventArgs e)
		{
			DialogResult res;
			if(modified==true)
			{
				res=MessageBox.Show("Do you want to save the current notes","Save notes?",System.Windows.Forms.MessageBoxButtons.YesNoCancel);
				if(res==DialogResult.Cancel) 
					return;
				else if(res==DialogResult.Yes)
				{
					btnSave_Click(this,new System.EventArgs());
				}
			}

			OpenFileDialog dlg=new OpenFileDialog();
			dlg.Filter="TXT|*.txt";
			res=dlg.ShowDialog();
			if(res==DialogResult.Cancel)
			{
				return;
			}
			try
			{
				fileName=dlg.FileName;//
				StreamReader sr = new StreamReader(dlg.FileName);
				string s = sr.ReadToEnd();
				sr.Close();			
				ignoreFirst=true;
				this.textBox.Text=s;
				this.modified=false;
			}
			catch(Exception)
			{	
				MeetingAlerts alert=new MeetingAlerts();
				alert.ShowMessage(Alerts.MeetingAlerts.AlertType.NonFatal,"Couldnt open " + this.fileName + " for reading",true,false);
				//MessageBox.Show("Couldnt open " + this.fileName + " for reading","WebMeeting",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
			}			
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			Save();
			/*try
			{
				StreamWriter sw = new StreamWriter(this.fileName);
				sw.Write(this.textBox.Text);
				sw.Close();			
				this.modified=false;
				MessageBox.Show(this.fileName + " saved");				
			}
			catch(Exception)
			{
				MessageBox.Show("Could not open " + this.fileName + " for writing");
			}
			*/
		}

		public void Save()
		{
			if(fileName=="")
			{
				SaveAs();
				return;
			}
			try
			{
				StreamWriter sw = new StreamWriter(this.fileName);
				sw.Write(this.textBox.Text);
				sw.Close();			
				this.modified=false;
				MeetingAlerts alert=new MeetingAlerts();
				alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Info,this.fileName + " saved",true,false);
//				MessageBox.Show(this.fileName + " saved","WebMeeting",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
			}
			catch(Exception)
			{
                MeetingAlerts alert=new MeetingAlerts();
				alert.ShowMessage(Alerts.MeetingAlerts.AlertType.Action,"Could not open " + this.fileName + " for writing",true,false);
				//MessageBox.Show("Could not open " + this.fileName + " for writing","WebMeeting",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
			}
		}
		public void SaveAs()
		{
			SaveFileDialog fd=new SaveFileDialog();
			fd.FileName=this.fileName;
			fd.Filter="Text Files(*.txt)|*.txt";
			DialogResult res=fd.ShowDialog();
			if(res==DialogResult.Cancel)
				return;
			fileName=fd.FileName;
			Save();
		}
	
		public void PrintIt()
		{
			if(NetworkManager.getInstance().profile.clientType == WebMeeting.Common.ClientType.ClientAttendee)
				return;

			System.Drawing.Printing.PrintDocument p = new System.Drawing.Printing.PrintDocument();
			p.DocumentName = "Notes";
			p.PrintPage+=new System.Drawing.Printing.PrintPageEventHandler(p_PrintPage);
			p.BeginPrint += new PrintEventHandler(p_BeginPrint);
			p.Print();
		}
		private void p_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			checkPrint = textBox.Print(checkPrint, textBox.TextLength, e);

			// Check for more pages
			if (checkPrint < textBox.TextLength)
				e.HasMorePages = true;
			else
				e.HasMorePages = false;
		}
		private int checkPrint;
		

		private void p_BeginPrint(object sender, PrintEventArgs e)
		{
			checkPrint = 0;
		}
		private void textBox_TextChanged(object sender, System.EventArgs e)
		{
			if(ignoreFirst==true)			
				ignoreFirst=false;			
			else
				this.modified=true;			
		}
	}
}
