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
        if (CheckIfLegalPinsAmount(pinsAmount))
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

    private List<Turn> FindTurnsPlayedSoFar()
    {
        List<Turn> turnsPlayedSoFar = new List<Turn>();
        foreach (Turn turn in CurrentPlayer.Turns)
        {
            if (turn.Status != TurnStatusEnum.STANDBY)
            {
                turnsPlayedSoFar.Add(turn);
            }
            else
            {
                break;
            }
        }
        return turnsPlayedSoFar;
    }
    private void RenderTurnsInView()
    {
        List<Turn> turnsToBeRendered = FindTurnsPlayedSoFar();
        int accumulatedScore = 0;
        for (int turnIndex = 0; turnIndex < turnsToBeRendered.Count; turnIndex++)
        {
            Turn turn = turnsToBeRendered[turnIndex];
            TurnViewDetail turnViewDetail = _gameView.GetPlayerTurnViewDetailFor(currentPlayerIndex, turnIndex);
            if (turnIndex==Turn.LAST_TURN_INDEX)
            {
                try
                {
                    accumulatedScore +=turn.PinsThrownOn(0);

                    string throwValue;
                    if (turn.PinsThrownOn(0)==Turn.MAX_PINS)
                    {
                        throwValue = "X";
                    }
                    else
                    {
                        throwValue = turn.PinsThrownOn(0).ToString();
                    }
                    turnViewDetail.FirstThrow.GetComponentInChildren<Text>().text = throwValue;
                    accumulatedScore +=turn.PinsThrownOn(1);
                    if (turn.PinsThrownOn(1)==Turn.MAX_PINS)
                    {
                        throwValue = "X";
                    }
                    else if (turn.PinsThrownOn(1)+turn.PinsThrownOn(0)==Turn.MAX_PINS)
                    {
                        throwValue = "-";
                    }
                    else
                    {
                        throwValue = turn.PinsThrownOn(1).ToString();
                    }
                    turnViewDetail.SecondThrow.GetComponentInChildren<Text>().text = throwValue;
                    accumulatedScore +=turn.PinsThrownOn(2);
                    if (turn.PinsThrownOn(2)==Turn.MAX_PINS)
                    {
                        throwValue = "X";
                    }
                    else if (turn.PinsThrownOn(2)+turn.PinsThrownOn(1)==Turn.MAX_PINS&&turn.PinsThrownOn(0)+turn.PinsThrownOn(1)!=Turn.MAX_PINS)
                    {
                        throwValue = "-";
                    }
                    else
                    {
                        throwValue = turn.PinsThrownOn(2).ToString();
                    }

                    turnViewDetail.ThirdThrow.GetComponentInChildren<Text>().text = throwValue;
                    turnViewDetail.FinalTurnScore.GetComponent<Text>().text = accumulatedScore.ToString();
                }
                catch (Exception)
                {
                    Debug.Log("Last Turn not finished yet.");
                }
            }
            else
            {
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
                                else if(nextTurn.GetEveryThrowMade().Count>=2)
                                {
                                    accumulatedScore += (nextTurn.PinsThrownOn(0)+nextTurn.PinsThrownOn(1));
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
        return currentPlayerIndex == 1 && !CurrentPlayer.Turns[Turn.LAST_TURN_INDEX].HasMoreThrows();
    }

    private bool CheckIfLegalPinsAmount(int pinsAmount)
    {
        if (pinsAmount < 0)
        {
            return false;
        }
        int pinsRemaining = 0;
        Turn playerCurrentTurn = CurrentPlayer.CurrentTurn();
        if (CurrentPlayer.IsLastTurn())
        {
            pinsRemaining=CalculatePinsRemainingForLastTurnThrows(pinsRemaining, playerCurrentTurn);
        }
        else
        {
            pinsRemaining = Turn.MAX_PINS - playerCurrentTurn.TotalPinsThrown();
        }
        return ThrowIsLegal(pinsAmount, pinsRemaining);
    }

    private int CalculatePinsRemainingForLastTurnThrows(int pinsRemaining, Turn playerCurrentTurn)
    {
        if (LastTurnHasNoThrowsYet(playerCurrentTurn))
        {
            pinsRemaining = Turn.MAX_PINS - playerCurrentTurn.TotalPinsThrown();
        }
        else if (LastTurnHasOneThrowMade(playerCurrentTurn))
        {
            if (LastTurnHasStrikeOnFirstThrow(playerCurrentTurn))
            {
                pinsRemaining = Turn.MAX_PINS;
            }
            else
            {
                pinsRemaining = Turn.MAX_PINS - playerCurrentTurn.TotalPinsThrown();
            }
        }
        else if (LastTurnHasTwoShotsMade(playerCurrentTurn))
        {
            if (LastTurnHasAstrikeOrAspareBeforeLastShot(playerCurrentTurn))
            {
                pinsRemaining = Turn.MAX_PINS;
            }
            else
            {
                pinsRemaining = Turn.MAX_PINS - playerCurrentTurn.PinsThrownOn(1);
            }
        }

        return pinsRemaining;
    }

    private bool ThrowIsLegal(int pinsAmount, int pinsRemaining)
    {
        return pinsAmount <= pinsRemaining && pinsAmount <= Turn.MAX_PINS;
    }

    private bool LastTurnHasAstrikeOrAspareBeforeLastShot(Turn playerCurrentTurn)
    {
        return playerCurrentTurn.PinsThrownOn(1)==Turn.MAX_PINS||playerCurrentTurn.PinsThrownOn(0)+playerCurrentTurn.PinsThrownOn(1)==Turn.MAX_PINS;
    }

    private bool LastTurnHasTwoShotsMade(Turn playerCurrentTurn)
    {
        return playerCurrentTurn.GetEveryThrowMade().Count==2;
    }

    private bool LastTurnHasStrikeOnFirstThrow(Turn playerCurrentTurn)
    {
        return playerCurrentTurn.PinsThrownOn(0)==Turn.MAX_PINS;
    }

    private bool LastTurnHasOneThrowMade(Turn playerCurrentTurn)
    {
        return playerCurrentTurn.GetEveryThrowMade().Count==1;
    }

    private bool LastTurnHasNoThrowsYet(Turn playerCurrentTurn)
    {
        return playerCurrentTurn.GetEveryThrowMade().Count==0;
    }

    private void UpdatePlayerForNextTurn()
    {
        CurrentPlayer = _scoreboard.Players[nextPlayerIndex];
        currentPlayerIndex = nextPlayerIndex;
        _gameView.UpdatePlayerTurnName(CurrentPlayer.Name);
    }
}
