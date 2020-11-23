using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameModes gameMode = GameModes.None;
    public static bool _gamePaused = false;
    public static bool _gameOver = false;
    public static LoadoutTypes loadout = LoadoutTypes.Assault;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public static void SetGameMode(string _modeName)
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