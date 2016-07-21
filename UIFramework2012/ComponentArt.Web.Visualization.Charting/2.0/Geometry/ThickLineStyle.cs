using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace ComponentArt.Web.Visualization.Charting
{
	// ---------------------------------------------------------------------------------------------------

	//[System.ComponentModel.TypeConverter(typeof(Design.ThickLineStyleConverter))]
	//[Serializable]
	/// <summary>
	/// Defines a thick line style.
	/// </summary>
	public class ThickLineStyle : LineStyle
	{
		private	double rw,rh;
		private bool equalRadii = false;

		private const JoinStyle _JoinStyleThickLineStyleDefault = JoinStyle.Bevel;
		private const double _WidthThickLineStyleDefault = 2;
		private const double _HeightThickLineStyleDefault = 2;

		private const double _WidthEdgeRadiusDefault = 0.5;
		private const double _HeightEdgeRadiusDefault = 0.5;


		internal ThickLineStyle(string name, JoinStyle joinStyle, ChartColor surface, double width,
			double h, double rw, double rh)
			: base (name,joinStyle,surface,width) 
		{
			Height = h;
			this.rw = rw;
			this.rh = rh;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ThickLineStyle"/> class with a specified name.
		/// </summary>
		/// <param name="name">The name of this <see cref="ThickLineStyle"/> object.</param>
		public ThickLineStyle(string name) :this(name, _JoinStyleThickLineStyleDefault,Color.Black,_WidthThickLineStyleDefault,_HeightThickLineStyleDefault,_WidthEdgeRadiusDefault,_HeightEdgeRadiusDefault) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ThickLineStyle"/> class.
		/// </summary>
		public ThickLineStyle() : this(null) {}

		/// <summary>
		/// Gets or sets the horizontal radius at the edges of the line.
		/// </summary>
		[SRDescription("ThickLineStyleWidthEdgeRadiusDescr")]
		[DefaultValue(_WidthEdgeRadiusDefault)]
		public virtual double WidthEdgeRadius
		{
			get { return rw; }
			set 
			{
				if(rw != value)
					hasChanged = true;
				rw = value;
			}
		}

		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="LineStyle.Width"/>.
		/// </summary>
		[DefaultValue(_WidthThickLineStyleDefault)]
		public override double Width 
		{
			get {return base.Width;}
			set {base.Width = value;}
		}

		/// <summary>
		/// Overrides DefaultValue attribute of the <see cref="LineStyle.Height"/>.
		/// </summary>
		[DefaultValue(_HeightThickLineStyleDefault)]
		public override double Height 
		{
			get {return base.Height;}
			set {base.Height = value;}
		}

		/// <summary>
		/// Gets or sets the vertical radius at the edges of the line.
		/// </summary>
		[SRDescription("ThickLineStyleHeightEdgeRadiusDescr")]
		[DefaultValue(_HeightEdgeRadiusDefault)]
		public virtual double HeightEdgeRadius
		{
			get { return rh; }
			set 
			{
				if(rh != value)
					hasChanged = true;
				rh = value;
			}
		}


		/// <summary>
		/// Overrides DefaultValue attribute of <see cref="LineStyle.JoinStyle"/>.
		/// </summary>
		[DefaultValue(_JoinStyleThickLineStyleDefault)]
		public override JoinStyle JoinStyle 
		{
			get {return base.JoinStyle;}
			set {base.JoinStyle = value;}
		}
		
	
		internal bool EqualRadii 
		{
			get 
			{
				return equalRadii;
			}
			set 
			{
				equalRadii = value;
			}
		}

		#region --- Serialization ---

		internal override void Serialize(XmlCustomSerializer S)
		{
			base.Serialize(S);
			S.AttributeProperty(this,"RH","HeightEdgeRadius");
			S.AttributeProperty(this,"RW","WidthEdgeRadius");
		}

		#endregion
	}

}
