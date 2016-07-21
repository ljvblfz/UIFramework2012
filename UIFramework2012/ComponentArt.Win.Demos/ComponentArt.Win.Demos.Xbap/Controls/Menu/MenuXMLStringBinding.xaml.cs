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
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace ComponentArt.Win.Demos
{
    public partial class MenuXMLStringBinding : UserControl, IDisposable
    {
        ComponentArt.Win.UI.Navigation.Menu mymenu;

        public MenuXMLStringBinding()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mymenu = new ComponentArt.Win.UI.Navigation.Menu();
            mymenu.XmlBindingSource = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                            <menu>
                                              <item label=""File"">
                                                <item label=""New..."" icon=""MenuIcons/New.png"" />
                                                <item label=""Open..."" icon=""MenuIcons/Open.png"" />
                                                <item separator=""true""/>
                                                <item label=""Save"" icon=""MenuIcons/Save.png"" />
                                                <item label=""Save As..."" />
                                              </item>
                                              <item label=""Edit"">
                                                <item label=""Cut"" image=""MenuIcons/Cut.png"" />
                                                <item label=""Copy"" icon=""MenuIcons/Copy.png"" />
                                                <separator/>
                                                <item label=""Office Clipboard..."" icon=""MenuIcons/OfficeClipboard.png"" />
                                                <item label=""Paste"" icon=""MenuIcons/Paste.png"" />
                                              </item>
                                              <item label=""View"">
                                                <item label=""Normal"" icon=""MenuIcons/ViewNormal.png"" />
                                                <item label=""Web Layout"" icon=""MenuIcons/ViewWebLayout.png"" />
                                                <item separator=""true""/>
                                                <item label=""Outline"" icon=""MenuIcons/ViewOutline.png"" />
                                              </item>
                                            </menu>";
            MenuPresenter.Content = mymenu;
        }

        #region IDisposable Members

        public void Dispose()
        {
            mymenu.Dispose();
        }

        #endregion
    }
}
