using System.Collections;
using System.Collections.Generic;

namespace HDyar.DiceRoller.RollCodeParser
{
	public interface IDiceRoller
	{
		public IEnumerator RollDice(int dice, int faceCount, DiceResult result);
		//public IEnumerator DoRollDice(int dice, int faceCount, DiceResult result);
		public bool IsRolling { get;}
	}
}