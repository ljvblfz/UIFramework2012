using System;
using System.Collections;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Represents an enumerated coordinate set.
	/// </summary>
	public class EnumeratedCoordinateSet : CoordinateSet
	{
		private Coordinate minValue = null;
		private Coordinate maxValue = null;
		private int	 step = 1;
        private CoordinateSetComputation computation = null;

		#region --- Constructors ---

		/// <summary>
		/// Initializes a new instance of <see cref="EnumeratedCoordinateSet"/> class with default parameters.
		/// </summary>
		public EnumeratedCoordinateSet() : base (null) { }
		
		/// <summary>
		/// Initializes a new instance of <see cref="EnumeratedCoordinateSet"/> class with a specified axis and number of items.
		/// </summary>
		/// <param name="axis">Axis of this enumerated coordinate set belongs to.</param>
		/// <param name="numberOfItems">Number of items in this enumerated coordinate set.</param>
		public EnumeratedCoordinateSet(Axis axis, int numberOfItems) : base(axis) 
		{
            computation = new CoordinateSetComputation(axis);
            computation.PrimitiveNumberOfValues = numberOfItems;
            computation.ComputationKind = CoordinatesComputationKind.ByNumberOfPoints;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="EnumeratedCoordinateSet"/> class with a specified axis.
		/// </summary>
		/// <param name="axis">Axis of this enumerated coordinate set belongs to.</param>
		public EnumeratedCoordinateSet(Axis axis) : base(axis) { }

        internal EnumeratedCoordinateSet(CoordinateSetComputation computation)
            : base(computation.Axis)
        {
            this.computation = computation;
        }

		#endregion

		internal override void ComputeValueList()
		{
			if(valueList == null)
				valueList = new ArrayList();
			else
				valueList.Clear();

			EnumeratedDataDimension dim = Axis.Dimension as EnumeratedDataDimension;

			if(minValue == null)
				SetInitialUserDefinedMinimum();
			if(maxValue == null)
				SetInitialUserDefinedMaximum();

			int gen = dim.Root.GenerationOf(minValue);

			ArrayList list = null;

			// Bounds in logical coordinates
			double lowBound = dim.Coordinate(Axis.MinValue)-0.001;
			double highBound = dim.Coordinate(Axis.MaxValue)+dim.Width(Axis.MaxValue)+0.001;

			ArrayList list2 = dim.Root.ListLeaves();

			list2 = CleanOverlapingCoordinates(list2);
				
			// Clear unused members
			ArrayList list1 = new ArrayList(list2.Count);
			for(int i = 0; i<list2.Count; i++)
			{
				Coordinate item = (Coordinate)(list2[i]);
				if(item.Value != null)
				{
					double itemCoord = item.Offset;
					if(lowBound < itemCoord && itemCoord + item.Width < highBound)
						list1.Add(item);
				}
			}
            int step1 = this.step;
            if (computation != null)
            {
                if (computation.ComputationKind == CoordinatesComputationKind.ByStep)
                    step1 = Math.Max(1, (int)computation.Step);
                else
                    step1 = Math.Max(1, list1.Count / computation.PrimitiveNumberOfValues);
            }
			list = list1;
			
			for(int i=0;i<list.Count;i+=step1)
				valueList.Add(list[i]);

		}

		private ArrayList CleanOverlapingCoordinates(ArrayList list)
		{
			// Some coordinates overlap when the axis is in z-role and composition rule
			// at the owning composite series is not "Sections"
			
			if(Axis != Axis.CoordSystem.ZAxis)
				return list;

			EnumeratedDataDimension zDim = Axis.Dimension as EnumeratedDataDimension;
			CompositeSeries cSeries = Axis.CoordSystem.OwningSeries as CompositeSeries;
		
			if(cSeries == null)
				return list;

			ArrayList list2 = new ArrayList();
			for(int i=0; i<list.Count; i++)
			{
				Coordinate item = (Coordinate)(list[i]);
				string seriesName = (string)item.Value;
				SeriesBase ser = cSeries.FindCompositeSeries(seriesName);
				if(ser == null)
					ser = cSeries.FindSeries(seriesName);
				if(ser == null)
					continue; //throw new Exception("Series '" + seriesName + "' is not subseries of '" + cSeries.Name + "'");
				CompositeSeries oser = ser.OwningSeries;
				if(oser != null && 
					(oser.CompositionKind == CompositionKind.Merged ||
					oser.CompositionKind == CompositionKind.Stacked ||
					oser.CompositionKind == CompositionKind.Stacked100))
				{
					// Add the composite series name, instead of subseries
					list2.Add(item.Parent);
					// Remove the other members of the same composite series
					i++;
					for(;i<list.Count;i++)
					{
						seriesName = (string)((Coordinate)list[i]).Value;
						ser = cSeries.FindCompositeSeries(seriesName);
						if(ser == null)
							ser = cSeries.FindSeries(seriesName);
						if(ser == null)
							throw new Exception("Series '" + seriesName + "' is not subseries of '" + cSeries.Name + "'");
						if(ser.OwningSeries != oser)
						{
							i--;
							break;
						}
					}			
				}
				else
					list2.Add(item);
			}
			return list2;
		}

		internal override void SetInitialUserDefinedMinimum()
		{
			EnumeratedDataDimension dim = Axis.Dimension as EnumeratedDataDimension;
			minValue = FirstChildRecursive(dim.Root);
		}

		internal override void SetInitialUserDefinedMaximum()
		{
			EnumeratedDataDimension dim = Axis.Dimension as EnumeratedDataDimension;
			maxValue = LastChildRecursive(dim.Root);
		}

		internal override void SetInitialUserDefinedStep()
		{
			step = 1;
		}

		internal override void SetMember(int i, object obj)
		{
			if(obj.GetType() == Axis.Dimension.ItemType || obj.GetType().IsSubclassOf(Axis.Dimension.ItemType))
				ValueList.Add(obj);
			else
				throw new Exception("Cannot set member type '" + obj.GetType().Name + "' to '" + GetType().Name +"'");
		}

		#region --- Private helpers ---

		private Coordinate FirstChildRecursive(Coordinate item)
		{
			if(item.FirstChild != null)
				return FirstChildRecursive(item.FirstChild);
			else
				return item;
		}

		private Coordinate LastChildRecursive(Coordinate item)
		{
			if(item.FirstChild != null)
				return LastChildRecursive(item.LastChild);
			else
				return item;
		}
		#endregion
	}
}
