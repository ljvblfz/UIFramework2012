using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ComponentArt.Web.UI;
using System.Collections;

namespace ComponentArt.Mvc.Demos.Controllers
{
  public partial class CalendarController : Controller
  {
    public ActionResult DisabledDates()
    {
      DateTime defaultDate = DateTime.Now;

      ArrayList dates = new ArrayList();
      dates.Add(defaultDate.AddDays(1));
      dates.Add(defaultDate.AddDays(3));
      dates.Add(defaultDate.AddDays(10));
      dates.Add(defaultDate.AddDays(11));
      dates.Add(defaultDate.AddDays(-2));
      dates.Add(defaultDate.AddDays(-6));
      dates.Add(defaultDate.AddDays(-7));
      DateCollection disabledDates = new DateCollection(dates);

      CalendarActionResponse actionResponse = new CalendarActionResponse();
      actionResponse.DisabledDates = disabledDates;
      actionResponse.SelectedDate = defaultDate;
      actionResponse.VisibleDate = defaultDate;

      return View(actionResponse);
    }
  }
}
