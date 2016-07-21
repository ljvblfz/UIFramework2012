using System;
using System.Web.UI.WebControls;


namespace ComponentArt.Web.Visualization.Charting
{
    /// <summary>
    /// Wrapper class that contains all of Chart's client-side methods and properties.
    /// </summary>
    public class ClientsideSettings
    {
        private Chart m_webchart;

        internal ClientsideSettings(Chart wc)
        {
            this.m_webchart = wc;
        }


        /// <summary>
        /// Allows the view range of this chart to be controlled by another control for the
        /// purposes of zooming and scrolling.
        /// /// </summary>
        public IScrollControl MyScrollControl
        {
            get
            {
                return m_webchart.MyScrollControl;
            }
            set
            {
                m_webchart.MyScrollControl = value;
            }
        }


        /// <summary>
        ///		When set to true, loads and initializes the JavaScript clientside API 
        ///     for client browser use when the chart is rendered.
        /// </summary>
        public bool ClientsideApiEnabled
        {
            get
            {
                return m_webchart.ClientsideApiEnabled;
            }
            set
            {
                m_webchart.ClientsideApiEnabled = value;
            }
        }

        /// <summary>
        ///		When set to true, loads and initializes the JavaScript clientside Chart
        ///     method that renders HTML for View Angle Chooser control.
        /// </summary>
        public bool ViewAngleChooserEnabled
        {
            get
            {
                return m_webchart.ViewAngleChooserEnabled;
            }
            set
            {
                m_webchart.ViewAngleChooserEnabled = value;
            }
        }

        /// <summary>
        ///		When set to true, chart renderings produced by clientside customizations
        ///     are cached.  The expiry and rerender rules are the same as for the uncustomized
        ///     chart image.  If a custom image is required for the same customizations which
        ///     has a cached image already, the cached image is used.
        /// </summary>
        public bool AutoRenderOnChange
        {
            get
            {
                return m_webchart.AutoRenderOnChange;
            }
            set
            {
                m_webchart.AutoRenderOnChange = value;
            }
        }

        ///// <summary>
        /////		When set to true, the rendering engine will create an image for each series in which
        /////     that series will be highlighted relative to other series.  The newly created images will
        /////     be tied into the client-side API for easy access, hence ClientsideApiEnabled has to be set
        /////     to true for this functionality to be usefull.
        ///// </summary>
        //public bool SeriesHighlightingEnabled
        //{
        //    get
        //    {
        //        return m_webchart.SeriesHighlightingEnabled;
        //    }
        //    set
        //    {
        //        m_webchart.SeriesHighlightingEnabled = value;
        //    }
        //}


        /// <summary>
        ///		Used only when the client-side API is enabled.
        ///     When set to true, AutoCallbackOnRefresh is set to false and calling refresh() method
        ///     in the client-side API automatically refreshes the page through a postback.
        /// </summary>
        public ClientsideRefreshMethod RefreshMethod
        {
            get
            {
                return m_webchart.RefreshMethod;
            }
            set
            {
                m_webchart.RefreshMethod = value;
            }
        }


        /// <summary>
        ///		When set to true, the rendering engine will create an image for each series in which
        ///     that series will be highlighted relative to other series.  The newly created images will
        ///     be tied into the client-side API for easy access, hence ClientsideApiEnabled has to be set
        ///     to true for this functionality to be usefull.
        /// </summary>
        public bool IsScrollControll
        {
            get
            {
                return m_webchart.IsScrollControl;
            }
            set
            {
                m_webchart.IsScrollControl = value;
            }
        }

        /// <summary>
        /// When set to true, the values of the following DataPoint properties will be available on the client-side
        /// through the client-side ComponentArt.Web.Visualization.Charting.DataPoint class: x, y, missing, from, to, open, close, high, low
        /// </summary>
        public bool SerializeDatapoints
        {
            get { return m_webchart.SerializeDatapoints; }
            set { m_webchart.SerializeDatapoints = value; }
        }

        /// <summary>
        ///		When set to true, chart renderings produced by clientside customizations
        ///     are cached.  The expiry and rerender rules are the same as for the uncustomized
        ///     chart image.  If a custom image is required for the same customizations which
        ///     has a cached image already, the cached image is used.
        /// </summary>
        public bool ClientsideCustomizedImageCachingEnabled
        {
            get
            {
                return m_webchart.ClientsideCustomizedImageCachingEnabled;
            }
            set
            {
                m_webchart.ClientsideCustomizedImageCachingEnabled = value;
            }
        }


        #region --- implementation of IScrollControl and additional Scrolling functionality ---

        /// <summary>
        /// Sets the initial selected range of the zoom control, as percentages of the entire range.
        /// As such both values must be between 0 and 1.
        /// </summary>
        /// <param name="start">start value of the selected range, as percentage of entire range. Value must be between 0 and 1.</param>
        /// <param name="end">end value of the selected range, as percentage of entire range. Value must be between 0 and 1.</param>
        public void SetZoomRangeInPrecent(double start, double end)
        {
            m_webchart.SetZoomRangeInPrecent(start, end);
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
            m_webchart.SetZoomRange(start, end);
        }


        //////////////////////// additional scrolling functionality //////////////////////

        /// <summary>
        /// Used only when the chart is a Scroll Controll for another chart.  Value represents the opacity
        /// of the shaded region in the scroll control.  Value must be between 0 and 1.
        /// </summary>
        public double ScrollShadowOpacity
        {
            get { return m_webchart.ScrollShadowOpacity; }
            set { m_webchart.ScrollShadowOpacity = value; }
        }


        /// <summary>
        /// Used only when the chart is a Scroll Controll for another chart.  Value represents the colour of
        /// the shaded region of the Scroll Controll.  The value must be a valid web colour that a browser can interpet.
        /// i.e. <code>white</code> or <code>#ffffff</code>
        /// </summary>
        public String ScrollShadowColor
        {
            get { return m_webchart.ScrollShadowColor; }
            set { m_webchart.ScrollShadowColor = value; }
        }


        /// <summary>
        /// An image that displays while a client-side modified chart is being refreshed via an AJAX callback.
        /// </summary>
        public String LoadingChartImagePath
        {
            get { return m_webchart.m_LoadingChartImagePath; }
            set 
            { 
                if (value.StartsWith("/") || value.StartsWith("\\"))
                    value = value.Substring(1);
                m_webchart.m_LoadingChartImagePath = value; 
            }
        }

        public String ScrollImagesDirectoryPath
        {
            get { return m_webchart.ScrollImagesDirectoryPath;  }
            set { m_webchart.ScrollImagesDirectoryPath = value; }
        }

        public int ControlResizeButtonWidthPx
        {
            get { return m_webchart.ControlResizeButtonSizePx; }
            set { m_webchart.ControlResizeButtonSizePx = value; }
        }


        /// <summary>
        /// Used only when the chart is a Scroll Controll for another chart. 
        /// Specifies the amount the scroll bar should move when the arrows on the ends are pressed.
        /// The value specifies what percentage of the currently displayed area will move over into
        /// the shaded area.
        /// Default value is 0.5 which means when the button is pressed, half the area will display
        /// new data, while half the area will have been displayed before the zoom.
        /// </summary>
        public double ScrollStepPercentage
        {
            get { return m_webchart.ScrollStepPercentage; }
            set { m_webchart.ScrollStepPercentage = value; }
        }


        /// <summary>
        /// A string representing the DateTime format, in standard notation (seconds not supported). Default value is "MMMM dd, yyyy".
        /// </summary>
        public String RangeClientTemplateDateFormat
        {
            get { return m_webchart.RangeClientTemplateDateFormat; }
            set { m_webchart.RangeClientTemplateDateFormat = value; }
        }

        /// <summary>
        /// The height in pixels of the images used for the scroll bar control
        /// </summary>
        public Unit ScrollControllHeight
        {
            get { return m_webchart.ScrollControllHeight; }
            set { m_webchart.ScrollControllHeight = value; }
        }


        /// <summary>
        /// The number of pixels to move the Range client template from its original position in the X direction 
        /// </summary>
        public int RangeClientTemplateXoffset
        {
            get { return m_webchart.RangeClientTemplateXoffset; }
            set { m_webchart.RangeClientTemplateXoffset = value; }
        }

        /// <summary>
        /// The number of pixels to move the Range client template from its original position in the Y direction 
        /// </summary>
        public int RangeClientTemplateYoffset
        {
            get { return m_webchart.RangeClientTemplateYoffset; }
            set { m_webchart.RangeClientTemplateYoffset = value; }
        }

        #endregion

    }

    public enum ClientsideRefreshMethod { Callback, Postback }
}
