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
using System.Reflection;
using System.Diagnostics;

namespace ComponentArt.Silverlight.Demos
{
    public partial class BestOfBreed_Navigation : UserControl
    {
        public BestOfBreed_Navigation()
        {
            InitializeComponent();
        }

        private void DemoButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                ((App)Application.Current).LoadDemo(sender, e);
            }
            else if (sender is HyperlinkButton)
            {
                ((App)Application.Current).LoadDemoHyper(sender, e);
            }
        }

        private void ControlButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                ((App)Application.Current).NavigateToDemo(sender, e);
            }
            else if (sender is HyperlinkButton)
            {
                ((App)Application.Current).NavigateToDemoHyper(sender, e);
            }
        }
    }
}
