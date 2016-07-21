using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ComponentArt.Silverlight.Demos
{
	public partial class HtmlPanelCoreFeatures : UserControl, IDisposable, IDisableable
    {
        private Regex uriPattern;

		public HtmlPanelCoreFeatures()
        {
            InitializeComponent();
            uriPattern = new Regex(@"^(http|https)\://([a-zA-Z0-9\-]+\.)+[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?(/?[a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*$");
            htmlPanel.SourceUri = htmlUri.Text;
        }

        public void Dispose()
        {
            htmlPanel.Dispose();
        }

        private void htmlUri_TextChanged(object sender, TextChangedEventArgs e)            
        {
            TextBox htmlUri = (TextBox)sender;      
            
            if (uriPattern.IsMatch(htmlUri.Text))
                htmlUri.Background = new SolidColorBrush(Color.FromArgb(0x7f, 0x55, 0xff, 0x55));
            else
                htmlUri.Background = new SolidColorBrush(Color.FromArgb(0x7f, 0xff, 0x55, 0x55));
        }
        
        private void htmlUri_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox htmlUri = (TextBox)sender;

            if (e.Key == Key.Enter && uriPattern.IsMatch(htmlUri.Text)) 
            {
                htmlUri.Background = new SolidColorBrush(Colors.White);
                htmlPanel.SourceUri = htmlUri.Text;
            }            
        }
    }
}
