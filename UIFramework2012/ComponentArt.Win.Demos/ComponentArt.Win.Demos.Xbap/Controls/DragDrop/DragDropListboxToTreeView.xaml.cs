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
using ComponentArt.Win.UI;
using ComponentArt.Win.UI.Utils;
using ComponentArt.Win.UI.Navigation;

namespace ComponentArt.Win.Demos
{
    public partial class DragDropListboxToTreeView : UserControl
  {
        public DragDropListboxToTreeView()
    {
        InitializeComponent();
        ComponentArt.Win.UI.DragDrop.DragCancelled += new EventHandler<ComponentArt.Win.UI.DragEventArgs>(DragDrop_DragCancelled);
        FirstTree.NodeDrop += new EventHandler<TreeViewNodeDropEventArgs>(FirstTree_NodeDragDropEnd);

        dragSource.Items.Add(new DataItem("/Assets/TreeView/Core/root.png", "Mailbox"));
        dragSource.Items.Add(new DataItem("/Assets/TreeView/Core/calendar.png", "Calendar"));
        dragSource.Items.Add(new DataItem("/Assets/TreeView/Core/contacts.png", "Contacts"));
        dragSource.Items.Add(new DataItem("/Assets/TreeView/Core/deleted.png", "Deleted Items"));

    }
    void DragDrop_DragCancelled(object sender, ComponentArt.Win.UI.DragEventArgs e)
    {
        Storyboard warning = Resources["errorMsgAnimation"] as Storyboard;
        warning.Begin();
    }

    public void dragSource_DragStarted(object sender, ComponentArt.Win.UI.DragEventArgs e)
    {
        if ((sender as ListBox).SelectedItem != null)
            e.DragControl.Add((sender as ListBox).SelectedItem, Resources["sourceList"] as DataTemplate);
    }

    void FirstTree_NodeDragDropEnd(object sender, TreeViewNodeDropEventArgs e)
    {
        if (e.Source is ListBox)
        {
            (e.Source as ListBox).Items.Remove(e.Items[0]);
            if (e.TargetParentNode != null)
            {
                e.TargetParentNode.Items.Insert(e.TargetIndex, new TreeViewNode() { Header = (e.Items[0] as DataItem).Text, IconSource = new System.Windows.Media.Imaging.BitmapImage(new Uri((e.Items[0] as DataItem).IconSource, UriKind.RelativeOrAbsolute)) });
                if (!e.TargetParentNode.IsExpanded)
                    e.TargetParentNode.IsExpanded = true;
            }
            else
            {
                (e.Target as ComponentArt.Win.UI.Navigation.TreeView).Items.Insert(e.TargetIndex, new TreeViewNode() { Header = (e.Items[0] as DataItem).Text, IconSource = new System.Windows.Media.Imaging.BitmapImage(new Uri((e.Items[0] as DataItem).IconSource, UriKind.RelativeOrAbsolute)) });
            }
        }
    }
  }

    public class MyListBox1 : ListBox, IDropTarget
    {
        #region IDropTarget Members

        public void OnDrop(object sender, ComponentArt.Win.UI.DragEventArgs e)
        {
            e.Handled = true;
            if (e.Source is ComponentArt.Win.UI.Navigation.TreeView)
            {
                TreeViewNode node = e.Items[0] as TreeViewNode;
                {
                    node.Detach();
                    this.Items.Add(new DataItem(node.IconSource.UriSource().ToString(), node.Text));
                    foreach (TreeViewNode childNode in node.WalkDepthFirst())
                    {
                        childNode.Detach();
                        this.Items.Add(new DataItem(childNode.IconSource.UriSource().ToString(), childNode.Text));
                    }
                }
            }
            else if (e.Source is ListBox)
            {
                (e.Source as ListBox).Items.Remove(e.Items[0]);
                Items.Add(e.Items[0]);
            }
            this.Background = new SolidColorBrush(Colors.White);
        }

        public void OnDragEnter(object sender, ComponentArt.Win.UI.DragEventArgs e)
        {
            e.Handled = true;
            this.Background = new SolidColorBrush(Colors.LightGray);
        }

        public void OnDragLeave(object sender, ComponentArt.Win.UI.DragEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.White);
        }

        public void OnDragOver(object sender, ComponentArt.Win.UI.DragEventArgs e)
        {
            e.Handled = true;
        }

        public void OnDragHover(object sender, ComponentArt.Win.UI.DragEventArgs e)
        {
        }

        #endregion
    }
}
