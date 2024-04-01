using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HDyar.DiceRoller.RollCodeParser
{
	public class Evaluator
	{
		public DiceResult result;

		public delegate int RollOneDiceFunctionDelegate(int numDice);

		public RollOneDiceFunctionDelegate RollFunction = DefaultRoll;

		public DiceResult Evaluate(RollCode code, RollOneDiceFunctionDelegate rollFunction = null)
		{
			if (rollFunction != null)
			{
				RollFunction = rollFunction;
			}
			
			result = new DiceResult();
			foreach (var rootExpression in code.Expression)
			{
				result.AddResult(Evaluate(rootExpression));
			}

			return result;
		}

		public DiceResult Evaluate(Expression exp)
		{
			if (exp is DiceRollExpression dre)
			{
				result = new DiceResult();
				var numDice = GetValueFromExpression(dre.NumberDice);
				var numFaces = GetValueFromExpression(dre.NumberFaces);
				for (int i = 0; i < numDice; i++)
				{
					int r = RollFunction(numFaces);
					result.Rolls.Add((r,numFaces));
				}
				return result;

			}else if (exp is ModifierExpression mod)
			{
				if (mod.Modifier == Modifier.Add)
				{
					var m = new DiceResult();
					m.ModifierTotal += GetValueFromExpression(mod.Expression);
					return m;
				}else if (mod.Modifier == Modifier.Subtract)
				{
					var m = new DiceResult();
					m.ModifierTotal -= GetValueFromExpression(mod.Expression);
					return m;
				}
				else
				{
					Debug.LogError("multiply or divide not currently supported");
					return new DiceResult();
				}
			}else if (exp is ExpressionGroup group)
			{
				var result = new DiceResult();
				foreach (var e in group.Expressions)
				{
					result.AddResult(Evaluate(e));
				}

				return result;
			}
			else
			{
				Debug.LogError("Invalid Root Expression");
			}

			return null;
		}

		private static int DefaultRoll(int numFaces)
		{
			return Random.Range(1, numFaces + 1);
		}

		public int GetValueFromExpression(Expression exp)
		{
			if (exp is NumberExpression ne)
			{
				return ne.Value;
			}
			else
			{
				var r = Evaluate(exp);
				return r.Total;
			}
		}
	}
}