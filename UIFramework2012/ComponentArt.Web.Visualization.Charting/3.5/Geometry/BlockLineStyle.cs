using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Defines a block line style.
	/// </summary>
	// ---------------------------------------------------------------------------------------------------
		
	public class BlockLineStyle : ThickLineStyle
	{

		private const double _WidthBlockLineStyleDefault = 10;
		private const double _HeightBlockLineStyleDefault = 10;

		private const double _WidthEdgeRadiusBlockLineStyleDefault = 0.2;
		private const double _HeightEdgeRadiusBlockLineStyleDefault = 0.2;


		/// <summary>
		/// Initializes a new instance of the <see cref="BlockLineStyle"/> class with specified name, color and width.
		/// </summary>
		/// <param name="name">The name of this <see cref="BlockLineStyle"/> object.</param>
		/// <param name="chartColor">Color of the line.</param>
		/// <param name="width">Width of the line.</param>
		/// <param name="height">Height of the line.</param>
		public BlockLineStyle(string name,ChartColor chartColor, double width, double height)
			: base (name,JoinStyle.Bevel, chartColor, width, height, _WidthEdgeRadiusBlockLineStyleDefault,_HeightEdgeRadiusBlockLineStyleDefault)
		{ }
		/// <summary>
		/// Initializes a new instance of the <see cref="BlockLineStyle"/> class with specified name, color and width.
		/// </summary>
		/// <param name="name">The name of this <see cref="BlockLineStyle"/> object.</param>
		/// <param name="chartColor">Color of the line.</param>
		/// <param name="width">Width of the line.</param>
		public BlockLineStyle(string name,ChartColor chartColor, double width)
			: this (name, chartColor, width, width)
		{ }
	
		/// <summary>
		/// Initializes a new instance of the <see cref="BlockLineStyle"/> class with a specified name.
		/// </summary>
		/// <param name="name">The name of this <see cref="BlockLineStyle"/> object.</param>
		public BlockLineStyle(string name) :this(name,Color.Black,_WidthBlockLineStyleDefault,_HeightBlockLineStyleDefault) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockLineStyle"/> class.
		/// </summary>
		public BlockLineStyle() : this(null) {}



		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="ThickLineStyle.Width"/>.
		/// </summary>
		[DefaultValue(_WidthBlockLineStyleDefault)]
		public override double Width 
		{
			get {return base.Width;}
			set {base.Width = value;}
		}

		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="ThickLineStyle.Height"/>.
		/// </summary>
		[DefaultValue(_HeightBlockLineStyleDefault)]
		public override double Height 
		{
			get {return base.Height;}
			set {base.Height = value;}
		}


		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="ThickLineStyle.WidthEdgeRadius"/>.
		/// </summary>
		[DefaultValue(_WidthEdgeRadiusBlockLineStyleDefault)]
		public override double WidthEdgeRadius 
		{
			get {return base.WidthEdgeRadius;}
			set {base.WidthEdgeRadius = value;}
		}

		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="ThickLineStyle.HeightEdgeRadius"/>.
		/// </summary>
		[DefaultValue(_HeightEdgeRadiusBlockLineStyleDefault)]
		public override double HeightEdgeRadius 
		{
			get {return base.HeightEdgeRadius;}
			set {base.HeightEdgeRadius = value;}
		}

		#region --- XML Serialization ---

		private new bool ShouldSerializeRadius1() { return false; }
		private new bool ShouldSerializeRadius2() { return false; }
		private new bool ShouldSerializeRadius3() { return false; }
		private new bool ShouldSerializeRadius4() { return false; }

		internal override void Serialize(XmlCustomSerializer S)
		{
			base.Serialize(S);
		}
		#endregion
	}

}
