using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ComponentArt.Win.Demos
{
    public class SmallIndexDemoButton : Button
    {

        #region ButtonImage

        /// <summary> 
        /// Gets or sets the ButtonImage possible Value of the ImageSource object.
        /// </summary> 
        public ImageSource ButtonImage
        {
            get { return (ImageSource)GetValue(ButtonImageProperty); }
            set { SetValue(ButtonImageProperty, value); }
        }

        /// <summary> 
        /// Identifies the ButtonImage dependency property.
        /// </summary> 
        public static readonly DependencyProperty ButtonImageProperty =
                    DependencyProperty.Register(
                          "ButtonImage",
                          typeof(ImageSource),
                          typeof(SmallIndexDemoButton),
                          new PropertyMetadata(OnButtonImagePropertyChanged));

        /// <summary>
        /// ButtonImageProperty property changed handler. 
        /// </summary>
        /// <param name="d">SmallIndexDemoButton that changed its ButtonImage.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnButtonImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SmallIndexDemoButton _SmallIndexDemoButton = d as SmallIndexDemoButton;
            if (_SmallIndexDemoButton != null)
            {
                //TODO: Handle new value. 
            }
        }
        #endregion ButtonImage

        #region HeaderText

        /// <summary> 
        /// Gets or sets the HeaderText possible Value of the String object.
        /// </summary> 
        public String HeaderText
        {
            get { return (String)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        /// <summary> 
        /// Identifies the HeaderText dependency property.
        /// </summary> 
        public static readonly DependencyProperty HeaderTextProperty =
                    DependencyProperty.Register(
                          "HeaderText",
                          typeof(String),
                          typeof(SmallIndexDemoButton),
                          new PropertyMetadata(OnHeaderTextPropertyChanged));

        /// <summary>
        /// HeaderTextProperty property changed handler. 
        /// </summary>
        /// <param name="d">SmallIndexDemoButton that changed its HeaderText.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnHeaderTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SmallIndexDemoButton _SmallIndexDemoButton = d as SmallIndexDemoButton;
            if (_SmallIndexDemoButton != null)
            {
                //TODO: Handle new value. 
            }
        }
        #endregion HeaderText

        #region Id

        /// <summary> 
        /// Gets or sets the Id possible Value of the String object.
        /// </summary> 
        public String Id
        {
            get { return (String)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        /// <summary> 
        /// Identifies the Id dependency property.
        /// </summary> 
        public static readonly DependencyProperty IdProperty =
                    DependencyProperty.Register(
                          "Id",
                          typeof(String),
                          typeof(SmallIndexDemoButton),
                          new PropertyMetadata(OnIdPropertyChanged));

        /// <summary>
        /// IdProperty property changed handler. 
        /// </summary>
        /// <param name="d">SmallIndexDemoButton that changed its Id.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SmallIndexDemoButton _SmallIndexDemoButton = d as SmallIndexDemoButton;
            if (_SmallIndexDemoButton != null)
            {
                //TODO: Handle new value. 
            }
        }
        #endregion Id


        #region ImgHeight

        /// <summary> 
        /// Gets or sets the ImgHeight possible Value of the double object.
        /// </summary> 
        public double ImgHeight
        {
            get { return (double)GetValue(ImgHeightProperty); }
            set { SetValue(ImgHeightProperty, value); }
        }

        /// <summary> 
        /// Identifies the ImgHeight dependency property.
        /// </summary> 
        public static readonly DependencyProperty ImgHeightProperty =
                    DependencyProperty.Register(
                          "ImgHeight",
                          typeof(double),
                          typeof(SmallIndexDemoButton),
                          new PropertyMetadata(OnImgHeightPropertyChanged));

        /// <summary>
        /// ImgHeightProperty property changed handler. 
        /// </summary>
        /// <param name="d">SmallIndexDemoButton that changed its ImgHeight.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnImgHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SmallIndexDemoButton _SmallIndexDemoButton = d as SmallIndexDemoButton;
            if (_SmallIndexDemoButton != null)
            {
                //TODO: Handle new value. 
            }
        }
        #endregion ImgHeight


        #region ImgWidth

        /// <summary> 
        /// Gets or sets the ImgWidth possible Value of the double object.
        /// </summary> 
        public double ImgWidth
        {
            get { return (double)GetValue(ImgWidthProperty); }
            set { SetValue(ImgWidthProperty, value); }
        }

        /// <summary> 
        /// Identifies the ImgWidth dependency property.
        /// </summary> 
        public static readonly DependencyProperty ImgWidthProperty =
                    DependencyProperty.Register(
                          "ImgWidth",
                          typeof(double),
                          typeof(SmallIndexDemoButton),
                          new PropertyMetadata(OnImgWidthPropertyChanged));

        /// <summary>
        /// ImgWidthProperty property changed handler. 
        /// </summary>
        /// <param name="d">SmallIndexDemoButton that changed its ImgWidth.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnImgWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SmallIndexDemoButton _SmallIndexDemoButton = d as SmallIndexDemoButton;
            if (_SmallIndexDemoButton != null)
            {
                //TODO: Handle new value. 
            }
        }
        #endregion ImgWidth


    }

}