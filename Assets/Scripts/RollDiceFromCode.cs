using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HDyar.DiceRoller.RollCodeParser;
using HDyar.DiceRoller.RollCodeParser.RollDescription;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace HDyar.DiceRoller
{

    public class RollDiceFromCode : MonoBehaviour
    {
        public DiceCollection DiceCollection;
        public DiceRoller Roller;
        public bool IsRolling => true;
        public TMP_Text resultText;

        public delegate void ResultDelegate(int result, int faces, bool exploded = false);

        public void Roll(string code)
        {
            StartCoroutine(DoRoll(code));
        }

        public IEnumerator DoRoll(string code)
        {
            var rollcode = new RollCode(code);
            resultText.text = "...";
            foreach (var rollGroup in rollcode.Roll.DiceRolls)
            {
                foreach (var droll in rollGroup.DiceRollDescriptions)
                {
                    droll.ResetResult();
                    yield return StartCoroutine(RollDice(droll.numberTimesToRoll, droll.numberSides, droll.exploding,
                        droll.GetRollResultByDice));
                }
            }

            resultText.text = rollcode.Roll.GetResultString();
        }

        public IEnumerator RollDice(int dice, int faceCount, ExplodeBehaviour eb, ResultDelegate onRollComplete)
        {
            List<Dice> DiceToRoll = new List<Dice>();
            for (int i = 0; i < dice; i++)
            {
                if (DiceCollection.TryGetDicePrefab(faceCount, out var prefab))
                {
                    DiceToRoll.Add(prefab);
                }
                else
                {
                    //no sim dice, just invent a number i guess?
                    int r = UnityEngine.Random.Range(1, faceCount + 1);
                    //uh...
                }
            }

            yield return StartCoroutine(Roller.DoRollDice(DiceToRoll));

            //todo: move this into Dice callback so the results come in over time. Fun for exploding to see number go up.
            if (eb == ExplodeBehaviour.ExplodeOnSingleHighestFace)
            {
                //this code is janky.
                var exploders = Roller.LastRolledDice.Where(x => x.currentUpFace.Value == x.Sides).ToList();
                while (exploders.Any(x => x.currentUpFace.Value == x.Sides))
                {
                    foreach (var d in exploders)
                    {
                        //i think this check is redundant.
                        if (d.currentUpFace.Value == d.Sides)
                        {
                            onRollComplete?.Invoke(d.currentUpFace.Value, d.Sides);
                        }
                        //when dice aren't prefabs, it does make sense for them to explode from their current location...
                        d.RandomizeOrientation();
                        //todo: move to new spawn position.
                    }

                    yield return StartCoroutine(Roller.DoRollDice(exploders,false));
                    //
                    exploders = exploders.Where(x => x.currentUpFace.Value == x.Sides).ToList();
                    //then ... check again!
                    //todo: probably lock the position/rotation of the other dice? this code will let them get knocked into exploding.
                }
            }
             foreach (var d in Roller.LastRolledDice)
             { 
                 onRollComplete?.Invoke(d.currentUpFace.Value, d.Sides);
             }
        }//end coroutine.
    }
}
