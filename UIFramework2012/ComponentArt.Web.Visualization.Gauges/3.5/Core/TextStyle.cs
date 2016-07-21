using System;
using System.ComponentModel;
using System.Collections;
using System.Drawing.Text;
using System.Text;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Specifies available pre-defined text styles.
	/// </summary>
	public enum TextStyleKind
	{
		Default,
		DefaultIndicatorLabelStyle,
		BlackIce,
		ArcticWhite,
		ArcticWhiteNumericBook,
		BlackIceNumericBook,
		BlackIceNumericMechanical,
		ArcticWhiteNumericLCD,
		Monochrome,
		Custom
	}

    /// <summary>
    /// Represents a visual style definition for <see cref="Theme"/>, <see cref="TextAnnotation"/> and <see cref="ScaleAnnotationStyle"/> objects.
    /// </summary>
	[Serializable]
	public class TextStyle : NamedObject
	{
		private string fontName = "Arial";
		private double fontSizePerc = 6;
		private FontStyle fontStyle = FontStyle.Bold;
		private Color fontColor = Color.Empty;
		private Color decimalFontColor = Color.Empty;
		private Color fontBackColor = Color.Empty;
		private Color decimalFontBackColor = Color.Empty;
		private Color outlineColor = Color.Black;
		private Color shadowColor = Color.FromArgb(128,0,0,0);
		private float shadeOffsetPerc = 5; // Percentage of the font size
		private float outlineWidth = 1;
		private bool drawOutline = false;
		private bool drawShadow = true;

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New TextStyle instances should be created with <see cref="TextStyleCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// </summary>
		public TextStyle(string name) : base(name) { }

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New TextStyle instances should be created with <see cref="TextStyleCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// </summary>
		public TextStyle() { }

        #region --- Properties ---
		/// <summary>
		/// Font used for the text
		/// </summary>
		[TypeConverter(typeof(FontNamesConverter))]
		public string FontName { get { return fontName; } set { fontName = value; } }

		/// <summary>
		/// Text size 0-100, relative to the size of the gauge. 
		/// </summary>
		[Editor(typeof(SliderEditor),typeof(System.Drawing.Design.UITypeEditor)),ValueRange(1,15,0.5)]
		[DefaultValue(6)]
		public double FontSizePerc { get { return fontSizePerc; } set { fontSizePerc = value; } }
		
		/// <summary>
		/// The FontStyle information that will be applied to the text
		/// </summary>
		public FontStyle FontStyle { get { return fontStyle; } set { fontStyle = value; } }
        
		/// <summary>
		/// Color of the text
		/// </summary>
		public Color FontColor { get { return fontColor; } set { fontColor = value; } }
		
		/// <summary>
		/// Color for the decimal part of the text (i.e. with Numeric gauges)
		/// </summary>
		public Color DecimalFontColor { get { return decimalFontColor; } set { decimalFontColor = value; } }
		
		/// <summary>
		/// Background colour for the text
		/// </summary>
		public Color FontBackColor { get { return fontBackColor; } set { fontBackColor = value; } }
		
		/// <summary>
		/// Background color for the decimal part of the text (i.e. with Numeric gauges)
		/// </summary>
		public Color DecimalFontBackColor { get { return decimalFontBackColor; } set { decimalFontBackColor = value; } }
		
		/// <summary>
		/// Text shadow color
		/// </summary>
		public Color ShadowColor { get { return shadowColor; } set { shadowColor = value; } }
		
		/// <summary>
		/// Text outline color
		/// </summary>
		public Color OutlineColor { get { return outlineColor; } set { outlineColor = value; } }
		
		/// <summary>
		/// Text shadow offset, as percentage of the whole gauge
		/// </summary>
		public float ShadeOffsetPerc { get { return shadeOffsetPerc; } set { shadeOffsetPerc = value; } }

		/// <summary>
		/// Text outline width
		/// </summary>
		public float OutlineWidth { get { return outlineWidth; } set { outlineWidth = value; } }
		
		/// <summary>
		/// Whether the text has an outline or not.
		/// </summary>
		public bool DrawOutline { get { return drawOutline; } set { drawOutline = value; } }

		/// <summary>
		/// Whether the text shadow is drawn or not
		/// </summary>
		public bool DrawShadow { get { return drawShadow; } set { drawShadow = value; } }

        #endregion
    
		#region --- TextStyleKind Handling ---

		/// <summary>
		/// Chose a TextStyle from one of the predefined styles.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextStyleKind TextStyleKind { get { return NameToKind(Name); } }

		internal static TextStyleKind NameToKind(string name)
		{
			try
			{
				string name1 = name.Replace(" ","");
				TypeConverter tc = new EnumConverter(typeof(TextStyleKind));
				return (TextStyleKind)tc.ConvertFromString(name1);
			}
			catch
			{
				return TextStyleKind.Custom;
			}
		}
		internal static string KindToName(TextStyleKind kind)
		{
			switch(kind)
			{
				case TextStyleKind.BlackIce: return "Black Ice"; 
				case TextStyleKind.ArcticWhite: return "Arctic White"; 
				case TextStyleKind.ArcticWhiteNumericBook: return "Arctic White Numeric Book"; 
				case TextStyleKind.BlackIceNumericBook: return "Black Ice Numeric Book"; 
				case TextStyleKind.BlackIceNumericMechanical: return "Black Ice Numeric Mechanical"; 
				case TextStyleKind.ArcticWhiteNumericLCD: return "Arctic White Numeric LCD"; 
				default: return kind.ToString();
			}
		}

		#endregion
	}

        
    /// <summary>
    /// Contains a collection of <see cref="TextStyle"/> objects.
    /// </summary>
	[Serializable]
	public class TextStyleCollection : NamedObjectCollection
    {
        internal TextStyleCollection(bool populateInitialContents) : base(populateInitialContents) { }
        public TextStyleCollection() : this(false) { }

        internal override void PopulateInitialContents()
        {
			Add(new TextStyle("Default"));
			TextStyle dils = new TextStyle("DefaultIndicatorLabelStyle");
			dils.FontSizePerc = 4;
			dils.FontColor = Color.FromArgb(128,128,128,128);

			Add(dils);
		}
        internal override NamedObject CreateNewMember()
        {
			TextStyle style = new TextStyle();
			SelectGenericNewName(style);
			Add(style);
			return style;
		}

		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates new member of the collection by cloning the member called "Default". If member named "Default" doesn't exist, a new
		/// instance of MarkerStyle is created.
		/// </summary>
		/// <param name="newMemberName">Name of the new member.</param>
		/// <returns>Returns the created object.</returns>
		public TextStyle AddNewMember(string newMemberName)
		{
			TextStyle newMember = AddNewMemberFrom(newMemberName,"Default");
			if(newMember == null)
			{
				newMember = new TextStyle(newMemberName);
				Add(newMember);
			}
			return newMember;
			
		}

		/// <summary>
		/// Clones and stores the specified <see cref="TextStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new TextStyle AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName,oldMemberName) as TextStyle;
		}

		/// <summary>
		/// Clones and stores the specified <see cref="TextStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="textStyleKind"><see cref="TextStyleKind"/> of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public TextStyle AddNewMemberFrom(string newMemberName, TextStyleKind textStyleKind)
		{
			return base.AddNewMemberFrom(newMemberName,TextStyle.KindToName(textStyleKind)) as TextStyle;
		}

		#endregion
		
		/// <summary>
		/// Returns the TextStyle object specified in the TextStyleKind parameter
		/// </summary>
		/// <param name="ix">The TextStyle requestd</param>
		/// <returns>the requested TextStyle object</returns>
		public new TextStyle this[object ix]
        {
            get 
			{
				if(ix is TextStyleKind)
					ix = TextStyle.KindToName((TextStyleKind)ix);
				return base[ix] as TextStyle; 
			}
            set
			{
				if(ix is TextStyleKind)
					ix = TextStyle.KindToName((TextStyleKind)ix);
				base[ix] = value; 
			}
        }

    }

	internal class TextStyleNameConverter : SelectedNameConverter 
	{
		public TextStyleNameConverter() { }

		protected override NamedObjectCollection GetNamedCollection(SubGauge gauge)
		{
			return gauge.TextStyles;
		}
	}

    internal class FontNamesConverter : StringConverter
    {

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            FontCollection fonts = new InstalledFontCollection();
            FontFamily[] families = fonts.Families;
            string[] names = new string[families.Length];
            for (int i = 0; i < names.Length; i++)
                names[i] = families[i].Name;

            // Return the collection
            return new StandardValuesCollection(names);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext
            context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext
            context)
        {
            return true;
        }
    }

}
