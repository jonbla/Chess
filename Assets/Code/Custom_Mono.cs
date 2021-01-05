using System;
using UnityEngine;

/// <summary>
/// Adapter class that adds common functionality to MonoBehaviour
/// </summary>
public class Custom_Mono : MonoBehaviour
{
    protected Main main;
    protected Team_Manager team;

    /// <summary>
    /// Chess Piece attached to this object
    /// </summary>
    protected Chess_Piece CP;

    protected int totalMoves = 0;

    // Start is called before the first frame update
    void Start()
    {
        CP = transform.GetComponent<Chess_Piece>();
        main = GameObject.Find("MainCode").GetComponent<Main>();
        team = transform.parent.GetComponent<Team_Manager>();
    }

    /// <summary>
    /// Kill named piece
    /// </summary>
    /// <param name="name">Name of piece to kill</param>
    protected void Kill(string name)
    {
        Coord_Manager.GetPiece(name, true).GetKilled();
    }

    /// <summary>
    /// Returns the team colour of piece
    /// </summary>
    /// <returns>True if black, False if White</returns>
    protected bool GetIsBlack()
    {
        return transform.parent.name == "Black";
    }

    /// <summary>
    /// Check if object is elegable for en passant
    /// </summary>
    /// <returns>True if it can, False if it can not, Null if error</returns>
    protected bool IsEnPassant()
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

    public void EndTurn()
    {
        totalMoves++;
    }
}
