using ExtraChessStructures;
using UnityEngine;

/// <summary>
/// Queen Chess Piece
/// </summary>
public class Queen_Piece : Custom_Mono
{
    /// <summary>
    /// Indicates if the Queen moved diagonally or horizontally
    /// </summary>
    [HideInInspector]
    public bool isDiagonal;

    /// <summary>
    /// Checks if this move is valid for a Queen
    /// </summary>
    /// <returns>Validity of move</returns>
    public bool IsValidQueenMove()
    {
        Vector2Int lastMove = CP.moveDelta;
        ColInfo flags = CP.CollisionInfo;

        isDiagonal = true;

        if (Mathf.Abs(lastMove.x) != Mathf.Abs(lastMove.y))
        {
            if (lastMove.x != 0 && lastMove.y != 0)
            {
                return false;
            }
            else
            {
                isDiagonal = false;
            }
        }

        Vector2Int currentCoord = Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition);

        if (isDiagonal)
        {
            for (int i = 1; i < Mathf.Abs(lastMove.y); i++)
            {
                Vector2Int checkedCoord = currentCoord;

                if (lastMove.y > 0)
                {
                    checkedCoord = new Vector2Int(checkedCoord.x, checkedCoord.y - i);
                }
                if (lastMove.y < 0)
                {
                    checkedCoord = new Vector2Int(checkedCoord.x, checkedCoord.y + i);
                }

                if (lastMove.x > 0)
                {
                    checkedCoord = new Vector2Int(checkedCoord.x - i, checkedCoord.y);
                }
                if (lastMove.x < 0)
                {
                    checkedCoord = new Vector2Int(checkedCoord.x + i, checkedCoord.y);
                }

                if (Coord_Manager.GetNameAt(checkedCoord) != null)
                {
                    return false;
                }
            }
        }
        else
        {
            if (lastMove.y != 0)
            {
                for (int i = 1; i < Mathf.Abs(lastMove.y); i++)
                {
                    Vector2Int checkedCoord;
                    if (lastMove.y > 0)
                    {
                        checkedCoord = new Vector2Int(currentCoord.x, currentCoord.y - i);
                    }
                    else
                    {
                        checkedCoord = new Vector2Int(currentCoord.x, currentCoord.y + i);
                    }
                    if (Coord_Manager.GetNameAt(checkedCoord) != null)
                    {
                        return false;
                    }
                }
            }


            if (lastMove.x != 0)
            {
                for (int i = 1; i < Mathf.Abs(lastMove.x); i++)
                {
                    Vector2Int checkedCoord;
                    if (lastMove.x > 0)
                    {
                        checkedCoord = new Vector2Int(currentCoord.x - i, currentCoord.y);
                    }
                    else
                    {
                        checkedCoord = new Vector2Int(currentCoord.x + i, currentCoord.y);
                    }
                    if (Coord_Manager.GetNameAt(checkedCoord) != null)
                    {
                        return false;
                    }
                }
            }
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
}
