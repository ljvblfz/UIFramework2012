using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using ComponentArt.Web.Visualization.Charting.Geometry;


namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	internal class ObjectMapper
	{
		#region --- Internal Class "DataPointSegment" ---

		internal class DataPointSegment
		{
			internal Point	point1, point2;
			internal DataPoint dataPoint;
			internal int Radius = 0; // use default distance to hot spot
			internal bool valid = false;
			// Unit vector
			private  double dx,dy,dd;
			// Unit normal vector
			private  double dxn,dyn;

			internal DataPointSegment(Point point, DataPoint dataPoint) : this(point,point,dataPoint)
			{ }

			internal DataPointSegment(Point point, DataPoint dataPoint, int radius) : this(point,dataPoint)
			{ 
				Radius = radius;
			}

			internal DataPointSegment(Point point1, Point point2, DataPoint dataPoint)
			{
				this.point1 = point1;
				this.point2 = point2;
				this.dataPoint = dataPoint;
				dx = point2.X-point1.X;
				dy = point2.Y-point1.Y;
				dd = dx*dx + dy*dy;
				if(dd>0)
				{
					dd = Math.Sqrt(dd);
					dx = dx/dd;
					dy = dy/dd;
				}
				dxn = dy;
				dyn = -dx;
			}

			internal double Distance(Point point)
			{
				double x = point.X - point1.X;
				double y = point.Y - point1.Y;
				if(dd==0)
					return Math.Sqrt(x*x + y*y);
				// Projection to the segment vector
				double sp = x*dx + y*dy;

				if(0 <= sp && sp <= dd)
					return Math.Abs(x*dxn + y*dyn);

				double d1 = x*x + y*y;

				x = point.X - point2.X;
				y = point.Y - point2.Y;
				double d2 = x*x + y*y;

				if(d1 < d2)
					return Math.Sqrt(d1);
				else
					return Math.Sqrt(d2);
			}

			internal double MaxDistance(LinkedPoint p)
			{
				LinkedPoint pp = p.next;
				double d = Distance(new Point(p.x,p.y));
				while(pp != p)
				{
					d = Math.Max(d,Distance(new Point(pp.x,pp.y)));
					pp = pp.next;
				}
				return d;
			}
		}
		#endregion

		#region --- Internal Class "LinkedPoint" ---
		internal class LinkedPoint
		{
			public int x,y;
			public LinkedPoint prev, next;
			bool		taken;

			public LinkedPoint(int x, int y)
			{
				this.x = x;
				this.y = y;
				prev = this;
				next = this;
			}

			public LinkedPoint(int x, int y, LinkedPoint prev)
			{
				this.x = x;
				this.y = y;
				this.next = prev.next;
				this.prev = prev;
				next.prev = this;
				prev.next = this;
			}

			public LinkedPoint InsertAfter(int x, int y)
			{
				return new LinkedPoint(x,y,this);
			}

			public LinkedPoint InsertBefore(int x, int y)
			{
				return new LinkedPoint(x,y,this.prev);
			}

			public LinkedPoint Remove()
			{
				if(prev == this)
					return null;
				prev.next = this.next;
				next.prev = this.prev;
				LinkedPoint R = this.prev;
				this.prev = null;
				this.next = null;
				return R;
			}

			public LinkedPoint RemoveSpike()
			{
				LinkedPoint p = this;
				while((p != null) && (p != p.prev) &&
					(((p.next.x == p.prev.x) && (p.next.y == p.prev.y))
					|| (p.x == p.prev.x && p.y == p.prev.y)
					))
					p = p.Remove();
				return p;
			}

			public int PointCount()
			{
				int n = 1;
				LinkedPoint p = next;
				while(p != this)
				{
					n++;
					p = p.next;
				}
				return n;
			}

			public Point[] GetPoints()
			{
				int n = PointCount();
				Point[] points = new Point[n];
				LinkedPoint p = this;
				for(int i=0;i<n;i++)
				{
					points[i].X = p.x;
					points[i].Y = p.y;
					p = p.next;
				}
				return points;
			}
			
			public Point[] ApproximationPoints(int maxN, double precision, ObjectMapper m)
			{
				LinkedPoint a = Approximation(maxN,precision,m);
				return a.GetPoints();
			}
			
			public LinkedPoint Approximation(int maxN, double precision, ObjectMapper m)
			{
				// Get approximation with maxN number of points with given precision 

				LinkedPoint pOposite=null, p0 = this, p = next;
				
				while(p != this)
				{
					p.taken = false;
					p = p.next;
				}

				// Indentify points overlapping with other points (for lines that make
				// the region singly connected)

				int msk =  0x0fffffff;
				int msk1 = 0x10000000;
				int msk2 = 0x20000000;
				int [,] mat = m.indexMatrix;
				// clear the trace
				mat[x,y] = mat[x,y] & msk;
				p = next;
				while(p != this)
				{
					mat[p.x,p.y] = mat[p.x,p.y] & msk;
					p = p.next;
				}
				// make trace of multipe points
				mat[x,y] = mat[x,y] | msk1;
				p = next;
				while(p != this)
				{
					if((mat[p.x,p.y] & msk1) == 0)
						mat[p.x,p.y] = mat[p.x,p.y] | msk1;
					else
						mat[p.x,p.y] = mat[p.x,p.y] | msk2;
					p = p.next;
				}

				// Find the first approximation: this point and opposite point
				double d = 0;
				p = next;
				while(p != this)
				{
					double da = Math.Abs(x-p.x)+Math.Abs(y-p.y);
					if(da > d)
					{
						pOposite = p;
						d = da;
					}
					p = p.next;
				}
				if(pOposite == null)
					return null;

				taken = true;
				pOposite.taken = true;
				int nPoints = 2;

				// Add ending multiple points

				p = next;
				while(p != this && nPoints<maxN)
				{
					if(!p.taken && ((mat[p.x,p.y] & msk2) != 0) && 
						(((mat[p.prev.x,p.prev.y] & msk2) == 0) || ((mat[p.next.x,p.next.y] & msk2) == 0)))
					{
						p.taken = true;
						nPoints++;
					}

					p = p.next;
				}

				// Keep taking points untill precision is satisfied or max number of point is reached

				d = double.MaxValue;
				while(nPoints<maxN && d>precision)
				{
					LinkedPoint P0 = this, P1;
					LinkedPoint newPoint = null;
					d = 0;
					// Loop over segments defined by two consecutive taken points
					while(true)
					{
						// Find next taken point
						P1 = P0.next;
						while(!P1.taken) 
							P1 = P1.next;
						// Check all points in this segment
						double dx = P1.x - P0.x;
						double dy = P1.y - P0.y;
						double da = Math.Sqrt(dx*dx+dy*dy);
						if(da != 0)
						{
							// take normal unit direction
							dx = dx/da;
							dy = -dy/da;
							p = P0.next;
							while(p != P1)
							{
								double dd = Math.Abs((p.x-P0.x)*dy + (p.y-P0.y)*dx);
								if(dd>d)
								{
									newPoint = p;
									d = dd;
								}
								p = p.next;
							}
						}
						else if(P1 != P0) 
						{
							// This is a double point in multiply connected region
							p = P0.next;
							while(p != P1)
							{
								double dd = Math.Abs(p.x-P0.x) + Math.Abs(p.y-P0.y);
								if(dd>d)
								{
									newPoint = p;
									d = dd;
								}
								p = p.next;
							}
						}
						// Go to the next segment
						if(P1 == this)
							break;
						P0 = P1;
					}
					
					// Adding the new point
					if(newPoint != null && d>precision)
					{
						newPoint.taken = true;
						nPoints++;
					}
				}

				// Creating output chain
				LinkedPoint outChain = new LinkedPoint(x,y);
				p = next;
				while(p != this)
				{
					if(p.taken)
						outChain = outChain.InsertAfter(p.x,p.y);
					p = p.next;
				}

				return outChain;
			}
		
			public override string ToString()
			{
				return "(" + x + "," + y + ")";
			}
		}

		#endregion

		#region --- Internal Class "RegionToObjectMapping" ---

		internal class RegionToObjectMapping
		{
			object		obj;
			LinkedPoint chain;

			internal RegionToObjectMapping(object obj, LinkedPoint chain)
			{
				this.obj = obj;
				this.chain = chain;
			}

			internal object Object { get { return obj; } }
			internal LinkedPoint Chain { get { return chain; } }
		}

		#endregion
		
		internal int[,]		indexMatrix;
		internal ArrayList	objects;
		internal int		nx,ny;
		private  GeometricEngine engine = null;
		private  bool		trackingEnabled = false;
		private  bool		useMatrixModel = false;
		private	 int		samplingStep = 1;

		private ArrayList	dataPointSegments = new ArrayList();
		private ArrayList	dataPointHotSpots = new ArrayList();
		private int			hotSpotDist = 5; // hot spot distance in pixels
		private Point		offsetPosition;
		private MapAreaCollection mapAreas;

		#region --- Constructors ---

		public ObjectMapper(GeometricEngine ge)
		{
			mapAreas = new MapAreaCollection();
		}

		internal void Clear()
		{
			mapAreas.Clear();
			dataPointSegments.Clear();
			dataPointHotSpots.Clear();
		}

		public GeometricEngine GeometricEngine { get { return engine; } }

		internal void Initialize(GeometricEngine engine, Point offsetPosition, int nx, int ny, bool trackingEnabled, bool useMatrixModel)
		{
			Clear();
			this.trackingEnabled = trackingEnabled;
			this.engine = engine;
			this.useMatrixModel = useMatrixModel;
			if(trackingEnabled)
			{
				this.nx = nx;
				this.ny = ny;
			}
			else
			{
				this.nx = 1;
				this.ny = 1;
			}
			this.offsetPosition = offsetPosition;
		}

		internal object GetObjectAt(int x, int y)
		{
			MapArea area = MapAreas[new Point(x,y)];
			if(area != null)
				return area.Object;
			else
				return null;
		}

		internal int SamplingStep { get { return samplingStep; } }

		internal void CreateMapping()
		{
			mapAreas.Clear();
			if(!trackingEnabled)
				return;

			engine.GetObjectTrackingInfo(out indexMatrix, out objects);

			nx = indexMatrix.GetUpperBound(0)+1;
			ny = indexMatrix.GetUpperBound(1)+1;

			int nObjects = objects.Count;
			int x,y;

			// Create hotspots
			// (Hotspots are being registered by now, we only filter them)

			foreach(DataPointSegment dps in dataPointHotSpots)
			{
				// Check if some close point is visible
				bool pointVisible = false;
				for(x=dps.point1.X-2;x<=dps.point1.X+2;x++)
				{
					if(0<=x && x<nx)
					{
						for(y=dps.point1.Y-2;y<=dps.point1.Y+2;y++)
						{
							if(0<=y && y<ny)
							{
								int objId = indexMatrix[x,y]&0x0fffffff;
								if(objId==0 || dps.dataPoint.OwningSeries == objects[objId-1])
								{
									pointVisible = true;
									break;
								}
							}
						}
					}
				}
				if(pointVisible)
				{
					int xx = dps.point1.X + offsetPosition.X;
					int yy = ny-1-dps.point1.Y+offsetPosition.Y;
					int radius = (dps.Radius<=0? hotSpotDist:dps.Radius);
					MapAreaCircle c = new MapAreaCircle(xx,yy,radius);
					c.SetObject(dps.dataPoint);
					mapAreas.Add(c);
				}
			}
//			useMatrixModel = false;

			if(useMatrixModel)
			{
				for(int i=0; i<objects.Count; i++)
				{
					MapAreaMatrix mx = new MapAreaMatrix(offsetPosition, this.indexMatrix,i+1);
					mapAreas.Add(mx);
					mx.SetObject(objects[i]);
				}
			}
			else
			{
				RemoveSharpEdges();

				// Create regions defined by chains

				for(x=1; x<nx-1; x++)
					for(y=1; y<ny-1; y++)
					{
						if(indexMatrix[x,y] > 0 && indexMatrix[x,y] <=nObjects)
						{
							int index = indexMatrix[x,y]-1;
							LinkedPoint chain = CreateGrowingBoundary(x,y);
							chain = chain.Approximation(120,1,this);
							object sObject = objects[index];
							object obj = sObject;
							if(sObject is Series)
							{
								double dd = double.MaxValue;
								foreach(DataPointSegment dps in dataPointSegments)
								{
									if(dps.dataPoint.OwningSeries == sObject)
									{
										double ddd = dps.MaxDistance(chain);
										if(ddd<dd)
										{
											obj = dps.dataPoint;
											dd = ddd;
										}
									}
								}
							}
							// We have to flip y-coordinate because mapping areas have y=0 at the top
							Point[] pts = chain.GetPoints();
							for(int i=0;i<pts.Length;i++)
								pts[i] = new Point(pts[i].X+offsetPosition.X,ny-1-pts[i].Y+offsetPosition.Y);
							MapAreaPolygon map = new MapAreaPolygon(pts);
							map.SetObject(obj);
							mapAreas.Add(map);
						}
					}
			}
		}

		
		#endregion

		#region --- Internal Interface ---

		internal MapAreaCollection MapAreas { get { return mapAreas; } }

		#endregion

		#region --- Data Population ---

		internal void AddDataPointSegment(int x1, int y1, int x2, int y2, DataPoint dataPoint)
		{
			dataPointSegments.Add(new DataPointSegment(new Point(x1,y1),new Point(x2,y2),dataPoint));
		}

		internal void AddDataPointCenter(int x, int y, DataPoint dataPoint)
		{
			dataPointSegments.Add(new DataPointSegment(new Point(x,y),dataPoint));
		}

		internal void AddDataPointHotSpot(int x, int y, DataPoint dataPoint)
		{
			dataPointHotSpots.Add(new DataPointSegment(new Point(x,y),dataPoint));
		}

		internal void AddDataPointHotSpot(Vector3D pTarget, DataPoint dataPoint)
		{
			dataPointHotSpots.Add(new DataPointSegment(new Point((int)(pTarget.X),(int)(pTarget.Y)),dataPoint));
		}

		internal void AddMarkerSegment(int targetX, int targetY, DataPoint dataPoint, int radius)
		{
			dataPointHotSpots.Add(new DataPointSegment(new Point(targetX,targetY),dataPoint,radius));
		}

		internal void AddMapAreaCircle(int x, int y, object obj)
		{
			MapAreaCircle mac = new MapAreaCircle(x,ny-1-y,hotSpotDist);
			mac.SetObject(obj);
			mapAreas.Add(mac);
		}

		internal void AddMapAreaCircle(int x, int y, int radius, object obj)
		{
			MapAreaCircle mac = new MapAreaCircle(x,ny-1-y,radius);
			mac.SetObject(obj);
			mapAreas.Add(mac);
		}

		internal void PrependMapAreaCircle(int x, int y, object obj)
		{
			MapAreaCircle mac = new MapAreaCircle(x,ny-1-y,hotSpotDist);
			mac.SetObject(obj);
			mapAreas.Insert(0,mac);
		}

		internal void AddMapAreaPolygon(Point[] points, int xOffset, int yOffset, object obj)
		{
			int n = points.Length;
			if(n <= 2)
				return;
			Point[] points1 = new Point[n];
			for(int i=0;i<n;i++)
				points1[i] = new Point(points[i].X+xOffset,ny-1-(points[i].Y+yOffset));
			MapAreaPolygon map = new MapAreaPolygon(points1);
			map.SetObject(obj);
			mapAreas.Add(map);
		}

		internal void AddMapAreaPolygon(GraphicsPath path, int xOffset, int yOffset, object obj)
		{
			PointF[] pts = path.PathPoints;
			int n = pts.Length;
			if(n <= 2)
				return;
			
			Point[] points = new Point[n];
			for(int i=0;i<n;i++)
				points[i] = new Point((int)(pts[i].X),(int)(pts[i].Y));
			AddMapAreaPolygon(points,xOffset,yOffset,obj);
		}

		internal void PrependMapAreaPolygon(Point[] points, int xOffset, int yOffset, object obj)
		{
			int n = points.Length;
			if(n <= 2)
				return;
			Point[] points1 = new Point[n];
			for(int i=0;i<n;i++)
				points1[i] = new Point(points[i].X+xOffset,ny-1-(points[i].Y+yOffset));
			MapAreaPolygon map = new MapAreaPolygon(points1);
			map.SetObject(obj);
			mapAreas.Insert(0,map);
		}

		internal void PrependMapAreaPolygon(GraphicsPath path, int xOffset, int yOffset, object obj)
		{
			PointF[] pts = path.PathPoints;
			int n = pts.Length;
			if(n <= 2)
				return;
			
			Point[] points = new Point[n];
			for(int i=0;i<n;i++)
				points[i] = new Point((int)(pts[i].X),(int)(pts[i].Y));
			PrependMapAreaPolygon(points,xOffset,yOffset,obj);
		}

		#endregion

		#region --- Regions Building ---

		private void RemoveSharpEdges()
		{
			int m1 = 0x10000000;
			int mb = 0x0fffffff;
			// Removing pixels that don't have at least three withing sorounding 8 pixels
			for(int x = 1; x<nx-1;x++)
				for(int y=1; y<ny-1; y++)
				{
					int val = indexMatrix[x,y];
					int cnt = 0;
					if(val > 0)
					{
						for(int x1 = x-1; x1<=x+1; x1++)
							for(int y1 = y-1; y1<=y+1; y1++)
								if((indexMatrix[x1,y1] & mb) == val)
									cnt ++;
					}
					if(cnt>=6)
						indexMatrix[x,y] = indexMatrix[x,y] | m1;
				}
			for(int x = 1; x<nx-1;x++)
				for(int y=1; y<ny-1; y++)
				{
					if((indexMatrix[x,y] & m1) != 0)
						indexMatrix[x,y] = indexMatrix[x,y] & mb;
					else
						indexMatrix[x,y] = 0;
				}
		}

		#region --- "Boundary Growing" Method ---

		private LinkedPoint CreateGrowingBoundary(int ix, int iy)
		{
			// Handles multiply connected regions by creating single boundary.
			// Masks-out internal points with "visited" bit

			int mVisited = 0x10000000;
			int x,y;

			LinkedPoint p0,p1;

			int val = indexMatrix[ix,iy];
			if(val == 0)
				return null;

			// Step 0: clear the border for easier indexing
			for(x=0; x<nx;x++)
			{
				indexMatrix[x,0] = 0;
				indexMatrix[x,ny-1] = 0;
			}
			for(y=0; y<ny;y++)
			{
				indexMatrix[0,y] = 0;
				indexMatrix[nx-1,y] = 0;
			}

			// Boundary arount current point

			LinkedPoint p = new LinkedPoint(ix,iy);
			p = p.InsertAfter(ix+1,iy  );
			p = p.InsertAfter(ix+1,iy+1);
			p = p.InsertAfter(ix  ,iy+1);
			indexMatrix[ix,iy] = indexMatrix[ix,iy] | mVisited;

			// Grow the boundary

			bool tryMorePoints = true;

			p0 = p;
			p = p.next;
			while(tryMorePoints || p != p0)
			{
				tryMorePoints = false;

				x = p.x;
				y = p.y;

				//   +-----+
				//   |     |
				//   |     |   X
				//   |     |
				//   +-----*

				while(x == p.next.x && y < p.next.y && indexMatrix[x,y] == val)
				{
					// Extend using cell to the right
					p1 = p.InsertAfter(x+1,y);
					p1.InsertAfter(x+1,y+1);
					p = p.RemoveSpike();
					indexMatrix[x,y] = indexMatrix[x,y] | mVisited;
					tryMorePoints = true;
					p0 = p;
					p = p.next;
				}

				//      X
				//      
				//   +-----*
				//   |     |
				//   |     |
				//   |     |
				//   +-----+

				while(x > p.next.x && p.y == p.next.y && indexMatrix[x-1,y] == val)
				{
					// Extend using cell above
					p1 = p.InsertAfter(x,y+1);
					p1.InsertAfter(x-1,y+1);
					p = p.RemoveSpike();
					indexMatrix[x-1,y] = indexMatrix[x-1,y] | mVisited;
					tryMorePoints = true;
					p0 = p;
					p = p.next;
				}

				//    *-----+
				//    |     |
				// X  |     |
				//    |     |
				//    +-----+
			
				while(p.x == p.next.x && p.y > p.next.y && indexMatrix[x-1,y-1] == val)
				{
					// Extend using cell to the left
					p1 = p.InsertAfter(x-1,y);
					p1.InsertAfter(x-1,y-1);
					p = p.RemoveSpike();
					indexMatrix[x-1,y-1] = indexMatrix[x-1,y-1] | mVisited;
					tryMorePoints = true;
					p0 = p;
					p = p.next;
				}

				//   +-----+
				//   |     |
				//   |     |
				//   |     |
				//   *-----+
				//      
				//      X

				while(p.x < p.next.x && p.y == p.next.y && indexMatrix[x,y-1] == val)
				{
					// Extend using cell below
					p1 = p.InsertAfter(x,y-1);
					p1.InsertAfter(x+1,y-1);
					p = p.RemoveSpike();
					indexMatrix[x,y-1] = indexMatrix[x,y-1] | mVisited;
					tryMorePoints = true;
					p0 = p;
					p = p.next;
				}

				p = p.next;
			}

			p = p.RemoveSpike();
			p1 = p.next;
			while(p1 != p)
			{
				p1 = p1.RemoveSpike().next;
			}

			return p;
		}

		#endregion

		#endregion
	}
}
