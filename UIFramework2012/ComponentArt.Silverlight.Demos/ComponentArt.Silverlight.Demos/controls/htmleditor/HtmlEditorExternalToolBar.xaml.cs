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

namespace ComponentArt.Silverlight.Demos
{
    public partial class HtmlEditorExternalToolBar : UserControl, IDisposable, IDisableable
    {

        public HtmlEditorExternalToolBar()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            HtmlEditor5.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HtmlEditor5.ExecCommand(((Button)sender).Tag.ToString(), null, false);
        }
    }
}
