using System;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// A collection of <see cref="DataPoint"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class DataPointCollection : CollectionWithType 
	{
		internal DataPointCollection(Object owner) : base(typeof(DataPoint), owner)
		{ }
        
		internal DataPointCollection() : this(null) {}

		/// <summary>
		/// Indicates the <see cref="DataPoint"/> at the specified indexed location in the <see cref="DataPointCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based index to retrieve a <see cref="DataPoint"/> from the <see cref="DataPointCollection"/> object.</param>
		public new DataPoint this[object index]   
		{ 
			get { return ((DataPoint)base[index]); } 
			set { base[index] = value; } 
		}
	}
}
