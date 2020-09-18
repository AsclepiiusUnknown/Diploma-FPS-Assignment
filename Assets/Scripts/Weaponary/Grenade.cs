using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
public class Grenade : MonoBehaviour
{
    [EnumToggleButtons]
    public GrenadeTypes grenadeType;

    #region |Variables
    [Title("Variables:")]
    public GameObject explosionEffect;
    public float blastRadius = 5;
    public float force = 700;
    public float damage = 10;
    [HideIf("grenadeType", GrenadeTypes.Impact)] public float delay = 3;

    #region ||SFX
    [Title("Sound Effects:")]
    public string throwSound;
    public string delaySound;
    public string explosionSound;
    #endregion

    //*PRIVATE
    private float _countdown;
    private bool hasExploded = false;
    private Rigidbody _rb;
    private AudioManager audioManager;
    #endregion

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _countdown = delay;
        audioManager = AudioManager.instance;

        if (throwSound != "")
            audioManager.Play(throwSound);
        if (delaySound != "")
            audioManager.Play(delaySound);
    }

    private void Update()
    {
        #region |NOT FOR IMPACT NADES
        if (grenadeType == GrenadeTypes.Impact)
            return;

        _countdown -= Time.deltaTime;

        if (_countdown <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
        #endregion
    }

    public void Explode()
    {
        GameObject explosionObject = Instantiate(explosionEffect, transform.position, transform.rotation);

        if (explosionSound != "")
            audioManager.Play(explosionSound);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.gameObject == this.gameObject)
            {
                break;
            }

            if (grenadeType != GrenadeTypes.Stun)
            {
                if (nearbyObject.GetComponent<Target>() != null)
                {
                    nearbyObject.GetComponent<Target>().TakeDamage(damage);
                }

                if (nearbyObject.GetComponentInParent<Dummy>() != null)
                {
                    nearbyObject.GetComponentInParent<Dummy>().Damage(damage);
                }
            }

            if (nearbyObject.GetComponent<Rigidbody>() != null)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                rb.AddExplosionForce(force, transform.position, blastRadius);
            }

            // if (nearbyObject.GetComponent<Grenade>() != null)
            // {
            //     print("**CHAIN**");
            //     nearbyObject.GetComponent<Grenade>().Explode();
            // }
        }

        Destroy(explosionObject, 5);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (grenadeType == GrenadeTypes.Impact)
        {
            Explode();
        }
        else if (grenadeType == GrenadeTypes.Sticky)
        {
            transform.SetParent(other.transform);
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}

[System.Serializable]
public enum GrenadeTypes
{
    Frag,
    Impact,
    Sticky,
    Stun
}