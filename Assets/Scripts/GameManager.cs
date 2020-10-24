using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameModes gameMode = GameModes.None;
    public static bool gamePaused = false;
    public static bool gameOver = false;

    public static void SetGameMode(string _modeName)
    {
        gameMode = (GameModes)Enum.Parse(typeof(GameModes), _modeName, true);
        print("Game Mode set to " + gameMode.ToString());
    }

    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void CycleMode()
    {
        int newMode = (int)GameManager.gameMode + 1;
        if (newMode >= System.Enum.GetValues(typeof(GameModes)).Length)
            newMode = 0;
        GameManager.gameMode = (GameModes)newMode;
        string modeValue = "MODE: " + System.Enum.GetName(typeof(GameModes), newMode); //*make this into a text element to display the cycling
    }
}

[System.Serializable]
public enum GameModes
{
    None = 0,
    Practice = 1,
    Duel = 2,
    Flag = 3,
    TDM = 4
}