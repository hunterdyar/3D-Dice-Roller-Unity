namespace HDyar.DiceRoller.RollCodeParser
{
	public enum Modifier
	{
		Add,
		Subtract,
		Multiply,
		Divide
	}
	
	
	public class ModifierExpression : Expression
	{
		public string Label;
		public Modifier Modifier;
		public Expression Expression;

		public override string ToString()
		{
			return ModToString(Modifier) + Expression.ToString();
		}

		public static string ModToString(Modifier m)
		{
			switch (m)
			{
				case Modifier.Add:
					return "+";
				case Modifier.Divide:
					return "/";
				case Modifier.Multiply:
					return "x";
				case Modifier.Subtract:
					return "-";
				default:
					return "";
			}
		}
	}
}