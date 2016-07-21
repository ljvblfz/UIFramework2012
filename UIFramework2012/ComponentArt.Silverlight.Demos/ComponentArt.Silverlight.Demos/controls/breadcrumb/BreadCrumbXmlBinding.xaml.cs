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
using System.ComponentModel;

namespace ComponentArt.Silverlight.Demos
{
    public partial class BreadCrumbXmlBinding : UserControl
    {
        public BreadCrumbXmlBinding()
        {
            InitializeComponent();
        }

        private void bcDemo_BreadCrumbClick(object sender, BreadCrumbCommandEventArgs mce)
        {
            // Debug.WriteLine("BreadCrumb click: " + bcea.ItemSource.Text + ", command: " + bcea.Action);
        }
     }
}
