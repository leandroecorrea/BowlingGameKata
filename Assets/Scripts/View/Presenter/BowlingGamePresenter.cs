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
        InitializeCurrentPlayer();
    }

    private void InitializeCurrentPlayer()
    {
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
        if(CheckPinsAmount(pinsAmount))
        {
            CurrentPlayer.Throw(pinsAmount);
            RenderTurnsInView();
            CheckIfGameShouldContinue();
        }
        else
        {
            _gameView.ShowErrorMessage(CurrentPlayer.Name);            
        }
    }

    private void CheckIfGameShouldContinue()
    {
        if (GameEnded())
        {
            string results = GameEndedResultMessage();
            _gameView.ShowWinningPlayer(results);
            _gameView.DisableThrows();
            _gameView.GameEnded();
        }
        else
        {
            UpdatePlayerForNextTurn();
        }
    }

    private void RenderTurnsInView()
    {
        List<Turn> turnsToBeRendered = new List<Turn>();        
        foreach (Turn turn in CurrentPlayer.Turns)
        {
            if (turn.Status != TurnStatusEnum.STANDBY)
            {
                turnsToBeRendered.Add(turn);
            }
            else
            {
                break;
            }
        }
        int accumulatedScore = 0;
        for (int turnIndex = 0; turnIndex < turnsToBeRendered.Count; turnIndex++)
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
                    if (turnIndex + 1 < turnsToBeRendered.Count)
                    {
                        Turn nextTurn = turnsToBeRendered[turnIndex + 1];
                        accumulatedScore += (nextTurn.PinsThrownOn(0) + 10);
                        turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();
                    }
                    break;
                case TurnStatusEnum.STRIKE:
                    turnViewDetail.FirstThrow.GetComponentInChildren<Text>().text = "";
                    turnViewDetail.SecondThrow.GetComponentInChildren<Text>().text = "X";
                    if (turnIndex + 1 < turnsToBeRendered.Count)
                    {
                        Turn nextTurn = turnsToBeRendered[turnIndex + 1];
                        if (nextTurn.Status == TurnStatusEnum.STRIKE)
                        {
                            if (turnIndex + 2 < turnsToBeRendered.Count)
                            {
                                Turn nextNextTurn = turnsToBeRendered[turnIndex + 1];
                                accumulatedScore += (nextNextTurn.TotalPinsThrown() + 20);
                                turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();
                            }
                        }
                        else
                        {
                            accumulatedScore += (nextTurn.TotalPinsThrown() + 10);
                            turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();
                        }

                    }
                    break;
            }
            if (!turn.ExtraThrowsEnabled && !turn.HasMoreThrows())
            {
                turnViewDetail.ThirdThrow.GetComponentInChildren<Text>().text = turn.PinsThrownOn(2).ToString();
                accumulatedScore += turn.TotalPinsThrown();
                turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();
            }
        }
        _gameView.SetFinalScoreForPlayer(currentPlayerIndex, accumulatedScore);
        playersScore[currentPlayerIndex] = accumulatedScore;
    }

    private string GameEndedResultMessage()
    {        
        if (playersScore[0] == playersScore[1])
        {
            return "It's a tie!";
        }
        else
        {
            if (playersScore[0] > playersScore[1])
            {
                return _scoreboard.Players[0].Name + " won!";
            }
            else
            {
                return _scoreboard.Players[1].Name + " won!";
            }
        }
    }

    private bool GameEnded()
    {
        return currentPlayerIndex == 1 && !CurrentPlayer.Turns[9].HasMoreThrows();
    }

    private bool CheckPinsAmount(int pinsAmount)
    {
        int pinsRemaining = Turn.MAX_PINS - CurrentPlayer.CurrentTurn().TotalPinsThrown();
        return pinsAmount <= pinsRemaining && pinsAmount <= Turn.MAX_PINS && pinsAmount >= 0;
    }

    private void UpdatePlayerForNextTurn()
    {
        CurrentPlayer = _scoreboard.Players[nextPlayerIndex];
        currentPlayerIndex = nextPlayerIndex;
        _gameView.UpdatePlayerTurnName(CurrentPlayer.Name);
    }
}
