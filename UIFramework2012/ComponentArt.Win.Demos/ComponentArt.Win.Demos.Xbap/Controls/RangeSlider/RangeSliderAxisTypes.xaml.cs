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

using ComponentArt.Win.UI.Input;

namespace ComponentArt.Win.Demos
{
  public partial class RangeSliderAxisTypes : UserControl
  {
      public RangeSliderAxisTypes()
    {
        InitializeComponent();
    }

      private void RangeSlider_BeforeTooltipShown(object sender, ComponentArt.Win.UI.Input.RangeSliderBeforeTooltipShownEventArgs e)
      {
          switch (e.DragPoint)
          {
              case RangeSliderDragPoint.SelectionStart:
                  e.TooltipContent = ((DateTime)dateTimeRangeSlider.SelectionStart).ToString("Y");
                  break;
              case RangeSliderDragPoint.SelectionEnd:
                  e.TooltipContent = ((DateTime)dateTimeRangeSlider.SelectionEnd).ToString("Y");
                  break;
              case RangeSliderDragPoint.WholeSlider:
                  e.TooltipContent = ((DateTime)dateTimeRangeSlider.SelectionStart).ToString("Y") +" – " +
                                     ((DateTime)dateTimeRangeSlider.SelectionEnd).ToString("Y");
                  break;
          }
      }
  }

  public class AxisSource : System.Collections.ObjectModel.Collection<string>
  {
      public AxisSource()
      {
          this.Add("Bachelor");
          this.Add("1 Bedroom");
          this.Add("2 Bedroom");
          this.Add("3+ Bedroom");
          this.Add("Penthouse");
          this.Add("Mansion");
      }
  }
}
