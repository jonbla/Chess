using System.Collections.Generic;
using System.Linq;
using ExtraChessStructures;
using UnityEngine;

//comp name is christian duffy

public class Main : MonoBehaviour
{
    /// <summary>
    /// Total Half moves in the game
    /// </summary>
    public int halfMoves;

    /// <summary>
    /// Debug bool, indicating if 
    /// </summary>
    public bool toggle;

    /// <summary>
    /// Is White in Check
    /// </summary>
    public bool whiteInCheck;

    /// <summary>
    /// Is Black in Check
    /// </summary>
    public bool blackInCheck;

    /// <summary>
    /// State of the game
    /// </summary>
    GameState state;

    /// <summary>
    /// Reference to White team
    /// </summary>
    Team_Manager whiteTeam;

    /// <summary>
    /// Reference to black team
    /// </summary>
    Team_Manager blackTeam;

    /// <summary>
    /// Reference to fader
    /// </summary>
    FadeMaster fade;

    /// <summary>
    /// Temp pawn that isn't commited yet
    /// </summary>
    PassantablePawn PawnInLimbo;

    /// <summary>
    /// pawns whos passant status needs to be updated
    /// </summary>
    Dictionary<Pawn_Piece, int> pawnsToUpdate;

    Menu pauseMenu;

    public bool inPause { get => pauseMenu.inPause; }

    /// <summary>
    /// pawn structure to be commited into dictionary
    /// </summary>
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
        halfMoves = 0;

        Coord_Manager.Init();
        whiteTeam = GameObject.Find("White").GetComponent<Team_Manager>();
        blackTeam = GameObject.Find("Black").GetComponent<Team_Manager>();

        fade = GameObject.Find("Fade Master").GetComponent<FadeMaster>();
        pauseMenu = GameObject.Find("ExtraCode").GetComponentInChildren<Menu>();

        state = GameState.WhiteTurn;
        whiteTeam.hasTurn = true;

        pawnsToUpdate = new Dictionary<Pawn_Piece, int>();

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
        catch (System.Exception NullReferenceException) { }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!inPause)
            {
                pauseMenu.EnterPauseMenu();
            }
            else
            {
                pauseMenu.ExitPauseMenu();
            }
        }
    }

    /// <summary>
    /// Toggles the turn
    /// </summary>
    void ToggleTurnState()
    {
        if (toggle == false) return;
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

    /// <summary>
    /// Request pawn to be added to list, wait for conformation
    /// </summary>
    /// <param name="pawn">Pawn</param>
    /// <param name="halfTurnsElidgableFor">how many half turns will this pawn be passanted for</param>
    public void RequestPawnToBeAddedToPassantList(Pawn_Piece pawn, int halfTurnsElidgableFor)
    {
        PawnInLimbo = new PassantablePawn(pawn, halfTurnsElidgableFor);
    }

    /// <summary>
    /// Confirm pawn is added to list
    /// </summary>
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

        halfMoves++;
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

    public void Reset()
    {
        Coord_Manager.Reset();
        Start();
    }
}
