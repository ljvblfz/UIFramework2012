using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// State properties sent from server to client.
  /// </summary>
  public class DataGridActionResponse
  {
   /// <summary>
   /// Row data to be loaded into the DataGrid.
   /// </summary>
    public object Data
    {
      get;
      set;
    }
    /// <summary>
    /// Total count of items in the data source.
    /// </summary>
    public int RecordCount
    {
      get;
      set;
    }
    /// <summary>
    /// Page of data presently shown.
    /// </summary>
    public int CurrentPageIndex
    {
      get;
      set;
    }
    /// <summary>
    /// The number of items to render per page of the Grid. 
    /// </summary>
    public int PageSize
    {
      get;
      set;
    }
    /// <summary>
    ///  Custom filter (SQL WHERE clause) to apply to data.
    /// </summary>
    public string Filter
    {
      get;
      set;
    }
    /// <summary>
    /// The grouping (SQL GROUP BY expression) to use on the data. 
    /// </summary>
    public string GroupBy
    {
      get;
      set;
    }
    /// <summary>
    /// The sort order (SQL ORDER BY expression) to use on the data. 
    /// </summary>
    public string Sort
    {
      get;
      set;
    }
  }
}
