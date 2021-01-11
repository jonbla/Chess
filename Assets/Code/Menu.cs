using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Menu : MonoBehaviour
{

    public Camera mainCamera;
    public Camera altCamera;
    public GameObject tint;
    public GameObject pauseMenuPopUp;

    public bool inPause;

    // Start is called before the first frame update
    void Start()
    {
        ExitPauseMenu();
    }

    public void EnterPauseMenu()
    {
        inPause = true;
        mainCamera.gameObject.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
        altCamera.gameObject.SetActive(true);
        tint.SetActive(true);
        pauseMenuPopUp.SetActive(true);
    }

    public void ExitPauseMenu()
    {
        inPause = false;
        mainCamera.gameObject.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = false;
        altCamera.gameObject.SetActive(false);
        tint.SetActive(false);
        pauseMenuPopUp.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        ExitPauseMenu();
    }

    public void Graphics()
    {
        Debug.LogWarning("This feature is not implemented yet");
    }

}
