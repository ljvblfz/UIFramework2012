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
	internal class SelectedDataPointLabelStyleConverter : SelectedNameConverter
	{
		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			
			return chart.DataPointLabelStyles;
		}

		public override Type GetReferencedType() {return typeof(DataPointLabelStyle);}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext
			context)
		{
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext
			context)
		{
			return false;
		}

	}

}
