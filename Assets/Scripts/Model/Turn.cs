using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Turn 
{
    public static int MAX_PINS = 10;
    private List<int> thrownPins;
    private int maxThrows;
    public bool ExtraThrowsEnabled = true;
    public TurnStatusEnum Status { get; private set; }

    private int throwNumber;
    public Turn()
    {
        Status = TurnStatusEnum.STANDBY;
        thrownPins = new List<int>();
        maxThrows = 2;
        
    }
    public void GainOneBonusThrow()
    {
        ExtraThrowsEnabled = false;
        maxThrows++;
    }
    public void GainTwoBonusThrow()
    {
        ExtraThrowsEnabled = false;
        maxThrows+=2;
    }

    public void Throw(int pinsAmount)
    {
        thrownPins.Add(pinsAmount);
        RecalculateTurnStatus();

    }
    private void RecalculateTurnStatus()
    {
        if (!ExtraThrowsEnabled)
        {
            return;
        }
        if(Status == TurnStatusEnum.STANDBY)
        {
            Status = TurnStatusEnum.ONGOING;
        }
        int pinsThrown = TotalPinsThrown();
        
        if (pinsThrown == MAX_PINS && (thrownPins.Count == 2))
        {
            Status = TurnStatusEnum.SPARE;
        }
        else if (pinsThrown == MAX_PINS && thrownPins.Count == 1)
        {
            maxThrows--;
            Status=TurnStatusEnum.STRIKE;
        }
        else if (thrownPins.Count == 2)
        {
            Status=TurnStatusEnum.NORMAL;
        }
        
    }

    public int TotalPinsThrown()
    {
        return thrownPins.Aggregate(0, (accum, current) => accum + current);
    }

    public int PinsThrownOn(int throwIndex)
    {
        return thrownPins[throwIndex];
    }
    public bool HasMoreThrows()
    {
        return thrownPins.Count < maxThrows;
    }

}
