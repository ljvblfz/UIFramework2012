using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using System.Data;
using ComponentArt.Web.UI;

namespace ComponentArt.Mvc.Demos.Controllers
{
  public class ClientSideAPIController : Controller
  {
    public ActionResult Creation()
    {
      return View();
    }

    public ActionResult FullApp()
    {
      return View();
    }
  }
}