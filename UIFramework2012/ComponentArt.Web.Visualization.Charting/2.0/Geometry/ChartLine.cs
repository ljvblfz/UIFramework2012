using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	internal class ChartLine : LineDirectrice
	{
		private string		lineStyle;
        private LineStyle   lineStyleRef;
		protected ChartColor	surface = null;
		internal ChartLine()
		{}

		internal ChartLine(string lineStyle, Vector3D P, Vector3D Vx, Vector3D Vy)
		{
			SetPlane(P,Vx,Vy);
			this.lineStyle = lineStyle;
		}

		internal ChartLine(Vector3D P, Vector3D Vx, Vector3D Vy) : this(null,P,Vx,Vy) { }

		public string LineStyle
		{
			get { return lineStyle; }
			set { lineStyle = value; }
		}

		public ChartColor ChartColor { get { return surface; } set { surface = value; } }

		internal override double OrderingZ()
		{
			// Center of mass
			Vector3D S = new Vector3D(0,0,0);
			double sw = 0;
			Mapping map = this.Mapping;
			for(int i=1; i<this.Nt; i++)
			{
				double d = (Point(i)-Point(i-1)).Abs;
				S = S + (Point(i-1) + Point(i))*d;
				sw = sw + d;
			}
			sw *= 2;
			if(sw > 0)
				S = S/sw;
			return map.Map(S).Z;
		}

		internal ChartLine AddPoints(SimpleLine LL)
		{
			int N = 0;
			SimpleLine c = LL;
			while(c != null)
			{
				N++;
				c = c.next;
				if(c==LL)
					break;
			}
			double[] xx = new double[N];
			double[] yy = new double[N];
			c = LL;
			for(int i=0;i<N;i++)
			{
				xx[i] = c.x;
				yy[i] = c.y;
				c = c.next;
			}
			AddPoints(xx,yy);
			return this;
		}

		public ChartLine AddPoints(PointF[] points)
		{
			if(this.points == null)
			{
				this.points = (PointF[])points.Clone();
			}
			else
			{
				PointF[] newPoints = new PointF[this.points.Length + points.Length];
				int i;
				for(i=0;i<this.points.Length;i++)
					newPoints[i] = this.points[i];
				for(int j=0;j<points.Length;j++)
				{
					newPoints[i] = points[j];
					i++;
				}
				this.points = newPoints;
			}
			return this;
		}

		public ChartLine AddPoints(double[] x, double[] y)
		{
			PointF[] newPoints;
			if(this.points == null)
				newPoints = new PointF[x.Length];
			else
				newPoints = new PointF[this.points.Length + x.Length];

			int i=0;
			if(this.points != null)
			{
				for(i=0;i<this.points.Length;i++)
					newPoints[i] = this.points[i];
			}
			for(int j=0;j<x.Length;j++)
			{
				newPoints[i] = new PointF((float)x[j],(float)y[j]);
				i++;
			}
			this.points = newPoints;
			return this;
		}

		public ChartLine AddPoints(float x0, float y0, float x1, float y1)
		{
			PointF[] pts = new PointF[2];
			pts[0].X = x0; pts[0].Y = y0; 
			pts[1].X = x1; pts[1].Y = y1; 
			AddPoints(pts);
			return this;
		}

		public ChartLine AddPoint(float x, float y)
		{
			PointF[] pts = new PointF[1];
			pts[0].X = x; pts[0].Y = y; 
			AddPoints(pts);
			return this;
		}

		public bool Closed
		{
			get { return isClosed; }
			set { isClosed = value; }
		}
        
		internal PointF[] Points 
		{
			get {return points;}
			set {points = value;}
		}	
		
        internal LineStyle StyleRef
		{
			get { return lineStyleRef; }
            set { lineStyleRef = value; }
		}

		internal Vector3D GetP0() { return P; }
		internal Vector3D GetAx() { return Vx; }
		internal Vector3D GetAy() { return Vy; }

        internal void Render(ILineGeneratrice gen, ComponentArt.Web.Visualization.Charting.Shader.RenderingEngine e)
		{
			if(surface != null)
				gen.SetSurface(surface);
			base.Render(gen,e);
		}
	}
}
