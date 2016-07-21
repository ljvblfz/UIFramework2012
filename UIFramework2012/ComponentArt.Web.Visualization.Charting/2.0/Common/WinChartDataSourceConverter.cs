using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Reflection;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for WinChartDataSourceConverter.
	/// </summary>
	internal class WinChartDataSourceConverter : TypeConverter
	{
		public override bool CanConvertFrom ( ITypeDescriptorContext context , Type destinationType )
		{
			if (destinationType == typeof(string)) 
			{
				return true;
			}
			return base.CanConvertFrom(context, destinationType);
		}
		
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			if (destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor)) 
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value )
		{
			if (value is string) 
			{
				string s = (string)value;

				if (s == "(null)") 
					return null;
				
				if (context == null) 
				{
					// This happens in the Web Control in runtime.
					return s;
				}

				foreach (IComponent comp in context.Container.Components) 
				{
					if (comp.Site.Name == s) 
					{
						return comp;
					}
				}
				throw new ArgumentException("Can not convert '" + s + "' to type DataSource");
			}

			return base.ConvertFrom(context, culture, value);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if (destinationType == typeof(string)) 
			{
				if (value == null)
					return "(null)";
				return ((IComponent)value).Site.Name;
			} 
			
			if  (destinationType == typeof(InstanceDescriptor)) 
			{
				System.Reflection.MemberInfo mi = typeof(string).GetMethod("Copy", new Type [] {typeof(string)});
				return new InstanceDescriptor(mi, new object[] {value});
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext
			context)
		{
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext
			context)
		{
			return true;
		}
		
		public override StandardValuesCollection GetStandardValues ( System.ComponentModel.ITypeDescriptorContext context ) 
		{ 			
			ArrayList svc = new ArrayList();
			svc.Add(null);
			// Geting all DataSet components
			PropertyDescriptor propDes = context.PropertyDescriptor;
			foreach (IComponent comp in context.Container.Components) 
			{
				if (comp is System.Data.DataSet) 
				{
					
					svc.Add(comp);
				}
				else
				{
					// Loop over component's public properties and check if they are IList
					PropertyInfo[] properties = comp.GetType().GetProperties();
					foreach(PropertyInfo property in properties)
					{
						if(!property.CanRead)
							continue;
						// Check if this property is an indexer
						Type pType = property.PropertyType;
						PropertyInfo propInfo = pType.GetProperty("Item",new Type[] {typeof(int)});
					}
				}
			}

			// Getting all IList data members
			
			return new StandardValuesCollection(svc);
		}
	}
}
