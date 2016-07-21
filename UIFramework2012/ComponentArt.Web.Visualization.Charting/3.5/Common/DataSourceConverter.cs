
using System;
using System.CodeDom;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting.Design
{

	internal class DataSourceConverter : ReferenceConverter
	{
		// Methods
		public DataSourceConverter() : base(typeof(IListSource))
		{
			this.listConverter = new ReferenceConverter(typeof(IList));
		}


		bool CanAddToList(object o) 
		{
			if (o == null)
			{
				return false;
			}
			ListBindableAttribute lba = (ListBindableAttribute) TypeDescriptor.GetAttributes(o)[typeof(ListBindableAttribute)];
			return (lba == null || lba.ListBindable);
		}


		void GetItemsFromCollection(TypeConverter.StandardValuesCollection collection, ArrayList al) 
		{
			foreach (object o in collection)
			{
				if (CanAddToList(o))
					al.Add(o);
			}
		}


		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			TypeConverter.StandardValuesCollection collection1 = base.GetStandardValues(context);
			TypeConverter.StandardValuesCollection collection2 = this.listConverter.GetStandardValues(context);
			ArrayList dropDownItems = new ArrayList();

			GetItemsFromCollection(collection1, dropDownItems);
			GetItemsFromCollection(collection2, dropDownItems);


			if ((context != null) && (context.Container != null))
			{
				foreach (IComponent component in context.Container.Components)
				{
					if (dropDownItems.IndexOf(component) == -1)
						if ((component is System.Data.OleDb.OleDbCommand) ||
							(component is System.Data.SqlClient.SqlCommand) ||
							(component is System.Data.OleDb.OleDbDataAdapter) ||
							(component is System.Data.SqlClient.SqlDataAdapter))
						{
							dropDownItems.Add(component);
						}
				}
			}


			dropDownItems.Add(null);
			return new TypeConverter.StandardValuesCollection(dropDownItems);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Fields
		private ReferenceConverter listConverter;
	}
	

	internal class WebDataSourceConverter : DataSourceConverter
	{

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return false;
		}
 

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			if (value.GetType() != typeof(string))
			{
				throw base.GetConvertFromException(value);
			}
			return (string) value;
		}


		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is string) 
			{
				return (string)value;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			TypeConverter.StandardValuesCollection collection = base.GetStandardValues(context);
			
			ArrayList dropDownItems = new ArrayList();

			foreach (object o in collection) 
			{
				if (o == null)
					dropDownItems.Add("");
				else 
				{
					ISite site = ((IComponent)o).Site;
					dropDownItems.Add(site != null ? site.Name : o.ToString());
				}
			}
			return new TypeConverter.StandardValuesCollection(dropDownItems);
		}
 	}
}