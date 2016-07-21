using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
    /// Represents a 2-dimensional point in the local coordinate system
	/// </summary>
	[TypeConverter(typeof(Point2DTypeConverter))]
	[Serializable]
	public class Point2D
	{
		private float x;
		private float y;

		public Point2D() : this(0,0) { }
		public Point2D(string coord)
		{
			try
			{
                string[] parts = coord.Replace("f", "").Split(',');
				x = float.Parse(parts[0],NumberFormatInfo.InvariantInfo);
				y = float.Parse(parts[1],NumberFormatInfo.InvariantInfo);
			}
			catch
			{
				throw new Exception("Cannot create Point2D from string '" + coord + "'");
			}
		}

		public Point2D(float xRel, float yRel)
		{
			x = xRel;
			y = yRel;
		}

		public float X { get { return x; } set { x = value; } }
		public float Y { get { return y; } set { y = value; } }
        public bool IsNull { get { return X == 0 && Y == 0; } }
        public static implicit operator PointF(Point2D p2d)
        {
            return new PointF(p2d.X, p2d.Y);
        }
		public static Point2D operator +(Point2D p2D, Size2D s2D)
		{
			return new Point2D(p2D.X + s2D.Width, p2D.Y + s2D.Height);
		}
		public static Point2D operator +(Size2D s2D, Point2D p2D)
		{
			return new Point2D(p2D.X + s2D.Width, p2D.Y + s2D.Height);
		}
		public static Point2D operator -(Point2D p2D, Size2D s2D)
		{
			return new Point2D(p2D.X - s2D.Width, p2D.Y - s2D.Height);
		}
		public static Point2D operator -(Size2D s2D, Point2D p2D)
		{
			return new Point2D(-p2D.X + s2D.Width, -p2D.Y + s2D.Height);
		}
		public static Size2D operator -(Point2D p2D1, Point2D p2D2)
		{
			return new Size2D(p2D1.X - p2D2.X, p2D1.Y - p2D2.Y);
		}

		#region --- Equality Implementation ---

		public static bool operator ==(Point2D x, Point2D y) 
		{
			if(((object)x) == null && ((object)y) == null)
				return true;
			if(((object)x) == null || ((object)y) == null)
				return false;
			return x.x == y.x && x.y == y.y;
		}
		public static bool operator !=(Point2D x, Point2D y) 
		{
			if(((object)x) == null && ((object)y) == null)
				return false;
			if(((object)x) == null || ((object)y) == null)
				return true;
			return x.x != y.x || x.y != y.y;
		}

		public override bool Equals(Object obj) 
		{
			if (obj == null || typeof(Point2D) != obj.GetType()) 
				return false;
      
			Point2D p = (Point2D)obj;
			return (x == p.x) && (y == p.y);
		}
		public override int GetHashCode() 
		{
			return x.GetHashCode() ^ y.GetHashCode();
		}

		#endregion

		public override string ToString()
		{
			return X.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + "," 
				+ Y.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
		}
	}
	/// <summary>
    /// Represents a relative size tuple within an absolute Rectangle or RectangleF
	/// </summary>
	[TypeConverter(typeof(Size2DTypeConverter))]
	[Serializable]
	public class Size2D
	{
		private float w;
		private float h;

		public Size2D() : this(0,0) { }
		public Size2D(float wRel, float hRel)
		{
			w = wRel;
			h = hRel;
		}
		public Size2D(string coord)
		{
			try
			{
                string[] parts = coord.Replace("f", "").Split(',');
				w = float.Parse(parts[0],NumberFormatInfo.InvariantInfo);
				h = float.Parse(parts[1],NumberFormatInfo.InvariantInfo);
			}
			catch
			{
				throw new Exception("Cannot create Size2D from string '" + coord + "'");
			}
		}

		public float Width { get { return w; } set { w = value; } }
		public float Height { get { return h; } set { h = value; } }
        public static implicit operator SizeF(Size2D s2d)
        {
            return new SizeF(s2d.Width, s2d.Height);
        }

		#region --- Operators ---

		public static Size2D operator -(Size2D s2D1, Size2D s2D2)
		{
			return new Size2D(s2D1.Width - s2D2.Width, s2D1.Height - s2D2.Height);
		}
		public static Size2D operator +(Size2D s2D1, Size2D s2D2)
		{
			return new Size2D(s2D1.Width + s2D2.Width, s2D1.Height + s2D2.Height);
		}
		public static Size2D operator *(Size2D s2D, double f)
		{
			return new Size2D((float)(s2D.Width * f), (float)(s2D.Height * f));
		}
		public static Size2D operator *(double f,Size2D s2D)
		{
			return new Size2D((float)(s2D.Width * f), (float)(s2D.Height * f));
		}
		public static Size2D operator /(Size2D s2dr, double f)
		{
			return new Size2D((float)(s2dr.Width / f), (float)(s2dr.Height / f));
		}
		#endregion
	
		#region --- Functions ---
		
		public static Size2D InDirection(double angleRadians)
		{
			return new Size2D((float)Math.Cos(angleRadians), (float)Math.Sin(angleRadians));
		}

		public double AngleRad() { return Math.Atan2(h,w); }

		public float Abs()
		{
			return (float) Math.Sqrt(w*w + h*h);
		}
		
		public Size2D Unit()
		{
			float d = Abs();
			if(d == 0)
				throw new Exception("Unit() of null " + this.GetType().Name + " is not defined");
			return new Size2D(w/d,h/d);
		}

		// Rotation 90 deg counterclockwise
		public Size2D Normal()
		{
			return new Size2D(-h,w);
		}

		// Angle in radians
		public float Angle()
		{
			return (float)Math.Atan2(h,w);
		}

		public Size2D Rotate(float angleRadians)
		{
			double c = Math.Cos(angleRadians);
			double s = Math.Sin(angleRadians);
			double x = w*c - h*s;
			double y = w*s + h*c;
			return new Size2D((float)x,(float)y);
		}
		#endregion

		#region --- Equality Implementation ---

		public static bool operator ==(Size2D x, Size2D y) 
		{
			if(((object)x) == null && ((object)y) == null)
				return true;
			if(((object)x) == null || ((object)y) == null)
				return false;
			return x.w == y.w && x.h == y.h;
		}
		public static bool operator !=(Size2D x, Size2D y) 
		{
			if(((object)x) == null && ((object)y) == null)
				return false;
			if(((object)x) == null || ((object)y) == null)
				return true;
			return x.w != y.w || x.h != y.h;
		}

		public override bool Equals(Object obj) 
		{
			if (obj == null || typeof(Size2D) != obj.GetType()) 
				return false;
      
			Size2D p = (Size2D)obj;
			return (w == p.w) && (h == p.h);
		}
		public override int GetHashCode() 
		{
			return w.GetHashCode() ^ h.GetHashCode();
		}

		#endregion
	
		public override string ToString()
		{
			return Width.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + "," + 
				Height.ToString(System.Globalization.NumberFormatInfo.InvariantInfo);
		}
	}
	
    /// <summary>
    /// Represents a 2-dimensional rectangle in the local coordinate system
    /// </summary>
	[TypeConverter(typeof(Rectangle2DTypeConverter))]
	[Serializable]
	public class Rectangle2D
    {
        private Point2D location = new Point2D();
        private Size2D size = new Size2D();
        public Rectangle2D(Point2D location, Size2D size)
		{
            this.location = location;
			this.size = size;
		}
		public Rectangle2D(string rectData)
		{
			try
			{
				string[] parts = rectData.Split(',');
                location = new Point2D(
					FloatParser.Parse(parts[0]),
					FloatParser.Parse(parts[1]));
				size = new Size2D(
					FloatParser.Parse(parts[2]),
					FloatParser.Parse(parts[3]));
			}
			catch(Exception ex)
			{
				throw new Exception("Cannot create Rectangle2D from string '" + rectData + "': " + ex.Message);
			}
		}
		public Rectangle2D(float xRel, float yRel, float wRel, float hRel)
            : this(new Point2D(xRel, yRel), new Size2D(wRel, hRel)) { }
        public Rectangle2D() : this(0,0,0,0) { }

        public Point2D Position { get { return location; } set { location = value; } }
        public Size2D Size { get { return size; } set { size = value; } }
        public float X { get { return location.X; } set { location.X = value; } }
        public float Y { get { return location.Y; } set { location.Y = value; } }
        public float Width { get { return size.Width; } set { size.Width = value; } }
        public float Height { get { return size.Height; } set { size.Height = value; } }
        public bool IsEmpty { get { return (Width == 0 && Height == 0); } }
        public static implicit operator RectangleF(Rectangle2D r2d)
        {
            return new RectangleF(r2d.X, r2d.Y, r2d.Width, r2d.Height);
        }

		public override string ToString()
		{
			return 
				X.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + "," +
				Y.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + "," +
				Width.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) + "," +
				Height.ToString(System.Globalization.NumberFormatInfo.InvariantInfo) ;
		}
    }

	internal class FloatParser
	{
		internal static float Parse(string s)
		{
            s = s.Replace("f", "");
			s = s.Trim();
			float sign;
			if(s[0] == '-')
			{
				sign = -1;
				s = s.Substring(1);
			}
			else
				sign = 1;
			return sign*float.Parse(s,System.Globalization.NumberFormatInfo.InvariantInfo);
		}
	}


	#region --- Type Converters ---

	internal class Point2DTypeConverter : ExpandableObjectConverter
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
			if(value is Point2D)
			{
				Point2D point = value as Point2D;
				if (destinationType == typeof(InstanceDescriptor))
				{
					System.Reflection.ConstructorInfo ci = value.GetType().GetConstructor
						(new Type[] {typeof(string)});
					return new InstanceDescriptor(ci, new Object[] {point.ToString()}, true);
				}
				if(destinationType == typeof(string))
					return point.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override System.Object ConvertFrom ( System.ComponentModel.ITypeDescriptorContext context , System.Globalization.CultureInfo culture , System.Object value ) 
		{
			if (value is string) 
			{
				return new Point2D(value as string);
			}
			return base.ConvertFrom(context, culture, value);
		}
	}

	internal class Size2DTypeConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			if (destinationType == typeof(InstanceDescriptor)) 
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override System.Boolean CanConvertFrom ( ITypeDescriptorContext context , System.Type sourceType )
		{
			return sourceType == typeof(string)
				|| base.CanConvertFrom(context, sourceType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if(value is Size2D)
			{
				Size2D size = value as Size2D;
				if (destinationType == typeof(InstanceDescriptor))
				{
					System.Reflection.ConstructorInfo ci = value.GetType().GetConstructor
						(new Type[] {typeof(string)});
					return new InstanceDescriptor(ci, new Object[] {size.ToString()}, true);
				}
				if(destinationType == typeof(string))
					return size.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
		
		public override System.Object ConvertFrom ( System.ComponentModel.ITypeDescriptorContext context , System.Globalization.CultureInfo culture , System.Object value ) 
		{
			if (value is string) 
			{
				return new Size2D(value as string);
			}
			return base.ConvertFrom(context, culture, value);
		}

	}

	internal class Rectangle2DTypeConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			return 
				destinationType == typeof(InstanceDescriptor) ||
				destinationType == typeof(string) ||
				base.CanConvertTo(context, destinationType);
		}
		public override System.Boolean CanConvertFrom ( ITypeDescriptorContext context , System.Type sourceType )
		{
			return sourceType == typeof(string)
				|| base.CanConvertFrom(context, sourceType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if(value is Rectangle2D)
			{
				Rectangle2D rect = value as Rectangle2D;
				if (destinationType == typeof(InstanceDescriptor))
				{
					System.Reflection.ConstructorInfo ci = value.GetType().GetConstructor
						(new Type[] {typeof(string)});
					return new InstanceDescriptor(ci, new Object[] {rect.ToString()}, false);
				}
				if (destinationType == typeof(string))
					return rect.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
		
		public override System.Object ConvertFrom ( System.ComponentModel.ITypeDescriptorContext context , System.Globalization.CultureInfo culture , System.Object value ) 
		{
			if (value is string) 
			{
				return new Rectangle2D(value as string);
			}
			return base.ConvertFrom(context, culture, value);
		}

	}

    internal interface ISizePositionRangeProvider
    {
        void GetRangesAndSteps(string propertyName,
            ref float x0, ref float x1, ref float stepx,
            ref float y0, ref float y1, ref float stepy);
    }
	#endregion
}
