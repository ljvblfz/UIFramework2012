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
using System.Windows.Media.Imaging;

namespace ComponentArt.Win.Demos
{
    public partial class MenuAddRemoveDisableItems : UserControl, IDisposable
    {
        ComponentArt.Win.UI.Navigation.Menu mymenu;

        public MenuAddRemoveDisableItems()
        {
            mymenu = new ComponentArt.Win.UI.Navigation.Menu();
            mymenu.MenuClick += new ComponentArt.Win.UI.Navigation.Menu.MenuClickEventHandler(muDemo_ItemClick);
            mymenu.Width = 730;
            mymenu.HorizontalAlignment = HorizontalAlignment.Left;

            mymenu.Items.Add(createMenuItemSource("Add", false, null, "add"));
            mymenu.GetItemById("add").Items.Add(createMenuItemSource("File", false, "MenuIcons/New.png", "add_file"));
            mymenu.GetItemById("add").Items.Add(createMenuItemSource("File From Web", false, "MenuIcons/Open.png", "add_filefromweb"));
            mymenu.GetItemById("add").Items.Add(createMenuItemSource("Open", false, "MenuIcons/Open.png", "add_open"));
            mymenu.GetItemById("add").Items.Add(createMenuItemSource("Save", false, "MenuIcons/Save.png", "add_save"));
            mymenu.GetItemById("add").Items.Add(createMenuItemSource("Save As...", false, "", "add_saveas"));
            mymenu.GetItemById("add").Items.Add(createMenuItemSource("", true, "", ""));

            mymenu.Items.Add(createMenuItemSource("Disable", false, null, "disable"));
            mymenu.GetItemById("disable").Items.Add(createMenuItemSource("Cut", false, "MenuIcons/Cut.png", "disable_cut"));
            mymenu.GetItemById("disable").Items.Add(createMenuItemSource("Copy", false, "MenuIcons/Copy.png", "disable_copy"));
            mymenu.GetItemById("disable").Items.Add(createMenuItemSource("Paste", false, "MenuIcons/Paste.png", "disable_paste"));
            mymenu.GetItemById("disable").Items.Add(createMenuItemSource("Complete Word", false, "", "disable_completeword"));

            mymenu.Items.Add(createMenuItemSource("Remove", false, null, "remove"));
            mymenu.GetItemById("remove").Items.Add(createMenuItemSource("Stop", false, "MenuIcons/ViewNormal.png", "remove_stop"));
            mymenu.GetItemById("remove").Items.Add(createMenuItemSource("Refresh", false, "MenuIcons/ViewWebLayout.png", "remove_refresh"));
            //((ComponentArt.Win.UI.Navigation.MenuItem)mymenu.Items.ElementAt(2)).Items.Add(createMenuItemSource("Privacy Report", false, "MenuIcons/ViewOutline.png", "remove_privacyreport"));
            mymenu.GetItemById("remove").Items.Add(createMenuItemSource("Navigate", false, "", "remove_navigate"));
            mymenu.GetItemById("remove_navigate").Items.Add(createMenuItemSource("Back", false, "", "remove_navigate_back"));
            mymenu.GetItemById("remove_navigate").Items.Add(createMenuItemSource("Forward", false, "", "remove_navigate_forward"));

            InitializeComponent();
            MenuRoot.Children.Add(mymenu);

        }

        void muDemo_ItemClick(object sender, ComponentArt.Win.UI.Navigation.MenuCommandEventArgs e)
        {
            String action = mymenu.GetTopLevelItemFromChild(e.ItemSource).Id;
            if (action == "add")
            {
                ComponentArt.Win.UI.Navigation.MenuItem mi = ((ComponentArt.Win.UI.Navigation.MenuItem)e.ItemSource).GetParentItem();
                mi.Items.Add(createMenuItemSource("New " + e.ItemSource.Text, e.ItemSource.IsSeparator, "", "new_" + e.ItemSource.Id));
            }
            else if (action == "disable")
            {
                e.ItemSource.Enabled = false;

            }
            else if (action == "remove")
            {
                ((ComponentArt.Win.UI.Navigation.MenuItem)e.ItemSource).RemoveItem();
            }
        }

        private ComponentArt.Win.UI.Navigation.MenuItem createMenuItemSource(String label, Boolean sep, String icon, String item_id)
        {
            ComponentArt.Win.UI.Navigation.MenuItem temp = new ComponentArt.Win.UI.Navigation.MenuItem();
            temp.Text = label;
            temp.IsSeparator = sep;
            temp.Id = item_id;
            if (icon != null && icon != "")
            {
                BitmapImage bi = new BitmapImage(new Uri(icon.ToString(), UriKind.RelativeOrAbsolute));
                temp.IconSource = (ImageSource)bi;
            }
            return temp;
        }

        #region IDisposable Members

        public void Dispose()
        {
            mymenu.Dispose();
        }

        #endregion
    }
}
