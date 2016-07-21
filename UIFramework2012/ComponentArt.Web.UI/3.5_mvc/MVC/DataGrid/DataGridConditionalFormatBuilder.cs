using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define GridConditionalFormat objects.
  /// </summary>
  public class DataGridConditionalFormatBuilder
  {
    GridConditionalFormat format = new GridConditionalFormat();
    /// <summary>
    /// Builder to define GridConditionalFormat objects.
    /// </summary>
    /// <param name="format"></param>
    public DataGridConditionalFormatBuilder(GridConditionalFormat format)
    {
      this.format = format;
    }
    /// <summary>
    /// Client expression that, when evaluated to true, applies its CSS classes to the row.
    /// </summary>
    /// <param name="value"></param>
    public DataGridConditionalFormatBuilder ClientFilter(string value)
    {
      format.ClientFilter = value;

      return this;
    }
    /// <summary>
    /// CSS class to apply on row hovers.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridConditionalFormatBuilder HoverRowCssClass(string value)
    {
      format.HoverRowCssClass = value;

      return this;
    }
    /// <summary>
    /// CSS class to apply to the row.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridConditionalFormatBuilder RowCssClass(string value)
    {
      format.RowCssClass = value;

      return this;
    }
    /// <summary>
    /// CSS class to apply to the row when selected while hovered.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridConditionalFormatBuilder SelectedHoverRowCssClass(string value)
    {
      format.SelectedHoverRowCssClass = value;

      return this;
    }
    /// <summary>
    /// CSS class to apply to the row when selected.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public DataGridConditionalFormatBuilder SelectedRowCssClass(string value)
    {
      format.SelectedRowCssClass = value;

      return this;
    }
  }
}
