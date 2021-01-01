using ExtraChessStructures;
using UnityEngine;

/// <summary>
/// King Chess Piece
/// </summary>
public class King_Piece : Custom_Mono
{

    bool canCastle = true;

    /// <summary>
    /// Checks if this move is valid for a King
    /// </summary>
    /// <returns>Validity of move</returns>
    public bool IsValidKingMove()
    {
        Vector2Int lastMove = CP.lastMove;
        ColInfo flags = CP.CollisionInfo;

        //If king moves more than 2 spaces, then invalid
        if (Mathf.Abs(lastMove.x) > 1 || Mathf.Abs(lastMove.y) > 1)
        {
            if(Mathf.Abs(lastMove.x) == 2) //check if king moved 2 squares left or right
            {
                if (canCastle) //check if king can castle
                {
                    if(totalMoves > 0) //double check if king can really castle
                    {
                        canCastle = false; //update check if can't 
                    }
                    else
                    {
                        if(lastMove.x > 0)
                        {
                            if (Coord_Manager.GetNameAt(new Vector2Int(6, GetIsBlack() ? 8 : 1)) != null)
                            {
                                Feedback.SetText("Can't Castle With Piece In The Way");
                                return false; //check for empty space for rook
                            }

                            if (Coord_Manager.GetCheckInfoAt(new Vector2Int(6, GetIsBlack() ? 8 : 1), GetIsBlack()).isInCheck)
                            {
                                Feedback.SetText("Can't Castle Through Check");
                                return false; //make sure king isn't moving through check
                            }
                        }
                        else
                        {
                            if (Coord_Manager.GetNameAt(new Vector2Int(4, GetIsBlack() ? 8 : 1)) != null
                                || Coord_Manager.GetNameAt(new Vector2Int(2, GetIsBlack() ? 8 : 1)) != null)
                            {
                                Feedback.SetText("Can't Castle With Piece In The Way");
                                return false; //check for empty piece for rook
                            }

                            if (Coord_Manager.GetCheckInfoAt(new Vector2Int(4, GetIsBlack() ? 8 : 1), GetIsBlack()).isInCheck)
                            {
                                Feedback.SetText("Can't Castle Through Check");
                                return false; //make sure king isn't moving through check
                            }
                        }

                        //Coord_Manager.GetNameAt()
                        Castle(lastMove.x > 0); //Castle and return valid move
                        return true;
                    }
                }
                Feedback.SetText("Can't Castle");
            }
            Feedback.SetText("Invalid move for King");
            return false;
        }

        if (flags.isColliding)
        {
            if (flags.isCollidingWithOwnTeam)
            {
                return false;
            }
            Kill(flags.nameOfColObject);
        }

        return true;
    }

    /// <summary>
    /// Executes Castle
    /// </summary>
    /// <param name="ShortCastle">True for short side castle, False, for long side</param>
    void Castle(bool ShortCastle)
    {
        if (GetIsBlack())
        {
            if (ShortCastle) {
                Transform rook = Coord_Manager.GetTransformObject("Black_Rook_2");
                Chess_Piece rook_CP = Coord_Manager.GetPiece("Black_Rook_2");
                Vector3 targetPos = Coord_Manager.ConvertChessUnitsToCoords(new Vector2Int(6, 8));

                rook.localPosition = targetPos;

                rook_CP.CenterPiece();
                Coord_Manager.UpdatePosition(rook.name, targetPos);
            }
            else
            {
                Transform rook = Coord_Manager.GetTransformObject("Black_Rook_1");
                Chess_Piece rook_CP = Coord_Manager.GetPiece("Black_Rook_1");
                Vector3 targetPos = Coord_Manager.ConvertChessUnitsToCoords(new Vector2Int(4, 8));

                rook.localPosition = targetPos;

                rook_CP.CenterPiece();
                Coord_Manager.UpdatePosition(rook.name, targetPos);
            }
        }
        else
        {
            if (ShortCastle)
            {
                Transform rook = Coord_Manager.GetTransformObject("White_Rook_2");
                Chess_Piece rook_CP = Coord_Manager.GetPiece("White_Rook_2");
                Vector3 targetPos = Coord_Manager.ConvertChessUnitsToCoords(new Vector2Int(6, 1));

                rook.localPosition = targetPos;

                rook_CP.CenterPiece();
                Coord_Manager.UpdatePosition(rook.name, targetPos);
            }
            else
            {
                Transform rook = Coord_Manager.GetTransformObject("White_Rook_1");
                Chess_Piece rook_CP = Coord_Manager.GetPiece("White_Rook_1");
                Vector3 targetPos = Coord_Manager.ConvertChessUnitsToCoords(new Vector2Int(4, 1));

                rook.localPosition = targetPos;

                rook_CP.CenterPiece();
                Coord_Manager.UpdatePosition(rook.name, targetPos);
            }
        }
    }
}
