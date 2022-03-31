using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnViewDetail
{

    public GameObject FirstThrow { get; private set; }
    public GameObject SecondThrow { get; private set; }
    public GameObject ThirdThrow { get; private set; }
    public GameObject FinalTurnScore { get; private set; }
    public TurnViewDetail(GameObject firstThrow, GameObject secondThrow, GameObject finalTurnScore)
    {
        FirstThrow=firstThrow;
        SecondThrow=secondThrow;
        FinalTurnScore=finalTurnScore;
    }

    public TurnViewDetail(GameObject firstThrow, GameObject secondThrow, GameObject thirdThrow, GameObject finalTurnScore)
    {
        FirstThrow=firstThrow;
        SecondThrow=secondThrow;
        ThirdThrow=thirdThrow;
        FinalTurnScore=finalTurnScore;
    }
    

}
