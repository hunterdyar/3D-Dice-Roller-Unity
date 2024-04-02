﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HDyar.DiceRoller.RollCodeParser.RollDescription;
using UnityEngine;

namespace HDyar.DiceRoller.RollCodeParser
{
	public class RollCode
	{
		private string code;
		public List<Expression> Expression => _parser.Expressions; 
		private List<Token> _tokens;
		private DiceCodeParser _parser;
		private Evaluator _evaluator;
		public StandardRoll Roll;
		delegate int RollDiceDelegate(DiceRollExpression dre);
		public RollCode(string code)
		{
			this.code = code;
			var tokens = Tokenize(code);
			_parser = new DiceCodeParser(tokens);
			_parser.Parse();
			_evaluator = new Evaluator();
			Roll = _evaluator.Evaluate(_parser.Expressions);
			Debug.Log($"Parsed as: {_parser.ToString()}");
		}

		public void Evaluate()
		{
			//parse through tree and pass in appropriate delegate to handle different types of functions? 
			//chain the callbacks along or have a master callback that sorts?
		}

		private List<Token> Tokenize(string s)
		{
			s = s.Trim().ToLower();
			var tokens = new List<Token>();
			for (int i = 0; i < s.Length; i++)
			{
				var c = s[i];
				//, is just a seperator. You can comma-separate rolls "1d6,2d20" or "1d6+2d20" or "1d6 2d20" will all work (although the + will parse differently)
				if (c == ' ' || c == '\n' || c == '\t' || c == '\r' || c == ',')
				{
					continue;
				}else if ("0123456789".Contains(c))
				{
					if (tokens.Count > 0 && tokens[^1].TType == RollTokenType.Number)
					{
						tokens[^1].Literal += c;
						if (tokens[^1] is NumberToken nt)
						{
							nt.RecalculateValue();
						}
						continue;
					}
					else
					{
						var t = new NumberToken(c.ToString());
						tokens.Add(t);
						continue;
					}
				}else if (c == 'd')
				{
					var t = new Token(RollTokenType.DiceSep, c);
					tokens.Add(t);
					continue;
				}else if (c == '+')
				{
					tokens.Add(new Token(RollTokenType.Add,c));
					continue;
				}else if (c == '-')
				{
					tokens.Add(new Token(RollTokenType.Subtract,c));
					continue;
				}else if (c == 'x' || c == '*')
				{
					tokens.Add(new Token(RollTokenType.Multiply,c));
					continue;
				}else if (c == '/')
				{
					tokens.Add(new Token(RollTokenType.Divide,c));
					continue;
				}
				else
				{
					Debug.LogError($"Unexpected character {c}");
					continue;
				}
			}
			return tokens;
		}
	}
}