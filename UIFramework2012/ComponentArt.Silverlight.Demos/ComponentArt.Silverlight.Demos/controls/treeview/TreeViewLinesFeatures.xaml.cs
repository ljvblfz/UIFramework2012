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
using ComponentArt.Silverlight.UI.Navigation;

namespace ComponentArt.Silverlight.Demos
{
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
        else if ((bool)lineDotted.IsChecked)
        {
            FirstTree.TreeLineStyle = TreeViewLineStyles.Dotted;
        }
    }
  }
}
