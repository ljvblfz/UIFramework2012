using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for DataProvider.
	/// </summary>
	internal class DataProvider
	{
		public DataProvider()
		{

		}
	}

	public enum AggregationKind
	{
		Count,
		Average,
		Sum,
		Minimum,
		Maximum,
		First,
		Last
	}

	internal class GaugeDataBinder : NamedObject
	{
		private object dataSource;
		private string dataSourcePath;
		private AggregationKind aKind;
		public GaugeDataBinder() : base() { }
		public GaugeDataBinder(string name) : base(name) { }
		public GaugeDataBinder(string propertyPath, object dataSource, string dataSourcePath, AggregationKind aKind) : base(propertyPath)
		{
			this.dataSource = dataSource;
			this.dataSourcePath = dataSourcePath;
			this.aKind = aKind;
		}

		#region --- Properties ---
		#endregion

		#region --- Binding ---
		internal void Bind()
		{
			PropertyDescriptor pDes = null;
			object target = ObjectModelBrowser.GetOwningTopmostGauge(this);
			string targetPath = Name;

			try
			{
				GetTarget(ref targetPath,ref target, out pDes);
				object source = dataSource;
				string dataPath = dataSourcePath;
				object sourceValue = EvaluateSourceValue(dataSource, dataPath);
				//object val = EvaluateAggregate(sourceValue);

			}
			catch(Exception ex)
			{
				throw new Exception("Cannot bind '" + Name + "': " + ex.Message);
			}
		}

		private void PreprocessDataSource()
		{
			if(dataSource is IDataAdapter)
			{
				IDataAdapter source = dataSource as IDataAdapter;
				DataSet dataSet = new DataSet();
				source.Fill(dataSet);
				dataSource = dataSet;
			}
			else if(dataSource is IDbCommand)
			{
				//AnalyseDataSource(dataSource as IDbCommand,"");
			}			
			else if (dataSource is IEnumerable)
			{
				//AnalyseIEnumerableDataSource();
			}
			else
			{
				throw new ArgumentException("Cannot bind to data type '" + dataSource.GetType().Name + "'");
			}

		}

		// Recursive computation of (targetObject, propertyDescriptor)
		private void GetTarget(ref string targetPath, ref object target, out PropertyDescriptor pDes)
		{
			// Take first subexpression terminating with a dot
			int ix = targetPath.IndexOf(".");
			string prefix, tail;
			if(ix>0)
			{
				prefix = targetPath.Substring(0,ix);
				tail = targetPath.Substring(ix+1);
				tail = tail.Trim();
			}
			else
			{
				prefix = targetPath;
				tail = null;
			}
			prefix = prefix.Trim();

			// Now the prefix is "propertyName" or "propertyName[memberName]".

			ix = prefix.IndexOf("[");
			string propertyName, memberName;
			if(ix > 0)
			{
				int kx = prefix.IndexOf("]");
				if(kx == -1)
					throw new Exception("Invalid section in targhet path '" + prefix + "': ']' is missing.");
				if(kx != prefix.Length-1)
					throw new Exception("Invalid section in targhet path '" + prefix + "': unexpected position of ']'.");
				propertyName = prefix.Substring(0,ix);
				memberName = prefix.Substring(ix+1,prefix.Length-ix-2);
				memberName = memberName.Trim();
			}
			else
			{
				propertyName = prefix;
				memberName = null;
			}
			propertyName = propertyName.Trim();

			// Get the property
			PropertyDescriptor propertyDes = TypeDescriptor.GetProperties(target)[propertyName];
			if(propertyDes == null)
				throw new Exception("Object class '" + target.GetType().Name + " doesn't have property '" + propertyName + "'.");

			if(tail == null)
			{
				pDes = propertyDes;
				return;
			}
			
			target = propertyDes.GetValue(target);
			
			// Get the member

			if(memberName != null)
			{
				NamedObjectCollection collection = target as NamedObjectCollection;
				if(collection == null)
					throw new Exception("Property '" + target.GetType().Name + "." + propertyName + "' is not valid collection.");
				target = collection[memberName];
			}
			
			// Call again
			 
			GetTarget(ref tail,ref target, out pDes);
		}
		
		
		// Recursive computation of source value 
		private object EvaluateSourceValue(object dataSource, string dataPath)
		{
			if(dataPath == null && dataPath == "")
				return dataSource;

			int ix = dataPath.IndexOf(".");
			string prefix = dataPath.Substring(0,ix);
			string tail = dataPath.Substring(ix+1);
			prefix = prefix.Trim();
			tail = tail.Trim();

			// Trying data set
			DataSet dataSet = dataSource as DataSet;
			if(dataSet != null)
			{
				DataTable tab = dataSet.Tables[prefix];
				if(tab == null)
					throw new Exception("Data set doesn't contain table '" + prefix + "'.");
				return EvaluateSourceValue(tab,tail);
			}

			// Trying data table
			DataTable table = dataSource as DataTable;
			if(table != null)
			{
				DataColumn column = table.Columns[prefix];
				if(column == null)
					throw new Exception("Data table '" + table.TableName + "' doesn't contain column '" + prefix + "'.");
				return column;
			}

			// Trying object property

			PropertyDescriptor pDes = TypeDescriptor.GetProperties(dataSource)[prefix];
			if(pDes != null)
				return EvaluateSourceValue(pDes.GetValue(dataSource),tail);

			// Trying IDictionary

			IDictionary dictionary = dataSource as IDictionary;
			if(dictionary != null)
			{
				dataSource = dictionary[prefix];
				if(dataSource == null) 
					throw new Exception("The dictionary doesn't have member '" + prefix + "'.");
				return EvaluateSourceValue(dataSource,tail);
			}

			// Trying IEnumerable - unfolding a collection

			IEnumerable enumerable = dataSource as IEnumerable;
			if(enumerable != null)
			{
				ArrayList list = new ArrayList();
				Type itemType = null;
				PropertyDescriptor pDesItem = null;
				foreach(object item in enumerable)
				{
					Type type = item.GetType();
					if(type != itemType)
					{
						itemType = type;
						pDesItem = TypeDescriptor.GetProperties(item)[prefix];
						if(pDes == null)
							throw new Exception("Object type '" + type.Name + "' doesn't have property '" + prefix + "'.");
					}
					list.Add(pDesItem.GetValue(item));
				}
				return EvaluateSourceValue(list,tail);
			}
			return null;
		}
		#endregion
	}
}
