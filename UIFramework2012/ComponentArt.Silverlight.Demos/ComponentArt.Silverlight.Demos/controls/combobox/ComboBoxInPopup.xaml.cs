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
using System.Diagnostics;

namespace ComponentArt.Silverlight.Demos
{
    public partial class ComboBoxInPopup : UserControl
    {
        public ComboBoxInPopup()
        {
            InitializeComponent();
        }

        private void ShowFormButton_Click(object sender, RoutedEventArgs e)
        {
            Window1.IsOpen = true;
        }

        private void Window1_Closed(object sender, ComponentArt.Silverlight.UI.Layout.WindowClosedEventArgs e)
        {
            Gender.Hide( false );
        }
    }
}
