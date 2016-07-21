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
  public partial class RangeSliderEvents : UserControl
  {
      public RangeSliderEvents()
    {
      InitializeComponent();
      MouseMoveAnimation = this.Resources["ShowMouseMove"] as Storyboard;
    }

      private Storyboard MouseMoveAnimation;

      private void myRangeSlider_SelectionChanged(object sender, ComponentArt.Silverlight.UI.Input.RangeSliderSelectionChangedEventArgs e)
      {
          addToList(string.Format("Selection Changed: {0} - {1}", e.NewSelectionStart, e.NewSelectionEnd));
      }

      private void myRangeSlider_SelectionChanging(object sender, ComponentArt.Silverlight.UI.Input.RangeSliderSelectionChangedEventArgs e)
      {
          MouseMoveAnimation.Begin();
      }

      private void addToList(string msg)
      {
          eventList.Items.Add(new EventDisplay(msg));
          // select & scroll to the bottom
          eventList.Dispatcher.BeginInvoke(() =>
          {
              eventList.ScrollIntoView(eventList.Items[eventList.Items.Count - 1]);
          });
      }

      private void Clear_Click(object sender, RoutedEventArgs e)
      {
          eventList.Items.Clear();
      }

      // used to avoid bugs in Listbox.ScrollIntoView() when Listbox.Items are strings
      private class EventDisplay
      {
          string EventMsg;
          public EventDisplay(string msg)
          { EventMsg = msg; }
          public override string ToString()
          {
              return EventMsg;
          }
      }
  }
}
