using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class to define DataGridItem objects for collections.
  /// </summary>
  public class DataGridItemFactory
  {
    private GridItemCollection items;
    private DataGrid grid;
    /// <summary>
    /// Factory to define GridItemCollections objects for DataGrid.
    /// </summary>
    /// <param name="items"></param>
    /// <param name="grid"></param>
    public DataGridItemFactory(GridItemCollection items, DataGrid grid)
    {
      this.items = items;
      this.grid = grid;
    }
    /// <summary>
    /// Add a DataGridItem to the DataGrid's top-level GridItemCollection.
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public virtual DataGridItemBuilder Add(object[] itemData)
    {
      GridItem item = new GridItem(grid, 0, itemData);  // In-line defined items are always level 0
      
      items.Add(item);

      return new DataGridItemBuilder(item, grid);
    }
  }
}
