using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    #region Game mode
    [SerializeField] int playersTeamID;
    public int teamID { get { return playersTeamID; } }

    Rigidbody playerRigidbody;
    #endregion

    #region weapons
    public List<Weapon> weapons;
    int currentWeapon = 0;
    int lastWeapon = 0;
    public Vector2 dropOffset;

    #endregion

    void Start()
    {
        SwitchWeapon(currentWeapon, true);
    }

    public void SwitchWeapon(int weaponID, bool overrideLock = false)
    {
        if (!overrideLock && weapons[currentWeapon].isWeaponLocked == true)
        {
            return;
        }

        lastWeapon = currentWeapon;
        currentWeapon = weaponID;

        weapons[lastWeapon].gameObject.SetActive(false);
        weapons[currentWeapon].gameObject.SetActive(true);
    }

    public void PickUpWeapon(GameObject weaponObject, Vector3 ogLocation, int teamID, int weaponID, bool overrideLock = false)
    {
        SwitchWeapon(weaponID, overrideLock);

        weapons[weaponID].SetUp(teamID, weaponObject, ogLocation);
    }
}