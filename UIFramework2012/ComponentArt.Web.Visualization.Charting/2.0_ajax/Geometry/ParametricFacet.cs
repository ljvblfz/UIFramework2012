using System;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for ParametricFacet.
	/// </summary>
	internal abstract class ParametricFacet : GeometricObject
	{
		protected int		nu = 0,nv = 0;
		protected double	u0,u1, v0,v1;
		protected ChartColor	surface;
		protected bool		closed = true;

        internal ParametricFacet() { }

		internal ParametricFacet(double u0, double u1, double v0, double v1, ChartColor surface)
			:this(0,0,u0,u1,v0,v1,surface)
		{ }

		internal ParametricFacet(int Nu, int Nv, double u0, double u1, double v0, double v1, ChartColor surface)
		{
			this.nu = Nu;
			this.nv = Nv;
			this.u0 = u0;
			this.v0 = v0;
			this.u1 = u1;
			this.v1 = v1;
			this.surface = surface;
		}

		public int		Nu		{ get { return nu; } set { nu = value; } }
		public int		Nv		{ get { return nv; } set { nv = value; } }
		public double	U0		{ get { return u0; } set { u0 = value; } }
		public double	U1		{ get { return u1; } set { u1 = value; } }
		public double	V0		{ get { return v0; } set { v0 = value; } }
		public double	V1		{ get { return v1; } set { v1 = value; } }
		public double	DU		{ get { return (u1-u0)/Nu; } }
		public double	DV		{ get { return (v1-v0)/Nv; } }
		public ChartColor	ChartColor	{ get { return surface; }	set { surface = value; } }
		public bool 	Closed	{ get { return closed; }	set { closed = value; } }

		public abstract Vector3D Point (double u, double v);
		public abstract Vector3D Normal(double u, double v);
		internal virtual int GetNu(Geometry.GeometricEngine e) { return 10; }
        internal virtual int GetNv(Geometry.GeometricEngine e) { return 10; }
	}
}
