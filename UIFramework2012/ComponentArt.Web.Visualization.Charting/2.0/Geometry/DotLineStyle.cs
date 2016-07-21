using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Defines a dotted line style. 
	/// </summary>
	// ---------------------------------------------------------------------------------------------------
		
	public class DotLineStyle : LineStyle
	{
		private const JoinStyle _JoinStyleDotLineStyleDefault = JoinStyle.Bevel;
		private const double _RelativeDistanceDefault = 1.5;
		private const double _WidthDotLineStyleDefault = 10;
		private	double relativeDistance;

		/// <summary>
		/// Initializes a new instance of the <see cref="DotLineStyle"/> class with specified name, color and width.
		/// </summary>
		/// <param name="name">The name of the new <see cref="DotLineStyle"/> object. This value is stored in the <see cref="NamedObjectBase.Name"/> property.</param>
		/// <param name="chartColor">The color of the line.</param>
		/// <param name="width">The width of the line.</param>
		public DotLineStyle(string name, ChartColor chartColor, double width)
			: this(name,chartColor, _RelativeDistanceDefault, width)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="DotLineStyle"/> class with specified name, color, relative distance and width.
		/// </summary>
		/// <param name="name">Name of the new <see cref="DotLineStyle"/> object.</param>
		/// <param name="chartColor">Color of the line.</param>
		/// <param name="relativeDistance">Distance between dots relative to the line width.</param>
		/// <param name="width">Width of the line.</param>
		public DotLineStyle(string name, ChartColor chartColor, double relativeDistance, double width)
			: this(name,chartColor, relativeDistance, width, width)
		{ }

		internal DotLineStyle(string name,ChartColor surface, double relativeDistance, 
			double width, double h)
			: base (name,_JoinStyleDotLineStyleDefault,surface,width) 
		{
			this.relativeDistance = relativeDistance;
			Height = h;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DotLineStyle"/> class with specified name.
		/// </summary>
		/// <param name="name">Name of the new <see cref="DotLineStyle"/> object.</param>
		public DotLineStyle(string name) :this(name,Color.Black,_WidthDotLineStyleDefault) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="DotLineStyle"/> class with default parameters.
		/// </summary>
		public DotLineStyle() : this(null) {}
		
		/// <summary>
		/// Gets or sets the distance between dots relative to the line width.
		/// </summary>
		[SRDescription("DotLineStyleRelativeDistanceDescr")]
		[DefaultValue(_RelativeDistanceDefault)]
		public double RelativeDistance
		{
			get { return relativeDistance; }
			set 
			{
				if(relativeDistance != value)
					hasChanged = true;
				relativeDistance = value;
			}
		}

		/// <summary>
		/// Gets or sets the width and hight of the dot.
		/// </summary>
		[DefaultValue(_WidthDotLineStyleDefault)]
		public override double Width 
		{
			get {return base.Width;}
			set {base.Width = value;}
		}

		/// <summary>
		/// Not used.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override double Height 
		{
			get {return base.Height;}
			set {}
		}


		/// <summary>
		/// Gets or sets the join style of this <see cref="DotLineStyle"/> object.
		/// </summary>
		[DefaultValue(_JoinStyleDotLineStyleDefault)]
		public override JoinStyle JoinStyle 
		{
			get {return base.JoinStyle;}
			set {base.JoinStyle = value;}
		}
	}

}
