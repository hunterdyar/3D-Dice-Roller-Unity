using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace HDyar.DiceRoller.RollCodeParser
{
	public class DiceCodeParser
	{
		public List<Token> Tokens;
		public List<Expression> Expressions;
		private int _pos;
		public DiceCodeParser(List<Token> tokens)
		{
			Tokens = tokens;
		}
		
		public void Parse()
		{
			_pos = 0;
			Expressions = new List<Expression>();
			while (_pos < Tokens.Count)
			{
				Expressions.Add(ParseNextToken());
			}
		}

		public override string ToString()
		{
			StringBuilder s = new StringBuilder();
			foreach (var e in Expressions)
			{
				s.AppendLine(e.ToString());
			}

			return s.ToString();
		}

		private Expression ParseNextToken()
		{
			var token = Tokens[_pos];
			switch (token.TType)
			{
				case RollTokenType.Number:
					var n = ParseNumberToken();
					return n;	
				case RollTokenType.Add:
				case RollTokenType.Divide:
				case RollTokenType.Multiply:
				case RollTokenType.Subtract:
					return ParseModifierToken();
				case RollTokenType.DiceSep:
					return ParseDiceToken();
				case RollTokenType.Explode:
					return ParseExplodeToken();
				case RollTokenType.Keep:
					return ParseKeepToken();
			}

			return null;
		}

		private Expression ParseKeepToken()
		{
			var left = PopLeftExpressionOrError();
			if (left is DiceRollExpression dre)
			{
				_pos++;
				dre.Keep = ParseNextToken();
				return dre;
			}
			else
			{
				//todo: exploding groups.
			}

			Debug.LogError("Keep token in bad location. ");
			_pos++;
			return left;
		}

		private Expression ParseExplodeToken()
		{
			var left = PopLeftExpressionOrError();
			if (left is DiceRollExpression dre)
			{
				dre.Exploding = true;
				_pos++;
				return dre;
			}
			else
			{
				//todo: exploding groups.
				Debug.LogError("Exploding token in bad location.");
			}

			_pos++;
			return left;
		}

		private Expression PopLeftExpressionOrError()
		{
			//get previous expression
			Expression left;
			if (Expressions.Count == 0)
			{
				//"d4" should become "1d4".
				left = new NumberExpression()
				{
					Value = 1,
				};
			}
			else
			{
				left = Expressions[^1];
				Expressions.Remove(left);
				return left;
			}

			Debug.LogError("Unable to pull left expression! invalid syntax.");
			return null;
		}
		private Expression ParseDiceToken()
		{

			Expression left = PopLeftExpressionOrError();
			//2d20d2 is roll 2d20 and drop the lowest 2 results.
			if (left is DiceRollExpression existingDRE)
			{
				_pos++;
				existingDRE.Drop = ParseNextToken();
				return existingDRE;
			}
			
			var dre = new DiceRollExpression();
			//consume the sep
			_pos++;
			dre.NumberDice = left;
			dre.NumberFaces = ParseNextToken();
			return dre;
		}

		private Expression ParseModifierToken()
		{
			var token = Tokens[_pos];
			var modifier = new ModifierExpression();
			if (token.TType == RollTokenType.Add)
			{
				modifier.Modifier = Modifier.Add;
			}else if (token.TType == RollTokenType.Multiply)
			{
				modifier.Modifier = Modifier.Multiply;
			}else if (token.TType == RollTokenType.Divide)
			{
				modifier.Modifier = Modifier.Divide;
			}else if (token.TType == RollTokenType.Subtract)
			{
				modifier.Modifier = Modifier.Subtract;
			}
			//consume the token.
			_pos++;
			modifier.Expression = ParseNextToken();
			return modifier;
		}

		private Expression ParseNumberToken()
		{
			var token = Tokens[_pos];
			if (token is NumberToken nt)
			{
				var numberExpression = new NumberExpression();
				numberExpression.Value = nt.Value;
				_pos++;//consume!
				return numberExpression;
			}
			else
			{
				Debug.LogError($"Unable to parse {token} as token.");
				return null;
			}
		}
	}
}