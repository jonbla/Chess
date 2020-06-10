using System.Collections;
using System.Collections.Generic;
using ExtraChessStructures;
using UnityEngine;

//comp name is christian duffy

public class Main : MonoBehaviour
{
    public int moves = 0;
    GameState state;
    Team_Manager whiteTeam;
    Team_Manager blackTeam;
    List<Pawn_Piece> pawnsToUpdate = new List<Pawn_Piece>();

    // Start is called before the first frame update
    void Start()
    {
        Coord_Manager.Init();
        whiteTeam = GameObject.Find("White").GetComponent<Team_Manager>();
        blackTeam = GameObject.Find("Black").GetComponent<Team_Manager>();

        state = GameState.WhiteTurn;
        whiteTeam.hasTurn = true;

        Feedback.init();
    }    

    void ToggleTurnState()
    {
        if(state == GameState.BlackTurn)
        {
            state = GameState.WhiteTurn;
            whiteTeam.hasTurn = true;
            blackTeam.hasTurn = false;
            Feedback.SetText("White Turn");
        }
        else if(state == GameState.WhiteTurn)
        {
            state = GameState.BlackTurn;
            whiteTeam.hasTurn = false;
            blackTeam.hasTurn = true;
            Feedback.SetText("Black Turn");
        }
    }

    public void EndTurn()
    {
        moves++;
        ToggleTurnState();
        CheckFlags checkInfo = whiteTeam.hasTurn ? whiteTeam.checkInfo : blackTeam.checkInfo;
        if (checkInfo.isInCheck)
        {
            Feedback.SetText("THE KING IS UNDER CHECK!");
        }
        if (checkInfo.isInCheckmate)
        {
            Feedback.SetText("THE KINf IS DEAD!");
        }
    }
}
