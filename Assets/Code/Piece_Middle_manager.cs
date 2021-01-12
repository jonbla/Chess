using System.Collections.Generic;
using ExtraChessStructures;
using DebuggingEssentials;
using UnityEngine;

/// <summary>
/// Class responsible for validating piece specific moves
/// </summary>
public class Piece_Middle_manager : MonoBehaviour
{
    //1  = White Pawn
    //2  = White Rook
    //3  = White Bishop
    //4  = White Horse
    //5  = White Queen
    //6  = White King

    //7  = Black Pawn
    //8  = Black Rook
    //9  = Black Bishop
    //10 = Black Horse
    //11 = Black Queen
    //12 = Black King

    /// <summary>
    /// Numerical representation of pieces
    /// </summary>
    int PieceTypeID;

    /// <summary>
    /// error message base
    /// </summary>
    const string errorMessage = "Invalid move for ";

    const string whiteKingName = "White_King";
    const string blackKingName = "Black_King";

    Chess_Piece piece;

    Pawn_Piece pawn;
    Horse_Piece horse;
    Rook_Piece rook;
    Bishop_Piece bishop;
    Queen_Piece queen;
    King_Piece king;

    List<Vector2Int> PawnDeltas_W = new List<Vector2Int> { new Vector2Int(0, 1),
                                                           new Vector2Int(0, 2),
                                                           new Vector2Int(1, 1),
                                                           new Vector2Int(-1, 1)};
    List<Vector2Int> PawnDeltas_B = new List<Vector2Int> { new Vector2Int(0, -1),
                                                           new Vector2Int(0, -2),
                                                           new Vector2Int(1, -1),
                                                           new Vector2Int(-1, -1)};
    List<Vector2Int> HorseDeltas = new List<Vector2Int> {  new Vector2Int(1, 2),
                                                           new Vector2Int(1, -2),
                                                           new Vector2Int(-1, 2),
                                                           new Vector2Int(-1, -2),
                                                           new Vector2Int(2, 1),
                                                           new Vector2Int(2, -1),
                                                           new Vector2Int(-2, 1),
                                                           new Vector2Int(-2, -1)};
    List<Vector2Int> KingDeltas = new List<Vector2Int> {   new Vector2Int(-1, 1),
                                                           new Vector2Int(0, 1),
                                                           new Vector2Int(1, 1),
                                                           new Vector2Int(1, 0),
                                                           new Vector2Int(1, -1),
                                                           new Vector2Int(0, -1),
                                                           new Vector2Int(-1, -1),
                                                           new Vector2Int(-1, 0),
                                                           new Vector2Int(2, 0),
                                                           new Vector2Int(-2, 0)};
    List<Vector2Int> RookDeltas = new List<Vector2Int> {   new Vector2Int(1, 0),
                                                           new Vector2Int(2, 0),
                                                           new Vector2Int(3, 0),
                                                           new Vector2Int(4, 0),
                                                           new Vector2Int(5, 0),
                                                           new Vector2Int(6, 0),
                                                           new Vector2Int(7, 0),
                                                           new Vector2Int(8, 0),
                                                           new Vector2Int(-1, 0),
                                                           new Vector2Int(-2, 0),
                                                           new Vector2Int(-3, 0),
                                                           new Vector2Int(-4, 0),
                                                           new Vector2Int(-5, 0),
                                                           new Vector2Int(-6, 0),
                                                           new Vector2Int(-7, 0),
                                                           new Vector2Int(-8, 0),
                                                           new Vector2Int(0, 1),
                                                           new Vector2Int(0, 2),
                                                           new Vector2Int(0, 3),
                                                           new Vector2Int(0, 4),
                                                           new Vector2Int(0, 5),
                                                           new Vector2Int(0, 6),
                                                           new Vector2Int(0, 7),
                                                           new Vector2Int(0, 8),
                                                           new Vector2Int(0, -1),
                                                           new Vector2Int(0, -2),
                                                           new Vector2Int(0, -3),
                                                           new Vector2Int(0, -4),
                                                           new Vector2Int(0, -5),
                                                           new Vector2Int(0, -6),
                                                           new Vector2Int(0, -7),
                                                           new Vector2Int(0, -8)};
    List<Vector2Int> BishopDeltas = new List<Vector2Int> { new Vector2Int(1, 1),
                                                           new Vector2Int(2, 2),
                                                           new Vector2Int(3, 3),
                                                           new Vector2Int(4, 4),
                                                           new Vector2Int(5, 5),
                                                           new Vector2Int(6, 6),
                                                           new Vector2Int(7, 7),
                                                           new Vector2Int(8, 8),
                                                           new Vector2Int(-1, -1),
                                                           new Vector2Int(-2, -2),
                                                           new Vector2Int(-3, -3),
                                                           new Vector2Int(-4, -4),
                                                           new Vector2Int(-5, -5),
                                                           new Vector2Int(-6, -6),
                                                           new Vector2Int(-7, -7),
                                                           new Vector2Int(-8, -8),
                                                           new Vector2Int(-1, 1),
                                                           new Vector2Int(-2, 2),
                                                           new Vector2Int(-3, 3),
                                                           new Vector2Int(-4, 4),
                                                           new Vector2Int(-5, 5),
                                                           new Vector2Int(-6, 6),
                                                           new Vector2Int(-7, 7),
                                                           new Vector2Int(-8, 8),
                                                           new Vector2Int(1, -1),
                                                           new Vector2Int(2, -2),
                                                           new Vector2Int(3, -3),
                                                           new Vector2Int(4, -4),
                                                           new Vector2Int(5, -5),
                                                           new Vector2Int(6, -6),
                                                           new Vector2Int(7, -7),
                                                           new Vector2Int(8, -8)};


    void Start()
    {
        piece = GetComponent<Chess_Piece>();

        //determine piece type
        switch (transform.name[6])
        {
            case 'P':
                PieceTypeID = 1;
                pawn = transform.GetComponent<Pawn_Piece>();
                break;

            case 'R':
                PieceTypeID = 2;
                rook = transform.GetComponent<Rook_Piece>();
                break;

            case 'B':
                PieceTypeID = 3;
                bishop = transform.GetComponent<Bishop_Piece>();
                break;

            case 'H':
                PieceTypeID = 4;
                horse = transform.GetComponent<Horse_Piece>();
                break;

            case 'Q':
                PieceTypeID = 5;
                queen = transform.GetComponent<Queen_Piece>();
                break;

            case 'K':
                PieceTypeID = 6;
                king = transform.GetComponent<King_Piece>();
                break;

            default:
                break;
        }

        if (transform.parent.name == "Black")
        {
            PieceTypeID += 6;
        }
    }

    /// <summary>
    /// Checks if move valid for the appropriate piece
    /// </summary>
    /// <returns>Validity</returns>
    public bool IsPieceSpecificMoveValid()
    {
        bool response;
        switch (PieceTypeID)
        {
            case 1:
                response = pawn.IsValidPawnMove();
                if (response == false)
                {
                    Feedback.SetText(errorMessage + transform.tag);
                }

                return response;

            case 7:
                response = pawn.IsValidPawnMove(true);
                if (response == false)
                {
                    Feedback.SetText(errorMessage + transform.tag);
                }

                return response;

            case 2:
            case 8:
                response = rook.IsValidRookMove();
                if (response == false)
                {
                    Feedback.SetText(errorMessage + transform.tag);
                }

                return response;

            case 3:
            case 9:
                response = bishop.IsValidBishopMove();
                if (response == false)
                {
                    Feedback.SetText(errorMessage + transform.tag);
                }

                return response;

            case 4:
            case 10:
                response = horse.IsValidHorseMove();
                if (response == false)
                {
                    Feedback.SetText(errorMessage + transform.tag);
                }

                return response;

            case 5:
            case 11:
                response = queen.IsValidQueenMove();
                if (response == false)
                {
                    Feedback.SetText(errorMessage + transform.tag);
                }

                return response;

            case 6:
            case 12:
                return king.IsValidKingMove();

            default:
                Feedback.SetText("this line shouldn't be reachable");
                return false;
        }
    }

    public void EndTurn()
    {
        switch (PieceTypeID)
        {
            case 1:
            case 7:
                pawn.EndTurn();
                break;

            case 2:
            case 8:
                rook.EndTurn();
                break;

            case 3:
            case 9:
                bishop.EndTurn();
                break;

            case 4:
            case 10:
                horse.EndTurn();
                break;

            case 5:
            case 11:
                queen.EndTurn();
                break;

            case 6:
            case 12:
                king.EndTurn();
                break;
        }
    }

    public List<Move> GetValidMoves()
    {
        List<Move> validMoves = new List<Move>();
        List<Move> moves = new List<Move>();

        //Debug.LogWarning(name);

        switch (PieceTypeID)
        {
            case 1:
                foreach (Vector2Int delta in PawnDeltas_W)
                {
                    moves.Add(new Move(name, tag, delta, piece.CurrentChessCoord + delta));
                }
                break;

            case 7:
                foreach (Vector2Int delta in PawnDeltas_B)
                {
                    moves.Add(new Move(name, tag, delta, piece.CurrentChessCoord + delta));
                }
                break;

            case 2:
            case 8:
                foreach (Vector2Int delta in RookDeltas)
                {
                    moves.Add(new Move(name, tag, delta, piece.CurrentChessCoord + delta));
                }
                break;

            case 3:
            case 9:
                foreach (Vector2Int delta in BishopDeltas)
                {
                    moves.Add(new Move(name, tag, delta, piece.CurrentChessCoord + delta));
                }
                break;

            case 4:
            case 10:
                foreach (Vector2Int delta in HorseDeltas)
                {
                    moves.Add(new Move(name, tag, delta, piece.CurrentChessCoord + delta));
                }
                break;

            case 5:
            case 11:
                foreach (Vector2Int delta in RookDeltas)
                {
                    moves.Add(new Move(name, tag, delta, piece.CurrentChessCoord + delta));
                }
                foreach (Vector2Int delta in BishopDeltas)
                {
                    moves.Add(new Move(name, tag, delta, piece.CurrentChessCoord + delta));
                }
                break;

            case 6:
            case 12:
                foreach (Vector2Int delta in KingDeltas)
                {
                    moves.Add(new Move(name, tag, delta, piece.CurrentChessCoord + delta));
                }
                break;

        }

        string temp = Feedback.GetText();

        foreach (Move move in moves)
        {
            try
            {
                Debug.Log(move.name);
                if (piece.IsValidMove(move))
                {
                    Debug.Log(move.name + " accepted");
                    validMoves.Add(move);
                }
                else
                {
                    Debug.Log(move.name + " rejected");
                }
                Coord_Manager.RevertMove(ref piece.moveDelta);
            } catch
            {
                Coord_Manager.RevertMove(ref piece.moveDelta);
            }
        } 

        Feedback.SetText(temp);

        return validMoves;
    }

    /// <summary>
    /// Gets location of king in board units
    /// </summary>
    /// <param name="Black">Black King or White King</param>
    /// <param name="main">Main Board or Temp Board</param>
    /// <returns>Board units of selected king</returns>
    public Vector2Int GetKingPosition(bool Black, bool main = false)
    {
        string kingName = Black ? blackKingName : whiteKingName;
        print(Black + " : " + kingName);
        return Coord_Manager.GetCoordPosition(kingName, main);
    }
}
