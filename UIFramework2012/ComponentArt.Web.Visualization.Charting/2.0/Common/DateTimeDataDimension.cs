using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Defines DateTime data dimension on an axis.
	/// </summary>
	//===========================================================================================================

	public class DateTimeDataDimension : DataDimension
	{
		/// <summary>
		/// Default reference date.
		/// </summary>
		static public DateTime referenceDay = new DateTime(2000,1,1,0,0,0,0);
		DateTime	minDTValue = referenceDay, maxDTValue = referenceDay;

		/// <summary>
		/// Initializes a new instance of the <see cref="DateTimeDataDimension"/> class with specified name.
		/// </summary>
		/// <param name="name">name of the date time dimension.</param>
		public DateTimeDataDimension(string name) : base(name,typeof(DateTime))	{ }
		/// <summary>
		/// Initializes a new instance of the <see cref="DateTimeDataDimension"/> class with default parameters.
		/// </summary>
		public DateTimeDataDimension() : this("")	{ }

		public DateTime MinValue { get { return minDTValue; } set { minDTValue = value; if(ReferenceValue == null) ReferenceValue = minDTValue; } }
		public override double FirstMemberCoordinate 
		{ 
			get { return (minDTValue - referenceDay).TotalDays; } 
			set 
			{   
				minDTValue = referenceDay + TimeSpan.FromDays(value);
			} 
		}
		/// <summary>
		/// Converts an object into a value in the Logical Coordinate System.
		/// </summary>
		/// <param name="obj">Object to convert.</param>
		/// <returns>Representation of the <paramref name="obj"/> in the Logical Coordinate System.</returns>
		public override double Coordinate(object obj)
		{
			DateTime x = referenceDay;

			if(obj.GetType() == typeof(DateTime))
				x = (DateTime)obj;

			TimeSpan ts = x-referenceDay;
			double val = ts.TotalDays;
			return val;
		}

		/// <summary>
		/// Returns the DateTime of a given logical coordinate.
		/// </summary>
		/// <param name="coordinate">Logical coordinate of a dimension element.</param>
		/// <returns> An object representing the DateTime value at a given logical coordinate. 
		/// To get the DateTime value, cast the return value to a DateTime.</returns>
		public override object ElementAt(double coordinate)
		{
			return referenceDay.AddDays(coordinate);
		}

        /// <summary>
        /// Returns the DateTime object given a string representation of it.
        /// The string should be in the format yyyy-mm-dd 
        /// or the number of miliseconds since Jan 1, 1970.
        /// </summary>
        /// <param name="value">String representation of the DateTime.</param>
        /// <returns>DateTime representing the dimension element.</returns>
        public override object ValueOf(string value)
        {
            try
            {
                return new DateTime(Convert.ToInt64(value));
            }
            catch (Exception)
            {
                // if the value is not a ticks represenatation of the date
                return Convert.ToDateTime(value, System.Globalization.CultureInfo.CurrentCulture);
            }
        }

		internal override void GetExtremesAndPointSize(Variable v, ref object minDCS,ref object maxDCS, ref double scatterPointSizeLCS, bool calculatePositiveScatteredPointSize)
		{
			if(v.Length == 0)
				return;

			DateTimeVariable nv = v as DateTimeVariable;
			if(nv == null)
				throw new Exception("Implementation error: wrong variable type in DateTimeDataDimension");

			DateTime[] dv = new DateTime[nv.Length];
			for(int i=0; i<nv.Length; i++)
				dv[i] = (DateTime)nv.ItemAt(i);

			Array.Sort(dv);

			minDCS = dv[0];
			maxDCS = dv[dv.Length-1];

			if(calculatePositiveScatteredPointSize)
			{
				if(dv.Length == 1)
					scatterPointSizeLCS = 10;
				else
				{
					TimeSpan sp = (DateTime)maxDCS-(DateTime)minDCS;
					for(int i=1;i<dv.Length; i++)
					{
						TimeSpan spc = dv[i]-dv[i-1];
						if(spc < sp)
							sp = spc;
					}
					TimeSpan half = TimeSpan.FromDays(sp.TotalDays/2);
					minDCS = (DateTime)minDCS - half;
					maxDCS = (DateTime)maxDCS + half;

					scatterPointSizeLCS = Coordinate((DateTime)minDCS+sp) - Coordinate((DateTime)minDCS);
				}
			}
			else
				scatterPointSizeLCS = 0;
		}

		internal override DataDimension Merge(DataDimension dim)
		{
			DateTimeDataDimension dim1 = dim as DateTimeDataDimension;
			if(dim1 == null)
				throw new Exception("Cannot combine 'DateTimeDataDimension' and '" + dim.GetType().Name +"'");

			DateTimeDataDimension merged = new DateTimeDataDimension(Name+"+"+dim.Name);
			if(dim1.MinValue < MinValue)
				merged.MinValue = dim1.MinValue;
			else			
				merged.MinValue = MinValue;
			if(dim1.maxDTValue > maxDTValue)
				merged.maxDTValue = dim1.maxDTValue;
			else			
				merged.maxDTValue = maxDTValue;
			return merged;
		}

		internal override int Compare(object coordinate1, object coordinate2)
		{
			if(!(coordinate1 is DateTime) || !(coordinate2 is DateTime))
				return -2;
			DateTime dt1 = (DateTime) coordinate1;
			DateTime dt2 = (DateTime) coordinate2;
			int c = dt1.CompareTo(dt2);
			if(c<0)
				return -1;
			else if(c>0)
				return 1;
			else
				return 0;
		}

		internal override double SingleScatterPointSize { get { return 0.5; } }

		internal override DimensionSpan CreateSpan()
		{
			return new TimeDimensionSpan();
		}
	}

	public class TimeDimensionSpan : DimensionSpan
	{
		DateTimeUnit unit;

		public TimeDimensionSpan() : base()
		{
			unit = DateTimeUnit.Day;
		}

		public DateTimeUnit Unit { get { return unit; } set { unit = value; } }

	}

}
