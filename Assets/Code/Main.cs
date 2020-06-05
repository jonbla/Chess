using System.Collections;
using System.Collections.Generic;
using ExtraChessStructures;
using UnityEngine;

//comp name is christian duffy

public class Main : MonoBehaviour
{
    GameState state;
    Team_Manager whiteTeam;
    Team_Manager blackTeam;

    // Start is called before the first frame update
    void Start()
    {
        Coord_Manager.init();
        whiteTeam = GameObject.Find("White").GetComponent<Team_Manager>();
        blackTeam = GameObject.Find("Black").GetComponent<Team_Manager>();

        state = GameState.WhiteTurn;
        whiteTeam.hasTurn = true;

        Feedback.init();
    }

    public void EndTurn()
    {
        ToggleTurnState();
        if(state == GameState.WhiteTurn)
        {
            whiteTeam.hasTurn = true;
            blackTeam.hasTurn = false;
        }
        else if (state == GameState.BlackTurn)
        {
            whiteTeam.hasTurn = false;
            blackTeam.hasTurn = true;
        }
    }

    void ToggleTurnState()
    {
        if(state == GameState.BlackTurn)
        {
            state = GameState.WhiteTurn;
            Feedback.SetText("White Turn");
        }
        else if(state == GameState.WhiteTurn)
        {
            state = GameState.BlackTurn;
            Feedback.SetText("Black Turn");
        }
    }
}
