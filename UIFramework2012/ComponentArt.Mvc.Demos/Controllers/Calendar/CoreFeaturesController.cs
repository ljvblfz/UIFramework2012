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
    public ActionResult CoreFeatures(CalendarActionRequest actionRequest)
    {
      return View(GetPostResponse(actionRequest));
    }
  }
}
