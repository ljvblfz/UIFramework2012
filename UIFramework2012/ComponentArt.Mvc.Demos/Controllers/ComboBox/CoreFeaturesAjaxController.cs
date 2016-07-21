using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ComponentArt.Web.UI;

namespace ComponentArt.Mvc.Demos.Controllers
{
  public partial class ComboBoxController : Controller
  {
    [AcceptVerbs(HttpVerbs.Get)]
    public ActionResult CoreFeaturesAjax()
    {
      // pre-populate with 40 items
      ComboBoxActionRequest actionRequest = new ComboBoxActionRequest();
      actionRequest.Take = 40;

      return View(GetCountryItems(actionRequest));
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public JsonResult CoreFeaturesAjax(ComboBoxActionRequest actionRequest)
    {
      return Json(GetCountryItems(actionRequest));
    }
  }
}
