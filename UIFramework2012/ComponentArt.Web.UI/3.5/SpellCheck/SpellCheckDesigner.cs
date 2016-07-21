using System;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;


namespace ComponentArt.Web.UI
{
  internal class SpellCheckDesigner : ControlDesigner
  {
    public override string GetDesignTimeHtml()
    {
      return GetEmptyDesignTimeHtml();
    }

    protected override string GetEmptyDesignTimeHtml()
    {
      return CreatePlaceHolderDesignTimeHtml("To configure and style this SpellCheck, please use the HTML view.");
    }
  }
}
