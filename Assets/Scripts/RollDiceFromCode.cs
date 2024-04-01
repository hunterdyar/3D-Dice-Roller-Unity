using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HDyar.DiceRoller.RollCodeParser;
using UnityEngine;
using Random = System.Random;

namespace HDyar.DiceRoller
{
    
public class RollDiceFromCode : MonoBehaviour, IDiceRoller
{
    public DiceCollection DiceCollection;
    public DiceRoller Roller;
    public bool IsRolling => true;
    public void Roll(string code)
    {
        StartCoroutine(DoRoll(code));
    }

    public IEnumerator DoRoll(string code)
    {
        var roll = new RollCode(code);

        var e = new Evaluator(this,this);//Rolldice will get called, it will update isRolling
        var result = e.Evaluate(roll);

        while (result.IsRolling())
        {
            yield return null;
        }

       
        Debug.Log(result.Total);
    }

    // public void RollDice(int dice, int faceCount, DiceResult result)
    // {
    //     Coroutine c = this.StartCoroutine(DoRollDice(dice, faceCount, result));
    //     //this doesn't work because coroutines need to be on the main thread.
    // }


    public IEnumerator RollDice(int dice, int faceCount, DiceResult result)
    {
        result.ActiveRolls++;
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
                result.AddRoll(r, faceCount);
            }
        }

        bool rolling = false;
        Roller.OnRollComplete += results =>
        {
            rolling = true;
            foreach (var roll in results)
            {
                result.AddRoll(roll.currentUpFace.Value, roll.Sides);
            }
        };
        
        Roller.RollDice(DiceToRoll);
        
        while (rolling)
        {
            yield return null;
        }

        result.ActiveRolls--; 
    }
}
}
