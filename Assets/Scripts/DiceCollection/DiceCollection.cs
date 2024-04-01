using UnityEngine;

namespace HDyar.DiceRoller
{
	[CreateAssetMenu(fileName = "Dice Collection", menuName = "Dice/Dice Collection", order = 0)]
	public class DiceCollection : ScriptableObject
	{
		public Dice[] dicePrefabs;
		
		//todo cache dictionary.
		public Dice GetDicePrefab(int numFaces)
		{
			foreach (var dicePrefab in dicePrefabs)
			{
				if (dicePrefab.Sides == numFaces)
				{
					return dicePrefab;
				}
			}
			
			Debug.LogWarning($"Unable To Get {numFaces} sided dice.",this);
			return null;
		}

		public bool TryGetDicePrefab(int numFaces, out Dice prefab)
		{
			prefab = GetDicePrefab(numFaces);
			return prefab != null;
		}
	}
}