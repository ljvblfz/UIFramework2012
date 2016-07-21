using System;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for Cone.
	/// </summary>

	// --------------------------------------------------------------------------------------------------

	internal class Cone : GeometricObject
	{
		Vector3D center;
		Vector3D vertex;
		Vector3D radius;
		ChartColor  surface;
		double   normRadius;
		double   H1;
		double	 H2;

        internal Cone()
        { }
        
		internal Cone(Vector3D center, Vector3D vertex, Vector3D radius, double normRadius, double relativeH1, double relativeH2, ChartColor surface)
{
			this.center = center;
			this.vertex = vertex;
			this.radius = radius;
			this.normRadius = normRadius;
			H1 = relativeH1;
			H2 = relativeH2;
			this.surface = surface;
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
    }
}

