using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HDyar.DiceRoller.RollCodeParser
{
	public class Evaluator
	{
		public DiceResult Result => _result;
		private DiceResult _result;
		private readonly MonoBehaviour _context;
		public IDiceRoller Roller => _roller;
		private readonly IDiceRoller _roller;

		public Evaluator(IDiceRoller roller, MonoBehaviour context)
		{
			this._roller = roller;
			this._context = context;
		}
		
		public DiceResult Evaluate(RollCode code)
		{
			_result = new DiceResult();
			foreach (var expr in code.Expression)
			{
				Coroutine routine = _context.StartCoroutine(Evaluate(expr,_result));
			}
			return _result;
		}

		public IEnumerator Evaluate(Expression exp, DiceResult result)
		{
			//todo: yield for evaluation of result.
			if (exp is DiceRollExpression dre)
			{
				var numDice = GetValueFromExpression(dre.NumberDice);
				var numFaces = GetValueFromExpression(dre.NumberFaces);
				yield return _context.StartCoroutine(_roller.RollDice(numDice, numFaces, result));
			}else if (exp is ModifierExpression mod)
			{
				if (mod.Modifier == Modifier.Add)
				{
					result.ModifierTotal += GetValueFromExpression(mod.Expression);
				}else if (mod.Modifier == Modifier.Subtract)
				{
					result.ModifierTotal -= GetValueFromExpression(mod.Expression);
				}
				else
				{
					Debug.LogError("multiply or divide not currently supported");
					yield break;
				}
			}else if (exp is ExpressionGroup group)
			{
				var subResult = new DiceResult();
				foreach (var e in group.Expressions)
				{
					//todo: make these concurent with Task.WhenAll
					yield return _context.StartCoroutine(Evaluate(e,subResult));
				}
				//return sub-result? i lost the plot but this is boilerplate until I parse groups.
				result.AddResult(subResult);
				yield break;
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
			if (exp is NumberExpression ne)
			{
				return ne.Value;
			}
			else
			{
				//todo: We have to yield here!
				var r = new DiceResult();
				Evaluate(exp,r);
				// while(r.WaitingForResults)....
				return r.Total;
			}
		}
	}
}