using System;
using System.Diagnostics;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using ComponentArt.Web.Visualization.Charting.Design;
#if __BuildingWebChart__
using System.Web.UI;

#endif

namespace ComponentArt.Web.Visualization.Charting 
{
    /// <summary>
    /// Describes the method of coordinate set computation.
    /// </summary>
    public enum CoordinatesComputationKind
    {
        /// <summary>
        /// The coordinate set is computed to have a step based on the specified minimum number of points within 
        /// the range of values.
        /// </summary>
        ByNumberOfPoints,
        /// <summary>
        /// The coordinate set is computed based on the specified step within 
        /// the range of values.
        /// </summary>
        ByStep
    }

    /// <summary>
    /// CoordinateSetComputation describes how to create coordinate sets used in axis annotation, coordinate 
    /// plane strips and coordinate plane grid lines.
    /// </summary>
    /// <remarks>
    /// The purpose of this object is to specify how coordinates will be computed at the time of data binding 
    /// (i.e. when the real data is available). 
    /// <para>The computation is based either on a specified step 
    /// when <see cref="ComputationKind"/> = <see cref="CoordinatesComputationKind.ByStep"/>, or 
    /// on the number of data points in the set when <see cref="ComputationKind"/> = 
    /// <see cref="CoordinatesComputationKind.ByNumberOfPoints"/>. 
    /// In the case where <see cref="ComputationKind"/> = <see cref="CoordinatesComputationKind.ByStep"/> values 
    /// of the coordinates are multiples of the <see cref="EffectiveStep"/> property. The number of points depends on 
    /// the range of values and the step.
    /// In the case where <see cref="ComputationKind"/> = <see cref="CoordinatesComputationKind.ByNumberOfPoints"/>, 
    /// the step between values is computed from the range of values and the number 
    /// of points specified in the propery <see cref="NumberOfValues"/>. Note that since the value of step is 
    /// is rounded off to the nearest round number, the number of points can be greater than specified in <see cref="NumberOfValues"/>. 
    /// </para>
    /// <para>
    /// The CoordinateSetComputation object always has an associated <see cref="Axis"/> object which is used to determine 
    /// the type of coordinates and the range of values if the minimum or maximum values are not explicitelly specified.
    /// </para> 
    /// </remarks>
    [TypeConverterAttribute(typeof(CoordinateSetComputationConverter))]
    [Serializable]
    public class CoordinateSetComputation : ICloneable
    {
        private Axis axis = null;
        private CoordinatesComputationKind kind = CoordinatesComputationKind.ByNumberOfPoints;
        private double step = 1;
        private DateTimeUnit unit = DateTimeUnit.Day;

		private int numberOfValues = 5;
		private bool numberOfValuesExplicit = false;
		private object minValue = null;
        private object maxValue = null;
        private bool minValueSet = false;
        private bool maxValueSet = false;
        private bool alwaysIncludeMinValue = true;
        private bool alwaysIncludeMaxValue = true;

        private CoordinateSet cSet = null;
        private ArrayList list = null;
        private bool dirty = true;
        private bool hasChanged = false;

		private bool roundValueRange = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSetComputation"/> class without specifying 
        /// the associated axis.
        /// </summary>
        /// <remarks>
        /// This constructor is used in serialization. If you use this constructor in your code, make sure
        /// that <see cref="CoordinateSetComputation.Axis"/> property is set. We recomend using the other
        /// constructor.
        /// </remarks>
        public CoordinateSetComputation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateSetComputation"/> class and sets 
        /// the associated axis.
        /// </summary>
        /// <param name="axis">The axis associated to this <see cref="CoordinateSetComputation"/>object.</param>
        public CoordinateSetComputation(Axis axis)
        {
            this.axis = axis;
        }
		/// <summary>
		/// Converts this object to string.
		/// </summary>

		public override string ToString()
		{
			return  kind.ToString() + "," + numberOfValues + "," + step + "," + unit + "," + AlwaysIncludeMinValue + "," + alwaysIncludeMaxValue;
		}

        internal CoordinateSetComputation(CoordinateSetComputation org)
        {
            this.axis = org.Axis;
            this.kind = org.kind;
            this.step = org.step;
            this.unit = org.unit;
            this.numberOfValues = org.numberOfValues;
            this.minValue = org.minValue;
            this.maxValue = org.maxValue;
            this.minValueSet = org.minValueSet;
            this.maxValueSet = org.maxValueSet;
            this.alwaysIncludeMinValue = org.alwaysIncludeMinValue;
            this.alwaysIncludeMaxValue = org.alwaysIncludeMaxValue;
			this.roundValueRange = org.roundValueRange;

            this.SetDirty(true);
            this.hasChanged = org.HasChanged;
        }

        /// <summary>
        /// Implements <see cref="ICloneable"/> interface.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new CoordinateSetComputation(this);
        }

        /// <summary>
        /// Sets or gets the minimum value for this <see cref="CoordinateSetComputation"/> object. If this 
        /// value is not set, the computation is based on the minimum value on the associated axis.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object MinValue
        {
            get { Evaluate(); return minValue; }
            set { minValue = value; minValueSet = true; dirty = true; }
        }

        /// <summary>
        /// Sets or gets the maximum value for this <see cref="CoordinateSetComputation"/> object. If this 
        /// value is not set, the computation is based on the maximum value on the associated axis.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object MaxValue
        {
            get { Evaluate(); return maxValue; }
            set { maxValue = value; maxValueSet = true; dirty = true; }
        }

        /// <summary>
        /// Sets or gets the associated <see cref="Axis"/> object. 
        /// </summary>
        /// <remarks>
        /// A <see cref="CoordinateSetComputation"/> object always has an axis associated to it. The axis object
        /// is used to determine the type (double, DateTime etc.) of coordinates and to determine value range if 
        /// <see cref="CoordinateSetComputation.MinValue"/> and/or <see cref="CoordinateSetComputation.MaxValue"/>
        /// are not set.
        /// </remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Axis Axis { get { return axis; } set { axis = value; dirty = true; } }
        internal double EffectiveStep
        {
            get
            {
                Evaluate();
                return step;
            }
            set
            {
                step = value; SetDirty(true);
            }
        }

        /// <summary>
        /// Sets or gets the minimum number of points.
        /// </summary>
        /// <remarks>
        /// This value is used if the <see cref="ComputationKind"/> is <see cref="CoordinateComputationKind.ByNumberOfPoints"/>,
        /// otherwise it is ignored. The number of computed points is greater or equal to this numnber. The
        /// number of points can be greater than this value because the algorithm creates some round values. 
        /// </remarks>
        [DefaultValue(5)]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		public int NumberOfValues 
		{
			get 
			{ 
				return numberOfValues; 
			} 
			set 
			{ 
				numberOfValues = value;
				if(Axis != null && Axis.OwningChart!=null && !Axis.OwningChart.InSerialization)
					kind = CoordinatesComputationKind.ByNumberOfPoints;
				numberOfValuesExplicit = true;
				SetDirty(true); 
			} 
		}
																	   
		/// <summary>
		/// Sets or gets the unit of distance between coordinates in case of 'DateTime' coordinates.
		/// </summary>
		/// <remarks>
		/// This value is used if the <see cref="ComputationKind"/> is <see cref="CoordinateComputationKind.ByStep"/>
		/// when the coordinate values are 'DateTime',
		/// otherwise it is ignored. Note that in case of 'DateTime' type of coordinates the distance between points
		/// is defined by this property annd the property <see cref="EffectiveStep"/>.
		/// </remarks>
		[DefaultValue(DateTimeUnit.Day)]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		public DateTimeUnit Unit 
		{
			get { try { Evaluate(); } catch{ }; return unit; } 
			set { unit = value; SetDirty(true); } 
		}

        /// <summary>
        /// Sets or gets the minimum value inclusion mode.
        /// </summary>
        /// <remarks>
        /// If this value is set to 'true', minimum value is always included in the coordinate set. If it is set to 'false', the minimum
        /// value is included only if it is multiple of the step. 
        /// </remarks>
        [DefaultValue(true)]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		public bool AlwaysIncludeMinValue { get { return alwaysIncludeMinValue; } set { if (alwaysIncludeMinValue != value) { alwaysIncludeMinValue = value; SetDirty(true); } } }

        /// <summary>
        /// Sets or gets the maximum value inclusion mode.
        /// </summary>
        /// <remarks>
        /// If this value is set to 'true', maximum value is always included in the coordinate set. If it is set to 'false', the maximum
        /// value is included only if it is multiple of the step. 
        /// </remarks>
        [DefaultValue(true)]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		public bool AlwaysIncludeMaxValue { get { return alwaysIncludeMaxValue; } set { if (alwaysIncludeMaxValue != value) { alwaysIncludeMaxValue = value; SetDirty(true); } } }

        /// <summary>
        /// Sets or gets the method of computation of coordinates.
        /// </summary>
        /// <remarks>
        /// The range of values is determined at the beginning of the coordinate set computation. 
        /// <see cref="MinValue"/> and <see cref="MaxValue"/> determine this range, if any of these
        /// are not set, the corresponding value from the axis is taken.
        /// <para>
        /// If <see cref="ComputationKind"/> = <see cref="CoordinatesComputationKind.ByStep"/> the resulting 
        /// values are multiples of the <see cref="EffectiveStep"/> property. The number of points depends on the 
        /// range of values and the step.
        /// </para>
        /// <para>
        /// If <see cref="ComputationKind"/> = <see cref="CoordinatesComputationKind.ByNumberOfValues"/> 
        /// the range of values and property <see cref="NumberOfValues"/> are used to select the step 
        /// from a set of round values. The selected step is the largest one that produces at least
        /// <see cref="NumberOfValues"/> points. Consequently, the resulting number of points can be greater
        /// than <see cref="NumberOfValues"/>.
        /// </para>
        /// </remarks>

        [DefaultValue(CoordinatesComputationKind.ByNumberOfPoints)]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		public CoordinatesComputationKind ComputationKind { get { return kind; } set { if (kind != value) { kind = value; SetDirty(true); } } }

        internal bool IsDirty { get { return dirty; } }
        internal bool HasChanged { get { return hasChanged; } }

        /// <summary>
        /// Sets or gets the distance between coordinates.
        /// </summary>
        /// <remarks>
        /// This value is used if the <see cref="ComputationKind"/> is <see cref="CoordinateComputationKind.ByStep"/>,
        /// otherwise it is ignored. Note that in case of 'DateTime' type of coordinates the distance between points
        /// is defined by this property annd the property <see cref="Unit"/>.
        /// </remarks>
        [NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		[DefaultValue(1.0)]
        public double Step { get { return step; } set { step = value; SetDirty(true); } }
        internal int PrimitiveNumberOfValues { get { return numberOfValues; } set { numberOfValues = value; SetDirty(true); } }
        internal DateTimeUnit PrimitiveUnit { get { return unit; } set { unit = value; SetDirty(true); } }

        internal void SetDirty(bool dirty)
        {
            this.dirty = dirty;
            hasChanged = true;
        }

        #region --- Computing step ---
        internal double GetStep()
        {
            if (kind == CoordinatesComputationKind.ByStep)
                return step;
            if (axis.Dimension is NumericDataDimension) return GetNumericStep();
            else if (axis.Dimension is DateTimeDataDimension) return 0;
            else if (axis.Dimension is IndexDataDimension) return GetIndexStep();
            else if (axis.Dimension is EnumeratedDataDimension) return GetEnumStep();
            else
                throw new Exception("Dimension type ' " + axis.Dimension.GetType().Name +
                    "' not implemented in class 'ValueComputation'");
        }

        private double GetNumericStep()
        {
            AutoIntervals AI = new AutoIntervals((double)GetMinValue(), (double)GetMaxValue(), numberOfValues, false);
            return AI.Step;
        }

        private object GetMinValue()
        {
            return (minValue == null ? axis.MinValue : minValue);
        }

        private object GetMaxValue()
        {
            return (maxValue == null ? axis.MaxValue : maxValue);
        }

        private void GetDateTimeStepUnit()
        {
            if (kind == CoordinatesComputationKind.ByStep)
                return;
            DateTimeAutoIntervals AI = new DateTimeAutoIntervals((DateTime)GetMinValue(),
                (DateTime)GetMaxValue(), numberOfValues, true);
            step = AI.Step;
            unit = AI.StepUnit;
        }

        private double GetIndexStep()
        {
            int x0, x1;
            object obj0 = GetMinValue();
            object obj1 = GetMaxValue();

            if (obj0 == null || obj1 == null)
                return 1;

            if (obj0 is int)
                x0 = (int)obj0;
            else if (obj0 is double)
                x0 = (int)(double)obj0;
            else if (obj0 is float)
                x0 = (int)(float)obj0;
            else
                throw new Exception("Cannot handle axis min value type '" + axis.MinValue.GetType().Name + "'");

            if (obj1 is int)
                x1 = (int)obj1;
            else if (obj1 is double)
                x1 = (int)(double)obj1;
            else if (obj1 is float)
                x1 = (int)(float)obj1;
            else
                throw new Exception("Cannot handle axis max value type '" + axis.MaxValue.GetType().Name + "'");

            if (x1 < x0)
            {
                int a = x0; x0 = x1; x1 = a;
            }
			
			int[] steps = new int[] { 1,2,5 };
			int oldStep = 0, newStep = 0, factor = 1;
			while(true)
			{
				bool valueFound = false;
				for(int i=0; i<3; i++)
				{
					newStep = steps[i]*factor;
					int v0 = (x0/newStep)*newStep;
					int v1 = (x1/newStep)*newStep;
					if(!AlwaysIncludeMinValue && v0 < x0) v0 += newStep;
					if(AlwaysIncludeMaxValue && v1 < x1) v1 += newStep;
					if((v1-v0)/newStep <= numberOfValues)
					{
						valueFound = true;
						if((v1-v0)/newStep == numberOfValues)
							oldStep = newStep;
						break;
					}
					oldStep = newStep;
				}
				if(valueFound)
				{
					if(oldStep > 0)
						step = oldStep;
					else
						step = newStep;
					break;
				}
				else
					factor = factor*10;
			}
            return step;
        }

        private double GetEnumStep()
        {
			return 1;
        }
        #endregion

        #region --- Adjusting Values ---

		internal bool RoundValueRange 
		{
			get { return roundValueRange; } 
			set 
			{ 
				if(value != roundValueRange)
				{
					roundValueRange = value; 
					SetDirty(true);
				}
			}
		}

        internal object GetAdjustedValue(object val, bool adjustUpwards)
        {
			if(!roundValueRange)
				return val;

            if (val is Coordinate)
                val = (val as Coordinate).Value;
            if (axis.Dimension is NumericDataDimension) return GetNumericAdjustedValue(val, adjustUpwards);
            else if (axis.Dimension is DateTimeDataDimension) return GetDateTimeAdjustedValue(val, adjustUpwards);
            else if (axis.Dimension is IndexDataDimension) return GetIndexAdjustedValue(val, adjustUpwards);
            else if (axis.Dimension is EnumeratedDataDimension) return GetEnumAdjustedValue(val, adjustUpwards);
            else
                throw new Exception("Dimension type ' " + axis.Dimension.GetType().Name +
                    "' not implemented in class 'ValueComputation'");
        }

        private double GetNumericAdjustedValue(object primitiveValue, bool adjustUpwards)
        {
            if (kind == CoordinatesComputationKind.ByNumberOfPoints)
                this.step = GetNumericStep();
            if (adjustUpwards)
                return step * Math.Ceiling((double)primitiveValue / step);
            else
                return step * Math.Floor((double)primitiveValue / step);
        }

        private DateTime GetDateTimeAdjustedValue(object primitiveValue, bool adjustUpwards)
        {
            DateTime primValue = (DateTime)primitiveValue;

            GetDateTimeStepUnit();

            return GetDateTimeAdjustedValue((DateTime)primitiveValue, step, unit, adjustUpwards);
        }

        internal static DateTime GetDateTimeAdjustedValue
            (DateTime primValue, double step, DateTimeUnit unit, bool adjustUpwards)
        {
            int year = primValue.Year;
            int month = primValue.Month;
            int day = primValue.Day;
            int hour = primValue.Hour;
            int min = primValue.Minute;
            int sec = primValue.Second;
            int ms = primValue.Millisecond;

            DateTime result = primValue;

            switch (unit)
            {
                case DateTimeUnit.Second:
                    result = new DateTime(year, month, day, hour, min, (int)(step * Math.Floor(sec / step)));
                    if (result < primValue && adjustUpwards)
                        result = result.AddSeconds((int)step);
                    break;
                case DateTimeUnit.Minute:
                    result = new DateTime(year, month, day, hour, (int)(step * Math.Floor(min / step)), 0);
                    if (result < primValue && adjustUpwards)
                        result = result.AddMinutes((int)step);
                    break;
                case DateTimeUnit.Hour:
                    result = new DateTime(year, month, day, (int)(step * Math.Floor(hour / step)), 0, 0);
                    if (result < primValue && adjustUpwards)
                        result = result.AddHours((int)step);
                    break;
                case DateTimeUnit.Day:
                    result = new DateTime(year, month, Math.Max(1, (int)(step * Math.Floor(day / step))), 12, 0, 0);
                    if (result < primValue && adjustUpwards)
                        result = result.AddDays((int)step);
                    break;
                case DateTimeUnit.Month:
                    result = new DateTime(year, Math.Max(1, (int)(step * Math.Floor(month / step))), 1, 12, 0, 0);
                    if (result < primValue && adjustUpwards)
                        result = result.AddMonths((int)step);
                    break;
                default:	//case DateTimeUnit.Year:
                    result = new DateTime((int)(step * Math.Floor(year / step)), 1, 1, 12, 0, 0);
                    if (result < primValue && adjustUpwards)
                        result = result.AddYears((int)step);
                    break;
            }
            return result;
        }

        private double GetIndexAdjustedValue(object primitiveValue, bool adjustUpwards)
        {
            int primValue = (int)primitiveValue;

            double step = GetStep();
            if (adjustUpwards)
                return step * Math.Ceiling(primValue / step);
            else
                return step * Math.Floor(primValue / step);
        }

        private string GetEnumAdjustedValue(object primitiveValue, bool adjustUpwards)
        {
            string primValue = (string)primitiveValue;

            double step = GetStep();
            if (step <= 0.0)
                step = 1.0;
            double dCoord = axis.Dimension.Coordinate(primitiveValue), targetCoord;
            if (adjustUpwards)
                targetCoord = step * Math.Ceiling(dCoord / step);
            else
                targetCoord = step * Math.Floor(dCoord / step);

            if (targetCoord == dCoord)
                return primValue;

            EnumeratedDataDimension dim = axis.Dimension as EnumeratedDataDimension;
            ArrayList leaves = dim.Root.ListLeaves();

            int i = 1;
            while (dim.Coordinate(leaves[i]) < targetCoord)
                i++;
            if (dim.Coordinate(leaves[i]) > targetCoord && !adjustUpwards)
                i--;
            Coordinate c = leaves[i] as Coordinate;
            return (string)c.Value;
        }


        #endregion

        #region --- Computing List of Coordinates ---

        private void Evaluate()
        {
            if (!dirty || axis == null || axis.OwningChart == null || axis.OwningChart.InSerialization)
                return;
            dirty = false;

            // Min/max values
            if (!minValueSet || minValue == null)
            {
                minValue = axis.MinValue;
                if (axis.CoordSystem.YAxis == axis)
                {
                    object refv = axis.CoordSystem.OwningSeries.GetReferenceValue();
                    if (refv != null)
                    {
                        if (axis.LCoordinate(refv) < axis.LCoordinate(minValue))
                            minValue = refv;
                    }
                }
            }
            if (!maxValueSet || maxValue == null)
            {
                maxValue = axis.MaxValue;
                if (axis.CoordSystem.YAxis == axis)
                {
                    object refv = axis.CoordSystem.OwningSeries.GetReferenceValue();
                    if (refv != null)
                    {
                        if (axis.LCoordinate(refv) > axis.LCoordinate(maxValue))
                            maxValue = refv;
                    }
                }
            }
            if (!minValueSet)
                minValue = GetAdjustedValue(minValue, false);
            if (!maxValueSet)
                maxValue = GetAdjustedValue(maxValue, true);

            // Coordinate set
            if (axis.Dimension is NumericDataDimension)
                cSet = GetNumericCoordinateSet();
            else if (axis.Dimension is DateTimeDataDimension)
                cSet = GetDateTimeCoordinateSet();
            else if (axis.Dimension is EnumeratedDataDimension)
                cSet = GetEnumeratedCoordinateSet();
            else if (axis.Dimension is IndexDataDimension)
                cSet = GetIndexCoordinateSet();
            else
                throw new Exception("Cannot create coordinate set for dimension type '" +
                    axis.Dimension.GetType().Name + "'");
        }

        /// <summary>
        /// Gets the resulting coordinate set. This property can be used only after data binding has been performed.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CoordinateSet Value { get { if(cSet == null) SetDirty(true); Evaluate(); return cSet; } }

        internal CoordinateSet GetCoordinateSet(bool includeMinValue, bool includeMaxValue)
        {
            bool oldIMin = alwaysIncludeMinValue;
            bool oldIMax = alwaysIncludeMaxValue;
            alwaysIncludeMinValue = includeMinValue;
            alwaysIncludeMaxValue = includeMaxValue;
            SetDirty(true);
            CoordinateSet result = Value;
            SetDirty(true);
            alwaysIncludeMinValue = oldIMin;
            alwaysIncludeMaxValue = oldIMax;
            return result;
        }

        #region --- Computing coordinate sets ---

        private NumericCoordinateSet GetNumericCoordinateSet()
        {
            NumericCoordinateSet cs = new NumericCoordinateSet(axis, 0);
            cs.ValuesMethod = ValueKind.UserDefined;
            ComputeNumericList();
            cs.Add(list);
            return cs;
        }

        private DateTimeCoordinateSet GetDateTimeCoordinateSet()
        {
            DateTimeCoordinateSet cs = new DateTimeCoordinateSet(axis, 0);
            cs.ValuesMethod = ValueKind.UserDefined;
            ComputeDateTimeList();
            cs.Add(list);
            return cs;
        }

        private IndexCoordinateSet GetIndexCoordinateSet()
        {
			if(!numberOfValuesExplicit)
			{
				numberOfValues = 50;
			}
			IndexCoordinateSet cs = new IndexCoordinateSet(axis, 0);
            cs.ValuesMethod = ValueKind.UserDefined;
            ComputeIndexList();
            cs.Add(list);
            return cs;
        }

        private EnumeratedCoordinateSet GetEnumeratedCoordinateSet()
        {
			if(!numberOfValuesExplicit)
			{
				numberOfValues = 50;
			}
            EnumeratedCoordinateSet cs = new EnumeratedCoordinateSet(this);
            cs.ValuesMethod = ValueKind.UserDefined;
            cs.ComputeValueList();
            list = cs.ValueList;
            return cs;
        }

        #endregion


        #region --- Computing value array list ---

        private void ComputeNumericList()
        {
            Evaluate();
            if (axis.IsLogarithmic)
                ComputeLogatithmicNumericList();
            else
                ComputeLinearNumericList();
        }

        private void ComputeLogatithmicNumericList()
        {
            double min = (double)minValue;
            double max = (double)maxValue;

            int logBase = axis.LogBase;
            if (max <= 0)
            {
                axis.OwningChart.RegisterErrorMessage("Cannot render log axis for values negative or zero");
                ComputeLinearNumericList();
                return;
            }
            if (min <= 0)
            {
                min = 1.0;
                while (min >= max)
                    min /= logBase;
            }

            int nItems;
            if (this.kind == CoordinatesComputationKind.ByStep)
                nItems = 5;
            else
                nItems = this.NumberOfValues;
            double[] values = Intervals.LogValues(min, max, logBase, nItems);

            // Add end points if needed
            list = new ArrayList(values.Length + 1);
            if (alwaysIncludeMinValue && values[0] > min)
                list.Add(min);
            list.AddRange(values);
            if (alwaysIncludeMaxValue && values[values.Length - 1] < max)
                list.Add(min);
        }

        private void ComputeLinearNumericList()
        {
            list = new ArrayList();

            double x = (double)minValue;
            step = GetStep();
            if (alwaysIncludeMinValue)
                list.Add(x);

            double y = Math.Ceiling(x / step) * step;
            double max = (double)maxValue;
            while (y < max)
            {
                if (y > x || y == x && !alwaysIncludeMinValue)
                    list.Add(y);
                y += step;
            }
            if (alwaysIncludeMaxValue || y == max)
                list.Add(max);
        }

        private void ComputeDateTimeList()
        {
            list = new ArrayList();
            DateTimeCoordinateSet cs = new DateTimeCoordinateSet(axis, 0);
            cs.ValuesMethod = ValueKind.UserDefined;
            DateTime x = (DateTime)minValue;
            step = GetStep();
            if (alwaysIncludeMinValue)
                list.Add(x);

            DateTime y = GetDateTimeAdjustedValue(x, true);
            DateTime max = (DateTime)maxValue;
            while (y < max)
            {
                if (y > x || y == x && !alwaysIncludeMinValue)
                    list.Add(y);
                switch (unit)
                {
                    case DateTimeUnit.Year:
                        y = y.AddYears((int)step);
                        break;
                    case DateTimeUnit.Month:
                        y = y.AddMonths((int)step);
                        break;
                    case DateTimeUnit.Day:
                        y = y.AddDays((int)step);
                        break;
                    case DateTimeUnit.Hour:
                        y = y.AddHours((int)step);
                        break;
                    case DateTimeUnit.Minute:
                        y = y.AddMinutes((int)step);
                        break;
                    case DateTimeUnit.Second:
                        y = y.AddSeconds((int)step);
                        break;
                }
            }
            if (alwaysIncludeMinValue || y == max)
                list.Add(max);
        }

        private void ComputeIndexList()
        {
            list = new ArrayList();
            int x = (int)(double)minValue;

            int step = (int)(double)GetStep();
            step = Math.Max(step, 1);
            this.step = step;
            list.Add(x);

            int y = x + step;
            int max = (int)(double)maxValue;
            while (y < max)
            {
                if (y > x)
                    list.Add(y);
                y += step;
            }
            list.Add(max);
        }

        #endregion

        #endregion
    }


	// ==================================================================================================

	internal enum CoordinateSystemKind
	{
		DCS,		/// Data coordinate system
		LCS,		/// Logical coordinate system
		ICS,		/// Intermediate coordinate system
		WCS,        /// World coordinate system
		TCS			/// Target coordinate system
	}

	/// <summary>
	/// Defines the Orientation of an Axis.
	/// </summary>
	public enum AxisOrientation
	{
		/// <summary>
		/// Default X-Axis Orientation.
		/// </summary>
		XAxis,
		/// <summary>
		/// Default Y-Axis Orientation.
		/// </summary>
		YAxis,
		/// <summary>
		/// Default Z-Axis Orientation.
		/// </summary>
		ZAxis,
		/// <summary>
		/// Reversed X-Axis Orientation.
		/// </summary>
		XAxisNegative, 
		/// <summary>
		/// Reversed Y-Axis Orientation.
		/// </summary>
		YAxisNegative,
		/// <summary>
		/// Reversed Z-Axis Orientation.
		/// </summary>
		ZAxisNegative,
		/// <summary>
		/// Unknown Orientation.
		/// </summary>
		Unknown
	}

	// ==========================================================================================================
	//  Axis 
	// ==========================================================================================================
 
	/// <summary>
	///  Axes help  in  determining  the  position and/or size of various types of objects within  the chart's  3D space.
	/// </summary>
	/// <remarks>
	///   <para>
	///  Property <see cref="AxisAnnotations"/> is  an <see cref="AxisAnnotationCollection"/> containing 
	///  all axis annotation objects related to this axis. It is important to understand  the difference
	///  between <see cref="Axis"/> and <see cref="AxisAnnotation"/>: axis helps locating things in the 3D
	///  space, axis annotation consists of  the  axis line, labels of specific values, tick marks etc . 
	///  One single axis may have many annotations (for example, YAxis may have annotations to the
	///  left side and/or to the right side of PlaneXY and/or wherever user wishes to place an anotation!).
	///   </para>
	///   <para>
	///     The control creates the following default axis annotations:
	///     <list type="bullet">
	///     <item>"X@Zmax", "X@Ymin", "X@Ymax" and "X@Zmin" for XAxis,</item>
	///     <item>"Y@Zmax", "Y@Xmin", "Y@Xmax" and "Y@Zmin" for YAxis, and</item>
	///     <item>"Z@Xmax", "Z@Xmin", "Z@Ymax" and "Z@Ymin" for ZAxis.</item>
	///    </list>
	///    At most one of  the annotations associated to an axis is initially visible, depending on the type of
	///     the coordinate system (2D or 3D). At after-dataBind time visibility and other properties of default
	///    annotations can be changed, or new annotations can be added using  the 
	///    <see cref="CreateAnnotation"/>   method .
	///   </para>
	///   <para>
	///     <see cref="Axis"/> object has properties defining minimum and maximum values displayed along
	///     that axis:
	///     <list type="bullet">
	///       <item><see cref="MinValue"/> and <see cref="MaxValue"/> define the range of values in the
	///         input Data Coordinate System (DCS). Values are automatically computed at DataBind time
	///         from input data and can be latter modified. (See  the  topic "Coordinates and Coordinate Systems" in
	///          the section "Advanced Concepts".)
	///       </item>
	///       <item>
	///   <see cref="MinValueLCS"/> and <see cref="MaxValueLCS"/> are readonly values derived from
	///           DCS minimum and maximum values.
	///       </item>
	///       <item>
	///   <see cref="MinValueICS"/> and <see cref="MaxValueICS"/> are minimum and maximum values in
	///           the Intermediate Coordinate System (ICS). MaxValueICS is initially created by the control,
	///           but it can be modified latter to change the size along this axis. MinValueICS is always 0 in
	///           this implementation. See "Multiple Coordinate Systems" in  the section "Advanced Concepts" for more 
	///            information about  the use    of these properties to shape and align coordinate systems.
	///       </item>
	///     </list>
	///   </para>
	///   <para>
	///     The methods <see cref="CreateCoordinateSet"/> and <see cref="CreateCoordinateSetByStep"/>
	///     can be used to create  coordinate sets for custom strips, grids and axis annotations.
	///   </para>
	/// </remarks>
	[TypeConverter(typeof(AxisConverter))]
	public sealed class Axis : ChartObject
	{
		private DataDimension	dimension = null;
		private object			minValue = null, maxValue = null;
		private AxisOrientation	orientation;
		private bool			reverse = false;

		private bool			hasChanged = true;

		private double			minICSCoordinate=0, maxICSCoordinate=0;
		private bool			autoICSRange = true;

		private AxisAnnotationCollection	axisAnnotations;

		private double			scale = 1.0;

		private GenericType genericMinimum = new GenericType();
		private GenericType genericMaximum = new GenericType();

		// Default coordinate set		
		private CoordinateSet defaultCoordinateSet = null;
		private CoordinateSetComputation  defaultCoordinateSetComputation;
		private bool defaultCoordinateSetIsCustom = false;

		private bool roundValueRange = true;

		#region --- Constructors --- 
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Axis"/> class.
		/// </summary>
		public Axis()
		{
			axisAnnotations = new AxisAnnotationCollection(this);
			defaultCoordinateSetComputation = new CoordinateSetComputation(this);
			genericMinimum.ValueSet += new EventHandler(HandleMinimumChanged);
			genericMaximum.ValueSet += new EventHandler(HandleMaximumChanged);
		}

		#endregion

		#region --- Axis Parameters Setup ---
		internal DataDimension	Dimension		
		{ 
			get 
			{ 
				return dimension;
			}
		}

		internal void SetDimension(DataDimension dim)
		{
			if(OwningChart.InDesignMode || OwningChart.InWizard)
			{
				minValue = null;
				maxValue = null;
				defaultCoordinateSet = null;
				if(defaultCoordinateSetComputation == null)
					defaultCoordinateSetComputation = new CoordinateSetComputation(this);
			}

			if(dimension != dim)
			{
				dimension = dim;
				defaultCoordinateSet = null;
				if(dimension != null)
				{
					if(minValue != null)
						minValue = dimension.ConvertToRightType(minValue);
					if(maxValue != null)
						maxValue = dimension.ConvertToRightType(maxValue);;
				}
			}
			if(defaultCoordinateSetComputation != null)
				defaultCoordinateSetComputation.SetDirty(true);

		}
		
		// This is axis orientation within the parent CS. May not be the effective orientation,
		// since parent orientation may be non-default.
		
		internal AxisOrientation Orientation	
		{
			get { return orientation;  } 
			set { orientation = value; }
		}

		internal AxisOrientation Role
		{
			get { return CoordSystem.Role(this); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="Axis"/> is reversed.
		/// </summary>
		[DefaultValue(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool				Reverse		{ get { return reverse; }  set { reverse = value;  } }
		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="Axis"/> is displayed.
		/// </summary>
		[Browsable(false)]
		public override bool 	Visible		{ get { return base.Visible; } set { base.Visible = value; } }


		#region -- Adjusting the values range ---
		/// <summary>
		/// Adjusts the minimum and maximum values of this axis to the closest multiples of 'step'.
		/// Values of series are enclosed within the adjusted range.
		/// </summary>
		/// <param name="step">EffectiveStep used in the adjusted values computation</param>
		/// <remarks>
		/// This method could be applied after DataBind() is invoked, for numeric x- or y-axis.
		/// </remarks>
		public void AdjustRange(double step)
		{
			AdjustMinimumValue(step);
			AdjustMaximumValue(step);
		}

		internal void AdjustRangeInternal()
		{
			if(roundValueRange)
				this.AdjustRange();
		}

		/// <summary>
		/// Adjusts the minimum value of this axis to the closest multiple of 'step' not greater than minimum value of series.
		/// </summary>
		/// <param name="step">EffectiveStep used in the adjusted value computation</param>
		/// <remarks>
		/// This method could be applied after DataBind() is invoked, for numeric x- or y-axis.
		/// </remarks>
		public void AdjustMinimumValue(double step)
		{
			if(step == 0)
				throw new Exception("Cannot adjust value with step = 0");
			if(Dimension.ItemType != typeof(double))
				throw new Exception("Cannot adjust value with double step for coordinate type '" + Dimension.ItemType.Name +"'");

			SeriesBase s = CoordSystem.OwningSeries;
			double val;
			if(minValue == null)
			{
				if(this == CoordSystem.XAxis)
					val = (double)s.MinXDCS();
				else if(this == CoordSystem.YAxis)
					val = (double)s.MinYDCS();
				else
					throw new Exception("Cannot adjust value for Z-coordinate");
			}
			else
				val = (double)minValue;
			step = Math.Abs(step);
			if(Reverse)
				maxValue = Math.Floor(val/step)*step;
			else
				minValue = Math.Floor(val/step)*step;
		}

		/// <summary>
		/// Adjusts the maximum value of this axis to the closest multiple of 'step' not less than maximum value of series.
		/// </summary>
		/// <param name="step">EffectiveStep used in the adjusted value computation</param>
		/// <remarks>
		/// This method could be applied after DataBind() is invoked, for numeric x- or y-axis.
		/// </remarks>
		public void AdjustMaximumValue(double step)
		{
			if(Dimension.ItemType != typeof(double))
				throw new Exception("Cannot adjust value with double step for coordinate type '" + Dimension.ItemType.Name +"'");
			if(step == 0)
				throw new Exception("Cannot adjust value with step = 0");

			SeriesBase s = CoordSystem.OwningSeries;
			double val;
			if(maxValue == null)
			{
				if(this == CoordSystem.XAxis)
					val = (double)s.MaxXDCS();
				else if(this == CoordSystem.YAxis)
					val = (double)s.MaxYDCS();
				else
					throw new Exception("Cannot adjust value for Z-coordinate");
			}
			else
				val = (double) maxValue;
			step = Math.Abs(step);
			if(Reverse)
				minValue = Math.Ceiling(val/step)*step;
			else
				maxValue = Math.Ceiling(val/step)*step;
		}

		private static double RoundDown(double val, double step)
		{
			if(step == 0)
				throw new Exception("Cannot adjust value with step = 0");
			return Math.Floor(val/step)*step;
		}

		private static double RoundUp(double val, double step)
		{
			if(step == 0)
				throw new Exception("Cannot adjust value with step = 0");
			return Math.Ceiling(val/step)*step;
		}

		/// <summary>
		/// Adjusts the minimum and maximum values of this axis to the closest multiples of 'step'.
		/// Values of series are enclosed within the adjusted range.
		/// </summary>
		/// <param name="step">EffectiveStep used in the adjusted values computation</param>
		/// <param name="unit">Specifies the units of the <paramref name="step" />.</param>
		/// <remarks>
		/// This method could be applied after DataBind() is invoked, for date-time x- or y-axis.
		/// </remarks>
		public void AdjustRange(int step, DateTimeUnit unit)
		{
			AdjustMaximumValue(step,unit);
			AdjustMinimumValue(step,unit);
		}

		/// <summary>
		/// Adjusts the minimum value of this axis to the closest multiple of 'step' not greater than minimum value of series.
		/// </summary>
		/// <param name="step">EffectiveStep used in the adjusted value computation</param>
		/// <param name="unit">Specifies the units of the <paramref name="step" />.</param>
		/// <remarks>
		/// This method could be applied after DataBind() is invoked, for date-time x- or y-axis.
		/// </remarks>
		public void AdjustMinimumValue(int step, DateTimeUnit unit)
		{
			if(step == 0)
				throw new Exception("Cannot adjust value with step = 0");
			if(Dimension.ItemType != typeof(DateTime))
				throw new Exception("Cannot adjust value with DateTimeUnit unit for coordinate type '" + Dimension.ItemType.Name +"'");

			CoordinateSetComputation csc = new CoordinateSetComputation(this);
			csc.EffectiveStep = step;
			csc.Unit = unit;
			csc.ComputationKind = CoordinatesComputationKind.ByStep;
			minValue = csc.MinValue;
			defaultCoordinateSetComputation.SetDirty(true);
		}

		/// <summary>
		/// Adjusts the maximum value of this axis to the closest multiple of 'step' not less than maximum value of series.
		/// </summary>
		/// <param name="step">EffectiveStep used in the adjusted value computation</param>
		/// <param name="unit">Specifies the units of the <paramref name="step" />.</param>
		/// <remarks>
		/// This method could be applied after DataBind() is invoked, for date-time x- or y-axis.
		/// </remarks>
		public void AdjustMaximumValue(int step, DateTimeUnit unit)
		{
			if(step == 0)
				throw new Exception("Cannot adjust value with step = 0");
			if(Dimension.ItemType != typeof(DateTime))
				throw new Exception("Cannot adjust value with DateTimeUnit unit for coordinate type '" + Dimension.ItemType.Name +"'");
			CoordinateSetComputation csc = new CoordinateSetComputation(this);
			csc.EffectiveStep = step;
			csc.Unit = unit;
			csc.ComputationKind = CoordinatesComputationKind.ByStep;
			maxValue = csc.MaxValue;
			defaultCoordinateSetComputation.SetDirty(true);
		}

		internal void AdjustRange()
		{
			if(Dimension == null)
				return;
            this.DefaultCoordinateSetComputation.Axis = this;
			if(Dimension.ItemType == typeof(DateTime))
			{
				DateTime min = (DateTime)minValue;
				DateTime max = (DateTime)maxValue;
				if(min == max)
				{
					minValue = min - TimeSpan.FromDays(0.5);
					maxValue = min + TimeSpan.FromDays(0.5);
				}
				else
				{
					minValue = (DateTime) DefaultCoordinateSetComputation.MinValue;
					maxValue = (DateTime) DefaultCoordinateSetComputation.MaxValue;
				}
				if(!Dimension.ReferenceVariableIsCustom)
				{
					Dimension.ReferenceValue = minValue;
					Dimension.ReferenceVariableIsCustom = false;
				}

			}
			else if(Dimension.ItemType == typeof(double))
			{
				double min = 0,max = 0;
				if(minValue is double)
					min = (double)minValue;
				else if(minValue is int)
					min = (double)(int)minValue;
				if(maxValue is double)
					max = (double)maxValue;
				else if(maxValue is int)
					max = (double)(int)maxValue;

				if(IsLogarithmic)
				{
					double aMin = Math.Min(min,max);
					double aMax = Math.Max(min,max);
					if(aMin > 0 && aMax>=aMin)
					{
						int nSteps = 5;
						double[] vals = Intervals.LogValues(aMin,aMax,LogBase,nSteps);
						minValue = vals[0];
						maxValue = vals[vals.Length-1];
						return;
					}
				}

				if(!IsLogarithmic)
				{
					min /= scale;
					max /= scale;
				}

				if(min < max)
				{
					minValue = min;
					maxValue = max;
					//xpi = new AutoIntervals(min,max, nSteps, false);
					minValue = (double)DefaultCoordinateSetComputation.MinValue;//xpi.MinValue;
					maxValue = (double)DefaultCoordinateSetComputation.MaxValue;//xpi.MaxValue;
				}
				else if(min > max)
				{
					minValue = max;
					maxValue = min;
					minValue = (double)DefaultCoordinateSetComputation.MinValue;
					maxValue = (double)DefaultCoordinateSetComputation.MaxValue;
//					xpi = new AutoIntervals(max,min, nSteps, false);
//					minValue = xpi.MaxValue;
//					maxValue = xpi.MinValue;
				}
				else
				{
					minValue = min;
					maxValue = max;
				}

				if(!IsLogarithmic)
				{
					minValue = scale*(double)minValue;
					maxValue = scale*(double)maxValue;
				}
			}
		}
		#endregion
		#endregion

		#region --- Coordinate Ranges ---

        /// <summary>
        /// Gets or sets the number by which this <see cref="Axis"/> object is scaled.
        /// </summary>
		[SRDescription("AxisScaleDescr")]
		[DefaultValue(1.0)]
		public double Scale 
		{
			get 
			{
				return scale; 
			}
			set 
			{
				if(value <= 0.0)
					throw new Exception("Axis scale has to be > 0");
				scale = value; 
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[DefaultValue(typeof(GenericType),"")]
		/// <summary>
		/// Gets or sets the minimum value of this <see cref="Axis"/> object.
		/// </summary>
		public GenericType Minimum 
		{
			get 
			{
				return genericMinimum; 
			}
			set 
			{
				genericMinimum = value; 
				minValue = genericMinimum.InternalValue;
				defaultCoordinateSetComputation.SetDirty(true);
			}
		}

		private void HandleMinimumChanged(object genMinimum,EventArgs ea)
		{
			if(OwningChart==null || !OwningChart.InDesignMode)
			{
				minValue = genericMinimum.InternalValue;
				defaultCoordinateSetComputation.SetDirty(true);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[DefaultValue(typeof(GenericType),"")]
		/// <summary>
		/// Gets or sets the minimum value of this <see cref="Axis"/> object.
		/// </summary>
		public GenericType Maximum 
		{
			get 
			{
				return genericMaximum; 
			}
			set 
			{
				genericMaximum = value; 
				maxValue = genericMaximum.InternalValue;
				defaultCoordinateSetComputation.SetDirty(true);
			}
		}

		private void HandleMaximumChanged(object genMinimum,EventArgs ea)
		{
			if(OwningChart==null || !OwningChart.InDesignMode)
			{
				maxValue = genericMaximum.InternalValue;
				defaultCoordinateSetComputation.SetDirty(true);
			}
		}

		/// <summary>
		/// Gets or sets the minimum value of this <see cref="Axis"/> object.
		/// </summary>
		/// <remarks>
		/// This method should only be applied after DataBind() is invoked.
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public object			MinValue	
		{
			get 
            {
                if (minValue is String && Dimension != null)
                    minValue = Dimension.ValueOf((string)minValue);

                return minValue; 
            } 
			set 
			{
				if(value == null)
					throw new Exception("Cannot set 'null' as MinValue");

				defaultCoordinateSetComputation.SetDirty(true);
				if(Dimension == null || value.GetType() == Dimension.ItemType)
				{
					minValue = value;
					return;
				}
				else
				{
					if(Dimension.ItemType == typeof(double))
					{
						if(value.GetType() == typeof(int))
						{
							minValue = (double)((int)value);
							return;
						}
						else if(value.GetType() == typeof(float))
						{
							minValue = (double)((float)value);
							return;
						}
					}
					else if(Dimension.ItemType == typeof(int))
					{
						if(value.GetType() == typeof(double))
						{
							minValue = (int)((double)value);
							return;
						}
						else if(value.GetType() == typeof(float))
						{
							minValue = (int)((float)value);
							return;
						}
					}
				}
				throw new Exception("Cannot set a(n) '" + value.GetType().Name + "' to MinValue when '" + Dimension.ItemType.Name + "' is expected");
			}
		}

		/// <summary>
		/// Gets or sets the maximum value of this <see cref="Axis"/> object.
		/// </summary>
		/// <remarks>
		/// This method should only be applied after DataBind() is invoked.
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public object			MaxValue	
		{
			get
            {
                if (maxValue is String && Dimension != null)
                    maxValue = Dimension.ValueOf((string)maxValue);

                return maxValue;
            } 
			
			set 
			{
				if(value == null)
					throw new Exception("Cannot set 'null' as MaxValue");

				defaultCoordinateSetComputation.SetDirty(true);
				if(Dimension == null || value.GetType() == Dimension.ItemType)
				{
					maxValue = value;
					return;
				}
				else
				{
					if(Dimension.ItemType == typeof(double))
					{
						if(value.GetType() == typeof(int))
						{
							maxValue = (double)((int)value);
							return;
						}
						else if(value.GetType() == typeof(float))
						{
							maxValue = (double)((float)value);
							return;
						}
					}
					else if(Dimension.ItemType == typeof(int))
					{
						if(value.GetType() == typeof(double))
						{
							maxValue = (int)((double)value);
							return;
						}
						else if(value.GetType() == typeof(float))
						{
							maxValue = (int)((float)value);
							return;
						}
					}
				}
				
				throw new Exception("Cannot set a(n) '" + value.GetType().Name + "' to MaxValue when '" + Dimension.ItemType.Name + "' is expected");
			}
		}

		/// <summary>
		/// Property indicating whether the value range should be rounded.
		/// </summary>
		[DefaultValue(true)]
		[Description("Indicates whether the minimum and maximum values should be rounded")]
		public bool RoundValueRange 
		{
			get { return roundValueRange; } 
			set { roundValueRange = value; } 
		}

		/// <summary>
		/// Gets the minimum value of this <see cref="Axis"/> object in the Logical Coordinate System (LCS).
		/// </summary>
		/// <remarks>
		/// This method should only be applied after DataBind() is invoked.
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public double MinValueLCS	{ get { return Dimension.Coordinate(minValue); } }
		/// <summary>
		/// Gets the maximum value of this <see cref="Axis"/> object in the Logical Coordinate System (LCS).
		/// </summary>
		/// <remarks>
		/// This method should only be applied after DataBind() is invoked.
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public double MaxValueLCS { get { return  Dimension.Coordinate(maxValue) + Dimension.Width(maxValue); } }
		
		/// <summary>
		/// Gets the minimum value of this <see cref="Axis"/> object in the Intermediate Coordinate System (ICS).
		/// </summary>
		/// <remarks>
		/// This method should only be applied after DataBind() is invoked.
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public double MinValueICS	{ get { return minICSCoordinate; } }
		
		/// <summary>
		/// Gets the maximim value of this <see cref="Axis"/> object in the Intermediate Coordinate System (ICS).
		/// </summary>
		/// <remarks>
		/// This method should only be applied after DataBind() is invoked.
		/// </remarks>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public double MaxValueICS { 
			get { return maxICSCoordinate; } 
			set { maxICSCoordinate = value; autoICSRange = false; } 
		}
		
		internal void SetMaxValueICS(double endICSCoordinate)
		{
			maxICSCoordinate = endICSCoordinate;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the range of Intermediate Coordinate System (ICS) is set automatically.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool AutoICSRange { get { return autoICSRange; } set { autoICSRange = value; } }

		#endregion

		#region --- Coordinates Mapping ---
		

		#region --- Basic Coordinate Mappings ---

		/// <summary>
		/// Returns the value in Logical Coordinate System (LCS) of a data coordinate along this <see cref="Axis"/> object.
		/// </summary>
		/// <param name="obj">Data coordinate in Data Coordinate System (DCS).</param>
		/// <returns>LCS value of the <paramref name="obj" /> along this <see cref="Axis"/> object.</returns>
		public double LCoordinate(object obj) { return Dimension.Coordinate(obj); }
		/// <summary>
		/// Returns the width in Logical Coordinate System (LCS) units of a data coordinate along this <see cref="Axis"/> object.
		/// </summary>
		/// <param name="obj">Data coordinate in Data Coordinate System (DCS).</param>
		/// <returns>LCS width of the <paramref name="obj" /> along this <see cref="Axis"/> object.</returns>
		public double LWidth     (object obj) { return Dimension.Width(obj); }
		/// <summary>
		/// Returns the value in Intermediate Coordinate System (ICS) of a data coordinate along this <see cref="Axis"/> object.
		/// </summary>
		/// <param name="obj">Data coordinate in Data Coordinate System (DCS).</param>
		/// <returns>ICS value of the <paramref name="obj" /> along this <see cref="Axis"/> object.</returns>
		public double ICoordinate(object obj) { return LCS2ICS(LCoordinate(obj)); }

		/// <summary>
		/// Returns the width in Intermediate Coordinate System (ICS) units of a data coordinate along this <see cref="Axis"/> object.
		/// </summary>
		/// <param name="obj">Data coordinate in Data Coordinate System (DCS).</param>
		/// <returns>ICS width of the <paramref name="obj" /> along this <see cref="Axis"/> object.</returns>
		public double IWidth     (object obj) { return LCS2ICS(LCoordinate(obj) + LWidth(obj)) - ICoordinate(obj); }

		// Standardized coordinate and width compensate for the negative width in case of reversed
		// axis.
		internal double ICoordinateStd(object obj) 
		{ 
			double iw = IWidth(obj);
			if(iw >= 0)
				return ICoordinate(obj);
			else
				return LCS2ICS(LCoordinate(obj) + LWidth(obj));
		}
		internal double IWidthStd  (object obj) { return Math.Abs(IWidth(obj)); }

		/// <summary>
		/// Converts a value in a Logical Coordinate System (LCS) to a corresponding value in Intermediate Coordinate System (ICS) along this <see cref="Axis"/> object.
		/// </summary>
		/// <param name="lc">value to convert.</param>
		/// <returns>Resuling value in ICS of <paramref name="lc" /></returns>
		public double LCS2ICS(double lc)
		{
			double minLCS = MinValueLCS;
			double maxLCS = MaxValueLCS;
			double minICS = MinValueICS;
			double maxICS = MaxValueICS;

			double a = (lc - minLCS)/(maxLCS - minLCS);
			if(IsLogarithmic)
			{
				if(lc <= 0 || minLCS <=0)
					throw new Exception("Logarithmic axis is not allowed for zero or negative values");
				a = (Math.Log(lc)-Math.Log(minLCS))/(Math.Log(maxLCS)-Math.Log(minLCS));
			}
			if(reverse)
				a = 1.0 - a;
			return a*maxICS + (1.0-a)*minICS;
		}

		#endregion

		internal CoordinateSystem CoordSystem { get { return Owner as CoordinateSystem; } }
        /// <summary>
        /// Internal x coordinates in pixels
        /// </summary>
		public void GetPixelPosition(object point, out double pixelPosition, out double pixelWidth)
		{
			// We assume that the other two coordinates are 0.
			double positionLCS = LCoordinate(point);
			double widthLCS = LWidth(point);
			// Position
			Vector3D pWCS = CoordSystem.LCS2WCS(positionLCS*this.UnitVector);
			Vector3D pTCS = CoordSystem.TargetArea.Mapping.Map(pWCS);
			pixelPosition = pTCS.X;
			// Width
			Vector3D wWCS = CoordSystem.LCS2WCS((positionLCS+widthLCS)*this.UnitVector);
			Vector3D wTCS = CoordSystem.TargetArea.Mapping.Map(wWCS);
			pixelWidth = wTCS.X - pixelPosition;

		}

		#endregion

		#region --- Axis Annotations ---

		/// <summary>
		/// Gets the collection of <see cref="AxisAnnotation"/>s in this <see cref="Axis"/> object.
		/// </summary>
#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[SRDescription("AxisAxisAnnotationsDescr")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public AxisAnnotationCollection AxisAnnotations	
		{
			get 
			{ 
				if(OwningChart != null && OwningChart.InSerialization)
				{
					AxisAnnotationCollection aac = new AxisAnnotationCollection(this);
					foreach(AxisAnnotation aa in axisAnnotations)
					{
						if(aa.HasChanged)
							aac.Add(aa);
					}
					return aac;
				}
				else
					return axisAnnotations; 
			} 
		}

		internal bool IsLogarithmic	
		{ 
			get { return CoordSystem.YAxis == this && CoordSystem.OwningSeries.IsLogarithmic; } 
		}

		internal int LogBase	{ get { return CoordSystem.OwningSeries.LogBase; } }

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		internal Vector3D UnitVector			
		{
			get 
			{
				return ChartSpace.AxisUnitVector(orientation); 
			}
		}
	
		internal Vector3D UnitVectorWCS
		{
			get
			{
				Vector3D r = UnitVector;
				if(Reverse)
					return -r;
				else
					return r;
			}
		}

		/// <summary>
		/// Creates an <see cref="AxisAnnotation"/> object for this axis and adds it to the <see cref="Axis.AxisAnnotations"/> collection.
		/// </summary>
		/// <param name="name">The name of the annotation.</param>
		/// <param name="plane">Plane the new annotation will be located on.</param>
		/// <param name="position">position of the annotation.</param>
		/// <returns>Newly created <see cref="AxisAnnotation"/> object.</returns>
		public AxisAnnotation CreateAnnotation(string name, CoordinatePlane plane, AxisLinePositionKind position)
		{
			if (plane != null && plane.XAxis != this && plane.YAxis != this)
				throw new ArgumentException("Annotation " + name + " could not be created since the specified plane does not belong to this axis.");

			AxisOrientation alternateAxisOrientation = 
				(plane.XAxis == this)? plane.YAxis.Role:plane.XAxis.Role;
			AxisAnnotation aa = new AxisAnnotation(name,alternateAxisOrientation,position);
			AxisAnnotations.Add(aa);
			aa.DataBind();
			return aa;
		}

		#endregion

		#region --- Coordinate Set ---

		/// <summary>
		/// Creates coordinate set containing multiples of 'step' between current minimum and maximum value.
		/// </summary>
		/// <param name="step">EffectiveStep between computed coordinates</param>
		/// <returns>Coordinate set containing multiples of 'step' between current minimum and maximum value. 
		/// If minimum and/or maximum value is multiple of the 'step', it is included in the coordinate set.
		/// </returns>
		/// <remarks>Applies only for numerical axis. For other types of axes an exception is thrown.</remarks>
		public CoordinateSet CreateCoordinateSetByStep(double step)
		{
			CoordinateSetComputation csc = new CoordinateSetComputation(this);
			csc.EffectiveStep = step;
			csc.ComputationKind = CoordinatesComputationKind.ByStep;
			return csc.Value;
		}

		/// <summary>
		/// Creates coordinate set containing multiples of 'step' time units 'unit' between current minimum and maximum value.
		/// </summary>
		/// <param name="step">EffectiveStep between computed coordinates</param>
		/// <param name="unit">Date/time unit in which the step is expressed</param>
		/// <returns>Coordinate set containing multiples of 'step'*'unit' between current minimum and maximum value. 
		/// If minimum and/or maximum value is multiple of the 'step', it is included in the coordinate set.
		/// </returns>
		/// <remarks>Applies only for numerical axis. For other types of axes an exception is thrown.</remarks>
		public CoordinateSet CreateCoordinateSetByStep(int step, DateTimeUnit unit)
		{
			CoordinateSetComputation csc = new CoordinateSetComputation(this);
			csc.EffectiveStep = step;
			csc.Unit = unit;
			csc.ComputationKind = CoordinatesComputationKind.ByStep;
			return csc.Value;
		}

		
		/// <summary>
		/// Creates coordinate set based on a number of items.
		/// </summary>
		/// <param name="numberOfItems">number of computed coordinates</param>
		/// <returns>
		/// Newly created <see cref="CoordinateSet"/>.
		/// </returns>
		/// <remarks>Applies only for numerical axis. For other types of axes an exception is thrown.</remarks>
		public CoordinateSet CreateCoordinateSet(int numberOfItems)
		{
			int numberOfItems2 = Math.Max(1,numberOfItems);

			CoordinateSetComputation csc = new CoordinateSetComputation(this);
			csc.NumberOfValues = numberOfItems2;
			csc.ComputationKind = CoordinatesComputationKind.ByNumberOfPoints;
			CoordinateSet cSet = csc.Value;

			if(numberOfItems <= 0)
			{
				cSet.ValueList = new ArrayList();
				cSet.ValueChanged = false;
			}
			
			return cSet;
		}

		#region --- Handling Coordinate Set Computation Properties ---
		/* ================================		
		 *		NOTES TO THE DEVELOPER
		 * ================================
		 * These parameters are passed to the CoordinateSetComputation member. The properties remain for
		 * backward compatibility to comply with old cs(vb) and aspx code. Properties are not
		 * declared "DesignerSerializationVisibility.Hidden" because that would brake old aspx code.
		 * Instead, 
		 *   1. Values provided in serialization are equal to the default value, so they aren't serialized,
		 *   2. The parameters aren't browsable any more, since they are accessible through the
		 *      "DefaultCoordinateSetComputation" property.
		 */
			
		/// <summary>
		/// Gets or sets minimum number of annotation points for this axis.
		/// </summary>
		/// <remarks>This value is used when default coordinate set is created.
		/// The number of coordinates in the coordinate set belonging to this axis is <b>at least</b> as specified in this property,
		/// but sometimes greater because coordinates take round values when computed by the control.
		/// <para>
		/// Default coordinate set is used for axis annotation, coordinate plane strips and coordinate plane
		/// grids.
		/// </para>
		/// </remarks>
		[DefaultValue(5)]
		[Browsable(false)]
		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int MinimumNumberOfAnnotationPoints 
		{ 
			get 
			{
				if(OwningChart != null && OwningChart.InSerialization)
					return 5;
				else
					return defaultCoordinateSetComputation.NumberOfValues; 
			} 
			set 
			{
				if(defaultCoordinateSetComputation.NumberOfValues != value)
				{
					defaultCoordinateSet = null;
					defaultCoordinateSetComputation.NumberOfValues = value;
				}
			}
		}
				
		[DefaultValue(1.0)]
		[Browsable(false)]
		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double CoordinateStep
		{ 
			get 
			{
				if(OwningChart != null && OwningChart.InSerialization)
					return 1;
				return defaultCoordinateSetComputation.Step; 
			} 
			set { defaultCoordinateSetComputation.Step = value; } 
		}

		[DefaultValue(typeof(DateTimeUnit),"Day")]
		[Browsable(false)]
		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DateTimeUnit DateTimeUnit
		{ 
			get 
			{
				if(OwningChart != null && OwningChart.InSerialization)
					return DateTimeUnit.Day;
				return defaultCoordinateSetComputation.Unit; 
			} 
			set { defaultCoordinateSetComputation.Unit = value; } 
		}
		
		#endregion
		
		/// <summary>
		/// Gets or sets default coordinate set for this axis.
		/// </summary>
		/// <remarks>
		/// Default coordinate set is used for axis annotation, coordinate plane strips and coordinate plane
		/// grids.
		/// </remarks>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CoordinateSet DefaultCoordinateSet
		{
			get
			{
				if(defaultCoordinateSetIsCustom && defaultCoordinateSet != null)
					return defaultCoordinateSet;
				else
				{
					return defaultCoordinateSetComputation.Value;
				}
			}
			set
			{
				defaultCoordinateSet = value;
				defaultCoordinateSetIsCustom = true;
			}
		}
					
		/// <summary>
		/// Gets or sets default coordinate set for this axis.
		/// </summary>
		/// <remarks>
		/// Default coordinate set is used for axis annotation, coordinate plane strips and coordinate plane
		/// grids.
		/// </remarks>
//		[Browsable(false)]
//		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[NotifyParentProperty(true), RefreshProperties(RefreshProperties.All)]
		public CoordinateSetComputation  DefaultCoordinateSetComputation
		{
			set 
			{ defaultCoordinateSetComputation = value; }
			get
			{ return defaultCoordinateSetComputation; }
		}

		#endregion
		
		#region --- Build ---

		internal void DataBind(DataDimension dim)
		{
			dimension = dim;
			if(!genericMinimum.IsNull)
				minValue = genericMinimum.InternalValue;

            DefaultCoordinateSetComputation.Axis = this;
			if(MinValue != MaxValue)
			{
				foreach(AxisAnnotation aa in AxisAnnotations)
					aa.DataBind();
			}
		}

		internal override void Build()
		{
			if(Dimension == null || MinValue == null || MaxValue == null)
				return;
			if(MinValueLCS != MaxValueLCS)
			{
				foreach(AxisAnnotation aa in AxisAnnotations)
					aa.Build();
			}
			else
				AxisAnnotations.Clear();
		}
		#endregion

		#region --- Internal Methods ---

		private void CreateDefaultAnnotation(string name, CoordinatePlane plane, AxisLinePositionKind pKind, bool visible2D, bool visible3D)
		{
			if(axisAnnotations[name] == null)
			{
				AxisAnnotation xa = CreateAnnotation(name,plane,pKind);
				xa.VisibleIn2D = visible2D;
				xa.VisibleIn3D = visible3D;
				xa.HasChanged = false;
			}

		}

		internal void CreateDefaultAnnotation(string seriesName, bool embeded)
		{
			bool is2D = CoordSystem.TargetArea.Mapping.Kind == ProjectionKind.TwoDimensional;
			string sName = "";
			if (embeded)
				sName = seriesName + ".";
			if(this == CoordSystem.XAxis)
			{
				CreateDefaultAnnotation(sName + "X@Zmax", CoordSystem.PlaneZX, AxisLinePositionKind.AtMaximumValue, false,  !embeded);
				CreateDefaultAnnotation(sName + "X@Ymin", CoordSystem.PlaneXY, AxisLinePositionKind.AtMinimumValue, !embeded, false   );
				CreateDefaultAnnotation(sName + "X@Ymax", CoordSystem.PlaneXY, AxisLinePositionKind.AtMaximumValue, false, false);
				CreateDefaultAnnotation(sName + "X@Zmin", CoordSystem.PlaneZX, AxisLinePositionKind.AtMinimumValue, false, false);
			}
			else if(this == CoordSystem.YAxis)
			{
				CreateDefaultAnnotation(sName + "Y@Zmax", CoordSystem.PlaneYZ, AxisLinePositionKind.AtMaximumValue, false,!embeded);
				CreateDefaultAnnotation(sName + "Y@Xmin", CoordSystem.PlaneXY, AxisLinePositionKind.AtMinimumValue, !embeded, false);
				CreateDefaultAnnotation(sName + "Y@Xmax", CoordSystem.PlaneXY, AxisLinePositionKind.AtMaximumValue, embeded, embeded);
				CreateDefaultAnnotation(sName + "Y@Zmin", CoordSystem.PlaneYZ, AxisLinePositionKind.AtMinimumValue, false, false);
			}
			else 
			{
				CreateDefaultAnnotation(sName + "Z@Xmax", CoordSystem.PlaneZX, AxisLinePositionKind.AtMaximumValue, false,
					!embeded
					//CoordSystem.OwningSeries is CompositeSeries &&
					//(CoordSystem.OwningSeries as CompositeSeries).CompositionKind == CompositionKind.Sections &&
					//(CoordSystem.OwningSeries as CompositeSeries).SubSeries.Count > 1
					);
				CreateDefaultAnnotation(sName + "Z@Xmin", CoordSystem.PlaneZX, AxisLinePositionKind.AtMinimumValue, false, false);
				CreateDefaultAnnotation(sName + "Z@Ymax", CoordSystem.PlaneYZ, AxisLinePositionKind.AtMaximumValue, false, false);
				CreateDefaultAnnotation(sName + "Z@Ymin", CoordSystem.PlaneYZ, AxisLinePositionKind.AtMinimumValue, false, false);
			}
		}

		internal void SetOrientation(AxisOrientation orientation)
		{
			this.orientation = orientation;
		}

		internal override void Render()
		{
			if(!Visible)
				return;

			// Render Axis Annotations

			if (this == CoordSystem.ZAxis)
			{
				CompositeSeries owningCompositeSeries;
				if(CoordSystem.OwningSeries is CompositeSeries)
					owningCompositeSeries = CoordSystem.OwningSeries as CompositeSeries;
				else
					owningCompositeSeries = CoordSystem.OwningSeries.OwningSeries; // in case of embeded CS

				if(owningCompositeSeries.CompositionKind != CompositionKind.Sections ||
					owningCompositeSeries.NumberOfSimpleSeries <= 1)
					return;                 
			}

			bool noneRendered = true;
			bool atLeastOneVisible = false;
			AxisAnnotation skippedBecauseOfPlane = null;
			foreach(AxisAnnotation aa in axisAnnotations)
			{

				if(aa.Visible)
				{
					atLeastOneVisible = atLeastOneVisible || aa.Plane != null;
					if (aa.Plane == null || aa.Plane.Visible)
					{
						aa.Render();
						noneRendered = false;
					}
					else
						skippedBecauseOfPlane = aa;

				}
			}

			if(atLeastOneVisible && noneRendered)
			{
				AxisAnnotation replacementAnnotation = ReplacementAnnotation(skippedBecauseOfPlane);
				if(replacementAnnotation != null)
				{
					replacementAnnotation.Visible = true;
					replacementAnnotation.Render();
					replacementAnnotation.Visible = false;
				}
			}
		}

		private AxisAnnotation ReplacementAnnotation(AxisAnnotation aa)
		{
			// Find the non-visible annotation that would replace a visible annotation
			// associated to an invisible plane.
			// The replacement will be rendered in spite of fact that it is designated as invisible

			return null;
		}
		#endregion

		#region --- Protected Methods ---

		private double MapReferenceValue() { return 0; } // TODO: Implement this

		//		internal ArrayList CreateDefaultLabels(int numberOfItems)
		//		{
		//			CoordinateSet cSet = CreateCoordinateSet(numberOfItems);
		//			return cSet.ValueList;
		//		}


		#endregion

		#region --- Serialization ---

		internal bool HasChanged 
		{ 
			get 
			{ 
				if(hasChanged || defaultCoordinateSetComputation.HasChanged)
					return true;
				foreach(AxisAnnotation L in axisAnnotations)
					if(L.HasChanged)
						return true;

				return false; 
			} 
			set { hasChanged = value; } 
		}
		internal bool ShouldSerializeMe { get { return HasChanged; } }

		private bool ShouldSerializeDimension()				{ return false; }
		private bool ShouldSerializeName()					{ return true; }
		private bool ShouldSerializeWorldStartCoordinate()	{ return false; }
		private bool ShouldSerializeWorldEndCoordinate()	{ return false; }

		#endregion
	
	}
}
