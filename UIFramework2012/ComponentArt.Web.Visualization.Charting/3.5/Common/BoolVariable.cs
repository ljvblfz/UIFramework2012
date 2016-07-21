using System;
using System.Collections;
using System.Data;
using System.Drawing;


namespace ComponentArt.Web.Visualization.Charting
{
	internal class BoolVariable : Variable
	{
		protected bool[]	val;

		public BoolVariable(string name) : base(name)
		{ 
			isConstant = false;
			valueType = Expression.ValueType.Bool;
		}

		internal BoolVariable() {}

		public BoolVariable(bool val) : this(val.ToString(),val) { }

		public BoolVariable(string name, bool val) : base(name)
		{ 
			this.val = new bool[1];
			this.val[0] = val;
			IsMissing = new bool[1];
			IsMissing[0] = false;
			isConstant = true;
			valueType = Expression.ValueType.Bool;
		}

		public BoolVariable(string name, bool[] val) : this(name,val,null)
		{ }

		public BoolVariable(string name, bool[] val, bool[] missing) : base(name)
		{ 
			isConstant = false;
			Add(val,missing);
			valueType = Expression.ValueType.Bool;
		}

		internal override void CreateDimension ()
		{
			dimension = DataDimension.StandardBooleanDimension;
		}

		public bool[] Value { get { return val; } }

		public override object ItemAt(int index) { return ValueAt(index); }

		public bool ValueAt(int x)
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
			if (val.GetType() != typeof(bool)) 
				throw new ArgumentException( "value must be of type bool.", "val" );

			bool[] v = new bool[1];
			v[0] = (bool)val;
			return Add(v);
		}

		public int Add(bool[] val)
		{
			return Add(val,null);
		}

		public int Add(bool[] val, bool[] missing)
		{
			if(IsConstant)
				throw new InvalidOperationException("Could not apply 'Add()' on a constant variable");
			if(missing != null && missing.Length != val.Length)
				throw new ArgumentException("Length of the 'missing' array is not equal to the length of 'val' array in 'Add()' operation");

			int L = val.Length;
			int i0;
			if(this.val == null)
			{
				this.val = new bool[L];
				IsMissing = new bool[L];
				i0 = 0;
			}
			else
			{
				int L1 = this.val.Length;
				bool[] newVal = new bool[L+L1];
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

		public override Variable Alternate(BoolVariable selection, Variable variable)
		{
			if(!(variable is BoolVariable))
				throw new ArgumentException("Variables in the 'Alternate()' operation don't have the same type");
		
			BoolVariable var = variable as BoolVariable;
			int M = Math.Max(selection.Length,Math.Max(var.Length,Length));
			if( !this.IsConstant && this.Length!=M ||
				!var.IsConstant && var.Length!=M ||
				!selection.IsConstant && selection.Length!=M)
				throw new ArgumentException("Variables in the 'Alternate()' operation don't have the same length");

			if(IsConstant && var.IsConstant && selection.IsConstant)
			{
				if(selection.ValueAt(0))
					return new BoolVariable(this.ValueAt(0));
				else
					return new BoolVariable(var.ValueAt(0));
			}
			
			bool[] r = new bool[M];
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
			return new BoolVariable("",r,m);
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
					return new BoolVariable(this.ValueAt(0));
				else
					return new BoolVariable("",new bool[] {} );
			}
			
			int L = 0;
			bool[] filter = selection.Value;

			for(int i=0; i<M; i++)
			{
				if(filter[i])
					L++;
			}

			bool[] r = new bool[L];
			bool[] m = new bool[L];

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
			return new BoolVariable("",r,m);
		}

		#region --- Operations ---
		public BoolVariable Not()
		{
			bool[] r = new bool[Length];
			bool[] m = new bool[Length];
			for(int i=0; i<Length; i++)
			{
				r[i] = !val[i];
				m[i] = IsMissing[i];
			}
			BoolVariable v = new BoolVariable("-(" + Name + ")",r,m);
			v.isConstant = isConstant;
			return v;
		}

		public BoolVariable BinaryOperation(Expression.OpKind operation, BoolVariable v2)
		{
			int M = Math.Max(Length,v2.Length);
			string strOp="";
			bool[] r = new bool[M];
			bool[] m = new bool[M];
			for(int i=0; i<M; i++)
			{
				switch(operation)
				{
					case Expression.OpKind.And: r[i] = ValueAt(i) && v2.ValueAt(i); strOp = "&"; break;
					case Expression.OpKind.Or : r[i] = ValueAt(i) || v2.ValueAt(i); strOp = "|"; break;
					default:
						throw new InvalidOperationException("Implementation: BoolVariable.BinaryOperation() called with wrong operation code");
				}
				m[i] = MissingAt(i) || v2.MissingAt(i) ;
			}
			BoolVariable v = new BoolVariable("(" + Name + ")" + strOp + "("+v2.Name+")",r,m);
			v.isConstant = isConstant;
			return v;
		}

		public override BoolVariable BinaryRelation(Expression.OpKind operation, Variable var2)
		{
			BoolVariable v2 = var2 as BoolVariable;
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
					case Expression.OpKind.Lt: r[i] = !ValueAt(i) &&  v2.ValueAt(i); strOp = "<";  break;
					case Expression.OpKind.Le: r[i] = !ValueAt(i) ||  v2.ValueAt(i); strOp = "<="; break;
					case Expression.OpKind.Eq: r[i] =  ValueAt(i) ==  v2.ValueAt(i); strOp = "=="; break;
					case Expression.OpKind.Ne: r[i] =  ValueAt(i) !=  v2.ValueAt(i); strOp = "!="; break;
					case Expression.OpKind.Ge: r[i] =  ValueAt(i) || !v2.ValueAt(i); strOp = ">="; break;
					case Expression.OpKind.Gt: r[i] =  ValueAt(i) && !v2.ValueAt(i); strOp = ">";  break;
					default:
						throw new InvalidOperationException("Implementation: BoolVariable.BinaryRelation() called with wrong operation code");
				}
				m[i] = MissingAt(i) || v2.MissingAt(i) ;
			}
			BoolVariable v = new BoolVariable("(" + Name + ")" + strOp + "("+v2.Name+")",r,m);
			v.isConstant = isConstant && v2.IsConstant;
			return v;
		}

		public override StringVariable EvaluateFormat(StringVariable var2)
		{
			int M = Math.Max(Length,var2.Length);
			string[] r = new string[M];
			bool  [] m = new bool  [M];
			for(int i=0;i<M;i++)
			{
				r[i] = ValueAt(i).ToString();
				m[i] = MissingAt(i);
			}
			return new StringVariable("",r,m);
		}
		#endregion
	}

}
