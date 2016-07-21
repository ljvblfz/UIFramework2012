using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents an indexed coordinate set.
	/// </summary>
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	[Serializable]
	public class IndexCoordinateSet : CoordinateSet
	{
		private int	minValue;
		private int	maxValue;
		private int		step;
		private int numberOfItems = 5;

		#region --- Constructors ---
		/// <summary>
		/// Initializes a new instance of <see cref="IndexCoordinateSet"/> class with default parameters.
		/// </summary>
		public IndexCoordinateSet() : this (null) { }
		
		/// <summary>
		/// Initializes a new instance of <see cref="IndexCoordinateSet"/> class with a specified axis.
		/// </summary>
		/// <param name="axis">Axis of this indexed coordinate set belongs to.</param>
		public IndexCoordinateSet(Axis axis) : base(axis) { }
		
		/// <summary>
		/// Initializes a new instance of <see cref="IndexCoordinateSet"/> class with a specified axis and number of items.
		/// </summary>
		/// <param name="axis">Axis of this indexed coordinate set belongs to.</param>
		/// <param name="numberOfItems">Number of items in this indexed coordinate set.</param>
		public IndexCoordinateSet(Axis axis, int numberOfItems) : base(axis) 
		{
			this.numberOfItems = numberOfItems;
		}

		#endregion

		internal override void ComputeValueList()
		{
			if(ValuesMethod == ValueKind.UserDefined)
				return;

			if(valueList == null)
				valueList = new ArrayList();
			else
				valueList.Clear();
			int min = Minimum;
			int max = Maximum; // Adding width
			int s;
			if(min == max)
				s = 1;
			else
				s = Step;
			valueList.Add(new Coordinate(Axis.Dimension,min));
			int k = (min/s)*s;
			while(k < max)
			{
				if(k>min)
					valueList.Add(new Coordinate(Axis.Dimension,k));
				k += s;
			}
			valueList.Add(new Coordinate(Axis.Dimension,max));
		}

#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[TypeConverter(typeof(CommaSeparatedIntArrayConverter))]
		internal int[] Values
		{
			get
			{
				int[] val = new int[ValueList.Count];
				for(int i = 0; i<valueList.Count; i++)
					val[i] = (int) valueList[i];
				return val;
			}
			set
			{
				valueList = new ArrayList(value);
				MaximumMethod = ValueKind.UserDefined;
			}
		}

		/// <summary>
		/// Gets or sets the minimum value of this <see cref="IndexCoordinateSet"/> object.
		/// </summary>
		[NotifyParentProperty(true)]
		public int Minimum
		{
			get
			{
				if(MinimumMethod == ValueKind.UserDefined)
				{
					return minValue;
				}
				else
				{
					if(Axis.MinValue is int)
						return (int)Axis.MinValue;
					else if(Axis.MinValue is double)
						return (int)Math.Floor((double)Axis.MinValue);
					else
						throw new Exception("MinValue on axis '" + Axis.Role.ToString() + "' is not numeric");
				}
			}
			set
			{
				if(minValue != value)
					ValueChanged = true;
				minValue = value;
				MinimumMethod = ValueKind.UserDefined;
			}
		}

		/// <summary>
		/// Gets or sets the maximum value of this <see cref="IndexCoordinateSet"/> object.
		/// </summary>
		[NotifyParentProperty(true)]
		public int Maximum
		{
			get
			{
				if(MaximumMethod == ValueKind.UserDefined)
				{
					return maxValue;
				}
				else
				{
					if(Axis.MaxValue is int)
						return (int)Axis.MaxValue;
					else if(Axis.MaxValue is double)
						return (int)Math.Ceiling((double)Axis.MaxValue);
					else
						throw new Exception("MaxValue on axis '" + Axis.Role.ToString() + "' is not numeric");
				}
			}
			set
			{
				if(maxValue != value)
					ValueChanged = true;
				maxValue = value;
				MaximumMethod = ValueKind.UserDefined;
			}
		}

		/// <summary>
		/// Gets or sets the step of this <see cref="IndexCoordinateSet"/> object.
		/// </summary>
		[NotifyParentProperty(true)]
		public int Step
		{
			get
			{
				if(StepMethod == ValueKind.UserDefined)
					return step;
				else
				{
					int min = Minimum, max=Maximum;

					AutoIntervals ai = new AutoIntervals((double)min,(double)max,numberOfItems,false);
					step = (int)(ai.Step);
					if(step == 0)
						step = 1;
					return step;
				}
			}
			set
			{
				if(step != value)
					ValueChanged = true;
				step = value;
				StepMethod = ValueKind.UserDefined;
			}
		}

		internal override void SetMember(int i, object obj)
		{
			if(obj is int)
				ValueList.Add((int)obj);
			else
				throw new Exception("Cannot set member type '" + obj.GetType().Name + "' to '" + GetType().Name +"'");
		}

		#region --- Serialization & Browsing Support ---

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
	
		private bool ShouldSerializeValues()			{ return ValuesMethod == ValueKind.UserDefined; }
		private bool ShouldSerializeValuesValueKind()	{ return ValuesMethod == ValueKind.UserDefined; }

		private bool ShouldSerializeMinimum()			{ return MinimumMethod == ValueKind.UserDefined; }
		private bool ShouldSerializeMinValueKind()		{ return MinimumMethod == ValueKind.UserDefined; }
		private bool ShouldSerializeMaximum()			{ return MaximumMethod == ValueKind.UserDefined; }
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
				"EffectiveStep"
			};
		#endregion
	}
}

