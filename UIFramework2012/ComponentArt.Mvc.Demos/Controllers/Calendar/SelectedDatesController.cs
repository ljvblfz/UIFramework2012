using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ComponentArt.Web.UI;

namespace ComponentArt.Mvc.Demos.Controllers
{
  public partial class CalendarController : Controller
  {
    public ActionResult SelectedDates()
    {
      CalendarActionResponse actionResponse = GetPostResponse(null);

      ViewData["MyPickerDate"] = actionResponse;

      return View(actionResponse);
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult SelectedDates(FormCollection formCollection)
    {
      // Picker1 persists in ViewData
      DateTime pickerSelectedDate = ComponentArt.Web.UI.CalendarHelperExtensions.GetSelectedDate("Picker1", formCollection);
      DateTime pickerVisibleDate = ComponentArt.Web.UI.CalendarHelperExtensions.GetVisibleDate("Picker1", formCollection);

      CalendarActionResponse pickerActionResponse = new CalendarActionResponse();
      pickerActionResponse.SelectedDate = pickerSelectedDate;
      pickerActionResponse.VisibleDate = pickerVisibleDate;
      
      ViewData["MyPickerDate"] = pickerActionResponse;

      // Calendar1 persists in the View Model
      DateCollection selectedDates = ComponentArt.Web.UI.CalendarHelperExtensions.GetSelectedDates("Calendar1", formCollection);
      DateTime visibleDate = ComponentArt.Web.UI.CalendarHelperExtensions.GetVisibleDate("Calendar1", formCollection);

      CalendarActionResponse actionResponse = new CalendarActionResponse();
      actionResponse.SelectedDates = selectedDates;
      actionResponse.VisibleDate = visibleDate;

      return View(actionResponse);
    }
  }
}
