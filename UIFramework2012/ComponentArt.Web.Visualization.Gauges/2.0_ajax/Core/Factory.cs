using System;
using System.Collections;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for Factory.
	/// </summary>
	[Serializable] 
	internal abstract class Factory: IDisposable
	{
		internal abstract IEngine CreateEngine();
		internal abstract RenderingContext CreateContext();
		internal abstract LayerVisualPart CreateLayerVisualPart(string name,object image);

        internal abstract TickMarkRenderingContext CreateTickMarkRenderingContext(MarkerStyle style);
        internal abstract TextRenderingContext CreateTextRenderingContext(TextStyle textStyle, RenderingContext context, float linearGaugeSize);

		internal abstract ArrayList CreateLayers();
		internal abstract System.Drawing.Design.UITypeEditor GetImageEditor(); 
		public abstract void Dispose();
	}

    internal class TickMarkRenderingContext : IDisposable
    {
		protected MarkerStyle style;

        public TickMarkRenderingContext(MarkerStyle style)
        {
			this.style = style;
        }
        public virtual void Dispose()
        {
        }

		public virtual Color BaseColor { set { } }
    }

    internal abstract class TextRenderingContext : IDisposable
    {
        public TextRenderingContext()
        { }
        public TextRenderingContext(TextStyle style, RenderingContext context, float linearGaugeSize)
        { }
        public virtual void Dispose()
        { }
		public virtual Size2D MeasureString(string text) 
		{ return new Size2D(0,0); }
		public abstract Color FontColor { set; get; }
		public abstract Color AlternateFontColor { set; get; }
		public abstract Color FontBackColor { set; get; }
		public abstract Color AlternateFontBackColor { set; get; }
		public abstract bool DisplayDecimalPoint { set; get; }
	}
}
