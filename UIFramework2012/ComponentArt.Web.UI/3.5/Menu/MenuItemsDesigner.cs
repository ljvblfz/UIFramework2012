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
  internal class MenuItemsDesigner : ControlDesigner
  {
    private DesignerVerbCollection _verbs;

    public override DesignerVerbCollection Verbs
    {
      get
      {
        if(_verbs == null)
        {
          _verbs = new DesignerVerbCollection(new DesignerVerb [] {new DesignerVerb("Build Menu...", new EventHandler(this.OnBuildMenu))});
        }
       
        return _verbs;
      }
    }

    public override string GetDesignTimeHtml()
    {
      try
      {
        Menu oMenu = ((Menu)Component);

        if(oMenu.nodes == null || oMenu.Items.Count == 0)
        {
          return GetEmptyDesignTimeHtml();
        }

        // Create stringbuilder
        StringBuilder oSB = new StringBuilder();

        // Create new Menu
        Menu oMenuCopy = new Menu();

        // Copy menu properties
        foreach (PropertyInfo menuProperty in oMenu.GetType().GetProperties())
        {
          if (menuProperty.CanWrite)
          {
            menuProperty.SetValue(oMenuCopy, menuProperty.GetValue(oMenu, null), null);
          }
        }

        // Copy looks
        foreach(ItemLook oLook in oMenu.ItemLooks)
        {
          ItemLook oNewLook = new ItemLook();
          oNewLook.LookId = oLook.LookId;
          oLook.CopyTo(oNewLook);

          oMenuCopy.ItemLooks.Add(oNewLook);
        }

        // Copy templates (references only)
        foreach(NavigationCustomTemplate oTemplate in oMenu.ServerTemplates)
        {
          oMenuCopy.ServerTemplates.Add(oTemplate);
        }

        // Copy all data
        oMenuCopy.LoadXml(oMenu.GetXml());
      
        StringWriter oStringWriter = new StringWriter(oSB);
        HtmlTextWriter oWriter = new HtmlTextWriter(oStringWriter);

        oMenuCopy.ApplyLooks();
        oMenuCopy.ConsiderDefaultStyles();
        oMenuCopy.RenderDownLevelContent(oWriter);
        
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
      return CreatePlaceHolderDesignTimeHtml("Right-click and select Build Menu for a quick start.");
    }

    private void OnBuildMenu(object sender, EventArgs e)
    {
      MenuItemsEditor oEditor = new MenuItemsEditor();
      oEditor.EditComponent(Component);
    }
  }
}
