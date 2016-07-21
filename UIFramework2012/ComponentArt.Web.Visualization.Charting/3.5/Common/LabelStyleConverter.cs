// Not Used

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Drawing;


namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class LabelStyleConverter : ExpandableObjectConverter
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
				LabelStyle ls = (LabelStyle)value;

				System.Reflection.ConstructorInfo ci = 
					typeof(LabelStyle).GetConstructor(new Type[] {typeof(string)/*, typeof(Chart)*/});

				return new InstanceDescriptor(ci, new object[] {ls.Name/*, ls.OwningChart*/}, false);

			}
			return base.ConvertTo(context, culture, value, destinationType);
		}	

	}
}
