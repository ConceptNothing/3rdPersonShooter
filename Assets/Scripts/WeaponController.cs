using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private float animationPlayTransition = 0.1f;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletParent;
    [SerializeField]
    private float bulletHitMissDistance = 25f;
    [SerializeField]
    private AmmoAmountUI ammoUi;

    private Weapon currentWeapon;
    private PlayerInput playerInput;
    private InputAction shootAction;
    private WeaponSwitching weaponSwitch;
    private Transform barrelTransform;
    private Transform cameraTransform;
    private Animator animator;
    private int recoilAnimation;
    private float timeSinceLastShot;
    private bool isShooting;
    private InputAction reloadAction;
    private bool isReloading;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Shoot"];
        animator = GetComponent<Animator>();
        weaponSwitch = GetComponent<WeaponSwitching>();
        recoilAnimation = Animator.StringToHash("GunRecoilAnimation");
        cameraTransform = Camera.main.transform;
        currentWeapon = weaponSwitch.CurrentWeapon;
        ammoUi.Weapon = currentWeapon;
        reloadAction = playerInput.actions["Reload"];
    }

    private void OnEnable()
    {
        shootAction.started += _ => StartShooting();
        shootAction.canceled += _ => StopShooting();
        reloadAction.started += _ => Reload();
    }

    private void OnDisable()
    {
        shootAction.started -= _ => StartShooting();
        shootAction.canceled -= _ => StopShooting();
        reloadAction.started -= _ => Reload();
    }
    // Update is called once per frame
    void Update()
    {
        ammoUi.Weapon = currentWeapon;
        timeSinceLastShot += Time.deltaTime;
        barrelTransform = weaponSwitch.weapons[weaponSwitch.currentWeaponIndex].transform.Find("Barrel");
        currentWeapon = weaponSwitch.CurrentWeapon;

        if (isReloading)
        {
            isShooting = false;
        }
        if (isShooting && !isReloading && timeSinceLastShot >= currentWeapon.ShootingRate)
        {
            Shoot();
        }

        timeSinceLastShot += Time.deltaTime;
        barrelTransform = weaponSwitch.weapons[weaponSwitch.currentWeaponIndex].transform.Find("Barrel");
        currentWeapon = weaponSwitch.CurrentWeapon;
    }
    public void Shoot()
    {
        if (currentWeapon.CurrentClipAmmo > 0)
        {
            currentWeapon.CurrentClipAmmo--;
            RaycastHit hit;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.BulletDamage = currentWeapon.Damage;
            bulletController.BulletSpeed = currentWeapon.BulletSpeed;

            TrailRenderer trailRenderer = bullet.GetComponentInChildren<TrailRenderer>();
            trailRenderer.emitting = true;

            trailRenderer.time = 0.5f;
            trailRenderer.startWidth = 0.1f;
            trailRenderer.endWidth = 0.01f;

            //if there has been an target (ADD LAYER MASK IF NEEDED AT THE END OF THE Physics.Raycast)
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
            {
                bulletController.Target = hit.point;
                bulletController.Hit = true;
            }
            //if shot was made in the sky
            else
            {
                bulletController.Target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
                bulletController.Hit = false;
            }
            animator.CrossFade(recoilAnimation, animationPlayTransition);
            timeSinceLastShot = 0f;
        }
        else
        {
            Debug.LogError("NO AMMO! RELOAD!");
            Reload();
        }
    }

    private void Reload()
    {
        if (!isReloading && currentWeapon.CurrentClipAmmo < currentWeapon.ClipSize && currentWeapon.CurrentAmmo > 0)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }
    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        yield return new WaitForSeconds(currentWeapon.ReloadSpeed);

        int bulletsToReload = currentWeapon.ClipSize - currentWeapon.CurrentClipAmmo;
        if (bulletsToReload > currentWeapon.CurrentAmmo)
        {
            bulletsToReload = currentWeapon.CurrentAmmo;
        }

        currentWeapon.CurrentAmmo -= bulletsToReload;
        currentWeapon.CurrentClipAmmo += bulletsToReload;

        isReloading = false;
    }
    private void StartShooting()
    {
        isShooting = true;
        Shoot();
    }

    private void StopShooting()
    {
        isShooting = false;
    }
    public void AddAmmo(int amount)
    {
        currentWeapon.CurrentAmmo += amount;
        //In case if player gained more ammo from lootbox than max amount of ammo
        if (currentWeapon.CurrentAmmo > currentWeapon.MaxAmmoAmount)
        {
            currentWeapon.CurrentAmmo = currentWeapon.MaxAmmoAmount;
        }
    }
}
