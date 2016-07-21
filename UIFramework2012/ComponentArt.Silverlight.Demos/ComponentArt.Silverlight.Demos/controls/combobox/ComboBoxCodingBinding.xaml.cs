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
using System.Text;

namespace ComponentArt.Silverlight.Demos {

    public partial class ComboBoxCodingBinding : UserControl, IDisposable
    {
        ObservableCollection<string> items_source;

        public ComboBoxCodingBinding()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ComboBoxCoreFeatures_Loaded);
        }

        void ComboBoxCoreFeatures_Loaded(object sender, RoutedEventArgs e)
        {
            items_source = new ObservableCollection<string>();
            string[] items = new string[] { "apple", "banana", "cherry", "fig", "grape", "lemon", "mango",
                "orange", "pineaple", "quince", "strawberry", "tangerine", "ugli", "watermelon" };
            foreach (string s in items)
            {
                items_source.Add(s);
            }
            MyComboBox.ItemsSource = items_source;
        }

        #region IDisposable Members
        void IDisposable.Dispose()
        {
            MyComboBox.Dispose();
        }
        #endregion

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            string toadd = RandomString(12, true);
            items_source.Add(toadd);
        }

        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (items_source.Count > 0)
            {
                items_source.RemoveAt(0);
            }
        }

    }
}
