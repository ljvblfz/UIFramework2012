using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace ComponentArt.Web.Visualization.Charting
{

	/// <summary>
	/// Summary description for SimpleLine.
	/// </summary>

	// =====================================================================================================
	//	Helper Linked Line Class
	// =====================================================================================================

	internal class SimpleLine
	{
		internal double x;
		internal double y;
		internal double flatness=0.10;
		internal SimpleLine next=null;
		internal bool smooth = false;

        internal SimpleLine() { }

		internal SimpleLine(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		internal SimpleLine(double[] x, double[] y)
		{
			SimpleLine LL = null;
			double lastY = 0;
			for (int i = 0; i < x.Length; i++)
			{
				if (LL == null)
					LL = this;
				else
				{
					LL.next = new SimpleLine(0, 0);
					LL = LL.next;
				}
				LL.x = x[i];
				if (i < y.Length)
				{
					LL.y = y[i];
					lastY = y[i];
				}
				else
					LL.y = lastY;
			}
		}

		internal SimpleLine(float[] x, float[] y)
		{
			SimpleLine LL = null;
			double lastY = 0;
			for (int i = 0; i < x.Length; i++)
			{
				if (LL == null)
					LL = this;
				else
				{
					LL.next = new SimpleLine(0, 0);
					LL = LL.next;
				}
				LL.x = x[i];
				if (i < y.Length)
				{
					LL.y = y[i];
					lastY = y[i];
				}
				else
					LL.y = lastY;
			}
		}


		internal SimpleLine(SimpleLine s)
		{
			SimpleLine c = null;
			SimpleLine LL = s;
			do
			{
				if(c == null)
					c = this;
				else
				{
					c.next = new SimpleLine(0,0);
					c = c.next;
				}
				c.x = LL.x;
				c.y = LL.y;
				c.smooth = LL.smooth;
				LL = LL.next;
			} while(LL != null && LL != s);

			if(LL==s)
				c.next = this;
		}

		internal void LastPoint(out double x, out double y)
		{
			SimpleLine LL = this;
			while(LL.next != null && LL.next != this)
				LL = LL.next;
			x = LL.x;
			y = LL.y;
		}

		internal int Length
		{
			get
			{
				int N = 1;
				SimpleLine LL = this;
				while(LL.next != null && LL.next != this)
				{
					LL = LL.next;
					N++;
				}
				return N;
			}
		}

		internal bool IsPositiveArea()
		{
			double area = 0;
			SimpleLine L = this;
			SimpleLine N = null;
			while((N = L.next)!=null)
			{
				area += (N.x-L.x)*(N.y+L.y);
				L = N;
				if(L == this)
					break;
			}
			
			return area > 0;
		}

		internal void GetAreaCenter(out double xC, out double yC)
		{
			double area = 0;
			double sX = 0;
			double sY = 0;

			double cX = 0;
			double cY = 0;
			double c = 0;

			SimpleLine L = this;
			SimpleLine N = null;
			while((N = L.next)!=null)
			{
				double x1 = N.x;
				double y1 = N.y;
				double x0 = L.x;
				double y0 = L.y;
				cX += x0;
				cY += y0;
				c += 1;
				double a;
				if(x1 != x0)
				{
					a = (y1-y0)/(x1-x0);
					double b = y0 - a*x0;
					// We are integrating
					//   xy = (ax+b)x = axx + bx  for x-coordinate
					//   yy = a2*x2 + 2abx + bb
					sX += a/3*(x1*x1*x1 - x0*x0*x0) + b/2*(x1*x1 - x0*x0);
					sY += a*a/3*(x1*x1*x1 - x0*x0*x0) + a*b*(x1*x1 - x0*x0) + b*b*(x1-x0);
					area += (x1-x0)*(y1+y0);
				}
				L = N;
				if(L == this)
					break;
			}
			if(area != 0)
			{
				area *= 0.5;
				xC = sX/area;
				yC = sY/area/2;
			}
			else
			{
				xC = 0;
				yC = 0;
			}
			cX /=c;
			cY /=c;
		}

		internal bool IsPositiveAreaInProjection(Mapping map, double z)
		{
			double area = 0;
			SimpleLine L = this;
			SimpleLine N = null;
			while((N = L.next)!=null)
			{
				Vector3D LV = new Vector3D(L.x,L.y,z);
				Vector3D NV = new Vector3D(N.x,N.y,z);
				LV = map.Map(LV);
				NV = map.Map(NV);
				area += (NV.X-LV.X)*(NV.Y+LV.Y);
				L = N;
				if(L == this)
					break;
			}
			
			return area > 0;
		}

		internal SimpleLine SmoothLine(bool closed)
		{
			int N = this.Length;
			PointF[] points = new PointF[N];
			SimpleLine LL = this;
			int i=0;
			do
			{
				points[i].X = (float)LL.x;
				points[i].Y = (float)LL.y;
				i++;
				LL = LL.next;
			} while(LL != null && LL != this);

			GraphicsPath path = new GraphicsPath();
			if(closed)
				path.AddClosedCurve(points);
			else
				path.AddCurve(points);
			path.Flatten(new Matrix(1.0f,0.0f,0.0f,1.0f,0.0f,0.0f),(float)flatness);
			PathData pd = path.PathData;
			double[] xx = new double[pd.Points.Length];
			double[] yy = new double[pd.Points.Length];
			for(i=0;i<pd.Points.Length;i++)
			{
				xx[i] = pd.Points[i].X;
				yy[i] = pd.Points[i].Y;
			}
			return new SimpleLine(xx,yy);
		}

		internal SimpleLine StepLine()
		{// The last y is ignored
			SimpleLine R=null;
			SimpleLine LL = null;
			SimpleLine C = this;
			while(C.next != null && C.next != this)
			{
				// Point (x,y)
				if(LL == null)
				{
					LL = new SimpleLine(C.x,C.y);
					R = LL;
				}
				else
				{
					LL.next = new SimpleLine(C.x,C.y);
					LL = LL.next;
				}
				// same y, next x
				LL.next = new SimpleLine(C.next.x,C.y);
				LL = LL.next;
				C = C.next;
			};
			return R;
		}

		internal SimpleLine Append(double x, double y)
		{
			Append(new SimpleLine(x,y));
			return this;
		}

		internal void Append(SimpleLine LL)
		{
			SimpleLine last = this;
			while(last.next != null && last.next != this)
				last = last.next;
			if(last.next != this)
				last.next = LL;
		}

		internal SimpleLine LastPoint()
		{
			SimpleLine last = this;
			while(last.next != null && last.next != this)
				last = last.next;
			return last;
		}

		internal bool IsClosed()
		{
			SimpleLine last = LastPoint();
			return (last.x == x && last.y == y);
		}

		internal void Close()
		{
			SimpleLine last = this;
			// Looking for the last point not having coordinates equal to
			// the coordinates of the first point.
			// (The algorithm is somewhat complicated, to avoid possible
			//  null edges at the end of the line)
			SimpleLine good = null;
			do
			{
				if(last.x != x || last.y != y)
					good = last;
				last = last.next;
			} while(last != null && last != this);
			if(good != null)
				good.next = this;
		}

		internal SimpleLine Reverse()
		{
			SimpleLine cur = this, prev=null, first=null;
			SimpleLine LL = null;
			while(cur != null)
			{
				LL = new SimpleLine(cur.x,cur.y);
				LL.smooth = cur.smooth;
				if(first==null)
					first = LL;
				LL.next = prev;
				prev = LL;
				cur = cur.next;
				if(cur == this)
					break;
			}
			if(cur == this)
				first.next = LL;
			return LL;
		}

		internal SimpleLine FindInternalTriangle()
		{ // It is assumed that the line is closed.
			// The tiangle contains the resulting vertex and the next two 
			SimpleLine LL0 = this,LL1,LL2;
			do
			{
				LL1 = LL0.next;
				if(LL1 == null) break;
				else LL2 = LL1.next;
				if(LL2 == null) break;

				double xn1 = LL1.y-LL0.y;
				double yn1 = LL0.x-LL1.x;
				double xn2 = LL2.y-LL1.y;
				double yn2 = LL1.x-LL2.x;
				double xn3 = LL0.y-LL2.y;
				double yn3 = LL2.x-LL0.x;
				// If triangle positive
				if((LL2.x-LL0.x)*xn1 + (LL2.y-LL0.y)*yn1 >= 0.0)
				{
					SimpleLine LL = LL2.next;
					bool thereIsInternalPoint = false;
					while(LL != LL0)
					{
						// Check if LL is inside triangle (LL0,LL1,LL2)
						if((LL.x-LL0.x)*xn1 + (LL.y-LL0.y)*yn1 > 0.0 &&
							(LL.x-LL1.x)*xn2 + (LL.y-LL1.y)*yn2 > 0.0 &&
							(LL.x-LL2.x)*xn3 + (LL.y-LL2.y)*yn3 > 0.0 )
						{
							thereIsInternalPoint = true;
							break;
						}
						LL = LL.next;
					}
					if(!thereIsInternalPoint)
						return LL0;
				}
				LL0 = LL1;
			} while(LL0 != null && LL0 != this);
			return null;
		}

		internal PointF[] GetPoints()
		{
			return GetPoints(false);
		}
		internal PointF[] GetPoints(bool includeClosingPoint)
		{
			int nPts = Length;
			if(includeClosingPoint)
			{
				if(IsClosed())
					nPts++;
				else
				{
					SimpleLine last = LastPoint();
					if(last.next != null && last.next.x == x && last.next.y == y)
						nPts++;
				}
			}
			PointF[] pts = new PointF[nPts];
			SimpleLine L = this;
			for (int i = 0; i < nPts && L!= null; i++, L = L.next)
			{
				pts[i].X = (float)L.x;
				pts[i].Y = (float)L.y;
			}
			return pts;
		}

		internal int GetPoints(out double[] x, out double[] y, out bool[] smooth)
		{
			return GetPoints(out x, out y, out smooth, false);
		}

		internal int GetPoints(out double[] x, out double[] y, out bool[] smooth, bool includeClosingPoint)
		{
			int nPts = Length;
			if(includeClosingPoint)
			{
				if(IsClosed())
					nPts++;
				else
				{
					SimpleLine last = LastPoint();
					if(last.next != null && last.next.x == this.x && last.next.y == this.y)
						nPts++;
				}
			}
			x = new double[nPts];
			y = new double[nPts];
			smooth = new bool[nPts];
			SimpleLine L = this;
			for (int i = 0; i < nPts; i++, L = L.next)
			{
				x[i] = L.x;
				y[i] = L.y;
				smooth[i] = L.SmoothPoint;
			}
			return nPts;
		}

		internal SimpleLine ReduceToXRange(double x0, double x1)
		{
			// Clone this line

			SimpleLine L = new SimpleLine(this);

			// Insert points at range ends

			bool inserted0 = L.InsertAtX(x0);
			bool inserted1 = L.InsertAtX(x1);

			if(!inserted0 && !inserted1)
			{
				if(x0 <= x && x <= x1)
					return L;
				else
					return null;
			}
	
			SimpleLine LL = L;
			while(L.next != LL && (x0 > L.x || L.x > x1))
				L = L.next;
			if(L.next == LL)
				return null;

			SimpleLine LP = L;
			SimpleLine LN = L.next;
			while(LN != L)
			{
				if(LN.x < x0 || LN.x > x1)
					LP.next = LN.next;
				else
					LP = LN;
				LN = LN.next;
			}

			return L;

		}

		internal bool SmoothPoint { get { return smooth; } set { smooth = value; } }
		internal void SetSmoothLine(bool smooth)
		{
			// in a closed line all points in chain are set the specified smoothness value,
			// in an open line the first and last points are always set value false.

			SimpleLine L = this, lastPoint = null;
			do
			{
				L.SmoothPoint = smooth;
				lastPoint = L;
				L = L.next;
			} while (L != this && L != null);

			if(L == null) // open
			{
				this.smooth = false;
				lastPoint.smooth = false;
			}
		}

		internal ArrayList GetPartitions(double xMin, double xMax,int n)
		{
			// Creates n subsets of line segments intersecting with n intervals between xMin and xMax.
			// Result is used to speed up search for points in intersection algorithms
			ArrayList parts = new ArrayList();
			for(int i=0;i<n;i++)
				parts.Add(new ArrayList());

			double dx = (xMax-xMin)/n;

			SimpleLine SL = this;
			while(SL != null && SL.next != null)
			{
				double x0 = Math.Min(SL.x,SL.next.x);
				double x1 = Math.Max(SL.x,SL.next.x);
				int i0 = (int)((x0-xMin)/dx);
				int i1 = (int)((x1-xMin)/dx);
				for(int i=Math.Max(0,i0); i<Math.Min(n,i1); i++)
				{
					(parts[i] as ArrayList).Add(SL);
				}
				SL = SL.next;
				if(SL == this)
					break;
			}
			return parts;
		}

		internal SimpleLine[] CutInXRange(double x0, double x1)
		{
			// Creates an array of linked lines clipped by x-range (x0,x1)

			// Clone this line

			SimpleLine L = new SimpleLine(this);

			// Insert points at range ends

			bool inserted0 = L.InsertAtX(x0);
			bool inserted1 = L.InsertAtX(x1);

			if(!inserted0 && !inserted1)
			{
				if(x0 <= x && x <= x1)
					return new SimpleLine[] { new SimpleLine(this) };
				else
					return null;
			}

			// Range affects this line

			ArrayList lst = new ArrayList();
			SimpleLine R = null;
			SimpleLine LC = L;
			do
			{
				double xc = LC.x;
				double yc = LC.y;
				LC = LC.next;
				if(LC == null)
					continue;
				double xn = LC.x;
				double yn = LC.y;
				if(x0 <= xc && xc <= x1 && x0 <= xn && xn <= x1) // segment in range
				{
					if(R == null)
					{
						R = new SimpleLine(xc,yc);
						lst.Add(R);
					}
					R.Append(xn,yn);
				}
				else
					R = null;

			} while(LC != null && LC != L);

			if(lst.Count == 0)
				return null;
			SimpleLine[] rez = new SimpleLine[lst.Count];
			for(int i=0;i<lst.Count;i++)
				rez[i] = (SimpleLine)lst[i];

			return rez;
		}

		internal bool InsertAtX(double x0)
		{
			// Insert points at given x - coordinates

			SimpleLine LC = this;
			SimpleLine LN;
			bool inserted = false;
			while(LC.next != null)
			{
				LN = LC.next;
				double xc = LC.x;
				double yc = LC.y;
				double xn = LN.x;
				double yn = LN.y;
				if(Math.Min(xc,xn) < x0 && x0 < Math.Max(xc,xn))
				{
					double a = (x0-xc)/(xn-xc);
					double y = a*yn + (1.0-a)*yc;
					SimpleLine LL = new SimpleLine(x0,y);
					LL.next = LN;
					LC.next = LL;
					inserted = true;
				}
				else if(xc == x0)
					inserted = true; 
				LC = LC.next;
				if(LC == this)
					break;
			}
			return inserted;
		}

		internal bool InsertAtY(double y0)
		{
			// Insert points at given y - coordinates

			SimpleLine LC = this;
			SimpleLine LN;
			bool inserted = false;
			while(LC.next != null)
			{
				LN = LC.next;
				double xc = LC.x;
				double yc = LC.y;
				double xn = LN.x;
				double yn = LN.y;
				if(Math.Min(yc,yn) < y0 && y0 < Math.Max(yc,yn))
				{
					double a = (y0-yc)/(yn-yc);
					double x = a*xn + (1.0-a)*xc;
					SimpleLine LL = new SimpleLine(x,y0);
					LL.next = LN;
					LC.next = LL;
					inserted = true;
				}
				LC = LC.next;
				if(LC == this)
					break;
			}
			return inserted;
		}
	};
}
