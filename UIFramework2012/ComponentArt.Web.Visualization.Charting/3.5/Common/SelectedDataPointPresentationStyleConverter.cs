using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class SelectedDataPointPresentationStyleConverter : TypeConverter
	{

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) 
		{
			SeriesStyleCollection dppss = 
				((ChartObject)context.Instance).OwningChart.SeriesStyles;
			ArrayList values = new ArrayList();			
			foreach (SeriesStyle dpps in dppss)
			{
				values.Add(dpps.Name);
			}

			StandardValuesCollection svc = new StandardValuesCollection(values);
			return svc;
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
	}

}