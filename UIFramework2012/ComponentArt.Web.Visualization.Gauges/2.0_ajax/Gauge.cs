using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

using ComponentArt.Web.Visualization.Gauges.GDIEngine;

namespace ComponentArt.Web.Visualization.Gauges
{
    /// <summary>
    /// Specifies which method to use for a client update.
    /// </summary>
    public enum ClientsideRefreshMethod { Callback, Postback }

	/// <summary>
	/// Represents a web control implementation of <see cref="Gauge"/>.
	/// </summary>
    [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
	[LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
    [Designer(typeof(GaugeDesigner))]
	[ToolboxBitmap(typeof(Gauge))]
    [ParseChildren(true)]
    [PersistChildren(false)]
    [ToolboxData("<{0}:Gauge runat=server></{0}:Gauge>")]
	public class Gauge : WebControl, IGaugeControl, IDrawableControl, IDisposable
#if FW2 || FW3 || FW35
        , ICallbackEventHandler
#endif	
    {
        #region Constants

        internal const int GAUGE_DEFAULT_WIDTH = 250;
        internal const int GAUGE_DEFAULT_HEIGHT = 250;

        #endregion

        private GaugeDesigner designer = null;

		private Factory factory = new ComponentArt.Web.Visualization.Gauges.GDIEngine.GDIFactory();

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal SubGauge SubGauge { 
			set 
			{
				m_gauge = value;

				m_gauge.SetFactory(factory);
				m_gauge.BackColor = Color.Transparent;
				m_gauge.VisualChanged += new VisualChangeHandler(OnVisualChanged);
				m_gauge.gaugeWrapper = this;
			} 
			get 
			{
				return m_gauge;
			} 
		}

		#region --- Rendering Message on Exception ---
		/// <summary>
		/// Gets or sets exception handling mode.
		/// </summary>
		/// <remarks>
		/// If this property is set to "true" and an exception is thrown during rendering, the error message is rendered in the control drawing area. If the value is 
		/// "false", the exception will be thrown unless there is an ExceptionEvent handler registered, which is envoked in that case. Default property value is "true".
		/// </remarks>
		[DefaultValue(true)]
		[Category("Exception Handling")]
		[Description("Exception handling mode")]
		public bool RenderMessageOnException { get { return m_gauge.RenderErrorMesage; } set { m_gauge.RenderErrorMesage = value; } }

		/// <summary>
		/// Delegate used for handling the <see cref="Gauge.ErrorHandler"/> event.
		/// </summary>
		public delegate void GaugeErrorHandler(object sender, ErrorEventArgs e);

		/// <summary>
		/// Event is raised when <see cref="Gauge.RenderMessageOnException"/> is set to false and there is a registered event handler.
		/// If no event handler is registered, exception is just rethrown.
		/// </summary>
		[Category("Exception Handling")]
		public event GaugeErrorHandler ErrorHandler; 

		#endregion

        #region --- IGaugeControl Properties ---

		#region --- Control Properties ---
		/// <summary>
		/// The ID of this instance of WebGauge
		/// </summary>
		public string Name { get{ return this.ID;} }
		#endregion

        internal GaugeDesigner Designer { set { designer = value; } get { return designer; } }

        #region --- Serialization Mode ---

        internal bool InSerialization { get { return m_gauge.InSerialization; } set { m_gauge.InSerialization = value; } }

        #endregion

        #region --- Appearance Category ---
        /// <summary>
        /// The color of the background in the image that lies outside the gauge.  Default is transparent.
        /// </summary>
		[RefreshProperties(RefreshProperties.All)]
        [Category("Appearance")]
        [Description("Background Color")]
        [DefaultValue(typeof(Color), "Transparent")]
        public new Color BackColor { get { return m_gauge.BackColor; } set { m_gauge.BackColor = value; } }

//        [RefreshProperties(RefreshProperties.All)]
//        [Category("Appearance")]
//        [Description("Background image layout")]
//#if __COMPILING_FOR_2_0_AND_ABOVE__
//        [DefaultValue(typeof(System.Windows.Forms.ImageLayout),"Tile")]
//        // NB: Here we need translations between ComponentArt.Web.Visualization.Gauges.ImageLayout and ystem.Windows.Forms.ImageLayout
//        public override System.Windows.Forms.ImageLayout BackgroundImageLayout { get { return m_gauge.BackgroundImageLayout; } set { m_gauge.BackgroundImageLayout = value; } }
//#else
//        [DefaultValue(typeof(ImageLayout), "Tile")]
//        public ImageLayout BackgroundImageLayout { get { return m_gauge.BackgroundImageLayout; } set { m_gauge.BackgroundImageLayout = value; } }
//#endif
        #endregion

        #region --- "Gauge Active Style" Category ---
		/// <summary>
		/// The geometry or look of the gauge. Can be Circular, Linear, Numeric, etc.
		/// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [Category("Gauge Active Style")]
        [Description("Gauge kind")]
        [DefaultValue(typeof(GaugeKind), "Circular")]
        public GaugeKind GaugeKind { get { return m_gauge.GaugeKind; } set { m_gauge.GaugeKind = value; } }

		/// <summary>
		/// Holds settings related to the visual theme of the gauge.
		/// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [Category("Gauge Active Style")]
        [Description("Gauge theme")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [XmlIgnore]
        public Theme Theme { get { return m_gauge.Theme; } }

		/// <summary>
		/// Selects a Theme for the overall visual look of the gauge.  Available themes are: Default, Black Ice, Arctic White, Monochrome
		/// </summary>
		[RefreshProperties(RefreshProperties.All)]
		[Category("Gauge Active Style")]
		[Description("Gauge theme name")]
		[TypeConverter(typeof(ThemeNameConverter))]
		public string ThemeName { get { return m_gauge.ThemeName; } set { m_gauge.ThemeName = value; } }

		/// <summary>
		/// Selects a Theme for the overall visual look of the gauge.  Available themes are: Default, Black Ice, Arctic White, Monochrome
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ThemeKind ThemeKind { get { return m_gauge.ThemeKind; } set { m_gauge.ThemeKind = value; } }

		/// <summary>
		/// Holds setting related to image layers used in the theme for the selected GaugeKind.
		/// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [Category("Gauge Active Style")]
        [Description("Gauge skin")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [XmlIgnore]
        public Skin Skin { get { return m_gauge.Skin; } }

        #endregion

        #region --- "Gauge Data" Category ---

		/// <summary>
		/// Minimum value of the main scale
		/// </summary>
        [Category("Gauge Data")]
        [Description("Minimum value of the main scale")]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [XmlIgnore]
        public double MinValue { get { return m_gauge.MinValue; } set { m_gauge.MinValue = value; } }

		/// <summary>
		/// Maximum value of the main scale
		/// </summary>
        [Category("Gauge Data")]
        [Description("Maximum value of the main scale")]
        [DefaultValue(100)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [XmlIgnore]
        public double MaxValue { get { return m_gauge.MaxValue; } set { m_gauge.MaxValue = value; } }

        // Note: The main gauge value is handled by these two properties: 'InitialValue' and 'Value'
        // 'InitialValue' is design-time only and uses slider editor, adjusts itself to the value range and
        // actually controls the 'Value' property. 

		/// <summary>
		/// Initial value of the main pointer. Property only used at design time, Value property should be used for run-time.
		/// </summary>
        [Category("Gauge Data")]
        [Description("Initial value of the main pointer")]
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Editor(typeof(SliderEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DesignOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SliderValue InitialValue { get { return new SliderValue(MinValue, MaxValue, m_gauge.Value, 0); } set { Value = value.Value; } }

		/// <summary>
		/// Value of the main pointer.
		/// </summary>
        [Category("Gauge Data")]
        [Description("Value of the main scale")]
        [DefaultValue(0)]
        [Browsable(false)]
        public double Value { get { return m_gauge.Value; } set { m_gauge.Value = value; } }
        
        #endregion

        #region --- "Annotations" Category ---

		/// <summary>
		/// A collection of all text annotations in the Gauge.
		/// </summary>
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Gauge Annotations")]
        public TextAnnotationCollection TextAnnotations
        {
            get { return m_gauge.TextAnnotations; }
        }

		/// <summary>
		/// A collection of all image annotations in the Gauge.
		/// </summary>
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Gauge Annotations")]
        public ImageAnnotationCollection ImageAnnotations
        {
            get { return m_gauge.ImageAnnotations; }
        }

        #endregion

        #region --- "Indicators" Category ---

		/// <summary>
		/// Collection of all the Indicators in this gauge.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[Category("Indicators")]
        [DefaultValue(null)]
        [NotifyParentProperty(true)]
        public IndicatorCollection Indicators { get { return m_gauge.Indicators; } }

        #endregion

        #region --- Main Gauge Objects ---
		/// <summary>
		/// The first range in the first (Main) scale.
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Gauge Main Objects")]
        public Range MainRange
        {
            get { return m_gauge.MainRange; }
            set { m_gauge.MainRange = value; }
        }

		/// <summary>
		/// The main annotation object in the main range of the main scale.
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Gauge Main Objects")]
        public Annotation MainAnnotation
        {
            get { return m_gauge.MainAnnotation; }
            set { m_gauge.MainAnnotation = value; }
        }

		/// <summary>
		/// The first pointer of the Main (first) scale.
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Gauge Main Objects")]
        public Pointer MainPointer
        {
            get { return m_gauge.MainPointer; }
            set { m_gauge.MainPointer = value; }
        }

		/// <summary>
		/// The first scale in the gauge.
		/// </summary>
        [RefreshProperties(RefreshProperties.Repaint)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Gauge Main Objects")]
        public Scale MainScale
        {
            get { return m_gauge.MainScale; }
            set { m_gauge.MainScale = value; }
        }
        #endregion

        #region --- Collections ---
		/// <summary>
		/// A collection of subgauges that live within this gauge.
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //[RefreshProperties(RefreshProperties.All)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Gauge Collections")]
        [NotifyParentProperty(true)]
        public SubGaugeCollection SubGauges { get { return m_gauge.SubGauges; } }

		/// <summary>
		/// A collection of all scales in this gauge.
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Gauge Collections")]
        public ScaleCollection Scales { get { return m_gauge.Scales; } }

		/// <summary>
		/// A collection of all ranges in the Main (primary) scale of the gauge.
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Gauge Collections")]
        public RangeCollection Ranges { get { return m_gauge.Ranges; } }

		/// <summary>
		/// A collection of all the pointers in the Main (primary) scale of the gauge.
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Gauge Collections")]
        public PointerCollection Pointers { get { return m_gauge.Pointers; } }

		/// <summary>
		/// A collection of all Annotations in the Main (primary) range of the gauge.
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Gauge Collections")]
        public AnnotationCollection Annotations { get { return m_gauge.Annotations; } }

        #endregion

        #region --- Style Collections ---

		/// <summary>
		/// A collection of all available themes
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [RefreshProperties(RefreshProperties.All)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Gauge Style Collections")]
        [Description("Active theme")]
        public ThemeCollection Themes { get { return m_gauge.Themes; } }

		/// <summary>
		/// A collection of all available pointer styles
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [RefreshProperties(RefreshProperties.All)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Gauge Style Collections")]
        [Description("Collection of pointer styles")]
        public PointerStyleCollection PointerStyles { get { return m_gauge.PointerStyles; } }

		/// <summary>
		/// A collection of all available text styles
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [RefreshProperties(RefreshProperties.All)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Gauge Style Collections")]
        [Description("Collection of text styles")]
        public TextStyleCollection TextStyles { get { return m_gauge.TextStyles; } }

		/// <summary>
		/// A collection of all available Marker Styles
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [RefreshProperties(RefreshProperties.All)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Gauge Style Collections")]
        [Description("Collection of marker styles")]
        public MarkerStyleCollection MarkerStyles { get { return m_gauge.MarkerStyles; } }

		/// <summary>
		/// A collection of all available Palettes
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [RefreshProperties(RefreshProperties.All)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Gauge Style Collections")]
        [Description("Gauge color palettes")]
        public GaugePaletteCollection Palettes { get { return m_gauge.Palettes; } }

		/// <summary>
		/// A collection of all available scale annotation styles
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [RefreshProperties(RefreshProperties.All)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Gauge Style Collections")]
        [Description("Gauge scale annotation styles")]
        public ScaleAnnotationStyleCollection ScaleAnnotationStyles { get { return m_gauge.ScaleAnnotationStyles; } }

        #endregion

        #endregion

        #region --- Public WebGauge Properties ---

        bool m_saveImageOnDisk = true;
        /// <summary>
        /// Gets or sets the value indicating if the image should be saved on disc or in the cache.
        /// </summary>
        [DefaultValue(true)]
        [Description("Indicating if the image should be saved on disc or in the cache.")]
        public bool SaveImageOnDisk
        {
            get 
            { 
                return m_saveImageOnDisk; 
            }
            set
            {
                if (m_saveImageOnDisk == value)
                    return;

                if (!DesignMode)
                {
					//TODO: Figure out why this line doesn't work and if we need a workaround (FW1)
                    //Page.Cache.Remove(Key);
                }

                m_saveImageOnDisk = value;
            }
        }

        private string m_customFileName = null;
        /// <summary>
        /// The custom image filename without an extension.
        /// </summary>
        /// <remarks>
        /// If this property is set (i.e. it's not null nor empty), it is used to create the image filename. Otherwise,
        /// the control creates a unique image filename for each rendered chart. Note that the file extension is appended
        /// to the filename, according to the <see cref="WebChartImageType"/> property.
        /// </remarks>
        [Description("Custom image file name. If not provided, control creates unique file name for each chart.")]
        [DefaultValue("")]
        public string CustomImageFileName 
        { 
            get { return m_customFileName; }
            set { m_customFileName = value; } 
        }

        string m_imageOutputDirectory = String.Empty;
        /// <summary>
        /// Gets or sets the output directory for the image.
        /// </summary>
        [DefaultValue(".")]
        [Description("Output directory for the image.")]
        public string ImageOutputDirectory
        {
            get
            {
                return m_imageOutputDirectory;
            }
            set
            {
                m_imageOutputDirectory = value.Trim();
            }
        }

        int m_cacheInterval = 0;
        /// <summary>
        /// Gets or sets the duration in seconds for which the chart is valid and does not need to be rerendered.
        /// </summary>
        [DefaultValue(0)]
        [Description("Duration in seconds for which the chart is valid.")]
        public int CacheInterval
        {
            get { return m_cacheInterval; }
            set { m_cacheInterval = value; }
        }

        const int deletionDelay_Default = 15;
        int m_deletionDelay = deletionDelay_Default;
        /// <summary>
        /// Gets or sets the amount of time in seconds that the image will exist after the cache interval has expired.
        /// </summary>
        /// <remarks>
        /// DeletionDelay is intended to keep the image valid long enough for it to be requested by the client.
        /// Therefore, for pages that could take a long time to load set DeletionDelay to a larger interval.
        /// DeletionDelay is mostly used if the <see cref="CacheInterval"/> is set to 0.
        /// 
        /// When the image is stored in the cache (not on disk) and the <see cref="CacheInterval"/> is set to 0, the image will be deleted after the first access.
        /// </remarks>
        [DefaultValue(deletionDelay_Default)]
        [Description("The amount of time in seconds that the image will exist after the cache interval has expired.")]
        public int DeletionDelay
        {
            get { return m_deletionDelay; }
            set { m_deletionDelay = value; }
        }

        const int jpegQuality_Default = 85;
        int m_jpegQuality = jpegQuality_Default;
        /// <summary>
        /// Gets or sets the jpeg quality of the image.
        /// </summary>
        [DefaultValue(jpegQuality_Default)]
        [Description("Jpeg quality of the image.")]
        public int JpegQuality
        {
            get
            {
                return m_jpegQuality;
            }
            set
            {
                if (value < 0)
                    m_jpegQuality = 0;
                else if (value > 100)
                    m_jpegQuality = 100;
                else
                    m_jpegQuality = value;
            }
        }

        const ImageType _defaultImageType = ImageType.Png;
        ImageType m_imageType = _defaultImageType;
        /// <summary>
        /// Gets or sets the format of the output image.
        /// </summary>
        [DefaultValue(_defaultImageType)]
        [Description("Indicates the file type of the image. Choose from Gif, Jpeg or Png.")]
        public ImageType ImageType
        {
            get { return m_imageType; }
            set { m_imageType = value; }
        }

        /// <summary>
        /// Returns true if the current Request is a callback for this gauge instance; otherwise, false.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCallback 
        {
            get 
            {
                if (DesignMode)
                    return false;
                    
                return (Context.Request.QueryString[this.UniqueID + "_Callback"] != null);
            }
        }
        
        #region --- Client-side Behavior ---        
        private string m_loadingImagePath = String.Empty;
        /// <summary>
        ///		When set, shows "loading" image between gauge refreshes.
        /// </summary>
        [Description("Loading image shown between gauge refreshes.")]
        [Category("Client-side Behavior")]
        [DefaultValue("")]
        public string LoadingImagePath
        {
            get { return m_loadingImagePath; }
            set { m_loadingImagePath = (value == null ? String.Empty : value); }
        }

        private string m_errorImagePath = String.Empty;
        /// <summary>
        ///		When set, shows image if error occurs during AJAX request.
        /// </summary>
        [Description("Error image shown if error occurs during AJAX request.")]
        [Category("Client-side Behavior")]
        [DefaultValue("")]
        public string ErrorImagePath
        {
            get { return m_errorImagePath; }
            set { m_errorImagePath = (value == null ? String.Empty : value); }
        }  
  
        private bool m_EnableClientsideApi = false;
        /// <summary>
        ///		When set to true, loads and initializes the JavaScript clientside API.
        /// </summary>
        [Description("Loads and initializes the JavaScript clientside API for use in client browser")]
        [Category("Client-side Behavior")]
        [DefaultValue(false)]
        public bool ClientsideApiEnabled
        {
            get { return m_EnableClientsideApi; }
            set { this.m_EnableClientsideApi = value; }
        }

        /// <summary>
        ///		When set to true, loads and initializes the JavaScript clientside API.
        /// </summary>
        [Description("Renders an area map for gauges for client-side mouse events.")]
        [Category("Client-side Behavior")]
        [DefaultValue(false)]
        public bool RenderGaugesMapAreas
        {
              get { return m_gauge.RenderGaugesMapAreas; }
              set 
              { 
                if (value) 
                    m_gauge.MapAreaSelection |= MapAreaSelectionKind.Gauges;
                else
                    m_gauge.MapAreaSelection &= ~MapAreaSelectionKind.Gauges;
              }
        }

        /// <summary>
        ///		When set to true, loads and initializes the JavaScript clientside API.
        /// </summary>
        [Description("Renders an area map for pointers for client-side mouse events.")]
        [Category("Client-side Behavior")]
        [DefaultValue(false)]
        public bool RenderPointersMapAreas
        {
            get { return m_gauge.RenderPointersMapAreas; }
            set
            {
                if (value)
                    m_gauge.MapAreaSelection |= MapAreaSelectionKind.Pointers;
                else
                    m_gauge.MapAreaSelection &= ~MapAreaSelectionKind.Pointers;
            }
        }

        /// <summary>
        ///		When set to true, loads and initializes the JavaScript clientside API.
        /// </summary>
        [Description("Renders an area map for indicators for client-side mouse events.")]
        [Category("Client-side Behavior")]
        [DefaultValue(false)]
        public bool RenderIndicatorsMapAreas
        {
            get { return m_gauge.RenderIndicatorsMapAreas; }
            set
            {
                if (value)
                    m_gauge.MapAreaSelection |= MapAreaSelectionKind.Indicators;
                else
                    m_gauge.MapAreaSelection &= ~MapAreaSelectionKind.Indicators;
            }
        }
        
        private bool m_ClientsideCustomizedImageCachingEnabled = false;
		/// <summary>
		///		When set to true, gauges which are customized through the client-side API will be cached for re-use
		/// </summary>
		[Description("Whether gauges customized through the client-side API will be cached for re-use")]
        [Category("Client-side Behavior")]
        [DefaultValue(false)]
		public bool ClientsideCustomizedImageCachingEnabled
		{
            get { return m_ClientsideCustomizedImageCachingEnabled; }
            set { this.m_ClientsideCustomizedImageCachingEnabled = value; }
		}

        private ClientsideRefreshMethod m_RefreshMethod = ClientsideRefreshMethod.Callback;
        /// <summary>
        /// Specifies whether the client refresh method should be callback or postback.
        /// </summary>
        [Description("Specifies whether the client refresh method should be callback or postback.")]
        [Category("Client-side Behavior")]
        [DefaultValue(ClientsideRefreshMethod.Callback)]
        public ClientsideRefreshMethod RefreshMethod
        {
            get { return m_RefreshMethod; }
            set { m_RefreshMethod = value; }
        }

        private double m_refreshInterval = 0;
        /// <summary>
        /// Specifies the interval at which the gauge is auto-refreshed.
        /// </summary>
        [Description("Specifies the interval at which the gauge is auto-refreshed.")]
        [Category("Client-side Behavior")]
        [DefaultValue(0)]
        public double RefreshInterval
        {
            get { return m_refreshInterval; }
            set { m_refreshInterval = value; }
        }

        private bool m_autoRefreshOnChange = false;
        /// <summary>
        /// Specifies whether the gauge will auto-refresh when a property has been changed.
        /// </summary>
        [Description("Specifies whether the gauge will auto-refresh when a property has been changed.")]
        [Category("Client-side Behavior")]
        [DefaultValue(false)]
        public bool AutoRefreshOnChange
        {
            get { return m_autoRefreshOnChange; }
            set { m_autoRefreshOnChange = value; }
        }

        private GaugeClientEvents m_clientEvents = null;
        /// <summary>
        /// Configures client event handlers that fire on certain client-side actions of WebGauge.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Description("Client event handlers that fire on certain client-side actions of WebGauge.")]
        [Category("Client-side Behavior")]
        public GaugeClientEvents ClientEvents
        {
            get
            {
                if (m_clientEvents == null)
                    m_clientEvents = new GaugeClientEvents();

                return m_clientEvents;
            }
        }
        
        #endregion

        /// <summary>
        /// Relative or absolute path to the folder containing the client-side script file(s).
        /// </summary>
        /// <remarks>
        /// The actual JavaScript files are placed in a folder named to correspond to the Gauge version being used, inside a folder named
        /// <b>componentart_gauge_client</b>, which is placed in the folder specified by this property.
        /// </remarks>
        [Category("Support")]
        [DefaultValue("")]
        [Description("Relative or absolute path to the folder containing the client-side script file(s).")]
        public string ClientScriptLocation
        {
            get
            {
                object o = ViewState["ClientScriptLocation"];
                return (o == null) ? String.Empty : Utils.ConvertUrl(Context, string.Empty, (string)o);
            }
            set
            {
                ViewState["ClientScriptLocation"] = value;
            }
        }
        
        #endregion

        #region --- Internal WebGauge Properties ---

        private string m_key = null;
        internal string Key
        {
            get
            {
                if (Context == null)
                    return null;
                    
                if (m_key == null)
                    m_key = Context.Request.PhysicalPath + "+++" + this.UniqueID;

                return m_key;
            }
            set
            {
                m_key = value;
            }
        }

        private string m_designModeFileName = null;
        internal string DesignModeFileName
        {
            get { return m_designModeFileName; }
            set { m_designModeFileName = value; }
        }


        private string m_clientCustomizations = null;
        internal string ClientCustomizations
        {
            get 
            {
                if (m_clientCustomizations == null) 
                {
                    //get the user customizations string from the request
                    m_clientCustomizations = Context.Request.Form[m_customizationsInputFieldName];
                    m_clientCustomizations = (m_clientCustomizations != null ? m_clientCustomizations : "");
                }
                
                return m_clientCustomizations; 
            }
        }
        
        private string m_clientCustomProperties = null;
        internal string ClientCustomProperties
        {
            get
            {
                if (m_clientCustomProperties == null)
                {
                    //get the user customizations string from the request
                    m_clientCustomProperties = Context.Request.Form[m_customizationsInputFieldName + "_properties"];
                    m_clientCustomProperties = (m_clientCustomProperties != null ? m_clientCustomProperties : "");
                }
                
                return m_clientCustomProperties;
            }
        }

        internal string CustomizationKey
        {
            get
            {
                // Hash code maps same customizations to the same storage Key.  
                // However there is a 1 in 2^32 chance of collision.
                return (ClientCustomizations + ClientCustomProperties).GetHashCode().ToString();
            }
        }
        
        internal bool RenderAreaMaps
        {
            get { return (m_gauge.MapAreaSelection != MapAreaSelectionKind.None); }
        }
        
        #endregion

        #region --- Private Member Variables ---

        private SubGauge m_gauge;
        private ImageManager m_imageManager;
        private Color backgroundColor = Color.White;

        // Client-side API properies
        private bool m_customizationsLoaded = false;
        private bool m_customized = false;
		
        private string m_defaultProperties = String.Empty;
        private bool m_defaultPropertiesSaved = false;

        private string m_customizationsInputFieldName = String.Empty;

        private Hashtable m_customProperties = new Hashtable();

        #endregion

        #region --- Exposed Methods ---

		/// <summary>
		/// Creates an instance of the WebGauge control with default settings.
		/// </summary>
        public Gauge() : base()
        {
            Width = GAUGE_DEFAULT_WIDTH;
            Height = GAUGE_DEFAULT_HEIGHT;

			licence = GetLicenseType();
            m_gauge = new SubGauge();
            m_gauge.SetFactory(factory);
			m_gauge.gaugeWrapper = this;

            m_imageManager = null;

            // The setter method of this property will also calculate the physical path and set
            // the physical path property in the image manager
            ImageOutputDirectory = ".";
			m_gauge.BackColor = Color.Transparent;
            m_gauge.VisualChanged += new VisualChangeHandler(OnVisualChanged);

        }

		public override void Dispose()
		{
			if(factory != null)
				factory.Dispose();
			base.Dispose();
		}

		internal void DisposeFactory()
		{
			if(factory != null)
				factory.Dispose();
			factory = null;
		}


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

			m_gauge.InDesignMode = this.DesignMode;
            m_customizationsInputFieldName = "CustomGauge_" + this.UniqueID;

            LoadCustomProperties();
        }
        
        private void OnVisualChanged(object sender)
        {
            if (designer != null)
                designer.ControlChanged();
        }

        
//        /// <summary>
//        /// FW 3.5 and AJAX scripts loaded with the Script Manager have to be done at this stage
//        /// </summary>
//        protected override void OnLoad(EventArgs e)
//        {
//            base.OnLoad(e);
            
//#if FW3 || FW35
//            RegisterAtlasScripts();
//#endif            
//        }

        /// <summary>
        /// Base class method overriden.
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

#if FW3 || FW35
            RegisterAtlasScripts();
#endif
            RegisterClientScripts();
            
            if (IsCallback)            
                RaiseCallbackEvent();
        }
        
		internal void OverwriteInternalDesignMode(bool mode)
		{
			m_gauge.InDesignMode = mode;
		}

#if !(FW2 || FW3 || FW35)
        protected internal bool DesignMode { get { return (Context == null); } }
#endif

        /// <summary>
        /// Base class method overriden.
        /// </summary>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            this.AddAttributesToRender(writer);
            writer.RenderBeginTag("img");
        }

        /// <summary>
        /// Base class method overriden.
        /// </summary>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Src, m_designModeFileName);

            if (this.BorderWidth.IsEmpty)
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
        }

        private void RenderDesignMode(HtmlTextWriter output)
        {
            if (m_imageManager == null)
                m_imageManager = new SavingImageManager();

            //m_imageManager.StorageKey = Key;
            //m_imageManager.CustomImageFileName = m_designModeFileName.Substring(m_designModeFileName.LastIndexOf("\\") + 1, 
            //                                                                    m_designModeFileName.Length - m_designModeFileName.LastIndexOf("\\") - 1);
            //m_imageManager.VirtualOutputDirectory = m_designModeFileName.Substring(0, m_designModeFileName.LastIndexOf("\\"));
            //m_imageManager.PhysicalOutputDirectory = m_imageManager.VirtualOutputDirectory;

            m_imageManager.DesignModeFileName = DesignModeFileName;
            
            RenderImage();

            base.Render(output);
        }

		private void RenderUnlicensedMessage(HtmlTextWriter output)
		{
			output.Write("<div style=\"height:" + this.Height + ";width:" + this.Width + ";background-color:#3F3F3F;border:1px;border-style:solid;border-bottom-color:black;border-right-color:black;border-left-color:lightslategray;border-top-color:lightslategray;color:cornsilk;padding:2px;font-family:verdana;font-size:11px;\">");
			output.Write("<b>Unlicensed control. Click <a href=\"http://www.componentart.com/unlicensedMessage.aspx\" style=\"color:red;\">here</a> for more information.</b>  ");
			output.Write("</div>");
		}

        /// <summary>
        /// Base class method overriden.
        /// </summary>
        protected override void Render(HtmlTextWriter output)
        {
            // ---------------------
            // Design time rendering
            // ---------------------
            if (DesignMode)
            {
                RenderDesignMode(output);
                return;
            }

			if (licence == LicenseType.Expired)
			{
				RenderUnlicensedMessage(output);
				return;
			}

            // ------------------
            // Run-Time Rendering
            // ------------------
            if (!m_customizationsLoaded)
                LoadCustomizations();

            ProcessImage();

            string imageUrl = m_imageManager.GetImageUrl(ImageType);

            // Is this a callback? Handle it now
            if (IsCallback)
            {
                Context.Response.Clear();
                Context.Response.ContentType = "text/xml";
                Context.Response.Write(CreateCallbackResponse(imageUrl, (RenderAreaMaps ? CreateInnerAreaMap() : "")));
                Context.Response.End();
                return;
            }

            // The HTML to be output
            string mapID = this.UniqueID + "_map";
            string containerID = this.UniqueID + "_container";

            string areaMapRef = (RenderAreaMaps ? ("usemap=\"#" + mapID + "\"") : "");

            StringBuilder clientSideHtml = new StringBuilder();
            
            // open div container
            clientSideHtml.Append("<div id='" + containerID + "' style=\"height:" + this.Height + ";width:" + this.Width + ";");
            
            if (m_loadingImagePath != null && m_loadingImagePath != String.Empty)
                clientSideHtml.Append("background-image:url(" + m_loadingImagePath + ");background-repeat:no-repeat;background-position:center;");

            clientSideHtml.Append("\">");
            
            // gauge image 
            clientSideHtml.Append("<img id='" + this.UniqueID + "' src='" + imageUrl + "' border='0' style=\"height:" + this.Height + ";width:" + this.Width + ";\" " + areaMapRef + " />");
            
            // close div container
            clientSideHtml.Append("</div>\n");            
                       
            if (m_EnableClientsideApi)
            {
                string customizationsID = this.m_customizationsInputFieldName;
                string customPropertiesID = this.m_customizationsInputFieldName + "_properties";

                clientSideHtml.Append("<input type='hidden' name='" + customizationsID + "' id='" + customizationsID + "' value=''>\n");
                clientSideHtml.Append("<input type='hidden' name='" + customPropertiesID + "' id='" + customPropertiesID + "' value=''>\n");

                string licenseParam = (licence == LicenseType.Full ? ",true" : "");
                                
                StringBuilder startupScripts = new StringBuilder();
                startupScripts.Append("<script type=\"text/javascript\"> \n");
                startupScripts.Append("window." + this.UniqueID + " = new ComponentArt.Web.Visualization.Gauges.Gauge('" + this.UniqueID + "'" + licenseParam + ");\n");

                // Write postback function reference
                startupScripts.Append(this.UniqueID + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");

                startupScripts.Append(this.UniqueID + ".loadData(" + m_defaultProperties + ");\n");

                //Then the customizations that the user created in their chart to persist back in the page
                //NOTE: We have to do this in case user chooses to submit customizations on postback instead of callback
                //      as in that case there is no persistance of clientside objects.
                if (!ClientCustomizations.Equals(""))
                    startupScripts.Append(this.UniqueID + ".loadCustomizations(" + ClientCustomizations + ");\n");

                if (m_customProperties.Count > 0) 
                    startupScripts.Append(this.UniqueID + ".loadCustomProperties(" + JsonUtils.Escape(m_customProperties) + ");\n");
                 
                // bind client events AFTER loading data and customizations
                startupScripts.Append(this.UniqueID + ".bindClientEvents(" + Utils.ConvertClientEventsToJsObject(ClientEvents) + ");\n");

                // finalize anything!
                startupScripts.Append(this.UniqueID + ".initialize();\n");       
                startupScripts.Append("</script>\n");

                WriteStartupScript(clientSideHtml, startupScripts.ToString());

                if (RenderAreaMaps)
                {
                    clientSideHtml.Append("<map id=\"" + mapID + "\" name=\"" + mapID + "\">\n");
                    clientSideHtml.Append(CreateInnerAreaMap());
                    clientSideHtml.Append("</map>\n");
                }
            }

            output.Write(clientSideHtml.ToString());            
        }

        private string CreateInnerAreaMap() 
        {
          StringBuilder map = new StringBuilder();
          MapAreaCollection mapAreas = m_gauge.MapAreas;

          foreach (MapArea area in mapAreas)
          {
            string path = area.ObjectString;
            string type = (area.Object.GetType() == typeof(SubGauge) ? "Gauge" : area.Object.GetType().Name);            

            map.Append("<Area shape=\"" + area.Kind.ToString() + "\" ");

            map.Append("onclick=\"window." + this.UniqueID + ".doMouseEvent('Click','" + type + "','" + path + "', event)\" ");
            map.Append("onmouseout=\"window." + this.UniqueID + ".doMouseEvent('Exit','" + type + "','" + path + "', event)\" ");
            map.Append("onmouseover=\"window." + this.UniqueID + ".doMouseEvent('Hover','" + type + "','" + path + "', event)\" ");

            map.Append("coords=\"");
            
            for (int i = 0; i < area.Coords.Length; i++)            
              map.Append((i == 0 ? "" : ",") + area.Coords[i]);

            map.Append("\" />\n");
          }
          
          return map.ToString();
        }
        
		/// <summary>
		/// Retrieves the value of a custom property that has been set on the client-side API.
		/// </summary>
		/// <param name="key">key of the custom property</param>
		/// <returns>string value of the property</returns>
        public string GetCustomProperty(string key) 
        {
            if (m_customProperties.ContainsKey(key)
                && m_customProperties[key] != null)
                return m_customProperties[key].ToString();
                
            return null;            
        }

		/// <summary>
		/// Sets the value of a custom property that can be retrieved through the client-side API
		/// </summary>
		/// <param name="key">key of the custom property</param>
		/// <param name="value">value of the custom property</param>
        public void SetCustomProperty(string key, string value)
        {
            m_customProperties[key] = value;
        }

        #endregion

        #region --- Helper Methods ---

        private void ProcessImage()
        {
            // Create an appropriate image manager object
            if (m_imageManager == null)
            {
                if (m_saveImageOnDisk)
                    m_imageManager = new SavingImageManager();
                else
                    m_imageManager = new CachingImageManager();
            }

            //Set common image manager properties
            m_imageManager.Cache = Page.Cache;
            m_imageManager.StorageKey = Key + (m_customized ? CustomizationKey : "");

            //Set properties specific to SavingImageManager
            if (m_saveImageOnDisk)
            {
                string outputDir = (m_imageOutputDirectory == "" ? "." : m_imageOutputDirectory);

                if (Utils.IsUrlAbsolute(outputDir) || (!outputDir.StartsWith("/") && Path.IsPathRooted(outputDir)))
                    outputDir = Utils.ConvertAbsoluteToVirtualPath(outputDir);

                string directory = Utils.ConvertUrl(Context, !outputDir.StartsWith("/") ? TemplateSourceDirectory : "", outputDir);

                string normalizedPath = Utils.NormalizePath(directory);
                string physicalPath = Utils.MapPhysicalPath(normalizedPath == "" ? "/" : normalizedPath);
                string relativePath = Utils.ConvertVirtualToRelativePath(normalizedPath, this.Page, this);

                m_imageManager.CustomImageFileName = m_customFileName;
                m_imageManager.VirtualOutputDirectory = relativePath;                
                m_imageManager.PhysicalOutputDirectory = physicalPath;
            }

            if (!m_imageManager.StoredImageAvailable())
                RenderImage();
        }
        
        private void RenderImage()
        {
            Bitmap bitmap = RenderBitmap((int)Width.Value, (int)Height.Value);

			//do not cache customized images if that option is disabled
			if (m_customized && !m_ClientsideCustomizedImageCachingEnabled)
				m_imageManager.StoreImage(bitmap, ImageType, JpegQuality, 0, DeletionDelay);
			else
				m_imageManager.StoreImage(bitmap, DesignMode?_defaultImageType:ImageType, JpegQuality, CacheInterval, DeletionDelay);

            bitmap.Dispose();
        }
        
		/// <summary>
		/// Returns the Bitmap image of the current WebGauge.
		/// </summary>
		/// <param name="width">width in pixels</param>
		/// <param name="height">height in pixels</param>
		/// <returns>Bitmap of the current gauge</returns>
		public Bitmap RenderBitmap(int width, int height)
		{
			Bitmap bitmap = new Bitmap(width, height);
            
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(this.BackColor);
			graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

			RenderingContext context = m_gauge.Factory.CreateContext();
			context.RenderingTarget = graphics;
			context.Size = new Size2D((float)width, (float)height);
			context.Engine = factory.CreateEngine();

			try
			{
				m_gauge.Render(context);
			}
			catch (Exception ex)
			{
				if (ErrorHandler != null)
					ErrorHandler(this, new ErrorEventArgs(ex));
				else
					throw ex;
			}

			return bitmap;
		}

		/// <summary>
		/// Forces the loading of customizations from the client-side.  Setting properties after calling this method will override client-side customizations.
		/// Is called automatically at render time if not called before.
		/// </summary>
        public bool LoadCustomizations()
        {
            //no need to check for customizations in design time.
            if (DesignMode)
                return false;

            // save defaults before we load client side properties
            if (!m_defaultPropertiesSaved)
                SaveDefaultProperties();
            
            // if client-side customizations not present
            if (ClientCustomizations.Equals(""))
              return false;

            Hashtable customizations = (Hashtable)JsonUtils.Parse(ClientCustomizations);

            try
            {
                ImportJsObject(customizations);
                m_customized = true;
            }
            catch (Exception e)
            {
                //ignore, or report that the value was not formatted properly 
                throw e;
            }

            m_customizationsLoaded = true;

            return true;
        }
        
        private void SaveDefaultProperties() 
        {
            //preserve the original chart settings in a string that is passed to the clientside API
            m_defaultProperties = JsonUtils.Escape(ExportJsObject());
            m_defaultPropertiesSaved = true;
        }

        private void LoadCustomProperties()
        {
            // no need to check for customizations in design time or if no client-side custom properties present
            if (DesignMode || ClientCustomProperties.Equals(""))
                return;

            Hashtable properties = (Hashtable)JsonUtils.Parse(ClientCustomProperties);

            if (properties.Count == 0)
                return;

            // add client-side entries to server-side table
            foreach (string key in properties.Keys)
                m_customProperties[key] = properties[key];

            m_customized = true;
        }        
        
        private string CreateCallbackResponse(string imageUrl, string areaMapHtml)
        {
            StringBuilder response = new StringBuilder();

            response.Append("<?xml version=\"1.0\" encoding=\"" + Context.Response.Charset + "\"?>\n");
            response.Append("<CallbackResponse><![CDATA[{");
            response.Append("\"imgUrl\":\"" + imageUrl + "\",");
            response.Append("\"data\":" + m_defaultProperties);            

            if (m_customProperties.Count > 0)
                response.Append(",\"customProperties\":" + JsonUtils.Escape(m_customProperties));
            
            if (areaMapHtml != "")
                response.Append(",\"areaMapHtml\":" + JsonUtils.Escape(areaMapHtml));

            response.Append("}]]></CallbackResponse>");

            return response.ToString();
        }

#if FW3 || FW35

        protected void RegisterScriptForAtlas(string sResourceName)
        {
            ScriptManager oScriptManager = ScriptManager.GetCurrent(Page);
            if (oScriptManager != null)
            {
                if (oScriptManager.IsInAsyncPostBack)
                {
                    ScriptManager.RegisterClientScriptResource(this, this.GetType(), sResourceName);
                }
                else
                {
                    ScriptReference oScriptReference = new ScriptReference(sResourceName, Assembly.GetExecutingAssembly().FullName);
                    if (!oScriptManager.Scripts.Contains(oScriptReference))
                    {
                        oScriptManager.Scripts.Add(oScriptReference);
                    }
                }
            }
        }
#endif

        private bool IsClientScriptBlockRegistered(string key)
        {
            return IsClientScriptBlockRegistered(this.GetType(), key);
        }

        private bool IsClientScriptBlockRegistered(Type type, string key)
        {
#if FW2 || FW3 || FW35
            return Page.ClientScript.IsClientScriptBlockRegistered(type, key);
#else
            return Page.IsClientScriptBlockRegistered(key);
#endif
        }

        private void RegisterClientScriptBlock(string key, string script)
        {
            RegisterClientScriptBlock(this.GetType(), key, script);
        }

        private void RegisterClientScriptBlock(Type type, string key, string script)
        {
#if FW2 || FW3 || FW35
            Page.ClientScript.RegisterClientScriptBlock(type, key, script);
#else
            Page.RegisterClientScriptBlock(key, script);
#endif
        }

#if FW2 || FW3 || FW35
        private string getScriptImportString(String fullScriptPath)
        {
            string sResourceUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), fullScriptPath);
            //sResourceUrl = sResourceUrl.Replace("&", "&amp;");
            return "\n<script src=\"" + sResourceUrl + "\" type=\"text/javascript\"></script>";
        }
#endif

        private string getClientScriptBlock(string sDefaultPath, string sScriptFile)
        {
#if FW2 || FW3 || FW35
            return getScriptImportString(sDefaultPath + "." + sScriptFile);
#else
            return GenerateClientScriptBlock(sDefaultPath, sScriptFile);
#endif
        }      


#if !(FW2 || FW3 || FW35)
        protected void WriteGlobalClientScript(HtmlTextWriter output, string sDefaultPath, string sScriptFile)
        {
            string sScript = GenerateClientScriptBlock(sDefaultPath, sScriptFile);
            output.Write(sScript);
        }

        private string GenerateClientScriptBlock(string sDefaultPath, string sScriptFile)
        {
            string sScript = string.Empty;
            string sScriptLocation = string.Empty;

            if (this.ClientScriptLocation != string.Empty)
            {
                sScriptLocation = Path.Combine(Path.Combine(this.ClientScriptLocation, this.VersionString), sScriptFile).Replace("\\", "/");
            }
            else
            {
                // First, try application config variable
                string sLocation = ConfigurationSettings.AppSettings["ComponentArt.Web.UI.ClientScriptLocation"];
                if (sLocation != null)
                {
                    sScriptLocation = Path.Combine(Path.Combine(sLocation, this.VersionString), sScriptFile).Replace("\\", "/");
                }

                // Next, try server root
                if (sScriptLocation == string.Empty)
                {
                    try
                    {
                        string sStandardRootClientScriptPath = Path.Combine(Path.Combine("/componentart_webui_client", this.VersionString), sScriptFile).Replace("\\", "/");

                        if (File.Exists(Context.Server.MapPath(sStandardRootClientScriptPath)))
                        {
                            sScriptLocation = sStandardRootClientScriptPath;
                        }
                    }
                    catch { }
                }

                // If failed, try application root
                if (sScriptLocation == string.Empty)
                {
                    try
                    {
                        string sAppRootClientScriptPath = Path.Combine(Path.Combine(Path.Combine(Page.Request.ApplicationPath, "componentart_webui_client"), this.VersionString), sScriptFile).Replace("\\", "/");

                        if (File.Exists(Context.Server.MapPath(sAppRootClientScriptPath)))
                        {
                            sScriptLocation = sAppRootClientScriptPath;
                        }
                    }
                    catch { }
                }
            }

            if (sScriptLocation != string.Empty)
            {
                // Do we have a tilde?
                if (sScriptLocation.StartsWith("~") && Context != null && Context.Request != null)
                {
                    string sAppPath = Context.Request.ApplicationPath;
                    if (sAppPath.EndsWith("/"))
                    {
                        sAppPath = sAppPath.Substring(0, sAppPath.Length - 1);
                    }

                    sScriptLocation = sScriptLocation.Replace("~", sAppPath);
                }

                if (File.Exists(Context.Server.MapPath(sScriptLocation)))
                {
                    sScript = "<script src=\"" + sScriptLocation + "\" type=\"text/javascript\"></script>";
                }
                else
                {
                    throw new Exception(sScriptLocation + " not found");
                }
            }
            else
            {
                // If everything failed, emit our internal script
                sScript = Utils.DemarcateClientScript(GetResourceContent(sDefaultPath + "." + sScriptFile));
            }

            return sScript;
        }

        protected string GetResourceContent(string sFileName)
        {
            try
            {
                Stream oStream = Assembly.GetAssembly(this.GetType()).GetManifestResourceStream(sFileName);
                StreamReader oReader = new StreamReader(oStream);

                return oReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not read resource \"" + sFileName + "\": " + ex);
            }
        }

        private string _versionString = null;
        internal string VersionString
        {
            get
            {
                if (_versionString == null)
                {
                    Version version = Assembly.GetExecutingAssembly().GetName().Version;
                    _versionString = version.Major.ToString() + "_" +
                                        version.Minor.ToString() + "_" +
                                        version.Build.ToString() + "_" +
                            version.Revision.ToString();
                }
                return _versionString;
            }
        }

#endif

#if FW3 || FW35
        private void RegisterAtlasScripts()
        {
            if (DesignMode) 
                return;     
                
            bool registerAtlas = (ScriptManager.GetCurrent(Page) != null);

            if (registerAtlas && !IsClientScriptBlockRegistered("ComponentArt_Atlas"))
            {
                string sAtlasScript = "window.ComponentArt_Atlas=1;";
                RegisterClientScriptBlock("ComponentArt_Atlas", Utils.DemarcateClientScript(sAtlasScript));                    
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ComponentArt_Atlas", Utils.DemarcateClientScript(sAtlasScript), false);
            }

            if (registerAtlas && m_EnableClientsideApi)
            {
                if (!IsClientScriptBlockRegistered(typeof(object), "A654u.js"))
                {
                    RegisterClientScriptBlock(typeof(object), "A654u.js", "");
                    RegisterScriptForAtlas("ComponentArt.Web.Visualization.Gauges.client_scripts.A654u.js");
                }

                if (!IsClientScriptBlockRegistered("A654k.js"))
                {
                    RegisterClientScriptBlock("A654k.js", "");
                    RegisterScriptForAtlas("ComponentArt.Web.Visualization.Gauges.client_scripts.A654k.js");
                }
            }
        }
#endif

        private void RegisterClientScripts()
        {
            if (DesignMode || !m_EnableClientsideApi)
                return;

            if (!IsClientScriptBlockRegistered(typeof(object), "A654u.js"))
            {
                string script = getClientScriptBlock("ComponentArt.Web.Visualization.Gauges.client_scripts", "A654u.js");
                RegisterClientScriptBlock(typeof(object), "A654u.js", script);
            }

            if (!IsClientScriptBlockRegistered("A654k.js"))
            {
                string script = getClientScriptBlock("ComponentArt.Web.Visualization.Gauges.client_scripts", "A654k.js");
                RegisterClientScriptBlock("A654k.js", script);
            }
        }

#if FW3 || FW35

        internal bool IsInUpdatePanel()
        {
            for (Control oControl = this.Parent; oControl != null; oControl = oControl.Parent)
            {
                if (oControl is UpdatePanel)
                {
                    return true;
                }
            }
            return false;
        }

        protected void WriteStartupScript(StringBuilder output, string sScript)
        {
            if (this.IsInUpdatePanel())
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), sScript, false);
            }
            else
            {
                output.Append(sScript);
            }
        }
#else

        protected void WriteStartupScript(StringBuilder output, string sScript)
        {
            output.Append(sScript);
        }

#endif

        #endregion

		#region --- Events ---
		//(ED)

		/// <summary>
		/// Occurs when a pointer value is changed.
		/// </summary>
		/// <remarks>
		/// The sender object is the pointer. Event arguments object contains old and new value.
		/// </remarks>
		public event ValueChangeHandler ValueChanged { add { m_gauge.ValueChanged += value; } remove { m_gauge.ValueChanged -= value; } }

        /// <summary>
        /// Occurs when a callback Request is processed.
        /// </summary>
        public event CallbackEventHandler Callback;
        
        protected virtual void RaiseCallbackEvent() 
        {
            if (Callback != null) 
                Callback(this, new CallbackEventArgs());
        }
        
        #endregion

        #region --- Client-side serialization ---

        internal Hashtable ExportJsObject()
        {
          Hashtable webGauge = m_gauge.ExportJsObject();

          webGauge.Add("refreshInterval", RefreshInterval);
          webGauge.Add("refreshMethod", RefreshMethod.ToString());
          webGauge.Add("autoRefreshOnChange", AutoRefreshOnChange);

          webGauge.Add("customizationsInputField", m_customizationsInputFieldName);
          
          webGauge.Add("loadingImagePath", m_loadingImagePath);
          webGauge.Add("errorImagePath", m_errorImagePath);

#if FW2 || FW3 || FW35
          webGauge.Add("usingICallbackEventHandler", true);
#else
          webGauge.Add("usingICallbackEventHandler", false);
#endif

          return webGauge;
        }

        internal void ImportJsObject(Hashtable webGauge)
        {
          if (webGauge.ContainsKey("refreshInterval"))
            RefreshInterval = (double)webGauge["refreshInterval"]; // from double

          if (webGauge.ContainsKey("refreshMethod"))
            RefreshMethod = (ClientsideRefreshMethod)Enum.Parse(typeof(ClientsideRefreshMethod), (string)webGauge["refreshMethod"]);

          if (webGauge.ContainsKey("autoRefreshOnChange"))
            AutoRefreshOnChange = (bool)webGauge["autoRefreshOnChange"];

          m_gauge.ImportJsObject(webGauge);
        }

        #endregion

		#region --- XML Serialization ---

		public void XMLSerialize(string fileName)
		{
			m_gauge.XMLSerialize(fileName);
		}

		public void XMLDeserialize(string fileName)
		{
			SubGauge g = new SubGauge();
			g.SetFactory(factory);

			bool oldInDesignMode = m_gauge.InDesignMode;
			SubGauge.XMLDeserialize(g,fileName);
			m_gauge = g;
			m_gauge.gaugeWrapper = this;
			m_gauge.InDesignMode = oldInDesignMode;
		}

		public void XMLSerialize(Stream outputStream)
		{
			m_gauge.XMLSerialize(outputStream);
		}

		public void XMLDeserialize(Stream inputStream)
		{
			SubGauge g = new SubGauge();
			g.SetFactory(factory);

			bool oldInDesignMode = m_gauge.InDesignMode;
			SubGauge.XMLDeserialize(g, inputStream);
			m_gauge = g;
			m_gauge.gaugeWrapper = this;
			m_gauge.InDesignMode = oldInDesignMode;
		}

		public void XMLSerializeThemesAndStyles(string fileName)
		{
			m_gauge.XMLSerializeThemesAndStyles(fileName,true);
		}

		public void XMLDeserializeThemesAndStyles(string fileName)
		{
			m_gauge.XMLDeserializeThemesAndStyles(fileName);
		}

#if DEBUG
		public void XMLSerializeBitmaps(string fileName)
		{
			m_gauge.XMLSerializeBitmaps(fileName);
		}
#endif

		#endregion


		private LicenseType licence = LicenseType.Expired;

		internal LicenseType GetLicenseType()
		{
			LicenseType type;
			try
			{
				License lic = LicenseManager.Validate(typeof(Gauge), this);
				if (lic == null)
				{
					type = LicenseType.Expired;
				}
				else
				{
					type = LicenseType.Full;
				}
			}
			catch
			{
				type = LicenseType.Expired;
			}
			return type;
		}

#if FW2 || FW3 || FW35
        #region --- ICallbackEventHandler ----
        public void RaiseCallbackEvent(string eventArgument)
        {
            if (IsCallback)
                RaiseCallbackEvent();
        }

        public string GetCallbackResult()
        {
            if (!m_customizationsLoaded)
                LoadCustomizations();

            ProcessImage();

            string imageUrl = m_imageManager.GetImageUrl(ImageType);
            string areaMapHtml = (RenderAreaMaps ? CreateInnerAreaMap() : "");

            return CreateCallbackResponse(imageUrl, areaMapHtml);
        }         
        #endregion
#endif        

    }
    
    
    #region --- Events ---

	/// <summary>
	/// Provides data for the <see cref="Gauge.ErrorHandler"/> event.
	/// </summary>
	public class ErrorEventArgs : EventArgs
	{
		public Exception exception;

		public ErrorEventArgs(Exception ex)	: base()
		{
			this.exception = ex;
		}
	}


    /// <summary>
    /// Provides data for the <see cref="WebGauge.Callback"/> event.
    /// </summary>
    public class CallbackEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackEventArgs"/> class.
        /// </summary>
        public CallbackEventArgs() : base() {}
    }

    /// <summary>
    /// Represents the method that will handle the <see cref="WebGauge.Callback"/> event of a <see cref="WebGauge"/>.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="eventArgs">The event data.</param>
    public delegate void CallbackEventHandler(object sender, CallbackEventArgs eventArgs);

    #endregion
}
