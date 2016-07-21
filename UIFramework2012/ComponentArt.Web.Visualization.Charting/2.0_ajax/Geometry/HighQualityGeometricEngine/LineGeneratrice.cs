using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Summary description for LineGeneratrice.
	/// </summary>
	internal interface ILineGeneratrice
	{
        void SetEngine(Shader.RenderingEngine e);
		int    Ns { get; }
		double X(int u);
		double Y(int u);
		// Left derivatives at coordinate u
		double dXdu0(int u);
		double dYdu0(int u);
		// Right derivatives at coordinate u
		double dXdu1(int u);
		double dYdu1(int u);
		// ChartColor per generatrice segment
		ChartColor ChartColor(int iSegment);
		void SetSurface(ChartColor surface);
		// Maximum x-coordinate
		double MaximumX { get; }
	}

	internal class Generatrice : ILineGeneratrice
	{
		double[] x, y;
		bool[]	 smooth;
		int		 n, nc=1, Nt;
		int		 nSimple, nSmooth;
		double	 r,xMax;
		ChartColor[] surfaces = null;
		ChartColor	 surface = null;

		#region --- Static Construction of Sdandard Generatrices ---

		public static Generatrice CreateElliptic(double a, double b, ChartColor surface)
		{
			PointF[] pts = new PointF[]
				{
					new PointF(0,			-(float)b),
					new PointF(-(float)a,	-(float)b),
					new PointF(-(float)a,	0),
					new PointF(-(float)a,	(float)b),
					new PointF(0,			(float)b),
					new PointF((float)a,	(float)b),
					new PointF((float)a,	0),
					new PointF((float)a,	-(float)b),
					new PointF(0,			-(float)b)
				};
			bool[] smooth = new bool[]
				{
					true,
					true,
					true,
					true,
					true,
					true,
					true,
					true,
					true					
				};
			return new Generatrice(pts,smooth,surface);
		}

		public static Generatrice CreateBlock(double width, double height, ChartColor surface)
		{
			float w2 = (float)(width/2);
			float h2 = (float)(height/2);
			PointF[] pts = new PointF[]
				{
					new PointF(0,	-h2),
					new PointF(-w2, -h2),
					new PointF(-w2,  h2),
					new PointF(0,	 h2),
					new PointF(w2,	 h2),
					new PointF(w2,	-h2),
					new PointF(0,	-h2)
				};
			bool[] smooth = new bool[]
				{
					false,
					false,
					false,
					false,
					false,
					false,
					false
			};
			return new Generatrice(pts,smooth,surface);
		}

		
		public static Generatrice CreateRoundBlock(double width, double height, double rw, double rh, ChartColor surface)
		{
			float y = (float)(height/2);
			float x = (float)(width/2);
			float rx = (float)Math.Min(rw,x);
			float ry = (float)Math.Min(rh,y);
			PointF[] pts = new PointF[]
				{
					new PointF(0,		-y),
					new PointF(-x+rx,	-y),
					new PointF(-x,		-y),
					new PointF(-x,		-y+ry),
					new PointF(-x,		y-ry),
					new PointF(-x,		y),
					new PointF(-x+rx,	y),
					new PointF(0,		y),
					new PointF(x-rx,	y),
					new PointF(x,		y),
					new PointF(x,		y-ry),
					new PointF(x,		-y+ry),
					new PointF(x,		-y),
					new PointF(x-rx,	-y),
					new PointF(0,		-y)
				};

			bool[] smooth = new bool[]
				{
					false,	//1
					true,	//2
					true,	//3
					false,	//4
					true,	//5
					true,	//6
					false,	//7
					false,	//8
					true,	//9
					true,	//10
					false,	//11
					true,	//132
					true,	//13
					false,	//14
					false	//15
				};
			return new Generatrice(pts,smooth,surface);
		}

		#endregion

		#region --- Constructors ---
		internal Generatrice(PointF[] pts, bool[] smoothPoint, ChartColor[] surfaces)
		{
			PreparePoints(pts,smoothPoint);
			this.surfaces = surfaces;
		}

		internal Generatrice(PointF[] pts, bool[] smoothPoint, ChartColor surface)
		{
			PreparePoints(pts,smoothPoint);
			this.surface = surface;
		}

		private void PreparePoints(PointF[] pts, bool[] smoothPoint)
		{
			xMax = 0;
			n = pts.Length;
			x = new double[n];
			y = new double[n];
			for(int i=0;i<n; i++)
			{
				x[i] = pts[i].X;
				y[i] = pts[i].Y;
				xMax = Math.Max(xMax,Math.Abs(x[i]));
			}
			smooth = smoothPoint;

			CountSubsegments();
		}

		private void CountSubsegments()
		{
			nSimple = 0;
			nSmooth = 0;
			r = 0;
			for(int i = 0; i<n-1; i++)
			{
				if(smooth[i])
				{
					nSmooth++;
					r = Math.Max(r,(x[i+1]-x[i])*(x[i+1]-x[i]) + (y[i+1]-y[i])*(y[i+1]-y[i]));
					i++;
					r = Math.Max(r,(x[i+1]-x[i])*(x[i+1]-x[i]) + (y[i+1]-y[i])*(y[i+1]-y[i]));
				}
				else
					nSimple++;
			}
		}
		#endregion
	
		#region --- ILineGeneratrice Interface ---

		public void SetSurface(ChartColor surface) { surfaces = null; this.surface = surface; }

		public double U0 { get { return 0; } }
		public double U1 { get { return nSimple + nSmooth*nc; } }
		public int	  Ns { get { return Nt; } }
        public void SetEngine(Shader.RenderingEngine e) 
		{
			if(r > 0)
				nc = Math.Max(1,(int)(e.NumberOfApproximationPointsForEllipse(r,r)/4));
			else
				nc = 0;
			Nt = nSimple + nSmooth*nc;
		}

		public double X(int u)
		{
			while(u<0) u += Nt;
			while(u>Nt) u -= Nt;
			for(int i = 0; i<n-1; i++)
			{
				if(smooth[i])
				{
					if(u<=nc)
					{
						double dx1 = x[i+1]-x[i];
						double dx2 = x[i+2]-x[i+1];
						double a = Math.PI*0.5*u/nc;
						// Equation of the elliptic arc:
						//		T(a) = T[i] + d2*(1-cos(a)) + d1*sin(a)
						return x[i] + dx1*Math.Sin(a) + dx2*(1.0-Math.Cos(a));
					}
					else
					{
						u -= nc;
						i++;
					}
				}
				else
				{
					if(u<=1)
					{
						return x[i+1]*u + x[i]*(1-u);
					}
					else
						u -= 1;
				}
			}
			return 0.0;// never used
		}
		
		public double Y(int u)
		{
			while(u<0) u += Nt;
			while(u>Nt) u -= Nt;
			for(int i = 0; i<n-1; i++)
			{
				if(smooth[i])
				{
					if(u<=nc)
					{
						double dy1 = y[i+1]-y[i];
						double dy2 = y[i+2]-y[i+1];
						double a = Math.PI*0.5*u/nc;
						return y[i] + dy1*Math.Sin(a) + dy2*(1.0-Math.Cos(a));
					}
					else
					{
						u -= nc;
						i++;
					}
				}
				else
				{
					if(u<=1)
					{
						return y[i+1]*u + y[i]*(1-u);
					}
					else
						u -= 1;
				}
			}
			return 0.0;// never used
		}

		// Left derivatives at coordinate u
		public double dXdu0(int u)
		{
			while(u<0) u += Nt;
			while(u>Nt) u -= Nt;
			for(int i = 0; i<n-1; i++)
			{
				if(smooth[i])
				{
					if(u<=nc)
					{
						double dx1 = x[i+1]-x[i];
						double dx2 = x[i+2]-x[i+1];
						double a = Math.PI*0.5*u/nc;
						return dx1*Math.Cos(a) + dx2*Math.Sin(a);
					}
					else
					{
						u -= nc;
						i++;
					}
				}
				else
				{
					if(u<=1)
					{
						return x[i+1] - x[i];
					}
					else
						u -= 1;
				}
			}
			return x[n-1] - x[n-2];
		}

		public double dYdu0(int u)
		{
			while(u<0) u += Nt;
			while(u>Nt) u -= Nt;
			for(int i = 0; i<n-1; i++)
			{
				if(smooth[i])
				{
					if(u<=nc)
					{
						double dy1 = y[i+1]-y[i];
						double dy2 = y[i+2]-y[i+1];
						double a = Math.PI*0.5*u/nc;
						return dy1*Math.Cos(a) + dy2*Math.Sin(a);
					}
					else
					{
						u -= nc;
						i++;
					}
				}
				else
				{
					if(u<=1)
					{
						return y[i+1] - y[i];
					}
					else
						u -= 1;
				}
			}
			return y[n-1] - y[n-2];
		}


		// Right derivatives at coordinate u
		public double dXdu1(int u)
		{
			while(u<0) u += Nt;
			while(u>Nt) u -= Nt;
			for(int i = 0; i<n-1; i++)
			{
				if(smooth[i])
				{
					if(u<nc)
					{
						double dx1 = x[i+1]-x[i];
						double dx2 = x[i+2]-x[i+1];
						double a = Math.PI*0.5*u/nc;
						return dx1*Math.Cos(a) + dx2*Math.Sin(a);
					}
					else
					{
						u -= nc;
						i++;
					}
				}
				else
				{
					if(u<1)
					{
						return x[i+1] - x[i];
					}
					else
						u -= 1;
				}
			}
			return x[n-1] - x[n-2];
		}

		public double dYdu1(int u)
		{
			while(u<0) u += Nt;
			while(u>Nt) u -= Nt;
			for(int i = 0; i<n-1; i++)
			{
				if(smooth[i])
				{
					if(u<nc)
					{
						double dy1 = y[i+1]-y[i];
						double dy2 = y[i+2]-y[i+1];
						double a = Math.PI*0.5*u/nc;
						return dy1*Math.Cos(a) + dy2*Math.Sin(a);
					}
					else
					{
						u -= nc;
						i++;
					}
				}
				else
				{
					if(u<1)
					{
						return y[i+1] - y[i];
					}
					else
						u -= 1;
				}
			}
			return y[n-1] - y[n-2];
		}

		// ChartColor per generatrice segment
		public ChartColor ChartColor(int u)
		{
			if(surfaces == null)
				return surface;

			while(u<0) u += Nt;
			while(u>Nt) u -= Nt;
			for(int i = 0; i<n-1; i++)
			{
				if(smooth[i])
				{
					if(u<=nc)
					{
						return surfaces[i];
					}
					else
					{
						u -= nc;
						i++;
					}
				}
				else
				{
					if(u<=1)
					{
						return surfaces[i];
					}
					else
						u -= 1;
				}
			}
			return surfaces[0];	// never used

		}

		// Maximum x-coordinate
		public double MaximumX { get { return xMax; } }

		#endregion
	}
}
