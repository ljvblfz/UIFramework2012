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


namespace ComponentArt.Silverlight.Demos
{
  public partial class DataGridEditing : UserControl, IDisposable
  {
    public DataGridEditing()
    {
      InitializeComponent();

      LoadAllData();
    }

    private string RowToString(DataGridRow oRow)
    {
      List<string> strings = new List<string>();
      foreach (DataGridCell cell in oRow.Cells)
      {
        strings.Add(cell.Text);
      }

      return string.Join(",", strings.ToArray());
    }

    public void LoadAllData()
    {
      Uri service = new Uri(Application.Current.Host.Source, "../WCFProductService.svc");

      ProductService.WCFProductServiceClient oSoapClient = new ProductService.WCFProductServiceClient("BasicHttpBinding_WCFProductService", service.AbsoluteUri);

      oSoapClient.GetProductsCompleted += oSoapClient_GetProductCompleted;
      oSoapClient.GetProductsAsync(null);
    }

    void oSoapClient_GetProductCompleted(object sender, ProductService.GetProductsCompletedEventArgs e)
    {
      DataGrid1.DataContext = e.Result;
    }

    private void DataGrid1_EditedRow(object sender, DataGridRowEventArgs e)
    {
      EventListBox.Items.Add("Edited row (" + RowToString(e.Row) + ")");

      EventListBox.ScrollIntoView(EventListBox.Items[EventListBox.Items.Count - 1]);
    }

    private void DataGrid1_RowDeleted(object sender, DataGridRowDeletedEventArgs e)
    {
      EventListBox.Items.Add("Deleted row (" + RowToString(e.Row) + ")");

      EventListBox.ScrollIntoView(EventListBox.Items[EventListBox.Items.Count - 1]);
    }

    public void Dispose()
    {
      DataGrid1.Dispose();
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
      DataGridCell oCell = DataGrid1.GetOwningCell(sender as FrameworkElement);

      if (oCell != null)
      {
        oCell.OwningRow.Edit();
      }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      DataGridCell oCell = DataGrid1.GetOwningCell(sender as FrameworkElement);

      if (oCell != null)
      {
        DataGrid1.DeleteRow(oCell.OwningRow);
      }
    }

    private void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
      DataGrid1.EditComplete();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      DataGrid1.EditCancel();
    }

    private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs evargs)
    {
      DataGridCell oCell = DataGrid1.GetOwningCell(sender as DependencyObject);
      oCell.Value = (sender as ComponentArt.Silverlight.UI.Input.ComboBox).SelectedItem.Text;
    }

    private void CategoryComboBox_Initialized(object sender, RoutedEventArgs e)
    {
      DataGridCell oCell = DataGrid1.GetOwningCell(sender as DependencyObject);
      (sender as ComponentArt.Silverlight.UI.Input.ComboBox).SelectItem(oCell.Text);
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
      DataGrid1.AddRow();
    }    
  }
}
