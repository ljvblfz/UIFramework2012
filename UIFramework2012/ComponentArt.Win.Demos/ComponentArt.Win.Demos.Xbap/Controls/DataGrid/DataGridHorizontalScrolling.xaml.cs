﻿using System;
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
  public partial class DataGridHorizontalScrolling : UserControl, IDisposable
  {
    public DataGridHorizontalScrolling()
    {
      InitializeComponent();

      DataGrid1.SortBy(DataGrid1.Columns[5], SortDirection.Ascending);

      LoadAllData();
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
  }
}
