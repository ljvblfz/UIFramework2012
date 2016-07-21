using System;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;
using System.Xml;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Navigation node class for the <see cref="TreeView"/> control. 
  /// </summary>
  /// <remarks>
  /// <para>
  /// The <see cref="TreeView" /> control is used to display hierarchical data, and is made up of a collection of nodes. 
  /// Each of these nodes is encapsulated by a TreeViewNode object. In order to render as part of the TreeView, a TreeViewNode
  /// must be added to the <see cref="TreeView.Nodes">Nodes</see> collection of the base TreeView object, or the <see cref="Nodes" /> collection of
  /// another TreeViewNode object. The resulting hierarchy is reflected in the visual structure of the rendered TreeView control. 
  /// </para>
  /// <para>
  /// There are a few TreeViewNode properties which are important for a properly functioning TreeView. The <see cref="ID" /> property provides a unique
  /// identifier for the node, and will be assigned automatically if the <see cref="Grid.AutoAssignNodeIDs" /> is set to true. 
  /// To take action when a node is clicked, either the 
  /// <see cref="NavigationNode.NavigateUrl" /> or the <see cref="NavigationNode.ClientSideCommand" /> property must be set.
  /// </para>
  /// <para>
  /// The <see cref="UseWebService" /> property allows a node to postpone loading its children until it is expanded by the user.
  /// More information about this functionality can be found in the 
  /// <see cref="ComponentArt.Web.UI.chm::/WebServices_TreeView_Load_On_Demand.htm">Using Web Service Load on Demand With TreeView</see> tutorial. 
  /// </para>
  /// <para>
  /// See the <see cref="ComponentArt.Web.UI.chm::/TreeView_Look_and_Feel_Properties.htm">Look and Feel Properties</see> 
  /// tutorial for information on styling, and 
  /// <see cref="ComponentArt.Web.UI.chm::/TreeView_Look_and_Feel_Properties.htm">Overview of Templates in Web.UI</see>
  /// for details on using templates to further customize individual nodes. 
  /// </remarks>
  [ToolboxItem(false)]
  [ParseChildren(true, "Nodes")]
	public class TreeViewNode : NavigationNode
	{
		#region Public Properties
		
    /// <summary>
    /// Whether to postback when this node is moved by drag and drop. Default: false.
    /// </summary>
    [Description("Whether to postback when this node is moved by drag and drop. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnMove
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("AutoPostBackOnMove")], false);
      }
      set 
      {
        Properties[GetAttributeVarName("AutoPostBackOnMove")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to postback when this node is renamed (edited). Default: false.
    /// </summary>
    [Description("Whether to postback when this node is renamed (edited). Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnRename 
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("AutoPostBackOnRename")], false);
      }
      set 
      {
        Properties[GetAttributeVarName("AutoPostBackOnRename")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to postback when this node is checked or unchecked. Default: false.
    /// </summary>
    [Description("Whether to postback when this node is checked or unchecked. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnCheckChanged  
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("AutoPostBackOnCheckChanged")], false);
      }
      set 
      {
        Properties[GetAttributeVarName("AutoPostBackOnCheckChanged")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to postback when this node is expanded. Default: false.
    /// </summary>
    [Description("Whether to postback when this node is expanded. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnExpand
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("AutoPostBackOnExpand")], false);
      }
      set 
      {
        Properties[GetAttributeVarName("AutoPostBackOnExpand")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to postback when this node is collapsed. Default: false.
    /// </summary>
    [Description("Whether to postback when this node is collapsed. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool AutoPostBackOnCollapse
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("AutoPostBackOnCollapse")], false);
      }
      set 
      {
        Properties[GetAttributeVarName("AutoPostBackOnCollapse")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether this node is checked. See ShowCheckBox.
    /// </summary>
    [Description("Whether this node is checked. See ShowCheckBox.")]
    [DefaultValue(false)]
    [Category("Data")]
    public bool Checked
		{
			get 
			{
				return Utils.ParseBool(this.Properties[GetAttributeVarName("Checked")], false);
			}
			set 
			{
				Properties[GetAttributeVarName("Checked")] = value.ToString();
			}
		}

    /// <summary>
    /// CSS class to use for this node when the selected node is a descendant.
    /// </summary>
    [Description("CSS class to use for this node when the selected node is a descendant.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string ChildSelectedCssClass
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("ChildSelectedCssClass")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("ChildSelectedCssClass")] = value;
			}
		}

    /// <summary>
    /// CSS class to use for the row of this node when the selected node is a descendant.
    /// </summary>
    [Description("CSS class to use for the row of this node when the selected node is a descendant.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string ChildSelectedRowCssClass
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("ChildSelectedRowCssClass")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("ChildSelectedRowCssClass")] = value;
			}
		}

    /// <summary>
    /// CSS class to use for this node on hover when the selected node is a descendant.
    /// </summary>
    [Description("CSS class to use for this node on hover when the selected node is a descendant.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string ChildSelectedHoverCssClass
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("ChildSelectedHoverCssClass")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("ChildSelectedHoverCssClass")] = value;
			}
		}

    /// <summary>
    /// CSS class to use for the row of this node on hover when the selected node is a descendant.
    /// </summary>
    [Description("CSS class to use for the row of this node on hover when the selected node is a descendant.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string ChildSelectedHoverRowCssClass
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("ChildSelectedHoverRowCssClass")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("ChildSelectedHoverRowCssClass")] = value;
			}
		}

    /// <summary>
    /// URL to load sub-tree contents from (XML).
    /// </summary>
    [Description("URL to load sub-tree contents from (XML).")]
    [Category("Data")]
    [DefaultValue("")]
		public string ContentCallbackUrl
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("ContentCallbackUrl")]; 
				return (o == null) ? string.Empty : (ParentTreeView == null ? o : ParentTreeView.ConvertUrl(o)); 
			}
			set 
			{
				Properties[GetAttributeVarName("ContentCallbackUrl")] = value;
			}
		}

    /// <summary>
    /// CSS class to use for this node when it is cut.
    /// </summary>
    [Description("CSS class to use for this node when it is cut.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string CutCssClass
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("CutCssClass")]; 
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("CutCssClass")] = value;
      }
    }

    /// <summary>
    /// CSS class to use for the row of this node when it is cut.
    /// </summary>
    [Description("CSS class to use for this node when it is cut.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string CutRowCssClass
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("CutRowCssClass")]; 
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("CutRowCssClass")] = value;
      }
    }

    /// <summary>
    /// Whether this node can be dragged to another TreeView. Default: false.
    /// </summary>
    [Description("Whether this node can be dragged to another TreeView. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool DraggingAcrossTreesEnabled
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("DraggingAcrossTreesEnabled")], false);
      }
      set 
      {
        Properties[GetAttributeVarName("DraggingAcrossTreesEnabled")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether this node can be dragged. 
    /// </summary>
    [Description("Whether this node can be dragged.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool DraggingEnabled
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("DraggingEnabled")], false);
      }
      set 
      {
        Properties[GetAttributeVarName("DraggingEnabled")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether this node can have a node from another TreeView dropped on or below it. Default: false.
    /// </summary>
    [Description("Whether this node can have a node from another TreeView dropped on it. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool DroppingAcrossTreesEnabled
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("DroppingAcrossTreesEnabled")], false);
      }
      set 
      {
        Properties[GetAttributeVarName("DroppingAcrossTreesEnabled")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether this node can have another node dropped on or below it. Default: false.
    /// </summary>
    [Description("Whether this node can have another node dropped on or below it. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool DroppingEnabled
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("DroppingEnabled")], false);
      }
      set 
      {
        Properties[GetAttributeVarName("DroppingEnabled")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether this node's label is editable by clicking it twice, as in Windows Explorer. Default: false.
    /// </summary>
    [Description("Whether this node's label is editable by clicking it twice, as in Windows Explorer. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool EditingEnabled
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("EditingEnabled")], false);
      }
      set 
      {
        Properties[GetAttributeVarName("EditingEnabled")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether this node is expanded.
    /// </summary>
    [Description("Whether this node is expanded.")]
    [DefaultValue(false)]
    [Category("Layout")]
    public bool Expanded
		{
			get 
			{
				return Utils.ParseBool(this.Properties[GetAttributeVarName("Expanded")], false);
			}
			set 
			{
				Properties[GetAttributeVarName("Expanded")] = value.ToString();
			}
		}

    /// <summary>
    /// Icon to use for this node when it is expanded.
    /// </summary>
    [Description("Icon to use for this node when it is expanded.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string ExpandedImageUrl
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("ExpandedImageUrl")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("ExpandedImageUrl")] = value;
			}
		}

    /// <summary>
    /// Whether to extend the label cell to the right edge of the TreeView. Default: false.
    /// </summary>
    [Description("Whether to extend the label cell to the right edge of the TreeView. Default: false.")]
    [Category("Layout")]
    [DefaultValue(false)]
    public bool ExtendNodeCell
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("ExtendNodeCell")], false);
      }
      set 
      {
        Properties[GetAttributeVarName("ExtendNodeCell")] = value.ToString();
      }
    }

    /// <summary>
    /// CSS class to use for this node when it is multiple-selected.
    /// </summary>
    [Description("CSS class to use for this node when it is multiple-selected.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string MultipleSelectedCssClass
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("MultipleSelectedCssClass")]; 
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("MultipleSelectedCssClass")] = value;
      }
    }

    /// <summary>
    /// CSS class to use for this node's row when it is multiple-selected.
    /// </summary>
    [Description("CSS class to use for this node's row when it is multiple-selected.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string MultipleSelectedRowCssClass
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("MultipleSelectedRowCssClass")]; 
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("MultipleSelectedRowCssClass")] = value;
      }
    }

    /// <summary>
    /// CSS class to use for this node on hover.
    /// </summary>
    [Description("CSS class to use for this node on hover.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string HoverCssClass
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("HoverCssClass")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("HoverCssClass")] = value;
			}
		}

    /// <summary>
    /// CSS class to use for the row of this node on hover.
    /// </summary>
    [Description("CSS class to use for the row of this node on hover.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string HoverRowCssClass
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("HoverRowCssClass")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("HoverRowCssClass")] = value;
			}
		}

    /// <summary>
    /// Height to apply to this node's icon (in pixels).
    /// </summary>
    [Description("Height to apply to this node's icon (in pixels).")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int ImageHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("ImageHeight")]; 
        return (o == null) ? 0 : int.Parse(o);
      }
      set 
      {
        Properties[GetAttributeVarName("ImageHeight")] = value.ToString();
      }
    }

    /// <summary>
    /// Width to apply to this node's icon (in pixels).
    /// </summary>
    [Description("Width to apply to this node's icon (in pixels).")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int ImageWidth
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("ImageWidth")]; 
        return (o == null) ? 0 : int.Parse(o);
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
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("ImageUrl")] = value;
      }
    }

    /// <summary>
    /// The number of pixels to indent for each level of depth for this node.
    /// </summary>
    [Description("The number of pixels to indent for each level of depth for this node.")]
    [Category("Layout")]
    [DefaultValue(-1)]
    public int Indent
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("Indent")]; 
        return (o == null) ? -1 : int.Parse(o);
      }
      set 
      {
        Properties[GetAttributeVarName("Indent")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether this node is multiple-selected.
    /// </summary>
    [Description("Whether this node is multiple-selected.")]
    [Category("Behavior")]
    [DefaultValue(false)]
    public bool IsMultipleSelected
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("IsMultipleSelected")], false);
      }
      set 
      {
        Properties[GetAttributeVarName("IsMultipleSelected")] = value.ToString();
      }
    }

    /// <summary>
    /// Padding (in pixels) to render between this node's label and its icon.
    /// </summary>
    [Description("Padding (in pixels) to render between this node's label and its icon.")]
    [Category("Layout")]
    [DefaultValue(-1)]
    public int LabelPadding
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("LabelPadding")]; 
        return (o == null) ? -1 : int.Parse(o);
      }
      set 
      {
        Properties[GetAttributeVarName("LabelPadding")] = value.ToString();
      }
    }

    /// <summary>
    /// Image to use in the margin for this item.
    /// </summary>
    /// <seealso cref="TreeView.DisplayMargin" />
    [Description("Image to use in the margin for this item.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string MarginImageUrl
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("MarginImageUrl")]; 
        return (o == null) ? string.Empty : o; 
      }
      set 
      {
        Properties[GetAttributeVarName("MarginImageUrl")] = value;
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
		public TreeViewNode NextSibling
		{
			get
			{
				return (TreeViewNode)(this.nextSibling);
			}
		}

    /// <summary>
    /// The collection of this node's child nodes.
    /// </summary>
		[
		PersistenceMode(PersistenceMode.InnerDefaultProperty),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
    NotifyParentProperty(true),
    Browsable(false)
		]
		public TreeViewNodeCollection Nodes
		{
			get
			{
				if(this.nodes == null)
				{
					nodes = new TreeViewNodeCollection(ParentTreeView, this);
				}

				return (TreeViewNodeCollection)nodes;
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
    public TreeViewNode ParentNode
		{
			get
			{
				return (TreeViewNode)(this.parentNode);
			}
		}

    /// <summary>
    /// The TreeView that this node belongs to.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public TreeView ParentTreeView
		{
			get
			{
				return (TreeView)(this.navigator);
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
    public TreeViewNode PreviousSibling
		{
			get
			{
				return (TreeViewNode)(this.previousSibling);
			}
		}

    /// <summary>
    /// CSS class to use for this node's row.
    /// </summary>
    [Description("CSS class to use for this node's row.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string RowCssClass
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("RowCssClass")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("RowCssClass")] = value;
			}
		}

    /// <summary>
    /// Whether this node can be selected. Default: true.
    /// </summary>
    [Description("Whether this node can be selected. Default: true.")]
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool Selectable
    {
      get 
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("Selectable")], true);
      }
      set 
      {
        Properties[GetAttributeVarName("Selectable")] = value.ToString();
      }
    }

    /// <summary>
    /// CSS class to use for this node when it is selected.
    /// </summary>
    [Description("CSS class to use for this node when it is selected.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string SelectedCssClass
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("SelectedCssClass")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("SelectedCssClass")] = value;
			}
		}

    /// <summary>
    /// Icon to use for this node when it is selected and expanded.
    /// </summary>
    [Description("Icon to use for this node when it is selected and expanded.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string SelectedExpandedImageUrl
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("SelectedExpandedImageUrl")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("SelectedExpandedImageUrl")] = value;
			}
		}

    /// <summary>
    /// CSS class to use for this node on hover when it is selected.
    /// </summary>
    [Description("CSS class to use for this node on hover when it is selected.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string SelectedHoverCssClass
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("SelectedHoverCssClass")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("SelectedHoverCssClass")] = value;
			}
		}

    /// <summary>
    /// CSS class to use for this node's row on hover when it is selected.
    /// </summary>
    [Description("CSS class to use for this node's row on hover when it is selected.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string SelectedHoverRowCssClass
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("SelectedHoverRowCssClass")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("SelectedHoverRowCssClass")] = value;
			}
		}

    /// <summary>
    /// Icon to use for this node when it is selected.
    /// </summary>
    [Description("Icon to use for this node when it is selected.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string SelectedImageUrl
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("SelectedImageUrl")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("SelectedImageUrl")] = value;
			}
		}

    /// <summary>
    /// CSS class to use for this node's row when it is selected.
    /// </summary>
    [Description("CSS class to use for this node's row when it is selected.")]
    [Category("Appearance")]
    [DefaultValue("")]
		public string SelectedRowCssClass
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("SelectedRowCssClass")]; 
				return (o == null) ? string.Empty : o; 
			}
			set 
			{
				Properties[GetAttributeVarName("SelectedRowCssClass")] = value;
			}
		}

    /// <summary>
    /// Whether this node is checkable. Default: false.
    /// </summary>
    [Description("Whether this node is checkable. Default: false.")]
    [Category("Behavior")]
    [DefaultValue(false)]
		public bool ShowCheckBox
		{
			get 
			{
				return Utils.ParseBool(this.Properties[GetAttributeVarName("ShowCheckBox")], false);
			}
			set 
			{
				Properties[GetAttributeVarName("ShowCheckBox")] = value.ToString();
			}
		}

    /// <summary>
    /// Whether this node should be populated using the specified web service. Default: false.
    /// </summary>
    /// <seealso cref="TreeView.WebService" />
    [Description("Whether this node should be populated using the specified web service. Default: false.")]
    [Category("Data")]
    [DefaultValue(false)]
    public bool UseWebService
    {
      get
      {
        return Utils.ParseBool(this.Properties[GetAttributeVarName("UseWebService")], false);
      }
      set
      {
        Properties[GetAttributeVarName("UseWebService")] = value.ToString();
      }
    }

		#endregion

    #region Public Methods

    /// <summary>
    /// Check this and all checkable child nodes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method checks the checkbox of the <see cref="TreeViewNode" /> it is called on, as well as all descendant nodes which have checkboxes.
    /// </para>
    /// <para>
    /// The <code>TreeViewNode's</code> <see cref="TreeViewNode.ShowCheckBox" /> property determines whether or not a node has a checkbox.
    /// To check all nodes in a <see cref="TreeView" /> hierarchy, use the <code>TreeView</code> <see cref="TreeView.CheckAll" /> method.
    /// </para>
    /// <para>
    /// All nodes can be un-checked using the <see cref="TreeViewNode.UnCheckAll" /> method. 
    /// </para>
    /// </remarks>
    public void CheckAll()
    {
      if(this.ShowCheckBox && !this.Checked)
      {
        this.Checked = true;
      }

      if(this.nodes != null)
      {
        foreach(TreeViewNode oNode in this.Nodes)
        {
          oNode.CheckAll();
        }
      }
    }

    /// <summary>
    /// Collapse this and all expanded child nodes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method collapses all expanded descendants of the node which it is called on. The <see cref="TreeViewNode" /> 
    /// <see cref="TreeViewNode.Expanded" /> property contains a boolean value indicating whether the node is currently expanded.
    /// That property can also be set, allowing a single node to be expanded or collapsed programatically. There is also a control-level
    /// <see cref="TreeView.CollapseAll" /> method, which collapses all expanded nodes in the <code>TreeView</code> hierarchy.
    /// </para>
    /// <para>
    /// All descendants of a node can be expanded using the <see cref="TreeViewNode.ExpandAll" /> method.
    /// </para>
    /// </remarks>
    public void CollapseAll()
    {
      if(this.nodes != null && this.Nodes.Count > 0)
      {
        if(this.Expanded)
        {
          this.Expanded = false;
        }

        foreach(TreeViewNode oNode in this.Nodes)
        {
          oNode.CollapseAll();
        }
      }
    }

    /// <summary>
    /// Expand this and all expandable child nodes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method expands all descendants of a <see cref="TreeViewNode" /> which have children. 
    /// The <see cref="TreeViewNode" /> <see cref="TreeViewNode.Expanded" /> 
    /// property contains a boolean value indicating whether the node is currently expanded.
    /// That property can also be set, allowing a single node to be expanded or collapsed programatically. There is also a control-level
    /// <see cref="TreeView.ExpandAll" /> method, which expands all nodes in the <code>TreeView</code> hierarchy.
    /// </para>
    /// <para>
    /// All descendants of a node can be collapsed using the <see cref="TreeViewNode.CollapseAll" /> method.
    /// </para>
    /// </remarks>
    public void ExpandAll()
    {
      if(this.nodes != null && this.Nodes.Count > 0)
      {
        if(!this.Expanded)
        {
          this.Expanded = true;
        }

        foreach(TreeViewNode oNode in this.Nodes)
        {
          oNode.ExpandAll();
        }
      }
    }

    /// <summary>
    /// Uncheck this and all checked child nodes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method un-checks the checkbox of the <see cref="TreeViewNode" /> it is called on, as well as all descendant nodes which
    /// have checkboxes.
    /// </para>
    /// <para>
    /// The <code>TreeViewNode's</code> <see cref="TreeViewNode.ShowCheckBox" /> property determines whether or not a node has a checkbox.
    /// To un-check all nodes in a <see cref="TreeView" /> hierarchy, use the <code>TreeView</code> <see cref="TreeView.UnCheckAll" /> method.
    /// </para>
    /// <para>
    /// All nodes can be checked using the <see cref="TreeViewNode.CheckAll" /> method.
    /// </para>
    /// </remarks>
    public void UnCheckAll()
    {
      if(this.ShowCheckBox && this.Checked)
      {
        this.Checked = false;
      }

      if(this.nodes != null)
      {
        foreach(TreeViewNode oNode in this.Nodes)
        {
          oNode.UnCheckAll();
        }
      }
    }

    #endregion

		#region Private Methods

    /// <summary>
    /// Creates a new TreeViewNode and adds it to this node's subgroup.
    /// </summary>
    /// <returns>The newly created child node.</returns>
		internal override NavigationNode AddNode()
		{
			TreeViewNode oNewNode = new TreeViewNode();
			this.Nodes.Add(oNewNode);
			return oNewNode;
		}

    internal bool IsChildForceHighlighted()
    {
      // If we don't have this pointer yet, forget it.
      if(this.ParentTreeView == null)
      {
        return false;
      }

      TreeViewNode oAncestor = this.ParentTreeView.ForceHighlightedNode;

      while(oAncestor != null)
      {
        oAncestor = oAncestor.ParentNode;

        if(oAncestor == this)
        {
          return true;
        }
      }

      return false;
    }

		#endregion
	}
}
