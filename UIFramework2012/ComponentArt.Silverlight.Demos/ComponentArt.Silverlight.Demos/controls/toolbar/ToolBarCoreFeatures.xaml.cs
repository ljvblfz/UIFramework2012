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
    public partial class ToolBarCoreFeatures : UserControl
    {
        public ToolBarCoreFeatures()
        {
            InitializeComponent();
        }

        private void Split1_MenuClick(object sender, MenuCommandEventArgs e)
        {
            
            ((ToolBarSplitItem)sender).IconSource = ((System.Windows.Media.Imaging.BitmapImage) e.ItemSource.IconSource).UriSource.ToString();
        }
    }
}
