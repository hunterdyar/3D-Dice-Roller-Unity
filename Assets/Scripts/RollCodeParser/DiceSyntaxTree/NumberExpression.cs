namespace HDyar.DiceRoller.RollCodeParser
{
	public class NumberExpression : Expression
	{
		public int Value;
		public override string ToString()
		{
			return Value.ToString();
		}
	}
}