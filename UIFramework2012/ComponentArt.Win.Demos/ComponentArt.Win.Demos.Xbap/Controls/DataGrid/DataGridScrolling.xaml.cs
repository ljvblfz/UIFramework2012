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
using ComponentArt.Win.UI.Data;


namespace ComponentArt.Win.Demos
{
  public partial class DataGridScrolling : UserControl, IDisposable
  {
    public DataGridScrolling()
    {
      InitializeComponent();

      DataGrid1.SortBy(DataGrid1.Columns[5], SortDirection.Ascending);

      LoadAllData();

      OnDemandScrollingButton.IsChecked = true;
    }

    public void LoadAllData()
    {
      Uri service = new Uri(DemoWebHelper.BaseUri, "../Services/WCFMessageService.svc");

      MessageService.WCFMessageServiceClient oSoapClient = new MessageService.WCFMessageServiceClient("BasicHttpBinding_WCFMessageService", service.AbsoluteUri);

      oSoapClient.GetRecordsCompleted += oSoapClient_GetAllRecordsCompleted;
      oSoapClient.GetRecordsAsync(100, 0, DataGrid1.SortExpression);
    }

    void oSoapClient_GetAllRecordsCompleted(object sender, MessageService.GetRecordsCompletedEventArgs e)
    {
      DataGrid1.ItemsSource = e.Result;
    }

    public void Dispose()
    {
      DataGrid1.Dispose();
    }

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
      RadioButton oButton = (RadioButton)sender;

      if (oButton.Tag.ToString() == "OnDemand")
      {
        DataGrid1.VerticalScrollMode = ComponentArt.Win.UI.Data.DataGridVerticalScrollMode.OnDemand;
        DataGrid1.PageSize = 18;
      }
      else
      {
        DataGrid1.VerticalScrollMode = ComponentArt.Win.UI.Data.DataGridVerticalScrollMode.Live;
        DataGrid1.PageSize = int.MaxValue;
        DataGrid1.RecordOffset = 0;
      }

      DataGrid1.ScrollTo(0);
      DataGrid1.Refresh();
    }
  }
}
