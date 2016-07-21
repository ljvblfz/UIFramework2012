using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComponentArt.Mvc.Demos.Models;

namespace ComponentArt.Mvc.Demos.Controllers
{
  [HandleError]
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }
    
    
    private bool OnSite()
    {
      /*
      try
      {
        
        ControlCon

        if (context.Request.Url.Host.IndexOf("aspnetajax.componentart.com") >= 0
            || context.Request.Url.Host.IndexOf("aspnetajax-stage.componentart.com") >= 0
            || context.Request.Url.Host.IndexOf("www.componentart.com") >= 0)
          return true;
        else
          return false;
      }
      catch (Exception ex)
      {
        return true;
      }
      */
      return false;
    }
  }
}
