using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adds extra features not possible with static objects, like transform objects
/// </summary>
public class Coord_Helper : MonoBehaviour
{
    /// <summary>
    /// Check if Object type exists at coord
    /// </summary>
    /// <param name="pos">Target position</param>
    /// <param name="type">Target type</param>
    /// <returns>True if type found at location, false otherwise</returns>
    public bool IsTypeAtCoord(Vector2Int pos, string type, bool lookForWhite, bool main = false)
    {
        Transform obj = Coord_Manager.GetTransformAt(pos, main);
        if (obj != null && obj.name != "Empty")
        {
            bool temp = !obj.GetComponent<Chess_Piece>().team.isBlack;
            if (temp == lookForWhite)
            {
                return obj.CompareTag(type);
            }
        }
        return false;

    }
}