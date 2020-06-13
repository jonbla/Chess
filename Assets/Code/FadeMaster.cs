using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMaster : MonoBehaviour
{
    public Fader BlackFade;
    public Fader WhiteFade;
   public void ToggleFade()
    {
        //print("TogglingMaster");
        BlackFade.ToggleFade();
        WhiteFade.ToggleFade();
    }
}
