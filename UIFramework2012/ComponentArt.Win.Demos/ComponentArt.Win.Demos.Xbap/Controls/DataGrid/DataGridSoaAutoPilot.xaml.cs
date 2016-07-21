using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace ComponentArt.Win.Demos
{
  public partial class DataGridSoaAutoPilot : UserControl
  {
    public DataGridSoaAutoPilot()
    {
      InitializeComponent();

      DataGrid1.SoaServiceUrl = new Uri(DemoWebHelper.BaseUri, "../Services/SoaDataGridMessageService.svc").ToString();
    }
  }

  public class MessageServiceImageSourceConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string sFile = (string)value;
      string sPath = new Uri(DemoWebHelper.BaseUri, "../ClientBin/Controls/DataGrid/" + sFile).ToString();

      return new ImageSourceConverter().ConvertFromString(sPath);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return 0;
    }
  }
}
