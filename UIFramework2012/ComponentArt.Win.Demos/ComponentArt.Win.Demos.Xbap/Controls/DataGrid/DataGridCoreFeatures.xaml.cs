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
  public partial class DataGridCoreFeatures : UserControl, IDisposable
  {
    public DataGridCoreFeatures()
    {
      InitializeComponent();

      DataGrid1.SoaServiceUrl = new Uri(DemoWebHelper.BaseUri, "../Services/SoaDataGridFeedService.svc").ToString();
    }

    public void Dispose()
    {
      DataGrid1.Dispose();
    }

    private void ShowFilterCheckBox_Click(object sender, RoutedEventArgs e)
    {
      DataGrid1.ShowColumnFilters = ((sender as CheckBox).IsChecked ?? false);
    }

    private void ShowSearchCheckBox_Click(object sender, RoutedEventArgs e)
    {
      DataGrid1.SearchBoxVisibility = ((sender as CheckBox).IsChecked ?? false) ? Visibility.Visible : Visibility.Collapsed;
    }

    private void PagingCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (DataGrid1 != null)
      {
        if ((sender as ComboBox).SelectedIndex == 0)
        {
          DataGrid1.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
          DataGrid1.FooterVisibility = Visibility.Visible;
          DataGrid1.PageSize = 14;
          DataGrid1.RecordOffset = -1;
          DataGrid1.CurrentPageIndex = 0;
        }
        else
        {
          DataGrid1.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
          DataGrid1.FooterVisibility = Visibility.Collapsed;
          DataGrid1.PageSize = 16;
          DataGrid1.CurrentPageIndex = 0;
          DataGrid1.RecordOffset = -1;
        }

        if (DataGrid1.RunningMode == ComponentArt.Win.UI.Data.DataGridRunningMode.Callback)
        {
          DataGrid1.Reload();
        }
        else
        {
          DataGrid1.Refresh();
        }
      }
    }

    private void Menu_ItemCheckedChanged(object sender, ComponentArt.Win.UI.Navigation.MenuCommandEventArgs mce)
    {
      switch(mce.ItemSource.Tag as string)
      {
        case "AllowSorting":
          DataGrid1.Columns[mce.ItemSource.GetParentItem().Tag as string].AllowSorting = mce.ItemSource.IsChecked;
          break;
        case "AllowGrouping":
          DataGrid1.Columns[mce.ItemSource.GetParentItem().Tag as string].AllowGrouping = mce.ItemSource.IsChecked;
          break;
        case "AllowFiltering":
          DataGrid1.Columns[mce.ItemSource.GetParentItem().Tag as string].ShowFilter = mce.ItemSource.IsChecked;
          break;
        case "AllowResizing":
          DataGrid1.Columns[mce.ItemSource.GetParentItem().Tag as string].AllowResizing = mce.ItemSource.IsChecked;
          break;
      }
    }

    private void RunningModeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (DataGrid1 != null)
      {
        if ((sender as ComboBox).SelectedIndex == 0)
        {
          DataGrid1.RunningMode = ComponentArt.Win.UI.Data.DataGridRunningMode.Callback;
          DataGrid1.SoaRequestTag = null;
        }
        else
        {
          DataGrid1.RunningMode = ComponentArt.Win.UI.Data.DataGridRunningMode.Client;
          DataGrid1.SoaRequestTag = "LoadEverything";
        }

        DataGrid1.RecordOffset = -1;
        DataGrid1.CurrentPageIndex = 0;
        DataGrid1.Reload();
      }
    }
  }

  public class RatingToImageSourceConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      int rating = (int)Math.Min(5, Math.Round(value is double? (double)value : 0.0));

      string sPath = @"pack://application:,,,/Assets/DataGrid/FeedBrowser/Rating/" + rating + ".png";
      
      return new ImageSourceConverter().ConvertFromString(sPath);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return 0;
    }
  }
}
