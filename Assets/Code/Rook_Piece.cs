using ExtraChessStructures;
using UnityEngine;

/// <summary>
/// Rook Chess Piece
/// </summary>
public class Rook_Piece : Custom_Mono
{
    /// <summary>
    /// Checks if this move is valid for a Rook
    /// </summary>
    /// <returns>Validity of move</returns>
    public bool IsValidRookMove()
    {
        Vector2Int lastMove = CP.lastMove;
        ColInfo flags = CP.CollisionInfo;

        if (lastMove.x != 0 && lastMove.y != 0)
        {
            return false;
        }

        Vector2Int currentCoord = Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition);

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

    public new void EndTurn()
    {
        totalMoves++;
        team.canCastle = false;
    }
}
