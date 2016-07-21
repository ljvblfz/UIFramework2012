using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;


namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class LightConverter : ExpandableWithBrowsingControlObjectConverter 
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			if (destinationType == typeof(InstanceDescriptor) )
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if (destinationType == typeof(InstanceDescriptor) && value is Light)
			{
				Light light = value as Light;
				if(light.IsAmbient)
				{
					System.Reflection.ConstructorInfo ci = typeof(Light).GetConstructor(new Type[] { typeof(float) });
					return new InstanceDescriptor(ci, new Object[] { light.Intensity }, true);
				}
				else
				{
					System.Reflection.ConstructorInfo ci = typeof(Light).GetConstructor(new Type[] { typeof(float), typeof(Vector3D) });
					return new InstanceDescriptor(ci, new Object[] { light.Intensity,light.Direction }, true);
				}

			} 

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

}
