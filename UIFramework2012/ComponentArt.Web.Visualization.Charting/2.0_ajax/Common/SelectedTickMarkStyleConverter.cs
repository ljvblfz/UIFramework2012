using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Collections;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Converter for line DataPointLabelStyle name
	/// </summary>
	internal class SelectedTickMarkStyleConverter : SelectedNameConverter
	{
		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			return chart.TickMarkStyles;
		}

		public override Type GetReferencedType() {return typeof(TickMarkStyle);}

	}

}
