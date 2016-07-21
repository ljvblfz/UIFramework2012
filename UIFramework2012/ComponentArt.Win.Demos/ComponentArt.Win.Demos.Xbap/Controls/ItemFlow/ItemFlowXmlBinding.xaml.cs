using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
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
using System.Diagnostics;

namespace ComponentArt.Win.Demos 
{
    public partial class ItemFlowXmlBinding : UserControl, IDisposable
    {
        public ItemFlowXmlBinding() 
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();

            String menuXml = "";
            StreamReader streader = App.GetResourceStreamReader("pack://application:,,,/Assets/ItemFlow/Houses/ItemFlowData.xml");

            using (XmlReader reader = XmlReader.Create(streader, null))
            {
                reader.MoveToContent();
                menuXml = reader.ReadOuterXml().ToString();
            }

            myItemFlow.XmlBindingSource = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n" + menuXml;
            myItemFlow.ApplyTemplate();
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
