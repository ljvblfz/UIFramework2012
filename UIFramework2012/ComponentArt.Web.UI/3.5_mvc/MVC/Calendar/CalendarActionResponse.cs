using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Core state properties sent from server to client.
  /// </summary>
  public class CalendarActionResponse
  {
    /// <summary>
    /// Collection of DateTime objects that represent the disabled dates in the Calendar.
    /// </summary>
    public DateCollection DisabledDates
    {
      get;
      set;
    }
    /// <summary>
    /// The selected DateTime.
    /// </summary>
    public DateTime SelectedDate
    {
      get;
      set;
    }
    /// <summary>
    /// Collection of DateTime objects that represent the selected dates on the Calendar. 
    /// </summary>
    public DateCollection SelectedDates
    {
      get;
      set;
    }
    /// <summary>
    /// DateTime that is displayed in the Calendar control.
    /// </summary>
    public DateTime VisibleDate
    {
      get;
      set;
    }
  }
}
