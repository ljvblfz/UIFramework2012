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
//using System.Windows.Browser;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Text;

namespace ComponentArt.Win.Demos
{

    public partial class ComboBoxCodingCsv : UserControl, IDisposable
    {
        public ComboBoxCodingCsv()
        {
            InitializeComponent();
        }
        #region IDisposable Members

        void IDisposable.Dispose()
        {
        }

        #endregion

        private void CreateFromCSV_Click(object sender, RoutedEventArgs e)
        {
            string[] csv = CSVValues.Text.Split(new char[] { ',' });
            CreateTarget.Children.Add(new ComponentArt.Win.UI.Input.ComboBox(csv));
        }
    }
}
