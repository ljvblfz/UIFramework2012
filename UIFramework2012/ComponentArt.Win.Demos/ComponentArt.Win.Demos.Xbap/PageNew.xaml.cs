using ComponentArt.Win.UI.Utils;
using ComponentArt.Win.UI.Navigation;
using System;
using System.Collections.Generic;
// using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
// using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Xml;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Interop;
using System.Windows.Navigation;
using System.Windows.Documents;

namespace ComponentArt.Win.Demos
{
    #region Parts and States Contract
    [TemplatePart(Name = PageNew.Part_LayoutRoot, Type = typeof(Panel))]
    [TemplatePart(Name = PageNew.Part_HeaderBorder, Type = typeof(Border))]
    [TemplatePart(Name = PageNew.Part_MenuBorder, Type = typeof(Border))]
    [TemplatePart(Name = PageNew.Part_Menu, Type = typeof(ComponentArt.Win.UI.Navigation.Menu))]
    [TemplatePart(Name = PageNew.Part_ContentHeaderBorder, Type = typeof(Border))]
    [TemplatePart(Name = PageNew.Part_ShowCodeButton, Type = typeof(Button))]
    [TemplatePart(Name = PageNew.Part_HeaderImage, Type = typeof(Image))]
    [TemplatePart(Name = PageNew.Part_ContentBorder, Type = typeof(Border))]
    [TemplatePart(Name = PageNew.Part_ContentPane, Type = typeof(ContentControl))]
    [TemplatePart(Name = PageNew.Part_ContentPaneDropShadow, Type = typeof(Image))]
    [TemplatePart(Name = PageNew.Part_DemoBorder, Type = typeof(Border))]
    [TemplatePart(Name = PageNew.Part_TreeViewBorder, Type = typeof(Border))]
    [TemplatePart(Name = PageNew.Part_TreeView, Type = typeof(ComponentArt.Win.UI.Navigation.TreeView))]
    [TemplatePart(Name = PageNew.Part_DescriptionBorder, Type = typeof(Border))]
    [TemplatePart(Name = PageNew.Part_FooterBorder, Type = typeof(Border))]
    [TemplatePart(Name = PageNew.Part_VersionTextBlock, Type = typeof(TextBlock))]
    [TemplateVisualState(Name = PageNew.State_PageType_Index, GroupName = PageNew.Group_PageType)]
    [TemplateVisualState(Name = PageNew.State_PageType_Integration, GroupName = PageNew.Group_PageType)]
    [TemplateVisualState(Name = PageNew.State_PageType_Demo, GroupName = PageNew.Group_PageType)]
    [TemplateVisualState(Name = PageNew.State_LocationType_Live, GroupName = PageNew.Group_LocationType)]
    [TemplateVisualState(Name = PageNew.State_LocationType_Local, GroupName = PageNew.Group_LocationType)]
    [TemplateVisualState(Name = PageNew.State_CodeButtonVisible, GroupName = PageNew.Group_ShowCodeButtonVisibility)]
    [TemplateVisualState(Name = PageNew.State_CodeButtonCollapsed, GroupName = PageNew.Group_ShowCodeButtonVisibility)]
    [TemplateVisualState(Name = PageNew.State_InitialStateNormal, GroupName = PageNew.Group_InitialStateGroup)]
    [TemplateVisualState(Name = PageNew.State_InitialStateFadeIn, GroupName = PageNew.Group_InitialStateGroup)]
    #endregion
    public partial class PageNew : UserControl
    {
        #region Parts and States
        //Parts
        private const string Part_LayoutRoot = "LayoutRoot";
        private const string Part_IntroLogo = "IntroLogoAnimation";
        private const string Part_IntroMenuBar = "IntroMenuBarAnimation";
        private const string Part_IntroContent = "IntroContentAnimation";
        private const string Part_HeaderBorder = "HeaderBorder";
        private const string Part_MenuBorder = "MenuBorder";
        private const string Part_Menu = "DemoMenu";
        private const string Part_ContentHeaderBorder = "ContentHeaderBorder";
        private const string Part_ShowCodeButton = "ShowCodeButton";
        private const string Part_HeaderImage = "HeaderImage";
        private const string Part_ContentBorder = "ContentBorder";
        private const string Part_DemoBorder = "DemoBorder";
        private const string Part_ContentSpinner = "ContentSpinner";
        private const string Part_ContentPane = "ContentPane";
        private const string Part_ContentPaneDropShadow = "ContentPaneDropShadow";
        private const string Part_TreeViewBorder = "TreeViewBorder";
        private const string Part_TreeView = "DemoTreeView";
        private const string Part_DescriptionBorder = "DescriptionBorder";
        private const string Part_FooterBorder = "FooterBorder";
        private const string Part_VersionTextBlock = "VersionTextBlock";

        // State Groups & States
        private const string Group_PageType = "PageTypeGroup";
        private const string State_PageType_Index = "PageTypeIndex";
        private const string State_PageType_Integration = "PageTypeIntegration";
        private const string State_PageType_Demo = "PageTypeDemo";

        private const string Group_LocationType = "LocationTypeGroup";
        private const string State_LocationType_Live = "LocationTypeLive";
        private const string State_LocationType_Local = "LocationTypeLocal";

        private const string Group_ShowCodeButtonVisibility = "ShowCodeButtonVisibility";
        private const string State_CodeButtonVisible = "CodeButtonVisible";
        private const string State_CodeButtonCollapsed = "CodeButtonCollapsed";

        private const string Group_InitialStateGroup = "InitialStateGroup";
        private const string State_InitialStateNormal = "InitialStateNormal";
        private const string State_InitialStateFadeIn = "InitialStateFadeIn";
        #endregion

        #region Part Instances
        private ComponentArt.Win.UI.Navigation.TreeView _treeView;
        private ComponentArt.Win.UI.Navigation.Menu _menu;
        private ContentControl _contentPane;
        private Image _contentPaneDropShadow;
        private Grid _layoutRoot;
        private Image _headerImage;
        private Button _showCodeButton;
        private Border _treeViewBorder;
        private Border _demoBorder;
        private IntroLogo _introLogo;
        private IntroMenuBar _introMenuBar;
        private IntroContent _introContent;
        #endregion

        #region Properties
        public bool debug = false;

        private string _demoName;
        private string _codeFiles;
        private string _codeviewerDevPath = "http://v3.corp.componentart.com/common/codeviewer/default.aspx?files=";
        private string _codeviewerLivePath = "http://www.componentart.com/common/codeviewer/default.aspx?files=";
        private string _devPath = @"c:\site\silverlight\";
        private string _livePath = @"e:\inetpub\silverlight\";
        private string _demoPath = "pack://application:,,,/Assets/Demo/";
        private string _demoXmlPath = "pack://application:,,,/Assets/Demo/TreeViewXml/";
        private string _menuXmlPath = "pack://application:,,,/Assets/Demo/Menu/MainMenu.xml";
        private string _xmlDecl = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n";
        private string _demoXml;
        private string _currentVisualState = State_PageType_Index; // [State_PageType_Index,State_PageType_Integration,State_PageType_Demo]
        private string _defaultDemo = "Welcome";
        internal bool _firstLoad = true;
        private List<string> _indexPages = new List<string>(); // pages which get special animations.
        private bool _doNotSelectTreeNode = false; // enable usage of same demo in multiple treeview categories.
        private Window nw;
        #endregion

        #region Startup & Initialization
        public PageNew()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(PageNew_Loaded);
            _currentVisualState = State_PageType_Index;
            _indexPages.Add("Welcome");
            _indexPages.Add("BestOfBreed_DataGrid");
            _indexPages.Add("BestOfBreed_Editors");
            _indexPages.Add("BestOfBreed_Navigation");

            nw = Application.Current.MainWindow;

        }

        internal void BeginLogoIntro()
        {
            _introLogo.Visibility = Visibility.Visible;
            _introLogo.BeginAnimation();
            VisualStateManager.GoToState(this, "InitialStateLogoFadeIn", true);
        }
        internal void IntroLogoComplete()
        {
            BeginMenuBarIntro();
        }

        internal void BeginMenuBarIntro()
        {
            _introMenuBar.Visibility = Visibility.Visible;
            _introMenuBar.BeginAnimation();
        }
        internal void IntroMenuBarComplete()
        {
            VisualStateManager.GoToState(this, "InitialStateMenuFadeIn", true);
            BeginContentIntro();
        }

        internal void BeginContentIntro()
        {
            _introContent.Visibility = Visibility.Visible;
            _introContent.BeginAnimation();
        }

        internal void IntroContentComplete()
        {
            VisualStateManager.GoToState(this, "InitialStateContentFadeIn", true);
            ((ScrollViewer)FindName("PageScrollViewer")).VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        internal void ResetIntroAnimation()
        {
            _introContent.ResetStoryboard();
            _introMenuBar.ResetStoryboard();
            _introLogo.ResetStoryboard();
            _introContent.Visibility = Visibility.Collapsed;
            _introMenuBar.Visibility = Visibility.Collapsed;
            _introContent.Visibility = Visibility.Collapsed;
            _introMenuBar.Visibility = Visibility.Collapsed;
            _introLogo.Visibility = Visibility.Collapsed;
            _firstLoad = true;
        }
        /// <summary>
        /// Set up Menu, set version and load initial demo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PageNew_Loaded(object sender, RoutedEventArgs e)
        {
            // Binding for the Version number.
            VersionTextBlock.DataContext = this;

            // Check for and generate references to parts.
            GetPartReferences();

            _introLogo.pn = _introMenuBar.pn = _introContent.pn = this;
            string initialDemo = GetInitialDemo();

            if (_indexPages.Contains(initialDemo))
            {
                ((Button)FindName("WebUiLogoButton")).Opacity = 0;
                ((Border)FindName("MenuBorder")).Opacity = 0;
                ((Border)FindName("MainBorder")).Opacity = 0;
                ((Border)FindName("FooterBorder")).Opacity = 0;
                ((ScrollViewer)FindName("PageScrollViewer")).VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                _contentPaneDropShadow.Opacity = 0;
            }
            else
            {
                _firstLoad = false;
            }

            ConfigureMenu();

            // do either LoadControlDemo, LoadIndexPage, LoadIntegrationDemo
            Dispatcher.BeginInvoke((Action)delegate { LoadDemo(initialDemo); });
        }

        private void ConfigureMenu()
        {
            // Configure the Menu.
            String menuXml = "";
            StreamReader streader = App.GetResourceStreamReader(_menuXmlPath);
            using (XmlReader reader = XmlReader.Create(streader, null))
            {
                reader.MoveToContent();
                menuXml = reader.ReadOuterXml().ToString();
            }

            _menu.XmlBindingSource = _xmlDecl + menuXml;

            foreach (ComponentArt.Win.UI.Navigation.MenuItem mi in _menu.Items)
            {
                mi.Text = mi.Id;
            }
        }

        /// <summary>
        /// Determine what the initial demo will be based on URL.
        /// </summary>
        /// <returns></returns>
        private string GetInitialDemo()
        {
            // string sStartDemo = System.Windows.Browser.HtmlPage.Window.CurrentBookmark;
            string sStartDemo = "";
            if (sStartDemo == "")
            {
                return _defaultDemo;
            }
            else
            {
                return sStartDemo;
            }
        }

        /// <summary>
        /// Get reference to TreeView, Menu, Content and LayoutRoot.
        /// </summary>
        private void GetPartReferences()
        {
            _treeView = (ComponentArt.Win.UI.Navigation.TreeView)FindName(Part_TreeView);
            _menu = (ComponentArt.Win.UI.Navigation.Menu)FindName(Part_Menu);
            _contentPane = (ContentControl)FindName(Part_ContentPane);
            _contentPaneDropShadow = (Image)FindName(Part_ContentPaneDropShadow);
            _layoutRoot = (Grid)FindName(Part_LayoutRoot);
            _headerImage = (Image)FindName(Part_HeaderImage);
            _showCodeButton = (Button)FindName(Part_ShowCodeButton);
            _demoBorder = (Border)FindName(Part_DemoBorder);
            _treeViewBorder = (Border)FindName(Part_TreeViewBorder);
            _introLogo = (IntroLogo)FindName(Part_IntroLogo);
            _introMenuBar = (IntroMenuBar)FindName(Part_IntroMenuBar);
            _introContent = (IntroContent)FindName(Part_IntroContent);
            if (_treeView == null || _menu == null
                  || _contentPane == null || _layoutRoot == null
                    || _headerImage == null || _showCodeButton == null
                      || _demoBorder == null || _treeViewBorder == null
                        || _contentPaneDropShadow == null)
            {
                throw new Exception("Invalid XAML");
            }
        }
        #endregion

        #region Header

        private void WebUiLogoButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDemo(_defaultDemo);
        }

        #region Menu
        private int _menuOpened = 0;
        private void DemoMenu_BeforeItemShow(object sender, MenuCancelEventArgs mcea)
        {
            _menuOpened++;
            if (_contentPane.Content is IDisableable)
            {
                _contentPane.IsEnabled = false;
            }
        }

        private void DemoMenu_ItemShow(object sender, MenuCommandEventArgs mce)
        {
            _menuOpened = 1;
            if (_contentPane.Content is IDisableable)
            {
                _contentPane.IsEnabled = false;
            }
        }

        private void DemoMenu_ItemHide(object sender, MenuCommandEventArgs mce)
        {
            _menuOpened--;
            if (_contentPane.Content is IDisableable && _menuOpened == 0)
            {
                _contentPane.IsEnabled = true;
            }
        }

        private void DemoMenu_MenuClick(object sender, ComponentArt.Win.UI.Navigation.MenuCommandEventArgs mce)
        {
            if (mce.ItemSource.Id != null)
            {
                string action = mce.ItemSource.Id.Split(new char[] { ' ' })[0];
                string target = mce.ItemSource.Id.Split(new char[] { ' ' })[1];

                NavigateToDemo(action, target);
            }
        }

        // for the Individual Controls menu.
        private void DemoMenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Hwan
            //((ComponentArt.Win.UI.Navigation.MenuItem)DemoMenu.Items[8]).IsOpened = false;

            if (((Button)sender).Tag != null)
            {
                string action = ((Button)sender).Tag.ToString().Split(new char[] { ' ' })[0];
                string target = ((Button)sender).Tag.ToString().Split(new char[] { ' ' })[1];
                NavigateToDemo(action, target);
            }
        }
        #endregion

        /// <summary>
        /// Takes a Control name and loads the first demo for it.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="target"></param>
        internal void NavigateToDemo(string action, string target)
        {
            switch (action)
            {
                case "control":
                    {
                        // update header image.
                        LoadHeaderImage(_demoPath + "ControlLogos/" + target.ToLower() + "-wpf.png");

                        // Load the Control XML for the TreeView.
                        string xmlUrl = _demoXmlPath + target + ".xml";
                        SetTreeForControlDemo(xmlUrl);

                        // SelectTreeNode will LoadDemo.
                        TreeViewNode tvn = FindFirstDemo(_treeView);
                        if (tvn != null)
                        {
                            SelectTreeNode(tvn.Tag.ToString());
                        }

                        break;
                    }
                case "integration":
                    {
                        LoadHeaderImage(_demoPath + "PageTitle-SilverlightLiveDemos.png");
                        LoadDemo(target);
                        break;
                    }
                case "technology":
                    {
                        LoadHeaderImage(_demoPath + "PageTitle-SilverlightLiveDemos.png");
                        LoadDemo(target);
                        break;
                    }
                case "design":
                    {
                        LoadHeaderImage(_demoPath + "PageTitle-SilverlightLiveDemos.png");
                        LoadDemo(target);
                        break;
                    }
                case "bob":
                    {
                        LoadHeaderImage(_demoPath + "PageTitle-SilverlightLiveDemos.png");
                        LoadDemo("BestOfBreed_" + target);
                        break;
                    }
            }
        }
        #endregion

        #region TreeView
        /// <summary>
        /// Set the selected node for TreeView.
        /// </summary>
        /// <param name="nodeTag"></param>
        public void SelectTreeNode(string nodeTag)
        {
            foreach (TreeViewNode node in _treeView.WalkBreadthFirst())
            {
                if ((string)node.Tag == nodeTag)
                {
                    node.IsSelected = true;
                    TreeViewNode tNode = node;
                    while (tNode.Parent is TreeViewNode)
                    {
                        tNode = tNode.Parent as TreeViewNode;
                        tNode.IsExpanded = true;
                    }
                    break;
                }
            }

        }

        /// <summary>
        /// TreeView has a new node selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DemoTreeView_NodeSelected(object sender, TreeViewNodeMouseEventArgs e)
        {
            string d = (string)e.Node.Tag;
            if (d != "" && d != null) SwitchToDemo(d);
        }

        /// <summary>
        /// Page is already Demo and a control section has been loaded.
        /// This is only for use by the TreeView.
        /// </summary>
        /// <param name="sDemo"></param>
        public void SwitchToDemo(string sDemo)
        {
            if (_demoName == sDemo)
            {
                return;
            }
            _demoName = sDemo;
            _doNotSelectTreeNode = true;
            LoadDemo(_demoName);
        }

        private void SetTreeForControlDemo(string xmlUrl)
        {
            StreamReader streader = App.GetResourceStreamReader(xmlUrl);
            using (XmlReader reader = XmlReader.Create(streader, null))
            {
                reader.MoveToContent();
                _demoXml = reader.ReadOuterXml().ToString();
            }
            _treeView.XmlBindingSource = _xmlDecl + _demoXml;
        }

        #endregion

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
                // System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://componentart.com", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show("bad URL: " + ex.Message);
            }
        }
        #endregion

        #region API Methods

        /// <summary>
        /// Set the state of PageNew to State_PageType_Index,State_PageType_Integration or State_PageType_Demo.
        /// </summary>
        /// <param name="useTransitions"></param>
        private void ChangeVisualState(bool useTransitions)
        {
            // Page template
            VisualStateManager.GoToState(this, _currentVisualState, true);

            // Tab context
            if (IsRunLive())
            {
                VisualStateManager.GoToState(this, State_LocationType_Live, false);
            }
            else
            {
                VisualStateManager.GoToState(this, State_LocationType_Local, false);
            }

            // Show Code Button
            if (IsRunLive() && _currentVisualState == State_PageType_Demo)
            {
                VisualStateManager.GoToState(this, State_CodeButtonVisible, false);
            }
            else
            {
                VisualStateManager.GoToState(this, State_CodeButtonCollapsed, false);
            }
        }

        /// <summary>
        /// Call dispose on current demo (if applicable).
        /// </summary>
        internal void ClearCurrentDemo()
        {
            if (!_doNotSelectTreeNode)
            {
                _treeView.SelectedNode = null;
            }
            if (_contentPane.Content != null && _contentPane.Content is IDisposable)
            {
                ((IDisposable)_contentPane.Content).Dispose();
            }
            _contentPane.IsEnabled = true;
        }

        /// <summary>
        /// Change the header image.
        /// </summary>
        /// <param name="sImage"></param>
        private void LoadHeaderImage(string sImage)
        {
            _headerImage.Source = (ImageSource)new BitmapImage(new Uri(sImage, UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Given a TreeView find the first demo in a control section.
        /// </summary>
        /// <param name="tv"></param>
        /// <returns></returns>
        private TreeViewNode FindFirstDemo(ComponentArt.Win.UI.Navigation.TreeView tv)
        {
            if (_treeView.Items.Count < 1) { return null; }
            TreeViewNode tvn = (TreeViewNode)_treeView.Items[0];

            if (tvn.Tag != null && tvn.Tag.ToString() != "")
            {
                return tvn;
            }
            else
            {
                return (TreeViewNode)tvn.Items[0];
            }
        }

        /// <summary>
        /// Load a page (Index, Integration or Demo) into the content area.
        /// </summary>
        /// <param name="sDemo"></param>
        public void LoadDemo(string sDemo)
        {
            if (sDemo != null && sDemo != "")
            {
                _demoName = sDemo;

                ClearCurrentDemo();
                _treeViewBorder.MaxHeight = double.PositiveInfinity;
                UserControl oDemoContent = null;
                if (debug)
                {
                    oDemoContent = Activator.CreateInstance(Type.GetType(this.GetType().Namespace + "." + sDemo)) as UserControl;
                }
                else
                {
                    try
                    {
                        oDemoContent = Activator.CreateInstance(Type.GetType(this.GetType().Namespace + "." + sDemo)) as UserControl;
                    }
                    catch (Exception e)
                    {
                        throw (e);
                        // couldn't find demo, go to welcome page:
                        /*_demoName = _defaultDemo;
                        oDemoContent = Activator.CreateInstance(Type.GetType(this.GetType().Namespace + "." + _demoName)) as UserControl;*/
                    }
                    oDemoContent.Loaded += new RoutedEventHandler(oDemoContent_Loaded);

                    oDemoContent.ApplyTemplate();
                }
                if (oDemoContent != null)
                {
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

                    // change page template based on DemoCategory.
                    TextBlock oCategoryBlock = (TextBlock)oDemoContent.FindName("DemoCategory");

                    if (oCategoryBlock != null)
                    {
                        switch (oCategoryBlock.Text)
                        {
                            case "Index":  // Index Template
                                {
                                    LoadHeaderImage(_demoPath + "PageTitle-SilverlightLiveDemos.png");
                                    _treeView.Visibility = Visibility.Collapsed;
                                    _currentVisualState = State_PageType_Index;
                                    break;
                                }
                            case "Integration":  // Integration Template
                                {
                                    LoadHeaderImage(_demoPath + "PageTitle-SilverlightLiveDemos.png");
                                    _treeView.Visibility = Visibility.Collapsed;
                                    _currentVisualState = State_PageType_Integration;
                                    break;
                                }
                            default:  // Demo Template
                                {
                                    _currentVisualState = State_PageType_Demo;
                                    _treeView.Visibility = Visibility.Visible;
                                    if (_doNotSelectTreeNode)
                                    {
                                        _doNotSelectTreeNode = false;
                                    }
                                    else
                                    {
                                        string xmlUrl = _demoXmlPath + oCategoryBlock.Text + ".xml";
                                        SetTreeForControlDemo(xmlUrl);
                                        SelectTreeNode(sDemo);
                                    }
                                    break;
                                }
                        }
                    }

                    ChangeVisualState(true);

                    TextBlock oDemoCodeFiles = (TextBlock)oDemoContent.FindName("DemoCodeFiles");

                    if (oDemoCodeFiles != null)
                    {
                        _codeFiles = oDemoCodeFiles.Text;
                    }
                    else
                    {
                        _codeFiles = null;
                    }

                    // load content
                    _contentPane.Content = oDemoContent;

                    // update bookmark
                    // NavigationService ns = NavigationService.GetNavigationService(this);

                    //nw.Navigate(BrowserInteropHelper.Source.AbsoluteUri + "#" + sDemo);
                    // System.Windows.Browser.HtmlPage.Window.NavigateToBookmark(sDemo);
                }
            }
        }


        void oDemoContent_Loaded(object sender, RoutedEventArgs e)
        {
            ((UserControl)sender).Loaded -= new RoutedEventHandler(oDemoContent_Loaded);

            _treeViewBorder.MaxHeight = _demoBorder.ActualHeight;

            _contentPane.IsEnabled = true;

            if (_firstLoad)
            {
                _firstLoad = false;
                BeginLogoIntro();
            }
        }

        /// <summary>
        /// Open new window with code view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string[] files = _codeFiles.Split(',');
            string filearray = "";
            string _viewerPath = _codeviewerDevPath;
            string _filePath = _devPath;

            // check for live
            if (IsRunLive())
            {
                _viewerPath = _codeviewerLivePath;
                _filePath = _livePath;
            }

            // force live ( remove for dev testing )
            _viewerPath = _codeviewerLivePath;
            _filePath = _livePath;

            for (int i = 0; i < files.Length; i++)
            {
                // trim and replace wrong slashes
                files[i] = files[i].Trim().Replace('/', '\\');

                // trim leading slash if found
                if (files[i].IndexOf('\\') == 0)
                    files[i] = files[i].Substring(1);

                // append proper path
                //filearray += _filePath + files[i] + ",";

                filearray += files[i] + ",";
            }

            // need to use hyperlink on the template for show code button instead. (perhaps can set the uri here... onpreviewleftmousedown? Hyperlink.TargetName="codeviewer")
            /*HtmlPage.Window.Invoke("open", new object[] {
                _viewerPath + filearray + "&site=silverlight",  
                "codeviewer", 
                "height=680,width=950,status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes"
              });*/
            // NavigationService ns = NavigationService.GetNavigationService(this);
        }

        private bool IsBrowserHosted()
        {
            if (BrowserInteropHelper.IsBrowserHosted)
            {
                return true;
            }
            return false;
        }

        private bool IsRunLive()
        {
            // check for live
            if (IsBrowserHosted() == true)
            {
                if (BrowserInteropHelper.Source.AbsoluteUri.Contains("componentart.com") ||
                    BrowserInteropHelper.Source.AbsoluteUri.Contains("c-art-app01"))
                {
                    return true;
                }
            }
            else // in Exe Window. (want the links.)
            {
                return false;
            }

            return false;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Hyperlink source = sender as Hyperlink;
                System.Diagnostics.Process.Start(source.NavigateUri.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
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
                          typeof(PageNew),
                          new PropertyMetadata(OnVersionTextPropertyChanged));

        /// <summary>
        /// VersionTextProperty property changed handler. 
        /// </summary>
        /// <param name="d">PageNew that changed its VersionText.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnVersionTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PageNew _PageNew = d as PageNew;
            if (_PageNew != null)
            {
            }
        }
        #endregion VersionText

        #endregion
    }
}
