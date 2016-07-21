using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{

	// ===================================================================================================
	//		CoordinateSetConverter
	// ===================================================================================================
	internal class CoordinateSetConverter : ExpandableWithBrowsingControlObjectConverter
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			if (destinationType == typeof(string)) 
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			CoordinateSet cs = (CoordinateSet)value;

			try 
			{
				if (destinationType == typeof(string) ) 
				{
					string encodedCoordinateSet = value.GetType().ToString() + ";";
				
					PropertyInfo [] pis = cs.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
					foreach (PropertyInfo pi in pis)
					{
						// Take care of Item property
						if (pi.Name == "Item")
							continue;

						Attribute dsva 
							= Attribute.GetCustomAttribute(pi, typeof(DesignerSerializationVisibilityAttribute));

						if (dsva != null && ((DesignerSerializationVisibilityAttribute)dsva).Visibility == DesignerSerializationVisibility.Hidden)
							continue;

						MethodInfo mi = cs.GetType().GetMethod("ShouldSerialize" + pi.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
						if ( mi == null || (bool)mi.Invoke(cs, new object [0]))
						{
							Attribute tca = Attribute.GetCustomAttribute(pi, typeof(TypeConverterAttribute));

							object propertyValue;
							try 
							{
								propertyValue = pi.GetValue(cs, null);
							}
							catch 
							{
								throw;
							}

							string propertyValueString;
							if (tca == null)
								propertyValueString = propertyValue.ToString();
							else 
							{								
								TypeConverter myConverter = (TypeConverter)
									Activator.CreateInstance(Type.GetType(((TypeConverterAttribute)tca).ConverterTypeName)) ;
								propertyValueString = (string)myConverter.ConvertTo(propertyValue, typeof(string));
							}
							
							encodedCoordinateSet = encodedCoordinateSet
								+ " "+pi.Name+": " + propertyValueString + ";";
						}
					}

					return encodedCoordinateSet;

				}
			} 
			catch (Exception ex)
			{
#if !__BUILDING_CRI__
				System.Windows.Forms.MessageBox.Show(ex.StackTrace, ex.Message);
#endif
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}


		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;

			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string) 
			{
				string strvalue = (string)value;

				try 
				{
					string [] coordinateSetStringAttributes = strvalue.Split(new Char[] {';'});

					Type coordinateSetType = Type.GetType(coordinateSetStringAttributes[0]);
					ConstructorInfo ci = coordinateSetType.GetConstructor(new Type [0]);
					CoordinateSet cs = (CoordinateSet)ci.Invoke(new object [0]);
					
					for (int i=1; i<coordinateSetStringAttributes.Length; ++i) 
					{
						string [] propertyValuePair 
							= coordinateSetStringAttributes[i].Trim().Split(new char [] {':'});

						if (propertyValuePair.Length != 2)
							continue;

						PropertyInfo pi 
							= coordinateSetType.GetProperty(propertyValuePair[0].Trim(), BindingFlags.Public | BindingFlags.Instance);
						
						Attribute tca = Attribute.GetCustomAttribute(pi, typeof(TypeConverterAttribute));

						TypeConverter converter;
						if (tca == null) 
						{
							converter = TypeDescriptor.GetConverter(pi.PropertyType);
						} 
						else 
						{
							converter = (TypeConverter)
								Activator.CreateInstance(Type.GetType(((TypeConverterAttribute)tca).ConverterTypeName));
						}
						object val = converter.ConvertFrom(null, culture, propertyValuePair[1].Trim());
						pi.SetValue(cs, val, null);
					}

					return cs;
				} 
				catch
				{
					throw new ArgumentException("Could not convert string '"+ strvalue +"'to CoordinateSet");
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

	}
}
