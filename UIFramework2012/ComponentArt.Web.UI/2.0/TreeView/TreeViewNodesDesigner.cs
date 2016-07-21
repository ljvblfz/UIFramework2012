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
  internal class TreeViewNodesDesigner : ControlDesigner
	{
    private DesignerVerbCollection _verbs;

    public override DesignerVerbCollection Verbs
    {
      get
      {
        if(_verbs == null)
        {
          _verbs = new DesignerVerbCollection(new DesignerVerb [] {new DesignerVerb("Build Tree...", new EventHandler(this.OnBuildTree))});
        }
       
        return _verbs;
      }
    }

    public override string GetDesignTimeHtml()
    {
      try
      {
        TreeView oTreeView = ((TreeView)Component);

        if(oTreeView.nodes == null || oTreeView.Nodes.Count == 0)
        {
          return GetEmptyDesignTimeHtml();
        }

        // Create stringbuilder
        StringBuilder oSB = new StringBuilder();

        // Create new TreeView
        TreeView oTreeViewCopy = new TreeView();

			  // Copy treeview properties
        foreach (PropertyInfo treeviewProperty in oTreeView.GetType().GetProperties())
        {
          if (treeviewProperty.CanWrite)
          {
            treeviewProperty.SetValue(oTreeViewCopy, treeviewProperty.GetValue(oTreeView, null), null);
          }
        }

        // Copy templates (references only)
        foreach(NavigationCustomTemplate oTemplate in oTreeView.Templates)
        {
          oTreeViewCopy.Templates.Add(oTemplate);
        }

        // Copy all data
        oTreeViewCopy.LoadXml(oTreeView.GetXml());
      
        StringWriter oStringWriter = new StringWriter(oSB);
        HtmlTextWriter oWriter = new HtmlTextWriter(oStringWriter);

        oTreeViewCopy.ConsiderDefaultStyles();
        oTreeViewCopy.RenderDownLevelContent(oWriter);
        
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
      return CreatePlaceHolderDesignTimeHtml("Right-click and select Build Tree for a quick start.");
    }

    private void OnBuildTree(object sender, EventArgs e)
    {
      TreeViewNodesEditor oEditor = new TreeViewNodesEditor();
      oEditor.EditComponent(Component);
    }
	}
}
