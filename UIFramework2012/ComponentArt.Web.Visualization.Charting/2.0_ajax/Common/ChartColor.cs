using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel.Design;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// ChartColor is extension of the GDI+ concept of <see cref="System.Drawing.Color"/>, containing the <see cref="Reflection"/>
	/// and <see cref="LogPhong"/> (logarithm of Phong exponent) properties.
	/// </summary>
	[System.ComponentModel.EditorAttribute(typeof(ChartColorEditor), typeof(System.Drawing.Design.UITypeEditor))]
	[TypeConverter(typeof(ChartColorConverter))]
	[Serializable]
	public class ChartColor : ICloneable
	{
		//private Color color;

		private float	alpha,red,green,blue;
		private int		phong;
		private float	reflection;
		private float	halfPhongFraction = 0.25f;

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartColor"/> with specific reflection log of phong and color.
		/// </summary>
		/// <param name="reflection">The reflection of the surface in this <see cref="ChartColor"/> object. The value is assigned to <see cref="ChartColor.Reflection"/> property.</param>
		/// <param name="logPhong">The size of the specular highlight expressed as a base 2 log of phong coefficient. The value is assigned to the <see cref="ChartColor.LogPhong"/> property.</param>
		/// <param name="color">The <see cref="System.Drawing.Color"/> of this <see cref="ChartColor"/> object. The value is assigned to the <see cref="ChartColor.Color"/> property.</param>
		public ChartColor(float reflection, int logPhong, Color color)
		{
			this.reflection = Math.Min(1.0f,Math.Max(0.0f,reflection));
			this.phong = logPhong;
			Color = color;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartColor"/> with specific color.
		/// </summary>
		/// <param name="color">The <see cref="System.Drawing.Color"/> of this <see cref="ChartColor"/> object. The value is assigned to the <see cref="ChartColor.Color"/> property.</param>
		public ChartColor(Color color) : this(0.5f,6,color) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartColor"/> with white color and other default parameters.
		/// </summary>
		public ChartColor() : this(Color.White) { }
		
		#region --- Properties ---

		/// <summary>
		/// Gets or sets the alpha component value of this <see cref="ChartColor"/> object.
		/// </summary>
		[Description("The alpha component value")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public float Alpha		{ get { return alpha; }			set { alpha = value; } }
		/// <summary>
		/// Gets or sets the red component value of this <see cref="ChartColor"/> object.
		/// </summary>
		[Description("The red component value")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public float Red		{ get { return red  ; }			set { red   = value; } }
		/// <summary>
		/// Gets or sets the green component value of this <see cref="ChartColor"/> object.
		/// </summary>
		[Description("The green component value")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public float Green		{ get { return green; }			set { green = value; } }
		/// <summary>
		/// Gets or sets the blue component value of this <see cref="ChartColor"/> object.
		/// </summary>
		[Description("The blue component value")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public float Blue		{ get { return blue ; }			set { blue  = value; } }

		/// <summary>
		/// Gets or sets the reflection of the surface in this <see cref="ChartColor"/> object.
		/// </summary>
		[Description("The reflection of the chart color")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public float Reflection	{ get { return reflection; }	set { reflection = value; } }
		/// <summary>
		/// Gets or sets the size of the specular highlight expressed as a base 2 log of phong coefficient in this <see cref="ChartColor"/> object.
		/// </summary>
		/// <remarks>
		/// The range of acceptable values is from 0 to 8. Use 0 for a big highlight and 8 for a small highlight.
		/// </remarks>
		[Description("The logarithm of Phong coefficient of the chart color")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int   LogPhong		{ get { return phong; }			set { phong = value; } }

		#endregion

		/// <summary>
		/// Creates an exact copy of the <see cref="ChartColor"/>.
		/// </summary>
		/// <returns>An exact copy of the <see cref="ChartColor"/>.</returns>
		public object Clone()
		{
			return new ChartColor(reflection,phong,Color);
		}


		/// <summary>
		/// Returns a value indicating whether this <see cref="ChartColor"/> is equal to a specified object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns><see langword="true" /> if obj is an instance of <see cref="ChartColor"/> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
		public override bool Equals(Object obj) 
		{
			if(obj == null || this.GetType() != obj.GetType())
				return false;

			ChartColor s = (ChartColor)obj;
			return s.phong == this.phong && s.reflection == this.reflection
				&& s.alpha == alpha && s.red == red && s.green == green && s.blue == blue;
		}

		/// <summary>
		/// Indicates whether this <see cref="ChartColor"/> object can be converted to <see cref="System.Drawing.Color"/> object.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="ChartColor"/> object can be converted to <see cref="System.Drawing.Color"/>; otherwise, <see langword="false" /></returns>
		public bool CanConvertToColor() 
		{
			return true;
		}

		/// <summary>
		/// Converts a <see cref="System.Drawing.Color"/> to a <see cref="ChartColor"/>.
		/// </summary>
		/// <param name="color">A <see cref="System.Drawing.Color"/> struct.</param>
		/// <returns>A <see cref="ChartColor"/> that represents the converted <see cref="System.Drawing.Color"/>.</returns>
		public static implicit operator ChartColor ( Color color )
		{
			return new ChartColor(0.5f,6,color);
		}
		
		private int FtoInt(float f) { return Math.Min(255,(int)(f*255+0.001f)); }

		/// <summary>
		/// Computong color in given single light condition
		/// </summary>
		/// <param name="spToNormal">Degree of colinearity between the light direction and normal to the surface
		/// (Scalar product between unit vectors)
		/// </param>
		/// <param name="spToViewDirection">Degree of colinearity between the reflected light direction and
		/// view direction.</param>
		/// <param name="ambientFraction">Ambient light fraction (between 0 and 1)</param>
		/// <returns>The point color</returns>
		internal Color GetColor(double spToNormal, double spToViewDirection, double ambientFraction)
		{
			float colorFraction = (float)ambientFraction;
			if(spToNormal>0)
				colorFraction += (float)(spToNormal*(1-ambientFraction));
			
			// Compute composed phong weight for white color component of reflected light
			float whiteFraction = 0;
			if(spToViewDirection > 0)
			{
				int phong2 = (phong+1)/2;
				float p = (float)spToViewDirection;
				int i;
				for(i=0; i<phong2; i++)
					p *= p;
				whiteFraction = p*halfPhongFraction;
				for(i=phong2; i<phong; i++)
					p *= p;
				whiteFraction += p*(1-halfPhongFraction);
				whiteFraction *= reflection;
				// we don't want total white reflection:
				whiteFraction = Math.Min(0.9f,whiteFraction);
			}
			
			// Transparency; white fraction is not transparent!
			float tr = (1-alpha)*(1-whiteFraction);

			return Color.FromArgb(FtoInt(1-tr),
				FtoInt(red*colorFraction + whiteFraction),
				FtoInt(green*colorFraction + whiteFraction),
				FtoInt(blue*colorFraction + whiteFraction));
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Drawing.Color"/> of this <see cref="ChartColor"/> object.
		/// </summary>
		[SRCategory("CatAppearance")]
		[SRDescription("ChartColorColorDescr")]
		public Color Color 
		{ 
			get { return Color.FromArgb(FtoInt(alpha),FtoInt(red),FtoInt(green),FtoInt(blue)); } 
			set { alpha = value.A/255f; red = value.R/255f;  green = value.G/255f;  blue = value.B/255f; }
		}

		/// <summary>
		/// Gets the name of the <see cref="System.Drawing.Color"/>.
		/// </summary>
		[Browsable(false)]
		public string Name 
		{ 
			get 
			{
				return this.Color.Name;
			} 
		}

		internal virtual void Serialize(XmlCustomSerializer S)
		{
			S.AttributeProperty(this,"Reflection","Reflection","0.0##");
			S.AttributeProperty(this,"LogPhong");
			S.AttributeProperty(this,"Color");
		}
	}

	/// <summary>
	/// <see cref="ChartColorCollection"/> is a class that stores the <see cref="ChartColor"/> objects.
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(ColorArrayConverter))]
#if !__BUILDING_CRI__
	[Editor(typeof(CollectionWithTypeEditor), typeof(UITypeEditor))]
#endif
	public class ChartColorCollection : CollectionWithType, ICloneable
	{
		private bool hasChanged = false;
		/// <summary>
		/// Initializes a new instance of <see cref="ChartColorCollection"/> class with default parameters.
		/// </summary>
		public ChartColorCollection() : base(typeof(ChartColor))  { }

		/// <summary>
		/// Initializes a new instance of <see cref="ChartColorCollection"/> class with an array of colors to be containted in this collection.
		/// </summary>
		/// <param name="colors">Colors to be containted in this collection.</param>
		public ChartColorCollection(Color[] colors) : base(typeof(ChartColor)) 
		{
			for (int i=0; i<colors.Length; i++)
				Add(new ChartColor(colors[i]));
		}

		/// <summary>
		/// Gets a <see cref="ChartColorCollection"/> from string.
		/// </summary>
		/// <param name="s">string to be converted into a <see cref="ChartColorCollection"/> object.</param>
		/// <returns></returns>
		static public ChartColorCollection FromString(string s) 
		{
			ChartColorCollection ccc = (ChartColorCollection)(new ColorArrayConverter()).ConvertFrom(s);
			ccc.HasChanged = false;
			return ccc;
		}

		internal bool HasChanged { get { return hasChanged; } set { hasChanged = value; } }

		/// <summary>
		/// Creates an exact copy of this <see cref="ChartColorCollection"/> object.
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			ChartColorCollection copy = new ChartColorCollection();
			for (int i=0; i<this.Count; i++)
				copy.Add((ChartColor)(this[i].Clone()));
			return copy;
		}

		/// <summary>
		/// Indicates the <see cref="ChartColor"/> at the specified indexed location in the <see cref="ChartColorCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name to retrieve a <see cref="ChartColor"/> from the <see cref="ChartColorCollection"/> object.</param>
		public ChartColor this[ int index ]  
		{
			get  
			{
				return( (ChartColor) List[index] );
			}
			set  
			{
				if(index >= List.Count || List[index] != value)
				{
					hasChanged = true;
					List[index] = value;
				}
			}
		}


		/// <summary>
		/// Adds a specified object to this <see cref="ChartColorCollection"/> object.
		/// </summary>
		/// <param name="value"><see cref="ChartColor"/> or <see cref="System.Drawing.Color"/> object to be added to this <see cref="ChartColorCollection"/>.</param>
		/// <returns>The index of the newly added object or -1 if the the object could not be added.</returns>
		public override int Add( object value )  
		{
			ChartColor cc = null;
			if(value is Color)
			{
				cc = new ChartColor((Color)value);
			}
			else if (value is ChartColor)
				cc = (ChartColor)value;
			else
				return -1;
			
			hasChanged = true;
			return List.Add(cc);
		}

		internal bool Eq(ChartColorCollection c)
		{
			if(Count != c.Count)
				return false;
			for(int i=0;i<Count;i++)
			{
				ChartColor c1 = this[i];
				ChartColor c2 = c[i];
				if(!c1.Equals(c2))
					return false;
			}
			return true;
		}
	
	}

}
