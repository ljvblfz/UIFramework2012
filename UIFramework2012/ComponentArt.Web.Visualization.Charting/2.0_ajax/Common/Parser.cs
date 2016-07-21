using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Reflection;

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Parser creates a parse tree from string defining series expression.
	/// </summary>
	internal class Parser
	{
		private ArrayList	stack;
		private int			top;
		private bool		computing = false;
		protected InputVariableCollection inputVars = null;

		private Lexer		lexer;
		private string		message;
		private Location	location;

		// Expression list handling
		private int[]		eListCount=new int[20];
		private int         eListDepth = 0;
		private int			eListLength;

		#region --- Constructor ---
		public Parser(string input)
		{ 
			lexer = new Lexer(input);
		}

		#endregion

		#region --- Parsing and Computing ---

		internal ChartBase OwningChart 
		{
			get
			{
				if(inputVars == null)
					return null;
				if(inputVars.Owner == null)
					throw new ApplicationException("inputVars.Owner == null");
				if(!(inputVars.Owner is DataProvider))
                    throw new ApplicationException("inputVars.Owner.Type = " + inputVars.Owner.GetType().Name);

				return (inputVars.Owner as DataProvider).OwningChart;
			}
		}

		public bool Parse()
		{
			try
			{
				bool success = LogAExpression();
				if(success && !lexer.EOF())
					throw new ParserException("Unexpected text",lexer.Location);
				return success;
			}
			catch (ParserException ex)
			{
				message = ex.Message;
				location = ex.Location;
				return false;
			}
		}

		public Variable Compute(InputVariableCollection inVars)
		{
			Variable var;
			inputVars = inVars;
			stack = new ArrayList();
			top = -1;
			computing = true;
			if(Parse())
			{
				if (stack[0] is DataExpression)
				{
					DataExpression de = stack[0] as DataExpression;
					var = de.Evaluate(inputVars);
					if (var == null)
						this.message = de.ErrorMessage;
				}
				else
					var = (Value(0) as Expression).Value;
			}
			else
				var = null;
			computing = false;
			return var;
		}
		#endregion

		#region --- Properties ---
		public string ErrorMessage { get { return message; } }
		#endregion

		#region --- Syntax ---
		#region --- ExpressionList ---
		public void ExpressionList()
		{
			if(LogAExpression())
			{
				eListDepth++;
				eListCount[eListDepth] = 1;
				ExpressionListTail();
				eListLength = (int)eListCount[eListDepth];
				eListDepth--;
			}
		}

		public void ExpressionListTail()
		{
			if(lexer.ScanText(","))
			{
				Location start1 = lexer.Location;
				if(LogAExpression())
				{
					eListCount[eListDepth] = (int)eListCount[eListDepth]+1;
					BuildCommaExpression();
					ExpressionListTail();
				}
				else
					throw new ParserException("Syntax error: expression expected",start1);
			}
		}
		#endregion
	
		#region --- Term ---

		// <Term> ::= (<LogAExpression>)
		//			| Number
		//			| Literal
		//			| <VariableAndMemberExpression>

		public bool Term()
		{
			Location start = lexer.Location;
			if(lexer.ScanText("("))
			{
				Location start1 = lexer.Location;
				if(!LogAExpression())
					throw new ParserException("Syntax error: expression expected",start1);
				else
				{
					Location start2 = lexer.Location;
					if(!lexer.ScanText(")"))
						throw new ParserException("Syntax error: ')' expected",start2);
				}
			}
			else if(lexer.ScanNumber())
				BuildNumber(lexer.Token.Val);
			else if(lexer.ScanLiteral())
				BuildLiteral(lexer.Token.Val);
			else if(lexer.ScanColor())
				BuildColor(lexer.Token.Val);
			else if(VariableAndMemberExpression())
			{
				// do nothing
			}
			else
			{
				lexer.Location = start;
				return false;
			}
			return true;
		}

		// <VariableAndMemberExpression> ::= Name ( "(" <ExpressopnList> ")"
		//											| .<MemberExpressionTail>
		//											| e
		//										  )
		public bool VariableAndMemberExpression()
		{
			if(!lexer.ScanName())
				return false;
			string name = lexer.Token.Val;
			if(lexer.ScanText("(")) // function
			{
				ExpressionList();
				Location start1 = lexer.Location;
				if(lexer.ScanText(")"))
					BuildFunctionCall(name);
				else
					throw new ParserException("Syntax error: ')' expected",start1);
			}
			else 
			{
				BuildVariable(name);
				if(lexer.ScanText("."))
					return MemberExpressionTail();
			}
			return true;
		}

		public bool MemberExpressionTail()
		{
			if(lexer.ScanName())
			{
				BuildMemberExpression(lexer.Token.Val);
				if(lexer.ScanText("."))
					return MemberExpressionTail();				
				else
					return true;
			}
			return false;
		}
		#endregion

		#region --- Arithmetic Expression ---
		public bool MExpression()
		{// Multiplicative expression
			Location start = lexer.Location;
			if(lexer.ScanText("+"))
			{
				Location start1 = lexer.Location;
				if(Term())
				{
					MExpressionTail();
					return true;
				}
				else
					throw new ParserException("Syntax error: expression expected",start1);
			}
			else if(lexer.ScanText("-"))
			{
				Location start1 = lexer.Location;
				if(Term())
				{
					BuildExpression(Expression.OpKind.Neg);
					MExpressionTail();
					return true;
				}
				else
					throw new ParserException("Syntax error: expression expected",start1);
			}
			else if(Term())
			{
				MExpressionTail();
				return true;
			}
			lexer.Location = start;
			return false;
		}

		public void MExpressionTail()
		{
			if(lexer.ScanText("*"))
			{
				Location start1 = lexer.Location;
				if(Term())
				{
					BuildExpression(Expression.OpKind.Mul);
					MExpressionTail();
				}
				else
					throw new ParserException("Syntax error: expression expected",start1);
			}
			else if(lexer.ScanText("/"))
			{
				Location start1 = lexer.Location;
				if(Term())
				{
					BuildExpression(Expression.OpKind.Div);
					MExpressionTail();
				}
				else
					throw new ParserException("Syntax error: expression expected",start1);
			}
		}

		public bool AExpression()
		{// Additive expression
			Location start = lexer.Location;
			if(MExpression())
			{
				AExpressionTail();
				if(lexer.ScanText("+") && MExpression())
					BuildExpression(Expression.OpKind.Add);
				else if(lexer.ScanText("-") && MExpression())
					BuildExpression(Expression.OpKind.Sub);
				return true;
			}
			lexer.Location = start;
			return false;
		}

		public void AExpressionTail()
		{
			Location start = lexer.Location;
			if(lexer.ScanText("+"))
			{
				Location start1 = lexer.Location;
				if(MExpression())
				{
					BuildExpression(Expression.OpKind.Add);
					AExpressionTail();
					return;
				}
				else
					throw new ParserException("Syntax error: expression expected",start1);
			}
			else if(lexer.ScanText("-"))
			{
				Location start1 = lexer.Location;
				if(MExpression())
				{
					BuildExpression(Expression.OpKind.Sub);
					AExpressionTail();
					return;
				}
				else
					throw new ParserException("Syntax error: expression expected",start1);
			}
			lexer.Location = start;
		}
		#endregion

		#region --- Relational Expression ---
		public bool RelExpression()
		{// Relational expression
			Location start = lexer.Location;
			if(AExpression())
			{
				if(lexer.ScanText("<"))
				{
					Location start1 = lexer.Location;
					if(AExpression())
						BuildExpression(Expression.OpKind.Lt);
					else
						throw new ParserException("Syntax error: expression expected",start1);
				}
				else if(lexer.ScanText("<="))
				{
					Location start1 = lexer.Location;
					if(AExpression())
						BuildExpression(Expression.OpKind.Le);
					else
						throw new ParserException("Syntax error: expression expected",start1);
				}
				else if(lexer.ScanText("=="))
				{
					Location start1 = lexer.Location;
					if(AExpression())
						BuildExpression(Expression.OpKind.Eq);
					else
						throw new ParserException("Syntax error: expression expected",start1);
				}
				else if(lexer.ScanText("!="))
				{
					Location start1 = lexer.Location;
					if(AExpression())
						BuildExpression(Expression.OpKind.Ne);
					else
						throw new ParserException("Syntax error: expression expected",start1);
				}
				else if(lexer.ScanText("<>"))
				{
					Location start1 = lexer.Location;
					if(AExpression())
						BuildExpression(Expression.OpKind.Ne);
					else
						throw new ParserException("Syntax error: expression expected",start1);
				}
				else if(lexer.ScanText(">="))
				{
					Location start1 = lexer.Location;
					if(AExpression())
						BuildExpression(Expression.OpKind.Ge);
					else
						throw new ParserException("Syntax error: expression expected",start1);
				}
				else if(lexer.ScanText(">"))
				{
					Location start1 = lexer.Location;
					if(AExpression())
						BuildExpression(Expression.OpKind.Gt);
					else
						throw new ParserException("Syntax error: expression expected",start1);
				}
				return true;
			}
			lexer.Location = start;
			return false;
		}
		#endregion

		#region --- Logical Expression ---
		public bool LogUExpression()
		{// <LogUExpression> ::= !<RelExpression> | <RelExpression>
			Location start = lexer.Location;
			
			if(lexer.ScanText("!"))
			{
				Location start1 = lexer.Location;
				if(!RelExpression())
					throw new ParserException("Syntax error: expression expected",start1);
				BuildExpression(Expression.OpKind.Not);
			}
			else if(RelExpression())
				return true;

			lexer.Location = start;
			return false;
		}

		public bool LogMExpression()
		{// <LogMExpression> ::= <LogUExpression> <LogMExpressionTail>
			Location start = lexer.Location;
			
			if(LogUExpression())
			{
				LogMExpressionTail();
				return true;
			}
			lexer.Location = start;
			return false;
		}

		public void LogMExpressionTail()
		{// <LogMExpressionTail> ::= (&&|&) <LogUExpression> <LogMExpressionTail> | e
			if(lexer.ScanText("&&") || lexer.ScanText("&"))
			{
				Location start = lexer.Location;
				if(!LogUExpression())
					throw new ParserException("Syntax error: expression expected",start);
				BuildExpression(Expression.OpKind.And);
				LogMExpressionTail();
			}
		}

		public bool LogAExpression()
		{// <LogAExpression> ::= <LogMExpression> <LogAExpressionTail>
			Location start = lexer.Location;
			
			if(LogMExpression())
			{
				LogAExpressionTail();
				return true;
			}
			lexer.Location = start;
			return false;
		}

		public void LogAExpressionTail()
		{// <LogAExpressionTail> ::= ("||" | "|" ) <LogMExpression> <LogAExpressionTail> | e
			if(lexer.ScanText("||") || lexer.ScanText("|"))
			{
				Location start = lexer.Location;
				if(!LogMExpression())
					throw new ParserException("Syntax error: expression expected",start);
				BuildExpression(Expression.OpKind.Or);
				LogAExpressionTail();
			}
		}
		#endregion
		
		#endregion

		#region --- Building Expressions ---

		void Push(object obj)
		{
			top++;
			if (obj is DataExpression)
			{
				DataExpression DE = obj as DataExpression;
				obj = DE.Evaluate(inputVars);
			}
			if(top<stack.Count)
				stack[top] = obj;
			else
				stack.Add(obj);
		}

		void BuildMemberExpression(string memberName)
		{
			NamedData nData = (NamedData)(stack[top]);
			string newName = "";
			object obj = null;

			try
			{
				newName = nData.Name + "." + memberName;
				obj = BuildMemberExpression(nData.Value, memberName);
			}
			catch(ParserException pe)
			{
				string message = "Can't evaluate expression '" + newName +"'\n" + pe.Message;
				throw new ParserException(message,lexer.Token.Loc);
			}
			
			if(obj == null)
				throw new ParserException("Expression '" + newName + "' is null",lexer.Token.Loc);
			stack[top] = new NamedData(newName,obj);
		}

		object BuildMemberExpression(object obj, string memberName)
		{
			// Try to process the object as a single entity
			
			// Try public property
			Type T = obj.GetType();
			PropertyInfo pi = T.GetProperty(memberName);
			if(pi != null && pi.PropertyType.IsPublic)
			{
				obj = pi.GetValue(obj,null);
				if(obj == null)
					throw new ParserException("Property '" + memberName + "' is null",lexer.Token.Loc);
				return obj;
			}
			// Try public member
			FieldInfo mi = T.GetField(memberName);
			if(mi != null && mi.IsPublic)
			{
				obj = mi.GetValue(obj);
				if(obj == null)
					throw new ParserException("Field '" + memberName + "' is null",lexer.Token.Loc);
				return obj;
			}
				// Try table in dataset
			else if(T == typeof(DataSet))
			{
				DataSet ds = obj as DataSet;
				if(ds.Tables[memberName] != null)
					return ds.Tables[memberName];
				else
					throw new ParserException("DataSet '" + ds.DataSetName + "' doesn't have table '" + memberName + "'",lexer.Token.Loc);
			}
				// Try column in table
			else if(T == typeof(DataTable))
			{
				DataTable dt = obj as DataTable;
				if(dt.Columns[memberName] != null)
					return dt.Columns[memberName];
				else
					throw new ParserException("DataTable '" + dt.TableName + "' doesn't have column '" + memberName + "'",lexer.Token.Loc);
			}

			// Try to evaluate collection object by evaluating individual members

			IEnumerable enObj = obj as IEnumerable;
			if(enObj != null)
			{
				IEnumerator enumerator = enObj.GetEnumerator();
				ArrayList list = new ArrayList();
				while(enumerator.MoveNext())
				{
					object result = BuildMemberExpression(enumerator.Current,memberName);
					if(result.GetType() == typeof(ArrayList))
					{
						foreach (object mobj in (result as ArrayList))
							list.Add(mobj);
					}
					else
						list.Add(result);
				}
				return list;
			}

			throw new ParserException("Can't evaluate member '" +  memberName 
				+ "' for data type '" + obj.GetType().Name + "'",lexer.Token.Loc);
		}

		void BuildCommaExpression()
		{
			if(!computing)
				return;
			// Compute comma expression here
		}

		void BuildFunctionCall(string name)
		{
			if(!computing)
				return;
			string nameLC = name.ToLower();

			if(nameLC == "abs")
			{
				if(CheckArgCount("abs",1))
					BuildExpression(Expression.OpKind.Abs);
			}
			else if(nameLC == "atan")
			{
				if(CheckArgCount("atan",1))
					BuildExpression(Expression.OpKind.Atan);
			}
			else if(nameLC == "atan2")
			{
				if(CheckArgCount("atan2",2))
					BuildExpression(Expression.OpKind.Atan2);
			}
			else if(nameLC == "min")
			{
				if(CheckArgCount("min",2))
					BuildExpression(Expression.OpKind.Min);
			}
			else if(nameLC == "max")
			{
				if(CheckArgCount("max",2))
					BuildExpression(Expression.OpKind.Max);
			}
			else if(nameLC == "pow")
			{
				if(CheckArgCount("pow",2))
					BuildExpression(Expression.OpKind.Pow);
			}
			else if(nameLC == "sin")
			{
				if(CheckArgCount("sin",1))
					BuildExpression(Expression.OpKind.Sin);
			}
			else if(nameLC == "cos")
			{
				if(CheckArgCount("cos",1))
					BuildExpression(Expression.OpKind.Cos);
			}
			else if(nameLC == "tan")
			{
				if(CheckArgCount("tan",1))
					BuildExpression(Expression.OpKind.Tan);
			}
			else if(nameLC == "asin")
			{
				if(CheckArgCount("asin",1))
					BuildExpression(Expression.OpKind.Asin);
			}
			else if(nameLC == "acos")
			{
				if(CheckArgCount("acos",1))
					BuildExpression(Expression.OpKind.Acos);
			}
			else if(nameLC == "sinh")
			{
				if(CheckArgCount("sinh",1))
					BuildExpression(Expression.OpKind.Sinh);
			}
			else if(nameLC == "cosh")
			{
				if(CheckArgCount("cosh",1))
					BuildExpression(Expression.OpKind.Cosh);
			}
			else if(nameLC == "tanh")
			{
				if(CheckArgCount("tanh",1))
					BuildExpression(Expression.OpKind.Tanh);
			}
			else if(nameLC == "exp")
			{
				if(CheckArgCount("exp",1))
					BuildExpression(Expression.OpKind.Exp);
			}
			else if(nameLC == "log")
			{
				if(CheckArgCount("log",1))
					BuildExpression(Expression.OpKind.Log);
			}
			else if(nameLC == "ceiling")
			{
				if(CheckArgCount("ceiling",1))
					BuildExpression(Expression.OpKind.Ceiling);
			}
			else if(nameLC == "random")
			{
				if(CheckArgCount("random",4))
					BuildExpression(Expression.OpKind.Random);
			}
			else if(nameLC == "format")
			{
				if(CheckArgCount("format",2))
					BuildExpression(Expression.OpKind.Format);
			}
			else if(nameLC == "if")
			{
				if(CheckArgCount("if",3))
					BuildExpression(Expression.OpKind.Select);
			}

			else if(nameLC == "count")
			{
				if(CheckArgCount("count",1))
					BuildExpression(Expression.OpKind.Count);
			}
			else if(nameLC == "sum")
			{
				if(CheckArgCount("sum",1))
					BuildExpression(Expression.OpKind.Sum);
			}
			else if(nameLC == "avg")
			{
				if(CheckArgCount("avg",1))
					BuildExpression(Expression.OpKind.Avg);
			}
			else if(nameLC == "mmin")
			{
				if(CheckArgCount("seriesmin",1))
					BuildExpression(Expression.OpKind.MinAgg);
			}
			else if(nameLC == "mmax")
			{
				if(CheckArgCount("seriesmax",1))
					BuildExpression(Expression.OpKind.MaxAgg);
			}
			else if(nameLC == "first")
			{
				if(CheckArgCount("first",1))
					BuildExpression(Expression.OpKind.First);
			}
			else if(nameLC == "last")
			{
				if(CheckArgCount("last",1))
					BuildExpression(Expression.OpKind.Last);
			}
			else if(nameLC == "span")
			{
				if(CheckArgCount("span",1))
					BuildExpression(Expression.OpKind.Span);
			}

			else if(nameLC == "orderasc")
			{
				if(CheckArgCount("orderasc",1))
					BuildExpression(Expression.OpKind.OrderAsc);
			}
			else if(nameLC == "orderdesc")
			{
				if(CheckArgCount("orderdesc",1))
					BuildExpression(Expression.OpKind.OrderDesc);
			}
			else if(nameLC == "sort")
			{
				if(CheckArgCount("sort",2))
					BuildExpression(Expression.OpKind.Sort);
			}
			else if(nameLC == "accum")
			{
				if(CheckArgCount("accum",1))
					BuildExpression(Expression.OpKind.Accum);
			}

			else
				throw new ParserException("Function '" + name + "' not supported",lexer.Location);
		}

		private bool CheckArgCount(string functionName, int count)
		{
			if(eListLength != count)
				throw new Exception("Error in expression '" + lexer.Source + "'\nFunction '" + functionName + "' requires " + count + " aguments");
			return true;
		}

		void BuildVariable(string name)
		{
			if(!computing)
				return;

			InputVariable inVar = inputVars[name];
			if(inVar == null)
				throw new ParserException("Variable '" + name + "' does not exist.",lexer.Location);

			Push(new NamedData(name,inVar.Value));
		}

		void BuildNumber(string value)
		{
			if(!computing)
				return;
			Push(new NumericVariable(value,double.Parse(value, CultureInfo.InvariantCulture)));
		}

		void BuildLiteral(string str)
		{
			if(!computing)
				return;
			Push(new StringVariable(str,str));
		}

		void BuildColor(string str)
		{
			if(!computing)
				return;

			Color color = Color.White;
			// Try first the pallete[index] notation n[x]
			int parenIndex = str.IndexOf('[');
			if(parenIndex >= 0)
			{
				// split to the palette name and index
				string name = str.Substring(0,parenIndex).Trim();
				string sIndex = str.Substring(parenIndex+1,str.Length-parenIndex-2).Trim();
				bool secondary = false;
				int index;
				if(sIndex[0] == 'S' || sIndex[0] == 's')
				{
					secondary = true;
					sIndex = sIndex.Substring(1,sIndex.Length-1);
				}
				try
				{
					index = int.Parse(sIndex);
				}
				catch
				{
					throw new ParserException("Wrong format '" + sIndex + "' of index of palette color",location);
				}
				if(name == "") // Active palette
				{
					if(secondary)
						color = OwningChart.Palette.GetSecondaryColor(index);
					else
						color = OwningChart.Palette.GetPrimaryColor(index);
				}
				else
				{
					Palette pal = OwningChart.Palettes[name];
					if(pal == null)
						throw new ParserException("Palette '" + name + "' doesn't exist",location);
					if(secondary)
						color = pal.GetSecondaryColor(index);
					else
						color = pal.GetPrimaryColor(index);
				}
			}

			else

			{
				int alpha, red, green, blue;
				bool colorDefined = false;
				try		// hex notation
				{
					if(str.Length==8)
					{
						alpha = byte.Parse(str.Substring(0,2),NumberStyles.AllowHexSpecifier);
						red   = byte.Parse(str.Substring(2,2),NumberStyles.AllowHexSpecifier);
						green = byte.Parse(str.Substring(4,2),NumberStyles.AllowHexSpecifier);
						blue  = byte.Parse(str.Substring(6,2),NumberStyles.AllowHexSpecifier);
						color = Color.FromArgb(alpha,red,green,blue);
						colorDefined = true;
					}
					else if(str.Length == 6)
					{
						alpha = 255;
						red   = byte.Parse(str.Substring(0,2),NumberStyles.AllowHexSpecifier);
						green = byte.Parse(str.Substring(2,2),NumberStyles.AllowHexSpecifier);
						blue  = byte.Parse(str.Substring(4,2),NumberStyles.AllowHexSpecifier);
						color = Color.FromArgb(alpha,red,green,blue);
						colorDefined = true;
					}
				}
				catch
				{ }

				if(!colorDefined)	// try color name
				{
					try
					{
						color = Color.FromName(str);
					}
					catch
					{
						throw new ParserException("String '" + str + "' is not valid color value",location);
					}
				}
			}
			Push(new ColorVariable(str,color));
		}

		void BuildExpression(Expression.OpKind op)
		{
			if(!computing)
				return;

			switch(op)
			{
				case Expression.OpKind.Neg:
					stack[top] = -Value(top);
					break;
				case Expression.OpKind.Not:
					stack[top] = !Value(top);
					break;

				case Expression.OpKind.Missing:
					stack[top] = Value(top).Missing();
					break;

				case Expression.OpKind.Add:
					top--;
					stack[top] = Value(top) + Value(top+1);
					break;
				case Expression.OpKind.Sub:
					top--;
					stack[top] =Value(top) - Value(top+1);
					break;
				case Expression.OpKind.Mul:
					top--;
					stack[top] = Value(top) * Value(top+1);
					break;
				case Expression.OpKind.Div:
					top--;
					stack[top] = Value(top) / Value(top+1);
					break;
				case Expression.OpKind.Atan2:
					top--;
					stack[top] = Value(top).Atan2(Value(top+1));
					break;
				case Expression.OpKind.Pow:
					top--;
					stack[top] = Value(top).Pow(Value(top+1));
					break;
				case Expression.OpKind.Min:
					top--;
					stack[top] = Value(top).Min(Value(top+1));
					break;
				case Expression.OpKind.Max:
					top--;
					stack[top] = Value(top).Max(Value(top+1));
					break;

				case Expression.OpKind.Format:
					top--;
					stack[top] = Value(top).Format(Value(top+1));
					break;

				case Expression.OpKind.Le:
					top--;
					stack[top] = Value(top) <= Value(top+1);
					break;
				case Expression.OpKind.Lt:
					top--;
					stack[top] = Value(top) < Value(top+1);
					break;
				case Expression.OpKind.Eq:
					top--;
					stack[top] = Value(top).Eq(Value(top+1));
					break;
				case Expression.OpKind.Ne:
					top--;
					stack[top] = Value(top).Ne(Value(top+1));
					break;
				case Expression.OpKind.Gt:
					top--;
					stack[top] = Value(top) > Value(top+1);
					break;
				case Expression.OpKind.Ge:
					top--;
					stack[top] = Value(top) >= Value(top+1);
					break;
			
				case Expression.OpKind.And:
					top--;
					stack[top] = Value(top).And(Value(top+1));
					break;
				case Expression.OpKind.Or:
					top--;
					stack[top] = Value(top).Or(Value(top+1));
					break;

				case Expression.OpKind.Abs:
					stack[top] = Value(top).Abs();
					break;
				case Expression.OpKind.Alias:
					// Not supported
					break;
				case Expression.OpKind.Sign:
					stack[top] = Value(top).Sign();
					break;
				case Expression.OpKind.Sin:
					stack[top] = Value(top).Sin();
					break;
				case Expression.OpKind.Cos:
					stack[top] = Value(top).Cos();
					break;
				case Expression.OpKind.Tan:
					stack[top] = Value(top).Tan();
					break;
				case Expression.OpKind.Sqrt:
					stack[top] = Value(top).Sqrt();
					break;
				case Expression.OpKind.Asin:
					stack[top] = Value(top).Asin();
					break;
				case Expression.OpKind.Acos:
					stack[top] = Value(top).Acos();
					break;
				case Expression.OpKind.Atan:
					stack[top] = Value(top).Atan();
					break;
				case Expression.OpKind.Sinh:
					stack[top] = Value(top).Sinh();
					break;
				case Expression.OpKind.Cosh:
					stack[top] = Value(top).Cosh();
					break;
					//				case Expression.OpKind.Tanh:
					//					stack[top] =(stack[top] as Expression).Tanh();
					//					break;
				case Expression.OpKind.Exp:
					stack[top] = Value(top).Exp();
					break;
				case Expression.OpKind.Log:
					stack[top] = Value(top).Log();
					break;
				case Expression.OpKind.Log10:
					stack[top] = Value(top).Log10();
					break;
				case Expression.OpKind.Floor:
					stack[top] = Value(top).Floor();
					break;
				case Expression.OpKind.Ceiling:
					stack[top] = Value(top).Ceiling();
					break;

				case Expression.OpKind.Select:
					top -= 2;
					stack[top] =  Value(top).Select(Value(top+1), Value(top+2));
					break;

				case Expression.OpKind.Random:
					top -= 3;
					stack[top] =  Value(top).Random(Value(top+1), Value(top+2), Value(top+3));
					break;
				default:


				case Expression.OpKind.Count:
					stack[top] =  Value(top).Count();
					break;
				case Expression.OpKind.Sum:
					stack[top] =  Value(top).Sum();
					break;
				case Expression.OpKind.Avg:
					stack[top] =  Value(top).Avg();
					break;
				case Expression.OpKind.MinAgg:
					stack[top] =  Value(top).MinAgg();
					break;
				case Expression.OpKind.MaxAgg:
					stack[top] =  Value(top).MaxAgg();
					break;
				case Expression.OpKind.First:
					stack[top] =  Value(top).First();
					break;
				case Expression.OpKind.Last:
					stack[top] =  Value(top).Last();
					break;
				case Expression.OpKind.Span:
					stack[top] =  Value(top).Span();
					break;

				case Expression.OpKind.OrderAsc:
					stack[top] =  Value(top).OrderAsc();
					break;
				case Expression.OpKind.OrderDesc:
					stack[top] =  Value(top).OrderDesc();
					break;
				case Expression.OpKind.Sort:
					top --;
					stack[top] =  Value(top).Sort(Value(top+1));
					break;
				case Expression.OpKind.Accum:
					stack[top] =  Value(top).Accum();
					break;
			}
		}

		private Expression Value(int x)
		{
			if(stack[x] is Expression)
				return stack[x] as Expression;

			string name = "";
			object var = stack[x];
			if(stack[x] is NamedData)
			{
				name = ((NamedData)stack[x]).Name;
				var  = ((NamedData)stack[x]).Value;
			}
			
			// Create Variable directly from the object, if possible

			Variable val = Variable.CreateVariable(name,var);
			if(val != null)
			{
				if(val.Length == 0 && !OwningChart.InDesignMode)
				{
					if(var is DataColumn)
						throw new Exception("Data table containing variable " + (name==""?"":"'" + name + "' ") + "has 0 rows");
					else
						throw new Exception("Variable " + (name==""?"":"'" + name + "' ") + "has no data");
				}
				return val;
			}

			// Flatten and try again

			object f = Flatten(var);
			if(f != null)
			{
				val = Variable.CreateVariable(name,f);
				if(val != null)
					return val;
			}

			// Try input variables
			InputVariable inVar = inputVars[name];
			if(inVar != null)
				val = inVar.Value as Variable;
			
			if(val != null)
				return val;

			if(name != "")
				throw new ParserException("Cannot evaluate variable ' " + name + "'",lexer.Location);
			else
				throw new ParserException("Cannot evaluate expression type ' " + var.GetType().Name + "'",lexer.Location);
		}

		internal static object Flatten(object var)
		{
			// Trying to transform 'var' to a data type suitable for construction of a Variable

			// 1. Transform scalar types
			if(	
				var is double ||
				var is decimal ||
				var is float ||
				var is Int16 ||
				var is Int32 ||
				var is Int64 ||
				var is SByte ||
				var is Single ||
				var is UInt16 ||
				var is UInt32 ||
				var is UInt64 
				)
				return (double)var;
			else if (
				var is string ||
				var is bool ||
				var is DateTime ||
				var is Color 
				)
				return var;

				// 2. Transform first kind arrays

			else if (
				var is double[]		||
				var is string[]		||
				var is bool[]		||
				var is DateTime[]	||
				var is Color[] 
				)
				return var;
			else if (var is decimal[])
			{
				decimal[] var1 = (decimal[])var;
				double[] v = new double[var1.Length];
				for(int i=0;i<var1.Length;i++)
					v[i] = (double)var1[i];
				return v;
			}
			else if (var is float[])
			{
				float[] var1 = (float[])var;
				double[] v = new double[var1.Length];
				for(int i=0;i<var1.Length;i++)
					v[i] = var1[i];
				return v;
			}
			else if (var is Int16[])
			{
				Int16[] var1 = (Int16[])var;
				double[] v = new double[var1.Length];
				for(int i=0;i<var1.Length;i++)
					v[i] = var1[i];
				return v;
			}
			else if (var is Int32[])
			{
				Int32[] var1 = (Int32[])var;
				double[] v = new double[var1.Length];
				for(int i=0;i<var1.Length;i++)
					v[i] = var1[i];
				return v;
			}
			else if (var is SByte[])
			{
				SByte[] var1 = (SByte[])var;
				double[] v = new double[var1.Length];
				for(int i=0;i<var1.Length;i++)
					v[i] = var1[i];
				return v;
			}
			else if (var is Single[])
			{
				Single[] var1 = (Single[])var;
				double[] v = new double[var1.Length];
				for(int i=0;i<var1.Length;i++)
					v[i] = var1[i];
				return v;
			}
			else if (var is UInt16[])
			{
				UInt16[] var1 = (UInt16[])var;
				double[] v = new double[var1.Length];
				for(int i=0;i<var1.Length;i++)
					v[i] = var1[i];
				return v;
			}
			else if (var is UInt32[])
			{
				UInt32[] var1 = (UInt32[])var;
				double[] v = new double[var1.Length];
				for(int i=0;i<var1.Length;i++)
					v[i] = var1[i];
				return v;
			}
			else if (var is UInt64[])
			{
				UInt64[] var1 = (UInt64[])var;
				double[] v = new double[var1.Length];
				for(int i=0;i<var1.Length;i++)
					v[i] = var1[i];
				return v;
			}

			// 3. Flattening IEnumerables

			if((var as IEnumerable) == null)
				return null;

			// Check if elements are the same kind
			bool sameKind = true;
			IEnumerator enumerator = (var as IEnumerable).GetEnumerator();
			enumerator.MoveNext();
			if(enumerator.Current == null)
				return null;

			int nElem = 1;
			Type elType = enumerator.Current.GetType();
			while(enumerator.MoveNext())
			{
				nElem++;
				if(elType != enumerator.Current.GetType())
				{
					sameKind = false;
					break;
				}
			}
			if(!sameKind)
				return null;

			enumerator.Reset();
			if(
				elType == typeof(double) ||
				elType == typeof(float) ||
				elType == typeof(decimal) ||
				elType == typeof(Int16) ||
				elType == typeof(Int32) ||
				elType == typeof(Int64) ||
				elType == typeof(SByte) ||
				elType == typeof(Single) ||
				elType == typeof(UInt16) ||
				elType == typeof(UInt32) ||
				elType == typeof(UInt64) 
				)
			{
				double[] val = new double[nElem];
				int i=0;
				while(enumerator.MoveNext())
				{
					Type t = enumerator.Current.GetType();
					val[i] = (double)TypeDescriptor.GetConverter(t).ConvertTo(enumerator.Current,typeof(double));
					i++;
				}
				return val;
			}
			else if (elType == typeof(string))
			{
				string[] val = new string[nElem];
				int i=0;
				while(enumerator.MoveNext())
				{
					val[i] = (string) (enumerator.Current);
					i++;
				}
				return val;
			}
			else if (elType == typeof(DateTime))
			{
				DateTime[] val = new DateTime[nElem];
				int i=0;
				while(enumerator.MoveNext())
				{
					val[i] = (DateTime) (enumerator.Current);
					i++;
				}
				return val;
			}
			else if (elType == typeof(bool))
			{
				bool[] val = new bool[nElem];
				int i=0;
				while(enumerator.MoveNext())
				{
					val[i] = (bool) (enumerator.Current);
					i++;
				}
				return val;
			}
			else if (elType == typeof(Color))
			{
				Color[] val = new Color[nElem];
				int i=0;
				while(enumerator.MoveNext())
				{
					val[i] = (Color) (enumerator.Current);
					i++;
				}
				return val;
			}
				
			else
			{
				ArrayList L = new ArrayList();
				while(enumerator.MoveNext())
				{
					object vObj = Flatten(enumerator.Current);
					if(vObj == null)
						return null;
					IEnumerable eVal = vObj as IEnumerable;
					if(eVal == null)
						return null;
					IEnumerator vEnumerator = eVal.GetEnumerator();
					while(vEnumerator.MoveNext())
						L.Add(vEnumerator.Current);
				}
				return Flatten(L);
			}

		}
		#endregion

		// -------------------------------------------------------------------------------------------

		private struct NamedData
		{
			public string	Name;
			public object	Value;
			public NamedData(string name, object value)
			{
				Name = name;
				Value = value;
			}
		}

		// -------------------------------------------------------------------------------------------

		private enum TokenKind
		{
			Name,
			Number,
			Literal,
			Text,
			Color,
			End
		}

		// -------------------------------------------------------------------------------------------

		private struct Location
		{
			public int		position, row, column;
			public Location(int position, int row, int column)
			{
				this.position = position;
				this.row = row;
				this.column = column;
			}
			public Location(Location loc)
			{
				this.position = loc.position;
				this.row = loc.row;
				this.column = loc.column;
			}
		}

		// -------------------------------------------------------------------------------------------

		private class Token
		{
			public Location		Loc;
			public string		Val;
			public TokenKind	Kind;

			public Token(TokenKind kind, string val, Location loc)
			{
				Kind = kind;
				Val = val;
				Loc = loc;
			}
		}

		// -------------------------------------------------------------------------------------------

		private class ParserException : System.Exception
		{
			Location loc;
			public ParserException(string message,Location loc) : base(message)
			{
				this.loc = loc;
			}

			public Location Location { get { return loc; } set { loc = value; } }
		}

		// -------------------------------------------------------------------------------------------
		/// <summary>
		/// Lexer produces tokens.
		/// </summary>
		private class Lexer
		{
			Location	loc = new Location(0,0,0);
			string		input;
			Token		token;

			#region --- Constructor ---

			public Lexer(string input)
			{
				this.input = input+'\0';
			}
			#endregion

			public Location Location { get { SkipSpace(); return loc; } set { loc = value; } }
			public Token	Token	 { get { return token; } }
			public string	Source	 { get { return input; } }

			#region --- Scaning ---

			public bool ScanName()
			{				
				// Scans name with optional ":" part
				string name = ScanSimpleName();
				if(name == null)
					return false;

				// Looking for :secondPart 
				SkipSpace();

				if(input[loc.position] == ':')
				{
					loc.position++;
					token = new Token(TokenKind.Name,name + ':' + ScanSimpleName(),loc);
				}
				else
					token = new Token(TokenKind.Name,name,loc);
				return true;
			}

			private string ScanSimpleName()
			{				
				// Scans name, [whatever] syntax is allowed
				SkipSpace();
				int p = loc.position;
				int start = p;

				if(input[p] == '[')
				{
					int count = 1;
					p++;
					start++;
					while(count > 0)
					{
						if(input[p]== ']')
							count--;
						else if(input[p]=='[')
							count++;
						p++;
					}
					loc.position = p;
					p--;
				}
				else
				{
					if(!char.IsLetter(input[p]) && input[p]!='_')
						return null;
					while(char.IsLetterOrDigit(input[p]) || input[p]=='_')
						p++;
					loc.position = p;
				}

				return input.Substring(start,p-start);
			}

			public bool ScanNumber()
			{
				SkipSpace();
				int p = loc.position;
				if(!char.IsNumber(input[p]))
					return false;
				int start = p;

				// format <digits>[.<digits>][(D|E){+|-}<digits>]
				// scan first sequence of digits
				while(char.IsDigit(input[p]))
					p++;
				// scan [.<digits>]
				if(input[p] == '.')
				{
					p++;
					while(char.IsDigit(input[p]))
						p++;
				}
				// scan [(D|E){+|-}<digits>]
				if(input[p] == 'D' || input[p] == 'd' || input[p] == 'E' || input[p] == 'e')
				{
					p++;
					if(input[p] == '+' || input[p] == '-')
					{
						p++;
						while(char.IsDigit(input[p]))
							p++;
					}
					else // 'D' or 'E' was not part of tne number, so we'll back and 'unscan' it
						p--;
				}
				token = new Token(TokenKind.Number,input.Substring(start,p-start),loc);
				loc.position = p;
				return true;
			}

			public bool ScanLiteral()
			{
				SkipSpace();
				char delim = input[loc.position];
				if(delim != '\'' && delim != '\"')
					return false;

				string val = "";
				int p = loc.position+1;
				while(input[p] != delim && !EOF(input,p))
				{
					if(input[p] == '\\')
						p++;
					val += input[p];
					p++;
				}
				if(EOF(input,p))
					throw new ParserException("String literal not closed",loc);
				token = new Token(TokenKind.Literal,val,loc);
				loc.position = p+1; // +1 is to skip the closing delimiter
				return true;
			}

			public bool ScanText(string text)
			{
				SkipSpace();
				if(input.Length - loc.position < text.Length)
					return false;
				if(text.CompareTo(input.Substring(loc.position,text.Length)) == 0)
				{
					token = new Token(TokenKind.Text,text,loc);
					loc.position += text.Length;
					return true;
				}
				else
					return false;
			}

			public bool ScanColor()
			{
				SkipSpace();
				int p = loc.position;
				if(input[p]!= '#')
					return false;
				p++;
				int start = p;
				while(char.IsLetterOrDigit(input[p]))
					p++;
				// Try form PalleteName[index]
				while(char.IsWhiteSpace(input[p]))
					p++;
				if(input[p]=='[')
				{
					p++;
					while(char.IsWhiteSpace(input[p]))
						p++;
					while(char.IsLetterOrDigit(input[p]))
						p++;
					while(char.IsWhiteSpace(input[p]))
						p++;
					if(input[p] !=']')
						throw new ParserException("The pallete color doesn't have ']'",loc);
					else
						p++;
				}
				token = new Token(TokenKind.Color,input.Substring(start,p-start),loc);
				loc.position = p;
				return true;
			}

			private bool EOF(string input, int position)
			{
				return position >= input.Length || input[position] == '\0';
			}
			public bool EOF()
			{
				return input[loc.position] == '\0';
			}

			public void SkipSpace()
			{
				while(true)
				{
					char c = input[loc.position];
					if(char.IsWhiteSpace(c))
					{
						loc.position++;
						loc.column++;
						if(c == '\n')
						{
							loc.row++;
							loc.column = 0;
						}
					}
					else
						break;
				}
			}

			#endregion
		}

	}

}
