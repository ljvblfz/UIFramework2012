using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Defines a strip line style.
	/// </summary>
	// ---------------------------------------------------------------------------------------------------

	//[TypeConverter(typeof(StripLineStyleConverter))]
	public class StripLineStyle : ThickLineStyle
	{
		private const double _WidthStripLineStyleDefault = 2.0;
		private const double _WidthEdgeRadiusStripLineStyleDefault = 0.25;
		private const double _HeightEdgeRadiusStripLineStyleDefault = 0.5;

		private double height = 20;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="StripLineStyle"/> class with a specified name, join style, .
		/// </summary>
		/// <param name="name">The name of this <see cref="StripLineStyle"/> object.</param>
		/// <param name="joinStyle">Join style of the line.</param>
		/// <param name="chartColor">Color of the line.</param>
		/// <param name="width">Width of the line.</param>
		public StripLineStyle(string name, JoinStyle joinStyle, ChartColor chartColor, double width)
			//string name, JoinStyle joinStyle, ChartColor surface, double width,
			//double h, double rw, double rh)
			: base (name,joinStyle,chartColor,width,0,_WidthEdgeRadiusStripLineStyleDefault,_HeightEdgeRadiusStripLineStyleDefault) { EqualRadii = true; }


		/// <summary>
		/// Initializes a new instance of the <see cref="StripLineStyle"/> class with a specified name.
		/// </summary>
		/// <param name="name">The name of this <see cref="StripLineStyle"/> object.</param>
		public StripLineStyle(string name) :this(name,JoinStyle.Bevel,Color.Black, _WidthStripLineStyleDefault)
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="StripLineStyle"/> class.
		/// </summary>
		public StripLineStyle() : this(null) {}


		/// <summary>
		/// Gets or sets the width of this <see cref="StripLineStyle"/> object.
		/// </summary>
		[DefaultValue(_WidthStripLineStyleDefault)]
		public override double Width 
		{
			get {return base.Width;}
			set {base.Width = value;}
		}

		/// <summary>
		/// Gets or sets the height of this <see cref="StripLineStyle"/> object.
		/// </summary>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override double Height 
		{
			get {return height;}
			set {height = value;}
		}


		/// <summary>
		/// Gets or sets the horizontal radius at the edges of the line.
		/// </summary>
		[DefaultValue(_WidthEdgeRadiusStripLineStyleDefault)]
		public override double WidthEdgeRadius 
		{
			get {return base.WidthEdgeRadius;}
			set {base.WidthEdgeRadius = value;}
		}

		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="LineStyle.Height"/>.
		/// </summary>
		[DefaultValue(_HeightEdgeRadiusStripLineStyleDefault)]
		public override double HeightEdgeRadius 
		{
			get {return base.HeightEdgeRadius;}
			set {base.HeightEdgeRadius = value;}
		}


		#region --- Serialization ---

		internal override void ResetJoinStyle() {JoinStyle=JoinStyle.Bevel;}
		private bool ShouldSerializeJoinStyle() {return JoinStyle!=JoinStyle.Bevel;}

		internal override void ResetWidth() {Width=2.0;}
		private bool ShouldSerializeWidth() {return Width!=2.0;}

		#endregion
	}

}
