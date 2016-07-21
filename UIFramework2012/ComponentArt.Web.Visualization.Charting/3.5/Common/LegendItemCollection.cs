using System;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class LegendItemCollection : CollectionWithType
	{
		public LegendItemCollection() : base (null) { }
		public LegendItemCollection(Object owner) : base (typeof(ILegendItemProvider),owner) { }
		public new ILegendItemProvider this[object index]
		{
			get { return (ILegendItemProvider)base[index]; }
			set { base[index] = value; }
		}
	}

}
