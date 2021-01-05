using System.Collections.Generic;
using System.Linq;
using ExtraChessStructures;
using UnityEngine;

//comp name is christian duffy

public class Main : MonoBehaviour
{
    public int moves = 0;
    GameState state;
    Team_Manager whiteTeam;
    Team_Manager blackTeam;
    FadeMaster fade;

    PassantablePawn PawnInLimbo;

    Dictionary<Pawn_Piece, int> pawnsToUpdate = new Dictionary<Pawn_Piece, int>();

    class PassantablePawn
    {
        public readonly Pawn_Piece tempPawn;
        public readonly int halfturns;

        public PassantablePawn(Pawn_Piece tempPawn, int halfturns)
        {
            this.tempPawn = tempPawn;
            this.halfturns = halfturns;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 60;
        Coord_Manager.Init();
        whiteTeam = GameObject.Find("White").GetComponent<Team_Manager>();
        blackTeam = GameObject.Find("Black").GetComponent<Team_Manager>();

        fade = GameObject.Find("Fade Master").GetComponent<FadeMaster>();

        state = GameState.WhiteTurn;
        whiteTeam.hasTurn = true;

        Feedback.Init();
    }

    void Update()
    {
        //Keep running this function while mouse is being held down
        try
        {
            if (Mouse_Manager.HeldPiece_CP.mouseIsClicked)
            {
                Mouse_Manager.MovePieceWithMouse();
            }
        }
        catch (System.Exception NullReferenceException) {}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// Toggles the turn
    /// </summary>
    void ToggleTurnState()
    {
        if (state == GameState.BlackTurn)
        {
            state = GameState.WhiteTurn;
            whiteTeam.hasTurn = true;
            blackTeam.hasTurn = false;
            Feedback.SetText("White Turn");
        }
        else if (state == GameState.WhiteTurn)
        {
            state = GameState.BlackTurn;
            whiteTeam.hasTurn = false;
            blackTeam.hasTurn = true;
            Feedback.SetText("Black Turn");
        }

        fade.ToggleFade();
    }

    public void RequestPawnToBeAddedToPassantList(Pawn_Piece pawn, int halfTurnsElidgableFor)
    {
        PawnInLimbo = new PassantablePawn(pawn, halfTurnsElidgableFor);
    }

    public void CommitPawnToPassantList()
    {
        if (PawnInLimbo == null) return;
        pawnsToUpdate.Add(PawnInLimbo.tempPawn, PawnInLimbo.halfturns);
        PawnInLimbo = null;
    }

    /// <summary>
    /// Preform end-turn functions
    /// </summary>
    public void EndTurn()
    {
        CommitPawnToPassantList();

        UpdatePassantList();

        moves++;
        ToggleTurnState();
    }

    /// <summary>
    /// Update list of pieces eligable for en-passant
    /// </summary>
    void UpdatePassantList()
    {
        Dictionary<Pawn_Piece, int> tempDict = pawnsToUpdate;
        foreach (var entry in pawnsToUpdate.ToList())
        {
            int temp = entry.Value;
            temp--;
            if (temp <= 0)
            {
                entry.Key.canBePassanted = false;
            }
            else
            {
                tempDict.Remove(entry.Key);
                tempDict.Add(entry.Key, temp);
            }
        }
        pawnsToUpdate = tempDict;
    }
}
