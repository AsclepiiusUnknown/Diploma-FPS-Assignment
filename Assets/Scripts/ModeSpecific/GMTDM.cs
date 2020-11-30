using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS;

public class GMTDM : MonoBehaviour
{
    private List<FpsCustomNetworked> allPlayers;
    private List<FpsCustomNetworked> redTeam = new List<FpsCustomNetworked>();
    private List<FpsCustomNetworked> blueTeam = new List<FpsCustomNetworked>();

    private void Start()
    {
        allPlayers = new List<FpsCustomNetworked>(FindObjectsOfType<FpsCustomNetworked>());

        for (int i = 0; i < allPlayers.Count; i++)
        {
            FpsCustomNetworked temp = allPlayers[i];
            int randomIndex = Random.Range(i, allPlayers.Count);
            allPlayers[i] = allPlayers[randomIndex];
            allPlayers[randomIndex] = temp;
        }

        bool _red = true;
        for (int i = 0; i < allPlayers.Count; i++)
        {
            if (_red)
                redTeam.Add(allPlayers[i]);
            else
                blueTeam.Add(allPlayers[i]);

            _red = !_red;
        }
    }
}
