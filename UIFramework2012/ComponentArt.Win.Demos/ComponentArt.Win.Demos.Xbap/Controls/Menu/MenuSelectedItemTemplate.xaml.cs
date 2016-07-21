using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ComponentArt.Win.UI.Navigation;
using System.ComponentModel;

namespace ComponentArt.Win.Demos {
	public partial class MenuSelectedItemTemplate : UserControl, IDisposable
    {
        public MenuSelectedItemTemplate()
        {
			InitializeComponent();
		}

		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
		}

        #region IDisposable Members

        public void Dispose()
        {
            SelectedItemMenu.Dispose();
        }

        #endregion

        private void SelectedItemMenu_MenuClick(object sender, MenuCommandEventArgs mce)
        {
            foreach (ComponentArt.Win.UI.Navigation.MenuItem _mi in SelectedItemMenu.Items)
            {
                ClearMenuItemSelections(_mi);
            }
            
            ComponentArt.Win.UI.Navigation.MenuItem mi = mce.ItemSource;

            do
            {
                if (mi.IsTopLevel != true)
                {
                    if (mi.HasSubMenu)
                    {
                        mi.CustomTemplate = "SelectedTemplateWithSubMenu";
                    }
                    else
                    {
                        mi.CustomTemplate = "SelectedTemplate";
                    }
                }
                mi = mi.GetParentItem();
            } while (mi.IsTopLevel != true);
            mi.CustomTemplate = "SelectedTemplateTop";
        }


        private void ClearMenuItemSelections(ComponentArt.Win.UI.Navigation.MenuItem mi)
        {
            mi.CustomTemplate = null;
            foreach (ComponentArt.Win.UI.Navigation.MenuItem _mi in mi.Items)
            {
                _mi.CustomTemplate = null;
                if (_mi.Items.Count > 0)
                {
                    ClearMenuItemSelections(_mi);
                }
            }
        }
    }
}
