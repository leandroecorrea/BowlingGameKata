using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameView : MonoBehaviour, IInGameView
{
    [SerializeField]
    private GameObject PlayerOneScore;
    [SerializeField]
    private GameObject PlayerTwoScore;
    [SerializeField]
    private GameObject InputField;
    [SerializeField]
    private GameObject ThrowButton;
    [SerializeField]
    private GameObject PlayerTurnName;

    private List<GameObject> playersTotalScores;
    
    List<TurnViewDetail[]> turnsDetailViewList;


    private BowlingGamePresenter _bowlingGamePresenter;
    // Start is called before the first frame update
    void Start()
    {
        _bowlingGamePresenter = BowlingGamePresenterBuilder.Build(this);        
        transform.GetComponentInChildren<Button>().onClick.AddListener(OnReceiveThrow);
    }

    public void InitScoreBoardsView(string playerOneName, string playerTwoName)
    {
        playersTotalScores = new List<GameObject>();
        turnsDetailViewList = new List<TurnViewDetail[]>();
        InitScoreBoard(PlayerOneScore, playerOneName);
        InitScoreBoard(PlayerTwoScore, playerTwoName);
    }
    void InitScoreBoard(GameObject playerScore, string playerName)
    {
        TurnViewDetail[] turnViewDetails = new TurnViewDetail[10];
        int turnViewDetailCounter = 0;
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
                GameObject finalTurnScore = child.GetChild(1).gameObject;
                
                firstThrow.GetComponent<Text>().text = "";
                secondThrow.GetComponent<Text>().text = "";
                finalTurnScore.GetComponent<Text>().text = "";
                TurnViewDetail turnViewDetail;
                if (child.name == "TurnScoreContainer (10)")
                {
                    GameObject thirdThrow = child.GetChild(0).transform.GetChild(2).gameObject;
                    thirdThrow.GetComponent<Text>().text =  "";
                    turnViewDetail = new TurnViewDetail(firstThrow, secondThrow, thirdThrow, finalTurnScore);
                }
                else
                {
                    turnViewDetail = new TurnViewDetail(firstThrow, secondThrow, finalTurnScore);
                }
                turnViewDetails[turnViewDetailCounter] = turnViewDetail;
                turnViewDetailCounter++;
            }
            if (child.name =="TotalScore")
            {
                child.GetComponent<Text>().text = "0";
                playersTotalScores.Add(child.gameObject);
            }
        }
        turnsDetailViewList.Add(turnViewDetails);
    }
    void OnReceiveThrow()
    {
        int pinsAmount;
        string pinsKnockedInputValue = InputField.GetComponentInChildren<Text>().text;

        if (int.TryParse(pinsKnockedInputValue, out pinsAmount)&&pinsAmount>=0)
        {
            _bowlingGamePresenter.ReceiveThrow(pinsAmount);
            Debug.Log(pinsAmount);
        }
        else
        {
            //Manejate...en el frontend
        }
    }

    

    public void UpdatePlayerTurnName(string playerName)
    {
        PlayerTurnName.GetComponent<Text>().text = playerName+"'s turn.";
    }

    
    

    public List<TurnViewDetail[]> TurnsDetailViewList()
    {
        return turnsDetailViewList;
    }

    public TurnViewDetail GetPlayerTurnViewDetailFor(int playerIndex, int turnIndex)
    {
        return turnsDetailViewList[playerIndex][turnIndex];
    }

    public void SetFinalScoreForPlayer(int playerIndex, int finalScore)
    {
        playersTotalScores[playerIndex].GetComponent<Text>().text = finalScore.ToString();
    }

    public void GameEnded()
    {
        StartCoroutine(LoadEndScene());
    }
    public IEnumerator LoadEndScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(2);
    }
    public void DisableThrows()
    {
        InputField.SetActive(false);
        ThrowButton.SetActive(false);
    }

    public void ShowWinningPlayer(string playerName)
    {
        PlayerTurnName.GetComponent<Text>().text = playerName;
    }
}

