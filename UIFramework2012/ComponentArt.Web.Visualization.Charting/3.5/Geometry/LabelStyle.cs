using System;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Xml;
using ComponentArt.Web.Visualization.Charting.Design;
using ComponentArt.Web.Visualization.Charting.Geometry;


namespace ComponentArt.Web.Visualization.Charting
{
	public enum LabelStyleKind 
	{
		Default,
		DefaultAxisLabels,
		Custom
	};

	/// <summary>
	/// Specifies where the reference point of the label is. 
	/// </summary>
	public enum TextReferencePoint
	{
		/// <summary>
		/// Let the control choose reference point.
		/// </summary>
		Default,
		/// <summary>
		/// The reference point is the left-bottom corner of the label.
		/// </summary>
		LeftBottom,
		/// <summary>
		/// The reference point is the left-center point of the label.
		/// </summary>
		LeftCenter,
		/// <summary>
		/// The reference point is the left-top corner of the label.
		/// </summary>
		LeftTop,
		/// <summary>
		/// The reference point is the center-bottom point of the label.
		/// </summary>
		CenterBottom,
		/// <summary>
		/// The reference point is the center of the label.
		/// </summary>
		Center,
		/// <summary>
		/// The reference point is the center-top point of the label.
		/// </summary>
		CenterTop,
		/// <summary>
		/// The reference point is the right-bottom corner of the label.
		/// </summary>
		RightBottom,
		/// <summary>
		/// The reference point is the right-center point of the label.
		/// </summary>
		RightCenter,
		/// <summary>
		/// The reference point is the right-top corner of the label.
		/// </summary>
		RightTop

	}

	/// <summary>
	///     Specifies the text orientation that is apllied to the label.
	/// </summary>
	/// <remarks>
	///   <para>
	///     Text orientation in 3D space is defined by two vectors: text direction and 
	///     text normal direction vector. When drawing latin capital "L", the direction of the horizontal 
	///     segment is "horizontal" text direction, direction of the vertical segment is "vertical" text direction.
	///     Therefore, expressions "horizontal" and "vertical" are used with respect to text orientation
	///     and need not be in in horizontal or vertical direction of the cart coordinate system.
	///   </para>
	///   <para>
	///     Text direction vectors are often parallel to coordinate axes. This makes possible easy
	///     direction specification using this enum type and directions of coordinate axes in 
	///     Data Coordinate System (DCS). Note that, for example, X-axis in this coordinate system 
	///     is not always horizontal, but might be affected by the coordinate system orientation.
	///     X-axis is where x-coordinates of data points are placed, y-axis is where y-coordinates are
	///     placed.
	///   </para>
	/// </remarks>
	public enum TextOrientation
	{
		/// <summary>
		/// Let the control decide what is the best text orientation.
		/// </summary>
		Default = 0,
		/// <summary>
		/// X-axis is text direction, Y-axis is normal direction.
		/// </summary>
		XYOrientation = 1,
		/// <summary>
		/// X-axis is text direction, Y-axis is normal direction.
		/// </summary>
		XZOrientation = 2,
		/// <summary>
		/// X-axis is text direction, Z-axis is normal direction.
		/// </summary>
		YZOrientation = 3,
		/// <summary>
		/// Y-axis is text direction, Z-axis is normal direction.
		/// </summary>
		YXOrientation = 4,
		/// <summary>
		/// Y-axis is text direction, X-axis is normal direction.
		/// </summary>
		ZXOrientation = 5,
		/// <summary>
		/// Z-axis is text direction, X-axis is normal direction.
		/// </summary>
		ZYOrientation = 6,
		/// <summary>
		/// Use text direction and normal direction vectors as defined in the label style.
		/// </summary>
		UserDefined = 7
	}

	internal enum HorizontalAlignment
	{
		Left,
		Center,
		Right
	};
	internal enum VerticalAlignment
	{
		Bottom,
		Center,
		Top
	};

	/// <summary>
	///     Specifies the text orientation that is apllied to the x-coordinate labels in a radar chart.
	/// </summary>
	public enum PolarTextOrientation
	{
		/// <summary>
		/// X-coordinates are horizontal.
		/// </summary>
		Horizontal,
		/// <summary>
		/// x-coordinates are circular, i.e. in the direction of the tangent to the radar circle.
		/// </summary>
		Circular,
		/// <summary>
		/// x-coordinates are radial, i.e. in the direction of the radius of the radar circle.
		/// </summary>
		Radial
	}

	// ================================================================================================
	//		LabelStyle
	// ================================================================================================

	/// <summary>
	/// Defines a particular style for labels, including all TextStyle attributes as well as orientation, offsets, .
	/// </summary>
	public class LabelStyle : TextStyle 
	{
		// Formatting
		private string	formattingString = "G";

		// Text position attributes

		private Vector3D	Vh = new Vector3D(1,0,0);
		private Vector3D	Vv = new Vector3D(0,1,0);
		private TextOrientation 
			textOrientation = TextOrientation.Default;
		private TextReferencePoint
			textReferencePoint=TextReferencePoint.Default;
		private double	hOffset = 0.0;
		private double	vOffset = 0.0;
		private double	angle = 0.0;
		private double	liftZ = 0.1;
		private bool	defaultHOffset = true;
		private bool	defaultVOffset = true;

		#region --- Constructors ---

		/// <summary>
		/// Initializes a new instance of the <see cref="LabelStyle"/> class.
		/// </summary>
		/// <param name="name">The name of the label style. This value is stored in the <see cref="LabelStyle.Name"/> property.</param>
		/// <param name="Vh">Horizontal direction of the text. This value is stored in the <see cref="LabelStyle.HorizontalDirection"/> property.</param>
        /// <param name="Vv">Vertical direction of the text. This value is stored in the <see cref="LabelStyle.VerticalDirection"/> property.</param>
		/// <param name="textReferencePoint">The reference point of the text. This value is stored in the <see cref="LabelStyle.TextReferencePoint"/> property.</param>
		/// <param name="hOffset">Offset of the text in the horizontal direction. This value is stored in the <see cref="LabelStyle.HOffset"/> property.</param>
		/// <param name="vOffset">Offset of the text in the vertical direction. This value is stored in the <see cref="LabelStyle.VOffset"/> property.</param>
		/// <param name="angle">Angle of the text in degrees relative to the horizontal direction vector. This value is stored in the <see cref="LabelStyle.Angle"/> property.</param>
		/// <param name="liftZ">The position of the label above its plane. This value is stored in the <see cref="LabelStyle.LiftZ"/> property.</param>
		public LabelStyle(string name, Vector3D Vh, Vector3D Vv,
			TextReferencePoint textReferencePoint,
			double hOffset, double vOffset, double angle,double liftZ) : base(name)
		{
			this.Vh = Vh;
			this.Vv = Vv;
			this.textOrientation = TextOrientation.UserDefined;
			this.textReferencePoint = textReferencePoint;
			this.HOffset = hOffset;
			this.VOffset = vOffset;
			this.angle = angle;
			this.liftZ = liftZ;
		}

        internal LabelStyle(LabelStyle orig)
            : base(orig)
        {
            this.formattingString = orig.formattingString;
            this.Vh = orig.Vh;
            this.Vv = orig.Vv;
            this.textOrientation = orig.textOrientation;
            this.textReferencePoint = orig.textReferencePoint;
            this.hOffset = orig.hOffset;
            this.vOffset = orig.vOffset;
            this.angle = orig.angle;
            this.liftZ = orig.liftZ;
            this.defaultHOffset = orig.defaultHOffset;
            this.defaultVOffset = orig.defaultVOffset;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="LabelStyle"/> class with default parametetrs.
		/// </summary>
		public LabelStyle() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="LabelStyle"/> class.
		/// </summary>
		/// <param name="name">The name of the label style. This value is stored in the <see cref="LabelStyle.Name"/> property.</param>
		/// <param name="textOrientation">Orientation kind of the label style. This value is stored in the <see cref="LabelStyle.TextOrientation"/> property.</param>
		/// <param name="textReferencePoint">The reference point of the text. This value is stored in the <see cref="LabelStyle.TextReferencePoint"/> property.</param>
		/// <param name="hOffset">Offset of the text in the horizontal direction. This value is stored in the <see cref="LabelStyle.HOffset"/> property.</param>
		/// <param name="vOffset">Offset of the text in the vertical direction. This value is stored in the <see cref="LabelStyle.VOffset"/> property.</param>
		/// <param name="angle">Angle of the text in degrees relative to the horizontal direction vector. This value is stored in the <see cref="LabelStyle.Angle"/> property.</param>
		/// <param name="liftZ">The position of the label above its plane. This value is stored in the <see cref="LabelStyle.LiftZ"/> property.</param>
		public LabelStyle(string name, TextOrientation textOrientation, TextReferencePoint textReferencePoint,
			double hOffset, double vOffset, double angle,double liftZ) : base(name)
		{
			this.textOrientation = textOrientation;
			this.textReferencePoint = textReferencePoint;
			this.HOffset = hOffset;
			this.VOffset = vOffset;
			this.angle = angle;
			this.liftZ = liftZ;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="LabelStyle"/> class with a specified name.
		/// </summary>
		/// <param name="name">The name of this label style. This value is stored in the <see cref="LabelStyle.Name"/> property.</param>
		public LabelStyle(string name) : this(name,TextOrientation.Default,
			TextReferencePoint.Default, 0.0,0.0,0.0, 0.1) {}

		internal void LoadFrom(LabelStyle s)
		{
			base.LoadFrom(s);
			this.textOrientation = s.textOrientation;
			this.Vh = s.Vh;
			this.Vv = s.Vv;
			this.textReferencePoint = s.textReferencePoint;
			this.hOffset = s.hOffset;
			this.vOffset = s.vOffset;
			this.angle = s.angle;
			this.liftZ = s.liftZ;
			this.defaultHOffset = s.DefaultHOffset;
			this.defaultVOffset = s.DefaultVOffset;
			this.FormattingString = s.FormattingString;
        }

		// Internaly created styles have h/v offsets considered default;
		internal LabelStyle MarkCreatedInternally()
		{
			this.defaultHOffset = true;
			this.defaultVOffset = true;
			return this;
		}
		#endregion

		#region --- Handling enum LabelStyleKind ---

		private static string[] names = new string[]
			{
				"Default",
				"DefaultAxisLabels",
				"Custom"
			};
		
		private static LabelStyleKind[] kinds = new LabelStyleKind[]
			{
				LabelStyleKind.Default,
				LabelStyleKind.DefaultAxisLabels,
				LabelStyleKind.Custom
			};

		internal static string NameOf(LabelStyleKind kind)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(kind==kinds[i])
					return names[i];
			}
			throw new Exception("Implementation: arrays names/kinds in class 'LabelStyle' mismatch");
		}

		internal new static LabelStyleKind KindOf(string name)
		{
			for(int i=0; i<kinds.Length;i++)
			{
				if(name==names[i])
					return kinds[i];
			}
			return LabelStyleKind.Custom;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LabelStyleKind LabelStyleKind
		{
			get
			{
				return LabelStyle.KindOf(Name);
			}
			set
			{
				Name = LabelStyle.NameOf(value);
			}
		}

		#endregion

		#region --- Properties ---

		#region --- Text Formatting ---

		/// <summary>
		/// Gets or sets the formatting string of this <see cref="Label"/> object.
		/// </summary>
		[DefaultValue("G")]
		[Category("General")]
		[Description("The formatting string of the label.")]
		public string FormattingString { get { return formattingString; } set { if(formattingString != value) hasChanged = true; formattingString = value; } }
		#endregion

		#region --- Category: Text Orientation ---
		/// <summary>
		/// Gets or sets the orientation kind of this label style.
		/// </summary>
		[DefaultValue(TextOrientation.Default)]
		[Category("Text Orientation"), Description("Orientation kind")]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		public virtual TextOrientation			Orientation			{ get { return textOrientation; } set { if(textOrientation != value) hasChanged = true; textOrientation = value; } }
		/// <summary>
		/// Gets or sets the vector representing the horizontal direction of the label text.
		/// </summary>
		[DefaultValue(typeof(Vector3D), "(1,0,0)")]
		[Category("Text Orientation"), Description("Vector representing the horizontal direction of the label text")]
		public virtual Vector3D HorizontalDirection	
		{
			get { return (Orientation == TextOrientation.Default || Orientation == TextOrientation.UserDefined)? Vh:HDirection(Orientation); }
			set { if(Vh != value) hasChanged = true; Vh = value; }
		}
		/// <summary>
		/// Gets or sets the vector representing the vertical direction of the label.
		/// </summary>
		[DefaultValue(typeof(Vector3D), "(0,1,0)")]
		[Category("Text Orientation"), Description("Vector representing the vertical direction of the label text")]
		public virtual Vector3D VerticalDirection	
		{
			get { return (Orientation == TextOrientation.Default || Orientation == TextOrientation.UserDefined)? Vv:VDirection(Orientation); }
			set { if(Vv != value) hasChanged = true; Vv = value; }
		}
		#endregion
		
		#region --- Category: Text Positioning ---
		/// <summary>
		/// Gets or sets the reference point of the text.
		/// </summary>
		[DefaultValue(TextReferencePoint.Default)]
		[Category("Text Positioning"),Description("Text reference point")]
		public virtual TextReferencePoint		ReferencePoint		
		{
			get 
			{
				return textReferencePoint; 
			}
			set 
			{
				if(textReferencePoint != value) 
					hasChanged = true; 
				textReferencePoint = value; 
			}
		}
		/// <summary>
		/// Gets or sets the offset of the text in the horizontal direction defined by <see cref="HorizontalDirection"/>.
		/// </summary>
		[DefaultValue((double) 0)]
		[Category("Text Positioning"),Description("Reference point offset in text direction (points)")]
		public virtual double HOffset { get { return hOffset; } set { if(hOffset != value) hasChanged = true; hOffset = value; defaultHOffset = false; } }
		/// <summary>
		/// Gets or sets the offset of the text in the vertical direction defined by <see cref="VerticalDirection"/>.
		/// </summary>
		[DefaultValue(0.0)]
		[Category("Text Positioning"),Description("Reference point offset in vertical direction (points)")]
		public virtual double VOffset { get { return vOffset; } set { if(vOffset != value) hasChanged = true; vOffset = value; defaultVOffset = false; } }
		/// <summary>
		/// Gets or sets the angle of the text about its normal.
		/// </summary>
		[DefaultValue(0.0)]
		[Category("Text Positioning"),Description("Text angle relative to the horizontal direction (degrees)")]
		internal virtual double Angle   { get { return angle; }   set { if(angle != value) hasChanged = true; angle = value; } }		
		/// <summary>
		/// Gets or sets the position of the label above its plane.
		/// </summary>
		[DefaultValue(0.1)]
		[Category("Text Positioning"),Description("Text lift amount (pixels)")]
		public virtual double LiftZ   { get { return liftZ; }	set { if(liftZ != value) hasChanged = true; liftZ = value; } }
		
		internal bool DefaultHOffset { get { return defaultHOffset; } set { defaultHOffset = value; } }
		internal bool DefaultVOffset { get { return defaultVOffset; } set { defaultVOffset = value; } }

		internal override void LockDirectionAndSideMode(Mapping mapping, Vector3D P, Vector3D Vx, Vector3D Vy, bool lockDirectionMode, bool lockSideMode)
		{
			double c = Math.Cos(angle*Math.PI/180);
			double s = Math.Sin(angle*Math.PI/180);
			Vector3D Vxa = HorizontalDirection*c + VerticalDirection*s;
			Vector3D Vya = VerticalDirection*c - HorizontalDirection*s;
			base.LockDirectionAndSideMode(mapping,P,Vxa,Vya,lockDirectionMode, lockSideMode);
		}
		#endregion

		#region --- Internal Properties ---
			internal virtual TextReferencePoint EffectiveReferencePoint
		{
			get 
			{
				if(ReferencePoint == TextReferencePoint.Default)
					return TextReferencePoint.LeftBottom;
				else
					return textReferencePoint;
			}
		}

		internal HorizontalAlignment HorizontalAlignment
		{
			get
			{
				switch(EffectiveReferencePoint)
				{
					case TextReferencePoint.LeftBottom:
						return HorizontalAlignment.Left;
					case TextReferencePoint.LeftCenter:
						return HorizontalAlignment.Left;
					case TextReferencePoint.LeftTop:
						return HorizontalAlignment.Left;
					case TextReferencePoint.CenterBottom:
						return HorizontalAlignment.Center;
					case TextReferencePoint.Center:
						return HorizontalAlignment.Center;
					case TextReferencePoint.CenterTop:
						return HorizontalAlignment.Center;
					case TextReferencePoint.RightTop:
						return HorizontalAlignment.Right;
					case TextReferencePoint.RightCenter:
						return HorizontalAlignment.Right;
					case TextReferencePoint.RightBottom:
						return HorizontalAlignment.Right;
				}
				return HorizontalAlignment.Left; // should never happen; 
			}
		}
		
		internal VerticalAlignment VerticalAlignment
		{
			get
			{
				switch(EffectiveReferencePoint)
				{
					case TextReferencePoint.LeftBottom:
						return VerticalAlignment.Bottom;
					case TextReferencePoint.LeftCenter:
						return VerticalAlignment.Center;
					case TextReferencePoint.LeftTop:
						return VerticalAlignment.Top;
					case TextReferencePoint.CenterBottom:
						return VerticalAlignment.Bottom;
					case TextReferencePoint.Center:
						return VerticalAlignment.Center;
					case TextReferencePoint.CenterTop:
						return VerticalAlignment.Top;
					case TextReferencePoint.RightBottom:
						return VerticalAlignment.Bottom;
					case TextReferencePoint.RightCenter:
						return VerticalAlignment.Center;
					case TextReferencePoint.RightTop:
						return VerticalAlignment.Top;
				}
				return VerticalAlignment.Bottom; // should never happen; 
			}
		}

		#endregion

		#endregion
	
		#region --- Static Helpers ---
		static internal Vector3D HDirection(TextOrientation orientation)
		{
			switch(orientation)
			{
				case TextOrientation.XYOrientation:
					return new Vector3D(1,0,0);
				case TextOrientation.XZOrientation:
					return new Vector3D(1,0,0);
				case TextOrientation.YZOrientation:
					return new Vector3D(0,1,0);
				case TextOrientation.YXOrientation:
					return new Vector3D(0,1,0);
				case TextOrientation.ZXOrientation:
					return new Vector3D(0,0,-1);
				case TextOrientation.ZYOrientation:
					return new Vector3D(0,0,-1);
				default: // Default and UserDefined
					return new Vector3D(0,0,0);
			}
		}

		static internal Vector3D VDirection(TextOrientation orientation)
		{
			switch(orientation)
			{
				case TextOrientation.XYOrientation:
					return new Vector3D(0,1,0);
				case TextOrientation.XZOrientation:
					return new Vector3D(0,0,-1);
				case TextOrientation.YZOrientation:
					return new Vector3D(0,0,1);
				case TextOrientation.YXOrientation:
					return new Vector3D(-1,0,0);
				case TextOrientation.ZXOrientation:
					return new Vector3D(-1,0,0);
				case TextOrientation.ZYOrientation:
					return new Vector3D(0,1,0);
				default: // Default and UserDefined
					return new Vector3D(0,0,0);
			}
		}
		#endregion
		
		#region --- Serialization and Browsing Control ---
		private bool ShouldBrowseVerticalDirection() { return Orientation == TextOrientation.UserDefined; }
		private bool ShouldBrowseHorizontalDirection() { return Orientation == TextOrientation.UserDefined; }

		#endregion

        internal DrawingBoard Render(GeometricEngine GE, ChartText text)
        {
            if (!Vh.IsNull && !Vv.IsNull)
                return Render(GE, text.Mapping,text.Text, text.Location, Vh, Vv, hOffset, vOffset, textReferencePoint, liftZ);
            else
                return null;
        }
        internal DrawingBoard Render(GeometricEngine GE, Mapping map, string txt, Vector3D P, Vector3D Vx, Vector3D Vy)
        {
            if (!Vx.IsNull && !Vy.IsNull)
                return Render(GE, map,txt, P, Vx, Vy, hOffset, vOffset, textReferencePoint, liftZ);
            else
                return null;
        }

        internal void GetWCSTextRectangle(Mapping map, string txt, ref Vector3D P,
            out Vector3D VxText, out Vector3D VyText)
        {
			double c = Math.Cos(angle*Math.PI/180);
			double s = Math.Sin(angle*Math.PI/180);
			Vector3D Vhe = c*Vh + s*Vv;
			Vector3D Vve = c*Vv - s*Vh;

			GetWCSTextRectangle(map, txt, ref P, Vhe, Vve, hOffset, vOffset, textReferencePoint, liftZ, out VxText, out VyText);
        }

		#region --- XML Serialization ---
		internal void Serialize(XmlCustomSerializer S)
		{
			base.Serialize(S);
			S.AttributeProperty(this,"HorizontalDirection");
			S.AttributeProperty(this,"VerticalDirection");
			S.AttributeProperty(this,"Orientation");
			S.AttributeProperty(this,"ReferencePoint");
			S.AttributeProperty(this,"HOffset");
			S.AttributeProperty(this,"VOffset");
			S.AttributeProperty(this,"Angle");
			S.AttributeProperty(this,"LiftZ","LiftZ","0.0##");
		}

		internal void CreateDOMAttributes(XmlElement root)
		{
			TypeConverter vectorConverter = TypeDescriptor.GetConverter(typeof(Vector3D));
			TypeConverter textOrientationConverter = TypeDescriptor.GetConverter(typeof(TextOrientation));
			TypeConverter textRefPointConverter = TypeDescriptor.GetConverter(typeof(TextReferencePoint));
			
			base.CreateDOMAttributes(root);

			root.SetAttribute("HorizontalDirection",vectorConverter.ConvertToString(HorizontalDirection));
			root.SetAttribute("VerticalDirection",vectorConverter.ConvertToString(VerticalDirection));
			root.SetAttribute("Orientation",textOrientationConverter.ConvertToString(Orientation));
			root.SetAttribute("TextReferencePoint",textRefPointConverter.ConvertToString(ReferencePoint));

			root.SetAttribute("HOffset",HOffset.ToString());
			root.SetAttribute("VOffset",VOffset.ToString());
			root.SetAttribute("Angle",Angle.ToString());
			root.SetAttribute("LiftZ",LiftZ.ToString());
		}

		internal void CreateDOM(XmlElement parent)
		{
			XmlDocument doc = parent.OwnerDocument;
			XmlElement root = doc.CreateElement("LabelStyle");
			CreateDOMAttributes(root);
			parent.AppendChild(root);
		}

		internal static void CreatePropertiesFromDOM(XmlElement root,LabelStyle S)
		{
			TypeConverter vectorConverter = TypeDescriptor.GetConverter(typeof(Vector3D));
			TypeConverter textOrientationConverter = TypeDescriptor.GetConverter(typeof(TextOrientation));
			TypeConverter textRefPointConverter = TypeDescriptor.GetConverter(typeof(TextReferencePoint));

			TextStyle.CreatePropertiesFromDOM(root,S);

			S.HorizontalDirection = (Vector3D)vectorConverter.ConvertFromString(root.GetAttribute("HorizontalDirection"));
			S.VerticalDirection = (Vector3D)vectorConverter.ConvertFromString(root.GetAttribute("VerticalDirection"));
			S.Orientation = (TextOrientation)textOrientationConverter.ConvertFromString(root.GetAttribute("Orientation"));
			S.ReferencePoint = (TextReferencePoint)textRefPointConverter.ConvertFromString(root.GetAttribute("TextReferencePoint"));

			S.HOffset = double.Parse(root.GetAttribute("HOffset"));
			S.VOffset = double.Parse(root.GetAttribute("VOffset"));
			S.Angle = double.Parse(root.GetAttribute("Angle"));
			S.LiftZ = double.Parse(root.GetAttribute("LiftZ"));
		}

		internal static LabelStyle CreateFromDOM(XmlElement root)
		{
			if(root.Name.ToLower() != "labelstyle")
				return null;
			LabelStyle S = new LabelStyle("");
			CreatePropertiesFromDOM(root,S);
			return S;
		}
		#endregion
	}
}



