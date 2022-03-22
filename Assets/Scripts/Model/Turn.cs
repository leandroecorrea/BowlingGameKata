using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn 
{
    private int maxPins = 10;
    public int ThrowsRemaining { get; private set; }
    public int Pins { get; private set; }
    public TurnStatusEnum Status { get; private set; }
    public Turn()
    {
        Pins = maxPins;
        ThrowsRemaining = 2;
        Status = TurnStatusEnum.ONGOING;
    }

    public void Throw(int thrownPins)
    {
        ThrowsRemaining--;
        Pins -= thrownPins;
        RecalculateTurnStatus();
    }
    private void RecalculateTurnStatus()
    {
        if (Pins == 0 && ThrowsRemaining == 0)
        {
            Status = TurnStatusEnum.SPARE;
        }
        else if (Pins == 0 && ThrowsRemaining > 0)
        {
            Status=TurnStatusEnum.STRIKE;
        }
        else if (Pins > 0 && ThrowsRemaining == 0)
        {
            Status=TurnStatusEnum.NORMAL;
        }
    }

    public int Score()
    {
        return maxPins - Pins;
    }

}
