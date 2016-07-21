using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

using ComponentArt.Silverlight.UI.Utils;


namespace ComponentArt.Silverlight.Demos
{
	public partial class HtmlPanelManualLayout : UserControl, IDisposable, IDisableable
    {
		public HtmlPanelManualLayout()
        {
            InitializeComponent();

			widthSlider.MouseEnter += new MouseEventHandler(Slider_MouseEnter);
			heightSlider.MouseEnter += new MouseEventHandler(Slider_MouseEnter);
			xSlider.MouseEnter += new MouseEventHandler(Slider_MouseEnter);
			ySlider.MouseEnter += new MouseEventHandler(Slider_MouseEnter);
		}

		void Slider_MouseEnter(object sender, MouseEventArgs e)
		{
			((Thumb)Visual.FindElementByName((Slider)sender, "HorizontalThumb")).CancelDrag();
		}

        public void Dispose()
        {
            htmlPanel.Dispose();
        }
    }
}
