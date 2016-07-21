using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents an index data dimension.
	/// </summary>
	public class IndexDataDimension : DataDimension
	{
		private int		 minIndex = 0;
		private int		 maxIndex = 0;

		/// <summary>
		/// Initializes a new instance of <see cref="IndexDataDimension"/> class with a specific name.
		/// </summary>
		/// <param name="name">The name of created <see cref="IndexDataDimension"/> instance.</param>
		public IndexDataDimension(string name) : base(name,typeof(int))	{ }
		/// <summary>
		/// Initializes a new instance of <see cref="IndexDataDimension"/> class with default parameters.
		/// </summary>
		public IndexDataDimension() : this("") { }

		/// <summary>
		/// Gets or sets the minimum index in this <see cref="IndexDataDimension"/> object.
		/// </summary>
		public virtual int MinIndex 
		{ 
			get { return minIndex; } 
			set
			{
				minIndex = value; 
				maxIndex = Math.Max(minIndex,maxIndex);
			} 
		}

		/// <summary>
		/// Gets or sets the maximum index in this <see cref="IndexDataDimension"/> object.
		/// </summary>
		public virtual int MaxIndex 
		{ 
			get { return maxIndex; } 
			set
			{
				maxIndex = value; 
				minIndex = Math.Min(minIndex,maxIndex);
			} 
		}
        
		/// <summary>
		/// Returns the dimension element given a string representation of it.
		/// </summary>
		/// <param name="value">String representation of the dimension element.</param>
		/// <returns>Object representing the dimension element.</returns>
		public override object ValueOf(string val)
		{
			return Convert.ChangeType(val,typeof(int));
		}

		/// <summary>
		/// Retrieves the value of an object in the Logical Coordinate System.
		/// </summary>
		/// <param name="obj">object whose coordinate is retrieved.</param>
		/// <returns>The value of an object in the Logical Coordinate System. This method will throw an exception if the object is not double or int.</returns>
		public override double Coordinate(object obj)
		{
			int x = 0;
			if(obj.GetType() == typeof(double))
				x = (int)(double)obj;
			else if(obj.GetType() == typeof(int))
				x = (int)obj;
			else
				throw new ArgumentException("Cannot compute coordinate of a(n) '" + obj.GetType().Name 
					+"' in dimension type '" + GetType().Name + "'");

			return FirstMemberCoordinate + x-minIndex;
		}
		public override double Width(object obj) { return 1.0; }

		/// <summary>
		/// Returns the dimension element at a given logical coordinate.
		/// </summary>
		/// <param name="logicalCoordinate">Logical coordinate of the dimension element.</param>
		/// <returns>Object representing the element at a given logical coordinate. 
		/// To get the int value, you have to cast the return value.</returns>
		public override object ElementAt(double coordinate)
		{
			return (int)(Math.Floor(coordinate));
		}

		internal override DataDimension Merge(DataDimension dim)
		{
			if(dim == this)
				return this;
			IndexDataDimension dim1 = dim as IndexDataDimension;
			if(dim1==null)
				throw new Exception("Cannot combine 'IndexDataDimension' and '" + dim.GetType().Name +"'");
			
			IndexDataDimension merged = new IndexDataDimension(Name+"+"+dim.Name);
			merged.MinIndex = Math.Min(MinIndex,dim1.MinIndex);
			merged.MaxIndex = Math.Max(MaxIndex,dim1.MaxIndex);
			return merged;
		}

		internal override int Compare(object coordinate1, object coordinate2)
		{
			if(!(coordinate1 is int) || !(coordinate2 is int))
				return -2;
			int i1 = (int) coordinate1;
			int i2 = (int) coordinate2;
			if(i1 < i2)
				return -1;
			else if(i1 > i2)
				return 1;
			else
				return 0;
		}

		internal virtual DimensionSpan CreateSpan()
		{
			return new IntDimensionSpan();
		}
	}
}
