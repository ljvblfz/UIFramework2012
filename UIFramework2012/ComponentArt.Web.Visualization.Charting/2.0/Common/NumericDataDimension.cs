using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents a numeric data dimension.
	/// </summary>
	//===========================================================================================================

	public class NumericDataDimension : DataDimension
	{
		/// <summary>
		/// Initializes a new instance of <see cref="NumericDataDimension"/> class with a specific name.
		/// </summary>
		/// <param name="name">The name of created <see cref="NumericDataDimension"/> instance.</param>
		public NumericDataDimension(string name) : base(name,typeof(double)) 
		{
			ReferenceValue = 0.0;
		}
		/// <summary>
		/// Initializes a new instance of <see cref="NumericDataDimension"/> class with default parameters.
		/// </summary>
		public NumericDataDimension() : this("") {}

		/// <summary>
		/// Converts an object into a value in the Logical Coordinate System.
		/// </summary>
		/// <param name="obj">Object to convert.</param>
		/// <returns>Representation of the <paramref name="obj"/> in the Logical Coordinate System.</returns>
		/// <remarks>
		/// Converts <see cref="System.Double"/> and <see cref="System.Int32"/> to the same number.
		/// </remarks>
		public override double Coordinate(object obj)
		{
			double x = 0.0;
			if(obj.GetType() == typeof(double))
				x = (double)obj;
			else if(obj.GetType() == typeof(int))
				x = (int)obj;
			else return base.Coordinate(obj);
			return x;
		}

		/// <summary>
		/// Returns the dimension element at a given logical coordinate.
		/// </summary>
		/// <param name="logicalCoordinate">Logical coordinate of the dimension element.</param>
        /// <returns>An object representing the element at a given logical coordinate. 
        /// It is the coordinate value packaged in an object.</returns>
		public override object ElementAt(double coordinate)
		{
			return coordinate;
		}

        /// <summary>
        /// Returns the dimension element given a string representation of it.
        /// </summary>
        /// <param name="value">String representation of the dimension element.</param>
        /// <returns>Object representing the dimension element.</returns>
        public override object ValueOf(string value)
        {
            return Double.Parse(value, System.Globalization.CultureInfo.CurrentCulture);
        }

		internal override void GetExtremesAndPointSize(Variable v, ref object minDCS,ref object maxDCS, 
			ref double scatterPointSizeLCS, bool calculatePositiveScatteredPointSize)
		{
			if(v.Length <= 0)
				return;

			NumericVariable nv = v as NumericVariable;
			if(nv == null)
				throw new Exception("Implementation error: wrong variable type '" +
					v.GetType().Name + "' in NumericDataDimension");
			ArrayList vals = new ArrayList(nv.Length);
			for(int i=0; i<nv.Length; i++)
			{
				if(!nv.IsMissing[i])
					vals.Add(nv[i]);
			}

			double[] dv = (double[]) vals.ToArray(typeof(double));//new double[nv.Length];

			Array.Sort(dv);

			minDCS = dv[0];
			maxDCS = dv[dv.Length-1];

			if(calculatePositiveScatteredPointSize)
			{
				// We have to cover single-point scattered series by cheating the pointSizeLCS
				if(dv.Length == 1)
					scatterPointSizeLCS = 10;
				else
				{
					scatterPointSizeLCS = double.MaxValue;
					for(int i=1;i<dv.Length; i++)
						scatterPointSizeLCS = Math.Min(scatterPointSizeLCS,dv[i]-dv[i-1]);
				}
				minDCS = (double)minDCS - scatterPointSizeLCS/2;
				maxDCS = (double)maxDCS + scatterPointSizeLCS/2;
			}
			else
				scatterPointSizeLCS = 0;
		}

		internal override DataDimension Merge(DataDimension dim)
		{
			if(dim is NumericDataDimension)
				return this;
			else
				throw new Exception("Cannot combine 'NumericDataDimension' and '" + dim.GetType().Name +"'");
		}

		internal override int Compare(object coordinate1, object coordinate2)
		{
			if(!(coordinate1 is double) || !(coordinate2 is double))
				return -2;
			double i1 = (double) coordinate1;
			double i2 = (double) coordinate2;
			if(i1 < i2)
				return -1;
			else if(i1 > i2)
				return 1;
			else
				return 0;
		}

		internal override double SingleScatterPointSize { get { return 10; } }
		internal override DimensionSpan CreateSpan()
		{
			return new NumericDimensionSpan();
		}
	}

	public class NumericDimensionSpan : DimensionSpan
	{
		double step;

		public NumericDimensionSpan() : base()
		{
			step = 1;
		}

		public double Step { get { return step; } set { step = value; } }

	}
}
