using System;
using System.CodeDom;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Extends behaviour of ExpandableObjectConverter providing dynamic properties selection as well as 
	/// custom properties ordering.
	/// </summary>
	internal class ExpandableWithBrowsingControlObjectConverter: System.ComponentModel.ExpandableObjectConverter 
	{
		public new bool GetPropertiesSupported() { return true; }
		public override PropertyDescriptorCollection GetProperties(
			ITypeDescriptorContext context,	object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = base.GetProperties(context,value,attributes);

			// count the properties
			int count = 0;
			foreach (PropertyDescriptor d in properties)
				if(ShouldBrowseProperty(value,d.Name))
					count ++;

			// create the property list
			PropertyDescriptor[] descriptors = new PropertyDescriptor[count];
			int i=0;
			foreach (PropertyDescriptor d in properties)
				if(ShouldBrowseProperty(value,d.Name))
					descriptors[i++] = d;

			PropertyDescriptorCollection result = new PropertyDescriptorCollection(descriptors);

			// sort properties, if possible
			Type T = value.GetType();
			FieldInfo field = T.GetField("PropertiesOrder",BindingFlags.Static | BindingFlags.NonPublic);
			if(field == null || field.FieldType != typeof(string[]))
				return result;
			else
				return result.Sort((string[])(field.GetValue(null)));
		}

		bool ShouldBrowseProperty(object obj, string propertyName)
		{
			Type T = obj.GetType();
			MethodInfo mInfo = T.GetMethod("ShouldBrowse"+propertyName,BindingFlags.NonPublic | BindingFlags.Instance);
			if(mInfo == null)
				return true;

			object result = mInfo.Invoke(obj,null);
			if(result != null && result is bool && !(bool)result)
				return false;
			else
				return true;
		}
	}	
}
