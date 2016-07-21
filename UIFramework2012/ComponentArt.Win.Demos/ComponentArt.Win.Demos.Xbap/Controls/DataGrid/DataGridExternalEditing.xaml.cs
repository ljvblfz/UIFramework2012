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
using ComponentArt.Win.UI.Data;


namespace ComponentArt.Win.Demos
{
  public partial class DataGridExternalEditing : UserControl, IDisposable
  {
    public DataGridExternalEditing()
    {
      InitializeComponent();

      LoadAllData();
    }

    public void LoadAllData()
    {
      Uri service = new Uri(DemoWebHelper.BaseUri, "../Services/WCFProductService.svc");

      ProductService.WCFProductServiceClient oSoapClient = new ProductService.WCFProductServiceClient("BasicHttpBinding_WCFProductService", service.AbsoluteUri);

      oSoapClient.GetProductsCompleted += oSoapClient_GetProductCompleted;
      oSoapClient.GetProductsAsync(null);
    }

    void oSoapClient_GetProductCompleted(object sender, ProductService.GetProductsCompletedEventArgs e)
    {
      DataGrid1.DataContext = e.Result;
    }

    public void Dispose()
    {
      DataGrid1.Dispose();
    }

    private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ComboBoxItem item = (ComboBoxItem)(sender as ComboBox).SelectedItem;

      if (item != null)
      {
        (EditForm.DataContext as ProductService.Product).CategoryName = Convert.ToString(item.Content);
        (EditForm.DataContext as ProductService.Product).CategoryId = Convert.ToInt32(item.Tag);
      }
    }
    
    private void DataGrid1_SelectionChanged(object sender, DataGridSelectionChangedEventArgs e)
    {
      EditForm.DataContext = e.AddedItems[0].DataContext;
      CategoryComboBox.SelectedIndex = (e.AddedItems[0].DataContext as ProductService.Product).CategoryId - 1;
    }

    private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      BindingExpression bExp = (sender as ComponentArt.Win.UI.Input.InputTextBox).GetBindingExpression(ComponentArt.Win.UI.Input.InputTextBox.TextProperty);
      if (bExp != null)
      {
        bExp.UpdateSource();
      }
    }
  }
}
