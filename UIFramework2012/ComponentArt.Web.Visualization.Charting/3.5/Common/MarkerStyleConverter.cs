using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class MarkerStyleConverter : ExpandableWithBrowsingControlObjectConverter
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
				MarkerStyle MS = value as MarkerStyle;
				if(MS == null)
					return null;
			
				// Get Constructor
				System.Reflection.ConstructorInfo ci = value.GetType().GetConstructor(new Type[] {typeof(string)});

				// Return InstanceDescriptor
				return new InstanceDescriptor(ci, new object[] {MS.Name}, false);
				
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
