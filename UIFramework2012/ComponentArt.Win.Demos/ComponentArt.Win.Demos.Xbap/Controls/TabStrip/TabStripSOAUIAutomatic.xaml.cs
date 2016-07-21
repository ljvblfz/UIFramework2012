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

namespace ComponentArt.Win.Demos 
{
    public partial class TabStripSOAUIAutomatic : UserControl, IDisposable
    {
        public TabStripSOAUIAutomatic() 
        {
            InitializeComponent();
            TabStrip1.SoaServiceUrl = new Uri(DemoWebHelper.BaseUri, "../Services/SoaSimpleTabStripService.svc").ToString();
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
        }

        private void TabStrip1_SoaResponseProcessed(object sender, TabStripSoaResponseEventArgs e)
        {
            foreach (TabStripTab myTab in e.Tabs)
                myTab.Padding = new Thickness(5, 3, 5, 3);
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

    }
}
