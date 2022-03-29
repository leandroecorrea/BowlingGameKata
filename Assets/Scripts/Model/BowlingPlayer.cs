using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPlayer : IBowlingPlayer
{
    public string Name { get; }
    public Turn[] Turns { get; set; }
    public readonly int totalTurns = 10;
    public BowlingPlayer(string name)
    {
        this.Name=name;
        Turns = new Turn[totalTurns];
        for (int i = 0; i < totalTurns; i++)
        {
            Turns[i] = new Turn();
        }
    }

    public void Throw(int pinsThrown)
    {
        foreach (var turn in Turns)
        {
            if (turn.HasMoreThrows())
            {
                turn.Throw(pinsThrown);
                if (turn.Status == TurnStatusEnum.SPARE && turn.ExtraThrowsEnabled && turn == Turns[totalTurns-1])
                {
                    turn.GainOneBonusThrow();
                }
                else if (turn.Status == TurnStatusEnum.STRIKE && turn.ExtraThrowsEnabled && turn == Turns[totalTurns-1])
                {
                    turn.GainTwoBonusThrow();
                }
                return;
            }
        }
    }

}
