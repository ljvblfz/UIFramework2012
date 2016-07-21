using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using System.Windows.Forms;


#if !__BUILDING_ComponentArt_Win_UI_Internal__
namespace ComponentArt.Win.UI.Internal
#else
namespace ComponentArt.Win.UI.WinChartSamples
#endif
{
	internal class Skin
	{
		#region --- Colors ---

		Color m_lightFrameColor;
		Color m_darkFrameColor;
		Color m_lightGradientColor;
		Color m_darkGradientColor;
		Color m_labelColor;

		public Color LightFrameColor {get {return m_lightFrameColor;}  set {m_lightFrameColor = value;}}
		public Color DarkFrameColor  {get {return m_darkFrameColor;}   set {m_darkFrameColor = value;}}

		public Color LightGradientColor  {get {return m_lightGradientColor;}  set {m_lightGradientColor = value;}}
		public Color DarkGradientColor  {get {return m_darkGradientColor;}    set {m_darkGradientColor = value;}}  

		public Color LabelColor  {get {return m_labelColor;}    set {m_labelColor = value;}}         
		#endregion

		#region --- Constructors ---

		public Skin() 
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

		public static void PaintVerticalGradient(Graphics g, Rectangle R, Color c1 , Color c2) 
		{
			Brush b = new LinearGradientBrush(new Point(R.X, R.Y), new Point(R.X, R.Y+R.Height), c1, c2);
			g.FillRectangle(b,R);
			b.Dispose();
		}

	}

	internal class GroupBoxSkin : Skin 
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
				b.Dispose();
			}
			else
			{
				Brush b = new LinearGradientBrush(new Point(R.X, R.Y), new Point(R.X, R.Y+R.Height), LightGradientColor,
					DarkGradientColor);
				g.FillRectangle(b,R);
				b.Dispose();
			}
		}
		
		public void PaintBackground(Graphics g, Rectangle R) 
		{
			PaintBackground(g, R, false);
		}

		
		public void PaintBackground(Graphics g, Rectangle rect, bool reverse)
		{
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