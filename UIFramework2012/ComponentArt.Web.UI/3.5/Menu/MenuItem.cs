using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Navigation node class for the <see cref="Menu"/> control. 
  /// </summary>
  /// <remarks>
  /// <para>
  /// <see cref="Menu" /> contents are organized as a hierarchy of items, each represented by a MenuItem object. In order to render as part of the Menu, 
  /// a MenuItem must be added to the <see cref="Menu.Items">Items</see> collection of the base Menu object, or the <see cref="Items" /> 
  /// collection of another Menu object. The resulting hierarchy is reflected in the visual structure of the rendered Menu control. 
  /// </para>
  /// <para>
  /// In order to take action when selected, a Menu item must have either its <see cref="ClientSideCommand" /> or <see cref="NavigateUrl" /> property set.
  /// </para>
  /// <para>
  /// As with the control as whole, the style of rendered MenuItems is governed by various <see cref="ItemLook" /> properties in combination with CSS.
  /// For more information on styling MenuItems, see the following tutorials: 
  /// <see cref="ComponentArt.Web.UI.chm::/WebUI_ItemLook_Concepts.htm">ItemLook Concepts</see>, 
  /// <see cref="ComponentArt.Web.UI.chm::/WebUI_Navigation_ItemLooks.htm">Overview of ItemLooks in ComponentArt Navigation Controls</see>, and
  /// <see cref="ComponentArt.Web.UI.chm::/WebUI_Look_and_Feel_Properties.htm">Look and Feel Properties</see>.
  /// Further customization can be accomplished using <see cref="ServerTemplates" /> or <see cref="ClientTemplates" />. 
  /// Templates are discussed in detail in the 
  /// <see cref="ComponentArt.Web.UI.chm::/WebUI_Templates_Overview.htm">Overview of Templates in Web.UI</see> tutorial.
  /// </para>  
  /// </remarks>
  [
  ToolboxItem(false),
  ParseChildren(true, "Items")
  ]
  public class MenuItem : BaseMenuItem
  {
    #region Public Properties

    /// <summary>
    /// Whether this item is checked.
    /// </summary>
    /// <remarks>
    /// This property only has effect in items with <see cref="ToggleType"/> set to <b>ItemToggleType.CheckBox</b>,
    /// <b>ItemToggleType.RadioButton</b> or <b>ItemToggleType.RadioCheckBox</b>.
    /// </remarks>
    [Category("Behavior")]
    [DefaultValue(false)]
    [Description("Whether this item is checked.")]
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
    /// Direction in which the subgroups expand.
    /// </summary>
    /// <value>
    /// Default value is GroupExpandDirection.Auto.
    /// </value>
    [Category("Layout")]
    [DefaultValue(GroupExpandDirection.Auto)]
		[Description("Direction in which the subgroups expand.")]
    public GroupExpandDirection DefaultSubGroupExpandDirection
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupExpandDirection")];
        return (o != null) ? Utils.ParseGroupExpandDirection(o) :
          (this.ParentItem != null) ? this.ParentItem.DefaultSubGroupExpandDirection :
          this.ParentMenu.DefaultGroupExpandDirection;
      }
      set 
      {
        Properties[GetAttributeVarName("DefaultSubGroupExpandDirection")] = value.ToString();
      }
    }

    /// <summary>
    /// Offset along x-axis from subgroups' normal expand positions.
    /// </summary>
    /// <value>
    /// Default value is 0.
    /// </value>
    [Category("Layout")]
    [DefaultValue(0)]
		[Description("Offset along x-axis from subgroups' normal expand positions.")]
    public int DefaultSubGroupExpandOffsetX
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupExpandOffsetX")];
        return (o != null) ? Utils.ParseInt(o) :
          (this.ParentItem != null) ? this.ParentItem.DefaultSubGroupExpandOffsetX :
          this.ParentMenu.DefaultGroupExpandOffsetX;
      }
      set 
      {
        Properties[GetAttributeVarName("DefaultSubGroupExpandOffsetX")] = value.ToString();
      }
    }

    /// <summary>
    /// Offset along y-axis from subgroups' normal expand positions.
    /// </summary>
    /// <value>
    /// Default value is 0.
    /// </value>
    [Category("Layout")]
    [DefaultValue(0)]
		[Description("Offset along y-axis from subgroups' normal expand positions.")]
    public int DefaultSubGroupExpandOffsetY
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupExpandOffsetY")];
        return (o != null) ? Utils.ParseInt(o) :
          (this.ParentItem != null) ? this.ParentItem.DefaultSubGroupExpandOffsetY :
          this.ParentMenu.DefaultGroupExpandOffsetY;
      }
      set 
      {
        Properties[GetAttributeVarName("DefaultSubGroupExpandOffsetY")] = value.ToString();
      }
    }

    /// <summary>
    /// Height of subgroups.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
		[Description("Height of subgroups.")]
    public Unit DefaultSubGroupHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupHeight")];
        return (o != null) ? Unit.Parse(o) :
          (this.ParentItem != null) ? this.ParentItem.DefaultSubGroupHeight :
          this.ParentMenu.DefaultGroupHeight;
      }
      set
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("DefaultSubGroupHeight")] = value.ToString();
        }
        else
        {
          throw new Exception("Group dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// Spacing between subgroups' items.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
		[Description("Spacing between subgroups' items.")]
    public Unit DefaultSubGroupItemSpacing
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupItemSpacing")];
        return (o != null) ? Unit.Parse(o) :
          (this.ParentItem != null) ? this.ParentItem.DefaultSubGroupItemSpacing :
          this.ParentMenu.DefaultGroupItemSpacing;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("DefaultSubGroupItemSpacing")] = value.ToString();
        }
        else
        {
          throw new Exception("Group item spacing may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Orientation of subgroups.
    /// </summary>
    /// <value>
    /// Default value is GroupOrientation.Vertical.
    /// </value>
    [Category("Layout")]
    [DefaultValue(GroupOrientation.Vertical)]
		[Description("Orientation of subgroups.")]
    public GroupOrientation DefaultSubGroupOrientation
    {
      get
      {
        string o = this.Properties[GetAttributeVarName("DefaultSubGroupOrientation")];
        return (o != null) ? Utils.ParseGroupOrientation(o) :
          (this.ParentItem != null) ? this.ParentItem.DefaultSubGroupOrientation :
          this.ParentMenu.DefaultGroupOrientation;
      }
      set 
      {
        Properties[GetAttributeVarName("DefaultSubGroupOrientation")] = value.ToString();
      }
    }

		/// <summary>
		/// Width of subgroups.
		/// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
		[Category("Layout")]
		[DefaultValue(typeof(Unit),"")]
		[Description("Width of subgroups.")]
		public Unit DefaultSubGroupWidth
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("DefaultSubGroupWidth")];
				return (o != null) ? Unit.Parse(o) :
					(this.ParentItem != null) ? this.ParentItem.DefaultSubGroupWidth :
					this.ParentMenu.DefaultGroupWidth;
			}
			set 
			{
				if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
				{
					Properties[GetAttributeVarName("DefaultSubGroupWidth")] = value.ToString();
				}
				else
				{
					throw new Exception("Group dimensions may only be specified in pixels or percentages.");
				}
			}
		}

    /// <summary>
    /// Item's height.
    /// </summary>
    [Category("Layout")]
    [DefaultValue(typeof(Unit), "")]
    [Description("Item's height.")]
    public override Unit Height
    {
      get 
      {
        return Unit.Parse(this.Properties[GetAttributeVarName("Height")]); 
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("Height")] = value.ToString();
        }
        else
        {
          throw new Exception("Item dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// Collection of immediate child MenuItems.
    /// </summary>
    [Description("Collection of immediate child MenuItems.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
    [Browsable(false)]
    public MenuItemCollection Items
    {
      get
      {
        if (this.nodes == null)
        {
          nodes = new MenuItemCollection(ParentMenu, this);
        }
        return (MenuItemCollection)nodes;
      }
    }

    /// <summary>
    /// Identifier of the toggle button group this toggle button belongs to.
    /// </summary>
    /// <remarks>
    /// This property only has effect in items with <see cref="ToggleType"/> set to 
    /// <b>ItemToggleType.RadioButton</b> or <b>ItemToggleType.RadioCheckBox</b>.
    /// </remarks>
    [Category("Behavior")]
    [Description("Identifier of the toggle button group this toggle button belongs to.")]
    [DefaultValue(null)]
    public string ToggleGroupId
    {
      get
      {
        return this.Properties[GetAttributeVarName("ToggleGroupId")];
      }
      set
      {
        Properties[GetAttributeVarName("ToggleGroupId")] = value;
      }
    }

    /// <summary>
    /// Specifies whether the item is supposed to function as a checkbox.
    /// </summary>
    [Category("Behavior")]
    [DefaultValue(ItemToggleType.None)]
    [Description("Specifies whether the item is supposed to function as a checkbox.")]
    public ItemToggleType ToggleType
    {
      get
      {
        return Utils.ParseItemToggleType(this.Properties[GetAttributeVarName("ToggleType")]);
      }
      set
      {
        Properties[GetAttributeVarName("ToggleType")] = value.ToString();
      }
    }

    /// <summary>
    /// The Menu that this item belongs to.
    /// </summary>
    /// <remarks>
    /// This is a read-only property.
    /// </remarks>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Menu ParentMenu
    {
      get
      {
        return (Menu)this.navigator;
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
    public new MenuItem ParentItem
    {
      get
      {
        return (MenuItem) this.parentNode;
      }
    }

    /// <summary>
    /// Subgroup's CSS class.
    /// </summary>
    [Category("Appearance")]
		[Description("Subgroup's CSS class.")]
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
		/// Direction in which the subgroup expands.
		/// </summary>
    /// <value>
    /// Default value is GroupExpandDirection.Auto.
    /// </value>
		[Category("Layout")]
		[DefaultValue(GroupExpandDirection.Auto)]
		[Description("Direction in which the subgroup expands.")]
		public GroupExpandDirection SubGroupExpandDirection
		{
			get 
			{
				string o = this.Properties[GetAttributeVarName("SubGroupExpandDirection")];
				return (o != null) ? Utils.ParseGroupExpandDirection(o) : this.DefaultSubGroupExpandDirection;
			}
			set 
			{
				Properties[GetAttributeVarName("SubGroupExpandDirection")] = value.ToString();
			}
		}

		/// <summary>
    /// Offset along x-axis from subgroup's normal expand position.
    /// </summary>
    /// <value>
    /// Default value is 0.
    /// </value>
    [Category("Layout")]
    [DefaultValue(0)]
		[Description("Offset along x-axis from subgroup's normal expand position.")]
    public int SubGroupExpandOffsetX
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupExpandOffsetX")];
        return (o != null) ? Utils.ParseInt(o) : this.DefaultSubGroupExpandOffsetX;
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupExpandOffsetX")] = value.ToString();
      }
    }

    /// <summary>
    /// Offset along y-axis from subgroup's normal expand position.
    /// </summary>
    /// <value>
    /// Default value is 0.
    /// </value>
    [Category("Layout")]
    [DefaultValue(0)]
		[Description("Offset along y-axis from subgroup's normal expand position.")]
    public int SubGroupExpandOffsetY
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupExpandOffsetY")];
        return (o != null) ? Utils.ParseInt(o) : this.DefaultSubGroupExpandOffsetY;
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupExpandOffsetY")] = value.ToString();
      }
    }

    /// <summary>
    /// Height of subgroup.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
		[Description("Height of subgroup.")]
    public Unit SubGroupHeight
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupHeight")];
        return (o != null) ? Unit.Parse(o) : this.DefaultSubGroupHeight;
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("SubGroupHeight")] = value.ToString();
        }
        else
        {
          throw new Exception("Group dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// Spacing between subgroup's items.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    [Category("Layout")]
		[DefaultValue(typeof(Unit),"")]
		[Description("Spacing between subgroup's items.")]
		public Unit SubGroupItemSpacing
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupItemSpacing")];
        return (o != null) ? Unit.Parse(o) : this.DefaultSubGroupItemSpacing;
      }
      set 
      {
        if (value.Type == UnitType.Pixel)
        {
          Properties[GetAttributeVarName("SubGroupItemSpacing")] = value.ToString();
        }
        else
        {
          throw new Exception("Group item spacing may only be specified in pixels.");
        }
      }
    }

    /// <summary>
    /// Orientation of subgroup.
    /// </summary>
    /// <value>
    /// Default value is GroupOrientation.Vertical.
    /// </value>
    [Category("Layout")]
    [DefaultValue(GroupOrientation.Vertical)]
		[Description("Orientation of subgroup.")]
    public GroupOrientation SubGroupOrientation
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupOrientation")];
        return (o != null) ? Utils.ParseGroupOrientation(o) : this.DefaultSubGroupOrientation;
      }
      set 
      {
        Properties[GetAttributeVarName("SubGroupOrientation")] = value.ToString();
      }
    }

    /// <summary>
    /// Width of subgroup.
    /// </summary>
    /// <value>
    /// Default value is Unit.Empty.
    /// </value>
    [Category("Layout")]
    [DefaultValue(typeof(Unit),"")]
		[Description("Width of subgroup.")]
    public Unit SubGroupWidth
    {
      get 
      {
        string o = this.Properties[GetAttributeVarName("SubGroupWidth")];
        return (o != null) ? Unit.Parse(o) : this.DefaultSubGroupWidth;
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("SubGroupWidth")] = value.ToString();
        }
        else
        {
          throw new Exception("Group dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    /// <summary>
    /// Item's width.
    /// </summary>
    [Category("Layout")]
		[Description("Item's width.")]
    public override Unit Width
    {
      get 
      {
        return Unit.Parse(this.Properties[GetAttributeVarName("Width")]); 
      }
      set 
      {
        if (value.Type == UnitType.Pixel || value.Type == UnitType.Percentage)
        {
          Properties[GetAttributeVarName("Width")] = value.ToString();
        }
        else
        {
          throw new Exception("Item dimensions may only be specified in pixels or percentages.");
        }
      }
    }

    #endregion

    #region Protected Properties
    #endregion

    #region Internal Properties

    /// <summary>
    /// Creates a new MenuItem and adds it to this one's subgroup.
    /// </summary>
    /// <returns>The newly created child item.</returns>
    internal override NavigationNode AddNode()
    {
      MenuItem newItem = new MenuItem();
      this.Items.Add(newItem);
      return newItem;
    }

    #endregion

    #region Private Properties
    #endregion

    #region Public Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Internal Methods
    #endregion

    #region Private Methods
    #endregion
  }
}
