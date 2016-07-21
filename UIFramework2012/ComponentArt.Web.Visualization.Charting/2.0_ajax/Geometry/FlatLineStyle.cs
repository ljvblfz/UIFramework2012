using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Defines a flat line style.
	/// </summary>
	// ---------------------------------------------------------------------------------------------------

	public class FlatLineStyle : ThickLineStyle
	{
		private const double _WidthFlatLineStyleDefault = 10;
		private const double _WidthEdgeRadiusFlatLineStyleDefault = 0.25;
		private const double _HeightEdgeRadiusFlatLineStyleDefault = 0.25;


		/// <summary>
		/// Initializes a new instance of <see cref="FlatLineStyle"/> class with specified name, join style, color and width.
		/// </summary>
		/// <param name="name">The name of this <see cref="FlatLineStyle"/> object.</param>
		/// <param name="joinStyle">Join style of the line.</param>
		/// <param name="chartColor">Color of the line.</param>
		/// <param name="width">Width of the line.</param>
		public FlatLineStyle(string name, JoinStyle joinStyle, ChartColor chartColor, double width)
			: base (name,joinStyle,chartColor,width,width/4, _WidthEdgeRadiusFlatLineStyleDefault,_HeightEdgeRadiusFlatLineStyleDefault) 
		{ 
			EqualRadii = true;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FlatLineStyle"/> class with specified name, color and width.
		/// </summary>
		/// <param name="name">The name of this <see cref="FlatLineStyle"/> object.</param>
		/// <param name="chartColor">Color of the line.</param>
		/// <param name="width">Width of the line.</param>
		public FlatLineStyle(string name,ChartColor chartColor, double width)
			: this(name,JoinStyle.Bevel, chartColor, width)
		{ }

		/// <summary>
		/// Initializes a new instance of <see cref="FlatLineStyle"/> class with specified name, color and width.
		/// </summary>
		/// <param name="name">The name of this <see cref="FlatLineStyle"/> object.</param>
		/// <param name="color">Color of the line.</param>
		/// <param name="width">Width of the line.</param>
		public FlatLineStyle(string name,Color color, double width)
			: this(name,(ChartColor)color,width)
		{ }

		/// <summary>
		/// Initializes a new instance of <see cref="FlatLineStyle"/> class with a specified name.
		/// </summary>
		/// <param name="name">The name of this <see cref="FlatLineStyle"/> object.</param>
		public FlatLineStyle(string name) : this(name, Color.Black,_WidthFlatLineStyleDefault) { }

		/// <summary>
		/// Initializes a new instance of <see cref="FlatLineStyle"/> class with default parameters.
		/// </summary>
		public FlatLineStyle() : this(null) {}
		
		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="ThickLineStyle.Width"/>.
		/// </summary>
		[DefaultValue(_WidthFlatLineStyleDefault)]
		public override double Width 
		{
			get {return base.Width;}
			set {base.Width = value;}
		}

		/// <summary>
		/// Overrides <see cref="ThickLineStyle.Height"/> to hide and disable it.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override double Height 
		{
			get {return base.Height;}
			set {/*base.Height = value;*/}
		}

		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="ThickLineStyle.WidthEdgeRadius"/>.
		/// </summary>
		[DefaultValue(_WidthEdgeRadiusFlatLineStyleDefault)]
		public override double WidthEdgeRadius 
		{
			get {return base.WidthEdgeRadius;}
			set {base.WidthEdgeRadius = value;}
		}

		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="ThickLineStyle.HeightEdgeRadius"/>.
		/// </summary>
		[DefaultValue(_HeightEdgeRadiusFlatLineStyleDefault)]
		public override double HeightEdgeRadius 
		{
			get {return base.HeightEdgeRadius;}
			set {base.HeightEdgeRadius = value;}
		}
	}
}
