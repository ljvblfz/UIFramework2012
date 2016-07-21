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
    public partial class ToolBarOrientation : UserControl, IDisposable
    {
        public ToolBarOrientation() 
        {
            InitializeComponent();
        }
        private void HorizontalOrientation_Checked(object sender, RoutedEventArgs e)
        {
            if (ToolBar1 != null)
            {
                ToolBar1.Orientation = Orientation.Horizontal;
                ToolBar1.Height = 28;
                ToolBar1.Width = 380;
            }
        }

        private void VerticalOrientation_Checked(object sender, RoutedEventArgs e)
        {
            if (ToolBar1 != null)
            {
                ToolBar1.Orientation = Orientation.Vertical;
                ToolBar1.Height = 380;
                ToolBar1.Width = 28;
            }
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
