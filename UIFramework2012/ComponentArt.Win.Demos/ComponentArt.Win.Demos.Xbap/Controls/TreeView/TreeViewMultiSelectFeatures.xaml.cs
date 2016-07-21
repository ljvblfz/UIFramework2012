using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ComponentArt.Win.UI.Navigation;

namespace ComponentArt.Win.Demos
{
    /// <summary>
    /// Interaction logic for TreeViewMultiSelectFeatures.xaml
    /// </summary>
    public partial class TreeViewMultiSelectFeatures : UserControl
    {
        public TreeViewMultiSelectFeatures()
        {
            InitializeComponent();
            MultiSelectTree.SelectedNodes.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Selection_CollectionChanged);
        }

        void Selection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (TreeViewNode remove in e.OldItems)
                {
                    selection.Items.Remove(remove.ToString());
                }
            }
            if (e.NewItems != null)
            {
                foreach (TreeViewNode add in e.NewItems)
                {
                    selection.Items.Add(add.ToString());
                }
                // scroll to the end
                selection.Dispatcher.BeginInvoke((Action) delegate ()
                    { selection.ScrollIntoView(selection.Items[selection.Items.Count - 1]); });
            }
        }
    }
}
