using System;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for CylindricSegment.
	/// </summary>
	internal class CylindricSegment: ParametricFacet
	{
		protected Vector3D	C1;
		protected Vector3D	C2;
		protected Vector3D	H;
		protected Vector3D	R1;
		protected Vector3D	R2;

		internal CylindricSegment() { }

		internal CylindricSegment(double alpha0, double alpha1, 
			Vector3D C1, Vector3D C2, Vector3D R1, double r2, ChartColor surface)
			: base(alpha0,alpha1,0,1,surface)
		{ 
			this.C1 = C1;
			this.C2 = C2;
			this.R1 = R1;
			
			H = C2-C1;
			R2 = H.CrossProduct(R1);
			if(!R2.IsNull)
				R2 = R2.Unit()*r2;
			Nv = 1;
		}

        internal override int GetNu(Geometry.GeometricEngine e) 
		{
			if(R2.IsNull)
				return 1;
			int n = (int)(this.NumberOfApproximationPointsForEllipse(R1.Abs,R2.Abs,e.RenderingPrecisionInPixelSize) * Math.Abs(U1-U0)/(2*Math.PI));
			return Math.Max(1,n);
		}

		public override Vector3D Point (double u, double v)
		{
			return C1 + R1*Math.Cos(u) + R2*Math.Sin(u) + H*v;
		}

		public override Vector3D Normal(double u, double v)
		{
			Vector3D dFdu = R1*(-Math.Sin(u)) + R2*Math.Cos(u);
			Vector3D dFdv = H;

			return dFdu.CrossProduct(dFdv);
		}
	}
}
