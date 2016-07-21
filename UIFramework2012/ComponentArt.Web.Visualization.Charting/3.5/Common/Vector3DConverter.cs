using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;


namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for Vector3DConverter.
	/// </summary>
	internal class Vector3DConverter : ExpandableObjectConverter 
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
			if (destinationType == typeof(InstanceDescriptor))
			{
				Vector3D v = (Vector3D)value;

				System.Reflection.ConstructorInfo ci = 
					typeof(Vector3D).GetConstructor(new Type[1] {typeof(string)});
				return new InstanceDescriptor(ci, new object[] {this.ConvertToInvariantString(v)}, true);

			}
			if ( destinationType == typeof(string)) 
			{
				return ((Vector3D)value).ToString();
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
					// Parse (x,y,z)

					int openParen = s.IndexOf('(');
					int closeParen = s.LastIndexOf(')');

					if (openParen != -1 && closeParen > openParen) 
					{
						int firstComma = s.IndexOf(',', openParen + 1);
						int secondComma = s.IndexOf(',',firstComma + 1);

						if (firstComma != -1 && secondComma != -1 && secondComma < closeParen) 
						{
							double x = Double.Parse(s.Substring(openParen+1, firstComma-openParen-1).Trim(), System.Globalization.NumberFormatInfo.InvariantInfo);
							double y = Double.Parse(s.Substring(firstComma+1, secondComma-firstComma-1).Trim(), System.Globalization.NumberFormatInfo.InvariantInfo);
							double z = Double.Parse(s.Substring(secondComma+1, closeParen-secondComma-1).Trim(), System.Globalization.NumberFormatInfo.InvariantInfo);
							Vector3D v = new Vector3D(x,y,z);
							return v;
						}
					}
				}
				catch 
				{
				}
				throw new ArgumentException("Can not convert '" + (string)value + "' to type Vector3D");
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}
