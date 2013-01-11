// *****************************************************************************
//  Copyright 2004, Stanley Teo Songling
//  You are free to use or modify it. The author will not be responsible for any 
//	damage caused by this code.
//  
//  I will feel very hornered if my name can appear in your software/source code 
//  Any question or feedback, please email zhangsongling@hotmail.com, thanks 
// 
//  Visual Studio.Net style status bar
// *****************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;	// for dll import
using System.Diagnostics;
using WebMeeting.Client;

using WebMeeting.Common;

namespace WebMeeting.Client.StatusBar
{
	/// <summary>
	/// Summary description for DotNetStatusBar.
	/// </summary>
	public class DotNetStatusBar : System.Windows.Forms.StatusBar
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		#region Private Fields

		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.StatusBarPanel _MessagePanel = null;
		private System.Windows.Forms.StatusBarPanel _CenterImagePanel=null;				
		private System.Windows.Forms.StatusBarPanel _RightMessagePanel=null;		
		private System.Windows.Forms.StatusBarPanel _RightImagePanel=null;		

		
		private Brush textBrush;//=new System.Drawing.SolidBrush(System.Drawing.Color.Gray);	
		private Brush backBrush;
		private Font textFont;
 
		#endregion

		#region Properties

		// To update the X and Y value of the second panel
	

		// To update the message shown in the first panel
		public string Message
		{
			get { 
					try
					{
						return _MessagePanel.Text; 
					}
					catch(Exception ex)
					{
						Trace.WriteLine("Exception at the place of status bar Message error :"+ex.ToString()); 
						ex=ex;
						return null;
					}
				}
			set 
			{
				try
				{
					_MessagePanel.Text = value;

				// redraw?
					int xStart = 0;
					//Graphics g = this.CreateGraphics();
					StatusBarDrawItemEventArgs ea = new StatusBarDrawItemEventArgs(this.CreateGraphics(),Font,
						new Rectangle(xStart,2,_MessagePanel.Width-2,Height),1,DrawItemState.Default,_MessagePanel);
					OnDrawItem(ea);
					//g.Dispose();
				}
				catch(Exception ex)
				{
					Trace.WriteLine("Exception at the place of status bar Message error :"+ex.ToString()); 
					ex=ex;
				}
			}
		}

		public string LeftMessage
		{
			get{
					try
					{
						return this._RightMessagePanel.Text;
					}
					catch(Exception ex)
					{
						Trace.WriteLine("Exception at the place of status bar LeftMessage error :"+ex.ToString()); 
						ex=ex;
						return null;
					}
				}
			set
			{
				try
				{
					_RightMessagePanel.Text=value; // by kamran
					//_RightMessagePanel.Icon = new System.Drawing.Icon("LibraryIcon.ico");
					
					// redraw?
					int xStart = 0;
					//Graphics g = this.CreateGraphics();
					StatusBarDrawItemEventArgs ea = new StatusBarDrawItemEventArgs(this.CreateGraphics(),Font,
						new Rectangle(xStart,2,_MessagePanel.Width-2,Height),1,DrawItemState.Default,this._RightMessagePanel);
     				OnDrawItem(ea);
					
					//_MessagePanel.Icon=new System.Drawing.Icon("LibraryIcon.ico");
					
				}
				catch(Exception ex)
				{
					Trace.WriteLine("Exception at the place of status bar LeftMessage error :"+ex.ToString()); 
					ex=ex;
				}


			}
		}


		#endregion

		#region Constructor/Dispose

		public ComboBox comboMood = new ComboBox();
		
		public Button synchButton = new Button();
		


		~DotNetStatusBar()
		{
			
		}

		public DotNetStatusBar()
		{			
			// This call is required by the Windows.Forms Form Designer.
			
			try
			{
				InitializeComponent();
				textFont=new Font("Arial",8,FontStyle.Bold);

				textBrush=new System.Drawing.SolidBrush(System.Drawing.Color.Gray);	
				backBrush=new System.Drawing.SolidBrush(Info.getInstance().backColor);//.SystemBrushes.InactiveBorder;
				// change the style to double buffering to avoid flickering
				this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | 
					ControlStyles.DoubleBuffer, true);
				ShowPanels = true;
				this.SizingGrip = false;

				// assign the same font as the Visual Studio .Net Statusbar ( on my machine :-> )
				Font = new Font("Arial",8);

				// create all the panels
				_MessagePanel = new System.Windows.Forms.StatusBarPanel();
				_CenterImagePanel=new System.Windows.Forms.StatusBarPanel();
				_RightMessagePanel=new System.Windows.Forms.StatusBarPanel();
				_RightImagePanel=new System.Windows.Forms.StatusBarPanel();

			
			
				Panels.AddRange( new StatusBarPanel[] {_MessagePanel,_CenterImagePanel,_RightMessagePanel});
			
	
				// calculate a proper height according the height of the font
				Size = new System.Drawing.Size(292, FontHeight + 7);

				// calculate the width of the overwrite/insert/capslock panel
				Graphics g = this.CreateGraphics();
				string [] aStr = new string[]{ "CAP","INS","OVR","NUM" };
				float fMax = 0f;
				foreach(string str in aStr)
				{
					System.Drawing.SizeF size = g.MeasureString(str,Font);
					if( size.Width > fMax )
						fMax = size.Width;
				}
				int width = Convert.ToInt32( fMax + 2.5f);

				// _MessagePanel
				_MessagePanel.BorderStyle = StatusBarPanelBorderStyle.None;
				_MessagePanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
				_MessagePanel.Width = 10;
				_MessagePanel.Style = StatusBarPanelStyle.OwnerDraw;
				_MessagePanel.Text = "Ready";
				// _CenterImagePanel
				_CenterImagePanel.BorderStyle=StatusBarPanelBorderStyle.None;
				_CenterImagePanel.AutoSize=System.Windows.Forms.StatusBarPanelAutoSize.Spring;
				_CenterImagePanel.Width=100;
				_CenterImagePanel.Alignment= HorizontalAlignment.Center;


				_RightMessagePanel.BorderStyle=StatusBarPanelBorderStyle.None;
				_RightMessagePanel.AutoSize=System.Windows.Forms.StatusBarPanelAutoSize.Spring;
				_RightMessagePanel.Text="Connecting";
				//_RightMessagePanel.Width=50;
				_RightMessagePanel.Alignment= HorizontalAlignment.Right;			

				///////_RightImagePanel
				_RightImagePanel.BorderStyle=StatusBarPanelBorderStyle.Raised;
				_RightImagePanel.AutoSize=System.Windows.Forms.StatusBarPanelAutoSize.Spring;
				_RightImagePanel.Width=10;
				_RightImagePanel.Alignment=HorizontalAlignment.Center;
				//_RightImagePanel.Style = StatusBarPanelStyle.OwnerDraw;			
				//this.Controls.Add(synchButton);
				comboMood.Visible=false;
				this.Controls.Add(comboMood);
				comboMood.DropDownStyle = ComboBoxStyle.DropDownList;
				comboMood.Items.Add("Yellow");
				comboMood.Items.Add("Blue");
				comboMood.Items.Add("Green");
				comboMood.Items.Add("Red");
				comboMood.Items.Add("Orange");	

				comboMood.SelectedIndexChanged +=new EventHandler(comboMood_SelectedIndexChanged);
				comboMood.SelectedIndex = 2;

				/*
				synchButton.Text = "Synchronize All";
				synchButton.Click+=new EventHandler(synchButton_Click);
				synchButton.Visible =false;
	*/
				g.Dispose();
			}
			catch(Exception ex)
			{
				Trace.WriteLine("Exception at the place of status bar contructor :"+ex.ToString()); 
				ex=ex;
			}
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

		#endregion

		
		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region Overwrites
	
		protected override void OnDrawItem(StatusBarDrawItemEventArgs sbdevent)
		{
			try
			{
				base.OnDrawItem (sbdevent);

				// Create a StringFormat object to align text in the panel.
				StringFormat sf = new StringFormat();

				// Format the String of the StatusBarPanel to be centered.
				sf.LineAlignment = StringAlignment.Center;
				sf.FormatFlags = StringFormatFlags.NoWrap;
				
				StatusBarPanel panel = (StatusBarPanel)sbdevent.Panel;
				if( panel.Alignment == HorizontalAlignment.Center )
					sf.Alignment = StringAlignment.Center;
				else if( panel.Alignment == HorizontalAlignment.Left )
					sf.Alignment = StringAlignment.Near;
				else
					sf.Alignment = StringAlignment.Far;

				
				if(panel==_CenterImagePanel)
				{
					Rectangle rect = sbdevent.Bounds;							
					
					Graphics g = sbdevent.Graphics;
					lock(this)
					{
				
						g.FillRectangle(this.backBrush,sbdevent.Bounds.X -2,sbdevent.Bounds.Y,sbdevent.Bounds.Width+2,sbdevent.Bounds.Height);
						//g.DrawRectangle(SystemPens.ControlDark,rect);
					
						g.DrawString("F L E X C O L L A B",this.textFont,textBrush,sbdevent.Bounds,sf);				
					}
				}
				else if(panel==_RightMessagePanel)
				{
					Rectangle rect = sbdevent.Bounds;							
					
					Graphics g = sbdevent.Graphics;
					lock(this)
					{
				
						g.FillRectangle(this.backBrush,sbdevent.Bounds.X -2,sbdevent.Bounds.Y,sbdevent.Bounds.Width+2,sbdevent.Bounds.Height);
						g.DrawString(_RightMessagePanel.Text,this.textFont,textBrush,sbdevent.Bounds,sf);
					}
					comboMood.Left = sbdevent.Bounds.Left  +100;
					Brush _pen;
					int cor=100;
					if(_RightMessagePanel.Text.Trim()=="Connected")
					{
						_pen=new SolidBrush(Color.Green);
						//WebMeeting.Client.ClientUI.getInstance().NetworkStatus(false);
					}
					else
					{
						_pen=new SolidBrush(Color.Red);
						//WebMeeting.Client.ClientUI.getInstance().NetworkStatus(true);
						cor=120;
					}

					lock(this)
					{
				
						g.FillEllipse(_pen,this.Right-cor,3,14,14);
						g.DrawString("",this.textFont,textBrush,comboMood.Left-5, comboMood.Top +10,sf);
					}
		
				}
				else if(panel==_RightImagePanel)
				{
					Rectangle rect = sbdevent.Bounds;							
					
					Graphics g = sbdevent.Graphics;
					//g.FillRectangle(this.backBrush,sbdevent.Bounds.X -2,sbdevent.Bounds.Y,sbdevent.Bounds.Width+2,sbdevent.Bounds.Height);
					//g.DrawRectangle(SystemPens.ControlDark,rect);
					//System.Drawing.Pen _pen=new Pen(Color.Red);
					//g.DrawEllipse(_pen,0,0,10,10);
				}
				else
				{
					
					//synchButton.Left = sbdevent.Bounds.Right - (synchButton.Width);
					Rectangle rect = sbdevent.Bounds;
					Graphics g = sbdevent.Graphics;
					lock(this)
					{
				
						g.DrawString(panel.Text,this.textFont,this.textBrush,sbdevent.Bounds,sf);
					}
				}

			}
			catch(Exception ex)
			{
				Trace.WriteLine("Exception at the place of status bar ondraw:"+ex.ToString()); 
				ex=ex;
			}
				

		}

		protected override void OnPaint(PaintEventArgs e)
		{
			// paint all the panels
			try
			{

				int xStart = 0;
				OnDrawItem(new StatusBarDrawItemEventArgs(e.Graphics,Font,
					new Rectangle(xStart,2,_MessagePanel.Width,Height),1,DrawItemState.Default,_MessagePanel));
				xStart+=_MessagePanel.Width;

				OnDrawItem(new StatusBarDrawItemEventArgs(e.Graphics,Font,
					new Rectangle(xStart,2,_CenterImagePanel.Width,Height),1,DrawItemState.Default,_CenterImagePanel));
				xStart+=_CenterImagePanel.Width;

				OnDrawItem(new StatusBarDrawItemEventArgs(e.Graphics,Font,
             		new Rectangle(xStart,2,_RightMessagePanel.Width,Height),1,DrawItemState.Default,_RightMessagePanel));
				xStart+=_RightMessagePanel.Width;
				OnDrawItem(new StatusBarDrawItemEventArgs(e.Graphics,Font,
					new Rectangle(xStart,2,_RightImagePanel.Width,Height),1,DrawItemState.Default,_RightImagePanel));
				// draw the resize grip
				if( this.SizingGrip == true )
					ControlPaint.DrawSizeGrip(e.Graphics,SystemColors.Control,new Rectangle(this.Width-17,this.Height - 17,
						16,16));

				base.OnPaint (e);
			}
			catch(Exception ex)
			{
				Trace.WriteLine("Exception at the place of status bar onpaint :"+ex.ToString()); 
				ex=ex;
			}
		}
	
		protected override void OnResize(EventArgs e)
		{
			// test if the size is near maximuized, disable the sizing grip
			try
			{
				if( SystemInformation.WorkingArea.Width <= Width )
					this.SizingGrip = false;
				else
					this.SizingGrip = true;
		
				base.OnResize (e);
			}
			catch(Exception ex)
			{
				Trace.WriteLine("Exception at the place of status bar onpaint :"+ex.ToString()); 
				ex=ex;
			}
		}

		#endregion

		private void comboMood_SelectedIndexChanged(object sender, EventArgs e)
		{	
			try
			{
				int nIndex = 12;
				if(comboMood.SelectedIndex == 0)
					nIndex = 10;
				else if(comboMood.SelectedIndex == 1)
					nIndex =  11;
				else if(comboMood.SelectedIndex == 2)
					nIndex = 12;
				else if(comboMood.SelectedIndex == 3)
					nIndex =  13;
				else if(comboMood.SelectedIndex == 4)
					nIndex = 14;						
	            
				NetworkManager.getInstance().profile.m_MoodIndicatorColor =(MoodIndicator) nIndex;

				
			

				ControlMessage m = new ControlMessage(ControlCode.AttendeeUpdate);
				m.SenderID = -1;//network.profile.ClientId;
				m.Parameter = NetworkManager.getInstance().profile;
				NetworkManager.getInstance().SendLoadPacket(m);
				//Trace.WriteLine("line 371 dotnetstatusbar message of attendee update");		
				NetworkManager.getInstance().myRecvMsgList.Add(m);			
			
			}
			catch(Exception ex)
			{
				Trace.WriteLine("Exception at the place of status bar comboMood_SelectedIndexChanged :"+ex.ToString()); 
				ex=ex;
			}
		}

		private void synchButton_Click(object sender, EventArgs e)
		{
			SynchornizeMesssage msg = new SynchornizeMesssage( NetworkManager.getInstance().profile.ClientId);
			/*
			Crownwood.Magic.Controls.TabPage page =  ClientUI.getInstance().tabBody.SelectedTab;
			System.Type a = page.Control.GetType();
			*/            
		}		
	}
}
