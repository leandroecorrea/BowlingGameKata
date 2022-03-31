using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInGameView 
{
    void InitScoreBoardsView(string playerOneName, string playerTwoName);

    void UpdatePlayerTurnName(string playerName);

    TurnViewDetail GetPlayerTurnViewDetailFor(int playerIndex, int turnIndex);

    List<TurnViewDetail[]> TurnsDetailViewList();

    void SetFinalScoreForPlayer(int playerIndex, int finalScore);
    void GameEnded();

    void DisableThrows();
    
    void ShowWinningPlayer(string playerName);
    void ShowErrorMessage(string playerName);
}
