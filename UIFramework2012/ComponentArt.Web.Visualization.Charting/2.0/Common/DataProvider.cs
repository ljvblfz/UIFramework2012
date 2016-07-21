using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
#if __COMPILING_FOR_2_0_AND_ABOVE__ && __BuildingWebChart__
using System.Web.UI;
#endif
#if __BuildingWebChart__

#endif

namespace ComponentArt.Web.Visualization.Charting
{
	internal class DataProvider
	{
		protected string errorMessage;
		protected InputVariableCollection inputVars;
		private Hashtable	availableDimensions = new Hashtable();
		private ChartBase		owningChart = null;
		private object		dataSource = null;
		private Hashtable	dataItems = null;
		private bool		dataSourceAnalysed = false;
		private bool		useRealDataInDesignTime = false;

		private int			topNumber = 0;
		private string		selectClause = null;
		private string		orderClause = null;

        private bool doDefaultDataBind = true;

		#region --- Construction and Setup ---
		internal DataProvider()
		{
			inputVars = new InputVariableCollection(this);
			dataItems = new Hashtable();
		}

		internal ChartBase OwningChart { get { return owningChart; } set { owningChart = value; } }

		#endregion

		#region --- Input variables ---

		internal InputVariableCollection InputVariables 
		{
			get 
			{ 
				return inputVars; 
			} 
		}

        internal void RenameSeries(string oldName, string newName)
        {
            foreach (InputVariable iv in inputVars)
            {
                string[] parts = iv.Name.Split(':');
                if (parts[0] == oldName)
                {
                    if (parts.Length > 1)
                        iv.Name = newName + ":" + parts[1];
                    else
                        iv.Name = newName;
                }
            }
        }

        internal void RemoveSeriesVariables(SeriesBase seriesBase)
        {
            if (seriesBase is CompositeSeries)
            {
                foreach (SeriesBase child in ((CompositeSeries)seriesBase).SubSeries)
                {
                    RemoveSeriesVariables(child);
                }
            }

            string seriesName = seriesBase.Name;
            ArrayList toRemove = new ArrayList();
            foreach (InputVariable iv in inputVars)
            {
                string[] parts = iv.Name.Split(':');
                if (parts[0] == seriesName && parts.Length>1)
                    toRemove.Add(iv);
            }
            foreach (InputVariable iv in toRemove)
                inputVars.Remove(iv);
            
        }

        internal bool DoDefaultDataBindToDatabase { get { return doDefaultDataBind; } set { doDefaultDataBind = value; } }

		internal bool SeriesNameInInputData
		{
			get
			{
				return inputVars["series"] != null;
			}
			set
			{
				if(value)
				{
					if(inputVars["series"] == null)
					{
						InputVariable serVar = new InputVariable("series");
						serVar.VariableKind = InputVariableKind.SeriesName;
						inputVars.Add(serVar);
						ArrayList toBeRemoved = new ArrayList();
						foreach(InputVariable iv in inputVars)
						{
							if(iv.Name.IndexOf(":")>=0)
								toBeRemoved.Add(iv);
						}
						foreach(InputVariable iv in toBeRemoved)
							inputVars.Remove(iv);
					}
				}
				else
				{
					if(inputVars["series"] != null)
					{
						ArrayList toBeRemoved = new ArrayList();
//						toBeRemoved.Add(inputVars["series"]);
						foreach(InputVariable iv in inputVars)
						{
							if(iv.Name.IndexOf(":")<0 )// && iv.CreatedByControl)
								toBeRemoved.Add(iv);
						}
						foreach(InputVariable iv in toBeRemoved)
							inputVars.Remove(iv);
					}
				}
			}
		}

		#endregion

		#region --- DataBinding ---

		internal void DataBind(string name, object obj)
		{
			InputVariable iv = inputVars[name];
			if( iv == null)
			{
				iv = new InputVariable(name);
				inputVars.Add(iv);
			}
			iv.ValueObject = obj;
			AdjustDimension(iv);
			if(obj is double[] || (obj is DataColumn && (obj as DataColumn).DataType == typeof(System.Double)) )
				DimensionBind(name,new NumericDataDimension(name));
		}

        internal void DefineValuePath(string name, string valuePath)
        {
            InputVariable iv = inputVars[name];
            if (iv == null)
            {
                iv = new InputVariable(name);
                inputVars.Add(iv);
            }
            iv.ValuePath = valuePath;
        }

        #if __BUILDING_CRI_DESIGNER__

        internal void DefineAsRSExpression(string name, string rsExpression)
        {
            InputVariable iv = inputVars[name];
            if (iv == null)
            {
                iv = new InputVariable(name);
                inputVars.Add(iv);
            }
            iv.ValuePath = null;
            iv.ValueObject = null;
            iv.RSExpression = rsExpression;
        }
        #endif
		private void AdjustDimension(InputVariable inVar)
		{
			object obj = inVar.ValueObject;
			if(obj == null)
				return;
			if(obj is double[] || (obj is DataColumn && (obj as DataColumn).DataType == typeof(System.Double)) )
				DimensionBind(inVar.Name,new NumericDataDimension(inVar.Name));
		}

		internal void DimensionBind(string name, DataDimension dimension)
		{
			availableDimensions.Remove(name);
			availableDimensions.Add(name,dimension);
		}

		internal DataDimension GetDimension(string name)
		{
			return (DataDimension)(availableDimensions[name]);
		}

		internal object DataSource
		{
			get { return dataSource; }
			set
			{
				if(dataSource != value)
				{
					dataSource = value;
                    ResetValuePaths();
				}
			}
		}

        internal void ResetValuePaths()
        {
            dataSourceAnalysed = false;
            foreach (InputVariable inVar in inputVars)
            {
                if (inVar.ValueObject != null && !(inVar.ValueObject is DataExpression))
                    inVar.ValueObject = null;
                if ((dataSource == null || OwningChart.InDesignMode) && !OwningChart.FreezeValuePath)
                    inVar.ValuePath = "";
            }
        }

		private void CleanInputVariables()
		{
			// Remove all unused variables

//			if(SeriesNameInInputData)
//				return;
			ArrayList toBeRemoved = new ArrayList();
			foreach(InputVariable iv in inputVars)
			{
				iv.ResetEvaluatedValue();
				if(iv.Name.IndexOf(":")>0)
				{
					string[] parts = iv.Name.Split(':');
					parts[0] = parts[0].Trim();
					parts[1] = parts[1].Trim();
					// check if the series exist
					Series s = OwningChart.Series.FindSeries(parts[0]);
					if(s != null && 
						!s.UsesParameter(parts[1]) && 
						!parts[1].ToLower().StartsWith("role?") && 
						parts[0].ToLower() != "series")
						toBeRemoved.Add(iv);
				}
			}
			foreach(InputVariable iv in toBeRemoved)
				inputVars.Remove(iv);
		}

		internal object GetObjectFromValuePath(string valuePath)
		{
			return dataItems[valuePath];
		}

		internal void DataBind()
		{
			AnalyseDataSource();
			CleanInputVariables();

			// Check if default data binding is needed, i.e. if 
			//  (1) dataSource is database type, and
			//  (2) there is no mapping between variables and columns

			Series[] series = OwningChart.Series.SimpleSubseriesList;
			if(dataItems.Count > 0 && NumberOfBoundVariables() == 0 && series.Length > 0)
			{
				DefaultDataBindingToDatabase(series);
			}			

			if(SeriesNameInInputData)
				ProcessIfSeriesDefinedAsInputVariable();
			else
				SmartDataBinding();

			foreach(InputVariable inVar in inputVars)
				inVar.DataBind();

			Filter();

			// Handle the case of series defined as input variable

		}

		internal string SelectClause	{ get { return selectClause; } set { selectClause = value; } }
		internal string OrderClause		{ get { return orderClause; } set { orderClause = value; } }
		internal int    TopNumber		{ get { return topNumber; } set { topNumber = value; } }
		
		internal void ProcessIfSeriesDefinedAsInputVariable()
		{
			if(!SeriesNameInInputData)
				return;

			InputVariable ivSeries = InputVariables["series"];

			StringVariable serVar = ivSeries.Value as StringVariable;
			string[] serStrings = serVar.Value;

			// Create the series info
			int nSeries = 0;
			string[] serNames = new string[serStrings.Length];

			for(int i=0; i<serStrings.Length; i++)
			{
				// Find ix such that serNames[ix] = serStrings[i];
				int ix = -1;
				for(int j=0; j<nSeries; j++)
				{
					if(serNames[j] == serStrings[i])
					{
						ix = j;
						break;
					}
				}
				// Not found, so we'll add a new series member 
				if(ix < 0)
				{
					ix = nSeries;
					serNames[ix] = serStrings[i].Trim();
					nSeries++;
				}
			}

			Series[] originalSeries = OwningChart.Series.SimpleSubseriesList;

			// Each original derives 'nSeries' series driven by the list of names in the "series" 
			// input variables. Derived series belong to the same parent as the original series
			// and they are clones of the original series (to retain properties defined in original
			// series at design time).
			// When derived series are created, the originals are removed
			//
			foreach(Series org in originalSeries)
			{
				CompositeSeries parent = org.OwningSeries;
				bool removeParentName = org.RemoveParentNameFromLabel;
				parent.SubSeries.Remove(org);
				org.OwningCollection = null; // ... to avoid removal of the original input variable
				string orgName = org.Name;
				for(int xName=0; xName<nSeries; xName++)
				{
					Series clone = (Series)(new ChartCloner()).Clone(org);
					clone.Name = "";
					clone.Name = serNames[xName] + InputVariable.seriesNameDelimiter + orgName;
					if(removeParentName)
						clone.Label = serNames[xName];
					else
						clone.Label = serNames[xName] + " " + orgName;
					
					parent.SubSeries.Add(clone);
				}
			}

			return;
		}

		internal void Filter()
		{
			InputVariable filter = inputVars["filter"];
			if(filter == null)
				return;
			BoolVariable filterVar = filter.EvaluatedValue as BoolVariable;
			foreach(InputVariable var in inputVars)
			{
				if(var != filter)
					var.FilterEvaluatedValue(filterVar);
			}
		}

		internal int NumberOfBoundVariables()
		{
			int n = 0;
			foreach(InputVariable iv in inputVars)
				if(iv.ValuePath != "" && iv.ValuePath != null)
					n++;
			return n;
		}

		internal bool UseRealDataAtDesignTime { get { return true; } }

		internal void DefineAsExpression(string name, string expression)
		{
			DataBind(name,new DataExpression(expression));
		}

		private bool SmartDataBinding()
		{
			// No smart data binding in design time
			if(OwningChart.InDesignMode)
				return false;
			// No smart data binding if data source is not defined
			if(dataSource == null)
				return false;
			// No smart data binding if at least one input variable is bound
			foreach(InputVariable inVar in inputVars)
				if(inVar.ValueObject != null || inVar.ValuePath != null)
					return false;

			// Find the candidates for x and y variables
			
			object xCandidate = null;
			object[] yCandidates = new object[dataItems.Count];
			int nyc = 0;
			
			IDictionaryEnumerator en = dataItems.GetEnumerator();
			while(en.MoveNext())
			{
				DataColumn column = en.Value as DataColumn;
				Type itemType = column.DataType;
				if(xCandidate == null && IsValidXType(itemType))
				{
					xCandidate = en.Key;
				}
				else if(IsValidYType(itemType) && column.ColumnName != "ID")
				{
					yCandidates[nyc] = en.Key;
					nyc ++;
				}
			}

			// Do data binding

			int yIndex = 0;
			string[] pars = new string[] { "y","from","to","open","low","close","high" };
			foreach(InputVariable inVar in inputVars)
			{
				string[] parts = inVar.Name.Split(':');
				if(parts.Length == 2)
				{
					if(parts[1] == "x")
						inVar.ValuePath = (string)xCandidate;
					else if(yIndex < yCandidates.Length)
					{
						for(int i=0; i<pars.Length; i++)
						{
							if(pars[i] == parts[1])
							{
								inVar.ValuePath = (string)yCandidates[yIndex];
								yIndex++;
								break;
							}
						}
					}				
				}
			}
			return true;
		}

		private bool IsValidXType(Type type)
		{
			return 
				type == typeof(System.String) ||
				type == typeof(System.DateTime);
		}

		private bool IsValidYType(Type type)
		{
			return 
				type == typeof(System.Double) ||
				type == typeof(System.Int32);
		}

		#endregion

		#region --- DataSource Analysis ---

		
		private void AnalyseDataSource()
		{
			if(dataSourceAnalysed)
				return;

			dataItems.Clear();

			if(dataSource == null)
				return;

			useRealDataInDesignTime = false;

			if(dataSource is DataSet)
				AnalyseDataSource(dataSource as DataSet,"");
			else if(dataSource is DataTable)
			{
				DataTable table = dataSource as DataTable;
				AnalyseDataSource(ref table,"");
				dataSource = table;
			}
			else if(dataSource is IDataAdapter)
				AnalyseDataSource(dataSource as IDataAdapter,"");
			else if(dataSource is IDbCommand)
				AnalyseDataSource(dataSource as IDbCommand,"");
#if __BUILDING_CRI_DESIGNER__
            else if (dataSource is string)
                AnalyseDataSource(dataSource as string, "");
#endif
			else if (dataSource is IEnumerable)
				AnalyseIEnumerableDataSource();
			else
			{
				throw new ArgumentException("Cannot bind to data type '" + dataSource.GetType().Name + "'");
			}
		}

        private void AnalyseIEnumerableDataSource()
        {
            DataTable dt = new DataTable();

			ICollection propertyDescriptors = null;
			IEnumerable ie = (IEnumerable)dataSource;

            IEnumerator ietor = ie.GetEnumerator();

			bool first_item_was_read = false;

			// Iterate through the data
            while (ietor.MoveNext())
            {
                ArrayList al = new ArrayList();

				object cur_item = ietor.Current;

				if (!first_item_was_read) 
				{
					// Discover what columns we have.
					propertyDescriptors = GetColumnsOfDataSource(cur_item);

					// Create rows in the DataTable.
					foreach (PropertyDescriptor pd in propertyDescriptors)
					{
						dt.Columns.Add(pd.Name, pd.PropertyType);
					}

					first_item_was_read = true;
				}

				// Get the items.
                foreach (PropertyDescriptor pd in propertyDescriptors)
                {
                    al.Add(pd.GetValue(cur_item));
                }

				// Add a row.
                dt.Rows.Add(al.ToArray());
            }

            if (!first_item_was_read)
            {
                // Discover what columns we have.
                propertyDescriptors = GetColumnsOfDataSource(null);

                // Create rows in the DataTable.
                foreach (PropertyDescriptor pd in propertyDescriptors)
                {
                    dt.Columns.Add(pd.Name, pd.PropertyType);
                }
            }

			// Pass the data as datatable.
            AnalyseDataSource(ref dt, "");
        }

        private PropertyDescriptorCollection GetColumnsOfDataSource(object firstItem)
        {

            if (dataSource == null)
            {
                return null;
            }
            PropertyDescriptorCollection pdc = null;

            if ((dataSource != null) && (dataSource is ITypedList))
                pdc = ((ITypedList)this.dataSource).GetItemProperties(new PropertyDescriptor[0]);

            if (pdc == null)
            {
                Type itemType = null;
                PropertyInfo itemProperty = dataSource.GetType().GetProperty("Item", BindingFlags.Public | (BindingFlags.Static | BindingFlags.Instance), null, null, new Type[1] { typeof(int) }, null);
                if (itemProperty != null)
                {
                    itemType = itemProperty.PropertyType;
                }
                if ((itemType == null) || (itemType == typeof(object)))
                {
                    if (firstItem != null)
                    {
                        itemType = firstItem.GetType();
                    }
                }
                if ((firstItem != null) && (firstItem is ICustomTypeDescriptor))
                {
                    pdc = TypeDescriptor.GetProperties(firstItem);
                }
                else if (itemType != null)
                {
                    if (!IsBindableType(itemType))
                    {
                        pdc = TypeDescriptor.GetProperties(itemType);
                    }
                }
            }

            ArrayList props = new ArrayList();
            if ((pdc != null) && (pdc.Count != 0))
            {
                foreach (PropertyDescriptor descr in pdc)
                {
                    if (IsBindableType(descr.PropertyType))
                    {
                        props.Add(descr);
                    }
                }
            }

            return new PropertyDescriptorCollection((PropertyDescriptor [])props.ToArray(typeof(PropertyDescriptor)));
        }

        internal static bool IsBindableType(Type type)
        {
            return (type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal) || type == typeof(Guid));
        }
 

#if __BUILDING_CRI_DESIGNER__

        // --- End of testing filters ---

		private void AnalyseDataSource(string source, string prefix)
		{
            Chart chart = this.OwningChart;
            SqlChartDesigner rschartDesigner = ((SqlChartDesigner)chart.Owner);
            Microsoft.ReportDesigner.IReport report = rschartDesigner.Report;

            for (int i = 0; i < report.DataSets.Count; ++i)
            {
                if (report.DataSets[i].Name == source)
                {
                    DataTable dt = new DataTable();
                    Microsoft.ReportDesigner.RptDataSet rptDataSet = report.DataSets[i];
                    for (int j = 0; j < rptDataSet.Fields.Count; ++j)
                    {
                        Microsoft.ReportDesigner.Field field = rptDataSet.Fields[j];
                        if (field.TypeName != null)
                        {
                            dt.Columns.Add(new DataColumn(field.Name, Type.GetType(field.TypeName)));
                        }
                        else
                        {
                            dt.Columns.Add(new DataColumn(field.Name, typeof(int)));
                        }
                    }
                    AnalyseDataSource(ref dt, "");
                    break;
                }
            }
        }
#endif


        private void AnalyseDataSource(IList source, string prefix)
		{
		}

		private void AnalyseDataSource(DataSet source, string prefix)
		{
			if(prefix != "")
				prefix = prefix + ".";
			DataSet newDataSet = new DataSet(source.DataSetName);
			ArrayList resTables = new ArrayList();
			foreach(DataTable table in source.Tables)
			{
				DataTable tab = table;
				AnalyseDataSource(ref tab,prefix + table.TableName);
				resTables.Add(tab);
			}
			source.Tables.Clear();
			foreach(DataTable tab in resTables)
				source.Tables.Add(tab);
		}

		internal DataTable m_lastDataTable;

		private void AnalyseDataSource(ref DataTable source, string prefix)
		{
			m_lastDataTable = source;

			if(prefix != "")
				prefix = prefix + ".";
			
			bool toSelectAndOrder = true;
#if __BUILDING_CRI_DESIGNER__ 
			toSelectAndOrder = false;
#endif
			if(toSelectAndOrder
                && (source == dataSource || source.DataSet == null || source.DataSet.Tables.Count == 1)
				&& (selectClause != null || topNumber > 0 || orderClause != null))
			{
				try
				{
					if((selectClause != null && selectClause != "") ||
						(orderClause != null && orderClause != "") )
					{
						DataTable newSource = source.Clone();
						DataRow[] dataRows = source.Select(selectClause,orderClause);
						newSource.BeginLoadData();
						foreach(DataRow row in dataRows)
							newSource.Rows.Add(row.ItemArray);
						newSource.EndLoadData();
						source = newSource;
					}
					
					if(0 < topNumber && topNumber < source.Rows.Count)
					{
                        // We have to create new table
                        DataTable newTable = source.Clone();
                        // Populate new table
                        newTable.BeginLoadData();
                        for (int i = 0; i < topNumber; i++)
                            newTable.Rows.Add(source.Rows[i].ItemArray);
                        newTable.EndLoadData();
                        source = newTable;
                    }
				}
				catch(Exception e)
				{
					throw new Exception("Cannot perform SELECT operation on input table.\n" +e.Message);
				}
			}

			foreach(DataColumn column in source.Columns)
			{
				if(IsValidType(column.DataType))
					dataItems.Add(prefix + column.ColumnName,column);
			}
		}

		private void AnalyseDataSource(IDbCommand source, string prefix)
		{
			useRealDataInDesignTime = true;

            if (prefix != "")
				prefix = prefix + ".";
			ConnectionState state = source.Connection.State;
			if(state == ConnectionState.Closed)
				source.Connection.Open();
			else if (state != ConnectionState.Open)
				throw new Exception("Cannot bind to command when connection state = '" + state.ToString() + "'");

			IDataReader reader = source.ExecuteReader();
			DataTable shTable = reader.GetSchemaTable();
			DataRowCollection rows = shTable.Rows;
			DataTable table = new DataTable();

			foreach(DataRow row in rows)
				table.Columns.Add((string)(row["ColumnName"]),row["DataType"] as System.Type);

			// Populating the table

			if(!OwningChart.InDesignMode || useRealDataInDesignTime)
			{
				object[] values = new object[reader.FieldCount];
				while(reader.Read())
				{
					reader.GetValues(values);
					table.Rows.Add(values);
				}
			}

			AnalyseDataSource(ref table,prefix);

			if(state == ConnectionState.Closed)
				source.Connection.Close();
		}

		private void AnalyseDataSource(IDataAdapter source, string prefix)
		{
			useRealDataInDesignTime = true;
			DataSet dataSet = new DataSet();
			source.Fill(dataSet);
			AnalyseDataSource(dataSet,prefix);
		}

		private bool IsValidType(Type type)
		{
			return true;
		}

		internal string[] DataSourcePaths
		{
			get
			{
				AnalyseDataSource();
				int n = dataItems.Count, i = 0;
				string[] names = new String[n];
				IDictionaryEnumerator en = dataItems.GetEnumerator();
				while(en.MoveNext())
				{
					names[i] = (string)(en.Key);
					i++;
				}
				return names;
			}
		}

		#endregion

		#region --- Data Evaluation ---

		internal Variable Evaluate(InputVariable inVar)
		{
			string[] parts = inVar.Name.Split(':');
			string expression;
			if(parts.Length == 2)
				expression = "[" + parts[0] + ":" + parts[1] + "]";
			else
				expression = inVar.Name;
			Parser parser = new Parser(expression);
			Variable var = parser.Compute(inputVars);
			if(var == null)
				OwningChart.RegisterErrorMessage(parser.ErrorMessage);
			else
			{// Set variable dimension
				object varDim = availableDimensions[inVar.Name];
				if(varDim != null)
					var.Dimension = varDim as DataDimension;
				else
					var.CreateDimension();
			}
			return var;
		}

		internal Variable Evaluate(string varExpression)
		{	
			errorMessage = "";
			Parser P = new Parser(varExpression);
			Variable v = P.Compute(inputVars);

			// Default x-coordinate, if not specified as input variable
			if(v == null && varExpression == "x" && inputVars["x"] == null)
			{
				double[] val = null;
				if(SeriesNameInInputData)
					val = new Double[InputVariables["series"].EvaluatedValue.Length];
				else
				{
					if(inputVars.Count > 0)
					{
						InputVariable yVar = inputVars[0];
						Variable yVarValue = Evaluate(yVar.Name);
						if(yVarValue != null)
						{
							val = new Double[yVarValue.Length];
						}
					}
				}
				if(val != null)
				{
					for(int i=0; i<val.Length; i++)
						val[i] = i;
					NumericVariable xVar = new NumericVariable("x",val);
					xVar.Dimension = DataDimension.StandardIndexDimension;	
					return xVar;
				}
				
			}
			errorMessage = P.ErrorMessage;
			return v;
		}

		#endregion
	
		#region --- Default Data Binding to Databases ---

		private bool IsSuitableForXCoordinate(Variable v)
		{
			if(v is StringVariable)
			{
				StringVariable sv = v as StringVariable;
				for(int i = 0; i<sv.Length - 1; i++)
				{
					for(int j=i+1; j<sv.Length; j++)
						if(sv[i] == sv[j])
							return false;
				}
				return true;
			}
			else if(v is DateTimeVariable)
			{
				DateTimeVariable dv = v as DateTimeVariable;
				bool notIncreasing = false;
				bool notDecreasing = false;
				for(int i = 0; i<dv.Length - 1; i++)
				{
					DateTime dtc = dv.ValueAt(i);
					DateTime dtn = dv.ValueAt(i+1);
					if(dtc < dtn)
						notDecreasing = true;
					else if(dtc > dtn)
						notIncreasing = true;
				}
				return !notDecreasing || !notIncreasing; // meaning Decreasing || Increasing
			}

			return false;
		}

		private bool IsSuitableForYCoordinate(Variable v)
		{
			return v is NumericVariable && v.Name.Length>=2 && v.Name.Substring(v.Name.Length-2)!="ID";
		}

		private bool IsTakenVariable(Variable v)
		{
			foreach (InputVariable iv in inputVars)
			{
				if(iv.ValuePath == v.Name)
					return true;
			}
			return false;
		}

		internal void DefaultDataBindingToDatabase(Series[] series)
		{
            if (!doDefaultDataBind)
                return;
			foreach(Series s in series)
			{
				DefaultDataBindingToDatabase(s);
			}
		}

		
		internal void DefaultDataBindingToDatabase(Series series)
		{
            if (!doDefaultDataBind)
                return;

			// Collect variables

			IDictionaryEnumerator en = dataItems.GetEnumerator();

			if(SeriesNameInInputData)
			{
				return;
			}
			else
			{
				Variable varForY = null;
				bool xDefined = false, yDefined = false;
				int nty = 0;
				while(en.MoveNext())
				{
					DataColumn column = en.Value as DataColumn;
					if(column != null && !column.AutoIncrement)
					{
						Variable var = Variable.CreateVariable((string)en.Key,en.Value);
						if(var != null)
						{
							if(!xDefined &&IsSuitableForXCoordinate(var))
							{
								string name = series.Name +":x";
								OwningChart.DefineValuePath(name,var.Name);
								inputVars[name].Evaluate();
								xDefined = true;
							}
							else if(!yDefined && IsSuitableForYCoordinate(var))
							{
								varForY = var;
								if(!IsTakenVariable(var))
								{
									string name = series.Name +":y";
									OwningChart.DefineValuePath(name,var.Name);
									inputVars[name].Evaluate();
									nty = var.Length;
									yDefined = true;
								}
							}
						}
					}
				}

				if(!yDefined)
				{
					if(varForY != null)
					{
						string name = series.Name +":y";
						OwningChart.DefineValuePath(name,varForY.Name);
						inputVars[name].Evaluate();
						nty = varForY.Length;
					}
					else
					{
						InputVariable inVar = OwningChart.InputVariables[series.Name + ":y"];
						if(inVar == null || inVar.ValueExpression == "" || inVar.ValueExpression == null)
							series.OwningSeries.SubSeries.Remove(series);
					}

				}
				if(!xDefined)
				{
					series.NumberOfPoints = nty;
				}
			}
		}

		#endregion
	}
}
