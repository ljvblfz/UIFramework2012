using System;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Design;
using System.Xml;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	// ------------------------------------------------------------------------------------------------
	//		Chart Kinds Enumeration 
	// ------------------------------------------------------------------------------------------------

	/// <summary>
	/// Specifies the chart or series kind.
	/// </summary>
	public enum ChartKind // Should be in sinc with 'FirstLinearKind' and 'LastLinearKind' members of SeriesStyle
	{
		/// <summary>
		/// Block chart or series.
		/// </summary>
		Block				, // by the FirstLinearKind and LastLinearKind
		/// <summary>
		/// Cylinder.
		/// </summary>
		Cylinder			,
		/// <summary>
		/// Cone.
		/// </summary>
		Cone				,
		/// <summary>
		/// Paraboloid
		/// </summary>
		Paraboloid			,
		/// <summary>
		/// Pyramid.
		/// </summary>
		Pyramid				,
		/// <summary>
		/// 3-sided prism
		/// </summary>
		Prism3				,
		/// <summary>
		/// Hexagon
		/// </summary>
		Hexagon				,
		/// <summary>
		/// 3-sided pyramid
		/// </summary>
		Pyramid3			,
		/// <summary>
		/// 6-sided pyramid.
		/// </summary>
		Pyramid6			,
		/// <summary>
		/// Rectangle.
		/// </summary>
		Rectangle           ,
		/// <summary>
		/// Bubble.
		/// </summary>
		Bubble				,
		/// <summary>
		/// Two-dimensioned bubble.
		/// </summary>
		Bubble2D			,
		/// <summary>
		/// Line.
		/// </summary>
		Line				,
		/// <summary>
		/// Two-dimensioned line.
		/// </summary>
		Line2D				,
		/// <summary>
		/// Area.
		/// </summary>
		Area                ,
		/// <summary>
		/// Two-dimensioned area.
		/// </summary>
		Area2D              ,
		/// <summary>
		/// Candle stick
		/// </summary>
		CandleStick			,
		HighLowOpenClose	,

		/// <summary>
		/// Pie chart.
		/// </summary>
		Pie					,
		/// <summary>
		/// Doughnut chart.
		/// </summary>
		Doughnut			,
		/// <summary>
		/// Marker.
		/// </summary>
		Marker
	}

	internal enum ChartKindCategory { Bar, Area, Line, PieDoughnut, Marker, /*Scatter, */Financial, Radar};

	/// <summary>
	/// Series style kind. Used to access predefined series styles. 
	/// All custom created styles have this kind set to 'Custom'.
	/// </summary>
	public enum SeriesStyleKind
	{
		/// <summary>
		/// Default series style
		/// </summary>
		Default,
		/// <summary>
		/// Default style for missing data points
		/// </summary>
		DefaultMissingPointsStyle,

		/// <summary>
		/// Cylinder series style
		/// </summary>
		Cylinder,
		/// <summary>
		/// Block series style
		/// </summary>
		Block,
		/// <summary>
		/// Hexagon series style
		/// </summary>
		Hexagon,
		/// <summary>
		/// Paraboloid series style
		/// </summary>
		Paraboloid,
		/// <summary>
		/// Cone series style
		/// </summary>
		Cone,
		/// <summary>
		/// Three-sided prism series style
		/// </summary>
		Prism3,
		/// <summary>
		/// Pyramid (four-sided) series style
		/// </summary>
		Pyramid,
		/// <summary>
		/// Three-sided pyramid series style
		/// </summary>
		Pyramid3,
		/// <summary>
		/// Six-sided pyramid series style
		/// </summary>
		Pyramid6,
		/// <summary>
		/// Two-dimensional rectangle series style
		/// </summary>
		Rectangle,

		/// <summary>
		/// 3D area series style
		/// </summary>
		Area,
		/// <summary>
		/// 3D smooth area series style
		/// </summary>
		AreaSmooth,
		/// <summary>
		/// 3D step area series style
		/// </summary>
		AreaStep,
		/// <summary>
		/// 2D area series style
		/// </summary>
		Area2D,
		/// <summary>
		/// 2D smooth area series style
		/// </summary>
		Area2DSmooth,
		/// <summary>
		/// 2D step area series style
		/// </summary>
		Area2DStep,

		/// <summary>
		/// 3D line series style
		/// </summary>
		Line,
		/// <summary>
		/// Smooth 3D line series style
		/// </summary>
		LineSmooth,
		/// <summary>
		/// Step 3D line series style
		/// </summary>
		LineStep,
		/// <summary>
		/// 3D line series style
		/// </summary>
		Line2D,
		/// <summary>
		/// 3D smooth line series style
		/// </summary>
		Line2DSmooth,
		/// <summary>
		/// 3D step line series style
		/// </summary>
		Line2DStep,
			
		/// <summary>
		/// Doughnut series style
		/// </summary>
		Doughnut,
		/// <summary>
		/// Thin doughnut series style
		/// </summary>
		DoughnutThin,
		/// <summary>
		/// Round, thick doughnut series style
		/// </summary>
		DoughnutRound,
		/// <summary>
		/// Round, thick doughnut series style with inverted curvature
		/// </summary>
		DoughnutInverted,
		/// <summary>
		/// Cylinder series style
		/// </summary>
		Pie,
		/// <summary>
		/// Pie series style
		/// </summary>
		PieThin,
		/// <summary>
		/// Round, thick pie series style
		/// </summary>
		PieRound,

		/// <summary>
		/// Bubble series style
		/// </summary>
		Bubble,
		/// <summary>
		/// 2D bubble series style
		/// </summary>
		Bubble2D,
		/// <summary>
		/// Block  marker series style
		/// </summary>
		BlockMarker,
		/// <summary>
		/// Circle marker series style
		/// </summary>
		CircleMarker,
		/// <summary>
		/// Diamond shaped marker series style
		/// </summary>
		Diamond,
		/// <summary>
		/// Triangle shaped marker series style
		/// </summary>
		Triangle,
		/// <summary>
		/// Inverted triangle shaped marker series style
		/// </summary>
		InvertedTriangle,
		/// <summary>
		/// Left triangle shaped marker series style
		/// </summary>
		LeftTriangle,
		/// <summary>
		/// Right triangle shaped marker series style
		/// </summary>
		RightTriangle,
		/// <summary>
		/// Cross shaped marker series style
		/// </summary>
		Cross,
		/// <summary>
		/// X shaped marker series style
		/// </summary>
		XShape,
		/// <summary>
		/// Arrow pointing to east (right) shaped marker series style
		/// </summary>
		ArrowE,
		/// <summary>
		/// Arrow pointing to west (left) shaped marker series style
		/// </summary>
		ArrowW,
		/// <summary>
		/// Arrow pointing to north (up) shaped marker series style
		/// </summary>
		ArrowN,
		/// <summary>
		/// Arrow pointing to south (down) shaped marker series style
		/// </summary>
		ArrowS,
		/// <summary>
		/// Arrow pointing to north-east shaped marker series style
		/// </summary>
		ArrowNE,
		/// <summary>
		/// Arrow pointing to north-west shaped marker series style
		/// </summary>
		ArrowNW,
		/// <summary>
		/// Arrow pointing to south-east shaped marker series style
		/// </summary>
		ArrowSE,
		/// <summary>
		/// Arrow pointing to south-west shaped marker series style
		/// </summary>
		ArrowSW,

		/// <summary>
		/// Candle-stick series style
		/// </summary>
		CandleStick,
		/// <summary>
		/// High-low-open-close series style
		/// </summary>
		HighLowOpenClose,

		/// <summary>
		/// Radar line series style
		/// </summary>
		RadarLine,
		/// <summary>
		/// Radar area series style
		/// </summary>
		RadarArea,
		/// <summary>
		/// Radar marker series style
		/// </summary>
		RadarMarker,

		/// <summary>
		/// Series style kind for all custom series styles
		/// </summary>
		Custom
	}

	/// <summary>
	/// Specifies the kind of line in a <see cref="SeriesStyle"/> object.
	/// </summary>
	public enum LineKind
	{
		/// <summary>
		/// Values in a series are connected with straight lines.
		/// </summary>
		Simple,
		/// <summary>
		/// The line connects the series values with a spline.
		/// </summary>
		Smooth,
		/// <summary>
		/// The line goes through the values in steps.
		/// </summary>
		Step
	}

	// ================================================================================================
	//		DataPoint Presentation Style and Collection
	// ================================================================================================
	// (ED)

	/// <summary>
	///     Describes how data points in a series are rendered.
	/// </summary>
	/// <remarks>
	///   <para>
	///     <see cref="SeriesStyle"/> objects are stored in the <see cref="SeriesStyleCollection"/> 
	///     property "SeriesStyles" of the charting control. 
	///     The control has many predefined series styles. They can be used as defined by 
	///     default or modified by user code.  Besides, the styles architecture 
	///     is extendible. The user can define his/her own style and store it in the "SeriesStyles"
	///     collection. 
	///   </para>
	///   <para>
	///     The <see cref="SeriesStyle.ChartKind"/> property defines the kind of a geometrical object or 
	///     symbol used to represent data point in the chart. The values are
	///     Block, Cylinder, Cone, Paraboloid, Pyramid,	Prism3, Hexagon, Pyramid3, Pyramid6,
	///     Rectangle, Bubble, Bubble2D, Line, Line2D, Area, Area2D, CandleStick, HighLowOpenClose,
	///     Pie, Doughnut and Marker.
	///   </para>
	///   <para>
	///     Some chart kinds make use of other types of styles. For example, if the chart kind is "Line",
	///     then we have to know what exactly is the style of that line. Note that line styles
	///     are stored in another collection of the control and that there might be many line styles
	///     available by default and/or created by the user. The <see cref="SeriesStyle.LineStyleName"/>
	///     property is the name of the line style that we use in this <see cref="SeriesStyle"/> object and it is
	///     the index to the corresponding collection of line styles. The same consideration applies to
	///     properties <see cref="SeriesStyle.BorderLineStyleName"/>, 
	///     <see cref="SeriesStyle.GradientStyleName"/>, <see cref="SeriesStyle.LineStyle2DName"/> 
	///     and <see cref="SeriesStyle.MarkerStyleName"/>
	///   </para>
	///   <para>
	///     The <see cref="SeriesStyle.LineStyleName"/> property is used only if <see cref="SeriesStyle.ChartKind"/>
	///     is "Line", otherwise this property is ignored. Many other properties of 
	///     <see cref="SeriesStyle"/> are used for some chart kinds and ignored for others.
	///   </para>
	/// </remarks>

	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	public class SeriesStyle : NamedObjectBase
	{
		// --- Members common to all presentation styles ---
		
		private static ChartKind	FirstLinearKind = ChartKind.Block;
		private static ChartKind  LastLinearKind = ChartKind.HighLowOpenClose;
		private static ChartKind	FirstLineKind = ChartKind.Line;
		private static ChartKind  LastLineKind = ChartKind.Line2D;

		// defaults
		private const ChartKind			chartKindDefault=ChartKind.Block;
		private const LineKind			lineKindDefault = LineKind.Simple;

		private ChartKind			chartKind=ChartKind.Line;
		private LineKind			lineKind=lineKindDefault;
		private LineKind			lineKind1=lineKindDefault;
		private LineKind			lineKind2=lineKindDefault;

		// --- Linear shape specifics ---

		private const double	edgeRadiusInPointsDefault = 3;
		private double		edgeRadiusInPoints=edgeRadiusInPointsDefault;
		
		// --- Radar coordinate system ---

		private bool			isRadar = false;

		// Relative spacing
		
		// defaults
		private const double		relativeLeftSpaceDefault = 0.1;
		private const double		relativeRightSpaceDefault = 0.1;
		private const double		relativeFrontSpaceDefault = 0.1;
		private const double		relativeBackSpaceDefault = 0.1;

		// variables
		private double		relativeLeftSpace=relativeLeftSpaceDefault;
		private double		relativeRightSpace=relativeRightSpaceDefault;
		private double		relativeFrontSpace=relativeFrontSpaceDefault;
		private double		relativeBackSpace=relativeBackSpaceDefault;
		// Base shape
		private const bool		forceSquareDefault = true;
		private bool			forceSquare=forceSquareDefault;

		// --- Pie/doughnut specifics ---

		// defaults
		private const double		radiusDefault = 50;
		private const double		relativeHeightDefault = 0.25;
		private const double		firstSegmentStartDefault = 180;
		private const double		relativeEdgeSmoothingRadiusDefault = 0.1;

		private bool				isDefault = false;

		// variables
		private double		radius=radiusDefault;
		private double		relativeHeight=relativeHeightDefault;
		private double		firstSegmentStart=firstSegmentStartDefault;
		private double		relativeEdgeSmoothingRadius=relativeEdgeSmoothingRadiusDefault;


		// doughnut style parameters
		// defaults
		private const double		relativeInnerRadiusDefault = 0.33;
		private const double		relativeInnerEdgeSmoothingRadiusDefault = 0.05;
		// variables
		private double		relativeInnerRadius=relativeInnerRadiusDefault;
		private double		relativeInnerEdgeSmoothingRadius=relativeInnerEdgeSmoothingRadiusDefault;

		// --- Line and area specifics ---

		private string		lineStyleName = "Default";
		private string		lineStyleName2D = "DefaultForLine2DSeriesStyle";
		private string		borderLineStyleName = "TwoDObjectBorder";
		private string		gradientStyleName = "Default";
		private string		markerStyleName = "Bubble";

		// --- Surfaces ---

		private ChartColor	surface;
		private ChartColor	secondarySurface;

		bool m_removable = true;

		#region --- Constructors and Copiers---

		/// <summary>
		/// Initializes a new <see cref="SeriesStyle"/> object with specified chart kind and name.
		/// </summary>
		/// <param name="kind">Specifies the kind of this style.</param>
		/// <param name="styleName">Name of this style.</param>
		public SeriesStyle(ChartKind kind, string styleName)
			: base(styleName) 
		{ 
			this.chartKind = kind;
		}
		/// <summary>
		/// Initializes a new <see cref="SeriesStyle"/> of kind <see cref="ChartKind.Block"/> with specified name.
		/// </summary>
		/// <param name="styleName">Name of this style.</param>
		public SeriesStyle(string styleName)
			:this(ChartKind.Block,styleName) { }

		/// <summary>
		/// Initializes a new <see cref="SeriesStyle"/> of kind <see cref="ChartKind.Block"/>.
		/// </summary>
		public SeriesStyle() :this(null) { }

		internal SeriesStyle CopyTo(SeriesStyleCollection collection, string name, bool keepRemovable)
		{
			SeriesStyle copy = (SeriesStyle)this.MemberwiseClone();
			copy.OwningCollection = null;
			copy.Name = name;
			if(collection != null)
				collection.Add(copy);
			return copy;
		}

		#endregion

		#region --- Properties ---

		/// <summary>
		/// Gets or sets a <see cref="Surface"/> object that determines the primary ChartColor properties of this <see cref="PresentationStyle"/> object.
		/// </summary>
		/// <value>
		/// A <see cref="ChartColor"/> object that keeps the primary ChartColor properties of this <see cref="SeriesStyle"/> object.
		/// </value>
		internal ChartColor Surface			{ get { return surface; }			set { surface = value; } }
		/// <summary>
		/// Gets or sets a <see cref="Surface"/> object that determines the secondary ChartColor properties of this <see cref="PresentationStyle"/> object.
		/// </summary>
		/// <value>
		/// A <see cref="ChartColor"/> object that keeps the secondary ChartColor properties of this <see cref="SeriesStyle"/> object.
		/// </value>
		internal ChartColor SecondarySurface	{ get { return secondarySurface; }	set { secondarySurface = value; } }

		[DefaultValue(true)]
		[Browsable(false)]
		internal bool Removable 
		{
			get 
			{
				return m_removable;
			}
			set 
			{
				m_removable = value;
			}
		}

		internal bool IsDefault	{ get { return isDefault; } set { isDefault = value; } }
		
		/// <summary>
		/// Gets or sets the chart kind of this <see cref="SeriesStyle"/> object.
		/// </summary>
		/// <value>
		/// A <see cref="ChartKind"/> object that keeps the chart kind of this <see cref="SeriesStyle"/> object.
		/// </value>
		[Description("The chart kind of the style")]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		[Category("General Properties")]
		public ChartKind			ChartKind		
		{
			get { return chartKind; }
			set 
			{
				if (chartKind == value)
					return;

				// We don't allow changing the chart kind for styles named after ChartKind enumeration
				// except if we are in object building (like in Xml serialization) when the ownership info is 
				// not complete
				if (!Removable && OwningChart != null)
					throw new ArgumentException("Cannot change property 'ChartKind' in predefined SeriesStyle '" 
						+ this.Name + "'");

				chartKind = value; 
			}
		}

		/// <summary>
		/// Gets or sets the line kind of this <see cref="SeriesStyle"/> object.
		/// </summary>
		[Description("The line kind")]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		[Category("Line/Area Properties")]
		[DefaultValue(lineKindDefault)]
		public LineKind			LineKind		{ get { return lineKind; } set { if(lineKind != value) isDefault = false; lineKind = value; } }
		
		/// <summary>
		/// Gets or sets the lower bound line kind of this <see cref="SeriesStyle"/> object.
		/// </summary>
		/// <value>
		/// A <see cref="LineKind"/> object that keeps the lower bound line kind of this <see cref="SeriesStyle"/> object.
		/// </value>
		[Description("The lower bound line kind")]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		[Category("Line/Area Properties")]
		[DefaultValue(lineKindDefault)]
		public LineKind			LowerBoundLineKind	{ get { return lineKind1; } set { if(lineKind1 != value) isDefault = false; lineKind1 = value; } }
		/// <summary>
		/// Gets or sets the upper bound line kind of this <see cref="SeriesStyle"/> object.
		/// </summary>
		[Description("The upper bound line kind")]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		[Category("Line/Area Properties")]
		[DefaultValue(lineKindDefault)]
		public LineKind			UpperBoundLineKind	{ get { return lineKind2; } set { if(lineKind2 != value) isDefault = false; lineKind2 = value; } }

		/// <summary>
		/// Gets or sets the line style to be used in this <see cref="SeriesStyle"/> object. Make sure the a line this name exists in LineStyles colection of the chart.
		/// </summary>
		[Description("The 3D line style of the series style.")]
		[TypeConverter(typeof(SelectedLineStyleConverter))]
		[Category("Line/Area Properties")]
		public string LineStyleName { get { return lineStyleName; } set { if(lineStyleName != value) isDefault = false; lineStyleName = value; } }

		/// <summary>
		/// Gets or sets the line style to be used in this <see cref="SeriesStyle"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LineStyleKind LineStyleKind 
		{ 
			get { return LineStyle.KindOf(LineStyleName); } 
			set { LineStyleName = LineStyle.NameOf(value); }
		}

		/// <summary>
		/// Gets or sets the 2D line style to be used in this <see cref="SeriesStyle"/> object. Make sure the a line this name exists in LineStyles2D colection of the chart.
		/// </summary>
		[Description("The 2D line style of the series style.")]
		[TypeConverter(typeof(SelectedLineStyleConverter))]
		[Category("Line/Area Properties")]
		public string LineStyle2DName { get { return lineStyleName2D; } set { if(lineStyleName2D != value) isDefault = false; lineStyleName2D = value; } }

		/// <summary>
		/// Gets or sets the 2d line style to be used in this <see cref="SeriesStyle"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LineStyle2DKind LineStyle2DKind 
		{ 
			get { return LineStyle2D.KindOf(LineStyle2DName); } 
			set { LineStyle2DName = LineStyle2D.NameOf(value); }
		}

		/// <summary>
		/// Gets or sets the marker style to be used in this <see cref="SeriesStyle"/> object. Make sure the a line this name exists in MarkerStyles colection of the chart.
		/// </summary>
		[Description("The marker style to be used.")]
		[TypeConverter(typeof(SelectedMarkerStyleConverter))]
		[Category("Marker Properties")]
		public string MarkerStyleName { get { return markerStyleName; } set { if(markerStyleName != value) isDefault = false; markerStyleName = value; } }


		/// <summary>
		/// Gets or sets the marker style kind to be used in this <see cref="SeriesStyle"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MarkerStyleKind MarkerStyleKind 
		{
			get { return MarkerStyle.KindOf(MarkerStyleName); } set { MarkerStyleName = MarkerStyle.NameOf(value); }
		}

		/// <summary>
		/// Gets or sets the gradient style to be used in this <see cref="SeriesStyle"/> object. Make sure the a line this name exists in GradientStyles colection of the chart.
		/// </summary>
		[Description("The gradient style to be used.")]
		[TypeConverter(typeof(SelectedGradientStyleConverter))]
		[Category("Line/Area Properties")]
		public string GradientStyleName { get { return gradientStyleName; } set { if(gradientStyleName != value) isDefault = false; gradientStyleName = value; } }

		/// <summary>
		/// Gets or sets the gradient style kind to be used in this <see cref="SeriesStyle"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public GradientStyleKind GradientStyleKind 
		{
			get { return GradientStyle.KindOf(GradientStyleName); } set { GradientStyleName = GradientStyle.NameOf(value); }
		}


		/// <summary>
		/// Gets or sets the border line style to be used in this <see cref="SeriesStyle"/> object. Make sure the a line this name exists in LineStyles2D colection of the chart.
		/// </summary>
		[Description("The border line style to be used.")]
		[TypeConverter(typeof(SelectedLineStyle2DConverter))]
		[Category("Line/Area Properties")]
		public string BorderLineStyleName { get { return borderLineStyleName; } set { if(borderLineStyleName != value) isDefault = false; borderLineStyleName = value; } }

		/// <summary>
		/// Gets or sets the gradient style kind to be used in this <see cref="SeriesStyle"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LineStyle2DKind BorderLineStyleKind 
		{
			get { return LineStyle2D.KindOf(BorderLineStyleName); } set { BorderLineStyleName = LineStyle2D.NameOf(value); }
		}

		// --- Linear shape properties ---
		/// <summary>
		/// Gets or sets the radius of the edge of graphical object that represents values in this <see cref="SeriesStyle"/>.
		/// </summary>
		[Description("The radius of the edge in points.")]
		[Category("Linear Shape Properties")]
		[DefaultValue(edgeRadiusInPointsDefault)]
		[NotifyParentProperty(true)]
		public double	EdgeRadiusInPoints		{ get { return edgeRadiusInPoints; }	set { if(edgeRadiusInPoints != value) isDefault = false; edgeRadiusInPoints = value; } }
		/// <summary>
		/// Gets or sets the relative left space of graphical object that represents values in this <see cref="SeriesStyle"/>.
		/// </summary>
		[Description("Relative left space of graphical object in percentages.")]
		[Category("Linear Shape Properties")]
		[DefaultValue(relativeLeftSpaceDefault)]
		[NotifyParentProperty(true)]
		public double	RelativeLeftSpace		{ get { return relativeLeftSpace; }		set { if(relativeLeftSpace != value) isDefault = false; relativeLeftSpace = value; } }
		/// <summary>
		/// Gets or sets the relative right space of graphical object that represents values in this <see cref="SeriesStyle"/>.
		/// </summary>
		[Description("Relative right space of graphical object in percentages.")]
		[Category("Linear Shape Properties")]
		[DefaultValue(relativeRightSpaceDefault)]
		[NotifyParentProperty(true)]
		public double	RelativeRightSpace		{ get { return relativeRightSpace; }	set { if(relativeRightSpace != value) isDefault = false; relativeRightSpace = value; } }
		/// <summary>
		/// Gets or sets the relative front space of graphical object that represents values in this <see cref="SeriesStyle"/>.
		/// </summary>
		[Description("Relative front space of graphical object in percentages.")]
		[Category("Linear Shape Properties")]
		[DefaultValue(relativeFrontSpaceDefault)]
		[NotifyParentProperty(true)]
		public double	RelativeFrontSpace		{ get { return relativeFrontSpace; }	set { if(relativeFrontSpace != value) isDefault = false; relativeFrontSpace = value; } }
		/// <summary>
		/// Gets or sets the relative back space of graphical object that represents values in this <see cref="SeriesStyle"/>.
		/// </summary>
		[Description("Relative back space of graphical object in percentages.")]
		[Category("Linear Shape Properties")]
		[DefaultValue(relativeBackSpaceDefault)]
		[NotifyParentProperty(true)]
		public double	RelativeBackSpace		{ get { return relativeBackSpace; }		set { if(relativeBackSpace != value) isDefault = false; relativeBackSpace = value; } }
		/// <summary>
		/// Gets or sets a value that indicates whether the base of graphical object representing values in this <see cref="SeriesStyle"/> is square.
		/// </summary>
		[Description("Indicates whether the base of the bar element is square.")]
		[Category("Linear Shape Properties")]
		[DefaultValue(forceSquareDefault)]
		[NotifyParentProperty(true)]
		public bool		ForceSquareBase			{ get { return forceSquare; }			set { if(forceSquare != value) isDefault = false; forceSquare = value; } }
		/// <summary>
		/// Gets or sets a value that indicates whether this <see cref="SeriesStyle"/> object is a radar.
		/// </summary>
		[Description("Indicates whether the style is radar.")]
		[Browsable(false)]
		[DefaultValue(false)]
		public bool		IsRadar					{ get { return isRadar; }				set { if(isRadar != value) isDefault = false; isRadar = value; } }
		
		// --- Pie/Doughnut properties ---

		//Fixme: doesn't do nothing
		internal double Radius							{ get { return radius; }							/*set { if(radius != value) isDefault = false; radius = value; }*/ }
		/// <summary>
		/// Gets or sets the relative height to the radius of this <see cref="SeriesStyle"/> object. Only applicable when ChartKind property is set to pie or doughnut.
		/// </summary>
		[Description("The height to the radius.")]
		[Category("Pie/Doughnut Properties")]
		[DefaultValue(relativeHeightDefault)]
		[NotifyParentProperty(true)]
		public double RelativeHeight					{ get { return relativeHeight; }					set { if(relativeHeight != value) isDefault = false; relativeHeight = value; } }	
		
		/// <summary>
		///     Gets or sets starting angle in degrees of the first segment of a pie or doughnut.
		/// </summary>
		[SRDescription("SeriesStyleFirstSegmentStartDescr")]
		[Category("Pie/Doughnut Properties")]
		[DefaultValue(firstSegmentStartDefault)]
		[NotifyParentProperty(true)]
		public double FirstSegmentStart					{ get { return firstSegmentStart; }					set { if(firstSegmentStart != value) isDefault = false; firstSegmentStart = value; } }		
		/// <summary>
		/// Gets or sets the relative height to the radius of this <see cref="SeriesStyle"/> object. Only applicable when ChartKind property is set to pie or doughnut.
		/// </summary>
		[SRDescription("SeriesStyleRelativeEdgeSmoothingRadiusDescr")]
		[Category("Pie/Doughnut Properties")]
		[DefaultValue(relativeEdgeSmoothingRadiusDefault)]
		[NotifyParentProperty(true)]
		public double RelativeEdgeSmoothingRadius		{ get { return relativeEdgeSmoothingRadius; }		set { if(relativeEdgeSmoothingRadius != value) isDefault = false; relativeEdgeSmoothingRadius = value; } }
		/// <summary>
		/// Gets or sets the the outer edge smoothing radius in relation to the radius of this <see cref="SeriesStyle"/> object. Only applicable when ChartKind property is set to doughnut.
		/// </summary>
		[SRDescription("SeriesStyleRelativeOuterEdgeSmoothingRadiusDescr")]
		[Category("Pie/Doughnut Properties")]
		[DefaultValue(relativeEdgeSmoothingRadiusDefault)]
		[NotifyParentProperty(true)]
		public double RelativeOuterEdgeSmoothingRadius	{ get { return relativeEdgeSmoothingRadius; }		set { if(relativeEdgeSmoothingRadius != value) isDefault = false; relativeEdgeSmoothingRadius = value; } }
		/// <summary>
		/// Gets or sets the relative inner radius of the doughnut to the radius of this <see cref="SeriesStyle"/> object. Only applicable when ChartKind property is set to doughnut.
		/// </summary>
		[SRDescription("SeriesStyleRelativeInnerRadiusDescr")]
		[Category("Pie/Doughnut Properties")]
		[DefaultValue(relativeInnerRadiusDefault)]
		[NotifyParentProperty(true)]
		public double RelativeInnerRadius				
		{ 
			get { return (chartKind==ChartKind.Doughnut)?relativeInnerRadius:0; }
			set { if(relativeInnerRadius != value) isDefault = false; relativeInnerRadius = value; } 
		}
		/// <summary>
		/// Gets or sets the outer edge smoothing radius in relation to the radius of this <see cref="SeriesStyle"/> object. Only applicable when ChartKind property is set to doughnut.
		/// </summary>
		[Description("The inner edge smoothing radius in relation to the radius.")]
		[Category("Pie/Doughnut Properties")]
		[DefaultValue(relativeInnerEdgeSmoothingRadiusDefault)]
		[NotifyParentProperty(true)]
		public double RelativeInnerEdgeSmoothingRadius	
		{ 
			get { return (chartKind==ChartKind.Doughnut)? relativeInnerEdgeSmoothingRadius:0; }
			set { if(relativeInnerEdgeSmoothingRadius != value) isDefault = false; relativeInnerEdgeSmoothingRadius = value; } 
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="SeriesStyle"/> object is of a linear kind.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal bool IsLinear { get { return FirstLinearKind<=chartKind && chartKind<=LastLinearKind || chartKind == ChartKind.Marker; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="SeriesStyle"/> object is of a bar kind.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal bool IsBar { get { return FirstLinearKind<=chartKind && chartKind<=ChartKind.Rectangle; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="SeriesStyle"/> object is of a line kind.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal bool IsLine   { get { return FirstLineKind<=chartKind && chartKind<=LastLineKind; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="SeriesStyle"/> object is of an area kind.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal bool IsArea   { get { return chartKind == ChartKind.Area || chartKind == ChartKind.Area2D; } }

		internal ChartKindCategory ChartKindCategory 
		{
			get 
			{
				if (IsRadar)
					return ChartKindCategory.Radar;
				if (ChartKind.Block <= ChartKind && ChartKind <= ChartKind.Rectangle)
					return ChartKindCategory.Bar;
				else if (ChartKind == ChartKind.Area || ChartKind == ChartKind.Area2D) 
					return ChartKindCategory.Area;
				else if (ChartKind == ChartKind.Line || ChartKind == ChartKind.Line2D) 
					return ChartKindCategory.Line;
				else if (ChartKind == ChartKind.Bubble || ChartKind == ChartKind.Bubble2D || ChartKind == ChartKind.Marker) 
					return ChartKindCategory.Marker;
				else if (ChartKind == ChartKind.Pie || ChartKind == ChartKind.Doughnut) 
					return ChartKindCategory.PieDoughnut;
				else if (ChartKind == ChartKind.CandleStick || ChartKind == ChartKind.HighLowOpenClose) 
					return ChartKindCategory.Financial;
				throw new NotSupportedException("Can't determine type of " + ChartKind.ToString());
			}
		}
	

		#endregion

		#region --- Serialization ---

		internal bool ShouldSerializeMe { get { return !IsDefault;} }

		internal bool ShouldSerializeEdgeRadiusInPoints() {return edgeRadiusInPoints!=edgeRadiusInPointsDefault;}
		internal bool ShouldSerializeRelativeLeftSpace() {return relativeLeftSpace!=relativeLeftSpaceDefault;}
		internal bool ShouldSerializeRelativeRightSpace() {return relativeRightSpace!=relativeRightSpaceDefault;}
		internal bool ShouldSerializeRelativeFrontSpace() {return relativeFrontSpace!=relativeFrontSpaceDefault;}
		internal bool ShouldSerializeRelativeBackSpace() {return relativeBackSpace!=relativeBackSpaceDefault;}
		internal bool ShouldSerializeForceSquareBase() {return forceSquare!=forceSquareDefault;}
		internal bool ShouldSerializeRadius() {return radius!=radiusDefault;}
		internal bool ShouldSerializeRelativeHeight() {return relativeHeight!=relativeHeightDefault;}
		internal bool ShouldSerializeFirstSegmentStart() {return firstSegmentStart!=firstSegmentStartDefault;}
		internal bool ShouldSerializeRelativeEdgeSmoothingRadius() {return relativeEdgeSmoothingRadius!=relativeEdgeSmoothingRadiusDefault;}
		internal bool ShouldSerializeRelativeInnerRadius() { return chartKind==ChartKind.Doughnut && relativeInnerRadius!=relativeInnerRadiusDefault; }
		internal bool ShouldSerializeRelativeInnerEdgeSmoothingRadius() { return chartKind==ChartKind.Doughnut && relativeInnerEdgeSmoothingRadius!=relativeInnerEdgeSmoothingRadiusDefault; }

		internal bool ShouldSerializeBorderLineStyleName() { return BorderLineStyleName != "TwoDObjectBorder"; }
		internal bool ShouldSerializeChartKind() { return ChartKind != ChartKind.Block; }
		internal bool ShouldSerializeGradientStyleName() { return GradientStyleName != "Default"; }
		internal bool ShouldSerializeIsRadar() { return IsRadar; }
		internal bool ShouldSerializeLineKind() { return LineKind != ComponentArt.Web.Visualization.Charting.LineKind.Simple; }
		internal bool ShouldSerializeLineStyle2DName() { return LineStyle2DName != "Default"; }
		internal bool ShouldSerializeLineStyleName() { return LineStyleName != "Default"; }
		internal bool ShouldSerializeLowerBoundLineKind() { return LowerBoundLineKind != ComponentArt.Web.Visualization.Charting.LineKind.Simple; }
		internal bool ShouldSerializeUpperBoundLineKind() { return UpperBoundLineKind != ComponentArt.Web.Visualization.Charting.LineKind.Simple; }
		internal bool ShouldSerializeMarkerStyleName() { return MarkerStyleName != "Bubble";; }
		internal bool ShouldSerializeRelativeOuterEdgeSmoothingRadius() { return RelativeOuterEdgeSmoothingRadius != 0.15; }

		internal void ResetEdgeRadiusInPoints() {edgeRadiusInPoints=edgeRadiusInPointsDefault;}
		internal void ResetRelativeLeftSpace() {relativeLeftSpace=relativeLeftSpaceDefault;}
		internal void ResetRelativeRightSpace() {relativeRightSpace=relativeRightSpaceDefault;}
		internal void ResetRelativeFrontSpace() {relativeFrontSpace=relativeFrontSpaceDefault;}
		internal void ResetRelativeBackSpace() {relativeBackSpace=relativeBackSpaceDefault;}
		internal void ResetForceSquareBase() {forceSquare=forceSquareDefault;}
		internal void ResetRadius() {radius=radiusDefault;}
		internal void ResetRelativeHeight() {relativeHeight=relativeHeightDefault;}
		internal void ResetFirstSegmentStart() {firstSegmentStart=firstSegmentStartDefault;}
		internal void ResetRelativeEdgeSmoothingRadius() {relativeEdgeSmoothingRadius=relativeEdgeSmoothingRadiusDefault;}
		internal void ResetRelativeInnerRadius() {relativeInnerRadius=relativeInnerRadiusDefault;}
		internal void ResetRelativeInnerEdgeSmoothingRadius() 
		{
			relativeInnerEdgeSmoothingRadius=relativeInnerEdgeSmoothingRadiusDefault;
		}
		#endregion

		#region --- XML Serialization ---

		internal void CreateDOM(XmlElement parent)
		{
			XmlElement root = parent.OwnerDocument.CreateElement("SeriesStyle");
			XmlCustomSerializer S = new XmlCustomSerializer(root);
			S.Reading = false;
			Serialize(S);
			parent.AppendChild(root);
		}

		internal void Serialize(XmlCustomSerializer S)
		{
			S.AttributeProperty(this, "Name");
			S.AttributeProperty(this, "CompositionKind");
			S.AttributeProperty(this, "Radar", "IsRadar");

			if(S.BeginTag("ColorScheme"))
			{
				S.AttributeProperty(this, "Palette", "PaletteName");
				S.EndTag();
			}

			if(S.BeginTag("BorderLine"))
			{
				S.AttributeProperty(this, "StyleName", "BorderLineStyleName");
				S.EndTag();
			}

			if(S.BeginTag("Spacing"))
			{
				S.AttributeProperty(this, "Left",  "RelativeLeftSpace");
				S.AttributeProperty(this, "Right", "RelativeRightSpace");
				S.AttributeProperty(this, "Front", "RelativeFrontSpace");
				S.AttributeProperty(this, "Back",  "RelativeBackSpace");
				S.EndTag();
			}

			if(S.BeginTag("Shape"))
			{
				S.AttributeProperty(this, "SquareBase", "ForceSquareBase");
				S.AttributeProperty(this, "EdgeRadiusInPoints", "EdgeRadiusInPoints");
				S.EndTag();
			}

			if(S.BeginTag("PieSpecifics"))
			{
				S.AttributeProperty(this, "FirstSegmentStart","FirstSegmentStart","0.0##");
				S.AttributeProperty(this, "RelativeInnerRadius","RelativeInnerRadius","0.0##");
				S.AttributeProperty(this, "RelativeHeight","RelativeHeight","0.0##");
				S.AttributeProperty(this, "RelativeEdgeSmoothingRadius", "RelativeEdgeSmoothingRadius","0.0##");
				S.AttributeProperty(this, "RelativeInnerEdgeSmoothingRadius", "RelativeInnerEdgeSmoothingRadius","0.0##");
				S.EndTag();
			}

			if(S.BeginTag("Marker"))
			{
				S.AttributeProperty(this, "FirstSegmentStart");
				S.AttributeProperty(this, "StyleName", "MarkerStyleName");
				S.EndTag();
			}

			if(S.BeginTag("Line-Area"))
			{
				S.AttributeProperty(this, "LineKind");
				S.AttributeProperty(this, "LowerBoundLineKind");
				S.AttributeProperty(this, "UpperBoundLineKind");
				S.AttributeProperty(this, "LineStyleName");
				S.AttributeProperty(this, "LineStyle2DName");
				S.AttributeProperty(this, "GradientStyleName");
				S.EndTag();
			}
		}

		internal static SeriesStyle CreateFromDOM(XmlElement root)
		{
			if(root.Name != "SeriesStyle")
				return null;

			string styleName = root.GetAttribute("Name");
			SeriesStyle style = new SeriesStyle(styleName);

			XmlCustomSerializer CS = new XmlCustomSerializer (root);
			CS.Reading = true;
			style.Serialize(CS);
			return style;
		}

		#endregion

		#region --- Browser Control ---

		private bool ShouldBrowseChartKind()	{ return false; }
		private bool ShouldBrowseName()			{ return Removable; }

		private bool ShouldBrowseCompositionKind()	{ return chartKind != ChartKind.Line && chartKind != ChartKind.Marker; }
		
		private bool ShouldBrowseLineKind() { return chartKind == ChartKind.Line || IsArea; }
		private bool ShouldBrowseLowerBoundLineKind() { return IsArea; }
		private bool ShouldBrowseUpperBoundLineKind() { return IsArea; }

		
		private bool	ShouldBrowseSurface()							{ return false; }
		private bool	ShouldBrowseSecondarySurface()					{ return false; }
		private bool	ShouldBrowseLineStyleName()						{ return ChartKind == ChartKind.Line;}
		private bool	ShouldBrowseLineStyle2DName()					{ return ChartKind == ChartKind.Line2D; }
		private bool	ShouldBrowseBorderLineStyleName()				
		{
			return ChartKind == ChartKind.Rectangle 
				|| ChartKind == ChartKind.Pie 
				|| ChartKind == ChartKind.Doughnut; }
		private bool	ShouldBrowseGradientStyleName()					{ return ChartKind == ChartKind.Rectangle || ChartKind == ChartKind.Area2D; }

		// --- Linear shape properties ---

		private bool	ShouldBrowseEdgeRadiusInPoints()				{ return ChartKindCategory == ChartKindCategory.Bar && chartKind != ChartKind.Marker && chartKind != ChartKind.Rectangle; }
		private bool	ShouldBrowseRelativeLeftSpace()					{ return ChartKindCategory == ChartKindCategory.Bar && chartKind != ChartKind.Marker && chartKind != ChartKind.Rectangle; }
		private bool	ShouldBrowseRelativeRightSpace()				{ return ChartKindCategory == ChartKindCategory.Bar && chartKind != ChartKind.Marker && chartKind != ChartKind.Rectangle; }
		private bool	ShouldBrowseRelativeFrontSpace()				{ return ChartKindCategory == ChartKindCategory.Bar && chartKind != ChartKind.Marker && chartKind != ChartKind.Rectangle; }
		private bool	ShouldBrowseRelativeBackSpace()					{ return ChartKindCategory == ChartKindCategory.Bar && chartKind != ChartKind.Marker && chartKind != ChartKind.Rectangle; }
		private bool	ShouldBrowseForceSquareBase()					{ return ChartKindCategory == ChartKindCategory.Bar && chartKind != ChartKind.Marker && chartKind != ChartKind.Rectangle; }

		
		// --- Pie/Doughnut properties ---

		private bool	ShouldBrowseRadius()							{ return false; }
		private bool	ShouldBrowseRelativeHeight()					{ return !IsLinear; }
		private bool	ShouldBrowseFirstSegmentStart()					{ return !IsLinear; }
		private bool	ShouldBrowseRelativeEdgeSmoothingRadius()		{ return ChartKind == ChartKind.Pie; }
		private bool	ShouldBrowseRelativeOuterEdgeSmoothingRadius()	{ return ChartKind == ChartKind.Doughnut; }
		private bool	ShouldBrowseRelativeInnerRadius()				{ return ChartKind == ChartKind.Doughnut; }
		private bool	ShouldBrowseRelativeInnerEdgeSmoothingRadius()	{ return ChartKind == ChartKind.Doughnut; }

		// --- Marker properties ---

		private bool	ShouldBrowseMarkerStyleName()	{ return ChartKind == ChartKind.Marker; }

		private static string[] PropertiesOrder = new string[]
			{
				"Name",
				"ChartKind",
				"IsRadar",
				"LineKind",
				"LowerBoundLineKind",
				"UpperBoundLineKind",
				"LineStyleName",
				"LineStyleName2D",
				"MarkerStyleName",
				"BorderLineStyleName",
				"CompositionKind",
				"PaletteName",
				"ForceSquareBase",
				"RelativeLeftSpace",
				"RelativeRightSpace",
				"RelativeFrontSpace",
				"RelativeBackSpace",
				"EdgeRadiusInPoints",
				"Radius",
				"RelativeEdgeSmoothingRadius",
				"RealtiveHeight"
			};
		#endregion	
		
		#region --- Style Kind Handling ---

        /// <summary>
        /// Gets style name of specified style kind.
        /// </summary>
        public static string StyleNameOf(SeriesStyleKind styleKind)
		{
			return styleKind.ToString();
		}

        /// <summary>
        /// Gets style kind of specified style name.
        /// </summary>
        public static SeriesStyleKind StyleKindOf(string styleName)
		{
			try
			{
				return (SeriesStyleKind)Enum.Parse(typeof(SeriesStyleKind),styleName,true);
			}
			catch
			{
				return SeriesStyleKind.Custom;
			}
		}

		/// <summary>
		/// Gets style kind of this style object.
		/// </summary>
		public SeriesStyleKind StyleKind
		{
			get
			{
				return StyleKindOf(Name);
			}
		}
		#endregion

		internal bool IsApplicable(SeriesBase sb) 
		{
			if (sb.OwningSeries == null)
				return true;

			CompositeSeries cs = (CompositeSeries)(sb.OwningSeries);

			if (cs.CompositionKind == CompositionKind.MultiArea)
				return true;

			if (cs.CompositionKind == CompositionKind.Concentric) 
			{
				return (ChartKind == ChartKind.Pie || ChartKind == ChartKind.Doughnut);
			}

			if (cs.Style.IsRadar) 
			{
				return (IsRadar);
			}

			bool res = true;

			res = !(ChartKind == ChartKind.Pie || ChartKind == ChartKind.Doughnut || IsRadar);
			return res;
		}
	}
}
