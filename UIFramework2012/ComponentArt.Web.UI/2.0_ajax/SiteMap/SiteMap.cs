using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Runtime.InteropServices; 


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Arguments for <see cref="SiteMap.NodeDataBound"/> server-side event of <see cref="SiteMap"/> control.
  /// </summary>
  [ToolboxItem(false)]
  public class SiteMapNodeDataBoundEventArgs : EventArgs
  {
    /// <summary>
    /// The SiteMap node.
    /// </summary>
    public SiteMapNode Node;

    /// <summary>
    /// The data item bound to.
    /// </summary>
    public object DataItem;
  }

  #region Enumeration Types

  /// <summary>
  /// Specifies horizontal direction in which <see cref="SiteMap"/> control should order its bread crumbs.
  /// </summary>
  public enum BreadcrumbsDirectionType
  {
    /// <summary>Breadcrumbs are ordered with higher-level nodes on the left and the selected node on the right.</summary>
    LeftToRight,

    /// <summary>Breadcrumbs are ordered with higher-level nodes on the right and the selected node on the left.</summary>
    RightToLeft
  }

  /// <summary>
  /// Specifies the type of layout <see cref="SiteMap"/> control should render.
  /// </summary>
  public enum SiteMapLayoutType
  {
    /// <summary>Nested overview of entire data set.</summary>
    Tree,

    /// <summary>Sectioned listing of entire data set.</summary>
    Directory,

    /// <summary>Select box containing entire data set.</summary>
    DropDown,

    /// <summary>Entire data set spread across a few select boxes.</summary>
    DropDownDirectory,

    /// <summary>Navigational drill-down to the <see cref="SiteMap.SelectedNode"/>.</summary>
    Breadcrumbs
  }

  /// <summary>
  /// Specifies order in which data within a <see cref="SiteMap"/> table is organized.
  /// </summary>
  public enum TableFillDirectionType
  {
    /// <summary>Table is filled row by row.</summary>
    AcrossRows,

    /// <summary>Table is filled column by column.</summary>
    DownColumns
  }


  #endregion

  /// <summary>
  /// Displays a customizable listing of contents of hirerachical data.
  /// </summary>
  /// <remarks>
  /// <para>
  /// The SiteMap control produces a visual representation of hierarchical data, typically a site map loaded from an
  /// XML file specified using the <see cref="BaseNavigator.SiteMapXmlFile" /> property.
  /// </para>
  /// <para>
  /// The type of layout is selected using the <see cref="SiteMapLayout" /> property. The <see cref="Table" /> property can be used
  /// to define a table-like layout for breaking up the site map into separate cells.
  /// </para>
  /// </remarks>
  [GuidAttribute("978e25d0-9c37-4791-a350-16fb4a13be32")]
  [LicenseProviderAttribute(typeof(ComponentArt.Licensing.Providers.RedistributableLicenseProvider))]
  [ToolboxData("<{0}:SiteMap Width=200 Height=400 runat=server></{0}:SiteMap>")]
  [PersistChildren(false)]
  [ParseChildren(true)]
  [Designer(typeof(ComponentArt.Web.UI.SiteMapNodesDesigner))]
  public sealed class SiteMap : BaseNavigator
  {
    #region Public Properties

    /// <summary>
    /// The direction in which the rendered Breadcrumbs read.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(BreadcrumbsDirectionType.LeftToRight)]
    [Description("The direction in which the rendered Breadcrumbs read. Default is LeftToRight.")]
    public BreadcrumbsDirectionType BreadcrumbsDirection
    {
      get 
      {
        object o = ViewState["BreadcrumbsDirection"];
        return o == null? BreadcrumbsDirectionType.LeftToRight : (BreadcrumbsDirectionType)Enum.Parse(typeof(BreadcrumbsDirectionType), o.ToString(), true);
      }
      set 
      {
        ViewState["BreadcrumbsDirection"] = value.ToString();
      }
    }

    /// <summary>
    /// How many levels of the path to render for Breadcrumbs functionality. Default is 0 (no limit).
    /// </summary>
    [Description("How many levels of the path to render for Breadcrumbs functionality.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int BreadcrumbsLevelsToDisplay
    {
      get 
      {
        object o = ViewState["BreadcrumbsLevelsToDisplay"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set 
      {
        ViewState["BreadcrumbsLevelsToDisplay"] = value;
      }
    }

    /// <summary>
    /// HTML to render between nodes for Breadcrumbs.
    /// </summary>
    [Description("HTML to render between nodes for Breadcrumbs.")]
    [Category("Appearance")]
    [DefaultValue("&nbsp;|&nbsp;")]
    public string BreadcrumbsSeparatorString
    {
      get 
      {
        object o = ViewState["BreadcrumbsSeparatorString"]; 
        return (o == null) ? "&nbsp;|&nbsp;" : (string) o; 
      }
      set 
      {
        ViewState["BreadcrumbsSeparatorString"] = value;
      }
    }

    /// <summary>
    /// ID of template to render between nodes for Breadcrumbs.
    /// </summary>
    [Description("ID of template to render between nodes for Breadcrumbs.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string BreadcrumbsSeparatorTemplateId
    {
      get 
      {
        object o = ViewState["BreadcrumbsSeparatorTemplateId"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        ViewState["BreadcrumbsSeparatorTemplateId"] = value;
      }
    }

    /// <summary>
    /// CssClass to use for child (non-root) nodes.
    /// </summary>
    [Description("CssClass to use for child (non-root) nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ChildNodeCssClass
    {
      get 
      {
        object o = ViewState["ChildNodeCssClass"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        ViewState["ChildNodeCssClass"] = value;
      }
    }

    /// <summary>
    /// The default height (in pixels) of images.
    /// </summary>
    [Description("The default height (in pixels) of images.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int DefaultImageHeight
    {
      get 
      {
        object o = ViewState["DefaultImageHeight"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set 
      {
        ViewState["DefaultImageHeight"] = value;
      }
    }

    /// <summary>
    /// The default width (in pixels) of images.
    /// </summary>
    [Description("The default width (in pixels) of images.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int DefaultImageWidth
    {
      get 
      {
        object o = ViewState["DefaultImageWidth"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set 
      {
        ViewState["DefaultImageWidth"] = value;
      }
    }

    /// <summary>
    /// HTML to render after lists of entries in a Directory.
    /// </summary>
    [Description("HTML to render after lists of entries in a Directory.")]
    [Category("Appearance")]
    [DefaultValue("<br /><br />")]
    public string DirectoryFooterString
    {
      get 
      {
        object o = ViewState["DirectoryFooterString"]; 
        return (o == null) ? "<br /><br />" : (string) o; 
      }
      set 
      {
        ViewState["DirectoryFooterString"] = value;
      }
    }

    /// <summary>
    /// HTML to render before lists of entries in a Directory.
    /// </summary>
    [Description("HTML to render before lists of entries in a Directory.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string DirectoryHeaderString
    {
      get 
      {
        object o = ViewState["DirectoryHeaderString"]; 
        return (o == null) ? "" : (string) o; 
      }
      set 
      {
        ViewState["DirectoryHeaderString"] = value;
      }
    }

    /// <summary>
    /// HTML to use for separating entries in a Directory.
    /// </summary>
    [Description("HTML to use for separating entries in a Directory.")]
    [Category("Appearance")]
    [DefaultValue("&nbsp;|&nbsp;")]
    public string DirectorySeparatorString
    {
      get 
      {
        object o = ViewState["DirectorySeparatorString"]; 
        return (o == null) ? "&nbsp;|&nbsp;" : (string) o; 
      }
      set 
      {
        ViewState["DirectorySeparatorString"] = value;
      }
    }

    /// <summary>
    /// CssClass to use for DropDown (SELECT) elements.
    /// </summary>
    [Description("CssClass to use for DropDown (SELECT) elements.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string DropDownCssClass
    {
      get 
      {
        object o = ViewState["DropDownCssClass"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        ViewState["DropDownCssClass"] = value;
      }
    }

    /// <summary>
    /// HTML to use for indenting each level of a DropDown.
    /// </summary>
    [Description("HTML to use for indenting each level of a DropDown.")]
    [Category("Appearance")]
    [DefaultValue("&nbsp;&nbsp;")]
    public string DropDownIndentString
    {
      get 
      {
        object o = ViewState["DropDownIndentString"]; 
        return (o == null) ? "&nbsp;&nbsp;" : (string) o; 
      }
      set 
      {
        ViewState["DropDownIndentString"] = value;
      }
    }

    /// <summary>
    /// CssClass to use for leaf nodes.
    /// </summary>
    [Description("CssClass to use for leaf nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string LeafNodeCssClass
    {
      get 
      {
        object o = ViewState["LeafNodeCssClass"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        ViewState["LeafNodeCssClass"] = value;
      }
    }

    /// <summary>
    /// Default image to use for leaf nodes.
    /// </summary>
    [Description("Default image to use for leaf nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string LeafNodeImageUrl
    {
      get 
      {
        object o = ViewState["LeafNodeImageUrl"]; 
        return (o == null) ? string.Empty : ConvertImageUrl((string)o); 
      }
      set 
      {
        ViewState["LeafNodeImageUrl"] = value;
      }
    }

    /// <summary>
    /// Prefix to prepend to the labels of leaf nodes.
    /// </summary>
    [Description("Prefix to prepend to the labels of leaf nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string LeafNodePrefix
    {
      get 
      {
        object o = ViewState["LeafNodePrefix"]; 
        return (o == null) ? string.Empty : (string)o; 
      }
      set 
      {
        ViewState["LeafNodePrefix"] = value;
      }
    }

    /// <summary>
    /// Padding to include between a node's icon and its label (in pixels). Default: 0.
    /// </summary>
    [Description("Padding to include between a node's icon and its label (in pixels). Default: 0.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int NodeLabelPadding
    {
      get 
      {
        object o = ViewState["NodeLabelPadding"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set 
      {
        ViewState["NodeLabelPadding"] = value;
      }
    }

    /// <summary>
    /// Width (in pixels) to indent each level of the SiteMap. Default: 16.
    /// </summary>
    [Description("Width (in pixels) to indent each level of the SiteMap. Default: 16.")]
    [Category("Layout")]
    [DefaultValue(16)]
    public int NodeIndent
    {
      get 
      {
        object o = ViewState["NodeIndent"]; 
        return (o == null) ? 16 : (int) o; 
      }
      set 
      {
        ViewState["NodeIndent"] = value;
      }
    }

    /// <summary>
    /// Collection of root SiteMapNodes.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    [Browsable(false)]
    public SiteMapNodeCollection Nodes
    {
      get
      {
        return (SiteMapNodeCollection)nodes;
      }
    }

    /// <summary>
    /// CssClass to use for parent nodes.
    /// </summary>
    [Description("CssClass to use for parent nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ParentNodeCssClass
    {
      get 
      {
        object o = ViewState["ParentNodeCssClass"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        ViewState["ParentNodeCssClass"] = value;
      }
    }

    /// <summary>
    /// Default image to use for parent nodes.
    /// </summary>
    [Description("Default image to use for parent nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string ParentNodeImageUrl
    {
      get 
      {
        object o = ViewState["ParentNodeImageUrl"]; 
        return (o == null) ? string.Empty : ConvertImageUrl((string)o); 
      }
      set 
      {
        ViewState["ParentNodeImageUrl"] = value;
      }
    }

    /// <summary>
    /// ID of node to begin rendering down from.
    /// </summary>
    [Description("ID of node to begin rendering down from.")]
    [DefaultValue("")]
    [Category("Data")]
    public new string RenderRootNodeId
    {
      get 
      {
        return base.RenderRootNodeId;
      }
      set 
      {
        base.RenderRootNodeId = value;
      }
    }

    /// <summary>
    /// Whether to include the RenderRootNode when rendering, instead of only its children. Default: false.
    /// </summary>
    [DefaultValue(false)]
    [Description("Whether to include the RenderRootNode when rendering, instead of only its children. Default: false.")]
    [Category("Data")]
    public new bool RenderRootNodeInclude
    {
      get 
      {
        return base.RenderRootNodeInclude;
      }
      set 
      {
        base.RenderRootNodeInclude = value;
      }
    }

    /// <summary>
    /// CssClass to use for root nodes.
    /// </summary>
    [Description("CssClass to use for root nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string RootNodeCssClass
    {
      get 
      {
        object o = ViewState["RootNodeCssClass"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        ViewState["RootNodeCssClass"] = value;
      }
    }

    /// <summary>
    /// Default image to use for root nodes.
    /// </summary>
    [Description("Default image to use for root nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string RootNodeImageUrl
    {
      get 
      {
        object o = ViewState["RootNodeImageUrl"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        ViewState["RootNodeImageUrl"] = value;
      }
    }

    /// <summary>
    /// Template to use for root nodes.
    /// </summary>
    [Description("Template to use for root nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string RootNodeTemplateId
    {
      get 
      {
        object o = ViewState["RootNodeTemplateId"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        ViewState["RootNodeTemplateId"] = value;
      }
    }

    /// <summary>
    /// The selected node. This can be set on the server-side to force a node selection.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SiteMapNode SelectedNode
    {
      get
      {
        return (SiteMapNode)(base.selectedNode);
      }
      set
      {
        base.selectedNode = value;

        if(value != null)
        {
          base.selectedNodePostbackId = value.PostBackID;
        }
      }
    }

    /// <summary>
    /// CssClass to use for the selected node.
    /// </summary>
    [Description("CssClass to use for the selected node.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string SelectedNodeCssClass
    {
      get 
      {
        object o = ViewState["SelectedNodeCssClass"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        ViewState["SelectedNodeCssClass"] = value;
      }
    }

    /// <summary>
    /// The type of layout this SiteMap should render.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(SiteMapLayoutType.Directory)]
    [Description("The type of layout this SiteMap should render.")]
    public SiteMapLayoutType SiteMapLayout
    {
      get 
      {
        object o = ViewState["SiteMapLayout"];
        return o == null? SiteMapLayoutType.Directory : (SiteMapLayoutType)Enum.Parse(typeof(SiteMapLayoutType), o.ToString(), true);
      }
      set 
      {
        ViewState["SiteMapLayout"] = value.ToString();
      }
    }

    private SiteMapTable _table;
    /// <summary>
    /// Table defining the layout of this SiteMap.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public SiteMapTable Table
    {
      get
      {
        if(_table == null)
        {
          _table = new SiteMapTable();
          _table.ParentSiteMap = this;
        }

        return _table;
      }
    }

    /// <summary>
    /// The direction in which to fill the Table layout SiteMap.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(TableFillDirectionType.AcrossRows)]
    [Description("The direction in which to fill the Table layout SiteMap.")]
    public TableFillDirectionType TableFillDirection
    {
      get 
      {
        object o = ViewState["TableFillDirection"];
        return o == null? TableFillDirectionType.AcrossRows : (TableFillDirectionType)Enum.Parse(typeof(TableFillDirectionType), o.ToString(), true);
      }
      set 
      {
        ViewState["TableFillDirection"] = value.ToString();
      }
    }

    /// <summary>
    /// The bullet image to render before tree nodes.
    /// </summary>
    [Description("The bullet image to render before tree nodes.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string TreeBulletImageUrl
    {
      get 
      {
        object o = ViewState["TreeBulletImageUrl"]; 
        return (o == null) ? string.Empty : ConvertImageUrl((string)o); 
      }
      set 
      {
        ViewState["TreeBulletImageUrl"] = value;
      }
    }

    /// <summary>
    /// HTML to render after every Tree.
    /// </summary>
    [Description("HTML to render after every Tree.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string TreeFooterString
    {
      get 
      {
        object o = ViewState["TreeFooterString"]; 
        return (o == null) ? "" : (string) o; 
      }
      set 
      {
        ViewState["TreeFooterString"] = value;
      }
    }

    /// <summary>
    /// HTML to render before every Tree.
    /// </summary>
    [Description("HTML to render before every Tree.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string TreeHeaderString
    {
      get 
      {
        object o = ViewState["TreeHeaderString"]; 
        return (o == null) ? "" : (string) o; 
      }
      set 
      {
        ViewState["TreeHeaderString"] = value;
      }
    }

    /// <summary>
    /// The height (in pixels) of line images for the Tree layout.
    /// </summary>
    /// <seealso cref="TreeShowLines" />
    /// <seealso cref="TreeLineImagesFolderUrl" />
    [Description("The height (in pixels) of line images for the Tree layout.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int TreeLineImageHeight
    {
      get 
      {
        object o = ViewState["TreeLineImageHeight"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set 
      {
        ViewState["TreeLineImageHeight"] = value;
      }
    }

    /// <summary>
    /// The width (in pixels) of line images for the Tree layout.
    /// </summary>
    /// <seealso cref="TreeShowLines" />
    /// <seealso cref="TreeLineImagesFolderUrl" />
    [Description("The width (in pixels) of line images for the Tree layout.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int TreeLineImageWidth
    {
      get 
      {
        object o = ViewState["TreeLineImageWidth"]; 
        return (o == null) ? 0 : (int) o; 
      }
      set 
      {
        ViewState["TreeLineImageWidth"] = value;
      }
    }

    /// <summary>
    /// The folder in which line images for trees are contained.
    /// </summary>
    [Description("The folder in which line images for trees are contained.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string TreeLineImagesFolderUrl
    {
      get 
      {
        object o = ViewState["TreeLineImagesFolderUrl"]; 
        return (o == null) ? string.Empty : (string) o; 
      }
      set 
      {
        ViewState["TreeLineImagesFolderUrl"] = value;
      }
    }

    /// <summary>
    /// Whether to render lines for trees.
    /// </summary>
    /// <seealso cref="TreeLineImagesFolderUrl" />
    [Category("Layout")]
    [DefaultValue(false)]
    [Description("Whether to render lines for trees.")]
    public bool TreeShowLines
    {
      get 
      {
        object o = ViewState["TreeShowLines"];
        return o == null? false : (bool)o;
      }
      set 
      {
        ViewState["TreeShowLines"] = value;
      }
    }

    #endregion

    #region Public Methods

    public SiteMap()
    {
      this.nodes = new SiteMapNodeCollection(this, null);
    }

    /// <summary>
    /// Find the SiteMapNode with the given ID.
    /// </summary>
    /// <param name="sNodeID">The ID to search for.</param>
    /// <returns>The found node or null.</returns>
    public new SiteMapNode FindNodeById(string sNodeID)
    {
      return (SiteMapNode)base.FindNodeById(sNodeID);
    }

    #endregion

    #region Protected Methods

    protected override NavigationNode AddNode()
    {
      SiteMapNode oNewNode = new SiteMapNode();
      this.Nodes.Add(oNewNode);
      return oNewNode;
    }
    
    protected override NavigationNode NewNode()
    {
      SiteMapNode newNode = new SiteMapNode();
      SiteMapNodeCollection dummy = newNode.Nodes; // This is a dummy call to ensure that newNode.nodes is not null
      return newNode;
    }

    protected override void ComponentArtRender(HtmlTextWriter output)
    {
      base.ComponentArtRender(output);

      // Make sure there is at least one cell.
      if(this.Table.Rows.Count == 0 || this.Table.Rows[0].Cells.Count == 0)
      {
        this.Table.Rows.Clear();
        this.Table.Rows.Add(new SiteMapTableRow());
        this.Table.Rows[0].Cells.Add(new SiteMapTableCell());
      }
      
      if(this.SiteMapLayout == SiteMapLayoutType.Breadcrumbs)
      {
        if(this.SelectedNode != null)
        {
          this.Table.Rows[0].Cells[0].Controls.Add(this.SelectedNode);
        }
      }
      else
      {
        // stick nodes into their table cells
        int iIndex = 0;

        if(TableFillDirection == TableFillDirectionType.AcrossRows)
        {
          for(int iRow = 0; iRow < this.Table.Rows.Count; iRow++)
          {
            SiteMapTableRow oRow = (SiteMapTableRow)this.Table.Rows[iRow];

            for(int iColumn = 0; iColumn < oRow.Cells.Count; iColumn++)
            {
              SiteMapTableCell oCell = (SiteMapTableCell)oRow.Cells[iColumn];

              int iHowMany = oCell.RootNodes >= 0? oCell.RootNodes : int.MaxValue;

              SiteMapNodeCollection arNodes = GetRootNodes(oCell);

              SiteMapNode oLastNode = null;

              for(int i = 0; i < iHowMany && iIndex < arNodes.Count; i++)
              {
                oLastNode = arNodes[iIndex];

                oCell.Controls.Add(oLastNode);

                iIndex++;
              }

              // tell the last one it's the last
              if(oLastNode != null)
              {
                oLastNode.LastInGroup = true;
              }
            }
          }
        }
        else  // fill columns first
        {
          // we'll assume that there are as many columns as the first row has.
          for(int iColumn = 0; iColumn < this.Table.Rows[0].Cells.Count; iColumn++)
          {
            for(int iRow = 0; iRow < this.Table.Rows.Count; iRow++)
            {
              SiteMapTableRow oRow = (SiteMapTableRow)this.Table.Rows[iRow];

              // only do something if there is such a column in this row
              if(oRow.Cells.Count > iColumn)
              {
                SiteMapTableCell oCell = (SiteMapTableCell)oRow.Cells[iColumn];

                int iHowMany = oCell.RootNodes >= 0? oCell.RootNodes : int.MaxValue;

                SiteMapNodeCollection arNodes = GetRootNodes(oCell);
                
                SiteMapNode oLastNode = null;

                for(int i = 0; i < iHowMany && iIndex < arNodes.Count; i++)
                {
                  oLastNode = arNodes[iIndex];

                  oCell.Controls.Add(oLastNode);

                  iIndex++;
                }

                // tell the last one it's the last
                if(oLastNode != null)
                {
                  oLastNode.LastInGroup = true;
                }
              }
            }
          }
        }
      }

      AddAttributesToRender(output);

      if (SiteMapLayout == SiteMapLayoutType.Breadcrumbs)
      {
        //this.Table.CellPadding = 0;
        this.Table.CellSpacing = 0;
      }

      this.Table.RenderControl(output);
    }

    protected override bool IsDownLevel()
    {
      return false;
    }

    #endregion

    #region Private Methods

    private SiteMapNodeCollection GetRootNodes(SiteMapTableCell oCell)
    {
      SiteMapNodeCollection arNodes = null;

      string sRenderRootNodeId = oCell.RenderRootNodeId;
      bool bRenderRootNodeInclude = oCell.RenderRootNodeInclude;

      // Determine which nodes we need to render
      if(sRenderRootNodeId != string.Empty )
      {
        SiteMapNode oRootNode = this.FindNodeById(sRenderRootNodeId);

        if(oRootNode == null)
        {
          throw new Exception("No node found with ID \"" + sRenderRootNodeId + "\".");
        }
        else
        {
          if(bRenderRootNodeInclude)
          {
            SiteMapNodeCollection oRootNodes = new SiteMapNodeCollection(this, null);
            oRootNodes.Add(oRootNode);
            arNodes = oRootNodes;
          }
          else
          {
            arNodes = oRootNode.Nodes;	
          }
        }
      }
      else
      {
        arNodes = this.Nodes;
      }

      // For DropDown, make sure we have only one root!
      if(this.SiteMapLayout == SiteMapLayoutType.DropDown)
      {
        SiteMapNode oRoot = new SiteMapNode();
        SiteMapNodeCollection oRootNodes = new SiteMapNodeCollection(this, null);
        oRootNodes.Add(oRoot);

        oRoot.nodes = arNodes;
        arNodes = oRootNodes;
      }

      return arNodes;
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Delegate for <see cref="NodeDataBound"/> event of <see cref="SiteMap"/> class.
    /// </summary>
    public delegate void NodeDataBoundEventHandler(object sender, SiteMapNodeDataBoundEventArgs e);
		
    /// <summary>
    /// Fires after a SiteMap node is data bound.
    /// </summary>
    [ Description("Fires after a SiteMap node is data bound."),
    Category("SiteMapNode Events") ]
    public event NodeDataBoundEventHandler NodeDataBound;

    // generic trigger
    protected override void OnNodeDataBound(NavigationNode oNode, object oDataItem) 
    {
      if (NodeDataBound != null) 
      {
        SiteMapNodeDataBoundEventArgs e = new SiteMapNodeDataBoundEventArgs();
        
        e.Node = (SiteMapNode)oNode;
        e.DataItem = oDataItem;

        NodeDataBound(this, e);
      }   
    }

    #endregion
  }
}
