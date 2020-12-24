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

    Pawn_Piece pawn;
    Horse_Piece horse;
    Rook_Piece rook;
    Bishop_Piece bishop;
    Queen_Piece queen;
    King_Piece king;

    void Start()
    {
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
                Debug.Log("Checking pawn validity");
                response = pawn.IsValidPawnMove();
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
            case 7:
                response = pawn.IsValidPawnMove(true);
                if (response == false)
                {
                    Feedback.SetText(errorMessage + transform.tag);
                }

                return response;

            default:
                Feedback.SetText("this line shouldn't be reachable");
                return false;
        }
    }
}
