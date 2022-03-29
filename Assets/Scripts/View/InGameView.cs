using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameView : MonoBehaviour
{
    public GameObject PlayerOneScore;
    public GameObject PlayerTwoScore;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponentInChildren<Button>().onClick.AddListener(Log);

        InitScoreBoardsView();
        
    }

    void InitScoreBoardsView()
    {
        InitScoreBoard(PlayerOneScore);
        InitScoreBoard(PlayerTwoScore);
    }
    void InitScoreBoard(GameObject playerScore)
    {
        foreach (Transform child in playerScore.transform)
        {
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
    void Log()
    {

        Debug.Log("throw!");
        

    }
}
