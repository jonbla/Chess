using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Manager
{
    public static Mouse_Manager Mouse_Man; //makes this a publicly accessable object
    public static Vector2 prevMousePos = Vector2.zero; //value of previous frame 

    //Calculate mouse delta as a vector 2 and return it. 
    //Vectors can not be null, therefor I am treating vector zero as null since getting exactly (0,0) is unlikely and negligable
    public static Vector2 getMouseDelta()
    {
        Vector3 currentMouseRaw = Input.mousePosition;
        Vector2 currentMouse = Camera.main.ScreenToWorldPoint(currentMouseRaw);

        if (prevMousePos == Vector2.zero)
        {
            prevMousePos = currentMouse;
        }

        Vector2 delta = currentMouse - prevMousePos;
        prevMousePos = currentMouse;
        return delta;
    }

    //reset for next click
    public static void resetMouseDelta()
    {
        prevMousePos = Vector2.zero;
    }
}
