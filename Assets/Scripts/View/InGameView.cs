using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameView : MonoBehaviour, IInGameView
{
    [SerializeField]
    private GameObject PlayerOneScore;
    [SerializeField]
    private GameObject PlayerTwoScore;
    [SerializeField]
    private GameObject InputField;

    private BowlingGamePresenter _bowlingGamePresenter;
    // Start is called before the first frame update
    void Start()
    {
        _bowlingGamePresenter = BowlingGamePresenterBuilder.Build(this);        
        transform.GetComponentInChildren<Button>().onClick.AddListener(OnReceiveThrow);        
    }

    public void InitScoreBoardsView(string playerOneName, string playerTwoName)
    {
        InitScoreBoard(PlayerOneScore, playerOneName);
        InitScoreBoard(PlayerTwoScore, playerTwoName);
    }
    void InitScoreBoard(GameObject playerScore, string playerName)
    {
        foreach (Transform child in playerScore.transform)
        {
            if(child.name == "PlayerText")
            {
                child.GetComponent<Text>().text = playerName;
            }
            if (child.name.Contains("TurnScoreContainer"))
            {
                GameObject firstThrow = child.GetChild(0).transform.GetChild(0).gameObject;
                GameObject secondThrow = child.GetChild(0).transform.GetChild(1).gameObject;
                GameObject turnScore = child.GetChild(1).gameObject;
                firstThrow.GetComponent<Text>().text = "";
                secondThrow.GetComponent<Text>().text = "";
                turnScore.GetComponent<Text>().text = "";
            }
        }
    }
    void OnReceiveThrow()
    {
        int pinsAmount;
        if (int.TryParse(InputField.GetComponent<Text>().text, out pinsAmount))
        {
            _bowlingGamePresenter.ReceiveThrow(pinsAmount);
        }
        else
        {
            //Manejate...en el frontend
        }


    }

    public void UpdateScoreboard()
    {
        
    }
}

