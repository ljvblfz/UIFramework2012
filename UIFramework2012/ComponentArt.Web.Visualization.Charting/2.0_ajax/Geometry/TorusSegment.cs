using System;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for ThorusSegment.
	/// </summary>
	internal class TorusSegment : ParametricFacet
	{
		protected Vector3D	C1;
		protected Vector3D	C2;
		protected Vector3D	H;
		protected Vector3D	R1;
		protected Vector3D	R2;
		protected double	rh;

		private double		f;

        internal TorusSegment() : base() { }

		internal TorusSegment(double alpha0, double alpha1, double beta0, double beta1,
			Vector3D C1, Vector3D C2, Vector3D R1, double r2, double rh, ChartColor surface)
			: base(alpha0,alpha1,beta0,beta1,surface)
		{ 
			this.C1 = C1;
			this.C2 = C2;
			this.R1 = R1;
			this.rh = rh;
			
			H = C2-C1;
			R2 = H.CrossProduct(R1);
			if(!R2.IsNull)
				R2 = R2.Unit()*r2;
			f = rh/R1.Abs;
		}

        internal override int GetNu(Geometry.GeometricEngine e) 
		{
			int n = (int)(NumberOfApproximationPointsForEllipse(R1.Abs,R2.Abs,e.RenderingPrecisionInPixelSize) * Math.Abs(U1-U0)/(2*Math.PI));
			return Math.Max(1,n);
		}

        internal override int GetNv(Geometry.GeometricEngine e) 
		{
			int n = (int)(NumberOfApproximationPointsForEllipse(rh,H.Abs,e.RenderingPrecisionInPixelSize) * Math.Abs(V1-V0)/(2*Math.PI));
			return Math.Max(1,n);
		}

		public override Vector3D Point (double u, double v)
		{
			Vector3D CA = R1*Math.Cos(u) + R2*Math.Sin(u);
			Vector3D A2A = f*CA;
			return C1 + CA*(1.0-f) + A2A*Math.Cos(v) + H*Math.Sin(v);// + H*u*0.35;
		}

		public override Vector3D Normal(double u, double v)
		{
			Vector3D dCAdu = R1*(-Math.Sin(u)) + R2*Math.Cos(u);
			Vector3D dA2Adu = dCAdu*f;
			Vector3D dFdu = dCAdu*(1.0-f) + dA2Adu*Math.Cos(v);

			Vector3D CA = R1*Math.Cos(u) + R2*Math.Sin(u);
			Vector3D A2A = f*CA;
			Vector3D dFdv = A2A*(-Math.Sin(v)) + H*Math.Cos(v);

			return dFdu.CrossProduct(dFdv);
		}

	}
}
