using UnityEngine;

namespace HDyar.DiceRoller
{
	[CreateAssetMenu(fileName = "DiceFace", menuName = "Dice/Face", order = 0)]
	public class DiceFace : ScriptableObject
	{
		public int Value => _value;
		[SerializeField]
		private int _value;
		
		//Want special faces? random faces? Icons? Etc etc. Data about them can live here.
	}
}