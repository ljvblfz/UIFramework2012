using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;

namespace ComponentArt.Web.UI
{
  public class ClientScriptFactory
  {
    private List<string> clientScripts = new List<string>();
    private List<string> renderedClientScripts = new List<string>();
    private Hashtable scriptDependencies = new Hashtable();

    public ClientScriptFactory(ref List<string> clientScripts, ref List<string> renderedClientScripts)
    {
      this.clientScripts = clientScripts;
      this.renderedClientScripts = renderedClientScripts;

      // Taken from Utils/ScriptHandler.cs:ScriptInfo
      // Note that this might be more efficient if control-specific scripts are grouped here
      scriptDependencies.Add("Calendar", new string[] { "u1", "u6", "u5", "c1", "c2" });
      scriptDependencies.Add("CallBack", new string[] { "u1", "cb1" });
      scriptDependencies.Add("ColorPicker", new string[] { "u1", "cp1", "sl1" });
      scriptDependencies.Add("ComboBox", new string[] { "u1", "u3", "cmb1", "cmb2", "cmb3" });
      scriptDependencies.Add("Dialog", new string[] { "u1", "d1", "d2" });
      scriptDependencies.Add("Editor", new string[] { "u1", "u3", "e1", "e2", "e3", "e4" });
      scriptDependencies.Add("Grid", new string[] { "u1", "u2", "u3", "g1", "g2", "g3", "g4", "g5" });
      scriptDependencies.Add("DataGrid", new string[] { "u1", "u2", "u3", "g1", "g2", "g3", "g4", "g5" });
      scriptDependencies.Add("NumberInput", new string[] { "u1", "ni1", "ni2" });
      scriptDependencies.Add("MaskedInput", new string[] { "u1", "mi1", "mi2", "mi3" });
      scriptDependencies.Add("Menu", new string[] { "u1", "u3", "u4", "m1", "m2", "m3" });
      scriptDependencies.Add("MultiPage", new string[] { "u1", "mp1" });
      scriptDependencies.Add("NavBar", new string[] { "u1", "u3", "u4", "n1", "n2", "n3" });
      scriptDependencies.Add("Rotator", new string[] { "u1", "u3", "r1", "r2" });
      scriptDependencies.Add("Scheduler", new string[] { "u1", "u6", "sch1", "sch2" });
      scriptDependencies.Add("SchedulerDaysView", new string[] { "u1", "u6", "sch3" });
      scriptDependencies.Add("Slider", new string[] { "u1", "sl1" });
      scriptDependencies.Add("Snap", new string[] { "u1", "s1", "s2", "s3", "s4", "s5", "s6", "s7" });
      scriptDependencies.Add("Splitter", new string[] { "u1", "sp1" });
      scriptDependencies.Add("SpellCheck", new string[] { "u1", "sc1" });
      scriptDependencies.Add("TabStrip", new string[] { "u1", "u3", "u4", "ts1", "ts2", "ts3" });
      scriptDependencies.Add("Ticker", new string[] { "u1", "u3", "r1", "r2" });
      scriptDependencies.Add("ToolBar", new string[] { "u1", "u3", "u4", "tb1", "tb2", "tb3" });
      scriptDependencies.Add("TreeView", new string[] { "u1", "u2", "u3", "tv1", "tv2", "tv3" });
      scriptDependencies.Add("Upload", new string[] { "u1", "up1" });
    }
    public virtual ClientScriptBuilder ToolBar()
    {
        AddControl("ToolBar");
        return new ClientScriptBuilder(ref clientScripts, ref renderedClientScripts);
    }

    public virtual ClientScriptBuilder Calendar()
    {
      AddControl("Calendar");
      return new ClientScriptBuilder(ref clientScripts, ref renderedClientScripts);
    }

    public virtual ClientScriptBuilder ComboBox()
    {
      AddControl("ComboBox");
      return new ClientScriptBuilder(ref clientScripts, ref renderedClientScripts);
    }

    public virtual ClientScriptBuilder DataGrid()
    {
      AddControl("Grid");
      return new ClientScriptBuilder(ref clientScripts, ref renderedClientScripts);
    }

    public virtual ClientScriptBuilder TreeView()
    {
        AddControl("TreeView");
        return new ClientScriptBuilder(ref clientScripts, ref renderedClientScripts);
    }

    public virtual ClientScriptBuilder TabStrip()
    {
        AddControl("TabStrip");
        return new ClientScriptBuilder(ref clientScripts, ref renderedClientScripts);
    }

    public virtual ClientScriptBuilder NavBar()
    {
      AddControl("NavBar");
      return new ClientScriptBuilder(ref clientScripts, ref renderedClientScripts);
    }

    public virtual ClientScriptBuilder Menu()
    {
      AddControl("Menu");
      return new ClientScriptBuilder(ref clientScripts, ref renderedClientScripts);
    }

    private void AddControl(string controlName)
    {
      string[] scripts = (string[])scriptDependencies[controlName];
      foreach (string script in scripts)
      {
        if (!clientScripts.Contains(script) && !renderedClientScripts.Contains(script))
        {
          clientScripts.Add(script);
        }
      }
    }
  }
}
