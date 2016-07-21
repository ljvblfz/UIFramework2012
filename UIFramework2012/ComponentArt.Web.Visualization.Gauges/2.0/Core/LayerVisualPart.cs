using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for LayerVisualPart.
	/// </summary>
	internal abstract class LayerVisualPart : NamedObject
	{
		private bool canRotate = true;
		private Point2D relativeCenterPoint = new Point2D(50,50);
		private Point2D relativeEndPoint = new Point2D(100,50);
		private Size2D size;
		private Size2D cornerSize = new Size2D(0,0);

		#region --- Construction and setup ---

		public LayerVisualPart() : base() { }
		public LayerVisualPart(string name) : base (name) { }

		#endregion

		#region --- Properties ---

		public Point2D RelativeCenterPoint { get { return relativeCenterPoint; } set { relativeCenterPoint = value; } }
		public Point2D RelativeEndPoint { get { return relativeEndPoint; } set { relativeEndPoint = value; } }
		internal virtual Size2D Size { get { return size; } set { size = value; } }
		public bool CanRotate { get { return canRotate; } set { canRotate = value; } }
		internal Size2D  CornerSize { get { return cornerSize; } set { cornerSize = value; } }

		#endregion

		#region --- Rendering

		internal abstract MapAreaCollection CreateMapAreas();
		internal abstract void RenderAsRegion(RenderingContext renderingContext, Color backColor);
		internal virtual void RenderAsNiddleRegion(RenderingContext renderingContext, Color backColor) { RenderAsRegion(renderingContext,backColor); }
		internal abstract void Render(RenderingContext renderingContext);
		internal abstract void RenderAfterSettingColor(RenderingContext renderingContext, Color color);
		internal virtual void RenderAsNiddle(RenderingContext renderingContext) { Render(renderingContext); }
		#endregion
	}

	internal class LayerVisualPartCollection : NamedObjectCollection
	{
		internal override NamedObject CreateNewMember()
		{
			return null;
		}
		public new LayerVisualPart this[object ix]
		{
			get { return base[ix] as LayerVisualPart; }
			set { base[ix] = value; }
		}
//
//		public new LayerVisualPart this[string name] 
//		{ 
//			get { return base[name] as LayerVisualPart; }
//			set { base[name] = value; }
//		}
	}

}
