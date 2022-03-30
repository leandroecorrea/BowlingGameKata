using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingGamePresenter
{
    private IInGameView _gameView;
    private IScoreboard _scoreboard;
    public IInGameView GameView { get => _gameView; }
    public IScoreboard Scoreboard { get => _scoreboard; }
    public BowlingGamePresenter(IInGameView view, IScoreboard scoreboard)
    {
        _scoreboard = scoreboard;
        _gameView = view;
        _gameView.InitScoreBoardsView(_scoreboard.Players[0].Name, _scoreboard.Players[1].Name);
    }



}
