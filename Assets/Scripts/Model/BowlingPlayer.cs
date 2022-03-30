using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPlayer : IBowlingPlayer
{
    public string Name { get; }
    public Turn[] Turns { get; set; }
    public static int TOTAL_TURNS = 10;
    public BowlingPlayer(string name)
    {
        this.Name=name;
        Turns = new Turn[TOTAL_TURNS];
        for (int i = 0; i < TOTAL_TURNS; i++)
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
                if (turn.Status == TurnStatusEnum.SPARE && turn.ExtraThrowsEnabled && turn == Turns[TOTAL_TURNS-1])
                {
                    turn.GainOneBonusThrow();
                }
                else if (turn.Status == TurnStatusEnum.STRIKE && turn.ExtraThrowsEnabled && turn == Turns[TOTAL_TURNS-1])
                {
                    turn.GainTwoBonusThrow();
                }
                return;
            }
        }
    }

}
