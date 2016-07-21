using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace ComponentArt.Mvc.Demos.Controllers
{
  public partial class TreeViewController : Controller
  {
    public ActionResult FileBrowser()
    {
      return View();
    }

    public ActionResult XmlFromFileSystem()
    {
      return View();
    }
  }
}
