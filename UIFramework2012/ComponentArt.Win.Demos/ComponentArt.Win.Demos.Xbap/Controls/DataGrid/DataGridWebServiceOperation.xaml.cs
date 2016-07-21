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
  public partial class DataGridWebServiceOperation : UserControl, IDisposable
  {
    public DataGridWebServiceOperation()
    {
      InitializeComponent();

      LoadData();
    }

    public void LoadData()
    {
      DataGrid1.ShowLoading();

      Uri service = new Uri(DemoWebHelper.BaseUri, "../Services/WCFMessageService.svc");

      MessageService.WCFMessageServiceClient oSoapClient = new MessageService.WCFMessageServiceClient("BasicHttpBinding_WCFMessageService", service.AbsoluteUri);

      oSoapClient.GetRecordsCompleted += oSoapClient_GetRecordsCompleted;
      oSoapClient.GetRecordsAsync(DataGrid1.PageSize, DataGrid1.CurrentPageIndex, DataGrid1.SortExpression);
    }

    void oSoapClient_GetRecordsCompleted(object sender, MessageService.GetRecordsCompletedEventArgs e)
    {
      DataGrid1.RecordCount = 447;
      DataGrid1.ItemsSource = e.Result;
    }

    private void DataGrid1_SortingChanged(object sender, DataGridSortingChangedEventArgs e)
    {
      LoadData();
    }

    private void DataGrid1_PageIndexChanged(object sender, DataGridPageIndexChangedEventArgs e)
    {
      LoadData();
    }

    public void Dispose()
    {
      DataGrid1.Dispose();
    }
  }
}
