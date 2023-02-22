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
    private float playerSpeed=2.0f; 

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

    // Start is called before the first frame update
    void Start()
    {
        controller=GetComponent<CharacterController>();
        playerInput=GetComponent<PlayerInput>();
        moveAction=playerInput.actions["Move"];
        mouseLookAction = playerInput.actions["MouseLook"];
        jumpAction = playerInput.actions["Jump"];
        aimAction = playerInput.actions["Aim"];
        sprintAction = playerInput.actions["Sprint"];
        shootAction = playerInput.actions["Shoot"];
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
