using System;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for Ellipse.
	/// </summary>
    internal class Ellipse : GeometricObject
    {
        protected Vector3D C;
        protected Vector3D R1;
        protected Vector3D R2;
        protected int n = 0;
        protected ChartColor surface;

		public Ellipse() { }

        public Ellipse(Vector3D C, Vector3D R1, Vector3D R2, ChartColor surface)
        {
            this.C = C;
            this.R1 = R1;
            this.R2 = R2;
            this.surface = surface;
        }

        internal int Nt { get { return n; } set { n = value; } }
        internal Vector3D Center { get { return C; } }
        internal Vector3D Radius1 { get { return R1; } }
        internal Vector3D Radius2 { get { return R2; } }
        internal ChartColor Color { get { return surface; } }
    }
}
