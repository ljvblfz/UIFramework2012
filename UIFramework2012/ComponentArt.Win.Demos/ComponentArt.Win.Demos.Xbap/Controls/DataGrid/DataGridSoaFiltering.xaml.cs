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

namespace ComponentArt.Win.Demos
{
  public partial class DataGridSoaFiltering : UserControl
  {
    public DataGridSoaFiltering()
    {
      InitializeComponent();

      DataGrid1.SoaServiceUrl = new Uri(DemoWebHelper.BaseUri, "../Services/SoaDataGridMessageService.svc").ToString();
    }
  }
}
