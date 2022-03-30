using System.Collections.Generic;

public interface IScoreboard
{
    List<IBowlingPlayer> Players { get; }

    void AddPlayer(IBowlingPlayer player);
    int ScoreForTurn(IBowlingPlayer selectedPlayer, int turnIndex);
}