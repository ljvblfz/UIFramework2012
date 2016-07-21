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
  public partial class RangeSliderDatabindingFeatures : UserControl
  {
      public RangeSliderDatabindingFeatures()
    {
        // give demo additional polish
       // this.Dispatcher.Thread.CurrentCulture = new System.Globalization.CultureInfo("pl-PL");
        InitializeComponent();
    }

    //update binding on Enter
    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
          System.Windows.Data.BindingExpression bExp = (sender as TextBox).GetBindingExpression(TextBox.TextProperty);
          if (bExp != null)
          {
              bExp.UpdateSource();
          }
      }
    }

    private void dateTimeRangeSlider_BeforeTooltipShown(object sender, RangeSliderBeforeTooltipShownEventArgs e)
    {
        switch (e.DragPoint)
        {
            case RangeSliderDragPoint.SelectionStart:
                e.TooltipContent = ((DateTime)dateTimeRangeSlider.SelectionStart).ToLongDateString();
                break;
            case RangeSliderDragPoint.SelectionEnd:
                e.TooltipContent = ((DateTime)dateTimeRangeSlider.SelectionEnd).ToLongDateString();
                break;
            case RangeSliderDragPoint.WholeSlider:
                e.TooltipContent = ((DateTime)dateTimeRangeSlider.SelectionStart).ToLongDateString() + " – " + 
                                   ((DateTime)dateTimeRangeSlider.SelectionEnd).ToLongDateString();
                break;
        }
    }
  }
}
