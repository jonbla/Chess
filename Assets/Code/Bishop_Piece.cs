using ExtraChessStructures;
using UnityEngine;

/// <summary>
/// Bishop Chess Piece
/// </summary>
public class Bishop_Piece : Custom_Mono
{

    /// <summary>
    /// Checks if this move is valid for a bishop
    /// </summary>
    /// <returns>Validity of move</returns>
    public bool IsValidBishopMove()
    {
        Vector2Int lastMove = CP.moveDelta;
        ColInfo flags = CP.CollisionInfo;

        if(Mathf.Abs(lastMove.x) != Mathf.Abs(lastMove.y))
        {
            return false;
        }

        Vector2Int currentCoord = Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition);

        
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
