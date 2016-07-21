using System;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	internal class Ellipsoid : ParametricFacet
	{
		Vector3D center, axis1, axis2, height;

		internal Ellipsoid() { }

		internal Ellipsoid(Vector3D center, Vector3D axis, Vector3D height, double axis2Length, ChartColor surface)
			: this(center, axis, height, axis2Length, 0.0, Math.PI*2, -Math.PI/2, Math.PI/2, surface)
		{ }

		internal Ellipsoid(Vector3D center, Vector3D axis, Vector3D height, double axis2Length, 
			double u0, double u1, double v0, double v1, ChartColor surface)
			: base(0,0,u0,u1,v0,v1,surface)
		{
			this.center = center;
			this.axis1 = axis;
			this.height = height;
			this.axis2 = height.CrossProduct(axis1).Unit()*axis2Length;
			closed = true;
		}

		#region --- Properties ---

		public Vector3D Center { get { return center; } }
		public Vector3D Axis1 { get { return axis1; } }
		public Vector3D Axis2 { get { return axis2; } }
		public Vector3D Height { get { return height; } }

		internal override double OrderingZ()
		{
			return Mapping.Map(center).Z;
		}

		#endregion

        internal override int GetNu(Geometry.GeometricEngine e)
        {
            Nu = (int)(NumberOfApproximationPointsForEllipse(axis1.Abs, axis2.Abs,e.RenderingPrecisionInPixelSize) * (u1 - u0) / (2 * Math.PI));
            Nu = Math.Max(1, Nu);
            return Nu;
        }

        internal override int GetNv(Geometry.GeometricEngine e)
        {
            Nv = (int)(NumberOfApproximationPointsForEllipse(height.Abs, Math.Max(axis1.Abs, axis2.Abs),e.RenderingPrecisionInPixelSize) * (v1 - v0) / (2 * Math.PI));
            Nv = Math.Max(1, Nv);
            return Nv;
        }

		public override Vector3D Point(double u, double v)
		{
			double cv, sv;
			cv = Math.Cos(v);
			sv = Math.Sin(v);
			return center + (axis1*Math.Cos(u) + axis2*Math.Sin(u))*cv + height*sv;
		}

		public override Vector3D Normal(double u, double v)
		{
			if(v<-Math.PI*0.5+0.00001)
				return -height;
			if(v>Math.PI*0.5-0.00001)
				return height;

			Vector3D dFdu = (-axis1*Math.Sin(u) + axis2*Math.Cos(u))*Math.Cos(v);
			Vector3D dFdv = -(axis1*Math.Cos(u) + axis2*Math.Sin(u))*Math.Sin(v) + height*Math.Cos(v);
			return dFdu.CrossProduct(dFdv);
		}
	}
}

