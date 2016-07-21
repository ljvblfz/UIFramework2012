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
    public partial class ToolBarOrientation : UserControl
    {
        public ToolBarOrientation()
        {
            InitializeComponent();
        }

        private void HorizontalOrientation_Checked(object sender, RoutedEventArgs e)
        {
            if(ToolBar1 != null)
                ToolBar1.Orientation = Orientation.Horizontal;
        }

        private void VerticalOrientation_Checked(object sender, RoutedEventArgs e)
        {
            if (ToolBar1 != null)
                ToolBar1.Orientation = Orientation.Vertical;
        }
    }
}
