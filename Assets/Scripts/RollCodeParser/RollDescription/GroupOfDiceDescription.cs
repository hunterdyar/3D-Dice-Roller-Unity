using System;
using System.Collections.Generic;

namespace HDyar.DiceRoller.RollCodeParser.RollDescription
{
	//Represents one or more rolls of dice intended to be summed together to a single type of multiplier.
	[System.Serializable]
	public class GroupOfDiceDescription
	{
		public int SumMultiplier;//set to -1 for subtract.
		public SingleDiceRollDescription[] DiceRollDescriptions;

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
	}
}