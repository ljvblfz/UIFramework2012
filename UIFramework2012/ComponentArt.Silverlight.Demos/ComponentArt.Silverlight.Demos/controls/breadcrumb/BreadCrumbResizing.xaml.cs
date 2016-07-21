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
    public partial class BreadCrumbResizing : UserControl
    {
        private double maximumSize = 750;
        private double minimumSize = 200;
        public BreadCrumbResizing()
        {
            InitializeComponent();
        }

        private void bcDemo_Initialized(object sender, RoutedEventArgs e)
        {
            cxUseRootIcon.IsChecked = bcDemo.UseRootIcon;
            cxExpandOnClick.IsChecked = bcDemo.ExpandOnClick;
            bcDemo.Width = maximumSize;
            boundary.Width = bcDemo.Width;
            bcDemo.SelectedItem = SelectMe;
        }

        void bcDemo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            dbg.Text += "Prop: " + e.PropertyName + "\n";
        }

        void bcDemo_ItemClick(object sender, BreadCrumbCommandEventArgs e)
        {
            dbg.Text = "Click: " + e.ItemSource.Text + ", Type: " + e.Action + "\n" + dbg.Text;
        }

        private void thumb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            thumb.MouseMove += new MouseEventHandler(thumb_MouseMove);
            thumb.CaptureMouse();
        }

        void thumb_MouseMove(object sender, MouseEventArgs e)
        {
            double w;

            w = e.GetPosition(LayoutRoot).X > minimumSize ? e.GetPosition(LayoutRoot).X : minimumSize;
            w = e.GetPosition(LayoutRoot).X > maximumSize ? maximumSize : w;

            boundary.Width = w + 10;
            boundary.Height = 20;
            bcDemo.Width = boundary.Width;

            // ((RectangleGeometry)bcDemo.Clip).Rect = new Rect(0, 0, boundary.Width, boundary.Height);
        }

        private void thumb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            thumb.ReleaseMouseCapture();
            thumb.MouseMove -= new MouseEventHandler(thumb_MouseMove);
        }

        private void cxExpandOnClick_Click(object sender, RoutedEventArgs e)
        {
            bcDemo.ExpandOnClick = (Boolean)((CheckBox)sender).IsChecked;
        }

        private void cxUseRootIcon_Click(object sender, RoutedEventArgs e)
        {
            bcDemo.UseRootIcon = (Boolean)((CheckBox)sender).IsChecked;
        }
    }
}
