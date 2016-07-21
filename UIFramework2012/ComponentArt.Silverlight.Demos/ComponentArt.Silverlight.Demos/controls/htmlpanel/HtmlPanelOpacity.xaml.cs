using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

using ComponentArt.Silverlight.UI.Utils;

namespace ComponentArt.Silverlight.Demos
{
	public partial class HtmlPanelOpacity : UserControl, IDisposable, IDisableable
    {
        public HtmlPanelOpacity()
        {
            InitializeComponent();
			opacitySlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(opacitySlider_ValueChanged);
			opacitySlider.MouseEnter += new MouseEventHandler(opacitySlider_MouseEnter);
		}

        public void Dispose()
        {
            htmlPanel.Dispose();
        }

		private void UpdateOpacity()
		{
			double opacity = opacitySlider.Value;
			bool onBorder = (opacityOnBorder.IsChecked == true);

			opacitySliderValue.Text = opacity.ToString("0.00");
			
			htmlPanel.Opacity = onBorder ? 1 : opacity;
			htmlPanelBorder.Opacity = onBorder ? opacity : 1;
		}

		private void opacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			UpdateOpacity();
		}
		
		void opacitySlider_MouseEnter(object sender, MouseEventArgs e)
		{
			((Thumb)Visual.FindElementByName(opacitySlider, "HorizontalThumb")).CancelDrag();
		}

		private void opacityOnBorder_Checked(object sender, RoutedEventArgs e)
		{
			UpdateOpacity();
		}

		private void opacityOnBorder_Unchecked(object sender, RoutedEventArgs e)
		{
			UpdateOpacity();
		}
	}
}
