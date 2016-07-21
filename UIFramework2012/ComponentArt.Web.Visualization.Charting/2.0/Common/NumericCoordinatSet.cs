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
	/// <summary>
	/// Represents a numeric coordinate set.
	/// </summary>

	[Editor(typeof(UITypeEditor), typeof(UITypeEditor))]
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	[Serializable]
	public class NumericCoordinateSet : CoordinateSet
	{
		private double	minValue;
		private double	maxValue;
		private double	step;
		private double	offset = 0;
		private int		numberOfItems = 5;
		private bool	isLog = false;
		private int		logBase = 10;

		#region --- Constructors ---
		/// <summary>
		/// Initializes a new instance of <see cref="NumericCoordinateSet"/> class with default parameters.
		/// </summary>
		public NumericCoordinateSet() : this (null) { }

		/// <summary>
		/// Initializes a new instance of <see cref="NumericCoordinateSet"/> class with a specified axis.
		/// </summary>
		/// <param name="axis">Axis of this numerical coordinate set belongs to.</param>
		public NumericCoordinateSet(Axis axis) : base(axis) { }

		/// <summary>
		/// Initializes a new instance of <see cref="NumericCoordinateSet"/> class with a specified axis and number of items.
		/// </summary>
		/// <param name="axis">Axis of this numerical coordinate set belongs to.</param>
		/// <param name="numberOfItems">Number of items in this numerical coordinate set.</param>
		public NumericCoordinateSet(Axis axis, int numberOfItems) : this(axis,numberOfItems,10) { isLog = false; }
		
		/// <summary>
		/// Initializes a new instance of <see cref="NumericCoordinateSet"/> class with a specified axis, number of items and log base.
		/// </summary>
		/// <param name="axis">Axis of this numerical coordinate set belongs to.</param>
		/// <param name="numberOfItems">Number of items in this numerical coordinate set.</param>
		/// <param name="logBase">log base of this </param>
		public NumericCoordinateSet(Axis axis, int numberOfItems, int logBase) : base(axis) 
		{
			this.isLog = true;
			this.logBase = logBase;
			this.numberOfItems = numberOfItems;
		}

		#endregion

		internal override void ComputeValueList()
		{			
			if(ValuesMethod == ValueKind.UserDefined)
				return;

			double min = Math.Min(Minimum,Maximum);
			double max = Math.Max(Minimum,Maximum);
			double[] values;
			if(isLog)
			{
				if(min <= 0)
					min = 1.0;
				values = Intervals.LogValues(min,max,logBase,numberOfItems);
			}
			else
				values = Intervals.FromStartEndStep(min,max,Step);
			
			valueList = new ArrayList();
			for(int i=0;i<values.Length; i++)
				valueList.Add(new Coordinate(Axis.Dimension,values[i]));

		}

		/// <summary>
		/// Gets or sets the minimum value of this <see cref="NumericCoordinateSet"/> object.
		/// </summary>
		public double Minimum
		{
			get
			{
				if(MinimumMethod == ValueKind.UserDefined)
				{
					return minValue;
				}
				else
				{
					if(Axis.IsLogarithmic)
						return Axis.MinValueLCS;
					else
					{
						double step = Step;
						int ix = (int)Math.Floor(Axis.MinValueLCS/step + 0.00001);
						return ix*step;
					}
				}
			}
			set
			{
				if(minValue != value)
					ValueChanged = true;
				MinimumMethod = ValueKind.UserDefined;
				minValue = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum value of this <see cref="NumericCoordinateSet"/> object.
		/// </summary>
		public double Maximum
		{
			get
			{
				if(MaximumMethod == ValueKind.UserDefined)
				{
					return maxValue;
				}
				else
				{
					if(Axis.IsLogarithmic)
						return Axis.MaxValueLCS;
					else
					{
						double step = Step;
						int ix = (int)Math.Floor(Axis.MaxValueLCS/step + 0.999999);
						return ix*step;
					}
				}
			}
			set
			{
				if(maxValue != value)
					ValueChanged = true;
				MaximumMethod = ValueKind.UserDefined;
				maxValue = value;
			}
		}

		/// <summary>
		/// Gets or sets the step of this <see cref="NumericCoordinateSet"/> object.
		/// </summary>
		public double Step
		{
			get
			{
				if(StepMethod == ValueKind.UserDefined)
					return step;
				else
				{
					double min, max;
					if(MinimumMethod == ValueKind.UserDefined)
						min = minValue;
					else
						min = Axis.MinValueLCS;
					if(MaximumMethod == ValueKind.UserDefined)
						max = maxValue;
					else
						max = Axis.MaxValueLCS;
					if(min>max) // may happen for reversed axis
					{
						double a = min;
						min = max;
						max = a;
					}

					AutoIntervals ai = new AutoIntervals(min/Axis.Scale,max/Axis.Scale,numberOfItems,false);
					return ai.Step*Axis.Scale;
				}
			}
			set
			{
				if(step != value)
					ValueChanged = true;
				StepMethod = ValueKind.UserDefined;
				step = value;
			}
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
			if(obj is double)
				ValueList.Add((double)obj);
			else if(obj is float)
				ValueList.Add((double)(float)obj);
			else if(obj is int)
				ValueList.Add((double)(int)obj);
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
		private bool ShouldSerializeStepValueKind()		{ return StepMethod == ValueKind.UserDefined; }

		private bool ShouldBrowseValues()		{ return ValuesMethod == ValueKind.UserDefined; }
		private bool ShouldBrowseMinimumMethod(){ return ValuesMethod != ValueKind.Auto; }
		private bool ShouldBrowseMinimum()		{ return ValuesMethod != ValueKind.Auto && MinimumMethod != ValueKind.Auto; }
		private bool ShouldBrowseMaximumMethod(){ return ValuesMethod != ValueKind.Auto; }
		private bool ShouldBrowseMaximum()		{ return ValuesMethod != ValueKind.Auto && MaximumMethod != ValueKind.Auto; }
		private bool ShouldBrowseStepMethod()	{ return ValuesMethod != ValueKind.Auto; }
		private bool ShouldBrowseStep()			{ return ValuesMethod != ValueKind.Auto && StepMethod != ValueKind.Auto; }
		private bool ShouldBrowseOffset()		{ return ValuesMethod != ValueKind.Auto; }
		private static string[] PropertiesOrder = new string[]
			{
				"ValuesMethod",
				"Values", 
				"MinimumMethod",
				"Minimum",
				"MaximumMethod",
				"Maximum",
				"StepMethod",
				"Step"
			};
		#endregion
	}
}
