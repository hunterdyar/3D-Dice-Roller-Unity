using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HDyar.DiceRoller.RollCodeParser
{
	public class DiceResult
	{
		public bool IsRolling() => ActiveRolls > 0;
		public int ActiveRolls;//coroutines can add one and remove one from this int by themselves. There are better ways to handle that, but this way keeps unity-coroutines out of code I want to abstract to c# generic, eventually.	
		//A better way might be to track "roller" objects and have the rolling state contained within them, the things that are doing the rolling.
		//Faces is part of our unity thing, but we could get rid of it.
		private List<(int result,int sides)> Rolls;
		public int FaceTotal => Rolls.Sum(x=> x.result);
		public int ModifierTotal;
		public int Total => FaceTotal+ModifierTotal;

		public DiceResult()
		{
			ActiveRolls = 0;
			Rolls = new List<(int,int)>();
			ModifierTotal = 0;
		}

		public void AddRoll(int result, int sides)
		{
			Rolls.Add((result,sides));
		}
		public void AddResult(DiceResult other)
		{
			if (other == null)
			{
				return;
			}

			if (other.ActiveRolls > 0)
			{
				Debug.LogError("Can't add an incomplete result to this result! We would have to wait.");
			}
			
			foreach (var r in other.Rolls)
			{
				Rolls.Add(r);
			}

			ModifierTotal += other.ModifierTotal;
			
		}
	}
}