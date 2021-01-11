using UnityEngine;

/// <summary>
/// Script responsible for mouse clicks
/// </summary>
public class Mouse_Manager
{
    public static Transform HeldPiece_Transform;
    public static Chess_Piece HeldPiece_CP;

    /// <summary>
    /// Static reference to class
    /// </summary>
    public static Mouse_Manager Mouse_Man;

    /// <summary>
    /// Mouse location last frame
    /// </summary>
    public static Vector2 prevMousePos = Vector2.zero;

    /// <summary>
    /// Move the piece with the mouse
    /// </summary>
    public static void MovePieceWithMouse()
    {
        HeldPiece_CP.mouseIsClicked = true;
        Vector2 temp = GetMouseDelta();
        HeldPiece_Transform.position = new Vector3(HeldPiece_Transform.position.x + temp.x, HeldPiece_Transform.position.y + temp.y, -1);
    }

    //Calculate mouse delta as a vector 2 and return it. 
    //Vectors can not be null, therefor I am treating vector zero as null since getting exactly (0,0) is unlikely and negligable

    /// <summary>
    /// Get change in coords of mouse since last frame
    /// </summary>
    /// <returns>Vector reprisenting mouse movement</returns>
    public static Vector2 GetMouseDelta()
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

    /// <summary>
    /// resets mouse anchor for next click
    /// </summary>
    public static void ResetMouseDelta()
    {
        prevMousePos = Vector2.zero;
    }
}
