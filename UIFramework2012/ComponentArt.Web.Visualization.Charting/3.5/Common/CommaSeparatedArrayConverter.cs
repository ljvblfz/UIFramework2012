using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Text;


namespace ComponentArt.Web.Visualization.Charting.Design 
{
	internal abstract class CommaSeparatedArrayConverter : ArrayConverter 
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			if (destinationType == typeof(string)) 
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if (destinationType == typeof(string)) 
			{
				System.Array array = (System.Array)value;
			
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i<array.Length; ++i)
				{
					sb.Append(((IList)array)[i].ToString() + ",");
				}
				if (sb.Length > 0)
					return sb.ToString(0, sb.Length-1);

				return "";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;

			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string) 
			{
				string strvalue = (string)value;
				try 
				{
					string [] values = strvalue.Split(new char [] {','});

					IList array = Array.CreateInstance(GetElementType(), values.Length);

					TypeConverter converter = TypeDescriptor.GetConverter(GetElementType());
					
					for (int i=0; i<values.Length; ++i) 
					{
						array[i] = converter.ConvertFrom(null, culture, values[i]);
					}
					return array;
					
				}
				catch
				{
				}
				throw new ArgumentException("Cannot convert");
			}
			return base.ConvertFrom(context, culture, value);
		}

		protected abstract Type GetElementType();
	}


	internal class CommaSeparatedIntArrayConverter : CommaSeparatedArrayConverter 
	{
		protected override Type GetElementType()
		{
			return typeof(int);
		}
	}


	internal class CommaSeparatedDoubleArrayConverter : CommaSeparatedArrayConverter 
	{
		protected override Type GetElementType()
		{
			return typeof(double);
		}
	}

	internal class CommaSeparatedDateTimeArrayConverter : CommaSeparatedArrayConverter 
	{
		protected override Type GetElementType()
		{
			return typeof(DateTime);
		}
	}
}
