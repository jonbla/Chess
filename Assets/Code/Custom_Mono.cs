using System;
using UnityEngine;

/// <summary>
/// Adapter class that adds common functionality to MonoBehaviour
/// </summary>
public class Custom_Mono : MonoBehaviour
{
    /// <summary>
    /// Chess Piece attached to this object
    /// </summary>
    [HideInInspector]
    public Chess_Piece CP;

    // Start is called before the first frame update
    void Start()
    {
        CP = transform.GetComponent<Chess_Piece>();
    }

    /// <summary>
    /// Kill named piece
    /// </summary>
    /// <param name="name">Name of piece to kill</param>
    public void Kill(string name)
    {
        Coord_Manager.GetPiece(name, true).GetKilled();
    }

    /// <summary>
    /// Kill piece at current location (assumes piece at current location isn't itself)
    /// </summary>
    //public void KillAtLocation()
    //{
    //    bool isBlack = GetIsBlack();

    //    string targetName = Coord_Manager.GetNameAt(Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition));
    //    //Coord_Manager.getTransformAt()
    //    if (isBlack)
    //    {
    //        transform.parent.parent.Find("White").Find(targetName).GetComponent<Chess_Piece>().GetKilled();
    //    }
    //    else
    //    {
    //        transform.parent.parent.Find("Black").Find(targetName).GetComponent<Chess_Piece>().GetKilled();
    //    }

    //}

    /// <summary>
    /// Returns the team colour of piece
    /// </summary>
    /// <returns>True if black, False if White</returns>
    public bool GetIsBlack()
    {
        bool isBlack = false;
        if (transform.parent.name == "Black")
        {
            isBlack = true;
        }
        return isBlack;
    }

    /// <summary>
    /// Check if object is elegable for en passant
    /// </summary>
    /// <returns>True if it can, False if it can not, Null if error</returns>
    public bool IsEnPassant()
    {
        int direction = 1;

        bool isBlack = GetIsBlack();
        if (isBlack)
        {
            direction = -1;
        }
        Vector2Int chessCoords = Coord_Manager.ConvertCoordsToChessUnits(transform.localPosition) - new Vector2Int(0, direction);
        Transform obj = Coord_Manager.GetTransformAt(chessCoords);
        try
        {
            if (obj.tag == "Pawn")
            {
                if (obj.parent != transform.parent)
                {
                    if (obj.GetComponent<Pawn_Piece>().canBePassanted)
                    {
                        obj.GetComponent<Chess_Piece>().GetKilled();
                        return true;
                    }
                }
            }
            return false;
        }
        catch (NullReferenceException)
        {
            return false;
        }
    }
}
