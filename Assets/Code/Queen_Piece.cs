﻿using System.Collections;
using System.Collections.Generic;
using ExtraChessStructures;
using UnityEngine;

public class Queen_Piece : Custom_Mono
{
    public bool isDiagonal;
    public bool isValidQueenMove()
    {
        Vector2Int lastMove = CP.lastMove;
        ColInfo flags = CP.CollisionInfo;

        isDiagonal = true;

        if (Mathf.Abs(lastMove.x) != Mathf.Abs(lastMove.y)) 
        {
            if (lastMove.x != 0 && lastMove.y != 0)
            {
                return false;
            } else
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
            KillAtLocation();
        }
        return true;
    }
}