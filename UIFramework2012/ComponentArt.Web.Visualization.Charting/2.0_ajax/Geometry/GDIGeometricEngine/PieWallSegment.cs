using System;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	internal class PieWallSegment
	{
		private GeometricObject seg;
		private int order = 0;
		private bool xRangeComputed = false;
		private double xMin, xMax;
		public PieWallSegment(GeometricObject seg)
		{
			this.seg = seg;
		}

		internal int Order { get { return order; } set { order = value; } }
		internal GeometricObject GeometricObject { get { return seg; } }

		internal bool AdjustOrderWith(PieWallSegment pws)
		{
			double x0this, x1this, x0pws,x1pws, x0,x1,xs;

			if(pws == this)
				return false;

			GetXRange(out x0this,out x1this);
			pws.GetXRange(out x0pws, out x1pws);
			x0 = Math.Max(x0this,x0pws);
			x1 = Math.Min(x1this,x1pws);
			if(x0 >= x1)
				return false;
			
			xs = (x0 + x1)/2;
			double zthis = ZAtX(xs);
			double zpws = pws.ZAtX(xs);

			if(zthis > zpws && Order <= pws.Order)
				order = pws.Order + 1;
			else if(zthis < zpws && Order >= pws.Order)
				pws.Order = order + 1;
			else
				return false;
			return true;
		}

		private void GetXRange(out double x0, out double x1)
		{
			if(!xRangeComputed)
			{
				xMin = double.MaxValue;
				xMax = double.MinValue;
				Vector3D [] points = null;
				if(seg is Polygon)
				{
					Polygon pol = seg as Polygon;
					points = pol.Points;
				}
				else if(seg is Band)
				{
					Band band = seg as Band;
					points = band.Line0;
				}
				else
					throw new Exception("Implementation: cannot gat x range for '" + this.GetType().Name + "'");

				for(int i=0; i<points.Length; i++)
				{
					// We'll project it to the y=0 plane.
					Vector3D PP = seg.Mapping.Map(new Vector3D(points[i].X,0,points[i].Z));
					xMin = Math.Min(xMin,PP.X);
					xMax = Math.Max(xMax,PP.X);
				}
				xRangeComputed = true;
			}
			x0 = xMin;
			x1 = xMax;
		}

		private double ZAtX(double x)
		{
			Vector3D [] points = null;
			if(seg is Polygon)
			{
				Polygon pol = seg as Polygon;
				points = pol.Points;
			}
			else if(seg is Band)
			{
				Band band = seg as Band;
				points = band.Line0;
			}
			else
				throw new Exception("Implementation: cannot get x range for '" + this.GetType().Name + "'");

			for(int i=0; i<points.Length-1; i++)
			{
				// We'll project it to the y=0 plane.
				Vector3D PP0 = seg.Mapping.Map(new Vector3D(points[i].X,0,points[i].Z));
				Vector3D PP1 = seg.Mapping.Map(new Vector3D(points[i+1].X,0,points[i+1].Z));
				double x0 = Math.Min(PP0.X,PP1.X);
				double x1 = Math.Max(PP0.X,PP1.X);
				if(x0 <= x && x <= x1 && x0 < x1)
				{
					double a = (x-PP0.X)/(PP1.X-PP0.X);
					return a*PP1.Z + (1-a)*PP0.Z;
				}
			}

			throw new Exception("Implementation: x out of range");
		}

		internal static void Sort(PieWallSegment[] segments)
		{
			int n = segments.Length;

			bool tryAggain = true;
			int it = 0;
			while(tryAggain)
			{
				tryAggain = false;
				for(int i=0;i<n;i++)
				{
					for(int j=0;j<n;j++)
					{
						bool changed = segments[i].AdjustOrderWith(segments[j]);
						if(changed)	
							tryAggain = true;
					}
				}
				if(it++ > n)
					break;
			}

			for(int i=0; i<n-1; i++)
				for(int j=i+1; j<n; j++)
				{
					if(segments[j].Order < segments[i].Order)
					{
						PieWallSegment S = segments[i];
						segments[i] = segments[j];
						segments[j] = S;
					}
				}
		}
	}
}
