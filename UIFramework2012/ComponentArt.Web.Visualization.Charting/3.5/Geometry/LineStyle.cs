using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using ComponentArt.Web.Visualization.Charting.Design;
using ComponentArt.Web.Visualization.Charting.Geometry;


namespace ComponentArt.Web.Visualization.Charting
{

	/// <summary>
	/// Line style kind. Used to access predefined line styles. 
	/// All custom created styles have this kind set to 'Custom'.
	/// </summary>
	public enum LineStyleKind
	{
		Default,
		StripLine,
		BlockLine,
		PipeLine,
		FlatLine,
		DotLine,
		DashLine,
		DashDotLine,
		MultiLine,
		Custom

	};

	/// <summary>
	/// Specifies how to join consecutive line or curve segments in the chart.
	/// </summary>
	public enum JoinStyle
	{
		/// <summary>
		/// Specifies a mitered join. This produces a sharp corner or a clipped corner, depending on whether the length of the miter exceeds the miter limit.
		/// </summary>
		Miter,
		/// <summary>
		/// Specifies a circular join. This produces a smooth, circular arc between the lines.
		/// </summary>
		Round,
		/// <summary>
		/// Specifies a beveled join. This produces a diagonal corner.
		/// </summary>
		Bevel
	};

	/// <summary>
	/// Classes that derive from this abstract class define an object used to draw 3D lines and curves in the chart.
	/// </summary>
	public abstract class LineStyle : NamedObjectBase
	{
		private JoinStyle	joinStyle;
		private double		width,h;
		private double		liftZ;
		internal bool		hasChanged = true;
		private ChartColor	surface;

		#region --- Constructors ---

		/// <summary>
		/// Initializes a new instance of the <see cref="LineStyle"/> class with a specified name.
		/// </summary>
		/// <param name="name">The name of the new <see cref="LineStyle"/> object.</param>
		protected LineStyle(string name) :base(name)
		{
			this.joinStyle = JoinStyle.Miter;
			this.width = 0.0;
			liftZ = 0.5;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LineStyle"/> class.
		/// </summary>
		protected LineStyle() : this(null) {}

		/// <summary>
		/// Initializes a new instance of the <see cref="LineStyle"/> class.
		/// </summary>
		/// <param name="name">The name of the new <see cref="LineStyle"/> object. This value is stored in the <see cref="NamedObjectBase.Name"/> property.</param>
		/// <param name="joinStyle">The join style for the ends of two consecutive lines drawn with this <see cref="LineStyle"/> object. This value is stored in the <see cref="LineStyle.JoinStyle"/> property.</param>
		/// <param name="chartColor">The color of the line.</param>
		/// <param name="width">The width of the line. This value is stored in the <see cref="LineStyle.Width"/> property.</param>
		protected LineStyle(string name, JoinStyle joinStyle, ChartColor chartColor, double width)
			:base(name)
		{
			this.joinStyle = joinStyle;
			this.surface = chartColor;
			this.width = width;
			liftZ = 0.5;
		}

		internal LineStyle Clone(string styleName)
		{
			StyleCloner cloner = new StyleCloner();
			LineStyle copy = (LineStyle)cloner.Clone(this);
			return copy;
		}

		#endregion

		#region --- Handling enum LineStyleKind ---

		private static string[] names = new string[]
			{
				"Default",
				"StripLine",
				"BlockLine",
				"PipeLine",
				"FlatLine",
				"DotLine",
				"DashLine",
				"DashDotLine",
				"MultiLine",
				"Custom"
			};
		
		private static LineStyleKind[] kinds = new LineStyleKind[]
			{
				LineStyleKind.Default,
				LineStyleKind.StripLine,
				LineStyleKind.BlockLine,
				LineStyleKind.PipeLine,
				LineStyleKind.FlatLine,
				LineStyleKind.DotLine,
				LineStyleKind.DashLine,
				LineStyleKind.DashDotLine,
				LineStyleKind.MultiLine,
				LineStyleKind.Custom
			};

		internal static string NameOf(LineStyleKind kind)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(kind==kinds[i])
					return names[i];
			}
			throw new Exception("Implementation: arrays names/kinds in class 'LineStyle' mismatch");
		}

		internal static LineStyleKind KindOf(string name)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(name==names[i])
					return kinds[i];
			}
			return LineStyleKind.Custom;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LineStyleKind LineStyleKind
		{
			get
			{
				return LineStyle.KindOf(Name);
			}
			set
			{
				Name = LineStyle.NameOf(value);
			}
		}

		#endregion

		#region --- Rendering ---

		#endregion

		#region --- Properties ---

		/// <summary>
		/// Gets or sets the join style of this <see cref="LineStyle"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[SRDescription("LineStyleJoinStyleDescr")]
		[DefaultValue(JoinStyle.Miter)]
		public virtual JoinStyle JoinStyle
		{
			get { return joinStyle; }
			set
			{
				if(joinStyle != value)
					hasChanged = true;
				joinStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the width of this <see cref="LineStyle"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[Description("Width of the line")]
		[DefaultValue(0)]
		public virtual double Width
		{
			get { return width; }
			set
			{
				if(width != value)
					hasChanged = true;
				width = value;
			}
		}

		/// <summary>
		/// Gets or sets the height of this <see cref="LineStyle"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[Description("Height of the line")]
		[DefaultValue(0)]
		public virtual double Height
		{
			get { return h; }
			set
			{
				if(h != value)
					hasChanged = true;
				h = value;
			}
		}


		/// <summary>
		/// Gets or sets the lift of this <see cref="LineStyle"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[SRDescription("LineStyleLiftDescr")]
		[DefaultValue(0.5)]
		public double Lift
		{
			get { return liftZ; }
			set
			{
				if(liftZ != value)
					hasChanged = true;
				liftZ = value;
			}
		}

		/// <summary>
		/// Gets or sets a <see cref="ChartColor"/> of this <see cref="LineStyle"/> object.
		/// </summary>
		internal ChartColor ChartColor			{ get { return surface; }			set { surface = value; } }

		#endregion

		#region --- Serialization ---

		internal bool HasChanged { get { return hasChanged; } set { hasChanged = value; } }
		internal bool ShouldSerializeMe { get { return hasChanged; } }

		internal virtual void ResetJoinStyle() {joinStyle=JoinStyle.Miter;}
		private bool ShouldSerializeJoinStyle() {return joinStyle!=JoinStyle.Bevel;}

		internal virtual void ResetWidth() {width=0.0f;}
		private bool ShouldSerializeWidth() {return width!=0.0f;}

		internal virtual void ResetHeight() {h=0.0f;}
		private bool ShouldSerializeHeight() {return h!=0.0f;}

		internal virtual void ResetLift() {liftZ=0.5;}
		private bool ShouldSerializeLift() {return liftZ!=0.5;}

		private bool ShouldSerializePaletteName() { return false; }

		internal virtual void Serialize(XmlCustomSerializer S)
		{
			S.AttributeProperty(this,"Name");
			if(!S.Reading)
			{
				S.AttributeProperty(this,"LineType");
			}
			if(S.Reading || ShouldSerializeJoinStyle())
				S.AttributeProperty(this ,"JoinStyle");
			if(S.Reading || ShouldSerializeWidth())
				S.AttributeProperty(this as LineStyle,"Width");
			if(S.Reading || ShouldSerializeHeight())
				S.AttributeProperty(this as LineStyle,"Height");
			if(S.Reading || ShouldSerializeLift())
				S.AttributeProperty(this as LineStyle,"Lift");

			if(S.Reading)
			{
				if(S.BeginTag("ChartColor"))
				{
					ChartColor = new ChartColor();
					ChartColor.Serialize(S);
					S.EndTag();
				}
			}
			else
			{
				if(ChartColor != null && S.BeginTag("ChartColor"))
				{
					ChartColor.Serialize(S);
					S.EndTag();
				}
			}
		}
		#endregion

	}
}
