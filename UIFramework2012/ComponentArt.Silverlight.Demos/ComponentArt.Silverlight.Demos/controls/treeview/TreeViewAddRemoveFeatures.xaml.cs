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
    public partial class TreeViewAddRemoveFeatures : UserControl
  {
        public TreeViewAddRemoveFeatures()
    {
      InitializeComponent();
      ((FirstTree.Items[0] as TreeViewNode).Items[0] as TreeViewNode).IsSelected = true;
    }
    private int counter = 0;
    private void add_Click(object sender, RoutedEventArgs e)
    {
        counter++;
        TreeViewNode newNode = new TreeViewNode() 
            { 
                Header = "New node " + counter.ToString(),
                IconSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("/controls/treeview/icons/folder.png", UriKind.RelativeOrAbsolute))
            };
        if (FirstTree.SelectedNodes.Count > 0)
        {

            (FirstTree.SelectedNodes[0] as TreeViewNode).Items.Add(newNode);
            (FirstTree.SelectedNodes[0] as TreeViewNode).IsExpanded = true;
        }
        else
        {
            FirstTree.Items.Add(newNode);
        }
    }

    private void remove_Click(object sender, RoutedEventArgs e)
    {
        if (FirstTree.SelectedNodes.Count > 0)
        {
            TreeViewNode deleted = FirstTree.SelectedNodes[0];
            ItemsControl parent = (deleted.Parent as ItemsControl);
            deleted.IsSelected = false;
            parent.Items.Remove(deleted);
            if (parent.Items.Count > 0 && parent.Items[0] != null)
            {
                (parent.Items[0] as TreeViewNode).IsSelected = true;
            }
            else
            {
                if (parent is TreeViewNode) (parent as TreeViewNode).IsSelected = true;
            }
        }
    }

    private void disable_Click(object sender, RoutedEventArgs e)
    {
        if (FirstTree.SelectedNodes.Count > 0)
        {
            TreeViewNode deleted = FirstTree.SelectedNodes[0];
            deleted.IsDisabled = true;
        }
    }
  }
}
