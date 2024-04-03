using System;
using System.Collections.Generic;
using System.Linq;

namespace HDyar.DiceRoller.RollCodeParser.RollDescription
{
	//Represents one or more rolls of dice intended to be summed together to a single type of multiplier.
	[System.Serializable]
	public class GroupOfDiceDescription
	{
		//serializable
		public int SumMultiplier;//set to -1 for subtract.
		public SingleDiceRollDescription[] DiceRollDescriptions;

		public int GetSumPreMult() => DiceRollDescriptions.Sum(x => x.TotalSum());
		public GroupOfDiceDescription(SingleDiceRollDescription[] rolls, int SumMultiplier = 1)
		{
			this.SumMultiplier = 1;
			this.DiceRollDescriptions = rolls;
		}

		public GroupOfDiceDescription(int numDice, int sides)
		{
			DiceRollDescriptions = new SingleDiceRollDescription[]{new SingleDiceRollDescription(numDice,sides)};
			SumMultiplier = 1;
		}
		//todo: append a group.

		public int GetResult()
		{
			int sum = GetSumPreMult();
			return sum * SumMultiplier;
		}
		
		public string GetResultString()
		{
			string s = "";
			if (DiceRollDescriptions.Length == 0)
			{
				return "+0";
			}else if (DiceRollDescriptions.Length == 1)
			{
				s = DiceRollDescriptions[0].GetResultString();
			}
			else
			{
				s += "(";
				for (var i = 0; i < DiceRollDescriptions.Length; i++)
				{
					var drd = DiceRollDescriptions[i];
					s += drd.GetResultString();
					
					if (i <= DiceRollDescriptions.Length)
					{
						s += ",";
					}
				}

				s += ")";
			}
			
			//mult
			if (SumMultiplier == 1)
			{
				return s;
			}else if (SumMultiplier == -1)
			{
				return "-" + s;
			}

			return SumMultiplier.ToString() + "x" + s;

		}
	}
}