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
using System.Text;

using ComponentArt.Silverlight.UI.Navigation;
using System.Collections.ObjectModel;

namespace ComponentArt.Silverlight.Demos
{
  public partial class TreeViewLoadOnDemandFeatures : UserControl
  {
      public TreeViewLoadOnDemandFeatures()
    {
      InitializeComponent();
    }

      private void FirstTree_LoadOnDemand(object sender, TreeViewNodeCancelEventArgs e)
      {
          Uri service = new Uri(Application.Current.Host.Source, "../ManualTreeViewFileExplorerService.svc");
          ManualTreeViewFileExplorerService.ManualTreeViewFileExplorerServiceClient wc = new ManualTreeViewFileExplorerService.ManualTreeViewFileExplorerServiceClient
                ("BasicHttpBinding_ManualTreeViewFileExplorerService", service.AbsoluteUri);
          wc.GetNodesCompleted += new EventHandler<ManualTreeViewFileExplorerService.GetNodesCompletedEventArgs>(wc_GetNodesCompleted);
          string path = "";
          if (e.Node != null) path = (e.Node as TreeViewNode).Id;
          wc.GetNodesAsync(path, e.Node);
      }

      void wc_GetNodesCompleted(object sender, ComponentArt.Silverlight.Demos.ManualTreeViewFileExplorerService.GetNodesCompletedEventArgs e)
      {
          if (e.Result != null)
          {
              BaseTreeViewNode parentNode = (e.UserState == null) ? FirstTree as BaseTreeViewNode : e.UserState as BaseTreeViewNode;
              addWSNodes(parentNode, e.Result);
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

      private void addWSNodes(BaseTreeViewNode parentNode, ObservableCollection<ManualTreeViewFileExplorerService.MyTreeViewNodeData> nodes)
      {
          foreach (ManualTreeViewFileExplorerService.MyTreeViewNodeData node in nodes)
          {
              TreeViewNode newNode = new TreeViewNode()
              {
                  IconSource = new System.Windows.Media.Imaging.BitmapImage(new Uri(node.IconSource, UriKind.RelativeOrAbsolute)),
                  Header = node.Header,
                  Id = node.Id,
                  IsLoadOnDemandEnabled = node.IsLoadOnDemandEnabled
              };
              parentNode.Items.Add(newNode);
              if (node.Nodes != null && node.Nodes.Count > 0)
                  addWSNodes(newNode, node.Nodes);
          }
      }
  }

  
 
}
