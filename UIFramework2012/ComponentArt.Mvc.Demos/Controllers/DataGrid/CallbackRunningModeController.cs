using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ComponentArt.Web.UI;

namespace ComponentArt.Mvc.Demos.Controllers
{
  public partial class DataGridController : Controller
  {
    [AcceptVerbs(HttpVerbs.Get)]
    public ActionResult CallbackRunningMode()
    {
      return View(GetFeedItems());
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult CallbackRunningMode(DataGridActionRequest actionRequest)
    {
      return Json(GetFeedItems(actionRequest));
    }
  }
}
