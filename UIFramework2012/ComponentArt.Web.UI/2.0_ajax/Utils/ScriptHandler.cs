using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.IO.Compression;

namespace ComponentArt.Web.UI
{
  internal class ScriptInfo
  {
    public static NameValueCollection ScriptIds;
    public static Hashtable ScriptDependencies;

    static ScriptInfo()
    {
      // define ids for embedded scripts
      ScriptIds = new NameValueCollection();
      ScriptIds.Add("u1", "ComponentArt.Web.UI.client_scripts.A573G988.js");
      ScriptIds.Add("u2", "ComponentArt.Web.UI.client_scripts.A573P291.js");
      ScriptIds.Add("u3", "ComponentArt.Web.UI.client_scripts.A573Z388.js");
      ScriptIds.Add("u4", "ComponentArt.Web.UI.client_scripts.A573S188.js");
      ScriptIds.Add("u5", "ComponentArt.Web.UI.client_scripts.A573T069.js");
      ScriptIds.Add("u6", "ComponentArt.Web.UI.client_scripts.A573P290.js");
      ScriptIds.Add("c1", "ComponentArt.Web.UI.Calendar.client_scripts.A573Q148.js");
      ScriptIds.Add("c2", "ComponentArt.Web.UI.Calendar.client_scripts.A573W128.js");
      ScriptIds.Add("cb1", "ComponentArt.Web.UI.Callback.client_scripts.A573P191.js");
      ScriptIds.Add("cmb1", "ComponentArt.Web.UI.ComboBox.client_scripts.A573P123.js");
      ScriptIds.Add("cmb2", "ComponentArt.Web.UI.ComboBox.client_scripts.A573P456.js");
      ScriptIds.Add("cmb3", "ComponentArt.Web.UI.ComboBox.client_scripts.A573P124.js");
      ScriptIds.Add("cp1", "ComponentArt.Web.UI.ColorPicker.client_scripts.A573C779.js");
      ScriptIds.Add("d1", "ComponentArt.Web.UI.Dialog.client_scripts.A573G999.js");
      ScriptIds.Add("d2", "ComponentArt.Web.UI.Dialog.client_scripts.A573G130.js");
      ScriptIds.Add("e1", "ComponentArt.Web.UI.Editor.client_scripts.A572GI44.js");
      ScriptIds.Add("e2", "ComponentArt.Web.UI.Editor.client_scripts.A572GI43.js");
      ScriptIds.Add("e3", "ComponentArt.Web.UI.Editor.client_scripts.A572GI45.js");
      ScriptIds.Add("e4", "ComponentArt.Web.UI.Editor.client_scripts.A572GI46.js");
      ScriptIds.Add("g1", "ComponentArt.Web.UI.Grid.client_scripts.A573R178.js");
      ScriptIds.Add("g2", "ComponentArt.Web.UI.Grid.client_scripts.A573J198.js");
      ScriptIds.Add("g3", "ComponentArt.Web.UI.Grid.client_scripts.A573L238.js");
      ScriptIds.Add("g4", "ComponentArt.Web.UI.Grid.client_scripts.A573G188.js");
      ScriptIds.Add("g5", "ComponentArt.Web.UI.Grid.client_scripts.A573R378.js");
      ScriptIds.Add("ni1", "ComponentArt.Web.UI.Input.client_scripts.A579I433.js");
      ScriptIds.Add("ni2", "ComponentArt.Web.UI.Input.client_scripts.A579I432.js");
      ScriptIds.Add("mi1", "ComponentArt.Web.UI.Input.client_scripts.A570I433.js");
      ScriptIds.Add("mi2", "ComponentArt.Web.UI.Input.client_scripts.A570I431.js");
      ScriptIds.Add("mi3", "ComponentArt.Web.UI.Input.client_scripts.A570I432.js");
      ScriptIds.Add("m1", "ComponentArt.Web.UI.Menu.client_scripts.A573W888.js");
      ScriptIds.Add("m2", "ComponentArt.Web.UI.Menu.client_scripts.A573R388.js");
      ScriptIds.Add("m3", "ComponentArt.Web.UI.Menu.client_scripts.A573Q288.js");
      ScriptIds.Add("mp1", "ComponentArt.Web.UI.MultiPage.client_scripts.A573A488.js");
      ScriptIds.Add("n1", "ComponentArt.Web.UI.NavBar.client_scripts.A573E888.js");
      ScriptIds.Add("n2", "ComponentArt.Web.UI.NavBar.client_scripts.A573M488.js");
      ScriptIds.Add("n3", "ComponentArt.Web.UI.NavBar.client_scripts.A573D588.js");
      ScriptIds.Add("r1", "ComponentArt.Web.UI.Rotator.client_scripts.A573Z288.js");
      ScriptIds.Add("r2", "ComponentArt.Web.UI.Rotator.client_scripts.A573G788.js");
      ScriptIds.Add("sch1", "ComponentArt.Web.UI.Scheduler.client_scripts.A577AB33.js");
      ScriptIds.Add("sch2", "ComponentArt.Web.UI.Scheduler.client_scripts.A577AB34.js");
      ScriptIds.Add("sch3", "ComponentArt.Web.UI.Scheduler.client_scripts.A577AB36.js");
      ScriptIds.Add("s1", "ComponentArt.Web.UI.Snap.client_scripts.A573U699.js");
      ScriptIds.Add("s2", "ComponentArt.Web.UI.Snap.client_scripts.A573P288.js");
      ScriptIds.Add("s3", "ComponentArt.Web.UI.Snap.client_scripts.A573J988.js");
      ScriptIds.Add("s4", "ComponentArt.Web.UI.Snap.client_scripts.A573V588.js");
      ScriptIds.Add("s5", "ComponentArt.Web.UI.Snap.client_scripts.A573X288.js");
      ScriptIds.Add("s6", "ComponentArt.Web.UI.Snap.client_scripts.A573K688.js");
      ScriptIds.Add("s7", "ComponentArt.Web.UI.Snap.client_scripts.A573W988.js");
      ScriptIds.Add("sl1", "ComponentArt.Web.UI.Slider.client_scripts.A573HR343.js");
      ScriptIds.Add("sp1", "ComponentArt.Web.UI.Splitter.client_scripts.A573J482.js");
      ScriptIds.Add("sc1", "ComponentArt.Web.UI.SpellCheck.client_scripts.A573O912.js");
      ScriptIds.Add("ts1", "ComponentArt.Web.UI.TabStrip.client_scripts.A573C488.js");
      ScriptIds.Add("ts2", "ComponentArt.Web.UI.TabStrip.client_scripts.A573B188.js");
      ScriptIds.Add("ts3", "ComponentArt.Web.UI.TabStrip.client_scripts.A573I688.js");
      ScriptIds.Add("tb1", "ComponentArt.Web.UI.ToolBar.client_scripts.A573B288.js");
      ScriptIds.Add("tb2", "ComponentArt.Web.UI.ToolBar.client_scripts.A573H988.js");
      ScriptIds.Add("tb3", "ComponentArt.Web.UI.ToolBar.client_scripts.A573I788.js");
      ScriptIds.Add("tv1", "ComponentArt.Web.UI.TreeView.client_scripts.A573S388.js");
      ScriptIds.Add("tv2", "ComponentArt.Web.UI.TreeView.client_scripts.A573R288.js");
      ScriptIds.Add("tv3", "ComponentArt.Web.UI.TreeView.client_scripts.A573O788.js");
      ScriptIds.Add("up1", "ComponentArt.Web.UI.Upload.client_scripts.A573P101.js");
      
      // define control->script dependencies
      ScriptDependencies = new Hashtable();
      ScriptDependencies.Add("Calendar", new string[] { "u1", "u6", "u5", "c1", "c2" });
      ScriptDependencies.Add("CallBack", new string[] { "u1", "cb1" });
      ScriptDependencies.Add("ColorPicker", new string[] { "u1", "cp1", "sl1" });
      ScriptDependencies.Add("ComboBox", new string[] { "u1", "u3", "cmb1", "cmb2", "cmb3" });
      ScriptDependencies.Add("Dialog", new string[] { "u1", "d1", "d2" });
      ScriptDependencies.Add("Editor", new string[] { "u1", "u3", "e1", "e2", "e3", "e4" });
      ScriptDependencies.Add("Grid", new string[] { "u1", "u2", "u3", "g1", "g2", "g3", "g4", "g5" });
      ScriptDependencies.Add("DataGrid", new string[] { "u1", "u2", "u3", "g1", "g2", "g3", "g4", "g5" });
      ScriptDependencies.Add("NumberInput", new string[] { "u1", "ni1", "ni2" });
      ScriptDependencies.Add("MaskedInput", new string[] { "u1", "mi1", "mi2", "mi3" });
      ScriptDependencies.Add("Menu", new string[] { "u1", "u3", "u4", "m1", "m2", "m3" });
      ScriptDependencies.Add("MultiPage", new string[] { "u1", "mp1" });
      ScriptDependencies.Add("NavBar", new string[] { "u1", "u3", "u4", "n1", "n2", "n3" });
      ScriptDependencies.Add("Rotator", new string[] { "u1", "u3", "r1", "r2" });
      ScriptDependencies.Add("Scheduler", new string[] { "u1", "u6", "sch1", "sch2" });
      ScriptDependencies.Add("SchedulerDaysView", new string[] { "u1", "u6", "sch3" });
      ScriptDependencies.Add("Slider", new string[] { "u1", "sl1" });
      ScriptDependencies.Add("Snap", new string[] { "u1", "s1", "s2", "s3", "s4", "s5", "s6", "s7" });
      ScriptDependencies.Add("Splitter", new string[] { "u1", "sp1" });
      ScriptDependencies.Add("SpellCheck", new string[] { "u1", "sc1" });
      ScriptDependencies.Add("TabStrip", new string[] { "u1", "u3", "u4", "ts1", "ts2", "ts3" });
      ScriptDependencies.Add("Ticker", new string[] { "u1", "u3", "r1", "r2" });
      ScriptDependencies.Add("ToolBar", new string[] { "u1", "u3", "u4", "tb1", "tb2", "tb3" });
      ScriptDependencies.Add("TreeView", new string[] { "u1", "u2", "u3", "tv1", "tv2", "tv3" });
      ScriptDependencies.Add("Upload", new string[] { "u1", "up1" });
    }
  }

  internal class ScriptGenerator
  {
    private static void LoadScriptReferences(ArrayList arList, string sControlName, bool bWithoutUtils)
    {
      string [] arDependencies = (string [])ScriptInfo.ScriptDependencies[sControlName];

      if (arDependencies != null)
      {
        foreach (string sScriptId in arDependencies)
        {
          if (!arList.Contains(sScriptId) && (!bWithoutUtils || !IsUtilScriptId(sScriptId)))
          {
            arList.Add(sScriptId);
          }
        }
      }
    }

    internal static string GetScriptReference(string sControlName)
    {
      ArrayList arList = new ArrayList();

      LoadScriptReferences(arList, sControlName, false);

      return string.Join(",", (string[])arList.ToArray(typeof(string)));
    }

    internal static string GetScriptReference(string sControlName, bool bWithoutUtils)
    {
      ArrayList arList = new ArrayList();

      LoadScriptReferences(arList, sControlName, bWithoutUtils);

      return string.Join(",", (string[])arList.ToArray(typeof(string)));
    }

    internal static string GetScriptReference(string [] arControlNames)
    {
      ArrayList arList = new ArrayList();

      foreach (string sControlName in arControlNames)
      {
        LoadScriptReferences(arList, sControlName.Trim(), false);
      }

      return string.Join(",", (string[])arList.ToArray(typeof(string)));
    }

    internal static string GetUtilsScriptReference()
    {
      ArrayList arList = new ArrayList();
      
      foreach(string sScriptId in ScriptInfo.ScriptIds.AllKeys)
      {
        if (IsUtilScriptId(sScriptId))
        {
          arList.Add(sScriptId);
        }
      }

      return string.Join(",", (string [])arList.ToArray(typeof(string)));
    }

    internal static string GetFullScriptReference()
    {
      string[] arKeys = new string[ScriptInfo.ScriptIds.Keys.Count];

      return string.Join(",", ScriptInfo.ScriptIds.AllKeys);
    }

    internal static bool IsScriptControl(string sControlName)
    {
      return (ScriptInfo.ScriptDependencies[sControlName] != null);
    }

    internal static bool IsUtilScriptId(string sScriptId)
    {
      return (sScriptId[0] == 'u' && Char.IsDigit(sScriptId[1]));
    }
  }

  public class ScriptHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
  {
    // Override the ProcessRequest method.
    public void ProcessRequest(HttpContext context)
    {
      // does the client have this cached?
      if (context.Request.Headers.Get("If-Modified-Since") != null)
      {
        // don't return anything but the code
        context.Response.StatusCode = 304;
        return;
      }

      StringBuilder oScriptContent = new StringBuilder();

      string sScriptId = context.Request.Params["f"];

      string[] arFiles = sScriptId.Split(',');

      foreach (string sFile in arFiles)
      {
        oScriptContent.Append(Utils.GetResourceContent((string)ScriptInfo.ScriptIds[sFile]));
        
        // make sure there's a newline after each file
        oScriptContent.Append("\n");
      }

      context.Response.Cache.SetLastModified(DateTime.Now.AddYears(-1));

      ApplyCompression(context);

      context.Response.ContentType = "text/javascript";
      context.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
      context.Response.Cache.SetCacheability(HttpCacheability.Public);
      context.Response.Write(oScriptContent.ToString());
    }

    private void ApplyCompression(HttpContext context)
    {
      // Get accepted encodings
      string oSupportedEncodings = context.Request.Headers.Get("Accept-Encoding");
      oSupportedEncodings = oSupportedEncodings == null ? "" : oSupportedEncodings.ToLower();

      // Get current response stream
      Stream baseStream = context.Response.Filter;

      // Apply compression if it's supported
      if (oSupportedEncodings.Contains("gzip"))
      {
        context.Response.Filter = new GZipStream(baseStream, CompressionMode.Compress);
        context.Response.AppendHeader("Content-Encoding", "gzip");
      }
      else if (oSupportedEncodings.Contains("deflate"))
      {
        context.Response.Filter = new DeflateStream(baseStream, CompressionMode.Compress);
        context.Response.AppendHeader("Content-Encoding", "deflate");
      }
    }

    // Override the IsReusable property.
    public bool IsReusable
    {
      get { return true; }
    }
  }
}
