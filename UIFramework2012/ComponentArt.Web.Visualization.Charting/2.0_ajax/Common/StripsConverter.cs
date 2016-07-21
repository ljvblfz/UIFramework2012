using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class StripSetConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			if (destinationType == typeof(InstanceDescriptor)) 
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if (destinationType == typeof(InstanceDescriptor) && value is StripSet)
			{
				StripSet s = value as StripSet;
				//	using constructor
				System.Reflection.ConstructorInfo ci = typeof(StripSet).GetConstructor
					(new Type[] { typeof(string), typeof(double), typeof(Color), typeof(Color)	});
				return new InstanceDescriptor(ci, new Object[] { s.Name,s.Width,s.Color,s.AlternateColor }, false);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
