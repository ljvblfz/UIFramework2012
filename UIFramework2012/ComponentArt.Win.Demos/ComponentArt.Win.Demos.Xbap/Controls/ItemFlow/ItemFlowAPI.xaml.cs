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
    public partial class ItemFlowAPI : UserControl, IDisposable
    {
        public ItemFlowAPI() 
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myItemFlow != null)
            {
                myItemFlow.MoveTo(((ComboBox)sender).SelectedIndex);
            }
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (myItemFlow != null)
            {
                myItemFlow.SelectedIndex = (((ComboBox)sender).SelectedIndex);
                myItemFlow.Initialize();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
