
using System;
using System.CodeDom;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class AxisConverter: System.ComponentModel.ExpandableObjectConverter 
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
			if (destinationType == typeof(InstanceDescriptor) && value is Axis)
			{
				// Trying NumericAxis
				Axis na = value as Axis;
				// Using constructor 
				// NumericAxis(string name, double worldStartCoordinate,double worldEndCoordinate, AxisOrientation orientation) 
				System.Reflection.ConstructorInfo ci = typeof(Axis).GetConstructor(new Type[]
						{  });
				return new InstanceDescriptor(ci, 
					new Object[] {  },
					false);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
	internal class CoordinateSetComputationConverter: System.ComponentModel.ExpandableObjectConverter 
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			if (destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string)) 
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if(value is CoordinateSetComputation)
			{
				CoordinateSetComputation csc = value as CoordinateSetComputation;
				if (destinationType == typeof(InstanceDescriptor))
				{
					System.Reflection.ConstructorInfo ci = typeof(CoordinateSetComputation).GetConstructor(new Type[]
						{  });
					return new InstanceDescriptor(ci, 
						new Object[] {  },
						false);
				}
				else if (destinationType == typeof(string))
					return csc.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override System.Boolean CanConvertFrom ( ITypeDescriptorContext context , System.Type sourceType )
		{
			if (sourceType == typeof(string)) 
			{
				return true;
			}

			return base.CanConvertFrom(context, sourceType);
		}
		public override System.Object ConvertFrom ( System.ComponentModel.ITypeDescriptorContext context , System.Globalization.CultureInfo culture , System.Object value ) 
		{
			if (value is string) 
			{
				try 
				{
					string s = (string) value;
					string[] parts = s.Split(',');
					CoordinateSetComputation csc = new CoordinateSetComputation();
					csc.ComputationKind = (CoordinatesComputationKind)(new EnumConverter(typeof(CoordinatesComputationKind)).ConvertFromString(parts[0]));
					csc.NumberOfValues = int.Parse(parts[1]);
					csc.EffectiveStep = double.Parse(parts[2]);
					csc.Unit = (DateTimeUnit)(new EnumConverter(typeof(DateTimeUnit)).ConvertFromString(parts[3]));
					csc.AlwaysIncludeMinValue = bool.Parse(parts[4]);
					csc.AlwaysIncludeMaxValue = bool.Parse(parts[5]);
					return csc;
				}
				catch 
				{
				}
				throw new ArgumentException("Can not convert '" + (string)value + "' to type 'CoordinateSetComputation'");
			}
			return base.ConvertFrom(context, culture, value);
		}


	}


	internal class AxisAnnotationConverter: ExpandableWithBrowsingControlObjectConverter 
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
			if (destinationType == typeof(InstanceDescriptor) && value is AxisAnnotation)
			{
				System.Reflection.ConstructorInfo ci = typeof(AxisAnnotation).GetConstructor(new Type[0]);
				return new InstanceDescriptor(ci, new Object[0], false);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

}
