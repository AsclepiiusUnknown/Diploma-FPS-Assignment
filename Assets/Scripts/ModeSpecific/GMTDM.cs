using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FPS;

public class GMTDM : MonoBehaviour
{
    private List<FpsCustom> allPlayers;
    private List<FpsCustom> redTeam = new List<FpsCustom>();
    private List<FpsCustom> blueTeam = new List<FpsCustom>();

    private void Start()
    {
        allPlayers = new List<FpsCustom>(FindObjectsOfType<FpsCustom>());

        for (int i = 0; i < allPlayers.Count; i++)
        {
            FpsCustom temp = allPlayers[i];
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
