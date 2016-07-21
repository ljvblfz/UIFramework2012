using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  public class CalendarDayBuilder
  {
    private readonly CalendarDay day;
    /// <summary>
    /// Builder to define CalendarDay objects.
    /// </summary>
    /// <param name="day"></param>
    public CalendarDayBuilder(CalendarDay day)
    {
      this.day = day;
    }
    /// <summary>
    /// Active CSS class for this day.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarDayBuilder ActiveCssClass(string value)
    {
      day.ActiveCssClass = value;
      return this;
    }
    /// <summary>
    /// CSS class for this day.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarDayBuilder CssClass(string value)
    {
      day.CssClass = value;
      return this;
    }
    /// <summary>
    /// Date this CalendarDay corresponds to.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarDayBuilder Date(DateTime value)
    {
      day.Date = value;
      return this;
    }
    /// <summary>
    /// Hover CSS class for this day.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public CalendarDayBuilder HoverCssClass(string value)
    {
      day.HoverCssClass = value;
      return this;
    }
  }
}