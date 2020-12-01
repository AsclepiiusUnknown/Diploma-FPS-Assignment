using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS;
using TMPro;

public class MP1v1 : MonoBehaviour
{


    public PlayerStats player1;
    public PlayerStats player2;

    [Space]
    public int winningCount = 3;

    public GameObject victoryUI;
    public TextMeshProUGUI winText;
    [TextArea] public string winMessage = " has won the match!!";

    private void Start()
    {
        if (player1.custom == null || player2.custom == null)
        {
            Debug.LogError("**ERROR**\n Players are missing!!");
            return;
        }
    }

    bool PlayersActive(FpsCustomNetworked _player1, FpsCustomNetworked _player2)
    {
        if (!_player1.gameObject.activeSelf || !_player2.gameObject.activeSelf)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void PlayerDied(int _playerId)
    {
        // if (player id == player 1 id)
        victoryUI.SetActive(true);
        string winner = (player1.custom.gameObject.activeSelf) ? "Player 1" : "Player 2";
        winText.text = winner + winMessage;
        GameManager._gameOver = true;
    }

    // Event that fires when players die (playerId) -> Handler for both players, if (player == playerId) playerDeaths++ 
}