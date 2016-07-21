using System;
using System.Collections; 
using System.Collections.Specialized; 
using System.Data; 
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.IO; 
using System.Text;
using System.Xml; 
using ComponentArt.Web.UI.Design; 
using System.Runtime.InteropServices; 
using ComponentArt.Licensing.Providers;

namespace ComponentArt.Web.UI
{
  #region Custom enumerations

  /// <summary>
  /// Specifies which slide the <see cref="Rotator"/> control should output to down-level browsers.
  /// </summary>
  public enum DownLevelOutputType
  {
    /// <summary>Display a random slide.</summary>
    RandomSlide,
    
    /// <summary>Display the first slide.</summary>
    FirstSlide
  }

  /// <summary>
  /// Specifies the Internet Explorer transition effect to use for slide changes of <see cref="Rotator"/> control.
  /// </summary>
  public enum RotationEffect
  {
    /// <summary>No transition.</summary>
    None,

    /// <summary>Fading from the old to the new content.</summary>
    Fade,

    /// <summary>An animation of colored squares that take the average color value of the pixels they replace.</summary>
    Pixelate,

    /// <summary>Exposure of pixels in random order.</summary>
    Dissolve,

    /// <summary>Reveals new content by passing a gradient band over the old content.</summary>
    GradientWipe
  }

  /// <summary>
  /// Specifies how the <see cref="Rotator"/> control should cycle through its slides.
  /// </summary>
  public enum RotationType
  {
    /// <summary>Simple linear movement of slides.</summary>
    ContentScroll,

    /// <summary>Visual transition between static slides.</summary>
    SlideShow,

    /// <summary>Display one static randomly-chosen slide.</summary>
    RandomSlide,

    /// <summary>Variable-speed movement of slides.</summary>
    SmoothScroll
  }

  /// <summary>
  /// Specifies the direction in which the slides in the <see cref="Rotator"/> control move.
  /// </summary>
  public enum ScrollDirection
  {
    /// <summary>Slides come from the bottom and move upwards.</summary>
    Up,
    
    /// <summary>Slides come from the right and move to the left.</summary>
    Left
  }

  /// <summary>
  /// Specifies the speed at which <see cref="RotationType.SmoothScroll"/> slides in the <see cref="Rotator"/> control move.
  /// </summary>
  public enum SmoothScrollSpeed
  {
    /// <summary>Move slides at a medium speed.</summary>
    Medium,
    
    /// <summary>Move slides slowly.</summary>
    Slow,
    
    /// <summary>Move slides fast.</summary>
    Fast 
  }

  #endregion

  #region Rotator web control 

  /// <summary>
  ///   Cycles through a series of page fragments in a section of the page.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     Rotator is a data bound control that displays  data items using templates.
  ///   </para>
  ///   <para>
  ///     Rotator can display one or more items at a time, in various static or animated styles, depending on the value of its 
  ///     <see cref="RotationType"/> property.
  ///   </para>
  ///   <para>
  ///     Further animation of item text is possible using <see cref="Ticker"/> controls.  Tickers enable typewriter-like animation
  ///     effects of rotator item text.
  ///   </para>
  /// </remarks>
  [Designer(typeof(ComponentArt.Web.UI.Design.RotatorDesigner))]
  [DefaultProperty("RotationType")]
  [ToolboxData("<{0}:Rotator runat=server></{0}:Rotator>")]
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
	public class Rotator : ComponentArt.Web.UI.WebControl
	{

    #region Rotator Control interface 

    /// <summary>
    /// AutoStart property. 
    /// </summary>
    [ 
    Description("Whether content rotation should automatically start when the page loads. "), 
    Category("Content Rotation") 
    ]
    public bool AutoStart
    {
      get 
      {
        object o = ViewState["AutoStart"]; 
        return (o == null) ? true : (bool) o; 
      }
        set 
      {
        ViewState["AutoStart"] = value;
      }
    }

    /// <summary>
    /// DataMember property. 
    /// </summary>
    [ 
    Description("Data member for ASP.NET data binding. "), 
    Category("Data") 
    ]
    public virtual string DataMember
    {
      get
      {
        object o = ViewState["DataMember"]; 
        return (o == null) ? String.Empty : (string)o; 
      }

      set
      {
        ViewState["DataMember"] = value; 
      }
    }

    /// <summary>
    /// DataSource property. 
    /// </summary>
    private object _dataSource; 
    [ 
    Browsable(false), 
    Description("Data source for ASP.NET data binding. "), 
    Category("Data") 
    ]
    public object DataSource
    {
      get 
      { 
        return _dataSource; 
      }
      
      set 
      { 
        if (value == null || value is IEnumerable || value is IListSource)
          _dataSource = value; 
        else
          throw new ArgumentException(); 
      }
    }

    /// <summary>
    /// DownLevelOutputType property. 
    /// </summary>
    [ 
    Description("What to render for down-level browsers."), 
    Category("Content Rotation") 
    ]
    public DownLevelOutputType DownLevelOutputType
    {
      get 
      {
        object o = ViewState["DownLevelOutputType"]; 
        return (o == null) ? new DownLevelOutputType() : (DownLevelOutputType) o; 
      }
      set 
      {
        ViewState["DownLevelOutputType"] = value;
      }
    }

    /// <summary>
    /// HideEffect property. 
    /// </summary>
    [ 
    Description("Transition effect used for hiding individual slides. "), 
    Category("Content Rotation") 
    ]
    public RotationEffect HideEffect
    {
      get 
      {
        object o = ViewState["HideEffect"]; 
        return (o == null) ? new RotationEffect() : (RotationEffect) o; 
      }
        set 
      {
        ViewState["HideEffect"] = value;
      }
    }

    /// <summary>
    /// HideEffectDuration property. 
    /// </summary>
    [ 
    Description("Hide effect duration in milliseconds. "), 
    Category("Content Rotation") 
    ]
    public int HideEffectDuration
    {
      get 
      {
        object o = ViewState["HideEffectDuration"]; 
        return (o == null) ? 250 : (int) o; 
      }
      set 
      {
        ViewState["HideEffectDuration"] = value;
      }
    }

    /// <summary>
    /// Loop property. 
    /// </summary>
    [ 
    Description("Whether to loop back to the first slide after the last slide has been displayed. "), 
    Category("Content Rotation") 
    ]
    public bool Loop
    {
      get 
      {
        object o = ViewState["Loop"]; 
        return (o == null) ? true : (bool) o; 
      }
      set 
      {
        ViewState["Loop"] = value;
      }
    }

    /// <summary>
    /// PauseOnMouseOver property. 
    /// </summary>
    [ 
    Description("Whether the next slide should be paused on mouse over. "), 
    Category("Content Rotation") 
    ]
    public bool PauseOnMouseOver
    {
      get 
      {
        object o = ViewState["PauseOnMouseOver"]; 
        return (o == null) ? false : (bool) o; 
      }
      set 
      {
        ViewState["PauseOnMouseOver"] = value;
      }
    }

    /// <summary>
    /// RotationType property. 
    /// </summary>
    [ 
    Description("Content rotation type. "), 
    Category("Content Rotation") 
    ]
    public RotationType RotationType
    {
      get 
      {
        object o = ViewState["RotationType"]; 
        return (o == null) ? new RotationType() : (RotationType) o; 
      }
      set 
      {
        ViewState["RotationType"] = value;
      }
    }

    /// <summary>
    /// ScrollInterval property. 
    /// </summary>
    [ 
    Description("Applicable when RotationType=ContentScroll/SmoothScroll. Delay between scroll steps in milliseconds."), 
    Category("Content Rotation") 
    ]
    public int ScrollInterval
    {
      get 
      {
        object o = ViewState["ScrollInterval"]; 
        return (o == null) ? 20 : (int) o; 
      }
      set 
      {
        ViewState["ScrollInterval"] = value;
      }
    }

    /// <summary>
    /// ScrollDirection property. 
    /// </summary>
    [ 
    Description("Applicable when RotationType=ContentScroll/SmoothScroll. Direction in which the content will be scrolled. "), 
    Category("Content Rotation") 
    ]
    public ScrollDirection ScrollDirection
    {
      get 
      {
        object o = ViewState["ScrollDirection"]; 
        return (o == null) ? new ScrollDirection() : (ScrollDirection) o; 
      }
      set 
      {
        ViewState["ScrollDirection"] = value;
      }
    }

    /// <summary>
    /// ShowEffect property. 
    /// </summary>
    [ 
    Description("Transition effect used for showing individual slides. "), 
    Category("Content Rotation") 
    ]
    public RotationEffect ShowEffect
    {
      get 
      {
        object o = ViewState["ShowEffect"]; 
        return (o == null) ? new RotationEffect() : (RotationEffect) o; 
      }
      set 
      {
        ViewState["ShowEffect"] = value;
      }
    }

    /// <summary>
    /// ShowEffectDuration property. 
    /// </summary>
    [ 
    Description("Show effect duration in milliseconds. "), 
    Category("Content Rotation") 
    ]
    public int ShowEffectDuration
    {
      get 
      {
        object o = ViewState["ShowEffectDuration"]; 
        return (o == null) ? 250 : (int) o; 
      }
      set 
      {
        ViewState["ShowEffectDuration"] = value;
      }
    }

    /// <summary>
    /// SlideCssClass property. 
    /// </summary>
    [ 
    Description("CSS class for each slide. "), 
    Category("Appearance") 
    ]
    public string SlideCssClass
    {
      get 
      {
        object o = ViewState["SlideCssClass"]; 
        return (o == null) ? String.Empty : (string) o; 
      }
      set 
      {
        ViewState["SlideCssClass"] = value;
      }
    }

    /// <summary>
    /// SlidePause property. 
    /// </summary>
    [ 
    Description("Pause between slides in milliseconds. "), 
    Category("Content Rotation") 
    ]
    public int SlidePause
    {
      get 
      {
        object o = ViewState["SlidePause"]; 
        return (o == null) ? 2000 : (int) o; 
      }
      set 
      {
        ViewState["SlidePause"] = value;
      }
    }

    /// <summary>
    /// SlideTemplate property. 
    /// </summary>
    private ITemplate _slideTemplate; 
    [
    Browsable(false), 
    PersistenceMode(PersistenceMode.InnerProperty),
    TemplateContainer(typeof(ComponentArt.Web.UI.Slide))
    ]
    public ITemplate SlideTemplate
    {
      get 
      { 
        return _slideTemplate; 
      }
      set 
      { 
        _slideTemplate = value; 
      }
    }

    /// <summary>
    /// SmoothScrollSpeed property. 
    /// </summary>
    [ 
    Description("Speed of the smooth scroll effect when RotationType=SmoothScroll. "), 
    Category("Content Rotation") 
    ]
    public SmoothScrollSpeed SmoothScrollSpeed
    {
      get 
      {
        object o = ViewState["SmoothScrollSpeed"]; 
        return (o == null) ? new SmoothScrollSpeed() : (SmoothScrollSpeed) o; 
      }
      set 
      {
        ViewState["SmoothScrollSpeed"] = value;
      }
    }

    /// <summary>
    /// XmlContentFile property. 
    /// </summary>
    [ 
    Description("XML file containing the rotation content. "), 
    Category("Data") 
    ]
    public string XmlContentFile
    {
      get 
      {
        object o = ViewState["XmlContentFile"]; 
        return (o == null) ? String.Empty : (string) o; 
      }
      set 
      {
        ViewState["XmlContentFile"] = value;
      }
    }

    #endregion

    #region Rotator Control implementation 

    // Private, protected, and internal members 
    protected StringCollection slides = new StringCollection(); 
    internal StringCollection tickers = new StringCollection(); 
    internal StringCollection leadTickers = new StringCollection(); 
    internal bool hasTickers = false; 
    private bool _writtenContents = false; 

/*

    public Rotator() : base ("DIV")
    {
    }

*/    
    #region Data Binding 

    protected virtual IEnumerable GetDataSource()
    {
      if (_dataSource == null) return null; 

      IEnumerable resolvedDataSource = _dataSource as IEnumerable; 
      if (resolvedDataSource != null) return resolvedDataSource; 

      IListSource listSource = _dataSource as IListSource; 
      if (listSource != null)
      {
        IList memberList = listSource.GetList(); 
        if (!listSource.ContainsListCollection) return (IEnumerable)memberList; 

        ITypedList typedMemberList = memberList as ITypedList; 
        if (typedMemberList != null)
        {
          PropertyDescriptorCollection propDescs = typedMemberList.GetItemProperties(new PropertyDescriptor[0]); 
          PropertyDescriptor memberProperty = null; 

          if (propDescs != null && propDescs.Count != 0)
          {
            string dataMember = DataMember; 

            if (dataMember.Length == 0) 
              memberProperty = propDescs[0]; 
            else
              memberProperty = propDescs.Find(dataMember, true); 

            if (memberProperty != null)
            {
              object listRow = memberList[0]; 
              object list = memberProperty.GetValue(listRow); 

              if (list is IEnumerable) return (IEnumerable)list; 
            }
            throw new Exception("A list corresponding to the selected DataMember was not found. "); 
          }
          throw new Exception("The selected data source did not contain any data members to bind to."); 
        }
      }
      return null; 
    }


    protected override void CreateChildControls()
    {
      Controls.Clear(); 

      if (ViewState["ItemCount"] != null)
      {
        CreateControlHierarchy(false); 
      }
    }

    protected void CreateControlHierarchy(bool UseDataSource)
    {
      IEnumerable data = UseDataSource ? GetDataSource() : new object[(int)ViewState["ItemCount"]];
      if (data == null)
      {
        return;
      }
      IEnumerator dataItems = data.GetEnumerator();
      int itemCounter = 0;
      if (this.ClientTarget != ClientTargetLevel.Accessible && this.IsDownLevel() || RotationType == RotationType.RandomSlide)
      {
        if (this.IsDownLevel() && DownLevelOutputType == DownLevelOutputType.FirstSlide) 
        {
          dataItems.MoveNext(); 
        }
        else
        {
          // Load items to count them
          ArrayList list = new ArrayList();
          while (dataItems.MoveNext())
          {
            list.Add(dataItems.Current);
          }
          itemCounter = list.Count;

          if (itemCounter > 0)
          {
            // Pick a random item 
            Random rnd = new Random();
            int choice = rnd.Next(0, itemCounter);

            // Create the chosen slide
            Slide slide = new Slide(list[choice], this.ClientID + "_slide0", this);
            slides.Add(this.ClientID + "_slide0");
            slide.DataBind();
            SlideTemplate.InstantiateIn(slide);
            Controls.Add(slide);
            this.OnSlideDataBound(slide, list[choice]);
          }
        }
      }
      else
      {
        while (dataItems.MoveNext())
        {
          Slide slide = new Slide(dataItems.Current, this.ClientID + "_slide" + itemCounter.ToString(), this); 
          slides.Add(this.ClientID + "_slide" + itemCounter.ToString()); 
          slide.DataBind(); 

          SlideTemplate.InstantiateIn(slide); 
          Controls.Add(slide);
          this.OnSlideDataBound(slide, dataItems.Current);
          itemCounter++; 
        }
        ViewState["ItemCount"] = itemCounter; 
      }
    }

    /// <summary>
    /// Delegate for <see cref="SlideDataBound"/> event of <see cref="Rotator"/> class.
    /// </summary>
    public delegate void SlideDataBoundEventHandler(object sender, SlideDataBoundEventArgs e);

    /// <summary>
    /// Fires after a Rotator slide is data bound.
    /// </summary>
    [Description("Fires after a Rotator slide is data bound.")]
    public event SlideDataBoundEventHandler SlideDataBound;

    // generic trigger
    protected void OnSlideDataBound(Slide slide, object dataItem)
    {
      if (this.SlideDataBound != null)
      {
        SlideDataBoundEventArgs e = new SlideDataBoundEventArgs();
        e.Slide = slide;
        e.DataItem = dataItem;
        SlideDataBound(this, e);
      }
    }

    protected override void OnDataBinding(EventArgs e)
    {
      base.OnDataBinding(e); 

      Controls.Clear(); 
      if (HasChildViewState) ClearChildViewState(); 

      CreateControlHierarchy(true); 

      ChildControlsCreated = true; 

      if (IsTrackingViewState) TrackViewState(); 
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      if (ScriptManager.GetCurrent(Page) != null)
      {
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573G988.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.client_scripts.A573Z388.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Rotator.client_scripts.A573Z288.js");
        this.RegisterScriptForAtlas("ComponentArt.Web.UI.Rotator.client_scripts.A573G788.js");
      }
    }

    private void bindWithXmlContentFile()
    {
      if (XmlContentFile != String.Empty)
      {
        DataSet ds = new DataSet(); 
        ds.ReadXml(Context.Server.MapPath(XmlContentFile)); 
        DataSource = ds; 
        DataBind(); 
      }
    }

    protected override bool IsDownLevel()
    {
      if(this.ClientTarget != ClientTargetLevel.Auto)
      {
        return this.ClientTarget == ClientTargetLevel.Downlevel;
      }
      return (!isUpLevelBrowser());
    }

    // This is really ugly, but we need to check IsDownLevel from the Slide class,
    // and its access modifier is "protected".
    internal bool _IsDownLevel()
    {
      return this.IsDownLevel();
    }

    internal new bool IsRunningInDesignMode()
    {
      return base.IsRunningInDesignMode();
    }

    #endregion 


    #region Rendering 

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      if (!this.IsDownLevel())
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
          if (!Page.IsClientScriptBlockRegistered("A573Z388.js"))
          {
            Page.RegisterClientScriptBlock("A573Z388.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.client_scripts", "A573Z388.js");
          }
          if (!Page.IsClientScriptBlockRegistered("A573Z288.js"))
          {
            Page.RegisterClientScriptBlock("A573Z288.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Rotator.client_scripts", "A573Z288.js");
          }
          if (!Page.IsClientScriptBlockRegistered("A573G788.js"))
          {
            Page.RegisterClientScriptBlock("A573G788.js", "");
            WriteGlobalClientScript(output, "ComponentArt.Web.UI.Rotator.client_scripts", "A573G788.js");
          }
        }
      }

      if (Width == Unit.Empty) Width = Unit.Parse("250px");
      if (Height == Unit.Empty) Height = Unit.Parse("50px");
      if (_dataSource == null)
      {
        if (XmlContentFile != String.Empty)
          bindWithXmlContentFile();
        else
          AutoStart = false;
      }

      System.Web.UI.HtmlTextWriter tw;

      if (!this.IsDownLevel())
      {
        if (Context.Request.Browser.Browser == "Opera" & RotationType == RotationType.SlideShow)
        {
          RotationType = RotationType.SmoothScroll;
        }
        tw = new HtmlTextWriter(output);
        if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
        {
          this.Width = Unit.Empty;
          this.Height = Unit.Empty;
        }
        AddAttributesToRender(tw);
        if (RotationType != RotationType.RandomSlide && this.ClientTarget != ClientTargetLevel.Accessible) tw.AddStyleAttribute("overflow", "hidden");
        RenderPauseEventHandlers(tw);
        tw.RenderBeginTag(HtmlTextWriterTag.Div);
        RenderContents(tw);
        _writtenContents = true;
        tw.RenderEndTag();
      }
      else
      {
        RenderSingleSlide(output);
      }
    }

    protected override void RenderContents(HtmlTextWriter output)
    {
      if (_writtenContents)
      {
        return; // Contents have already been rendered
      }
      if (this.ClientTarget == ClientTargetLevel.Accessible || this.ClientTarget == ClientTargetLevel.Auto && this.IsAccessible())
      {
        RenderAccessibleContents(output);
      }
      else
      {
        switch (RotationType)
        {
          case RotationType.ContentScroll:
            RenderScrollContents(output);
            break;
          case RotationType.SmoothScroll:
            RenderScrollContents(output);
            break;
          case RotationType.SlideShow:
            RenderSlideShowContents(output);
            break;
          case RotationType.RandomSlide:
            RenderSingleSlide(output);
            break;
        }
      }
    }

    protected void RenderPauseEventHandlers(HtmlTextWriter output)
    {
      if (this.ClientTarget != ClientTargetLevel.Accessible)
      {
        if (PauseOnMouseOver && RotationType != RotationType.RandomSlide &&
             (_dataSource != null || XmlContentFile != String.Empty))
        {
          if (Context.Request.Browser.Browser == "IE")
          {
            output.AddAttribute("onmouseover", "ie_MsOver(this, " + "rco_" + ClientID + ");");
            output.AddAttribute("onmouseout", "ie_MsOut(this, " + "rco_" + ClientID + ");");
          }
          else if (Context.Request.Browser.Browser == "Netscape")
          {
            output.AddAttribute("onmouseover", "ns_MsOver(event, '" + ClientID + "', " + "rco_" + ClientID + ");");
            output.AddAttribute("onmouseout", "ns_MsOut(event, '" + ClientID + "', " + "rco_" + ClientID + ");");
          }
        }
      }
    }

    private void RenderAccessibleContents(HtmlTextWriter output)
    {
      output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_SlideContainer");
      output.RenderBeginTag(HtmlTextWriterTag.Div);

      base.RenderContents(output);

      output.RenderEndTag(); // </div>
    }

    protected void RenderScrollContents(HtmlTextWriter output)
    {
      //RegisterClientScript("Rotator"); 

      output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_SlideContainer"); 
      if (Context.Request.Browser.Browser != "IE")
      {
        output.AddStyleAttribute("position", "relative");
      }
      output.AddStyleAttribute("visibility", "hidden"); 
      output.RenderBeginTag(HtmlTextWriterTag.Div); 

      if (ScrollDirection == ScrollDirection.Left)
      {
        output.AddAttribute("cellpadding", "0"); 
        output.AddAttribute("cellspacing", "0"); 
        output.RenderBeginTag("table"); 
        output.AddAttribute("id", ClientID + "_ContainerRow"); 
        output.RenderBeginTag("tr"); 
      }

      base.RenderContents(output); 

      if (ScrollDirection == ScrollDirection.Left)
      {
        output.RenderEndTag(); // </tr>
        output.RenderEndTag(); // </table>
      }

      output.RenderEndTag(); // </div>
      
      RenderInstanceClientScript(output); 
    }    

    protected void RenderSlideShowContents(HtmlTextWriter output)
    {
      //RegisterClientScript("Rotator"); 

      output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + "_SlideContainer"); 
      if (Context.Request.Browser.Browser != "IE")
      {
        output.AddStyleAttribute("position", "relative");
      }
      output.AddStyleAttribute("visibility", "hidden"); 
      output.AddStyleAttribute("width", "100%"); 
      output.AddAttribute(HtmlTextWriterAttribute.Class, SlideCssClass); 
      output.RenderBeginTag(HtmlTextWriterTag.Div); 
      output.RenderEndTag(); 
      //output.Write("</div>"); 

      base.RenderContents(output); 
      
      RenderInstanceClientScript(output); 
    }    

    protected void RenderSingleSlide(HtmlTextWriter output)
    {
      base.RenderContents(output);       
    }

    // Render javascript to initialize the Rotator instance.
    private void RenderInstanceClientScript(HtmlTextWriter output)
    {
      StringBuilder instanceScript = new StringBuilder();
      string rotatorClientVarName = "rco_" + this.ClientID;
      instanceScript.Append("if(window." + rotatorClientVarName + "){" + rotatorClientVarName + ".Destroy();}\n");
      instanceScript.Append("window." + rotatorClientVarName + " = new ComponentArt_Rotator();\n");
      instanceScript.Append(rotatorClientVarName + ".GlobalID = '" + rotatorClientVarName + "';\n");
      instanceScript.Append(rotatorClientVarName + ".ElementID = '" + this.ClientID + "';\n");
      instanceScript.Append(rotatorClientVarName + ".ContainerID = '" + this.ClientID + "_SlideContainer';\n");
      instanceScript.Append(rotatorClientVarName + ".ContainerRowID = '" + this.ClientID + "_ContainerRow';\n");
      instanceScript.Append(rotatorClientVarName + ".AutoStart = " + this.AutoStart.ToString().ToLower() + ";\n");
      instanceScript.Append(rotatorClientVarName + ".SlidePause = " + this.SlidePause.ToString() + ";\n");
      instanceScript.Append(rotatorClientVarName + ".HideEffect = " + this.effectString(this.HideEffect, this.HideEffectDuration) + ";\n");
      instanceScript.Append(rotatorClientVarName + ".HideEffectDuration = " + this.HideEffectDuration.ToString(NumberFormatInfo.InvariantInfo) + ";\n");
      instanceScript.Append(rotatorClientVarName + ".Loop = " + this.Loop.ToString().ToLower() + ";\n");
      instanceScript.Append(rotatorClientVarName + ".PauseOnMouseOver = " + PauseOnMouseOver.ToString().ToLower() + ";\n");
      instanceScript.Append(rotatorClientVarName + ".RotationType = '" + this.RotationType.ToString() + "';\n");
      instanceScript.Append(rotatorClientVarName + ".ScrollDirection = '" + this.ScrollDirection.ToString().ToLower() + "';\n");
      instanceScript.Append(rotatorClientVarName + ".ScrollInterval = " + this.ScrollInterval.ToString() + ";\n");
      instanceScript.Append(rotatorClientVarName + ".ShowEffect = " + this.effectString(this.ShowEffect, this.ShowEffectDuration) + ";\n");
      instanceScript.Append(rotatorClientVarName + ".ShowEffectDuration = " + this.ShowEffectDuration.ToString(NumberFormatInfo.InvariantInfo) + ";\n");
      instanceScript.Append(rotatorClientVarName + ".SmoothScrollSpeed = '" + this.SmoothScrollSpeed.ToString() + "';\n");
      if (this.slides.Count > 0)
      {
        string[] _slides = new string[this.slides.Count];
        for (int i = 0; i < _slides.Length; i++)
        {
          _slides[i] = "'" + this.slides[i].Replace("'", "\\'") + "'";
        }
        instanceScript.Append(rotatorClientVarName + ".Slides = [" + String.Join(",", _slides) + "];\n");
      }
      instanceScript.Append(rotatorClientVarName + ".HasTickers = " + this.hasTickers.ToString().ToLower() + ";\n");
      if (this.leadTickers.Count > 0)
      {
        string[] _leadTickers = new string[this.leadTickers.Count];
        this.leadTickers.CopyTo(_leadTickers, 0);
        instanceScript.Append(rotatorClientVarName + ".LeadTickers = [" + String.Join(",", _leadTickers) + "];\n");
      }
      if (this.tickers.Count > 0)
      {
        string[] _tickers = new string[this.tickers.Count];
        this.tickers.CopyTo(_tickers, 0);
        instanceScript.Append(rotatorClientVarName + ".Tickers = [" + String.Join(",", _tickers) + "];\n");
      }
      instanceScript.Append("if(" + rotatorClientVarName + ".AutoStart) {rcr_Start(" + rotatorClientVarName + ");}\n");
      string scriptComment = "ComponentArt Web.UI Rotator " + this.VersionString() + " start-up script for instance " + this.ClientID;
      output.Write(this.DemarcateClientScript(instanceScript.ToString(), scriptComment));
    }

    private void RenderMessage(HtmlTextWriter output, string Message)
    {
      output.Write("<div style=\"background-color:#3F3F3F;border:1px;border-style:solid;border-bottom-color:black;border-right-color:black;border-left-color:lightslategray;border-top-color:lightslategray;color:cornsilk;padding:2px;font-family:verdana;font-size:11px;");
      if (this.IsRunningInDesignMode()) output.Write("height:100%;"); 
      if (this.Width.IsEmpty)
        output.Write("\">"); 
      else
        output.Write("width:100%\">"); 
      output.Write("<b>Rich Content Rotator</b> :: " + Message + "</div>"); 
    }

    private string effectString(RotationEffect effect, int duration)
    {
      if (!supportEffects()) return "null"; 

      string result; 

      switch(effect)
      {
        case RotationEffect.Dissolve : 
          result = "'progid:DXImageTransform.Microsoft.RandomDissolve(Duration=_duration_)'"; 
          break; 
        case RotationEffect.Fade : 
          result = "'progid:DXImageTransform.Microsoft.Fade(Duration=_duration_, Overlap=0.0)'"; 
          break; 
        case RotationEffect.GradientWipe : 
          result = "'progid:DXImageTransform.Microsoft.GradientWipe(duration=_duration_, GradientSize=0.25, wipestyle=0, motion=forward)'"; 
          break; 
        case RotationEffect.Pixelate : 
          result = "'progid:DXImageTransform.Microsoft.Pixelate(Duration=_duration_, MaxSquare=15)'"; 
          break; 
        default : 
          result = "null"; 
          break; 
      }
      double d = duration * 0.001; 
      result = result.Replace("_duration_", d.ToString(System.Globalization.NumberFormatInfo.InvariantInfo)); 
      return result; 
    }

    // Whether the request was made by a supported browser 
    internal bool isUpLevelBrowser()
    {
      if (this.IsRunningInDesignMode()) return false; 
      if (Context.Request.UserAgent == null) return false; 
      if (Context.Request.UserAgent.IndexOf("Safari") >= 0) return true; 

      if (Context.Request.Browser.Browser == "IE" & 
          Context.Request.Browser.MajorVersion >= 5 & 
          !(Context.Request.Browser.Platform.ToLower().StartsWith("mac")))
        return true; 
      else if (Context.Request.UserAgent.IndexOf("Gecko") >= 0)
        return true; 
      else if (Context.Request.Browser.Browser == "Opera" & Context.Request.Browser.MajorVersion >= 7)
        return true; 
      else
        return false; 
    }

    // Whether to support expand effects. 
    internal bool supportEffects()
    {
      if (Context.Request.Headers["User-Agent"].IndexOf("NT 4") > 0)  
        return false; 
      else if (Context.Request.Browser.Platform.StartsWith("Mac"))
        return false; 
      else if (Context.Request.Browser.Browser == "IE" && Context.Request.Browser.MajorVersion >= 6)
        return true; 
      else if (Context.Request.Browser.Browser == "IE" && Context.Request.Browser.MajorVersion == 5 && Context.Request.Browser.MinorVersion >= 5)
        return true; 
      else
        return false; 
    }

    #endregion 

    #endregion
    
	}
  #endregion 

  #region Slide web control 

  /// <summary>
  /// One of the page fragments managed by <see cref="Rotator"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class Slide : System.Web.UI.WebControls.WebControl, INamingContainer 
  {
    #region Slide Control Interface 
    /// <summary>
    /// ClientSideID property. 
    /// </summary>
    private string _clientSideID;
    public string ClientSideID
    {
      get 
      { 
        return _clientSideID; 
      }

      set 
      { 
        _clientSideID = value; 
      }
    }

    /// <summary>
    /// DataItem property. 
    /// </summary>
    private object _dataItem;
    public object DataItem
    {
      get 
      { 
        return _dataItem; 
      }

      set 
      { 
        _dataItem = value; 
      }
    }

    #endregion 

    #region Slide Control Implementation 

    // Private, protected, internal members ---------------------------------------------
    private Rotator _rotator; 
    private StringCollection _tickers = new StringCollection(); 

    public Slide(object dataItem, string clientID, Rotator Rotator) : base ("DIV")
    {
      DataItem = dataItem; 
      CssClass = Rotator.SlideCssClass;
      ClientSideID = clientID; 
      _rotator = Rotator; 
    }

    protected override void Render(HtmlTextWriter output)
    {
      if (_rotator.IsRunningInDesignMode())
      {
        base.RenderContents(output); 
      }
      else
      {
        HtmlTextWriter tw = new HtmlTextWriter(output);
        //tw = new HtmlTextWriter(Page.Response.Output); 
        
        if ((_rotator.RotationType == RotationType.ContentScroll || _rotator.RotationType == RotationType.SmoothScroll) && _rotator.ScrollDirection == ScrollDirection.Left)
        {
          tw.RenderBeginTag(HtmlTextWriterTag.Td); 
        }
        AddAttributesToRender(tw);
        tw.AddAttribute("id", ClientSideID); 
        if (_rotator.ClientTarget != ClientTargetLevel.Accessible && !_rotator._IsDownLevel() && _rotator.RotationType == RotationType.SlideShow)
        {
          tw.AddStyleAttribute("position", "absolute"); 
          tw.AddStyleAttribute("visibility", "hidden"); 
        }
        SetChildTickers(); 
        tw.RenderBeginTag(HtmlTextWriterTag.Div);
        base.RenderContents(tw); 
        tw.RenderEndTag(); // </div>
        if ((_rotator.RotationType == RotationType.ContentScroll || _rotator.RotationType == RotationType.SmoothScroll) && _rotator.ScrollDirection == ScrollDirection.Left)
        {
          tw.RenderEndTag(); // </td>
        }
      }
    }
   
    protected void SetChildTickers()
    {
      // Enumerate all tickers, and set their default properties 
      foreach (Control childControl in Controls)
      {
        Ticker curTicker; 
        if (childControl is Ticker)
        {
          _rotator.hasTickers = true; 
          curTicker = childControl as Ticker; 
          curTicker.AutoStart = false; 
          curTicker.Loop = false; 
          if (_rotator.ClientScriptLocation != String.Empty && curTicker.ClientScriptLocation == String.Empty)
          {
            curTicker.ClientScriptLocation = _rotator.ClientScriptLocation; 
          }
          _tickers.Add(curTicker.ClientID); 
          _rotator.tickers.Add("tco_" + curTicker.ClientID); 
        }
      }

      // Add the first ticker to the head ticker collection 
      if (_rotator.hasTickers) _rotator.leadTickers.Add("tco_" + _tickers[0]); 

      // Set OnEnd event handlers for every ticker instance 
      int tickerIndex = 0; 
      foreach (Control childControl in Controls)
      {
        Ticker curTicker; 
        if (childControl is Ticker)
        {
          curTicker = childControl as Ticker; 
          if (tickerIndex == _tickers.Count - 1) 
            curTicker.OnEnd = "rcr_EndTickerSequence(rco_" + _rotator.ClientID + ")"; 
          else
            curTicker.OnEnd = "rcr_StartTicker(tco_" + _tickers[tickerIndex + 1] + ")"; 
          tickerIndex++; 
        }
      }
    }

    #endregion 
  }
  #endregion 

  /// <summary>
  /// Arguments for <see cref="Rotator.SlideDataBound"/> server-side event of <see cref="Rotator"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class SlideDataBoundEventArgs : EventArgs
  {
    /// <summary>
    /// The Rotator slide.
    /// </summary>
    public Slide Slide;

    /// <summary>
    /// The data item bound to.
    /// </summary>
    public object DataItem;
  }
}
