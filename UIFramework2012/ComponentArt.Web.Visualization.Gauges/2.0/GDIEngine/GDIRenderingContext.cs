using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Gauges.GDIEngine
{
	/// <summary>
	/// Summary description for CGDIRenderingContext.
	/// </summary>
	internal class GDIRenderingContext : RenderingContext
	{
		public GDIRenderingContext() : base() { }
		public override RenderingContext CreateCopy()
		{
			GDIRenderingContext copy = new GDIRenderingContext();
			base.InitializeNewInstance(copy);
            return copy;
		}

        internal override Size2D Size
        {
            get { return base.Size; }
            set
            {
                // invert y-coordinates
                base.Size = value;
                Matrix inv = new Matrix(1, 0, 0, -1, 0, value.Height);
                matrix.Multiply(inv);
            }
        }

        public override object RenderingTarget 
        {
            get
            {
                Graphics g = base.RenderingTarget as Graphics;
                g.Transform = matrix;
                return g;
            }
            set { base.RenderingTarget = value; } 
        }
	}
}
