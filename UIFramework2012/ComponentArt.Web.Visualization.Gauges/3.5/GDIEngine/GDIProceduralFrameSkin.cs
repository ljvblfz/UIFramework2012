using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Gauges.GDIEngine
{
	/// <summary>
	/// Summary description for GDIProceduralFrameVisualPart.
	/// </summary>
	internal class GDIProceduralFrameVisualPart : GDILayerVisualPart
	{
		private GaugeKind kind;
		private double relWidth;
		private double rrw0;
		private double rrw1;

		public GDIProceduralFrameVisualPart(string name, GaugeKind kind, double relWidth, double rrw0, double rrw1)
			: base(name)
		{
			this.kind = kind;
			this.relWidth = relWidth;
			this.rrw0 = rrw0;
			this.rrw1 = rrw1;
			this.Size = new Size2D(200,200);

			CreateContour();
		}

		#region --- Private: Contours and brushes ---

		private void AddCircle(GraphicsPath path, float rad)
		{
			rad = rad*Size.Width/2;
			path.AddEllipse(-rad, -rad, 2*rad, 2*rad);
		}
		private void CreateContour()
		{
			Contour = new GraphicsPath();
			if(kind == GaugeKind.Circular)
			{
				AddCircle(Contour,1);
				AddCircle(Contour,1-(float)relWidth);
			}
		}

		private GraphicsPath InnerRelativeContour()
		{
			GraphicsPath path = new GraphicsPath();
			float r0 = 1-(float)relWidth;
			float r1 = r0+(float)(relWidth*rrw0);
			AddCircle(path,r0);
			AddCircle(path,r1);
			return path;
		}
		private GraphicsPath OuterRelativeContour()
		{
			GraphicsPath path = new GraphicsPath();
			AddCircle(path,1);
			AddCircle(path,(float)(1-relWidth+ relWidth*rrw1));
			return path;
		}
		private Brush InnerGradient(RenderingContext renderingContext)
		{
			float a = (float)Math.Sqrt(2)*Size.Width/4;
			PointF p1 = new PointF(-a,a);
			PointF p2 = new PointF(a,-a);
			return new LinearGradientBrush(p1,p2,Color.FromArgb(192,0,0,0),	Color.FromArgb(192,255,255,255));
		}
		private Brush OuterGradient(RenderingContext renderingContext)
		{
			float a = (float)Math.Sqrt(2)*Size.Width/4;
			PointF p1 = new PointF(-a,a);
			PointF p2 = new PointF(a,-a);
			return new LinearGradientBrush(p1,p2,Color.FromArgb(192,255,255,255),Color.FromArgb(192,0,0,0));
		}
		#endregion

		internal override void Render(RenderingContext renderingContext)
		{
			Rectangle2D localRect = new Rectangle2D(-Size.Width/2,-Size.Height/2,
				Size.Width,Size.Height);
			RenderingContext context = renderingContext.SetAreaMapping(localRect,true);
			Graphics g = context.RenderingTarget as Graphics;
			if(g == null)
				throw new Exception("GDIProceduralFrameVisualPart '" + Name + "' cannot render on target type '" + 
					renderingContext.RenderingTarget.GetType().Name + "'");		

			// Background
			Layer layer = ObjectModelBrowser.GetAncestorByType(this,typeof(Layer)) as Layer;
			Color backColor = layer.BackgroundColor;
			Brush brush = new SolidBrush(backColor);
			g.FillPath(brush,Contour);
			brush.Dispose();
			// Outer ring
			brush = OuterGradient(renderingContext);
			g.FillPath(brush,OuterRelativeContour());
			brush.Dispose();
			// Inner ring
			brush = InnerGradient(renderingContext);
			g.FillPath(brush,InnerRelativeContour());
			brush.Dispose();
		}
	}

	internal class GDIProceduralBackgroundVisualPart : GDILayerVisualPart
	{
		private GaugeKind kind;

		public GDIProceduralBackgroundVisualPart(string name, GaugeKind kind)
			: base(name)
		{
			this.kind = kind;
			this.Size = new Size2D(100,100);
			CreateContour();
		}

		#region --- Private: Contours and brushes ---

		private void CreateContour()
		{
			Contour = new GraphicsPath();
			if(kind == GaugeKind.Circular)
			{
				Contour.AddEllipse(-Size.Width/2,-Size.Height/2,Size.Width,Size.Height);
			}
		}

		#endregion

		internal override void Render(RenderingContext renderingContext)
		{
			RenderingContext context = renderingContext
			.SetAreaMapping(
				new Rectangle2D(-Size.Width/2,-Size.Height/2,Size.Width,Size.Height),true);
			Graphics g = context.RenderingTarget as Graphics;
			if(g == null)
				throw new Exception("GDIProceduralBackgroundVisualPart '" + Name + "' cannot render on target type '" + 
					renderingContext.RenderingTarget.GetType().Name + "'");		

			// Background
			Layer layer = ObjectModelBrowser.GetAncestorByType(this,typeof(Layer)) as Layer;
			if(layer != null)
			{
				Color color = layer.BackgroundColor;
				Brush bkBrush = new SolidBrush(color);
				g.FillPath(bkBrush,Contour);
				bkBrush.Dispose();
			}
			// Gradient
			float a = (float)Math.Sqrt(2)*Size.Width/4;
			PointF p1 = new PointF(-a,a);
			PointF p2 = new PointF(a,-a);
			Brush brush = new LinearGradientBrush(p2,p1,Color.FromArgb(64,255,255,255),Color.FromArgb(64,0,0,0));
			g.FillPath(brush,Contour);
			brush.Dispose();
		}
	}

	internal class GDIProceduralNeedleVisualPart : GDILayerVisualPart
	{
		public GDIProceduralNeedleVisualPart(string name) : base(name)
		{
			Contour = new GraphicsPath();
			Contour.AddPolygon(new PointF[] 
				{
					new PointF(0,0),
					new PointF(0,10),
					new PointF(100,6),
					new PointF(100, 4),
					new PointF(0,0)
				}
				);
			this.RelativeCenterPoint = new Point2D(10,50);
			this.RelativeEndPoint = new Point2D(100,50);
			this.Size = new Size2D(100,10);
		}

		private Color ContourColor = Color.FromArgb(128,0,0,0); // NB: get this from palette

		internal override void RenderAsNiddleRegion(RenderingContext context, Color color)
		{
			Graphics g = context.RenderingTarget as Graphics;
			Brush bBrush = new SolidBrush(color);
			g.FillPath(bBrush,Contour);
			bBrush.Dispose();
		}

		internal override void Render(RenderingContext context) 
		{
			Graphics g = context.RenderingTarget as Graphics;
			// NB: Get pen style from the pointer style
			Pen pen = new Pen(ContourColor,1);
			g.DrawPath(pen,Contour);
			pen.Dispose();
		}
	}
}
