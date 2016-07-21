using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using ComponentArt.Web.Visualization.Gauges;

namespace ComponentArt.Web.Visualization.Gauges.GDIEngine
{
	/// <summary>
	/// GDI part defined by a bitmap.
	/// </summary>
	internal class GDILayerVisualPart : LayerVisualPart
	{
		private GraphicsPath contour;
		public GDILayerVisualPart(string name) : base(name) { }
			public GDILayerVisualPart() : base() { }

		public GraphicsPath Contour { get { return contour; } set { contour = value; } }

		public void CreateContourRing(float relInnerRadius, float relOuterRadius)
		{
			contour = new GraphicsPath();
			contour.AddEllipse(0,0, 2*relInnerRadius, 2*relInnerRadius);
			contour.AddEllipse(0,0, 2*relOuterRadius, 2*relOuterRadius);
		}
		public void CreateContourCircle(float relRadius)
		{
			contour = new GraphicsPath();
			contour.AddEllipse(0,0, 2*relRadius, 2*relRadius);
		}
		internal override void Render(RenderingContext renderingContext)
		{ }
		internal override void RenderAsRegion(RenderingContext renderingContext, Color backColor)
		{ }
		internal override void RenderAfterSettingColor(RenderingContext renderingContext, Color color)
		{ }
		internal override MapAreaCollection CreateMapAreas()
		{ return null; }

	}
}
