using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WebMeeting
{
	public delegate void DelegateKeyPress(KeyPressEventArgs e);
	public delegate void DelegateKeyActivity(KeyEventArgs e);	
	public delegate void DelegateOnScroll(int nPosition,DataType nType);
	public class Custom : PictureBox
	{
		public event DelegateKeyActivity OnKeyDownEvent;
		public event DelegateKeyActivity OnKeyUpEvent;
		public event DelegateKeyPress OnKeyPressEvent;
		
		//public event DelegateOnScroll OnScroll;

		public Custom()
		{

		}
		private void FireKeyUp(KeyEventArgs e)
		{
			if(OnKeyUpEvent != null)
				OnKeyUpEvent(e);
		}
		private void FireKeyDown(KeyEventArgs e)
		{
			if(OnKeyDownEvent != null)
				OnKeyDownEvent(e);
		}
		private void FireKeyPress(KeyPressEventArgs e)
		{
			if(OnKeyPressEvent != null)
				OnKeyPressEvent(e);
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			FireKeyDown(e);
			base.OnKeyDown(e);
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			FireKeyUp(e);
			base.OnKeyUp (e);
		}
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			FireKeyPress(e);
			base.OnKeyPress(e);
		}

		
		protected override void OnMouseDown(MouseEventArgs e)
		{			
			Focus();
			
			base.OnMouseDown (e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick (e);
		}


	}
}
