using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class CoordinatePlaneConverter : ExpandableWithBrowsingControlObjectConverter
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
			if (destinationType == typeof(InstanceDescriptor) && value is CoordinatePlane)
			{
				CoordinatePlane cp = value as CoordinatePlane;
				//	using constructorl CoordinatePlane()
				System.Reflection.ConstructorInfo ci = typeof(CoordinatePlane).GetConstructor
					(new Type[] {});
				return new InstanceDescriptor(ci, new Object[] {}, false);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
