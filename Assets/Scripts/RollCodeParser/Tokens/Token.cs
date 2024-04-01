namespace HDyar.DiceRoller.RollCodeParser
{
	public class Token
	{
		public string Literal;
		public RollTokenType TType;

		public Token()
		{
			Literal = "";
			TType = RollTokenType.Invalid;
		}
		
		public Token(RollTokenType type, string literal)
		{
			TType = type;
			Literal = literal;
		}

		public Token(RollTokenType type, char literal)
		{
			TType = type;
			Literal = literal.ToString();
		}
	}
}