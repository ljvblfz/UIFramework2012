using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ComponentArt.Win.UI.Navigation;

namespace ComponentArt.Win.Demos
{
    /// <summary>
    /// Interaction logic for TreeViewLinesFeatures.xaml
    /// </summary>
    public partial class TreeViewLinesFeatures : UserControl
    {
        public TreeViewLinesFeatures()
        {
            InitializeComponent();
        }
        private void colors_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton s = (RadioButton)sender;
            if ((bool)s.IsChecked && FirstTree != null)
            {
                FirstTree.TreeLineStroke = s.Foreground;
            }
        }
        private void lines_Checked(object sender, RoutedEventArgs e)
        {
            if (FirstTree == null) return;
            RadioButton s = (RadioButton)sender;
            if ((bool)lineNone.IsChecked)
            {
                FirstTree.TreeLineStyle = TreeViewLineStyles.None;
            }
            else if ((bool)lineFull.IsChecked)
            {
                FirstTree.TreeLineStyle = TreeViewLineStyles.Solid;
            }
        }
    }
}
