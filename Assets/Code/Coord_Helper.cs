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
    public bool IsTypeAtCoord(Vector2Int pos, string type)
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
}