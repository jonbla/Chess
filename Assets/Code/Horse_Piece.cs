using ExtraChessStructures;
using UnityEngine;

/// <summary>
/// Horse Chess Piece
/// </summary>
public class Horse_Piece : Custom_Mono
{
    /// <summary>
    /// Checks if this move is valid for a Horse
    /// </summary>
    /// <returns>Validity of move</returns>
    public bool IsValidHorseMove()
    {
        Vector2Int lastMove = CP.moveDelta;
        ColInfo flags = CP.CollisionInfo;

        if ((Mathf.Abs(lastMove.x) == 2 && Mathf.Abs(lastMove.y) == 1) || (Mathf.Abs(lastMove.x) == 1 && Mathf.Abs(lastMove.y) == 2))
        {
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
        return false;
    }
}
    
