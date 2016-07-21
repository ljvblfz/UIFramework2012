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

namespace ComponentArt.Silverlight.Demos
{
    public partial class MenuBoundaryDetection : UserControl, IDisposable
    {
        public MenuBoundaryDetection()
        {
            InitializeComponent();
        }

        private void muDemo_Initialized(object sender, RoutedEventArgs e)
        {
            muDemo.Clip = new RectangleGeometry();
            ((RectangleGeometry)muDemo.Clip).Rect = new Rect(0, 0, 350, 300);
            RectangleGeometry rg = (RectangleGeometry)muDemo.Clip;
            boundary.Width = rg.Rect.Width;
            boundary.Height = rg.Rect.Height;
        }

        private void thumb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            thumb.MouseMove += new MouseEventHandler(thumb_MouseMove);
            thumb.CaptureMouse();
        }

        void thumb_MouseMove(object sender, MouseEventArgs e)
        {
            double w, h;

            w = e.GetPosition(LayoutRoot).X > 0 ? e.GetPosition(LayoutRoot).X : 0;
            h = e.GetPosition(LayoutRoot).Y > 45 ? e.GetPosition(LayoutRoot).Y : 45;
            w = e.GetPosition(LayoutRoot).X > Application.Current.Host.Content.ActualWidth ? Application.Current.Host.Content.ActualWidth : w;
            h = e.GetPosition(LayoutRoot).Y > Application.Current.Host.Content.ActualHeight ? Application.Current.Host.Content.ActualHeight : h;

            boundary.Width = w + 10;
            boundary.Height = h + 10;

            ((RectangleGeometry)muDemo.Clip).Rect = new Rect(0,0,boundary.Width,boundary.Height);
        }

        private void thumb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            thumb.ReleaseMouseCapture();
            thumb.MouseMove -= new MouseEventHandler(thumb_MouseMove);
        }

        private void cb_option_Checked(object sender, RoutedEventArgs e)
        {
            muDemo.Clip = null;
            muDemo.UpdateLayout();
            boundary.Visibility = Visibility.Collapsed;
            muDemo.Visibility = Visibility.Visible;
        }

        private void cb_option_Unchecked(object sender, RoutedEventArgs e)
        {
            muDemo.Clip = new RectangleGeometry();
            ((RectangleGeometry)muDemo.Clip).Rect = new Rect(0, 0, boundary.Width, boundary.Height);
            boundary.Visibility = Visibility.Visible;
            muDemo.Visibility = Visibility.Visible;
        }

        #region IDisposable Members

        public void Dispose()
        {
            muDemo.Dispose();
        }

        #endregion
    }
}
