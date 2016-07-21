using System;
using System.Collections;
using System.Collections.Specialized;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	
	internal enum SeriesValueKind 
	{
		NumericSeries, 
		CurrencySeries,
		DateSeries,
		DateTimeSeries,
		CategorycalSeries 
	}

	[Serializable()]
	internal class ExpressionCollection : NamedCollection
	{
		private	bool	evaluated;
		private	int		numberOfSamples;

		public ExpressionCollection() : base(typeof(Expression))
		{ }

		public int NumberOfSamples
		{
			get
			{
				Evaluate();
				return numberOfSamples;
			}
		}

		private void Evaluate()
		{
			if(evaluated) return;

			numberOfSamples = 0;
			for(int i=0; i<this.Count; i++)
			{
				Variable var = this[i].Value;
				numberOfSamples = Math.Max(numberOfSamples,var.Length);
			}
			evaluated = true;
		}

		public NumericVariable Add(string name, double[] values)
		{
			NumericVariable v = new NumericVariable(name, values);
			Add(v);
			return v;
		}

		public NumericVariable Add(string name, double val)
		{
			NumericVariable v = new NumericVariable(name, val);
			Add(v);
			return v;
		}

		public DateTimeVariable Add(string name, DateTime[] values)
		{
			DateTimeVariable v = new DateTimeVariable(name, values);
			Add(v);
			return v;
		}

		public DateTimeVariable Add(string name, DateTime val)
		{
			DateTimeVariable v = new DateTimeVariable(name,val);
			Add(v);
			return v;
		}

		public StringVariable Add(string name, string[] values)
		{
			StringVariable v = new StringVariable(name, values);
			Add(v);
			return v;
		}

		public StringVariable Add(string name)
		{
			StringVariable v = new StringVariable(name);
			Add(v);
			return v;
		}

		#region --- Aliases ---
		public void AddAlias(string name, string aliasName)
		{
			Expression e = this[name];
			if( e== null)
				throw new ArgumentException("Expression '" + name + "' does not exist");
			Expression alias = new Expression(Expression.OpKind.Alias,e);
			alias.SetName(aliasName);
			Add(alias);
		}

		public void AddAlias(string[] names, string[] aliasNames)
		{
			if(names.Length != aliasNames.Length)
				throw new ArgumentException("String arrays in 'AddAlias' do not have equal lengths");
			for(int i=0; i<names.Length; i++)
				AddAlias(names[i], aliasNames[i]);
		}
		#endregion

		#region --- Collection Implementation ---
		public new Expression this[ object index ]  
		{
			get { return  (Expression)base[index]; }
			set { base[index] = value; }
		}

		// Gets or sets the value associated with the specified key.
		public Expression this[ String expressionName ]  
		{
			get  
			{
				return( base[expressionName] as Expression );
			}
			set  
			{
				evaluated = false;
				base[expressionName] = value;
			}
		}

		public void Remove( String expressionName )  { base.Remove(this[expressionName]);evaluated = false; }
		public void Remove( int index )  { base.Remove(this[index]); evaluated = false; }
		public new void Clear()  { base.Clear(); evaluated = false; }
		
		#endregion
	}
}
