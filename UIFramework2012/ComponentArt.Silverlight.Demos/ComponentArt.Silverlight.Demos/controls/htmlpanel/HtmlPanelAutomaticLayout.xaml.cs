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

using ComponentArt.Silverlight.UI.Layout;

namespace ComponentArt.Silverlight.Demos
{
    public partial class HtmlPanelAutomaticLayout : UserControl, IDisposable, IDisableable
    {
		public HtmlPanelAutomaticLayout()
        {
            InitializeComponent();

			boundary.Width = 350;
			boundary.Height = 300;
		}

        public void Dispose()
        {
            htmlPanel.Dispose();
        }


		private void thumb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			thumb.MouseMove += new MouseEventHandler(thumb_MouseMove);
			thumb.CaptureMouse();
			
		}

		private void thumb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			thumb.ReleaseMouseCapture();
			thumb.MouseMove -= new MouseEventHandler(thumb_MouseMove);
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
		}
    }
}
