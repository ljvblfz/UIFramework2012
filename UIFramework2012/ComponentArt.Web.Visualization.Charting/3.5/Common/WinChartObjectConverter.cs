
using System;
using System.CodeDom;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for WinChartObjectConverter.
	/// </summary>
	internal class WinChartObjectConverter : ExpandableWithBrowsingControlObjectConverter
	{
		public new bool GetPropertiesSupported() { return true; }
		public PropertyDescriptorCollection GetProperties(object value)
		{
			return GetProperties(null,value,null);
		}
		public override PropertyDescriptorCollection GetProperties(
				ITypeDescriptorContext context,	object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = base.GetProperties(context,value,attributes);
			WinChart chart = value as WinChart;

			if(chart == null)
				return properties;

#if __BuildingWebChart__ || __BUILDING_CRI_DESIGNER__
            return properties;
#else
			PropertyDescriptorCollection allowedProperties = new PropertyDescriptorCollection(new PropertyDescriptor[] { });

			string[] allowedPropertyNames = chart.RuntimeEditablePropertyNames;
			// This handles the list creation at control initialization
			if(allowedPropertyNames == null)
				return properties;
			int n = allowedPropertyNames.Length;

			foreach(PropertyDescriptor pd in properties)
			{
				for(int i=0; i<n; i++)
				{
					if(pd.Name == allowedPropertyNames[i])
					{
						allowedProperties.Add(pd);
						break;
					}
				}
			}
			return allowedProperties;
#endif
		}	
	}
}
