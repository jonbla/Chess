using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team_Manager : Custom_Mono
{
    public bool hasTurn = false;
    public bool isUnderCheck = false;
    King_Piece king;
    Main main;

    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("MainCode").GetComponent<Main>();
        king = transform.Find((name == "Black" ? "Black" : "White") + "_King").GetComponent<King_Piece>();
    }

    public void EndTurn()
    {
        main.EndTurn();
    }


}
