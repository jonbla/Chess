using UnityEngine;

/// <summary>
/// Controls Fade
/// </summary>
public class FadeMaster : MonoBehaviour
{
    public Fader BlackFade;
    public Fader WhiteFade;

    /// <summary>
    /// Fade White to Black, or Black to White
    /// </summary>
    public void ToggleFade()
    {
        //print("TogglingMaster");
        BlackFade.ToggleFade();
        WhiteFade.ToggleFade();
    }
}
