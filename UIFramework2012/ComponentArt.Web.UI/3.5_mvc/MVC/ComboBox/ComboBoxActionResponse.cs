using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// State properties sent from server to client.
  /// </summary>
  public class ComboBoxActionResponse
  {
    /// <summary>
    /// Data to be loaded into the ComboBox.
    /// </summary>
    public object Data
    {
      get;
      set;
    }
    /// <summary>
    /// Total count of items in the data source.
    /// </summary>
    public int ItemCount
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
  }
}
