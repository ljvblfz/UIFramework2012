using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace ComponentArt.Web.UI
{
	/// <summary>
  /// Navigation node class for the <see cref="NavBar"/> control. 
	/// </summary>
  /// <remarks>
  /// <para>
  /// A NavBarItem object corresponds to a node in a <see cref="NavBar" />. NavBarItem are created on the
  /// fly as data is loaded from a data source, or can be created and added to the NavBar rogrammatically. They
  /// can also be defined inline, inside NavBar's <see cref="NavBar.Items" /> collection.
  /// </para>
  /// <para>
  /// Typically, a NavBar node needs to have its <see cref="NavigationNode.Text" /> and <see cref="NavigationNode.ID" /> properties set, with
  /// some associated action on click, such as a <see cref="NavigationNode.NavigateUrl" /> to go to, or a <see cref="NavigationNode.ClientSideCommand" />
  /// to execute.
  /// </para>
  /// </remarks>
  [ToolboxItem(false)]
  [ParseChildren(true, "Items")]
	public class NavBarItem : BaseMenuItem
	{
    #region Public Properties
    
    /// <summary>
    /// Whether to perform a postback when a group is collapsed.
    /// </summary>
    [Description("Whether to perform a postback when a group is collapsed.")]
    [DefaultValue(false)]
    [Category("Behavior")]
    public bool AutoPostBackOnCollapse
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("AutoPostBackOnCollapse")]; 
        return (o == null) ? false : Utils.ParseBool(o,false); 
      }
      set 
      {
        Properties[GetAttributeVarName("AutoPostBackOnCollapse")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether to perform a postback when an item is expanded.
    /// </summary>
    [Description("Whether to perform a postback when an item is expanded.")]
    [DefaultValue(false)]
    [Category("Behavior")]
    public bool AutoPostBackOnExpand
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("AutoPostBackOnExpand")]; 
        return (o == null) ? false : Utils.ParseBool(o,false);
      }
      set 
      {
        Properties[GetAttributeVarName("AutoPostBackOnExpand")] = value.ToString();
      }
    }

    /// <summary>
    /// Whether this item is expanded.
    /// </summary>
    [Description("Whether this item is expanded.")]
    [DefaultValue(false)]
    [Category("Layout")]
    public bool Expanded
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("Expanded")]; 
        return (o == null) ? false : Utils.ParseBool(o,false);
      }
      set 
      {
        Properties[GetAttributeVarName("Expanded")] = value.ToString();
      }
    }

    /// <summary>
    /// Height of this NavBar item.
    /// </summary>
    [Description("Height of this NavBar item.")]
    [DefaultValue(typeof(Unit),"")]
    [Category("Layout")]
    public new Unit Height
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("Height")]; 
        return (o == null) ?
          (this.ParentNavBar == null? Unit.Empty : this.ParentNavBar.DefaultItemHeight) : Unit.Parse(o); 
      }
      set 
      {
        Properties[GetAttributeVarName("Height")] = value.ToString();
      }
    }

    /// <summary>
    /// Collection of root NavBarItems.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    [Browsable(false)]
    public NavBarItemCollection Items
    {
      get
      {
        if(this.nodes == null)
        {
          nodes = new NavBarItemCollection(ParentNavBar, this);
        }

        return (NavBarItemCollection)nodes;
      }
    }

    /// <summary>
    /// This item's parent item.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new NavBarItem ParentItem
    {
      get
      {
        return (NavBarItem)(this.parentNode);
      }
    }

    /// <summary>
    /// The NavBar that this item belongs to.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public NavBar ParentNavBar
    {
      get
      {
        return (NavBar)(this.navigator);
      }
    }

    /// <summary>
    /// Whether this item can be selected.
    /// </summary>
    [Description("Whether this item can be selected.")]
    [DefaultValue(true)]
    [Category("Behavior")]
    public bool Selectable
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("Selectable")]; 
        return (o == null) ? true : Utils.ParseBool(o,true); 
      }
      set 
      {
        Properties[GetAttributeVarName("Selectable")] = value.ToString();
      }
    }

    /// <summary>
    /// The CSS class to use for this item's subgroup.
    /// </summary>
    [Description("The CSS class to use for this item's subgroup.")]
    [Category("Appearance")]
    [DefaultValue("")]
    public string SubGroupCssClass
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupCssClass")];
        return (o != null) ? o : this.DefaultSubGroupCssClass;
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupCssClass")] = value;
      }
    }

    /// <summary>
    /// The height to force for this item's subgroup. If specified, regardless of its actual size,
    /// the subgroup will be expanded to exactly the height specified here.
    /// </summary>
    [Description("The height to force for this item's subgroup.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int SubGroupHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupHeight")]; 
        return (o == null) ? 0 : Utils.ParseInt(o,0); 
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupHeight")] = value.ToString();
      }
    }
    
    /// <summary>
    /// The spacing (in pixels) to render between items in this item's subgroup.
    /// </summary>
    [Description("The spacing (in pixels) to render between items in this item's subgroup.")]
    [Category("Layout")]
    [DefaultValue(0)]
    public int SubGroupItemSpacing
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupItemSpacing")]; 
        return (o == null) ? (ParentNavBar != null? ParentNavBar.DefaultItemSpacing : 0) : Utils.ParseInt(o,0); 
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupItemSpacing")] = value.ToString();
      }
    }

    #endregion

    #region Private Methods

    internal override NavigationNode AddNode()
    {
      NavBarItem oNewItem = new NavBarItem();
      this.Items.Add(oNewItem);
      return oNewItem;
    }

    #endregion
	}
}
