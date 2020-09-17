using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] int teamID;
    public Vector3 ogLocation;

    const int weaponID = 1;

    void Start()
    {
        ogLocation = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        MyPlayer player = other.GetComponent<MyPlayer>();

        if (player != null)
        {
            if (player.teamID == teamID)
                return;

            print("Capture Flag!!");

            player.PickUpWeapon(gameObject, ogLocation, teamID, weaponID);

            gameObject.SetActive(false);
        }
    }
}