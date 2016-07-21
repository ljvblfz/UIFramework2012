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
  public partial class RangeSliderAPI : UserControl
  {
      public RangeSliderAPI()
    {
      InitializeComponent();
    }

      private void Button_Click(object sender, RoutedEventArgs e)
      {
          object prev, next;
          switch ((sender as Button).Name)
          {
              case "selStartDecrement":
                  prev = myRangeSlider.GetPreviousValue(myRangeSlider.SelectionStart);
                  if (prev != null)
                      myRangeSlider.SelectionStart = prev;
                  break;
              case "selStartIncrement":
                  next = myRangeSlider.GetNextValue(myRangeSlider.SelectionStart);
                  if (next != null)
                      myRangeSlider.SelectionStart = next;
                  break;
              case "selEndDecrement":
                  prev = myRangeSlider.GetPreviousValue(myRangeSlider.SelectionEnd);
                  if (prev != null)
                      myRangeSlider.SelectionEnd = prev;
                  break;
              case "selEndIncrement":
                  next = myRangeSlider.GetNextValue(myRangeSlider.SelectionEnd);
                  if (next != null)
                      myRangeSlider.SelectionEnd = next;
                  break;
              case "thumbDecrement":
                  prev = myRangeSlider.GetPreviousValue(myRangeSlider.SelectionStart);
                  next = myRangeSlider.GetPreviousValue(myRangeSlider.SelectionEnd);
                  if (prev != null && next != null)
                  {
                      myRangeSlider.SelectionStart = prev;
                      myRangeSlider.SelectionEnd = next;
                  }
                  break;
              case "thumbIncrement":
                  prev = myRangeSlider.GetNextValue(myRangeSlider.SelectionStart);
                  next = myRangeSlider.GetNextValue(myRangeSlider.SelectionEnd);
                  if (prev != null && next != null)
                  {
                      myRangeSlider.SelectionStart = prev;
                      myRangeSlider.SelectionEnd = next;
                  }
                  break;

              case "axisZoomout":
                  myAxis.Minimum -= 10;
                  myAxis.Maximum += 10;
                  break;
              case "axisZoomIn":
                  if (myAxis.Maximum - myAxis.Minimum > 20 &&
                      myAxis.Minimum + 10 <= (double)myRangeSlider.SelectionStart &&
                      myAxis.Maximum - 10 >= (double)myRangeSlider.SelectionEnd)
                  {
                      myAxis.Minimum += 10;
                      myAxis.Maximum -= 10;
                  }
                  break;
                  
                      
          }
      }
  }
}
