using System;
using System.ComponentModel;
using System.Globalization;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for DoubleWithAutoConverter.
	/// </summary>
	internal class DoubleWithAutoConverter : DoubleConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context,	Type sourceType	) 
		{
			return (sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context,Type destinationType) 
		{
			return (destinationType == typeof(string)) || base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(	ITypeDescriptorContext context,	CultureInfo culture, object value) 
		{
			if (value is string) 
			{
				string s = (string) value;
				string sl = s.ToLower();
				if(sl == "auto" || sl == "nan")
					return double.NaN;
			}

			return base.ConvertFrom(context,culture,value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,	Type destinationType) 
		{
			if (destinationType == typeof(string)) 
			{
				double val = (double)value;
				if(double.IsNaN(val))
					return "Auto";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

	}
}
