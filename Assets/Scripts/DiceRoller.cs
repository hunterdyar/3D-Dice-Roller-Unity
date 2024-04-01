using System.Collections;
using System.Collections.Generic;
using HDyar.DiceRoller.RollCodeParser;
using UnityEngine;

namespace HDyar.DiceRoller
{
    
public class DiceRoller : MonoBehaviour
{
    public DiceCollection DiceCollection;

    private int total;
    public void Roll(string code)
    {
        var roll = new RollCode(code);
        foreach (var exp in roll.Expression)
        {
            //Evaluate the roll.
            var e = new Evaluator();
            var result = e.Evaluate(roll);
            Debug.Log($"Result: {result.Total}");
        }
    }

}
}
