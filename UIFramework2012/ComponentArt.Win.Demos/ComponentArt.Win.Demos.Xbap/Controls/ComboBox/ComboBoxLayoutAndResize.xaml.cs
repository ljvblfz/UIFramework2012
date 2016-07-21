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
using ComponentArt.Win.UI.Input;
using ComponentArt.Win.UI.Utils;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Globalization;
// using System.Windows.Browser;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Text;

namespace ComponentArt.Win.Demos {

    public partial class ComboBoxLayoutAndResize : UserControl, IDisposable
    {

        public ComboBoxLayoutAndResize()
        {
            InitializeComponent();
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            MyComboBox.Dispose();
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void Contains_Checked(object sender, RoutedEventArgs e)
        {
            if (MyComboBox == null) { return; }
            MyComboBox.FilteringType = ComboBoxFilteringTypeMode.Contains;
        }

        private void StartsWith_Checked(object sender, RoutedEventArgs e)
        {
            if (MyComboBox == null) { return; }
            MyComboBox.FilteringType = ComboBoxFilteringTypeMode.StartsWith;
        }

        private void ComparisonType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyComboBox == null) { return; }
            StringComparison cs = StringComparison.CurrentCultureIgnoreCase;
            switch (((ListBoxItem)e.AddedItems[0]).Content.ToString())
            {
                case "CurrentCulture":
                    cs = StringComparison.CurrentCulture;
                    break;
                case "CurrentCultureIgnoreCase":
                    cs = StringComparison.CurrentCultureIgnoreCase;
                    break;
                case "InvariantCulture":
                    cs = StringComparison.InvariantCulture;
                    break;
                case "InvariantCultureIgnoreCase":
                    cs = StringComparison.InvariantCultureIgnoreCase;
                    break;
                case "Ordinal":
                    cs = StringComparison.Ordinal;
                    break;
                case "OrdinalIgnoreCase":
                    cs = StringComparison.OrdinalIgnoreCase;
                    break;
            }
            MyComboBox.StringComparisonType = cs;
        }

        private void AutoFilter_Checked(object sender, RoutedEventArgs e)
        {
            if (MyComboBox == null) { return; }
            MyComboBox.AutoFilter = true;
        }

        private void AutoFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            if (MyComboBox == null) { return; }
            MyComboBox.AutoFilter = false;
        }

        private void AutoComplete_Checked(object sender, RoutedEventArgs e)
        {
            if (MyComboBox == null) { return; }
            MyComboBox.AutoComplete = true;
        }

        private void AutoComplete_Unchecked(object sender, RoutedEventArgs e)
        {
            if (MyComboBox == null) { return; }
            MyComboBox.AutoComplete = false;
        }

        private void AutoHighlight_Checked(object sender, RoutedEventArgs e)
        {
            if (MyComboBox == null) { return; }
            MyComboBox.AutoHighlight = true;
        }

        private void AutoHighlight_Unchecked(object sender, RoutedEventArgs e)
        {
            if (MyComboBox == null) { return; }
            MyComboBox.AutoHighlight = false;
        }

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs evargs)
        {
            foreach (object cbi in evargs.AddedItems)
            {
                if (evargs.AddedItems.Count > 0)
                {
                    Debug.WriteLine("SelectionChanged[Added]:" +
                        ((ComponentArt.Win.UI.Input.ComboBoxItem)evargs.AddedItems[0]).Text);
                }
                if (evargs.RemovedItems.Count > 0)
                {
                    Debug.WriteLine("SelectionChanged[Removed]:" +
                        ((ComponentArt.Win.UI.Input.ComboBoxItem)evargs.RemovedItems[0]).Text);
                }
            }
        }

        private void IsReadOnly_Checked(object sender, RoutedEventArgs e)
        {
            if (MyComboBox == null) { return; }
            MyComboBox.IsReadOnly = true;
        }

        private void IsReadOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            if (MyComboBox == null) { return; }
            MyComboBox.IsReadOnly = false;
        }
    }
}
