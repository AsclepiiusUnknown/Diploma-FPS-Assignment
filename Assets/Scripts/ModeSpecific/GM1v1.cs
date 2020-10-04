using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS;
using TMPro;

public class GM1v1 : MonoBehaviour
{
    public FpsCustom player1;
    public FpsCustom player2;

    public GameObject victoryUI;
    public TextMeshProUGUI winText;
    [TextArea]
    public string winMessage = " has won the match!!";

    private void Start()
    {
        FpsCustom[] _players = FindObjectsOfType<FpsCustom>();

        if (_players.Length > 2)
        {
            Debug.LogError("**ERROR**\nThere are too many players in the game for the 1v1 mode to operate properly!!");
            return;
        }

        for (int i = 0; i < _players.Length; i++)
        {
            if (_players[i].gameObject.name == "Player1")
            {
                player1 = _players[i];
            }
            else if (_players[i].gameObject.name == "Player2")
            {
                player2 = _players[i];
            }
            else
            {
                Debug.LogError("**ERROR**\nIncorrect player naming converntions (check here)!!");
            }
        }
    }

    private void Update()
    {
        if (!PlayersActive(player1, player2))
        {
            victoryUI.SetActive(true);
            string winner = (player1.gameObject.activeSelf) ? "Player 1" : "Player 2";
            winText.text = winner + winMessage;
            GameManager._gameOver = true;
        }
    }

    bool PlayersActive(FpsCustom _player1, FpsCustom _player2)
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
}
