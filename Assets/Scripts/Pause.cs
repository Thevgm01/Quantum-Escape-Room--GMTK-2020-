using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;

    PlayerController player;
    public static bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void PauseGame(bool showMenu)
    {
        if (showMenu)
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        player.SetMovement(false);
        player.SetLook(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void UnpauseGame()
    {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.SetMovement(true);
        player.SetLook(true);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
