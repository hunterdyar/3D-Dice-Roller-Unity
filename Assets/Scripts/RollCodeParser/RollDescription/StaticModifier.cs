namespace HDyar.DiceRoller.RollCodeParser.RollDescription
{
	public class StaticModifier
	{
		public int modifyValue;
		public string modifyType;

		public StaticModifier(int value, string type="")
		{
			modifyValue = value;
			modifyType = type;
		}
	}
}