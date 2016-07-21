using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Collections;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{

	/// <summary>
	/// Converter for style name
	/// </summary>
	internal class SelectedSeriesStyleConverter : SelectedNameConverter
	{
		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			return chart.SeriesStyles;
		}

		public override Type GetReferencedType() {return typeof(SeriesStyle);}

	}

}
