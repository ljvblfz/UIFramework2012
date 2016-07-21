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
using ComponentArt.Silverlight.UI.Navigation;
using System.Windows.Browser;

namespace ComponentArt.Silverlight.Demos
{
    public partial class HtmlEditorAPI : UserControl, IDisposable, IDisableable
    {
        
		public HtmlEditorAPI()
        {
            InitializeComponent();
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            HtmlEditor1.PasteHtml(html.Text);
        }

        private void Selection_Click(object sender, RoutedEventArgs e)
        {
            selection.Text = HtmlEditor1.SelectedHtml.Replace("\n","").Replace("\r","");
        }

        private void SelectionText_Click(object sender, RoutedEventArgs e)
        {
            selectiontext.Text = HtmlEditor1.SelectedText.Replace("\n", "").Replace("\r", "");
        }

        private void HtmlContent_Click(object sender, RoutedEventArgs e)
        {
            htmlcontent.Text = HtmlEditor1.HtmlContent;
        }

        private void TextContent_Click(object sender, RoutedEventArgs e)
        {
            textcontent.Text = HtmlEditor1.TextContent;
        }

        public void Dispose()
        {
            HtmlEditor1.Dispose();
        }
    }
}
