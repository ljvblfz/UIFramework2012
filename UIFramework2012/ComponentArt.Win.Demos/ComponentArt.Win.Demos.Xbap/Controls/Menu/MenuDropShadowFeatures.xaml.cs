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
    public partial class MenuDropShadowFeatures : UserControl, IDisposable
    {
        private String BaseColor = "000000";
        private bool initd = false;
        private bool link = false;

        public MenuDropShadowFeatures()
        {
            InitializeComponent();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void muDemo_Initialized(object sender, RoutedEventArgs e)
        {
            initd = true;
            muDemo.PropertyChanged += new PropertyChangedEventHandler(muDemo_PropertyChanged);
            muDemo.MenuClick += new ComponentArt.Win.UI.Navigation.Menu.MenuClickEventHandler(muDemo_ItemClick);
        }

        void muDemo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        void muDemo_ItemClick(object sender, MenuCommandEventArgs e)
        {

        }

        private void Slider_Voffset_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!initd) { return; }
            if (link == true)
            {
                sl_Hoffset.Value = sl_Voffset.Value;
            }
            muDemo.ShadowOffsetVertical = (int)sl_Voffset.Value;
        }

        private void Slider_Hoffset_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!initd) { return; }
            if (link == true)
            {
                sl_Voffset.Value = sl_Hoffset.Value;
            }
            muDemo.ShadowOffsetHorizontal = (int)sl_Hoffset.Value;
        }

        private void Slider_opacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!initd) { return; }
            int dhex = (int)sl_opacity.Value;

            String hex = String.Format("{00:X}", dhex);
            muDemo.ShadowColor = ComponentArt.Win.UI.Utils.Visual.GetColorFromHexString(hex + BaseColor);
        }

        private void cx_link_Click(object sender, RoutedEventArgs e)
        {
            if ( ((CheckBox)e.OriginalSource).IsChecked == true )
            {
                link = true;
                double newval = Math.Min( sl_Voffset.Value , sl_Hoffset.Value) + (Math.Abs(sl_Voffset.Value - sl_Hoffset.Value)/2);
                sl_Hoffset.Value = newval;
                sl_Voffset.Value = newval;
            }
            else
            {
                link = false;
            }
        }




        #region IDisposable Members

        public void Dispose()
        {
            muDemo.Dispose();
        }

        #endregion
    }
}
