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
    public partial class WindowAlignment : UserControl
    {
        public WindowAlignment()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1.IsOpen = true;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Window1 == null || !Window1.IsOpen) return;

            switch (((ComboBox)sender).SelectedIndex)
            {
                case 0:
                    Window1.Alignment = ComponentArt.Silverlight.UI.Layout.WindowAlignment.Center;
                    break;
                case 1:
                    Window1.Alignment = ComponentArt.Silverlight.UI.Layout.WindowAlignment.TopLeft;
                    break;
                case 2:
                    Window1.Alignment = ComponentArt.Silverlight.UI.Layout.WindowAlignment.TopRight;
                    break;
                case 3:
                    Window1.Alignment = ComponentArt.Silverlight.UI.Layout.WindowAlignment.BottomLeft;
                    break;
                case 4:
                    Window1.Alignment = ComponentArt.Silverlight.UI.Layout.WindowAlignment.BottomRight;
                    break;
            }
        }

    }
}
