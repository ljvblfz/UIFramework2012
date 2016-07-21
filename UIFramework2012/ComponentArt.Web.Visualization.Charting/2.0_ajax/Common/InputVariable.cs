using System;
using System.ComponentModel;
using System.Diagnostics;

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	internal enum InputVariableKind
	{
		SimpleNumeric,
		Index,
		NumericOrdered,
		NumericEquidistant,
		NumericRangeFrom,
		NumericRangeTo,
		SimpleDateTime,
		DateTimeOrdered,
		DateTimeEquidistant,
		Boolean,
		String,
		FinancialOpen,
		FinancialClose,
		FinancialHigh,
		FinancialLow,
		IndexedString,
		SeriesName
	}

	/// <summary>
	/// Specifies the time step for the <see cref="InputVariable"/> object.
	/// </summary>
	public enum TimeStepKind
	{
		Year,
		Month,
		Week,
		Day,
		Hour,
		Minute,
		Second
	}

	/// <summary>
	/// Represents an input variable for the chart.
	/// </summary>
	[TypeConverter(typeof(InputVariableConverter))]
	public class InputVariable : NamedObjectBase
	{
		private static DateTime defaultStart = new DateTime(DateTime.Today.Year,1,1,0,0,0);
		private static DateTime defaultEnd   = new DateTime(DateTime.Today.Year+1,1,1,0,0,0);
		private const double	defaultNumStart = 10.0;
		private const double	defaultNumEnd   = 100.0;
		private const double	defaultNumStep  = 10.0;

		internal static char    seriesNameDelimiter = '|';

		private InputVariableKind	kind = InputVariableKind.SimpleNumeric;
		private int					numberOfPoints = 0;
		// Numeric data
		private double				numStart = defaultNumStart;
		private double				numEnd = defaultNumEnd;
		private double				numStep = defaultNumStep;
		// Time data
		private DateTime			timeEnd = defaultEnd;
		private DateTime			timeStart = defaultStart;
		private TimeStepKind		timeStepUnit = TimeStepKind.Month;
		// String data
		private string[]			strings;
		private string				stringPrefix;

		private int					yIndex = 0;
		private int[]				yIndexList = null;

		private object				valueObject = null;			// object the variable is bound to
		private string				valuePath = null;			// path to the value object within the data source object
		private string				valueExpression = null;		// path to the value object within the data source object
		private object				evaluatedValue = null;

		private bool				createdByControl = false;
		private bool				hasChanged = false;

		// Checking cyclic dependency
		private bool				inEvaluation = false;

		#region --- Construction and Clonning ---
		/// <summary>
		/// Creates a new instance of the <see cref="InputVariable"/> class with default settings.
		/// </summary>
		public InputVariable()
		{ }
		/// <summary>
		/// Creates a new instance of the <see cref="InputVariable"/> class with default settings.
		/// </summary>
		/// <param name="name">The name of this <see cref="InputVariable"/> object.</param>
		public InputVariable(string name) : base(name)
		{
        }

		#endregion

		#region --- Data Binding ---

		internal void ResetEvaluatedValue()
		{
			evaluatedValue = null;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal object ValueObject 
		{ 
			get 
			{
				if(OwningChart.InSerialization)
					return null;
				else
					return valueObject; 
			}
			set { valueObject = value; }
        }

#if !__BUILDING_CRI_DESIGNER__
		/// <summary>
		/// Gets or sets the path of this <see cref="InputVariable"/> object.
		/// </summary>
		/// <remarks>
		/// The path should be in a [Table_name.]Column_name format.
		/// </remarks>
		[Category("Data Binding")]
	    [Description("Path within the data source object")]
		[TypeConverter(typeof(DataSourcePathConverter))]
		[DefaultValue((string)null)]
		[NotifyParentProperty(true)]
		public 
#else
        internal
#endif
            string ValuePath	
		{ 
			get { return valuePath; }
			set 
			{ 
				if (value != valuePath)
				{
					if(value != null && value.ToLower() == "(none)")
						valuePath = null;
					else
						valuePath = value; 
					valueObject = null;
					evaluatedValue = null;
				}
			} 
		}
		
		/// <summary>
		/// Gets or sets the value of this <see cref="InputVariable"/> based on an expression.
		/// </summary>
		[Category("Data Binding")]
		[Description("Expression describing how this variable is computed from other input variables")]
		[NotifyParentProperty(true)]
		[DefaultValue((string)null)]
		public string ValueExpression 
		{
			get { return valueExpression; }
			set 
			{ 
				valueExpression  = value; 
				if(valueExpression != null && valueExpression != "")
					valueObject = new DataExpression(valueExpression);
			} 
		}

#if __BUILDING_CRI_DESIGNER__
        string m_rsExpression = "";
        public string RSExpression
        {
            get { return m_rsExpression; }
            set { m_rsExpression = value; }
        }
#endif

		#endregion

		#region --- Properties ---
		
		internal new ChartBase			OwningChart		{ get { if(OwningCollection == null) return null; return (OwningCollection.Owner as DataProvider).OwningChart; } }

		internal bool CreatedByControl { get { return createdByControl; } set { createdByControl = value; } }
		internal bool HasChanged { get { return hasChanged; } set { hasChanged = value; } }
		
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(InputVariableKind.SimpleNumeric)]
		[Category("Design Time Value Simulation")]
		internal InputVariableKind	VariableKind	{ get { return kind; }				set { kind = value; } }

		[SRDescription("InputVariableNumStartDescr")]
		[DefaultValue(defaultNumStart)]
		//[DesignOnly(true)]
		[Category("Design Time Value Simulation")]
		public double				NumStart		{ get { return numStart; }			set { if(value != defaultNumStart) hasChanged = true; numStart = value; } }

		[SRDescription("InputVariableNumEndDescr")]
		[DefaultValue(defaultNumEnd)]
		//[DesignOnly(true)]
		[Category("Design Time Value Simulation")]
		public double				NumEnd			{ get { return numEnd; }			set { if(value != defaultNumEnd) hasChanged = true; numEnd = value; } }
		
		[SRDescription("InputVariableNumStepDescr")]
		[DefaultValue(defaultNumStep)]
		//[DesignOnly(true)]
		[Category("Design Time Value Simulation")]
		public double				NumStep			{ get { return numStep; }			set { if(value != defaultNumStep) hasChanged = true; numStep = value; } }

		[SRDescription("InputVariableTimeStartDescr")]
		[DesignOnly(true)]
		[Category("Design Time Value Simulation")]
		public DateTime				TimeStart		{ get { return timeStart; }			set { if(value != defaultStart) hasChanged = true; timeStart = value; } }
		
		[SRDescription("InputVariableTimeEndDescr")]
		[DesignOnly(true)]
		[Category("Design Time Value Simulation")]
		public DateTime				TimeEnd			{ get { return timeEnd; }			set { if(value != defaultEnd) hasChanged = true; timeEnd = value; } }
		
		[SRDescription("InputVariableTimeStepDescr")]
		[DesignOnly(true)]
		[Category("Design Time Value Simulation")]
		public double				TimeStep		{ get { return numStep; }			set { if(value != defaultNumStep) hasChanged = true; numStep = value; } }
		
		[SRDescription("InputVariableTimeStepUnitDescr")]
		[DefaultValue(TimeStepKind.Month)]
		[DesignOnly(true)]
		[Category("Design Time Value Simulation")]
		public TimeStepKind			TimeStepUnit	{ get { return timeStepUnit; }		set { if(value != TimeStepKind.Month) hasChanged = true; timeStepUnit = value; } }

		[SRDescription("InputVariableStringsDescr")]
		[DefaultValue(null)]
		[DesignOnly(true)]
		[Category("Design Time Value Simulation")]
		public string[]				Strings			{ get { return strings; }			set { strings = value; ComputeString(); } }
		
        /// <summary>
        /// Gets or sets the prefix of values of the input variable to be used in design time.
        /// </summary>
		[SRDescription("InputVariableStringPrefixDescr")]
		[DefaultValue(null)]
		[DesignOnly(true)]
		[Category("Design Time Value Simulation")]
		public string				StringPrefix	{ get { return stringPrefix; }		set { stringPrefix = value;  ComputeString(); } }

		internal bool IsDefined 
		{
			get { return (valueObject != null) || (valuePath != null && valuePath != "") || (valueExpression != null); }
		}

		#endregion

		#region --- Evaluation ---

		internal int YIndex { get { return yIndex; } set { yIndex = value; } }

		internal object Value { get { Evaluate(); return evaluatedValue; } }

		internal Variable EvaluatedValue 
        { 
            get 
            {
                if (evaluatedValue != null && (evaluatedValue as Variable).Length == 0 && OwningChart.InDesignMode)
                    evaluatedValue = SimulatedValue;
                return evaluatedValue as Variable; 
            }
        }

		internal void FilterEvaluatedValue(BoolVariable filter)
		{
			evaluatedValue = EvaluatedValue.Filter(filter);
		}

		private DataProvider DataProvider { get { return OwningChart.DataProvider; } }

		private int[] YIndexList { get { return yIndexList; } }

		internal int NumberOfPoints
		{
			get { return numberOfPoints; }
			set { numberOfPoints = value; }
		}

		internal void DataBind()
		{
			Evaluate();
		}

		internal void Evaluate()
		{
			if(evaluatedValue != null && !DataProvider.SeriesNameInInputData)
					return;

			// Initial value
			evaluatedValue = valueObject;
			// If null, get initial value from valuePath
			if(evaluatedValue == null && valuePath != null && valuePath != "")
			{
				evaluatedValue = OwningChart.DataProvider.GetObjectFromValuePath(valuePath);
				if(evaluatedValue == null)
				{
                    if (OwningChart.SchemaExists)
                    {
                        OwningChart.RegisterErrorMessage("Input variable '" + Name + "' cannot be evaluated" +
                            "\n   because the value path '" + valuePath + "' is not valid");
                        return;
                    }
				}
			}
			// Try to map it to variable
            Variable v = null;
			if(evaluatedValue != null && !(evaluatedValue is Variable))
				v = Variable.CreateVariable(Name,evaluatedValue);

            if(v != null)
            {
                if (v.Length > 0 || !OwningChart.InDesignMode)
                {
                    evaluatedValue = v;
                }
                else
                {
                    switch (v.ValType)
                    {
                        case Expression.ValueType.Numeric:
                            kind = InputVariableKind.SimpleNumeric;
                            yIndex = GetYIndex();
                            break;
                        case Expression.ValueType.Bool:
                            kind = InputVariableKind.Boolean;
                            break;
                        case Expression.ValueType.DateTime:
                            kind = InputVariableKind.SimpleDateTime;
                            break;
                        case Expression.ValueType.String:
                            if (evaluatedValue is System.Data.DataColumn)
                            {
                                StringPrefix = ((System.Data.DataColumn)evaluatedValue).ColumnName;
                            }
                            kind = InputVariableKind.IndexedString;
                            break;
                        default:
                            break;


                    }
                    evaluatedValue = SimulatedValue;
                }     

			}

			bool valueExpressionExists = (valueExpression != null && valueExpression != "");

			// Try data expression
			DataExpression DE = null;

			if(evaluatedValue is DataExpression)
				DE = evaluatedValue as DataExpression;
			else if(evaluatedValue == null && valueExpressionExists)
				DE = new DataExpression(valueExpression);

			if(DE != null)
			{
				evaluatedValue = DE.Evaluate(OwningChart.InputVariables);
				if(evaluatedValue == null)
				{
					OwningChart.RegisterErrorMessage("Variable '" + Name + "' cannot be evaluated:\n  " + DE.ErrorMessage);
					return;
				}
			}

			// If still null and in design mode, simulate
			if(evaluatedValue == null && OwningChart.InDesignMode)
				evaluatedValue = SimulatedValue;
			
			// Setting proper dimension
			if(evaluatedValue != null && evaluatedValue is Variable)
			{
				DataDimension ddim = OwningChart.DataProvider.GetDimension(Name);
				Variable var = evaluatedValue as Variable;
				if(ddim != null)
					var.Dimension = ddim;
				else if(!var.HasDimension)
					var.CreateDimension();
			}
		}

        int GetYIndex()
        {
            int count = 0;
            for (int i = 0; i < OwningCollection.Count; ++i)
            {
                if (OwningCollection[i] == this)
                    return count;
                else
                {
                    if (OwningChart.InputVariables[i].kind == InputVariableKind.SimpleNumeric)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

		internal Variable SimulatedValue
		{
			get
			{
				if(numberOfPoints <= 0)
					numberOfPoints = OwningChart.NumberOfSimulatedDataPoints;
				switch(kind)
				{
					case InputVariableKind.Boolean:
						return ComputeBoolean();

					case InputVariableKind.Index:
						return ComputeIndex();

					case InputVariableKind.SimpleDateTime:
						return ComputeSimpleDateTime();
					case InputVariableKind.DateTimeEquidistant:
						return ComputeDateTimeEquidistant();
					case InputVariableKind.DateTimeOrdered:
						return ComputeDateTimeOrdered();

					case InputVariableKind.SimpleNumeric:
						return ComputeSimpleNumeric();
					case InputVariableKind.NumericEquidistant:
						return ComputeNumericEquidistant();
					case InputVariableKind.NumericOrdered:
						return ComputeNumericOrdered();
					case InputVariableKind.NumericRangeFrom:
						return ComputeNumericFrom();
					case InputVariableKind.NumericRangeTo:
						return ComputeNumericTo();

					case InputVariableKind.IndexedString:
						return ComputeIndexedString();
					case InputVariableKind.String:
						return ComputeString();

					case InputVariableKind.FinancialOpen:
						return ComputeFinancialValues("open");
					case InputVariableKind.FinancialClose:
						return ComputeFinancialValues("close");
					case InputVariableKind.FinancialLow:
						return ComputeFinancialValues("low");
					case InputVariableKind.FinancialHigh:
						return ComputeFinancialValues("high");
					case InputVariableKind.SeriesName:
						return ComputeSeriesName();


					default: throw new Exception("Implementation: unhandled case");
				}
			}
		}

		#region --- Simulating Series Name ---

		internal Variable ComputeSeriesName()
		{
			Series[] seriesList = OwningChart.Series.SimpleSubseriesList;
			int numberOfPoints = OwningChart.NumberOfSimulatedDataPoints;
			string[] serNames = new string[numberOfPoints*seriesList.Length];
			yIndexList = new int[numberOfPoints*seriesList.Length];
			int k = 0;
			for(int p = 0; p<numberOfPoints ; p++)
			{
				for(int s = 0; s<seriesList.Length; s++)
				{
					serNames[k] = seriesList[s].Name;
					yIndexList[k] = s;
					k++;
				}
			}

			return new StringVariable(this.Name,serNames);
		}

		#endregion

		#region --- Simulating Numeric Variables ---

		internal static double[][] yy = new double[][]
							{
								new double[] {0.79, 0.99, 0.82, 0.59, 0.47},
								new double[] {0.51, 0.45, 0.60, 0.40, 0.26},
								new double[] {0.32, 0.30, 0.38, 0.25, 0.15},
								new double[] {0.05, 0.20, 0.15, 0.01, 0.10}
							};


		internal Variable  ComputeSimpleNumeric()
		{
			double[] v;
			int hash = Name.GetHashCode()+2;
			Random rnd = new Random(hash);
			double d = NumEnd - NumStart;
			int[] yIndices = null;
			if(DataProvider.SeriesNameInInputData)
			{
				yIndices = DataProvider.InputVariables["series"].YIndexList;
				v = new double[yIndices.Length];
			}
			else
				v = new double[numberOfPoints];
			

			for(int i=0;i<v.Length;i++)
			{
				int yIndex = this.yIndex;
				if(yIndices != null)
					yIndex = yIndices[i];
				double a;
				if(yIndex<yy.Length && i<yy[0].Length)
					a = yy[yIndex][i];
				else
				{
					double dd = 1 + (0.2*(i-1)*(i-1-0.3*yIndex) + 0.8*yIndex*yIndex);
					double y = 1.0/dd;
					// Add a random flavour
					double s = (i+yIndex)*0.1;
					s = s*s*s;
					s = 1/(1+s);
					y = s*y + (1-s)*rnd.NextDouble();
					// Fix to get 2 dec digits only
					a = (int)(y*100)*0.01;
				}
				v[i] = NumStart + a*d;
			}

			return new NumericVariable(Name,v);
		}

		internal Variable  ComputeNumericEquidistant()
		{
			double[] v = new double[numberOfPoints];
			v[0] = NumStart;
			for(int i=1;i<numberOfPoints;i++)
				v[i] = v[i-1] + NumStep;

			return new NumericVariable(Name,v);
		}

		internal Variable  ComputeNumericOrdered()
		{
			double[] v = new double[numberOfPoints];
			Random rnd = new Random(Name.GetHashCode());
			double d = NumEnd - NumStart;
			v[0] = 0;
			for(int i=1;i<numberOfPoints;i++)
				v[i] = v[i-1] + rnd.NextDouble() + 0.1;
			for(int i=0;i<numberOfPoints;i++)
				v[i] = NumStart + v[i]/v[numberOfPoints-1]*d;

			return new NumericVariable(Name,v);
		}

		internal Variable ComputeIndex()
		{
			if(DataProvider.SeriesNameInInputData)
			{
				string[] serNames = (DataProvider.InputVariables["series"].Value as StringVariable).Value;
				string[] distinctNames = new string[serNames.Length];
				int[] serLen = new int[serNames.Length];
				double[] serIndex = new double[serNames.Length];
				int ndn = 0;
				for(int i=0; i<serNames.Length; i++)
				{
					bool found = false;
					for(int j=0; j<ndn; j++)
					{
						if(serNames[i] == distinctNames[j])
						{
							found = true;
							serIndex[i] = serLen[j];
							serLen[j] ++;
							break;
						}
					}
				
					if(!found)
					{
						distinctNames[ndn] = serNames[i];
						serLen[ndn] = 1;
						ndn++;
					}
				}
				Variable var = new NumericVariable(Name,serIndex);
				var.Dimension = DataDimension.StandardIndexDimension;
				return var;
			}
			else
			{
				double[] v = new double[numberOfPoints];
				for(int i=0;i<numberOfPoints;i++)
					v[i] = i;
				Variable var = new NumericVariable(Name,v);
				var.Dimension = DataDimension.StandardIndexDimension;
				return var;
			}
		}

		
		private Variable ComputeNumericFrom()
		{
			int nPoints = numberOfPoints;

			double val0=NumStart,val1=NumEnd;
			val0 *= 0.8;
			val1 *= 0.8;
			double[] val = new double[nPoints];
			Random rnd = new Random(6543);
			for(int i=0;i<nPoints;i++)
			{
				double f = rnd.Next(100)*0.01;
				val[i] = f*val0 + (1.0-f)*val1; 
			}
			return new NumericVariable(Name,val);
		}

		private Variable ComputeNumericTo()
		{
			int nPoints = numberOfPoints;

			double val0=NumStart,val1=NumEnd;
			val0 *= 0.8;
			val1 *= 0.8;
			double[] val = new double[nPoints];
			Random rnd = new Random(6543);
			Random rnd1 = new Random(4321);
			for(int i=0;i<nPoints;i++)
			{
				double f = rnd.Next(100)*0.01;
				double from = f*val0 + (1.0-f)*val1; 
				double f1 = rnd1.Next(100)*0.01;
				val[i] = f1*from + (1.0-f1)*val1;
			}
			return new NumericVariable(Name,val);
		}

		#endregion

		#region --- Simulating Date/Time Variables ---

		internal Variable ComputeDateTimeEquidistant()
		{
			DateTime[] v = new DateTime[numberOfPoints];
			v[0] = TimeStart;
			for(int i=1;i<numberOfPoints;i++)
			{
				switch(TimeStepUnit)
				{
					case TimeStepKind.Year:		v[i] = v[i-1].AddYears	((int)TimeStep); break;
					case TimeStepKind.Month:	v[i] = v[i-1].AddMonths	((int)TimeStep); break;
					case TimeStepKind.Week:		v[i] = v[i-1].AddDays	((int)TimeStep*7); break;
					case TimeStepKind.Day:		v[i] = v[i-1].AddDays	((int)TimeStep); break;
					case TimeStepKind.Hour:		v[i] = v[i-1].AddHours	((int)TimeStep); break;
					case TimeStepKind.Minute:	v[i] = v[i-1].AddMinutes((int)TimeStep); break;
					case TimeStepKind.Second:	v[i] = v[i-1].AddSeconds((int)TimeStep); break;
					default: throw new Exception("Implementation: Unhandled case");
				}
			}
			return new DateTimeVariable(Name,v);
		}

		internal Variable ComputeDateTimeOrdered()
		{
			DateTime[] v = new DateTime[numberOfPoints];
			double[] x = new Double[numberOfPoints];
			Random rnd = new Random(Name.GetHashCode());
			x[0] = 0;
			for(int i=1;i<numberOfPoints;i++)
				x[i] = x[i-1] + rnd.NextDouble() + 0.1;
			v[0] = TimeStart;
			TimeSpan ts = (TimeEnd - TimeStart);
			double seconds = ts.TotalSeconds;
			for(int i=1;i<numberOfPoints;i++)
			{
				double a = x[i]/x[numberOfPoints-1];
				int s = (int)(a*seconds);
				v[i] = v[0].AddSeconds(s);
			}
			v[numberOfPoints-1] = TimeEnd;

			return new DateTimeVariable(Name,v);
		}

		internal Variable ComputeSimpleDateTime()
		{
			DateTime[] v = new DateTime[numberOfPoints];
			Random rnd = new Random(Name.GetHashCode());
			TimeSpan ts = (TimeEnd - TimeStart);
			double seconds = ts.TotalSeconds;
            v[0] = TimeStart;
			for(int i=1;i<numberOfPoints;i++)
			{
				double a = rnd.NextDouble();
				int s = (int)(a*seconds);
				v[i] = v[0].AddSeconds(s);
			}

			return new DateTimeVariable(Name,v);
		}

		#endregion

		#region --- Simulating String Variables ---

		internal Variable ComputeIndexedString()
		{
			if(OwningChart == null)
				return null;
			string[] v = new string[numberOfPoints];
			string p = StringPrefix;
			if(p == null || p == "")
				p = Name;
			for(int i=0;i<numberOfPoints;i++)
				v[i] = p + (i+1).ToString("00");

			Variable var = new StringVariable(Name,v);
			OwningChart.DataDimensions.Remove("Dimension " + Name);
			OwningChart.DataDimensions.Add(var.Dimension);
			return var;
		}

		internal Variable  ComputeString()
		{
			if(OwningChart == null)
				return null;
			if(strings == null || strings.Length < numberOfPoints)
				return ComputeIndexedString();

			Variable var = new StringVariable(Name,strings);
			OwningChart.DataDimensions.Remove("Dimension " + Name);
			OwningChart.DataDimensions.Add(var.Dimension);
			return var;
		}

		#endregion

		#region --- Simulating Boolean Variables ---

		internal Variable ComputeBoolean()
		{
			bool[] v = new bool[numberOfPoints];
			Random rnd = new Random(Name.GetHashCode());
			for(int i=0;i<numberOfPoints;i++)
			{
				v[i] = rnd.NextDouble()>0.5;
			}
			return new BoolVariable(Name,v);
		}

		#endregion

		#region --- Simulating Financial Variables ---

		private Variable ComputeFinancialValues(string whichOne)
		{
			int nPoints = numberOfPoints;;
			double[] valS = new Double[nPoints];

			double val0, val1;

			val0=NumStart;
			val1=NumEnd;

			double min = double.MaxValue;
			double max = double.MinValue;
			Random rnd = new Random(3456);

			// central value

			for(int i=0;i<nPoints;i++)
			{
				double x = (2.0*i)/nPoints;
				double a = Math.Sin(x) + 0.5*Math.Cos(2*x) + 0.25*Math.Sin(4*x)+0.1*Math.Cos(8*x);
				valS[i] = a;
				min = Math.Min(min,a);
				max = Math.Max(max,a);
			}
			double d = max-min;
			for(int i=0;i<nPoints;i++)
			{
				double a = (valS[i]-min)/(max-min);
				valS[i] = a*val1 + (1-a)*val0;
				valS[i] *= (1-rnd.NextDouble()*0.10);
			}

			double high,low, open,close;

			for(int i=0;i<nPoints;i++)
			{
				high = valS[i];
				low = (0.9 - rnd.NextDouble()*0.1)*valS[i];
				double a = rnd.NextDouble();
				open = a*high + (1-a)*low;
				a = rnd.NextDouble();
				close = a*high + (1-a)*low;
				if(whichOne == "high")
					valS[i] = high;
				else if(whichOne == "low")
					valS[i] = low;
				else if(whichOne == "open")
					valS[i] = open;
				else if(whichOne == "close")
					valS[i] = close;
			}
			return new NumericVariable(Name,valS);
		}

		#endregion

		#endregion

		#region --- Browsing Control ---
		
		private bool ShouldBrowseNumStart()		{ return kind != InputVariableKind.Boolean && kind != InputVariableKind.DateTimeEquidistant && kind != InputVariableKind.DateTimeOrdered && kind != InputVariableKind.SimpleDateTime && kind != InputVariableKind.IndexedString && kind != InputVariableKind.String; }
		private bool ShouldBrowseNumEnd()		{ return ShouldBrowseNumStart(); }
		private bool ShouldBrowseNumStep()		{ return kind == InputVariableKind.NumericEquidistant; }
		private bool ShouldBrowseTimeStart()	{ return kind == InputVariableKind.DateTimeEquidistant || kind == InputVariableKind.DateTimeOrdered || kind == InputVariableKind.SimpleDateTime; }
		private bool ShouldBrowseTimeEnd()		{ return ShouldBrowseTimeStart(); }
		private bool ShouldBrowseTimeStep()		{ return kind == InputVariableKind.DateTimeEquidistant; }
		private bool ShouldBrowseTimeStepUnit() { return kind == InputVariableKind.DateTimeEquidistant; }
		private bool ShouldBrowseStrings()		{ return kind == InputVariableKind.String; }
		private bool ShouldBrowseStringPrefix()	{ return kind == InputVariableKind.IndexedString; }

		#endregion
	}
}


