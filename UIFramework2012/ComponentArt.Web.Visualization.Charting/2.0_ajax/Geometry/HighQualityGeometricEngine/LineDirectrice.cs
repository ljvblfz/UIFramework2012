using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	internal class LineDirectrice : GeometricObject
	{
		protected Vector3D	P;
		protected Vector3D	Vx;
		protected Vector3D	Vy;
		protected Vector3D  Vz;
		protected PointF[]	points;
		protected bool		isClosed = false;
		protected bool		isSmooth = false;
		protected JoinStyle joinStyle = JoinStyle.Bevel;

		public LineDirectrice()	{ }

		public void SetPlane(Vector3D P, Vector3D Vx, Vector3D Vy)
		{
			this.P  = P;
			this.Vx = Vx;
			this.Vy = Vy;
			this.Vz = Vx.CrossProduct(Vy);
		}
		
		public int		Nt			{ get { return points.Length; } }
		public bool		IsClosed	{ get { return isClosed; } set { isClosed = value; } }
		public bool		IsSmooth	{ get { return isSmooth; } set { isSmooth = value; } }
		public Vector3D VZ			{ get { return Vz; } }
		public JoinStyle JoinStyle  { get { return joinStyle; } set { joinStyle = value; } }
		public Vector3D Origin		{ get { return P; } }
		internal Vector3D XDirection { get { return Vx; } }
		internal Vector3D YDirection { get { return Vy; } }

		public PointF this[int i] 
		{
			get
			{
				int ix = i;
				if(isClosed)
					ix = i%Nt;
				return points[ix];
			}
		}

		public Vector3D Point(int i)
		{
			if(isClosed)
				i = i%Nt;
			PointF pt = this[i];
			return P + Vx*pt.X + Vy*pt.Y;
		}

		public Vector3D SegmentDirection(int i)
		{
			if(isClosed)
				i = i%Nt;
			int i1 = i+1;
			if(isClosed)
				i1 = i1%Nt;
			return Point(i1)-Point(i);
		}

		public Vector3D SegmentNormalUnit(int i)
		{
			if(isClosed)
				i = i%Nt;
			int i1 = i+1;
			if(isClosed)
				i1 = i1%Nt;
			float dx = points[i1].X-points[i].X;
			float dy = points[i1].Y-points[i].Y;
			return (Vx*(-dy) + Vy*dx).Unit();
		}

        internal void Render(ILineGeneratrice gen, Shader.RenderingEngine e)
		{
			// Shake the coordinates to avoid singularities at joins
			ShakeCoordinates();

			// Number of steps for join
			int nSteps;
			if(joinStyle == JoinStyle.Bevel)
				nSteps = 1;
			else if(joinStyle == JoinStyle.Miter)
				nSteps = 0;
			else
				nSteps =(int)(Math.Max(e.NumberOfApproximationPointsForEllipse(gen.MaximumX,gen.MaximumX)/2+0.5,2));

			LineTile tile = new LineTile(gen,this);
			gen.SetEngine(e);
			int nt = Nt;
			if(isClosed)
				nt ++;
			for (int i=0;i<nt; i++)
			{
				for(int j = 0; j<gen.Ns; j++)
				{
					if(tile.GoTo(i,j))
						tile.Render(e);
				}
				if(nSteps>0)
				{
					double a = 0; 
					double da = 1.0/nSteps;
					for(int k=0; k<nSteps; k++)
					{
						for(int j = 0; j<gen.Ns; j++)
						{
							if(tile.GoToJoin(i,j,a,a+da))
								tile.Render(e);
						}
						a += da;
					}
				}
			}
			if(!isClosed)
			{
				RenderCap(e,gen,true);
				RenderCap(e,gen,false);
			}
		}

        internal void RenderCap(Shader.RenderingEngine e, ILineGeneratrice gen, bool atStart)
		{
			if(Nt<2)
				return;

			Vector3D N,S,P0,P1,DN;
			if(atStart)
			{
				S = P + Vx*points[0].X + Vy*points[0].Y;
				N = -SegmentDirection(0);
				DN = Vx*(points[0].Y-points[1].Y) + Vy*(points[1].X-points[0].X);
				DN = DN.Unit();
			}
			else
			{
				S = P + Vx*points[Nt-1].X + Vy*points[Nt-1].Y;
				N = SegmentDirection(Nt-2);
				DN = Vx*(points[Nt-2].Y-points[Nt-1].Y) + Vy*(points[Nt-1].X-points[Nt-2].X);
				DN = DN.Unit();
			}

			for(int i=1;i<=gen.Ns;i++)
			{
				P0 = S + DN*gen.X(i-1) + Vz*gen.Y(i-1);
				P1 = S + DN*gen.X(i) + Vz*gen.Y(i);

				if(atStart)
					e.RenderElement(S,P1,P0, N,N,N, ConvertToCore.Color(gen.ChartColor(i)), false);
				else
					e.RenderElement(S,P0,P1, N,N,N, ConvertToCore.Color(gen.ChartColor(i)), false);
			}

		}

		#region --- Shaking Coordinates ---
		
		private void ShakeCoordinates()
		{
			for(int i=0; i<Nt-1;i++)
			{
				int iprev = i-1;
				int inext = i+1;
				if(IsClosed)
				{
					iprev = iprev%Nt;
					inext = inext%Nt;
				}
				if(iprev>=0 && inext < Nt)
				{
					// Removing double points
					if(points[inext].X == points[i].X && points[inext].Y == points[i].Y)
					{
						RemovePoint(i);
						ShakeCoordinates();
						break;
					}
					// 2D vectors
					double dxprev = points[i].X - points[iprev].X;
					double dyprev = points[i].Y - points[iprev].Y;
					double dxnext = points[inext].X - points[i].X;
					double dynext = points[inext].Y - points[i].Y;
					// First normal
					double dxnorm = -dyprev;
					double dynorm = dxprev;
					// Scalar product with the second vector
					double sp = dxnorm*dxnext + dynorm*dynext;

					if(sp == 0)
					{
						// Removing unnecessary points
						if(Math.Abs(dxprev)+Math.Abs(dyprev) < Math.Abs(dxprev+dxnext) + Math.Abs(dyprev+dynext))
							RemovePoint(i);
						else
						{
							// There is a U-turn at point i
							// Insert a point and make two 90 degrees turns
							// The complex computation is due to our need to insert a point dinstict from the point "i",
							// but very close to it
							float anorm = (float)Math.Sqrt(dxnorm*dxnorm + dynorm*dynorm);
							float fac = Math.Abs(points[i].X) + Math.Abs(points[i].Y);
							dxnorm *= fac/anorm;
							dynorm *= fac/anorm;
							float newX = (float)(points[i].X + dxnorm*0.00001);
							float newY = (float)(points[i].Y + dynorm*0.00001);
							InsertPoint(i,newX,newY);
							ShakeCoordinates();
						}
						break;
					}
					if(joinStyle == JoinStyle.Miter)
					{
						// Check for sharp joins
						double a2 = Math.Sqrt(dxnext*dxnext + dynext*dynext);
						dxnext /= a2;
						dynext /= a2;
						a2 = Math.Sqrt(dxnorm*dxnorm + dynorm*dynorm);
						dxnorm /= a2;
						dynorm /= a2;
						sp = dxnext*dxnorm + dynext*dynorm;
						if(Math.Abs(sp)<0.3 && dxnext*dxprev + dynext*dyprev < 0)
						{
							if(sp>0)
								InsertPoint(i,points[i].X + a2*dxnorm*0.00001, points[i].Y + a2*dynorm*0.00001);
							else
								InsertPoint(i,points[i].X - a2*dxnorm*0.00001, points[i].Y - a2*dynorm*0.00001);
							ShakeCoordinates();
							break;
						}
					}
				}
			}
		}

		private void RemovePoint(int ix)
		{
			PointF[] newPoints = new PointF[points.Length-1];
			int i;
			for(i=0;i<ix;i++)
				newPoints[i] = points[i];
			
			for(;i<newPoints.Length;i++)
				newPoints[i] = points[i+1];
			points = newPoints;
		}

		private void InsertPoint(int ix, double x, double y)
		{
			PointF[] newPoints = new PointF[points.Length+1];
			int i;
			for(i=0;i<=ix;i++)
				newPoints[i] = points[i];
			newPoints[i] = new PointF((float)x,(float)y);
			i++;
			for(;i<newPoints.Length;i++)
				newPoints[i] = points[i-1];
			points = newPoints;
		}
		#endregion
	}

	internal class LineTile
	{
		ILineGeneratrice	gen;
		LineDirectrice		dir;

		// tile position and workong data
		int					iDir,iGen;
		Vector3D			P0,P1, Q0,Q1;
		Vector3D			NP0,NP1,NQ0,NQ1;

		// Direction and normal to the direction
		Vector3D			D,DN;
		// The same for the next segment, used in joins
		Vector3D			D1,DN1;
		// The join point
		Vector3D		    S;

		internal LineTile(ILineGeneratrice gen, LineDirectrice dir)
		{
			this.gen = gen;
			this.dir = dir;
		}

		#region --- Tile on a line section ---

		internal bool GoTo(int iDir, int iGen)
		{
			if(dir.IsClosed)
				iDir = iDir%dir.Nt;
			if(iDir > dir.Nt-2 || iGen >= gen.Ns)
				return false;
			this.iDir = iDir;
			this.iGen = iGen;
			D = dir.SegmentDirection(iDir).Unit();
			DN = dir.SegmentNormalUnit(iDir);

			GetPoints();
			GetNormals();
			return true;
		}

		protected virtual void GetPoints()
		{
			GetPoints(iGen, out P0, out Q0);
			GetPoints(iGen+1, out P1, out Q1);
		}

		protected virtual void GetPoints(int iGen, out Vector3D P, out Vector3D Q)
		{
			Vector3D S0 = dir.Point(iDir);
			Vector3D S1 = dir.Point(iDir+1);
	
			P = S0 + DN*gen.X(iGen) + dir.VZ*gen.Y(iGen);
			Q = S1 + DN*gen.X(iGen) + dir.VZ*gen.Y(iGen);

			if(dir.JoinStyle == JoinStyle.Miter)
			{
				// Starting of the tile
				double slope0 = 0;
				if(dir.IsClosed || iDir > 0)
				{
					Vector3D sim = DN + dir.SegmentNormalUnit(iDir-1);
					double spx = sim*DN;
					if(spx != 0)
						slope0 = sim*D/spx;
				}
				double s = slope0*gen.X(iGen);
				P = P + D*s;

				// Ending of the tile
				double slope1 = 0;
				if(dir.IsClosed || iDir < dir.Nt-2)
				{
					Vector3D sim = DN + dir.SegmentNormalUnit(iDir+1);
					double spx = sim*DN;
					if(spx != 0)
						slope1 = sim*D/spx;
				}
				s = slope1*gen.X(iGen);
				Q = Q + D*s;
			}
		}

		protected virtual void GetNormals()
		{
			Vector3D Tangent, DA,DNA;			

			if(dir.IsSmooth)
			{
				if(dir.IsClosed || iDir > 0)
				{
					DA = (dir.SegmentDirection(iDir-1).Unit() + D.Unit()).Unit();
					DNA = (dir.SegmentNormalUnit(iDir-1) + DN).Unit();
				}
				else
				{
					DA = D;
					DNA = DN;
				}
				Tangent = DNA*gen.dXdu1(iGen) + dir.VZ*gen.dYdu1(iGen);
				NP0 = DA.CrossProduct(Tangent);
				Tangent = DNA*gen.dXdu0(iGen+1) + dir.VZ*gen.dYdu0(iGen+1);
				NP1 = DA.CrossProduct(Tangent);

				if(dir.IsClosed || iDir < dir.Nt-2)
				{
					DA = (dir.SegmentDirection(iDir+1).Unit() + D.Unit()).Unit();
					DNA = (dir.SegmentNormalUnit(iDir+1) + DN).Unit();
				}
				else
				{
					DA = D;
					DNA = DN;
				}
				Tangent = DNA*gen.dXdu1(iGen) + dir.VZ*gen.dYdu1(iGen);
				NQ0 = DA.CrossProduct(Tangent);
				Tangent = DNA*gen.dXdu0(iGen+1) + dir.VZ*gen.dYdu0(iGen+1);
				NQ1 = DA.CrossProduct(Tangent);
			}
			else
			{
				Tangent = DN*gen.dXdu1(iGen) + dir.VZ*gen.dYdu1(iGen);
				NP0 = D.CrossProduct(Tangent);
				Tangent = DN*gen.dXdu0(iGen+1) + dir.VZ*gen.dYdu0(iGen+1);
				NP1 = D.CrossProduct(Tangent);
				NQ0 = NP0;
				NQ1 = NP1;
			}
		}


		#endregion

		#region --- Tile on a join ---

		internal bool GoToJoin(int iDir, int iGen, double aStart, double aEnd)
		{
			// Consructing tile at point iDir.
			// aStart, aEnd are interpolation factors for starting and ending point of the tile
			// between starting and ending point of the join; 0 <= aStart < aEnd <= 1

			// D,DN are directions before the join at directrice point iDir
			// D1,DN1 are directions after the join

			if(!dir.IsClosed && (iDir<=0 || iDir>=dir.Nt-1))
				return false;
			iDir = iDir%dir.Nt;

			if(iGen >= gen.Ns)
				return false;

			this.iDir = iDir;
			this.iGen = iGen;

			S = dir.Point(iDir);

			D = dir.SegmentDirection(iDir-1).Unit();
			DN = dir.SegmentNormalUnit(iDir-1);
			D1 = dir.SegmentDirection(iDir).Unit();
			DN1 = dir.SegmentNormalUnit(iDir);
			if((DN+DN1).Abs < 0.01)
			{
				DN = D.Unit();
				DN1 = DN;
			}

			P0 = JoinPoint(aStart,iGen);
			P1 = JoinPoint(aStart,iGen+1);
			Q0 = JoinPoint(aEnd  ,iGen);
			Q1 = JoinPoint(aEnd  ,iGen+1);

			// Checking the order of points WRT the direction of the line
			Vector3D DS = D+D1;
			if(DS.IsNull)
				return false;
			if((Q0-P0)*DS <=0 && (Q1-P1)*DS <= 0)
				return false;

			if(dir.JoinStyle == JoinStyle.Bevel)
			{
				double aMid = (aStart+aEnd)/2;
				NP0 = JoinNormal(aMid,iGen,false);
				NP1 = JoinNormal(aMid,iGen+1,true);
				NQ0 = JoinNormal(aMid,iGen,false);
				NQ1 = JoinNormal(aMid,iGen+1,true);
			}
			else
			{
				NP0 = JoinNormal(aStart,iGen,false);
				NP1 = JoinNormal(aStart,iGen+1,true);
				NQ0 = JoinNormal(aEnd  ,iGen,false);
				NQ1 = JoinNormal(aEnd  ,iGen+1,true);
			}

			return true;
		}

		private Vector3D Interpolate(Vector3D V0, Vector3D V1, double a)
		{// Interpolation of a unit vector between two unit vectors.
		 // The solution need not to be precise except for a = 0 and a = 1.
		 // The method applied provides good distribution of values, close to 
		 // angle partition
			Vector3D S = (V0+V1).Unit();
			if(a <= 0.5)
			{
				a = a*2;
				return (S*a + V0*(1-a)).Unit();
			}
			else
			{
				a = (a-0.5)*2;
				return (V1*a + S*(1-a)).Unit();
			}
			
		}
		private Vector3D JoinPoint(double a, int xGen)
		{
			Vector3D DNA = Interpolate(DN,DN1,a);//(DN1*a + DN*(1.0-a)).Unit();
			return S + DNA*gen.X(xGen) + dir.VZ*gen.Y(xGen);
		}

		private Vector3D JoinNormal(double a, int xGen, bool fromLeft)
		{
			Vector3D DNA = Interpolate(DN,DN1,a);//(DN1*a + DN*(1.0-a)).Unit();
			Vector3D DA = Interpolate(D,D1,a); //(D1*a + D*(1.0-a)).Unit();
			Vector3D Tangent;
			if(fromLeft)
				Tangent = DNA*gen.dXdu0(xGen) + dir.VZ*gen.dYdu0(xGen);
			else
				Tangent = DNA*gen.dXdu1(xGen) + dir.VZ*gen.dYdu1(xGen);

			return DA.CrossProduct(Tangent);
		}

		#endregion

        internal virtual void Render(Shader.RenderingEngine e)
		{
			if(NP0.IsNull || NQ0.IsNull || NP1.IsNull || NQ1.IsNull)
				return;
			if((P1-Q1).Abs > 0 && (P1-P0).Abs > 0)
				e.RenderElement(P0,P1,Q1, NP0,NP1,NQ1, ConvertToCore.Color(gen.ChartColor(iGen)),false);
			if((P0-Q0).Abs > 0 && (Q1-Q0).Abs > 0)
				e.RenderElement(P0,Q1,Q0, NP0,NQ1,NQ0, ConvertToCore.Color(gen.ChartColor(iGen)),false);
		}
	}
}
