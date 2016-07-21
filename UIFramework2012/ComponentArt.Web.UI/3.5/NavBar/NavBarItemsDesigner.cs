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
  internal class NavBarItemsDesigner : ControlDesigner
  {
    private DesignerVerbCollection _verbs;

    public override DesignerVerbCollection Verbs
    {
      get
      {
        if(_verbs == null)
        {
          _verbs = new DesignerVerbCollection(new DesignerVerb [] {new DesignerVerb("Build NavBar...", new EventHandler(this.OnBuildNavBar))});
        }
       
        return _verbs;
      }
    }

    public override string GetDesignTimeHtml()
    {
      try
      {
        NavBar oNavBar = ((NavBar)Component);

        if(oNavBar.nodes == null || oNavBar.Items.Count == 0)
        {
          return GetEmptyDesignTimeHtml();
        }

        // Create stringbuilder
        StringBuilder oSB = new StringBuilder();

        // Create new NavBar
        NavBar oNavBarCopy = new NavBar();

        // Copy navbar properties
        foreach (PropertyInfo navbarProperty in oNavBar.GetType().GetProperties())
        {
          if (navbarProperty.CanWrite)
          {
            navbarProperty.SetValue(oNavBarCopy, navbarProperty.GetValue(oNavBar, null), null);
          }
        }

        // Copy looks
        foreach(ItemLook oLook in oNavBar.ItemLooks)
        {
          ItemLook oNewLook = new ItemLook();
          oNewLook.LookId = oLook.LookId;
          oLook.CopyTo(oNewLook);

          oNavBarCopy.ItemLooks.Add(oNewLook);
        }

        // Copy templates (references only)
        foreach(NavigationCustomTemplate oTemplate in oNavBar.ServerTemplates)
        {
          oNavBarCopy.ServerTemplates.Add(oTemplate);
        }

        // Copy all data
        oNavBarCopy.LoadXml(oNavBar.GetXml());
      
        StringWriter oStringWriter = new StringWriter(oSB);
        HtmlTextWriter oWriter = new HtmlTextWriter(oStringWriter);

        oNavBarCopy.ApplyLooks();
        oNavBarCopy.ConsiderDefaultStyles();
        oNavBarCopy.RenderDownLevelContent(oWriter);
        
        oWriter.Flush();
        oStringWriter.Flush();
        
        return oSB.ToString();//.Replace("<", "&lt;").Replace(">", "&gt;");
      }
      catch(Exception ex)
      {
        return CreatePlaceHolderDesignTimeHtml("Error generating design-time HTML:\n\n" + ex.ToString());
      }
    }

    protected override string GetEmptyDesignTimeHtml() 
    {
      return CreatePlaceHolderDesignTimeHtml("Right-click and select Build NavBar for a quick start.");
    }

    private void OnBuildNavBar(object sender, EventArgs e)
    {
      NavBarItemsEditor oEditor = new NavBarItemsEditor();
      oEditor.EditComponent(Component);
    }
  }
}
