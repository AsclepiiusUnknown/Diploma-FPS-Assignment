using UnityEngine;
using Rewired;
using UnityEngine.UI;
using FPS;
using System.Collections.Generic;
using TMPro;

public class WeaponHandler : MonoBehaviour
{
    #region |Variables
    // * //
    #region ||Global
    [HideInInspector]
    public static GunTypes currentType;
    #endregion

    #region ||General
    [SerializeField] private int selectedWeapon = 0;
    // [SerializeField] private Image[] gunIcons;
    #endregion

    #region ||References
    private Player _player;
    private FpsCustomNetworked _custom;
    #endregion

    #region ||Flash Light
    [SerializeField] private GameObject flashLight;
    #endregion
    #endregion

    #region ||Loadouts
    public List<Loadout> loadouts = new List<Loadout>();
    #endregion

    #region ||Gun HUD
    public TextMeshProUGUI currentAmmoText;
    public TextMeshProUGUI totalAmmoText;
    public GameObject crosshair;
    #endregion

    #region |Setup
    private void Awake()
    {
        //Setup automated refrences
        _custom = GetComponentInParent<FpsCustomNetworked>();
    }

    private void Start()
    {
        SelectLoadout();

        //Select first weapon to start
        SelectWeapon();

        //This setup is done in start to ensure the custom script has completed is awake method
        _player = _custom._player;
    }
    #endregion

    private void Update()
    {
        if (!_custom.IsSetup)
            return;

        //Our previous selection of weapon is what we have at the start of each frame
        int prevSelection = selectedWeapon;

        #region |ScrollWheel & D-Pad
        //* Increase Selection
        if (GetScrollInputs() > 0)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        //* Decrease Selection
        else if (GetScrollInputs() < 0)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }
        #endregion

        #region |Number Changes
        if (Input.GetKeyDown(KeyCode.Alpha1) && transform.childCount >= 1)
            selectedWeapon = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
            selectedWeapon = 1;

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
            selectedWeapon = 2;

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
            selectedWeapon = 3;

        if (Input.GetKeyDown(KeyCode.Alpha5) && transform.childCount >= 5)
            selectedWeapon = 4;
        #endregion

        if (prevSelection != selectedWeapon)
            SelectWeapon();

        if (_player.GetButtonDown("Toggle Light"))
            ToggleLight();
    }

    /// <summary>
    /// Public method used to get weapon-switching inputs from the scrollwheel.
    /// </summary>
    /// <returns>Returns either -1, 0, or 1. These are then added to the current weapon index to change said index accordingly.</returns>
    int GetScrollInputs()
    {
        if (_player == null)
            _player = _custom._player;

        int input = 0;

        input = (Input.GetAxis("Mouse ScrollWheel") < 0) ? -1 : ((Input.GetAxis("Mouse ScrollWheel") > 0) ? 1 : input);

        input = (_player.GetButtonDown("Switch Down")) ? -1 : ((_player.GetButtonDown("Switch Up")) ? 1 : input);

        return input;
    }

    #region |Apply Selection
    /// <summary>
    /// Used to select the weapon using the variable \link #selectedWeapon selectedWeapon.
    /// </summary>
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                currentType = weapon.GetComponent<Gun>().gunType;
                // gunIcons[i].color = Color.white;
            }
            else
            {
                weapon.gameObject.SetActive(false);
                // gunIcons[i].color = new Color32(178, 178, 178, 255);
            }

            i++;
        }
    }
    #endregion

    #region |Loadout Selection
    void SelectLoadout()
    {
        if (loadouts == null || loadouts.Count <= 0)
        {
            Debug.LogError("**ERROR**\nNo loadouts set in the weapon handler!!");
            return;
        }

        //*Get Correct Loadout
        Loadout tempLoadout = loadouts[0];
        for (int i = 0; i < loadouts.Count; i++)
        {
            if (loadouts[i].loadoutType == GameManager.loadout)
            {
                tempLoadout = loadouts[i];
            }
        }

        //*Destroy old children (Peter Pan Style)
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        //*Make New Primary
        Vector3 tempPos = transform.position + tempLoadout.primaryWpn.transform.position;
        Quaternion tempRot = tempLoadout.primaryWpn.transform.rotation;
        Instantiate(tempLoadout.primaryWpn, tempPos, tempRot, transform);

        //*Make New Secondary
        tempPos = transform.position + tempLoadout.secondaryWpn.transform.position;
        tempRot = tempLoadout.secondaryWpn.transform.rotation;
        Instantiate(tempLoadout.secondaryWpn, tempPos, tempRot, transform);
    }
    #endregion

    void ToggleLight()
    {
        flashLight.SetActive(!flashLight.activeSelf);
    }
}

/// <summary>
/// An enumarator containing the possible options for types of guns including Rifle, Pistol, Shotgun, and Sniper.
/// </summary>
[System.Serializable]
public enum GunTypes
{
    Rifle,
    Pistol,
    Shotgun,
    Sniper
}

[System.Serializable]
public struct Loadout
{
    public LoadoutTypes loadoutType;
    // public Vector3 offsetPos;
    public GameObject primaryWpn;
    public GameObject secondaryWpn;
}