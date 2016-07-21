using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define MenuItem objects.
  /// </summary>
  public class MenuItemBuilder
  {
    private readonly MenuItem item;
    /// <summary>
    /// Builder to define MenuItem objects.
    /// </summary>
    /// <param name="item"></param>
    public MenuItemBuilder(MenuItem item)
    {
      this.item = item;
    }
    /// <summary>
    /// Collection of immediate child MenuItems. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public MenuItemBuilder Items(Action<MenuItemFactory> addAction)
    {
      var factory = new MenuItemFactory(item.Items);
      addAction(factory);
      return this;
    }
    /// <summary>
    /// Whether this item is checked. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder Checked(bool value)
    {
      item.Checked = value;
      return this;
    }
    /// <summary>
    /// Direction in which the subgroups expand. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder DefaultSubGroupExpandDirection(GroupExpandDirection value)
    {
      item.DefaultSubGroupExpandDirection = value;
      return this;
    }
    /// <summary>
    /// Offset along x-axis from subgroups' normal expand positions. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder DefaultSubGroupExpandOffsetX(int value)
    {
      item.DefaultSubGroupExpandOffsetX = value;
      return this;
    }
    /// <summary>
    /// Offset along y-axis from subgroups' normal expand positions. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder DefaultSubGroupExpandOffsetY(int value)
    {
      item.DefaultSubGroupExpandOffsetY = value;
      return this;
    }
    /// <summary>
    /// Height of subgroups. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder DefaultSubGroupHeight(int value)
    {
      item.DefaultSubGroupHeight = value;
      return this;
    }
    /// <summary>
    /// Spacing between subgroups' items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder DefaultSubGroupItemSpacing(System.Web.UI.WebControls.Unit value)
    {
      item.DefaultSubGroupItemSpacing = value;
      return this;
    }
    /// <summary>
    /// Orientation of subgroups. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder DefaultSubGroupOrientation(GroupOrientation value)
    {
      item.DefaultSubGroupOrientation = value;
      return this;
    }
    /// <summary>
    /// Width of subgroups. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder DefaultSubGroupWidth(int value)
    {
      item.DefaultSubGroupWidth = value;
      return this;
    }
    /// <summary>
    /// Subgroup's CSS class. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder SubGroupCssClass(string value)
    {
      item.SubGroupCssClass = value;
      return this;
    }
    /// <summary>
    /// Direction in which the subgroup expands. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder SubGroupExpandDirection(GroupExpandDirection value)
    {
      item.SubGroupExpandDirection = value;
      return this;
    }
    /// <summary>
    /// Offset along x-axis from subgroup's normal expand position. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder SubGroupExpandOffsetX(int value)
    {
      item.SubGroupExpandOffsetX = value;
      return this;
    }
    /// <summary>
    /// Offset along y-axis from subgroup's normal expand position. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder SubGroupExpandOffsetY(int value)
    {
      item.SubGroupExpandOffsetY = value;
      return this;
    }
    /// <summary>
    /// Height of subgroup. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder SubGroupHeight(int value)
    {
      item.SubGroupHeight = value;
      return this;
    }
    /// <summary>
    /// Spacing between subgroup's items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder SubGroupItemSpacing(System.Web.UI.WebControls.Unit value)
    {
      item.SubGroupItemSpacing = value;
      return this;
    }
/// <summary>
    /// Orientation of subgroup. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public MenuItemBuilder SubGroupOrientation(GroupOrientation value)
    {
      item.SubGroupOrientation = value;
      return this;
    }
/// <summary>
    /// Width of subgroup. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public MenuItemBuilder SubGroupWidth(int value)
    {
      item.SubGroupWidth = value;
      return this;
    }

    // Inherited from Navigation Node

/// <summary>
    /// Whether to perform a postback when this node is selected. Default: false. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public MenuItemBuilder AutoPostBackOnSelect(bool value)
    {
      item.AutoPostBackOnSelect = value;
      return this;
    }
    /// <summary>
    /// Client-side command to execute on selection. This can be any valid client script.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder ClientSideCommand(string value)
    {
      item.ClientSideCommand = value;
      return this;
    }
/// <summary>
    /// ID of the client template to use for this node. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public MenuItemBuilder ClientTemplateId(string value)
    {
      item.ClientTemplateId = value;

      return this;
    }
    /// <summary>
    /// Default CSS class to use for this node.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder CssClass(string value)
    {
      item.CssClass = value;
      return this;
    }
    /// <summary>
    /// ID of this node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder ID(string value)
    {
      item.ID = value;
      return this;
    }
/// <summary>
    /// CSS class to apply in a Themes' designated icon area for this item.
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public MenuItemBuilder IconCssClass(string value)
    {
      item.Properties["IconCssClass"] = value;
      return this;
    }
    /// <summary>
    /// Image to draw in a Themes' designated icon area for this item.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder IconUrl(string value)
    {
      item.Properties["IconUrl"] = value;
      return this;
    }
    /// <summary>
    /// Whether this navigation node is enabled.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder Enabled(bool value)
    {
      item.Enabled = value;
      return this;
    }
    /// <summary>
    /// String representing keyboard shortcut for selecting this node.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder KeyboardShortcut(string value)
    {
      item.KeyboardShortcut = value;
      return this;
    }
    /// <summary>
    /// URL to navigate to when this node is selected. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder NavigateUrl(string value)
    {
      item.NavigateUrl = value;
      return this;
    }
    /// <summary>
    /// ID of PageView to switch the target ComponentArt MultiPage control to when this node is selected. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder PageViewId(string value)
    {
      item.PageViewId = value;
      return this;
    }
    /// <summary>
    /// XML file to load the substructure of this node from. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder SiteMapXmlFile(string value)
    {
      item.SiteMapXmlFile = value;
      return this;
    }
    /// <summary>
    /// Target frame for this node's navigation.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder Target(string value)
    {
      item.Target = value;
      return this;
    }
    /// <summary>
    /// Text label of this node.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder Text(string value)
    {
      item.Text = value;
      return this;
    }
    /// <summary>
    /// ToolTip to display for this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder ToolTip(string value)
    {
      item.ToolTip = value;
      return this;
    }
    /// <summary>
    /// Optional internal string value of this node.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder Value(string value)
    {
      item.Value = value;
      return this;
    }
    /// <summary>
    /// Whether this node should be displayed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder Visible(bool value)
    {
      item.Visible = value;
      return this;
    }

    // Inherited from BaseMenuItem
    /// <summary>
    /// Default CSS class to apply to sub-groups below this item, including this item's subgroup. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder DefaultSubGroupCssClass(string value)
    {
      item.DefaultSubGroupCssClass = value;
      return this;
    }
    /// <summary>
    /// The default text alignment to apply to labels of sub-items.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder DefaultSubItemTextAlign(TextAlign value)
    {
      item.DefaultSubItemTextAlign = value;
      return this;
    }
    /// <summary>
    /// Whether to wrap text in sub-item labels by default. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder DefaultSubItemTextWrap(bool value)
    {
      item.DefaultSubItemTextWrap = value;
      return this;
    }
    /// <summary>
    /// Whether to wrap text in this node's label.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public MenuItemBuilder TextWrap(bool value)
    {
      item.TextWrap = value;
      return this;
    }


  }
}
