using ComponentArt.Web.Visualization.Charting.Design;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;


namespace ComponentArt.Web.Visualization.Charting
{
	// ============================================================================================
	//		InputVariable Converter Class
	// ============================================================================================

		internal class InputVariableConverter : ExpandableWithBrowsingControlObjectConverter 
		{
	
			public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
			{
				if (destinationType == typeof(InstanceDescriptor)) 
				{
					return true;
				}
				return base.CanConvertTo(context, destinationType);
			}
		

			public override bool CanConvertFrom ( ITypeDescriptorContext context , Type destinationType )
			{
				if (destinationType == typeof(InstanceDescriptor)) 
				{
					return true;
				}
				return base.CanConvertFrom(context, destinationType);
			}

		
			public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
			{
				if (destinationType == typeof(InstanceDescriptor)) 
				{
					InputVariable p = (InputVariable)value;
					System.Reflection.ConstructorInfo ci = 
						typeof(InputVariable).GetConstructor(new Type[] {typeof(string)});
					return new InstanceDescriptor(ci, new object[] {p.Name}, false);
				}

				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
	}
