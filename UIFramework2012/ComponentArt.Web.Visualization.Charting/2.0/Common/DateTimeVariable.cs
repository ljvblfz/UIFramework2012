using System;
using System.Collections;
using System.Data;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class DateTimeVariable : Variable
	{
		protected DateTime[]	val;

		internal DateTimeVariable() {}

		public DateTimeVariable(string name) : base(name)
		{ 
			isConstant = false;
			valueType = Expression.ValueType.DateTime;
		}

		public DateTimeVariable(string name, DateTime val) : base(name)
		{ 
			this.val = new DateTime[1];
			this.val[0] = val;
			IsMissing = new bool[1];
			IsMissing[0] = false;
			isConstant = true;
			valueType = Expression.ValueType.DateTime;
		}

		public DateTimeVariable(DateTime val) : this(val.ToString(),val) { }

		public DateTimeVariable(string name, DateTime[] val) : this(name,val,null)	{ }

		public DateTimeVariable(string name, DateTime[] val, bool[] missing) : base(name)
		{ 
			isConstant = false;
			Add(val,missing);
			valueType = Expression.ValueType.DateTime;
		}

		internal override void CreateDimension ()
		{
			DateTimeDataDimension dim = new DateTimeDataDimension(Name+"_Dimension");
			dimension = dim;
			if(val.Length>0)
			{
				dim.MinValue = val[0];
				for(int i=1; i<val.Length; i++)
				{
					if(val[i] < dim.MinValue)
                        dim.MinValue = val[i];
				}
			}
			else
			{
				dim.MinValue = new DateTime(DateTime.Now.Year,1,1);
			}
			dim.ReferenceValue = dim.MinValue;
			dimension.ReferenceVariableIsCustom = false;
		}

		public DateTime[] Value { get { return val; } }

		public override object ItemAt(int index) { return ValueAt(index); }

		public DateTime ValueAt(int x)
		{
			if(IsConstant)
				return val[0];
			else
				return val[x];
		}

		public override void Clear() 
		{
			val = null;
			IsMissing = null;
		}

		public override int Add(object val)
		{
			if (val.GetType() != typeof(System.DateTime)) 
				throw new ArgumentException( "value must be of type DateTime.", "val" );

			DateTime[] v = new DateTime[1];
			v[0] = (DateTime)val;
			return Add(v);
		}

		public int Add(DateTime[] val)
		{
			return Add(val,null);
		}

		public int Add(DateTime[] val, bool[] missing)
		{
			if(IsConstant)
				throw new InvalidOperationException("Could not apply 'Add()' on a constant variable");
			if(missing != null && missing.Length != val.Length)
				throw new ArgumentException("Length of the 'missing' array is not equal to the length of 'val' array in 'Add()' operation");

			int L = val.Length;
			int i0;
			if(this.val == null)
			{
				this.val = new DateTime[L];
				IsMissing = new bool[L];
				i0 = 0;
			}
			else
			{
				int L1 = this.val.Length;
				DateTime[] newVal = new DateTime[L+L1];
				bool[] newMss = new bool[L+L1];
				for (int i=0;i<L1;i++)
				{
					newVal[i] = this.val[i];
					newMss[i] = IsMissing[i];
				}
				this.val = newVal;
				IsMissing = newMss;
				i0 = L1;
			}

			for(int i=0;i<val.Length;i++)
			{
				this.val[i0+i] = val[i];
				if(missing != null)
					IsMissing[i0+i] = missing[i];
				else
					IsMissing[i0+i] = false;
			}

			return this.val.Length;
		}

		#region --- Operations ---

		public override Variable Alternate(BoolVariable selection, Variable variable)
		{
			if(!(variable is DateTimeVariable))
				throw new ArgumentException("Variables in the 'Alternate()' operation don't have the same type");
		
			DateTimeVariable var = variable as DateTimeVariable;
			int M = Math.Max(selection.Length,Math.Max(var.Length,Length));
			if( !this.IsConstant && this.Length!=M ||
				!var.IsConstant && var.Length!=M ||
				!selection.IsConstant && selection.Length!=M)
				throw new ArgumentException("Variables in the 'Alternate()' operation don't have the same length");

			if(IsConstant && var.IsConstant && selection.IsConstant)
			{
				if(selection.ValueAt(0))
					return new DateTimeVariable(this.ValueAt(0));
				else
					return new DateTimeVariable(var.ValueAt(0));
			}
			
			DateTime[] r = new DateTime[M];
			bool[] m = new bool[M];
			for(int i=0; i<M; i++)
			{
				if(selection.ValueAt(i))
				{
					r[i] = this.ValueAt(i);
					m[i] = this.MissingAt(i);
				}
				else
				{
					r[i] = var.ValueAt(i);
					m[i] = var.MissingAt(i);
				}
			}
			return new DateTimeVariable("",r,m);
		}


		public override Variable Filter(BoolVariable selection)
		{
			int M = Math.Max(selection.Length,Length);
			if( !this.IsConstant && this.Length!=M ||
				!selection.IsConstant && selection.Length!=M)
				throw new ArgumentException("Variables in the 'Filter()' operation don't have the same length");

			if(IsConstant && selection.IsConstant)
			{
				if(selection.ValueAt(0))
					return new DateTimeVariable(this.ValueAt(0));
				else
					return new DateTimeVariable("",new DateTime[] {} );
			}
			
			int L = 0;
			bool[] filter = selection.Value;

			for(int i=0; i<M; i++)
			{
				if(filter[i])
					L++;
			}

			bool[] m = new bool[L];
			DateTime[] r = new DateTime[L];

			int k = 0;
			for(int i=0; i<M; i++)
			{
				if(selection.ValueAt(i))
				{
					r[k] = this.ValueAt(i);
					m[k] = this.MissingAt(i);
					k++;
				}
			}
			return new DateTimeVariable("",r,m);
		}

		public override BoolVariable BinaryRelation(Expression.OpKind operation, Variable var2)
		{
			DateTimeVariable v2 = var2 as DateTimeVariable;
			if(v2==null)
				throw new ArgumentException("Variables in the binary relation don't have the same type");

			int M = Math.Max(Length,v2.Length);
			string strOp="";
			bool[] r = new bool[M];
			bool[] m = new bool[M];
			for(int i=0; i<M; i++)
			{
				switch(operation)
				{
					case Expression.OpKind.Lt: r[i] = ValueAt(i) <  v2.ValueAt(i); strOp = "<";  break;
					case Expression.OpKind.Le: r[i] = ValueAt(i) <= v2.ValueAt(i); strOp = "<="; break;
					case Expression.OpKind.Eq: r[i] = ValueAt(i) == v2.ValueAt(i); strOp = "=="; break;
					case Expression.OpKind.Ne: r[i] = ValueAt(i) != v2.ValueAt(i); strOp = "!="; break;
					case Expression.OpKind.Ge: r[i] = ValueAt(i) >= v2.ValueAt(i); strOp = ">="; break;
					case Expression.OpKind.Gt: r[i] = ValueAt(i) >  v2.ValueAt(i); strOp = ">";  break;
					default:
						throw new InvalidOperationException("Implementation: DoubleVariable.BinaryRelation() called with wrong operation code");
				}
				m[i] = MissingAt(i) || v2.MissingAt(i) ;
			}
			BoolVariable v = new BoolVariable("(" + Name + ")" + strOp + "("+v2.Name+")",r,m);
			v.SetConstant = isConstant && v2.IsConstant;
			return v;
		}

		public override StringVariable EvaluateFormat(StringVariable var2)
		{
			int M = val.Length;
			string[] r = new string[M];
			bool  [] m = new bool  [M];
			for(int i=0;i<M;i++)
			{
				if(!MissingAt(i) && !var2.MissingAt(i))
				{
					r[i] = ValueAt(i).ToString(var2.ValueAt(i));
					m[i] = false;
				}
				else
				{
					r[i] = "";
					m[i] = true;
				}
			}
			return new StringVariable("",r,m);
		}
		#endregion
	}

}
