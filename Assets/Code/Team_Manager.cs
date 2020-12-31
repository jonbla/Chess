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
    /// reference to main
    /// </summary>
    Main main;

    /// <summary>
    /// Team Colour Name as String
    /// </summary>
    public string colour;

    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("MainCode").GetComponent<Main>();
        king = transform.Find((name == "Black" ? "Black" : "White") + "_King").GetComponent<King_Piece>();
        colour = name[0] == 'B' ? "Black" : "White";
    }

    public new void EndTurn()
    {
        main.EndTurn();
    }
}
