using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blooper.Utilities;
using HDyar.DiceRoller.RollCodeParser;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace HDyar.DiceRoller
{
	public class DiceRoller : MonoBehaviour
	{
		[ScenePath, SerializeField]
		private string DiceRollScene;
		public Dice[] CurrentlyRollingDice;
		public List<Dice> LastRolledDice;
		private DiceWorldCreator _worldCreator;
		public void RollDice(List<Dice> diceToRoll)
		{
			StartCoroutine(DoRollDice(diceToRoll));
		}

		public bool IsRolling()
		{
			return CurrentlyRollingDice.Any(x => !x.isStill);
		}
		public IEnumerator DoRollDice(List<Dice> diceToRoll, bool clearLastRolled = true)
		{
			if (!SceneManager.GetSceneByPath(DiceRollScene).IsValid())
			{
				var s = SceneManager.LoadSceneAsync(DiceRollScene, LoadSceneMode.Additive);
				while (!s.isDone)
				{
					yield return null;
				}
			}

			if (clearLastRolled)
			{
				foreach (var dice in LastRolledDice)
				{
					if (dice != null)
					{
						Destroy(dice.gameObject);
					}
				}
			}

			CurrentlyRollingDice = new Dice[diceToRoll.Count];
			if (_worldCreator == null)
			{
				_worldCreator = GameObject.FindObjectOfType<DiceWorldCreator>();
			}

			_worldCreator.gameObject.SetActive(true);
			var points = _worldCreator.GetSpawnPoints(diceToRoll.Count);
			
			for (int i = 0; i < diceToRoll.Count; i++)
			{
				var dice = Instantiate(diceToRoll[i]);
				SceneManager.MoveGameObjectToScene(dice.gameObject,_worldCreator.gameObject.scene);
				transform.position = points[i].transform.position;
				dice.activeRolling = true;
				dice.currentUpFace = null;
				dice.RandomizeOrientation();
				CurrentlyRollingDice[i] = dice;
				dice.Roll(Random.insideUnitSphere,Random.insideUnitSphere);
			}

			//wait for dice to settle.... this needs a timeout.
			while (CurrentlyRollingDice.Any(x => !x.isStill))
			{
				yield return null;
			}

			if (clearLastRolled)
			{
				LastRolledDice = CurrentlyRollingDice.ToList(); //we bout to delete these tho.
			}
			else
			{
				foreach (var d in CurrentlyRollingDice)
				{
					LastRolledDice.Add(d);
				}
			}
		}
	}
}