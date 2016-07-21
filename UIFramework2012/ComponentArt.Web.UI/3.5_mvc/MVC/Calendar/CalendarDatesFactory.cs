using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComponentArt.Web.UI
{
  public class CalendarDateFactory
  {
    private DateCollection dates;
    /// <summary>
    /// Factory to define DateTimes for collections.
    /// </summary>
    /// <param name="dates"></param>
    public CalendarDateFactory(DateCollection dates)
    {
      this.dates = dates;
    }
    /// <summary>
    /// Add a DateTime to a collection.
    /// </summary>
    public virtual void Add()
    {
      DateTime day = new DateTime();

      dates.Add(day);
    }
  }
}