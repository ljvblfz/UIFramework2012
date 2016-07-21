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
        public ActionResult SelectedItem()
        {
          ViewData["FeedData"] = GetFeedItems(350);

          return View();
        }

        // handle posts
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SelectedItem(FormCollection frmCollection)
        {
          ViewData["SelectedItems"] = DataGridHelperExtensions.GetSelectedItems("Grid1", frmCollection);
          ViewData["FeedData"] = GetFeedItems(350);

          return View();
        }
    }
}
