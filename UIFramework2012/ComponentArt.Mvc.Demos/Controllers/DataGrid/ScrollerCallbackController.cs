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
    public ActionResult ScrollerCallback()
    {
      return View(GetPostItems());
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult ScrollerCallback(DataGridActionRequest actionRequest)
    {
      return Json(GetPostItems(actionRequest));
    }    
  }
}
