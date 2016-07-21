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

namespace ComponentArt.Silverlight.Demos
{
  public class GroupTotalViewsConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      ComponentArt.Silverlight.UI.Data.DataGridGroupDataContext dggdc = (ComponentArt.Silverlight.UI.Data.DataGridGroupDataContext)value;
      List<ComponentArt.Silverlight.UI.Data.DataGridRow> myRows = (List<ComponentArt.Silverlight.UI.Data.DataGridRow>)dggdc.Group.Rows;
      int total = 0;
      foreach (ComponentArt.Silverlight.UI.Data.DataGridRow myRow in myRows)
      {
        total += int.Parse(myRow.Cells["TotalViews"].Value.ToString());
      }
      return total;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return "[ConvertBack]";
    }
  }

  public partial class DataGridGroupTotals : UserControl, IDisposable
  {
    public DataGridGroupTotals()
    {
      InitializeComponent();

      DataGrid1.GroupBy(DataGrid1.Columns["StartedBy"], ComponentArt.Silverlight.UI.Data.SortDirection.Ascending);

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
