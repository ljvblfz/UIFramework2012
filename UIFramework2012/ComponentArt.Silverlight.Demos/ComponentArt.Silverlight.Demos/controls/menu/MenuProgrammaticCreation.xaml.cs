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
using ComponentArt.Silverlight.UI.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace ComponentArt.Silverlight.Demos
{
    public partial class MenuProgrammaticCreation : UserControl, IDisposable
    {
        Menu mymenu;

        public MenuProgrammaticCreation()
        {
            InitializeComponent();
        }

        void mymenu_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private MenuItem createMenuItemSource(String label, Boolean sep, String icon, String item_id )
        {
            MenuItem temp = new MenuItem();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mymenu = new Menu();
            mymenu.Orientation = Orientation.Horizontal;
            mymenu.HorizontalAlignment = HorizontalAlignment.Stretch;

            mymenu.Items.Add(createMenuItemSource("File", false, null, "file"));
            mymenu.GetItemById("file").Items.Add(createMenuItemSource("New", false, "MenuIcons/New.png", "file_new"));
            mymenu.GetItemById("file").Items.Add(createMenuItemSource("Open", false, "MenuIcons/Open.png", "file_open"));
            mymenu.GetItemById("file").Items.Add(createMenuItemSource("", true, "", ""));
            mymenu.GetItemById("file").Items.Add(createMenuItemSource("Save", false, "MenuIcons/Save.png", "file_save"));
            mymenu.GetItemById("file").Items.Add(createMenuItemSource("Save As...", false, "", "file_saveas"));
            mymenu.GetItemById("file").Items.Add(createMenuItemSource("Save As Web Page", false, "MenuIcons/SaveAsWebPage.png", "file_saveaswebpage"));
            mymenu.GetItemById("file").Items.Add(createMenuItemSource("FileSearch...", false, "MenuIcons/FileSearch.png", "file_filesearch"));
            mymenu.GetItemById("file").Items.Add(createMenuItemSource("", true, "", ""));
            mymenu.GetItemById("file").Items.Add(createMenuItemSource("Exit", false, "", "file_exit"));

            mymenu.Items.Add(createMenuItemSource("Edit", false, null, "edit"));
            mymenu.GetItemById("edit").Items.Add(createMenuItemSource("Cut", false, "MenuIcons/Cut.png", "edit_cut"));
            mymenu.GetItemById("edit").Items.Add(createMenuItemSource("Copy", false, "MenuIcons/Copy.png", "edit_copy"));
            mymenu.GetItemById("edit").Items.Add(createMenuItemSource("Office Clipboard", false, "MenuIcons/OfficeClipboard.png", "edit_officeclipboard"));
            mymenu.GetItemById("edit").Items.Add(createMenuItemSource("Paste", false, "MenuIcons/Paste.png", "edit_paste"));

            mymenu.Items.Add(createMenuItemSource("View", false, null, "view"));
            mymenu.GetItemById("view").Items.Add(createMenuItemSource("Normal", false, "MenuIcons/ViewNormal.png", "view_normal"));
            mymenu.GetItemById("view").Items.Add(createMenuItemSource("Web Layout", false, "MenuIcons/ViewWebLayout.png", "view_web"));
            ((MenuItem)mymenu.Items.ElementAt(2)).Items.Add(createMenuItemSource("Outline", false, "MenuIcons/ViewOutline.png", "view_outline"));
            mymenu.GetItemById("view").Items.Add(createMenuItemSource("", true, "", ""));
            mymenu.GetItemById("view").Items.Add(createMenuItemSource("Navigate", false, "", "view_navigate"));
            mymenu.GetItemById("view_navigate").Items.Add(createMenuItemSource("Back", false, "", "view_navigate_back"));
            mymenu.GetItemById("view_navigate").Items.Add(createMenuItemSource("Forward", false, "", "view_navigate_forward"));

            MenuRoot.Children.Add(mymenu);
        }

        #region IDisposable Members

        public void Dispose()
        {
			if( mymenu != null)
			{
	            mymenu.Dispose();
			}
        }

        #endregion
    }
}
