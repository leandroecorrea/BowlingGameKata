using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingPlayerFactory
{
    public IBowlingPlayer[] Create(params string[] playerNames)
    {
        IBowlingPlayer[] players = new IBowlingPlayer[playerNames.Length];
        for(int i = 0; i < playerNames.Length; i++)
        {
            players[i] = new BowlingPlayer(playerNames[i]);
        }
        return players;
    }
}
