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
  public partial class DataGridObservableCollection : UserControl, IDisposable
  {
    ObservableCollection<Person> _people;


    public DataGridObservableCollection()
    {
      InitializeComponent();

      DataGrid1.Loaded += new RoutedEventHandler(DataGrid1_Loaded);
    }

    void DataGrid1_Loaded(object sender, RoutedEventArgs e)
    {
      _people = GeneratePeople(100);

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

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
      _people.Add(Person.Generate());

      DataGrid1.CurrentPageIndex = DataGrid1.PageCount - 1;
    }

    private void RemoveButton_Click(object sender, RoutedEventArgs e)
    {
      while (DataGrid1.SelectedIndices.Count > 0)
      {
        _people.Remove(DataGrid1.LoadedRows[DataGrid1.SelectedIndices[0]].DataContext as Person);
      }
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
      _people.Clear();
    }
  }

  public class Person : Object
  {
    static Random _random = new Random();
    static string[] firstNames = { "Josef", "Benjamin", "Karl", "Milos", "Steve", "Patrick", "Jorge", "Walter", "Eric", "Bob", "Bill", "Greg", "Richard", "Jacob", "Kurt" };
    static string[] lastNames = { "Borges", "Kafka", "Beckett", "Joyce", "Marquez", "Eco", "Pynchon", "Rollins", "O'Brian", "Smith", "Johnson", "Bronowski", "Murakami" };
    static string[] occupations = { "Writer", "Theorist", "Scholar", "Academic", "Architect", "Designer", "Developer", "Manager" };

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public string Occupation { get; set; }
    public bool IsActive { get; set; }

    public static Person Generate()
    {
      return new Person()
      {
        FirstName = firstNames[_random.Next(firstNames.Length)],
        LastName = lastNames[_random.Next(lastNames.Length)],
        Occupation = occupations[_random.Next(occupations.Length)],
        Birthday = DateTime.Today.AddYears(-20).AddDays(-1 * _random.Next(365 * 50)),
        IsActive = _random.Next(2) == 0
      };
    }
  }
}
