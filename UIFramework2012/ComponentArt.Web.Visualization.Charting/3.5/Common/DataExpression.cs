using System;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class DataExpression
	{
		private string expression = "";
		private string errorMessage = "";

		public DataExpression()
		{ }
		public DataExpression(string expression)
		{ 
			this.expression = expression;
		}

		public string Expression { get { return expression; } set { expression = value; } }
		public string ErrorMessage { get { return errorMessage; } }

		public Variable Evaluate(InputVariableCollection inVars)
		{
			if(expression=="")
				return null;
			Parser parser = new Parser(expression);
			Variable var = parser.Compute(inVars);
			if(var == null)
				errorMessage = parser.ErrorMessage;
			return var;
		}
	}
}
