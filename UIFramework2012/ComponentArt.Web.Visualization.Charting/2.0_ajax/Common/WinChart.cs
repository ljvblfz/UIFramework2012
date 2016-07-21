using System;
using System.CodeDom;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices; 

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	// (ED)
	/// <summary>
	/// Represents the method that will handle a <see cref="WinChart"/> event.
	/// </summary>
#if __BuildingWebChart__ || __BUILDING_CRI_DESIGNER__
	internal
#else
	public 
#endif
	delegate void WinChartEventHandler(WinChart chart);

	/// <summary>
	///		ComponentArt WinChart for .NET.
	/// </summary>
	/// <remarks>
	///   <para>
	///     Creates a chart in one or multiple <see cref="TargetArea"/>s (rectangular regions of the client area
	///     of the control). Each area has one or more coordinate systems where input data are presented.
	///   </para>
	///   <para>
	///     The charting control's object model consists of two major parts: data and styles.
	///     <b>Data</b> structure is presented in a tree-like hierarchy made of <see cref="Series"/> and 
	///     <see cref="CompositeSeries"/> (both inheriting from <see cref="SeriesBase"/>)
	///     nodes to properly reflect relationships of data in real life and to allow flexible data
	///     combination within the chart. <b>Styles</b> are presented by collection of style objects describing
	///     style aspects of different objects appearing within the chart. 
	///   </para>
	///   <para>
	///     <b>Data</b> structure is build by creating <see cref="CompositeSeries"/>
	///     and <see cref="Series"/> objects and arranging them in a series hierarchy. 
	///     Data values are given to <see cref="Series"/> nodes using functions <see cref="WinChart.DefineData"/>, 
	///     <see cref="WinChart.DefineAsExpression"/> and <see cref="WinChart.DefineValuePath"/>
	///     and property <see cref="WinChart.DataSource"/>.
	///     When data definition is complete, function <see cref="WinChart.DataBind"/> is called to build the rest
	///     of the chart object model. After that, user code may further manipulate and fine tune all
	///     objects of the chart.
	///   </para>
	///   <para>
	///     <b>It is important to understand that many objects in the chart object model are not available
	///     before <see cref="WinChart.DataBind"/> is completed.</b> The parts of the chart that depend on 
	///     input data do not exist before <see cref="WinChart.DataBind"/>. That includes <see cref="DataPoint"/>s,
	///     <see cref="SeriesLabels"/>, <see cref="Axis"/>, <see cref="CoordinatePlane"/> objects and related objects
	///     (because they cannot be created before data values, values ranges and type are known) and
	///     complete inner coordinate systems (in case of multi system or multi area charts).
	///     For more on charting data structures and data binding, see "Basic Concepts" topics and topic "Advanced Data Binding"
	///     in "Advanced Concepts".
	///   </para>
	///   <para>
	///     <b>Style</b> related data are implemented as the following style collections:
	///         <see cref="WinChart.SeriesStyles"/>,
	///         <see cref="WinChart.DataPointLabelStyles"/>,
	///         <see cref="WinChart.LineStyles"/>,
	///         <see cref="WinChart.LineStyles2D"/>,
	///         <see cref="WinChart.MarkerStyles"/>,
	///         <see cref="WinChart.GradientStyles"/>,
	///         <see cref="WinChart.LabelStyles"/>,
	///         <see cref="WinChart.TextStyles"/>,
	///         <see cref="WinChart.TickMarkStyles"/> and
	///         <see cref="WinChart.Palettes"/>.
	///      Members of these collections are styles used to define how objects that populate the chart 
	///      (like data points, labels, lines etc.) are rendered, or what color scheme is choosen,
	///      in case of palettes. At the control's creation, collections are populated with a rich set of
	///      predefined styles. They may be used to override the default styles. 
	///      The ComponentArt Web.Visualization.Charting style architecture is highly extendible, since existing styles can be customized
	///      and new styles can be added to the collections.
	///   </para>
    /// </remarks>
#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
    [GuidAttribute("D71F8B84-5D72-4e18-8F2C-F4763256F184")]
    [DesignerAttribute(typeof(WinChartControlDesigner))]
#endif
	[TypeConverter(typeof(WinChartObjectConverter))]
	[DesignerSerializer(typeof(WinChartCodeDomSerializer), typeof(CodeDomSerializer))]
#if __COMPILING_FOR_2_0_AND_ABOVE__
    [ComplexBindingProperties("DataSource")]
#endif
#if __BuildingWebChart__ || __BUILDING_CRI_DESIGNER__
    internal
#else
	public 
#endif
		class WinChart : Control, ICloneable, IChart
	{
		private ChartBase		m_chart;
		private bool		testRuntimeMenu = false;
		private System.Windows.Forms.ContextMenu m_runtimeContextMenu;
		private bool		m_trackballEnabled = false;
		private ContextMenuHandler	menuHandler;
		
#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
		private ChartDesigner m_chartDesigner;
#endif		
        static Cursor m_trackBallCursor = new Cursor(CommonFunctions.GetManifestResourceStream("TrackBallCursor.cur"));
        static Cursor m_trackBallDownCursor = new Cursor(CommonFunctions.GetManifestResourceStream("TrackBallDown.cur"));
        static Cursor m_trackBallUpCursor = new Cursor(CommonFunctions.GetManifestResourceStream("TrackBallUp.cur"));

		private Exception m_exception = null;
		internal Exception Exception {get {return m_exception;}}

		// Licensing info
		private Assembly callingAssembly;
#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
		private System.Windows.Forms.Timer timer1;
#endif
        private System.ComponentModel.IContainer components;

#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
		// Resizing support
		private DateTime lastTimeResized = DateTime.MinValue;
		private bool	 atLeastOncePainted = false;

		// Runtime Support

		// Editable properties
		private string[] fullListOfRuntimeEditablePropertyNames = null;
		private string[] runtimeEditablePropertyNames = null;
		// Save image parameters
		private string	imageFileName = null;
		private int		imageQuality = 85;
		private Size	savedImageSize = Size.Empty;
		// Printing
		private PrintDocument	printDocument;
		private PageSettings	pageSettings;
        // Test
        private bool overlayTrackingData = false;
#endif
#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
		// (ED)
		/// <summary>
		/// Occurs when the chart control is about to perform DataBind() operation.
		/// </summary>
		/// <remarks>
		/// Handle this event if you have to set control properties needed in data binding
		/// operation, such as input variables, chart style and composition kind.
		/// </remarks>
		[Description("Occurs when the chart control is about to perform DataBind() operation")]
		public event WinChartEventHandler PreDataBind;
		// (ED)
		/// <summary>
		/// Occurs after the chart control has performed DataBind() operation.
		/// </summary>
		/// <remarks>
		/// Handle this event if you have to set properties to chart object model
		/// components not available before DataBind(), i.e. before input data have been 
		/// proccessed, such as data points, axis annotations etc.
		/// </remarks>
		[Description("Occurs after the chart control has performed DataBind() operation.")]
		public event WinChartEventHandler PostDataBind;
#endif
        #region --- Constructor ---

        internal static WinChart CreateInstanceForWizard(ChartBase chartToClone)
        {
            WinChart winChart = new WinChart();
            winChart.Chart = chartToClone.GetCopy();
            winChart.Chart.InWizard = true;
            return winChart;
        }
        /// <summary>
		/// Initializes a new instance of the WinChart class.
		/// </summary>
		public WinChart()
		{
			InitializeComponent();
#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
			InitializeRuntimeEditablePropertyList(true);
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			// 
			// timer1
			// 
			this.timer1.Interval = 300;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
#endif

            m_chart = new ChartBase();
			m_chart.UseMatrixObjectTrackingModel = true;
			SetStyle(ControlStyles.SupportsTransparentBackColor, true); 
			BackColor = Color.Transparent;
			this.Resize += new EventHandler(this.HandleResizedEvent);
			m_chart.Mapping.ViewDirectionChanged += new EventHandler(this.HandleViewDirectionChanged);
			menuHandler = new ContextMenuHandler(this,m_chart, m_runtimeContextMenu);
			TabStop = false;
			callingAssembly = Assembly.GetCallingAssembly();
#if DEBUG
			testRuntimeMenu = true;
#endif
		}

		public void Clear()
		{
			m_trackballEnabled = false;
			testRuntimeMenu = false;
			m_exception = null;

			m_chart = new ChartBase();
			m_chart.UseMatrixObjectTrackingModel = true;
			m_chart.Mapping.ViewDirectionChanged += new EventHandler(this.HandleViewDirectionChanged);
			BackColor = Color.Transparent;
		}

		void BuildRunTimeMenu() 
		{
			//	return;
			m_runtimeContextMenu.MenuItems.Clear();
			menuHandler = new ContextMenuHandler(this,m_chart, m_runtimeContextMenu);

			if (!ChartBase.IsObfuscated) 
			{
				menuHandler.ContentsEnabled = true;
				m_runtimeContextMenu.MenuItems.Add(new MenuItem("Breakpoint", new EventHandler(BreakpointHandler)));
			}

			if (m_exception != null) 
			{
				m_runtimeContextMenu.MenuItems.Add(new MenuItem("Error Message", new EventHandler(ErrorMessageHandler)));
			}
		}

		internal void Build() { m_chart.Build(); }


		void ErrorMessageHandler(object sender, EventArgs e) 
		{
			MessageBox.Show(m_exception.StackTrace, m_exception.Message);
		}


		/// <summary>
		/// Base class method overriden.
		/// </summary>
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			m_chart.SetDesignMode(this.DesignMode);
		}

		#endregion

		#region ==== COMMON =====

		// (ED)
		/// <summary>
		/// Checks if this chart object supports the specified feature.
		/// </summary>
		/// <param name="featureName">Feature name.</param>
		/// <returns>True if the feature is supported</returns>
		/// <remarks>
		/// The feature set supported by a chart depends on the geometric engine. There are two features
		/// supported by HighQualityRendering engine but not 
		/// supported by HighSpeedRendering engine: "VariablePieHeight" and "PieLift".
		/// </remarks>
		public bool SupportsFeature(string featureName) { return m_chart.GeometricEngine.SupportsFeature(featureName); }

		#region ======== Design Time Properties ========

		#region --- Appearance Properties ---

		/// <summary>
		/// Gets or sets the <see cref="ChartFrame"/> object of the <see cref="WinChart"/>.
		/// </summary>
		[NotifyParentProperty(true)]
		[Bindable(true)]
		[Description("Chart frame"), Category("Appearance")]
		[DefaultValue(null)]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public ChartFrame Frame { get { return m_chart.ChartFrame; } set { m_chart.ChartFrame  = value; } }

		/// <summary>
		/// Gets or sets the text allignment of the frame.
		/// </summary>
		[NotifyParentProperty(true)]
		[Bindable(true)]
		[
		Description("Text Alignment"), 
		Category("Appearance"),
		DefaultValue(System.Drawing.StringAlignment.Center)
		]
		public StringAlignment TextAlignment 
		{	
			get 
			{ 
				if(Frame != null)
					return Frame.TextAlignment; 
				else
					return StringAlignment.Center;
			}
			set 
			{
				if(Frame != null)
					Frame.TextAlignment = value; 
			}
		}


		/// <summary>
		///		Gets or sets the frame title position.
		/// </summary>
		[NotifyParentProperty(true)]
		[Bindable(true)]
		[
		Description("Text Position"), 
		Category("Appearance"),
		DefaultValue(ComponentArt.Web.Visualization.Charting.FrameTextPosition.Top)
		]
		public FrameTextPosition TextPosition 
		{
			get 
			{ 
				if(Frame != null)
					return Frame.TextPosition; 
				else
					return FrameTextPosition.Top;
			}
			set 
			{
				if(Frame != null)
					Frame.TextPosition = value; 
			}
		}

		/// <summary>
		///		Gets or sets the value indicating whether the frame title text has a shade.
		/// </summary>
		[NotifyParentProperty(true)]
		[Bindable(true)]
		[Description("Indicates whether to display the text shade."), 
		Category("Appearance")]
		[DefaultValue(false)]
		public bool TextShade 
		{	
			get 
			{
				if(Frame != null)
					return Frame.TextShade;
				else
					return false;
			}
			set
			{ 
				if(Frame != null)
					Frame.TextShade = value; 
			}
		}

		/// <summary>
		///		Gets or sets the background gradient ending color.
		/// </summary>
		/// <remarks>
		///		When the chart background is rendered as gradient, i.e. the <see cref="BackGradientKind"/>
		///		is not set to <see cref="GradientKind.None"/>, two colors are used: <b>BackColor</b> and this color. Default setting
		///		for these colors is <see cref="Color.Transparent"/>. In that case, special <see cref="Palette"/> colors <see cref="Palette.BackgroundColor"/>
		///		and <see cref="Palette.BackgroundEndingColor"/> are used for gradient. To override
		///		palette color, set this property to value different from <see cref="Color.Transparent"/>.
		/// </remarks>
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		[Description("Set the background gradient type."), 
		Category("Appearance")]
		[DefaultValue(typeof(Color), "Transparent")]
		public Color BackGradientEndingColor {	get { return m_chart.BackGradientEndingColor; } set { m_chart.BackGradientEndingColor = value; } }

		/// <summary>
		///		Gets or sets the background gradient kind.
		/// </summary>
		/// <remarks>
		///   If this property is <see cref="GradientKind.None"/>, <see cref="Control.BackColor"/> is used 
		///   to paint the background, otherwise <see cref="GradientKind.None"/> and
		///   <see cref="WinChart.BackGradientEndingColor"/> are used for gradient. If any of these
		///   two properties is set to <see cref="Color.Transparent"/>, the corresponding special color from the
		///   <see cref="Palette"/> is used.
		/// </remarks>
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		[Description("Set the background gradient type."), 
		Category("Appearance")]
		[DefaultValue(typeof(GradientKind),"Vertical")]
		public GradientKind BackGradientKind {	get { return m_chart.BackGradientKind; } set { m_chart.BackGradientKind = value; } }

		#endregion
	
		#region --- Styles Properties ---

		/// <summary>
		/// Gets the collection of label styles contained within the chart.
		/// </summary>
		[
		Description("Collection of predefined and user defined label styles "), 
		Category("Chart Styles"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)
		]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public LabelStyleCollection LabelStyles {	get { return m_chart.LabelStyles; }	}

		/// <summary>
		/// Gets the collection of text styles contained within the chart.
		/// </summary>
		[
		Description("Collection of predefined and user defined text styles "), 
		Category("Chart Styles"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)
		]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public TextStyleCollection TextStyles { get { return m_chart.TextStyles; } }

		/// <summary>
		/// Gets the collection of data-point label styles contained within the chart.
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Description("Collection of predefined and user defined data point label styles "), 
		Category("Chart Styles")
		]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public DataPointLabelStyleCollection DataPointLabelStyles { get { return m_chart.DataPointLabelStylesX; } }

		/// <summary>
		/// Gets the collection of series styles contained within the chart.
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Description("Collection of predefined and user defined data point presentation styles "), 
		Editor(typeof(SeriesStyleCollectionEditor), typeof(System.Drawing.Design.UITypeEditor)),
		Category("Chart Styles")
		]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public SeriesStyleCollection SeriesStyles { get { return m_chart.SeriesStylesX; } }

		/// <summary>
		/// Gets the collection of text box styles contained within the chart.
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Description("Collection of predefined and user defined data text box styles "), 
		Category("Chart Styles")
		]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public TextBoxStyleCollection TextBoxStyles { get { return m_chart.TextBoxStyles; } }

		/// <summary>
		/// Gets the collection of color palettes contained within the chart.
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Description("Collection of palettes "), 
		Category("Chart Styles")
		]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public PaletteCollection Palettes { get { return m_chart.PalettesX; } }


		/// <summary>
		/// Gets the collection of light setups contained within the chart.
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Description("Collection of light setups"), 
		Category("Chart Styles")
		]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public LightingSetupCollection LightingSetups { get { return m_chart.LightingSetups; } }

		/// <summary>
		/// Gets or sets the palette used in the chart.
		/// </summary>
		[TypeConverter(typeof(SelectedPaletteConverter))]
		[EditorAttribute(typeof(SelectedPaletteEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[Description("Selected palette name"), 
		Category("Chart Styles")]
		[NotifyParentProperty(true)]
		[DefaultValue("Default")]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.Attribute)]
#endif
		public string SelectedPaletteName { get { return m_chart.SelectedPalette; } set { m_chart.SelectedPalette = value; } }

		/// <summary>
		/// Gets or sets the palette used in the chart.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PaletteKind SelectedPaletteKind { get { return Palette.KindOf(SelectedPaletteName); } set { SelectedPaletteName = Palette.NameOf(value); } }

		/// <summary>
		/// Gets or sets the lighting setup used in the chart.
		/// </summary>
		[TypeConverter(typeof(SelectedLightingSetupConverter))]
		//[EditorAttribute(typeof(SelectedPaletteEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[Description("Selected lighting setup name"), 
		Category("Chart Styles")]
		[NotifyParentProperty(true)]
		[DefaultValue("Default")]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.Attribute)]
#endif
		public string SelectedLightingSetupName { get { return m_chart.SelectedLightingSetup; } set { m_chart.SelectedLightingSetup = value; } }

		/// <summary>
		/// Gets the collection of line styles contained within the chart.
		/// </summary>
		//[Editor(typeof(LineStyleCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Description("Collection of predefined and user defined line styles "), 
		Category("Chart Styles")]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public LineStyleCollection LineStyles { get { return m_chart.LineStylesX; } }

		/// <summary>
		/// Gets the collection of 2D line styles contained within the chart.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Description("Collection of predefined and user defined 2D line styles "), 
		Category("Chart Styles")]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public LineStyle2DCollection LineStyles2D { get { return m_chart.LineStyles2DX; } }
		
		/// <summary>
		/// Gets the collection of gradient styles contained within the chart.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Description("Collection of predefined and user defined data point presentation styles "), 
		Category("Chart Styles")]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public GradientStyleCollection GradientStyles { get { return m_chart.GradientStyles; } }
		
		/// <summary>
		/// Gets the collection of marker styles contained within the chart.
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Description("Collection of predefined and user defined marker styles "), 
		Category("Chart Styles")
		]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public MarkerStyleCollection MarkerStyles { get { return m_chart.MarkerStyles; } }
		
		/// <summary>
		/// Gets the collection of tickmark styles contained within the chart.
		/// </summary>
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Description("Collection of predefined and user defined tickmark styles "), 
		Category("Chart Styles")
		]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public TickMarkStyleCollection TickMarkStyles { get { return m_chart.TickMarkStyles; } }

#endregion

		#region --- Chart Contents Properties ---
		/// <summary>
		/// Gets or sets automatic margins resizing to fit axis labels.
		/// </summary>
		[Description("Resize margins to make room for axis labels")]		
		[Category("Chart Contents")]
#if __BUILDING_CRI__ || __BUILDING_CRI_DESIGNER__
		[DefaultValue(true)]
#else
		[DefaultValue(false)]
#endif
		public bool ResizeMarginsToFitLabels { get { return m_chart.ResizeMarginsToFitLabels; } set { m_chart.ResizeMarginsToFitLabels = value; } }

		/// <summary>
		/// Gets or sets safety margins percentage.
		/// </summary>
		/// <remarks>
		/// This value defines safety margins between axis labels and border of the image in percentage of the
		///  margin size. For example, if the margins are 10 and safety margins are 20, the chart will 
		///  be sized so that labels don't cover 2 percent of the chart area.
		/// </remarks>
		[Description("Safety Margins for Axis Labels")]		
		[Category("Chart Contents")]
		[DefaultValue(10)]
		public double SafetyMarginsPercentage { get { return m_chart.SafetyMarginsPercentage; } set { m_chart.SafetyMarginsPercentage = value; } }

		/// <summary>
		/// Gets or sets the <see cref="Legend"/> object of the chart.
		/// </summary>
		[Description("Legend"), 
		Category("Chart Contents")]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[NotifyParentProperty(true)]
		public Legend Legend { get { return m_chart.Legend; } set { m_chart.Legend = value; } }
		/// <summary>
		/// Gets or sets the collection of secondary legends.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public LegendCollection SecondaryLegends { get { return m_chart.SecondaryLegends; } set { m_chart.SecondaryLegends = value; } }

#if __INCLUDE_TO_BE_REMOVED_CANDIDATES__
		/// <summary>
		/// Gets the collection of data dimensions contained within the chart.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public DataDimensionCollection DataDimensions { get { return m_chart.DataDimensionsX; } }
#endif
		
		/// <summary>
		/// Gets the collection of input variables contained within the chart.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This is collection of <see cref="InputVariable"/> objects populated by methods
		///     <see cref="WinChart.DefineValue"/>,
		///     <see cref="WinChart.DefineAsExpression"/> and
		///     <see cref="WinChart.DefineValuePath"/>.
		///   </para>
		///   <para>
		///     For more on input variables see topic "Data Binding" in "Basic Concepts"
		///     and "Advanced Data Binding" in "Advanced Concepts".
		///   </para>
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Description("Input variables"), 
		Category("Chart Contents")]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public InputVariableCollection InputVariables { get { return m_chart.InputVariables; } }

		/// <summary>
		/// Gets the collection of series contained in the first level of the series tree.
		/// </summary>
		[	Description("Series collection"), 
		Category("Chart Contents"),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		public SeriesCollection Series { get { return m_chart.Series.SubSeries; } }

		/// <summary>
		/// Gets or sets the number of data points simulated in design time.
		/// </summary>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[	Description("Number of data points simulated at design time"), 
		Category("Chart Contents")]
		public int NumberOfSimulatedDataPoints { get { return m_chart.NumberOfSimulatedDataPoints; } set { m_chart.NumberOfSimulatedDataPoints = value; } }

		/// <summary>
		/// Default style of the chart.
		/// </summary>
		[	Description("Chart style"), 
		Category("Chart Contents"),
		Editor(typeof(SeriesStyleEditor), typeof(System.Drawing.Design.UITypeEditor)),
		//TypeConverter(typeof(Design.SelectedSeriesStyleConverter)),
		RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[DefaultValue("Cylinder")]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.Attribute)]
#endif
		public string MainStyle { get { return m_chart.Series.StyleName; } set { m_chart.Series.StyleName = value; } }
		
		// (ED)
		/// <summary>
		/// Gets or sets default chart kind.
		/// </summary>
		/// <remarks>
		/// This property is used to get or set the main style when the style is one of predefined series styles.
		/// Setting the value <see cref="SeriesStyleKind.Custom"/> is wrong, unless there is a user created style named "Custom".
		/// For all user created styles, this property gets the value <see cref="SeriesStyleKind.Custom"/>.
		/// </remarks>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SeriesStyleKind MainStyleKind
		{
			get { return SeriesStyle.StyleKindOf(MainStyle); } 
			set { MainStyle = value.ToString(); }
		}

		/// <summary>
		///   Gets or sets the composition kind of the root series.
		/// </summary>
		/// <remarks>
		///    Composition kind defines the way series that belong to the root composite series
		///    are combined in the chart. See <see cref="CompositionKind"/> for more information 
		///    on series composition.
		/// </remarks>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.Attribute)]
#endif
		[NotifyParentProperty(true)]
		[Bindable(true)]
		[RefreshProperties(RefreshProperties.All)]
		[Description("Multiple series composition kind"), 
		Category("Chart Contents")]
		[DefaultValue(CompositionKind.Sections)]
		public CompositionKind CompositionKind { get { return m_chart.CompositionKind; } set { m_chart.CompositionKind = value; } }

		/// <summary>
		///		Gets or sets the main coordinate system of the chart.
		/// </summary>
		/// <remarks>
		///   <para>
		///		The main coordinate system of the chart is the <see cref="CompositeSeries.CoordSystem"/>
		///		property of the root <see cref="CompositeSeries"/>. Often this is the only coordinate system in the chart.
		///		It is available for modification in design time using wizard or property view.
		///	  </para>
		///	  <para>
		///	    When <see cref="CompositionKind"/> is <see cref="CompositionKind.MultiSystem"/> the main coordinate system
		///	    hosts multiple coordinate systems, one for each <see cref="Series"/> or
		///	    <see cref="CompositeSeries"/> child of the root series hierarchy node. In that case
		///	    position and size of children systems are expressed in coordinates of the main system and 
		///	    the main system is invisible.
		///	  </para>
		///	  <para>
		///	    When <see cref="CompositionKind"/> is <see cref="CompositionKind.MultiArea"/>
		///	    each area has its own coordinate system and the main coordinate system is ignored.
		///	  </para>
		/// </remarks>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
			/// <summary>
			///		Main coordinate system of the chart.
			/// </summary>
		[NotifyParentProperty(true)]
		[Bindable(true)]
		[	Description("Chart coordinate system"), 
		Category("Chart Contents") ]
		public CoordinateSystem CoordinateSystem { get { return m_chart.Series.CoordSystem; } set { m_chart.Series.CoordSystem = value; } }

		/// <summary>
		/// Gets or sets the value indicating whether the y-axis of the main coordinate system is logarithmic.
		/// </summary>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.Attribute)]
#endif
		[	Description("Is the chart logarithmic"), 
		Category("Chart Contents"),
		DefaultValue(false) ]
		public bool IsLogarithmic { get { return m_chart.Series.IsLogarithmic; } set { m_chart.Series.IsLogarithmic = value; } }

		/// <summary>
		/// Gets or sets the logarithm base of this chart.
		/// </summary>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.Attribute)]
#endif
		[	Description("The logarithm base"), 
		Category("Chart Contents") ,
		DefaultValue(10) ]
		public int LogBase { get { return m_chart.Series.LogBase; } set { m_chart.Series.LogBase = value; } }

		/// <summary>
		/// Gets the collection of lights contained within the chart.
		/// </summary>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Category("Chart Contents")]
        [Browsable(false)]
		public LightCollection Lights {	get { return m_chart.Lights; } }

		/// <summary>
		/// Gets or sets the rendering precision of the chart.
		/// </summary>
		/// <remarks>
		/// Rendering precision is the maximum distance between a rendered smooth surface or line and
		/// its theoretical position in coordinate system of the target bitmap. The unit is 1 pixel. 
		/// Values above 0.5 give coarse and faster rendering, value 0.1 is considered fine. (Note that
		/// ComponentArt chart uses sub-pixel sampling where needed to obtain smooth lines and surfaces.)
		/// </remarks>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.Attribute)]
#endif
		[Description("Rendering precision of smooth surfaces in pixels. Lower rendering precision value produces higher quality renderings."),
		Category("Chart Contents")]
		[DefaultValue(0.2)]
		public double RenderingPrecision { get {return m_chart.RenderingPrecision; } set { m_chart.RenderingPrecision = value; } }

		/// <summary>
		///		Gets or sets a <see cref="Mapping"/> object to the chart.
		/// </summary>
		/// <remarks>
		///   <para>
		///		The <see cref="Mapping"/> object defines mapping from thw World Coordinate System (WCS)
		///		into Target Coordinate System (TCS). We also call this "projection". Read more on coordinates
		///		in topic "Coordinates and Coordinate Systems" in section "Advanced Concepts".
		///	  </para>
		/// </remarks>
		[Description("Coordinate System Parameters"),
		Category("Chart Contents")]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[NotifyParentProperty(true)]
		[Bindable(true)]public Mapping View			
		{
			get 
			{ 
				return m_chart.Mapping; 
			} 
			set 
			{
				m_chart.Mapping = value; 
				m_chart.Mapping.Chart = this.Chart; 
				m_chart.Mapping.ViewDirectionChanged += new EventHandler(this.HandleViewDirectionChanged);
			} 
		}

		internal Mapping Mapping			
		{
			get 
			{ 
				return View; 
			} 
			set 
			{
				View = value;
			} 
		}

		/// <summary>
		/// Collection of chart titles.
		/// </summary>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Category("Chart Contents")]
		public ChartTitleCollection Titles { get { return m_chart.Titles; } }

		
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		internal CompositeSeries RootSeries { get { return m_chart.Series; } set { m_chart.Series = value; } }
		
		
		#endregion

		#endregion

		private static string[] PropertiesOrder = new string[] 
			{
				"SeriesStyles",
				"DataPointLabelStyles",
				"LineStyles",
				"MarkerStyles",
				"TickMarkStyles",
				"LabelStyles",
				"TextStyles",
				"Palettes",
				"SelectedPaletteName",	//	Delete "Palette"

				"Series",
				"MainStyle",
				"CompositionKind",
				"View",
				"CoordinateSystem",
				"DataDimensions",
				"Axes",
				"Lights",
				"RenderingPrecision"	//	Delete "Presentations"			
			};


		
		#region --- Data Binding ---

		/// <summary>
		/// Sets or gets the where clause for filtering rows in data source. 
        /// The property contains expression that follows word "Where" in SQL "Where" clause, for example "sales > 10000".
		/// Applies only if data source is <see cref="DataTable"/>.
		/// </summary>
		[Description("Sets or gets the select clause for filtering rows in data source.")]
		[Category("Data")]
		[DefaultValue(null)]
		public string SelectClause { get { return m_chart.DataProvider.SelectClause; } set { m_chart.DataProvider.SelectClause = value; } }

		/// <summary>
		/// Sets or gets the order clause for filtering rows in data source. 
        /// The property contains expression that follows words "ORDER BY" in SQL "ORDER BY" clause, for example "sales, expences DESC".
		/// Applies only if data source is <see cref="DataTable"/>.
		/// </summary>
		[Description("Sets or gets the order clause for filtering rows in data source.")]
		[Category("Data")]
		[DefaultValue(null)]
		public string OrderClause { get { return m_chart.DataProvider.OrderClause; } set { m_chart.DataProvider.OrderClause = value; } }

		/// <summary>
		/// Sets or gets the number rows selected from data source. 
        /// The property contains expression that follows the "TOP" key word in SQL.
		/// Applies only if data source is <see cref="DataTable"/>.
		/// </summary>
		[Description("Sets or gets the number rows selected from data source.")]
		[Category("Data")]
		[DefaultValue(0)]
		public int TopNumber { get { return m_chart.DataProvider.TopNumber; } set { m_chart.DataProvider.TopNumber = value; } }

		/// <summary>
		/// Defines value of an input variable.
		/// </summary>
		/// <param name="name">Variable name.</param>
		/// <param name="obj"> Value <see cref="object"/>.</param>
		/// <remarks>
		///     For more about input variables and data binding, see topics
		///     "Data Binding" in "Basic Concepts" and "Advanced Data Binding" in 
		///     "Advanced Concepts". Expressions are described in "Using Expressions" and
		///     "Using Expresion - Reference", both in section "Advanced Concepts".
		/// </remarks>
		public void DefineValue(string name, object obj) 
		{ 
			m_chart.DefineValue(name,obj); 
		}

		/// <summary>
		/// Defines value of an input variable.
		/// </summary>
		/// <param name="name">Variable name.</param>
		/// <param name="obj"> Value <see cref="object"/>.</param>
		/// <param name="dimension"> Dimension of the variable. <see cref="DataDimension"/>.</param>
		/// <remarks>
		///   <para>
		///     For more about input variables and data binding, see topics
		///     "Data Binding" in "Basic Concepts" and "Advanced Data Binding" in 
		///     "Advanced Concepts". Expressions are described in "Using Expressions" and
		///     "Using Expresion - Reference", both in section "Advanced Concepts".
		///   </para>
		///   <para>
		///     For non-standard input data the <see cref="DataDimension"/> object handling 
		///     input values is provided in this function call. The <see cref="DataDimension"/>
		///     object maps input values (in Data Coordinate System - DCS) into Logical 
		///     Coordinate System (LCS), which is the first step in allocating space that thata 
		///     points will ocupy in the charting coordinate system. For more about this, see
		///     "Coordinates and Coordinate Systems" in "Advanced Topics".
		///   </para>
		/// </remarks>
		public void DefineValue(string name, object obj, DataDimension dimension) 
		{ 
			DefineValue(name,obj);
			m_chart.DefineDimension(name,dimension);
		}
		
		/// <summary>
		///		Defines value of an input variable as expression.
		/// </summary>
		/// <param name="name">Input variable name.</param>
		/// <param name="expression">String representing the expression.</param>
		/// <remarks>
		///     For more about input variables and data binding, see topics
		///     "Data Binding" in "Basic Concepts" and "Advanced Data Binding" in 
		///     "Advanced Concepts". Expressions are described in "Using Expressions" and
		///     "Using Expresion - Reference", both in section "Advanced Concepts".
		/// </remarks>
		public void DefineAsExpression(string name, string expression) 
		{ 
			m_chart.DefineAsExpression(name,expression); 
		}

		/// <summary>
		///		Defines value of an input variable as expression and provides <see cref="DataDimension"/>
		///		that handles values of the input variable.
		/// </summary>
		/// <param name="name">Input variable name.</param>
		/// <param name="expression">String representing the expression.</param>
		/// <param name="dimension"> Dimension of the variable. <see cref="DataDimension"/>.</param>
		/// <remarks>
		///   <para>
		///     For more about input variables and data binding, see topics
		///     "Data Binding" in "Basic Concepts" and "Advanced Data Binding" in 
		///     "Advanced Concepts". Expressions are described in "Using Expressions" and
		///     "Using Expresion - Reference", both in section "Advanced Concepts".
		///   </para>
		///   <para>
		///     For non-standard input data the <see cref="DataDimension"/> object handling 
		///     input values is provided in this function call. The <see cref="DataDimension"/>
		///     object maps input values (in Data Coordinate System - DCS) into Logical 
		///     Coordinate System (LCS), which is the first step in allocating space that thata 
		///     points will ocupy in the charting coordinate system. For more about this, see
		///     "Coordinates and Coordinate Systems" in "Advanced Topics".
		///   </para>
		/// </remarks>
		/// 
		public void DefineAsExpression(string name, string expression, DataDimension dimension) 
		{ 
			DefineAsExpression(name,expression);
			m_chart.DefineDimension(name,dimension);
		}

		/// <summary>
		///		Defines value path of an input variable within data source object.
		/// </summary>
		/// <param name="name">Variable name.</param>
		/// <param name="valuePath">String representing the value path.</param>
		/// <remarks>
		///   <para>
		///     This method is used when the <see cref="WinChart.DataSource"/> object is used
		///     to provide data for the chart. Since data source may have multiple tables
		///     and each table may have multiple columns, the value path is used to select table
		///     and column in format "tablename.columnname".
		///   </para>
		///   <para>
		///     For more about input variables and data binding, see topics
		///     "Data Binding" in "Basic Concepts" and "Advanced Data Binding" in 
		///     "Advanced Concepts".
		///   </para>
		/// </remarks>
		public void DefineValuePath(string name, string valuePath) 
		{ 
			m_chart.DefineValuePath(name,valuePath); 
		}

		/// <summary>
		/// Defines value path of an input variable within data source object and variable dimension
		/// </summary>
		/// <param name="name">Variable name.</param>
		/// <param name="valuePath">String representing the value path.</param>
		/// <param name="dimension"> Dimension of the variable. <see cref="DataDimension"/>.</param>
		/// <remarks>
		///   <para>
		///     This method is used when the <see cref="WinChart.DataSource"/> object is used
		///     to provide data for the chart. Since data source may have multiple tables
		///     and each table may have multiple columns, the value path is used to select table
		///     and column in format "tablename.columnname".
		///   </para>
		///   <para>
		///     For more about input variables and data binding, see topics
		///     "Data Binding" in "Basic Concepts" and "Advanced Data Binding" in 
		///     "Advanced Concepts".
		///   </para>
		///   <para>
		///     For non-standard input data the <see cref="DataDimension"/> object handling 
		///     input values is provided in this function call. The <see cref="DataDimension"/>
		///     object maps input values (in Data Coordinate System - DCS) into Logical 
		///     Coordinate System (LCS), which is the first step in allocating space that thata 
		///     points will ocupy in the charting coordinate system. For more about this, see
		///     "Coordinates and Coordinate Systems" in "Advanced Topics".
		///   </para>
		/// </remarks>
		public void DefineValuePath(string name, string valuePath, DataDimension dimension) 
		{ 
			DefineValuePath(name,valuePath);
			m_chart.DefineDimension(name,dimension);
		}

		/// <summary>
		/// Sets or gets the mode of handling of series names. 
		/// </summary>
		/// <remarks>
		/// If false, each series has input variables for x, y and other coordinates. If true, 
		/// then a special input variable named "series" provides series names and x, y and other coordinates
		/// are given in common input variables. The control automatically creates series based on list of
		/// distinct names found in the variable "series" and selects series coordinates from common 
		/// variables.
		/// </remarks>
		[DesignOnly(true)]
		[Description("Sets or gets the mode of handling of series names.")]
		[Category("Data")]
		[DefaultValue(false)]
		public bool SeriesNameInInputData 
		{ 
			get { return m_chart.DataProvider.SeriesNameInInputData; }
			set { m_chart.DataProvider.SeriesNameInInputData = value; }
		}

#endregion

		#region --- Missing data point handling ---
		/// <summary>
		/// Gets or sets the missing points style name to be used with this <see cref="SeriesBase"/> object.
		/// </summary>
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
#if !__BUILDING_CRI__
		[Editor(typeof(SeriesStyleEditor), typeof(System.Drawing.Design.UITypeEditor))]
#endif
		[DefaultValue("")]
		[Category("Missing Points")]
		[SRDescription("SeriesBaseMissingPointStyleNameDescr")]
		public virtual string MissingPointsStyleName		
		{ 
			get 
			{ 
				return m_chart.Series.MissingPointsStyleName;
			} 
			set 
			{ 
				m_chart.Series.MissingPointsStyleName = value; 
			}
		}

		/// <summary>
		/// Gets or sets the missing points style kind.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Category("Missing Points")]
		[DefaultValue(typeof(SeriesStyleKind),"DefaultMissingPointsStyle")]
		[SRDescription("SeriesBaseMissingPointStyleKindDescr")]
		public SeriesStyleKind MissingPointsStyleKind 
		{
			get { return m_chart.Series.MissingPointsStyleKind; } 
			set { m_chart.Series.MissingPointsStyleKind = value; } 
		}


		// (ED)
		/// <summary>
		/// Gets or sets missing point handler kind.
		/// </summary>
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		[DefaultValue(MissingPointHandlerKind.Auto)]			
		[Category("Missing Points")]
		[Description("Missing data point handling kind")]
		public MissingPointHandlerKind MissingPointHandlerKind 
		{
			get { return m_chart.Series.MissingPointHandlerKind; }
			set { m_chart.Series.MissingPointHandlerKind = value; }
		}

		// (ED)
		/// <summary>
		/// Sets custom missing point handler.
		/// </summary>
		/// <param name="mph"></param>
		public void SetCustomMissingPointHandler(MissingPointHandler mph)
		{
			m_chart.Series.SetCustomMissingPointHandler(mph);
		}
		#endregion

		#region --- Chart Template Handling ---

		/// <summary>
		///		Loads chart from an XML template.
		/// </summary>
		/// <param name="templateFileName">Name of the input XML file</param>
		/// <remarks>
		///   <para>
		///     Loads chart settings from an XML file. The input file might be created by 
		///     <see cref="WinChart.StoreTemplate"/> functions and maybe hand-edited afterwards.
		///     After the chart is loaded from a template, data definition functions are needed
		///     to set real data and <see cref="WinChart.DataBind"/> is supposed to be called.
		///   </para>
		///   <para>
		///     The template concept makes possible saving and reusing design work involving 
		///     all pre-DataBind settings, including color palettes, coordinate systems setup,
		///     frame and titles etc. It allows easy creation of "the same chart with different data"
		///     and can save a lot of time when consistency is needed for a series of charts
		///     or in any other situation where chart standardization is required.
		///   </para>
		/// </remarks>
		public void LoadTemplate(string templateFileName)
		{
			Size mySize = this.Size;
			ChartXmlSerializer bld = new ChartXmlSerializer();
			bld.SerializingTemplate = true;
			bld.ReadObject(this,templateFileName);
			if(this.Size.IsEmpty)
				this.Size = mySize;
		}

		/// <summary>
		///		Loads chart from an XML template.
		/// </summary>
		/// <param name="templateStream">The input stream object</param>
		/// <remarks>
		///   <para>
		///     Loads chart settings from an XML file. The input file might be created by 
		///     <see cref="WinChart.StoreTemplate"/> functions and maybe hand-edited afterwards.
		///     After the chart is loaded from a template, data definition functions are needed
		///     to set real data and <see cref="WinChart.DataBind"/> is supposed to be called.
		///   </para>
		///   <para>
		///     The template concept makes possible saving and reusing design work involving 
		///     all pre-DataBind settings, including color palettes, coordinate systems setup,
		///     frame and titles etc. It allows easy creation of "the same chart with different data"
		///     and can save a lot of time when consistency is needed for a series of charts
		///     or in any other situation where chart standardization is required.
		///   </para>
		/// </remarks>
		public void LoadTemplate(Stream templateStream)
		{
			Size mySize = this.Size;
			ChartXmlSerializer bld = new ChartXmlSerializer();
			bld.SerializingTemplate = true;
			bld.ReadObject(this,templateStream);
			if(this.Size.IsEmpty)
				this.Size = mySize;
		}

		/// <summary>
		///		Stores chart to an XML template.
		/// </summary>
		/// <param name="templateFileName">Name of the output XML file</param>
		/// <remarks>
		///   <para>
		///     Stores chart settings to an XML file. The output file might be used in
		///     <see cref="WinChart.LoadTemplate"/> function to restore chart settings.
		///   </para>
		///   <para>
		///     The template concept makes possible saving and reusing design work involving 
		///     all pre-DataBind settings, including color palettes, coordinate systems setup,
		///     frame and titles etc. It allows easy creation of "the same chart with different data"
		///     and can save a lot of time when consistency is needed for a series of charts
		///     or in any other situation where chart standardization is required.
		///   </para>
		/// </remarks>
		public void StoreTemplate(string templateFileName)
		{
			ChartXmlSerializer bld = new ChartXmlSerializer(this,"Chart");
			bld.SerializingTemplate = true;
			bld.Write(templateFileName);
		}

		/// <summary>
		///		Stores chart to an XML template.
		/// </summary>
		/// <param name="templateFileName">The output stream</param>
		/// <remarks>
		///   <para>
		///     Stores chart settings to a stream. The output might be used in
		///     <see cref="WinChart.LoadTemplate"/> function to restore chart settings.
		///   </para>
		///   <para>
		///     The template concept makes possible saving and reusing design work involving 
		///     all pre-DataBind settings, including color palettes, coordinate systems setup,
		///     frame and titles etc. It allows easy creation of "the same chart with different data"
		///     and can save a lot of time when consistency is needed for a series of charts
		///     or in any other situation where chart standardization is required.
		///   </para>
		/// </remarks>
		public void StoreTemplate(Stream templateStream)
		{
			ChartXmlSerializer bld = new ChartXmlSerializer(this,"Chart");
			bld.SerializingTemplate = true;
			bld.Write(templateStream);
		}
		#endregion

		/// <summary>
		/// Gets or sets the reference value.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This is a base value along the y-axis of the charts. All bars, as well as areas
		///     are rendered with this base values. The type of this object has to be the type 
		///     of y-values of the chart.
		///   </para>
		///   <para>
		///     Often the y-values are numeric and default reference value is 0. If values
		///     are from a narrow range, say between 200 and 210, you may choose reference value
		///     200. The same kind of operation makes sense for other types of y-values.
		///   </para>
		///   <para>
		///     Reference value is handy when there is a baseline value (like average) and we want
		///     values below the baseline to be drawn downwards.
		///   </para>
		/// </remarks>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object ReferenceValue { get { return m_chart.Series.ReferenceValue; } set { m_chart.Series.ReferenceValue = value; } }

		// (ED)
		/// <summary>
		/// Gets or sets the reference value.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This property can be edited in design time. In code it is equivalent to 
		///     the <see cref="ReferenceValue"/> property.
		///   </para>
		///   <para>
		///     This is a base value along the y-axis of the charts. All bars, as well as areas
		///     are rendered with this base values. The type of this object has to be the type 
		///     of y-values of the chart.
		///   </para>
		///   <para>
		///     Often the y-values are numeric and default reference value is 0. If values
		///     are from a narrow range, say between 200 and 210, you may choose reference value
		///     200. The same kind of operation makes sense for other types of y-values.
		///   </para>
		///   <para>
		///     Reference value is handy when there is a baseline value (like average) and we want
		///     values below the baseline to be drawn downwards.
		///   </para>
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[Description("Reference value for all chart series")]
		[Category("Chart Contents")]
		[DefaultValue(typeof(GenericType),"")]
		public GenericType Reference { get { return m_chart.Series.Reference; } set { m_chart.Series.Reference = value; } }
 
		// (ED)
		/// <summary>
		/// Gets or sets a value that indicates whether reference value should be adjusted to the y-value range.
		/// </summary>
		[Description("Should reference value be adjusted to the y-value range")]
		[Category("Chart Contents")]
		[DefaultValue(false)]
		public bool AdjustReferenceValue
		{
			get { return m_chart.Series.AdjustReferenceValue; } set { m_chart.Series.AdjustReferenceValue = value; } 
		}
		
		// (ED)
#if SAFE_CODE_ONLY
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GeometricEngineKind GeometricEngineKind { get { return m_chart.GeometricEngineKind; } set { m_chart.GeometricEngineKind = value; } }
#else
		[Category("Chart Contents")]
		[Description("Type of geometric engine")]
        public GeometricEngineKind GeometricEngineKind { get { return m_chart.GeometricEngineKind; } set { m_chart.GeometricEngineKind = value; } }
#endif
        #endregion (COMMON)

        #region --- WinChart Run-time Support ---

#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
		
        #region --- Run-time Save Image Support ---

		/// <summary>
		/// Gets or sets image file name.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ImageFileName { get { return imageFileName; } set { imageFileName = value; } }

		/// <summary>
		/// Gets or sets image quality for JPG image format. The value is between 1 (lowest quality) and 100 (highest quality).
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int ImageQuality { get { return imageQuality; } set { imageQuality = Math.Max(0, Math.Min(100,value)); } }

		/// <summary>
		/// Gets or sets saved image size.
		/// </summary>
		/// <remarks>
		/// This property makes possible the size (resolution) of saved image to be different from the size of the control.
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Size SavedImageSize { get { return savedImageSize; } set { savedImageSize = value; } }

		/// <summary>
		/// Saves the chart image to a file.
		/// </summary>
		/// <param name="imageSize">The size of the output image.</param> 
		/// <param name="fileName">Output file name. Extension must indicate the image file format.</param>
		/// <param name="jpegQuality">Quality of compressed bitmap. Used only for JPG type of image.</param>
		/// <remarks>
		/// This function changes parameters <see cref="SavedImageSize"/>, <see cref="ImageFileName"/> and <see cref="ImageQuality"/>
		/// </remarks>
		public void SaveImage(Size imageSize, string fileName, int jpegQuality)
		{
			this.SavedImageSize = imageSize;
			this.ImageFileName = fileName;
			this.ImageQuality = jpegQuality;

			// Process the file name
			ImageCodecInfo[] encoders;
			string[] parts = fileName.Split('.');
			if(parts.Length <= 1)
				throw new ArgumentException("File name '" + fileName + "' doesn't have valid extension","fileName");
			string ext = "*." + parts[parts.Length-1].ToLower();
			encoders = ImageCodecInfo.GetImageEncoders();
			ImageCodecInfo jpegCodecInfo = null;
			bool validExtension = false;
			for(int j = 0; j < encoders.Length; ++j)
			{
				if(encoders[j].MimeType == "image/jpeg")
					jpegCodecInfo = encoders[j];
				// Checking extension
				string allExtensions = encoders[j].FilenameExtension;
				string[] extensions = allExtensions.Split(';');
				for(int k=0;k<extensions.Length; k++)
					if(extensions[k].ToLower() == ext)
					{
						validExtension = true;
					}
			}
			if(!validExtension)
				throw new ArgumentException("File name '" + fileName + "' doesn't have valid extension","fileName");


			Size oldSize = this.Size;
			Size = imageSize;
			Bitmap bmp = Draw();
			// Getting extension and type

			if(ext == "*.jpg" || ext == "*.jpeg")
			{
				EncoderParameters myEncoderParameters = new EncoderParameters(1);
				myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, jpegQuality);

				bmp.Save(fileName, jpegCodecInfo, myEncoderParameters);
			}
			else
				bmp.Save(fileName);

			this.Size = oldSize;
		}

        #endregion //(Run-time Save Image Support)

        #region --- Run-time Print Support ---

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PrintDocument PrintDocument
		{
			get
			{
				if(printDocument == null)
				{
					printDocument = new PrintDocument();
					printDocument.PrintPage += new PrintPageEventHandler(HandlePrintPage);
					printDocument.QueryPageSettings += new QueryPageSettingsEventHandler(HandleQueryPageSettings);
				}
				return printDocument;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PageSettings	PageSettings
		{ 
			get 
			{ 
				if(pageSettings == null)
					pageSettings = new PageSettings();
				return pageSettings;
			}
			set
			{
				pageSettings = value;
			}
		}

		public void Print()
		{
			PrintDocument.PrinterSettings = PageSettings.PrinterSettings;
			PrintDocument.Print();
		}

		private void HandlePrintPage(object sender, PrintPageEventArgs e)
		{
			PrinterResolution pRes = PageSettings.PrinterResolution;
			int pResX, pResY;
			switch(pRes.Kind)
			{
				case PrinterResolutionKind.Draft:
					pResX = 72;
					pResY = 72;
					break;
				case PrinterResolutionKind.Low:
					pResX = 100;
					pResY = 100;
					break;
				case PrinterResolutionKind.Medium:
					pResX = 150;
					pResY = 150;
					break;
				case PrinterResolutionKind.High:
					pResX = 200;
					pResY = 200;
					break;
				default:
					pResX = pRes.X;
					pResY = pRes.Y;
					if(pResX <= 0)
						pResX = pResY;
					if(pResY <= 0)
						pResY = pResX;
					pResX = Math.Max(72,Math.Min(200,pResX));
					pResY = Math.Max(72,Math.Min(200,pResY));
					break;
			}

			PaperSize pSize = PageSettings.PaperSize;
			Margins margs = PageSettings.Margins;
			Rectangle r = new Rectangle(margs.Left,margs.Top,pSize.Width-margs.Left-margs.Right, pSize.Height-margs.Top-margs.Bottom);
			Size oldSize = this.Size;
			Size = new Size(pResX*r.Size.Width/100,pResY*r.Size.Height/100);
			Bitmap bmp = Draw();
			e.Graphics.DrawImage(bmp,r);
			bmp.Dispose();
			this.Size = oldSize;
			Refresh();

			e.HasMorePages = false;
		}

		private void HandleQueryPageSettings(object sender, QueryPageSettingsEventArgs e)
		{
			
		}

        #endregion //(Run-time Print Support)

#endif
        #endregion //(Run-time Support)

        internal Rectangle TargetRectangle 
		{
			set
			{
				m_chart.TargetAreaLocation = value.Location;
				m_chart.TargetSize = value.Size;
			}
			get
			{
				return new Rectangle(m_chart.TargetAreaLocation,m_chart.TargetSize);
			}
		}

		/// <summary>
		/// Overriden base class property.
		/// </summary>
		[Description("Text to be displayed in the frame.")]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				try
				{
					if(m_chart != null)
						m_chart.Dispose();
					m_chart = null;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}


		#region ======= Public User Interface =========

		/// <summary>
		///		Context menu of this control.
		/// </summary>
		/// <remarks>
		///		User can create a <see cref="ContextMenu"/> object and assign it to this property. After
		///		that, mouse right-click on the chart will bring up the menu.
		/// </remarks>
		public ContextMenu ContextMenu
		{
			get { return m_runtimeContextMenu; }
			set { m_runtimeContextMenu = value; }
		}

		#endregion 

		#region --- Trackball Properties ---

		/// <summary>
		/// Gets or sets a value that indicates whether the trackball is enabled.
		/// </summary>
		/// <remarks>
		/// When trackball is enabled, user can rotate coordinate system by moving mouse
		/// over client regia of the chart, while left mouse button is kept pressed.
		/// </remarks>
		[Description("Enables or Disables trackball on the chart")]
			//[Category("Run-time features")]
			//[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		public bool TrackballEnabled 
		{
			get 
			{
				return m_trackballEnabled;
			}
			set 
			{
				m_trackballEnabled = value;
				if (!InDesignMode)
				{
					this.Cursor = m_trackballEnabled ? m_trackBallUpCursor : Cursors.Default;
				}
			}
		}
		#endregion

		#region --- Data Binding ---

		/// <summary>
		///		The source of data for the chart control.
		/// </summary>
		/// <remarks>
		///		Data source can be various objects that help extracting data from databases.
		///		For more on data sources see topic "Data Binding" in "Basic Concepts".
		/// </remarks>
		//[TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design")]
#if __COMPILING_FOR_2_0_AND_ABOVE__
        [AttributeProvider(typeof(IListSource))]
        //[Editor(typeof(DataSourceListEditor), typeof(System.Drawing.Design.UITypeEditor)), TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
#else
		[TypeConverter(typeof(DataSourceConverter))]
#endif
		[Category("Data")]
		[Description("Indicates the source of data for the chart control")]
		[NotifyParentProperty(true)]
		[DefaultValue(null)]
		[Bindable(true)]
		public object DataSource 
		{
			get { return m_chart.DataSource; } 
			set 
			{
				m_chart.DataSource = value; 
			}
		}

		/// <summary>
		///		Completes input variables definition and builds internal structure of the chart.
		/// </summary>
		/// <remarks>
		///   <para>
		///		This function is invoked after data has been defined using
		///		<see cref="WinChart.DefineData"/>, 
		///		<see cref="WinChart.DefineAsExpression"/>, 
		///		<see cref="WinChart.DefineDataPath"/> and
		///		<see cref="WinChart.DataSource"/>. 
		///	  </para>
		///   <para>
		///     <b>Many objects in the chart object model are not available
		///     before <see cref="WinChart.DataBind"/> is completed.</b> The parts of the chart that depend on 
		///     input data do not exist before <see cref="WinChart.DataBind"/>. That includes <see cref="DataPoint"/>s,
		///     <see cref="SeriesLabels"/>s, <see cref="Axis"/>, <see cref="CoordinatePlane"/> objects and related objects
		///     (because they cannot be created before data values, values ranges and data type are known) and
		///     complete inner coordinate systems (in case of multi system or multi area charts).
		///     For more on charting data structures and data binding, see "Basic Concepts" topics and topic "Advanced Data Binding"
		///     in "Advanced Concepts".
		///   </para>
		/// </remarks>
		public void DataBind()
		{
			try
			{
				m_chart.TargetSize = this.Size;
				m_chart.ProcessBeforeDataBinding(this);
#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
				if(PreDataBind != null)
					PreDataBind(this);
				m_chart.DataBind();
				if(PostDataBind != null)
					PostDataBind(this);
#else
                m_chart.DataBind();
#endif
				m_chart.ProcessAfterDataBinding(this);
			}
			catch (Exception ex)
			{
				m_exception = ex;
			}
		}

#if !__BuildingWebChart__

		/// <summary>
		/// Sets or gets initialization mode on data bind.
		/// </summary>
		/// <remarks>
		///   <para>
		///		This flag is used when the <see cref="DataBind"/> method is used more than once. 
		///	  </para>
		///   <para>
		///		The first time the "DataBind()" function is called, the control creates the chart's 
		///		object model. Optionally, user can override some parts of the model in post-DataBind
		///		operations. If this flag is set, the next time "DataBind()" is called, the control
		///		overrides results of the previous post-DataBind operations.
		///	  </para>
		/// </remarks>
		[Category("Data")]
		[Description("Sets or gets initialization mode on data bind")]
		[NotifyParentProperty(true)]
		[DefaultValue(false)]
		public bool InitializeOnDataBind 
		{
			set { m_chart.InitializeOnDataBind = value; }
			get { return m_chart.InitializeOnDataBind; }
		}
#endif
		#endregion

		#region --- OnPaint and other methods ---
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
#if __BuildingWebChart__ 
        public
#else
        internal
#endif
 ChartBase Chart 
		{
			get { return m_chart; }
			set { m_chart = value; }
		}

		internal bool InDesignMode { get { return DesignMode; } }

		/// <summary>
		/// Base class method overriden.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnResize(EventArgs e)
        {
#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
			TimeSpan ts = DateTime.Now-lastTimeResized;
			if(ts.TotalMilliseconds < 2000 && atLeastOncePainted)
			{
				timer1.Start();
				m_chart.SetAutoReducedSamplingStep();
			}
			else
			{
				timer1.Stop();
			}
			lastTimeResized = DateTime.Now;
#endif
            m_chart.Invalidate();
			Invalidate(ClientRectangle);
		}

		/// <summary>
		///		Base class method overriden.
		/// </summary>
		/// <param name="e"> Event arguments</param>
		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			m_chart.BackColor = this.BackColor;
			m_chart.Invalidate();
		}

		/// <summary>
		///		The chart background image.
		/// </summary>
		[Description("The chart background image.")]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
				if(value is Bitmap)
				{
					m_chart.Background = (Bitmap)value;
					m_chart.Invalidate();
				}
				else if (value == null)
					m_chart.Background = null;
			}
		}


		/// <summary>
		/// Base class method overriden. 
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected override void OnInvalidated ( System.Windows.Forms.InvalidateEventArgs e ) 
		{
			m_chart.Invalidate();
			base.OnInvalidated(e);
		}

		/// <summary>
		/// Base class method overriden.
		/// </summary>
		/// <param name="e">Event arguments</param>
		/// <remarks>
		/// This method is implemented to do nothing. Background is not painted to avoid flicker when
		/// the control is repainted. 
		/// </remarks>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{ // Do nothing to prevent flicker. Chart uses background object to fill the area.
		}

		/// <summary>
		/// Renders the chart into specified <see cref="System.Drawing.Graphics"/> object within 
		/// a specified rectangle <see cref="System.Drawing.Rectangle"/>.
		/// </summary>
		/// <param name="g"><see cref="System.Drawing.Graphics"/> object to draw into.</param>
		/// <param name="rect"><see cref="System.Drawing.Rectangle"/> in g to draw into.</param>
		public void Draw(Graphics g, Rectangle rect)
		{
			if(rect.IsEmpty)
				return;			
			try
            {
				bool toInvalidate;
				if(m_chart.TargetSize.Width != rect.Size.Width || m_chart.TargetSize.Height != rect.Size.Height)
				{
					m_chart.TargetSize = rect.Size;
					toInvalidate = true;
				}
				else
					toInvalidate = false;
				m_chart.BackColor = BackColor;
				m_chart.Invalidate();
				m_chart.ReducedSamplingStep = 1;
				m_chart.ChartFrame.FontColor = ForeColor;
				if(m_chart.NeedsDataBinding)
					DataBind();
				m_chart.Render(g);
				if(toInvalidate)
					m_chart.Invalidate();
			}
			catch(Exception ex)
			{
				m_chart.RenderErrorMessage(ex,g);
				m_exception = ex;
				throw/* new Exception(ex.Message, ex)*/;
			}
		}

#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
		private bool OverlayTrackingData { set { overlayTrackingData = value; } }
#endif
        private void DoOverlayTrackingData(Graphics g)
        {
            if (!ObjectTrackingEnabled)
                return;
            ArrayList objects = new ArrayList();
            
            int nx = ClientRectangle.Width;
            int ny = ClientRectangle.Height;
			Bitmap bmp = new Bitmap(nx,ny);
            for(int ix = 0; ix<nx; ix++)
                for (int iy = 0; iy < ny; iy++)
                {
                    object obj = GetObjectAt(ix, iy);
					if (obj != null)
					{
						int index = objects.IndexOf(obj);
						if (index < 0)
							index = objects.Add(obj);
						int red = (255 + index * 127) % 256;
						int green = (128 + index * 37) % 256;
						int blue = (0 + index * 7) % 256;

						bmp.SetPixel(ix,iy,Color.FromArgb(196,red,green,blue));
					}
                }
			g.DrawImage(bmp,0,0);
			bmp.Dispose();
        }


        /// <summary>
        /// Draws the chart into a <see cref="Bitmap"/> of the same size ad the chart control.
        /// </summary>
        /// <returns>Newly generated <see cref="Bitmap"/> with the image of the chart.</returns>
        public Bitmap Draw()
		{
			Bitmap bmp = new Bitmap(Size.Width, Size.Height);
			Graphics g = Graphics.FromImage(bmp);
			Rectangle oldRec = TargetRectangle;
			TargetRectangle = new Rectangle(0,0,Size.Width,Size.Height);

			Draw(g, new Rectangle(0, 0, Size.Width, Size.Height));
			g.Dispose();

			TargetRectangle = oldRec;
			return bmp;
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.m_runtimeContextMenu = new System.Windows.Forms.ContextMenu();
		}

		/// <summary>
		/// Base class method overriden.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if(e.ClipRectangle.IsEmpty)
				return;
			Graphics g = e.Graphics;

			Rectangle rect = ClientRectangle;

			if(rect.IsEmpty)
				return;	
		
#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
			if (ChartDesigner != null)
				ChartDesigner.Exception = null;
#endif
			try
			{
				m_exception = null;
				DateTime start = DateTime.Now;
				m_chart.TargetAreaLocation = new Point(0,0);
				m_chart.TargetSize = rect.Size;
				m_chart.ChartFrame.SetText(Text);
				m_chart.ChartFrame.FontColor = ForeColor;
				if(m_chart.NeedsDataBinding)
					DataBind();
				m_chart.Render(g);

#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
                if (overlayTrackingData)
                    DoOverlayTrackingData(g);
#endif
                if (rect.Width >= 300 && rect.Height >= 200)
				{
					DateTime end = DateTime.Now;
					TimeSpan span = end-start;	
					string s = 
						m_chart.Duration3DBmp.TotalSeconds.ToString("Time = 0.00") + " + " +
						m_chart.Duration2DBmp.TotalSeconds.ToString(" 0.00") + " + " +
						m_chart.DurationMapping.TotalSeconds.ToString(" 0.00 sec");
					//	(span.TotalSeconds).ToString("Time = 0.000sec");
#if DEBUG
					s = s + "(D)";
					g.DrawString(s,new Font("Arial",8), new SolidBrush(Color.Blue), new Point(0,0));
#else
					s = s + "(R)";
#endif
                }
#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
				atLeastOncePainted = true;
#endif
            }
			catch(Exception ex)
			{
				m_chart.RenderErrorMessage(ex,g);
				m_exception = ex;
#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
				if (ChartDesigner != null) 
				{
					ChartDesigner.Exception = m_exception;
				}
#endif
			}

		}
		#endregion

		#region --- Performance Indicators ---
		private TimeSpan DurationDataBind { get { return m_chart.DurationDataBind; } }
		private TimeSpan Duration3DBmp { get { return m_chart.Duration3DBmp; } }
		private TimeSpan Duration2DBmp { get { return m_chart.Duration2DBmp; } }
		private TimeSpan DurationMapping { get { return m_chart.DurationMapping; } }
		#endregion

		#region --- Object Tracking ---

		public object GetObjectAt(int x, int y)
		{
			return m_chart.GetObjectAt(x,y);
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal MapAreaCollection MapAreas { get { return m_chart.MapAreas; } }

		/// <summary>
		///		Gets or sets the object tracking mode.
		/// </summary>
		/// <remarks>
		///   <para>
		///		When this mode is on, function <see cref="GetObjectAt"/> returns <see cref="Series"/>
		///		or <see cref="DataPoint"/> at coordinates (x,y), or null if no such object exists at
		///		given coordinates. If this property = false, null is always returned. 
		///   </para>
		///   <para>
		///		This property is used to get faster chart rendering if <see cref="GetObjectAt"/>
		///		is not used.
		///   </para>
		/// </remarks>
		[SRCategory("CatBehavior")]
		[Description("Indicates whether the objects in the chart are tracked.")]
		[DefaultValue(false)]
		public bool ObjectTrackingEnabled { get { return m_chart.ObjectTrackingEnabled; } set { m_chart.ObjectTrackingEnabled = value; } }

		#endregion

		#region --- Trackball Stuff ---


		double m_radius;
		bool m_dragging;

		Point m_lastPoint = new Point(0,0);
		bool anglesSet = false;

		double m_alpha;
		double m_beta;

		int m_direction;

		/// <summary>
		/// Base class method overriden.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnMouseDown ( System.Windows.Forms.MouseEventArgs e ) 
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left) 
			{
				if (!m_trackballEnabled)
					return;

#if !__BuildingWebChart__
				m_chart.SetAutoReducedSamplingStep();
#endif
				this.Cursor = m_trackBallDownCursor;
				
				m_radius = Mapping.ViewDirection.Abs;

				m_dragging = true;
				m_lastPoint.X = e.X;
				m_lastPoint.Y = e.Y;

				m_direction = Math.Cos(m_beta) < 0 ? -1 : 1;

				if (!anglesSet) 
				{
					setAngles();
					anglesSet=true;
				}
			}

			if (e.Button == MouseButtons.Right && testRuntimeMenu)
			{
				BuildRunTimeMenu();
				m_runtimeContextMenu.Show(this, new Point(e.X, e.Y));
			}
		}


		void BreakpointHandler(object sender, EventArgs e) 
		{
			Invalidate();
		}
		
		internal double Alpha 
		{
			get {return m_alpha*180/Math.PI;}
			set 
			{
				m_radius = Mapping.ViewDirection.Abs;
				m_alpha = value/(180/Math.PI);
				setDirectionFromAngles();
				Invalidate();
			}
		}


		internal double Beta 
		{
			get {return m_beta*180/Math.PI;}
			set 
			{
				m_radius = Mapping.ViewDirection.Abs;
				m_beta = value/(180/Math.PI);
				setDirectionFromAngles();
				Invalidate();
			}
		}

		/// <summary>
		/// Occurs when the view direction of the chart is changed
		/// </summary>
		public event EventHandler ViewDirectionChanged;	

		/// <summary>
		/// Raises the <see langword="ViewDirectionChanged"/> event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnViewDirectionChanged(EventArgs e)
		{
			setAngles();
			if (ViewDirectionChanged!=null)
				ViewDirectionChanged(this, e);
		}

		private void setAngles() 
		{
			Vector3D v = Chart.ViewDirection.Unit();

			m_alpha = Math.Atan2(v.Z, v.X);
			m_beta = Math.Asin(v.Y);
		}

		void setDirectionFromAngles() 
		{
			Mapping.ViewDirection = m_radius * 
				new Vector3D(
				Math.Cos(m_alpha) * Math.Cos(m_beta),
				Math.Sin(m_beta),
				Math.Sin(m_alpha) * Math.Cos(m_beta)
				);
		}

		/// <summary>
		/// Base class method overriden.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnMouseUp ( System.Windows.Forms.MouseEventArgs e ) 
		{
			base.OnMouseUp(e);


			if (e.Button == MouseButtons.Left) 
			{
				if (!m_trackballEnabled)
					return;
				m_chart.ReducedSamplingStep = 1;
				Invalidate();

				m_dragging = false;
				this.Cursor = m_trackBallUpCursor;
			}
		}

		/// <summary>
		/// Base class method overriden.
		/// </summary>
		/// <param name="e">Event arguments</param>		
		protected override void OnMouseMove ( System.Windows.Forms.MouseEventArgs e ) 
		{
			base.OnMouseMove(e);

			if (!m_trackballEnabled)
				return;

			if (m_dragging) 
			{

				int dY = e.Y-m_lastPoint.Y;
				int dX = e.X-m_lastPoint.X;

				m_alpha += dX*0.02;
				m_beta	+= dY*m_direction*0.02;

				setDirectionFromAngles();

				// Set new last points
				m_lastPoint.Y = e.Y;
				m_lastPoint.X = e.X;

				Invalidate();
			}
		}


		#endregion

		#region --- Free Annotations ---
		// (ED)
		/// <summary>
		/// Creates free annotation.
		/// </summary>
		/// <param name="text">Annotation text.</param>
		/// <param name="styleName">Name of the text box style.</param>
		/// <param name="anchorPoint">Anchor point.</param>
		/// <returns>
		/// Resulting text box object.
		/// </returns>
		public ChartTextBox CreateAnnotation(string text, string styleName, TextAnchor anchorPoint)
		{
			return m_chart.CreateTextBox(styleName,text,anchorPoint);
		}

		// (ED)
		/// <summary>
		/// Creates free annotation.
		/// </summary>
		/// <param name="text">Annotation text.</param>
		/// <param name="styleKind">The enum value selecting a predefined text style.</param>
		/// <param name="anchorPoint">Anchor point.</param>
		/// <returns>
		/// Resulting text box object.
		/// </returns>
		public ChartTextBox CreateAnnotation(string text, TextBoxStyleKind styleKind, TextAnchor anchorPoint)
		{
			return m_chart.CreateTextBox(TextBoxStyle.NameOf(styleKind),text,anchorPoint);
		}

		#endregion

		#region --- Testing code ----

		private void HandleResizedEvent(object sender, System.EventArgs e)
		{
			if(!ClientRectangle.IsEmpty)
				this.Invalidate(ClientRectangle);
		}

		#endregion

		/// <summary>
		/// Overrides the base class Font property.
		/// </summary>
		/// <remarks>
		/// This property is ignored in <see cref="WinChart"/> and <see cref="ChartFrame"/>
		/// property <see cref="ChartFrame.Font"/> property is used instead. The overriden property doesn't show
		/// in property view.
		/// </remarks>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		private void HandleViewDirectionChanged(object sender, System.EventArgs e)
		{
			OnViewDirectionChanged(e);
		}

#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
		internal ChartDesigner ChartDesigner 
		{
			get {return m_chartDesigner;}
			set {m_chartDesigner = value;}
		}
#endif

		#region --- Serialization ---
		internal bool InSerialization				{ get { return m_chart.InSerialization; }  set { m_chart.InSerialization = value; } }
		private bool ShouldSerializeLegend()		{ return Legend.Visible; }

#if __INCLUDE_TO_BE_REMOVED_CANDIDATES__
		internal void StoreSeriesStyleDataToXml(string fileName)
		{
			m_chart.StoreSeriesStyleDataToXml(fileName);
		}

		internal void LoadSeriesStyleDataFromXml(string fileName)
		{
			m_chart.LoadSeriesStyleDataFromXml(fileName);
		}
#endif
		/// <summary>
		/// Base class method overriden.
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnSizeChanged ( System.EventArgs e )
		{
			base.OnSizeChanged(e);
			m_chart.TargetSize = this.Size;
		}

		#endregion


		/// <summary>
		/// Overriden base class property.
		/// </summary>
		[DefaultValue(false), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}
 
		/// <summary>
		///		The initial size of the control when it is placed on the form at design time.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(350,270);
			}
		}

		public object Clone() 
		{
			WinChart w = new WinChart();

			ChartCloner cloner = new ChartCloner();
			ChartBase chartCopy = (ChartBase)cloner.Clone(Chart);

			chartCopy.Invalidate();
			w.Chart = chartCopy;

			w.BackColor = BackColor;
			if (BackgroundImage != null)
				w.BackgroundImage = (Image)BackgroundImage.Clone();

			w.Font = Font;
			w.Text = (string)Text.Clone();
			w.ForeColor = ForeColor;

			return w;
		}


		#region --- Licence Handling ---

		internal Assembly CallingAssembly { get { return callingAssembly; } }

#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__

		[SkipObfuscation]
		internal void ToWizard(Wizard dest) 
		{
			dest.WinChart.BackColor = BackColor;
			if (BackgroundImage != null)
				dest.WinChart.BackgroundImage = (Image)BackgroundImage.Clone();

			//dest.Font = Font;
			dest.WinChart.Text = (string)Text.Clone();
			dest.WinChart.ForeColor = ForeColor;
		}

		[SkipObfuscation]
		internal void FromWizard(Wizard src) 
		{
			BackColor = src.WinChart.BackColor;
			if (src.WinChart.BackgroundImage != null)
				BackgroundImage = (Image)src.WinChart.BackgroundImage.Clone();

			//Font = dest.Font;
			Text = (string)src.WinChart.Text.Clone();
			ForeColor = src.WinChart.ForeColor;
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			timer1.Stop();
			m_chart.ReducedSamplingStep = 1;
			Invalidate();
		}
#endif

        #endregion

#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__
        #region --- Runtime Editable Prperties Handling ---

		// The list of run-time excluded properties
		string[] excludedProperties = new string[] 
		{
			"AccessibleDescription",
			"AccessibleName",
			"AccessibleRole",
			
			"Cursor",
			"AllowDrop",
			"ContextMenu",
			"Enabled",
			"ImeMode",
			"ObjectTrackingEnabled",
			"TabIndex",
			"TrackballEnabled",
			"Visible",
			"NumberOfSimulatedData",

			"DataSource",
			"InitializeOnDataBind",
			"Tag",

			"CausesValidation",
			//"Anchor",
			//"Dock",
			//"Location",
			//"Size",

			"PageSettings",
			"PrintDocument",
			"(DataBindings)"
		};

		internal void InitializeRuntimeEditablePropertyList()
		{
			InitializeRuntimeEditablePropertyList(false);
		}

		private void InitializeRuntimeEditablePropertyList(bool includeDataSource)
		{
			int i = 0;

			runtimeEditablePropertyNames =  null;
			PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this);

			// Remove excluded properties
			ArrayList included = new ArrayList();
			foreach(PropertyDescriptor pd in pdc)
			{
				bool isIncluded = true;
				for(i=0;i<excludedProperties.Length;i++)
					if(excludedProperties[i] == pd.Name)
					{
						isIncluded = false;
						break;
					}
				if(includeDataSource && pd.Name == "DataSource")
					isIncluded = true;
				if(isIncluded)
					included.Add(pd);
			}

			runtimeEditablePropertyNames = new string[included.Count];
			fullListOfRuntimeEditablePropertyNames = new string[included.Count];
			i = 0;
			foreach(PropertyDescriptor pd in included)
			{
				runtimeEditablePropertyNames[i] = pd.Name;
				fullListOfRuntimeEditablePropertyNames[i] = pd.Name;
				i++;
			}
		}

		internal void ClearRuntimeEditablePropertyList()
		{
			runtimeEditablePropertyNames = new string[0];
		}

		internal string[] RuntimeEditablePropertyNames { get { return runtimeEditablePropertyNames; } }

		private bool ListContainsString(string[] list, string str)
		{
			for(int i=0;i<list.Length; i++)
				if(list[i] == str)
					return true;
			return false;
		}

		internal void AddPropertiesForRuntimeEditing(string[] properties)
		{
			// Get the list of new names, not already in the list
			int n = 0, i;
			ArrayList newNames = new ArrayList();
			for(i=0; i<properties.Length; i++)
			{
				if(!ListContainsString(runtimeEditablePropertyNames,properties[i]) &&
					ListContainsString(fullListOfRuntimeEditablePropertyNames, properties[i]))
					newNames.Add(properties[i]);
			}

			string[] newList = new string[runtimeEditablePropertyNames.Length + newNames.Count];
			for(i = 0; i<runtimeEditablePropertyNames.Length; i++)
				newList[i] = runtimeEditablePropertyNames[i];
			for(i=0; i<newNames.Count; i++)
				newList[i+runtimeEditablePropertyNames.Length] = (string)(newNames[i]);

			runtimeEditablePropertyNames = newList;
		}

		internal void RemovePropertiesForRuntimeEditing(string[] properties)
		{
			// Compress the list to include only allowed items
			int n = 0, i, j;
			for(j=0; j<runtimeEditablePropertyNames.Length; j++)
			{
				if(ListContainsString(properties,runtimeEditablePropertyNames[j]))
				{
					runtimeEditablePropertyNames[j] = "";
					n ++;
				}
			}
			string[] newList = new string[runtimeEditablePropertyNames.Length - n];
			n = 0;
			for(i=0; i<runtimeEditablePropertyNames.Length; i++)
			{
				if(runtimeEditablePropertyNames[i] != "")
				{
					newList[n] = runtimeEditablePropertyNames[i];
					n++;
				}
			}
			runtimeEditablePropertyNames = newList;
		}

        #endregion

#endif

        #region --- Before and After Data Binding Methods ---

#if DEBUG && !__BuildingWebChart__

        [DefaultValue(null)]
		public RunTimeCompiledSource RunTimeCompiledSource
		{
			get { return m_chart.RunTimeCompiledSource; }
			set { m_chart.RunTimeCompiledSource = value; }
		}

		#endif
		#endregion
	}

#if !__BuildingWebChart__ && !__BUILDING_CRI_DESIGNER__

    internal sealed class SkipObfuscationAttribute : Attribute
    {
    }
#endif
}
