using System;
using System.Collections;
using System.Data;


namespace ComponentArt.Web.Visualization.Charting
{
	internal class StringVariable : Variable
	{
		protected string[]	val;

		internal StringVariable() {}

		public StringVariable(string name) : base(name)
		{ 
			isConstant = false;
			valueType = Expression.ValueType.String;
		}


		public StringVariable(string name, string val) : base(name)
		{ 
			this.val = new string[1];
			this.val[0] = val;
			IsMissing = new bool[1];
			IsMissing[0] = false;
			isConstant = true;
			valueType = Expression.ValueType.String;
		}

		public StringVariable(string name, string[] val) : this(name,val,null)
		{ }

		public StringVariable(string name, string[] val, bool[] missing) : base(name)
		{ 
			isConstant = false;
			Add(val,missing);
			valueType = Expression.ValueType.String;
		}

		internal override void CreateDimension()
		{
			bool[] z = new Boolean[1024];
			EnumeratedDataDimension strDimension = new EnumeratedDataDimension(Name,typeof(string));
			for(int i=0;i<Length;i++)
			{
				int x = Math.Abs(val[i].GetHashCode())%1024;
				bool found = false;
				if(z[x])
				{
					for(int j=0; j<i; j++)
					{
						if(val[i] == val[j])
						{
							found = true;
							break;
						}
					}
				}
				else
					z[x] = true;
				if(!found)
					strDimension.Add(this[i]);
			}
			dimension = strDimension;
		}

		public string[] Value { get { return val; } }

		public string this[int x]
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

		public string ValueAt(int x)
		{
			if(IsConstant)
				return val[0];
			else
				return val[x];
		}

		public StringVariable Trim()
		{
			string[] tstr = new string[val.Length];
			bool[] tmiss = new bool[val.Length];
			for(int i=0; i<val.Length; i++)
			{
				tstr[i] = val[i].Trim();
				tmiss[i] = IsMissing[i];
			}
			return new StringVariable("Trim(" + Name +")",tstr,tmiss);
		}

		public override void Clear() 
		{
			val = null;
			IsMissing = null;
		}

		public override int Add(object val)
		{
			if (val.GetType() != typeof(string)) 
				throw new ArgumentException( "value must be of type string.", "val" );

			string[] v = new string[1];
			v[0] = (string)val;
			return Add(v);
		}

		public int Add(string[] val)
		{
			return Add(val,null);
		}

		public int Add(string[] val, bool[] missing)
		{
			if(IsConstant)
				throw new InvalidOperationException("Could not apply 'Add()' on a constant variable");
			if(missing != null && missing.Length != val.Length)
				throw new ArgumentException("Length of the 'missing' array is not equal to the length of 'val' array in 'Add()' operation");

			int L = val.Length;
			int i0;
			if(this.val == null)
			{
				this.val = new string[L];
				IsMissing = new bool[L];
				i0 = 0;
			}
			else
			{
				int L1 = this.val.Length;
				string[] newVal = new string[L+L1];
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

		public StringVariable Concatenate(StringVariable a2)
		{
			if(IsConstant && a2.IsConstant)
				return new StringVariable(ValueAt(0) + a2.ValueAt(0));
			
			int M = Math.Max(Length,a2.Length);
			int Min = Math.Min(Length,a2.Length);
			if(M != Min && Min>1) // both non-constants and different length
				throw new ArgumentException("Concatenating variables that don't have the same length");

			string[] r = new string[M];
			bool  [] m = new bool  [M];
			for(int i=0; i<M; i++)
			{
				m[i] = this.MissingAt(i) || a2.MissingAt(i);
				if(m[i])
					r[i] = "";
				else
					r[i] = this.ValueAt(i) + a2.ValueAt(i);
			}
			return new StringVariable("",r,m);
		}

		public override Variable Alternate(BoolVariable selection, Variable variable)
		{
			if(!(variable is StringVariable))
				throw new ArgumentException("Variables in the 'Alternate()' operation don't have the same type");
		
			StringVariable var = variable as StringVariable;
			int M = Math.Max(selection.Length,Math.Max(var.Length,Length));
			if( !this.IsConstant && this.Length!=M ||
				!var.IsConstant && var.Length!=M ||
				!selection.IsConstant && selection.Length!=M)
				throw new ArgumentException("Variables in the 'Alternate()' operation don't have the same length");

			if(IsConstant && var.IsConstant && selection.IsConstant)
			{
				if(selection.ValueAt(0))
					return new StringVariable(this.ValueAt(0));
				else
					return new StringVariable(var.ValueAt(0));
			}
			
			string[] r = new string[M];
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
			return new StringVariable("",r,m);
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
					return new StringVariable(this.ValueAt(0));
				else
					return new StringVariable("",new string[] {} );
			}
			
			int L = 0;
			bool[] filter = selection.Value;

			for(int i=0; i<M; i++)
			{
				if(filter[i])
					L++;
			}

			bool[] m = new bool[L];
			string[] r = new string[L];

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
			return new StringVariable("",r,m);
		}

		public override BoolVariable BinaryRelation(Expression.OpKind operation, Variable var2)
		{
			StringVariable v2 = var2 as StringVariable;
			if(v2==null)
				throw new ArgumentException("Variables in the binary relation don't have the same type");

			int M = Math.Max(Length,v2.Length);
			string strOp="";
			bool[] r = new bool[M];
			bool[] m = new bool[M];
			for(int i=0; i<M; i++)
			{
				int c = ValueAt(i).CompareTo(v2.ValueAt(i));
				switch(operation)
				{
					case Expression.OpKind.Lt: r[i] = c <  0; strOp = "<";  break;
					case Expression.OpKind.Le: r[i] = c <= 0; strOp = "<="; break;
					case Expression.OpKind.Eq: r[i] = c == 0; strOp = "=="; break;
					case Expression.OpKind.Ne: r[i] = c != 0; strOp = "!="; break;
					case Expression.OpKind.Ge: r[i] = c >= 0; strOp = ">="; break;
					case Expression.OpKind.Gt: r[i] = c >  0; strOp = ">";  break;
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
				r[i] = ValueAt(i);
				m[i] = MissingAt(i);
			}
			return new StringVariable("",r,m);
		}
		#endregion
	}

}
