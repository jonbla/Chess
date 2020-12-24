using ExtraChessStructures;
using UnityEngine;

public class King_Piece : Custom_Mono
{
    //Is this move valid for a king
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

    //Check if king is being attacked
    public bool IsBeingAttacked()
    {
        return IsBeingAttackedByPawn() || isBeingAttackedByHorse() || IsBeingAttackedByRook() || IsBeingAttackedByBishop() || IsBeingAttackedByKing();
    }

    public bool IsBeingAttacked(Vector2Int space)
    {
        return IsBeingAttackedByPawn(space) || IsBeingAttackedByHorse(space) || IsBeingAttackedByRook(space) || IsBeingAttackedByBishop(space) || IsBeingAttackedByKing(space);
    }

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

    //Check if king is being attacked by pawn
    bool IsBeingAttackedByPawn()
    {
        return IsBeingAttackedByPawn(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

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

    bool isBeingAttackedByHorse()
    {
        return IsBeingAttackedByHorse(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));

    }

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
    //Is the King being attacked by a Rook
    bool IsBeingAttackedByRook()
    {
        return IsBeingAttackedByRook(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

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

    //support method to make the Rook check smaller
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

    bool IsBeingAttackedByBishop()
    {
        return IsBeingAttackedByBishop(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

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

    bool IsBeingAttackedByKing()
    {
        return IsBeingAttackedByKing(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    }

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
