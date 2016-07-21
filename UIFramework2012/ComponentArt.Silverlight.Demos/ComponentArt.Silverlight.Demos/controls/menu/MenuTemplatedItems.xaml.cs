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
using ComponentArt.Silverlight.UI.Navigation;

namespace ComponentArt.Silverlight.Demos
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
        private void myCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void myCalendar_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            ((Calendar)sender).DisplayMode = CalendarMode.Month;
        }

        #region IDisposable Members

        public void Dispose()
        {
            muDemo.Dispose();
        }

        #endregion
    }
}
