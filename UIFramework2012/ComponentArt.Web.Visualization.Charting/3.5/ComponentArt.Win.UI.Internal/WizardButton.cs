using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Win.UI.Internal
{
	/// <summary>
	/// Summary description for MyButton.
	/// </summary>
	internal class Button : System.Windows.Forms.Button
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		bool m_pressed = false;

		bool m_over = false;

		Image m_disabledIcon = null;
		Image m_normalIcon = null;
		Image m_hoverIcon = null;

		Point m_iconLocation;
		Point m_textLocation = new Point(0,0);

		public Button() 
		{
			base.Font = new Font("Microsoft Sans Serif", 11, GraphicsUnit.Pixel);
		}

		protected override void OnInvalidated ( System.Windows.Forms.InvalidateEventArgs e ) 
		{
			GenerateButtonImages();
			base.OnInvalidated(e);
		}


		protected override void OnTextChanged ( System.EventArgs e ) 
		{
			base.OnTextChanged(e);
			GenerateButtonImages();
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

				m_downImage.Dispose();
				m_disabledImage.Dispose();
				m_hoverImage.Dispose();
				m_normalImage.Dispose();
			}
			base.Dispose( disposing );
		}
		
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
			}
		}

		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				Invalidate();
				Refresh();
			}
		}
	
		Bitmap m_normalImage, m_hoverImage, m_disabledImage, m_downImage;
		
		void DrawButton(ref Bitmap bmp, Color borderColor, Color textColor, Color highShade, Color lowShade) 
		{
			DrawButton (ref bmp, borderColor, textColor, highShade, lowShade, null, new Point(0,0));
		}

		void DrawButton(ref Bitmap bmp, Color borderColor, Color textColor, Color highShade, Color lowShade, Image icon, Point iconLocation) 
		{
			if (bmp != null) 
				bmp.Dispose();

			bmp = new Bitmap(Width, Height);

			Graphics g = Graphics.FromImage(bmp);
			g.Clear(Color.FromArgb(238, 238, 238));

			// outer lines
			g.DrawLine(new Pen(Color.FromArgb(204, 204, 204)), 0, 0, Width-1, 0);
			g.DrawLine(new Pen(Color.FromArgb(204, 204, 204)), 0, 0, 0, Height-1);

			g.DrawLine(new Pen(Color.FromArgb(243, 243, 243)), Width-1, Height-1, Width-1, 0);
			g.DrawLine(new Pen(Color.FromArgb(243, 243, 243)), Width-1, Height-1, 0, Height-1);


			// frame
			g.DrawRectangle(new Pen(borderColor), 1, 1, Width-3, Height-3);

			// inner lines
			g.DrawLine(new Pen(highShade), 2, 2, Width-4, 2);
			g.DrawLine(new Pen(highShade), 2, 2, 2, Height-4);

			g.DrawLine(new Pen(lowShade), Width-3, Height-3, Width-3, 3);
			g.DrawLine(new Pen(lowShade), Width-3, Height-3, 3, Height-3);

			// draw text
			SizeF textsize = g.MeasureString(Text, Font);
			PointF startingPoint = new PointF(Width/2 - textsize.Width/2 + 1, Height/2 - textsize.Height/2 + 1);
			g.DrawString(Text, Font, new SolidBrush(textColor), m_textLocation == new Point(0,0)? startingPoint : m_textLocation);


			// draw icon
			if (icon != null) 
			{
				Point p = (m_iconDistance <= 0) ? iconLocation : new Point((int)startingPoint.X - m_iconDistance - icon.Width, iconLocation.Y);
				g.DrawImage(icon, new Rectangle (p, icon.Size) );
			}
			g.Dispose();
		}

		void DrawNormal() 
		{
			DrawButton(ref m_normalImage, Color.FromArgb(68, 68, 68), Color.FromArgb(51, 51, 51), Color.White, Color.FromArgb(204, 204, 204), m_normalIcon, m_iconLocation);
		}

		void DrawHover() 
		{
			DrawButton(ref m_hoverImage, Color.FromArgb(255, 51, 0), Color.FromArgb(221, 52, 59), Color.White, Color.FromArgb(204, 204, 204), m_hoverIcon, m_iconLocation);
		}

		void DrawDisabled() 
		{
			DrawButton(ref m_disabledImage, Color.FromArgb(136, 136, 136), Color.FromArgb(170, 170, 170), Color.White, Color.FromArgb(204, 204, 204), m_disabledIcon, m_iconLocation);
		}

		void DrawDown() 
		{
			DrawButton(ref m_downImage, Color.FromArgb(255, 51, 0), Color.FromArgb(221, 52, 59), Color.FromArgb(204, 204, 204), Color.White, m_hoverIcon, m_iconLocation);
		}

		void GenerateButtonImages() 
		{
			DrawDisabled();
			DrawDown();
			DrawNormal();
			DrawHover();
		}

		protected override void OnResize(EventArgs e) 
		{
			base.OnResize(e);
			if (Width == 0 || Height == 0)
				return;

			GenerateButtonImages();
		}

		protected override void OnMouseDown ( System.Windows.Forms.MouseEventArgs mevent )
		{
			m_pressed = true;
			base.OnMouseDown(mevent);
			Invalidate();
		}

		protected override void OnMouseUp ( System.Windows.Forms.MouseEventArgs mevent )
		{
			m_pressed = false;
			base.OnMouseUp(mevent);
			Invalidate();
		}
        
		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs e )
		{
			base.OnPaint(e);

			Graphics g = e.Graphics;
			g.DrawImageUnscaled(m_pressed ? m_downImage : (m_over || Focused) ? m_hoverImage : (!Enabled) ? m_disabledImage : m_normalImage, 0, 0);
		}

		protected override void OnMouseEnter ( System.EventArgs eventargs ) 
		{
			base.OnMouseEnter(eventargs);
			m_over = true;
			Invalidate();
		}
		
		protected override void OnMouseLeave ( System.EventArgs eventargs ) 
		{
			base.OnMouseLeave(eventargs);
			m_over = false;
			Invalidate();
		}

		[DefaultValue(null)]
		public Image DisabledIcon 
		{
			get 
			{
				return m_disabledIcon;
			}
			set 
			{
				m_disabledIcon = value;
				DrawDisabled();
				Invalidate();
			}
		}

		[DefaultValue(null)]
		public Image NormalIcon 
		{
			get 
			{
				return m_normalIcon;
			}
			set 
			{
				m_normalIcon = value;
				DrawNormal();
				Invalidate();
			}
		}

		[DefaultValue(null)]
		public Image HoverIcon 
		{
			get 
			{
				return m_hoverIcon;
			}
			set 
			{
				m_hoverIcon = value;
				DrawDown();
				DrawHover();
				Invalidate();
			}
		}

		int m_iconDistance = 0;

		[DefaultValue(0)]
		public int IconDistance 
		{
			get 
			{
				return m_iconDistance;
			}
			set 
			{
				m_iconDistance = value;
				GenerateButtonImages();
				Invalidate();
			}
		}

		[DefaultValue(typeof(Point), "0,0")]
		public Point IconLocation 
		{
			get 
			{
				return m_iconLocation;
			}
			set 
			{
				m_iconLocation = value;
				GenerateButtonImages();
				Invalidate();
			}
		}


		[DefaultValue(typeof(Point), "0,0")]
		public Point TextLocation 
		{
			get 
			{
				return m_textLocation;
			}
			set 
			{
				m_textLocation = value;
				GenerateButtonImages();
				Invalidate();
			}
		}
	}
}