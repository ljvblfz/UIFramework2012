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
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	internal class ChartRectangularFrameInner : ChartFrameInner
	{

		static readonly Color _frameColorDefault = Color.Transparent;
		internal const int _shadeWidthDefault = 5;
		internal const int _extraDefault = 0;

		private int		shadeWidth = _shadeWidthDefault;
		private int		extraTop = _extraDefault;
		private int		extraBottom = _extraDefault;
		private int       extraLeft = _extraDefault;
		private int       extraRight = _extraDefault;

		private bool		smoothLines = true;
		private bool		sharpLines = false;

		private Color		frameColor = _frameColorDefault;

		public ChartRectangularFrameInner() { }

		#region --- Properties ---

		[NotifyParentProperty(true)]
		[DefaultValue(typeof(Color), "Transparent")]
		internal Color	FrameColor			
		{
			get { return frameColor; }
			set { frameColor = value; } 
		}
		internal Color	EffectiveFrameColor			
		{
			get 
			{
				if(frameColor.A == 0 && OwningChart != null && !OwningChart.InSerialization)
					return OwningChart.Palette.FrameColor;
				return frameColor;	
			}
		}

		[NotifyParentProperty(true)]
		[DefaultValue(_extraDefault)]
		internal int		ExtraSpaceTop		{ get { return extraTop;	}	set { extraTop = value; } }
		[NotifyParentProperty(true)]
		[DefaultValue(_extraDefault)]
		internal int		ExtraSpaceBottom	{ get { return extraBottom; }	set { extraBottom = value; } }
		[NotifyParentProperty(true)]
		[DefaultValue(_extraDefault)]
		internal int		ExtraSpaceLeft		{ get { return extraLeft;	}	set { extraLeft = value; } }
		[NotifyParentProperty(true)]
		[DefaultValue(_extraDefault)]
		internal int		ExtraSpaceRight		{ get { return extraRight;	}	set { extraRight = value; } }
		
		internal bool	ShowSharpLines		{ get { return sharpLines;	}	set { sharpLines = value; } }
		internal int	ShadeWidth			{ get { return shadeWidth;	}	set { shadeWidth = value; } }
		internal bool	ShowSmoothLines		{ get { return smoothLines; }	set { smoothLines = value; } }

		internal Rectangle InternalRectangle(Graphics g)
		{
			Rectangle r = Rectangle;
			int x0 = (int)(r.X + (extraLeft + shadeWidth)*scaleFactor);
			int y0 = (int)(r.Y + (extraTop + shadeWidth)*scaleFactor);
			int x1 = (int)(r.X + r.Width - (extraRight + shadeWidth)*scaleFactor);
			int y1 = (int)(r.Y + r.Height - (extraBottom + shadeWidth)*scaleFactor);
			SizeF textSize = g.MeasureString(Text,ScaledFont);
			int textH = (int)(textSize.Height+1);
			switch(TextPosition)
			{
				case FrameTextPosition.Bottom:
					y1 -= textH;
					break;
				case FrameTextPosition.Top:
					y0 += textH;
					break;
				case FrameTextPosition.Left:
					x0 += textH;
					break;
				case FrameTextPosition.Right:
					x1 -= textH;
					break;
			}
			return new Rectangle(x0,y0,x1-x0,y1-y0);
		}
		#endregion

		public override void Render(Graphics g)
		{
			PointF[] pts = new PointF[50];
			int nPts;

			Pen whitePen = new Pen(Color.FromArgb(150,255,255,255),scaleFactor);
			Pen blackPen = new Pen(Color.FromArgb(150,0,0,0),scaleFactor);

			// Text metrics
			SizeF textSize = g.MeasureString(Text,ScaledFont);
			int textH = (int)(textSize.Height+1);

			ChartFrameShadePrimitive shade;

			// Inner rectangle

			int x0 = (int)(Rectangle.X + (shadeWidth*2 + extraLeft)*scaleFactor);
			int y0 = (int)(Rectangle.Y + (shadeWidth*2 + extraTop)*scaleFactor);
			int x1 = (int)(Rectangle.X + Rectangle.Width - (shadeWidth*2 + extraRight)*scaleFactor);
			int y1 = (int)(Rectangle.Y + Rectangle.Height - (shadeWidth*2 + extraBottom)*scaleFactor);
			switch(TextPosition)
			{
				case FrameTextPosition.Bottom:	y1 -= textH; break;
				case FrameTextPosition.Top:		y0 += textH; break;
				case FrameTextPosition.Left:		x0 += textH; break;
				case FrameTextPosition.Right:		x1 -= textH; break;
			}
			Rectangle innerRect = new Rectangle(x0,y0,x1-x0,y1-y0);

			// Frame base color

			GraphicsPath path = ExternalBorder(Rectangle);
			path.AddPath(ExternalBorder(innerRect),false);
			Color fColor = EffectiveFrameColor;
			g.FillPath(new SolidBrush(fColor),path);

			// Shades

			if(smoothLines)
			{
				// TopLeft dark
				shade = new ChartFrameShadePrimitive(innerRect);
				shade.CornerRadius = CornerRadius*scaleFactor;
				shade.Outside = true;
				shade.RoundBottom = RoundBottomCorners;
				shade.RoundTop = RoundTopCorners;
				shade.ShadePosition = ChartFrameShadePosition.LeftTop;
				shade.Width = (int)(shadeWidth*scaleFactor);
				shade.Render(g);

				// TopLeft light
				shade = new ChartFrameShadePrimitive(Rectangle);
				shade.CornerRadius = CornerRadius*scaleFactor;
				shade.Outside = false;
				shade.RoundBottom = RoundBottomCorners;
				shade.RoundTop = RoundTopCorners;
				shade.ShadePosition = ChartFrameShadePosition.LeftTop;
				shade.Width = (int)(shadeWidth*scaleFactor);
				shade.Color0 = Color.FromArgb(0,255,255,255);
				shade.Color1 = Color.FromArgb(250,255,255,255);
				shade.Render(g);

				// BottomRight dark
				shade = new ChartFrameShadePrimitive(Rectangle);
				shade.CornerRadius = CornerRadius*scaleFactor;
				shade.Outside = false;
				shade.RoundBottom = RoundBottomCorners;
				shade.RoundTop = RoundTopCorners;
				shade.ShadePosition = ChartFrameShadePosition.RightBottom;
				shade.Width = (int)(shadeWidth*scaleFactor);
				shade.Render(g);

				// TopLeft light
				shade = new ChartFrameShadePrimitive(innerRect);
				shade.CornerRadius = CornerRadius*scaleFactor;
				shade.Outside = true;
				shade.RoundBottom = RoundBottomCorners;
				shade.RoundTop = RoundTopCorners;
				shade.ShadePosition = ChartFrameShadePosition.RightBottom;
				shade.Width = (int)(shadeWidth*scaleFactor);
				shade.Color0 = Color.FromArgb(0,255,255,255);
				shade.Color1 = Color.FromArgb(250,255,255,255);
				shade.Render(g);
			}

			if(sharpLines)
			{
				ChartFrameEffect.CreateTopLeftPolygon(innerRect,RoundTopCorners,RoundBottomCorners,CornerRadius*scaleFactor, pts, out nPts);
				for(int i=1;i<nPts;i++)
					g.DrawLine(blackPen,pts[i-1],pts[i]);
				ChartFrameEffect.CreateTopLeftPolygon(Rectangle,RoundTopCorners,RoundBottomCorners,CornerRadius*scaleFactor, pts, out nPts);
				for(int i=1;i<nPts;i++)
					g.DrawLine(whitePen,pts[i-1],pts[i]);
				ChartFrameEffect.CreateRightBottomPolygon(innerRect,RoundTopCorners,RoundBottomCorners,CornerRadius*scaleFactor, pts, out nPts);
				for(int i=1;i<nPts;i++)
					g.DrawLine(whitePen,pts[i-1],pts[i]);
				ChartFrameEffect.CreateRightBottomPolygon(Rectangle,RoundTopCorners,RoundBottomCorners,CornerRadius*scaleFactor, pts, out nPts);
				for(int i=1;i<nPts;i++)
					g.DrawLine(blackPen,pts[i-1],pts[i]);
			}
			whitePen.Dispose();
			blackPen.Dispose();

			RenderText(g, ShadeWidth);
		}
	
		#region --- Serialization & Browsing ---

		private bool ShouldSerializeExtraSpaceTop()		{ return ExtraSpaceTop != 0; } 
		private bool ShouldSerializeExtraSpaceBottom()	{ return ExtraSpaceBottom != 0; } 
		private bool ShouldSerializeExtraSpaceLeft()	{ return ExtraSpaceLeft != 0; } 
		private bool ShouldSerializeRight()				{ return ExtraSpaceRight != 0; } 
		private bool ShouldSerializeFrameColor()		{ return FrameColor != Color.Gray; }
		private bool ShouldSerializeShowSharpLines()	{ return sharpLines; }
		private bool ShouldSerializeShadeWidth()		{ return shadeWidth != 5; }
		private bool ShouldSerializeShowSmoothLines()	{ return !smoothLines; }

		internal void Serialize(XmlCustomSerializer S)
		{
			S.AttributeProperty(this,"ExtraSpaceTop");
			S.AttributeProperty(this,"ExtraSpaceBottom");
			S.AttributeProperty(this,"ExtraSpaceLeft");
			S.AttributeProperty(this,"Right");
			S.AttributeProperty(this,"BorderColor","FrameColor");
			S.AttributeProperty(this,"ShowSharpLines");
			S.AttributeProperty(this,"ShadeWidth");
			S.AttributeProperty(this,"ShowSmoothLines");
		}
		#endregion
	}
}
