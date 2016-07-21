using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;
using System.Runtime.Serialization;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	// -------------------------------------------------------------------------------------------------------
	//	DateTime Coordinate Set
	// -------------------------------------------------------------------------------------------------------

	/// <summary>
	/// Units of DateTime.
	/// </summary>
	public enum DateTimeUnit
	{
		/// <summary>
		/// Units are Years.
		/// </summary>
		Year,
		/// <summary>
		/// Units are Months.
		/// </summary>
		Month,
		/// <summary>
		/// Units are Days.
		/// </summary>
		Day,
		/// <summary>
		/// Units are Hours.
		/// </summary>
		Hour,
		/// <summary>
		/// Units are Minutes
		/// </summary>
		Minute,
		/// <summary>
		/// Units are Seconds.
		/// </summary>
		Second
	}

	/// <summary>
	/// Represents a datetime coordinate set.
	/// </summary>
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	[Serializable]
	public class DateTimeCoordinateSet : CoordinateSet
	{
		private int				step;
		private DateTimeUnit	unit;		// The real step is expressed in this unit
		private double			offset = 0;
		private DateTime		minValue;
		private DateTime		maxValue;
		private int				numberOfItems = 5;

		/// <summary>
		/// Initializes a new instance of <see cref="DateTimeCoordinateSet"/> class with default parameters.
		/// </summary>
		public DateTimeCoordinateSet() { }
		
		/// <summary>
		/// Initializes a new instance of <see cref="DateTimeCoordinateSet"/> class with a specified axis.
		/// </summary>
		/// <param name="axis">Axis of this date-time coordinate set belongs to.</param>
		public DateTimeCoordinateSet(Axis axis) : this(axis,5) { }
		
		/// <summary>
		/// Initializes a new instance of <see cref="DateTimeCoordinateSet"/> class with a specified axis and number of items.
		/// </summary>
		/// <param name="axis">Axis of this date-time coordinate set belongs to.</param>
		/// <param name="numberOfItems">Number of items in this date-time coordinate set.</param>
		public DateTimeCoordinateSet(Axis axis, int numberOfItems)
		{
			if(!(axis.Dimension is DateTimeDataDimension))
				throw new ArgumentException("Cannot set '" + axis.GetType().Name + "' to a DateTimeCoordinateSet");
			this.Axis = axis; 
			this.numberOfItems = numberOfItems;
		}
	
		internal override void ComputeValueList()
		{
			if(ValuesMethod == ValueKind.UserDefined)
				return;

			if(StepMethod == ValueKind.UserDefined)
			{
				valueList = new ArrayList();
				DateTime t = Minimum;
				TimeSpan dt = TimeSpan.FromDays(0); // this will be overriden
				switch (StepUnit)
				{
					case DateTimeUnit.Second:
						dt = TimeSpan.FromSeconds(Step);
						break;
					case DateTimeUnit.Minute:
						dt = TimeSpan.FromMinutes(Step);
						break;
					case DateTimeUnit.Hour:
						dt = TimeSpan.FromHours(Step);
						break;
					case DateTimeUnit.Day:
						dt = TimeSpan.FromDays(Step);
						break;
				}
				while(Minimum <= t && t <= Maximum)
				{
					valueList.Add(new Coordinate(Axis.Dimension,t));
					t = t + dt;
				}
			}
			else
			{
				DateTime dtMin = Minimum;
				DateTime dtMax = Maximum;
				DateTimeAutoIntervals DTAI = new DateTimeAutoIntervals(dtMin,dtMax,numberOfItems,false);
				DateTime[] values = DTAI.Values;
				valueList = new ArrayList();
				for(int i=0;i<values.Length; i++)
					valueList.Add(new Coordinate(Axis.Dimension,values[i]));
			}
		}

		/// <summary>
		/// Gets or sets the minimum value of this <see cref="DateTimeCoordinateSet"/> object.
		/// </summary>
		public DateTime Minimum
		{
			get
			{
				if(MinimumMethod == ValueKind.UserDefined)
				{
					return minValue;
				}
				else
				{
					return (DateTime) Axis.MinValue;
				}
			}
			set
			{
				MinimumMethod = ValueKind.UserDefined;
				minValue = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum value of this <see cref="DateTimeCoordinateSet"/> object.
		/// </summary>
		public DateTime Maximum
		{
			get
			{
				if(MaximumMethod == ValueKind.UserDefined)
				{
					return maxValue;
				}
				else
				{
					return (DateTime) Axis.MaxValue;
				}
			}
			set
			{
				MaximumMethod = ValueKind.UserDefined;
				maxValue = value;
			}
		}

		/// <summary>
		/// Gets or sets the step of this <see cref="DateTimeCoordinateSet"/> object.
		/// </summary>
		public int Step
		{
			get
			{
				if(StepMethod == ValueKind.Auto)
				{
					AutoStep();
				}
				return step;
			}
			set
			{
				StepMethod = ValueKind.UserDefined;
				step = value;
			}
		}

		internal DateTimeUnit StepUnit
		{
			get
			{
				if(StepMethod == ValueKind.Auto)
				{
					AutoStep();
				}
				return unit;
			}
			set
			{
				StepMethod = ValueKind.UserDefined;
				unit = value;
			}
		}

		private void AutoStep()
		{
			DateTime dtMin = (DateTime) Axis.MinValue;
			DateTime dtMax = (DateTime) Axis.MaxValue;
			DateTimeAutoIntervals ai = new DateTimeAutoIntervals(dtMin,dtMax,numberOfItems,false);
			step = ai.Step;
			unit = ai.StepUnit;
		}

		internal override void SetInitialUserDefinedMinimum()
		{
			minValue = Minimum;
		}

		internal override void SetInitialUserDefinedMaximum()
		{
			maxValue = Maximum;
		}

		internal override void SetInitialUserDefinedStep()
		{
			step = Step;
		}

		internal override void SetMember(int i, object obj)
		{
			if(obj is DateTime)
				ValueList.Add((DateTime)obj);
			else
				throw new Exception("Cannot set member type '" + obj.GetType().Name + "' to '" + GetType().Name +"'");
		}

	
		#region --- Serialization ---
		private bool ShouldSerializeValues()			{ return ValuesMethod == ValueKind.UserDefined; }
		private bool ShouldSerializeValuesValueKind()	{ return ValuesMethod == ValueKind.UserDefined; }

		private bool ShouldSerializeMinValue()			{ return MinimumMethod == ValueKind.UserDefined; }
		private bool ShouldSerializeMinValueKind()		{ return MinimumMethod == ValueKind.UserDefined; }
		private bool ShouldSerializeMaxValue()			{ return MaximumMethod == ValueKind.UserDefined; }
		private bool ShouldSerializeMaxValueKind()		{ return MaximumMethod == ValueKind.UserDefined; }
		private bool ShouldSerializeStep()				{ return StepMethod == ValueKind.UserDefined; }
		private bool ShouldSerializeStepUnit()			{ return StepMethod == ValueKind.UserDefined; }
		private bool ShouldSerializeStepValueKind()		{ return StepMethod == ValueKind.UserDefined; }

		#endregion
	}
}
