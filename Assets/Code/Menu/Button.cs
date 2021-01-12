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

            case "Back":
                pauseMenu.Back();
                break;

            case "Set1":
                pauseMenu.SetTexture(0);
                break;

            case "Set2":
                pauseMenu.SetTexture(1);
                break;

            case "Set3":
                pauseMenu.SetTexture(2);
                break;

            case "Set4":
                pauseMenu.SetTexture(3);
                break;
        }
    }
}
