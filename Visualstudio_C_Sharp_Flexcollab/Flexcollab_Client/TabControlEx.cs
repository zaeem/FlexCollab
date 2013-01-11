using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;

namespace WebMeeting.Client
{
	/// <summary>
	/// Summary description for TabControlEx.
	/// </summary>
	public class TabControlEx : Crownwood.Magic.Controls.TabControl
	{
		int iCurrentTab, iGate;// As Integer
		public delegate bool DelegateSelectionChanged(int nNewTabPageIndex );
		public event DelegateSelectionChanged OnSelectionChangingEx;

		public bool bAllowIt;
		public Crownwood.Magic.Controls.TabPage SelectedTabEx
		{

			set
			{
				bAllowIt = true;
				SelectedTab = value;

			}
			get
			{
				return SelectedTab;
			}
		}
		public override void OnSelectionChanging(EventArgs e)
		{
						
			base.OnSelectionChanging (e);
		}

		public override void OnSelectionChanged(EventArgs e)
		{	
















			if(bAllowIt)
			{


				bAllowIt=false;
				base.OnSelectionChanged(e);
				iCurrentTab = SelectedIndex;
				

				return;
			}


			if(iGate == 0)//
			{
				iGate+=1;
				if(!OnSelectionChangingEx(SelectedIndex))
				{
					SelectedIndex = iCurrentTab;
					
				}
				else
				{
					base.OnSelectionChanged(e);
					iCurrentTab = this.SelectedIndex;
					
				}
			}	
			iGate = 0;
			

		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TabControlEx(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public TabControlEx()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
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
	}
}

