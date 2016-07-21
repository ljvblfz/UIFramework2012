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
using ComponentArt.Win.UI.Navigation;

namespace ComponentArt.Win.Demos
{
    public partial class MenuTemplatedItems : UserControl, IDisposable
    {
        public MenuTemplatedItems()
        {
            InitializeComponent();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void muDemo_Initialized(object sender, RoutedEventArgs e)
        {
            
        }

        void muDemo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }


        #region IDisposable Members

        public void Dispose()
        {
            muDemo.Dispose();
        }

        #endregion
    }
}
