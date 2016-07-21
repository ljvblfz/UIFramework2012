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
    public partial class WindowEvents : UserControl
    {
        public WindowEvents()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window2.IsOpen = true;
        }

        private void Window2_Opened(object sender, EventArgs e)
        {
            output.Text += "Opened\n";
        }

        private void Window2_Closed(object sender, WindowClosedEventArgs e)
        {
            output.Text += "Closed\n";
        }

        private void Window2_MinimizeClick(object sender, EventArgs e)
        {
            output.Text += "Minimize Click\n";
        }

        private void Window2_MaximizeClick(object sender, EventArgs e)
        {
            output.Text += "Maximize Click\n";
        }

        private void Window2_DragStart(object sender, EventArgs e)
        {
            output.Text += "Drag Started\n";
        }

        private void Window2_DragComplete(object sender, EventArgs e)
        {
            output.Text += "Drag Complete\n";
        }

        private void Window2_IconClick(object sender, EventArgs e)
        {
            output.Text += "Icon Click\n";
        }

        private void Window2_Resized(object sender, EventArgs e)
        {
            output.Text += "Resized\n";
        }

    }
}
