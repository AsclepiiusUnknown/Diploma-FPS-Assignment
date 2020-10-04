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
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager._gameOver)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        GameManager._gamePaused = !GameManager._gamePaused;
        Time.timeScale = (GameManager._gamePaused) ? 0 : 1;
        mouseLook.enabled = (GameManager._gamePaused) ? false : true;//?add bool in mouselook to control pauses
        gameUI.SetActive(GameManager._gamePaused);
        pauseUI.SetActive(GameManager._gamePaused);
    }

    public void ReturnToMenu()
    {
        TogglePause();
        GameManager.gameMode = GameModes.None;
        print("Game Mode reset");

        GameManager._gameOver = false;

        SceneManager.LoadScene(0); //!assuming scene 0 is always main menu
    }
}
