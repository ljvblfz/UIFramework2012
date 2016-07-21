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
using System.ComponentModel;

namespace ComponentArt.Silverlight.Demos
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
            //muDemo.Background = new SolidColorBrush(Menu.getColorFromHexString("7FFFFFFF"));
            nsExpandDuration.MaximumValue = 1000;
            nsExpandDuration.Increment = 25;

            nsExpandDuration.Value = muDemo.ExpandDuration;
            cbExpandSlide.SelectedIndex = 1;

            // muDemo._dbg = dbg;
            muDemo.PropertyChanged += new PropertyChangedEventHandler(muDemo_PropertyChanged);
            muDemo.MenuClick += new Menu.MenuClickEventHandler(muDemo_ItemClick);
        }

        void muDemo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //dbg.Text += "Prop: " + e.PropertyName + "\n";
        }
        void muDemo_ItemClick(object sender, MenuCommandEventArgs e)
        {
            //dbg.Text += "Click: " + e.itemSource.Text + "\n";
        }

        private void nsExpandDuration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
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

