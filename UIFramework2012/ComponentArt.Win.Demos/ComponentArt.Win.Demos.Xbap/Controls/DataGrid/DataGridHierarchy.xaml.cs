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
  public partial class DataGridHierarchy : UserControl, IDisposable
  {
    public DataGridHierarchy()
    {
      InitializeComponent();

      LoadCategories();
    }

    public void LoadCategories()
    {
      Uri service = new Uri(DemoWebHelper.BaseUri, "../Services/WCFProductService.svc");

      ProductService.WCFProductServiceClient oSoapClient = new ProductService.WCFProductServiceClient("BasicHttpBinding_WCFProductService", service.AbsoluteUri);

      oSoapClient.GetCategoriesCompleted += oSoapClient_GetCategoriesCompleted;
      oSoapClient.GetCategoriesAsync();
    }

    void oSoapClient_GetCategoriesCompleted(object sender, ProductService.GetCategoriesCompletedEventArgs e)
    {
      DataGrid1.DataContext = e.Result;
    }

    public void LoadProducts(DataGrid grid, int categoryId)
    {
      Uri service = new Uri(DemoWebHelper.BaseUri, "../Services/WCFProductService.svc");

      ProductService.WCFProductServiceClient oSoapClient = new ProductService.WCFProductServiceClient("BasicHttpBinding_WCFProductService", service.AbsoluteUri);

      oSoapClient.GetProductsCompleted += oSoapClient_GetProductCompleted;
      oSoapClient.GetProductsAsync(categoryId, grid);
    }

    void oSoapClient_GetProductCompleted(object sender, ProductService.GetProductsCompletedEventArgs e)
    {
      DataGrid grid = (DataGrid)e.UserState;

      grid.ItemsSource = e.Result;
    }

    private void CustomGrid2_Loaded(object sender, RoutedEventArgs e)
    {
      DataGrid grid = (DataGrid)sender;

      ProductService.Category oCat = (ProductService.Category)grid.DataContext;

      LoadProducts(grid, oCat.Id);
    }

    public void LoadOrders(DataGrid grid, int productId)
    {
      Uri service = new Uri(DemoWebHelper.BaseUri, "../Services/WCFProductService.svc");

      ProductService.WCFProductServiceClient oSoapClient = new ProductService.WCFProductServiceClient("BasicHttpBinding_WCFProductService", service.AbsoluteUri);

      oSoapClient.GetOrdersCompleted += oSoapClient_GetOrdersCompleted;
      oSoapClient.GetOrdersAsync(productId, grid);
    }

    void oSoapClient_GetOrdersCompleted(object sender, ProductService.GetOrdersCompletedEventArgs e)
    {
      DataGrid grid = (DataGrid)e.UserState;

      grid.ItemsSource = e.Result;
    }

    private void CustomGrid3_Loaded(object sender, RoutedEventArgs e)
    {
      DataGrid grid = (DataGrid)sender;

      ProductService.Product oProd = (ProductService.Product)grid.DataContext;

      LoadOrders(grid, oProd.Id);
    }

    public void Dispose()
    {
      DataGrid1.Dispose();
    }
  }
}
