using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Drawing;


namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class DataPointLabelStyleConverter : ExpandableWithBrowsingControlObjectConverter
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
				DataPointLabelStyle dpls = (DataPointLabelStyle)value;

				System.Reflection.ConstructorInfo ci = 
					typeof(DataPointLabelStyle).GetConstructor(new Type[] 
						{
							typeof(string), typeof(PieLabelPositionKind), typeof(string),
							typeof(double), typeof(double), typeof(double),
							typeof(double), typeof(double)
						}
					);
				return new InstanceDescriptor(ci, new object[] 
					{
						dpls.Name, dpls.PieLabelPosition, dpls.LineStyle, dpls.RelativeLine1Start,
						dpls.RelativeLine1Length, dpls.RelativeLine2Length, dpls.PixelsToLabel, 
						dpls.RelativeOffsetOfAlignedLabels
					}, false);

			}
			return base.ConvertTo(context, culture, value, destinationType);
		}	
		public new bool GetPropertiesSupported() { return true; }
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context,	object value, Attribute[] attributes)
		{
			return base.GetProperties(context,value,attributes);
		}
	}
}
