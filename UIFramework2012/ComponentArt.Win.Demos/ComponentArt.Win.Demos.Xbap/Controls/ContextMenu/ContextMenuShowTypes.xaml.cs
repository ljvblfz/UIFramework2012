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
using System.ComponentModel;

namespace ComponentArt.Win.Demos
{
    public partial class ContextMenuShowTypes : UserControl, IDisposable
    {
        public ContextMenuShowTypes()
        {
            InitializeComponent();
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            muDemo.Dispose();
        }

        #endregion

        private void muDemo_Initialized(object sender, RoutedEventArgs e)
        {
            nsExpandDuration.Value = muDemo.ExpandDuration;
            cbExpandSlide.SelectedIndex = 1;
            muDemo.PropertyChanged += new PropertyChangedEventHandler(muDemo_PropertyChanged);
            muDemo.MenuClick += new ComponentArt.Win.UI.Navigation.Menu.MenuClickEventHandler(muDemo_ItemClick);
        }

        void muDemo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        void muDemo_ItemClick(object sender, MenuCommandEventArgs e)
        {
        }

        private void nsExpandDuration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(muDemo != null)
                muDemo.ExpandDuration = (int)e.NewValue;
        }

        private void cbExpandSlide_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbExpandSlide.SelectedIndex == 0)
            {
                muDemo.ShowType = ContextMenuShowTypeMode.None;
            }
            else if (cbExpandSlide.SelectedIndex == 1)
            {
                muDemo.ShowType = ContextMenuShowTypeMode.Slide;
            }
            else if (cbExpandSlide.SelectedIndex == 2)
            {
                muDemo.ShowType = ContextMenuShowTypeMode.Fade;
            }
            else if (cbExpandSlide.SelectedIndex == 3)
            {
                muDemo.ShowType = ContextMenuShowTypeMode.FlyLine;
            }
        }
    }
}

