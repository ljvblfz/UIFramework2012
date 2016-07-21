using System;
using System.Windows.Controls;

namespace ComponentArt.Silverlight.Demos
{
	public partial class HtmlPanelHtmlSource : UserControl, IDisposable, IDisableable
    {
        public HtmlPanelHtmlSource()
        {
            InitializeComponent();
			htmlPanel.SourceHtml = htmlSource.Text;
		}

        public void Dispose()
        {
            htmlPanel.Dispose();
        }

        private void htmlSource_TextChanged(object sender, TextChangedEventArgs e)
        {
            htmlPanel.SourceHtml = ((TextBox)sender).Text;
        }
    }
}
