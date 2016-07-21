using System;
using System.CodeDom;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;


namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class SeriesCollectionConverter : ExpandableWithBrowsingControlObjectConverter 
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
			if (destinationType == typeof(InstanceDescriptor) && value is SeriesCollection)
			{
				// Using constructor 
				System.Reflection.ConstructorInfo ci = 
					typeof(SeriesCollection).GetConstructor(new Type[] { });
				return new InstanceDescriptor(ci, new Object[] { },false);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

}
