using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class ChartFrameShadePrimitive : ChartFrameEffect
	{
		private ChartFrameShadePosition 
			position = ChartFrameShadePosition.RightBottom;
		private bool		outside = false;
		private bool		soft = true;
		private int			width = 10;
		private Rectangle	rect;
		private bool		roundTop = true;
		private bool		roundBottom = true;
		private double		cornerRadius = 20;
		private Color		color0 = Color.FromArgb(0,0,0,0);
		private Color		color1 = Color.FromArgb(100,0,0,0);
		private Color		formColor;

		public ChartFrameShadePrimitive() { FormColor = Color.White; }

		public ChartFrameShadePrimitive(Rectangle rect)
		{
			this.rect = rect;
		}

		#region --- Properties ---
		public ChartFrameShadePosition	ShadePosition	{ get { return position; }		set { position = value; } }
		public bool						Soft			{ get { return soft; }			set { soft = value; } }
		public int						Width			{ get { return width; }			set { width = value; } }
		internal Color					Color0			{ get { return color0; }		set { color0 = value; } }
		internal Color					Color1			{ get { return color1; }		set { color1 = value; } }
		internal bool					RoundTop		{ get { return roundTop; }		set { roundTop = value; } }
		internal bool					RoundBottom		{ get { return roundBottom; }	set { roundBottom = value; } }
		internal double					CornerRadius	{ get { return cornerRadius; }	set { cornerRadius = value; } }
		internal bool					Outside			{ get { return outside; }		set { outside = value; } }
		internal Rectangle				Rectangle		{ get { return rect; }			set { rect = value; } }

		internal Color					FormColor
		{
			get { return formColor; }
			set
			{
				formColor = value;
				color0 = formColor;
				color1 = Color.FromArgb(formColor.R/3,formColor.G/3,formColor.B/3);
			}
		}
		internal Color MidColor(double t)
		{
			t = Math.Min(1, Math.Max(0,t));
			return Color.FromArgb(
				(int)(t*Color1.A + (1-t)*Color0.A),
				(int)(t*Color1.R + (1-t)*Color0.R),
				(int)(t*Color1.G + (1-t)*Color0.G),
				(int)(t*Color1.B + (1-t)*Color0.B));
		}
		#endregion

		internal override void Render(Graphics g)
		{
			int nStep = 1, i;
			double dt = 1, t=0;
			GraphicsPath p;

			if(Soft)
				nStep = Math.Min(10,width);
			else
				nStep = 1;
			if(outside)
			{
				t = 1;
				dt = -1.0/nStep;
			}
			else
			{
				t = 0;
				dt = 1.0/nStep;
			}

			g.SmoothingMode = SmoothingMode.AntiAlias;
			switch(position)
			{
				case ChartFrameShadePosition.Centered:
					for(i=0; i<nStep;i++,t+=dt)
					{
						Rectangle r0;
						Color c = MidColor(t);
						if(outside)
						{
							r0 = new Rectangle(rect.X-width,rect.Y-width, rect.Width + 2*width, rect.Height + 2*width);
							if(!Soft)
								c = Color1;
						}
						else
							r0 = new Rectangle(rect.X+width,rect.Y+width, rect.Width - 2*width, rect.Height - 2*width);
						Rectangle r1 = rect;
						p = ChartFrameEffect.CreateFrameArea(r0,r1,t,t+dt,roundTop,roundBottom,cornerRadius);
						Brush brush = new SolidBrush(c);
						g.FillPath(brush,p);
						brush.Dispose();
					}
					break;
				case ChartFrameShadePosition.RightBottom:
					g.SmoothingMode = SmoothingMode.None;
					for(i=0; i<nStep;i++,t+=dt)
					{
						Rectangle r = new Rectangle(rect.X,rect.Y,rect.Width-width,rect.Height-width);
						if(outside)
						{
							p = ChartFrameEffect.CreateRightBottomArea(rect,t*width,(t+dt)*width,roundTop,roundBottom, cornerRadius);
							Color c = (Soft? MidColor(1-t): Color1);
							Brush brush = new SolidBrush(c);
							g.FillPath(brush,p);
							brush.Dispose();
						}
						else
						{
							p = ChartFrameEffect.CreateRightBottomInnerArea(rect,(1-t)*width,(1-(t+dt))*width,roundTop,roundBottom, cornerRadius);
							Brush brush = new SolidBrush(MidColor(t));
							g.FillPath(brush,p);
							brush.Dispose();
						}
					}
					g.SmoothingMode = SmoothingMode.AntiAlias;
					break;
				case ChartFrameShadePosition.LeftTop:
					for(i=0; i<nStep;i++,t+=dt)
					{
						Color c = MidColor(t);
						Rectangle r = new Rectangle(rect.X-width,rect.Y-width,rect.Width,rect.Height);
						if(outside)
						{
							p = ChartFrameEffect.CreateTopLeftArea(r,t*width,(t+dt)*width,roundTop,roundBottom, cornerRadius);
							if(!Soft)
								c = Color1;
						}
						else
							p = ChartFrameEffect.CreateTopLeftInnerArea(rect,(1-t)*width,(1-(t+dt))*width,roundTop,roundBottom, cornerRadius);
						Brush brush = new SolidBrush(c);
						g.FillPath(brush,p);
						brush.Dispose();
					}
					break;
			}
		}
	}

}
