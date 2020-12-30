using System.Collections.Generic;
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

    //converts real coords to board coords
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
    private static readonly Transform[,] board = new Transform[BOARDSIZE+1, BOARDSIZE + 1];

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

    /*
    /// <summary>
    /// Get row by number
    /// </summary>
    /// <param name="rowNum">Row to return</param>
    /// <returns>nth row</returns>
    static RowStruct GetRow(int rowNum)
    {
        Transform[] row = new Transform[9];
        Debug.LogWarning("Starting Row Get");
        for (int i = 1; i <= 8; i++)
        {
            row[i] = board[i, rowNum];
            //Debug.Log(pieces[i, rowNum]);
        }
        Debug.LogWarning("Ending Row Get");
        return new RowStruct(row, rowNum);
    }

    /// <summary>
    /// Modify a specific row in the table
    /// </summary>
    /// <param name="row">Row struct to insert</param>
    static void InsertRow(RowStruct row)
    {
        for (int i = 1; i <= 8; i++)
        {
            board[i, row.rowNum] = row.row[i];
        }
    }*/



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
    /// Finds piece by its name
    /// </summary>
    /// <param name="name">Name of piece to find</param>
    /// <param name="main">Main board, or temp board target</param>
    /// <returns>Found piece Transform, null if not found</returns>
    static Transform GetTransformObject(string name, bool main = false)
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
    static Vector2Int GetCoordPosition(string name, bool main = false)
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

    //public static void UpdatePosition(string name, Vector3 value)
    //{
    //    Transform transformObj = GetTransformObject(name);

    //    Vector2Int ChessCoords = ConvertCoordsToChessUnits(value);
    //    Vector2Int old = GetCoordPosition(name);

    //    rowTemp1 = GetRow(ChessCoords.y);
    //    rowTemp2 = GetRow(old.y);

    //    hoverPos = ChessCoords;
    //    board[old.x, old.y] = empty;

    //    sourcePos = old;

    //    Debug.Log("updated " + name);
    //}

    /// <summary>
    /// Undo Function, can only go back 1 move
    /// </summary>

    public static void RevertMove()
    {
        ClearTempBoard();
        Debug.Log("revertMove");
    }

    //public static void RevertMove()
    //{
    //    InsertRow(rowTemp1);
    //    InsertRow(rowTemp2);
    //    Debug.Log("revertMove");
    //}

    /// <summary>
    /// Tell board to commit temp to permanent board
    /// </summary>
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

        if(temp != null)
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
    public CheckFlags GetCheckInfo()
    {
        CheckFlags flags = new CheckFlags();

        //TODO Add Check logic here

        return flags;
    }

}
