using System;
using System.Drawing;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
	/// <summary>
	/// Summary description for Prism.
	/// </summary>
	internal class Prism : GeometricObject //ChartObject
	{
		protected Vector3D	C1;
		protected Vector3D	C2;
		protected Vector3D	R1;
		protected Vector3D	R2;
		protected Vector3D  H;
		protected Vector3D  He;
		protected double	rEdge;
		protected ChartColor	surface;
		protected int		nSides;
		protected double	startingAngle;
		private   double    f;
		private   double	h;
		private	  double	r2;
		private double angleStep;

        
        // For cloning
        internal Prism()
        {
        }

		/// <summary>
		/// Multisided prism. The base is inscribed in an ellipse. 
		/// </summary>
		/// <param name="C1">Base center.</param>
		/// <param name="C2">Top center.</param>
		/// <param name="R1">Radius vector connecting base center and one base vertex.</param>
		/// <param name="r2">Orthogonal second radius of the base ellipse.</param>
		/// <param name="nSides">Number of sides.</param>
		/// <param name="rEdge">Edge radius.</param>
		/// <param name="surface">Surface color.</param>
		internal Prism(Vector3D C1, Vector3D C2, Vector3D R1, double r2, int nSides, double rEdge, ChartColor surface)
		{
			this.nSides = nSides;
			this.C1 = C1;
			this.C2 = C2;
			this.R1 = R1;
			this.r2 = r2;
			this.rEdge = rEdge;
			this.surface = surface;

			H = C2-C1;
			h = H.Abs;
			He = H/h;
			startingAngle = -Math.PI*(0.5-1.0/nSides);
			angleStep = 2*Math.PI/nSides;
		}

		#region --- Properties ---
		internal double Side 
		{
			get
			{
				// Based on the main axis
				return 2*R1.Abs*Math.Sin(angleStep/2);
			}
			set
			{
				// Main axis fixed and ratio between main and conjugated axis remains the same
				double R1Abs = value/(2*Math.Sin(angleStep/2));
				double factor = R1Abs/R1.Abs;
				R1 = R1.Unit()*R1Abs;
				r2 *= factor;
			}
		}

		internal Vector3D BaseCenter { get { return C1; } }
		internal Vector3D TopCenter  { get { return C2; } }
		internal Vector3D BaseRadius { get { return R1; } }
		internal double   OrthogonalRadius { get { return r2; } }
		internal double   EdgeRadius { get { return rEdge; } }
		internal int      NumberOfSides { get { return nSides; } }
		internal ChartColor ChartColor { get { return surface; } }

		internal Vector3D BasePoint(int i)
		{
			Vector3D R2 = (H.CrossProduct(BaseRadius)).Unit()*r2;
			return BaseCenter + R1 * Math.Cos(startingAngle+angleStep*i) + R2 * Math.Sin(startingAngle+angleStep*i);
		}

		#endregion
	}

}
