using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Collections;
using System.Reflection;


namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Converter for gradient style name
	/// </summary>
	internal class SelectedGradientStyleConverter : SelectedNameConverter
	{
		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			return chart.GradientStyles;
		}

		public override Type GetReferencedType() {return typeof(GradientStyle);}
	}

}
