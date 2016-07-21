using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// State properties sent from client to server.
  /// </summary>
  public class DataGridActionRequest
  {
    private int take = 20;
    private int skip = 0;
    /// <summary>
    /// Number of records requested.
    /// </summary>
    public int Take
    {
      get
      {
        return take;
      }
      set
      {
        take = value;
      }
    }
    /// <summary>
    /// Number of records to pass over before grabbing the requested Take records.
    /// </summary>
    public int Skip
    {
      get
      {
        return skip;
      }
      set
      {
        skip = value;
      }
    }
    /// <summary>
    /// Custom filter (SQL WHERE clause) to apply to data.
    /// </summary>
    public string Filter
    {
      get;
      set;
    }
    /// <summary>
    /// SQL SORT BY clause to apply to groups.
    /// </summary>
    public string GroupOrder
    {
      get;
      set;
    }
    /// <summary>
    /// The sort order (SQL ORDER BY expression) to use on the data. 
    /// </summary>
    public string Order
    {
      get;
      set;
    }
  }
}
