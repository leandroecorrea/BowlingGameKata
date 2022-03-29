using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponentInChildren<Button>().onClick.AddListener(Log);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void Log()
    {
        Transform player1Scores = transform.Find("BoardBackground").transform.Find("PlayerScores (1)");        
        
        foreach (Transform child in player1Scores.transform)
        {
            if (child.name.Contains("TurnScoreContainer"))
            {
                Debug.Log(child.name);
            }
        }

    }
}
