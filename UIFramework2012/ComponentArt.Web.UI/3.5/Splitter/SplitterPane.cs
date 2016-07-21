using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text;
using System.Xml;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Represents a single pane in the layout of a <see cref="Splitter"/> control.
  /// </summary>
  [ParseChildren(true)]
  [ToolboxItem(false)]
  public class SplitterPane : System.Web.UI.WebControls.WebControl
  {
    #region Private Properties

    internal SplitterPaneGroup ParentGroup;

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

    #endregion

    #region Public Properties

    /// <summary>
    /// Whether to allow the resizing of this pane. Default: true.
    /// </summary>
    [DefaultValue(true)]
    public bool AllowResizing
    {
      get
      {
        return Utils.ParseBool(Properties["AllowResizing"], true);
      }
      set
      {
        Properties["AllowResizing"] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to allow the resizing of this pane. Default: false.
    /// </summary>
    [DefaultValue(false)]
    public bool AllowScrolling
    {
      get
      {
        return Utils.ParseBool(Properties["AllowScrolling"], false);
      }
      set
      {
        Properties["AllowScrolling"] = value.ToString();
      }
    }

    private SplitterPaneClientEvents _clientEvents = null;
    /// <summary>
    /// Client event handler definitions.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Description("Client event handler definitions.")]
    [Category("Client events")]
    public SplitterPaneClientEvents ClientEvents
    {
      get
      {
        if (_clientEvents == null)
        {
          _clientEvents = new SplitterPaneClientEvents();
        }
        return _clientEvents;
      }
    }

    /// <summary>
    /// Client-side handler for the resize event of this SplitterPane
    /// EventArgs: pane id, height, width
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [DefaultValue("")]
    public string ClientSideOnResize
    {
      get
      {
        return (string)Properties["ClientSideOnResize"];
      }
      set
      {
        Properties["ClientSideOnResize"] = value;
      }
    }

    /// <summary>
    /// Whether this pane is collapsed. Default: false.
    /// </summary>
    [DefaultValue(false)]
    public bool Collapsed
    {
      get
      {
        return Utils.ParseBool(Properties["Collapsed"], false);
      }
      set
      {
        Properties["Collapsed"] = value.ToString();
      }
    }

    /// <summary>
    /// The CSS class to use for this pane.
    /// </summary>
    [DefaultValue("")]
    public new string CssClass
    {
      get
      {
        return (string)Properties["CssClass"];
      }
      set
      {
        Properties["CssClass"] = value;
      }
    }
    
    /// <summary>
    /// The height of this pane.
    /// </summary>
    public new Unit Height
    {
      get
      {
        return Utils.ParseUnit(Properties["Height"]);
      }
      set
      {
        Properties["Height"] = value.ToString();
      }
    }

    /// <summary>
    /// The ID of this pane.
    /// </summary>
    //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new string ID
    {
      get
      {
        return (string)Properties["ID"];
      }
      set
      {
        Properties["ID"] = value;
      }
    }

    /// <summary>
    /// Minimum height to allow for this pane, in pixels.
    /// </summary>
    [DefaultValue(0)]
    public int MinHeight
    {
      get
      {
        return Utils.ParseInt(Properties["MinHeight"], 0);
      }
      set
      {
        Properties["MinHeight"] = value.ToString();
      }
    }

    /// <summary>
    /// Minimum width to allow for this pane, in pixels.
    /// </summary>
    [DefaultValue(0)]
    public int MinWidth
    {
      get
      {
        return Utils.ParseInt(Properties["MinWidth"], 0);
      }
      set
      {
        Properties["MinWidth"] = value.ToString();
      }
    }

    /// <summary>
    /// The ID of the SplitterPaneContent object from which to load content for this pane.
    /// </summary>
    [DefaultValue("")]
    public string PaneContentId
    {
      get
      {
        return (string)Properties["PaneContentId"];
      }
      set
      {
        Properties["PaneContentId"] = value;
      }
    }

    private SplitterPaneGroup _panes;
    /// <summary>
    /// The collection of SplitterPanes contained within this one.
    /// </summary>
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public SplitterPaneGroup Panes
    {
      get
      {
        return _panes;
      }
      set
      {
        _panes = value;
      }
    }

    /// <summary>
    /// The width of this pane.
    /// </summary>
    public new Unit Width
    {
      get
      {
        return Utils.ParseUnit(Properties["Width"]);
      }
      set
      {
        Properties["Width"] = value.ToString();
      }
    }

    #endregion

    #region Methods
    
    protected override void Render(HtmlTextWriter output)
    {
      if(Panes != null)
      {
        //Panes.Height = Unit.Percentage(100);
        //Panes.Width = Unit.Percentage(100);
        output.AddStyleAttribute("height", "100%");
        output.AddStyleAttribute("width", "100%");
        Panes.RenderControl(output);
      }
      else 
      {
        if(PaneContentId != null && PaneContentId != "")
        {
          SplitterPaneContent oContent = ParentGroup.ParentSplitter.FindPaneContentById(this.PaneContentId);

          if(oContent == null)
          {
            throw new Exception("PaneContent '" + PaneContentId + "' not found.");
          }
          
          bool bRenderDiv = true; /*
            (ParentGroup.Orientation == SplitterOrientation.Vertical && !Height.IsEmpty) ||
            (ParentGroup.Orientation == SplitterOrientation.Horizontal && !Width.IsEmpty);
*/
          if(bRenderDiv)
          {
            if(ParentGroup.Orientation == SplitterOrientation.Vertical)
            {
              if(!Height.IsEmpty && Height.Type == UnitType.Pixel)
              {
                output.AddStyleAttribute("height", Height.ToString());
              }
              else
              {
                output.AddStyleAttribute("height", "100%");
              }

              output.AddStyleAttribute("width", "100%");
            }
            else
            {
              if(!Width.IsEmpty && Width.Type == UnitType.Pixel)
              {
                output.AddStyleAttribute("width", Width.ToString());
              }
              else
              {
                output.AddStyleAttribute("width", "100%");
              }

              // make sure this doesn't stretch anything!
              output.AddStyleAttribute("height", "1px");
            }
          
            if(this.AllowScrolling)
            {
              output.AddStyleAttribute("overflow", "auto");
            }
            else
            {
              output.AddStyleAttribute("overflow", "hidden");
            }
            output.RenderBeginTag(HtmlTextWriterTag.Div); // <div>
          }

          oContent.RenderControl(output);

          if(bRenderDiv)
          {
            output.RenderEndTag(); // </div>
          }
        }
      }
    }

    internal string GetXml()
    {
      StringBuilder oSB = new StringBuilder();
      oSB.Append("<Pane");

      foreach(string sKey in Properties.Keys)
      {
        oSB.Append(" " + sKey + "=\"" + Properties[sKey] + "\"");
      }
      
      oSB.Append(">");
      if(Panes != null && Panes.Count > 0)
      {
        oSB.Append(Panes.GetXml());
      }
      oSB.Append("</Pane>");

      return oSB.ToString();
    }

    internal void LoadXml(XmlNode oNode)
    {
      foreach(XmlAttribute oAttr in oNode.Attributes)
      {
        Properties[oAttr.Name] = oAttr.Value;
      }

      if(oNode.ChildNodes.Count > 0)
      {
        this.Panes = new SplitterPaneGroup();
        this.Panes.LoadXml(oNode.FirstChild);
      }
    }

    #endregion
  }
}
