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
  internal class CallBackDesigner : ControlDesigner
  {
    public override string GetDesignTimeHtml()
    {
      try
      {
        CallBack oCallBack = ((CallBack)Component);

        if(oCallBack.Controls.Count == 0 || oCallBack.Content == null || oCallBack.Content.Controls.Count == 0)
        {
          return GetEmptyDesignTimeHtml();
        }

        StringBuilder oSB = new StringBuilder();
        StringWriter oStringWriter = new StringWriter(oSB);
        HtmlTextWriter oWriter = new HtmlTextWriter(oStringWriter);

        oCallBack.RenderDesignTime(oWriter);
        
        oWriter.Flush();
        oStringWriter.Flush();
        
        return oSB.ToString();
      }
      catch(Exception ex)
      {
        return CreatePlaceHolderDesignTimeHtml("Error generating design-time HTML:\n\n" + ex.ToString());
      }
    }

    [Obsolete()]
    public override string GetPersistInnerHtml()
    {
      // don't touch inner stuff
      return null;
    }

    protected override string GetEmptyDesignTimeHtml() 
    {
      return CreatePlaceHolderDesignTimeHtml("To add content to this CallBack, please switch to HTML view.");
    }
  }
}
