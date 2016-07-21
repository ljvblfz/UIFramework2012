using System;
using System.ComponentModel;
using System.Globalization;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	[TypeConverter(typeof(DataBindingPairConverter))]
	internal class DataBindingPair 
	{
		string m_variable="";
		string m_path="";

		private object m_owner;
		internal static readonly string m_noneString = "(none)";

		public DataBindingPair() 
		{
			m_path = m_noneString;
			m_variable = "";
		}

		[TypeConverter(typeof(DataBindingPairVariableNameConverter))]
		public string Variable 
		{
			get 
			{
				return m_variable;
			}
			set 
			{
				m_variable = value;
			}
		}

		[TypeConverter(typeof(DataSourcePathConverter))]
		public string Path 
		{
			get 
			{
				return m_path;
			}
			set 
			{
				m_path = value;
			}
		}


		internal void SetContext(object obj) 
		{
			m_owner = obj;
		}

		internal object Owner 
		{
			get {return m_owner;}
		}

	}
}

namespace ComponentArt.Web.Visualization.Charting.Design 
{

	internal class DataBindingPairConverter : GenericExpandableObjectConverter 
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
			if (destinationType == typeof(string))
			{
				DataBindingPair dbp = (DataBindingPair)value;
				return dbp.Variable + "<==>" + dbp.Path;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

	internal class DataSourcePathConverter : TypeConverter 
	{
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
#if __COMPILING_FOR_2_0_AND_ABOVE__
			return false;
#else
			return true;
#endif
        }

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}


		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			ChartBase chart;
            
            if (context == null)
            {
                return base.GetStandardValues(context);
            }

			if(context.Instance is ChartBase)
				chart = (ChartBase)context.Instance;
			else if(context.Instance is InputVariable)
				chart = ((InputVariable)(context.Instance)).OwningChart;
			else
				chart = ChartBase.GetChartFromObject(((DataBindingPair)(context.Instance)).Owner);

			if (!chart.SchemaExists) 
			{
				return base.GetStandardValues(context);
			}

			System.Collections.ArrayList al = new System.Collections.ArrayList(chart.DataSourcePaths);
			al.Add(DataBindingPair.m_noneString);
			return (new TypeConverter.StandardValuesCollection(al));
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value == null)
				return DataBindingPair.m_noneString;

			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string str = ((string) value).Trim();
                if (string.Equals(str, DataBindingPair.m_noneString))
					return null;
				return value;
			}

			return base.ConvertFrom(context, culture, value);
		}
	}

	internal class DataBindingPairVariableNameConverter : TypeConverter 
	{
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			ChartBase chart = ChartBase.GetChartFromObject(((DataBindingPair)(context.Instance)).Owner);
			return (new TypeConverter.StandardValuesCollection(chart.InputVariables.GetNames()));
		}
	}
}
