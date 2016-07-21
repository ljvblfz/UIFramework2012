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
using System.Collections.ObjectModel;

namespace ComponentArt.Silverlight.Demos
{
    public partial class TreeViewSOAUICustom : UserControl
  {
    public TreeViewSOAUICustom()
    {
      InitializeComponent();
    }

    private void TreeView_LoadOnDemand(object sender, ComponentArt.Silverlight.UI.Navigation.TreeViewNodeCancelLoadOnDemandEventArgs e)
    {
        TreeViewFileBrowserService.SoaTreeViewServiceClient soaClient =
            new TreeViewFileBrowserService.SoaTreeViewServiceClient("BasicHttpBinding_ISoaTreeViewService", 
                new Uri(Application.Current.Host.Source, "../TreeViewFileBrowserService.svc").AbsoluteUri);

        soaClient.GetNodesCompleted += new EventHandler<TreeViewFileBrowserService.GetNodesCompletedEventArgs>(soaClient_GetNodesCompleted);
        soaClient.GetNodesAsync(e.CreateRequest(), e.Node);
    }

    void soaClient_GetNodesCompleted(object sender, ComponentArt.Silverlight.Demos.TreeViewFileBrowserService.GetNodesCompletedEventArgs e)
    {
          if (e.Result != null && e.Result.Nodes != null)
          {
              BaseTreeViewNode parentNode = (e.UserState == null) ? FileTree as BaseTreeViewNode : e.UserState as BaseTreeViewNode;
              _deserializeNewSoaNodes(parentNode, e.Result.Nodes);
              
              // make sure you set this before manually expanding the subtree
              parentNode.IsLoadOnDemandCompleted = true;
              if (parentNode is TreeViewNode)
              {
                  (parentNode as TreeViewNode).IsExpanded = true;
              }
              else if (parentNode.Items != null && parentNode.Items.Count > 0)
              {
                  (parentNode.Items[0] as TreeViewNode).IsExpanded = true;
              }
          }
    }

    // recursively walk the resultset and add nodes to the tree
    private void _deserializeNewSoaNodes(BaseTreeViewNode parentNode, ObservableCollection<SOA.UI.SoaTreeViewNode> nodes)
    {
        foreach (SOA.UI.SoaTreeViewNode dataNode in nodes)
        {
            TreeViewNode newNode = new TreeViewNode() { SoaData = dataNode };
            parentNode.Items.Add(newNode);
            if (dataNode.Items != null && dataNode.Items.Count > 0)
                _deserializeNewSoaNodes(newNode, dataNode.Items);
        }
    }
  }
}
