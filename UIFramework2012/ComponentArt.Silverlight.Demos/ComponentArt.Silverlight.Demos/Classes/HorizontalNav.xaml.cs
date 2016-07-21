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
using System.Collections.ObjectModel;

namespace ComponentArt.Silverlight.Demos
{
    /// <summary>
    /// Provides a BreadCrumb-Lite horizontal navigation control.
    /// Usage:
    ///   Add an item:
    ///   _horizontalNav.NavItems.Add(new NavItem("First Item"));
    ///   Remove an item:
    ///   _horizontalNav.NavItems.RemoveAt(1);
    ///   Select an item:
    ///   _horizontalNav.SelectedItem = _horizontalNav.NavItems[1];
    ///   Get selected item:
    ///   _horizontalNav.SelectedItem
    ///   Get selected index:
    ///   _horizontalNav.SelectedIndex
    /// </summary>
    public partial class HorizontalNav : UserControl
    {
        #region Startup & Shutdown
        public HorizontalNav()
        {
            NavItems = new ObservableCollection<NavItem>();
            InitializeComponent();
            NavItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(NavItems_CollectionChanged);
        }

        void NavItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GenerateNavItems();
            if (NavItems.Count > 0)
            {
                SelectedItem = NavItems[NavItems.Count() - 1];
            }
            else
            {
                SelectedItem = null;
            }
        }

        #endregion

        #region Events
        #region SelectionChanged

        /// <summary>
        /// Item was clicked
        /// </summary>
        public event RoutedEventHandler SelectedItemChanged;

        private void RaiseSelectedItemChanged()
        {
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(this, new RoutedEventArgs());
            }
        }

        #endregion

        #region ResetClicked

        /// <summary>
        /// Reset button was clicked.
        /// </summary>
        public event RoutedEventHandler ResetClicked;

        private void RaiseResetClicked()
        {
            if (ResetClicked != null)
            {
                ResetClicked(this, new RoutedEventArgs());
            }
        }

        #endregion

        #region SelectedIndexChanged

        /// <summary>
        /// SelectedItem changed.
        /// </summary>
        public event RoutedEventHandler SelectedIndexChanged;

        private void RaiseSelectedIndexChanged()
        {
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(this, new RoutedEventArgs());
            }
        }

        #endregion

        #endregion

        #region Private API

        /// <summary>
        /// Remove items trailing given index.
        /// </summary>
        /// <param name="SelectedIndex"></param>
        void RemoveItemsAfter(int idx)
        {
            while (NavItems.Count > idx + 1)
            {
                NavItems.RemoveAt(ItemsStackPanel.Children.Count - 1);
            }
        }

        void GenerateNavItems()
        {
            ItemsStackPanel.Children.Clear();
            foreach (NavItem item in NavItems)
            {
                GenerateNavItem(item);
            }
        }
        
        void SelectSelectedItem()
        {
            if (SelectedButton == null) { return; }
            foreach (UIElement uie in ItemsStackPanel.Children)
            {
                ((Button)uie).IsEnabled = true;
                Dispatcher.BeginInvoke(() =>
                    VisualStateManager.GoToState((Button)uie, "Normal", false));
            }
            SelectedButton.IsEnabled = false;
            Dispatcher.BeginInvoke(() =>
                VisualStateManager.GoToState(SelectedButton, "SelectionSelected", false));
        }

        void GenerateNavItem(NavItem item)
        {
            // make new button,
            Button btn = new Button();
            if (ItemsStackPanel.Children.Count > 0)
            {
                btn.Margin = new Thickness(-16, 0, 0, 0);
                if (Application.Current.Resources.Contains("NavItemButton"))
                {
                    btn.Template = (ControlTemplate)Application.Current.Resources["NavItemButton"];
                }
            }
            else
            {
                btn.Margin = new Thickness(0, 0, 0, 0);
                if (Application.Current.Resources.Contains("NavItemFirstButton"))
                {
                    btn.Template = (ControlTemplate)Application.Current.Resources["NavItemFirstButton"];
                }
            }
            btn.Content = item.Text.ToString();
            btn.DataContext = btn;
            btn.Tag = item;
            btn.Click += new RoutedEventHandler(btn_Click);

            ItemsStackPanel.Children.Add(btn);
        }
        
        int GetNavItemIndex(NavItem ni)
        {
            int i = 0;
            foreach (UIElement uie in ItemsStackPanel.Children)
            {
                if (((Button)uie).Tag == ni)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
        #endregion 

        #region Parts events callbacks
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            NavItem newNi = (NavItem)((Button)sender).Tag;
            RemoveItemsAfter(GetNavItemIndex(newNi));
            SelectedItem = newNi;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            NavItems.Clear();
            RaiseResetClicked();
        }
        #endregion

        #region DependencyProperties
        #region NavItems

        /// <summary> 
        /// Gets or sets the NavItems possible Value of the ObservableCollection<NavItem> object.
        /// </summary> 
        public ObservableCollection<NavItem> NavItems 
        {
            get { return (ObservableCollection<NavItem>)GetValue(NavItemsProperty); } 
            set { SetValue(NavItemsProperty, value); } 
        } 

        /// <summary> 
        /// Identifies the NavItems dependency property.
        /// </summary> 
        public static readonly DependencyProperty NavItemsProperty = 
                    DependencyProperty.Register(
                          "NavItems",
                          typeof(ObservableCollection<NavItem>), 
                          typeof(HorizontalNav), 
                          new PropertyMetadata(OnNavItemsPropertyChanged)); 

        /// <summary>
        /// NavItemsProperty property changed handler. 
        /// </summary>
        /// <param name="d">HorizontalNav that changed its NavItems.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnNavItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
        { 
          HorizontalNav _HorizontalNav = d as HorizontalNav; 
          if (_HorizontalNav!=null) 
          {
          }
        } 
        #endregion NavItems


        #region SelectedButton

        /// <summary> 
        /// Gets the SelectedButton possible Value of the Button object.  This is the visual equivalent of the SelectedItem.
        /// The SelectedItem is stored as the Tag value of the Button.
        /// </summary> 
        public Button SelectedButton
        {
            get { return (Button)GetValue(SelectedButtonProperty); }
            set { SetValue(SelectedButtonProperty, value); }
        }

        /// <summary> 
        /// Identifies the SelectedButton dependency property.
        /// </summary> 
        public static readonly DependencyProperty SelectedButtonProperty =
                    DependencyProperty.Register(
                          "SelectedButton",
                          typeof(Button),
                          typeof(HorizontalNav),
                          new PropertyMetadata(OnSelectedButtonPropertyChanged));

        /// <summary>
        /// SelectedButtonProperty property changed handler. 
        /// </summary>
        /// <param name="d">HorizontalNav that changed its SelectedButton.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnSelectedButtonPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HorizontalNav _HorizontalNav = d as HorizontalNav;
            if (_HorizontalNav != null)
            {
                //TODO: Handle new value. 
            }
        }
        #endregion SelectedButton


        #region SelectedItem

        /// <summary> 
        /// Gets or sets the SelectedItem possible Value of the NavItem object.
        /// </summary> 
        public NavItem SelectedItem
        {
            get { return (NavItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary> 
        /// Identifies the SelectedItem dependency property.
        /// </summary> 
        public static readonly DependencyProperty SelectedItemProperty =
                    DependencyProperty.Register(
                          "SelectedItem",
                          typeof(NavItem),
                          typeof(HorizontalNav),
                          new PropertyMetadata(OnSelectedItemPropertyChanged));

        /// <summary>
        /// SelectedItemProperty property changed handler. 
        /// </summary>
        /// <param name="d">HorizontalNav that changed its SelectedItem.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HorizontalNav _HorizontalNav = d as HorizontalNav;
            if (_HorizontalNav != null)
            {
                _HorizontalNav.SelectedIndex = _HorizontalNav.GetNavItemIndex(_HorizontalNav.SelectedItem);
                if (_HorizontalNav.SelectedIndex < 0)
                {
                    _HorizontalNav.SelectedButton = null;
                }
                else
                {
                    _HorizontalNav.RemoveItemsAfter(_HorizontalNav.SelectedIndex);
                    _HorizontalNav.SelectedButton = (Button)_HorizontalNav.ItemsStackPanel.Children[_HorizontalNav.SelectedIndex];
                    _HorizontalNav.Dispatcher.BeginInvoke(() =>
                        _HorizontalNav.SelectSelectedItem());
                }
                _HorizontalNav.RaiseSelectedItemChanged();
            }
        }
        #endregion SelectedItem


        #region SelectedIndex

        /// <summary> 
        /// Gets the SelectedIndex possible Value of the int object.  Returns -1 if SelectedItem is null.
        /// 
        /// </summary> 
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary> 
        /// Identifies the SelectedIndex dependency property.
        /// </summary> 
        public static readonly DependencyProperty SelectedIndexProperty =
                    DependencyProperty.Register(
                          "SelectedIndex",
                          typeof(int),
                          typeof(HorizontalNav),
                          new PropertyMetadata(OnSelectedIndexPropertyChanged));

        /// <summary>
        /// SelectedIndexProperty property changed handler. 
        /// </summary>
        /// <param name="d">HorizontalNav that changed its SelectedIndex.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnSelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HorizontalNav _HorizontalNav = d as HorizontalNav;
            if (_HorizontalNav != null)
            {
                _HorizontalNav.RaiseSelectedIndexChanged();
            }
        }
        #endregion SelectedIndex

        #endregion
    }

    #region NavItem
    public class NavItem : DependencyObject
    {
        public NavItem()
        {
        }
        public NavItem(string text)
        {
            Text = text;
        }
        public NavItem(string text, object tag)
        {
            Text = text;
            Tag = tag;
        }

        #region Text

        /// <summary> 
        /// Gets or sets the Text possible Value of the String object.
        /// </summary> 
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary> 
        /// Identifies the Text dependency property.
        /// </summary> 
        public static readonly DependencyProperty TextProperty =
                    DependencyProperty.Register(
                          "Text",
                          typeof(String),
                          typeof(NavItem),
                          new PropertyMetadata(OnTextPropertyChanged));

        /// <summary>
        /// TextProperty property changed handler. 
        /// </summary>
        /// <param name="d">NavItem that changed its Text.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NavItem _NavItem = d as NavItem;
            if (_NavItem != null)
            {
                //TODO: Handle new value. 
            }
        }
        #endregion Text


        #region Tag

        /// <summary> 
        /// Gets or sets the Tag possible Value of the object object.
        /// </summary> 
        public object Tag
        {
            get { return (object)GetValue(TagProperty); }
            set { SetValue(TagProperty, value); }
        }

        /// <summary> 
        /// Identifies the Tag dependency property.
        /// </summary> 
        public static readonly DependencyProperty TagProperty =
                    DependencyProperty.Register(
                          "Tag",
                          typeof(object),
                          typeof(NavItem),
                          new PropertyMetadata(OnTagPropertyChanged));

        /// <summary>
        /// TagProperty property changed handler. 
        /// </summary>
        /// <param name="d">NavItem that changed its Tag.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnTagPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NavItem _NavItem = d as NavItem;
            if (_NavItem != null)
            {
                
            }
        }
        #endregion Tag


    }
    #endregion
}
