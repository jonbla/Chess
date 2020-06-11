﻿using System.Collections;
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
        bool returnVal = true;
        bool[,] emptySpaces = new bool[3,3];

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if(x == 1 && y == 1)
                {
                    emptySpaces[x, y] = false;
                    continue;
                }

                bool temp = false;
                Transform lookingAt = Coord_Manager.GetTransformAt(new Vector2Int(pos.x + x - 1, pos.y + y - 1));

                if (lookingAt.name == "Empty")
                {
                    temp = true;
                }
                emptySpaces[x, y] = temp;
            }
        }
        

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                print("x: " + x + "y: " +y+ "  " + emptySpaces[x, y]);
                if (x == 1 && y == 1) continue;
                if (emptySpaces[x, y])
                {
                    returnVal = returnVal && IsBeingAttacked(new Vector2Int(pos.x + x - 1, pos.y + y - 1));
                    if (!returnVal) return false;
                }
            }
        }
        return returnVal;
    }

    //Check if type of object exists at positions
    bool isTypeAtCoord(Vector2Int pos, string type, bool onExcept = false){

        Transform obj = Coord_Manager.GetTransformAt(pos);
        if (obj.name != "Empty" || obj == null)
        {
            if (obj.parent != transform.parent)
            {
                if (obj.tag == type)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            return onExcept;
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

            if (isTypeAtCoord(new Vector2Int(units.x + 1, units.y + (isBlack ? -1 : 1)), "Pawn", onExcept))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x + -1, units.y + (isBlack ? -1 : 1)), "Pawn", onExcept))
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
        if (isTypeAtCoord(new Vector2Int(units.x + 1, units.y + 2), "Horse", onExcept))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x + 1, units.y - 2), "Horse", onExcept))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x - 1, units.y - 2), "Horse", onExcept))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x - 1, units.y + 2), "Horse", onExcept))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x + 2, units.y + 1), "Horse", onExcept))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x + 2, units.y - 1), "Horse", onExcept))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x - 2, units.y - 1), "Horse", onExcept))
        {
            return true;
        }

        if (isTypeAtCoord(new Vector2Int(units.x - 2, units.y + 1), "Horse", onExcept))
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
                if (isTypeAtCoord(new Vector2Int(i, units.y), "Rook", onExcept))
                {
                    return true;
                }

                if (isTypeAtCoord(new Vector2Int(i, units.y), "Queen", onExcept))
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
                if (isTypeAtCoord(new Vector2Int(i, units.y), "Rook", onExcept))
                {
                    return true;
                }

                if (isTypeAtCoord(new Vector2Int(i, units.y), "Queen", onExcept))
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
                if (isTypeAtCoord(new Vector2Int(units.x, i), "Rook", onExcept))
                {
                    return true;
                }

                if (isTypeAtCoord(new Vector2Int(units.x, i), "Queen", onExcept))
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
                if (isTypeAtCoord(new Vector2Int(units.x, i), "Rook", onExcept))
                {
                    return true;
                }

                if (isTypeAtCoord(new Vector2Int(units.x, i), "Queen", onExcept))
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
                if (isTypeAtCoord(new Vector2Int(units.x + i, units.y + i), "Bishop", onExcept))
                {
                    return true;
                }

                if (isTypeAtCoord(new Vector2Int(units.x + i, units.y + i), "Queen", onExcept))
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
                if (isTypeAtCoord(new Vector2Int(units.x - i, units.y + i), "Bishop", onExcept))
                {
                    return true;
                }

                if (isTypeAtCoord(new Vector2Int(units.x - i, units.y + i), "Queen", onExcept))
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
                if (isTypeAtCoord(new Vector2Int(units.x + i, units.y - i), "Bishop", onExcept))
                {
                    return true;
                }

                if (isTypeAtCoord(new Vector2Int(units.x + i, units.y - i), "Queen", onExcept))
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
                if (isTypeAtCoord(new Vector2Int(units.x - i, units.y - i), "Bishop", onExcept))
                {
                    return true;
                }

                if (isTypeAtCoord(new Vector2Int(units.x - i, units.y - i), "Queen", onExcept))
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
            if (isTypeAtCoord(new Vector2Int(units.x + 1, units.y), "King", onExcept))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x - 1, units.y), "King", onExcept))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x, units.y + 1), "King", onExcept))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x, units.y - 1), "King", onExcept))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x + 1, units.y + 1), "King", onExcept))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x - 1, units.y - 1), "King", onExcept))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x - 1, units.y + 1), "King", onExcept))
            {
                return true;
            }

            if (isTypeAtCoord(new Vector2Int(units.x + 1, units.y - 1), "King", onExcept))
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
