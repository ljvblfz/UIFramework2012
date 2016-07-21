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
    internal class SliderDesigner : ControlDesigner 
    {
        public override string GetDesignTimeHtml()
        {
            try
            {
                Slider mySlider = ((Slider)Component);

                StringBuilder oSB = new StringBuilder();
                StringWriter oStringWriter = new StringWriter(oSB);
                HtmlTextWriter output = new HtmlTextWriter(oStringWriter);

                mySlider.RenderControl(output);

                output.Flush();
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
