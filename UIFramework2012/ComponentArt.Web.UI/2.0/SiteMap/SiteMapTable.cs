using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// A <see cref="Table"/> organizing the contents within the <see cref="SiteMap"/> control.
	/// </summary>
	[ToolboxItem(false)]
  [ParseChildren(true, "Rows")]
  public class SiteMapTable : Table
  {
    private SiteMap _parentSiteMap;
    /// <summary>
    /// The SiteMap that this table belongs to.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SiteMap ParentSiteMap
    {
      get
      {
        return _parentSiteMap;
      }
      set
      {
        _parentSiteMap = value;
      }
    }

    protected override void Render(HtmlTextWriter output)
    {
      AddAttributesToRender(output);
      output.RenderBeginTag(HtmlTextWriterTag.Table);

      foreach(SiteMapTableRow oRow in this.Rows)
      {
        oRow.RenderControl(output);
      }

      output.RenderEndTag();
    }
  }

  /// <summary>
  /// A <see cref="TableCell"/> organizing the contents within the <see cref="SiteMap"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class SiteMapTableCell : TableCell
  {
    /// <summary>
    /// The SiteMap that this cell belongs to.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SiteMap ParentSiteMap
    {
      get
      {
        Control oControl = this;
        while(oControl.Parent != null)
        {
          oControl = oControl.Parent;
          if(oControl is SiteMapTable)
          {
            return ((SiteMapTable)oControl).ParentSiteMap;
          }
        }

        return null;
      }
    }

    /// <summary>
    /// Whether to include the specified root node when rendering.
    /// </summary>
    /// <seealso cref="RenderRootNodeId" />
    [Category("Layout")]
    [DefaultValue(false)]
    [Description("Whether to include the specified root node when rendering.")]
    public bool RenderRootNodeInclude
    {
      get 
      {
        object o = ViewState["RenderRootNodeInclude"];
        return o == null? (ParentSiteMap != null? ParentSiteMap.RenderRootNodeInclude : false ): (bool)o;
      }
      set 
      {
        ViewState["RenderRootNodeInclude"] = value;
      }
    }

    /// <summary>
    /// The ID of the SiteMapNode to begin rendering from, for this cell.
    /// </summary>
    [Category("Layout")]
    [DefaultValue("")]
    [Description("The ID of the SiteMapNode to begin rendering from, for this cell.")]
    public string RenderRootNodeId
    {
      get 
      {
        object o = ViewState["RenderRootNodeId"];
        return o == null? (ParentSiteMap != null? ParentSiteMap.RenderRootNodeId : "" ): (string)o;
      }
      set 
      {
        ViewState["RenderRootNodeId"] = value;
      }
    }

    /// <summary>
    /// How many SiteMap root nodes and their substructures should go into this cell? Default: -1.
    /// A negative value will result in all the available roots being inserted here.
    /// </summary>
    [Description("How many SiteMap root nodes and their substructures should go into this cell? Default: -1.")]
    [Category("Layout")]
    [DefaultValue(-1)]
    public int RootNodes
    {
      get 
      {
        object o = ViewState["RootNodes"]; 
        return (o == null) ? -1 : (int) o; 
      }
      set 
      {
        ViewState["RootNodes"] = value;
      }
    }

    /// <summary>
    /// The type of SiteMap layout to be used for this cell.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(SiteMapLayoutType.Tree)]
    [Description("The type of layout this SiteMap should render.")]
    public SiteMapLayoutType SiteMapLayout
    {
      get 
      {
        object o = ViewState["SiteMapLayout"];
        return o == null? (ParentSiteMap != null? ParentSiteMap.SiteMapLayout : SiteMapLayoutType.Tree ): 
          (SiteMapLayoutType)Enum.Parse(typeof(SiteMapLayoutType), o.ToString(), true);
      }
      set 
      {
        ViewState["SiteMapLayout"] = value.ToString();
      }
    }

    protected override void AddParsedSubObject(object o)
    {
      if(o is Literal)
      {
        this.Controls.Add(new LiteralControl(o.ToString()));
      }
      else if(o is Control)
      {
        this.Controls.Add((Control)o);
      }
    }
  }

  /// <summary>
  /// A <see cref="TableRow"/> organizing the contents within the <see cref="SiteMap"/> control.
  /// </summary>
  [ToolboxItem(false)]
  [ParseChildren(true, "Cells")]
  public class SiteMapTableRow : TableRow
  {
    protected override void Render(HtmlTextWriter output)
    {
      AddAttributesToRender(output);
      output.RenderBeginTag(HtmlTextWriterTag.Tr);

      foreach(SiteMapTableCell oCell in this.Cells)
      {
        oCell.RenderControl(output);
      }

      output.RenderEndTag();
    }
  }
}
