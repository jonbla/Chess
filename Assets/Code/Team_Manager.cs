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
    public CheckFlags checkFlags;

    /// <summary>
    /// Reference to this team's king
    /// </summary>
    King_Piece king;

    /// <summary>
    /// Team Colour Name as bool
    /// </summary>
    public bool isBlack;

    /// <summary>
    /// Indicates if this team can Castle
    /// </summary>
    public bool canCastle;

    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("MainCode").GetComponent<Main>();
        king = transform.Find((name == "Black" ? "Black" : "White") + "_King").GetComponent<King_Piece>();
        isBlack = name[0] == 'B';
        canCastle = true;
    }


    public new void EndTurn()
    {
        main.EndTurn();
    }
}
