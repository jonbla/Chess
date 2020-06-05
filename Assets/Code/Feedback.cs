using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Feedback// : MonoBehaviour
{
    static TextMesh line;

    public static void init()
    {
        line = GameObject.Find("Display").GetComponent<TextMesh>();
        SetText("Welcome to Chess");
    }

    public static void SetText(string text)
    {
        line.text = text;
    }
}
