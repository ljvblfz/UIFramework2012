using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Marker style kind. Used to access predefined marker styles. 
	/// All custom created styles have this kind set to 'Custom'.
	/// </summary>
	public enum GradientStyleKind
	{
		None,
		Horizontal,
		Vertical,
		DiagonalRight,
		DiagonalLeft,
		Center,
		HorizontalCenter,
		VerticalCenter,
		DiagonalRightCenter,
		DiagonalLeftCenter,
		Custom
	};
	/// <summary>
	/// Describes the kind of the gradient used in chart's background.
	/// </summary>
	public enum GradientKind
	{
		None,
		Horizontal,
		Vertical,
		DiagonalRight,
		DiagonalLeft,
		Center,
		HorizontalCenter,
		VerticalCenter,
		DiagonalRightCenter,
		DiagonalLeftCenter

	};
	/// <summary>
	/// Represents the gradient style used in Rectangle and Area2D styles.
	/// </summary>
	public class GradientStyle : NamedObjectBase,ICloneable
	{
		private GradientKind	kind = GradientKind.None;
		private Color		color0;
		private Color		color1;
		private bool		hasChanged = true;

		#region --- Construction and Clonning ---

		/// <summary>
		/// Initializes a new instance of <see cref="GradientStyle"/> class with specified name, kind, and starting and ending colors.
		/// </summary>
		/// <param name="name">Name of the gradient style.</param>
		/// <param name="kind">Kind of the gradient style.</param>
		/// <param name="startColor">Starting color of the gradient style.</param>
		/// <param name="endColor">Ending color of the gradient style.</param>
		public GradientStyle(string name, GradientKind kind, Color startColor, Color endColor) : base(name)
		{
			this.kind = kind;
			this.color0 = startColor;
			this.color1 = endColor;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="GradientStyle"/> class with a specified name.
		/// </summary>
		/// <param name="name">Name of the gradient style.</param>
		public GradientStyle(string name) :this(name,GradientKind.None,Color.Transparent,Color.Transparent) {}

		/// <summary>
		/// Initializes a new instance of <see cref="GradientStyle"/> class with default parameters.
		/// </summary>
		public GradientStyle() :this("") {}

		/// <summary>
		/// Creates and returns an exact copy of this <see cref="GradientStyle"/> object.
		/// </summary>
		/// <returns>An exact copy of this <see cref="GradientStyle"/> object.</returns>
		public object Clone()
		{
			GradientStyle newGS = new GradientStyle(Name,GradientKind,StartColor,EndColor);
			newGS.HasChanged = HasChanged;
			return newGS;
		}
		#endregion

		#region --- Properties ---

		/// <summary>
		/// Gets or sets the gradient kind of this <see cref="GradientStyle"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[SRDescription("GradientStyleGradientKindDescr")]
		public GradientKind	GradientKind{ get { return kind; } set { if(kind != value) { hasChanged = true; kind = value; } } }
		/// <summary>
		/// Gets or sets the starting color of this <see cref="GradientStyle"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[SRDescription("GradientStyleStartColorDescr")]
		[DefaultValue(typeof(Color), "Transparent")]
		public Color		StartColor	{ get { return color0; } set { if(color0 != value) { color0 = value; hasChanged = true; } } }
		/// <summary>
		/// Gets or sets the ending color of this <see cref="GradientStyle"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[SRDescription("GradientStyleEndColorDescr")]
		[DefaultValue(typeof(Color), "Transparent")]
		public Color		EndColor	{ get { return color1; } set { if(color1 != value) { color1 = value; hasChanged = true; } } }

		internal bool HasChanged { get { return hasChanged; } set { hasChanged = value; } }
		
		#endregion
		
		#region --- Handling enum GradientStyleKind ---

		private static string[] names = new string[]
			{
				"None",
				"Horizontal",
				"Vertical",
				"DiagonalRight",
				"DiagonalLeft",
				"Center",
				"HorizontalCenter",
				"VerticalCenter",
				"DiagonalRightCenter",
				"DiagonalLeftCenter",
				"Custom"
			};
		
		private static GradientStyleKind[] kinds = new GradientStyleKind[]
			{
				GradientStyleKind.None,
				GradientStyleKind.Horizontal,
				GradientStyleKind.Vertical,
				GradientStyleKind.DiagonalRight,
				GradientStyleKind.DiagonalLeft,
				GradientStyleKind.Center,
				GradientStyleKind.HorizontalCenter,
				GradientStyleKind.VerticalCenter,
				GradientStyleKind.DiagonalRightCenter,
				GradientStyleKind.DiagonalLeftCenter,
				GradientStyleKind.Custom
			};

		internal static string NameOf(GradientStyleKind kind)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(kind==kinds[i])
					return names[i];
			}
			throw new Exception("Implementation: arrays names/kinds in class 'MarkerStyle' mismatch");
		}

		internal new static GradientStyleKind KindOf(string name)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(name==names[i])
					return kinds[i];
			}
			return GradientStyleKind.Custom;
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


		internal Brush CreateBrush(RectangleF rect)
		{
			float xMin = rect.X;
			float yMin = rect.Y;
			float xMax = rect.X + rect.Width;
			float yMax = rect.Y + rect.Height;
			switch(GradientKind)
			{
				case GradientKind.DiagonalLeft:
					return CreateLinearGradientBrush(new PointF(xMin,yMin),new PointF(xMax,yMax),StartColor,EndColor);
				case GradientKind.DiagonalRight:
					return CreateLinearGradientBrush(new PointF(xMin,yMax),new PointF(xMax,yMin),StartColor,EndColor);
				case GradientKind.Horizontal:
					return CreateLinearGradientBrush(new PointF(xMin,yMin),new PointF(xMax,yMin),StartColor,EndColor);
				case GradientKind.Vertical:
					return CreateLinearGradientBrush(new PointF(xMin,yMax),new PointF(xMin,yMin),StartColor,EndColor);
				case GradientKind.DiagonalLeftCenter:
					return CreateCenteredLinearGradientBrush(new PointF(xMin,yMin),new PointF(xMax,yMax),StartColor,EndColor);
				case GradientKind.DiagonalRightCenter:
					return CreateCenteredLinearGradientBrush(new PointF(xMax,yMin),new PointF(xMin,yMax),StartColor,EndColor);
				case GradientKind.HorizontalCenter:
					return CreateCenteredLinearGradientBrush(new PointF(xMin,yMin),new PointF(xMax,yMin),StartColor,EndColor);
				case GradientKind.VerticalCenter:
					return CreateCenteredLinearGradientBrush(new PointF(xMin,yMin),new PointF(xMin,yMax),StartColor,EndColor);
				case GradientKind.Center:
					return CreateCircularGradientBrush(new PointF((xMin+xMax)/2,(yMin+yMax)/2),
						Math.Sqrt(Math.Pow(xMin-xMax,2) + Math.Pow(yMin-yMax,2))/2.0,StartColor,EndColor);
			}
			return new SolidBrush(StartColor);
		}

		internal Brush CreateLinearGradientBrush(PointF startPoint, PointF endPoint, Color startColor, Color endColor)
		{
			// The standard GDI+ gradient brush between two points doesn't work well before the first and
			// after the last point. This function uses blending mechanism to extend the brush to cover
			// area before the first point (using starting color) and after the last point (using ending color).
			//
			// The method used here triples the area that is properly rendered, adding a band before starting
			// point and another band after the last point.

			// New ending points
			PointF startPoint1 = new PointF(2*startPoint.X - endPoint.X,2*startPoint.Y - endPoint.Y);
			PointF endPoint1 = new PointF(2*endPoint.X - startPoint.X,2*endPoint.Y - startPoint.Y);

			float[] pos = new float[4] { 0.0f, 0.33333f, 0.66666f, 1.0f };
			float[] fac = new float[4] { 0.0f, 0.00001f, 0.99999f, 1.0f };

			LinearGradientBrush brush = new LinearGradientBrush(startPoint1,endPoint1,startColor,endColor);
			Blend blend = new Blend(4);
			blend.Factors = fac;
			blend.Positions = pos;
			brush.Blend = blend;

			return brush;
		}

		internal Brush CreateCenteredLinearGradientBrush(PointF startPoint, PointF endPoint, Color startColor, Color endColor)
		{
			// The standard GDI+ gradient brush between two points doesn't work well before the first and
			// after the last point. This function uses blending mechanism to extend the brush to cover
			// area before the first point (using starting color) and after the last point (using ending color).
			//
			// The method used here triples the area that is properly rendered, adding a band before starting
			// point and another band after the last point.

			// New ending points
			PointF startPoint1 = new PointF(2*startPoint.X - endPoint.X,2*startPoint.Y - endPoint.Y);
			PointF endPoint1 = new PointF(2*endPoint.X - startPoint.X,2*endPoint.Y - startPoint.Y);

			float[] pos = new float[5] { 0.0f, 0.33333f, 0.5f, 0.66666f, 1.0f };
			float[] fac = new float[5] { 1.0f, 0.99999f, 0.0f, 0.99999f, 1.0f };

			LinearGradientBrush brush = new LinearGradientBrush(startPoint1,endPoint1,startColor,endColor);
			Blend blend = new Blend(5);
			blend.Factors = fac;
			blend.Positions = pos;
			brush.Blend = blend;

			return brush;
		}

		internal static Brush CreateCircularGradientBrush(PointF centerPoint, double radius, Color centerColor, Color endColor)
		{
			return CreateEllipticGradientBrush(centerPoint, new PointF((float)(centerPoint.X+radius),centerPoint.Y),
				radius, centerColor, endColor);
		}

		internal static Brush CreateEllipticGradientBrush(PointF centerPoint, PointF endPoint, double axis2, Color centerColor, Color endColor)
		{
			// (dx,dy) and (dxn,dyn) are orthogonal axes of the ellipse
			
			double dx = (endPoint.X-centerPoint.X)*1.01;
			double dy = (endPoint.Y-centerPoint.Y)*1.01;
			double dd = Math.Sqrt(dx*dx + dy*dy);
			double dxn = -dy/dd*axis2*1.01;
			double dyn =  dx/dd*axis2*1.01;

			// Creating the path (40-point approximation)

			PointF[] pts = new PointF[40];
			double da = Math.PI/pts.Length*2;
			for(int i=0; i<pts.Length;i++)
			{
				double s = Math.Sin(da*i);
				double c = Math.Cos(da*i);
				pts[i].X = (float)(centerPoint.X + c*dx + s*dxn);
				pts[i].Y = (float)(centerPoint.Y + c*dy + s*dyn);
			}

			// Creating the brush

			PathGradientBrush brush = new PathGradientBrush(pts);
			brush.CenterPoint = centerPoint;
			brush.CenterColor = centerColor;
			// Setting ending color
			Color[] colors = new Color[40];
			for(int j=0;j<40;j++)
				colors[j] = endColor;
			brush.SurroundColors = new Color[] { endColor };

			return brush;
		}


		#region --- XML Serialization ---

		internal void Serialize(XmlCustomSerializer S)
		{
			S.AttributeProperty(this,"Name");
			S.AttributeProperty(this,"GradientKind");
			S.AttributeProperty(this,"StartColor");
			S.AttributeProperty(this,"EndColor");
		}
		#endregion
	}

}
