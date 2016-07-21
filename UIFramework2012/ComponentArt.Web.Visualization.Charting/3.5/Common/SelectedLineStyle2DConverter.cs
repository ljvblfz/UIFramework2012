using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Collections;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{

	/// <summary>
	/// Converter for line style 2D name
	/// </summary>
	internal class SelectedLineStyle2DConverter : SelectedNameConverter
	{
		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			return chart.LineStyles2D;
		}
		public override Type GetReferencedType() {return typeof(LineStyle2D);}
	}

}
