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
  public partial class ItemFlowCoreFeatures : UserControl
  {
    public ItemFlowCoreFeatures()
    {
      InitializeComponent();

      scroll.Maximum = myItemFlow.Items.Count - 1;
      scroll.Value = myItemFlow.SelectedIndex;
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
    }

    private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      if (myItemFlow == null) return;
      myItemFlow.ItemHeight = Convert.ToInt16(e.NewValue);
      myItemFlow.ItemWidth = Convert.ToInt16(e.NewValue);
    }

    private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      if (myItemFlow == null) return;
      myItemFlow.ItemSpacing = e.NewValue;
    }

    private void Slider_ValueChanged_2(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      if (myItemFlow == null) return;
      myItemFlow.HorizontalPerspective = e.NewValue;
    }

    private void scroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      if (myItemFlow == null) return;
      myItemFlow.MoveTo(Convert.ToInt16(e.NewValue));
    }

    private void myItemFlow_FlowComplete(object sender, ComponentArt.Silverlight.UI.Navigation.ItemFlowItemEventArgs e)
    {
      scroll.Value = e.Index;
    }

    private void Slider_ValueChanged_4(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      if (myItemFlow == null) return;
      myItemFlow.Zoom = e.NewValue;
    }

    private void Slider_ValueChanged_5(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      if (myItemFlow == null) return;
      myItemFlow.SelectedOffsetY = Convert.ToInt16(e.NewValue);
    }
  }
}
