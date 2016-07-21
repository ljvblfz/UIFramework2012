using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Collections;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Converter for line style name
	/// </summary>
	internal class SelectedMarkerStyleConverter : SelectedNameConverter
	{
		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			return chart.MarkerStyles;
		}

		public override Type GetReferencedType() {return typeof(MarkerStyle);}
	}

}
