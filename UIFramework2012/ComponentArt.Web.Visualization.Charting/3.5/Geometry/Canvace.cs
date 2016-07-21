using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{

	internal abstract class Canvace : IDisposable
	{
		public Canvace() { }
		public virtual void Dispose() { }
		
		#region --- Drawing Tools: Pens and Gradients ---

		internal abstract object CreatePen(Color color, float width, DashStyle dashStyle, LineCap endCap);
		internal abstract object CreateBrush(GradientKind kind, RectangleF rect, Color startColor, Color endColor);
		internal abstract object CreateBrush(GradientKind kind, GraphicsPath path, Color startColor, Color endColor);
		internal abstract object CreateFont(string fontName, float emSize, FontStyle style);
		#endregion

		#region --- Drawing ---

		#region --- Drawing Lines ---
		internal abstract void DrawPath(object pen, GraphicsPath path);
		internal abstract void DrawLines(object pen, PointF[] points);
		internal abstract void DrawLine(object pen, PointF point1, PointF point2);
		#endregion

        #region --- Filling Regions ---
		internal abstract void FillPath(object brushOrColor, GraphicsPath path);
		#endregion

		#region --- Text Functions ---

		internal abstract void DrawString(string text, object font, object brushOrColor, RectangleF rectangle,  StringAlignment alignment, TextBoxOrientation orientation);
		internal abstract SizeF MeasureString(string text, object font, TextBoxOrientation orientation);
		internal abstract SizeF MeasureString(string text, object font, int width, TextBoxOrientation orientation);

		#endregion

		#endregion

	}
}
