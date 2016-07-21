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
  internal class ToolBarItemsDesigner : ControlDesigner
  {
    private DesignerVerbCollection _verbs;

    public override DesignerVerbCollection Verbs
    {
      get
      {
        if(_verbs == null)
        {
          _verbs = new DesignerVerbCollection(new DesignerVerb [] {new DesignerVerb("Build ToolBar...", new EventHandler(this.OnBuildToolBar))});
        }
       
        return _verbs;
      }
    }

    public override string GetDesignTimeHtml()
    {
      try
      {
        ToolBar oToolBar = ((ToolBar)Component);

        if (oToolBar.Items == null || oToolBar.Items.Count == 0)
        {
          return GetEmptyDesignTimeHtml();
        }

        // Create stringbuilder
        StringBuilder oSB = new StringBuilder();

        // Create new ToolBar
        ToolBar oToolBarCopy = new ToolBar();

        // Copy ToolBar properties
        foreach (PropertyInfo toolBarProperty in oToolBar.GetType().GetProperties())
        {
          if (toolBarProperty.CanWrite)
          {
            toolBarProperty.SetValue(oToolBarCopy, toolBarProperty.GetValue(oToolBar, null), null);
          }
        }

        // Copy templates (references only)
        foreach(ToolBarCustomTemplate oTemplate in oToolBar.ServerTemplates)
        {
          oToolBarCopy.ServerTemplates.Add(oTemplate);
        }

        // Copy custom content
        foreach (ToolBarItemContent oContent in oToolBar.Controls)
        {
          oToolBarCopy.Controls.Add(oContent);
        }

        // Copy all data
        oToolBarCopy.LoadXml(oToolBar.GetXml());
      
        StringWriter oStringWriter = new StringWriter(oSB);
        HtmlTextWriter oWriter = new HtmlTextWriter(oStringWriter);

        oToolBarCopy.RenderDefaultStyles(oWriter);
        oToolBarCopy.RenderDownLevelToolBar(oWriter);
        
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
      return CreatePlaceHolderDesignTimeHtml("Right-click and select Build ToolBar for a quick start.");
    }

    private void OnBuildToolBar(object sender, EventArgs e)
    {
      ToolBarItemsEditor oEditor = new ToolBarItemsEditor();
      oEditor.EditComponent(Component);
    }
  }
}
