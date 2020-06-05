using System.Collections;
using System.Collections.Generic;
using ExtraChessStructures;
using UnityEngine;

public class King_Piece : Custom_Mono
{
    //Is this move valid for a king
    public bool isValidKingMove()
    {
        Vector2Int lastMove = CP.lastMove;
        ColInfo flags = CP.CollisionInfo;

        //If king moves more than 2 spaces, then invalid
        if(Mathf.Abs(lastMove.x) > 1 || Mathf.Abs(lastMove.y) > 1)
        {
            Feedback.SetText("Invalid move for King");
            return false;
        }

        //if king is being attacked, then invalid
        if (isBeingAttacked())
        {
            Feedback.SetText("King can be attacked there");
            return false;
        }

        //kill piece if is enemy piece
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

    //Check if king is being attacked
    bool isBeingAttacked()
    {
        return isBeingAttackedByPawn() || isBeingAttackedByHorse() || isBeingAttackedByKing() || isBeingAttackedByRook() || isBeingAttackedByBishop();
    }

    bool isBeingAttacked(Vector2Int space)
    {
        return isBeingAttackedByPawn(space) || isBeingAttackedByHorse(space) || isBeingAttackedByKing(space) || isBeingAttackedByRook(space) || isBeingAttackedByBishop(space);
    }

    //Check if type of object exists at positions
    bool isTypeAtCoord(Vector2Int pos, string type){

        Transform obj = Coord_Manager.GetGameObjectAt(pos);
        if (obj != null)
        {
            if (obj.tag == type && obj.transform.parent != transform.parent)
            {
                return true;
            }
        }

        return false;
    }

    //Check if king is being attacked by pawn
    bool isBeingAttackedByPawn()
    {
        return isBeingAttackedByPawn(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

    bool isBeingAttackedByPawn(Vector2Int units)
    {
        bool isBlack = GetIsBlack();

        if (isTypeAtCoord(new Vector2Int(units.x + 1, units.y + (isBlack ? -1 : 1)), "Pawn"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x + -1, units.y + (isBlack ? -1 : 1)), "Pawn"))
        {
            return true;
        }

        return false;
    }

    bool isBeingAttackedByHorse()
    {
        return isBeingAttackedByHorse(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
        
    }

    bool isBeingAttackedByHorse(Vector2Int units)
    {
        if (isTypeAtCoord(new Vector2Int(units.x + 1, units.y + 2), "Horse"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x + 1, units.y - 2), "Horse"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x - 1, units.y - 2), "Horse"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x - 1, units.y + 2), "Horse"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x + 2, units.y + 1), "Horse"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x + 2, units.y - 1), "Horse"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x - 2, units.y - 1), "Horse"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x - 2, units.y + 1), "Horse"))
        {
            return true;
        }

        return false;
    }

    bool isBeingAttackedByRook()
    {
        return isBeingAttackedByRook(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

    bool isBeingAttackedByRook(Vector2Int units)
    {

        //Debug.Log("looking right\n");
        for (int i = units.x + 1; i <= 8; i++)
        {
            if (isTypeAtCoord(new Vector2Int(i, units.y), "Rook"))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(i, units.y), "Queen"))
            {
                return true;
            }

            if (Coord_Manager.GetGameObjectAt(new Vector2Int(i, units.y)) != null)
            {
                break;
            }
        }

        //Debug.Log("looking left\n");
        for (int i = units.x - 1; i >= 0; i--)
        {
            if (isTypeAtCoord(new Vector2Int(i, units.y), "Rook"))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(i, units.y), "Queen"))
            {
                return true;
            }

            if (Coord_Manager.GetGameObjectAt(new Vector2Int(i, units.y)) != null)
            {
                break;
            }
        }

        //Debug.Log("looking up\n");
        for (int i = units.y + 1; i <= 8; i++)
        {
            if (isTypeAtCoord(new Vector2Int(units.x, i), "Rook"))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x, i), "Queen"))
            {
                return true;
            }

            if (Coord_Manager.GetGameObjectAt(new Vector2Int(units.x, i)) != null)
            {
                break;
            }
        }

        //Debug.Log("looking down\n");
        for (int i = units.y - 1; i >= 0; i--)
        {
            if (isTypeAtCoord(new Vector2Int(units.x, i), "Rook"))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x, i), "Queen"))
            {
                return true;
            }

            if (Coord_Manager.GetGameObjectAt(new Vector2Int(units.x, i)) != null)
            {
                break;
            }
        }

        return false;
    }

    bool isBeingAttackedByBishop()
    {
        return isBeingAttackedByBishop(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

    bool isBeingAttackedByBishop(Vector2Int units)
    {
        for (int i = 1; i <= 8 - Mathf.Min(units.x, units.y); i++)
        {
            if (isTypeAtCoord(new Vector2Int(units.x + i, units.y + i), "Bishop"))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x + i, units.y + i), "Queen"))
            {
                return true;
            }

            if (Coord_Manager.GetGameObjectAt(new Vector2Int(units.x + i, units.y + i)) != null)
            {
                break;
            }
        }

        for (int i = 1; i <= 8 - Mathf.Min(units.x, units.y); i++)
        {
            if (isTypeAtCoord(new Vector2Int(units.x - i, units.y + i), "Bishop"))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x - i, units.y + i), "Queen"))
            {
                return true;
            }

            if (Coord_Manager.GetGameObjectAt(new Vector2Int(units.x - i, units.y + i)) != null)
            {
                break;
            }
        }

        for (int i = 1; i <= 8 - Mathf.Min(units.x, units.y); i++)
        {
            if (isTypeAtCoord(new Vector2Int(units.x + i, units.y - i), "Bishop"))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x + i, units.y - i), "Queen"))
            {
                return true;
            }

            if (Coord_Manager.GetGameObjectAt(new Vector2Int(units.x + i, units.y - i)) != null)
            {
                break;
            }
        }

        for (int i = 1; i <= Mathf.Max(units.x, units.y); i++)
        {
            if (isTypeAtCoord(new Vector2Int(units.x - i, units.y - i), "Bishop"))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x - i, units.y - i), "Queen"))
            {
                return true;
            }

            if (Coord_Manager.GetGameObjectAt(new Vector2Int(units.x - i, units.y - i)) != null)
            {
                break;
            }
        }

        return false;
    }

    bool isBeingAttackedByKing()
    {
        return isBeingAttackedByKing(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

    bool isBeingAttackedByKing(Vector2Int units)
    {
        if (isTypeAtCoord(new Vector2Int(units.x + 1, units.y), "King"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x - 1, units.y), "King"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x, units.y + 1), "King"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x, units.y - 1), "King"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x + 1, units.y + 1), "King"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x - 1, units.y - 1), "King"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x - 1, units.y + 1), "King"))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x + 1, units.y - 1), "King"))
        {
            return true;
        }

        return false;
    }

}
