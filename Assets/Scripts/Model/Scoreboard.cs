using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard
{
    public List<BowlingPlayer> Players { get; private set; }

    public Scoreboard()
    {
        Players = new List<BowlingPlayer>();
    }
    public void AddPlayer(BowlingPlayer player)
    {
        Players.Add(player);
    }
    public int ScoreForTurn(BowlingPlayer selectedPlayer, int turnIndex)
    {
        BowlingPlayer player = Players.Find(p => p == selectedPlayer);
        var turn = player.Turns[turnIndex];
        if (turn.Status == TurnStatusEnum.NORMAL)
        {
            return turn.TotalPinsThrown();
        }
        else if (IsLastTurnOfPlayer(turnIndex, player))
        {
            return turn.TotalPinsThrown();
        }
        else if (turn.Status == TurnStatusEnum.SPARE)
        {
            return turn.TotalPinsThrown() + player.Turns[turnIndex+1].PinsThrownOn(0);
        }
        return turn.TotalPinsThrown() + player.Turns[turnIndex+1].TotalPinsThrown();
    }

    private static bool IsLastTurnOfPlayer(int turnIndex, BowlingPlayer player)
    {
        return turnIndex == player.totalTurns-1;
    }


}
