using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : IScoreboard
{
    public List<IBowlingPlayer> Players { get; private set; }
    
    public Scoreboard(IBowlingPlayer[] players)
    {
        Players = new List<IBowlingPlayer>();
        for(int i = 0; i < players.Length; i++)
        {
            Players.Add(players[i]);
        }
        
    }
    public void AddPlayer(IBowlingPlayer player)
    {
        Players.Add(player);
    }
    public int ScoreForTurn(IBowlingPlayer selectedPlayer, int turnIndex)
    {
        IBowlingPlayer player = Players.Find(p => p == selectedPlayer);
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

    private static bool IsLastTurnOfPlayer(int turnIndex, IBowlingPlayer player)
    {
        return turnIndex == BowlingPlayer.TOTAL_TURNS-1;
    }


}
