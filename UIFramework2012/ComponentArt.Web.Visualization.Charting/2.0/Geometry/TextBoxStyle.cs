using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	public enum TextBoxOrientation
	{
		Horizontal,
		Vertical90,
		Vertical270
	};

	public enum TextBoxStyleKind
	{
		Default,
		TextOnly,
		Rectangle,
		RoundedRectangle,
		Ellipse,
		OneSideLine,
		Custom
	};

	public enum TextBoxBorderKind
	{
		None,
		Rectangle,
		RoundedRectangle,
		Ellipse,
		OneSideLine
	};

	public enum TextBoxAnchorKind
	{
		None,
		Line,
		LineArrow,
		LineDiamond,
		LineSquare,
		LineCircle,
		Cartoon,
		CartoonClouds
	};


	[RefreshProperties (RefreshProperties.All)]
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	public class TextBoxStyle : NamedObjectBase
	{
		// --- Font
		private string fontName = "Arial";
		private FontStyle fontStyle = FontStyle.Regular;
		private float fontSize = 10; // pts
		private Color fontColor = Color.FromArgb(255,0,0,0);

		// --- Border
		private bool borderVisible = true;
		private float borderWidth = 1; // pt
		private DashStyle dashStyle = DashStyle.Solid;
		private Color borderColor = Color.Black;

		// --- Box
		private TextBoxBorderKind textBoxBorderKind = TextBoxBorderKind.Rectangle;
		internal const double textMargin = 3.0; // pts
		private double leftMargin = textMargin;		// 10pt
		private double rightMargin = textMargin;	// 10pt
		private double topMargin = textMargin;		// 10pt
		private double bottomMargin = textMargin;	// 10pt
		private double maxBoxWidth = 0; // default is one-line annotations
		private double shadeWidth = 0;

		// --- Background
		private bool fillBackground = true;
		private GradientKind gradientKind = GradientKind.None;
		private Color color = Color.FromArgb(255,255,255);
		private Color endingColor = Color.FromArgb(255,255,255);

		// --- Pointer
		private TextBoxAnchorKind textBoxAnchorKind = TextBoxAnchorKind.Line;
		private double defaultDxPts = 10;
		private double defaultDyPts = -20;
		private TextReferencePoint textReferencePoint = TextReferencePoint.CenterBottom;

		// --- Alignment
		private StringAlignment alignment = StringAlignment.Center;
		private TextBoxOrientation orientation = TextBoxOrientation.Horizontal;

		// Chage control

		private bool changed = true; // This causes serialization by default
		
		#region --- Construction and clonning ---
		public TextBoxStyle(string styleName) : base(styleName)
		{ }

		/// <summary>
		/// Constructor used only in serialization.
		/// </summary>
		public TextBoxStyle() : this(null)
		{ }
		internal TextBoxStyle Clone()
		{
			return MemberwiseClone() as TextBoxStyle;
		}
		#endregion

		#region --- Properties ---

		[DefaultValue("Arial")]
		public string FontName { get { return fontName; } set { if(fontName != value) changed = true; fontName = value; } }
		[DefaultValue(FontStyle.Regular)]
		public FontStyle FontStyle { get { return fontStyle; } set { if(fontStyle != value) changed = true; fontStyle = value; } }
		[DefaultValue(10)]
		public float FontSize { get { return fontSize; } set { if(fontSize != value) changed = true; fontSize = value; } }
		[DefaultValue(typeof(Color), "Black")]
		public Color TextColor { get { return fontColor; } set { if(fontColor != value) changed = true; fontColor = value; } }

		[DefaultValue(true)]
		public bool BorderVisible { get { return borderVisible; } set { if(borderVisible != value) changed = true; borderVisible = value; } }
		[DefaultValue(1)]
		public float BorderWidth { get { return borderWidth; } set { if(borderWidth != value) changed = true; borderWidth = value; } }
		[DefaultValue(typeof(DashStyle),"Solid")]
		public DashStyle BorderDashStyle { get { return dashStyle; } set { if(dashStyle != value) changed = true; dashStyle = value; } }
		[DefaultValue(typeof(Color),"Black")]
		public Color BorderColor { get { return borderColor; } set { if(borderColor != value) changed = true; borderColor = value; } }

		[DefaultValue(true)]
		public bool FillBackground { get { return fillBackground; } set { if(fillBackground != value) changed = true; fillBackground = value; } }
		[DefaultValue(typeof(GradientKind), "None")]
		public GradientKind GradientKind { get { return gradientKind; } set { if(gradientKind != value) changed = true; gradientKind = value; } }
		[DefaultValue(typeof(Color), "White")]
		public Color BackgroundColor { get { return color; } set { if(color != value) changed = true; color = value; } }
		[DefaultValue(typeof(Color), "White")]
		public Color BackgroundEndingColor { get { return endingColor; } set { if(endingColor != value) changed = true; endingColor = value; } }

		[DefaultValue(typeof(TextBoxBorderKind),"Rectangle")]
		public TextBoxBorderKind TextBoxBorderKind { get { return textBoxBorderKind; } set { if(textBoxBorderKind != value) changed = true; textBoxBorderKind = value; } }
		[DefaultValue(TextBoxStyle.textMargin)]
		public double LeftMargin { get { return leftMargin; } set { if(leftMargin != value) changed = true; leftMargin = value; } }
		[DefaultValue(TextBoxStyle.textMargin)]
		public double RightMargin { get { return rightMargin; } set { if(rightMargin != value) changed = true; rightMargin = value; } }
		[DefaultValue(TextBoxStyle.textMargin)]
		public double TopMargin { get { return topMargin; } set { if(topMargin != value) changed = true; topMargin = value; } }
		[DefaultValue(TextBoxStyle.textMargin)]
		public double BottomMargin { get { return bottomMargin; } set { if(bottomMargin != value) changed = true; bottomMargin = value; } }
		[DefaultValue(0)]
		public double MaxBoxWidth { get { return maxBoxWidth; } set { if(maxBoxWidth != value) changed = true; maxBoxWidth = value; } }
		[DefaultValue(0)]
		public double ShadeWidth { get { return shadeWidth; } set { if(shadeWidth != value) changed = true; shadeWidth = value; } }

		[DefaultValue(typeof(TextBoxAnchorKind),"Line")]
		public TextBoxAnchorKind TextBoxAnchorKind { get { return textBoxAnchorKind; } set { if(textBoxAnchorKind != value) changed = true; textBoxAnchorKind = value; } }
		[DefaultValue(10)]
		public double DefaultDxPts { get { return defaultDxPts; } set { if(defaultDxPts != value) changed = true; defaultDxPts = value; } }
		[DefaultValue(-20)]
		public double DefaultDyPts { get { return defaultDyPts; } set { if(defaultDyPts != value) changed = true; defaultDyPts = value; } }
		[DefaultValue(typeof(TextReferencePoint),"CenterBottom")]
		public TextReferencePoint TextReferencePoint { get { return textReferencePoint; } set { if(textReferencePoint != value) changed = true; textReferencePoint = value; } }

		[DefaultValue(typeof(StringAlignment),"Center")]
		public StringAlignment Alignment { get { return alignment; } set { alignment = value; } }

		[DefaultValue(typeof(TextBoxOrientation),"Horizontal")]
		public TextBoxOrientation Orientation { get { return orientation; } set { orientation = value; } }
		
		internal bool Changed { get { return changed; } set { changed = value; } }

		#endregion

		#region --- Handling Predefined Styles ---

		private static string[] names = new String[]
			{
				"Default",
				"TextOnly",
				"Rectangle",
				"RoundedRectangle",
				"Ellipse",
				"OneSideLine",
				"Custom"
			};
		private static TextBoxStyleKind[] kinds = new TextBoxStyleKind[]
			{
				TextBoxStyleKind.Default,
				TextBoxStyleKind.TextOnly,
				TextBoxStyleKind.Rectangle,
				TextBoxStyleKind.RoundedRectangle,
				TextBoxStyleKind.Ellipse,
				TextBoxStyleKind.OneSideLine,
				TextBoxStyleKind.Custom
			};

		internal static TextBoxStyleKind KindOf(string styleName)
		{
			for(int i=0; i<names.Length; i++)
			{
				if(styleName == names[i])
					return kinds[i];
			}
			return TextBoxStyleKind.Custom;
		}
        /// <summary>
        /// Creates text box style name from <see cref="TextBoxStyleKind"/>
        /// </summary>
		public static string NameOf(TextBoxStyleKind kind)
		{
			for(int i=0; i<kinds.Length; i++)
			{
				if(kind == kinds[i])
					return names[i];
			}
			throw new Exception("Implementation: table (kinds,names) is not valid in 'TextBoxStyle'");
		}

		#endregion

		#region --- XML Serialization ---
//
//		internal void CreateDOM(XmlElement parent)
//		{
//		}
//
//		internal void Serialize(XmlCustomSerializer S)
//		{
//		}
//
//		internal static SeriesStyle CreateFromDOM(XmlElement root)
//		{
//			return null;
//		}

		#endregion

	}

	public sealed class TextBoxStyleCollection : NamedCollection
	{
		internal TextBoxStyleCollection(Object owner, bool initialize) 
			: base (typeof(TextBoxStyle),owner) 
		{
			if(initialize)
				InitializeContents();
		}

		internal TextBoxStyleCollection(Object owner) : this(owner,true) { }
		
		internal TextBoxStyleCollection() : this(null) { }

		/// <summary>
		/// Indicates the <see cref="TextBoxStyle"/> at the specified indexed location 
		/// in the <see cref="TextBoxStyleCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name 
		/// or a <see cref="TextBoxStyleKind"/> to retrieve 
		/// a <see cref="TextBoxStyle"/> from the <see cref="TextBoxStyleCollection"/> object.</param>
		public new TextBoxStyle this[object index]   
		{ 
			get 
			{ 
				if(index is TextBoxStyleKind)
					index = ((TextBoxStyleKind)index).ToString();
				return (TextBoxStyle)base[index]; 
			} 
			set 
			{
				if(index is TextBoxStyleKind)
					index = ((TextBoxStyleKind)index).ToString();
				base[index] = value; 
			} 
		}

		#region --- Member Creation Interface ---

		public TextBoxStyle CreateNew(string newMemberName)
		{
			return CreateFrom(newMemberName,"Default");
		}

		/// <summary>
		/// Clones and stores the specified <see cref="TextBoxStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned style.</param>
		/// <param name="oldMemberName">Name of the original style.</param>
		/// <returns>Returns the cloned style.</returns>
		/// <remarks>If the original style does not exist, the function returns null. 
		/// If the collection already contents the style with the cloned style name, the old style will be overriden.
		/// </remarks>
		public TextBoxStyle CreateFrom(string newMemberName, string oldMemberName)
		{
			TextBoxStyle original = this[oldMemberName];
			if(original == null)
				return null;
			StyleCloner cloner = new StyleCloner();
			TextBoxStyle clonedStyle = cloner.Clone(original) as TextBoxStyle;
			clonedStyle.Name = newMemberName;
			original.OwningCollection.Add(clonedStyle);

			return clonedStyle;
		}

		/// <summary>
		/// Clones and stores the specified <see cref="TextBoxStyleStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned style.</param>
		/// <param name="oldMemberKind"><see cref="TextBoxStyleKind"/> of the original style.</param>
		/// <returns>Returns the cloned style.</returns>
		/// <remarks> 
		/// If the collection already contents the style with the cloned style name, the old style will be overriden.
		/// </remarks>
		public TextBoxStyle CreateFrom(string newMemberName, TextBoxStyleKind oldMemberKind)
		{
			return CreateFrom(newMemberName,oldMemberKind.ToString());
		}
		#endregion

		#region --- XML Serialization ---

		internal void Serialize(XmlCustomSerializer S)
		{
		}


		internal void CreateDOM(XmlElement parent)
		{
		}

		internal static SeriesStyleCollection CreateFromDOM(XmlElement root)
		{
			SeriesStyleCollection styles = new SeriesStyleCollection(null);
			XmlNode subNode = root.FirstChild;
			while(subNode != null)
			{
				if(subNode.Name.ToLower() == "textboxstyle")
				{
					SeriesStyle S = SeriesStyle.CreateFromDOM(subNode as XmlElement);
					if(S != null)
					{
						styles.Add(S);
					}
				}
				subNode = subNode.NextSibling;
			}
			return styles;
		}
		#endregion

		#region --- Collection Initialization ---

		private void InitializeContents()
		{
			TextBoxStyle style = new TextBoxStyle("Default");
			style.Changed = false;
			Add(style);

			style = new TextBoxStyle("TextOnly");
			style.BorderVisible = false;
			style.FillBackground = false;
			style.TextBoxBorderKind = TextBoxBorderKind.None;
			style.TextBoxAnchorKind = TextBoxAnchorKind.None;
			style.Changed = false;
			Add(style);

			style = new TextBoxStyle("Rectangle");
			style.TextBoxAnchorKind = TextBoxAnchorKind.Cartoon;
			style.Changed = false;
			Add(style);

			style = new TextBoxStyle("RoundedRectangle");
			style.TextBoxBorderKind = TextBoxBorderKind.RoundedRectangle;
			style.TextBoxAnchorKind = TextBoxAnchorKind.Cartoon;
			style.Changed = false;
			Add(style);

			style = new TextBoxStyle("Ellipse");
			style.TextBoxBorderKind = TextBoxBorderKind.Ellipse;
			style.TextBoxAnchorKind = TextBoxAnchorKind.CartoonClouds;
			style.Changed = false;
			Add(style);

			style = new TextBoxStyle("OneSideLine");
			style.TextBoxBorderKind = TextBoxBorderKind.OneSideLine;
			style.TextBoxAnchorKind = TextBoxAnchorKind.LineArrow;
			style.FillBackground = false;
			style.Changed = false;
			Add(style);
		}
		#endregion
	}

}
