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

        public Material redMat;
        public Material blueMat;


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
                    players[i].custom.gameObject.GetComponent<MeshRenderer>().material = redMat;
                }
                else
                {
                    players[i].team = Team.Blue;
                    blue.Add(players[i]);
                    players[i].custom.gameObject.GetComponent<MeshRenderer>().material = blueMat;
                }
            }

            if (players.Count < 2)
                Debug.LogError("**ERROR** \nOne or more players could not be found by GMTDM!!");

            if (victoryUi == null)
                Debug.LogError("**ERROR** \nThe Victory UI element within GM1v1 has not been set!!");

            if (winText == null)
                Debug.LogError("**ERROR** \nThe Win Text element within GM1v1 has not been set!!");
        }

        public void AddKill(FpsCustom _custom)
        {
            int _playerId = 69;
            for (int i = 0; i < players.Count; i++)
            {
                if (_custom == players[i].custom)
                {
                    _playerId = i;
                }
            }
            if (_playerId == 69)
                Debug.LogWarning("**WARNING** \nIt's possible something has gone wrong when getting the players ID within AddKill...");

            //*

            if (red.Contains(players[_playerId])) //if it was a red player that died
            {
                blueStats.runningKills++; //blue got a kill
                redStats.runningDeaths++; //red got a death
            }
            else if (blue.Contains(players[_playerId])) //if it was a blue player that died
            {
                redStats.runningKills++; //red got a kill
                blueStats.runningDeaths++; //blue got a death
            }
            else
            {
                Debug.LogWarning("**WARNING** \nSomething has possibly gone wrong within AddKill...");
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

            CheckForTeamWin(players[_playerId]);
        }

        void CheckForTeamWin(TDMStats _player)
        {
            if (redStats.runningKills >= teamKillsToWin)
            {
                TeamWin(Team.Red);
            }
            else if (blueStats.runningKills >= teamKillsToWin)
            {
                TeamWin(Team.Blue);
            }
            else if (redStats.runningDeaths >= teamDeathsToLose)
            {
                TeamWin(Team.Blue);
            }
            else if (blueStats.runningDeaths >= teamDeathsToLose)
            {
                TeamWin(Team.Red);
            }
            else
            {
                StartCoroutine(RespawnPlayer(_player.custom.gameObject));
            }
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