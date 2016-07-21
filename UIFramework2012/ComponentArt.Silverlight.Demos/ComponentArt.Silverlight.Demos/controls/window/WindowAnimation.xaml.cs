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
using ComponentArt.Silverlight.UI.Layout;

namespace ComponentArt.Silverlight.Demos
{
    public partial class WindowAnimation : UserControl
    {
        public WindowAnimation()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1.IsOpen = !Window1.IsOpen;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window2.IsOpen = !Window2.IsOpen;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window3.IsOpen = !Window3.IsOpen;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Window4.IsOpen = !Window4.IsOpen;
        }
    }
}
