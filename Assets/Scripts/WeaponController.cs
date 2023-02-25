using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    private PlayerInput playerInput;
    private InputAction shootAction;
    private WeaponSwitching weaponSwitch;
    private Transform barrelTransform;
    private Transform cameraTransform;
    private Animator animator;
    private int recoilAnimation;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Shoot"];
        animator = GetComponent<Animator>();
        weaponSwitch = GetComponent<WeaponSwitching>();
        recoilAnimation = Animator.StringToHash("GunRecoilAnimation");
        cameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        shootAction.performed += _ => Shoot();
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => Shoot();
    }
    // Update is called once per frame
    void Update()
    {
        barrelTransform = weaponSwitch.weapons[weaponSwitch.currentWeaponIndex].transform.Find("Barrel");
        int? log = weaponSwitch.currentWeaponIndex;
    }
    public void Shoot()
    {
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        //if there has been an target (ADD LAYER MASK IF NEEDED AT THE END OF THE Physics.Raycast)
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            var sw = hit.rigidbody;
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
    }
}
