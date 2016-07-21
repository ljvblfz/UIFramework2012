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
    public partial class ItemFlowXamlContent : UserControl, IDisposable
    {
        public ItemFlowXamlContent() 
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            myItemFlow.Previous();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            myItemFlow.Next();
        }

        private void refresh(object sender, RoutedEventArgs e)
        {
            myItemFlow.Initialize();
        }

        private void update(object sender, RoutedEventArgs e)
        {
 
        }
    
        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
