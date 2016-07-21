using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Xml;

using ComponentArt.Web.Visualization.Charting.Design;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting
{
	// ------------------------------------------------------------------------------------------------------
	//		Text Orientation/Side Styles
	// ------------------------------------------------------------------------------------------------------

	/// <summary>
	/// Specifies how text should look from the back.
	/// </summary>
	public enum ReverseSideStyle
	{
		ShowReverseSide,
		HideReverseSide,
		FlipReverseSide
	};

	/// <summary>
	/// Specifies how text in reversed direction should look.
	/// </summary>
	public enum ReverseDirectionStyle
	{
		ShowReverseDirection,
		HideReverseDirection,
		FlipReverseDirection
	};

	/// <summary>
	/// Enum type for accessing the predefined text styles.
	/// </summary>
	/// <remarks>
	/// User defined text styles have StyleKind = <see cref="TextStyleKind.Custom"/>.
	/// </remarks>
	public enum TextStyleKind
	{
		Default,
		DataValue,
		HighlightedDataValue,
		Golden,
		Custom
	}

	/// <summary>
	/// Represents a text style used in a chart
	/// </summary>
	public class TextStyle : NamedObjectBase, IDisposable
	{
		private		Font					font;
		private		Color					foregroundColor, shadowColor;
		private		double					shadowDepthPxl;
		private		ReverseSideStyle		revSideStyle = ReverseSideStyle.FlipReverseSide;
		private		ReverseDirectionStyle	revDirectionStyle = ReverseDirectionStyle.ShowReverseDirection;
		private		bool					is2D;

		private		bool					lockedReverseSide = false;
		private		bool					lockedReverseDirection = false;
		private		bool					reverseSide;
		private		bool					reverseDirection;

		// This variable is used in serialization.
		// The initial value sets serialization on by default.
		// For default styles, do
		// 1. Create
		// 2. Set properties, if needed
		// 3. Set HasChanged property to false
		protected internal	bool			hasChanged = true;

		#region --- Handling enum TextStyleKind ---

		private static string[] names = new string[]
			{
				"Default",
				"DataValue",
				"HighlightedDataValue",
				"Golden",
				"Custom"
			};
		
		private static TextStyleKind[] kinds = new TextStyleKind[]
			{
				TextStyleKind.Default,
				TextStyleKind.DataValue,
				TextStyleKind.HighlightedDataValue,
				TextStyleKind.Golden,
				TextStyleKind.Custom
			};

		internal static string NameOf(TextStyleKind kind)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(kind==kinds[i])
					return names[i];
			}
			throw new Exception("Implementation: arrays names/kinds in class 'Grid' mismatch");
		}

		internal static TextStyleKind KindOf(string name)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(name==names[i])
					return kinds[i];
			}
			return TextStyleKind.Custom;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextStyleKind StyleKind
		{
			get
			{
				return TextStyle.KindOf(Name);
			}
			set
			{
				Name = TextStyle.NameOf(value);
			}
		}

		#endregion

		#region --- Constructors ---
		/// <summary>
		/// Initializes a new instance of the <see cref="TextStyle"/> class with multiple specified parameters.
		/// </summary>
		/// <param name="name">The name of this <see cref="TextStyle"/> object.</param>
		/// <param name="font">The font of this <see cref="TextStyle"/> object.</param>
		/// <param name="foregroundColor">Color of this <see cref="TextStyle"/> object.</param>
		/// <param name="shadowColor">Shadow color of this <see cref="TextStyle"/> object.</param>
		/// <param name="shadowDepthPxl">Size of shadow of this text style.</param>
		/// <param name="revSideStyle">How text should look from the back.</param>
		/// <param name="revDirectionStyle">Direction of the text from the back.</param>
		/// <param name="is2D">Indicating whether the text is in 2D.</param>
		public TextStyle(string name,  Font font, Color foregroundColor, Color shadowColor,
			double shadowDepthPxl,ReverseSideStyle revSideStyle,ReverseDirectionStyle revDirectionStyle,
			bool is2D): base(name)
		{
			this.font = font;
			this.foregroundColor = foregroundColor;
			this.shadowColor = shadowColor;
			this.shadowDepthPxl = shadowDepthPxl;
			this.revSideStyle = revSideStyle;
			this.revDirectionStyle = revDirectionStyle;
			this.is2D = is2D;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextStyle"/> class with specified name.
		/// </summary>
		/// <param name="name">The name of this <see cref="TextStyle"/> object.</param>
		public TextStyle(string name) : this(name,  new Font("Arial",10),Color.Black,Color.Black,0,
			ReverseSideStyle.FlipReverseSide,ReverseDirectionStyle.FlipReverseDirection,false)
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextStyle"/>.
		/// </summary>
        public TextStyle()
            : this(null, new Font("Arial", 10), Color.Black, Color.Black, 0,
            ReverseSideStyle.FlipReverseSide, ReverseDirectionStyle.FlipReverseDirection, false) { }

        internal TextStyle(TextStyle s)
        {
            SetOwningChart(s.OwningChart);
            font = s.Font;
            foregroundColor = s.ForeColor;
            shadowColor = s.ShadowColor;
            shadowDepthPxl = s.ShadowDepthPxl;
            revSideStyle = s.RevSideStyle;
            revDirectionStyle = s.RevDirectionStyle;
            is2D = s.Is2D;

            lockedReverseSide = s.lockedReverseSide;
            lockedReverseDirection = s.lockedReverseDirection;
            reverseSide = s.reverseSide;
            reverseDirection = s.reverseDirection;
            SetOwningChart(s.OwningChart);
        }

		internal void LoadFrom(TextStyle s)
		{
			SetOwningChart(s.OwningChart);
			font = s.Font;
			foregroundColor = s.ForeColor;
			shadowColor = s.ShadowColor;
			shadowDepthPxl = s.ShadowDepthPxl;
			revSideStyle = s.RevSideStyle;
			revDirectionStyle = s.RevDirectionStyle;
			is2D = s.Is2D;

            lockedReverseSide = s.lockedReverseSide;
            lockedReverseDirection = s.lockedReverseDirection;
            reverseSide = s.reverseSide;
            reverseDirection = s.reverseDirection;
            SetOwningChart(s.OwningChart);
        }

		internal void SetOwningChart(ChartBase chart)
		{
			base.OwningChart = chart;
		}

		#endregion

		#region --- Properties ---
		internal bool					HasChanged			{ get { return hasChanged; }		set { hasChanged = value; } }

		/// <summary>
		/// The name of this <see cref="TextStyle"/> object.
		/// </summary>
		[Category("General")]
		[SRDescription("TextStyleNameDescr")]
		public new string				Name				{ get { return base.Name; }			set { if(base.Name != value) hasChanged = true; base.Name = value; } }

		/// <summary>
		/// Gets or sets the font of the text displayed by this <see cref="TextStyle"/> object.
		/// </summary>
		[DefaultValue(typeof(Font), "Arial, 10pt")]
		[Description("The font of the text displayed.")]
		[Category("Font")]
		public Font						Font				{ get { return font; }				set { if(font != value) hasChanged = true; font = value; } }
		/// <summary>
		/// Gets or sets the color of the text displayed by this <see cref="TextStyle"/> object.
		/// </summary>
		[DefaultValue(typeof(Color), "Black")]
		[Description("The color of the text displayed.")]
		[Category("Font")]
		public Color					ForeColor			{ get { return foregroundColor; }	set { if(foregroundColor != value) hasChanged = true; foregroundColor = value; } }
		/// <summary>
		/// Gets or sets the color of the shadow displayed by this <see cref="TextStyle"/> object.
		/// </summary>
		[DefaultValue(typeof(Color), "Black")]
		[Description("The shadow color of the text displayed.")]
		[Category("Font")]
		public Color					ShadowColor			{ get { return shadowColor; }		set { if(shadowColor != value) hasChanged = true; shadowColor = value; } }
		/// <summary>
		/// Gets or sets shadow depth in pixels.
		/// </summary>
		[SRDescription("TextStyleShadowDepthPxlDescr")]
		[DefaultValue(0.0)]
		[Category("Font")]
		public double					ShadowDepthPxl		{ get { return shadowDepthPxl; }	set { if(shadowDepthPxl != value) hasChanged = true; shadowDepthPxl = value; } }
		/// <summary>
		/// Gets or sets a value that specifies how text should look from the back.
		/// </summary>
		[SRDescription("TextStyleRevSideStyleDescr")]
		[DefaultValue(ReverseSideStyle.FlipReverseSide)]
		[Category("Font")]
		public ReverseSideStyle			RevSideStyle		{ get { return revSideStyle; }		set { if(revSideStyle != value) hasChanged = true; revSideStyle = value; } }
		/// <summary>
		/// Gets or sets the direction of the text from the back.
		/// </summary>
		[SRDescription("TextStyleRevDirectionStyleDescr")]
		[DefaultValue(ReverseDirectionStyle.FlipReverseDirection)]
		[Category("Font")]
		public ReverseDirectionStyle	RevDirectionStyle	{ get { return revDirectionStyle; }	set { if(revDirectionStyle != value) hasChanged = true; revDirectionStyle = value; } }
		/// <summary>
		/// Gets or sets a value indicating whether the text is in 2D.
		/// </summary>
		[DefaultValue(false)]
		[Description("Indicating whether the text is 2D.")]
		[Category("Font")]
		public bool						Is2D				{ get { return is2D; }				set { if(is2D != value) hasChanged = true; is2D = value; } }

		#endregion

		#region --- Serialization ---
		internal bool ShouldSerializeMe	{ get { return hasChanged; } }

		internal void ResetFont() {font=new Font("Arial",10);}
		internal bool ShouldSerializeFont() {return !font.Equals(new Font("Arial",10));}

		internal void ResetForegroundColor() {foregroundColor=Color.Black;}
		internal bool ShouldSerializeForegroundColor() {return foregroundColor!=Color.Black;}

		internal void ResetShadowColor() {shadowColor=Color.Black;}
		internal bool ShouldSerializeShadowColor() {return shadowColor!=Color.Black;}

		internal void ResetShadowDepthPxl() {shadowDepthPxl=0;}
		internal bool ShouldSerializeShadowDepthPxl() {return shadowDepthPxl!=0;}

		internal void ResetRevSideStyle() {revSideStyle=ReverseSideStyle.FlipReverseSide;}
		internal bool ShouldSerializeRevSideStyle() {return revSideStyle!=ReverseSideStyle.FlipReverseSide;}

		internal void ResetRevDirectionStyle() {revDirectionStyle=ReverseDirectionStyle.FlipReverseDirection;}
		internal bool ShouldSerializeRevDirectionStyle() {return revDirectionStyle!=ReverseDirectionStyle.FlipReverseDirection;}

		internal void ResetIs2D() {is2D=false;}
		internal bool ShouldSerializeIs2D() {return is2D!=false;}

		#endregion

		#region --- Rendering ---

		internal virtual void LockDirectionAndSideMode(Mapping mapping, Vector3D P, Vector3D Vx, Vector3D Vy, bool lockDirectionMOde, bool lockSideMode)
		{
			PrepareDirectionAndSideMode(mapping,P,Vx,Vy);
			lockedReverseDirection = lockDirectionMOde;
			lockedReverseSide = lockSideMode;
		}

		internal void PrepareDirectionAndSideMode(Mapping mapping, Vector3D P, Vector3D Vx, Vector3D Vy)
		{
			Vector3D PT,PXT,PYT;
			mapping.Map(P,out PT);
			mapping.Map(P+Vx,out PXT);
			mapping.Map(P+Vy,out PYT);
			Vector3D VxeTxt = PXT-PT;
			Vector3D VyeTxt = PYT-PT;

			// Decide text transformation

			// Flip side mode

			if(!lockedReverseSide)
			{
				if(VxeTxt.CrossProduct(VyeTxt).Z < 0.0)
				{
					if(revSideStyle == ReverseSideStyle.HideReverseSide)
						return;
					reverseSide = true;
				}
				else
					reverseSide = false;
			}

			// Flip direction mode

			if(!lockedReverseDirection)
			{
				reverseDirection = false;
				if(VyeTxt.Y < -0.001)
				{
					if(revDirectionStyle == ReverseDirectionStyle.HideReverseDirection)
						return;
					reverseDirection = revDirectionStyle == ReverseDirectionStyle.FlipReverseDirection; 
				}
			}
		}

        internal void GetWCSTextRectangle(Mapping mapping, string txt, ref Vector3D P, Vector3D Vx, Vector3D Vy,
			double xOffset, double yOffset, TextReferencePoint refPoint, double liftZ,
			out Vector3D VxText, out Vector3D VyText)
		{
			float sizeW = (float)(font.Size*mapping.FromPointToWorld);

			PrepareDirectionAndSideMode(mapping,P,Vx,Vy);

			float cLiftZ = (float)liftZ;				// Lift parameter is affected by the flip side mode
			if(reverseSide)
				cLiftZ = -Math.Abs(cLiftZ);

			bool reverseX = reverseSide && !reverseDirection || !reverseSide && reverseDirection;
			bool reverseY = reverseDirection;

			// Text size and initial text vectors

			Vector3D Vxe = Vector3D.Null;
			Vector3D Vye = Vector3D.Null;
			if(txt.Trim() == "")
			{
				VxText = new Vector3D(0,0,0);
				VyText = new Vector3D(0,0,0);
				return;
			}
			else
			{
				GraphicsPath path = new GraphicsPath(FillMode.Alternate);
				path.AddString(txt,font.FontFamily,(int)font.Style,sizeW,new PointF(0,0),StringFormat.GenericTypographic);

				float xMin = float.MaxValue, yMin = float.MaxValue, xMax = float.MinValue, yMax = float.MinValue;
				PointF[] points = path.PathPoints;
				int nPoints = points.Length;
				int i;
				for(i=0;i<nPoints;i++)
				{
					xMin = Math.Min(xMin,points[i].X);
					yMin = Math.Min(yMin,points[i].Y);
					xMax = Math.Max(xMax,points[i].X);
					yMax = Math.Max(yMax,points[i].Y);
				}
				Vxe = Vx.Unit();
				Vye = Vy.Unit();
				VxText = Vxe*(xMax-xMin);
				VyText = Vye*(yMax-yMin);
				path.Dispose();
			}

			// Offset

			P = P + Vxe*xOffset*mapping.FromPointToWorld + Vye*yOffset*mapping.FromPointToWorld;

			// Reference point. Point P is moved to the lower left corner of the text

			switch(refPoint)
			{
				case TextReferencePoint.LeftBottom:
					break;
				case TextReferencePoint.LeftCenter:
					P = P - (VyText*0.5);
					break;
				case TextReferencePoint.LeftTop:
					P = P - VyText;
					break;
				case TextReferencePoint.CenterBottom:
					P = P - (VxText*0.5);
					break;
				case TextReferencePoint.Center:
					P = P - (VyText*0.5) - (VxText*0.5);
					break;
				case TextReferencePoint.CenterTop:
					P = P - VyText - (VxText*0.5);
					break;
				case TextReferencePoint.RightBottom:
					P = P - VxText;
					break;
				case TextReferencePoint.RightCenter:
					P = P - VxText - VyText*0.5;
					break;
				case TextReferencePoint.RightTop:
					P = P - VxText - VyText;
					break;
				default:
					break;
			}

			// Modifying text vectors and reference point
			
			double fx = 1, fy = 1;
			if(reverseX)
			{
				P = P + VxText;
				VxText = - VxText;
				fx = -1;
			}
			if(reverseY)
			{
				P = P + VyText;
				VyText = - VyText;
				fy = -1;
			}

			// Accomodate the shadow
			if(shadowDepthPxl>0)
			{
				double d = shadowDepthPxl/mapping.Enlargement;
				VxText = VxText + Vxe*d*fx;
				VyText = VyText + Vye*d*fy;
			}
		}

		internal DrawingBoard Render(GeometricEngine GE,Mapping mapping, string txt, Vector3D P, Vector3D Vx, Vector3D Vy, double xOffset, double yOffset, TextReferencePoint refPoint, double liftZ)
		{
			Vector3D VxText, VyText;
			if(txt == null || txt.Trim() == "")
				return null;
			GetWCSTextRectangle(mapping, txt, ref P, Vx, Vy, xOffset, yOffset, refPoint, liftZ, out VxText, out VyText);

            DrawingBoard B = GE.CreateDrawingBoard(P, VxText, VyText);
			B.LiftZ = liftZ;

			// Accomodate the shadow
			
			PointF origin = new PointF(0,0);
			if(shadowDepthPxl>0)
			{
				double d = shadowDepthPxl/mapping.Enlargement;
				origin = new PointF(0,(float)d);
			}

			B.DrawString(txt,this,origin,0);
			B.LightsOff = true;
            return B;
		}

		#endregion

		#region --- XML Serialization ---

		internal void Serialize(XmlCustomSerializer S)
		{
			S.AttributeProperty(this,"Name");
			S.AttributeProperty(this,"Font");
			S.AttributeProperty(this,"ForeColor");
			S.AttributeProperty(this,"ShadowColor");
			S.AttributeProperty(this,"ShadowDepthPxl");
			S.AttributeProperty(this,"RevSideStyle");
			S.AttributeProperty(this,"RevDirectionStyle");
			S.AttributeProperty(this,"Is2D");
		}

		internal void CreateDOMAttributes(XmlElement root)
		{
			TypeConverter fontConverter = TypeDescriptor.GetConverter(typeof(Font));
			TypeConverter colorConverter = TypeDescriptor.GetConverter(typeof(Color));
			TypeConverter RSSConverter = TypeDescriptor.GetConverter(typeof(ReverseSideStyle));
			TypeConverter RDSConverter = TypeDescriptor.GetConverter(typeof(ReverseDirectionStyle));
			
			root.SetAttribute("Name",Name);
			root.SetAttribute("Font",fontConverter.ConvertToString(this.Font));
			root.SetAttribute("ForeColor",colorConverter.ConvertToString(ForeColor));
			root.SetAttribute("ShadowColor",colorConverter.ConvertToString(ShadowColor));
			root.SetAttribute("ShadowDepthPxl",ShadowDepthPxl.ToString());
			root.SetAttribute("RevSideStyle",RSSConverter.ConvertToString(RevSideStyle));
			root.SetAttribute("RevDirectionStyle",RDSConverter.ConvertToString(RevDirectionStyle));
			root.SetAttribute("Is2D",Is2D.ToString());
		}

		internal XmlElement CreateDOM(XmlDocument doc)
		{
			XmlElement root = doc.CreateElement("TextStyle");
			CreateDOMAttributes(root);
			return root;
		}

		internal static void CreatePropertiesFromDOM(XmlElement root,TextStyle S)
		{
			TypeConverter fontConverter = TypeDescriptor.GetConverter(typeof(Font));
			TypeConverter colorConverter = TypeDescriptor.GetConverter(typeof(Color));
			TypeConverter RSSConverter = TypeDescriptor.GetConverter(typeof(ReverseSideStyle));
			TypeConverter RDSConverter = TypeDescriptor.GetConverter(typeof(ReverseDirectionStyle));

			S.Name = root.GetAttribute("Name");
			S.Font = (Font)fontConverter.ConvertFromString(root.GetAttribute("Font"));
			S.ForeColor = (Color)colorConverter.ConvertFromString(root.GetAttribute("ForeColor"));
			S.ShadowColor = (Color)colorConverter.ConvertFromString(root.GetAttribute("ShadowColor"));
			S.ShadowDepthPxl = double.Parse(root.GetAttribute("ShadowDepthPxl"));
			S.RevSideStyle = (ReverseSideStyle)RSSConverter.ConvertFromString(root.GetAttribute("RevSideStyle"));
			S.RevDirectionStyle = (ReverseDirectionStyle)RDSConverter.ConvertFromString(root.GetAttribute("RevDirectionStyle"));
			S.Is2D = bool.Parse(root.GetAttribute("Is2D"));
		}

		internal static TextStyle CreateFromDOM(XmlElement root)
		{
			if(root.Name.ToLower() != "textstyle")
				return null;
			TextStyle S = new TextStyle();
			CreatePropertiesFromDOM(root,S);
			return S;
		}
		#endregion

		#region IDisposable Members


		internal void Dispose(bool disposing) 
		{
			if (disposing)
			{
				if (font != null) 
				{
					font.Dispose();
					font = null;
				}
			}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}

}
