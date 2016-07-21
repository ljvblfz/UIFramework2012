using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;


namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class LegendConverter : ExpandableWithBrowsingControlObjectConverter
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
			if (destinationType == typeof(InstanceDescriptor)) 
			{
				System.Reflection.ConstructorInfo ci = 
					typeof(Legend).GetConstructor(new Type[0]);
				return new InstanceDescriptor(ci, null, false);
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
