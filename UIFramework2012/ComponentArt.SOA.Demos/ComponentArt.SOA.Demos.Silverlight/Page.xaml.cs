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

namespace ComponentArt.SOA.Demos
{
    public partial class Page : UserControl
    {
        public bool debug = false;

        internal string _demoName;

        public Page()
        {
            InitializeComponent();

            string sStartDemo = System.Windows.Browser.HtmlPage.Window.CurrentBookmark;

            if (sStartDemo == null || sStartDemo == "")
            {
                sStartDemo = "FileExplorer";
            }
            else if (sStartDemo == "TreeViewFileExplorer")
            {
                sStartDemo = "TreeViewFileBrowser";
            }
            VersionTextBlock.DataContext = this;
            LoadDemo(sStartDemo);
        }

        public void LoadDemo(string sDemo)
        {
            _demoName = sDemo;

            if (sDemo != null && sDemo != "")
            {

                UserControl oDemoContent = null;

                try
                {
                  oDemoContent = Activator.CreateInstance(Type.GetType(this.GetType().Namespace + "." + sDemo)) as UserControl;
                }
                catch (Exception ex)
                {
                  // error, show it?

                  if (debug)
                  {
                    MessageBox.Show(ex.ToString());
                  }

                  _demoName = "FileExplorer";
                  oDemoContent = Activator.CreateInstance(Type.GetType(this.GetType().Namespace + "." + _demoName)) as UserControl;
                }

                // update header title, pull from user control
                TextBlock oTitleBlock = (TextBlock)FindName("demo_header_title");
                TextBlock oTitleSource = (TextBlock)oDemoContent.FindName("DemoTitle");
                if (oTitleSource != null)
                {
                    oTitleBlock.Text = oTitleSource.Text;
                }
                else
                {
                    oTitleBlock.Text = sDemo;
                }

                // update header description, pull from user control
                TextBlock oDescriptionBlock = (TextBlock)FindName("demo_header_description");
                TextBlock oDescriptionSource = (TextBlock)oDemoContent.FindName("DemoDescription");
                
                if (oDescriptionSource != null)
                {
                  oDescriptionBlock.Text = oDescriptionSource.Text;
                }
                else
                {
                    oDescriptionBlock.Text = oTitleBlock.Text + " demo";
                }

                if (oDemoContent != null)
                {

                    // load content
                    ContentControl oPanel = (ContentControl)FindName("demo_panel");
                    if (oPanel.Content is IDisposable)
                    {
                        ((IDisposable)oPanel.Content).Dispose();
                    }

                    // dispose?
                    if (oPanel.Content is IDisposable)
                    {
                        ((IDisposable)oPanel.Content).Dispose();
                    }

                    oPanel.Content = oDemoContent;

                    // update bookmark
                    System.Windows.Browser.HtmlPage.Window.NavigateToBookmark(sDemo);
                }
            }
        }

        private void SoaDemoMenuSl_MenuClick(object sender, ComponentArt.Silverlight.UI.Navigation.MenuCommandEventArgs mce)
        {
            LoadDemo(mce.ItemSource.Id);
        }

        private void SoaDemoMenuAj_MenuClick(object sender, ComponentArt.Silverlight.UI.Navigation.MenuCommandEventArgs mce)
        {
            // LoadDemo(mce.itemSource.Id);
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("../", UriKind.RelativeOrAbsolute));
        }

        private void TabButton_Click(object sender, RoutedEventArgs e)
        {
            switch (((ImageButton)sender).Name)
            {
                case "TabButton_Silverlight":
                    {
                        System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://silverlight.componentart.com", UriKind.RelativeOrAbsolute));
                        break;
                    }
                case "TabButton_AspNetAjax":
                    {
                        System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://aspnetajax.componentart.com/", UriKind.RelativeOrAbsolute));
                        break;
                    }
                case "TabButton_WPF":
                    {
                        System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://wpf.componentart.com/", UriKind.RelativeOrAbsolute));
                        break;
                    }
                case "TabButton_AspNetMvc":
                    {
                        System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://aspnetmvc.componentart.com/", UriKind.RelativeOrAbsolute));
                        break;
                    }
                case "TabButton_SoaUi":
                    {
                        System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://soaui.componentart.com", UriKind.RelativeOrAbsolute));
                        break;
                    }
                case "TabButton_Download":
                    {
                        System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://www.componentart.com/download/", UriKind.RelativeOrAbsolute));
                        break;
                    }
                case "TabButton_Silverlight_Local":
                    {
                        System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://www.componentart.com", UriKind.RelativeOrAbsolute));
                        break;
                    }
                default:
                    {
                        // LoadIndexPage("Welcome");
                        break;
                    }
            }

        }

        #region Footer
        /// <summary>
        /// Take the browser to the ComponentArt home page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyrightInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://componentart.com", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show("bad URL: " + ex.Message);
            }
        }
        #endregion
		#region DependencyProperties
		
        #region VersionText

        /// <summary> 
        /// Gets or sets the VersionText possible Value of the string object.
        /// </summary> 
        public string VersionText
        {
            get { return (string)GetValue(VersionTextProperty); }
            set { SetValue(VersionTextProperty, value); }
        }

        /// <summary> 
        /// Identifies the VersionText dependency property.
        /// </summary> 
        public static readonly DependencyProperty VersionTextProperty =
                    DependencyProperty.Register(
                          "VersionText",
                          typeof(string),
                          typeof(Page),
                          new PropertyMetadata(OnVersionTextPropertyChanged));

        /// <summary>
        /// VersionTextProperty property changed handler. 
        /// </summary>
        /// <param name="d">PageNew that changed its VersionText.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnVersionTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Page _Page = d as Page;
            if (_Page != null)
            {
                //TODO: Handle new value. 
            }
        }
        #endregion VersionText

        #endregion
		
    }
}
