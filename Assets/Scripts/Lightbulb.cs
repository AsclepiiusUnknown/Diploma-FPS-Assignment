using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Lightbulb : MonoBehaviour
{
    public GameObject destroyEffect;
    private Light thisLight;

    private void Awake()
    {
        if (GetComponent<Light>() == null)
        {
            Debug.LogError("**ERROR**\nThere is no light on this object!");
            return;
        }
        thisLight = GetComponent<Light>();
    }

    private void Start()
    {
        if (destroyEffect == null)
        {
            Debug.LogError("**ERROR**\nThe destroy effect for this lightbulb is null.");
        }
    }

    public void Explode()
    {
        thisLight.intensity *= 2;
        GameObject _effectGO = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(_effectGO, 5);
        Destroy(gameObject);
    }
}
