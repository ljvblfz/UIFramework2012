using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// 
	/// </summary>
	internal class CoordinateSystemConverter: ExpandableWithBrowsingControlObjectConverter
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
			if (destinationType == typeof(InstanceDescriptor) && value is CoordinateSystem)
			{
				CoordinateSystem cs = value as CoordinateSystem;
				System.Reflection.ConstructorInfo ci = typeof(CoordinateSystem).GetConstructor
					(new Type[] {});
				return new InstanceDescriptor(ci, new Object[] {}, false);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
