using System;
using System.Collections.Generic;
// using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Resources;
using System.Windows.Threading;
using System.Windows.Interop;
using System.Windows.Navigation;

namespace ComponentArt.Win.Demos
{
  public partial class App : Application
  {
    private System.Diagnostics.Process cassiniProcess = null;
    private Grid root;
    private string startupUri;
    private ComponentArt.Win.UI.Utils.CommonPopup errorPopup;

    public App()
    {
      Startup += Application_Startup;
      Exit += Application_Exit;
      Navigating += Application_Navigating;
#if EXE
      Window mWindow = new MainWindow();
      mWindow.Show();
      ShutdownMode = ShutdownMode.OnMainWindowClose;
#endif
      InitializeComponent();
    }

    /// <summary>
    /// Handle Page Refresh.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Application_Navigating(object sender, NavigatingCancelEventArgs e)
    {
        if (e.NavigationMode == NavigationMode.Refresh)
        {
            // ((PageNew)root.Children[0]).ResetIntroAnimation();
            ((PageNew)root.Children[0]).LoadDemo("Welcome");
            e.Cancel = true;
        }
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;

      string exceptionText = "Exception:\n";
      if (e.Exception != null)
      {
        exceptionText += String.Format("{0}\n{1}", e.Exception.Message, e.Exception.StackTrace);
      }
      if (e.Exception.InnerException != null)
      {
		exceptionText += String.Format("\n\nInnerException:\n{0}\n{1}", e.Exception.InnerException.Message, e.Exception.InnerException.StackTrace);
	  }

      errorPopup = new ComponentArt.Win.UI.Utils.CommonPopup() 
      { 
          Height = Application.Current.MainWindow.ActualHeight,
          Width = Application.Current.MainWindow.ActualWidth,
          HorizontalAlignment = HorizontalAlignment.Stretch, 
          VerticalAlignment = VerticalAlignment.Stretch,
          AllowsTransparency=true
      };

      Grid errorGrid = new Grid()
      {
          Height = Application.Current.MainWindow.ActualHeight,
          Width = Application.Current.MainWindow.ActualWidth,
          HorizontalAlignment = HorizontalAlignment.Stretch,
          VerticalAlignment = VerticalAlignment.Stretch,
          Background = new SolidColorBrush(Color.FromArgb(0x7f, 0, 0, 0))
      };

      ErrorPanel ep = new ErrorPanel()
      {
          Height=450, Width=510
      };

      ep.ExceptionText.Text = exceptionText;
      errorGrid.Children.Add(ep);
      errorPopup.Child = errorGrid;

      root.Children.Add(errorPopup);

      errorPopup.IsOpen = true;
    }

    internal void errorBackButton_Click(object sender, RoutedEventArgs e)
    {
        ((Button)sender).Click -= errorBackButton_Click;
        errorPopup.IsOpen = false;
        errorPopup.Dispose();
        root.Children.Remove(errorPopup);

        ((PageNew)root.Children[0]).LoadDemo("Welcome");
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        DispatcherUnhandledException += App_DispatcherUnhandledException;
#if EXE
        Start_Application_Exe();
#elif XBAP
        Start_Application();
#endif
    }

#if EXE

    private void Start_Services_Callback(object state)
    {
      try
      {
        if (Application.Current.Properties["LocalWebUri"] == null)
        {
          CassiniLauncher oLauncher = new CassiniLauncher("");
          string sUrl = oLauncher.LaunchCassini();

          if (sUrl != null)
          {
            cassiniProcess = oLauncher.GetCassiniProcess();
            Application.Current.Properties["LocalWebUri"] = new Uri(sUrl, UriKind.Absolute);
          }
        }
      }
      catch
      {

      }
    }

    private void Start_Application_Exe()
    {
      Timer oTimer = new Timer(new TimerCallback(Start_Services_Callback), null, 1, Timeout.Infinite);

      root = (Grid)Application.Current.MainWindow.Content;

      root.SetValue(System.Windows.Automation.AutomationProperties.AutomationIdProperty, "ComponentArt.Win.Demos");

      PageNew pn = new PageNew();
      //pn.VersionText = "Version "+(String)HtmlPage.Window.Invoke("get_version");
      root.Children.Clear();
      root.Children.Add(pn);

      //if (HtmlPage.Document.QueryString.ContainsKey("debug"))
      //    ((PageNew)root.Children[0]).debug = true;
    }

#elif XBAP
        private void Start_Application()
        {
            UIElement uie_rv;
            uie_rv = ComponentArt.Win.UI.Utils.Tools.GetRootVisual();

            ContentControl ctrl_rv = null;
            if (uie_rv is ContentControl)
            {
                ctrl_rv = uie_rv as ContentControl;
            }

            root = new Grid();
            root.SetValue(System.Windows.Automation.AutomationProperties.AutomationIdProperty, "ComponentArt.Win.Demos");

            if (ctrl_rv != null)
            {
                ctrl_rv.Content = root;
            }

            PageNew pn = new PageNew();

            root.Children.Clear();
            root.Children.Add(pn);

            //if (HtmlPage.Document.QueryString.ContainsKey("debug"))
            //    ((PageNew)root.Children[0]).debug = true;
        }
#endif

    #region Click Handlers
    // for Hyperlink Buttons
    internal void LoadDemoHyper(object sender, RoutedEventArgs e)
    {
      LoadDemo(sender, e);
    }

    // for Regular Buttons
    internal void LoadDemo(object sender, RoutedEventArgs e)
    {
      if (((Button)sender).Tag != null)
      {
        string action = ((Button)sender).Tag.ToString().Split(new char[] { ' ' })[0];
        string target = ((Button)sender).Tag.ToString().Split(new char[] { ' ' })[1];

        ((PageNew)root.Children[0]).LoadDemo(target);
      }
    }

    // for Hyperlink Buttons
    internal void NavigateToDemoHyper(object sender, RoutedEventArgs e)
    {
      NavigateToDemo(sender, e);
    }

    // for Regular Buttons
    internal void NavigateToDemo(object sender, RoutedEventArgs e)
    {
      if (((Button)sender).Tag != null)
      {
        string action = ((Button)sender).Tag.ToString().Split(new char[] { ' ' })[0];
        string target = ((Button)sender).Tag.ToString().Split(new char[] { ' ' })[1];

        ((PageNew)root.Children[0]).NavigateToDemo(action, target);
      }
    }
    // for Radio Buttons
    internal void LoadTheme(object sender, RoutedEventArgs e)
    {
        if (((RadioButton)sender).Tag != null)
        {
            string action = ((RadioButton)sender).Tag.ToString().Split(new char[] { ' ' })[0];
            string target = ((RadioButton)sender).Tag.ToString().Split(new char[] { ' ' })[1];

            ((PageNew)root.Children[0]).NavigateToDemo("integration", target); // LoadDemo(target);
        }
    }
    #endregion

    #region Utility
    internal static StreamReader GetResourceStreamReader(string resourceFile)
    {
      StreamResourceInfo info;
      try
      {
        info = Application.GetResourceStream(new Uri(resourceFile, UriKind.RelativeOrAbsolute));
      }
      catch (Exception)
      {
        throw new Exception("error creating streamresourceinfo.");
      }

      StreamReader streader;
      try
      {
        streader = new StreamReader(info.Stream);
      }
      catch (Exception)
      {
        throw new Exception("error creating stream reader. " + info.Stream.ToString());
      }
      return streader;
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

    private void Application_Exit(object sender, EventArgs e)
    {
#if EXE
      if (cassiniProcess != null)
      {
        cassiniProcess.Kill();
      }
      
      Application.Current.Shutdown();
#endif
    }
  }


  public class DemoWebHelper
  {
    public static Uri BaseUri
    {
      get
      {
        if (!System.Windows.Interop.BrowserInteropHelper.IsBrowserHosted && Application.Current.Properties["LocalWebUri"] != null)
        {
          return Application.Current.Properties["LocalWebUri"] as Uri;
        }

#if DEBUG
        return new Uri("http://localhost:1337/", UriKind.Absolute);
#endif

        return ComponentArt.Win.UI.Utils.Tools.ApplicationSource;
      }
    }
  }
}
