﻿using System.Collections.Generic;
using ExtraChessStructures;
using UnityEngine;

/// <summary>
/// Responsible for all functions related to chess coordinates and locations
/// </summary>
public class Coord_Manager
{
    /// <summary>
    /// Struct to hold rows
    /// </summary>
    struct RowStruct
    {
        /// <summary>
        /// Row Value
        /// </summary>
        public Transform[] row;

        /// <summary>
        /// Int representation of row
        /// </summary>
        public int rowNum;

        /// <summary>
        /// Struct to hold rows
        /// </summary>
        /// <param name="row">Row Content</param>
        /// <param name="rowNum">Int representation of row</param>
        public RowStruct(Transform[] row, int rowNum)
        {
            this.row = row;
            this.rowNum = rowNum;
        }
    }

    /// <summary>
    /// Coordinate Manager Static Instance
    /// </summary>
    public static Coord_Manager Coord_Man; //makes this a publicly accessable object

    static readonly Coord_Helper helper = GameObject.Find("ExtraCode").transform.Find("Coord_Helper").GetComponent<Coord_Helper>();
    private static readonly Main main = GameObject.Find("MainCode").GetComponent<Main>();

    /// <summary>
    /// Converts real coords to board coords
    /// </summary>
    static readonly Dictionary<float, int> map = new Dictionary<float, int>() {
                                                {-4.06f,  1},
                                                {-2.90f,  2},
                                                {-1.74f,  3},
                                                {-0.58f,  4},
                                                {0.58f,   5},
                                                {1.74f,   6},
                                                {2.90f,   7},
                                                {4.06f,   8}
                                                };

    /// <summary>
    /// Converts board coords to Real coords (Local units)
    /// </summary>
    static readonly Dictionary<int, float> map_R = new Dictionary<int, float>() {
                                                {1, -4.06f},
                                                {2, -2.90f},
                                                {3, -1.74f},
                                                {4, -0.58f},
                                                {5, 0.58f},
                                                {6, 1.74f},
                                                {7, 2.90f},
                                                {8, 4.06f}
                                                };

    //table of all piece locations
    /*static Dictionary<string, Vector2> PiecePositions = new Dictionary<string, Vector2>() {
                                                {"White_Rook_1",   new Vector2(1,1) },
                                                {"White_Rook_2",   new Vector2(8,1) },
                                                {"White_Bishop_1", new Vector2(2,1) },
                                                {"White_Bishop_2", new Vector2(7,1) },
                                                {"White_Horse_1",  new Vector2(3,1) },
                                                {"White_Horse_2",  new Vector2(6,1) },
                                                {"White_King",     new Vector2(5,1) },
                                                {"White_Queen",    new Vector2(4,1) },
                                                {"White_Pawn_1",   new Vector2(1,2) },
                                                {"White_Pawn_2",   new Vector2(2,2) },
                                                {"White_Pawn_3",   new Vector2(3,2) },
                                                {"White_Pawn_4",   new Vector2(4,2) },
                                                {"White_Pawn_5",   new Vector2(5,2) },
                                                {"White_Pawn_6",   new Vector2(6,2) },
                                                {"White_Pawn_7",   new Vector2(7,2) },
                                                {"White_Pawn_8",   new Vector2(8,2) },

                                                {"Black_Rook_1",   new Vector2(1,8) },
                                                {"Black_Rook_2",   new Vector2(8,8) },
                                                {"Black_Bishop_1", new Vector2(2,8) },
                                                {"Black_Bishop_2", new Vector2(7,8) },
                                                {"Black_Horse_1",  new Vector2(3,8) },
                                                {"Black_Horse_2",  new Vector2(6,8) },
                                                {"Black_King",     new Vector2(5,8) },
                                                {"Black_Queen",    new Vector2(4,8) },
                                                {"Black_Pawn_1",   new Vector2(1,7) },
                                                {"Black_Pawn_2",   new Vector2(2,7) },
                                                {"Black_Pawn_3",   new Vector2(3,7) },
                                                {"Black_Pawn_4",   new Vector2(4,7) },
                                                {"Black_Pawn_5",   new Vector2(5,7) },
                                                {"Black_Pawn_6",   new Vector2(6,7) },
                                                {"Black_Pawn_7",   new Vector2(7,7) },
                                                {"Black_Pawn_8",   new Vector2(8,7) }
                                                };*/

    /// <summary>
    /// Size of board in x and y direction
    /// </summary>
    public static readonly int BOARDSIZE = 8;

    /// <summary>
    /// List of elements that are out of play
    /// </summary>
    private static readonly List<string> deadPieces = new List<string>();

    /// <summary>
    /// List of elements that are out of play
    /// </summary>
    private static readonly List<string> tempDead = new List<string>();

    /// <summary>
    /// 8x8 Table of pieces 
    /// </summary>
    private static readonly Transform[,] board = new Transform[BOARDSIZE + 1, BOARDSIZE + 1];

    /// <summary>
    /// 8x8 Table of pieces 
    /// </summary>
    private static readonly Transform[,] tempBoard = new Transform[BOARDSIZE + 1, BOARDSIZE + 1];

    /// <summary>
    /// Where the piece is being dropped over
    /// </summary>
    public static Vector2Int hoverPos = new Vector2Int(0, 0);

    /// <summary>
    /// Where the piece was picked up from
    /// </summary>
    public static Vector2Int sourcePos = new Vector2Int(0, 0);

    /// <summary>
    /// Representation of an empty cell
    /// </summary>
    static readonly Transform empty = new GameObject("Empty").transform;

    /// <summary>
    /// Intitialisation of board and coordinate manager
    /// </summary>
    public static void Init()
    {
        //List<Transform> temp = new List<Transform>();

        foreach (Transform colour in GameObject.Find("Board").transform.Find("Pieces").transform)
        {
            foreach (Transform piece in colour)
            {
                Vector2Int chessCoord = ConvertCoordsToChessUnits(piece.localPosition);
                board[chessCoord.x, chessCoord.y] = piece;
            }
        }

        for (int i = 1; i <= BOARDSIZE; i++)
        {
            for (int j = 1; j <= BOARDSIZE; j++)
            {
                if (board[i, j] == null)
                {
                    board[i, j] = empty;
                }
                //Debug.Log(pieces[i, j]);
            }
        }
        ClearTempBoard();
    }

    /// <summary>
    /// Sets the tempboard to the currect board
    /// </summary>
    static void ClearTempBoard()
    {
        for (int i = 1; i <= BOARDSIZE; i++)
        {
            for (int j = 1; j <= BOARDSIZE; j++)
            {
                tempBoard[i, j] = board[i, j];
            }
        }

        for (int i = 0; i < deadPieces.Count; i++)
        {
            tempDead[i] = deadPieces[i];
        }
    }

    /// <summary>
    /// Converts world units into Chess Board Coords.
    /// World units must be local
    /// </summary>
    /// <param name="raw">Raw input to be converted</param>
    /// <returns>Converted raw input as custom chess units</returns>
    public static Vector2Int ConvertCoordsToChessUnits(Vector2 raw)
    {
        Vector2Int temp = Vector2Int.zero;
        foreach (KeyValuePair<float, int> space in map)
        {
            if (Mathf.Approximately(space.Key, raw.x))
            {
                temp = new Vector2Int(map[space.Key], temp.y);
            }
            if (Mathf.Approximately(space.Key, raw.y))
            {
                temp = new Vector2Int(temp.x, map[space.Key]);
            }
        }
        return temp;
    }

    /// <summary>
    /// Converts Chess Board units into Real Coords.
    /// </summary>
    /// <param name="raw">Raw input to be converted</param>
    /// <returns>Converted raw input as custom chess units</returns>
    public static Vector2 ConvertChessUnitsToCoords(Vector2Int raw)
    {
        Vector2 temp = Vector2.zero;
        foreach (KeyValuePair<int, float> space in map_R)
        {
            if (Mathf.Approximately(space.Key, raw.x))
            {
                temp = new Vector2(map_R[space.Key], temp.y);
            }
            if (Mathf.Approximately(space.Key, raw.y))
            {
                temp = new Vector2(temp.x, map_R[space.Key]);
            }
        }
        return temp;
    }

    /// <summary>
    /// Finds piece by its name
    /// </summary>
    /// <param name="name">Name of piece to find</param>
    /// <param name="main">Main board, or temp board target</param>
    /// <returns>Found piece Transform, null if not found</returns>
    public static Transform GetTransformObject(string name, bool main = false)
    {
        Transform[,] targetBoard = main ? board : tempBoard;
        for (int i = 1; i <= 8; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                Transform temp = targetBoard[i, j];
                if (temp != null && temp.name != "Empty")
                {
                    if (temp.name == name)
                    {
                        return targetBoard[i, j];
                    }
                }
            }
        }
        return null;
    }

    //get the chess coords of the named piece

    /// <summary>
    /// Find coordinates of a piece from its name
    /// </summary>
    /// <param name="name">Name of piece to find</param>
    /// <param name="main">Main board, or temp board target</param>
    /// <returns>Coodinates of found piece, returns (-1,-1) if not found</returns>
    public static Vector2Int GetCoordPosition(string name, bool main = false)
    {
        Transform[,] targetBoard = main ? board : tempBoard;
        for (int i = 1; i <= 8; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                Transform temp = targetBoard[i, j];
                if (temp != null && temp.name != "Empty")
                {
                    if (temp.name == name)
                    {
                        return new Vector2Int(i, j);
                    }
                }
            }
        }
        return Vector2Int.one * -1;
    }


    /// <summary>
    /// Notify board of a move that has been made
    /// </summary>
    /// <param name="name">Name of piece to update</param>
    /// <param name="value">Coords of location it moved to</param>
    public static void UpdatePosition(string name, Vector3 value)
    {
        Transform transformObj = GetTransformObject(name);

        Vector2Int ChessCoords = ConvertCoordsToChessUnits(value);
        Vector2Int old = GetCoordPosition(name);

        hoverPos = ChessCoords;

        Debug.Log(old.x + ", " + old.y + ", " + name);
        tempBoard[old.x, old.y] = empty;
        tempBoard[ChessCoords.x, ChessCoords.y] = transformObj;

        sourcePos = old;

        Debug.Log("updated " + name);
    }

    public static void RevertMove(ref Vector2Int moveDelta)
    {
        ClearTempBoard();
        main.Clear();
        moveDelta = Vector2Int.zero;
        Debug.Log("revertMove");
    }

    public static void CommitPositionUpdate()
    {
        for (int i = 1; i <= BOARDSIZE; i++)
        {
            for (int j = 1; j <= BOARDSIZE; j++)
            {
                board[i, j] = tempBoard[i, j];
            }
        }

        for (int i = 0; i < deadPieces.Count; i++)
        {
            deadPieces[i] = tempDead[i];
        }
    }

    //Compare old position with current position, returns offset

    /// <summary>
    /// Compare old to current positions to find Delta
    /// </summary>
    /// <returns>Delta of last move</returns>
    public static Vector2Int GetPositionDifference()
    {
        Vector2 temp = hoverPos - sourcePos;
        Debug.Log(temp);
        return new Vector2Int((int)temp.x, (int)temp.y);
    }

    //Checks if piece is currently colliding with another piece

    /// <summary>
    /// Build and return collition struct
    /// </summary>
    /// <param name="piece">Piece to get collition information from</param>
    /// <param name="main">Main board, or temp board target</param>
    /// <returns>Col info</returns>
    public static ColInfo CheckCollition(Transform piece, bool main = true)
    {
        Transform[,] targetBoard = main ? board : tempBoard;
        Vector2Int chessCoords = ConvertCoordsToChessUnits(piece.localPosition);
        ColInfo flags = new ColInfo(false, false, false);

        Transform col = targetBoard[chessCoords.x, chessCoords.y];

        flags.nameOfColObject = col.name;

        if (col.name != "Empty")
        {
            flags.isColliding = true;
            if (col.parent == piece.parent)
            {
                flags.isCollidingWithOwnTeam = true;
                if (col.CompareTag("King"))
                {
                    flags.isCollidingWithKing = true;
                }
            }
        }
        return flags;
    }

    public static ColInfo CheckCollitionAt(Vector2Int pos, string parentName, bool main = true)
    {
        Transform[,] targetBoard = main ? board : tempBoard;
        Transform col = targetBoard[pos.x, pos.y];
        ColInfo flags = new ColInfo(false, false, false);

        flags.nameOfColObject = col.name;

        if (col.name != "Empty")
        {
            flags.isColliding = true;
            if (col.parent.name == parentName)
            {
                flags.isCollidingWithOwnTeam = true;
                if (col.CompareTag("King"))
                {
                    flags.isCollidingWithKing = true;
                }
            }
        }
        return flags;


    }

    //reverse table lookup, gets piece name from coord locations

    /// <summary>
    /// Find name of piece at coord
    /// </summary>
    /// <param name="pos">Position to look at</param>
    /// <param name="main">Main board, or temp board target</param>
    /// <returns>Name of piece at location, null if empty</returns>
    public static string GetNameAt(Vector2Int pos, bool main = false)
    {
        Transform[,] targetBoard = main ? board : tempBoard;
        Transform temp = targetBoard[pos.x, pos.y];
        if (temp != null && temp.name != "Empty")
        {
            return temp.name;
        }
        return null;
    }

    /// <summary>
    /// Find type of piece at coord
    /// </summary>
    /// <param name="pos">Position to look at</param>
    /// <param name="main">Main board, or temp board target</param>
    /// <returns>Type of piece at location, null if empty</returns>
    public static string GetTypeAt(Vector2Int pos, bool main = false)
    {
        Transform[,] targetBoard = main ? board : tempBoard;
        Transform temp = targetBoard[pos.x, pos.y];
        if (temp != null && temp.name != "Empty")
        {
            return temp.tag;
        }
        return null;
    }

    /// <summary>
    /// Find piece at coord
    /// </summary>
    /// <param name="pos">Position to look at</param>
    /// <param name="main">Main board, or temp board target</param>
    /// <returns>Piece at location, null if empty</returns>
    public static Transform GetTransformAt(Vector2Int pos, bool main = false)
    {
        Transform[,] targetBoard = main ? board : tempBoard;
        try
        {
            return tempBoard[pos.x, pos.y];
        }
        catch (System.IndexOutOfRangeException)
        {
            //Debug.LogError("out of index");
            //throw ex;
            return null;
        }

    }

    /// <summary>
    /// Find chess piece component by coords
    /// </summary>
    /// <param name="pos">Piece coordinates</param>
    /// <param name="main">Main board, or temp board target</param>
    /// <returns>Chess Piece component</returns>
    public static Chess_Piece GetPieceAt(Vector2Int pos, bool main = false)
    {
        Transform temp = GetTransformAt(pos, main);

        if (temp != null)
        {
            return temp.GetComponent<Chess_Piece>();
        }
        return null;
    }

    /// <summary>
    /// Find chess piece component by name
    /// </summary>
    /// <param name="name">Piece name</param>
    /// <param name="main">Main board, or temp board target</param>
    /// <returns>Chess Piece component</returns>
    public static Chess_Piece GetPiece(string name, bool main = false)
    {
        Transform temp = GetTransformObject(name, main);
        if (temp != null)
        {
            return temp.GetComponent<Chess_Piece>();
        }
        return null;
    }

    /// <summary>
    /// Kills named piece
    /// </summary>
    /// <param name="name">Name of piece to kill</param>
    /// <param name="main">Main board, or temp board target</param>
    public static void KillPiece(string name, bool main = false)
    {
        List<string> targetDeadList = main ? deadPieces : tempDead;
        Transform[,] targetBoard = main ? board : tempBoard;
        for (int i = 1; i <= 8; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                Transform temp = targetBoard[i, j];
                if (temp != null && temp.name != "Empty")
                {
                    if (targetBoard[i, j].name == name)
                    {
                        targetBoard[i, j] = empty;
                    }
                }
            }
        }
        targetDeadList.Add(name);
    }

    /// <summary>
    /// Finds information about King's check status
    /// </summary>
    /// <returns>Check Flags describing kings check status</returns>
    public static CheckFlags GetCheckInfoAt(Vector2Int space, bool isBlack, bool main = false)
    {
        CheckFlags flags = new CheckFlags
        {
            isInCheck = IsBeingAttacked(space, isBlack, main),
            isInCheckmate = false
        };

        //flags.isInCheckmate = flags.isInCheck ? IsInMate(space, isBlack, main) : false;

        return flags;
    }

    /// <summary>
    /// Check if input location is being attacked
    /// </summary>
    /// <param name="space">Target location</param>
    /// <returns>True if being attacked, False if it is not</returns>
    static bool IsBeingAttacked(Vector2Int space, bool isBlack, bool main = false)
    {
        return IsBeingAttackedByPawn(space, isBlack, main) || IsBeingAttackedByHorse(space, isBlack, main) || IsBeingAttackedByRook(space, isBlack, main) || IsBeingAttackedByBishop(space, isBlack, main) || IsBeingAttackedByKing(space, isBlack, main);
    }

    /// <summary>
    /// Check if king is being attacked by pawn at Target
    /// </summary>
    /// <param name="units">Target</param>
    /// <returns>True if being attacked, False if it is not</returns>
    static bool IsBeingAttackedByPawn(Vector2Int units, bool isBlack, bool main = false)
    {

        if (helper.IsTypeAtCoord(new Vector2Int(units.x + 1, units.y + (isBlack ? -1 : 1)), "Pawn", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x + -1, units.y + (isBlack ? -1 : 1)), "Pawn", isBlack, main))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Check if king is being attacked by Horse at Target
    /// </summary>
    /// <param name="units">Target</param>
    /// <returns>True if being attacked, False if it is not</returns>
    private static bool IsBeingAttackedByHorse(Vector2Int units, bool isBlack, bool main = false)
    {
        if (helper.IsTypeAtCoord(new Vector2Int(units.x + 1, units.y + 2), "Horse", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x + 1, units.y - 2), "Horse", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x - 1, units.y - 2), "Horse", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x - 1, units.y + 2), "Horse", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x + 2, units.y + 1), "Horse", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x + 2, units.y - 1), "Horse", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x - 2, units.y - 1), "Horse", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x - 2, units.y + 1), "Horse", isBlack, main))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Check if king is being attacked by Rook at Target
    /// </summary>
    /// <param name="units">Target</param>
    /// <returns>True if being attacked, False if it is not</returns>
    static bool IsBeingAttackedByRook(Vector2Int units, bool isBlack, bool main = false)
    {
        //Debug.Log("looking right\n");
        for (int i = units.x + 1; i <= 8; i++)
        {
            Vector2Int lookingAt = new Vector2Int(i, units.y);

            int stepVal = RookStep(lookingAt, isBlack, main);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    goto StraightLoop2;
                default:
                    Debug.Log("This code should be impossible");
                    break;
            }
        }

    //Debug.Log("looking left\n");
    StraightLoop2: { }
        for (int i = units.x - 1; i >= 0; i--)
        {
            Vector2Int lookingAt = new Vector2Int(i, units.y);

            int stepVal = RookStep(lookingAt, isBlack, main);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    goto StraightLoop3;
                default:
                    Debug.Log("This code should be impossible");
                    break;
            }
        }

    //Debug.Log("looking up\n");
    StraightLoop3: { }
        for (int i = units.y + 1; i <= 8; i++)
        {
            Vector2Int lookingAt = new Vector2Int(units.x, i);

            int stepVal = RookStep(lookingAt, isBlack, main);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    goto StraightLoop4;
                default:
                    Debug.Log("This code should be impossible");
                    break;
            }
        }

    //Debug.Log("looking down\n");
    StraightLoop4: { }
        for (int i = units.y - 1; i >= 0; i--)
        {
            Vector2Int lookingAt = new Vector2Int(units.x, i);

            int stepVal = RookStep(lookingAt, isBlack, main);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    return false;
                default:
                    Debug.Log("This code should be impossible");
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
    static private int RookStep(Vector2Int target, bool isBlack, bool main = false)
    {
        if (helper.IsTypeAtCoord(target, "Rook", isBlack, main))
        {
            return 1;
        }

        if (helper.IsTypeAtCoord(target, "Queen", isBlack, main))
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
    /// Check if king is being attacked by Bishop at Target
    /// </summary>
    /// <param name="units">Target</param>
    /// <returns>True if being attacked, False if it is not</returns>
    static bool IsBeingAttackedByBishop(Vector2Int units, bool isBlack, bool main = false)
    {
        for (int i = 1; i <= 8 - Mathf.Min(units.x, units.y); i++)
        {
            Vector2Int lookingAt = new Vector2Int(units.x + i, units.y + i);

            int stepVal = BishopStep(lookingAt, isBlack, main);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    goto DiagonalLoop2;
                default:
                    Debug.Log("This code should be impossible");
                    break;
            }
        }

    DiagonalLoop2: { }
        for (int i = 1; i <= 8 - Mathf.Min(units.x, units.y); i++)
        {
            Vector2Int lookingAt = new Vector2Int(units.x - i, units.y + i);

            int stepVal = BishopStep(lookingAt, isBlack, main);
            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    goto DiagonalLoop3;
                default:
                    Debug.Log("This code should be impossible");
                    break;
            }
        }
    DiagonalLoop3: { }
        for (int i = 1; i <= 8 - Mathf.Min(units.x, units.y); i++)
        {
            Vector2Int lookingAt = new Vector2Int(units.x + i, units.y - i);

            int stepVal = BishopStep(lookingAt, isBlack, main);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    goto DiagonalLoop4;
                default:
                    Debug.Log("This code should be impossible");
                    break;
            }
        }

    DiagonalLoop4: { }
        for (int i = 1; i <= Mathf.Max(units.x, units.y); i++)
        {
            Vector2Int lookingAt = new Vector2Int(units.x - i, units.y - i);

            int stepVal = BishopStep(lookingAt, isBlack, main);

            switch (stepVal)
            {
                case 1:
                    return true;
                case 0:
                    continue;
                case -1:
                    return false;
                default:
                    Debug.Log("This code should be impossible");
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
    static int BishopStep(Vector2Int target, bool isBlack, bool main = false)
    {

        if (helper.IsTypeAtCoord(target, "Bishop", isBlack, main))
        {
            return 1;
        }

        if (helper.IsTypeAtCoord(target, "Queen", isBlack, main))
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
    /// Check if king is being attacked by King at Target
    /// </summary>
    /// <param name="units">Target</param>
    /// <returns>True if being attacked, False if it is not</returns>
    static bool IsBeingAttackedByKing(Vector2Int units, bool isBlack, bool main = false)
    {

        if (helper.IsTypeAtCoord(new Vector2Int(units.x + 1, units.y), "King", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x - 1, units.y), "King", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x, units.y + 1), "King", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x, units.y - 1), "King", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x + 1, units.y + 1), "King", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x - 1, units.y - 1), "King", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x - 1, units.y + 1), "King", isBlack, main))
        {
            return true;
        }

        if (helper.IsTypeAtCoord(new Vector2Int(units.x + 1, units.y - 1), "King", isBlack, main))
        {
            return true;
        }

        return false;

    }
    public static void Reset()
    {
        foreach (Transform colour in GameObject.Find("Board").transform.Find("Pieces").transform)
        {
            foreach (Transform piece in colour)
            {
                piece.GetComponent<Chess_Piece>().Reset();
            }
        }        
    }

    public static List<Move> GetMoves(bool main, bool LookForBlackPieces)
    {
        Transform[,] targetBoard = main ? board : tempBoard;
        return Move_Generator.Generate(targetBoard, LookForBlackPieces);
    }

    public static void ExecuteMove(Move move)
    {
        UpdatePosition(move.name, move.finalPosRaw);
        GetPiece(move.name).IsValidMove(move);
        main.KillPieceMarkedForDeath();
        CommitPositionUpdate();
        RenderBoard();
    }

    public static void RenderBoard(bool main = true)
    {
        RenderBoard(main ? board : tempBoard);
    }

    public static void RenderBoard(Transform[,] targetBoard)
    {        
        for (int i = 1; i <= BOARDSIZE; i++)
        {
            for (int j = 1; j <= BOARDSIZE; j++)
            {
                targetBoard[i, j].localPosition = ConvertChessUnitsToCoords(new Vector2Int(i, j));
                try
                {
                    targetBoard[i, j].GetComponent<Chess_Piece>().CenterPiece();
                }
                catch { }
            }
        }
    }    
}
