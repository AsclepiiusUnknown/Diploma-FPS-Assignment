using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FPS;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector]
    public bool isPaused = false;

    public GameObject pauseUI;
    public GameObject gameUI;
    public FpsCustom mouseLook;

    private void Awake()
    {
        mouseLook = FindObjectOfType<FpsCustom>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = (isPaused) ? 0 : 1;
        mouseLook.enabled = (isPaused) ? false : true;//?add bool in mouselook to control pauses
        gameUI.SetActive(isPaused);
        pauseUI.SetActive(isPaused);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0); //!assuming scene 0 is always main menu
    }
}
