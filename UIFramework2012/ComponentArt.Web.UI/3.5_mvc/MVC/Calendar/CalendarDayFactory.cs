using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  public class CalendarDayFactory
  {
    private CalendarDayCollection days;
    /// <summary>
    /// Factory to define CalendarDay objects for collections.
    /// </summary>
    /// <param name="days"></param>
    public CalendarDayFactory(CalendarDayCollection days)
    {
      this.days = days;
    }
    /// <summary>
    /// Add a CalendarDay to a collection.
    /// </summary>
    public virtual CalendarDayBuilder Add()
    {
      CalendarDay day = new CalendarDay();

      days.Add(day);

      return new CalendarDayBuilder(day);
    }
  }
}