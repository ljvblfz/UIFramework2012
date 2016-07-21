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
	internal class SelectedLabelStyleConverter : SelectedNameConverter
	{
		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			return chart.LabelStyles;
		}

		public override Type GetReferencedType() {return typeof(LabelStyle);}

	}

}
