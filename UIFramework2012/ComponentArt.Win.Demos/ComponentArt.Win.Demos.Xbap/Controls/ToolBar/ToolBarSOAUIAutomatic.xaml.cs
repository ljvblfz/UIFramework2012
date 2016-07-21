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
    public partial class ToolBarSOAUIAutomatic : UserControl, IDisposable
    {
        public ToolBarSOAUIAutomatic() 
        {
            InitializeComponent();
            ToolBar1.SoaServiceUrl = new Uri(DemoWebHelper.BaseUri, "../Services/SoaSimpleToolBarService.svc").ToString();
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
