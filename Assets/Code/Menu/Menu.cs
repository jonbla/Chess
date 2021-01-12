using System.Collections;
using System.Collections.Generic;
using System;
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
    public GameObject graphicsMenuPopUp;

    [SerializeField]
    public bool inPause;

    public ChessSet[] sets;

    [Serializable]
    public struct ChessTeam
    {
        public Sprite Pawn;
        public Sprite Rook;
        public Sprite Horse;
        public Sprite Bishop;
        public Sprite King;
        public Sprite Queen;
    }

    [Serializable]
    public struct ChessSet
    {
        public ChessTeam Black;
        public ChessTeam White;
    }

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
        graphicsMenuPopUp.SetActive(false);
    }

    public void ExitPauseMenu()
    {
        inPause = false;
        mainCamera.gameObject.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = false;
        altCamera.gameObject.SetActive(false);
        tint.SetActive(false);
        pauseMenuPopUp.SetActive(false);
        graphicsMenuPopUp.SetActive(false);
    }

    public void QuitGame()
    {
        Feedback.SetText("Quiting please wait....");
        Application.Quit();
    }

    public void ResumeGame()
    {
        ExitPauseMenu();
    }

    public void Graphics()
    {
        pauseMenuPopUp.SetActive(false);
        graphicsMenuPopUp.SetActive(true);
    }

    public void Back()
    {
        pauseMenuPopUp.SetActive(true);
        graphicsMenuPopUp.SetActive(false);
    }

    public void SetTexture(int index)
    {
        foreach (Transform colour in GameObject.Find("Board").transform.Find("Pieces").transform)
        {
            if(colour.name == "Black")
            {
                foreach (Transform piece in colour)
                {
                    if (piece.CompareTag("Pawn"))
                    {
                        piece.GetComponent<SpriteRenderer>().sprite = sets[index].Black.Pawn;
                    }
                    if (piece.CompareTag("Rook"))
                    {
                        piece.GetComponent<SpriteRenderer>().sprite = sets[index].Black.Rook;
                    }
                    if (piece.CompareTag("Horse"))
                    {
                        piece.GetComponent<SpriteRenderer>().sprite = sets[index].Black.Horse;
                    }
                    if (piece.CompareTag("Bishop"))
                    {
                        piece.GetComponent<SpriteRenderer>().sprite = sets[index].Black.Bishop;
                    }
                    if (piece.CompareTag("King"))
                    {
                        piece.GetComponent<SpriteRenderer>().sprite = sets[index].Black.King;
                    }
                    if (piece.CompareTag("Queen"))
                    {
                        piece.GetComponent<SpriteRenderer>().sprite = sets[index].Black.Queen;
                    }
                }
            }
            else
            {
                foreach (Transform piece in colour)
                {
                    if (piece.CompareTag("Pawn"))
                    {
                        piece.GetComponent<SpriteRenderer>().sprite = sets[index].White.Pawn;
                    }
                    if (piece.CompareTag("Rook"))
                    {
                        piece.GetComponent<SpriteRenderer>().sprite = sets[index].White.Rook;
                    }
                    if (piece.CompareTag("Horse"))
                    {
                        piece.GetComponent<SpriteRenderer>().sprite = sets[index].White.Horse;
                    }
                    if (piece.CompareTag("Bishop"))
                    {
                        piece.GetComponent<SpriteRenderer>().sprite = sets[index].White.Bishop;
                    }
                    if (piece.CompareTag("King"))
                    {
                        piece.GetComponent<SpriteRenderer>().sprite = sets[index].White.King;
                    }
                    if (piece.CompareTag("Queen"))
                    {
                        piece.GetComponent<SpriteRenderer>().sprite = sets[index].White.Queen;
                    }
                }
            }

            
        }
    }

    public void RestartGame()
    {
        Feedback.SetText("Reseting...");
        main.Reset();
    }

}
