using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for rightcarrotWindow.
	/// </summary>
	public class rightcarrotWindow : System.Windows.Forms.Form
	{
		public Button SelectedButton;
		private int lastButtonTop=8;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public rightcarrotWindow()
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
			// 
			// rightcarrotWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(192, 240);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "rightcarrotWindow";
			this.Text = "rightcarrotWindow";

		}
		#endregion
		public Button AddButton(string strText)
		{
			Button bTemp = new Button();
			bTemp.Left = 8;
			bTemp.Width = 176;			
            bTemp.Top = lastButtonTop;
			bTemp.Height = 23;			
			bTemp.Text = strText;
			bTemp.Click += new EventHandler(Button_ClickEvent);
			bTemp.MouseEnter += new EventHandler(bTemp_MouseEnter);
			bTemp.MouseLeave += new EventHandler(bTemp_MouseLeave);
			lastButtonTop = lastButtonTop+ 23;
			bTemp.FlatStyle = FlatStyle.Popup;
			this.Controls.Add(bTemp);
			return bTemp;
		}
		public void InheritFromMenu(ContextMenu menu)
		{
			this.Controls.Clear();
			for(int i = 0 ; i < menu.MenuItems.Count ; i ++)
			{
				MenuItem m = menu.MenuItems[i];
				if((m.Text != "") || (m.Text != "Mood"))
				{					
					AddButton(m.Text).Enabled = m.Enabled;					
				}
			}		

		}
		/*
		public void ChangeStates(ClientProfile Profile)
		{
			#region e
					
				if(Profile.ClientId == network.profile.ClientId)
				{													

					if(ClientUI.getInstance().videoEnabled)
						ChangeButtonText("Share My Video","Disable Video");
					else
						ChangeButtonText("Disable Video","Share My Video");

					if(ClientUI.getInstance().audioEnabled)
						ChangeButtonText("Start Audio","Disable Audio");
					else
						ChangeButtonText("Disable Audio","Start Audio");
						
					if(network.profile.clientType != ClientType.ClientAttendee)
					{
						if(network.profile.clientType == ClientType.ClientHost)
						{
							Controls[0].Enabled = true;
							Controls[1].Enabled = true;
						}
						else
						{
							Controls[0].Enabled = NetworkManager.getInstance().profile.clientAccess.accessShareVideo ; 
							Controls[1].Enabled =  NetworkManager.getInstance().profile.clientAccess.accessShareAudio ;
						}
					}
					else
					{
						Controls[0].Enabled  = false;                        
						//contextMenuParticipents.MenuItems[1].Enabled = false;
					}


					bool bValue = false;
					for(int i= 2 ; i < 10; i ++)
						Controls[i].Enabled = bValue;						
						
				}
				else
				{
					
					ChangeContextMenuItemState(menuMoodIndicator,false);					
					Controls[0].Enabled = false;                        
					Controls[1].Enabled = false;

					Controls[2].Enabled = true;                        
					Controls[3].Enabled = true;                        
						
					Controls[4].Enabled  = false;
					//contextMenuParticipents.MenuItems[4].Enabled = (Profile.audioID > 0);                        						
					Controls[5].Enabled = (Profile.videoID> 0); 
					if(ClientUI.getInstance().CheckAlreadyVideoSessionAvailable(Profile))
					{
						Controls[5].Enabled = false;						
					}
					else
						contextMenuParticipents.MenuItems[5].Checked = false;
						
					if(network.profile.clientType == ClientType.ClientAttendee)
					{
						Controls[6].Enabled = false;
						Controls[6].Checked= false;
						Controls[7].Enabled = false;
								
						//ChangeMenuItemState(
					}
					else
					{
						#region Presenter
						if(NetworkManager.getInstance().profile.clientType == ClientType.ClientPresenter) // if its a presenter
						{
							if(NetworkManager.getInstance().profile.clientAccess.accessDesktopSharing != true) // he doesn't have access of sharing desktop
							{
								Controls[6].Enabled = false;                        								
							}
							else
							{ // he has the sharing desktop access .. check if there is already a session

								bool bVal = false;
								if(desktopSharedProfile== null)
									bVal = true;						
								Controls[6].Enabled = bVal;                        								
							}						
							    
						}			
							#endregion
						else if(network.profile.clientType == ClientType.ClientHost) // if its a presenter
						{																				
							
							/*ChangeContextMenuItemState(contextMenuDesktop,(desktopSharedProfile== null));                        

							ChangeContextMenuItemState(contextmenuOptions,true);

							contextMenuParticipents.MenuItems[7].Enabled= true;	*/							
							/*
																
						}
					
					}

			}
			
			#endregion

		}
		*/
		public void ChangeButtonText(string strOldText,string strNewText)
		{
			for(int i = 0; i < this.Controls.Count; i++)
			{
				Control ctrl = Controls[i];
				if(ctrl.GetType().Equals(typeof(Button)))
				{
					Button btn = (Button)ctrl;
					if(btn.Text == strOldText)
					{
						btn.Text = strNewText;
						break;
					}

				}
			}
		}

		private void Button_ClickEvent(object sender, EventArgs e)
		{
			SelectedButton = (Button)sender;
		}

		private void bTemp_MouseLeave(object sender, EventArgs e)
		{
			((Button)sender).BackColor = this.BackColor;

		}

		private void bTemp_MouseEnter(object sender, EventArgs e)
		{
			((Button)sender).BackColor = Color.LightCyan;
		}

	}
}
