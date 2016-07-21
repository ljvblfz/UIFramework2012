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

namespace ComponentArt.Silverlight.Demos
{
    public partial class ContextMenuAPIControl : UserControl, IDisposable
    {
        private bool toggle;

        public ContextMenuAPIControl()
        {
            InitializeComponent();
        }

        #region IDisposable Members
        void IDisposable.Dispose()
        {
            ApiContextMenu.Dispose();
        }


        #endregion

        private void ApiContextMenu_Initialized(object sender, RoutedEventArgs e)
        {

        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShowButton.Content.ToString().IndexOf("Show") > -1)
            {
                ShowButton.Content = "Hide Context Menu";
                ApiContextMenu.ShowContextMenu(new Point(Convert.ToDouble(cx.Text), Convert.ToDouble(cy.Text)));
            }
            else
            {
                ShowButton.Content = "Show Context Menu";
                ApiContextMenu.HideContextMenu();
            }
        }

        private void ApiContextMenu_MenuClick(object sender, ComponentArt.Silverlight.UI.Navigation.MenuCommandEventArgs mce)
        {
            ShowButton.Content = "Show Context Menu";
        }
    }
}
