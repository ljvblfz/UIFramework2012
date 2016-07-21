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
    public class ImageButton : Button
    {
        #region ImageNormal

        /// <summary> 
        /// Gets or sets the ImageNormal possible Value of the ImageSource object.
        /// </summary> 
        public ImageSource ImageNormal
        {
            get { return (ImageSource)GetValue(ImageNormalProperty); }
            set { SetValue(ImageNormalProperty, value); }
        }

        /// <summary> 
        /// Identifies the ImageNormal dependency property.
        /// </summary> 
        public static readonly DependencyProperty ImageNormalProperty =
                    DependencyProperty.Register(
                          "ImageNormal",
                          typeof(ImageSource),
                          typeof(ImageButton),
                          new PropertyMetadata(OnImageNormalPropertyChanged));

        /// <summary>
        /// ImageNormalProperty property changed handler. 
        /// </summary>
        /// <param name="d">ImageButton that changed its ImageNormal.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnImageNormalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageButton _ImageButton = d as ImageButton;
            if (_ImageButton != null)
            {
            }
        }
        #endregion ImageNormal

        #region ImageHover

        /// <summary> 
        /// Gets or sets the ImageHover possible Value of the ImageSource object.
        /// </summary> 
        public ImageSource ImageHover
        {
            get { return (ImageSource)GetValue(ImageHoverProperty); }
            set { SetValue(ImageHoverProperty, value); }
        }

        /// <summary> 
        /// The ImageHover property. (DependencyProperty)
        /// </summary> 
        public static readonly DependencyProperty ImageHoverProperty =
                    DependencyProperty.Register(
                          "ImageHover",
                          typeof(ImageSource),
                          typeof(ImageButton),
                          new PropertyMetadata(OnImageHoverPropertyChanged));

        /// <summary>
        /// ImageHoverProperty property changed handler. 
        /// </summary>
        /// <param name="dpObj">ImageButton that changed its ImageHover.</param>
        /// <param name="change">DependencyPropertyChangedEventArgs.</param> 
        private static void OnImageHoverPropertyChanged(DependencyObject dpObj, DependencyPropertyChangedEventArgs change)
        {
            ImageButton _ImageButton = dpObj as ImageButton;
            if (_ImageButton != null)
            {
            }
        }
        #endregion ImageHover

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
                          typeof(ImageButton),
                          new PropertyMetadata(OnTextPropertyChanged));

        /// <summary>
        /// TextProperty property changed handler. 
        /// </summary>
        /// <param name="d">ImageButton that changed its Text.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageButton _ImageButton = d as ImageButton;
            if (_ImageButton != null)
            {
                //TODO: Handle new value. 
            }
        }
        #endregion Text


        #region HyperlinkUri

        /// <summary> 
        /// Gets or sets the HyperlinkUri possible Value of the Uri object.
        /// </summary> 
        public Uri HyperlinkUri
        {
            get { return (Uri)GetValue(HyperlinkUriProperty); }
            set { SetValue(HyperlinkUriProperty, value); }
        }

        /// <summary> 
        /// Identifies the HyperlinkUri dependency property.
        /// </summary> 
        public static readonly DependencyProperty HyperlinkUriProperty =
                    DependencyProperty.Register(
                          "HyperlinkUri",
                          typeof(Uri),
                          typeof(ImageButton),
                          new PropertyMetadata(OnHyperlinkUriPropertyChanged));

        /// <summary>
        /// HyperlinkUriProperty property changed handler. 
        /// </summary>
        /// <param name="d">ImageButton that changed its HyperlinkUri.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs.</param> 
        private static void OnHyperlinkUriPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageButton _ImageButton = d as ImageButton;
            if (_ImageButton != null)
            {
                //TODO: Handle new value. 
            }
        }
        #endregion HyperlinkUri

    }
}