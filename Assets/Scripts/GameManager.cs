using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameModes gameMode = GameModes.None;

    public void SetGameMode(string _modeName)
    {
        gameMode = (GameModes)Enum.Parse(typeof(GameModes), _modeName, true);
        print("Game Mode set to " + gameMode.ToString());
    }

    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}

[System.Serializable]
public enum GameModes
{
    Practice,
    Duel,
    Flag,
    TDM,
    None
}