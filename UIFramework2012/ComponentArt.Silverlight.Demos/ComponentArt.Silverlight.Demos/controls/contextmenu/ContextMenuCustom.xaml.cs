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

namespace ComponentArt.Silverlight.Demos
{
    public partial class ContextMenuCustom : UserControl, IDisposable
    {
        TreeViewNode current;

        public ContextMenuCustom()
        {
            InitializeComponent();
            MouseRightClick.Instance.startListening();
            MouseRightClick.Instance.RightClick -= new MouseRightClick.RightClickEventHandler(OnContextMenu);
            MouseRightClick.Instance.RightClick += new MouseRightClick.RightClickEventHandler(OnContextMenu);
        }

        #region IDisposable Members
        void IDisposable.Dispose()
        {
            MouseRightClick.Instance.RightClick -= new MouseRightClick.RightClickEventHandler(OnContextMenu);

            menu_calendar.Dispose();
            menu_inbox.Dispose();
            menu_contacts.Dispose();
        }
        #endregion

        private void OnContextMenu(object sender, RightClickEventArgs rce)
        {
            foreach (UIElement uie in rce.objectsUnderPoint)
            {
                if (uie is TreeViewNode)
                {
                    string test = ((TreeViewNode)uie).Header.ToString();
                    if (test == "Calendar")
                    {
                        menu_contacts.HideContextMenu();
                        menu_inbox.HideContextMenu();

                        if (menu_calendar.IsShowing)
                        {
                            menu_calendar.HideContextMenu();
                        }
                        menu_calendar.ShowContextMenu(rce.clientPoint);
                        current = (TreeViewNode)uie;
                    }
                    else if (test == "Contacts")
                    {
                        if (menu_contacts.IsShowing)
                        {
                            menu_contacts.HideContextMenu();
                        }
                        menu_contacts.ShowContextMenu(rce.clientPoint);
                        menu_inbox.HideContextMenu();
                        menu_calendar.HideContextMenu();
                        current = (TreeViewNode)uie;
                    }
                    else if (test == "Inbox")
                    {
                        menu_calendar.HideContextMenu();
                        menu_contacts.HideContextMenu();
                        if (menu_inbox.IsShowing)
                        {
                            menu_inbox.HideContextMenu();
                        }
                        menu_inbox.ShowContextMenu(rce.clientPoint);
                        current = (TreeViewNode)uie;
                    }
                }
            }
        }

        private void menu_MenuClick(object sender, MenuCommandEventArgs mce)
        {
            if (current == null) { return; }
            output.Text = '"' + mce.ItemSource.Text + '"' + " command was issued on the " + current.Header + " node from " + ((TreeViewNode)current.Parent).Header;
        }

    }
}
