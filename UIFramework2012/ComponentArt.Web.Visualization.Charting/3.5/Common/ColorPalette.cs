using System;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using ComponentArt.Web.Visualization.Charting.Design;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Special color of the chart
	/// </summary>
	public enum SpecialColor
	{
		/// <summary>
		/// CoordinatePlane
		/// </summary>
		CoordinatePlane,
		/// <summary>
		/// CoordinatePlaneSecondary
		/// </summary>
		CoordinatePlaneSecondary,
		/// <summary>
		/// CoordinateLine
		/// </summary>
		CoordinateLine,
		/// <summary>
		/// AxisLine
		/// </summary>
		AxisLine,
		/// <summary>
		/// CoodinateLabelFont
		/// </summary>
		CoodinateLabelFont,
		/// <summary>
		/// DataLabelFont
		/// </summary>
		DataLabelFont,
		/// <summary>
		/// Chart background color, also starting color of the background gradient
		/// </summary>
		BackColor,
		/// <summary>
		/// Ending color of the chart background gradient
		/// </summary>
		BackGradientEndingColor,
		/// <summary>
		/// Color of 2D objects border
		/// </summary>
		TwoDObjectBorder,
		/// <summary>
		/// Title font color
		/// </summary>
		TitleFont,
		/// <summary>
		/// Frame font color
		/// </summary>
		FrameFont,
		/// <summary>
		/// Frame color
		/// </summary>
		Frame,
		/// <summary>
		/// Secondary frame color
		/// </summary>
		FrameSecondary,
		/// <summary>
		///  Legend border line color
		/// </summary>
		LegendBorder,
		/// <summary>
		/// Legend background color
		/// </summary>
		LegendBackground,
		/// <summary>
		/// Legend font color
		/// </summary>
		LegendFont
	}

	/// <summary>
	/// Enumeration type for accessing predefined color palettes
	/// </summary>
	/// <remarks>
	/// User defined palettes have PaletteKind = <see cref="PaletteKind.Custom"/>.
	/// </remarks>
	public enum PaletteKind
	{
		Default,
		Fire,
		Organic,
		Electricity,
		Excel,
		Earth,
		CoolBlue,
		Green,
		Pastel,
		Contrast,
		Greyscale,
		ComponentArt,
		Custom
	};

	/// <summary>
	/// Specifies the color palette to be used in a chart.
	/// </summary>
	[Serializable()]
	[System.ComponentModel.TypeConverter(typeof(Design.GenericExpandableObjectConverter))]
	[System.ComponentModel.EditorAttribute(typeof(PaletteEditor), typeof(System.Drawing.Design.UITypeEditor))]
	public class Palette : INamedObject, ICloneable
	{ 
		ChartColorCollection	specialColors = new ChartColorCollection(new Color[Enum.GetValues(typeof(SpecialColor)).Length]);
		ChartColorCollection	colors, secondaryColors;
		ChartColorCollection	colorsDef, secondaryColorsDef, specialColorsDef;
		int						alphaDef;

		int		currentColorIndex;
		int		alpha=255; 
		bool	removable = true;

		bool	hasChanged = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="Palette"/> class.
		/// </summary>
		#region --- Construction and Clonning ---
		public Palette() : this("") { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Palette"/> class.
		/// </summary>
		/// <param name="name">The name of the <see cref="Palette"/> object.</param>
		public Palette(string name) : this (name, Palette.stdPrimary,Palette.stdSecondary,Palette.stdSpecial)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Palette"/> class.
		/// </summary>
		/// <param name="name">The name of the <see cref="Palette"/> object.</param>
		/// <param name="colors">The array of <see cref="Color"/>s that represent the primary colors in the <see cref="Palette"/> object.</param>
		/// <param name="secondaryColors">The array of <see cref="Color"/>s that represent the secondary colors in the <see cref="Palette"/> object.</param>
		/// <param name="specialColors">The array of <see cref="Color"/>s that represent the special colors in the <see cref="Palette"/> object.</param>
		public Palette(string name, Color[] colors, Color[] secondaryColors, Color[] specialColors)
		{
			Name = name;
			
			if(colors == null)
				throw new ArgumentException("The 'colors' argument of 'Palette' constructor must not be null");
			if(secondaryColors == null)
				throw new ArgumentException("The 'secondaryColors' argument of 'Palette' constructor must not be null");
			if(secondaryColors.Length != colors.Length)
				throw new ArgumentException("The 'colors' and 'secondaryColors' arguments of 'Palette' constructor must have same length");
			
			SetInitialSpecialColors();

			if(specialColors != null)
				this.specialColors = new ChartColorCollection(specialColors);
			this.colors = new ChartColorCollection(colors);
			this.secondaryColors = new ChartColorCollection(secondaryColors);

			currentColorIndex = -1;
			SaveDefaults();
		}

		private void SaveDefaults()
		{
			colorsDef = (ChartColorCollection)colors.Clone();
			secondaryColorsDef = (ChartColorCollection)SecondaryColors.Clone();
			specialColorsDef = (ChartColorCollection)specialColors.Clone();
			alphaDef = alpha;
		}

		/// <summary>
		/// Creates an exact copy of this Brush object. 
		/// </summary>
		/// <returns>The new <see cref="Palette"/> object that this method creates.</returns>
		public object Clone() 
		{
			Palette P = new Palette();
			P.Name = (string)Name.Clone();
			P.colors = (ChartColorCollection) colors.Clone();
			P.secondaryColors = (ChartColorCollection) secondaryColors.Clone();
			P.specialColors = (ChartColorCollection) specialColors.Clone();
			P.Alpha = Alpha;
			P.SetChanged(HasChanged);
			return P;
		}

		private void SetInitialSpecialColors()
		{
			// default special colors

			specialColors = new ChartColorCollection(new Color[Enum.GetValues(typeof(SpecialColor)).Length]);
			specialColors[(int)SpecialColor.CoordinatePlane] = Color.White;
			specialColors[(int)SpecialColor.CoordinatePlaneSecondary] = Color.FromArgb(100,100,255);
			specialColors[(int)SpecialColor.CoordinateLine] = Color.DarkGray;
			specialColors[(int)SpecialColor.CoodinateLabelFont] = Color.Gray;
			specialColors[(int)SpecialColor.DataLabelFont] = Color.Gray;
		}
		#endregion

		#region --- Enum Handling ---


		static string[] names = new string[]
			{
				"Default",
				"Fire",
				"Organic",
				"Electricity",
				"Excel",
				"Earth",
				"Cool Blue",
				"Green",
				"Pastel",
				"Contrast",
				"Greyscale",
				"ComponentArt",
				"Custom"
			};

		static PaletteKind[] kinds = new PaletteKind[]
			{
				PaletteKind.Default,
				PaletteKind.Fire,
				PaletteKind.Organic,
				PaletteKind.Electricity,
				PaletteKind.Excel,
				PaletteKind.Earth,
				PaletteKind.CoolBlue,
				PaletteKind.Green,
				PaletteKind.Pastel,
				PaletteKind.Contrast,
				PaletteKind.Greyscale,
				PaletteKind.ComponentArt,
				PaletteKind.Custom
			};

        /// <summary>
        /// Convertes palette kind into palette name.
        /// </summary>
    	public static string NameOf(PaletteKind kind)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(kind==kinds[i])
					return names[i];
			}
			throw new Exception("Implementation: arrays names/kinds in class 'Grid' mismatch");
		}
        /// <summary>
        /// Converts palette name into <see cref="PaletteKind"/>.
        /// </summary>
        /// <param name="name">Palette name to be converted.</param>
        /// <returns>
        /// Resulting pallete kind. All custom palettes have <see cref="PaletteKind"/> = <see cref="PaletteKind.Custom"/>.
        /// </returns>
		public static PaletteKind KindOf(string name)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(name==names[i])
					return kinds[i];
			}
			return PaletteKind.Custom;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PaletteKind Kind
		{
			get
			{
                return Palette.KindOf(Name);
			}
			set
			{
				Name = Palette.NameOf(value);
			}
		}

		#endregion

		#region --- INamedObject I/F Implementation ---
			internal NamedStyleInternal m_nsi = new NamedStyleInternal();

		private bool ShouldSerializeNamedCollection() { return false; }
		/// <summary>
		/// Gets the <see cref="NamedCollection"/> object the item belongs to.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public NamedCollection OwningCollection 
		{
			get {return m_nsi.NamedCollection;} set { m_nsi.NamedCollection = value; }
		}

		/// <summary>
		/// Gets or sets the name of the color palette.
		/// </summary>
		[SRCategory("CatGeneral")]
		[SRDescription("PaletteNameDescr")]
		public string Name 
		{
			get 
			{
				return m_nsi.Name; 
			}
			set 
			{
				if(m_nsi.Name != value)
					hasChanged = true;
				m_nsi.Name = value;
			}
		}
		#endregion

		#region ---- Palette Serialization ----

		private void ResetAlpha() {alpha=255;}
		private bool ShouldSerializeAlpha() {return alpha!=255;}
		private bool ShouldSerializePrimaryColors() { return PrimaryColors.HasChanged; }
		private bool ShouldSerializeSecondaryColors() { return SecondaryColors.HasChanged; }
		private bool ShouldSerializeSpecialColors() { return specialColors.HasChanged; }

		#endregion
		internal static Color[] stdPrimary = new Color [] 
		{
			Color.Red,
			Color.Green,
			Color.FromArgb(0,100,200),
			Color.FromArgb(255,130,0),
			Color.Sienna,
			Color.Goldenrod,
			Color.Orange,
			Color.AntiqueWhite,
			Color.BlueViolet,
			Color.Chocolate
		}; 

		internal static Color[] stdSecondary = new Color [] 
		{
			Color.Yellow,
			Color.Yellow,
			Color.Yellow,
			Color.Yellow,
			Color.Yellow,
			Color.Yellow,
			Color.Yellow,
			Color.Yellow,
			Color.Yellow,
			Color.Yellow
		};

		internal static Color[] stdSpecial = new Color [] 
		{
			Color.White,
			Color.FromArgb(160,200,255),
			Color.DarkGray,
			Color.DarkGray,
			Color.Black,
			Color.Black,
			Color.FromArgb(240, 240, 240),
			Color.White,
			Color.FromArgb(200,0,0,0),
			Color.Black,
			Color.Black,
			Color.White,
			Color.DarkGray,
			Color.FromArgb(64,64,64),
			Color.FromArgb(255,255,196),
			Color.Black
		};

		#region --- Properties ---
		public override string ToString() { return Name; }
		internal bool Removable 
		{
			get 
			{
				return removable;
			}
			set 
			{
				removable = value;
			}
		}

		internal ChartBase OwningChart
		{
			get
			{
				PaletteCollection owner = (PaletteCollection) OwningCollection;
				return owner.Owner as ChartBase;
			}
		}

		/// <summary>
		/// Gets or sets the collection of primary colors of this palette.
		/// </summary>
		/// <remarks>
		/// Single color from this collection is assigned to a sigle series in all compositions except concentric (pie and doughnut style). In concentric composition single color is assigned to a single <see cref="DataPoint"/>.
		/// </remarks>
		[SRCategory("CatColors")]
		[SRDescription("PalettePrimaryColorsDescr")]
		[TypeConverter(typeof(ColorArrayConverter))]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.Repaint)]
		public ChartColorCollection PrimaryColors 
		{
			get 
			{
				return colors;
			}
			set 
			{
				colors = (ChartColorCollection)value.Clone();
				colors.HasChanged = true;
			}
		}

		/// <summary>
		/// Gets or sets the collection of secondary colors of this palette.
		/// </summary>
		/// <remarks>
		/// Single color from this collection is assigned to a sigle series in all compositions except concentric (pie and doughnut style). In concentric composition single color is assigned to a single <see cref="DataPoint"/>.
		/// </remarks>
		[SRCategory("CatColors")]
		[SRDescription("PaletteSecondaryColorsDescr")]
		[TypeConverter(typeof(ColorArrayConverter))]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.Repaint)]
		public ChartColorCollection SecondaryColors 
		{
			get 
			{
				return secondaryColors;
			}
			set 
			{
				secondaryColors = (ChartColorCollection)value.Clone();
				secondaryColors.HasChanged = true;
			}
		}

		internal string PrimaryColorsAsString
		{
			get
			{
				ColorArrayConverter cac = new ColorArrayConverter();
				return cac.ConvertToString(colors);
			}
			set
			{
				ColorArrayConverter cac = new ColorArrayConverter();
				colors = (ChartColorCollection) cac.ConvertFromString(value);
				colors.HasChanged = true;
			}
		}

		internal string SecondaryColorsAsString
		{
			get
			{
				ColorArrayConverter cac = new ColorArrayConverter();
				return cac.ConvertToString(secondaryColors);
			}
			set
			{
				ColorArrayConverter cac = new ColorArrayConverter();
				secondaryColors = (ChartColorCollection) cac.ConvertFromString(value);
				secondaryColors.HasChanged = true;
			}
		}


		/// <summary>
		/// Gets or sets the primary coordinate plane <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Coordinate plane color")]
		public Color CoordinatePlaneColor 
		{
			get { return specialColors[(int)SpecialColor.CoordinatePlane].Color; }
			set {specialColors[(int)SpecialColor.CoordinatePlane].Color = value;}
		}

		/// <summary>
		/// Gets or sets the secondary coordinate plane <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Coordinate plane secondary color")]
		public Color CoordinatePlaneSecondaryColor 
		{
			get {return specialColors[(int)SpecialColor.CoordinatePlaneSecondary].Color;}
			set {specialColors[(int)SpecialColor.CoordinatePlaneSecondary].Color = value;}
		}

		/// <summary>
		/// Gets or sets the primary coordinate plane <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Coordinate line color")]
		public Color CoordinateLineColor 
		{
			get {return specialColors[(int)SpecialColor.CoordinateLine].Color;}
			set {specialColors[(int)SpecialColor.CoordinateLine].Color = value;}
		}

		/// <summary>
		/// Gets or sets the primary coordinate plane <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Axis line color")]
		public Color AxisLineColor 
		{
			get {return specialColors[(int)SpecialColor.AxisLine].Color;}
			set {specialColors[(int)SpecialColor.AxisLine].Color = value;}
		}

		/// <summary>
		/// Gets or sets the secondary coordinate plane <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Coordinate label font color")]
		public Color CoodinateLabelFontColor 
		{
			get {return specialColors[(int)SpecialColor.CoodinateLabelFont].Color;}
			set {specialColors[(int)SpecialColor.CoodinateLabelFont].Color = value;}
		}


		/// <summary>
		/// Gets or sets the data label font <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Data label font color")]
		public Color DataLabelFontColor 
		{
			get {return specialColors[(int)SpecialColor.DataLabelFont].Color;}
			set {specialColors[(int)SpecialColor.DataLabelFont].Color = value;}
		}

		/// <summary>
		/// Gets or sets the chart background (or background gradient starting) <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Background color or background gradient starting color")]
		public Color BackgroundColor 
		{
			get {return specialColors[(int)SpecialColor.BackColor].Color;}
			set {specialColors[(int)SpecialColor.BackColor].Color = value;}
		}

		/// <summary>
		/// Gets or sets the chart background gradient ending <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Background gradient ending color")]
		public Color BackgroundEndingColor 
		{
			get {return specialColors[(int)SpecialColor.BackGradientEndingColor].Color;}
			set {specialColors[(int)SpecialColor.BackGradientEndingColor].Color = value;}
		}

		/// <summary>
		/// Gets or sets the chart title font color <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Color of the border of 2D objects")]
		public Color TwoDObjectBorderColor 
		{
			get {return specialColors[(int)SpecialColor.TwoDObjectBorder].Color;}
			set {specialColors[(int)SpecialColor.TwoDObjectBorder].Color = value;}
		}

		/// <summary>
		/// Gets or sets the chart title font color <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Chart title font color")]
		public Color TitleFontColor 
		{
			get {return specialColors[(int)SpecialColor.TitleFont].Color;}
			set {specialColors[(int)SpecialColor.TitleFont].Color = value;}
		}
		/// <summary>
		/// Gets or sets the chart frame font <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Frame font color")]
		public Color FrameFontColor 
		{
			get {return specialColors[(int)SpecialColor.FrameFont].Color;}
			set {specialColors[(int)SpecialColor.FrameFont].Color = value;}
		}
		/// <summary>
		/// Gets or sets the chart frame <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Frame color")]
		public Color FrameColor 
		{
			get {return specialColors[(int)SpecialColor.Frame].Color;}
			set {specialColors[(int)SpecialColor.Frame].Color = value;}
		}
		
		/// <summary>
		/// Gets or sets the chart frame secondary <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Frame secondary color")]
		public Color FrameSecondaryColor 
		{
			get {return specialColors[(int)SpecialColor.FrameSecondary].Color;}
			set {specialColors[(int)SpecialColor.FrameSecondary].Color = value;}
		}
		
		/// <summary>
		/// Gets or sets the chart legend border <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Legend border color")]
		public Color LegendBorderColor 
		{
			get {return specialColors[(int)SpecialColor.LegendBorder].Color;}
			set {specialColors[(int)SpecialColor.LegendBorder].Color = value;}
		}
		/// <summary>
		/// Gets or sets the chart legend background <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Legend background color")]
		public Color LegendBackgroundColor 
		{
			get {return specialColors[(int)SpecialColor.LegendBackground].Color;}
			set {specialColors[(int)SpecialColor.LegendBackground].Color = value;}
		}
		
		/// <summary>
		/// Gets or sets the chart legend font <see cref="Color"/>.
		/// </summary>
		[Category("Special Colors")]
		[Description("Legend font color")]
		public Color LegendFontColor 
		{
			get {return specialColors[(int)SpecialColor.LegendFont].Color;}
			set {specialColors[(int)SpecialColor.LegendFont].Color = value;}
		}

		/// <summary>
		/// Gets or sets the alpha (transparency) value of the palette.
		/// </summary>
		[SRCategory("CatColors")]
		[SRDescription("PaletteAlphaDescr")]
		public int Alpha
		{
			get 
			{ 
				return alpha; 
			}
			set
			{
				if(value != alpha)
				{
					hasChanged = (alpha != value);
					alpha = value;
				}
			}
		}
		
		private int NumberOfColors { get { return colors.Count; } }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[System.ComponentModel.Browsable(false)]
		internal int CurrentColorIndex { get { return currentColorIndex; } }
		internal void ResetColorIndex() { currentColorIndex = -1; }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[System.ComponentModel.Browsable(false)]
		internal Color NextColor
		{
			get 
			{
				currentColorIndex = (currentColorIndex+1)%NumberOfColors;
				return colors[currentColorIndex].Color;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[System.ComponentModel.Browsable(false)]
		internal Color SecondaryColor
		{
			get 
			{
				if(currentColorIndex<0)
					currentColorIndex = 0;
				else 
					currentColorIndex = currentColorIndex%NumberOfColors;
				return secondaryColors[currentColorIndex].Color;
			}
		}

		#region --- Serialization Control Properties ---

		internal bool HasChanged 
		{
			get
			{
				return hasChanged ||
					!(colors.Eq(colorsDef) &&
					SecondaryColors.Eq(secondaryColorsDef) &&
					specialColors.Eq(specialColorsDef) &&
					alpha == alphaDef) ;
			}
		}

		internal void SetChanged(bool changed)
		{
			if(changed)
				hasChanged = true;
			else
			{
				hasChanged = false;
				PrimaryColors.HasChanged = false;
				secondaryColors.HasChanged = false;
				specialColors.HasChanged = false;
				SaveDefaults();
			}
		}

		#endregion

		#endregion

		#region --- Colors ---
        /// <summary>
        /// Get a specified color from <see cref="PrimaryColors"/> of this <see cref="Palette"/>.
        /// </summary>
        /// <param name="i">index of the color.</param>
        /// <returns><see cref="System.Drawing.Color"/> in this palette.</returns>
		public Color GetPrimaryColor(int i)
		{
			Color c = PrimaryColors[i%NumberOfColors].Color;
			if(alpha == 255)
				return c;
			else
				// Combine palette and color opacity
				return Color.FromArgb(alpha*c.A/255,c.R,c.G,c.B);
		}

        /// <summary>
        /// Get a specified color from <see cref="SecondaryColors"/> of this <see cref="Palette"/>.
        /// </summary>
        /// <param name="i">index of the color.</param>
        /// <returns><see cref="System.Drawing.Color"/> in this palette.</returns>
		public Color GetSecondaryColor(int i)
		{
			Color c = SecondaryColors[i%NumberOfColors].Color;
			if(alpha == 255)
				return c;
			else
				// Combine palette and color opacity
				return Color.FromArgb(alpha*c.A/255,c.R,c.G,c.B);
		}
		#endregion

		#region --- Serialization ---

		private bool ShouldSerializeMe { get { return HasChanged; } }
		internal void Serialize(XmlCustomSerializer S)
		{
			string[] atts = new String[] 
				{
					"Name",
					"Alpha",
					"AxisLineColor",
					"BackgroundColor",
					"BackgroundEndingColor",
					"CoordinatePlaneSecondaryColor",
					"CoordinatePlaneColor",
					"CoordinateLineColor",
					"CoodinateLabelFontColor",
					"DataLabelFontColor",
					"TwoDObjectBorderColor",
					"TitleFontColor",
					"FrameFontColor",
					"FrameColor",
					"FrameSecondaryColor",
					"LegendBorderColor",
					"LegendBackgroundColor",
					"LegendFontColor"

				};
			foreach(string attName in atts)
				S.AttributeProperty(this,attName);
			S.AttributeProperty(this,"PrimaryColors","PrimaryColorsAsString");
			S.AttributeProperty(this,"SecondaryColors","SecondaryColorsAsString");
		}

		#endregion
	}
}
