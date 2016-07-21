using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	// ==============================================================================================
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	[Serializable]
	internal class DataDescriptor : NamedObjectBase
	{
		private Type			valueType;
		private bool			required = false;
		private Variable		varValue;
		private bool			hasChanged = false;
		private InputVariableKind inputVariableKind = InputVariableKind.SimpleNumeric;
		private int				yIndex;

		#region --- Constructors ---
		public DataDescriptor() { }
		public DataDescriptor(string name) : base(name) 
		{ }
		public DataDescriptor(string name, string dimensionName, bool required) : base(name)
		{
			this.required = required;
		}
		public DataDescriptor(string name, Type valueType) : this(name,valueType,true) {}

		internal DataDescriptor(string name, Type valueType, bool required) 
			: this(name,"",required) { this.valueType = valueType; }
		#endregion
		#region --- Properties ---

		internal bool			HasChanged		{ get { return hasChanged; }	set { hasChanged = value;  }  }
		public Type				ValueType		{ get { return valueType; }		set { if(valueType != value) hasChanged = true; valueType = value; } }
        public bool Required { get { return required; } set { required = value; } }	

		internal void SetRequired(bool req)
		{ 
			if(required != req) hasChanged = true; required = req; 
		} 

		internal DataDescriptorCollection OwningCollection { get { return base.OwningCollection as DataDescriptorCollection; } }
		[DefaultValue(5)]
		internal new ChartBase		OwningChart		{ get { return OwningCollection == null ? null : OwningCollection.OwningChart; } }
		internal InputVariableKind InputVariableKind { get { return inputVariableKind; } }
		internal int			YIndex { get { return yIndex; } }
		internal Variable		Value
		{
			get 
			{
				return varValue; 
			}
			set 
			{ 
				varValue = value; 
			}
		}

		internal static InputVariableKind VariableKind(string variableName)
		{
			InputVariableKind inputVariableKind;
			if(variableName == "x")
				inputVariableKind = InputVariableKind.Index;
			else if(variableName == "from")
				inputVariableKind = InputVariableKind.NumericRangeFrom;
			else if(variableName == "to")
				inputVariableKind = InputVariableKind.NumericRangeTo;
			else if(variableName == "open")
				inputVariableKind = InputVariableKind.FinancialOpen;
			else if(variableName == "close")
				inputVariableKind = InputVariableKind.FinancialClose;
			else if(variableName == "high")
				inputVariableKind = InputVariableKind.FinancialHigh;
			else if(variableName == "low")
				inputVariableKind = InputVariableKind.FinancialLow;
			else if(variableName == "series")
				inputVariableKind = InputVariableKind.SeriesName;
			else
				inputVariableKind = InputVariableKind.SimpleNumeric;	
			return inputVariableKind;
		}
		
		internal void EvaluateValue()
		{
			Series ser = OwningCollection.Owner as Series;

			if(OwningChart.DataProvider.SeriesNameInInputData)
			{
				InputVariable iVar = OwningChart.DataProvider.InputVariables[Name];
				if(iVar == null)
				{
					string[] parts = (OwningCollection.Owner as Series).Name.Split(InputVariable.seriesNameDelimiter);
					if(parts.Length == 2)
					{
						string iVarName = parts[1] + ":" + Name;
						iVar = OwningChart.DataProvider.InputVariables[iVarName];
					}
				}
				if(iVar == null)
				{
					varValue = null;
					return;
				}
				StringVariable seriesNames = OwningChart.DataProvider.InputVariables["series"].EvaluatedValue as StringVariable;
				seriesNames = seriesNames.Trim();

				Variable varValue1 = iVar.EvaluatedValue;
				if(varValue1.Length != seriesNames.Length && OwningChart.InDesignMode)
				{
					// varValue1 is simulated before the "series" variable is set, so the length is 
					// not compatible with the "series" variable. Recompute.
					varValue1 = iVar.Value as Variable;
				}
				if(varValue1 is StringVariable)
					varValue1 = (varValue1 as StringVariable).Trim();
				string[] nameParts = ser.Name.Split(InputVariable.seriesNameDelimiter);
				string serName = nameParts[0];
				Expression filter = new Expression(Expression.OpKind.Eq,seriesNames,new StringVariable("name",serName));
				varValue = varValue1.Filter(filter.Value as BoolVariable);
				varValue.Dimension = varValue1.Dimension;
#if DEBUG_
				Debug.Write(ser.Name + ":" + Name + "[" + varValue.Dimension + "] = ");
				for(int k=0;k<varValue.Length;k++)
					Debug.Write(varValue.ItemAt(k).ToString() + " ");
				Debug.WriteLine(" ");
#endif
				return;
			}
			int seqN = ser.OwningSeries.GetSequenceNumber(ser);
			varValue = null;
			
			// Compute the input variable kind in design mode

			inputVariableKind = VariableKind(Name);

			// Series index

			yIndex = OwningChart.Series.GetSequenceNumber(OwningCollection.Owner as Series);

			// Looking for the input variable

			InputVariable inVar = null;

			// Try with seriesName:paramName combination

			inVar = OwningChart.InputVariables[ser.Name + ":" + Name];
			// For the first series we may try without seriesName prefix,
			// unless the series name is input data
			if(inVar == null && (seqN == 0 || Name == "x") && !OwningChart.DataProvider.SeriesNameInInputData)
				inVar = OwningChart.InputVariables[Name];

            // If found, return
            if (inVar != null)
            {
                varValue = inVar.EvaluatedValue;
				if (Name == "x" && varValue != null && varValue.Dimension == DataDimension.StandardNumericDimension) 
					// x-coordinate has numeric dimension.
					// If all values are integral, we'll switch to index dimension
				{
					NumericVariable numVar = varValue.Value as NumericVariable;
					if((numVar).HasIntegralValues())
					{
						double minV = numVar[0];
						double maxV = numVar[0];
						for(int i=0; i<numVar.Length; i++)
						{
							minV = Math.Min(minV,numVar[i]);
							maxV = Math.Max(maxV,numVar[i]);
						}
						// If most points aren't missing...
						if((maxV-minV)/numVar.Length < 2)
							varValue.Dimension = DataDimension.StandardIndexDimension;
					}
				}
                return;
            }

			// We'll create missing required input variable if in design mode
			if(inVar == null && Required && OwningChart != null && OwningChart.InDesignMode)
			{
				inVar = new InputVariable(ser.Name + ":" + Name);
				OwningChart.InputVariables.Add(inVar);

				inVar.VariableKind = inputVariableKind;
				inVar.YIndex = yIndex;
				inVar.Evaluate();
			}
			// If x-coordinate, get default
			if(inVar == null && Name == "x")
			{
				varValue = OwningChart.DataProvider.Evaluate("x");
				return;
			}
			if(inVar != null)
			{
				inVar.YIndex = yIndex;
                inVar.VariableKind = inputVariableKind;
                varValue = inVar.EvaluatedValue;
				if (Name == "x" && Required && varValue != null && varValue.Dimension == DataDimension.StandardNumericDimension) // x-coordinate has numeric dimension
					varValue.Dimension = DataDimension.StandardIndexDimension;
				inVar.CreatedByControl = true;
			}
		}

		#endregion
		#region --- Queries ---

		internal double Coordinate(int i)
		{
			Variable v = Value;
			if(i>=v.Length)
				throw new Exception("Value index " + i + " out of range for variable '" + v.Name + "'");
			return v.Dimension.Coordinate(v.ItemAt(i));
		}
		internal double Width(int i)
		{
			Variable v = Value;
			if(i>=v.Length)
				throw new Exception("Value index " + i + " out of range for variable '" + v.Name + "'");
			return v.Dimension.Width(v.ItemAt(i));
		}

		#endregion
		#region --- Browsing and Serialization Control ---
		// Browsing control
		private bool ShouldBrowseValueType()			{ return false; }
		private bool ShouldBrowseDimension()			{ return false; }
		// Serialization control
		private bool ShouldSerializeValueType()			{ return false; }
		private bool ShouldSerializeDimension()			{ return false; }
		#endregion
		#region --- Parameter Computation ---

		#endregion
		#region --- Internal Methods ---

		internal object this[int i]
		{
			get
			{
				if(i==0)
					ComputeVariable();

				if(varValue != null)
					return varValue.ItemAt(i);
				else
					return null;
			}
		}

		internal Variable ComputeVariable()
		{
			varValue = Value;
			return varValue;
		}

		#endregion
	}
}
