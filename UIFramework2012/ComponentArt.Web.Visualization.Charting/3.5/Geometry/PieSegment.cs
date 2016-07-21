using System;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for PieSegment.
	/// </summary>
	internal class PieSegment : GeometricObject
	{
		protected double	alpha0;
		protected double	alpha1;
		protected double	innerRadius;
		protected double	outerEdgeSmoothingRadius;
		protected double	innerEdgeSmoothingRadius;
		protected Vector3D	C1, C2, R1;
		protected ChartColor surface;

        internal PieSegment() { }

		internal PieSegment(double alpha0, double alpha1,
			Vector3D C1, Vector3D C2, Vector3D R1, double innerRadius, 
			double outerEdgeSmoothingRadius, double innerEdgeSmoothingRadius, ChartColor surface)
		{
			this.alpha0 = alpha0;
			this.alpha1 = alpha1;
			this.C1 = C1;
			this.C2 = C2;
			this.R1 = R1;
			this.innerRadius = innerRadius;
			this.outerEdgeSmoothingRadius = outerEdgeSmoothingRadius;
			this.innerEdgeSmoothingRadius = innerEdgeSmoothingRadius;
			this.surface = surface;
        }

        #region --- Properties ---

        internal double Alpha0 { get { return alpha0; } }
        internal double Alpha1 { get { return alpha1; } }
        internal Vector3D BaseCenter { get { return C1; } }
        internal Vector3D TopCenter { get { return C2; } }
        internal Vector3D BaseRadius { get { return R1; } }
        internal double InnerRadius { get { return innerRadius; } }
        internal double InnerEdgeRadius { get { return innerEdgeSmoothingRadius; } }
        internal double OuterEdgeRadius { get { return outerEdgeSmoothingRadius; } }
        internal ChartColor ChartColor { get { return surface; } }

		// Computed properties

		internal Vector3D OrthogonalRadius 
		{
			get
			{
				return ((new Vector3D(0,1,0)).CrossProduct(BaseRadius)).Unit()*(BaseRadius.Abs);
				//return ((TopCenter - BaseCenter).CrossProduct(BaseRadius)).Unit()*(BaseRadius.Abs);
			}
		}

		internal override double OrderingZ()
		{ 
			// Returns z-coordinate of the base at center angle
			double alphaCenter = (Alpha0 + Alpha1)/2;
			double c = Math.Cos(alphaCenter);
			double s = Math.Sin(alphaCenter);
			double baseRadiusAbs = BaseRadius.Abs;
			double orthRadiusAbs = OrthogonalRadius.Abs;

			Vector3D CS = BaseCenter + 
				BaseRadius.Unit()*(c*(baseRadiusAbs + InnerRadius)/2) +
				OrthogonalRadius.Unit()*(s*(orthRadiusAbs + InnerRadius)/2);
			return Mapping.Map(CS).Z;

		}

		internal int NumberOfApproximationPoints(double renderingPrecisionInPixels)
		{
			double a1 = (Mapping.Map(BaseCenter+BaseRadius) - Mapping.Map(BaseCenter)).Abs;
			double a2 = (Mapping.Map(BaseCenter+OrthogonalRadius) - Mapping.Map(BaseCenter)).Abs;
			int n = base.NumberOfApproximationPointsForEllipse(a1,12,renderingPrecisionInPixels);
			n = (int)Math.Floor((Alpha1-Alpha0)*n/(2.0*Math.PI));
			return Math.Max(2,n);
		}

		internal Vector3D[] GetInnerLine(double renderingPrecisionInPixels, bool top)
		{
			int n = NumberOfApproximationPoints(renderingPrecisionInPixels);
			double a = Alpha0, da = (Alpha1-Alpha0)/(n-1);
			Vector3D[] points = new Vector3D[n];
			Vector3D Vx = BaseRadius.Unit()*InnerRadius;
			Vector3D Vy = OrthogonalRadius.Unit()*InnerRadius;
			Vector3D C = top? TopCenter:BaseCenter;
			for(int i=0;i<n;i++)
			{
				double c = Math.Cos(a);
				double s = Math.Sin(a);
				points[i] = C + c*Vx + s*Vy;
				a += da;
			}

			return points;
		}

		internal Vector3D AtLocal(double x, double y, double z)
		{
			Vector3D C = z*TopCenter + (1.0-z)*BaseCenter;
			Vector3D Vx0 = BaseRadius.Unit()*InnerRadius;
			Vector3D Vy0 = OrthogonalRadius.Unit()*InnerRadius;
			Vector3D Vx1 = BaseRadius;
			Vector3D Vy1 = OrthogonalRadius;
			Vector3D Vx = x*Vx1 + (1-x)*Vx0;
			Vector3D Vy = x*Vy1 + (1-x)*Vy0;

			double a = y*Alpha1 + (1-y)*Alpha0;
			return C + Math.Cos(a)*Vx + Math.Sin(a)*Vy;

		}

		internal Vector3D[] GetOuterLine(double renderingPrecisionInPixels, bool top)
		{
			int n = NumberOfApproximationPoints(renderingPrecisionInPixels);
			double a = Alpha0, da = (Alpha1-Alpha0)/(n-1);
			Vector3D[] points = new Vector3D[n];
			Vector3D Vx = BaseRadius;
			Vector3D Vy = OrthogonalRadius;
			Vector3D C = top? TopCenter:BaseCenter;
			for(int i=0;i<n;i++)
			{
				double c = Math.Cos(a);
				double s = Math.Sin(a);
				points[i] = C + c*Vx + s*Vy;
				a += da;
			}

			return points;
		}

		internal override TargetCoordinateRange CoordinateRange(bool usingTexts)
		{
			TargetCoordinateRange tcr = new TargetCoordinateRange();
			Vector3D[] points = GetOuterLine(1.0,true);
			for(int i=0; i<points.Length; i++)
				tcr.Include(this.Mapping.Map(points[i]));
			points = GetOuterLine(1.0,false);
			for(int i=0; i<points.Length; i++)
				tcr.Include(this.Mapping.Map(points[i]));
			return tcr;
		}

        #endregion
	}
}
