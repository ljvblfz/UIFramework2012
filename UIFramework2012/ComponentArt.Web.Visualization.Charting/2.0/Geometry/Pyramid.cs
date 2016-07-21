using System;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for Pyramid.
	/// </summary>
	internal class Pyramid : GeometricObject
	{
		int		 nSides;
		Vector3D center;
		Vector3D vertex;
		Vector3D radius;
		ChartColor  surface;
		double   normRadius;
		double   H1;
		double	 H2;

        internal Pyramid() { }

		internal Pyramid(Vector3D center, Vector3D vertex, Vector3D radius, double normRadius, 
			double relativeH1, double relativeH2, int nSides, ChartColor surface)
		{
			this.nSides = nSides;
			this.center = center;
			this.vertex = vertex;
			this.radius = radius;
			this.normRadius = normRadius;
			H1 = relativeH1;
			H2 = relativeH2;
			this.surface = surface;
		}

        internal Vector3D Vertex { get { return vertex; } }
        internal Vector3D BaseCenter { get { return center; } }
        internal Vector3D BaseRadius { get { return radius; } }
        internal double OrthogonalRadius { get { return normRadius; } }
        internal int NumberOfSides { get { return nSides; } }
        internal ChartColor ChartColor { get { return surface; } }
        internal double RelativeH1 { get { return H1; } }
        internal double RelativeH2 { get { return H2; } }

		internal Vector3D BasePoint(int i)
		{
			Vector3D radiusN = (vertex-center).CrossProduct(radius).Unit() * normRadius;
			Vector3D C1 = vertex*H1 + (1.0-H1)*center;
			Vector3D C2 = vertex*H2 + (1.0-H2)*center;
			Vector3D radius1 = radius*(1.0-H1);
			Vector3D radius2 = radius*(1.0-H2);
			Vector3D radiusN1 = radiusN*(1.0-H1);
			Vector3D radiusN2 = radiusN*(1.0-H2);
			Vector3D H = C2-C1;
			double startingAngle = -Math.PI*(0.5-1.0/nSides);
			double angleStep = 2*Math.PI/nSides;

			double angle = startingAngle + i*angleStep;
			return C1 + radius1*Math.Cos(angle) + radiusN1*Math.Sin(angle);
		}

		internal Vector3D TopPoint(int i)
		{
			Vector3D radiusN = (vertex-center).CrossProduct(radius).Unit() * normRadius;
			Vector3D C1 = vertex*H1 + (1.0-H1)*center;
			Vector3D C2 = vertex*H2 + (1.0-H2)*center;
			Vector3D radius1 = radius*(1.0-H1);
			Vector3D radius2 = radius*(1.0-H2);
			Vector3D radiusN1 = radiusN*(1.0-H1);
			Vector3D radiusN2 = radiusN*(1.0-H2);
			Vector3D H = C2-C1;
			double startingAngle = -Math.PI*(0.5-1.0/nSides);
			double angleStep = 2*Math.PI/nSides;

			double angle = startingAngle + i*angleStep;
			return C2 + radius2*Math.Cos(angle) + radiusN2*Math.Sin(angle);
		}
	}
}
