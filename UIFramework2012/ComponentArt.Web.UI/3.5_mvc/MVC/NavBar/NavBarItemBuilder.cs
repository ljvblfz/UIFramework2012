using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define NavBarItem objects.
  /// </summary>
  public class NavBarItemBuilder
  {
    private readonly NavBarItem item;
    /// <summary>
    /// Builder to define NavBarItem objects.
    /// </summary>
    /// <param name="item"></param>
    public NavBarItemBuilder(NavBarItem item)
    {
      this.item = item;
    }
    /// <summary>
    /// Collection of immediate child NavBarItems. 
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public NavBarItemBuilder Items(Action<NavBarItemFactory> addAction)
    {
      var factory = new NavBarItemFactory(item.Items);
      addAction(factory);
      return this;
    }
    /// <summary>
    /// Whether to perform a postback when a group is collapsed. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder AutoPostBackOnCollapse(bool value)
    {
      item.AutoPostBackOnCollapse = value;
      return this;
    }
    /// <summary>
    /// Whether to perform a postback when an item is expanded. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder AutoPostBackOnExpand(bool value)
    {
      item.AutoPostBackOnExpand = value;
      return this;
    }
    /// <summary>
    /// Whether this item can be selected. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder Selectable(bool value)
    {
      item.Selectable = value;
      return this;
    }
    /// <summary>
    /// Whether this item is expanded. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder Expanded(bool value)
    {
      item.Expanded = value;
      return this;
    }
    /// <summary>
    /// The CSS class to use for this item's subgroup. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder SubGroupCssClass(string value)
    {
      item.SubGroupCssClass = value;
      return this;
    }
    /// <summary>
    /// The height to force for this item's subgroup. If specified, regardless of its actual size, the 
    /// subgroup will be expanded to exactly the height specified here. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder SubGroupHeight(int value)
    {
      item.SubGroupHeight = value;
      return this;
    }
    /// <summary>
    /// The spacing (in pixels) to render between items in this item's subgroup. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder SubGroupItemSpacing(int value)
    {
      item.SubGroupItemSpacing = value;
      return this;
    }
      
    // Inherited from NavigationNode
    /// <summary>
    /// Whether to perform a postback when this node is selected. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder AutoPostBackOnSelect(bool value)
    {
      item.AutoPostBackOnSelect = value;
      return this;
    }
    /// <summary>
    /// Client-side command to execute on selection. This can be any valid client script.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder ClientSideCommand(string value)
    {
      item.ClientSideCommand = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for this node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder ClientTemplateId(string value)
    {
      item.ClientTemplateId = value;

      return this;
    }
    /// <summary>
    /// Default CSS class to use for this node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder CssClass(string value)
    {
      item.CssClass = value;
      return this;
    }
    /// <summary>
    /// ID of this node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder ID(string value)
    {
      item.ID = value;
      return this;
    }
    /// <summary>
    /// CSS class to apply in the Themes' designated icon area for this item.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder IconCssClass(string value)
    {
      item.Properties["IconCssClass"] = value;
      return this;
    }
    /// <summary>
    /// Image to draw in the Themes' designated icon area for this item.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder IconUrl(string value)
    {
      item.Properties["IconUrl"] = value;
      return this;
    }
    /// <summary>
    /// Whether this navigation node is enabled. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder Enabled(bool value)
    {
      item.Enabled = value;
      return this;
    }    
    /// <summary>
    /// String representing keyboard shortcut for selecting this node.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder KeyboardShortcut(string value)
    {
      item.KeyboardShortcut = value;
      return this;
    }
    /// <summary>
    /// URL to navigate to when this node is selected.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder NavigateUrl(string value)
    {
      item.NavigateUrl = value;
      return this;
    }
    /// <summary>
    /// ID of PageView to switch the target ComponentArt MultiPage control to when this node is selected. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder PageViewId(string value)
    {
      item.PageViewId = value;
      return this;
    }
    /// <summary>
    /// XML file to load the substructure of this node from. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder SiteMapXmlFile(string value)
    {
      item.SiteMapXmlFile = value;
      return this;
    }
    /// <summary>
    /// Target frame for this node's navigation. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder Target(string value)
    {
      item.Target = value;
      return this;
    }
    /// <summary>
    /// Text label of this node.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder Text(string value)
    {
      item.Text = value;
      return this;
    }
    /// <summary>
    /// ToolTip to display for this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder ToolTip(string value)
    {
      item.ToolTip = value;
      return this;
    }
    /// <summary>
    /// Optional internal string value of this node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder Value(string value)
    {
      item.Value = value;
      return this;
    }
    /// <summary>
    /// Whether this node should be displayed. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder Visible(bool value)
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
    public NavBarItemBuilder DefaultSubGroupCssClass(string value)
    {
      item.DefaultSubGroupCssClass = value;
      return this;
    }
    /// <summary>
    /// The default text alignment to apply to labels of sub-items. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder DefaultSubItemTextAlign(TextAlign value)
    {
      item.DefaultSubItemTextAlign = value;
      return this;
    }
    /// <summary>
    /// Whether to wrap text in sub-item labels by default. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder DefaultSubItemTextWrap(bool value)
    {
      item.DefaultSubItemTextWrap = value;
      return this;
    }
    /// <summary>
    /// Whether to wrap text in this node's label. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavBarItemBuilder TextWrap(bool value)
    {
      item.TextWrap = value;
      return this;
    }

    
  }
}
