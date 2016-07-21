using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Factory class to define DataGridColumn objects for collections.
  /// </summary>
  public class DataGridColumnFactory
  {
    private GridColumnCollection columns;
    /// <summary>
    /// Factory to define DataGridColumns objects.
    /// </summary>
    /// <param name="columns"></param>
    public DataGridColumnFactory(GridColumnCollection columns)
    {
      this.columns = columns;
    }
    /// <summary>
    /// Add a DataGridColumn to a Level's Columns collection.
    /// </summary>
    public virtual DataGridColumnBuilder Add()
    {
      GridColumn column = new GridColumn();

      columns.Add(column);

      return new DataGridColumnBuilder(column);
    }
  }
}
