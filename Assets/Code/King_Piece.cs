using ExtraChessStructures;
using UnityEngine;

/// <summary>
/// King Chess Piece
/// </summary>
public class King_Piece : Custom_Mono
{
    /// <summary>
    /// Checks if this move is valid for a King
    /// </summary>
    /// <returns>Validity of move</returns>
    public bool IsValidKingMove()
    {
        Vector2Int lastMove = CP.lastMove;
        ColInfo flags = CP.CollisionInfo;

        //If king moves more than 2 spaces, then invalid
        if (Mathf.Abs(lastMove.x) > 1 || Mathf.Abs(lastMove.y) > 1)
        {
            Feedback.SetText("Invalid move for King");
            return false;
        }

        //if king is being attacked, then invalid
        /*if (IsBeingAttacked())
        {
            Feedback.SetText("King can be attacked there");
            return false;
        }*/

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

    /// <summary>
    /// Check if the king is being attacked
    /// </summary>
    /// <returns>True if being attacked, False if it is not</returns>
    public bool IsBeingAttacked()
    {
        return IsBeingAttackedByPawn() || IsBeingAttackedByHorse() || IsBeingAttackedByRook() || IsBeingAttackedByBishop() || IsBeingAttackedByKing();
    }

    /// <summary>
    /// Check if input location is being attacked
    /// </summary>
    /// <param name="space">Target location</param>
    /// <returns>True if being attacked, False if it is not</returns>
    public bool IsBeingAttacked(Vector2Int space)
    {
        return IsBeingAttackedByPawn(space) || IsBeingAttackedByHorse(space) || IsBeingAttackedByRook(space) || IsBeingAttackedByBishop(space) || IsBeingAttackedByKing(space);
    }

    /// <summary>
    /// Check if the king is in mate
    /// </summary>
    /// <returns>Mate status of king</returns>
    public bool IsInMate()
    {
        Vector2Int pos = Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition);
        bool returnVal = true;
        bool[,] emptySpaces = new bool[3, 3];

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (x == 1 && y == 1)
                {
                    emptySpaces[x, y] = false;
                    continue;
                }

                bool temp = false;
                Transform lookingAt = Coord_Manager.GetTransformAt(new Vector2Int(pos.x + x - 1, pos.y + y - 1));

                if (lookingAt != null && lookingAt.name == "Empty")
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
                print("x: " + x + "y: " + y + "  " + emptySpaces[x, y]);
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

    /// <summary>
    /// Check if Object type exists at coord
    /// </summary>
    /// <param name="pos">Target position</param>
    /// <param name="type">Target type</param>
    /// <returns>True if type found at location, false otherwise</returns>
    bool IsTypeAtCoord(Vector2Int pos, string type)
    {

        Transform obj = Coord_Manager.GetTransformAt(pos);
        if (obj != null && obj.name != "Empty")
        {
            if (obj.parent != transform.parent)
            {
                return obj.tag == type;
            }
        }
        return false;

    }

    /// <summary>
    /// Check if king is being attacked by pawn
    /// </summary>
    /// <returns>True if being attacked, False if it is not</returns>
    bool IsBeingAttackedByPawn()
    {
        return IsBeingAttackedByPawn(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

    /// <summary>
    /// Check if king is being attacked by pawn at Target
    /// </summary>
    /// <param name="units">Target</param>
    /// <returns>True if being attacked, False if it is not</returns>
    bool IsBeingAttackedByPawn(Vector2Int units)
    {
        bool isBlack = GetIsBlack();

        if (IsTypeAtCoord(new Vector2Int(units.x + 1, units.y + (isBlack ? -1 : 1)), "Pawn"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x + -1, units.y + (isBlack ? -1 : 1)), "Pawn"))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Check if king is being attacked by Horse
    /// </summary>
    /// <returns>True if being attacked, False if it is not</returns>
    bool IsBeingAttackedByHorse()
    {
        return IsBeingAttackedByHorse(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));

    }

    /// <summary>
    /// Check if king is being attacked by Horse at Target
    /// </summary>
    /// <param name="units">Target</param>
    /// <returns>True if being attacked, False if it is not</returns>
    bool IsBeingAttackedByHorse(Vector2Int units, bool onExcept = false)
    {
        if (IsTypeAtCoord(new Vector2Int(units.x + 1, units.y + 2), "Horse"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x + 1, units.y - 2), "Horse"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x - 1, units.y - 2), "Horse"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x - 1, units.y + 2), "Horse"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x + 2, units.y + 1), "Horse"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x + 2, units.y - 1), "Horse"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x - 2, units.y - 1), "Horse"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x - 2, units.y + 1), "Horse"))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Check if king is being attacked by Rook
    /// </summary>
    /// <returns>True if being attacked, False if it is not</returns>
    bool IsBeingAttackedByRook()
    {
        return IsBeingAttackedByRook(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

    /// <summary>
    /// Check if king is being attacked by Rook at Target
    /// </summary>
    /// <param name="units">Target</param>
    /// <returns>True if being attacked, False if it is not</returns>
    bool IsBeingAttackedByRook(Vector2Int units)
    {
        //Debug.Log("looking right\n");
        for (int i = units.x + 1; i <= 8; i++)
        {
            Vector2Int lookingAt = new Vector2Int(i, units.y);

            int stepVal = RookStep(lookingAt);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    goto StraightLoop2;
                default:
                    print("This code should be impossible");
                    break;
            }
        }

    //Debug.Log("looking left\n");
    StraightLoop2: { }
        for (int i = units.x - 1; i >= 0; i--)
        {
            Vector2Int lookingAt = new Vector2Int(i, units.y);

            int stepVal = RookStep(lookingAt);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    goto StraightLoop3;
                default:
                    print("This code should be impossible");
                    break;
            }
        }

    //Debug.Log("looking up\n");
    StraightLoop3: { }
        for (int i = units.y + 1; i <= 8; i++)
        {
            Vector2Int lookingAt = new Vector2Int(units.x, i);

            int stepVal = RookStep(lookingAt);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    goto StraightLoop4;
                default:
                    print("This code should be impossible");
                    break;
            }
        }

    //Debug.Log("looking down\n");
    StraightLoop4: { }
        for (int i = units.y - 1; i >= 0; i--)
        {
            Vector2Int lookingAt = new Vector2Int(units.x, i);

            int stepVal = RookStep(lookingAt);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    return false;
                default:
                    print("This code should be impossible");
                    break;
            }
        }

        return false;
    }

    /// <summary>
    /// Support method to make the Rook check smaller
    /// </summary>
    /// <param name="target">Target Coord</param>
    /// <returns>1 if found, -1 if null, 0 if empty space</returns>
    private int RookStep(Vector2Int target)
    {
        if (IsTypeAtCoord(target, "Rook"))
        {
            return 1;
        }

        if (IsTypeAtCoord(target, "Queen"))
        {
            return 1;
        }

        Transform transformBeingLookedAt = Coord_Manager.GetTransformAt(target);

        if (transformBeingLookedAt == null)
        {
            return -1;
        }
        if (transformBeingLookedAt.name == "Empty")
        {
            return 0;
        }
        return -1;
    }

    /// <summary>
    /// Check if king is being attacked by Bishop
    /// </summary>
    /// <returns>True if being attacked, False if it is not</returns>
    bool IsBeingAttackedByBishop()
    {
        return IsBeingAttackedByBishop(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

    /// <summary>
    /// Check if king is being attacked by Bishop at Target
    /// </summary>
    /// <param name="units">Target</param>
    /// <returns>True if being attacked, False if it is not</returns>
    bool IsBeingAttackedByBishop(Vector2Int units)
    {
        for (int i = 1; i <= 8 - Mathf.Min(units.x, units.y); i++)
        {
            Vector2Int lookingAt = new Vector2Int(units.x + i, units.y + i);

            int stepVal = BishopStep(lookingAt);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    goto DiagonalLoop2;
                default:
                    print("This code should be impossible");
                    break;
            }
        }

    DiagonalLoop2: { }
        for (int i = 1; i <= 8 - Mathf.Min(units.x, units.y); i++)
        {
            Vector2Int lookingAt = new Vector2Int(units.x - i, units.y + i);

            int stepVal = BishopStep(lookingAt);
            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    goto DiagonalLoop3;
                default:
                    print("This code should be impossible");
                    break;
            }
        }
    DiagonalLoop3: { }
        for (int i = 1; i <= 8 - Mathf.Min(units.x, units.y); i++)
        {
            Vector2Int lookingAt = new Vector2Int(units.x + i, units.y - i);

            int stepVal = BishopStep(lookingAt);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    goto DiagonalLoop4;
                default:
                    print("This code should be impossible");
                    break;
            }
        }

    DiagonalLoop4: { }
        for (int i = 1; i <= Mathf.Max(units.x, units.y); i++)
        {
            Vector2Int lookingAt = new Vector2Int(units.x - i, units.y - i);

            int stepVal = BishopStep(lookingAt);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    return false;
                default:
                    print("This code should be impossible");
                    break;
            }
        }
        return false;
    }

    /// <summary>
    /// Support method to make the Bishop check smaller
    /// </summary>
    /// <param name="target">Target Coord</param>
    /// <returns>1 if found, -1 if null, 0 if empty space</returns>
    int BishopStep(Vector2Int target)
    {

        if (IsTypeAtCoord(target, "Bishop"))
        {
            return 1;
        }

        if (IsTypeAtCoord(target, "Queen"))
        {
            return 1;
        }

        Transform transformBeingLookedAt = Coord_Manager.GetTransformAt(target);

        if (transformBeingLookedAt == null)
        {
            return -1;
        }
        if (transformBeingLookedAt.name == "Empty")
        {
            return 0;
        }
        return -1;
    }

    /// <summary>
    /// Check if king is being attacked by King
    /// </summary>
    /// <returns>True if being attacked, False if it is not</returns>
    bool IsBeingAttackedByKing()
    {
        return IsBeingAttackedByKing(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

    /// <summary>
    /// Check if king is being attacked by King at Target
    /// </summary>
    /// <param name="units">Target</param>
    /// <returns>True if being attacked, False if it is not</returns>
    bool IsBeingAttackedByKing(Vector2Int units, bool onExcept = false)
    {

        if (IsTypeAtCoord(new Vector2Int(units.x + 1, units.y), "King"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x - 1, units.y), "King"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x, units.y + 1), "King"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x, units.y - 1), "King"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x + 1, units.y + 1), "King"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x - 1, units.y - 1), "King"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x - 1, units.y + 1), "King"))
        {
            return true;
        }

        if (IsTypeAtCoord(new Vector2Int(units.x + 1, units.y - 1), "King"))
        {
            return true;
        }

        return false;

    }

}
