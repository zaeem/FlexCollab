using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace WebMeeting.Client.AppSharing
{
	/// <summary>
	/// Summary description for SmallWindow.
	/// </summary>
	public class SharedWindowHostFrm: System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>
		/// Handle to the application Window
		/// </summary>
		public IntPtr appWin;
		public IntPtr SharedWindowParent;
		public IntPtr AppShareWindow;/*for sending hotkey message*/
		public bool bIsCloseAllowed=false;/*flag used in wndproc for close*/
		public bool bIsWM_SYSKEYUP=false;
		public Win32.USER32.RECT rectShareWindow = new Win32.USER32.RECT();/*for storing previous coordinats of the shared window*/
		
		
		public int oldWinStyle;/*WM_CLOSE Message*/
		public CloseAppShare appClose;
		private System.Windows.Forms.LinkLabel linkLabel2;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.LinkLabel btnCloseAppShare;
		public TakeAnnotationRightsBack deltakeback;
		
		
		

		/// <summary>
		/// Force redraw of control when size changes
		/// </summary>
		/// <param name="e">Not used</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			try
			{
				this.Invalidate();
				base.OnSizeChanged (e);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare protected override void OnSizeChanged(EventArgs e)",ex,"",false);				
			}
		}
		
		/// <summary>
		/// Track if the application has been created
		/// </summary>
		bool created = false;

		public SharedWindowHostFrm()
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public SharedWindowHostFrm()",ex,"",false);
			}
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		
		public SharedWindowHostFrm(int width,int height)
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();
				this.Size=new Size(width,height);		
				//
				// TODO: Add any constructor code after InitializeComponent call
				//
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public SharedWindowHostFrm(int width,int height)",ex,"",false);
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
		protected override void WndProc(ref Message m)
		{
			try
			{
				switch(m.Msg)
				{
					case Win32.USER32.WM_CLOSE:
						if(bIsCloseAllowed==true)
						{
							base.WndProc(ref m);
						}
						break;
					case Win32.USER32.USER_CLOSE_FORM:
						bIsCloseAllowed=true;
						if(Win32.USER32.IsWindow(this.appWin))
						{
							
							// when u call ApplicationSharing_appShareClose
							// it restores the application which is shared to its previous position  
							//RestoreShareWindowParent();
							// Move window to its previous position 
							Win32.USER32.MoveWindow(appWin, rectShareWindow.left,rectShareWindow.top, rectShareWindow.right, rectShareWindow.bottom, true);
							// restore the pold style of window 
							Win32.USER32.SetWindowLong(appWin, Win32.USER32.GWL_STYLE,oldWinStyle);
							// Move the window to overlay it on this window
							Win32.USER32.MoveWindow(appWin, rectShareWindow.left,rectShareWindow.top, rectShareWindow.right, rectShareWindow.bottom, true);

						}
						this.Close();
						base.WndProc(ref m);

						break;

					case Win32.USER32.WM_SYSKEYUP:
						//mark boolean variables to indentify at WM_KEYUP Message that WM_SYSKEYUP just come before
						bIsWM_SYSKEYUP=true;
						break;
					case Win32.USER32.WM_KEYUP:
						if(this.bIsWM_SYSKEYUP)
						{
							SendFocusToSharedWindow();
							bIsWM_SYSKEYUP=false;
						}
						break;
						//check bool variable, if its then focus the shared window
//					case WM_CAPTURECHANGED:
//						SendFocusToSharedWindow();
//						Trace.WriteLine("WM_CAPTURECHANGED");
//						break;
//					case WM_NCACTIVATE:
////						SendFocusToSharedWindow();
////						Trace.WriteLine("WM_CAPTURECHANGED");
//						break;
					default:
						base.WndProc(ref m);                
						break;
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare protected override void WndProc(ref Message m)",ex,"",false);
			}
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SharedWindowHostFrm));
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.btnCloseAppShare = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// linkLabel2
			// 
			this.linkLabel2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.linkLabel2.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
			this.linkLabel2.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.linkLabel2.LinkColor = System.Drawing.Color.White;
			this.linkLabel2.Location = new System.Drawing.Point(368, 8);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(192, 17);
			this.linkLabel2.TabIndex = 4;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "Minimize  Shared  Application";
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			// 
			// linkLabel1
			// 
			this.linkLabel1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.linkLabel1.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
			this.linkLabel1.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.linkLabel1.LinkColor = System.Drawing.Color.White;
			this.linkLabel1.Location = new System.Drawing.Point(8, 8);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(167, 17);
			this.linkLabel1.TabIndex = 2;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Switch to Annoation Mode";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// btnCloseAppShare
			// 
			this.btnCloseAppShare.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.btnCloseAppShare.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
			this.btnCloseAppShare.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.btnCloseAppShare.LinkColor = System.Drawing.Color.White;
			this.btnCloseAppShare.Location = new System.Drawing.Point(192, 8);
			this.btnCloseAppShare.Name = "btnCloseAppShare";
			this.btnCloseAppShare.Size = new System.Drawing.Size(167, 17);
			this.btnCloseAppShare.TabIndex = 1;
			this.btnCloseAppShare.TabStop = true;
			this.btnCloseAppShare.Text = "Close Application Sharing";
			this.btnCloseAppShare.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnCloseAppShare_LinkClicked);
			// 
			// SharedWindowHostFrm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.ClientSize = new System.Drawing.Size(1024, 768);
			this.Controls.Add(this.linkLabel2);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.btnCloseAppShare);
			this.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(174)), ((System.Byte)(239)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SharedWindowHostFrm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CloseApplicationShare";
			this.Load += new System.EventHandler(this.SharedWindowHostFrm_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.SharedWindowHostFrm_Paint);
			this.ResumeLayout(false);

		}
		#endregion

		public void closeForm()
		{
			try
			{
				this.bIsCloseAllowed=true;
				try
				{
					BeginInvoke(this.appClose);
				}
				catch(Exception exp){}
				if(Win32.USER32.IsWindow(this.appWin))
				{
					//RestoreShareWindowParent();
					//SetWindowLong(appWin, GWL_STYLE,oldWinStyle);
					// Move the window to overlay it on this window
				
					//MoveWindow(appWin, rectShareWindow.Left,rectShareWindow.Top, rectShareWindow.Right, rectShareWindow.Bottom, true);

				}
				this.Close();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void closeForm()",ex,"",false);
			}
		}
		public void btnCloseAppShare_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				closeForm();
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare public void btnCloseAppShare_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)",ex,"",false);
			}
		}
		
		/// <summary>
		/// to resote the shared window parent 
		/// 
		/// 
		/// </summary>
		private void RestoreShareWindowParent()
		{
			try
			{
				//SetParent(this.appWin,SharedWindowParent);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private void RestoreShareWindowParent()",ex,"",false);				
			}
		}
		protected override void OnVisibleChanged(EventArgs e)
		{
			try
			{
				// If control needs to be initialized/created
				if (created == false)
				{

				
					Win32.USER32.GetWindowRect(this.appWin, ref rectShareWindow);
					int style = Win32.USER32.GetWindowLong(this.appWin , Win32.USER32.GWL_STYLE);
					// Mark that control is created
					created = true;

	
					//get parent of the shared window when app share stop its replace it
					this.SharedWindowParent=Win32.USER32.GetParent(appWin);

					oldWinStyle = style;
					// Put it into this form
					// Remove border and whatnot								
					Win32.USER32.SetWindowLong(appWin,Win32.USER32.GWL_STYLE, style);
					//SetParent(appWin,this.Handle);

				
				
					if ( (style & Win32.USER32.WS_MINIMIZEBOX) == Win32.USER32.WS_MINIMIZEBOX)
					{
						style   &= ~Win32.USER32.WS_MINIMIZEBOX;
					}
					//				if ( (style & WS_MAXIMIZEBOX) == WS_MAXIMIZEBOX)
					//				{
					//					style   &= ~WS_MAXIMIZEBOX;
					//				}

					//	

					// Move the window to overlay it on this window
					//MessageBox.Show(this.Left.ToString());
					//MessageBox.Show(this.Top.ToString());
					


					Win32.USER32.MoveWindow(appWin, this.Left+40,this.Top+60, this.Width-80, this.Height-80, true);

					// Remove border and whatnot				
					Win32.USER32.SetWindowLong(appWin,Win32.USER32.GWL_STYLE, style);
				
				

					// Move the window to overlay it on this window
					Win32.USER32.MoveWindow(appWin, this.Left+40,this.Top+60, this.Width-80, this.Height-80, true);
					Win32.USER32.SetForegroundWindow(appWin);
				
				}
				//if(created)
				//MoveWindow(appWin, 10, 10, this.Width-100, this.Height-100, true);
				base.OnVisibleChanged (e);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare protected override void OnVisibleChanged(EventArgs e)",ex,"",false);
			}
		}

		private void SendFocusToSharedWindow()
		{
			if (this.appWin != IntPtr.Zero)
			{
				
				//MoveWindow(appWin, 30, 30, this.Width-80, this.Height-80, true);
				Win32.USER32.MoveWindow(appWin, this.Left+40,this.Top+60, this.Width-80, this.Height-80, true);

				Win32.USER32.SetForegroundWindow(appWin);
			}

		}

		protected override void OnResize(EventArgs e)
		{
			try
			{
				SendFocusToSharedWindow();
				base.OnResize (e);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare protected override void OnResize(EventArgs e)",ex,"",false);
			}
		}

		private void linkLabel2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				this.WindowState= FormWindowState.Minimized;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private void linkLabel2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)",ex,"",false);
			}
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				//this.TopMost=false;
				//this.WindowState=FormWindowState.Minimized;
				// 1 Handle of the window to whom u wasna send messg
				// 2 Type of Message 
				// 3 Message value (high parametes)
				// 4 Message value (Low parametes)
				Win32.USER32.SendMessage(this.AppShareWindow,Win32.USER32.WM_HOTKEY,0,7471104);
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: AppShare private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)",ex,"",false);
			}
		}

		private void SharedWindowHostFrm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Invoke(this.deltakeback);
			SendFocusToSharedWindow();
		}

		private void SharedWindowHostFrm_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
