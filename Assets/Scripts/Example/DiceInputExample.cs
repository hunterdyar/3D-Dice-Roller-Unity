using System;
using System.Collections;
using HDyar.DiceRoller.RollCodeParser;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace HDyar.DiceRoller.Example
{
	public class DiceInputExample : MonoBehaviour
	{
		private TMP_InputField _inputField;
		[FormerlySerializedAs("_roller")] [SerializeField] private RollDiceFromCode fromCode;//in this namespace, diceRoller is the unity thing.
		private void Awake()
		{
			_inputField = GetComponent<TMP_InputField>();
		}

		public void Roll()
		{
			fromCode.Roll(_inputField.text);	
		}
	}
}