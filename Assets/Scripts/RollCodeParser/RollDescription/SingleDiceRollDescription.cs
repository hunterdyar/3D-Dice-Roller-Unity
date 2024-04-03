using System.Collections.Generic;
using System.Linq;

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
		public int TotalSum() => _rolls.Sum(x => x.Item1);
		private List<(int, int)> _rolls;

		public SingleDiceRollDescription(int times, int sides)
		{
			numberTimesToRoll = times;
			numberSides = sides;
			exploding = false;
		}
		
		public string GetResultString()
		{
			int total = TotalSum();
			string s = "";

			if (_rolls.Count == 0)
			{
				return "+0";
			}

			// if (_rolls.Count == 1)
			// {
			// 	s += _rolls[0].Item1;
			// 	return s;
			// }

			// s += "(";
			for (var index = 0; index < _rolls.Count; index++)
			{
				int a = _rolls[index].Item1;
				if (a > 0)
				{
					s += "+";
				}
				s += a;
				if (index < _rolls.Count - 1)
				{
					s += " ";
				}
			}

			// s += ")";

			return s;
		}

		public void ResetResult()
		{
			_rolls = new List<(int, int)>();
		}

		public void GetRollResultByDice(int result, int faces)
		{
			_rolls.Add((result,faces));
		}
	}
}