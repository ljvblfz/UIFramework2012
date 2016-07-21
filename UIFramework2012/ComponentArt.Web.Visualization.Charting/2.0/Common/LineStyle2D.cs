using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{

	/// <summary>
	/// 2D line style kind. Used to access predefined 2-dimensional line styles. 
	/// All custom created styles have this kind set to 'Custom'.
	/// </summary>
	public enum LineStyle2DKind
	{
		Default,
		DefaultForLine2DSeriesStyle,
		TwoDObjectBorder,
		CoordinateLine,
		AxisLine,
		Solid,
		Dash,
		Dot,
		DashDot,
		DashDotDot,
		Custom
	};

	/// <summary>
	/// Specifies the style of 2D lines in the chart.
	/// </summary>
	public class LineStyle2D : NamedObjectBase,ICloneable, IDisposable
	{	        
		const double	_WidthDefault = 1.0;
		const DashStyle _DashLineStyleDefault = DashStyle.Solid;
		const double	_ShadeWidthDefault = 0.0;
		const string	_ColorDefault = "Black";
    
		private Pen		pen = null;

		private double	width = _WidthDefault;
		private Color		color = Color.FromName(_ColorDefault);
		private DashStyle dashLineStyle = _DashLineStyleDefault;
		private double	shadeWidth = _ShadeWidthDefault;
		private bool		hasChanged = true;


		/// <summary>
		/// Initializes a new instance of the <see cref="LineStyle2D"/> class.
		/// </summary>
		/// <param name="name">The name of the <see cref="LineStyle2D"/> object.</param>
		/// <param name="width">The width of the line.</param>
		/// <param name="color">A <see cref="Color"/> structure that indicates the color of this <see cref="LineStyle2D"/> object.</param>
		/// <param name="dashLineStyle">The style used for dashed lines drawn with this <see cref="LineStyle2D"/> object. </param>
		public LineStyle2D(string name, double width,Color color, DashStyle dashLineStyle) : base(name) 
		{
			this.width = width;
			this.color = color;
			this.dashLineStyle = dashLineStyle;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="LineStyle2D"/> class.
		/// </summary>
		/// <param name="name">The name of the <see cref="LineStyle2D"/> object.</param>
		/// <param name="width">The width of the line.</param>
		/// <param name="color">A <see cref="Color"/> structure that indicates the color of this <see cref="LineStyle2D"/> object.</param>
		public LineStyle2D(string name, float width, Color color) 
			: this(name,width,color,DashStyle.Solid) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="LineStyle2D"/> class.
		/// </summary>
		/// <param name="name">The name of the <see cref="LineStyle2D"/> object.</param>
		public LineStyle2D(string name)
			: this(name,1.0f,Color.Black) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="LineStyle2D"/> class.
		/// </summary>
		public LineStyle2D() : this("") { }

		internal void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if(pen != null) 
				{
					pen.Dispose();
					pen = null;
				}
			}
		}

		
		#region --- Handling enum LineStyleKind ---

		private static string[] names = new string[]
			{
				"Default",
				"DefaultForLine2DSeriesStyle",
				"TwoDObjectBorder",
				"CoordinateLine",
				"AxisLine",
				"Solid",
				"Dash",
				"Dot",
				"DashDot",
				"DashDotDot",
				"Custom"
			};
		
		private static LineStyle2DKind[] kinds = new LineStyle2DKind[]
			{
				LineStyle2DKind.Default,
				LineStyle2DKind.DefaultForLine2DSeriesStyle,
				LineStyle2DKind.TwoDObjectBorder,
				LineStyle2DKind.CoordinateLine,
				LineStyle2DKind.AxisLine,
				LineStyle2DKind.Solid,
				LineStyle2DKind.Dash,
				LineStyle2DKind.Dot,
				LineStyle2DKind.DashDot,
				LineStyle2DKind.DashDotDot,
				LineStyle2DKind.Custom
			};

		internal static string NameOf(LineStyle2DKind kind)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(kind==kinds[i])
					return names[i];
			}
			throw new Exception("Implementation: arrays names/kinds in class 'LineStyle2D' mismatch");
		}

		internal new static LineStyle2DKind KindOf(string name)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(name==names[i])
					return kinds[i];
			}
			return LineStyle2DKind.Custom;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LineStyle2DKind Style2DKind
		{
			get
			{
				return LineStyle2D.KindOf(Name);
			}
			set
			{
				Name = LineStyle2D.NameOf(value);
			}
		}

		#endregion

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		internal bool		HasChanged  { get { return hasChanged; } set { hasChanged = value; } }

		/// <summary>
		/// Gets or sets the line width of this <see cref="LineStyle2D"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[SRDescription("LineStyle2DWidthDescr")]
		[DefaultValue(_WidthDefault)]
		public double		Width		{ get { return width; } set { if(width != value) hasChanged = true; width = value; } }
		/// <summary>
		/// Gets or sets the shade width of this <see cref="LineStyle2D"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[SRDescription("LineStyle2DShadeWidthDescr")]
		[DefaultValue(_ShadeWidthDefault)]
		public double		ShadeWidth	{ get { return shadeWidth; } set { if(shadeWidth != value) hasChanged = true; shadeWidth = value; } }
		/// <summary>
		/// Gets or sets the color of this <see cref="LineStyle2D"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[SRDescription("LineStyle2DColorDescr")]
		[DefaultValue(typeof(Color), _ColorDefault)]
		public Color		Color		{ get { return color; } set { if(color != value) hasChanged = true; color = value; } }
		/// <summary>
		/// Gets or sets the style used for dashed lines drawn with this <see cref="LineStyle2D"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[SRDescription("LineStyle2DDashStyleDescr")]
		[DefaultValue(_DashLineStyleDefault)]
		public DashStyle	DashStyle	{ get { return dashLineStyle; } set { if(dashLineStyle != value) hasChanged = true; dashLineStyle = value; } }
		
		/// <summary>
		/// Gets the <see cref="System.Drawing.Pen"/> object associated with this <see cref="LineStyle2D"/> object. 
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public Pen Pen
		{
			get
			{
				if(hasChanged || pen==null)
				{
					if(pen != null)
						pen.Dispose();
					pen = new Pen(color,(float)width);
					pen.DashStyle = dashLineStyle;
                    hasChanged = false;
				}
				return pen;
			}
		}

		/// <summary>
		/// Creates an exact copy of this <see cref="LineStyle2D"/> object.
		/// </summary>
		/// <returns>An <see cref="object"/> that can be cast to a <see cref="LineStyle2D"/> object.</returns>
		public object Clone()
		{
			LineStyle2D LS = new LineStyle2D((string)Name.Clone(),width,color,dashLineStyle);
			LS.ShadeWidth = ShadeWidth;
			LS.HasChanged = HasChanged;
			return LS;
		}

		#region --- Serialization ---
		internal bool ShouldSerializeMe				{ get { return hasChanged; } }

		private bool ShouldBrowsePen()				{ return false; }

		internal void Serialize(XmlCustomSerializer S)
		{
			S.AttributeProperty(this,"Name");
			S.AttributeProperty(this,"Width");
			S.AttributeProperty(this,"ShadeWidth");
			S.AttributeProperty(this,"Color");
			S.AttributeProperty(this,"DashStyle");
		}
		#endregion
	}
}