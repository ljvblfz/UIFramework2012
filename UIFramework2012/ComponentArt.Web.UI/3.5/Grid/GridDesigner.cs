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
  internal class GridDesigner : ControlDesigner
  {
    public override string GetDesignTimeHtml()
    {
      try
      {
        Grid oGrid = ((Grid)Component);

        if(oGrid.Controls.Count == 0)
        {
          return GetEmptyDesignTimeHtml();
        }

        StringBuilder oSB = new StringBuilder();
        StringWriter oStringWriter = new StringWriter(oSB);
        HtmlTextWriter oWriter = new HtmlTextWriter(oStringWriter);

        oGrid.RenderDesignTime(oWriter);
        
        oWriter.Flush();
        oStringWriter.Flush();
        
        return oSB.ToString();
      }
      catch(Exception ex)
      {
        return CreatePlaceHolderDesignTimeHtml("Error generating design-time HTML:\n\n" + ex.ToString());
      }
    }

    protected override string GetEmptyDesignTimeHtml() 
    {
      return CreatePlaceHolderDesignTimeHtml("To configure and style this Grid, please switch to HTML view.");
    }
  }
}
