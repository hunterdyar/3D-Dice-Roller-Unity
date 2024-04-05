using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HDyar.DiceRoller.RollCodeParser.RollDescription;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HDyar.DiceRoller.RollCodeParser
{
	public class Evaluator
	{
		public StandardRoll Evaluate(List<Expression> expressions)
		{
			StandardRoll sr = new StandardRoll();
			foreach (var expr in expressions)
			{ 
				Evaluate(expr, ref sr);
			}

			return sr;
		}

		public void Evaluate(Expression exp, ref StandardRoll roll) 
		{
			//todo: yield for evaluation of result.
			if (exp is DiceRollExpression dre)
			{
				var numDice = GetValueFromExpression(dre.NumberDice);
				var numFaces = GetValueFromExpression(dre.NumberFaces);
				int drop = GetValueFromExpression(dre.Drop);
				int keep = GetValueFromExpression(dre.Keep);
				//for now we only support single highest face.
				var eb = dre.Exploding ? ExplodeBehaviour.ExplodeOnSingleHighestFace : ExplodeBehaviour.DontExplode;
				GroupOfDiceDescription group = new GroupOfDiceDescription(numDice,numFaces, drop,keep, eb);
				group.Label = dre.Label;
				roll.AppendGroup(group);
			}else if (exp is ModifierExpression mod)
			{
				if (mod.Modifier == Modifier.Add)
				{
					roll.AppendModifier(new StaticModifier(GetValueFromExpression(mod.Expression),mod.Label));
				}else if (mod.Modifier == Modifier.Subtract)
				{
					roll.AppendModifier(new StaticModifier(0- GetValueFromExpression(mod.Expression),mod.Label));
				}
				else
				{
					Debug.LogError("multiply or divide not currently supported");
					throw new NotImplementedException("oops.");
				}
			}else if (exp is ExpressionGroup group)
			{
				//label
				throw new NotImplementedException("Expression Groups not yet supported");
			}
			else
			{
				Debug.LogError("Invalid Root Expression");
			}
		}

		// private static int DefaultRoll(int numFaces)
		// {
		// 	return Random.Range(1, numFaces + 1);
		// }

		public int GetValueFromExpression(Expression exp)
		{
			if (exp == null)
			{
				//This is valid for drop and keep items - if we don't have them, they'll be null!
				return 0;
			}
			
			if (exp is NumberExpression ne)
			{
				return ne.Value;
			}
			else
			{
				//todo: We have to yield here!
				//we need to make some kind of ... thing for this...
				throw new ArgumentException("Not supporting expressions that aren't numbers, yet.");
			}
		}
	}
}