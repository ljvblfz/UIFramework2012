using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define ComboBoxItem objects.
  /// </summary>
  public class ComboBoxItemBuilder
  {
    private readonly ComboBoxItem item;
    /// <summary>
    /// Builder to define ComboBoxItem objects.
    /// </summary>
    /// <param name="item"></param>
    public ComboBoxItemBuilder(ComboBoxItem item)
    {
      this.item = item;
    }
    /// <summary>
    /// The client template to use for this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxItemBuilder ClientTemplateId(string value)
    {
      item.ClientTemplateId = value;
      return this;
    }
    /// <summary>
    /// The CSS class to use for this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxItemBuilder CssClass(string value)
    {
      item.CssClass = value;
      return this;
    }
    /// <summary>
    /// Whether this item is enabled. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxItemBuilder Enabled(bool value)
    {
      item.Enabled = value;
      return this;
    }
    /// <summary>
    /// The unique identifier of this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxItemBuilder Id(string value)
    {
      item.Id = value;
      return this;
    }
    /// <summary>
    /// Whether this item is currently selected. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxItemBuilder Selected(bool value)
    {
      item.Selected = value;
      return this;
    }
    /// <summary>
    /// The text for this item. 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxItemBuilder Text(string value)
    {
      item.Text = value;
      return this;
    }
    /// <summary>
    /// The value for this item (if different from text). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public ComboBoxItemBuilder Value(string value)
    {
      item.Value = value;
      return this;
    }
  }
}