using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ScreenCapture
{
	/// <summary>
	/// Summary description for SourceEnum.
	/// </summary>
	public class SourceEnum : System.Windows.Forms.Form
	{
		public System.Windows.Forms.ListBox audioSources;
		public string selectedItem;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SourceEnum()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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
			this.audioSources = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// audioSources
			// 
			this.audioSources.Location = new System.Drawing.Point(8, 32);
			this.audioSources.Name = "audioSources";
			this.audioSources.Size = new System.Drawing.Size(272, 43);
			this.audioSources.TabIndex = 0;
			this.audioSources.SelectedIndexChanged += new System.EventHandler(this.audioSources_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.TabIndex = 1;
			this.label1.Text = "Audio sources";
			// 
			// SourceEnum
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(288, 86);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.audioSources);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SourceEnum";
			this.Text = "SourceEnum";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.SourceEnum_Closing);
			this.Load += new System.EventHandler(this.SourceEnum_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void audioSources_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			selectedItem = this.audioSources.GetItemText(this.audioSources.SelectedItem);
			
			if(this.selectedItem!="")
				DialogResult=DialogResult.OK;
			else
				DialogResult=DialogResult.Cancel;
			this.Close();

		}

		private void SourceEnum_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}

		private void SourceEnum_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
