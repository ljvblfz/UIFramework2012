using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Collections;
using System.Reflection;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Converter for line style name
	/// </summary>
	internal class SelectedLineStyleConverter : SelectedNameConverter
	{
		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			return chart.LineStyles;
		}
		public override Type GetReferencedType() {return typeof(LineStyle);}
	}

}
