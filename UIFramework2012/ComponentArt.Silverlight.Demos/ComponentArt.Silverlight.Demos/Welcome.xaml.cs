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
using System.Windows.Media.Imaging;
using System.Reflection;
using System.Diagnostics;

namespace ComponentArt.Silverlight.Demos
{
  public partial class Welcome : UserControl
  {
    public Welcome()
    {
      InitializeComponent();
	}
        private void DemoButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                ((App)Application.Current).LoadDemo(sender, e);
            }
            else if (sender is HyperlinkButton)
            {
                ((App)Application.Current).LoadDemoHyper(sender, e);
            }
        }

        private void ControlButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                ((App)Application.Current).NavigateToDemo(sender, e);
            }
            else if (sender is HyperlinkButton)
            {
                ((App)Application.Current).NavigateToDemoHyper(sender, e);
            }
        }

    private void RollOverButton_MouseEnter(object sender, MouseEventArgs e)
	{
    	string action = ((Button)sender).Tag.ToString().Split(new char[] { ' ' })[0];
        string target = ((Button)sender).Tag.ToString().Split(new char[] { ' ' })[1];
		MainImage.Source = (ImageSource)new BitmapImage(new Uri( "../Assets/Pages/Common/mainVisual-"+target+".png", UriKind.RelativeOrAbsolute) );

        Storyboard oAnimation = (Storyboard)this.Resources[ "MainImageHighlight_animation" ];
        oAnimation.Begin();
    }

    private void RollOverButton_MouseLeave(object sender, MouseEventArgs e)
    {
		MainImage.Source = (ImageSource)new BitmapImage(new Uri( "../Assets/Pages/Welcome/WebUiBox.png", UriKind.RelativeOrAbsolute) );
    }
  }
}
