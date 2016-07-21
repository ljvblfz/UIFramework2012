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
    public partial class BreadCrumbCoreFeatures : UserControl
    {

        public BreadCrumbCoreFeatures()
        {
            InitializeComponent();
        }

        private void bcDemo_Initialized(object sender, RoutedEventArgs e)
        {
            bcDemo.SelectedItem = SelectMe;
            path.Text = bcDemo.GetPathForItem(bcDemo.SelectedItem, "/");
            nsCollapseDelay.MaximumValue = 1000;
            nsCollapseDuration.MaximumValue = 1000;
            nsExpandDelay.MaximumValue = 1000;
            nsExpandDelay.MinimumValue = 0;
            nsExpandDuration.MaximumValue = 1000;
            nsCollapseDelay.Increment = 25;
            nsCollapseDuration.Increment = 25;
            nsExpandDelay.Increment = 25;
            nsExpandDuration.Increment = 25;
            cxUseRootIcon.IsChecked = bcDemo.UseRootIcon;
            cxExpandOnClick.IsChecked = bcDemo.ExpandOnClick;
            nsExpandDelay.Value = bcDemo.ExpandDelay;
            nsCollapseDelay.Value = bcDemo.CollapseDelay;
            nsExpandDuration.Value = bcDemo.ExpandDuration;
            nsCollapseDuration.Value = bcDemo.CollapseDuration;

            bcDemo.PropertyChanged += new PropertyChangedEventHandler(bcDemo_PropertyChanged);
            bcDemo.BreadCrumbClick += new BreadCrumb.BreadCrumbClickEventHandler(bcDemo_ItemClick);
        }

        private void cbExpandTransition_Initialized(object sender, RoutedEventArgs e)
        {
            if (bcDemo.ExpandTransition == BreadCrumbExpandTransitionMode.Fade)
            {
                cbExpandTransition.SelectItem(1);
            }
            else
            {
                cbExpandTransition.SelectItem(0);
            }
        }

        private void cbCollapseTransition_Initialized(object sender, RoutedEventArgs e)
        {
            if (bcDemo.CollapseTransition == BreadCrumbCollapseTransitionMode.Fade)
            {
                cbCollapseTransition.SelectItem(1);
            }
            else
            {
                cbCollapseTransition.SelectItem(0);
            }
        }

        private void cbCollapseSlide_Initialized(object sender, RoutedEventArgs e)
        {
            if (bcDemo.CollapseSlide == BreadCrumbCollapseSlideMode.Linear)
            {
                cbCollapseSlide.SelectItem(0);
            }
            else if (bcDemo.CollapseSlide == BreadCrumbCollapseSlideMode.Accelerate)
            {
                cbCollapseSlide.SelectItem(1);
            }
            else if (bcDemo.CollapseSlide == BreadCrumbCollapseSlideMode.Decelerate)
            {
                cbCollapseSlide.SelectItem(2);
            }

        }

        private void cbExpandSlide_Initialized(object sender, RoutedEventArgs e)
        {
            if (bcDemo.ExpandSlide == BreadCrumbExpandSlideMode.Linear)
            {
                cbExpandSlide.SelectItem(0);
            }
            else if (bcDemo.ExpandSlide == BreadCrumbExpandSlideMode.Accelerate)
            {
                cbExpandSlide.SelectItem(1);
            }
            else if (bcDemo.ExpandSlide == BreadCrumbExpandSlideMode.Decelerate)
            {
                cbExpandSlide.SelectItem(2);
            }
        }

        void bcDemo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            dbg.Text += "Prop: " + e.PropertyName + "\n";
        }

        void bcDemo_ItemClick(object sender, BreadCrumbCommandEventArgs e)
        {
            dbg.Text = "Click: " + e.ItemSource.Text + ", Type: "+e.Action+"\n" + dbg.Text;
            if (e.Action == "label" || e.Action == "sub")
            {
                path.Text = e.ItemSource.GetParentBreadcrumb().GetPathForItem(e.ItemSource, "/");
            }
        }

        private void cxExpandOnClick_Click(object sender, RoutedEventArgs e)
        {
            bcDemo.ExpandOnClick = (Boolean)((CheckBox)sender).IsChecked;
        }

        private void cxUseRootIcon_Click(object sender, RoutedEventArgs e)
        {
            bcDemo.UseRootIcon = (Boolean)((CheckBox)sender).IsChecked;
        }

        private void nsExpandDelay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            bcDemo.ExpandDelay = (int)e.NewValue;
        }

        private void nsCollapseDelay_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            bcDemo.CollapseDelay = (int)e.NewValue;
        }

        private void nsExpandDuration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            bcDemo.ExpandDuration = (int)e.NewValue;
        }

        private void nsCollapseDuration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            bcDemo.CollapseDuration = (int)e.NewValue;
        }

        private void cbExpandTransition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbExpandTransition.SelectedIndex == 1)
            {
                bcDemo.ExpandTransition = BreadCrumbExpandTransitionMode.Fade;
            }
            else if (cbExpandTransition.SelectedIndex == 0)
            {
                bcDemo.ExpandTransition = BreadCrumbExpandTransitionMode.None;
            }
        }

        private void cbCollapseTransition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCollapseTransition.SelectedIndex == 1)
            {
                bcDemo.CollapseTransition = BreadCrumbCollapseTransitionMode.Fade;
            }
            else if (cbCollapseTransition.SelectedIndex == 0)
            {
                bcDemo.CollapseTransition = BreadCrumbCollapseTransitionMode.None;
            }
        }

        private void cbCollapseSlide_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCollapseSlide.SelectedIndex == 0)
            {
                bcDemo.CollapseSlide = BreadCrumbCollapseSlideMode.Linear;
            }
            else if (cbCollapseSlide.SelectedIndex == 1)
            {
                bcDemo.CollapseSlide = BreadCrumbCollapseSlideMode.Accelerate;
            }
            else if (cbCollapseSlide.SelectedIndex == 2)
            {
                bcDemo.CollapseSlide = BreadCrumbCollapseSlideMode.Decelerate;
            }
        }

        private void cbExpandSlide_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbExpandSlide.SelectedIndex == 0)
            {
                bcDemo.ExpandSlide = BreadCrumbExpandSlideMode.Linear;
            }
            else if (cbExpandSlide.SelectedIndex == 1)
            {
                bcDemo.ExpandSlide = BreadCrumbExpandSlideMode.Accelerate;
            }
            else if (cbExpandSlide.SelectedIndex == 2)
            {
                bcDemo.ExpandSlide = BreadCrumbExpandSlideMode.Decelerate;
            }
        }

        private void dbg_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }


    }
}
