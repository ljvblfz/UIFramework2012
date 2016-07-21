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
using ComponentArt.Win.UI.Input;
using ComponentArt.Win.UI.Navigation;

namespace ComponentArt.Win.Demos
{
	public partial class Office2007 : UserControl
  {
		ObservableCollection<Person> _people;

        public Office2007()
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


		private void ThemeButton_Click(object sender,RoutedEventArgs e) {
			((App) Application.Current).LoadTheme(sender,e);
		}
	}
}
