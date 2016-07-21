using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Collections;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Converter for selected palette name
	/// </summary>
	internal class SelectedPaletteConverter : SelectedNameConverter
	{
		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			return chart.Palettes;
		}

		public override Type GetReferencedType() {return typeof(Palette);}

	}

}
