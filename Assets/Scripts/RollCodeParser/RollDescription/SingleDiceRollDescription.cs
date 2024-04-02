namespace HDyar.DiceRoller.RollCodeParser.RollDescription
{
	[System.Serializable]
	public class SingleDiceRollDescription
	{
		public int numberTimesToRoll;
		public int numberSides;
		public bool exploding;

		public SingleDiceRollDescription(int times, int sides)
		{
			numberTimesToRoll = times;
			numberSides = sides;
			exploding = false;
		}
	}
}