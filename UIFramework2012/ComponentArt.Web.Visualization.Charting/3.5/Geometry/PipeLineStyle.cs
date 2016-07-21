using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Defines a pipe line style.
	/// </summary>

	// ---------------------------------------------------------------------------------------------------
		
	//[TypeConverter(typeof(PipeLineStyleConverter))]
	public class PipeLineStyle : ThickLineStyle
	{
		private const double _WidthPipeLineStyleDefault = 10;
		private const double _WidthEdgeRadiusPipeLineStyleDefault = 0.5;
		private const double _HeightEdgeRadiusPipeLineStyleDefault = 0.5;
		private const JoinStyle _JoinStylePipeLineStyleDefault = JoinStyle.Round;
	
		/// <summary>
		/// Initializes a new instance of the <see cref="PipeLineStyle"/> class with specified name, color and width.
		/// </summary>
		/// <param name="name">The name of this <see cref="PipeLineStyle"/> object.</param>
		/// <param name="chartColor">Color of the line.</param>
		/// <param name="width">Width of the line.</param>
		public PipeLineStyle(string name,ChartColor chartColor, double width)
			: base (name,_JoinStylePipeLineStyleDefault, chartColor, width, width, _WidthEdgeRadiusPipeLineStyleDefault,_HeightEdgeRadiusPipeLineStyleDefault)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="PipeLineStyle"/> class with a specified name.
		/// </summary>
		/// <param name="name">The name of this <see cref="PipeLineStyle"/> object.</param>
		public PipeLineStyle(string name) :this(name,Color.Black,_WidthPipeLineStyleDefault) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="PipeLineStyle"/> class.
		/// </summary>
		public PipeLineStyle() : this(null) {}

		/// <summary>
		/// Gets or sets the width and hight of the pipe.
		/// </summary>
		[DefaultValue(_WidthPipeLineStyleDefault)]
		public override double Width 
		{
			get {return base.Width;}
			set 
			{
				if(base.Width != value || base.Height != value)
				{
					hasChanged = true;
					base.Width = value; base.Height = value;
				}
			}
		}

		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="ThickLineStyle.WidthEdgeRadius"/>.
		/// </summary>
		[DefaultValue(_WidthEdgeRadiusPipeLineStyleDefault)]
		public override double WidthEdgeRadius 
		{
			get {return base.WidthEdgeRadius;}
			set {base.WidthEdgeRadius = value;}
		}

		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="ThickLineStyle.HeightEdgeRadius"/>.
		/// </summary>
		[DefaultValue(_HeightEdgeRadiusPipeLineStyleDefault)]
		public override double HeightEdgeRadius 
		{
			get {return base.HeightEdgeRadius;}
			set {base.HeightEdgeRadius = value;}
		}


		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="ThickLineStyle.JoinStyle"/>.
		/// </summary>
		[DefaultValue(_JoinStylePipeLineStyleDefault)]
		public override JoinStyle JoinStyle 
		{
			get {return base.JoinStyle;}
			set {base.JoinStyle = value;}
		}
		
		/// <summary>
		/// Overrides <see cref="ThickLineStyle.Height"/> to hide and disable it.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override double Height 
		{
			get { return Width; }
			set { /*Width = value; */}
		}
	}
}
