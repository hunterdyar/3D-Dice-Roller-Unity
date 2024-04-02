namespace HDyar.DiceRoller.RollCodeParser.RollDescription
{
	[System.Serializable]
	public class SingleDiceRollDescription
	{
		//Serializable Roll Description
		public int numberTimesToRoll;
		public int numberSides;
		public bool exploding;


		//Utility Result Storage. Not serializable, so get-only.
		public int TotalSum => _total;
		private int _total;

		public SingleDiceRollDescription(int times, int sides)
		{
			numberTimesToRoll = times;
			numberSides = sides;
			exploding = false;
		}

		public void GetRollResultTotal(int total)
		{
			this._total = total;
		}

		public string GetResultString()
		{
			if (_total >= 0)
			{
				//+1
				return $"+{_total}";
			}
			else
			{
				//-1
				return _total.ToString();
			}
		}
	}
}