using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
#if __BuildingWebChart__
using System.Web.UI;
#endif

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{

	internal class CoordinateSystemOrientationConverter : EnumConverter 
	{
		public CoordinateSystemOrientationConverter(Type type) : base (type)
		{
		}
 
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			ChartBase chart = ((CoordinateSystem)context.Instance).OwningChart;
			if (chart.Series.Style.IsRadar || chart.Series.Style.ChartKind == ChartKind.Pie
				|| chart.Series.Style.ChartKind == ChartKind.Doughnut) 
			{
				return new StandardValuesCollection(new object [] {CoordinateSystemOrientation.Default});
			}

			if (chart.Mapping.Kind 
				== ProjectionKind.TwoDimensional )
				return new StandardValuesCollection(new object [] {CoordinateSystemOrientation.Default, CoordinateSystemOrientation.Horizontal});

			StandardValuesCollection svc = base.GetStandardValues(context);
			return svc;
		}
	}

	/// <summary>
	/// Describes the orientation of the coordinate system.
	/// </summary>
	public enum CoordinateSystemOrientation
	{
		/// <summary>
		/// Default orientation. Data Coordinate System (DCS) is aligned with the World Coordinate System (WCS).
		/// </summary>
		Default,	
		/// <summary>
		/// Horizontal orientation. Y and X axes of the DCS are aligned with X and Y axes of the WCS, respectively.
		/// </summary>
		Horizontal,
		/// <summary>
		/// Y, Z and X axes of the DCS are aligned with X, Y and Z axes of the WCS, respectively.
		/// </summary>
		YZX,
		/// <summary>
		/// Z, X and Y axes of the DCS are aligned with X, Y and Z axes of the WCS, respectively.
		/// </summary>
		ZXY,
		/// <summary>
		/// X, Z and Y axes of the DCS are aligned with X, Y and Z axes of the WCS, respectively.
		/// </summary>
		XZY,
		/// <summary>
		/// Z, Y and X axes of the DCS are aligned with X, Y and Z axes of the WCS, respectively.
		/// </summary>
		ZYX
	};

	/// <summary>
	///		Object using coordinate planes and coordinate axes to describe position and size of other 
	///		objects in the chartâ€™s 3D space.
	/// </summary>
	/// <remarks>
	///   <para>
	///     Coordinate systems define size, position and orientation of objects within cart's 3D space.
	///     These objects are series, data points, as well as other coordinate systems in the case
	///     of <see cref="CompositionKind.MultiSystem"/> composition kind. Please read more about 
	///     this subject in the topics "Coordinates and Coordinate Systems" and "Multiple Coordinate Systems"
	///     in the section "Advanced Concepts".
	///   </para>
	///   <para>
	///     The coordinate system contains three <see cref="CoordinatePlane"/>s accessible through properties:
	///     <see cref="PlaneXY"/>, <see cref="PlaneYZ"/> and <see cref="PlaneZX"/>. Each of those planes 
	///     can be made visible or invisible. In 2D charts, <see cref="PlaneYZ"/> and <see cref="PlaneZX"/>
	///     are invisible.
	///   </para>
	///   <para>
	///     <see cref="CoordinateSystem"/> also has three axes: <see cref="XAxis"/>, <see cref="YAxis"/>
	///     and <see cref="ZAxis"/>. <see cref="XAxis"/> is the axis of data points x-coordinate,
	///     <see cref="YAxis"/> is the data point "value" axis and <see cref="ZAxis"/> is used to accommodate
	///     multiple series within chart's 3D space.
	///     By default <see cref="XAxis"/> is horizontal, <see cref="YAxis"/> is vertical and <see cref="ZAxis"/>
	///     is the depth axis. This can be changed by the coordinate system's <see cref="Orientation"/>.
	///   </para>
	///   <para>
	///     Properties <see cref="DXICS"/>, <see cref="DYICS"/> and <see cref="DZICS"/> can be used to
	///     override default proportions of the dimensions of the coordinate system along <see cref="XAxis"/>,
	///     <see cref="YAxis"/> and <see cref="ZAxis"/>. In case of multiple coordinate systems, properties
	///     <see cref="OffsetXICS"/>, <see cref="OffsetYICS"/> and <see cref="OffsetZICS"/> are used
	///     to define the position of origin of this coordinate system within the parent system.
	///   </para>
	/// </remarks>

	[System.ComponentModel.TypeConverter(typeof(CoordinateSystemConverter))]
	[Serializable()]

	public sealed class CoordinateSystem : ChartObject
	{
		private Axis			xAxis, yAxis, zAxis;
		private CoordinatePlane	planeXY,planeYZ,planeZX;
		private bool			isDefault = false;
		private bool			hasChanged = true; // force serialization by default
		private ArrayList		coordinateRanges;

		//		Offsets IN THE PARENT ICS
		private	double		offsetXICS = 0, offsetYICS = 0, offsetZICS = 0;
		
		private CoordinateSystemOrientation orientation = CoordinateSystemOrientation.Default;

		private	bool	renderPlanes = true;

		private bool	isEmbeded = false;

		/// <summary>
		/// Initializes a new instance of <see cref="CoordinateSystem"/> class. 
		/// </summary>
		public CoordinateSystem()
		{
			XAxis = new Axis();
			YAxis = new Axis();
			ZAxis = new Axis();
			XAxis.HasChanged = false;
			YAxis.HasChanged = false;
			ZAxis.HasChanged = false;
			coordinateRanges = new ArrayList();
			// This is default setup, so...
			hasChanged = false;
		}

		#region --- Properties ---

        internal Vector3D XAxisWCS { get { return (ICS2WCS(new Vector3D(1, 0, 0)) - ICS2WCS(new Vector3D(0, 0, 0))).Unit(); } }
        internal Vector3D YAxisWCS { get { return (ICS2WCS(new Vector3D(0, 1, 0)) - ICS2WCS(new Vector3D(0, 0, 0))).Unit(); } }
        internal Vector3D ZAxisWCS { get { return (ICS2WCS(new Vector3D(0, 0, 1)) - ICS2WCS(new Vector3D(0, 0, 0))).Unit(); } }

		internal TargetArea TargetArea { get { return OwningSeries.TargetArea; } }

		internal CoordinateSystem NonEmbededOwner
		{
			get
			{
				if(IsEmbeded)
					return OwningSeries.OwningSeries.CoordSystem.NonEmbededOwner;
				else
					return this;
			}
		}

			/// <summary>
			/// Gets or sets the orientation for this <see cref="CoordinateSystem"/> object.
			/// </summary>
			[Description("The orientation of the coordinate system.")]
			[DefaultValue(CoordinateSystemOrientation.Default)]
			[TypeConverter(typeof(CoordinateSystemOrientationConverter))]
			public CoordinateSystemOrientation Orientation 
		{
			get { return orientation; }
			set
			{
				if(orientation == value)
					return;
				hasChanged = true;
				if(OwningChart != null)
					OwningChart.Invalidate();
				orientation = value;
				SetAxesOrientation();
			}
		}

		/// <summary>
		/// Gets a value indicating whether this coordinate system is embeded.
		/// </summary>
		[Description("Indicates whether this coordinate system is embeded.")]
		[DefaultValue(false)]
		[Browsable(false)]
		public bool IsEmbeded 
		{
			get 
			{ 
				if(OwningSeries == null)
					return false;
				CompositeSeries pSeries = OwningSeries.OwningSeries;
				// Top of series hierarchy is not embeded
				if(pSeries == null)
					return false;
				// MultiArea and MultiSystem nodes are not embeded
				CompositionKind pComp = pSeries.CompositionKind;
				if(pComp == CompositionKind.MultiArea || pComp == CompositionKind.MultiSystem)
					return false;
				return isEmbeded; 
			}
		}

		internal void SetEmbeded(bool embeded)
		{
			isEmbeded = embeded;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the x-axis is reversed.
		/// </summary>
		[Description("Indicates whether the x-axis is reversed.")]
		[DefaultValue(false)]
		public bool ReverseXAxis 
		{
			get { return (xAxis!=null)?xAxis.Reverse:false; }
			set { if(xAxis != null) xAxis.Reverse = value; hasChanged = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the y-axis is reversed.
		/// </summary>
		[Description("Indicates whether the y-axis is reversed.")]
		[DefaultValue(false)]
		public bool ReverseYAxis 
		{
			get { return (yAxis!=null)?yAxis.Reverse:false; }
			set { if(yAxis != null) yAxis.Reverse = value; hasChanged = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the z-axis is reversed.
		/// </summary>
		[Description("Indicates whether the z-axis is reversed.")]
		[DefaultValue(false)]
		public bool ReverseZAxis 
		{
			get { return (zAxis!=null)?zAxis.Reverse:false; }
			set { if(zAxis != null) zAxis.Reverse = value; hasChanged = value; }
		}

		/// <summary>
		/// Gets a value that indicates whether this <see cref="CoordinateSystem"/> object is positive (right-hand coordinate system).
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsPositive 
		{
			get 
			{ 
				return XAxis.UnitVector.CrossProduct(YAxis.UnitVector)*ZAxis.UnitVector > 0;
			}
		}

		internal bool IsReverse
		{
			get
			{

				bool positive = XAxis.Reverse;
				if(YAxis.Reverse)
					positive = !positive;
				if(ZAxis.Reverse)
					positive = !positive;
				return positive;
			}
		}

        internal bool IsPositiveInWCS
        {
            get
            {
                bool p = IsPositive;
                if (XAxis.Reverse)
                    p = !p;
                if (YAxis.Reverse)
                    p = !p;
                return p;
            }
        }

			/// <summary>
			/// Gets or sets the X-<see cref="Axis"/> of this <see cref="CoordinateSystem"/> object.
			/// </summary>
#if __BuildingWebChart__
		[PersistenceMode(PersistenceMode.InnerProperty)]
#endif
			[SRDescription("CoordinateSystemXAxisDescr")]
			[NotifyParentProperty(true)]
			[DefaultValue(null)]
			public Axis XAxis 
		{
			get 
			{
				if(OwningChart != null && OwningChart.InSerialization && xAxis!= null && !xAxis.HasChanged)
					return null;
				else
					return xAxis; 
			} 
			set 
			{ 
				hasChanged = true; 
				xAxis = value; 
				xAxis.Orientation = AxisOrientation.XAxis; // TODO: This is valid when the coordinate system is in default orientation
				xAxis.SetOwner(this); 
			}
		}

		/// <summary>
		/// Gets or sets the Y-<see cref="Axis"/> of this <see cref="CoordinateSystem"/> object.
		/// </summary>
#if __BuildingWebChart__
		[PersistenceMode(PersistenceMode.InnerProperty)]
#endif
        [NotifyParentProperty(true)]
		[SRDescription("CoordinateSystemYAxisDescr")]
		[DefaultValue(null)]
		public Axis YAxis 
		{ 
			get 
			{
				if(OwningChart != null && OwningChart.InSerialization && yAxis!= null && !yAxis.HasChanged)
					return null;
				else
					return yAxis; 
			} 
			set 
			{
				hasChanged = true; 
				yAxis = value; 
				yAxis.Orientation = AxisOrientation.YAxis;
				yAxis.SetOwner(this); 
			}
		}

		/// <summary>
		/// Gets or sets the Z-<see cref="Axis"/> of this <see cref="CoordinateSystem"/> object.
		/// </summary>
#if __BuildingWebChart__
		[PersistenceMode(PersistenceMode.InnerProperty)]
#endif
		[SRDescription("CoordinateSystemZAxisDescr")]
		[NotifyParentProperty(true)]
		[DefaultValue(null)]
		public Axis ZAxis 
		{ 
			get 
			{
				if(OwningChart != null && OwningChart.InSerialization && zAxis!= null && !zAxis.HasChanged)
					return null;
				else
					return zAxis; 
			} 
			set 
			{
				hasChanged = true; 
				zAxis = value; 
				zAxis.Orientation = AxisOrientation.ZAxis;
				zAxis.SetOwner(this); 
			}
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ChartObject"/> is displayed.
		/// </summary>
		[DefaultValue(true)]
		public override bool Visible
		{
			get 
			{ 
				if(!base.Visible)
					return false;
				CompositeSeries os = OwningSeries as CompositeSeries;
				if(os != null && os.MultiCS)
					return false;
				return true; 
			}
			set 
			{ 
				base.Visible = value; 
			}
		}
		// Offsets. Note that these are offsets in the parent coordinate system and may not
		// be colinear with this system orientation
		/// <summary>
		/// Gets or sets the X-offset of this <see cref="CoordinateSystem"/> in the parent system.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(0.0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double OffsetXICS	
		{ 
			get { return offsetXICS; } 
			set 
			{ 
				offsetXICS = value; 
				if(OwningCoordSystem != null) OwningCoordSystem.AdjustSize(); 
			}
		}
		/// <summary>
		/// Gets or sets the Y-offset of this <see cref="CoordinateSystem"/> in the parent system.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(0.0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double OffsetYICS	{ get { return offsetYICS; } set { offsetYICS = value; if(OwningCoordSystem != null) OwningCoordSystem.AdjustSize(); } }
		/// <summary>
		/// Gets or sets the Z-offset of this <see cref="CoordinateSystem"/> in the parent system.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(0.0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double OffsetZICS	{ get { return offsetZICS; } set { offsetZICS = value; if(OwningCoordSystem != null) OwningCoordSystem.AdjustSize(); } }

		/// <summary>
		/// Gets or sets the length of X dimension of this <see cref="CoordinateSystem"/> in the intermediate coordinate system. 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double DXICS	
		{ 
			get { return XAxis.MaxValueICS; } 
			set 
			{ XAxis.SetMaxValueICS(value); if(OwningCoordSystem != null) OwningCoordSystem.AdjustSize(); } 
		}
		/// <summary>
		/// Gets or sets the length of Y dimension of this <see cref="CoordinateSystem"/> in the intermediate coordinate system. 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double DYICS	{ 
			get { return YAxis.MaxValueICS; } 
			set 
			{ YAxis.MaxValueICS = value; if(OwningCoordSystem != null) OwningCoordSystem.AdjustSize(); } 
		}
		/// <summary>
		/// Gets or sets the length of Z dimension of this <see cref="CoordinateSystem"/> in the intermediate coordinate system. 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double DZICS	{ get { return ZAxis.MaxValueICS; } 
			set 
			{ ZAxis.SetMaxValueICS(value); /*if(OwningCoordSystem != null) OwningCoordSystem.AdjustSize();*/ } 
		}

		internal bool RenderPlanes { get { return renderPlanes; } set { renderPlanes = value; } }

		internal bool IsDefault { get { return isDefault; } set { isDefault = value; } }
		internal SeriesBase OwningSeries { get { return Owner as SeriesBase; } }
		internal CoordinateSystem OwningCoordSystem
		{
			get
			{
				SeriesBase ser = OwningSeries;
				if(ser == null)
					return null;

				CompositeSeries owningSer = ser.OwningSeries;
				if(owningSer == null)
					return null;
				
				// Topmost coordinate system in one target area is not owned by any other
				// coordinate system
				if(owningSer.CompositionKind == CompositionKind.MultiArea)
					return null;

				// Climb the series ownership chain 
				while(owningSer != null)
				{
					if(owningSer.OwnCoordSystem != null)
						return owningSer.OwnCoordSystem;
					owningSer = owningSer.OwningSeries;
				}
				return null;
			}
		}

		#endregion

		#region --- Coordinates Mapping ---

		/// <summary>
		/// Returns coordinates in Logical Coordinate System (LCS) of datapoint given by its data coordinates.
		/// </summary>
		/// <param name="xDCS">x-xoordinate of the datapoint.</param>
		/// <param name="yDCS">y-xoordinate of the datapoint.</param>
		/// <param name="zDCS">z-xoordinate of the datapoint.</param>
		/// <returns>Coordinates in Logical Coordinate System (LCS) of datapoint given by its data coordinates.</returns>
		public Vector3D LCoordinate(object xDCS, object yDCS, object zDCS)
		{
			return new Vector3D(
				XAxis.LCoordinate(xDCS),
				YAxis.LCoordinate(yDCS),
				ZAxis.LCoordinate(zDCS));
		}

		/// <summary>
		/// Returns a vector representing widths in Logical Coordinate System (LCS) of a datapoint given by its data coordinates.
		/// </summary>
		/// <param name="xDCS">x-xoordinate of the datapoint.</param>
		/// <param name="yDCS">y-xoordinate of the datapoint.</param>
		/// <param name="zDCS">z-xoordinate of the datapoint.</param>
		/// <returns>Vector representing widths in Logical Coordinate System (LCS) of a datapoint given by its data coordinates.</returns>
		public Vector3D LWidth(object xDCS, object yDCS, object zDCS)
		{
			return new Vector3D(
				XAxis.LWidth(xDCS),
				YAxis.LWidth(yDCS),
				ZAxis.LWidth(zDCS));
		}

		/// <summary>
		/// Returns coordinates in Intermediate Coordinate System (ICS) of datapoint given by its data coordinates.
		/// </summary>
		/// <param name="xDCS">x-xoordinate of the datapoint.</param>
		/// <param name="yDCS">y-xoordinate of the datapoint.</param>
		/// <param name="zDCS">z-xoordinate of the datapoint.</param>
		/// <returns>Coordinates in Intermediate Coordinate System (ICS) of datapoint given by its data coordinates.</returns>
		public Vector3D ICoordinate(object xDCS, object yDCS, object zDCS)
		{
			return new Vector3D(
				XAxis.ICoordinate(xDCS),
				YAxis.ICoordinate(yDCS),
				ZAxis.ICoordinate(zDCS));
		}

		/// <summary>
		/// Returns a vector representing widths in Intermediate Coordinate System (ICS) of a datapoint given by its data coordinates.
		/// </summary>
		/// <param name="xDCS">x-xoordinate of the datapoint.</param>
		/// <param name="yDCS">y-xoordinate of the datapoint.</param>
		/// <param name="zDCS">z-xoordinate of the datapoint.</param>
		/// <returns>Vector representing widths in Intermediate Coordinate System (ICS) of a datapoint given by its data coordinates.</returns>
		public Vector3D IWidth(object xDCS, object yDCS, object zDCS)
		{
			return (new Vector3D(
				XAxis.ICoordinate(xDCS),
				YAxis.ICoordinate(yDCS),
				ZAxis.ICoordinate(zDCS))) -
				ICoordinate(xDCS,yDCS,zDCS);
		}

		/// <summary>
		/// Returns the minimum coordinates in Intermediate Coordinate System (ICS) of datapoint given by its data coordinates.
		/// </summary>
		/// <param name="xDCS">x-xoordinate of the datapoint.</param>
		/// <param name="yDCS">y-xoordinate of the datapoint.</param>
		/// <param name="zDCS">z-xoordinate of the datapoint.</param>
		/// <returns>Minimum coordinates in Intermediate Coordinate System (ICS) of datapoint given by its data coordinates.</returns>
		public Vector3D ICoordinateStd(object xDCS, object yDCS, object zDCS)
		{
			Vector3D ic = ICoordinate(xDCS,yDCS,zDCS);
			Vector3D iw = IWidth     (xDCS,yDCS,zDCS);
			double icx = ic.X;
			double icy = ic.Y;
			double icz = ic.Z;
			double iwx = iw.X;
			double iwy = iw.Y;
			double iwz = iw.Z;
			if(iwx < 0) icx += iwx;
			if(iwy < 0) icy += iwy;
			if(iwz < 0) icz += iwz;
			return new Vector3D(icx,icy,icz);
		}

		/// <summary>
		/// Returns a vector representing positive widths in Intermediate Coordinate System (ICS) of a datapoint given by its data coordinates.
		/// </summary>
		/// <param name="xDCS">x-xoordinate of the datapoint.</param>
		/// <param name="yDCS">y-xoordinate of the datapoint.</param>
		/// <param name="zDCS">z-xoordinate of the datapoint.</param>
		/// <returns>Vector representing positive widths in Intermediate Coordinate System (ICS) of a datapoint given by its data coordinates.</returns>
		public Vector3D IWidthStd(object xDCS, object yDCS, object zDCS)
		{
			Vector3D iw = IWidth(xDCS,yDCS,zDCS);
			return new Vector3D(Math.Abs(iw.X),Math.Abs(iw.Y),Math.Abs(iw.Y));
		}

		/// <summary>
		/// Returns coordinates in World Coordinate System (WCS) of datapoint given by its data coordinates.
		/// </summary>
		/// <param name="xDCS">x-xoordinate of the datapoint.</param>
		/// <param name="yDCS">y-xoordinate of the datapoint.</param>
		/// <param name="zDCS">z-xoordinate of the datapoint.</param>
		/// <returns>Coordinates in World Coordinate System (WCS) of datapoint given by its data coordinates.</returns>
		public Vector3D WCoordinate(object xDCS, object yDCS, object zDCS)
		{
			return ICS2WCS(ICoordinate(xDCS,yDCS,zDCS));
		}

		/// <summary>
		/// Returns coordinates in Intermediate Coordinate System (ICS) of a point in the Logical Coordinate System (LCS).
		/// </summary>
		/// <param name="vLCS">A point in the Logical Coordinate System (LCS).</param>
		/// <returns>coordinates in ICS of a point in the LCS.</returns>
		public Vector3D LCS2ICS(Vector3D vLCS)
		{
			return new Vector3D(XAxis.LCS2ICS(vLCS.X),YAxis.LCS2ICS(vLCS.Y),ZAxis.LCS2ICS(vLCS.Z));
		}

		/// <summary>
		/// Returns coordinates in World Coordinate System (WCS) of a point in the Intermediate Coordinate System (ICS).
		/// </summary>
		/// <param name="ics">Point in the ICS.</param>
		/// <returns>Coordinates in World Coordinate System (WCS).</returns>
		public Vector3D ICS2WCS(Vector3D ics)
		{
			Vector3D vecParentICS = XAxis.UnitVector*ics.X + YAxis.UnitVector*ics.Y + ZAxis.UnitVector*ics.Z;
		
			CoordinateSystem ocs = OwningCoordSystem;
			if(ocs != null)
			{
				return ocs.ICS2WCS(vecParentICS + new Vector3D(offsetXICS,OffsetYICS,OffsetZICS));
			}
			else
				return vecParentICS;
		}

		/// <summary>
		/// Returns coordinates in World Coordinate System (WCS) of a point in the Logical Coordinate System (LCS).
		/// </summary>
		/// <param name="vLCS">Point in the LCS.</param>
		/// <returns>Coordinates in World Coordinate System (WCS).</returns>
		public Vector3D LCS2WCS(Vector3D vLCS)
		{
			return ICS2WCS(LCS2ICS(vLCS));
		}

		/// <summary>
		/// Gets the origin in the World Coordinate System (WCS).
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Vector3D OriginWCS { get { return ICS2WCS(Vector3D.Null); } }

		private void AdjustSize()
		{
			// Adjusts size of coordinate system based on sizes of children coordinate systems.
			// Propagates adjustments to parent coordinate systems.
			AdjustSize(OwningSeries);
			if(OwningCoordSystem != null)
				OwningCoordSystem.AdjustSize();
		}

		private void AdjustSize(SeriesBase series)
		{
			CompositeSeries cs = series as CompositeSeries;
			if(cs==null)
				return;
			double dx = 0, dy = 0, dz = 0;
			foreach (SeriesBase ser in cs.SubSeries)
			{
				CoordinateSystem sys = ser.OwnCoordSystem;
				if(sys != null)
				{
					// Size vector in this coordinate system
					Vector3D sizeVec = 
						sys.XAxis.UnitVector*sys.XAxis.MaxValueICS +
						sys.YAxis.UnitVector*sys.YAxis.MaxValueICS +
						sys.ZAxis.UnitVector*sys.ZAxis.MaxValueICS +
						new Vector3D(sys.OffsetXICS,sys.OffsetYICS,sys.OffsetZICS);
					dx = Math.Max(dx,sizeVec.X);
					dy = Math.Max(dy,sizeVec.Y);
					dz = Math.Max(dz,sizeVec.Z);
				}
			}
			XAxis.SetMaxValueICS(dx);
			YAxis.SetMaxValueICS(dy);
			ZAxis.SetMaxValueICS(dz);

		}


		/// <summary>
		/// Gets or sets the ratio of y-axis size to x-axis size.
		/// </summary>
		/// <remarks>
		/// This property should only be set after DataBind(). Use to override default y-axis to x-axis proportion.
		/// </remarks>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double YbyXRatio 
		{ 
			get { return (YAxis.MaxValueICS-YAxis.MinValueICS)/(XAxis.MaxValueICS-XAxis.MinValueICS); }
			set { if(value>0) YAxis.MaxValueICS = YAxis.MinValueICS + value*(XAxis.MaxValueICS-XAxis.MinValueICS); }
		}


		/// <summary>
		/// Gets or sets the ratio of z-axis size to x-axis size.
		/// </summary>
		/// <remarks>
		/// This property should only be set after DataBind(). Use to override default z-axis to x-axis proportion.
		/// </remarks>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double ZbyXRatio 
		{ 
			get { return (ZAxis.MaxValueICS-ZAxis.MinValueICS)/(XAxis.MaxValueICS-XAxis.MinValueICS); }
			set { if(value>0) ZAxis.MaxValueICS = ZAxis.MinValueICS + value*(XAxis.MaxValueICS-XAxis.MinValueICS); }
		}

		#endregion

		#region --- Coordinate Planes and Axes Annotation ---

		internal CoordinatePlane CreateCoordinatePlaneXY()
		{
			if(planeXY != null)
				return planeXY;

			planeXY = new CoordinatePlane();
			planeXY.SetOwner(this);
			planeXY.SetCoordinateSystem(this);
			planeXY.Depth = Space.CoordinatePlanesDepth;
			planeXY.HasChanged = false;

			return planeXY;
		}

		internal CoordinatePlane CreateCoordinatePlaneYZ()
		{
			if(planeYZ != null)
				return planeYZ;

			planeYZ = new CoordinatePlane();
			planeYZ.SetOwner(this);
			planeYZ.SetCoordinateSystem(this);
			planeYZ.Depth = Space.CoordinatePlanesDepth;
			planeYZ.HasChanged = false;

			return planeYZ;
		}

		internal CoordinatePlane CreateCoordinatePlaneZX()
		{
			if(planeZX != null)
				return planeZX;

			planeZX = new CoordinatePlane();
			planeZX.SetOwner(this);
			planeZX.SetCoordinateSystem(this);
			planeZX.Depth = Space.CoordinatePlanesDepth;
			planeZX.HasChanged = false;

			return planeZX;
		}

		internal void CreateCoordinatePlanes()
		{
			CreateCoordinatePlaneXY();
			CreateCoordinatePlaneYZ();
			CreateCoordinatePlaneZX();
			planeXY.CreateDefaultContents();
			planeYZ.CreateDefaultContents();
			planeZX.CreateDefaultContents();
		}

		/// <summary>
		/// Creates a <see cref="ConstantPlane"/> and adds it to this <see cref="CoordinateSystem"/> object.
		/// </summary>
		/// <param name="axis">Axis on which the plane will be located.</param>
		/// <param name="coordinate">coordinate of the plane in Data Coordinate System (DCS).</param>
		/// <param name="color">color of the plane.</param>
		/// <returns>Newly created <see cref="ConstantPlane"/> object.</returns>
		public ConstantPlane CreateConstantPlane(Axis axis, object coordinate, Color color)
		{
			ConstantPlane cp = new ConstantPlane(axis,coordinate,new ChartColor(color));
			cp.SetOwner(this);
			coordinateRanges.Add(cp);
			return cp;
		}

		/// <summary>
		/// Creates a <see cref="CoordinateRange"/> and adds it to this <see cref="CoordinateSystem"/> object.
		/// </summary>
		/// <param name="color">color of the range.</param>
		/// <returns>Newly created <see cref="CoordinateRange"/> object.</returns>
		public CoordinateRange CreateCoordinateRange(Color color)
		{
			CoordinateRange cp = new CoordinateRange(new ChartColor(color));
			cp.SetOwner(this);
			coordinateRanges.Add(cp);
			return cp;
		}

		/// <summary>
		/// Gets or sets the coordinate plane that corresponds to XY plane.
		/// </summary>
		[Description("XY plane of the chart.")]
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
#if __BuildingWebChart__
		[PersistenceMode(PersistenceMode.InnerProperty)]
#endif
        [DefaultValue(null)]
		[Bindable(true)]
		public CoordinatePlane PlaneXY 
		{
			get 
			{
				if(planeXY==null)
					CreateCoordinatePlaneXY(); 
				if(OwningChart.InSerialization && !planeXY.HasChanged)
					return null;
				else
					return planeXY;
			} 
			set 
			{ 
				planeXY = value; 
				planeXY.SetOwner(this);
				planeXY.SetCoordinateSystem(this);
			}
		}
		/// <summary>
		/// Gets or sets the coordinate plane that corresponds to YZ plane.
		/// </summary>
		[Description("YZ plane of the chart.")]
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
#if __BuildingWebChart__
		[PersistenceMode(PersistenceMode.InnerProperty)]
#endif
        [Bindable(true)]
		[DefaultValue(null)]
		public CoordinatePlane PlaneYZ 
		{ 
			get 
			{
				if(planeYZ==null)
					CreateCoordinatePlaneYZ(); 
				if(OwningChart.InSerialization && !planeYZ.HasChanged)
					return null;
				else
					return planeYZ;
			} 
			set
			{ 
				planeYZ = value; 
				planeYZ.SetOwner(this);
				planeYZ.SetCoordinateSystem(this);
			} 
		}
		/// <summary>
		/// Gets or sets the coordinate plane that corresponds to ZX plane.
		/// </summary>
		[Description("ZX plane of the chart.")]
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
#if __BuildingWebChart__
		[PersistenceMode(PersistenceMode.InnerProperty)]
#endif
        [Bindable(true)]
		[DefaultValue(null)]
		public CoordinatePlane PlaneZX 
		{ 
			get 
			{
				if(planeZX==null)
					CreateCoordinatePlaneZX(); 
				if(OwningChart.InSerialization && !planeZX.HasChanged)
					return null;
				else
					return planeZX;
			} 
			set 
			{ 
				planeZX = value; 
				planeZX.SetOwner(this);
				planeZX.SetCoordinateSystem(this);
			}
		}
		#endregion

		#region --- Building & rendering ---

		// Finds plane by roles of its axes
		internal CoordinatePlane Plane(AxisOrientation or1, AxisOrientation or2)
		{
			if(PlaneXY.XAxis.Role == or1 && PlaneXY.YAxis.Role == or2 ||
			   PlaneXY.XAxis.Role == or2 && PlaneXY.YAxis.Role == or1)
				return PlaneXY;
			if(PlaneYZ.XAxis.Role == or1 && PlaneYZ.YAxis.Role == or2 ||
			   PlaneYZ.XAxis.Role == or2 && PlaneYZ.YAxis.Role == or1)
				return PlaneYZ;
			if(PlaneZX.XAxis.Role == or1 && PlaneZX.YAxis.Role == or2 ||
			   PlaneZX.XAxis.Role == or2 && PlaneZX.YAxis.Role == or1)
				return PlaneZX;
			throw new Exception("Cannot find plane with axes orientations '" + or1.ToString() + "' and '" + or2.ToString() + "'");
		}

		/// <summary>
		/// Computes axis orientation indicating axis role in this coordinate system.
		/// </summary>
		/// <param name="axis"> Axis whose role is queried.</param>
		/// <returns>Axis orientation indicating axis role in this coordinate system.
		/// Note that tis may not be equal to the axis orientation in the coordinate system,
		/// since it may be afected by the system rotation.
		/// </returns>

		internal AxisOrientation Role (Axis axis)
		{
			if(axis == XAxis)
				return AxisOrientation.XAxis;
			if(axis == YAxis)
				return AxisOrientation.YAxis;
			if(axis == ZAxis)
				return AxisOrientation.ZAxis;
			throw new Exception("Cannot determine the axis role since the axis does not belong to the coordinate system.");
		}

		private void DataBindPieDoughnut()
		{
			XAxis.DataBind(DataDimension.StandardNumericDimension);
			YAxis.DataBind(OwningSeries.YDimension);
			ZAxis.DataBind(OwningSeries.ZDimension);
			//ZAxis.DataBind(DataDimension.StandardNumericDimension);

			if(OwningSeries.MaxXDCS() == null ||
				OwningSeries.MaxXDCS() == null) // checking those two is enough
				return;

			YAxis.MinValue = OwningSeries.MinYDCS();
			YAxis.MaxValue = OwningSeries.MaxYDCS();
		
			XAxis.MinValue = OwningSeries.MinXDCS();
			XAxis.MaxValue = OwningSeries.MaxXDCS();
			YAxis.MinValue = OwningSeries.MinYDCS();
			YAxis.MaxValue = OwningSeries.MaxYDCS();
			ZAxis.MinValue = OwningSeries.MinZDCS();
			ZAxis.MaxValue = OwningSeries.MaxZDCS();

			XAxis.SetMaxValueICS((double)XAxis.MaxValue);
			YAxis.SetMaxValueICS((double)YAxis.MaxValue);
			ZAxis.SetMaxValueICS((double)XAxis.MaxValue);
			//ZAxis.SetMaxValueICS((double)ZAxis.MaxValue);
		}

		internal void DataBind()
		{
			CompositeSeries owningSeries = OwningSeries as CompositeSeries;

			if(owningSeries != null && owningSeries.CompositionKind == CompositionKind.MultiArea)
				return;

			else		// Not "MultiArea"
			{
				if(OwningSeries.Style.ChartKindCategory == ChartKindCategory.PieDoughnut)
				{
					DataBindPieDoughnut();
					return;
				}

				if(IsEmbeded)
				{
					XAxis = NonEmbededOwner.XAxis;
					ZAxis = NonEmbededOwner.ZAxis;

					YAxis.DataBind(OwningSeries.YDimension);

					object yAxisMinValue = OwningSeries.MinYDCS();
					object yAxisMaxValue = OwningSeries.MaxYDCS();
					
					// Securing non-null y-size
					if(yAxisMinValue is double)
					{
						double y0 = (double)yAxisMinValue;
						double y1 = (double)yAxisMaxValue;
						if(y0 == y1)
						{
							if(y0 == 0)
								yAxisMaxValue = 10;
							else
								yAxisMaxValue = y0 + Math.Abs(y0)*0.01;
							YAxis.MaxValue = yAxisMaxValue;
						}
					}
					else if(yAxisMinValue is DateTime)
					{
						DateTime y0 = (DateTime)yAxisMinValue;
						DateTime y1 = (DateTime)yAxisMaxValue;
						if(y0 == y1)
						{
							yAxisMaxValue = y0 + new TimeSpan(24,0,0);
							YAxis.MaxValue = yAxisMaxValue;
						}
					}


					if(yAxisMaxValue == null)
						return;

					YAxis.MinValue = yAxisMinValue;
					YAxis.MaxValue = yAxisMaxValue;
					object refValue = OwningSeries.ReferenceValue;
					if(refValue != null)
					{
						if(OwningSeries.YDimension.Coordinate(refValue) < OwningSeries.YDimension.Coordinate(YAxis.MinValue))
							YAxis.MinValue = refValue;
						if(OwningSeries.YDimension.Coordinate(refValue) + OwningSeries.YDimension.Width(refValue) >
							OwningSeries.YDimension.Coordinate(YAxis.MaxValue) + OwningSeries.YDimension.Width(YAxis.MaxValue))
							YAxis.MaxValue = refValue;
					}
					YAxis.AdjustRangeInternal();
            
					OwningSeries.ComputeSize();
					if(YAxis.AutoICSRange)
						YAxis.SetMaxValueICS(OwningSeries.DYICS);

                    XAxis.CreateDefaultAnnotation(OwningSeries.Name,true);
                    YAxis.CreateDefaultAnnotation(OwningSeries.Name,true);
                    ZAxis.CreateDefaultAnnotation(OwningSeries.Name,true);

					// Set axes orientation
					SetAxesOrientation();
				}
				else
				{
					XAxis.DataBind(OwningSeries.XDimension);
					YAxis.DataBind(OwningSeries.YDimension);

					object xAxisMinValue = OwningSeries.MinXDCS();
					object xAxisMaxValue = OwningSeries.MaxXDCS();
					object yAxisMinValue = OwningSeries.MinYDCS();
					object yAxisMaxValue = OwningSeries.MaxYDCS();
					
					object zAxisMinValue = null;
					object zAxisMaxValue = null;
					// Securing non-null y-size
					if(yAxisMinValue is double)
					{
						double y0 = (double)yAxisMinValue;						
						double y1 = y0;
						if(yAxisMaxValue != null)
							y1 = (double)yAxisMaxValue;
						if(y0 == y1)
						{
							if(y0 != 0)
								yAxisMaxValue = y0 + 0.01*Math.Abs(y0);
							else
								yAxisMaxValue = 1;
							//yAxisMaxValue = y0 + 100;
							YAxis.MaxValue = yAxisMaxValue;
						}
					}
					else if(yAxisMinValue is DateTime)
					{
						DateTime y0 = (DateTime)yAxisMinValue;
						DateTime y1 = y0;
						if(yAxisMaxValue != null)
							y1 = (DateTime)yAxisMaxValue;
						if(y0 == y1)
						{
							yAxisMaxValue = y0 + new TimeSpan(24,0,0);
							YAxis.MaxValue = yAxisMaxValue;
						}
					}

					if(ZAxis.Dimension is EnumeratedDataDimension)
					{
						EnumeratedDataDimension zDim = ZAxis.Dimension as EnumeratedDataDimension;
						Coordinate coord = zDim[OwningSeries.Name];
						Coordinate coord1 = coord;
						while(coord1 != null)
						{
							zAxisMinValue = coord1.Value;
							coord1 = coord1.FirstChild;
						}
						coord1 = coord;
						while(coord1 != null)
						{
							zAxisMaxValue = coord1.Value;
							coord1 = coord1.LastChild;
						}
					}
					else
					{
						zAxisMinValue = OwningSeries.MinZDCS();
						zAxisMaxValue = OwningSeries.MaxZDCS();
					}

					if(xAxisMaxValue == null ||
						yAxisMaxValue == null) // checking those two is enough
						return;

					bool yMinValueSet = true; // Mark if values are explicit, or derived from data
					bool yMaxValueSet = true;
					if(XAxis.MinValue == null)
					{
						yMinValueSet = false;
						XAxis.MinValue = xAxisMinValue;
					}
					if(XAxis.MaxValue == null)
					{
						yMaxValueSet = false;
						XAxis.MaxValue = xAxisMaxValue;
					}
					if(YAxis.MinValue == null)
						YAxis.MinValue = yAxisMinValue;
					if(YAxis.MaxValue == null)
						YAxis.MaxValue = yAxisMaxValue;
					ZAxis.MinValue = zAxisMinValue;
					ZAxis.MaxValue = zAxisMaxValue;

					// Adjusting ranges

					XAxis.AdjustRangeInternal();

					if(!OwningChart.InDesignMode)
					{
						// We adjust reference value to y-range if OwningSeries.AdjustReferenceValue = true
						// or y-range to reference value if OwningSeries.AdjustReferenceValue = false (default)
						//
						// We have to walk the subtree of the owning series. If we adjust y-range to
						// the reference values, we need to determine the range of all reference values of
						// series subordinate to the owning series. If we adjust ref values to the range of y 
						// values, we have to do it for the subtree

						bool adjustRefValue = OwningSeries.AdjustReferenceValue;
						if(adjustRefValue)
						{
							// We adjust reference value the y-range.
							YAxis.AdjustRangeInternal();
							OwningSeries.AdjustReferenceValuesToYRange(YAxis.MinValue,YAxis.MaxValue);
						}
						else
						{
							DataDimension yDim = YAxis.Dimension;
							// We adjust y range to reference values.
							// We do this only if yDim != null, i.e. we skip the case of C.S.
							//	which is parent of multiple coordinate systems
							if(yDim != null)
							{
								object refValueMin = null, refValueMax = null;
								OwningSeries.GetReferenceValuesRange(ref refValueMin, ref refValueMax,true);
								if(refValueMin != null) // then refValueMax != null as well
								{
									if(yAxisMinValue == null ||  yDim.Coordinate(refValueMin) < yDim.Coordinate(YAxis.MinValue))
										YAxis.MinValue = refValueMin;
									if(yAxisMaxValue == null ||  
										yDim.Coordinate(refValueMax) + yDim.Width(refValueMax) >
										yDim.Coordinate(YAxis.MaxValue) + yDim.Width(YAxis.MaxValue))
										YAxis.MaxValue = refValueMax;
								}
								YAxis.AdjustRangeInternal();
								// If no explicit ref value set, so we adjust implied values to the y-range
								// We use only those y-range values that are explicitly set
								if(refValueMin == null)	
									OwningSeries.AdjustReferenceValuesToYRange
										(yMinValueSet?YAxis.MinValue:null,yMaxValueSet?YAxis.MaxValue:null);
							}
						}
					}
					else
					{
						object refValue;
						bool adjustRefValue = OwningSeries.AdjustReferenceValue;
						if(adjustRefValue)
							refValue = OwningSeries.RawReferenceValue;
						else
							refValue = OwningSeries.ReferenceValue;
						if(refValue != null && !adjustRefValue)
						{
							if(OwningSeries.YDimension.Coordinate(refValue) < OwningSeries.YDimension.Coordinate(YAxis.MinValue))
								YAxis.MinValue = refValue;
							if(OwningSeries.YDimension.Coordinate(refValue) + OwningSeries.YDimension.Width(refValue) >
								OwningSeries.YDimension.Coordinate(YAxis.MaxValue) + OwningSeries.YDimension.Width(YAxis.MaxValue))
								YAxis.MaxValue = refValue;
						}
						YAxis.AdjustRangeInternal();
					}

					ZAxis.AdjustRangeInternal();
            
					OwningSeries.ComputeSize();
					if(XAxis.AutoICSRange)
						XAxis.SetMaxValueICS(OwningSeries.DXICS);
					if(YAxis.AutoICSRange)
						YAxis.SetMaxValueICS(OwningSeries.DYICS);
					if(ZAxis.AutoICSRange)
						ZAxis.SetMaxValueICS(OwningSeries.DZICS);

                    XAxis.CreateDefaultAnnotation(OwningSeries.Name,false);
                    YAxis.CreateDefaultAnnotation(OwningSeries.Name,false);
                    ZAxis.CreateDefaultAnnotation(OwningSeries.Name,false);

					// Set axes orientation
					SetAxesOrientation();
				}
				CreateCoordinatePlanes();
			}
		}

		private void SetAxesOrientation()
		{
			if(IsEmbeded)
			{
				YAxis.Orientation = NonEmbededOwner.YAxis.Orientation;
			}
			else
			{
				switch(Orientation)
				{
					case CoordinateSystemOrientation.Default:
						XAxis.Orientation = AxisOrientation.XAxis;
						YAxis.Orientation = AxisOrientation.YAxis;
						ZAxis.Orientation = AxisOrientation.ZAxis;
						break;
					case CoordinateSystemOrientation.Horizontal:
						XAxis.Orientation = AxisOrientation.YAxis;
						YAxis.Orientation = AxisOrientation.XAxis;
						ZAxis.Orientation = AxisOrientation.ZAxis;
						break;
					case CoordinateSystemOrientation.XZY:
						XAxis.Orientation = AxisOrientation.XAxis;
						YAxis.Orientation = AxisOrientation.ZAxis;
						ZAxis.Orientation = AxisOrientation.YAxis;
						break;
					case CoordinateSystemOrientation.YZX:
						XAxis.Orientation = AxisOrientation.YAxis;
						YAxis.Orientation = AxisOrientation.ZAxis;
						ZAxis.Orientation = AxisOrientation.XAxis;
						break;
					case CoordinateSystemOrientation.ZXY:
						XAxis.Orientation = AxisOrientation.ZAxis;
						YAxis.Orientation = AxisOrientation.XAxis;
						ZAxis.Orientation = AxisOrientation.YAxis;
						break;
					case CoordinateSystemOrientation.ZYX:
						XAxis.Orientation = AxisOrientation.ZAxis;
						YAxis.Orientation = AxisOrientation.YAxis;
						ZAxis.Orientation = AxisOrientation.XAxis;
						break;
				}
			}
		}

		internal override void Build()
		{
			SeriesBase series = OwningSeries;

			base.Build();
			XAxis.Build();
			YAxis.Build();
			ZAxis.Build();
			PlaneXY.Build();
			PlaneYZ.Build();
			PlaneZX.Build();
		}

		internal override void Render()
		{
			if(!Visible)
				return;
			CompositeSeries CS = OwningSeries as CompositeSeries;
			if(CS != null && CS.MultiCS)
				return;

			if(IsEmbeded)
				DYICS = NonEmbededOwner.DYICS;
			
			if(renderPlanes)
			{
				PlaneXY.Render();
				PlaneYZ.Render();
				PlaneZX.Render();
        	}
            if (IsEmbeded)
            {
				xAxis.Render();
				yAxis.Render();
				zAxis.Render();
			}
            else
            {
                xAxis.Render();
                yAxis.Render();
                zAxis.Render();
            }

			if(coordinateRanges != null)
			{
				foreach(CoordinateRange cr in coordinateRanges)
					cr.Render();
			}
		}

		#endregion

		#region --- Serialization ---
		
		private bool ShouldSerializePlaneXY() { return PlaneXY!=null && PlaneXY.HasChanged; }
		private bool ShouldSerializePlaneYZ() { return PlaneYZ!=null && PlaneYZ.HasChanged; }
		private bool ShouldSerializePlaneZX() { return PlaneZX!=null && PlaneZX.HasChanged; }

		internal bool HasChanged
		{
			get 
			{
				return hasChanged || 
					(planeXY != null && planeXY.HasChanged) ||
					(planeXY != null && planeYZ.HasChanged) ||
					(planeXY != null && planeZX.HasChanged);
			}
			set
			{
				hasChanged = value;
				if(!hasChanged)
				{
					// force the same mode on planes
					if(planeXY != null)
						PlaneXY.HasChanged = false;
					if(planeYZ != null)
						PlaneYZ.HasChanged = false;
					if(planeZX != null)
						PlaneZX.HasChanged = false;
				}
			}
		}

		internal bool ShouldSerializeMe { get { return HasChanged; } }

		private static string[] PropertiesOrder = new string[]
			{
				"Orientation",
				"ReverseXAxis",
				"ReverseYAxis",
				"ReverseZAxis",
				"PlaneXY",
				"PlaneYZ",
				"PlaneZX",
				"XAxisName",
				"YAxisName",
				"ZAxisName",
				"Visible"
			};

		#endregion
	}
}
