using System;
using System.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ComponentArt.Web.UI
{
  internal class TabStripTabsEditor : WindowsFormsComponentEditor
  {
    public override bool EditComponent(ITypeDescriptorContext context, object component, IWin32Window owner) 
    {
      ComponentArt.Web.UI.TabStrip oControl = (ComponentArt.Web.UI.TabStrip)component;
      IServiceProvider site = oControl.Site;
      IComponentChangeService changeService = null;

      DesignerTransaction transaction = null;
      bool changed = false;

      try 
      {
        if (site != null) 
        {
          IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
          transaction = designerHost.CreateTransaction("BuildTabStrip");

          changeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
          if (changeService != null) 
          {
            try 
            {
              changeService.OnComponentChanging(component, null);
            }
            catch (CheckoutException ex) 
            {
              if (ex == CheckoutException.Canceled)
                return false;
              throw ex;
            }
          }
        }

        try 
        {
          TabStripTabsEditorForm oEditorForm = new TabStripTabsEditorForm(oControl);
          if (oEditorForm.ShowDialog(owner) == DialogResult.OK) 
          {
            changed = true;
          }
        }
        finally 
        {
          if (changed && changeService != null) 
          {
            changeService.OnComponentChanged(oControl, null, null, null);
          }
        }
      }
      finally 
      {
        if (transaction != null) 
        {
          if (changed) 
          {
            transaction.Commit();
          }
          else 
          {
            transaction.Cancel();
          }
        }
      }

      return changed;
    }
  }
}
