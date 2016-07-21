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
  public partial class SplitterEvents : UserControl
  {
      public SplitterEvents()
    {
      InitializeComponent();
      MouseMoveAnimation = this.Resources["ShowMouseMove"] as Storyboard;
    }

      private Storyboard MouseMoveAnimation;

      private void Splitter_ResizeStart(object sender, ComponentArt.Silverlight.UI.Layout.SplitterCancelEventArgs e)
      {
          addToList("Resize Started");
      }

      private void Splitter_ResizeEnd(object sender, ComponentArt.Silverlight.UI.Layout.SplitterResizedEventArgs e)
      {
          addToList("Resize Finished");
      }

      private void Splitter_CollapseButtonClick(object sender, SplitterCancelEventArgs e)
      {
          addToList("Collapse Button Clicked: " + ((sender as Splitter).IsCollapsed ? "Expanding" : "Collapsing"));
      }

      private void Splitter_Resizing(object sender, ComponentArt.Silverlight.UI.Layout.SplitterResizedEventArgs e)
      {
          MouseMoveAnimation.Begin();
      }

      private void HorizontalSplitter_Collapsed(object sender, EventArgs e)
      {
          Splitter sp = sender as Splitter;
          if (sp == null)
              return;

          if ((sender as Splitter).IsCollapsed)
              addToList("Collapsed");
          else
              addToList("Expanded");
      }

      private void VerticalSplitter_Collapsed(object sender, EventArgs e)
      {
          if ((sender as Splitter).IsCollapsed)
              addToList("Collapsed");
          else
              addToList("Expanded");
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
