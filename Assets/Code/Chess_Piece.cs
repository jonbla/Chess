using System.Collections.Generic;
using ExtraChessStructures;
using UnityEngine;

/// <summary>
/// Universal Chess Piece
/// </summary>
public class Chess_Piece : MonoBehaviour
{
    /// <summary>
    /// Handles piece specific functions
    /// </summary>
    Piece_Middle_manager middleMan;

    /// <summary>
    /// Used for moving the piece while mouse is being held down
    /// </summary>
    public bool mouseIsClicked = false;

    /// <summary>
    /// Struct describing collition status
    /// </summary>
    public ColInfo CollisionInfo;

    /// <summary>
    /// Check flags
    /// </summary>
    CheckFlags CF;

    /// <summary>
    /// Used for determining offset
    /// </summary>
    Vector3 startPos;

    /// <summary>
    /// Last move in chessboard units
    /// </summary>
    public Vector2Int lastMove;

    /// <summary>
    /// Flag showing if this piece is in play or dead
    /// </summary>
    public bool isDead = false;

    /// <summary>
    /// Reference to TeamManager class
    /// </summary>
    public Team_Manager team;

    /// <summary>
    /// Reference to main
    /// </summary>
    Main main;

    /// <summary>
    /// The space the piece is currently on
    /// </summary>
    public Vector2Int CurrentChessCoord;



    // Start is called before the first frame update
    void Start()
    {
        middleMan = transform.GetComponent<Piece_Middle_manager>();
        team = transform.parent.GetComponent<Team_Manager>();
        main = GameObject.Find("MainCode").GetComponent<Main>();

        //On start, make sure all the pieces are center
        CenterPiece();

        CurrentChessCoord = Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition);
    }

    /// <summary>
    /// Returns the team colour of piece
    /// </summary>
    /// <returns>True if black, False if White</returns>
    public bool GetIsBlack()
    {
        return transform.parent.name == "Black";
    }


    /// <summary>
    /// Locate the nearest cell and centre the piece into it
    /// </summary>
    public void CenterPiece()
    {
        Grid grid = transform.parent.parent.parent.GetComponent<Grid>();
        Vector3Int cellPosition = grid.LocalToCell(transform.localPosition);
        transform.localPosition = grid.GetCellCenterLocal(cellPosition);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1);
    }

    /// <summary>
    /// Checks if piece is in playable area
    /// </summary>
    /// <returns>True if on board, False otherwise</returns>
    bool IsInBounds(Move move)
    {
        bool inBoundsCondition1 = false;
        bool inBoundsCondition2 = false;
        if (Mathf.Abs(move.finalPosRaw.x) <= 4.1 && Mathf.Abs(move.finalPosRaw.y) <= 4.1)
        {
            inBoundsCondition1 = true;
        }

        if(move.finalPos.x <= 8 && move.finalPos.y >= 0 && move.finalPos.y <= 8 && move.finalPos.y >= 0)
        {
            inBoundsCondition2 = true;
        }

        return inBoundsCondition1 && inBoundsCondition2;
    }

    /// <summary>
    /// Checks if the last move was valid
    /// </summary>
    /// <returns>Validity of last move</returns>
    public bool IsValidMove(Move move)
    {

        //is the piece on the board?
        if (!IsInBounds(move))
        {
            Feedback.SetText("Out of Bounds");
            return false;
        }

        //is it your turn to go?
        if (!team.hasTurn)
        {
            Feedback.SetText("Not your turn");
            return false;
        }

        lastMove = move.newMoveOffset;

        Debug.Log(lastMove);

        if (lastMove == Vector2Int.zero)
        {
            Feedback.SetText("Oops, dropped your piece");
            return false;
        }

        CollisionInfo = Coord_Manager.CheckCollitionAt(move.finalPos, GetIsBlack() ? "Black" : "White");

        bool isCollidingWithOwnTeam = CollisionInfo.isCollidingWithOwnTeam;

        //is the piece trying to kill it's own team?
        if (isCollidingWithOwnTeam)
        {
            Feedback.SetText("Can't kill your own piece");
            return false;
        }

        //is the piece making a move that particular piece is able to do?
        if (!middleMan.IsPieceSpecificMoveValid())
        {
            return false;
        }


        CF = Coord_Manager.GetCheckInfoAt(middleMan.GetKingPosition(team.isBlack), team.isBlack);
        if (CF.isInCheck)
        {
            Feedback.SetText("King In Check");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Function called by other pieces when this piece is being killed
    /// </summary>
    public void GetKilled()
    {
        Coord_Manager.KillPiece(transform.name);
        isDead = true;

        if (CompareTag("Rook"))
        {
            print(tag);
            if (transform.localPosition.x > 0)
            {
                team.canCastleShort = false;
            }
            else
            {
                team.canCastleLong = false;
            }
        }

        transform.localPosition = team.isBlack ? Death_Manager.GetBlackCoord() : Death_Manager.GetWhiteCoord();

    }

    /// <summary>
    /// End piece move, calculate move made and execute it
    /// </summary>
    void DropPiece()
    {
        Debug.LogWarning("timestart: " + Time.time);
        mouseIsClicked = false;
        Mouse_Manager.ResetMouseDelta();
        CenterPiece();
        Coord_Manager.UpdatePosition(transform.name, transform.localPosition);

        Move move = new Move(name, tag, Coord_Manager.GetPositionDifference(), transform.localPosition);

        if (!IsValidMove(move))
        {
            transform.position = startPos;
            Coord_Manager.RevertMove();
        }
        else
        {
            if (team.isBlack)
            {
                main.blackInCheck = false;
            } else
            {
                main.whiteInCheck = false;
            }

            main.KillPieceMarkedForDeath();

            Coord_Manager.CommitPositionUpdate();
            CurrentChessCoord = Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition);

            middleMan.EndTurn();
            team.checkFlags = CF;
            team.EndTurn();

            CF = Coord_Manager.GetCheckInfoAt(middleMan.GetKingPosition(!team.isBlack), !team.isBlack);
            if (CF.isInCheck)
            {
                Feedback.SetText("CHECK!");

                if (team.isBlack)
                {
                    main.whiteInCheck = true;
                }
                else
                {
                    main.blackInCheck = true;
                }
            }
        }
        Debug.LogWarning("timeend: " + Time.time);
    }

    /// <summary>
    /// Event called when mouse is down
    /// </summary>
    private void OnMouseDown()
    {
        Mouse_Manager.HeldPiece_Transform = this.transform;
        Mouse_Manager.HeldPiece_CP = this;

        startPos = transform.position;
        Mouse_Manager.MovePieceWithMouse();
    }

    /// <summary>
    /// Event called when mouse is let go
    /// </summary>
    private void OnMouseUp()
    {
        DropPiece();
    }

    public List<Move> GenerateAllValidMoves()
    {
        List<Move> moves = new List<Move>();

        moves = middleMan.GetValidMoves();

        return moves;
    }
}
