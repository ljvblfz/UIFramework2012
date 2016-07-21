using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Gauges
{
	internal enum MapAreaKind
	{
		Rectangle,
		Circle,
		Polygon,
		Matrix
	};

	// ==============================================================================================

	/// <summary>
	/// Summary description for MapArea.
	/// </summary>
	internal abstract class MapArea
	{
		private object	obj;

		public MapArea() { }

		public abstract MapAreaKind Kind { get; }
		public abstract int[] Coords { get; set; }
		public abstract bool Contains(Point p);
		public object Object { get { return obj; } }
		public string ObjectString 
		{ 
			get
			{
				if(obj == null)
					return "null";
				return ObjectModelBrowser.CreateExpression(obj as IObjectModelNode);
			}
		}
		internal void SetObject(object obj) { this.obj = obj; }
	}

	// ==============================================================================================

	internal class MapAreaRectangle : MapArea
	{
		int x0,y0, x1,y1;

		public MapAreaRectangle() { }
		public MapAreaRectangle(int x0, int y0, int x1, int y1) 
		{
			this.x0 = x0;
			this.y0 = y0;
			this.x1 = x1;
			this.y1 = y1;
		}

		public override MapAreaKind Kind { get { return MapAreaKind.Rectangle; } }
		public override int[] Coords 
		{ 
			get { return new int[] { x0,y0,x1,y1 }; }
			set
			{
				x0 = value[0];
				y0 = value[1];
				x1 = value[2];
				y1 = value[3];
			}
		}
		public int X0 { get { return x0; } set { x0 = value; } }
		public int Y0 { get { return x1; } set { x1 = value; } }
		public int X1 { get { return y0; } set { y0 = value; } }
		public int Y1 { get { return y1; } set { y1 = value; } }
		public override bool Contains(Point p)
		{
			int xp = p.X;
			int yp = p.Y;

			return 
				Math.Min(x0,x1) <= xp &&
				Math.Min(y0,y1) <= yp &&
				xp <= Math.Max(x0,x1) &&
				yp <= Math.Max(y0,y1);
		}
	}

	// ==============================================================================================

	internal class MapAreaCircle : MapArea
	{
		int xc,yc, radius;

		public MapAreaCircle() { }
		public MapAreaCircle(int xc, int yc, int radius)
		{
			this.xc = xc;
			this.yc = yc;
			this.radius = radius;
		}

		public override MapAreaKind Kind { get { return MapAreaKind.Circle; } }
		public override int[] Coords 
		{ 
			get { return new int[] { xc,yc,radius }; }
			set 
			{ 
				xc = value[0];
				yc = value[1];
				radius = value[2];
			}
		}
		public int Xc { get { return xc; } set { xc = value; } }
		public int Yc { get { return yc; } set { yc = value; } }
		public int Radius { get { return radius; } set { radius = value; } }
		public override bool Contains(Point p)
		{
			int dx = p.X-xc;
			int dy = p.Y-yc;
			return dx*dx + dy*dy <= radius*radius;
		}
	}


	// ==============================================================================================

	internal class MapAreaPolygon : MapArea
	{
		int[] x,y;
		public MapAreaPolygon() { }
		public MapAreaPolygon(int[] coords)
		{
			x = new int[coords.Length/2];
			y = new int[coords.Length/2];
			int j = 0;
			for(int i = 0; i<x.Length; i++,j+=2)
			{
				x[i] = coords[j];
				y[i] = coords[j+1];
			}
		}
		public MapAreaPolygon(Point[] points)
		{
			x = new int[points.Length];
			y = new int[points.Length];
			for(int i = 0; i<x.Length; i++)
			{
				x[i] = points[i].X;
				y[i] = points[i].Y;
			}
		}

		internal Point[] Points
		{
			get
			{
				Point[] pts = new Point[x.Length];
				for(int i=0; i<x.Length; i++)
				{
					pts[i] = new Point(x[i],y[i]);
				}
				return pts;
			}
		}

		public override MapAreaKind Kind { get { return MapAreaKind.Polygon; } }
		public override int[] Coords 
		{
			get 
			{
				if(x == null)
					return new int[0] { };

				int[] coords = new int[x.Length*2];
				int j = 0;
				for(int i=0; i<x.Length;i++,j+=2)
				{
					coords[j] = x[i];
					coords[j+1] = y[i];
				}
				return coords;
			}
			set
			{
				int[] coords = value;
				x = new int[coords.Length/2];
				y = new int[coords.Length/2];
				int j = 0;
				for(int i = 0; i<x.Length; i++,j+=2)
				{
					x[i] = coords[j];
					y[i] = coords[j+1];
				}

			}
		}
		public override bool Contains(Point p)
		{
			if(x == null || y == null)
				return false;
			
			bool inside = false;

			int xp = p.X;
			int yp = p.Y;

			int n = x.Length;
			for(int i=0; i<n; i++)
			{
				int j = (i+1)%n;
				if(Math.Max(x[i],x[j])>xp && Math.Min(x[i],x[j])<=xp)
				{
					int x0,y0,x1,y1;
					if(x[i]<x[j])
					{
						x0 = x[i];
						x1 = x[j];
						y0 = y[i];
						y1 = y[j];
					}
					else
					{
						x0 = x[j];
						x1 = x[i];
						y0 = y[j];
						y1 = y[i];
					}
					int s1 = (yp-y0)*(x1-x0);
					int s2 = (xp-x0)*(y1-y0);
					bool pointAbove = s1>s2;
					if(pointAbove)
						inside = !inside;
				}
			}
			return inside;
		}

	}

	// ==============================================================================================

	internal class MapAreaCollection : CollectionBase
	{
		internal MapAreaCollection() 
		{ }

		// Adding member at the lis beginning, because of the way mapareas are processed by browsers
		public void Add (MapArea mapArea)
		{
			if(List.Count == 0)
				List.Add(mapArea);
			else
				List.Insert(0,mapArea);
		}

		public MapArea this[int index] 
		{
			get
			{
				return (MapArea)(base.List[index]);
			}
		}

		public MapArea this[Point point]
		{
			get
			{
				for(int i=0;i<Count; i++)
				{
					if(this[i].Contains(point))
						return this[i];
				}
				return null;
			}
		}
	}
}
