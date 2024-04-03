namespace HDyar.DiceRoller.RollCodeParser.RollDescription
{
	[System.Serializable]
	public class RollResult
	{
		public int Result;
		public int Sides;
		public bool Dropped;
		public bool Exploded;

		public RollResult(int result, int sides, bool dropped = false, bool exploded = false)
		{
			this.Result = result;
			this.Sides = sides;
			this.Dropped = dropped;
			this.Exploded = exploded;
		}
	}
}