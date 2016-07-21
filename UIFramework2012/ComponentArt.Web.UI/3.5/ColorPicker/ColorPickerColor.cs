using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Drawing;

namespace ComponentArt.Web.UI
{
    [Serializable]
    public class ColorPickerColor
    {
        private string _hex = String.Empty;
        private string _name = String.Empty;
        private int _red = 0;
        private int _green = 0;
        private int _blue = 0;
        private Color _color = Color.Empty;

        # region public properties

        /// <summary>
        /// The System.Drawing.Color object of the ColorPickerColor.
        /// </summary>
        public Color SystemDrawingColor
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        /// <summary>
        /// The Hex value of the ColorPickerColor.
        /// </summary>
        public string Hex
        {
            get
            {
                return string.Format("{0:X8}", _color.ToArgb()).Substring(2);
            }
            set
            {
                if (value.IndexOf('#') > -1) 
                    value = value.Substring(1);

                if (value.Length == 6)
                    value = "FF" + value;

                _color = ColorTranslator.FromHtml("#"+value);
            }
        }

        /// <summary>
        /// The name value of the ColorPickerColor.
        /// </summary>
        public string Name
        {
            get
            {
                return (_color.IsNamedColor) ? _color.Name : String.Empty;
            }
            set
            {
                _color = ColorTranslator.FromHtml(value);           
            }
        }

        /// <summary>
        /// The HTML value of the ColorPickerColor.
        /// </summary>
        public string HTML
        {
            get
            {
                return ColorTranslator.ToHtml(_color);
            }
            set
            {
                _color = ColorTranslator.FromHtml(value);
            }
        }

        /// <summary>
        /// The Red value of the ColorPickerColor.
        /// </summary>
        public int R
        {
            get
            {
                return _color.R;
            }
            set
            {
                _red = value;
                _color = Color.FromArgb(0, _red, _green, _blue);
            }
        }

        /// <summary>
        /// The Green value of the ColorPickerColor.
        /// </summary>
        public int G
        {
            get
            {
                return _color.G;
            }
            set
            {
                _green = value;
                _color = Color.FromArgb(0, _red, _green, _blue);
            }
        }

        /// <summary>
        /// The Blue value of the ColorPickerColor.
        /// </summary>
        public int B
        {
            get
            {
                return _color.B;
            }
            set
            {
                _blue = value;
                _color = Color.FromArgb(0, _red, _green, _blue);
            }
        }

        #endregion

    }
}
