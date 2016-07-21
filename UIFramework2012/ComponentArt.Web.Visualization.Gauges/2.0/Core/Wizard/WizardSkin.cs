using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using System.Windows.Forms;


namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for Skin.
	/// </summary>
	//[TypeConverter(typeof(SkinConverter))]
	internal class WizardSkin
	{
		#region --- Colors ---


		Color m_lightFrameColor;
		Color m_darkFrameColor;
		Color m_lightGradientColor;
		Color m_darkGradientColor;
		Color m_labelColor;

		/*
		public static Skin DefaultSkin 
		{
			get 
			{
				return new Skin(
					Color.FromArgb(255,255,255),
					Color.FromArgb(150,150,180),
					Color.FromArgb(240,240,240),
					Color.FromArgb(200,200,220),
					Color.FromArgb(50,50,150)
					);
			}
		}
		*/

		/*
		public static Skin RedSkin 
		{
			get 
			{
				return new Skin();
			}
		}
		*/

		public Color LightFrameColor {get {return m_lightFrameColor;}  set {m_lightFrameColor = value;}}
		public Color DarkFrameColor  {get {return m_darkFrameColor;}   set {m_darkFrameColor = value;}}

		public Color LightGradientColor  {get {return m_lightGradientColor;}  set {m_lightGradientColor = value;}}
		public Color DarkGradientColor  {get {return m_darkGradientColor;}    set {m_darkGradientColor = value;}}  

		public Color LabelColor  {get {return m_labelColor;}    set {m_labelColor = value;}}         
		#endregion

		#region --- Constructors ---
		/*
		public Skin(
				Color lightFrameColor,
			Color darkFrameColor,
			Color lightGradientColor,
			Color darkGradientColor,
			Color labelColor
			) 
		{
			m_lightFrameColor = lightFrameColor;
			m_darkFrameColor = darkFrameColor;
			m_lightGradientColor = lightGradientColor;
			m_darkGradientColor = darkGradientColor;
			m_labelColor = labelColor;
		}
		*/
        public WizardSkin() 
		{
			m_lightFrameColor = 
				Color.FromArgb(255,128,128);
			m_darkFrameColor = 
				Color.FromArgb(150,50,80);
			m_lightGradientColor = 
				Color.FromArgb(240,140,140);
			m_darkGradientColor = 
				Color.FromArgb(200,100,120);
			m_labelColor = 
				Color.FromArgb(150,50,50);
		}

		#endregion

		#region --- Public Methods ---

		/*
		public void PaintFormBackground(Graphics g, Rectangle R)
		{
			PaintInterlivedBackground(g,R);
		}
		*/

		/*
		public virtual void DrawText(Graphics g, Rectangle R, string text)
		{
			Font font = new Font("Arial",8);
			g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			Brush brush = new SolidBrush(LabelColor);
			g.DrawString(text,font,brush,R);
		}
		*/
		

		/*
		public Bitmap GetBackgroundImage(Control c)
		{
			return GetBackgroundImage(c, false);
		}
		*/

		/*
		public Color BackColor 
		{
			get 
			{
				return 
					Color.FromArgb(
					(LightGradientColor.A+DarkGradientColor.A)/2,
					(LightGradientColor.R+DarkGradientColor.R)/2,
					(LightGradientColor.G+DarkGradientColor.G)/2,
					(LightGradientColor.B+DarkGradientColor.B)/2
					);
			}
		}
		*/

		/*
		public Bitmap GetBackgroundImage(Control c, bool reverse)
		{
			if (c.Width == 0 || c.Height == 0)
				return null;
			Bitmap bmp = new Bitmap(c.Width, c.Height);
			
			Graphics g = Graphics.FromImage(bmp);
			PaintBackground(g,new Rectangle(0,0,c.Width,c.Height), reverse);			
			return bmp;
		}
		*/


		/*
		public Bitmap GetBackgroundWithBorderImage(Control c, bool reverse)
		{
			if (c.Width == 0 || c.Height == 0)
				return null;
			Bitmap bmp = new Bitmap(c.Width, c.Height);
			
			Graphics g = Graphics.FromImage(bmp);
			PaintBackground(g,new Rectangle(0,0,c.Width,c.Height), reverse);			
			DrawBorder(g,new Rectangle(0,0,c.Width,c.Height), reverse);

			return bmp;
		}
		*/


		/*
		public virtual void PaintBackground(Graphics g, Rectangle R, bool reverse)
		{
			PaintHorizontalGradient(g,R,reverse);
		}
		*/
		

		/*
		public void DrawBorder(Graphics g,  Rectangle R, bool reverse)
		{
			Color light = LightFrameColor;
			Color dark  = DarkFrameColor;
			Pen pLight = new Pen(light,1);
			Pen pDark =  new Pen(dark, 1);
			if(reverse)
			{
				g.DrawLine(pDark,0,0, R.Width,0);
				g.DrawLine(pDark,0,0, 0,R.Height);
				g.DrawLine(pLight, 0,R.Height-1, R.Width,R.Height-1);
				g.DrawLine(pLight, R.Width-1,0, R.Width-1,R.Height-1);
			}
			else
			{
				g.DrawLine(pLight,0,0, R.Width,0);
				g.DrawLine(pLight,0,0, 0,R.Height);
				g.DrawLine(pDark, 0,R.Height-1, R.Width,R.Height-1);
				g.DrawLine(pDark, R.Width-1,0, R.Width-1,R.Height-1);
			}
		}
		*/
		#endregion

		#region --- Private Methods ---

		/*
		protected void PaintInterlivedBackground(Graphics g, Rectangle R)
		{
			Color c10 = Color.FromArgb(240,240,240);
			Color c11 = Color.FromArgb(200,200,200);

			Color c21 = Color.FromArgb(255,255,255);
			Color c20 = Color.FromArgb(200,200,240);
			Brush b = new LinearGradientBrush(new Point(0,0),new Point(R.Width,0),c10, c11);
			g.FillRectangle(b,0,0,R.Width,R.Height);
			Brush b1 = new LinearGradientBrush(new Point(0,0),new Point(R.Width,0),c20,c21);
			Pen p = new Pen(b1,1);
			for(int i = 0; i< R.Height; i+=3)
				g.DrawLine(p,0,i,R.Width,i);
		}
		*/

		/*
		protected void PaintHorizontalGradient(Graphics g, Rectangle R, bool reverse)
		{
			if(reverse)
			{
				Brush b = new LinearGradientBrush(new Point(R.X+R.Width,R.Y), new Point(R.X,R.Y), LightGradientColor,
					DarkGradientColor);
				g.FillRectangle(b,R);
			}
			else
			{
				Brush b = new LinearGradientBrush(new Point(R.X,R.Y), new Point(R.X+R.Width,R.Y), LightGradientColor,
					DarkGradientColor);
				g.FillRectangle(b,R);
			}
		}

		*/

				
		#endregion

		public static void PaintVerticalGradient(Graphics g, Rectangle R, Color c1 , Color c2) 
		{
			Brush b = new LinearGradientBrush(new Point(R.X, R.Y), new Point(R.X, R.Y+R.Height), c1, c2);
			g.FillRectangle(b,R);
		}

	}

	internal class GroupBoxSkin : WizardSkin 
	{

		public GroupBoxSkin() 
		{
			LightGradientColor = Color.White;
			DarkGradientColor = Color.FromArgb(244,244,244);
		}

		protected void PaintVerticalGradient(Graphics g, Rectangle R, bool reverse)
		{
			if(reverse)
			{
				Brush b = new LinearGradientBrush(new Point(R.X, R.Y+R.Height), new Point(R.X, R.Y), LightGradientColor,
					DarkGradientColor);
				g.FillRectangle(b,R);
			}
			else
			{
				Brush b = new LinearGradientBrush(new Point(R.X, R.Y), new Point(R.X, R.Y+R.Height), LightGradientColor,
					DarkGradientColor);
				g.FillRectangle(b,R);
			}
		}
		


		public void PaintBackground(Graphics g, Rectangle R) 
		{
			PaintBackground(g, R, false);
		}

		
		public void PaintBackground(Graphics g, Rectangle rect, bool reverse)
		{
			//PaintHorizontalGradient(g,R,reverse);
			g.FillRectangle(new SolidBrush(Color.White), rect);
			g.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(153, 153, 153))), new Rectangle(rect.X+1, rect.Y+1, rect.Width-3, rect.Height-3));
			g.DrawRectangle(new Pen(new SolidBrush(Color.White)), new Rectangle(rect.X+2, rect.Y+2, rect.Width-5, rect.Height-5));
			g.DrawLine(new Pen(new SolidBrush(Color.FromArgb(243, 243, 243))), 
				new Point(rect.X+2, rect.Y+2 + rect.Height-5),
				new Point(rect.X+2 + rect.Width-5, rect.Y+2 + rect.Height-5));
			
			Rectangle head = new Rectangle(new Point(rect.X+4, rect.Y+13), new Size(rect.Width-7,10));
            PaintVerticalGradient(g, head, false);

			Pen pen = new Pen(new SolidBrush(Color.FromArgb(204,204,204)));
			pen.DashStyle = DashStyle.Dot;


			g.DrawLine(pen
				, new Point( rect.X+4, rect.Y+23)
				, new Point( rect.X+rect.Width - 4, rect.Y+23)
				);

		}
		

		public void DrawText(Graphics g, Rectangle R, string text)
		{
			Font font = new Font("Verdana", 11, FontStyle.Bold, GraphicsUnit.Pixel);
			Brush brush = new SolidBrush(Color.FromArgb(102, 102, 102));
			g.DrawString(text,font,brush,R);
		}

	}
}