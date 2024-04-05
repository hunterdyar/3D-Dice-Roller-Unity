using System.Collections.Generic;
using System.Linq;

namespace HDyar.DiceRoller.RollCodeParser.RollDescription
{
	[System.Serializable]
	public class SingleRollDescription
	{
		//Serializable Roll Description
		public int numberTimesToRoll;
		public int numberSides;
		public ExplodeBehaviour exploding;
		
		//Utility Result Storage. Not serializable, so get-only.
		public List<RollResult> Rolls => _rolls;
		public int TotalSum() => _rolls.Where(x=>!x.Dropped).Sum(x => x.Result);
		private List<RollResult> _rolls;

		public SingleRollDescription(int times, int sides, ExplodeBehaviour exploding = ExplodeBehaviour.DontExplode)
		{
			numberTimesToRoll = times;
			numberSides = sides;
			this.exploding = exploding;
		}
		
		public string GetResultString()
		{
			int total = TotalSum();
			string s = "";

			if (_rolls.Count == 0)
			{
				return "0";
			}

			// if (_rolls.Count == 1)
			// {
			// 	s += _rolls[0].Item1;
			// 	return s;
			// }

			// s += "(";
			for (var index = 0; index < _rolls.Count; index++)
			{
				var d = _rolls[index].Dropped;
				if (d)
				{
					s += "<s>";
				}
				int a = _rolls[index].Result;
				if (a > 0)
				{
					//s += "+";
				}
				s += a;
				if (index < _rolls.Count - 1)
				{
					s += " ";
				}

				if (d)
				{
					s += "</s>";
				}
			}

			// s += ")";

			return s;
		}

		public void ResetResult()
		{
			_rolls = new List<RollResult>();
		}

		public void GetRollResultByDice(int result, int faces, bool exploded = false)
		{
			_rolls.Add(new RollResult(result,faces, false, exploded));
		}
	}
}