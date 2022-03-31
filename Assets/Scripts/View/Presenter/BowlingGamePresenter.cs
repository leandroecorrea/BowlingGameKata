﻿using System;
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
        //Turn playerCurrentTurn = CurrentPlayer.CurrentTurn();
        CurrentPlayer.Throw(pinsAmount);

        // pinsAmount = 9
        // string[] throwResult = {"9"}
        //pinsAmount = 10
        // string[] throwResult = {"", "X"}
        /*List<string> throwResult = new List<string>();
        switch (playerCurrentTurn.Status)
        {
            case TurnStatusEnum.ONGOING:
                int throwAmount = playerCurrentTurn.PinsThrownOn(0);
                if (CurrentPlayer.PreviousTurn()!=null&&CurrentPlayer.PreviousTurn().Status == TurnStatusEnum.SPARE)
                {
                    throwResult.Add((10+throwAmount).ToString());
                }
                throwResult.Add(throwAmount.ToString());
                break;
            case TurnStatusEnum.NORMAL:
                if (CurrentPlayer.PreviousTurn()!=null&&CurrentPlayer.PreviousTurn().Status == TurnStatusEnum.STRIKE)
                {
                    throwResult.Add((10+CurrentPlayer.PreviousTurn().TotalPinsThrown()).ToString());
                }
                throwResult.Add(playerCurrentTurn.PinsThrownOn(1).ToString());
                throwResult.Add(playerCurrentTurn.TotalPinsThrown().ToString());
                break;
            case TurnStatusEnum.SPARE:
                throwResult.Add("-");
                break;
            case TurnStatusEnum.STRIKE:
                throwResult.Add("");
                throwResult.Add("X");
                break;
        }*/
       // _gameView.UpdateScoreboard(currentPlayerIndex, throwResult.ToArray());
        List<Turn> currentPlayerTurns = new List<Turn>();
        foreach(Turn turn in CurrentPlayer.Turns)
        {
            if (turn.Status!=TurnStatusEnum.ONGOING)
            {
                currentPlayerTurns.Add(turn);
            }
            else
            {
                break;
            }
        }
        int accumulatedScore = 0;
        for(int turnIndex = 0; turnIndex < currentPlayerTurns.Count-1; turnIndex++)
        {
            Turn turn = currentPlayerTurns[turnIndex];
            TurnViewDetail turnViewDetail = _gameView.GetPlayerTurnViewDetailFor(currentPlayerIndex, turnIndex);
            switch (turn.Status)
            {
                case TurnStatusEnum.NORMAL:
                    turnViewDetail.FirstThrow.GetComponentInChildren<Text>().text = turn.PinsThrownOn(0).ToString();
                    turnViewDetail.SecondThrow.GetComponentInChildren<Text>().text = turn.PinsThrownOn(1).ToString();
                    accumulatedScore += turn.TotalPinsThrown();
                    turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();
                    break;
                case TurnStatusEnum.SPARE:
                    turnViewDetail.FirstThrow.GetComponentInChildren<Text>().text = turn.PinsThrownOn(0).ToString();
                    turnViewDetail.SecondThrow.GetComponentInChildren<Text>().text = "-";
                    if (turnIndex+1<currentPlayerTurns.Count)
                    {
                        Turn nextTurn = currentPlayerTurns[turnIndex+1];
                        accumulatedScore += (nextTurn.PinsThrownOn(0)+10);
                        turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();
                        
                    }
                    break;
                case TurnStatusEnum.STRIKE:
                    turnViewDetail.FirstThrow.GetComponentInChildren<Text>().text = "";
                    turnViewDetail.SecondThrow.GetComponentInChildren<Text>().text = "X";
                    if (turnIndex+1<currentPlayerTurns.Count)
                    {
                        Turn nextTurn = currentPlayerTurns[turnIndex+1];
                        if(nextTurn.Status == TurnStatusEnum.STRIKE)
                        {
                            if (turnIndex+2<currentPlayerTurns.Count)
                            {
                                Turn nextNextTurn = currentPlayerTurns[turnIndex+1];
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
        }
        if (CurrentPlayer.Turns[9].Status != TurnStatusEnum.ONGOING)
        {
            TurnViewDetail turnViewDetail = _gameView.GetPlayerTurnViewDetailFor(currentPlayerIndex, 9);
            Turn turn = currentPlayerTurns[9];
            turnViewDetail.FirstThrow.GetComponentInChildren<Text>().text = turn.PinsThrownOn(0).ToString();
            turnViewDetail.SecondThrow.GetComponentInChildren<Text>().text = turn.PinsThrownOn(1).ToString();
            if (!turn.ExtraThrowsEnabled&&!turn.HasMoreThrows())
            {
                turnViewDetail.ThirdThrow.GetComponentInChildren<Text>().text = turn.PinsThrownOn(2).ToString();
            }
            accumulatedScore += turn.TotalPinsThrown();
            turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();            
        }
        _gameView.SetFinalScoreForPlayer(currentPlayerIndex, accumulatedScore);
        playersScore[currentPlayerIndex] = accumulatedScore;
        if(currentPlayerIndex == 1 && CurrentPlayer.Turns[9].Status != TurnStatusEnum.ONGOING)
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
