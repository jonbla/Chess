using ExtraChessStructures;
using UnityEngine;

/// <summary>
/// Class that manages team functions. There should be 2 instantiated instances
/// </summary>
public class Team_Manager : Custom_Mono
{
    /// <summary>
    /// Indicates if it is this team's turn
    /// </summary>
    public bool hasTurn = false;

    /// <summary>
    /// Mate status of this team
    /// </summary>
    CheckFlags checkFlags;

    /// <summary>
    /// Reference to this team's king
    /// </summary>
    King_Piece king;

    /// <summary>
    /// reference to main
    /// </summary>
    Main main;

    /// <summary>
    /// Gets mate status of this team
    /// </summary>
    public CheckFlags CheckInfo { get => GetCheckFlags(); }

    /// <summary>
    /// Indicates if this team is in check
    /// </summary>
    public bool IsInCheck { get => IsinCheck(); }

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

    /// <summary>
    /// Manually set check flags
    /// </summary>
    /// <param name="flags"></param>
    void SetCheckFlags(CheckFlags flags)
    {
        checkFlags = flags;
    }

}
