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
using System.Windows.Media.Imaging;

using ComponentArt.SOA.UI;

namespace ComponentArt.SOA.Demos
{
    public partial class TreeViewStructureEdit : UserControl
    {
        public TreeViewStructureEdit()
        {
            InitializeComponent();
        }
        private void TreeView_BeforeSoaRequestSent(object sender, ComponentArt.Silverlight.UI.Navigation.TreeViewSoaRequestCancelEventArgs e)
        {
            if (e.Request is SoaTreeViewAddRequest)
                addToList("AddNode request sent, initiated through code");
            else if (e.Request is SoaTreeViewDeleteRequest)
                addToList("DeleteNode request sent, initiated through code");
            else if (e.Request is SoaTreeViewEditRequest)
                addToList("EditNode request sent, initiated through end-user action");
            else if (e.Request is SoaTreeViewMoveRequest)
                addToList("MoveNode request sent, initiated through end-user action");

            //e.IsBusyIndicatorVisible = (endUserFeedback.IsChecked == true);
            e.Request.Tag = (cancelEvents.IsChecked == true);
        }

        private void TreeView_BeforeSoaResponseProcessed(object sender, ComponentArt.Silverlight.UI.Navigation.TreeViewSoaResponseCancelEventArgs e)
        {
            string actionResult = (e.Response as SoaTreeViewCancelResponse).Cancel ? " was cancelled" : " was successful";
            if (e.Response is SoaTreeViewAddResponse)
                addToList("AddNode " + actionResult);
            else if (e.Response is SoaTreeViewDeleteResponse)
                addToList("DeleteNode" + actionResult);
            else if (e.Response is SoaTreeViewEditResponse)
                addToList("EditNode" + actionResult);
            else if (e.Response is SoaTreeViewMoveResponse)
                addToList("MoveNode" + actionResult);
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
                    IconSource = new BitmapImage(new Uri("/ComponentArt.SOA.Demos;component/assets/TreeViewEdit/folder.png", UriKind.RelativeOrAbsolute)),
                    ExpandedIconSource = new BitmapImage(new Uri("/ComponentArt.SOA.Demos;component/assets/TreeViewEdit/folder_open.png", UriKind.RelativeOrAbsolute))
                };
                myTree.Items.Add(newRootNode);
                newRootNode.IsInEditMode = true;

                EditNode.IsEnabled = true;
                DeleteNode.IsEnabled = true;
                NewChildNode.IsEnabled = true;

                SoaTreeViewAddRequest addReq = new SoaTreeViewAddRequest() { Node = newRootNode.SoaData };
                myTree.SendSoaRequest(addReq);
            }
            else if (e.Item == NewChildNode && myTree.SelectedNode != null)
            {
                TreeViewNode newNode = new TreeViewNode()
                {
                    Header = "Child node " + (nextChildNode++).ToString(),
                    IsSelected = true,
                    IconSource = new BitmapImage(new Uri("/ComponentArt.SOA.Demos;component/assets/TreeViewEdit/folder.png", UriKind.RelativeOrAbsolute)),
                    ExpandedIconSource = new BitmapImage(new Uri("/ComponentArt.SOA.Demos;component/assets/TreeViewEdit/folder_open.png", UriKind.RelativeOrAbsolute))
                };
                myTree.SelectedNode.Items.Add(newNode);
                newNode.ParentNode.IsExpanded = true;
                newNode.IsInEditMode = true;

                SoaTreeViewAddRequest addReq = new SoaTreeViewAddRequest() { Node = newNode.SoaData };
                myTree.SendSoaRequest(addReq);
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
                SoaTreeViewDeleteRequest delReq = new SoaTreeViewDeleteRequest() { Node = deletedNode.SoaData };
                deletedNode.Detach();
                myTree.SendSoaRequest(delReq);
            }
        }

        private void addToList(string msg)
        {
            eventList.Items.Add(new EventDisplay(msg));
            // select & scroll to the bottom
            eventList.Dispatcher.BeginInvoke(() =>
            {
                eventList.ScrollIntoView(eventList.Items[eventList.Items.Count - 1]);
            });
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            eventList.Items.Clear();
        }
        // used to avoid bugs in Listbox.ScrollIntoView() when Listbox.Items are strings
        private class EventDisplay
        {
            string EventMsg;
            public EventDisplay(string msg)
            { EventMsg = msg; }
            public override string ToString()
            {
                return EventMsg;
            }
        }
    }
}
