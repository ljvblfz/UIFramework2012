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
  internal class TabStripTabsDesigner : ControlDesigner
  {
    private DesignerVerbCollection _verbs;

    public override DesignerVerbCollection Verbs
    {
      get
      {
        if(_verbs == null)
        {
          _verbs = new DesignerVerbCollection(new DesignerVerb [] {new DesignerVerb("Build TabStrip...", new EventHandler(this.OnBuildTabStrip))});
        }
       
        return _verbs;
      }
    }

    public override string GetDesignTimeHtml()
    {
      try
      {
        TabStrip oTabStrip = ((TabStrip)Component);

        if(oTabStrip.nodes == null || oTabStrip.Tabs.Count == 0)
        {
          return GetEmptyDesignTimeHtml();
        }

        // Create stringbuilder
        StringBuilder oSB = new StringBuilder();

        // Create new TabStrip
        TabStrip oTabStripCopy = new TabStrip();

        // Copy TabStrip properties
        foreach (PropertyInfo tabStripProperty in oTabStrip.GetType().GetProperties())
        {
          if (tabStripProperty.CanWrite)
          {
            tabStripProperty.SetValue(oTabStripCopy, tabStripProperty.GetValue(oTabStrip, null), null);
          }
        }

        // Copy looks
        foreach(ItemLook oLook in oTabStrip.ItemLooks)
        {
          ItemLook oNewLook = new ItemLook();
          oNewLook.LookId = oLook.LookId;
          oLook.CopyTo(oNewLook);

          oTabStripCopy.ItemLooks.Add(oNewLook);
        }

        // Copy templates (references only)
        foreach(NavigationCustomTemplate oTemplate in oTabStrip.ServerTemplates)
        {
          oTabStripCopy.ServerTemplates.Add(oTemplate);
        }

        // Copy all data
        oTabStripCopy.LoadXml(oTabStrip.GetXml());
      
        StringWriter oStringWriter = new StringWriter(oSB);
        HtmlTextWriter oWriter = new HtmlTextWriter(oStringWriter);

        oTabStripCopy.ApplyLooks();
        oTabStripCopy.ConsiderDefaultStyles();
        oTabStripCopy.RenderDownLevelContent(oWriter);
        
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
      return CreatePlaceHolderDesignTimeHtml("Right-click and select Build TabStrip for a quick start.");
    }

    private void OnBuildTabStrip(object sender, EventArgs e)
    {
      TabStripTabsEditor oEditor = new TabStripTabsEditor();
      oEditor.EditComponent(Component);
    }
  }
}
