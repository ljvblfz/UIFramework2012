using System;
using System.Collections;
using System.Data;
using System.Drawing;


namespace ComponentArt.Web.Visualization.Charting
{
	// ===============================================================================================
	//		Variable
	// ===============================================================================================

	internal abstract class Variable : Expression
	{
		private bool[]	isMissing;
		protected bool		isConstant;
		protected DataDimension dimension = null;
		// Design-time data simulation
		private int		expectedNumberOfSamples = 5;

		#region --- Construction, including static ---
		public Variable(string name) : base(Expression.OpKind.Self)
		{
			Name = name;
			expressionValue = this;
			dimension = null;
		}

		internal Variable() {}

		internal static Variable CreateVariable(string name, ValueType valueType)
		{
			switch(valueType)
			{
				case ValueType.Bool:
					return new BoolVariable(name);
				case ValueType.Color:
					return new ColorVariable(name);
				case ValueType.DateTime:
					return new DateTimeVariable(name);
				case ValueType.Numeric:
					return new NumericVariable(name);
				case ValueType.Object:
					return new ObjectVariable(name);
				case ValueType.String:
					return new StringVariable(name);
				default:
					return null;
			}
		}

		internal static Variable CreateVariable(string name, Type dataType)
		{
			if(dataType == typeof(Byte) ||
				dataType == typeof(Char) ||
				dataType == typeof(Decimal) ||
				dataType == typeof(Double) ||
				dataType == typeof(Int16) ||
				dataType == typeof(Int32) ||
				dataType == typeof(Int64) ||
				dataType == typeof(SByte) ||
				dataType == typeof(Single) ||
				dataType == typeof(UInt16) ||
				dataType == typeof(UInt32) ||
				dataType == typeof(UInt64))
				return new NumericVariable(name);
			else if (dataType == typeof(DateTime))
				return new DateTimeVariable(name);
			else if (dataType == typeof(Boolean))
				return new BoolVariable(name);
			else if (dataType == typeof(String))
				return new StringVariable(name);
			else if (dataType == typeof(Color))
				return new ColorVariable(name);
			else
				return null;
		}

		internal static Variable CreateVariable(string name, object val)
		{
			Type dataType = val.GetType();

			// Scalar types

			try 
			{
				if(
					dataType == typeof(Byte) ||
					dataType == typeof(Decimal) ||
					dataType == typeof(Double) ||
					dataType == typeof(Int16) ||
					dataType == typeof(Int32) ||
					dataType == typeof(Int64) ||
					dataType == typeof(SByte) ||
					dataType == typeof(Single) ||
					dataType == typeof(UInt16) ||
					dataType == typeof(UInt32) ||
					dataType == typeof(UInt64)) 
				{
					double jjj = (double)val;
					return new NumericVariable(name,jjj);
				}
				else if (dataType == typeof(DateTime))
					return new DateTimeVariable(name,(DateTime)val);
				else if (dataType == typeof(Boolean))
					return new BoolVariable(name,(bool)val);
				else if (dataType == typeof(String))
					return new StringVariable(name,(string)val);
				else if (dataType == typeof(Color))
					return new ColorVariable(name,(Color)val);
			} 
			catch (Exception ex) 
			{
				throw;
			}
			// Direct Array types

			if(dataType == typeof(double[]))
				return new NumericVariable(name,(double[])val);
			if(dataType == typeof(DateTime[]))
				return new DateTimeVariable(name,(DateTime[])val);
			if(dataType == typeof(Boolean[]))
				return new BoolVariable(name,(Boolean[])val);
			if(dataType == typeof(String[]))
				return new StringVariable(name,(String[])val);
			if(dataType == typeof(Color[]))
				return new ColorVariable(name,(Color[])val);

			// Indirect numeric array types

			if(dataType == typeof(Byte[]))
			{
				Byte[] x = (Byte[])val;
				double[] y = new double[x.Length];
				for(int i=0;i<x.Length;i++)
					y[i] = (double)x[i];
				return new NumericVariable(name,y);
			}
			if(dataType == typeof(Decimal[]))
			{
				Decimal[] x = (Decimal[])val;
				double[] y = new double[x.Length];
				for(int i=0;i<x.Length;i++)
					y[i] = (double)x[i];
				return new NumericVariable(name,y);
			}
			if(dataType == typeof(Int16[]))
			{
				Int16[] x = (Int16[])val;
				double[] y = new double[x.Length];
				for(int i=0;i<x.Length;i++)
					y[i] = (double)x[i];
				return new NumericVariable(name,y);
			}
			if(dataType == typeof(Int32[]))
			{
				Int32[] x = (Int32[])val;
				double[] y = new double[x.Length];
				for(int i=0;i<x.Length;i++)
					y[i] = (double)x[i];
				return new NumericVariable(name,y);
			}
			if(dataType == typeof(Int64[]))
			{
				Int64[] x = (Int64[])val;
				double[] y = new double[x.Length];
				for(int i=0;i<x.Length;i++)
					y[i] = (double)x[i];
				return new NumericVariable(name,y);
			}
			if(dataType == typeof(SByte[]))
			{
				SByte[] x = (SByte[])val;
				double[] y = new double[x.Length];
				for(int i=0;i<x.Length;i++)
					y[i] = (double)x[i];
				return new NumericVariable(name,y);
			}
			if(dataType == typeof(Single[]))
			{
				Single[] x = (Single[])val;
				double[] y = new double[x.Length];
				for(int i=0;i<x.Length;i++)
					y[i] = (double)x[i];
				return new NumericVariable(name,y);
			}
			if(dataType == typeof(UInt16[]))
			{
				UInt16[] x = (UInt16[])val;
				double[] y = new double[x.Length];
				for(int i=0;i<x.Length;i++)
					y[i] = (double)x[i];
				return new NumericVariable(name,y);
			}
			if(dataType == typeof(UInt32[]))
			{
				UInt32[] x = (UInt32[])val;
				double[] y = new double[x.Length];
				for(int i=0;i<x.Length;i++)
					y[i] = (double)x[i];
				return new NumericVariable(name,y);
			}
			if(dataType == typeof(UInt64[]))
			{
				UInt64[] x = (UInt64[])val;
				double[] y = new double[x.Length];
				for(int i=0;i<x.Length;i++)
					y[i] = (double)x[i];
				return new NumericVariable(name,y);
			}

			// Data Column type

			if(val is DataColumn)
			{
				DataColumn col = val as DataColumn;
				string colName = col.ColumnName;
				DataTable  table = col.Table;
				DataRowCollection rows = table.Rows;
				int nData = rows.Count;
				bool[] missing = new bool[nData];
				if(col.DataType == typeof(bool))
				{
					bool[] v = new bool[nData];
					for(int i=0;i<nData;i++)
					{
						object item = table.Rows[i][colName];
						if(item is DBNull)
						{
							v[i] = false;
							missing[i] = true;
						}
						else
						{
							v[i] = (bool)item;
							missing[i] = false;
						}
					}
					BoolVariable r = new BoolVariable(name,v);
					r.IsMissing = missing;
					return r;
				}
				else if(col.DataType == typeof(string))
				{
					string[] v = new string[nData];
					for(int i=0;i<nData;i++)
					{
						object item = table.Rows[i][colName];
						if(item is DBNull)
						{
							v[i] = "";
							missing[i] = true;
						}
						else
						{
							v[i] = (string)item;
							missing[i] = false;
						}
					}
					StringVariable r = new StringVariable(name,v);
					r.IsMissing = missing;
					return r;
				}
				else if(col.DataType == typeof(DateTime))
				{
					DateTime[] v = new DateTime[nData];
					for(int i=0;i<nData;i++)
					{
						object item = table.Rows[i][colName];
						if(item is DBNull)
						{
							v[i] = DateTime.Today;;
							missing[i] = true;
						}
						else
						{
							v[i] = (DateTime)item;
							missing[i] = false;
						}
					}
					DateTimeVariable r = new DateTimeVariable(name,v);
					r.IsMissing = missing;
					return r;
				}
				else if(
					col.DataType == typeof(byte) ||
					col.DataType == typeof(decimal) ||
					col.DataType == typeof(double) ||
					col.DataType == typeof(Int16) ||
					col.DataType == typeof(Int32) ||
					col.DataType == typeof(Int64) ||
					col.DataType == typeof(SByte) ||
					col.DataType == typeof(Single) ||
					col.DataType == typeof(UInt16) ||
					col.DataType == typeof(UInt32) ||
					col.DataType == typeof(UInt64) 
					)
				{
					double[] v = new double[nData];
					for(int i=0;i<nData;i++)
					{
						object item = table.Rows[i][colName];

						if(item is DBNull)
						{
							missing[i] = true;
							continue;
						}
						else
							missing[i] = false;
						if(col.DataType == typeof(byte))
							v[i] = (byte)item;
						else if(col.DataType == typeof(decimal))
							v[i] = (double)(decimal)item;
						else if(col.DataType == typeof(double))
							 v[i] = (double)item;
						 else if(col.DataType == typeof(Int16))
							 v[i] = (Int16)item;
						 else if(col.DataType == typeof(Int32))
							 v[i] = (Int32)item;
						 else if(col.DataType == typeof(Int64))
							 v[i] = (Int64)item;
						 else if(col.DataType == typeof(SByte))
							 v[i] = (SByte)item;
						 else if(col.DataType == typeof(Single))
							 v[i] = (Single)item;
						 else if(col.DataType == typeof(UInt16))
							 v[i] = (UInt16)item;
						 else if(col.DataType == typeof(UInt32))
							 v[i] = (UInt32)item;
						 else 
							v[i] = (UInt64)item;
					}
					NumericVariable r = new NumericVariable(name,v);
					r.IsMissing = missing;
					return r;
				}
			}

			// Trying IEnumerable

			if(val is IEnumerable)
			{
				object obj = Parser.Flatten(val);
				if(obj is double[] ||
				   obj is string[] ||
				   obj is bool  [] ||
				   obj is DateTime[] ||
				   obj is Color[])
					return CreateVariable(name,obj);
			}

			return null;
		}


		#endregion

		#region --- Properties ---

		internal bool HasDimension { get { return (dimension != null); } }

		public DataDimension Dimension 
		{
			get 
			{ 
				if(dimension == null)
					CreateDimension();
				return dimension; 
			} 
			set { dimension = value; } 
		}

		public int  ExpectedNumberOfSamples 
		{
			get { return expectedNumberOfSamples; } 
			set { if(value>0) expectedNumberOfSamples = value; }
		}

		public bool[] IsMissing
		{ 
			get { return isMissing; }
			set { isMissing = value; }
		}
		
		public int Length 
		{
			get
			{
				if(isMissing==null)
					return -1;
				else if(isConstant)
					return 1;
				else
					return isMissing.Length;
			}
		}

		public bool IsConstant { get { return isConstant; } }
		internal bool SetConstant { set { isConstant = value; } }

		public override ValueType ValType {get {return valueType;}}
		
		public object MinObject
		{
			get 
			{
				double minNum = double.MaxValue;
				object minObj  = null;

				for(int i=0;i<Length;i++)
				{
					object val = ItemAt(i);
					double y0 = dimension.Coordinate(val);
					double y1 = y0 + dimension.Width(val);
					if(y0 < minNum || (minNum == y0 && dimension.Width(val) < dimension.Width(minObj)))
					{
						minNum = y0;
						minObj = val;
					}
				}
				return minObj;
			}
		}
		
		public object MaxObject
		{
			get 
			{
				double maxNum = double.MinValue;
				object maxObj  = null;

				for(int i=0;i<Length;i++)
				{
					object val = ItemAt(i);
					double y0 = dimension.Coordinate(val);
					double y1 = y0 + dimension.Width(val);
					if(y1 > maxNum || (maxNum == y1 && dimension.Width(val) < dimension.Width(maxObj)))
					{
						maxNum = y1;
						maxObj = val;
					}
				}
				return maxObj;
			}
		}
		
		public double MinNumeric
		{
			get 
			{
				double minNum = double.MaxValue;

				for(int i=0;i<Length;i++)
				{
					object val = ItemAt(i);
					double y0 = dimension.Coordinate(val);
					minNum = Math.Min(minNum,y0);
				}
				return minNum;
			}
		}
		
		public double MaxNumeric
		{
			get 
			{
				double maxNum = double.MinValue;

				for(int i=0;i<Length;i++)
				{
					object val = ItemAt(i);
					double y0 = dimension.Coordinate(val);
					double y1 = y0 + dimension.Width(val);
					maxNum = Math.Max(maxNum,y1);
				}
				return maxNum;
			}
		}

		internal virtual void CreateDimension()
		{
			EnumeratedDataDimension edd = new EnumeratedDataDimension(Name);
			for(int i=0;i<Length;i++)
				edd.Add(ItemAt(i));
			dimension = edd;
		}

		public double Coordinate(int i) { return Dimension.Coordinate(ItemAt(i)); }
		public double Width(int i) { return Dimension.Width(ItemAt(i)); }
		#endregion

		public bool MissingAt(int x)
		{
			if(IsConstant)
				return false;
			else
				return isMissing[x];
		}
		public abstract Variable Alternate(BoolVariable selection, Variable var);
		public abstract Variable Filter(BoolVariable selection);
		public abstract BoolVariable BinaryRelation(Expression.OpKind operation, Variable var2);
		public virtual  StringVariable EvaluateFormat(StringVariable var2)
		{
			return new StringVariable("");
		}
		public StringVariable Format(StringVariable formatVariable)
		{
			return EvaluateFormat(formatVariable);
		}
		public new StringVariable Format(string formatString)
		{
			return Format(new StringVariable(formatString,formatString));
		}
		public new StringVariable Format()
		{
			return Format("G");
		}

		public abstract object ItemAt(int index);
		public abstract int Add(object o);
		public abstract void Clear();
	}

}
