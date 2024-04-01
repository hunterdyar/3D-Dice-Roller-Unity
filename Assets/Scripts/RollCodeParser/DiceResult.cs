using System.Collections.Generic;
using System.Linq;

namespace HDyar.DiceRoller.RollCodeParser
{
	public class DiceResult
	{
		//Faces is part of our unity thing, but we could get rid of it.
		public List<(int result,int sides)> Rolls;
		public int FaceTotal => Rolls.Sum(x=> x.result);
		public int ModifierTotal;
		public int Total => FaceTotal+ModifierTotal;

		public DiceResult()
		{
			Rolls = new List<(int,int)>();
			ModifierTotal = 0;
		}

		public void AddResult(DiceResult other)
		{
			if (other == null)
			{
				return;
			}
			
			foreach (var r in other.Rolls)
			{
				Rolls.Add(r);
			}

			ModifierTotal += other.ModifierTotal;
			
		}
	}
}