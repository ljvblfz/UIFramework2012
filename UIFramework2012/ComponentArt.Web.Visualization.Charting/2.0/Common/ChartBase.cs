using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Xml;
using ComponentArt.Web.Visualization.Charting;
using ComponentArt.Web.Visualization.Charting.Design;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Enumeration type for geometric rendering engine kinds
	/// </summary>
	public enum GeometricEngineKind
	{
		/// <summary>
		/// High image quality 3D rendering engine
		/// </summary>
		HighQualityRendering,
		/// <summary>
		/// High speed 3D rendering engine
		/// </summary>
		HighSpeedRendering
	}
    
	public enum LicenseType
	{
		Expired,
		Full
	};

	/// <summary>
	/// Chart object provides interface to the engine so that engine is not exposed as public object.
	/// 
	/// </summary>
	[TypeConverter(typeof(ExpandableWithBrowsingControlObjectConverter))]
	[Serializable()]
	internal class ChartBase : IDisposable
	{
		private bool m_inCollectionEditing = false;
		private bool inSerialization = false;
		private static int clonedCount = 0;
		private bool initializeOnDataBind = false;

		private Bitmap background = null;
		private Bitmap renderedBitmap = null;
#if __BUILDING_CRI__ || __BUILDING_CRI_DESIGNER__
        private GradientKind backGradientKind = GradientKind.None;
        private Color backGradientEndingColor = Color.White;
        private Color backColor = Color.White;
		private bool  automaticMarginsSizingToFitLabels = true;
#else
		private GradientKind backGradientKind = GradientKind.Vertical;
		private Color backGradientEndingColor = Color.Transparent;
		private Color backColor = Color.Transparent;
		private bool  automaticMarginsSizingToFitLabels = false;
#endif
		private double safetyMarginsPercentage = 10;

		private bool inDataBinding = false;
		private bool backGroundIsInputBitmap = false;

		private bool m_inWizard = false;

		private int numberOfPoints = 5;

		// Target size and sampling step
		private int reducedSamplingStep = 1;
		private Size realTargetSize = new Size(600, 400);
		private Point targetAreaLocation = new Point(0, 0);

		// Geometric RenderingEngine
#if SAFE_CODE_ONLY
        private GeometricEngineKind gEngineKind = GeometricEngineKind.HighSpeedRendering;
#else
        private GeometricEngineKind gEngineKind = GeometricEngineKind.HighQualityRendering;
#endif
		private GeometricEngine	ge = null;

		private LightingSetupCollection m_lightingSetups;
		private string m_selectedLightingSetup = "Default";

		// Text boxes
		private ChartTextBoxCollection textBoxes;
		

		// Debug
#if DEBUG
		internal static ObjectTracker objTracker = new ObjectTracker();
#endif


		[DoNotClone]
		object m_owner;

		internal object Owner
		{
			get
			{
				return m_owner;
			}
			set
			{
				m_owner = value;
			}
		}

        public ChartBase GetCopy()
        {
            ChartCloner cloner = new ChartCloner();
            object clone =  cloner.Clone(this);
            string clonerMessage = cloner.Message();

            if (clonerMessage != "")
            {
                throw new NotSupportedException("Could not clone chart: " + clonerMessage);
            }
            return clone as ChartBase;
        }


		internal int NumberOfSimulatedDataPoints { get { return numberOfPoints; } set { numberOfPoints = value; } }

        #region --- Contents ---

		// Contents Architecture

		private ChartSpace cSpace;
		private ChartFrame frame = null;
		private Point position = new Point(0, 0);
		private Rectangle targetRectangle;

		// Working mode flags

		private bool inDesignMode = false;
		private bool freezeValuePath = false;
		private bool buildValid = false;
		private bool defaultContents = true;

		// Dimensions
		private DataDimensionCollection dimensions;

		// Data Binding
		private DataProvider dataProvider;
		private ArrayList bindingErrors = new ArrayList();
		private bool needsDataBinding = true;

		// Titles
		private ChartTitleCollection titles;

		// Working graphics object
		Graphics bmg;

        #endregion

        #region --- Palette and Styles ---

		// Palettes
		private Palette palette;
		private PaletteCollection palettes;
		private string selectedPalette = "Default";

		// Text styles
		private TextStyleCollection textStyles;

		// Label styles
		private LabelStyleCollection labelStyles;
		private DataPointLabelStyleCollection dataPointLabelStyles;

		// Line Styles
		private LineStyleCollection lineStyles;
		private LineStyle2DCollection lineStyles2D;

		// Presentation Styles
		private SeriesStyleCollection presentationStyles;

		// Gradient Styles
		private GradientStyleCollection gradientStyles;

		// Marker Styles
		private MarkerStyleCollection markerStyles;

		// TickMark Styles
		private TickMarkStyleCollection tickMarkStyles;

		// TextBox Styles
		private TextBoxStyleCollection textBoxStyles;
        #endregion

		public static ResourceManager ResourceMngr; 

        #region --- Constructor and Initialization ---

		static ChartBase()
		{
			ResourceMngr = new ResourceManager("ComponentArt.Web.Visualization.Charting.Common.Charting", (typeof(ChartBase)).Assembly);
		}

		public ChartBase()
		{
			background = null;

			dataProvider = new DataProvider();
			dataProvider.OwningChart = this;

			dimensions = new DataDimensionCollection(this);
			cSpace = new ChartSpace(this);
			textBoxes = new ChartTextBoxCollection(this);

			InitializeStyles();
			InitializeContents();
		}

		public void Dispose()
		{
			if (background != null)
			{
				background.Dispose();
				background = null;
			}
			if (renderedBitmap != null)
			{
				renderedBitmap.Dispose();
				renderedBitmap = null;
			}
			if (bmg != null)
			{
				bmg.Dispose();
				bmg = null;
			}
			if (cSpace != null)
			{
				cSpace.Dispose();
				cSpace = null;
			}
			foreach (TextStyle style in TextStyles)
			{
				style.Dispose();
			}
			TextStyles.Clear();
			foreach (LineStyle2D ls in LineStyles2D)
			{
				ls.Dispose();
			}
			LineStyles2D.Clear();

		}

        #region --- Initialization of Styles and Contents ---

		// Initialization is done at the chart construction.
		// Style initialization 
		//		- initializes style collections
		//		- creates default styles
		//		- initialize palette collection
		//		- creates default palettes
		//
		// Content initialization
		//		- initializes dimension collection
		//		- creates standard dimensions
		//		- creates the "ChartSpace" object
		//

		private void InitializeStyles()
		{
			palettes = new PaletteCollection();
			palettes.SetOwner(this);
			textStyles = new TextStyleCollection(this,true);
			lineStyles = new LineStyleCollection(this, true);
			lineStyles2D = new LineStyle2DCollection(this, true);
			labelStyles = new LabelStyleCollection(this,true);
			presentationStyles = new SeriesStyleCollection(this);
			dataPointLabelStyles = new DataPointLabelStyleCollection(this, true);
			gradientStyles = new GradientStyleCollection(this);
			markerStyles = new MarkerStyleCollection(this, true);
			tickMarkStyles = new TickMarkStyleCollection(this, true);
			m_lightingSetups = new LightingSetupCollection(this, true);
			textBoxStyles = new TextBoxStyleCollection(this,true);
		}

		//		--- Contents ---
		internal bool NeedsCreationOfInitialContents
		{ get { return cSpace.NeedsCreationOfInitialContents; } set { cSpace.NeedsCreationOfInitialContents = value; } }

		private void InitializeContents()
		{
			SetupStandardDimensions();
		}

		private void SetupStandardDimensions()
		{
			DataDimension.StandardNumericDimension = new NumericDataDimension("StandardNumericDimension");
			DataDimension.StandardIndexDimension = new IndexDataDimension("StandardIndexDimension");
			DataDimension.StandardDateTimeDimension = new DateTimeDataDimension("StandardDateTimeDimension");
			DataDimension.StandardBooleanDimension = new EnumeratedDataDimension("StandardBooleanDimension");
			DataDimension.StandardBooleanDimension.Add(false);
			DataDimension.StandardBooleanDimension.Add(true);
			dimensions.Add(DataDimension.StandardNumericDimension);
			dimensions.Add(DataDimension.StandardIndexDimension);
			dimensions.Add(DataDimension.StandardDateTimeDimension);
			dimensions.Add(DataDimension.StandardBooleanDimension);
			DataDimension.StandardNumericDimension.IsDefault = true;
			DataDimension.StandardIndexDimension.IsDefault = true;
			DataDimension.StandardDateTimeDimension.IsDefault = true;
			DataDimension.StandardBooleanDimension.IsDefault = true;
		}

        #endregion

        #region --- Build ---

		internal void Build()
		{
			if (buildValid)
				return;
			buildValid = false;
			if (bindingErrors.Count > 0)
				return;
			try
			{
				cSpace.Build();
			}
			catch (Exception ex)
			{
				RegisterErrorMessage(ex.Message);
			}
			buildValid = true;
			defaultContents = false;
		}

        #endregion

		internal void SetDesignMode(bool dm)
		{
			if (dm == inDesignMode)
				return;
			inDesignMode = dm;
			if (inDesignMode)
				buildValid = false;		// in design mode we always build before rendering
			else
				defaultContents = false; // we add default contents in design mode only
		}
        #endregion

		#region --- Geometric RenderingEngine ---

		internal GeometricEngineKind GeometricEngineKind 
		{
			get 
			{
				return gEngineKind; 
			}
			set 
			{
				if(gEngineKind != value)
				{
                    cSpace.GE = null;
					gEngineKind = value; 
				}
			}
		}

        internal GeometricEngine CreateGeometricEngine()
        {
#if SAFE_CODE_ONLY
            return new Geometry.GeometricEngineBasedOnRenderingOrder(this);
#else
			if(gEngineKind == GeometricEngineKind.HighQualityRendering)
				return new ComponentArt.Web.Visualization.Charting.Geometry.HighQualityRendering.HighQualityGeometricEngine(this);
			else
				return new GeometricEngineBasedOnRenderingOrder(this);
#endif
		}

		internal GeometricEngine GeometricEngine     
		{
			get 
			{
                if(cSpace.GE == null)
                    cSpace.GE = CreateGeometricEngine();
				return cSpace.GE;
			}
		}

		#endregion
        #region --- Variables and Data Binding ---
        /// <summary>
        /// Defining value of input variable.
        /// </summary>
        /// <param name="name">Input variable name</param>
        /// <param name="obj">Value of the input variable</param>
		public void DefineValue(string name, object obj)
		{
			dataProvider.DataBind(name, obj);
			buildValid = false;
			needsDataBinding = true;
		}

        /// <summary>
        /// Defining input variable using expression.
        /// </summary>
        /// <param name="name">Input variable name</param>
        /// <param name="expression">Expression</param>
		public void DefineAsExpression(string name, string expression)
		{
			dataProvider.DefineAsExpression(name, expression);
			buildValid = false;
			needsDataBinding = true;
		}
        /// <summary>
        /// Defining custom dimension
        /// </summary>
		public void DefineDimension(string name, DataDimension dimension)
		{
			dataProvider.DimensionBind(name, dimension);
			buildValid = false;
			needsDataBinding = true;
		}

        /// <summary>
        /// Defining input variable using value path.
        /// </summary>
        /// <param name="name">Input variable name</param>
        /// <param name="expression">Expression</param>
        public void DefineValuePath(string name, string valuePath)
		{
			dataProvider.DefineValuePath(name, valuePath);
			buildValid = false;
			needsDataBinding = true;
		}

        #if __BUILDING_CRI_DESIGNER__

        internal void DefineAsRSExpression(string name, string rsExpression)
        {
            dataProvider.DefineAsRSExpression(name, rsExpression);
            buildValid = false;
            needsDataBinding = true;
        }
        #endif

		internal object DataSource
		{
			get { return dataProvider.DataSource; }
			set
			{
                if (dataProvider.DataSource != value)
                {
                    dataProvider.DataSource = value;
                    buildValid = false;
                    needsDataBinding = true;
                }
			}
		}

		internal bool NeedsDataBinding { get { return needsDataBinding; } }

#if __BuildingWebChart__
        bool m_allowOnLoadDataBinding = true;

        internal bool AllowOnLoadDataBinding
        {
            get
            {
                return m_allowOnLoadDataBinding;
            }
        }

		public bool InitializeOnDataBind 
		{
			get { return initializeOnDataBind; }
		}

#else
		public bool InitializeOnDataBind
		{
			set { initializeOnDataBind = value; }
			get { return initializeOnDataBind; }
		}

#endif
        /// <summary>
        /// Internal data binding method
        /// </summary>
		public void DataBind()
		{
#if __BuildingWebChart__
            m_allowOnLoadDataBinding = false;
#endif
			bindingErrors.Clear();

			inDataBinding = true;

			try
			{
				this.Series.RegisterVariables();
				dataProvider.DataBind();
				cSpace.DataBind();
				Legend.Clear();
				cSpace.FillLegend(Legend);
			}
			catch (Exception e)
			{
				RegisterErrorMessage(e.Message);
				Debug.WriteLine(e.Message + "\n" + e.StackTrace);
			}
			inDataBinding = false;
			needsDataBinding = false;

		}

		internal void RegisterErrorMessage(string message)
		{
			if (message.IndexOf("\n") > 0)
			{
				string[] parts = message.Split('\n');
				foreach (string m in parts)
					bindingErrors.Add(m);
			}
			else
				bindingErrors.Add(message);
		}
		internal bool HasErrors { get { return bindingErrors.Count > 0; } }
		internal string ErrorMessage 
		{
			get
			{
				if(bindingErrors.Count == 0)
					return null;
				string msg = "";
				for(int i=0; i<bindingErrors.Count; i++)
				{
					string line = (string)bindingErrors[i];
					msg = msg + line;
				}
				return msg;
			}
		}

		internal bool InDataBinding { get { return inDataBinding; } }

		internal string[] DataSourcePaths
		{
			get { return dataProvider.DataSourcePaths; }
		}

        #endregion

        #region --- Presentation ---

		internal CompositionKind CompositionKind { get { return cSpace.Series.CompositionKind; } set { cSpace.Series.CompositionKind = value; } }
		internal CompositeSeries Series
		{
			get { return cSpace.Series; }
			set { cSpace.Series = value; }
		}
        #endregion
        #region --- Coordinate Systems ---
		internal int XAxisStartPixel
		{
			get 
			{
				AxisAnnotationCollection aac = Series.CoordSystem.XAxis.AxisAnnotations;
				foreach(AxisAnnotation aa in aac)
				{
					if(aa.Visible)
					{
						return (int)(Series.TargetArea.Mapping.Map(aa.WCSCoordinate(aa.Axis.MinValueICS)).X);
					}
				}
				return 0;
			}
		}
		internal int XAxisEndPixel
		{
			get 
			{
				AxisAnnotationCollection aac = Series.CoordSystem.XAxis.AxisAnnotations;
				foreach(AxisAnnotation aa in aac)
				{
					if(aa.Visible)
					{
						return (int)(Series.TargetArea.Mapping.Map(aa.WCSCoordinate(aa.Axis.MaxValueICS)).X);
					}
				}
				return 1;
			}
		}

        #endregion

        #region --- Mapping ---

			public Mapping Mapping { get { return cSpace.Mapping; } set { cSpace.Mapping = value; } }

		/// <summary>
		/// Sets or gets automatic margins resizing to fit axis labels.
		/// </summary>
		[Description("Resize margins to make room for axis labels")]		
		public bool ResizeMarginsToFitLabels { get { return automaticMarginsSizingToFitLabels; } set { automaticMarginsSizingToFitLabels = value; } }

		/// <summary>
		/// Sets or gets safety margins percentage.
		/// </summary>
		/// <remarks>
		/// This value defines safety margine between axis labels and border of the image.
		/// </remarks>
		public double SafetyMarginsPercentage { get { return safetyMarginsPercentage; } set { safetyMarginsPercentage = value; } }

		[Browsable(false)]
		public double Dpi { get { return cSpace.Dpi; } set { cSpace.Dpi = value; } }
		[Browsable(false)]
		public double FromWorldToTarget { get { return MetricsBaseMapping.Enlargement; } }
		[Browsable(false)]
		public double FromTargetToWorld { get { return 1.0 / MetricsBaseMapping.Enlargement; } }
		[Browsable(false)]
		public double FromPointToTarget { get { return MetricsBaseMapping.DPI / 72.0; } }
		[Browsable(false)]
		public double FromPointToWorld { get { return FromPointToTarget * FromTargetToWorld; } }
		public Size NativeSize
		{
			get
			{
				return Mapping.NativeSize;
			}
			set
			{
				if (Mapping.NativeSize != value)
				{
					Mapping.NativeSize = value;
					SetCoordinates();
					cSpace.NativeSizeChanged();
					buildValid = false;
				}
			}
		}

		[Browsable(false)]
		public double ScaleToNativeSize { get { return Math.Min((double)TargetSize.Width / NativeSize.Width, (double)TargetSize.Height / NativeSize.Height); } }

		private Mapping MetricsBaseMapping // Mapping used for graphics object sizing
		{
			get
			{
				if (Series.CompositionKind == CompositionKind.MultiArea)
				{
					return FindMetricsBaseMapping(Series);
				}
				else
					return cSpace.Mapping;
			}
		}

		private Mapping FindMetricsBaseMapping(SeriesBase s)
		{
			if (s is Series)
				return s.TargetArea.Mapping;
			CompositeSeries cs = s as CompositeSeries;
			if (cs.CompositionKind != CompositionKind.MultiArea)
				return cs.TargetArea.Mapping;

			// Here we go backwards since the subseries are rendered backwards,
			// therefore their mappings are properly setup in that order
			for (int i = cs.SubSeries.Count - 1; i >= 0; i--)
			{
				Mapping m = FindMetricsBaseMapping(cs.SubSeries[i]);
				if (m != null)
					return m;
			}
			return null;
		}

		public Vector3D DomainSize
		{
			get
			{
				return Mapping.DomainSize;
			}
			set
			{
				if (Mapping.DomainSize != value)
				{
					Mapping.DomainSize = value;
					SetCoordinates();
					cSpace.DomainSizeChanged();
					buildValid = false;
				}
			}
		}

		public PointF Map2D(Vector3D worldPoint)
		{
			Vector3D targetPoint;
			Mapping.Map(worldPoint, out targetPoint);
			return new PointF((float)targetPoint.X, (float)targetPoint.Y);
		}

		public Vector3D Map(Vector3D worldPoint)
		{
			Vector3D targetPoint;
			Mapping.Map(worldPoint, out targetPoint);
			return targetPoint;
		}

		public void SetCoordinates()
		{
			cSpace.SetCoordinates();
		}

        #endregion
        #region --- Lights ---

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal LightCollection Lights
		{
			get
			{
				return LightingSetups[this.SelectedLightingSetup].Lights;
			}
		}

		internal string SelectedLightingSetup
		{
			get { return m_selectedLightingSetup; }
			set { m_selectedLightingSetup = value; }
		}

		// Used by the wrapper control class.
		internal LightingSetupCollection LightingSetups
		{
			get
			{
				if (InSerialization)
				{
					LightingSetupCollection C = new LightingSetupCollection();
					C.Clear();
					foreach (LightingSetup S in m_lightingSetups)
						if (S.HasChanged)
							C.Add(S);
					return C;
				}
				else
					return m_lightingSetups;
			}
		}

        #endregion
        #region --- Palettes ---

		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		internal PaletteCollection Palettes
		{
			get
			{
				return this.palettes;
			}
		}

		internal string SelectedPalette
		{
			get { return selectedPalette; }
			set { palette = null; selectedPalette = value; }
		}

		public Palette Palette
		{
			get
			{
				if (palette != null)
					return palette;
				if (selectedPalette == "")
					selectedPalette = "Default";
				palette = Palettes[selectedPalette];
				if (palette != null)
					return palette;
				if (Palettes.Count == 0)
					return null;
				palette = Palettes[0];
				selectedPalette = palette.Name;
				return palette;
			}
		}

        #endregion
        #region --- Text Styles ---

		internal TextStyleCollection TextStyles
		{
			get
			{
				if (InSerialization)
				{
					TextStyleCollection C = new TextStyleCollection(this);
					foreach (TextStyle S in textStyles)
						if (S.ShouldSerializeMe)
							C.Add(S);
					return C;
				}
				else
					return textStyles;
			}
		}

		internal TickMarkStyleCollection TickMarkStyles
		{
			get
			{
				if (InSerialization)
				{
					TickMarkStyleCollection outCollection = new TickMarkStyleCollection(this, false);
					foreach (TickMarkStyle tm in tickMarkStyles)
					{
						if (tm.HasChanged)
							outCollection.Add(tm);
					}
					return outCollection;
				}
				else
					return tickMarkStyles;
			}
			set
			{
				tickMarkStyles = value;
			}
		}

		internal TextBoxStyleCollection TextBoxStyles
		{
			get
			{
				if (InSerialization)
				{
					TextBoxStyleCollection outCollection = new TextBoxStyleCollection(this, false);
					foreach (TextBoxStyle tm in textBoxStyles)
					{
						if (tm.Changed)
							outCollection.Add(tm);
					}
					return outCollection;
				}
				else
					return textBoxStyles;
			}
			set
			{
				textBoxStyles = value;
			}
		}

		public TextStyle GetTextStyle(string name)
		{
			return TextStyles[name] as TextStyle;
		}

		public TextStyle CreateTextStyle(string name, string fontName, double pointSize, FontStyle style)
		{
			Font font = new Font(fontName, (float)pointSize, style);
			return CreateTextStyle(name, font);
		}

		public TextStyle CreateTextStyle(string name, Font font)
		{
			TextStyle cFont = new TextStyle(name, font, Color.Black, Color.Black, 0, ReverseSideStyle.FlipReverseSide,
				ReverseDirectionStyle.FlipReverseDirection, false);
			cFont.SetOwningChart(this);
			textStyles.Add(cFont);
			return cFont;
		}

		public TextStyle CreateTextStyle(string name, Font font, Color foregroundColor, Color shadowColor, double shadowDepthPxl)
		{
			TextStyle cFont = new TextStyle(name, font, foregroundColor, shadowColor, shadowDepthPxl, ReverseSideStyle.FlipReverseSide,
				ReverseDirectionStyle.FlipReverseDirection, false);
			cFont.SetOwningChart(this);
			textStyles.Add(cFont);
			return cFont;
		}

		public TextStyle CreateTextStyle(string name)
		{
			return CreateTextStyle(name, "Arial", 10, FontStyle.Regular);
		}

		public TextStyle CloneTextStyle(string name, string sourceName)
		{
			TextStyle src = GetTextStyle(sourceName);
			if (src == null)
			{
				Message("Text style '" + sourceName + "' doesn't exist");
				src = GetTextStyle("Default");
			}
			TextStyle style = new TextStyle(name, src.Font, src.ForeColor, src.ShadowColor, src.ShadowDepthPxl,
				src.RevSideStyle, src.RevDirectionStyle, src.Is2D);
			style.SetOwningChart(this);
			textStyles.Add(style);
			return style;
		}
        #endregion
        #region --- Gradient Styles ---

		internal GradientStyleCollection GradientStyles
		{
			get
			{
				if (InSerialization)
				{
					GradientStyleCollection outCollection = new GradientStyleCollection(this, false);
					foreach (GradientStyle gs in gradientStyles)
					{
						if (gs.HasChanged)
							outCollection.Add(gs);
					}
					return outCollection;
				}
				else
					return gradientStyles;
			}
		}

		internal GradientStyle FindOrCreateGradientStyle(string gradientStyleName)
		{
			GradientStyle gStyle = gradientStyles[gradientStyleName];
			if (gStyle == null)
			{
				Color color = Palette.NextColor;
				gStyle = new GradientStyle(gradientStyleName, GradientKind.None, color, color);
				gradientStyles.Add(gStyle);
			}
			return gStyle;
		}

        #endregion
        #region --- Marker Styles ---

		internal MarkerStyleCollection MarkerStyles
		{
			get
			{
				if (InSerialization)
				{
					MarkerStyleCollection C = new MarkerStyleCollection(this);
					foreach (MarkerStyle S in markerStyles)
						if (!S.IsDefault)
							C.Add(S);
					return C;
				}
				else
					return markerStyles;
			}
		}

        #endregion
        #region --- Label Styles ---

		internal LabelStyleCollection LabelStyles
		{
			get
			{
				if (InSerialization)
				{
					LabelStyleCollection C = new LabelStyleCollection(this);
					foreach (LabelStyle S in labelStyles)
						if (S.ShouldSerializeMe)
							C.Add(S);
					return C;
				}
				else
					return labelStyles;
			}
		}

		internal LineStyleCollection LineStyles
		{
			get { return this.lineStyles; }
		}

		internal LineStyle2DCollection LineStyles2D
		{
			get { return this.lineStyles2D; }
		}

		public LabelStyle GetLabelStyle(string name)
		{
			return LabelStyles[name] as LabelStyle;
		}

		public LabelStyle CreateLabelStyle(string name, TextOrientation textOrientation,
			TextReferencePoint textReferencePoint, double hOffset, double vOffset)
		{
			LabelStyle ls = new LabelStyle(name, textOrientation, textReferencePoint, hOffset, vOffset, 0.0, 0.5).MarkCreatedInternally();
			ls.SetOwningChart(this);
			labelStyles.Add(ls);
			return ls;
		}

		public LabelStyle CreateLabelStyle(string name, Vector3D Vh, Vector3D Vv,
			TextReferencePoint textReferencePoint, double hOffset, double vOffset)
		{
			LabelStyle ls = new LabelStyle(name, Vh, Vv, textReferencePoint, hOffset, vOffset, 0.0, 0.5).MarkCreatedInternally();
			ls.SetOwningChart(this);
			labelStyles.Add(ls);
			return ls;
		}

		public LabelStyle CloneLabelStyle(string name, string sourceName)
		{
			LabelStyle src = GetLabelStyle(sourceName);
			if (src == null)
			{
				Message("Label style '" + sourceName + "' does not exist");
				src = GetLabelStyle("Default");
			}
			LabelStyle style = new LabelStyle(name, src.HorizontalDirection, src.VerticalDirection,
				src.ReferencePoint, src.HOffset, src.VOffset, src.Angle, src.LiftZ).MarkCreatedInternally();
			style.SetOwningChart(this);
			labelStyles.Add(style);
			return style;
		}
        #endregion
        #region --- DataPointlabelStyle ---

		internal DataPointLabelStyleCollection DataPointLabelStyles
		{
			get { return dataPointLabelStyles; }
		}

		public DataPointLabelStyle GetDataPointLabelStyle(string name)
		{
			return DataPointLabelStyles[name] as DataPointLabelStyle;
		}

		public DataPointLabelStyle CreateDataPointLabelStyle(string name)
		{
			DataPointLabelStyle dpls = new DataPointLabelStyle(name, new Vector3D(0.5, 0.5, 1.0));
			dataPointLabelStyles.Add(dpls);
			return dpls;
		}

		public DataPointLabelStyle CreateDataPointLabelStyle()
		{
			DataPointLabelStyle dpls = new DataPointLabelStyle("", new Vector3D(0.5, 0.5, 1.0));
			dpls.SetNextDefaultName();
			dataPointLabelStyles.Add(dpls);
			return dpls;
		}

		public DataPointLabelStyle CloneDataPointLabelStyle(string name, string sourceName)
		{
			DataPointLabelStyle src = GetDataPointLabelStyle(sourceName);
			if (src == null)
			{
				Message("DataPointLabelStyle '" + sourceName + "' doesn't exist");
				src = GetDataPointLabelStyle("Default");
			}
			DataPointLabelStyle style = new DataPointLabelStyle(name, src.PieLabelPosition,
				src.LineStyle, src.RelativeLine1Start, src.RelativeLine1Length, src.RelativeLine2Length,
				src.PixelsToLabel, src.RelativeOffsetOfAlignedLabels);
			style.SetOwningChart(this);
			dataPointLabelStyles.Add(style);
			return style;
		}

        #endregion
        #region --- Line Styles ---

		public LineStyle GetLineStyle(string lineStyleName)
		{
			return lineStyles[lineStyleName] as LineStyle;
		}

        public LineStyle CreateFlatLineStyle(string lineStyleName, ChartColor surface, double width)
		{
			LineStyle LS = new FlatLineStyle(lineStyleName, surface, width * FromPointToTarget * FromTargetToWorld);
			lineStyles.Add(LS);
			return LS;
		}

        public LineStyle CreateStripLineStyle(string lineStyleName, ChartColor surface, double width)
		{
			LineStyle LS = new BlockLineStyle(lineStyleName, surface, 1, width);
			lineStyles.Add(LS);
			return LS;
		}

        public LineStyle CreateBlockLineStyle(string lineStyleName, ChartColor surface, double width, double height)
		{
			LineStyle LS = new BlockLineStyle(lineStyleName, surface, width, height);
			lineStyles.Add(LS);
			return LS;
		}

        public LineStyle CreateBlockLineStyle(string lineStyleName, ChartColor surface, double width)
		{
			return CreateBlockLineStyle(lineStyleName, surface, width, width);
		}

        public LineStyle CreatePipeLineStyle(string lineStyleName, ChartColor surface, double width)
		{
			LineStyle LS = new PipeLineStyle(lineStyleName, surface, width);
			lineStyles.Add(LS);
			return LS;
		}

        public LineStyle CreateDashLineStyle(string lineStyleName, string style, double width1, double width2)
		{
			return CreateDashLineStyle(lineStyleName, style, "NoLine", width1, width2);
		}

		public LineStyle CreateDashLineStyle(string lineStyleName, string style1, string style2, double width1, double width2)
		{
			return CreateDashLineStyle(lineStyleName, new string[] { style1, style2 }, new double[] { width1, width2 });
		}

		public LineStyle CreateDashLineStyle(string lineStyleName, string[] styles, double[] widths)
		{
			LineStyle LS = new DashLineStyle(lineStyleName, styles, widths);
			lineStyles.Add(LS);
			return LS;
		}

		public LineStyle CreateDashLineStyle(string lineStyleName, string baseStyleName, double[] widths)
		{
			int N = (widths.Length % 2) * 2;
			LineStyle LS0 = new NoLineStyle();
			string[] styles = new string[N];
			for (int i = 0; i < N; i++)
				styles[i] = (i % 2 == 0) ? baseStyleName : "NoLine";
			LineStyle LS = new DashLineStyle(lineStyleName, styles, widths);
			lineStyles.Add(LS);
			return LS;
		}

		public LineStyle CreateDotLineStyle(string lineStyleName, ChartColor surface, double width, double relativeDistance)
		{
			LineStyle LS = new DotLineStyle(lineStyleName, surface, relativeDistance, width);
			return LS;
		}
		public LineStyle CreateDotLineStyle(string lineStyleName, ChartColor surface, double width)
		{
			LineStyle LS = new DotLineStyle(lineStyleName, surface, 1.5, width);
			lineStyles.Add(LS);
			return LS;
		}
		public LineStyle CreateDashDotLineStyle(string lineStyleName, string baseStyleName, ChartColor dotSurface, double dotWidth, double relativeDash, double relativeDistance)
		{
			LineStyle DLS = new DotLineStyle(lineStyleName + ".dot", dotSurface, 0.01, dotWidth);
			lineStyles.Add(DLS);
			string[] styles = new string[4];
			double[] widths = new Double[4];
			styles[0] = baseStyleName;
			styles[1] = "NoLine";
			styles[2] = lineStyleName + ".dot";
			styles[3] = "NoLine";
			widths[0] = dotWidth * relativeDash;
			widths[1] = dotWidth * relativeDistance;
			widths[2] = dotWidth * 0.01;
			widths[3] = dotWidth * relativeDistance;
			LineStyle LS = new DashLineStyle(lineStyleName, styles, widths);
			lineStyles.Add(LS);
			return LS;
		}
		public LineStyle CreateDoubleLineStyle(string lineStyleName, string style1, string style2)
		{
			LineStyle LS = new MultiLineStyle(lineStyleName, new string[] { style1, style2 });
			lineStyles.Add(LS);
			return LS;
		}

		public LineStyle CreateMultiLineStyle(string lineStyleName, string[] styles)
		{
			LineStyle LS = new MultiLineStyle(lineStyleName, styles);
			lineStyles.Add(LS);
			return LS;
		}

		public LineStyle CloneLineStyle(string name, string sourceName)
		{
			LineStyle src = GetLineStyle(sourceName);
			if (src == null)
			{
				Message("Line style '" + name + "' doesn't exist");
				src = GetLineStyle("Default");
			}

			LineStyle style;

			StyleCloner csc = new StyleCloner();

			style = (LineStyle)csc.Clone(src);

			lineStyles.Add(style);
			return style;
		}
        #endregion
        #region --- Chart Styles ---

		internal SeriesStyleCollection SeriesStyles
		{
			get { return presentationStyles; }
		}

		public SeriesStyle GetPresentationStyle(string styleName)
		{
			return presentationStyles[styleName] as SeriesStyle;
		}

        #endregion
        #region --- Properties ---

		internal Vector3D ViewDirection { get { return Mapping.ViewDirection; } set { Mapping.ViewDirection = value; } }
		internal double PerspectiveFactor { get { return Mapping.PerspectiveFactor; } set { Mapping.PerspectiveFactor = value; } }

		internal bool InDesignMode { get { return inDesignMode; } }
		internal bool FreezeValuePath { get { return freezeValuePath; } set { freezeValuePath = value; } }
		internal bool InSerialization { get { return inSerialization; } set { inSerialization = value; } }
		internal bool InCollectionEditing { get { return m_inCollectionEditing; } set { m_inCollectionEditing = value; } }
		internal bool DefaultContents { get { return defaultContents; } }
		internal ChartTitleCollection Titles
		{
			get { return cSpace.Series.TargetArea.Titles; }
		}

		internal ChartSpace ChartSpace
		{
			get { return cSpace; }
			set { cSpace = value; cSpace.SetOwningChart(this); }
		}

		internal Graphics WorkingGraphics { get { return bmg; } }

		internal DataDimensionCollection DataDimensions { get { return dimensions; } }

		internal Point TargetAreaLocation { get { return targetAreaLocation; } set { targetAreaLocation = value; } }

		public Size TargetSize
		{
			get { return realTargetSize; }
			set
			{
				if (realTargetSize != value)
				{
					realTargetSize = value;
					cSpace.Mapping.TargetSize = EffectiveTargetSize;
					if (!cSpace.Mapping.TargetSize.IsEmpty)
					{
						double dpi = 96.0 * Math.Max(
							((double)cSpace.Mapping.TargetSize.Width) / NativeSize.Width,
							((double)cSpace.Mapping.TargetSize.Height) / NativeSize.Height);
						Mapping.DPI = dpi / reducedSamplingStep;
					}
					if(cSpace.GE != null)
						SetCoordinates();
					cSpace.TargetSizeChanged();
				}
			}
		}

		public Size EffectiveTargetSize { get { return new Size(realTargetSize.Width / reducedSamplingStep, realTargetSize.Height / reducedSamplingStep); } }
		internal int ReducedSamplingStep { get { return reducedSamplingStep; } set { reducedSamplingStep = value; } }
		internal void SetAutoReducedSamplingStep()
		{
			int n = Math.Max(realTargetSize.Width, realTargetSize.Height) / 400 + 1;
			ReducedSamplingStep = Math.Max(1, n);
			ReducedSamplingStep = 1;
		}

		internal InputVariableCollection InputVariables
		{
			get
			{
				if (InSerialization)
				{
					bool seriesInInputData = DataProvider.SeriesNameInInputData;
					InputVariableCollection ivc = new InputVariableCollection(dataProvider.InputVariables.Owner);
					foreach (InputVariable iv in dataProvider.InputVariables)
					{
						// Skip variables with series name prefix if series is in input data
						if(seriesInInputData && iv.Name.IndexOf(":")>=0)
							continue;
						if ((iv.ValuePath != null && iv.ValuePath != "") ||
							(iv.ValueExpression != null && iv.ValueExpression != "") ||
							!iv.CreatedByControl || iv.HasChanged)
						{
							ivc.Add(iv);
						}
					}
					return ivc;
				}
				return dataProvider.InputVariables;
			}
		}

		internal DataProvider DataProvider { get { return dataProvider; } }

		internal Color BackColor
		{
			get { return backColor; }
			set
			{
				if (backColor == value)
					return;
				backColor = value;
			}
		}
		internal Color BackGradientEndingColor { get { return backGradientEndingColor; } set { backGradientEndingColor = value; } }
		internal GradientKind BackGradientKind { get { return backGradientKind; } set { backGradientKind = value; } }

		public Bitmap Background
		{
			get
			{
				if (!backGroundIsInputBitmap) // Create from back color
				{
					Bitmap bmp = new Bitmap(Mapping.TargetSize.Width, Mapping.TargetSize.Height);
					Graphics gBmp = Graphics.FromImage(bmp);
					// Efective gradient colors
					Color c0 = backColor;
					Color c1 = backGradientEndingColor;
					if (c0.A == 0)
						c0 = Palette.BackgroundColor;
					if (c1.A == 0)
						c1 = Palette.BackgroundEndingColor;
					if (backGradientKind == GradientKind.None)
					{
						gBmp.Clear(c0);
					}
					else
					{
						GradientStyle gs = new GradientStyle("bgd", backGradientKind, c0, c1);
						Rectangle rect = new Rectangle(new Point(0, 0), TargetSize);
						Brush brush = gs.CreateBrush(rect);
						gBmp.FillRectangle(brush, rect);
					}
					gBmp.Dispose();
					if (background != null)
						background.Dispose();
					background = bmp;
					return bmp;
				}
				else
				{
					return background;
				}
			}
			set
			{
				backGroundIsInputBitmap = false;
				if (background != null)
					background.Dispose();
				if (value != null)
				{
					backGroundIsInputBitmap = true;
					background = value;
				}
				else
					background = null;
			}
		}

		public Legend Legend
		{
			get { return cSpace.Series.TargetArea.Legend; }
			set { cSpace.Series.TargetArea.Legend = value; }
		}

		public LegendCollection SecondaryLegends
		{
			get { return cSpace.Series.TargetArea.SecondaryLegends; }
			set { cSpace.Series.TargetArea.SecondaryLegends = value; }
		}

		internal double RenderingPrecision { get { return cSpace.RenderingPrecision; } set { cSpace.RenderingPrecision = value; } }


        #endregion
        #region --- Build and Rendering ---

		public void Invalidate()
		{
			buildValid = false;
		}

		bool m_schemaExists = true;
		internal bool SchemaExists
		{
			get { return m_schemaExists; }
			set { m_schemaExists = value; }
		}

		internal Rectangle TargetRectangle { get { return targetRectangle; } }

		internal Rectangle GetTargetRectangle(Graphics g)
		{
			if (frame != null)
			{
				frame.Rectangle = new Rectangle(TargetAreaLocation, TargetSize);
				frame.SetScaleFactor(FromPointToTarget * reducedSamplingStep);
				targetRectangle = frame.InternalRectangle(g);
				// Fix the offset and size
				targetRectangle.X--;
				targetRectangle.Y--;
				targetRectangle.Width += 2;
				targetRectangle.Height += 2;
				targetRectangle.Width = Math.Min(targetRectangle.Width, TargetSize.Width);
				targetRectangle.Height = Math.Min(targetRectangle.Height, TargetSize.Height);
			}
			else
				targetRectangle = new Rectangle(TargetAreaLocation, TargetSize); ;
			return targetRectangle;
		}

		internal TimeSpan DurationDataBind { get { return cSpace.DurationDataBind; } }
		internal TimeSpan Duration3DBmp { get { return cSpace.Duration3DBmp; } }
		internal TimeSpan Duration2DBmp { get { return cSpace.Duration2DBmp; } }
		internal TimeSpan DurationMapping { get { return cSpace.DurationMapping; } }

		public void Render(Graphics g)
		{
			if ((needsDataBinding || InDesignMode) && !buildValid)
				DataBind();
			if (bindingErrors.Count > 0)
			{
				g.Clear(Color.White);
				RenderBindingErrors(g);
				return;
			}

			if (!Mapping.TargetSize.IsEmpty)
			{
				if (cSpace.Series.NumberOfSimpleSeries == 0 && cSpace.Series.SubSeries.Count > 0)
				{
					g.DrawImage(Background, position.X, position.Y);
					g.DrawString("No Data", new Font("Arial", 14, FontStyle.Bold), Brushes.Gray, 20, 20);
					return;
				}

				Series.HandleMissingDataPoints();

				bmg = g;
				Rectangle tRect = GetTargetRectangle(g);
				position = tRect.Location;
				TargetSize = tRect.Size;
				if(backGroundIsInputBitmap && background!=null && TargetSize != background.Size)
				{
					// Adjust size of the input bitmap to the target size
					Bitmap newBackground = new Bitmap(background,TargetSize);
					background.Dispose();
					background = newBackground;
				}
				if (!buildValid)
				{
					cSpace.Invalidate();
					Build();
					buildValid = true;
					DebugLeakingDetector leakDec1 = new DebugLeakingDetector();

					renderedBitmap = cSpace.Render(g, position, Background);
					leakDec1.ReportLeak();
				}

				g.DrawImage(renderedBitmap, tRect);
				RenderTextBoxes();
				Series.RenderTitles();
				Series.RenderLegends();

				if (frame != null)
					frame.Render(bmg);

				RenderBindingErrors(g);
			}
			bmg = null;
		}

		#region --- Handling Text Boxes ---

		private void RenderTextBoxes()
		{
			Canvace canvace = GeometricEngine.CreateCanvace();
			foreach(ChartTextBox ctb in textBoxes)
				ctb.Render(canvace);
			canvace.Dispose();
		}
        /// <summary>
        /// Charting internal creation of text box
        /// </summary>
		public ChartTextBox CreateTextBox(string styleName, string text, TextAnchor point)
		{
			TextBoxStyle style = textBoxStyles[styleName];
			ChartTextBox textBox = new ChartTextBox(text,point,styleName,
				style.DefaultDxPts,style.DefaultDyPts,style.MaxBoxWidth);
			textBoxes.Add(textBox);
			return textBox;
		}
		#endregion

		internal bool InWizard
		{
			get
			{
				return m_inWizard;
			}
			set
			{
				m_inWizard = value;
			}
		}


		private void RenderBindingErrors(Graphics g)
		{
			if (bindingErrors.Count == 0)
				return;

			// Messages

			Font font = new Font("Arial", 10);
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			string accMessage = "";
			foreach (string be in bindingErrors)
			{
				if (accMessage.Length > 0)
					accMessage += "\n";
				accMessage += be;
			}
			SizeF sz = g.MeasureString(accMessage, font, TargetSize.Width - 10, StringFormat.GenericDefault);

			// Background

			Rectangle rect = new Rectangle(5, 5, Math.Max((int)sz.Width, TargetSize.Width - 10), (int)sz.Height);
			g.FillRectangle(new SolidBrush(Color.FromArgb(128, 255, 255, 128)), rect);
			g.DrawRectangle(Pens.Gray, rect);

			// Message

			g.DrawString(accMessage, font, Brushes.Red, new RectangleF(5f, 5f, sz.Width, sz.Height), StringFormat.GenericDefault);
			font.Dispose();
			font = null;
		}

        #endregion
        #region --- Object Tracking ---

		internal object GetObjectAt(int x, int y)
		{
			return cSpace.GetObjectAt(x, y);
		}

		internal ObjectMapper ObjectMapper { get { return cSpace.ObjectMapper; } }

		internal bool ObjectTrackingEnabled { get { return cSpace.ObjectTrackingEnabled; } set { cSpace.ObjectTrackingEnabled = value; } }

		internal MapAreaCollection MapAreas { get { return cSpace.MapAreas; } }

		internal bool UseMatrixObjectTrackingModel { get { return cSpace.UseMatrixObjectTrackingModel; } set { cSpace.UseMatrixObjectTrackingModel = value; } }

        #endregion
        #region --- Frame ---

		public ChartFrame ChartFrame
		{
			get
			{
				if (InSerialization && (frame == null || frame.FrameKind == ChartFrameKind.NoFrame))
					return null;
				if (frame == null)
				{
					frame = new ChartFrame(new Rectangle(new Point(0, 0), TargetSize));
					frame.OwningChart = this;
				}
				return frame;
			}
			set
			{
				frame = value;
				if (frame != null)
				{
					frame.Rectangle = new Rectangle(new Point(0, 0), TargetSize);
					frame.OwningChart = this;
				}
			}
		}

        #endregion
        #region --- Internal Methods ---
        /// <summary>
        /// Internal method to check if a feature is supported by the geometric engine
        /// </summary>
        public bool GeometricEngineSupports(string featureName)
        {
            return GeometricEngine.SupportsFeature(featureName);
        }
        /// <summary>
        /// Internal building of Html strings related to map areas
        /// </summary>
        public string BuildMapAreaHtml( bool clientsideApiEnabled, bool serializeDatapoints, string uniqueID)
        {
			StringBuilder totalInnerHtml = new StringBuilder();

            foreach (MapArea ma in MapAreas)
            {
				StringBuilder innerHtml = new StringBuilder();
				StringBuilder coordString = new StringBuilder();

                foreach (int number in ma.Coords)
                {
                    coordString.Append(number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    coordString.Append(',');
                }

                DataPoint dp = ma.Object as DataPoint;
                Series ser = ma.Object as Series;

                if (dp != null || ser != null)
                {
                    innerHtml.Append("<Area ");

                    innerHtml.Append("shape=\"");
                    innerHtml.Append(
                        ma.Kind == MapAreaKind.Rectangle ? "Rectangle" :
                        (ma.Kind == MapAreaKind.Circle ? "Circle" : "Polygon")
                        );

                    innerHtml.Append("\" coords=\"");
                    innerHtml.Append(coordString.ToString(0, coordString.Length - 1));
                    innerHtml.Append("\" ");

                    object hrefobj;
                    if (dp != null)
                    {
                        hrefobj = dp.Parameters["href"];
                        if (hrefobj == null)
                            hrefobj = dp.OwningSeries.Parameter("pointHref");
                    }
                    else
                        hrefobj = ser.Parameter("href");

                    if (dp != null && clientsideApiEnabled && serializeDatapoints)
                    {
                        innerHtml.Append("onMouseOver=\"window." + uniqueID + ".dpHover(event,'");
                        innerHtml.Append(dp.OwningSeries.Name);
                        innerHtml.Append("',");
                        innerHtml.Append(dp.Index);
                        innerHtml.Append(",1);\" ");

                        innerHtml.Append("onMouseOut=\"window." + uniqueID + ".dpHover(event,'");
                        innerHtml.Append(dp.OwningSeries.Name);
                        innerHtml.Append("',");
                        innerHtml.Append(dp.Index);
                        innerHtml.Append(");\" ");
                    }

                    if (ser != null && clientsideApiEnabled)
                    {
                        innerHtml.Append("onMouseOver=\"window." + uniqueID + ".seriesHover(event,'");
                        innerHtml.Append(ser.Name);
                        innerHtml.Append("',1);\" ");

                        innerHtml.Append("onMouseOut=\"window." + uniqueID + ".seriesHover(event,'");
                        innerHtml.Append(ser.Name);
                        innerHtml.Append("');\" ");
                    }

                    if (hrefobj != null && hrefobj is string)
                    {
                        innerHtml.Append("href=\"");
                        innerHtml.Append(CommonFunctions.ParseFormatString((string)hrefobj, ma.Object));
                        innerHtml.Append("\" ");
                    }
                    object MapAreaAttributesObj;

                    if (dp != null)
                    {
                        MapAreaAttributesObj = dp.Parameters["areaAttributes"];
                        if (MapAreaAttributesObj == null)
                            MapAreaAttributesObj = dp.OwningSeries.Parameter("pointAreaAttributes");
                    }
                    else
                        MapAreaAttributesObj = ser.Parameter("areaAttributes");

                    if (MapAreaAttributesObj != null && MapAreaAttributesObj is string)
                    {
                        innerHtml.Append(CommonFunctions.ParseFormatString((string)MapAreaAttributesObj, ma.Object));
                    }

                    if (dp != null && clientsideApiEnabled)
                    {

                    }

                    innerHtml.Append(" />");
					//Debug.WriteLine("Point: " + (dp==null?"null":dp.Index.ToString()) + " innerHtml: " + innerHtml);
                }
				totalInnerHtml.Append(innerHtml.ToString());
            }

            return totalInnerHtml.ToString();
        }


		internal void Message(string message)
		{
			throw new InvalidOperationException(message);
		}

        /// <summary>
		/// Internal method for error message rendering
		/// </summary>
		public void RenderErrorMessage(Exception ex, Graphics g)
		{
			// Clean the background, since it is nor drawn in the "OnPaintBackground()"
			g.Clear(Color.White);
			PointF position = DrawMessageString(g, ex.Message, new Font("Arial", 12, FontStyle.Bold), Brushes.DarkGray, new Point(0, 0));
			position.Y += 2;
			if (ex.StackTrace != null)
				DrawMessageString(g, ex.StackTrace, new Font("Arial", 10), Brushes.DarkGray, position);

			buildValid = false;

			Debug.WriteLine("\nError: " + ex.Message);
			if (ex.StackTrace != null)
				Debug.WriteLine(ex.StackTrace);
		}

		private PointF DrawMessageString(Graphics g, string text, Font font, Brush brush, PointF position)
		{
			int indentPixels = 20;
			string[] lines = text.Split('\n');

			// Start with the given position
			PointF wPosition = position;
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				int w = TargetSize.Width;
				// start line with no indent
				wPosition.X = position.X;
				while (true)
				{
					SizeF lineSize = g.MeasureString(line, font);
					if (lineSize.Width <= w)
					{
						g.DrawString(line, font, brush, wPosition);
						wPosition.Y += lineSize.Height;
						break;
					}
					else
					{
						// find maximum part that fits
						int x;
						for (x = line.Length - 1; x > 0; x--)
						{
							if ((line[x] == ' ' || line[x] == ',' || line[x] == '\\')
								&& g.MeasureString(line.Substring(0, x + 1), font).Width <= w)
							{
								// split and draw the first part
								g.DrawString(line.Substring(0, x + 1), font, brush, wPosition);
								// continue with the second part, with indent
								w = TargetSize.Width - indentPixels;
								wPosition.X = position.X + indentPixels;
								wPosition.Y += g.MeasureString(line.Substring(0, x + 1), font).Height;
								line = line.Substring(x, line.Length - x);
								break;
							}
						}
						if (x <= 1) // couldn't brake the line!
						{
							g.DrawString(line, font, brush, wPosition);
							wPosition.Y += g.MeasureString(line, font).Height;
							break;
						}
					}
				}
			}
			wPosition.X = position.X;
			return wPosition;

		}
        #endregion
        #region --- Private Methods ---

		private string NextClonedName(string name)
		{
			return name + (clonedCount++).ToString("_0000");
		}
        #endregion
        #region --- Serialization ---

		internal LineStyle2DCollection LineStyles2DX
		{
			get
			{
				if (InSerialization)
				{
					LineStyle2DCollection C = new LineStyle2DCollection(this);
					foreach (LineStyle2D S in lineStyles2D)
						if (S.ShouldSerializeMe)
							C.Add(S);
					return C;
				}
				else
					return lineStyles2D;
			}
		}

		internal LineStyleCollection LineStylesX
		{
			get
			{
				if (InSerialization)
				{
					LineStyleCollection C = new LineStyleCollection(this, false);
					foreach (LineStyle S in lineStyles)
						if (S.ShouldSerializeMe)
							C.Add(S);
					return C;
				}
				else
					return lineStyles;
			}
		}

		internal DataPointLabelStyleCollection DataPointLabelStylesX
		{
			get
			{
				if (InSerialization)
				{
					DataPointLabelStyleCollection C = new DataPointLabelStyleCollection(this, false);
					foreach (DataPointLabelStyle S in dataPointLabelStyles)
						if (S.ShouldSerializeMe)
							C.Add(S);
					return C;
				}
				else
					return dataPointLabelStyles;
			}
		}

		// Used by the wrapper control class.
		internal PaletteCollection PalettesX
		{
			get
			{
				if (InSerialization)
				{
					PaletteCollection C = new PaletteCollection();
					C.Clear();
					foreach (Palette S in palettes)
						if (S.HasChanged)
							C.Add(S);
					return C;
				}
				else
					return palettes;
			}
		}

		// Used by the wrapper control class.
		internal DataDimensionCollection DataDimensionsX
		{
			get
			{
				if (InSerialization)
				{
					DataDimensionCollection C = new DataDimensionCollection(this);
					foreach (DataDimension S in dimensions)
						if (S.ShouldSerializeMe)
							C.Add(S);
					return C;
				}
				else
					return dimensions;
			}
		}

		// Used by the wrapper control class.
		internal SeriesStyleCollection SeriesStylesX
		{
			get
			{
				if (InSerialization)
				{
					SeriesStyleCollection C = new SeriesStyleCollection(this, false);
					foreach (SeriesStyle S in presentationStyles)
						if (S.ShouldSerializeMe)
							C.Add(S);
					return C;
				}
				else
					return presentationStyles;
			}
		}


        #endregion
        #region --- XML Serialization ---

		public void StoreSeriesStyleDataToXml(string fileName)
		{
			// The root node
			XmlDocument doc = new XmlDocument();
			XmlNode root = doc.CreateElement("StyleData");
			doc.AppendChild(root);

			XmlCustomSerializer CS = new XmlCustomSerializer(root);
			CS.Reading = false;
			Serialize(CS);

			// Writing data

			XmlTextWriter wrt = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
			wrt.Formatting = Formatting.Indented;

			doc.WriteTo(wrt);
			wrt.Close();
		}

		private void Serialize(XmlCustomSerializer CS)
		{
			// Intro comments
			CS.Comment("    ==================================    ");
			CS.Comment("    ComponentArt Web.Visualization.Charting - Style data    ");
			CS.Comment("      Created: " + DateTime.Now + "     ");
			CS.Comment("    ==================================    ");

			if (CS.BeginTag("Palettes"))
			{
				Palettes.Serialize(CS, this);
				CS.EndTag();
			}

			if (CS.BeginTag("SeriesStyles"))
			{
				SeriesStyles.Serialize(CS);
				CS.EndTag();
			}

			if (CS.BeginTag("TextStyles"))
			{
				TextStyles.Serialize(CS);
				CS.EndTag();
			}

			if (CS.BeginTag("LabelStyles"))
			{
				LabelStyles.Serialize(CS);
				CS.EndTag();
			}

			if (CS.BeginTag("DataPointLabelStyles"))
			{
				DataPointLabelStyles.Serialize(CS);
				CS.EndTag();
			}

			if (CS.BeginTag("LineStyles"))
			{
				LineStyles.Serialize(CS);
				CS.EndTag();
			}

			if (CS.BeginTag("LineStyles2D"))
			{
				LineStyles2D.Serialize(CS);
				CS.EndTag();
			}

			if (CS.BeginTag("GradientStyles"))
			{
				GradientStyles.Serialize(CS);
				CS.EndTag();
			}

			if (CS.BeginTag("MarkerStyles"))
			{
				MarkerStyles.Serialize(CS);
				CS.EndTag();
			}

			if (CS.BeginTag("Legend"))
			{
				Legend.Serialize(CS);
				CS.EndTag();
			}

			if (CS.BeginTag("Frame"))
			{
				if (CS.Reading && frame == null)
					frame = new ChartFrame();
				ChartFrame.Serialize(CS);
				CS.EndTag();
			}

		}

		public void LoadSeriesStyleDataFromXml(string fileName)
		{
			XmlDocument doc = new XmlDocument();
			XmlTextReader rdr = new XmlTextReader(fileName);
			doc.Load(rdr);
			rdr.Close();

			XmlElement root = doc.FirstChild as XmlElement;

			XmlCustomSerializer CS = new XmlCustomSerializer(root);
			CS.Reading = true;
			Serialize(CS);
		}

        #endregion
        #region --- Static Stuff ---

		internal static ChartBase GetChartFromObject(object o)
		{
			ChartBase chart;

			// Get the chart
			if (o is ChartBase)
			{
				chart = (ChartBase)o;
			}
#if !__BUILDING_CRI__
			else if (o is WinChart) 
			{
				chart = ((WinChart)o).Chart;
			} 
#endif
#if __BUILDING_CRI_DESIGNER__
            else if (o.GetType().ToString() == "Microsoft.ReportDesigner.Controls.CustomReportItemHost") 
            {
                FieldInfo fi = o.GetType().GetField("m_component", BindingFlags.Instance | BindingFlags.NonPublic);
                object obj = fi.GetValue(o);
                SqlChartDesigner scd = (SqlChartDesigner)obj;
                if (scd == null)
                    chart = null;
                else
                    chart = scd.Chart;
            }
#endif
			else if (o is NamedObjectBase)
			{
				chart = ((NamedObjectBase)o).OwningChart;
			}
			else if (o is ChartObject)
			{
				chart = ((ChartObject)o).OwningChart;
			}
			else if (o is MultiLineStyleItem)
			{
				chart = ((MultiLineStyleItem)o).LineStyle.OwningChart;
			}
            else if (o.GetType().Name == "Chart" || o.GetType().Name == "WinChart")
			{
                chart = (ChartBase)PropertyValue.Get(o, "ChartBase");
			}
#if !__BUILDING_CRI__ && __COMPILING_FOR_2_0_AND_ABOVE__
            else if (o is ChartActionList)
            {
                chart = ((ChartActionList)o).Chart;
            }
#endif
			else
			{
				// Try extracting "Chart" property;
                PropertyInfo pi = o.GetType().GetProperty("ChartBase", BindingFlags.Instance | BindingFlags.NonPublic);
				if (pi == null)
				{
					// Try extracting "OwningChart" property;
					pi = o.GetType().GetProperty("OwningChart", BindingFlags.Instance | BindingFlags.NonPublic);
				}
				if (pi != null)
				{
					chart = (ChartBase)pi.GetValue(o, null);
				}
				else
				{
					chart = null;
				}
			}

			if (chart == null)
				throw new ArgumentException("Could not extract chart from object " + o.ToString());

			return chart;
		}

        #endregion



 #if __BuildingWebChart__
        internal const string MainAssemblyTypeName = "ComponentArt.Web.Visualization.Charting.Chart";
        internal const string ProductName = "Chart";
#else
#if __BUILDING_CRI_DESIGNER__
        internal const string MainAssemblyTypeName = "ComponentArt.Charting.SqlChartDesigner";
        internal const string ProductName = "SqlChart";

#else
#if __BUILDING_CRI__
        internal const string MainAssemblyTypeName = "ComponentArt.Charting.SqlChart";
        internal const string ProductName = "SqlChart";
#else
		internal const string MainAssemblyTypeName = "ComponentArt.Charting.WinChart";
		internal const string ProductName = "WinChart";
#endif
#endif
#endif

		internal static readonly Type MainAssemblyType = Type.GetType(MainAssemblyTypeName);

		internal static bool IsObfuscated
		{
			get
			{
				return (Type.GetType("ComponentArt.Web.Visualization.Charting.ChartBase") == null);
			}
		}

        #region --- Before and After Data Binding Methods ---

        /// <summary>
        /// Performs a DataBind if this has not already been done.
        /// </summary>
        internal void DataBindIfNeeded()
        {
            if (needsDataBinding)
                this.DataBind();
        }

		private RunTimeCompiledSource runTimeCompiledSource = null;
		internal RunTimeCompiledSource RunTimeCompiledSource
		{
			get { return runTimeCompiledSource; }
			set { runTimeCompiledSource = value; }
		}

		internal void ProcessBeforeDataBinding(object ownerControl)
		{
			if(InDesignMode)
				return;
			if(runTimeCompiledSource == null)
				return;

			try
			{
				runTimeCompiledSource.ProcessBeforeDataBinding(ownerControl);
			}
			catch(Exception ex)
			{
				RegisterErrorMessage(ex.Message);
			}
		}

		internal void ProcessAfterDataBinding(object ownerControl)
		{
			if(InDesignMode)
				return;
			if(runTimeCompiledSource == null)
				return;

			try
			{
				runTimeCompiledSource.ProcessAfterDataBinding(ownerControl);
			}
			catch(Exception ex)
			{
				RegisterErrorMessage(ex.Message);
			}
		}
        #endregion

		Series[] Leaves(CompositeSeries cs)
		{
			ArrayList al = new ArrayList();

			foreach (SeriesBase sb in cs.SubSeries)
			{
				if (sb is Series)
				{
					al.Add(sb);
				}
				else
				{
					al.AddRange(Leaves((CompositeSeries)sb));
				}
			}
			return (Series[])al.ToArray(typeof(Series));
		}

		internal Series[] AllLeaves()
		{
			return Leaves(this.Series);
		}
    
	}

}
