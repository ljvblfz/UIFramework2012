using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Core state properties sent from client to server.
  /// </summary>
  public class CalendarActionRequest
  {
    /// <summary>
    /// DateTime selected on client.
    /// </summary>
    public DateTime SelectedDate
    {
      get;
      set;
    }
    /// <summary>
    /// DateTime visible on client.
    /// </summary>
    public DateTime VisibleDate
    {
      get;
      set;
    }
  }
}
