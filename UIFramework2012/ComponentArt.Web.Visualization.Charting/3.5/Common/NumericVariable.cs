using System;
using System.Collections;
using System.Data;


namespace ComponentArt.Web.Visualization.Charting
{
	internal class NumericVariable : Variable
	{
		protected double[]	val;
		public NumericVariable(string name) : base(name)
		{ 
			isConstant = false;
			valueType = Expression.ValueType.Numeric;
		}

		public NumericVariable(string name, double val) : base(name)
		{ 
			this.val = new double[1];
			this.val[0] = val;
			IsMissing = new bool[1];
			IsMissing[0] = false;
			isConstant = true;
			valueType = Expression.ValueType.Numeric;
		}

		internal NumericVariable() {}

		public NumericVariable(double val) : this(val.ToString(), val) { }

		public NumericVariable(string name, double[] val) : this(name,val,null)
		{ }

		public NumericVariable(string name, double[] val, bool[] missing) : base(name)
		{ 
			isConstant = false;
			Add(val,missing);
			valueType = Expression.ValueType.Numeric;
		}

		public bool HasIntegralValues()
		{
			for(int i=0; i<val.Length; i++)
				if(val[i] != Math.Floor(val[i]))
					return false;
			return true;
		}

		internal override void CreateDimension ()
		{
			dimension = DataDimension.StandardNumericDimension;
		}
		public new double[] Value { get { return val; } }
		public double this[int x]
		{
			get
			{
				if(IsConstant)
					return val[0];
				if(0<=x && x<val.Length)
					return val[x];
				else
					throw new IndexOutOfRangeException();
			}
			set
			{
				if(0<=x && x<val.Length)
					val[x] = value;
				else
					throw new IndexOutOfRangeException();
			}
		}

		public override object ItemAt(int index) { return ValueAt(index); }

		public double ValueAt(int x)
		{
			if(IsConstant)
				return val[0];
			else
				return val[x];
		}


		public double Min
		{
			get
			{
				double y = this[0];
				for(int i=1; i<Length; i++)
					y = Math.Min(y,this[i]);
				return y;
			}
		}

		public double Max
		{
			get
			{
				double y = this[0];
				for(int i=1; i<Length; i++)
					y = Math.Max(y,this[i]);
				return y;
			}
		}

		public override void Clear() 
		{
			val = null;
			IsMissing = null;
		}

		
		public override int Add(object val)
		{
			if (val.GetType() != typeof(double) && val.GetType() != typeof(System.Int32)) 
				throw new ArgumentException("value must be of numeric type.", "val" );

			if (val.GetType() == typeof(System.Int32)) 
			{
				val = Convert.ToDouble(val);
			}

			double[] v = new double[1];
			v[0] = (double)val;
			return Add(v);
		}

		public int Add(double[] val)
		{
			return Add(val,null);
		}

		public int Add(double[] val, bool[] missing)
		{
			if(IsConstant)
				throw new InvalidOperationException("Could not apply 'Add()' on a constant variable");
			if(missing != null && missing.Length != val.Length)
				throw new ArgumentException("Length of the 'missing' array is not equal to the length of 'val' array in 'Add()' operation");

			int L = val.Length;
			int i0;
			if(this.val == null)
			{
				this.val = new double[L];
				IsMissing = new bool[L];
				i0 = 0;
			}
			else
			{
				int L1 = this.val.Length;
				double[] newVal = new double[L+L1];
				bool  [] newMss = new bool  [L+L1];
				for (int i=0;i<L1;i++)
				{
					newVal[i] = this.Value[i];
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
			if(!(variable is NumericVariable))
				throw new ArgumentException("Variables in the 'Alternate()' operation don't have the same type");
		
			NumericVariable var = variable as NumericVariable;
			int M = Math.Max(selection.Length,Math.Max(var.Length,Length));
			if( !this.IsConstant && this.Length!=M ||
				!var.IsConstant && var.Length!=M ||
				!selection.IsConstant && selection.Length!=M)
				throw new ArgumentException("Variables in the 'Alternate()' operation don't have the same length");

			if(IsConstant && var.IsConstant && selection.IsConstant)
			{
				if(selection.ValueAt(0))
					return new NumericVariable(this.ValueAt(0));
				else
					return new NumericVariable(var.ValueAt(0));
			}
			
			double[] r = new Double[M];
			bool  [] m = new bool  [M];
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
			return new NumericVariable("",r,m);
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
					return new NumericVariable(this.ValueAt(0));
				else
					return new NumericVariable("",new double[] {} );
			}
			
			int L = 0;
			bool[] filter = selection.Value;

			for(int i=0; i<M; i++)
			{
				if(filter[i])
					L++;
			}

			double[] r = new double[L];
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
			return new NumericVariable("",r,m);
		}

		public NumericVariable Neg()
		{
			double[] r = new double[Length];
			bool  [] m = new bool[Length];
			for(int i=0; i<Length; i++)
			{
				r[i] = -val[i];
				m[i] = IsMissing[i];
			}
			NumericVariable v = new NumericVariable("-(" + Name + ")",r,m);
			v.isConstant = isConstant;
			return v;
		}

		public NumericVariable BinaryOperation(Expression.OpKind operation, NumericVariable v2)
		{
			int M = Math.Max(Length,v2.Length);
			string strOp="";
			double[] r = new double[M];
			bool  [] m = new bool[M];
			for(int i=0; i<M; i++)
			{
				m[i] = false;
				try
				{
					switch(operation)
					{
						case Expression.OpKind.Add: r[i] = ValueAt(i) + v2.ValueAt(i); strOp = "+"; break;
						case Expression.OpKind.Sub: r[i] = ValueAt(i) - v2.ValueAt(i); strOp = "-"; break;
						case Expression.OpKind.Mul: r[i] = ValueAt(i) * v2.ValueAt(i); strOp = "*"; break;
						case Expression.OpKind.Div: r[i] = ValueAt(i) / v2.ValueAt(i); strOp = "/"; break;
						case Expression.OpKind.Min: r[i] = Math.Min(ValueAt(i), v2.ValueAt(i)); strOp = ".Min"; break;
						case Expression.OpKind.Max: r[i] = Math.Max(ValueAt(i), v2.ValueAt(i)); strOp = ".Max"; break;
						case Expression.OpKind.Atan2: r[i] = Math.Atan2(ValueAt(i), v2.ValueAt(i)); strOp = ".Atan2"; break;
						case Expression.OpKind.Pow : r[i] = Math.Pow(ValueAt(i),v2.ValueAt(i)); strOp = "Pow";      break;
						default:
							throw new InvalidOperationException("Implementation: DoubleVariable.BinaryOperation() called with wrong operation code");
					}
				}
				catch(System.ArithmeticException)
				{
					m[i] = true;
					r[i] = 0.0;
				}

				m[i] = m[i] || MissingAt(i) || v2.MissingAt(i) ;
			}
			
			NumericVariable v = new NumericVariable("(" + Name + ")" + strOp + "("+v2.Name+")",r,m);
			v.isConstant = isConstant && v2.IsConstant;
			return v;
		}

		public override BoolVariable BinaryRelation(Expression.OpKind operation, Variable var2)
		{
			NumericVariable v2 = var2 as NumericVariable;
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

		public NumericVariable Function(Expression.OpKind operation)
		{
			if(operation == Expression.OpKind.Alias)
				return this;
			
			string strOp="";
			double[] r = new double[Length];
			bool  [] m = new bool[Length];
			for(int i=0; i<Length; i++)
			{
				switch(operation)
				{
					case Expression.OpKind.Abs    : r[i] = Math.Abs    (ValueAt(i)); strOp = "Abs";      break;
					case Expression.OpKind.Sign   : r[i] = Math.Sign   (ValueAt(i)); strOp = "Sign";     break;
					case Expression.OpKind.Sin    : r[i] = Math.Sin    (ValueAt(i)); strOp = "Sin";      break;
					case Expression.OpKind.Cos    : r[i] = Math.Cos    (ValueAt(i)); strOp = "Cos";      break;
					case Expression.OpKind.Tan    : r[i] = Math.Tan    (ValueAt(i)); strOp = "Tan";      break;
					case Expression.OpKind.Sqrt   : r[i] = Math.Sqrt   (ValueAt(i)); strOp = "Sqrt";     break;
					case Expression.OpKind.Asin   : r[i] = Math.Asin   (ValueAt(i)); strOp = "Asin";     break;
					case Expression.OpKind.Acos   : r[i] = Math.Acos   (ValueAt(i)); strOp = "Acos";     break;
					case Expression.OpKind.Atan   : r[i] = Math.Atan   (ValueAt(i)); strOp = "Atan";     break;
					case Expression.OpKind.Sinh   : r[i] = Math.Sinh   (ValueAt(i)); strOp = "Sinh";     break;
					case Expression.OpKind.Cosh   : r[i] = Math.Cosh   (ValueAt(i)); strOp = "Cosh";     break;
					case Expression.OpKind.Tanh   : r[i] = Math.Tanh   (ValueAt(i)); strOp = "Tanh";     break;
					case Expression.OpKind.Exp    : r[i] = Math.Exp    (ValueAt(i)); strOp = "Exp";      break;
					case Expression.OpKind.Log    : r[i] = Math.Log    (ValueAt(i)); strOp = "Log";      break;
					case Expression.OpKind.Log10  : r[i] = Math.Log10  (ValueAt(i)); strOp = "Log10";    break;
					case Expression.OpKind.Floor  : r[i] = Math.Floor  (ValueAt(i)); strOp = "Floor";    break;
					case Expression.OpKind.Ceiling: r[i] = Math.Ceiling(ValueAt(i)); strOp = "Ceiling";  break;
					default:
						throw new InvalidOperationException("Implementation: DoubleVariable.Function() called with wrong operation code");
				}
				r[i] = -val[i];
				m[i] = IsMissing[i];
			}
			NumericVariable v = new NumericVariable("(" + Name + ")" + strOp + "()" ,r,m);
			v.isConstant = isConstant;
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
