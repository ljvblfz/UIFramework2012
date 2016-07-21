using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Builder class to define DataGridItem objects.
  /// </summary>
  public class DataGridItemBuilder
  {
    private readonly GridItem item;
    private DataGrid grid;
    /// <summary>
    /// Builder to define DataGridItem objects.  The DataGridLevel must be defined before these can be added.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="grid"></param>
    public DataGridItemBuilder(GridItem item, DataGrid grid)
    {
      this.item = item;
      this.grid = grid;
    }
    /// <summary>
    /// Collection of child DataGridItems.
    /// </summary>
    /// <param name="addAction"></param>
    /// <returns></returns>
    public DataGridItemBuilder Items(Action<DataGridItemFactory> addAction)
    {
      var factory = new DataGridItemFactory(item.Items, grid);
      addAction(factory);
      return this;
    }
  }
}
