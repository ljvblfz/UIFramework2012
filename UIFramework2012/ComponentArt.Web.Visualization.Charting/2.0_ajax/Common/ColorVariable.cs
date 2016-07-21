using System;
using System.Collections;
using System.Data;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class ColorVariable : Variable
	{
		protected Color[]	val;

		internal ColorVariable() {}

		public ColorVariable(string name) : base(name)
		{ 
			isConstant = false;
			valueType = Expression.ValueType.Color;
		}

		public ColorVariable(string name, Color val) : base(name)
		{ 
			this.val = new Color[1];
			this.val[0] = val;
			IsMissing = new bool[1];
			IsMissing[0] = false;
			isConstant = true;
			valueType = Expression.ValueType.Color;
		}

		public ColorVariable(string name, Color[] val) : this(name,val,null)
		{ }

		public ColorVariable(string name, Color[] val, bool[] missing) : base(name)
		{ 
			isConstant = false;
			Add(val,missing);
			valueType = Expression.ValueType.Color;
		}

		public new Color[] Value { get { return val; } }

		public Color this[int x]
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

		public Color ValueAt(int x)
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
			if (val.GetType() != typeof(Color)) 
				throw new ArgumentException( "value must be of type Color.", "val" );

			Color[] v = new Color[1];
			v[0] = (Color)val;
			return Add(v);
		}

		public int Add(Color[] val)
		{
			return Add(val,null);
		}

		public int Add(Color[] val, bool[] missing)
		{
			if(IsConstant)
				throw new InvalidOperationException("Could not apply 'Add()' on a constant variable");
			if(missing != null && missing.Length != val.Length)
				throw new ArgumentException("Length of the 'missing' array is not equal to the length of 'val' array in 'Add()' operation");

			int L = val.Length;
			int i0;
			if(this.val == null)
			{
				this.val = new Color[L];
				IsMissing = new bool[L];
				i0 = 0;
			}
			else
			{
				int L1 = this.val.Length;
				Color[] newVal = new Color[L+L1];
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
			if(!(variable is ColorVariable))
				throw new ArgumentException("Variables in the 'Alternate()' operation don't have the same type");
		
			ColorVariable var = variable as ColorVariable;
			int M = Math.Max(selection.Length,Math.Max(var.Length,Length));
			if( !this.IsConstant && this.Length!=M ||
				!var.IsConstant && var.Length!=M ||
				!selection.IsConstant && selection.Length!=M)
				throw new ArgumentException("Variables in the 'Alternate()' operation don't have the same length");

			if(IsConstant && var.IsConstant && selection.IsConstant)
			{
				if(selection.ValueAt(0))
					return new ColorVariable("",this.ValueAt(0));
				else
					return new ColorVariable("",var.ValueAt(0));
			}
			
			Color[] r = new Color[M];
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
			return new ColorVariable("",r,m);
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
					return new ColorVariable("",this.ValueAt(0));
				else
					return new ColorVariable("",new Color[] {} );
			}
			
			int L = 0;
			bool[] filter = selection.Value;

			for(int i=0; i<M; i++)
			{
				if(filter[i])
					L++;
			}

			bool[] m = new bool[L];
			Color[] r = new Color[L];

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
			return new ColorVariable("",r,m);
		}

		public override BoolVariable BinaryRelation(Expression.OpKind operation, Variable var2)
		{
			ColorVariable v2 = var2 as ColorVariable;
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
					case Expression.OpKind.Eq: r[i] = this[i] == v2[i]; strOp = "=="; break;
					case Expression.OpKind.Ne: r[i] = this[i] != v2[i]; strOp = "!="; break;
					default:
						throw new InvalidOperationException("ColorVariable.BinaryRelation() called with wrong operation code");
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
				r[i] = "#" + this[i].A.ToString("XX") + this[i].R.ToString("XX") + this[i].G.ToString("XX") + this[i].B.ToString("XX");;
				m[i] = MissingAt(i);
			}
			return new StringVariable("",r,m);
		}
		#endregion
	}

}
