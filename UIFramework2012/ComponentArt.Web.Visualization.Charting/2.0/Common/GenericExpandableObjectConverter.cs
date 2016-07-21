using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Extends ExpandableWithBrowsingControlObjectConverter providing 
	/// conversion to "InstanceDescriptor" based on default constructor
	/// </summary>
	internal class GenericExpandableObjectConverter : ExpandableWithBrowsingControlObjectConverter
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
				System.Reflection.ConstructorInfo ci = value.GetType().GetConstructor(new Type[] { });
				return new InstanceDescriptor(ci, new Object[] { }, false);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
