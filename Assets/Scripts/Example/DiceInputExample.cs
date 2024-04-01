﻿using System;
using HDyar.DiceRoller.RollCodeParser;
using TMPro;
using UnityEngine;

namespace HDyar.DiceRoller.Example
{
	public class DiceInputExample : MonoBehaviour
	{
		private TMP_InputField _inputField;

		private void Awake()
		{
			_inputField = GetComponent<TMP_InputField>();
			_inputField.onSubmit.AddListener(Roll);	
		}

		private void OnEnable()
		{
			
		}

		public void Roll()
		{
			Roll(_inputField.text);	
		}

		public void Roll(string code)
		{
			var roll = new RollCode(code);
			
		}
	}
}