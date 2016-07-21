using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.WinUI
{
#if DEBUG
	public
#else
	internal
#endif
	class GroupBox : System.Windows.Forms.GroupBox
	{
		GroupBoxSkin m_skin = new GroupBoxSkin();

		Bitmap m_bitmap;

		private int	topMargin, leftMargin, rightMargin, bottomMargin;
		private int distance;

		private bool m_simple = false;
		private bool m_drawBorderAroundControl = false;

		private bool m_resizeChildren = false;

		public GroupBox() 
		{
			topMargin = 2;
			leftMargin = 2;
			bottomMargin = 2;
			rightMargin = 2;
			distance = 2;
		}


		[System.ComponentModel.DefaultValue(false)]
		public bool DrawBorderAroundControl 
		{
			get 
			{
				return m_drawBorderAroundControl;
			}
			set 
			{
				m_drawBorderAroundControl = value;
				Invalidate();
			}
		}



		[System.ComponentModel.DefaultValue(false)]
		public bool SimpleGroupBox 
		{
			get 
			{
				return m_simple;
			}
			set 
			{
				m_simple = value;
				if (!m_simple)
					GenerateBitmap();
				Invalidate();
			}
		}

		/*
		public Skin Skin 
		{
			get {return m_skin;}
			set {m_skin = value;}
		}
		*/

		public bool ResizeChildren 
		{
			get {return m_resizeChildren;}
			set {m_resizeChildren = value;}
		}

		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs e )
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;

			if (!m_simple) 
			{
				g.DrawImage(m_bitmap, 0, 0);
				m_skin.DrawText(g, new Rectangle(12, 6, Width, 14), Text);
			}
			else 
			{
				g.Clear(BackColor);

				Font font = new Font("Verdana", 11, GraphicsUnit.Pixel);

				Brush brush = new SolidBrush(Color.FromArgb(102, 102, 102));
				g.DrawString(Text,font,brush, new Rectangle(2, 6, Width, 14));
			}


			if (DrawBorderAroundControl && Controls.Count > 0) 
			{
				g.DrawRectangle(new Pen(Color.FromArgb(204, 204, 204)), new Rectangle(new Point(Controls[0].Location.X-1, Controls[0].Location.Y-1), new Size(Controls[0].Size.Width + 1, Controls[0].Size.Height + 1)));
			}
		}



		void GenerateBitmap()
		{
			if (m_bitmap != null) 
				m_bitmap.Dispose();

			m_bitmap = new Bitmap(Width, Height);
			Graphics g = Graphics.FromImage(m_bitmap);
			m_skin.PaintBackground(g,new Rectangle(0,0,Width,Height),false);
			g.Dispose();
		}


		protected override void OnResize (EventArgs e) 
		{
			base.OnResize(e);

			
			if (!m_simple) 
			{
				GenerateBitmap();
			}			
			

			if (m_resizeChildren)
				foreach (Control c in Controls)
					ResizeChild(c);
		}

		void ResizeChild (Control c) 
		{
			//c.Top = 2 + 14 + distance;
			c.Top = 2 + 22 + distance;
			c.Left = leftMargin;
			c.Width = Width - leftMargin - rightMargin;
			c.Height = this.Height - topMargin - bottomMargin - distance - 22;
		}
	}
}
