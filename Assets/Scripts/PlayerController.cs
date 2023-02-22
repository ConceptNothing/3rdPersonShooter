using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float gravity=-9.81f;
    [SerializeField]
    private float jumpHeight=1.0f;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float playerSpeed=7f;
    [SerializeField]
    private float sprintSpeedMultiplier=1.5f;
    [SerializeField]
    private float maxStamina = 100f;
    [SerializeField]
    private float sprintStaminaDrain=20f;
    [SerializeField]
    private float staminaRegen=10f;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform barrelTransform;
    [SerializeField]
    private Transform bulletParent;
    [SerializeField]
    private float bulletHitMissDistance=25f;


    private Transform cameraTransform;
    private PlayerInput playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private InputAction moveAction;
    private InputAction mouseLookAction;
    private InputAction jumpAction;
    private InputAction aimAction;
    private InputAction sprintAction;
    private InputAction shootAction;
    private bool isSprinting;
    private float currentStamina;


    // Start is called before the first frame update
    void Awake()
    {
        controller=GetComponent<CharacterController>();
        playerInput=GetComponent<PlayerInput>();
        moveAction=playerInput.actions["Move"];
        mouseLookAction = playerInput.actions["MouseLook"];
        jumpAction = playerInput.actions["Jump"];
        aimAction = playerInput.actions["Aim"];
        sprintAction = playerInput.actions["Sprint"];
        shootAction = playerInput.actions["Shoot"];
        cameraTransform= Camera.main.transform;

        Cursor.lockState=CursorLockMode.Locked;
        currentStamina = maxStamina;
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
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input=moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;

        // Check if player is sprinting
        float sprintValue = sprintAction.ReadValue<float>();
        if (sprintValue > 0.1f && isGrounded && currentStamina>0)
        {
            isSprinting = true;
            currentStamina -= Time.deltaTime * sprintStaminaDrain;
            move *= sprintSpeedMultiplier;

            //stop sprint if stamina reaches < 0
            if (currentStamina < 0)
            {
                currentStamina = 0;
                isSprinting = false;
            }
        }
        else
        {
            isSprinting = false;
            currentStamina += Time.deltaTime * staminaRegen;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }


        move.y = 0f;
        controller.Move(move * Time.deltaTime * (isSprinting ? playerSpeed * sprintSpeedMultiplier : playerSpeed));

        if (jumpAction.triggered && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Towards cam direction
        Quaternion rotation=Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    public float GetStamina()
    {
        return currentStamina;
    }
    public float GetMaxStamina()
    {
        return maxStamina;
    }
    private void Shoot()
    {
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
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
    }
}
