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

namespace ComponentArt.Win.Demos 
{
    public partial class ToolBarEnableDisableFeatures : UserControl, IDisposable
    {
        public ToolBarEnableDisableFeatures() 
        {
            InitializeComponent();
        }

        private void EnableToolBar_Unchecked(object sender, RoutedEventArgs e)
        {
            ToolBar1.IsEnabled = false;

            EnableCommands.IsChecked = false;
            EnableAlignment.IsChecked = false;
            EnableBullets.IsChecked = false;

            EnableCommands.IsEnabled = false;
            EnableAlignment.IsEnabled = false;
            EnableBullets.IsEnabled = false;
        }

        private void EnableToolBar_Checked(object sender, RoutedEventArgs e)
        {
            ToolBar1.IsEnabled = true;

            EnableCommands.IsChecked = true;
            EnableAlignment.IsChecked = true;
            EnableBullets.IsChecked = true;

            EnableCommands.IsEnabled = true;
            EnableAlignment.IsEnabled = true;
            EnableBullets.IsEnabled = true;
        }

        private void EnableCommands_Unchecked(object sender, RoutedEventArgs e)
        {
            EditCut.IsEnabled = false;
            EditCopy.IsEnabled = false;
            EditPaste.IsEnabled = false;
        }

        private void EnableCommands_Checked(object sender, RoutedEventArgs e)
        {
            EditCut.IsEnabled = true;
            EditCopy.IsEnabled = true;
            EditPaste.IsEnabled = true;
        }

        private void EnableBullets_Unchecked(object sender, RoutedEventArgs e)
        {
            BulletsNumbering.IsEnabled = false;
            BulletsSymbols.IsEnabled = false;
        }

        private void EnableBullets_Checked(object sender, RoutedEventArgs e)
        {
            BulletsNumbering.IsEnabled = true;
            BulletsSymbols.IsEnabled = true;
        }

        private void EnableAlignment_Unchecked(object sender, RoutedEventArgs e)
        {
            AlignLeft.IsEnabled = false;
            AlignCenter.IsEnabled = false;
            AlignRight.IsEnabled = false;
        }

        private void EnableAlignment_Checked(object sender, RoutedEventArgs e)
        {
            AlignLeft.IsEnabled = true;
            AlignCenter.IsEnabled = true;
            AlignRight.IsEnabled = true;
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
