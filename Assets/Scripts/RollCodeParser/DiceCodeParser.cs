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
			}

			return null;
		}

		private Expression ParseDiceToken()
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