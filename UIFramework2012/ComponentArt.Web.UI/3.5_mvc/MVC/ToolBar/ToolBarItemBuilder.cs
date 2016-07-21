using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define ToolBarItem objects.
  /// </summary>
  public class ToolBarItemBuilder
  {
    private readonly ToolBarItem item;
    /// <summary>
    /// Builder to define ToolBarItem objects.
    /// </summary>
    /// <param name="item"></param>
    public ToolBarItemBuilder(ToolBarItem item)
    {
      this.item = item;
    }
    /// <summary>
    /// Whether to perform a postback when this item is selected. Default: false. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder AutoPostBackOnSelect(bool value)
    {
      item.AutoPostBackOnSelect = value;
      return this;
    }
/// <summary>
    /// Client-side command to execute on click. This can be any valid client script. 
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
    public ToolBarItemBuilder ClientSideCommand(string value)
    {
      item.ClientSideCommand = value;
      return this;
    }
    /// <summary>
    /// ID of the client template to use for this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ClientTemplateId(string value)
    {
      item.ClientTemplateId = value;

      return this;
    }
    /// <summary>
    /// The position of the dropdown image relative to rest of the item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder DropDownImagePosition(ToolBarDropDownImagePosition value)
    {
        item.DropDownImagePosition = value;
        return this;
    }
    /// <summary>
    /// Type of this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ItemType(ToolBarItemType value)
    {
        item.ItemType = value;
        return this;
    }
    /// <summary>
    /// Item's image height. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ImageHeight(Unit value)
    {
        item.ImageHeight = value;
        return this;
    }
    /// <summary>
    /// Item's image width. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ImageWidth(Unit value)
    {
        item.ImageWidth = value;
        return this;
    }
    /// <summary>
    /// CSS class of this item when it is Checked. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder CheckedCssClass(string value)
    {
        item.CheckedCssClass = value;
        return this;
    }
    /// <summary>
    /// CSS class of this item when it is not Enabled and it is Checked. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder DisabledCheckedCssClass(string value)
    {
        item.DisabledCheckedCssClass = value;
        return this;
    }
    /// <summary>
    /// CSS class to use for this item on hover (on mouse over) when it is Checked. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder CheckedHoverCssClass(string value)
    {
        item.CheckedHoverCssClass = value;
        return this;
    }
    /// <summary>
    /// CSS class to use for this item when active (on mouse down) when it is Checked. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder CheckedActiveCssClass(string value)
    {
        item.CheckedActiveCssClass = value;
        return this;
    }
    /// <summary>
    /// CSS class to use for this item when active (on mouse down). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ActiveCssClass(string value)
    {
        item.ActiveCssClass = value;
        return this;
    }
    /// <summary>
    /// CSS class of this item when it is not Enabled. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder DisabledCssClass(string value)
    {
        item.DisabledCssClass = value;
        return this;
    }
    /// <summary>
    /// CSS class of this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder CssClass(string value)
    {
      item.CssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class to use for this item when its dropdown menu is expanded. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ExpandedCssClass(string value)
    {
        item.ExpandedCssClass = value;
        return this;
    }
    /// <summary>
    /// CSS class to use for this item on hover (on mouse over). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder HoverCssClass(string value)
    {
        item.HoverCssClass = value;
        return this;
    }
    /// <summary>
    /// ID of the child control to use for content of this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder CustomContentId(string value)
    {
        item.CustomContentId = value;
        return this;
    }
    /// <summary>
    /// CSS class to apply in the Themes' designated icon area for this item.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder IconCssClass(string value)
    {
      item.Properties["IconCssClass"] = value;
      return this;
    }
    /// <summary>
    /// Image to draw in the Themes' designated icon area for this item.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder IconUrl(string value)
    {
      item.Properties["IconUrl"] = value;
      return this;
    }
    /// <summary>
    /// ID of this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ID(string value)
    {
      item.ID = value;
      return this;
    }
    /// <summary>
    /// Whether this item is enabled. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder Enabled(bool value)
    {
      item.Enabled = value;
      return this;
    }
    /// <summary>
    /// Whether this item is checked. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder Checked(bool value)
    {
        item.Checked = value;
        return this;
    }
    /// <summary>
    /// Whether to allow HTML content in this item's Text field. Default: true. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder AllowHtmlContent(bool value)
    {
        item.AllowHtmlContent = value;
        return this;
    }
    /// <summary>
    /// String representing keyboard shortcut for selecting this node. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder KeyboardShortcut(string value)
    {
      item.KeyboardShortcut = value;
      return this;
    }
 /// <summary>
    /// Text label of this item. 
 /// </summary>
 /// <param name="value"></param>
 /// <returns></returns>
    public ToolBarItemBuilder Text(string value)
    {
      item.Text = value;
      return this;
    }
    /// <summary>
    /// The text alignment to apply to this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder TextAlign(TextAlign value)
    {
        item.TextAlign = value;
        return this;
    }
    /// <summary>
    /// The position of item text and image relative to each other. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder TextImageRelation(ToolBarTextImageRelation value)
    {
        item.TextImageRelation = value;
        return this;
    }
    /// <summary>
    /// The gap in pixels between item text and image. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder TextImageSpacing(int value)
    {
        item.TextImageSpacing = value;
        return this;
    }
    /// <summary>
    /// Whether to wrap text in this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder TextWrap(bool value)
    {
        item.TextWrap = value;
        return this;
    }
    /// <summary>
    /// Item's dropdown image height. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder DropDownImageHeight(Unit value)
    {
        item.DropDownImageHeight = value;
        return this;
    }
    /// <summary>
    /// Item's dropdown image width. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder DropDownImageWidth(Unit value)
    {
        item.DropDownImageWidth = value;
        return this;
    }
    /// <summary>
    /// Identifier of the toggle group this item belongs to. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ToggleGroupId(string value)
    {
        item.ToggleGroupId = value;
        return this;
    }
    /// <summary>
    /// ID of ComponentArt Menu to act as a dropdown for this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder DropDownId(string value)
    {
        item.DropDownId = value;
        return this;
    }
    /// <summary>
    /// The URL of the image to use in this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ImageUrl(string value)
    {
        item.ImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the image to use in this item when its dropdown menu is expanded. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ExpandedImageUrl(string value)
    {
        item.ExpandedImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the dropdown image to use in this item when its dropdown menu is expanded. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ExpandedDropDownImageUrl(string value)
    {
        item.ExpandedDropDownImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the image to use in this item when it is Checked. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder CheckedImageUrl(string value)
    {
        item.CheckedImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the dropdown image to use in this item.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder DropDownImageUrl(string value)
    {
        item.DropDownImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the image to use in this item when it is not Enabled and it is Checked. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder DisabledCheckedImageUrl(string value)
    {
        item.DisabledCheckedImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the image to use in this item on hover (on mouse over) when it is Checked. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder CheckedHoverImageUrl(string value)
    {
        item.CheckedHoverImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the image to use in this item when active (on mouse down) when it is Checked. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder CheckedActiveImageUrl(string value)
    {
        item.CheckedActiveImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the image to use in this item when active (on mouse down). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ActiveImageUrl(string value)
    {
        item.ActiveImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the dropdown image to use in this item when active (on mouse down). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ActiveDropDownImageUrl(string value)
    {
        item.ActiveDropDownImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the dropdown image to use in this item when it is not Enabled. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder DisabledDropDownImageUrl(string value)
    {
        item.DisabledDropDownImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the image to use in this item when it is not Enabled. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder DisabledImageUrl(string value)
    {
        item.DisabledImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the dropdown image to use in this item on hover (on mouse over). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder HoverDropDownImageUrl(string value)
    {
        item.HoverDropDownImageUrl = value;
        return this;
    }
    /// <summary>
    /// The URL of the image to use in this item on hover (on mouse over). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder HoverImageUrl(string value)
    {
        item.HoverImageUrl = value;
        return this;
    }
    /// <summary>
    /// ToolTip to display for this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder ToolTip(string value)
    {
      item.ToolTip = value;
      return this;
    }
    /// <summary>
    /// Optional internal string value of this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder Value(string value)
    {
      item.Value = value;
      return this;
    }
    /// <summary>
    /// Whether this item should be displayed. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ToolBarItemBuilder Visible(bool value)
    {
      item.Visible = value;
      return this;
    }   
  }
}
