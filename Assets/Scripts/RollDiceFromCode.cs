using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HDyar.DiceRoller.RollCodeParser;
using UnityEngine;
using Random = System.Random;

namespace HDyar.DiceRoller
{
    
public class RollDiceFromCode : MonoBehaviour 
{
    public DiceCollection DiceCollection;
    public DiceRoller Roller;
    public bool IsRolling => true;

    public delegate void ResultDelegate(int result);
    public void Roll(string code)
    {
        StartCoroutine(DoRoll(code));
    }

    public IEnumerator DoRoll(string code)
    {
        var rollcode = new RollCode(code);
        
        foreach (var rollGroup in rollcode.Roll.DiceRolls)
        {
            foreach (var droll in rollGroup.DiceRollDescriptions)
            {
                yield return StartCoroutine(RollDice(droll.numberTimesToRoll, droll.numberSides, droll.GetRollResultTotal));
            }
        }
        
        Debug.Log(rollcode.Roll.GetResultString());
    }

    public IEnumerator RollDice(int dice, int faceCount, ResultDelegate onComplete) 
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

        bool rolling = true;
        
        
       yield return StartCoroutine(Roller.DoRollDice(DiceToRoll));
       int rollerTotal = Roller.LastRolledDice.Sum(x=>x.currentUpFace.Value);
       onComplete?.Invoke(rollerTotal);
    }
}
}
