using System; 
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel; 
using System.ComponentModel.Design; 
using System.Diagnostics; 
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI; 
using System.Web.UI.Design; 
using System.Web.UI.WebControls; 
using ComponentArt.Web.UI; 

namespace ComponentArt.Web.UI
{
  internal class CalendarDesigner : ControlDesigner
  {
    public override string GetDesignTimeHtml()
    {
      try
      {
        Calendar calendar = (Calendar)this.Component;
        Calendar calendarCopy = new Calendar(); // Create new Calendar
        foreach (PropertyInfo calendarProperty in calendar.GetType().GetProperties()) // Copy calendar properties
        {
          if (calendarProperty.CanWrite && calendarProperty.CanRead)
          {
            calendarProperty.SetValue(calendarCopy, calendarProperty.GetValue(calendar, null), null);
          }
        }
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        calendarCopy.RenderDownLevelContent(htw);
        htw.Flush();
        sw.Flush();
        return sb.ToString();
      }
      catch(Exception e)
      {
        return this.GetErrorDesignTimeHtml(e);
      }
    }

    protected override string GetErrorDesignTimeHtml(Exception e)
    {
      string msg = "<font color=red><b>Error: </b>"; 
      msg += e.Message + "</font>"; 
      return this.CreatePlaceHolderDesignTimeHtml(msg);  
    }
  }
}