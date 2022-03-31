using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlingGamePresenter : IPlayerObserver
{
    private IInGameView _gameView;
    private IScoreboard _scoreboard;
    public IInGameView GameView { get => _gameView; }
    public IScoreboard Scoreboard { get => _scoreboard; }
    public IBowlingPlayer CurrentPlayer { get; private set; }
    private int currentPlayerIndex;
    private int nextPlayerIndex;
    private int[] playersScore;
    public BowlingGamePresenter(IInGameView view, IScoreboard scoreboard)
    {
         playersScore = new int[2];
        _scoreboard = scoreboard;
        _gameView = view;
        _gameView.InitScoreBoardsView(_scoreboard.Players[0].Name, _scoreboard.Players[1].Name);
        SubscribeToPlayers();
        currentPlayerIndex = 0;
        nextPlayerIndex = 0;
        CurrentPlayer = _scoreboard.Players[currentPlayerIndex];
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
        if (_scoreboard.Players[0] == aPlayer)
        {
            nextPlayerIndex = 1;
        }
        else
        {
            nextPlayerIndex = 0;
        }
    }

    public void ReceiveThrow(int pinsAmount)
    {
        CurrentPlayer.Throw(pinsAmount);
        List<Turn> turnsToBeRendered = new List<Turn>();
        foreach(Turn turn in CurrentPlayer.Turns)
        {
            if (turn.Status!=TurnStatusEnum.STANDBY)
            {
                turnsToBeRendered.Add(turn);
            }
            else
            {
                break;
            }
        }
        int accumulatedScore = 0;
        for(int turnIndex = 0; turnIndex < turnsToBeRendered.Count; turnIndex++)
        {
            Turn turn = turnsToBeRendered[turnIndex];
            TurnViewDetail turnViewDetail = _gameView.GetPlayerTurnViewDetailFor(currentPlayerIndex, turnIndex);
            switch (turn.Status)
            {
                case TurnStatusEnum.ONGOING:
                    turnViewDetail.FirstThrow.GetComponentInChildren<Text>().text = turn.PinsThrownOn(0).ToString();
                    break;
                case TurnStatusEnum.NORMAL:
                    turnViewDetail.FirstThrow.GetComponentInChildren<Text>().text = turn.PinsThrownOn(0).ToString();
                    turnViewDetail.SecondThrow.GetComponentInChildren<Text>().text = turn.PinsThrownOn(1).ToString();
                    accumulatedScore += turn.TotalPinsThrown();
                    turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();
                    break;
                case TurnStatusEnum.SPARE:
                    turnViewDetail.FirstThrow.GetComponentInChildren<Text>().text = turn.PinsThrownOn(0).ToString();
                    turnViewDetail.SecondThrow.GetComponentInChildren<Text>().text = "-";
                    if (turnIndex+1<turnsToBeRendered.Count)
                    {
                        Turn nextTurn = turnsToBeRendered[turnIndex+1];
                        accumulatedScore += (nextTurn.PinsThrownOn(0)+10);
                        turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();
                        
                    }
                    break;
                case TurnStatusEnum.STRIKE:
                    turnViewDetail.FirstThrow.GetComponentInChildren<Text>().text = "";
                    turnViewDetail.SecondThrow.GetComponentInChildren<Text>().text = "X";
                    if (turnIndex+1<turnsToBeRendered.Count)
                    {
                        Turn nextTurn = turnsToBeRendered[turnIndex+1];
                        if(nextTurn.Status == TurnStatusEnum.STRIKE)
                        {
                            if (turnIndex+2<turnsToBeRendered.Count)
                            {
                                Turn nextNextTurn = turnsToBeRendered[turnIndex+1];
                                accumulatedScore +=(nextNextTurn.TotalPinsThrown()+20);
                                turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();
                            }
                        }
                        else
                        {
                            accumulatedScore +=(nextTurn.TotalPinsThrown()+10);
                            turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();
                        }

                    }
                    break;
            }
            if (!turn.ExtraThrowsEnabled&&!turn.HasMoreThrows())
            {
                turnViewDetail.ThirdThrow.GetComponentInChildren<Text>().text = turn.PinsThrownOn(2).ToString();
                accumulatedScore += turn.TotalPinsThrown();
                turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();
            }
        }
        _gameView.SetFinalScoreForPlayer(currentPlayerIndex, accumulatedScore);
        playersScore[currentPlayerIndex] = accumulatedScore;
        if(currentPlayerIndex == 1 && !CurrentPlayer.Turns[9].HasMoreThrows())
        {
            string winnerPlayersName;
            if (playersScore[0]==playersScore[1])
            {
                winnerPlayersName = "It's a tie!";
            }
            else
            {
                if (playersScore[0]>playersScore[1])
                {
                    winnerPlayersName = _scoreboard.Players[0].Name+" won!";
                }
                else
                {
                    winnerPlayersName = _scoreboard.Players[1].Name+" won!";
                }
            }
            _gameView.ShowWinningPlayer(winnerPlayersName);
            _gameView.DisableThrows();
            _gameView.GameEnded();
        }
        else
        {
            _gameView.UpdatePlayerTurnName(CurrentPlayer.Name);
        }
        CurrentPlayer = _scoreboard.Players[nextPlayerIndex];
        currentPlayerIndex = nextPlayerIndex;
    }
}
