using System;
using System.Collections.Specialized;
using System.Text;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Runtime.InteropServices; 


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Divides its contents into a number of sections that the user can resize, expand and collapse.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Splitter provides the ability to easily define layouts consisting of different pane configurations. Once defined,
  /// panes can be resized, collapsed and expanded.
  /// </para>
  /// <para>
  /// Layouts are defined in the <see cref="Layouts" /> collection. A <see cref="SplitterLayout" /> consists of a <see cref="SplitterPaneGroup" /> which in turn contains a number of
  /// <see cref="SplitterPane" /> objects. Each SplitterPane defines styles for that particular pane, and references a <see cref="SplitterPaneContent" /> object contained in Splitter's
  /// <see cref="Content" /> collection. SplitterPaneContent wraps any ASP.NET content that is to be used in a SplitterPane. Each SplitterPane also contains a <see cref="SplitterPane.Panes" /> collection,
  /// making it possible to nest SplitterPaneGroups inside SplitterPanes.
  /// </para>
  /// <para>
  /// With most types of content, the <see cref="SplitterPane.ClientSideOnResize" /> event should be used to set the correct size of DOM
  /// elements on the client when a pane is resized. This is usually necessary in order to guarantee the proper functioning of Splitter
  /// resizing.
  /// </para>
  /// </remarks>
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [Designer(typeof(ComponentArt.Web.UI.SplitterDesigner))]
  [ParseChildren(true)]
  [ToolboxData("<{0}:Splitter runat=\"server\"></{0}:Splitter>")]
  public sealed class Splitter : WebControl
	{
    #region Private Properties

    /// <summary>
    /// A list of images to preload.
    /// </summary>
    private StringCollection _preloadImages;
    private StringCollection PreloadImages
    {
      get
      {
        if(_preloadImages == null)
        {
          _preloadImages = new StringCollection();
        }

        return _preloadImages;
      }
    }

    #endregion

    #region Public Properties

    /// <summary>
    /// Comma-delimited list of client-side conditions to check for before initializing.
    /// </summary>
    [DefaultValue("")]
    public string ClientDependencies
    {
      get
      {
        object o = ViewState["ClientDependencies"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ClientDependencies"] = value;
      }
    }

    private SplitterClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public SplitterClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new SplitterClientEvents();
        }
        return _clientEvents;
      }
    }

    /// <summary>
    /// Client-side handler for the collapse event. 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public string ClientSideOnCollapse
    {
      get
      {
        object o = ViewState["ClientSideOnCollapse"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ClientSideOnCollapse"] = value;
      }
    }

    /// <summary>
    /// Client-side handler for the expand event. 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public string ClientSideOnExpand
    {
      get
      {
        object o = ViewState["ClientSideOnExpand"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ClientSideOnExpand"] = value;
      }
    }

    /// <summary>
    /// Client-side handler for the load event. 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public string ClientSideOnLoad
    {
      get
      {
        object o = ViewState["ClientSideOnLoad"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ClientSideOnLoad"] = value;
      }
    }

    /// <summary>
    /// Client-side handler for the resize end event. 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public string ClientSideOnResizeEnd
    {
      get
      {
        object o = ViewState["ClientSideOnResizeEnd"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ClientSideOnResizeEnd"] = value;
      }
    }
    
    /// <summary>
    /// Client-side handler for the resize start event. 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public string ClientSideOnResizeStart
    {
      get
      {
        object o = ViewState["ClientSideOnResizeStart"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["ClientSideOnResizeStart"] = value;
      }
    }
    
    /// <summary>
    /// Creates a new collection of child controls for the current control.
    /// </summary>
    /// <returns>A PageViewCollection object that contains the currents control's children.</returns>
    protected override ControlCollection CreateControlCollection()
    {
      return new SplitterPaneContentCollection(this);
    }
    
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public SplitterPaneContentCollection Content
    {
      get
      {
        return (SplitterPaneContentCollection)this.Controls;
      }
    }
    
    /// <summary>
    /// The currently active SplitterLayout.
    /// </summary>
    /// <seealso cref="SplitterLayout" />
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public SplitterLayout CurrentLayout
    {
      get
      {
        if(this.LayoutId != "")
        {
          return this.FindLayoutById(this.LayoutId);
        }
        else if(this.Layouts.Count > 0)
        {
          return Layouts[0];
        }

        return null;
      }
    }

    /// <summary>
    /// Whether to fill all the available space in the containing DOM element.
    /// </summary>
    [DefaultValue(false)]
    public bool FillContainer
    {
      get
      {
        object o = ViewState["FillContainer"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["FillContainer"] = value;
      }
    }

    /// <summary>
    /// Whether to fill all the available vertical space on the screen.
    /// </summary>
    [DefaultValue(false)]
    public bool FillHeight
    {
      get
      {
        object o = ViewState["FillHeight"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["FillHeight"] = value;
      }
    }

    /// <summary>
    /// Whether to fill all the available horizontal space on the screen.
    /// </summary>
    [DefaultValue(false)]
    public bool FillWidth
    {
      get
      {
        object o = ViewState["FillWidth"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["FillWidth"] = value;
      }
    }

    /// <summary>
    /// The number of pixels to subtract when FillHeight is set to true.
    /// </summary>
    /// <seealso cref="FillHeight" />
    [DefaultValue(0)]
    public int HeightAdjustment
    {
      get
      {
        object o = ViewState["HeightAdjustment"];
        return o == null ? 0 : (int)o;
      }
      set
      {
        ViewState["HeightAdjustment"] = value;
      }
    }

    /// <summary>
    /// Prefix to use for all image URL paths.
    /// </summary>
    [Description("Used as a prefix for all image URLs. ")]
    [DefaultValue("")]
    public string ImagesBaseUrl
    {
      get
      {
        object o = ViewState["ImagesBaseUrl"]; 
        return (o == null) ? String.Empty : (string)o; 
      }

      set
      {
        ViewState["ImagesBaseUrl"] = value; 
      }
    }

    private SplitterLayoutCollection _layouts;
    /// <summary>
    /// The collection of available layouts.
    /// </summary>
    /// <seealso cref="SplitterLayout" />
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public SplitterLayoutCollection Layouts
    {
      get
      {
        if(_layouts == null)
        {
          _layouts = new SplitterLayoutCollection();
        }

        return _layouts;
      }
    }

    /// <summary>
    /// The ID of the layout to use.
    /// </summary>
    [DefaultValue("")]
    public string LayoutId
    {
      get
      {
        object o = ViewState["LayoutId"];
        return o == null ? "" : (string)o;
      }
      set
      {
        ViewState["LayoutId"] = value;
      }
    }

    /// <summary>
    /// Whether a pane resize affects the adjacent pane as well.
    /// </summary>
    [DefaultValue(false)]
    public bool ResizeAdjacentPane
    {
      get
      {
        object o = ViewState["ResizeAdjacentPane"];
        return o == null ? false : (bool)o;
      }
      set
      {
        ViewState["ResizeAdjacentPane"] = value;
      }
    }

    /// <summary>
    /// The number of pixels to subtract when FillWidth is set to true.
    /// </summary>
    /// <seealso cref="FillWidth" />
    [DefaultValue(0)]
    public int WidthAdjustment
    {
      get
      {
        object o = ViewState["WidthAdjustment"];
        return o == null ? 0 : (int)o;
      }
      set
      {
        ViewState["WidthAdjustment"] = value;
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Find the SplitterPane with the given ID.
    /// </summary>
    /// <param name="oLayout">The SplitterLayout to search in</param>
    /// <param name="sId">The ID to search by</param>
    /// <returns>The matching SplitterPane</returns>
    public SplitterPane FindPaneById(SplitterLayout oLayout, string sId)
    {
      if(oLayout != null && oLayout.Panes != null)
      {
        return FindPaneById(this.GetSaneId(), sId, oLayout.Panes);
      }

      return null;
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Overridden. Filters out all objects except SplitterLayout and SplitterPaneContent objects.
    /// </summary>
    /// <param name="obj">The parsed element.</param>
    protected override void AddParsedSubObject(object obj)
    {
      if (obj is SplitterLayout)
      {
        base.AddParsedSubObject(obj);

        this.Layouts.Add((SplitterLayout)obj);
      }
      else if(obj is SplitterPaneContent)
      {
        base.AddParsedSubObject(obj);
      } 
    }

    protected override void ComponentArtPreRender(EventArgs args)
    {
      base.ComponentArtPreRender(args);

      if(!this.IsDownLevel())
      {
        // Tack this to the bottom of the page, to know when we've loaded.
        RegisterStartupScript("ComponentArt_Page_Loaded", this.DemarcateClientScript("window.ComponentArt_Page_Loaded = true;"));
      }
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      if(Layouts.Count == 0)
      {
        return;
      }

      bool bDownLevel = this.IsDownLevel();

      if(Page != null && !bDownLevel)
      {
        // do we need to render scripts for non-Atlas?
        ScriptManager oScriptManager = ScriptManager.GetCurrent(Page);
        if (oScriptManager == null)
        {
          // Add core code
          if (!Page.IsClientScriptBlockRegistered("A573G988.js"))
          {
            Page.RegisterClientScriptBlock("A573G988.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573G988.js");
          }
          if (!Page.IsClientScriptBlockRegistered("A573J482.js"))
          {
            Page.RegisterClientScriptBlock("A573J482.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Splitter.client_scripts", "A573J482.js");
          }
        }
      }

      // override writer
      output = new HtmlTextWriter(output, string.Empty);

      string sSplitterId = this.GetSaneId();

      // Render resize list hidden field
      output.AddAttribute("id", sSplitterId + "_ResizeList");
      output.AddAttribute("name", sSplitterId + "_ResizeList");
      output.AddAttribute("value", string.Empty);
      output.AddAttribute("type", "hidden");
      output.RenderBeginTag(HtmlTextWriterTag.Input);
      output.RenderEndTag();

      // Render collapse change list hidden field
      output.AddAttribute("id", sSplitterId + "_CollapseChangeList");
      output.AddAttribute("name", sSplitterId + "_CollapseChangeList");
      output.AddAttribute("value", string.Empty);
      output.AddAttribute("type", "hidden");
      output.RenderBeginTag(HtmlTextWriterTag.Input);
      output.RenderEndTag();

      if (CurrentLayout == null)
      {
        throw new Exception("No valid layout found.");
      }
      else if(!CurrentLayout.Fixed)
      {
        // make sure all panes have IDs before rendering
        AssignPaneIDs(this.CurrentLayout.Panes, null);
      }

      // pre-load images
      if(!bDownLevel)
      {
        this.BuildImageList();
      
        // Preload images, if any
        if(this.PreloadImages.Count > 0)
        {
          this.RenderPreloadImages(output);
        }
      }

      // Render the content
      AddAttributesToRender(output);

      if(!bDownLevel)
      {
        output.AddStyleAttribute("visibility", "hidden");
        //output.AddStyleAttribute("display", "none");
      }
      
      CurrentLayout.Panes.RenderControl(output);

      if(!bDownLevel)
      {
        // Render client-side object initiation.
        StringBuilder oStartupSB = new StringBuilder();
        oStartupSB.Append("/*** ComponentArt.Web.UI.Splitter ").Append(this.VersionString()).Append(" ").Append(sSplitterId).Append(" ***/\n");
        oStartupSB.Append("function ComponentArt_Init_" + sSplitterId + "() {\n");

        // Include check for whether everything we need is loaded,
        // and a retry after a delay in case it isn't.
        oStartupSB.Append("if(!window.ComponentArt_Splitter_Loaded || !window.ComponentArt_Page_Loaded)\n");
        oStartupSB.Append("\t{setTimeout('ComponentArt_Init_" + sSplitterId + "()', 50); return; }\n\n");

        // Instantiate object
        oStartupSB.Append("window." + sSplitterId + " = new ComponentArt_Splitter('" + sSplitterId + "');\n");

        // Write postback function reference
        if (Page != null)
        {
          oStartupSB.Append(sSplitterId + ".Postback = function() { " + Page.GetPostBackEventReference(this) + " };\n");
        }

        // Hook the actual ID if available and different from effective client ID
        if (this.ID != sSplitterId)
        {
          oStartupSB.Append("if(!window['" + ID + "']) { window['" + ID + "'] = window." + sSplitterId + "; " + sSplitterId + ".GlobalAlias = '" + ID + "'; }\n");
        }

        oStartupSB.Append(GeneratePaneDefinitions(CurrentLayout.Panes, sSplitterId));

        if (ClientDependencies != "") oStartupSB.Append(sSplitterId + ".ClientDependencies = '" + ClientDependencies + "';\n");
        oStartupSB.Append(sSplitterId + ".ClientEvents = " + Utils.ConvertClientEventsToJsObject(this._clientEvents) + ";\n");
        if (ClientSideOnCollapse != "") oStartupSB.Append(sSplitterId + ".ClientSideOnCollapse = " + ClientSideOnCollapse + ";\n");
        if(ClientSideOnExpand != "") oStartupSB.Append(sSplitterId + ".ClientSideOnExpand = " + ClientSideOnExpand + ";\n");
        if (ClientSideOnLoad != "") oStartupSB.Append(sSplitterId + ".ClientSideOnLoad = " + ClientSideOnLoad + ";\n");
        if(ClientSideOnResizeEnd != "") oStartupSB.Append(sSplitterId + ".ClientSideOnResizeEnd = " + ClientSideOnResizeEnd + ";\n");
        if(ClientSideOnResizeStart != "") oStartupSB.Append(sSplitterId + ".ClientSideOnResizeStart = " + ClientSideOnResizeStart + ";\n");
        oStartupSB.Append(sSplitterId + ".ControlId = '" + this.UniqueID + "';\n");
        if(FillContainer)
        {
          oStartupSB.Append(sSplitterId + ".FillContainer = 1;\n");
        }
        else
        {
          if(FillHeight)
          {
            oStartupSB.Append(sSplitterId + ".FillHeight = 1;\n");
            oStartupSB.Append(sSplitterId + ".HeightAdjustment = " + HeightAdjustment + ";\n");
          }
          if(FillWidth)
          {
            oStartupSB.Append(sSplitterId + ".FillWidth = 1;\n");
            oStartupSB.Append(sSplitterId + ".WidthAdjustment = " + WidthAdjustment + ";\n");
          }
        }
        oStartupSB.Append(sSplitterId + ".Orientation = " + (CurrentLayout.Panes.Orientation == SplitterOrientation.Horizontal? 1 : 0) + ";\n");
        if(ResizeAdjacentPane) oStartupSB.Append(sSplitterId + ".ResizeAdjacentPane = 1;\n");

        oStartupSB.Append(sSplitterId + ".Initialize();\n");

        oStartupSB.Append("\nwindow." + sSplitterId + "_loaded = true;\n}\n");

        // window event handlers
        if (this.FillHeight || this.FillWidth || this.FillContainer ||
          Height.Type == UnitType.Percentage || Width.Type == UnitType.Percentage)
        {
          oStartupSB.Append("window.ComponentArt_" + sSplitterId + "_ResizeHandler = function() { " + sSplitterId + ".HandleWindowResize(); };\n");
        }

        // Initiate MultiPage creation
        oStartupSB.Append("ComponentArt_Init_" + sSplitterId + "();");
      
        WriteStartupScript(output, this.DemarcateClientScript(oStartupSB.ToString()));
      }
    }

    protected override bool IsDownLevel()
    {
      if(this.ClientTarget != ClientTargetLevel.Auto)
      {
        return this.ClientTarget == ClientTargetLevel.Downlevel;
      }
      
      if(Context == null)
      {
        return true;
      }

      string sUserAgent = Context.Request.UserAgent;

      if (sUserAgent == null || sUserAgent == "") return true;

      int iMajorVersion = 0;
      
      try
      {
        iMajorVersion = Context.Request.Browser.MajorVersion;
      }
      catch {}

      if( // We are good if:

        // 0. We have the W3C Validator
        (sUserAgent.IndexOf("Validator") >= 0) ||

        // 1. We have IE 5 or greater on a non-Mac
        (sUserAgent.IndexOf("MSIE") >= 0 && iMajorVersion >= 5) ||

        // 2. We have Gecko-based browser (Netscape 6+, Mozilla, FireFox)
        (sUserAgent.IndexOf("Gecko") >= 0) ||

        // 3. We have Opera 7 or later
        (sUserAgent.IndexOf("Opera") >= 0 && iMajorVersion >= 7) ||

        // 4. We have Safari
        (sUserAgent.IndexOf("Safari") >= 0) ||

        // 5. We have Konqueror
        (sUserAgent.IndexOf("Konqueror") >= 0)
        )
      {
        return false;
      }
      else
      {
        return true;
      }
    }

    protected override void LoadViewState(object state)
    {
      base.LoadViewState(state);
      
      // Load level data
      if(ViewState["Layouts"] != null)
      {
        this.Layouts.LoadXml((string)ViewState["Layouts"]);
      }
    }

    protected override void OnLoad(EventArgs oArgs)
    {
      base.OnLoad(oArgs);

      if(Context != null && Page != null && Context.Request != null)
      {
        if(Page.IsPostBack)
        {
          string sSplitterId = this.GetSaneId();
          
          // Process resize list.
          string sResizeList = Context.Request.Form[sSplitterId + "_ResizeList"];
          if(sResizeList != null)
          {
            this.ProcessResizeList(sResizeList);
          }

          // Process collapse change list.
          string sCollapseChangeList = Context.Request.Form[sSplitterId + "_CollapseChangeList"];
          if(sCollapseChangeList != null)
          {
            this.ProcessCollapseChangeList(sCollapseChangeList);
          }
        }
      }

      // make sure all panes have IDs
      if(CurrentLayout != null && !CurrentLayout.Fixed)
      {
        AssignPaneIDs(this.CurrentLayout.Panes, null);
        CurrentLayout.Fixed = true;
      }

      if (!this.IsDownLevel())
      {
        if (ScriptManager.GetCurrent(Page) != null)
        {
          this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
          this.RegisterScriptForAtlas("ComponentArt.Web.UI.Splitter.client_scripts.A573J482.js");
        }
      }
    }

    protected override object SaveViewState()
    {
      if(EnableViewState && Layouts != null)
      {
        ViewState["Layouts"] = Layouts.GetXml();
      }

      return base.SaveViewState();     
    }

    #endregion

    #region Private Methods

    private void AssignPaneIDs(SplitterPaneGroup group, string parentID)
    {
      string sControlId = this.GetSaneId();

      // set parentid
      group.ParentSplitter = this;

      for(int i = 0; i < group.Count; i++)
      {
        SplitterPane oPane = group[i];

        oPane.ParentGroup = group;

        string sId = parentID == null? i.ToString() : parentID + "_" + i.ToString();

        if(oPane.ID == null || oPane.ID == "")
        {
          oPane.ID = sControlId + "_pane_" + sId;
        }
        else if (!oPane.ID.StartsWith(sControlId + "_pane_"))
        {
          oPane.ID = sControlId + "_pane_" + oPane.ID;
        }

        if(oPane.Panes != null && oPane.Panes.Count > 0)
        {
          AssignPaneIDs(oPane.Panes, sId);
        }
      }
    }

    /// <summary>
    /// Put together the list of images that need to be pre-loaded.
    /// </summary>
    private void BuildImageList()
    {
      this.PreloadImages.Clear();

      this.BuildImageList(this.CurrentLayout.Panes);
    }

    // preload individual set images
    private void BuildImageList(SplitterPaneGroup oGroup)
    {
      if(oGroup.SplitterBarCollapseHoverImageUrl != string.Empty)
      {
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.ImagesBaseUrl, oGroup.SplitterBarCollapseHoverImageUrl));
      }
      if(oGroup.SplitterBarCollapseImageUrl != string.Empty)
      {
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.ImagesBaseUrl, oGroup.SplitterBarCollapseImageUrl));
      }
      if(oGroup.SplitterBarExpandHoverImageUrl != string.Empty)
      {
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.ImagesBaseUrl, oGroup.SplitterBarExpandHoverImageUrl));
      }
      if(oGroup.SplitterBarExpandImageUrl != string.Empty)
      {
        this.PreloadImages.Add(Utils.ConvertUrl(Context, this.ImagesBaseUrl, oGroup.SplitterBarExpandImageUrl));
      }

      foreach(SplitterPane oPane in oGroup.Panes)
      {
        if(oPane.Panes != null && oPane.Panes.Count > 0)
        {
          this.BuildImageList(oPane.Panes);
        }
      }
    }

    /// <summary>
    /// Returns the SplitterLayout with the given ID.
    /// </summary>
    /// <param name="sId">The ID of the SplitterLayout to look for.</param>
    /// <returns>The SplitterLayout with the given ID.</returns>
    public SplitterLayout FindLayoutById(string sId)
    {
      foreach(SplitterLayout oLayout in Layouts)
      {
        if(oLayout.ID == sId)
        {
          return oLayout;
        }
      }

      return null;
    }

    private SplitterPane FindPaneById(string sControlId, string sId, SplitterPaneGroup group)
    {
      foreach(SplitterPane pane in group.Panes)
      {
        if(pane.ID == sId || pane.ID == sControlId + "_pane_" + sId)
        {
          return pane;
        }

        if(pane.Panes != null && pane.Panes.Count > 0)
        {
          SplitterPane subPane = FindPaneById(sControlId, sId, pane.Panes);

          if(subPane != null)
          {
            return subPane;
          }
        }
      }

      return null;
    }

    /// <summary>
    /// Returns the SplitterPaneContent object with the given ID.
    /// </summary>
    /// <param name="sId">The ID to look for.</param>
    /// <returns>The SplitterPaneContent with the given ID.</returns>
    public SplitterPaneContent FindPaneContentById(string sId)
    {
      foreach(SplitterPaneContent oContent in Content)
      {
        if(oContent.ID == sId)
        {
          return oContent;
        }
      }

      return null;
    }

    private string GeneratePaneDefinitions(SplitterPaneGroup group, string sPrefix)
    {
      StringBuilder oSB = new StringBuilder();

      // group properties
      if(group.LiveResize) oSB.Append(sPrefix + ".Panes.LiveResize = 1;\n");
      if(group.Orientation == SplitterOrientation.Horizontal) oSB.Append(sPrefix + ".Panes.Orientation = 1;\n");
      oSB.Append(sPrefix + ".Panes.SplitterBarWidth = " + group.SplitterBarWidth + ";\n");
      if(group.ResizeStep > 0) oSB.Append(sPrefix + ".Panes.ResizeStep = " + group.ResizeStep + ";\n");
      if(group.SplitterBarActiveCssClass != "") oSB.Append(sPrefix + ".Panes.SplitterBarActiveCssClass = '" + group.SplitterBarActiveCssClass + "';\n");
      if(group.SplitterBarCssClass != "") oSB.Append(sPrefix + ".Panes.SplitterBarCssClass = '" + group.SplitterBarCssClass + "';\n");
      if(group.SplitterBarCollapsedCssClass != "") oSB.Append(sPrefix + ".Panes.SplitterBarCollapsedCssClass = '" + group.SplitterBarCollapsedCssClass + "';\n");
      if(group.SplitterBarCollapseHoverImageUrl != "") oSB.Append(sPrefix + ".Panes.SplitterBarCollapseHoverImageUrl = '" + Utils.ConvertUrl(Context, ImagesBaseUrl, group.SplitterBarCollapseHoverImageUrl) + "';\n");
      if(group.SplitterBarCollapseImageUrl != "") oSB.Append(sPrefix + ".Panes.SplitterBarCollapseImageUrl = '" + Utils.ConvertUrl(Context, ImagesBaseUrl, group.SplitterBarCollapseImageUrl) + "';\n");
      if(group.SplitterBarExpandImageUrl != "") oSB.Append(sPrefix + ".Panes.SplitterBarExpandImageUrl = '" + Utils.ConvertUrl(Context, ImagesBaseUrl, group.SplitterBarExpandImageUrl) + "';\n");
      if(group.SplitterBarExpandHoverImageUrl != "") oSB.Append(sPrefix + ".Panes.SplitterBarExpandHoverImageUrl = '" + Utils.ConvertUrl(Context, ImagesBaseUrl, group.SplitterBarExpandHoverImageUrl) + "';\n");
      if(group.SplitterBarHoverCssClass != "") oSB.Append(sPrefix + ".Panes.SplitterBarHoverCssClass = '" + group.SplitterBarHoverCssClass + "';\n");
      
      int iCount = 0;
      foreach(SplitterPane pane in group.Panes)
      {
        oSB.Append(sPrefix + ".Panes[" + iCount + "] = new ComponentArt_SplitterPane('" + pane.ID + "');\n");
        oSB.Append(sPrefix + ".Panes[" + iCount + "].ClientEvents = " + Utils.ConvertClientEventsToJsObject(pane.ClientEvents) + ";\n");
        if(pane.Collapsed) oSB.Append(sPrefix + ".Panes[" + iCount + "].Collapsed = true;\n");
        if(pane.ClientSideOnResize != null) oSB.Append(sPrefix + ".Panes[" + iCount + "].ClientSideOnResize = " + pane.ClientSideOnResize + ";\n");
        oSB.Append(sPrefix + ".Panes[" + iCount + "].MinHeight = " + (pane.MinHeight > 0? pane.MinHeight : 1) + ";\n");
        oSB.Append(sPrefix + ".Panes[" + iCount + "].MinWidth = " + (pane.MinWidth > 0? pane.MinWidth : 1) + ";\n");
        
        if(pane.Panes != null && pane.Panes.Count > 0)
        {
          oSB.Append(GeneratePaneDefinitions(pane.Panes, sPrefix + ".Panes[" + iCount + "]"));
        }

        iCount++;
      }
      
      return oSB.ToString();
    }

    private void ProcessCollapseChangeList(string sCollapseChangeList)
    {
      if(sCollapseChangeList != null && sCollapseChangeList != "")
      {
        string [] arCollapseChangeList = sCollapseChangeList.Split(';');

        foreach(string sCollapseChange in arCollapseChangeList)
        {
          string [] arParams = sCollapseChange.Split(' ');

          string sId = arParams[0];
          bool bCollapsed = (arParams[1] == "1");

          SplitterPane pane = FindPaneById(CurrentLayout, sId);

          if(pane != null && bCollapsed != pane.Collapsed)
          {
            // we have a change, apply it
            pane.Collapsed = bCollapsed;

            // trigger an event?
          }
        }
      }
    }

    private void ProcessResizeList(string sResizeList)
    {
      if(sResizeList != null && sResizeList != "")
      {
        string [] arResizeList = sResizeList.Split(';');

        foreach(string sResize in arResizeList)
        {
          string [] arParams = sResize.Split(' ');

          string sId = arParams[0];
          int iHeight = int.Parse(arParams[1]);
          int iWidth = int.Parse(arParams[2]);

          SplitterPane pane = FindPaneById(CurrentLayout, sId);

          if(pane != null)
          {
            pane.Height = iHeight;
            pane.Width = iWidth;
          }
        }
      }
    }

    internal void RenderDesignTime(HtmlTextWriter output)
    {
      // make sure all panes have IDs
      if(CurrentLayout != null)
      {
        AssignPaneIDs(this.CurrentLayout.Panes, null);
      }

      // Sanify writer
      output = new HtmlTextWriter(output);
      
      // Render the outer frame
      AddAttributesToRender(output);

      // Render the selected inner frame, if any
      if(CurrentLayout != null && CurrentLayout.Panes != null)
      {
        CurrentLayout.Panes.RenderControl(output);
      }
    }

    private void RenderPreloadImages(HtmlTextWriter output)
    {
      output.Write("<div style=\"position:absolute;top:0px;left:0px;visibility:hidden;\">"); 
      foreach(string sImage in this.PreloadImages)
      {
        output.Write("<img src=\"" + sImage + "\" width=\"0\" height=\"0\" alt=\"\" />\n");
      }
      output.Write("</div>"); 
    }

    #endregion
	}
}
