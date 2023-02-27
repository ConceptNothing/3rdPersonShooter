using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchCam : MonoBehaviour
{
    [SerializeField]
    private int priorityBoostAmount=15;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private Canvas thirdPersonCanvas;
    [SerializeField]
    private Canvas aimCanvas;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    private void Awake()
    {
        virtualCamera= GetComponent<CinemachineVirtualCamera>();
        aimAction= playerInput.actions["Aim"]; 
    }
    //Subscribes to event
    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }
    //Unsubscribes to event
    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }
    private void StartAim()
    {
        //Boosting camera priority
        virtualCamera.Priority += priorityBoostAmount;

        aimCanvas.enabled= true;
        thirdPersonCanvas.enabled= false;
    }
    private void CancelAim()
    {
        //Deboosting camera priority
        virtualCamera.Priority -= priorityBoostAmount;

        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
}
