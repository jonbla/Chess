using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : Custom_Mono
{

    public bool isFadingOut;

    public SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        isFadingOut = GetIsBlack();
    }

    // Update is called once per frame
    void Update()
    {
        float fade = isFadingOut ? 0 : 1;        
        sp.color = Color.Lerp(sp.color, new Color(sp.color.r, sp.color.g, sp.color.b, fade), .03f);
    }

    public void ToggleFade()
    {
        isFadingOut = isFadingOut ? false : true;
        //print("Toggling");
    }
}
