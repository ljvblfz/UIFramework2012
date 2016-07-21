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
using System.Diagnostics;

namespace ComponentArt.Silverlight.Demos
{
    public partial class MenuCoreFeatures : UserControl, IDisposable
    {

        public MenuCoreFeatures()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void muDemo_Initialized(object sender, RoutedEventArgs e)
        {
            nsCollapseDelay.MaximumValue = 1000;
            nsCollapseDuration.MaximumValue = 1000;
            nsExpandDelay.MaximumValue = 1000;
            nsExpandDelay.MinimumValue = 0;
            nsExpandDuration.MaximumValue = 1000;
            nsCollapseDelay.Increment = 25;
            nsCollapseDuration.Increment = 25;
            nsExpandDelay.Increment = 25;
            nsExpandDuration.Increment = 25;
            cxExpandOnClick.IsChecked = muDemo.ExpandOnClick;
            cxCascadeCollapse.IsChecked = muDemo.CascadeCollapse;
            nsExpandDelay.Value = muDemo.ExpandDelay;
            nsCollapseDelay.Value = muDemo.CollapseDelay;
            nsExpandDuration.Value = muDemo.ExpandDuration;
            nsCollapseDuration.Value = muDemo.CollapseDuration;

            if (muDemo.ExpandTransition == MenuExpandTransitionMode.Fade)
            {
                cbExpandTransition.SelectItem(1);
            }
            else
            {
                cbExpandTransition.SelectItem(0);
            }

            if (muDemo.CollapseTransition == MenuCollapseTransitionMode.Fade)
            {
                cbCollapseTransition.SelectItem(1);
            }
            else
            {
                cbCollapseTransition.SelectItem(0);
            }

            if (muDemo.CollapseSlide == MenuCollapseSlideMode.Linear)
            {
                cbCollapseSlide.SelectItem(0);
            }
            else if (muDemo.CollapseSlide == MenuCollapseSlideMode.Accelerate)
            {
                cbCollapseSlide.SelectItem(1);
            }
            else if (muDemo.CollapseSlide == MenuCollapseSlideMode.Decelerate)
            {
                cbCollapseSlide.SelectItem(2);
            }

            if (muDemo.ExpandSlide == MenuExpandSlideMode.Linear)
            {
                cbExpandSlide.SelectItem(0);
            }
            else if (muDemo.ExpandSlide == MenuExpandSlideMode.Accelerate)
            {
                cbExpandSlide.SelectItem(1);
            }
            else if (muDemo.ExpandSlide == MenuExpandSlideMode.Decelerate)
            {
                cbExpandSlide.SelectItem(2);
            }

            muDemo.PropertyChanged += new PropertyChangedEventHandler(muDemo_PropertyChanged);
            muDemo.MenuClick += new Menu.MenuClickEventHandler(muDemo_ItemClick);
        }

        void muDemo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            dbg.Text += "Prop: " + e.PropertyName + "\n";
        }

        void muDemo_ItemClick(object sender, MenuCommandEventArgs e)
        {
            dbg.Text += "Click: " + e.ItemSource.Text + "\n";
        }

        private void cxExpandOnClick_Click(object sender, RoutedEventArgs e)
        {
            muDemo.ExpandOnClick = (Boolean)((CheckBox)sender).IsChecked;
        }

        private void cxCascadeCollapse_Click(object sender, RoutedEventArgs e)
        {
            muDemo.CascadeCollapse = (Boolean)((CheckBox)sender).IsChecked;
        }

        private void nsExpandDelay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            muDemo.ExpandDelay = (int)e.NewValue;
        }

        private void nsCollapseDelay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            muDemo.CollapseDelay = (int)e.NewValue;
        }

        private void nsExpandDuration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            muDemo.ExpandDuration = (int)e.NewValue;
        }

        private void nsCollapseDuration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            muDemo.CollapseDuration = (int)e.NewValue;
        }

        private void cbExpandTransition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("ExpandTransition: "+cbExpandTransition.SelectedIndex );
            if (cbExpandTransition.SelectedIndex == 1)
            {
                muDemo.ExpandTransition = MenuExpandTransitionMode.Fade;
            }
            else if (cbExpandTransition.SelectedIndex == 0)
            {
                muDemo.ExpandTransition = MenuExpandTransitionMode.None;
            }
        }

        private void cbCollapseTransition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCollapseTransition.SelectedIndex == 1)
            {
                muDemo.CollapseTransition = MenuCollapseTransitionMode.Fade;
            }
            else if (cbCollapseTransition.SelectedIndex == 0)
            {
                muDemo.CollapseTransition = MenuCollapseTransitionMode.None;
            }
        }

        private void cbCollapseSlide_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCollapseSlide.SelectedIndex == 0)
            {
                muDemo.CollapseSlide = MenuCollapseSlideMode.Linear;
            }
            else if (cbCollapseSlide.SelectedIndex == 1)
            {
                muDemo.CollapseSlide = MenuCollapseSlideMode.Accelerate;
            }
            else if (cbCollapseSlide.SelectedIndex == 2)
            {
                muDemo.CollapseSlide = MenuCollapseSlideMode.Decelerate;
            }
        }

        private void cbExpandSlide_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbExpandSlide.SelectedIndex == 0)
            {
                muDemo.ExpandSlide = MenuExpandSlideMode.Linear;
            }
            else if (cbExpandSlide.SelectedIndex == 1)
            {
                muDemo.ExpandSlide = MenuExpandSlideMode.Accelerate;
            }
            else if (cbExpandSlide.SelectedIndex == 2)
            {
                muDemo.ExpandSlide = MenuExpandSlideMode.Decelerate;
            }
        }

        private void dbg_SizeChanged(object sender, SizeChangedEventArgs e)
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
