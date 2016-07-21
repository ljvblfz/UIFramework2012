using System;
using System.Drawing;


namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for Cylinder.
	/// </summary>
	internal class Cylinder : GeometricObject//ChartObject
	{
		protected Vector3D	C1;
		protected Vector3D	C2;
		protected Vector3D	H;
		protected Vector3D	R1;
		protected Vector3D	R2;
		protected double    r2;
		protected double    rEdgePoints;
		protected ChartColor surface;


        // For cloning purposes
        internal Cylinder()
        {
        }

		internal Cylinder(Vector3D C1, Vector3D C2, Vector3D R1, double r2, ChartColor surface)
			: this(C1,C2,R1,r2,1.5,surface) {}
		internal Cylinder(Vector3D C1, Vector3D C2, Vector3D R1, double r2, double rEdgePoints, ChartColor surface)
		{
			this.C1 = C1;
			this.C2 = C2;
			this.R1 = R1;
			this.r2 = r2;
			this.rEdgePoints = rEdgePoints;
			this.surface = surface;
			
			H = C2-C1;
			R2 = H.CrossProduct(R1);
			if(!R2.IsNull)
				R2 = R2.Unit()*r2;
		}

        internal Vector3D BaseCenter { get { return C1; } }
        internal Vector3D TopCenter { get { return C2; } }
        internal Vector3D BaseRadius { get { return R1; } }
        internal double OrthogonalRadius { get { return r2; } }
        internal double EdgeRadius { get { return rEdgePoints; } }
        internal ChartColor ChartColor { get { return surface; } }
	}
}
