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
using ComponentArt.Silverlight.UI.Data;
using ComponentArt.SOA.UI;


namespace ComponentArt.Silverlight.Demos
{
  public partial class DataGridSoaCustom : UserControl
  {
    private SoaDataGridMessageService.SoaDataGridServiceClient _soapClient;


    public DataGridSoaCustom()
    {
      InitializeComponent();
      InitializeSoapClient();

      DataGridNeedDataCancelEventArgs initialArgs = new DataGridNeedDataCancelEventArgs(DataGrid1);
      _soapClient.SelectAsync(initialArgs.CreateSoaRequest());
    }

    public void InitializeSoapClient()
    {
      Uri service = new Uri(Application.Current.Host.Source, "../SoaDataGridMessageService.svc");

      _soapClient = new SoaDataGridMessageService.SoaDataGridServiceClient("BasicHttpBinding_ISoaDataGridService", service.AbsoluteUri);
      _soapClient.GroupCompleted += _soapClient_GroupCompleted;
      _soapClient.SelectCompleted += _soapClient_SelectCompleted;
    }

    void _soapClient_GroupCompleted(object sender, SoaDataGridMessageService.GroupCompletedEventArgs e)
    {
      SoaDataGridGroupResponse response = (SoaDataGridGroupResponse)e.Result;
      DataGrid1.ProcessSoaResponse(response);
    }

    void _soapClient_SelectCompleted(object sender, SoaDataGridMessageService.SelectCompletedEventArgs e)
    {
      SoaDataGridSelectResponse response = (SoaDataGridSelectResponse)e.Result;
      DataGrid1.ProcessSoaResponse(response);
    }

    private void DataGrid1_NeedData(object sender, DataGridNeedDataCancelEventArgs e)
    {
      _soapClient.SelectAsync(e.CreateSoaRequest());
    }

    private void DataGrid1_NeedGroups(object sender, DataGridNeedGroupsCancelEventArgs e)
    {
      _soapClient.GroupAsync(e.CreateSoaRequest());
    }
  }
}
