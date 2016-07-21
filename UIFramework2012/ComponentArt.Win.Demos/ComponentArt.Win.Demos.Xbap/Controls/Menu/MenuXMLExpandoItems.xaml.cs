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
    public partial class MenuXMLExpandoItems : UserControl, IDisposable
    {
        public MenuXMLExpandoItems()
        {
            InitializeComponent();
        }

        private void MyMenu_MenuClick(object sender, MenuCommandEventArgs mce)
        {

        }

        private void MyMenu_Initialized(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("MyMenu_Initialized");
        }


        #region IDisposable Members

        public void Dispose()
        {
            MyMenu.Dispose();
        }

        #endregion
    }
}
