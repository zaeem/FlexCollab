using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using WebMeeting.Common;
using Belikov.GenuineChannels;

namespace WebMeeting.Client.NetworkDiagonstic
{
	/// <summary>
	/// Summary description for TestNetwork.
	/// </summary>
	public class TestNetwork : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnTestNetwork;
		private System.Windows.Forms.TextBox txtnetworkdelay;
		private System.Windows.Forms.TextBox txtMessageLength;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtnolenght;
		private System.Windows.Forms.Button btnclose;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblstatus;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TestNetwork()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TestNetwork));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnTestNetwork = new System.Windows.Forms.Button();
			this.txtnetworkdelay = new System.Windows.Forms.TextBox();
			this.txtMessageLength = new System.Windows.Forms.TextBox();
			this.txtnolenght = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btnclose = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.lblstatus = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Message Length";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(144, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Network Relay  in sec";
			// 
			// btnTestNetwork
			// 
			this.btnTestNetwork.Location = new System.Drawing.Point(192, 136);
			this.btnTestNetwork.Name = "btnTestNetwork";
			this.btnTestNetwork.Size = new System.Drawing.Size(152, 24);
			this.btnTestNetwork.TabIndex = 2;
			this.btnTestNetwork.Text = "Test Network Connectivity";
			this.btnTestNetwork.Click += new System.EventHandler(this.btnTestNetwork_Click);
			// 
			// txtnetworkdelay
			// 
			this.txtnetworkdelay.Enabled = false;
			this.txtnetworkdelay.Location = new System.Drawing.Point(160, 64);
			this.txtnetworkdelay.Name = "txtnetworkdelay";
			this.txtnetworkdelay.Size = new System.Drawing.Size(224, 20);
			this.txtnetworkdelay.TabIndex = 3;
			this.txtnetworkdelay.Text = "";
			// 
			// txtMessageLength
			// 
			this.txtMessageLength.Enabled = false;
			this.txtMessageLength.Location = new System.Drawing.Point(160, 40);
			this.txtMessageLength.Name = "txtMessageLength";
			this.txtMessageLength.Size = new System.Drawing.Size(224, 20);
			this.txtMessageLength.TabIndex = 4;
			this.txtMessageLength.Text = "20";
			// 
			// txtnolenght
			// 
			this.txtnolenght.Enabled = false;
			this.txtnolenght.Location = new System.Drawing.Point(160, 16);
			this.txtnolenght.Name = "txtnolenght";
			this.txtnolenght.Size = new System.Drawing.Size(224, 20);
			this.txtnolenght.TabIndex = 6;
			this.txtnolenght.Text = "10";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(144, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "No of Messages to Send";
			// 
			// btnclose
			// 
			this.btnclose.Location = new System.Drawing.Point(32, 136);
			this.btnclose.Name = "btnclose";
			this.btnclose.Size = new System.Drawing.Size(152, 24);
			this.btnclose.TabIndex = 7;
			this.btnclose.Text = "Close";
			this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(144, 16);
			this.label4.TabIndex = 8;
			this.label4.Text = "Status";
			// 
			// lblstatus
			// 
			this.lblstatus.Location = new System.Drawing.Point(160, 96);
			this.lblstatus.Name = "lblstatus";
			this.lblstatus.Size = new System.Drawing.Size(240, 32);
			this.lblstatus.TabIndex = 9;
			// 
			// TestNetwork
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(416, 165);
			this.Controls.Add(this.lblstatus);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnclose);
			this.Controls.Add(this.txtnolenght);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtMessageLength);
			this.Controls.Add(this.txtnetworkdelay);
			this.Controls.Add(this.btnTestNetwork);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "TestNetwork";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Test Network";
			this.ResumeLayout(false);

		}
		#endregion
		
		int StartTickCount;
		int LastTickCount;

		private void btnTestNetwork_Click(object sender, System.EventArgs e)
		{
			this.btnTestNetwork.Enabled=false;
			//store start tick count
			
			try
			{		
				int messageTextLength=Convert.ToInt32(this.txtMessageLength.Text.Trim());
				string msgtext=NetworkDiagonstic.GenerateString.Generate(messageTextLength,messageTextLength);
				string msgout="";				
				StartTickCount=System.Environment.TickCount;			
				for(int i=0;i<Convert.ToInt32(this.txtnolenght.Text);i++)
				{										
					msgout=NetworkManager.getInstance().IConferenceRoom.UserDefinedPing(msgtext);
					//((IConferenceRoom)NetworkManager.getInstance()._dispatcher.TransparentProxy).UserDefinedPing(msgtext);					
				}
				this.lblstatus.Text="Network Message Send & Receive Successfully";
				//tick count at the end
				LastTickCount=System.Environment.TickCount;
				//network delay will be get by sub startTickCount from LastTickCount.
				this.txtnetworkdelay.Text=Convert.ToString((LastTickCount-this.StartTickCount)/1000);
			}
			catch(Exception ex)
			{
				if (ex is OperationException)
				{
					OperationException operationException = (OperationException) ex;
					//MessageBox.Show("Genuine Channels Error");
					MessageBox.Show(this, operationException.OperationErrorMessage.UserFriendlyMessage, operationException.OperationErrorMessage.ErrorIdentifier, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
					MessageBox.Show(this, ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.lblstatus.Text="Problem in sending/Receiving messages";
				//if(ex is
			}	
			this.btnTestNetwork.Enabled=true;				
		}

		private void btnclose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
