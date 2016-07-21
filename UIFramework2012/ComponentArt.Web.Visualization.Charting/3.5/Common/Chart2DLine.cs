using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class Chart2DLine : GeometricObject
	{
		Pen			pen;
		private class Point2D
		{
			public PointF	point;
			public Point2D	next;
		}

		Point2D		firstPoint, lastPoint;

		#region --- Construction ---
		public Chart2DLine()
		{
			pen = Pens.Black;
		}

		public Chart2DLine(Pen pen)
		{
			this.pen = pen;
		}

		public Chart2DLine Add(PointF point)
		{
			Point2D p2d = new Point2D();
			p2d.point = point;
			if(firstPoint != null)
				lastPoint.next = p2d;
			else
				firstPoint = p2d;
			lastPoint = p2d;

			return this;
		}

		public Chart2DLine Add(float x, float y)
		{
			return Add (new PointF(x,y));
		}

		public Chart2DLine Add(Vector3D pointW)
		{
			return Add (OwningChart.Map2D(pointW));
		}

		public void Close()
		{
			Add(LastPoint);
		}

		#endregion

		#region --- First and last point ---
		public PointF FirstPoint
		{
			get { return firstPoint.point; }
		}

		public PointF LastPoint
		{
			get { return lastPoint.point; }
		}
		#endregion

		#region --- Target coordinate range ---
		/// <summary>
		/// Default implementation of coordinate range, based on subobjects.
		/// </summary>
		/// <returns>Target coordinate range of this object</returns>
		internal override TargetCoordinateRange CoordinateRange(bool usingTexts)
		{
			Mapping map = TargetArea.Mapping;
			TargetCoordinateRange tcr = new TargetCoordinateRange();
			Point2D p2d = firstPoint;
			while(p2d != null)
			{
				Vector3D p3d = new Vector3D((double)p2d.point.X,(double)p2d.point.Y,0);
				PointF p2 = map.PostProjectionMap(p2d.point.X,p2d.point.Y);
				tcr.Include(new Vector3D(p2.X,p2.Y,0.0));
				p2d = p2d.next;
			}
			return tcr;
		}
		#endregion

		#region --- Rendering ---

		internal void Render(Graphics g)
		{
			Mapping map = TargetArea.Mapping;
			int yMax = OwningChart.TargetSize.Height;
			PointF p1,p2;
			if(firstPoint == null)
				return;
			Point2D p2d = firstPoint;
			p1 = map.PostProjectionMap(p2d.point.X,p2d.point.Y);
			p1.Y = yMax-p1.Y;
			p2d = p2d.next;
			while(p2d!=null)
			{
				Vector3D p3d = new Vector3D((double)p2d.point.X,(double)p2d.point.Y,0);
				p2 = map.PostProjectionMap(p2d.point.X,p2d.point.Y);
				p2.Y = yMax-p2.Y;
				g.DrawLine(pen,p1,p2);
				p1 = p2;
				p2d = p2d.next;
			}
		}
		#endregion
	}				

}
