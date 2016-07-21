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
  internal class UploadDesigner : ControlDesigner
  {
    public override string GetDesignTimeHtml()
    {
      try
      {
        Upload oUpload = ((Upload)Component);

        StringBuilder oSB = new StringBuilder();
        StringWriter oStringWriter = new StringWriter(oSB);
        HtmlTextWriter oWriter = new HtmlTextWriter(oStringWriter);

        oUpload.RenderDownLevelContent(oWriter);

        oWriter.Flush();
        oStringWriter.Flush();

        return oSB.ToString();
      }
      catch (Exception ex)
      {
        return CreatePlaceHolderDesignTimeHtml("Error generating design-time HTML:\n\n" + ex.ToString());
      }
    }
  }
}
