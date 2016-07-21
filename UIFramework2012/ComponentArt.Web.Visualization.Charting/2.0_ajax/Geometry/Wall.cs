using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Summary description for Wall.
	/// </summary>
	internal class Wall : GeometricObject
	{
		private Vector3D	P,Vx,Vy;
        private SimpleLineCollection lines;
		private double		height;
		private ChartColor	frontSurface = null;

        public Wall() { }

        public Wall(Vector3D P, Vector3D Vx, Vector3D Vy, SimpleLineCollection lines,
            double height, ChartColor frontSurface)
        {
            this.P = P;
            this.Vx = Vx;
            this.Vy = Vy;
            this.lines = lines;
            this.height = height;
            this.frontSurface = frontSurface;
        }

        public Wall(Vector3D P, Vector3D Vx, Vector3D Vy, float[] x, float[] y,
            double height, ChartColor frontSurface)
        {
            this.P = P;
            this.Vx = Vx;
            this.Vy = Vy;
            this.lines = new SimpleLineCollection();
            lines.Add(new SimpleLine(x,y));
            this.height = height;
            this.frontSurface = frontSurface;
        }

		internal GeometricObject GetBandSections()
		{
			GeometricObject bs = new GeometricObject();
			bs.Parent = this;
			AddBendSections(bs,true);
			AddBendSections(bs,false);
			return bs;
		}

		private void AddBendSections(GeometricObject sections, bool smooth)
		{
			Vector3D VH = YDirection.CrossProduct(XDirection)*Height;

			foreach (SimpleLine sl in lines)
			{
				bool thereIsSmoothSections = false;
				bool thereIsStandardSections = false;
				SimpleLine slc = sl;
				do
				{
					if(slc.SmoothPoint)
					{
						thereIsSmoothSections = true;
						if(thereIsStandardSections)
							break;
					}
					else
					{
						thereIsStandardSections = true;
						if(thereIsSmoothSections)
							break;
					}
					slc = slc.next;
				} while (slc != null && slc != sl);

				if(smooth && !thereIsStandardSections || !smooth && !thereIsSmoothSections)
				{
					// the whole line goes to the band
					PointF[] pointsF = sl.GetPoints(true);
					Vector3D[] points0 = new Vector3D[pointsF.Length];
					Vector3D[] points1 = new Vector3D[pointsF.Length];

					for(int i=0; i<pointsF.Length; i++)
					{
						points0[i] = this.Origin + XDirection*pointsF[i].X + YDirection*pointsF[i].Y;
						points1[i] = points0[i] + VH;
					}

					sections.Add(new Band(points0,points1,Color,sl.IsClosed(),smooth));
				}
				else if(smooth)
				{
					if(!thereIsSmoothSections)
						continue;
					// Find the start of the smooth section. At this point the mooth section contains
					// a series of consecutive smooth points with previous and next non-smooth ones.

					// Looking for nonsmooth-->smooth point combination, to start a line
					slc = sl;
					while(slc.next != null && (slc.SmoothPoint || !slc.next.SmoothPoint)) 
						slc = slc.next;
					if(slc.next == null)
						continue;
					double[] x, y; bool[] s;
                    slc.GetPoints(out x,out y, out s, true);
					int n = x.Length;

					int ii = 0;
					while(true)
					{
						int i0 = ii; // the starting point
						for(ii = i0+1; ii<n && s[ii]; ii++); // skip smooth points
						// at this point !(ii<n && s[ii]) = (ii>=n || !s[ii])
						int i1 = Math.Min(n-1,ii);
						// Create segment [i0,i1]
						int ns = i1-i0+1;
						Vector3D[] points0 = new Vector3D[ns];
						Vector3D[] points1 = new Vector3D[ns];
						for(int i=0; i<ns; i++)
						{
							points0[i] = this.Origin + XDirection*x[i+i0] + YDirection*y[i+i0];
							points1[i] = points0[i] + VH;
						}

						sections.Add(new Band(points0,points1,Color,false,true));

						for(ii=i1; ii<n && !s[ii]; ii++); // skip non-smooth points
						if(ii >= n-1)
							break;
						// ii is the first smooth point, so back 1 step to the previous non-smooth point.
						ii--;
					}



				}
				else // not smooth
				{
					if(!thereIsStandardSections)
						continue;
					// Find the start of the smooth section. At this point the mooth section contains
					// a series of consecutive smooth points with previous and next non-smooth ones.

					// Looking for smooth-->nonsmooth point combination, to start a line
					slc = sl;
					while(slc.next != null && !(slc.SmoothPoint && !slc.next.SmoothPoint)) 
						slc = slc.next;
					if(slc.next == null)
						continue;
					slc = slc.next;

					// emitting the segments
					double[] x, y; bool[] s;
					slc.GetPoints(out x,out y, out s, true);
					int n = x.Length;

					int ii = 0;
					while(true)
					{
						int i0 = ii; // the starting point
						for(ii = i0+1; ii<n && !s[ii]; ii++); // skip nonsmooth points
						// at this point !(ii<n && !s[ii]) = (ii>=n || s[ii]),
						// i.e. ii is one step beyond the last non smooth point of the segment
						int i1 = Math.Min(n-1,ii-1);
						// Create segment [i0,i1]
						int ns = i1-i0+1;
						Vector3D[] points0 = new Vector3D[ns];
						Vector3D[] points1 = new Vector3D[ns];
						for(int i=0; i<ns; i++)
						{
							points0[i] = this.Origin + XDirection*x[i+i0] + YDirection*y[i+i0];
							points1[i] = points0[i] + VH;
						}
						sections.Add(new Band(points0,points1,Color,false,false));

						for(; ii<n && s[ii]; ii++); // skip smooth points
						// at this point: !(ii<n && s[ii]) = (ii= n || !s[ii])
						if(ii >= n-1)
							break;
					}
				}
			}
		}


        #region --- Properties ---

        public Vector3D Origin { get { return P; } }
        public Vector3D XDirection { get { return Vx; } }
        public Vector3D YDirection { get { return Vy; } }
        public SimpleLineCollection Lines { get { return lines; } }
        public double Height { get { return height; } }
        public ChartColor Color { get { return frontSurface; } }

        #endregion
	}


	/// <summary>
	/// This is 3D facet consisting of a number of 3D quadrilaterals (not necesserily flat)
	/// defined by two lines. The lines have equal number of points. Corresponding points 
	/// are considered connected.
	/// It can be CLOSED and/or SMOOTH.
	/// </summary>
	internal class Band : GeometricObject
	{
		private Vector3D[] line0;
		private Vector3D[] line1;
		private ChartColor chartColor;
		private bool closed;
		private bool smooth;

		public Band() { }

		public Band(Vector3D[] line0, Vector3D[] line1, ChartColor chartColor, bool closed, bool smooth)
		{
			this.line0 = line0;
			this.line1 = line1;
			this.chartColor = chartColor;
			this.closed = closed;
			this.smooth = smooth;
		}

		#region --- Properties ---

		public Vector3D[] Line0 { get { return line0; } }
		public Vector3D[] Line1 { get { return line1; } }
		public ChartColor ChartColor { get { return chartColor; } }
		public bool Smooth { get { return smooth; } }
		public bool Closed { get { return closed; } }

		internal override double OrderingZ()
		{
			// Finding the middle point
			double L = 0;
			int nSeg = line0.Length-1;
			if(closed)
				nSeg ++;
			for(int i=0; i<=nSeg; i++)
			{
				int j = (i+1)%line0.Length; // next point
				L += (line0[j]-line0[i]).Abs;
			}

			double S = L/2;

			L = 0;
			for(int i=0; i<=nSeg; i++)
			{
				int j = (i+1)%line0.Length; // next point
				double d = (line0[j]-line0[i]).Abs;
				L += d;
				if(L >= S)
				{
					double a = (L-S)/d;
					// The middle point
					Vector3D C = (line0[i] + line1[i])*(a/2) + (line0[j] + line1[j])*((1.0-a)/2);
					return Mapping.Map(C).Z;
				}
			}
			
			return 0;

		}
		#endregion
		internal ArrayList GetVisibleSections()
		{
			return GetSections(true);
		}

		internal ArrayList GetInvisibleSections()
		{
			return GetSections(false);
		}

		internal ArrayList GetSections(bool visible)
		{
			int i;
			int np = line0.Length;
			if((line0[0]-line0[np-1]).IsNull)
			{
				np --;
				closed = true;
			}

			ArrayList segments = new ArrayList();
			if(np <= 0)
				return segments;

			int[] flag = new int[np];
			Mapping mapping = Mapping;
			if(mapping == null)
				return segments;
			for(i=0; i<np; i++)
			{
				int iNext = (i+1)%np;
				Vector3D PNext = line0[0];
				PNext = mapping.Map(line0[iNext]);
				Vector3D P = mapping.Map(line0[i]);
				Vector3D PN = mapping.Map(line1[i]);

				PNext.Z = 0;
				P.Z = 0;
				PN.Z = 0;
				int flg = visible?1:0;
				flag[i] = ((PNext-P).CrossProduct(PN-P)).Z>0? flg:1-flg;
			}
			if(!closed)
				flag[np-1] = 2;

			int ii = 0;

			while(true)
			{
				int i0 = -1;
				int i1 = -1;
				for(i=ii; i<ii+np; i++)
				{
					int j = i%np;
					int jp = (i-1+np)%np;
					// Note: even if we fold jp as if the line was closed, this
					// code works for open lines as well, because flag[jp] is set to 2
					// at the end of an open line!
					bool startingCondition = (flag[jp] != 1 && flag[j] == 1);
					if(startingCondition)
					{
						i0 = j;
						break;
					}
				}
				if(i0 == -1)
				{
					if(flag[0] == 1 && visible || flag[0] != 1 && !visible)
					{ // the whole strip is visible
						segments.Add(this);
					}
					return segments;
				}
				else
				{
					for(i=i0+1; i<i0+np; i++)
					{
						int j = i%np;
						int jp = (i-1+np)%np;
						bool endingCondition = (flag[jp] == 1 && flag[j] != 1);
						if(endingCondition)
						{
							i1 = i;
							flag[jp] = 2; // In the next turn, it'll be skipped
							break;
						}
						else
							flag[jp] = 2; // In the next turn, it'll be skipped
					}
					flag[i0] = 2;
				}
				if(i1 < 0)
					i1 = i0+1;
				ii = i1;

				int npts = i1-i0 + 1;
				Vector3D[] seg0 = new Vector3D[npts];
				Vector3D[] seg1 = new Vector3D[npts];
				int n = 0;
				for(i=i0; i<=i1; i++,n++)
				{
					seg0[n] = line0[i%np];
					seg1[n] = line1[i%np];
				}
				Band b = new Band(seg0,seg1,this.ChartColor,false,this.Smooth);
				b.Parent = this;
				segments.Add(b);
			}

			return segments;
		}

		/// <summary>
		/// Returns surrounding polygon
		/// </summary>
		/// <returns></returns>
		internal Polygon GetPolygon()
		{
			int np = line0.Length;
			int nSeg = np - 1;
			if(Closed)
				nSeg ++;
			Vector3D[] points = new Vector3D[nSeg*2 + 3];

			int n = 0;
			int i;
			for(i=0; i<=nSeg; i++,n++)
				points[n] = line1[i%np];
			for(i=nSeg; i>=0; i--,n++)
				points[n] = line0[i%np];

			points[n] = points[0];
			return new Polygon(points,ChartColor,true,false);
		}

	}
}

