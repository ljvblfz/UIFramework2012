using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Collections;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// A grouping of <see cref="SplitterPane"/> controls.
  /// </summary>
  [ParseChildren(true, "Panes")]
  [ToolboxItem(false)]
  public class SplitterPaneGroup : System.Web.UI.WebControls.WebControl
  {
    #region Properties

    internal Splitter ParentSplitter;

    public SplitterPane this[int index] 
    {
      get 
      {
        return this.Panes[index];
      }
    }

    private SplitterPaneCollection _panes;
    /// <summary>
    /// Gets a SplitterPaneCollection object that represents the child controls for a specified server control in the UI hierarchy.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    public SplitterPaneCollection Panes
    {
      get
      {
        if (_panes == null)
        {
          _panes = new SplitterPaneCollection();
        }
        return _panes;
      }
    }

    /// <summary>
    /// Not persisted.  Use <see cref="Panes"/> instead.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override ControlCollection Controls
    {
      get
      {
        return base.Controls;
      }
    }

    /// <summary>
    /// The number of SplitterPanes in this group.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int Count
    {
      get
      {
        return this.Panes.Count;
      }
    }

    /// <summary>
    /// Whether to resize content and fire pane resize events as the user drags to resize.
    /// </summary>
    [DefaultValue(false)]
    public bool LiveResize
    {
      get
      {
        string o = Properties["LiveResize"]; 
        return (o == null) ? false : bool.Parse(o);
      }
      set
      {
        Properties["LiveResize"] = value.ToString();
      }
    }

    /// <summary>
    /// The orientation to use for this collection of panes.
    /// </summary>
    [DefaultValue(SplitterOrientation.Horizontal)]
    [Description("The orientation to use for this collection of panes.")]
    public SplitterOrientation Orientation
    {
      get
      {
        string o = Properties["Orientation"]; 
        return (o == null) ? SplitterOrientation.Horizontal : (SplitterOrientation)Enum.Parse(typeof(SplitterOrientation), o);
      }
      set
      {
        Properties["Orientation"] = value.ToString();
      }
    }

    private System.Web.UI.AttributeCollection _properties;
    internal System.Web.UI.AttributeCollection Properties
    {
      get
      {
        if(_properties == null)
        {
          StateBag oBag = new StateBag(true);
          _properties = new System.Web.UI.AttributeCollection(oBag);
        }

        return _properties;
      }
      set
      {
        _properties = value;
      }
    }

    /// <summary>
    /// Whether to allow resizing in this pane group.
    /// </summary>
    [DefaultValue(true)]
    public bool Resizable
    {
      get
      {
        string o = Properties["Resizable"];
        return o == null? true : bool.Parse(o);
      }
      set
      {
        Properties["Resizable"] = value.ToString();
      }
    }

    /// <summary>
    /// The size of a minimal step (in pixels) while resizing.
    /// </summary>
    [DefaultValue(0)]
    public int ResizeStep
    {
      get
      {
        string o = Properties["ResizeStep"];
        return o == null? 0 : int.Parse(o);
      }
      set
      {
        Properties["ResizeStep"] = value.ToString();
      }
    }

    /// <summary>
    /// The CssClass to apply to splitter bars in this pane group while dragging.
    /// </summary>
    [DefaultValue("")]
    public string SplitterBarActiveCssClass
    {
      get
      {
        string o = Properties["SplitterBarActiveCssClass"];
        return o == null? string.Empty : o;
      }
      set
      {
        Properties["SplitterBarActiveCssClass"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to splitter bars in this pane group when the pane to the left is collapsed.
    /// </summary>
    [DefaultValue("")]
    public string SplitterBarCollapsedCssClass
    {
      get
      {
        string o = Properties["SplitterBarCollapsedCssClass"];
        return o == null? string.Empty : o;
      }
      set
      {
        Properties["SplitterBarCollapsedCssClass"] = value;
      }
    }

    /// <summary>
    /// The image to use on hover for pane collapse functionality.
    /// </summary>
    [DefaultValue("")]
    public string SplitterBarCollapseHoverImageUrl
    {
      get
      {
        string o = Properties["SplitterBarCollapseHoverImageUrl"];
        return o == null? string.Empty : o;
      }
      set
      {
        Properties["SplitterBarCollapseHoverImageUrl"] = value;
      }
    }

    /// <summary>
    /// The height of the collapse image in pixels.
    /// </summary>
    [DefaultValue("")]
    public int SplitterBarCollapseImageHeight
    {
      get
      {
        string o = Properties["SplitterBarCollapseImageHeight"];
        return o == null? 0 : int.Parse(o);
      }
      set
      {
        Properties["SplitterBarCollapseImageHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// The image to use for pane collapse functionality.
    /// </summary>
    [DefaultValue("")]
    public string SplitterBarCollapseImageUrl
    {
      get
      {
        string o = Properties["SplitterBarCollapseImageUrl"];
        return o == null? string.Empty : o;
      }
      set
      {
        Properties["SplitterBarCollapseImageUrl"] = value;
      }
    }

    /// <summary>
    /// The width of the collapse image in pixels.
    /// </summary>
    [DefaultValue("")]
    public int SplitterBarCollapseImageWidth
    {
      get
      {
        string o = Properties["SplitterBarCollapseImageWidth"];
        return o == null? 0 : int.Parse(o);
      }
      set
      {
        Properties["SplitterBarCollapseImageWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// The CssClass to apply to splitter bars in this pane group.
    /// </summary>
    [DefaultValue("")]
    public string SplitterBarCssClass
    {
      get
      {
        string o = Properties["SplitterBarCssClass"];
        return o == null? string.Empty : o;
      }
      set
      {
        Properties["SplitterBarCssClass"] = value;
      }
    }
    
    /// <summary>
    /// The image to use on hover for pane expand functionality.
    /// </summary>
    [DefaultValue("")]
    public string SplitterBarExpandHoverImageUrl
    {
      get
      {
        string o = Properties["SplitterBarExpandHoverImageUrl"];
        return o == null? string.Empty : o;
      }
      set
      {
        Properties["SplitterBarExpandHoverImageUrl"] = value;
      }
    }

    /// <summary>
    /// The image to use for pane expand functionality.
    /// </summary>
    [DefaultValue("")]
    public string SplitterBarExpandImageUrl
    {
      get
      {
        string o = Properties["SplitterBarExpandImageUrl"];
        return o == null? string.Empty : o;
      }
      set
      {
        Properties["SplitterBarExpandImageUrl"] = value;
      }
    }

    /// <summary>
    /// The CssClass to apply to splitter bars in this pane group on hover.
    /// </summary>
    [DefaultValue("")]
    public string SplitterBarHoverCssClass
    {
      get
      {
        string o = Properties["SplitterBarHoverCssClass"];
        return o == null? string.Empty : o;
      }
      set
      {
        Properties["SplitterBarHoverCssClass"] = value;
      }
    }
    
    /// <summary>
    /// The height of the splitter bar for this group, in pixels.
    /// </summary>
    [DefaultValue(0)]
    public int SplitterBarWidth
    {
      get
      {
        string o = Properties["SplitterBarWidth"];
        return o == null? 0 : int.Parse(o);
      }
      set
      {
        Properties["SplitterBarWidth"] = value.ToString();
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Overridden. Filters out all objects except SplitterPane objects.
    /// </summary>
    /// <param name="obj">The parsed element.</param>
    protected override void AddParsedSubObject(object obj)
    {
      if (obj is SplitterPane)
      {
        base.AddParsedSubObject(obj);
      }
    }

    protected override void Render(HtmlTextWriter output)
    {
      //AddAttributesToRender(output);
      output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
      output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
      output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
      output.RenderBeginTag(HtmlTextWriterTag.Table);

      if(Orientation == SplitterOrientation.Horizontal)
      {
        output.RenderBeginTag(HtmlTextWriterTag.Tr);
      }

      for(int i = 0; i < this.Panes.Count; i++)
      {
        SplitterPane pane = (SplitterPane)this.Panes[i];

        if(Orientation == SplitterOrientation.Vertical)
        {
          output.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
          if(pane.Height != Unit.Empty)
          {
            output.AddStyleAttribute("height", pane.Height.ToString());
          }

          output.AddStyleAttribute("width", "100%");
        }

        if(Orientation == SplitterOrientation.Horizontal)
        {
          if(pane.Width != Unit.Empty)
          {          
            output.AddStyleAttribute("width", pane.Width.ToString());
          }
          else
          {
            output.AddAttribute("width", "100%");
          }

          output.AddStyleAttribute("height", "100%");
        }

        if(pane.Collapsed)
        {
          output.AddStyleAttribute("display", "none");
        }
        if(pane.CssClass != "")
        {
          output.AddAttribute(HtmlTextWriterAttribute.Class, pane.CssClass);
        }
        output.AddAttribute(HtmlTextWriterAttribute.Id, pane.ID);
        output.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
        output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

        pane.RenderControl(output);
        
        output.RenderEndTag(); // </td>
          
        if(Orientation == SplitterOrientation.Vertical)
        {
          output.RenderEndTag(); // </tr>
        }

        // Splitter dragbar
        if(i < this.Panes.Count - 1)
        {
          if(Orientation == SplitterOrientation.Vertical)
          {
            output.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
            output.AddStyleAttribute("height", SplitterBarWidth.ToString() + "px");            
          }
          else
          {
            output.AddStyleAttribute("width", SplitterBarWidth.ToString() + "px");
          }

          if(this.SplitterBarCssClass != "")
          {
            output.AddAttribute(HtmlTextWriterAttribute.Class, SplitterBarCssClass);
          }
          
          if(pane.AllowResizing && this.Resizable)
          {
            if(this.SplitterBarHoverCssClass != "")
            {
              output.AddAttribute("onmouseout", ParentSplitter.ClientObjectId + ".HandleBarMouseOut(this)");
              output.AddAttribute("onmouseover", ParentSplitter.ClientObjectId + ".HandleBarMouseOver(this)");
            }
            output.AddAttribute("onmousedown", ParentSplitter.ClientObjectId + ".HandleResizeStart(event,this,'" + pane.ID + "'," + (Orientation == SplitterOrientation.Horizontal? 1 : 0) + ")");
            
            // add mouse cursor
            if(this.Orientation == SplitterOrientation.Horizontal)
            {
              if(Context == null || Context.Request.UserAgent.IndexOf("MSIE") >= 0)
              {
                output.AddStyleAttribute("cursor", "col-resize");
              }
              else
              {
                output.AddStyleAttribute("cursor", "w-resize");
              }
            }
            else
            {
              if(Context == null || Context.Request.UserAgent.IndexOf("MSIE") >= 0)
              {
                output.AddStyleAttribute("cursor", "row-resize");
              }
              else
              {
                output.AddStyleAttribute("cursor", "n-resize");
              }
            }
          }

          output.AddStyleAttribute("vertical-align", "middle");
          output.AddStyleAttribute("text-align", "center");
              
          output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
          
          if(this.SplitterBarCollapseImageUrl != "")
          {
            if(this.SplitterBarCollapseHoverImageUrl != "")
            {
              output.AddAttribute("onmouseout", ParentSplitter.ClientObjectId + ".HandleButtonMouseOut(this)");
              output.AddAttribute("onmouseover", ParentSplitter.ClientObjectId + ".HandleButtonMouseOver(this)");
            }
            output.AddAttribute("onmousedown", "ComponentArt_CancelEvent(event)");
            output.AddAttribute("onclick", ParentSplitter.ClientObjectId + ".ToggleExpand('" + pane.ID + "')");
            
            if(this.SplitterBarCollapseImageHeight > 0)
            {
              output.AddAttribute(HtmlTextWriterAttribute.Height, this.SplitterBarCollapseImageHeight.ToString());
            }
            if(this.SplitterBarCollapseImageWidth > 0)
            {
              output.AddAttribute(HtmlTextWriterAttribute.Width, this.SplitterBarCollapseImageWidth.ToString());
            }

            output.AddStyleAttribute("cursor", "pointer");

            if(Orientation == SplitterOrientation.Vertical)
            {
              output.AddStyleAttribute("display", "block");
              output.AddStyleAttribute("text-align", "center");
              output.AddStyleAttribute("margin-left", "auto");
              output.AddStyleAttribute("margin-right", "auto");
            }

            output.AddAttribute(HtmlTextWriterAttribute.Alt, "");
            output.AddAttribute(HtmlTextWriterAttribute.Src, Utils.ConvertUrl(Context, ParentSplitter.ImagesBaseUrl, this.SplitterBarCollapseImageUrl));
            output.RenderBeginTag(HtmlTextWriterTag.Img);
            output.RenderEndTag();
          }
          
          // force min height/width with a div
          if(Orientation == SplitterOrientation.Vertical)
          {
            //output.Write("<div style=\"width:1px;height:" + SplitterBarWidth + "px\"></div>");
          }
          else
          {
            output.Write("<div style=\"height:1px;width:" + SplitterBarWidth + "px\"></div>");
          }

          output.RenderEndTag(); // </td>

          if(Orientation == SplitterOrientation.Vertical)
          {
            output.RenderEndTag(); // </tr>
          }
        }
      }

      if(Orientation == SplitterOrientation.Horizontal)
      {
        output.RenderEndTag();
      }

      output.RenderEndTag();
    }

    internal string GetXml()
    {
      StringBuilder oSB = new StringBuilder();

      oSB.Append("<Panes");
      foreach(string sKey in Properties.Keys)
      {
        oSB.Append(" " + sKey + "=\"" + Properties[sKey] + "\"");
      }
      
      oSB.Append(">");
      foreach(SplitterPane pane in this.Panes)
      {
        oSB.Append(pane.GetXml());
      }
      oSB.Append("</Panes>");

      return oSB.ToString();
    }

    internal void LoadXml(XmlNode oNode)
    {
      foreach(XmlAttribute oAttr in oNode.Attributes)
      {
        Properties[oAttr.Name] = oAttr.Value;
      }

      for(int i = 0; i < oNode.ChildNodes.Count; i++)
      {
        XmlNode oSubNode = oNode.ChildNodes[i];

        SplitterPane pane;

        if(i >= this.Count)
        {
          pane = new SplitterPane();
          Panes.Add(pane);
        }
        else
        {
          pane = (SplitterPane)this[i];
        }

        pane.LoadXml(oSubNode);
      }
    }

    #endregion
  }

  /// <summary>
  /// Collection of <see cref="SplitterPane"/> objects.
  /// </summary>
  public class SplitterPaneCollection : CollectionBase
  {
    public new SplitterPane this[int index]
    {
      get
      {
        return (SplitterPane)base.List[index];
      }
      set
      {
        base.List[index] = value;
      }
    }

    public new int Add(SplitterPane template)
    {
      return this.List.Add(template);
    }
  }

  /// <summary>
  /// Specifies which way a <see cref="SplitterPaneGroup"/> is oriented.
  /// </summary>
  public enum SplitterOrientation
  {
    /// <summary>Horizontal orientation - the panes are in a row.</summary>
    Horizontal,

    /// <summary>Vertical orientation - the panes are in a column.</summary>
    Vertical
  }
}
