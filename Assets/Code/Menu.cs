using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    Main main;

    public Camera mainCamera;
    public Camera altCamera;
    public GameObject tint;
    public GameObject pauseMenuPopUp;

    public bool inPause;

    // Start is called before the first frame update
    void Start()
    {
        ExitPauseMenu();
        main = GameObject.Find("MainCode").GetComponent<Main>();
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
        Feedback.SetText("Quiting....");
        Application.Quit();
    }

    public void ResumeGame()
    {
        ExitPauseMenu();
    }

    public void Graphics()
    {
        Debug.LogError("This feature is not implemented yet");
    }

    public void RestartGame()
    {
        Feedback.SetText("Reseting...");
        main.Reset();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
