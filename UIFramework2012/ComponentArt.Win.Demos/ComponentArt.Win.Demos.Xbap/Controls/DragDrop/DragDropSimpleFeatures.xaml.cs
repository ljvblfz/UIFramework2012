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
using ComponentArt.Win.UI;
using ComponentArt.Win.UI.Utils;

namespace ComponentArt.Win.Demos
{
    public partial class DragDropSimpleFeatures : UserControl
  {
        public DragDropSimpleFeatures()
    {
        InitializeComponent();
        ComponentArt.Win.UI.DragDrop.DragCancelled += new EventHandler<ComponentArt.Win.UI.DragEventArgs>(DragDrop_DragCancelled);

        dragSource.Items.Add(new DataItem("/Assets/TreeView/Core/root.png", "Mailbox"));
        dragSource.Items.Add(new DataItem("/Assets/TreeView/Core/calendar.png", "Calendar"));
        dragSource.Items.Add(new DataItem("/Assets/TreeView/Core/contacts.png", "Contacts"));
        dragSource.Items.Add(new DataItem("/Assets/TreeView/Core/deleted.png", "Deleted Items"));


        dropSource.Items.Add(new DataItem("/Assets/TreeView/Core/junk.png", "Junk E-mail"));
        dropSource.Items.Add(new DataItem("/Assets/TreeView/Core/notes.png", "Notes"));
        dropSource.Items.Add(new DataItem("/Assets/TreeView/Core/outbox.png", "Outbox"));
        dropSource.Items.Add(new DataItem("/Assets/TreeView/Core/sentItems.png", "Sent Items"));

    }
        void DragDrop_DragCancelled(object sender, ComponentArt.Win.UI.DragEventArgs e)
        {
            Storyboard warning = Resources["errorMsgAnimation"] as Storyboard;
            warning.Begin();
        }

        public void dragSource_DragStarted(object sender, ComponentArt.Win.UI.DragEventArgs e)
        {
            if ((sender as ListBox).SelectedItem != null)
            {
                e.DragControl.Add((sender as ListBox).SelectedItem, Resources["sourceList"] as DataTemplate);
            }
            else
            {
                e.Cancel = true;
            }
        }
  }

    public class MyListBox : ListBox, IDropTarget
    {
        #region IDropTarget Members

        public void OnDrop(object sender, ComponentArt.Win.UI.DragEventArgs e)
        {
            e.Handled = true;
            if (e.Source is ListBox)
            {
                (e.Source as ListBox).Items.Remove(e.Items[0]);
                Items.Add(e.Items[0]);
            }
            this.Background = new SolidColorBrush(Colors.White);
        }

        public void OnDragEnter(object sender, ComponentArt.Win.UI.DragEventArgs e)
        {
            e.Handled = true;
            this.Background = new SolidColorBrush(Colors.LightGray);
        }

        public void OnDragLeave(object sender, ComponentArt.Win.UI.DragEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.White);
        }

        public void OnDragOver(object sender, ComponentArt.Win.UI.DragEventArgs e)
        {
            e.Handled = true;
        }

        public void OnDragHover(object sender, ComponentArt.Win.UI.DragEventArgs e)
        {
        }

        #endregion
    }

    #region demo dataset

    public class DataItem
    {
        public String IconSource { get; set; }
        public String Text { get; set; }

        public DataItem(String iconSource, String text)
        {
            this.IconSource = iconSource;
            this.Text = text;
        }
    }
    #endregion
}
