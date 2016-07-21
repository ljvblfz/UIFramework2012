using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

using ComponentArt.Silverlight.UI.Layout;

using ComponentArt.Silverlight.UI.Utils;

namespace ComponentArt.Silverlight.Demos
{
	public partial class HtmlPanelScrolling : UserControl, IDisposable, IDisableable
    {
        public HtmlPanelScrolling()
        {
            InitializeComponent();

            htmlPanel.LayoutUpdated += new EventHandler(htmlPanel_LayoutUpdated);
			hScrollBar.MouseEnter += new MouseEventHandler(hScrollBar_MouseEnter);
			vScrollBar.MouseEnter += new MouseEventHandler(vScrollBar_MouseEnter);
        }

		void hScrollBar_MouseEnter(object sender, MouseEventArgs e)
		{
			((Thumb)Visual.FindElementByName(hScrollBar, "HorizontalThumb")).CancelDrag();
		}

		void vScrollBar_MouseEnter(object sender, MouseEventArgs e)
		{
			((Thumb)Visual.FindElementByName(vScrollBar, "VerticalThumb")).CancelDrag();
		}

        void htmlPanel_LayoutUpdated(object sender, EventArgs e)
        {
            double vValue = vScrollBar.Value;            
            vScrollBar.Maximum = htmlPanel.ScrollableHeight;
            vScrollBar.Value = Math.Min(vValue, vScrollBar.Value);
            vScrollBar.ViewportSize = htmlPanel.Height;

            double hValue = hScrollBar.Value;
            hScrollBar.Maximum = htmlPanel.ScrollableWidth;
            hScrollBar.Value = Math.Min(hValue, hScrollBar.Value);
            hScrollBar.ViewportSize = htmlPanel.Width;
        }

        public void Dispose()
        {
            htmlPanel.Dispose();
        }

        private void vScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            htmlPanel.VerticalOffset = e.NewValue;
		}
        
        private void hScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            htmlPanel.HorizontalOffset = e.NewValue;
		}
    }
}
