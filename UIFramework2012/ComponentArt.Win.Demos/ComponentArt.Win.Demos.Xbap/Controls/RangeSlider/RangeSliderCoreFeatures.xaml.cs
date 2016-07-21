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

namespace ComponentArt.Win.Demos
{
    public partial class RangeSliderCoreFeatures : UserControl
    {
        public RangeSliderCoreFeatures()
        {
            InitializeComponent();
        }

        private void WaveformRangeSlider_BeforeTooltipShown(object sender, ComponentArt.Win.UI.Input.RangeSliderBeforeTooltipShownEventArgs e)
        {
            e.TooltipContent = string.Format("{0} ms", e.TooltipContent);
        }

        private void NumericAxis_BeforeTickMarkShown(object sender, ComponentArt.Win.UI.Input.SliderAxisBeforeTickMarkShown e)
        {
            // alternate sides
            e.Cancel = ((e.TickMarkPosition == ComponentArt.Win.UI.Input.SliderAxisTickMarkPositions.BottomRight) ^ ((double)e.Value % 10 < 1));
        }
    }
}
