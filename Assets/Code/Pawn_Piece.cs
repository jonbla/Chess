using ExtraChessStructures;
using UnityEngine;

/// <summary>
/// Pawn Chess Piece
/// </summary>
public class Pawn_Piece : Custom_Mono
{
    /// <summary>
    /// Flag indicating if an en passant can be preformed on this piece
    /// </summary>
    public bool canBePassanted;

    /// <summary>
    /// Flag indicating if this piece has completed its second move
    /// </summary>
    //[HideInInspector]
    //public bool secondMoveDone = false;

    /// <summary>
    /// Flag indicating if this piece has completed its second move
    /// </summary>
    //[HideInInspector]
    //bool firstMoveDone = false;

    private void Awake()
    {
        main = GameObject.Find("MainCode").GetComponent<Main>();
        canBePassanted = false;
    }

    /// <summary>
    /// Checks if this move is valid for a bishop
    /// </summary>
    /// <returns>Validity of move</returns>
    public bool IsValidPawnMove(bool isBlack = false)
    {
        Vector2Int lastMove = CP.moveDelta;
        ColInfo flags = CP.CollisionInfo;

        if (isBlack)
        {
            lastMove = new Vector2Int(lastMove.x, lastMove.y * -1);
        }

        if (lastMove.x != 0) //if there was a horizontal move
        {
            if (Mathf.Abs(lastMove.x) == 1 && lastMove.y == 1) //was it one of (1,1) or (-1,1)?
            {
                if (!flags.isCollidingWithKing) //make sure you didn't try to kill a king
                {
                    if (flags.isColliding) //make sure you actually are colliding with something
                    {
                        Kill(flags.nameOfColObject);
                        return true; //if all these were true, then the move is valid
                    }
                }
                if (IsEnPassant())
                {
                    return true;
                }
            }
            return false; //if there was a horizontal move and any of these conditions are false, then this move was invalid
        }

        if (lastMove.y >= 2 || lastMove.y < 0) //if you tried to move more than 1 space forward
        {
            if (lastMove.y == 2 && totalMoves == 0) //is this your first move?
            {
                Vector2Int currentCoord = Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition);
                Debug.Log(currentCoord.y - (isBlack ? -1 : 1));
                if (Coord_Manager.GetNameAt(new Vector2Int(currentCoord.x, currentCoord.y - (isBlack ? -1 : 1))) == null)
                {
                    main.RequestPawnToBeAddedToPassantList(this, 2);
                    return true; //if so, then this move is valid
                }
            }
            return false; //otherwise it isn't
        }

        if (lastMove.y == 1 && flags.isColliding) //can't move forward when there is a piece infront of you as a pawn
        {
            return false;
        }

        return true; //all other moves are fine
    }
}
