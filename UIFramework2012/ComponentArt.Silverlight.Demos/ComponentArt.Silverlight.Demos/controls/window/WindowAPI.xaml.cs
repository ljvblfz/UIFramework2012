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
    public partial class WindowAPI : UserControl
    {
        public WindowAPI()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1.Alert(alert.Text, Closed);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window1.Confirm(confirm.Text, Closed);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Window1.Prompt(prompt.Text, Closed);
        }

        private void Closed(object sender, WindowClosedEventArgs e)
        {
            output.Text = "";

            if(e.Result != WindowResult.None)
                output.Text += e.Result.ToString()+"\n";

            if (e.Option != null)
                output.Text += e.Option.Content.ToString() + "\n";

            if (e.Text != null)
                output.Text += "Text: '"+e.Text + "'\n";
        }
    }
}
