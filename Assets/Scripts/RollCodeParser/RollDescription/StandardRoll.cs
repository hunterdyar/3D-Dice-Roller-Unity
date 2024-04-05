using System;
using System.Linq;
using System.Text;

namespace HDyar.DiceRoller.RollCodeParser.RollDescription
{
	/// <summary>
	/// A standard roll would be something like "2d6+4"
	/// </summary>
	public class StandardRoll
	{
		public GroupOfDiceDescription[] DiceRolls = Array.Empty<GroupOfDiceDescription>();
		public StaticModifier[] Modifiers = Array.Empty<StaticModifier>();

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

		public int GetResult()
		{
			int result = 0;
			foreach (var droll in DiceRolls)
			{
				result += droll.GetResult();
			}

			if (Modifiers != null)
			{
				foreach (var mod in Modifiers)
				{
					result += mod.GetResult();
				}
			}

			return result;
		}

		public string GetResultString()
		{
			StringBuilder result = new StringBuilder();
			foreach (var droll in DiceRolls)
			{
				result = result.Append(droll.GetResultString());
			}

			if (Modifiers != null)
			{
				foreach (var mod in Modifiers)
				{
					result = result.Append(mod.GetResultString());
				}
			}

			return $"{result.ToString()} = {GetResult().ToString()}";
		}
	}
}