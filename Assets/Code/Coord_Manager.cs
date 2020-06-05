﻿using System.Collections;
using System.Collections.Generic;
using ExtraChessStructures;
using UnityEngine;

public class Coord_Manager
{
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
    
    public static void init()
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
                Debug.Log(pieces[i,j]);
            }

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
    static Transform getTransformObject(string name)
    {
        for (int i = 1; i <= 8; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                Transform temp = pieces[i, j];
                if (temp != null) {
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
    static Vector2Int getCoordPosition(string name){
    	for (int i = 1; i <= 8; i++){
    		for (int j = 1; j<= 8; j++){
    			Transform temp = pieces[i, j];
                if (temp != null) {
    				if(temp.name == name){
    					return new Vector2Int(i, j);
    				}
    			}
    		}
    	}
    	return Vector2Int.one * -1;
    }

    //Updates pieces table with new position
    public static void UpdatePosition(string name, Vector3 value)
    {
    	Debug.Log("updated");
        Transform transformObj = getTransformObject(name);

        Vector2Int ChessCoords = ConvertCoordsToChessUnits(value);
        Vector2Int old = getCoordPosition(name);

        pieces[ChessCoords.x, ChessCoords.y] = transformObj;
        pieces[old.x, old.y] = null;
    }

    //Compare old position with current position, returns offset
    public static Vector2Int GetPositionDifference(string name, Vector3 value)
    {
        Vector2 convertedValue = ConvertCoordsToChessUnits(value);
        Vector2Int transformObj = getCoordPosition(name);
        Debug.Log(getCoordPosition(name));
        Vector2 temp = convertedValue - transformObj;
        return new Vector2Int((int)temp.x, (int)temp.y);
    }

    //Checks if piece is currently colliding with another piece
    //returns 2 flags as a vector 2
    //(0, 0, 0) = no collition
    //(1, 0, 0) = colliding with opposite colour
    //(1, 1, 0) = colliding with own colour
    //(1, 0, 1) = colliding with king
    public static ColInfo CheckCollition(Transform piece)
    {
        Vector2Int chessCoords = ConvertCoordsToChessUnits(piece.localPosition);
        ColInfo flags = new ColInfo(false, false, false);

        Transform col = pieces[chessCoords.x, chessCoords.y];

        if ( col != null)
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
        if (temp != null) {
            return temp.name;
        }
        return null;
    }

    public static string GetTypeAt(Vector2Int pos)
    {
        Transform temp = pieces[pos.x, pos.y];
        if (temp != null)
        {
            return temp.tag;
        }
        return null;
    }

    public static Transform GetGameObjectAt(Vector2Int pos)
    {
        try
        {
            return pieces[pos.x, pos.y];
        }
        catch (System.IndexOutOfRangeException ex)
        {
            Debug.LogError(ex);
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
                if (temp != null) {
                	if(pieces[i, j].name == name)
                	{
                    	pieces[i, j] = null;
                	}
                }
            }
        }
        deadPieces.Add(name);
    }
}