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

namespace ComponentArt.Silverlight.Demos
{
    public partial class HtmlEditorOffice2007 : UserControl, IDisposable, IDisableable
    {

        public HtmlEditorOffice2007()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            HtmlEditor6.Dispose();
        }
    }
}
