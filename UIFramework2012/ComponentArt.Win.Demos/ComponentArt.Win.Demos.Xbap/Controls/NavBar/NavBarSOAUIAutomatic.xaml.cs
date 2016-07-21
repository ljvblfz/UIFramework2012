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

namespace ComponentArt.Win.Demos
{
    public partial class NavBarSOAUIAutomatic : UserControl
    {
        public NavBarSOAUIAutomatic()
        {
            InitializeComponent();

            MyNavBar.SoaServiceUrl = new Uri(DemoWebHelper.BaseUri, "../Services/SoaNavBar.svc").ToString();
        }

        /// ComponentArt SOA.UI Framework allows for data binding without any code on the client side.

    }
}
