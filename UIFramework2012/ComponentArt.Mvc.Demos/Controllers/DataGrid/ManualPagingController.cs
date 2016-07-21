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
    public ActionResult ManualPaging(DataGridActionRequest actionRequest)
    {
      return View(GetFeedItems(actionRequest));
    }
  }
}
