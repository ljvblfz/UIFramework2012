using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Collections;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Base class for name selection converter
	/// </summary>
	internal abstract class SelectedNameConverter : StringConverter 
	{

		public virtual ArrayList GetNames(ChartBase chart) 
		{
			// Get the named collection
			NamedCollection coll = GetNamedCollection(chart);

			// Build a name array from the collection items
			ArrayList values = new ArrayList();			
			foreach (INamedObject namedObject in coll)
			{
				values.Add(namedObject.Name);
			}

			return values;

		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{ 
            if (context == null)
                return new StandardValuesCollection(new object[] { });

			ChartBase chart = ChartBase.GetChartFromObject(context.Instance);
			if(chart == null)
			{
				throw new Exception("SelectedNameConverter cannot find Chart object associated to '" + context.Instance.GetType().Name 
					+ "' context instance");
			}

			// Return the collection
			return new StandardValuesCollection(GetNames(chart));
		}

		protected abstract NamedCollection GetNamedCollection(ChartBase chart);

		internal NamedCollection GetCollection(ChartBase chart) 
		{
			return GetNamedCollection(chart);
		}
		
		// Required for ObjectModel Generator
		public abstract Type GetReferencedType();

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

        public static SelectedNameConverter GetSelectedNameConverter(PropertyInfo pi)
        {
            TypeConverterAttribute tc;

            tc = (TypeConverterAttribute)Attribute.GetCustomAttribute(pi, typeof(TypeConverterAttribute));

            if (tc == null)
                throw new ArgumentException("The property " + pi.Name + " does not have the TypeConverterAttribute");

            string tcs = tc.ConverterTypeName;
            Type t = Type.GetType(tcs.Substring(0, tcs.IndexOf(",")));

            if (!t.IsSubclassOf(typeof(SelectedNameConverter)))
                throw new ArgumentException("The property " + pi.Name + " is not a subclass of SelectedNameConverter");

            SelectedNameConverter snc = (SelectedNameConverter)t.GetConstructor(new Type[0]).Invoke(new object[0]);
            return snc;
        }

        public static ArrayList GetNames(ChartBase chart, PropertyInfo pi)
        {

            return GetSelectedNameConverter(pi).GetNames(chart);
        }
	}
}
