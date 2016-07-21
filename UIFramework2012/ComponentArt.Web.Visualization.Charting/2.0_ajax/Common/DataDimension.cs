using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	//===========================================================================================================
	
	/// <summary>
	/// Abstract base class for dimension classes in the multidimensional data space.
	/// </summary>
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	public abstract class DataDimension : NamedObjectBase
	{
		static internal NumericDataDimension		StandardNumericDimension = null;
		static internal DateTimeDataDimension		StandardDateTimeDimension = null;
		static internal IndexDataDimension		StandardIndexDimension = null;
		static internal EnumeratedDataDimension	StandardBooleanDimension = null;
		
		private bool	isDefault = false;
		private double	minValue = 0;
		private Type	itemType;

		private object  referenceValue = null;
		private bool	referenceVariableIsCustom = false;
		
		internal DataDimension(string name) : base(name) { }
		internal DataDimension(string name, Type itemType) : base(name) { this.itemType = itemType; }

		/// <summary>
		/// Gets the type of this <see cref="DataDimension"/> object.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Type ItemType { get { return itemType; } }
		/// <summary>
		/// Gets or sets the value of the first member in the Logical Coordinate System.
		/// </summary>
		public virtual double FirstMemberCoordinate {	get { return minValue; } set { minValue = value; } }
		/// <summary>
		/// Gets or sets the reference value of this dimension.
		/// </summary>
		/// <remarks>
		/// This is used to determine where the bar starts from, for example.
		/// </remarks>
		public virtual object ReferenceValue 
		{
			get { return referenceValue; }
			set
			{
				if(value == null || value.GetType() == itemType)
					referenceValue = value;
				else
				{
					TypeConverter TC = TypeDescriptor.GetConverter(value.GetType());
					if(TC.CanConvertTo(itemType))
						referenceValue = TC.ConvertTo(value,itemType);
				}
				referenceVariableIsCustom = true;
			}
		}

		internal bool ReferenceVariableIsCustom { get { return referenceVariableIsCustom; } set { referenceVariableIsCustom = value; } }
		/// <summary>
		/// Converts an object into a value in the Logical Coordinate System.
		/// </summary>
		/// <param name="obj">Object to convert.</param>
		/// <returns>Representation of the <paramref name="obj"/> in the Logical Coordinate System.</returns>
		public virtual double Coordinate(object obj)
		{
			throw new ArgumentException("Cannot compute coordinate of a(n) '" + obj.GetType().Name 
				+"' in dimension type '" + GetType().Name + "'");
		}
		/// <summary>
		/// Provides the width of the object in the Logical Coordinate System.
		/// </summary>
		/// <param name="obj">Object of interest.</param>
		/// <returns>The width of the <paramref name="obj"/> in the Logical Coordinate System.</returns>
		public virtual double Width(object obj) { return 0.0; }

		/// <summary>
		/// Returns the dimension element at the given logical coordinate.
		/// </summary>
		/// <param name="logicalCoordinate">Logical coordinate of the dimension element.</param>
		/// <returns>Object representing the dimension element at the given logical coordinate.</returns>
		public abstract object ElementAt(double logicalCoordinate);
        
        /// <summary>
        /// Returns the dimension element given a string representation of it.
        /// </summary>
        /// <param name="value">String representation of the dimension element.</param>
        /// <returns>Object representing the dimension element.</returns>
        public abstract object ValueOf(string value);
        
		internal object ConvertToRightType(object val)
		{
			try
			{
				if(val is string)
				{
					return ValueOf((string)val);
				}
				double c = Coordinate(val);
				return val;
			}
			catch
			{
				return null;
			}
		}
        internal bool IsDefault { get { return isDefault; } set { isDefault = value; } }
		internal bool ShouldSerializeMe { get { return !isDefault; } }

		internal virtual void GetExtremesAndPointSize(Variable v, ref object minDCS,ref object maxDCS, ref double scatterPointSizeLCS, bool dummyInBaseClass)
		{
			for(int i=0; i<v.Length;i++)
			{
				object obj = v.ItemAt(i);
				if(obj == null || v.MissingAt(i))
					continue;
				if(minDCS == null)
					minDCS = obj;
				else
				{
					if(Coordinate(minDCS) > Coordinate(obj))
						minDCS = obj;
				}
				if(maxDCS == null)
					maxDCS = obj;
				else
				{
					if(Coordinate(maxDCS)+Width(maxDCS) < Coordinate(obj) + Width(obj))
						maxDCS = obj;
				}
			}
			scatterPointSizeLCS = 0;
		}

		internal abstract DataDimension Merge(DataDimension dim);
		internal abstract int Compare(object coordinate1, object coordinate2);
		internal virtual void Rename(string oldName, string newName) { }
		internal virtual double SingleScatterPointSize { get { return 0; } }
		internal virtual DimensionSpan CreateSpan()
		{
			return new DimensionSpan();
		}
	}

	public class DimensionSpan 
	{
	}

}
