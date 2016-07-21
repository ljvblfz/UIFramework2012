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
using System.Windows.Browser;
using System.Diagnostics;
using System.Reflection;

namespace ComponentArt.Silverlight.Demos {
	public partial class App : Application {

        private Grid root;

		public App() {
			this.Startup += this.Application_Startup;
			this.Exit += this.Application_Exit;
			this.UnhandledException += this.Application_UnhandledException;

			InitializeComponent();
		}

		private void Application_Startup( object sender, StartupEventArgs e ) {
            root = new Grid();
            this.RootVisual = root;

            PageNew pn = new PageNew();
            pn.VersionText = "Version "+(String)HtmlPage.Window.Invoke("get_version");
            root.Children.Add(pn);

            if (HtmlPage.Document.QueryString.ContainsKey("debug"))
                ((PageNew)root.Children[0]).debug = true;
        }
		
        #region Click Handlers
        // for Hyperlink Buttons
        internal void LoadDemoHyper(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).Tag != null)
            {
                string action = ((HyperlinkButton)sender).Tag.ToString().Split(new char[] { ' ' })[0];
                string target = ((HyperlinkButton)sender).Tag.ToString().Split(new char[] { ' ' })[1];
                Grid rv = (Grid)Application.Current.RootVisual;
                ((PageNew)rv.Children[0]).LoadDemo(target);
            }
        }

        // for Regular Buttons
        internal void LoadDemo(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                string action = ((Button)sender).Tag.ToString().Split(new char[] { ' ' })[0];
                string target = ((Button)sender).Tag.ToString().Split(new char[] { ' ' })[1];
                Grid rv = (Grid)Application.Current.RootVisual;
                ((PageNew)rv.Children[0]).LoadDemo(target);
            }
        }

        // for Hyperlink Buttons
        internal void NavigateToDemoHyper(object sender, RoutedEventArgs e)
        {
            if (((HyperlinkButton)sender).Tag != null)
            {
                string action = ((HyperlinkButton)sender).Tag.ToString().Split(new char[] { ' ' })[0];
                string target = ((HyperlinkButton)sender).Tag.ToString().Split(new char[] { ' ' })[1];
                Grid rv = (Grid)Application.Current.RootVisual;
                ((PageNew)rv.Children[0]).NavigateToDemo(action, target);
            }
        }

        // for Regular Buttons
        internal void NavigateToDemo(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                string action = ((Button)sender).Tag.ToString().Split(new char[] { ' ' })[0];
                string target = ((Button)sender).Tag.ToString().Split(new char[] { ' ' })[1];
                Grid rv = (Grid)Application.Current.RootVisual;
                ((PageNew)rv.Children[0]).NavigateToDemo(action, target);
            }
        }

		// for Radio Buttons
		internal void LoadTheme(object sender,RoutedEventArgs e) 
        {
			if (((RadioButton) sender).Tag != null) {
				string action = ((RadioButton) sender).Tag.ToString().Split(new char[] { ' ' })[0];
				string target = ((RadioButton) sender).Tag.ToString().Split(new char[] { ' ' })[1];
				Grid rv = (Grid) Application.Current.RootVisual;
                ((PageNew)rv.Children[0]).NavigateToDemo("integration", target); // LoadDemo(target);
			}
		}
        #endregion

        #region FeedBrowser
        /// <summary>
        /// Custom RollOvers for CustomTemplate MenuItems (See Application.Resources)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomMenuTemplate_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x66, 0x66, 0x66));
            TextBlock tb = ((Border)sender).FindName("CustomHeading") as TextBlock;
            tb.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x00, 0x00, 0x00));
            LinearGradientBrush lgb = new LinearGradientBrush();
            if (((Border)sender).Resources.Contains("ComponentArt_Menu_ItemBackgroundOver"))
            {
                lgb = ((Border)sender).Resources["ComponentArt_Menu_ItemBackgroundOver"] as LinearGradientBrush;
            }
            ((Border)sender).Background = lgb;
        }

        private void CustomMenuTemplate_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            TextBlock tb = ((Border)sender).FindName("CustomHeading") as TextBlock;
            tb.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0x40, 0x40, 0x40));
            ((Border)sender).Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
        }

        #endregion

        private void Application_Exit( object sender, EventArgs e ) 
        {

		}

		private void Application_UnhandledException( object sender, ApplicationUnhandledExceptionEventArgs e ) {
			// If the app is running outside of the debugger then report the exception using
			// the browser's exception mechanism. On IE this will display it a yellow alert 
			// icon in the status bar and Firefox will display a script error.
			if (!System.Diagnostics.Debugger.IsAttached) {

				// NOTE: This will allow the application to continue running after an exception has been thrown
				// but not handled. 
				// For production applications this error handling should be replaced with something that will 
				// report the error to the website and stop the application.
#if DEBUG
                e.Handled = true;
                Debug.Assert(false);
#else
				e.Handled = true;
				Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
#endif
			}
		}
		private void ReportErrorToDOM( ApplicationUnhandledExceptionEventArgs e ) {
			try {
				string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
				errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

				System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight 3 Application " + errorMsg + "\");");
			}
			catch (Exception) {
			}
		}
	}
}
