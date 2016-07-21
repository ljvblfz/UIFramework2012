using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Marker style kind. Used to access predefined marker styles. 
	/// All custom created styles have this kind set to 'Custom'.
	/// </summary>
	public enum MarkerStyleKind
	{
		/// <summary>
		/// Block Marker.
		/// </summary>
		Block,
		/// <summary>
		/// Circular marker style.
		/// </summary>
		Circle,
		/// <summary>
		/// Bubble marker style.
		/// </summary>
		Bubble,	// this must be second in the list, because it is casted to the "ChartEngine.MarkerKind.Bubble"
		/// <summary>
		/// Diamond marker style.
		/// </summary>
		Diamond,
		/// <summary>
		/// Triangular marker style.
		/// </summary>
		Triangle,
		/// <summary>
		/// Inverted triangular marker style.
		/// </summary>
		InvertedTriangle,
		/// <summary>
		/// Left triangle marker style.
		/// </summary>
		LeftTriangle,
		/// <summary>
		/// Right triangle marker style.
		/// </summary>
		RightTriangle,
		/// <summary>
		/// Cross marker style.
		/// </summary>
		Cross,
		/// <summary>
		/// X-shape marker style.
		/// </summary>
		XShape,
		/// <summary>
		/// East arrow marker style.
		/// </summary>
		ArrowE,
		/// <summary>
		/// West arrow marker style.
		/// </summary>
		ArrowW,
		/// <summary>
		/// North arrow marker style.
		/// </summary>
		ArrowN,
		/// <summary>
		/// South arrow marker style.
		/// </summary>
		ArrowS,
		/// <summary>
		/// North-east arrow marker style.
		/// </summary>
		ArrowNE,
		/// <summary>
		/// North-west arrow marker style.
		/// </summary>
		ArrowNW,
		/// <summary>
		/// South-east arrow marker style.
		/// </summary>
		ArrowSE,
		/// <summary>
		/// South-west arrow marker style.
		/// </summary>
		ArrowSW,
		/// <summary>
		/// Marker style kind for custom marker styles
		/// </summary>
		Custom
	}

	/// <summary>
	/// Specifies the kind of marker to be used in a series.
	/// </summary>
	public enum MarkerKind
	{
		/// <summary>
		/// Block Marker.
		/// </summary>
		Block,
		/// <summary>
		/// Circular marker.
		/// </summary>
		Circle,
		/// <summary>
		/// Bubble marker
		/// </summary>
		Bubble,	// this must be second in the list, because it is casted to the "ChartEngine.MarkerKind.Bubble"
		/// <summary>
		/// Diamond marker.
		/// </summary>
		Diamond,
		/// <summary>
		/// Triangular marker.
		/// </summary>
		Triangle,
		/// <summary>
		/// Inverted triangular marker.
		/// </summary>
		InvertedTriangle,
		/// <summary>
		/// Left triangle marker.
		/// </summary>
		LeftTriangle,
		/// <summary>
		/// Right triangle marker
		/// </summary>
		RightTriangle,
		/// <summary>
		/// Cross marker.
		/// </summary>
		Cross,
		/// <summary>
		/// X-shape marker.
		/// </summary>
		XShape,
		/// <summary>
		/// East arrow marker.
		/// </summary>
		ArrowE,
		/// <summary>
		/// West arrow marker.
		/// </summary>
		ArrowW,
		/// <summary>
		/// North arrow marker.
		/// </summary>
		ArrowN,
		/// <summary>
		/// South arrow marker.
		/// </summary>
		ArrowS,
		/// <summary>
		/// North-east arrow marker.
		/// </summary>
		ArrowNE,
		/// <summary>
		/// North-west arrow marker.
		/// </summary>
		ArrowNW,
		/// <summary>
		/// South-east arrow marker.
		/// </summary>
		ArrowSE,
		/// <summary>
		/// South-west arrow marker.
		/// </summary>
		ArrowSW
	}

	/// <summary>
	/// Describes the style of the marker.
	/// </summary>
	[TypeConverter(typeof(MarkerStyleConverter))]
	public class MarkerStyle : NamedObjectBase,ICloneable
	{
		private MarkerKind	style;
		private Vector3D		markerSize;
		private ChartColor		surface;
		private bool			isDefault = false;
		private bool			removable = true;
		private Color			borderColor = Color.Black;
		private double			shadeWidth = 2;


		/// <summary>
		/// Initializes a new instance of the <see cref="MarkerStyle"/> class with a specified name.
		/// </summary>
		/// <param name="name">The name of this <see cref="MarkerStyle"/> object.</param>
		public MarkerStyle(String name) : this(name,MarkerKind.Bubble,new Vector3D(10,10,10),new ChartColor(0.5f,6,Color.Red)) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="MarkerStyle"/> class.
		/// </summary>
		public MarkerStyle() : this("") { }


		/// <summary>
		/// Initializes a new instance of the <see cref="MarkerStyle"/> class with speficied name, style, marker size and surface.
		/// </summary>
		/// <param name="name">The name of this <see cref="MarkerStyle"/> object.</param>
		/// <param name="style">The marker kind of this <see cref="MarkerStyle"/> object.</param>
		/// <param name="markerSize">The size of the marker.</param>
		/// <param name="chartColor">The color of the marker.</param>
		public MarkerStyle(String name, MarkerKind style, Vector3D markerSize, ChartColor chartColor) : base(name)
		{
			this.style = style;
			this.markerSize = markerSize;
			this.surface = chartColor;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MarkerStyle"/> class with speficied name, style, marker size and surface.
		/// </summary>
		/// <param name="name">The name of this <see cref="MarkerStyle"/> object.</param>
		/// <param name="style">The marker kind of this <see cref="MarkerStyle"/> object.</param>
		/// <param name="size">The size of the marker.</param>
		/// <param name="chartColor">The color of the marker.</param>
		public MarkerStyle(String name, MarkerKind style, double size, ChartColor chartColor) 
			:this(name, style,new Vector3D(size,size,size),chartColor) { }

		#region --- Handling enum LabelStyleKind ---

		private static string[] names = new string[]
			{
				"Block",
				"Circle",
				"Bubble",
				"Diamond",
				"Triangle",
				"InvertedTriangle",
				"LeftTriangle",
				"RightTriangle",
				"Cross",
				"XShape",
				"ArrowE",
				"ArrowW",
				"ArrowN",
				"ArrowS",
				"ArrowNE",
				"ArrowNW",
				"ArrowSE",
				"ArrowSW",
				"Custom"
			};
		
		private static MarkerStyleKind[] kinds = new MarkerStyleKind[]
			{
				MarkerStyleKind.Block,
				MarkerStyleKind.Circle,
				MarkerStyleKind.Bubble,
				MarkerStyleKind.Diamond,
				MarkerStyleKind.Triangle,
				MarkerStyleKind.InvertedTriangle,
				MarkerStyleKind.LeftTriangle,
				MarkerStyleKind.RightTriangle,
				MarkerStyleKind.Cross,
				MarkerStyleKind.XShape,
				MarkerStyleKind.ArrowE,
				MarkerStyleKind.ArrowW,
				MarkerStyleKind.ArrowN,
				MarkerStyleKind.ArrowS,
				MarkerStyleKind.ArrowNE,
				MarkerStyleKind.ArrowNW,
				MarkerStyleKind.ArrowSE,
				MarkerStyleKind.ArrowSW,
				MarkerStyleKind.Custom
			};

		internal static string NameOf(MarkerStyleKind kind)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(kind==kinds[i])
					return names[i];
			}
			throw new Exception("Implementation: arrays names/kinds in class 'MarkerStyle' mismatch");
		}

		internal new static MarkerStyleKind KindOf(string name)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(name==names[i])
					return kinds[i];
			}
			return MarkerStyleKind.Custom;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MarkerStyleKind MarkerStyleKind
		{
			get
			{
				return MarkerStyle.KindOf(Name);
			}
			set
			{
				Name = MarkerStyle.NameOf(value);
			}
		}

		#endregion

		#region --- Properties ---

		//[DefaultValue(true)]
		//[Browsable(false)]
		internal bool		Removable   { get { return removable; } set { removable = value; }}
		internal bool		IsDefault	{ get { return isDefault; }	set { isDefault = value; } }

		/// <summary>
		/// Gets or sets the name of this <see cref="MarkerStyle"/> object.
		/// </summary>
		public new string Name
		{
			get { return base.Name; }
			set 
			{ 
				if(base.Name == value)
					return;
				// We don't allow changing the name for styles named after MarkerKind enumeration
				// except if we are in object building (like in Xml serialization) when the ownership info is 
				// not complete
				if(!Removable && OwningChart != null)
					throw new Exception("Name cannot be modified for this predefined marker style");
				isDefault = false; 
				base.Name = value; 
			} 
		}

		/// <summary>
		/// Gets or sets the marker kind of this <see cref="MarkerStyle"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[SRDescription("MarkerStyleMarkerKindDescr")]
		public MarkerKind	MarkerKind	
		{
			get { return style; }
			set 
			{ 
				if(style == value)
					return;
				// We don't allow changing the marker kind for styles named after MarkerKind enumeration
				// except if we are in object building (like in Xml serialization) when the ownership info is 
				// not complete
				if(!Removable && OwningChart != null)
					throw new Exception("MarkerKind cannot be modified for this predefined marker style");
				isDefault = false; 
				style = value; 
			} 
		}
		/// <summary>
		/// Gets or sets the size of markers drawn with this <see cref="MarkerStyle"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[DefaultValue(typeof(Vector3D), "(10,10,10)")]
		[NotifyParentProperty(true)]
		[Description("The size of the marker.")]
		public Vector3D		MarkerSize	{ get { return markerSize; }	set { if(markerSize != value)isDefault = false; markerSize = value; } }

		/// <summary>
		/// Gets or sets the ChartColor of markers drawn with this <see cref="MarkerStyle"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[Description("The ChartColor of the marker.")]
		public ChartColor		ChartColor		{ get { return surface; }		set { isDefault = false; surface = value; } }

		/// <summary>
		/// Gets or sets the border color of with this <see cref="MarkerStyle"/> object. It is applied only in those styles taht use border line.
		/// </summary>
		[SRCategory("CatAppearance")]
		[Description("The color of the marker border.")]
		[DefaultValue(typeof(Color), "Black")]
		public Color BorderColor { get { return borderColor; } set { borderColor = value; } }
		
		/// <summary>
		/// Gets or sets the shade width of with this <see cref="MarkerStyle"/> object. It is applied only in 2D markers.
		/// </summary>
		[SRCategory("CatAppearance")]
		[Description("The width of the marker shade (for 2D markers).")]
		[DefaultValue(2.0)]
		public double ShadeWidth { get { return shadeWidth; } set { shadeWidth = value; } }
		#endregion
		
		/// <summary>
		/// Creates an exact copy of this <see cref="MarkerStyle"/> object.
		/// </summary>
		/// <returns>An object that can be cast to <see cref="MarkerStyle"/> object.</returns>
		public object Clone()
		{
			ChartColor s = (ChartColor) surface.Clone();
			MarkerStyle newMS = new MarkerStyle(Name,style,new Vector3D(markerSize),s);
			newMS.IsDefault = IsDefault;
			return newMS;
		}

		private bool ShouldBrowseMarkerKind()	{ return Removable; }
		private bool ShouldBrowseName()			{ return Removable; }

		#region --- XML Serialization ---
		internal void Serialize(XmlCustomSerializer S)
		{
			S.AttributeProperty(this,"Name");
			S.AttributeProperty(this,"MarkerKind");
			S.AttributeProperty(this,"MarkerSize");
			S.AttributeProperty(this,"Surface");
		}
		#endregion
	}

}
