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
            if (exp is DiceRollExpression dre)
            {
                RollDice(dre);
            }else if (exp is ModifierExpression mod)
            {
                
            }
        }
    }

    private void RollDice(DiceRollExpression dre)
    {
        throw new System.NotImplementedException();
    }
}
}
