using System.Collections.Generic;

namespace HDyar.DiceRoller.RollCodeParser
{
	public class ExpressionGroup : Expression
	{
		public string Label;
		public List<Expression> Expressions;
		public override string ToString()
		{
			var s = "(";
			foreach (var e in Expressions)
			{
				s += e.ToString();
				s += ",";
			}

			s += ")";
			return s;
		}
	}
}