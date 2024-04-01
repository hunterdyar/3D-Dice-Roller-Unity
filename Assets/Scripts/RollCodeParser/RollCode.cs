using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HDyar.DiceRoller.RollCodeParser
{
	public class RollCode
	{
		private string code;
		public List<Expression> Expression { get; set; }
		private List<Token> _tokens;
		private DiceCodeParser _parser;
		public RollCode(string code)
		{
			this.code = code;
			var tokens = Tokenize(code);
			_parser = new DiceCodeParser(tokens);
			_parser.Parse();
			Debug.Log("Parsed as:");
			Debug.Log(_parser.ToString());
		}

		private List<Token> Tokenize(string s)
		{
			s = s.Trim().ToLower();
			var tokens = new List<Token>();
			for (int i = 0; i < s.Length; i++)
			{
				var c = s[i];
				if (c == ' ' || c == '\n' || c == '\t' || c == '\r')
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
					Debug.LogError($"Unknown Character {c}");
					continue;
				}
			}
			return tokens;
		}
	}
}