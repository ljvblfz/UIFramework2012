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
using System.Xml;
using System.Windows.Media.Imaging;

namespace ComponentArt.Silverlight.Demos
{
    public partial class DemoList : UserControl
    {
        internal string _demoXmlPath = "Assets/Demo/TreeViewXml/";
        private string _controlIconPath = "../Assets/Demo/ControlLogos/";

        public DemoList()
        {
            Controls = new List<String>();
            InitializeComponent();
            Loaded += new RoutedEventHandler(DemoList_Loaded);
        }

        void DemoList_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (String item in Controls)
            {
                GenerateControlHeader(item);
                LoadControlXml(item);
            }
        }

        private void LoadControlXml(string controlName)
        {
            using (XmlReader reader = XmlReader.Create(_demoXmlPath + controlName + ".xml"))
            {
                while (reader.Read())
                {
                    if(reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.MoveToAttribute("Tag"))
                        {
                            // this is a demo node
                            string link = reader.Value.ToString();
                            reader.MoveToAttribute("Header");
                            GenerateDemoLink(reader.Value.ToString(), link);

                        }
                        else
                        {
                            // this is a section heading node
                            reader.MoveToAttribute("Header");
                            GenerateDemoSectionHeader(reader.Value.ToString());
                        }
                    }
                }
            }
        }

        private void GenerateControlHeader(string controlName)
        {
            StackPanel sp = new StackPanel();
            sp.Margin = new Thickness(0, 16, 0, -8);
            sp.Orientation = Orientation.Horizontal;
            Image im = new Image();
            string imPath = _controlIconPath + "node-"+controlName.ToLower()+".png";
            im.Source = (ImageSource)new BitmapImage(new Uri( imPath , UriKind.RelativeOrAbsolute ));
            im.Margin = new Thickness(0, 0, 3, 0);
            sp.Children.Add(im);
            TextBlock tb = new TextBlock();
            tb.Text = controlName + " for Silverlight";
            tb.FontSize = 13;
            tb.FontWeight = FontWeights.Bold;
            tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            sp.Children.Add(tb);
            ItemsStackPanel.Children.Add(sp);
        }

        private void GenerateDemoSectionHeader(string demoSection)
        {
            TextBlock tb = new TextBlock();
            tb.FontSize = 11;
            tb.FontWeight = FontWeights.Bold;
            tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            tb.Text = demoSection;
            tb.Margin = new Thickness(0, 8, 0, 0);
            ItemsStackPanel.Children.Add(tb);
        }

        private void GenerateDemoLink(string demoName, string demoLink)
        {
            HyperlinkButton hb = new HyperlinkButton();
            hb.Content = demoName;
            hb.Padding = new Thickness(12, 0, 0, 0);
            hb.Height = 14;
            if (Application.Current.Resources.Contains("BulletedHyperlinkButton"))
            {
                hb.Style = (Style)Application.Current.Resources["BulletedHyperlinkButton"];
            }
            hb.Tag = "demo " + demoLink;
            hb.Click += new RoutedEventHandler(hb_Click);
            ItemsStackPanel.Children.Add(hb);
        }

        void hb_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).LoadDemoHyper(sender, e);
        }


        #region Header

        /// <summary> 
        /// Gets or sets the Header possible Value of the ImageSource object.
        /// </summary> 
        public ImageSource Header
        {
            get { return (ImageSource)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary> 
        /// Identifies the Header dependency property.
        /// </summary> 
        public static readonly DependencyProperty HeaderProperty =
                    DependencyProperty.Register(
                          "Header",
                          typeof(ImageSource),
                          typeof(DemoList),
                          new PropertyMetadata(OnHeaderPropertyChanged));

        /// <summary>
        /// HeaderProperty property changed handler. 
        /// </summary>
        /// <param name="d">DemoList that changed its Header.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DemoList _DemoList = d as DemoList;
            if (_DemoList != null)
            {
                //TODO: Handle new value. 
            }
        }
        #endregion Header



        #region Controls

        /// <summary> 
        /// Gets or sets the Controls possible Value of the List<string> object.
        /// </summary> 
        public List<String> Controls
        {
            get { return (List<String>)GetValue(ControlsProperty); }
            set { SetValue(ControlsProperty, value); }
        }

        /// <summary> 
        /// Identifies the Controls dependency property.
        /// </summary> 
        public static readonly DependencyProperty ControlsProperty =
                    DependencyProperty.Register(
                          "Controls",
                          typeof(List<String>),
                          typeof(DemoList),
                          new PropertyMetadata(OnControlsPropertyChanged));

        /// <summary>
        /// ControlsProperty property changed handler. 
        /// </summary>
        /// <param name="d">DemoList that changed its Controls.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnControlsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DemoList _DemoList = d as DemoList;
            if (_DemoList != null)
            {
                //TODO: Handle new value. 
            }
        }
        #endregion Controls

    }
}
