using UnityEngine;

/// <summary>
/// Fading object that controls the speed ramp of fade
/// </summary>
public class Fader : Custom_Mono
{

    /// <summary>
    /// Controls direction of fade
    /// </summary>
    public bool isFadingOut;

    /// <summary>
    /// Reference to sprite renderer of compunent
    /// </summary>
    public SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        isFadingOut = transform.name == "Black Wood";
    }

    // Update is called once per frame
    void Update()
    {
        float fade = isFadingOut ? 0 : 1;        
        sp.color = Color.Lerp(sp.color, new Color(sp.color.r, sp.color.g, sp.color.b, fade), .03f);
    }

    /// <summary>
    /// Changes direction of fade
    /// </summary>
    public void ToggleFade()
    {
        isFadingOut = isFadingOut ? false : true;
        //print("Toggling");
    }
}
