using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

using System.Xml.Serialization;

namespace ComponentArt.Web.Visualization.Gauges
{
	internal enum LicenseType
	{
		Expired,
		Full
	}

    /// <summary>
    /// Specifies which map areas to generate.  The values can be combined with bitwise operations.
    /// </summary>
    [FlagsAttribute]
    public enum MapAreaSelectionKind
	{
		None = 0,
		Gauges = 1,
		Pointers = 2,
		Ranges = 4,
		TickMarks = 8,
		Indicators = 16,
		All = 31
	}

#if DEBUG
	public
#else
	internal
#endif
	enum GaugeKindGroup
	{
	    All = 0x000000,
        Radial = 0x110000,
		Circle = 0x110100,
		HalfCircle = 0x110200,
		QuarterCircle = 0x110400,
		Linear = 0x120000,
		LinearHorizontal = 0x120100,
		LinearVertical = 0x120200,
		WithScale = 0x100000,
		Numeric = 0x400000
	}
	
	/// <summary>
	/// Specifies the type of gauge to render.
	/// </summary>
	public enum GaugeKind
	{
		Circular = GaugeKindGroup.Circle,
		HalfCircleE = GaugeKindGroup.HalfCircle + 1,
		HalfCircleN = GaugeKindGroup.HalfCircle + 2,
		HalfCircleW = GaugeKindGroup.HalfCircle + 3,
		HalfCircleS = GaugeKindGroup.HalfCircle + 4,
		QuarterCircleE = GaugeKindGroup.QuarterCircle + 1,
		QuarterCircleNE = GaugeKindGroup.QuarterCircle + 2,
		QuarterCircleN = GaugeKindGroup.QuarterCircle + 3,
		QuarterCircleNW = GaugeKindGroup.QuarterCircle + 4,
		QuarterCircleW = GaugeKindGroup.QuarterCircle + 5,
		QuarterCircleSW = GaugeKindGroup.QuarterCircle + 6,
		QuarterCircleS = GaugeKindGroup.QuarterCircle + 7,
		QuarterCircleSE = GaugeKindGroup.QuarterCircle + 8,
		LinearHorizontal = GaugeKindGroup.LinearHorizontal,
		LinearVertical = GaugeKindGroup.LinearVertical,
		Numeric = GaugeKindGroup.Numeric
	}

    /// <summary>
    /// Specifies the type of numeric gauge to render.
    /// </summary>
	public enum NumericDisplayKind
	{
		Simple,
		Mechanical,
		LCD,
		Book
	}

	/// <summary>
	/// Specifies the position of the image on the control. 
	/// </summary>
	public enum ImageLayout
	{
		/// <summary>
		/// The image is centered within the control's client rectangle.  
		/// </summary>
		Center,
		/// <summary>
		/// The image is left-aligned at the top across the control's client rectangle.  
		/// </summary>
		None,
		/// <summary>
		/// The image is stretched across the control's client rectangle.  
		/// </summary>
		Stretch,
		/// <summary>
		///  The image is tiled across the control's client rectangle.  
		/// </summary>
		Tile,
		/// <summary>
		/// The image is enlarged within the control's client rectangle. 
		/// </summary>
		Zoom 
	}

	internal delegate void VisualChangeHandler(object sender);

    /// <summary>
    /// Represents a visual gauge (or sub-gauge) component.
    /// </summary>
	[TypeConverter(typeof(NamedConverterWithRelevanceCheck))]
	[Serializable]
	public class SubGauge : NamedObject, IDisposable
	{
		// events
		internal event ValueChangeHandler ValueChanged;
		
        internal object gaugeWrapper = null;		

		private SubGaugeCollection subGauges;

		private Factory factory;

		internal event VisualChangeHandler VisualChanged;

		private bool visible = true;

		private bool renderErrorMesage = true;

		// Target rectangle
		private Rectangle2D targetRectangle = new Rectangle2D(0,0,0,0);
		// Relative size and position
		private Point2D relativeLocation = new Point2D(50,50);
		private Size2D relativeSize = new Size2D(100,100);
		private ImageLayout backgroundImageLayout = ImageLayout.Tile;
		private Color backgroundColor = Color.FromKnownColor(KnownColor.Control);
		// Note: this has to be object type to handle background for various framworks
		private Object backgroundImage = null;

		// Style Name and Collections
		private string themeName = "Default";
        private GaugeKind gaugeKind = GaugeKind.Circular;

        private string paletteName = "Default";

        private ThemeCollection themeCollection = new ThemeCollection();

		private GaugePalette palette = new GaugePalette("Default");
		private GaugePaletteCollection palettes = new GaugePaletteCollection(true);
		private ScaleAnnotationStyleCollection scaleAnnotationStyles = new ScaleAnnotationStyleCollection(true);
		private TextStyleCollection textStyleCollection = new TextStyleCollection(true);
		private PointerStyleCollection pointerStyleCollection = new PointerStyleCollection();
		private MarkerStyleCollection markerStyleCollection = new MarkerStyleCollection();

		private LayerCollection backgroundLayers = new LayerCollection();
		private LayerCollection frameLayers = new LayerCollection();
		private LayerCollection coverLayers = new LayerCollection();
		private LayerCollection needleLayers = new LayerCollection();
		private LayerCollection hubLayers = new LayerCollection();
		private LayerCollection markerLayers = new LayerCollection();
		private LayerCollection digitMaskLayers = new LayerCollection();

		private TextAnnotationCollection textAnnotations = new TextAnnotationCollection();
		private ImageAnnotationCollection imageAnnotations = new ImageAnnotationCollection();
		private IndicatorCollection indicators = new IndicatorCollection();

		// Map areas
		private MapAreaCollection mapAreas = new MapAreaCollection();
		private MapAreaSelectionKind mapAreaSelection = MapAreaSelectionKind.None;

		// Editing mode
		private bool editing = false;
		private bool inSerialization = false;
		private bool inDesignMode = false;

        /// <summary>
        /// Occurs before a sub-gauge's name is changed.
        /// </summary>
		public event NameChangeHandler SubGaugeChangingName;
		
        /// <summary>
        /// Occurs aftere a sub-gauge's name has been changed.
        /// </summary>
        public event NameChangeHandler SubGaugeChangedName;

		private ScaleCollection scales;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gauge"/> class.
        /// </summary>
		public SubGauge() : this(null) { }

		internal SubGauge(string name) : base(name) 
		{
			subGauges = new SubGaugeCollection();
			subGauges.MemberChangedName += new NameChangeHandler(HandleSubGaugeChangedName);
			subGauges.MemberChangingName += new NameChangeHandler(HandleSubGaugeChangingName);
			
			scales = new ScaleCollection();
			Scale mainScale = new Scale("Main");
			mainScale.IsRequired = true;
			scales.Add(mainScale);

            (themeCollection as IObjectModelNode).ParentNode = this; // populate later

			(subGauges as IObjectModelNode).ParentNode = this;
			(scales as IObjectModelNode).ParentNode = this;

			(scaleAnnotationStyles as IObjectModelNode).ParentNode = this;
			(palettes as IObjectModelNode).ParentNode = this;
			(pointerStyleCollection as IObjectModelNode).ParentNode = this;
			(markerStyleCollection as IObjectModelNode).ParentNode = this;
			(textStyleCollection as IObjectModelNode).ParentNode = this;
			
			(backgroundLayers as IObjectModelNode).ParentNode = this;
			(frameLayers as IObjectModelNode).ParentNode = this;
			(coverLayers as IObjectModelNode).ParentNode = this;
			(needleLayers as IObjectModelNode).ParentNode = this;
			(hubLayers as IObjectModelNode).ParentNode = this;
			(markerLayers as IObjectModelNode).ParentNode = this;
			(digitMaskLayers as IObjectModelNode).ParentNode = this;
						
			(textAnnotations as IObjectModelNode).ParentNode = this;
			(imageAnnotations as IObjectModelNode).ParentNode = this;
			(indicators as IObjectModelNode).ParentNode = this;

            scales.TakeSnapshot();
        }

		public void Dispose()
		{
		}

		#region --- Factory Operation ---
		internal void SetFactory(Factory factory)
		{
			this.factory = factory;
			GetPredefinedCollections();
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal Factory Factory 
		{
			get 
			{
				SubGauge parent = ObjectModelBrowser.GetOwningGauge(this);
				if(parent != null) 
					return parent.Factory;
				else
					return factory;
			}
		}
		private void GetPredefinedCollections()
		{
			// This sets predefined contents of those collection that cannot be set at construction time
			// since they need input from "Themes.xml" and the gauge object hierarchy

			ArrayList allLayers = Factory.CreateLayers();
			for (int i = 0; i < allLayers.Count; i++)
			{
				Layer layer = allLayers[i] as Layer;
				switch (layer.LayerRoleKind)
				{
					case LayerRoleKind.Background: BackgroundLayers.Add(layer); break;
					case LayerRoleKind.Frame: FrameLayers.Add(layer); break;
					case LayerRoleKind.Cover: CoverLayers.Add(layer); break;
					case LayerRoleKind.Pointer: NeedleLayers.Add(layer); break;
					case LayerRoleKind.Hub: HubLayers.Add(layer); break;
					case LayerRoleKind.Marker: MarkerLayers.Add(layer); break;
					case LayerRoleKind.DigitMask: DigitMaskLayers.Add(layer); break;
					default: break;
				}
			}

			string name = "ComponentArt.Web.Visualization.Gauges.GDIEngine.Resources.Themes.xml";
			Stream rcStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
			GaugeXmlSerializer.ReadObject(this, rcStream);
			rcStream.Close();

            themeCollection.PopulateInitialContents();
            pointerStyleCollection.PopulateInitialContents();
            markerStyleCollection.PopulateInitialContents();
			
			// Now take snapshots everywhere

			themeCollection.TakeSnapshot();
			palettes.TakeSnapshot();
			scaleAnnotationStyles.TakeSnapshot();
			textStyleCollection.TakeSnapshot();
			pointerStyleCollection.TakeSnapshot();
			markerStyleCollection.TakeSnapshot();
        }

		#region --- Working Layers ---

		internal Layer Background
		{
			get
			{
				return Skin.Background();
			}
		}
		#endregion

		#endregion

		#region --- Properties ---
		
		#region --- Style Collections ---

        /// <summary>
        /// Gets a collection of all available themes.
        /// </summary>
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Category("Gauge Style Collections")]
		[Description("Themes collection")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DefaultValue(null)]
		[Browsable(false)]
		public ThemeCollection Themes 
		{
			get 
			{
                if (TopmostGauge.InSerialization)
                {
					if (TopmostGauge == this)
					{
						return (ThemeCollection)TopmostGauge.themeCollection.GetModifiedCollection();
					}
					else
						return null;
                }
				return TopmostGauge.themeCollection; 
			}
		}

        /// <summary>
        /// Gets a collection of all available pointer styles.
        /// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Category("Gauge Style Collections")]
		[Description("Collection of pointer styles")]
		[DefaultValue(null)]
		[Browsable(false)]
		public PointerStyleCollection PointerStyles 
		{
			get 
			{
                if (TopmostGauge.InSerialization)
                {
                    if (TopmostGauge == this)
                        return (PointerStyleCollection)TopmostGauge.pointerStyleCollection.GetModifiedCollection();
                    else
                        return null;
                }
				return TopmostGauge.pointerStyleCollection; 
			}
		}

        /// <summary>
        /// Gets a collection of all available text styles.
        /// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Category("Gauge Style Collections")]
		[Description("Collection of text styles")]
		[DefaultValue(null)]
		[Browsable(false)]
		public TextStyleCollection TextStyles 
		{
			get 
			{
				if (TopmostGauge.InSerialization)
				{
					if (TopmostGauge == this)
						return (TextStyleCollection)TopmostGauge.textStyleCollection.GetModifiedCollection();
					else
						return null;
				}
				return TopmostGauge.textStyleCollection; 
			}
		}

        /// <summary>
        /// Gets a collection of all available marker styles.
        /// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Category("Gauge Style Collections")]
		[Description("Collection of marker styles")]
		[DefaultValue(null)]
		[Browsable(false)]
		public MarkerStyleCollection MarkerStyles 
		{
			get 
			{
                if (TopmostGauge.InSerialization)
                {
                    if (TopmostGauge == this)
                        return (MarkerStyleCollection)TopmostGauge.markerStyleCollection.GetModifiedCollection();
                    else
                        return null;
                }
				return TopmostGauge.markerStyleCollection; 
			}
		}

        /// <summary>
        /// Gets a collection of all available palettes.
        /// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] 
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Category("Gauge Style Collections")]
		[Description("Gauge color palettes")]
		[Browsable(false)]
		[DefaultValue(null)]
		public GaugePaletteCollection Palettes 
		{
			get 
			{
				if (TopmostGauge.InSerialization)
				{
					if (TopmostGauge == this)
						return (GaugePaletteCollection)TopmostGauge.palettes.GetModifiedCollection();
					else
						return null;
				}
				return TopmostGauge.palettes; 
			}
		}

        /// <summary>
        /// Gets a collection of all available scale annotation styles.
        /// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)] 
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Category("Gauge Style Collections")]
		[Description("Gauge scale annotation styles")]
		[DefaultValue(null)]
		[Browsable(false)]
		public ScaleAnnotationStyleCollection ScaleAnnotationStyles 
		{
			get 
			{
				if (TopmostGauge.InSerialization)
				{
					if (TopmostGauge == this)
						return (ScaleAnnotationStyleCollection)TopmostGauge.scaleAnnotationStyles.GetModifiedCollection();
					else
						return null;
				}
				return TopmostGauge.scaleAnnotationStyles; 
			}
		}

		#endregion

		#region --- Background Image ---
		/// <summary>
		/// Gets or sets layout of the background image for this object.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(ImageLayout.Tile)]
		public ImageLayout BackgroundImageLayout { get { return backgroundImageLayout; } set { backgroundImageLayout = value; } }
		
        /// <summary>
        /// Gets or sets the background image of this object.
        /// </summary>		
		[Browsable(false)]
		public object BackgroundImage { get { return backgroundImage; } set { backgroundImage = value; } }

        /// <summary>
        /// Gets or sets the color of the background in the image that lies outside the gauge. Default is transparent.
        /// </summary>
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Category("Appearance")]
		[Description("Background Color")]		
		[DefaultValue(typeof(Color),"Control")]
		public Color BackColor { get { return backgroundColor; } set { backgroundColor = value; } }
		#endregion

		#region --- Position and Size Category ---

        /// <summary>
        /// Gets or sets the relative coordinates of lower-left corner in percentages of control or parent coordinates ranges.
        /// </summary>
		[Category("Size and Position")]
		[Description("Relative coordinates of lower-left corner in percentages of control or parent coordinates ranges")]
		[Editor(typeof(SizePositionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		[DefaultValue(typeof(Point2D),"50,50")]
        public Point2D RelativeLocation { get { return relativeLocation; } set { relativeLocation = value; } }

        /// <summary>
        /// Gets or sets the relative size of the gauge in percentages of control or parent size.
        /// </summary>
		[Category("Size and Position")]
		[Description("Relative size of the gauge in percentages of control or parent size")]
		[Editor(typeof(SizePositionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		[DefaultValue(typeof(Size2D),"100,100")]
		public Size2D RelativeSize { get { return relativeSize; } set { relativeSize = value; } }

		#endregion

		#region --- Styles Category ---

		internal GaugePalette Palette 
		{
			get 
			{
				if(TopmostGauge != this)
					return TopmostGauge.Palette;
				else
				{
					if(Skin != null)
						return Skin.Palette();
					else
						return palettes[paletteName];
				}
			}
		}

        /// <summary>
        /// Gets or sets the geometry or look of the gauge. Can be Circular, Linear, Numeric, etc. 
        /// </summary>        
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Category("Gauge Active Style")]
		[Description("Gauge kind")]
		[DefaultValue(typeof(GaugeKind), "Circular")]
        public GaugeKind GaugeKind 
		{ 
			get 
			{ 
				return gaugeKind; 
			} 
			set 
			{ 
				gaugeKind = value; 
				CopyScaleLayoutFormSkin();
			} 
		}
        
        /// <summary>
        /// Gets or sets a Theme for the overall visual look of the gauge. Available themes are: Default, Black Ice, Arctic White, Monochrome 
        /// </summary>
		[RefreshProperties(RefreshProperties.All)]
		[NotifyParentProperty(true)]
		[Category("Gauge Active Style")]
		[Description("Gauge skin name")]
		[TypeConverter(typeof(ThemeNameConverter))]
		[DefaultValue("Default")]
		public string ThemeName 
		{ 
			get 
			{
				return themeName; 
			} 
			set 
			{ 
				themeName = value;
				CopyScaleLayoutFormSkin();
			} 
		}

        /// <summary>
        /// Gets or sets a Theme for the overall visual look of the gauge. Available themes are: Default, Black Ice, Arctic White, Monochrome 
        /// </summary>
        [Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ThemeKind ThemeKind 
		{ 
			get 
			{ 
				return Theme.NameToKind(themeName); 
			} 
			set 
			{ 
				themeName = Theme.KindToName(value);
				CopyScaleLayoutFormSkin();
			} 
		}

        /// <summary>
        /// Holds settings related to the visual theme of the gauge. 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [XmlIgnore]
        public Theme Theme 
		{ 
			get { return Themes[ThemeName]; } 
			set 
			{ 
				Themes[ThemeName] = value;
				CopyScaleLayoutFormSkin();
			} 
		}

        /// <summary>
        /// Holds setting related to image layers used in the theme for the selected GaugeKind. 
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [XmlIgnore]
        public Skin Skin 
		{ 
			get 
			{ 
				return (Theme==null? null:Theme.Skins[GaugeKind.ToString()]); 
			} 
		}
		
		internal LayerCollection BackgroundLayers { get { return TopmostGauge.backgroundLayers; } }
		internal LayerCollection FrameLayers { get { return TopmostGauge.frameLayers; } }
		internal LayerCollection CoverLayers { get { return TopmostGauge.coverLayers; } }
		internal LayerCollection NeedleLayers { get { return TopmostGauge.needleLayers; } }
		internal LayerCollection HubLayers { get { return TopmostGauge.hubLayers; } }
		internal LayerCollection MarkerLayers { get { return TopmostGauge.markerLayers; } }
		internal LayerCollection DigitMaskLayers { get { return TopmostGauge.digitMaskLayers; } }

        /// <summary>
        /// Gets or sets the type name of the current palette.
        /// </summary>
		[TypeConverter(typeof(GaugePaletteNameConverter))]
		[DefaultValue("Default")]
		public string PaletteName { get { return paletteName; } set { paletteName = value; } }

        /// <summary>
        /// Gets or sets the type of the current palette.
        /// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PaletteKind PaletteKind { get { return GaugePalette.NameToKind(paletteName); } set { paletteName = GaugePalette.KindToName(value); } }
		#endregion

		#region --- Annotations Category ---

        /// <summary>
        /// Gets a collection of all <see cref="TextAnnotation"/> objects in the Gauge. 
        /// </summary>
#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [Browsable(false)]
        [DefaultValue(null)]
        public TextAnnotationCollection TextAnnotations
		{
			get 
			{
				return textAnnotations; 
			}
		}

        /// <summary>
        /// Gets a collection of all <see cref="ImageAnnotation"/> objects in the Gauge. 
        /// </summary>
#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
		[Browsable(false)]
        [DefaultValue(null)]
        public ImageAnnotationCollection ImageAnnotations
		{
			get 
			{
				return imageAnnotations; 
			}
		}

        /// <summary>
        /// Gets a collection of all the <see cref="Indicator"/> objects in this gauge. 
        /// </summary>
#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [Browsable(false)]
        [DefaultValue(null)]
        public IndicatorCollection Indicators
		{
			get 
			{
				return indicators;
			}
		}

		#endregion

		#region --- Value Category ---

        // Note: The main gauge value is handled by these two properties: 'InitialValue' and 'Val'
        // 'InitialValue' is design-time only and uses slider editor, adjusts itself to the value range and
        // actually controls the 'Val' property. 

        /// <summary>
        /// Initial value of the main pointer. Property only used at design time, Value property should be used for run-time. 
        /// </summary>
        [Category("Values")]
        [Description("Initial value of the main pointer")]
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [NotifyParentProperty(true)]
        [Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DesignOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[XmlIgnore]
        public SliderValue InitialValue { get { return new SliderValue(MinValue, MaxValue, Value, 0); } set { Value = value.Value; } }

        /// <summary>
        /// Gets or sets the value of the main <see cref="Pointer"/> object.
        /// </summary>
		[Category("Values")]
		[Description("Value of the main pointer")]
		[DefaultValue(0.0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double Value 
		{
			get 
			{
				if(InSerialization && MainScale == null)
					return 0; 
				return MainPointer.Value; 
			}
			set { MainPointer.Value = value; } 
		}

        /// <summary>
        /// Gets or sets the minimum value of the main <see cref="Scale"/> object.
        /// </summary>
		[Category("Values")]
		[Description("Minimum value of the main scale")]
		[DefaultValue(0.0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double MinValue 
		{
			get 
			{
				if(InSerialization && MainScale == null)
					return 0; 
				return MainScale.MinValue; 
			}
			set { MainScale.MinValue = value; } 
		}

        /// <summary>
        /// Gets or sets the maximum value of the main <see cref="Scale"/> object.
        /// </summary>
        [Category("Values")]
		[Description("Maximum value of the main scale")]
		[DefaultValue(100.0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double MaxValue 
		{ 
			get 
			{
				if(InSerialization && MainScale == null)
					return 100; 
				return MainScale.MaxValue; 
			}
			set { MainScale.MaxValue = value; } 
		}
		#endregion

		#endregion

		#region --- Coordinates and Transformations ---

		internal virtual bool IsIsomorphic { get { return true; } } // NB: may depend on geometry

		#endregion

		#region --- Internal Properties ---

		// Topmost gauge
		internal SubGauge TopmostGauge 
		{ 
			get 
			{ 
				SubGauge gauge = ObjectModelBrowser.GetOwningTopmostGauge(this);
				if(gauge == null)
					return this;
				else
					return gauge;
			}
		}

		internal bool RenderErrorMesage { get { return renderErrorMesage; } set { renderErrorMesage = value; } }

		// Editing mode
		internal bool Editing { get { return editing; } set { editing = value; } }

		internal bool InSerialization { get { return inSerialization; } set { inSerialization = value; } }
		internal bool InDesignMode { get { return TopmostGauge.inDesignMode; } set { inDesignMode = value; } }

		#endregion

        #region --- Subgauges ---

#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[RelevantFor(GaugeKindGroup.WithScale)]
		public SubGaugeCollection SubGauges { get { return subGauges; } }

		void HandleSubGaugeChangingName(NamedObject sender, string newName)
		{
			if (SubGaugeChangingName != null)
				SubGaugeChangingName(sender, newName);
		}

		void HandleSubGaugeChangedName(NamedObject sender, string newName)
		{
			if (SubGaugeChangedName != null)
				SubGaugeChangedName(sender, newName);
		}

        #endregion

		#region --- Scales ---

        /// <summary>
        /// Gets or sets the "Main" <see cref="Scale"/> object in this gauge. 
        /// </summary>
        [NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Main Objects")]
		[RelevantFor(GaugeKindGroup.WithScale)]
		public Scale MainScale
		{
			get { return Scales["Main"]; }
			set
			{
				value.OverrideName("Main");
				Scales["Main"] = value;
			}
		}
		    
        /// <summary>
        /// Gets a collection of all <see cref="Scale"/> objects in this gauge. 
        /// </summary>
#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Collections")]
		[RelevantFor(GaugeKindGroup.WithScale)]
		public ScaleCollection Scales 
		{ 
			get 
			{
				if (InSerialization)
					return scales.GetModifiedCollection() as ScaleCollection;
					
				return scales; 
			}
		}

		#endregion

        #region --- Range ---

        /// <summary>
        /// Gets or sets the "Main" <see cref="Range"/> object in the "Main" <see cref="Scale"/> object. 
        /// </summary>
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Main Objects")]
        [RelevantFor(GaugeKindGroup.WithScale)]
		public Range MainRange
        {
            get { return MainScale.MainRange; }
            set { MainScale.MainRange = value; }
        }

        /// <summary>
        /// Gets a collection of all <see cref="Range"/> objects in the "Main" <see cref="Scale"/> object. 
        /// </summary>
#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Collections")]
		[RelevantFor(GaugeKindGroup.WithScale)]
		public RangeCollection Ranges { get { return MainScale.Ranges; } }

        #endregion

        #region --- Annotation ---

        /// <summary>
        /// Gets or sets the "Main" <see cref="Annotation"/> object in the "Main" <see cref="Range"/> object. 
        /// </summary>
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Main Objects")]
		[RelevantFor(GaugeKindGroup.WithScale)]
		public Annotation MainAnnotation
        {
            get { return MainRange.MainAnnotation; }
            set { MainRange.MainAnnotation = value; }
        }
        
        /// <summary>
        /// Gets a collection of all <see cref="Annotation"/> objects in the "Main" <see cref="Range"/> object. 
        /// </summary>
#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Collections")]
		[RelevantFor(GaugeKindGroup.WithScale)]
		public AnnotationCollection Annotations { get { return MainRange.Annotations; } }

        #endregion

        #region --- Pointer ---

        /// <summary>
        /// Gets or sets the "Main" <see cref="Pointer"/> object in the "Main" <see cref="Scale"/> object. 
        /// </summary>
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Main Objects")]
		[RelevantFor(GaugeKindGroup.WithScale)]
		public Pointer MainPointer
        {
            get { return MainScale.MainPointer; }
            set { MainScale.MainPointer = value; }
        }

        /// <summary>
        /// Gets a collection of all <see cref="Pointer"/> objects in the "Main" <see cref="Scale"/> object. 
        /// </summary>
#if WEB
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Collections")]
		[RelevantFor(GaugeKindGroup.WithScale)]
		public PointerCollection Pointers { get { return MainScale.Pointers; } }

        #endregion

		#region --- Rendering ---
				
	    /// <summary>
	    /// Gets or sets the visibility of this gauge.
	    /// </summary>
		[Category("General")]
		[Description("Gauge visible")]
		[NotifyParentProperty(true)]
		[DefaultValue(true)]
		public bool Visible { get { return visible; } set { visible = value; ObjectModelBrowser.NotifyChanged(this); } }

        private Size2D renderingSize;
		internal void Render(RenderingContext context)
		{
			if(!Visible)
				return;
			try
			{
				// Check the logarithhmic scales
				foreach (Scale scale in this.Scales)
				{
					if (scale.IsLogarithmic && scale.MinValue <= 0)
						throw new Exception("Minimum value of the logarithmic scale '" + scale.Name + "' cannot be less than or equal to zero");
					if (scale.MinValue >= scale.MaxValue)
						throw new Exception("Minimum value of the logarithmic scale '" + scale.Name + "' cannot be greater than or equal to the maximum value");
				}

				// Prepare the rendering context

				// Prepare the targetArea
				Rectangle2D targetArea;
				SubGauge parent = ObjectModelBrowser.GetOwningGauge(this);
				if (parent != null)
				{
					Size2D sz = new Size2D(
						parent.RenderingSize.Width * RelativeSize.Width * 0.01f,
						parent.RenderingSize.Height * RelativeSize.Height * 0.01f);
					Point2D loc = new Point2D(
						parent.RenderingSize.Width * RelativeLocation.X * 0.01f - sz.Width * 0.5f,
						parent.RenderingSize.Height * RelativeLocation.Y * 0.01f - sz.Height * 0.5f);
					targetArea = new Rectangle2D(loc, sz);
				}
				else
					targetArea = new Rectangle2D(0, 0, context.Size.Width, context.Size.Height);

				// Prepare the rendering size 
				if(RenderingSizeConstrained())
				{
					Size2D targetSize = targetArea.Size;
					Size2D nativeSize = GetNativeSize(context);
					// scale native size to fit in target size;
					float f = Math.Min(targetSize.Width / nativeSize.Width, targetSize.Height / nativeSize.Height);
					renderingSize = new Size2D(f * nativeSize.Width, f * nativeSize.Height);
				}
				else
					renderingSize = targetArea.Size;

				RenderingContext newContext = context.SetAreaMapping(new Rectangle2D(0, 0, renderingSize.Width, renderingSize.Height), targetArea, true);
				MapAreas.Clear();
				if(TopmostGauge.RenderGaugesMapAreas)
				{
					PointF[] cntr = GaugeGeometry.CanonicContour(GaugeKind);
					RenderingContext contourContext = newContext.SetAreaMapping(new Rectangle2D(0,0,1,1),true);
					contourContext.Matrix.TransformPoints(cntr);
					Point[] ipoints = new Point[cntr.Length];
					for(int i=0; i< cntr.Length; i++)
						ipoints[i] = new Point((int)(cntr[i].X+0.5f),(int)(cntr[i].Y+0.5f));
					MapAreaPolygon ma = new MapAreaPolygon(ipoints);
					ma.SetObject(this);
					TopmostGauge.MapAreas.Add(ma);
				}
				(new Renderer(this,newContext)).Render();
			}
			catch (Exception ex)
			{
				if(renderErrorMesage && TopmostGauge == this)
				{
					string msg = ex.Message;
					if(ex.InnerException != null)
						msg = ":\n"+ ex.InnerException.Message;
					context.Engine.RenderErrorMessage(msg,context);
				}
				else
					throw ex;
			}
		}

		private void FillBackground(Graphics g)
		{
			g.Clear(this.BackColor);
		}

		private bool RenderingSizeConstrained()
		{
			return 
				GaugeKind != GaugeKind.LinearHorizontal &&
				GaugeKind != GaugeKind.LinearVertical &&
				GaugeKind != GaugeKind.Numeric ;
		}

        internal Size2D RenderingSize { get { return renderingSize; } }

        private Size2D GetNativeSize(RenderingContext context)
        {
            if (SubGauge.IsInGroup(GaugeKind, GaugeKindGroup.Radial) && Skin != null)
            { // Try to take it from the bitmap
                Layer layer = Skin.Background();
                if (layer == null)
                    layer = Skin.Frame();
                if (layer != null)
                    return layer.Size;
            }
            
            Size2D targetSize = context.TargetArea.Size;
            float w = targetSize.Width;
            float h = targetSize.Height;
            float d = Math.Min(w, h);
            float s2 = (float)Math.Sqrt(2.0);
            switch (GaugeKind)
            {
                case GaugeKind.Circular: w = d; h = d; break;
                case GaugeKind.HalfCircleE: w = Math.Min(w, h / 2); h = 2 * w; break;
                case GaugeKind.HalfCircleN: h = Math.Min(h, w / 2); w = 2 * h; break;
                case GaugeKind.HalfCircleS: h = Math.Min(h, w / 2); w = 2 * h; break;
                case GaugeKind.HalfCircleW: w = Math.Min(w, h / 2); h = 2 * w; break;
                case GaugeKind.QuarterCircleE: h = Math.Min(h, w * s2); w = h / s2; break;
                case GaugeKind.QuarterCircleW: h = Math.Min(h, w * s2); w = h / s2; break;
                case GaugeKind.QuarterCircleN: w = Math.Min(w, h * s2); h = w / s2; break;
                case GaugeKind.QuarterCircleS: w = Math.Min(w, h * s2); h = w / s2; break;
                case GaugeKind.QuarterCircleNE: w = d; h = d; break;
                case GaugeKind.QuarterCircleNW: w = d; h = d; break;
                case GaugeKind.QuarterCircleSE: w = d; h = d; break;
                case GaugeKind.QuarterCircleSW: w = d; h = d; break;
                default: break;
            }
            return new Size2D(w, h);
        }

		internal void Invalidate()
		{
			SubGauge topmostGauge = TopmostGauge;
			if(topmostGauge != this && topmostGauge != null)
				topmostGauge.Invalidate();
			else
			{
				if(VisualChanged != null)
					VisualChanged(this);
			}
		}

		#endregion

		#region --- XML Serialization ---

        #region --- Control Serialization ---

        /// <summary>
        /// Serializes this gauge as XML to the specified file.
        /// </summary>
        /// <param name="fileName">The file which this gauge will be serialized to.</param>
        public void XMLSerialize(string fileName)
        {
            XMLSerialize(fileName, false);
        }

        internal void XMLSerialize(string fileName, bool serializeBitmaps)
		{
			try
			{
				//InSerialization = true;
				GaugeXmlSerializer ser = new GaugeXmlSerializer();
                ser.BuildNode(null, "SubGauge", this);
				ser.IgnoreUnknownProperty = true;
				ser.Write(fileName);
				InSerialization = false;
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);

				if(ex.InnerException != null)
				{
					Debug.WriteLine("Inner exception:");
					Debug.WriteLine(ex.InnerException.Message);
					Debug.WriteLine(ex.InnerException.StackTrace);
				}
			}
        }

		/// <summary>
		/// Serializes this gauge as XML to the specified stream.
		/// </summary>
		/// <param name="outputStream">The stream which this gauge will be serialized to.</param>
		public void XMLSerialize(Stream outputStream)
		{
			XMLSerialize(outputStream, false);
		}

		internal void XMLSerialize(Stream outputStream, bool serializeBitmaps)
		{
			try
			{
				//InSerialization = true;
				GaugeXmlSerializer ser = new GaugeXmlSerializer();
				ser.BuildNode(null, "SubGauge", this);
				ser.IgnoreUnknownProperty = true;
				ser.Write(outputStream);
				InSerialization = false;
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);

				if(ex.InnerException != null)
				{
					Debug.WriteLine("Inner exception:");
					Debug.WriteLine(ex.InnerException.Message);
					Debug.WriteLine(ex.InnerException.StackTrace);
				}
			}
		}

        /// <summary>
        /// Deserializes a gauge from the specified XML file.
        /// </summary>
        /// <param name="gauge">An instance of a gauge to deserialize to.</param>
        /// <param name="fileName">The file which contains a serialized gauge.</param>
        public static void XMLDeserialize(SubGauge gauge, string fileName)
        {
            GaugeXmlSerializer.ReadObject(gauge, fileName);
        }

		/// <summary>
		/// Deserializes a gauge from the specified stream.
		/// </summary>
		/// <param name="gauge">An instance of a gauge to deserialize to.</param>
		/// <param name="inputStream">The stream which contains a serialized gauge.</param>
		public static void XMLDeserialize(SubGauge gauge, Stream inputStream)
		{
			GaugeXmlSerializer.ReadObject(gauge, inputStream);
		}

        #endregion

        #region --- XML Serialization of Layer Bitmaps ---

        internal void XMLSerializeBitmaps(string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("Layers");
                AppendLayerNodes(BackgroundLayers, doc, root);
                AppendLayerNodes(FrameLayers, doc, root);
                AppendLayerNodes(CoverLayers, doc, root);
                AppendLayerNodes(HubLayers, doc, root);
				AppendLayerNodes(NeedleLayers, doc, root);
				AppendLayerNodes(MarkerLayers, doc, root);
				AppendLayerNodes(DigitMaskLayers, doc, root);
				doc.AppendChild(root);

                XMLUtils.Write(doc, fileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);

                if (ex.InnerException != null)
                {
                    Debug.WriteLine("Inner exception:");
                    Debug.WriteLine(ex.InnerException.Message);
                    Debug.WriteLine(ex.InnerException.StackTrace);
                }
            }
        }

        internal void XMLDeserializeBitmaps(string fileName)
        {
        }

        private void AppendLayerNodes(LayerCollection layers, XmlDocument doc, XmlElement root)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                XmlElement layerNode = XMLUtils.CreateXmlNode(doc, layers[i]);
                if (layerNode != null)
                    root.AppendChild(layerNode);
            }
        }
        #endregion

        #region --- XML Serialization of Themes and Styles ---

        /// <summary>
        /// Deserializes theme and style collections from the specified XML file.
        /// </summary>
        /// <param name="fileName">The file which contains serialized themes and styles.</param>
        public void XMLDeserializeThemesAndStyles(string fileName)
		{
			themeCollection.PopulateInitialContents();
			pointerStyleCollection.PopulateInitialContents();
            scaleAnnotationStyles.PopulateInitialContents();
			textStyleCollection.PopulateInitialContents();
			markerStyleCollection.PopulateInitialContents();
			palettes.PopulateInitialContents();
			GaugeXmlSerializer.ReadObject(this,fileName);
		}

        /// <summary>
        /// Serializes this gauge's theme and style collections as XML to the specified file.
        /// </summary>
        /// <param name="fileName">The file which this gauge's theme and style collections will be serialized to.</param>
        /// <param name="serializeBitmaps">Whether to serialize bitmap data within the XML file.</param>
        public void XMLSerializeThemesAndStyles(string fileName, bool serializeBitmaps)
		{
			try
			{
				ScaleCollection scalesSaved = Scales;
				scales = new ScaleCollection();
                XMLSerialize(fileName, serializeBitmaps);
				scales = scalesSaved;
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);

				if(ex.InnerException != null)
				{
					Debug.WriteLine("Inner exception:");
					Debug.WriteLine(ex.InnerException.Message);
					Debug.WriteLine(ex.InnerException.StackTrace);
				}
			}
        }

        #endregion

		#endregion
	
		#region --- Helpers ---
		internal static bool IsInGroup(GaugeKind kind, GaugeKindGroup styleGroup)
		{
			return ((((int)kind) & (int)styleGroup) == (int)styleGroup);
		}

		private void CopyScaleLayoutFormSkin()
		{
			//overwrite ScaleLayouts of any existing Scales
			foreach (Scale s in this.Scales)
			{
				if (Skin != null && Skin.ScaleLayout != null)
					s.ScaleLayout = Skin.ScaleLayout.Copy();
			}
		}
		#endregion

		#region --- Map Areas ---

		internal MapAreaCollection MapAreas { get { return mapAreas; } }
		public MapAreaSelectionKind MapAreaSelection { get { return mapAreaSelection; } set { mapAreaSelection = value; } }
		internal bool RenderGaugesMapAreas { get { return (((int)mapAreaSelection) & ((int)MapAreaSelectionKind.Gauges)) != 0; } }
		internal bool RenderPointersMapAreas { get { return (((int)mapAreaSelection) & ((int)MapAreaSelectionKind.Pointers)) != 0; } }
		internal bool RenderRangesMapAreas { get { return (((int)mapAreaSelection) & ((int)MapAreaSelectionKind.Ranges)) != 0; } }
		internal bool RenderTickMarksMapAreas { get { return (((int)mapAreaSelection) & ((int)MapAreaSelectionKind.TickMarks)) != 0; } }
		internal bool RenderIndicatorsMapAreas { get { return (((int)mapAreaSelection) & ((int)MapAreaSelectionKind.Indicators)) != 0; } }

		#endregion

		#region --- Client-side serialization ---
		
#if WEB 

    internal Hashtable ExportJsObject()
    {
      Hashtable gauge = new Hashtable();

      gauge.Add("name", Name);

      gauge.Add("gaugeKind", GaugeKind.ToString());
      gauge.Add("themeName", ThemeName);
      gauge.Add("visible", Visible);

      gauge.Add("scales", Scales.ExportJsArray());
      gauge.Add("indicators", Indicators.ExportJsArray());
      gauge.Add("subGauges", SubGauges.ExportJsArray());

      return gauge;
    }

    internal void ImportJsObject(Hashtable gauge)
    {
      if (gauge.ContainsKey("gaugeKind"))
        GaugeKind = (GaugeKind)Enum.Parse(typeof(GaugeKind), (string)gauge["gaugeKind"]);

      if (gauge.ContainsKey("themeName"))
        ThemeName = (string)gauge["themeName"];

      if (gauge.ContainsKey("visible"))
          Visible = (bool)gauge["visible"];

      if (gauge.ContainsKey("scales"))
        Scales.ImportJsArray((IEnumerable)gauge["scales"]);

      if (gauge.ContainsKey("indicators"))
        Indicators.ImportJsArray((IEnumerable)gauge["indicators"]);

      if (gauge.ContainsKey("subGauges"))
        SubGauges.ImportJsArray((IEnumerable)gauge["subGauges"]);
    }

#endif

		#endregion

		#region --- Handling Value Changed Event ---

		internal void HandleValueChange(object sender, double oldValue, double newValue)
		{
			ValueChangedEventArgs eventArgs = new ValueChangedEventArgs(oldValue, newValue);
			if(ValueChanged != null)
				ValueChanged(sender,eventArgs);

		}
		#endregion

	}

	// =======================================================================================================

    /// <summary>
    /// Contains a collection of <see cref="Gauge"/> objects.
    /// </summary>
	[Serializable]
	public class SubGaugeCollection : NamedObjectCollection
	{
		internal override NamedObject CreateNewMember()
		{
			SubGauge newMember = new SubGauge();
			SelectGenericNewName(newMember);
			Add(newMember);
			newMember.RelativeLocation = new Point2D(50,25);
			newMember.RelativeSize = new Size2D(40,40);
            ObjectModelBrowser.NotifyChanged(this);
			return newMember;
		}

		
		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates new member of the collection.
		/// </summary>
		/// <param name="newMemberName">Name of the new member.</param>
		/// <returns>Returns the created object.</returns>
		public SubGauge AddNewMember(string newMemberName)
		{
			SubGauge newMember = new SubGauge(newMemberName);
			Add(newMember);
			newMember.RelativeLocation = new Point2D(50,25);
			newMember.RelativeSize = new Size2D(40,40);
			ObjectModelBrowser.NotifyChanged(this);
			
			return newMember;			
		}

		/// <summary>
		/// Clones and stores the specified <see cref="Gauge"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new SubGauge AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName,oldMemberName) as SubGauge;
		}

		#endregion

        /// <summary>
        /// Gets or sets a <see cref="Gauge"/> object at the specified name or index.
        /// </summary>
        /// <param name="ix">The name or index of the <see cref="Gauge"/> object.</param>
        /// <returns>The <see cref="Gauge"/> object located by name or index.</returns>
		public new SubGauge this[object ix]
		{
			get { return base[ix] as SubGauge; }
			set { base[ix] = value; }
		}
//
//		public new SubGauge this[string name]
//		{
//			get { return base[name] as SubGauge; }
//			set { base[name] = value; }
//		}

    #region --- Client-side serialization ---
#if WEB
    internal ArrayList ExportJsArray()
    {
      ArrayList gauges = new ArrayList();

      for (int i = 0; i < this.Count; i++)
        gauges.Insert(i, this[i].ExportJsObject());

      return gauges;
    }

    internal void ImportJsArray(IEnumerable gauges)
    {
      foreach (Hashtable gauge in gauges)
        this[(string)gauge["name"]].ImportJsObject(gauge);
    }
#endif
    #endregion

  }

	// ========================================================================================================
	
	internal class NamedConverterWithRelevanceCheck : NamedObjectConverter
	{
		public override bool GetPropertiesSupported (ITypeDescriptorContext context)
		{
			return true;
		}

		public override PropertyDescriptorCollection GetProperties (ITypeDescriptorContext context,	Object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection props = base.GetProperties(context,value,attributes);
			if(context == null || !(context.Instance is IObjectModelNode))
				return props;
			
			SubGauge gauge = null;

			object instance = context.Instance;
			PropertyDescriptor pDesInst = TypeDescriptor.GetProperties(instance,true)["SubGauge"];
			if(pDesInst != null)
				instance = pDesInst.GetValue(instance);

			if(context.Instance is SubGauge)
				gauge = context.Instance as SubGauge;
			else if(context.Instance is IObjectModelNode)
				gauge = ObjectModelBrowser.GetOwningGauge(context.Instance as IObjectModelNode);
			else if(context.Instance is IGaugeControl)
			{
				IGaugeControl gControl = context.Instance as IGaugeControl;
				gauge = ObjectModelBrowser.GetOwningTopmostGauge(gControl.Palettes);
			}
			else
				return props;

			 
			PropertyDescriptorCollection outProps = new PropertyDescriptorCollection(new PropertyDescriptor[] { });
			foreach (PropertyDescriptor pDes in props)
			{
				RelevantForAttribute relevantFor = pDes.Attributes[typeof(RelevantForAttribute)] as RelevantForAttribute;
				if(relevantFor != null && !relevantFor.IsRelevantFor(gauge.GaugeKind))
					continue;
				else
					outProps.Add(pDes);
			}
			return outProps;
		}
	}

	// =========================================================================================================

#if DEBUG
	public
#else
	internal
#endif
		class RelevantForAttribute : Attribute 
	{
		private int flags;
		public RelevantForAttribute(params GaugeKind[] kinds)
		{
			for(int i=0; i<kinds.Length; i++)
				flags = flags | (int)kinds[i];
		}
		public RelevantForAttribute(params GaugeKindGroup[] kinds)
		{
			for(int i=0; i<kinds.Length; i++)
				flags = flags | (int)kinds[i];
		}
		public bool IsRelevantFor(GaugeKind kind)
		{
			return ( flags & (int)kind ) != 0;
		}
	}

	#region --- Events ---


    /// <summary>
    /// Provides data for the <see cref="Gauge.ValueChanged"/> event.
    /// </summary>
    public class ValueChangedEventArgs : EventArgs 
	{
		private double oldValue;
		private double newValue;
		public ValueChangedEventArgs(double oldValue, double newValue) : base() 
		{
			this.oldValue = oldValue;
			this.newValue = newValue;
		}
		public double OldValue { get { return oldValue; } }
		public double NewValue { get { return newValue; } }
	}

    /// <summary>
    /// Represents the method that will handle the <see cref="SubGauge.ValueChanged"/> event of a <see cref="Gauge"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="eventArgs">The event data.</param>
    public delegate void ValueChangeHandler(object sender, ValueChangedEventArgs eventArgs);

	#endregion
}
