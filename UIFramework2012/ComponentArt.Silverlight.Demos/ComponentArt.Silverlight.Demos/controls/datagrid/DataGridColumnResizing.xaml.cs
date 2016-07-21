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

namespace ComponentArt.Silverlight.Demos
{
  public partial class DataGridColumnResizing : UserControl, IDisposable
  {
    public DataGridColumnResizing()
    {
      InitializeComponent();

      LoadAllData();

      DistributiveSizingButton.IsChecked = true;
    }

    public void LoadAllData()
    {
      Uri service = new Uri(Application.Current.Host.Source, "../WCFMessageService.svc");

      MessageService.WCFMessageServiceClient oSoapClient = new MessageService.WCFMessageServiceClient("BasicHttpBinding_WCFMessageService", service.AbsoluteUri);

      oSoapClient.GetRecordsCompleted += oSoapClient_GetAllRecordsCompleted;
      oSoapClient.GetRecordsAsync(int.MaxValue, 0, DataGrid1.SortExpression);
    }

    void oSoapClient_GetAllRecordsCompleted(object sender, MessageService.GetRecordsCompletedEventArgs e)
    {
      DataGrid1.ItemsSource = e.Result;
    }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
      RadioButton oButton = (RadioButton)sender;

      if (oButton.Tag.ToString() == "Default")
      {
        DataGrid1.ColumnResizingMode = ComponentArt.Silverlight.UI.Data.DataGridColumnResizingMode.Normal;
        DataGrid1.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
      }
      else if (DataGrid1.ColumnResizingMode == ComponentArt.Silverlight.UI.Data.DataGridColumnResizingMode.Normal)
      {
        DataGrid1.ColumnResizingMode = ComponentArt.Silverlight.UI.Data.DataGridColumnResizingMode.Distribute;
        DataGrid1.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;

        // reset column widths
        DataGrid1.SetColumnWidth(DataGrid1.Columns[3], 310);
        DataGrid1.SetColumnWidth(DataGrid1.Columns[4], 150);
        DataGrid1.SetColumnWidth(DataGrid1.Columns[5], 80);
        DataGrid1.SetColumnWidth(DataGrid1.Columns[6], 80);
        DataGrid1.SetColumnWidth(DataGrid1.Columns[7], 80);
      }
    }

    public void Dispose()
    {
      DataGrid1.Dispose();
    }
  }
}
