using System;
using System.CodeDom;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;



namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// 
	/// </summary>
	internal class SeriesBaseConverter : ExpandableWithBrowsingControlObjectConverter 
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
				if(value is Series)
				{
					// Trying CompositePresentation
					Series s = value as Series;
					// Using constructor 
					// Series(name)
					System.Reflection.ConstructorInfo ci = 
						typeof(Series).GetConstructor(new Type[]	{ typeof(string) });
					return new InstanceDescriptor(ci, new Object[] { s.Name },false);
				}
				else if(value is CompositeSeries)
				{
					// Trying CompositePresentation
					CompositeSeries s = value as CompositeSeries;
					// Using constructor 
					// Series(name)
					System.Reflection.ConstructorInfo ci = 
						typeof(CompositeSeries).GetConstructor(new Type[]	{ typeof(string) });
					return new InstanceDescriptor(ci, new Object[] { s.Name },false);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
