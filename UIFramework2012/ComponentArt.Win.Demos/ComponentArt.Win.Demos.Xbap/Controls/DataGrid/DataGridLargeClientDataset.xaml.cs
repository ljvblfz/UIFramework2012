using System;
using System.Collections.ObjectModel;
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
  public partial class DataGridLargeClientDataset : UserControl, IDisposable
  {
    ObservableCollection<Person> _people;


    public DataGridLargeClientDataset()
    {
      InitializeComponent();

      DataGrid1.Loaded += new RoutedEventHandler(DataGrid1_Loaded);
    }

    void DataGrid1_Loaded(object sender, RoutedEventArgs e)
    {
      _people = GeneratePeople(10000);

      DataGrid1.ItemsSource = _people;
    }

    ObservableCollection<Person> GeneratePeople(int howMany)
    {
      ObservableCollection<Person> people = new ObservableCollection<Person>();

      for (int i = 0; i < howMany; i++)
      {
        people.Add(Person.Generate());
      }

      return people;
    }

    public void Dispose()
    {
      DataGrid1.Dispose();
    }
  }  
}
