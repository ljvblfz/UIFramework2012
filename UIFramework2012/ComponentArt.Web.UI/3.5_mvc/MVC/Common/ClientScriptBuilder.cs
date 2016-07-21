using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Collections;

namespace ComponentArt.Web.UI
{
  public class ClientScriptBuilder
  {
    private List<string> clientScripts;
    private List<string> renderedClientScripts;

    public ClientScriptBuilder(ref List<string> clientScripts, ref List<string> renderedClientScripts)
    {
      this.clientScripts = clientScripts;
      this.renderedClientScripts = renderedClientScripts;
    }

    public string GetVersion()
    {
      Version version = Assembly.GetExecutingAssembly().GetName().Version;
      return version.Major.ToString() + "_" + version.Minor.ToString() + "_" + version.Build.ToString() + "_" + version.Revision.ToString();      
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();

      string sPath = "";

      if (HttpContext.Current != null)
      {
        sPath = HttpContext.Current.Request.ApplicationPath;
        if (!sPath.EndsWith("/")) sPath += "/";
      }

      if (clientScripts.Count > 0)
      {
        sb.Append("<script src=\"" + sPath + "ComponentArtScript.axd?f=");
        foreach (string script in clientScripts)
        {
          sb.Append(script + ",");
          renderedClientScripts.Add(script);
        }
        clientScripts.Clear();
        sb.Remove(sb.Length - 1, 1);
        sb.Append("&v=" + GetVersion());
        sb.Append("\" type=\"text/javascript\"></script>");

        clientScripts.Clear();
      }
      return sb.ToString();
    }

  }
}
