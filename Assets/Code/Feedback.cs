using UnityEngine;

/// <summary>
/// Class that controls feedback text
/// </summary>
public static class Feedback// : MonoBehaviour
{
    /// <summary>
    /// reference to text
    /// </summary>
    static TextMesh line;

    /// <summary>
    /// Initialization of text
    /// </summary>
    public static void Init()
    {
        line = GameObject.Find("Display").GetComponent<TextMesh>();
        SetText("Welcome to Chess");
    }

    /// <summary>
    /// Set feedback text
    /// </summary>
    /// <param name="text">Text to set</param>
    public static void SetText(string text)
    {
        line.text = text;
    }
}
