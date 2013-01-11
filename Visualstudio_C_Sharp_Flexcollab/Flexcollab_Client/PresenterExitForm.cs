using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for PresenterExitForm.
	/// </summary>
	public class PresenterExitForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private NSPAControls.NSButton button1;
		private NSPAControls.NSButton button2;
		private NSPAControls.NSButton btnClose;
		public System.Windows.Forms.CheckBox chk_MM;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PresenterExitForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PresenterExitForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chk_MM = new System.Windows.Forms.CheckBox();
			this.btnClose = new NSPAControls.NSButton();
			this.button2 = new NSPAControls.NSButton();
			this.button1 = new NSPAControls.NSButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.groupBox1.Controls.Add(this.chk_MM);
			this.groupBox1.Controls.Add(this.btnClose);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.ForeColor = System.Drawing.Color.White;
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(384, 88);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Do you want to";
			// 
			// chk_MM
			// 
			this.chk_MM.Checked = true;
			this.chk_MM.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk_MM.Location = new System.Drawing.Point(16, 24);
			this.chk_MM.Name = "chk_MM";
			this.chk_MM.Size = new System.Drawing.Size(224, 24);
			this.chk_MM.TabIndex = 3;
			this.chk_MM.Text = "Want to Upload  Minuts of Meeting.";
			// 
			// btnClose
			// 
			this.btnClose.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnClose.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnClose.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnClose.HottrackImage")));
			this.btnClose.Location = new System.Drawing.Point(272, 48);
			this.btnClose.Name = "btnClose";
			this.btnClose.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnClose.NormalImage")));
			this.btnClose.OnlyShowBitmap = true;
			this.btnClose.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnClose.PressedImage")));
			this.btnClose.Size = new System.Drawing.Size(104, 32);
			this.btnClose.Text = "Close Conference";
			this.btnClose.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnClose.ToolTip = null;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// button2
			// 
			this.button2.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.button2.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.button2.HottrackImage = ((System.Drawing.Image)(resources.GetObject("button2.HottrackImage")));
			this.button2.Location = new System.Drawing.Point(128, 48);
			this.button2.Name = "button2";
			this.button2.NormalImage = ((System.Drawing.Image)(resources.GetObject("button2.NormalImage")));
			this.button2.OnlyShowBitmap = true;
			this.button2.PressedImage = ((System.Drawing.Image)(resources.GetObject("button2.PressedImage")));
			this.button2.Size = new System.Drawing.Size(144, 32);
			this.button2.Text = "Do not Close Conference";
			this.button2.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.button2.ToolTip = null;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.button1.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.button1.HottrackImage = ((System.Drawing.Image)(resources.GetObject("button1.HottrackImage")));
			this.button1.Location = new System.Drawing.Point(16, 48);
			this.button1.Name = "button1";
			this.button1.NormalImage = ((System.Drawing.Image)(resources.GetObject("button1.NormalImage")));
			this.button1.OnlyShowBitmap = true;
			this.button1.PressedImage = ((System.Drawing.Image)(resources.GetObject("button1.PressedImage")));
			this.button1.Size = new System.Drawing.Size(106, 32);
			this.button1.Text = "Close Conference";
			this.button1.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.button1.ToolTip = null;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// PresenterExitForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(204)), ((System.Byte)(72)));
			this.ClientSize = new System.Drawing.Size(402, 106);
			this.ControlBox = false;
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "PresenterExitForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public bool close = false;
		private void button1_Click(object sender, System.EventArgs e)
		{
			close = true;
			DialogResult = DialogResult.OK;
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			
			close = false;
			DialogResult = DialogResult.OK;
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			
		}
	}
}
