using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingGamePresenterBuilder
{
    public static BowlingGamePresenter Build(IInGameView view)
    {
        IScoreboard scoreboard = new ScoreboardFactory().CreateAscoreboard();
        return new BowlingGamePresenter(view, scoreboard);
    }
}
