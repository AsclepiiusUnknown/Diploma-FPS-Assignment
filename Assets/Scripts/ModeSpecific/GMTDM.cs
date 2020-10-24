using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FPS.GameModes
{
    public class GMTDM : MonoBehaviour
    {
        public List<TDMStats> players;

        [Space]

        public List<TDMStats> red;
        public List<TDMStats> blue;

        public TeamStats redStats;
        public TeamStats blueStats;

        [Space]

        public int teamKillsToWin = 3;
        public int teamDeathsToLose = 5;

        public GameObject victoryUi;
        public TextMeshProUGUI winText;
        public string winPrefix = " Team has won the game!!";


        private void Start()
        {
            FpsCustom[] tempCustoms = FindObjectsOfType<FpsCustom>();
            players = new List<TDMStats>();
            float _defHalf = Mathf.Round(tempCustoms.Length / 2);

            for (int i = 0; i < tempCustoms.Length; i++)
            {
                players.Add(new TDMStats());

                players[i].playerId = i;
                players[i].custom = tempCustoms[i];

                if (i <= _defHalf)
                {
                    players[i].team = Team.Red;
                    red.Add(players[i]);
                }
                else
                {
                    players[i].team = Team.Blue;
                    blue.Add(players[i]);
                }
            }

            if (players.Count < 2)
                Debug.LogError("**ERROR** \nOne or more players could not be found by GMTDM!!");

            if (victoryUi == null)
                Debug.LogError("**ERROR** \nThe Victory UI element within GM1v1 has not been set!!");

            if (winText == null)
                Debug.LogError("**ERROR** \nThe Win Text element within GM1v1 has not been set!!");
        }

        public void AddKill(int _playerId)
        {
            for (int i = 0; i < players.Count; i++) //for all the players
            {
                if (i == _playerId) //if it was this index
                {
                    if (players[i].team == Team.Red) //if it was a red player that died
                    {
                        blueStats.runningKills++; //blue got a kill
                        redStats.runningDeaths++; //red got a death
                    }
                    else if (players[i].team == Team.Blue) //if it was a blue player that died
                    {
                        redStats.runningKills++; //red got a kill
                        blueStats.runningDeaths++; //blue got a death
                    }
                }
            }

            //* Adding personal kills
            /**
            // if (i != _playerId)
            //     red[i].killCount++;
            // else if (i == _playerId)
            //     red[i].deathCount++;
            // else
            //     Debug.LogWarning("**WARNING** \nSomething has possibly gone wrong within AddKill...");
            **/

            CheckForTeamWin(_playerId);
        }

        void CheckForTeamWin(int _playerId)
        {
            if (redStats.runningKills >= teamKillsToWin || blueStats.runningKills >= teamKillsToWin)
            {
                TeamWin(players[_playerId].team);
            }
            else if (redStats.runningDeaths >= teamDeathsToLose || blueStats.runningDeaths >= teamDeathsToLose)
            {
                if (players[_playerId].team == Team.Red)
                    TeamWin(Team.Blue);
                else if (players[_playerId].team == Team.Blue)
                    TeamWin(Team.Red);
            }
            else
                StartCoroutine(RespawnPlayer(players[_playerId].custom.gameObject));
        }

        void TeamWin(Team _winner)
        {
            winText.text = _winner + winPrefix;

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
    public enum Team
    {
        Red,
        Blue
    }

    [System.Serializable]
    public struct TeamStats
    {
        public int runningKills;
        public int runningDeaths;
    }

    [System.Serializable]
    public class TDMStats
    {
        public int playerId;
        public FpsCustom custom;
        public Team team;
        public int kills = 0;
        public int deaths = 0;
    }
}