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
using System.Windows.Media.Imaging;
using ComponentArt.Silverlight.UI.Layout;
using ComponentArt.Silverlight.UI.Navigation;
using ComponentArt.Silverlight.UI.Data;
using System.Diagnostics;

namespace ComponentArt.Silverlight.Demos
{
    public partial class Outlook2007 : UserControl
    {
        public string[] shortcuts = new string[] { "folder", "notes", "tasks", "contacts", "calendar", "mail" };

        public Outlook2007()
        {
            InitializeComponent();
            LoadAllData();
        }

        public void LoadAllData()
        {
            Uri service = new Uri(Application.Current.Host.Source, "../WCFMessageService.svc");

            MessageService.WCFMessageServiceClient oSoapClient = new MessageService.WCFMessageServiceClient("BasicHttpBinding_WCFMessageService", service.AbsoluteUri);

            oSoapClient.GetRecordsCompleted += oSoapClient_GetAllRecordsCompleted;
            oSoapClient.GetRecordsAsync(int.MaxValue, 0, DataGrid1.SortExpression);
        }

        void oSoapClient_GetAllRecordsCompleted(object sender, MessageService.GetRecordsCompletedEventArgs e)
        {
           DataGrid1.DataContext = e.Result;
        }

        public void Dispose()
        {
           DataGrid1.Dispose();
        }

        private void DataGridClicked(object sender, DataGridRowEventArgs e)
        {
            MessageService.Message myMessage = (MessageService.Message)e.Row.DataContext;
            subject.Text = myMessage.Subject;
            startedby.Text = myMessage.StartedBy;
            date.Text = String.Format("{0:d/M/yyyy h:mm:ss tt}", myMessage.LastPostDate);
        }

        private void HandleNavBar(Splitter mySplitter)
        {
            double start = 201;
            double height = 32;

            // force minimum
            if (mySplitter.CurrentPosition < start) mySplitter.CurrentPosition = start;

            double mod = (mySplitter.CurrentPosition - start) % height;
            double count = ((mySplitter.CurrentPosition - start) - mod) / height;

            if (mod > height - 5)
            {
                count++;
                mySplitter.CurrentPosition = start + height * count;
            }
            else
            {
                mySplitter.CurrentPosition = start + height * count;
            }

            ToolBar2.Items.Clear();

            ToolBarItem myItem = new ToolBarItem();

            for (int i = 0; i < count; i++)
            {
                if (i >= 0)
                {
                    myItem = new ToolBarItem();
                    myItem.Style = ToolBar2.ItemStyle;
                    myItem.IconSource = "./icons/" + shortcuts[i] + ".png";
                    ToolBar2.Items.Add(myItem);
                }
            }

            myItem = new ToolBarItem();
            myItem.Style = ToolBar2.ItemStyle;
            myItem.IconSource = "./icons/shortcuts.png";
            ToolBar2.Items.Add(myItem);
        }

        private void HorizontalResizing(object sender, ComponentArt.Silverlight.UI.Layout.SplitterResizedEventArgs e)
        {
            HandleNavBar((Splitter)sender);
        }
    }
}
