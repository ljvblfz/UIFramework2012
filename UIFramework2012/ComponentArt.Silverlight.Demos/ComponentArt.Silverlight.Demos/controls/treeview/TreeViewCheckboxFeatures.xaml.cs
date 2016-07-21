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
    public partial class TreeViewCheckboxFeatures : UserControl
  {
      public TreeViewCheckboxFeatures()
    {
      InitializeComponent();
    }

      private void CheckboxTree_NodeChecked(object sender, ComponentArt.Silverlight.UI.Navigation.TreeViewNodeCheckBoxEventArgs e)
      {
          string state = "unchecked";
          if (e.NewValue == true)
              state = "checked";
          else if (e.NewValue == null)
              state = "indeterminate";
              
          Selection.Items.Add(string.Format("{0} is {1}", e.Node, state));
          // select & scroll to the bottom
          Selection.Dispatcher.BeginInvoke(() =>
              {
                  Selection.SelectedIndex = Selection.Items.Count - 1;
                  Selection.ScrollIntoView(Selection.SelectedItem);
              });
      }

      private void Clear_Click(object sender, RoutedEventArgs e)
      {
          Selection.Items.Clear();
      }
  }
}
