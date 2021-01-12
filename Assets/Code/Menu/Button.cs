using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    Menu pauseMenu;

    void Start()
    {
        pauseMenu = GameObject.Find("ExtraCode").GetComponentInChildren<Menu>();
    }

    private void OnMouseDown()
    {
        switch (name)
        {
            case "Resume":
                pauseMenu.ExitPauseMenu();
                break;

            case "Graphics":
                pauseMenu.Graphics();
                break;

            case "Reset":
                pauseMenu.RestartGame();
                pauseMenu.ExitPauseMenu();
                break;

            case "Exit Application":
                pauseMenu.QuitGame();
                break;
        }
    }
}
