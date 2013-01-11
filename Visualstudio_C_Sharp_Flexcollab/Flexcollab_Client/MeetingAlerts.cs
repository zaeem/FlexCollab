using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;
namespace WebMeeting.Client.Alerts
{
	/// <summary>
	/// Summary description for MeetingAlerts.
	/// </summary>	
	public class MeetingAlerts : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnSendInfo;		
		public enum AlertType
		{
			Info=1,Warning=2,Action=3,NonFatal=4,Fatal=5
		}
		#region Private Variables
		private Int16 MsgType;
		private string message;
		private bool visibility;
		private System.Windows.Forms.PictureBox pbIconInfo;
		private System.Windows.Forms.PictureBox pbIconWarning;
		private System.Windows.Forms.PictureBox pbIconAction;
		private System.Windows.Forms.PictureBox pbIconNonFatal;
		private System.Windows.Forms.PictureBox pbIconFatal;
		private System.Windows.Forms.LinkLabel btnSendInfo2;
		private System.Windows.Forms.TextBox txtInfo;
		private System.Windows.Forms.Button btnOK;
		private bool force;
		enum MessageType
		{MsgInfo=1,MsgWarning=2,MsgActionError=3,MsgNonFatalError=4,MsgFatalError=5}
		#endregion
		#region Public Functions		
		public void ShowMessage(int Type,string msg,bool visibility,bool force)
		{	
			if((Type<1)||(Type>5))
				Type=1;
			MsgType=Convert.ToInt16(Type);
			txtInfo.Text=msg.Trim();
			message=msg;
			ShowIcon(Type);
			/*if(Type==Convert.ToInt16(MessageType.MsgInfo))
			{
				//this.Text="Information";				
			}
			else if(Type==Convert.ToInt16(MessageType.MsgWarning))
			{
				//this.Text="Warning";
				
			}
			else if(Type==Convert.ToInt16(MessageType.MsgActionError))
			{
				//this.Text="Error";
				
			}
			else if(Type==Convert.ToInt16(MessageType.MsgNonFatalError))
			{
				//this.Text="Non-Fatal Error";
				
			}
			else if(Type==Convert.ToInt16(MessageType.MsgFatalError))
			{
				//this.Text="Fatal Error";
				
			}*/			
			 this.ShowDialog();
			if(!visibility)
				this.Close();
		}
		public void ShowMessage(AlertType type,string msg,bool visibility,bool force)
		{	
			int temp=Convert.ToInt16(type);
			if((temp<1)||(temp>5))
				temp=1;
			MsgType=Convert.ToInt16(temp);
			txtInfo.Text=msg.Trim();
			message=msg;
			ShowIcon(Convert.ToInt16(temp));					
			this.ShowDialog();
			if(!visibility)
				this.Close();
		}
		#endregion
		#region private function
			private void ShowIcon(int Type)
			{
				if(Type==1)
				{
					pbIconInfo.Visible=true;
					pbIconWarning.Visible=false;
					pbIconAction.Visible=false;
					pbIconNonFatal.Visible=false;
					pbIconFatal.Visible=false;
				}
				else if(Type==2)
				{
					pbIconInfo.Visible=false;
					pbIconWarning.Visible=true;
					pbIconAction.Visible=false;
					pbIconNonFatal.Visible=false;
					pbIconFatal.Visible=false;
				}
				else if(Type==3)
				{
					pbIconInfo.Visible=false;
					pbIconWarning.Visible=false;
					pbIconAction.Visible=true;
					pbIconNonFatal.Visible=false;
					pbIconFatal.Visible=false;
				}
				else if(Type==4)
				{
					pbIconInfo.Visible=false;
					pbIconWarning.Visible=false;
					pbIconAction.Visible=false;
					pbIconNonFatal.Visible=true;
					pbIconFatal.Visible=false;
				}
				else if(Type==5)
				{
					pbIconInfo.Visible=false;
					pbIconWarning.Visible=false;
					pbIconAction.Visible=false;
					pbIconNonFatal.Visible=false;
					pbIconFatal.Visible=true;
				}
			}
		#endregion
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MeetingAlerts()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			MsgType=Convert.ToInt16(MessageType.MsgInfo);
			this.message="NULL";
			this.visibility=true;
			this.force=false;
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MeetingAlerts));
			this.pbIconInfo = new System.Windows.Forms.PictureBox();
			this.btnSendInfo = new System.Windows.Forms.Button();
			this.pbIconWarning = new System.Windows.Forms.PictureBox();
			this.pbIconAction = new System.Windows.Forms.PictureBox();
			this.pbIconNonFatal = new System.Windows.Forms.PictureBox();
			this.pbIconFatal = new System.Windows.Forms.PictureBox();
			this.btnSendInfo2 = new System.Windows.Forms.LinkLabel();
			this.txtInfo = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pbIconInfo
			// 
			this.pbIconInfo.AccessibleDescription = resources.GetString("pbIconInfo.AccessibleDescription");
			this.pbIconInfo.AccessibleName = resources.GetString("pbIconInfo.AccessibleName");
			this.pbIconInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pbIconInfo.Anchor")));
			this.pbIconInfo.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.pbIconInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbIconInfo.BackgroundImage")));
			this.pbIconInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pbIconInfo.Dock")));
			this.pbIconInfo.Enabled = ((bool)(resources.GetObject("pbIconInfo.Enabled")));
			this.pbIconInfo.Font = ((System.Drawing.Font)(resources.GetObject("pbIconInfo.Font")));
			this.pbIconInfo.Image = ((System.Drawing.Image)(resources.GetObject("pbIconInfo.Image")));
			this.pbIconInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pbIconInfo.ImeMode")));
			this.pbIconInfo.Location = ((System.Drawing.Point)(resources.GetObject("pbIconInfo.Location")));
			this.pbIconInfo.Name = "pbIconInfo";
			this.pbIconInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pbIconInfo.RightToLeft")));
			this.pbIconInfo.Size = ((System.Drawing.Size)(resources.GetObject("pbIconInfo.Size")));
			this.pbIconInfo.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("pbIconInfo.SizeMode")));
			this.pbIconInfo.TabIndex = ((int)(resources.GetObject("pbIconInfo.TabIndex")));
			this.pbIconInfo.TabStop = false;
			this.pbIconInfo.Text = resources.GetString("pbIconInfo.Text");
			this.pbIconInfo.Visible = ((bool)(resources.GetObject("pbIconInfo.Visible")));
			// 
			// btnSendInfo
			// 
			this.btnSendInfo.AccessibleDescription = resources.GetString("btnSendInfo.AccessibleDescription");
			this.btnSendInfo.AccessibleName = resources.GetString("btnSendInfo.AccessibleName");
			this.btnSendInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnSendInfo.Anchor")));
			this.btnSendInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSendInfo.BackgroundImage")));
			this.btnSendInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnSendInfo.Dock")));
			this.btnSendInfo.Enabled = ((bool)(resources.GetObject("btnSendInfo.Enabled")));
			this.btnSendInfo.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnSendInfo.FlatStyle")));
			this.btnSendInfo.Font = ((System.Drawing.Font)(resources.GetObject("btnSendInfo.Font")));
			this.btnSendInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnSendInfo.Image")));
			this.btnSendInfo.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnSendInfo.ImageAlign")));
			this.btnSendInfo.ImageIndex = ((int)(resources.GetObject("btnSendInfo.ImageIndex")));
			this.btnSendInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnSendInfo.ImeMode")));
			this.btnSendInfo.Location = ((System.Drawing.Point)(resources.GetObject("btnSendInfo.Location")));
			this.btnSendInfo.Name = "btnSendInfo";
			this.btnSendInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnSendInfo.RightToLeft")));
			this.btnSendInfo.Size = ((System.Drawing.Size)(resources.GetObject("btnSendInfo.Size")));
			this.btnSendInfo.TabIndex = ((int)(resources.GetObject("btnSendInfo.TabIndex")));
			this.btnSendInfo.Text = resources.GetString("btnSendInfo.Text");
			this.btnSendInfo.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnSendInfo.TextAlign")));
			this.btnSendInfo.Visible = ((bool)(resources.GetObject("btnSendInfo.Visible")));
			this.btnSendInfo.Click += new System.EventHandler(this.btnSendInfo_Click);
			// 
			// pbIconWarning
			// 
			this.pbIconWarning.AccessibleDescription = resources.GetString("pbIconWarning.AccessibleDescription");
			this.pbIconWarning.AccessibleName = resources.GetString("pbIconWarning.AccessibleName");
			this.pbIconWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pbIconWarning.Anchor")));
			this.pbIconWarning.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.pbIconWarning.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbIconWarning.BackgroundImage")));
			this.pbIconWarning.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pbIconWarning.Dock")));
			this.pbIconWarning.Enabled = ((bool)(resources.GetObject("pbIconWarning.Enabled")));
			this.pbIconWarning.Font = ((System.Drawing.Font)(resources.GetObject("pbIconWarning.Font")));
			this.pbIconWarning.Image = ((System.Drawing.Image)(resources.GetObject("pbIconWarning.Image")));
			this.pbIconWarning.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pbIconWarning.ImeMode")));
			this.pbIconWarning.Location = ((System.Drawing.Point)(resources.GetObject("pbIconWarning.Location")));
			this.pbIconWarning.Name = "pbIconWarning";
			this.pbIconWarning.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pbIconWarning.RightToLeft")));
			this.pbIconWarning.Size = ((System.Drawing.Size)(resources.GetObject("pbIconWarning.Size")));
			this.pbIconWarning.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("pbIconWarning.SizeMode")));
			this.pbIconWarning.TabIndex = ((int)(resources.GetObject("pbIconWarning.TabIndex")));
			this.pbIconWarning.TabStop = false;
			this.pbIconWarning.Text = resources.GetString("pbIconWarning.Text");
			this.pbIconWarning.Visible = ((bool)(resources.GetObject("pbIconWarning.Visible")));
			// 
			// pbIconAction
			// 
			this.pbIconAction.AccessibleDescription = resources.GetString("pbIconAction.AccessibleDescription");
			this.pbIconAction.AccessibleName = resources.GetString("pbIconAction.AccessibleName");
			this.pbIconAction.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pbIconAction.Anchor")));
			this.pbIconAction.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.pbIconAction.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbIconAction.BackgroundImage")));
			this.pbIconAction.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pbIconAction.Dock")));
			this.pbIconAction.Enabled = ((bool)(resources.GetObject("pbIconAction.Enabled")));
			this.pbIconAction.Font = ((System.Drawing.Font)(resources.GetObject("pbIconAction.Font")));
			this.pbIconAction.Image = ((System.Drawing.Image)(resources.GetObject("pbIconAction.Image")));
			this.pbIconAction.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pbIconAction.ImeMode")));
			this.pbIconAction.Location = ((System.Drawing.Point)(resources.GetObject("pbIconAction.Location")));
			this.pbIconAction.Name = "pbIconAction";
			this.pbIconAction.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pbIconAction.RightToLeft")));
			this.pbIconAction.Size = ((System.Drawing.Size)(resources.GetObject("pbIconAction.Size")));
			this.pbIconAction.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("pbIconAction.SizeMode")));
			this.pbIconAction.TabIndex = ((int)(resources.GetObject("pbIconAction.TabIndex")));
			this.pbIconAction.TabStop = false;
			this.pbIconAction.Text = resources.GetString("pbIconAction.Text");
			this.pbIconAction.Visible = ((bool)(resources.GetObject("pbIconAction.Visible")));
			// 
			// pbIconNonFatal
			// 
			this.pbIconNonFatal.AccessibleDescription = resources.GetString("pbIconNonFatal.AccessibleDescription");
			this.pbIconNonFatal.AccessibleName = resources.GetString("pbIconNonFatal.AccessibleName");
			this.pbIconNonFatal.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pbIconNonFatal.Anchor")));
			this.pbIconNonFatal.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.pbIconNonFatal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbIconNonFatal.BackgroundImage")));
			this.pbIconNonFatal.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pbIconNonFatal.Dock")));
			this.pbIconNonFatal.Enabled = ((bool)(resources.GetObject("pbIconNonFatal.Enabled")));
			this.pbIconNonFatal.Font = ((System.Drawing.Font)(resources.GetObject("pbIconNonFatal.Font")));
			this.pbIconNonFatal.Image = ((System.Drawing.Image)(resources.GetObject("pbIconNonFatal.Image")));
			this.pbIconNonFatal.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pbIconNonFatal.ImeMode")));
			this.pbIconNonFatal.Location = ((System.Drawing.Point)(resources.GetObject("pbIconNonFatal.Location")));
			this.pbIconNonFatal.Name = "pbIconNonFatal";
			this.pbIconNonFatal.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pbIconNonFatal.RightToLeft")));
			this.pbIconNonFatal.Size = ((System.Drawing.Size)(resources.GetObject("pbIconNonFatal.Size")));
			this.pbIconNonFatal.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("pbIconNonFatal.SizeMode")));
			this.pbIconNonFatal.TabIndex = ((int)(resources.GetObject("pbIconNonFatal.TabIndex")));
			this.pbIconNonFatal.TabStop = false;
			this.pbIconNonFatal.Text = resources.GetString("pbIconNonFatal.Text");
			this.pbIconNonFatal.Visible = ((bool)(resources.GetObject("pbIconNonFatal.Visible")));
			// 
			// pbIconFatal
			// 
			this.pbIconFatal.AccessibleDescription = resources.GetString("pbIconFatal.AccessibleDescription");
			this.pbIconFatal.AccessibleName = resources.GetString("pbIconFatal.AccessibleName");
			this.pbIconFatal.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pbIconFatal.Anchor")));
			this.pbIconFatal.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.pbIconFatal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbIconFatal.BackgroundImage")));
			this.pbIconFatal.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pbIconFatal.Dock")));
			this.pbIconFatal.Enabled = ((bool)(resources.GetObject("pbIconFatal.Enabled")));
			this.pbIconFatal.Font = ((System.Drawing.Font)(resources.GetObject("pbIconFatal.Font")));
			this.pbIconFatal.Image = ((System.Drawing.Image)(resources.GetObject("pbIconFatal.Image")));
			this.pbIconFatal.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pbIconFatal.ImeMode")));
			this.pbIconFatal.Location = ((System.Drawing.Point)(resources.GetObject("pbIconFatal.Location")));
			this.pbIconFatal.Name = "pbIconFatal";
			this.pbIconFatal.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pbIconFatal.RightToLeft")));
			this.pbIconFatal.Size = ((System.Drawing.Size)(resources.GetObject("pbIconFatal.Size")));
			this.pbIconFatal.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("pbIconFatal.SizeMode")));
			this.pbIconFatal.TabIndex = ((int)(resources.GetObject("pbIconFatal.TabIndex")));
			this.pbIconFatal.TabStop = false;
			this.pbIconFatal.Text = resources.GetString("pbIconFatal.Text");
			this.pbIconFatal.Visible = ((bool)(resources.GetObject("pbIconFatal.Visible")));
			// 
			// btnSendInfo2
			// 
			this.btnSendInfo2.AccessibleDescription = resources.GetString("btnSendInfo2.AccessibleDescription");
			this.btnSendInfo2.AccessibleName = resources.GetString("btnSendInfo2.AccessibleName");
			this.btnSendInfo2.ActiveLinkColor = System.Drawing.Color.Brown;
			this.btnSendInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnSendInfo2.Anchor")));
			this.btnSendInfo2.AutoSize = ((bool)(resources.GetObject("btnSendInfo2.AutoSize")));
			this.btnSendInfo2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.btnSendInfo2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnSendInfo2.Dock")));
			this.btnSendInfo2.Enabled = ((bool)(resources.GetObject("btnSendInfo2.Enabled")));
			this.btnSendInfo2.Font = ((System.Drawing.Font)(resources.GetObject("btnSendInfo2.Font")));
			this.btnSendInfo2.Image = ((System.Drawing.Image)(resources.GetObject("btnSendInfo2.Image")));
			this.btnSendInfo2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnSendInfo2.ImageAlign")));
			this.btnSendInfo2.ImageIndex = ((int)(resources.GetObject("btnSendInfo2.ImageIndex")));
			this.btnSendInfo2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnSendInfo2.ImeMode")));
			this.btnSendInfo2.LinkArea = ((System.Windows.Forms.LinkArea)(resources.GetObject("btnSendInfo2.LinkArea")));
			this.btnSendInfo2.LinkColor = System.Drawing.Color.White;
			this.btnSendInfo2.Location = ((System.Drawing.Point)(resources.GetObject("btnSendInfo2.Location")));
			this.btnSendInfo2.Name = "btnSendInfo2";
			this.btnSendInfo2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnSendInfo2.RightToLeft")));
			this.btnSendInfo2.Size = ((System.Drawing.Size)(resources.GetObject("btnSendInfo2.Size")));
			this.btnSendInfo2.TabIndex = ((int)(resources.GetObject("btnSendInfo2.TabIndex")));
			this.btnSendInfo2.TabStop = true;
			this.btnSendInfo2.Text = resources.GetString("btnSendInfo2.Text");
			this.btnSendInfo2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnSendInfo2.TextAlign")));
			this.btnSendInfo2.Visible = ((bool)(resources.GetObject("btnSendInfo2.Visible")));
			this.btnSendInfo2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnSendInfo2_LinkClicked);
			// 
			// txtInfo
			// 
			this.txtInfo.AccessibleDescription = resources.GetString("txtInfo.AccessibleDescription");
			this.txtInfo.AccessibleName = resources.GetString("txtInfo.AccessibleName");
			this.txtInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtInfo.Anchor")));
			this.txtInfo.AutoSize = ((bool)(resources.GetObject("txtInfo.AutoSize")));
			this.txtInfo.BackColor = System.Drawing.Color.White;
			this.txtInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtInfo.BackgroundImage")));
			this.txtInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtInfo.Dock")));
			this.txtInfo.Enabled = ((bool)(resources.GetObject("txtInfo.Enabled")));
			this.txtInfo.Font = ((System.Drawing.Font)(resources.GetObject("txtInfo.Font")));
			this.txtInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtInfo.ImeMode")));
			this.txtInfo.Location = ((System.Drawing.Point)(resources.GetObject("txtInfo.Location")));
			this.txtInfo.MaxLength = ((int)(resources.GetObject("txtInfo.MaxLength")));
			this.txtInfo.Multiline = ((bool)(resources.GetObject("txtInfo.Multiline")));
			this.txtInfo.Name = "txtInfo";
			this.txtInfo.PasswordChar = ((char)(resources.GetObject("txtInfo.PasswordChar")));
			this.txtInfo.ReadOnly = true;
			this.txtInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtInfo.RightToLeft")));
			this.txtInfo.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtInfo.ScrollBars")));
			this.txtInfo.Size = ((System.Drawing.Size)(resources.GetObject("txtInfo.Size")));
			this.txtInfo.TabIndex = ((int)(resources.GetObject("txtInfo.TabIndex")));
			this.txtInfo.Text = resources.GetString("txtInfo.Text");
			this.txtInfo.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtInfo.TextAlign")));
			this.txtInfo.Visible = ((bool)(resources.GetObject("txtInfo.Visible")));
			this.txtInfo.WordWrap = ((bool)(resources.GetObject("txtInfo.WordWrap")));
			// 
			// btnOK
			// 
			this.btnOK.AccessibleDescription = resources.GetString("btnOK.AccessibleDescription");
			this.btnOK.AccessibleName = resources.GetString("btnOK.AccessibleName");
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnOK.Anchor")));
			this.btnOK.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.btnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOK.BackgroundImage")));
			this.btnOK.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnOK.Dock")));
			this.btnOK.Enabled = ((bool)(resources.GetObject("btnOK.Enabled")));
			this.btnOK.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnOK.FlatStyle")));
			this.btnOK.Font = ((System.Drawing.Font)(resources.GetObject("btnOK.Font")));
			this.btnOK.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
			this.btnOK.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnOK.ImageAlign")));
			this.btnOK.ImageIndex = ((int)(resources.GetObject("btnOK.ImageIndex")));
			this.btnOK.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnOK.ImeMode")));
			this.btnOK.Location = ((System.Drawing.Point)(resources.GetObject("btnOK.Location")));
			this.btnOK.Name = "btnOK";
			this.btnOK.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnOK.RightToLeft")));
			this.btnOK.Size = ((System.Drawing.Size)(resources.GetObject("btnOK.Size")));
			this.btnOK.TabIndex = ((int)(resources.GetObject("btnOK.TabIndex")));
			this.btnOK.Text = resources.GetString("btnOK.Text");
			this.btnOK.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnOK.TextAlign")));
			this.btnOK.Visible = ((bool)(resources.GetObject("btnOK.Visible")));
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// MeetingAlerts
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.ControlBox = false;
			this.Controls.Add(this.txtInfo);
			this.Controls.Add(this.btnSendInfo2);
			this.Controls.Add(this.pbIconFatal);
			this.Controls.Add(this.pbIconNonFatal);
			this.Controls.Add(this.pbIconAction);
			this.Controls.Add(this.pbIconWarning);
			this.Controls.Add(this.btnSendInfo);
			this.Controls.Add(this.pbIconInfo);
			this.Controls.Add(this.btnOK);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximizeBox = false;
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "MeetingAlerts";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.TopMost = true;
			this.Enter += new System.EventHandler(this.MeetingAlerts_Enter);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
		    this.Close();
		}

		private void btnSendInfo_Click(object sender, System.EventArgs e)
		{
			StackTrace st = new StackTrace(true);
			string filename="Null";
			int errline=-1;
			string urlEnd;
			WebResponse result = null;				
			for(int i =0; i< st.FrameCount; i++ )
			{
				// High up the call stack, there is only one stack frame
				StackFrame sf = st.GetFrame(i);
				filename=sf.GetFileName().ToString();				
				errline=sf.GetFileLineNumber();
				urlEnd=Info.getInstance().WebsiteName + "/support/error.php?Message="+ message + "&FileName="+ filename + "&LineNumber=" + errline + "&Clientid=" +NetworkManager.getInstance().profile.ClientId +"&MeetingId="+NetworkManager.getInstance().profile.ConferenceID + "& Source=Client & Type="+ MsgType +"&Version="+Application.ProductVersion;
				HttpWebRequest myReq =(HttpWebRequest)WebRequest.Create(urlEnd);
				result=myReq.GetResponse();
				Stream ReceiveStream = result.GetResponseStream();
			}
		    this.Close();
		}

		private void MeetingAlerts_Enter(object sender, System.EventArgs e)
		{
			
		}

		private void btnSendInfo2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{			
			StackTrace st = new StackTrace(new StackFrame(true));
			StackFrame callStack=new StackFrame(1,true);			
			string filename=callStack.GetFileName();
			int errline=callStack.GetFileLineNumber();
			string urlEnd;
			WebResponse result = null;				
			urlEnd=Info.getInstance().WebsiteName + "/error/inserterror.php?Message="+ message + "&FileName="+ filename + "&LineNumber=" + errline + "&ClientId=" +NetworkManager.getInstance().profile.ClientId +"&MeetingId="+NetworkManager.getInstance().profile.ConferenceID + "&Source=Client&Type="+ MsgType;
			HttpWebRequest myReq =(HttpWebRequest)WebRequest.Create(urlEnd);
			result=myReq.GetResponse();
			Stream ReceiveStream = result.GetResponseStream();
			this.Close();
		}
	}	
	
}


