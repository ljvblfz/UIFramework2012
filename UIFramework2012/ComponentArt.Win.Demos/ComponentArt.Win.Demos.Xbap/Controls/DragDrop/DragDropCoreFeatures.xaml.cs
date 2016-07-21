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
using ComponentArt.Win.UI.Data;
using System.Windows.Media.Imaging;

namespace ComponentArt.Win.Demos
{
    public partial class DragDropCoreFeatures : UserControl
  {
        public DragDropCoreFeatures()
    {
        InitializeComponent();
        ComponentArt.Win.UI.DragDrop.DragCancelled += new EventHandler<ComponentArt.Win.UI.DragEventArgs>(DragDrop_DragCancelled);

        DataGrid1.GroupBy(DataGrid1.Columns[5], SortDirection.Ascending);
        LoadAllData();
    }

    void DragDrop_DragCancelled(object sender, ComponentArt.Win.UI.DragEventArgs e)
    {
        Storyboard warning = Resources["errorMsgAnimation"] as Storyboard;
        warning.Begin();
    }

    private void DataGrid1_DragEnter(object sender, ComponentArt.Win.UI.DragEventArgs e)
    {
        e.Cancel = true;
        if (e.Source != DataGrid1)
            e.DragControl.Overlay = ((DataTemplate)Resources["DropNotAllowed"]).LoadContent() as FrameworkElement;
    }

    private void DataGrid1_DragStarted(object sender, ComponentArt.Win.UI.DragEventArgs e)
    {
        // customize Drag control for multiple rows
        if (e.Items.Count > 1)
        {
            e.DragControl.VisualControl.ContentTemplate = (DataTemplate)Resources["DataGridDragMultipleRows"];
            e.DragControl.VisualControl.Content = e.Items.Count;
            e.DragControl.VisualControl.Dispatcher.BeginInvoke((Action) delegate()
            {
                object t_grid = UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "ImageGrid");
                if (t_grid != null && t_grid is Grid)
                {
                    Grid grid = t_grid as Grid;
                    int margin = 0;
                    int count = 0;
                    foreach (DataGridRow row in e.Items)
                    {
                        string iconSource = (row.DataItem as MessageService.Message).LargeEmailIcon;
                        grid.Children.Insert(0, new Image() 
                        { 
                            Margin = new Thickness(margin, margin, 0, 0), 
                            Source = new BitmapImage(new Uri(iconSource, UriKind.RelativeOrAbsolute)), 
                            Stretch = Stretch.None, VerticalAlignment = VerticalAlignment.Top, 
                            HorizontalAlignment = HorizontalAlignment.Left 
                        });
                        margin += 7;
                        count++;
                        // don't show too many icons
                        if (count > 10) break;
                    }
                }
            });
        }
    }


    // add dropped rows to the TreeView
    void FirstTree_NodeDrop(object sender, TreeViewNodeDropEventArgs e)
    {
        if (e.Source is DataGrid)
        {
            foreach (DataGridRow row in e.Items)
            {
                string header = (row.DataItem as MessageService.Message).Subject;
                string iconSource = (row.DataItem as MessageService.Message).EmailIcon;
                if (e.TargetParentNode != null)
                {
                    e.TargetParentNode.Items.Insert(e.TargetIndex, new TreeViewNode() { Header = header, IconSource = new BitmapImage(new Uri(iconSource, UriKind.RelativeOrAbsolute)) });
                    if (!e.TargetParentNode.IsExpanded)
                        e.TargetParentNode.IsExpanded = true;
                }
                else
                {
                    (e.Target as ComponentArt.Win.UI.Navigation.TreeView).Items.Insert(e.TargetIndex, new TreeViewNode() { Header = header, IconSource = new BitmapImage(new Uri(iconSource, UriKind.RelativeOrAbsolute)) });
                }
            }
        }
    }

    // set the description overlay
    private void FirstTree_NodeDragEnter(object sender, TreeViewNodeDragEventArgs e)
    {
        string target = "the root of the TreeView";
        if (e.TargetNode != null)
            target = e.TargetNode.ToString();

        e.DragControl.Overlay = new ContentControl()
        {
            Content = "Copy to " + target,
            Style = (Style)Resources["OverlayText"]
        };
    }

    #region Loading DataGrid data
    public void LoadAllData()
    {
        Uri service = new Uri(DemoWebHelper.BaseUri, "../Services/WCFMessageService.svc");

        MessageService.WCFMessageServiceClient oSoapClient = new MessageService.WCFMessageServiceClient("BasicHttpBinding_WCFMessageService", service.AbsoluteUri);

        oSoapClient.GetRecordsCompleted += oSoapClient_GetAllRecordsCompleted;
        oSoapClient.GetRecordsAsync(int.MaxValue, 0, DataGrid1.SortExpression);
    }

    void oSoapClient_GetAllRecordsCompleted(object sender, MessageService.GetRecordsCompletedEventArgs e)
    {
        DataGrid1.ItemsSource = e.Result;
    }

    public void Dispose()
    {
        DataGrid1.Dispose();
    }
    #endregion
  }

}
