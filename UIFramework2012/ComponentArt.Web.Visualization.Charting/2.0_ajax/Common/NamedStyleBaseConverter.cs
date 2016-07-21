using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class NamedObjectBaseConverter : ExpandableObjectConverter 
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
				System.Reflection.ConstructorInfo ci;

				// Get Type
				Type type = value.GetType();

				// Get Property info
				string StringIndexPropertyName = "Name";
				PropertyInfo pi = type.GetProperty(StringIndexPropertyName);

				if (pi == null) 
				{
					throw new ArgumentException("Type " + type.ToString() + 
						" doesn't have property '" + StringIndexPropertyName + "'");
				}

				if (pi.PropertyType != typeof (string))
				{
					throw new ArgumentException("Property " + type.ToString() + "." + 
						StringIndexPropertyName + " must be of type string.");
				}

				// Get Constructor
				ci = type.GetConstructor(new Type[] {typeof(string)});

				// Get Property value
				string name = (string)pi.GetValue(value, null);

				// Return InstanceDescriptor
				return new InstanceDescriptor(ci, new object[] {name}, false);
				
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

}
