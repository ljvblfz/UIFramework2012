using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class Expression : INamedObject
	{
		internal enum OpKind
		{
			Self,

			Neg,
			Not,

			Missing,

			Add,
			Sub,
			Mul,
			Div,
			Atan2,
			Pow,
			Min,
			Max,

			Format,

			Le,
			Lt,
			Eq,
			Ne,
			Gt,
			Ge,
			
			And,
			Or,

			Abs,
			Alias,
			Sign,
			Sin,
			Cos,
			Tan,
			Sqrt,
			Asin,
			Acos,
			Atan,
			Sinh,
			Cosh,
			Tanh,
			Exp,
			Log,
			Log10,
			Floor,
			Ceiling,

			Select,
			Filter,
			Random,

			Count,
			Avg,
			Sum,
			MinAgg,
			MaxAgg,
			Span,
			First,
			Last,

			OrderDesc,
			OrderAsc,
			Sort,

			Accum
		};

		public enum ValueType
		{
			Numeric,
			Bool,
			DateTime,
			String,
			Color,
			Object,
			Invalid
		}

		protected	OpKind		operation;
		protected	ArrayList	subExpressions;
		protected	Variable	expressionValue;
		protected	ValueType	valueType;
		protected	string		message;
		internal NamedStyleInternal m_nsi = new NamedStyleInternal();

		#region --- Constructors ---
		public Expression(string name)
		{
			valueType = ValueType.Invalid;
		}

		internal Expression() {}

		public Expression(OpKind operation)
		{
			this.operation = operation;
			valueType = ValueType.Invalid;
		}
		
		public Expression(OpKind operation, Expression e1)
		{
			this.operation = operation;
			AddSubexpression(e1);
		}
		
		public Expression(OpKind operation, Expression e1, Expression e2)
		{
			this.operation = operation;
			AddSubexpression(e1);
			AddSubexpression(e2);
		}
		
		public Expression(OpKind operation, Expression e1, Expression e2, Expression e3)
		{
			this.operation = operation;
			AddSubexpression(e1);
			AddSubexpression(e2);
			AddSubexpression(e3);
		}

		public Expression(OpKind operation, Expression e1, Expression e2, Expression e3, Expression e4)
		{
			this.operation = operation;
			AddSubexpression(e1);
			AddSubexpression(e2);
			AddSubexpression(e3);
			AddSubexpression(e4);
		}
		#endregion

		#region --- Queries ---
		public string Name { get { return m_nsi.Name; } set {m_nsi.Name = value;} }

		private bool ShouldSerializeNamedCollection() { return false; }
		[Browsable(false)]
		public NamedCollection OwningCollection 
		{
			get {return m_nsi.NamedCollection;} set { m_nsi.NamedCollection = value; }
		}

		internal void SetName (string name ) { Name = name; }
		public Variable Value
		{
			get
			{
				if(expressionValue == null)
				{
					if(!Evaluate(false,ref message))
						throw new InvalidOperationException(message);
				}
				return expressionValue;
			}
		}

		public virtual ValueType ValType
		{
			get 
			{
				string result = "";
				this.Evaluate(true, ref result);
				return valueType;
			}
		}
		
		#endregion

		#region --- Building Expressions ---

		public void AddSubexpression(Expression e)
		{
			if(subExpressions == null)
				subExpressions = new ArrayList();
			subExpressions.Add(e);
		}

		public Expression SubExpression(int x)
		{
			if(x<0 || subExpressions==null || subExpressions.Count<=x)
				return null;
			else
				return subExpressions[x] as Expression;
		}


		#region --- Operators ---
		public static implicit operator Expression(double x)
		{
			return new NumericVariable(x);
		}

		public static implicit operator Expression(bool x)
		{
			return new BoolVariable(x);
		}

		public static implicit operator Expression(string x)
		{
			return new StringVariable(x);
		}

		public static implicit operator Expression(DateTime x)
		{
			return new DateTimeVariable(x);
		}

		public static implicit operator Expression(Color x)
		{
			return new ColorVariable("",x);
		}

		public static implicit operator Expression(double[] x)
		{
			return new NumericVariable("", x);
		}

		public static implicit operator Expression(bool[] x)
		{
			return new BoolVariable("", x);
		}

		public static implicit operator Expression(string[] x)
		{
			return new StringVariable("", x);
		}

		public static implicit operator Expression(DateTime[] x)
		{
			return new DateTimeVariable("", x);
		}

		public static implicit operator Expression(Color[] x)
		{
			return new ColorVariable("", x);
		}

		#endregion

		#region --- Unary Expressions ---
		public static Expression operator + (Expression e1) { return e1; }
		public static Expression operator - (Expression e1) { return new Expression(OpKind.Neg,e1); }
		public static Expression operator ! (Expression e1) { return new Expression(OpKind.Not,e1); }
		#endregion

		#region --- Binary Expressions ---
		public static Expression operator + (Expression e1, Expression e2) { return new Expression(OpKind.Add,e1,e2); }
		public static Expression operator - (Expression e1, Expression e2) { return new Expression(OpKind.Sub,e1,e2); }
		public static Expression operator * (Expression e1, Expression e2) { return new Expression(OpKind.Mul,e1,e2); }
		public static Expression operator / (Expression e1, Expression e2) { return new Expression(OpKind.Div,e1,e2); }
		public static Expression operator < (Expression e1, Expression e2) { return new Expression(OpKind.Lt ,e1,e2); }
		public static Expression operator <=(Expression e1, Expression e2) { return new Expression(OpKind.Le ,e1,e2); }
		public static Expression operator > (Expression e1, Expression e2) { return new Expression(OpKind.Gt ,e1,e2); }
		public static Expression operator >=(Expression e1, Expression e2) { return new Expression(OpKind.Ge ,e1,e2); }
		public static Expression operator & (Expression e1, Expression e2) { return new Expression(OpKind.And,e1,e2); }
		public static Expression operator | (Expression e1, Expression e2) { return new Expression(OpKind.Or ,e1,e2); }

		public static Expression operator + (double e1, Expression e2) { return new Expression(OpKind.Add,new NumericVariable(e1),e2); }
		public static Expression operator - (double e1, Expression e2) { return new Expression(OpKind.Sub,new NumericVariable(e1),e2); }
		public static Expression operator * (double e1, Expression e2) { return new Expression(OpKind.Mul,new NumericVariable(e1),e2); }
		public static Expression operator / (double e1, Expression e2) { return new Expression(OpKind.Div,new NumericVariable(e1),e2); }
		public static Expression operator < (double e1, Expression e2) { return new Expression(OpKind.Lt ,new NumericVariable(e1),e2); }
		public static Expression operator <=(double e1, Expression e2) { return new Expression(OpKind.Le ,new NumericVariable(e1),e2); }
		public static Expression operator > (double e1, Expression e2) { return new Expression(OpKind.Gt ,new NumericVariable(e1),e2); }
		public static Expression operator >=(double e1, Expression e2) { return new Expression(OpKind.Ge ,new NumericVariable(e1),e2); }
		public static Expression operator & (double e1, Expression e2) { return new Expression(OpKind.And,new NumericVariable(e1),e2); }
		public static Expression operator | (double e1, Expression e2) { return new Expression(OpKind.Or ,new NumericVariable(e1),e2); }
		
		public static Expression operator + (Expression e1, double e2) { return new Expression(OpKind.Add,e1,new NumericVariable(e2)); }
		public static Expression operator - (Expression e1, double e2) { return new Expression(OpKind.Sub,e1,new NumericVariable(e2)); }
		public static Expression operator * (Expression e1, double e2) { return new Expression(OpKind.Mul,e1,new NumericVariable(e2)); }
		public static Expression operator / (Expression e1, double e2) { return new Expression(OpKind.Div,e1,new NumericVariable(e2)); }
		public static Expression operator < (Expression e1, double e2) { return new Expression(OpKind.Lt ,e1,new NumericVariable(e2)); }
		public static Expression operator <=(Expression e1, double e2) { return new Expression(OpKind.Le ,e1,new NumericVariable(e2)); }
		public static Expression operator > (Expression e1, double e2) { return new Expression(OpKind.Gt ,e1,new NumericVariable(e2)); }
		public static Expression operator >=(Expression e1, double e2) { return new Expression(OpKind.Ge ,e1,new NumericVariable(e2)); }
		public static Expression operator & (Expression e1, double e2) { return new Expression(OpKind.And,e1,new NumericVariable(e2)); }
		public static Expression operator | (Expression e1, double e2) { return new Expression(OpKind.Or ,e1,new NumericVariable(e2)); }

		public Expression Eq		(Expression e2) { return new Expression(OpKind.Eq ,		this,e2); }
		public Expression Ne		(Expression e2) { return new Expression(OpKind.Ne ,		this,e2); }
		public Expression Min		(Expression e2) { return new Expression(OpKind.Min,		this,e2); }
		public Expression Max		(Expression e2) { return new Expression(OpKind.Max,		this,e2); }
		public Expression Format	(Expression e2) { return new Expression(OpKind.Format,	this,e2); }
		public Expression Format	(string formatString) 
		{ 
			return new Expression(OpKind.Format,this,new StringVariable(formatString)); 
		}
		public Expression Format	() 
		{ 
			return Format("G");
		}
		#endregion

		#region --- Functions ---
		public Expression Missing() { return new Expression(OpKind.Missing,this); }
		public Expression Abs	 () { return new Expression(OpKind.Abs    ,this); }
		public Expression Sign	 () { return new Expression(OpKind.Sign   ,this); }
		public Expression Sin	 () { return new Expression(OpKind.Sin    ,this); }
		public Expression Cos	 () { return new Expression(OpKind.Cos    ,this); }
		public Expression Tan	 () { return new Expression(OpKind.Tan    ,this); }
		public Expression Sqrt	 () { return new Expression(OpKind.Sqrt   ,this); }
		public Expression Asin	 () { return new Expression(OpKind.Asin   ,this); }
		public Expression Acos	 () { return new Expression(OpKind.Acos   ,this); }
		public Expression Atan	 () { return new Expression(OpKind.Atan   ,this); }
		public Expression Sinh	 () { return new Expression(OpKind.Sinh   ,this); }
		public Expression Cosh	 () { return new Expression(OpKind.Cosh   ,this); }
		public Expression Exp	 () { return new Expression(OpKind.Exp    ,this); }
		public Expression Log	 () { return new Expression(OpKind.Log    ,this); }
		public Expression Log10	 () { return new Expression(OpKind.Log10  ,this); }
		public Expression Floor	 () { return new Expression(OpKind.Floor  ,this); }
		public Expression Ceiling() { return new Expression(OpKind.Ceiling,this); }

		public Expression Count    () { return new Expression(OpKind.Count    ,this); }
		public Expression Avg      () { return new Expression(OpKind.Avg      ,this); }
		public Expression Sum      () { return new Expression(OpKind.Sum      ,this); }
		public Expression MinAgg   () { return new Expression(OpKind.MinAgg   ,this); }
		public Expression MaxAgg   () { return new Expression(OpKind.MaxAgg   ,this); }
		public Expression Span     () { return new Expression(OpKind.Span     ,this); }
		public Expression First    () { return new Expression(OpKind.First    ,this); }
		public Expression Last     () { return new Expression(OpKind.Last     ,this); }
		public Expression OrderDesc() { return new Expression(OpKind.OrderDesc,this); }
		public Expression OrderAsc () { return new Expression(OpKind.OrderAsc ,this); }
		public Expression Accum    () { return new Expression(OpKind.Accum    ,this); }

		public Expression Atan2	(Expression e2) { return new Expression(OpKind.Atan2 ,this,e2); }
		public Expression Pow	(Expression e2) { return new Expression(OpKind.Pow   ,this,e2); }

		public Expression And	(Expression e2) { return new Expression(OpKind.And   ,this,e2); }
		public Expression Or	(Expression e2) { return new Expression(OpKind.Or    ,this,e2); }

		public Expression Sort  (Expression e2) { return new Expression(OpKind.Sort  ,this,e2); }
#endregion

		#region --- Selection Expression ---
		public Expression Select	(Expression e1, Expression e2) { return new Expression(OpKind.Select,this,e1,e2); }
		public Expression Filter	(Expression e)				   { return new Expression(OpKind.Select,this,e); }
		#endregion

		#region --- Random Expression ---
		// this is seed, 
		//		e1 is number of points, 
		//		e2 is min value,
		//		e3 is max value,
		//		result is random sequence between min and max
		public Expression Random	(Expression e1,Expression e2,Expression e3) { return new Expression(OpKind.Random,this,e1,e2,e3); }
		#endregion
		
		#endregion

		#region --- Evaluating Expressions ---

		internal bool Evaluate(bool typeOnly, ref string message)
		{
			message = "";
			// Check for concatenation before anything else
			if(operation==OpKind.Add && 
				(SubExpression(0).ValType == Expression.ValueType.String ||
				SubExpression(1).ValType == Expression.ValueType.String)
				)
				return EvaluateConcatenation(typeOnly, ref message);
			//
			if(operation == OpKind.Self)
				return true;
			else if(OpKind.Neg<=operation && operation<=OpKind.Not)
				return EvaluateUnaryExpression(typeOnly, ref message);
			else if(OpKind.Count<=operation && operation<=OpKind.Last)
				return EvaluateAggregateExpression(typeOnly, ref message);
			else if(operation==OpKind.Missing)
				return EvaluateMissing(typeOnly, ref message);
			else if(OpKind.Add<=operation && operation<=OpKind.Max)
				return EvaluateBinaryDoubleExpression(typeOnly, ref message);
			else if(operation==OpKind.Format)
				return EvaluateFormat(typeOnly, ref message);
			else if(OpKind.Le<=operation && operation<=OpKind.Ge)
				return EvaluateRelationExpression(typeOnly, ref message);
			else if(OpKind.And<=operation && operation<=OpKind.Or)
				return EvaluateBinaryBoolExpression(typeOnly, ref message);
			else if(OpKind.Abs<=operation && operation<=OpKind.Ceiling)
				return EvaluateFunctionCallExpression(typeOnly, ref message);
			else if(operation==OpKind.Select)
				return EvaluateSelectExpression(typeOnly, ref message);
			else if(operation==OpKind.Random)
				return EvaluateRandomExpression(typeOnly, ref message);
			
			else if(operation==OpKind.OrderAsc)
				return EvaluateOrderExpression(typeOnly, ref message, true);
			else if(operation==OpKind.OrderDesc)
				return EvaluateOrderExpression(typeOnly, ref message, false);
			else if(operation==OpKind.Sort)
				return EvaluateSortExpression(typeOnly, ref message);
			else if(operation==OpKind.Accum)
				return EvaluateAccumExpression(typeOnly, ref message);
			
			
			else
				throw new InvalidOperationException("Implementation: Evaluating unknown type of expression");
		}

		private bool EvaluateAccumExpression(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			if(!e1.Evaluate(typeOnly,ref message))
			{
				valueType = ValueType.Invalid;
				return false;
			}
			if(e1.ValType != ValueType.Numeric)
			{
				valueType = ValueType.Invalid;
				message = "'" + e1.Name + "' is not numeric";
				return false;
			}

			NumericVariable arg = e1.Value as NumericVariable;
			NumericVariable acc = new NumericVariable("accum(" + e1.Name +")");
			double y = 0;
			for(int i=0; i<arg.Length; i++)
			{
				y += arg[i];
				acc.Add(y);
			}

			expressionValue = acc;
			return true;
		}

		private bool EvaluateSortExpression(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			if(!e1.Evaluate(typeOnly,ref message))
			{
				valueType = ValueType.Invalid;
				return false;
			}
			Expression e2 = SubExpression(1);
			if(!e2.Evaluate(typeOnly,ref message))
			{
				valueType = ValueType.Invalid;
				return false;
			}
			if(e2.ValType != ValueType.Numeric)
			{
				valueType = ValueType.Invalid;
				message = "'" + e2.Name + "' is not numeric";
				return false;
			}

			NumericVariable order = e2.Value as NumericVariable;
			int n = order.Length;
			bool[] check = new bool[n];
			for(int i=0;i<n;i++)
			{
				if(order[i]<0 || order[i]>=n)
				{
					valueType = ValueType.Invalid;
					message = "'" + e2.Name + "' is not a valid order: index out of range";
					return false;
				}
				if(check[(int)order[i]])
				{
					valueType = ValueType.Invalid;
					message = "'" + e2.Name + "' is not a valid order: index '" + (int)order[i] + "' appears more than once";
					return false;
				}
				else
					check[(int)order[i]] = true;
			}

			expressionValue = Variable.CreateVariable("sort(" + e1.Name + "," + e2.Name + ")",e1.valueType);
			for(int i=0;i<n;i++)
			{
				expressionValue.Add(e1.Value.ItemAt((int)order[i]));
			}

			valueType = e1.ValType;
			return true;
		}

		private bool EvaluateOrderExpression(bool typeOnly, ref string message, bool ascending)
		{
			Expression e1 = SubExpression(0);
			if(!e1.Evaluate(typeOnly,ref message))
			{
				valueType = ValueType.Invalid;
				return false;
			}

			NumericVariable numV = e1.Value as NumericVariable;
			StringVariable strV = e1.Value as StringVariable;

			if(numV == null && strV == null)
			{
				message = "Cannot evaluate '" + operation.ToString() + "': invalid type of argument '" + e1.Name + "'";
				valueType = ValueType.Invalid;
				return false;
			}

			int n = (numV != null)? numV.Length:strV.Length;
			double[] order = new double[n];
			for(int i=0;i<n;i++)
				order[i] = i;

			for(int i=0;i<n-1;i++)
				for(int j=i+1; j<n; j++)
				{
					int ii = (int)order[i];
					int jj = (int)order[j];
					bool toSwitch;
					if(numV != null)
						toSwitch = (ascending && numV[jj]<numV[ii] || !ascending && numV[jj]>numV[ii]);
					else
					{
						int c = strV[ii].CompareTo(strV[jj]);
						toSwitch = (ascending && c<0 || !ascending && c>0);
					}
					if(toSwitch)
					{
						order[i] = jj;
						order[j] = ii;
					}
				}

			expressionValue = new NumericVariable(operation.ToString() + "(" + e1.Name + ")",order);
			return true;
		}

		private bool EvaluateAggregateExpression(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			if(!e1.Evaluate(typeOnly,ref message))
			{
				valueType = ValueType.Invalid;
				return false;
			}

			if(e1.ValType != ValueType.Numeric)
			{
				valueType = ValueType.Invalid;
				message = "Can not apply operation '" + operation.ToString() + "': value type of argument '" + e1.ToString() +"' is not numeric";
				return false;
			}

			NumericVariable arg = e1.Value as NumericVariable;

			double count = arg.Length;
			double first = arg[0];
			double last = first;
			double min = first;
			double max = first;
			double sum = 0;

			for(int i=0;i<arg.Length;i++)
			{
				double val = arg[i];
				last = val;
				min = Math.Min(min,val);
				max = Math.Max(max,val);
				sum += val;
			}
			
			double avg = (count>0? sum/count: 0);
			double span = max-min;

			double[] y = new double[arg.Length];
			for(int i=0;i<arg.Length;i++)
			{

				switch(operation)
				{
					case OpKind.Count:
						y[i] = count;
						break;
					case OpKind.Sum:
						y[i] = sum;
						break;
					case OpKind.Avg:
						y[i] = avg;
						break;
					case OpKind.First:
						y[i] = first;
						break;
					case OpKind.Last:
						y[i] = last;
						break;
					case OpKind.Span:
						y[i] = span;
						break;
					case OpKind.MinAgg:
						y[i] = min;
						break;
					case OpKind.MaxAgg:
						y[i] = max;
						break;
					default:
						message = "Operation '" + operation.ToString() + "': not implemented";
						return false;
				}
			}

			expressionValue = new NumericVariable(operation.ToString() + "(" + arg.Name + ")",y);
			return true;
		}


		private bool EvaluateUnaryExpression(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			if(!e1.Evaluate(typeOnly,ref message))
			{
				valueType = ValueType.Invalid;
				return false;
			}

			switch(operation)
			{
				case OpKind.Not:
					if(e1.valueType != ValueType.Bool)
					{
						message = "Can not apply operation '!': not 'Bool' value type of argument '" + e1.ToString() +"'";
						return false;
					}
					valueType = ValueType.Bool;
					if(!typeOnly)
						expressionValue = (e1.expressionValue as BoolVariable).Not();
					return true;
				case OpKind.Neg:
					if(e1.valueType != ValueType.Numeric)
					{
						message = "Can not apply operation unary '-': not 'Double' value type of argument '" + e1.ToString() +"'";
						return false;
					}
					valueType = ValueType.Numeric;
					if(!typeOnly)
						expressionValue = (e1.expressionValue as NumericVariable).Neg();
					return true;
			}
			return false;
		}

		private bool EvaluateMissing(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			if(!e1.Evaluate(typeOnly,ref message))
			{
				valueType = ValueType.Invalid;
				return false;
			}

			valueType = ValueType.Bool;
			if(!typeOnly)
			{
				int M = e1.expressionValue.Length;
				bool[] r = new bool[M];
				bool[] m = new bool[M];
				for(int i=0;i<M;i++)
				{
					r[i] = e1.expressionValue.MissingAt(i);
					m[i] = false;
				}
				expressionValue = new BoolVariable("",r,m);
				expressionValue.SetConstant = e1.expressionValue.IsConstant;
			}
			return true;
		}

		private bool EvaluateFormat(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			Expression e2 = SubExpression(1);
			if(!e1.Evaluate(typeOnly,ref message))
			{
				valueType = ValueType.Invalid;
				return false;
			}

			if(!e2.Evaluate(typeOnly,ref message))
			{
				valueType = ValueType.Invalid;
				return false;
			}
			
			if(e2.valueType != ValueType.String)
			{
				message = "Formatting argument is not StringVariable";
				valueType = ValueType.Invalid;
				return false;
			}

			valueType = ValueType.String;
			if(!typeOnly)
				expressionValue = e1.expressionValue.EvaluateFormat(e2 as StringVariable);

			return true;
		}

		private bool EvaluateConcatenation(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			Expression e2 = SubExpression(1);

			if(!e1.Evaluate(typeOnly,ref message))
				return false;
			if(!e2.Evaluate(typeOnly,ref message))
				return false;

			valueType = ValueType.String;
			if(e1.valueType == Expression.ValueType.String)
			{
				if(typeOnly)
					return true;
				Variable v2 = e2.expressionValue;
				if(!(v2 is StringVariable))
					v2 = v2.Format();
				expressionValue = (e1.expressionValue as StringVariable).Concatenate(v2 as StringVariable);
			}

			else if(e2.valueType == Expression.ValueType.String)
			{
				if(typeOnly)
					return true;
				Variable v1 = e1.expressionValue;
				if(!(v1 is StringVariable))
					v1 = v1.Format();
				expressionValue = (v1 as StringVariable).Concatenate(e2.expressionValue as StringVariable);
			}
			else
			{
				message = "Can not concatenate non-string arguments";
				valueType = ValueType.Invalid;
				return false;
			}

			return true;
		}

		private struct OpCodeString
		{
			public OpKind	op;
			public string	str;
			public OpCodeString(OpKind op, string str)
			{
				this.op = op;
				this.str = str;
			}
		}

		private static OpCodeString[] CodeStrings = new OpCodeString[]
			{
				new OpCodeString(OpKind.Neg,"-"),
				new OpCodeString(OpKind.Not,"!"),
				new OpCodeString(OpKind.Add,"+"),
				new OpCodeString(OpKind.Sub,"-"),
				new OpCodeString(OpKind.Mul,"*"),
				new OpCodeString(OpKind.Div,"/"),
				new OpCodeString(OpKind.Le, "<="),
				new OpCodeString(OpKind.Lt, "<"),
				new OpCodeString(OpKind.Eq, "="),
				new OpCodeString(OpKind.Ne, "!="),
				new OpCodeString(OpKind.Gt, ">"),
				new OpCodeString(OpKind.Ge, ">="),
				new OpCodeString(OpKind.And,"&&"),
				new OpCodeString(OpKind.Or, "||")
			};
	
		private string StringOf(OpKind op)
		{
			for(int i=0;i<CodeStrings.Length;i++)
				if(op == CodeStrings[i].op)
					return CodeStrings[i].str;
			return op.ToString();
		}
	

		private bool EvaluateBinaryDoubleExpression(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			Expression e2 = SubExpression(1);

			if(!e1.Evaluate(typeOnly,ref message))
				return false;
			if(!e2.Evaluate(typeOnly,ref message))
				return false;

			if(e1.valueType != Expression.ValueType.Numeric)
			{
				message = "Can not evaluate '" + StringOf(operation) + "' : '" + e1.Name +"' is not 'Double'";
				valueType = ValueType.Invalid;
				return false;
			}

			if(e2.valueType != Expression.ValueType.Numeric)
			{
				message = "Can not evaluate '" + StringOf(operation) + "' : '" + e2.Name +"' is not 'Double'";
				valueType = ValueType.Invalid;
				return false;
			}

			valueType = ValueType.Numeric;
			if(!typeOnly)
				expressionValue = (e1.expressionValue as NumericVariable).BinaryOperation(operation,e2.expressionValue as NumericVariable);
			return true;
		}

		private bool EvaluateRelationExpression(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			Expression e2 = SubExpression(1);

			if(!e1.Evaluate(typeOnly,ref message))
				return false;
			if(!e2.Evaluate(typeOnly,ref message))
				return false;

			if(e1.valueType != e2.valueType)
			{
				message = "Can not compare different types";
				valueType = ValueType.Invalid;
				return false;
			}

			if(e1.valueType == Expression.ValueType.Object)
			{
				message = "Can not compare ObjectVariables";
				valueType = ValueType.Invalid;
				return false;
			}

			valueType = ValueType.Bool;
			if(!typeOnly)
				expressionValue = e1.expressionValue.BinaryRelation(operation,e2.expressionValue);
			return true;
		}

		private bool EvaluateBinaryBoolExpression(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			Expression e2 = SubExpression(1);

			if(!e1.Evaluate(typeOnly,ref message))
				return false;
			if(!e2.Evaluate(typeOnly,ref message))
				return false;

			if(e1.valueType != Expression.ValueType.Bool)
			{
				message = "Can not apply binary operation : not 'Bool' value type of argument '" + e1.ToString() +"'";
				valueType = ValueType.Invalid;
				return false;
			}

			if(e2.valueType != Expression.ValueType.Bool)
			{
				message = "Can not apply binary operation : not 'Bool' value type of argument '" + e2.ToString() +"'";
				valueType = ValueType.Invalid;
				return false;
			}

			valueType = ValueType.Bool;
			if(!typeOnly)
				expressionValue = (e1.expressionValue as BoolVariable).BinaryOperation(operation,e2.expressionValue as BoolVariable);
			return true;
		}

		private bool EvaluateFunctionCallExpression(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);

			if(!e1.Evaluate(typeOnly,ref message))
				return false;

			if(e1.valueType != Expression.ValueType.Numeric)
			{
				message = "Can not apply function call : not 'Double' value type of argument '" + e1.ToString() +"'";
				valueType = ValueType.Invalid;
				return false;
			}

			valueType = ValueType.Numeric;
			if(!typeOnly)
				expressionValue = (e1.expressionValue as NumericVariable).Function(operation);
			return true;
		}

		private bool EvaluateRandomExpression(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			Expression e2 = SubExpression(1);
			Expression e3 = SubExpression(2);
			Expression e4 = SubExpression(3);

			if(!e1.Evaluate(typeOnly,ref message))
				return false;
			if(!e2.Evaluate(typeOnly,ref message))
				return false;
			if(!e3.Evaluate(typeOnly,ref message))
				return false;
			if(!e4.Evaluate(typeOnly,ref message))
				return false;

			if(e1.valueType != Expression.ValueType.Numeric)
			{
				message = "Can not compute random series - seed is not numeric'";
				valueType = ValueType.Invalid;
				return false;
			}

			if(e2.valueType != Expression.ValueType.Numeric)
			{
				message = "Can not compute random series - number of points is not numeric'";
				valueType = ValueType.Invalid;
				return false;
			}

			if(e3.valueType != Expression.ValueType.Numeric)
			{
				message = "Can not compute random series - min value is not numeric'";
				valueType = ValueType.Invalid;
				return false;
			}

			if(e4.valueType != Expression.ValueType.Numeric)
			{
				message = "Can not compute random series - max value is not numeric'";
				valueType = ValueType.Invalid;
				return false;
			}
			
			valueType = Expression.ValueType.Numeric;

			if(typeOnly)
				return true;

			int seed = (int)((e1.Value as NumericVariable)[0]);

			NumericVariable vMin = e3.Value as NumericVariable;
			NumericVariable vMax = e4.Value as NumericVariable;
			double [] v = null;

			if(vMin.Length == 1 && vMax.Length == 1)
			{
				// We compute e2 values between constants e3 and e4,
				// i.e. e2 is used as number of samples
				int nPoints = (int)((e2.Value as NumericVariable)[0]);
				if(nPoints <=0)
				{
					message = "Can not compute random series - number of points is " + nPoints;
					valueType = ValueType.Invalid;
					return false;
				}
				double min = (int)((e3.Value as NumericVariable)[0]);
				double max = (int)((e4.Value as NumericVariable)[0]);

				Random r = new Random(seed);
				v = new double[nPoints];
				for (int i = 0; i<nPoints;i++)
				{
					double f = r.NextDouble();
					v[i] = f*min + (1-f)*max;
				}
			}
			else
			{
				// We ignore e2 and derive length from e3 and e4
				// i.e. e2 is used as number of samples
				int nPoints = Math.Max(vMin.Length,vMax.Length);
				if(nPoints <=0)
				{
					message = "Can not compute random series - number of points is " + nPoints;
					valueType = ValueType.Invalid;
					return false;
				}

				Random r = new Random(seed);
				v = new double[nPoints];
				for (int i = 0; i<nPoints;i++)
				{
					double min = vMin[i];
					double max = vMax[i];
					double f = r.NextDouble();
					v[i] = f*min + (1-f)*max;
				}
			}

			expressionValue = new NumericVariable("Random("+e1.Name+","+e2.Name+","+e3.Name+","+e4.Name+")",v);
			
			return true;
		}

		private bool EvaluateSelectExpression(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			Expression e2 = SubExpression(1);
			Expression e3 = SubExpression(2);

			if(!e1.Evaluate(typeOnly,ref message))
				return false;
			if(!e2.Evaluate(typeOnly,ref message))
				return false;
			if(!e3.Evaluate(typeOnly,ref message))
				return false;

			if(e1.valueType != Expression.ValueType.Bool)
			{
				message = "Can not apply selection : not 'Bool' value type of the selection criteria '" + e1.ToString() +"'";
				valueType = ValueType.Invalid;
				return false;
			}

			if(e2.valueType != e3.valueType)
			{
				message = "Can not apply selection : value type mismatch, arguments '" + e1.ToString() +"' and '" + e2.ToString() +"'";
				valueType = ValueType.Invalid;
				return false;
			}

			valueType = e2.valueType;
			if(!typeOnly)
			{
				if(e2.valueType == Expression.ValueType.Numeric)
				{
					expressionValue = (e2.expressionValue as NumericVariable).Alternate(
						e1.expressionValue as BoolVariable, e3.expressionValue as NumericVariable);
				}
				else if(e2.valueType == Expression.ValueType.DateTime)
				{
					expressionValue = (e2.expressionValue as DateTimeVariable).Alternate(
						e1.expressionValue as BoolVariable, e3.expressionValue as DateTimeVariable);
				}
				else if(e2.valueType == Expression.ValueType.Bool)
				{
					expressionValue = (e2.expressionValue as BoolVariable).Alternate(
						e1.expressionValue as BoolVariable, e3.expressionValue as BoolVariable);
				}
				else if(e2.valueType == Expression.ValueType.String)
				{
					expressionValue = (e2.expressionValue as StringVariable).Alternate(
						e1.expressionValue as BoolVariable, e3.expressionValue as StringVariable);
				}
				else if(e2.valueType == Expression.ValueType.Color)
				{
					expressionValue = (e2.expressionValue as ColorVariable).Alternate(
						e1.expressionValue as BoolVariable, e3.expressionValue as ColorVariable);
				}
				else if(e2.valueType == Expression.ValueType.Object)
				{
					expressionValue = (e2.expressionValue as ObjectVariable).Alternate(
						e1.expressionValue as BoolVariable, e3.expressionValue as ObjectVariable);
				}
			}
			return true;
		}

		private bool EvaluateFilterExpression(bool typeOnly, ref string message)
		{
			Expression e1 = SubExpression(0);
			Expression e2 = SubExpression(1);

			if(!e1.Evaluate(typeOnly,ref message))
				return false;
			if(!e2.Evaluate(typeOnly,ref message))
				return false;

			if(e1.valueType != Expression.ValueType.Bool)
			{
				message = "Can not apply filter : not 'Bool' value type of the selection criteria '" + e1.ToString() +"'";
				valueType = ValueType.Invalid;
				return false;
			}

			valueType = e2.valueType;
			if(!typeOnly)
			{
				expressionValue = (e2 as Variable).Filter(e1) as Variable;
			}
			return true;
		}
		#endregion

	}
}
