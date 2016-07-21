using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Collections;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	// FIXME: how is it different from SelectedDataDimensionConverter???

	/// <summary>
	/// Converter for line DataPointLabelStyle name
	/// </summary>
	internal class SelectedIndexDimensionConverter : SelectedNameConverter
	{
		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			return chart.DataDimensions;
		}
		public override Type GetReferencedType() {return typeof(DataDimension);}

	}

}
