using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using WebMeeting;
using WebMeeting.Client;
using WebMeeting.Common;


namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for findparticipents.
	/// </summary>
	public class findparticipents : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label lblText;
		private NSPAControls.NSButton btnFind;
		public System.Windows.Forms.ListView listParticipents;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public findparticipents()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(findparticipents));
			this.listParticipents = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.lblText = new System.Windows.Forms.Label();
			this.btnFind = new NSPAControls.NSButton();
			this.SuspendLayout();
			// 
			// listParticipents
			// 
			this.listParticipents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listParticipents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listParticipents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							   this.columnHeader1,
																							   this.columnHeader2});
			this.listParticipents.Location = new System.Drawing.Point(0, 64);
			this.listParticipents.Name = "listParticipents";
			this.listParticipents.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.listParticipents.Size = new System.Drawing.Size(232, 200);
			this.listParticipents.TabIndex = 0;
			this.listParticipents.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Status";
			this.columnHeader1.Width = 79;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Name";
			this.columnHeader2.Width = 152;
			// 
			// txtSearch
			// 
			this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSearch.Location = new System.Drawing.Point(8, 16);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(216, 21);
			this.txtSearch.TabIndex = 1;
			this.txtSearch.Text = "";
			this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
			this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
			// 
			// lblText
			// 
			this.lblText.Location = new System.Drawing.Point(8, 0);
			this.lblText.Name = "lblText";
			this.lblText.Size = new System.Drawing.Size(160, 16);
			this.lblText.TabIndex = 2;
			this.lblText.Text = "Search";
			// 
			// btnFind
			// 
			this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFind.ButtonForm = NSPAControls.eButtonForm.Rectangle;
			this.btnFind.Enabled = false;
			this.btnFind.HighlightColor = System.Drawing.SystemColors.HotTrack;
			this.btnFind.HottrackImage = ((System.Drawing.Image)(resources.GetObject("btnFind.HottrackImage")));
			this.btnFind.Location = new System.Drawing.Point(152, 37);
			this.btnFind.Name = "btnFind";
			this.btnFind.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnFind.NormalImage")));
			this.btnFind.OnlyShowBitmap = true;
			this.btnFind.PressedImage = ((System.Drawing.Image)(resources.GetObject("btnFind.PressedImage")));
			this.btnFind.Size = new System.Drawing.Size(60, 24);
			this.btnFind.Text = "Submit";
			this.btnFind.TextAlign = NSPAControls.eTextAlign.Bottom;
			this.btnFind.ToolTip = null;
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// findparticipents
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(212)), ((System.Byte)(208)), ((System.Byte)(200)));
			this.Controls.Add(this.btnFind);
			this.Controls.Add(this.lblText);
			this.Controls.Add(this.txtSearch);
			this.Controls.Add(this.listParticipents);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "findparticipents";
			this.Size = new System.Drawing.Size(232, 264);
			this.ResumeLayout(false);

		}
		#endregion

		public void FindParticipents(string strText)
		{
			listParticipents.Items.Clear();
			ArrayList list = ClientUI.getInstance().arrayParticipents;
			bool isParticipentExist = false;

			for(int i = 0 ; i <list.Count; i ++)
			{			
				ClientProfile profile = (ClientProfile)list[i];
				if(profile.Name.ToLower().IndexOf(strText.ToLower()) != -1)
				{
					string strTemp = "";
					if(profile.clientType == ClientType.ClientAttendee)
						strTemp = "Attendee";
					else if(profile.clientType == ClientType.ClientHost)
						strTemp = "Host";
					else if(profile.clientType == ClientType.ClientPresenter)
						strTemp = "Presenter";

					ListViewItem lv = listParticipents.Items.Add(strTemp);
					lv.UseItemStyleForSubItems = false;
					lv.SubItems.Add(profile.Name);
					lv.SubItems.Add("");//.BackColor = profile.m_MoodIndicatorColor;
					lv.SubItems.Add("");//.BackColor = profile.m_AssignedColor;
					isParticipentExist = true;					
				}
			}

			if(!isParticipentExist)
			{
				ListViewItem lv = listParticipents.Items.Add(" ");
				lv.UseItemStyleForSubItems = false;
				lv.SubItems.Add("Nothing matching");
			}

		}
		private void btnFind_Click(object sender, System.EventArgs e)
		{
			FindParticipents(txtSearch.Text);
		}

		private void txtSearch_TextChanged(object sender, System.EventArgs e)
		{
			btnFind.Enabled = (txtSearch.Text.Length > 0);
			
		}

		private void txtSearch_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				if(txtSearch.Text.Length > 0)
				{
					FindParticipents(txtSearch.Text);
				}
			}
		}
	}
}
