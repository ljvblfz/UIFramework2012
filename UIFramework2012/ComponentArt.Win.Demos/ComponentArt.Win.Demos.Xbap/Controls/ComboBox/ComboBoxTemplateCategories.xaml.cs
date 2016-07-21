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

namespace ComponentArt.Win.Demos
{

    public partial class ComboBoxTemplateCategories : UserControl, IDisposable
    {
        public ComboBoxTemplateCategories()
        {
            InitializeComponent();
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            MyComboBox.Dispose();
            MyComboBox2.Dispose();
        }

        #endregion

        private void MyComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs evargs)
        {
            foreach (object cbi in evargs.AddedItems)
            {
                if (evargs.AddedItems.Count > 0)
                {
                    Debug.WriteLine("SelectionChanged[Added]:" +
                        ((ComponentArt.Win.UI.Input.ComboBoxItem)evargs.AddedItems[0]).Text);
                    MyComboBox2.FontFamily = new FontFamily(((ComponentArt.Win.UI.Input.ComboBoxItem)evargs.AddedItems[0]).Text);
                }
            }
        }
    }
}
