using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WMEncoderLib;
using System.Configuration;

namespace WebMeeting.Client.Screen_Capture
{
	/// <summary>
	/// Summary description for frm_AudioConfig.
	/// </summary>
	public class frm_AudioConfig : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.GroupBox Tip;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btn_Previous;
		private System.Windows.Forms.Button btn_Next;
		private System.Windows.Forms.Button btn_Finish;
		private System.Windows.Forms.Button btn_Cancel;
		public IWMEncPluginInfo PlugInfo;
		public ScreenCapture.Config config;
		
		

		public System.Windows.Forms.ComboBox cboAudioSource;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Construtor Audio Configuration
		public frm_AudioConfig()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			btn_Previous.Enabled=false;
			btn_Finish.Enabled=false;
			
			// Create a WMEncoder object.
			WMEncoder Encoder = new WMEncoder();



			// Retrieve source and device plug-in info manager objects from WMEncoder.
			IWMEncSourcePluginInfoManager SrcPlugMgr = Encoder.SourcePluginInfoManager;
			IWMEncDeviceControlPluginInfoManager DCPlugMgr = Encoder.DeviceControlPluginInfoManager;
			
			// Loop through all the audio and video devices on the system.
			for (int i = 0; i < SrcPlugMgr.Count; i++)
			{
				// Set the IWMEncPluginInfo object to the current plug-in.
				PlugInfo = SrcPlugMgr.Item(i);

				// Find the device plug-ins that support resources.
				if (PlugInfo.SchemeType == "DEVICE" && PlugInfo.Resources == true)
				{
					// Loop through the resources in the current plug-in.
					for (int j = 0; j<PlugInfo.Count; j++)
					{
						// Add audio resources to the cboAudioSource combo box.
						if (PlugInfo.MediaType == WMENC_SOURCE_TYPE.WMENC_AUDIO) 
							cboAudioSource.Items.Add(PlugInfo.Item(j));


					}
				}
			}

			cboAudioSource.SelectedIndex=0;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		#endregion
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frm_AudioConfig));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.Tip = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cboAudioSource = new System.Windows.Forms.ComboBox();
			this.btn_Previous = new System.Windows.Forms.Button();
			this.btn_Next = new System.Windows.Forms.Button();
			this.btn_Finish = new System.Windows.Forms.Button();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.Tip.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Device Options";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(32, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(256, 40);
			this.label2.TabIndex = 1;
			this.label2.Text = "Select the device to use for audio and video in this session.Only currently insta" +
				"lled devices are listed";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 24);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(24, 32);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// Tip
			// 
			this.Tip.Controls.Add(this.label3);
			this.Tip.Controls.Add(this.pictureBox1);
			this.Tip.Location = new System.Drawing.Point(8, 120);
			this.Tip.Name = "Tip";
			this.Tip.Size = new System.Drawing.Size(352, 80);
			this.Tip.TabIndex = 3;
			this.Tip.TabStop = false;
			this.Tip.Text = "Tip";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(56, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(272, 40);
			this.label3.TabIndex = 3;
			this.label3.Text = "The default device is selected automatically. Use the drop down list only if you " +
				"want to change the default device.";
			// 
			// cboAudioSource
			// 
			this.cboAudioSource.Location = new System.Drawing.Point(112, 80);
			this.cboAudioSource.Name = "cboAudioSource";
			this.cboAudioSource.Size = new System.Drawing.Size(248, 21);
			this.cboAudioSource.TabIndex = 4;
			// 
			// btn_Previous
			// 
			this.btn_Previous.Location = new System.Drawing.Point(16, 232);
			this.btn_Previous.Name = "btn_Previous";
			this.btn_Previous.TabIndex = 5;
			this.btn_Previous.Text = "< Previous";
			// 
			// btn_Next
			// 
			this.btn_Next.Location = new System.Drawing.Point(96, 232);
			this.btn_Next.Name = "btn_Next";
			this.btn_Next.TabIndex = 6;
			this.btn_Next.Text = "Next >";
			this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
			// 
			// btn_Finish
			// 
			this.btn_Finish.Location = new System.Drawing.Point(200, 232);
			this.btn_Finish.Name = "btn_Finish";
			this.btn_Finish.TabIndex = 7;
			this.btn_Finish.Text = "Finish";
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.Location = new System.Drawing.Point(288, 232);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.TabIndex = 8;
			this.btn_Cancel.Text = "Cancel";
			this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 80);
			this.label4.Name = "label4";
			this.label4.TabIndex = 9;
			this.label4.Text = "Audio Device";
			// 
			// frm_AudioConfig
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(376, 262);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.btn_Finish);
			this.Controls.Add(this.btn_Next);
			this.Controls.Add(this.btn_Previous);
			this.Controls.Add(this.cboAudioSource);
			this.Controls.Add(this.Tip);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frm_AudioConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Audio Configuration";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frm_AudioConfig_Load);
			this.Tip.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Next Button 
		private void btn_Next_Click(object sender, System.EventArgs e)
		{
			
			config=new ScreenCapture.Config();
			config.cfgFile = "WebMeeting.exe.config"; 
			config.SetValue("//appSettings//add[@key='Driver']", cboAudioSource.Text.ToString());
	
			
			this.Close();
			frm_AudioConfig2 fa2=new frm_AudioConfig2();
			fa2.Show();
			
		}
		#endregion

		private void btn_Cancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frm_AudioConfig_Load(object sender, System.EventArgs e)
		{
		
		}

		

		
	}
}
