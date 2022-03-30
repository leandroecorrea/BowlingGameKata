using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingGamePresenter : IPlayerObserver
{
    private IInGameView _gameView;
    private IScoreboard _scoreboard;
    public IInGameView GameView { get => _gameView; }
    public IScoreboard Scoreboard { get => _scoreboard; }
    public IBowlingPlayer CurrentPlayer { get; private set; }
    public BowlingGamePresenter(IInGameView view, IScoreboard scoreboard)
    {
        _scoreboard = scoreboard;
        _gameView = view;
        _gameView.InitScoreBoardsView(_scoreboard.Players[0].Name, _scoreboard.Players[1].Name);
        SubscribeToPlayers();
        CurrentPlayer = _scoreboard.Players[0];
    }

    private void SubscribeToPlayers()
    {
        foreach (var player in _scoreboard.Players)
        {
            player.Attach(this);
        }
    }

    public void SwitchPlayerTurn(IBowlingPlayer aPlayer)
    {
        if(_scoreboard.Players[0] == aPlayer)
        {
            CurrentPlayer = _scoreboard.Players[1];
        }
        else
        {
            CurrentPlayer = _scoreboard.Players[0];
        }
    }

    public void ReceiveThrow(int pinsAmount)
    {
        CurrentPlayer.Throw(pinsAmount);
        _gameView.UpdateScoreboard();
    }
}
