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
using ComponentArt.Win.UI.Utils;

namespace ComponentArt.Win.Demos
{
    public partial class ContextMenuSimple : UserControl, IDisposable
    {
        public ContextMenuSimple()
        {
            InitializeComponent();
        }


        #region IDisposable Members

        void IDisposable.Dispose()
        {
            SimpleMenu.Dispose();
        }

        #endregion

        private void SimpleMenu_MenuClick(object sender, MenuCommandEventArgs mce)
        {
            // dbg.Text += "Click: " + mce.itemSource.Text + "\n";
            ControlEvent.Text = "Clicked: " + mce.ItemSource.Text + "\n";
        }
    }
}
