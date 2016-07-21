using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
  public partial class DataGridSoaEditing : UserControl
  {
    public DataGridSoaEditing()
    {
      InitializeComponent();
    }

    private string ValuesToString(object[] values)
    {
      List<string> strings = new List<string>();
      foreach (object value in values)
      {
        strings.Add(value == null ? "" : value.ToString());
      }

      return string.Join(",", strings.ToArray());
    }

    private void DataGrid1_BeforeSoaRequestSent(object sender, DataGridSoaRequestCancelEventArgs e)
    {
      if (e.Request is SoaDataGridUpdateRequest)
      {
        EventListBox.Items.Add("Issuing SOA.UI update request for row (" + ValuesToString((e.Request as SoaDataGridUpdateRequest).Values.ToArray()) + ")");
      }
      else if (e.Request is SoaDataGridDeleteRequest)
      {
        EventListBox.Items.Add("Issuing SOA.UI delete request for row (" + ValuesToString((e.Request as SoaDataGridDeleteRequest).Values.ToArray()) + ")");
      }
      else if (e.Request is SoaDataGridInsertRequest)
      {
        EventListBox.Items.Add("Issuing SOA.UI insert request for row (" + ValuesToString((e.Request as SoaDataGridInsertRequest).Values.ToArray()) + ")");
      }

      if (EventListBox.Items.Count > 0)
      {
        EventListBox.ScrollIntoView(EventListBox.Items[EventListBox.Items.Count - 1]);
      }
    }

    private void DataGrid1_BeforeSoaResponseProcessed(object sender, DataGridSoaResponseCancelEventArgs e)
    {
      if (e.Response is SoaDataGridUpdateResponse)
      {
        EventListBox.Items.Add("Received SOA.UI update response: " + (e.Response as SoaDataGridUpdateResponse).Message);
      }
      else if (e.Response is SoaDataGridDeleteResponse)
      {
        EventListBox.Items.Add("Received SOA.UI delete response: " + (e.Response as SoaDataGridDeleteResponse).Message);
      }
      else if (e.Response is SoaDataGridInsertResponse)
      {
        EventListBox.Items.Add("Received SOA.UI insert response: " + (e.Response as SoaDataGridInsertResponse).Message);
      }

      if (EventListBox.Items.Count > 0)
      {
        EventListBox.ScrollIntoView(EventListBox.Items[EventListBox.Items.Count - 1]);
      }
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
