using System;
using System.Collections;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{

	// ==============================================================================================

	internal class DataDescriptorCollection : NamedCollection
	{
		internal DataDescriptorCollection() : base(typeof(DataDescriptor)) {  }
		internal DataDescriptorCollection(Object owner) : base(typeof(DataDescriptor), owner)
		{ }

		internal new DataDescriptor this[object obj] 
		{ 
			get { return (DataDescriptor)base[obj]; } 
			set 
			{ 
				
				base[obj] = value; 
			}
		}
		internal ChartBase OwningChart { get { return (Owner as ChartObject).OwningChart; } }
	}
}
