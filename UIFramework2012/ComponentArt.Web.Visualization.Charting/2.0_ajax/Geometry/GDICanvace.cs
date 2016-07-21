using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Summary description for GDICanvace.
	/// </summary>
	internal class GDICanvace : Canvace
	{
		private Graphics graphics;
		private ArrayList pens = new ArrayList();
		private ArrayList brushes = new ArrayList();
		private ArrayList fonts = new ArrayList();

		private GraphicsContainer container;
		int height;

		#region --- Construction and Disposal ---
		public GDICanvace()
		{}

		public GDICanvace(Graphics graphics, int height)
		{
			this.graphics = graphics;
			this.height = height;
			container = graphics.BeginContainer();
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
		}
		
		internal void Dispose(bool disposing)
		{
			graphics.EndContainer(container);
			if(disposing)
			{
				foreach(Pen p in pens)
					p.Dispose();
				foreach(Brush b in brushes)
					b.Dispose();
				foreach(Font f in fonts)
					f.Dispose();
			}
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region --- Drawing Tools: Pens and Gradients ---

		internal override object CreatePen(Color color, float width, DashStyle dashStyle, LineCap endCap)
		{
			float h = 4/width+1;

			Pen pen = new Pen(color, width);
			pen.DashStyle = dashStyle;
			if (endCap == LineCap.SquareAnchor)
			{
				GraphicsPath p = new GraphicsPath();
				p.AddRectangle(new RectangleF(-h / 2f, -h / 2f, h, h));
				CustomLineCap clc = new CustomLineCap(null, p, LineCap.SquareAnchor, h * 0.5f);
				pen.CustomEndCap = clc;
			}
			else if (endCap == LineCap.RoundAnchor)
			{
				GraphicsPath p = new GraphicsPath();
				p.AddEllipse(-h / 2, -h / 2, h, h);
				CustomLineCap clc = new CustomLineCap(null, p, LineCap.SquareAnchor, h * 0.5f);
				pen.CustomEndCap = clc;
			}
			else if (endCap == LineCap.DiamondAnchor)
			{
				GraphicsPath p = new GraphicsPath();
				PointF[] points = new PointF[] 
									{ 
										new PointF(-h/2,0),
										new PointF(0,h/2),
										new PointF(h/2,0),
										new PointF(0,-h/2),
										new PointF(-h/2,0) };
				p.AddPolygon(points);
				CustomLineCap clc = new CustomLineCap(null, p, LineCap.SquareAnchor, h * 0.5f);
				pen.CustomEndCap = clc;
			}
			else if (endCap == LineCap.ArrowAnchor)
			{
				GraphicsPath p = new GraphicsPath();
				PointF[] points = new PointF[] 
					{ 
						new PointF(0,-h),
						new PointF(-h/3,-h),
						new PointF(0,0),
						new PointF(h/3,-h),
						new PointF(0,-h)
					};
				p.AddPolygon(points);
				CustomLineCap clc = new CustomLineCap(null, p, LineCap.SquareAnchor, h);
				pen.CustomEndCap = clc;
			}
			else
				pen.EndCap = endCap;
			pens.Add(pen);
			return pen;
		}

		internal override object CreateBrush(GradientKind kind, GraphicsPath path, Color startColor, Color endColor)
		{
			PointF[] pts = path.PathPoints;
			float x0 = float.MaxValue;
			float y0 = float.MaxValue;
			float x1 = float.MinValue;
			float y1 = float.MinValue;
			for(int i=0; i<pts.Length; i++)
			{
				x0 = Math.Min(x0,pts[i].X);
				y0 = Math.Min(y0,pts[i].Y);
				x1 = Math.Max(x1,pts[i].X);
				y1 = Math.Max(y1,pts[i].Y);
			}
			RectangleF rect = new RectangleF(x0,y0,x1-x0+1,y1-y0+1);
			return CreateBrush(kind,rect,startColor,endColor);
		}

		internal override object CreateBrush(GradientKind kind, RectangleF rect, Color startColor, Color endColor)
		{
			Brush brush = null;
			switch(kind)
			{
				case GradientKind.Center:
				{
					int n = 20;
					PointF[] points = new PointF[n];
					Color[] colors = new Color[n];
					PointF center = new PointF(rect.X + rect.Width/2, rect.Y + rect.Height/2);
					double r = Math.Sqrt(rect.Width*rect.Width + rect.Height*rect.Height)/2;
					for(int i=0;i<n;i++)
					{
						double angle = 2*Math.PI/n;
						double c = Math.Cos(angle);
						double s = Math.Sin(angle);
						points[i] = new PointF(center.X + (float)(c*r),center.Y + (float)(s*r));
						colors[i] = endColor;
					}
					PathGradientBrush pgb = new PathGradientBrush(points);
					brush = pgb;
					pgb.CenterColor = startColor;
					pgb.CenterPoint = center;
					pgb.SurroundColors = colors;
					pgb.WrapMode = WrapMode.TileFlipY;

				}
					break;
				case GradientKind.DiagonalLeft:
				{
					LinearGradientBrush lgb = new LinearGradientBrush(rect,startColor,endColor,135);
					lgb.WrapMode = WrapMode.TileFlipY;
					brush = lgb;
				}
					break;
				case GradientKind.DiagonalRight:
				{
					LinearGradientBrush lgb = new LinearGradientBrush(rect,startColor,endColor,45);
					lgb.WrapMode = WrapMode.TileFlipY;
					brush = lgb;
				}
					break;
				case GradientKind.Vertical:
				{
					LinearGradientBrush lgb = new LinearGradientBrush(rect,startColor,endColor,90);
					lgb.WrapMode = WrapMode.TileFlipY;
					brush = lgb;
				}
					break;
				case GradientKind.Horizontal:
				{
					LinearGradientBrush lgb = new LinearGradientBrush(rect,startColor,endColor,0f);
					lgb.WrapMode = WrapMode.TileFlipY;
					brush = lgb;
				}
					break;
				default:
					brush = new SolidBrush(startColor);
					break;
			}
			brushes.Add(brush);
			return brush;
		}

		internal override object CreateFont(string fontName, float emSize, FontStyle style)
		{
			Font font = new Font(fontName,emSize,style);
			fonts.Add(font);
			return font;
		}
		#endregion

		#region --- Drawing ---

		#region --- Drawing Lines ---
		internal override void DrawPath(object pen, GraphicsPath path)
		{
			Pen p = pen as Pen;
			if(p != null && graphics != null)
				graphics.DrawPath(p,Transform(path));
		}

		internal override void DrawLines(object pen, PointF[] points)
		{
			Pen p = pen as Pen;
			if(p != null && graphics != null)
				graphics.DrawLines(p,Transform(points));
		}

		internal override void DrawLine(object pen, PointF point1, PointF point2)
		{
			Pen p = pen as Pen;
			if(p != null && graphics != null)
				graphics.DrawLine(p,Transform(point1),Transform(point2));
		}

		internal PointF Transform(PointF point)
		{
			return new PointF(point.X,height-point.Y);
		}

		internal PointF[] Transform(PointF[] points)
		{
			PointF[] rPoints = new PointF[points.Length];
			for(int i=0; i<points.Length; i++)
				rPoints[i] = new PointF(points[i].X,height-points[i].Y);
			return rPoints;
		}
		#endregion

        #region --- Filling Regions ---

		internal override void FillPath(object brushOrColor, GraphicsPath path)
		{
			if(graphics == null) return;

			path = Transform(path);
			if(brushOrColor is Brush)
			{
				graphics.FillPath(brushOrColor as Brush,path);
			}
			else if (brushOrColor is Color)
			{
				Brush b = new SolidBrush((Color)brushOrColor);
				graphics.FillPath(b,path);
				b.Dispose();
			}
		}

		
		private GraphicsPath Transform(GraphicsPath path)
		{
			PointF[] pts = path.PathPoints;
			for(int i=0; i<pts.Length; i++)
				pts[i] = new PointF(pts[i].X,height-pts[i].Y);
			return new GraphicsPath(pts,path.PathTypes,path.FillMode);
		}
		#endregion

		#region --- Text Functions ---

		internal override void DrawString(string text, object font, object brushOrColor, RectangleF rectangle, StringAlignment alignment,TextBoxOrientation orientation)
		{
			bool brushCreated;
			if(graphics == null) return;

			Brush brush;
			if(brushOrColor is Brush)
			{
				brush = brushOrColor as Brush;
				brushCreated = false;
			}
			else if (brushOrColor is Color)
			{
				brush = new SolidBrush((Color) brushOrColor);
				brushCreated = true;
			}
			else
				return;

			StringFormat format = new StringFormat();
			format.Alignment = alignment;
			RectangleF tRect = Transform(rectangle);
			float angle = 0;
			if(orientation == TextBoxOrientation.Vertical90)
				angle = 270;
			else if(orientation == TextBoxOrientation.Vertical270)
				angle = 90;
			GraphicsContainer cnt = graphics.BeginContainer();
			graphics.TranslateTransform(tRect.X+tRect.Width/2,tRect.Y+tRect.Height/2);
			graphics.RotateTransform(angle);
			RectangleF textRectangle;
			if(orientation == TextBoxOrientation.Horizontal)
				textRectangle = new RectangleF(-tRect.Width/2,-tRect.Height/2,tRect.Width,tRect.Height);
			else
				textRectangle = new RectangleF(-tRect.Height/2,-tRect.Width/2,tRect.Height,tRect.Width);
			graphics.DrawString(text,font as Font,brush,textRectangle,format);
			graphics.EndContainer(cnt);
			format.Dispose();
			
			if(brushCreated)
				brush.Dispose();
		}

		private RectangleF Transform(RectangleF rect)
		{
			return new RectangleF(rect.Left,height-rect.Y-rect.Height,rect.Width,rect.Height);
		}

		internal override SizeF MeasureString(string text, object font, TextBoxOrientation orientation)
		{
			return MeasureString(text,font,int.MaxValue,orientation);
		}

		internal override SizeF MeasureString(string text, object font, int width, TextBoxOrientation orientation)
		{
			if(graphics == null || !(font is Font))
				return SizeF.Empty;

			SizeF size = graphics.MeasureString(text, font as Font,width);
			if(orientation != TextBoxOrientation.Horizontal)
				size = new SizeF(size.Height,size.Width);
			return size;
		}

		#endregion

		#endregion

	}
}
