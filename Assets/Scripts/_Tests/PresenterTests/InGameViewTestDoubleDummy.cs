using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameViewTestDoubleDummy : IInGameView
{
    public void DisableThrows()
    {
    }

    public void GameEnded()
    {
    }

    public TurnViewDetail GetPlayerTurnViewDetailFor(int playerIndex, int turnIndex)
    {
        GameObject firstThrow = new GameObject();
        GameObject secondThrow = new GameObject();
        GameObject thirdThrow = new GameObject();
        GameObject finalScore = new GameObject();
        firstThrow.AddComponent<Text>();
        secondThrow.AddComponent<Text>();
        thirdThrow.AddComponent<Text>();
        finalScore.AddComponent<Text>();
        return new TurnViewDetail(firstThrow,secondThrow,thirdThrow,finalScore);
    }

    public void InitScoreBoardsView(string playerOneName, string playerTwoName)
    {
        
    }

    public void SetFinalScoreForPlayer(int playerIndex, int finalScore)
    {
        
    }

    public void ShowWinningPlayer(string playerName)
    {
        throw new System.NotImplementedException();
    }

    public List<TurnViewDetail[]> TurnsDetailViewList()
    {
        return null;
    }

    public void UpdatePlayerTurnName(string playerName)
    {
        
    }      
}
