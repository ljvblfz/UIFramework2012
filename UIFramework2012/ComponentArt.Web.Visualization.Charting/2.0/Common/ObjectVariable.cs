using System;
using System.Collections;
using System.Data;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class ObjectVariable : Variable
	{
		private ArrayList		val;

		internal ObjectVariable() {}

		public ObjectVariable(string name) : base(name)
		{ 
			isConstant = false;
			valueType = Expression.ValueType.String;
		}

		public ObjectVariable(string name, object val) : base(name)
		{ 
			this.val = new ArrayList();
			this.val.Add(val);
			IsMissing = new bool[1];
			IsMissing[0] = false;
			isConstant = true;
			valueType = Expression.ValueType.String;
		}

		public ObjectVariable(string name, object[] val) : this(name,val,null)
		{ }

		public ObjectVariable(string name, object[] val, bool[] missing) : base(name)
		{ 
			isConstant = false;
			Add(val,missing);
			valueType = Expression.ValueType.String;
		}

		public ArrayList Value { get { return val; } }

		public override object ItemAt(int index) { return ValueAt(index); }

		public object ValueAt(int x)
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
			object[] v = new object[1];
			v[0] = val;
			return Add(v);
		}

		public int Add(object[] val)
		{
			return Add(val,null);
		}

		public int Add(object[] val, bool[] missing)
		{
			if(IsConstant)
				throw new InvalidOperationException("Could not apply 'Add()' on a constant variable");
			if(missing != null && missing.Length != val.Length)
				throw new ArgumentException("Length of the 'missing' array is not equal to the length of 'val' array in 'Add()' operation");

			int L = val.Length;
			int i0;
			if(this.val == null)
			{
				this.val = new ArrayList(L);
				IsMissing = new bool[L];
				i0 = 0;
			}
			else
			{
				int L1 = this.val.Count;
				bool[] newMss = new bool[L+L1];
				for (int i=0;i<L1;i++)
				{
					newMss[i] = IsMissing[i];
				}
				IsMissing = newMss;
				i0 = L1;
			}

			for(int i=0;i<val.Length;i++)
			{
				this.val.Add(val[i]);
				if(missing != null)
					IsMissing[i0+i] = missing[i];
				else
					IsMissing[i0+i] = false;
			}

			return this.val.Count;
		}

		public override Variable Alternate(BoolVariable selection, Variable variable)
		{
			if(!(variable is ObjectVariable))
				throw new ArgumentException("Variables in the 'Alternate()' operation don't have the same type");
		
			ObjectVariable var = variable as ObjectVariable;
			int M = Math.Max(selection.Length,Math.Max(var.Length,Length));
			if( !this.IsConstant && this.Length!=M ||
				!var.IsConstant && var.Length!=M ||
				!selection.IsConstant && selection.Length!=M)
				throw new ArgumentException("Variables in the 'Alternate()' operation don't have the same length");

			if(IsConstant && var.IsConstant && selection.IsConstant)
			{
				if(selection.ValueAt(0))
					return new ObjectVariable("",this.ValueAt(0));
				else
					return new ObjectVariable("",var.ValueAt(0));
			}
			
			Object[] r = new Object[M];
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
			return new ObjectVariable("",r,m);
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
					return new ObjectVariable("",this.ValueAt(0));
				else
					return new ObjectVariable("",new object[] {} );
			}
			
			int L = 0;
			bool[] filter = selection.Value;

			for(int i=0; i<M; i++)
			{
				if(filter[i])
					L++;
			}

			bool[] m = new bool[L];
			object[] r = new object[L];

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
			return new ObjectVariable("",r,m);
		}

		public override BoolVariable BinaryRelation(Expression.OpKind operation, Variable var2)
		{
			throw new ArgumentException("Binary relations not defined for ObjectVariable kind");
		}
	}
}
