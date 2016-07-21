using System;

namespace ComponentArt.Web.Visualization.Charting 
{
	public class CoordinateRange : ChartObject
	{
		private object	minX = null, maxX = null;
		private object	minY = null, maxY = null;
		private object	minZ = null, maxZ = null;
		private ChartColor surface;

		/// <summary>
		/// Initializes a new instance of <see cref="CoordinateRange"/> with specified surface.
		/// </summary>
		/// <param name="surface">The <see cref="ChartColor"/> object to be used in the rendering of this <see cref="CoordinateRange"/>.</param>
		public CoordinateRange(ChartColor chartColor)
		{
			this.surface = chartColor;
		}

		private CoordinateSystem CoordSystem { get { return Owner as CoordinateSystem; } }

		/// <summary>
		/// Gets or sets the starting value of this <see cref="CoordinateRange"/> along the X axis.
		/// </summary>
		public virtual Object MinX
		{
			get
			{
				if(minX == null)
					return CoordSystem.XAxis.MinValue;
				else
					return minX;
			}
			set
			{
				minX = value;
			}
		}

		/// <summary>
		/// Gets or sets the ending value of the this <see cref="CoordinateRange"/> along the X axis.
		/// </summary>
		public virtual Object MaxX
		{
			get
			{
				if(maxX == null)
					return CoordSystem.XAxis.MaxValue;
				else
					return maxX;
			}
			set
			{
				maxX = value;
			}
		}

		/// <summary>
		/// Gets or sets the starting value of this <see cref="CoordinateRange"/> along the Y axis.
		/// </summary>
		public virtual Object MinY
		{
			get
			{
				if(minY == null)
					return CoordSystem.YAxis.MinValue;
				else
					return minY;
			}
			set
			{
				minY = value;
			}
		}

		/// <summary>
		/// Gets or sets the ending value of the this <see cref="CoordinateRange"/> along the Y axis.
		/// </summary>
		public virtual Object MaxY
		{
			get
			{
				if(maxY == null)
					return CoordSystem.YAxis.MaxValue;
				else
					return maxY;
			}
			set
			{
				maxY = value;
			}
		}

		/// <summary>
		/// Gets or sets the starting value of this <see cref="CoordinateRange"/> along the Z axis.
		/// </summary>
		public virtual Object MinZ
		{
			get
			{
				if(minZ == null)
					return CoordSystem.ZAxis.MinValue;
				else
					return minZ;
			}
			set
			{
				minZ = value;
			}
		}

		/// <summary>
		/// Gets or sets the ending value of the this <see cref="CoordinateRange"/> along the Z axis.
		/// </summary>
		public virtual Object MaxZ
		{
			get
			{
				if(maxZ == null)
					return CoordSystem.ZAxis.MaxValue;
				else
					return maxZ;
			}
			set
			{
				maxZ = value;
			}
		}

		internal override void Render()
		{
			if(!Visible)
				return;

			CoordinateSystem cs = Owner as CoordinateSystem;
			Vector3D D0 = cs.WCoordinate(MinX,MinY,MinZ);
			Vector3D D1 = cs.LCS2WCS(new Vector3D(
				cs.XAxis.LCoordinate(MaxX)+cs.XAxis.LWidth(MaxX),
				cs.YAxis.LCoordinate(MaxY)+cs.YAxis.LWidth(MaxY),
				cs.ZAxis.LCoordinate(MaxZ)+cs.ZAxis.LWidth(MaxZ)));
			GE.CreateBox(D0,D1,surface);
		}
	}

	/// <summary>
	/// Represents a constant plane object drawn in the chart.
	/// </summary>
	public class ConstantPlane : CoordinateRange
	{
		private Axis	axis = null;
		private object  coord = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConstantPlane"/> class with default settings.
		/// </summary>
		/// <param name="axis">The to <see cref="Axis"/> this <see cref="ConstantPlane"/> object is on.</param>
		/// <param name="coordinate">The coordinate of this <see cref="ConstantPlane"/> object.</param>
		/// <param name="surface">The <see cref="ChartColor"/> to be used for this <see cref="ConstantPlane"/> object.</param>
		public ConstantPlane(Axis axis, object coordinate, ChartColor chartColor): base(chartColor)
		{
			this.axis = axis;
			this.coord = coordinate;
		}

		private CoordinateSystem CoordSystem { get { return Owner as CoordinateSystem; } }

		/// <summary>
		/// Gets or sets either the coordinate if the <see cref="Axis"/> of this <see cref="ConstantPlane"/> object is X, or the starting value of this <see cref="ConstantPlane"/> along the X axis. 
		/// </summary>
		public override Object MinX
		{
			get
			{
				if(axis == CoordSystem.XAxis)
					return coord;
				else 
					return base.MinX;
			}
			set
			{
				if(axis == CoordSystem.XAxis)
					coord = value;
				else 
					base.MinX = value;
			}
		}

		/// <summary>
		/// Gets or sets either the coordinate if the <see cref="Axis"/> of this <see cref="ConstantPlane"/> object is X, or the ending value of this <see cref="ConstantPlane"/> along the X axis. 
		/// </summary>
		public override Object MaxX
		{
			get
			{
				if(axis == CoordSystem.XAxis)
					return coord;
				else 
					return base.MaxX;
			}
			set
			{
				if(axis == CoordSystem.XAxis)
					coord = value;
				else 
					base.MaxX = value;
			}
		}

		/// <summary>
		/// Gets or sets either the coordinate if the <see cref="Axis"/> of this <see cref="ConstantPlane"/> object is Y, or the starting value of this <see cref="ConstantPlane"/> along the Y axis. 
		/// </summary>
		public override Object MinY
		{
			get
			{
				if(axis == CoordSystem.YAxis)
					return coord;
				else 
					return base.MinY;
			}
			set
			{
				if(axis == CoordSystem.YAxis)
					coord = value;
				else 
					base.MinY = value;
			}
		}

		/// <summary>
		/// Gets or sets either the coordinate if the <see cref="Axis"/> of this <see cref="ConstantPlane"/> object is Y, or the ending value of this <see cref="ConstantPlane"/> along the Y axis. 
		/// </summary>
		public override Object MaxY
		{
			get
			{
				if(axis == CoordSystem.YAxis)
					return coord;
				else 
					return base.MaxY;
			}
			set
			{
				if(axis == CoordSystem.YAxis)
					coord = value;
				else 
					base.MaxY = value;
			}
		}

		/// <summary>
		/// Gets or sets either the coordinate if the <see cref="Axis"/> of this <see cref="ConstantPlane"/> object is Z, or the starting value of this <see cref="ConstantPlane"/> along the Z axis. 
		/// </summary>
		public override Object MinZ
		{
			get
			{
				if(axis == CoordSystem.ZAxis)
					return coord;
				else 
					return base.MinZ;
			}
			set
			{
				if(axis == CoordSystem.ZAxis)
					coord = value;
				else 
					base.MinZ = value;
			}
		}

		/// <summary>
		/// Gets or sets either the coordinate if the <see cref="Axis"/> of this <see cref="ConstantPlane"/> object is Z, or the ending value of this <see cref="ConstantPlane"/> along the Z axis. 
		/// </summary>
		public override Object MaxZ
		{
			get
			{
				if(axis == CoordSystem.ZAxis)
					return coord;
				else 
					return base.MaxZ;
			}
			set
			{
				if(axis == CoordSystem.ZAxis)
					coord = value;
				else 
					base.MaxZ = value;
			}
		}
	}
}
