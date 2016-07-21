using System;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
using System.Collections;
using System.Reflection;
using System.ComponentModel.Design.Serialization;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class ColorArrayConverter : TypeConverter
	{
		public override bool CanConvertFrom(
			ITypeDescriptorContext context,
			Type sourceType
			) 
		{

			if (sourceType == typeof(string)) 
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}


		public override bool CanConvertTo(
			ITypeDescriptorContext context,
			Type destinationType
			) 
		{
			if (destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor)) 
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(
			ITypeDescriptorContext context,
			CultureInfo culture,
			object value
			) 
		{
			if (value is string) 
			{
				string colorString = (string)value;
				string [] colors = colorString.Split(new char [] {','});
				ChartColorCollection cCol = new ChartColorCollection();
				foreach (string color in colors) 
				{
					string cs = color.Trim();
					if(cs != "")
					{
						Color c = ColorFromString(cs);
						cCol.Add(new ChartColor(c));
					}
				}
				return cCol;
			}

			return base.ConvertFrom(context,culture,value);
		}


		public override object ConvertTo(
			ITypeDescriptorContext context,
			CultureInfo culture,
			object value,
			Type destinationType
			) 
		{
			if (destinationType == typeof(string)) 
			{
				ChartColorCollection colorArr = (ChartColorCollection)value;
				System.Text.StringBuilder sb = new System.Text.StringBuilder();

				foreach (ChartColor c in colorArr) 
				{
					sb.Append(c.Color.Name + ",");
				}

				if (sb.Length > 0)
					return sb.ToString(0, sb.Length-1);
				return "";
			}
			else if (destinationType == typeof(InstanceDescriptor)) 
			{
				MethodInfo mi = typeof(ChartColorCollection).GetMethod("FromString");
				return new InstanceDescriptor(mi, new object[] {ConvertTo(value, typeof(string))});
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}


		public static Color ColorFromString( string str )
		{

			Color c;

			try 
			{
				bool isHex = true;
				if(str.Length != 6 && str.Length != 8)
					isHex = false;
				else
				{
					foreach(char ch in str)
					{
						if(char.IsDigit(ch))
							continue;
						if('a' <= ch && ch <= 'f')
							continue;
						if('A' <= ch && ch <= 'F')
							continue;
						isHex = false;
						break;
					}
				}
				if(!isHex)
				{
					KnownColor kc = (KnownColor)Enum.Parse(typeof(KnownColor), str);
					c = Color.FromKnownColor(kc);
					if (c.A >= 0 && c.A <= 255)
						return c;
					else
						throw new ArgumentException(str + " is not a valid color string.");
				}
			} 
			catch 
			{
			}
			
			string r, g, b, a;

			if (str.Length == 6) 
			{
				r = str.Substring(0,2);
				g = str.Substring(2,2);
				b = str.Substring(4,2);

				c = Color.FromArgb( 
					Convert.ToByte( r, 16 ), 
					Convert.ToByte( g, 16 ), 
					Convert.ToByte( b, 16 ) 
					);

				return c;
			}

			a = str.Substring(0,2);
			r = str.Substring(2,2);
			g = str.Substring(4,2);
			b = str.Substring(6,2);

			c = Color.FromArgb( 
				Convert.ToByte( a, 16 ), 
				Convert.ToByte( r, 16 ), 
				Convert.ToByte( g, 16 ), 
				Convert.ToByte( b, 16 ) 
				);

			return c;
		}

	}
}
