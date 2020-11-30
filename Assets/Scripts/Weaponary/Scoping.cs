using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Sirenix.OdinInspector;
using FPS;

[RequireComponent(typeof(Gun))]
public class Scoping : MonoBehaviour
{
    #region |Variables
    // * //
    #region ||General
    [TabGroup("General")]
    public GameObject crosshair;

    //*PRIVATE
    [HideInInspector]
    public bool isScoping = false;
    private bool wasScoping = false;
    private float ogFOV;
    #endregion

    #region ||Snipers Only
    [TabGroup("Snipers Only")]
    public static GameObject scopeOverlay;
    [TabGroup("Snipers Only")]
    public float scopedFOV = 15;
    [TabGroup("Snipers Only")]
    public Camera mainCam;
    [TabGroup("Snipers Only")]
    public GameObject weaponCam;
    #endregion

    #region ||Refrences
    private Gun _gun;
    private FpsCustomNetworked _custom;
    private Player _player;
    #endregion
    #endregion

    #region |Setup
    public void Awake()
    {
        _gun = GetComponent<Gun>();
        ogFOV = Camera.main.fieldOfView;
        _custom = GetComponentInParent<FpsCustomNetworked>();
    }

    private void Start()
    {
        _player = _custom._player;
    }
    #endregion

    #region |Scoping Input 
    public void Update()
    {
        if (this.enabled && MouseLook._isScoping != this.isScoping)
            MouseLook._isScoping = this.isScoping;

        if (scopeOverlay != null)
        {
            if (scopeOverlay.activeSelf && (WeaponHandler.currentType != GunTypes.Sniper || !isScoping))
                scopeOverlay.SetActive(false);
        }


        if (Input.GetMouseButtonDown(1) || _player.GetButtonDown("Scope"))
            isScoping = true;
        else if (Input.GetMouseButtonUp(1) || _player.GetButtonUp("Scope"))
            isScoping = false;

        if (isScoping && !wasScoping)
        {
            print("entered scoping");
            UpdateScoping(true);
        }
        else if (!isScoping && wasScoping)
        {
            print("exited scoping");
            UpdateScoping(false);
        }
        // if (_isScoping && !_wasScoping)
        // {
        //     UpdateScoping(true);
        // }
        // else if (!_isScoping && _wasScoping)
        // {
        //     UpdateScoping(false);
        // }
        // else if (_isScoping && !_custom._isWalking)
        // {
        //     UpdateScoping(false);
        // }
        // else
        // {
        //     UpdateScoping(_isScoping);
        // }

        wasScoping = isScoping;
    }
    #endregion

    #region |Scoping
    void UpdateScoping(bool _isScoping)
    {
        isScoping = _isScoping;
        _gun.animator.SetBool("Scoping", _isScoping);
        _custom._canRun = !_isScoping;

        if (crosshair == null)
        {
            crosshair = GetComponentInParent<WeaponHandler>().crosshair;
        }

        crosshair.SetActive(!_isScoping);

        if (WeaponHandler.currentType == GunTypes.Sniper)
        {
            if (scopeOverlay == null)

                if (_isScoping)
                    StartCoroutine(SniperScopeIn());
                else
                    SniperScopeOut();
        }
    }

    #region ||Sniper Overlay
    IEnumerator SniperScopeIn()
    {
        yield return new WaitForSeconds(.5f); //delay

        scopeOverlay.SetActive(true); //overaly
        weaponCam.SetActive(false); //weapon rendering
        mainCam.fieldOfView = scopedFOV; //FOV / Zoom
    }

    void SniperScopeOut()
    {
        mainCam.fieldOfView = ogFOV; //FOV / Zoom
        weaponCam.SetActive(true); //weapon rendering
        scopeOverlay.SetActive(false); //overaly
    }
    #endregion
    #endregion
}
