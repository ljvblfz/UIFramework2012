using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace ComponentArt.Web.Visualization.Charting
{
	public abstract class TextAnchor
	{
		internal abstract PointF Position { get; }
	}

	public class DataPointAnchor : TextAnchor
	{
		private DataPoint dataPoint;
		private double rX = 0.5, rY = 0.5;

		public DataPointAnchor(DataPoint dataPoint)
		{
			this.dataPoint = dataPoint;
		}

		public double RelativeX { get { return rX; } set { rX = value; } }
		public double RelativeY { get { return rY; } set { rY = value; } }

		internal override PointF Position 
		{
			get
			{
				Vector3D p = dataPoint.TargetArea.Mapping.Map(dataPoint.AtLocal(rX,rY,1));
				return new PointF((float)(p.X),(float)(p.Y));
			}
		}
	}

	public class AxisPointAnchor : TextAnchor
	{
		private AxisAnnotation axisAnnotation;
		private object coordinate;
		private double rX = 0.5;

		public AxisPointAnchor (AxisAnnotation axisAnnotation, object coordinate)
		{
			this.axisAnnotation = axisAnnotation;
			this.coordinate = coordinate;
		}

		public double RelativeX { get { return rX; } set { rX = value; } }
		
		internal override PointF Position 
		{
			get
			{
				double xLCS = axisAnnotation.Axis.Dimension.Coordinate(coordinate) + 
					rX*axisAnnotation.Axis.Dimension.Width(coordinate);
				double xICS = axisAnnotation.Axis.LCS2ICS(xLCS);
				Vector3D wCS = axisAnnotation.WCSCoordinate(xICS);
				Vector3D p = axisAnnotation.Axis.CoordSystem.TargetArea.Mapping.Map(wCS);
				return new PointF((float)(p.X),(float)(p.Y));
			}
		}
	}

	public class PlanePointAnchor : TextAnchor
	{
		private CoordinatePlane plane;
		private object atValue1, atValue2;
		private double rX = 0.5, rY = 0.5;
		public PlanePointAnchor(CoordinatePlane plane, object coordinate1, object coordinate2)
		{
			this.plane = plane; 
			this.atValue1 = coordinate1;
			this.atValue2 = coordinate2;
		}

		public double Relative1 { get { return rX; } set { rX = value; } }

		public double Relative2 { get { return rY; } set { rY = value; } }
	
		internal override PointF Position 
		{
			get
			{
				double xICS = plane.XAxis.LCS2ICS(plane.XAxis.Dimension.Coordinate(atValue1) + rX*plane.XAxis.Dimension.Width(atValue1));
				double yICS = plane.YAxis.LCS2ICS(plane.YAxis.Dimension.Coordinate(atValue1) + rY*plane.YAxis.Dimension.Width(atValue1));
				Vector3D wcs = plane.ICS2WCS(xICS,yICS);
				Vector3D p = plane.CoordinateSystem.TargetArea.Mapping.Map(wcs);
				return new PointF((float)(p.X),(float)(p.Y));
			}
		}
	}
}
