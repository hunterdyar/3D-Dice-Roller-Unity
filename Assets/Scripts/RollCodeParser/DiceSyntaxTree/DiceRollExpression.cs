namespace HDyar.DiceRoller.RollCodeParser
{
	public class DiceRollExpression : Expression
	{
		public Expression NumberDice;
		public Expression NumberFaces;

		public override string ToString()
		{
			return NumberDice.ToString() + "d" + NumberFaces.ToString();
		}
	}
}