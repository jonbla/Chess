using System.Collections;
using System.Collections.Generic;
using ExtraChessStructures;
using UnityEngine;

public class Team_Manager : Custom_Mono
{
    public bool hasTurn = false;
    CheckFlags checkFlags;
    King_Piece king;
    Main main;

    public CheckFlags CheckInfo { get => GetCheckFlags(); }
    public bool isInCheck { get => IsinCheck(); }

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

    CheckFlags GetCheckFlags()
    {
        bool check = king.IsBeingAttacked();
        bool mate = check ? king.IsInMate() : false;

        print("check: " + check);
        print("mate: " + mate);

        CheckFlags returnVal = new CheckFlags(check, mate);
        SetCheckFlags(returnVal);
        return returnVal;
    }

    bool IsinCheck()
    {
        return king.IsBeingAttacked();
    }

    void SetCheckFlags(CheckFlags flags)
    {
        checkFlags = flags;
    }

}
