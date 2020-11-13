using System.Collections;
using FPS;
using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    #region |Variables
    // * //
    #region ||Base
    [TabGroup("Base")]
    /// <summary>
    /// Using the #GunTypes enum this variable specifies the type of gun it is in order to allow for type-specific functionality.
    /// </summary>
    public GunTypes gunType;
    [TabGroup("Base")]
    [SerializeField] private float damage = 10f;
    [TabGroup("Base")]
    [SerializeField] private float range = 100f;
    [TabGroup("Base")]
    [SerializeField] private bool automatic = false;
    [ShowIf("automatic"), TabGroup("Base")]
    [SerializeField] private float fireRate = 15f;
    [TabGroup("Base")]
    [SerializeField] private float impactForce = 30;
    [TabGroup("Base")]
    [SerializeField] private Camera mainCam;
    [TabGroup("Base")]
    [SerializeField] private GameObject weaponCam;

    //*PRIVATE
    private Player _player;
    private FpsCustom _custom;
    private float _nextFire = 0;
    #endregion

    #region ||Effects
    [TabGroup("Effects")]
    [SerializeField] private Transform muzzlePoint;
    [TabGroup("Effects")]
    [SerializeField] private GameObject muzzleFlash;
    [TabGroup("Effects")]
    [SerializeField] private GameObject[] impactEffects;
    #endregion

    #region ||Ammo & Reload
    [TabGroup("Ammo & Reload")]
    [SerializeField] private int currentAmmo = 10;
    [TabGroup("Ammo & Reload")]
    [SerializeField] private int rounds = 3;
    [TabGroup("Ammo & Reload")]
    [SerializeField] private float reloadTime = 1;
    [TabGroup("Ammo & Reload")]
    [SerializeField] private float cooldownTime = .25f;
    /// <summary>
    /// A reference to this guns animator component within the Unity editor to allow for each gun to have its own animations whilst still having a simple system for calling each animation using the same tiggers, bools, etc.
    /// </summary>
    [TabGroup("Ammo & Reload")]
    public Animator animator;
    [TabGroup("Ammo & Reload")]
    [SerializeField] private string reloadSound;
    [TabGroup("Ammo & Reload")]
    [SerializeField] private string cooldownSound;

    //*PRIVATE
    private int _currentAmmo;
    private int _totalAmmo;
    private bool _isReloading;
    private bool _isCooling;
    #endregion

    #region ||HUD
    [TabGroup("HUD")]
    [SerializeField] private TextMeshProUGUI currentAmmoText;
    [TabGroup("HUD")]
    [SerializeField] private TextMeshProUGUI totalAmmoText;
    #endregion

    #endregion

    #region |Setup
    private void Awake()
    {
        //initial setup for guns 
        _custom = GetComponentInParent<FpsCustom>();
        _currentAmmo = currentAmmo;
        _totalAmmo = currentAmmo * rounds;

        if (mainCam == null)
            mainCam = Camera.main;
    }

    private void OnEnable()
    {
        //Bug fixes for changing weapons mid-animation
        _isReloading = false;
        _isCooling = false;
        animator.SetBool("Reloading", false);

        AmmoCountUpdate();
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
        if (_currentAmmo <= 0 && _totalAmmo > 0)
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
        if (automatic && (Input.GetButton("Fire1") || _player.GetButton("Shoot")) && Time.time >= _nextFire)
        {
            _nextFire = Time.time + 1 / fireRate;
            Shoot();
        }
        //manual
        else if (Input.GetButtonDown("Fire1") || _player.GetButtonDown("Shoot"))
        {
            Shoot();
        }
        #endregion

        #region |Manual Reload Input
        if (_player.GetButtonDown("Reload") && _currentAmmo < currentAmmo)
            StartCoroutine(Reload());
        #endregion
    }

    #region |Reloading
    IEnumerator Reload()
    {
        if (_totalAmmo <= 0)
            StopCoroutine(Reload());

        _isReloading = true;
        print("Reloading...");

        if (reloadSound != "")
            AudioManager.instance.Play(reloadSound);
        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        if (_totalAmmo >= currentAmmo)
        {
            _currentAmmo = currentAmmo;
        }
        else
        {
            _currentAmmo = _totalAmmo;
        }

        _isReloading = false;

        AmmoCountUpdate();
    }
    #endregion

    #region |Cooldown
    IEnumerator Cooldown()
    {
        _isCooling = true;
        if (cooldownSound != "")
            AudioManager.instance.Play(cooldownSound);
        animator.SetTrigger("Cooling");
        yield return new WaitForSeconds(cooldownTime + .25f);
        _isCooling = false;
    }
    #endregion

    #region |Shooting Functionality
    void Shoot()
    {
        if (_currentAmmo <= 0)
            return;

        //play muzzle flash effect
        GameObject muzzleObject = Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
        Destroy(muzzleObject, 5);

        //play a gunshot sound
        AudioManager.instance.PlayGunshot(WeaponHandler.currentType);

        //reduce ammo accordingly
        _currentAmmo--;
        _totalAmmo--;

        CheckHits();

        AmmoCountUpdate();

        //Start Cooldown
        StartCoroutine(Cooldown());
    }

    void CheckHits()
    {
        //perform raycast check
        RaycastHit _hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out _hit, range))
        {
            //*TARGET
            if (_hit.transform.GetComponent<Target>() != null)
            {
                Target target = _hit.transform.GetComponent<Target>();
                target.TakeDamage(damage);
            }

            //*RIGIDBODY
            if (_hit.rigidbody != null)
            {
                _hit.rigidbody.AddForce(-_hit.normal * impactForce);
            }

            //*GRENADE
            if (_hit.transform.GetComponent<Grenade>() != null)
            {
                Grenade nade = _hit.transform.GetComponent<Grenade>();
                nade.Explode();
            }

            //*DUMMY
            if (_hit.transform.GetComponentInParent<Dummy>() != null)
            {
                bool _isHeadshot = (_hit.transform.gameObject.name == "Head") ? true : false;
                _hit.transform.GetComponentInParent<Dummy>().Damage(damage, _isHeadshot);
                print("Hit dummy");
            }

            //*LIGHTBULB
            if (_hit.transform.GetComponent<Lightbulb>() != null)
            {
                Lightbulb _bulb = _hit.transform.GetComponent<Lightbulb>();
                _bulb.Explode();
            }

            ImpactEffects(_hit);
        }
    }

    void ImpactEffects(RaycastHit _hit)
    {
        //check that we have impact effects
        if (impactEffects.Length <= 0)
            return;
        //randomise effect choice
        int randomImpact = Random.Range(0, impactEffects.Length);

        //create and destroy impact effect
        GameObject impactObject = Instantiate(impactEffects[randomImpact], _hit.point, Quaternion.LookRotation(_hit.normal));
        Destroy(impactObject, 5);
    }
    #endregion

    #region |Text Element Updating
    void AmmoCountUpdate()
    {
        #region ||Current Ammo
        if (currentAmmoText == null)
        {
            print("**NULL**");
            return;
        }
        else
        {
            currentAmmoText.text = _currentAmmo.ToString();
        }
        #endregion

        #region ||Total Ammo
        if (totalAmmoText == null)
        {
            print("**NULL**");
            return;
        }
        else
        {
            totalAmmoText.text = _totalAmmo.ToString();
        }
        #endregion
    }
    #endregion
}