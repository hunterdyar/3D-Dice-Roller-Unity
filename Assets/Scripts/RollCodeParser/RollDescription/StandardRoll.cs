using System.Linq;

namespace HDyar.DiceRoller.RollCodeParser.RollDescription
{
	/// <summary>
	/// A standard roll would be something like "2d6+4"
	/// </summary>
	public class StandardRoll
	{
		public GroupOfDiceDescription[] DiceRolls;
		public StaticModifier[] Modifiers;

		public void AppendGroup(GroupOfDiceDescription group)
		{
			if (DiceRolls == null)
			{
				DiceRolls = new[] { group };
			}
			else
			{
				var current = DiceRolls.ToList();
				current.Add(group);
				DiceRolls = current.ToArray();
			}
		}

		public void AppendModifier(StaticModifier staticModifier)
		{
			if (Modifiers == null) {
				Modifiers = new[] { staticModifier };
			}else {
				var current = Modifiers.ToList();
				current.Add(staticModifier);
				Modifiers = current.ToArray();
			}
		}
	}
}