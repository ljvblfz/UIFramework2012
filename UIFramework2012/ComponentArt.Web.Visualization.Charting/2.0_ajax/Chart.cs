using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{

	/// <summary>
	/// Represents the method that will handle a <see cref="Chart"/> event.
	/// </summary>
	public delegate void WebChartEventHandler(Chart chart);

    /// <summary>
    ///		ComponentArt Chart for ASP.NET.
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
    ///     Data values are given to <see cref="Series"/> nodes using functions <see cref="Chart.DefineData"/>, 
    ///     <see cref="Chart.DefineAsExpression"/> and <see cref="Chart.DefineValuePath"/>
    ///     and property <see cref="Chart.DataSource"/>.
    ///     When data definition is complete, function <see cref="Chart.DataBind"/> is called to build the rest
    ///     of the chart object model. After that, user code may further manipulate and fine tune all
    ///     objects of the chart.
    ///   </para>
    ///   <para>
    ///     <b>It is important to understand that many objects in the chart object model are not available
    ///     before <see cref="Chart.DataBind"/> is completed.</b> The parts of the chart that depend on 
    ///     input data do not exist before <see cref="Chart.DataBind"/>. That includes <see cref="DataPoint"/>s,
    ///     <see cref="SeriesLabels"/>, <see cref="Axis"/>, <see cref="CoordinatePlane"/> objects and related objects
    ///     (because they cannot be created before data values, values ranges and type are known) and
    ///     complete inner coordinate systems (in case of multi system or multi area charts).
    ///     For more on charting data structures and data binding, see "Basic Concepts" topics and topic "Advanced Data Binding"
    ///     in "Advanced Concepts".
    ///   </para>
    ///   <para>
    ///     <b>Style</b> related data are implemented as the following style collections:
    ///         <see cref="Chart.SeriesStyles"/>,
    ///         <see cref="Chart.DataPointLabelStyles"/>,
    ///         <see cref="Chart.LineStyles"/>,
    ///         <see cref="Chart.LineStyles2D"/>,
    ///         <see cref="Chart.MarkerStyles"/>,
    ///         <see cref="Chart.GradientStyles"/>,
    ///         <see cref="Chart.LabelStyles"/>,
    ///         <see cref="Chart.TextStyles"/>,
    ///         <see cref="Chart.TickMarkStyles"/> and
    ///         <see cref="Chart.Palettes"/>.
    ///      Members of these collections are styles used to define how objects that populate the chart 
    ///      (like data points, labels, lines etc.) are rendered, or what color scheme is choosen,
    ///      in case of palettes. At the control's creation, collections are populated with a rich set of
    ///      predefined styles. They may be used to override the default styles. 
    ///      The ComponentArt Web.Visualization.Charting.Core style architecture is highly extendible, since existing styles can be customized
    ///      and new styles can be added to the collections.
    ///   </para>
    /// </remarks>

    [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
	[LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
    [TypeConverter(typeof(ExpandableWithBrowsingControlObjectConverter))]
    [DesignerAttribute(typeof(WebChartDesigner), typeof(IDesigner))]
    [ParseChildren(true)]
    [PersistChildren(false)]
    [ToolboxData(
          @"<{0}:Chart runat='server' >
 	         <Series>
               <{0}:Series Name='S0'> </{0}:Series>
               <{0}:Series Name='S1'> </{0}:Series>
             </Series>
             </{0}:Chart>")]

    public class Chart :
#if __COMPILING_FOR_2_0_AND_ABOVE__
        System.Web.UI.WebControls.DataBoundControl, 
#else
        System.Web.UI.WebControls.WebControl,
#endif
        IChart, IScrollControl, IPostBackEventHandler
    {
        DynamicImage m_di/* = new DynamicImage()*/;
        //System.Web.UI.WebControls.Image m_image = new System.Web.UI.WebControls.Image();

        double m_alpha;
        double m_beta;
        string m_customizationsInputFieldName;
        string m_ControlSettingsInputFieldName;
        string m_returnDataFormatInputFiledName;
        
        //thread for creating Highlighted series images behind the scenes.
        //Thread m_seriesHighlighterThread = null;

        //holds the filename for cashed Client-side HTML response

        Hashtable m_customPropertiesHashtable = new Hashtable();

		/// <summary>
		/// Occurs when the chart control is about to perform DataBind() operation.
		/// </summary>
		/// <remarks>
		/// Handle this event if you have to set control properties needed in a data binding
		/// operation, such as input variables, chart style and composition kind.
		/// </remarks>
		[Description("Occurs when the chart control is about to perform DataBind() operation")]
		public event WebChartEventHandler PreDataBind;

		/// <summary>
		/// Occurs after the chart control has performed DataBind() operation.
		/// </summary>
		/// <remarks>
		/// Handle this event if you have to set properties to the chart object model
		/// components not available before DataBind(), i.e. before the input data has been 
		/// proccessed, such as data points, axis annotations etc.
		/// </remarks>
		[Description("Occurs after the chart control has performed DataBind() operation.")]
		public event WebChartEventHandler PostDataBind;

		/// <summary>
		/// Occurs when the chart control throws an exception during image rendering.
		/// </summary>
		/// <remarks>
		/// The default behaviour of the chart control is to render a message with call stack 
		/// in the chart image. Handle this event to customize the behaviour. 
		/// Note that the event argument provides the exception object as well as 
		/// the graphics object that can be used to override the generated image.
		/// </remarks>
		[Description("Occurs when the chart control throws an exception.")]
		public event ChartErrorEventHandler ChartError;

        string m_text = "";

        ArrayList al = new ArrayList();

        object m_chartDesigner;

        internal object ChartDesigner
        {
            get { return m_chartDesigner; }
            set { m_chartDesigner = value; }
        }


        // DO NOT ADD ANYTHING TO COMMON CODE IN WEBCHART. 
        // ADD IT IN WINCHART AND COPY OVER.
        // THEN REPLACE WINCHART WITH WEBCHART
        #region ==== COMMON =====

        #region ======== Design Time Properties ========

        #region --- Appearance Properties ---

        /// <summary>
        /// Gets or sets the <see cref="ChartFrame"/> object of the <see cref="Chart"/>.
        /// </summary>
        [NotifyParentProperty(true)]
        [Bindable(true)]
        [Description("ChartBase frame"), Category("Appearance")]
        [DefaultValue(null)]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public ChartFrame Frame { get { return m_chart.ChartFrame; } set { m_chart.ChartFrame = value; } }

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
                if (Frame != null)
                    return Frame.TextAlignment;
                else
                    return StringAlignment.Center;
            }
            set
            {
                if (Frame != null)
                    Frame.TextAlignment = value;
            }
        }


        ChartImageInfo m_webChartImageInfo = null;
        internal ChartImageInfo WebChartImageInfo 
        {
            get {return m_webChartImageInfo;}
            set {m_webChartImageInfo = value;}
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
                if (Frame != null)
                    return Frame.TextPosition;
                else
                    return FrameTextPosition.Top;
            }
            set
            {
                if (Frame != null)
                    Frame.TextPosition = value;
            }
        }

        private ClientsideSettings m_Clientside = null;
        /// <summary>
        ///		Placeholder for all client-side properties and methods of Chart
        /// </summary>
        [NotifyParentProperty(true)]
        [Bindable(true)]
        [Description("Placeholder for all client-side properties and methods of Chart"),
        Category("Behavior")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ClientsideSettings Clientside
        {
            get
            {
                if (m_Clientside == null)
                    m_Clientside = new ClientsideSettings(this);

                return m_Clientside;
            }
        }


        //whether customizations from the clientside API have been loaded or not
        private string m_loadedCustomizations = "";
        private string m_loadedCustomProperties = "";
        private string m_defaultChartProperties = "";

        internal string m_LoadingChartImagePath = null;

        /// <summary>
        /// Allows the view range of this chart to be controlled by another control for the
        /// purposes of zooming and scrolling.
        /// /// </summary>
        internal IScrollControl MyScrollControl
        {
            get
            {
                return m_myScrollControl;
            }
            set
            {
                this.m_myScrollControl = value;
                this.m_EnableClientsideApi = true;
            }
        }
        private IScrollControl m_myScrollControl;

        bool m_customized = false;
        internal bool Customized
        { get { return m_customized; } }

        bool m_customizationsLoaded = false;
        private bool m_EnableClientsideApi = false;
        /// <summary>
        ///		When set to true, loads and initializes the JavaScript clientside API 
        ///     for client browser use when the chart is rendered.
        /// </summary>
        [NotifyParentProperty(true)]
        [Bindable(true)]
        [Description("Loads and initializes the JavaScript clientside API for use in client browser"),
        Category("Behavior")]
        [DefaultValue(false)]
        internal bool ClientsideApiEnabled
        {
            get
            {
                return m_EnableClientsideApi;
            }
            set
            {
                this.m_EnableClientsideApi = value;
            }
        }

		/// <summary>
		/// Returns true if the current Request is a callback for this chart instance, otherwise false.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsCallback
		{
			get
			{
				if (DesignMode)
					return false;

				return (Context.Request.QueryString[GetSaneClientSideID() + "_Callback"] != null);
			}
		}

        private bool m_EnableViewAngleChooser = false;
        /// <summary>
        ///		When set to true, loads and initializes the JavaScript clientside Chart
        ///     method that renders HTML for View Angle Chooser control.
        /// </summary>
        [NotifyParentProperty(true)]
        [Bindable(true)]
        [Description("Loads the JavaScript clientside Chart method that renders the View Angle Chooser control"),
        Category("Behavior")]
        [DefaultValue(false)]
        internal bool ViewAngleChooserEnabled
        {
            get
            {
                return m_EnableViewAngleChooser;
            }
            set
            {
                this.m_EnableViewAngleChooser = value;
            }
        }

        //private bool m_EnableSeriesHighlighting = false;
        ///// <summary>
        /////		When set to true, the rendering engine will create an image for each series in which
        /////     that series will be highlighted relative to other series.  The newly created images will
        /////     be tied into the client-side API for easy access, hence ClientsideApiEnabled has to be set
        /////     to true for this functionality to be usefull.
        ///// </summary>
        //[NotifyParentProperty(true)]
        //[Bindable(true)]
        //[Description("Creates multiple images with highlighted series and ties it into the client-side API"),
        //Category("Behavior")]
        //[DefaultValue(false)]
        //internal bool SeriesHighlightingEnabled
        //{
        //    get
        //    {
        //        return m_EnableSeriesHighlighting;
        //    }
        //    set
        //    {
        //        this.m_EnableSeriesHighlighting = value;
        //    }
        //}

        internal ClientsideRefreshMethod RefreshMethod = ClientsideRefreshMethod.Callback;

        private bool m_IsScrollControl = false;

        [NotifyParentProperty(true)]
        [Bindable(true)]
        [Description("Designates this Chart to be a zooming and scrolling controll for another Chart"),
        Category("Behavior")]
        [DefaultValue(false)]
        internal bool IsScrollControl
        {
            get
            {
                if (m_IsScrollControl)
                    this.View.Kind = ProjectionKind.TwoDimensional;
                //TODO: throw some warning (dialog) in design time to warn user

                return m_IsScrollControl;
            }
            set
            {
                this.m_IsScrollControl = value;
                if (value)
                    this.m_EnableClientsideApi = true;
            }
        }

        private bool m_ClientsideCustomizedImageCachingEnabled = false;
        /// <summary>
        ///		When set to true, chart renderings produced by clientside customizations
        ///     are cached.  The expiry and rerender rules are the same as for the uncustomized
        ///     chart image.  If a custom image is required for the same customizations which
        ///     has a cached image already, the cached image is used.
        /// </summary>
        [NotifyParentProperty(true)]
        [Bindable(true)]
        [Description("Caches custom images modified by the clientside API."),
        Category("Behavior")]
        [DefaultValue(false)]
        internal bool ClientsideCustomizedImageCachingEnabled
        {
            get
            {
                return m_ClientsideCustomizedImageCachingEnabled;
            }
            set
            {
                this.m_ClientsideCustomizedImageCachingEnabled = value;
            }
        }

        internal ClientTemplateCollection m_clientTemplates = new ClientTemplateCollection();
        /// <summary>
        /// Collection of client-templates that may be used by this control.
        /// </summary>
        [Browsable(false)]
        [Description("Collection of client-templates that may be used by this control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ClientTemplateCollection ClientTemplates
        {
            get
            {
                return (ClientTemplateCollection)m_clientTemplates;
            }
        }

        private ChartClientEvents m_clientEvents = null;
        /// <summary>
        /// Configures client evennt handlers that fire on certain client-side actions of Chart.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ChartClientEvents ClientEvents
        {
            get
            {
                if (m_clientEvents == null)
                {
                    m_clientEvents = new ChartClientEvents();
                }
                return m_clientEvents;
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
                if (Frame != null)
                    return Frame.TextShade;
                else
                    return false;
            }
            set
            {
                if (Frame != null)
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
        public Color BackGradientEndingColor 
        {
            get { return m_chart.BackGradientEndingColor; }
            set { m_chart.BackGradientEndingColor = value; }
        }

        /// <summary>
        ///		Gets or sets the background gradient kind.
        /// </summary>
        /// <remarks>
        ///   If this property is <see cref="GradientKind.None"/>, <see cref="Windows.BackColor"/> is used 
        ///   to paint the background, otherwise <see cref="GradientKind.None"/> and
        ///   <see cref="Chart.BackGradientEndingColor"/> are used for gradient. If any of these
        ///   two properties is set to <see cref="Color.Transparent"/>, the corresponding special color from the
        ///   <see cref="Palette"/> is used.
        /// </remarks>
        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.All)]
        [Description("Set the background gradient type."),
        Category("Appearance")]
        [DefaultValue(typeof(GradientKind), "Vertical")]
        public GradientKind BackGradientKind { get { return m_chart.BackGradientKind; } set { m_chart.BackGradientKind = value; } }


        #endregion

        #region --- Styles Properties ---

        /// <summary>
        /// Gets the collection of label styles contained within the chart.
        /// </summary>
        [
        Description("Collection of predefined and user defined label styles "),
        Category("ChartBase Styles"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)
        ]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public LabelStyleCollection LabelStyles  { get { return m_chart.LabelStyles; } }

        /// <summary>
        /// Gets the collection of text styles contained within the chart.
        /// </summary>
        [
        Description("Collection of predefined and user defined text styles "),
        Category("ChartBase Styles"),
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
        Category("ChartBase Styles")
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
        Category("ChartBase Styles")
        ]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public SeriesStyleCollection SeriesStyles { get { return m_chart.SeriesStylesX; } }

        /// <summary>
        /// Gets the collection of color palettes contained within the chart.
        /// </summary>
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Description("Collection of palettes "),
        Category("ChartBase Styles")
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
        Category("ChartBase Styles")
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
        Category("ChartBase Styles")]
        [NotifyParentProperty(true)]
        [DefaultValue("Default")]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.Attribute)]
#endif
        public string SelectedPaletteName 
        {
            get { return m_chart.SelectedPalette; } 
            set { m_chart.SelectedPalette = value; } 
        }

        /// <summary>
        /// Gets or sets the palette used in the chart.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PaletteKind SelectedPaletteKind 
        {
            get { return Palette.KindOf(SelectedPaletteName); }
            set { SelectedPaletteName = Palette.NameOf(value); } 
        }

        /// <summary>
        /// Gets or sets the lighting setup used in the chart.
        /// </summary>
        [TypeConverter(typeof(SelectedLightingSetupConverter))]
        //[EditorAttribute(typeof(SelectedPaletteEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Selected lighting setup name"),
        Category("ChartBase Styles")]
        [NotifyParentProperty(true)]
        [DefaultValue("Default")]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.Attribute)]
#endif
        public string SelectedLightingSetupName 
        {
            get { return m_chart.SelectedLightingSetup; } 
            set { m_chart.SelectedLightingSetup = value; } 
        }

        /// <summary>
        /// Gets the collection of line styles contained within the chart.
        /// </summary>
        //[Editor(typeof(LineStyleCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Collection of predefined and user defined line styles "),
        Category("ChartBase Styles")]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public LineStyleCollection LineStyles { get { return m_chart.LineStylesX; } }

        /// <summary>
        /// Gets the collection of 2D line styles contained within the chart.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Collection of predefined and user defined 2D line styles "),
        Category("ChartBase Styles")]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public LineStyle2DCollection LineStyles2D { get { return m_chart.LineStyles2DX; } }

        /// <summary>
        /// Gets the collection of gradient styles contained within the chart.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Collection of predefined and user defined data point presentation styles "),
        Category("ChartBase Styles")]
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
        Category("ChartBase Styles")
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
        Category("ChartBase Styles")
        ]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public TickMarkStyleCollection TickMarkStyles { get { return m_chart.TickMarkStyles; } }

        #endregion

        #region --- ChartBase Contents Properties ---
        /// <summary>
        /// Gets or sets automatic margins resizing to fit axis labels.
        /// </summary>
        [Description("Resize margins to make room for axis labels")]
        [Category("ChartBase Contents")]
#if __BUILDING_CRI__ || __BUILDING_CRI_DESIGNER__
		[DefaultValue(true)]
#else
        [DefaultValue(false)]
#endif
        public bool ResizeMarginsToFitLabels 
        {
            get { return m_chart.ResizeMarginsToFitLabels; } 
            set { m_chart.ResizeMarginsToFitLabels = value; } 
        }

        /// <summary>
        /// Gets or sets safety margins percentage.
        /// </summary>
        /// <remarks>
        /// This value defines safety margins between axis labels and border of the image in percentage of the
        ///  margin size. For example, if the margins are 10 and safety margins are 20, the chart will 
        ///  be sized so that labels don't cover 2 percent of the chart area.
        /// </remarks>
        [Description("Safety Margins for Axis Labels")]
        [Category("ChartBase Contents")]
        [DefaultValue(10)]
        public double SafetyMarginsPercentage 
        {
            get { return m_chart.SafetyMarginsPercentage; } 
            set { m_chart.SafetyMarginsPercentage = value; } 
        }

        /// <summary>
        /// Gets or sets the <see cref="Legend"/> object of the chart.
        /// </summary>
        [Description("Legend"),
        Category("ChartBase Contents")]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [NotifyParentProperty(true)]
        public Legend Legend 
        {
            get { return m_chart.Legend; }
            set { m_chart.Legend = value; } 
        }
        /// <summary>
        /// Gets or sets the collection of secondary legends.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public LegendCollection SecondaryLegends 
        {
            get { return m_chart.SecondaryLegends; } 
            set { m_chart.SecondaryLegends = value; } 
        }

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
        ///     <see cref="Chart.DefineValue"/>,
        ///     <see cref="Chart.DefineAsExpression"/> and
        ///     <see cref="Chart.DefineValuePath"/>.
        ///   </para>
        ///   <para>
        ///     For more on input variables see topic "Data Binding" in "Basic Concepts"
        ///     and "Advanced Data Binding" in "Advanced Concepts".
        ///   </para>
        /// </remarks>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Description("Input variables"),
        Category("ChartBase Contents")]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public InputVariableCollection InputVariables { get { return m_chart.InputVariables; } }

        /// <summary>
        /// Gets the collection of series contained in the first level of the series tree.
        /// </summary>
        [Description("Series collection"),
     Category("ChartBase Contents"),
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
        [Description("Number of data points simulated at design time"),
     Category("ChartBase Contents")]
        public int NumberOfSimulatedDataPoints 
        {
            get { return m_chart.NumberOfSimulatedDataPoints; } 
            set { m_chart.NumberOfSimulatedDataPoints = value; } 
        }

        /// <summary>
        /// Default style of the chart.
        /// </summary>
        [Description("ChartBase style"),
     Category("ChartBase Contents"),
     Editor(typeof(SeriesStyleEditor), typeof(System.Drawing.Design.UITypeEditor)),
     RefreshProperties(RefreshProperties.All)]
        [NotifyParentProperty(true)]
        [DefaultValue("Cylinder")]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.Attribute)]
#endif
        public string MainStyle 
        {
            get { return m_chart.Series.StyleName; } 
            set { m_chart.Series.StyleName = value; } 
        }

        /// <summary>
        /// The default chart kind.
        /// </summary>
        /// <remarks>
        /// This property is used to get or set the main style when the style is one of the predefined series styles.
        /// Setting the value to <see cref="SeriesStyleKind.Custom"/> is wrong, unless there is a user created style named "Custom".
        /// For all of the user created styles, this property gets the value <see cref="SeriesStyleKind.Custom"/>.
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
        Category("ChartBase Contents")]
        [DefaultValue(CompositionKind.Sections)]
        public CompositionKind CompositionKind 
        {
            get { return m_chart.CompositionKind; } 
            set { m_chart.CompositionKind = value; } 
        }

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
        [Description("ChartBase coordinate system"),
        Category("ChartBase Contents")]
        public CoordinateSystem CoordinateSystem 
        {
            get { return m_chart.Series.CoordSystem; }
            set { m_chart.Series.CoordSystem = value; } 
        }

        /// <summary>
        /// Gets or sets the value indicating whether the y-axis of the main coordinate system is logarithmic.
        /// </summary>
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.Attribute)]
#endif
        [Description("Is the chart logarithmic"),
     Category("ChartBase Contents"),
     DefaultValue(false)]
        public bool IsLogarithmic 
        {
            get { return m_chart.Series.IsLogarithmic; } 
            set { m_chart.Series.IsLogarithmic = value; } 
        }

        /// <summary>
        /// Gets or sets the logarithm base of this chart.
        /// </summary>
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.Attribute)]
#endif
        [Description("The logarithm base"),
     Category("ChartBase Contents"),
     DefaultValue(10)]
        public int LogBase 
        {
            get { return m_chart.Series.LogBase; } 
            set { m_chart.Series.LogBase = value; } 
        }

        /// <summary>
        /// Gets the collection of lights contained within the chart.
        /// </summary>

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Category("ChartBase Contents")]
        public LightCollection Lights { get { return m_chart.Lights; } }

        /// <summary>
        /// The reference value.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     This property can be edited in design time. In the code it is equivalent to 
        ///     the <see cref="ReferenceValue"/> property.
        ///   </para>
        ///   <para>
        ///     This is a base value along the y-axis of the charts. All bars, as well as areas
        ///     are rendered with this base value. TThe reference value has to be the same type 
        ///     as the type of the y-values in a chart.
        ///   </para>
        ///   <para>
        ///     Often the y-values are numeric and default reference value is 0. If values
        ///     are from a narrow range, say between 200 and 210, you may choose a reference value of
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
        [Category("ChartBase Contents")]
        public GenericType Reference 
        {
            get { return m_chart.Series.Reference; } 
            set { m_chart.Series.Reference = value; } 
        }
 

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
        Category("ChartBase Contents")]
        [DefaultValue(0.2)]
        public double RenderingPrecision 
        {
            get { return m_chart.RenderingPrecision; } 
            set { m_chart.RenderingPrecision = value; }
        }

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
        Category("ChartBase Contents")]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [NotifyParentProperty(true)]
        [Bindable(true)]
        public Mapping View
        {
            get { return m_chart.Mapping; }
            set
            {
                m_chart.Mapping = value;
                m_chart.Mapping.Chart = this.ChartBase;
                m_chart.Mapping.ViewDirectionChanged += new EventHandler(this.HandleViewDirectionChanged);
            }
        }

        /// <summary>
        /// Collection of chart titles.
        /// </summary>
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Category("ChartBase Contents")]
        public ChartTitleCollection Titles { get { return m_chart.Titles; } }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        internal CompositeSeries RootSeries 
        {
            get { return m_chart.Series; } 
            set { m_chart.Series = value; } 
        }


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
        public string SelectClause 
        {
            get { return m_chart.DataProvider.SelectClause; } 
            set { m_chart.DataProvider.SelectClause = value; } 
        }

        /// <summary>
        /// Sets or gets the order clause for filtering rows in data source. 
        /// The property contains expression that follows words "ORDER BY" in SQL "ORDER BY" clause, for example "sales, expences DESC".
        /// Applies only if data source is <see cref="DataTable"/>.
        /// </summary>
        [Description("Sets or gets the order clause for filtering rows in data source.")]
        [Category("Data")]
        [DefaultValue(null)]
        public string OrderClause 
        {
            get { return m_chart.DataProvider.OrderClause; } 
            set { m_chart.DataProvider.OrderClause = value; } 
        }

        /// <summary>
        /// Sets or gets the number rows selected from data source. 
        /// The property contains expression that follows the "TOP" key word in SQL.
        /// Applies only if data source is <see cref="DataTable"/>.
        /// </summary>
        [Description("Sets or gets the number rows selected from data source.")]
        [Category("Data")]
        [DefaultValue(0)]
        public int TopNumber 
        {
            get { return m_chart.DataProvider.TopNumber; } 
            set { m_chart.DataProvider.TopNumber = value; }
        }

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
            m_chart.DefineValue(name, obj);
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
            DefineValue(name, obj);
            m_chart.DefineDimension(name, dimension);
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
            m_chart.DefineAsExpression(name, expression);
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
            DefineAsExpression(name, expression);
            m_chart.DefineDimension(name, dimension);
        }

        /// <summary>
        ///		Defines value path of an input variable within data source object.
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="valuePath">String representing the value path.</param>
        /// <remarks>
        ///   <para>
        ///     This method is used when the <see cref="Chart.DataSource"/> object is used
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
            m_chart.DefineValuePath(name, valuePath);
        }

        /// <summary>
        /// Defines value path of an input variable within data source object and variable dimension
        /// </summary>
        /// <param name="name">Variable name.</param>
        /// <param name="valuePath">String representing the value path.</param>
        /// <param name="dimension"> Dimension of the variable. <see cref="DataDimension"/>.</param>
        /// <remarks>
        ///   <para>
        ///     This method is used when the <see cref="Chart.DataSource"/> object is used
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
            DefineValuePath(name, valuePath);
            m_chart.DefineDimension(name, dimension);
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

        #region --- ChartBase Template Handling ---

        /// <summary>
        ///		Loads chart from an XML template.
        /// </summary>
        /// <param name="templateFileName">Name of the input XML file</param>
        /// <remarks>
        ///   <para>
        ///     Loads chart settings from an XML file. The input file might be created by 
        ///     <see cref="Chart.StoreTemplate"/> functions and maybe hand-edited afterwards.
        ///     After the chart is loaded from a template, data definition functions are needed
        ///     to set real data and <see cref="Chart.DataBind"/> is supposed to be called.
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
            ChartXmlSerializer bld = new ChartXmlSerializer();
            bld.ReadObject(this, templateFileName);
        }

        /// <summary>
        ///		Loads chart from an XML template.
        /// </summary>
        /// <param name="templateStream">The input stream object</param>
        /// <remarks>
        ///   <para>
        ///     Loads chart settings from an XML file. The input file might be created by 
        ///     <see cref="Chart.StoreTemplate"/> functions and maybe hand-edited afterwards.
        ///     After the chart is loaded from a template, data definition functions are needed
        ///     to set real data and <see cref="Chart.DataBind"/> is supposed to be called.
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
            ChartXmlSerializer bld = new ChartXmlSerializer();
            bld.ReadObject(this, templateStream);
        }

        /// <summary>
        ///		Stores chart to an XML template.
        /// </summary>
        /// <param name="templateFileName">Name of the output XML file</param>
        /// <remarks>
        ///   <para>
        ///     Stores chart settings to an XML file. The output file might be used in
        ///     <see cref="Chart.LoadTemplate"/> function to restore chart settings.
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
            ChartXmlSerializer bld = new ChartXmlSerializer(this, "ChartBase");
            bld.Write(templateFileName);
        }

        /// <summary>
        ///		Stores chart to an XML template.
        /// </summary>
        /// <param name="templateFileName">The output stream</param>
        /// <remarks>
        ///   <para>
        ///     Stores chart settings to a stream. The output might be used in
        ///     <see cref="Chart.LoadTemplate"/> function to restore chart settings.
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
            ChartXmlSerializer bld = new ChartXmlSerializer(this, "ChartBase");
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
        public object ReferenceValue 
        {
            get { return m_chart.Series.ReferenceValue; } 
            set { m_chart.Series.ReferenceValue = value; } 
        }

		/// <summary>
		/// A property that indicates whether the reference value should be adjusted to the y-value range.
		/// </summary>
		[Description("Should reference value be adjusted to the y-value range")]
		[Category("ChartBase Contents")]
		[DefaultValue(false)]
		public bool AdjustReferenceValue
		{
            get { return m_chart.Series.AdjustReferenceValue; } 
            set { m_chart.Series.AdjustReferenceValue = value; } 
		}
		
#if SAFE_CODE_ONLY
		[Category("Contents")]
		[Description("Type of geometric engine")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public GeometricEngineKind GeometricEngineKind 
        {
            get { return m_chart.GeometricEngineKind; } 
            set { m_chart.GeometricEngineKind = value; } 
        }
#else
		[Category("ChartBase Contents")]
		[Description("Type of geometric engine")]
		public GeometricEngineKind GeometricEngineKind 
        {
            get { return m_chart.GeometricEngineKind; } 
            set { m_chart.GeometricEngineKind = value; }
        }
#endif
        #endregion (COMMON)

        /// <summary>
        /// Gets the collection of text box styles contained within the chart.
        /// </summary>
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Description("Collection of predefined and user defined data text box styles "),
        Category("ChartBase Styles")
        ]
#if __BuildingWebChart__
        [System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
        public TextBoxStyleCollection TextBoxStyles { get { return m_chart.TextBoxStyles; } }

        #region --- Free Annotations ---
        /// <summary>
        /// Creates a free annotation text box.
        /// </summary>
        /// <param name="text">Annotation text.</param>
        /// <param name="styleName">Name of the text box style.</param>
        /// <param name="anchorPoint">Anchor point.</param>
        /// <returns>
        /// Resulting text box object.
        /// </returns>
        public ChartTextBox CreateAnnotation(string text, string styleName, TextAnchor anchorPoint)
        {
            return m_chart.CreateTextBox(styleName, text, anchorPoint);
        }

        /// <summary>
        /// Creates a free annotation text box.
        /// </summary>
        /// <param name="text">Annotation text.</param>
        /// <param name="styleKind">The enum value selecting a predefined text style.</param>
        /// <param name="anchorPoint">Anchor point.</param>
        /// <returns>
        /// Resulting text box object.
        /// </returns>
        public ChartTextBox CreateAnnotation(string text, TextBoxStyleKind styleKind, TextAnchor anchorPoint)
        {
            return m_chart.CreateTextBox(TextBoxStyle.NameOf(styleKind), text, anchorPoint);
        }

        #endregion

        #region --- Missing data point handling ---
        /// <summary>
        /// The missing points style name to be used with this <see cref="SeriesBase"/> object.
        /// </summary>
        [NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
#if !__BUILDING_CRI__
        [Editor(typeof(SeriesStyleEditor), typeof(System.Drawing.Design.UITypeEditor))]
#endif
        [DefaultValue("")]
        [Category("Missing Points")]
        [SRDescription("SeriesBaseMissingPointStyleNameDescr")]
        public string MissingPointsStyleName
        {
            get { return m_chart.Series.MissingPointsStyleName; }
            set { m_chart.Series.MissingPointsStyleName = value; }
        }

		
		/// <summary>
		/// The missing points style kind.
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

        /// <summary>
        /// The missing point handler kind.
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

        /// <summary>
        /// Sets the custom missing point handler.
        /// </summary>
        /// <param name="mph"></param>
        public void SetCustomMissingPointHandler(MissingPointHandler mph)
        {
            CompositeSeries series = m_chart.Series;
            series.SetCustomMissingPointHandler(mph);
        }
        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether area maps will be rendered.
        /// </summary>
        [SRCategory("CatBehavior")]
        [DefaultValue(false)]
        [Description("Indicates whether area maps will be rendered.")]
        public bool RenderAreaMap
        {
            get
            {
                object obj = this.ViewState["RenderAreaMap"];
                return ((obj != null) && ((bool)obj));
            }
            set
            {
                this.ViewState["RenderAreaMap"] = value;
                m_chart.ObjectTrackingEnabled = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the chart image will be rendered directly from the page.
        /// </summary>
        [SRCategory("CatBehavior")]
        [DefaultValue(false)]
        [Description("Indicates whether the chart image will be rendered directly from the page.")]
        public bool DirectImage
        {
            get
            {
                object directImage = this.ViewState["DirectImage"];
                return ((directImage != null) && ((bool)directImage));
            }
            set
            {
                this.ViewState["DirectImage"] = value;
            }
        }

        /// <summary>
        /// Method used to render raw data only in form of an XML or CSV file.
        /// </summary>
        private void RenderDataOnly()
        {
            String dataFormat = Context.Request.Form[m_returnDataFormatInputFiledName];
            if ("xml" == dataFormat.ToLower())
            {
                Context.Response.Clear();
                Context.Response.ContentType = "text/xml";
                Context.Response.Write("<?xml version=\"1.0\"?>\n");
                Context.Response.Write("<Data>\n");
                foreach (SeriesBase s in this.Series)
                {
                    Context.Response.Write(nodeXmlText(s));
                }
                Context.Response.Write("</Data>");
            }
            else if ("csv" == dataFormat.ToLower())
            {
                Context.Response.Clear();
                Context.Response.ContentType = "text/csv";

                //write the header record first
                Context.Response.Write("Series");
                Series simpleSeries = new Series();
                //find a simple series to write the X parameters first
                foreach (SeriesBase s in this.Series)
                {
                    if (s is Series)
                    {
                        simpleSeries = (Series)s;
                        break;
                    }
                }
                foreach (DataPoint dp in simpleSeries.DataPoints)
                {
                    Context.Response.Write(',');
                    Context.Response.Write(dp.Parameters["x"]);
                }
                Context.Response.Write('\n');

                //then write all the series data
                foreach (SeriesBase s in this.Series)
                {
                    Context.Response.Write(nodeCsvText(s));
                }
            }
            else
            {
                //TODO: Report an error of an unsupported data format.
            }
            Context.Response.End();
        }

        //recursive method for generating the XML representation of a given series
        private String nodeXmlText(SeriesBase s)
        {
            StringBuilder nodeText = new StringBuilder();
            if (s is CompositeSeries)
            {
                CompositeSeries cs = (CompositeSeries)s;
                foreach (Series currentSeries in cs.SubSeries)
                {
                    nodeText.Append('\n');
                    nodeText.Append(nodeXmlText(currentSeries));
                }
            }
            else if (s is Series)
            {
                Series series = (Series)s;
                nodeText.Append("<Series name=\"");
                nodeText.Append(series.Name);
                nodeText.Append("\">\n");
                foreach (DataPoint dp in series.DataPoints)
                {
                    if (dp.Parameters["y"] != null)
                    {
                        nodeText.Append("\t<DataPoint value=\"");
                        nodeText.Append(dp.Parameters["y"]);
                        nodeText.Append("\" x=\"");
                        nodeText.Append(dp.Parameters["x"].ToString());
                        nodeText.Append("\" />\n");
                    }
                    else if (dp.Parameters["from"] != null)
                    {
                        nodeText.Append("\t<DataPoint from=\"");
                        nodeText.Append(dp.Parameters["from"]);
                        nodeText.Append("\" to=\"");
                        nodeText.Append(dp.Parameters["to"]);
                        nodeText.Append("\" x=\"");
                        nodeText.Append(dp.Parameters["x"].ToString());
                        nodeText.Append("\" />\n");
                    }
                    else if (dp.Parameters["open"] != null)
                    {
                        nodeText.Append("\t<DataPoint open=\"");
                        nodeText.Append(dp.Parameters["open"]);
                        nodeText.Append("\" close=\"");
                        nodeText.Append(dp.Parameters["close"]);
                        nodeText.Append("\" high=\"");
                        nodeText.Append(dp.Parameters["high"]);
                        nodeText.Append("\" low=\"");
                        nodeText.Append(dp.Parameters["low"]);
                        nodeText.Append("\" x=\"");
                        nodeText.Append(dp.Parameters["x"].ToString());
                        nodeText.Append("\" />\n");
                    }
                    else
                        throw new Exception("Implementation: Could not get value(s) of data point.");
                }
                nodeText.Append("</Series>\n");
            }
            return nodeText.ToString();
        }

        //recursive method for generating the CSV representation of a given series
        private String nodeCsvText(SeriesBase s)
        {
            StringBuilder nodeText = new StringBuilder();
            if (s is CompositeSeries)
            {
                CompositeSeries cs = (CompositeSeries)s;
                foreach (Series currentSeries in cs.SubSeries)
                {
                    nodeText.Append(nodeCsvText(currentSeries));
                }
            }
            else if (s is Series)
            {
                Series series = (Series)s;
                nodeText.Append(series.Name);
                foreach (DataPoint dp in series.DataPoints)
                {
                    if (dp.Parameters["y"] != null)
                    {
                        nodeText.Append(",");
                        nodeText.Append(dp.Parameters["y"]);
                    }
                    else if (dp.Parameters["from"] != null)
                    {
                        nodeText.Append(",");
                        nodeText.Append(dp.Parameters["from"]);
                        nodeText.Append(" - ");
                        nodeText.Append(dp.Parameters["to"]);
                    }
                    else if (dp.Parameters["open"] != null)
                    {
                        nodeText.Append(",o:");
                        nodeText.Append(dp.Parameters["open"]);
                        nodeText.Append(" c:");
                        nodeText.Append(dp.Parameters["close"]);
                        nodeText.Append(" h:");
                        nodeText.Append(dp.Parameters["high"]);
                        nodeText.Append(" l:");
                        nodeText.Append(dp.Parameters["low"]);
                    }
                    else
                        throw new Exception("Implementation: Could not get value(s) of data point.");
                }
                nodeText.Append("\n");
            }
            return nodeText.ToString();
        }

        //recursive method for generating the HTML table representation of a given series
        private String nodeHtmlTableText(SeriesBase s)
        {
            StringBuilder nodeText = new StringBuilder();
            if (s is CompositeSeries)
            {
                CompositeSeries cs = (CompositeSeries)s;
                foreach (Series currentSeries in cs.SubSeries)
                {
                    nodeText.Append('\n');
                    nodeText.Append(nodeHtmlTableText(currentSeries));
                }
            }
            else if (s is Series)
            {
                Series series = (Series)s;
                nodeText.Append("<tr> /n/t<td> ");
                nodeText.Append(series.Name);
                nodeText.Append("</>\n");
                foreach (DataPoint dp in series.DataPoints)
                {
                    if (dp.Parameters["y"] != null)
                    {
                        nodeText.Append("\t<td>");
                        nodeText.Append(dp.Parameters["y"]);
                        nodeText.Append("</td>\n");
                    }
                    else if (dp.Parameters["from"] != null)
                    {
                        nodeText.Append("\t<td>");
                        nodeText.Append(dp.Parameters["from"]);
                        nodeText.Append(" - ");
                        nodeText.Append(dp.Parameters["to"]);
                        nodeText.Append("</td>\n");
                    }
                    else if (dp.Parameters["open"] != null)
                    {
                        nodeText.Append("\t<td>o:");
                        nodeText.Append(dp.Parameters["open"]);
                        nodeText.Append(" c:");
                        nodeText.Append(dp.Parameters["close"]);
                        nodeText.Append(" h:");
                        nodeText.Append(dp.Parameters["high"]);
                        nodeText.Append(" l");
                        nodeText.Append(dp.Parameters["low"]);
                        nodeText.Append("</td>\n");
                    }
                    else
                        throw new Exception("Implementation: Could not get value(s) of data point.");
                }
                nodeText.Append("</tr>\n");
            }
            return nodeText.ToString();
        }

        //private bool m_disposeLater = false;

        //internal void CreateHighlightedImages()
        //{
        //    foreach (Series highlited in this.RootSeries.SimpleSubseriesList)
        //    {
        //        highlited.Transparency = 5;
        //        foreach (Series notHighlited in this.RootSeries.SimpleSubseriesList)
        //        {
        //            if (notHighlited != highlited)
        //                notHighlited.Transparency = 60;
        //        }

        //        //reset the dynamic image so that a new one is created for each highlighted image
        //        //this is used so each image will have its own callback entry for deleteion
        //        this.m_di = null;

        //        //invalidate the current chart so that a new image is rendered
        //        this.m_chart.Invalidate();
        //        this.PreprocessImage(null, highlited.highlightedImageName);
        //    }

        //    // Dispose of the object that was scipped when the hightlighter thread was created
        //    if (m_disposeLater = true)
        //    {
        //        m_disposeLater = false;
        //        this.Dispose();
        //    }
        //}

        private ArrayList m_controlledCharts = null;
        internal ArrayList controlledCharts 
        {
            get 
            {
                if (m_controlledCharts == null)
                    m_controlledCharts = findControlledCharts(Page.Controls);
                
                return m_controlledCharts;
            }
        }

        //method that recursively searches a controlls collection to finda all WebCharts
        //controlled by this instance
        private ArrayList findControlledCharts(ControlCollection controls)
        {
            ArrayList list = new ArrayList();
            foreach (Control c in controls)
            {
                if ((c is Chart) && (((Chart)c).MyScrollControl == this))
                    list.Add(c);
                else if (c.Controls.Count > 0)
                    list.AddRange(findControlledCharts(c.Controls));
            }
            return list;
        }
        
        private string createCallbackResponse(String imageUrl, String minCoord, String maxCoord)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version=\"1.0\" encoding=\"" + Context.Response.Charset + "\"?>\n");
            sb.Append("<CallbackResponse><![CDATA[ var callbackResult={");
            sb.Append("'imgUrl':'" + imageUrl + "'");
            sb.Append(",'height':'" + this.Height + "'");
            sb.Append(",'width':'" + this.Width + "'");
            sb.Append(",'startX':" + minCoord);
            sb.Append(",'endX':" + maxCoord);
            sb.Append(",'series': [1" + seriesClientsideText(this.RootSeries) + "]");
            if (m_RangeChangedByClientside)
                sb.Append(",'rangeChanged':'" + 1 + "'");
			string areaMapsHtml = getAreaMapInnerHtml(true);
			if (areaMapsHtml != null && areaMapsHtml != "")
			{
				sb.Append(",'areaMapHtml':'" + areaMapsHtml + "'");
				Debug.WriteLine("---------- AREA MAP --------------");
				Debug.WriteLine(areaMapsHtml);
			}

            sb.Append("} ]]></CallbackResponse>");

            return sb.ToString();
        }


//#if FWAJAX || FW35
//        /// <summary>
//        /// FW 3.5 and AJAX scripts loaded with the Script Manager have to be done at this stage
//        /// </summary>
//        protected override void OnLoad(EventArgs e)
//        {
//            base.OnLoad(e);
//            RegisterAtlasScripts();
//        }
//#endif

#if !__COMPILING_FOR_2_0_AND_ABOVE__
        protected internal bool DesignMode { get { return (Context == null); } }
#endif


#if FWAJAX || FW35
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
#endif

        private bool IsClientScriptBlockRegistered(string key) 
        {   
            return IsClientScriptBlockRegistered(this.GetType(), key);
        }
        
        private bool IsClientScriptBlockRegistered(Type type, string key) 
        {
#if __COMPILING_FOR_2_0_AND_ABOVE__
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
#if __COMPILING_FOR_2_0_AND_ABOVE__
            Page.ClientScript.RegisterClientScriptBlock(type, key, script);
#else
            Page.RegisterClientScriptBlock(key, script);            
#endif
        }

        protected void WriteStartupScript(StringBuilder output, string sScript)
        {
#if FWAJAX || FW35
            if (this.IsInUpdatePanel())
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), sScript, false);
                return;
            }
#endif
            output.Append(sScript);
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

#if FWAJAX || FW35
            RegisterAtlasScripts();
#endif
            RegisterClientScripts();

			//raise the callback event
			if (IsCallback)
				RaiseCallbackEvent();
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

#if __COMPILING_FOR_2_0_AND_ABOVE__
            if (DesignMode && DesignSavePath == string.Empty)
            {
                base.Render(output);
                return;
            }
#endif
			//store the HTML to be output and if caching enabled, stored for later use
			StringBuilder clientSideHtml = new StringBuilder();
#if !DEBUG
			if (licence == LicenseType.Expired)
			{
				RenderUnlicensedMessage(output);
				return;
			}
#endif

            //set the view state variables needed in the DynamicImage
            DynamicImage.ImageFile = Convert.ToString(ViewState["ImageFile"]);
            DynamicImage.StorageKey = Convert.ToString(ViewState["StorageKey"]);
            DynamicImage.URLGUID = Convert.ToString(ViewState["urlGuid"]);

            if (DesignSavePath == String.Empty && DirectImage)
            {
                string typeresponse = "";

                Page.Response.Clear();
                typeresponse = WebChartImageType == ChartImageType.Gif ? "gif" : WebChartImageType == ChartImageType.Jpeg ? "jpeg" : "png";

                Page.Response.ContentType = "image/" + typeresponse;
                MemoryStream stream1 = new MemoryStream();

                PreprocessImage(stream1);
                byte[] bbb = stream1.GetBuffer();
                this.Page.Response.BinaryWrite(bbb);
                Page.Response.End();

                //this.Page.Response.OutputStream.Write(img.ImageBytes, 0, img.ImageBytes.Length);
                //Page.Response.BinaryWrite(img.ImageBytes);

                ViewState["StorageKey"] = DynamicImage.StorageKey;
                ViewState["urlGuid"] = DynamicImage.URLGUID;
                return;
            }

            PreprocessImage();

            if (this.UseCached)
            {
				if (this.ClientsideApiEnabled || this.RenderAreaMap)
				{
					if (Context != null && Context.Request.QueryString[GetSaneClientSideID() + "_Callback"] != null)
					{
						Context.Response.Clear();
						Context.Response.ContentType = "text/xml";
						Context.Response.Write(DynamicImage.ReadClientsideHtml(this, true));
						Context.Response.End();
					}
					else
					{
						output.Write(DynamicImage.ReadClientsideHtml(this, false));
					}
				}
				else //just a regular image needs to be output with standard HTML
				{
					clientSideHtml.Append("<div style=\"height:" + this.Height + ";width:" + this.Width + ";\">");
					clientSideHtml.Append("<img id='" + GetSaneClientSideID() + "' src='" + this.DynamicImage.GetImageUrl(this) + "' border='0' style=\"height:" + this.Height + ";width:" + this.Width + ";\" />");
					clientSideHtml.Append("</div>\n");

					output.Write(clientSideHtml.ToString());
				}

                return;
            }


            if (DesignMode)
            {
                //String designHtml = "<img id='" + GetSaneClientSideID() + "' src='" + this.DynamicImage.GetImageUrl(this) + "' border='0' style=\"height:" + this.Height + ";width:" + this.Width + ";\"" + " />";
                //output.WriteLine(designHtml);
                base.Render(output);
                return;
            }

            //disable the client-side API for MuliArea and MultiSystem charts
            if (this.RootSeries.CompositionKind == CompositionKind.MultiArea
                || this.RootSeries.CompositionKind == CompositionKind.MultiSystem)
            {
                this.ClientsideApiEnabled = false;
            }

            String minCoord = String.Empty, maxCoord = String.Empty, dimension = "0";
            
            //Dimension rules
            // 0 - no dimension (for multi-area and multi-system charts)
            // 1 - DateTime
            // 2 - Numeric
            // 3 - String

            //Replace latest client-side IDs in stored chart properties:
            DetermineClientsideIDs();
            this.m_defaultChartProperties = m_defaultChartProperties.Replace("##m_customizationsInputFieldName##", m_customizationsInputFieldName);
            this.m_defaultChartProperties = m_defaultChartProperties.Replace("##m_returnDataFormatInputFiledName##", m_returnDataFormatInputFiledName);
            this.m_defaultChartProperties = m_defaultChartProperties.Replace("##SaneClientSideID##", GetSaneClientSideID());


            DataDimension xAxisDimension = this.CoordinateSystem.XAxis.Dimension;
            if (this.ClientsideApiEnabled)
            {
                if (xAxisDimension == null) // for multi-area and multi-system charts
                {
                   dimension = "0";
                   minCoord = String.Empty;
                   maxCoord = String.Empty;
                }
                else if (this.CoordinateSystem.XAxis.MinValue is DateTime)
                {
                    //needed in order to sync with JavaScripts representation of DateTime objects
                    DateTime zero = new DateTime(1970, 1, 1);
                    minCoord = Convert.ToString((((DateTime)this.CoordinateSystem.XAxis.MinValue) - zero).TotalMilliseconds);
                    maxCoord = Convert.ToString((((DateTime)this.CoordinateSystem.XAxis.MaxValue) - zero).TotalMilliseconds);
                    dimension = "1";
                }
                else
                {
                    if (xAxisDimension is NumericDataDimension)
                    {
                        dimension = "2";
                        minCoord = CommonFunctions.GetCultureFreeString(CoordinateSystem.XAxis.MinValue);
                        maxCoord = CommonFunctions.GetCultureFreeString(this.CoordinateSystem.XAxis.MaxValue);
                    }
                    else
                    {
                        dimension = "3";
                        try
                        {
                            minCoord = "'" + EscapeForJS(this.CoordinateSystem.XAxis.MinValue.ToString()) + "'";
                        }
						catch (NullReferenceException)
                        {
                            minCoord = "''";
                        }

                        try
                        {
                            maxCoord = "'" + EscapeForJS(this.CoordinateSystem.XAxis.MaxValue.ToString()) + "'";
                        }
						catch (NullReferenceException)
                        {
                            maxCoord = "''";
                        }
                    }
                }
            }

            // Is this a callback? Handle it now
            if (Context != null && Context.Request.QueryString[GetSaneClientSideID() + "_Callback"] != null)
            {
                String imageUrl = this.DynamicImage.GetImageUrl(this);

                Context.Response.Clear();
                Context.Response.ContentType = "text/xml";
                Context.Response.Write(createCallbackResponse(imageUrl, minCoord, maxCoord));                
            }

            if (!m_IsScrollControl)
            {
                string areaMap = String.Empty;

                if (RenderAreaMap)
                    areaMap = " usemap=\"#" + MapID + "\"";

                StringBuilder sb = new StringBuilder();

                // open div container
                clientSideHtml.Append("<div style=\"height:" + this.Height + ";width:" + this.Width + ";");

                if (m_LoadingChartImagePath != null && m_LoadingChartImagePath != String.Empty)
                    clientSideHtml.Append("background-image:url(" + m_LoadingChartImagePath + ");background-repeat:no-repeat;background-position:center center;");
                    
                clientSideHtml.Append("\">");
                
                // chart image
                clientSideHtml.Append("<img id='" + GetSaneClientSideID() + "' src='" + this.DynamicImage.GetImageUrl(this) + "' border='0' style=\"height:" + this.Height + ";width:" + this.Width + ";\"" + areaMap + " />");
                
                // close div container
                clientSideHtml.Append("</div>\n");               
            }
            else
            {
                String ScrollJsId = GetSaneClientSideID();
                int selectedSizePx = m_scrollRangeEndPixel - m_scrollRangeStartPixel - 2 * ControlResizeButtonSizePx;
                int rightSizePx = (int)this.Width.Value - m_scrollRangeEndPixel;

                //create the browser specific opacity style code
                String opacityStyle;
                //CANNOT BE BROWSER SPECIFIC BECAUSE OF THE CACHING OF HTML RESPONSE
                //string agent = Context.Request.UserAgent;
                //if (Context.Request.UserAgent.IndexOf("MSIE")>= 0) //MS Internet Explorer
                opacityStyle = "filter:alpha(opacity=" + CommonFunctions.GetCultureFreeString(ScrollShadowOpacity * 100) + ");";
                //else //all other browsers
                opacityStyle += "opacity:" + CommonFunctions.GetCultureFreeString(ScrollShadowOpacity) + ";";


                StringBuilder sb = new StringBuilder();

                sb.Append("<table id=\"zoomTable_" + GetSaneClientSideID() + "\" style=\"width:" + this.Width + "; height:" + this.Height + ";"
                    + "  background-image: url(" + this.DynamicImage.GetImageUrl(this) + "); background-repeat: no-repeat;\" cellspacing=0 cellpadding=0 >\n");

                sb.Append("\t<tr>\n");
                sb.Append("\t\t<td style=\"font-size:1px; " + opacityStyle +" width:" + m_scrollRangeStartPixel + "px; height:" + this.ImageHeight + "px; background-color:" + ScrollShadowColor + ";\" id=\'" + ScrollJsId + "_leftGreyArea\'>&nbsp;</td>\n");
                sb.Append("\t\t<td style=\"font-size:1px; width:" + ControlResizeButtonSizePx + "px; height:" + this.ImageHeight + "px; cursor: w-resize;\" onmousedown=\"window." + ScrollJsId + ".rangePicker.dragStart(event,\'" + ScrollJsId + "_leftGreyArea\')\">&nbsp;</td>\n");
                sb.Append("\t\t<td style=\"font-size:1px; width:" + selectedSizePx + "px; height:" + this.ImageHeight + "px;\" id=\'" + ScrollJsId + "_selectedArea\'>&nbsp;</td>\n");
                sb.Append("\t\t<td style=\"font-size:1px; width:" + ControlResizeButtonSizePx + "px; height:" + this.ImageHeight + "px; cursor: e-resize;\" onmousedown=\"window." + ScrollJsId + ".rangePicker.dragStart(event,\'" + ScrollJsId + "_rightGreyArea\')\">&nbsp;</td>\n");
                sb.Append("\t\t<td style=\"font-size:1px; " + opacityStyle + " width:" + rightSizePx + "px; height:" + this.ImageHeight + "px; background-color:" + ScrollShadowColor + ";\" id=\'" + ScrollJsId + "_rightGreyArea\'>&nbsp;</td>\n");
                sb.Append("\t</tr>\n");

                sb.Append("\t<tr>\n");
                sb.Append("\t\t<td align=\"left\" style=\"width:" + m_scrollRangeStartPixel + "px; height:" + ScrollControllHeight + "; background-image: url(" + m_ScrollImagesDirectoryPath + "gradient_empty.png); background-repeat: repeat-x; text-align: left;\" id=\'" + ScrollJsId + "_leftGreyAreaScroll\' onclick=\"window." + ScrollJsId + ".rangePicker.moveOver(event,\'" + ScrollJsId + "_leftGreyArea\', \'" + ScrollJsId + "_rightGreyArea\', false)\"><img src=\"" + m_ScrollImagesDirectoryPath + "step_left.png\" onclick=\"window." + ScrollJsId + ".rangePicker.moveOver(event,\'" + ScrollJsId + "_leftGreyArea\', \'" + ScrollJsId + "_rightGreyArea\', true)\" /></td>\n");
                sb.Append("\t\t<td style=\"width:" + ControlResizeButtonSizePx + "px; height:" + ScrollControllHeight + "; cursor: w-resize;\" id=\'" + ScrollJsId + "_leftGreyAreaResize\' onmousedown=\"" + ScrollJsId + ".rangePicker.dragStart(event,\'" + ScrollJsId + "_leftGreyArea\')\"><img src=\"" + m_ScrollImagesDirectoryPath + "resize_left.png\" /></td>\n");
                sb.Append("\t\t<td style=\"width:" + m_scrollRangeStartPixel + "px; height:" + ScrollControllHeight + "; background-image: url(" + m_ScrollImagesDirectoryPath + "gradient_bar.png); background-repeat: repeat-x; text-align: center;\" id=\'" + ScrollJsId + "_selectedAreaScroll\' onmousedown=\"window." + ScrollJsId + ".rangePicker.dragStart(event, \'" + ScrollJsId + "_selectedArea\')\"><img src=\"" + m_ScrollImagesDirectoryPath + "bar_handle.png\" /></td>\n");
                sb.Append("\t\t<td style=\"width:" + ControlResizeButtonSizePx + "px; height:" + ScrollControllHeight + "; cursor: e-resize;\" id=\'" + ScrollJsId + "_rightGreyAreaResize\' onmousedown=\"" + ScrollJsId + ".rangePicker.dragStart(event,\'" + ScrollJsId + "_rightGreyArea\')\"><img src=\"" + m_ScrollImagesDirectoryPath + "resize_right.png\" /></td>\n");
                sb.Append("\t\t<td align=\"right\" style=\"width:" + m_scrollRangeStartPixel + "px; height:" + ScrollControllHeight + "; background-image: url(" + m_ScrollImagesDirectoryPath + "gradient_empty.png); background-repeat: repeat-x; text-align: right;\" id=\'" + ScrollJsId + "_rightGreyAreaScroll\' onclick=\"window." + ScrollJsId + ".rangePicker.moveOver(event,\'" + ScrollJsId + "_rightGreyArea\', \'" + ScrollJsId + "_leftGreyArea\', false)\"><img src=\"" + m_ScrollImagesDirectoryPath + "step_right.png\" onclick=\"window." + ScrollJsId + ".rangePicker.moveOver(event,\'" + ScrollJsId + "_rightGreyArea\', \'" + ScrollJsId + "_leftGreyArea\', true)\" /></td>\n");
                sb.Append("\t</tr>\n");

                sb.Append("</table>\n");

                sb.Append("<input type='hidden' name='" + this.m_ControlSettingsInputFieldName + "' id='" + this.m_ControlSettingsInputFieldName + "' value='' />\n");


                clientSideHtml.Append(sb.ToString());
            }

            StringBuilder startupScripts = new StringBuilder();

            if (m_EnableClientsideApi)
            {
                          
                //determine auto-refresh method
                // 0: no auto-refresh
                // 1: Callback
                // 2: Postback
                int autoRefresh = 0;
                if (RefreshMethod == ClientsideRefreshMethod.Callback) autoRefresh = 1;
                else if (RefreshMethod == ClientsideRefreshMethod.Postback) autoRefresh = 2;

                //First its the properties of the chart
                String clientEventsStr = "\"this.ClientEvents=" + Utils.ConvertClientEventsToJsObject(this.m_clientEvents) + "\"";

                clientSideHtml.Append("\n <input type='hidden' name='" + this.m_customizationsInputFieldName + "' id='" + this.m_customizationsInputFieldName + "' value=''>\n");
                clientSideHtml.Append("<input type='hidden' name='" + this.m_customizationsInputFieldName + "_properties' id='" + this.m_customizationsInputFieldName + "_properties' value=''>\n");
                clientSideHtml.Append("<input type='hidden' name='" + this.m_returnDataFormatInputFiledName + "' id='" + this.m_returnDataFormatInputFiledName + "' value=''>\n");
                startupScripts.Append("<script type=\"text/javascript\"> \n");
                startupScripts.Append("window." + GetSaneClientSideID() + " = new ComponentArt.Web.Visualization.Charting.Chart(" + autoRefresh + ", '" + GetSaneClientSideID() + "', " + clientEventsStr + ");\n");
      
                // Write postback function reference
                startupScripts.Append("window." + GetSaneClientSideID() + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");

                startupScripts.Append("window." + GetSaneClientSideID() + ".loadData([[" + m_defaultChartProperties
                                    + "," + minCoord + "," + maxCoord + "," + dimension + "]");

                //Then the customizations that the user created in their chart to persist back in the page
                //NOTE: We have to do this in case user chooses to submit customizations on postback instead of callback
                //      as in that case there is no persistance of clientside objects.

                startupScripts.Append(",[" + Utils.QuoteElements(this.m_loadedCustomizations) + "]");

                //Then the custom properties that the user submitted through our clientside API
                startupScripts.Append(",[" + Utils.QuoteElements(this.m_loadedCustomProperties) + "]");

                //then all the series in the chart.
                startupScripts.Append(seriesClientsideText(this.RootSeries));

                startupScripts.Append("]);\n");
                //output.WriteLine("setTimeout('"+ GetSaneClientSideID() + "._loadHighlightedImages();',5000);</script>");
                //if (m_EnableSeriesHighlighting)
                //    output.WriteLine(GetSaneClientSideID() + "._loadHighlightedImages();</script>");
                //else
                startupScripts.Append("</script>\n");
                
            }

            if (m_IsScrollControl) 
            {
                String dimensionDesc = String.Empty;
                if (xAxisDimension is DateTimeDataDimension || xAxisDimension is NumericDataDimension)
                {
                    dimensionDesc = "0";
                }
                else //get the coordinates of X-axis dimension for zoom range display purposes
                {
                    Series s = RootSeries.SimpleSubseriesList[0];
                    double position = 0;
                    double pxWidth = 0; ;

                    StringBuilder dimensionDescriptor = new StringBuilder("[");
                    foreach (DataPoint dp in s.DataPoints)
                    {
                        this.CoordinateSystem.XAxis.GetPixelPosition(dp.Parameters["x"], out position, out pxWidth);
                        dimensionDescriptor.Append("['");
                        dimensionDescriptor.Append(CommonFunctions.GetCultureFreeString(dp.Parameters["x"]));
                        dimensionDescriptor.Append("',");
                        dimensionDescriptor.Append(CommonFunctions.GetCultureFreeString(Math.Round(position)));
                        dimensionDescriptor.Append("],");
                    }
                    dimensionDescriptor.Append("['end'," + CommonFunctions.GetCultureFreeString(Math.Round(position + pxWidth)) + "]]");
                    dimensionDesc = dimensionDescriptor.ToString();
                }

                String ScrollJsId = GetSaneClientSideID();

                startupScripts.Append("<script type='text/javascript'>\n");
				startupScripts.Append("window." + ScrollJsId + ".rangePicker = new ComponentArt.Web.Visualization.Charting.RangePicker(\'"
                       + ScrollJsId + "\'," + this.Height.Value + "," + this.Width.Value + ","
                       + m_chart.XAxisStartPixel + "," + m_chart.XAxisEndPixel + "," + CommonFunctions.GetCultureFreeString(this.ScrollStepPercentage)
                       + "," + minCoord + "," + maxCoord + "," + dimension + "," + RangeClientTemplateXoffset + "," + RangeClientTemplateYoffset
                       + ",[");
                foreach (Chart wc in controlledCharts)
                    startupScripts.Append("'" + wc.GetSaneClientSideID() + "',");

                startupScripts.Append("]," + dimensionDesc + ",'" + RangeClientTemplateDateFormat + "');\n");

                //serialize the client templates to the client-side
                startupScripts.Append("window." + ScrollJsId + ".rangePicker.ClientTemplates = " + this.m_clientTemplates.ToString() + ";\n");

                //check if we need to persist previous positioning of the control
                String previousPosition = Context.Request.Form[this.m_ControlSettingsInputFieldName];
                if ((previousPosition != null) && (!"".Equals(previousPosition)))
                    startupScripts.Append("window." + ScrollJsId + ".rangePicker.setPosition(" + previousPosition + ");\n");
                else
                    startupScripts.Append("window." + ScrollJsId + ".rangePicker.setPosition(" + getZoomRangeParameters() + ");\n");

                startupScripts.Append("</script> \n");
            }
            
            if (startupScripts.Length > 0) 
                 WriteStartupScript(clientSideHtml, startupScripts.ToString());

            ////create the images used for series highlighting in a new thread, so that the current
            ////request can complete and the user can see the original chart before these images are completed
            //if (m_EnableSeriesHighlighting)
            //{
            //    m_disposeLater = true;
            //    m_seriesHighlighterThread = new Thread(new ThreadStart(this.CreateHighlightedImages));
            //    m_seriesHighlighterThread.Name = "CA Web.Visualization.Charting.Core Series Highlighting thread";
            //    m_seriesHighlighterThread.Start();
            //}


			if (!m_chart.HasErrors)
			{
				string areaMapsHtml = getAreaMapInnerHtml(false);
			
				if (areaMapsHtml != null && areaMapsHtml != "")
				{
					clientSideHtml.Append("<map id=\"" + MapID + "\" name=\"" + MapID + "\">\n");
					clientSideHtml.Append(areaMapsHtml);
					clientSideHtml.Append("</map>\n");
//					Debug.WriteLine("---------- AREA MAP --------------");
//					Debug.WriteLine(areaMapsHtml);
				}
			}


            //if caching enabled and client-side API or image maps enabled, cache the client-side settings (whole response)
            //for both callback and postback
            if (this.CacheInterval > 0 && (this.ClientsideApiEnabled || this.RenderAreaMap) 
                && ((this.ClientsideCustomizedImageCachingEnabled && m_customized) || !m_customized) )
            {
                String callbackResponse = createCallbackResponse(this.DynamicImage.GetImageUrl(this), minCoord, maxCoord);

                this.WebChartImageInfo.ClientsideCallbackHtml = callbackResponse;
                this.WebChartImageInfo.ClientsidePostbackHtml = clientSideHtml.ToString();

                this.DynamicImage.StoreClientsideHtml(this);
            }

            //set the storage ID variables in ViewState
            ViewState["StorageKey"] = DynamicImage.StorageKey;
            ViewState["urlGuid"] = DynamicImage.URLGUID;

            //if callback send the response, if postback write to the output stream
            if (Context != null && Context.Request.QueryString[GetSaneClientSideID() + "_Callback"] != null)
                Context.Response.End();
            else
                output.Write(clientSideHtml.ToString());
        }

        private string retrieveCachedWebChartHtml()
        {
            if (this.SaveImageOnDisk)
            {
                //retrieve saved HTML from disk
                return "";
            }
            else
            {
                //retrieve saved HTML from application cache
                return ((DynamicImage.ImageEntry)Context.Cache[Key]).Maps;
            }
            
        } 

        private string getAreaMapInnerHtml(bool escapeForJS)
        {
            string areasStr;

            if (UseCached)
            {
                areasStr = ((DynamicImage.ImageEntry)Context.Cache[Key]).Maps;
            }
            else
            {
                areasStr = m_chart.BuildMapAreaHtml(ClientsideApiEnabled, SerializeDatapoints, GetSaneClientSideID());

                if (Context != null)
                {
                    DynamicImage.ImageEntry ie = (DynamicImage.ImageEntry)Context.Cache[Key];
                    if (ie != null)
                        ie.Maps = areasStr;
                }
            }

           if (escapeForJS)
                areasStr = EscapeForJS(areasStr);

            return areasStr;
        }

        private static String EscapeForJS(String innerHtml)
        {
            innerHtml = innerHtml.Replace("\n", "\\n");
            innerHtml = innerHtml.Replace("\r", "");
            innerHtml = innerHtml.Replace("'", "\\'");
            return innerHtml;
        }

        internal bool SerializeDatapoints = false;

        //recursive method for generating the clientside instantiation of simple series
        private String seriesClientsideText(SeriesBase s)
        {
            StringBuilder seriesText = new StringBuilder();
            if (s is CompositeSeries)
            {
                CompositeSeries cs = (CompositeSeries)s;
                foreach (SeriesBase currentSeries in cs.SubSeries)
                {
                    seriesText.Append(seriesClientsideText(currentSeries));
                }
            }
            else if (s is Series)
            {
                Series series = (Series)s;
                //if (m_EnableSeriesHighlighting)
                //    seriesText.Append(",['" + s.Name + "','" + s.Style + "','" + getHighligtedImageUrl(s) + "',");
                //else
                    seriesText.Append(",['" + s.Name + "','" + s.Style + "',");

                if (this.SerializeDatapoints)
                {   
                    seriesText.Append("[");
                    foreach (DataPoint dp in series.DataPoints)
                    {
                        seriesText.Append("['");
                        seriesText.Append(EscapeForJS(dp.Parameters["x"].ToString()));
                        seriesText.Append("',");
                        seriesText.Append(dp.Parameters["y"]);
                        seriesText.Append(",");
                        seriesText.Append(dp.Parameters["isMissing"]);
                        seriesText.Append(",'");
                        seriesText.Append(dp.Parameters["from"]);
                        seriesText.Append("','");
                        seriesText.Append(dp.Parameters["to"]);
                        seriesText.Append("',");
                        seriesText.Append(dp.Parameters["high"]);
                        seriesText.Append(",");
                        seriesText.Append(dp.Parameters["low"]);
                        seriesText.Append(",");
                        seriesText.Append(dp.Parameters["open"]);
                        seriesText.Append(",");
                        seriesText.Append(dp.Parameters["close"]);
                        seriesText.Append("],");
                    }
                    seriesText.Remove(seriesText.Length-1, 1);
                    seriesText.Append("]");
                }

                seriesText.Append("]");
            }
            return seriesText.ToString();
        }

#if __COMPILING_FOR_2_0_AND_ABOVE__
        private string getScriptImportString(String fullScriptPath)
        {
            string sResourceUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), fullScriptPath);
            //sResourceUrl = sResourceUrl.Replace("&", "&amp;");
            return "<script src=\"" + sResourceUrl + "\" type=\"text/javascript\"></script>";
        }
#endif

        private string getClientScriptBlock(string sDefaultPath, string sScriptFile)
        {
#if __COMPILING_FOR_2_0_AND_ABOVE__
            return getScriptImportString(sDefaultPath + "." + sScriptFile);
#else
            return GenerateClientScriptBlock(sDefaultPath, sScriptFile);
#endif
        }      
        

#if FWAJAX || FW35
        protected void RegisterScriptForAtlas(string sResourceName)
        {
            //if (_scriptHandlerControls != null)
            //{
            //    // do nothing, we're using our own script handler
            //    return;
            //}

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
#if FWAJAX || FW35
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
                if (!IsClientScriptBlockRegistered(typeof(object), "B317u.js"))
                {
                    RegisterClientScriptBlock(typeof(object), "B317u.js", "");
                    RegisterScriptForAtlas("ComponentArt.Web.Visualization.Charting.client_scripts.B317u.js");
                }

                if (!IsClientScriptBlockRegistered("B317k.js"))
                {
                    RegisterClientScriptBlock("B317k.js", "");
                    RegisterScriptForAtlas("ComponentArt.Web.Visualization.Charting.client_scripts.B317k.js");
                }

                if (IsScrollControl && !IsClientScriptBlockRegistered("B317z.js"))
                {
                    RegisterClientScriptBlock("B317z.js", "");
                    RegisterScriptForAtlas("ComponentArt.Web.Visualization.Charting.client_scripts.B317z.js");
                }

                if (ViewAngleChooserEnabled && !IsClientScriptBlockRegistered("B317b.js"))
                {
                    RegisterClientScriptBlock("B317b.js", "");
                    RegisterScriptForAtlas("ComponentArt.Web.Visualization.Charting.client_scripts.B317b.js");
                }
            }
        }
#endif

        private void RegisterClientScripts()
        {
            if (DesignMode || !m_EnableClientsideApi)
                return;

            //Initialize the Clientside JavaScript objects to be used with the clientside API,
            //if already not done with ScriptManager
            if (!IsClientScriptBlockRegistered(typeof(object), "B317u.js"))
            {
				string script = getClientScriptBlock("ComponentArt.Web.Visualization.Charting.client_scripts", "B317u.js");
                RegisterClientScriptBlock(typeof(object), "B317u.js", script);
            }

            if (!IsClientScriptBlockRegistered("B317k.js"))
            {
				string script = getClientScriptBlock("ComponentArt.Web.Visualization.Charting.client_scripts", "B317k.js");
                RegisterClientScriptBlock("B317k.js", script);
            }

            if (IsScrollControl && !IsClientScriptBlockRegistered("B317z.js"))
            {
				string script = getClientScriptBlock("ComponentArt.Web.Visualization.Charting.client_scripts", "B317z.js");
                RegisterClientScriptBlock("B317z.js", script);
            }

            if (ViewAngleChooserEnabled && !IsClientScriptBlockRegistered("B317b.js"))
            {
				string script = getClientScriptBlock("ComponentArt.Web.Visualization.Charting.client_scripts", "B317b.js");
                RegisterClientScriptBlock("B317b.js", script);
            }
        }


        private string GenerateClientScriptBlock(string sDefaultPath, string sScriptFile)
        {
            string sScript = string.Empty;
            string sScriptLocation = string.Empty;

#if SCRIPT_DEBUG
      // If SCRIPT_DEBUG is defined we force inline script output.
      sScript = Utils.DemarcateClientScript(GetResourceContent(sDefaultPath + "." + sScriptFile));
      return sScript;
#endif

            if (this.ClientScriptLocation != string.Empty)
            {
                sScriptLocation = Path.Combine(Path.Combine(this.ClientScriptLocation, this.VersionString), sScriptFile).Replace("\\", "/");
            }
#if !ASPNET2
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
#endif

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
#if ASPNET2
        string sResourceUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), sDefaultPath + "." + sScriptFile);
        sResourceUrl = sResourceUrl.Replace("&", "&amp;");
        sScript = "<script src=\"" + sResourceUrl + "\" type=\"text/javascript\"></script>";
#endif

#if !ASPNET2
                sScript = Utils.DemarcateClientScript(GetResourceContent(sDefaultPath + "." + sScriptFile));
#endif
            }

            return sScript;
        }

#if ATLAS
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

        internal string MapID
        {
            get
            {
                return GetSaneClientSideID() + "Map";
            }
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

        /// <summary>
        /// Base class method overriden.
        /// </summary>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            string imageUrl = DynamicImage.GetImageUrl(this);
            if (imageUrl.Length > 0)
            {
                string resolvedUrl = base.ResolveUrl(imageUrl);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, resolvedUrl);
            }

            if (this.BorderWidth.IsEmpty)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            }

            if (RenderAreaMap && !DesignMode)
                writer.AddAttribute("usemap", "#" + MapID);
        }


        /// <summary>
        /// Initializes a new instance of the Chart class.
        /// </summary>
        public Chart()
#if !__COMPILING_FOR_2_0_AND_ABOVE__
            : base()
#endif
        {
            Width = 350;
            Height = 270;

            m_chart = new ChartBase();
			licence = GetLicenseType();
            BackColor = BackColor = Color.Transparent;
        }

        internal void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_chart != null)
                {
                    m_chart.Dispose();
                    m_chart = null;
                }
                if (DynamicImage != null)
                {
                    m_di = null;
                }
            }
        }

        /// <summary>
        /// Base class method overriden.
        /// </summary>
        public sealed override void Dispose()
        {
                base.Dispose();
                Dispose(true);
                GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// Base class method overriden.
        /// </summary>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {        
            this.AddAttributesToRender(writer);
            writer.RenderBeginTag("img");
        }

		private LicenseType licence = LicenseType.Expired;

        internal LicenseType GetLicenseType()
        {
            LicenseType type;
            try
            {
                License lic = LicenseManager.Validate(typeof(Chart), this);
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

        /// <summary>
        /// Gets or sets the back color of the chart.
        /// </summary>
        [SRCategory("CatAppearance")]
        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.All)]
        public override Color BackColor
        {
            get
            {
                return m_chart.BackColor;
            }
            set
            {
                m_chart.BackColor = value;
            }
        }


        /// <summary>
        /// Relative or absolute path to the folder containing the client-side script file(s).
        /// </summary>
        /// <remarks>
        /// The actual JavaScript files are placed in a folder named to correspond to the version of Chart being used, inside a folder named
        /// <b>componentart_webchart_client</b>, which is placed in the folder specified by this property.
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

        private Exception m_exception = null;
        internal Exception Exception { get { return m_exception; } }


#if !__COMPILING_FOR_2_0_AND_ABOVE__
        /// <summary>
        ///		Completes input variables definition and builds internal structure of the chart.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///		This function is invoked from <see cref="Control.DataBind"/> call which should be called after data has been defined using
        ///		<see cref="Chart.DefineData"/>, 
        ///		<see cref="Chart.DefineAsExpression"/>, 
        ///		<see cref="Chart.DefineDataPath"/> and
        ///		<see cref="Chart.DataSource"/>. 
        ///	  </para>
        ///   <para>
        ///     <b>Many objects in the chart object model are not available
        ///     before <see cref="Chart.OnDataBinding"/> is completed.</b> The parts of the chart that depend on 
        ///     input data do not exist before <see cref="Chart.OnDataBinding"/>. That includes <see cref="DataPoint"/>s,
        ///     <see cref="SeriesLabels"/>s, <see cref="Axis"/>, <see cref="CoordinatePlane"/> objects and related objects
        ///     (because they cannot be created before data values, values ranges and data type are known) and
        ///     complete inner coordinate systems (in case of multi system or multi area charts).
        ///     For more on charting data structures and data binding, see "Basic Concepts" topics and topic "Advanced Data Binding"
        ///     in "Advanced Concepts".
        ///   </para>
        /// </remarks>
        protected override void OnDataBinding(EventArgs e)
        {
            if (!m_customizationsLoaded)
                m_customized = LoadCustomizations();
            base.OnDataBinding(e);

            try
            {
                m_chart.TargetSize = new System.Drawing.Size((int)(Width.Value), this.ImageHeight);
                //m_chart.DataBind();
            }
            catch (Exception ex)
            {
                m_exception = ex;
            }
        }
#endif


        internal ChartBase m_chart;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [NotifyParentProperty(true)]
        [RefreshProperties(RefreshProperties.All)]
        internal ChartBase ChartBase
        {
            get { return m_chart; }
            set { m_chart = value; }
        }


        //Bitmap bmp;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //m_di.Chart = this;
            //m_di.Page = Page;
            DetermineClientsideIDs();
            Page.Load += new EventHandler(PageLoad);
        }

        private void DetermineClientsideIDs()
        {
            m_customizationsInputFieldName = "CustomChart_" + GetSaneClientSideID();
            m_ControlSettingsInputFieldName = "Position_" + GetSaneClientSideID();
            m_returnDataFormatInputFiledName = GetSaneClientSideID() + "_dataOnlyFormat";
        }

        DynamicImage DynamicImage
        {
            get
            {
                if (m_di == null)
                {
                    m_di = new DynamicImage();
                }
                return m_di;
            }
        }

        /// <summary>
        /// Text to be displayed in the frame.
        /// </summary>
        [Description("Text to be displayed in the frame.")]
        [DefaultValue("")]
        [Category("Appearance")]
        public string Text
        {
            get
            {
                return m_text;
            }
            set
            {
                m_text = value;
            }
        }


        public void Clear() 
        {
            m_chart = new ChartBase();
            m_chart.Mapping.ViewDirectionChanged += new EventHandler(this.HandleViewDirectionChanged);
        }


        private string m_key = null;

        internal string Key
        {
            get
            {
                if (Context == null)
                    return null;
                if (m_key == null)
                    m_key = Context.Request.Url + "+++" + UniqueID;

                return m_key;
            }
            set
            {
                m_key = value;
            }
        }

        private bool m_useCached = false;

        internal bool UseCached
        {
            get
            {
                return m_useCached;
            }
        }

        public override void  DataBind()
        {
			if(PreDataBind != null)
				PreDataBind(this);

            if (!m_customizationsLoaded)
                m_customized = LoadCustomizations();

            base.DataBind();

             m_chart.TargetSize = new System.Drawing.Size((int)(Width.Value), this.ImageHeight);
             m_chart.DataBind();

             LoadPostDataBindCustomizations();

			if(PostDataBind != null)
				PostDataBind(this);


		}

        void PageLoad(object sender, EventArgs e) 
        {
        }        

        public bool LoadPostDataBindCustomizations()
        {
            //no need to check for customizations in design time.
            if (Context == null)
                return false;

            String chartCustomizations = Context.Request.Form[m_customizationsInputFieldName];
            if (chartCustomizations == null || "".Equals(chartCustomizations))
            {
                return false;
            }
            else if (chartCustomizations != null && !"".Equals(chartCustomizations))
            {
                // 
                String[] customizationsArray = chartCustomizations.Split(new Char[] { ',' });

                String MoveSeries = customizationsArray[12];
                if (MoveSeries != null && !"".Equals(MoveSeries))
                {
                    try
                    {
                        String[] seriesToMove = MoveSeries.Split(new Char[] { '|' });
                        foreach (String s in seriesToMove)
                        {
                            if (s[0] == 'f')
                                this.RootSeries.FindSeries(s.Substring(1)).MoveToFront();
                            else if (s[0] == 'b')
                                this.RootSeries.FindSeries(s.Substring(1)).MoveToBack();
                        }
                        return true;
                    }
                    catch
                    {
                        //ignore, or report that the ToBackSeries was not moved
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Loads customizations by setting all chart properties that have been customized through the client-side API.
        /// Called at data bind time if not explicitly called before, subsequent calls have no effect.
        /// </summary>
        /// <returns>Whether the customizations were successfully loaded</returns>
        public bool LoadCustomizations()
        {
                m_customizationsLoaded = true;
                bool foundCustomizations = false;

                //no need to check for customizations in design time.
                if (Context == null)
                    return false;

                //preserve the original chart settings in a string that is passed to the clientside API
                StringBuilder originalSettings = new StringBuilder();
                originalSettings.Append("'" + this.MainStyle + "',");
                originalSettings.Append(this.Legend.Visible.ToString().ToLower() + ",");
                originalSettings.Append("'" + this.SelectedPaletteName + "',");
                originalSettings.Append("'" + this.View.Kind + "',");
                originalSettings.Append("'" + this.CoordinateSystem.Orientation + "',");
                originalSettings.Append("'" + this.View.Perspective + "',");
                originalSettings.Append("'" + this.View.ViewDirection.X + "',");
                originalSettings.Append("'" + this.View.ViewDirection.Y + "',");
                originalSettings.Append("'" + this.View.ViewDirection.Z + "',");
                originalSettings.Append(this.CoordinateSystem.PlaneXY.Visible.ToString().ToLower() + ",");
                originalSettings.Append(this.CoordinateSystem.PlaneYZ.Visible.ToString().ToLower() + ",");
                originalSettings.Append(this.CoordinateSystem.PlaneZX.Visible.ToString().ToLower() + ",");
                originalSettings.Append("'" + "##m_customizationsInputFieldName##" + "',");
                originalSettings.Append("'" + "##m_returnDataFormatInputFiledName##" + "',");
                originalSettings.Append("'" + "##SaneClientSideID##" + "',");
                originalSettings.Append(this.AutoRenderOnChange.ToString().ToLower() + ",");
                originalSettings.Append("'" + this.GeometricEngineKind + "',");
                originalSettings.Append(this.AdjustReferenceValue.ToString().ToLower() + ",");
                originalSettings.Append(licence == LicenseType.Full ? "1," : "0,");
				originalSettings.Append(m_LoadingChartImagePath == null ? "0" : "1");
                
                //originalSettings.Append(this.SeriesHighlightingEnabled.ToString().ToLower());
                m_defaultChartProperties = originalSettings.ToString();

                //get the user customizations string from the request
                String chartCustomizations = Context.Request.Form[m_customizationsInputFieldName];
                String customProperties = Context.Request.Form[m_customizationsInputFieldName + "_properties"];

                if ((chartCustomizations == null || "".Equals(chartCustomizations))
                    && (customProperties == null || "".Equals(customProperties)))
                {
                    return false;
                }
                if (chartCustomizations != null && !"".Equals(chartCustomizations) && chartCustomizations.Replace(",", String.Empty) != String.Empty)
                {
                    // 
                    String[] customizationsArray = chartCustomizations.Split(new Char[] { ',' });


                    String MainChartType = customizationsArray[0];
                    if (MainChartType != null && !"".Equals(MainChartType))
                    {
                        this.MainStyle = MainChartType;
                        foundCustomizations = true;
                    }

                    String CustomChartLegendVisibility = customizationsArray[1];
                    if (CustomChartLegendVisibility != null && !"".Equals(CustomChartLegendVisibility))
                    {
                        try
                        {
                            this.Legend.Visible = Boolean.Parse(CustomChartLegendVisibility);
                            foundCustomizations = true;
                        }
                        catch 
                        {
                            //ignore, or report that the ChartBase Legend Visibility was not formatted properly 
                        }
                    }
                    String CustomChartColorPalette = customizationsArray[2];
                    if (CustomChartColorPalette != null && !"".Equals(CustomChartColorPalette))
                    {
                        this.SelectedPaletteName = CustomChartColorPalette;
                        foundCustomizations = true;
                    }

                    String ChartProjectionKind = customizationsArray[3];
                    if (ChartProjectionKind != null && !"".Equals(ChartProjectionKind))
                    {
                        try
                        {
                            this.View.Kind = (ProjectionKind)Enum.Parse(typeof(ProjectionKind), ChartProjectionKind);
                            foundCustomizations = true;
                        }
                        catch
                        {
                            //ignore, or report that the ChartProjectionKind was not formatted properly 
                        }
                    }

                    //done after databind()???
                    String ChartOrientation = customizationsArray[4];
                    if (ChartOrientation != null && !"".Equals(ChartOrientation))
                    {
                        try
                        {
                            this.CoordinateSystem.Orientation = (CoordinateSystemOrientation)Enum.Parse(typeof(CoordinateSystemOrientation), ChartOrientation);
                            foundCustomizations = true;
                        }
                        catch
                        {
                            //ignore, or report that the ChartOrientation was not formatted properly 
                        }
                    }

                    String ChartPerspective = customizationsArray[5];
                    if (ChartPerspective != null && !"".Equals(ChartPerspective))
                    {
                        this.View.Perspective = Int32.Parse(ChartPerspective);
                        foundCustomizations = true;
                    }

                    if (customizationsArray[6] != null && !"".Equals(customizationsArray[6])
                        || customizationsArray[7] != null && !"".Equals(customizationsArray[7])
                        || customizationsArray[8] != null && !"".Equals(customizationsArray[8]))
                    {
                        try
                        {
                            double ChartViewingDirectionX = Double.Parse(customizationsArray[6]);
                            double ChartViewingDirectionY = Double.Parse(customizationsArray[7]);
                            double ChartViewingDirectionZ = Double.Parse(customizationsArray[8]);
                            this.View.ViewDirection = new Vector3D(ChartViewingDirectionX, ChartViewingDirectionY, ChartViewingDirectionZ);
                            foundCustomizations = true;
                        }
                        catch
                        {
                            //ignore, or report that the ChartBase View direction was not formatted properly 
                        }
                    }

                    String ChartPlaneXYVisible = customizationsArray[9];
                    if (ChartPlaneXYVisible != null && !"".Equals(ChartPlaneXYVisible))
                    {
                        try
                        {
                            this.CoordinateSystem.PlaneXY.Visible = Boolean.Parse(ChartPlaneXYVisible);
                            foundCustomizations = true;
                        }
                        catch 
                        {
                            //ignore, or report that the ChartPlaneXYVisible was not formatted properly 
                        }
                    }

                    String ChartPlaneYZVisible = customizationsArray[10];
                    if (ChartPlaneYZVisible != null && !"".Equals(ChartPlaneYZVisible))
                    {
                        try
                        {
                            this.CoordinateSystem.PlaneYZ.Visible = Boolean.Parse(ChartPlaneYZVisible);
                            foundCustomizations = true;
                        }
                        catch
                        {
                            //ignore, or report that the ChartPlaneYZVisible was not formatted properly 
                        }
                    }

                    String ChartPlaneZXVisible = customizationsArray[11];
                    if (ChartPlaneZXVisible != null && !"".Equals(ChartPlaneZXVisible))
                    {
                        try
                        {
                            this.CoordinateSystem.PlaneZX.Visible = Boolean.Parse(ChartPlaneZXVisible);
                            foundCustomizations = true;
                        }
                        catch 
                        {
                            //ignore, or report that the ChartPlaneZXVisible was not formatted properly 
                        }
                    }

                    //Moving series to front/back [12] is done post dataBind

                    String ZoomTo = customizationsArray[13];
                    if (ZoomTo != null && !"".Equals(ZoomTo))
                    {
                        try
                        {
                            bool absoluteZoom = ZoomTo[0] == 'a';
                            bool ZoomRangeIsDateTime = ZoomTo.Substring(1).StartsWith("|DT|");
                            int startIndex = ZoomRangeIsDateTime ? 5 : 1;

                            String[] ZoomValues = ZoomTo.Substring(startIndex).Split(new Char[] { '|' });

                            String zoomStartX = ZoomValues[0];
                            String zoomEndX = ZoomValues[1];

                            if (absoluteZoom)
                            {
                                if (ZoomRangeIsDateTime)
                                {
                                    this.CoordinateSystem.XAxis.MinValue = ConvertJSTicksStringToDateTime(zoomStartX);
                                    this.CoordinateSystem.XAxis.MaxValue = ConvertJSTicksStringToDateTime(zoomEndX);
                                }
                                else
                                {
                                    this.CoordinateSystem.XAxis.MinValue = zoomStartX;
                                    this.CoordinateSystem.XAxis.MaxValue = zoomEndX;
                                }
                            }
                            else //relative zoom
                            {
                                double RelativeXPctStart = Double.Parse(ZoomValues[2], CultureInfo.InvariantCulture);
                                double RelativeXPctEnd = Double.Parse(ZoomValues[3], CultureInfo.InvariantCulture);

                                try
                                {
                                    double zoomStartXl = Convert.ToDouble(zoomStartX);
									double zoomEndXl = Convert.ToDouble(zoomEndX);
                                    double minXval = zoomStartXl + (zoomEndXl - zoomStartXl) * RelativeXPctStart;
                                    double maxXval = zoomStartXl + (zoomEndXl - zoomStartXl) * RelativeXPctEnd;

                                    if (ZoomRangeIsDateTime)
                                    {
                                        this.CoordinateSystem.XAxis.MinValue = ConvertJSTicksStringToDateTime(minXval);
                                        this.CoordinateSystem.XAxis.MaxValue = ConvertJSTicksStringToDateTime(maxXval);
                                    }
                                    else
                                    {
                                        this.CoordinateSystem.XAxis.MinValue = minXval;
                                        this.CoordinateSystem.XAxis.MaxValue = maxXval;
                                    }
                                }
                                catch (SystemException se)
                                {
                                    throw new Exception("Cannot use relative zooming with the current X-axis dimension. Use the absolute zooming method.", se);
                                    //NOTE: This could be implemented if needed by mapping String values to their coordinate values.
                                }
                            }

                            m_RangeChangedByClientside = true;
                            this.CoordinateSystem.XAxis.RoundValueRange = false;
                            foundCustomizations = true;
                        }
                        catch
                        {
                            //ignore, or report that the zoom range was not set
                        }
                    }

                    String highlightS = customizationsArray[14];
                    if (highlightS != null && !"".Equals(highlightS))
                    {
                        foreach (Series s in RootSeries.SimpleSubseriesList)
                        {
                            if (s.Name == highlightS)
                                s.Transparency = 5;
                            else
                                s.Transparency = 60;
                        }
                        foundCustomizations = true;
                    }

                    String renderingEng = customizationsArray[15];
                    if (renderingEng != null && !"".Equals(renderingEng))
                    {
                        try
                        {
                            this.GeometricEngineKind = (GeometricEngineKind)Enum.Parse(typeof(GeometricEngineKind), renderingEng);
                            foundCustomizations = true;
                        }
                        catch
                        {
                            //ignore, or report that the ChartPlaneYZVisible was not formatted properly 
                        }
                    }

                    String adjustYaxis = customizationsArray[16];
                    if (adjustYaxis != null && !"".Equals(adjustYaxis))
                    {
                        try
                        {
                            this.AdjustReferenceValue = Boolean.Parse(adjustYaxis);
                            foundCustomizations = true;
                        }
                        catch
                        {
                            //ignore, or report that the ChartPlaneYZVisible was not formatted properly 
                        }
                    }

                    m_loadedCustomizations = chartCustomizations;

                }
                if (customProperties != null && !"".Equals(customProperties))
                {
                    String[] customPropArray = customProperties.Split(new Char[] { '`' });

                    if (customPropArray.Length > 1)
                        foundCustomizations = true;

                    for (int j = 0; j < customPropArray.Length - 1; j += 2)
                    {
                        m_customPropertiesHashtable.Add(customPropArray[j], customPropArray[j + 1]);
                    }
                    m_loadedCustomProperties = customProperties;
                }

                m_customized = foundCustomizations;
                return foundCustomizations;

        }


        private bool m_RangeChangedByClientside = false;

        private DateTime ConvertJSTicksStringToDateTime(long val)
        {
            //need to account that value comes from JavaScript Date object, 
            //which is milliseconds since Jan 1, 1970
            DateTime zero = new DateTime(1970, 1, 1);
            TimeSpan ts = new TimeSpan((long)(val * TimeSpan.TicksPerMillisecond));

            return zero.Add(ts);
        }

        private DateTime ConvertJSTicksStringToDateTime(double val)
        {
            return ConvertJSTicksStringToDateTime(Convert.ToInt64(val));
        }

        private DateTime ConvertJSTicksStringToDateTime(string value)
        {
            return ConvertJSTicksStringToDateTime(Convert.ToInt64(value));
        }

        public string GetCustomProperty(string key)
        {
            return (string)m_customPropertiesHashtable[key];
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private bool ShouldBrowseJpegQuality()
        {
            return WebChartImageType == ChartImageType.Jpeg;
        }

        int m_jpegQuality = 85;

        /// <summary>
        /// Gets or sets the jpeg quality of the image.
        /// </summary>
        [DefaultValue(85)]
        [SRCategory("CatBehavior")]
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

        string m_designSavePath = string.Empty;

        internal string DesignSavePath
        {
            get { return m_designSavePath; }
            set { m_designSavePath = value; }
        }


        #region --- ConvertUrl Functions ---
        /// <summary>
        /// Determines whether the given string is an absolute URL.
        /// </summary>
        /// <param name="url">The string to examine.</param>
        /// <returns>True if the given string begins with a valid protocol identifier; false otherwise.</returns>
        internal static bool IsUrlAbsolute(string url)
        {
            if (url == null) return false;
            string[] protocols = { "about:", "file:///", "ftp://", "gopher://", "http://", "https://", "javascript:", "mailto:", "news:", "res://", "telnet://", "view-source:" };
            foreach (string protocol in protocols)
                if (url.StartsWith(protocol))
                    return true;
            return false;
        }



        /// <summary>
        /// Resolve the effective URL given its string, a base URL, and the HttpContext.
        /// </summary>
        /// <param name="oContext">HTTP Context.</param>
        /// <param name="sBaseUrl">Base URL.</param>
        /// <param name="sUrl">URL.</param>
        /// <returns>Effective URL.</returns>
        internal static string ConvertUrl(HttpContext oContext, string sBaseUrl, string sUrl)
        {
            if (sUrl == null || sUrl == string.Empty || IsUrlAbsolute(sUrl) || (!sUrl.StartsWith("/") && Path.IsPathRooted(sUrl)))
            {
                return sUrl;
            }

            // Do we have a tilde?
            if (sUrl.StartsWith("~") && oContext != null && oContext.Request != null)
            {
                string sAppPath = oContext.Request.ApplicationPath;
                if (sAppPath.EndsWith("/"))
                {
                    sAppPath = sAppPath.Substring(0, sAppPath.Length - 1);
                }

                return sUrl.Replace("~", sAppPath);
            }

            if (sBaseUrl != string.Empty)
            {
                // Do we have a tilde in the base url?
                if (sBaseUrl.StartsWith("~") && oContext != null && oContext.Request != null)
                {
                    string sAppPath = oContext.Request.ApplicationPath;
                    if (sAppPath.EndsWith("/"))
                    {
                        sAppPath = sAppPath.Substring(0, sAppPath.Length - 1);
                    }
                    sBaseUrl = sBaseUrl.Replace("~", sAppPath);
                }

                if (sBaseUrl.EndsWith("/"))
                {
                    sBaseUrl = sBaseUrl.Substring(0, sBaseUrl.Length - 1);
                }

                if (sUrl.StartsWith("/"))
                {
                    sUrl = sUrl.Substring(1, sUrl.Length - 1);
                }

                return sBaseUrl + "/" + sUrl;
            }

            return sUrl;
        }

        private static string NormalizePath(string path)
        {
            // replace any backslashes 
            path = path.Replace('\\', '/');
            
            // remove any double forward slashes                 
            int pathLength = 0;
            while (path.Length > 0 && path.Length != pathLength) 
            {
                pathLength = path.Length;
                path = path.Replace("//", "/");
            }             
             
            // remove trailing any forward slashes
            while (path.Length > 0 && path.EndsWith("/"))
                path = path.Substring(0, path.Length - 1);

            // split parts of path up                    
            string[] pathParts = path.Split(new char[] { '/' });                
            
            // collapse any . and .. parts
            int index = 0; 
            for (int i = 0; i < pathParts.Length; i++) 
            {
                string pathPart = pathParts[i];
                
                // ignore current directory references
                if (pathPart == ".") 
                    continue;
                
                // handle parent directory references, if we have a parent
                if (pathPart == ".." && index > 0) 
                {
                    --index;
                    continue;
                }

                // else add path part
                pathParts[index++] = pathPart;
            }
            
            return String.Join("/", pathParts, 0, index);
        }

        private static string MapPhysicalPath(string path)
        {
            if (path == null || path == String.Empty)
                return String.Empty;

            path = path.Trim();

            if (path == String.Empty)
                return String.Empty;

            if (IsUrlAbsolute(path))
            {
                const string fileProtocol = "file:///";

                if (!path.StartsWith(fileProtocol))
                    return String.Empty;

                path = path.Substring(fileProtocol.Length);
            }

            if (!path.StartsWith("/") && Path.IsPathRooted(path))
                return Path.GetFullPath(path);
                
            HttpContext context = HttpContext.Current;

            if (context == null)
                return path;
                
            return context.Server.MapPath(path);
        }

        private static string MapVirtualPath(string path)
        {
            HttpContext context = HttpContext.Current;            
            string appPath = NormalizePath(MapPhysicalPath(context.Request.ApplicationPath));
            string absPath = NormalizePath(MapPhysicalPath(path));

            if (String.Compare(appPath, 0, absPath, 0, appPath.Length, true) != 0)
                throw new Exception("Path '" + absPath + "' is not descended from Application root.");
                
            string virtualPath = NormalizePath(context.Request.ApplicationPath + absPath.Substring(appPath.Length));

            return virtualPath;
        }

        private static string MapRelativePath(string path, Page page, Chart webChart)
        {
            string dummyRoot = "http://www.componentart.com";
            string pagePath = "";
            if (page != null)
                pagePath = page.ResolveUrl(".");
            else
                pagePath = webChart.TemplateSourceDirectory;

            //Uri pageUri = new Uri(dummyRoot + NormalizePath(page.ResolveUrl(".")) + "/");
            Uri pageUri = new Uri(dummyRoot + NormalizePath(pagePath) + "/");
            Uri pathUri = new Uri(dummyRoot + NormalizePath(webChart.ResolveUrl(path)) + "/");
            
#if __COMPILING_FOR_2_0_AND_ABOVE__
            string relativePath = NormalizePath(pageUri.MakeRelativeUri(pathUri).ToString());        
#else
            string relativePath = NormalizePath(pageUri.MakeRelative(pathUri).ToString());        
#endif
            return (relativePath == "" ? "." : relativePath);
        }

        private string MapRelativePath(string path)
        {
            return MapRelativePath(path, this.Page, this);
        }

        #endregion


        private Object m_fileNameLock = new Object();
        private string customFileName = null;

        /// <summary>
        /// The custom image filename without an extension.
        /// </summary>
        /// <remarks>
        /// If this property is set (i.e. it's not null nor empty), it is used to create the image filename. Otherwise,
        /// the control creates a unique image filename for each rendered chart. Note that the file extension is appended
        /// to the filename, according to the <see cref="ChartImageType"/> property.
        /// </remarks>
        [Description("Custom image file name. If not provided, control creates unique file name for each chart.")]
        [SRCategory("CatBehavior")]
        [DefaultValue("")]
        public string CustomImageFileName { get { return customFileName; } set { customFileName = value; } }

        private string EffectiveImageFilename(string name)
        {
            if (customFileName != null)
                customFileName = customFileName.Trim();
            if (customFileName == null || customFileName == "")
            {
                Guid g = Guid.NewGuid();
                return name + "-" + g.ToString();
            }
            else
            {
                // strip extension
                string[] parts = customFileName.Split('.');
                return parts[0];
            }
        }

        string SaveImageToDisc(Bitmap bmp, string virtualDir, string name, bool useBaseDirectory)
        {
            return SaveImageToDisc(bmp, virtualDir, name, useBaseDirectory, null);
        }

        string SaveImageToDisc(Bitmap bmp, string virtualDir, string name, bool useBaseDirectory, string imageFileName)
        {
            string ext = (WebChartImageType == ChartImageType.Gif ? "gif" : WebChartImageType == ChartImageType.Jpeg ? "jpg" : "png");

            if (DesignMode)
            {
                SaveImageToDisc(bmp, DesignSavePath);
                DynamicImage.ImageFile = DesignSavePath;
                ViewState["ImageFile"] = DynamicImage.ImageFile;
                return DesignSavePath;
            }
            else
            {
                if (IsUrlAbsolute(virtualDir) || (!virtualDir.StartsWith("/") && Path.IsPathRooted(virtualDir)))
                    virtualDir = MapVirtualPath(virtualDir);

                string directory = ConvertUrl(Context, useBaseDirectory && !virtualDir.StartsWith("/") ? TemplateSourceDirectory : "", virtualDir);                                    
                    
                string normalizedPath = NormalizePath(directory);
                string physicalOutputDirectory = MapPhysicalPath(normalizedPath == "" ? "/" : normalizedPath);
                string relativePath = MapRelativePath(normalizedPath);

                if (!Directory.Exists(physicalOutputDirectory))
                    throw new DirectoryNotFoundException("Directory '" + physicalOutputDirectory + "' does not exist.");

                string filePath;
                lock (m_fileNameLock)
                {
                    for (int i = 0; ; ++i)
                    {
                        string nameWithExt;
                        if (imageFileName == null || String.Empty == imageFileName)
                            nameWithExt = EffectiveImageFilename(name) + "." + ext;
                        else
                            nameWithExt = imageFileName;

                        filePath = physicalOutputDirectory + Path.DirectorySeparatorChar + nameWithExt;

                        if (!System.IO.File.Exists(filePath))
                        {
                            try
                            {
                                SaveImageToDisc(bmp, filePath);
                            }
                            catch (System.Runtime.InteropServices.ExternalException)
                            {
                                m_attemptedToWriteToFile = filePath;
                                throw new IOException("Could not write image onto the file system at '" + filePath + "'.");
                            }

                            DynamicImage.ImageFile = relativePath + "/" + nameWithExt;
                            ViewState["ImageFile"] = DynamicImage.ImageFile;
                            break;
                        }
                    }
                }
                
                return filePath;
            }
        }

        void SaveImageToDisc(Bitmap bmp, string filePath)
        {
            Debug.WriteLine(" === Chart : " + this.GetHashCode() + " saved image to file " + filePath);
            FileStream stream = new FileStream(filePath, FileMode.Create);

            ImageFormat imageFormat;
            int jpegQuality;
            bool isJpeg = false;
            
            if (!DesignMode) 
            {
                imageFormat = (WebChartImageType == ChartImageType.Jpeg) ? ImageFormat.Jpeg :
                    (WebChartImageType == ChartImageType.Gif) ? ImageFormat.Gif : ImageFormat.Png;
                    
                jpegQuality = JpegQuality;
                isJpeg = (WebChartImageType == ChartImageType.Jpeg);
            }
            else
            {
                string fileExt = Path.GetExtension(filePath).ToLower();
                
                if (fileExt == ".jpg") 
                    imageFormat = ImageFormat.Jpeg;
                else if (fileExt == ".gif")
                    imageFormat = ImageFormat.Gif;
                else if (fileExt == ".png")
                    imageFormat = ImageFormat.Png;
                else 
                    imageFormat = ImageFormat.Bmp;
                    
                jpegQuality = 100;
                isJpeg = (fileExt == ".jpg");
            }
            
            if (isJpeg) 
            {
                ImageCodecInfo codecInfo = GetEncoderInfo("image/jpeg");
                System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters encoderParameters = new EncoderParameters(1);
                EncoderParameter encoderParameter = new EncoderParameter(encoder, (long)jpegQuality);
                encoderParameters.Param[0] = encoderParameter;

                bmp.Save(stream, codecInfo, encoderParameters);
            }
            else
            {
                bmp.Save(stream, imageFormat);
            }
            
            stream.Flush();
            stream.Close();
        }


        string m_imageOutputDirectory = "";

        /// <summary>
        /// Gets or sets the output directory for the image.
        /// </summary>
        [DefaultValue("")]
        [SRCategory("CatBehavior")]
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


        const int deletionDelay_Default = 30;

        int m_cacheInterval = 0;
        int m_deletionDelay = deletionDelay_Default;



        /// <summary>
        /// Gets or sets the duration in seconds for which the chart is valid and does not need to be rerendered.
        /// </summary>
        [SRCategory("CatBehavior")]
        [DefaultValue(0)]
        [Description("Duration in seconds for which the chart is valid.")]
        public int CacheInterval
        {
            get
            {
                return m_cacheInterval;
            }
            set
            {
                m_cacheInterval = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of time in seconds that the image will 
        /// exist after cache interval is over. In case where cache interval is zero, 
        /// this amount represents the time that the image will exist after its creation.
        /// </summary>
        /// <remarks>
        /// Use this property to set the amount of time the images will be stored in the 
        /// memory cache or on disk after it isn't valid anymore. This property exists to make sure
        /// that the client's image request is served even if the cache entry has just expired.
        /// 
        /// For pages that might take a long time to load this property should 
        /// be greater as the image might get deleted before it is requested.
        /// This will include pages that use very large charts, or other CPU intensive controls.
        /// 
        /// When the image is stored in the cache (not on disk) and the <see cref="CacheInterval"/> 
        /// is set to 0, the image will be deleted after the first access.
        /// 
        /// To handle deletion manually <see cref="Chart.Draw"/> method should be used to create an image from a <see cref="Chart"/>. 
        /// The image should later on be saved to either a cache or a file system and deleted by any other means.
        /// </remarks>
        [SRCategory("CatBehavior")]
        [DefaultValue(deletionDelay_Default)]
        [Description("Time the chart will stay available in the cache or on the file system after the cache period has expired.")]
        public int DeletionDelay
        {
            get
            {
                return m_deletionDelay;
            }
            set
            {
                m_deletionDelay = value;
            }
        }



        bool m_saveImageOnDisk = true;

        /// <summary>
        /// Gets or sets the value indicating if the chart image should be saved on disc or in the cache.
        /// </summary>
        [DefaultValue(true)]
        [SRCategory("CatBehavior")]
        [Description("Indicating if the chart image should be saved on disc or in the cache")]
        public bool SaveImageOnDisk
        {
            get { return m_saveImageOnDisk; }
            set
            {
                if (m_saveImageOnDisk == value)
                    return;

                //EraseKey();

                m_saveImageOnDisk = value;
            }
        }


        const ChartImageType _defaultWebChartImageType = ChartImageType.Png;
        ChartImageType m_webChartImageType = _defaultWebChartImageType;


        /// <summary>
        /// Gets or sets the format of the output image.
        /// </summary>
        [DefaultValue(_defaultWebChartImageType)]
        [SRCategory("CatBehavior")]
        [Description("Format of the output image.")]
        public ChartImageType WebChartImageType
        {
            get
            {
                return m_webChartImageType;
            }
            set
            {
                m_webChartImageType = value;
            }
        }

        bool CanUseCached()
        {
            if (this.DesignMode)
                return false;

            if (m_customized)
            {
                if (!ClientsideCustomizedImageCachingEnabled)
                {
                    return false;
                }
                else 
                {
                    this.Key += m_loadedCustomizations + m_loadedCustomProperties;
                }
            }

            if (Context != null && m_cacheInterval != 0)
            {
                DateTime lastRefresh = DynamicImage.GetStoredDate(Key, this);

                TimeSpan diff = DateTime.Now - lastRefresh;
                if (lastRefresh != DateTime.MinValue && diff.Seconds < m_cacheInterval)
                {
                    return true;
                }
            }
            return false;

        }

        void PreprocessImage()
        {
            PreprocessImage(null);
        }

        void PreprocessImage(MemoryStream ms)
        {
            PreprocessImage(ms, null);
        }

        void PreprocessImage(MemoryStream ms, string imageFileName)
        {

            if (!Visible)
            {
                return;
            }

            if (CanUseCached())
            {
                m_useCached = true;
                return;
            }

			Graphics g = null;
			Bitmap bmp = null;
			try
			{

				int w = (int)(Width.Value);
				int h = this.ImageHeight;

				bmp = new Bitmap(w, h);
				m_chart.TargetSize = bmp.Size;

				g = Graphics.FromImage(bmp);
				m_chart.ChartFrame.Text = Text;

				Frame.FontColor = ForeColor;

                if(m_chart.NeedsDataBinding)
					DataBind();

				m_chart.Render(g);
				if(m_chart.HasErrors)
				{
					throw new Exception(m_chart.ErrorMessage);
				}

				if (this.PostPaint != null)
				{
					this.PostPaint(this, new ChartPaintEventArgs(g, new Rectangle(0, 0, w, h)));
				}
			}
			catch(Exception ex)
			{
				m_chart.RenderErrorMessage(ex,g);
				m_exception = ex;
				if(ChartError != null)
				{
					ChartError(this,new ChartErrorEventArgs(ex,g));
				}
				else if(!DesignMode)
					throw/* new Exception(ex.Message, ex)*/;
			}

            string uniqueID = GetSaneClientSideID();
            string clientID = this.ClientID;            

            if ((SaveImageOnDisk && !DirectImage) || DesignSavePath != String.Empty)
            {
#if FW2
				string virtualDirectory = "/ComponentArt.WebChart.2008.1.fw2.temp/";
#elif FWAJAX
				string virtualDirectory = "/ComponentArt.WebChart.2008.1.fw3.temp/";
#elif FW35
				string virtualDirectory = "/ComponentArt.WebChart.2008.1.fw35.temp/";
#else
                string virtualDirectory = "/ComponentArt.Chart.2008.1.fw1.temp/";
#endif
                //if not set.  It will be set when creating it for highlighted series images

                String basefilename = DesignSavePath != String.Empty ? "CArtDesTimeWebChart" : "CArtWebChart";

                string filename;
                m_attemptedToWriteToFile = "";
                if (ImageOutputDirectory == "")
                {
                    try
                    {
                        filename = SaveImageToDisc(bmp, ".", basefilename, true, imageFileName);
                    }
                    catch (IOException)
                    {
                        try
                        {
                            filename = SaveImageToDisc(bmp, virtualDirectory, basefilename, false, imageFileName);
                        }
                        catch (DirectoryNotFoundException)
                        {
                            throw new IOException("Could not write image onto the file system at '" + m_attemptedToWriteToFile + "'. Please grant write permissions to ASPNET user or perform a full installation of the product.");
                        }
                    }
                }
                else
                {
                    filename = SaveImageToDisc(bmp, ImageOutputDirectory, basefilename, true, imageFileName);
                }
                
                if (!DesignMode)
                {
                    this.WebChartImageInfo = new ChartImageInfo(filename);

                    if (!(this.CacheInterval > 0 && (this.ClientsideApiEnabled || this.RenderAreaMap)
                        && ((this.ClientsideCustomizedImageCachingEnabled && m_customized) || !m_customized)))                        
                        DynamicImage.StoreData(this.WebChartImageInfo, this); 
                }

                DynamicImage.ClientsideHtmlUrl = filename.Substring(0, filename.LastIndexOf('.')) + ".txt";
            }
            else
            {
                if (ms == null)
                    ms = new System.IO.MemoryStream();
                    
                ImageFormat imageFormat = WebChartImageType == ChartImageType.Gif ? ImageFormat.Gif : WebChartImageType == ChartImageType.Jpeg ? ImageFormat.Jpeg : ImageFormat.Png;

                if (WebChartImageType == ChartImageType.Jpeg)
                {
                    ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
                    System.Drawing.Imaging.Encoder myEncoder;
                    myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    System.Drawing.Imaging.EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, JpegQuality);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmp.Save(ms, ici, myEncoderParameters);
                }
                else
                {
                    bmp.Save(ms, imageFormat);
                }
                this.WebChartImageInfo = new ChartImageInfo(ms.ToArray(), WebChartImageType, (this.CacheInterval == 0), imageFileName);
                if (!(this.CacheInterval > 0 && (this.ClientsideApiEnabled || this.RenderAreaMap)
                && ((this.ClientsideCustomizedImageCachingEnabled && m_customized) || !m_customized)))
                    DynamicImage.StoreData(this.WebChartImageInfo, this);
            }

            bmp.Dispose();
        }

        private int ImageHeight
        {
            get
            {
                if (this.IsScrollControl)
                    return ((int)this.Height.Value) - ((int)this.ScrollControllHeight.Value);
                else
                    return (int)this.Height.Value;
            }
        }

        string m_attemptedToWriteToFile = "";

        /// <summary>
        /// Occurs after the control is drawn
        /// </summary>
        public event PaintEventHandler PostPaint;

        /// <summary>
        /// Represents the method that will handle the <see cref="PostPaint"/> event of the <see cref="Chart"/> class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void PaintEventHandler(object sender, ChartPaintEventArgs e);


        internal bool InSerialization 
        {
            get { return m_chart.InSerialization; } 
            set { m_chart.InSerialization = value; } 
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
            if (ViewDirectionChanged != null)
                ViewDirectionChanged(this, e);
        }

        private void setAngles()
        {
            Vector3D v = View.ViewDirection.Unit();


            m_alpha = Math.Atan2(v.Z, v.X);
            m_beta = Math.Asin(v.Y);
        }

        private string GetSaneClientSideID()
        {
            if (this.UniqueID == null || this.UniqueID == "")
            {
                throw new Exception("An ID must be defined on the control.");
            }
            else
            {
                return this.UniqueID.Replace("$", "_").Replace("{", "_").Replace("}", "_").Replace(":", "_");
            }
        }

#if __COMPILING_FOR_2_0_AND_ABOVE__

        protected override void OnDataPropertyChanged()
        {
            base.OnDataPropertyChanged();
        }

        protected override void OnDataSourceViewChanged(object sender, EventArgs e)
        {
            m_chart.DataSource = DataSource;
            base.OnDataSourceViewChanged(sender, e);
        }

        protected override void ValidateDataSource(object dataSource)
        {
            if (dataSource is System.Data.OleDb.OleDbDataAdapter || dataSource is System.Data.SqlClient.SqlDataAdapter)
                return;

            base.ValidateDataSource(dataSource);
        }

        public override object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                base.DataSource = value;
            }
        }

        protected override IDataSource GetDataSource()
        {
            IDataSource ids = base.GetDataSource();
            return ids;
        }

        protected override void PerformDataBinding(IEnumerable data)
        {
            base.PerformDataBinding(data);

            if (data == null 
                && (DataSource is System.Data.OleDb.OleDbDataAdapter
                || DataSource is System.Data.SqlClient.SqlDataAdapter)
                || DataSource is System.Data.DataSet
                )
            {
                m_chart.DataSource = DataSource;
            }
            else
            {
                m_chart.DataSource = data;
            }
			m_chart.TargetSize = new System.Drawing.Size((int)(Width.Value), (int)(Height.Value));
        }
 
#else
        /// <summary>
        ///		The source of data for the chart control.
        /// </summary>
        /// <remarks>
        ///		Data source can be various objects that help extracting data from databases.
        ///		For more on data sources see topic "Data Binding" in "Basic Concepts".
        /// </remarks>
        [Category("ChartBase Contents")]
        [Description("Indicates the source of data for the chart control")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue((string)null)]
        [Bindable(true)]
        [NotifyParentProperty(true)]
        //[TypeConverter(typeof(ComponentArt.Web.Visualization.Charting.Design.DataSourceConverter))]
        //[PersistenceMode(PersistenceMode.Attribute)]
        public object DataSource
        {
            get { return m_chart.DataSource; }
            set
            {
                if (m_chart.DataSource == value)
                    return;
                m_chart.DataSource = value;
            }
        }
#endif

        private void HandleViewDirectionChanged(object sender, System.EventArgs e)
        {
            OnViewDirectionChanged(e);
        }

        string m_backgroundImageUrl = "";

        /// <summary>
        ///		The chart background image.
        /// </summary>
        [Editor(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(UITypeEditor)), Bindable(true), Category("Appearance"), DefaultValue("")]
        public virtual string BackgroundImageUrl
        {
            get
            {
                return m_backgroundImageUrl;
            }
            set
            {
                if (m_backgroundImageUrl == value)
                    return;

                m_backgroundImageUrl = value.Trim();

                if (m_backgroundImageUrl != "") 
                {
                    try
                    {
                        string directory = ConvertUrl(Context, TemplateSourceDirectory, m_backgroundImageUrl);
                        string physicalPath = MapPhysicalPath(directory);
                        System.Drawing.Image img = System.Drawing.Image.FromFile(physicalPath);
                        m_chart.Background = (Bitmap)img;
                        return;
                    }
                    catch (System.IO.FileNotFoundException) { }
                }

                m_chart.Background = null;
            }
        }

        /// <summary>
        /// Renders the chart into specified <see cref="System.Drawing.Graphics"/> object within 
        /// a specified rectangle <see cref="System.Drawing.Rectangle"/>.
        /// </summary>
        /// <param name="g"><see cref="System.Drawing.Graphics"/> object to draw into.</param>
        /// <param name="rect"><see cref="System.Drawing.Rectangle"/> in g to draw into.</param>
        public void Draw(Graphics g, Rectangle rect)
        {
            if (rect.IsEmpty)
                return;
            try
            {
                m_chart.TargetSize = rect.Size;
                m_chart.BackColor = BackColor;
                m_chart.Invalidate();

                Frame.FontColor = ForeColor;

                // If DataBind() hasn't been called yet, we have to call it now.
                // We cannot leave it ot the m_chart because Chart.DataBind() calls the base
                // DataBind() which gets the data source object
                if (m_chart.NeedsDataBinding)
                    DataBind();

                m_chart.Render(g);
                m_chart.Invalidate();
            }
            catch (Exception ex)
            {
				m_chart.RenderErrorMessage(ex,g);
                m_exception = ex;
				if(ChartError != null)
					ChartError(this,new ChartErrorEventArgs(ex,g));
				else
					throw/* new Exception(ex.Message, ex)*/;
            }
        }


        /// <summary>
        /// Draws the chart into a <see cref="Bitmap"/> of the same size ad the chart control.
        /// </summary>
        /// <returns>Newly generated <see cref="Bitmap"/> with the image of the chart.</returns>
        public Bitmap Draw()
        {
            Bitmap bmp = new System.Drawing.Bitmap((int)Width.Value, (int)Height.Value);
            Graphics g = Graphics.FromImage(bmp);

            Draw(g, new System.Drawing.Rectangle(0, 0, (int)Width.Value, (int)Height.Value));
            g.Dispose();
            return bmp;
        }

        [SkipObfuscation]
        internal void ToWizard(ComponentArt.Web.Visualization.Charting.Design.Wizard dest)
        {
            dest.ChartBackColor = BackColor;

            dest.ChartText = (string)Text.Clone();
            dest.ChartForeColor = ForeColor;

            dest.BackgroundImageURL = BackgroundImageUrl;
#if __COMPILING_FOR_2_0_AND_ABOVE__
            dest.DataSourceId = DataSourceID;
#endif
        }

        [SkipObfuscation]
        internal void FromWizard(ComponentArt.Web.Visualization.Charting.Design.Wizard src)
        {
            BackColor = src.ChartBackColor;
            Text = src.ChartText;
            ForeColor = src.ChartForeColor;

            BackgroundImageUrl = src.BackgroundImageURL;
#if __COMPILING_FOR_2_0_AND_ABOVE__
            DataSourceID = src.DataSourceId;
#endif
        }

        /// <summary>
        /// Checks if the chart object supports the specified feature.
        /// </summary>
        /// <param name="featureName">Feature name.</param>
        /// <returns>True if the feature is supported.</returns>
        /// <remarks>
        /// The feature set supported by a chart depends on the geometric engine. There are two features
        /// supported by the HighQualityRendering engine but not 
        /// supported by the HighSpeedRendering engine: "VariablePieHeight" and "PieLift".
        /// </remarks>
        public bool SupportsFeature(string featureName) { return m_chart.GeometricEngineSupports(featureName); }


#region ----- (empty) implementation of IPostBackEventHandler  -----
        public void RaisePostBackEvent(string eventArgument)
        {
            //don't need anything now, we're just doing this so the framework 
            //gives us an postback method that can be called from the client-side
        }
        #endregion


        #region --- implementation of IScrollControl and additional Scrolling functionality ---

        private int m_scrollRangeStartPixel = 150;
        private int m_scrollRangeEndPixel = 220;
        /// <summary>
        /// Sets the initial selected range of the zoom control, as percentages of the entire range.
        /// As such both values must be between 0 and 1.
        /// </summary>
        /// <param name="start">start value of the selected range, as percentage of entire range. Value must be between 0 and 1.</param>
        /// <param name="end">end value of the selected range, as percentage of entire range. Value must be between 0 and 1.</param>
        public void SetZoomRangeInPrecent(double start, double end)
        {
            m_ControlZoomStartPct = start;
            m_ControlZoomEndPct = end;
        }
        private double m_ControlZoomStartPct = 0;
        private double m_ControlZoomEndPct = 0;

        private String getZoomRangeParameters()
        {
            if (m_ControlZoomEndObj != null && m_ControlZoomStartObj != null)
            {
                double minValue = this.CoordinateSystem.XAxis.LCoordinate(this.CoordinateSystem.XAxis.MinValue);
                double maxValue = this.CoordinateSystem.XAxis.LCoordinate(this.CoordinateSystem.XAxis.MaxValue);

                double minX = this.CoordinateSystem.XAxis.LCoordinate(m_ControlZoomStartObj);
                double maxX = this.CoordinateSystem.XAxis.LCoordinate(m_ControlZoomEndObj);

                double total = maxValue - minValue;

                m_ControlZoomStartPct = (minX - minValue) / total;
                m_ControlZoomEndPct = (maxX - minValue) / total;
            }

            if (m_ControlZoomEndPct > 0)
            {
                int xAxisStartPixel = m_chart.XAxisStartPixel;
                int xAxisEndPixel = m_chart.XAxisEndPixel;
                int totalPx = xAxisEndPixel - xAxisStartPixel;
                int leftSize = xAxisStartPixel + (int)Math.Floor(totalPx * m_ControlZoomStartPct);
                int selectSize = xAxisStartPixel + (int)Math.Ceiling(totalPx * m_ControlZoomEndPct);
                int rightSize = (int)this.Width.Value - selectSize;

                //fix the selectSize
                selectSize = selectSize - leftSize - 2 * ControlResizeButtonSizePx;

                return (leftSize + "," + selectSize + "," + rightSize);
            }
            else
                return null;

        }


        /// <summary>
        /// Sets the initial selected range of the zoom control, overriding settings set through
        /// the <code>SetZoomRangeInPrecent</code> method;
        /// Both parameters must be of the same type as CoordinateSystem type
        /// of the X axis of the TargetChart.
        /// </summary>
        /// <param name="start">start value of the selected range</param>
        /// <param name="end">end value of the selected range</param>
        public void SetZoomRange(object start, object end)
        {
            m_ControlZoomStartObj = start;
            m_ControlZoomEndObj = end;

            //set the Min and Max range on the controlled charts to match that of the zoom control
            foreach (Chart wc in controlledCharts)
            {
                wc.CoordinateSystem.XAxis.MinValue = start;
                wc.CoordinateSystem.XAxis.MaxValue = end;
                wc.CoordinateSystem.XAxis.RoundValueRange = false;
            }
        }
        private object m_ControlZoomStartObj = null;
        private object m_ControlZoomEndObj = null;


        //////////////////////// additional scrolling functionality //////////////////////

        /// <summary>
        /// Used only when the chart is a Scroll Controll for another chart.  Value represents the opacity
        /// of the shaded region in the scroll control.  Value must be between 0 and 1.
        /// </summary>
        internal double ScrollShadowOpacity
        {
            get { return m_ScrollShadowOpacity; }
            set { m_ScrollShadowOpacity = value; }
        }
        double m_ScrollShadowOpacity = 0.7;

        /// <summary>
        /// Used only when the chart is a Scroll Controll for another chart.  Value represents the colour of
        /// the shaded region of the Scroll Controll.  The value must be a valid web colour that a browser can interpet.
        /// i.e. <code>white</code> or <code>#ffffff</code>
        /// </summary>
        internal String ScrollShadowColor
        {
            get { return m_ScrollShadowColor; }
            set { m_ScrollShadowColor = value; }
        }
        String m_ScrollShadowColor = "white";

        internal String ScrollImagesDirectoryPath
        {
            get { return m_ScrollImagesDirectoryPath; }
            set
            {
                m_ScrollImagesDirectoryPath = value;
                if (!m_ScrollImagesDirectoryPath.EndsWith("/"))
                    m_ScrollImagesDirectoryPath = m_ScrollImagesDirectoryPath + "/";
            }
        }
        private String m_ScrollImagesDirectoryPath = "scroll_images/";

        internal int ControlResizeButtonSizePx
        {
            get { return m_ControlResizeButtonSizePx; }
            set { m_ControlResizeButtonSizePx = value; }
        }
        private int m_ControlResizeButtonSizePx = 9; 

        /// <summary>
        /// Used only when the chart is a Scroll Controll for another chart. 
        /// Specifies the amount the scroll bar should move when the arrows on the ends are pressed.
        /// The value specifies what percentage of the currently displayed area will move over into
        /// the shaded area.
        /// Default value is 0.5 which means when the button is pressed, half the area will display
        /// new data, while half the area will have been displayed before the zoom.
        /// </summary>
        internal double ScrollStepPercentage
        {
            get { return m_ScrollStepPercentage; }
            set { m_ScrollStepPercentage = value; }
        }
        double m_ScrollStepPercentage = 0.5;

        private Unit m_ScrollControllHeight = new Unit(21);
        
        /// <summary>
        /// The height in pixels of the images used for the scroll bar control
        /// </summary>
        internal Unit ScrollControllHeight
        {
            get { return m_ScrollControllHeight; }
            set { m_ScrollControllHeight = value; }
        }

        //set directly through the ClientsideSettings class (i.e. Clientside property instance)
        internal int RangeClientTemplateXoffset = 0;
        internal int RangeClientTemplateYoffset = 0;
        internal String RangeClientTemplateDateFormat = "MMMM dd, yyyy";

        internal bool AutoRenderOnChange = false;

        #endregion

		#region --- Events ---

		/// <summary>
		/// Occurs when a callback Request is processed.
		/// </summary>
		public event CallbackEventHandler Callback;

		protected virtual void RaiseCallbackEvent()
		{
			if (Callback != null)
				Callback(this, new CallbackEventArgs());
		}

		/// <summary>
		/// Provides data for the <see cref="Chart.Callback"/> event.
		/// </summary>
		public class CallbackEventArgs : EventArgs
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="CallbackEventArgs"/> class.
			/// </summary>
			public CallbackEventArgs() : base() { }
		}

		/// <summary>
		/// Represents the method that will handle the <see cref="Chart.Callback"/> event of a <see cref="Chart"/>.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="eventArgs">The event data.</param>
		public delegate void CallbackEventHandler(object sender, CallbackEventArgs eventArgs);

		#endregion

    }

    /// <summary>
    /// Specifies the format of the output image.
    /// </summary>
    public enum ChartImageType
    {
        /// <summary>
        /// Graphics Interchange Format (GIF) image format.
        /// </summary>
        Gif,
        /// <summary>
        /// Joint Photographic Experts Group (JPEG) image format.
        /// </summary>
        Jpeg,
        /// <summary>
        /// W3C Portable Network Graphics (PNG) image format.
        /// </summary>
        Png
    }

    [Serializable]
    internal class ChartImageInfo
    {
        byte[] m_imageBytes;
        ChartImageType m_webChartImageType;
        string m_fileName;
        string m_clientsideHtmlFileName;
        string m_clientsideCallbackHtml;
        string m_clientsidePostbackHtml;
        bool m_removeOnAccess = false;


        public ChartImageInfo(string fileName)
        {
            m_fileName = fileName;
            //m_removeOnAccess = removeOnAccess;
        }

        public ChartImageInfo(byte[] imageBytes, ChartImageType webChartImageType, bool removeOnAccess)
        : this (imageBytes, webChartImageType, removeOnAccess, null) {}

        public ChartImageInfo(byte[] imageBytes, ChartImageType webChartImageType, bool removeOnAccess, string fileName)
        {
            m_imageBytes = imageBytes;
            m_webChartImageType = webChartImageType;
            m_removeOnAccess = removeOnAccess;

            if (FileName != null || !"".Equals(fileName))
                m_fileName = fileName;
        }

        public bool RemoveOnAccess
        {
            get { return m_removeOnAccess; }
        }

        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }

        public byte[] ImageBytes
        {
            get { return m_imageBytes; }
            set { m_imageBytes = value; }
        }

        public ChartImageType WebChartImageType
        {
            get { return m_webChartImageType; }
            set { m_webChartImageType = value; }
        }

        public string ClientsideHtmlFileName
        {
            get { return m_clientsideHtmlFileName; }
            set { m_clientsideHtmlFileName = value; }
        }
            
        public string ClientsideCallbackHtml
        {
            get { return m_clientsideCallbackHtml; }
            set { m_clientsideCallbackHtml = value; }
        }

        public string ClientsidePostbackHtml
        {
            get { return m_clientsidePostbackHtml; }
            set { m_clientsidePostbackHtml = value; }
        }


    }

    /// <summary>
    /// Provides data for painting over a Chart.
    /// </summary>
    public class ChartPaintEventArgs : EventArgs
    {
        internal ChartPaintEventArgs(Graphics g, Rectangle clipRectangle)
        {
            m_graphics = g;
            m_clipRectangle = clipRectangle;
        }

        Rectangle m_clipRectangle;
        Graphics m_graphics;

        /// <summary>
        /// Gets the <see cref="Graphics"/> object with which painting should be done.
        /// </summary>
        public Graphics Graphics
        {
            get
            {
                return m_graphics;
            }
        }

        /// <summary>
        /// Gets the rectangle that indicates the area in which the painting should be done.
        /// </summary>
        public Rectangle ClipRectangle
        {
            get
            {
                return m_clipRectangle;
            }
        }
    }

	#region --- Types for Error Handling ---

	/// <summary>
	/// Provides information on excepcion thrown by Chart
	/// </summary>
	public class ChartErrorEventArgs : EventArgs
	{
		private Exception ex;
		private Graphics g;
		internal ChartErrorEventArgs(Exception ex, Graphics g)
		{
			this.ex = ex;
			this.g = g;
		}

		public Exception Exception { get { return ex; } }
		public Graphics Graphics { get { return g; } }
	}

	public delegate void ChartErrorEventHandler(object sender, ChartErrorEventArgs e);

	#endregion
     
#if __BuildingWebChart__
	internal sealed class SkipObfuscationAttribute : Attribute
    {
    }
#endif
}


