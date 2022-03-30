using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardFactory
{
    private BowlingPlayerFactory playerFactory;
    public ScoreboardFactory()
    {
        playerFactory = new BowlingPlayerFactory();
    }
    public IScoreboard CreateAscoreboard()
    {

        IBowlingPlayer[] players = playerFactory.Create("German", "Lean");
        IScoreboard scoreboard = new Scoreboard(players);
        return scoreboard;

    }
}
