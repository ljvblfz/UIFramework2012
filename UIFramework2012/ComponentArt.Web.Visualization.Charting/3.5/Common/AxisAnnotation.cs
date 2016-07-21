using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Diagnostics;
using ComponentArt.Web.Visualization.Charting.Design;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents a single label on the <see cref="AxisAnnotation"/> object.
	/// </summary>
	public class AxisLabel
	{
		Coordinate	coordinate;
		string		text = null;
		bool		rotate = false;
		LabelStyle	labelStyle;
		string		labelStyleName = null;
		double		scale = 1.0;
		double		rotationAngle = 0;
		AxisAnnotation	owningAnnotation = null;

		/// <summary>
		/// Initialises a new instance of <see cref="AxisLabel"/> class with specified coordinate and scale.
		/// </summary>
		/// <param name="coordinate">Coordinate at which the label is placed. This value is stored in the <see cref="AxisLabel.Coordinate"/> property.</param>
		/// <param name="scale">Factor by which the value is divided to get the label text. This value is stored in the <see cref="AxisLabel.Scale"/> property.</param>
		public AxisLabel(Coordinate coordinate, double scale)
		{
			this.coordinate = coordinate;
			if(scale > 0.0)
				this.scale = scale;
			else
				throw new Exception("Axis scale must be > 0");
		}

		/// <summary>
		/// Initialises a new instance of <see cref="AxisLabel"/> class with a specified coordinate.
		/// </summary>
		/// <param name="coordinate">Coordinate at which the label is placed. This value is stored in the <see cref="AxisLabel.Coordinate"/> property.</param>
		public AxisLabel(Coordinate coordinate) : this(coordinate,1.0) { }

		internal AxisLabel() : this(null) { }

		/// <summary>
		/// Gets or sets the label style of this label.
		/// </summary>
		public LabelStyle Style { get { if(labelStyle == null) labelStyle = EffectiveStyle; return labelStyle; } set { labelStyle = value; } }

		internal LabelStyle GetStyle() { return labelStyle; }

		internal string StyleName { get { return labelStyleName; } set { labelStyleName = value; labelStyle = null; } }
		
		
		/// <summary>
		/// Gets or sets the label style to be used in this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LabelStyleKind StyleKind
		{
			get { return LabelStyle.KindOf(StyleName); }
			set { StyleName = LabelStyle.NameOf(value); }
		}

		internal AxisAnnotation OwningAnnotation { get { return owningAnnotation; } set { owningAnnotation = value; } }
        
		internal LabelStyle EffectiveStyle
		{
			get
			{
				if(labelStyle != null)
					return labelStyle;
				LabelStyle style = null;
				if(labelStyleName != "" && labelStyleName != null && owningAnnotation != null)
					style = owningAnnotation.OwningChart.LabelStyles[labelStyleName];
				if(style == null)
					style = new LabelStyle();
				return style;
			}					
		}

		/// <summary>
		/// Gets or sets the factor by which the value is divided to get the label text. 
		/// </summary>
		public double Scale { get { return scale; } }

		/// <summary>
		/// Gets or sets the text of the label.
		/// </summary>
		public string Text 
		{
			get 
			{ 
				if(text == null)
					Format("");
				return text; 
			}
			set 
			{
				text = value; 
			} 
		}

		/// <summary>
		/// Gets or sets the value indicating if this label is rotated.
		/// </summary>
		public bool Rotate { get { return rotate; } set { rotate = value; } }

		/// <summary>
		/// Gets or sets the rotation angle of the label in degrees.
		/// </summary>
		public double RotationAngle { get { return rotationAngle; } set { rotationAngle = value; } }

		internal double EffectiveRotationAngle
		{
			get
			{
				if(rotationAngle == 0 && rotate)
					return 90;
				else
					return rotationAngle;
			}
		}

		/// <summary>
		/// Gets the coordinate of this <see cref="AxisLabel"/> object.
		/// </summary>
		public Coordinate Coordinate { get { return coordinate; } }

		/// <summary>
		/// Formats the label text.
		/// </summary>
		/// <param name="formatString">A format string.</param>
		/// <returns>
		/// The string representation of the value of this <see cref="AxisLabel"/> object as specified by formatString.
		/// </returns>
		public string Format(string formatString)
		{
			if (text != null)
				return text;
			string fs = formatString;
			if(fs=="")
				fs = "G";

			object label = coordinate.Value;
			if (label is double)
			{
				double dval = (double)label/scale;
				text = dval.ToString(fs);
			}
			else if (label is DateTime)
			{
				DateTime dval = (DateTime)label;
				if(fs=="G")
					fs = "d";
				text = dval.ToString(fs);
			}
			else if (label is string)
			{
				text = (string)label;
			}
			else
				text = label.ToString();
			return text;
		}
	}


	/// <summary>
	/// <see cref="AxisLabelCollection"/> is a class that stores the chart's <see cref="AxisLabel"/> objects. 
	/// </summary>
	public class AxisLabelCollection: CollectionWithType 
	{
		internal AxisLabelCollection() : base(typeof(AxisLabel)) { }

		/// <summary>
		/// Indicates the <see cref="AxisLabel"/> at the specified indexed location in the <see cref="AxisLabelCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based index to retrieve a <see cref="AxisLabel"/> from the <see cref="AxisLabelCollection"/> object.</param>
		public new AxisLabel this[object obj]
		{
			get { return (AxisLabel)(List[IndexOf(obj)]); } 
			set { List[IndexOf(obj)] = value;} 
		}

		public new int Add(object obj)
		{
			AxisLabel lab = obj as AxisLabel;
			if(lab == null)
				throw new ArgumentException("Cannot add a(n) " + obj.GetType().Name + " to an AxisLabelCollection");
			lab.OwningAnnotation = this.Owner as AxisAnnotation;
			return base.Add(obj);
		}

	}
	/// <summary>
	/// Defines the position of <see cref="AxisAnnotation"/> object.
	/// </summary>

	// ==========================================================================================================
	//		Axis Annotation 
	// ==========================================================================================================

	public enum AxisLinePositionKind
	{
		AtMaximumValue,
		AtMinimumValue,
		Custom
	}

	public enum AxisTitlePositionKind
	{
		Auto,
		AtMaximumValue,
		AtMinimumValue,
		AtMiddlePoint,
		Custom
	};

	
	public enum AxisAnnotationKind
	{
		XAtYMin,
		XAtYMax,
		XAtZMin,
		XAtZMax,

		YAtXMin,
		YAtXMax,
		YAtZMin,
		YAtYMax,

		ZAtXMin,
		ZAtXMax,
		ZAtYMin,
		ZAtYMax,

		Custom
	}


	/// <summary>
	/// This class displays <see cref="Axis"/> information.
	/// </summary>
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	[Serializable]
	public class AxisAnnotation : ChartObject,INamedObject
	{
		// ----------- Variables REALLY used -------------------

		private AxisLabelCollection		labels = null;
		private string			labelStyleName = "DefaultAxisLabels";
		private string			lineStyleName = "AxisLine";
		private CoordinatePlane plane = null;
		private AxisOrientation alternateAxisOrientation;
		private AxisLinePositionKind positionKind = AxisLinePositionKind.Custom;
		private bool			alternateTextOrientation = false;

		private double			rotationAngle = 0;

		private CoordinateSet   coordinates;
		private string			formatString = "G";
		private int				numberOfLabels = 0;

		private double			relativePosition = 0;
		private double			coord2;

		private TextOrientation	textOrientation = TextOrientation.Default;
		private TextReferencePoint textReferencePoint = TextReferencePoint.Default;

		private PolarTextOrientation polarTextOrientation = PolarTextOrientation.Circular;
		private double			hOffset=5, vOffset=5;

		private bool visibleIn2D = true;
		private bool visibleIn3D = true;

		// --- Computed Variables ---
		private Axis axis2;

		private bool				hasChanged = true;	// setialization on by default 

		private double				liftZ = 0.2;

		// Tick marks data

		private TickMarkCollection	tickMarks = null;
		private string				tickMarkStyleName = "Default";
		private bool				tickMarksVisible = true;

		private bool				labelsVisible = true;
		private bool				lineVisible = true;

		// Axis Title

		const double				defaultAxisTitleOffsetPts = 40;
		private string				axisTitle = "";
		private string				axisTitleStyleName = "DefaultAxisLabels";
		private double				axisTitleOffsetPts = defaultAxisTitleOffsetPts;
		private bool 				axisTitleOffsetPtsIsDefaultZero = true;
		private LabelStyle			axisTitleStyle;
		private AxisTitlePositionKind axisTitlePositionKind = AxisTitlePositionKind.Auto;
		private double				titleReferencePosition = 0.0;

		// INamedObject I/F data
		private string				name;
		private AxisAnnotationCollection owningCollection;

		#region --- Constructors ---

		internal AxisAnnotation(string name, AxisOrientation alternateAxisOrientation, AxisLinePositionKind positionKind)
		{
			this.name = name;
			this.alternateAxisOrientation = alternateAxisOrientation;
			this.positionKind = positionKind;
		}

		/// <summary>
		/// Instantiates a new axis annotation object.
		/// </summary>
		/// <param name="name">Name of the axis annotation.</param>
		public AxisAnnotation(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Instantiates a new axis annotation object.
		/// </summary>
		public AxisAnnotation() : this("") { }

		#endregion

		#region --- INamedObject Interface Implementation ---
		/// <summary>
		/// Gets or sets the name of this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[SRDescription("AxisAnnotationNameDescr")]
		public string Name 
		{
			get { return name; } 
			set 
			{
				name = value; 
				if(owningCollection != null)
				{
					owningCollection.Remove(this);
					owningCollection.Add(this);
				}
			} 
		}

		/// <summary>
		/// Gets or sets the parent collection of this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public NamedCollection OwningCollection 
		{
			get { return owningCollection; } 
			set 
			{
				if(!(value is AxisAnnotationCollection))
					throw new Exception("Invalid type of value");
				owningCollection = value as AxisAnnotationCollection; 
			}
		}

		#endregion

		#region --- Computing Effective Label Style ---

		private LabelStyle GetEffectiveLabelStyle(AxisLabel label)
		{
			LabelStyle style = label.GetStyle();

			// Initialize style 
			
			LabelStyle eStyle = new LabelStyle();
			if(style == null)
			{
				if(OwningChart != null)
				{
					style = OwningChart.LabelStyles[label.StyleName];
					if(style == null)
						style = OwningChart.LabelStyles[labelStyleName];
				}
				else
					eStyle.SetOwningChart(null);
			}
			if(style != null)
				eStyle.LoadFrom(style);

			// Computing effective styles
			if(eStyle.Orientation == TextOrientation.Default)
			{
				AxisAnnotationPlane aap = new AxisAnnotationPlane(this);
				aap.AdjustAxisLabelStyle(eStyle,label.EffectiveRotationAngle);
			}
			return eStyle;
		}

		private void GetAxis2()
		{
			if(Plane.XAxis == Axis)
				axis2 = Plane.YAxis;
			else
				axis2 = Plane.XAxis;
		}

		#endregion

		#region --- Public Browsable Properties ---

		#region Category "General"

		/// <summary>
		/// Gets or sets a value indcating whether the labels in this <see cref="AxisAnnotation"/> object are visible.
		/// </summary>
		[SRDescription("AxisAnnotationLabelsVisibleDescr")]
		[Category("General")]
		[DefaultValue(true)]
		public bool LabelsVisible	{ get { return labelsVisible; } set { if(labelsVisible != value) hasChanged = true; labelsVisible = value; } }

		/// <summary>
		/// Gets or sets a value indicating whether the line at this <see cref="AxisAnnotation"/> object is visible.
		/// </summary>
		[SRDescription("AxisAnnotationLineVisibleDescr")]
		[Category("General")]
		[DefaultValue(true)]
		public bool LineVisible		{ get { return lineVisible; }	set { if(lineVisible != value) hasChanged = true; lineVisible = value; } }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="AxisAnnotation"/> object is visible.
		/// </summary>
		[Category("General")]
		[DefaultValue(true)]
		[Description("Determines whether to show the annotation.")]
		[Browsable(false)]
		public override bool Visible 
		{
			get 
			{
				if (CoordinateSystem.OwningSeries.TargetArea.Mapping.Kind == ProjectionKind.TwoDimensional ||
					Name.EndsWith("X@Ymin") && !Axis.CoordSystem.PlaneZX.Visible ||
					Name.EndsWith("Y@Xmin") && !Axis.CoordSystem.PlaneYZ.Visible )
					return visibleIn2D;
				else
					return VisibleIn3D;
			}
			set 
			{
				if (visibleIn3D != value || visibleIn2D != value)
				{
					hasChanged = true;
					visibleIn2D = value;
					visibleIn3D = value;
				}
			}
		}


		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="AxisAnnotation"/> object is visible when the chart is two-dimensional.
		/// </summary>
		[Category("General")]
		[DefaultValue(true)]
		[Description("Determines whether to show the annotation in 2D chart.")]
		public bool VisibleIn2D { get { return visibleIn2D; } set { visibleIn2D = value; } }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="AxisAnnotation"/> object is visible when the chart is three-dimensional.
		/// </summary>
		[Category("General")]
		[DefaultValue(true)]
		[Description("Determines whether to show the annotation in 3D chart.")]
		public bool VisibleIn3D { get { return visibleIn3D; } set { visibleIn3D = value; } }

		#endregion

		#region Category "Position and Orientation"

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public AxisAnnotationKind Kind
		{
			get
			{
				return AxisAnnotation.KindOf(Name);
			}
			set
			{
				Name = AxisAnnotation.NameOf(value);
			}
		}

		/// <summary>
		/// Gets or sets the text orientation for radar chart x-coordinate values.
		/// </summary>
		[SRDescription("RadarTextOrientationDescr")]
		[Category("Position and Orientation")]
		[DefaultValue(PolarTextOrientation.Circular)]
		public PolarTextOrientation RadarTextOrientation { get { return polarTextOrientation; } set { polarTextOrientation = value; } }


		/// <summary>
		/// Gets or sets the imaginary point on the text label at which it is attached to this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[SRDescription("AxisAnnotationReferencePointDescr")]
		[Category("Position and Orientation")]
		[DefaultValue(TextReferencePoint.Default)]
		public TextReferencePoint	ReferencePoint		{ get { return textReferencePoint; }	set { if(textReferencePoint != value) hasChanged = true; textReferencePoint = value; } }
		
		/// <summary>
		/// Gets or sets the text orientation of this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[SRDescription("AxisAnnotationTextOrientationDescr")]
		[Category("Position and Orientation")]
		[DefaultValue(TextOrientation.Default)]
		public TextOrientation		TextOrientation		{ get { return textOrientation; } set { if(textOrientation != value) hasChanged = true; textOrientation = value; } }
		
		/// <summary>
		/// Gets or sets the value indicating whether the labels are rotated in this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[Category("Position and Orientation")]
		[DefaultValue(false)]
		[Description("Determines whether to rotate the annotation.")]
		public bool		Rotate		{ get { return alternateTextOrientation; } set { if(alternateTextOrientation != value) hasChanged = true; alternateTextOrientation = value; } }
		
		/// <summary>
		/// Gets or sets the value indicating whether the labels are rotated in this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[Category("Position and Orientation")]
		[DefaultValue(false)]
		[Description("Rotation angle of the annotation in degrees.")]
		public double	RotationAngle		{ get { return rotationAngle; } set { if(rotationAngle != value) hasChanged = true; rotationAngle = value; } }

		/// <summary>
		/// Gets or sets the offset of this <see cref="AxisAnnotation"/> object in the horizontal direction of the text. 
		/// </summary>
		[SRDescription("AxisAnnotationHOffsetDescr")]
		[Category("Position and Orientation")]
		[DefaultValue(5.0)]
		public double HOffset { get { return hOffset; } set { if(hOffset != value) hasChanged = true; hOffset = value; } }

		/// <summary>
		/// Gets or sets the offset of this <see cref="AxisAnnotation"/> object in the vertical direction of the text. 
		/// </summary>
		[SRDescription("AxisAnnotationVOffsetDescr")]
		[Category("Position and Orientation")]
		[DefaultValue(5.0)]
		public double VOffset { get { return vOffset; } set { if(vOffset != value) hasChanged = true; vOffset = value; } }

		/// <summary>
		/// Gets or sets the z displacement of this <see cref="AxisAnnotation"/> object in the target coordinate system.
		/// </summary>
		[SRDescription("AxisAnnotationLiftZDescr")]
		[Category("Position and Orientation")]
		[DefaultValue(0.1)]
		public double				LiftZ				{ get { return liftZ; } set { if(liftZ != value) hasChanged = true; liftZ = value; } }

		/// <summary>
		/// Gets or sets the position of this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[SRDescription("AxisAnnotationPositionKindDescr")]
		[Category("Position and Orientation")]
		[DefaultValue(AxisLinePositionKind.Custom)]
		public AxisLinePositionKind PositionKind			{ get { return positionKind; } set { positionKind = value; } }

		#endregion

		#region Category "Style and Formatting"
		/// <summary>
		/// Gets or sets the format string used to convert axis values to string in this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[SRDescription("AxisAnnotationFormatStringDescr")]
		[Category("Style and Formatting")]
		[DefaultValue("G")]
		public string FormatString	{ get { return formatString; } set { if(formatString != value) hasChanged = true; formatString = value; } }

		/// <summary>
		/// Gets or sets the label style to be used in this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[Category("Style and Formatting")]
		[DefaultValue("DefaultAxisLabels")]
		[TypeConverter(typeof(SelectedLabelStyleConverter))]
		[Description("Annotation label style.")]
		public string LabelStyleName
		{
			get { return (labelStyleName=="")? "Default":labelStyleName; }
			set { if(labelStyleName != value) hasChanged = true; labelStyleName = value; }
		}

		/// <summary>
		/// Gets or sets the label style to be used in this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LabelStyleKind LabelStyleKind
		{
			get { return LabelStyle.KindOf(LabelStyleName); }
			set { LabelStyleName = LabelStyle.NameOf(value); }
		}

		/// <summary>
		/// Gets or sets the line style name of the line along this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[Category("Style and Formatting")]
		[DefaultValue("AxisLine")]
		[TypeConverter(typeof(SelectedLineStyle2DConverter))]
		[Description("Annotation line style.")]
		public string LineStyleName
		{
			get { return lineStyleName; }
			set { if(lineStyleName != value) hasChanged = true; lineStyleName = value; }
		}

		/// <summary>
		/// Gets or sets the minimum number of labels in this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[SRDescription("AxisAnnotationMinimumNumberOfLabelsDescr")]
		[Category("Style and Formatting")]
		[DefaultValue(0)]
		public int MinimumNumberOfLabels { get { return numberOfLabels; } set { numberOfLabels = value; labels = null; } }

		#endregion

		#region Category "Axis Title"

		/// <summary>
		/// Gets or sets the title of this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[Category("Axis Title")]
		[DefaultValue("")]
		[Description("Text to be displayed within the annotation.")]
		public string AxisTitle { get { return axisTitle; } set { if(axisTitle != value)hasChanged = true; axisTitle = value; } }
		
		/// <summary>
		/// Gets or sets the title style name in this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[Category("Axis Title")]
		[DefaultValue("DefaultAxisLabels")]
		[TypeConverter(typeof(SelectedLabelStyleConverter))]
		[Description("Text style for the title in this annotation.")]
		public string AxisTitleStyleName { get { return axisTitleStyleName; } set { if(axisTitleStyleName != value)hasChanged = true; axisTitleStyleName = value; } }

		/// <summary>
		/// The <see cref="TextStyleKind"/> property of the axis title. 
		/// </summary>
		/// <remarks>
		/// This property could be used instead of the <see cref="AxisTitleStyleName"/> when the style is one of the predefined text styles.
		/// </remarks>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextStyleKind AxisTitleStyleKind 
		{
			get
			{
				return TextStyle.KindOf(AxisTitleStyleName);
			}
			set
			{
				AxisTitleStyleName = TextStyle.NameOf(value);
			}
		}

		/// <summary>
		/// Gets or sets the offset of the title in points in this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[Category("Axis Title")]
		[DefaultValue(defaultAxisTitleOffsetPts)]
		[Description("Offset of the axis title in points.")]
		public double AxisTitleOffsetPts 
		{
			get { return axisTitleOffsetPts; } 
			set 
			{
				if(axisTitleOffsetPts != value)
					hasChanged = true; 
				axisTitleOffsetPts = value; 
				axisTitleOffsetPtsIsDefaultZero = false;
			}
		}

        /// <summary>
		/// Gets or sets the positionning method of the axis title.
		/// </summary>
		[Category("Axis Title")]
		[DefaultValue(typeof(AxisTitlePositionKind),"Auto")]
		[Description("Axis title positionning method.")]
		public AxisTitlePositionKind AxisTitlePositionKind 
		{
			get { return axisTitlePositionKind; } 
			set 
			{
				if(axisTitlePositionKind != value)
					hasChanged = true; 
				axisTitlePositionKind = value; 
			}
		}

		/// <summary>
		/// Property indicating title reference position on the axis line. Valid values are between 0 and 1. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// Value of 0 sets the reference point to the minimum axis value. 
		/// Likewise, value of 1 sets the reference point to the maximum value. 
        /// Other values set the reference point between the minimum and the maximum value.
		/// </para>
		/// <para>
		/// Setting this property automatically sets the <see cref="AxisTitlePositionKind"/> to <see cref="AxisTitlePositionKind.Custom"/>.
		/// </para>
		/// </remarks>
		[Category("Axis Title")]
		[DefaultValue(0)]
		[Description("Axis title reference position (between 0 and 1).")]
		public double TitleReferencePosition { get { return titleReferencePosition; } set { if(titleReferencePosition != value)hasChanged = true; titleReferencePosition = value; AxisTitlePositionKind = AxisTitlePositionKind.Custom; } }

		#endregion
		#endregion

		#region --- Public Non-Browsable Properties ---


		/// <summary>
		/// Sets the value of the orthogonal axis at which this <see cref="AxisAnnotation"/> object should be located.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public object Position 
		{ 
			set { GetAxis2(); coord2 = axis2.ICoordinate(value); } 
		}

		/// <summary>
		/// Gets or sets the position along the orthogonal axis in the intermediate coordinate system of this <see cref="AxisAnnotation"/> object.
		/// </summary>
		/// <remarks>
		/// This property should be only called after the DataBind() call.
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public double PositionICS 
		{
			get { return coord2;}
			set { coord2 = value; } 
		}

		/// <summary>
		/// Gets or sets the relative position along the orthogonal axis. Value 0 sets position
		/// at minimum coordinate along the orthogonal axis, value 1 sets position at maximum coordinate.
		/// </summary>
		/// <remarks>
		/// This value is used only if PositionKind is set to Custom.
		/// </remarks>
		[DefaultValue(0.0)]
		[Description("Relative position of annotation along the orthogonal axis")]
		public double RelativePosition
		{
			get { return relativePosition;}
			set { relativePosition = value; } 
		}

		/// <summary>
		/// Gets or sets the <see cref="CoordinateSet"/> of this <see cref="AxisAnnotation"/> object.
		/// </summary>
		/// <remarks>
		/// This property should be only called after the DataBind() call.
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public CoordinateSet Coordinates 
		{ 
			get 
			{ 
				if(coordinates == null || OwningChart.InDesignMode)
				{
					if(numberOfLabels > 0)
						coordinates = Axis.CreateCoordinateSet(numberOfLabels);
					else
					{
						bool roundValues = Axis.RoundValueRange;
						if(roundValues)
							coordinates = Axis.DefaultCoordinateSet.GetCopy();
						else
						{
							CoordinateSetComputation csc = Axis.DefaultCoordinateSetComputation.Clone() as CoordinateSetComputation;
							DataDimension dd = Axis.Dimension;
							csc.RoundValueRange = true;
							CoordinateSet cst = csc.Value;
							if(dd.Compare(cst[0].Value,Axis.MinValue) < 0)
								cst.Remove(cst[0]);
							Coordinate last = cst[cst.Count-1];
							if(dd.Compare(Axis.MaxValue,last.Value) < 0)
								cst.Remove(last);
							coordinates = cst;
						}
					}
				}
				return coordinates; 
			} 
			set { coordinates = value; } 
		}		

		/// <summary>
		/// Gets the tick mark collection.
		/// </summary>
		/// <remarks>
		/// This property should be only called after the DataBind() call.
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public TickMarkCollection TickMarks	
		{
			get
			{
				if(tickMarks == null)
					tickMarks = GetTickMarks();
				return tickMarks;
			}
		}

		private TickMarkCollection GetTickMarks()
		{
			if(tickMarks != null)
				return tickMarks;

			double min = this.Axis.MinValueLCS;
			double max = this.Axis.MaxValueLCS;
			if(min>max)
			{
				double a = min;
				min = max; 
				max = a;
			}
			min -= (max-min)*0.001;
			max += (max-min)*0.001;

			TickMarkCollection newTickMarks = new TickMarkCollection();
			newTickMarks.SetOwner(this);
			CoordinateSet cSet = Coordinates;
			for(int i = 0; i<cSet.Count; i++)
			{
				double c = Axis.Dimension.Coordinate(cSet[i].Value);
				double w = Axis.Dimension.Width(cSet[i].Value);
				if(c < min || c > max )
					continue;
				TickMark tm = new TickMark(this,tickMarkStyleName,cSet[i]);
				tm.SetOwner(this);
				newTickMarks.Add(tm);
			}
			return newTickMarks;
		}

		/// <summary>
		/// Gets or sets the value indicating whether the tick marks are visible in this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[SRCategory("CatTickMarks")]
		[SRDescription("AxisAnnotationTickMarksVisibleDescr")]
		[DefaultValue(true)]
		public bool TickMarksVisible { get { return tickMarksVisible; } set { tickMarksVisible = value; } }


		/// <summary>
		/// Gets or sets the tick mark style name to be used in this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[SRCategory("CatTickMarks")]
		[SRDescription("AxisAnnotationTickMarkStyleNameDescr")]
		[TypeConverter(typeof(SelectedTickMarkStyleConverter))]
		public string TickMarkStyleName { get { return tickMarkStyleName; } set { tickMarkStyleName = value; tickMarks = null; } }

		/// <summary>
		/// The <see cref="TextStyleKind"/> of the axis title. 
		/// </summary>
		/// <remarks>
		/// This property could be used instead of the <see cref="AxisTitleStyleName"/> 
        /// when the style is one of the predefined text styles.
		/// </remarks>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TickMarkStyleKind TickMarkStyleKind 
		{
			get
			{
				return TickMarkStyle.KindOf(TickMarkStyleName);
			}
			set
			{
				TickMarkStyleName = TickMarkStyle.NameOf(value);
			}
		}
		/// <summary>
		/// Gets the labels of this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public AxisLabelCollection Labels	
		{ 
			get 
			{
				if(labels==null || OwningChart.InDesignMode)
				{
					CompositeSeries pcs = Axis.CoordSystem.OwningSeries as CompositeSeries;
					if(pcs != null && pcs.CompositionKind == CompositionKind.MultiSystem)
						labels = new AxisLabelCollection();
					else
						CreateLabels(Coordinates);
				}
				return labels; 
			}
		}

		/// <summary>
		/// Gets the coordinate plane of this <see cref="AxisAnnotation"/> object.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public CoordinatePlane Plane 
		{ 
			get 
			{
				if(plane == null)
				{
					Axis a = this.Axis;
					if(a.CoordSystem != null)
					{
						CoordinateSystem sys = a.CoordSystem;
						if(sys.PlaneXY.XAxis == a && sys.PlaneXY.YAxis.Role == alternateAxisOrientation ||
							sys.PlaneXY.YAxis == a && sys.PlaneXY.XAxis.Role == alternateAxisOrientation)
							plane = sys.PlaneXY;
						else if(sys.PlaneYZ.XAxis == a && sys.PlaneYZ.YAxis.Role == alternateAxisOrientation ||
							sys.PlaneYZ.YAxis == a && sys.PlaneYZ.XAxis.Role == alternateAxisOrientation)
							plane = sys.PlaneYZ;
						else if(sys.PlaneZX.XAxis == a && sys.PlaneZX.YAxis.Role == alternateAxisOrientation ||
							sys.PlaneZX.YAxis == a && sys.PlaneZX.XAxis.Role == alternateAxisOrientation)
							plane = sys.PlaneZX;
						else
							throw new Exception("Axis annotation '" + this.Name + "': invalid alternate axis orientation '" + alternateAxisOrientation.ToString() + "'");
					}
				}
				return plane; 
			} 
		}

		[Browsable(false)]
		public AxisOrientation AlternateAxisOrientation { get { return alternateAxisOrientation; } set { alternateAxisOrientation = value; plane = null; } }

		#endregion

		#region --- Internal and Private Properties ---
		
		public override string ToString() { return Name; }
		internal CoordinateSystem CoordinateSystem { get { return Axis.CoordSystem; } }
		internal TargetArea TargetArea { get { return CoordinateSystem.TargetArea; } }

		internal bool HasChanged { get { return hasChanged; } set { hasChanged = value; } }

		/// <summary>
		/// Label style of the axis title.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LabelStyle AxisTitleStyle 
		{
			get 
			{ 
				if(axisTitleStyle == null)
				{
					axisTitleStyle = new LabelStyle();
					LabelStyle ts = OwningChart.LabelStyles[axisTitleStyleName];
					if(ts != null)
						axisTitleStyle.LoadFrom(ts);
				}
				return axisTitleStyle; 
			} 
		}

		#endregion
		
		#region --- Internal and private Methods ---

		#region --- Handling Annotation Kind ---
		static string[] names = new string[]
			{
				"X@Ymin",
				"X@Ymax",
				"X@Zmin",
				"X@Zmax",

				"Y@Xmin",
				"Y@Xmax",
				"Y@Zmin",
				"Y@Ymax",

				"Z@Xmin",
				"Z@Xmax",
				"Z@Ymin",
				"Z@Ymax",

				"Custom"
			};
		static AxisAnnotationKind[] kinds = new AxisAnnotationKind[]
			{
				AxisAnnotationKind.XAtYMin,
				AxisAnnotationKind.XAtYMax,
				AxisAnnotationKind.XAtZMin,
				AxisAnnotationKind.XAtZMax,

				AxisAnnotationKind.YAtXMin,
				AxisAnnotationKind.YAtXMax,
				AxisAnnotationKind.YAtZMin,
				AxisAnnotationKind.YAtYMax,

				AxisAnnotationKind.ZAtXMin,
				AxisAnnotationKind.ZAtXMax,
				AxisAnnotationKind.ZAtYMin,
				AxisAnnotationKind.ZAtYMax,

				AxisAnnotationKind.Custom
			};
		internal static AxisAnnotationKind KindOf(string annotationName)
		{
			for(int i=0; i<names.Length; i++)
				if(names[i]==annotationName)
					return kinds[i];
			return AxisAnnotationKind.Custom;
		}

		internal static string NameOf(AxisAnnotationKind kind)
		{
			for(int i=0; i<names.Length; i++)
				if(kinds[i]==kind)
					return names[i];
			throw new Exception("Implementation error: missmatch of names/kinds array in AxisAnnotation class.");
		}
		#endregion

		internal Axis Axis
		{
			get 
			{			
				if(Owner is Axis)
					return Owner as Axis;
				if(Owner.Owner is Axis)
					return Owner.Owner as Axis;
				return null;
			}
		}

		internal override void SetOwner(ChartObject owner)
		{
			base.SetOwner(owner);
		}

		internal override void Build()
		{
			GetAxis2();
			if(labels == null)
				labels = Labels;
		}

		internal void DataBind()
		{
			if(Plane != null)
			{
				hOffset = Plane.Depth;
				vOffset = Plane.Depth;
			}
			tickMarks = null;
			if(OwningChart.InitializeOnDataBind)
			{
				coordinates = null;
				labels = null;
			}
			if(PositionKind == AxisLinePositionKind.Custom)
			{
				GetAxis2();
				coord2 = relativePosition*axis2.MaxValueICS +
					(1-relativePosition)*axis2.MinValueICS;
			}
		}

		private AxisLabelCollection CreateLabels(CoordinateSet coordinates)
		{
			int n = coordinates.Count;
			labels = new AxisLabelCollection();

			DataDimension dimension = Axis.Dimension;
			bool isZAxis = Axis.CoordSystem.ZAxis == Axis;

			for(int i=0; i<n; i++)
			{
				Coordinate coord = coordinates[i];
				coord.UpdateOffsetWidth(dimension);
				AxisLabel label = new AxisLabel(coord,Axis.Scale);
				if (isZAxis)
				{
					SeriesBase sb = OwningChart.Series.FindSeriesBase(coord.Value.ToString());
					if (sb != null)
						label.Text = sb.GetEffectiveLabel();
				}
				label.Rotate = Rotate;
				label.RotationAngle = rotationAngle;
				labels.Add(label);
				label.StyleName = labelStyleName;
				label.Format(formatString);
			}
			return labels;
		}

		private void RenderRadar()
		{
			if(!Visible)
				return;

			Axis axis = this.Axis;

			// Labels

			Axis XAxis = CoordinateSystem.XAxis;
			Axis YAxis = CoordinateSystem.YAxis;
			CoordinatePlane planeXY = CoordinateSystem.PlaneXY;

			// Range of LCoordinates
			double lcMinimum, lcMaximum;
			lcMinimum = Axis.MinValueLCS;
			lcMaximum = Axis.MaxValueLCS;

			lcMinimum -= (lcMaximum-lcMinimum)*0.0001;
			lcMaximum += (lcMaximum-lcMinimum)*0.0001;

			if(LabelsVisible)
			{
				double xd, yd;
                DrawingBoard drawingBoard = CreateRadarDrawingBoard(out xd, out yd);
				drawingBoard.LiftZ = 1.0;
				for(int i=0; i<coordinates.Count; i++)
				{
					Coordinate coordinate = coordinates[i];
					LabelStyle eStyle = labels[i].Style;
					string text = labels[i].Text;

					Color oldColor = eStyle.ForeColor;
					if(oldColor.A == 0)
						eStyle.ForeColor = OwningChart.Palette.CoodinateLabelFontColor;

					Vector3D refPoint;
					double logicalCoordinate = axis.LCS2ICS(coordinate.Offset);
					if(axis == planeXY.YAxis) // the axis is radial
					{
						refPoint = planeXY.LogicalToWorld(0.0,logicalCoordinate);
						eStyle.ReferencePoint = TextReferencePoint.LeftCenter; // TODO: put into the style!
						eStyle.HOffset = 3;
						eStyle.VOffset = 0; 
						drawingBoard.DrawString(text,labels[i].Style, new PointF((float)(refPoint.X+xd), (float)(refPoint.Y+yd)),0);
					}
					else // The axis is circular
					{
						refPoint = planeXY.LogicalToWorld(logicalCoordinate,planeXY.YAxis.MaxValueICS);
						double angleDegrees = planeXY.YAngleAtLogical(logicalCoordinate)/Math.PI*180.0;
						if(angleDegrees>360) angleDegrees -= 360;
						double angle = 0;
						eStyle.HOffset = 0;
						eStyle.VOffset = 0;
						switch(polarTextOrientation)
						{
							case PolarTextOrientation.Circular:
								angle = angleDegrees - 90;
								eStyle.ReferencePoint = TextReferencePoint.CenterBottom; 
								eStyle.VOffset = 5;
								break;
							case PolarTextOrientation.Horizontal:
								angle = 0;
								if(angleDegrees < 0) angleDegrees += 360;
								if(angleDegrees < 1 || angleDegrees > 359)
								{
									eStyle.ReferencePoint = TextReferencePoint.LeftCenter; 
								}
								else if (angleDegrees < 89)
								{
									eStyle.ReferencePoint = TextReferencePoint.LeftBottom; 
								}
								else if (angleDegrees < 91)
								{
									eStyle.ReferencePoint = TextReferencePoint.CenterBottom;
								}
								else if (angleDegrees < 179)
								{
									eStyle.ReferencePoint = TextReferencePoint.RightBottom;
								}
								else if (angleDegrees < 181)
								{
									eStyle.ReferencePoint = TextReferencePoint.RightCenter;
								}
								else if (angleDegrees < 269)
								{
									eStyle.ReferencePoint = TextReferencePoint.RightTop;
								}
								else if (angleDegrees < 271)
								{
									eStyle.ReferencePoint = TextReferencePoint.CenterTop;
								}
								else 
								{
									eStyle.ReferencePoint = TextReferencePoint.LeftTop;
								}
								break;
							case PolarTextOrientation.Radial:
								angle = angleDegrees;
								eStyle.ReferencePoint = TextReferencePoint.LeftCenter;
								break;
						}
						drawingBoard.DrawString(text,eStyle, new PointF((float)(refPoint.X+xd), (float)(refPoint.Y+yd)),angle);
					}					
					eStyle.ForeColor = oldColor;
				}
			}
		}

		internal DrawingBoard CreateRadarDrawingBoard(out double xd, out double yd)
		{
			DrawingBoard drawingBoard = GE.CreateDrawingBoard();
			Mapping map = CoordinateSystem.TargetArea.Mapping;

			// Finding drawing board parameters so that it covers full output bitmap
			// The "map" does mapping:
			//   xm = x0 + a*x
			//   ym = y0 + b*y
			// The inverse mapping is
			//   x = x0m + am*xm
			//   y = y0m + bm*ym

			Size sz = CoordinateSystem.TargetArea.Mapping.TargetSize;
			Vector3D P00 = map.Map(new Vector3D(0,0,0));
			Vector3D P10 = map.Map(new Vector3D(1,0,0));
			Vector3D P01 = map.Map(new Vector3D(0,1,0));
			double am = 1.0/(P10.X-P00.X);
			double bm = 1.0/(P01.Y-P00.Y);
			double x0m = - P00.X*am;
			double y0m = - P00.Y*bm;
			xd = -x0m;
			yd = -y0m;
			Vector3D V0 = new Vector3D(x0m,y0m,0);
			Vector3D Vx = new Vector3D(am*sz.Width,0,0);
			Vector3D Vy = new Vector3D(0,bm*sz.Height,0);
	
			drawingBoard.V0 = V0;
			drawingBoard.Vx = Vx;
			drawingBoard.Vy = Vy;
			drawingBoard.Initialize();
			drawingBoard.LiftZ = 0.1;
			
			return drawingBoard;
		}

		internal Vector3D WCSCoordinate(double aICS)
		{
			double bICS;
			switch(positionKind)
			{
				case AxisLinePositionKind.AtMaximumValue:
					bICS = axis2.MaxValueICS;
					break;
				case AxisLinePositionKind.AtMinimumValue:
					bICS = axis2.MinValueICS;
					break;
				default:
					bICS = coord2;
					break;
			}

			if(Axis == Plane.XAxis)
				return Plane.ICS2WCS(aICS,bICS);
			else
				return Plane.ICS2WCS(bICS,aICS);

		}

		private void RenderLinear()
		{
			if(!Visible)
				return;

			if(!PushOwningCoordinatePlaneBox())
				return;

			Axis axis = this.Axis;

			// Range of LCoordinates
			double lcMinimum, lcMaximum;
			lcMinimum = Axis.MinValueLCS;
			lcMaximum = Axis.MaxValueLCS;

			lcMinimum -= (lcMaximum-lcMinimum)*0.0001;
			lcMaximum += (lcMaximum-lcMinimum)*0.0001;

			// The other two coordinates (fixed)
			double coordinate2ICS = Coordinate2ICS;
			double coordinate3ICS = Coordinate3ICS;

			bool trackObjects = OwningChart.ObjectTrackingEnabled;

			if(Labels != null && LabelsVisible )
			{
				for(int i=0; i<labels.Count; i++)
				{
					LabelStyle lStyle = GetEffectiveLabelStyle(labels[i]);
                    LabelStyle eStyle = new LabelStyle(lStyle);
					if(TargetArea.IsTwoDimensional)
						eStyle.LiftZ = 1000;
					eStyle.Angle = labels[i].EffectiveRotationAngle;
					Coordinate coordinate = labels[i].Coordinate;
					string text = labels[i].Text;

					Color oldColor = eStyle.ForeColor;
					if(oldColor.A == 0)
						eStyle.ForeColor = OwningChart.Palette.CoodinateLabelFontColor;
					if(this.ReferencePoint != TextReferencePoint.Default)
						eStyle.ReferencePoint = this.ReferencePoint;

					double coord = coordinate.Offset + coordinate.Width*0.5;
					if(lcMinimum <= coord && coord < lcMaximum)
					{
						if(trackObjects)
							GE.SetActiveObject(coordinate);
						GE.CreateText(eStyle,WCSCoordinate(Axis.LCS2ICS(coord)),text);
						if(trackObjects)
							GE.SetActiveObject(null);
					}
				}
			}

			// Tick Marks

			if(tickMarksVisible)
			{
				foreach(TickMark tm in GetTickMarks())
				{
					if(positionKind == AxisLinePositionKind.AtMinimumValue)
						tm.InDirection = axis2.UnitVector;
					else 
						tm.InDirection = -axis2.UnitVector;
					tm.OutDirection = -tm.InDirection;
					tm.Render();
				}
			}

			// Line
				
			if((lineStyleName != "") && lineVisible)
			{
				LineStyle2D style = OwningChart.LineStyles2D[lineStyleName];
				if(style != null)
				{
					Vector3D P0 = WCSCoordinate(Axis.MinValueICS);
					Vector3D P1 = WCSCoordinate(Axis.MaxValueICS);
					Vector3D wx = axis2.UnitVector*style.Width*OwningChart.FromPointToWorld;
					Vector3D wy = Axis.UnitVector*style.Width*OwningChart.FromPointToWorld;
                    DrawingBoard brd = GE.CreateDrawingBoard(P0 - wx - wy, wx * 2, P1 - P0 + wy * 2);

					//brd.SetOwner(this);
					if(TargetArea.IsTwoDimensional)
						brd.LiftZ = 1000;
					else
						brd.LiftZ = liftZ;
					if(style.Color.A == 0)
					{
						LineStyle2D LS = (LineStyle2D)(style.Clone());
						LS.Color = OwningChart.Palette.AxisLineColor;
						brd.DrawLine(LS,P0,P1);
					}
					else
						brd.DrawLine(style,P0,P1);
				}
			}

			// Axis Title
			if(axisTitle != null && axisTitle != "")
			{
				Color oldColor = AxisTitleStyle.ForeColor;
				if(oldColor.A == 0)
					AxisTitleStyle.ForeColor = OwningChart.Palette.CoodinateLabelFontColor;

				double coord = 0;
				double coord0 = Axis.MinValueICS;
				double coord1 = Axis.MaxValueICS;

				switch(axisTitlePositionKind)
				{
					case AxisTitlePositionKind.AtMinimumValue: coord = coord0; break;
					case AxisTitlePositionKind.AtMaximumValue: coord = coord1; break;
					case AxisTitlePositionKind.AtMiddlePoint: coord = (coord0+coord1)*0.5; break;
					case AxisTitlePositionKind.Custom: coord = titleReferencePosition; break;
					default: // = Auto
					switch(AxisTitleStyle.ReferencePoint)
					{
						case TextReferencePoint.Center:
							coord = (coord0+coord1)*0.5;
							break;
						case TextReferencePoint.CenterBottom:
							coord = (coord0+coord1)*0.5;
							break;
						case TextReferencePoint.CenterTop:
							coord = (coord0+coord1)*0.5;
							break;
						case TextReferencePoint.Default:
							coord = (coord0+coord1)*0.5;
							break;
						case TextReferencePoint.LeftBottom:
							coord = coord0;
							break;
						case TextReferencePoint.LeftCenter:
							coord = coord0;
							break;
						case TextReferencePoint.LeftTop:
							coord = coord0;
							break;
						case TextReferencePoint.RightBottom:
							coord = coord1;
							break;
						case TextReferencePoint.RightCenter:
							coord = coord1;
							break;
						case TextReferencePoint.RightTop:
							coord = coord1;
							break;
					}
						break;
				}

				LabelStyle labelStyle = new LabelStyle(AxisTitleStyle);

				if(!axisTitleOffsetPtsIsDefaultZero || axisTitlePositionKind == AxisTitlePositionKind.Auto)
				{
					switch(AxisTitleStyle.ReferencePoint)
					{
						case TextReferencePoint.Center:
							break;
						case TextReferencePoint.CenterBottom:
							labelStyle.VOffset = axisTitleOffsetPts;
							break;
						case TextReferencePoint.CenterTop:
							labelStyle.VOffset = -axisTitleOffsetPts;
							break;
						case TextReferencePoint.Default:
							break;
						case TextReferencePoint.LeftBottom:
							labelStyle.HOffset = axisTitleOffsetPts;
							break;
						case TextReferencePoint.LeftCenter:
							labelStyle.HOffset = axisTitleOffsetPts;
							break;
						case TextReferencePoint.LeftTop:
							labelStyle.VOffset = -AxisTitleStyle.VOffset;
							break;
						case TextReferencePoint.RightBottom:
							labelStyle.HOffset = axisTitleOffsetPts;
							break;
						case TextReferencePoint.RightCenter:
							labelStyle.HOffset = axisTitleOffsetPts;
							break;
						case TextReferencePoint.RightTop:
							labelStyle.HOffset = axisTitleOffsetPts;
							break;
					}
				}
				GE.CreateText(labelStyle,WCSCoordinate(coord),axisTitle);
				
			}
			GE.Pop(typeof(CoordinatePlaneBox));
		}

		private bool PushOwningCoordinatePlaneBox()
		{
			CoordinateSystemBox csb = GE.Top.Owning(typeof(CoordinateSystemBox)) as CoordinateSystemBox;
			if(csb == null)
				return false;
			GeometricObject planes = csb.CoordinatePlanes;
			// --- Test
			TargetCoordinateRange tcr = csb.CoordinateRange(true);
			// --- End
			for(int i = 0; i<planes.SubObjects.Count; i++)
			{
				CoordinatePlaneBox cpb = planes[i] as CoordinatePlaneBox;
				if(cpb == null)
					continue;
				CoordinatePlane plane = cpb.Tag as CoordinatePlane;
				if(this.Plane == plane)
				{
					GE.Push(cpb);
					return true;
				}
			}
			return false;
		}

		internal override void Render()
		{
			if(!Visible)
				return;

			Build();

			if(CoordinateSystem.OwningSeries.Style.IsRadar)
			{
				RenderRadar();
				return;
			}

			if(Plane != null && !Plane.Visible)
				return;

			RenderLinear();
		}
		private double Coordinate2ICS
		{
			get
			{
				switch(positionKind)
				{
					case AxisLinePositionKind.AtMaximumValue:
						return axis2.MaxValueICS;
					case AxisLinePositionKind.AtMinimumValue:
						return axis2.MinValueICS;
					default:
						return coord2;
				}
			}
		}

		private double Coordinate3ICS
		{
			get
			{
				return Plane.ICSOffset;
			}
		}


		#endregion
	
		#region --- Serialization and Browsing Control ---
		private bool ShouldSerializeMe		{ get { return hasChanged; } }

		private bool ShouldSerializeLiftZ() { return LiftZ != 0.1; }

		private bool ShouldBrowseHOffset() { return textReferencePoint != TextReferencePoint.Default; }
		private bool ShouldBrowseVOffset() { return textReferencePoint != TextReferencePoint.Default; }
		private bool ShouldBrowseXAxisCoordinate() { return Axis != CoordinateSystem.XAxis; }
		private bool ShouldBrowseYAxisCoordinate() { return Axis != CoordinateSystem.YAxis; }
		private bool ShouldBrowseZAxisCoordinate() { return Axis != CoordinateSystem.ZAxis; }

		private static string[] PropertiesOrder = new string[]
			{
				"LabelsVisible",
				"LineVisible",
				"Visible",

				"XAxisCoordinate",
				"YAxisCoordinate",
				"ZAxisCoordinate",
				"ReferencePoint",
				"TextOrientation",
				"HOffset",
				"VOffset",
				"LiftZ",

				"FormatString",
				"LabelStyleName",
				"LineStyleName"
			};
		#endregion
	
	}

	internal class AxisAnnotationPlane : ChartObject
	{
		private AxisAnnotation annotation;

		private Vector3D	originWCS;
		private Vector3D	xAxisWCS; // direction of the annotated axis
		private Vector3D	yAxisWCS; // the other direction of the annotation plane
		private Vector3D	txtXDirection;
		private Vector3D	txtYDirection;
		private bool		upperPart;

		private LabelStyle	labelStyle;

		private bool		textParallelToAxis;

		internal AxisAnnotationPlane(AxisAnnotation annotation)
		{
			this.annotation = annotation;
			SetOwner(annotation);

			GetOriginWCS();
			GetDirectionsWCS();
			GetTitleDirections();
		}


		#region --- Creating annotation plane parameters ---
		
		private void GetOriginWCS()
		{
			Axis axis2;
			if(annotation.Plane.XAxis == annotation.Axis)
				axis2 = annotation.Plane.YAxis;
			else
				axis2 = annotation.Plane.XAxis;
		
			// Origin in WCS
			Vector3D vICS = Vector3D.Null;
			switch(annotation.PositionKind)
			{
				case AxisLinePositionKind.Custom:
					vICS = ChartSpace.AxisUnitVector(axis2.Role)*annotation.PositionICS;
					break;
				case AxisLinePositionKind.AtMinimumValue:
					vICS = Vector3D.Null;
					break;
				case AxisLinePositionKind.AtMaximumValue:
					vICS = ChartSpace.AxisUnitVector(axis2.Role)*axis2.MaxValueICS;
					break;
			}
			originWCS = CoordinateSystem.ICS2WCS(vICS);
		}

		private void GetDirectionsWCS()
		{
			// Here we compute annotation plane directions as well as text directions
			// Results are in WCS

			// --- First approximation of text directions ---

			if(annotation.TextOrientation == TextOrientation.UserDefined)
			{
				txtXDirection = GetWCSDirection(Style.HorizontalDirection);
				txtYDirection = GetWCSDirection(Style.VerticalDirection);
			}
			else
			{
				TextOrientation txtOr;
				if(annotation.TextOrientation == TextOrientation.Default)
				{
					// Here the text orientation is in WCS
					switch(GetOptimumTextPlaneWCS())
					{
						case CoordinatePlaneOrientation.XYPlane:
							txtOr = TextOrientation.XYOrientation;
							break;
						case CoordinatePlaneOrientation.YZPlane:
							txtOr = TextOrientation.ZYOrientation;
							break;
						default: // ZXPlane
							txtOr = TextOrientation.XZOrientation;
							break;
					}
				}
				else
					// Here the text orientation is in ICS
					txtOr = annotation.TextOrientation;
				
				Vector3D v1 = Vector3D.Null, v2 = Vector3D.Null;
				switch(txtOr)
				{
					case TextOrientation.XYOrientation:
						v1 = new Vector3D(1,0,0);
						v2 = new Vector3D(0,1,0);
						break;
					case TextOrientation.XZOrientation:
						v1 = new Vector3D(1,0,0);
						v2 = new Vector3D(0,0,-1);
						break;
					case TextOrientation.YXOrientation:
						v1 = new Vector3D(0,1,0);
						v2 = new Vector3D(-1,0,0);
						break;
					case TextOrientation.YZOrientation:
						v1 = new Vector3D(0,1,0);
						v2 = new Vector3D(0,0,1);
						break;
					case TextOrientation.ZXOrientation:
						v1 = new Vector3D(0,0,1);
						v2 = new Vector3D(1,0,0);
						break;
					case TextOrientation.ZYOrientation:
						v1 = new Vector3D(0,0,-1);
						v2 = new Vector3D(0,1,0);
						break;
					default:
						break;
				}
				// Fix orientation if it is in ICS
				if(annotation.TextOrientation != TextOrientation.Default)
				{
					v1 = GetWCSDirection(v1);
					v2 = GetWCSDirection(v2);
				}
				txtXDirection = v1;
				txtYDirection = v2;
			}

			// --- Plane directions ---
			
			xAxisWCS = annotation.Axis.UnitVector;

			// The yAxisWCS is the text direction vector which is "less collinear" to the xAxisWCS.
			// Besides, we make sure that text directions comply with ann. plane directions
			if(Math.Abs(txtXDirection*xAxisWCS) > Math.Abs(txtYDirection*xAxisWCS))
			{
				yAxisWCS = txtYDirection;
				txtXDirection = xAxisWCS; // to make sure that text directions comply with the plane
				textParallelToAxis = true;
			}
			else
			{
				yAxisWCS = txtXDirection;
				txtYDirection = xAxisWCS; // same as above
				textParallelToAxis = false;
			}
			// Just in case, use the orthogonal component
			yAxisWCS = (yAxisWCS - xAxisWCS*(xAxisWCS*yAxisWCS)).Unit();

			// Turn the positive side of the ann plane towards the viewer

			if(xAxisWCS.CrossProduct(yAxisWCS).Z < 0)
			{
				yAxisWCS = -yAxisWCS;
			}

			// Compute the side of the ann semi-plane to respect "S-pattern":
			//          +---->
			//			|
			//			|  
			//			|   \
			//	   ---->+    \ (view direction)
			//				  \
			Axis axis2;
			if(Axis == Plane.XAxis)
				axis2 = Plane.YAxis;
			else
				axis2 = Plane.XAxis;

			Vector3D a2TCS = GetTCSDirection(axis2.UnitVector);
			Vector3D yAxisTCS = GetTCSDirection(yAxisWCS);
			a2TCS.Z = 0;
			yAxisTCS.Z = 0;
			
			double sp = a2TCS*yAxisTCS;	//	axis2.UnitVector*yAxisWCS;
			if(annotation.PositionKind == AxisLinePositionKind.AtMinimumValue)
				upperPart = (sp<0);
			else
				upperPart = (sp>0);
		}

		internal override void Render()
		{
		}

		private Vector3D GetWCSDirection(Vector3D v)
		{
			// Direction of the vector in the WCS
			CoordinateSystem sys = CoordinateSystem;
			Vector3D d = sys.ICS2WCS(v) - sys.ICS2WCS(Vector3D.Null);
			if(!d.IsNull)
				return d.Unit();
			else
				return d;
		}

		#endregion

		#region --- Adjusting axis label style ---

		internal void AdjustAxisLabelStyle(LabelStyle eStyle, double rotationAngle)
		{
			eStyle.HorizontalDirection = txtXDirection;
			eStyle.VerticalDirection = txtYDirection;
			eStyle.Angle = rotationAngle;

			if(eStyle.ReferencePoint == TextReferencePoint.Default)
			{
				if(textParallelToAxis)
				{
					if(upperPart)
						eStyle.ReferencePoint = TextReferencePoint.CenterBottom;
					else
						eStyle.ReferencePoint = TextReferencePoint.CenterTop;
				}
				else
				{
					if(upperPart)
					{
						if(txtXDirection*yAxisWCS > 0) 
							eStyle.ReferencePoint = TextReferencePoint.LeftCenter;
						else
							eStyle.ReferencePoint = TextReferencePoint.RightCenter;
					}
					else
					{
						if(txtXDirection*yAxisWCS < 0) 
							eStyle.ReferencePoint = TextReferencePoint.LeftCenter;
						else
							eStyle.ReferencePoint = TextReferencePoint.RightCenter;
					}
				}
			}

			ModifyRefPointDueToAngle(eStyle,rotationAngle);
			eStyle.LockDirectionAndSideMode(Mapping, GetAxisMidPointWCS(),eStyle.HorizontalDirection,eStyle.VerticalDirection, true,true);
			GetEffectiveOffsets(eStyle);
		}

		private void GetEffectiveOffsets(LabelStyle eStyle)
		{
			double h = 0.0, v = 0.0;
			double hOffset = annotation.HOffset;
			double vOffset = annotation.VOffset;
			switch(eStyle.ReferencePoint)
			{
				case TextReferencePoint.LeftBottom:
					h = hOffset;
					v = vOffset;
					break;
				case TextReferencePoint.LeftCenter:
					h = hOffset;
					break;
				case TextReferencePoint.LeftTop:
					h = hOffset;
					v = -vOffset;
					break;
				case TextReferencePoint.CenterBottom:
					v = vOffset;
					break;
				case TextReferencePoint.Center:
					break;
				case TextReferencePoint.CenterTop:
					v = -vOffset;
					break;
				case TextReferencePoint.RightBottom:
					h = -hOffset;
					v = vOffset;
					break;
				case TextReferencePoint.RightCenter:
					h = -hOffset;
					break;
				case TextReferencePoint.RightTop:
					h = -hOffset;
					v = -vOffset;
					break;
				default:
					break;
			}
			if(eStyle.DefaultHOffset)
				eStyle.HOffset = h;
			if(eStyle.DefaultVOffset)
				eStyle.VOffset = v;
		}

		#endregion

		#region --- Creating Title Directions ---

		private void GetTitleDirections()
		{
			LabelStyle titleStyle = annotation.AxisTitleStyle;
			if(titleStyle.ReferencePoint == TextReferencePoint.Default)
			{
				titleStyle.HorizontalDirection = xAxisWCS;
				titleStyle.VerticalDirection = yAxisWCS;

				if(upperPart)
					titleStyle.ReferencePoint = TextReferencePoint.CenterBottom;
				else
					titleStyle.ReferencePoint = TextReferencePoint.CenterTop;
			}
		}

		#endregion
		
		#region --- Private Properties ---

		private LabelStyle Style 
		{
			get
			{
				if(labelStyle != null)
					return labelStyle;
				LabelStyle ls = OwningChart.LabelStyles[annotation.LabelStyleName];
				labelStyle = new LabelStyle();
				if(ls != null)
					labelStyle.LoadFrom(ls);
				return labelStyle;
			}
		}

		private CoordinateSystem CoordinateSystem { get { return annotation.CoordinateSystem; } }
		private Mapping Mapping { get { return CoordinateSystem.TargetArea.Mapping; } }
		private CoordinatePlane Plane { get { return annotation.Plane; } }
		private Axis Axis { get { return annotation.Axis; } }

		#endregion

		#region --- Helpers ---
		
		private void ModifyRefPointDueToAngle(LabelStyle eStyle, double angDegrees)
		{
			while(angDegrees < 0)
				angDegrees += 360;
			while(angDegrees > 360)
				angDegrees -= 360;

			double tol = 10; // tolerance in degrees

			TextReferencePoint rp = eStyle.ReferencePoint;
			while(angDegrees >= 0)
			{
				switch(rp)
				{
					case TextReferencePoint.Center:
						return;
					case TextReferencePoint.LeftCenter:
						if(angDegrees < tol)
							break;
						rp = TextReferencePoint.LeftTop;
						break;
					case TextReferencePoint.LeftTop:
						if(angDegrees < 45-tol)
							break;
						rp = TextReferencePoint.CenterTop;
						break;
					case TextReferencePoint.CenterTop:
						if(angDegrees < tol)
							break;
						rp = TextReferencePoint.RightTop;
						break;
					case TextReferencePoint.RightTop:
						if(angDegrees < 45-tol)
							break;
						rp = TextReferencePoint.RightCenter;
						break;
					case TextReferencePoint.RightCenter:
						if(angDegrees < tol)
							break;
						rp = TextReferencePoint.RightBottom;
						break;
					case TextReferencePoint.RightBottom:
						if(angDegrees < 45-tol)
							break;
						rp = TextReferencePoint.CenterBottom;
						break;
					case TextReferencePoint.CenterBottom:
						if(angDegrees < tol)
							break;
						rp = TextReferencePoint.LeftBottom;
						break;
					case TextReferencePoint.LeftBottom:
						if(angDegrees < 45-tol)
							break;
						rp = TextReferencePoint.LeftCenter;
						break;
					default:
						break;
				}
				angDegrees -= 45;
			}

			eStyle.ReferencePoint = rp;
		}

		private CoordinatePlaneOrientation GetOptimumTextPlaneWCS()
		{
			Vector3D P = GetAxisMidPointWCS();

			// returns the plane in the owning coordinate system that has minimum text distortion at point P 
			// due to the projection and the view point position

			CoordinateSystem cs = this.CoordinateSystem;
			Vector3D v0 = Mapping.Map(P);
			CoordinatePlane[] planes = new CoordinatePlane[] { cs.PlaneXY, cs.PlaneYZ, cs.PlaneZX };

			CoordinatePlane plane = null;
			double p = -1;
            Mapping map = Axis.CoordSystem.TargetArea.Mapping;
			foreach(CoordinatePlane pl in planes)
			{
				if(pl.XAxis == this.Axis || pl.YAxis == this.Axis)
				{
					Vector3D vx = map.Map(P + pl.XAxis.UnitVector) - v0;
					Vector3D vy = map.Map(P + pl.YAxis.UnitVector) - v0;
					// We are interested only in x-y coordinates:
					vx.Z = 0;
					vy.Z = 0;
					double p1 = (vx.CrossProduct(vy)).Abs;
					if(p1 > p)
					{
						p = p1;
						plane = pl;
					}
				}
			}
			
			return plane.WCSOrientation;
		}
		
		private Vector3D GetAxisMidPointWCS()
		{
			Vector3D avgLabelPosition = Axis.UnitVector*(Axis.MaxValueICS/2);
			Axis axis2 = null;
			if(Axis == Plane.XAxis)
				axis2 = Plane.YAxis;
			else
				axis2 = Plane.XAxis;
			if(annotation.PositionKind == AxisLinePositionKind.AtMaximumValue)
				avgLabelPosition = avgLabelPosition + axis2.UnitVector*(axis2.MaxValueICS);
			return avgLabelPosition;
		}
		
		private Vector3D GetTCSDirection(Vector3D vectorWCS) // Target CS direction
		{
			Vector3D midAxisWCS = GetAxisMidPointWCS();
			Vector3D dir = Mapping.Map(vectorWCS + midAxisWCS) - Mapping.Map(midAxisWCS);
			if(!dir.IsNull)
				return dir.Unit();
			else
				return dir;
		}
		#endregion
	}
}
