using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace ComponentArt.Mvc.Demos.Controllers
{
    public class TabStripController : Controller
    {
        public ActionResult CoreFeatures(string page)
        {
          ViewData["TabStripPage"] = page ?? "";
          return View();
        }

        public ActionResult ThemeSwitching()
        {
          return View();
        }
    }
}
