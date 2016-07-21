using System;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Xml;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Specifies the position of the label in a pie or doughnut chart.
	/// </summary>
	public enum PieLabelPositionKind
	{
		/// <summary>
		/// All labels are perpendicular to the tangent at center of the doughnut segment.
		/// </summary>
		InsideRadial,
		/// <summary>
		/// All labels are parallel to the tangent at center of the doughnut segment.
		/// </summary>
		InsideCircular,
		/// <summary>
		/// All labels are placed horizontally in the middle of segments.
		/// </summary>
		InsideHorizontal,
		/// <summary>
		/// All labels are placed horizontally at a specified distance outside the segment.
		/// </summary>
		Outside,
		/// <summary>
		/// All labels are placed horizontally outside the segment and are aligned with other labels.
		/// </summary>
		OutsideAligned,
		/// <summary>
		/// All labels are not visible.
		/// </summary>
		NoLabel
	}

	
	/// <summary>
	/// Specifies the position of the label in a bar, line or point series.
	/// </summary>
	public enum DataPointLabelStyleKind
	{
		/// <summary>
		/// Deefault label style. It is interpreted as InsideRadial for pie/doughnut series styles
		/// and CenterVertical for other series styles. 
		/// </summary>
		Default,
		/// <summary>
		/// The label is located at the top of the bar's front face and is vertical.
		/// </summary>
		TopVertical,		//	Bar
		/// <summary>
		/// The label is located at the middle of the bar's front face and is vertical.
		/// </summary>
		CenterVertical,		//	Bar
		/// <summary>
		/// The label is located at the bottom of the bar's front face  and is vertical.
		/// </summary>
		BottomVertical,		//	Bar
		/// <summary>
		/// The label is located at the top of the bar's front face and is horizontal.
		/// </summary>
		TopHorizontal,		//	Bar
		/// <summary>
		/// The label is located at the middle of the bar's front face and is horizontal.
		/// </summary>
		CenterHorizontal,	//	Bar
		/// <summary>
		/// The label is located at the bottom of the bar's front face and is horizontal.
		/// </summary>
		BottomHorizontal,	//	Bar
		/// <summary>
		/// The label is located above the value and is vertical.
		/// </summary>
		AboveVertical,		//	Bar,Line,Point
		/// <summary>
		/// The label is located above the value and is horizontal.
		/// </summary>
		AboveHorizontal,	//	Bar,Line,Point
		/// <summary>
		/// The label is located below the value and is vertical.
		/// </summary>
		BelowVertical,		//	Bar,Line,Point
		/// <summary>
		/// The label is located below the value and is horizontal.
		/// </summary>
		BelowHorizontal,	//	Bar,Line,Point
		/// <summary>
		/// All labels are perpendicular to the tangent at center of the doughnut segment.
		/// </summary>
		InsideRadial,
		/// <summary>
		/// All labels are parallel to the tangent at center of the doughnut segment.
		/// </summary>
		InsideCircular,
		/// <summary>
		/// All labels are placed horizontally in the middle of segments.
		/// </summary>
		InsideHorizontal,
		/// <summary>
		/// All labels are placed horizontally at a specified distance outside the segment.
		/// </summary>
		Outside,
		/// <summary>
		/// All labels are placed horizontally outside the segment and are aligned with other labels.
		/// </summary>
		OutsideAligned,
		/// <summary>
		/// The label specified by LocalRefPoint property of the <see cref="DataPointLabelStyle"/>.
		/// </summary>
		Custom
	}

	/// <summary>
	/// Specifies the position of the label in a bar, line or point series.
	/// </summary>
	public enum LabelPositionKind
	{
		/// <summary>
		/// The label is located at the top of the bar's front face and is vertical.
		/// </summary>
		TopVertical,		//	Bar
		/// <summary>
		/// The label is located at the middle of the bar's front face and is vertical.
		/// </summary>
		CenterVertical,		//	Bar
		/// <summary>
		/// The label is located at the bottom of the bar's front face  and is vertical.
		/// </summary>
		BottomVertical,		//	Bar
		/// <summary>
		/// The label is located at the top of the bar's front face and is horizontal.
		/// </summary>
		TopHorizontal,		//	Bar
		/// <summary>
		/// The label is located at the middle of the bar's front face and is horizontal.
		/// </summary>
		CenterHorizontal,	//	Bar
		/// <summary>
		/// The label is located at the bottom of the bar's front face and is horizontal.
		/// </summary>
		BottomHorizontal,	//	Bar
		/// <summary>
		/// The label is located above the value and is vertical.
		/// </summary>
		AboveVertical,		//	Bar,Line,Point
		/// <summary>
		/// The label is located above the value and is horizontal.
		/// </summary>
		AboveHorizontal,	//	Bar,Line,Point
		/// <summary>
		/// The label is located below the value and is vertical.
		/// </summary>
		BelowVertical,		//	Bar,Line,Point
		/// <summary>
		/// The label is located below the value and is horizontal.
		/// </summary>
		BelowHorizontal,	//	Bar,Line,Point
		/// <summary>
		/// The label specified by LocalRefPoint property of the <see cref="DataPointLabelStyle"/>.
		/// </summary>
		Custom
	}

	/// <summary>
	/// Specifies the type of chart the <see cref="DataPointLabelStyle"/> object is applied to.
	/// </summary>
	public enum DataPointLabelKind
	{
		/// <summary>
		/// <see cref="DataPointLabelStyle"/> object will be applied to a bar series.
		/// </summary>
		BarShape, 
		/// <summary>
		/// <see cref="DataPointLabelStyle"/> object will be applied to a line series.
		/// </summary>
		LineShape,
		/// <summary>
		/// <see cref="DataPointLabelStyle"/> object will be applied to a pie or doughnut chart.
		/// </summary>
		PieDoughnutShape
	} 

	/// <summary>
	/// Defines an object used to draw data point labels.
	/// </summary>

	// ================================================================================================
	//		DataPointLabelStyle
	// ================================================================================================

	/// <summary>
	///		Describes how data point labels are drawn.
	/// </summary>
	/// <remarks>
	///   <para>
	///     <see cref="DataPointLabelStyle"/> objects are stored in the <see cref="DataPointLabelStyleCollection"/> 
	///     property "DataPointLabelStyles" of the charting control. 
	///     The control has many predefined data point label styles. They can be used as defined by 
	///     default or modified by user code.  Besides, the styles architecture 
	///     is extendible. User can define his/her own style and store it in the "DataPointLabelStyles"
	///     collection. 
	///   </para>
	///   <para>
	///     <see cref="DataPointLabelStyle.LabelPosition"/> defines where the label is to be placed relative to the
	///     owning data point. It also defines text orientation, except when LabelPosition = 
	///     <see cref="LabelPositionKind.Custom"/>. In that case,  <see cref="DataPointLabelStyle.HorizontalDirection"/>
	///     and <see cref="DataPointLabelStyle.VerticalDirection"/> vectors define text direction.
	///   </para>
	///   <para>
	///     A special set od properties describes labels drawing for pies and doughnuts. 
	///     <see cref="DataPointLabelStyle.PieLabelPosition"/> specifies position of the label. If the label
	///     is outside or outside aligned, then a line is drawn from pie/doughnut segment towards the
	///     label. <see cref="DataPointLabelStyle.LineStyle"/> is the name of <see cref="LineStyle2D"/> 
	///     used to draw the line. <see cref="DataPointLabelStyle.RelativeLine1Start"/> is the value specifying
	///     where the line starts, between the pie center and perimeter. Value 0 denotes the center, value 1
	///     denotes point of the perimeter. Usual values are between these two extremes. 
	///     <see cref="DataPointLabelStyle.RelativeLine1Length"/> is the length of the line in units
	///     of radius length. If this value is 1, than the length of line is equal to the pie/doughnut radius.
	///     The line ahs second segment, which is always horizontal. <see cref="DataPointLabelStyle.RelativeLine2Length"/>
	///     specifies the length of this line in the same units. This value is ignored if labels are outside aligned.
	///   </para>
	/// </remarks>
	
	[System.ComponentModel.TypeConverter(typeof(DataPointLabelStyleConverter))]
	public class DataPointLabelStyle : LabelStyle
	{
		DataPointLabelKind labelKind = DataPointLabelKind.BarShape;
		// Linear shapes label specifics
		Vector3D	localRefPoint;
		// Pie label specifics
		PieLabelPositionKind	pieLabelPosition;
		string		lineStyle;
		double		relativeLine1Start;
		double		relativeLine1Length;
		double		relativeLine2Length;
		double		pixelsToLabel;
		double		relativeOffsetOfAlignedLabels;
		
		LabelPositionKind labelPosition = LabelPositionKind.CenterVertical;

		// fixme: correct this
		#region --- Pie Label Style Constructors ---
		/// <summary>
		/// Initializes a new instance of the <see cref="DataPointLabelStyle"/> class with a name, pie label position.
		/// </summary>
		/// <param name="name">The name of this <see cref="DataPointLabelStyle"/> object.</param>
		/// <param name="pieLabelPosition">The position of the label in a pie/doughnut series. This value is stored in the <see cref="DataPointLabelStyle.PieLabelPosition"/> property.</param>
		/// <param name="lineStyle">The name of the 2D line style used for label line. This value is stored in the <see cref="DataPointLabelStyle.LineStyle"/> property.</param>
		/// <param name="relativeLine1Start">A value that indicates where the first line segment starts. This value is stored in the <see cref="DataPointLabelStyle.RelativeLine1Start"/> property.</param>
		/// <param name="relativeLine1Length">A value that indicates how long the first line segment lasts. This value is stored in the <see cref="DataPointLabelStyle.RelativeLine1Length"/> property.</param>
		/// <param name="relativeLine2Length">A value that indicates how long the second line segment lasts. This value is stored in the <see cref="DataPointLabelStyle.RelativeLine1Length"/> property.</param>
		/// <param name="pixelsToLabel">The distance between line and label. This value is stored in the <see cref="DataPointLabelStyle.PixelsToLabel"/> property.</param>
		/// <param name="relativeOffsetOfAlignedLabels">Obsolete</param>
		public DataPointLabelStyle(string name,  PieLabelPositionKind pieLabelPosition, string lineStyle,
			double relativeLine1Start, double relativeLine1Length, double relativeLine2Length,
			double pixelsToLabel, double relativeOffsetOfAlignedLabels) : base (name)
		{
			SetDefaults();
            this.Orientation = TextOrientation.YXOrientation;
			this.pieLabelPosition = pieLabelPosition;
			this.lineStyle = lineStyle;
			this.relativeLine1Start = relativeLine1Start;
			this.relativeLine1Length = relativeLine1Length;
			this.relativeLine2Length = relativeLine2Length;
			this.pixelsToLabel = pixelsToLabel;
			this.relativeOffsetOfAlignedLabels = relativeOffsetOfAlignedLabels;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataPointLabelStyle"/> class with a name and a local reference point.
		/// </summary>
		/// <param name="name">The name of this <see cref="DataPointLabelStyle"/> object.</param>
		/// <param name="localRefPoint">A <see cref="Vector3D"/> structure that specifies a local reference point.</param>
		public DataPointLabelStyle(string name, Vector3D localRefPoint) 
			: base (name)
		{
			SetDefaults();
			this.localRefPoint = localRefPoint;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataPointLabelStyle"/> class with a specified name.
		/// </summary>
		/// <param name="name">The name of this <see cref="DataPointLabelStyle"/> object.</param>
		public DataPointLabelStyle(string name)
			: base (name)
		{
			SetDefaults();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataPointLabelStyle"/> class.
		/// </summary>
		public DataPointLabelStyle()
            : base("")
        {
            SetDefaults();
        }

		private void SetDefaults()
		{
			// Linear style defaults
			localRefPoint = new Vector3D(0.5,0.5,1.01);
			HorizontalDirection = new Vector3D(0,1,0);
			VerticalDirection = new Vector3D(-1,0,0);

			ReferencePoint = TextReferencePoint.Center;
			Orientation = TextOrientation.YXOrientation;

			// Pie label style defaults
			pieLabelPosition = PieLabelPositionKind.InsideRadial;
			lineStyle = "Default";
			relativeLine1Start = 0.8;
			relativeLine1Length = 0.3;
			relativeLine2Length = 0.1;
			relativeOffsetOfAlignedLabels = 0.0;
			pixelsToLabel = 2;

			// Font
			Font = new Font("Arial",12f);

			// LiftZ
			LiftZ = 0.5;
		}

		internal void LoadFrom(DataPointLabelStyle s)
		{
			base.LoadFrom(s);
			labelKind = s.labelKind;
			localRefPoint = s.localRefPoint;
			pieLabelPosition = s.pieLabelPosition;
			lineStyle = s.lineStyle;
			relativeLine1Start = s.relativeLine1Start;
			relativeLine1Length = s.relativeLine1Length;
			relativeLine2Length = s.relativeLine2Length;
			pixelsToLabel = s.pixelsToLabel;
			relativeOffsetOfAlignedLabels = s.relativeOffsetOfAlignedLabels;
            Orientation = s.Orientation;
            SetOwningChart(s.OwningChart);
		}

		#endregion
		
		#region --- Style Kind Handling ---
		
		internal static DataPointLabelStyleKind StyleKindOf(string styleName)
		{
			try
			{
				return (DataPointLabelStyleKind)Enum.Parse(typeof(DataPointLabelStyleKind),styleName,true);
			}
			catch
			{
				return DataPointLabelStyleKind.Custom;
			}
		}

		/// <summary>
		/// Gets style kind of this style object.
		/// </summary>
		public DataPointLabelStyleKind StyleKind
		{
			get
			{
				return StyleKindOf(Name);
			}
		}
		#endregion
		#region --- Properties ---

		[DefaultValue(typeof(Vector3D), "(-1,0,0)")]
		public override Vector3D VerticalDirection 
		{
			get {return base.VerticalDirection;}
			set {base.VerticalDirection = value;}
		}

		[DefaultValue(typeof(Vector3D), "(0,1,0)")]
		public override Vector3D HorizontalDirection 
		{
			get {return base.HorizontalDirection;}
			set {base.HorizontalDirection = value;}
		}

		[DefaultValue(TextReferencePoint.Center)]
		public override TextReferencePoint		ReferencePoint		
		{
			get {return base.ReferencePoint;}
			set {base.ReferencePoint = value;}
		}

		[DefaultValue(TextOrientation.YXOrientation)]
		public override TextOrientation			Orientation			
		{
			get { return base.Orientation; } 
			set { base.Orientation = value; } 
		}


		/// <summary>
		/// Gets of sets a label position of this <see cref="DataPointLabelStyle"/>.
		/// </summary>
		[DefaultValue(LabelPositionKind.CenterVertical)]
		[Description("Label position"), Category("General")]
		[RefreshProperties(RefreshProperties.All)]
		public LabelPositionKind LabelPosition { get { return labelPosition; } set { if(labelPosition != value) hasChanged = true; labelPosition = value; AdjustPositionParameters(); } }

		#region --- Category: General ---
		/// <summary>
		/// Gets of sets the kind of this <see cref="DataPointLabelStyle"/>.
		/// </summary>
		[DefaultValue(DataPointLabelKind.BarShape)]
		[Description("Data point category"), Category("General")]
		[RefreshProperties(RefreshProperties.All)]
		public DataPointLabelKind DataPointLabelKind { get { return labelKind; } set { if(labelKind != value) hasChanged = true; labelKind = value; } }
		/// <summary>
		/// Gets or sets the local reference point of this <see cref="DataPointLabelStyle"/>.
		/// </summary>
		[DefaultValue(typeof(Vector3D), "(0.5,0.5,1.01)")]
		[ Description("Reference point of label in relative item coordinates"), Category("General") ]
		public Vector3D LocalRefPoint				{ get { return localRefPoint; }	set { if(localRefPoint != value) hasChanged = true; localRefPoint = value; } }
		#endregion
		
		#region --- Category: Pie Specific Style ---
		/// <summary>
		/// Gets or sets the label position of the pie in this <see cref="DataPointLabelStyle"/>.
		/// </summary>
		[DefaultValue(PieLabelPositionKind.InsideRadial)]
		[ Description("Label position within pie slice"), Category("Pie Specific Style") ]
		[RefreshProperties(RefreshProperties.All)]
		public PieLabelPositionKind	PieLabelPosition	{ get { return pieLabelPosition; }				set { if(pieLabelPosition != value) hasChanged = true; pieLabelPosition = value; } }
		/// <summary>
		/// Gets or sets the line style in this <see cref="DataPointLabelStyle"/>.
		/// </summary>
		[DefaultValue("Default")]
		[ TypeConverter(typeof(SelectedLineStyle2DConverter))]
		[ Description("Line style for external labels"), Category("Pie Specific Style") ]
		public string LineStyle							{ get { return lineStyle; }						set { if(lineStyle != value) hasChanged = true; lineStyle = value; } }
		/// <summary>
		/// Gets or sets a value that indicates where the first line segment starts.
		/// </summary>
		[DefaultValue(0.8)]
		[ Description("Relative starting point of the first line segment (between 0 and 2)"), Category("Pie Specific Style") ]
		public double RelativeLine1Start				{ get { return relativeLine1Start; }			
			set 
			{
				if (value < 0 || 2 < value) 
					throw new ArgumentException("Relative starting point of the first line segment must be between 0 and 2");
				if(relativeLine1Start != value) hasChanged = true; 
				relativeLine1Start = value; 
			} 
		}

		/// <summary>
		/// Gets or sets a value that indicates how long the first line segment lasts.
		/// </summary>
		[DefaultValue(0.3)]
		[ Description("Relative length of the first line segment"), Category("Pie Specific Style") ]
		public double RelativeLine1Length				{ get { return relativeLine1Length; }			set { if(relativeLine1Length != value) hasChanged = true; relativeLine1Length = value; } }
		/// <summary>
		/// Gets or sets a value that indicates how long the first second segment lasts.
		/// </summary>
		[DefaultValue(0.1)]
		[ Description("Relative length of the second line segment"), Category("Pie Specific Style") ]
		public double RelativeLine2Length				{ get { return relativeLine2Length; }			set { if(relativeLine2Length != value) hasChanged = true; relativeLine2Length = value; } }
		/// <summary>
		/// Gets or sets the distance between line and label.
		/// </summary>
		[DefaultValue((double) 2)]
		[ Description("Distance between line and label"), Category("Pie Specific Style") ]
		public double PixelsToLabel						{ get { return pixelsToLabel; }					set { if(pixelsToLabel != value) hasChanged = true; pixelsToLabel = value; } }

		/// <summary>
		/// This property is obsolete
		/// </summary>
		[DefaultValue(0.0)]
		[ Description("obsolete"), Category("Pie Specific Style") ]
		public double RelativeOffsetOfAlignedLabels		{ get { return relativeOffsetOfAlignedLabels; }	set { if(relativeOffsetOfAlignedLabels != value) hasChanged = true; relativeOffsetOfAlignedLabels = value; } }
		#endregion
		#endregion

		#region --- Serialization and Browsing Control ---

#if false
		internal void  ResetPieLabelPosition() {pieLabelPosition=PieLabelPositionKind.InsideRadial;}
		internal void  ResetLineStyle() {lineStyle="Default";}
		internal void  ResetRelativeLine1Start() {relativeLine1Start=0.8;}
		internal void  ResetRelativeLine1Length() {relativeLine1Length=0.3;}
		internal void  ResetRelativeLine2Length() {relativeLine2Length=0.1;}
		internal void  ResetPixelsToLabel() {pixelsToLabel=2;}
		internal void  ResetRelativeOffsetOfAlignedLabels() {relativeOffsetOfAlignedLabels=0.0;}
		internal void  ResetLocalRefPoint() {localRefPoint=new Vector3D(0.5,0.5,1.01);}
		/*
		internal override void ResetHorizontalDirection() {HorizontalDirection=new Vector3D(0,1,0);}
		internal override void ResetVerticalDirection() {VerticalDirection=new Vector3D(-1,0,0);}
		*/

		internal bool  ShouldSerializePieLabelPosition() {return pieLabelPosition!=PieLabelPositionKind.InsideRadial;}
		internal bool  ShouldSerializeLineStyle() {return lineStyle!="Default";}
		internal bool  ShouldSerializeRelativeLine1Start() {return Math.Abs(relativeLine1Start-0.8)>0.00001;}
		internal bool  ShouldSerializeRelativeLine1Length() {return Math.Abs(relativeLine1Length-0.3)>0.00001;}
		internal bool  ShouldSerializeRelativeLine2Length() {return Math.Abs(relativeLine2Length-0.1)>0.00001;}
		internal bool  ShouldSerializePixelsToLabel() {return pixelsToLabel!=2;}
		internal bool  ShouldSerializeRelativeOffsetOfAlignedLabels() {return relativeOffsetOfAlignedLabels!=0.0;}
		internal bool  ShouldSerializeLocalRefPoint() {return localRefPoint!=new Vector3D(0.5,0.5,1.01);}
		/*
		internal override bool ShouldSerializeHorizontalDirection() {return HorizontalDirection!=new Vector3D(0,1,0);}
		internal override bool ShouldSerializeVerticalDirection() {return VerticalDirection!=new Vector3D(-1,0,0);}
		*/
#endif

		private bool ShouldBrowseLabelPosition()		{ return labelKind == DataPointLabelKind.BarShape || labelKind == DataPointLabelKind.LineShape; }
		private bool ShouldBrowseOrientation()			{ return (labelKind == DataPointLabelKind.BarShape || labelKind == DataPointLabelKind.LineShape) && LabelPosition == LabelPositionKind.Custom; }
		private bool ShouldBrowseReferencePoint()		{ return ShouldBrowseOrientation(); }
		private bool ShouldBrowseHOffset()				{ return ShouldBrowseOrientation(); }
		private bool ShouldBrowseVOffset()				{ return ShouldBrowseOrientation(); }
		private bool ShouldBrowseLiftZ()				{ return ShouldBrowseOrientation(); }
		private bool ShouldBrowseLocalRefPoint()		{ return ShouldBrowseOrientation(); }

		private bool ShouldBrowseVerticalDirection()			{ return Orientation == TextOrientation.UserDefined; }
		private bool ShouldBrowseHorizontalDirection()			{ return Orientation == TextOrientation.UserDefined; }
		private bool ShouldBrowsePieLabelPosition()				{ return labelKind == DataPointLabelKind.PieDoughnutShape; }
		private bool ShouldBrowseLineStyle()					{ return labelKind == DataPointLabelKind.PieDoughnutShape; }
		private bool ShouldBrowseRelativeLine1Start()			{ return labelKind == DataPointLabelKind.PieDoughnutShape && (pieLabelPosition == PieLabelPositionKind.Outside || pieLabelPosition == PieLabelPositionKind.OutsideAligned); }
		private bool ShouldBrowseRelativeLine1Length()			{ return ShouldBrowseRelativeLine1Start(); }
		private bool ShouldBrowseRelativeLine2Length()			{ return ShouldBrowseRelativeLine1Start(); }
		private bool ShouldBrowsePixelsToLabel()				{ return ShouldBrowseRelativeLine1Start(); }
		private bool ShouldBrowseRelativeOffsetOfAlignedLabels(){ return ShouldBrowseRelativeLine1Start(); }

		private static string[] PropertiesOrder = new string[] 
			{
				"Orientation","HorizontalDirection","VerticalDirection",
				"ReferencePoint","HOffset","VOffset","Angle","LiftZ",
				"PieLabelPosition","LineStyle","RelativeLine1Start","RelativeLine1Length","RelativeLine2Length",
				"PixelsToLabel","RelativeOffsetToAlignedLabel",
				"Name",
				"DataPointLabelKind",
				"LocalRefPoint",
				"Font",
				"ForeColor",
				"ShadowColor",
				"ShadowDepthPxl",
				"RevDirectionStyle",
				"RevSideStyle",
				"Is2D"
			};
		#endregion

		#region --- XML Serialization ---

		internal void Serialize(XmlCustomSerializer S)
		{
			base.Serialize(S);
			S.AttributeProperty(this,"PieLabelPosition");
			S.AttributeProperty(this,"LineStyle");
			S.AttributeProperty(this,"RelativeLine1Start","RelativeLine1Start","0.0##");
			S.AttributeProperty(this,"RelativeLine1Length","RelativeLine1Length","0.0##");
			S.AttributeProperty(this,"RelativeLine2Length","RelativeLine2Length","0.0##");
			S.AttributeProperty(this,"PixelsToLabel");
			S.AttributeProperty(this,"RelativeOffsetOfAlignedLabels");
		}

		internal void CreateDOMAttributes(XmlElement root)
		{
			TypeConverter pieLabelPositionConverter = TypeDescriptor.GetConverter(typeof(PieLabelPositionKind));
			
			base.CreateDOMAttributes(root);

			root.SetAttribute("PieLabelPosition",pieLabelPositionConverter.ConvertToString(PieLabelPosition));
			root.SetAttribute("LineStyle",LineStyle);

			root.SetAttribute("RelativeLine1Start",RelativeLine1Start.ToString());
			root.SetAttribute("RelativeLine1Length",RelativeLine1Length.ToString());
			root.SetAttribute("RelativeLine2Length",RelativeLine2Length.ToString());
			root.SetAttribute("PixelsToLabel",PixelsToLabel.ToString());
			root.SetAttribute("RelativeOffsetOfAlignedLabels",RelativeOffsetOfAlignedLabels.ToString());
		}

		internal void CreateDOM(XmlElement parent)
		{
			XmlDocument doc = parent.OwnerDocument;
			XmlElement root = doc.CreateElement("DataPointLabelStyle");
			CreateDOMAttributes(root);
			parent.AppendChild(root);
		}

		internal static void CreatePropertiesFromDOM(XmlElement root,DataPointLabelStyle S)
		{
			TypeConverter pieLabelPositionConverter = TypeDescriptor.GetConverter(typeof(PieLabelPositionKind));

			LabelStyle.CreatePropertiesFromDOM(root,S);

			S.PieLabelPosition = (PieLabelPositionKind)pieLabelPositionConverter.ConvertFromString(root.GetAttribute("PieLabelPosition"));
			S.LineStyle = root.GetAttribute("LineStyle");

			S.RelativeLine1Start = double.Parse(root.GetAttribute("RelativeLine1Start"));
			S.RelativeLine1Length = double.Parse(root.GetAttribute("RelativeLine1Length"));
			S.RelativeLine2Length = double.Parse(root.GetAttribute("RelativeLine2Length"));
			S.PixelsToLabel = double.Parse(root.GetAttribute("PixelsToLabel"));
			S.RelativeOffsetOfAlignedLabels = double.Parse(root.GetAttribute("RelativeOffsetOfAlignedLabels"));
		}

		internal static DataPointLabelStyle CreateFromDOM(XmlElement root)
		{
			if(root.Name.ToLower() != "datapointlabelstyle")
				return null;
			DataPointLabelStyle S = new DataPointLabelStyle("");
			CreatePropertiesFromDOM(root,S);
			return S;
		}
		#endregion

		#region --- Helpers ---
		private void AdjustPositionParameters()
		{
			double offsetSize = 3;
			if(labelPosition != LabelPositionKind.Custom)
			{
				localRefPoint.X = 0.5;
				localRefPoint.Z = 1.0;
			}
			switch(labelPosition)
			{
				case LabelPositionKind.TopVertical:
					Orientation = TextOrientation.YXOrientation;
					ReferencePoint = TextReferencePoint.RightCenter;
					HOffset = -offsetSize;
					VOffset = 0;
					localRefPoint.Y = 1.0;
					break;
				case LabelPositionKind.CenterVertical:
					Orientation = TextOrientation.YXOrientation;
					ReferencePoint = TextReferencePoint.Center;
					HOffset = 0;
					VOffset = 0;
					localRefPoint.Y = 0.5;
					break;
				case LabelPositionKind.BottomVertical:
					Orientation = TextOrientation.YXOrientation;
					ReferencePoint = TextReferencePoint.LeftCenter;
					HOffset = offsetSize;
					VOffset = 0;
					localRefPoint.Y = 0.0;
					break;
				case LabelPositionKind.TopHorizontal:	
					Orientation = TextOrientation.XYOrientation;
					ReferencePoint = TextReferencePoint.CenterTop;
					HOffset = 0;
					VOffset = -offsetSize;
					localRefPoint.Y = 1.0;
					break;
				case LabelPositionKind.CenterHorizontal:
					Orientation = TextOrientation.XYOrientation;
					ReferencePoint = TextReferencePoint.Center;
					HOffset = 0;
					VOffset = 0;
					localRefPoint.Y = 0.5;
					break;
				case LabelPositionKind.BottomHorizontal:
					Orientation = TextOrientation.XYOrientation;
					ReferencePoint = TextReferencePoint.CenterBottom;
					HOffset = 0;
					VOffset = offsetSize;
					localRefPoint.Y = 0.0;
					break;
				case LabelPositionKind.AboveVertical:	
					Orientation = TextOrientation.YXOrientation;
					ReferencePoint = TextReferencePoint.LeftCenter;
					HOffset = offsetSize;
					VOffset = 0;
					localRefPoint.Y = 1.0;
					localRefPoint.Z = 0.5;
					break;
				case LabelPositionKind.AboveHorizontal:
					Orientation = TextOrientation.XYOrientation;
					ReferencePoint = TextReferencePoint.CenterBottom;
					HOffset = 0;
					VOffset = offsetSize;
					localRefPoint.Y = 1.0;
					localRefPoint.Z = 0.5;
					break;
				case LabelPositionKind.BelowVertical:	
					Orientation = TextOrientation.YXOrientation;
					ReferencePoint = TextReferencePoint.RightCenter;
					HOffset = -offsetSize;
					VOffset = 0;
					localRefPoint.Y = 0.0;
					localRefPoint.Z = 0.5;
					break;
				case LabelPositionKind.BelowHorizontal:
					Orientation = TextOrientation.XYOrientation;
					ReferencePoint = TextReferencePoint.CenterTop;
					HOffset = 0;
					VOffset = -offsetSize;
					localRefPoint.Y = 0.0;
					localRefPoint.Z = 0.5;
					break;
				default: ;
					// do nothing here
					break;
			}
		}
		#endregion
	}

}
