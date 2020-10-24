using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FPS.GameModes
{
    public class GM1v1 : MonoBehaviour
    {
        public List<DuelStats> players = new List<DuelStats>(2);

        public int killsToWin = 3;
        public int deathsToLose = 5;

        public GameObject victoryUi;
        public TextMeshProUGUI winText;
        public string winPrefix = " has won the game!!";

        private void Awake()
        {
            if (players.Count < 2)
                Debug.LogError("**ERROR** \nOne or more players could not be found by GM1v1!!");

            if (victoryUi == null)
                Debug.LogError("**ERROR** \nThe Victory UI element within GM1v1 has not been set!!");

            if (winText == null)
                Debug.LogError("**ERROR** \nThe Win Text element within GM1v1 has not been set!!");
        }

        public void AddKill(int _playerId)
        {
            // List<PlayerStats> tempList = new List<PlayerStats>(players);
            // tempList.Reverse();

            for (int i = 0; i < players.Count; i++)
            {
                if (i != _playerId)
                    players[i].kills++;
                else if (i == _playerId)
                    players[i].deaths++;
                else
                    Debug.LogWarning("**WARNING** \nSomething has possibly gone wrong within AddKill...");
            }
            CheckForWin(_playerId);
        }

        void CheckForWin(int _playerId)
        {
            bool someoneWon = false;

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].kills >= killsToWin)
                {
                    PlayerWin(players[i]);
                    someoneWon = true;
                }
            }

            if (players[0].deaths >= deathsToLose)
            {
                PlayerWin(players[1]);
                someoneWon = true;
            }
            else if (players[1].deaths >= deathsToLose)
            {
                PlayerWin(players[0]);
                someoneWon = true;
            }

            if (!someoneWon)
                StartCoroutine(RespawnPlayer(players[_playerId].custom.gameObject));
        }

        void PlayerWin(DuelStats _winner)
        {
            if (_winner == players[0])
                winText.text = "Player 1" + winPrefix;
            else if (_winner == players[1])
                winText.text = "Player 2" + winPrefix;

            victoryUi.SetActive(true);
            GameManager.gameOver = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public IEnumerator RespawnPlayer(GameObject _playerGO)
        {
            _playerGO.GetComponentInChildren<Target>().Reset();

            Gun[] tempGuns = _playerGO.GetComponentsInChildren<Gun>();
            foreach (Gun g in tempGuns)
            {
                g.Reset();
            }

            yield return new WaitForSeconds(2);

            _playerGO.SetActive(true);
        }
    }

    [System.Serializable]
    public class DuelStats
    {
        public FpsCustom custom;
        public int playerId;
        // [HideInInspector]
        public int kills = 0;
        // [HideInInspector]
        public int deaths = 0;
        public TeamStats team;
    }
}