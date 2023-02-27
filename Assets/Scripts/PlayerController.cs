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
    private Transform aimTarget;
    [SerializeField]
    private float aimDistance=1.1f;

    private Transform cameraTransform;
    private PlayerInput playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded; 
    private InputAction moveAction;
    //private InputAction mouseLookAction;
    private InputAction jumpAction;
    private InputAction aimAction;
    private InputAction sprintAction;
    private bool isSprinting;
    private float currentStamina;


    private Animator animator;
    private int moveXAnimationParameterId;
    private int moveZAnimationParameterId;
    Vector2 currentAnimationBlend;
    Vector2 animationVelocity;
    [SerializeField]
    private float animationSmoothTime=0.1f;
    private int jumpAnimation;
    [SerializeField]
    private float animationPlayTransition=0.1f;

    

    // Start is called before the first frame update
    void Awake()
    {
        controller=GetComponent<CharacterController>();
        playerInput=GetComponent<PlayerInput>();
        moveAction=playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];
        cameraTransform= Camera.main.transform;
        aimAction = playerInput.actions["Aim"];
        Cursor.lockState=CursorLockMode.Locked;
        currentStamina = maxStamina;

        //Animations
        animator = GetComponent<Animator>();
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");
        jumpAnimation = Animator.StringToHash("Pistol Jump");
    }

    // Update is called once per frame
    void Update()
    {
        aimTarget.position=cameraTransform.position+cameraTransform.forward*aimDistance;

        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlend = Vector2.SmoothDamp(currentAnimationBlend,input,ref animationVelocity,animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlend.x, 0, currentAnimationBlend.y);
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

        //BLEND Strafe animation
        animator.SetFloat(moveXAnimationParameterId,currentAnimationBlend.x);
        animator.SetFloat(moveZAnimationParameterId, currentAnimationBlend.y);

        if (jumpAction.triggered && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);

            //Jumping animation
            animator.CrossFade(jumpAnimation,animationPlayTransition);
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
}
