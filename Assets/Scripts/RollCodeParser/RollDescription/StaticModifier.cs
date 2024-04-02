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

		public int GetResult()
		{
			return modifyValue;
		}

		public string GetResultString()
		{
			if (string.IsNullOrEmpty(modifyType))
			{
				return ModValueString();
			}
			else
			{
				return ModValueString() + $"({modifyType}) ";
			}
		}

		public string ModValueString()
		{
			if (modifyValue >= 0)
			{
				return "+" + modifyValue.ToString();
			}
			else
			{
				return modifyValue.ToString();
			}
		}
	}
}