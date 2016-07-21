using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;

namespace ComponentArt.Web.Visualization.Gauges
{
	// =========================================================================================================

    /// <summary>
    /// Represents a positional color of a <see cref="MultiColor"/> object.
    /// </summary>
	[Serializable]
	[TypeConverter(typeof(ColorStopTypeConverter))]
	public class ColorStop
	{
		private double position;
		private Color color;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ColorStop"/> class.
		/// </summary>
		public ColorStop() : this(0, Color.Black) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorStop"/> class with the specified position and color.
        /// </summary>
        public ColorStop(double position, Color color)
		{
			this.position = position;
			this.color = color;
		}
		
		/// <summary>
		/// Gets or sets the position of the current object.
		/// </summary>
		[NotifyParentProperty(true)]
		public double Position { get { return position; } set { position = value; } }
		
		/// <summary>
		/// Gets or sets the color of the current object.
		/// </summary>
		[NotifyParentProperty(true)]
		public Color Color  { get { return color; } set { color = value; } }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() { return position.ToString("0.####") + "=" + ColorHex(Color); }

		internal static string ColorHex(Color c)
		{
			if(c.IsEmpty)
				return "Empty";
			else
				return "#" + c.A.ToString("X2") + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2") ;
		}
		internal static ColorStop FromString(string s)
		{
			string[] parts = s.Split('=');
			if(parts.Length != 2)
				throw new ArgumentException("'" + s + "' is not valid format of ColorStop data.");
			try
			{
				double position = double.Parse(parts[0],NumberFormatInfo.InvariantInfo);
				Color color;
				if(parts[1].ToLower() == "empty")
					color = Color.Empty;
				else
				{
					if(parts[1].StartsWith("#"))
					{
						byte a = Convert.ToByte(parts[1].Substring(1,2),16);
						byte r = Convert.ToByte(parts[1].Substring(3,2),16);
						byte g = Convert.ToByte(parts[1].Substring(5,2),16);
						byte b = Convert.ToByte(parts[1].Substring(7,2),16);
						color = Color.FromArgb(a,r,g,b);
					}
					else
					{
						KnownColor kc = (KnownColor)Enum.Parse(typeof(KnownColor), parts[1]);
						color = Color.FromKnownColor(kc);
					}
				}
				return new ColorStop(position,color);
			}
			catch
			{
				throw new ArgumentException("'" + s + "' is not valid format of ColorStop data.");
			}

		}
		
		internal ColorStop Interpolate(ColorStop cs, double fac)
		{
			double p = fac*cs.Position + (1-fac)*Position;
			int a = Math.Max(0,Math.Min(255,(int)(fac*cs.Color.A + (1-fac)*Color.A)));
			int r = Math.Max(0,Math.Min(255,(int)(fac*cs.Color.R + (1-fac)*Color.R)));
			int g = Math.Max(0,Math.Min(255,(int)(fac*cs.Color.G + (1-fac)*Color.G)));
			int b = Math.Max(0,Math.Min(255,(int)(fac*cs.Color.B + (1-fac)*Color.B)));
			return new ColorStop(p,Color.FromArgb(a,r,g,b));
		}
	}

	// =========================================================================================================

    /// <summary>
    /// Contains a collection of <see cref="ColorStop"/> objects.
    /// </summary>
	[Serializable]
	[TypeConverter(typeof(ColorStopCollectionTypeConverter))]
	public class ColorStopCollection : CollectionBase, IObjectModelNode
	{
		public ColorStopCollection() : base () { }
		public ColorStopCollection(string str) : base () 
		{
			ConvertFromString(str);
		}

		public int Add(ColorStop colorStop)
		{
			return InnerList.Add(colorStop);
		}
		public int Add(double position, Color color)
		{
			return Add(new ColorStop(position,color));
		}

		[NotifyParentProperty(true)]
		public ColorStop this[int index]
		{
			get 
			{ 
				if(index >= InnerList.Count)
					return null;
				return InnerList[index] as ColorStop; 
			}
			set { InnerList[index] = value; }
		}
	  
		#region --- IObjectModelNode Implementation ---

		private IObjectModelNode parent;

		IObjectModelNode IObjectModelNode.ParentNode
		{
			get { return parent; }
			set { parent = value; }
		}

		public override string ToString()
		{
			if(Count == 0)
				return "Empty";
			String s = "";
			for(int i=0; i < Count; i++)
			{
				if(i>0)
					s = s+",";
				s = s+this[i].ToString();
			}
			if(s=="0=Empty,1=Empty")
				s = "Empty";
			return s;
		}

		internal void ConvertFromString(string str)
		{
			Clear();
			if(str.ToLower() == "empty")
				str = "0=Empty,1=Empty";
			string[] parts = str.Split(',');
			for(int i=0; i<parts.Length; i++)
				Add(ColorStop.FromString(parts[i]));
		}

		#endregion
	}

	// =========================================================================================================

    /// <summary>
    /// Represents a color structure that contains multiple values.
    /// </summary>
	[TypeConverter(typeof(MultiColorTypeConverter))]
	[Serializable]
	public class MultiColor : IObjectModelNode
	{
		private bool solid;
		private ColorStopCollection colorStops = new ColorStopCollection();
		
		#region --- Constructors & conversions to/from string ---

		public MultiColor() { }

		public MultiColor(Color color)
		{
			colorStops.Add(0,color);
			colorStops.Add(1,color);
			(colorStops as IObjectModelNode).ParentNode = this;
		}

		public MultiColor(float[] values, Color[] colors, bool solid)
		{
			if(values.Length != colors.Length)
				throw new ArgumentException("Values and colors arrays in 'MultiColor' constructor not the same size");
			for(int i=0; i<values.Length; i++)
				colorStops.Add(values[i],colors[i]);
			this.solid = solid;
		}

		public override string ToString()
		{
			return "MultiColor";
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		
		public bool IsEmpty { get { return (colorStops.Count == 2 && colorStops[0].Color.IsEmpty && colorStops[1].Color.IsEmpty); } }

		#endregion

		/// <summary>
		/// Gets an empty multi-color object.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public static MultiColor Empty { get { return new MultiColor(Color.Empty); } }

        /// <summary>
        /// Gets or sets whether the colors are rendered as solid (true) or blended (false).
        /// </summary>
		[DefaultValue(false)]
		public bool Solid { get { return solid; } set { solid = value; } }
		
		/// <summary>
		/// Gets or sets a collection of <see cref="ColorStop"/> objects.
		/// </summary>
		[DefaultValue(typeof(ColorStopCollection),"Empty")]
		[Editor(typeof(ColorStopsEditor),typeof(System.Drawing.Design.UITypeEditor))]
		public ColorStopCollection ColorStops 
		{
			get { return colorStops; } 
			set
			{
				(value as IObjectModelNode).ParentNode = this;
				colorStops = value;
                ObjectModelBrowser.NotifyChanged(this);
			}
		}
		
		internal bool IsSolid { get { return (colorStops.Count==2 && colorStops[0].Color == colorStops[1].Color); } }

        /// <summary>
        /// Determines the color at a specified color-stop position.
        /// </summary>
        /// <param name="x">The color-stop position to calculate a color value for.</param>
        /// <returns>The color at the specified color-stop position.</returns>
		public Color ColorAt(float x)
		{
			int i;
			int n = colorStops.Count;
			for(i=0; i<n; i++)
				if(colorStops[i].Position >= x)
				{
					i--;
					break;
				}
			// Before the first value
			if(i<0)
				return colorStops[0].Color;
			if(i >= colorStops.Count-1)
				return colorStops[colorStops.Count-1].Color;

			if(solid)
				return colorStops[i].Color;
			else
			{
				double a = (x - colorStops[i].Position)/(colorStops[i+1].Position - colorStops[i].Position);
				a = Math.Max(0,Math.Min(1,a));
				Color c0 = colorStops[i].Color;
				Color c1 = colorStops[i+1].Color;
				return Color.FromArgb(
					(int)(a*c1.A + (1-a)*c0.A),
					(int)(a*c1.R + (1-a)*c0.R),
					(int)(a*c1.G + (1-a)*c0.G),
					(int)(a*c1.B + (1-a)*c0.B) );			
			}
		}
		
		/// <summary>
		/// Returns a <see cref="System.Drawing.ColorBlend"/> object.
		/// </summary>
        /// <returns>A <see cref="System.Drawing.ColorBlend"/> object.</returns>
		public ColorBlend GetColorBlend()
		{
			float[] v;
			Color[] c;
			GetColorsAndPositions(out c, out v);
			ColorBlend blend = new ColorBlend();
			blend.Positions = v;
			blend.Colors = c;
			return blend;
		}

		internal void GetColorsAndPositions(out Color[] colors, out float[] positions)
		{
			int n = colorStops.Count;
			if(n < 2)
			{
				colors = null;
				positions = null;
				return;
			}

			float[] v;
			Color[] c;
			float val0 = (float)colorStops[0].Position;
			float val1 = (float)colorStops[n-1].Position;
			if(solid)
			{
				v = new float[2*n-2];
				c = new Color[2*n-2];
				for(int i=0; i<n-1; i++)
				{
					v[2*i] = (float)((colorStops[i].Position-val0)/(val1-val0));
					c[2*i] = colorStops[i].Color;
					v[2*i+1] = (float)((colorStops[i+1].Position-val0)/(val1-val0));
					c[2*i+1] = colorStops[i].Color;
				}
			}
			else
			{
				v = new float[n];
				c = new Color[n];
				for(int i=0; i<n; i++)
				{
					v[i] = (float)((colorStops[i].Position-val0)/(val1-val0));
					c[i] = colorStops[i].Color;
				}
			}
			colors = c;
			positions = v;
		}

		public static implicit operator MultiColor(Color color)
		{
			return new MultiColor(color);
		}

		#region --- IObjectModelNode Implementation ---		
		private IObjectModelNode parent;
		IObjectModelNode IObjectModelNode.ParentNode
		{
			get { return parent; }
			set { parent = value; }
		}
		#endregion

	}

	#region --- Type Converters ---

	// ==============================================================================================================

	internal class ColorStopTypeConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			return destinationType == typeof(InstanceDescriptor) 
				|| destinationType == typeof(string) 
				|| base.CanConvertTo(context, destinationType);
		}
		
		public override System.Boolean CanConvertFrom ( ITypeDescriptorContext context , System.Type sourceType )
		{
			return sourceType == typeof(string)
				|| base.CanConvertFrom(context, sourceType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if(value is ColorStop)
			{
				ColorStop pc = value as ColorStop;
				if (destinationType == typeof(InstanceDescriptor))
				{
					System.Reflection.ConstructorInfo ci = value.GetType().GetConstructor
						(new Type[] {typeof(double),typeof(Color)});
					return new InstanceDescriptor(ci, new Object[] {pc.Position,pc.Color}, true);
				}
				if(destinationType == typeof(string))
					return pc.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override System.Object ConvertFrom ( System.ComponentModel.ITypeDescriptorContext context , System.Globalization.CultureInfo culture , System.Object value ) 
		{
			if (value is string) 
			{
				return ColorStop.FromString(value as string);
			}
			return base.ConvertFrom(context, culture, value);
		}
	}

	// ==============================================================================================================

	internal class ColorStopCollectionTypeConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			return destinationType == typeof(InstanceDescriptor) 
				|| destinationType == typeof(string) 
				|| base.CanConvertTo(context, destinationType);
		}
		
		public override System.Boolean CanConvertFrom ( ITypeDescriptorContext context , System.Type sourceType )
		{
			return sourceType == typeof(string)
				|| base.CanConvertFrom(context, sourceType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if(value is ColorStopCollection)
			{
				ColorStopCollection csc = value as ColorStopCollection;
				if (destinationType == typeof(InstanceDescriptor))
				{
					System.Reflection.ConstructorInfo ci = value.GetType().GetConstructor
						(new Type[] {typeof(string)});
					return new InstanceDescriptor(ci, new Object[] {csc.ToString()}, true);
				}
				if(destinationType == typeof(string))
					return csc.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override System.Object ConvertFrom ( System.ComponentModel.ITypeDescriptorContext context , System.Globalization.CultureInfo culture , System.Object value ) 
		{
			if (value is string) 
			{
				return new ColorStopCollection(value as string);
			}
			return base.ConvertFrom(context, culture, value);
		}
	}


	// ==============================================================================================================

	internal class MultiColorTypeConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			return destinationType == typeof(InstanceDescriptor) 
				|| destinationType == typeof(string) 
				|| base.CanConvertTo(context, destinationType);
		}
		
		public override System.Boolean CanConvertFrom ( ITypeDescriptorContext context , System.Type sourceType )
		{
			return base.CanConvertFrom(context, sourceType) 
								|| sourceType == typeof(string) ;

		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if(value is MultiColor)
			{
				MultiColor mc = value as MultiColor;
				if (destinationType == typeof(InstanceDescriptor))
				{
					System.Reflection.ConstructorInfo ci = value.GetType().GetConstructor
						(new Type[] {});
					return new InstanceDescriptor(ci, new Object[] {}, false);
				}
			else if (destinationType == typeof(string))
			{
				return mc.Solid.ToString() + "," + mc.ColorStops.ToString();
			}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override System.Object ConvertFrom ( System.ComponentModel.ITypeDescriptorContext context , System.Globalization.CultureInfo culture , System.Object value ) 
		{
			if(value is string)
			{
				string sValue = value as string;
				bool solid = (sValue.ToLower().StartsWith("true"));
				int ix = sValue.IndexOf(',');
				string mColor = sValue.Substring(ix+1);
				ColorStopCollection csc = new ColorStopCollection(mColor);
				MultiColor mc = new MultiColor();
				mc.Solid = solid;
				mc.ColorStops = csc;
				return mc;
			}
			return base.ConvertFrom(context, culture, value);
		}
	}

	// ==============================================================================================================

	internal class ColorStopsEditor : System.ComponentModel.Design.CollectionEditor
	{
		public ColorStopsEditor() : base(typeof(ColorStopCollection)) 
		{ }

		public override Object EditValue (ITypeDescriptorContext context, IServiceProvider provider, Object value)
		{
			IObjectModelNode node = value as IObjectModelNode;
			object obj = base.EditValue(context,provider,value);
			ObjectModelBrowser.NotifyChanged(node);
			return obj;
		}

	}
	#endregion
}
