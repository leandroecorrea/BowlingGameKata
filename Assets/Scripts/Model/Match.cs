using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match
{
    private Turn[] turns;
    public Turn[] Turns { get { return turns; } }
    public Match()
    {
        turns = new Turn[10];
    }

}
