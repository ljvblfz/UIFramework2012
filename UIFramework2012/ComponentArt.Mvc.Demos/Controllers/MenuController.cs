using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace ComponentArt.Mvc.Demos.Controllers
{
    public class MenuController : Controller
    {

      #region Handle the simple demos
        public ActionResult CoreFeatures()
        {
            return View();
        }

        public ActionResult ThemedWithCssIcons()
        {
          return View();
        }

        public ActionResult ThemedWithIcons()
        {
          return View();
        }

        public ActionResult ThemeSwitching()
        {
          return View();
        }
      #endregion
    }
}
