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
using ComponentArt.Silverlight.UI.Data;

namespace ComponentArt.Silverlight.Demos
{
  public partial class DataGridHierarchyHighlight : UserControl, IDisposable
  {
    public DataGridHierarchyHighlight()
    {
      InitializeComponent();

      LoadCategories();
    }

    public void LoadCategories()
    {
      Uri service = new Uri(Application.Current.Host.Source, "../WCFCarService.svc");

      WCFCarService.WCFCarServiceClient oSoapClient = new WCFCarService.WCFCarServiceClient("BasicHttpBinding_WCFCarService", service.AbsoluteUri);

      oSoapClient.GetCategoriesCompleted += oSoapClient_GetCategoriesCompleted;
      oSoapClient.GetCategoriesAsync();
    }

    void oSoapClient_GetCategoriesCompleted(object sender, WCFCarService.GetCategoriesCompletedEventArgs e)
    {
      DataGrid1.DataContext = e.Result;
    }

    public void LoadModels(DataGrid grid, int categoryId)
    {
      Uri service = new Uri(Application.Current.Host.Source, "../WCFCarService.svc");

      WCFCarService.WCFCarServiceClient oSoapClient = new WCFCarService.WCFCarServiceClient("BasicHttpBinding_WCFCarService", service.AbsoluteUri);

      oSoapClient.GetModelsCompleted += oSoapClient_GetModelsCompleted;
      oSoapClient.GetModelsAsync(categoryId, grid);
    }

    void oSoapClient_GetModelsCompleted(object sender, WCFCarService.GetModelsCompletedEventArgs e)
    {
      DataGrid grid = (DataGrid)e.UserState;

      grid.ItemsSource = e.Result;
    }

    private void CustomGrid2_Loaded(object sender, RoutedEventArgs e)
    {
      DataGrid grid = (DataGrid)sender;

      WCFCarService.CarCategory oCat = (WCFCarService.CarCategory)grid.DataContext;

      LoadModels(grid, oCat.Id);
    }

    public void LoadSales(DataGrid grid, int modelId)
    {
      Uri service = new Uri(Application.Current.Host.Source, "../WCFCarService.svc");

      WCFCarService.WCFCarServiceClient oSoapClient = new WCFCarService.WCFCarServiceClient("BasicHttpBinding_WCFCarService", service.AbsoluteUri);

      oSoapClient.GetSalesCompleted += oSoapClient_GetSalesCompleted;
      oSoapClient.GetSalesAsync(modelId, grid);
    }

    void oSoapClient_GetSalesCompleted(object sender, WCFCarService.GetSalesCompletedEventArgs e)
    {
      DataGrid grid = (DataGrid)e.UserState;

      grid.ItemsSource = e.Result;
    }

    private void CustomGrid3_Loaded(object sender, RoutedEventArgs e)
    {
      DataGrid grid = (DataGrid)sender;

      WCFCarService.CarModel oProd = (WCFCarService.CarModel)grid.DataContext;

      LoadSales(grid, oProd.Id);
    }

    public void Dispose()
    {
      DataGrid1.Dispose();
    }
  }

  public class CarModelToImageSourceConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string sModel = (string)value;

      string sPath = "cars/" + sModel.ToLower().Replace(' ', '_').Replace(".", "") + ".jpg";

      ImageSourceConverter conv = new ImageSourceConverter();
      return conv.ConvertFromString(sPath);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return "";
    }
  }
}
