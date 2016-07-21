using System;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for Paraboloid.
	/// </summary>
	internal class Paraboloid : ParametricFacet
	{
		Vector3D center;
		Vector3D vertex;
		Vector3D radius;
		double   normRadius;
		double   H1;
		double	 H2;
		Vector3D radiusN;
		Vector3D H;

        internal Paraboloid() { }
        		
		internal Paraboloid(Vector3D center, Vector3D vertex, Vector3D radius, double normRadius, double relativeH1, double relativeH2, ChartColor surface)
			:base(0.0,2*Math.PI, 1.0-relativeH1*relativeH1,1.0-relativeH2*relativeH2,surface)
{
			this.center = center;
			this.vertex = vertex;
			this.radius = radius;
			this.normRadius = normRadius;
			H1 = relativeH1;
			H2 = relativeH2;
			this.surface = surface;
			H = (vertex-center);

			radiusN = (vertex-center).CrossProduct(radius).Unit()*normRadius;
		}

		#region --- Properties ---

		
		internal Vector3D Vertex { get { return vertex; } }
		internal Vector3D BaseCenter { get { return center; } }
		internal Vector3D BaseRadius { get { return radius; } }
		internal double OrthogonalRadius { get { return normRadius; } }
		internal ChartColor ChartColor { get { return surface; } }
		internal double RelativeH1 { get { return H1; } }
		internal double RelativeH2 { get { return H2; } }

		#endregion

		public override Vector3D Point (double u, double v)
		{
			double f = (1-v*v);
			return center + radius*(Math.Cos(u)*v) + radiusN*(Math.Sin(u)*v) + f*H;
		}

		public override Vector3D Normal(double u, double v)
		{
			if(v < 0.0001)
				return H;
			Vector3D dFdu = radius*(-Math.Sin(u)*v) + radiusN*(Math.Cos(u)*v);
			Vector3D dFdv = radius*Math.Cos(u) + radiusN*Math.Sin(u) - H*(2*v);
			return dFdv.CrossProduct(dFdu);
		}

        internal override int GetNu(Geometry.GeometricEngine e)
		{
			return Math.Max(1,NumberOfApproximationPointsForEllipse(radius.Abs,normRadius,e.RenderingPrecisionInPixelSize));
		}

        internal override int GetNv(Geometry.GeometricEngine e)
		{
			double h = (vertex-center).Abs;
			return (int)Math.Max(1,NumberOfApproximationPointsForEllipse(radius.Abs,(H2-H1)*h,e.RenderingPrecisionInPixelSize)/4);
		}
	}
}

