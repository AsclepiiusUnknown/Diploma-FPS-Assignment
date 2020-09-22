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
    [TabGroup("General")]
    private Camera mainCam;
    [TabGroup("General")]
    private GameObject weaponCam;

    //*PRIVATE
    [HideInInspector]
    public bool _isScoping = false;
    private bool _wasScoping = false;
    private float ogFOV;
    #endregion

    #region ||Snipers Only
    [TabGroup("Snipers Only")]
    public GameObject scopeOverlay;
    [TabGroup("Snipers Only")]
    public float scopedFOV = 15;
    #endregion

    #region ||Refrences
    private Gun _gun;
    private FpsCustom _custom;
    private Player _player;
    #endregion
    #endregion

    #region |Setup
    public void Awake()
    {
        _gun = GetComponent<Gun>();
        ogFOV = Camera.main.fieldOfView;
        _custom = GetComponentInParent<FpsCustom>();
    }

    private void Start()
    {
        _player = _custom._player;
    }
    #endregion

    #region |Scoping Input 
    public void Update()
    {
        if (this.enabled && MouseLook._isScoping != this._isScoping)
            MouseLook._isScoping = this._isScoping;

        if (scopeOverlay.activeSelf && (WeaponHandler.currentType != GunTypes.Sniper || !_isScoping))
            scopeOverlay.SetActive(false);

        if (Input.GetMouseButtonDown(1) || _player.GetButtonDown("Scope"))
            _isScoping = true;
        else if (Input.GetMouseButtonUp(1) || _player.GetButtonUp("Scope"))
            _isScoping = false;

        if (_isScoping && !_wasScoping)
        {
            UpdateScoping(true);
        }
        else if (!_isScoping && _wasScoping)
        {
            UpdateScoping(false);
        }
        else if (_isScoping && !_custom._isWalking)
        {
            UpdateScoping(false);
        }
        else
        {
            UpdateScoping(_isScoping);
        }

        _wasScoping = _isScoping;
    }
    #endregion

    #region |Scoping
    void UpdateScoping(bool isScoping)
    {
        _isScoping = isScoping;
        _gun.animator.SetBool("Scoping", isScoping);
        _custom._canRun = !isScoping;
        crosshair.SetActive(!isScoping);

        if (WeaponHandler.currentType == GunTypes.Sniper && isScoping)
        {
            if (isScoping)
            {
                StartCoroutine(OnSniperScoped());
            }
            else
            {
                scopeOverlay.SetActive(false);
                weaponCam.SetActive(true);
                mainCam.fieldOfView = ogFOV;
            }
        }
    }

    #region ||Sniper Overlay
    IEnumerator OnSniperScoped()
    {
        yield return new WaitForSeconds(.5f);

        scopeOverlay.SetActive(true);
        weaponCam.SetActive(false);
        mainCam.fieldOfView = scopedFOV;
    }
    #endregion
    #endregion
}
