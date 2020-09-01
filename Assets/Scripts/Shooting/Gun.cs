using System.Collections;
using UnityEngine;
using Rewired;
using FPS;
using Sirenix.OdinInspector;

public class Gun : MonoBehaviour
{
    #region |Variables
    // * //
    #region ||Base
    [TabGroup("Base")]
    public GunTypes gunType;
    [TabGroup("Base")]
    public float damage = 10f;
    [TabGroup("Base")]
    public float range = 100f;
    [TabGroup("Base")]
    public bool automatic = false;
    [ShowIf("automatic"), TabGroup("Base")]
    public float fireRate = 15f;
    [TabGroup("Base")]
    public float impactForce = 30;
    [TabGroup("Base")]
    public Camera fpsCam;

    //*PRIVATE
    private Player _player;
    private FpsCustom _custom;
    private float _nextFire = 0;
    #endregion

    #region ||Effects
    [TabGroup("Effects")]
    public ParticleSystem muzzleFlash;
    [TabGroup("Effects")]
    public GameObject[] impactEffects;
    #endregion

    #region ||Ammo & Reload
    [TabGroup("Ammo & Reload")]
    public int maxAmmo = 10;
    [TabGroup("Ammo & Reload")]
    public float reloadTime = 1;
    [TabGroup("Ammo & Reload")]
    public float cooldownTime = .25f;
    [TabGroup("Ammo & Reload")]
    public Animator animator;

    //*PRIVATE
    private int _currentAmmo;
    private bool _isReloading;
    private bool _isCooling;
    #endregion
    #endregion


    #region |Setup
    private void Awake()
    {
        //initial setup for guns 
        _custom = GetComponentInParent<FpsCustom>();
        _currentAmmo = maxAmmo;
    }

    private void OnEnable()
    {
        //Bug fixes for changing weapons mid-animation
        _isReloading = false;
        animator.SetBool("Reloading", false);
    }
    #endregion

    private void Update()
    {
        #region |Checks
        //Reloading return
        if (_isReloading)
            return;

        //Cooldown return
        if (_isCooling)
            return;

        //Checking for auto-reload
        if (_currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        //Input Debugging
        if (_player == null)
        {
            _player = _custom._player;
        }
        #endregion

        #region |Shooting Input
        //automatic
        if (automatic && Input.GetButton("Fire1") && Time.time >= _nextFire)
        {
            _nextFire = Time.time + 1 / fireRate;
            Shoot();
        }
        //manual
        else if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        #endregion
    }

    #region |Reloading
    IEnumerator Reload()
    {
        _isReloading = true;
        print("Reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        _currentAmmo = maxAmmo;
        _isReloading = false;
    }
    #endregion

    #region |Cooldown
    IEnumerator Cooldown()
    {
        _isCooling = true;
        yield return new WaitForSeconds(cooldownTime);
        _isCooling = false;
    }
    #endregion

    #region |Shooting Functionality
    void Shoot()
    {
        //play muzzle flash effect
        muzzleFlash.Play();

        //play a gunshot sound
        AudioManager.instance.PlayGunshot(WeaponSwitching.currentType);

        //reduce ammo accordingly
        _currentAmmo--;

        //perform raycast check
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            //damage target if we hit one
            if (GetComponent<Target>() != null)
            {
                Target target = hit.transform.GetComponent<Target>();
                target.TakeDamage(damage);
            }

            //move object if it has a rigidbody
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            #region ||Impact Effects
            //check that we have impact effects
            if (impactEffects.Length <= 0)
                return;
            //randomise effect choice
            int randomImpact = Random.Range(0, impactEffects.Length);

            //create and destroy impact effect
            GameObject impactObject = Instantiate(impactEffects[randomImpact], hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactObject, 5);
            #endregion
        }

        //Start Cooldown
        StartCoroutine(Cooldown());
    }
    #endregion
}
