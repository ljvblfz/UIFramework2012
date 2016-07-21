using System;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;

namespace ComponentArt.Web.UI
{
	/// <summary>
  /// Navigation node class for the <see cref="SiteMap"/> control. 
	/// </summary>
  [ToolboxItem(false)]
  [ParseChildren(true, "Nodes")]
  public class SiteMapNode : NavigationNode
  {
    private SiteMapLayoutType _renderLayout;

    private bool _lastInGroup = false;
    internal bool LastInGroup
    {
      get
      {
        if (!_lastInGroup)
        {
          return (this.NextSibling == null);
        }

        return true;
      }
      set
      {
        _lastInGroup = value;
      }
    }

    #region Public Properties

    /// <summary>
    /// Height (in pixels) of this node's Image.
    /// </summary>
    [Description("Height (in pixels) of this node's Image.")]
    [DefaultValue(0)]
    [Category("Layout")]
    public int ImageHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("ImageHeight")]; 
        return (o == null) ?
          (ParentSiteMap != null? ParentSiteMap.DefaultImageHeight : 0) :
          int.Parse(o); 
      }
      set 
      {
        Properties[GetAttributeVarName("ImageHeight")] = value.ToString();
      }
    }

    /// <summary>
    /// Width (in pixels) of this node's Image.
    /// </summary>
    [Description("Width (in pixels) of this node's Image.")]
    [DefaultValue(0)]
    [Category("Layout")]
    public int ImageWidth
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("ImageWidth")]; 
        return (o == null) ?
          (ParentSiteMap != null? ParentSiteMap.DefaultImageWidth : 0) :
          int.Parse(o); 
      }
      set 
      {
        Properties[GetAttributeVarName("ImageWidth")] = value.ToString();
      }
    }

    /// <summary>
    /// Icon to use for this node.
    /// </summary>
    [Description("Icon to use for this node.")]
    [DefaultValue("")]
    [Category("Appearance")]
    public string ImageUrl
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("ImageUrl")]; 
        return (o == null) ? string.Empty : (ParentSiteMap == null ? o : ParentSiteMap.ConvertImageUrl(o)); 
      }
      set 
      {
        Properties[GetAttributeVarName("ImageUrl")] = value;
      }
    }

    /// <summary>
    /// Whether to include this node in the SiteMap.
    /// </summary>
    [Description("Whether to include this node in the SiteMap.")]
    [DefaultValue(true)]
    [Category("Data")]
    public bool IncludeInSiteMap
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("IncludeInSiteMap")]; 
        return (o == null) ? true : bool.Parse(o); 
      }
      set 
      {
        Properties[GetAttributeVarName("IncludeInSiteMap")] = value.ToString();
      }
    }

    /// <summary>
    /// The following node in this node's group.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SiteMapNode NextSibling
    {
      get
      {
        return (SiteMapNode)(this.nextSibling);
      }
    }

    /// <summary>
    /// The collection of this node's child nodes.
    /// </summary>
    [Description("The collection of this node's child nodes.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    [Browsable(false)]
    public SiteMapNodeCollection Nodes
    {
      get
      {
        if(this.nodes == null)
        {
          nodes = new SiteMapNodeCollection(ParentSiteMap, this);
        }

        return (SiteMapNodeCollection)nodes;
      }
    }

    /// <summary>
    /// This node's parent node.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SiteMapNode ParentNode
    {
      get
      {
        return (SiteMapNode)(this.parentNode);
      }
    }

    /// <summary>
    /// The SiteMap that this node belongs to.
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
        return (SiteMap)(this.navigator);
      }
    }

    /// <summary>
    /// The node preceding this one in its group.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SiteMapNode PreviousSibling
    {
      get
      {
        return (SiteMapNode)(this.previousSibling);
      }
    }

    #endregion

    #region General Methods
 
    public SiteMapNode()
    {
    }

    internal override NavigationNode AddNode()
    {
      SiteMapNode oNewNode = new SiteMapNode();
      this.Nodes.Add(oNewNode);
      return oNewNode;
    }

    // Prevent ViewState-saving of nodes as child controls.
    protected override object SaveViewState()
    {
      return null;
    }

    protected override void Render(HtmlTextWriter output)
    {
      if(this.ParentSiteMap != null)
      {
        _renderLayout = (this.Parent is SiteMapTableCell? ((SiteMapTableCell)(this.Parent)).SiteMapLayout : ParentSiteMap.SiteMapLayout);
      
        switch(_renderLayout)
        {
          case SiteMapLayoutType.Breadcrumbs:
            RenderSiteMapPath(output);
            break;
          case SiteMapLayoutType.Tree:
            RenderTree(output);
            break;
          case SiteMapLayoutType.Directory:
            RenderDirectory(output);
            break;
          case SiteMapLayoutType.DropDownDirectory:
            RenderDropDownDirectory(output);
            break;
          case SiteMapLayoutType.DropDown:
            RenderDropDown(output);
            break;
          default:
            output.Write("SiteMap layout " + _renderLayout.ToString() + " not supported.");
            break;
        }
      }
    }

    #endregion

    #region Private Methods

    private string GetEffectiveCssClass(SiteMapNode oNode)
    {
      // we have our own css class?
      if(oNode.CssClass != string.Empty)
      {
        return oNode.CssClass;
      }

      // we are selected and selected has a class?
      if(oNode == ParentSiteMap.SelectedNode && ParentSiteMap.SelectedNodeCssClass != string.Empty)
      {
        return ParentSiteMap.SelectedNodeCssClass;
      }

      // we are a root and roots have a class?
      if(oNode.ParentNode == null && ParentSiteMap.RootNodeCssClass != string.Empty)
      {
        return ParentSiteMap.RootNodeCssClass;
      }

      // we are a parent and parents have a class?
      if(oNode.nodes != null && oNode.Nodes.Count > 0 && ParentSiteMap.ParentNodeCssClass != string.Empty)
      {
        return ParentSiteMap.ParentNodeCssClass;
      }

      // we are a non-root, and childcssclass is defined?
      if(oNode.ParentNode != null && ParentSiteMap.ChildNodeCssClass != string.Empty)
      {
        return ParentSiteMap.ChildNodeCssClass;
      }

      // we must be a leaf.
      return ParentSiteMap.LeafNodeCssClass;
    }

    private string GetEffectiveImageUrl(SiteMapNode oNode)
    {
      string sImageUrl = string.Empty;

      if(oNode.ImageUrl != string.Empty)
      {
        sImageUrl = oNode.ImageUrl;
      }
      else if(oNode.parentNode == null && ParentSiteMap.RootNodeImageUrl != string.Empty)
      {
        sImageUrl = ParentSiteMap.RootNodeImageUrl;
      }
      else if(oNode.nodes != null && oNode.Nodes.Count > 0 && ParentSiteMap.ParentNodeImageUrl != string.Empty)
      {
        sImageUrl = ParentSiteMap.ParentNodeImageUrl;
      }
      else if((oNode.nodes == null || oNode.Nodes.Count == 0) && ParentSiteMap.LeafNodeImageUrl != string.Empty)
      {
        sImageUrl = ParentSiteMap.LeafNodeImageUrl;
      }

      return sImageUrl;
    }

    #endregion

    #region Render Common

    private void RenderImageCell(HtmlTextWriter output, SiteMapNode oNode)
    {
      RenderImageCell(output, oNode, null);
    }

    private void RenderImageCell(HtmlTextWriter output, SiteMapNode oNode, string sImageUrl)
    {
      if(sImageUrl == null)
      {
        sImageUrl = this.GetEffectiveImageUrl(oNode);
      }

      if(sImageUrl != string.Empty)
      {
        output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

        string sHeight = ImageHeight > 0 ? " height=\"" + ImageHeight + "\"" : "";
        string sWidth = ImageWidth > 0 ? " width=\"" + ImageWidth + "\"" : "";

        output.Write("<img style=\"display:block;\" alt=\"\"" + sHeight + sWidth + " src=\"" + sImageUrl + "\" />");
        
        output.RenderEndTag(); // </td>

        // do we need to render padding?
        if(ParentSiteMap.NodeLabelPadding > 0)
        {
          output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>
          output.Write("<div style=\"height:1px;width:" + ParentSiteMap.NodeLabelPadding + "px;\"></div>");
          output.RenderEndTag(); // </td>
        }
      }
    }

    private void RenderTemplate(HtmlTextWriter output, SiteMapNode oNode, string sTemplateId)
    {
      NavigationCustomTemplate oTemplate = ParentSiteMap.FindTemplateById(sTemplateId);

      if(oTemplate == null)
      {
        throw new Exception("Template not found: " + sTemplateId);
      }

      NavigationTemplateContainer oContainer = new NavigationTemplateContainer(oNode);
      oTemplate.Template.InstantiateIn(oContainer);
      oContainer.DataBind();

      oContainer.RenderControl(output);
    }

    private void RenderNode(HtmlTextWriter output, SiteMapNode oNode)
    {
      output.Write("<div");

      string sCssClass = GetEffectiveCssClass(oNode);
      if(sCssClass != string.Empty)
      {
        output.WriteAttribute("class", sCssClass);
      }
      // special case: no break!
      if( (oNode.ParentNode != null && _renderLayout == SiteMapLayoutType.Directory) ||
          _renderLayout == SiteMapLayoutType.Breadcrumbs)
      {
        output.WriteAttribute("style", "display:inline;");
      }
      output.Write(">");

      // do we have a template for this node already generated?
      if(oNode.ServerTemplateId != string.Empty)
      {
        string sTemplateId = ParentSiteMap.ClientID + "_" + oNode.PostBackID;

        Control oRenderedTemplate = ParentSiteMap.FindControl(sTemplateId);

        if(oRenderedTemplate != null)
        {
          oRenderedTemplate.RenderControl(output);
        }
        else
        {
          output.Write("Template not found.");
        }
      }
        // Default root template?
      else if(ParentSiteMap.RootNodeTemplateId != string.Empty && oNode.ParentNode == null)
      {
        RenderTemplate(output, oNode, ParentSiteMap.RootNodeTemplateId);
      }
      else // Standard template
      {
        if(oNode.Enabled)
        {
          // Render node
          if(Context == null || !Context.Request.Browser.JavaScript || oNode.NavigateUrl != "")
          {
            // We don't support JS
            if(oNode.Target != string.Empty)
            {
              output.AddAttribute(HtmlTextWriterAttribute.Target, oNode.Target);
            }
            output.AddAttribute(HtmlTextWriterAttribute.Href, oNode.NavigateUrl);
          }
          else if(Context != null && Context.Request.Browser.JavaScript)
          {
            string sScriptCommand = oNode.GenerateClientCommand();
            if(sScriptCommand != string.Empty)
            {
              output.AddAttribute(HtmlTextWriterAttribute.Href, "javascript:" + sScriptCommand);
            }
            else
            {
              output.AddAttribute(HtmlTextWriterAttribute.Href, "#");
            }
          }

          // LeafNodePrefix?
          if(oNode.nodes == null || oNode.Nodes.Count == 0 &&
            ParentSiteMap.LeafNodePrefix != string.Empty)
          {
            output.Write(ParentSiteMap.LeafNodePrefix);
          }
        
          if(oNode.ToolTip != string.Empty)
          {
            output.AddAttribute(HtmlTextWriterAttribute.Title, oNode.ToolTip);
          }
        
          output.RenderBeginTag(HtmlTextWriterTag.A);
        }

        output.Write(oNode.Text);
        
        if(oNode.Enabled)
        {
          output.RenderEndTag(); 
        }
      }

      output.Write("</div>");
    }

    #endregion
    
    #region Render SiteMapPath

    private void RenderSiteMapPathSeparatorCell(HtmlTextWriter output)
    {
      output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

      if(this.ParentSiteMap.BreadcrumbsSeparatorTemplateId != string.Empty)
      {
        NavigationCustomTemplate oSeparatorTemplate = this.ParentSiteMap.FindTemplateById(this.ParentSiteMap.BreadcrumbsSeparatorTemplateId);
        NavigationTemplateContainer oSeparatorContainer = new NavigationTemplateContainer(null);
        oSeparatorTemplate.Template.InstantiateIn(oSeparatorContainer);
        oSeparatorContainer.RenderControl(output);
      }
      else
      {
        output.Write(this.ParentSiteMap.BreadcrumbsSeparatorString);
      }

      output.RenderEndTag(); // </td>
    }
    
    private void RenderSiteMapPathNode(HtmlTextWriter output, SiteMapNode oNode, int iLevels)
    {
      if(iLevels <= 0)
      {
        return;
      }

      if(oNode.ParentNode != null)
      {
        if(this.ParentSiteMap.BreadcrumbsDirection == BreadcrumbsDirectionType.LeftToRight)
        {
          RenderSiteMapPathNode(output, oNode.ParentNode, iLevels - 1);
        
          if(oNode.IncludeInSiteMap)
          {
            // Render spacer
            if(iLevels > 1)
            {
              RenderSiteMapPathSeparatorCell(output);
            }
          }
        }

        if(oNode.IncludeInSiteMap)
        {
          // Render non-root node
          RenderSiteMapPathSingleNode(output, oNode);
        }
        
        if(this.ParentSiteMap.BreadcrumbsDirection == BreadcrumbsDirectionType.RightToLeft)
        {
          if(oNode.IncludeInSiteMap)
          {
            // Render spacer
            if(iLevels > 1)
            {
              RenderSiteMapPathSeparatorCell(output);
            }
          }

          RenderSiteMapPathNode(output, oNode.ParentNode, iLevels - 1);
        }
      }
      else
      {
        if(oNode.IncludeInSiteMap)
        {
          // Render root node
          RenderSiteMapPathSingleNode(output, oNode);
        }
      }
    }

    private void RenderSiteMapPathSingleNode(HtmlTextWriter output, SiteMapNode oNode)
    {
      string sImageUrl = this.GetEffectiveImageUrl(oNode);

      // do we have an icon?
      if(sImageUrl != string.Empty)
      {
        if(ParentSiteMap.BreadcrumbsDirection == BreadcrumbsDirectionType.LeftToRight)
        {
          RenderImageCell(output, oNode, sImageUrl);
        }
      }

      output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

      RenderNode(output, oNode);

      output.RenderEndTag(); // </td>

      // did we have an icon?
      if(sImageUrl != string.Empty)
      {
        // we'll need to close the table
        if(ParentSiteMap.BreadcrumbsDirection == BreadcrumbsDirectionType.RightToLeft)
        {
          RenderImageCell(output, oNode, sImageUrl);
        }
      }
    }

    private void RenderSiteMapPath(HtmlTextWriter output)
    {
      // we'll need a table
      output.AddAttribute("border", "0");
      output.AddAttribute("cellpadding", "0");
      output.AddAttribute("cellspacing", "0");
      output.RenderBeginTag(HtmlTextWriterTag.Table); // <table>
      output.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>

      RenderSiteMapPathNode(output, this, (this.ParentSiteMap.BreadcrumbsLevelsToDisplay > 0? this.ParentSiteMap.BreadcrumbsLevelsToDisplay : int.MaxValue));

      output.RenderEndTag(); // </tr>
      output.RenderEndTag(); // </table>
    }

    #endregion

    #region Render Tree

    private void RenderTreeIndentation(HtmlTextWriter output, SiteMapNode oNode, int depth)
    {
      int start = 0;

      // if we have multiple cells, we must omit the outermost level of lines
      if(ParentSiteMap.TreeShowLines && Math.Max(ParentSiteMap.Table.Rows.Count, ParentSiteMap.Table.Rows[0].Cells.Count) > 1)
      {
        start = 1;
      }

      for(int i = start; i < depth; i++)
      {
        RenderTreeIndentationCell(output, oNode, i);
      }
    }

    private void RenderTreeIndentationCell(HtmlTextWriter output, SiteMapNode oNode, int depth)
    {
      output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

      if(ParentSiteMap.TreeShowLines)
      {
        SiteMapNode oParent = oNode;
        for(int i = oNode.Depth; i > depth; i--)
        {
          oParent = oParent.ParentNode;
        }
		
        string sHeight = ParentSiteMap.TreeLineImageHeight > 0 ? " height=\"" + ParentSiteMap.TreeLineImageHeight + "\"" : "";
        string sWidth = ParentSiteMap.TreeLineImageWidth > 0 ? " width=\"" + ParentSiteMap.TreeLineImageWidth + "\"" : "";

        if(oParent.LastInGroup) // is last in group?
        {
          output.Write("<img style=\"display:block;\" alt=\"\"" + sHeight + sWidth + " src=\"" + Utils.ConvertUrl(Context, ParentSiteMap.TreeLineImagesFolderUrl, "noexpand.gif") + "\" />");
        }
        else
        {
          output.Write("<img style=\"display:block;\" alt=\"\"" + sHeight + sWidth + " src=\"" + Utils.ConvertUrl(Context, ParentSiteMap.TreeLineImagesFolderUrl, "i.gif") + "\" />");
        }
      }
      else
      {
        output.Write("<div style=\"height:1px;width:" + ParentSiteMap.NodeIndent + "px;\"></div>");
      }

      output.RenderEndTag(); // </td>
    }

    private void RenderTreeBullet(HtmlTextWriter output, SiteMapNode oNode, int depth)
    {
      output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

      if(ParentSiteMap.TreeShowLines)
      {
        string sHeight = ParentSiteMap.TreeLineImageHeight > 0 ? " height=\"" + ParentSiteMap.TreeLineImageHeight + "\"" : "";
        string sWidth = ParentSiteMap.TreeLineImageWidth > 0 ? " width=\"" + ParentSiteMap.TreeLineImageWidth + "\"" : "";

        if(oNode.ParentNode == null) // is a root?
        {
          if(oNode.PreviousSibling == null) // is the first root?
          {
            if (oNode.LastInGroup)
            {
              output.Write("<img style=\"display:block;\" alt=\"\"" + sHeight + sWidth + " src=\"" + Utils.ConvertUrl(Context, ParentSiteMap.TreeLineImagesFolderUrl, "dash.gif") + "\" />");
            }
            else
            {
              output.Write("<img style=\"display:block;\" alt=\"\"" + sHeight + sWidth + " src=\"" + Utils.ConvertUrl(Context, ParentSiteMap.TreeLineImagesFolderUrl, "r.gif") + "\" />");
            }
          }
          else if (oNode.LastInGroup) // is last root in group?
          {
            output.Write("<img style=\"display:block;\" alt=\"\"" + sHeight + sWidth + " src=\"" + Utils.ConvertUrl(Context, ParentSiteMap.TreeLineImagesFolderUrl, "l.gif") + "\" />");
          }
          else
          {
            output.Write("<img style=\"display:block;\" alt=\"\"" + sHeight + sWidth + " src=\"" + Utils.ConvertUrl(Context, ParentSiteMap.TreeLineImagesFolderUrl, "t.gif") + "\" />");
          }
        }
        else
        {
          if (oNode.LastInGroup)
          {
            output.Write("<img style=\"display:block;\" alt=\"\"" + sHeight + sWidth + " src=\"" + Utils.ConvertUrl(Context, ParentSiteMap.TreeLineImagesFolderUrl, "l.gif") + "\" />");
          }
          else
          {
            output.Write("<img style=\"display:block;\" alt=\"\"" + sHeight + sWidth + " src=\"" + Utils.ConvertUrl(Context, ParentSiteMap.TreeLineImagesFolderUrl, "t.gif") + "\" />");
          }
        }
      }
      else
      {
        if(ParentSiteMap.TreeBulletImageUrl != string.Empty)
        {
          output.Write("<img style=\"display:block;\" alt=\"\" src=\"" + ParentSiteMap.TreeBulletImageUrl + "\" />");
        }
        else
        {	
          RenderTreeIndentationCell(output, oNode, depth);
        }
      }
 
      output.RenderEndTag(); // </td>
    }

    private void RenderTreeNode(HtmlTextWriter output, SiteMapNode oNode, int depth)
    {
      output.AddAttribute("border", "0");
      output.AddAttribute("cellpadding", "0");
      output.AddAttribute("cellspacing", "0");
      output.RenderBeginTag(HtmlTextWriterTag.Table); // <table>
      output.RenderBeginTag(HtmlTextWriterTag.Tr); // <tr>
 
      // render indentation
      if(ParentSiteMap.NodeIndent > 0)
      {
        RenderTreeIndentation(output, oNode, depth);
      }

      // if we have multiple cells, roots must omit line bullets
      if(ParentSiteMap.TreeShowLines && ((ParentSiteMap.Table.Rows.Count == 1 && ParentSiteMap.Table.Rows[0].Cells.Count == 1) || oNode.ParentNode != null) || ParentSiteMap.TreeBulletImageUrl != string.Empty)
      {
        RenderTreeBullet(output, oNode, depth);
      }

      RenderImageCell(output, oNode);

      output.AddStyleAttribute("width", "100%");
      output.RenderBeginTag(HtmlTextWriterTag.Td); // <td>

      // render node
      RenderNode(output, oNode);

      output.RenderEndTag(); // </td>
      output.RenderEndTag(); // </tr>
      output.RenderEndTag(); // </table>
    }

    private void RenderTreeNodes(HtmlTextWriter output, SiteMapNodeCollection arNodes, int depth)
    {
      foreach(SiteMapNode oNode in arNodes)
      {
        if(oNode.IncludeInSiteMap)
        {
          RenderTreeNode(output, oNode, depth);
        
          // render sub-tree
          if(oNode.nodes != null && oNode.Nodes.Count > 0 && (ParentSiteMap.RenderDrillDownDepth == 0 || depth + 1 < ParentSiteMap.RenderDrillDownDepth))
          {
            RenderTreeNodes(output, oNode.Nodes, depth + 1);
          }
        }
      }
    }

    private void RenderTree(HtmlTextWriter output)
    {
      output.Write(ParentSiteMap.TreeHeaderString);

      if(this.IncludeInSiteMap)
      {
        RenderTreeNode(output, this, 0);
      }

      if(this.nodes != null && this.Nodes.Count > 0)
      {
        RenderTreeNodes(output, this.Nodes, 1);
      }

      output.Write(ParentSiteMap.TreeFooterString);
    }

    #endregion

    #region Render Directory

    private void RenderDirectory(HtmlTextWriter output)
    {
      if(!this.IncludeInSiteMap)
      {
        return;
      }

      // render heading node
      RenderNode(output, this);

      // render directory header
      output.Write(ParentSiteMap.DirectoryHeaderString);

      foreach(SiteMapNode oChild in this.Nodes)
      {
        if(oChild.IncludeInSiteMap)
        {
          RenderNode(output, oChild);

          if(oChild.nextSibling != null)
          {
            output.Write(ParentSiteMap.DirectorySeparatorString);
          }
        }
      }

      // render directory footer
      output.Write(ParentSiteMap.DirectoryFooterString);
    }

    #endregion

    #region Render DropDownDirectory

    private void RenderDropDownDirectory(HtmlTextWriter output)
    {
      if(!this.IncludeInSiteMap)
      {
        return;
      }

      // render heading node
      RenderNode(output, this);

      // render directory header
      output.Write(ParentSiteMap.DirectoryHeaderString);

      if(this.nodes != null && this.nodes.Count > 0)
      {
        // is there a class for dropdowns?
        if(ParentSiteMap.DropDownCssClass != string.Empty)
        {
          output.AddAttribute(HtmlTextWriterAttribute.Class, ParentSiteMap.DropDownCssClass);
        }
      
        // change handler
        output.AddAttribute(HtmlTextWriterAttribute.Onchange, "eval(this.options[this.selectedIndex].value)");
      
        output.RenderBeginTag(HtmlTextWriterTag.Select);

        // render nodes
        RenderDropDownNodes(output, this.Nodes, 0);

        output.RenderEndTag();
      }

      // render directory footer
      output.Write(ParentSiteMap.DirectoryFooterString);
    }

    #endregion

    #region Render DropDown

    private void RenderDropDownNode(HtmlTextWriter output, SiteMapNode oNode, int depth)
    {
      // does this node have a class?
      string sCssClass = GetEffectiveCssClass(oNode);
      if(sCssClass != string.Empty)
      {
        output.AddAttribute("class", sCssClass);
      }

      // client-command action
      if(Context != null)
      {
        string sScriptCommand = oNode.GenerateClientCommand();
        if(sScriptCommand != string.Empty)
        {
          output.AddAttribute(HtmlTextWriterAttribute.Value, sScriptCommand);
        }
      }

      // is this node selected?
      if(oNode == ParentSiteMap.SelectedNode)
      {
        output.AddAttribute(HtmlTextWriterAttribute.Selected, "true");
      }

      output.RenderBeginTag(HtmlTextWriterTag.Option);
        
      // Write indentation
      for(int i = 0; i < depth; i++)
      {
        output.Write(ParentSiteMap.DropDownIndentString);
      }

      // Write label
      output.Write(oNode.Text);

      output.RenderEndTag();
    }

    private void RenderDropDownNodes(HtmlTextWriter output, SiteMapNodeCollection arNodes, int depth)
    {
      foreach(SiteMapNode oNode in arNodes)
      {
        if(oNode.IncludeInSiteMap)
        {
          RenderDropDownNode(output, oNode, depth);
        
          if(oNode.nodes != null && oNode.Nodes.Count > 0)
          {
            RenderDropDownNodes(output, oNode.Nodes, depth + 1);
          }
        }
      }
    }

    private void RenderDropDown(HtmlTextWriter output)
    {
      // is there a class for dropdowns?
      if(ParentSiteMap.DropDownCssClass != string.Empty)
      {
        output.AddAttribute(HtmlTextWriterAttribute.Class, ParentSiteMap.DropDownCssClass);
      }
      output.AddAttribute(HtmlTextWriterAttribute.Onchange, "eval(this.options[this.selectedIndex].value)");
      output.RenderBeginTag(HtmlTextWriterTag.Select); // <select>

      if(this.nodes != null && this.Nodes.Count > 0)
      {
        RenderDropDownNodes(output, this.Nodes, 0);
      }

      output.RenderEndTag(); // </select>
    }

    #endregion
	}
}
