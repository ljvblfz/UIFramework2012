using System;

using System.Collections;
using ComponentArt.Web.Visualization.Charting.Geometry;


namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for SelectedLineStyleWithNoLineConverter.
	/// </summary>

	internal class SelectedLineStyleWithNoLineConverter : SelectedNameConverter
	{

		public override ArrayList GetNames(ChartBase chart) 
		{
			ArrayList al = base.GetNames(chart);
			al.Add("NoLine");
			return al;
		}

		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			return chart.LineStyles;
		}

		public override Type GetReferencedType() {return typeof(LineStyle);}
	}

}
