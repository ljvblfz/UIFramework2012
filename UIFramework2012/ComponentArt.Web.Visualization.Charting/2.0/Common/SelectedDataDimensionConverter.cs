using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Collections;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{

	/// <summary>
	/// Converter for data dimension name
	/// </summary>
	internal class SelectedDataDimensionConverter : SelectedNameConverter
	{
		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			PropertyInfo pi = typeof(ChartBase).GetProperty("DataDimensions", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			return chart.DataDimensions;
		}

		public override Type GetReferencedType() {return typeof(DataDimension);}

	}

}
