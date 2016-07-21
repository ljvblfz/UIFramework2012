using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using ComponentArt.Web.UI;


namespace ComponentArt.Mvc.Demos.Controllers
{
    public partial class TreeViewController : Controller
    {
        public ActionResult CoreFeatures()
        {
            return View();
        }

        // handle posts
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CoreFeatures(FormCollection frmCollection)
        {
          ViewData["SelectedNode"] = TreeViewHelperExtensions.GetSelectedText("TreeView1", frmCollection);

          return View();
        }

        public ActionResult DragDropFeatures()
        {
            return View();
        }

        public ActionResult ExpandSinglePath()
        {
            return View();
        }

        public ActionResult KeyboardControl()
        {
            return View();
        }

        public ActionResult NodeCellStyles()
        {
            return View();
        }

        public ActionResult ThemeSwitching()
        {
          return View();
        }
    }
}
