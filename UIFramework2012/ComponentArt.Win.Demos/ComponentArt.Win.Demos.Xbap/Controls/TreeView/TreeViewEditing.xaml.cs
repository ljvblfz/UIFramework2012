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

using ComponentArt.SOA.UI;
using System.Diagnostics;
using ComponentArt.Win.UI.Utils;
using ComponentArt.Win.UI.Navigation;
using System.Windows.Media.Imaging;

namespace ComponentArt.Win.Demos.Xbap
{
    public partial class TreeViewEditing : UserControl
    {
        public TreeViewEditing()
        {
            InitializeComponent();
        }

        private int nextRootNode, nextChildNode;
        private void ToolBar_ItemClick(object sender, ToolBarItemEventArgs e)
        {
            if (e.Item == NewRootNode)
            {
                TreeViewNode newRootNode = new TreeViewNode() 
                { 
                    Header = "Root node " + (nextRootNode++).ToString(), 
                    IsSelected = true,
                    AllowNodeEditing = true,
                    IconSource = new BitmapImage(new Uri("/ComponentArt.Win.Demos.Xbap;component/Assets/TreeView/TreeViewSOAUIEditing/folder.png", UriKind.RelativeOrAbsolute)),
                    ExpandedIconSource = new BitmapImage(new Uri("/ComponentArt.Win.Demos.Xbap;component/Assets/TreeView/TreeViewSOAUIEditing/folder_open.png", UriKind.RelativeOrAbsolute))
                };
                myTree.Items.Add(newRootNode);
                newRootNode.IsInEditMode = true;

                EditNode.IsEnabled = true;
                DeleteNode.IsEnabled = true;
                NewChildNode.IsEnabled = true;
            }
            else if (e.Item == NewChildNode && myTree.SelectedNode != null)
            {
                TreeViewNode newNode = new TreeViewNode()
                {
                    Header = "Child node " + (nextChildNode++).ToString(),
                    IsSelected = true,
                    AllowNodeEditing = true,
                    IconSource = new BitmapImage(new Uri("/ComponentArt.Win.Demos.Xbap;component/Assets/TreeView/TreeViewSOAUIEditing/folder.png", UriKind.RelativeOrAbsolute)),
                    ExpandedIconSource = new BitmapImage(new Uri("/ComponentArt.Win.Demos.Xbap;component/Assets/TreeView/TreeViewSOAUIEditing/folder_open.png", UriKind.RelativeOrAbsolute))
                };
                myTree.SelectedNode.Items.Add(newNode);
                newNode.ParentNode.IsExpanded = true;
                newNode.IsInEditMode = true;
            }
            else if (e.Item == EditNode && myTree.SelectedNode != null)
            {
                myTree.SelectedNode.IsInEditMode = true;
            }
            else if (e.Item == DeleteNode && myTree.SelectedNode != null)
            {
                TreeViewNode deletedNode = myTree.SelectedNode;
                if (deletedNode.PreviousSibling != null)
                    deletedNode.PreviousSibling.IsSelected = true;
                else if (deletedNode.NextSibling != null)
                    deletedNode.NextSibling.IsSelected = true;
                else if (deletedNode.ParentNode != null)
                    deletedNode.ParentNode.IsSelected = true;

                if (deletedNode == myTree.Items[0] && myTree.Items.Count < 2)
                {
                    EditNode.IsEnabled = false;
                    DeleteNode.IsEnabled = false;
                    NewChildNode.IsEnabled = false;
                }
                deletedNode.Detach();
            }
        }
    }
}
