using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using FPS;
using Sirenix.OdinInspector;
using TMPro;

public class GrenadeThrow : MonoBehaviour
{
    public float throwForce = 40;
    public Transform launchPoint;
    public FpsCustomNetworked _custom;
    [MinMaxSlider(0, 5, true)]
    public Vector2 holdMinMax;

    private Player _player;
    private float _holdValue;
    private bool _isHolding;

    public GrenadeInfo[] grenadeInfo;
    public TextMeshProUGUI grenadeCountText;
    public TextMeshProUGUI grenadeTypeText;

    private int _grenadeIndex = 0;

    private void Start()
    {
        _player = _custom._player;

        for (int i = 0; i < grenadeInfo.Length; i++)
        {
            grenadeInfo[i]._currentAmount = grenadeInfo[i].maxAmount;
        }

        UpdateUI();
    }

    private void Update()
    {
        if (!_custom.IsSetup)
            return;

        if (_player.GetButtonDown("Grenade"))
        {
            _isHolding = true;
        }
        else if (_player.GetButtonUp("Grenade"))
        {
            _isHolding = false;
            ThrowGrenade();
        }

        if (_player.GetButtonDown("CycleNades"))
        {
            if (_grenadeIndex >= grenadeInfo.Length - 1)
                _grenadeIndex = 0;
            else
                _grenadeIndex++;

            UpdateUI();
        }

        if (_isHolding && _holdValue < holdMinMax.y)
            _holdValue += Time.deltaTime;
    }

    void ThrowGrenade()
    {
        if (grenadeInfo[_grenadeIndex]._currentAmount <= 0)
            return;

        GameObject grenadeObject;
        GameObject grenade = grenadeInfo[_grenadeIndex].nadePrefab;

        if (launchPoint != null)
        {
            grenadeObject = Instantiate(grenade, launchPoint.position, launchPoint.rotation);
        }
        else
        {
            grenadeObject = Instantiate(grenade, transform.position, transform.rotation);
        }

        Rigidbody rb = grenadeObject.GetComponent<Rigidbody>();
        float holdDiv = _holdValue / holdMinMax.y;
        print(_holdValue);
        rb.AddForce(transform.forward * throwForce * holdDiv, ForceMode.VelocityChange);

        _holdValue = 0;

        grenadeInfo[_grenadeIndex]._currentAmount--;
        UpdateUI();
    }

    void UpdateUI()
    {
        //! grenadeTypeText.text = grenadeInfo[_grenadeIndex].grenadeType.ToString();
        //! grenadeCountText.text = grenadeInfo[_grenadeIndex]._currentAmount.ToString();
    }
}

[System.Serializable]
public struct GrenadeInfo
{
    public string name;
    public GrenadeTypes grenadeType;
    public int maxAmount;
    public GameObject nadePrefab;

    [HideInInspector]
    public int _currentAmount;
}