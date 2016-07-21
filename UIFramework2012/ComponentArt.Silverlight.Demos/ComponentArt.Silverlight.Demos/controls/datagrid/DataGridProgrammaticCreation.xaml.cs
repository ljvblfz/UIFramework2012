using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ComponentArt.Silverlight.UI.Data;


namespace ComponentArt.Silverlight.Demos
{
  public partial class DataGridProgrammaticCreation : UserControl, IDisposable
  {
    public DataGridProgrammaticCreation()
    {
      InitializeComponent();

      DataGrid1.Loaded += new RoutedEventHandler(DataGrid1_Loaded);
    }

    void DataGrid1_Loaded(object sender, RoutedEventArgs e)
    {
      // Create columns:

      DataGridTextColumn oSubjectColumn = new DataGridTextColumn();
      oSubjectColumn.Binding = new Binding("Subject");
      oSubjectColumn.Width = new GridLength(3, GridUnitType.Star);
      oSubjectColumn.Header = "Subject";
      DataGrid1.Columns.Add(oSubjectColumn);

      DataGridTextColumn oLastPostDateColumn = new DataGridTextColumn();
      oLastPostDateColumn.Binding = new Binding("LastPostDate");
      oLastPostDateColumn.Width = new GridLength(2, GridUnitType.Star);
      oLastPostDateColumn.Header = "Last Post";
      DataGrid1.Columns.Add(oLastPostDateColumn);

      DataGridTextColumn oStartedByColumn = new DataGridTextColumn();
      oStartedByColumn.Binding = new Binding("StartedBy");
      oStartedByColumn.Width = new GridLength(1, GridUnitType.Star);
      oStartedByColumn.Header = "Started By";
      DataGrid1.Columns.Add(oStartedByColumn);

      DataGridTextColumn oRepliesColumn = new DataGridTextColumn();
      oRepliesColumn.Binding = new Binding("Replies");
      oRepliesColumn.Width = new GridLength(1, GridUnitType.Star);
      oRepliesColumn.Header = "Replies";
      DataGrid1.Columns.Add(oRepliesColumn);

      DataGridTextColumn oTotalViewsColumn = new DataGridTextColumn();
      oTotalViewsColumn.Binding = new Binding("TotalViews");
      oTotalViewsColumn.Width = new GridLength(1, GridUnitType.Star);
      oTotalViewsColumn.Header = "Total Views";
      DataGrid1.Columns.Add(oTotalViewsColumn);

      // Load data:

      LoadAllData();
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
      DataGrid1.DataContext = e.Result;
    }

    public void Dispose()
    {
      DataGrid1.Dispose();
    }
  }
}
