using System.Collections;
using System.Collections.Generic;
using ExtraChessStructures;
using System;
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
        if (IsBeingAttacked())
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
    public bool IsBeingAttacked()
    {
        return isBeingAttackedByPawn() || isBeingAttackedByHorse() || isBeingAttackedByKing() || isBeingAttackedByRook() || isBeingAttackedByBishop();
    }

    public bool IsBeingAttacked(Vector2Int space, bool onExcept = false)
    {
        return isBeingAttackedByPawn(space, onExcept) || isBeingAttackedByHorse(space, onExcept) || isBeingAttackedByKing(space, onExcept) || isBeingAttackedByRook(space, onExcept) || isBeingAttackedByBishop(space, onExcept);
    }

    public bool IsInMate()
    {
        Vector2Int pos = Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition);
        return 
            !IsBeingAttacked(new Vector2Int(pos.x - 1, pos.y + 1))    &&
            !IsBeingAttacked(new Vector2Int(pos.x, pos.y + 1))        &&
            !IsBeingAttacked(new Vector2Int(pos.x + 1, pos.y + 1))    &&
            !IsBeingAttacked(new Vector2Int(pos.x + 1, pos.y))        &&
            !IsBeingAttacked(new Vector2Int(pos.x + 1, pos.y - 1))    &&
            !IsBeingAttacked(new Vector2Int(pos.x, pos.y - 1))        &&
            !IsBeingAttacked(new Vector2Int(pos.x - 1, pos.y + -1))   &&
            !IsBeingAttacked(new Vector2Int(pos.x - 1, pos.y));
    }

    //Check if type of object exists at positions
    bool isTypeAtCoord(Vector2Int pos, string type){

        Transform obj = Coord_Manager.GetTransformAt(pos);
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

    bool isBeingAttackedByPawn(Vector2Int units, bool onExcept = false)
    {
        try
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
        catch (IndexOutOfRangeException)
        {
            return onExcept;
        }
    }

    bool isBeingAttackedByHorse()
    {
        return isBeingAttackedByHorse(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
        
    }

    bool isBeingAttackedByHorse(Vector2Int units, bool onExcept = false)
    {
        try { 
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
        catch (IndexOutOfRangeException)
        {
            return onExcept;
        }
    }

    bool isBeingAttackedByRook()
    {
        return isBeingAttackedByRook(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

    bool isBeingAttackedByRook(Vector2Int units, bool onExcept = false)
    {
        try
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

                if (Coord_Manager.GetTransformAt(new Vector2Int(i, units.y)) != null)
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

                if (Coord_Manager.GetTransformAt(new Vector2Int(i, units.y)) != null)
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

                if (Coord_Manager.GetTransformAt(new Vector2Int(units.x, i)) != null)
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

                if (Coord_Manager.GetTransformAt(new Vector2Int(units.x, i)) != null)
                {
                    break;
                }
            }

            return false;
        }
        catch (IndexOutOfRangeException)
        {
            return onExcept;
        }
    }

    bool isBeingAttackedByBishop()
    {
        return isBeingAttackedByBishop(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

    bool isBeingAttackedByBishop(Vector2Int units, bool onExcept = false)
    {
        try
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

                if (Coord_Manager.GetTransformAt(new Vector2Int(units.x + i, units.y + i)) != null)
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

                if (Coord_Manager.GetTransformAt(new Vector2Int(units.x - i, units.y + i)) != null)
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

                if (Coord_Manager.GetTransformAt(new Vector2Int(units.x + i, units.y - i)) != null)
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

                if (Coord_Manager.GetTransformAt(new Vector2Int(units.x - i, units.y - i)) != null)
                {
                    break;
                }
            }

            return false;
        }
        catch (IndexOutOfRangeException)
        {
            return onExcept;
        }
    }

    bool isBeingAttackedByKing()
    {
        return isBeingAttackedByKing(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

    bool isBeingAttackedByKing(Vector2Int units, bool onExcept = false)
    {
        try
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
        catch (IndexOutOfRangeException)
        {
            return onExcept;
        }
    }

}
