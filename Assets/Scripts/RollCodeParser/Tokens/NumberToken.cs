using UnityEngine;
using UnityEngine.UIElements;

namespace HDyar.DiceRoller.RollCodeParser
{
	public class NumberToken : Token
	{
		public int Value;

		public NumberToken(string literal)
		{
			this.Literal = literal;
			this.TType = RollTokenType.Number;
			RecalculateValue();
		}

		public void RecalculateValue()
		{
			Value = int.Parse(Literal);
		}
	}
}