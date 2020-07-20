using System;
using System.Collections;
using System.Collections.Generic;
using ExtraChessStructures;
using UnityEngine;

public class Coord_Manager
{

    struct RowStruct
    {
        public Transform[] row;
        public int rowNum;

        public RowStruct(Transform[] row, int rowNum)
        {
            this.row = row;
            this.rowNum = rowNum;
        }
    }


    public static Coord_Manager Coord_Man; //makes this a publicly accessable object    
    //converts real coords to board coords
    static Dictionary<float, int> map = new Dictionary<float, int>() { 
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

    //list of elements that are out of play
    static List<string> deadPieces = new List<string>();

    static Transform[,] pieces = new Transform[9,9];
    //static Transform[,] piecesTemp = new Transform[9, 9];
    static Vector2Int hoverPos = new Vector2Int(0, 0);

    static RowStruct rowTemp1, rowTemp2 = GetRow(0);

    static Transform empty = new GameObject("Empty").transform;

    public static void Init()
    {
        List<Transform> temp = new List<Transform>();        

        foreach(Transform colour in GameObject.Find("Board").transform.Find("Pieces").transform){
            foreach(Transform piece in colour)
            {
                Vector2Int chessCoord = ConvertCoordsToChessUnits(piece.localPosition);
                pieces[chessCoord.x,chessCoord.y] = piece;
            }
        }        

        for (int i = 1; i <= 8; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                if(pieces[i,j] == null)
                {
                    pieces[i, j] = empty;
                }
                Debug.Log(pieces[i,j]);
            }

        }
        //ClearTempBoard();
    }

    /*static void ClearTempBoard()
    {
        piecesTemp = pieces;
    }*/

    static RowStruct GetRow(int rowNum)
    {
        Transform[] row = new Transform[9];
        Debug.LogWarning("Starting");
        for (int i = 1; i <= 8; i++)
        {
            row[i] = pieces[i, rowNum];
            Debug.Log(pieces[i, rowNum]);
        }
        Debug.LogWarning("Ending");
        return new RowStruct(row, rowNum);
    }

    static void InsertRow(RowStruct row)
    {
        for (int i = 1; i <= 8; i++)
        {
            pieces[i, row.rowNum] = row.row[i];
        }
    }

    //Converts world units into Chess Board Coords
    //world units must be local
    public static Vector2Int ConvertCoordsToChessUnits(Vector2 raw)
    {
        Vector2Int temp = Vector2Int.zero;
        foreach(KeyValuePair<float, int> space in map)
        {
            if(Mathf.Approximately(space.Key, raw.x))
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

    //return the piece that has this name
    //return null if that name isn't found
    static Transform GetTransformObject(string name)
    {
        for (int i = 1; i <= 8; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                Transform temp = pieces[i, j];
                if (temp != null && temp.name != "Empty") {
                    if (temp.name == name)
                    {
                        return pieces[i, j];
                    }
                }
            }
        }
        return null;
    }

    //get the chess coords of the named piece
    static Vector2Int GetCoordPosition(string name){
    	for (int i = 1; i <= 8; i++){
    		for (int j = 1; j<= 8; j++){
    			Transform temp = pieces[i, j];
                if (temp != null && temp.name != "Empty") {
    				if(temp.name == name){
    					return new Vector2Int(i, j);
    				}
    			}
    		}
    	}
    	return Vector2Int.one * -1;
    }



    public static void UpdatePosition(string name, Vector3 value)
    {
        Transform transformObj = GetTransformObject(name);

        Vector2Int ChessCoords = ConvertCoordsToChessUnits(value);
        Vector2Int old = GetCoordPosition(name);

        rowTemp1 = GetRow(ChessCoords.y);
        rowTemp2 = GetRow(old.y);

        hoverPos = ChessCoords;
        pieces[old.x, old.y] = empty;

        Debug.Log("updated " + name);
    }

    static void RevertMove()
    {
        InsertRow(rowTemp1);
        InsertRow(rowTemp2);
    }

    //Updates pieces table with new position
    public static void CommitPositionUpdate(string name, Vector3 value)
    {    	
        Transform transformObj = GetTransformObject(name);

        Vector2Int ChessCoords = ConvertCoordsToChessUnits(value);
        Vector2Int old = GetCoordPosition(name);

        pieces[ChessCoords.x, ChessCoords.y] = transformObj;
        pieces[old.x, old.y] = empty;
        Debug.Log("Commited "+name+ChessCoords);
    }

    //Compare old position with current position, returns offset
    public static Vector2Int GetPositionDifference(string name, Vector3 value)
    {
        Vector2 convertedValue = ConvertCoordsToChessUnits(value);
        Vector2Int transformObj = GetCoordPosition(name);
        Debug.Log(GetCoordPosition(name));
        Vector2 temp = convertedValue - transformObj;
        return new Vector2Int((int)temp.x, (int)temp.y);
    }

    //Checks if piece is currently colliding with another piece    
    public static ColInfo CheckCollition(Transform piece) 
    {
        Vector2Int chessCoords = ConvertCoordsToChessUnits(piece.localPosition);
        ColInfo flags = new ColInfo(false, false, false);

        Transform col = pieces[chessCoords.x, chessCoords.y];

        if (col.name != "Empty")
        {
            flags.isColliding = true;
            if(col.parent == piece.parent)
            {
                flags.isCollidingWithOwnTeam = true;
                if(col.CompareTag("King"))
                {
                    flags.isCollidingWithKing = true;
                }
            }
        }
        return flags;
    }

    //reverse table lookup, gets piece name from coord locations
    public static string GetNameAt(Vector2Int pos)
    {
        Transform temp = pieces[pos.x, pos.y];
        if (temp != null && temp.name != "Empty") {
            return temp.name;
        }
        return null;
    }

    public static string GetTypeAt(Vector2Int pos)
    {
        Transform temp = pieces[pos.x, pos.y];
        if (temp != null && temp.name != "Empty")
        {
            return temp.tag;
        }
        return null;
    }

    public static Transform GetTransformAt(Vector2Int pos)
    {
        try
        {
            return pieces[pos.x, pos.y];
        }
        catch (System.IndexOutOfRangeException)
        {
            //Debug.LogError("out of index");
            //throw ex;
            return null;
        }
        
    }

    //kills piece by name
    public static void KillPiece(string name)
    {
        for (int i = 0; i <= 8; i++)
        {
            for (int j = 0; j <= 8; j++)
            {
            	Transform temp = pieces[i, j];
                if (temp != null && temp.name != "Empty") {
                	if(pieces[i, j].name == name)
                	{
                    	pieces[i, j] = empty;
                	}
                }
            }
        }
        deadPieces.Add(name);
    }
}
