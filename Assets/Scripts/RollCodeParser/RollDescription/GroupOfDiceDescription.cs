using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HDyar.DiceRoller.RollCodeParser.RollDescription
{
	//Represents one or more rolls of dice intended to be summed together to a single type of multiplier.
	[System.Serializable]
	public class GroupOfDiceDescription
	{
		//serializable
		public int SumMultiplier;//set to -1 for subtract.
		public SingleRollDescription[] DiceRollDescriptions;
		public int keepHighest=0;
		public int dropLowest=0;
		private ExplodeBehaviour exploding = ExplodeBehaviour.DontExplode;
		public string Label;

		public int TotalRolls() => DiceRollDescriptions.Sum(x => x.numberTimesToRoll);
		public int GetSumPreMult() => DiceRollDescriptions.Sum(x => x.TotalSum());
		public GroupOfDiceDescription(SingleRollDescription[] rolls, int SumMultiplier = 1)
		{
			this.SumMultiplier = 1;
			this.DiceRollDescriptions = rolls;
		}

		public GroupOfDiceDescription(int numDice, int sides)
		{
			DiceRollDescriptions = new SingleRollDescription[]{new SingleRollDescription(numDice,sides)};
			SumMultiplier = 1;
			exploding = ExplodeBehaviour.DontExplode;
			keepHighest = 0;
			dropLowest = 0;
		}

		public GroupOfDiceDescription(int numDice, int sides, int drop, int keep, ExplodeBehaviour exploding)
		{
			DiceRollDescriptions = new SingleRollDescription[] { new SingleRollDescription(numDice, sides, exploding) };
			SumMultiplier = 1;
			this.exploding = exploding;
			keepHighest = drop;
			dropLowest = keep;
		}
		//todo: append a group.

		void CalculateDropAndKeep()
		{
			//Keeping the highest is another way to write dropping the lowest, I think!
			if (keepHighest > 0)
			{
				if (dropLowest == 0)
				{
					dropLowest = TotalRolls() - keepHighest;
				}
				else
				{
					Debug.LogError("having both KeepHighest and DropLowest is not supported.");
				}
			}
			//Take the lowest x rolls.
			if (dropLowest > 0)
			{
				var allRolls = DiceRollDescriptions.SelectMany(x => x.Rolls).OrderBy(x => x.Result).ToList();

				for (var i = 0; i < allRolls.Count; i++)
				{
					var r = allRolls[i];
					r.Dropped = i<dropLowest;
				}
			}
		}

		public int GetResult()
		{
			// this should get cached whenever the results change...
			CalculateDropAndKeep();
			int sum = GetSumPreMult();
			return sum * SumMultiplier;
		}

		public string GetResultString()
		{
			CalculateDropAndKeep();
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

		
		
		//todo: validate. Drop and Keeps <= total rolls, etc.
	}
}