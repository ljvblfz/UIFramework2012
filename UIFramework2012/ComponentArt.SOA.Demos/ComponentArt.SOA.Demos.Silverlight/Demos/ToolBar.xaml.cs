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

namespace ComponentArt.SOA.Demos
{
    public partial class ToolBar : UserControl
    {
        public ToolBar()
        {
            InitializeComponent();
        }

        private void ToolBar1_SoaResponseProcessed(object sender, ComponentArt.Silverlight.UI.Navigation.ToolBarSoaResponseEventArgs e)
        {
            foreach (ToolBarItem myItem in e.Items)
            {
                if (myItem is ToolBarSeparatorItem)
                {
                    myItem.Width = 5;
                    myItem.Height = 28;
                }
                else
                {
                    myItem.Width = 30;
                    myItem.Height = 28;
                }
            }
        }
    }
}
