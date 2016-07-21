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
using ComponentArt.Win.UI.Navigation;


namespace ComponentArt.Win.Demos
{
  public partial class FileExplorer : UserControl
  {
    public FileExplorer()
    {
      InitializeComponent();
      TreeViewBrowser.SoaServiceUrl = new Uri(DemoWebHelper.BaseUri, "../Services/TreeViewFileBrowserService.svc").ToString();
      DataGrid1.SoaServiceUrl = new Uri(DemoWebHelper.BaseUri, "../Services/DataGridFileBrowserService.svc").ToString();

      ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
      TreeViewBrowser.ExpanderClosedIconSource = (ImageSource)imageSourceConverter.ConvertFromString(new Uri(DemoWebHelper.BaseUri, "../ClientBin/controls/treeview/icons/FileBrowser/plus_16x16.png").ToString());
      TreeViewBrowser.ExpanderOpenIconSource = (ImageSource)imageSourceConverter.ConvertFromString(new Uri(DemoWebHelper.BaseUri, "../ClientBin/controls/treeview/icons/FileBrowser/minus_18x21.png").ToString());

    }

    void TreeView_NodeSelected(object sender, TreeViewNodeMouseEventArgs e)
    {
      if (e.Node.Tag != null)
      {
        DataGrid1.SoaRequestTag = e.Node.Tag;
        DataGrid1.Reload();
      }
    }
  }
}
