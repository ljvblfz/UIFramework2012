using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for Polygon.
	/// </summary>
	internal class Polygon : GeometricObject
	{
		private Vector3D[]	points;
		private bool		closed,smooth;
		private ChartColor	chartColor;

        public Polygon() { }

		public Polygon(Vector3D[] points, ChartColor chartColor, bool closed, bool smooth)
		{
			this.points = points;
			this.chartColor = chartColor;
			this.closed = closed;
			this.smooth = smooth;
		}

		public Vector3D[] Points { get { return points; } }
		public ChartColor ChartColor { get { return chartColor; } }
		public bool Closed { get { return closed; } }
		public bool Smooth { get { return smooth; } }

		internal override double OrderingZ()
		{
			int n = points.Length;
			double Z = 0;
			
			for(int i=0; i<n; i++)
				Z += Mapping.Map(points[i]).Z;
			return Z/n;
		}
	}
}
