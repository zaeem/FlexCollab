using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using WebMeeting.Client;
using WebMeeting.Common;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Diagnostics;


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*  Whiteboard is a Generic control, which can be used for all the modules  
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


namespace WebMeeting.Client.WhiteBoard
{




	#region Drawing tools Enumeration
	/// <summary>
	/// Drawing tools Enumeration
	/// </summary>
	[Serializable]
	public enum enum_WhiteboardToolCode
	{
		WhiteboardToolMin, 
		Pencil,
		Eraser,
		Brush,
		Line,
		Ellipse,
		Rectangle,		
		None,
		Text,
		UniArrow,
		Undo,
		Redo,
		PlaceHolderArrow,
		WhiteboardToolMax
	}
	#endregion 


	#region Module Enumeration
	public enum enum_Module
	{
	Whiteboard,
	Appshare,
	Webshare,
	Docshare
	}
	#endregion 


	/// <summary>
	/// Whiteboard Control is the control designed for all the module .
	/// </summary>
	/// 




	public class Whiteboard : System.Windows.Forms.UserControl
	{
		public System.Windows.Forms.PictureBox _picBox;

		// This pool  contains the annotation messages data structures 
		public ArrayList  MsgPool_SendAnnotation;
		public  ArrayList MsgPool_Annotation;
		
		public  ArrayList MsgPool_Clear;
		public  ArrayList test;
		public  bool flag_SendingThread=true;
		public  bool flag_ConsumeThread=true;
		public  bool m_bIsActive=true;
		
		#region Font related variables 
		public FontStyle constFont;
		public Font constmyFont;
		private FontStyle SelfFont;
		private Font localCopyOfFont;
		# endregion 	
		
		#region Enumeration object declaration 
		private enum_WhiteboardToolCode tool;	
		private enum_Module module;	
		#endregion 


		private Common.MsgAnnotation_Generic msg;
		private Common.MsgAnnotation_Generic message;
		private static Graphics g;
		private Image Global_image;
		//private Image Global_image;
		

		private System.Windows.Forms.TextBox textBox1;		


		private struct ARROWSTRUCT 
		{
			public int nWidth;		// width (in pixels) of the full base of the arrowhead
			public float fTheta;	// angle (in radians) at the arrow tip between the two
			//  sides of the arrowhead
			public bool bFill;		// flag indicating whether or not the arrowhead should be
		} ;

		private ARROWSTRUCT pA;
		public Color color;
		private Rectangle eraser;
		

		private Point mouseFirstPos_OnDown;
		private Point mouseLastPos_OnDown;
		private Point mouseDownPos;
		private Point mouseCurrentPos;
		private Point pencilLastPoint;
		private Point lastPoints;

		
		private System.ComponentModel.Container components = null;
		



		private int slideNo=0;
		private int lineThickness;
		private int brushThickness;
		public int nSessionId =0;

		


		public Whiteboard()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			SelfFont=new FontStyle();
			SelfFont=SelfFont|FontStyle.Bold;
			localCopyOfFont = new Font(this.Font.FontFamily,this.Font.Size,SelfFont);

				

			//Self BLock 
			mouseCurrentPos=new Point(0,0);
			mouseFirstPos_OnDown=new Point(0,0);
			mouseLastPos_OnDown=new Point(0,0);
			mouseDownPos=new Point(0,0);
			
			pencilLastPoint=new Point(0,0);
			lastPoints=new Point(0,0);

			// Hard coded thickness
			brushThickness=8;
			eraser=new Rectangle(0,0,10,10);
				
			
			// For Arrow
			pA.nWidth = 10;
			pA.fTheta = 0.3f;
			pA.bFill = true;



			constFont=new FontStyle();
			constFont=constFont|FontStyle.Bold;
			constmyFont = new Font(this.Font.FontFamily,this.Font.Size,constFont);

			MsgPool_Annotation=new ArrayList();
			MsgPool_Annotation=(ArrayList)ArrayList.Synchronized(MsgPool_Annotation);

			MsgPool_SendAnnotation=new ArrayList();
			MsgPool_SendAnnotation=(ArrayList)ArrayList.Synchronized(MsgPool_SendAnnotation);

			MsgPool_Clear=new ArrayList();
			test=new ArrayList();
			msg=new MsgAnnotation_Generic();
			
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
			this._picBox = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// _picBox
			// 
			this._picBox.Location = new System.Drawing.Point(0, 0);
			this._picBox.Name = "_picBox";
			this._picBox.Size = new System.Drawing.Size(576, 400);
			this._picBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._picBox.TabIndex = 0;
			this._picBox.TabStop = false;
			
			this._picBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this._picBox_MouseUp);
			this._picBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this._picBox_MouseMove);
			this._picBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this._picBox_MouseDown);
			
			// 
			// textBox1
			// 
			this.textBox1=new TextBox();
			this.textBox1.Location = new System.Drawing.Point(80, 136);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(16, 21);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "";
			this.textBox1.Visible = false;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
			this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);




			// 
			// Whiteboard
			// 
			this.AllowDrop = true;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.White;
			this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "Whiteboard";
			this.Size = new System.Drawing.Size(624, 432);
			this.ResumeLayout(false);


			this.Controls.Add(this._picBox);
			this._picBox.Controls.Add(this.textBox1);

		}
		#endregion

		
		# region Set Module like whiteboard , appshare etc
		/// <summary>
		/// pass 
		/// 1 for Whiteboard
		/// 2 for Appshare
		/// 3 for Webshare 
		/// 4 for Docshare
		/// 
		/// </summary>
		/// <param name="inc_module"></param>
		public void setModule(int inc_module)
		{
		
			if(inc_module==1)
			{
				
			module=enum_Module.Whiteboard;
			}
			else if(inc_module==2)
			{
			module=enum_Module.Appshare;
			}

			else if(inc_module==3)
			{
				module=enum_Module.Webshare;
			}

			else if(inc_module==4)
			{
				module=enum_Module.Docshare;
			}


		}
		

		#endregion 



		#region Setting the Graphics Object
		/// <summary>
		/// This function does two things 
		/// 1. Set the grphics object to point towards the latest image 
		/// 2. Update the Gloabal Image 
		/// </summary>
		public void setGraphicsobject()
		{
			try
			{
				if(this._picBox.Image!=null)
				{
					g=Graphics.FromImage(this._picBox.Image);
					// if i dont use the clone then the Grphics object will 
					// actually contain the reference to the Image object and when i will try to clean it 
					// it will actually clean the actual object.

					Global_image=(Image)this._picBox.Image.Clone();
				}
			}catch 	(Exception exp)
			{
			MessageBox.Show(exp.Message.ToString()+ "Stack trace "+ exp.StackTrace.ToString());
			}

		}
		#endregion 


		/// <summary>
		/// This method is for those tools which require explicit left click 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
 
		private void _picBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{

			this.flag_SendingThread=true;
			
			try
			{
			
				if(e.Button==MouseButtons.Left)
				{				
					color = NetworkManager.thisInstance.profile.m_AssignedColor;
					tool= (enum_WhiteboardToolCode)ClientUI.getInstance().whiteBoard.tool;
					



					#region Module selection
					if(module==enum_Module.Whiteboard)
					{
						msg.moduleType=Common.enum_Module.Whiteboard;
					}
					else if(module==enum_Module.Appshare)
					{
						msg.moduleType=Common.enum_Module.Appshare;
					}
					else if(module==enum_Module.Webshare)
					{
						msg.moduleType=Common.enum_Module.Webshare;
					}
					
					#endregion 


					// To take the current mouse poisition
					mouseCurrentPos.X=e.X;
					mouseCurrentPos.Y=e.Y;


					
					if((ushort)enum_WhiteboardToolCode.Brush==(ushort)tool ||   (ushort)enum_WhiteboardToolCode.Pencil==(ushort)tool)
					{
						//this.MsgPool_Annotation.Add(msg);
						msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
						msg.SenderID =  NetworkManager.thisInstance.profile.ClientId;
						msg.nSessionId = nSessionId;
						msg.tool=(ushort)tool;
						msg.c=this.color;

						this.MsgPool_Annotation.Add(msg);
						this.MsgPool_SendAnnotation.Add(msg);	
						
						
					}
					else if(((ushort)tool & (ushort)( enum_WhiteboardToolCode.Eraser |enum_WhiteboardToolCode.PlaceHolderArrow)) == (ushort)tool)
					{
						//msg=new MsgAnnotation_Generic();	
						msg.ConferenceID = NetworkManager.thisInstance.profile.ConferenceID ;
						msg.start=new Point(mouseDownPos.X , mouseDownPos.Y); // new for checking why points are not new 
						msg.end=new Point(e.X,e.Y);
						msg.tool=(ushort)tool;
						msg.c=this.color;
						msg.thickness=ClientUI.getInstance().whiteBoard.LineThickness;
						msg.pencilLastPoint=new Point(this.pencilLastPoint.X,this.pencilLastPoint.Y);
						msg.SenderID =  NetworkManager.thisInstance.profile.ClientId;
						
						msg.nSessionId = nSessionId;
						msg.text=NetworkManager.thisInstance.profile.Name;
                    						
						this.MsgPool_Annotation.Add(msg);
						this.MsgPool_SendAnnotation.Add(msg);	
						
						this._picBox.Refresh();
						this._picBox.Update();
						
					}
					else
					{					
						if((ushort)tool!=(ushort)enum_WhiteboardToolCode.Text)
						{
							msg.start=new Point(mouseDownPos.X , mouseDownPos.Y); // check here for new points
							msg.end=new Point(e.X,e.Y);
							msg.tool=(ushort)tool;
							msg.c=this.color;

							//msg.thickness=this.lineThickness;
							msg.thickness=WebMeeting.Client.ClientUI.getInstance().whiteBoard.LineThickness;
							msg.nSessionId = nSessionId;
							msg.SenderID = NetworkManager.thisInstance.profile.ClientId;

							this.MsgPool_Annotation.Add(msg);
							this.MsgPool_SendAnnotation.Add(msg);
							
							//WebMeeting.Client.NetworkManager network = NetworkManager.thisInstance;
							//sendingArrayList.Add(msg);
						}
					
					}
			
				}

			}
			catch(Exception exp)
			{
			
			}
		}

		private void _picBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				
				if(e.Button==MouseButtons.Left  )
				{		
					
					Thread.Sleep(10);
					color = NetworkManager.thisInstance.profile.m_AssignedColor;
					tool= (enum_WhiteboardToolCode)ClientUI.getInstance().whiteBoard.tool;
					
					#region Module selection
					if(module==enum_Module.Whiteboard)
					{
						msg.moduleType=Common.enum_Module.Whiteboard;
					}
					else if(module==enum_Module.Appshare)
					{
						msg.moduleType=Common.enum_Module.Appshare;
					}
					else if(module==enum_Module.Webshare)
					{
						msg.moduleType=Common.enum_Module.Webshare;
					}
					
					#endregion 

					if(this.tool==enum_WhiteboardToolCode.Brush )
					{
						
						DrawBrush(mouseLastPos_OnDown, new Point(e.X,e.Y),msg.c,msg.thickness);
						msg.lst_Points.Add(mouseLastPos_OnDown);
						this._picBox.Refresh();
						this._picBox.Update();
						Thread.Sleep(20);
						this.mouseLastPos_OnDown=new Point(e.X,e.Y);
						
						
						
					}

					else if(this.tool==enum_WhiteboardToolCode.Pencil)
					{
						DrawPencil(mouseLastPos_OnDown, new Point(e.X,e.Y),msg.c,msg.thickness);
						msg.lst_Points.Add(mouseLastPos_OnDown);
						this._picBox.Refresh();
						this._picBox.Update();
						Thread.Sleep(20);
						this.mouseLastPos_OnDown=new Point(e.X,e.Y);
					
					
					}
					
				}			
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void pictureBox1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)",ex,"",false);
				
			}
		}	

		private void _picBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			msg=new MsgAnnotation_Generic();
			//mouseDownPos=new Point(e.X,e.Y);
			this.flag_SendingThread=false;
			try
			{
				

				color = NetworkManager.thisInstance.profile.m_AssignedColor;
				tool= (enum_WhiteboardToolCode)ClientUI.getInstance().whiteBoard.tool;
				
				

				mouseDownPos=new Point(e.X,e.Y);
				
				if(this.textBox1.Visible==true)//!="")
				{
					textBox1_Leave(this,new System.EventArgs());
					this.textBox1.Visible=false;
				}
				pencilLastPoint=new Point(e.X,e.Y);
				if(this.tool==enum_WhiteboardToolCode.Brush ||this.tool==enum_WhiteboardToolCode.Pencil)
				{
					
					this.mouseFirstPos_OnDown=new Point(e.X,e.Y);
					this.mouseLastPos_OnDown=this.mouseFirstPos_OnDown;
					msg.lst_Points.Add(this.mouseLastPos_OnDown);

					
					msg.ConferenceID = WebMeeting.Client.NetworkManager.thisInstance.profile.ConferenceID ;
					msg.tool=(ushort)tool;
					msg.c=this.color;

					if(tool == enum_WhiteboardToolCode.Brush)
						msg.thickness=this.brushThickness;
					else
						msg.thickness=WebMeeting.Client.ClientUI.getInstance().whiteBoard.LineThickness;
				
				}
				else if(this.tool!=enum_WhiteboardToolCode.None)
				{
					
					if(this.tool==enum_WhiteboardToolCode.Text)
					{
						if(this.textBox1.Text!="")
						{
							textBox1_Leave(this,new System.EventArgs());
						}
						textBox1.Font=WebMeeting.Client.ClientUI.getInstance().whiteBoard.localCopyOfFont;					
					
						this.textBox1.Top=e.Y;
						this.textBox1.Left=e.X;
						this.textBox1.Visible=true;
						this.textBox1.Width=100;
						this.textBox1.BorderStyle=BorderStyle.None;
						this.textBox1.Focus();
					}		
		
				}
			}					
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)",ex,"",false);
				//Trace.WriteLine("private void pictureBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}			



		}








		private void CreateMouseMove(Common.MsgAnnotation_Generic msg)//Point mouseDownPos,Point currentPos,WhiteboardToolCode tool,Color c)
		{
			try
			{
				switch((enum_WhiteboardToolCode)msg.tool)
				{
					case enum_WhiteboardToolCode.Brush:
						DrawBrush(msg.pencilLastPoint,msg.end,msg.c,msg.thickness);
						break;
					case enum_WhiteboardToolCode.Ellipse:
						DrawEllipse(msg.start,msg.end,msg.c, msg.thickness);
						break;
					case enum_WhiteboardToolCode.Eraser:
						DrawEraser(msg.end);
						break;
					case enum_WhiteboardToolCode.Line:
						DrawLine(msg.start,msg.end,msg.c, msg.thickness);
						break;
					case enum_WhiteboardToolCode.Pencil:
						DrawPencil(msg.pencilLastPoint,msg.end,msg.c, msg.thickness);
						break;
					case enum_WhiteboardToolCode.Rectangle:
						DrawRectangle(msg.start,msg.end,msg.c, msg.thickness);
						break;
					case enum_WhiteboardToolCode.Text:
						//lTxtMsg=msg;
						DrawText(msg.start,msg.text,msg.FontFamily,msg.txtSize,msg.txtBold,msg.txtItalics,msg.txtUnderline,msg.c);
						break;
					case enum_WhiteboardToolCode.UniArrow:
						DrawUniArrow(msg.start,msg.end,msg.c,msg.thickness);
						break;
					case enum_WhiteboardToolCode.Undo:
						//Undo();
						break;
					case enum_WhiteboardToolCode.Redo:
						//Redo();
						break;
					case enum_WhiteboardToolCode.PlaceHolderArrow:
						PlaceHolderArrow(msg.start,msg.c,msg.text);
						break;
					default:
						break;
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void CreateMouseMove(WhiteboardMessage msg)",ex,"",false);
				//Trace.WriteLine("private void CreateMouseMove(WhiteboardMessage msg)//Point mouseDownPos,Point currentPos,WhiteboardToolCode tool,Color c)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}

		}

		private void PlaceHolderArrow(Point p,Color c,String text)
		{			
	
			try
			{
				int width=text.Length * 5 + 20;
				p.X=p.X - width - 19;
				p.Y=p.Y- 19;
			
				Point[] points=new Point[7];
				points[0]=new Point(p.X ,p.Y+10);
				points[1]=new Point(p.X +width,p.Y+10);
				points[2]=new Point(p.X +width,p.Y+1);
				points[3]=new Point(p.X +width+19,p.Y+19);
				points[4]=new Point(p.X +width,p.Y+37);
				points[5]=new Point(p.X +width,p.Y+28);
				points[6]=new Point(p.X ,p.Y+28);
			
				GraphicsPath path = new GraphicsPath();
				path.AddPolygon(points);
				Region r=new Region(path);
						
				lock(this)
				{
				
					g.FillRegion(new Pen(c).Brush,r);
				}
				FontStyle ff=new FontStyle();
				ff=ff|FontStyle.Bold;
						
				Font myFont = new Font(this.Font.FontFamily,this.Font.Size,ff);
				g.DrawString(text,constmyFont,new Pen(Color.White).Brush,p.X + 3 ,p.Y + 12);
			}
			catch(Exception ex)
			{
				// Definate ExceptionWebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void PlaceHolderArrow(Point p,Color c,String text)",ex,"",false);
				//Trace.WriteLine("private void PlaceHolderArrow(Point p,Color c,String text)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}

		}
		private void DrawText(Point pos,string text,Font f,Color c)
		{
			try
			{
				g.DrawString(text,f,(new Pen(c)).Brush,pos);			
			}
			catch(Exception ex)
			{
				// Definate ExceptionWebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawText(Point pos,string text,Font f,Color c)",ex,"",false);
				//Trace.WriteLine("private void DrawText(Point pos,string text,Font f,Color c)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}					
		}
		private void DrawBrush(Point start, Point end, Color c,int thickness)
		{		
			try
			{
				Point[] points=new Point[4];
				int thick=thickness/2;
				points[0]=new Point(start.X - thick, start.Y-thick);//start.X - brushThickness/2,start.Y - brushThickness/2);
				points[1]=new Point(start.X+thick,start.Y+thick);//,maxY);//start.X - brushThickness/2,start.Y + brushThickness/2);			
				points[3]=new Point(end.X-thick,end.Y-thick);//maxX,minY);//end.X + brushThickness/2,end.Y + brushThickness/2);
				points[2]=new Point(end.X+thick,end.Y+thick);//,maxY);//end.X + brushThickness/2,end.Y - brushThickness/2);

				GraphicsPath path = new GraphicsPath();
				path.AddPolygon(points);
				Region r=new Region(path);			
				
				lock(this)
				{
                    g.FillRegion(new Pen(c).Brush,r);
				}
			}
			catch(Exception ex)
			{
				// Definate ExceptionWebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawBrush(Point start, Point end, Color c,int thickness)",ex,"",false);
				//Trace.WriteLine("private void DrawBrush(Point start, Point end, Color c,int thickness)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}					

		}
		private void DrawPencil(Point start,Point end,Color c,int thickness)
		{			
			try
			{
				g.DrawLine((new Pen(c,thickness)),start,end);
			}
			catch(Exception ex)
			{
				// Definate ExceptionWebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawPencil(Point start,Point end,Color c,int thickness)",ex,"",false);
				//Trace.WriteLine("private void DrawPencil(Point start,Point end,Color c,int thickness)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}					

		}

		
		private void DrawUniArrow(Point start,Point end,Color c,int thickness)
		{			
			try
			{
				Point pFrom = start;
				Point pBase = new Point(0,0);
				Point[] aptPoly=new Point[3];
				float[] vecLine = new float[2];
				float[] vecLeft=  new float[2];;
				float fLength;
				float th;
				float ta;

				// get from point
		

				// set to point
				aptPoly[0].X = end.X;
				aptPoly[0].Y = end.Y;

				// build the line vector
				vecLine[0] = (float) aptPoly[0].X - pFrom.X;
				vecLine[1] = (float) aptPoly[0].Y - pFrom.Y;

				// build the arrow base vector - normal to the line
				vecLeft[0] = -vecLine[1];
				vecLeft[1] = vecLine[0];

				// setup length parameters
				fLength = (float) Math.Sqrt(vecLine[0] * vecLine[0] + vecLine[1] * vecLine[1]);
				th = pA.nWidth / (2.0f * fLength);
				ta = pA.nWidth / (2.0f * ((float)Math.Tan(pA.fTheta) / 2.0f) * fLength);

				// find the base of the arrow
				pBase.X = (int) (aptPoly[0].X + -ta * vecLine[0]);
				pBase.Y = (int) (aptPoly[0].Y + -ta * vecLine[1]);

				// build the points on the sides of the arrow
				aptPoly[1].X = (int) (pBase.X + th * vecLeft[0]);
				aptPoly[1].Y = (int) (pBase.Y + th * vecLeft[1]);
				aptPoly[2].X = (int) (pBase.X + -th * vecLeft[0]);
				aptPoly[2].Y = (int) (pBase.Y + -th * vecLeft[1]);


				Point  p6 = new Point(pBase.X, pBase.Y);
				Point p7= new Point(aptPoly[1].X, aptPoly[1].Y);
				Point p3= new Point (aptPoly[0].X, aptPoly[0].Y);
				Point p4= new Point(aptPoly[2].X, aptPoly[2].Y);
				Point p5= new Point(pBase.X, pBase.Y);
				lock(this)
				{
					g.FillPolygon((new Pen(c,this.lineThickness)).Brush,new Point[]{p6,p7,p3,p4,p5});
					g.DrawLine((new Pen(c)),start,end);
				}
			}
			catch(Exception exp)
			{
				exp=exp;
			}					
		}
		
		
		
		/// <summary>
		/// This Methods Adds all the Mesages from MsgPool_Clear ==>MsgPool_Annotation
		/// Because Consumethread is gona consume "MsgPool_Annotation" Automatically
		/// </summary>
		/// <param name="clientID">whoes annotation message ere to be cleared </param>
		public void ReAnnotate(int clientID)
		{

			try
			{
				ArrayList temp_lst_Points;
				MsgAnnotation_Generic msg_re;
				//Efficient methodology 
				//test=(ArrayList)this.MsgPool_Clear.Clone();
				//while(this.MsgPool_Clear.Count>0)
				for (int i=this.MsgPool_Clear.Count;i>0;i--)
				{
			
					msg_re=(MsgAnnotation_Generic)this.MsgPool_Clear[i-1];
					// messages except this profile are to be reannotated 
				
					if(msg_re.SenderID!=clientID)
					{
						
					// If Brush or Pencil is selected 

					if((enum_WhiteboardToolCode)msg_re.tool==enum_WhiteboardToolCode.Brush || (enum_WhiteboardToolCode)msg_re.tool==enum_WhiteboardToolCode.Pencil)
						 {
							temp_lst_Points=(ArrayList)msg_re.lst_Points.Clone();

						#region Brush or Pencil 
							 if(temp_lst_Points!=null)
							 {
								 while (temp_lst_Points.Count>1)
								 {
									 //Thread.Sleep(100);
									 try
									 {
											
										 lock(temp_lst_Points.SyncRoot)
										 {
											 msg_re.pencilLastPoint=(Point)temp_lst_Points[0];
											 msg_re.end			   =(Point)temp_lst_Points[1];
										 }
										 CreateMouseMove(msg_re);//message.start,message.end,(WebMeetingDrawingBoard.WhiteboardToolCode) message.tool,message.c);
										 lock(msg_re.lst_Points.SyncRoot)
										 {
											 temp_lst_Points.RemoveAt(0);
										 }
									 }
									 catch(Exception exp)
									 {
										 WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Websharing 1165 new whiteboard.cs Consume Thread's While loop for annotation arraylistof points",exp,"",true);									
									 }

								 }//while
							 }//If !=null
							 #endregion 

							this._picBox.Refresh();
							this._picBox.Update();
						 }
						 else
						 {
							 CreateMouseMove	(msg_re);
						 }
						//test.RemoveAt(i-1);
					}
					else
					{
						if(this.MsgPool_Clear.Count>0)
						{
							this.MsgPool_Clear.RemoveAt(i-1);
						}
						
					}
				
					
				}
			
			}
			catch(Exception exp)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Whiteboard.cs reAnotate(int clientID) 747",exp,exp.Message ,true);
			}


			//Consitant methodology 

			/*
			Common.MsgAnnotation_Generic message=new MsgAnnotation_Generic();
			for (int i=0;i<this.MsgPool_Clear.Count;i++)
			{
				
				this.MsgPool_Annotation.Add(this.MsgPool_Clear[i]);
				

			}
			

			this.MsgPool_Clear.Clear();
			*/
		}









		public void Clear(int clientId)
		{

				try
				{
					if(g!=null)
						g.Clear(Color.White);		
				
					this._picBox.Image=(Image)this.Global_image.Clone();
					g=Graphics.FromImage(this._picBox.Image);
					
					// If its -1 then its a clear All message rather then a clear message
					if(clientId!=-1)
					{
									
						ReAnnotate(clientId);
						
					}
				}
				catch(Exception exp)
				{
					MessageBox.Show(exp.Message.ToString()+ "" +exp.StackTrace.ToString());
				}
			
			
		}		



		private void DrawEraser(Point p)
		{
			try
			{
				lock(this)
				{
				
					g.FillRectangle((new Pen(Color.White)).Brush,p.X,p.Y,eraser.Width,eraser.Height);
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawEraser(Point p)",ex,"",false);
				//Trace.WriteLine("private void DrawEraser(Point p)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);				
			}					
		}
		
	
		public void DrawText(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c)
		{
			try
			{
				FontStyle ff=new FontStyle();
				if(bold)
					ff=ff|FontStyle.Bold;
				if(italics)
					ff=ff|FontStyle.Italic;
				if(underline)
					ff=ff|FontStyle.Underline;           
				try 
				{
					if(fontFamily!=null)
					{
						this.Font = new Font(fontFamily,size,ff);
						DrawText(pos,text,this.Font,c);
					}
				}
				catch(Exception ex)
				{
					// Definate ExceptionWebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void DrawText(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c)",ex,"",false);
					//Trace.WriteLine("public void DrawText(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
				}
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard public void DrawText(Point pos,string text, string fontFamily, float size, bool bold, bool italics, bool underline,Color c)",ex,"",false);
			}
		}

		private void DrawLine(Point start, Point end, Color c, int thickness)
		{
			try
			{				
				g.DrawLine((new Pen(c, thickness)),start,end);
			}
			catch(Exception ex)
			{
				// Definate ExceptionWebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawLine(Point start, Point end, Color c, int thickness)",ex,"",false);
				//Trace.WriteLine("private void DrawLine(Point start, Point end, Color c, int thickness)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}

		}
		private void DrawEllipse(Point start,Point end,Color c, int thickness)
		{
			try
			{
				int left=Math.Min(start.X,end.X);
				int top=Math.Min(start.Y,end.Y);
				int width=Math.Max(start.X,end.X) - left;
				int height=Math.Max(start.Y,end.Y) - top;
				g.DrawEllipse((new Pen(c, thickness)),left,top,width,height);

			}
			catch(Exception ex)
			{
				// Definate ExceptionWebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawEllipse(Point start,Point end,Color c, int thickness)",ex,"",false);
				//Trace.WriteLine("private void DrawEllipse(Point start,Point end,Color c, int thickness)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}
		}
		private void DrawRectangle(Point start,Point end,Color c, int thickness)
		{
			try
			{
				int left=Math.Min(start.X,end.X);
				int top=Math.Min(start.Y,end.Y);
				int width=Math.Max(start.X,end.X) - left;
				int height=Math.Max(start.Y,end.Y) - top;
				g.DrawRectangle((new Pen(c, thickness)),left,top,width,height);
			}
			catch(Exception ex)
			{
				// Definate ExceptionWebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void DrawRectangle(Point start,Point end,Color c, int thickness)",ex,"",false);
				//Trace.WriteLine("private void DrawRectangle(Point start,Point end,Color c, int thickness)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}
		}


		public void SendingThread()
		{
			try
			{
				//while(m_bIsActive==true)
				while(true)
				{
				
						
					if(MsgPool_SendAnnotation.Count<1)
					{
						Thread.Sleep(10);
						continue;
					}

						while(MsgPool_SendAnnotation.Count>0)
						{	
		

							///*
							if(this.flag_SendingThread)
							{
								
								try
								{
									//Monitor.Enter(ClientUI.Lock_MsgPool_SendAnnotation);
									//lock(ClientUI.Lock_MsgPool_SendAnnotation)
									lock(MsgPool_SendAnnotation.SyncRoot)
									{
										if(MsgPool_SendAnnotation.Count>0)
										{
											if(ClientUI.Flag_MsgPool_SendAnnotation)
											{
												MsgPool_SendAnnotation.RemoveAt(0);
											}
											else
											{
												message=(MsgAnnotation_Generic)MsgPool_SendAnnotation[0];
												NetworkManager.thisInstance.SendLoadPacket(message);
												if(MsgPool_SendAnnotation.Count>0)
													MsgPool_SendAnnotation.RemoveAt(0);
											}
										}
									}
							
									
									
								}
								catch(Exception ex)
								{	
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WhiteBoard public void SendingThreadFunction()",ex,"",false);
							
								}
								finally
								{
									//Monitor.Exit(ClientUI.Lock_MsgPool_SendAnnotation);
								
								}
							}
							//*/
							//ClientUI.browserControl.whiteBoard.MsgPool_Remove(0,1);
						}//while
					
					ClientUI.Flag_MsgPool_SendAnnotation=false;
				}
			}		
			catch(System.Threading.ThreadAbortException ex)
			{
				ex=ex;
			}
			catch(Exception exp)
			{	
				
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: WhiteBoard public void SendingThreadFunction()",exp,"",false);
				
			}
		}

	




		/// <summary>
		/// 
		/// This Method consume the Annotation Genric messgae designed for different modules
		/// </summary>
		public  void ConsumeThread()
		{
			try
			{
				ArrayList temp_lst_Points;
				//while(m_bIsActive==true)
				while(true)
				{
					
					if(MsgPool_Annotation.Count<1)
					{
						Thread.Sleep(10);
						continue;
					}

					while(MsgPool_Annotation.Count>0)
					{
						
						try
						{
							
							message=(MsgAnnotation_Generic)MsgPool_Annotation[0];
							
							
							// If this is true it means that a clear msg is generated
							if(ClientUI.Flag_MsgPool_Annotation)
							{
							
								lock(MsgPool_Annotation.SyncRoot)
								{
									if(MsgPool_Annotation.Count>0)
										MsgPool_Annotation.RemoveAt(0);
								}
							}
							//If Pencil or brush is the tool 
							else if((enum_WhiteboardToolCode)message.tool==enum_WhiteboardToolCode.Brush || (enum_WhiteboardToolCode)message.tool==enum_WhiteboardToolCode.Pencil)
							{
									
								#region Brush or pencil 
									//								
									temp_lst_Points=(ArrayList)message.lst_Points.Clone();
								
								this.MsgPool_Clear.Add(message);
	
								if(message.lst_Points!=null)
								{
									
									while (temp_lst_Points.Count>1)
									{
										
										//Thread.Sleep(100);
										try
										{
											
											lock(temp_lst_Points.SyncRoot)
											{
												message.pencilLastPoint=(Point)temp_lst_Points[0];
												message.end			   =(Point)temp_lst_Points[1];
											}
											CreateMouseMove(message);//message.start,message.end,(WebMeetingDrawingBoard.WhiteboardToolCode) message.tool,message.c);
											
											//message.lst_Points.RemoveRange(0,2);
											lock(message.lst_Points.SyncRoot)
											{
												temp_lst_Points.RemoveAt(0);
											}
										}
										catch(Exception exp)
										{
											WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Websharing 1165 new whiteboard.cs Consume Thread's While loop for annotation arraylistof points",exp,"",true);									
										}

									}//while


								}//If !=null

								this._picBox.Invalidate();
								this._picBox.Update();

				
								
								
								try
								{
								
									lock(MsgPool_Annotation.SyncRoot)
									{
										if(MsgPool_Annotation.Count>0)
											MsgPool_Annotation.RemoveAt(0);
									}
								}

								catch(Exception exp)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing 1240 new whiteboard.cs  MsgPool_SendAnnotation removeaAt section in Consume Thread",exp,"",true);
								}

								

								#endregion 
								
							}
							
							else
							{						
																
								this.MsgPool_Clear.Add(message);
								
								try
								{
								
									lock(MsgPool_Annotation.SyncRoot)
									{
										if(MsgPool_Annotation.Count>0)
											MsgPool_Annotation.RemoveAt(0);
									}
								}

								catch(Exception exp)
								{
									WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing 1240 new whiteboard.cs  MsgPool_SendAnnotation removeaAt section in Consume Thread",exp,"",true);
								}
								
								
								this._picBox.Invalidate();
								CreateMouseMove(message);
								
								
								this._picBox.Invalidate();
								this._picBox.Update();
								CreateMouseMove(message);//message.start,message.end,(WebMeetingDrawingBoard.WhiteboardToolCode) message.tool,message.c);

							}
						}
						catch(System.Threading.ThreadAbortException ex)
						{
							ex=ex;
						}
						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: private void ConsumeThread() ",exp,exp.Message,false);
						}					
					}//while 					
					ClientUI.Flag_MsgPool_Annotation=false;
				}				
			}
			catch(Exception ex)
			{
				
			}		
		}

		
		/*
		public void MsgPool_Remove(int a,int b)
		{

			if(b==0)
			{
				lock(MsgPool_Annotation.SyncRoot)
				{
					
					if(a==0)
					{
						try
						{
							//Monitor.Enter(ClientUI.Lock_MsgPool_Annotation);
						
							if(MsgPool_Annotation.Count>0)
							MsgPool_Annotation.RemoveAt(0);
						}

						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing 1240 new whiteboard.cs  MsgPool_SendAnnotation removeaAt section in Consume Thread",exp,"public void MsgPool_AnnotationRemove",true);
						}
						finally
						{
							//Monitor.Exit(ClientUI.Lock_MsgPool_Annotation);
						}
					}
					else
					{
						if(MsgPool_Annotation.Count>0)
						MsgPool_Annotation.Clear();
					}
				}

			}
			//MsgPool_SendAnnotation
			else
			{

						
				lock(MsgPool_SendAnnotation.SyncRoot)
				{
					
					
					if(a==0)
					{
						try
						{
							//Monitor.Enter(ClientUI.Lock_MsgPool_Annotation);
							if(MsgPool_SendAnnotation.Count>0)
							MsgPool_SendAnnotation.RemoveAt(0);
						}

						catch(Exception exp)
						{
							WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: Websharing 1240 new whiteboard.cs  MsgPool_SendAnnotation removeaAt section in Consume Thread",exp,"public void MsgPool_SendAnnotationRemove",true);
						}
						finally
						{
							//Monitor.Exit(ClientUI.Lock_MsgPool_Annotation);
						}
					}
					else
					{
						if(MsgPool_SendAnnotation.Count>0)
						MsgPool_SendAnnotation.Clear();
					}
				}
			
			
			
			
			}
		
		}

		*/
		#region Textbox Handlers
		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				this.textBox1.Width = Math.Min((int)( this.textBox1.Text.Length * WebMeeting.Client.ClientUI.getInstance().whiteBoard.localCopyOfFont.Size /1.5) + 10,this.Width - this.textBox1.Left);									
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void textBox1_TextChanged(object sender, System.EventArgs e)",ex,"",false);
				//Trace.WriteLine("private void textBox1_TextChanged(object sender, System.EventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}
		}

		private void textBox1_Leave(object sender, System.EventArgs e)
		{
			try
			{
						
				#region Module selection
				if(module==enum_Module.Whiteboard)
				{
					msg.moduleType=Common.enum_Module.Whiteboard;
				}
				else if(module==enum_Module.Appshare)
				{
					msg.moduleType=Common.enum_Module.Appshare;
				}
				else if(module==enum_Module.Webshare)
				{
					msg.moduleType=Common.enum_Module.Webshare;
				}
					
				#endregion 



				msg.ConferenceID = WebMeeting.Client.NetworkManager.thisInstance.profile.ConferenceID ;

				msg.start=this.textBox1.Location;//mouseDownPos;
				msg.end=this.textBox1.Location;
				msg.tool=(ushort)enum_WhiteboardToolCode.Text;//tool;
				msg.c=WebMeeting.Client.NetworkManager.thisInstance.profile.m_AssignedColor;
				msg.font= WebMeeting.Client.ClientUI.getInstance().whiteBoard.localCopyOfFont;
				msg.thickness=this.lineThickness;
			
				msg.text=this.textBox1.Text;
				this.MsgPool_Annotation.Add(msg);			
				this.MsgPool_SendAnnotation.Add(msg);			
				msg.txtSize = localCopyOfFont.Size;

				msg.txtItalics =WebMeeting.Client.ClientUI.getInstance().whiteBoard.localCopyOfFont.Italic;
				msg.txtBold = WebMeeting.Client.ClientUI.getInstance().whiteBoard.localCopyOfFont.Bold;
				msg.txtUnderline = WebMeeting.Client.ClientUI.getInstance().whiteBoard.localCopyOfFont.Underline;
				msg.FontFamily = WebMeeting.Client.ClientUI.getInstance().whiteBoard.localCopyOfFont.Name;

				WebMeeting.Client.NetworkManager network = NetworkManager.thisInstance;
				msg.SenderID = NetworkManager.thisInstance.profile.ClientId;;
				msg.nSessionId = nSessionId;
				
				this.MsgPool_SendAnnotation.Add(msg);	
				//network.SendLoadPacket(msg);						
				this.textBox1.Text="";
				this.tool=enum_WhiteboardToolCode.None;
			}
			catch(Exception ex)
			{
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void textBox1_Leave(object sender, System.EventArgs e)",ex,"",false);
				//Trace.WriteLine("private void textBox1_Leave(object sender, System.EventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
			}

		}
		

		private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				this.textBox1.Width = Math.Min((int)( this.textBox1.Text.Length * WebMeeting.Client.ClientUI.getInstance().whiteBoard.localCopyOfFont.Size /1.5) + 10,this.Width - this.textBox1.Left);
				if(e.KeyCode == System.Windows.Forms.Keys.Enter)
				{
					textBox1_Leave(this,new System.EventArgs());				
					this.BringToFront();
					this.Focus();
					textBox1.Visible = false;				
				}
				if(e.KeyCode==System.Windows.Forms.Keys.Escape)
				{
					textBox1.Text="";
					textBox1.Visible = false;
				}
			}
			catch(Exception ex)
			{
				//Trace.WriteLine("private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)"+" exception string :::: "+ex.ToString()+" stack trace ::: " +ex.StackTrace+" Message ::: "  +ex.Message);
				WebMeeting.Client.ClientUI.getInstance().ShowExceptionMessage("Module ::: whiteboard private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)",ex,"",false);
			}
		}

		#endregion Textbox Handlers



		public void AddMessagetoPool(Common.MsgAnnotation_Generic _msgannotation_Generic)
		{

			this.MsgPool_Annotation.Add(_msgannotation_Generic);	
		
		}

	
	
	}

}
