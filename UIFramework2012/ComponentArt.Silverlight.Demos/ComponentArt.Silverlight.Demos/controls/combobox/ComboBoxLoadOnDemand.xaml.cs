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
using ComponentArt.Silverlight.UI.Input;
using ComponentArt.Silverlight.UI.Utils;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Windows.Browser;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Collections.ObjectModel;
using ComponentArt.Silverlight.Demos.ManualComboBoxLocationsService;

namespace ComponentArt.Silverlight.Demos {
    public partial class ComboBoxLoadOnDemand : UserControl, IDisposable
    {
        #region IDisposable Members
        public void Dispose()
        {
            MyComboBox.Dispose();
        }
        #endregion

        public ComboBoxLoadOnDemand()
        {
            itemsSource = new ObservableCollection<Location>();
            InitializeComponent();
            Uri service = new Uri(Application.Current.Host.Source, "../ManualComboBoxLocationsService.svc");
            client = new ManualComboBoxLocationsService.ManualComboBoxLocationsServiceClient
                  ("BasicHttpBinding_ManualComboBoxLocationsService", service.AbsoluteUri);

            Loaded += new RoutedEventHandler(ComboBoxLoadOnDemand_Loaded);
        }

        ManualComboBoxLocationsServiceClient client;
        ObservableCollection<Location> itemsSource;
        string currentFilter;
        int currentIndex = 0;
        int numberOfRecordsPerPage = 10;
        bool loadInProgress = false;
        ComboBoxPagingDirection currentDirection;

        void ComboBoxLoadOnDemand_Loaded(object sender, RoutedEventArgs e)
        {
            currentFilter = "";
            client.GetMatchesForArgumentCompleted -= new EventHandler<GetMatchesForArgumentCompletedEventArgs>(client_GetMatchesForArgumentCompleted);
            client.GetMatchesForArgumentCompleted += new EventHandler<GetMatchesForArgumentCompletedEventArgs>(client_GetMatchesForArgumentCompleted);
            loadInProgress = true;
            client.GetMatchesForArgumentAsync("", "startsWith", 0, numberOfRecordsPerPage);
            MyComboBox.ItemsSource = itemsSource;
        }

        void client_GetMatchesForArgumentCompleted(object sender, GetMatchesForArgumentCompletedEventArgs e)
        {
            client.GetMatchesForArgumentCompleted -= new EventHandler<GetMatchesForArgumentCompletedEventArgs>(client_GetMatchesForArgumentCompleted);
            itemsSource.Clear();
            if (e.Result == null)
                return;
            foreach (string s in e.Result)
            {
                itemsSource.Add(new Location(s, currentIndex));
                currentIndex++;
            }
            MyComboBox.IsLoadOnDemandCompleted = true;
            loadInProgress = false;
            currentIndex = numberOfRecordsPerPage;
        }

        private void MyComboBox_BeforeFilter(object sender, ComboBoxFilterCancelEventArgs cea)
        {
            if (loadInProgress)
            {
                cea.Cancel = true;
                return;
            }
            currentIndex = 0;
            currentFilter = cea.Filter;
            client.GetMatchesForArgumentCompleted -= new EventHandler<GetMatchesForArgumentCompletedEventArgs>(client_GetMatchesForArgumentCompleted);
            client.GetMatchesForArgumentCompleted += new EventHandler<GetMatchesForArgumentCompletedEventArgs>(client_GetMatchesForArgumentCompleted);
            loadInProgress = true;
            MyComboBox.SelectedItem = null;
            itemsSource.Clear();
            client.GetMatchesForArgumentAsync(cea.Filter, "startsWith", 0, numberOfRecordsPerPage);
            MyComboBox.ItemsSource = itemsSource;
        }

        void client_GetMatchesForPagingArgumentCompleted(object sender, GetMatchesForArgumentCompletedEventArgs e)
        {
            client.GetMatchesForArgumentCompleted -= new EventHandler<GetMatchesForArgumentCompletedEventArgs>(client_GetMatchesForPagingArgumentCompleted);
            if (e.Result == null)
            {
                return;
            }

            if (itemsSource.Count + e.Result.Count > MyComboBox.MaxItemsDuringPaging)
            {
                int i = (itemsSource.Count + e.Result.Count) - MyComboBox.MaxItemsDuringPaging;

                if (currentDirection == ComboBoxPagingDirection.Down)
                {
                    for (--i; i >= 0; --i)
                    {
                        itemsSource.RemoveAt(0);
                    }
                }
                else
                {
                    for (--i; i >= 0; i--)
                    {
                        itemsSource.RemoveAt(itemsSource.Count - 1);
                    }
                }
            }
            if (currentDirection == ComboBoxPagingDirection.Down)
            {
                foreach (string s in e.Result)
                {
                    itemsSource.Add(new Location(s, currentIndex));
                    currentIndex++;
                }
            }
            else
            {
                for (int i = e.Result.Count - 1; i > -1 && currentIndex > -1; i--)
                {
                    currentIndex--;
                    itemsSource.Insert(0, new Location(e.Result[i], currentIndex));
                }
            }
            MyComboBox.IsLoadOnDemandCompleted = true;
            loadInProgress = false;
        }

        private void MyComboBox_FilterPaging(object sender, ComboBoxPagingCancelEventArgs pea)
        {
            currentFilter = pea.Filter;
            client.GetMatchesForArgumentCompleted -= new EventHandler<GetMatchesForArgumentCompletedEventArgs>(client_GetMatchesForPagingArgumentCompleted);
            client.GetMatchesForArgumentCompleted += new EventHandler<GetMatchesForArgumentCompletedEventArgs>(client_GetMatchesForPagingArgumentCompleted);

            MyComboBox.ItemsSource = itemsSource;
            if (MyComboBox.SelectedItem == null)
            {
                client.GetMatchesForArgumentCompleted -= new EventHandler<GetMatchesForArgumentCompletedEventArgs>(client_GetMatchesForPagingArgumentCompleted);
                MyComboBox.IsLoadOnDemandCompleted = true;
                return;
            }
            if (pea.Direction == ComboBoxPagingDirection.Down)
            {
                currentIndex = Convert.ToInt32(MyComboBox.SelectedItem.Id);

                if (currentIndex < 1)
                {
                    cancelLoad();
                }

                currentDirection = ComboBoxPagingDirection.Down;
                loadInProgress = true;
                client.GetMatchesForArgumentAsync(pea.Filter, "startsWith", ++currentIndex, numberOfRecordsPerPage);
            }
            else if (pea.Direction == ComboBoxPagingDirection.Up)
            {
                currentIndex = Convert.ToInt32(MyComboBox.SelectedItem.Id);

                if (currentIndex < 1)
                {
                    cancelLoad();
                }
                int sel = (currentIndex - 1) - numberOfRecordsPerPage > 0 ? currentIndex - numberOfRecordsPerPage : 0;

                currentDirection = ComboBoxPagingDirection.Up;
                loadInProgress = true;
                client.GetMatchesForArgumentAsync(pea.Filter, "startsWith", sel, numberOfRecordsPerPage);
            }
        }

        private void cancelLoad()
        {
            client.GetMatchesForArgumentCompleted -= new EventHandler<GetMatchesForArgumentCompletedEventArgs>(client_GetMatchesForArgumentCompleted);
            client.GetMatchesForArgumentCompleted -= new EventHandler<GetMatchesForArgumentCompletedEventArgs>(client_GetMatchesForPagingArgumentCompleted);
            MyComboBox.IsLoadOnDemandCompleted = true;
            loadInProgress = false;
            return;
        }
    }

    public class Location
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
        private string _tag;
        public string Tag
        {
            get { return _tag; }
            set
            {
                _tag = value;
            }
        }
        public Location(string s)
        {
            Name = s;
        }
        public Location(string s, int i)
        {
            Name = s;
            Id = i;
        }
        public Location(string s, int i, string t)
        {
            Name = s;
            Id = i;
            Tag = t;
        }
    }
}

