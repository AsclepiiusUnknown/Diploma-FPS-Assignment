using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FPS;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject gameUI;
    public FpsCustom mouseLook;

    private void Awake()
    {
        mouseLook = FindObjectOfType<FpsCustom>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.gameOver)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        GameManager.gamePaused = !GameManager.gamePaused;
        Time.timeScale = (GameManager.gamePaused) ? 0 : 1;
        mouseLook.mouseLook.enabled = (GameManager.gamePaused) ? false : true;//?add bool in mouselook to control pauses
        gameUI.SetActive(GameManager.gamePaused);
        pauseUI.SetActive(GameManager.gamePaused);
    }

    public void ReturnToMenu()
    {
        TogglePause();
        GameManager.gameMode = GameModes.None;
        print("Game Mode reset");

        GameManager.gameOver = false;

        SceneManager.LoadScene(0); //!assuming scene 0 is always main menu
    }
}
