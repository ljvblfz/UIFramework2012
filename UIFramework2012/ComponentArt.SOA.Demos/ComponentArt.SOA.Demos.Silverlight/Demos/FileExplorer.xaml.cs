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


namespace ComponentArt.SOA.Demos
{
  public partial class FileExplorer : UserControl
  {
    public FileExplorer()
    {
      InitializeComponent();
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
