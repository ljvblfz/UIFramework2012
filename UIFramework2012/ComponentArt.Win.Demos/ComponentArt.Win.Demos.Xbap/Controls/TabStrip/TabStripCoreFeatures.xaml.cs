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
using ComponentArt.Win.UI.Navigation;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace ComponentArt.Win.Demos 
{
    public partial class TabStripCoreFeatures : UserControl, IDisposable
    {
       private string[] sites = new String[10]{"ComponentArt","Google","Bing","Microsoft","Slashdot","Yahoo","Wikipedia","Codeplex","Mono","MSDN"};

        public TabStripCoreFeatures() 
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
        }

 private void AddTab_Click(object sender, RoutedEventArgs e)
        {
            
            Random ran = new Random();
            TabStripTab newTab = new TabStripTab();
            newTab.Padding = new Thickness(3, 3, 8, 3);
            newTab.Height = 30;
            newTab.Tag = sites[ran.Next(9)];
            newTab.Style = (Style)this.Resources["TabStripTabStyle"];
            newTab.VerticalAlignment = VerticalAlignment.Bottom;

            Image newImage = new Image();
            BitmapImage bi = new BitmapImage(new Uri("/ComponentArt.Win.Demos;component/assets/tabstrip/explorer.png", UriKind.RelativeOrAbsolute));
            newImage.Source =(ImageSource)bi;
            newImage.Stretch = Stretch.None;
            newImage.Margin = new Thickness(0, 1, 0, 0);
            newImage.VerticalAlignment = VerticalAlignment.Top;
            newImage.HorizontalAlignment = HorizontalAlignment.Center;

            Button newButton = new Button();
            Style myStyle = (Style)this.Resources["ComponentArt_TabStrip_RemoveButton"];
            newButton.Style = myStyle;
            newButton.Height = 16;
            newButton.Margin = new Thickness(0, 2, 0, 0);
            newButton.Click += new RoutedEventHandler(RemoveTab_Click);

            TextBlock newTextBlock = new TextBlock();
            newTextBlock.Text = newTab.Tag.ToString();
            newTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            newTextBlock.FontFamily = new FontFamily("Tahoma");
            newTextBlock.FontSize = 11;
            newTextBlock.Margin = new Thickness(.5,4,10,0);

            StackPanel newStack = new StackPanel();
            newStack.Orientation = Orientation.Horizontal;

            newStack.Children.Add(newImage);
            newStack.Children.Add(newTextBlock);
            newStack.Children.Add(newButton);

            newTab.Content = newStack;
            TabStrip1.Items.Add(newTab);
        }

        void RemoveTab_Click(object sender, RoutedEventArgs e)
        {
            TabStripTab myTab = (TabStripTab)((StackPanel)((Button)sender).Parent).Parent;
            
            if(TabStrip1.Items.Count > 1)
                TabStrip1.Items.Remove(myTab);

        }

        private void TabStrip1_TabClick(object sender, TabStripTabEventArgs e)
        {
            output.Text = e.Tab.Tag.ToString();
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
