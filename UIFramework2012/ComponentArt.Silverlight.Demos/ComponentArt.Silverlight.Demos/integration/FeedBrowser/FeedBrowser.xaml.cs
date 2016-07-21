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
using ComponentArt.Silverlight.UI.Navigation;
using ComponentArt.Silverlight.UI.Utils;
using ComponentArt.Silverlight.UI.Data;
using System.Diagnostics;

namespace ComponentArt.Silverlight.Demos
{

  public partial class FeedBrowser : UserControl
  {
    private int _feedHistoryPosition = -1;
    private List<FeedDataGridView> _feedHistory = new List<FeedDataGridView>();
    private FeedDataGridView _defaultView = null;
    private FeedDataGridView _currentView = null;

    #region Page Methods
    public FeedBrowser()
    {
      InitializeComponent();

      _defaultView = new FeedDataGridView();
      _defaultView.Sortings.Add(new DataGridSorting(FeedDataGrid.Columns["DatePosted"], SortDirection.Descending));
      _currentView = _defaultView.Clone();
    }

    #endregion

    #region View and History methods

    public void NavigateBack()
    {
      PreviousMenu.HideContextMenu();
      NextMenu.HideContextMenu();
      if (_feedHistory.Count > _feedHistoryPosition+1 )
      {
        _feedHistoryPosition++;
      
        LoadView(_feedHistory[_feedHistoryPosition], false);
      }
      CreateNextView();
      CreatePrevView();
    }

    public void NavigateForward()
    {
      PreviousMenu.HideContextMenu();
      NextMenu.HideContextMenu();

      if (_feedHistoryPosition > 0)
      {
        _feedHistoryPosition--;

        LoadView(_feedHistory[_feedHistoryPosition], false);
      }
      CreateNextView();
      CreatePrevView();
    }
    
    public void NavigateToPosition( int position )
    {
        if (_feedHistory.Count > position)
        {
            _feedHistoryPosition = position;

            LoadView(_feedHistory[_feedHistoryPosition], false);
        }
        CreateNextView();
        CreatePrevView();
    }

    public void LoadView(FeedDataGridView oView)
    {
      LoadView(oView, true);
    }

    public void LoadView(FeedDataGridView oView, bool bAddToHistory)
    {
      // clear any previously loaded groups and items
      FeedDataGrid.LoadedGroups.Clear();
      FeedDataGrid.LoadedRows.Clear();

      // clear selections
      FeedDataGrid.SelectedIndices.Clear();

      // reload groupings
      FeedDataGrid.Groupings.Clear();
      foreach (ComponentArt.Silverlight.UI.Data.DataGridGrouping oGrouping in oView.Groupings)
      {
        FeedDataGrid.Groupings.Add(oGrouping);
      }

      // reload sortings
      FeedDataGrid.Sortings.Clear();
      foreach (DataGridSorting oSorting in oView.Sortings)
      {
        FeedDataGrid.Sortings.Add(oSorting);
      }

      // reload filters
      FeedDataGrid.Filters.Clear();
      foreach (DataGridDataCondition oFilter in oView.Filters)
      {
        FeedDataGrid.Filters.Add(oFilter);
      }

      // sync column visibility
      if (oView.Columns.Count > 0)
      {
        foreach (DataGridColumn oColumn in FeedDataGrid.Columns)
        {
          oColumn.Visibility = oView.Columns.Contains(oColumn.ColumnName) ? Visibility.Visible : Visibility.Collapsed;
        }
      }

      // reload
      FeedDataGrid.RecordOffset = 0;
      FeedDataGrid.Refresh();
      FeedDataGrid.Reload();

      // clean up
      ReactToViewChange(oView, bAddToHistory);
    }

    public void ReactToViewChange(FeedDataGridView oView, bool bAddToHistory)
    {
      MenuLoadView(oView);

      // add old view to history and update current pointer
      if (bAddToHistory)
      {
        NextMenu.HideContextMenu();
        PreviousMenu.HideContextMenu();
        _feedHistory.Insert(0, _currentView);
        _feedHistoryPosition = -1;
        _currentView = oView;
        CreateNextView();
        CreatePrevView();
      }
    }

    #endregion

    #region Menu Methods
    private void FeedMenu_MenuClick(object sender, ComponentArt.Silverlight.UI.Navigation.MenuCommandEventArgs mce)
    {
      if (mce.ItemSource.Id == null) { return; }

        string[] test = mce.ItemSource.Id.Split(new char[] { '_' });

        // choose what to do:
        switch (test[0])
        {
            case "show":
                FeedDataGrid.Columns[test[1]].Visibility = mce.ItemSource.IsChecked ? Visibility.Visible : Visibility.Collapsed;
                FeedDataGrid.Refresh();
                break;
            case "group":
                if (test.Length > 2)
                {
                    FeedMenuGroup(int.Parse(test[1]), int.Parse(test[2]), test.Length > 3 ? test[3] : "a", mce.ItemSource.IsChecked);
                }
                CheckParentItems(test);
                ClearMenu(test);
                break;
            case "sort":
                if (test.Length > 2)
                {
                    FeedMenuSort(int.Parse(test[1]), int.Parse(test[2]), test.Length > 3 ? test[3] : "a", mce.ItemSource.IsChecked);
                    CheckParentItems(test);
                    ClearMenu(test);
                }
                break;
            case "feature":
                switch (test[1])
                {
                  case "1":
                    ShowFeaturedView1();
                    break;
                  case "2":
                    ShowFeaturedView2();
                    break;
                  case "3":
                    ShowFeaturedView3();
                    break;
                  default:
                    break;
                }
                break;
            case "filter":

                for (int i = 0; i < FeedItemFlow.Items.Count; i++)
                {
                  if (((ItemFlowItem)FeedItemFlow.Items[i]).Text == mce.ItemSource.Text)
                  {
                    FeedItemFlow.MoveTo(i);
                    break;
                  }
                }

                FeedDataGridView oView = new FeedDataGridView(); // _currentView.Clone();
                if (test[1] != "allfeeds")
                    oView.Filters.Add(new DataGridDataCondition("FeedName", mce.ItemSource.Text));
                oView.FilterText = "FeedName = " + mce.ItemSource.Text;

                LoadView(oView);
                break;
            case "clear":
                FeedDataGrid.UnGroup();
                ClearMenu(new string[] { test[1], "0" });
                break;
            default:
                break;
        }
    }

    private void MenuLoadView(FeedDataGridView oView)
    {
        Dictionary<string, int> TitleToMenuId = new Dictionary<string,int>();
        TitleToMenuId.Add("FeedName", 1 );
        TitleToMenuId.Add("Title", 2 );
        TitleToMenuId.Add("DatePosted", 3);
        TitleToMenuId.Add("Visits", 4 );
        TitleToMenuId.Add("Rating", 5 );
        ClearMenu(new string[] { "group", "0" });
        ClearMenu(new string[] { "sort", "0" });
        int i = 0;
        foreach (ComponentArt.Silverlight.UI.Data.DataGridGrouping oGrouping in oView.Groupings)
        {
            i++;
            string[] groupset = { "group", i.ToString(), TitleToMenuId[oGrouping.Column.ColumnName].ToString(), i < 4 ? oGrouping.Direction == SortDirection.Ascending ? "a" : "d" : "" };
            FeedMenu.GetItemById(String.Join("_", groupset)).IsChecked = true;
            CheckParentItems(groupset);            
            ClearMenu(groupset);
        }
        i = 0;
        foreach (DataGridSorting oSorting in oView.Sortings)
        {
            i++;
            string[] sortset = { "sort", i.ToString(), TitleToMenuId[oSorting.Column.ColumnName].ToString(), i < 4 ? oSorting.Direction == SortDirection.Ascending ? "a" : "d" : "" };
            FeedMenu.GetItemById(String.Join("_", sortset)).IsChecked = true;
            CheckParentItems(sortset);
            ClearMenu(sortset);
        }

        foreach (DataGridColumn oColumn in FeedDataGrid.Columns)
        {
            FeedMenu.GetItemByText("show_" + oColumn.ColumnName).IsChecked = oColumn.Visibility == Visibility.Visible ? true : false;
        }
    }


    private void FeedMenu_Initialized(object sender, RoutedEventArgs e)
    {
        // set default stuff.
        ClearMenu(new string[] { "group", "0" });
        ClearMenu(new string[] { "sort", "0" });
    }

    private void FeedMenuGroup(int level, int col, string dir, bool ischecked)
    {
      FeedDataGridView oView = _currentView.Clone();

      while (oView.Groupings.Count >= level)
      {
        oView.Groupings.RemoveAt(level - 1);
      }

      if (ischecked)
      {
        oView.Groupings.Add(new ComponentArt.Silverlight.UI.Data.DataGridGrouping(FeedDataGrid.Columns[col - 1], dir == "d" ? SortDirection.Descending : SortDirection.Ascending));
      }

      LoadView(oView);
    }

    private void FeedMenuSort(int level, int col, string dir, bool ischecked)
    {
      FeedDataGridView oView = _currentView.Clone();

      while (oView.Sortings.Count >= level)
      {
        oView.Sortings.RemoveAt(level - 1);
      }

      if (ischecked)
      {
        oView.Sortings.Add(new DataGridSorting(FeedDataGrid.Columns[col - 1], dir == "d" ? SortDirection.Descending : SortDirection.Ascending));
      }

      LoadView(oView);
    }

    private void CheckParentItems(string[] test)
    {
        Stack<string> ss = new Stack<string>(test);
        ss.Pop();
        while ( ss.Count > 1)
        {
            FeedMenu.GetItemById(String.Join("_",ss.Reverse().ToArray())).IsChecked = true;
            ss.Pop();
        }
    }

    private void ClearMenu(string[] test)
    {
        if( test[1] == "3" ) {return;}
        for (int i = 3; i > Int16.Parse(test[1]); i--)
        {
            // disable existing top-level items
            FeedMenu.GetItemById(test[0] + "_" + i).Enabled = false;
            // 
            if (Int16.Parse(test[1]) + 1 < 4)
            {
                FeedMenu.GetItemById(test[0] + "_" + (Int16.Parse(test[1]) + 1)).Enabled = true;
            }
            // uncheck existing top-level items
            FeedMenu.GetItemById(test[0] + "_" + i).IsChecked = false;
            // clear out level 2 items 
            for (int j = 1; j < 6; j++)
            {
                FeedMenu.GetItemById(test[0] + "_" + i + "_" + j).Enabled = true;
                FeedMenu.GetItemById(test[0] + "_" + i + "_" + j).IsChecked = false;
                // clear out level 3 items if they exist
                if( j < 4 ) 
                {
                    FeedMenu.GetItemById(test[0] + "_" + i + "_" + j + "_a").IsChecked = false;
                    FeedMenu.GetItemById(test[0] + "_" + i + "_" + j + "_d").IsChecked = false;
                }
                // starting at current check previous level 1 items to see if they should be disabled.
                for (int k = Int16.Parse(test[1]); k > 0; k--)
                {
                    MenuItem temp = FeedMenu.GetItemById(test[0] + "_" + k + "_" + j);
                    if (FeedMenu.GetItemById(test[0] + "_" + k + "_" + j).IsChecked )
                    {
                        FeedMenu.GetItemById(test[0] + "_" + i + "_" + j).Enabled = false;
                    }
                }
            }
        }
    }
    #endregion

    #region FeaturedViews
    private void FeaturedView1_Click(object sender, RoutedEventArgs e)
    {
        ShowFeaturedView1();
    }
    private void ShowFeaturedView1()
    {
        FeedDataGridView oView = new FeedDataGridView();

        oView.Groupings.Add(new ComponentArt.Silverlight.UI.Data.DataGridGrouping(FeedDataGrid.Columns["FeedName"], SortDirection.Ascending));

        oView.Sortings.Add(new DataGridSorting(FeedDataGrid.Columns["Rating"], SortDirection.Descending));

        oView.FilterText = "All feeds";

        LoadView(oView);
    }
    private void FeaturedView2_Click(object sender, RoutedEventArgs e)
    {
        ShowFeaturedView2();
    }
    private void ShowFeaturedView2()
    {
      FeedDataGridView oView = new FeedDataGridView();

      oView.Groupings.Add(new ComponentArt.Silverlight.UI.Data.DataGridGrouping(FeedDataGrid.Columns["FeedName"], SortDirection.Ascending));
      oView.Filters.Add(new DataGridDataCondition("FeedName", new System.Collections.ObjectModel.ObservableCollection<object> { "GigaOm", "TechCrunch", "ReadWriteWeb"}, DataGridDataConditionOperand.In));
      oView.Filters.Add(new DataGridDataCondition("Rating", 5));
      oView.FilterText = "Web 2.0 Blogs";
      
      LoadView(oView);
    }
    private void FeaturedView3_Click(object sender, RoutedEventArgs e)
    {
        ShowFeaturedView3();
    }
    private void ShowFeaturedView3()
    {
      FeedDataGridView oView = new FeedDataGridView();

      oView.Groupings.Add(new ComponentArt.Silverlight.UI.Data.DataGridGrouping(FeedDataGrid.Columns["Visits"], SortDirection.Ascending));
      oView.Filters.Add(new DataGridDataCondition("FeedName", new System.Collections.ObjectModel.ObservableCollection<object> { "MSDN Blogs", "Brad Abrams", "Nikhil Kothari", "Scott Guthrie", "Jesse Liberty", "Mike Snow", "Eilon Lipton", "Bertrand Le Roy", "Scott Hanselman", "Scott Barnes" }, DataGridDataConditionOperand.In));
      oView.Filters.Add(new DataGridDataCondition("Visits", 500, DataGridDataConditionOperand.GreaterThanOrEqual));
      oView.FilterText = "Microsoft Dev Blogs";

      LoadView(oView);
    }
    #endregion

    #region ToolBar Methods

    void Previous_Click(object sender, EventArgs e)
    {
        NavigateBack();
    }

    void Next_Click(object sender, EventArgs e)
    {
        NavigateForward();
    }

    private void PreviousMenu_Hide(object sender, RoutedEventArgs e)
    {
        PreviousMenu.HideContextMenu();
    }

    private void NextMenu_Hide(object sender, RoutedEventArgs e)
    {
        NextMenu.HideContextMenu();
    }
    
    private void CreatePrevView()
    {
        PreviousMenu.Items.Clear();

        if (_feedHistory.Count > 0)
        {
            for (int i = (_feedHistoryPosition + 1); i < (_feedHistoryPosition + 6); i++ )
            {
                if (_feedHistory.Count > i)
                {
                    PreviousMenu.Items.Add(GenerateToolBarMenuItem(_feedHistory[i],i));
                }
            }
        }
        PreviousButton.IsEnabled = (PreviousMenu.Items.Count > 0) ? true : false;
    }

    private void CreateNextView()
    {
        NextMenu.Items.Clear();

        if (_feedHistory.Count > 0)
        {
            for (int i = (_feedHistoryPosition - 1); (i > -1 && i > _feedHistoryPosition-6); i--)
            {
                if (_feedHistory.Count > i)
                {
                    NextMenu.Items.Add(GenerateToolBarMenuItem(_feedHistory[i],i));
                }
            }
        }
        NextButton.IsEnabled = (NextMenu.Items.Count > 0) ? true : false;
    }

    private MenuItem GenerateToolBarMenuItem(FeedDataGridView oView, int idx)
    {
        Dictionary<string, string> seed = new Dictionary<string, string>();
        switch (oView.FilterText)
        {
            case "Microsoft Dev Blogs":
                seed.Add("BannerSource", "./Menu/featuredFeed-MSDevs.png");
                break;
            case "Web 2.0 Blogs":
                seed.Add("BannerSource", "./Menu/featuredFeed-5Star.png");
                break;
            case "All feeds":
                seed.Add("BannerSource", "./Menu/featuredFeed-100000.png");
                break;
            default:
        seed.Add("BannerSource", "./Menu/featuredFeed-userCreated.png");
                break;
        }
        seed.Add("Heading", "User Created");
        string filterText = oView.FilterText;
        if (oView.FilterText.IndexOf('=') > -1)
        {
            filterText = oView.FilterText.Split('=')[1];
        }
        seed.Add("Feeds", "Feeds: " + filterText);
        string groupstring = "";
        foreach (ComponentArt.Silverlight.UI.Data.DataGridGrouping dgg in oView.Groupings)
        {
            if (dgg.Column.Header != null)
            {
                groupstring += dgg.Column.ColumnName + ",";
            }
        }
        if (groupstring.IndexOf(',') > -1)
        {
            groupstring = groupstring.Remove(groupstring.Length-1, 1);
        }
        seed.Add("Grouping", "Grouping: " + groupstring);
        seed.Add("Fields", "Fields: " + "All Fields");
        seed.Add("Filter", "Filter: " + "All");
        string sortstring = "";
        foreach (DataGridSorting dgs in oView.Sortings)
        {
            if (dgs.Column.Header != null)
            {
                sortstring += dgs.Column.ColumnName + ",";
            }
        }
        if (sortstring.IndexOf(',') > -1)
        {
            sortstring = sortstring.Remove(sortstring.Length - 1, 1);
        }
        seed.Add("Sorting", "Sorting: " + sortstring );
        seed.Add("LoadAll", "Load All: " + "Yes");
        MenuItem mi = new MenuItem(seed);
        mi.Tag = idx;
        mi.CustomTemplate = "MenuViews";
        mi.CustomTemplateHighlight = false;
        return mi;
    }

    private void NextMenu_MenuClick(object sender, MenuCommandEventArgs mce)
    {
        int test = (int)mce.ItemSource.Tag;
        if (_feedHistory.Count > test)
        {
            NavigateToPosition(test);
        }
    }

    private void PreviousMenu_MenuClick(object sender, MenuCommandEventArgs mce)
    {
        int test = (int)mce.ItemSource.Tag;
        if( _feedHistory.Count > test )
        {
            NavigateToPosition(test);
        }
    }

    private void RefreshButton_Click(object sender, EventArgs e)
    {
        LoadView(_currentView, false);
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {

    }

    private void HomeButton_Click(object sender, EventArgs e)
    {
        ShowFeaturedView3();
    }
    #endregion

    #region ItemFlow Methods

    private void Animate(ItemFlowItem myItem)
    {

        if (Title0.Text == myItem.Text) return;

        Title0.Text = myItem.Text;
        SubTitle0.Text = " - " + myItem.Tag.ToString().Substring(myItem.Tag.ToString().IndexOf("|") + 1);

        Storyboard oAnimation = (Storyboard)this.Resources["Title_Animation"];

        if (oAnimation != null && oAnimation.GetCurrentState() != ClockState.Active)
        {
            oAnimation.Begin();
            oAnimation.Completed += new EventHandler(Animation_Completed);
        }
    }

    void Animation_Completed(object sender, EventArgs e)
    {
        Title.Text = Title0.Text;
        SubTitle.Text = SubTitle0.Text;

        Storyboard oAnimation = (Storyboard)this.Resources["Title_Animation_Reverse"];
        oAnimation.Begin();
    }

    private void myItemFlow_FlowComplete(object sender, ItemFlowItemEventArgs e)
    {
        Animate(e.Item);

        string sFeedName = e.Item.Tag.ToString().Substring(0, e.Item.Tag.ToString().IndexOf("|"));

        FeedDataGridView oView = new FeedDataGridView(); // _currentView.Clone();
        oView.Filters.Add(new DataGridDataCondition("FeedName", sFeedName));
        oView.FilterText = "FeedName = " + sFeedName;

        LoadView(oView);
    }

    private void myItemFlow_ItemMouseEnter(object sender, ItemFlowItemEventArgs e)
    {
        Animate(e.Item);
    }

    private void myItemFlow_ItemMouseLeave(object sender, ItemFlowItemEventArgs e)
    {
        Animate(((ItemFlow)sender).Items[((ItemFlow)sender).SelectedIndex]);
    }

    private void myItemFlow_Loaded(object sender, RoutedEventArgs e)
    {
        Animate(((ItemFlow)sender).Items[((ItemFlow)sender).SelectedIndex]);
    }

    private void myItemFlow_OnDragSelected(object sender, ItemFlowItemEventArgs e)
    {
        Animate(e.Item);

        string sFeedName = e.Item.Tag.ToString().Substring(0, e.Item.Tag.ToString().IndexOf("|"));

        FeedDataGridView oView = new FeedDataGridView(); // _currentView.Clone();
        oView.Filters.Add(new DataGridDataCondition("FeedName", sFeedName));
        oView.FilterText = "FeedName = " + sFeedName;

        LoadView(oView);
    }

    private void myItemFlow_ItemClick(object sender, ComponentArt.Silverlight.UI.Navigation.ItemFlowItemEventArgs e)
    {
        if (e.Item.IsSelected)
        {
            Animate(e.Item);

            string sFeedName = e.Item.Tag.ToString().Substring(0, e.Item.Tag.ToString().IndexOf("|"));

            FeedDataGridView oView = new FeedDataGridView(); // _currentView.Clone();
            oView.Filters.Add(new DataGridDataCondition("FeedName", sFeedName));
            oView.FilterText = "FeedName = " + sFeedName;

            LoadView(oView);
        }
    }

    #endregion


    #region TreeView(s) Methods

    private void FeedTreeView_ImageError(object sender, TreeViewNodeEventArgs args)
    {
    }

    private void FeedTreeView_Browse_NodeSelected(object sender, TreeViewNodeEventArgs args)
    {
        if (args.Node.ParentNode != null && args.Node.ParentNode.Text == favoritesTreeViewFolder)
        {
            if (!string.IsNullOrEmpty(args.Node.Id))
                try
                {
                    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(args.Node.Id, UriKind.RelativeOrAbsolute), "BlogPost");
                }
                catch { }
        }
        else
        {
      if (!args.Node.IsLoadOnDemandEnabled)
      {
        FeedDataGridView oView = new FeedDataGridView(); // _currentView.Clone();
        oView.Filters.Add(new DataGridDataCondition("FeedName", args.Node.Text.ToString()));
        oView.FilterText = "FeedName = " + args.Node.Text.ToString();
        LoadView(oView);
      }
    }
    }

    private void FeedTreeView_Browse_Loaded(object sender, EventArgs args)
    {
      if (FeedTreeView_Browse.Items.Count > 0)
      {
          (FeedTreeView_Browse.Items[0] as TreeViewNode).IsExpanded = true;
          
      }
    }

    private void FeedTreeView_Browse_SoaResponseProcessed(object sender, TreeViewSoaResponseEventArgs e)
    {
        // make category nodes bold
        foreach (TreeViewNode node in e.AffectedNodes)
        {
            if (node.Id == "Category")
                node.FontWeight = FontWeights.Bold;
            else
                node.FontWeight = FontWeights.Normal;
        }
    }

    private void FeedTreeView_Browse_NodeDragEnter(object sender, TreeViewNodeDragEventArgs e)
    {
        if (e.TargetNode != null && e.TargetNode.Text == favoritesTreeViewFolder)
        {
            e.Handled = true;
            e.Cancel = false;
            UIElement el;
            el = ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "CantDropIcon");
            if (el != null) el.Visibility = Visibility.Collapsed;
            el = ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "CantDropText");
            if (el != null) el.Visibility = Visibility.Collapsed;
            el = ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "CanDropIcon");
            if (el != null) el.Visibility = Visibility.Visible;
        }
        else
        {
            e.Handled = false;
            e.Cancel = true;
            UIElement el;
            el = ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "CantDropIcon");
            if (el != null) el.Visibility = Visibility.Visible;
            el = ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "CantDropText");
            if (el != null) el.Visibility = Visibility.Visible;
            el = ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "CanDropIcon");
            if (el != null) el.Visibility = Visibility.Collapsed;
        }
    }

    private void FeedTreeView_Browse_NodeDrop(object sender, TreeViewNodeDropEventArgs e)
    {
        if (e.TargetParentNode != null && e.TargetParentNode.Text == favoritesTreeViewFolder)
        {
            if (e.Source == FeedDataGrid)
            {
                foreach (DataGridRow row in e.Items)
                {
                    TreeViewNode feedNode = new TreeViewNode()
                    {
                        Text = (string)row.Cells["Title"].Value,
                        IconSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("/ComponentArt.Silverlight.Demos;component/Assets/DataGrid/FeedBrowser/FeedTitle_icon.png", UriKind.Absolute)),
                        Id = (string)row.Cells["ItemURL"].Value,
                        FontWeight = FontWeights.Normal
                    };

                    e.TargetParentNode.Items.Insert(e.TargetIndex, feedNode);
                    e.TargetParentNode.Dispatcher.BeginInvoke(() =>
                        {
                            feedNode.Padding = new Thickness(feedNode.Padding.Left, feedNode.Padding.Top - 1, feedNode.Padding.Right, feedNode.Padding.Bottom - 1);

                        });
                }
                e.TargetParentNode.IsExpanded = true;

            } 
        }
    }

    private void FeedTreeView_Browse_BeforeNodeDrop(object sender, TreeViewNodeDropEventArgs e)
    {
        e.Cancel = !(e.TargetParentNode != null && e.TargetParentNode.Text == favoritesTreeViewFolder);
    }

    private void FeedTreeView_Browse_DragLeave(object sender, ComponentArt.Silverlight.UI.DragEventArgs e)
    {
        UIElement el;
        el = ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "CantDropIcon");
        if (el != null) el.Visibility = Visibility.Collapsed;
        el = ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "CantDropText");
        if (el != null) el.Visibility = Visibility.Collapsed;
        el = ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "CanDropIcon");
        if (el != null) el.Visibility = Visibility.Visible;
    }


    private string favoritesTreeViewFolder { get { return (string)Resources["MyFavoritesFolder"]; } }

    #endregion

    #region DataGrid Methods

    private void FeedDataGrid_GroupingChanged(object sender, ComponentArt.Silverlight.UI.Data.DataGridGroupingChangedEventArgs args)
    {
        FeedDataGridView oView = _currentView.Clone();

        oView.Groupings.Clear();
        foreach (ComponentArt.Silverlight.UI.Data.DataGridGrouping oGrouping in FeedDataGrid.Groupings)
        {
            oView.Groupings.Add(new ComponentArt.Silverlight.UI.Data.DataGridGrouping(oGrouping.Column, oGrouping.Direction));
        }

        // update view
        ReactToViewChange(oView, true);
    }

    private void FeedDataGrid_SortingChanged(object sender, ComponentArt.Silverlight.UI.Data.DataGridSortingChangedEventArgs args)
    {
        FeedDataGridView oView = _currentView.Clone();

        oView.Sortings.Clear();
        foreach (DataGridSorting oSorting in FeedDataGrid.Sortings)
        {
            oView.Sortings.Add(new DataGridSorting(oSorting.Column, oSorting.Direction));
        }

        // update view
        ReactToViewChange(oView, true);
    }

    private void FeedDataGrid_DragStarted(object sender, ComponentArt.Silverlight.UI.DragEventArgs e)
    {
        // customize Drag control for multiple rows
        if (e.Items.Count > 1)
        {
            e.DragControl.VisualControl.ContentTemplate = (DataTemplate)Resources["DataGridDragMultipleRows"];
            e.DragControl.VisualControl.Content = string.Format("{0} Posts", e.Items.Count);
            e.DragControl.VisualControl.Dispatcher.BeginInvoke(() =>
            {
                Grid grid = (Grid)ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "ImageGrid");
                if (grid != null)
                {
                    int margin = 0;
                    int count = 0;
                    foreach (DataGridRow row in e.Items)
                    {
                        string iconSource = "/ComponentArt.Silverlight.Demos;component/Assets/DataGrid/FeedBrowser/FeedTitle_icon.png";
                        Image imgPost = new Image()
                        {
                            Margin = new Thickness(margin, 0, 0, 0),
                            Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(iconSource, UriKind.RelativeOrAbsolute)),
                            Stretch = Stretch.None,
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left
                        };
                        imgPost.SetValue(Grid.ColumnProperty, 0);
                        grid.Children.Add(imgPost);
                        margin += 5;
                        count++;
                        // don't show too many icons
                        if (count > 10) break;
                    }
                }
            });
        }
        else
        {
            e.DragControl.VisualControl.ContentTemplate = (DataTemplate)Resources["DataGridDragSingleRow"];

            e.DragControl.VisualControl.Dispatcher.BeginInvoke(() =>
            {
                TextBlock itemTitle = (TextBlock)ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "ItemTitle");
                if (itemTitle != null)
                    itemTitle.Text = (string)(e.Items[0] as DataGridRow).Cells["Title"].Value;
                TextBlock feedTitle = (TextBlock)ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "FeedTitle");
                if (feedTitle != null)
                    feedTitle.Text = (string)(e.Items[0] as DataGridRow).Cells["FeedName"].Value;
                TextBlock timestamp = (TextBlock)ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "Timestamp");
                if (timestamp != null)
                    timestamp.Text = ((DateTime)(e.Items[0] as DataGridRow).Cells["DatePosted"].Value).ToString();
                Image imgRating = (Image)ComponentArt.Silverlight.UI.Utils.Visual.FindElementByName(e.DragControl.VisualControl, "Rating");
                if (imgRating != null)
                    imgRating.Source = (ImageSource)(new RatingToImageSourceConverter().Convert((e.Items[0] as DataGridRow).Cells["Rating"].Value, null, null, null));

            });
        }
    }

#endregion

    public void Dispose()
    {
        FeedDataGrid.Dispose();
    }

    private void GoButton_Click(object sender, RoutedEventArgs e)
    {
        Button btn = sender as Button;
        if (btn != null && !string.IsNullOrEmpty((string)btn.Tag))
        {
            try
            {
                System.Windows.Browser.HtmlPage.Window.Navigate(new Uri((string)btn.Tag, UriKind.RelativeOrAbsolute), "BlogPost");
            }
            catch { }
        }
    }
  }

  public class FeedDataGridView
  {
    public List<string> Columns;
    public DataGridGroupingCollection Groupings;
    public DataGridSortingCollection Sortings;
    public DataGridDataConditionCollection Filters;
    public string FilterText = "Custom";


    public FeedDataGridView()
    {
      Groupings = new DataGridGroupingCollection();
      Sortings = new DataGridSortingCollection();
      Filters = new DataGridDataConditionCollection();
      Columns = new List<string>();
    }

    public FeedDataGridView Clone()
    {
      FeedDataGridView oClone = new FeedDataGridView();

      foreach (ComponentArt.Silverlight.UI.Data.DataGridGrouping oGrouping in Groupings)
      {
          oClone.Groupings.Add(new ComponentArt.Silverlight.UI.Data.DataGridGrouping(oGrouping.Column, oGrouping.Direction));
      }

      foreach (DataGridSorting oSorting in Sortings)
      {
        oClone.Sortings.Add(new DataGridSorting(oSorting.Column, oSorting.Direction));
      }

      foreach (DataGridDataCondition oFilter in Filters)
      {
        oClone.Filters.Add(new DataGridDataCondition(oFilter.DataFieldName, oFilter.DataFieldValue, oFilter.Operand));
      }

      foreach (string sColumn in Columns)
      {
        oClone.Columns.Add(sColumn);
      }

      return oClone;
    }
  }

}
