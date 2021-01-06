using System.Collections;
using System.Collections.Generic;
using ExtraChessStructures;
using UnityEngine;

public static class Death_Manager
{
    private static int blackIndex = -1;
    private static int whiteIndex = -1;

    private static readonly List<Vector2> Blackcoords = new List<Vector2>() {new Vector2(6.38f, 4.06f),
                                                        new Vector2(7.54f, 4.06f),
                                                        new Vector2(6.38f, 2.90f),
                                                        new Vector2(7.54f, 2.90f),
                                                        new Vector2(6.38f, 1.74f),
                                                        new Vector2(7.54f, 1.74f),
                                                        new Vector2(6.38f, 0.58f),
                                                        new Vector2(7.54f, 0.58f),
                                                        new Vector2(6.38f, -0.58f),
                                                        new Vector2(7.54f, -0.58f),
                                                        new Vector2(6.38f, -1.74f),
                                                        new Vector2(7.54f, -1.74f),
                                                        new Vector2(6.38f, -2.90f),
                                                        new Vector2(7.54f, -2.90f),
                                                        new Vector2(6.38f, -4.06f),
                                                        new Vector2(7.54f, -4.06f)};

    private static readonly List<Vector2> Whitecoords = new List<Vector2>() {new Vector2(-6.38f, 4.06f),
                                                        new Vector2(-7.54f, 4.06f),
                                                        new Vector2(-6.38f, 2.90f),
                                                        new Vector2(-7.54f, 2.90f),
                                                        new Vector2(-6.38f, 1.74f),
                                                        new Vector2(-7.54f, 1.74f),
                                                        new Vector2(-6.38f, 0.58f),
                                                        new Vector2(-7.54f, 0.58f),
                                                        new Vector2(-6.38f, -0.58f),
                                                        new Vector2(-7.54f, -0.58f),
                                                        new Vector2(-6.38f, -1.74f),
                                                        new Vector2(-7.54f, -1.74f),
                                                        new Vector2(-6.38f, -2.90f),
                                                        new Vector2(-7.54f, -2.90f),
                                                        new Vector2(-6.38f, -4.06f),
                                                        new Vector2(-7.54f, -4.06f)};

    /// <summary>
    /// Gets death index for next black piece
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetBlackCoord()
    {
        blackIndex++;
        return Blackcoords[blackIndex];
    }

    /// <summary>
    /// Gets death index for next white piece
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetWhiteCoord()
    {
        whiteIndex++;
        return Whitecoords[whiteIndex];
    }



}
