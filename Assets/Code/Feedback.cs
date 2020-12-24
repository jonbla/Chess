using UnityEngine;

public static class Feedback// : MonoBehaviour
{
    static TextMesh line;

    public static void Init()
    {
        line = GameObject.Find("Display").GetComponent<TextMesh>();
        SetText("Welcome to Chess");
    }

    public static void SetText(string text)
    {
        line.text = text;
    }
}
