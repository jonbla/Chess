using ExtraChessStructures;
using UnityEngine;

public class Horse_Piece : Custom_Mono
{
    public bool IsValidHorseMove()
    {
        Vector2Int lastMove = CP.lastMove;
        ColInfo flags = CP.CollisionInfo;

        if ((Mathf.Abs(lastMove.x) == 2 && Mathf.Abs(lastMove.y) == 1) || (Mathf.Abs(lastMove.x) == 1 && Mathf.Abs(lastMove.y) == 2))
        {
            if (flags.isColliding)
            {
                if (flags.isCollidingWithOwnTeam)
                {
                    return false;
                }
                KillAtLocation();
            }
            return true;
        }
        return false;
    }
}
    
