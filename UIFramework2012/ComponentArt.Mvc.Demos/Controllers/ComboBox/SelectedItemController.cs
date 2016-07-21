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
    public ActionResult SelectedItem()
    {
      ViewData["SelectedIndex1"] = -1;
      ViewData["SelectedIndex2"] = -1;
      ViewData["SelectedIndex3"] = -1;

      return View(GetCountryItems());
    }

    // handle posts
    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult SelectedItem(FormCollection frmCollection)
    {
      ViewData["SelectedIndex1"] = ComboBoxHelperExtensions.GetSelectedIndex("ComboBox1", frmCollection);
      ViewData["SelectedIndex2"] = ComboBoxHelperExtensions.GetSelectedIndex("ComboBox2", frmCollection);
      ViewData["SelectedIndex3"] = ComboBoxHelperExtensions.GetSelectedIndex("ComboBox3", frmCollection);

      ViewData["SelectedItem1"] = ComboBoxHelperExtensions.GetSelectedItem("ComboBox1", frmCollection);
      ViewData["SelectedItem2"] = ComboBoxHelperExtensions.GetSelectedItem("ComboBox2", frmCollection);
      ViewData["SelectedItem3"] = ComboBoxHelperExtensions.GetSelectedItem("ComboBox3", frmCollection);

      return View(GetCountryItems());
    }
  }
}
