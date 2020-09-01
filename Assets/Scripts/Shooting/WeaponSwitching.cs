using UnityEngine;
using Rewired;
using FPS;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;

    [HideInInspector]
    public static GunTypes currentType;

    private Player _player;
    private FpsCustom _custom;

    #region |Setup
    private void Awake()
    {
        _custom = GetComponentInParent<FpsCustom>();
        _player = _custom._player;
    }

    private void Start()
    {
        SelectWeapon();
    }
    #endregion

    private void Update()
    {
        int prevSelection = selectedWeapon;

        #region |Debugging Checks
        if (_player == null)
            _player = _custom._player;
        #endregion

        #region |Scroll Wheel Changes
        //* Increase Selection
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        //* Decrease Selection
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
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
    }

    #region |Apply Selection
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                currentType = weapon.GetComponent<Gun>().gunType;
            }
            else
                weapon.gameObject.SetActive(false);

            i++;
        }
    }
    #endregion
}

[System.Serializable]
public enum GunTypes
{
    Rifle,
    Pistol,
    Shotgun,
    Sniper
}